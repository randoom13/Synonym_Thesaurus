using Caliburn.Micro;
using System.Threading.Tasks;
using WpfAppT1.SynonymThesaurus;

namespace WpfAppT1.ViewModels
{
    public class AllWordsViewModel : PropertyChangedBase
    {
        public AllWordsViewModel(IThesaurus thesaurus)
        {
            _thesaurus = thesaurus;
        }

        public async void ShowAsync()
        {
            ShowProgress = true;
            var words = await Task.Run(()=>_thesaurus.GetWords());
            ShowProgress = false;
            Words.Clear();
            Words.AddRange(words);
        }

        public bool ShowProgress
        {
            get { return _showProgress; }
            set { _showProgress = value; NotifyOfPropertyChange(() => ShowProgress); }
        }

        public BindableCollection<string> Words { get { return _words; } }

        private bool _showProgress = false;
        private BindableCollection<string> _words = new BindableCollection<string>();
        private readonly IThesaurus _thesaurus;
        
    }
}
