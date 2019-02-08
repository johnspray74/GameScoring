using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingParadigms
{
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
