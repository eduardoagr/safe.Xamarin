using PropertyChanged;

using Safe.Model;

using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace Safe.ViewModel {

    [AddINotifyPropertyChangedInterface]
    public class EditorVM {
        public string Text { get; set; }
        public ICommand SaveContent { get; set; }
        public Note RecivedNote { get; set; }
        public EditorVM() {

            SaveContent = new Command(() => { UpdateNote(); });
            MessagingCenter.Subscribe<NotesVM, Note>(this, "note", (obj, item) => {
                RecivedNote = item;
            });
        }

        private void UpdateNote() {
            throw new NotImplementedException();
        }
    }
}
