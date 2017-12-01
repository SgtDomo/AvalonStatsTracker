using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Avalon_Stats
{
    /// <summary>
    /// Interaction logic for PlayerRoleChooser.xaml
    /// </summary>
    public partial class PlayerRoleChooser
    {
        public static readonly DependencyProperty SelectedPlayerProperty =
            DependencyProperty.Register("SelectedPlayer", typeof(Player), typeof(PlayerRoleChooser));

        [Bindable(true)]
        public Player SelectedPlayer
        {
            get => GetValue(SelectedPlayerProperty) as Player;
            set => SetValue(SelectedPlayerProperty, value);
        }


        public static readonly DependencyProperty PlayersProperty =
            DependencyProperty.Register("Players", typeof(ObservableCollection<Player>), typeof(PlayerRoleChooser));

        [Bindable(true)]
        public ObservableCollection<Player> Players
        {
            get => GetValue(PlayersProperty) as ObservableCollection<Player>;
            set => SetValue(PlayersProperty, value);
        }

        public static readonly DependencyProperty SelectedRoleProperty =
            DependencyProperty.Register("SelectedRole", typeof(GameRole), typeof(PlayerRoleChooser));

        [Bindable(true)]
        public GameRole SelectedRole
        {
            get => GetValue(SelectedRoleProperty) as GameRole;
            set => SetValue(SelectedRoleProperty, value);
        }

        public static readonly DependencyProperty RolesProperty =
            DependencyProperty.Register("Roles", typeof(ObservableCollection<GameRole>), typeof(PlayerRoleChooser));

        [Bindable(true)]
        public ObservableCollection<GameRole> Roles
        {
            get => GetValue(RolesProperty) as ObservableCollection<GameRole>;
            set => SetValue(RolesProperty, value);
        }

        public PlayerRoleChooser()
        {
            InitializeComponent();
        }
    }
}
