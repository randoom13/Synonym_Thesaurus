using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppT1.SynonymThesaurus;

namespace WpfAppT1
{
    public interface IApplicationData
    {
        IThesaurus Thesaurus { get; }
    }
}
