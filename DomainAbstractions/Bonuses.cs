using System;
using System.Collections.Generic;
using System.Linq;
using GameScoring.ProgrammingParadigms;
using System.Text;

namespace GameScoring.DomainAbstractions
{
    /// <summary>
    /// ALA Domain Abstraction. When its subgame completes, it continues to add ball scores until its lambda goes true.
    /// </summary>
    /// <remarks>
    /// Prerequisites to understanding
    /// To understand the full background and reasoning behind this abstraction, you need to know about ALA which is explained here 'abstractionlayeredarchitecture.com'
    /// Particular prerequisite knowledge is the Decorator pattern, which is the most common pattern in ALA (displacing the Observer (Publish/Subscribe) pattern.) 
    /// A decorator implements and accepts the same interface, and just passes most methods straight through
    /// so it can be inserted between any other two abstractions that are connected by that interface without affecting operation except by modifying the behaviour on that interface in one specific way.
    /// 
    /// Bonuses is a domain abstraction and a Decorator that implements and accepts the IConsistsOf interface. It always has one child.
    /// The modification is to add bonues to the score.
    /// It does this only after the IsComplete method of the downstream child has returned true.
    /// The IsComplete method result returned from downstream is passed upstream normally, but it is assumed that ball scores are still received. 
    /// These are added to the local score until the local IsComplete lambda function returns true.
    /// <example>
    /// For example, in 10-pin bowling you would use this with a lambda of "score&lt;10 || plays==3" so that after the downstream Frame completes, it will keep adding ball scores if the score is already 10, until the total throws is 3.
    /// </example>
    /// The lambda is a function of the state of the frame, e.g. plays,score.
    /// </remarks>
    public class Bonuses : IConsistsOf
    {
        // configurations for the abstraction

        private IConsistsOf downStreamFrame;            // We should be wired to a downstream something
        private int bonusScore;                         // local state consists of the bonus score, and number of bonus plays
        private int bonusBalls;
        private readonly int frameNumber = 0;           // which child are we to our parent. (sometimes the lambda expressions may want to use this)
        private Func<int, int, bool> lambdaFunction;
        private string objectName;                      // only used to identify objects during debugging




        public Bonuses(string name)
        {
            objectName = name;
        }




        public Bonuses(int frameNumber)
        {
            this.frameNumber = frameNumber;
        }




        // Following functions are for configuration




        /// <summary>
        /// Set a lambda function that must return true when we stop adding bonuses. Function gets three parameters: frameNumber, nPlays, score
        /// </summary>
        /// <param name="lambda">A lambda function</param>
        /// <returns>this for fluent programming style</returns>
        public Bonuses setIsBonusesCompleteLambda(Func<int, int, bool> lambda)
        {
            lambdaFunction = lambda;
            return this;
        }




        // This method is provided by an extension method in the project 'Wiring'.
        // The extension method uses reflection to do the same thing as what you see here
        // The method returns the object it is called on to support fluent coding style
        /*
                public Bonuses WireTo(IConsistsOf c)
                {
                    downStreamFrame = c;
                    return this;
                }
        */





        private bool IsBonusScoringComplete()
        {
            return IsComplete() && (lambdaFunction == null || lambdaFunction(GetnPlays() + bonusBalls, downStreamFrame.GetScore()[0]));
        }






        // Following function implement the IConsistsOf interface



        /// <summary>
        /// Drives the game forward by one play. Use one of the parameters to indicate the result of the play.
        /// </summary>
        /// <param name="player">Which player won the play e.g. in tennis 0 or 1</param>
        /// <param name="score">Or the score on the play e.g. in Bowling the number of pins downed</param>
        public void Ball(int player, int score)
        {
            // This is where all the logic for the abstraction is 
            // We have three things to do
            // 1. Check if we are finished and if so do nothing
            // 2. After the downstream frame completes, add further throws to our local score. Stop when the lambda function says we are complete
            // 3. As uaual pass through the Ball scores downstream

            if (IsBonusScoringComplete()) return;

            if (IsComplete() && !IsBonusScoringComplete())
            {
                bonusScore += score;
                bonusBalls++;
            }

            if (downStreamFrame != null) downStreamFrame.Ball(player, score);

        }





        public bool IsComplete() { return downStreamFrame.IsComplete(); }




 
        public int GetnPlays() { return downStreamFrame.GetnPlays();  }






        int[] IConsistsOf.GetScore()
        {
            int[] s = downStreamFrame.GetScore();
            s[0] += bonusScore;
            return s;
        }





        List<IConsistsOf> IConsistsOf.GetSubFrames()
        {
            return downStreamFrame.GetSubFrames();
        }





        IConsistsOf IConsistsOf.GetCopy(int frameNumber)
        {
            var bs = new Bonuses(frameNumber);
            bs.objectName = this.objectName;
            if (downStreamFrame != null)
            {
                bs.downStreamFrame = downStreamFrame.GetCopy(frameNumber);
            }
            bs.lambdaFunction = this.lambdaFunction;
            return bs as IConsistsOf;
        }






        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name = "); sb.Append(objectName); sb.Append(Environment.NewLine);
            sb.Append("frameNumber = "); sb.Append(frameNumber); sb.Append(Environment.NewLine);
            sb.Append("nBonusBalls = "); sb.Append(bonusBalls); sb.Append(Environment.NewLine);
            sb.Append("bonusScore = "); sb.Append(bonusScore); sb.Append(","); sb.Append(Environment.NewLine);
            sb.Append("Complete = "); sb.Append(IsComplete()); sb.Append(Environment.NewLine);
            sb.Append("ScoringComplete = "); sb.Append(IsBonusScoringComplete()); sb.Append(Environment.NewLine);
            if (downStreamFrame == null) sb.Append("no downstream frame");
            else
            {
                sb.Append("subframes ====>" + Environment.NewLine);
                string sf = downStreamFrame.ToString();
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


