using Hearthstone_Deck_Tracker.API;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace HDT.Plugins.TransferStudentTracker.Settings
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class TSPluginSettingsView : ScrollViewer
    {
		private static Flyout _flyout;

		public static Flyout Flyout
		{
			get
			{
				if (_flyout == null)
				{
					_flyout = CreateSettingsFlyout();
				}
				return _flyout;
			}
		}

		private static Flyout CreateSettingsFlyout()
		{
			var settings = new Flyout();
			settings.Position = Position.Left;
			Panel.SetZIndex(settings, 100);
			settings.Header = "Plugin Settings";
			settings.Content = new TSPluginSettingsView();
			Core.MainWindow.Flyouts.Items.Add(settings);
			return settings;
			
		}

		public TSPluginSettingsView()
		{
			InitializeComponent();
			TSPluginSettings.Default.PropertyChanged += (sender, e) => TSPluginSettings.Default.Save();
		}
	}
}
