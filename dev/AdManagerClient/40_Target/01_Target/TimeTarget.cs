using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace AdManagerClient
{
	
	/// <summary>
	/// TimeTarget에 대한 요약 설명입니다.
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
		/// 주중/주말 값을 정수의 합으로 표현
		/// </summary>
		private int dayOfNum = 0;
		private System.Windows.Forms.Label lblWeekDay;
							
		private string setTimes = string.Empty;
        private Label lblMsg;

		/// <summary>
		/// 타겟 지정 된 시간 문자열
		/// </summary>
		public string TargetingTime
		{
			set{setTimes = value;}	
			get{return setTimes;}
		}
		
		
		/// <summary>
		/// 주중,주말을 숫자의 합으로 설정
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
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			
			//
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


		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
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
            this.columnHeader1.Text = "시간";
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
            this.columnHeader2.Text = "시간";
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
            this.btnConfirm.Text = "확 인";
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
            this.btnCancel.Text = "취 소";
            this.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblWeekDay
            // 
            this.lblWeekDay.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWeekDay.Location = new System.Drawing.Point(16, 24);
            this.lblWeekDay.Name = "lblWeekDay";
            this.lblWeekDay.Size = new System.Drawing.Size(168, 23);
            this.lblWeekDay.TabIndex = 214;
            this.lblWeekDay.Text = "주중 시간 설정";
            this.lblWeekDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(232, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 215;
            this.label2.Text = "주말 시간 설정";
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
            this.Text = "시간 타겟팅";
            this.Load += new System.EventHandler(this.TimeTarget_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void TimeTarget_Load(object sender, System.EventArgs e)
		{
			init_tvTimes();  // treeview 구성 초기화
			
			enableTreeView(); // treeview 컨트롤 enable설정

			if (setTimes != null && !setTimes.Equals(""))
				filteringDaynEnd();

            if (STypeNm.Equals("시간대독점편성"))
            {
                lblMsg.Text = "시간대독점편성의 시간 설정 입니다. ";
            }
            else
            {
                lblMsg.Text = "";
            }
            
		}

		
		/// <summary>
		/// 시간대 treeView 초기 구성(주중/주말)
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

			if (dayOfNum == -1) // 주중/주말 구분 없음
			{
				lblWeekDay.Text = "주중-주말 구분없음";
				dayItems = new TreeListViewItem("구분없음",0);
			}
			else
				dayItems = new TreeListViewItem("주중",0);

			TreeListViewItem endItems = new TreeListViewItem("주말",0);
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
				// 주중 시간대
				timeDay = new TreeListViewItem(strFrom + "시",1);
				timeDay.SubItems.Add(strFrom + "-" + strTo);
				timeDay.Tag = strFrom;				
				timeDay.Items.SortOrder= System.Windows.Forms.SortOrder.None;
				dayItems.Items.Add(timeDay);
                // 주말 시간대
				timeEnd = new TreeListViewItem(strFrom + "시",1);
				timeEnd.SubItems.Add(strFrom + "-" + strTo);
				timeEnd.Tag = strFrom;				
				timeEnd.Items.SortOrder= System.Windows.Forms.SortOrder.None;
				endItems.Items.Add(timeEnd);
			}
			
			tlvDay.Items.Add(dayItems);
			tlvEnd.Items.Add(endItems);
			// 모두 확장 된 상태가 되도록
			tlvDay.ExpandAll();
			tlvEnd.ExpandAll();
			
		}


		/// <summary>
		/// 요일 타겟팅 관련해서 treeview컨트롤 제어
		/// </summary>
		private void enableTreeView()
		{
			if (dayOfNum >= 1 && dayOfNum <= 15)
			{
				// 주중 treeview만 enable
				tlvDay.Enabled = true;
				tlvEnd.Enabled = false;
			}
			else if (dayOfNum == 16 || dayOfNum == 17 || dayOfNum == 33)
			{
				// 주말 treeview만 enable
				tlvDay.Enabled = false;
				tlvEnd.Enabled = true;
			}
			else if (dayOfNum == -1)
			{
				// 주중,주말 구분 없음
				// 그래서 하나의 컨트롤만 사용가능하게 함.
				tlvDay.Enabled = true;
				tlvEnd.Enabled = false;
			}
		}

		
		/// <summary>
		/// 시간 데이터들의 필터링-주중/주말/혼합 구분처리
		/// </summary>
		private void filteringDaynEnd()
		{
			string[] weeks = setTimes.Split('-');
			if (weeks != null && weeks.Length > 1)
			{
				// 주중, 주말 분리
				string[] splits_d = weeks[0].Substring(1, weeks[0].Length-1).Split('^');
				string[] splits_e = weeks[1].Substring(1, weeks[1].Length-1).Split('^');
				checkingTreeView(splits_d, WeekDayOfEnd.day);
				checkingTreeView(splits_e, WeekDayOfEnd.end);
			}
			else
			{
				if (setTimes.Substring(0,1) == "d" ) // 주중
				{
					string[] splits = setTimes.Substring(1, setTimes.Length -1).Split('^');
					checkingTreeView(splits, WeekDayOfEnd.day);
				}
				else if (setTimes.Substring(0,1) == "e") // 주말
				{
					string[] splits = setTimes.Substring(1, setTimes.Length -1).Split('^');
					checkingTreeView(splits, WeekDayOfEnd.end);
				}
				else
				{
					string[] splits = setTimes.Split('^');

					//**과거에 등록된 데이터 처리**//
					// 요일별 체크 항목이 있다면 주중/주말 구분해서 처리하고
					// 없다면 혼합 모드로 처리 한다.
					if (dayOfNum >= 1 && dayOfNum <= 15)
					{
						checkingTreeView(splits, WeekDayOfEnd.day); // 주중
					}
					else if (dayOfNum == 16 || dayOfNum == 17 || dayOfNum == 33)
					{
						checkingTreeView(splits, WeekDayOfEnd.end); // 주말
					}
					else
					{
						// 주중, 주말 공통(주중,주말 관계 없다는 의미)
						// 그래서 하나의 컨트롤만 enable
						checkingTreeView(splits, WeekDayOfEnd.dayNend);
						//checkingTreeView(splits, WeekDayOfEnd.end);
					}															
				}
			}
		}
		
		
		/// <summary>
		/// 시간 값을 treeview에 바인딩(check-in n out)
		/// </summary>
		/// <param name="chkList">시간 값</param>
		/// <param name="weekType">주중/주말구분</param>
		private void checkingTreeView(string[] chkList, WeekDayOfEnd weekType)
		{
			if (weekType == WeekDayOfEnd.day) // 주중
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
			else if (weekType == WeekDayOfEnd.end) // 주말
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
			else if (weekType == WeekDayOfEnd.dayNend) // 주중, 주말 구분 없음.
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
		/// 타겟 시간 문자열 구성
		/// </summary>
		private void compositTimes()
		{
			string dayFix = "d";
			string middleFix = "-";
			string endFix = "e";
			string strDay = "";
			string strEnd = "";
			
			if (dayOfNum == -1) // 주중,주말 구분 없이..
			{
				strDay =  compositDetail(WeekDayOfEnd.dayNend);
				//최종 타겟 시간 문자열 구성
				if (strDay.Length >= 2)
					setTimes = strDay;// 예상(00^14)
				else 
					setTimes = "";
			}
			else if (dayOfNum >= 1 && dayOfNum <= 15) // 주중
			{
				strDay =  compositDetail(WeekDayOfEnd.day);			
				//최종 타겟 시간 문자열 구성
				if (strDay.Length >= 2)
					setTimes = dayFix + strDay;// 예상(d00^14)
				else 
					setTimes = "";
			}
			else if (dayOfNum == 16 || dayOfNum == 17 || dayOfNum == 33) //주말
			{
				strEnd =  compositDetail(WeekDayOfEnd.end);			
				//최종 타겟 시간 문자열 구성
				if (strEnd.Length >= 2)
					setTimes = endFix + strEnd;// 예상(e00^14)
				else 
					setTimes = "";
			}
			else
			{
				// 주중,주말 혼합형				
				strDay =  compositDetail(WeekDayOfEnd.day);			
				strEnd =  compositDetail(WeekDayOfEnd.end);

				//최종 타겟 시간 문자열 구성
				if (strDay.Length >= 2 || strEnd.Length >= 2) // 적어도 하나의 시간 값과 앞 구분자는 3자리이므로
					setTimes = dayFix + strDay + middleFix + endFix + strEnd;// 예상(d00^14-e01^23)
				else
					setTimes = "";
			}			
		}

		/// <summary>
		/// 타겟 시간 문자열 구성 상세
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
				if (strDay.Length > 1) // 체크 값 길이 검증
					strDay = strDay.Substring(0, strDay.Length - 1); // 마지막 구분자 제거
			}
			else if (weekType == WeekDayOfEnd.end)
			{
				// 주말 처리
				for (int i = 0; i < tlvEnd.Items[0].Items.Count; i++)
				{
					TreeListViewItem tlv = tlvEnd.Items[0].Items[i];
					if (tlv.Checked)
						strDay += tlv.Tag.ToString() + "^";
				}

				if (strDay.Length > 1) // 체크 값 길이 검증
					strDay = strDay.Substring(0, strDay.Length - 1); // 마지막 구분자 제거
			}
			else if (weekType == WeekDayOfEnd.dayNend)
			{
				// 주중,주말 구분 없이
				// 컨트롤은 주중 컨트롤과 같이 사용하므로..enableTreeView 참고
				for(int i = 0; i < tlvDay.Items[0].Items.Count; i++)
				{
					TreeListViewItem tlv = tlvDay.Items[0].Items[i];
					if (tlv.Checked)
						strDay += tlv.Tag.ToString() + "^";
				}
				if (strDay.Length > 1) // 체크 값 길이 검증
					strDay = strDay.Substring(0, strDay.Length - 1); // 마지막 구분자 제거
			}
			return strDay;
		}



		private void btnConfirm_Click(object sender, System.EventArgs e)
		{
			compositTimes();
			
			if (setTimes == "" || setTimes.Length == 0)
			{
				DialogResult result = MessageBox.Show("시간 설정이 된 데이터가 없습니다!\r계속 진행하시겠습니까?"
					,"타겟팅" ,MessageBoxButtons.YesNo,MessageBoxIcon.Question
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
	/// 주간 타입{day-주중, end-주말,dayNend-혼합}
	/// </summary>
	public enum WeekDayOfEnd{ day, end, dayNend};
}
