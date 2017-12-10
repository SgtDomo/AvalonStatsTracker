using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Avalon_Stats
{
    class AvalonDbDataContextProvider
    {
        private static AvalonDBDataContext _dbcontextInstance;

        public static AvalonDBDataContext DbContextInstance => _dbcontextInstance ?? (_dbcontextInstance = new AvalonDBDataContext());

        public static void RefreshInstance() => _dbcontextInstance = new AvalonDBDataContext();

        public static void RefreshObject(object obj) =>
            DbContextInstance.Refresh(RefreshMode.OverwriteCurrentValues, obj);
    }
}
