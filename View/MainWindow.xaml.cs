using System.Windows;

namespace GuessWho.View {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void CloseWindowButton_OnClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void MaximizeWindowButton_OnClick(object sender, RoutedEventArgs e) {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeWindowButton_OnClick(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }
    }
}