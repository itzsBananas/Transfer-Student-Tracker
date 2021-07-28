using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using Emgu.CV;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Controls;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Hearthstone_Deck_Tracker.Windows;
using HDT.Plugins.TransferStudentTracker.Utilities;
using Card = Hearthstone_Deck_Tracker.Hearthstone.Card;
using Core = Hearthstone_Deck_Tracker.API.Core;

using Path = System.IO.Path;
using System.IO;

namespace HDT.Plugins.TransferStudentTracker.Controls
{
    /// <summary>
    /// Interaction logic for TransferStudentButton.xaml
    /// </summary>
    public partial class TransferStudentButton : UserControl
    {
        private const float xPosOffset = 230;
        private const float yPosOffset = 150;
        private const float heightScale = 0.025f;
        private const float screenRatio = 4/3f;
        private const float maxHeight = 40;

        public const string buttonIconNameColor = "TransferStudentIcon.png";
        public const string buttonIconNameBW = "TransferStudentIconBW.png";

        public double absLeftValue;

        private bool _hoverOnToggler = false;
        private bool imagesFound = false;

        public TSToolTipControl tooltip;

        public BitmapImage TSBrushColor;
        public BitmapImage TSBrushBW;

        public TransferStudentButton()
        {
            InitializeComponent();

            InitializeButtonImages();

            Core.OverlayCanvas.Children.Add(this);
            //this.Loaded += ControlLoaded;
            this.Visibility = Visibility.Hidden;
            //SetButtonImageColor(false);

            tooltip = new TSToolTipControl();

            Button_Scaling_Positioning();

            //Whenever Size of Window is changed, change button appropriately based on pos and scale
            Core.OverlayCanvas.SizeChanged += (s, e) => Button_Scaling_Positioning();
        }

        private void InitializeButtonImages()
        {
            //string assemblylocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string bwfile = Path.Combine(FileRetrieval.GetResourcesDir, buttonIconNameBW);
            string colorfile = Path.Combine(FileRetrieval.GetResourcesDir, buttonIconNameColor);

            if (!File.Exists(bwfile) || !File.Exists(colorfile))
            {
                imagesFound = false;
            }
            else
            {
                imagesFound = true;
                TSBrushBW = new BitmapImage(new Uri(bwfile));
                TSBrushColor = new BitmapImage(new Uri(colorfile));
            }

        }

        public void SetButtonImageColor(bool color)
        {
            if (imagesFound)
            {
                if (color)
                {
                    TSButtonImage.ImageSource = TSBrushColor;
                }
                else
                {
                    TSButtonImage.ImageSource = TSBrushBW;
                }
            }
        }

        //Copied from FaceOnly Plugin; https://github.com/antfu/FaceOnly
        //Maybe enable click in the future; http://joelabrahamsson.com/detecting-mouse-and-keyboard-input-with-net/
        //Input manager in graveyard
        public void OnUpdate()
        {

            if (DetectingBoardMain.effectFound && tooltip != null && TSButton != null && TSButton.Visibility == Visibility.Visible)
            {
                var pos = User32.GetMousePos();

                var point = TSButton.PointFromScreen(new Point(pos.X, pos.Y));
                //Don't delete next line; prevents crash if in Hearthstone game and debugging
                var p = point;

                var hovered = p.X > 0 && p.X < TSButton.ActualWidth && p.Y > 0 && p.Y < TSButton.ActualHeight;

                _hoverOnToggler = tooltip.IsToggled();

                if (!_hoverOnToggler && hovered)
                {
                    tooltip.Toggle();
                }
                else if (_hoverOnToggler && !hovered)
                {
                    tooltip.Untoggle();
                }
            }
        }

        public void Button_Scaling_Positioning()
        {
            var size = Core.OverlayCanvas.RenderSize;

            var scaledHeight = heightScale * size.Width;
            this.Height = maxHeight < scaledHeight ? maxHeight : scaledHeight;
            this.Width = screenRatio * this.Height;

            absLeftValue = size.Width - xPosOffset - this.Width;
            Canvas.SetLeft(this, absLeftValue);
            Canvas.SetBottom(this, yPosOffset);

            tooltip.Tooltip_Positioning(absLeftValue, yPosOffset + this.Height); 
        }
    }
}
