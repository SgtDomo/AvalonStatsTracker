using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Avalon_Stats.Annotations;

namespace Avalon_Stats
{
    /// <summary>
    /// Interaction logic for GameRegistrationView.xaml
    /// </summary>
    public partial class GameRegistrationView : INotifyPropertyChanged
    {
        private Player[] _allPlayers;

        public Player[] AllPlayers
        {
            get => _allPlayers;
            set
            {
                _allPlayers = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Player> _availablePlayers;

        public ObservableCollection<Player> AvailablePlayers
        {
            get => _availablePlayers;
            set
            {
                _availablePlayers = value;
                OnPropertyChanged();
            }
        }

        private Player _selectedKilledPlayer;

        public Player SelectedKilledPlayer
        {
            get => _selectedKilledPlayer;
            set
            {
                _selectedKilledPlayer = value;
                OnPropertyChanged();
            }
        }

        private GameRole[] _allRoles;

        public GameRole[] AllRoles
        {
            get => _allRoles;
            set
            {
                _allRoles = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<GameRole> _availableRoles;

        public ObservableCollection<GameRole> AvailableRoles
        {
            get => _availableRoles;
            set
            {
                _availableRoles = value;
                OnPropertyChanged();
            }
        }

        private DateTime _endDateEndDateTime;

        public DateTime EndDateTime
        {
            get => _endDateEndDateTime;
            set
            {
                _endDateEndDateTime = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PlayerRole> _playerRoles;

        public ObservableCollection<PlayerRole> PlayerRoles
        {
            get => _playerRoles;
            set
            {
                _playerRoles = value;
                OnPropertyChanged();
            }
        }

        private Brush _missionResultErrorColor;

        public Brush MissionResultErrorColor
        {
            get => _missionResultErrorColor;
            set
            {
                _missionResultErrorColor = value;
                OnPropertyChanged();
            }
        }

        private int _playerCount;

        public int PlayerCount
        {
            get => _playerCount;
            set
            {
                _playerCount = value;
                OnPropertyChanged();
                UpdatePlayerRolesCount(value);
            }
        }

        private readonly AvalonDBDataContext _dbcontext = AvalonDbDataContextProvider.DbContextInstance;

        private readonly Dictionary<Button, MissionResult> _missionBtns = new Dictionary<Button, MissionResult>();

        private readonly Dictionary<MissionResult, Brush> _missionResultColors = new Dictionary<MissionResult, Brush>
        {
            {MissionResult.NotPlayed, Brushes.Gray},
            {MissionResult.Good, Brushes.LightBlue},
            {MissionResult.Evil, Brushes.Coral}
        };

        private readonly List<MissionResult> _missionResults = new List<MissionResult>
            {MissionResult.Good, MissionResult.Evil, MissionResult.NotPlayed};

        public GameRegistrationView()
        {
            InitializeComponent();
            DataContext = this;
            _missionBtns.Add(Mission1Btn, MissionResult.Good);
            _missionBtns.Add(Mission2Btn, MissionResult.Good);
            _missionBtns.Add(Mission3Btn, MissionResult.Good);
            _missionBtns.Add(Mission4Btn, MissionResult.NotPlayed);
            _missionBtns.Add(Mission5Btn, MissionResult.NotPlayed);
            ResetEverything();
        }
        private void MissionBtnClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (int.TryParse((sender as Button)?.Content.ToString(), out int missionId))
            {
                if (missionId <= 3)
                {
                    if (_missionBtns[b] == MissionResult.Good)
                    {
                        _missionBtns[b] = MissionResult.Evil;
                    }
                    else
                    {
                        _missionBtns[b] = MissionResult.Good;
                    }
                }
                else
                {
                    int nextResult = _missionResults.IndexOf(_missionBtns[b]) + 1;
                    if (nextResult == _missionResults.Count)
                    {
                        nextResult = 0;
                    }
                    _missionBtns[b] = _missionResults[nextResult];
                }
                UpdateMissionButtonColor(b);
            }
            UpdateKilledPlayerEnabled();
            if (!AreMissionResultsValid())
            {
                MissionResultErrorColor = Brushes.Red;
            }
            else
            {
                MissionResultErrorColor = Background;
            }
        }

        private void UpdateKilledPlayerEnabled()
        {
            if (HaveGoodWon())
            {
                KilledPlayerCb.IsEnabled = true;
            }
            else
            {
                KilledPlayerCb.IsEnabled = false;
                SelectedKilledPlayer = null;
            }
        }

        private void UpdatePlayerRolesCount(int count)
        {
            int currCount = PlayerRoles.Count;
            if (currCount < count)
            {
                for (int i = 0; i < (count - currCount); i++)
                {
                    PlayerRoles.Add(new PlayerRole());
                }
            }
            else
            {
                for (int i = currCount - 1; i >= count; i--)
                {
                    PlayerRoles.RemoveAt(i);
                }
            }
        }

        private int GetEvilPoints()
        {
            return _missionBtns.Count(x => x.Value == MissionResult.Evil);
        }

        private int GetGoodPoints()
        {
            return _missionBtns.Count(x => x.Value == MissionResult.Good);
        }

        private bool HaveEvilWon()
        {
            return GetEvilPoints() == 3;
        }

        private bool HaveGoodWon()
        {
            return GetGoodPoints() == 3;
        }

        private bool AreMissionResultsValid()
        {
            return HaveEvilWon() || HaveGoodWon();
        }

        private void UpdateMissionButtonColor([NotNull] Button b)
        {
            b.Background = _missionResultColors[_missionBtns[b]];
            b.BorderBrush = _missionResultColors[_missionBtns[b]];
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowErrorMsg(string msg, string title)
        {
            MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowErrorMsg(string msg)
        {
            ShowErrorMsg(msg, "Fehler");
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!AreMissionResultsValid())
            {
                ShowErrorMsg("Die Missionen-Ergebnisse müssen gültig sein!");
                return;
            }
            if (HaveGoodWon() && SelectedKilledPlayer == null)
            {
                ShowErrorMsg("Es ist kein getöter Spieler ausgewählt. Wenn die guten gewonnen haben, muss ein Spieler getöten werden!");
                return;
            }
            if (HaveGoodWon() && !PlayerRoles.Select(x => x.Player).Contains(SelectedKilledPlayer))
            {
                ShowErrorMsg("Der getöteter Spieler muss auch mitspielen!");
                return;
            }
            if (PlayerRoles.Count(x => x.Role == null) > 0 || PlayerRoles.Count(x => x.Player == null) > 0)
            {
                ShowErrorMsg("Die Spieler und Rollen sind noch nicht fertig ausgefüllt!");
                return;
            }
            foreach (Player player in PlayerRoles.Select(x => x.Player))
            {
                if (PlayerRoles.Count(x => x.Player == player) > 1)
                {
                    ShowErrorMsg($"Spieler {player.PlayerName} ist mehrfach eingetragen. Ein Spieler kann nur einmal mitspielen!");
                    return;
                }
            }
            foreach (GameRole role in PlayerRoles.Select(x => x.Role))
            {
                if (!UnuniqueRoleProvider.UnuniqueRoleShortcuts.Contains(role.RoleShortcut) &&
                    PlayerRoles.Count(x => x.Role == role) > 1)
                {
                    ShowErrorMsg($"Die Rolle {role.FullName} ist mehrmals ausgewählt worden. Diese darf aber nur einmal vergeben werden!");
                    return;
                }
            }
            MissionResult[] results =
                _missionBtns.OrderBy(x => x.Key.Content.ToString()).Select(x => x.Value).ToArray();
            Game newGame = new Game
            {
                GameTime = EndDateTime,
                FirstMissionResult = ConvertMissionResultToBool(results[0]).Value,
                SecondMissionResult = ConvertMissionResultToBool(results[1]).Value,
                ThirdMissionResult = ConvertMissionResultToBool(results[2]).Value,
                FourthMissionResult = ConvertMissionResultToBool(results[3]),
                FithMissionResult = ConvertMissionResultToBool(results[4]),
                Player = SelectedKilledPlayer
            };
            foreach (PlayerRole pr in PlayerRoles)
            {
                newGame.Participations.Add(new Participation
                {
                    Game = newGame,
                    Player = pr.Player,
                    GameRole = pr.Role
                });
            }
            _dbcontext.Games.InsertOnSubmit(newGame);
            _dbcontext.SubmitChanges();
            QueryRestrictions.RefreshDateTimeRestricitons();
            MessageBox.Show("Spiel erfolgreich registriert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            ResetEverything();
        }

        private void ResetEverything()
        {

            AllPlayers = _dbcontext.Players.OrderBy(x => x.PlayerName).ToArray();
            AllRoles = _dbcontext.GameRoles.OrderByDescending(x => x.Side).ThenBy(x => x.FullName).ToArray();
            AvailablePlayers = new ObservableCollection<Player>(AllPlayers);
            AvailableRoles = new ObservableCollection<GameRole>(AllRoles);
            PlayerRoles = new ObservableCollection<PlayerRole>();

            _missionBtns[Mission1Btn] = MissionResult.Good;
            _missionBtns[Mission2Btn] = MissionResult.Good;
            _missionBtns[Mission3Btn] = MissionResult.Good;
            _missionBtns[Mission4Btn] = MissionResult.NotPlayed;
            _missionBtns[Mission5Btn] = MissionResult.NotPlayed;
            foreach (Button b in _missionBtns.Keys)
            {
                UpdateMissionButtonColor(b);
            }
            MissionResultErrorColor = Background;

            EndDateTime = DateTime.Now;

            SelectedKilledPlayer = null;
            UpdateKilledPlayerEnabled();

            PlayerCount = 5;

            foreach (PlayerRole pr in PlayerRoles)
            {
                pr.Player = null;
                pr.Role = null;
            }
            PlayerRoles[0].Player = AllPlayers.Where(x => x.PlayerName == "Baron").ToArray()[0];
            PlayerRoles[1].Player = AllPlayers.Where(x => x.PlayerName == "Fogl").ToArray()[0];
            PlayerRoles[2].Player = AllPlayers.Where(x => x.PlayerName == "Hadujacz").ToArray()[0];
            PlayerRoles[3].Player = AllPlayers.Where(x => x.PlayerName == "Haiden").ToArray()[0];
            PlayerRoles[4].Player = AllPlayers.Where(x => x.PlayerName == "Ungersböck").ToArray()[0];
        }

        private bool? ConvertMissionResultToBool(MissionResult mr)
        {
            switch (mr)
            {
                case MissionResult.Evil:
                    return false;
                case MissionResult.Good:
                    return true;
                case MissionResult.NotPlayed:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException($"No MissionResult {mr} expected");
            }
        }
    }
}
