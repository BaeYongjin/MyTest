using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
    /// <summary>
    /// Client_pForm에 대한 요약 설명입니다.
    /// </summary>
    public class Client_pForm : System.Windows.Forms.Form
    {

        #region 화면 컴포넌트, 생성자, 소멸자
        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private Janus.Windows.EditControls.UIComboBox cbSearchAgency;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Windows.Forms.Panel panel1;
        private System.ComponentModel.IContainer components;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private AdManagerClient.Client_pDs client_pDs;
        private System.Data.DataView dvMediaAgency;
        private Janus.Windows.GridEX.GridEX grdExMediaAgencyList;
        
        private ClientControl parentCtl = null;

        public Client_pForm()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

            //
            
            //
        }
        /// <summary>
        /// 데이터 넘겨야 할 넘
        /// </summary>
        /// <param name="sender"></param>
        public Client_pForm(ClientControl sender)
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

            //
            
            //
            
            parentCtl = sender;
        }


        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }


        #endregion

        #region 이벤트핸들러
        public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러

        #endregion

        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        // 사용할 정보모델
        MediaAgencyModel mediaAgencyModel  = new MediaAgencyModel(); //대행사 모델
        // 사용할 정보모델
        ClientModel clientModel  = new ClientModel();	// 매체대행광고주정보모델
        
        // 화면처리용 변수
        bool IsNewSearchKey		  = true;					// 검색어입력 여부
        CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable       dt        = null;

        #endregion

        #region Windows Form 디자이너에서 생성한 코드
        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client_pForm));
            Janus.Windows.GridEX.GridEXLayout grdExMediaAgencyList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAgency = new Janus.Windows.EditControls.UIComboBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExMediaAgencyList = new Janus.Windows.GridEX.GridEX();
            this.dvMediaAgency = new System.Data.DataView();
            this.client_pDs = new AdManagerClient.Client_pDs();
            this.pnlSearch.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExMediaAgencyList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMediaAgency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.client_pDs)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.cbSearchAgency);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(504, 62);
            this.pnlSearch.TabIndex = 4;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(392, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(8, 32);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(248, 21);
            this.ebSearchKey.TabIndex = 6;
            this.ebSearchKey.Text = "검색어를 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(120, 21);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "매체선택";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(136, 8);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
            this.cbSearchRap.TabIndex = 2;
            this.cbSearchRap.Text = "랩사선택";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAgency
            // 
            this.cbSearchAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAgency.Location = new System.Drawing.Point(264, 8);
            this.cbSearchAgency.Name = "cbSearchAgency";
            this.cbSearchAgency.Size = new System.Drawing.Size(120, 21);
            this.cbSearchAgency.TabIndex = 3;
            this.cbSearchAgency.Text = "대행사선택";
            this.cbSearchAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 420);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(504, 42);
            this.pnlBtn.TabIndex = 17;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.ImageIndex = 1;
            this.btnClose.ImageList = this.imageList;
            this.btnClose.Location = new System.Drawing.Point(261, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "닫기";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.ImageIndex = 0;
            this.btnOk.ImageList = this.imageList;
            this.btnOk.Location = new System.Drawing.Point(173, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 23);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "확인";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.grdExMediaAgencyList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(504, 358);
            this.panel1.TabIndex = 18;
            // 
            // grdExMediaAgencyList
            // 
            this.grdExMediaAgencyList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExMediaAgencyList.AlternatingColors = true;
            this.grdExMediaAgencyList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExMediaAgencyList.DataSource = this.dvMediaAgency;
            grdExMediaAgencyList_DesignTimeLayout.LayoutString = resources.GetString("grdExMediaAgencyList_DesignTimeLayout.LayoutString");
            this.grdExMediaAgencyList.DesignTimeLayout = grdExMediaAgencyList_DesignTimeLayout;
            this.grdExMediaAgencyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExMediaAgencyList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExMediaAgencyList.EmptyRows = true;
            this.grdExMediaAgencyList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExMediaAgencyList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExMediaAgencyList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExMediaAgencyList.Font = new System.Drawing.Font("굴림체", 9F);
            this.grdExMediaAgencyList.FrozenColumns = 2;
            this.grdExMediaAgencyList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExMediaAgencyList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExMediaAgencyList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExMediaAgencyList.GroupByBoxVisible = false;
            this.grdExMediaAgencyList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExMediaAgencyList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExMediaAgencyList.Location = new System.Drawing.Point(0, 0);
            this.grdExMediaAgencyList.Name = "grdExMediaAgencyList";
            this.grdExMediaAgencyList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExMediaAgencyList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExMediaAgencyList.SelectedInactiveFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExMediaAgencyList.Size = new System.Drawing.Size(504, 358);
            this.grdExMediaAgencyList.TabIndex = 10;
            this.grdExMediaAgencyList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExMediaAgencyList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExMediaAgencyList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvMediaAgency
            // 
            this.dvMediaAgency.Table = this.client_pDs.MediaAgencys;
            // 
            // client_pDs
            // 
            this.client_pDs.DataSetName = "Client_pDs";
            this.client_pDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.client_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Client_pForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(504, 462);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.pnlSearch);
            this.Name = "Client_pForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "대행사목록";
            this.Load += new System.EventHandler(this.UserControl_Load);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExMediaAgencyList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMediaAgency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.client_pDs)).EndInit();
            this.ResumeLayout(false);

		}
        #endregion

        #region 컨트롤 로드
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // 데이터관리용 객체생성
            dt = ((DataView)grdExMediaAgencyList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExMediaAgencyList.DataSource]; 
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            // 컨트롤 초기화
            InitControl();	
        }

        #endregion

        #region 컨트롤 초기화
        private void InitControl()
        {
            InitCombo();

			SearchMediaAgency();

        }

        private void InitCombo()
        {
            Init_MediaCode();
            Init_RapCode();
            Init_AgencyCode();
            InitCombo_Level();

        }

        private void Init_MediaCode()
        {
            // 매체를 조회한다.
            MediaCodeModel mediaCodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediaCodeModel);
			
            if (mediaCodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(client_pDs.Medias, mediaCodeModel.MediaCodeDataSet);				
            }

            // 검색조건의 콤보
            this.cbSearchMedia.Items.Clear();
			
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택","00");
			
            for(int i=0;i<mediaCodeModel.ResultCnt;i++)
            {
                DataRow row = client_pDs.Medias.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // 검색 콤보에 셋트
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;

			Application.DoEvents();
        }

        private void Init_RapCode()
        {
            // 랩을 조회한다.
            MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
            if (mediaRapCodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(client_pDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
            }

            // 검색조건의 콤보
            this.cbSearchRap.Items.Clear();
           
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택","00");
			
            for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
            {
                DataRow row = client_pDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // 콤보에 셋트
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

			Application.DoEvents();

        }

        private void Init_AgencyCode()
        {
            // 대행사를 조회한다.
            AgencyCodeModel agencyCodeModel = new AgencyCodeModel();
            new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencyCodeModel);
			
            if (agencyCodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(client_pDs.Agencys, agencyCodeModel.AgencyCodeDataSet);				
            }

            // 검색조건의 콤보
            this.cbSearchAgency.Items.Clear();
			
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencyCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("대행사선택","00");
			
            for(int i=0;i<agencyCodeModel.ResultCnt;i++)
            {
                DataRow row = client_pDs.Agencys.Rows[i];

                string val = row["AgencyCode"].ToString();
                string txt = row["AgencyName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
			
            // 콤보에 셋트
            this.cbSearchAgency.Items.AddRange(comboItems);
            this.cbSearchAgency.SelectedIndex = 0;

			Application.DoEvents();
        }

        private void InitCombo_Level()
        {
			
            if(commonModel.UserLevel=="20")
            {
                cbSearchMedia.SelectedValue = commonModel.MediaCode;			
                cbSearchMedia.ReadOnly = true;					
            }
            else
            {
                //cbSearchMedia.SelectedValue = FrameSystem._HANATV; // 하나TV를 기본값으로 한다.
            }
            if(commonModel.UserLevel=="30")
            {
                cbSearchRap.SelectedValue = commonModel.RapCode;			
                cbSearchRap.ReadOnly = true;				
            }
            if(commonModel.UserLevel=="40")
            {
                cbSearchAgency.SelectedValue = commonModel.AgencyCode;			
                cbSearchAgency.ReadOnly = true;					
            }	
            Application.DoEvents();		
        }

        #endregion

        #region 계약정보 액션처리 메소드

        /// <summary>
        /// 그리드의 Row변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if(grdExMediaAgencyList.RowCount > 0)
            {
                //                SetDetailText();
                //                InitButton();
            }
        }

        /// <summary>
        /// 조회버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            SearchMediaAgency();
        }

        /// <summary>
        /// 검색어 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
        {
            IsNewSearchKey = false;
        }

        /// <summary>
        /// 검색어 클릭 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_Click(object sender, System.EventArgs e)
        {
            if(IsNewSearchKey)
            {
                ebSearchKey.Text = "";
            }
            else
            {
                ebSearchKey.SelectAll();
            }
        }

        private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                SearchMediaAgency();
            }
        }

        #endregion

        #region 처리메소드

        /// <summary>
        /// 매체대행사목록 조회
        /// </summary>
        private void SearchMediaAgency()
        {
            StatusMessage("매체대행사 정보를 조회합니다.");

            try
            {
                mediaAgencyModel.Init();
                // 데이터모델에 전송할 내용을 셋트한다.
                if(IsNewSearchKey)
                {
                    mediaAgencyModel.SearchKey = "";
                }
                else
                {
                    mediaAgencyModel.SearchKey  = ebSearchKey.Text;
                }

                mediaAgencyModel.SearchMediaName = cbSearchMedia.SelectedItem.Value.ToString();
                mediaAgencyModel.SearchRapName = cbSearchRap.SelectedItem.Value.ToString();
                mediaAgencyModel.SearchMediaAgency = cbSearchAgency.SelectedItem.Value.ToString();

                // 매체대행사목록조회 서비스를 호출한다.
                new MediaAgencyManager(systemModel,commonModel).GetMediaAgencyPop(mediaAgencyModel);

                if (mediaAgencyModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(client_pDs.MediaAgencys, mediaAgencyModel.MediaAgencyDataSet);				
                    StatusMessage(mediaAgencyModel.ResultCnt + "건의 매체대행사 정보가 조회되었습니다.");

                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("매체대행사조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("매체대행사조회오류",new string[] {"",ex.Message});
            }
        }

        #endregion

        #region 이벤트함수

        private void StatusMessage(string strMessage)
        {
            if (StatusEvent != null) 
            {
                StatusEventArgs ea = new StatusEventArgs();
                ea.Message   = strMessage;
                StatusEvent(this,ea);
            }
        }

        #endregion

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            //코드 넘겨줌
            clientModel.MediaCode = dt.Rows[grdExMediaAgencyList.GetRow().RowIndex]["MediaCode"].ToString();
            clientModel.RapCode = dt.Rows[grdExMediaAgencyList.GetRow().RowIndex]["RapCode"].ToString();
            clientModel.AgencyCode = dt.Rows[grdExMediaAgencyList.GetRow().RowIndex]["AgencyCode"].ToString();
            //이름 넘겨줌
            clientModel.MediaName = dt.Rows[grdExMediaAgencyList.GetRow().RowIndex]["MediaName"].ToString();
            clientModel.RapName = dt.Rows[grdExMediaAgencyList.GetRow().RowIndex]["RapName"].ToString();
            clientModel.AgencyName = dt.Rows[grdExMediaAgencyList.GetRow().RowIndex]["AgencyName"].ToString();

            parentCtl.adOn_AddContract(clientModel);
            this.Close();

        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }



    }
}