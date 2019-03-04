using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GameScoring.Application;

namespace Tests
{

    [TestClass]
    public class TestScoringEngineBowling
    {
        ITestBowling game;

        [TestInitialize]
        public void Setup()
        {
            game = new GameScoring.Application.Bowling();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }


        [TestMethod]
        public void Test01OneBall()
        {
            game.Play(2);
            Assert.AreEqual(2, game.GetTotalScore());
        }

        [TestMethod]
        public void Test02TwoBalls()
        {
            Assert.AreEqual(0, game.NFrames());
            game.Play(2);
            Assert.AreEqual(1, game.NFrames());
            game.Play(3);
            Assert.AreEqual(5, game.GetTotalScore());
            Assert.AreEqual(1, game.NFrames());
            Assert.AreEqual(5, game.GetAccumulatedFrameScores()[0]);
        }

        [TestMethod]
        public void test02Zeros()
        {
            for (int i = 0; i < 20; i++)
            {
                game.Play(0);
            }
            Assert.AreEqual(10, game.NFrames());
            Assert.AreEqual(0, game.GetTotalScore());

            // Test the accumulating frame by frame scores
            Assert.AreEqual(0, game.GetAccumulatedFrameScores()[0]);
            Assert.AreEqual(0, game.GetAccumulatedFrameScores()[1]);
            Assert.AreEqual(0, game.GetAccumulatedFrameScores()[9]);
            // Test the individual ball scores (index is the frame, value is a list of ball scores)
            Assert.AreEqual(2, game.GetFrameThrowScores()[0].Count());
            Assert.AreEqual(0, game.GetFrameThrowScores()[0][0]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[1][0]);
            Assert.AreEqual(2, game.GetFrameThrowScores()[1].Count);
            Assert.AreEqual(0, game.GetFrameThrowScores()[1][0]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[1][1]);
            Assert.AreEqual(2, game.GetFrameThrowScores()[9].Count);
            Assert.AreEqual(0, game.GetFrameThrowScores()[9][0]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[9][1]);
        }

        [TestMethod]
        public void test03Spares()
        {
            for (int i = 0; i < 21; i++)
            {
                game.Play(5);
            }
            Console.WriteLine(game);
            // Test the accumulating frame by frame scores
            Assert.AreEqual(10, game.NFrames());
            Assert.AreEqual(15, game.GetAccumulatedFrameScores()[0]);
            Assert.AreEqual(30, game.GetAccumulatedFrameScores()[1]);
            // Test the individual ball scores (index is the frame, value is a list of ball scores)
            Assert.AreEqual(2, game.GetFrameThrowScores()[0].Count());
            Assert.AreEqual(5, game.GetFrameThrowScores()[0][0]);
            Assert.AreEqual(5, game.GetFrameThrowScores()[1][0]);
            Assert.AreEqual(2, game.GetFrameThrowScores()[1].Count);
            Assert.AreEqual(5, game.GetFrameThrowScores()[1][0]);
            Assert.AreEqual(5, game.GetFrameThrowScores()[1][1]);
            Assert.AreEqual(3, game.GetFrameThrowScores()[9].Count);
            Assert.AreEqual(5, game.GetFrameThrowScores()[9][0]);
            Assert.AreEqual(5, game.GetFrameThrowScores()[9][1]);
            Assert.AreEqual(5, game.GetFrameThrowScores()[9][2]);
            Assert.AreEqual(150, game.GetAccumulatedFrameScores()[9]);
            Assert.AreEqual(150, game.GetTotalScore());
        }
        [TestMethod]
        public void test04Strikes()
        {
            for (int i = 0; i < 12; i++)
            {
                game.Play(10);
            }
            Assert.AreEqual(300, game.GetTotalScore());
            // Test the accumulating frame by frame scores
            Assert.AreEqual(10, game.NFrames());
            Assert.AreEqual(30, game.GetAccumulatedFrameScores()[0]);
            Assert.AreEqual(60, game.GetAccumulatedFrameScores()[1]);
            Assert.AreEqual(300, game.GetAccumulatedFrameScores()[9]);
            // Test the individual ball scores (1st index is the frame, 2nd index is the ball)
            Assert.AreEqual(1, game.GetFrameThrowScores()[0].Count);
            Assert.AreEqual(10, game.GetFrameThrowScores()[0][0]);
            Assert.AreEqual(1, game.GetFrameThrowScores()[1].Count);
            Assert.AreEqual(10, game.GetFrameThrowScores()[1][0]);
            Assert.AreEqual(3, game.GetFrameThrowScores()[9].Count);
            Assert.AreEqual(10, game.GetFrameThrowScores()[9][0]);
            Assert.AreEqual(10, game.GetFrameThrowScores()[9][1]);
            Assert.AreEqual(10, game.GetFrameThrowScores()[9][2]);
        }

        // Robert Martin's Tests

        [TestMethod]
        public void test01TwoBallsNoMark()
        {
            game.Play(5);
            game.Play(4);
            Assert.AreEqual(9, game.GetTotalScore());
        }

        [TestMethod]
        public void test02FourBallsNoMark()
        {
            game.Play(5);
            game.Play(4);
            game.Play(7);
            game.Play(2);
            Assert.AreEqual(9, game.GetAccumulatedFrameScores()[0]);
            Assert.AreEqual(18, game.GetAccumulatedFrameScores()[1]);
            Assert.AreEqual(18, game.GetTotalScore());
        }

        [TestMethod]
        public void test02SixBallsNoMark()
        {
            game.Play(1);
            game.Play(2);
            game.Play(1);
            game.Play(3);
            game.Play(1);
            // Test the accumulating frame by frame scores
            Assert.AreEqual(3, game.NFrames());
            Assert.AreEqual(3, game.GetAccumulatedFrameScores()[0]);
            Assert.AreEqual(7, game.GetAccumulatedFrameScores()[1]);
            Assert.AreEqual(8, game.GetAccumulatedFrameScores()[2]);
            Assert.AreEqual(8, game.GetTotalScore());
        }

        [TestMethod]
        public void test03SimpleSpare()
        {
            game.Play(3);
            game.Play(7);
            game.Play(4);
            Assert.AreEqual(14, game.GetAccumulatedFrameScores()[0]);
            Assert.AreEqual(18, game.GetAccumulatedFrameScores()[1]);
            Assert.AreEqual(18, game.GetTotalScore());
        }

        [TestMethod]
        public void test04SimpleFrameAfterSpare()
        {
            game.Play(9);
            game.Play(1);
            game.Play(4);
            game.Play(2);
            Assert.AreEqual(14, game.GetAccumulatedFrameScores()[0]);
            Assert.AreEqual(20, game.GetAccumulatedFrameScores()[1]);
            Assert.AreEqual(20, game.GetTotalScore());
        }

        [TestMethod]
        public void test05SimpleStrike()
        {
            game.Play(10);
            game.Play(3);
            game.Play(6);
            Assert.AreEqual(19, game.GetAccumulatedFrameScores()[0]);
            Assert.AreEqual(28, game.GetAccumulatedFrameScores()[1]);
            Assert.AreEqual(28, game.GetTotalScore());
        }

        [TestMethod]
        public void test06PerfectGame()
        {
            for (int i = 0; i < 12; i++)
            {
                game.Play(10);
            }
            Assert.AreEqual(300, game.GetTotalScore());
        }

        [TestMethod]
        public void test07EndOfArray()
        {
            for (int i = 0; i < 9; i++)
            {
                game.Play(0);
                game.Play(0);
            }
            game.Play(2);
            game.Play(8); // 10th frame spare
            game.Play(10); // Strike in last position of array.
            Assert.AreEqual(20, game.GetTotalScore());
        }

        [TestMethod]
        public void test08AlternateZeroFrames()
        {
            game.Play(0);
            game.Play(0);
            game.Play(5);
            game.Play(5);
            game.Play(0);
            game.Play(0);
            game.Play(5);
            game.Play(5);
            game.Play(0);
            game.Play(1);
            game.Play(10);
            game.Play(0);
            game.Play(0);
            game.Play(10);
            game.Play(0);
            game.Play(1);
            game.Play(10);
            game.Play(0);
            game.Play(0);
            Assert.AreEqual(53, game.GetTotalScore());
            // Test the accumulating frame by frame scores
            Assert.AreEqual(10, game.NFrames());
            Assert.AreEqual(0, game.GetAccumulatedFrameScores()[0]);
            Assert.AreEqual(10, game.GetAccumulatedFrameScores()[1]);
            Assert.AreEqual(10, game.GetAccumulatedFrameScores()[2]);
            Assert.AreEqual(20, game.GetAccumulatedFrameScores()[3]);
            Assert.AreEqual(21, game.GetAccumulatedFrameScores()[4]);
            Assert.AreEqual(31, game.GetAccumulatedFrameScores()[5]);
            Assert.AreEqual(31, game.GetAccumulatedFrameScores()[6]);
            Assert.AreEqual(42, game.GetAccumulatedFrameScores()[7]);
            Assert.AreEqual(43, game.GetAccumulatedFrameScores()[8]);
            Assert.AreEqual(53, game.GetAccumulatedFrameScores()[9]);
            // Test the individual ball scores (1st index is the frame, 2nd index is the ball)
            Assert.AreEqual(2, game.GetFrameThrowScores()[0].Count);
            Assert.AreEqual(2, game.GetFrameThrowScores()[1].Count);
            Assert.AreEqual(2, game.GetFrameThrowScores()[2].Count);
            Assert.AreEqual(2, game.GetFrameThrowScores()[3].Count);
            Assert.AreEqual(2, game.GetFrameThrowScores()[4].Count);
            Assert.AreEqual(1, game.GetFrameThrowScores()[5].Count);
            Assert.AreEqual(2, game.GetFrameThrowScores()[6].Count);
            Assert.AreEqual(1, game.GetFrameThrowScores()[7].Count);
            Assert.AreEqual(2, game.GetFrameThrowScores()[8].Count);
            Assert.AreEqual(3, game.GetFrameThrowScores()[9].Count);
            Assert.AreEqual(0, game.GetFrameThrowScores()[0][0]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[0][1]);
            Assert.AreEqual(5, game.GetFrameThrowScores()[1][0]);
            Assert.AreEqual(5, game.GetFrameThrowScores()[1][1]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[2][0]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[2][1]);
            Assert.AreEqual(5, game.GetFrameThrowScores()[3][0]);
            Assert.AreEqual(5, game.GetFrameThrowScores()[3][1]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[4][0]);
            Assert.AreEqual(1, game.GetFrameThrowScores()[4][1]);
            Assert.AreEqual(10, game.GetFrameThrowScores()[5][0]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[6][0]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[6][1]);
            Assert.AreEqual(10, game.GetFrameThrowScores()[7][0]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[8][0]);
            Assert.AreEqual(1, game.GetFrameThrowScores()[8][1]);
            Assert.AreEqual(10, game.GetFrameThrowScores()[9][0]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[9][1]);
            Assert.AreEqual(0, game.GetFrameThrowScores()[9][2]);
        }


        [TestMethod]
        public void test08aSampleGame()
        {
            game.Play(1);
            game.Play(4);
            game.Play(4);
            game.Play(5);
            game.Play(6);
            game.Play(4);
            game.Play(5);
            game.Play(5);
            game.Play(10);
            game.Play(0);
            game.Play(1);
            game.Play(7);
            game.Play(3);
            game.Play(6);
            game.Play(4);
            game.Play(10);
            game.Play(2);
            game.Play(8);
            game.Play(6);
            Assert.AreEqual(133, game.GetTotalScore());
            // Test the accumulating frame by frame scores
            Assert.AreEqual(10, game.NFrames());
            Assert.AreEqual(5, game.GetAccumulatedFrameScores()[0]);
            Assert.AreEqual(29, game.GetAccumulatedFrameScores()[2]);
            Assert.AreEqual(60, game.GetAccumulatedFrameScores()[4]);
            Assert.AreEqual(117, game.GetAccumulatedFrameScores()[8]);
            // Test the individual ball scores (1st index is the frame, 2nd index is the ball)
            Assert.AreEqual(2, game.GetFrameThrowScores()[0].Count);
            Assert.AreEqual(2, game.GetFrameThrowScores()[1].Count);
            Assert.AreEqual(2, game.GetFrameThrowScores()[2].Count);
            Assert.AreEqual(1, game.GetFrameThrowScores()[4].Count);
            Assert.AreEqual(2, game.GetFrameThrowScores()[5].Count);
            Assert.AreEqual(1, game.GetFrameThrowScores()[8].Count);
            Assert.AreEqual(3, game.GetFrameThrowScores()[9].Count);
            Assert.AreEqual(1, game.GetFrameThrowScores()[0][0]);
            Assert.AreEqual(4, game.GetFrameThrowScores()[0][1]);
            Assert.AreEqual(6, game.GetFrameThrowScores()[2][0]);
            Assert.AreEqual(4, game.GetFrameThrowScores()[2][1]);
            Assert.AreEqual(10, game.GetFrameThrowScores()[4][0]);
            Assert.AreEqual(10, game.GetFrameThrowScores()[8][0]);
            Assert.AreEqual(2, game.GetFrameThrowScores()[9][0]);
            Assert.AreEqual(8, game.GetFrameThrowScores()[9][1]);
            Assert.AreEqual(6, game.GetFrameThrowScores()[9][2]);
        }

        [TestMethod]
        public void test09HeartBreak()
        {
            for (int i = 0; i < 11; i++)
                game.Play(10);
            game.Play(9);
            Assert.AreEqual(299, game.GetTotalScore());
        }

        [TestMethod]
        public void test10TenthFrameSpare()
        {
            for (int i = 0; i < 9; i++)
                game.Play(10);
            game.Play(9);
            game.Play(1);
            game.Play(1);
            Assert.AreEqual(270, game.GetTotalScore());
        }


        // Various Full Games

        [TestMethod]
        public void testVariousFullGames01()
        {
            TestFullGame(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 0);
        }
        [TestMethod]
        public void testVariousFullGames02()
        {
            TestFullGame(new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, 20);
        }
        [TestMethod]
        public void testVariousFullGames03()
        {
            TestFullGame(new int[] {10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10}, 300);
        }
        [TestMethod]
        public void testVariousFullGames04()
        {
            TestFullGame(new int[] {5,5,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, 16);
        }
        [TestMethod]
        public void testVariousFullGames05()
        {
            TestFullGame(new int[] {10,3,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, 24);
        }
        [TestMethod]
        public void testVariousFullGames06()
        {
            TestFullGame(new int[] { 10, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 24);
        }
        [TestMethod]
        public void testVariousFullGames07()
        {
            TestFullGame(new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,1 }, 11);  // tenth frame spare
        }
        [TestMethod]
        public void testVariousFullGames08()
        {
            TestFullGame(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 1, 1 }, 12);  // tenth frame strike
        }
        [TestMethod]
        public void testVariousFullGames09()
        {
            TestFullGame(new int[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 }, 133);  // example game from katas
        }

        private void TestFullGame(int[] ba, int CorrectGameScore)
        {
            List<int> balls = new List<int>(ba);
            balls.ForEach(pins => game.Play(pins));
            Assert.AreEqual(game.GetTotalScore(),CorrectGameScore);
            Assert.AreEqual(10, game.NFrames());
            Assert.IsTrue(game.IsComplete());
        }
    }
}
