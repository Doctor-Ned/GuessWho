using System.Collections.Generic;

using GuessWho.Model;

using NedMaterialMVVM;

namespace GuessWho.ViewModel {
    public class ChampionCategoryViewModel : PropertyChangedWrapper {
        private bool _IsSelected;

        public ChampionCategoryViewModel(ChampionCategory category) {
            IsSelected = category.IsSelected;
            CategoryName = category.CategoryName;
            Champions = category.Champions;
        }

        public bool IsSelected {
            get { return _IsSelected; }
            set {
                _IsSelected = value;
                RaisePropertyChanged();
            }
        }

        public string CategoryName { get; }

        public HashSet<Champion> Champions { get; }
    }
}