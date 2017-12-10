using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon_Stats
{
    static class PlayerExtensionMethods
    {
        public static int PlayedGamesCount(this Player player) => QueryRestrictions.FilterParticipations(player.Participations).Count();

        public static double GeneralWr(this Player player)
        {
            return (double)QueryRestrictions.FilterParticipations(player.Participations)
                    .Count(x => x.GameRole.Side == x.GameView.WonBy)
                / PlayedGamesCount(player) * 100;
        }

        public static int GoodGamesCount(this Player player)
        {
            return QueryRestrictions.FilterParticipations(player.Participations).Count(x => x.GameRole.Side);
        }

        public static double GoodWr(this Player player)
        {
            return (double)QueryRestrictions.FilterParticipations(player.Participations)
                       .Count(x => x.GameRole.Side && x.GameView.WonBy)
                   / GoodGamesCount(player) * 100;
        }

        public static int EvilGamesCount(this Player player)
        {
            return QueryRestrictions.FilterParticipations(player.Participations).Count(x => !x.GameRole.Side);
        }

        public static double EvilWr(this Player player)
        {
            return (double) QueryRestrictions.FilterParticipations(player.Participations)
                    .Count(x => !x.GameRole.Side && !x.GameView.WonBy)
                / EvilGamesCount(player) * 100;
        }
    }

    static class AvalonDbDataContextExtensionMethods
    {
        public static IEnumerable<GameView> GetFilteredGameViews(this AvalonDBDataContext dbcontext)
        {
            return QueryRestrictions.FilterGameViews(dbcontext.GameViews);
        }
    }
}
