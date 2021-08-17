using GuessWhoResources;

namespace GuessWho.ViewModel {
    public class BasicCategoryViewModel : CategoryViewModel {
        public BasicCategoryViewModel(BasicCategory category) {
            Category = category;
            BasicCategory = category;
        }

        public BasicCategory BasicCategory { get; }
    }
}