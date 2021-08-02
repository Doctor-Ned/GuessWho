using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using GuessWho.Model;

namespace GuessWho.View {
    public partial class ChampionControl : UserControl {
        public ChampionControl() {
            InitializeComponent();
        }

        public static readonly DependencyProperty ChampionProperty = DependencyProperty.Register
        (nameof(Champion), typeof(Champion?), typeof(ChampionControl));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register
        (nameof(Command), typeof(ICommand), typeof(ChampionControl));

        public Champion? Champion {
            get { return (Champion?)GetValue(ChampionProperty); }
            set { SetValue(ChampionProperty, value); }
        }

        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
    }
}
