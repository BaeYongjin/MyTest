﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.34209
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdManagerClient.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:8086/AdManagerWebserviceV7/Schedule/SchTargetHomeAdService.asmx")]
        public string AdManager_SchTargetHomeAdServicePloxy_SchTargetHomeAdService {
            get {
                return ((string)(this["AdManager_SchTargetHomeAdServicePloxy_SchTargetHomeAdService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:8086/AdManagerWebserviceV7/Target/TargetingService.asmx")]
        public string AdManager_TargetingServicePloxy_TargetingService {
            get {
                return ((string)(this["AdManager_TargetingServicePloxy_TargetingService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:8086/AdManagerWebserviceV7/Media/MenuMapService.asmx")]
        public string AdManager_MenuMapServiceProxy_MenuMapService {
            get {
                return ((string)(this["AdManager_MenuMapServiceProxy_MenuMapService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:8086/AdManagerWebserviceV7/ReportSummaryAd/BizManageService.asmx" +
            "")]
        public string AdManager_BizManageServiceProxy_BizManageService {
            get {
                return ((string)(this["AdManager_BizManageServiceProxy_BizManageService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("나눔고딕, 8.5pt")]
        public global::System.Drawing.Font Font1 {
            get {
                return ((global::System.Drawing.Font)(this["Font1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:8086/AdManagerWebserviceV7/Schedule/SchRecommService.asmx")]
        public string AdManager_SchRecommServiceProxy_SchRecommService {
            get {
                return ((string)(this["AdManager_SchRecommServiceProxy_SchRecommService"]));
            }
        }
    }
}
