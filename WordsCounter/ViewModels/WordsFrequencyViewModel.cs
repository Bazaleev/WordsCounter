namespace WordsCounter.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Linq;
    using Contracts;
    using Models;
    using Prism.Commands;
    using Prism.Mvvm;

    /// <summary>
    /// View model for calculating word frequency
    /// </summary>
    [Export]
    public sealed class WordsFrequencyViewModel : BindableBase
    {
        /// <summary>
        /// Service that splits sentence into words
        /// </summary>
        private readonly ISentenceAnalyzerService _sentenceAnalyzer;

        /// <summary>
        /// A result list of words count
        /// </summary>
        private ICollection<WordCount> _wordsCount;

        /// <summary>
        /// A sentence to analyze
        /// </summary>
        private string _sentence;

        /// <summary>
        /// Initializes a new instance of the <see cref="WordsFrequencyViewModel"/> class
        /// </summary>
        /// <param name="sentenceAnalyzer">service that splits sentence into words</param>
        /// <exception cref="ArgumentNullException">throws if <paramref name="sentenceAnalyzer"/> is null</exception>
        [ImportingConstructor]
        public WordsFrequencyViewModel(ISentenceAnalyzerService sentenceAnalyzer)
        {
            if (sentenceAnalyzer == null)
            {
                throw new ArgumentNullException(nameof(sentenceAnalyzer));
            }

            WordsCount = new WordCount[0];
            _sentenceAnalyzer = sentenceAnalyzer;

            CalculateWordsCommand = new DelegateCommand(OnCalculateWords, OnCanExecuteCalculateWords);
            PropertyChanged += OnViewModelPropertyChanged;
        }

        /// <summary>
        /// Gets or sets a sentence to analyze
        /// </summary>
        public string Sentence
        {
            get
            {
                return _sentence;
            }

            set
            {
                SetProperty(ref _sentence, value);
            }
        }

        /// <summary>
        /// Gets a result words count
        /// </summary>
        public ICollection<WordCount> WordsCount
        {
            get
            {
                return _wordsCount;
            }

            private set
            {
                SetProperty(ref _wordsCount, value);
            }
        }

        /// <summary>
        /// Gets a command that calculates the words frequency
        /// </summary>
        public DelegateCommand CalculateWordsCommand { get; private set; }

        /// <summary>
        /// Calculate words frequency and refreshes words list
        /// </summary>
        private void OnCalculateWords()
        {
            var words = _sentenceAnalyzer.SplitIntoWords(Sentence);
            var wordsInLower = from word in words
                               where !String.IsNullOrEmpty(word)
                               select word.ToLowerInvariant();

            var result = from word in wordsInLower
                         group word by word into wordsGroup
                         select new WordCount
                         {
                             Word = wordsGroup.Key,
                             Count = wordsGroup.Count()
                         };

            WordsCount = result
                .OrderByDescending(wc => wc.Count)
                .ToList();
        }

        /// <summary>
        /// Checks is it possible to execute <seealso cref="CalculateWordsCommand"/>
        /// /// </summary>
        /// <returns>true if command can be executed, false otherwise</returns>
        private bool OnCanExecuteCalculateWords()
        {
            return !String.IsNullOrWhiteSpace(Sentence);
        }

        /// <summary>
        /// Processes PropertyChangedEvent on some properties
        /// </summary>
        /// <param name="o">event sender</param>
        /// <param name="e">event arguments</param>
        private void OnViewModelPropertyChanged(object o, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Sentence))
            {
                CalculateWordsCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
