﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BrowserChooser.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("FIREFOX.EXE")]
        public string CheckedBrowsers {
            get {
                return ((string)(this["CheckedBrowsers"]));
            }
            set {
                this["CheckedBrowsers"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool hasSetDefault {
            get {
                return ((bool)(this["hasSetDefault"]));
            }
            set {
                this["hasSetDefault"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::BrowserChooser.RememberedUrlActions StoredActions {
            get {
                return ((global::BrowserChooser.RememberedUrlActions)(this["StoredActions"]));
            }
            set {
                this["StoredActions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool UpgradeSettings {
            get {
                return ((bool)(this["UpgradeSettings"]));
            }
            set {
                this["UpgradeSettings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"
     <Protocols xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
      <Items>
       <Protocol>
        <Proto>http</Proto>
        <Desc>URL:HyperText Transfer Protocol</Desc>
       </Protocol>
       <Protocol>
        <Proto>https</Proto>
        <Desc>URL:HyperText Transfer Protocol with Privacy</Desc>
       </Protocol>
      </Items>
     </Protocols>
    ")]
        public global::BrowserChooser.Protocols URLHandlers {
            get {
                return ((global::BrowserChooser.Protocols)(this["URLHandlers"]));
            }
            set {
                this["URLHandlers"] = value;
            }
        }
    }
}