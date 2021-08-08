using System.Collections.Generic;
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
            RestoreChampion = new RelayCommand<Champion>(ExecuteRestoreChampion);
            RestoreAll = new RelayCommand(ExecuteRestoreAll);
            DialogYesNoViewModel = new DialogTwoButtonViewModel {
                Button1Action = () =>
                {
                    RejectedChampions.Clear();
                    MainViewModel.RestoreAllChampions();
                    CloseDialog();
                },
                Button1Text = "TAK",
                Button2Text = "NIE",
                Message = "Przywrócisz wszystkie odrzucone postacie.\nCzy na pewno?"
            };
        }

        public ObservableCollection<Champion> RejectedChampions { get; } = new ObservableCollection<Champion>();

        private MainViewModel MainViewModel { get; }

        public ICommand Close { get; }
        public ICommand RestoreChampion { get; }
        public ICommand RestoreAll { get; }

        private void ExecuteClose() {
            CloseDialog();
        }

        private void ExecuteRestoreChampion(Champion champion) {
            RejectedChampions.Remove(champion);
            MainViewModel.RestoreChampion(champion);
            if (!RejectedChampions.Any()) {
                CloseDialog();
            }
        }

        private void ExecuteRestoreAll() {
            DialogYesNoViewModel.OpenDialog(MainViewModel.DialogIdentifier2);
        }

        public void OpenDialog(HashSet<Champion> rejectedChampions) {
            Inject(rejectedChampions);
            OpenDialog();
        }

        public void OpenDialog(object dialogIdentifier, HashSet<Champion> rejectedChampions) {
            Inject(rejectedChampions);
            OpenDialog(dialogIdentifier);
        }

        private void Inject(HashSet<Champion> rejectedChampions) {
            RejectedChampions.Clear();
            foreach (Champion champ in rejectedChampions.OrderByChampionName()) {
                RejectedChampions.Add(champ);
            }
        }
    }
}