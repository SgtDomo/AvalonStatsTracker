using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Avalon_Stats.Annotations;

namespace Avalon_Stats
{
    /// <summary>
    /// Interaction logic for RestrictionsView.xaml
    /// </summary>
    public partial class RestrictionsView : INotifyPropertyChanged
    {
        public DateTime MinDateTime
        {
            get => QueryRestrictions.MinDateTime;
            set => QueryRestrictions.MinDateTime = value;
        }

        public DateTime MaxDateTime
        {
            get => QueryRestrictions.MaxDateTime;
            set => QueryRestrictions.MaxDateTime = value;
        }

        public int? GroupSize
        {
            get => QueryRestrictions.GroupSize;
            set => QueryRestrictions.GroupSize = value;
        }

        public Player ContainedPlayer
        {
            get => QueryRestrictions.ContainedPlayer;
            set => QueryRestrictions.ContainedPlayer = value;
        }

        public Player[] Players { get; }

        public RestrictionsView()
        {
            Players = new AvalonDBDataContext().Players.ToArray();
            InitializeComponent();
        }

        private void MinDateTimeResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            QueryRestrictions.ResetMinDateTime();
            OnPropertyChanged(nameof(MinDateTime));
        }

        private void MaxDateTimeResetBtn_OnClickDateTimeResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            QueryRestrictions.ResetMaxDateTime();
            OnPropertyChanged(nameof(MaxDateTime));
        }

        private void GroupSizeResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            QueryRestrictions.ResetGroupSize();
            OnPropertyChanged(nameof(GroupSize));
        }

        private void ContainedPlayerResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            QueryRestrictions.ResetContainedPlayer();
            OnPropertyChanged(nameof(ContainedPlayer));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
