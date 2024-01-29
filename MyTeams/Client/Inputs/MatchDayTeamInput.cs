namespace MyTeams.Client.Inputs {
    public struct MatchDayTeamInput {
        public string TeamName { get; set; }
        public MatchDayTeamPlayerInput[] Players { get; set; }
        public MatchDayTeamInput(string teamName, MatchDayTeamPlayerInput[] players) {
            TeamName = teamName;
            Players = players;
        }
    }
}
