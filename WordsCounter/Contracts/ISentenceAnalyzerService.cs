namespace WordsCounter.Contracts
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides methods to split sentence into parts
    /// </summary>
    public interface ISentenceAnalyzerService
    {
        /// <summary>
        /// Splits sentence into words
        /// </summary>
        /// <param name="sentence">sentence to split</param>
        /// <returns>collection of words</returns>
        ICollection<string> SplitIntoWords(string sentence);
    }
}
