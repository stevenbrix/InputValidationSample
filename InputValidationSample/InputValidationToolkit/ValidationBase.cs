using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using Microsoft.UI.Xaml.Data;
using System.Linq;

namespace InputValidationSample.InputValidationToolkit
{
    /// <summary>
    /// Base class that implements INotifyPropertyChanged and INotifyDataErrorInfo boilerplate code
    /// </summary>
    public class ValidationBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(currentValue, newValue))
            {
                currentValue = newValue;
                NotifyPropertyChanged(propertyName);
                OnPropertyChanged(newValue, propertyName);
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
        public IEnumerable<object> GetErrors(string propertyName)
        {
            return _errors[propertyName];
        }

        private void OnPropertyChanged(object value, string propertyName)
        {
            ClearErrors(propertyName);
            List<ValidationResult> results = new List<ValidationResult>();
            bool result = Validator.TryValidateProperty(
                value,
                new ValidationContext(this, null, null)
                {
                    MemberName = propertyName
                },
                results
                );

            if (!result)
            {
                AddErrors(propertyName, results);
            }
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
    }
}
