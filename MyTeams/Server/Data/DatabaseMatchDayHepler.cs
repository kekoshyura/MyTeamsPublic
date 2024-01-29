using MyTeams.Client.Messages;
using System.Text.Json;

namespace MyTeams.Server.Data {
    public static class DatabaseMatchDayHepler {

        public static async Task<Message>
        GetMatchDay(this DataContext dataContext) {
            var lastMatchDay = await dataContext.GetLastMatchDayDb();
            var result = await dataContext.GetMatchDayMatches(lastMatchDay);
            var teams = await dataContext.GetMatchDayTeams(lastMatchDay);
            if (teams.Value == null)
                throw new Exception(); //return NoContent()
            var tableTeams = teams.Value.GetSortedTeams(result).OrderByDescending(x => x.Points).ToList();
            var message = new MatchResultMessage(tableTeams, result).ToMessage();
            return message;
        }

        public static async Task<Message>
        Teams(this DataContext dataContext) {
            var lastMatchDay = await dataContext.GetLastMatchDayDb();
            var teams = await dataContext.GetMatchDayTeams(lastMatchDay);
            if (teams.Value == null)
                throw new Exception(); //return NoContent()
            var message = new TeamsMessage(teams.Value).ToMessage();
            return message;
        }

        public static async Task<Message>
        AddNewMatchDay(this DataContext dataContext, AddMatchDayMessage message) {
            //var result = JsonSerializer.Serialize(message.MatchDay);
            var teams = await dataContext.AddMatchDay(message.MatchDay.Teams);
            var result = new DbTeamsMessage(teams).ToMessage();
            return result;
        }


        public static async Task<Message>
        AddTeamsToMatchDay(this DataContext dataContext, AddTeamsToExistMatchDayMessage message) {
            var teams = await dataContext.AddTeamsToExistMatchDay(message.Teams, message.MatchDayId);
            var result = new TeamsMessage(teams).ToMessage();
            return result;

        }

        public static async Task<Message>
        GetDatabaseCache1(this DataContext dataContext) {
            var result = await dataContext.GetDatabaseCache();
            if (result == null)
                throw new Exception(); //return NoContent()
            var r = JsonSerializer.Serialize(result);
            var message = new DatabaseCacheMessage(result).ToMessage();
            return message;
        }
    }
}
