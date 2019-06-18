using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace InputValidationSample.InputValidationToolkit
{
    /// <summary>
    /// This is a helper class that is intended to provide all the functionality for a control that implements InputValidation
    /// </summary>
    interface IInputValidationControlTemplateAccessor
    {
        ContentPresenter GetErrorPresenter();
        ContentPresenter GetDescriptionPresenter();
    }
    /// <summary>
    /// This is a helper class that is intended to provide all the functionality for a control that implements InputValidation
    /// </summary>
    public class ValidationService : IInputValidationControl
    {
        WeakReference<IInputValidationControl> _owner;

        public ValidationService(IInputValidationControl control)
        {
            _owner.SetTarget(control);
        }

        public IObservableVector<InputValidationError> ValidationErrors => new InputValidationErrorsCollection();

        public InputValidationContext ValidationContext { get; set; }

        DataTemplate _errorTemplate;
        public DataTemplate ErrorTemplate
        {
            get => _errorTemplate;
            set
            {
                _errorTemplate = value;
                if (_owner.TryGetTarget(out IInputValidationControl target))
                {
                    UpdateErrorVisuals(target);
                }
            }
        }

        InputValidationKind _inputValidationKind;
        public InputValidationKind InputValidationKind
        {
            get => _inputValidationKind;
            set
            {
                _inputValidationKind = value;
                if (_owner.TryGetTarget(out IInputValidationControl target))
                {
                    UpdateErrorVisuals(target);
                }
            }
        }

        InputValidationCommand _validationCommand;
        public InputValidationCommand ValidationCommand
        {
            get => _validationCommand;
            set
            {
                _validationCommand = value;
                if (_owner.TryGetTarget(out IInputValidationControl target))
                {
                    UpdateErrorVisuals(target);
                }
            }
        }


        private void UpdateErrorVisuals(IInputValidationControl control)
        {
            if (control is IInputValidationControlTemplateAccessor accessor)
            {
                var errorPresenter = accessor.GetErrorPresenter();

                if (InputValidationKind != InputValidationKind.Hidden)
                {
                    if (ErrorTemplate != null && errorPresenter != null)
                    {
                        var loadedContent = (FrameworkElement)ErrorTemplate.LoadContent();
                        loadedContent.DataContext = control;

                        DependencyObject content = loadedContent;

                        // For compact errors we display to ErrorTemplate inside a tooltip
                        if (InputValidationKind == InputValidationKind.Compact || InputValidationKind == InputValidationKind.Auto)
                        {
                            // Get the CompactErrorIconTemplate
                            var iconTemplate = Application.Current.Resources["DefaultCompactErrorIconTemplate"] as DataTemplate;
                            if (iconTemplate != null)
                            {
                                var iconContent = iconTemplate.LoadContent();
                                ToolTip toolTip = (ToolTip)ToolTipService.GetToolTip(iconContent);
                                toolTip.Content = loadedContent;
                                content = iconContent;
                            }
                        }

                        errorPresenter.Content = content;
                    }
                }
                else if (errorPresenter != null)
                {
                    Windows.UI.Xaml.Markup.XamlMarkupHelper.UnloadObject(errorPresenter);
                }
            }
        }
    }
}
