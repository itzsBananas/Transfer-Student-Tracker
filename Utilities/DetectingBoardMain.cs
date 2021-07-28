using HDT.Plugins.TransferStudentTracker.Enums;
using HDT.Plugins.TransferStudentTracker.Controls;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using System.Threading.Tasks;
using Core = Hearthstone_Deck_Tracker.API.Core;
using Point = System.Drawing.Point;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Hearthstone_Deck_Tracker.Enums;

namespace HDT.Plugins.TransferStudentTracker.Utilities
{

    public static class DetectingBoardMain
    {
        //change rectangle to look at part of board
        private static Rectangle portraitcroprectbottom = new Rectangle(96, 616, 192, 192);
        private static Rectangle portraitcroprecttop = new Rectangle(247, 0, 128, 128);

        public static bool detectionInProgress = false;
        public static bool effectFound = false;

        public const string boardHashesFileName = "BoardHashes.json";

        public static List<TSHashList> TSHashListWhole = new List<TSHashList>();
        public static List<TSHashPair> TSHashPairBottom = new List<TSHashPair>();
        public static List<TSHashPair> TSHashPairTop = new List<TSHashPair>();

        public static String boardID;
        public static TSToolTipControl TStooltip;

        public static TransferStudentButton TSButton;

        public static bool initialDetectionDelayDone;

        public static int best_distance_test;



        public static void InitializeDetection(TSToolTipControl tooltip, TransferStudentButton button)
        {
            LoadData();
            TStooltip = tooltip;
            TSButton = button;
            InitializeDetectionControls();
        }

        public async static void DetectBoardUpdate()
        {
            //var stateOfGame = Core.Game.PlayerEntity?.GetTag(HearthDb.Enums.GameTag.MULLIGAN_STATE); //&& stateOfGame == (int)Mulligan.DONE 
            if (TrackerPlugin.loaded && (TrackerPlugin.gameState == GameState.PreMulligan || TrackerPlugin.gameState == GameState.PostMulligan) && !detectionInProgress && !effectFound)
            {
                detectionInProgress = true;

                //delay to not read previous gameboard
                if (!initialDetectionDelayDone && TrackerPlugin.gameState == GameState.PreMulligan)
                {
                    await Task.Delay(2000);
                    initialDetectionDelayDone = true;
                }
                String id = GetBoardID();
                if (id.Length != 0)
                {
                    TStooltip.ChangeTooltipEffect(id);
                    effectFound = true;
                    TSButton.SetButtonImageColor(true);
                }
                detectionInProgress = false;
            }
        }

        public static void InitializeDetectionControls()
        {
            detectionInProgress = false;
            effectFound = false;
            initialDetectionDelayDone = false;
            TSButton.SetButtonImageColor(false);
        }



        private static void LoadData()
        {
            if (!Directory.Exists(FileRetrieval.GetResourcesDir))
                Log.Error("Resources Directory is missing");

            //may support auto updates in the future :)
            //put code that would support it here

            string boardhashesfile = Path.Combine(FileRetrieval.GetResourcesDir, boardHashesFileName);

            if (File.Exists(boardhashesfile))
            {
                try
                {
                    TSHashListWhole = JsonConvert.DeserializeObject<List<TSHashList>>(File.ReadAllText(boardhashesfile));
                    if (TSHashListWhole == null)
                    {
                        Log.Error("The Board Hashes object returned null.");
                    }
                }
                catch
                {
                    Log.Error("The Board Hashes was not deserialized successfully.");
                }
            }
            else
            {
                Log.Error("The Board Hashes File in the Resources Directory is missing.");
            }
        }
        

        public static string GetBoardID()
        {
            //Core.OverlayCanvas.Visibility = System.Windows.Visibility.Collapsed;
            //await Task.Delay(5000);

            // Get the Hearthstone window rectangle
            var hsrect = User32.GetHearthstoneRect(false);
            if (hsrect.Width <= 0 || hsrect.Height <= 0)
            {
                return "";
            }

            Bitmap screenshot = ScreenCapture.CaptureHearthstone(new Point(0, 0), (int)hsrect.Width, (int)hsrect.Height);

            //don't save if screenshot is null
            if (screenshot == null)
            {
                return "";
            }

            var hashexpbottom = GetHashFromScreenshot(hsrect, screenshot, portraitcroprectbottom);
            var hashexptop = GetHashFromScreenshot(hsrect, screenshot, portraitcroprecttop);

            string id = FindBoardID(hashexpbottom, hashexptop);
            //compare difference and return string id
            return id;
        }

        private static ulong GetHashFromScreenshot(Rectangle hsrect, Bitmap screenshot, Rectangle portraitcroprect)
        {
            Point pos = Detection.GetHSPos(hsrect, portraitcroprect.X, portraitcroprect.Y, 1280, 960);
            Point size = Detection.GetHSSize(hsrect, portraitcroprect.Width, portraitcroprect.Height, 1280, 960);

            Bitmap cropped = screenshot.Clone(new Rectangle(pos.X, pos.Y, size.X, size.Y), screenshot.PixelFormat);

            var hashexp = HashCalculations.GetImageHash(cropped);

            return hashexp;
        }

        
        private static String FindBoardID(ulong hashexpbottom, ulong hashexptop)
        {
            string id = "";
            int best_distance = int.MaxValue;
            ulong tempDistBottom;
            ulong tempDistTop;
            foreach (TSHashList ts in TSHashListWhole)
            {
                if (TrackerPlugin.gameState == GameState.PreMulligan)
                {
                    tempDistBottom = ts.preMulligan.bottom;
                    tempDistTop = ts.preMulligan.top;
                }
                else if(TrackerPlugin.gameState == GameState.PostMulligan)
                {
                    tempDistBottom = ts.postMulligan.bottom;
                    tempDistTop = ts.postMulligan.top;
                }
                else
                {
                    Log.Error("Comparing hashes in the wrong mode...");
                    return "";
                }
                int distBottom = HashCalculations.GetHashDistance(tempDistBottom, hashexpbottom);
                int distTop = HashCalculations.GetHashDistance(tempDistTop, hashexptop);

                int totaldist = distBottom + distTop;

                if (totaldist < best_distance)
                {
                    id = ts.id;
                    best_distance = totaldist;
                }
            }
            best_distance_test = best_distance;
            if (best_distance_test >= 30)
                id = "";
            return id;
        }

        public class TSHashList
        {
            public string id;
            public TSHashPair preMulligan;
            public TSHashPair postMulligan;

            public TSHashList(string id, string preB, string preT, string postB, string postT)
            {
                this.id = id;
                this.preMulligan = new TSHashPair(preB, preT);
                this.postMulligan = new TSHashPair(postB, postT);
            }
        }

        public class TSHashPair
        {
            public ulong bottom;
            public ulong top;

            public TSHashPair(string b, string t)
            {
                bottom = Convert.ToUInt64(b);
                top = Convert.ToUInt64(t);
            }
        }
    }
}
