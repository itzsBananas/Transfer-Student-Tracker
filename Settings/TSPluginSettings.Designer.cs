﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HDT.Plugins.TransferStudentTracker.Settings {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.6.0.0")]
    public sealed partial class TSPluginSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static TSPluginSettings defaultInstance = ((TSPluginSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new TSPluginSettings())));
        
        public static TSPluginSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CasualEnabled {
            get {
                return ((bool)(this["CasualEnabled"]));
            }
            set {
                this["CasualEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ArenaEnabled {
            get {
                return ((bool)(this["ArenaEnabled"]));
            }
            set {
                this["ArenaEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool BrawlEnabled {
            get {
                return ((bool)(this["BrawlEnabled"]));
            }
            set {
                this["BrawlEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool FriendlyEnabled {
            get {
                return ((bool)(this["FriendlyEnabled"]));
            }
            set {
                this["FriendlyEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PracticeEnabled {
            get {
                return ((bool)(this["PracticeEnabled"]));
            }
            set {
                this["PracticeEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SpectatorEnabled {
            get {
                return ((bool)(this["SpectatorEnabled"]));
            }
            set {
                this["SpectatorEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool RankedEnabled {
            get {
                return ((bool)(this["RankedEnabled"]));
            }
            set {
                this["RankedEnabled"] = value;
            }
        }
    }
}
