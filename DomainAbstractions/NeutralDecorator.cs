using System;
using System.Collections.Generic;
using System.Linq;
using GameScoring.ProgrammingParadigms;
using System.Text;

namespace DomainAbstractions
{
    /// <summary>
    /// A do nothing template decorator pattern of the IConsistsOf interface for use in making other abstractions.
    /// </summary>
    /// <remarks>
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
    /// </remarks>
    public class NeutralDecorator : IConsistsOf
    {
        private string objectName;                      // Just used to identify objects during debugging. (Becasue ALA makes many instances from abstractions, it is useful for them to be identifiable during debug)
        private readonly int frameNumber = 0;           // This is our child number of our parent, also useful to identify instances (sometimes a lambda expressions may want to use this)
        private IConsistsOf downStream; 




        public NeutralDecorator(string name)
        {
             objectName = name;  
        }




        public NeutralDecorator(int frameNumber)
        {
            this.frameNumber = frameNumber;
        }





        // This method is provided by an extension method in the project 'Wiring'.
        // The extension method uses reflection to do the same thing
        // The method returns the object it is called on to support fluent coding style
        /*
        public NeutralDecorator WireTo(IConsistsOf c)
        {
            downStreamFrame = c;
            return this;
        }
        */





        public void Ball(int player, int score)
        {
            if (downStream != null) downStream.Ball(player, score);
        }





        public bool IsComplete()
        {
            return downStream.IsComplete();
        }







        public int GetnPlays() { return downStream.GetnPlays(); }





        public int[] GetScore() { return downStream.GetScore(); }



 

        List<IConsistsOf> IConsistsOf.GetSubFrames()
        {
            return downStream.GetSubFrames();
        }





        IConsistsOf IConsistsOf.GetCopy(int frameNumber)
        {
            // Copy both the decorator and the object it's conencted to
            var bs = new NeutralDecorator(frameNumber);
            if (downStream != null)
            {
                bs.downStream = downStream.GetCopy(frameNumber);
            }
            return bs as IConsistsOf;
        }





        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("frameNumber = "); sb.Append(frameNumber); sb.Append(Environment.NewLine);
            sb.Append("nPlays = "); sb.Append(GetnPlays()); sb.Append(Environment.NewLine);
            sb.Append("localScore = "); sb.Append(GetScore()[0]); sb.Append(","); sb.Append(GetScore()[1]); sb.Append(Environment.NewLine);
            sb.Append("ourFrameComplete = "); sb.Append(IsComplete()); sb.Append(Environment.NewLine);
            if (downStream == null) sb.Append("no downstream frame");
            else
            {
                sb.Append("===================" + Environment.NewLine);
                string sf = downStream.ToString();
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


