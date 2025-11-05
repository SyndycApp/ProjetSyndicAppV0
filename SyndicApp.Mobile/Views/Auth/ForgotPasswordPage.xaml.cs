using SyndicApp.Mobile.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Views.Auth
{
    public  partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage(ForgotPasswordViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}


