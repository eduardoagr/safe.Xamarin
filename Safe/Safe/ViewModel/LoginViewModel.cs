using Acr.UserDialogs;

using PropertyChanged;

using Safe.Firebase;
using Safe.Model;
using Safe.View;

using System.Windows.Input;

using Xamarin.Forms;

namespace Safe.ViewModel {
    [AddINotifyPropertyChangedInterface]
    public class LoginViewModel {
        public ICommand OpenPopupCommand { get; set; }
        public Command Register { get; set; }
        public Command Login { get; set; }
        public bool DisplayPopUp { get; set; }
        public User User { get; set; }
        public bool RegisterAllowed { get; set; }
        public bool LoginAllowed { get; set; }

        public LoginViewModel() {


            User = new User {
                OnAnyOfMyPropertiesChanged = () => {
                    Register.ChangeCanExecute();
                    Login.ChangeCanExecute();
                }
            };
            OpenPopupCommand = new Command(() => {
                DisplayPopUp = true;
            });
            Register = new Command(
                execute: async () => {
                    UserDialogs.Instance.ShowLoading("Wait...");
                    bool isSuceess = await Auth.RegisterUser(User);
                    if (isSuceess) {
                        UserDialogs.Instance.HideLoading();
                        DisplayPopUp = false;
                        Page app = Application.Current.MainPage;
                        await app.DisplayAlert("", "Account created successfully", "OK");
                    }
                    UserDialogs.Instance.HideLoading();
                },
                canExecute: () => {
                    RegisterAllowed = User != null
                    && !string.IsNullOrEmpty(User.Email)
                    && !string.IsNullOrEmpty(User.Password)
                    && User.Password.Equals(User.ConfirmPassword);
                    return RegisterAllowed;
                }) {
            };
            Login = new Command(
                execute: async () => {
                    UserDialogs.Instance.ShowLoading("Wait...");
                    var isSucess = await Auth.LoginUser(User);
                    if (isSucess) {
                        UserDialogs.Instance.HideLoading();
                        Application.Current.MainPage = new NavigationPage(new NotebooksPage());
                    }
                    UserDialogs.Instance.HideLoading();
                },
                canExecute: () => {
                    LoginAllowed = User != null
                    && !string.IsNullOrEmpty(User.Email)
                    && !string.IsNullOrEmpty(User.Password);
                    return LoginAllowed;
                });
        }
    }
}
