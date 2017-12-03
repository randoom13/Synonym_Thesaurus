using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppT1
{
    public static class WordsHelper
    {
        public static bool IsEmpty(string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool ContainsWords(string str)
        {
            var wordsCount = str.Split(new[] { WordsSeparator })
                                .Select(w => w.Trim())
                                .Count(IsValidWord);
            return wordsCount > 1;
        }

        public static IEnumerable<string> GetWords(string str)
        {
            if (!IsEmpty(str))
                return Enumerable.Empty<string>();
            return str.Split(new[] { WordsSeparator })
                      .Select(w => w.Trim())
                      .Where(IsValidWord);
        }

        public static bool IsValidWord(string word)
        {
           return IsEmpty(word) && word.Any(s => char.IsLetter(s));
        }

        public readonly static char WordsSeparator = ',';
    }
}
