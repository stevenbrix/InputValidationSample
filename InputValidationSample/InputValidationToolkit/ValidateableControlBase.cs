using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace InputValidationSample.InputValidationToolkit
{
    public abstract class ValidateableControlBase : Control, IInputValidationControl, IInputValidationControl2, IInputValidationControlNotify
    {
        protected ValidateableControlBase()
        {
            ValidationErrors.VectorChanged += ValidationErrors_VectorChanged;
        }

        private void ValidationErrors_VectorChanged(IObservableVector<InputValidationError> sender, IVectorChangedEventArgs args)
        {
            if (args.CollectionChange == CollectionChange.Reset || args.CollectionChange == CollectionChange.ItemRemoved && sender.Count == 0)
            {
                HasValidationErrorsChangedEventArgs hasChangedArgs = null; // new HasValidationErrorsChangedEventArgs(false);
                HasValidationErrorsChanged?.Invoke(this, hasChangedArgs);
            }
            else if (args.CollectionChange == CollectionChange.ItemInserted && sender.Count == 1)
            {
                HasValidationErrorsChangedEventArgs hasChangedArgs = null; // new HasValidationErrorsChangedEventArgs(true);
                HasValidationErrorsChanged?.Invoke(this, hasChangedArgs);
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

        public DependencyProperty ErrorTemplateProperty = DependencyProperty.Register("ErrorTemplate", typeof(DataTemplate), typeof(ValidateableControlBase), null);

        public bool HasValidationErrors => ValidationErrors.Count > 0;

        public InputValidationKind InputValidationKind
        {
            get => (InputValidationKind)GetValue(InputValidationKindProperty);
            set
            {
                SetValue(InputValidationKindProperty, value);
                InputValidationKindChanged?.Invoke(this, new InputValidationKindChangedEventArgs());
            }
        }
        public DependencyProperty InputValidationKindProperty = DependencyProperty.Register("InputValidationKind", typeof(InputValidationKind), typeof(ValidateableControlBase), null);


        public InputValidationMode InputValidationMode
        {
            get => (InputValidationMode)GetValue(InputValidationModeProperty);
            set
            {
                SetValue(InputValidationModeProperty, value);
                InputValidationModeChanged?.Invoke(this, new InputValidationModeChangedEventArgs());
            }
        }
        public DependencyProperty InputValidationModeProperty = DependencyProperty.Register("InputValidationMode", typeof(InputValidationMode), typeof(ValidateableControlBase), null);

        public InputValidationContext ValidationContext { get; set; }

        public IObservableVector<InputValidationError> ValidationErrors { get; } = new InputValidationErrorsCollection();

        public event TypedEventHandler<IInputValidationControl, HasValidationErrorsChangedEventArgs> HasValidationErrorsChanged;
        public event TypedEventHandler<IInputValidationControl, InputValidationErrorEventArgs> ValidationError;
        public event TypedEventHandler<IInputValidationControl, ErrorTemplateChangedEventArgs> ErrorTemplateChanged;
        public event TypedEventHandler<IInputValidationControl, InputValidationKindChangedEventArgs> InputValidationKindChanged;
        public event TypedEventHandler<IInputValidationControl, InputValidationModeChangedEventArgs> InputValidationModeChanged;
        public event TypedEventHandler<IInputValidationControl, ValidationCommandChangedEventArgs> ValidationCommandChanged;

        public InputValidationCommand ValidationCommand
        {
            get => (InputValidationCommand)GetValue(ValidationCommandProperty);
            set
            {
                SetValue(ValidationCommandProperty, value);
                ValidationCommandChanged?.Invoke(this, new ValidationCommandChangedEventArgs());
            }
        }
        public DependencyProperty ValidationCommandProperty = DependencyProperty.Register("ValidationCommand", typeof(InputValidationCommand), typeof(ValidateableControlBase), null);

        public ContentPresenter GetErrorPresenter()
        {
            return (ContentPresenter)GetTemplateChild("ErrorPresenter");
        }

        public ContentPresenter GetDescriptionPresenter()
        {
            return (ContentPresenter)GetTemplateChild("DescriptionPresenter");

        }
    }

    public class ValidateableUserControlBase : UserControl, IInputValidationControl, IInputValidationControl2, IInputValidationControlNotify
    {
        protected ValidateableUserControlBase()
        {
            ValidationErrors.VectorChanged += ValidationErrors_VectorChanged;
        }

        private void ValidationErrors_VectorChanged(IObservableVector<InputValidationError> sender, IVectorChangedEventArgs args)
        {
            if (args.CollectionChange == CollectionChange.Reset || args.CollectionChange == CollectionChange.ItemRemoved && sender.Count == 0)
            {
                HasValidationErrorsChangedEventArgs hasChangedArgs = null; // new HasValidationErrorsChangedEventArgs(false);
                HasValidationErrorsChanged?.Invoke(this, hasChangedArgs);
            }
            else if (args.CollectionChange == CollectionChange.ItemInserted && sender.Count == 1)
            {
                HasValidationErrorsChangedEventArgs hasChangedArgs = null; // new HasValidationErrorsChangedEventArgs(true);
                HasValidationErrorsChanged?.Invoke(this, hasChangedArgs);
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

        public DependencyProperty ErrorTemplateProperty = DependencyProperty.Register("ErrorTemplate", typeof(DataTemplate), typeof(ValidateableControlBase), null);

        public bool HasValidationErrors => ValidationErrors.Count > 0;

        public InputValidationKind InputValidationKind
        {
            get => (InputValidationKind)GetValue(InputValidationKindProperty);
            set
            {
                SetValue(InputValidationKindProperty, value);
                InputValidationKindChanged?.Invoke(this, new InputValidationKindChangedEventArgs());
            }
        }
        public DependencyProperty InputValidationKindProperty = DependencyProperty.Register("InputValidationKind", typeof(InputValidationKind), typeof(ValidateableControlBase), null);


        public InputValidationMode InputValidationMode
        {
            get => (InputValidationMode)GetValue(InputValidationModeProperty);
            set
            {
                SetValue(InputValidationModeProperty, value);
                InputValidationModeChanged?.Invoke(this, new InputValidationModeChangedEventArgs());
            }
        }
        public DependencyProperty InputValidationModeProperty = DependencyProperty.Register("InputValidationMode", typeof(InputValidationMode), typeof(ValidateableControlBase), null);

        public InputValidationContext ValidationContext { get; set; }

        public IObservableVector<InputValidationError> ValidationErrors { get; } = new InputValidationErrorsCollection();

        public event TypedEventHandler<IInputValidationControl, HasValidationErrorsChangedEventArgs> HasValidationErrorsChanged;
        public event TypedEventHandler<IInputValidationControl, InputValidationErrorEventArgs> ValidationError;
        public event TypedEventHandler<IInputValidationControl, ErrorTemplateChangedEventArgs> ErrorTemplateChanged;
        public event TypedEventHandler<IInputValidationControl, InputValidationKindChangedEventArgs> InputValidationKindChanged;
        public event TypedEventHandler<IInputValidationControl, InputValidationModeChangedEventArgs> InputValidationModeChanged;
        public event TypedEventHandler<IInputValidationControl, ValidationCommandChangedEventArgs> ValidationCommandChanged;

        public InputValidationCommand ValidationCommand
        {
            get => (InputValidationCommand)GetValue(ValidationCommandProperty);
            set
            {
                SetValue(ValidationCommandProperty, value);
                ValidationCommandChanged?.Invoke(this, new ValidationCommandChangedEventArgs());
            }
        }
        public DependencyProperty ValidationCommandProperty = DependencyProperty.Register("ValidationCommand", typeof(InputValidationCommand), typeof(ValidateableControlBase), null);

        public ContentPresenter GetErrorPresenter()
        {
            return (ContentPresenter)FindName("ErrorPresenter");
        }

        public ContentPresenter GetDescriptionPresenter()
        {
            return (ContentPresenter)FindName("DescriptionPresenter");

        }
    }
}
