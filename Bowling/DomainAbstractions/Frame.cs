using System;
using System.Collections.Generic;
using System.Linq;
using ProgrammingParadigms;
using System.Text;

namespace DomainAbstractions
{
    /// <summary>
    /// Knowledge prequisites:
    /// To understand the full background and reasoning behind this abstraction, you need to know about ALA which is explained here 'abstractionlayeredarchitecture.com'
    /// 
    /// Frame is a domain abstraction for scoring games that are structured with 'frames'
    /// For example, in Tennis you would use one instance for the match, one for the set, and one for the game.
    /// For example, in 10-pin bowling you would use one for the whole game, and one for the Frame.
    /// Frame basically implements a composite design pattern. It has a list of itself or any other objects that implement the IConsistsOf interface.

    /// To use this abstraction to build a game you instantiate one of each type of Frame and wire them together.
    /// As teh game progresses, it will create more instances in the tree based on these inital ones using the prototype pattern.
    /// The meaning of the wiring is 'Consists of'
    /// For example, in tennis, A Match 'consists of' Sets which 'consists of' Games.
    /// in Bowling, a games consists of frames, which consists of throws.
    /// The programming paradigm interface name is therefore "ConsistsOf"
    /// When we wire up instances of Frame to represent things like games, frames, matches, sets, we give them a name in the constructor to help identify them at debugging.

    /// The abstraction instances are configured with a lambda expression that tells it when that type of Frame completes.
    /// It is a function of the state of the frame, e.g. frameNumber/plays/score, 
    /// (where for example, in a Bowling frame, frameNumber is 1st, 2nd 3rd child frame of the game)
    /// For example, the lambda expression for the completion of a 10-pin bowls game is (plays==10),
    /// For a tennes set the lambda is (setNumber, nGames, score) => score.Max() >= 6 && Math.Abs(score[0] - score[1]) >= 2
    /// If no completion lambda expression is provided, the frame completes when its first subframe completes
    /// The Frame always passes the Ball score to all it's children, whether they are complete or not.


    public class Frame : IConsistsOf
    {


        // configurations for the abstraction
        private const int nPlayers = 2;                 // TBD make this configurable
        private Func<int, int, int[], bool> isFrameComplete;    // our lambda function that tells us when we are complete

        // These are just used to identify objects during debugging 
        private string objectName;  // used to identify objects druing debugging (e.g. can be used to compare before Console.Writeline)
        private readonly int frameNumber = 0;           // which child are we to our parent. (sometimes the lambda expressions may want to use this)
        private StringBuilder debug = new StringBuilder();  // used to accumulate local debug information to be printed out later by the ToString method

        private IConsistsOf downstream;      // this gives us the downstream object we are wired to by the application. This object only used for prototype pattern

        // state of the game 
        // At run-time, Frame objects are built up in a composite pattern (tree structure) because they both provide an interface and accept a list of the same interface.
        // The first of each type of frame is provided at the start of play, but during play more Frames are created using the prototype pattern
        // local state consists only of our subframes
        private List<IConsistsOf> subFrames = new List<IConsistsOf>(); // at runtime, these are the composite pattern tree of subobjects (may not be frames)




        public Frame(string name)  
        {
            objectName = name;
        }


        public Frame(int frameNumber)
        {
            this.frameNumber = frameNumber;
        }



        // fluent setters are used for the Lambda functions that configure this class and for wiring to other instances

        public Frame setIsFrameCompleteLambda(Func<int, int, int[], bool> lambda)
        {
            isFrameComplete = lambda;
            return this;
        }




        // This method is provided by an extension method in the project 'Wiring'.
        // The extension method uses reflection to do the same thing
        // The method returns the object it is called on to support fluent coding style
/*
        public Frame WireTo(IConsistsOf c)
        {
            subFrames = new List<IConsistsOf>();
            subFrames.Add(c);
            return this;
        }
*/



        // This is where all the logic for the abstraction is 
        // We have three things to do
        // 1. Pass the ball to all subframes
        // 2. See if the last frame is complete
        // 3. If so, start a new subframe unless out lambda tells us we are complete.

        public void Ball(int player, int score)
        {
            // if our frame is complete, so nothing
            if (IsComplete()) return;

            // 2. see if it's time to start a new subframe by seeing if the last subframe is complete
            if (subFrames.Count==0 || subFrames.Last().IsComplete())
            {
                subFrames.Add(downstream.GetCopy(subFrames.Count));  // prototype pattern. Note: subFrames.Count gives us a frame number
            }

            // 2. Pass the ball onto subframes
            foreach (IConsistsOf s in subFrames)
            {
                s.Ball(player, score);
            }
        }



        private bool IsComplete()
        {
            return (subFrames.Count==0 || subFrames.Last().IsComplete()) &&  // last subframe is complete
                (isFrameComplete == null || isFrameComplete(frameNumber, GetnPlays(), GetScore()));  // lambda is complete
        }


        bool IConsistsOf.IsComplete() { return IsComplete(); }

        private int GetnPlays()
        {
            return subFrames.Count();
        }

        int IConsistsOf.GetnPlays() { return GetnPlays(); }


        private int[] GetScore()
        {
            return subFrames.Select(sf => sf.GetScore()).Sum();
        }

        int[] IConsistsOf.GetScore() { return GetScore(); }


        // This method allows the client to access individual scores for sub-frames
        List<IConsistsOf> IConsistsOf.GetSubFrames()
        {
            return subFrames;
        }



        // This used when we start a new subframe. It uses the prototype pattern, that is it makes a copy of the first subframe that was provided at the start of the game.
        IConsistsOf IConsistsOf.GetCopy(int frameNumber)
        {
            var gf = new Frame(frameNumber);
            gf.objectName = this.objectName;
            gf.subFrames = new List<IConsistsOf>();
            gf.downstream = downstream.GetCopy(0);
            gf.isFrameComplete = this.isFrameComplete;
            return gf as IConsistsOf;
        }




        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name = "); sb.Append(objectName); sb.Append(Environment.NewLine);
            sb.Append("frameNumber = "); sb.Append(frameNumber); sb.Append(Environment.NewLine);
            sb.Append("nPlays = "); sb.Append(GetnPlays()); sb.Append(Environment.NewLine);
            sb.Append("localScore = "); sb.Append(GetScore()[0]); sb.Append(","); sb.Append(GetScore()[1]); sb.Append(Environment.NewLine); 
            sb.Append("ourFrameComplete = "); sb.Append(IsComplete()); sb.Append(Environment.NewLine);
            // sb.Append("debug = "); sb.Append(debug); sb.Append(Environment.NewLine);
            if (downstream == null) sb.Append("no subframes");
            else
            {
                sb.Append("numberSubFrames = "); sb.Append(subFrames.Count()); sb.Append(Environment.NewLine);
                sb.Append("subFrames ====>" + Environment.NewLine);
                foreach (IConsistsOf subFrame in subFrames)
                {
                    // string sf = subFrames.Last().ToString();
                    string sf = subFrame.ToString();
                    string[] lines = sf.Split(new string[] { Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        sb.Append("----" + line + Environment.NewLine);
                    }
                }
            }
            return sb.ToString();
        }

    }
}


