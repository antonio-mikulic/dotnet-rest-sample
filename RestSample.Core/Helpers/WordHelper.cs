using System;
using System.Collections.Generic;
using System.Linq;

namespace RestSample.Core.Helpers
{
    public static class WordHelper
    {
        // TODO This could be refactored as recursive method which can pass through more levels of words
        public static string[] GetAllDistinctWordsFromListsOfWords(IEnumerable<IEnumerable<string>> words)
        {
            var wordList = new List<string>();

            // Add a list of all child words into single list

            // TODO Test performance
            foreach (var childWords in words)
                wordList.AddRange(childWords);

            // Select distinct
            return wordList.Distinct().ToArray();
        }

        // Process list of strings and return most common words by given parameters
        /// <param name="words">List of words.</param>
        /// <param name="skipCount">How much most common words should be skipped.</param>
        /// <param name="takeCount">After skipping words based on skipCount param, take this much words.</param>
        public static string[] BuildCommonWords(IEnumerable<string> words, int skipCount, int takeCount)
        {
            // Make sure max take size is 10
            takeCount = takeCount > 10 ? 10 : takeCount;

            // Group by unique words
            // Select word as key and occurrences of that word
            // Order by most common
            // Skip first few most common 
            // Take next most common
            // Select just the string

            var newWordList = new List<string>();
            foreach (var word in words)
            {
                newWordList.AddRange(word.Split(' ', '.', StringSplitOptions.RemoveEmptyEntries));
            }

            var mostCommon = newWordList
            .GroupBy(s => s)
            .Select(x => new { x.Key, Count = x.Count() })
            .OrderByDescending(x => x.Count)
            .Skip(skipCount)
            .Take(takeCount)
            .Select(s => s.Key)
            .ToArray();

            return mostCommon;
        }

        // Find all wanted words from a sentence
        public static string HighlightWord(string sentence, string[] highlights)
        {
            // Separate by space
            var words = sentence.Split(' ');

            var processedWords = new List<string>();

            foreach (var word in words)
            {
                var processedWord = highlights.Contains(word) ? $"<em>{word}</em>" : word;
                processedWords.Add(processedWord);
            }

            return String.Join(' ', processedWords);
        }
    }
}
