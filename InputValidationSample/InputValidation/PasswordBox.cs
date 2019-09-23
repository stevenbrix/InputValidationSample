using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using InputValidationSample.InputValidationToolkit;
using Windows.Foundation;

namespace InputValidationSample.InputValidation
{
    // Windows.UI.Xaml.Controls.PasswordBox is a sealed type so we'll just derive from TextBox. We won't do anything fancy for the sake of simplicity.
    
    [InputProperty(Name = "Password")]
    public class ValidateablePasswordBox : Windows.UI.Xaml.Controls.TextBox, IInputValidationControl, IInputValidationControlNotify
    {

        public ValidateablePasswordBox()
        {
            validationService = new ValidationService(this);
        }


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(ValidateablePasswordBox), null);
        public IObservableVector<InputValidationError> ValidationErrors { get; } = new InputValidationErrorsCollection();

        public InputValidationContext ValidationContext
        {
            get => (InputValidationContext)GetValue(ValidationContextProperty);
            set
            {
                SetValue(ValidationContextProperty, value);
                ValidationContextChanged?.Invoke(this, new ValidationContextChangedEventArgs());
            }
        }
        public DataTemplate ErrorTemplate
        {
            get => (DataTemplate)GetValue(ErrorTemplateProperty);
            set
            {
                SetValue(ErrorTemplateProperty, value);
                ErrorTemplateChanged?.Invoke(this, new ErrorTemplateChangedEventArgs());
            }
        }
        public InputValidationKind InputValidationKind
        {
            get => (InputValidationKind)GetValue(InputValidationKindProperty);
            set
            {
                SetValue(InputValidationKindProperty, value);
                InputValidationKindChanged?.Invoke(this, new InputValidationKindChangedEventArgs());
            }
        }
        public InputValidationCommand ValidationCommand
        {
            get => (InputValidationCommand)GetValue(ValidationCommandProperty);
            set
            {
                SetValue(ValidationCommandProperty, value);
                ValidationCommandChanged?.Invoke(this, new ValidationCommandChangedEventArgs());
            }
        }

        public static DependencyProperty ErrorTemplateProperty = DependencyProperty.Register("ErrorTemplate", typeof(DataTemplate), typeof(ValidateablePasswordBox), null);
        public static DependencyProperty ValidationContextProperty = DependencyProperty.Register("ValidationContext", typeof(InputValidationContext), typeof(ValidateablePasswordBox), null);
        public static DependencyProperty InputValidationKindProperty = DependencyProperty.Register("InputValidationKind", typeof(InputValidationKind), typeof(ValidateablePasswordBox), null);
        public static DependencyProperty ValidationCommandProperty = DependencyProperty.Register("ValidationCommand", typeof(InputValidationCommand), typeof(ValidateablePasswordBox), null);
        public static DependencyProperty ValidationErrorsProperty = DependencyProperty.Register("ValidationErrors", typeof(IObservableVector<InputValidationError>), typeof(ValidateablePasswordBox), null);

        ValidationService validationService;

        public event TypedEventHandler<IInputValidationControl, ValidationContextChangedEventArgs> ValidationContextChanged;
        public event TypedEventHandler<IInputValidationControl, ErrorTemplateChangedEventArgs> ErrorTemplateChanged;
        public event TypedEventHandler<IInputValidationControl, InputValidationKindChangedEventArgs> InputValidationKindChanged;
        public event TypedEventHandler<IInputValidationControl, ValidationCommandChangedEventArgs> ValidationCommandChanged;
    }

}
