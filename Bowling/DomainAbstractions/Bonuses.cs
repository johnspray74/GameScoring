using System;
using System.Collections.Generic;
using System.Linq;
using ProgrammingParadigms;
using System.Text;

namespace DomainAbstractions
{
    /// <summary>
    /// Prerequisites to understanding
    /// To understand the full background and reasoning behind this abstraction, you need to know about ALA which is explained here 'abstractionlayeredarchitecture.com'
    /// Particular prerequisite knowledge is the Decorator pattern, which is the most common pattern in ALA (displacing the Observer (Publish/Subscribe) pattern.) 
    /// A decorator implements and accepts the same interface, and just passes most methods straight through
    /// so it can be inserted between any other two abstractions that are connected by that interface without affecting operation except by modifying the behaviour on that interface in one specific way.
    /// 
    /// Bonuses is a domain abstraction and a Decorator that implements and accepts the IConsistsOf<TScore> interface. It always has one child.
    /// The modification is to add bonues to the score.
    /// It does this only after the IsComplete method of the downstream child has returned true.
    /// The IsComplete method result returned from downstream is passed upstream normally, but it is assumed that ball scores are still received. 
    /// These are added to the local score until the local IsComplete lambda function returns true.
    /// For example, in 10-pin bowling you would use this with a lambda of "score<10 || plays==3" so that after the downstream Frame completes, it will keep adding ball scores if the score is already 10, until the total throws is 3.
    /// The lambda is a function of the state of the frame, e.g. plays/score.


    public class Bonuses : IConsistsOf
    {
        private string objectName;  // used to identify objects during debugging (e.g. can be used to compare before Console.Writeline) Becasue of ALA use of abstractions, instances must be identifiable during debug


        // configurations for the abstraction
        private Func<int, int, bool> isBonusesComplete;

        // local state consists of our single downstream frame, and the bonus score.
        private IConsistsOf downStreamFrame; 



        // state of the game variables
        private readonly int frameNumber = 0;           // This is where our Frame is in the sequence of Frames (sometimes the lambda expressions may want to use this)
        private int bonusScore;
        private int bonusBalls;

        public Bonuses(string name)
        {
            objectName = name;
        }


        public Bonuses(int frameNumber)
        {
            this.frameNumber = frameNumber;
        }
        
        
        // fluent setters are used for the Lambda functions that configure this class and for wiring to other instances
        public Bonuses setIsBonusesCompleteLambda(Func<int, int, bool> lambda)
        {
            isBonusesComplete = lambda;
            return this;
        }




        // This method is provided by an extension method in the project 'Wiring'.
        // The extension method uses reflection to do the same thing
        // The method returns the object it is called on to support fluent coding style
/*
        public Bonuses WireTo(IConsistsOf c)
        {
            downStreamFrame = c;
            return this;
        }
*/

 
        // This is where all the logic for the abstraction is 
        // We have three things to do
        // 1. Pass through the Ball function to our downstream frame
        // 2. Call our local lambda to see if bonuses are complete
        // 3. After the downstream frame completes, if bonuses are still pending, add further throws to our local score.

        public void Ball(int player, int score)
        {
            if (IsScoringComplete()) return;

            if (IsComplete() && !IsScoringComplete())
            {
                bonusScore += score;
                bonusBalls++;
            }

            if (downStreamFrame != null) downStreamFrame.Ball(player, score);

        }



        private bool IsScoringComplete()
        {
            return IsComplete() && (isBonusesComplete==null || isBonusesComplete(GetnPlays()+bonusBalls, downStreamFrame.GetScore()[0]));
        }

        private bool IsComplete() { return downStreamFrame.IsComplete(); }

        bool IConsistsOf.IsComplete() { return IsComplete(); }
 
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
            bs.isBonusesComplete = this.isBonusesComplete;
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
            sb.Append("ScoringComplete = "); sb.Append(IsScoringComplete()); sb.Append(Environment.NewLine);
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


