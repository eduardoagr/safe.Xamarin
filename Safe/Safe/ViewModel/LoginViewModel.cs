using PropertyChanged;

using Safe.Model;

using System.Windows.Input;

using Xamarin.Forms;

namespace Safe.ViewModel {
    [AddINotifyPropertyChangedInterface]
    public class LoginViewModel {
        public ICommand OpenPopupCommand { get; set; }
        public ICommand SignIn { get; set; }
        public bool DisplayPopUp { get; set; }
        public User User { get; set; }
        public LoginViewModel() {

            OpenPopupCommand = new Command(() => {
                DisplayPopUp = true;
            });
            SignIn = new Command()
        }
    }
}
