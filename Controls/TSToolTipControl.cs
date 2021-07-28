using Hearthstone_Deck_Tracker.Controls;
using Hearthstone_Deck_Tracker.Hearthstone;
using Core = Hearthstone_Deck_Tracker.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HDT.Plugins.TransferStudentTracker.Controls;
using Hearthstone_Deck_Tracker.API;
using HDT.Plugins.TransferStudentTracker.Utilities;
using System.Windows.Controls;

namespace HDT.Plugins.TransferStudentTracker.Controls
{
    public class TSToolTipControl
    {
        public CardToolTipControl tooltip;

        private const int xPosOffset = 210;

        private bool _hoverOnToggler = false;

        public TSToolTipControl()
        {
            //Add 
            //Look at OverlayWindow.Tooltips.cs for scaling stuff
            var TSCard = Database.GetCardFromId("SCH_199");
            tooltip = new CardToolTipControl();
            tooltip.SetValue(FrameworkElement.DataContextProperty, TSCard);

            tooltip.Visibility = Visibility.Hidden;

            Core.OverlayCanvas.Children.Add(tooltip);
        }

        public void Toggle()
        {
            tooltip.Visibility = Visibility.Visible;
            _hoverOnToggler = true;
        }

        public void Untoggle()
        {
            tooltip.Visibility = Visibility.Hidden;
            _hoverOnToggler = false;
        }

        public bool IsToggled()
        {
            return _hoverOnToggler;
        }

        public void Tooltip_Positioning(double absXPos, double absYPos)
        {
            var size = Core.OverlayCanvas.RenderSize;

            Canvas.SetLeft(tooltip, absXPos - xPosOffset);
            Canvas.SetTop(tooltip, size.Height - absYPos);
        }

        public void ChangeTooltipEffect(string id)
        {
            //remove old tooltip
            Core.OverlayCanvas.Children.Remove(tooltip);

            var TSCard = Database.GetCardFromId(id);
            tooltip = new CardToolTipControl();
            tooltip.SetValue(FrameworkElement.DataContextProperty, TSCard);

            tooltip.Visibility = Visibility.Hidden;
            TrackerPlugin.button.Button_Scaling_Positioning();

            Core.OverlayCanvas.Children.Add(tooltip);
        }

    }
}
