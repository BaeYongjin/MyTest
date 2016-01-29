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

namespace AdManagerClient.OAPWeekSummaryRptServicePloxy {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="OAPWeekSummaryRptServiceSoap", Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseModel))]
    public partial class OAPWeekSummaryRptService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetOAPWeekHomeAdOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetOAPWeekChannelJumpOperationCompleted;
        
        private System.Threading.SendOrPostCallback mGetHomeCmReportOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public OAPWeekSummaryRptService() {
            this.Url = "http://localhost:8086/AdManagerWebserviceV5/ReportSummaryAd/OAPWeekSummaryRptServ" +
                "ice.asmx";
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
        public event GetOAPWeekHomeAdCompletedEventHandler GetOAPWeekHomeAdCompleted;
        
        /// <remarks/>
        public event GetOAPWeekChannelJumpCompletedEventHandler GetOAPWeekChannelJumpCompleted;
        
        /// <remarks/>
        public event mGetHomeCmReportCompletedEventHandler mGetHomeCmReportCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/GetOAPWeekHomeAd", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public OAPWeekSummaryRptModel GetOAPWeekHomeAd(HeaderModel header, OAPWeekSummaryRptModel model) {
            object[] results = this.Invoke("GetOAPWeekHomeAd", new object[] {
                        header,
                        model});
            return ((OAPWeekSummaryRptModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetOAPWeekHomeAd(HeaderModel header, OAPWeekSummaryRptModel model, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetOAPWeekHomeAd", new object[] {
                        header,
                        model}, callback, asyncState);
        }
        
        /// <remarks/>
        public OAPWeekSummaryRptModel EndGetOAPWeekHomeAd(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((OAPWeekSummaryRptModel)(results[0]));
        }
        
        /// <remarks/>
        public void GetOAPWeekHomeAdAsync(HeaderModel header, OAPWeekSummaryRptModel model) {
            this.GetOAPWeekHomeAdAsync(header, model, null);
        }
        
        /// <remarks/>
        public void GetOAPWeekHomeAdAsync(HeaderModel header, OAPWeekSummaryRptModel model, object userState) {
            if ((this.GetOAPWeekHomeAdOperationCompleted == null)) {
                this.GetOAPWeekHomeAdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetOAPWeekHomeAdOperationCompleted);
            }
            this.InvokeAsync("GetOAPWeekHomeAd", new object[] {
                        header,
                        model}, this.GetOAPWeekHomeAdOperationCompleted, userState);
        }
        
        private void OnGetOAPWeekHomeAdOperationCompleted(object arg) {
            if ((this.GetOAPWeekHomeAdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetOAPWeekHomeAdCompleted(this, new GetOAPWeekHomeAdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/GetOAPWeekChannelJump", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public OAPWeekSummaryRptModel GetOAPWeekChannelJump(HeaderModel header, OAPWeekSummaryRptModel model) {
            object[] results = this.Invoke("GetOAPWeekChannelJump", new object[] {
                        header,
                        model});
            return ((OAPWeekSummaryRptModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetOAPWeekChannelJump(HeaderModel header, OAPWeekSummaryRptModel model, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetOAPWeekChannelJump", new object[] {
                        header,
                        model}, callback, asyncState);
        }
        
        /// <remarks/>
        public OAPWeekSummaryRptModel EndGetOAPWeekChannelJump(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((OAPWeekSummaryRptModel)(results[0]));
        }
        
        /// <remarks/>
        public void GetOAPWeekChannelJumpAsync(HeaderModel header, OAPWeekSummaryRptModel model) {
            this.GetOAPWeekChannelJumpAsync(header, model, null);
        }
        
        /// <remarks/>
        public void GetOAPWeekChannelJumpAsync(HeaderModel header, OAPWeekSummaryRptModel model, object userState) {
            if ((this.GetOAPWeekChannelJumpOperationCompleted == null)) {
                this.GetOAPWeekChannelJumpOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetOAPWeekChannelJumpOperationCompleted);
            }
            this.InvokeAsync("GetOAPWeekChannelJump", new object[] {
                        header,
                        model}, this.GetOAPWeekChannelJumpOperationCompleted, userState);
        }
        
        private void OnGetOAPWeekChannelJumpOperationCompleted(object arg) {
            if ((this.GetOAPWeekChannelJumpCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetOAPWeekChannelJumpCompleted(this, new GetOAPWeekChannelJumpCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/mGetHomeCmReport", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet mGetHomeCmReport(string beginDay, string endDay, string mediaRep) {
            object[] results = this.Invoke("mGetHomeCmReport", new object[] {
                        beginDay,
                        endDay,
                        mediaRep});
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginmGetHomeCmReport(string beginDay, string endDay, string mediaRep, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("mGetHomeCmReport", new object[] {
                        beginDay,
                        endDay,
                        mediaRep}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Data.DataSet EndmGetHomeCmReport(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void mGetHomeCmReportAsync(string beginDay, string endDay, string mediaRep) {
            this.mGetHomeCmReportAsync(beginDay, endDay, mediaRep, null);
        }
        
        /// <remarks/>
        public void mGetHomeCmReportAsync(string beginDay, string endDay, string mediaRep, object userState) {
            if ((this.mGetHomeCmReportOperationCompleted == null)) {
                this.mGetHomeCmReportOperationCompleted = new System.Threading.SendOrPostCallback(this.OnmGetHomeCmReportOperationCompleted);
            }
            this.InvokeAsync("mGetHomeCmReport", new object[] {
                        beginDay,
                        endDay,
                        mediaRep}, this.mGetHomeCmReportOperationCompleted, userState);
        }
        
        private void OnmGetHomeCmReportOperationCompleted(object arg) {
            if ((this.mGetHomeCmReportCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.mGetHomeCmReportCompleted(this, new mGetHomeCmReportCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OAPWeekSummaryRptModel))]
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
    public partial class OAPWeekSummaryRptModel : BaseModel {
        
        private System.Data.DataSet reportDataSetField;
        
        private System.Data.DataSet itemDataSetField;
        
        private string logDay1Field;
        
        private string logDay2Field;
        
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
        public System.Data.DataSet ItemDataSet {
            get {
                return this.itemDataSetField;
            }
            set {
                this.itemDataSetField = value;
            }
        }
        
        /// <remarks/>
        public string LogDay1 {
            get {
                return this.logDay1Field;
            }
            set {
                this.logDay1Field = value;
            }
        }
        
        /// <remarks/>
        public string LogDay2 {
            get {
                return this.logDay2Field;
            }
            set {
                this.logDay2Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetOAPWeekHomeAdCompletedEventHandler(object sender, GetOAPWeekHomeAdCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetOAPWeekHomeAdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetOAPWeekHomeAdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public OAPWeekSummaryRptModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((OAPWeekSummaryRptModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetOAPWeekChannelJumpCompletedEventHandler(object sender, GetOAPWeekChannelJumpCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetOAPWeekChannelJumpCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetOAPWeekChannelJumpCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public OAPWeekSummaryRptModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((OAPWeekSummaryRptModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void mGetHomeCmReportCompletedEventHandler(object sender, mGetHomeCmReportCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class mGetHomeCmReportCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal mGetHomeCmReportCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591