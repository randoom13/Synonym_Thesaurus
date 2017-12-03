using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppT1.SynonymThesaurus
{
    public interface IThesaurus
    {
        IEnumerable<string> GetWords();

        IEnumerable<string> GetSynonyms(string word);

        void AddSynonyms(IEnumerable<string> words);
    }
}
