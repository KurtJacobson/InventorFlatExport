﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InventorFlatExport.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.1.0.0")]
    internal sealed partial class DxfSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static DxfSettings defaultInstance = ((DxfSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new DxfSettings())));
        
        public static DxfSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DashDot")]
        public global::System.Drawing.Drawing2D.DashStyle OuterProfileLineType {
            get {
                return ((global::System.Drawing.Drawing2D.DashStyle)(this["OuterProfileLineType"]));
            }
            set {
                this["OuterProfileLineType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("InnerOutlines")]
        public string InteriorProfilesLayer {
            get {
                return ((string)(this["InteriorProfilesLayer"]));
            }
            set {
                this["InteriorProfilesLayer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Outline")]
        public string OuterProfileLayer {
            get {
                return ((string)(this["OuterProfileLayer"]));
            }
            set {
                this["OuterProfileLayer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("192, 0, 0")]
        public global::System.Drawing.Color OuterProfileLayerColor {
            get {
                return ((global::System.Drawing.Color)(this["OuterProfileLayerColor"]));
            }
            set {
                this["OuterProfileLayerColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Green")]
        public global::System.Drawing.Color InteriorProfilesLayerColor {
            get {
                return ((global::System.Drawing.Color)(this["InteriorProfilesLayerColor"]));
            }
            set {
                this["InteriorProfilesLayerColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("kDefaultLineType")]
        public global::Inventor.LineTypeEnum InteriorProfilesLineType {
            get {
                return ((global::Inventor.LineTypeEnum)(this["InteriorProfilesLineType"]));
            }
            set {
                this["InteriorProfilesLineType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("BendingLines")]
        public string BendUpLayer {
            get {
                return ((string)(this["BendUpLayer"]));
            }
            set {
                this["BendUpLayer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Blue")]
        public global::System.Drawing.Color BendUpLayerColor {
            get {
                return ((global::System.Drawing.Color)(this["BendUpLayerColor"]));
            }
            set {
                this["BendUpLayerColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("kDashedLineType")]
        public global::Inventor.LineTypeEnum BendUpLineType {
            get {
                return ((global::Inventor.LineTypeEnum)(this["BendUpLineType"]));
            }
            set {
                this["BendUpLineType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("BendingLines")]
        public string BendDownLayer {
            get {
                return ((string)(this["BendDownLayer"]));
            }
            set {
                this["BendDownLayer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Magenta")]
        public global::System.Drawing.Color BendDownLayerColor {
            get {
                return ((global::System.Drawing.Color)(this["BendDownLayerColor"]));
            }
            set {
                this["BendDownLayerColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("kDashedLineType")]
        public global::Inventor.LineTypeEnum BendDownLineType {
            get {
                return ((global::Inventor.LineTypeEnum)(this["BendDownLineType"]));
            }
            set {
                this["BendDownLineType"] = value;
            }
        }
    }
}
