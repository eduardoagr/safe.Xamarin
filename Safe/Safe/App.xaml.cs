
using Safe.View;

using Syncfusion.Licensing;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace Safe {
    public partial class App : Application {
        private const string KEY = "NDY1Njk2QDMxMzkyZTMyMmUzMExaM3hGVTJlQUhTQjJUclR5K1BCaFlFb0dTalpkZGF0SU1BZk1ZVzN6T009";
        public static string UserId = string.Empty;
        public App() {
            SyncfusionLicenseProvider.RegisterLicense(KEY);
            NetworkAccess current = Connectivity.NetworkAccess;

            InitializeComponent();

            if (current == NetworkAccess.Internet) {
                MainPage = new LoginPage();
            } else {
                MainPage = new NoInternetPage();
            }
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {

        }
    }
}
