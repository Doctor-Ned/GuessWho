using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using GuessWho.Model;

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
                    RejectedChampions.Clear();
                    MainViewModel.RejectedChampions.Clear();
                    CloseDialog();
                },
                Button1Text = "TAK",
                Button2Text = "NIE",
                Message = "Przywrócisz wszystkie odrzucone postacie.\nCzy na pewno?"
            };
        }

        public MainViewModel MainViewModel { get; }

        public ObservableCollection<string> RejectedChampions { get; } = new ObservableCollection<string>();

        public ICommand Close { get; }
        public ICommand RestoreChampion { get; }
        public ICommand RestoreAll { get; }

        private void ExecuteClose() {
            CloseDialog();
        }

        private void ExecuteRestoreChampion(string champId) {
            RejectedChampions.Remove(champId);
            MainViewModel.RejectedChampions.Remove(champId);
            if (!MainViewModel.AnyChampionsRejected) {
                CloseDialog();
            }
        }

        private void ExecuteRestoreAll() {
            DialogYesNoViewModel.OpenIDialog(MainViewModel.DialogIdentifier2);
        }

        public new void OpenDialog() {
            InitializeRejectedChampions();
            base.OpenDialog();
        }

        public new void OpenIDialog(object dialogIdentifier) {
            InitializeRejectedChampions();
            base.OpenIDialog(dialogIdentifier);
        }

        private void InitializeRejectedChampions() {
            RejectedChampions.Clear();
            foreach (string champ in MainViewModel.RejectedChampions.OrderBy(
                ResourceProvider.GetLocalizedChampionName, StringComparer.CurrentCultureIgnoreCase)) {
                RejectedChampions.Add(champ);
            }
        }
    }
}