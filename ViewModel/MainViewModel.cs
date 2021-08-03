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
        private bool _ShowTooltips = true;
        private bool _ShowSettings = false;

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

        public bool ShowTooltips {
            get { return _ShowTooltips; }
            set {
                _ShowTooltips = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowSettings {
            get { return _ShowSettings; }
            set {
                _ShowSettings = value;
                RaisePropertyChanged();
            }
        }

        public ICommand OnLoaded { get; }

        public ICommand ToggleSettings { get; }

        public MainViewModel() {
            OnLoaded = new RelayCommand(ExecuteOnLoaded);
            ToggleSettings = new RelayCommand(ExecuteToggleSettings);
        }

        private void ExecuteOnLoaded() {
            ReplaceChampions(Enum.GetValues(typeof(Champion)).Cast<Champion>());
        }

        private void ExecuteToggleSettings() {
            ShowSettings = !ShowSettings;
        }

        public void ReplaceChampions(IEnumerable<Champion> champions) {
            Champions.Clear();
            foreach (Champion champ in champions.Distinct().OrderByChampionName()) {
                Champions.Add(champ);
            }
        }
    }

}
