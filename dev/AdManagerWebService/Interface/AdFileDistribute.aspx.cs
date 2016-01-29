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
	/// AdFileDistribute에 대한 요약 설명입니다.
	/// </summary>
	public partial class AdFileDistribute : System.Web.UI.Page
	{
		protected AdManagerWebService.Interface.DataSetFileDis dsFileDis;

		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private Logger         _log         = FrameSystem.oLog;

		private	AdFileDistributeModel	_model		= new AdFileDistributeModel();
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
			if(!IsPostBack)
			{
				TextBox2.Text = DateTime.Now.ToString("yyyy-MM-dd");
				TextBox1.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
			}

		}

		#region Web Form 디자이너에서 생성한 코드
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 이 호출은 ASP.NET Web Form 디자이너에 필요합니다.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
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

				// 결과 셋트
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 End");
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
