using HDT.Plugins.TransferStudentTracker.Utilities;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDT.Plugins.TransferStudentTracker.Settings
{
    //Credit to the Graveyard Plugin owner: Redhatter
    //https://github.com/RedHatter/Graveyard
    public class Settings
    {
        public Settings() { }
        public Settings(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public string Value { get; set; }


    }

    public sealed partial class TSPluginSettings
    {
        private const string Filename = "Settings.xml";
        private static string SettingsPath => Path.Combine(FileRetrieval.GetResourcesDir, Filename);

        public TSPluginSettings()
        {
            var provider = Providers;

            SettingsLoaded += SettingsLoadedEventHandler;
            SettingsSaving += SettingsSavingEventHandler;
        }

        private void SettingsLoadedEventHandler(object sender, System.Configuration.SettingsLoadedEventArgs e)
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var actual = XmlManager<List<Settings>>.Load(SettingsPath);

                    foreach (var setting in actual)
                    {
                        this[setting.Name] = Convert.ChangeType(setting.Value, this.Properties[setting.Name].PropertyType);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                var saveFormat = PropertyValues.Cast<SettingsPropertyValue>()
                    .Where(p => p.SerializedValue.ToString() != p.Property.DefaultValue.ToString())
                    .Select(p => new Settings(p.Name, p.SerializedValue.ToString()))
                    .ToList();

                XmlManager<List<Settings>>.Save(SettingsPath, saveFormat);

                e.Cancel = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
