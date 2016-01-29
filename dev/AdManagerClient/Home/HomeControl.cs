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
	/// HomeControl�� ���� ��� �����Դϴ�.
	/// </summary>
	public class HomeControl : System.Windows.Forms.UserControl, IUserControl
    {
		/// <summary> 
		/// �ʼ� �����̳� �����Դϴ�.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public HomeControl()
		{
			// �� ȣ���� Windows.Forms Form �����̳ʿ� �ʿ��մϴ�.
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

		#region ���� ��� �����̳ʿ��� ������ �ڵ�
		/// <summary> 
		/// �����̳� ������ �ʿ��� �޼����Դϴ�. 
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
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

        #region IUserControl ����

        // �޴��ڵ� : ������ �ʿ��� ȭ�鿡 �ʿ���
        private string menuCode = "";

        /// <summary>
        /// �޴� �ڵ�-������ �ʿ��� ȭ�鿡 �ʿ���
        /// </summary>
        public string MenuCode
        {
            set { this.menuCode = value; }
            get { return this.menuCode; }
        }

        /// <summary>
        /// �θ���Ʈ�� ����
        /// </summary>
        /// <param name="control"></param>
        public void SetParent(Control control)
        {
            this.Parent = control;
        }
        /// <summary>
        /// DockStype����
        /// </summary>
        /// <param name="style"></param>
        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
        }
        #endregion

        #region �̺�Ʈ�ڵ鷯
        public event StatusEventHandler StatusEvent;			// �����̺�Ʈ �ڵ鷯
        public event ProgressEventHandler ProgressEvent;        // ó�����̺�Ʈ �ڵ鷯

        private void HomeControl_Load(object sender, EventArgs e)
        {

            picHomeImg.ImageLocation = Application.StartupPath.ToString() + "/Images/wallpaper.jpg";

        }
        #endregion
	}
}
