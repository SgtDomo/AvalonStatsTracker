using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Avalon_Stats.Annotations;

namespace Avalon_Stats
{
    /// <summary>
    /// Interaction logic for PlayerStatsListView.xaml
    /// </summary>
    public partial class PlayerStatsListView : INotifyPropertyChanged
    {
        public string[] PlayerNames => DbContext.Players.Select(x => x.PlayerName).OrderBy(x => x).ToArray();

        private string _playerNameInput;

        public string PlayerNameInput
        {
            get => _playerNameInput;
            set
            {
                _playerNameInput = value;
                OnPropertyChanged();
                FilterPlayerForPlayerName(null, null);
            }
        }

        private ObservableCollection<Player> _selectedPlayers;
        public ObservableCollection<Player> SelectedPlayers
        {
            get => _selectedPlayers;
            set
            {
                _selectedPlayers = value;
                OnPropertyChanged();
            }
        }

        public List<Player> AllPlayers { get; } = DbContext.Players.ToList();

        private static readonly AvalonDBDataContext DbContext = AvalonDbDataContextProvider.DbContextInstance;

        public PlayerStatsListView()
        {
            SelectedPlayers = new ObservableCollection<Player>(DbContext.Players.ToList());
            InitializeComponent();
            DataContext = this;
            PlayerNameInput = "";
        }

        private void FilterPlayerForPlayerName(object sender, RoutedEventArgs e)
        {
            SelectedPlayers.Clear();
            foreach (Player player in DbContext.Players)
            {
                if ((player.FirstName + " " + player.PlayerName).ToLower().Contains(PlayerNameInput.ToLower()))
                {
                    SelectedPlayers.Add(player);
                }
            }
        }


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
