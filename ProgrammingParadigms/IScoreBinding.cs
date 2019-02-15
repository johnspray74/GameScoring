using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoring.ProgrammingParadigms
{
    interface IScoreBinding
    {
        string Label { get; }
        string GetScore();
        string GetScore(int x);
        string GetScore(int x, int y);
    }
}
