using System;
using System.Collections.Generic;
using System.Linq;
using GameScoring.ProgrammingParadigms;
using Wiring;
using GameScoring.DomainAbstractions;

namespace GameScoring.Application
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
    /// WinnerTakesPoint - converts a score like 6,4 to 1,0
    /// Switch - configure with a lambda function. When the function eveluates to true it will switch from it's first downstream IConsistsOf object to it's second one. e.g can be used to switch to a tiebreak.
    /// SinglePlay - a leaf node for the tree of Frames, this object records the score of a single play (throw in Bowling).
    /// ConsoleGameRunner - runs a gneric game using console as the I/O. Reguires a IGame interface
    /// Scorecard - configure it with an ASCII drawing of the scoreboard. After a play, it will return it filled in with actual scores. Requires some IScoreBindings to get the scores.
    /// ScoreBinding - configure with a function that gets the score, and a single letter to identify the score. It can return a single scores, a lists of scores, or a list of list of scores and a few other variations.
    /// Also from the Programming Paradigms layer, the wireTo extension method, which is used to wire together instances of domain abstractions to make the application.
    /// The wireTo extension method does the following -> if one object implements an interface and another has a private field of that interface (or a list of) it wires them together. It is like a gneralized Dependency Injection setter.
    /// </summary>
    public class Tennis : ITestTennis
    {
        // This code is generated manually from the diagram: see TennisDiagram.png 

        private IConsistsOf match;
        private Scorecard scorecard;
        private ConsoleGameRunner consolerunner;


        public Tennis()
        {
            // This code is hand written from the diagram. Go change the diagram first where you can reason about the logic, then come here and make it match the diagram
            // Note following code uses the fluent pattern - every method returns the this reference of the object it is called on.
            match = new Frame("match")      // (note the string is just used to identify instances during debugging, but also helps reading this code to know what they are for)
                .setIsFrameCompleteLambda((matchNumber, nSets, score) => score.Max() == 3)  // best of 5 sets is first to win 3 sets
                .WireTo(new WinnerTakesPoint("winnerOfSet")           
                    .WireTo(new Switch("switch")
                        .setSwitchLambda((setNumber, nGames, score) => (setNumber < 4 && score[0] == 6 && score[1] == 6))  
                        .WireTo(new Frame("set")
                            .setIsFrameCompleteLambda((setNumber, nGames, score) => score.Max() >= 6 && Math.Abs(score[0] - score[1]) >= 2)  
                            .WireTo(new WinnerTakesPoint("winnerOfGame")          
                                .WireTo(new Frame("game")
                                    .setIsFrameCompleteLambda((gameNumber, nBalls, score) => score.Max() >= 4 && Math.Abs(score[0] - score[1]) >= 2)
                                    .WireTo(new SinglePlay("singlePlayGame"))    
                                )
                            )
                        )
                        .WireTo(new WinnerTakesPoint("winnerOfTieBreak")         
                            .WireTo(new Frame("tiebreak")
                                .setIsFrameCompleteLambda((setNumber, nBalls, score) => score.Max() == 7)
                                .WireTo(new SinglePlay("singlePlayTiebreak"))    
                        )
                    )
                )
            );



            consolerunner = new ConsoleGameRunner("Enter winner 0 or 1", (winner, scorer) => scorer.Ball(winner, 1))
                .WireTo(match)
                .WireTo(new Scorecard(
                        "--------------------------------------------\n" +
                        "| M0  |S00|S10|S20|S30|S40|S50|S60|  G0--- |\n" +
                        "| M1  |S01|S11|S21|S31|S41|S51|S61|  G1--- |\n" +
                        "--------------------------------------------\n")
                    .WireTo(new ScoreBinding<int[]>("M", () => match.GetScore()))
                    .WireTo(new ScoreBinding<List<int[]>>("S", () => GetSetScores(match)))
                    .WireTo(new ScoreBinding<string[]>("G", () => GetLastGameScore(match)))
                );


        }





        /// <summary>
        /// Starts the Tennis game running
        /// </summary>
        public void Run()
        {
            consolerunner.Run();
        }

        // uncomment to run Bowling
        /*
        static void Main(string[] args)
        {
            new Tennis().Run();
        }
        */








        // Following two functions know how to get scores from the tree structure




        /// <summary>
        /// Gets all the set scores as a List e.g. int[] { 6, 4}, {5, 7}, {6, 2}, {8, 6}
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private List<int[]> GetSetScores(IConsistsOf match)
        {
            // note: to understand following code, see wiring diagram of the application - you are reaching in through the match and the first WinnerGetsOnePoint objects using GetSubFrames to get the Sets
            return match.GetSubFrames() // list of WinnerGetsOnePoint for sets (this just gives the winner of the set e.g. 1,0
                .Select(sf => sf.GetSubFrames().First())  // map to actual set Frames so we can get the set scores
                .Select(s => s.GetScore()) // get the scores from the sets
                .ToList();
        }




        /// <summary>
        /// Gets the current game score e.g. "30","love", "deuce","" or "adv","". If it's in a tie break, returns like "5","4"
        /// </summary>
        private string[] GetLastGameScore(IConsistsOf match)
        {
            // refer to the diagram to see how each GetSubFrames goes one deeper into the scoring tree
            if (match.GetSubFrames().Count == 0) return new string[] { "", "" };  // no play yet
            var set = match.GetSubFrames().Last()         // WinnerGetsPoint of last set
                        .GetSubFrames().First()           // switch
                        .GetSubFrames().First();          // either set or WinnerGetsPoint of tiebreak
            if (set.GetType() == typeof(Frame))           // its a set
            {
                return
                   TranslateGameScore(
                       set.GetSubFrames().Last()          // WinnerGetsPoint of last game
                       .GetSubFrames().First()            // last game
                       .GetScore());
            }
            else // it was a tiebreak
            {
                return 
                        set.GetSubFrames().First()        // tiebreak
                        .GetScore()
                        .Select(n => n.ToString()).ToArray();
            }
        }



        
        /// <summary>
        /// By example, does the following tranlations
        /// 0,0 => "" (no play yet)
        /// 1,0 => "15","love"
        /// 2,0 => "30","love"
        /// 3,0 => "40","love"
        /// 4,0 => "game",""
        /// 2,4 => "30,"game"
        /// 3,3 => "deuce",""
        /// 4,3 => "adv",""
        /// 3,4 => "","adv"
        /// 4,0 => "game",""
        /// 4,4 => "deuce",""
        /// 5,5 -> "deuce",""
        /// 5,6 => "","adv"
        /// This gets the current game score e.g. "30,love", or "adv,40"
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private string[] TranslateGameScore(int[] gamescore)
        {
            var rv = new string[] { "", "" };
            if (gamescore[0] >= 4 && gamescore[0] >= gamescore[1] + 2) { rv[0] = "game"; return rv; }
            if (gamescore[1] >= 4 && gamescore[1] >= gamescore[0] + 2) { rv[1] = "game"; return rv; }
            if (gamescore[0] >= 3 && gamescore[1] >= 3)
            {
                if (gamescore[0] > gamescore[1]) { rv[0] = "adv"; return rv; };
                if (gamescore[0] < gamescore[1]) { rv[1] = "adv"; return rv; };
                rv[0] = "deuce"; return rv;
            }
            var map = new Dictionary<int, string> { { 0, "love" }, { 1, "15" }, { 2, "30" }, { 3, "40" }, { 4, "game" } };
            return new string[] { map[gamescore[0]], map[gamescore[1]] };
        }






        // used only for unit tests



        int ITestTennis.NSets() { return match.GetSubFrames().Count(); }

        string[] ITestTennis.GetLastGameScore() { return GetLastGameScore(match); }

        int[] ITestTennis.GetMatchScore() { return match.GetScore(); }

        List<int[]> ITestTennis.GetSetScores() { return GetSetScores(match); }



        void ITestTennis.Play(int result)
        {
            match.Ball(result, 1);
        }

        bool ITestTennis.IsComplete()
        {
            return match.IsComplete();
        }

        int ITestTennis.GetTotalScore()
        {
            return match.GetScore()[0];
        }


        // returns a string representation of the entire match tree - used for debugging only
        public override string ToString()
        {
            return match.ToString();
        }

    }

}
