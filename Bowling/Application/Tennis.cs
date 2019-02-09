using System;
using System.Collections.Generic;
using System.Linq;
using DomainAbstractions;
using ProgrammingParadigms;
using Wiring;
using System.Text;




namespace Application
{
    /// <summary>
    /// Knowledge dependencies:
    /// Firstly you need knowledge of ALA (Abstraction layered Architecture) to understand the structure of this code.
    /// In the conext of ALA, this is the application layer.
    /// This is not the source code - it is code manually generated from the application diagram "Tennis.adoc" projected as "Tennis.pdf"
    /// From the DomainAbstractions layer/folder/namespace, you need knowledge of the abstractions Frame, SinglePlay and WinnerGetOnePoint - see the folder comments at top of Frame.cs, SinglePlay.cs and WinnerGetsOnePoint.cs for this knowledge
    /// From the Programming Paradigms layer/folder/namespace, you need knowledge of the IConsistsOf interface 
    /// We don't need knowledge of the specific contents of this interfaces, we just need to know that it allows the instances of Domain Abstractions to be wired together. Like a grammar).
    /// The 'Frame' abstraction instances can be wired in a chain meaning 'consists of'.
    /// Also from the Programming Paradigms layer, the wireTo extension method, which is used to wire together instances of domain abstractions to make the application.
    /// The wireTo extension method does the following -> if one object implements an interface and another has a private field of that interface (or a list of) it wires them together. It is like a gneralized Dependency Injection setter.
    /// </summary>
    public class Tennis
    {
        // This section of code is generated manually from the diagram
        // Go change the diagram first where you can reason about the logic, then come here and make it match the diagram
        // Note following code uses the fluent pattern - every method returns the this reference of the object it is called on.
        private IConsistsOf match = new Frame("match")      // (note the string is just used to identify instances during debugging, but also helps reading this code to know what they are for)
            .setIsFrameCompleteLambda((matchNumber, nSets, score) => score.Max()==3)  // best of 5 sets is first to win 3 sets
            .WireTo(new WinnerTakesPoint("winnerOfSet")            // Reduce set score to one point for winner e.g. 6,4 to 1,0
                .WireTo(new Switch("switch")
                    .setSwitchLambda((setNumber, nGames, score) => (setNumber<4 && score[0]==6 && score[1]==6))   // switch to tiebreak instead of set when set score is 6,6, except last set
                    .WireTo(new Frame("set")                     
                        .setIsFrameCompleteLambda((setNumber, nGames, score) => score.Max()>=6 && Math.Abs(score[0]-score[1])>=2)  // A set completes when one player wins 6 games with a margin of 2
                        .WireTo(new WinnerTakesPoint("winnerOfGame")            // Convert game score to one point for winner
                            .WireTo(new Frame("game")          
                                .setIsFrameCompleteLambda((gameNumber, nBalls, score) => score.Max()>=4 && Math.Abs(score[0]-score[1])>=2) 
                                .WireTo(new SinglePlay("singlePlayGame"))    // Must always terminate the chain with SinglePlay
                            )
                        )
                    )
                    .WireTo(new WinnerTakesPoint("winnerOfTieBreak")            // Convert tiebreak score to one point for winner e.g. 6,7 -> 0,1
                        .WireTo(new Frame("tiebreak")          
                            .setIsFrameCompleteLambda((setNumber, nBalls, score) => score.Max()==7)
                            .WireTo(new SinglePlay("singlePlayTiebreak"))    // Must always terminate the chain with SinglePlay
                    )
                )
            )
        );





        public void Play(int result)
        {
            // Play is a Ball
            // result is the winner, player 0 or 1.
            match.Ball(result, 1);
        }

        public bool IsComplete()
        {
            return match.IsComplete();
        }

        public string GetScore()
        {
            // returns score in the form:
            // match score = 2,1   Set scores = 6 4   2 0      Game score = 30,love
            // If in a tiebreak:
            // Set score = 6 4   6 6      Game score = 6,5
            StringBuilder sb = new StringBuilder();
            sb.Append("Match score = ");
            sb.Append(GetTotalScore()[0] + " " + GetTotalScore()[1]);
            sb.Append("   ");
            sb.Append("Set scores = ");
            GetSetScores().Aggregate(sb, (s1, n) => s1.Append(n[0].ToString() + " " + n[1].ToString() + "   "));
            sb.Append("   ");
            sb.Append("Game score = "); sb.Append(GetGameScore());
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }




        // This gets the current game score e.g. "30,love", or "adv 0"
        // If it's in a tie break, returns like "5,4"
        // note: to understand following code, see wiring diagram of the application - you are using the GetSubFrames method to reach the current game instance via the match, the first WinnerGetsOnePoint, the switch, the set, and the 2nd WinnerGetsOnePoint objects
        public string GetGameScore()
        {
            var map = new Dictionary<int, string> { { 0, "love" }, { 1, "15" }, { 2, "30" }, { 3, "40" } };
            if (match.GetSubFrames().Count == 0) return "";
            IConsistsOf temp = match
            .GetSubFrames().Last()     // WinnerGetsOnPoint of last set
            .GetSubFrames().First()    // switch
            .GetSubFrames().First();   // either set or WinnerGetsOnePoint of tiebreak
            if (temp.GetType() == typeof(Frame))  // we are in a normal game, not a tiebreak
            {
                int[] gamescore = temp
                    .GetSubFrames().Last()     // WinnerGetsOnPoint of last game
                    .GetSubFrames().First()    // current game object
                    .GetScore();
                if (gamescore[0] >= 4 && gamescore[0] >= gamescore[1] + 2) return "game 0";
                if (gamescore[1] >= 4 && gamescore[1] >= gamescore[0] + 2) return "game 1";
                if (gamescore[0] >= 3 && gamescore[1] >= 3)
                {
                    if (gamescore[0] > gamescore[1]) return "adv 0";
                    if (gamescore[0] < gamescore[1]) return "adv 1";
                    return "deuce";
                }
                if (gamescore[0] > 3 || gamescore[1] > 3 || gamescore[0] < 0 || gamescore[1] < 0) return "";
                return map[gamescore[0]] + "," + map[gamescore[1]];
            }
            else  // in a tie break
            {
                int[] tiebreakscore = temp
                    .GetSubFrames().First()    // current tiebreak object
                    .GetScore();
                return tiebreakscore[0].ToString() + "," + tiebreakscore[1].ToString();
            }
        }


        // -------------------- following would normally be private but are useful testing or debugging functions ---------------------------------------------


        public int NSets()
        {
            return match.GetSubFrames().Count();
        }


        public int[] GetTotalScore()
        {
            return match.GetScore();
        }


        // Gets all the set scores as a List e.g. int[] { 6, 4}, {5, 7}, {6, 2}, {8, 6}
        // note: to understand following code, see wiring diagram of the application - you are reaching in through the match and the first WinnerGetsOnePoint objects using GetSubFrames to get the Sets
        public List<int[]> GetSetScores()
        {
            return match.GetSubFrames() // list of WinnerGetsOnePoint for sets (this just gives the winner of the set e.g. 1,0
                .Select(sf => sf.GetSubFrames().First())  // map to actual set Frames so we can get the set scores
                .Select(s => s.GetScore()) // get the scores from the sets
                .ToList();
        }




        // returns a string representation of the entire match tree - used for debugging only
        public override string ToString()
        {
            return match.ToString();
        }

    }


}
