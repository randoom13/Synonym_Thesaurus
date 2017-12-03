using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppT1.SynonymThesaurus
{
    public class LuceneSynonymData
    {
        public Guid Id { get; private set; }
        public string Word { get; private set; }

        public LuceneSynonymData(Guid id, string word)
        {
            Id = id;
            Word = word;
        }

        public static IEnumerable<LuceneSynonymData> GetSynonymsGroup(IEnumerable<string> synonyms)
        {
            Guid id = Guid.NewGuid();
            return synonyms.Select(word => new LuceneSynonymData(id, word));
        }
    }
}
