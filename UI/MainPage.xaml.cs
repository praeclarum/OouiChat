using System;
using System.Collections.Generic;

using Xamarin.Forms;
using OouiChat.ViewModels;

namespace OouiChat.UI
{
    public partial class MainPage : ContentPage
    {
        public MainPage ()
        {
            InitializeComponent ();

            BindingContext = new MainPageViewModel ();
        }
    }
}
