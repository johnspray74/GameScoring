using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoring.ProgrammingParadigms
{
    interface IGame
    {
        void Play(int result);
        bool IsComplete();
        string GetScore();
    }
}
