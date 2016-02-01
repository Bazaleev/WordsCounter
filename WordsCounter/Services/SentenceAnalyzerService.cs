namespace WordsCounter.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics.Contracts;
    using System.Text.RegularExpressions;
    using Contracts;

    /// <summary>
    /// Service for sentences analyze
    /// </summary>
    [Export(typeof(ISentenceAnalyzerService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SentenceAnalyzerService : ISentenceAnalyzerService
    {
        /// <summary>
        /// Splits provided sentence into words
        /// </summary>
        /// <param name="sentence">sentence to split</param>
        /// <returns>collection of words</returns>
        public ICollection<string> SplitIntoWords(string sentence)
        {
            Contract.Ensures(Contract.Result<ICollection<string>>() != null);

            if (String.IsNullOrEmpty(sentence))
            {
                return new string[0];
            }

            // Split by clear word separators
            var pattern = @"[\s\!\?\;\(\)\[\]\{\}\<\>""]";
            var temp = Regex.Replace(sentence, pattern, " ", RegexOptions.Multiline);

            // by COMMA, unless it has numbers on both sides: 3,000,000
            pattern = @"[,](?!\d)";
            temp = Regex.Replace(temp, pattern, " ", RegexOptions.Multiline);
            pattern = @"(?<!\d)[,]";
            temp = Regex.Replace(temp, pattern, " ", RegexOptions.Multiline);

            // by FULL-STOP, SINGLE-QUOTE, HYPHEN, AMPERSAND, unless it has a letter on both sides
            pattern = @"(?<!\w)[\.\-\&]";
            temp = Regex.Replace(temp, pattern, " ", RegexOptions.Multiline);
            pattern = @"[\.\-\&](?!\w)";
            temp = Regex.Replace(temp, pattern, " ", RegexOptions.Multiline);

            // by QUOTE, unless it follows a letter (e.g.McDonald's, Holmes')
            // ToDo words in quotas are left
            pattern = @"(?<!\w)[']";
            temp = Regex.Replace(temp, pattern, " ", RegexOptions.Multiline);

            // by SLASH, if it has spaces on at least one side. (URLs shouldn't be split)
            pattern = @"\s\/";
            temp = Regex.Replace(temp, pattern, " ", RegexOptions.Multiline);
            pattern = @"\/\s";
            temp = Regex.Replace(temp, pattern, " ", RegexOptions.Multiline);

            // by COLON, unless it's a URL or a time (11:30am for e.g.)
            pattern = @"\:(?!\/\/|\d)";
            temp = Regex.Replace(temp, pattern, " ", RegexOptions.Multiline);

            return temp.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
