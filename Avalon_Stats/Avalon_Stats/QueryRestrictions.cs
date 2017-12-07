using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon_Stats
{
    class QueryRestrictions
    {
        public static DateTime MinDateTime { get; set; }
        public static DateTime MaxDateTime { get; set; }
        public static int? GroupSize { get; set; }
        public static Player ContainedPlayer { get; set; }

        private static DateTime DefaultMinDateTime { get; }
        private static DateTime DefaultMaxDateTime { get; }
        private static int? DefaultGroupSize { get; }
        private static Player DefaultContainedPlayer { get; }

        static QueryRestrictions()
        {
            AvalonDBDataContext dbcontext = new AvalonDBDataContext();
            DefaultMinDateTime = dbcontext.Games.Select(x => x.GameTime).Min();
            DefaultMaxDateTime = dbcontext.Games.Select(x => x.GameTime).Max();
            DefaultGroupSize = null;
            DefaultContainedPlayer = null;

            ResetMinDateTime();
            ResetMaxDateTime();
            ResetGroupSize();
            ResetContainedPlayer();
        }

        public static void ResetMinDateTime()
        {
            MinDateTime = DefaultMinDateTime;
        }

        public static void ResetMaxDateTime()
        {
            MaxDateTime = DefaultMaxDateTime;
        }

        public static void ResetGroupSize()
        {
            GroupSize = DefaultGroupSize;
        }

        public static void ResetContainedPlayer()
        {
            ContainedPlayer = DefaultContainedPlayer;
        }

        public static IEnumerable<Participation> FilterParticipations(IEnumerable<Participation> baseSet)
        {
            return baseSet.Where(x => x.Game.GameTime >= MinDateTime
                                      && x.Game.GameTime <= MaxDateTime
                                      && (GroupSize == null || x.GameView.GroupSize == GroupSize)
                                      && (ContainedPlayer == null || x.GameView.Participations.Select(y => y.PlayerName)
                                          .Contains(ContainedPlayer.PlayerName)));
        }

        public static IEnumerable<GameView> FilterGameViews(IEnumerable<GameView> baseSet)
        {
            return baseSet.Where(x => x.GameTime >= MinDateTime
                                      && x.GameTime <= MaxDateTime
                                      && (GroupSize == null || x.GroupSize == GroupSize));
        }
    }
}
