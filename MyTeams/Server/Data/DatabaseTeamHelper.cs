using MyTeams.Client.Messages;
using MyTeamsCore.Common;

namespace MyTeams.Server.Data {


    public static class DatabaseTeamHelper {

        public static async Task<Message>
        GetDatabaseTeam(this DataContext dataContext, GetDatabaseTeamMessage message) {

            var dbTeam = await dataContext.Teams.FindAsync(message.TeamId);

            var dbPlayers = dataContext.TeamPlayers.Where(player => player.TeamId == dbTeam.Id).ToListAsync().Result;
            var team = new Team(
                id: dbTeam.Id,
                name: dbTeam.Name,
                subTeams: new List<Team>(),
                players: dbPlayers.MapToList(player => dataContext.GetPlayer(player.PlayerId))
            );

            var result = new TeamMessage(team).ToMessage();


            return result;
        }



        public static async Task<Message>
        TeamPlayerAddRemove(this DataContext dataContext, TeamPlayerAddRemoveMessage message) {
            var player = dataContext.TeamPlayers.Where(x => x.TeamId == message.TeamId && x.PlayerId == message.RemovePlayer.Id).ToList();
            if (player == null)
                throw new NullReferenceException();
            dataContext.TeamPlayers.Remove(player.FirstOrDefault());
            var teamPlayer = new TeamPlayer(message.TeamId, message.AddPlayer.Id);
            dataContext.TeamPlayers.Add(teamPlayer);
            await dataContext.SaveChangesAsync();
            var result = dataContext.TeamPlayers.Find(teamPlayer.Id);
            var mes = new TeamPlayerMessage(teamPlayer).ToMessage();
            return mes;
        }

        public static async Task<Message>
        GetTeamPlayer(this DataContext dataContext, GetTeamPlayerMessage message) {
            var player = dataContext.TeamPlayers.Find(message.TeamPlayerId);
            var result = new TeamPlayerMessage(player).ToMessage();
            return result;
        }

        public static async Task<Message>
        RenameTeam(this DataContext dataContext, RenameTeamMessage message) {
            var dbTeam = dataContext.Teams.Find(message.Team.Id);
            if (dbTeam != null)
                dbTeam.Name = message.Team.Name;
            await dataContext.SaveChangesAsync();
            return OkMessage.Instance.ToMessage();
        }

        public static async Task<Message>
        RemoveTeam(this DataContext dataContext, RemoveTeamMessage message) {
            foreach (var player in message.Team.Players) {
                var teamPlayer = await dataContext.TeamPlayers.Where(x => x.PlayerId == player.Id && x.TeamId == message.Team.Id).ToListAsync();

                dataContext.TeamPlayers.Remove(teamPlayer[0]);
            }
            var dbTeam = dataContext.Teams.Find(message.Team.Id);
            if (dbTeam != null)
                dataContext.Teams.Remove(dbTeam);
            await dataContext.SaveChangesAsync();
            return OkMessage.Instance.ToMessage();
        }

        public static async Task<Message>
        AddTeam(this DataContext dataContext, AddTeamMessage message) {
            var newTeam = new DbTeam(message.Team.TeamName, message.Team.MatchDayId);
            newTeam.ParentTeamId = message.Team.ParentTeamId;
            dataContext.Teams.Add(newTeam);
            await dataContext.SaveChangesAsync();
            foreach (var playerId in message.Team.PlayerIds) {
                var player = dataContext.GetPlayer(playerId);
                dataContext.TeamPlayers.Add(new TeamPlayer(id: 0, teamId: newTeam.Id, playerId: player.Id));
            }
            await dataContext.SaveChangesAsync();
            var res = dataContext.Teams.Find(newTeam.Id);

            if (res == null) {
                throw new NullReferenceException();
            }
            var result = new DbTeamMessage(res).ToMessage();
            return result;
        }

        public static async Task<Message>
        GetDatabaseTeams(this DataContext dataContext) {

            var dbTeams = await dataContext.Teams.ToListAsync();
            var subTeams = dbTeams.Where(x => x.IsSubTeam).ToList();
            var resultTeams = GetTeamsAndSubTeams(dbTeams, subTeams).ToList();
            var message = new TeamsMessage(resultTeams).ToMessage();

            return message;

            IEnumerable<Team>
            GetTeamsAndSubTeams(List<DbTeam> teams, List<DbTeam> subTeams) {
                foreach (var dbTeam in teams) {
                    var dbPlayers = dataContext.TeamPlayers.Where(player => player.TeamId == dbTeam.Id).ToListAsync().Result;
                    var players = new List<Player>();
                    foreach (var dbPlayer in dbPlayers) {
                        if (!dataContext.TryGetPlayer(dbPlayer.PlayerId, out var player))
                            continue;
                        players.Add(player);
                    }
                    if (players.Count == 0)
                        continue;

                    yield return new Team(
                        id: dbTeam.Id,
                        name: dbTeam.Name,
                        subTeams: subTeams.Where(x => x.ParentTeamId == dbTeam.Id).MapToList(subTeam => {
                            var dbSubPlayers = dataContext.TeamPlayers.Where(player => player.TeamId == subTeam.Id).ToListAsync().Result;
                            return new Team(
                                id: subTeam.Id,
                                name: subTeam.Name,
                                subTeams: new List<Team>(),
                                players: dbSubPlayers.MapToList(player => dataContext.GetPlayer(player.PlayerId)));
                        }
                        ),
                        players: players
                    );
                }
            }
        }
    }
}
