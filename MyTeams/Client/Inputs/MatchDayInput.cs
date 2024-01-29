namespace MyTeams.Client.Inputs {
    public struct MatchDayInput {
        public MatchDayTeamInput[] Teams { get; set; }
        public MatchDayInput(MatchDayTeamInput[] teams) {
            Teams = teams;
        }
    }
}
