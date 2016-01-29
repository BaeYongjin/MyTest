﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.34209
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 이 소스 코드가 Microsoft.VSDesigner, 버전 4.0.30319.34209에서 자동으로 생성되었습니다.
// 
#pragma warning disable 1591

namespace AdManagerClient.ChannelServicePloxy {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ChannelServiceSoap", Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseModel))]
    public partial class ChannelService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetChannelListOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetChannelDetailListOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetChannelSetDetailListOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetChannelUpdateOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetChannelCreateOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetChannelDeleteOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ChannelService() {
            this.Url = "http://localhost:8086/AdManagerWebservice/Media/ChannelService.asmx";
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
        public event GetChannelListCompletedEventHandler GetChannelListCompleted;
        
        /// <remarks/>
        public event GetChannelDetailListCompletedEventHandler GetChannelDetailListCompleted;
        
        /// <remarks/>
        public event GetChannelSetDetailListCompletedEventHandler GetChannelSetDetailListCompleted;
        
        /// <remarks/>
        public event SetChannelUpdateCompletedEventHandler SetChannelUpdateCompleted;
        
        /// <remarks/>
        public event SetChannelCreateCompletedEventHandler SetChannelCreateCompleted;
        
        /// <remarks/>
        public event SetChannelDeleteCompletedEventHandler SetChannelDeleteCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/GetChannelList", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ChannelModel GetChannelList(HeaderModel header, ChannelModel channelModel) {
            object[] results = this.Invoke("GetChannelList", new object[] {
                        header,
                        channelModel});
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetChannelList(HeaderModel header, ChannelModel channelModel, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetChannelList", new object[] {
                        header,
                        channelModel}, callback, asyncState);
        }
        
        /// <remarks/>
        public ChannelModel EndGetChannelList(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public void GetChannelListAsync(HeaderModel header, ChannelModel channelModel) {
            this.GetChannelListAsync(header, channelModel, null);
        }
        
        /// <remarks/>
        public void GetChannelListAsync(HeaderModel header, ChannelModel channelModel, object userState) {
            if ((this.GetChannelListOperationCompleted == null)) {
                this.GetChannelListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetChannelListOperationCompleted);
            }
            this.InvokeAsync("GetChannelList", new object[] {
                        header,
                        channelModel}, this.GetChannelListOperationCompleted, userState);
        }
        
        private void OnGetChannelListOperationCompleted(object arg) {
            if ((this.GetChannelListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetChannelListCompleted(this, new GetChannelListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/GetChannelDetailList", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ChannelModel GetChannelDetailList(HeaderModel header, ChannelModel channelModel) {
            object[] results = this.Invoke("GetChannelDetailList", new object[] {
                        header,
                        channelModel});
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetChannelDetailList(HeaderModel header, ChannelModel channelModel, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetChannelDetailList", new object[] {
                        header,
                        channelModel}, callback, asyncState);
        }
        
        /// <remarks/>
        public ChannelModel EndGetChannelDetailList(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public void GetChannelDetailListAsync(HeaderModel header, ChannelModel channelModel) {
            this.GetChannelDetailListAsync(header, channelModel, null);
        }
        
        /// <remarks/>
        public void GetChannelDetailListAsync(HeaderModel header, ChannelModel channelModel, object userState) {
            if ((this.GetChannelDetailListOperationCompleted == null)) {
                this.GetChannelDetailListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetChannelDetailListOperationCompleted);
            }
            this.InvokeAsync("GetChannelDetailList", new object[] {
                        header,
                        channelModel}, this.GetChannelDetailListOperationCompleted, userState);
        }
        
        private void OnGetChannelDetailListOperationCompleted(object arg) {
            if ((this.GetChannelDetailListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetChannelDetailListCompleted(this, new GetChannelDetailListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/GetChannelSetDetailList", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ChannelModel GetChannelSetDetailList(HeaderModel header, ChannelModel channelModel) {
            object[] results = this.Invoke("GetChannelSetDetailList", new object[] {
                        header,
                        channelModel});
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetChannelSetDetailList(HeaderModel header, ChannelModel channelModel, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetChannelSetDetailList", new object[] {
                        header,
                        channelModel}, callback, asyncState);
        }
        
        /// <remarks/>
        public ChannelModel EndGetChannelSetDetailList(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public void GetChannelSetDetailListAsync(HeaderModel header, ChannelModel channelModel) {
            this.GetChannelSetDetailListAsync(header, channelModel, null);
        }
        
        /// <remarks/>
        public void GetChannelSetDetailListAsync(HeaderModel header, ChannelModel channelModel, object userState) {
            if ((this.GetChannelSetDetailListOperationCompleted == null)) {
                this.GetChannelSetDetailListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetChannelSetDetailListOperationCompleted);
            }
            this.InvokeAsync("GetChannelSetDetailList", new object[] {
                        header,
                        channelModel}, this.GetChannelSetDetailListOperationCompleted, userState);
        }
        
        private void OnGetChannelSetDetailListOperationCompleted(object arg) {
            if ((this.GetChannelSetDetailListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetChannelSetDetailListCompleted(this, new GetChannelSetDetailListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/SetChannelUpdate", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ChannelModel SetChannelUpdate(HeaderModel header, ChannelModel channelModel) {
            object[] results = this.Invoke("SetChannelUpdate", new object[] {
                        header,
                        channelModel});
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSetChannelUpdate(HeaderModel header, ChannelModel channelModel, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SetChannelUpdate", new object[] {
                        header,
                        channelModel}, callback, asyncState);
        }
        
        /// <remarks/>
        public ChannelModel EndSetChannelUpdate(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public void SetChannelUpdateAsync(HeaderModel header, ChannelModel channelModel) {
            this.SetChannelUpdateAsync(header, channelModel, null);
        }
        
        /// <remarks/>
        public void SetChannelUpdateAsync(HeaderModel header, ChannelModel channelModel, object userState) {
            if ((this.SetChannelUpdateOperationCompleted == null)) {
                this.SetChannelUpdateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetChannelUpdateOperationCompleted);
            }
            this.InvokeAsync("SetChannelUpdate", new object[] {
                        header,
                        channelModel}, this.SetChannelUpdateOperationCompleted, userState);
        }
        
        private void OnSetChannelUpdateOperationCompleted(object arg) {
            if ((this.SetChannelUpdateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetChannelUpdateCompleted(this, new SetChannelUpdateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/SetChannelCreate", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ChannelModel SetChannelCreate(HeaderModel header, ChannelModel channelModel) {
            object[] results = this.Invoke("SetChannelCreate", new object[] {
                        header,
                        channelModel});
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSetChannelCreate(HeaderModel header, ChannelModel channelModel, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SetChannelCreate", new object[] {
                        header,
                        channelModel}, callback, asyncState);
        }
        
        /// <remarks/>
        public ChannelModel EndSetChannelCreate(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public void SetChannelCreateAsync(HeaderModel header, ChannelModel channelModel) {
            this.SetChannelCreateAsync(header, channelModel, null);
        }
        
        /// <remarks/>
        public void SetChannelCreateAsync(HeaderModel header, ChannelModel channelModel, object userState) {
            if ((this.SetChannelCreateOperationCompleted == null)) {
                this.SetChannelCreateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetChannelCreateOperationCompleted);
            }
            this.InvokeAsync("SetChannelCreate", new object[] {
                        header,
                        channelModel}, this.SetChannelCreateOperationCompleted, userState);
        }
        
        private void OnSetChannelCreateOperationCompleted(object arg) {
            if ((this.SetChannelCreateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetChannelCreateCompleted(this, new SetChannelCreateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/SetChannelDelete", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ChannelModel SetChannelDelete(HeaderModel header, ChannelModel channelModel) {
            object[] results = this.Invoke("SetChannelDelete", new object[] {
                        header,
                        channelModel});
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSetChannelDelete(HeaderModel header, ChannelModel channelModel, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SetChannelDelete", new object[] {
                        header,
                        channelModel}, callback, asyncState);
        }
        
        /// <remarks/>
        public ChannelModel EndSetChannelDelete(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ChannelModel)(results[0]));
        }
        
        /// <remarks/>
        public void SetChannelDeleteAsync(HeaderModel header, ChannelModel channelModel) {
            this.SetChannelDeleteAsync(header, channelModel, null);
        }
        
        /// <remarks/>
        public void SetChannelDeleteAsync(HeaderModel header, ChannelModel channelModel, object userState) {
            if ((this.SetChannelDeleteOperationCompleted == null)) {
                this.SetChannelDeleteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetChannelDeleteOperationCompleted);
            }
            this.InvokeAsync("SetChannelDelete", new object[] {
                        header,
                        channelModel}, this.SetChannelDeleteOperationCompleted, userState);
        }
        
        private void OnSetChannelDeleteOperationCompleted(object arg) {
            if ((this.SetChannelDeleteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetChannelDeleteCompleted(this, new SetChannelDeleteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ChannelModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    public partial class ChannelModel : BaseModel {
        
        private System.Data.DataSet channelDataSetField;
        
        private System.Data.DataSet mediaDataSetField;
        
        private string searchKeyField;
        
        private string contentIdField;
        
        private string mediaCodeField;
        
        private string mediaNameField;
        
        private string channelNoField;
        
        private string titleField;
        
        private string seriesNoField;
        
        private string modDtField;
        
        private string totalSeriesField;
        
        private string checkYnField;
        
        private string serviceIDField;
        
        private string genreCodeField;
        
        private string channelNumberField;
        
        private string useYnField;
        
        private string adYnField;
        
        private string adRateField;
        
        private string adnRateField;
        
        /// <remarks/>
        public System.Data.DataSet ChannelDataSet {
            get {
                return this.channelDataSetField;
            }
            set {
                this.channelDataSetField = value;
            }
        }
        
        /// <remarks/>
        public System.Data.DataSet MediaDataSet {
            get {
                return this.mediaDataSetField;
            }
            set {
                this.mediaDataSetField = value;
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
        public string ContentId {
            get {
                return this.contentIdField;
            }
            set {
                this.contentIdField = value;
            }
        }
        
        /// <remarks/>
        public string MediaCode {
            get {
                return this.mediaCodeField;
            }
            set {
                this.mediaCodeField = value;
            }
        }
        
        /// <remarks/>
        public string MediaName {
            get {
                return this.mediaNameField;
            }
            set {
                this.mediaNameField = value;
            }
        }
        
        /// <remarks/>
        public string ChannelNo {
            get {
                return this.channelNoField;
            }
            set {
                this.channelNoField = value;
            }
        }
        
        /// <remarks/>
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        public string SeriesNo {
            get {
                return this.seriesNoField;
            }
            set {
                this.seriesNoField = value;
            }
        }
        
        /// <remarks/>
        public string ModDt {
            get {
                return this.modDtField;
            }
            set {
                this.modDtField = value;
            }
        }
        
        /// <remarks/>
        public string TotalSeries {
            get {
                return this.totalSeriesField;
            }
            set {
                this.totalSeriesField = value;
            }
        }
        
        /// <remarks/>
        public string CheckYn {
            get {
                return this.checkYnField;
            }
            set {
                this.checkYnField = value;
            }
        }
        
        /// <remarks/>
        public string ServiceID {
            get {
                return this.serviceIDField;
            }
            set {
                this.serviceIDField = value;
            }
        }
        
        /// <remarks/>
        public string GenreCode {
            get {
                return this.genreCodeField;
            }
            set {
                this.genreCodeField = value;
            }
        }
        
        /// <remarks/>
        public string ChannelNumber {
            get {
                return this.channelNumberField;
            }
            set {
                this.channelNumberField = value;
            }
        }
        
        /// <remarks/>
        public string UseYn {
            get {
                return this.useYnField;
            }
            set {
                this.useYnField = value;
            }
        }
        
        /// <remarks/>
        public string AdYn {
            get {
                return this.adYnField;
            }
            set {
                this.adYnField = value;
            }
        }
        
        /// <remarks/>
        public string AdRate {
            get {
                return this.adRateField;
            }
            set {
                this.adRateField = value;
            }
        }
        
        /// <remarks/>
        public string AdnRate {
            get {
                return this.adnRateField;
            }
            set {
                this.adnRateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void GetChannelListCompletedEventHandler(object sender, GetChannelListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetChannelListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetChannelListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ChannelModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ChannelModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void GetChannelDetailListCompletedEventHandler(object sender, GetChannelDetailListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetChannelDetailListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetChannelDetailListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ChannelModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ChannelModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void GetChannelSetDetailListCompletedEventHandler(object sender, GetChannelSetDetailListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetChannelSetDetailListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetChannelSetDetailListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ChannelModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ChannelModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SetChannelUpdateCompletedEventHandler(object sender, SetChannelUpdateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetChannelUpdateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SetChannelUpdateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ChannelModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ChannelModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SetChannelCreateCompletedEventHandler(object sender, SetChannelCreateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetChannelCreateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SetChannelCreateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ChannelModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ChannelModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void SetChannelDeleteCompletedEventHandler(object sender, SetChannelDeleteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetChannelDeleteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SetChannelDeleteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ChannelModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ChannelModel)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591