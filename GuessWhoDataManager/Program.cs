﻿using System;

namespace GuessWhoDataManager {
    internal class Program {
        static void Main() {
            // todo: add args etc., I guess
            DataManager dataManager = new DataManager();
            dataManager.ReworkResources(true);
            Console.WriteLine("Done! (Press any button to continue)");
            Console.ReadKey();
        }
    }
}
