﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.18051
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 이 소스 코드가 Microsoft.VSDesigner, 버전 4.0.30319.18051에서 자동으로 생성되었습니다.
// 
#pragma warning disable 1591

namespace AdManagerClient.StatisticsTotalServicePloxy {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="StatisticsTotalServiceSoap", Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseModel))]
    public partial class StatisticsTotalService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetStatisticsTotalOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public StatisticsTotalService() {
            this.Url = "http://localhost:8086/AdManagerWebserviceV5/ReportAd/StatisticsTotalService.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetStatisticsTotalCompletedEventHandler GetStatisticsTotalCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/GetStatisticsTotal", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public StatisticsTotalModel GetStatisticsTotal(HeaderModel header, StatisticsTotalModel model) {
            object[] results = this.Invoke("GetStatisticsTotal", new object[] {
                        header,
                        model});
            return ((StatisticsTotalModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetStatisticsTotal(HeaderModel header, StatisticsTotalModel model, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetStatisticsTotal", new object[] {
                        header,
                        model}, callback, asyncState);
        }
        
        /// <remarks/>
        public StatisticsTotalModel EndGetStatisticsTotal(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((StatisticsTotalModel)(results[0]));
        }
        
        /// <remarks/>
        public void GetStatisticsTotalAsync(HeaderModel header, StatisticsTotalModel model) {
            this.GetStatisticsTotalAsync(header, model, null);
        }
        
        /// <remarks/>
        public void GetStatisticsTotalAsync(HeaderModel header, StatisticsTotalModel model, object userState) {
            if ((this.GetStatisticsTotalOperationCompleted == null)) {
                this.GetStatisticsTotalOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetStatisticsTotalOperationCompleted);
            }
            this.InvokeAsync("GetStatisticsTotal", new object[] {
                        header,
                        model}, this.GetStatisticsTotalOperationCompleted, userState);
        }
        
        private void OnGetStatisticsTotalOperationCompleted(object arg) {
            if ((this.GetStatisticsTotalCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetStatisticsTotalCompleted(this, new GetStatisticsTotalCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    public partial class HeaderModel {
        
        private string clientKeyField;
        
        private string sysVersionField;
        
        private string userIDField;
        
        private string userLevelField;
        
        private string userClassField;
        
        private string userGroupField;
        
        /// <remarks/>
        public string ClientKey {
            get {
                return this.clientKeyField;
            }
            set {
                this.clientKeyField = value;
            }
        }
        
        /// <remarks/>
        public string SysVersion {
            get {
                return this.sysVersionField;
            }
            set {
                this.sysVersionField = value;
            }
        }
        
        /// <remarks/>
        public string UserID {
            get {
                return this.userIDField;
            }
            set {
                this.userIDField = value;
            }
        }
        
        /// <remarks/>
        public string UserLevel {
            get {
                return this.userLevelField;
            }
            set {
                this.userLevelField = value;
            }
        }
        
        /// <remarks/>
        public string UserClass {
            get {
                return this.userClassField;
            }
            set {
                this.userClassField = value;
            }
        }
        
        /// <remarks/>
        public string UserGroup {
            get {
                return this.userGroupField;
            }
            set {
                this.userGroupField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StatisticsTotalModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    public partial class BaseModel {
        
        private string resultDescField;
        
        private int resultCntField;
        
        private string resultCDField;
        
        /// <remarks/>
        public string ResultDesc {
            get {
                return this.resultDescField;
            }
            set {
                this.resultDescField = value;
            }
        }
        
        /// <remarks/>
        public int ResultCnt {
            get {
                return this.resultCntField;
            }
            set {
                this.resultCntField = value;
            }
        }
        
        /// <remarks/>
        public string ResultCD {
            get {
                return this.resultCDField;
            }
            set {
                this.resultCDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    public partial class StatisticsTotalModel : BaseModel {
        
        private System.Data.DataSet reportDataSetField;
        
        private string searchMediaCodeField;
        
        private string searchRapCodeField;
        
        private string searchAgencyCodeField;
        
        private string searchKeyField;
        
        /// <remarks/>
        public System.Data.DataSet ReportDataSet {
            get {
                return this.reportDataSetField;
            }
            set {
                this.reportDataSetField = value;
            }
        }
        
        /// <remarks/>
        public string SearchMediaCode {
            get {
                return this.searchMediaCodeField;
            }
            set {
                this.searchMediaCodeField = value;
            }
        }
        
        /// <remarks/>
        public string SearchRapCode {
            get {
                return this.searchRapCodeField;
            }
            set {
                this.searchRapCodeField = value;
            }
        }
        
        /// <remarks/>
        public string SearchAgencyCode {
            get {
                return this.searchAgencyCodeField;
            }
            set {
                this.searchAgencyCodeField = value;
            }
        }
        
        /// <remarks/>
        public string SearchKey {
            get {
                return this.searchKeyField;
            }
            set {
                this.searchKeyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetStatisticsTotalCompletedEventHandler(object sender, GetStatisticsTotalCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetStatisticsTotalCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetStatisticsTotalCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public StatisticsTotalModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((StatisticsTotalModel)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591