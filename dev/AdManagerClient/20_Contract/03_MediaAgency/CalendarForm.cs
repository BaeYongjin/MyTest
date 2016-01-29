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
	/// ContentsSearch_pForm에 대한 요약 설명입니다.
	/// </summary>
	/// 
	public class CalendarForm : System.Windows.Forms.Form
	{
	
		#region 사용자정의 객체 및 변수

		// 시스템 정보 : 화면공통
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		
		// 사용할 정보모델
		MediaAgencyModel mediaAgencyModel  = new MediaAgencyModel();	// 컨텐츠정보모델

		MediaAgencyControl mediaAgencyControl = null;

		// 화면처리용 변수
		private string        contStartday = null;
		private string        contEndday = null;
		public string        startFlag = null;

		#endregion
		private System.Windows.Forms.MonthCalendar monthCalendar1;

		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CalendarForm(MediaAgencyControl sender)
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
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
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			
			//
			startFlag = startFlag2;
			mediaAgencyControl = sender;
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
			this.Text = "달력";
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
