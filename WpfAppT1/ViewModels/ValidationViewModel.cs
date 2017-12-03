using Caliburn.Micro;
using System;
using System.Windows.Media;

namespace WpfAppT1.ViewModels
{
    public class ValidationViewModel : PropertyChangedBase
    {
        public ValidationViewModel(Func<string, string> validationProvider)
        {
            if (validationProvider == null)
                throw new ArgumentNullException(nameof(validationProvider));
            _validationProvider = validationProvider;
            UpdateProperties();
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                UpdateProperties();
                NotifyOfPropertyChange(() => Value);
            }
        }

        public string ValidationMessage { get; private set; }

        public bool IsValid
        {
            get { return string.IsNullOrEmpty(ValidationMessage); }
        }

        public Color Color { get { return IsValid ? Colors.Gray : Colors.Red; } }

        private void UpdateProperties()
        {
            ValidationMessage = _validationProvider(_value);
            NotifyOfPropertyChange(() => IsValid);
            NotifyOfPropertyChange(() => ValidationMessage);
            NotifyOfPropertyChange(() => Color);
        }

        private string _value;
        private Func<string, string> _validationProvider;
    }
}
