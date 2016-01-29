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

namespace AdManagerWebService.Media
{
	/// <summary>
	/// MediaRapBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MediaRapBiz : BaseBiz
	{
		public MediaRapBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		/// �̵������ȸ
		/// </summary>
		/// <param name="mediarapModel"></param>
		public void GetMediaRapList(HeaderModel header, MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + mediarapModel.SearchKey       + "]");
				_log.Debug("SearchMediaRapLevel:[" + mediarapModel.SearchMediaRap + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
                    + " SELECT A.REP_COD AS RapCode, A.REP_NM AS RapName, A.REP_TP AS RapType, A.PHONE_NO AS Tell, A.REP_MEMO AS \"Comment\"  \n"
                    + "       ,A.USE_YN AS UseYn              \n"
                    + "       ,DECODE(A.USE_YN,'Y','','N','������') AS UseYn_N  \n"
                    + "       ,TO_CHAR(A.DT_INSERT, 'YYYY-MM-DD') AS RegDt              \n"
                    + "       ,TO_CHAR(A.DT_UPDATE, 'YYYY-MM-DD') AS ModDt              \n"
                    + "       ,B.USER_NM AS RegName                                 \n"
                    + "  FROM MDA_REP A LEFT JOIN STM_USER B ON A.ID_INSERT = B.USER_ID \n                 \n"					
					+ " WHERE 1 = 1  \n"
					);


				// ���ΰ� ���������� ��뿩�ΰ� 'Y', 'N' �����͸� �� ��ȸ�Ѵ�.
//				if (header.UserClass.Equals("10") || header.UserClass.Equals("20"))
//				{
//					sbQuery.Append(" AND A.UseYn = 'Y' OR A.UseYn = 'N' \n");
//				}
//				else
//				{
//					sbQuery.Append(" AND A.UseYn = 'Y' \n");
//				}
				
				// �˻�� ������
				if (mediarapModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
                        + "    REP_COD      LIKE '%" + mediarapModel.SearchKey.Trim() + "%' \n"
                        + " OR PHONE_NO    LIKE '%" + mediarapModel.SearchKey.Trim() + "%' \n"
                        + " OR REP_MEMO    LIKE '%" + mediarapModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}

				// �̵������� ����������
				if(mediarapModel.SearchMediaRap.Trim().Length > 0 && !mediarapModel.SearchMediaRap.Trim().Equals("00"))
				{
                    sbQuery.Append(" AND REP_TP = '" + mediarapModel.SearchMediaRap.Trim() + "' \n");
				}			

				if(mediarapModel.SearchchkAdState_10.Trim().Length > 0 && mediarapModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append(" AND A.USE_YN = 'Y' OR A.USE_YN = 'N' \n");
				}	
				if(mediarapModel.SearchchkAdState_10.Trim().Length > 0 && mediarapModel.SearchchkAdState_10.Trim().Equals("N"))
				{
                    sbQuery.Append(" AND  A.USE_YN  = 'Y' \n");					
				}

                sbQuery.Append(" ORDER BY A.REP_COD desc \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �̵��𵨿� ����
				mediarapModel.MediaRapDataSet = ds.Copy();
				// ���
				mediarapModel.ResultCnt = Utility.GetDatasetCount(mediarapModel.MediaRapDataSet);
				// ����ڵ� ��Ʈ
				mediarapModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + mediarapModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				mediarapModel.ResultCD = "3000";
				mediarapModel.ResultDesc = "�̵����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// �̵����� ����
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetMediaRapUpdate(HeaderModel header, MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[7];

				sbQuery.Append(""
                    + "UPDATE MDA_REP                     \n"
                    + "   SET REP_NM     = :RapName      \n"
                    + "      ,REP_TP     = :RapType      \n"
                    + "      ,PHONE_NO   = :Tell      \n"
                    + "      ,REP_MEMO   = :Comment1   \n"
                    + "      ,USE_YN	 = :UseYn      \n"
                    + "      ,DT_UPDATE  = SYSDATE      \n"
                    + "      ,ID_UPDATE  = :RegID         \n"
                    + " WHERE REP_COD    = :RapCode        \n"
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":RapName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":RapType", OracleDbType.Char, 2);
                sqlParams[i++] = new OracleParameter(":Tell", OracleDbType.Varchar2, 15);
                sqlParams[i++] = new OracleParameter(":Comment1", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);

				i = 0;
				sqlParams[i++].Value = mediarapModel.RapName;				
				sqlParams[i++].Value = mediarapModel.RapType;				
				sqlParams[i++].Value = mediarapModel.Tell;				
				sqlParams[i++].Value = mediarapModel.Comment;				
				sqlParams[i++].Value = mediarapModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // �����
				sqlParams[i++].Value = Convert.ToInt32(mediarapModel.RapCode);

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�̵���������:["+mediarapModel.RapCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediarapModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				mediarapModel.ResultCD   = "3201";
				mediarapModel.ResultDesc = "�̵����� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			// �����ͺ��̽���  Close�Ѵ�
			_db.Close();

		}

		/// <summary>
		/// �̵� ����
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetMediaRapCreate(HeaderModel header, MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[5];

				sbQuery.Append( ""
                    + "INSERT INTO MDA_REP (                         \n"
                    + "       REP_COD         \n"
                    + "      ,REP_NM        \n"
                    + "      ,REP_TP         \n"
                    + "      ,PHONE_NO         \n"
                    + "      ,REP_MEMO         \n"
                    + "		 ,USE_YN                \n"
                    + "      ,DT_INSERT         \n"
                    + "      ,DT_UPDATE         \n"
                    + "      ,ID_INSERT                                     \n"
					+ "      )                                          \n"
					+ " SELECT                                        \n"
                    + "       NVL(MAX(REP_COD),0)+1        \n"
					+ "      ,:RapName      \n"
					+ "      ,:RapType  \n" 
					+ "      ,:Tell     \n"
					+ "      ,:Comment1      \n"					
					+ "      ,'Y'      \n"
                    + "      ,SYSDATE      \n"
                    + "      ,SYSDATE      \n"
					+ "      ,:RegID         \n"
                    + "      FROM MDA_REP               \n"
					);

                sqlParams[i++] = new OracleParameter(":RapName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":RapType", OracleDbType.Char, 2);
                sqlParams[i++] = new OracleParameter(":Tell", OracleDbType.Varchar2, 15);
                sqlParams[i++] = new OracleParameter(":Comment1", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);

				i = 0;				
				sqlParams[i++].Value = mediarapModel.RapName;
				sqlParams[i++].Value = mediarapModel.RapType;				
				sqlParams[i++].Value = mediarapModel.Tell;
				sqlParams[i++].Value = mediarapModel.Comment;				
				sqlParams[i++].Value = header.UserID;				// �����

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�̵���������:[" + mediarapModel.RapCode + "(" + mediarapModel.RapName + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediarapModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				mediarapModel.ResultCD   = "3101";
				mediarapModel.ResultDesc = "�̵����� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}


		public void SetMediaRapDelete(HeaderModel header, MediaRapModel mediarapModel)
		{
            int MediaAgencyCount = 0;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbQueryMediaAgencyCount = new StringBuilder();
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[1];
                OracleParameter[] sqlParams2 = new OracleParameter[1];
                sbQueryMediaAgencyCount.Append( "\n"
                    + "        SELECT COUNT(*) FROM    MDA_AGNC			    \n"
                    + "              WHERE REP_COD  = :RapCode          	\n"
                    );  

				sbQuery.Append(""
                    + "DELETE MDA_REP         \n"
                    + " WHERE REP_COD  = :RapCode  \n"
					);

                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams2[0] = new OracleParameter(":RapCode", OracleDbType.Int32);
				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(mediarapModel.RapCode);
                sqlParams2[0].Value = Convert.ToInt32(mediarapModel.RapCode);
				// ��������
				try
				{
                    //��ü���౤���� ���� Count����///////////////////////////////////////////////
                    // __DEBUG__
                    _log.Debug(sbQueryMediaAgencyCount.ToString());
                    // __DEBUG__

                    // ��������
                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds,sbQueryMediaAgencyCount.ToString(),sqlParams);
                    
                    MediaAgencyCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                    _log.Debug("MediaAgencyCount          -->" + MediaAgencyCount);

                    // �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ� Exception�� �߻���Ų��.
                    if(MediaAgencyCount > 0) throw new Exception();


					_db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams2);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�̵���������:[" + mediarapModel.RapCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediarapModel.ResultCD = "0000";  // ����
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				mediarapModel.ResultCD   = "3301";
                // �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ�
                if(MediaAgencyCount > 0 )
                {
                    mediarapModel.ResultDesc = "��ϵ� ��ü����簡 �����Ƿ� �̵������� �����Ҽ� �����ϴ�.";
                }
                else
                {
                    mediarapModel.ResultDesc = "�̵����� ������ ������ �߻��Ͽ����ϴ�";
                }
                    _log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}
	}
}
