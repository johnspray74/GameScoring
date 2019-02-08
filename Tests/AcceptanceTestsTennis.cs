﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application;

namespace Tests
{
    [TestClass]
    public class AcceptanceTestsTennis1
    {
        Tennis game;

        [TestInitialize]
        public void Setup()
        {
            game = new Tennis();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }


        // Test game scoring

        [TestMethod]
        public void TestSimpleGameScoring0()
        {
            Assert.AreEqual("", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("15,love", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("30,love", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("40,love", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("win 0", game.GetGameScore());
        }

        [TestMethod]
        public void TestSimpleGameScoring1()
        {
            game.Play(1);
            Assert.AreEqual("love,15", game.GetGameScore());
            game.Play(1);
            Assert.AreEqual("love,30", game.GetGameScore());
            game.Play(1);
            Assert.AreEqual("love,40", game.GetGameScore());
            game.Play(1);
            Assert.AreEqual("win 1", game.GetGameScore());
        }

        [TestMethod]
        public void TestSimpleAlternating0()
        {
            game.Play(0);
            game.Play(1);
            Assert.AreEqual("15,15", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("30,15", game.GetGameScore());
            game.Play(1);
            Assert.AreEqual("30,30", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("40,30", game.GetGameScore());
            game.Play(1);
            Assert.AreEqual("deuce", game.GetGameScore());
            game.Play(1);
            Assert.AreEqual("adv 1", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("deuce", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("adv 0", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("win 0", game.GetGameScore());
        }

        [TestMethod]
        public void TestSimpleAlternating1()
        {
            game.Play(1);
            game.Play(0);
            Assert.AreEqual("15,15", game.GetGameScore());
            game.Play(1);
            Assert.AreEqual("15,30", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("30,30", game.GetGameScore());
            game.Play(1);
            Assert.AreEqual("30,40", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("deuce", game.GetGameScore());
            game.Play(0);
            Assert.AreEqual("adv 0", game.GetGameScore());
            game.Play(1);
            Assert.AreEqual("deuce", game.GetGameScore());
            game.Play(1);
            Assert.AreEqual("adv 1", game.GetGameScore());
            game.Play(1);
            Assert.AreEqual("win 1", game.GetGameScore());
        }


        // Test set scoring

        public void PlayPoints(int[] winners)
        {
            foreach (int w in winners) game.Play(w);
        }


        public void PlayGame(int winner)
        {
            if (winner == 0)
            {
                PlayPoints(new int[] { 1, 0, 1, 0, 0, 0 }); 
            }
            else
            {
                PlayPoints(new int[] { 1, 0, 1, 0, 1, 1 });
            }
        }


        [TestMethod]
        public void TestSimpleSetScoring0()
        {
            Assert.AreEqual(0, game.GetSetScores().Count);
            PlayGame(0);
            Assert.AreEqual(1, game.GetSetScores()[0][0]);
            Assert.AreEqual(0, game.GetSetScores()[0][1]);
            PlayGame(0);
            Assert.AreEqual(2, game.GetSetScores()[0][0]);
            PlayGame(0);
            Assert.AreEqual(3, game.GetSetScores()[0][0]);
            PlayGame(0);
            Assert.AreEqual(4, game.GetSetScores()[0][0]);
            PlayGame(0);
            Assert.AreEqual(5, game.GetSetScores()[0][0]);
            PlayGame(0);
            Assert.AreEqual(6, game.GetSetScores()[0][0]);
            PlayGame(0);
            Assert.AreEqual(6, game.GetSetScores()[0][0]);
            Assert.AreEqual(1, game.GetSetScores()[1][0]);
            PlayGame(0);
            Assert.AreEqual(6, game.GetSetScores()[0][0]);
            Assert.AreEqual(2, game.GetSetScores()[1][0]);
            Assert.AreEqual(0, game.GetSetScores()[0][1]);
            Assert.AreEqual(0, game.GetSetScores()[1][1]);
        }



        public void PlayGames(int[] gameWinners)
        {
            foreach (int w in gameWinners) PlayGame(w);
        }

        public void CheckSetScores(int[][] setScores)
        {
            int set = 0;
            foreach (int[] ss in setScores)
            {

                Assert.AreEqual(ss[0], game.GetSetScores()[set][0]);
                Assert.AreEqual(ss[1], game.GetSetScores()[set][1]);
                set++;
            }

        }

        [TestMethod]
        public void TestStartSet()
        {
            CheckSetScores(new int[][] { });
            game.Play(0);
            CheckSetScores(new int[][] { new int[] { 0, 0 } });
        }

        [TestMethod]
        public void TestFullGameNoTiebreaks()
        {
            PlayGames(new int[] { 0, 0, 0, 0, 0, });
            CheckSetScores(new int[][] { new int[] { 5, 0 } });
            PlayGames(new int[] { 0 });
            CheckSetScores(new int[][] { new int[] { 6, 0 } });
            PlayGames(new int[] { 1, 1, 1, 1, 1 });
            CheckSetScores(new int[][] { new int[] { 6, 0 }, new int[] { 0, 5 } });
            PlayGames(new int[] { 1 });
            CheckSetScores(new int[][] { new int[] { 6, 0 }, new int[] { 0, 6 } });
            PlayGames(new int[] { 0, 0, 0, 0, 0, 0 });
            CheckSetScores(new int[][] { new int[] { 6, 0 }, new int[] { 0, 6 }, new int[] { 6, 0 } });
            PlayGames(new int[] { 1, 1, 1, 1, 1, 1 });
            CheckSetScores(new int[][] { new int[] { 6, 0 }, new int[] { 0, 6 }, new int[] { 6, 0 }, new int[] { 0, 6 } });
            PlayGames(new int[] { 0, 0, 0, 0, 0, 0 });
            CheckSetScores(new int[][] { new int[] { 6, 0 }, new int[] { 0, 6 }, new int[] { 6, 0 }, new int[] { 0, 6 }, new int[] { 6, 0 } });
            Assert.IsTrue(game.IsComplete());
        }


        [TestMethod]
        public void TestFullGame75s()
        {
            PlayGames(new int[] { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 });
            CheckSetScores(new int[][] { new int[] { 6, 5 } });
            PlayGames(new int[] { 0 });
            CheckSetScores(new int[][] { new int[] { 7, 5 } });
            PlayGames(new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, });
            CheckSetScores(new int[][] { new int[] { 7, 5 }, new int[] { 5, 7 } });
            PlayGames(new int[] { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 });
            CheckSetScores(new int[][] { new int[] { 7, 5 }, new int[] { 5, 7 }, new int[] { 7, 5 } });
            PlayGames(new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, });
            CheckSetScores(new int[][] { new int[] { 7, 5 }, new int[] { 5, 7 }, new int[] { 7, 5 }, new int[] { 5, 7 } });
            PlayGames(new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, });
            CheckSetScores(new int[][] { new int[] { 7, 5 }, new int[] { 5, 7 }, new int[] { 7, 5 }, new int[] { 5, 7 }, new int[] { 5, 7 } });
            Assert.IsTrue(game.IsComplete());
        }



        [TestMethod]
        public void TestTieBreak()
        {
            PlayGames(new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 }); // 6,6
            CheckSetScores(new int[][] { new int[] { 6, 6 } });
            PlayPoints(new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 });
            CheckSetScores(new int[][] { new int[] { 6, 7 } });
            PlayGames(new int[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 }); // 6,6
            CheckSetScores(new int[][] { new int[] { 6, 7 }, new int[] { 6, 6 } });
            PlayPoints(new int[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 });
            CheckSetScores(new int[][] { new int[] { 6, 7 }, new int[] { 7, 6 } });
        }

        [TestMethod]
        public void TestFullGameTieBreaks()
        {
            PlayGames(new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0 }); // 6,6
            CheckSetScores(new int[][] { new int[] { 6, 6 } });
            PlayPoints(new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 });
            CheckSetScores(new int[][] { new int[] { 7, 6 } });
            PlayGames(new int[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 }); // 6,6
            CheckSetScores(new int[][] { new int[] { 7, 6 }, new int[] { 6, 6 } });
            PlayPoints(new int[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1 });
            CheckSetScores(new int[][] { new int[] { 7, 6 }, new int[] { 6, 7 } });
            PlayGames(new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0 }); // 6,6
            PlayPoints(new int[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0 });
            CheckSetScores(new int[][] { new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 7, 6 } });
            PlayGames(new int[] { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1}); // 6,6
            PlayPoints(new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1 });
            CheckSetScores(new int[][] { new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 7, 6 }, new int[] { 6, 7 } });
            PlayGames(new int[] { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1 }); // 6,6
            CheckSetScores(new int[][] { new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 6, 6 } });
            PlayGames(new int[] { 0, 1 });
            CheckSetScores(new int[][] { new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 7, 7 } });
            PlayGames(new int[] { 1, 0 });
            CheckSetScores(new int[][] { new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 8, 8 } });
            PlayGames(new int[] { 0, 0 });
            CheckSetScores(new int[][] { new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 10, 8 } });
            Assert.IsTrue(game.IsComplete());
        }


        // set up some new functions to test games at the set scores level

        public void PlayPoints(int s1, int s2)
        {
            // Play points alternately until the given score
            for (int i = 0; i < s1; i++)
            {
                game.Play(0);
                if (i < s2)
                {
                    game.Play(1);
                }
            }
            for (int i = s1; i < s2; i++)
            {
                game.Play(1);
            }
        }


        public void PlaySet(int s1, int s2)
        {
            if (s1==7 && s2==6 || s1==6 && s2==7)   // tie break set
            {
                for (int i = 0; i < 6; i++)
                {
                    PlayGame(0);
                    PlayGame(1);
                }
                if (s1==7)
                {
                    PlayPoints(7, 0);
                }
                if (s2==7)
                {
                    PlayPoints(0, 7);
                }
            }
            else  // not tie break set
            {
                // Play games alternately until the given score
                for (int i = 0; i < s1; i++)
                {
                    PlayGame(0);
                    if (i < s2)
                    {
                        PlayGame(1);
                    }
                }
                for (int i = s1; i < s2; i++)
                {
                    PlayGame(1);
                }
            }

        }



        [TestMethod]
        public void TestFullGame1()
        {
            PlaySet(6, 4);
            PlaySet(7, 5);
            PlaySet(7, 6);
            CheckSetScores(new int[][] { new int[] { 6, 4 }, new int[] { 7, 5 }, new int[] { 7, 6 } } );
            Assert.IsTrue(game.IsComplete());

        }


        [TestMethod]
        public void TestFullGame2()
        {
            PlaySet(4, 6);
            PlaySet(5, 7);
            PlaySet(6, 7);
            CheckSetScores(new int[][] { new int[] { 4, 6 }, new int[] { 5, 7 }, new int[] { 6, 7 } } );
            Assert.IsTrue(game.IsComplete());

        }


        [TestMethod]
        public void TestFullGame3()
        {
            PlaySet(7, 5);
            PlaySet(5, 7);
            PlaySet(7, 5);
            PlaySet(5, 7);
            PlaySet(7, 5);
            CheckSetScores(new int[][] { new int[] { 7, 5 }, new int[] { 5, 7 }, new int[] { 7, 5 }, new int[] { 5, 7 }, new int[] { 7, 5 } });
            Assert.IsTrue(game.IsComplete());

        }

        [TestMethod]
        public void TestFullGame4()
        {
            PlaySet(7, 6);
            PlaySet(6, 7);
            PlaySet(7, 6);
            PlaySet(6, 7);
            PlaySet(102, 100);
            CheckSetScores(new int[][] { new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 7, 6 }, new int[] { 6, 7 }, new int[] { 102, 100 } });
            Assert.IsTrue(game.IsComplete());

        }

    }



}