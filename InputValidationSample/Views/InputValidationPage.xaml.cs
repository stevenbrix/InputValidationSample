// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using InputValidationSample.ViewModel;

namespace InputValidationSample
{
    public sealed partial class InputValidationPage : Page
    {
        public InputValidationPage()
        {
            ViewModel = new PurchaseViewModel();
            ValidationCommand = new InputValidationToolkit.ValidationCommand(ViewModel);
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(PurchaseViewModel), typeof(InputValidationPage), new PropertyMetadata(null));

        public PurchaseViewModel ViewModel
        {
            get { return GetValue(ViewModelProperty) as PurchaseViewModel; }
            set { SetValue(ViewModelProperty, value); }
        }

        public InputValidationToolkit.ValidationCommand ValidationCommand { get; private set; }
    }

    
}
