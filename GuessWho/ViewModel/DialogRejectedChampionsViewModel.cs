using System.Windows.Input;

using NedMaterialMVVM;
using NedMaterialMVVM.ViewModel;

namespace GuessWho.ViewModel {
    public class DialogRejectedChampionsViewModel : DialogBaseViewModel {
        private DialogTwoButtonViewModel DialogYesNoViewModel { get; }

        public DialogRejectedChampionsViewModel(MainViewModel mainViewModel) {
            MainViewModel = mainViewModel;
            Close = new RelayCommand(ExecuteClose);
            RestoreChampion = new RelayCommand<string>(ExecuteRestoreChampion);
            RestoreAll = new RelayCommand(ExecuteRestoreAll);
            DialogYesNoViewModel = new DialogTwoButtonViewModel {
                Button1Action = () =>
                {
                    MainViewModel.RejectedChampions.Clear();
                    CloseDialog();
                },
                Button1Text = "TAK",
                Button2Text = "NIE",
                Message = "Przywrócisz wszystkie odrzucone postacie.\nCzy na pewno?"
            };
        }

        private MainViewModel MainViewModel { get; }

        public ICommand Close { get; }
        public ICommand RestoreChampion { get; }
        public ICommand RestoreAll { get; }

        private void ExecuteClose() {
            CloseDialog();
        }

        private void ExecuteRestoreChampion(string champId) {
            MainViewModel.RejectedChampions.Remove(champId);
            if (!MainViewModel.AnyChampionsRejected) {
                CloseDialog();
            }
        }

        private void ExecuteRestoreAll() {
            DialogYesNoViewModel.OpenIDialog(MainViewModel.DialogIdentifier2);
        }

        public new void OpenDialog() {
            base.OpenDialog();
        }

        public new void OpenIDialog(object dialogIdentifier) {
            base.OpenIDialog(dialogIdentifier);
        }
    }
}