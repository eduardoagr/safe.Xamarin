using System.Windows.Input;

using Xamarin.Forms;

namespace Safe.ViewModel {
    public class EditorVM {
        public ICommand SpeakBton { get; set; }
        public string Title { get; set; }
        public EditorVM() {

            SpeakBton = new Command(() => {

            });

            MessagingCenter.Subscribe<NotebooksVM, string>(this, "details",
               (obj, item) => {
                   Title = item;
               });
        }
    }
}
