using NUnit.Framework;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Testing
{
    [TestFixture]
    public class AppendingRefsInATextContext
    {
        
        private TextContext _textCtx;
        private const string TextWithNewlineWithoutReference = "The brown fox\r\n";
        private const string TextWithoutNewlineWithoutReference = "The brown fox";
        private const string TextWithReference = "The brown fox\r\n\r\n  [1]: hiho";
        private const string TextWithTwoReferences = "The brown fox\r\n\r\n  [1]: hiho\r\n  [2]: hehe";


        private void StartWithThisText(string text, string selection = "")
        {
            _textCtx = new TextContext(selection, text);
        }

        [Test]
        public void AppendingNothing()
        {
            StartWithThisText(TextWithNewlineWithoutReference);
            ThisWasAppended(null);
        }

        [Test]
        public void NextReferenceIdIs1()
        {
            StartWithThisText(TextWithNewlineWithoutReference);
            Assert.AreEqual(1, _textCtx.NextReferenceId);
        }

        [Test]
        public void NextReferenceIdIs2()
        {
            StartWithThisText(TextWithReference);
            Assert.AreEqual(2, _textCtx.NextReferenceId);
        }

        [Test]
        public void NextReferenceIdAlsoConsidersExistingAppends()
        {
            StartWithThisText(TextWithReference);
            _textCtx.AppendReference("blabla");
            Assert.AreEqual(3, _textCtx.NextReferenceId);
        }

        [Test]
        public void AddAReferenceToText()
        {
            StartWithThisText(TextWithNewlineWithoutReference);
            _textCtx.AppendReference("jonesy");
            ThisWasAppended("\r\n  [1]: jonesy");
        }

        [Test]
        public void AddExtraNewLineWhenNoneAtEnd()
        {
            StartWithThisText(TextWithoutNewlineWithoutReference);
            _textCtx.AppendReference("jonesy");
            ThisWasAppended("\r\n\r\n  [1]: jonesy");
        }

        [Test]
        public void AppendToTextContainingRef()
        {
            StartWithThisText(TextWithReference);
            _textCtx.AppendReference("jonesy");
            ThisWasAppended("\r\n  [2]: jonesy");
        }

        [Test]
        public void AppendTwoRefs()
        {
            StartWithThisText(TextWithReference);
            _textCtx.AppendReference("A");
            _textCtx.AppendReference("B");
            ThisWasAppended("\r\n  [2]: A\r\n  [3]: B");
        }

        [Test]
        public void AppendWithTwoRefsDefined()
        {
            StartWithThisText(TextWithTwoReferences);
            _textCtx.AppendReference("A");
            ThisWasAppended("\r\n  [3]: A");
        }

        private void ThisWasAppended(string appendage)
        {
            Assert.AreEqual(appendage, _textCtx.TextToAppend);
        }
    }
}
