using System;
using System.Linq;
using System.Xml;

namespace GuessWhoDataManager {
    internal class Program {
        static void Main(string[] args) {
            // todo: add args etc., I guess
            DataManager dataManager = new DataManager();
            //dataManager.ReworkResources(true);

            XmlDocument doc = new XmlDocument();
            doc.Load("D:\\VisualStudioRepos\\GuessWho\\GuessWhoResources\\GuessWhoResources.csproj");
            XmlNodeList resources = doc.GetElementsByTagName("Resource");
            foreach (XmlNode node in resources.Cast<XmlNode>()) {
                var af = node.Attributes["Include"].Value;
            }
            Console.ReadKey();
        }
    }
}
