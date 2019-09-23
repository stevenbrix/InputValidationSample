using System;
using Windows.Foundation;

namespace Microsoft.UI.Xaml.Controls
{
 
    /// <summary>
    /// This interface should be wrapped into IInputValidationControl, but is currently not in the spec. We should have
    /// events that allow listeners to listen to property changes. It's not common for our controls to implement INotifyPropertyChanged.
    /// <summary/>
    public interface IInputValidationControlNotify
    {
        /// <summary>
        /// Event to notify listeners when the ValidationContext property changes
        /// </summary>
        event TypedEventHandler<IInputValidationControl, ValidationContextChangedEventArgs> ValidationContextChanged;
        /// <summary>
        /// Event to notify listeners when the ErrorTemplate property changes
        /// </summary>
        event TypedEventHandler<IInputValidationControl, ErrorTemplateChangedEventArgs> ErrorTemplateChanged;
        /// <summary>
        /// Event to notify listeners when the InputValidationKind property changes
        /// </summary>
        event TypedEventHandler<IInputValidationControl, InputValidationKindChangedEventArgs> InputValidationKindChanged;
        /// <summary>
        /// Event to notify listeners when the ValidationCommand property changes
        /// </summary>
        event TypedEventHandler<IInputValidationControl, ValidationCommandChangedEventArgs> ValidationCommandChanged;

    };

    public class ValidationContextChangedEventArgs : EventArgs { }
    public class ErrorTemplateChangedEventArgs : EventArgs { }
    public class InputValidationKindChangedEventArgs : EventArgs { }
    public class ValidationCommandChangedEventArgs : EventArgs { }

}
