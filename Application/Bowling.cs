﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameScoring.DomainAbstractions;
using GameScoring.ProgrammingParadigms;
using Wiring;
using System.Text;


namespace GameScoring.Application
{

    /// <summary>
    /// Knowledge dependencies:
    /// This application is written using ALA, a software architecture that is described at AbstractionLayeredArchitecture.com  
    /// This is not the source code - it is code manually generated from the application diagram (in the Application folder)
    /// You also need knowledge of the domain asbtractions:
    ///  Frame - represents a sequence of 'subgames'. Frames can be wired together as the implement and accept IConsistsOf interface.
    ///     At run-time, Frame builds a composite pattern (tree structure) of subgames as the game progresses. It stops when a supplied lambda function evealuates to true.
    ///     The lammbda function is given three parameters to use that tell it the state of the game, frame number to our parent, number of plays so far, and current score.
    ///  Bonuses - when the attached IConsistsOf completes, this continues to add scores until the supplied lambda returns true. 
    ///  SinglePlay - a leaf node for the tree of Frames, this object records the score of a single play (throw in Bowling).
    ///  ConsoleGameRunner - runs a gneric game using console as the I/O. Reguires a IGame interface
    ///  Scorecard - configure it with an ASCII drawing of the scoreboard. After a play, it will return it filled in with actual scores. Requires some IScoreBindings to get the scores.
    ///  ScoreBinding - configure with a function that gets the score, and a single letter to identify the score. It can return a single scores, a lists of scores, or a list of list of scores and a few other variations.
    /// You also need knowledge of the WireTo extension method - if one object implements an interface and another has a private field of that interface (or a list of) it wires them together.
    /// </summary>
    public class Bowling : IGame
    {
        private IConsistsOf game;
        private Scorecard scorecard;
        private ConsoleGameRunner consolerunner;

        public Bowling()
        { 
            // standard rules game
            // This section of code is generated manually from the diagram
            // Go change the diagram first where you can reason about the logic, then come here and make it match the diagram
            // Note following code uses the fluent pattern - every method returns the this reference of the object it is called on.
            game = new Frame("game")        // Frame to represent the whole game of ten frames
                .setIsFrameCompleteLambda((gameNumber, frames, score) => frames == 10)
                .WireTo(new Bonuses("bonus")                               // Frame to represent one bowling Frame, and add the bonuses for spares and strikes.
                    .setIsBonusesCompleteLambda((plays, score) => score < 10 || plays == 3)
                    .WireTo(new Frame("frame")                  // Frame to represent one bowling Frame without spares or strikes
                        .setIsFrameCompleteLambda((frameNumber, balls, pins) => frameNumber < 9 && (balls == 2 || pins[0] == 10) || (balls == 2 && pins[0] < 10 || balls == 3))
                        // Note in the lambda expression above that frame 9 is different - it can complete after 3 balls and the score card records 3 ball scores instead of 2.
                        .WireTo(new SinglePlay("SinglePlay")               // represents the pins scored for one ball
                )));

            /*
                    // kids simple rules rules game 
                    private IConsistsOf kidsGame = new Frame("game")  // Frame to represent the whole game of five frames
                        .setIsFrameCompleteLambda((gameNumber, frames, score) => frames==5)
                        .WireTo(new Frame("frame")                 // Frame to represent one bowling Frame
                            .setIsFrameCompleteLambda((frameNumber, balls, pins) => balls==3 || pins[0] == 10)  // give the kids 3 balls every frame and keep all frames the same
                                .WireTo(new SinglePlay("SinglePlay")               // represents the pins scored for one ball
                        ));
            */

            scorecard = new Scorecard(
                "-------------------------------------------------------------------------------------\n" +
                "|F00|F01|F10|F11|F20|F21|F30|F31|F40|F41|F50|F51|F60|F61|F70|F71|F80|F81|F90|F91|F92|\n" +
                "|    ---+    ---+    ---+    ---+    ---+    ---+    ---+    ---+    ---+    ---+----\n" +
                "|  T0-  |  T1-  |  T2-  |  T3-  |  T4-  |  T5-  |  T6-  |  T7-  |  T8-  |    T9-    |\n" +
                "-------------------------------------------------------------------------------------\n")
                .WireTo(new ScoreBinding<List<List<string>>>("F", () => TranslateFrameScores(GetFrameThrowScores())))
                .WireTo(new ScoreBinding<List<int>>("T", GetAccumulatedFrameScores));

            consolerunner = new ConsoleGameRunner("Enter number pins:")
                .WireTo(this);
        }


        public void Run()
        {
            consolerunner.Run();
        }


        // Following three functions are the interface used by whatever is running the game, such as a console runner

        public void Play(int result)
        {
            // A play is a throw
            // result is the number of pins
            game.Ball(0,result);  // scoring one player, so the player index is always 0.
            // (if two players you need a second instance of the application, because the tree structure of Frames is different for each.)
        }

        public bool IsComplete()
        {
            return game.IsComplete();
        }



        // returns scorecard in the format of a string like:
        // | 5 4 | 5 / | X   |
        // |  9  |  29 |  39 |
        // see real score cards to understand the numbers, dashes, slashes-s /s and Xs
        string IGame.GetScore()
        {
            return scorecard.Score();
        }



        /// <summary>
        /// get frame scoring transformed as needed for a real bowling scorecard. examples:
        /// 7,2 -> "7","2"
        /// 7,0 -> "7","-"
        /// -,3 -> "-","7"
        /// 7,3 -> "7","/" 
        /// 10,0 -> "",X
        /// 0,10 -> "-","/"
        /// additional ninth frame translations:
        /// 10,0 -> "X","-"
        /// 7,3,2 -> "7","/","2"
        /// 10,7,3 -> "X","7","/"
        /// 0,10,10 -> "-","/","X"
        /// 10,10,10 -> "X","X","X"
        /// This function is an abstraction  (does not refer to local variables or have side effects)
        /// </summary>
        /// <param name="frames">
        /// The parameter, frames, is a list of frames, each with a list of integers between 0 and 10 for the numbers of pins.
        /// </param>
        /// <returns>
        /// return value will be exactly the same structure as the parameter but with strings instead of ints
        /// </returns>
        private List<List<string>> TranslateFrameScores(List<List<int>> frames)
        { 
            // This function looks a bit daunting but actually it just methodically makes the above example tranlations of the frame pin scores needed for a real bowling scorecard 
            // (is there an abstractiuon to be found in this, so that it can be expressed as a set of rules similar to those above examples, and handle tennis game type number to string translations as well?)
            List<List<string>> rv = new List<List<string>>(); 
            int frameNumber = 0;
            foreach (List<int> frame in frames)
            {
                var frameScoring = new List<string>();
                if (frame.Count > 0)
                {
                    // The first 9 frames position the X in the second box on a real scorecard - handle this case separately
                    if (frameNumber<9 && frame[0] == 10)
                    {
                        frameScoring.Add("");
                        frameScoring.Add("X");
                    }
                    else
                    {
                        int ballNumber = 0;
                        foreach (int pins in frame)
                        {
                            if (pins == 0)
                            {
                                frameScoring.Add("-");
                            }
                            else
                            if (ballNumber>0 && frame[ballNumber]+frame[ballNumber-1] == 10)
                            {
                                frameScoring.Add(@"/");
                            }
                            else
                            if (pins == 10)
                            {
                                frameScoring.Add("X");
                            }
                            else
                            {
                                frameScoring.Add(pins.ToString());
                            }
                            ballNumber++;
                        }

                    }
                }
                rv.Add(frameScoring);
                frameNumber++;
            }
            return rv;
        }


        public int NFrames()
        {
            return game.GetSubFrames().Count();
        }

        // get a list of lists of frame ball scores
        public List<List<int>> GetFrameThrowScores()
        {
            // extract the individual ball scores from the score tree
            return game.GetSubFrames().Select(f => f.GetSubFrames().Select(b => b.GetScore()[0]).ToList()).ToList();
        }


        public List<int> GetAccumulatedFrameScores()
        {
            return game.GetSubFrames().Select(sf => sf.GetScore()[0]).Accumulate().ToList();
        }

        public int GetTotalScore()
        {
            return game.GetScore()[0];
        }



        // A large string representing the tree structure of teh whole game - used only for debugging
        public override string ToString()
        {
            return game.ToString();
        }

    }


}

