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
	/// ContentsSearch_pForm�� ���� ��� �����Դϴ�.
	/// </summary>
	/// 
	public class CalendarForm : System.Windows.Forms.Form
	{
	
		#region ��������� ��ü �� ����

		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		
		// ����� ������
		MediaAgencyModel mediaAgencyModel  = new MediaAgencyModel();	// ������������

		MediaAgencyControl mediaAgencyControl = null;

		// ȭ��ó���� ����
		private string        contStartday = null;
		private string        contEndday = null;
		public string        startFlag = null;

		#endregion
		private System.Windows.Forms.MonthCalendar monthCalendar1;

		/// <summary>
		/// �ʼ� �����̳� �����Դϴ�.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CalendarForm(MediaAgencyControl sender)
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			
			//
			mediaAgencyControl = sender;
		}

		public CalendarForm(MediaAgencyControl sender, string startFlag2)
		{

			Debug.WriteLine("@@@");
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			
			//
			startFlag = startFlag2;
			mediaAgencyControl = sender;
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
			this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
			this.SuspendLayout();
			// 
			// monthCalendar1
			// 
			this.monthCalendar1.Location = new System.Drawing.Point(0, 0);
			this.monthCalendar1.Name = "monthCalendar1";
			this.monthCalendar1.TabIndex = 0;
			this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
			// 
			// CalendarForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(136, 149);
			this.Controls.Add(this.monthCalendar1);
			this.Name = "CalendarForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "�޷�";
			this.ResumeLayout(false);

		}
		#endregion
				
		private void monthCalendar1_DateChanged(object sender, System.Windows.Forms.DateRangeEventArgs e)
		{
			string [] strs =  monthCalendar1.SelectionStart.ToShortDateString().Split('-');
			contStartday = strs[0]+strs[1]+strs[2];
			contEndday = strs[0]+strs[1]+strs[2];

			if(startFlag.Equals("Start"))
			{
				this.mediaAgencyControl.SetYear1  = contStartday;			
			}
			else
			{
				this.mediaAgencyControl.SetYear2  = contEndday;
			}	
			
			this.Close();
		}

		
	}
}
