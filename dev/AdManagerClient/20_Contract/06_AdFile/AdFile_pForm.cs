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
	/// AdFile_pForm에 대한 요약 설명입니다.
	/// </summary>
	public class AdFile_pForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
		private Janus.Windows.EditControls.UIComboBox cbSearchRap;
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Data.DataView dvFile;
		private AdManagerClient._20_Contract._06_AdFile.AdFile_pDs adFile_pDs;
		private Janus.Windows.GridEX.GridEX grdExAdFileSearch;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;

		private AdFileControl parentCtl = null;

		public AdFile_pForm()
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
		public AdFile_pForm(AdFileControl sender)
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

		#region 이벤트핸들러
		public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러

		#endregion

		#region 사용자정의 객체 및 변수

		// 시스템 정보 : 화면공통
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;

		// 사용할 정보모델
		AdFileModel adFileModel  = new AdFileModel();	// 광고파일모델
		        
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
			Janus.Windows.GridEX.GridEXLayout grdExAdFileSearch_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdFile_pForm));
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
			this.grdExAdFileSearch = new Janus.Windows.GridEX.GridEX();
			this.dvFile = new System.Data.DataView();
			this.adFile_pDs = new AdManagerClient._20_Contract._06_AdFile.AdFile_pDs();
			this.pnlBtn = new System.Windows.Forms.Panel();
			this.btnClose = new Janus.Windows.EditControls.UIButton();
			this.btnOk = new Janus.Windows.EditControls.UIButton();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExAdFileSearch)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvFile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.adFile_pDs)).BeginInit();
			this.pnlBtn.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.cbSearchMedia);
			this.pnlSearch.Controls.Add(this.cbSearchRap);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(712, 40);
			this.pnlSearch.TabIndex = 0;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Location = new System.Drawing.Point(598, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 4;
			this.btnSearch.Text = "조 회";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(264, 8);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(224, 21);
			this.ebSearchKey.TabIndex = 3;
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
			// grdExAdFileSearch
			// 
			this.grdExAdFileSearch.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExAdFileSearch.AlternatingColors = true;
			this.grdExAdFileSearch.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExAdFileSearch.DataSource = this.dvFile;
			grdExAdFileSearch_DesignTimeLayout.LayoutString = resources.GetString("grdExAdFileSearch_DesignTimeLayout.LayoutString");
			this.grdExAdFileSearch.DesignTimeLayout = grdExAdFileSearch_DesignTimeLayout;
			this.grdExAdFileSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExAdFileSearch.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExAdFileSearch.EmptyRows = true;
			this.grdExAdFileSearch.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExAdFileSearch.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExAdFileSearch.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExAdFileSearch.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.grdExAdFileSearch.GridLineColor = System.Drawing.Color.Silver;
			this.grdExAdFileSearch.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExAdFileSearch.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExAdFileSearch.GroupByBoxVisible = false;
			this.grdExAdFileSearch.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExAdFileSearch.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExAdFileSearch.Location = new System.Drawing.Point(0, 40);
			this.grdExAdFileSearch.Name = "grdExAdFileSearch";
			this.grdExAdFileSearch.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExAdFileSearch.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExAdFileSearch.Size = new System.Drawing.Size(712, 262);
			this.grdExAdFileSearch.TabIndex = 5;
			this.grdExAdFileSearch.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExAdFileSearch.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExAdFileSearch.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExAdFileSearch.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.grdExAdFileSearch_RowDoubleClick);
			// 
			// dvFile
			// 
			this.dvFile.Table = this.adFile_pDs.AdFile;
			// 
			// adFile_pDs
			// 
			this.adFile_pDs.DataSetName = "AdFile_pDs";
			this.adFile_pDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.adFile_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// pnlBtn
			// 
			this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
			this.pnlBtn.Controls.Add(this.btnClose);
			this.pnlBtn.Controls.Add(this.btnOk);
			this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtn.Location = new System.Drawing.Point(0, 260);
			this.pnlBtn.Name = "pnlBtn";
			this.pnlBtn.Size = new System.Drawing.Size(712, 42);
			this.pnlBtn.TabIndex = 19;
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.SystemColors.Control;
			this.btnClose.ImageIndex = 1;
			this.btnClose.Location = new System.Drawing.Point(352, 9);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(70, 23);
			this.btnClose.TabIndex = 7;
			this.btnClose.Text = "닫기";
			this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.SystemColors.Control;
			this.btnOk.ImageIndex = 0;
			this.btnOk.Location = new System.Drawing.Point(264, 9);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(70, 23);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "확인";
			this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// AdFile_pForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(712, 302);
			this.Controls.Add(this.pnlBtn);
			this.Controls.Add(this.grdExAdFileSearch);
			this.Controls.Add(this.pnlSearch);
			this.Name = "AdFile_pForm";
			this.Text = "등록파일검색";
			this.Load += new System.EventHandler(this.UserControl_Load);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExAdFileSearch)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvFile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.adFile_pDs)).EndInit();
			this.pnlBtn.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// 데이터관리용 객체생성
			dt = ((DataView)grdExAdFileSearch.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExAdFileSearch.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// 컨트롤 초기화
			InitControl();	
		}

		#endregion

		#region 컨트롤 초기화
		private void InitControl()
		{
			InitCombo();

			SearchFile();

		}

		private void InitCombo()
		{
			Init_MediaCode();
			Init_RapCode();			
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
				Utility.SetDataTable(adFile_pDs.Medias, mediaCodeModel.MediaCodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbSearchMedia.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택","00");
			
			for(int i=0;i<mediaCodeModel.ResultCnt;i++)
			{
				DataRow row = adFile_pDs.Medias.Rows[i];

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
				Utility.SetDataTable(adFile_pDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbSearchRap.Items.Clear();
           
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택","00");
			
			for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
			{
				DataRow row = adFile_pDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchRap.Items.AddRange(comboItems);
			this.cbSearchRap.SelectedIndex = 0;

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
				for(int i=0;i < adFile_pDs.Medias.Rows.Count;i++)
				{
					DataRow row = adFile_pDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMedia.SelectedValue = FrameSystem._HANATV; // 하나TV를 기본값으로 한다.	 		
						break;															
					}
					else
					{
						cbSearchMedia.SelectedValue="00";
					}
				}	
			}
			if(commonModel.UserLevel=="30")
			{
				cbSearchRap.SelectedValue = commonModel.RapCode;			
				cbSearchRap.ReadOnly = true;				
			}
			
			Application.DoEvents();		
		}

		#endregion

		#region 파일검색 액션처리 메소드

		/// <summary>
		/// 그리드의 Row변경시
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
			if(grdExAdFileSearch.RowCount > 0)
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
			SearchFile();
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
				SearchFile();
			}
		}

		#endregion

		#region 처리메소드

		/// <summary>
		/// 매체대행사목록 조회
		/// </summary>
		private void SearchFile()
		{
			StatusMessage("매체대행사 정보를 조회합니다.");

			try
			{
				adFileModel.Init();
				// 데이터모델에 전송할 내용을 셋트한다.
				if(IsNewSearchKey)
				{
					adFileModel.SearchKey = "";
				}
				else
				{
					adFileModel.SearchKey  = ebSearchKey.Text;
				}

				adFileModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 
				adFileModel.SearchRapCode		 =  cbSearchRap.SelectedItem.Value.ToString();     
				
				// 매체대행사목록조회 서비스를 호출한다.
				new AdFileManager(systemModel,commonModel).GetAdFileSearch(adFileModel);

				if (adFileModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(adFile_pDs.AdFile, adFileModel.AdFileDataSet);				
					StatusMessage(adFileModel.ResultCnt + "건의 매체대행사 정보가 조회되었습니다.");

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
			adFileModel.newItemNo = dt.Rows[grdExAdFileSearch.GetRow().RowIndex]["ItemNo"].ToString();
			adFileModel.FileName = dt.Rows[grdExAdFileSearch.GetRow().RowIndex]["FileName"].ToString();
			adFileModel.FileState = dt.Rows[grdExAdFileSearch.GetRow().RowIndex]["FileState"].ToString();
			adFileModel.Flag = "Y";
			
			parentCtl.adOn_AdFile(adFileModel);
			this.Close();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdExAdFileSearch_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{
			//코드 넘겨줌
			adFileModel.newItemNo = dt.Rows[grdExAdFileSearch.GetRow().RowIndex]["ItemNo"].ToString();
			adFileModel.FileName = dt.Rows[grdExAdFileSearch.GetRow().RowIndex]["FileName"].ToString();
			adFileModel.FileState = dt.Rows[grdExAdFileSearch.GetRow().RowIndex]["FileState"].ToString();
			adFileModel.Flag = "Y";
			
			parentCtl.adOn_AdFile(adFileModel);
			this.Close();
		}
	}
}
