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
        public static GameRole ContainedRole { get; set; }

        private static DateTime DefaultMinDateTime { get; set; }
        private static DateTime DefaultMaxDateTime { get; set; }
        private static int? DefaultGroupSize { get; }
        private static Player DefaultContainedPlayer { get; }
        private static GameRole DefaultContainedRole { get; }

        static QueryRestrictions()
        {
            AvalonDBDataContext dbcontext = AvalonDbDataContextProvider.DbContextInstance;
            DefaultMinDateTime = dbcontext.Games.Select(x => x.GameTime).Min();
            DefaultMaxDateTime = dbcontext.Games.Select(x => x.GameTime).Max();
            DefaultGroupSize = null;
            DefaultContainedPlayer = null;
            DefaultContainedRole = null;

            ResetMinDateTime();
            ResetMaxDateTime();
            ResetGroupSize();
            ResetContainedPlayer();
            ResetContainedRole();
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

        public static void ResetContainedRole()
        {
            ContainedRole = DefaultContainedRole;
        }

        public static IEnumerable<Participation> FilterParticipations(IEnumerable<Participation> baseSet)
        {
            return baseSet.Where(x => x.Game.GameTime >= MinDateTime
                                   && x.Game.GameTime <= MaxDateTime
                                   && (GroupSize == null || x.GameView.GroupSize == GroupSize)
                                   && (ContainedPlayer == null || x.GameView.Participations.Select(y => y.PlayerName)
                                        .Contains(ContainedPlayer.PlayerName))
                                   && (ContainedRole == null || x.GameView.Participations.Select(y => y.RoleShortcut)
                                        .Contains(ContainedRole.RoleShortcut)));
        }

        public static IEnumerable<GameView> FilterGameViews(IEnumerable<GameView> baseSet)
        {
            return baseSet.Where(x => x.GameTime >= MinDateTime
                                   && x.GameTime <= MaxDateTime
                                   && (GroupSize == null || x.GroupSize == GroupSize)
                                   && (ContainedPlayer == null || x.Participations.Select(y => y.PlayerName)
                                        .Contains(ContainedPlayer.PlayerName))
                                   && (ContainedRole == null || x.Participations.Select(y => y.RoleShortcut)
                                        .Contains(ContainedRole.RoleShortcut)));
        }

        public static void RefreshDateTimeRestricitons()
        {
            AvalonDBDataContext dbcontext = AvalonDbDataContextProvider.DbContextInstance;
            DefaultMinDateTime = dbcontext.Games.Select(x => x.GameTime).Min().Date;
            DefaultMaxDateTime = dbcontext.Games.Select(x => x.GameTime).Max().Date;
            ResetMinDateTime();
            ResetMaxDateTime();
        }
    }
}
