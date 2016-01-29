using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Common
{
	/// <summary>
	/// CugCodeService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class CugCodeBiz : BaseBiz
	{
		public CugCodeBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  �ڵ�����ȸ
		/// </summary>
		/// <param name="agencycodeModel"></param>
		public void GetCugCodeList(HeaderModel header, CugCodeModel cugCodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCugCodeList() Start");
				_log.Debug("-----------------------------------------");

				// ����Ʈ���̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("Section :[" + cugCodeModel.CugCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT CugCode, CugName  \n"
					+ "   FROM Cug with(noLock)                \n"
					+ "   WHERE UseYn = 'Y'      \n"					
					+ "     AND CugCode != '0'      \n"					
					);

				// �ڵ�з��� ����������
				if (cugCodeModel.SearchKey.Trim().Length > 0)
				{
					sbQuery.Append("  AND AgencyName    LIKE '%" + cugCodeModel.SearchKey.Trim() + "%' \n");
				}	

				sbQuery.Append(" ORDER BY CugCode \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				cugCodeModel.CugCodeDataSet = ds.Copy();
				// ���
				cugCodeModel.ResultCnt = Utility.GetDatasetCount(cugCodeModel.CugCodeDataSet);
				// ����ڵ� ��Ʈ
				cugCodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + cugCodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCugCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				cugCodeModel.ResultCD = "3000";
				cugCodeModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);

				// ����Ʈ���̽���  Close�Ѵ�
				_db.Close();
			}
			// ����Ʈ���̽���  Close�Ѵ�
			_db.Close();
		}
	}
}
