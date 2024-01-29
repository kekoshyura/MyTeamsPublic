using Microsoft.AspNetCore.Mvc;
using MyTeams.Client.Inputs;
using MyTeamsCore.Common;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace MyTeams.Server.Data;

public class DataContext : DbContext {
    public DbSet<Player> Players { get; set; }
    public DbSet<TeamPlayer> TeamPlayers { get; set; }
    public DbSet<DbTeam> Teams { get; set; }

    public DbSet<DbMatchDay> MatchDays { get; set; }

    public DbSet<DbMatch> Matches { get; set; }
    public DbSet<DbMatchGoal> Goals { get; set; }
    public DbSet<DbLog> Logs { get; set; }

    public DataContext(DbContextOptions options) : base(options) {
        Database.EnsureCreated();
    }

    public DataContext() {

    }

    public async Task<DatabaseCache>
    GetDatabaseCache() {
        var players = Players.ToImmutableList();
        var teams = Teams.ToImmutableList();
        var matchDays = MatchDays.ToImmutableList();
        var teamPlayers = TeamPlayers.ToImmutableList();
        var matches = Matches.ToImmutableList();
        var goals = Goals.ToImmutableList();
        return new DatabaseCache(
            players: players,
            teamPlayers: teamPlayers,
            teams: teams,
            matchDays: matchDays,
            matches: matches,
            goals: goals);
    }


    public Player
    GetPlayer(int playerId) {
        var result = Players.Find(playerId);
        if (result == null)
            throw new Exception("Player ot found");
        return result;
    }

    public bool
    TryGetPlayer(int playerId, [NotNullWhen(true)] out Player? result) {
        result = Players.Find(playerId);

        return result != null;
    }

    public int
    GetNewTeamId() =>
        Teams.Count();

    public async Task<DbMatchDay>
    GetCurrentMatchDay() {

        var result = MatchDays.Where(matchDay => !matchDay.IsFinished).ToListAsync().Result;
        if (result.Count != 0) {
            return result[0];
        }
        var matchDay = new DbMatchDay();
        MatchDays.Add(matchDay);

        await SaveChangesAsync();
        return matchDay;
    }

    public async Task<DbMatchDay>
    GetLastMatchDayDb() {
        var lastId = MatchDays.Max(item => item.Id);
        var result = MatchDays.Find(lastId);
        return result;
    }

    public async Task<List<DbMatch>>
    GetCurrentMatchDayMatches() {

        var matchDay = await GetCurrentMatchDay();
        var matches = await GetMatchDayMatches(matchDay);
        return matches;
    }
    public async Task<DbMatch>
        GetMatchBeforeLastDb() {
        var currentMatchDate = await GetCurrentMatchDayMatches();
        var lastIndex = currentMatchDate.Count;
        var res = currentMatchDate.Where(x => x.Equals(currentMatchDate[lastIndex - 2]));
        var result = res.FirstOrDefault();
        if (result == null)
            throw new NullReferenceException();
        return result;
    }

    public async Task<List<DbMatch>>
    GetMatchDayMatches(DbMatchDay dbMatchDay) {
        var matches = await Matches.Where(match => dbMatchDay.Id == match.MatchDayId).ToListAsync();
        return matches;
    }

    public async Task<List<DbMatchDay>>
    GetMatchDaysDb() {
        var result = await MatchDays.ToListAsync();
        return result;
    }

    public async Task<DbMatch>
    GetLastMatchDb() {
        var lastId = Matches.Max(item => item.Id);
        var result = Matches.Find(lastId);
        return result;
    }

    public List<DbTeam>
    GetMatchDayTeams(int matchDayId) {
        var teams = Teams.Where(item => item.MatchDayId == matchDayId).ToListAsync().Result;
        return teams;
    }

    public List<DbTeam>
    GetCurrentMatchDayTeams() {
        var matchDay = GetCurrentMatchDay().Result;
        var teams = Teams.Where(item => item.MatchDayId == matchDay.Id).ToListAsync().Result;
        return teams;
    }
    public async Task<ActionResult<List<Team>>>
    GetCurrentTeams() {
        var dbTeams = await Teams.ToListAsync();
        var currentMatchDayTeams = GetCurrentMatchDayTeams();
        var resultTeams = dbTeams.Where(db => currentMatchDayTeams.Any(x => x.Id == db.Id)).MapToList(dbTeam => {
            var dbPlayers = TeamPlayers.Where(player => player.TeamId == dbTeam.Id).ToListAsync().Result;
            return new Team(
                id: dbTeam.Id,
                name: dbTeam.Name,
                subTeams: new List<Team>(),
                players: dbPlayers.MapToList(player => GetPlayer(player.PlayerId))
            );
        });

        return new ActionResult<List<Team>>(value: resultTeams);
    }

    public async Task<ActionResult<List<Team>>>
    GetMatchDayTeams(DbMatchDay dbMatchDay) {
        var dbTeams = await Teams.ToListAsync();
        var currentMatchDayTeams = GetMatchDayTeams(dbMatchDay.Id);
        var resultTeams = dbTeams.Where(db => currentMatchDayTeams.Any(x => x.Id == db.Id)).MapToList(dbTeam => {
            var dbPlayers = TeamPlayers.Where(player => player.TeamId == dbTeam.Id).ToListAsync().Result;
            return new Team(
                id: dbTeam.Id,
                name: dbTeam.Name,
                subTeams: new List<Team>(),
                players: dbPlayers.MapToList(player => GetPlayer(player.PlayerId))
            );
        });

        return new ActionResult<List<Team>>(value: resultTeams);
    }

    public async Task
    FinishMatchDayDb() {

        var result = MatchDays.Where(matchDay => !matchDay.IsFinished).ToListAsync().Result;
        if (result.Count == 0) {
            return;
        }

        var matchDay = result[0];
        result[0].IsFinished = true;
        foreach (var match in Matches.Where(match => match.MatchDayId == matchDay.Id)) {
            match.IsFinished = true;
        }
        await SaveChangesAsync();
        return;
    }

    public async Task<List<DbTeam>>
    AddMatchDay(MatchDayTeamInput[] teams) {
        var matchDay = new DbMatchDay {
            Date = DateTime.Now
        };
        await FinishMatchDayDb();
        MatchDays.Add(matchDay);
        await Log("Match day added");
        await SaveChangesAsync();
        await Log($"Teams count: {teams.Length}");
        var matchDayId = matchDay.Id;
        foreach (var team in teams) {
            await Log($"Team name: {team.TeamName}");
            var dbTeam = new DbTeam(name: team.TeamName, matchDayId: matchDayId);
            Teams.Add(dbTeam);
            await SaveChangesAsync();
            await Log($"Team added");
            var teamId = dbTeam.Id;
            foreach (var player in team.Players) {
                await Log($"trying to add player {player.PlayerId}");
                var dbPlayers = await Players.Where(dbPlayer => dbPlayer.Id == player.PlayerId).ToListAsync();
                var dbPlayer = dbPlayers.Count > 0 ? dbPlayers[0] : null;
                if (dbPlayers.Count == 0) {
                    dbPlayer = new Player(id: 0, name: player.PlayerName, nickname: player.PlayerNickname, telegramId: player.PlayerTelegramId);
                    Players.Add(dbPlayer);
                    await SaveChangesAsync();
                }
                var playerId = dbPlayer.Id;
                TeamPlayers.Add(new TeamPlayer(id: 0, teamId: teamId, playerId: playerId));
                await Log($"player {player.PlayerName} added");
                await SaveChangesAsync();

            }
        }
        List<DbTeam> matchDayTeams = new();
        if (teams.Length == 2) {
            matchDayTeams = Teams.Where(x => x.MatchDayId == matchDayId).ToList();
        }


        await SaveChangesAsync();
        return matchDayTeams;
    }

    public async Task<List<Team>> AddTeamsToExistMatchDay(MatchDayTeamInput[] teams, int matchDayId) {

        List<Team> matchDayTeams = new();
        foreach (var team in teams) {
            List<Player> playersToTeam = new();
            var dbTeam = new DbTeam(name: team.TeamName, matchDayId: matchDayId);
            Teams.Add(dbTeam);
            await SaveChangesAsync();
            var teamId = dbTeam.Id;
            foreach (var player in team.Players) {
                var dbPlayers = await Players.Where(dbPlayer => dbPlayer.Id == player.PlayerId).ToListAsync();
                var dbPlayer = dbPlayers.Count > 0 ? dbPlayers[0] : null;
                if (dbPlayers.Count == 0) {
                    dbPlayer = new Player(id: 0, name: player.PlayerName, nickname: player.PlayerNickname, telegramId: player.PlayerTelegramId);
                    Players.Add(dbPlayer);
                    playersToTeam.Add(dbPlayer);
                    await SaveChangesAsync();
                }
                var playerId = dbPlayer.Id;
                TeamPlayers.Add(new TeamPlayer(id: 0, teamId: teamId, playerId: playerId));
                playersToTeam.Add(dbPlayer);
                await SaveChangesAsync();

            }
            Team createdTeam = new(id: teamId, name: dbTeam.Name, players: playersToTeam, subTeams: new List<Team>());
            matchDayTeams.Add(createdTeam);
        }
        await SaveChangesAsync();
        return matchDayTeams;
    }

    public async Task
    AddMatchDay(BotMatchDayTeam[] teams) {
        var matchDay = new DbMatchDay {
            Date = DateTime.Now
        };
        await FinishMatchDayDb();
        MatchDays.Add(matchDay);
        await Log("Match day added");
        await SaveChangesAsync();
        await Log($"Teams count: {teams.Length}");
        var matchDayId = matchDay.Id;

        foreach (var botTeam in teams) {
            await Log($"Team name: {botTeam.Name}");
            var dbTeam = new DbTeam(name: botTeam.Name, matchDayId: matchDayId);
            Teams.Add(dbTeam);
            await SaveChangesAsync();
            await Log($"Team added");
            var teamId = dbTeam.Id;
            foreach (var player in botTeam.players) {
                await Log($"trying to add player {player.name}");
                var dbPlayers = await Players.Where(dbPlayer => dbPlayer.TelegramId == player.telegramId).ToListAsync();
                var dbPlayer = dbPlayers.Count > 0 ? dbPlayers[0] : null;
                if (dbPlayers.Count == 0) {
                    dbPlayer = new Player(id: 0, name: player.name, nickname: player.nickname, telegramId: player.telegramId);
                    Players.Add(dbPlayer);
                    await SaveChangesAsync();
                }
                var playerId = dbPlayer.Id;
                TeamPlayers.Add(new TeamPlayer(id: 0, teamId: teamId, playerId: playerId));
                await Log($"player {player.name} added");
                await SaveChangesAsync();

            }
        }
        await SaveChangesAsync();
    }




    public async Task
    Log(string message) {
        Logs.Add(new DbLog(0, message));
        await SaveChangesAsync();
    }

    public async Task
    AddGoal(DbMatchGoal goal) {
        Goals.Add(goal);
        await SaveChangesAsync();
    }

    public async Task<BotPlayerRatings>
    GetRatings(BotPlayers botPlayers) {
        var builder = new PlayerStatsBuilder(new Dictionary<int, PlayerStats>(), botPlayers.PlayerTelegramIds.ToHashSet());
        var matches = Matches.ToListAsync().Result;
        var teams = Teams.ToListAsync().Result;
        var players = Players.ToListAsync().Result;
        var teamPlayers = TeamPlayers.ToListAsync().Result;
        var teamsById = teams.ToDictionary(x => x.Id, x => x);
        foreach (var match in matches)
            builder.HandleMatch(GetMatch(match, teamsById, players, teamPlayers));
        var stats = builder.StatsById.DistinctBy(x => x.Value.Player.TelegramId).ToDictionary(x => x.Value.Player.TelegramId, x => x.Value.WinRate);
        var average = stats.Values.Average();
        foreach (var newId in botPlayers.PlayerTelegramIds.Where(id => !stats.ContainsKey(id))) {
            stats.Add(newId, average);
        }
        return new BotPlayerRatings(stats);
    }

    public Match
    GetMatch(DbMatch dbMatch, Dictionary<int, DbTeam> teams, List<Player> players, List<TeamPlayer> teamPlayers) {
        var teamA = teams[dbMatch.ATeamId];
        var teamB = teams[dbMatch.BTeamId];
        if (teamA == null || teamB == null)
            throw new InvalidOperationException("Team not found");
        var homeTeam = GetTeam(teamA, new List<DbTeam>(), players, teamPlayers);
        var awayTeam = GetTeam(teamB, new List<DbTeam>(), players, teamPlayers);
        return new Match(
            id: dbMatch.Id,
            matchDayId: dbMatch.MatchDayId,
            homeTeam: homeTeam,
            awayTeam: awayTeam,
            score: GetMatchScore(dbMatch),
            isFinished: dbMatch.IsFinished
            );
    }

    public Team
    GetTeam(DbTeam dbTeam, List<DbTeam> subTeams, List<Player> players, List<TeamPlayer> allTeamPlayers) {
        var teamPlayers = allTeamPlayers.Where(player => player.TeamId == dbTeam.Id).Select(x => x.PlayerId).ToHashSet();

        return new Team(
            id: dbTeam.Id,
            name: dbTeam.Name,
            subTeams: new List<Team>(),// subTeams.Where(x => x.ParentTeamId == dbTeam.Id).MapToList(t => GetTeam(t, new List<DbTeam>())),
            players: players.Where(player => teamPlayers.Contains(player.Id)).ToList()
        );
    }

    public static Score
    GetMatchScore(DbMatch dbMatch) {

        var homeGoals = new List<Goal>();
        var awayGoals = new List<Goal>();

        while (homeGoals.Count < dbMatch.AGoals) {
            homeGoals.Add(new Goal());
        }
        while (awayGoals.Count < dbMatch.BGoals) {
            awayGoals.Add(new Goal());
        }
        return new Score(homeGoals, awayGoals);

    }
}