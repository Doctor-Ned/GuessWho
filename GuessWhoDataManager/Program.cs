using System;

namespace GuessWhoDataManager {
    internal class Program {
        static void Main(string[] args) {
            // todo: add args etc., I guess
            DataManager dataManager = new DataManager();
            dataManager.ReworkResources(true);
            Console.ReadKey();
        }
    }
}
