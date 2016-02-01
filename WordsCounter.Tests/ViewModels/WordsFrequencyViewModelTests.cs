namespace WordsCounter.ViewModels.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using ViewModels;

    /// <summary>
    /// Tests for <see cref="WordsFrequencyViewModel"/>
    /// </summary>
    [TestClass]
    public class WordsFrequencyViewModelTests
    {
        [TestMethod, Description("ArgumentNullException should be thrown if sentenceAnalyzer is null")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorShouldThrowArgumentNullException()
        {
            var viewModel = new WordsFrequencyViewModel(null);
        }

        [TestMethod, Description("CalcWords should be disabled if Sentence is empty")]
        public void CalcWordsCommandShouldBeDisabledIfSentenceIsEmpty()
        {
            var viewModel = new WordsFrequencyViewModel(A.Fake<ISentenceAnalyzerService>());
            Assert.IsFalse(viewModel.CalculateWordsCommand.CanExecute());

            var canExecuteChanged = A.Fake<EventHandler>();
            viewModel.CalculateWordsCommand.CanExecuteChanged += canExecuteChanged;
            viewModel.Sentence = "Some sentence";
            Assert.IsTrue(viewModel.CalculateWordsCommand.CanExecute(), "Command should be enabled if a sentence is provided");
            A.CallTo(() => canExecuteChanged(A<object>._, A<EventArgs>._)).MustHaveHappened();

            viewModel.Sentence = String.Empty;
            Assert.IsFalse(viewModel.CalculateWordsCommand.CanExecute(), "Command should be enabled if a sentence is provided");
            A.CallTo(() => canExecuteChanged(A<object>._, A<EventArgs>._)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [TestMethod, Description("words in sentence should be grouped. Words case should be ignored")]
        public void CalcWordsShouldProvideValidWordsFrequency()
        {
            var words = new[] { "This", "is", "a", "statement", "and", "so", "is", "this" };
            var fakeService = A.Fake<ISentenceAnalyzerService>();
            A.CallTo(() => fakeService.SplitIntoWords(A<string>._)).Returns(words);

            var viewModel = new WordsFrequencyViewModel(fakeService);

            const string sentence = "Doesn't matter";
            viewModel.Sentence = sentence;
            viewModel.CalculateWordsCommand.Execute();

            A.CallTo(() => fakeService.SplitIntoWords(A<string>.That.IsEqualTo(sentence))).MustHaveHappened();

            var expected = new[]
            {
                new WordCount("this", 2),
                new WordCount("is", 2),
                new WordCount("a", 1),
                new WordCount("statement", 1),
                new WordCount("and", 1),
                new WordCount("so", 1)
            };

            var comparer = Comparer<WordCount>.Create((x, y) => x.Word.CompareTo(y.Word) * 10 + x.Count.CompareTo(y.Count));
            CollectionAssert.AreEqual(expected, viewModel.WordsCount.ToList(), comparer);
        }

        [TestMethod, Description("Words calculation result should be sorted by Count in descending order")]
        public void WordsCountResultShouldBeSorted()
        {
            var words = new[] { "last", "middle", "middle", "median", "median", "top", "top", "top" };
            var fakeService = A.Fake<ISentenceAnalyzerService>();
            A.CallTo(() => fakeService.SplitIntoWords(A<string>._)).Returns(words);

            var viewModel = new WordsFrequencyViewModel(fakeService);

            const string sentence = "Doesn't matter";
            viewModel.Sentence = sentence;
            viewModel.CalculateWordsCommand.Execute();

            var expected = new[]
{
                new WordCount("top", 3),
                new WordCount("middle", 2),
                new WordCount("median", 2),
                new WordCount("last", 1)
            };

            var comparer = Comparer<WordCount>.Create((x, y) => x.Word.CompareTo(y.Word) * 10 + x.Count.CompareTo(y.Count));
            CollectionAssert.AreEqual(expected, viewModel.WordsCount.ToList(), comparer);
        }
    }
}