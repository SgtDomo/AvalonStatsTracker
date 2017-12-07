using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Avalon_Stats.Annotations;
using DateTime = System.DateTime;

namespace Avalon_Stats
{
    enum MissionResult
    {
        NotPlayed,
        Evil,
        Good
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private UserControl _contentUc;
        public UserControl ContentUc
        {
            get => _contentUc;
            set
            {
                switch (value)
                {
                    case RestrictionsView _:
                        ContentUcVerticalAlignment = VerticalAlignment.Center;
                        ContentUcHorizontalAlignment = HorizontalAlignment.Center;
                        break;
                    case GameRegistrationView _:
                        ContentUcHorizontalAlignment = HorizontalAlignment.Center;
                        break;
                    default:
                        ContentUcHorizontalAlignment = HorizontalAlignment.Stretch;
                        ContentUcVerticalAlignment = VerticalAlignment.Stretch;

                        break;
                }
                _contentUc = value; 
                OnPropertyChanged();
            }
        }

        private HorizontalAlignment _contentUcHorizontalAlignment;

        public HorizontalAlignment ContentUcHorizontalAlignment
        {
            get => _contentUcHorizontalAlignment;
            set
            {
                _contentUcHorizontalAlignment = value;
                OnPropertyChanged();
            }
        }

        private VerticalAlignment _contentUcVerticalAlignment;

        public VerticalAlignment ContentUcVerticalAlignment
        {
            get => _contentUcVerticalAlignment;
            set
            {
                _contentUcVerticalAlignment = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            RegisterGameBtn_OnClick(null, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RegisterGameBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ContentUc = new GameRegistrationView();
        }

        private void PlayerStatsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ContentUc = new PlayerStatsListView();
        }

        private void RestrictionsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ContentUc = new RestrictionsView();
        }

        private void GeneralStatsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ContentUc = new GeneralGameStats();
        }
    }
}
