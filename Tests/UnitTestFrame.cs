using GameScoring.DomainAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameScoring.ProgrammingParadigms;
using System.Collections.Generic;
using Wiring;

namespace Tests
{
    [TestClass]
    public class UnitTestFrame
    {

        Frame frame;

        [TestInitialize]
        public void Setup()
        {
            frame = new Frame("frame");
            frame.WireTo(new SinglePlay("singlePlay"));
        }

        [TestCleanup]
        public void Cleanup()
        {
        }



        [TestMethod]
        public void TestGetnPlays()
        {
            this.frame.setIsFrameCompleteLambda((frameNumber, nPlays, score) => nPlays == 3);
            IConsistsOf frame = this.frame;
            Assert.AreEqual(0, frame.GetnPlays());
            frame.Ball(0, 3);
            Assert.AreEqual(1, frame.GetnPlays());
            frame.Ball(0, 2);
            Assert.AreEqual(2, frame.GetnPlays());
            frame.Ball(1, 6);
            Assert.AreEqual(3, frame.GetnPlays());
            frame.Ball(1, 6);
            Assert.AreEqual(3, frame.GetnPlays());
        }


        [TestMethod]
        public void TestGetScore()
        {
            this.frame.setIsFrameCompleteLambda((frameNumber, nPlays, score) => nPlays == 3);
            IConsistsOf frame = this.frame;
            Assert.AreEqual(0, frame.GetScore()[0]);
            frame.Ball(0, 3);
            Assert.AreEqual(3, frame.GetScore()[0]);
            Assert.AreEqual(0, frame.GetScore()[1]);
            frame.Ball(0, 2);
            Assert.AreEqual(5, frame.GetScore()[0]);
            frame.Ball(1, 6);
            Assert.AreEqual(6, frame.GetScore()[1]);
            frame.Ball(1, 6);
            Assert.AreEqual(6, frame.GetScore()[1]);
        }

        [TestMethod]
        public void TestCompleteMethodPlays()
        {
            this.frame.setIsFrameCompleteLambda((frameNumber, nPlays, score) => nPlays == 2);
            IConsistsOf frame = this.frame;  // make a new reference to the frame under test with the type of the interface
            Assert.IsFalse(frame.IsComplete());
            frame.Ball(0, 8);
            Assert.IsFalse(frame.IsComplete());
            frame.Ball(0, 1);
            Assert.IsTrue(frame.IsComplete());
        }


        [TestMethod]
        public void TestCompleteMethodScore()
        {
            this.frame.setIsFrameCompleteLambda((frameNumber, nPlays, score) => score[0] == 10);
            IConsistsOf frame = this.frame;
            Assert.IsFalse(frame.IsComplete());
            frame.Ball(0, 9);
            Assert.IsFalse(frame.IsComplete());
            frame.Ball(0, 1);
            Assert.IsTrue(frame.IsComplete());
        }

        [TestMethod]
        public void TestCompleteMethodframeNumber()
        {
            // This test also tests GetCopy
            // set the completion lambda function for the frame object under test to complete only if it is the 2nd frame of its parent
            this.frame.setIsFrameCompleteLambda((frameNumber, nPlays, score) => frameNumber == 1);
            IConsistsOf frame = this.frame;
            frame.Ball(0, 1);
            Assert.IsFalse(frame.IsComplete());
            frame = frame.GetCopy(1);  // makes a Copy of the Frame with the passed in frameNumber
            Assert.IsFalse(frame.IsComplete());
            frame.Ball(0, 1);          // The child (SinglePlay) must be complete as well as the lambda expression
            Assert.IsTrue(frame.IsComplete());
        }



        public void TestGetSubFrames()
        {
            this.frame.setIsFrameCompleteLambda((frameNumber, nPlays, score) => nPlays == 2);
            IConsistsOf frame = this.frame;
            List<IConsistsOf> L = frame.GetSubFrames();
            Assert.AreEqual(1, L.Count);
            frame.Ball(0, 3);
            L = frame.GetSubFrames();
            Assert.AreEqual(2, L.Count);
        }
    }

}

