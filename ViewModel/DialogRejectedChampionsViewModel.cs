using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using GuessWho.Model;

using NedMaterialMVVM;
using NedMaterialMVVM.ViewModel;

namespace GuessWho.ViewModel {
    public class DialogRejectedChampionsViewModel : DialogBaseViewModel {
        public DialogRejectedChampionsViewModel(MainViewModel mainViewModel) {
            MainViewModel = mainViewModel;
            Close = new RelayCommand(ExecuteClose);
            RestoreChampion = new RelayCommand<Champion>(ExecuteRestoreChampion);
        }

        public ObservableCollection<Champion> RejectedChampions { get; } = new ObservableCollection<Champion>();

        private MainViewModel MainViewModel { get; }

        public ICommand Close { get; }
        public ICommand RestoreChampion { get; }

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