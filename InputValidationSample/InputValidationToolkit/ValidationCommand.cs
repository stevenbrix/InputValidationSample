using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace InputValidationSample.InputValidationToolkit
{
    public class ValidationCommand : InputValidationCommand
    {
        readonly IValidate validator = null;
        public ValidationCommand(IValidate validate)
        {
            validator = validate;
        }

        protected override bool CanValidateCore(IInputValidationControl validationControl)
        {
            // Validation is disabled
            if (validationControl.InputValidationMode == InputValidationMode.Disabled || InputValidationMode == InputValidationMode.Disabled)
            {
                return false;
            }

            // If we have errors, then we can try to trigger validation
            return validationControl.HasValidationErrors;
        }
        protected override void ValidateCore(IInputValidationControl validationControl)
        {
            if (validationControl.TryGetInputPropertyValue(out object value))
            {
                validator.Validate(validationControl.ValidationContext.MemberName, value);
            }
        }
    }

    public static class IInputValidationControlExtensions
    {
        // Gets the value of the InputProperty declared on the control so we an properly validate.
        public static bool TryGetInputPropertyValue(this IInputValidationControl ctrl, out object value)
        {
            Type controlType = ctrl.GetType();
            value = null;
            CustomAttributeData inputPropertyData = controlType.CustomAttributes.Where(attr => attr.AttributeType == typeof(InputPropertyAttribute)).SingleOrDefault();
            if (inputPropertyData != null)
            {
                string propertyName = inputPropertyData.NamedArguments[0].TypedValue.Value.ToString();
                value = controlType.GetProperty(propertyName).GetValue(ctrl);
            }
            return inputPropertyData != null;
        }
    }
}
