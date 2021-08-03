using System.Collections.Generic;

namespace GuessWho.Model {
    public class ChampionCategory {
        public string CategoryName { get; set; }
        public bool IsSelected { get; set; }
        public HashSet<Champion> Champions { get; set; }
    }
}
