using MaterialFonts.Fonts;

using PropertyChanged;

using Safe.Firebase;
using Safe.Model;
using Safe.View;

using Syncfusion.ListView.XForms;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Xsl;

using Xamarin.Forms;

namespace Safe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class NotesVM {
        public ObservableCollection<Note> NotesCollection { get; set; }
        public ICommand ChangeLayoutCommand { get; set; }
        public ICommand CreateNewNoteCommand { get; set; }
        public Notebook RecivedNotebook { get; set; }
        public LayoutBase LayoutBase { get; set; }
        public Note SelectedNote { get; set; }
        public ICommand SelectedNoteCommand { get; set; }
        public FontImageSource Glyph { get; set; }

        public NotesVM() {

            LayoutBase = new LinearLayout();

            NotesCollection = new ObservableCollection<Note>();

            CreateNewNoteCommand = new Command(async () => {
                var note = await CreateNote(RecivedNotebook);
                await GetNotes(RecivedNotebook);
            });

            SelectedNoteCommand = new Command(async () => {
                await Application.Current.MainPage.Navigation.PushAsync(new EditorPage());
                MessagingCenter.Send(this, "note", SelectedNote);
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

        private async Task<Note> CreateNote(Notebook recivedNotebook) {

            var result = await Application.Current.MainPage.DisplayPromptAsync(string.Empty,
                 "Note name?");

            if (!string.IsNullOrEmpty(result)) {
                var note = new Note() {
                    NotebookId = recivedNotebook.Id,
                    Title = result,
                    CreatedAt = DateTime.Now.ToString("ddd, MMMM yyyy")
                };
                await Database.InsertAsync(note);
                return note;

            } else {
                return null;
            }
        }
        private void OnChangeLayout(object obj) {
            if (LayoutBase is LinearLayout) {
                LayoutBase = new GridLayout();
                Glyph = new FontImageSource {
                    Glyph = IconFonts.Grid,
                    FontFamily = "material",
                    Size = 44
                };
                Debug.WriteLine("I am a grid");
            } else {
                LayoutBase = new LinearLayout();
                Glyph = new FontImageSource {
                    Glyph = IconFonts.FormatListBulleted,
                    FontFamily = "material",
                    Size = 44
                };

            }
        }
    }
}

