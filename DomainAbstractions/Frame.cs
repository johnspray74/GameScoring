using System;
using System.Collections.Generic;
using System.Linq;
using GameScoring.ProgrammingParadigms;
using System.Text;

namespace GameScoring.DomainAbstractions
{
    /// <summary>
    /// ALA Domain abstraction for game scoring applications. Makes new subgames until the lambda function is true. See GameScoring.DomainAbstractions.Frame for more explanation.
    /// </summary>
    /// <remarks>
    /// Knowledge prequisites:
    /// To understand the full background and reasoning behind this abstraction, you need to know about ALA which is explained here 'abstractionlayeredarchitecture.com'
    /// 
    /// Frame is a domain abstraction for scoring games that are structured with 'frames'
    /// For example, in Tennis you would use one instance for the match, one for the set, and one for the game.
    /// For example, in 10-pin bowling you would use one for the game, and one for the frame.
    /// Frame basically implements a composite design pattern. It has a list of itself or any other objects that implement the IConsistsOf interface.
    /// 
    /// To use this abstraction to build a game you instantiate one of each type of Frame and wire them together.
    /// As the game progresses, it will create instances in a composite pattern (tree) by making copies of these prototypes (prototype pattern).
    /// The meaning of the wiring (Programming paradigm) is 'Consists of'
    /// For example, in tennis, A Match 'consists of' Sets which 'consists of' Games.
    /// in Bowling, a games consists of frames, which consists of throws.
    /// The programming paradigm interface name is therefore "IConsistsOf"
    /// 
    /// The abstraction instances are configured with a lambda expression that tells it when the Frame completes.
    /// It is a function of the state of the frame, e.g. frameNumber/plays/score, 
    /// (where for example, in a Bowling frame, frameNumber is 1st, 2nd 3rd child frame of the game)
    /// For example, the lambda expression for the completion of a 10-pin bowls game is (plays==10),
    /// For a tennis set the lambda is (setNumber, nGames, score) => score.Max() >= 6 &amp;&amp; Math.Abs(score[0] - score[1]) >= 2
    /// If no completion lambda expression is provided, the frame completes when its first subframe completes
    /// The Frame always passes the Ball score to all it's children, whether they are complete or not, in case they want to do something with it after they are complete.
    /// </remarks>
    public class Frame : IConsistsOf
    {
        // configurations for the abstraction
        private const int nPlayers = 2;                 // TBD make this configurable
        private Func<int, int, int[], bool> isLambdaComplete;    // our lambda function that tells us when we are complete
        private readonly int frameNumber = 0;           // which child are we to our parent. (passed to the lambda expressions in case it needs it)
        private IConsistsOf downstream;                 // this gives us the downstream object we are wired to by the application. This object only used for prototype pattern
        private string objectName;                      // only used to identify objects during debugging 
        // At run-time, Frame objects are built up in a composite pattern (tree structure) (Frame both provides an interface and accepts a list of the same interface.)
        // local state of the game consists only of our subtree; the leaves of which hold the individual scores:
        private List<IConsistsOf> subFrames = new List<IConsistsOf>();




        // Note we put the same summary as the class type on constructors becasue in ALA wiring code, Intellisence is mostly using constructor names.
        /// <summary>
        /// ALA Domain abstraction for game scoring applications. Makes new subgames until the lambda function is true. See GameScoring.DomainAbstractions.Frame for more explanation.
        /// </summary>
        public Frame(string name)  
        {
            objectName = name;
        }




        // Constructor used by GetCopy sets the FrameNumber, which is a readonly property
        public Frame(int frameNumber)
        {
            this.frameNumber = frameNumber;
        }



        // Configuration setters follow. 




        /// <summary>
        /// Set a lambda function that must return true when the frame is to complete. Function gets three parameters: frameNumber, nPlays, score
        /// </summary>
        /// <param name="lambda">A lambda function</param>
        /// <returns>this for fluent programming style</returns>
        public Frame setIsFrameCompleteLambda(Func<int, int, int[], bool> lambda)
        {
            isLambdaComplete = lambda;
            return this;
        }




        // This method is provided by an extension method in the project 'Wiring'.
        // The extension method uses reflection to do the same thing as what you see here
        // The method returns the object it is called on to support fluent coding style
        /*
        public Frame WireTo(IConsistsOf c)
        {
            subFrames = new List<IConsistsOf>();
            subFrames.Add(c);
            return this;
        }
        */




        // Methods to implement the IConsistsOf interface follow




        /// <summary>
        /// Progresses the state of the game each time a player scores.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="score"></param>
        public void Ball(int player, int score)
        {
            // This is where all the logic for the 'Frame' domain abstraction is 
            // We have three things to do
            // 1. Check if we are complete, if so do nothing
            // 2. See if the last subframe is complete, if so, start a new subframe
            // 3. Pass the ball score to all subframes

            if (IsComplete()) return;

            if (subFrames.Count==0 || subFrames.Last().IsComplete())
            {
                subFrames.Add(downstream.GetCopy(subFrames.Count));  // prototype pattern. Note: subFrames.Count gives us a frame number
            }

            foreach (IConsistsOf s in subFrames)
            {
                s.Ball(player, score);
            }
        }




        public bool IsComplete()
        {
            // Our frame is complete when the last subframe is complete AND the lambda function says we are complete
            if (subFrames.Count == 0) return false; // no plays yet
            return (subFrames.Last().IsComplete()) &&  // last subframe is complete
                (isLambdaComplete == null || isLambdaComplete(frameNumber, GetnPlays(), GetScore()));  // lambda is complete
        }




        public int GetnPlays()
        {
            return subFrames.Count();
        }




        public int[] GetScore()
        {
            return subFrames.Select(sf => sf.GetScore()).Sum();
        }



        // This method allows the client to access individual scores for sub-frames
        List<IConsistsOf> IConsistsOf.GetSubFrames()
        {
            return subFrames;
        }




        // This used when we start a new subframe. It uses the prototype pattern, that is it makes a copy of ourself and the child that was provided by the wiring.
        IConsistsOf IConsistsOf.GetCopy(int frameNumber)
        {
            var gf = new Frame(frameNumber);
            gf.objectName = this.objectName;
            gf.subFrames = new List<IConsistsOf>();
            gf.downstream = downstream.GetCopy(0);
            gf.isLambdaComplete = this.isLambdaComplete;
            return gf as IConsistsOf;
        }




        // for debugging only -- allows us to print out a map of the whole tree
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name = "); sb.Append(objectName); sb.Append(Environment.NewLine);
            sb.Append("frameNumber = "); sb.Append(frameNumber); sb.Append(Environment.NewLine);
            sb.Append("nPlays = "); sb.Append(GetnPlays()); sb.Append(Environment.NewLine);
            sb.Append("localScore = "); sb.Append(GetScore()[0]); sb.Append(","); sb.Append(GetScore()[1]); sb.Append(Environment.NewLine); 
            sb.Append("ourFrameComplete = "); sb.Append(IsComplete()); sb.Append(Environment.NewLine);
            if (downstream == null) sb.Append("no subframes");
            else
            {
                sb.Append("numberSubFrames = "); sb.Append(subFrames.Count()); sb.Append(Environment.NewLine);
                sb.Append("subFrames ====>" + Environment.NewLine);
                foreach (IConsistsOf subFrame in subFrames)
                {
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


