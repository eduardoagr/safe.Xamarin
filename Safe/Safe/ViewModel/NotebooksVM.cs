
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
    public class NotebooksVM {
        public ObservableCollection<Notebook> NotebooksCollection { get; set; }
        public ICommand ChangeLayoutCommand { get; set; }
        public ICommand CreateNewNotebook { get; set; }
        public ICommand SelectedNoteBookCommand { get; set; }
        public Notebook SelectedNotebook { get; set; }
        public LayoutBase LayoutBase { get; set; }
        public FontImageSource Glyph { get; set; }

        public NotebooksVM() {

            Glyph = new FontImageSource() {
                Glyph = IconFonts.FormatListBulleted,
                FontFamily = "material",
                Size = 44
            };

            LayoutBase = new LinearLayout();

            NotebooksCollection = new ObservableCollection<Notebook>();

            CreateNewNotebook = new Command(async () => {
                await CreateNote();
            });
            GetNotebooksAsync();
            SelectedNoteBookCommand = new Command(async () => {
                await Application.Current.MainPage.Navigation.PushAsync(new NotesPage());
                MessagingCenter.Send(this, "data", SelectedNotebook);
                SelectedNotebook = null;

            });
            ChangeLayoutCommand = new Command(OnChangeLayout);
        }

        private async void GetNotebooksAsync() {
            var notebooks = await Database.ReadAsync<Notebook>();
            if (notebooks != null) {
                notebooks.Where(notebook => notebook.Id == App.UserId);
                NotebooksCollection.Clear();
                foreach (var item in notebooks) {
                    NotebooksCollection.Add(item);
                }

            }
        }

        private async Task CreateNote() {
            var result = await Application.Current.MainPage.DisplayPromptAsync(string.Empty,
                "Notebook name?");

            if (!string.IsNullOrEmpty(result)) {
                var notebook = new Notebook() {
                    Name = result,
                    UserId = App.UserId,
                    CreatedAt = DateTime.Now.ToString("ddd, MMMM yyyy")
                };
                await Database.InsertAsync(notebook);
                GetNotebooksAsync();
                await Application.Current.MainPage.Navigation.PushAsync(new NotesPage());
                MessagingCenter.Send(this, "data", result);
            } else {
                return;
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