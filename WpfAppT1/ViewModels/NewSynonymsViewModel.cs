using Caliburn.Micro;
using System.Threading.Tasks;
using WpfAppT1.SynonymThesaurus;

namespace WpfAppT1.ViewModels
{
    public class NewSynonymsViewModel : PropertyChangedBase
    {
        public NewSynonymsViewModel(IThesaurus thesaurus)
        {
            _thesaurus = thesaurus;
            SynonymsValidation = new ValidationViewModel(ValidateSynonyms);
        }

        public ValidationViewModel SynonymsValidation { get; private set; }

        public async void AddSynonymsAsync()
        {
            ShowProgress = true;
            var synonyms = WordsHelper.GetWords(SynonymsValidation.Value);
            await Task.Run(()=>_thesaurus.AddSynonyms(synonyms));
            ShowProgress = false;
            SynonymsValidation.Value = string.Empty;
        }

        public bool ShowProgress
        {
            get { return _showProgress; }
            set { _showProgress = value; NotifyOfPropertyChange(() => ShowProgress); }
        }

        private string ValidateSynonyms(string str)
        {
            if (!WordsHelper.IsEmpty(str))
                return "Please enter words";

            if (!WordsHelper.ContainsWords(str))
                return $"You should have at least two words separated by {WordsHelper.WordsSeparator}";

            return string.Empty;
        }

        private bool _showProgress = false;
        private readonly IThesaurus _thesaurus;
    }
}
