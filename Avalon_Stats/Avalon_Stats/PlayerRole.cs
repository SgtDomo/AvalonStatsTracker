using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon_Stats
{
    public class PlayerRole
    {
        public Player Player { get; set; }
        public GameRole Role { get; set; }

        public PlayerRole() : this(null, null)
        {
        }

        public PlayerRole(Player player, GameRole role)
        {
            Player = player;
            Role = role;
        }
    }
}
