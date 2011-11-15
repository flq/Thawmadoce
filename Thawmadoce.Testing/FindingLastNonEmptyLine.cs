using NUnit.Framework;
using Thawmadoce;

namespace Thawmadoce.Testing
{
    [TestFixture]
    public class FindingLastNonEmptyLine
    {
        [TestCase("The brown fox\r\n\r\n  [1]: hiho", "  [1]: hiho")]
        [TestCase("HiHo\r\nHello", "Hello")]
        [TestCase("HiHo\r\nHello\r\n \r\n", "Hello")]
        [TestCase("", null)]
        [TestCase("Hello", "Hello")]
        [TestCase("Hello\r\n\r\n", "Hello")]
        [TestCase("Hello\r\n \r\n", "Hello")]
        [Test]
        public void FindNonEmptyLines(string input, string expectedOutput)
        {
            var s = input.FindLastNonEmptyLine();
            Assert.AreEqual(expectedOutput, s);
        }
    }
}