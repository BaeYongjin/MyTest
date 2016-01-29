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
    /// StatisticsDaily_pForm�� ���� ��� �����Դϴ�.
    /// </summary>
    public class ChannelJump_SearchAdPopForm : System.Windows.Forms.Form
    {

		#region ��������� ��ü �� ����

		// ������
		private ChannelJumpControl Opener = null;
        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Windows.Forms.Panel panel1;
		private Janus.Windows.GridEX.GridEX grdExAdPopList;
		private System.Data.DataView dvAdPopList;
		private AdManagerClient.ChannelJumpDs channelJumpDs;			// �˾�ȣ��� ��ü��Ʈ


		#endregion

		#region ������ �� �Ҹ���
       
        /// <summary>
        /// ������ �Ѱܾ� �� ��
        /// </summary>
        /// <param name="sender"></param>
        public ChannelJump_SearchAdPopForm(ChannelJumpControl sender)
        {
            //
            // Windows Form �����̳� ������ �ʿ��մϴ�.
            //
            InitializeComponent();

            //
            
            //
            
            Opener = sender;
        }


        /// <summary>
        /// ��� ���� ��� ���ҽ��� �����մϴ�.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
//                if(components != null)
//                {
//                    components.Dispose();
//                }
            }
            base.Dispose( disposing );
        }


        #endregion

        #region �̺�Ʈ�ڵ鷯
        public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯

        #endregion

        #region ��������� ��ü �� ����

		public string keyType = "";
				
        // �ý��� ���� : ȭ�����
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;


        // ����� ������
        ChannelJumpModel channelJumpModel  = new ChannelJumpModel();	// ä������������

        // ȭ��ó���� ����
        CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
        DataTable       dt        = null;

		
        #endregion

        #region Windows Form �����̳ʿ��� ������ �ڵ�
        /// <summary>
        /// �����̳� ������ �ʿ��� �޼����Դϴ�.
        /// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
        /// </summary>
        private void InitializeComponent()
        {
            Janus.Windows.GridEX.GridEXLayout grdExAdPopList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelJump_SearchAdPopForm));
            this.dvAdPopList = new System.Data.DataView();
            this.channelJumpDs = new AdManagerClient.ChannelJumpDs();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExAdPopList = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.dvAdPopList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelJumpDs)).BeginInit();
            this.pnlBtn.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdPopList)).BeginInit();
            this.SuspendLayout();
            // 
            // dvAdPopList
            // 
            this.dvAdPopList.Table = this.channelJumpDs.AdPopList;
            // 
            // channelJumpDs
            // 
            this.channelJumpDs.DataSetName = "ChannelJumpDs";
            this.channelJumpDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.channelJumpDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 420);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(680, 42);
            this.pnlBtn.TabIndex = 17;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(344, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 24);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "�� ��";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(232, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 24);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ȯ ��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.grdExAdPopList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(680, 420);
            this.panel1.TabIndex = 18;
            // 
            // grdExAdPopList
            // 
            this.grdExAdPopList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExAdPopList.AlternatingColors = true;
            this.grdExAdPopList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExAdPopList.DataSource = this.dvAdPopList;
            this.grdExAdPopList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExAdPopList.EmptyRows = true;
            this.grdExAdPopList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExAdPopList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExAdPopList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExAdPopList.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdExAdPopList.FrozenColumns = 2;
            this.grdExAdPopList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExAdPopList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExAdPopList.GroupByBoxVisible = false;
            this.grdExAdPopList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExAdPopList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExAdPopList_Layout_0.DataSource = this.dvAdPopList;
            grdExAdPopList_Layout_0.IsCurrentLayout = true;
            grdExAdPopList_Layout_0.Key = "bae";
            grdExAdPopList_Layout_0.LayoutString = resources.GetString("grdExAdPopList_Layout_0.LayoutString");
            this.grdExAdPopList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExAdPopList_Layout_0});
            this.grdExAdPopList.Location = new System.Drawing.Point(0, 0);
            this.grdExAdPopList.Name = "grdExAdPopList";
            this.grdExAdPopList.Size = new System.Drawing.Size(680, 420);
            this.grdExAdPopList.TabIndex = 1;
            this.grdExAdPopList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExAdPopList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExAdPopList.DoubleClick += new System.EventHandler(this.grdExAdPopList_DoubleClick);
            // 
            // ChannelJump_SearchAdPopForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(680, 462);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBtn);
            this.Name = "ChannelJump_SearchAdPopForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "�˾������˻�";
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvAdPopList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelJumpDs)).EndInit();
            this.pnlBtn.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdPopList)).EndInit();
            this.ResumeLayout(false);

		}
        #endregion

        #region ��Ʈ�� �ε�
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // �����Ͱ����� ��ü����
            dt = ((DataView)grdExAdPopList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExAdPopList.DataSource]; 
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            // ��Ʈ�� �ʱ�ȭ
            InitControl();	
        }

        #endregion

        #region ��Ʈ�� �ʱ�ȭ
		private void InitControl()
		{
			SearchAdPopList();
		}

        #endregion

        #region �׼�ó�� �޼ҵ�

        /// <summary>
        /// �׸����� Row�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if(grdExAdPopList.RowCount > 0)
            {
                //                SetDetailText();
                //                InitButton();
            }
        }

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			SelectAdPopList();
			this.Close();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdExAdPopList_DoubleClick(object sender, System.EventArgs e)
		{
			SelectAdPopList();
			this.Close();		
		}

        #endregion

        #region ó���޼ҵ�

        /// <summary>
        /// �������� ��ȸ
        /// </summary>
        private void SearchAdPopList()
        {
            try
            {
                //�˻� ���� ���� �ʱ�ȭ ���ش�.
                channelJumpModel.Init();
			
				channelJumpModel.Type      = keyType;
                // ���� ��� ��� ���񽺸� ȣ���Ѵ�.
                new ChannelJumpManager(systemModel,commonModel).GetAdPopList(channelJumpModel);

                if (channelJumpModel.ResultCD.Equals("0000"))
                {
					if(channelJumpModel.ResultCnt > 0)
					{
						Utility.SetDataTable(channelJumpDs.AdPopList, channelJumpModel.AdPopListDataSet);				
					}

					if(Convert.ToInt32(channelJumpModel.ResultCnt) > 0) cm.Position = 0;
					grdExAdPopList.EnsureVisible();

                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("�˾����� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("�˾����� ��ȸ����",new string[] {"",ex.Message});
            }
        }


		/// <summary>
		/// ���� ����
		/// </summary>
		private void SelectAdPopList()
		{

			int curRow = cm.Position;
			if(curRow < 0) return;	// �����Ͱ� ������ �������� �ʴ´�.

			string PopID        = dt.Rows[curRow]["nid"].ToString();			
			string Title        = dt.Rows[curRow]["ntitle"].ToString();			
	
			Opener.SetPopup(PopID, Title);
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

    }
}