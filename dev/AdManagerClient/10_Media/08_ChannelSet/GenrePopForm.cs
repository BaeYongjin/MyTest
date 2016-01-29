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
	/// GenrePopForm�� ���� ��� �����Դϴ�.
	/// </summary>
	/// 
    
	public class GenrePopForm : System.Windows.Forms.Form
	{

		#region �̺�Ʈ�ڵ鷯
		public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
		#endregion
			
		#region ��������� ��ü �� ����

		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;

		// ����� ������
		ChannelSetModel channelSetModel  = new ChannelSetModel();	// ������������
				
		ChannelSetControl GenreCtl = null;

		// ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����
		CurrencyManager ccm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dtt        = null;
		
		#endregion



		private System.Windows.Forms.Panel panel4;
		private System.Data.DataView dvGenre;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private AdManagerClient._10_Media._08_ChannelSet.ChannelSetDs GenreDs;
		private Janus.Windows.GridEX.GridEX grdExGenreList;
//		private System.ComponentModel.IContainer components;

		/// <summary>
		/// ������ �Ѱܾ� �� ��
		/// </summary>
		/// <param name="sender"></param>
		public GenrePopForm(ChannelSetControl sender)
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			
			//
            
			GenreCtl = sender;
		}

		/// <summary>
		/// �Ϲݻ����
		/// </summary>
		public GenrePopForm()
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			
			//
            
			GenreCtl = null;
		}

		/// <summary>
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				/*
				if(components != null)
				{
					components.Dispose();
				}
				*/
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
			Janus.Windows.GridEX.GridEXLayout gridEXLayout1 = new Janus.Windows.GridEX.GridEXLayout();
			this.dvGenre = new System.Data.DataView();
			this.GenreDs = new AdManagerClient._10_Media._08_ChannelSet.ChannelSetDs();
			this.panel4 = new System.Windows.Forms.Panel();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.pnlBtn = new System.Windows.Forms.Panel();
			this.btnClose = new Janus.Windows.EditControls.UIButton();
			this.btnOk = new Janus.Windows.EditControls.UIButton();
			this.grdExGenreList = new Janus.Windows.GridEX.GridEX();
			((System.ComponentModel.ISupportInitialize)(this.dvGenre)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.GenreDs)).BeginInit();
			this.panel4.SuspendLayout();
			this.pnlBtn.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).BeginInit();
			this.SuspendLayout();
			// 
			// dvGenre
			// 
			this.dvGenre.Table = this.GenreDs.Genres;
			// 
			// GenreDs
			// 
			this.GenreDs.DataSetName = "ChannelSetDs";
			this.GenreDs.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.White;
			this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel4.Controls.Add(this.ebSearchKey);
			this.panel4.Controls.Add(this.btnSearch);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(320, 32);
			this.panel4.TabIndex = 1;
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.ebSearchKey.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.ebSearchKey.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ebSearchKey.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ebSearchKey.Location = new System.Drawing.Point(8, 5);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(184, 21);
			this.ebSearchKey.TabIndex = 1;
			this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// btnSearch
			// 
			this.btnSearch.BackColor = System.Drawing.Color.White;
			this.btnSearch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnSearch.Location = new System.Drawing.Point(208, 3);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 2;
			this.btnSearch.Text = "�� ȸ";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// pnlBtn
			// 
			this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
			this.pnlBtn.Controls.Add(this.btnClose);
			this.pnlBtn.Controls.Add(this.btnOk);
			this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtn.Location = new System.Drawing.Point(0, 382);
			this.pnlBtn.Name = "pnlBtn";
			this.pnlBtn.Size = new System.Drawing.Size(320, 40);
			this.pnlBtn.TabIndex = 16;
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.SystemColors.Control;
			this.btnClose.Location = new System.Drawing.Point(168, 8);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(64, 23);
			this.btnClose.TabIndex = 5;
			this.btnClose.Text = "�ݱ�";
			this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.SystemColors.Control;
			this.btnOk.Location = new System.Drawing.Point(88, 8);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(64, 23);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "Ȯ��";
			this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// grdExGenreList
			// 
			this.grdExGenreList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExGenreList.AlternatingColors = true;
			this.grdExGenreList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExGenreList.DataSource = this.dvGenre;
			this.grdExGenreList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExGenreList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExGenreList.EmptyRows = true;
			this.grdExGenreList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExGenreList.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.grdExGenreList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExGenreList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExGenreList.GroupByBoxVisible = false;
			this.grdExGenreList.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
			this.grdExGenreList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			gridEXLayout1.DataSource = this.dvGenre;
			gridEXLayout1.IsCurrentLayout = true;
			gridEXLayout1.Key = "bae";
			gridEXLayout1.LayoutString = "<GridEXLayoutData><RootTable><Caption>Genre</Caption><Columns Collection=\"true\"><" +
				"Column0 ID=\"GenreCode\"><Caption>�帣�ڵ�</Caption><DataMember>GenreCode</DataMember>" +
				"<EditType>NoEdit</EditType><Key>GenreCode</Key><Position>0</Position><Width>87</" +
				"Width></Column0><Column1 ID=\"GenreName\"><Caption>�帣��</Caption><DataMember>GenreN" +
				"ame</DataMember><EditType>NoEdit</EditType><Key>GenreName</Key><Position>1</Posi" +
				"tion><Width>216</Width></Column1></Columns><GroupCondition ID=\"\" /><Key>Genre</K" +
				"ey><SortKeys Collection=\"true\"><SortKey0 ID=\"SortKey0\"><ColIndex>1</ColIndex></S" +
				"ortKey0></SortKeys></RootTable><RowWithErrorsFormatStyle><PredefinedStyle>RowWit" +
				"hErrorsFormatStyle</PredefinedStyle></RowWithErrorsFormatStyle><LinkFormatStyle>" +
				"<PredefinedStyle>LinkFormatStyle</PredefinedStyle></LinkFormatStyle><CardCaption" +
				"FormatStyle><PredefinedStyle>CardCaptionFormatStyle</PredefinedStyle></CardCapti" +
				"onFormatStyle><GroupByBoxFormatStyle><PredefinedStyle>GroupByBoxFormatStyle</Pre" +
				"definedStyle></GroupByBoxFormatStyle><GroupByBoxInfoFormatStyle><PredefinedStyle" +
				">GroupByBoxInfoFormatStyle</PredefinedStyle></GroupByBoxInfoFormatStyle><GroupRo" +
				"wFormatStyle><PredefinedStyle>GroupRowFormatStyle</PredefinedStyle></GroupRowFor" +
				"matStyle><HeaderFormatStyle><PredefinedStyle>HeaderFormatStyle</PredefinedStyle>" +
				"<TextAlignment>Center</TextAlignment></HeaderFormatStyle><PreviewRowFormatStyle>" +
				"<PredefinedStyle>PreviewRowFormatStyle</PredefinedStyle></PreviewRowFormatStyle>" +
				"<RowFormatStyle><PredefinedStyle>RowFormatStyle</PredefinedStyle></RowFormatStyl" +
				"e><SelectedFormatStyle><PredefinedStyle>SelectedFormatStyle</PredefinedStyle></S" +
				"electedFormatStyle><SelectedInactiveFormatStyle><BackColor>Gold</BackColor><Pred" +
				"efinedStyle>SelectedInactiveFormatStyle</PredefinedStyle></SelectedInactiveForma" +
				"tStyle><GroupRowVisualStyle>Outlook2003</GroupRowVisualStyle><WatermarkImage /><" +
				"AlternatingColors>True</AlternatingColors><GridLineStyle>Solid</GridLineStyle><B" +
				"orderStyle>None</BorderStyle><Font>Tahoma, 8.25pt</Font><ControlStyle /><VisualS" +
				"tyle>Office2003</VisualStyle><AllowEdit>False</AllowEdit><EmptyRows>True</EmptyR" +
				"ows><FocusStyle>None</FocusStyle><GridLines>Vertical</GridLines><GroupByBoxVisib" +
				"le>False</GroupByBoxVisible><ScrollBars>Vertical</ScrollBars><TabKeyBehavior>Con" +
				"trolNavigation</TabKeyBehavior></GridEXLayoutData>";
			this.grdExGenreList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
																							 gridEXLayout1});
			this.grdExGenreList.Location = new System.Drawing.Point(0, 32);
			this.grdExGenreList.Name = "grdExGenreList";
			this.grdExGenreList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExGenreList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExGenreList.Size = new System.Drawing.Size(320, 350);
			this.grdExGenreList.TabIndex = 3;
			this.grdExGenreList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExGenreList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
			this.grdExGenreList.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.grdExGenreList_RowDoubleClick);
			// 
			// GenrePopForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(320, 422);
			this.Controls.Add(this.grdExGenreList);
			this.Controls.Add(this.pnlBtn);
			this.Controls.Add(this.panel4);
			this.Name = "GenrePopForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "�帣��ϰ˻�";
			this.Load += new System.EventHandler(this.GenrePopForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dvGenre)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.GenreDs)).EndInit();
			this.panel4.ResumeLayout(false);
			this.pnlBtn.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void GenrePopForm_Load(object sender, System.EventArgs e)
		{
            
			// �����Ͱ����� ��ü����
			dtt = ((DataView)grdExGenreList.DataSource).Table;  
			ccm = (CurrencyManager) this.BindingContext[grdExGenreList.DataSource]; 
			//cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ��Ʈ�� �ʱ�ȭ
			InitControl();
			SearchGenre();
		}
		#endregion

		#region ����� �׼�ó�� �޼ҵ�

		/// <summary>
		/// �˻��� ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// 
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
		
		/// <summary>
		/// �˻��� Ŭ�� 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
		{
			IsNewSearchKey = false;
		}

		private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				SearchGenre();
			}
		}

		#endregion

		#region ��Ʈ�� �ʱ�ȭ
		private void InitControl()
		{
													
		}
		
		#endregion
  
		#region ó���޼ҵ�

		/// <summary>
		/// ��������� ��ȸ
		/// </summary>
		private void SearchGenre()
		{
			StatusMessage("������ ������ ��ȸ�մϴ�.");

			try
			{
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				if(IsNewSearchKey)
				{
					channelSetModel.SearchKey = "";
				}
				else
				{
					channelSetModel.SearchKey  = ebSearchKey.Text;
				}

				// �����������ȸ ���񽺸� ȣ���Ѵ�.
				new ChannelSetManager(systemModel,commonModel).GetGenreList(channelSetModel);

				if (channelSetModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(GenreDs.Genres, channelSetModel.GenreDataSet);				
					StatusMessage(channelSetModel.ResultCnt + "���� ������ ������ ��ȸ�Ǿ����ϴ�.");
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("��������ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("��������ȸ����",new string[] {"",ex.Message});
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

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			string newKey = grdExGenreList.SelectedItems[0].GetRow().Cells["GenreCode"].Value.ToString();			
			string newKey_N = grdExGenreList.SelectedItems[0].GetRow().Cells["GenreName"].Value.ToString();			
			this.GenreCtl.GenreCode = newKey;				
			this.GenreCtl.GenreName = newKey_N;				
			this.Close();
		}
     


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchGenre();
		}

		private void grdExGenreList_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{						
			string newKey = grdExGenreList.SelectedItems[0].GetRow().Cells["GenreCode"].Value.ToString();			
			string newKey_N = grdExGenreList.SelectedItems[0].GetRow().Cells["GenreName"].Value.ToString();			
			this.GenreCtl.GenreCode = newKey;				
			this.GenreCtl.GenreName = newKey_N;				
			this.Close();
		}	
		
		
	}
   

	#endregion


}
