using Safe.Firebase;
using Safe.Model;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Xamarin.Forms;

namespace Safe.ViewModel {
    public class NotesVM {
        public ObservableCollection<Note> NotesCollection { get; set; }
        public ICommand CreateNewNoteCommand { get; set; }
        public Notebook RecivedNotebook { get; set; }

        public NotesVM() {

            NotesCollection = new ObservableCollection<Note>();

            MessagingCenter.Subscribe<NotebooksVM, Notebook>(this, "data",
            async (sender, data) => {
                if (data != null) {
                    RecivedNotebook = data;
                } else {
                    return;
                }

                CreateNewNoteCommand = new Command(() => {CeateNote(RecivedNotebook);});


                var notes = await Database.ReadAsync<Note>();
                if (notes != null) {
                    notes.Where(n => n.NotebookId == RecivedNotebook.Id);
                    NotesCollection.Clear();
                    foreach (var item in notes) {
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

