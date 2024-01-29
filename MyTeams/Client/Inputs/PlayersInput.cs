namespace MyTeams.Client.Inputs {
    public struct PlayersInput {
        public int[] PlayersIds { get; set; }
        public PlayersInput(int[] playersIds) {
            PlayersIds = playersIds;
        }
    }
}
