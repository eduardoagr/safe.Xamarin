
using PropertyChanged;

using Safe.Firebase;
using Safe.Model;
using Safe.View;

using Syncfusion.ListView.XForms;

using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        public NotebooksVM() {

            LayoutBase = new LinearLayout();

            NotebooksCollection = new ObservableCollection<Notebook>();

            CreateNewNotebook = new Command(async () => {
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
                } else {
                    return;
                }
            });
            SelectedNoteBookCommand = new Command(async () => {
                await Application.Current.MainPage.Navigation.PushAsync(new NotesPage());
                MessagingCenter.Send(this, "details", SelectedNotebook);
                SelectedNotebook = null;
            });
            ChangeLayoutCommand = new Command(OnChangeLayout);
            GetNotebooksAsync();
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

        private void OnChangeLayout(object obj) {
            if (LayoutBase is LinearLayout) {
                LayoutBase = new GridLayout();
            } else {
                LayoutBase = new LinearLayout();
            }
        }
    }
}