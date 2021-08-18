using System;
using System.Collections.Generic;
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
            DialogLanguageSelectionViewModel = new DialogLanguageSelectionViewModel(this);
            RejectedChampions.CollectionChanged += RejectedChampions_CollectionChanged;
            RejectedBasicCategories.CollectionChanged += RejectedBasicCategories_CollectionChanged;
            RejectedCustomCategories.CollectionChanged += RejectedCustomCategories_CollectionChanged;
        }

        private void RejectedChampions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Remove: {
                        bool changed = false;
                        foreach (string champId in e.OldItems) {
                            changed |= AddChampionIfValid(champId);
                        }
                        if (changed) {
                            RefreshVisibleChampions();
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Add:
                    foreach (string champId in e.NewItems) {
                        RemoveChampion(champId);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace: {
                        bool changed = false;
                        foreach (string champId in e.OldItems) {
                            changed |= AddChampionIfValid(champId);
                        }
                        if (changed) {
                            RefreshVisibleChampions();
                        }
                        foreach (string champId in e.NewItems) {
                            RemoveChampion(champId);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Reset: {
                        RevalidateChampions();
                        break;
                    }
            }
            RaisePropertyChanged(nameof(AnyChampionsRejected));
        }

        private void RejectedBasicCategories_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Remove: {
                        bool changed = false;
                        foreach (BasicCategory cat in e.OldItems) {
                            foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                                changed |= AddChampionIfValid(champId);
                            }
                        }
                        if (changed) {
                            RefreshVisibleChampions();
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Add:
                    foreach (BasicCategory cat in e.NewItems) {
                        foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                            RemoveChampion(champId);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace: {
                        bool changed = false;
                        foreach (BasicCategory cat in e.OldItems) {
                            foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                                changed |= AddChampionIfValid(champId);
                            }
                        }
                        if (changed) {
                            RefreshVisibleChampions();
                        }
                        foreach (BasicCategory cat in e.NewItems) {
                            foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                                RemoveChampion(champId);
                            }
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Reset: {
                        RevalidateChampions();
                        break;
                    }
            }
        }

        private void RejectedCustomCategories_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            switch (e.Action) {
                case NotifyCollectionChangedAction.Remove: {
                        bool changed = false;
                        foreach (CustomCategory cat in e.OldItems) {
                            foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                                changed |= AddChampionIfValid(champId);
                            }
                        }
                        if (changed) {
                            RefreshVisibleChampions();
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Add:
                    foreach (CustomCategory cat in e.NewItems) {
                        foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                            RemoveChampion(champId);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace: {
                        bool changed = false;
                        foreach (CustomCategory cat in e.OldItems) {
                            foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                                changed |= AddChampionIfValid(champId);
                            }
                        }
                        if (changed) {
                            RefreshVisibleChampions();
                        }
                        foreach (CustomCategory cat in e.NewItems) {
                            foreach (string champId in ChampionProvider.GetChampIds(cat)) {
                                RemoveChampion(champId);
                            }
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Reset: {
                        RevalidateChampions();
                        break;
                    }
            }
        }

        #region Public properties

        public ObservableCollection<string> VisibleChampions { get; } = new ObservableCollection<string>();

        public ObservableCollection<string> RejectedChampions { get; } = new ObservableCollection<string>();

        public ObservableCollection<BasicCategory> RejectedBasicCategories { get; } = new ObservableCollection<BasicCategory>();

        public ObservableCollection<CustomCategory> RejectedCustomCategories { get; } = new ObservableCollection<CustomCategory>();

        public ObservableCollection<CategoryViewModel> Categories { get; } =
            new ObservableCollection<CategoryViewModel>();

        public Locale Locale {
            get { return _Locale; }
            set {
                _Locale = value;
                LocalizeDictionary.Instance.Culture = _Locale.ToCultureInfo();
                RefreshVisibleChampions();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged("");
                foreach (CategoryViewModel cvm in Categories) {
                    cvm.RefreshBindings();
                }
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

        private HashSet<string> Champions { get; } = new HashSet<string>();

        private DialogOneButtonViewModel DialogInformationViewModel { get; } = new DialogOneButtonViewModel();

        private DialogTwoButtonViewModel DialogYesNoViewModel { get; } = new DialogTwoButtonViewModel();

        private DialogRejectedChampionsViewModel DialogRejectedChampionsViewModel { get; }

        private DialogLanguageSelectionViewModel DialogLanguageSelectionViewModel { get; }

        private GuessWhoConfigManager ConfigManager { get; }

        #endregion

        #region Command actions

        private void ExecuteRestoreChampions() {
            DialogRejectedChampionsViewModel.OpenIDialog(DialogIdentifier1);
        }

        private void ExecuteRejectChampion(string champId) {
            RejectedChampions.Add(champId);
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
            ShowYesNoDialog(ResourceProvider.GetLocaleString("ResetGamePrompt"), () =>
            {
                GuessWhoConfig config = GetCurrentConfig();
                config.RejectedChampions.Clear();
                config.RejectedBasicCategories.Clear();
                config.RejectedCustomCategories.Clear();
                ApplyConfiguration(config);
                try {
                    WriteConfiguration();
                } catch (Exception e) {
                    ShowInformationDialog(string.Format(ResourceProvider.GetLocaleString("ResetGameDoneWithExceptionFormat"), e));
                }
            });
        }

        private void ExecuteLoadConfig() {
            ShowYesNoDialog(ResourceProvider.GetLocaleString("LoadConfigPrompt"), () =>
                {
                    try {
                        LoadConfiguration();
                    } catch (Exception e) {
                        ShowInformationDialog(string.Format(ResourceProvider.GetLocaleString("LoadConfigFailedFormat"), e));
                    }
                });
        }

        private void ExecuteSaveConfig() {
            try {
                WriteConfiguration();
                ShowInformationDialog(ResourceProvider.GetLocaleString("SaveConfigSuccessful"));
            } catch (Exception e) {
                ShowInformationDialog(string.Format(ResourceProvider.GetLocaleString("SaveConfigFailedFormat"), e));
            }
        }

        private void ExecuteResetConfig() {
            ShowYesNoDialog(ResourceProvider.GetLocaleString("ResetConfigPrompt"), () =>
            {
                ApplyConfiguration(GuessWhoConfig.GetDefaultConfig());
                try {
                    WriteConfiguration();
                } catch (Exception e) {
                    ShowInformationDialog(string.Format(ResourceProvider.GetLocaleString("ResetConfigDoneWithExceptionFormat"), e));
                }
            });
        }

        private void ExecuteChangeLanguage() {
            DialogLanguageSelectionViewModel.OpenIDialog(DialogIdentifier1);
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
            RevalidateChampions();
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

        private void RemoveChampion(string champId) {
            Champions.Remove(champId);
            VisibleChampions.Remove(champId);
        }

        private void RefreshVisibleChampions() {
            VisibleChampions.Clear();
            foreach (string champ in Champions.OrderBy(
                ResourceProvider.GetLocalizedChampionName, StringComparer.CurrentCultureIgnoreCase)) {
                VisibleChampions.Add(champ);
            }
        }

        private void RevalidateChampions() {
            Champions.Clear();
            foreach (string champId in ChampionProvider.AllChampionIds) {
                AddChampionIfValid(champId);
            }
            RefreshVisibleChampions();
        }

        private bool AddChampionIfValid(string champId) {
            if (!RejectedChampions.Contains(champId) &&
                !ChampionProvider.GetBasicCategories(champId).Any(c => RejectedBasicCategories.Contains(c)) &&
                !ChampionProvider.GetCustomCategories(champId).Any(c => RejectedCustomCategories.Contains(c))) {
                Champions.Add(champId);
                return true;
            }
            return false;
        }

        private void ShowYesNoDialog(string message, Action yesAction = null, Action noAction = null) {
            DialogYesNoViewModel.Button1Text = ResourceProvider.GetLocaleString("YesCapital");
            DialogYesNoViewModel.Button2Text = ResourceProvider.GetLocaleString("NoCapital");
            DialogYesNoViewModel.Message = message;
            DialogYesNoViewModel.Button1Action = yesAction;
            DialogYesNoViewModel.Button2Action = noAction;
            DialogYesNoViewModel.OpenIDialog(DialogIdentifier1);
        }

        private void ShowInformationDialog(string message) {
            DialogInformationViewModel.ButtonText = ResourceProvider.GetLocaleString("OK");
            DialogInformationViewModel.OpenIDialog(DialogIdentifier2, message);
        }

        #endregion

    }
}