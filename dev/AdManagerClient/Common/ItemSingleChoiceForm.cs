// ===============================================================================
// SchHomeAd SearchForm  for Charites Project
//
// SchHomeAdSearch_pForm.cs
//
// Ȩ������� ����� ��ȸ. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : YJ.Park
 * ������    : 2014.08.8
 * ��������  :        
 *            - ������ ����߰�.
 * --------------------------------------------------------
 */
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
using AdManagerClient.Common.Args;
using AdManagerClient.Common;
using System.Text;

namespace AdManagerClient
{
    /// <summary>
    /// SchHomeAdSearch_pForm�� ���� ��� �����Դϴ�.
    /// </summary>
    /// 
    public class ItemSingleChoiceForm : System.Windows.Forms.Form
    {
        #region �̺�Ʈ�ڵ鷯
        public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
        #endregion
			
        #region ��������� ��ü �� ����

        // �ý��� ���� : ȭ�����
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        public delegate void PopupService(object sender, EventArgs args);
        public event PopupService ReturnDate;

        // ����� ������
        ItemModel itemModel = new ItemModel();	// ������������

        // ȭ��ó���� ����
        bool IsNewSearchKey = true;				// �˻����Է� ����
        CurrencyManager cm = null;				// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
        DataTable dt = null;

        //ȣ���� ��ü
        public string callType = "";    //�˾��� ȣ���� �θ� �����ϱ� ���ؼ� ������
        public string callItemNo = ""; //�˾��� ȣ�� �� �θ𿡼� ���õ� ItemNo���� �޾ƿ��� ���� ���� [E_01]

		// ��ü
		public string	keyMediaCode = "";			// �˾�ȣ��� ��ü��Ʈ
		public string	keyOrder	= "";

        private System.Windows.Forms.Panel panel1;
		private AdManagerClient.ItemMultiChoice_pDs itemMultiChoice_pDs;
		private System.Windows.Forms.Panel panel4;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;
		private System.Data.DataView dvItems;
		private System.Windows.Forms.Label label13;
		private Janus.Windows.EditControls.UICheckBox chkAdState_40;
		private Janus.Windows.EditControls.UICheckBox chkAdState_30;
		private System.Windows.Forms.Label lbFileListCount;
		private Janus.Windows.EditControls.UICheckBox chkAdState_20;
		private Janus.Windows.EditControls.UICheckBox chkAdState_10;
		private Janus.Windows.GridEX.GridEX grdExItemList;

		#endregion

		#region ������ �� �Ҹ���
		public ItemSingleChoiceForm(UserControl parent)
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();
           
			// ��â�� ȣ���� ��Ʈ��
            itemModel.Init();
		}		
		public ItemSingleChoiceForm()
        {
            //
            // Windows Form �����̳� ������ �ʿ��մϴ�.
            //
            InitializeComponent();

            itemModel.Init();
        }

        /// <summary>
        /// ��� ���� ��� ���ҽ��� �����մϴ�.
        /// </summary>
        protected override void Dispose( bool disposing )
        {

            base.Dispose( disposing );
        }

		#endregion

        #region Windows Form �����̳ʿ��� ������ �ڵ�
        /// <summary>
        /// �����̳� ������ �ʿ��� �޼����Դϴ�.
        /// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
        /// </summary>
        private void InitializeComponent()
        {
			Janus.Windows.GridEX.GridEXLayout grdExItemList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemSingleChoiceForm));
			this.dvItems = new System.Data.DataView();
			this.itemMultiChoice_pDs = new AdManagerClient.ItemMultiChoice_pDs();
			this.panel1 = new System.Windows.Forms.Panel();
			this.grdExItemList = new Janus.Windows.GridEX.GridEX();
			this.pnlBtn = new System.Windows.Forms.Panel();
			this.lbFileListCount = new System.Windows.Forms.Label();
			this.btnClose = new Janus.Windows.EditControls.UIButton();
			this.btnOk = new Janus.Windows.EditControls.UIButton();
			this.panel4 = new System.Windows.Forms.Panel();
			this.chkAdState_10 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
			this.label13 = new System.Windows.Forms.Label();
			this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			((System.ComponentModel.ISupportInitialize)(this.dvItems)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.itemMultiChoice_pDs)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).BeginInit();
			this.pnlBtn.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// dvItems
			// 
			this.dvItems.Table = this.itemMultiChoice_pDs.ChoiceAdItems;
			// 
			// itemMultiChoice_pDs
			// 
			this.itemMultiChoice_pDs.DataSetName = "ItemMultiChoice_pDs";
			this.itemMultiChoice_pDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.itemMultiChoice_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
			this.grdExItemList.Font = new System.Drawing.Font("����", 9F);
			this.grdExItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExItemList.GroupByBoxVisible = false;
			this.grdExItemList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			this.grdExItemList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExItemList.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
			grdExItemList_Layout_0.DataSource = this.dvItems;
			grdExItemList_Layout_0.IsCurrentLayout = true;
			grdExItemList_Layout_0.Key = "bae";
			grdExItemList_Layout_0.LayoutString = resources.GetString("grdExItemList_Layout_0.LayoutString");
			this.grdExItemList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExItemList_Layout_0});
			this.grdExItemList.Location = new System.Drawing.Point(0, 43);
			this.grdExItemList.Name = "grdExItemList";
			this.grdExItemList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExItemList.Size = new System.Drawing.Size(792, 380);
			this.grdExItemList.TabIndex = 8;
			this.grdExItemList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExItemList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExItemList_ColumnHeaderClick);
			// 
			// pnlBtn
			// 
			this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
			this.pnlBtn.Controls.Add(this.lbFileListCount);
			this.pnlBtn.Controls.Add(this.btnClose);
			this.pnlBtn.Controls.Add(this.btnOk);
			this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtn.Location = new System.Drawing.Point(0, 423);
			this.pnlBtn.Name = "pnlBtn";
			this.pnlBtn.Size = new System.Drawing.Size(792, 43);
			this.pnlBtn.TabIndex = 9;
			// 
			// lbFileListCount
			// 
			this.lbFileListCount.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.lbFileListCount.Location = new System.Drawing.Point(152, 9);
			this.lbFileListCount.Name = "lbFileListCount";
			this.lbFileListCount.Size = new System.Drawing.Size(88, 22);
			this.lbFileListCount.TabIndex = 45;
			this.lbFileListCount.Text = "0/0";
			this.lbFileListCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.SystemColors.Control;
			this.btnClose.Location = new System.Drawing.Point(400, 9);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(104, 24);
			this.btnClose.TabIndex = 11;
			this.btnClose.Text = "�� ��";
			this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.SystemColors.Control;
			this.btnOk.Location = new System.Drawing.Point(288, 9);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(104, 24);
			this.btnOk.TabIndex = 10;
			this.btnOk.Text = "�� ��";
			this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.White;
			this.panel4.Controls.Add(this.chkAdState_10);
			this.panel4.Controls.Add(this.chkAdState_20);
			this.panel4.Controls.Add(this.label13);
			this.panel4.Controls.Add(this.chkAdState_40);
			this.panel4.Controls.Add(this.chkAdState_30);
			this.panel4.Controls.Add(this.btnSearch);
			this.panel4.Controls.Add(this.ebSearchKey);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(792, 43);
			this.panel4.TabIndex = 7;
			// 
			// chkAdState_10
			// 
			this.chkAdState_10.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_10.Checked = true;
			this.chkAdState_10.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_10.Location = new System.Drawing.Point(320, 13);
			this.chkAdState_10.Name = "chkAdState_10";
			this.chkAdState_10.Size = new System.Drawing.Size(58, 20);
			this.chkAdState_10.TabIndex = 36;
			this.chkAdState_10.Text = "���";
			this.chkAdState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_20
			// 
			this.chkAdState_20.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_20.Checked = true;
			this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_20.Location = new System.Drawing.Point(384, 13);
			this.chkAdState_20.Name = "chkAdState_20";
			this.chkAdState_20.Size = new System.Drawing.Size(56, 19);
			this.chkAdState_20.TabIndex = 35;
			this.chkAdState_20.Text = "��";
			this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(238, 12);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(64, 22);
			this.label13.TabIndex = 34;
			this.label13.Text = "�������";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkAdState_40
			// 
			this.chkAdState_40.Location = new System.Drawing.Point(510, 13);
			this.chkAdState_40.Name = "chkAdState_40";
			this.chkAdState_40.Size = new System.Drawing.Size(55, 20);
			this.chkAdState_40.TabIndex = 5;
			this.chkAdState_40.Text = "����";
			this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_30
			// 
			this.chkAdState_30.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_30.Location = new System.Drawing.Point(446, 13);
			this.chkAdState_30.Name = "chkAdState_30";
			this.chkAdState_30.Size = new System.Drawing.Size(58, 19);
			this.chkAdState_30.TabIndex = 4;
			this.chkAdState_30.Text = "����";
			this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// btnSearch
			// 
			this.btnSearch.BackColor = System.Drawing.Color.White;
			this.btnSearch.Location = new System.Drawing.Point(680, 9);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 25);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "�� ȸ";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(8, 11);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(208, 21);
			this.ebSearchKey.TabIndex = 1;
			this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// ItemSingleChoiceForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(792, 466);
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "ItemSingleChoiceForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "���� �˻�";
			this.Load += new System.EventHandler(this.SchHomeAdSearch_pForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dvItems)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.itemMultiChoice_pDs)).EndInit();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).EndInit();
			this.pnlBtn.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.ResumeLayout(false);

        }
        #endregion

        #region ��Ʈ�� �ε�
        private void SchHomeAdSearch_pForm_Load(object sender, System.EventArgs e)
        {
            
			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExItemList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExItemList.DataSource]; 
			cm.PositionChanged += new EventHandler(cm_PositionChanged);

            SearchHomeAdItem();
        }

        #endregion

        #region ����� �׼�ó�� �޼ҵ�

        /// <summary>
        /// �˻��� ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
        {
            IsNewSearchKey = false;
        }

        /// <summary>
        /// �˻��� Ŭ�� 
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

			//�÷�Index 0(üũ�ڽ��÷���)�� �ƴϸ� ���������� ó��.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click�ÿ� dt Setting 
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
  
        #region ó���޼ҵ�

        /// <summary>
        /// Ȩ������� ����� ������ ��ȸ
        /// </summary>
        private void SearchHomeAdItem()
        {
            StatusMessage("Ȩ������� ����� �������� ��ȸ�մϴ�.");

            try
            {
                grdExItemList.UnCheckAllRecords();

                itemModel.Init();

                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                if (IsNewSearchKey || ebSearchKey.Text.Length == 0) itemModel.SearchKey = "";
                else itemModel.SearchKey = ebSearchKey.Text;

                if (chkAdState_10.Checked) itemModel.SearchchkAdState_10 = "Y";
                if (chkAdState_20.Checked) itemModel.SearchchkAdState_20 = "Y";
                if (chkAdState_30.Checked) itemModel.SearchchkAdState_30 = "Y";
                if (chkAdState_40.Checked) itemModel.SearchchkAdState_40 = "Y";

                itemModel.SearchMediaCode = keyMediaCode; // ��ü�ڵ��Ʈ

                //������ ���� �� ������ �ҷ��� ��� [E_01]
                if (callType.Equals("GetSchAdItemList"))
                {
                    itemModel.ItemNo = callItemNo;
                    new ItemManager(systemModel, commonModel).GetSchAdItemList(itemModel);
                }
                else
                {
                    new ItemManager(systemModel, commonModel).GetContractItemList(itemModel, callType);
                }
                if (itemModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(itemMultiChoice_pDs.ChoiceAdItems, itemModel.ScheduleDataSet);
                    StatusMessage(itemModel.ResultCnt + "���� �������� ��ȸ�Ǿ����ϴ�.");
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("������ ��ȸ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("������ ��ȸ����", new string[] { "", ex.Message });
            }
        }


        /// <summary>
        /// ���õȱ��� Ȩ������� ��
        /// </summary>
        private void AddSchHomeAd()
        {
            StatusMessage("���õ� ���� ���մϴ�.");

            if (keyItemNo == 0)
            {
                MessageBox.Show("���õ� ���������� �����ϴ�.", "������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                ItemEventArgs args = new ItemEventArgs();

                args.keyItemNo = keyItemNo;
                args.keyItemName = keyItemName;
                args.AdType = AdType;
                
                if (callType.Equals("GetSchAdItemList"))
                {
                    args.CheckSchResult = CheckSchAd(callItemNo);
                    if (!args.CheckSchResult.Equals(""))
                    {
                        ReturnDate(this, args);
                        this.Close();
                    }
                }
                else
                {
                    ReturnDate(this, args);
                    this.Close();
                }

            }
        }

        /// <summary>
        /// �� ���� üũ �� �� �ٿ��ֱ� ������ �޾ƿ� [E_01]
        /// </summary>
        /// <param name="ItemNo"></param>
        /// <returns></returns>
        private string CheckSchAd(string ItemNo)
        {
            SchChoiceAdModel schChoiceAdModel = new SchChoiceAdModel();
            schChoiceAdModel.ItemNo = ItemNo;   //���� ������ ���� ItemNo
            StringBuilder CheckSch = new StringBuilder(); //�� ���� üũ
            try
            {

                new SchChoiceAdManager(systemModel, commonModel).CheckSchChoice(schChoiceAdModel);
                //üũ�� �����¸� ����ϱ� ����
                if (schChoiceAdModel.CheckMenu)
                {
                    CheckSch.Append("�޴���");
                    CheckSch.Append("/");
                }
                if (schChoiceAdModel.CheckChannel)
                {
                    CheckSch.Append("ä����");
                    CheckSch.Append("/");
                }
                if (schChoiceAdModel.CheckSeries)
                {
                    CheckSch.Append("ȸ����");
                    CheckSch.Append("/");
                }
                if (schChoiceAdModel.CheckDetail)
                {
                    CheckSch.Append("������");
                    CheckSch.Append("/");
                }
                if (CheckSch.Length > 0)
                {
                    CheckSch.Remove(CheckSch.Length - 1, 1);
                }

                //�޴���, ä����, ȸ������ 1���̶� ��ȸ�ɰ�� yj
                if (schChoiceAdModel.CheckMenu || schChoiceAdModel.CheckChannel || schChoiceAdModel.CheckSeries || schChoiceAdModel.CheckDetail)
                {
                    DialogResult result = new DialogResult();

                    result = UserMessageBox.Show("������ �����ȣ:" + ItemNo + "�� \n" + CheckSch.ToString() + " ������ �����մϴ�.\n'�߰�����' ���ý� ������ �������� �߰� ����˴ϴ�.\n'��������' ���ý� ������ �������� ������ �������� ���� �˴ϴ�.", "�� �����ϱ�", "�߰�����", "��������", "���");

                    if (result == DialogResult.Yes)
                    {
                        return "ADD";
                    }
                    else if (result == DialogResult.No)
                    {
                        return "DELETE";
                    }
                }
                else
                {
                    return "ADD";
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("�� ���� üũ ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("�� ���� üũ ����", new string[] { "", ex.Message });
            }
            return "";
        }

		private void ClearListCheck()
		{

			// üũ�� ��� �׸��� Ŭ����
			grdExItemList.UnCheckAllRecords();
			grdExItemList.UpdateData();
				   
			// ������ Ŭ����
			for(int i=0;i < dt.Rows.Count;i++)
			{
				dt.Rows[i].BeginEdit();
				dt.Rows[i]["CheckYn"]="False";
				dt.Rows[i].EndEdit();
			}
		}



        #endregion

        #region �̺�Ʈ�Լ�

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

		private	int		keyItemNo = 0;
		private	string	keyItemName = "";
        private string AdType = ""; 

		private void cm_PositionChanged(object sender, EventArgs e)
		{
			int	idx = cm.Position;
			if( idx >= 0 )
			{
				keyItemNo	=	Convert.ToInt32( dt.Rows[idx]["ItemNo"].ToString() );
				keyItemName	=	dt.Rows[idx]["ItemName"].ToString();
                AdType = dt.Rows[idx]["AdType"].ToString(); 
			}
			else
			{
				keyItemNo	=	0;
				keyItemName	=	"";
                AdType = ""; 
			}
			Debug.WriteLine( idx.ToString() + "-" + keyItemNo.ToString() + "-" + keyItemName );
		}
	}
}
