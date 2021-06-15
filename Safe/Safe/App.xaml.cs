using Safe.View;

using Syncfusion.Licensing;

using Xamarin.Forms;

namespace Safe {
    public partial class App : Application {
        const string KEY = "NDU4ODU0QDMxMzkyZTMxMmUzMEt2UE5NVkJRbU5Ud3lFT0oyWVVPWWU0a2gzLzZPbmN0dm5TYS9La2UybzA9";
        public App() {
            SyncfusionLicenseProvider.RegisterLicense(KEY);
            InitializeComponent();

            MainPage = new LoginPage();
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}
