using PropertyChanged;

using Safe.Firebase;
using Safe.Model;

using Syncfusion.ListView.XForms;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace Safe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class NotesVM {
        public ObservableCollection<Note> NotesCollection { get; set; }
        public ICommand ChangeLayoutCommand { get; set; }
        public ICommand CreateNewNoteCommand { get; set; }
        public Notebook RecivedNotebook { get; set; }
        public LayoutBase LayoutBase { get; set; }

        public NotesVM() {

            LayoutBase = new LinearLayout();

            NotesCollection = new ObservableCollection<Note>();

            CreateNewNoteCommand = new Command(async () => {
                await CreateNote(RecivedNotebook);
                await GetNotes(RecivedNotebook);
            });

            MessagingCenter.Subscribe<NotebooksVM, Notebook>(this, "data",
            async (sender, data) => {
                RecivedNotebook = data;
                await GetNotes(RecivedNotebook);
            });

            ChangeLayoutCommand = new Command(OnChangeLayout);
        }

        private async Task GetNotes(Notebook notebook) {
            var notes = await Database.ReadAsync<Note>();
            if (notes != null) {
                var newNotes = notes.Where(n => n.NotebookId == notebook.Id);
                NotesCollection.Clear();
                foreach (var item in newNotes) {
                    NotesCollection.Add(item);
                }
            }
        }

        private async Task<bool> CreateNote(Notebook recivedNotebook) {

            var result = await Application.Current.MainPage.DisplayPromptAsync(string.Empty,
                 "Notebook name?");

            if (!string.IsNullOrEmpty(result)) {
                var note = new Note() {
                    NotebookId = recivedNotebook.Id,
                    Title = result,
                    CreatedAt = DateTime.Now.ToString("ddd, MMMM yyyy")
                };
                await Database.InsertAsync(note);
                return true;

            } else {
                return false;
            }
        }
        private void OnChangeLayout(object obj) {
            if (LayoutBase is LinearLayout) {
                LayoutBase = new GridLayout();

                // new Gliph
            } else {
                LayoutBase = new LinearLayout();

                //new Gliph
            }
        }
    }
}

