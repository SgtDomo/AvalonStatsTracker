using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using Avalon_Stats.Annotations;

namespace Avalon_Stats
{
    /// <summary>
    /// Interaction logic for PlayerStatsView.xaml
    /// </summary>
    public partial class PlayerStatsView : INotifyPropertyChanged
    {
        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register("Player", typeof(Player), typeof(PlayerStatsView), new PropertyMetadata(PlayerSetCallback));

        private static void PlayerSetCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PlayerStatsView psv))
            {
                return;
            }
            foreach (PropertyInfo propertyInfo in d.GetType().GetProperties())
            {
                psv.OnPropertyChanged(propertyInfo.Name);
            }
        }


        [Bindable(true)]
        public Player Player
        {
            get => GetValue(PlayerProperty) as Player;
            set => SetValue(PlayerProperty, value);
        }

        public int GamesCount => Player == null ? -1 : QueryRestrictions.FilterParticipations(Player.Participations).Count();

        public int GoodGamesCount => Player == null ? -1 : QueryRestrictions.FilterParticipations(Player.Participations).Count(x => x.GameRole.Side);

        public int EvilGamesCount => Player == null ? -1 : QueryRestrictions.FilterParticipations(Player.Participations).Count(x => !x.GameRole.Side);

        public double GeneralWr
        {
            get
            {
                if (Player == null)
                {
                    return -1;
                }
                double wonGamesCount = QueryRestrictions.FilterParticipations(Player.Participations).Count(x => x.GameView.WonBy == x.GameRole.Side);
                double winrate = Math.Round(wonGamesCount / GamesCount * 100, 2);
                return double.IsNaN(winrate) ? 0 : winrate;
            }
        }

        public double GoodWr
        {
            get
            {
                if (Player == null)
                {
                    return -1;
                }
                double wonGamesCount = QueryRestrictions.FilterParticipations(Player.Participations).Count(x => x.GameRole.Side && x.GameView.WonBy.Value);
                double winrate = Math.Round(wonGamesCount / GoodGamesCount * 100, 2);
                return double.IsNaN(winrate) ? 0 : winrate;
            }
        }

        public double EvilWr
        {
            get
            {
                if (Player == null)
                {
                    return -1;
                }
                double wonGamesCount = QueryRestrictions.FilterParticipations(Player.Participations).Count(x => !x.GameRole.Side && !x.GameView.WonBy.Value);
                double winrate = Math.Round(wonGamesCount / EvilGamesCount * 100, 2);
                return double.IsNaN(winrate) ? 0 : winrate;
            }
        }

        public PlayerStatsView()
        {
            InitializeComponent();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
