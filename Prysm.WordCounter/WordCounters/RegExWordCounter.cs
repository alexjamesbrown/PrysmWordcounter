using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Prysm.WordCounter
{
    public class RegExWordCounter : IWordCounter
    {
        public IEnumerable<Result> CountWords(string input)
        {
            const string regexPattern = @"'?([a-zA-z'-]+)'?";

            return Regex.Matches(input, regexPattern).OfType<Match>()
                .Select(x => x.Value.ToLower())
                .GroupBy(x => x)
                .Select(x => new Result { Word = x.Key, Count = x.Count() })
                .ToList();
        }
    }
}
