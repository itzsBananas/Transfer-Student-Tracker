using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using HDT.Plugins.TransferStudentTracker.Controls;
using Hearthstone_Deck_Tracker.API;
using System.Windows;
using Hearthstone_Deck_Tracker.Controls;
using HDT.Plugins.TransferStudentTracker.Utilities;
using HearthDb.Enums;
using HDT.Plugins.TransferStudentTracker.Enums;
using HDT.Plugins.TransferStudentTracker.Settings;

namespace HDT.Plugins.TransferStudentTracker
{
    public class TrackerPlugin : IPlugin
    {
        public static TransferStudentButton button;
        public static TSToolTipControl tstooltip;

        public static bool loaded = false;  

        public static GameState gameState;

        public void OnLoad()
        {
            // Triggered upon startup and when the user ticks the plugin on

            // Adding Button and corresponding tip control
            button = new TransferStudentButton();
            tstooltip = button.tooltip;

            DetectingBoardMain.InitializeDetection(tstooltip, button);
            gameState = GameState.InMenu;

            loaded = true;
        }

        public void OnUnload()
        {
            // Triggered when the user unticks the plugin, however, HDT does not completely unload the plugin.
            // see https://git.io/vxEcH
            TSPluginSettings.Default.Save();

            Core.OverlayCanvas.Children.Remove(button);
            Core.OverlayCanvas.Children.Remove(tstooltip.tooltip);

            loaded = false;
        }

        public void OnButtonPress()
        {
            // Triggered when the user clicks your button in the plugin list

            //maybe copy from graveyard or arena helper plugin...
            TSPluginSettingsView.Flyout.IsOpen = true;
        }

        public void OnUpdate()
        {
            if (loaded)
            {
                // called every ~100ms
                button.OnUpdate();
                if (!Core.Game.IsInMenu && Core.Game.CurrentGameMode != GameMode.Battlegrounds && SettingsUtilities.IsGameSettingEnabled())
                {
                    if(button.Visibility != Visibility.Visible)
                    {
                        button.Visibility = Visibility.Visible;
                    }
                    var state = Core.Game.PlayerEntity?.GetTag(HearthDb.Enums.GameTag.MULLIGAN_STATE);
                    if (state > 0 && state < 4) //in mulligan phase
                    {
                        gameState = GameState.PreMulligan;
                    }
                    else //post mulligan
                    {
                        gameState = GameState.PostMulligan;
                    }
                }
                else //InMenu
                {
                    if (button.Visibility != Visibility.Hidden)
                    {
                        button.Visibility = Visibility.Hidden;
                    }
                    if (tstooltip.tooltip.Visibility != Visibility.Hidden)
                    {
                        tstooltip.tooltip.Visibility = Visibility.Hidden;
                    }
                    gameState = GameState.InMenu;
                    DetectingBoardMain.InitializeDetectionControls();
                }
                DetectingBoardMain.DetectBoardUpdate();
            }
        }

        public string Name => "Transfer Student Tracker";

        public string Description => "Tracks the Effect of the Hearthstone card, Transfer Student.";

        public string ButtonText => "Settings";

        public string Author => "Discord: Banana#4408";

        public Version Version => new Version(1, 0, 0);

        public MenuItem MenuItem => null;
    }
}
