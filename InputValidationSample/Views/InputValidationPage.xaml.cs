// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(PurchaseViewModel), typeof(InputValidationPage), new PropertyMetadata(null));

        public PurchaseViewModel ViewModel
        {
            get { return GetValue(ViewModelProperty) as PurchaseViewModel; }
            set { SetValue(ViewModelProperty, value); }
        }
    }

    
}
