using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel.DataAnnotations;
using InputValidationSample.InputValidationToolkit;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace InputValidationSample.CustomControl
{
    [InputProperty(Name = "FullName")]
    public sealed partial class PersonControl : ValidateableUserControlBase
    {
        public PersonControl()
        {
            this.InitializeComponent();
        }

        public string FullName
        {
            get => (string)GetValue(FullNameProperty);
            set => SetValue(FullNameProperty, value);
        }

        public static DependencyProperty FullNameProperty = DependencyProperty.Register("FullName", typeof(string), typeof(PersonControl), null);

        public string EmailAddress
        {
            get => (string)GetValue(EmailAddressProperty);
            set => SetValue(EmailAddressProperty, value);
        }

        public static DependencyProperty EmailAddressProperty = DependencyProperty.Register("EmailAddress", typeof(string), typeof(PersonControl), null);

        public string PhoneNumber
        {
            get => (string)GetValue(PhoneNumberProperty);
            set => SetValue(PhoneNumberProperty, value);
        }

        public static DependencyProperty PhoneNumberProperty = DependencyProperty.Register("PhoneNumber", typeof(string), typeof(PersonControl), null);
        public static string ToInitials(string fullName)
        {
            if (!string.IsNullOrEmpty(fullName))
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(2);
                var names = fullName.Split(' ');
                foreach (var name in names)
                {
                    stringBuilder.Append(name[0]);
                }
                return stringBuilder.ToString();
            }
            return string.Empty;
        }
    }
}
