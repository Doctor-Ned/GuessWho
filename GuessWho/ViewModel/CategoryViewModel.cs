using NedMaterialMVVM;

namespace GuessWho.ViewModel {
    public abstract class CategoryViewModel : PropertyChangedWrapper {
        private bool _IsSelected;
        public bool IsSelected {
            get { return _IsSelected; }
            set {
                _IsSelected = value;
                RaisePropertyChanged();
            }
        }

        public object Category { get; protected set; }
    }
}