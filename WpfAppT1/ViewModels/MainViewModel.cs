using Caliburn.Micro;
using System;

namespace WpfAppT1.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        public MainViewModel(IApplicationData applicationData)
        {
            if (applicationData == null)
                throw new ArgumentNullException(nameof(applicationData));

            var thesaurus = applicationData.Thesaurus;
            SearchSynonyms = new SearchSynonymsViewModel(thesaurus);
            NewSynonyms = new NewSynonymsViewModel(thesaurus);
            AllWords = new AllWordsViewModel(thesaurus);
        }

        public NewSynonymsViewModel NewSynonyms { get; private set; }

        public SearchSynonymsViewModel SearchSynonyms { get; private set; }

        public AllWordsViewModel AllWords { get; private set; }
    }
}
