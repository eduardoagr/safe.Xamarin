using PropertyChanged;

using System;

namespace Safe.Model {
    [AddINotifyPropertyChangedInterface]
    public class User {
        [OnChangedMethod(nameof(OnSomePropertyChanged))]
        public string Email { get; set; }
        [OnChangedMethod(nameof(OnSomePropertyChanged))]
        public string Password { get; set; }
        [OnChangedMethod(nameof(OnSomePropertyChanged))]
        public string ConfirmPassword { get; set; }
        public Action OnAnyOfMyPropertiesChanged { get; set; }
        private void OnSomePropertyChanged() {
            OnAnyOfMyPropertiesChanged?.Invoke();
        }
    }
}
