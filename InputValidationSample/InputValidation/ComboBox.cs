using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using InputValidationSample.InputValidationToolkit;
using Windows.Foundation;

namespace InputValidationSample.InputValidation
{
    [InputProperty(Name = "Text")]

    public class ValidatableComboBox : Windows.UI.Xaml.Controls.ComboBox, IInputValidationControl, IInputValidationControlNotify
    {
        ValidationService validationService;
        public ValidatableComboBox()
        {
            validationService = new ValidationService(this);
        }

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

        public static DependencyProperty ErrorTemplateProperty = DependencyProperty.Register("ErrorTemplate", typeof(DataTemplate), typeof(ValidatableComboBox), null);
        public static DependencyProperty ValidationContextProperty = DependencyProperty.Register("ValidationContext", typeof(InputValidationContext), typeof(ValidatableComboBox), null);
        public static DependencyProperty InputValidationKindProperty = DependencyProperty.Register("InputValidationKind", typeof(InputValidationKind), typeof(ValidatableComboBox), null);
        public static DependencyProperty ValidationCommandProperty = DependencyProperty.Register("ValidationCommand", typeof(InputValidationCommand), typeof(ValidatableComboBox), null);
        public static DependencyProperty ValidationErrorsProperty = DependencyProperty.Register("ValidationErrors", typeof(IObservableVector<InputValidationError>), typeof(ValidatableComboBox), null);

        public event TypedEventHandler<IInputValidationControl, ValidationContextChangedEventArgs> ValidationContextChanged;
        public event TypedEventHandler<IInputValidationControl, ErrorTemplateChangedEventArgs> ErrorTemplateChanged;
        public event TypedEventHandler<IInputValidationControl, InputValidationKindChangedEventArgs> InputValidationKindChanged;
        public event TypedEventHandler<IInputValidationControl, ValidationCommandChangedEventArgs> ValidationCommandChanged;

        /*
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public static DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(ValidateableTextBox), null);

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }
        public static DependencyProperty PlaceholderTextProperty = DependencyProperty.Register("PlaceholderText", typeof(string), typeof(ValidateableTextBox), null);

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(ValidateableTextBox), null);
        */

    }
}
