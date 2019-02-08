using System;
using System.Collections.Generic;
using System.Linq;
using ProgrammingParadigms;
using System.Text;

namespace DomainAbstractions
{
    /// <summary>
    /// To understand the full background and reasoning behind this abstraction, you need to know about ALA which is explained here 'abstractionlayeredarchitecture.com'
    /// 
    /// SinglePlay is a domain abstraction for the leaf nodes that records the score of a single play.
    /// It cannot have any children like the GrameFrame or Bonus abstractions can.
    /// In terms of functional programming, it doesn't pass function calls onto children, it returns local state.
    /// It always completes after one play. It does not take a completion lambda like some of the other domain abstractions.


    public class SinglePlay : IConsistsOf
    {


        // configurations for the abstraction
        private const int nPlayers = 2;   // TBD make this configurable
        private string objectName;  // used to identify objects druing debugging (e.g. can be used to compare before Console.Writeline)


        // At run-time, SinglePlay objects are at the leaf nodes of a the tree structure.
        // The first SinglePlay object is created by the application wiring, but during the running of the game more are created as need using the prototype pattern (copying the object)

        // local state consists of the score for the single play, and a boolean to indicate that we have done the play.



        // state of the game 
        private int[] localScore = { 0, 0 };            // local score for each player (typically only one player will score).
        private bool complete = false;                  // Goes true when we get one play score.


        public SinglePlay(string name)
        {
            // used for debugging only - not part of the primary functionality
            objectName = name;                  // used for object identification during debugging
        }

        // used for debugging only - not part of the primary functionality
        private readonly int childNumber = 0;           // object identification - which child are we to our parent.
        public SinglePlay(int childNumber)
        {
            this.childNumber = childNumber;     
        }

        // used for debugging only - not part of the primary functionality
        // used to accumulate local debug information to be printed out later by the ToString method
        private StringBuilder debug = new StringBuilder();


        // This is where all the logic for the abstraction is 
        // 1. Update the score and game state becomes complete
        public void Ball(int player, int score)
        {
            if (complete) return;
            localScore[player] += score;
            complete = true;
        }





        bool IConsistsOf.IsComplete()
        {
            return complete;
        }

        public int GetnPlays()
        {
            if (complete) return 1; else return 0;
        }

        int[] IConsistsOf.GetScore()
        {
            return localScore;
        }



        List<IConsistsOf> IConsistsOf.GetSubFrames()
        {
            return null;
        }




        IConsistsOf IConsistsOf.GetCopy(int frameNumber)
        {
            var gf = new SinglePlay(frameNumber);
            gf.objectName = this.objectName;
            return gf as IConsistsOf;
        }



        // used for debugging only - not part of the primary functionality
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name = "); sb.Append(objectName); sb.Append(Environment.NewLine);
            // sb.Append("childNumber = "); sb.Append(childNumber); sb.Append(Environment.NewLine);
            sb.Append("localScore = "); sb.Append(localScore[0]); sb.Append(","); sb.Append(localScore[1]); sb.Append(Environment.NewLine);
            // sb.Append("complete = "); sb.Append(complete); sb.Append(Environment.NewLine);
            // sb.Append("debug = "); sb.Append(debug); sb.Append(Environment.NewLine);
            return sb.ToString();
        }

    }
}


