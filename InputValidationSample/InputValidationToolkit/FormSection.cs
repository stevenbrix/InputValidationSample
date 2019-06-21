using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Foundation.Collections;
using Windows.Foundation;

namespace InputValidationSample.InputValidationToolkit
{
    public sealed  class FormSectionChangedEventArgs : EventArgs
    {
        public FormSectionChangedEventArgs()
        {
        }

        public bool IsCompleted { get; set; }
    }

    public class FormSection : Grid, IInputValidationControl2
    {
        public FormSection()
        {
            Loaded += FormSection_Loaded;
        }


        private void FormSection_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var child in Children.OfType<IInputValidationControl>())
            {
                // If the control doesn't require input, then it's considered valid. Otherwise,
                // we need to wait for input to change
                inputControlsVerified.Add(child, !child.ValidationContext.IsInputRequired);
                child.HasValidationErrorsChanged += Child_HasValidationErrorsChanged;

            }
        }

        private void Child_HasValidationErrorsChanged(IInputValidationControl sender, HasValidationErrorsChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public bool Completed { get; private set; } = false;

        public InputValidationCommand ValidationCommand
        {
            get => (InputValidationCommand)GetValue(ValidationCommandProperty);
            set => SetValue(ValidationCommandProperty, value);
        }
        public DependencyProperty ValidationCommandProperty = DependencyProperty.Register("ValidationCommand", typeof(InputValidationCommand), typeof(ValidateableControlBase), null);

        public event TypedEventHandler<FormSection, FormSectionChangedEventArgs> SectionChanged;

        private void Items_VectorChanged(Windows.Foundation.Collections.IObservableVector<object> sender, Windows.Foundation.Collections.IVectorChangedEventArgs args)
        {
            if (args.CollectionChange == CollectionChange.ItemChanged || args.CollectionChange == CollectionChange.ItemInserted)
            {
                if (sender[(int)args.Index] is IInputValidationControl)
                {

                }
            }
            else if (args.CollectionChange == CollectionChange.ItemRemoved)
            {
                if (sender[(int)args.Index] is IInputValidationControl)
                {

                }
            }
            else
            {

            }
        }

        private Dictionary<IInputValidationControl, bool> inputControlsVerified = new Dictionary<IInputValidationControl, bool>();
    }
}
