﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

namespace PairPicker.Properties
{


    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {

        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }


        [global::System.Configuration.ApplicationScopedSetting]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public IEnumerable<string> Users
        {
            get
            {
                return ((IEnumerable<string>)(this["Users"])) ?? new List<string>();
            }
            set
            {
                this["Users"] = value;
            }
        }
    }
}
