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

namespace AdManagerClient.StatisticsPgRegionServicePloxy {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="StatisticsPgRegionServiceSoap", Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseModel))]
    public partial class StatisticsPgRegionService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetCategoryListOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetGenreListOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetStatisticsPgRegionOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetStatisticsPgRegionAVGOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public StatisticsPgRegionService() {
            this.Url = "http://localhost:8086/AdManagerWebserviceV5/ReportMedia/StatisticsPgRegionService" +
                ".asmx";
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
        public event GetCategoryListCompletedEventHandler GetCategoryListCompleted;
        
        /// <remarks/>
        public event GetGenreListCompletedEventHandler GetGenreListCompleted;
        
        /// <remarks/>
        public event GetStatisticsPgRegionCompletedEventHandler GetStatisticsPgRegionCompleted;
        
        /// <remarks/>
        public event GetStatisticsPgRegionAVGCompletedEventHandler GetStatisticsPgRegionAVGCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/GetCategoryList", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public StatisticsPgRegionModel GetCategoryList(HeaderModel header, StatisticsPgRegionModel model) {
            object[] results = this.Invoke("GetCategoryList", new object[] {
                        header,
                        model});
            return ((StatisticsPgRegionModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetCategoryList(HeaderModel header, StatisticsPgRegionModel model, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetCategoryList", new object[] {
                        header,
                        model}, callback, asyncState);
        }
        
        /// <remarks/>
        public StatisticsPgRegionModel EndGetCategoryList(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((StatisticsPgRegionModel)(results[0]));
        }
        
        /// <remarks/>
        public void GetCategoryListAsync(HeaderModel header, StatisticsPgRegionModel model) {
            this.GetCategoryListAsync(header, model, null);
        }
        
        /// <remarks/>
        public void GetCategoryListAsync(HeaderModel header, StatisticsPgRegionModel model, object userState) {
            if ((this.GetCategoryListOperationCompleted == null)) {
                this.GetCategoryListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetCategoryListOperationCompleted);
            }
            this.InvokeAsync("GetCategoryList", new object[] {
                        header,
                        model}, this.GetCategoryListOperationCompleted, userState);
        }
        
        private void OnGetCategoryListOperationCompleted(object arg) {
            if ((this.GetCategoryListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetCategoryListCompleted(this, new GetCategoryListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/GetGenreList", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public StatisticsPgRegionModel GetGenreList(HeaderModel header, StatisticsPgRegionModel model) {
            object[] results = this.Invoke("GetGenreList", new object[] {
                        header,
                        model});
            return ((StatisticsPgRegionModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetGenreList(HeaderModel header, StatisticsPgRegionModel model, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetGenreList", new object[] {
                        header,
                        model}, callback, asyncState);
        }
        
        /// <remarks/>
        public StatisticsPgRegionModel EndGetGenreList(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((StatisticsPgRegionModel)(results[0]));
        }
        
        /// <remarks/>
        public void GetGenreListAsync(HeaderModel header, StatisticsPgRegionModel model) {
            this.GetGenreListAsync(header, model, null);
        }
        
        /// <remarks/>
        public void GetGenreListAsync(HeaderModel header, StatisticsPgRegionModel model, object userState) {
            if ((this.GetGenreListOperationCompleted == null)) {
                this.GetGenreListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetGenreListOperationCompleted);
            }
            this.InvokeAsync("GetGenreList", new object[] {
                        header,
                        model}, this.GetGenreListOperationCompleted, userState);
        }
        
        private void OnGetGenreListOperationCompleted(object arg) {
            if ((this.GetGenreListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetGenreListCompleted(this, new GetGenreListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/GetStatisticsPgRegion", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public StatisticsPgRegionModel GetStatisticsPgRegion(HeaderModel header, StatisticsPgRegionModel model) {
            object[] results = this.Invoke("GetStatisticsPgRegion", new object[] {
                        header,
                        model});
            return ((StatisticsPgRegionModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetStatisticsPgRegion(HeaderModel header, StatisticsPgRegionModel model, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetStatisticsPgRegion", new object[] {
                        header,
                        model}, callback, asyncState);
        }
        
        /// <remarks/>
        public StatisticsPgRegionModel EndGetStatisticsPgRegion(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((StatisticsPgRegionModel)(results[0]));
        }
        
        /// <remarks/>
        public void GetStatisticsPgRegionAsync(HeaderModel header, StatisticsPgRegionModel model) {
            this.GetStatisticsPgRegionAsync(header, model, null);
        }
        
        /// <remarks/>
        public void GetStatisticsPgRegionAsync(HeaderModel header, StatisticsPgRegionModel model, object userState) {
            if ((this.GetStatisticsPgRegionOperationCompleted == null)) {
                this.GetStatisticsPgRegionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetStatisticsPgRegionOperationCompleted);
            }
            this.InvokeAsync("GetStatisticsPgRegion", new object[] {
                        header,
                        model}, this.GetStatisticsPgRegionOperationCompleted, userState);
        }
        
        private void OnGetStatisticsPgRegionOperationCompleted(object arg) {
            if ((this.GetStatisticsPgRegionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetStatisticsPgRegionCompleted(this, new GetStatisticsPgRegionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/GetStatisticsPgRegionAVG", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public StatisticsPgRegionModel GetStatisticsPgRegionAVG(HeaderModel header, StatisticsPgRegionModel model) {
            object[] results = this.Invoke("GetStatisticsPgRegionAVG", new object[] {
                        header,
                        model});
            return ((StatisticsPgRegionModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetStatisticsPgRegionAVG(HeaderModel header, StatisticsPgRegionModel model, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetStatisticsPgRegionAVG", new object[] {
                        header,
                        model}, callback, asyncState);
        }
        
        /// <remarks/>
        public StatisticsPgRegionModel EndGetStatisticsPgRegionAVG(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((StatisticsPgRegionModel)(results[0]));
        }
        
        /// <remarks/>
        public void GetStatisticsPgRegionAVGAsync(HeaderModel header, StatisticsPgRegionModel model) {
            this.GetStatisticsPgRegionAVGAsync(header, model, null);
        }
        
        /// <remarks/>
        public void GetStatisticsPgRegionAVGAsync(HeaderModel header, StatisticsPgRegionModel model, object userState) {
            if ((this.GetStatisticsPgRegionAVGOperationCompleted == null)) {
                this.GetStatisticsPgRegionAVGOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetStatisticsPgRegionAVGOperationCompleted);
            }
            this.InvokeAsync("GetStatisticsPgRegionAVG", new object[] {
                        header,
                        model}, this.GetStatisticsPgRegionAVGOperationCompleted, userState);
        }
        
        private void OnGetStatisticsPgRegionAVGOperationCompleted(object arg) {
            if ((this.GetStatisticsPgRegionAVGCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetStatisticsPgRegionAVGCompleted(this, new GetStatisticsPgRegionAVGCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StatisticsPgRegionModel))]
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
    public partial class StatisticsPgRegionModel : BaseModel {
        
        private System.Data.DataSet reportDataSetField;
        
        private System.Data.DataSet categoryDataSetField;
        
        private System.Data.DataSet genreDataSetField;
        
        private string searchMediaCodeField;
        
        private string searchCategoryCodeField;
        
        private string searchGenreCodeField;
        
        private string searchKeyField;
        
        private string searchTypeField;
        
        private string searchStartDayField;
        
        private string searchEndDayField;
        
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
        public System.Data.DataSet CategoryDataSet {
            get {
                return this.categoryDataSetField;
            }
            set {
                this.categoryDataSetField = value;
            }
        }
        
        /// <remarks/>
        public System.Data.DataSet GenreDataSet {
            get {
                return this.genreDataSetField;
            }
            set {
                this.genreDataSetField = value;
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
        public string SearchCategoryCode {
            get {
                return this.searchCategoryCodeField;
            }
            set {
                this.searchCategoryCodeField = value;
            }
        }
        
        /// <remarks/>
        public string SearchGenreCode {
            get {
                return this.searchGenreCodeField;
            }
            set {
                this.searchGenreCodeField = value;
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
        
        /// <remarks/>
        public string SearchType {
            get {
                return this.searchTypeField;
            }
            set {
                this.searchTypeField = value;
            }
        }
        
        /// <remarks/>
        public string SearchStartDay {
            get {
                return this.searchStartDayField;
            }
            set {
                this.searchStartDayField = value;
            }
        }
        
        /// <remarks/>
        public string SearchEndDay {
            get {
                return this.searchEndDayField;
            }
            set {
                this.searchEndDayField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetCategoryListCompletedEventHandler(object sender, GetCategoryListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetCategoryListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetCategoryListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public StatisticsPgRegionModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((StatisticsPgRegionModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetGenreListCompletedEventHandler(object sender, GetGenreListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetGenreListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetGenreListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public StatisticsPgRegionModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((StatisticsPgRegionModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetStatisticsPgRegionCompletedEventHandler(object sender, GetStatisticsPgRegionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetStatisticsPgRegionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetStatisticsPgRegionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public StatisticsPgRegionModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((StatisticsPgRegionModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetStatisticsPgRegionAVGCompletedEventHandler(object sender, GetStatisticsPgRegionAVGCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetStatisticsPgRegionAVGCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetStatisticsPgRegionAVGCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public StatisticsPgRegionModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((StatisticsPgRegionModel)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591