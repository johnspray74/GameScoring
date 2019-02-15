using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoring.ProgrammingParadigms
{
    /// <summary>
    /// A programming Paradigm interface for game scoring. Allows wiring between things that "Consist of" other things. refer Abstraction Layered Architecture document. 
    /// </summary>
    public interface IConsistsOf
    {
        void Ball(int player, int score);

        // Following are for getting the score
        bool IsComplete();

        int[] GetScore();

        int GetnPlays();

        List<IConsistsOf> GetSubFrames();

        IConsistsOf GetCopy(int frameNumber);
    }
}
