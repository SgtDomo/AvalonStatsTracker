using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalon_Stats
{
    class UnuniqueRoleProvider
    {
        public static string[] UnuniqueRoleShortcuts { get; } =
            Properties.Settings.Default.UnuniqueRoles.Split(';').Where(x => x != "").ToArray();

    }
}
