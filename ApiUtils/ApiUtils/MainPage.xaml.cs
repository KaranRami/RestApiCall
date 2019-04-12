using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace ApiUtils
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        CancellationTokenSource cts = null;
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                using (cts = new CancellationTokenSource())
                {
                    LoginRequestModel loginRequestModel = new LoginRequestModel()
                    {
                        userName = Constants.UserName,
                        password = Constants.Password,
                    };
                    var response = await UserManager.Instance.Login(loginRequestModel, cts.Token);
                    if (response.ServiceReaponseHeader.ErrorException != null)
                    {
                        if (response.ServiceReaponseHeader.ErrorException is TaskCanceledException)
                        {
                            await DisplayAlert("Login Alert", "Login task has been cancelled", "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Login Alert", response.ServiceReaponseHeader.ErrorException.Message, "Ok");
                        }
                    }
                    else
                    {
                        if (response.Result.IsSuccess)
                        {
                            await DisplayAlert("Login Alert", response.Result.token, "Ok");
                        }
                        else if (!response.Result.IsSuccess && response.Result.ErrorEcxeption == null)
                        {
                            await DisplayAlert("Login Alert", response.Result.ErrorMessage, "Ok");
                        }
                        else if (response.Result.ErrorEcxeption != null)
                        {
                            await DisplayAlert("Login Alert", response.Result.ErrorEcxeption.Message, "Ok");
                        }
                        else
                        {
                            await DisplayAlert("Login Alert", "Something went wrong.", "Ok");
                        }
                    }
                }
            });
        }
    }
}
