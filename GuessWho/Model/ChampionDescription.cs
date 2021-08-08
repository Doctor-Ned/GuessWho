namespace GuessWho.Model {
    public class ChampionDescription {
        public ChampionDescription(Champion champion, string description) {
            Champion = champion;
            Description = description;
        }
        public Champion Champion { get; }

        public string Description { get; }
    }
}
