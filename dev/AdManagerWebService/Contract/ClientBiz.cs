/*
 * -------------------------------------------------------
 * [�߰�]
 * -------------------------------------------------------
 * �����ڵ�  : [A_01]
 * ������    : bae
 * ������    : 2015.11
 * ��������  : ���౤����(���౤����) �߰�,����,����,��ȸ
 *             
 * --------------------------------------------------------
 * -------------------------------------------------------
 * [����]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
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
	/// ClientBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ClientBiz : BaseBiz
	{
		public ClientBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}

        /// <summary>
        /// ������ �޺�
        /// </summary>
        /// <param name="header"></param>
        /// <param name="clientModel"></param>
	    public void GetAdvertiserList(HeaderModel header, ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdvertiserList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				 //__DEBUG__
				_log.Debug("<�Է�����>");
				//_log.Debug("AdvertiserCode :[" + ClientModel.AdvertiserCode + "]");
				 //__DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
                    + " SELECT advter_cod as AdvertiserCode, advter_nm as AdvertiserName\n"
					+ "   FROM ADVTER                \n"
					+ "   WHERE use_yn = 'Y'         \n"					
					);

                sbQuery.Append(" ORDER BY advter_cod \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				clientModel.ClientDataSet = ds.Copy();
				// ���
				clientModel.ResultCnt = Utility.GetDatasetCount(clientModel.ClientDataSet);
				// ����ڵ� ��Ʈ
				clientModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + clientModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdvertiserList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				clientModel.ResultCD = "3000";
				clientModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		

		}

		/// <summary>
		/// ��ü����� ��ü �����ȸ
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientMediaList(HeaderModel header, ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientMediaList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + clientModel.SearchKey       + "]");
				_log.Debug("SearchClient:[" + clientModel.SearchAdvertiserName + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// ��������
				sbQuery.Append("\n"
					+ " SELECT						  \n"							
					+ "   A.mda_cod as MediaCode                \n"									
					+ "  ,B.mda_nm as MediaName                \n"											
					+ "  FROM           \n"					
					+ " MDA_AGNC a, MDA b   \n"						
					+ " WHERE a.mda_cod =  b.mda_cod  \n"						
					+ " group by a.mda_cod, b.mda_nm  \n"				
							
					);
								
				sbQuery.Append(" ORDER BY a.mda_cod DESC  \n");

				// __DEBUG__
				_log.Debug("MediaCode:[" + clientModel.MediaCode_C + "]");
				_log.Debug("RapCode:[" + clientModel.RapCode_C + "]");
				_log.Debug("AgencyCode:[" + clientModel.AgencyCode_C + "]");
				//_log.Debug("AdvertiserCode:[" + clientModel.AdvertiserCode + "]");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				clientModel.ClientDataSet = ds.Copy();
				// ���
				clientModel.ResultCnt = Utility.GetDatasetCount(clientModel.ClientDataSet);
				// ����ڵ� ��Ʈ
				clientModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + clientModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientMediaList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				clientModel.ResultCD = "3000";
				clientModel.ResultDesc = "��ü����籤�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// ��ü����� ��������ȸ
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientRapList(HeaderModel header, ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientRapList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + clientModel.SearchKey       + "]");
				_log.Debug("SearchClient:[" + clientModel.SearchAdvertiserName + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// ��������
				sbQuery.Append("\n"
					+ " SELECT					     \n"							
					+ "   a.mda_cod as MediaCode     \n"						
					+ "  ,a.rep_cod as RapCode       \n"									
					+ "  ,b.rep_nm  as RapName       \n"											
					+ "  FROM           \n"					
					+ " MDA_AGNC a , MDA_REP b   \n"						
					+ " WHERE a.rep_cod =  b.rep_cod  \n"
					+ "   AND b.use_yn = 'Y'         \n"					
																
					);
				

				// ��ü����籤���ַ����� ����������
				if(clientModel.MediaCode_C.Trim().Length > 0 && !clientModel.MediaCode_C.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.mda_cod = '" + clientModel.MediaCode_C.Trim() + "' \n");
				}				
				
				sbQuery.Append(" group by a.mda_cod, a.rep_cod, b.rep_nm  \n");
				sbQuery.Append(" ORDER BY a.mda_cod DESC  \n");

				// __DEBUG__
				_log.Debug("MediaCode:[" + clientModel.MediaCode_C + "]");
				_log.Debug("RapCode:[" + clientModel.RapCode_C + "]");
				_log.Debug("AgencyCode:[" + clientModel.AgencyCode_C + "]");
				//_log.Debug("AdvertiserCode:[" + clientModel.AdvertiserCode + "]");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				clientModel.ClientDataSet = ds.Copy();
				// ���
				clientModel.ResultCnt = Utility.GetDatasetCount(clientModel.ClientDataSet);
				// ����ڵ� ��Ʈ
				clientModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + clientModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientMediaList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				clientModel.ResultCD = "3000";
				clientModel.ResultDesc = "��ü����� �������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// ��ü����� ����� �����ȸ
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientAgencyList(HeaderModel header, ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientAgencyList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + clientModel.SearchKey       + "]");
				_log.Debug("SearchClient:[" + clientModel.SearchAdvertiserName + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// ��������
				sbQuery.Append("\n"
					+ " SELECT						  \n"							
					+ "   A.mda_cod as MediaCode            \n"						
					+ "  ,A.rep_cod as RapCode              \n"									
					+ "  ,A.agnc_cod  as AgencyCode         \n"
					+ "  ,B.agnc_nm   as AgencyName         \n"											
					+ "  FROM           \n"					
					+ " MDA_AGNC a , AGNC b  \n"
                    + " WHERE a.agnc_cod =  b.agnc_cod  \n"
					+ "   AND b.use_yn = 'Y'         \n"					
																
					);
				

				// ��ü����籤���ַ����� ����������
				if(clientModel.MediaCode_C.Trim().Length > 0 && !clientModel.MediaCode_C.Trim().Equals("00"))
				{
                    sbQuery.Append(" AND A.mda_cod = '" + clientModel.MediaCode_C.Trim() + "' \n");
				}
				if(clientModel.RapCode_C.Trim().Length > 0 && !clientModel.RapCode_C.Trim().Equals("00"))
				{
                    sbQuery.Append(" AND A.rep_cod = '" + clientModel.RapCode_C.Trim() + "' \n");
				}

                sbQuery.Append(" group by A.mda_cod, A.rep_cod, A.agnc_cod, B.agnc_nm  \n");
                sbQuery.Append(" ORDER BY A.mda_cod DESC  \n");

				// __DEBUG__
				_log.Debug("MediaCode:[" + clientModel.MediaCode_C + "]");
				_log.Debug("RapCode:[" + clientModel.RapCode_C + "]");
				_log.Debug("AgencyCode:[" + clientModel.AgencyCode_C + "]");
				//_log.Debug("AdvertiserCode:[" + clientModel.AdvertiserCode + "]");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				clientModel.ClientDataSet = ds.Copy();
				// ���
				clientModel.ResultCnt = Utility.GetDatasetCount(clientModel.ClientDataSet);
				// ����ڵ� ��Ʈ
				clientModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + clientModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientAgencyList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				clientModel.ResultCD = "3000";
				clientModel.ResultDesc = "��ü����籤�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// ��ü����� �����ָ����ȸ
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientAdvertiserList(HeaderModel header, ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientAdvertiserList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + clientModel.SearchKey       + "]");
				_log.Debug("SearchClient:[" + clientModel.SearchAdvertiserName + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// ��������
				sbQuery.Append("\n"
					+ " SELECT						  \n"					
					+ "   a.advter_cod as AdvertiserCode                \n"
					+ "  ,a.advter_nm as AdvertiserName                \n"											
					+ "  FROM           \n"					
					+ " ADVTER a   \n"																
					+ " LEFT JOIN MDA_REP   b  ON (a.rep_cod = b.rep_cod)  \n"																
					+ " WHERE a.use_yn = 'Y'  \n"																
					);

				if(!clientModel.SearchRap.Equals("00"))
				{
					sbQuery.Append("  AND(  a.rep_cod = '"+clientModel.SearchRap+"' OR a.RapCode = 0 ) \n");
				}    

				if (clientModel.SearchKey.Trim().Length > 0)
				{
					sbQuery.Append("  AND a.advter_nm    LIKE '%" + clientModel.SearchKey.Trim() + "%' \n");
				}	
				

				// ��ü����籤���ַ����� ����������
				
				sbQuery.Append(" ORDER BY a.advter_cod DESC  \n");

				// __DEBUG__				
				_log.Debug("AdvertiserCode:[" + clientModel.AdvertiserCode + "]");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				clientModel.ClientDataSet = ds.Copy();
				// ���
				clientModel.ResultCnt = Utility.GetDatasetCount(clientModel.ClientDataSet);
				// ����ڵ� ��Ʈ
				clientModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + clientModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientAdvertiserList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				clientModel.ResultCD = "3000";
				clientModel.ResultDesc = "��ü����籤�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

        /// <summary>
        /// ���౤���� �����ָ����ȸ
        /// </summary>
        /// <param name="clientModel"></param>
        public void GetClientAdvertiserListByCombo(HeaderModel header, ClientModel clientModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientAdvertiserListByCombo() Start");
                _log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + clientModel.SearchKey       + "]");
                _log.Debug("SearchClient:[" + clientModel.SearchAdvertiserName + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
			
                // ��������
                sbQuery.Append("\n");
                sbQuery.Append("SELECT                                              \n");				           	
                sbQuery.Append("        a.adver_cod as AdvertiserCode           				    \n");
                sbQuery.Append("       ,b.advter_nm as AdvertiserName      					    \n");
                sbQuery.Append("FROM   CLNT a  LEFT JOIN ADVTER b	 \n");
                sbQuery.Append("        ON (a.adver_cod = b.advter_cod)    \n");
                sbQuery.Append("WHERE   1 = 1 				                        \n");
                sbQuery.Append("AND mda_cod = '"+clientModel.MediaCode+"'         \n");
                sbQuery.Append("AND rep_cod = '"+clientModel.RapCode+"'           \n");
                sbQuery.Append("AND agnc_cod ='"+clientModel.AgencyCode+"'        \n");
				

                // ��ü����籤���ַ����� ����������
                sbQuery.Append(" ORDER BY advter_cod DESC  \n");

                // __DEBUG__				
                _log.Debug("AdvertiserCode:[" + clientModel.AdvertiserCode + "]");

                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // ��� DataSet�� ��ü����籤���ָ𵨿� ����
                clientModel.ClientDataSet = ds.Copy();
                // ���
                clientModel.ResultCnt = Utility.GetDatasetCount(clientModel.ClientDataSet);
                // ����ڵ� ��Ʈ
                clientModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + clientModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientAdvertiserListByCombo() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                clientModel.ResultCD = "3000";
                clientModel.ResultDesc = "��ü����籤�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
        }


		/// <summary>
		/// ���౤����(���౤����) ��� ��ȸ
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientList(HeaderModel header, ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + clientModel.SearchKey       + "]");
				_log.Debug("SearchClient:[" + clientModel.SearchAdvertiserName + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT						  \n"							
					+ "   A.mda_cod as MediaCode                \n"															
					+ "  ,A.rep_cod as RapCode                \n"															
					+ "  ,A.agnc_cod as  AgencyCode                \n"	
					+ "  ,A.adver_cod as AdvertiserCode                \n"	
					+ "  ,B.mda_nm as MediaName                \n"							
					+ "  ,C.rep_nm as RapName                \n"			
					+ "  ,D.agnc_nm as AgencyName                \n"							
					+ "  ,E.advter_nm as AdvertiserName                \n"																						
					+ "  ,A.clnt_memo as \"Comment\"                \n"															
					+ "  ,A.use_yn as UseYn                \n"		
					+ "  ,CASE A.use_yn WHEN 'Y' THEN '' WHEN 'N' THEN '������' END AS UseYn_N  \n"								
					+ "  ,A.dt_insert as RegDt                \n"															
					+ "  ,A.dt_update as ModDt                \n"															
					+ "  ,A.id_insert as RegID                \n"															
					+ "  FROM CLNT a  \n"
                    + "       LEFT JOIN MDA      b  ON A.mda_cod      = B.mda_cod \n"
                    + "       LEFT JOIN MDA_REP  c  ON A.rep_cod        = C.rep_cod \n"
                    + "       LEFT JOIN AGNC     d  ON A.agnc_cod     = D.agnc_cod \n"	
					+ "       LEFT JOIN ADVTER e ON a.adver_cod = e.advter_cod \n"	
					+ " WHERE 1 = 1  \n"				
					);
				
				// �˻�� ������
				if (clientModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
						+ "    B.mda_nm      LIKE '%" + clientModel.SearchKey.Trim() + "%' \n"		
						+ " OR C.rep_nm    LIKE '%" + clientModel.SearchKey.Trim() + "%' \n"
						+ " OR D.agnc_nm    LIKE '%" + clientModel.SearchKey.Trim() + "%' \n"
						+ " OR E.advter_nm   LIKE '%" + clientModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}				
				// ���ΰ� ���������� ��뿩�ΰ� 'Y', 'N' �����͸� �� ��ȸ�Ѵ�.
//				if (header.UserClass.Equals("10") || header.UserClass.Equals("20"))
//				{
//					sbQuery.Append(" AND A.UseYn = 'Y' OR A.UseYn = 'N' \n");
//				}
//				else
//				{
//					sbQuery.Append(" AND A.UseYn = 'Y' \n");
//				}
				if(clientModel.SearchchkAdState_10.Trim().Length > 0 && clientModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND A.use_yn = 'Y' OR A.use_yn = 'N' \n");
				}	
				if(clientModel.SearchchkAdState_10.Trim().Length > 0 && clientModel.SearchchkAdState_10.Trim().Equals("N"))
				{
					sbQuery.Append(" AND  A.use_yn  = 'Y' \n");					
				}	
				// ��ü����籤���ַ����� ����������
				if(clientModel.SearchMediaName.Trim().Length > 0 && !clientModel.SearchMediaName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.mda_cod = '" + clientModel.SearchMediaName.Trim() + "' \n");
				}
				if(clientModel.SearchRapName.Trim().Length > 0 && !clientModel.SearchRapName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.rep_cod = '" + clientModel.SearchRapName.Trim() + "' \n");
				}		
				if(clientModel.SearchMediaAgency.Trim().Length > 0 && !clientModel.SearchMediaAgency.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.agnc_cod = '" + clientModel.SearchMediaAgency.Trim() + "' \n");
				}		
				if(clientModel.SearchAdvertiserName.Trim().Length > 0 && !clientModel.SearchAdvertiserName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.adver_cod = '" + clientModel.SearchAdvertiserName.Trim() + "' \n");
				}						

				sbQuery.Append(" ORDER BY A.mda_cod, A.rep_cod, A.agnc_cod, A.adver_cod DESC  \n");

				_log.Debug("UserClass      :[" + header.UserClass+ "]");
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				clientModel.ClientDataSet = ds.Copy();
				// ���
				clientModel.ResultCnt = Utility.GetDatasetCount(clientModel.ClientDataSet);				
				// ����ڵ� ��Ʈ
				clientModel.ResultCD = "0000";			
				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + clientModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				clientModel.ResultCD = "3000";
				clientModel.ResultDesc = "��ü����籤�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// ���౤���� ����
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
		public void SetClientUpdate(HeaderModel header, ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientUpdate() Start");
				_log.Debug("-----------------------------------------");
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[7];

				sbQuery.Append(""
					+ "UPDATE CLNT                     \n"
					+ "   SET clnt_memo      = :Comments      \n"													
					+ "      ,use_yn		 = :UseYn      \n"														
					+ "      ,dt_update		 = SYSDATE      \n"
					+ "      ,id_update 	 = :RegID         \n"
					+ " WHERE mda_cod        = :MediaCode        \n"					
					+ " AND rep_cod			 = :RapCode        \n"					
					+ " AND agnc_cod         = :AgencyCode        \n"					
					+ " AND adver_cod        = :AdvertiserCode        \n"					
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":Comments", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);

                sqlParams[i++] = new OracleParameter(":MediaCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AdvertiserCode", OracleDbType.Int32);
						
				i = 0;
				sqlParams[i++].Value = clientModel.Comment;																	
				sqlParams[i++].Value = clientModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // �����

				sqlParams[i++].Value = Convert.ToInt32(clientModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(clientModel.RapCode);
				sqlParams[i++].Value = Convert.ToInt32(clientModel.AgencyCode);
				sqlParams[i++].Value = Convert.ToInt32(clientModel.AdvertiserCode);

				
				

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("��ü����籤������������:["+clientModel.AgencyCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				clientModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				clientModel.ResultCD   = "3201";
				clientModel.ResultDesc = "��ü����籤�������� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		/// ���౤���� ����
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
		public void SetClientCreate(HeaderModel header, ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[6];
		

				sbQuery.Append( ""
					+ "INSERT INTO CLNT (                         \n"
					+ "		 mda_cod                \n"															
					+ "		,rep_cod                \n"															
					+ "		,agnc_cod                \n"															
					+ "		,adver_cod                \n"															
					+ "		,clnt_memo                \n"																														
					+ "		,use_yn                \n"															
					+ "		,dt_insert                \n"																				
					+ "		,dt_update                \n"																				
					+ "		,id_insert                \n"							
					+ "      )                                          \n"
					+ " VALUES(                                         \n"			
					+ "       :MediaCode      \n"						
					+ "      ,:RapCode      \n"		
					+ "      ,:AgencyCode      \n"		
					+ "      ,:AdvertiserCode      \n"		
					+ "      ,:Comments      \n"														
					+ "      ,'Y'      \n"	
					+ "      ,SYSDATE      \n"	
					+ "      ,SYSDATE      \n"	
					+ "      ,:RegID         \n"
					+ " )		\n"					
					);

                sqlParams[i++] = new OracleParameter(":MediaCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AdvertiserCode", OracleDbType.Int32);

                sqlParams[i++] = new OracleParameter(":Comments", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);		
	

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(clientModel.MediaCode);
                sqlParams[i++].Value = Convert.ToInt32(clientModel.RapCode);
                sqlParams[i++].Value = Convert.ToInt32(clientModel.AgencyCode);
                sqlParams[i++].Value = Convert.ToInt32(clientModel.AdvertiserCode);

				sqlParams[i++].Value = clientModel.Comment;																						
				sqlParams[i++].Value = header.UserID;      // �����
		
				
				_log.Debug("MediaCode:[" + clientModel.MediaCode + "]");
				_log.Debug("RapCode:[" + clientModel.RapCode + "]");
				_log.Debug("AgencyCode:[" + clientModel.AgencyCode + "]");
				_log.Debug("AdvertiserCode:[" + clientModel.AdvertiserCode + "]");

				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("��ü����籤������������:[" + clientModel.AdvertiserCode + "(" + clientModel.AdvertiserCode + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				clientModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				clientModel.ResultCD   = "3101";
				clientModel.ResultDesc = "��ü����籤�������� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}


		public void SetClientDelete(HeaderModel header, ClientModel clientModel)
		{
            int ContractCount = 0;
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbQueryContractCount = new StringBuilder();
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[4];

                sbQueryContractCount.Append(                "\n"
                    + "SELECT COUNT(*) FROM CNTR  \n"
                    + " WHERE mda_cod  = :MediaCode  \n"					
                    + " AND rep_cod  = :RapCode  \n"					
                    + " AND agnc_cod  = :AgencyCode  \n"					
                    + " AND advter_cod  = :AdvertiserCode  \n"					
                    );   
				sbQuery.Append(""
					+ "DELETE CLNT         \n"
					+ " WHERE mda_cod  = :MediaCode  \n"					
					+ " AND rep_cod  = :RapCode  \n"					
					+ " AND agnc_cod  = :AgencyCode  \n"					
					+ " AND adver_cod  = :AdvertiserCode  \n"					
					);

                sqlParams[i++] = new OracleParameter(":MediaCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AdvertiserCode", OracleDbType.Int32);

				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(clientModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(clientModel.RapCode);
				sqlParams[i++].Value = Convert.ToInt32(clientModel.AgencyCode);
				sqlParams[i++].Value = Convert.ToInt32(clientModel.AdvertiserCode);

				_log.Debug("MediaCode:[" + clientModel.MediaCode + "]");
				_log.Debug("MediaCode:[" + clientModel.RapCode + "]");
				_log.Debug("MediaCode:[" + clientModel.AgencyCode + "]");
				_log.Debug("MediaCode:[" + clientModel.AdvertiserCode + "]");

				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
                    //������ ���� Count����///////////////////////////////////////////////
                    // __DEBUG__
                    _log.Debug(sbQueryContractCount.ToString());
                    // __DEBUG__

                    // ��������
                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds,sbQueryContractCount.ToString(),sqlParams);
                    
                    ContractCount =  Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
					
					ds.Dispose();

                    _log.Debug("ContractCount          -->" + ContractCount);

                    // �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ� Exception�� �߻���Ų��.
                    if(ContractCount > 0) throw new Exception();

                    OracleParameter[] sqlParams2 = new OracleParameter[4];
                    i = 0;
                    sqlParams2[i++] = new OracleParameter(":MediaCode", OracleDbType.Int32);
                    sqlParams2[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                    sqlParams2[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);
                    sqlParams2[i++] = new OracleParameter(":AdvertiserCode", OracleDbType.Int32);

                    i = 0;
                    sqlParams2[i++].Value = Convert.ToInt32(clientModel.MediaCode);
                    sqlParams2[i++].Value = Convert.ToInt32(clientModel.RapCode);
                    sqlParams2[i++].Value = Convert.ToInt32(clientModel.AgencyCode);
                    sqlParams2[i++].Value = Convert.ToInt32(clientModel.AdvertiserCode);

					_db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams2);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("��ü����籤������������:[" + clientModel.AdvertiserCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				clientModel.ResultCD = "0000";  // ����
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				clientModel.ResultCD   = "3301";
                // �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ�
                if(ContractCount > 0 )
                {
                    clientModel.ResultDesc = "��ϵ� ������������ �����Ƿ� ��ü����籤���������� �����Ҽ� �����ϴ�.";
                }
                else
                {
                    clientModel.ResultDesc = "��ü����籤�������� ������ ������ �߻��Ͽ����ϴ�";
					_log.Exception(ex);
				}
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}
	}
}
