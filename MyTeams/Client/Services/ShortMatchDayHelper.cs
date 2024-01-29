using MyTeamsCore;

namespace MyTeams.Client.Services {
    public static class ShortMatchDayHelper {
        public static Team CreateTeam(this List<Player> players, int teamsCount) {
            Team teamToReturn = new();
            List<Player> availablePlayers = players.ToList();
            Random random = new Random();
            for (int j = 0; j < 5; j++) {
                int randomIndex = random.Next(0, availablePlayers.Count);
                Player selectedPlayer = availablePlayers[randomIndex];
                teamToReturn.Players.Add(selectedPlayer);
                availablePlayers.RemoveAt(randomIndex);
            }

            teamToReturn.Name = "tryryr";
            return teamToReturn;
        }
    }
}
