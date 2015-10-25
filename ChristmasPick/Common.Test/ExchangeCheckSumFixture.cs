using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Test
{
    [TestClass]
    public class ExchangeCheckSumFixture
    {
        public void VerifyDiagnosticMessage(int expectedIn, int expectedOut, string actualMsg)
        {
            Assert.AreEqual(string.Format("buying {0} present(s) and is recieving {1} present(s)", expectedOut, expectedIn), actualMsg);
        }

        [TestMethod]
        public void ConstructedCheckSumIsNotValid()
        {
            ExchangeCheckSum subject = new ExchangeCheckSum();
            Assert.AreEqual(false, subject.isValid());
            Assert.AreEqual("not buying or recieving a gift", subject.DiagnosticMessage());
        }

        [TestMethod]
        public void OnePresentInAndOnePresentOutWillBeValid()
        {
            ExchangeCheckSum subject = new ExchangeCheckSum();
            subject.updatePresentsIn();
            subject.updatePresentsOut();
            Assert.AreEqual(true, subject.isValid());
            Assert.AreEqual("correct", subject.DiagnosticMessage());
        }

        [TestMethod]
        public void OnePresentInAndNoPresentOutWillBeInvalid()
        {
            ExchangeCheckSum subject = new ExchangeCheckSum();
            subject.updatePresentsIn();
            Assert.AreEqual(false, subject.isValid());
            VerifyDiagnosticMessage(1, 0, subject.DiagnosticMessage());
        }

        [TestMethod]
        public void NoPresentInAndOnePresentOutWillBeInvalid()
        {
            ExchangeCheckSum subject = new ExchangeCheckSum();
            subject.updatePresentsOut();
            Assert.AreEqual(false, subject.isValid());
            VerifyDiagnosticMessage(0, 1, subject.DiagnosticMessage());
        }

        [TestMethod]
        public void MultiplePresentInAndMultiplePresentOutWillBeInvalid()
        {
            ExchangeCheckSum subject = new ExchangeCheckSum();
            subject.updatePresentsIn();
            subject.updatePresentsIn();
            subject.updatePresentsIn();
            subject.updatePresentsOut();
            subject.updatePresentsOut();
            subject.updatePresentsOut();
            Assert.AreEqual(false, subject.isValid());
            VerifyDiagnosticMessage(3, 3, subject.DiagnosticMessage());
        }

    }
}
