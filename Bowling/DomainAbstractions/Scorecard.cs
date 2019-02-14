using GameScoring.ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameScoring.DomainAbstractions
{
    /// <summary>
    /// Pass into the constructor a ASCII template of the format for the scorecard
    /// Use single letters as place holders for scores.
    /// Follow the single letter with optional single digit indexes if the scores have one or two dimentions.
    /// Follow with dashes to increase the size of the field that the score can be displayed in.
    /// Wire objects that implement IScoreBinding to so that the Scorecard can use them to get the scores 
    /// Call Score to get back a scorecard with the actual scores 
    /// Example of usage:
    ///        scorecard = new Scorecard(
    ///            "-------------------\n" +
    ///            "| T0- |S00|S10|S20|\n" +
    ///            "| T1- |S01|S11|S21|\n" +
    ///            "--------------------------------------------\n")
    ///            .WireTo(new ScoreBinding<int[]>("M", GetMatchScore))
    ///            .WireTo(new ScoreBinding<List<int[]>>("S", GetSetScores))
    ///            .WireTo(new ScoreBinding<string[]>("G", GetLastGameScore)
    ///            );  
    /// </summary>
    class Scorecard
    {
        private readonly string ASCIIDrawing;


        public Scorecard(string value) { ASCIIDrawing = value; }

        List<IScoreBinding> scoreGetters = new List<IScoreBinding>();

        public string Score()
        {
            var matches = Regex.Matches(ASCIIDrawing, "(([A-Z][0-9][0-9])|([A-Z][0-9])|([A-Z]))-*"); // The regular expression matches e.g. A, B1, C12, D-, E00--
            var rv = ASCIIDrawing;
            foreach (Match match in matches)
            {
                char id = match.Value[0];
                foreach (IScoreBinding sg in scoreGetters)
                {
                    if (id == sg.Label[0])
                    {
                        if (match.Length>=2 && char.IsDigit(match.Value[1]))
                        {
                            if (match.Length >= 3 && char.IsDigit(match.Value[2]))
                            {
                                rv = rv.Replace(match.Value, sg.GetScore(Convert.ToInt32(match.Value[1]) - Convert.ToInt32('0'), Convert.ToInt32(match.Value[2]) - Convert.ToInt32('0')).PadLeft(match.Length));
                            }
                            else
                            {
                                rv = rv.Replace(match.Value, sg.GetScore(Convert.ToInt32(match.Value[1]) - Convert.ToInt32('0')).PadLeft(match.Length));
                            }
                        }
                        else
                        {
                            rv = rv.Replace(match.Value, sg.GetScore().PadLeft(match.Length));
                        }
                    }
                }
            }
            return rv;
        }
    }
}
