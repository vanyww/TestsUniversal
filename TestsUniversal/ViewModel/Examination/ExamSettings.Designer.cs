﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestsUniversal.ViewModel.Examination {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.8.0.0")]
    internal sealed partial class ExamSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static ExamSettings defaultInstance = ((ExamSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ExamSettings())));
        
        public static ExamSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".\\Data")]
        public string TasksPath {
            get {
                return ((string)(this["TasksPath"]));
            }
            set {
                this["TasksPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ChooseVariant {
            get {
                return ((bool)(this["ChooseVariant"]));
            }
            set {
                this["ChooseVariant"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int SignesNumber {
            get {
                return ((int)(this["SignesNumber"]));
            }
            set {
                this["SignesNumber"] = value;
            }
        }
    }
}