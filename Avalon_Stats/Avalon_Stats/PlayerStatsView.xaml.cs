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
            AvalonDbDataContextProvider.RefreshObject(psv.Player);
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

        public int GamesCount => Player?.PlayedGamesCount() ?? -1;

        public int GoodGamesCount => Player?.GoodGamesCount() ?? -1;

        public int EvilGamesCount => Player?.EvilGamesCount() ?? -1;

        public double GeneralWr => Player?.GeneralWr() ?? -1;

        public double GoodWr => Player?.GoodWr() ?? -1;

        public double EvilWr => Player?.EvilWr() ?? -1;

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
