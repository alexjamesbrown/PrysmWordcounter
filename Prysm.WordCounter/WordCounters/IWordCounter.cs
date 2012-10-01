using System.Collections.Generic;

namespace Prysm.WordCounter
{
    public interface IWordCounter
    {
        IEnumerable<Result> CountWords(string input);
    }
}