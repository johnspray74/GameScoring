using System;
using System.Collections.Generic;
using System.Linq;
using GameScoring.ProgrammingParadigms;
using System.Text;

namespace GameScoring.DomainAbstractions
{
    /// <summary>
    /// ALA Domain abstraction for game scoring applications. Has two subgames. Switches from the first subgame to the second when the lambda function goes true. See GameScoring.DomainAbstractions.Frame for more explanation.
    /// </summary>
    /// <remarks>
    /// Prerequisites to understanding
    /// To understand the full background and reasoning behind this abstraction, you need to know about ALA which is explained here 'abstractionlayeredarchitecture.com'
    /// Additional prerequisite knowledge is the Decorator pattern, which along with composite is the most common pattern in ALA (displacing the Observer (Publish/Subscribe) pattern.) 
    /// A decorator implements and accepts the same interface, and usually passes most methods straight through
    /// so it can be inserted between any other two abstractions that are connected by that interface without affecting operation
    /// except by modifying the behaviour on that interface in one specific way.
    /// In this case, there are two subgames.
    /// When the lambda function returns true, we switch from the first subgame to the second. Further Balls are passed to the second subframe.
    /// The returned score is the summ from the two subgames.
    /// </remarks>
    /// <example>
    /// An example of the use of this abstraction is for the tiebreak in Tennis.
    /// </example>
    public class Switch : IConsistsOf
    {
        private IConsistsOf downStreamFrame1;           // the one we start with 
        private IConsistsOf downStreamFrame2;           // the one we switch to
        private Func<int, int, int[], bool> isLambdaComplete;
        private string objectName;                      // Just used to identify objects druing debugging. Becasue ALA makes many instances from abstractions, it is useful for them to be identifiable during debug  (e.g. can be used to compare before Console.Writeline)
        private readonly int frameNumber = 0;           // This is our child number of our parent, also useful to identify instances (sometimes a lambda expressions may want to use this)




        /// <summary>
        /// ALA Domain abstraction for game scoring applications. Has two subgames. Switches from the first subgame to the second when the lambda function goes true. See GameScoring.DomainAbstractions.Frame for more explanation.
        /// </summary>
        public Switch(string name)
        {
            objectName = name;
        }




        public Switch(int frameNumber)
        {
            this.frameNumber = frameNumber;
        }




        /// <summary>
        /// Set a lambda function that must return true when we need to switch to the second subgame. Function gets three parameters: frameNumber, nPlays, score
        /// </summary>
        /// <param name="lambda">A lambda function</param>
        /// <returns>this for fluent programming style</returns>
        public Switch setSwitchLambda(Func<int, int, int[], bool> lambda)
        {
            isLambdaComplete = lambda;
            return this;
        }





        // This method is provided by an extension method in the project 'Wiring'.
        // The extension method uses reflection to do the same thing
        // The method returns the object it is called on to support fluent coding style
        /*
        public Switch WireTo(IConsistsOf c)
        {
            if (downStreamFrame1 == null)
            {
                downStreamFrame1 = c;
            }
            else
            {
                downStreamFrame2 = c;
            }
            return this;
        }
        */




        public void Ball(int player, int score)
        {
            if (IsSwitched())
            {
                if (downStreamFrame2 != null) downStreamFrame2.Ball(player, score);
            }
            else
            {
                if (downStreamFrame1 != null) downStreamFrame1.Ball(player, score);
            }
        }




        public bool IsComplete()
        {
            return IsSwitched() ? downStreamFrame2.IsComplete() : downStreamFrame1.IsComplete();
        }




        private bool IsSwitched()
        {
            return isLambdaComplete!=null && isLambdaComplete(frameNumber, downStreamFrame1.GetnPlays(), downStreamFrame1.GetScore());
        }




        public int GetnPlays()
        {
            return IsSwitched() ? downStreamFrame2.GetnPlays() : downStreamFrame1.GetnPlays();
        }




        public int[] GetScore()
        {
            if (IsSwitched())
            {
                return downStreamFrame1.GetScore().AddIntArray(downStreamFrame2.GetScore());
            }
            else
            {
                return downStreamFrame1.GetScore();
            }
        }




        List<IConsistsOf> IConsistsOf.GetSubFrames()
        {
            return new List<IConsistsOf> { IsSwitched() ? downStreamFrame2 : downStreamFrame1 };
        }




        IConsistsOf IConsistsOf.GetCopy(int frameNumber)
        {
            // Copy both the decorator and the object it's conencted to
            var sw = new Switch(frameNumber);
            if (downStreamFrame1 != null)
            {
                sw.downStreamFrame1 = downStreamFrame1.GetCopy(frameNumber);
            }
            if (downStreamFrame2 != null)
            {
                sw.downStreamFrame2 = downStreamFrame2.GetCopy(frameNumber);
            }
            sw.isLambdaComplete = this.isLambdaComplete;
            sw.objectName = this.objectName;
            return sw as IConsistsOf;
        }




        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name = "); sb.Append(objectName); sb.Append(Environment.NewLine);
            sb.Append("frameNumber = "); sb.Append(frameNumber); sb.Append(Environment.NewLine);
            sb.Append("nPlays = "); sb.Append(GetnPlays()); sb.Append(Environment.NewLine);
            sb.Append("localScore = "); sb.Append(GetScore()[0]); sb.Append(","); sb.Append(GetScore()[1]); sb.Append(Environment.NewLine);
            sb.Append("ourFrameComplete = "); sb.Append(IsComplete()); sb.Append(Environment.NewLine);
            if (downStreamFrame1 == null) sb.Append("no downstream frame");
            else
            {
                sb.Append("===================" + Environment.NewLine);
                string sf = downStreamFrame1.ToString();
                string[] lines = sf.Split(new string[] { Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    sb.Append("----" + line + Environment.NewLine);
                }
            }
            if (downStreamFrame2 == null) sb.Append("no downstream switch frame");
            else
            {
                sb.Append("~~~~~~~~~~~~~~~~~~~~" + Environment.NewLine);
                string sf = downStreamFrame2.ToString();
                string[] lines = sf.Split(new string[] { Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    sb.Append("----" + line + Environment.NewLine);
                }
            }
            return sb.ToString();
        }

    }
}


