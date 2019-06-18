using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using InputValidationSample.InputValidationToolkit;

namespace InputValidationSample.InputValidation
{
    [InputProperty(Name = "Text")]

    public class TextBox : Windows.UI.Xaml.Controls.TextBox, IInputValidationControl, IInputValidationControlTemplateAccessor
    {
        ValidationService validationService;

        public IObservableVector<InputValidationError> ValidationErrors => ((IInputValidationControl)validationService).ValidationErrors;

        public InputValidationContext ValidationContext { get => ((IInputValidationControl)validationService).ValidationContext; set => ((IInputValidationControl)validationService).ValidationContext = value; }
        public DataTemplate ErrorTemplate { get => ((IInputValidationControl)validationService).ErrorTemplate; set => ((IInputValidationControl)validationService).ErrorTemplate = value; }
        public InputValidationKind InputValidationKind { get => ((IInputValidationControl)validationService).InputValidationKind; set => ((IInputValidationControl)validationService).InputValidationKind = value; }
        public InputValidationCommand ValidationCommand { get => ((IInputValidationControl)validationService).ValidationCommand; set => ((IInputValidationControl)validationService).ValidationCommand = value; }

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
