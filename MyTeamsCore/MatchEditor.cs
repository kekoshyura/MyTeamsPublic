using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeamsCore;
public class MatchEditor{

    public Match Match {get; set;}
    public bool CanEditScore {get;}

    public MatchEditor(Match match, bool canEditScore) {
        Match = match;
        CanEditScore = canEditScore;
    }
}
