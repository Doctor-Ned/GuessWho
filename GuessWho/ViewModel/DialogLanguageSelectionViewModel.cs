using System;
using System.Dynamic;
using System.Linq;
using System.Windows.Input;

using GuessWhoResources;

using NedMaterialMVVM;
using NedMaterialMVVM.ViewModel;

namespace GuessWho.ViewModel {
    public class DialogLanguageSelectionViewModel : DialogBaseViewModel {
        public DialogLanguageSelectionViewModel(MainViewModel mainViewModel) {
            MainViewModel = mainViewModel;
            Close = new RelayCommand(ExecuteClose);
        }

        public MainViewModel MainViewModel { get; }

        public Locale[] Locales { get; } = Enum.GetValues(typeof(Locale)).Cast<Locale>().OrderBy(l => l.ToString()).ToArray();

        public ICommand Close { get; }

        private void ExecuteClose() {
            CloseDialog();
        }

        public new void OpenDialog() {
            base.OpenDialog();
        }

        public new void OpenIDialog(object dialogIdentifier) {
            base.OpenIDialog(dialogIdentifier);
        }
    }
}