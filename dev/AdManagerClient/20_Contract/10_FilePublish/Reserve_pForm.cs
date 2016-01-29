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
using Excel = Microsoft.Office.Interop.Excel; // ¿¢¼¿ ÂüÁ¶
using System.Reflection;

namespace AdManagerClient
{
	/// <summary>
	/// Reserve_pForm¿¡ ´ëÇÑ ¿ä¾à ¼³¸íÀÔ´Ï´Ù.
	/// </summary>
	public class Reserve_pForm : System.Windows.Forms.Form
	{
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.UI.Dock.UIPanel uiPanelTop;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelTopContainer;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelMain;
		private Janus.Windows.UI.Dock.UIPanel uiPanelBottom;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelBottomContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelFile;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelFileContainer;
		private Janus.Windows.GridEX.GridEX grdFile;
		private System.ComponentModel.IContainer components;

		private AdManagerClient.FilePublishDs filePublishDs1;
		private System.Data.DataView dvFile;

		#region »ç¿ëÀÚÁ¤ÀÇ °´Ã¼ ¹× º¯¼ö

		// ½Ã½ºÅÛ Á¤º¸ : È­¸é°øÅë
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// ¸Þ´ºÄÚµå : º¸¾ÈÀÌ ÇÊ¿äÇÑ È­¸é¿¡ ÇÊ¿äÇÔ
		public	string      MenuCode		= "";
		private	bool		IsAdding		= false;

		// »ç¿ëÇÒ Á¤º¸¸ðµ¨
		FilePublishModel mainModel = new FilePublishModel();	

		// È­¸éÃ³¸®¿ë º¯¼ö
		CurrencyManager	cmFile	= null;
		DataTable		dtFile	= null;
		#endregion

		private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
		private Janus.Windows.EditControls.UIButton btnWorkInsert;
		private Janus.Windows.EditControls.UIButton btnWorkMod;
		private Janus.Windows.EditControls.UIButton btnWorkSave;
		private System.Windows.Forms.Label lblAckNo;
		private Janus.Windows.GridEX.EditControls.EditBox ebAckNo;
		private System.Windows.Forms.Label lblReserveDt;
		private System.Windows.Forms.Label lblState;
		private Janus.Windows.EditControls.UIRadioButton ebState10;
		private Janus.Windows.EditControls.UIRadioButton ebState20;
		private Janus.Windows.EditControls.UIRadioButton ebState30;
		private Janus.Windows.EditControls.UIRadioButton ebState90;
		private Janus.Windows.GridEX.EditControls.EditBox ebMsg;
		private System.Windows.Forms.Label lblMsg;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private System.Windows.Forms.Label lblMod;
		private Janus.Windows.GridEX.EditControls.EditBox ebReserveUser;
		private Janus.Windows.GridEX.EditControls.EditBox ebModUser;
		private Janus.Windows.GridEX.EditControls.MaskedEditBox ebReserveDt;
		private Janus.Windows.CalendarCombo.CalendarCombo ebReserveYYMMDD;
		private Janus.Windows.GridEX.EditControls.IntegerUpDown ebReserveDtHH;
		private Janus.Windows.GridEX.EditControls.IntegerUpDown ebReserveDtmm;
		private System.Windows.Forms.Label lblMessage;

		#region [P] ½ÂÀÎ¹øÈ£
		/// <summary>
		/// ½ÂÀÎ¹øÈ£
		/// </summary>
		private int	keyAckNo     = 0;
		private Janus.Windows.EditControls.UIButton btnClose;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebFileSize;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebFileCnt;
        private System.Windows.Forms.Label lblNowMsg;
		private Janus.Windows.EditControls.UIButton btnExcel;
		/// <summary>
		/// ½ÂÀÎ¹øÈ£¸¦ ¼³Á¤ÇÏ°Å³ª °¡Á®¿É´Ï´Ù( È£ÃâÇÑ À©µµ¿ì¿¡¼­ ¼³Á¤ÇØ¾ßÇÔ)
		/// </summary>
		public	int AckNo
		{
			get
			{
				return keyAckNo ;
			}
			set
			{
				keyAckNo = value;
			}
		}
		#endregion

		#region [P] ¿¹¾à¹øÈ£

		/// <summary>
		/// ¿¹¾àÀÛ¾÷ ÀÏ½Ã
		/// </summary>
		private	string	keyReserveDt = "";
		/// <summary>
		/// ¿¹¾à¹øÈ£¸¦ ¼³Á¤ÇÏ°Å³ª °¡Á®¿É´Ï´Ù(È£ÃâÇÑ À©µµ¿ì¿¡¼­ ¼³Á¤ÇØ¾ß ÇÔ)
		/// </summary>
		public  string  ReserveDt
		{
			get
			{
				return keyReserveDt;
			}
			set
			{
				keyReserveDt = value;
			}
		}
		#endregion

		#region Æû»ý¼ºÀÚ¹× ¼Ò¸êÀÚ
		public Reserve_pForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// »ç¿ë ÁßÀÎ ¸ðµç ¸®¼Ò½º¸¦ Á¤¸®ÇÕ´Ï´Ù.
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

		#region Windows Form µðÀÚÀÌ³Ê¿¡¼­ »ý¼ºÇÑ ÄÚµå
		/// <summary>
		/// µðÀÚÀÌ³Ê Áö¿ø¿¡ ÇÊ¿äÇÑ ¸Þ¼­µåÀÔ´Ï´Ù.
		/// ÀÌ ¸Þ¼­µåÀÇ ³»¿ëÀ» ÄÚµå ÆíÁý±â·Î ¼öÁ¤ÇÏÁö ¸¶½Ê½Ã¿À.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Janus.Windows.GridEX.GridEXLayout grdFile_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reserve_pForm));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelTop = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelTopContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.btnExcel = new Janus.Windows.EditControls.UIButton();
			this.lblMessage = new System.Windows.Forms.Label();
			this.uiPanelMain = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelFile = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelFileContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdFile = new Janus.Windows.GridEX.GridEX();
			this.dvFile = new System.Data.DataView();
			this.filePublishDs1 = new AdManagerClient.FilePublishDs();
			this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.lblNowMsg = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.ebReserveDtmm = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
			this.ebReserveDtHH = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
			this.ebReserveYYMMDD = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.ebReserveDt = new Janus.Windows.GridEX.EditControls.MaskedEditBox();
			this.ebModUser = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lblMod = new System.Windows.Forms.Label();
			this.ebReserveUser = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebMsg = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lblMsg = new System.Windows.Forms.Label();
			this.ebState90 = new Janus.Windows.EditControls.UIRadioButton();
			this.ebState30 = new Janus.Windows.EditControls.UIRadioButton();
			this.ebState20 = new Janus.Windows.EditControls.UIRadioButton();
			this.ebState10 = new Janus.Windows.EditControls.UIRadioButton();
			this.lblState = new System.Windows.Forms.Label();
			this.lblReserveDt = new System.Windows.Forms.Label();
			this.ebAckNo = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lblAckNo = new System.Windows.Forms.Label();
			this.uiPanelBottom = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelBottomContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.ebFileCnt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.ebFileSize = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.btnClose = new Janus.Windows.EditControls.UIButton();
			this.btnWorkSave = new Janus.Windows.EditControls.UIButton();
			this.btnWorkMod = new Janus.Windows.EditControls.UIButton();
			this.btnWorkInsert = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelTop)).BeginInit();
			this.uiPanelTop.SuspendLayout();
			this.uiPanelTopContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMain)).BeginInit();
			this.uiPanelMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelFile)).BeginInit();
			this.uiPanelFile.SuspendLayout();
			this.uiPanelFileContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdFile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvFile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.filePublishDs1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
			this.uiPanel0.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
			this.uiPanelDetail.SuspendLayout();
			this.uiPanelDetailContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelBottom)).BeginInit();
			this.uiPanelBottom.SuspendLayout();
			this.uiPanelBottomContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// uiPM
			// 
			this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
			this.uiPM.ContainerControl = this;
			this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			this.uiPanelTop.Id = new System.Guid("715803f4-77f0-476f-9663-94c1b2f518dc");
			this.uiPM.Panels.Add(this.uiPanelTop);
			this.uiPanelMain.Id = new System.Guid("5e9aac98-f32a-4edb-bd6c-7dd4196b18a3");
			this.uiPanelMain.StaticGroup = true;
			this.uiPanelFile.Id = new System.Guid("e0c4591c-1b67-4969-adf9-7cc91bcf9aa8");
			this.uiPanelMain.Panels.Add(this.uiPanelFile);
			this.uiPanel0.Id = new System.Guid("45be2da5-7bd8-4405-aed0-6b5d5a3e4d9d");
			this.uiPanel0.StaticGroup = true;
			this.uiPanelDetail.Id = new System.Guid("ea942e70-7f85-4917-ac9e-c82a9b07dc35");
			this.uiPanel0.Panels.Add(this.uiPanelDetail);
			this.uiPanelMain.Panels.Add(this.uiPanel0);
			this.uiPM.Panels.Add(this.uiPanelMain);
			this.uiPanelBottom.Id = new System.Guid("6026cbd4-9332-46dd-9fd4-c95cfb2af1d5");
			this.uiPM.Panels.Add(this.uiPanelBottom);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("715803f4-77f0-476f-9663-94c1b2f518dc"), Janus.Windows.UI.Dock.PanelDockStyle.Top, new System.Drawing.Size(974, 40), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("5e9aac98-f32a-4edb-bd6c-7dd4196b18a3"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Top, true, new System.Drawing.Size(974, 488), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("e0c4591c-1b67-4969-adf9-7cc91bcf9aa8"), new System.Guid("5e9aac98-f32a-4edb-bd6c-7dd4196b18a3"), 599, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("45be2da5-7bd8-4405-aed0-6b5d5a3e4d9d"), new System.Guid("5e9aac98-f32a-4edb-bd6c-7dd4196b18a3"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 371, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("ea942e70-7f85-4917-ac9e-c82a9b07dc35"), new System.Guid("45be2da5-7bd8-4405-aed0-6b5d5a3e4d9d"), 408, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("6026cbd4-9332-46dd-9fd4-c95cfb2af1d5"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(974, 41), true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("715803f4-77f0-476f-9663-94c1b2f518dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("5e9aac98-f32a-4edb-bd6c-7dd4196b18a3"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("e0c4591c-1b67-4969-adf9-7cc91bcf9aa8"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("c1c52130-ac6a-4c02-9a73-85ceb7567bb7"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("6026cbd4-9332-46dd-9fd4-c95cfb2af1d5"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("45be2da5-7bd8-4405-aed0-6b5d5a3e4d9d"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("f9f86711-f862-470c-ab7d-c54b88ce0dc5"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("ea942e70-7f85-4917-ac9e-c82a9b07dc35"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelTop
			// 
			this.uiPanelTop.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelTop.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
			this.uiPanelTop.InnerContainer = this.uiPanelTopContainer;
			this.uiPanelTop.Location = new System.Drawing.Point(3, 3);
			this.uiPanelTop.Name = "uiPanelTop";
			this.uiPanelTop.Size = new System.Drawing.Size(974, 40);
			this.uiPanelTop.TabIndex = 4;
			this.uiPanelTop.Text = "Á¶È¸ºÎ";
			// 
			// uiPanelTopContainer
			// 
			this.uiPanelTopContainer.Controls.Add(this.btnExcel);
			this.uiPanelTopContainer.Controls.Add(this.lblMessage);
			this.uiPanelTopContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelTopContainer.Name = "uiPanelTopContainer";
			this.uiPanelTopContainer.Size = new System.Drawing.Size(972, 34);
			this.uiPanelTopContainer.TabIndex = 0;
			// 
			// btnExcel
			// 
			this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExcel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnExcel.Location = new System.Drawing.Point(876, 5);
			this.btnExcel.Name = "btnExcel";
			this.btnExcel.Size = new System.Drawing.Size(88, 24);
			this.btnExcel.TabIndex = 5;
			this.btnExcel.Text = "Excel";
			this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
			// 
			// lblMessage
			// 
			this.lblMessage.BackColor = System.Drawing.Color.Transparent;
			this.lblMessage.Font = new System.Drawing.Font("¸¼Àº °íµñ", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblMessage.Location = new System.Drawing.Point(16, 10);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(848, 16);
			this.lblMessage.TabIndex = 4;
			// 
			// uiPanelMain
			// 
			this.uiPanelMain.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMain.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
			this.uiPanelMain.Location = new System.Drawing.Point(3, 43);
			this.uiPanelMain.Name = "uiPanelMain";
			this.uiPanelMain.Size = new System.Drawing.Size(974, 488);
			this.uiPanelMain.TabIndex = 4;
			this.uiPanelMain.Text = "Main";
			// 
			// uiPanelFile
			// 
			this.uiPanelFile.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Standard;
			this.uiPanelFile.InnerContainer = this.uiPanelFileContainer;
			this.uiPanelFile.Location = new System.Drawing.Point(0, 0);
			this.uiPanelFile.Name = "uiPanelFile";
			this.uiPanelFile.Size = new System.Drawing.Size(599, 484);
			this.uiPanelFile.TabIndex = 4;
			this.uiPanelFile.Text = "¿¹¾àÆÄÀÏ";
			// 
			// uiPanelFileContainer
			// 
			this.uiPanelFileContainer.Controls.Add(this.grdFile);
			this.uiPanelFileContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelFileContainer.Name = "uiPanelFileContainer";
			this.uiPanelFileContainer.Size = new System.Drawing.Size(597, 460);
			this.uiPanelFileContainer.TabIndex = 0;
			// 
			// grdFile
			// 
			this.grdFile.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdFile.AlternatingColors = true;
			this.grdFile.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdFile.DataSource = this.dvFile;
			grdFile_DesignTimeLayout.LayoutString = resources.GetString("grdFile_DesignTimeLayout.LayoutString");
			this.grdFile.DesignTimeLayout = grdFile_DesignTimeLayout;
			this.grdFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdFile.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdFile.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdFile.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdFile.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdFile.GridLineColor = System.Drawing.Color.Silver;
			this.grdFile.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdFile.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdFile.GroupByBoxVisible = false;
			this.grdFile.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdFile.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdFile.Location = new System.Drawing.Point(0, 0);
			this.grdFile.Name = "grdFile";
			this.grdFile.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdFile.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdFile.Size = new System.Drawing.Size(597, 460);
			this.grdFile.TabIndex = 4;
			this.grdFile.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdFile.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdFile.TotalRowFormatStyle.BackColor = System.Drawing.Color.MediumSeaGreen;
			this.grdFile.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdFile.ColumnButtonClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdFile_ColumnButtonClick);
			// 
			// dvFile
			// 
			this.dvFile.Table = this.filePublishDs1.FileReserveDetail;
			// 
			// filePublishDs1
			// 
			this.filePublishDs1.DataSetName = "FilePublishDs";
			this.filePublishDs1.Locale = new System.Globalization.CultureInfo("en-US");
			this.filePublishDs1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanel0
			// 
			this.uiPanel0.Location = new System.Drawing.Point(603, 0);
			this.uiPanel0.Name = "uiPanel0";
			this.uiPanel0.Size = new System.Drawing.Size(371, 484);
			this.uiPanel0.TabIndex = 4;
			this.uiPanel0.Text = "¿¹¾àÀÛ¾÷";
			// 
			// uiPanelDetail
			// 
			this.uiPanelDetail.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
			this.uiPanelDetail.Location = new System.Drawing.Point(0, 22);
			this.uiPanelDetail.Name = "uiPanelDetail";
			this.uiPanelDetail.Size = new System.Drawing.Size(371, 462);
			this.uiPanelDetail.TabIndex = 4;
			this.uiPanelDetail.Text = "ÀÛ¾÷³»¿ª";
			// 
			// uiPanelDetailContainer
			// 
			this.uiPanelDetailContainer.Controls.Add(this.lblNowMsg);
			this.uiPanelDetailContainer.Controls.Add(this.label1);
			this.uiPanelDetailContainer.Controls.Add(this.ebReserveDtmm);
			this.uiPanelDetailContainer.Controls.Add(this.ebReserveDtHH);
			this.uiPanelDetailContainer.Controls.Add(this.ebReserveYYMMDD);
			this.uiPanelDetailContainer.Controls.Add(this.ebReserveDt);
			this.uiPanelDetailContainer.Controls.Add(this.ebModUser);
			this.uiPanelDetailContainer.Controls.Add(this.ebModDt);
			this.uiPanelDetailContainer.Controls.Add(this.lblMod);
			this.uiPanelDetailContainer.Controls.Add(this.ebReserveUser);
			this.uiPanelDetailContainer.Controls.Add(this.ebMsg);
			this.uiPanelDetailContainer.Controls.Add(this.lblMsg);
			this.uiPanelDetailContainer.Controls.Add(this.ebState90);
			this.uiPanelDetailContainer.Controls.Add(this.ebState30);
			this.uiPanelDetailContainer.Controls.Add(this.ebState20);
			this.uiPanelDetailContainer.Controls.Add(this.ebState10);
			this.uiPanelDetailContainer.Controls.Add(this.lblState);
			this.uiPanelDetailContainer.Controls.Add(this.lblReserveDt);
			this.uiPanelDetailContainer.Controls.Add(this.ebAckNo);
			this.uiPanelDetailContainer.Controls.Add(this.lblAckNo);
			this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
			this.uiPanelDetailContainer.Size = new System.Drawing.Size(369, 460);
			this.uiPanelDetailContainer.TabIndex = 0;
			// 
			// lblNowMsg
			// 
			this.lblNowMsg.Location = new System.Drawing.Point(140, 297);
			this.lblNowMsg.Name = "lblNowMsg";
			this.lblNowMsg.Size = new System.Drawing.Size(162, 22);
			this.lblNowMsg.TabIndex = 21;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("³ª´®°íµñ", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label1.Location = new System.Drawing.Point(10, 384);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(344, 62);
			this.label1.TabIndex = 19;
			this.label1.Text = "¿¹¾àÀÛ¾÷ Æí¼º½Ã ¾Æ·¡ Ãß°¡/º¯°æ ¹öÆ°À» ¸ÕÀú Å¬¸¯ÇÏ½Ã±â ¹Ù¶ø´Ï´Ù. ¿¹¾àÆÄÀÏÀº ¿ÞÂÊ ±×¸®µåÀÇ ¹öÆ°À» Å¬¸¯ÇÏ½Ã¸é Áï½Ã Ã³¸®µË´Ï´Ù.";
			// 
			// ebReserveDtmm
			// 
			this.ebReserveDtmm.Location = new System.Drawing.Point(230, 37);
			this.ebReserveDtmm.Maximum = 59;
			this.ebReserveDtmm.Name = "ebReserveDtmm";
			this.ebReserveDtmm.Size = new System.Drawing.Size(36, 20);
			this.ebReserveDtmm.TabIndex = 18;
			this.ebReserveDtmm.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.ebReserveDtmm.Value = 30;
			this.ebReserveDtmm.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebReserveDtHH
			// 
			this.ebReserveDtHH.Location = new System.Drawing.Point(192, 37);
			this.ebReserveDtHH.Maximum = 23;
			this.ebReserveDtHH.Name = "ebReserveDtHH";
			this.ebReserveDtHH.Size = new System.Drawing.Size(36, 20);
			this.ebReserveDtHH.TabIndex = 17;
			this.ebReserveDtHH.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.ebReserveDtHH.Value = 15;
			this.ebReserveDtHH.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebReserveYYMMDD
			// 
			this.ebReserveYYMMDD.AutoSize = false;
			// 
			// 
			// 
			this.ebReserveYYMMDD.DropDownCalendar.FirstMonth = new System.DateTime(2010, 9, 1, 0, 0, 0, 0);
			this.ebReserveYYMMDD.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.ebReserveYYMMDD.Location = new System.Drawing.Point(86, 37);
			this.ebReserveYYMMDD.Name = "ebReserveYYMMDD";
			this.ebReserveYYMMDD.ShowDropDown = false;
			this.ebReserveYYMMDD.ShowTodayButton = false;
			this.ebReserveYYMMDD.ShowUpDown = true;
			this.ebReserveYYMMDD.Size = new System.Drawing.Size(100, 21);
			this.ebReserveYYMMDD.TabIndex = 16;
			this.ebReserveYYMMDD.TodayButtonText = "¿À´Ã";
			this.ebReserveYYMMDD.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			// 
			// ebReserveDt
			// 
			this.ebReserveDt.Location = new System.Drawing.Point(84, 226);
			this.ebReserveDt.Name = "ebReserveDt";
			this.ebReserveDt.Numeric = true;
			this.ebReserveDt.Size = new System.Drawing.Size(176, 20);
			this.ebReserveDt.TabIndex = 15;
			this.ebReserveDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebReserveDt.Visible = false;
			this.ebReserveDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebModUser
			// 
			this.ebModUser.Location = new System.Drawing.Point(273, 61);
			this.ebModUser.Name = "ebModUser";
			this.ebModUser.Size = new System.Drawing.Size(72, 20);
			this.ebModUser.TabIndex = 14;
			this.ebModUser.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebModUser.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebModDt
			// 
			this.ebModDt.Location = new System.Drawing.Point(86, 61);
			this.ebModDt.Name = "ebModDt";
			this.ebModDt.Size = new System.Drawing.Size(180, 20);
			this.ebModDt.TabIndex = 13;
			this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lblMod
			// 
			this.lblMod.Location = new System.Drawing.Point(22, 63);
			this.lblMod.Name = "lblMod";
			this.lblMod.Size = new System.Drawing.Size(64, 18);
			this.lblMod.TabIndex = 12;
			this.lblMod.Text = "¼öÁ¤ÀÏ½Ã";
			// 
			// ebReserveUser
			// 
			this.ebReserveUser.Location = new System.Drawing.Point(273, 37);
			this.ebReserveUser.Name = "ebReserveUser";
			this.ebReserveUser.Size = new System.Drawing.Size(72, 20);
			this.ebReserveUser.TabIndex = 11;
			this.ebReserveUser.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebReserveUser.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebMsg
			// 
			this.ebMsg.Location = new System.Drawing.Point(86, 114);
			this.ebMsg.Multiline = true;
			this.ebMsg.Name = "ebMsg";
			this.ebMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ebMsg.Size = new System.Drawing.Size(252, 102);
			this.ebMsg.TabIndex = 10;
			this.ebMsg.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMsg.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lblMsg
			// 
			this.lblMsg.Location = new System.Drawing.Point(22, 116);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(64, 18);
			this.lblMsg.TabIndex = 9;
			this.lblMsg.Text = "ÀÛ¾÷¸Þ¸ð";
			// 
			// ebState90
			// 
			this.ebState90.Font = new System.Drawing.Font("¸¼Àº °íµñ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.ebState90.ForeColor = System.Drawing.Color.OrangeRed;
			this.ebState90.Location = new System.Drawing.Point(270, 90);
			this.ebState90.Name = "ebState90";
			this.ebState90.Size = new System.Drawing.Size(52, 20);
			this.ebState90.TabIndex = 8;
			this.ebState90.Text = "¿À·ù";
			this.ebState90.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebState30
			// 
			this.ebState30.Font = new System.Drawing.Font("¸¼Àº °íµñ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.ebState30.ForeColor = System.Drawing.Color.DimGray;
			this.ebState30.Location = new System.Drawing.Point(210, 90);
			this.ebState30.Name = "ebState30";
			this.ebState30.Size = new System.Drawing.Size(52, 20);
			this.ebState30.TabIndex = 7;
			this.ebState30.Text = "Ãë¼Ò";
			this.ebState30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebState20
			// 
			this.ebState20.Font = new System.Drawing.Font("¸¼Àº °íµñ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.ebState20.ForeColor = System.Drawing.Color.SeaGreen;
			this.ebState20.Location = new System.Drawing.Point(150, 90);
			this.ebState20.Name = "ebState20";
			this.ebState20.Size = new System.Drawing.Size(52, 20);
			this.ebState20.TabIndex = 6;
			this.ebState20.Text = "¿Ï·á";
			this.ebState20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebState10
			// 
			this.ebState10.Font = new System.Drawing.Font("¸¼Àº °íµñ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.ebState10.ForeColor = System.Drawing.Color.DodgerBlue;
			this.ebState10.Location = new System.Drawing.Point(90, 90);
			this.ebState10.Name = "ebState10";
			this.ebState10.Size = new System.Drawing.Size(52, 20);
			this.ebState10.TabIndex = 5;
			this.ebState10.Text = "¿¹¾à";
			this.ebState10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lblState
			// 
			this.lblState.Location = new System.Drawing.Point(22, 90);
			this.lblState.Name = "lblState";
			this.lblState.Size = new System.Drawing.Size(64, 18);
			this.lblState.TabIndex = 4;
			this.lblState.Text = "ÀÛ¾÷»óÅÂ";
			// 
			// lblReserveDt
			// 
			this.lblReserveDt.Location = new System.Drawing.Point(22, 39);
			this.lblReserveDt.Name = "lblReserveDt";
			this.lblReserveDt.Size = new System.Drawing.Size(64, 18);
			this.lblReserveDt.TabIndex = 2;
			this.lblReserveDt.Text = "¿¹¾àÀÏ½Ã";
			// 
			// ebAckNo
			// 
			this.ebAckNo.BackColor = System.Drawing.Color.LightCyan;
			this.ebAckNo.Location = new System.Drawing.Point(86, 13);
			this.ebAckNo.Name = "ebAckNo";
			this.ebAckNo.Size = new System.Drawing.Size(100, 20);
			this.ebAckNo.TabIndex = 1;
			this.ebAckNo.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAckNo.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lblAckNo
			// 
			this.lblAckNo.Location = new System.Drawing.Point(22, 14);
			this.lblAckNo.Name = "lblAckNo";
			this.lblAckNo.Size = new System.Drawing.Size(64, 18);
			this.lblAckNo.TabIndex = 0;
			this.lblAckNo.Text = "½ÂÀÎ¹øÈ£";
			// 
			// uiPanelBottom
			// 
			this.uiPanelBottom.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelBottom.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.ContainerPanel;
			this.uiPanelBottom.InnerContainer = this.uiPanelBottomContainer;
			this.uiPanelBottom.Location = new System.Drawing.Point(3, 531);
			this.uiPanelBottom.Name = "uiPanelBottom";
			this.uiPanelBottom.Size = new System.Drawing.Size(974, 41);
			this.uiPanelBottom.TabIndex = 4;
			this.uiPanelBottom.Text = "Bottom";
			// 
			// uiPanelBottomContainer
			// 
			this.uiPanelBottomContainer.Controls.Add(this.label4);
			this.uiPanelBottomContainer.Controls.Add(this.label3);
			this.uiPanelBottomContainer.Controls.Add(this.label2);
			this.uiPanelBottomContainer.Controls.Add(this.ebFileCnt);
			this.uiPanelBottomContainer.Controls.Add(this.ebFileSize);
			this.uiPanelBottomContainer.Controls.Add(this.btnClose);
			this.uiPanelBottomContainer.Controls.Add(this.btnWorkSave);
			this.uiPanelBottomContainer.Controls.Add(this.btnWorkMod);
			this.uiPanelBottomContainer.Controls.Add(this.btnWorkInsert);
			this.uiPanelBottomContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelBottomContainer.Name = "uiPanelBottomContainer";
			this.uiPanelBottomContainer.Size = new System.Drawing.Size(972, 39);
			this.uiPanelBottomContainer.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(464, 10);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 18);
			this.label4.TabIndex = 14;
			this.label4.Text = "Byte";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(288, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(24, 18);
			this.label3.TabIndex = 13;
			this.label3.Text = "°³";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(160, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 18);
			this.label2.TabIndex = 12;
			this.label2.Text = "¼±ÅÃÆÄÀÏ";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ebFileCnt
			// 
			this.ebFileCnt.FormatMask = Janus.Windows.GridEX.NumericEditFormatMask.General;
			this.ebFileCnt.FormatString = "##,##0";
			this.ebFileCnt.Location = new System.Drawing.Point(232, 8);
			this.ebFileCnt.Name = "ebFileCnt";
			this.ebFileCnt.Size = new System.Drawing.Size(48, 20);
			this.ebFileCnt.TabIndex = 11;
			this.ebFileCnt.Text = "0";
			this.ebFileCnt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFileCnt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ebFileCnt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebFileSize
			// 
			this.ebFileSize.FormatString = "##,##0";
			this.ebFileSize.Location = new System.Drawing.Point(320, 8);
			this.ebFileSize.Name = "ebFileSize";
			this.ebFileSize.Size = new System.Drawing.Size(136, 20);
			this.ebFileSize.TabIndex = 10;
			this.ebFileSize.Text = "0";
			this.ebFileSize.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
			this.ebFileSize.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ebFileSize.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnClose.Location = new System.Drawing.Point(8, 7);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(104, 24);
			this.btnClose.TabIndex = 9;
			this.btnClose.Text = "´Ý±â";
			this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnWorkSave
			// 
			this.btnWorkSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWorkSave.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnWorkSave.Location = new System.Drawing.Point(846, 7);
			this.btnWorkSave.Name = "btnWorkSave";
			this.btnWorkSave.Size = new System.Drawing.Size(104, 24);
			this.btnWorkSave.TabIndex = 8;
			this.btnWorkSave.Text = "ÀÛ¾÷ÀúÀå";
			this.btnWorkSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnWorkSave.Click += new System.EventHandler(this.btnWorkSave_Click);
			// 
			// btnWorkMod
			// 
			this.btnWorkMod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWorkMod.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnWorkMod.Location = new System.Drawing.Point(734, 7);
			this.btnWorkMod.Name = "btnWorkMod";
			this.btnWorkMod.Size = new System.Drawing.Size(104, 24);
			this.btnWorkMod.TabIndex = 7;
			this.btnWorkMod.Text = "ÀÛ¾÷º¯°æ";
			this.btnWorkMod.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnWorkMod.Click += new System.EventHandler(this.btnWorkMod_Click);
			// 
			// btnWorkInsert
			// 
			this.btnWorkInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWorkInsert.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnWorkInsert.Location = new System.Drawing.Point(622, 7);
			this.btnWorkInsert.Name = "btnWorkInsert";
			this.btnWorkInsert.Size = new System.Drawing.Size(104, 24);
			this.btnWorkInsert.TabIndex = 4;
			this.btnWorkInsert.Text = "ÀÛ¾÷Ãß°¡";
			this.btnWorkInsert.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnWorkInsert.Click += new System.EventHandler(this.btnWorkInsert_Click);
			// 
			// Reserve_pForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 13);
			this.ClientSize = new System.Drawing.Size(980, 575);
			this.Controls.Add(this.uiPanelBottom);
			this.Controls.Add(this.uiPanelMain);
			this.Controls.Add(this.uiPanelTop);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Reserve_pForm";
			this.Text = "ÆÄÀÏ¹èÆ÷½ÂÀÎ ¿¹¾à";
			this.Load += new System.EventHandler(this.Reserve_pForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelTop)).EndInit();
			this.uiPanelTop.ResumeLayout(false);
			this.uiPanelTopContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMain)).EndInit();
			this.uiPanelMain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelFile)).EndInit();
			this.uiPanelFile.ResumeLayout(false);
			this.uiPanelFileContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdFile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvFile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.filePublishDs1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
			this.uiPanel0.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
			this.uiPanelDetail.ResumeLayout(false);
			this.uiPanelDetailContainer.ResumeLayout(false);
			this.uiPanelDetailContainer.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelBottom)).EndInit();
			this.uiPanelBottom.ResumeLayout(false);
			this.uiPanelBottomContainer.ResumeLayout(false);
			this.uiPanelBottomContainer.PerformLayout();
			this.ResumeLayout(false);

        }
		#endregion

		private void Reserve_pForm_Load(object sender, System.EventArgs e)
		{
			// µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º, ¿ÞÂÊ±×¸®µå
			//dtFile	= ((DataView)grdFile.DataSource).Table;  
			//cmFile	= (CurrencyManager) this.BindingContext[grdFile.DataSource]; 
			//cmFile.PositionChanged += new System.EventHandler(OnFileRowChanged); 

		}

		#region Ã³¸®¸Þ¼Òµå

		private void DisabledButton()
		{
			btnWorkInsert.Enabled = false;
			btnWorkMod.Enabled	= false;
			btnWorkSave.Enabled	= false;
		}

		/// <summary>
		/// ¿¹¾àÀÛ¾÷ ÆÄÀÏ¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchFile()
		{
			try
			{
				mainModel.Init();
				mainModel.MediaCode = "0";
				mainModel.AckNo		= Convert.ToString(keyAckNo);

				new FilePublishManager(systemModel,commonModel).GetReserveFileList(mainModel);
				if (mainModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(filePublishDs1.FileReserveDetail, mainModel.FileListDataSet);
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÆÄÀÏ¸ñ·Ï Á¶È¸ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÆÄÀÏ¸ñ·Ï Á¶È¸ ¿À·ù",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// ÀÛ¾÷³»¿ë »ó¼¼º¸±â
		/// </summary>
		/// <param name="key"></param>
		private void SearchWorkDetail(string key)
		{
			try
			{
				mainModel.Init();
				mainModel.MediaCode =	"0";
				mainModel.AckNo		=	Convert.ToString(keyAckNo);
				mainModel.ReserveDt	=	key;
				this.OnWorkColumnSet("S");

				// ¹Ìµî·Ï°ÇÀº µ¥ÀÌÅÍ Á¶È¸¾ÊÇÔ
				if( key != "" )
				{

					new FilePublishManager(systemModel,commonModel).GetReserveWorkDetail(mainModel);
					if (mainModel.ResultCD.Equals("0000"))
					{
						ebAckNo.Text		=	mainModel.AckNo;
						ebReserveDt.Text	=	mainModel.ReserveDt;
						ebReserveYYMMDD.Text= mainModel.ReserveDt.Substring(0,4) + "-" + mainModel.ReserveDt.Substring(4,2) + "-" + mainModel.ReserveDt.Substring(6,2);
						ebReserveDtHH.Text	= mainModel.ReserveDt.Substring(8,2);
						ebReserveDtmm.Text	= mainModel.ReserveDt.Substring(10,2);

						ebReserveUser.Text	=	mainModel.ReserveUserName;
						ebModDt.Text		=	mainModel.ModDt;
						ebModUser.Text		=	mainModel.ModifyUserName;
						ebMsg.Text			=	mainModel.PublishDesc;

						if( mainModel.State.Equals("10") )	ebState10.Checked = true;
						if( mainModel.State.Equals("20") )
						{
							// ¿Ï·áµÈ ¿¹¾àÀÛ¾÷Àº Á¶È¸¸¸ °¡´ÉÇÏ´Ù
							ebState20.Checked = true;
							DisabledButton();
						}
						if( mainModel.State.Equals("30") )	ebState30.Checked = true;
						if( mainModel.State.Equals("90") )	ebState90.Checked = true;


					
					}
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÀÛ¾÷¸ñ·Ï Á¶È¸ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÀÛ¾÷¸ñ·Ï Á¶È¸ ¿À·ù",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// ¿¹¾àÀÛ¾÷ MasterÀúÀå
		/// </summary>
		private void SaveWorkDetail()
		{
			try
			{
				string	resKey = "";
				// ÀÔ·Â¹ÞÀº ÀÏ½Ã
				string	strDate1 = ebReserveYYMMDD.Value.ToString("yyyyMMdd");
				int		intDtYear	= Convert.ToInt32(strDate1.Substring(0,4));
				int		intDtMon	= Convert.ToInt32(strDate1.Substring(4,2));
				int		intDtDay	= Convert.ToInt32(strDate1.Substring(6,2));
				int		intHour1	= ebReserveDtHH.Value;
				int		intMin1		= ebReserveDtmm.Value;
				DateTime dtRev = new DateTime(intDtYear, intDtMon, intDtDay,intHour1, intMin1,0);

				// ½Ã½ºÅÛÀÏ½Ã
				string	strDate2 = DateTime.Now.ToString("yyyyMMdd");
				int		intHour2 = DateTime.Now.Hour;
				int		intMin2	 = DateTime.Now.Minute;

				System.TimeSpan diff1 = dtRev.Subtract( DateTime.Now);

				// ÃÖ¼ÒÇÑ 10ºÐÀÌÈÄ¿©¾ß ÇÑ´Ù.
				if( diff1.TotalMinutes < 10 )
				{
					MessageBox.Show("¿¹¾àÀÏ½Ã´Â ÇöÀçº¸´Ù ÃÖ¼ÒÇÑ 10ºÐ ÀÌ»ó Ä¿¾ß ÇÕ´Ï´Ù","¹èÆ÷¿¹¾à",MessageBoxButtons.OK, MessageBoxIcon.Information);
					ebReserveDtmm.Focus();
					return;
				}

				resKey = strDate1 +intHour1.ToString("00") + intMin1.ToString("00");

				mainModel.Init();
				mainModel.MediaCode			= "0";
				mainModel.AckNo				= Convert.ToString(keyAckNo);
				mainModel.SearchReserveKey	= keyReserveDt;

				mainModel.ReserveDt			= resKey;
				mainModel.PublishDesc		= ebMsg.Text.Trim();
				if(		 ebState10.Checked == true )	mainModel.State = "10";
				else if( ebState20.Checked == true )	mainModel.State = "20";
				else if( ebState30.Checked == true )	mainModel.State = "30";
				else if( ebState90.Checked == true )	mainModel.State = "90";
				else
				{
					MessageBox.Show("ÀÛ¾÷»óÅÂ°¡ ¼±ÅÃµÇÁö ¾Ê¾Ò½À´Ï´Ù.","¹èÆ÷¿¹¾à",MessageBoxButtons.OK, MessageBoxIcon.Information);
					ebState90.Focus();
					return;
				}

				new FilePublishManager(systemModel,commonModel).SetReserveWorkDetail(mainModel);
				if (mainModel.ResultCD.Equals("0000"))
				{
					this.ReserveDt = resKey;
					this.SearchMain();
					lblMessage.Text = "¿¹¾àÀÛ¾÷ ¼öÁ¤¿Ï·á!!!";
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÀÛ¾÷¸ñ·Ï Á¶È¸ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÀÛ¾÷¸ñ·Ï Á¶È¸ ¿À·ù",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// ¿¹¾àÀÛ¾÷ MasterÀÔ·Â
		/// </summary>
		private void InsertWorkDetail()
		{
			try
			{
				string	resKey = "";
				// ÀÔ·Â¹ÞÀº ÀÏ½Ã
				string	strDate1	= ebReserveYYMMDD.Value.ToString("yyyyMMdd");
				int		intDtYear	= Convert.ToInt32(strDate1.Substring(0,4));
				int		intDtMon	= Convert.ToInt32(strDate1.Substring(4,2));
				int		intDtDay	= Convert.ToInt32(strDate1.Substring(6,2));
				int		intHour1	= ebReserveDtHH.Value;
				int		intMin1		= ebReserveDtmm.Value;
				DateTime dtRev = new DateTime(intDtYear, intDtMon, intDtDay,intHour1, intMin1,0);

				// ½Ã½ºÅÛÀÏ½Ã
				string	strDate2 = DateTime.Now.ToString("yyyyMMdd");
				int		intHour2 = DateTime.Now.Hour;
				int		intMin2	 = DateTime.Now.Minute;

				System.TimeSpan diff1 = dtRev.Subtract( DateTime.Now);

				// ÃÖ¼ÒÇÑ 10ºÐÀÌÈÄ¿©¾ß ÇÑ´Ù.
				if( diff1.TotalMinutes < 10 )
				{
					MessageBox.Show("¿¹¾àÀÏ½Ã´Â ÇöÀçº¸´Ù ÃÖ¼ÒÇÑ 10ºÐ ÀÌ»ó Ä¿¾ß ÇÕ´Ï´Ù","¹èÆ÷¿¹¾à",MessageBoxButtons.OK, MessageBoxIcon.Information);
					ebReserveDtmm.Focus();
					return;
				}

				resKey = strDate1 +intHour1.ToString("00") + intMin1.ToString("00");

				mainModel.Init();
				mainModel.MediaCode			= "0";
				mainModel.AckNo				= Convert.ToString(keyAckNo);
				mainModel.ReserveDt			= resKey;
				mainModel.PublishDesc		= ebMsg.Text.Trim();
				if(		 ebState10.Checked == true )	mainModel.State = "10";
				else if( ebState20.Checked == true )	mainModel.State = "20";
				else if( ebState30.Checked == true )	mainModel.State = "30";
				else if( ebState90.Checked == true )	mainModel.State = "90";
				else
				{
					MessageBox.Show("ÀÛ¾÷»óÅÂ°¡ ¼±ÅÃµÇÁö ¾Ê¾Ò½À´Ï´Ù.","¹èÆ÷¿¹¾à",MessageBoxButtons.OK, MessageBoxIcon.Information);
					ebState90.Focus();
					return;
				}

				new FilePublishManager(systemModel,commonModel).NewReserveWorkDetail(mainModel);
				if (mainModel.ResultCD.Equals("0000"))
				{
					this.ReserveDt = resKey;
					this.SearchMain();
					lblMessage.Text = "¿¹¾àÀÛ¾÷ ÀÔ·Â¿Ï·á!!!";
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÀÛ¾÷¸ñ·Ï Á¶È¸ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÀÛ¾÷¸ñ·Ï Á¶È¸ ¿À·ù",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// ¿¹¾àÆÄÀÏ ÀÛ¾÷ Ã³¸®
		/// </summary>
		/// <param name="ItemNo">´ë»ó ±¤°í¹øÈ£</param>
		/// <param name="jobType">Ã³¸®ÀÛ¾÷±¸ºÐ + Ãß°¡, - Á¦°Å </param>
		private bool SaveFileDetail(int ItemNo, string jobType)
		{
			bool rtnValue = false;
			try
			{
				mainModel.Init();
				mainModel.MediaCode		= "0";
				mainModel.AckNo			= Convert.ToString(keyAckNo);
				mainModel.ReserveDt		= keyReserveDt;
				mainModel.ItemNo		= ItemNo;
				mainModel.JobType		= jobType;

				new FilePublishManager(systemModel,commonModel).SetReserveFileDetail(mainModel);
				if (mainModel.ResultCD.Equals("0000"))
				{
					rtnValue = true;
					lblMessage.Text = "¿¹¾àÀÛ¾÷ ÆÄÀÏÃ³¸® ¼öÁ¤¿Ï·á!!!";
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÀÛ¾÷¸ñ·Ï Á¶È¸ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÀÛ¾÷¸ñ·Ï Á¶È¸ ¿À·ù",new string[] {"",ex.Message});
			}
			return rtnValue;
		}


		/// <summary>
		/// È­¸é¿ÀÇÂÈÄ È£ÃâÇÏ´Â ÇÔ¼ö
		/// </summary>
		public void SearchMain()
		{
			dtFile	= ((DataView)grdFile.DataSource).Table;  
			cmFile	= (CurrencyManager) this.BindingContext[grdFile.DataSource]; 

			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			DisabledButton();
			OnWorkColumnSet("S");
			this.SearchFile();
			this.CheckRowsFile( keyReserveDt );
			this.SearchWorkDetail( keyReserveDt );
            this.CheckRowSum();
		}
		#endregion


		/// <summary>
		/// ¿¹¾àÀÛ¾÷¿¡ ÇÒ´çµÈ ÆÄÀÏµéÀ» ¿ÞÂÊ±×¸®µå¿¡ Ç¥½ÃÇÑ´Ù.
		/// </summary>
		/// <param name="key"></param>
		private void CheckRowsFile(string key)
		{
			try
			{
				if ( dtFile.Rows.Count < 1 ) return;

				foreach (DataRow row in dtFile.Rows)
				{
					row["CheckYn"] = "False";
				}
				Application.DoEvents();
              
				foreach (DataRow row in dtFile.Rows)
				{
					if( row["ReserveDt"].ToString().Equals(key) )
					{
						row["CheckYn"] = "True";
					}
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Å°¯“¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Å°¯“¿À·ù",new string[] {"",ex.Message});
			}			
		}

        /// <summary>
        /// ¼±ÅÃµÈ Ç×¸ñÀÇ °¹¼ö¹× »çÀÌÁîÇÕÀ» ±¸ÇÑ´Ù
        /// </summary>
        private void CheckRowSum()
        {
            try
            {
                int fileSize = 0;
                int fileCnt  = 0;

                if ( dtFile.Rows.Count < 1 )
                {
                    ebFileCnt.Value = fileCnt;
                    ebFileSize.Value= fileSize;
                    return;
                }

                foreach (DataRow row in dtFile.Rows)
                {
                    if( row["CheckYn"].ToString().Equals("True") )
                    {
                        fileSize += Convert.ToInt32( row["FileSize"].ToString() );
                        fileCnt  += 1;
                    }
                    Application.DoEvents();
                }

                ebFileCnt.Value = fileCnt;
                ebFileSize.Value= fileSize;
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("Å°¯“¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("Å°¯“¿À·ù",new string[] {"",ex.Message});
            }			
        }


		/// <summary>
		/// ¿¹¾àÀÛ¾÷ ÇÊµåµéÀ» ¼³Á¤ÇÑ´Ù.
		/// Á¶È¸(S),¼öÁ¤(U),ÀÔ·Â(I)
		/// </summary>
		/// <param name="workType"></param>
		private void OnWorkColumnSet( string workType )
		{
			ebAckNo.ReadOnly		= true;
			ebReserveYYMMDD.ReadOnly= true;
			ebReserveDtHH.ReadOnly	= true;
			ebReserveDtmm.ReadOnly	= true;
			ebReserveDt.ReadOnly	= true;
			ebReserveUser.ReadOnly	= true;
			ebModDt.ReadOnly		= true;
			ebModUser.ReadOnly		= true;
			ebMsg.ReadOnly			= true;
			//ebState10.Enabled		= false;
			//ebState20.Enabled		= false;
			//ebState30.Enabled		= false;
			//ebState90.Enabled		= false;

			ebAckNo.BackColor		= Color.LightCyan;
			ebReserveDt.BackColor	= Color.LightCyan;
			ebReserveYYMMDD.BackColor	= Color.LightCyan;
			ebReserveDtHH.BackColor	= Color.LightCyan;
			ebReserveDtmm.BackColor	= Color.LightCyan;
			ebReserveUser.BackColor	= Color.LightCyan;
			ebModDt.BackColor		= Color.LightCyan;
			ebModUser.BackColor		= Color.LightCyan;
			ebMsg.BackColor			= Color.LightCyan;

			if( workType.Equals("S") )
			{
				ebAckNo.Text		=	"";
				ebReserveDt.Text	=	"";
				ebReserveUser.Text	=	"";
				ebModDt.Text		=	"";
				ebModUser.Text		=	"";
				ebState10.Checked	=	false;
				ebState20.Checked	=	false;
				ebState30.Checked	=	false;
				ebState90.Checked	=	false;
				ebMsg.Text			=	"";

				// ¹öÆ°µé
				btnWorkInsert.Enabled = true;
				btnWorkMod.Enabled	= true;
			}
			else if( workType.Equals("U") )
			{
				ebReserveDt.ReadOnly	= false;
				ebReserveUser.ReadOnly	= true;
				ebReserveYYMMDD.ReadOnly= false;
				ebReserveDtHH.ReadOnly	= false;
				ebReserveDtmm.ReadOnly	= false;
				ebModDt.ReadOnly		= false;
				ebModUser.ReadOnly		= true;
				ebMsg.ReadOnly			= false;

				ebReserveDt.BackColor	= Color.White;
				ebReserveYYMMDD.BackColor	= Color.White;
				ebReserveDtHH.BackColor	= Color.White;
				ebReserveDtmm.BackColor	= Color.White;
				ebModDt.BackColor		= Color.White;
				ebMsg.BackColor			= Color.White;
				btnWorkSave.Enabled = true;
				ebMsg.Focus();
				
			}
			else if( workType.Equals("I") )
			{
				ebReserveDt.ReadOnly	= false;
				ebReserveUser.ReadOnly	= true;
				ebReserveYYMMDD.ReadOnly= false;
				ebReserveDtHH.ReadOnly	= false;
				ebReserveDtmm.ReadOnly	= false;
				ebModDt.ReadOnly		= false;
				ebModUser.ReadOnly		= true;
				ebMsg.ReadOnly			= false;

				ebReserveDt.BackColor	= Color.White;
				ebReserveYYMMDD.BackColor	= Color.White;
				ebReserveDtHH.BackColor	= Color.White;
				ebReserveDtmm.BackColor	= Color.White;
				ebModDt.BackColor		= Color.White;
				ebMsg.BackColor			= Color.White;
				btnWorkSave.Enabled = true;

				grdFile.Enabled = true;

				ebAckNo.Text			= Convert.ToString(keyAckNo);
				ebReserveYYMMDD.Value	= DateTime.Now.AddMinutes(30);
				ebReserveDtHH.Value		= DateTime.Now.AddMinutes(30).Hour;
				ebReserveDtmm.Value		= DateTime.Now.AddMinutes(30).Minute;

				ebReserveUser.Text	=	"";
				ebModDt.Text		=	"";
				ebModUser.Text		=	"";
				ebState10.Checked	=	true;
				ebState20.Checked	=	false;
				ebState30.Checked	=	false;
				ebState90.Checked	=	false;
				ebMsg.Text			=	"";

				ebReserveDtmm.Focus();
			}

		
		}


		/// <summary>
		/// º¯°æ¹öÆ° Å¬¸¯½Ã
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnWorkMod_Click(object sender, System.EventArgs e)
		{
			if( keyReserveDt == "" )
			{
				MessageBox.Show("¹Ì¼³Á¤ÀÛ¾÷Àº ¼öÁ¤À» ÇÒ ¼ö ¾ø½À´Ï´Ù","¹èÆ÷¿¹¾à",MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				IsAdding = false;
				lblMessage.Text = "¿¹¾àÀÛ¾÷ ¼öÁ¤";
				DisabledButton();
				OnWorkColumnSet("U");
			}
		}

		/// <summary>
		/// ÀÔ·Â¹öÆ° Å¬¸¯½Ã
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnWorkInsert_Click(object sender, System.EventArgs e)
		{
			IsAdding = true;
			lblMessage.Text = "¿¹¾àÀÛ¾÷ Ãß°¡";
			DisabledButton();
			OnWorkColumnSet("I");
		}

		/// <summary>
		/// ÀúÀå¹öÆ° Å¬¸¯½Ã
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnWorkSave_Click(object sender, System.EventArgs e)
		{
			//DisabledButton();
			//OnWorkColumnSet("S");
			if( IsAdding )
			{
				this.InsertWorkDetail();
			}
			else
			{
				this.SaveWorkDetail();
			}
		}

		/// <summary>
		/// ÆÄÀÏ°ü¸® ¹öÆ° Å¬¸¯½Ã
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnFile_Click(object sender, System.EventArgs e)
		{
			IsAdding = false;
			lblMessage.Text = "¿¹¾àÆÄÀÏ °ü¸®";
			DisabledButton();
			
			uiPanelFile.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;

			grdFile.Enabled = true;
			//grdFile.Tables[0].Columns["CheckYn"].EditType = Janus.Windows.GridEX.EditType.CheckBox;
		}

		private void btnFileEnd_Click(object sender, System.EventArgs e)
		{
			IsAdding = false;
			lblMessage.Text = "¿¹¾àÆÄÀÏ °ü¸®";
			uiPanelFile.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
			DisabledButton();
			DisabledButton();
			OnWorkColumnSet("S");
			this.SearchFile();
			this.CheckRowsFile( keyReserveDt );
			this.SearchWorkDetail( keyReserveDt );
			
			//grdFile.Enabled = true;
			//grdFile.Tables[0].Columns["CheckYn"].EditType = Janus.Windows.GridEX.EditType.CheckBox;
		
		}

		private void grdFile_ColumnButtonClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
			int		cur			= cmFile.Position;
			string	jobMode		= "+";
			int		itemNo		= Convert.ToInt32(dtFile.Rows[cur]["ItemNo"]);
			bool	curData		= Convert.ToBoolean(dtFile.Rows[cur]["CheckYn"]);

			if( curData )
			{
				dtFile.Rows[cmFile.Position]["CheckYn"] = false;
				jobMode = "-";
			}
			else
			{
				dtFile.Rows[cmFile.Position]["CheckYn"] = true;
				jobMode = "+";
			}

			
			if ( this.SaveFileDetail(itemNo,jobMode) )
			{
				dtFile.AcceptChanges();
			}
			else
			{
				dtFile.Rows[cmFile.Position]["CheckYn"] = curData;
				dtFile.AcceptChanges();
			}

            // ÇÕ°èÇ×¸ñ °è»ê
            this.CheckRowSum();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void btnNow_Click(object sender, System.EventArgs e)
        {
            DialogResult result = MessageBox.Show("¼±ÅÃµÈ ¿¹¾àÀÛ¾÷À» Áï½Ã ½ÇÇàÇÕ´Ï´Ù.","¿¹¾àÀÛ¾÷",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
                lblNowMsg.Text = "ÀÛ¾÷½ÇÇà ¿Ï·á!!!";
            else
                lblNowMsg.Text = "Ãë¼Ò!!!";
        }

		#region ¿¢¼¿ Ãâ·Â
		/// <summary>
		/// ¿¢¼¿ »ý¼º
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnExcel_Click(object sender, System.EventArgs e)
		{
			Excel.Application xlApp = null;
			Excel._Workbook oWB = null;
			Excel._Worksheet oSheet = null;
			Excel.Range oRng = null;

			try
			{
				int ColMax = 6; // ÄÃ·³¼ö 

				int HeaderRow = 6;
				int DataRow = 7;
				string StartCol = "A";
				string EndCol = "";
				int CondCount = 0;
				int HeaderCount = 0;
				int DataCount = 0;

				int FileSizeSum = 0;

				// ¸¶Áö¸· ÄÃ·³ÀÇ ÀÎµ¦½º¹®ÀÚ
				EndCol = GetColumnIndex(ColMax);

				#region [ ¿¢¼¿ ¿ÀºêÁ§Æ® ]
				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;
				oSheet.Name = "ÆÄÀÏ¹èÆ÷ ¿¹¾à³»¿ª";
				#endregion

				//xlApp.Visible = true;

				#region [ Å¸ÀÌÆ²¹× Á¶°Ç ºÎºÐ ]
				// Å¸ÀÌÆ² ÀÛ¼º
				oSheet.Cells[1, 1] = "·¹Æ÷Æ®¸í";
				oRng = oSheet.get_Range("A1", "B1");
				oRng.Merge(true);

				oSheet.Cells[2, 1] = "±âÁØÀÏÀÚ";
				oRng = oSheet.get_Range("A2", "B2");
				oRng.Merge(true);

				oSheet.Cells[3, 1] = "ÃÑ °Ç¼ö";
				oRng = oSheet.get_Range("A3", "B3");
				oRng.Merge(true);

				oSheet.Cells[4, 1] = "ÃÑÆÄÀÏBytes";
				oRng = oSheet.get_Range("A4", "B4");
				oRng.Merge(true);

				oRng = oSheet.get_Range("A1", "B4");
				oRng.Font.Bold = false;
				oRng.Font.Size = 10;
				oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);   //¼¿ ¹è°æ»ö 
				oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

				oSheet.Cells[1, 4] = "ÆÄÀÏ¹èÆ÷ ¿¹¾à³»¿ª";
				oRng = oSheet.get_Range("C1", "D1");
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 12;
				oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

				oSheet.Cells[2, 4] = DateTime.Now.ToString();
				oRng = oSheet.get_Range("C2", "D2");
				oRng.Merge(true);

				oSheet.Cells[3, 4] = "";
				oRng = oSheet.get_Range("C3", "D3");
				oRng.Merge(true);
				oRng.NumberFormatLocal = "#,##0";

				oSheet.Cells[4, 4] = "";
				oRng = oSheet.get_Range("C4", "D4");
				oRng.Merge(true);
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range("C2", "D4");
				oRng.Font.Bold = true;
				oRng.Font.Size = 10;
				oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;


				// Á¶°ÇºÎ Å×µÎ¸®
				oRng = oSheet.get_Range("A1", "D4");
				oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
				oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;	// Å×µÎ¸®¼Ó¼º ¾Æ·¡¿¡ ½Ç¼±
				oRng.Borders.Weight = Excel.XlBorderWeight.xlThin;		// Å×µÎ¸®¼Ó¼º ¾Æ·¡¿¡ °¡´Â¼±
				#endregion

				#region [ Çì´õºÎºÐ ]
				CondCount++;
				HeaderCount = 1;
				oSheet.Cells[HeaderRow, HeaderCount++] = "¼ø¼­";
				oSheet.Cells[HeaderRow, HeaderCount++] = "¹øÈ£";
				oSheet.Cells[HeaderRow, HeaderCount++] = "±¤°í¸í";
				oSheet.Cells[HeaderRow, HeaderCount++] = "ÆÄÀÏ¸í";
				oSheet.Cells[HeaderRow, HeaderCount++] = "ÀÛ¾÷¿¹¾àÀÏ½Ã";
				oSheet.Cells[HeaderRow, HeaderCount++] = "FileSize";

				oRng = oSheet.get_Range(StartCol + Convert.ToString(HeaderRow), EndCol + Convert.ToString(HeaderRow)); // Çì´õÀÇ ¹üÀ§
				oRng.Font.Bold = true;							// ÆùÆ® ±½°Ô
				oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;	// ¼¼·ÎÁß¾ÓÁ¤·Ä
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// °¡·ÎÁß¾ÓÁ¤·Ä	 
				oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //¼¿ ¹è°æ»ö 
				oRng.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //ÅØ½ºÆ®»ö		
				#endregion

				DataCount = 0;
				for (int inx = 0; inx < filePublishDs1.FileReserveDetail.Rows.Count; inx++)
				{
					oSheet.Cells[DataRow + DataCount, 1] = inx + 1;
					oSheet.Cells[DataRow + DataCount, 2] = filePublishDs1.FileReserveDetail.Rows[inx]["ItemNo"].ToString();
					oSheet.Cells[DataRow + DataCount, 3] = filePublishDs1.FileReserveDetail.Rows[inx]["ItemNm"].ToString();
					oSheet.Cells[DataRow + DataCount, 4] = filePublishDs1.FileReserveDetail.Rows[inx]["FileName"].ToString();
					oSheet.Cells[DataRow + DataCount, 5] = "'" + filePublishDs1.FileReserveDetail.Rows[inx]["ReserveDt"].ToString();
					oSheet.Cells[DataRow + DataCount, 6] = filePublishDs1.FileReserveDetail.Rows[inx]["FileSize"].ToString();
					DataCount++;

					FileSizeSum += Convert.ToInt32(filePublishDs1.FileReserveDetail.Rows[inx]["FileSize"].ToString());
				}

				oSheet.Cells[3, 3] = DataCount;
				oSheet.Cells[4, 3] = FileSizeSum;

				DataCount--;


				// Áß¾ÓÁ¤·Ä
				oRng = oSheet.get_Range(GetColumnIndex(1) + Convert.ToString(DataRow), GetColumnIndex(2) + Convert.ToString(DataRow + DataCount));
				oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;	// ¼¼·ÎÁß¾ÓÁ¤·Ä
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// °¡·ÎÁß¾ÓÁ¤·Ä	 

				// Áß¾ÓÁ¤·Ä
				oRng = oSheet.get_Range(GetColumnIndex(5) + Convert.ToString(DataRow), GetColumnIndex(5) + Convert.ToString(DataRow + DataCount));
				oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;	// ¼¼·ÎÁß¾ÓÁ¤·Ä
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// °¡·ÎÁß¾ÓÁ¤·Ä	 

				// »çÀÌÁî ¼ýÀÚÇü
				oRng = oSheet.get_Range(GetColumnIndex(6) + Convert.ToString(DataRow), GetColumnIndex(6) + Convert.ToString(DataRow + DataCount));
				oRng.NumberFormatLocal = "#,##0";

				// µ¥ÀÌÅÍ ÀÛ¼º
				oRng = oSheet.get_Range(StartCol + Convert.ToString(HeaderRow), EndCol + Convert.ToString(DataRow + DataCount));	// µ¥ÀÌÅÍÀÇ ¹üÀ§
				oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;	// Å×µÎ¸®¼Ó¼º ¾Æ·¡¿¡ ½Ç¼±
				oRng.Borders.Weight = Excel.XlBorderWeight.xlThin;		// Å×µÎ¸®¼Ó¼º ¾Æ·¡¿¡ °¡´Â¼±
				oRng.Font.Bold = false;
				oRng.Font.Size = 9;
				oRng.EntireColumn.AutoFit();					// µ¥ÀÌÅÍÀÇ Å©±â¿¡ ¼¿ÀÇ °¡·ÎÅ©±â ¸ÂÃã

				xlApp.Visible = true;
				xlApp.UserControl = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private string GetColumnIndex(int ColCount)
		{
			string[] ColName = { "Z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y" };

			string ColumnIndex;

			// 26º¸´Ù Å©¸é
			if (ColCount > ColName.Length)
			{
				// 2ÀÚ¸® ÀÎµ¦½º¹®ÀÚ 26 => Z;  27->AA
				ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount / ColName.Length)))] + ColName[(int)(Math.Floor((float)(ColCount % ColName.Length)))];
			}
			else
			{
				ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount % ColName.Length)))];
			}

			return ColumnIndex;
		}

		#endregion     	
	}
}
