namespace WordsCounter.Models
{
    using System;

    /// <summary>
    /// Represents a word frequency
    /// </summary>
    [Serializable]
    public class WordCount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WordCount"/> class
        /// </summary>
        public WordCount()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WordCount"/> class
        /// </summary>
        /// <param name="word">a word</param>
        /// <param name="count">word count</param>
        public WordCount(string word, int count)
        {
            Word = word;
            Count = count;
        }

        /// <summary>
        /// Gets or sets a word
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// Gets or sets words count
        /// </summary>
        public int Count { get; set; }
    }
}
