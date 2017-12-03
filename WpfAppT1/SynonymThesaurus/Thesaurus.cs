using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppT1.SynonymThesaurus
{
    public class Thesaurus : IThesaurus
    {
        public Thesaurus(string path)
        {
            _luceneProxy = new LuceneProxy(path);
        }

        public void AddSynonyms(IEnumerable<string> words)
        {
            _luceneProxy.AddUpdateLuceneIndex(LuceneSynonymData.GetSynonymsGroup(words));
        }

        public IEnumerable<string> GetSynonyms(string word)
        {
           var groups = _luceneProxy
                .SearchDefault(word, nameof(LuceneSynonymData.Word))
                .Select(sd=>sd.Id).ToList();
           if (!groups.Any())
                yield break;
            int lastItemIndex = groups.Count - 1;
            for (int ind = 0; ind < groups.Count; ind++)
            {
                var words = _luceneProxy
                    .SearchDefault(groups[ind].ToString(),nameof(LuceneSynonymData.Id))
                    .Select(sd => sd.Word).Where(wo => wo != word);

                foreach (var wo in words)
                    yield return wo;

                if (lastItemIndex != ind)
                    yield return " ";
            }
        }

        public IEnumerable<string> GetWords()
        {
            return _luceneProxy.GetAllIndexRecords().Select(it=>it.Word)
                               .Distinct().OrderBy(w=>w);
        }

        private readonly LuceneProxy _luceneProxy;
    }
}
