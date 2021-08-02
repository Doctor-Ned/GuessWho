using System.Windows;

using GuessWho.Model;

namespace GuessWho.View {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            ChampionProvider.Validate();
        }
    }
}
