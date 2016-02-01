namespace WordsCounter.Services.Tests
{
    using System.Linq;
    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Services;

    /// <summary>
    /// Tests for <see cref="SentenceAnalyzerService"/>
    /// </summary>
    [TestClass]
    public class SentenceAnalyzerServiceTests
    {
        [TestMethod, Description("Simple and complicated sentences should be splited correclty")]
        public void SentencesShouldBeSplitedCorrectly()
        {
            const string input = "This is a statement, and so is this.";
            var service = GetService();

            var actual = service.SplitIntoWords(input);
            var expected = new[] { "This", "is", "a", "statement", "and", "so", "is", "this" };

            CollectionAssert.AreEqual(expected, actual.ToList(), "Sentence should be splited by coma, dot and space ");
        }

        [TestMethod, Description("Complicated sentecnes should be splitted correctly")]
        public void ComplicatedSentencesShouldBeSplittedCorrectly()
        {
            var service = GetService();

            var input = "\"I'd look at McDonald's,\" he said.";
            var actual = service.SplitIntoWords(input);
            var expected = new[] { "I'd", "look", "at", "McDonald's", "he", "said" };
            CollectionAssert.AreEqual(expected, actual.ToList());

            input = "\"They sell over 3,000,000 burgers a day-- at $1.50 each.\"";
            actual = service.SplitIntoWords(input);
            expected = new[] { "They", "sell", "over", "3,000,000", "burgers", "a", "day", "at", "$1.50", "each" };
            CollectionAssert.AreEqual(expected, actual.ToList());

            input = "High-fat foods were the rage. For e.g., margins in fries";
            actual = service.SplitIntoWords(input);
            expected = new[] { "High-fat", "foods", "were", "the", "rage", "For", "e.g", "margins", "in", "fries" };
            CollectionAssert.AreEqual(expected, actual.ToList());

            input = "were over 50%... and (except for R&M & Dyana [sic]) everyone";
            actual = service.SplitIntoWords(input);
            expected = new[] { "were", "over", "50%", "and", "except", "for", "R&M", "Dyana", "sic", "everyone" };
            CollectionAssert.AreEqual(expected, actual.ToList());

            input = "was at ~30% net margin; growing at 25% too!";
            actual = service.SplitIntoWords(input);
            expected = new[] { "was", "at", "~30%", "net", "margin", "growing", "at", "25%", "too" };
            CollectionAssert.AreEqual(expected, actual.ToList());

            input = "url: http://somurl.com";
            actual = service.SplitIntoWords(input);
            expected = new[] { "url", "http://somurl.com" };
            CollectionAssert.AreEqual(expected, actual.ToList());
        }

        /// <summary>
        /// Instantiate new instance to test
        /// </summary>
        /// <returns>new service instance</returns>
        private ISentenceAnalyzerService GetService()
        {
            return new SentenceAnalyzerService();
        }
    }
}
