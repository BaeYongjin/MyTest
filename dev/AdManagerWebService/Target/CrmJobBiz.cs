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
		/// CRM작업 목록조회
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
				
				// 쿼리실행
				_db.Open();
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 미디어렙모델에 복사
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
				data.ResultDesc = "CRM작업리스트 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}

		/// <summary>
		/// 셋탑리스트를 읽어온다
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetStbList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStbList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn        \n"
					+ "		  ,ServiceNum as    UserId  \n"     
					+ "		  ,StbId					\n"
					+ "       ,PostNo					\n"
					+ "       ,ServiceCode				\n"
					+ "		  ,ResidentNo				\n"
					+ "		  ,Sex						\n"
					+ "       ,CASE Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N  \n"
					+ "		  ,RegDt					\n"					
					+ "  FROM StbUser with(noLock)      \n"				
					+ " WHERE  1 = 1					\n"
					);
				
				// 검색어가 있으면
				if (targetcollectionModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
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
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				targetcollectionModel.StbListDataSet = ds.Copy();
				// 결과
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.StbListDataSet);
				// 결과코드 셋트
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStbList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "Stb 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		/// <summary>
		/// 고객리스트
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetClientList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn            \n"
					+ "       ,A.CollectionCode			\n"     
					+ "		  ,A.UserId					\n"	
					+ "		  ,B.StbId					\n"	
					+ "		  ,B.PostNo					\n"	
					+ "		  ,B.ServiceCode			\n"	
					+ "		  ,B.ResidentNo				\n"	
					+ "		  ,B.Sex					\n"	
					+ "       ,CASE B.Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N  \n"
					+ "		  ,A.RegDt					\n"										
					+ "  FROM ClientList A LEFT JOIN StbUser B with(NoLock) ON (A.UserId          = B.UserId)        \n"					
					+ " WHERE  1 = 1					\n"
					);
				
				// 검색어가 있으면
//				if (targetcollectionModel.SearchKey.Trim().Length > 0)
//				{
//					// 여러컬럼에 대하여 LIKE 검색을 한다.
//					sbQuery.Append("\n"
//						+ "  AND ( StbId		  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR PostNo	      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR ResidentNo     LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR Sex		      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ " 	)       \n"
//						);
//				}

				// 매체대행사광고주레벨을 선택했으면
				if(targetcollectionModel.CollectionCode.Trim().Length > 0 && !targetcollectionModel.CollectionCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "' \n");
				}		
       
				sbQuery.Append("  ORDER BY A.CollectionCode, A.UserId");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				targetcollectionModel.ClientListDataSet = ds.Copy();
				// 결과
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);
				// 결과코드 셋트
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "고객 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		/// <summary>
		/// 고객리스트-페이징
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetClientPageList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientPageList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				
				// 쿼리생성
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
					+ "       ,CASE B.Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N  \n"
					+ "		  ,A.RegDt					\n"										
					+ "		  ,C.SummaryName DongName					\n"										
					+ "  FROM ClientList A LEFT JOIN StbUser B with(NoLock) ON (A.UserId          = B.UserId)        \n"					
					+ "					   LEFT JOIN SummaryCode C with(NoLock) ON ((select dbo.ufnGetTargetRegionCode2(substring(B.PostNo,0,4))) = C.SummaryCode AND C.SummaryType=5)        \n"					
					+ " WHERE  1 = 1					\n"
					);
				
				// 검색어가 있으면
				//				if (targetcollectionModel.SearchKey.Trim().Length > 0)
				//				{
				//					// 여러컬럼에 대하여 LIKE 검색을 한다.
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
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				targetcollectionModel.ClientListDataSet = ds.Copy();
				// 결과
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);
				// 결과코드 셋트
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientPageList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "고객 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		/// <summary>
		/// 타겟군정보 저장
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

				// 데이터베이스를 OPEN한다
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
				sqlParams[i++].Value = header.UserID;      // 등록자
				sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("타겟군정보수정:["+targetcollectionModel.CollectionCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3201";
				targetcollectionModel.ResultDesc = "타겟군정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			// 데이터베이스를  Close한다
			_db.Close();

		}

		/// <summary>
		/// 타겟군 생성
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

				// 데이터베이스를 OPEN한다
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
				sqlParams[i++].Value = header.UserID;				// 등록자

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("타겟군정보생성:[" + targetcollectionModel.CollectionCode + "(" + targetcollectionModel.CollectionName + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3101";
				targetcollectionModel.ResultDesc = "타겟군정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		/// <summary>
		/// 고객리스트 생성
		/// </summary>
		public void SetClientCreate(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

                int     iCollectionCode =   Convert.ToInt32(targetcollectionModel.CollectionCode);
                string  iServiceNum     =   targetcollectionModel.UserId.ToString();

				StringBuilder sbQuery = new StringBuilder();
                #region [ 존재여부 확인및 등록여부 확인 ]
                // 해당가번이 사용자정보에 존재하는지 검색
                sbQuery.Append(" select	1 as gubun ,count(*) as cnt "           + "\n");
                sbQuery.Append(" from	StbUser with(noLock) "                  + "\n");
                sbQuery.Append(" where  ServiceNum = '" + iServiceNum + "'"     + "\n");
                sbQuery.Append(" union all " + "\n");
                // 기등록된 데이터인지 검색
                sbQuery.Append(" select  2 as gubun, count(*) as cnt " + "\n");
                sbQuery.Append(" from   ClientList with(noLock) " + "\n");
                sbQuery.Append(" where	CollectionCode	= " +  iCollectionCode  + "\n");
                sbQuery.Append(" and    ServiceNum  = '" + iServiceNum + "'"    + "\n");
                sbQuery.Append(" order by gubun " + "\n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                if(ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception();
                }
                #endregion

                int userFound   =   Convert.ToInt32(ds.Tables[0].Rows[0]["cnt"].ToString());    // 광고고객정보에 등록되어 있는 가번인지
                int collFound   =   Convert.ToInt32(ds.Tables[0].Rows[1]["cnt"].ToString());    // 기등록된 가번인지
                int rc          = 0;
                ds.Dispose();
			
                //
                if( userFound == 0 )
                {
                    // 해당가번으로 등록된 고객이 없습니다.
                    targetcollectionModel.ResultCD   = "3101";
                    targetcollectionModel.ResultDesc = "광고DB에 존재하지 않는 가번입니다.";
                    return;
                }

                if( collFound > 0 )
                {
                    targetcollectionModel.ResultCD   = "3100";
                    targetcollectionModel.ResultDesc = "등록되어 있는 정보임으로 SKIP합니다.";
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

				// 쿼리실행
				try
				{
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
				}
				catch(Exception ex)
				{
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // 정상
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3109";
				targetcollectionModel.ResultDesc = "고객리스트정보 생성 중 오류가 발생하였습니다";
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

				// 데이터베이스를 OPEN한다
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

				// 쿼리실행
				try
				{
					//매체대행광고주 관계 Count조사///////////////////////////////////////////////
					// __DEBUG__
					_log.Debug(sbQueryClientListCount.ToString());
					// __DEBUG__

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQueryClientListCount.ToString(),sqlParams);
                    
					ClientListCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					_log.Debug("ClientListCount          -->" + ClientListCount);

					// 이미 다른테이블에 사용중인 데이터가 있다면 Exception를 발생시킨다.
					if(ClientListCount > 0) throw new Exception();


					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("타겟군정보삭제:[" + targetcollectionModel.CollectionCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3301";
				// 이미 다른테이블에 사용중인 데이터가 있다면
				if(ClientListCount > 0 )
				{
					targetcollectionModel.ResultDesc = "등록된 매체대행사가 있으므로 미디어렙정보를 삭제할수 없습니다.";
				}
				else
				{
					targetcollectionModel.ResultDesc = "미디어렙정보 삭제중 오류가 발생하였습니다";
				}
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
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
				_log.Debug("<입력정보>");
				_log.Debug("CollectionCode  :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("UserId          :[" + targetcollectionModel.UserId       + "]");

				// 데이터베이스를 OPEN한다
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
				// 쿼리실행

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				try
				{
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("카테고리정보삭제:[" + targetcollectionModel.CollectionCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3301";
				targetcollectionModel.ResultDesc = "고객정보 삭제중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}
	}
}
