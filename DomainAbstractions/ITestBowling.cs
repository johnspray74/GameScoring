using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoring.Application
{
    public interface ITestBowling
    {
        // get the individual pin scores for all throws orf all Frames
        List<List<int>> GetFrameThrowScores();

        // Get the accumulating frame scores
        List<int> GetAccumulatedFrameScores();


        int NFrames();


        void Play(int result);



        bool IsComplete();


        int GetTotalScore();


    }
}
