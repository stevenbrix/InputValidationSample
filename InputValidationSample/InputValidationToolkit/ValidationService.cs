using System;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace InputValidationSample.InputValidationToolkit
{
    /// <summary>
    /// Interface for ValidationService to listen to custom control events
    /// <summary/>
    public interface IInputValidationControlNotify
    {
        /// <summary>
        /// Event to notify listeners when the ErrorTemplate property changes
        /// </summary>
        event TypedEventHandler<IInputValidationControl, ErrorTemplateChangedEventArgs> ErrorTemplateChanged;
        /// <summary>
        /// Event to notify listeners when the InputValidationKind property changes
        /// </summary>
        event TypedEventHandler<IInputValidationControl, InputValidationKindChangedEventArgs> InputValidationKindChanged;
        /// <summary>
        /// Event to notify listeners when the InputValidationMode property changes
        /// </summary>
        event TypedEventHandler<IInputValidationControl, InputValidationModeChangedEventArgs> InputValidationModeChanged;
        /// <summary>
        /// Event to notify listeners when the ValidationCommand property changes
        /// </summary>
        event TypedEventHandler<IInputValidationControl, ValidationCommandChangedEventArgs> ValidationCommandChanged;

    };

    public class ErrorTemplateChangedEventArgs : EventArgs { }
    public class InputValidationKindChangedEventArgs : EventArgs { }
    public class InputValidationModeChangedEventArgs : EventArgs { }
    public class ValidationCommandChangedEventArgs : EventArgs { }


    /// <summary>
    /// This is a helper class that is intended to provide all the functionality for a control that implements InputValidation
    /// </summary>
    public interface IInputValidationControlTemplateAccessor
    {
        ContentPresenter GetErrorPresenter();
        ContentPresenter GetDescriptionPresenter();
    }

    /// <summary>
    /// This is a helper class that is intended to provide all the functionality for a control that implements InputValidation.
    /// It handles listening to the appropriate errors and ensuring that the visuals are displayed properly at all times.
    /// </summary>
    public class ValidationService
    {
        readonly WeakReference<IInputValidationControl> _owner;

        public ValidationService(IInputValidationControl control)
        {
            _owner = new WeakReference<IInputValidationControl>(control);
            Register(control);
        }

        public static void Register(IInputValidationControl control)
        {
            control.HasValidationErrorsChanged += Control_HasValidationErrorsChanged;
            if (control is IInputValidationControlNotify validationControlNotify)
            {
                validationControlNotify.ErrorTemplateChanged += ValidationControlNotify_PropertyChanged;
                validationControlNotify.InputValidationKindChanged += ValidationControlNotify_PropertyChanged;
                validationControlNotify.ValidationCommandChanged += ValidationControlNotify_PropertyChanged;
                validationControlNotify.InputValidationModeChanged += ValidationControlNotify_PropertyChanged;
            }
        }

        public static void Unregister(IInputValidationControl control)
        {
            control.HasValidationErrorsChanged -= Control_HasValidationErrorsChanged;
            if (control is IInputValidationControlNotify validationControlNotify)
            {
                validationControlNotify.ErrorTemplateChanged -= ValidationControlNotify_PropertyChanged;
                validationControlNotify.InputValidationKindChanged -= ValidationControlNotify_PropertyChanged;
                validationControlNotify.ValidationCommandChanged -= ValidationControlNotify_PropertyChanged;
                validationControlNotify.InputValidationModeChanged -= ValidationControlNotify_PropertyChanged;
            }
        }

        private static void Control_HasValidationErrorsChanged(IInputValidationControl sender, HasValidationErrorsChangedEventArgs args)
        {
            UpdateErrorVisuals(sender);
        }

        private static void ValidationControlNotify_PropertyChanged(IInputValidationControl sender, EventArgs args)
        {
            UpdateErrorVisuals(sender);
        }

        private static void UpdateErrorVisuals(IInputValidationControl control)
        {
            ValidationVisualAction action = GetValidationVisualAction(control, out ContentPresenter errorPresenter);
            GoToValidationStates(control);

            if (action == ValidationVisualAction.LoadAndSetContent)
            {
                var loadedContent = (FrameworkElement)control.ErrorTemplate.LoadContent();
                loadedContent.DataContext = control;

                DependencyObject content = loadedContent;

                // For compact errors we display to ErrorTemplate inside a tooltip
                if (control.InputValidationKind == InputValidationKind.Compact || control.InputValidationKind == InputValidationKind.Auto)
                {
                    // Get the CompactErrorIconTemplate
                    if (Application.Current.Resources["DefaultCompactErrorIconTemplate"] is DataTemplate iconTemplate)
                    {
                        var iconContent = iconTemplate.LoadContent();
                        ToolTip toolTip = (ToolTip)ToolTipService.GetToolTip(iconContent);
                        toolTip.Content = loadedContent;
                        content = iconContent;
                    }
                }

                errorPresenter.Content = content;
            }
            else if (action == ValidationVisualAction.Unload)
            {
                Windows.UI.Xaml.Markup.XamlMarkupHelper.UnloadObject(errorPresenter);
            }
        }

        private static void GoToValidationStates(IInputValidationControl validationControl)
        {
            if (validationControl is Control ctrl)
            {
                VisualStateManager.GoToState(ctrl, ValidationEnabledStates.GetState(validationControl), false);
                if (ValidationErrorStates.TryGetState(validationControl, out string errorState))
                {
                    VisualStateManager.GoToState(ctrl, errorState, false);
                }
            }
        }

        private static ValidationVisualAction GetValidationVisualAction(IInputValidationControl control, out ContentPresenter errorPresenter)
        {
            ValidationVisualAction action = ValidationVisualAction.NoChange;
            errorPresenter = null;
            if (control is IInputValidationControlTemplateAccessor templateAccessor)
            {
                errorPresenter = templateAccessor.GetErrorPresenter();
                if (errorPresenter != null)
                {
                    if (control.InputValidationMode != InputValidationMode.Disabled && control.ValidationErrors.Count > 0 && control.ErrorTemplate != null)
                    {
                        action = ValidationVisualAction.LoadAndSetContent;
                    }
                    else if (control.InputValidationMode == InputValidationMode.Disabled || control.ValidationErrors.Count == 0 || control.ErrorTemplate == null)
                    {
                        action = ValidationVisualAction.Unload;
                    }
                }
            }

            return action;
        }

        private enum ValidationVisualAction
        {
            NoChange,
            LoadAndSetContent,
            Unload
        }

        private static class ValidationEnabledStates
        {
            // VisualState names for when InputValidation is enabled
            private const string Inline = "InlineValidationEnabled";
            private const string Compact = "CompactValidationEnabled";
            private const string Disabled = "ValidationDisabled";

            // Retrieves the appropriate VisualState name for the control
            public static string GetState(IInputValidationControl control)
            {
                if (control.InputValidationMode == InputValidationMode.Disabled)
                {
                    return Disabled;
                }
                else if (control.InputValidationKind == InputValidationKind.Inline)
                {
                    return Inline;
                }
                else
                {
                    return Compact;
                }
            }
        }

        private static class ValidationErrorStates
        {
            // VisualState names for when there are errors

            private const string Inline= "InlineErrors";
            private const string Compact = "CompactErrors";
            private const string None = "ErrorsCleared";

            // Tries to get the state name. Returns false if InputValidation is disabled and we
            // shouldn't make the GoToState call.
            public static bool TryGetState(IInputValidationControl control, out string state)
            {
                state = null;
                if (control.InputValidationMode != InputValidationMode.Disabled)
                {
                    if (!control.HasValidationErrors)
                    {
                        state = None;
                    }
                    else if (control.InputValidationKind == InputValidationKind.Inline)
                    {
                        state = Inline;
                    }
                    else
                    {
                        state = Compact;
                    }
                    return true;
                }
                return false;
            }
        }

    }
}
