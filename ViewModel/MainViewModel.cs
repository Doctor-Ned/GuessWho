using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using GuessWho.Model;

using NedMaterialMVVM;

namespace GuessWho.ViewModel {
    public class MainViewModel : PropertyChangedWrapper {
        private int _IconSize = 60;

        public ObservableCollection<Champion> Champions {
            get;
        } = new ObservableCollection<Champion>();

        public int IconSize {
            get { return _IconSize; }
            set {
                _IconSize = value;
                RaisePropertyChanged();
            }
        }

        public ICommand OnLoaded { get; }

        public MainViewModel() {
            OnLoaded = new RelayCommand(ExecuteOnLoaded);
        }

        private void ExecuteOnLoaded() {
            ReplaceChampions(Enum.GetValues(typeof(Champion)).Cast<Champion>());
        }

        public void ReplaceChampions(IEnumerable<Champion> champions) {
            Champions.Clear();
            foreach (Champion champ in champions.Distinct().OrderByChampionName()) {
                Champions.Add(champ);
            }
        }
    }

}
