using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using InputValidationSample.InputValidationToolkit;

namespace InputValidationSample.InputValidation
{
    // Windows.UI.Xaml.Controls.PasswordBox is a sealed type so we'll just derive from TextBox. We won't do anything fancy for the sake of simplicity.
    [InputProperty(Name = "Password")]
    public class PasswordBox : Windows.UI.Xaml.Controls.TextBox, IInputValidationControl, IInputValidationControlTemplateAccessor
    {

        public PasswordBox()
        {
            validationService = new ValidationService(this);
        }


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set
            {
                SetValue(PasswordProperty, value);
                // Also update text
                Text = value;
            }
        }
        public static DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(PasswordBox), null);

        public IObservableVector<InputValidationError> ValidationErrors => ((IInputValidationControl)validationService).ValidationErrors;

        public InputValidationContext ValidationContext { get => ((IInputValidationControl)validationService).ValidationContext; set => ((IInputValidationControl)validationService).ValidationContext = value; }
        public DataTemplate ErrorTemplate { get => ((IInputValidationControl)validationService).ErrorTemplate; set => ((IInputValidationControl)validationService).ErrorTemplate = value; }
        public InputValidationKind InputValidationKind { get => ((IInputValidationControl)validationService).InputValidationKind; set => ((IInputValidationControl)validationService).InputValidationKind = value; }
        public InputValidationCommand ValidationCommand { get => ((IInputValidationControl)validationService).ValidationCommand; set => ((IInputValidationControl)validationService).ValidationCommand = value; }

        ValidationService validationService;

        public ContentPresenter GetDescriptionPresenter()
        {
            return (ContentPresenter)GetTemplateChild("DescriptionPresenter");
        }

        public ContentPresenter GetErrorPresenter()
        {
            return (ContentPresenter)GetTemplateChild("ErrorPresenter");
        }
    }
}
