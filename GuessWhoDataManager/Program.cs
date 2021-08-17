using System;

namespace GuessWhoDataManager {
    internal class Program {
        static void Main(string[] args) {
            DataManager dataManager = new DataManager();
            dataManager.ReworkResources(true);
            Console.ReadKey();
        }
    }
}
