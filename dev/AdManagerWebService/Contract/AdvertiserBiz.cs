/*
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : 
 * ������    : 
 * ��������  :
 *             
 * --------------------------------------------------------
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : 
 * ������    : 
 * ��������  : 
 * --------------------------------------------------------
 */
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

namespace AdManagerWebService.Contract
{
	/// <summary>
	/// AdvertiserBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdvertiserBiz : BaseBiz
	{
		public AdvertiserBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}
		/// <summary>
		/// �����ָ����ȸ
		/// </summary>
		/// <param name="advertiserModel"></param>
		public void GetAdvertiserList(HeaderModel header, AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdvertiserList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + advertiserModel.SearchKey       + "]");
				_log.Debug("SearchAdvertiserLevel:[" + advertiserModel.SearchAdvertiserLevel + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
                #region ���� �� ��
                /*
                sbQuery.AppendLine("    SELECT	a.AdvertiserCode, a.AdvertiserName 		                                ");
                sbQuery.AppendLine("    	,	a.RapCode                                                               ");
                sbQuery.AppendLine("    	,	ISNULL(b.RapName,'����') AS RapName                                     ");
                sbQuery.AppendLine("    	,	a.Comment                                                               ");
                sbQuery.AppendLine("    	,	a.UseYn                                                                 ");
                sbQuery.AppendLine("    	,	CASE a.UseYn WHEN 'Y' THEN '' WHEN 'N' THEN '������' END AS UseYn_N   ");
                sbQuery.AppendLine("    	,	a.RegDt                                                                 ");
                sbQuery.AppendLine("    	,	a.ModDt                                                                 ");
                sbQuery.AppendLine("    	,	a.RegID                                                                 ");
                sbQuery.AppendLine("    	,	c1.JobCode                                                              "); // [A_01]
                sbQuery.AppendLine("    	,	c2.JobName as JobNameLevel1                                             "); // [A_01]
                sbQuery.AppendLine("    	,	c1.JobName as JobNameLevel2                                             "); // [A_01]
                sbQuery.AppendLine("    FROM Advertiser a with(NoLock)                                                  ");
                sbQuery.AppendLine("    LEFT JOIN MediaRap b with(NoLock) ON (b.RapCode = a.RapCode)                    ");
                sbQuery.AppendLine("    LEFT JOIN JobClass c1 with(nolock) ON (c1.JobCode = a.JobClass)                 "); // [A_01]
                sbQuery.AppendLine("    LEFT JOIN JobClass c2 with(nolock) ON (c2.JobCode = c1.Level1Code)              "); // [A_01]
                sbQuery.AppendLine("    WHERE 1 = 1                                                                     ");
				
				// �˻�� ������
				if (advertiserModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
						+ "    a.AdvertiserName      LIKE '%" + advertiserModel.SearchKey.Trim() + "%' \n"
                        + " OR a.Comment    LIKE '%" + advertiserModel.SearchKey.Trim() + "%' \n"   // [E_01]
						+ " ) ");
				}

				if(!advertiserModel.SearchRap.Equals("00"))
				{
					sbQuery.Append("  AND(  a.RapCode = '"+advertiserModel.SearchRap+"' OR a.RapCode = 0 ) \n");
				}      
				// ���ΰ� ���������� ��뿩�ΰ� 'Y', 'N' �����͸� �� ��ȸ�Ѵ�.
//				if (header.UserClass.Equals("10") || header.UserClass.Equals("20"))
//				{
//					sbQuery.Append(" AND UseYn = 'Y' OR UseYn = 'N' \n");
//				}
//				else
//				{
//					sbQuery.Append(" AND UseYn = 'Y' \n");
//				}

				if(advertiserModel.SearchchkAdState_10.Trim().Length > 0 && advertiserModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append(" AND a.UseYn = 'Y' OR a.UseYn = 'N' \n");   // [E_01]
				}	
				if(advertiserModel.SearchchkAdState_10.Trim().Length > 0 && advertiserModel.SearchchkAdState_10.Trim().Equals("N"))
				{
					sbQuery.Append(" AND  a.UseYn  = 'Y' \n");					
				}	
				// �����ַ����� ����������
				/*if(advertiserModel.SearchAdvertiserLevel.Trim().Length > 0 && !advertiserModel.SearchAdvertiserLevel.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode = '" + advertiserModel.SearchAdvertiserLevel.Trim() + "' \n");
				}*/			

				//sbQuery.Append(" ORDER BY a.AdvertiserCode DESC  \n");
                
                #endregion

                sbQuery.Append("\n" + " SELECT  ");
                sbQuery.Append("\n" + "    a.advter_cod as AdvertiserCode   "); 
                sbQuery.Append("\n" + "   ,a.advter_nm  as AdvertiserName   ");
                sbQuery.Append("\n" + "   ,a.rep_cod    as RapCode          ");
                sbQuery.Append("\n" + "   ,NVL(b.rep_nm,'����') as RapName  "); 
                sbQuery.Append("\n" + "   ,a.adver_memo as \"Comment\"      ");
                sbQuery.Append("\n" + "   ,a.use_yn     as UseYn            ");
                sbQuery.Append("\n" + "   ,CASE a.use_yn WHEN 'Y' THEN '' WHEN 'N' THEN '������' END AS UseYn_N   ");
                sbQuery.Append("\n" + "   ,a.dt_insert  as RegDt    ");        
                sbQuery.Append("\n" + "   ,a.dt_update  as ModDt    ");
                sbQuery.Append("\n" + "   ,a.id_insert  as RegID    ");                
                sbQuery.Append("\n" + " FROM ADVTER a   ");
                sbQuery.Append("\n" + " LEFT JOIN MDA_REP b ON (b.rep_cod = a.rep_cod)  ");
                sbQuery.Append("\n" + " WHERE 1 = 1     ");

                // �˻�� ������
                if (advertiserModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append(" AND ("
                        + "    a.advter_nm      LIKE '%" + advertiserModel.SearchKey.Trim() + "%' \n"
                        + " OR a.adver_memo    LIKE '%" + advertiserModel.SearchKey.Trim() + "%' \n"   // [E_01]
                        + " ) ");
                }

                if (!advertiserModel.SearchRap.Equals("00"))
                {
                    sbQuery.Append("  AND(  a.rep_cod = '" + advertiserModel.SearchRap + "' OR a.rep_cod = 0 ) \n");
                }

                if (advertiserModel.SearchchkAdState_10.Trim().Length > 0 && advertiserModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND a.use_yn = 'Y' OR a.use_yn = 'N' \n");   
                }
                if (advertiserModel.SearchchkAdState_10.Trim().Length > 0 && advertiserModel.SearchchkAdState_10.Trim().Equals("N"))
                {
                    sbQuery.Append(" AND  a.use_yn  = 'Y' \n");
                }
                
                sbQuery.Append(" ORDER BY a.advter_cod DESC  \n");

                // __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����ָ𵨿� ����
				advertiserModel.AdvertiserDataSet = ds.Copy();
				// ���
				advertiserModel.ResultCnt = Utility.GetDatasetCount(advertiserModel.AdvertiserDataSet);
				// ����ڵ� ��Ʈ
				advertiserModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + advertiserModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdvertiserList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				advertiserModel.ResultCD = "3000";
				advertiserModel.ResultDesc = "���������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		

		}

		/// <summary>
        /// ���������� ����
		/// </summary>
		/// <param name="header"></param>
		/// <param name="advertiserModel"></param>
		public void SetAdvertiserUpdate(HeaderModel header, AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;				
                OracleParameter[] sqlParams = new OracleParameter[6];

                sbQuery.AppendLine("UPDATE ADVTER                           ");
                sbQuery.AppendLine("\n " + "   SET advter_nm    = :AdvertiserName   ");
                sbQuery.AppendLine("\n " + "   ,rep_cod			= :RapCode          ");
                sbQuery.AppendLine("\n " + "   ,adver_memo		= :Comments  		");
                sbQuery.AppendLine("\n " + "   ,use_yn			= :UseYn      		");
                sbQuery.AppendLine("\n " + "   ,dt_update		= SYSDATE           ");
                sbQuery.AppendLine("\n " + "   ,id_update		= :RegID            ");
                sbQuery.AppendLine("\n " + "WHERE advter_cod = :AdvertiserCode     ");        
                

				i = 0;
                sqlParams[i++] = new OracleParameter(":AdvertiserName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":Comments", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":AdvertiserCode", OracleDbType.Int32);      
		

				i = 0;
				sqlParams[i++].Value = advertiserModel.AdvertiserName;		
				if(advertiserModel.RapCode.Trim().Length > 0)
				{
					sqlParams[i++].Value = Convert.ToInt32(advertiserModel.RapCode);	
				}
				else
				{
					sqlParams[i++].Value = 0;		
				}
                /* ���� ���п�..
                if (!advertiserModel.JobCode.Trim().Equals(string.Empty))    // [A_01]
                {
                    sqlParams[i++].Value = string.Format("{0,6}", advertiserModel.JobCode).Replace(" ", "0");
                }
                else
                {
                    sqlParams[i++].Value = "";
                }
                */
				sqlParams[i++].Value = advertiserModel.Comment;
				sqlParams[i++].Value = advertiserModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // �����
				sqlParams[i++].Value = Convert.ToInt32(advertiserModel.AdvertiserCode);

                _log.Debug("comment:[" + advertiserModel.Comment + "]");
                _log.Debug("useyn:[" + advertiserModel.UseYn + "]");                
                _log.Debug("advertiserCode:[" + advertiserModel.AdvertiserCode + "]");
                _log.Debug("RapCode:[" + advertiserModel.RapCode + "]");
                _log.Debug("AdvertiserName:[" + advertiserModel.AdvertiserName + "]");
                _log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("��������������:["+advertiserModel.AdvertiserCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				advertiserModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				advertiserModel.ResultCD   = "3201";
				advertiserModel.ResultDesc = "���������� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		

		}

		/// <summary>
		/// ������ ����
		/// </summary>
		/// <param name="UserID"></param>
		/// <param name="UserName"></param>
		/// <param name="UserPassword"></param>
		/// <param name="UserLevel"></param>
		/// <param name="UserDept"></param>
		/// <param name="UserTitle"></param>
		/// <param name="UserTell"></param>
		/// <param name="UserMobile"></param>
		/// <param name="UserComment"></param>
		/// <returns></returns>
		public void SetAdvertiserCreate(HeaderModel header, AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				
                OracleParameter[] sqlParams = new OracleParameter[4];
                sbQuery.Append("\n" + " INSERT INTO ADVTER(     ");
                sbQuery.Append("\n" + "   advter_cod            ");
                sbQuery.Append("\n" + "   ,advter_nm            ");
                sbQuery.Append("\n" + "   ,rep_cod              ");
                sbQuery.Append("\n" + "   ,adver_memo           ");    
                sbQuery.Append("\n" + "   ,use_yn               ");
                sbQuery.Append("\n" + "   ,dt_insert            ");
                sbQuery.Append("\n" + "   ,dt_update            ");
                sbQuery.Append("\n" + "   ,id_insert )          ");
                sbQuery.Append("\n" + " SELECT NVL(MAX(advter_cod),0)+1 ");
                sbQuery.Append("\n" + "       ,:AdvertiserName  ");
                sbQuery.Append("\n" + "       ,:RapCode         ");
                sbQuery.Append("\n" + "       ,:Comments         ");
                sbQuery.Append("\n" + "       ,'Y'              ");
                sbQuery.Append("\n" + "       ,SYSDATE          ");
                sbQuery.Append("\n" + "       ,SYSDATE          ");
                sbQuery.Append("\n" + "       ,:RegID           ");                
                sbQuery.Append("\n" + " FROM ADVTER             ");

                sqlParams[i++] = new OracleParameter(":AdvertiserName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":Comments", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);		

				i = 0;
				sqlParams[i++].Value = advertiserModel.AdvertiserName;
				if(advertiserModel.RapCode.Trim().Length > 0)
				{
					sqlParams[i++].Value = Convert.ToInt32(advertiserModel.RapCode);		
				}
				else
				{
					sqlParams[i++].Value = 0;		
				}
                
				sqlParams[i++].Value = advertiserModel.Comment;				
				sqlParams[i++].Value = header.UserID;      // �����
				
				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("��������������:[" + advertiserModel.AdvertiserCode + "(" + advertiserModel.AdvertiserName + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				advertiserModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				advertiserModel.ResultCD   = "3101";
				advertiserModel.ResultDesc = "���������� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		public void SetAdvertiserDelete(HeaderModel header, AdvertiserModel advertiserModel)
		{   
			int ClientCount = 0;
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQueryClientCount = new StringBuilder();
				int i = 0;
				int rc = 0;
				OracleParameter[] sqlParams = new OracleParameter[1];
                
				sbQueryClientCount.Append( "\n"
					+ "   SELECT COUNT(*) FROM    CLNT 	      \n"
					+ "   WHERE adver_cod  = :AdvertiserCode  \n"
					);      

				sbQuery.Append(""
					+ " DELETE ADVTER         \n"
					+ " WHERE advter_cod  = :AdvertiserCode  \n"					
					);

                sqlParams[i++] = new OracleParameter(":AdvertiserCode", OracleDbType.Int32);  // [E_01]

				i = 0;				
				sqlParams[i++].Value = advertiserModel.AdvertiserCode;

				// ��������
				try
				{
					//��ü���౤���� ���� Count����///////////////////////////////////////////////
					// __DEBUG__
					_log.Debug(sbQueryClientCount.ToString());
					// __DEBUG__

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQueryClientCount.ToString(),sqlParams);
                    
					ClientCount =  Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					ds.Dispose();

					_log.Debug("ClientCount          -->" + ClientCount);

					// �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ� Exception�� �߻���Ų��.
					if(ClientCount > 0) throw new Exception();

                    i = 0;
                    sqlParams[i++] = new OracleParameter(":AdvertiserCode", OracleDbType.Int32);  // [E_01]
                    i = 0;
                    sqlParams[i++].Value = advertiserModel.AdvertiserCode;
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("��������������:[" + advertiserModel.AdvertiserCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				advertiserModel.ResultCD = "0000";  // ����
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				advertiserModel.ResultCD   = "3301";
				// �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ�
				if(ClientCount > 0 )
				{
					advertiserModel.ResultDesc = "��ϵ� ��ü���౤���ְ� �����Ƿ� ������������ �����Ҽ� �����ϴ�.";
				}
				else
				{
					advertiserModel.ResultDesc = "������ ���� ������ ������ �߻��Ͽ����ϴ�";
					_log.Exception(ex);
				}
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

        /// <summary>
        /// [A_01]
        /// ���� ���� ���
        /// </summary>
        /// <param name="contractModel"></param>
        public void GetJobClassList(HeaderModel header, AdvertiserModel model)
        {
            try
            {  // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetJobClassList() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.AppendLine("    Select	JobCode                     ");
                sbQuery.AppendLine("    	,	JobName                     ");
                sbQuery.AppendLine("    	,	Level1Code as JobUpperCode  ");
                sbQuery.AppendLine("    	,	Level	   as JobLevel      ");
                sbQuery.AppendLine("    From JobClass with(nolock)          ");
                sbQuery.AppendLine("    Where  1 = 1                        ");
                sbQuery.AppendLine("    AND (Level = 1 or Level = 2);       ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� ������Ű���𵨿� ����
                model.AdvertiserDataSet = ds.Copy();
                // ���
                model.ResultCnt = Utility.GetDatasetCount(model.AdvertiserDataSet);
                // ����ڵ� ��Ʈ
                model.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetJobClassList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "���� ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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