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
    public class ChannelJump_SearchChannelForm : System.Windows.Forms.Form
    {

		#region ��������� ��ü �� ����

		public string keyMediaCode = "";
		// ������
		private ChannelJumpControl Opener = null;

        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Windows.Forms.Panel panel1;
//        private System.ComponentModel.IContainer components;
        private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Data.DataView dvChannel;
		private Janus.Windows.GridEX.GridEX grdExChannelList;
		private AdManagerClient.ChannelJumpDs channelJumpDs;			// �˾�ȣ��� ��ü��Ʈ


		#endregion

		#region ������ �� �Ҹ���
        public ChannelJump_SearchChannelForm()
        {
            //
            // Windows Form �����̳� ������ �ʿ��մϴ�.
            //
            InitializeComponent();

            //
            
            //
        }

        /// <summary>
        /// ������ �Ѱܾ� �� ��
        /// </summary>
        /// <param name="sender"></param>
        public ChannelJump_SearchChannelForm(ChannelJumpControl sender)
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

        // �ý��� ���� : ȭ�����
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;


        // ����� ������
        ChannelJumpModel channelJumpModel  = new ChannelJumpModel();	// ä������������

        // ȭ��ó���� ����
        bool IsNewSearchKey		  = true;					// �˻����Է� ����
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
            Janus.Windows.GridEX.GridEXLayout grdExChannelList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelJump_SearchChannelForm));
            this.dvChannel = new System.Data.DataView();
            this.channelJumpDs = new AdManagerClient.ChannelJumpDs();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExChannelList = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelJumpDs)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelList)).BeginInit();
            this.SuspendLayout();
            // 
            // dvChannel
            // 
            this.dvChannel.Table = this.channelJumpDs.ChannelSearch;
            // 
            // channelJumpDs
            // 
            this.channelJumpDs.DataSetName = "ChannelJumpDs";
            this.channelJumpDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.channelJumpDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(648, 40);
            this.pnlSearch.TabIndex = 3;
            this.pnlSearch.TabStop = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(536, 8);
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
            this.ebSearchKey.Size = new System.Drawing.Size(168, 22);
            this.ebSearchKey.TabIndex = 1;
            this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 420);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(648, 42);
            this.pnlBtn.TabIndex = 17;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(328, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 24);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "�� ��";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(216, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 24);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Ȯ ��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.grdExChannelList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(648, 380);
            this.panel1.TabIndex = 18;
            // 
            // grdExChannelList
            // 
            this.grdExChannelList.AlternatingColors = true;
            this.grdExChannelList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExChannelList.DataSource = this.dvChannel;
            this.grdExChannelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExChannelList.EmptyRows = true;
            this.grdExChannelList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExChannelList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExChannelList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChannelList.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdExChannelList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExChannelList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExChannelList.GroupByBoxVisible = false;
            this.grdExChannelList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExChannelList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExChannelList_Layout_0.DataSource = this.dvChannel;
            grdExChannelList_Layout_0.IsCurrentLayout = true;
            grdExChannelList_Layout_0.Key = "bae";
            grdExChannelList_Layout_0.LayoutString = resources.GetString("grdExChannelList_Layout_0.LayoutString");
            this.grdExChannelList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExChannelList_Layout_0});
            this.grdExChannelList.Location = new System.Drawing.Point(0, 0);
            this.grdExChannelList.Name = "grdExChannelList";
            this.grdExChannelList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExChannelList.Size = new System.Drawing.Size(648, 380);
            this.grdExChannelList.TabIndex = 4;
            this.grdExChannelList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExChannelList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExChannelList.DoubleClick += new System.EventHandler(this.grdExChannelList_DoubleClick);
            // 
            // ChannelJump_SearchChannelForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(648, 462);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.pnlSearch);
            this.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "ChannelJump_SearchChannelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ä�ΰ˻�";
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelJumpDs)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelList)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region ��Ʈ�� �ε�
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // �����Ͱ����� ��ü����
            dt = ((DataView)grdExChannelList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExChannelList.DataSource]; 
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            // ��Ʈ�� �ʱ�ȭ
            InitControl();	
        }

        #endregion

        #region ��Ʈ�� �ʱ�ȭ
        private void InitControl()
        {
 
		}

        #endregion

        #region ������� �׼�ó�� �޼ҵ�

        /// <summary>
        /// �׸����� Row�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if(grdExChannelList.RowCount > 0)
            {
                //                SetDetailText();
                //                InitButton();
            }
        }

        /// <summary>
        /// ��ȸ��ư Ŭ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            SearchChannel();
        }

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
            if(e.KeyCode == Keys.Enter)
            {
                SearchChannel();
            }
        }

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			SelectChannel();
			this.Close();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdExChannelList_DoubleClick(object sender, System.EventArgs e)
		{
			SelectChannel();
			this.Close();		
		}

        #endregion

        #region ó���޼ҵ�

        /// <summary>
        /// ä�ΰ˻�
        /// </summary>
        private void SearchChannel()
        {
            StatusMessage("ä�� ������ �˻��մϴ�.");

			if(IsNewSearchKey || ebSearchKey.Text.Trim().Length == 0) 
			{
				MessageBox.Show("�˻�� �Է��� �ֽñ� �ٶ��ϴ�.","ä�ΰ˻�", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

            try
            {
                //�˻� ���� ���� �ʱ�ȭ ���ش�.
                channelJumpModel.Init();
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                channelJumpModel.SearchMediaCode      = keyMediaCode;
    
                if(IsNewSearchKey)
                {
                    channelJumpModel.SearchKey = "";
                }
                else
                {
                    channelJumpModel.SearchKey  = ebSearchKey.Text;
                }

                // ä�ΰ˻� ���񽺸� ȣ���Ѵ�.
                new ChannelJumpManager(systemModel,commonModel).GetChannelList(channelJumpModel);

                if (channelJumpModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(channelJumpDs.ChannelSearch, channelJumpModel.ChannelListDataSet);				
                    StatusMessage(channelJumpModel.ResultCnt + "���� ä�� ������ ��ȸ�Ǿ����ϴ�.");
					if(Convert.ToInt32(channelJumpModel.ResultCnt) > 0) cm.Position = 0;
					grdExChannelList.EnsureVisible();

                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("ä�ΰ˻� ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("ä�ΰ˻� ����",new string[] {"",ex.Message});
            }
        }


		/// <summary>
		/// ���� ����
		/// </summary>
		private void SelectChannel()
		{

			int curRow = cm.Position;
			if(curRow < 0) return;	// �����Ͱ� ������ �������� �ʴ´�.

			string GenreCode      = dt.Rows[curRow]["GenreCode"].ToString();			
			string GenreName      = dt.Rows[curRow]["GenreName"].ToString();			
			string ChannelNo      = dt.Rows[curRow]["ChannelNo"].ToString();			
			string ChannelName    = dt.Rows[curRow]["ChannelName"].ToString();			
	
			Opener.SetChannel(GenreCode, GenreName, ChannelNo, ChannelName);
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