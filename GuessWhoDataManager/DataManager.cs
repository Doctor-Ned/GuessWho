using System;
using System.IO;
using System.Reflection;

namespace GuessWhoDataManager {
    internal class DataManager {
        internal const string DATA_MANAGER_PROJECT_NAME = "GuessWhoDataManager";
        internal const string RESOURCE_PROJECT_NAME = "GuessWhoResources";

        internal static string DefaultDataManagerProjectPath {
            get { return Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName ?? throw new InvalidOperationException(), "..", ".."); }
        }

        internal static string DefaultSolutionPath {
            get { return Path.Combine(DefaultDataManagerProjectPath, ".."); }
        }

        public DataManager() : this(DefaultSolutionPath) { }

        public DataManager(string solutionPath) : this(Path.Combine(solutionPath, DATA_MANAGER_PROJECT_NAME), Path.Combine(solutionPath, RESOURCE_PROJECT_NAME)) { }

        public DataManager(string dataManagerProjectPath, string resourcesProjectPath) {
            DataManagerProjectPath = dataManagerProjectPath;
            ResourcesProjectPath = resourcesProjectPath;
        }

        private string DataManagerProjectPath { get; }

        private string ResourcesProjectPath { get; }
    }
}
