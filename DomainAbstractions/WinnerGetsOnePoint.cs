﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameScoring.ProgrammingParadigms;
using System.Text;

namespace GameScoring.DomainAbstractions
{
    /// <summary>
    /// ALA Domain Abstraction. Returns 0,0 until the subgame completes, then converts the subgame score like 6,4 to like 1,0 to the winner.
    /// </summary>
    /// <remarks>
    /// Decorator pattern of the IConsistsOf interface.
    /// Converts an array of two player scores from downstream into a single point to one player when the downstream object completes.
    /// For example, in tennis it is used to convert a set score like 7,6 into a 1,0 to add one point to the players match score.
    /// Decorator pattern of the IConsistsOf interface.
    /// </remarks>
    public class WinnerTakesPoint : IConsistsOf
    {
        private string objectName;  // used to identify objects druing debugging (e.g. can be used to compare before Console.Writeline) Becasue of ALA use of abstractions, instances must be identifiable during debug


        // local state consists of our single downstream frame, the frame score, the state of the frame (completed), and the number of plays so far within the frame.
        private IConsistsOf downStreamFrame; // 

        // state of the game variables
        private readonly int frameNumber = 0;           // This is where our Frame is in the sequence of Frames (sometimes the lambda expressions may want to use this)


        /// <summary>
        /// ALA Domain Abstraction. Returns 0,0 until the subgame completes, then converts the subgame score like 6,4 to like 1,0 to the winner.
        /// Decorator pattern of the IConsistsOf interface.
        /// </summary>
        /// <param name="name">Used to identify the instance during debugging</param>
        public WinnerTakesPoint(string name)
        {
            objectName = name;
        }




        public WinnerTakesPoint(int frameNumber)
        {
            this.frameNumber = frameNumber;
        }



        // This method is provided by an extension method in the project 'Wiring'.
        // The extension method uses reflection to do the same thing
        // The method returns the object it is called on to support fluent coding style
        /*
        public WinnerTakesPoint WireTo(IConsistsOf c)
        {
            downStreamFrame = c;
            return this;
        }
        */


        public void Ball(int player, int score)
        {
            // This is where all the logic for the abstraction is 
            // 1. Nothing if the downstrean frame has already completed
            // 2. Pass the ball to the downstream frame 
            if (IsComplete()) return;
            if (downStreamFrame != null) downStreamFrame.Ball(player, score);
        }




        public bool IsComplete() { return downStreamFrame.IsComplete(); }


        public int GetnPlays() { return downStreamFrame.GetnPlays(); }

        public int[] GetScore()
        {
            // scoring algorthm is 1 point if subframe won
            // determine which player had the higher score and give that player one point in our local score state
            int[] localScore = { 0, 0 };            // local score for our Frame
            if (IsComplete())
            { 
                int[] s = downStreamFrame.GetScore();
                if (s[0] > s[1]) localScore[0]++;
                if (s[0] < s[1]) localScore[1]++;
            }
            return localScore;
        }

        List<IConsistsOf> IConsistsOf.GetSubFrames()
        {
            return new List<IConsistsOf> { downStreamFrame };
        }



        IConsistsOf IConsistsOf.GetCopy(int frameNumber)
        {
            var bs = new WinnerTakesPoint(frameNumber);
            bs.objectName = this.objectName;
            if (downStreamFrame != null)
            {
                bs.downStreamFrame = downStreamFrame.GetCopy(frameNumber);
            }
            return bs as IConsistsOf;
        }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name = "); sb.Append(objectName); sb.Append(Environment.NewLine);
            sb.Append("frameNumber = "); sb.Append(frameNumber); sb.Append(Environment.NewLine);
            sb.Append("localScore = "); sb.Append(GetScore()[0]); sb.Append(","); sb.Append(GetScore()[1]); sb.Append(Environment.NewLine);
            sb.Append("FrameComplete = "); sb.Append(IsComplete()); sb.Append(Environment.NewLine);
            if (downStreamFrame == null) sb.Append("no downstream frame");
            else
            {
                sb.Append("===================" + Environment.NewLine);
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


