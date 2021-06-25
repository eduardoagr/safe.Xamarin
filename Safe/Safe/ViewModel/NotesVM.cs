using Safe.Firebase;
using Safe.Model;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;

namespace Safe.ViewModel {
    public class NotesVM {
        public ObservableCollection<Note> NotesCollection { get; set; }
        public ICommand CreateNewNote { get; set; }
        public Notebook RecivedNotebook { get; set; }

        public NotesVM() {

            NotesCollection = new ObservableCollection<Note>();

            CreateNewNote = new Command(async () => {
                var result = await Application.Current.MainPage.DisplayPromptAsync(string.Empty,
                                 "Note name?");
                if (!string.IsNullOrEmpty(result)) {
                    var note = new Note() {
                        NotebookId = RecivedNotebook.Id,
                        CreatedAt = DateTime.Now.ToString("ddd, MMMM yyyy"),
                        UpdatedAt = DateTime.Now.ToString("ddd, MMMM yyyy"),
                        Title = result
                    };
                    await Database.InsertAsync(note);
                    GetNotes();
                } else {
                    return;
                }
            });

            MessagingCenter.Subscribe<NotebooksVM, Notebook>(this, "details",
                (obj, item) => {
                    RecivedNotebook = item;
                });

            GetNotes();
        }

        private async void GetNotes() {
            var notes = await Database.ReadAsync<Note>();

            foreach (var item in notes) {
                Debug.WriteLine(item.NotebookId);
            }
            if (notes != null) {
                notes.Where(n => n.NotebookId == RecivedNotebook.Id);
                NotesCollection.Clear();
                foreach (var element in notes) {
                    NotesCollection.Add(element);
                }
            }
        }
    }
}
