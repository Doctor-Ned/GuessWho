using System;
using System.Collections.ObjectModel;

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

        public MainViewModel() {
            foreach (Champion champ in Enum.GetValues(typeof(Champion))) {
                Champions.Add(champ);
            }
        }
    }

}
