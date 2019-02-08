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
    /// This application is written using ALA, a software architecture that is described at AbstractionLayeredArchitecture.com  
    /// This is not the source code - it is code manually generated from the application diagram "StandardBowlingGame.pdf"
    /// You also need knowledge of the domain asbtractions:
    ///     Frame - represents a sequence of 'subgames'. It builds a composite pattern (tree structure) of IConsistsOfs as the game progresses, and stops when a supplied lambda function is true.
    ///     Bonuses - when the attached IConsistsOf completes, this continues to add scores until the supplied lambda returns true. 
    ///     SinglePlay - a leaf node for the tree of Frames, this object records the score of a single play (throw in Bowling).
    /// You also need knowledge of the WireTo extension method - if one object implements an interface and another has a private field of that interface (or a list of) it wires them together.
    /// </summary>
    public class Bowling
    {
        // standard rules game
        // This section of code is generated manually from the diagram
        // Go change the diagram first where you can reason about the logic, then come here and make it match the diagram
        // Note following code uses the fluent pattern - every method returns the this reference of the object it is called on.
        private IConsistsOf game = new Frame("game")        // Frame to represent the whole game of ten frames
            .setIsFrameCompleteLambda((gameNumber, frames, score) => frames==10)
            .WireTo(new Bonuses("bonus")                               // Frame to represent one bowling Frame, and add the bonuses for spares and strikes.
                .setIsBonusesCompleteLambda((plays, score) => score<10 || plays==3)
                .WireTo(new Frame("frame")                  // Frame to represent one bowling Frame without spares or strikes
                    .setIsFrameCompleteLambda((frameNumber, balls, pins) => frameNumber<9 && (balls==2 || pins[0]==10) || (balls==2 && pins[0]<10 || balls == 3))
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


        public void Play(int result)
        {
            // A play is a throw
            // result is the number of pins
            game.Ball(0,result);  // scoring one player, so the player index is always 0.
            // (if two players you need a second instance of the game, because the tree structure of Frames is different for each.)
        }

        public bool IsComplete()
        {
            return game.IsComplete();
        }


        // returns scorecard in the format of a string like:
        // | 5 4 | 5 / | X   |
        // |  9  |  29 |  39 |
        // see real score cards to understand the numbers, dashes, slashes-s /s and Xs
        public string GetScore()
        {
            // returns score in the form:
            // Set score = 6 4   2 0      Game score = 30,love
            StringBuilder sb = new StringBuilder();
            // sb.Append("Total score = " + GetTotalScore());
            // sb.Append(Environment.NewLine);
            // sb.Append("NFrames = " + NFrames());
            // sb.Append(Environment.NewLine);
            Console.Write("      Frame ball scores = ");
            sb.Append("|");
            for (int frame = 0; frame < NFrames(); frame++)
            {
                List<int> frameScores = GetFrameThrowScores(frame);
                sb.Append(" ");
                if (frameScores[0] == 0)
                {
                    sb.Append("-");
                }
                else
                if (frameScores[0] == 10)
                {
                    sb.Append("X");
                }
                else
                {
                    sb.Append(frameScores[0]);
                }
                sb.Append(" ");
                if (frameScores.Count >= 2)
                {
                    if (frameScores[1] == 10)
                    {
                        sb.Append("X");
                    }
                    else
                    if (frameScores[0] + frameScores[1] == 10)
                    {
                        sb.Append(@"/");
                    }
                    else
                    if (frameScores[1] == 0)
                    {
                        sb.Append("-");
                    }
                    else
                    {
                        sb.Append(frameScores[1]);
                    }
                }
                sb.Append(" ");
                if (frameScores.Count >= 3)
                {
                    if (frameScores[0] == 10)
                    {
                        sb.Append("X");
                    }
                    else
                    {
                        sb.Append(frameScores[2]);
                    }
                    sb.Append(" ");
                }
                sb.Append("|");
            }
            sb.Append(Environment.NewLine);
            sb.Append("Cumulating frame scores = ");
            sb.Append("|");
            GetAccumulatedFrameScores().ForEach(s => sb.Append(String.Format(" {0,3} |",s)));
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }






        // -------------------- following would normally be private but are useful testing or debugging functions ---------------------------------------------


        public int NFrames()
        {
            return game.GetSubFrames().Count();
        }

        public int GetTotalScore()
        {
            return game.GetScore()[0];
        }


        // get the accumulated score for a given frame 
        public int GetFrameScore(int frame)
        {
            return GetAccumulatedFrameScores()[frame];
        }


        // get a list of the ball scores for a given frame (Not needed for the standard bowling score card - just used for testing)
        public List<int> GetFrameThrowScores(int frame)
        {
            return game.GetSubFrames()[frame].GetSubFrames().Select(sf => sf.GetScore()[0]).ToList();
        }


        // Get the individual Frame scores needed for a standard bowling score card 
        private List<int> GetAccumulatedFrameScores()
        {
            return game.GetSubFrames().Select(sf => sf.GetScore()[0]).Accumulate().ToList();
        }



        // A large string representing the tree structure of teh whole game - used only for debugging
        public override string ToString()
        {
            return game.ToString();
        }

    }


}


