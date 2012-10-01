using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Prysm.WordCounter
{
    public class LinqWordCounter : IWordCounter
    {
        //todo: ioc / config?
        private readonly string[] _wordEndings = new[]
                                                     {
                                                         " ", 
                                                         ",", 
                                                         ".", 
                                                         "?",
                                                         ";",
                                                         @"""",
                                                         "!"
                                                     };

        public IEnumerable<Result> CountWords(string input)
        {
            //replace double spaces with a single space
            var words = Regex.Replace(input, @"\s+", " ")
                .Split(_wordEndings, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.ToLower())
                .ToList();

            return words
                .Distinct()
                .Select(w => new Result { Word = w, Count = words.Count(word => word == w) })
                .ToList();
        }
    }
}