using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

using GuessWho.Model;

using NedMaterialMVVM;
using NedMaterialMVVM.ViewModel;

namespace GuessWho.ViewModel {
    public class MainViewModel : PropertyChangedWrapper {

        #region Backing fields

        private double _CategoryCheckBoxSize;
        private double _CategoryFontSize;
        private double _IconSize;
        private bool _ShowSettings;
        private bool _ShowTooltips;
        private double _SidePanelWidth = 220.0;
        private double _WindowHeight;
        private double _WindowWidth;

        #endregion

        public MainViewModel() {
            OnLoaded = new RelayCommand(ExecuteOnLoaded);
            OnClosing = new RelayCommand(ExecuteOnClosing);
            ToggleSettings = new RelayCommand(ExecuteToggleSettings);
            RestoreChampions = new RelayCommand(ExecuteRestoreChampions);
            RejectChampion = new RelayCommand<Champion>(ExecuteRejectChampion);
            ResetGame = new RelayCommand(ExecuteResetGame);
            LoadConfig = new RelayCommand(ExecuteLoadConfig);
            SaveConfig = new RelayCommand(ExecuteSaveConfig);
            ResetConfig = new RelayCommand(ExecuteResetConfig);
            ConfigManager = new GuessWhoConfigManager();
            DialogRejectedChampionsViewModel = new DialogRejectedChampionsViewModel(this);
        }

        #region Public properties

        public ObservableCollection<ChampionDescription> Champions { get; } = new ObservableCollection<ChampionDescription>();

        public ObservableCollection<ChampionCategoryViewModel> Categories { get; } =
            new ObservableCollection<ChampionCategoryViewModel>();

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

        public double CategoryCheckBoxSize {
            get { return _CategoryCheckBoxSize; }
            set {
                _CategoryCheckBoxSize = value;
                RaisePropertyChanged();
            }
        }

        public double CategoryFontSize {
            get { return _CategoryFontSize; }
            set {
                _CategoryFontSize = value;
                RaisePropertyChanged();
            }
        }

        public double SidePanelWidth {
            get { return _SidePanelWidth; }
            set {
                _SidePanelWidth = value;
                RaisePropertyChanged();
            }
        }

        public double IconSize {
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

        public bool AnyChampionsRejected {
            get { return RejectedChampions != null && RejectedChampions.Any(); }
        }
        public object DialogIdentifier1 { get; } = 1;
        public object DialogIdentifier2 { get; } = 2;

        #endregion

        #region Commands

        public ICommand OnLoaded { get; }
        public ICommand OnClosing { get; }

        public ICommand ToggleSettings { get; }
        public ICommand RestoreChampions { get; }
        public ICommand RejectChampion { get; }
        public ICommand ResetGame { get; }
        public ICommand LoadConfig { get; }
        public ICommand SaveConfig { get; }
        public ICommand ResetConfig { get; }

        #endregion

        #region Private properties

        private Dictionary<Champion, ChampionDescription> AllChampionDescriptions { get; } = new Dictionary<Champion, ChampionDescription>();

        private HashSet<Champion> RejectedChampions { get; set; }

        private DialogOneButtonViewModel DialogOneButtonViewModel { get; } = new DialogOneButtonViewModel {
            ButtonText = "OK"
        };

        private DialogTwoButtonViewModel DialogTwoButtonViewModel { get; } = new DialogTwoButtonViewModel {
            Button1Text = "TAK",
            Button2Text = "NIE"
        };

        private DialogRejectedChampionsViewModel DialogRejectedChampionsViewModel { get; }

        private GuessWhoConfigManager ConfigManager { get; }

        #endregion

        public void RestoreChampion(Champion champion) {
            RejectedChampions.Remove(champion);
            RevalidateChampions();
            RaisePropertyChanged(nameof(AnyChampionsRejected));
        }

        #region Private methods

        #region Command actions

        private void ExecuteRestoreChampions() {
            DialogRejectedChampionsViewModel.OpenDialog(DialogIdentifier1, RejectedChampions);
        }

        private void ExecuteRejectChampion(Champion champion) {
            RejectedChampions.Add(champion);
            Champions.Remove(AllChampionDescriptions[champion]);
            RaisePropertyChanged(nameof(AnyChampionsRejected));
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
            ShowYesNoDialog(
                "Zamierzasz wczytać ponownie konfigurację. Wszelkie niezapisane zmiany zostaną utracone!\nNa pewno?",
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
                    ShowInformationDialog(
                        $"Konfiguracja została zresetowana, lecz w trakcie zapisu wystąpił błąd!\n{e}");
                }
            });
        }

        #endregion

        private void LoadConfiguration() {
            ApplyConfiguration(ConfigManager.ReadConfig());
        }

        private void WriteConfiguration() {
            ConfigManager.WriteConfig(GetCurrentConfig());
        }

        private GuessWhoConfig GetCurrentConfig() {
            return new GuessWhoConfig {
                WindowWidth = WindowWidth,
                WindowHeight = WindowHeight,
                CategoryCheckBoxSize = CategoryCheckBoxSize,
                CategoryFontSize = CategoryFontSize,
                SidePanelWidth = SidePanelWidth,
                IconSize = IconSize,
                ShowTooltips = ShowTooltips,
                RejectedChampions = RejectedChampions,
                Categories = Categories.Select(c => new ChampionCategory { CategoryName = c.CategoryName, Champions = c.Champions, IsSelected = c.IsSelected }).ToList()
            };
        }

        private void ApplyConfiguration(GuessWhoConfig config) {
            WindowWidth = config.WindowWidth;
            WindowHeight = config.WindowHeight;
            CategoryCheckBoxSize = config.CategoryCheckBoxSize;
            CategoryFontSize = config.CategoryFontSize;
            SidePanelWidth = config.SidePanelWidth;
            IconSize = config.IconSize;
            ShowTooltips = config.ShowTooltips;
            RejectedChampions = config.RejectedChampions;
            RaisePropertyChanged(nameof(AnyChampionsRejected));
            foreach (ChampionCategoryViewModel vm in Categories) {
                vm.PropertyChanged -= ChampionCategory_PropertyChanged;
            }

            Categories.Clear();
            foreach (ChampionCategory category in config.Categories) {
                ChampionCategoryViewModel vm = new ChampionCategoryViewModel(category);
                vm.PropertyChanged += ChampionCategory_PropertyChanged;
                Categories.Add(vm);
            }

            AllChampionDescriptions.Clear();
            foreach (Champion champ in ChampionProvider.AllChampionsAlphabetical) {
                AllChampionDescriptions.Add(champ,
                    new ChampionDescription(champ,
                        string.Join("\n",
                            Categories.Where(c => c.Champions.Contains(champ)).
                                    Select(c => c.CategoryName))));
            }

            RevalidateChampions();
        }

        private void ChampionCategory_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case nameof(ChampionCategoryViewModel.IsSelected):
                    RevalidateChampions();
                    break;
            }
        }

        private void RevalidateChampions() {
            Champions.Clear();
            foreach (Champion champ in ChampionProvider.AllChampionsAlphabetical) {
                if (!RejectedChampions.Contains(champ) &&
                    !Categories.Any(c => c.Champions.Contains(champ) && !c.IsSelected)) {
                    Champions.Add(AllChampionDescriptions[champ]);
                }
            }
        }

        private void ShowYesNoDialog(string message, Action yesAction = null, Action noAction = null) {
            DialogTwoButtonViewModel.OpenDialog(DialogIdentifier1, message, yesAction, noAction);
        }

        private void ShowInformationDialog(string message) {
            DialogOneButtonViewModel.OpenDialog(DialogIdentifier2, message);
        }

        #endregion

    }
}