using System;
using System.Collections.Generic;
using System.Linq;
using GameScoring.ProgrammingParadigms;
using System.Text;

namespace GameScoring.DomainAbstractions
{
    /// <summary>
    /// Prerequisites to understanding
    /// To understand the full background and reasoning behind this abstraction, you need to know about ALA which is explained here 'abstractionlayeredarchitecture.com'
    /// Additional prerequisite knowledge is the Decorator pattern, which along with composite is the most common pattern in ALA (displacing the Observer (Publish/Subscribe) pattern.) 
    /// A decorator implements and accepts the same interface, and usually passes most methods straight through
    /// so it can be inserted between any other two abstractions that are connected by that interface without affecting operation
    /// except by modifying the behaviour on that interface in one specific way.
    /// 
    /// NeutralDecorator is not a useful abstraction in itself - it is an inert decorator of the IConsistsOf<TScore> interface
    /// - that is it can be inserted between any two objects wired using the IConsists interface and it doesn't change the behaviour.
    /// - all methods are effectively passed through
    /// It is used as a copy/paste starting point for making new abstractions e.g. Bonuses and WinnerGetsOnePoint
    /// </summary>
    public class Switch : IConsistsOf
    {
        private string objectName;                      // Just used to identify objects druing debugging. Becasue ALA makes many instances from abstractions, it is useful for them to be identifiable during debug  (e.g. can be used to compare before Console.Writeline)
        private readonly int frameNumber = 0;           // This is our child number of our parent, also useful to identify instances (sometimes a lambda expressions may want to use this)


        // local state consists of our two downstream IConsistOf objects
        private IConsistsOf downStreamFrame1;          // 
        private IConsistsOf downStreamFrame2;          // 

        // configurations for the abstraction
        private Func<int, int, int[], bool> isSwitchCompleteLambda;



        public Switch(string name)
        {
            objectName = name;
        }




        public Switch(int frameNumber)
        {
            this.frameNumber = frameNumber;
        }



        // fluent setters are used for the Lambda functions that configure this class and for wiring to other instances
        public Switch setSwitchLambda(Func<int, int, int[], bool> lambda)
        {
            isSwitchCompleteLambda = lambda;
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


        private bool IsComplete()
        {
            if (IsSwitched())
            {
                return downStreamFrame2.IsComplete();
            }
            else
            {
                return downStreamFrame1.IsComplete();
            }
        }


        bool IConsistsOf.IsComplete() { return IsComplete(); }

        private bool IsSwitched()
        {
            return isSwitchCompleteLambda!=null && isSwitchCompleteLambda(frameNumber, downStreamFrame1.GetnPlays(), downStreamFrame1.GetScore());
        }


        private int GetnPlays()
        {
            if (IsSwitched())
            {
                return downStreamFrame2.GetnPlays();
            }
            else
            {
                return downStreamFrame1.GetnPlays();
            }
        }

        int IConsistsOf.GetnPlays() { return GetnPlays(); }

        private int[] GetScore()
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

        int[] IConsistsOf.GetScore()
        {
            return GetScore();
        }


        List<IConsistsOf> IConsistsOf.GetSubFrames()
        {
            IConsistsOf downstreamframe;
            if (IsSwitched())
            {
                downstreamframe = downStreamFrame2;
            }
            else
            {
                downstreamframe = downStreamFrame1;
            }
            return new List<IConsistsOf> { downstreamframe };
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
            sw.isSwitchCompleteLambda = this.isSwitchCompleteLambda;
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


