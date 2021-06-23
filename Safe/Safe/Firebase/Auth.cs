using Newtonsoft.Json;

using Safe.Model;

using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Safe.Firebase {
    public class Auth {
        private static readonly string FirebaseKey = "AIzaSyA-ZJyEl0cQJlu0LUKBgOv3Q-1Vjr2-qaY";
        public static async Task<bool> RegisterUser(User user) {

            using (HttpClient client = new HttpClient()) {
                var bodyJson = JsonConvert.SerializeObject(new {
                    email = user.Email,
                    password = user.Password,
                    returnSecureToken = true });

                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync
                    ($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={FirebaseKey}", data);
                if (response.IsSuccessStatusCode) {
                    string resutJson = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<AuthResult>(resutJson);
                    App.UserId = res.localId;

                    return true;

                } else {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<AuthError>(errorJson);
                    DispayError(res);

                    return false;
                }
            }
        }
        public static async Task<bool> LoginUser(User user) {

            using (HttpClient client = new HttpClient()) {
                var bodyJson = JsonConvert.SerializeObject(new {
                    email = user.Email,
                    password = user.Password,
                    returnSecureToken = true
                });

                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync
                    ($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={FirebaseKey}", data);
                if (response.IsSuccessStatusCode) {
                    string resutJson = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<AuthResult>(resutJson);
                    App.UserId = res.localId;
                    return true;

                } else {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<AuthError>(errorJson);
                    DispayError(res);

                    return false;
                }
            }
        }

        private static void DispayError(AuthError res) {
            Application.Current.MainPage.DisplayAlert("Error", res.error.message, "OK");
        }
    }
}