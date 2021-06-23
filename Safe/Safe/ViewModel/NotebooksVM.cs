using PropertyChanged;

using Safe.Firebase;
using Safe.Model;
using Safe.View;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;

namespace Safe.ViewModel {
    [AddINotifyPropertyChangedInterface]
    public class NotebooksVM {
        public ObservableCollection<Notebook> NotebooksCollection { get; set; }
        public ICommand CreateNewNotebook { get; set; }
        public ICommand SelectedNoteBookCommand { get; set; }
        public Notebook SelectedNotebook { get; set; }

        public NotebooksVM() {
            NotebooksCollection = new ObservableCollection<Notebook>();
            CreateNewNotebook = new Command(async () => {
                var result = await Application.Current.MainPage.DisplayPromptAsync(string.Empty,
                    "Notebook name?");

                if (!string.IsNullOrEmpty(result)) {
                    Notebook notebook = new Notebook() {
                        Name = result,
                        UserId = App.UserId,
                        CreatedAt = DateTime.Now.ToString("dddd, dd MMMM yyyy")
                    };
                    await Database.InsertAsync(notebook);
                    GetNotebooksAsync();
                }
            });
            SelectedNoteBookCommand = new Command(async () => {
                MessagingCenter.Send(this, "details", SelectedNotebook);
                await Application.Current.MainPage.Navigation.PushAsync(new NotesPage());
                SelectedNotebook = null;
            });
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
    }
}