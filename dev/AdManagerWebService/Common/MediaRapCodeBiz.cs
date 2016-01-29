using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Common
{
	/// <summary>
	/// MediaRapCodeService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MediaRapCodeBiz : BaseBiz
	{
		public MediaRapCodeBiz() : base(FrameSystem.connDbString, true)
		{

			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  �ڵ�����ȸ
		/// </summary>
		/// <param name="mediacodeModel"></param>
		public void GetMediaRapCodeList(HeaderModel header, MediaRapCodeModel mediacodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapCodeList() Start");
				_log.Debug("-----------------------------------------");		

				// ����Ʈ���̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("Section :[" + mediacodeModel.RapCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
                    + " SELECT REP_COD AS RapCode, REP_NM AS RapName  \n"
                    + "   FROM MDA_REP               \n"
                    + "   WHERE USE_YN = 'Y'         \n"					
					);

				// �ڵ�з��� ����������
				if (mediacodeModel.SearchKey.Trim().Length > 0)
				{
                    sbQuery.Append("  AND REP_NM    LIKE '%" + mediacodeModel.SearchKey.Trim() + "%' \n");
				}

                sbQuery.Append(" ORDER BY REP_COD \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				mediacodeModel.MediaRapCodeDataSet = ds.Copy();
				// ���
				mediacodeModel.ResultCnt = Utility.GetDatasetCount(mediacodeModel.MediaRapCodeDataSet);
				// ����ڵ� ��Ʈ
				mediacodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + mediacodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				mediacodeModel.ResultCD = "3000";
				mediacodeModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
				// ����Ʈ���̽���  Close�Ѵ�
				_db.Close();
			}
			// ����Ʈ���̽���  Close�Ѵ�
			_db.Close();
		}


		/// <summary>
		///  �ڵ�����ȸ(ID�� üũ�Ͽ� �ش��ϴ� ���̵� ���Ѹ� ��ȸ ��Ų��.
		/// </summary>
		/// <param name="GetMediaRapCodeListIdCheck"></param>
		public void GetMediaRapCodeListIdCheck(HeaderModel header, MediaRapCodeModel mediacodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapCodeList() Start");
				_log.Debug("-----------------------------------------");		

				// ����Ʈ���̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("Section :[" + mediacodeModel.RapCode + "]");
				_log.Debug("Section :[" + header.UserID + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				String RapCode = "0";
				//user�����ڵ� ����
				if(header.UserID != null && !header.UserID.Equals(""))
				{
                    OracleParameter[] sqlParams = new OracleParameter[1];
					int i = 0;
					sbQuery.Append("\n"
                        + " SELECT ISNULL(a.REP_COD,'')    AS RapCode	\n"
                        + "   FROM STM_USER a LEFT JOIN STM_COD b ON (a.USER_LVL = b.STM_COD and b.STM_COD_CLS = '11')	\n"
                        + "                     LEFT JOIN STM_COD c ON (a.USER_CLS = c.STM_COD and c.STM_COD_CLS = '12')	\n"
                        + " WHERE USER_ID = @UserID   \n"
                        + "    AND USE_YN  = 'Y'	\n");

                    sqlParams[i] = new OracleParameter("@UserID", OracleDbType.Varchar2, 10);
					sqlParams[i++].Value = header.UserID;
					
					// __DEBUG__
					_log.Debug(sbQuery.ToString());

					DataSet dsID = new DataSet();
					_db.ExecuteQueryParams(dsID, sbQuery.ToString(), sqlParams);
				
					if (Utility.GetDatasetCount(dsID) == 0)
					{
						mediacodeModel.ResultCD = "2000"; // �ش� ID�� DB�� �������� ����
						mediacodeModel.ResultDesc = "�ش��ϴ� ID�� �������� �ʽ��ϴ�.";
						dsID.Dispose();

						// ����Ʈ���̽���  Close�Ѵ�
						_db.Close();

						throw new FrameException("�ش�ID�� �������� �ʽ��ϴ�.");
					}

					// RapCode�� ������ �´�.
					RapCode = Utility.GetDatasetString(dsID, 0, "RapCode");
					sbQuery.Length = 0;
				}
				_log.Debug("##############################"+ RapCode);

				// ��������
				sbQuery.Append("\n"
                    + " SELECT REP_COD, REP_NM  \n"
                    + "   FROM MDA_REP               \n"
                    + "   WHERE USE_YN = 'Y'         \n"					
					);

				// �ڵ�з��� ����������
				if (mediacodeModel.SearchKey.Trim().Length > 0)
				{
                    sbQuery.Append("  AND REP_NM    LIKE '%" + mediacodeModel.SearchKey.Trim() + "%' \n");
				}
				// ���ڵ尡 ��ü������ �ƴ϶�� �ش��ϴ� ���ڵ常 �о�´�.
				if (!RapCode.Equals("0"))
				{
                    sbQuery.Append("  AND REP_COD    = '" + RapCode + "' \n");
				}


                sbQuery.Append(" ORDER BY REP_COD \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				mediacodeModel.MediaRapCodeDataSet = ds.Copy();
				// ���
				mediacodeModel.ResultCnt = Utility.GetDatasetCount(mediacodeModel.MediaRapCodeDataSet);
				// ����ڵ� ��Ʈ
				mediacodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + mediacodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				if(!"2000".Equals(mediacodeModel.ResultCD))
				{
					mediacodeModel.ResultCD = "3000";
					mediacodeModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				}
				_log.Exception(ex);
				// ����Ʈ���̽���  Close�Ѵ�
				_db.Close();
			}
			// ����Ʈ���̽���  Close�Ѵ�
			_db.Close();
		}
	}
}
