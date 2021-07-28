using Hearthstone_Deck_Tracker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDT.Plugins.TransferStudentTracker.Utilities
{
    public class FileRetrieval
    {
        public static string GetResourcesDir
        {
            get { return Path.Combine(Config.Instance.DataDir, "Plugins", "TransferStudentTracker", "Resources"); }
        }
    }
}
