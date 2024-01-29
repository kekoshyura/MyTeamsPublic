using MyTeams.Client.Messages;
using MyTeamsCore.Common;

namespace MyTeams.Server.Data {
    public static class DatabaseMatchHelper {

        public static async Task<Message>
        GetMatches(this DataContext dataContext) {
            var result = await dataContext.GetCurrentMatchDayMatches();
            var message = new MatchesMessage(result).ToMessage();
            return message;
        }


        public static async Task<Message>
        GetLastMatchDayMatches(this DataContext dataContext) {
            var lastDay = await dataContext.GetLastMatchDayDb();
            var result = await dataContext.GetMatchDayMatches(lastDay);
            var message = new MatchesMessage(result).ToMessage();
            return message;
        }


        public static async Task<Message>
        GetLastMatchDay(this DataContext dataContext) {
            var lastDay = await dataContext.GetLastMatchDayDb();
            var message = new MatchDayMessage(lastDay).ToMessage();
            return message;
        }

        public static async Task<Message>
        GetMatchDays(this DataContext dataContext) {
            var result = await dataContext.GetMatchDaysDb();
            var message = new MatchDaysMessage(result).ToMessage();
            return message;
        }

        public static async Task<Message>
        GetLastMatch(this DataContext dataContext) {
            var result = await dataContext.GetLastMatchDb();
            var message = new MatchMessage(result).ToMessage();
            return message;
        }

        public static async Task<Message>
        AddMatch(this DataContext dataContext, AddMatchMessage message) {
            var currentMatchDate = dataContext.GetCurrentMatchDay().Result;
            var match = new DbMatch(
                id: 0,
                matchDayId: currentMatchDate.Id,
                aTeamId: message.Teams[0],
                bTeamId: message.Teams[1],
                aGoals: 0,
                bGoals: 0,
                isFinished: false);
            var matches = await dataContext.GetCurrentMatchDayMatches();
            if (matches.Count > 1) {
                var lastMatch = await dataContext.GetLastMatchDb();
                var matchBeforeLast = await dataContext.GetMatchBeforeLastDb();
                if (lastMatch.MatchDayId == currentMatchDate.Id) {
                    if (lastMatch.HasTeam(message.Teams[0]) && matchBeforeLast.HasTeam(message.Teams[0])) {
                        return new MatchErrorMessage(IsAddedMessageTypes.Fail, match).ToMessage();
                    }
                    else if (lastMatch.HasTeam(message.Teams[1]) && matchBeforeLast.HasTeam(message.Teams[1])) {
                        return new MatchErrorMessage(IsAddedMessageTypes.Fail, match).ToMessage();
                    }
                }
            }
            dataContext.Matches.Add(match);
            await dataContext.SaveChangesAsync();
            var result = new MatchErrorMessage(IsAddedMessageTypes.Success, match).ToMessage();
            return result;
        }

        public static async Task<Message>
        DeleteMatch(this DataContext dataContext, DeleteMatchMessage message) {
            var dbMatch = dataContext.Matches.Find(message.Match.Id);
            if (dbMatch == null)
                return new MatchErrorMessage(IsAddedMessageTypes.Fail).ToMessage();
            dataContext.Remove(dbMatch);
            await dataContext.SaveChangesAsync();
            var result = new MatchErrorMessage(IsAddedMessageTypes.Success).ToMessage();
            return result;
        }

        public static async Task<Message>
        FinishMatchDay(this DataContext dataContext) {
            await dataContext.FinishMatchDayDb();
            return OkMessage.Instance.ToMessage(); //return Ok(true);
        }


        public static async Task<Message>
        SetHomeTeamMatchResult(this DataContext dataContext, SetHomeTeamMatchResultMessage message) {
            var result = dataContext.Matches.Find(message.Match.Id);
            if (result.AGoals == message.Match.AGoals || result.BGoals != message.Match.BGoals) {
                return new GoalAddedMessage(ResultMessageTypes.Decline).ToMessage();
            }
            else {
                result.AGoals = message.Match.AGoals;
                await dataContext.SaveChangesAsync();
                return new GoalAddedMessage(ResultMessageTypes.Updated).ToMessage();
            }
        }

        public static async Task<Message>
        SetAwayTeamMatchResult(this DataContext dataContext, SetAwayTeamMatchResultMessage message) {
            var result = dataContext.Matches.Find(message.Match.Id);
            if (result.BGoals == message.Match.BGoals || result.AGoals != message.Match.AGoals) {
                return new GoalAddedMessage(ResultMessageTypes.Decline).ToMessage();
            }
            else {
                result.BGoals = message.Match.BGoals;
                await dataContext.SaveChangesAsync();
                return new GoalAddedMessage(ResultMessageTypes.Updated).ToMessage();
            }
        }


        public static async Task<Message>
        ReplaceMatchTeams(this DataContext dataContext, ReplaceMatchTeamsMessage message) {
            var result = dataContext.Matches.Find(message.Match.Id);
            result.ATeamId = message.Match.ATeamId;
            result.BTeamId = message.Match.BTeamId;
            await dataContext.SaveChangesAsync();
            return OkMessage.Instance.ToMessage();  //return Ok(true);
        }

        public static async Task<Message>
        SetGoal(this DataContext dataContext, SetGoalMessage message) {
            var result = dataContext.Goals.Find(message.Goal.Id);

            if (result == null) {
                dataContext.Goals.Add(message.Goal);
                result = message.Goal;
            }
            else {
                result.PlayerGoalId = message.Goal.PlayerGoalId;
                result.PlayerPassId = message.Goal.PlayerPassId;
            }
            await dataContext.SaveChangesAsync();
            return OkMessage.Instance.ToMessage();
        }


    }
}
