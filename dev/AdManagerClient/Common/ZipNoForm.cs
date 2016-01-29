/*
 * -------------------------------------------------------
 * Class Name: ZipNoForm
 * �ֿ���  : �����ȣ ����
 * �ۼ���    : ? 
 * �ۼ���    : ?
 * Ư�̻���  : 
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.08.13
 * �����κ�  :
 *            - ��� ������ �Ӽ����߰� 
 *			  - ������ Ÿ�ӿ� ������
 *              btnOk �� DialogResult ���� Yes���� Noneó�� ��
 *            - checkRowTotal(..)
 *              initDataTable(..)
 *              initZip(..)
 *              setZip(..)
 *              btnOk_Click(..)
 *              btnSearch_Click(..)
 *              SearchContentsList(..) Overload..
 *              ContentsForm_Load(..)
 * ��������  :           
				- �����ȣ ���� ���ð��� �ϵ��� ��� �߰�
				  �ִ� 100���� �����ȣ�� �����ϵ��� ��.
 * -------------------------------------------------------    
 
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

namespace AdManagerClient.Common
{
	/// <summary>
	/// ContentsForm�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ZipNoForm : System.Windows.Forms.Form
	{
        #region ��������� ��ü �� ����
        // �ý��� ���� : ȭ�����
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        // ����� ������
        ZipCodeModel	modelData	= new ZipCodeModel();
        CurrencyManager cm			= null;
        DataTable       dt			= null;
        #endregion
		
		#region [E_01] ���� �߰� �׸��...
		
		/// <summary>
		/// �ִ� �����ȣ ���� ���� ��(default 100)
		/// </summary>
		private int maxCount = 100;
		
		/// <summary>
		/// ������ �����ȣ ��
		/// </summary>
		private int existCount = 0;
		
		/// <summary>
		/// üũ �� �����ȣ ���
		/// </summary>
		private DataTable dtList = null;
		
		/// <summary>
		/// �ű��߰�����(true:�ű�)
		/// </summary>
		private bool isNew = true;

		/// <summary>
		/// �����ȣ �ܼ� ��ȸ(������ ��Ī:true..else..�ű� or �߰�)
		/// </summary>
		private bool isReadOnly  = false;

		/// <summary>
		/// �����ȣ �ű� �߰�����(true:�ű� else �����Ϳ� �߰�)
		/// </summary>
		public bool IsNewZip
		{
			set{ isNew = value;}
		}

		
		/// <summary>
		/// ���õ� �����ȣ ����Ʈ ��
		/// </summary>
		public int ExistZipCount
		{
			set{ existCount = value;}
			get{ return existCount;}
		}


		/// <summary>
		/// ������ �����ȣ Set/Get
		/// </summary>
		public DataTable ZipCodes
		{
			get{return dtList;}
			
			set
			{
				dtList = value;
				initZip(); // grid�� �����ȣ ��Ī�ϱ�

				bindingZip();
			}
		}


		/// <summary>
		/// �� ���õ� �����ȣ ��Ī�۾�����(true:��Ī��, else �ű� or �߰�)
		/// </summary>
		public bool IsReadOnly
		{
			set{isReadOnly = value;}
		}
		#endregion

        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Windows.Forms.Panel pnlGrid;
		private System.Data.DataView dvZip;
		private AdManagerClient.Common.ZipCodeDs zipCodeDs1;
		private System.Windows.Forms.Panel pnlOld;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlSearch;
		private System.Windows.Forms.Label lblEx;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Windows.Forms.Panel pnlNew;
		private Janus.Windows.GridEX.GridEX grdExZipList;
		private Janus.Windows.GridEX.GridEX grdOld;
		/// <summary>
		/// �ʼ� �����̳� �����Դϴ�.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ZipNoForm()
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
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
            Janus.Windows.GridEX.GridEXLayout grdExZipList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZipNoForm));
            Janus.Windows.GridEX.GridEXLayout grdOld_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdOld_Layout_1 = new Janus.Windows.GridEX.GridEXLayout();
            this.dvZip = new System.Data.DataView();
            this.zipCodeDs1 = new AdManagerClient.Common.ZipCodeDs();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.pnlNew = new System.Windows.Forms.Panel();
            this.grdExZipList = new Janus.Windows.GridEX.GridEX();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblEx = new System.Windows.Forms.Label();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.pnlOld = new System.Windows.Forms.Panel();
            this.grdOld = new Janus.Windows.GridEX.GridEX();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.dvZip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zipCodeDs1)).BeginInit();
            this.pnlGrid.SuspendLayout();
            this.pnlNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExZipList)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlOld.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOld)).BeginInit();
            this.pnlBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // dvZip
            // 
            this.dvZip.AllowDelete = false;
            this.dvZip.AllowEdit = false;
            this.dvZip.AllowNew = false;
            this.dvZip.Table = this.zipCodeDs1.SystemZip;
            // 
            // zipCodeDs1
            // 
            this.zipCodeDs1.DataSetName = "ZipCodeDs";
            this.zipCodeDs1.Locale = new System.Globalization.CultureInfo("en-US");
            this.zipCodeDs1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlGrid.Controls.Add(this.pnlNew);
            this.pnlGrid.Controls.Add(this.pnlOld);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 0);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(620, 504);
            this.pnlGrid.TabIndex = 21;
            // 
            // pnlNew
            // 
            this.pnlNew.Controls.Add(this.grdExZipList);
            this.pnlNew.Controls.Add(this.pnlSearch);
            this.pnlNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNew.Location = new System.Drawing.Point(0, 184);
            this.pnlNew.Name = "pnlNew";
            this.pnlNew.Size = new System.Drawing.Size(620, 320);
            this.pnlNew.TabIndex = 27;
            // 
            // grdExZipList
            // 
            this.grdExZipList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExZipList.AlternatingColors = true;
            this.grdExZipList.AutomaticSort = false;
            this.grdExZipList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExZipList.DataSource = this.dvZip;
            this.grdExZipList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExZipList.EmptyRows = true;
            this.grdExZipList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExZipList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExZipList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExZipList.Font = new System.Drawing.Font("���� ���", 8.25F);
            this.grdExZipList.FrozenColumns = 4;
            this.grdExZipList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExZipList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExZipList.GroupByBoxVisible = false;
            this.grdExZipList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExZipList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExZipList_Layout_0.DataSource = this.dvZip;
            grdExZipList_Layout_0.IsCurrentLayout = true;
            grdExZipList_Layout_0.Key = "bae";
            grdExZipList_Layout_0.LayoutString = resources.GetString("grdExZipList_Layout_0.LayoutString");
            this.grdExZipList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExZipList_Layout_0});
            this.grdExZipList.Location = new System.Drawing.Point(0, 43);
            this.grdExZipList.Name = "grdExZipList";
            this.grdExZipList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExZipList.Size = new System.Drawing.Size(620, 277);
            this.grdExZipList.TabIndex = 27;
            this.grdExZipList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExZipList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.lblEx);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(620, 43);
            this.pnlSearch.TabIndex = 24;
            // 
            // lblEx
            // 
            this.lblEx.Location = new System.Drawing.Point(219, 10);
            this.lblEx.Name = "lblEx";
            this.lblEx.Size = new System.Drawing.Size(261, 16);
            this.lblEx.TabIndex = 3;
            this.lblEx.Text = "��) ���̸� �Է½� \'������\', �ǹ��� �Է½� oo����";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(508, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(8, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(200, 22);
            this.ebSearchKey.TabIndex = 1;
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // pnlOld
            // 
            this.pnlOld.BackColor = System.Drawing.SystemColors.Control;
            this.pnlOld.Controls.Add(this.grdOld);
            this.pnlOld.Controls.Add(this.label1);
            this.pnlOld.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOld.Location = new System.Drawing.Point(0, 0);
            this.pnlOld.Name = "pnlOld";
            this.pnlOld.Size = new System.Drawing.Size(620, 184);
            this.pnlOld.TabIndex = 23;
            // 
            // grdOld
            // 
            this.grdOld.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdOld.AlternatingColors = true;
            this.grdOld.AutomaticSort = false;
            this.grdOld.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdOld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdOld.EmptyRows = true;
            this.grdOld.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdOld.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdOld.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdOld.Font = new System.Drawing.Font("���� ���", 8.25F);
            this.grdOld.FrozenColumns = 4;
            this.grdOld.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdOld.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdOld.GroupByBoxVisible = false;
            this.grdOld.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdOld.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdOld_Layout_0.IsCurrentLayout = true;
            grdOld_Layout_0.Key = "bae";
            grdOld_Layout_0.LayoutString = resources.GetString("grdOld_Layout_0.LayoutString");
            grdOld_Layout_1.Key = "Layout1";
            this.grdOld.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdOld_Layout_0,
            grdOld_Layout_1});
            this.grdOld.Location = new System.Drawing.Point(0, 24);
            this.grdOld.Name = "grdOld";
            this.grdOld.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdOld.Size = new System.Drawing.Size(620, 160);
            this.grdOld.TabIndex = 23;
            this.grdOld.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdOld.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(620, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ÿ���� �� �����ȣ �����Դϴ�.!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 504);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(620, 42);
            this.pnlBtn.TabIndex = 20;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.Location = new System.Drawing.Point(314, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 24);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "�� ��";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(202, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 24);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ȯ ��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ZipNoForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(620, 546);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlBtn);
            this.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ZipNoForm";
            this.Text = "�����ȣ �˻�";
            this.Load += new System.EventHandler(this.ContentsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvZip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zipCodeDs1)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            this.pnlNew.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExZipList)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlOld.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdOld)).EndInit();
            this.pnlBtn.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        private void ContentsForm_Load(object sender, System.EventArgs e)
        {
            dt  = ((DataView)grdExZipList.DataSource).Table;
            cm  = (CurrencyManager)this.BindingContext[ grdExZipList.DataSource];
            cm.PositionChanged += new EventHandler(cmContents_PositionChanged);
			ebSearchKey.Focus();
			initControls(); //[E_01]
        }

		private	string	keyCode = "";
        private string  keyText = "";

		/// <summary>
		/// mode �Ӽ��� ���� ��Ʈ�� visible ����[E_01]
		/// </summary>
		private void initControls()
		{
			// �б� �����̸� �˻� �Ұ�[E_01]
			if (isReadOnly)
			{
				btnSearch.Enabled = false;
				pnlNew.Visible = false;  // ��� ��Ʈ�� ����
				pnlOld.Dock = DockStyle.Fill;
			}
			else
			{
				if (isNew)
					pnlOld.Visible  = false; // �ű��߰��ÿ� Ȯ�ο� �б� ���� ��Ʈ�� ���� 
			}
		}

        private void cmContents_PositionChanged(object sender, EventArgs e)
        {
            int currRow = cm.Position;

            if( currRow > -1 )
            {
                keyCode = dt.Rows[currRow]["ZipCode"].ToString();
                keyText = dt.Rows[currRow]["AddrFull"].ToString();
            }
        }

        
		/// <summary>
        /// �������� ã�ƿ´�
        /// </summary>
        private void SearchContentsList()
        {
            try
            {
				modelData.Init();
				zipCodeDs1.SystemZip.Clear();
                modelData.SearchKey	= ebSearchKey.Text.Trim();

				if ( modelData.SearchKey.Length > 0 )
				{					
					new Common.ZipCodeManager( systemModel, commonModel).GetZipCodeList( modelData );
					if (modelData.ResultCD.Equals("0000"))
					{
						Utility.SetDataTable(zipCodeDs1.SystemZip, modelData.DsAddr);
					}
				}
				else
				{
					MessageBox.Show("�˻������� �Է��Ͻʽÿ�.","�����ȣ ã��", MessageBoxButtons.OK, MessageBoxIcon.Information );
					ebSearchKey.Focus();
					return;				
				}
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("�����ȣ ã�� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("�����ȣ ã�� ��ȸ����",new string[] {"",ex.Message});
            }
        }

		

		 
		/// <summary>
		/// �������� ã�ƿ´�(�����ȣ ��3�ڸ� �⺻+ �ɼ�(�� �̸��˻�)[E_01]
		/// </summary>
		private void SearchContentsList(string preZip)
		{
			try
			{
				modelData.Init();
				zipCodeDs1.SystemZip.Clear();
				modelData.SearchKey	= ebSearchKey.Text.Trim(); // �� �̸�
				modelData.SearchZip = preZip;
				
				new Common.ZipCodeManager( systemModel, commonModel).GetZipPreCodeList( modelData );

				if (modelData.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(zipCodeDs1.SystemZip, modelData.DsAddr);
				}				
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�����ȣ ã�� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�����ȣ ã�� ��ȸ����",new string[] {"",ex.Message});
			}
		}

		/// <summary>
		/// ������ Ÿ�ټ����� �����ȣ �����ͼ� ��Ʈ�� ���ε�...
		/// </summary>
		/// <param name="zip"></param>
		private void SearchReadOnly(string zip)
		{
			try
			{
				modelData.Init();
				zipCodeDs1.SystemZip.Clear();				
				modelData.SearchZip = zip;
				
				// Ȯ�ο�(read only)-Ÿ�������� ��ϵ� �����ȣ�� �˻�				
				new Common.ZipCodeManager( systemModel, commonModel).GetZipIncludeList( modelData );					
				grdOld.DataSource = modelData.DsAddr.Tables[0];												
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�����ȣ ã�� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�����ȣ ã�� ��ȸ����",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// �����ȣ �˻� [E_01]
		/// </summary>		
        private void btnSearch_Click(object sender, System.EventArgs e)
        {            				
			SearchContentsList();			
        }

        
		/// <summary>
        /// �帣���� �̺�Ʈ ó����
        /// </summary>
        public event ZipCodeEventHandler	SelectZipCode;

        /// <summary>
        /// �帣���� �̺�Ʈ ó���Լ�
        /// </summary>
        private void Selected()
        {
			try
			{				

				if( SelectZipCode != null )
				{					
					ZipCodeEventArgs args = new ZipCodeEventArgs();
					args.ZipCode	= keyCode;
					args.ZipAddr	= keyText;
					SelectZipCode( this, args );
				}

				this.Close();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}            
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
			// [E_01]
            //this.Selected();
			setZip();
			
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
			this.DialogResult = DialogResult.No;
            this.Close();
        }

		private void grdExZipList_DoubleClick(object sender, System.EventArgs e)
		{
			// [E_01]
			//this.Selected();
		}

		
		
		/// <summary>
		/// ���� ��  ��ü ä�� �� üũ(E_01)
		/// </summary>
		private void checkRowTotal()
		{
			int selectedRows = 0;
			
			// ������ ��� �� �����ȣ �� üũ�� ��� ���� ���Խ�Ŵ(�ű԰� �ƴ� �߰��� ���)
			if (isReadOnly)			
				selectedRows = grdOld.GetCheckedRows().Length;
			
			selectedRows += grdExZipList.GetCheckedRows().Length;

			if (selectedRows > maxCount)
			{
				throw new Exception( string.Format("������ �����ȣ�� �� {0}���� �ʰ��� �� �����ϴ�.!" ,maxCount) );
			}
		}
		
		
		/// <summary>
		/// TALBE ����[E_01]
		/// </summary>
		private void initDataTable()
		{
			if (dtList == null)
			{
				dtList = new DataTable("Zip");
				dtList.Columns.Add("ZipCode" ,typeof(string));						
			}
		}

		
		/// <summary>
		/// �����ȣ �� ���õ� �׸��� Check-in ó�� ��.[E_01]
		/// </summary>
		private void initZip()
		{			
			string groupZip = "";
			try
			{				
				initDataTable();

				if (dtList != null)
				{
					if (isNew == false) // Ÿ���� ���� �� �����ȣ //�ű� �߰� �ƴϸ� ������ ����
					{
						foreach(DataRow row in dtList.Rows)
						{
							// systemZip �� ��ϵ� ������ ���Ŀ� �°� ����
							groupZip += "'" + row["ZipCode"].ToString().Substring(0,3)
								            + "-"
								            + row["ZipCode"].ToString().Substring(3,3)
								            + "',";
						}
						// �������� ������ ����(,)
						if (groupZip.Length > 6)
							groupZip = groupZip.Substring(0, groupZip.Length -1);
						
						// �˻�
						SearchReadOnly(groupZip);
					}

					#region ������ Ÿ�ټ����� �����ȣ ��Ʈ�� �и� ó���ϹǷ� ���ʿ�...
					/*
					if (isReadOnly == false)
					{
						groupZip = "";
						// ����Ʈ���� �����ȣ ��3�ڸ��� ó���ؼ�
						// �����ȣ ����� ������ �� ��Īó�� ��.
						// why? �����ȣ�� �ʹ� ���Ƽ�(5��)
						foreach(DataRow row in dtList.Rows)
						{
							if (oldZip == "")
							{
								oldZip =  row["ZipCode"].ToString().Trim().Substring(0,3);
								groupZip += oldZip + ",";
							}
							else
							{
								newZip =  row["ZipCode"].ToString().Trim().Substring(0,3);
								if (oldZip != newZip)
								{
									groupZip += newZip + ",";
									oldZip = newZip;
								}
							}
						}

						// �ߺ��Ǵ� ���� ������ �����ȣ �� 3�ڸ� ��
						groupZip = groupZip.Substring(0, groupZip.Length - 1);
						// �˻�
						SearchContentsList(groupZip);
					}					
					*/					
					#endregion

					
					
				}// end if
				
			}
			catch(Exception)
			{
				if (dtList != null)
				{
					dtList.Clear();
					dtList.Dispose();
					dtList = null;
				}
			}
		
		}


		/// <summary>
		/// �� Ÿ�� ������ �����ȣ üũ�ڽ� ���ε� ó��
		/// </summary>
		private void bindingZip()
		{
			string oldZip = "";
			string gridZip = "";
			try
			{
				foreach(DataRow row in dtList.Rows)
				{
					oldZip =  row["ZipCode"].ToString().Trim();
					// ���� �����ȣ �׸��忡 üũ�� ó��(��� �׸���)		
					foreach(Janus.Windows.GridEX.GridEXRow grow in grdOld.GetRows())
					{
						gridZip = grow.Cells["ZipCode"].Value.ToString().Trim().Replace("-","");						
						if (oldZip.Equals(gridZip))						
							grow.Cells.Row.IsChecked = true;						
					}

					#region ������ Ÿ�ټ����� �����ȣ ��Ʈ�� �и� ó���ϹǷ� ���ʿ�...
					/*						
						if (isReadOnly == false) // �б� ���¿��� ó�� �� �ʿ�(���� �Ǵ� �׸��̹Ƿ�)
						{
							foreach(Janus.Windows.GridEX.GridEXRow grow in grdExZipList.GetRows())
							{
								gridZip = grow.Cells["ZipCode"].Value.ToString().Trim().Replace("-","");
						
								if (oldZip.Equals(gridZip))
								{
									//grow.Cells.Row.IsChecked = true;
									
									
								}
							}
						}
						*/
					#endregion
				}
			}
			catch(Exception)
			{
			}
		}



		/// <summary>
		/// �����ȣ ���� �� ���̺� ����[E_01]
		/// </summary>
		private void setZip()
		{
			try
			{
				checkRowTotal(); // üũ �� üũ
				
				initDataTable();
				dtList.Clear();
				
				// ���� ���� �߰��ϴ� ��쿡�� ����
				if (isNew == false && isReadOnly == false)
				{
					if (grdOld.GetCheckedRows().Length > 0)
					{										
						foreach(Janus.Windows.GridEX.GridEXRow row in grdOld.GetCheckedRows())
						{
							DataRow nrow    = dtList.NewRow();
							nrow["ZipCode"] = row.Cells["ZipCode"].Value.ToString().Replace("-","");						
							dtList.Rows.Add(nrow);
						}
					}
				}
				
				if (isReadOnly == false)
				{
					if (grdExZipList.GetCheckedRows().Length > 0)
					{										
						foreach(Janus.Windows.GridEX.GridEXRow row in grdExZipList.GetCheckedRows())
						{
							DataRow nrow    = dtList.NewRow();
							nrow["ZipCode"] = row.Cells["ZipCode"].Value.ToString().Replace("-","");						
							dtList.Rows.Add(nrow);
						}
					}
				}

				this.DialogResult = DialogResult.Yes;
				this.Close();
			}
			catch(Exception ex)
			{				
				MessageBox.Show(ex.Message, "����" ,MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

    }
}
