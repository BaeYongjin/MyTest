using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Target
{

	/// <summary>
	/// 
	/// </summary>
	public class CrmJobBiz : BaseBiz
	{
		public CrmJobBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		/// CRM�۾� �����ȸ
		/// </summary>
		/// <param name="header"></param>
		/// <param name="data"></param>
		public void GetCrmJobList(HeaderModel header, CrmJobModel	data)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCrmJobList() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();
				sbQuery.Append("\n select	a.CampaignId ");
				sbQuery.Append("\n 			, a.ItemNo ");
				sbQuery.Append("\n 			, a.WorkDay ");
				sbQuery.Append("\n 			, a.WorkStatus ");
				sbQuery.Append("\n 			, c.CodeName		as WorkStatusName ");
				sbQuery.Append("\n 			, a.BeginDay ");
				sbQuery.Append("\n 			, a.EndDay ");
				sbQuery.Append("\n 			, a.CollectionCode  ");
				sbQuery.Append("\n 			, isnull(d.CollectionName,'')	as CollectionName ");
				sbQuery.Append("\n 			, a.UserCnt ");
				sbQuery.Append("\n 			, a.StbCnt ");
				sbQuery.Append("\n 			, a.BatchStart ");
				sbQuery.Append("\n 			, a.BatchEnd ");
				sbQuery.Append("\n 			, b.ItemName ");
				sbQuery.Append("\n from	CrmJob	a	with(noLock) ");
				sbQuery.Append("\n		inner join	ContractItem	b with(noLock)	on	b.ItemNo	= a.ItemNo ");
				sbQuery.Append("\n 		inner join	SystemCode		c with(noLock)	on	c.Code		= a.WorkStatus and c.Section = '91' ");
				sbQuery.Append("\n 		left outer join Collection	d with(noLock)	on	d.CollectionCode = a.CollectionCode ");
				sbQuery.Append("\n order by a.CampaignId desc ");
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				_db.Open();
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �̵��𵨿� ����
				data.DsCrmJob	= ds.Copy();
				data.ResultCnt	= Utility.GetDatasetCount(data.DsCrmJob);
				data.ResultCD = "0000";

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCrmJobList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "CRM�۾�����Ʈ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}

		/// <summary>
		/// ��ž����Ʈ�� �о�´�
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetStbList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{  // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStbList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn        \n"
					+ "		  ,ServiceNum as    UserId  \n"     
					+ "		  ,StbId					\n"
					+ "       ,PostNo					\n"
					+ "       ,ServiceCode				\n"
					+ "		  ,ResidentNo				\n"
					+ "		  ,Sex						\n"
					+ "       ,CASE Sex WHEN 'M' THEN '��' WHEN 'F' THEN '��' END AS Sex_N  \n"
					+ "		  ,RegDt					\n"					
					+ "  FROM StbUser with(noLock)      \n"				
					+ " WHERE  1 = 1					\n"
					);
				
				// �˻�� ������
				if (targetcollectionModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append("\n"
						+ "  AND ( StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
						+ "     OR PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
						+ "     OR ServiceNum   LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
						+ " 	)       \n"
						);
				}
       
				sbQuery.Append("  ORDER BY ServiceNum, PostNo, ResidentNo, Sex");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ������Ű���𵨿� ����
				targetcollectionModel.StbListDataSet = ds.Copy();
				// ���
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.StbListDataSet);
				// ����ڵ� ��Ʈ
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStbList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "Stb ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}

		/// <summary>
		/// ������Ʈ
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetClientList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{  // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				
				// ��������
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn            \n"
					+ "       ,A.CollectionCode			\n"     
					+ "		  ,A.UserId					\n"	
					+ "		  ,B.StbId					\n"	
					+ "		  ,B.PostNo					\n"	
					+ "		  ,B.ServiceCode			\n"	
					+ "		  ,B.ResidentNo				\n"	
					+ "		  ,B.Sex					\n"	
					+ "       ,CASE B.Sex WHEN 'M' THEN '��' WHEN 'F' THEN '��' END AS Sex_N  \n"
					+ "		  ,A.RegDt					\n"										
					+ "  FROM ClientList A LEFT JOIN StbUser B with(NoLock) ON (A.UserId          = B.UserId)        \n"					
					+ " WHERE  1 = 1					\n"
					);
				
				// �˻�� ������
//				if (targetcollectionModel.SearchKey.Trim().Length > 0)
//				{
//					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
//					sbQuery.Append("\n"
//						+ "  AND ( StbId		  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR PostNo	      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR ResidentNo     LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR Sex		      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ " 	)       \n"
//						);
//				}

				// ��ü����籤���ַ����� ����������
				if(targetcollectionModel.CollectionCode.Trim().Length > 0 && !targetcollectionModel.CollectionCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "' \n");
				}		
       
				sbQuery.Append("  ORDER BY A.CollectionCode, A.UserId");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ������Ű���𵨿� ����
				targetcollectionModel.ClientListDataSet = ds.Copy();
				// ���
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);
				// ����ڵ� ��Ʈ
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "�� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}

		/// <summary>
		/// ������Ʈ-����¡
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetClientPageList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{  // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientPageList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				
				// ��������
				sbQuery.Append("\n"
					+ "SELECT top (" + targetcollectionModel.PageSize.Trim() + ")            \n"
					+ "       'False' AS CheckYn			\n"     
					+ "       ,A.CollectionCode			\n"     
					+ "		  ,A.UserId					\n"	
					+ "		  ,B.StbId					\n"	
					+ "		  ,B.PostNo					\n"						
					+ "		  ,B.ServiceCode			\n"	
					+ "		  ,B.ResidentNo				\n"	
					+ "		  ,B.Sex					\n"	
					+ "       ,CASE B.Sex WHEN 'M' THEN '��' WHEN 'F' THEN '��' END AS Sex_N  \n"
					+ "		  ,A.RegDt					\n"										
					+ "		  ,C.SummaryName DongName					\n"										
					+ "  FROM ClientList A LEFT JOIN StbUser B with(NoLock) ON (A.UserId          = B.UserId)        \n"					
					+ "					   LEFT JOIN SummaryCode C with(NoLock) ON ((select dbo.ufnGetTargetRegionCode2(substring(B.PostNo,0,4))) = C.SummaryCode AND C.SummaryType=5)        \n"					
					+ " WHERE  1 = 1					\n"
					);
				
				// �˻�� ������
				//				if (targetcollectionModel.SearchKey.Trim().Length > 0)
				//				{
				//					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
				//					sbQuery.Append("\n"
				//						+ "  AND ( StbId		  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR PostNo	      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR ResidentNo     LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR Sex		      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ " 	)       \n"
				//						);
				//				}

				sbQuery.Append(" AND A.UserId not in(select top ((" + targetcollectionModel.Page.Trim() + "-1)*" + targetcollectionModel.PageSize.Trim() + ")UserId \n");
				sbQuery.Append("						from 	ClientList \n");
				sbQuery.Append("					ORDER BY CollectionCode,UserId) \n");				
				
				if(targetcollectionModel.CollectionCode.Trim().Length > 0 && !targetcollectionModel.CollectionCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "' \n");
				}		
       
				sbQuery.Append("  ORDER BY A.CollectionCode, A.UserId");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ������Ű���𵨿� ����
				targetcollectionModel.ClientListDataSet = ds.Copy();
				// ���
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);
				// ����ڵ� ��Ʈ
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientPageList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "�� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}

		/// <summary>
		/// Ÿ�ٱ����� ����
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
		public void SetTargetCollectionUpdate(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[5];

				sbQuery.Append(""
					+ "UPDATE Collection                   \n"
					+ "   SET CollectionName      = @CollectionName      \n"										
					+ "      ,Comment		= @Comment   \n"			
					+ "      ,UseYn			= @UseYn      \n"	
					+ "      ,ModDt         = GETDATE()      \n"
					+ "      ,RegID         = @RegID         \n"
					+ " WHERE CollectionCode        = @CollectionCode        \n"
					);

				i = 0;
				sqlParams[i++] = new SqlParameter("@CollectionName"     , SqlDbType.VarChar , 500);												
				sqlParams[i++] = new SqlParameter("@Comment"  , SqlDbType.VarChar , 2000);		
				sqlParams[i++] = new SqlParameter("@UseYn"  , SqlDbType.Char , 1);
				sqlParams[i++] = new SqlParameter("@RegID"        , SqlDbType.VarChar , 10);
				sqlParams[i++] = new SqlParameter("@CollectionCode"       , SqlDbType.Int);

				i = 0;
				sqlParams[i++].Value = targetcollectionModel.CollectionName;								
				sqlParams[i++].Value = targetcollectionModel.Comment;				
				sqlParams[i++].Value = targetcollectionModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // �����
				sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ÿ�ٱ���������:["+targetcollectionModel.CollectionCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3201";
				targetcollectionModel.ResultDesc = "Ÿ�ٱ����� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			// �����ͺ��̽���  Close�Ѵ�
			_db.Close();

		}

		/// <summary>
		/// Ÿ�ٱ� ����
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
		public void SetTargetCollectionCreate(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[3];

				sbQuery.Append( ""
					+ "INSERT INTO Collection (                         \n"
					+ "       CollectionCode         \n"
					+ "      ,CollectionName        \n"					
					+ "      ,Comment         \n"
					+ "		 ,UseYn                \n"															
					+ "      ,RegDt         \n"
					+ "      ,ModDt         \n"
					+ "      ,RegID                                     \n"
					+ "      )                                          \n"
					+ " SELECT                                        \n"
					+ "       ISNULL(MAX(CollectionCode),0)+1        \n"
					+ "      ,@CollectionName      \n"					
					+ "      ,@Comment      \n"					
					+ "      ,'Y'      \n"					
					+ "      ,GETDATE()      \n"
					+ "      ,GETDATE()      \n"
					+ "      ,@RegID         \n"
					+ " FROM Collection               \n"
					);
				
				sqlParams[i++] = new SqlParameter("@CollectionName"     , SqlDbType.VarChar , 40);				
				sqlParams[i++] = new SqlParameter("@Comment"     , SqlDbType.VarChar , 50);				
				sqlParams[i++] = new SqlParameter("@RegID"        , SqlDbType.VarChar , 10);

				i = 0;				
				sqlParams[i++].Value = targetcollectionModel.CollectionName;				
				sqlParams[i++].Value = targetcollectionModel.Comment;				
				sqlParams[i++].Value = header.UserID;				// �����

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ÿ�ٱ���������:[" + targetcollectionModel.CollectionCode + "(" + targetcollectionModel.CollectionName + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3101";
				targetcollectionModel.ResultDesc = "Ÿ�ٱ����� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		/// ������Ʈ ����
		/// </summary>
		public void SetClientCreate(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

                int     iCollectionCode =   Convert.ToInt32(targetcollectionModel.CollectionCode);
                string  iServiceNum     =   targetcollectionModel.UserId.ToString();

				StringBuilder sbQuery = new StringBuilder();
                #region [ ���翩�� Ȯ�ι� ��Ͽ��� Ȯ�� ]
                // �ش簡���� ����������� �����ϴ��� �˻�
                sbQuery.Append(" select	1 as gubun ,count(*) as cnt "           + "\n");
                sbQuery.Append(" from	StbUser with(noLock) "                  + "\n");
                sbQuery.Append(" where  ServiceNum = '" + iServiceNum + "'"     + "\n");
                sbQuery.Append(" union all " + "\n");
                // ���ϵ� ���������� �˻�
                sbQuery.Append(" select  2 as gubun, count(*) as cnt " + "\n");
                sbQuery.Append(" from   ClientList with(noLock) " + "\n");
                sbQuery.Append(" where	CollectionCode	= " +  iCollectionCode  + "\n");
                sbQuery.Append(" and    ServiceNum  = '" + iServiceNum + "'"    + "\n");
                sbQuery.Append(" order by gubun " + "\n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                if(ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception();
                }
                #endregion

                int userFound   =   Convert.ToInt32(ds.Tables[0].Rows[0]["cnt"].ToString());    // ����������� ��ϵǾ� �ִ� ��������
                int collFound   =   Convert.ToInt32(ds.Tables[0].Rows[1]["cnt"].ToString());    // ���ϵ� ��������
                int rc          = 0;
                ds.Dispose();
			
                //
                if( userFound == 0 )
                {
                    // �ش簡������ ��ϵ� ���� �����ϴ�.
                    targetcollectionModel.ResultCD   = "3101";
                    targetcollectionModel.ResultDesc = "����DB�� �������� �ʴ� �����Դϴ�.";
                    return;
                }

                if( collFound > 0 )
                {
                    targetcollectionModel.ResultCD   = "3100";
                    targetcollectionModel.ResultDesc = "��ϵǾ� �ִ� ���������� SKIP�մϴ�.";
                    return;
                }
                sbQuery = new StringBuilder();
                sbQuery.Append(" insert into ClientList ( CollectionCode, UserId, ServiceNum, RegDt ) " + "\n");
                sbQuery.Append(" select   " + iCollectionCode + "\n");
                sbQuery.Append("         ,UserId " + "\n");
                sbQuery.Append("         ,ServiceNum " + "\n");
                sbQuery.Append("         ,GetDate() " + "\n");
                sbQuery.Append(" from	StbUser noLock " + "\n");
                sbQuery.Append(" where	ServiceNum = '" + iServiceNum + "'"    + "\n");
				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// ��������
				try
				{
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
				}
				catch(Exception ex)
				{
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // ����
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3109";
				targetcollectionModel.ResultDesc = "������Ʈ���� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}
		}


		public void SetTargetCollectionDelete(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			int ClientListCount = 0;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQueryClientListCount = new StringBuilder();
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[1];

				sbQueryClientListCount.Append( "\n"
					+ "        SELECT COUNT(*) FROM    ClientList			    \n"
					+ "              WHERE CollectionCode  = @CollectionCode          	\n"
					);  

				sbQuery.Append(""
					+ "DELETE Collection         \n"
					+ " WHERE CollectionCode  = @CollectionCode  \n"
					);

				sqlParams[i++] = new SqlParameter("@CollectionCode"       , SqlDbType.Int);

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);

				// ��������
				try
				{
					//��ü���౤���� ���� Count����///////////////////////////////////////////////
					// __DEBUG__
					_log.Debug(sbQueryClientListCount.ToString());
					// __DEBUG__

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQueryClientListCount.ToString(),sqlParams);
                    
					ClientListCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					_log.Debug("ClientListCount          -->" + ClientListCount);

					// �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ� Exception�� �߻���Ų��.
					if(ClientListCount > 0) throw new Exception();


					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ÿ�ٱ���������:[" + targetcollectionModel.CollectionCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // ����
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3301";
				// �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ�
				if(ClientListCount > 0 )
				{
					targetcollectionModel.ResultDesc = "��ϵ� ��ü����簡 �����Ƿ� �̵������� �����Ҽ� �����ϴ�.";
				}
				else
				{
					targetcollectionModel.ResultDesc = "�̵����� ������ ������ �߻��Ͽ����ϴ�";
				}
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		public void SetClientDelete(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientDelete() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("CollectionCode  :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("UserId          :[" + targetcollectionModel.UserId       + "]");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];

				sbQuery.Append(""
					+ "DELETE ClientList         \n"
					+ " WHERE CollectionCode  = @CollectionCode  \n"
					+ " AND UserId  = @UserId  \n"
					);

				sqlParams[i++] = new SqlParameter("@CollectionCode"     , SqlDbType.Int);				
				sqlParams[i++] = new SqlParameter("@UserId"     , SqlDbType.Int);				
				
				i = 0;								
				sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);
				sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.UserId);
				// ��������

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				try
				{
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("ī�װ���������:[" + targetcollectionModel.CollectionCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // ����
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3301";
				targetcollectionModel.ResultDesc = "������ ������ ������ �߻��Ͽ����ϴ�";
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
