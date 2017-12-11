using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon_Stats
{
    class PlayerWrapper
    {
        public Player Player { get; }

        public String FirstName => Player.FirstName;

        public String PlayerName => Player.PlayerName;



        public PlayerWrapper(Player player)
        {
            Player = player;
        }
    }
}
