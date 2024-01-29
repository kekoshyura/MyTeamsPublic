using MyTeams.Client.Dialogs;
using MyTeams.Client.Messages;
using MyTeamsCore;
using MyTeamsCore.Common;
using System.Collections.Immutable;
using System.Net.Http.Json;

namespace MyTeams.Client;

public static class AppHelper {
    public static AppView ViewState { get; set; }

    public static object _lockObject = new object();

    public static bool _isLoading = false;

    public static App
    LoadLastMatchDay(this App app) {
        Task.Run(async () => {
            var databaseCache = await app.GetDatabaseCache();

            new AppCommand(app => {
                app.DatabaseCache = databaseCache;
                app = app with { LastMatchDay = app.GetMatchDay(app.LastDbMatchDay.Id) };
                return app;
            }).Dispatch();
        });

        return app;
    }

    public static App
    SwitchPlayerInTeam(this App app, Player addPlayer, Player removePlayer, int teamId) {
        Task.Run(async () => {
            var message = await app.Client.SendAndGetMessage(new TeamPlayerAddRemoveMessage(addPlayer, removePlayer, teamId));
            var teamPlayerMessage = message.ReadMessage<TeamPlayerMessage>();
            new AppCommand(app => app.SwitchPlayerInTeam(removePlayer, teamPlayerMessage.TeamPlayer)).Dispatch();
        });
        return app;
    }

    public static App
    BeginAddingPlayer(this App app, Player player) {
        Task.Run(async () => {
            var message = await app.Client.SendAndGetMessage(new AddPlayerMessage(player));
            var addedPlayer = message.ReadMessage<PlayerMessage>();
            new AppCommand(app => app.AddPlayerToCache(addedPlayer.Player)).Dispatch();
        });
        return app;
    }


    public static App
    AddPlayerToCache(this App app, Player player) {
        var newPlayersList = app.DatabaseCache.Players.Add(player);
        var newDatabaseCache = app.DatabaseCache with { Players = newPlayersList };
        return app with { DatabaseCache = newDatabaseCache };
    }

    public static App
    SwitchPlayerInTeam(this App app, Player removePlayer, TeamPlayer playerToAdd) {
        if (!app.DatabaseCache.TeamPlayers.TryGet(x => x.PlayerId == removePlayer.Id, out var teamPlayer1))
            throw new InvalidOperationException($"Player with id {removePlayer.Id} not found in database cache");
        var newTeamPlayersList = app.DatabaseCache.TeamPlayers;
        var index = newTeamPlayersList.IndexOf(teamPlayer1);
        newTeamPlayersList = newTeamPlayersList.SetItem(index, playerToAdd);
        var newDbCache = app.DatabaseCache with { TeamPlayers = newTeamPlayersList };
        app = app with {
            DatabaseCache = newDbCache
        };
        app = app with {
            LastMatchDay = app.GetMatchDay(app.LastDbMatchDay.Id),
        };
        return app;
    }

    public static ImmutableList<Player>
    GetPlayersFromCache(this App app) {
        return app.DatabaseCache.Players;
    }

    public static IEnumerable<Team>
    GetLastMatchDayTeams(this App app) =>
        app.GetMatchDayTeams(app.LastDbMatchDay.Id);

    public static IEnumerable<Team>
    GetMatchDayTeams(this App app, int matchDayId) {
        var lastDbTeams = app.Teams.Where(team => team.MatchDayId == matchDayId).ToList();
        var subTeams = lastDbTeams.Where(x => x.IsSubTeam).ToList();
        return lastDbTeams.Where(x => !x.IsSubTeam).Select(x => app.GetTeam(x, subTeams));
    }

    public static Team
    GetTeam(this App app, DbTeam dbTeam, List<DbTeam> subTeams) {
        var teamPlayers = app.DatabaseCache.TeamPlayers.Where(player => player.TeamId == dbTeam.Id).Select(x => x.PlayerId).ToHashSet();

        return new Team(
            id: dbTeam.Id,
            name: dbTeam.Name,
            subTeams: subTeams.Where(x => x.ParentTeamId == dbTeam.Id).MapToList(t => app.GetTeam(t, new List<DbTeam>())),
            players: app.DatabaseCache.Players.Where(player => teamPlayers.Contains(player.Id)).ToList()
        );
    }

    public static List<Match>
    GetLastMatchDayMatches(this App app) {
        var matches = app.GetMatchDayMatches(app.LastDbMatchDay).ToList();
        return matches;
    }


    public static IEnumerable<Match>
    GetMatchDayMatches(this App app, DbMatchDay matchDay) {
        var dbMatches = app.Matches.Where(match => match.MatchDayId == matchDay.Id);
        return dbMatches.Select(x => app.GetMatch(x));
    }

    public static Match
    GetMatch(this App app, DbMatch dbMatch) {
        var teamA = app.Teams.Find(team => team.Id == dbMatch.ATeamId);
        var teamB = app.Teams.Find(team => team.Id == dbMatch.BTeamId);
        if (teamA == null || teamB == null)
            throw new InvalidOperationException("Team not found");
        var homeTeam = app.GetTeam(teamA, new List<DbTeam>());
        var awayTeam = app.GetTeam(teamB, new List<DbTeam>());
        return new Match(
            id: dbMatch.Id,
            matchDayId: dbMatch.MatchDayId,
            homeTeam: homeTeam,
            awayTeam: awayTeam,
            score: app.GetMatchScore(dbMatch.Id, homeTeam, awayTeam, dbMatch),
            isFinished: dbMatch.IsFinished
            );
    }

    public static Score
    GetMatchScore(this App app, int matchId, Team homeTeam, Team awayTeam, DbMatch dbMatch) {
        var goals = app.Goals.Where(goal => goal.MatchId == matchId).ToList();
        var homeGoals = new List<Goal>();
        var awayGoals = new List<Goal>();
        foreach (var goal in goals) {
            if (homeTeam.Players.TryGet(player => player.Id == goal.PlayerGoalId, out var goalPlayer))
                homeGoals.Add(new Goal(
                    id: goal.Id,
                    player: goalPlayer,
                    assist: goal.PlayerPassId == 0 ? null : homeTeam.Players.TryGet(x => x.Id == goal.PlayerPassId, out var passPlayer) ? passPlayer : throw new InvalidOperationException("Pass player not found")));
            else if (awayTeam.Players.TryGet(player => player.Id == goal.PlayerGoalId, out goalPlayer)) {
                awayGoals.Add(new Goal(
                    id: goal.Id,
                    player: goalPlayer,
                    assist: goal.PlayerPassId == 0 ? null : homeTeam.Players.TryGet(x => x.Id == goal.PlayerPassId, out var passPlayer) ? passPlayer : throw new InvalidOperationException("Pass player not found")));
            }
        }
        while (homeGoals.Count < dbMatch.AGoals) {
            homeGoals.Add(new Goal());
        }
        while (awayGoals.Count < dbMatch.BGoals) {
            awayGoals.Add(new Goal());
        }
        return new Score(homeGoals, awayGoals);

    }

    public static App
    ReplaceLastMatchDay(this App app, MatchDay newMatchDay) =>
        app with { LastMatchDay = newMatchDay };

    public static MatchDay
    GetMatchDay(this App app, int id) {
        var matchDay = app.DatabaseCache.MatchDays.GetOrThrow(x => x.Id == id);
        var matches = app.GetMatchDayMatches(matchDay).ToList();
        var teams = app.GetMatchDayTeams(matchDay.Id).ToList();
        return new MatchDay(id, matches, teams);

    }

    public static MatchDay
    AddMatch(this MatchDay day, Match match) {
        day.Matches.Add(match);
        return new MatchDay(day.Id, day.Matches, day.Teams);

    }

    public static List<PlayerStats>
    GetPlayerStatsUntil(this App app, MatchDay? matchDay = null) {
        var builder = PlayerStatsBuilder.Default;
        foreach (var match in app.Matches.Where(match => matchDay == null || matchDay.Id > match.MatchDayId))
            builder.HandleMatch(app.GetMatch(match));
        return builder.StatsById.Select(x => x.Value).ToList();
    }

    public static List<PlayerStats>
    GetPlayersStatsByMatchDay(this App app, MatchDay matchDay) {
        var builder = PlayerStatsBuilder.Default;
        if (app.Matches.TryGet(x => x.MatchDayId == matchDay.Id, out var match))
            builder.HandleMatch(app.GetMatch(match));
        return builder.StatsById.Select(x => x.Value).ToList();
    }

    public static List<PlayerStats>
    GetPlayerStatsSeason(this App app, PlayersStatsSeason season) {

        var builder = PlayerStatsBuilder.Default;
        foreach (var match in app.Matches.Where(match => match.HisSeaasoon(season)))
            builder.HandleMatch(app.GetMatch(match));

        return builder.StatsById.Select(x => x.Value).ToList();
    }

    public static bool
    HisSeaasoon(this DbMatch match, PlayersStatsSeason season) =>
        season switch {
            PlayersStatsSeason.Season1 => match.MatchDayId < 5,
            PlayersStatsSeason.Season2 => match.MatchDayId > 4,
            _ => true
        };

    public static App
    HandleMatchResult(this App app, DbMatch match) {
        if (!app.Matches.TryGet(x => x.Id == match.Id, out var dbMatch))
            throw new InvalidOperationException("Match not found");
        var newMatchesList = app.Matches;
        var index = app.Matches.IndexOf(dbMatch);
        newMatchesList = newMatchesList.SetItem(index, match);
        var newDbCache = app.DatabaseCache with { Matches = newMatchesList };
        app = app with { DatabaseCache = newDbCache };
        app = app with { LastMatchDay = app.GetMatchDay(app.LastMatchDay.Id) };
        return app;
    }

    public static App
    HandleMatchMessage(this App app, string message) {
        app = app with { Dialog = new MessageDialog(message) };
        return app;
    }



    public static App
    CloseDialog(this App app) {
        app = app with { Dialog = null };
        return app;
    }

    public static App
    OpenTeamPlayersDialog(this App app, Team team, MatchDay matchDay) {
        return app;
        var stats = app.GetTeamStats(team, matchDay);
        var dialog = new TeamPlayersDialog(team, stats);
        app = app with { Dialog = dialog };
        //app.Dialog = dialog;
        return app;
    }

    public static TeamStats
    GetTeamStats(this App app, Team team, MatchDay matchDay) {
        var playerStats = app.GetPlayerStatsUntil(matchDay);
        return new TeamStats(playerStats.Where(player => team.Players.Any(x => x.Id == player.Player.Id)).ToList());
    }

    public static App
    EditMatch(this App app, Match match, bool canEditScore) {
        app = app with { MatchEditor = new MatchEditor(match, canEditScore) };
        return app;
    }



    public static App
    ShowMessage(this App app, string message) {
        app = app with { Dialog = new MessageDialog(message) };
        return app;
    }



    public static async Task<App>
    LoadPlayersFromDatabase(this App app) {
        var message = await app.Client.SendMessage<GetPlayersMessage>();
        var result = message.ReadMessage<PlayersMessage>();
        var newDatabaseCache = app.DatabaseCache with { Players = result.Players };
        app = app with { DatabaseCache = newDatabaseCache };
        return app;
    }

    public static async Task<DatabaseCache>
    GetDatabaseCache(this App app) {
        var res = await app.Client.SendMessage<GetDatabaseCacheMessage>();
        var cache = res.ReadMessage<DatabaseCacheMessage>();
        return cache.DatabaseCache;
    }

    public static async Task<App>
    ReplaceMatchTeams(this App app, DbMatch dbMatch) {
        var result = await app.Client.PostAsJsonAsync("api/message", new ReplaceMatchTeamsMessage(dbMatch));
        if (result == null)
            throw new NullReferenceException();
        return app;
    }

    public static async Task<ImmutableList<Player>>
    GetPlayers(this App app) {
        var result = await app.Client.SendMessage<GetPlayersMessage>();
        var playersMessage = result.ReadMessage<PlayersMessage>();
        return playersMessage.Players;
    }

    public static async Task<ImmutableList<Player>>
    GetFreePlayers(this App app) {
        var result = await app.Client.SendMessage<GetFreePlayersMessage>();
        var freePlayers = result.ReadMessage<PlayersMessage>();
        return freePlayers.Players;
    }

    public static async Task<List<Team>>
    GetTeams(this App app) {
        var result = await app.Client.SendMessage<GetDatabaseTeamsMessage>();
        var teams = result.ReadMessage<TeamsMessage>();
        if (teams == null)
            throw new NullReferenceException();
        return teams.Teams;
    }



    public static App
    DeleteMatch(this App app, Match match) {

        if (app.DatabaseCache.Matches.TryGet(x => x.Id == match.Id, out var matchAtCache)) {
            var newMatchsList = app.DatabaseCache.Matches.Remove(matchAtCache);
            var newDbCache = app.DatabaseCache with { Matches = newMatchsList };
            app = app with { DatabaseCache = newDbCache };
        }

        app = app with { LastMatchDay = app.GetMatchDay(app.LastDbMatchDay.Id) };
        return app;
    }

    public static App
    StartMatch(this App app, DbMatch match) {
        var newMatchsList = app.DatabaseCache.Matches.Add(match);
        var newDbCache = app.DatabaseCache with { Matches = newMatchsList };
        app = app with { DatabaseCache = newDbCache };
        app = app with { LastMatchDay = app.LastMatchDay?.AddMatch(app.GetMatch(match) ?? throw new InvalidOperationException("Last match day was null")) };
        return app;
    }


    public static App
    AddTeamWithPlayersToMatch(this App app, Team teamToAdd) {
        var dbTeam = new DbTeam(teamToAdd.Id, app.LastDbMatchDay.Id, teamToAdd.Name);
        var newTeamsList = app.DatabaseCache.Teams.Add(dbTeam);

        var playersToAdd = new List<TeamPlayer>();
        foreach (var player in teamToAdd.Players) {
            var teamPlayer = new TeamPlayer(teamId: teamToAdd.Id, playerId: player.Id);
            playersToAdd.Add(teamPlayer);
        }
        var newTeamPlayersList = app.DatabaseCache.TeamPlayers.AddRange(playersToAdd);
        var newDbCache = app.DatabaseCache with { Teams = newTeamsList, TeamPlayers = newTeamPlayersList };
        app = app with { DatabaseCache = newDbCache };
        return app;
    }


    public static App
    UpdatePlayersReport(this App app, ImmutableList<PlayerStats> stats, PlayersStatsSeason season, ImmutableList<PlayerReportColumns> columns) {
        app = app with { Report = new PLayersReport(stats, season, columns) };
        return app;
    }

    public static App
    UpdatePlayersReportWithSeason(this App app, ImmutableList<PlayerStats> stats, PlayersStatsSeason season) {
        var newReport = app.Report with { Stats = stats, PlayersStatsSeason = season };
        app = app with { Report = newReport };
        return app;

    }
    public class BotPlayers {
        public int[] PlayerTelegramIds { get; set; }
        public BotPlayers(int[] playerTelegramIds) {
            PlayerTelegramIds = playerTelegramIds;
        }
    }
    public class BotMatchDay {
        public BotMatchDayTeam[] Teams { get; set; }

        public BotMatchDay(BotMatchDayTeam[] teams) {
            Teams = teams;
        }
    }

    public class BotMatchDayTeam {
        public string Name { get; set; }
        public BotMatchDayTeamPlayer[] players { get; set; }

        public BotMatchDayTeam(string name, BotMatchDayTeamPlayer[] players) {
            Name = name;
            this.players = players;
        }
    }

    public class BotMatchDayTeamPlayer {
        public string name { get; set; }
        public string nickname { get; set; }
        public int telegramId { get; set; }

        public BotMatchDayTeamPlayer(string name, string nickname, int telegramId) {
            this.name = name;
            this.nickname = nickname;
            this.telegramId = telegramId;
        }
    }
}


