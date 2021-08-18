using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

using GuessWho.Model;

using GuessWhoResources;

using NedMaterialMVVM;
using NedMaterialMVVM.ViewModel;

using WPFLocalizeExtension.Engine;

namespace GuessWho.ViewModel {
    public class MainViewModel : PropertyChangedWrapper {

        #region Backing fields

        private double _CategoryCheckBoxSize;
        private double _CategoryFontSize;
        private double _IconSize;
        private bool _ShowSettings;
        private bool _ShowTooltips;
        private double _SidePanelWidth;
        private double _WindowHeight;
        private double _WindowWidth;
        private Locale _Locale;

        #endregion

        public MainViewModel() {
            OnLoaded = new RelayCommand(ExecuteOnLoaded);
            OnClosing = new RelayCommand(ExecuteOnClosing);
            ToggleSettings = new RelayCommand(ExecuteToggleSettings);
            RestoreChampions = new RelayCommand(ExecuteRestoreChampions);
            RejectChampion = new RelayCommand<string>(ExecuteRejectChampion);
            ResetGame = new RelayCommand(ExecuteResetGame);
            LoadConfig = new RelayCommand(ExecuteLoadConfig);
            SaveConfig = new RelayCommand(ExecuteSaveConfig);
            ResetConfig = new RelayCommand(ExecuteResetConfig);
            ChangeLanguage = new RelayCommand(ExecuteChangeLanguage);
            ConfigManager = new GuessWhoConfigManager();
            DialogRejectedChampionsViewModel = new DialogRejectedChampionsViewModel(this);
            RejectedChampions.CollectionChanged += RejectedChampions_CollectionChanged;
            RejectedBasicCategories.CollectionChanged += RejectedBasicCategories_CollectionChanged;
            RejectedCustomCategories.CollectionChanged += RejectedCustomCategories_CollectionChanged;
        }

        private void RejectedChampions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Remove:
                    foreach (string champId in e.OldItems) {
                        AddChampionIfValid(champId);
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (string champId in e.NewItems) {
                        Champions.Add(champId);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (string champId in e.OldItems) {
                        AddChampionIfValid(champId);
                    }
                    foreach (string champId in e.NewItems) {
                        Champions.Add(champId);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems != null) {
                        foreach (string champId in e.OldItems) {
                            AddChampionIfValid(champId);
                        }
                    }
                    break;
            }
            RaisePropertyChanged(nameof(AnyChampionsRejected));
        }

        private void RejectedBasicCategories_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Remove:
                    foreach (BasicCategory cat in e.OldItems) {
                        foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                            AddChampionIfValid(champId);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (BasicCategory cat in e.NewItems) {
                        foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                            Champions.Remove(champId);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (BasicCategory cat in e.OldItems) {
                        foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                            AddChampionIfValid(champId);
                        }
                    }
                    foreach (BasicCategory cat in e.NewItems) {
                        foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                            Champions.Remove(champId);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems != null) {
                        foreach (BasicCategory cat in e.OldItems) {
                            foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                                AddChampionIfValid(champId);
                            }
                        }
                    }
                    break;
            }
        }

        private void RejectedCustomCategories_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Remove:
                    foreach (CustomCategory cat in e.OldItems) {
                        foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                            AddChampionIfValid(champId);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (CustomCategory cat in e.NewItems) {
                        foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                            Champions.Remove(champId);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (CustomCategory cat in e.OldItems) {
                        foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                            AddChampionIfValid(champId);
                        }
                    }

                    foreach (CustomCategory cat in e.NewItems) {
                        foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                            Champions.Remove(champId);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems != null) {
                        foreach (CustomCategory cat in e.OldItems) {
                            foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                                AddChampionIfValid(champId);
                            }
                        }
                    }
                    break;
            }
        }

        #region Public properties

        public ObservableCollection<CategoryViewModel> Categories { get; } =
            new ObservableCollection<CategoryViewModel>();

        public Locale Locale {
            get { return _Locale; }
            set {
                _Locale = value;
                LocalizeDictionary.Instance.Culture = _Locale.ToCultureInfo();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged("");
                RaisePropertyChanged();
            }
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
            get { return RejectedChampions.Any(); }
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
        public ICommand ChangeLanguage { get; }

        #endregion

        #region Private properties

        public ObservableCollection<string> Champions { get; } = new ObservableCollection<string>();

        public ObservableCollection<string> RejectedChampions { get; } = new ObservableCollection<string>();

        public ObservableCollection<BasicCategory> RejectedBasicCategories { get; } = new ObservableCollection<BasicCategory>();

        public ObservableCollection<CustomCategory> RejectedCustomCategories { get; } = new ObservableCollection<CustomCategory>();

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

        #region Command actions

        private void ExecuteRestoreChampions() {
            DialogRejectedChampionsViewModel.OpenIDialog(DialogIdentifier1);
        }

        private void ExecuteRejectChampion(string champId) {
            RejectedChampions.Add(champId);
            Champions.Remove(champId);
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
                config.RejectedBasicCategories.Clear();
                config.RejectedCustomCategories.Clear();
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

        private void ExecuteChangeLanguage() {
            Locale = Locale == Locale.en_US ? Locale.pl_PL : Locale.en_US;
        }

        #endregion

        #region Private methods

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
                Locale = Locale,
                RejectedChampions = RejectedChampions.ToList(),
                RejectedBasicCategories = RejectedBasicCategories.ToList(),
                RejectedCustomCategories = RejectedCustomCategories.ToList()
            };
        }

        private void ApplyConfiguration(GuessWhoConfig config) {
            Locale = config.Locale;
            WindowWidth = config.WindowWidth;
            WindowHeight = config.WindowHeight;
            CategoryCheckBoxSize = config.CategoryCheckBoxSize;
            CategoryFontSize = config.CategoryFontSize;
            SidePanelWidth = config.SidePanelWidth;
            IconSize = config.IconSize;
            ShowTooltips = config.ShowTooltips;
            RejectedChampions.Clear();
            foreach (string champId in config.RejectedChampions) {
                RejectedChampions.Add(champId);
            }
            RejectedBasicCategories.Clear();
            foreach (BasicCategory cat in config.RejectedBasicCategories) {
                RejectedBasicCategories.Add(cat);
            }
            RejectedCustomCategories.Clear();

            foreach (CustomCategory cat in config.RejectedCustomCategories) {
                RejectedCustomCategories.Add(cat);
            }
            foreach (CategoryViewModel vm in Categories) {
                vm.PropertyChanged -= ChampionCategory_PropertyChanged;
            }

            Categories.Clear();
            foreach (CustomCategory category in Enum.GetValues(typeof(CustomCategory))) {
                CustomCategoryViewModel vm = new CustomCategoryViewModel(category) {
                    IsSelected = !RejectedCustomCategories.Contains(category)
                };
                vm.PropertyChanged += ChampionCategory_PropertyChanged;
                Categories.Add(vm);
            }
            foreach (BasicCategory category in Enum.GetValues(typeof(BasicCategory))) {
                BasicCategoryViewModel vm = new BasicCategoryViewModel(category) {
                    IsSelected = !RejectedBasicCategories.Contains(category)
                };
                vm.PropertyChanged += ChampionCategory_PropertyChanged;
                Categories.Add(vm);
            }
        }

        private void ChampionCategory_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            switch (e.PropertyName) {
                case nameof(CategoryViewModel.IsSelected):
                    if (sender is BasicCategoryViewModel bcVm) {
                        if (bcVm.IsSelected) {
                            RejectedBasicCategories.Remove(bcVm.BasicCategory);
                        } else {
                            RejectedBasicCategories.Add(bcVm.BasicCategory);
                        }
                    } else if (sender is CustomCategoryViewModel ccVm) {
                        if (ccVm.IsSelected) {
                            RejectedCustomCategories.Remove(ccVm.CustomCategory);
                        } else {
                            RejectedCustomCategories.Add(ccVm.CustomCategory);
                        }
                    } else {
                        throw new InvalidOperationException($"{nameof(ChampionCategory_PropertyChanged)} sender type is invalid!");
                    }
                    break;
            }
        }

        private void RevalidateChampions() {
            Champions.Clear();
            foreach (string champId in ChampionProvider.AllChampionIds) {
                AddChampionIfValid(champId);
            }
        }

        private void AddChampionIfValid(string champId) {
            if (!RejectedChampions.Contains(champId) &&
                !ChampionProvider.GetBasicCategories(champId).Any(c => RejectedBasicCategories.Contains(c)) &&
                !ChampionProvider.GetCustomCategories(champId).Any(c => RejectedCustomCategories.Contains(c))) {
                Champions.Add(champId);
            }
        }

        private void ShowYesNoDialog(string message, Action yesAction = null, Action noAction = null) {
            DialogTwoButtonViewModel.Message = message;
            DialogTwoButtonViewModel.Button1Action = yesAction;
            DialogTwoButtonViewModel.Button2Action = noAction;
            DialogTwoButtonViewModel.OpenIDialog(DialogIdentifier1);
        }

        private void ShowInformationDialog(string message) {
            DialogOneButtonViewModel.OpenIDialog(DialogIdentifier2, message);
        }

        #endregion

    }
}