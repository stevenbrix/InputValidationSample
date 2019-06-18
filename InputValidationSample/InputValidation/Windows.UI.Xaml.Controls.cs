using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation.Collections;

namespace Windows.UI.Xaml.Controls
{
    /// <summary>
    /// Name of the property that is associated with user input.
    /// </summary>
    public class InputPropertyAttribute : Attribute
    {
        public string Name { get; set; }
    }

    /// <summary>
    ///    Determines how the control displays it's validation visuals
    /// </summary>
    public enum InputValidationKind
    {
        /// <summary>
        /// Let's the Control dictate how to display visuals. This is the default value.
        /// </summary>
        Auto,
        /// <summary>
        /// An error icon displays to the right of the control that displays the error messages in a tool tip.
        /// </summary>
        Compact,
        /// <summary>
        /// Text for the error messages is displayed underneath the control.  
        /// </summary>
        Inline,
        /// <summary>
        /// Validation visuals are not displayed at all.
        /// </summary>
        Hidden,
    }

    /// <summary>
    /// Interface for Controls that participate in input validation.
    /// <summary/>
    public interface IInputValidationControl
    {
        /// <summary>
        /// Collection of errors to display based on the InputValidationKind property.
        /// </summary>
        IObservableVector<InputValidationError> ValidationErrors { get; }
        /// <summary>
        ///  Extra information about the Source of the validation. 
        ///  Note: This is automatically set by the XAML markup compiler. See summary above. 
        /// </summary>
        InputValidationContext ValidationContext { get; set; }
        /// <summary>
        ///  DataTemplate that expresses how the errors are displayed.
        /// </summary>
        DataTemplate ErrorTemplate { get; set; }
        /// <summary>
        ///  Determines how the control should visualize validation errors.
        /// </summary>
        InputValidationKind InputValidationKind { get; set; }
        /// <summary>
        /// Command associated with the IInputValidationControl. See InputValidationCommand for more
        /// information.
        /// </summary>
        InputValidationCommand ValidationCommand { get; set; }
    };



    /// <summary>
    /// Command that controls how an IInputValidationControl does validation.
    /// </summary>
    public class InputValidationCommand
    {
        Windows.UI.Xaml.Controls.InputValidationKind InputValidationKind;
        /// <summary>
        /// Called by a control to determine if it should call the Validate method.
        /// </summary>
        public bool CanValidate(Windows.UI.Xaml.Controls.IInputValidationControl validationControl)
        {
            return CanValidateCore(validationControl);
        }
        /// <summary>
        /// If CanValidate returns true, Validate is invoked to perform validation on the
        /// validationControl provided.
        /// </summary>
        public void Validate(Windows.UI.Xaml.Controls.IInputValidationControl validationControl)
        {
            ValidateCore(validationControl);
        }
        /// <summary>
        /// Method that derived classes implement to provide their own implementation of the CanValidate method.
        /// </summary>
        public virtual bool CanValidateCore(Windows.UI.Xaml.Controls.IInputValidationControl validationControl)
        {
            // Validate only when we have errors
            return validationControl.ValidationErrors.Count > 0;
        }
        /// <summary>
        /// Method that derived classes implement to provide their own implementation of the Validate method.
        /// </summary>
        public virtual void ValidateCore(Windows.UI.Xaml.Controls.IInputValidationControl validationControl)
        {
            foreach (var attr in validationControl.GetType().GetCustomAttributes(true))
            {
                Console.WriteLine(attr);
            }
        }
    }

    /// <summary>
    /// Provides more contextual information about how to validate, display, and/or format input.
    /// </summary>
    public class InputValidationContext
    {
        public InputValidationContext(string memberName, bool isRequired)
        {
            IsInputRequired = isRequired;
            MemberName = memberName;
        }
        bool IsInputRequired { get; }
        string MemberName { get; }
    }

    /// <summary>
    ///  Provides the error message for what should be displayed to the user.
    /// </summary>
    public class InputValidationError
    {
        InputValidationError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        string ErrorMessage { get; }
    }
}
