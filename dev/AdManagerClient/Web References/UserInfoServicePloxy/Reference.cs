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

namespace AdManagerClient.UserInfoServicePloxy {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="UserInfoServiceSoap", Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseModel))]
    public partial class UserInfoService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetUsersListOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetUserUpdateOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetUserCreateOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetUserDeleteOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public UserInfoService() {
            this.Url = "http://localhost:8086/AdManagerWebserviceV5/Media/UserInfoService.asmx";
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
        public event GetUsersListCompletedEventHandler GetUsersListCompleted;
        
        /// <remarks/>
        public event SetUserUpdateCompletedEventHandler SetUserUpdateCompleted;
        
        /// <remarks/>
        public event SetUserCreateCompletedEventHandler SetUserCreateCompleted;
        
        /// <remarks/>
        public event SetUserDeleteCompletedEventHandler SetUserDeleteCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/GetUsersList", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public UserInfoModel GetUsersList(HeaderModel header, UserInfoModel usersModel) {
            object[] results = this.Invoke("GetUsersList", new object[] {
                        header,
                        usersModel});
            return ((UserInfoModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetUsersList(HeaderModel header, UserInfoModel usersModel, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetUsersList", new object[] {
                        header,
                        usersModel}, callback, asyncState);
        }
        
        /// <remarks/>
        public UserInfoModel EndGetUsersList(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((UserInfoModel)(results[0]));
        }
        
        /// <remarks/>
        public void GetUsersListAsync(HeaderModel header, UserInfoModel usersModel) {
            this.GetUsersListAsync(header, usersModel, null);
        }
        
        /// <remarks/>
        public void GetUsersListAsync(HeaderModel header, UserInfoModel usersModel, object userState) {
            if ((this.GetUsersListOperationCompleted == null)) {
                this.GetUsersListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetUsersListOperationCompleted);
            }
            this.InvokeAsync("GetUsersList", new object[] {
                        header,
                        usersModel}, this.GetUsersListOperationCompleted, userState);
        }
        
        private void OnGetUsersListOperationCompleted(object arg) {
            if ((this.GetUsersListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetUsersListCompleted(this, new GetUsersListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/SetUserUpdate", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public UserInfoModel SetUserUpdate(HeaderModel header, UserInfoModel usersModel) {
            object[] results = this.Invoke("SetUserUpdate", new object[] {
                        header,
                        usersModel});
            return ((UserInfoModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSetUserUpdate(HeaderModel header, UserInfoModel usersModel, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SetUserUpdate", new object[] {
                        header,
                        usersModel}, callback, asyncState);
        }
        
        /// <remarks/>
        public UserInfoModel EndSetUserUpdate(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((UserInfoModel)(results[0]));
        }
        
        /// <remarks/>
        public void SetUserUpdateAsync(HeaderModel header, UserInfoModel usersModel) {
            this.SetUserUpdateAsync(header, usersModel, null);
        }
        
        /// <remarks/>
        public void SetUserUpdateAsync(HeaderModel header, UserInfoModel usersModel, object userState) {
            if ((this.SetUserUpdateOperationCompleted == null)) {
                this.SetUserUpdateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetUserUpdateOperationCompleted);
            }
            this.InvokeAsync("SetUserUpdate", new object[] {
                        header,
                        usersModel}, this.SetUserUpdateOperationCompleted, userState);
        }
        
        private void OnSetUserUpdateOperationCompleted(object arg) {
            if ((this.SetUserUpdateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetUserUpdateCompleted(this, new SetUserUpdateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/SetUserCreate", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public UserInfoModel SetUserCreate(HeaderModel header, UserInfoModel usersModel) {
            object[] results = this.Invoke("SetUserCreate", new object[] {
                        header,
                        usersModel});
            return ((UserInfoModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSetUserCreate(HeaderModel header, UserInfoModel usersModel, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SetUserCreate", new object[] {
                        header,
                        usersModel}, callback, asyncState);
        }
        
        /// <remarks/>
        public UserInfoModel EndSetUserCreate(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((UserInfoModel)(results[0]));
        }
        
        /// <remarks/>
        public void SetUserCreateAsync(HeaderModel header, UserInfoModel usersModel) {
            this.SetUserCreateAsync(header, usersModel, null);
        }
        
        /// <remarks/>
        public void SetUserCreateAsync(HeaderModel header, UserInfoModel usersModel, object userState) {
            if ((this.SetUserCreateOperationCompleted == null)) {
                this.SetUserCreateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetUserCreateOperationCompleted);
            }
            this.InvokeAsync("SetUserCreate", new object[] {
                        header,
                        usersModel}, this.SetUserCreateOperationCompleted, userState);
        }
        
        private void OnSetUserCreateOperationCompleted(object arg) {
            if ((this.SetUserCreateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetUserCreateCompleted(this, new SetUserCreateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://advmgt.hanafostv.com/AdManagerWebService/SetUserDelete", RequestNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", ResponseNamespace="http://advmgt.hanafostv.com/AdManagerWebService/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public UserInfoModel SetUserDelete(HeaderModel header, UserInfoModel usersModel) {
            object[] results = this.Invoke("SetUserDelete", new object[] {
                        header,
                        usersModel});
            return ((UserInfoModel)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSetUserDelete(HeaderModel header, UserInfoModel usersModel, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SetUserDelete", new object[] {
                        header,
                        usersModel}, callback, asyncState);
        }
        
        /// <remarks/>
        public UserInfoModel EndSetUserDelete(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((UserInfoModel)(results[0]));
        }
        
        /// <remarks/>
        public void SetUserDeleteAsync(HeaderModel header, UserInfoModel usersModel) {
            this.SetUserDeleteAsync(header, usersModel, null);
        }
        
        /// <remarks/>
        public void SetUserDeleteAsync(HeaderModel header, UserInfoModel usersModel, object userState) {
            if ((this.SetUserDeleteOperationCompleted == null)) {
                this.SetUserDeleteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetUserDeleteOperationCompleted);
            }
            this.InvokeAsync("SetUserDelete", new object[] {
                        header,
                        usersModel}, this.SetUserDeleteOperationCompleted, userState);
        }
        
        private void OnSetUserDeleteOperationCompleted(object arg) {
            if ((this.SetUserDeleteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetUserDeleteCompleted(this, new SetUserDeleteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UserInfoModel))]
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
    public partial class UserInfoModel : BaseModel {
        
        private System.Data.DataSet userDataSetField;
        
        private string searchKeyField;
        
        private string searchUserLevelField;
        
        private string searchUserClassField;
        
        private string searchchkAdState_10Field;
        
        private string userIDField;
        
        private string userNameField;
        
        private string userPasswordField;
        
        private string userLevelField;
        
        private string userClassField;
        
        private string mediaCodeField;
        
        private string rapCodeField;
        
        private string agencyCodeField;
        
        private string userLevelNameField;
        
        private string userDeptField;
        
        private string userTitleField;
        
        private string userTellField;
        
        private string userMobileField;
        
        private string userEMailField;
        
        private string lastLoginField;
        
        private string regDtField;
        
        private string userCommentField;
        
        private string useYnField;
        
        /// <remarks/>
        public System.Data.DataSet UserDataSet {
            get {
                return this.userDataSetField;
            }
            set {
                this.userDataSetField = value;
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
        public string SearchUserLevel {
            get {
                return this.searchUserLevelField;
            }
            set {
                this.searchUserLevelField = value;
            }
        }
        
        /// <remarks/>
        public string SearchUserClass {
            get {
                return this.searchUserClassField;
            }
            set {
                this.searchUserClassField = value;
            }
        }
        
        /// <remarks/>
        public string SearchchkAdState_10 {
            get {
                return this.searchchkAdState_10Field;
            }
            set {
                this.searchchkAdState_10Field = value;
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
        public string UserName {
            get {
                return this.userNameField;
            }
            set {
                this.userNameField = value;
            }
        }
        
        /// <remarks/>
        public string UserPassword {
            get {
                return this.userPasswordField;
            }
            set {
                this.userPasswordField = value;
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
        public string MediaCode {
            get {
                return this.mediaCodeField;
            }
            set {
                this.mediaCodeField = value;
            }
        }
        
        /// <remarks/>
        public string RapCode {
            get {
                return this.rapCodeField;
            }
            set {
                this.rapCodeField = value;
            }
        }
        
        /// <remarks/>
        public string AgencyCode {
            get {
                return this.agencyCodeField;
            }
            set {
                this.agencyCodeField = value;
            }
        }
        
        /// <remarks/>
        public string UserLevelName {
            get {
                return this.userLevelNameField;
            }
            set {
                this.userLevelNameField = value;
            }
        }
        
        /// <remarks/>
        public string UserDept {
            get {
                return this.userDeptField;
            }
            set {
                this.userDeptField = value;
            }
        }
        
        /// <remarks/>
        public string UserTitle {
            get {
                return this.userTitleField;
            }
            set {
                this.userTitleField = value;
            }
        }
        
        /// <remarks/>
        public string UserTell {
            get {
                return this.userTellField;
            }
            set {
                this.userTellField = value;
            }
        }
        
        /// <remarks/>
        public string UserMobile {
            get {
                return this.userMobileField;
            }
            set {
                this.userMobileField = value;
            }
        }
        
        /// <remarks/>
        public string UserEMail {
            get {
                return this.userEMailField;
            }
            set {
                this.userEMailField = value;
            }
        }
        
        /// <remarks/>
        public string LastLogin {
            get {
                return this.lastLoginField;
            }
            set {
                this.lastLoginField = value;
            }
        }
        
        /// <remarks/>
        public string RegDt {
            get {
                return this.regDtField;
            }
            set {
                this.regDtField = value;
            }
        }
        
        /// <remarks/>
        public string UserComment {
            get {
                return this.userCommentField;
            }
            set {
                this.userCommentField = value;
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
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void GetUsersListCompletedEventHandler(object sender, GetUsersListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetUsersListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetUsersListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public UserInfoModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((UserInfoModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void SetUserUpdateCompletedEventHandler(object sender, SetUserUpdateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetUserUpdateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SetUserUpdateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public UserInfoModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((UserInfoModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void SetUserCreateCompletedEventHandler(object sender, SetUserCreateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetUserCreateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SetUserCreateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public UserInfoModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((UserInfoModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void SetUserDeleteCompletedEventHandler(object sender, SetUserDeleteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetUserDeleteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SetUserDeleteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public UserInfoModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((UserInfoModel)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591