using MaterialFonts.Fonts;

using PropertyChanged;

using Safe.Firebase;
using Safe.Model;
using Safe.View;

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
        public Note SelectedNote { get; set; }
        public ICommand SelectedNoteCommand { get; set; }
        public FontImageSource Glyph { get; set; }
        public NotesVM() {

            Glyph = new FontImageSource() {
                Glyph = IconFonts.FormatListBulleted,
                FontFamily = "material",
                Size = 44
            };

            LayoutBase = new LinearLayout();

            NotesCollection = new ObservableCollection<Note>();

            CreateNewNoteCommand = new Command(async () => {
                var note = await CreateNoteAsync(RecivedNotebook);
                await Application.Current.MainPage.Navigation.PushAsync(new EditorPage());
                MessagingCenter.Send(this, "note", note);
                await GetNotesAsync(RecivedNotebook);
            });

            SelectedNoteCommand = new Command(async () => {
                await Application.Current.MainPage.Navigation.PushAsync(new EditorPage());
                MessagingCenter.Send(this, "note", SelectedNote);
                SelectedNote = null;

            });

            MessagingCenter.Subscribe<NotebooksVM, Notebook>(this, "data",
            async (sender, data) => {
                RecivedNotebook = data;
                await GetNotesAsync(RecivedNotebook);
            });

            ChangeLayoutCommand = new Command(OnChangeLayout);
        }

        private async Task GetNotesAsync(Notebook notebook) {
            var notes = await Database.ReadAsync<Note>();
            if (notes != null) {
                notes = notes.Where(n => n.NotebookId == notebook.Id).ToList();
                NotesCollection.Clear();
                foreach (var item in notes) {
                    NotesCollection.Add(item);
                }
            }
        }

        private async Task<Note> CreateNoteAsync(Notebook recivedNotebook) {

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
        private void OnChangeLayout() {
            if (LayoutBase is LinearLayout) {
                LayoutBase = new GridLayout();
                Glyph = new FontImageSource {
                    Glyph = IconFonts.Grid,
                    FontFamily = "material",
                    Size = 44
                };
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

