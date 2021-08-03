using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using GuessWho.Model;

using NedMaterialMVVM;
using NedMaterialMVVM.ViewModel;

namespace GuessWho.ViewModel {
    public class MainViewModel : PropertyChangedWrapper {
        private double _WindowWidth;
        private double _WindowHeight;
        private int _IconSize;
        private bool _ShowTooltips;
        private bool _ShowSettings;

        public ObservableCollection<Champion> Champions {
            get;
        } = new ObservableCollection<Champion>();

        public ObservableCollection<ChampionCategoryViewModel> Categories {
            get;
        } = new ObservableCollection<ChampionCategoryViewModel>();

        public HashSet<Champion> RejectedChampions {
            get;
            private set;
        }

        public double WindowWidth {
            get { return _WindowWidth; }
            set {
                _WindowWidth = value;
                RaisePropertyChanged();
            }
        }

        public double WindowHeight {
            get { return _WindowHeight; }
            set {
                _WindowHeight = value;
                RaisePropertyChanged();
            }
        }

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
        public ICommand OnClosing { get; }

        public ICommand ToggleSettings { get; }
        public ICommand RestoreChampion { get; }
        public ICommand RejectChampion { get; }
        public ICommand ResetGame { get; }
        public ICommand LoadConfig { get; }
        public ICommand SaveConfig { get; }
        public ICommand ResetConfig { get; }

        private DialogInformationViewModel DialogInformationViewModel { get; } = new DialogInformationViewModel();

        private DialogYesNoViewModel DialogYesNoViewModel { get; } = new DialogYesNoViewModel();

        private GuessWhoConfigManager ConfigManager { get; }
        public object DialogIdentifier1 { get; } = 1;
        public object DialogIdentifier2 { get; } = 2;

        public MainViewModel() {
            OnLoaded = new RelayCommand(ExecuteOnLoaded);
            OnClosing = new RelayCommand(ExecuteOnClosing);
            ToggleSettings = new RelayCommand(ExecuteToggleSettings);
            RestoreChampion = new RelayCommand<Champion>(ExecuteRestoreChampion);
            RejectChampion = new RelayCommand<Champion>(ExecuteRejectChampion);
            ResetGame = new RelayCommand(ExecuteResetGame);
            LoadConfig = new RelayCommand(ExecuteLoadConfig);
            SaveConfig = new RelayCommand(ExecuteSaveConfig);
            ResetConfig = new RelayCommand(ExecuteResetConfig);
            ConfigManager = new GuessWhoConfigManager();
        }

        public void LoadConfiguration() {
            ApplyConfiguration(ConfigManager.ReadConfig());
        }

        public void WriteConfiguration() {
            ConfigManager.WriteConfig(GetCurrentConfig());
        }

        public GuessWhoConfig GetCurrentConfig() {
            return new GuessWhoConfig {
                IconSize = IconSize,
                ShowTooltips = ShowTooltips,
                WindowWidth = WindowWidth,
                WindowHeight = WindowHeight,
                RejectedChampions = RejectedChampions,
                Categories = Categories.Select(c => new ChampionCategory { CategoryName = c.CategoryName, Champions = c.Champions, IsSelected = c.IsSelected }).ToList()
            };
        }

        public void ExecuteRestoreChampion(Champion champion) {
            RejectedChampions.Remove(champion);
            RevalidateChampions();
        }

        public void ExecuteRejectChampion(Champion champion) {
            RejectedChampions.Add(champion);
            Champions.Remove(champion);
        }

        private void ApplyConfiguration(GuessWhoConfig config) {
            WindowWidth = config.WindowWidth;
            WindowHeight = config.WindowHeight;
            IconSize = config.IconSize;
            ShowTooltips = config.ShowTooltips;
            RejectedChampions = config.RejectedChampions;
            foreach (ChampionCategoryViewModel vm in Categories) {
                vm.PropertyChanged -= ChampionCategory_PropertyChanged;
            }
            Categories.Clear();
            foreach (ChampionCategory category in config.Categories) {
                ChampionCategoryViewModel vm = new ChampionCategoryViewModel(category);
                vm.PropertyChanged += ChampionCategory_PropertyChanged;
                Categories.Add(vm);
            }

            RevalidateChampions();
        }

        private void ChampionCategory_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case nameof(ChampionCategoryViewModel.IsSelected):
                    RevalidateChampions();
                    break;
            }
        }

        private void RevalidateChampions() {
            Champions.Clear();
            foreach (Champion champ in ChampionProvider.AllChampionsAlphabetical) {
                if (!RejectedChampions.Contains(champ) && !Categories.Any(c => c.Champions.Contains(champ) && !c.IsSelected)) {
                    Champions.Add(champ);
                }
            }
        }

        private void ExecuteOnLoaded() {
            LoadConfiguration();
        }

        private void ExecuteOnClosing() {
            WriteConfiguration();
        }

        private void ExecuteToggleSettings() {
            ShowSettings = !ShowSettings;
        }

        private void ExecuteResetGame() {
            ShowYesNoDialog("Zamierzasz zresetować grę. Ustawienia pozostaną nienaruszone.\nNa pewno?", () =>
            {
                GuessWhoConfig config = GetCurrentConfig();
                config.RejectedChampions.Clear();
                foreach (ChampionCategory category in config.Categories) {
                    category.IsSelected = true;
                }
                ApplyConfiguration(config);
                try {
                    WriteConfiguration();
                } catch (Exception e) {
                    ShowInformationDialog($"Gra została zresetowana, lecz w trakcie zapisu wystąpił błąd!\n{e}");
                }
            });
        }

        private void ExecuteLoadConfig() {
            ShowYesNoDialog("Zamierzasz wczytać ponownie konfigurację. Wszelkie niezapisane zmiany zostaną utracone!\nNa pewno?",
                () =>
                {
                    try {
                        LoadConfiguration();
                    } catch (Exception e) {
                        ShowInformationDialog($"Nie udało się wczytać konfiguracji!\n{e}");
                    }
                });
        }

        private void ExecuteSaveConfig() {
            try {
                WriteConfiguration();
                ShowInformationDialog("Konfiguracja zapisana!");
            } catch (Exception e) {
                ShowInformationDialog($"Nie udało się zapisać konfiguracji!\n{e}");
            }
        }

        private void ExecuteResetConfig() {
            ShowYesNoDialog("Zamierzasz zresetować grę i wszystkie ustawienia.\nNa pewno?", () =>
            {
                ApplyConfiguration(GuessWhoConfig.GetDefaultConfig());
                try {
                    WriteConfiguration();
                } catch (Exception e) {
                    ShowInformationDialog($"Konfiguracja została zresetowana, lecz w trakcie zapisu wystąpił błąd!\n{e}");
                }
            });
        }

        public void ShowYesNoDialog(string message, Action yesAction = null, Action noAction = null) {
            DialogYesNoViewModel.OpenDialog(DialogIdentifier1, message, yesAction, noAction);
        }

        public void ShowInformationDialog(string message) {
            DialogInformationViewModel.OpenDialog(DialogIdentifier2, message);
        }
    }

}
