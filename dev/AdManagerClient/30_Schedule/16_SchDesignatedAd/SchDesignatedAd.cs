using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient.Schedule
{
	/// <summary>
	/// SchDesignatedAd�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchDesignatedAdForm : System.Windows.Forms.Form
	{
		#region [ ȭ�� ��Ʈ�ѵ� ]

		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private Janus.Windows.EditControls.UICheckBox chkAdState_20;
		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
		private Janus.Windows.EditControls.UIComboBox cbSearchRap;
		private Janus.Windows.EditControls.UICheckBox chkAdState_30;
		private Janus.Windows.EditControls.UICheckBox chkAdState_40;
		private AdManagerClient._30_Schedule._16_SchDesignatedAd.SchDesignateAdDs schDesignateAdDs;
		private System.Data.DataView dvItem;
		private System.ComponentModel.IContainer components;
		#endregion

		#region [����] �ý��������� �޴����� ����

		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		#endregion

		#region [ ����� ���� ������ ]
		// ����� ������
		private	SchDesignateModel mainModel	= new SchDesignateModel();

		// ȭ��ó���� ����
		private	CurrencyManager	cm0		= null;
		private Janus.Windows.GridEX.GridEX grdExItem;
		private	DataTable       dt0		= null;

		private int keyItemNo = 0;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private	string	keyItemName	= "";
		#endregion

		#region [ ������ �� �Ҹ��� ]
		public SchDesignatedAdForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
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

		#region Windows Form �����̳ʿ��� ������ �ڵ�
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout grdExItem_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchDesignatedAdForm));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExItem = new Janus.Windows.GridEX.GridEX();
            this.dvItem = new System.Data.DataView();
            this.schDesignateAdDs = new AdManagerClient._30_Schedule._16_SchDesignatedAd.SchDesignateAdDs();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schDesignateAdDs)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPM
            // 
            this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.uiPM.ContainerControl = this;
            this.uiPM.SplitterSize = 2;
            this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanelSearch.Id = new System.Guid("974df519-7d71-4a07-ba21-26d3323759c7");
            this.uiPM.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("53c802a3-1b31-427d-8584-f19b270a3145");
            this.uiPM.Panels.Add(this.uiPanelList);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("974df519-7d71-4a07-ba21-26d3323759c7"), Janus.Windows.UI.Dock.PanelDockStyle.Top, new System.Drawing.Size(620, 45), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("53c802a3-1b31-427d-8584-f19b270a3145"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(620, 581), true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("974df519-7d71-4a07-ba21-26d3323759c7"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("53c802a3-1b31-427d-8584-f19b270a3145"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(3, 3);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(620, 45);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "Panel 0";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.btnClose);
            this.uiPanelSearchContainer.Controls.Add(this.btnSearch);
            this.uiPanelSearchContainer.Controls.Add(this.chkAdState_20);
            this.uiPanelSearchContainer.Controls.Add(this.cbSearchMedia);
            this.uiPanelSearchContainer.Controls.Add(this.cbSearchRap);
            this.uiPanelSearchContainer.Controls.Add(this.chkAdState_30);
            this.uiPanelSearchContainer.Controls.Add(this.chkAdState_40);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(618, 41);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(523, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(78, 24);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "�� ��";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(439, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(78, 24);
            this.btnSearch.TabIndex = 22;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.BackColor = System.Drawing.Color.Transparent;
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckedValue = "";
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAdState_20.Location = new System.Drawing.Point(272, 10);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(44, 22);
            this.chkAdState_20.TabIndex = 16;
            this.chkAdState_20.Text = "��";
            this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.AutoSize = false;
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 10);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(120, 22);
            this.cbSearchMedia.TabIndex = 14;
            this.cbSearchMedia.Text = "��ü����";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.AutoSize = false;
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(136, 10);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 22);
            this.cbSearchRap.TabIndex = 15;
            this.cbSearchRap.Text = "�̵�����";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_30
            // 
            this.chkAdState_30.BackColor = System.Drawing.Color.Transparent;
            this.chkAdState_30.Checked = true;
            this.chkAdState_30.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_30.Location = new System.Drawing.Point(328, 10);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(44, 22);
            this.chkAdState_30.TabIndex = 17;
            this.chkAdState_30.Text = "����";
            this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.BackColor = System.Drawing.Color.Transparent;
            this.chkAdState_40.Location = new System.Drawing.Point(384, 10);
            this.chkAdState_40.Name = "chkAdState_40";
            this.chkAdState_40.Size = new System.Drawing.Size(44, 22);
            this.chkAdState_40.TabIndex = 18;
            this.chkAdState_40.Text = "����";
            this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiPanelList
            // 
            this.uiPanelList.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelList.AllowPanelDrag = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelList.AllowPanelDrop = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelList.AutoHideButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(3, 48);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(620, 581);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "������";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.Controls.Add(this.grdExItem);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(618, 557);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExItem
            // 
            this.grdExItem.AlternatingColors = true;
            this.grdExItem.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExItem.CellToolTipText = "���õ� Row�� [����Ŭ��] �Ͻø� �˴ϴ�";
            this.grdExItem.DataSource = this.dvItem;
            grdExItem_DesignTimeLayout.LayoutString = resources.GetString("grdExItem_DesignTimeLayout.LayoutString");
            this.grdExItem.DesignTimeLayout = grdExItem_DesignTimeLayout;
            this.grdExItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExItem.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExItem.EmptyRows = true;
            this.grdExItem.FocusCellFormatStyle.BackColor = System.Drawing.Color.DodgerBlue;
            this.grdExItem.FocusCellFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExItem.FocusCellFormatStyle.ForeColor = System.Drawing.Color.Gold;
            this.grdExItem.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExItem.Font = new System.Drawing.Font("���� ���", 8.25F);
            this.grdExItem.FrozenColumns = 1;
            this.grdExItem.GridLineColor = System.Drawing.Color.Silver;
            this.grdExItem.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExItem.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExItem.GroupByBoxVisible = false;
            this.grdExItem.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExItem.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdExItem.Location = new System.Drawing.Point(0, 0);
            this.grdExItem.Name = "grdExItem";
            this.grdExItem.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExItem.SelectedFormatStyle.BackColor = System.Drawing.Color.DodgerBlue;
            this.grdExItem.SelectedFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExItem.SelectedFormatStyle.ForeColor = System.Drawing.Color.Gold;
            this.grdExItem.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExItem.Size = new System.Drawing.Size(618, 557);
            this.grdExItem.TabIndex = 13;
            this.grdExItem.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExItem.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExItem.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExItem.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.grdExItem_RowDoubleClick);
            // 
            // dvItem
            // 
            this.dvItem.Table = this.schDesignateAdDs.ContractItem;
            // 
            // schDesignateAdDs
            // 
            this.schDesignateAdDs.DataSetName = "SchDesignateAdDs";
            this.schDesignateAdDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.schDesignateAdDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // SchDesignatedAdForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(626, 632);
            this.Controls.Add(this.uiPanelList);
            this.Controls.Add(this.uiPanelSearch);
            this.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SchDesignatedAdForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "��������� �߰�-STEP1";
            this.Load += new System.EventHandler(this.SchDesignatedAd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schDesignateAdDs)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		#endregion

		#region [ �Ӽ� ]
		/// <summary>
		/// [�����] ���õ� �����ȣ�� �������ų� �����մϴ�
		/// </summary>
		public int	ItemNo
		{
			get
			{
				return keyItemNo;
			}
			set
			{
				keyItemNo = value;
			}
		}

		/// <summary>
		/// [�����] ���õ� ������� �����ɴϴ�
		/// </summary>
		public string	ItemName
		{
			get
			{
				return	keyItemName;
			}
		}
		#endregion


		private void SchDesignatedAd_Load(object sender, System.EventArgs e)
		{
			// �����Ͱ����� ��ü����
			dt0	= ((DataView)grdExItem.DataSource).Table;  
			cm0 = (CurrencyManager) this.BindingContext[grdExItem.DataSource]; 

			// ��Ʈ�� �ʱ�ȭ
			InitControl();	
		}



		#region ��Ʈ�� �ʱ�ȭ

		private void InitControl()
		{
			ProgressStart();
			InitCombo();
			SearchItem();
			if( keyItemNo > 0 )	ScrollToRow(keyItemNo.ToString());
			ProgressStop();
		}

		private void InitCombo()
		{
			Init_MediaCode();
			Init_RapCode();
			InitCombo_Level();
		}

		private void Init_MediaCode()
		{
			// ��ü�� ��ȸ�Ѵ�.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(schDesignateAdDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchMedia.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = schDesignateAdDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchMedia.Items.AddRange(comboItems);
			this.cbSearchMedia.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void Init_RapCode()
		{
			// ���� ��ȸ�Ѵ�.
			MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);
			
			if (mediarapcodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(schDesignateAdDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchRap.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediarapcodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
			for(int i=0;i<mediarapcodeModel.ResultCnt;i++)
			{
				DataRow row = schDesignateAdDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
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
				for(int i=0;i < schDesignateAdDs.Medias.Rows.Count;i++)
				{
					DataRow row = schDesignateAdDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMedia.SelectedValue = FrameSystem._HANATV; // �ϳ�TV�� �⺻������ �Ѵ�.	 		
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

		/// <summary>
		/// �������� �о�´�
		/// </summary>
		private void SearchItem()
		{
			try
			{
				#region [ ������ ��ȸ ]
				// ��� üũ�ڽ��� üũ�� Ǭ��.
				grdExItem.UnCheckAllRecords(); 

				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				mainModel.Init();
				mainModel.Media		=	int.Parse(cbSearchMedia.SelectedItem.Value.ToString());
				mainModel.MediaRep	=	int.Parse(cbSearchRap.SelectedItem.Value.ToString());
				mainModel.AdState10	=	true;
				mainModel.AdState20	=	chkAdState_20.Checked;
				mainModel.AdState30	=	chkAdState_30.Checked;
				mainModel.AdState40	=	chkAdState_40.Checked;

				new SchDesignatedAdManager(systemModel, commonModel).GetItemList( mainModel );

				schDesignateAdDs.ContractItem.Clear();
				if( mainModel.ResultCD.Equals("0000") )
				{
					//keyMedia	= mainModel.Media;

					Utility.SetDataTable( schDesignateAdDs.ContractItem, mainModel.DsItem );
					StatusMessage(mainModel.ResultCnt + "���� ������ ��ȸ�Ǿ����ϴ�.");
				}
				#endregion
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� ����� ���� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� ����� ���� ��ȸ����",new string[] {"",ex.Message});
			}
		}

		/// <summary>
		/// Row�� ����Ŭ���ҽ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void grdExItem_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{
			keyItemNo	= Convert.ToInt32( e.Row.Cells["ItemNo"].Value );	
			keyItemName	= e.Row.Cells["ItemName"].Value.ToString();
			this.DialogResult	= DialogResult.OK;
			this.Close();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			ProgressStart();
			SearchItem();
			if( keyItemNo > 0 )	ScrollToRow(keyItemNo.ToString());
			ProgressStop();
		}

		/// <summary>
		/// �������� �ʰ� �ݱ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			keyItemNo	= 0;
			keyItemName	= "���õ� ���� �����ϴ�";
			this.DialogResult	= DialogResult.Cancel;
			this.Close();
		}

		/// <summary>
		/// Ű ��ġ�� �׸��� �̵�
		/// </summary>
		/// <param name="item"></param>
		private void ScrollToRow(string item)
		{
			int rowIndex = 0;
			if ( dt0.Rows.Count < 1 ) return;
            
			foreach (DataRow row in dt0.Rows )
			{					
				if(row["ItemNo"].ToString().Equals(item))
				{					
					cm0.Position = rowIndex;
					break;								
				}					
				rowIndex++;
			}

			grdExItem.EnsureVisible();
		}


		#region [����] �̺�Ʈ �ڵ鷯�� �Լ�

		public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
		public event ProgressEventHandler 			ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯

		private void StatusMessage(string strMessage)
		{
			if (StatusEvent != null) 
			{
				StatusEventArgs ea = new StatusEventArgs();
				ea.Message   = strMessage;
				StatusEvent(this,ea);
			}
		}
		private void ProgressStart()
		{
			if (ProgressEvent != null) 
			{
				ProgressEventArgs ea = new ProgressEventArgs();
				ea.Type   = ProgressEventArgs.Start;
				ProgressEvent(this,ea);
			}
		}

		private void ProgressStop()
		{
			if (ProgressEvent != null) 
			{
				ProgressEventArgs ea = new ProgressEventArgs();
				ea.Type   = ProgressEventArgs.Stop;
				ProgressEvent(this,ea);
			}
		}
		#endregion
	}
}
