using GuessWhoResources;

namespace GuessWho.ViewModel {
    public class CustomCategoryViewModel : CategoryViewModel {
        public CustomCategoryViewModel(CustomCategory category) {
            Category = category;
            CustomCategory = category;
        }

        public CustomCategory CustomCategory { get; set; }
    }
}