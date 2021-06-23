using Safe.Firebase;
using Safe.Model;

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace Safe.ViewModel {
    public class NotesVM {
        public ObservableCollection<Note> NotesCollection { get; set; }
        public ICommand CreateNewNote { get; set; }

        public NotesVM() {

            NotesCollection = new ObservableCollection<Note>();

            CreateNewNote = new Command(() => {


            });
            MessagingCenter.Subscribe<NotebooksVM, Notebook>(this, "details", async (obj, item) => {
                Notebook newNotebook = item;

                MainThread.BeginInvokeOnMainThread(async () => {
                    var notes = await Database.ReadAsync<Note>();
                    if (notes != null) {
                        var notebookNotes = notes.Where(n => n.NotebookId == newNotebook.Id);
                        NotesCollection.Clear();
                        foreach (var element in notebookNotes) {
                            NotesCollection.Add(element);
                        }
                    }
                });
             
            });
        }
    }
}
