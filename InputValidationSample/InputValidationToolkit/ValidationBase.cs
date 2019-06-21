using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace InputValidationSample.InputValidationToolkit
{
    public interface IValidate
    {
        void Validate(string memberName, object value);
    }

    /// <summary>
    /// Base class that implements INotifyPropertyChanged and INotifyDataErrorInfo boilerplate code
    /// </summary>
    public class ValidationBase : INotifyPropertyChanged, INotifyDataErrorInfo, IValidate
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void SetValue<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(currentValue, newValue))
            {
                currentValue = newValue;
                OnPropertyChanged(propertyName, newValue);
            }
        }

        readonly Dictionary<string, List<ValidationResult>> _errors = new Dictionary<string, List<ValidationResult>>();
        public bool HasErrors
        {
            get
            {
                return _errors.Any();
            }
        }
        public IEnumerable GetErrors(string propertyName)
        {
            return _errors[propertyName];
        }

        private void OnPropertyChanged(string propertyName, object value)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Validate(propertyName, value);
        }

        private void AddErrors(string propertyName, IEnumerable<ValidationResult> results)
        {
            if (!_errors.TryGetValue(propertyName, out List<ValidationResult> errors))
            {
                errors = new List<ValidationResult>();
                _errors.Add(propertyName, errors);
            }

            errors.AddRange(results);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ClearErrors(string propertyName)
        {
            if (_errors.TryGetValue(propertyName, out List<ValidationResult> errors))
            {
                errors.Clear();
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public void Validate(string memberName, object value)
        {
            ClearErrors(memberName);
            List<ValidationResult> results = new List<ValidationResult>();
            bool result = Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null)
                {
                    MemberName = memberName
                },
                results
                );

            if (!result)
            {
                AddErrors(memberName, results);
            }
        }
    }
}
