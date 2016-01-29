/*
 * -------------------------------------------------------
 * Class Name: ReserveModel
 * �ֿ���  : ����ó���� ���� history data model
 * �ۼ���    : bae 
 * �ۼ���    : 2010.06.07
 * Ư�̻���  : 
 * -------------------------------------------------------
 * [��������]
 * �����ڵ�  : 
 * ������    : 
 * ������    : 
 * ��������  :             
 * --------------------------------------------------------
 * 
 */

using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// ReserveModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ReserveHistoryModel : BaseModel
	{
		private string ackNo = string.Empty;
		private string seq = string.Empty;
		private string rvType = string.Empty;
		private string regDt = string.Empty;
		private string rvDesc = string.Empty;
		private string isSuccess = string.Empty;

		private DataSet dsList = new DataSet();

		public ReserveHistoryModel() : base()
		{			
		}
		
		/// <summary>
		/// ���Ϲ������� ��ȣ
		/// </summary>
		public string AckNo
		{
			set{this.ackNo = value;}
			get{return this.ackNo;}
		}
		/// <summary>
		/// ���Ϲ������� seq
		/// </summary>
		public string Seq
		{
			set{this.seq = value;}
			get{return this.seq;}
		}
		/// <summary>
		/// ����ó�� ����(01-���Ϲ�������,�̷��� Ȯ�尡�ɼ� ������ �߰�)
		/// </summary>
		public string RvType
		{
			set{this.rvType = value;}
			get{return this.rvType;}
		}
		/// <summary>
		/// ������ �����
		/// </summary>
		public string RegDt
		{
			set{this.regDt = value;}
			get{return this.regDt;}
		}
		/// <summary>
		/// ó�����࿡ ���� ����
		/// </summary>
		public string RvDesc
		{
			set{this.rvDesc = value;}
			get{return this.rvDesc;}
		}

		/// <summary>
		/// ����ó���� ����(Y),����(N) ��
		/// </summary>
		public string IsSuccess
		{
			set{this.isSuccess = value;}
			get{return this.isSuccess;}
		}

		/// <summary>
		/// History ���
		/// </summary>
		public DataSet DataList
		{
			set{this.dsList = value;}
			get{return this.dsList;}
		}
	}
}
