using HearthDb.Enums;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace HDT.Plugins.TransferStudentTracker.Settings
{
    //Credit to the Graveyard Plugin owner: Redhatter
    //https://github.com/RedHatter/Graveyard
    public static class SettingsUtilities
    {

        public static bool IsGameSettingEnabled()
        {
            var currentMode = Core.Game.CurrentGameMode;
            bool modeEnabled = GetGameModeSetting(currentMode);

            //for modes not spectator mode
            if (currentMode.ToString() != "Spectator" || modeEnabled == false)
            {
                return modeEnabled;
            }
            //for the spectator mode only;
            else
            {
                //use other settings to determine whether to display tracker;
                var currentType = Core.Game.CurrentGameType;
                var currentSpecMode = HearthDbConverter.GetGameMode(currentType);
                return GetGameModeSetting(currentSpecMode);
            }
        }

        private static bool GetGameModeSetting(GameMode property)
        {
            switch (property.ToString())
            {
                case "Arena": 
                    return TSPluginSettings.Default.ArenaEnabled;
                case "Brawl": 
                    return TSPluginSettings.Default.BrawlEnabled;
                case "Casual": 
                    return TSPluginSettings.Default.CasualEnabled;
                case "Friendly": 
                    return TSPluginSettings.Default.FriendlyEnabled;
                case "Practice": 
                    return TSPluginSettings.Default.PracticeEnabled;
                case "Ranked": 
                    return TSPluginSettings.Default.RankedEnabled;
                case "Spectator": 
                    return TSPluginSettings.Default.SpectatorEnabled;
                default:
                    return false;
            }
            
        }
    }

}
