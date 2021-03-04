using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RestSample.Core.Helpers;

namespace RestSample.Test.Core.Helpers
{
    [TestClass]
    public class WordHelperUnitTest
    {
        [TestMethod]
        public void GetAllDistinctWordsFromListsOfWords_EmptyLists_ShouldReturnEmpty()
        {
            var emptyWords = new List<List<string>> { new List<string>() };
            var uniqueWords = WordHelper.GetAllDistinctWordsFromListsOfWords(emptyWords);

            Assert.AreEqual(uniqueWords.Length, 0);
        }

        [TestMethod]
        public void GetAllDistinctWordsFromListsOfWords_InsertMultipleWordsWithDuplicates_ShouldReturnTwoWords()
        {
            var emptyWords = new List<List<string>> { new List<string> { "tomato", "pasta" }, new List<string> { "tomato", "tomato" }, new List<string> { "pasta" } };
            var uniqueWords = WordHelper.GetAllDistinctWordsFromListsOfWords(emptyWords);

            Assert.AreEqual(uniqueWords.Length, 2);
        }


        [TestMethod]
        public void BuildCommonWords_FindThirdMostCommonWord_ShouldReturnOneWord()
        {
            var words = new List<string> { "tomato", "pasta", "pasta", "potato", "pasta", "potato", "potato", "plate", "fork", "tomato" };
            var uniqueWords = WordHelper.BuildCommonWords(words, 2, 1);

            var expected = new[] { "tomato" };
            CollectionAssert.AreEqual(uniqueWords, expected);
        }

    }
}
