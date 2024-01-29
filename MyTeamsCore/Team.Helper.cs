using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeamsCore;
public static class 
TeamHelper {

    public static string
    GetTeamColor(this Team team) =>
        team.Name switch {
            "Red" => "team-red",
            "Green" => "team-green",
            _ => "team-blue"
        };
}