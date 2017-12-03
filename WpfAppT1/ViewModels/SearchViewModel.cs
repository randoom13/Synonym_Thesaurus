using Caliburn.Micro;
using System.Threading.Tasks;
using WpfAppT1.SynonymThesaurus;

namespace WpfAppT1.ViewModels
{
    public class SearchSynonymsViewModel : PropertyChangedBase
    {
        public SearchSynonymsViewModel(IThesaurus thesaurus)
        {
            _thesaurus = thesaurus;
            WordValidation = new ValidationViewModel(ValidateWord);
        }

        public async void SearchAsync()
        {
            ShowProgress = true;
            var words = await Task.Run(()=>_thesaurus.GetSynonyms(WordValidation.Value));
            ShowProgress = false;
            _synonyms.Clear();
            _synonyms.AddRange(words);
        }

        public BindableCollection<string> Synonyms { get { return _synonyms; } }

        public ValidationViewModel WordValidation { get; private set; }

        public bool ShowProgress
        {
            get { return _showProgress; }
            set { _showProgress = value; NotifyOfPropertyChange(() => ShowProgress); }
        }

        private string ValidateWord(string str)
        {
            if (!WordsHelper.IsValidWord(str))
                return "Please enter a word";
            return string.Empty;
        }

        private bool _showProgress = false;
        private BindableCollection<string> _synonyms = new BindableCollection<string>();
        private readonly IThesaurus _thesaurus;
    }
}
