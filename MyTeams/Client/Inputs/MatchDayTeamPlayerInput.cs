namespace MyTeams.Client.Inputs {
    public struct MatchDayTeamPlayerInput {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string PlayerNickname { get; set; }
        public int PlayerTelegramId { get; set; }
        public MatchDayTeamPlayerInput(int playersId, string playerName, string playerNickname, int playerTelegramId) {
            PlayerId = playersId;
            PlayerName = playerName;
            PlayerNickname = playerNickname;
            PlayerTelegramId = playerTelegramId;
        }
    }
}
