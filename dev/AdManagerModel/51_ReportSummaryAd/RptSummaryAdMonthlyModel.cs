using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// RptSummaryAdMonthlyModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RptSummaryAdMonthlyModel : BaseModel
	{
		//��ȸ��
		private string _SearchDay = null;			//��ȸ ���賯¥		 

		//�����ȸ��
		private DataSet _DataSet = null;

		public RptSummaryAdMonthlyModel() : base()
		{
			Init();	

		}

		#region public Init()
		public void Init()
		{
			_SearchDay = "";
			_DataSet = new DataSet();
		}
		#endregion

		#region ������Ƽ
		public DataSet RptMonthlyDataSet
		{
			get {return _DataSet; }
			set {_DataSet = value; }
		}

		public string SearchDay
		{
			get { return _SearchDay; }
			set { _SearchDay = value; }
		}

		#endregion
	}
}
