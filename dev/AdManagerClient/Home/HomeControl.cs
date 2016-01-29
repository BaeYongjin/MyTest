using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using WinFramework.Base;

namespace AdManagerClient
{
	/// <summary>
	/// HomeControl에 대한 요약 설명입니다.
	/// </summary>
	public class HomeControl : System.Windows.Forms.UserControl, IUserControl
    {
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public HomeControl()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();
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

		#region 구성 요소 디자이너에서 생성한 코드
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeControl));
            this.picHomeImg = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picHomeImg)).BeginInit();
            this.SuspendLayout();
            // 
            // picHomeImg
            // 
            this.picHomeImg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picHomeImg.InitialImage = ((System.Drawing.Image)(resources.GetObject("picHomeImg.InitialImage")));
            this.picHomeImg.Location = new System.Drawing.Point(0, 0);
            this.picHomeImg.Margin = new System.Windows.Forms.Padding(0);
            this.picHomeImg.Name = "picHomeImg";
            this.picHomeImg.Size = new System.Drawing.Size(1010, 677);
            this.picHomeImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHomeImg.TabIndex = 0;
            this.picHomeImg.TabStop = false;
            // 
            // HomeControl
            // 
            this.Controls.Add(this.picHomeImg);
            this.Name = "HomeControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.HomeControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picHomeImg)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        private PictureBox picHomeImg;

        #region IUserControl 구현

        // 메뉴코드 : 보안이 필요한 화면에 필요함
        private string menuCode = "";

        /// <summary>
        /// 메뉴 코드-보안이 필요한 화면에 필요함
        /// </summary>
        public string MenuCode
        {
            set { this.menuCode = value; }
            get { return this.menuCode; }
        }

        /// <summary>
        /// 부모컨트롤 지정
        /// </summary>
        /// <param name="control"></param>
        public void SetParent(Control control)
        {
            this.Parent = control;
        }
        /// <summary>
        /// DockStype지정
        /// </summary>
        /// <param name="style"></param>
        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
        }
        #endregion

        #region 이벤트핸들러
        public event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler ProgressEvent;        // 처리중이벤트 핸들러

        private void HomeControl_Load(object sender, EventArgs e)
        {

            picHomeImg.ImageLocation = Application.StartupPath.ToString() + "/Images/wallpaper.jpg";

        }
        #endregion
	}
}
