/*
 * -------------------------------------------------------
 * Class Name: AgencyCodeBiz
 * �ֿ���  : �׷��������� ����
 * �ۼ���    : ��
 * �ۼ���    : ��
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : �躸��
 * ������    : 2013.02
 * ��������  :        
 *            - �̵� ��Ʈ�� Disable�� �˻� ����
 * �����Լ�  :
 *            - GetAgencyCodeList
 * --------------------------------------------------------
 */

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
	/// AgencyCodeService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AgencyCodeBiz : BaseBiz
	{
		public AgencyCodeBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  �ڵ�����ȸ
		/// </summary>
		/// <param name="agencycodeModel"></param>
		public void GetAgencyCodeList(HeaderModel header, AgencyCodeModel agencycodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgencyCodeList() Start");
				_log.Debug("-----------------------------------------");

				// ����Ʈ���̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("Section :[" + agencycodeModel.AgencyCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
                    + " SELECT a.AGNC_COD AS AgencyCode, a.AGNC_NM AS AgencyName  \n"
                    + "   FROM AGNC a              \n"
                    + "   LEFT JOIN MDA_REP   b  ON (a.REP_COD = b.REP_COD)               \n"
                    + "   WHERE a.USE_YN = 'Y'         \n"					
					);

				// �ڵ�з��� ����������
				if (agencycodeModel.SearchKey.Trim().Length > 0)
				{
                    sbQuery.Append("  AND a.AGNC_NM    LIKE '%" + agencycodeModel.SearchKey.Trim() + "%' \n");
				}
                // [E_01]
                //if(!agencycodeModel.SearchRap.Equals("00"))
                //{
                //    sbQuery.Append("  AND(  a.RapCode = '"+agencycodeModel.SearchRap+"' OR a.RapCode = 0 ) \n");
                //}    

                sbQuery.Append(" ORDER BY a.AGNC_COD \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				agencycodeModel.AgencyCodeDataSet = ds.Copy();
				// ���
				agencycodeModel.ResultCnt = Utility.GetDatasetCount(agencycodeModel.AgencyCodeDataSet);
				// ����ڵ� ��Ʈ
				agencycodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + agencycodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgencyCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				agencycodeModel.ResultCD = "3000";
				agencycodeModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);

				// ����Ʈ���̽���  Close�Ѵ�
				_db.Close();
			}
			// ����Ʈ���̽���  Close�Ѵ�
			_db.Close();
		}
	}
}
