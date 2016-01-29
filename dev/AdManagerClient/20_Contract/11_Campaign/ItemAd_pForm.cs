// ===============================================================================
// SchHomeAd SearchForm  for Charites Project
//
// ItemAd_pForm.cs
//
// 홈광고 편성대상 조회. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Diagnostics;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// ItemAd_pForm에 대한 요약 설명입니다.
	/// </summary>
	/// 
	public class ItemAd_pForm : System.Windows.Forms.Form
	{
       
		#region 이벤트핸들러및 함수
		public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러

        private void StatusMessage(string strMessage)
        {
            if (StatusEvent != null)
            {
                StatusEventArgs ea = new StatusEventArgs();
                ea.Message = strMessage;
                StatusEvent(this, ea);
            }
        }
		#endregion
			
		#region 사용자정의 객체 및 변수

		// 시스템 정보 : 화면공통
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;

		// 사용할 정보모델
		//SchHomeAdModel schHomeAdModel  = new SchHomeAdModel();	// 홈광고편성모델
		CampaignModel campaignModel  = new CampaignModel();	// 홈광고편성모델

		// 화면처리용 변수
		bool IsNewSearchKey		  = true;					// 검색어입력 여부
		CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dt        = null;

		// 이 창을 연 컨트롤
		CampaignControl Opener = null;

		// 매체
		public string keyMediaCode = "";			// 팝업호출시 매체셋트
		public string keyCampaign = "";			// 팝업호출시 매체셋트
		public string keyContractSeq = "";			// 팝업호출시 매체셋트

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel4;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;
		private System.Data.DataView dvItems;
		private System.Windows.Forms.Label label13;
		private Janus.Windows.EditControls.UICheckBox chkAdState_10;
		private Janus.Windows.EditControls.UICheckBox chkAdState_40;
		private Janus.Windows.EditControls.UICheckBox chkAdState_30;
		private Janus.Windows.EditControls.UICheckBox chkAdState_20;
		private AdManagerClient._20_Contract._11_Campaign.CampaignDs campaign_pDs;
		private Janus.Windows.GridEX.GridEX grdExItemList;

		#endregion

		#region 생성자 및 소멸자
		public ItemAd_pForm(UserControl parent)
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();
           
			// 이창을 호출한 컨트롤
			Opener = (CampaignControl) parent;
			//schHomeAdModel.Init();
		}		
		public ItemAd_pForm()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//schHomeAdModel.Init();
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{

			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            Janus.Windows.GridEX.GridEXLayout grdExItemList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemAd_pForm));
            this.dvItems = new System.Data.DataView();
            this.campaign_pDs = new AdManagerClient._20_Contract._11_Campaign.CampaignDs();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExItemList = new Janus.Windows.GridEX.GridEX();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.chkAdState_10 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.dvItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaign_pDs)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).BeginInit();
            this.pnlBtn.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dvItems
            // 
            this.dvItems.Table = this.campaign_pDs.HomeAdItems;
            // 
            // campaign_pDs
            // 
            this.campaign_pDs.DataSetName = "CampaignDs";
            this.campaign_pDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.campaign_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdExItemList);
            this.panel1.Controls.Add(this.pnlBtn);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(792, 466);
            this.panel1.TabIndex = 17;
            // 
            // grdExItemList
            // 
            this.grdExItemList.AlternatingColors = true;
            this.grdExItemList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExItemList.DataSource = this.dvItems;
            this.grdExItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExItemList.EmptyRows = true;
            this.grdExItemList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExItemList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExItemList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExItemList.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.grdExItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExItemList.GroupByBoxVisible = false;
            this.grdExItemList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExItemList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExItemList_Layout_0.DataSource = this.dvItems;
            grdExItemList_Layout_0.IsCurrentLayout = true;
            grdExItemList_Layout_0.Key = "bae";
            grdExItemList_Layout_0.LayoutString = resources.GetString("grdExItemList_Layout_0.LayoutString");
            this.grdExItemList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExItemList_Layout_0});
            this.grdExItemList.Location = new System.Drawing.Point(0, 40);
            this.grdExItemList.Name = "grdExItemList";
            this.grdExItemList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExItemList.Size = new System.Drawing.Size(792, 386);
            this.grdExItemList.TabIndex = 8;
            this.grdExItemList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExItemList.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExItemList_CellValueChanged);
            this.grdExItemList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExItemList_ColumnHeaderClick);
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 426);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(792, 40);
            this.pnlBtn.TabIndex = 9;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(400, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "취 소";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(288, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 23);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "편 성";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.chkAdState_10);
            this.panel4.Controls.Add(this.chkAdState_40);
            this.panel4.Controls.Add(this.chkAdState_30);
            this.panel4.Controls.Add(this.chkAdState_20);
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Controls.Add(this.ebSearchKey);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(792, 40);
            this.panel4.TabIndex = 7;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(238, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 22);
            this.label13.TabIndex = 34;
            this.label13.Text = "광고상태";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkAdState_10
            // 
            this.chkAdState_10.Checked = true;
            this.chkAdState_10.CheckedValue = "";
            this.chkAdState_10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAdState_10.Location = new System.Drawing.Point(310, 8);
            this.chkAdState_10.Name = "chkAdState_10";
            this.chkAdState_10.Size = new System.Drawing.Size(44, 22);
            this.chkAdState_10.TabIndex = 2;
            this.chkAdState_10.Text = "대기";
            this.chkAdState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.Location = new System.Drawing.Point(508, 8);
            this.chkAdState_40.Name = "chkAdState_40";
            this.chkAdState_40.Size = new System.Drawing.Size(44, 22);
            this.chkAdState_40.TabIndex = 5;
            this.chkAdState_40.Text = "종료";
            this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_30
            // 
            this.chkAdState_30.BackColor = System.Drawing.SystemColors.Window;
            this.chkAdState_30.Location = new System.Drawing.Point(442, 8);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(44, 22);
            this.chkAdState_30.TabIndex = 4;
            this.chkAdState_30.Text = "중지";
            this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckedValue = "";
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAdState_20.Location = new System.Drawing.Point(376, 8);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(44, 22);
            this.chkAdState_20.TabIndex = 3;
            this.chkAdState_20.Text = "편성";
            this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(680, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ebSearchKey.Location = new System.Drawing.Point(8, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(208, 22);
            this.ebSearchKey.TabIndex = 1;
            this.ebSearchKey.Text = "검색어를 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // ItemAd_pForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(792, 466);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ItemAd_pForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "광고대상 검색";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ItemAd_pForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaign_pDs)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).EndInit();
            this.pnlBtn.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void ItemAd_pForm_Load(object sender, System.EventArgs e)
		{
            
			// 데이터관리용 객체생성
			dt = ((DataView)grdExItemList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExItemList.DataSource]; 

			SearchHomeAdItem();
		}
		#endregion

		#region 사용자 액션처리 메소드

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
			if(e.KeyCode == Keys.Enter && !(IsNewSearchKey))
			{
				SearchHomeAdItem();
			}
		}


		private void btnOk_Click(object sender, System.EventArgs e)
		{
			AddSchHomeAd();
            this.Close();
		}
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}


		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchHomeAdItem();
		}

		private void grdExItemList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{

			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dt.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="False";
					dt.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="True";
					dt.Rows[i].EndEdit();
				}
			}
		}

		#endregion
  
		#region 처리메소드

		/// <summary>
		/// 홈광고 편성대상 광고목록 조회
		/// </summary>
		private void SearchHomeAdItem()
		{
			StatusMessage("홈광고 편성대상 광고목록을 조회합니다.");

			try
			{
				grdExItemList.UnCheckAllRecords();

				campaignModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
				if(IsNewSearchKey || ebSearchKey.Text.Length == 0)
				{
					campaignModel.SearchKey = "";
				}
				else
				{
					campaignModel.SearchKey  = ebSearchKey.Text;
				}

				if(chkAdState_10.Checked)   campaignModel.SearchchkAdState_10   = "Y";
				if(chkAdState_20.Checked)   campaignModel.SearchchkAdState_20   = "Y";
				if(chkAdState_30.Checked)   campaignModel.SearchchkAdState_30   = "Y";
				if(chkAdState_40.Checked)   campaignModel.SearchchkAdState_40   = "Y";

				campaignModel.SearchMediaCode = keyMediaCode; // 매체코드셋트
				campaignModel.ContractSeq = keyContractSeq;

				// 목록조회 서비스를 호출한다.
				new CampaignManager(systemModel,commonModel).GetContractItemPopList(campaignModel);

				if (campaignModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(campaign_pDs.HomeAdItems, campaignModel.ContractDataSet);				
					StatusMessage(campaignModel.ResultCnt + "건의  홈광고 편성대상 광고목록이 조회되었습니다.");					
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고목록 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고목록 조회오류",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// 선택된광고를 홈광고에 편성
		/// </summary>
		private void AddSchHomeAd()
		{
			StatusMessage("선택된 광고를 홈광고에 편성합니다.");

			// 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExItemList.UpdateData();

			DataRow[] foundRows = campaign_pDs.HomeAdItems.Select("CheckYn = 'True'");

			if(foundRows.Length == 0 )
			{
				MessageBox.Show("선택된 광고파일이 없습니다.","광고파일 배포관리", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				int SetCount = 0;

				//인서트 시킴
				for(int i=0;i < campaign_pDs.HomeAdItems.Rows.Count;i++)
				{
					DataRow row = campaign_pDs.HomeAdItems.Rows[i];

					if(row["CheckYn"].ToString().Equals("True"))
					{
						campaignModel.Init();

						campaignModel.MediaCode = keyMediaCode;
						campaignModel.CampaignCode = keyCampaign;
						campaignModel.ContractSeq = keyContractSeq;
						campaignModel.ItemNo    = row["ItemNo"].ToString();
						
						new CampaignManager(systemModel,commonModel).SetCampaignDetailCreate(campaignModel);
						
						if(campaignModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}
					}
				}

				// 체크된 모든 항목을 클리어
				ClearListCheck();

				//Opener.keyScheduleOrder = schHomeAdModel.ScheduleOrder;
				Opener.ReloadList();


				if(SetCount > 0)
				{
					SearchHomeAdItem();
				}

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("캠페인디테일 편성오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("캠페인디테일 편성오류",new string[] {"",ex.Message});
			}			
		}


		private void ClearListCheck()
		{

			// 체크된 모든 항목을 클리어
			grdExItemList.UnCheckAllRecords();
			grdExItemList.UpdateData();
				   
			// 데이터 클리어
			for(int i=0;i < dt.Rows.Count;i++)
			{
				dt.Rows[i].BeginEdit();
				dt.Rows[i]["CheckYn"]="False";
				dt.Rows[i].EndEdit();
			}
		}



		#endregion

        private void grdExItemList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cm.Position;
                if (curRow >= 0)
                {
                    dt.Rows[curRow].BeginEdit();
                    dt.Rows[curRow]["CheckYn"] = grdExItemList.GetValue(e.Column).ToString();
                    dt.Rows[curRow].EndEdit();
                }
            }
        }
	}
}
