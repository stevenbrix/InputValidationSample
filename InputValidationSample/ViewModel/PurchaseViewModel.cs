using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace InputValidationSample.ViewModel
{
    public class PurchaseViewModel : InputValidationToolkit.ValidationBase
    {
        public PurchaseViewModel()
        {
        }


        private string _name;

        [DefaultValue("")]
        [MinLength(5, ErrorMessage = "Name must be more than 4 characters")]
        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        private string _cardNumber;
        public string CardNumber
        {
            get { return _cardNumber; }
            set { SetValue(ref _cardNumber, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetValue(ref _address, value); }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set { SetValue(ref _city, value); }
        }

        private string _zip;
        [CustomValidation(typeof(PurchaseViewModel), "ValidateZip")]
        public string Zip
        {
            get { return _zip; }
            set { SetValue(ref _zip, value); }
        }

        private string _cardExpirationMonth;
        public string CardExpirationMonth
        {
            get { return _cardExpirationMonth; }
            set { SetValue(ref _cardExpirationMonth, value); }
        }

        private string _cardExpirationYear;
        public string CardExpirationYear
        {
            get { return _cardExpirationYear; }
            set { SetValue(ref _cardExpirationYear, value); }
        }

        private string _ccv;
        public string CCV
        {
            get { return _ccv; }
            set { SetValue(ref _ccv, value); }
        }

        private string _billingAddress;
        public string BillingAddress
        {
            get { return _billingAddress; }
            set { SetValue(ref _billingAddress, value); }
        }

        private string _billingCity;
        public string BillingCity
        {
            get { return _billingCity; }
            set { SetValue(ref _billingCity, value); }
        }

        private string _billingZip;
        [CustomValidation(typeof(PurchaseViewModel), "ValidateZip")]
        public string BillingZip
        {
            get { return _billingZip; }
            set { SetValue(ref _billingZip, value); }
        }

        public static ValidationResult ValidateZip(string zip)
        {
            if (!zip.Any(x => char.IsLetter(x)))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(
                    "Zip code must contain numbers only");
            }
        }
     
    }
}
