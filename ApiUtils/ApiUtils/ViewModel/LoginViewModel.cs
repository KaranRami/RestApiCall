using Acr.UserDialogs;
using ApiUtils.DataAccess;
using ApiUtils.DataModel;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ApiUtils.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(ContentPage view) : base(view)
        {
        }

        #region Method
        public bool ValidateAction()
        {
            try
            {
                if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password))
                {
                    m_View.DisplayAlert("Login Alert", "Please enter email and password.", "Ok");
                    return false;
                }
                if (string.IsNullOrEmpty(Username))
                {
                    m_View.DisplayAlert("Login Alert", "Please enter the email address.", "Ok");
                    return false;
                }
                if (!Regex.IsMatch(Username, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"))
                {
                    m_View.DisplayAlert("Login Alert", "Please enter valid email address.", "Ok");
                    return false;
                }
                if (string.IsNullOrEmpty(Password))
                {
                    m_View.DisplayAlert("Login Alert", "Please enter password.", "Ok");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteExceptionToConsole(ex);
                return false;
            }
        }
        #endregion

        #region Command
        public ICommand LoginCommand { get { return new Command(async () => await LoginCommandEvent()); } }
        private async Task LoginCommandEvent()
        {
            await SingleRun(async () =>
            {
                if (!ValidateAction())
                    return;
                UserDialogs.Instance.ShowLoading();

                using (cts = new CancellationTokenSource())
                {
                    LoginRequestModel loginRequestModel = new LoginRequestModel()
                    {
                        userName = Username,
                        password = Password,
                    };
                    var response = await UserManager.Instance.Login(loginRequestModel, cts.Token);
                    UserDialogs.Instance.HideLoading();
                    if (response.ServiceReaponseHeader.ErrorException != null)
                    {
                        if (response.ServiceReaponseHeader.ErrorException is TaskCanceledException)
                        {
                            await m_View.DisplayAlert("Login Alert", "Login task has been cancelled", "Ok");
                        }
                        else
                        {
                            await m_View.DisplayAlert("Login Alert", response.ServiceReaponseHeader.ErrorException.Message, "Ok");
                        }
                    }
                    else
                    {
                        if (response.Result.IsSuccess)
                        {
                            await m_View.DisplayAlert("Login Alert", response.Result.token, "Ok");
                        }
                        else if (!response.Result.IsSuccess && response.Result.ErrorEcxeption == null)
                        {
                            await m_View.DisplayAlert("Login Alert", response.Result.ErrorMessage, "Ok");
                        }
                        else if (response.Result.ErrorEcxeption != null)
                        {
                            await m_View.DisplayAlert("Login Alert", response.Result.ErrorEcxeption.Message, "Ok");
                        }
                        else
                        {
                            await m_View.DisplayAlert("Login Alert", "Something went wrong.", "Ok");
                        }
                    }
                }
            });
        }
        public ICommand ReturnCommand { get { return new Command<string>(ReturnCommandEvent); } }
        private void ReturnCommandEvent(string nextFocus)
        {
            (m_View.FindByName(nextFocus) as Xamarin.Forms.View)?.Focus();
        }
        #endregion

        #region Property
        public CancellationTokenSource cts { get; set; }

        private string username;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        #endregion
    }
}
