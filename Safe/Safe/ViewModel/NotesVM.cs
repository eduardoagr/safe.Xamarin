using Safe.Firebase;
using Safe.Model;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;

namespace Safe.ViewModel {
    public class NotesVM {
        public ObservableCollection<Note> NotesCollection { get; set; }
        public ICommand CreateNewNoteCommand { get; set; }
        public Notebook RecivedNotebook { get; set; }

        public NotesVM() {

            NotesCollection = new ObservableCollection<Note>();

            CreateNewNoteCommand = new Command(() => {
                CeateNote(RecivedNotebook);
            });

            MessagingCenter.Subscribe<NotebooksVM, Notebook>(this, "data",
            async (sender, data) => {
                RecivedNotebook = data;
                var notes = await Database.ReadAsync<Note>();
                if (notes != null) {
                    var newNotes = notes.Where(n => n.NotebookId == RecivedNotebook.Id);
                    NotesCollection.Clear();
                    foreach (var item in newNotes) {
                        NotesCollection.Add(item);
                    }
                }
            });
        }

        private async void CeateNote(Notebook recivedNotebook) {

            var result = await Application.Current.MainPage.DisplayPromptAsync(string.Empty,
                 "Notebook name?");

            if (!string.IsNullOrEmpty(result)) {
                var note = new Note() {
                    NotebookId = recivedNotebook.Id,
                    Title = result,
                    CreatedAt = DateTime.Now.ToString("ddd, MMMM yyyy")
                };
                await Database.InsertAsync(note);
            } else {
                return;
            }

        }
    }
}

