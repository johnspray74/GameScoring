using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoring.Application
{
    public interface ITestTennis
    {
        int NSets();

        string[] GetLastGameScore();

        int[] GetMatchScore();

        List<int[]> GetSetScores();

        void Play(int result);



        bool IsComplete();


        int GetTotalScore();
    }
}
