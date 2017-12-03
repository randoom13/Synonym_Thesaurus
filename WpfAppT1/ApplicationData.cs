using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppT1.SynonymThesaurus;

namespace WpfAppT1
{
    public class ApplicationData :IApplicationData
    {
        public ApplicationData()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var thesaurusPath = Path.Combine(folder, "Synonyms");
            if (!Directory.Exists(thesaurusPath))
                Directory.CreateDirectory(thesaurusPath);
            Thesaurus = new Thesaurus(thesaurusPath);
        }

      public  IThesaurus Thesaurus { get; private set; }
    }
}
