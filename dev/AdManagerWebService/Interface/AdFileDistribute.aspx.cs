using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerWebService.Interface
{
	/// <summary>
	/// AdFileDistribute�� ���� ��� �����Դϴ�.
	/// </summary>
	public partial class AdFileDistribute : System.Web.UI.Page
	{
		protected AdManagerWebService.Interface.DataSetFileDis dsFileDis;

		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private Logger         _log         = FrameSystem.oLog;

		private	AdFileDistributeModel	_model		= new AdFileDistributeModel();
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// ���⿡ ����� �ڵ带 ��ġ�Ͽ� �������� �ʱ�ȭ�մϴ�.
			if(!IsPostBack)
			{
				TextBox2.Text = DateTime.Now.ToString("yyyy-MM-dd");
				TextBox1.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
			}

		}

		#region Web Form �����̳ʿ��� ������ �ڵ�
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �� ȣ���� ASP.NET Web Form �����̳ʿ� �ʿ��մϴ�.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{    
			this.dsFileDis = new AdManagerWebService.Interface.DataSetFileDis();
			((System.ComponentModel.ISupportInitialize)(this.dsFileDis)).BeginInit();
			// 
			// dsFileDis
			// 
			this.dsFileDis.DataSetName = "DataSetFileDis";
			this.dsFileDis.Locale = new System.Globalization.CultureInfo("en-US");
			((System.ComponentModel.ISupportInitialize)(this.dsFileDis)).EndInit();

		}
		#endregion

		protected void Button1_Click(object sender, System.EventArgs e)
		{
			this.SearchData();
			DataGrid1.DataBind();
		}

		private void SearchData()
		{
			try
			{
				_model.Init();
				_model.BeginDate	= TextBox1.Text;
				_model.EndDate		= TextBox2.Text;

				new AdFileDistributeBiz().GetFileDistributeList(_model);

				if(!_model.ResultCD.Equals("0000"))
				{
					throw new FrameException(_model.ResultDesc, "FileDis", _model.ResultCD);
				}

				Utility.SetDataTable(dsFileDis.AdFileDis, _model.ListDataSet);

				// ��� ��Ʈ
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetMediaRapList():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}
	}
}
