using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace AdManagerClient
{
	
	/// <summary>
	/// TimeTarget�� ���� ��� �����Դϴ�.
	/// </summary>
	public class TimeTarget : System.Windows.Forms.Form
	{
		
		private System.Windows.Forms.TreeListView tlvDay;
		private System.ComponentModel.IContainer components;

		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ImageList imgTree;
		private Janus.Windows.EditControls.UIButton btnConfirm;
		private Janus.Windows.EditControls.UIButton btnCancel;
		private System.Windows.Forms.TreeListView tlvEnd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		

		/// <summary>
		/// ����/�ָ� ���� ������ ������ ǥ��
		/// </summary>
		private int dayOfNum = 0;
		private System.Windows.Forms.Label lblWeekDay;
							
		private string setTimes = string.Empty;
        private Label lblMsg;

		/// <summary>
		/// Ÿ�� ���� �� �ð� ���ڿ�
		/// </summary>
		public string TargetingTime
		{
			set{setTimes = value;}	
			get{return setTimes;}
		}
		
		
		/// <summary>
		/// ����,�ָ��� ������ ������ ����
		/// </summary>
		public int DaysOfNumber
		{
			set{ dayOfNum = value;}
		}

        private string sTypeNm = string.Empty;

        public string STypeNm
        {
            set { sTypeNm = value; }
            get { return sTypeNm; }
        }
		

		public TimeTarget()
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			
			//
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
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer2 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeTarget));
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer3 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            this.tlvDay = new System.Windows.Forms.TreeListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgTree = new System.Windows.Forms.ImageList(this.components);
            this.tlvEnd = new System.Windows.Forms.TreeListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnConfirm = new Janus.Windows.EditControls.UIButton();
            this.btnCancel = new Janus.Windows.EditControls.UIButton();
            this.lblWeekDay = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tlvDay
            // 
            this.tlvDay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvDay.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            this.tlvDay.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            treeListViewItemCollectionComparer2.Column = 0;
            treeListViewItemCollectionComparer2.SortOrder = System.Windows.Forms.SortOrder.None;
            this.tlvDay.Comparer = treeListViewItemCollectionComparer2;
            this.tlvDay.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tlvDay.Location = new System.Drawing.Point(16, 48);
            this.tlvDay.Name = "tlvDay";
            this.tlvDay.Size = new System.Drawing.Size(176, 456);
            this.tlvDay.SmallImageList = this.imgTree;
            this.tlvDay.Sorting = System.Windows.Forms.SortOrder.None;
            this.tlvDay.TabIndex = 0;
            this.tlvDay.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "�ð�";
            this.columnHeader1.Width = 150;
            // 
            // imgTree
            // 
            this.imgTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTree.ImageStream")));
            this.imgTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imgTree.Images.SetKeyName(0, "");
            // 
            // tlvEnd
            // 
            this.tlvEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvEnd.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            this.tlvEnd.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            treeListViewItemCollectionComparer3.Column = 0;
            treeListViewItemCollectionComparer3.SortOrder = System.Windows.Forms.SortOrder.None;
            this.tlvEnd.Comparer = treeListViewItemCollectionComparer3;
            this.tlvEnd.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tlvEnd.Location = new System.Drawing.Point(232, 48);
            this.tlvEnd.Name = "tlvEnd";
            this.tlvEnd.Size = new System.Drawing.Size(176, 456);
            this.tlvEnd.SmallImageList = this.imgTree;
            this.tlvEnd.Sorting = System.Windows.Forms.SortOrder.None;
            this.tlvEnd.TabIndex = 1;
            this.tlvEnd.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "�ð�";
            this.columnHeader2.Width = 150;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConfirm.BackColor = System.Drawing.SystemColors.Window;
            this.btnConfirm.Location = new System.Drawing.Point(120, 514);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 24);
            this.btnConfirm.TabIndex = 212;
            this.btnConfirm.Text = "Ȯ ��";
            this.btnConfirm.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.Window;
            this.btnCancel.Location = new System.Drawing.Point(224, 514);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 213;
            this.btnCancel.Text = "�� ��";
            this.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblWeekDay
            // 
            this.lblWeekDay.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWeekDay.Location = new System.Drawing.Point(16, 24);
            this.lblWeekDay.Name = "lblWeekDay";
            this.lblWeekDay.Size = new System.Drawing.Size(168, 23);
            this.lblWeekDay.TabIndex = 214;
            this.lblWeekDay.Text = "���� �ð� ����";
            this.lblWeekDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(232, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 215;
            this.label2.Text = "�ָ� �ð� ����";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(16, 9);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(0, 12);
            this.lblMsg.TabIndex = 216;
            // 
            // TimeTarget
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(424, 550);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblWeekDay);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.tlvEnd);
            this.Controls.Add(this.tlvDay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimeTarget";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "�ð� Ÿ����";
            this.Load += new System.EventHandler(this.TimeTarget_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void TimeTarget_Load(object sender, System.EventArgs e)
		{
			init_tvTimes();  // treeview ���� �ʱ�ȭ
			
			enableTreeView(); // treeview ��Ʈ�� enable����

			if (setTimes != null && !setTimes.Equals(""))
				filteringDaynEnd();

            if (STypeNm.Equals("�ð��뵶����"))
            {
                lblMsg.Text = "�ð��뵶������ �ð� ���� �Դϴ�. ";
            }
            else
            {
                lblMsg.Text = "";
            }
            
		}

		
		/// <summary>
		/// �ð��� treeView �ʱ� ����(����/�ָ�)
		/// </summary>
		private void init_tvTimes()
		{
			int fromTm = 0;
			int toTm = 0;
			string strFrom = "";
			string strTo   = "";
			
			tlvDay.Items.Clear();
			tlvEnd.Items.Clear();

			TreeListViewItem dayItems = null;

			if (dayOfNum == -1) // ����/�ָ� ���� ����
			{
				lblWeekDay.Text = "����-�ָ� ���о���";
				dayItems = new TreeListViewItem("���о���",0);
			}
			else
				dayItems = new TreeListViewItem("����",0);

			TreeListViewItem endItems = new TreeListViewItem("�ָ�",0);
			TreeListViewItem timeDay = null;
			TreeListViewItem timeEnd = null;
			dayItems.Tag = "day";
			endItems.Tag = "end";

			for(int i = 0; i < 24; i++)
			{
				fromTm = i;
				toTm = i+1;
				
				strFrom = string.Format("{0:0#}",fromTm);
				strTo   = string.Format("{0:0#}",toTm);
				// ���� �ð���
				timeDay = new TreeListViewItem(strFrom + "��",1);
				timeDay.SubItems.Add(strFrom + "-" + strTo);
				timeDay.Tag = strFrom;				
				timeDay.Items.SortOrder= System.Windows.Forms.SortOrder.None;
				dayItems.Items.Add(timeDay);
                // �ָ� �ð���
				timeEnd = new TreeListViewItem(strFrom + "��",1);
				timeEnd.SubItems.Add(strFrom + "-" + strTo);
				timeEnd.Tag = strFrom;				
				timeEnd.Items.SortOrder= System.Windows.Forms.SortOrder.None;
				endItems.Items.Add(timeEnd);
			}
			
			tlvDay.Items.Add(dayItems);
			tlvEnd.Items.Add(endItems);
			// ��� Ȯ�� �� ���°� �ǵ���
			tlvDay.ExpandAll();
			tlvEnd.ExpandAll();
			
		}


		/// <summary>
		/// ���� Ÿ���� �����ؼ� treeview��Ʈ�� ����
		/// </summary>
		private void enableTreeView()
		{
			if (dayOfNum >= 1 && dayOfNum <= 15)
			{
				// ���� treeview�� enable
				tlvDay.Enabled = true;
				tlvEnd.Enabled = false;
			}
			else if (dayOfNum == 16 || dayOfNum == 17 || dayOfNum == 33)
			{
				// �ָ� treeview�� enable
				tlvDay.Enabled = false;
				tlvEnd.Enabled = true;
			}
			else if (dayOfNum == -1)
			{
				// ����,�ָ� ���� ����
				// �׷��� �ϳ��� ��Ʈ�Ѹ� ��밡���ϰ� ��.
				tlvDay.Enabled = true;
				tlvEnd.Enabled = false;
			}
		}

		
		/// <summary>
		/// �ð� �����͵��� ���͸�-����/�ָ�/ȥ�� ����ó��
		/// </summary>
		private void filteringDaynEnd()
		{
			string[] weeks = setTimes.Split('-');
			if (weeks != null && weeks.Length > 1)
			{
				// ����, �ָ� �и�
				string[] splits_d = weeks[0].Substring(1, weeks[0].Length-1).Split('^');
				string[] splits_e = weeks[1].Substring(1, weeks[1].Length-1).Split('^');
				checkingTreeView(splits_d, WeekDayOfEnd.day);
				checkingTreeView(splits_e, WeekDayOfEnd.end);
			}
			else
			{
				if (setTimes.Substring(0,1) == "d" ) // ����
				{
					string[] splits = setTimes.Substring(1, setTimes.Length -1).Split('^');
					checkingTreeView(splits, WeekDayOfEnd.day);
				}
				else if (setTimes.Substring(0,1) == "e") // �ָ�
				{
					string[] splits = setTimes.Substring(1, setTimes.Length -1).Split('^');
					checkingTreeView(splits, WeekDayOfEnd.end);
				}
				else
				{
					string[] splits = setTimes.Split('^');

					//**���ſ� ��ϵ� ������ ó��**//
					// ���Ϻ� üũ �׸��� �ִٸ� ����/�ָ� �����ؼ� ó���ϰ�
					// ���ٸ� ȥ�� ���� ó�� �Ѵ�.
					if (dayOfNum >= 1 && dayOfNum <= 15)
					{
						checkingTreeView(splits, WeekDayOfEnd.day); // ����
					}
					else if (dayOfNum == 16 || dayOfNum == 17 || dayOfNum == 33)
					{
						checkingTreeView(splits, WeekDayOfEnd.end); // �ָ�
					}
					else
					{
						// ����, �ָ� ����(����,�ָ� ���� ���ٴ� �ǹ�)
						// �׷��� �ϳ��� ��Ʈ�Ѹ� enable
						checkingTreeView(splits, WeekDayOfEnd.dayNend);
						//checkingTreeView(splits, WeekDayOfEnd.end);
					}															
				}
			}
		}
		
		
		/// <summary>
		/// �ð� ���� treeview�� ���ε�(check-in n out)
		/// </summary>
		/// <param name="chkList">�ð� ��</param>
		/// <param name="weekType">����/�ָ�����</param>
		private void checkingTreeView(string[] chkList, WeekDayOfEnd weekType)
		{
			if (weekType == WeekDayOfEnd.day) // ����
			{
				for(int i = 0; i < chkList.Length; i++)
				{
					for(int j = 0; j < tlvDay.Items[0].Items.Count; j++)
					{
						TreeListViewItem item = tlvDay.Items[0].Items[j];						
						
						if (item.Tag.ToString() == chkList[i])
						{
							item.Checked = true;
							break;
						}
					}
				}
			}			
			else if (weekType == WeekDayOfEnd.end) // �ָ�
			{
				for(int i = 0; i < chkList.Length; i++)
				{
					for(int j = 0; j < tlvEnd.Items[0].Items.Count; j++)
					{
						TreeListViewItem item = tlvEnd.Items[0].Items[j];						
						
						if (item.Tag.ToString() == chkList[i])
						{
							item.Checked = true;
							break;
						}
					}
				}
			}			
			else if (weekType == WeekDayOfEnd.dayNend) // ����, �ָ� ���� ����.
			{
				for(int i = 0; i < chkList.Length; i++)
				{
					for(int j = 0; j < tlvDay.Items[0].Items.Count; j++)
					{
						TreeListViewItem item = tlvDay.Items[0].Items[j];						
						
						if (item.Tag.ToString() == chkList[i])
						{
							item.Checked = true;
							break;
						}
					}
				}
			}
		}

		
		/// <summary>
		/// Ÿ�� �ð� ���ڿ� ����
		/// </summary>
		private void compositTimes()
		{
			string dayFix = "d";
			string middleFix = "-";
			string endFix = "e";
			string strDay = "";
			string strEnd = "";
			
			if (dayOfNum == -1) // ����,�ָ� ���� ����..
			{
				strDay =  compositDetail(WeekDayOfEnd.dayNend);
				//���� Ÿ�� �ð� ���ڿ� ����
				if (strDay.Length >= 2)
					setTimes = strDay;// ����(00^14)
				else 
					setTimes = "";
			}
			else if (dayOfNum >= 1 && dayOfNum <= 15) // ����
			{
				strDay =  compositDetail(WeekDayOfEnd.day);			
				//���� Ÿ�� �ð� ���ڿ� ����
				if (strDay.Length >= 2)
					setTimes = dayFix + strDay;// ����(d00^14)
				else 
					setTimes = "";
			}
			else if (dayOfNum == 16 || dayOfNum == 17 || dayOfNum == 33) //�ָ�
			{
				strEnd =  compositDetail(WeekDayOfEnd.end);			
				//���� Ÿ�� �ð� ���ڿ� ����
				if (strEnd.Length >= 2)
					setTimes = endFix + strEnd;// ����(e00^14)
				else 
					setTimes = "";
			}
			else
			{
				// ����,�ָ� ȥ����				
				strDay =  compositDetail(WeekDayOfEnd.day);			
				strEnd =  compositDetail(WeekDayOfEnd.end);

				//���� Ÿ�� �ð� ���ڿ� ����
				if (strDay.Length >= 2 || strEnd.Length >= 2) // ��� �ϳ��� �ð� ���� �� �����ڴ� 3�ڸ��̹Ƿ�
					setTimes = dayFix + strDay + middleFix + endFix + strEnd;// ����(d00^14-e01^23)
				else
					setTimes = "";
			}			
		}

		/// <summary>
		/// Ÿ�� �ð� ���ڿ� ���� ��
		/// </summary>
		/// <param name="weekType"></param>
		/// <returns></returns>
		private string compositDetail( WeekDayOfEnd weekType)
		{
			string strDay = "";
			
			if (weekType == WeekDayOfEnd.day)
			{
				for(int i = 0; i < tlvDay.Items[0].Items.Count; i++)
				{
					TreeListViewItem tlv = tlvDay.Items[0].Items[i];
					if (tlv.Checked)
						strDay += tlv.Tag.ToString() + "^";
				}
				if (strDay.Length > 1) // üũ �� ���� ����
					strDay = strDay.Substring(0, strDay.Length - 1); // ������ ������ ����
			}
			else if (weekType == WeekDayOfEnd.end)
			{
				// �ָ� ó��
				for (int i = 0; i < tlvEnd.Items[0].Items.Count; i++)
				{
					TreeListViewItem tlv = tlvEnd.Items[0].Items[i];
					if (tlv.Checked)
						strDay += tlv.Tag.ToString() + "^";
				}

				if (strDay.Length > 1) // üũ �� ���� ����
					strDay = strDay.Substring(0, strDay.Length - 1); // ������ ������ ����
			}
			else if (weekType == WeekDayOfEnd.dayNend)
			{
				// ����,�ָ� ���� ����
				// ��Ʈ���� ���� ��Ʈ�Ѱ� ���� ����ϹǷ�..enableTreeView ����
				for(int i = 0; i < tlvDay.Items[0].Items.Count; i++)
				{
					TreeListViewItem tlv = tlvDay.Items[0].Items[i];
					if (tlv.Checked)
						strDay += tlv.Tag.ToString() + "^";
				}
				if (strDay.Length > 1) // üũ �� ���� ����
					strDay = strDay.Substring(0, strDay.Length - 1); // ������ ������ ����
			}
			return strDay;
		}



		private void btnConfirm_Click(object sender, System.EventArgs e)
		{
			compositTimes();
			
			if (setTimes == "" || setTimes.Length == 0)
			{
				DialogResult result = MessageBox.Show("�ð� ������ �� �����Ͱ� �����ϴ�!\r��� �����Ͻðڽ��ϱ�?"
					,"Ÿ����" ,MessageBoxButtons.YesNo,MessageBoxIcon.Question
					,MessageBoxDefaultButton.Button2);
				if (result == DialogResult.Yes)
				{
					this.DialogResult = DialogResult.Yes;
					this.Close();
				}
			}
			else
			{
				this.DialogResult = DialogResult.Yes;
				this.Close();
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			this.Close();
        }
		
		
		
	}

	/// <summary>
	/// �ְ� Ÿ��{day-����, end-�ָ�,dayNend-ȥ��}
	/// </summary>
	public enum WeekDayOfEnd{ day, end, dayNend};
}
