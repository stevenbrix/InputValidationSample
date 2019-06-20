using System;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls;


namespace InputValidationSample.InputValidationToolkit
{
    /// <summary>
    /// This is a helper class that is intended to provide all the functionality for a control that implements InputValidation
    /// </summary>
    public class ValidationService
    {
        readonly WeakReference<IInputValidationControl> _owner;

        public ValidationService(IInputValidationControl control)
        {
            _owner = new WeakReference<IInputValidationControl>(control);
            control.ValidationErrors.VectorChanged += ValidationErrors_VectorChanged;
            if (control is IInputValidationControlNotify validationControlNotify)
            {
                validationControlNotify.ErrorTemplateChanged += ValidationControlNotify_ErrorTemplateChanged;
                validationControlNotify.InputValidationKindChanged += ValidationControlNotify_InputValidationKindChanged;
                validationControlNotify.ValidationCommandChanged += ValidationControlNotify_ValidationCommandChanged;
                validationControlNotify.ValidationContextChanged += ValidationControlNotify_ValidationContextChanged;
            }
        }

        private void ValidationControlNotify_ValidationContextChanged(IInputValidationControl sender, ValidationContextChangedEventArgs args)
        {
            UpdateErrorVisuals(sender);
        }

        private void ValidationControlNotify_ValidationCommandChanged(IInputValidationControl sender, ValidationCommandChangedEventArgs args)
        {
            UpdateErrorVisuals(sender);
        }

        private void ValidationControlNotify_InputValidationKindChanged(IInputValidationControl sender, InputValidationKindChangedEventArgs args)
        {
            UpdateErrorVisuals(sender);
        }

        private void ValidationControlNotify_ErrorTemplateChanged(IInputValidationControl sender, ErrorTemplateChangedEventArgs args)
        {
            UpdateErrorVisuals(sender);
        }

        private void ValidationErrors_VectorChanged(IObservableVector<InputValidationError> sender, IVectorChangedEventArgs args)
        {
            if (_owner.TryGetTarget(out IInputValidationControl target))
            {
                // Only update the error visuals when an important change in the collection occured. Otherwise the vector is bindable and so
                // the ui will update automatically with these changes once they are already loaded.
                bool updateVisuals = (args.CollectionChange == CollectionChange.Reset) ||
                                     (args.CollectionChange == CollectionChange.ItemRemoved && target.ValidationErrors.Count == 0) ||
                                     (args.CollectionChange == CollectionChange.ItemInserted && target.ValidationErrors.Count == 1);
                if (updateVisuals)
                {
                    UpdateErrorVisuals(target);
                }
            }
        }
   
        private void UpdateErrorVisuals(IInputValidationControl control)
        {
            ValidationVisualAction action = GetValidationVisualAction(control, out ContentPresenter errorPresenter);
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

        private void GoToValidationState(IInputValidationControl control)
        {
            if (control.InputValidationKind != InputValidationKind.Hidden)
            {
                VisualStateManager.GoToState((Control)control, "InlineValidat");
            }
        }

        private ValidationVisualAction GetValidationVisualAction(IInputValidationControl control, out ContentPresenter errorPresenter)
        {
            ValidationVisualAction action = ValidationVisualAction.NoChange;
            errorPresenter = FindNameInSubtree<ContentPresenter>(control, "ErrorPresenter");
            if (errorPresenter != null)
            {
                if (control.InputValidationKind != InputValidationKind.Hidden && control.ValidationErrors.Count > 0 && control.ErrorTemplate != null)
                {
                    action = ValidationVisualAction.LoadAndSetContent;
                }
                else if (control.InputValidationKind == InputValidationKind.Hidden || control.ValidationErrors.Count == 0 ||  control.ErrorTemplate == null)
                {
                    action = ValidationVisualAction.Unload;
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


        private static T FindNameInSubtree<T>(IInputValidationControl element, string descendantName) where T: class
        {
            return FindNameInSubtreeWorker((DependencyObject)element, descendantName) as T;
        }

        private static FrameworkElement FindNameInSubtreeWorker(DependencyObject element, string descendantName)
        {
            if (element == null)
                return null;

            FrameworkElement frameworkElement = element as FrameworkElement;

            if (frameworkElement?.Name == descendantName)
                return frameworkElement;

            int childrenCount = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < childrenCount; i++)
            {
                var result = FindNameInSubtreeWorker(Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(element, i), descendantName);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
