using System;
using System.Collections.Generic;
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
    /// Interaction logic for GeneralGameStats.xaml
    /// </summary>
    public partial class GeneralGameStats : INotifyPropertyChanged
    {
        private static readonly AvalonDBDataContext Dbcontext = AvalonDbDataContextProvider.DbContextInstance;

        public int GamesCount => Dbcontext.GetFilteredGameViews().Count();

        public int GoodWonGamesCount => Dbcontext.GetFilteredGameViews().Count(x => x.WonBy);

        public double GoodWr => (double) GoodWonGamesCount / GamesCount * 100;

        public int EvilWonGamesCount => Dbcontext.GetFilteredGameViews().Count(x => !x.WonBy);

        public double EvilWr => (double) EvilWonGamesCount / GamesCount * 100;


        public GeneralGameStats()
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
