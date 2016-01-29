/*
 * -------------------------------------------------------
 * 클래스명  :  CollectionHomeBiz
 * 주요기능  :  홈광고 타겟고객군 관리
 * 작성자    :  HJ
 * 작성일    :  2015.05.12
 * 특이사항  :  [OAP고도화] 홈광고전용 타겟고객군을 관리
 * -------------------------------------------------------
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;
using System.Threading;

using AdManagerModel;

namespace AdManagerWebService.Target
{
    public class CollectionHomeBiz : BaseBiz
	{
        public CollectionHomeBiz()
            : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

        public void GetCollectionHomeCombo(HeaderModel header, CollectionHomeModel collectionhomeModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCollectionHomeCombo() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchLevel      :[" + collectionhomeModel.SearchLevel + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                // 타겟 고객군 정보 가져오기 //
                // 고객군코드, 고객군명, 커멘트, 사용유무
                // 등록일, 수정일
                // 고객군별 사용자 수
                // 고객군별 타겟팅된 광고 수
                sbQuery.Append("\n SELECT CollectionCode, CollectionName        ");
                sbQuery.Append("\n FROM	CollectionHome			with(noLock)    ");
                sbQuery.Append("\n WHERE 1 = 1  ");
                sbQuery.Append("\n AND   UseYn = 'Y'    ");
                sbQuery.Append("\n AND   CONVERT(varchar(30), GETDATE(), 112) <= ExpiryDay    ");

                if (collectionhomeModel.SearchLevel.Equals("ADD"))
                {
                    sbQuery.Append("\n AND   CollectionCode not in (SELECT CollectionCode B     ");
                    sbQuery.Append("\n 							    FROM SchHomeList B with(nolock)    ");
                    sbQuery.Append("\n 							    WHERE B.UseYn = 'Y'     ");
                    sbQuery.Append("\n 							    AND  CONVERT(varchar(30), GETDATE(), 112) < B.ExpiryDay)       ");
                }
                else
                {
                    sbQuery.Append("\n UNION                ");
                    sbQuery.Append("\n SELECT A.CollectionCode               ");
                    sbQuery.Append("\n 		 ,B.CollectIonName       ");
                    sbQuery.Append("\n FROM SchHomeList A WITH(NOLOCK)           ");
                    sbQuery.Append("\n LEFT JOIN CollectionHome B ON A.CollectionCode = B.CollectionCode           ");
                    sbQuery.Append("\n WHERE 1=1           ");
                    sbQuery.Append("\n AND   A.UseYn = 'Y'           ");
                    sbQuery.Append("\n AND	 A.CollectionCode != 0     ");
                }
                sbQuery.Append("\n ORDER BY CollectionCode desc ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 미디어렙모델에 복사
                collectionhomeModel.CollectionHomeDataSet = ds.Copy();
                // 결과
                collectionhomeModel.ResultCnt = Utility.GetDatasetCount(collectionhomeModel.CollectionHomeDataSet);
                // 결과코드 셋트
                collectionhomeModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + collectionhomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCollectionHomeCombo() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                collectionhomeModel.ResultCD = "3000";
                collectionhomeModel.ResultDesc = "콤보박스용 타겟고객군정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


		/// <summary>
		/// 타겟군목록조회
		/// </summary>
		/// <param name="collectionhomeModel"></param>
		public void GetCollectionHomeList(HeaderModel header, CollectionHomeModel collectionhomeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCollectionHomeList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + collectionhomeModel.SearchKey       + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
                // 타겟 고객군 정보 가져오기 //
                // 고객군코드, 고객군명, 커멘트, 사용유무
                // 등록일, 수정일
                // 고객군별 사용자 수
                // 고객군별 타겟팅된 광고 정보
				sbQuery.Append("\n SELECT A.CollectionCode, A.CollectionName, A.Comment ");
				sbQuery.Append("\n        ,A.UseYn ");
				sbQuery.Append("\n        ,CASE A.UseYn WHEN 'Y' THEN '사용' WHEN 'N' THEN '사용안함' END AS UseYn_N ");
				sbQuery.Append("\n        ,convert(char(19), A.RegDt, 120) RegDt ");
				sbQuery.Append("\n        ,convert(char(19), A.ModDt, 120) ModDt ");
				sbQuery.Append("\n        ,B.UserName RegName ");
                sbQuery.Append("\n        ,( Select count(*) from ClientListHome with(noLock) where CollectionCode = A.CollectionCode AND CollectionCode = A.CollectionCode) Cnt ");
                sbQuery.Append("\n        ,( Select TOP 1 ListName from SchHomeList with(noLock) where CollectionCode = A.CollectionCode AND UseYn = 'Y' ORDER BY ModDt Desc ) UseCnt ");
                sbQuery.Append("\n        ,A.StartDay ");
                sbQuery.Append("\n        ,A.ExpiryDay ");
                sbQuery.Append("\n        ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay     ");
				sbQuery.Append("\n FROM	CollectionHome			A with(noLock) ");
				sbQuery.Append("\n LEFT JOIN SystemUser	B with(NoLock) ON (A.RegId          = B.UserId) ");
				sbQuery.Append("\n WHERE 1 = 1  ");			
				// 검색어가 있으면
				if (collectionhomeModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
						+ "  A.CollectionName      LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"	
						+ " OR A.Comment    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
						+ " OR B.UserName    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}
				
				if(collectionhomeModel.SearchchkAdState_10.Trim().Length > 0 && collectionhomeModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND A.UseYn = 'Y' OR A.UseYn = 'N' \n");
				}	
				if(collectionhomeModel.SearchchkAdState_10.Trim().Length > 0 && collectionhomeModel.SearchchkAdState_10.Trim().Equals("N"))
				{
					sbQuery.Append(" AND  A.UseYn  = 'Y' \n");					
				}	

				sbQuery.Append(" ORDER BY A.CollectionCode desc \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 미디어렙모델에 복사
				collectionhomeModel.CollectionHomeDataSet = ds.Copy();
				// 결과
				collectionhomeModel.ResultCnt = Utility.GetDatasetCount(collectionhomeModel.CollectionHomeDataSet);
				// 결과코드 셋트
				collectionhomeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + collectionhomeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCollectionHomeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				collectionhomeModel.ResultCD = "3000";
				collectionhomeModel.ResultDesc = "타겟고객군정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 셋탑리스트를 읽어온다
		/// </summary>
		/// <param name="collectionhomeModel"></param>
		public void GetStbList(HeaderModel header, CollectionHomeModel collectionhomeModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStbList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + collectionhomeModel.SearchKey       + "]");
				_log.Debug("JobCode      :[" + collectionhomeModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
                    + "Select Top 1000                  \n"
					+ "       'False' AS CheckYn        \n"
					+ "		  ,ServiceNum as    UserId  \n"     
					+ "		  ,StbId					\n"
					+ "       ,PostNo					\n"
					+ "       ,ServiceCode				\n"
					+ "		  ,ResidentNo				\n"
					+ "		  ,Sex						\n"
					+ "       ,CASE Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N  \n"
					+ "		  ,RegDt					\n"					
					+ "  FROM StbUser with(noLock)      \n"
                    + " WHERE  ServiceNum != ''			\n"
					//+ " WHERE  1 = 1					\n"
					);
				
				// 검색어가 있으면
                if (collectionhomeModel.SearchKey.Trim().Length > 0)
				{
                    //검색조건 추가
                    switch (collectionhomeModel.SearchLevel)
                    {
                        case "ALL": // 여러컬럼에 대하여 LIKE 검색을 한다.
                            sbQuery.Append("\n"
                                + "  AND ( StbId		LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                + "     OR PostNo	    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                + "     OR ServiceNum   LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                + " 	)       \n"
                                );
                            break;
                        case "CLIENT":
                            sbQuery.Append("\n"
                                + "  AND  ServiceNum   LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"

                                );
                            break;
                        case "STB":
                            sbQuery.Append("\n"
                                + "  AND  StbId		LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                );
                            break;
                        case "POST":
                            sbQuery.Append("\n"
                                + "  AND PostNo	    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                );
                            break;
                    }

					

				}
       
				sbQuery.Append("  ORDER BY ServiceNum, PostNo, ResidentNo, Sex");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				collectionhomeModel.StbListDataSet = ds.Copy();
				// 결과
				collectionhomeModel.ResultCnt = Utility.GetDatasetCount(collectionhomeModel.StbListDataSet);
				// 결과코드 셋트
				collectionhomeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + collectionhomeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStbList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				collectionhomeModel.ResultCD = "3000";
				collectionhomeModel.ResultDesc = "Stb 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

        /// <summary>
        /// 셋탑군에서 특정셋탑을 조회한다. //사용중지
        /// </summary>
        /// <param name="header"></param>
        /// <param name="collectionhomeModel"></param>
        public void GetStbListColl(HeaderModel header, CollectionHomeModel collectionhomeModel)
        {
            try
            {  // 데이터베이스를 OPEN한다
                _db.Open(); 

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbListColl() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + collectionhomeModel.SearchKey       + "]");
                _log.Debug("JobCode      :[" + collectionhomeModel.UserId       + "]");
				               
                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n Select Top 1000          "); // 1000 개씩 보이기
                sbQuery.Append("\n       'False' AS CheckYn ");
                sbQuery.Append("\n        ,A.CollectionCode ");
                sbQuery.Append("\n 		  ,A.UserId ");
                sbQuery.Append("\n 		  ,B.StbId ");
                sbQuery.Append("\n 		  ,B.PostNo ");
                sbQuery.Append("\n 		  ,B.ServiceCode ");
                sbQuery.Append("\n 		  ,B.ResidentNo ");
                sbQuery.Append("\n 		  ,B.Sex ");
                sbQuery.Append("\n        ,CASE B.Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N ");
                sbQuery.Append("\n 		  ,A.RegDt ");
                sbQuery.Append("\n from	ClientListHome      a with(nolock) ");
                sbQuery.Append("\n inner join StbUser   b with(nolock) on b.UserId = a.UserId");
                sbQuery.Append("\n where    a.CollectionCode = " + collectionhomeModel.CollectionCode );
				
                // 검색어가 있으면
                if (collectionhomeModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "     AND (   b.StbId		LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.PostNo	    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.ServiceNum  LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                        + " 	    )  \n"
                        );
                }
       
                sbQuery.Append("  ORDER BY b.ServiceNum, b.PostNo, b.ResidentNo, b.Sex");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 광고팩키지모델에 복사
                collectionhomeModel.StbListDataSet = ds.Copy();                
                // 결과
                collectionhomeModel.ResultCnt = Utility.GetDatasetCount(collectionhomeModel.StbListDataSet);
                              
                // 결과코드 셋트
                collectionhomeModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + collectionhomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbListColl() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                collectionhomeModel.ResultCD = "3000";
                collectionhomeModel.ResultDesc = "Stb 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }


        /// <summary>
        /// 셋탑 리스트 페이징 처리.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="collectionhomeModel"></param>
        public void GetStbPageList(HeaderModel header, CollectionHomeModel collectionhomeModel)
        {
            try
            {  // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbPageList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + collectionhomeModel.SearchKey + "]");
                _log.Debug("JobCode      :[" + collectionhomeModel.UserId + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
          
                // 쿼리생성                    
                sbQuery.Append(" Select Top (" + collectionhomeModel.PageSize.Trim() + ") \n");
                sbQuery.Append("     'False' AS CheckYn         \n");
                sbQuery.Append("	 ,ServiceNum as    UserId   \n");
                sbQuery.Append("	 ,StbId					    \n");
                sbQuery.Append("     ,PostNo					\n");
                sbQuery.Append("     ,ServiceCode				\n");
                sbQuery.Append("	 ,ResidentNo				\n");
                sbQuery.Append("	 ,Sex						\n");
                sbQuery.Append("     ,CASE Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N  \n");
                sbQuery.Append("	 ,RegDt					    \n");
                sbQuery.Append(" FROM StbUser with(noLock)      \n");
                sbQuery.Append(" WHERE  ServiceNum != ''		\n");
                //sbQuery.Append(" WHERE  1 = 1					\n");

                    
                if (collectionhomeModel.SearchKey.Trim().Length > 0)
                {
                    //검색조건 추가
                    switch (collectionhomeModel.SearchLevel)
                    {
                        case "ALL": // 여러컬럼에 대하여 LIKE 검색을 한다.
                            sbQuery.Append("\n"
                                + "  AND ( StbId		LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                + "     OR PostNo	    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                + "     OR ServiceNum   LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                + " 	)       \n"
                                );
                            break;
                        case "CLIENT":
                            sbQuery.Append("\n"
                                + "  AND  ServiceNum   LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"

                                );
                            break;
                        case "STB":
                            sbQuery.Append("\n"
                                + "  AND  StbId		LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                );
                            break;
                        case "POST":
                            sbQuery.Append("\n"
                                + "  AND PostNo	    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                );
                            break;
                    }
                }
    
                sbQuery.Append(" And UserId Not IN (Select Top((" + collectionhomeModel.Page.Trim() + "-1)*" + collectionhomeModel.PageSize.Trim() + ")UserId \n");
                sbQuery.Append("                    From StbUser \n");
                sbQuery.Append("                    Where ServiceNum != ''    \n");
                //sbQuery.Append("                    Where 1=1    \n");
                if (collectionhomeModel.SearchKey.Trim().Length > 0) // 검색어가 있으면
                {
                    //검색조건 추가
                    switch (collectionhomeModel.SearchLevel)
                    {
                        case "ALL": // 여러컬럼에 대하여 LIKE 검색을 한다.
                            sbQuery.Append("\n"
                                + "  AND ( StbId		LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                + "     OR PostNo	    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                + "     OR ServiceNum   LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                + " 	)       \n"
                                );
                            break;
                        case "CLIENT":
                            sbQuery.Append("\n"
                                + "  AND  ServiceNum   LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"

                                );
                            break;
                        case "STB":
                            sbQuery.Append("\n"
                                + "  AND  StbId		LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                );
                            break;
                        case "POST":
                            sbQuery.Append("\n"
                                + "  AND PostNo	    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                                );
                            break;
                    }
                }                   
                sbQuery.Append("  ) ORDER BY ServiceNum, PostNo, ResidentNo, Sex");
                
                
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 광고팩키지모델에 복사
                collectionhomeModel.StbListDataSet = ds.Copy();
                // 결과
                collectionhomeModel.ResultCnt = Utility.GetDatasetCount(collectionhomeModel.StbListDataSet);
                // 결과코드 셋트
                collectionhomeModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + collectionhomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                collectionhomeModel.ResultCD = "3000";
                collectionhomeModel.ResultDesc = "Stb 조회중 오류가 발생하였습니다";
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
		/// <param name="collectionhomeModel"></param>
		public void GetClientList(HeaderModel header, CollectionHomeModel collectionhomeModel)
        {
 
            try
            {  // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + collectionhomeModel.SearchKey + "]");
                _log.Debug("JobCode      :[" + collectionhomeModel.UserId + "]");

                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n Select Top 1000          "); // 1000 개씩 보이기
                sbQuery.Append("\n       'False' AS CheckYn ");
                sbQuery.Append("\n        ,A.CollectionCode ");
                sbQuery.Append("\n 		  ,A.UserId ");
                sbQuery.Append("\n 		  ,B.StbId ");
                sbQuery.Append("\n 		  ,B.PostNo ");
                sbQuery.Append("\n 		  ,B.ServiceCode ");
                sbQuery.Append("\n 		  ,B.ResidentNo ");
                sbQuery.Append("\n 		  ,B.Sex ");
                sbQuery.Append("\n        ,CASE B.Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N ");
                sbQuery.Append("\n 		  ,A.RegDt ");
                sbQuery.Append("\n from	ClientListHome      a with(nolock) ");
                sbQuery.Append("\n inner join StbUser   b with(nolock) on b.UserId = a.UserId");
                sbQuery.Append("\n where    a.CollectionCode = " + collectionhomeModel.CollectionCode);

                // 검색어가 있으면
                if (collectionhomeModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "     AND (   b.StbId		LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.PostNo	    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.ServiceNum  LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
                        + " 	    )  \n"
                        );
                }

                sbQuery.Append("  ORDER BY b.ServiceNum, b.PostNo, b.ResidentNo, b.Sex");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 광고팩키지모델에 복사
                collectionhomeModel.ClientListDataSet = ds.Copy();
                // 결과
                collectionhomeModel.ResultCnt = Utility.GetDatasetCount(collectionhomeModel.ClientListDataSet);

                // 결과코드 셋트
                collectionhomeModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + collectionhomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                collectionhomeModel.ResultCD = "3000";
                collectionhomeModel.ResultDesc = "고객 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

		/// <summary>
		/// 고객리스트-페이징
		/// </summary>
		/// <param name="collectionhomeModel"></param>
		public void GetClientPageList(HeaderModel header, CollectionHomeModel collectionhomeModel)
        {
            #region 기존 방식..
            /*
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientPageList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + collectionhomeModel.CollectionCode       + "]");
				_log.Debug("JobCode      :[" + collectionhomeModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT top (" + collectionhomeModel.PageSize.Trim() + ")            \n"
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
					+ "  FROM ClientList A with(noLock) LEFT JOIN StbUser B with(NoLock) ON (A.UserId          = B.UserId)        \n"					
					+ "					   LEFT JOIN SummaryCode C with(NoLock) ON ((select dbo.ufnGetTargetRegionCode2(substring(B.PostNo,0,4))) = C.SummaryCode AND C.SummaryType=5)        \n"					
					+ " WHERE  1 = 1					\n"
					);
				
				// 검색어가 있으면
				//				if (collectionhomeModel.SearchKey.Trim().Length > 0)
				//				{
				//					// 여러컬럼에 대하여 LIKE 검색을 한다.
				//					sbQuery.Append("\n"
				//						+ "  AND ( StbId		  LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR PostNo	      LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR ResidentNo     LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR Sex		      LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n"
				//						+ " 	)       \n"
				//						);
				//				}

				sbQuery.Append(" AND A.UserId not in(Select top ((" + collectionhomeModel.Page.Trim() + "-1)*" + collectionhomeModel.PageSize.Trim() + ")UserId \n");
				sbQuery.Append("					 From 	ClientList  \n");                 
			    sbQuery.Append("                     Where  CollectionCode = '" + collectionhomeModel.CollectionCode.Trim() + "') \n");
				
				if(collectionhomeModel.CollectionCode.Trim().Length > 0 && !collectionhomeModel.CollectionCode.Trim().Equals("00"))			
					sbQuery.Append(" AND A.CollectionCode = '" + collectionhomeModel.CollectionCode.Trim() + "' \n");       
				sbQuery.Append("  ORDER BY A.CollectionCode, A.UserId");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				collectionhomeModel.ClientListDataSet = ds.Copy();
				// 결과
				collectionhomeModel.ResultCnt = Utility.GetDatasetCount(collectionhomeModel.ClientListDataSet);
				// 결과코드 셋트
				collectionhomeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + collectionhomeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientPageList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				collectionhomeModel.ResultCD = "3000";
				collectionhomeModel.ResultDesc = "고객 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
            */
            #endregion
            try
            {  // 데이터베이스를 OPEN한다
                _db.ConnectionString = FrameSystem.connDbString;

                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientPageList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + collectionhomeModel.SearchKey + "]");
                _log.Debug("JobCode      :[" + collectionhomeModel.UserId + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                               
                // 특정 고객군
                sbQuery.Append("\n Select Top (" + collectionhomeModel.PageSize + ") ");
                sbQuery.Append("\n        'False' AS CheckYn ");
                sbQuery.Append("\n        ,A.CollectionCode ");
                sbQuery.Append("\n 		  ,A.UserId ");
                sbQuery.Append("\n 		  ,B.StbId ");
                sbQuery.Append("\n 		  ,B.PostNo ");
                sbQuery.Append("\n 		  ,B.ServiceCode ");
                sbQuery.Append("\n 		  ,B.ResidentNo ");
                sbQuery.Append("\n 		  ,B.Sex ");
                sbQuery.Append("\n        ,CASE B.Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N ");
                sbQuery.Append("\n 		  ,A.RegDt ");
                sbQuery.Append("\n        ,C.SummaryName DongName       ");
                sbQuery.Append("\n From	AdTargetsHanaTV.dbo.ClientListHome      a with(nolock)  ");
                sbQuery.Append("\n INNER join AdTargetsHanaTV.dbo.StbUser   b with(nolock) on b.UserId = a.UserId");
                sbQuery.Append("\n LEFT  JOIN SummaryCode C with(NoLock) ON ((select AdTargetsHanaTV.dbo.ufnGetTargetRegionCode2(substring(B.PostNo,0,4))) = C.SummaryCode AND C.SummaryType=5)        \n");
                sbQuery.Append("\n where    a.CollectionCode = " + collectionhomeModel.CollectionCode);
                // 검색어가 있으면
                if (collectionhomeModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.                        
                    sbQuery.Append("     AND (   b.StbId		LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.PostNo	    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.ServiceNum  LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append(" 	    )  \n");
                }

                //sbQuery.Append("\n AND A.UserId not in(select top ((" + collectionhomeModel.Page.Trim() + "-1)*" + collectionhomeModel.PageSize.Trim() + ")UserId \n");
                //sbQuery.Append("\n  				   from 	ClientList  \n");
                //sbQuery.Append("\n                     Where CollectionCode = '" + collectionhomeModel.CollectionCode.Trim() + "' \n");

                sbQuery.Append("\n AND A.UserId > ( select isnull( max(userId), 0)  \n");
                sbQuery.Append("\n                  from (  select top ((" + collectionhomeModel.Page.Trim() + "-1)*" + collectionhomeModel.PageSize.Trim() + ")UserId \n");
                sbQuery.Append("\n  				        from    AdTargetsHanaTV.dbo.ClientListHome nolock  \n");
                sbQuery.Append("\n                          Where   CollectionCode = '" + collectionhomeModel.CollectionCode.Trim() + "' \n");
                // 검색어가 있으면
                if (collectionhomeModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("     AND (   b.StbId		LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.PostNo	    LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.ServiceNum  LIKE '%" + collectionhomeModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append(" 	    )  \n");
                }
                sbQuery.Append("        order by userId  ) v ) ");
                sbQuery.Append(" ORDER BY a.UserId");
               

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 광고팩키지모델에 복사
                collectionhomeModel.ClientListDataSet = ds.Copy();
                // 결과
                collectionhomeModel.ResultCnt = Utility.GetDatasetCount(collectionhomeModel.ClientListDataSet);
                // 결과코드 셋트
                collectionhomeModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + collectionhomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientPageList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                collectionhomeModel.ResultCD = "3000";
                collectionhomeModel.ResultDesc = "고객군 조회중 오류가 발생하였습니다";
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
		public void SetCollectionHomeUpdate(HeaderModel header, CollectionHomeModel collectionhomeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCollectionHomeUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[7];

				sbQuery.Append(""
					+ " UPDATE  CollectionHome  \n"
					+ " SET	  CollectionName= @CollectionName	\n"										
					+ "      ,Comment		= @Comment  \n"			
					+ "      ,UseYn			= @UseYn    \n"	
					+ "      ,ModDt         = GETDATE() \n"
					+ "      ,RegID         = @RegID    \n"
                    + "      ,StartDay      = @StartDay \n"
                    + "      ,ExpiryDay      = @ExpiryDay \n"
					+ " WHERE CollectionCode        = @CollectionCode        \n"
					);

				sqlParams[0] = new SqlParameter("@CollectionName" , SqlDbType.VarChar	, 500);												
				sqlParams[1] = new SqlParameter("@Comment"		, SqlDbType.VarChar , 2000);		
				sqlParams[2] = new SqlParameter("@UseYn"			, SqlDbType.Char	, 1);
				sqlParams[3] = new SqlParameter("@RegID"			, SqlDbType.VarChar , 10);
				sqlParams[4] = new SqlParameter("@CollectionCode" , SqlDbType.Int);
                sqlParams[5] = new SqlParameter("@StartDay", SqlDbType.VarChar, 8);
                sqlParams[6] = new SqlParameter("@ExpiryDay", SqlDbType.VarChar, 8);

				sqlParams[0].Value = collectionhomeModel.CollectionName;								
				sqlParams[1].Value = collectionhomeModel.Comment;				
				sqlParams[2].Value = collectionhomeModel.UseYn;
				sqlParams[3].Value = header.UserID;      // 등록자
				sqlParams[4].Value = Convert.ToInt32(collectionhomeModel.CollectionCode);
                sqlParams[5].Value = collectionhomeModel.StartDay;
                sqlParams[6].Value = collectionhomeModel.ExpiryDay;

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("타겟군정보수정:["+collectionhomeModel.CollectionCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				collectionhomeModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCollectionHomeUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				collectionhomeModel.ResultCD   = "3201";
				collectionhomeModel.ResultDesc = "타겟군정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			// 데이터베이스를  Close한다
			_db.Close();

		}

		/// <summary>
		/// 타겟군 생성
		/// </summary>
		public void SetCollectionHomeCreate(HeaderModel header, CollectionHomeModel collectionhomeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCollectionHomeCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[5];

				sbQuery.Append( ""
					+ "INSERT INTO CollectionHome (	\n"
					+ "       CollectionCode    \n"
					+ "      ,CollectionName    \n"					
					+ "      ,Comment			\n"
					+ "		 ,UseYn				\n"															
					+ "      ,RegDt				\n"
					+ "      ,ModDt				\n"
					+ "      ,RegID             \n"
                    + "      ,StartDay          \n"
                    + "      ,ExpiryDay         \n"
					+ "      )                  \n"
					+ " SELECT                  \n"
					+ "       ISNULL(MAX(CollectionCode),0)+1        \n"
					+ "      ,@CollectionName   \n"					
					+ "      ,@Comment			\n"					
					+ "      ,'Y'				\n"					
					+ "      ,GETDATE()			\n"
					+ "      ,GETDATE()			\n"
					+ "      ,@RegID			\n"
                    + "      ,@StartDay			\n"
                    + "      ,@ExpiryDay		\n"
					+ " FROM CollectionHome               \n"
					);
				
				sqlParams[i++] = new SqlParameter("@CollectionName" , SqlDbType.VarChar , 40);				
				sqlParams[i++] = new SqlParameter("@Comment"		, SqlDbType.VarChar , 50);				
				sqlParams[i++] = new SqlParameter("@RegID"			, SqlDbType.VarChar , 10);
                sqlParams[i++] = new SqlParameter("@StartDay"       , SqlDbType.VarChar, 8);
                sqlParams[i++] = new SqlParameter("@ExpiryDay"      , SqlDbType.VarChar, 8);
				i = 0;				
				sqlParams[i++].Value = collectionhomeModel.CollectionName;				
				sqlParams[i++].Value = collectionhomeModel.Comment;				
				sqlParams[i++].Value = header.UserID;				// 등록자
                sqlParams[i++].Value = collectionhomeModel.StartDay;
                sqlParams[i++].Value = collectionhomeModel.ExpiryDay;

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("타겟군정보생성:[" + collectionhomeModel.CollectionCode + "(" + collectionhomeModel.CollectionName + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				collectionhomeModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCollectionHomeCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				collectionhomeModel.ResultCD   = "3101";
				collectionhomeModel.ResultDesc = "타겟군정보 생성 중 오류가 발생하였습니다";
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
		public void SetClientCreate(HeaderModel header, CollectionHomeModel collectionhomeModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

                int     iCollectionCode =   Convert.ToInt32(collectionhomeModel.CollectionCode);
                string  iServiceNum     =   collectionhomeModel.UserId.ToString();

				StringBuilder sbQuery = new StringBuilder();

                #region [ 존재여부 확인및 등록여부 확인 ]
                // 해당가번이 사용자정보에 존재하는지 검색
                // 0:가입중, 1:사용중, 2:해지,3:일시중단,4:계약휴지,5:서비스제공휴지,7:예약가입,9:서비스중단, 10:접수취소
                sbQuery.Append(" select  sum( case gubun when 1 then cnt else 0 end )	as	Stb " + "\n");
                sbQuery.Append("		,sum( case gubun when 2 then cnt else 0 end )	as	Clt " + "\n");
                sbQuery.Append(" from ( " + "\n");
                sbQuery.Append("		select	1 as gubun ,count(*) as cnt " + "\n");
                sbQuery.Append("		from	StbUser with(noLock) "                  + "\n");
                sbQuery.Append("		where	UserId = '" + iServiceNum + "'"     + "\n");
                sbQuery.Append("		and		ServiceStatusCode in(1) " + "\n");
                sbQuery.Append("		union all " + "\n");
                // 기등록된 데이터인지 검색
                sbQuery.Append("		select  2 as gubun, count(*) as cnt " + "\n");
                sbQuery.Append("		from    ClientListHome with(noLock) " + "\n");
                sbQuery.Append("		where	CollectionCode	= " +  iCollectionCode  + "\n");
                sbQuery.Append("		and     UserId      = '" + iServiceNum + "'" + "\n");
                 sbQuery.Append("	 ) v1 " + "\n");
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

                int userFound   =   Convert.ToInt32(ds.Tables[0].Rows[0]["Stb"].ToString());    // 광고고객정보에 등록되어 있는 가번인지
                int collFound   =   Convert.ToInt32(ds.Tables[0].Rows[0]["Clt"].ToString());    // 기등록된 가번인지
                int rc          = 0;
                ds.Dispose();
			
                //
                if( userFound == 0 )
                {
                    // 해당가번으로 등록된 고객이 없습니다.
                    collectionhomeModel.ResultCD   = "3101";
                    collectionhomeModel.ResultDesc = "광고DB에 존재하지 않는 가번입니다.";
                    return;
                }

                if( collFound > 0 )
                {
                    collectionhomeModel.ResultCD   = "3100";
                    collectionhomeModel.ResultDesc = "등록되어 있는 정보임으로 SKIP합니다.";
                    return;
                }
                sbQuery = new StringBuilder();
                sbQuery.Append(" insert into ClientListHome ( CollectionCode, UserId, StbId, RegDt ) " + "\n");
                sbQuery.Append(" select   " + iCollectionCode + "\n");
                sbQuery.Append("         ,UserId " + "\n");
                sbQuery.Append("         ,StbId " + "\n");
                sbQuery.Append("         ,GetDate() " + "\n");
                sbQuery.Append(" from	StbUser noLock " + "\n");
                sbQuery.Append(" where	UserId = '" + iServiceNum + "'" + "\n");
				sbQuery.Append(" and	ServiceStatusCode = 1"  + "\n");

				
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

				collectionhomeModel.ResultCD = "0000";  // 정상
			}
			catch(Exception ex)
			{
				collectionhomeModel.ResultCD   = "3109";
				collectionhomeModel.ResultDesc = "고객리스트정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}
		}

        /// <summary>
        /// [E_01] 특정 고객군에 등록할 사용자들 처리...
        /// OPENXML 이용하려 했으나 개별 처리에 대한 결과 분석이 난해해서 데이터 한 ROW씩 처리 하는 방식으로 해 둠.(트랜잭션 실행)
        /// </summary>      
        public void SetClientCollectionCreate(HeaderModel header, CollectionHomeModel collectionhomeModel)
        {
            try
            {
                int iCollectionCode = Convert.ToInt32(collectionhomeModel.CollectionCode);
                
                int x, rc = 0;
                int userFound = 0; // 광고고객정보에 등록되어 있는 가번인지
                int collFound = 0; // 기등록된 가번인지

                int totalCnt = 0; // 처리 할 전체 데이터 수
                int readCnt = 0; // 처리한 데이터 수
                int addCnt = 0;  // 고객군에 추가된 등록 수
                int nonCnt = 0;  // 셋탑등록이 안된 정보 수
                int skipCnt = 0; // skip 수
                int failCnt = 0; // 등록 실패 수

                // 데이터 검수
                if (collectionhomeModel.ClientListDataSet.Tables == null
                    || collectionhomeModel.ClientListDataSet.Tables["CollectionUsers"] == null
                    || collectionhomeModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count == 0)
                    throw new Exception("추가할 데이터가 없습니다!");

                StringBuilder sbQuery = new StringBuilder();
                
                // 데이터베이스를 OPEN한다
                _db.Open();

                #region 해당가번이 사용자정보에 존재하는지 검색 쿼리
                
                // 0:가입중, 1:사용중, 2:해지,3:일시중단,4:계약휴지,5:서비스제공휴지,7:예약가입,9:서비스중단, 10:접수취소
                sbQuery.Append(" select  sum( case gubun when 1 then cnt else 0 end )	as	Stb " + "\n");
                sbQuery.Append("		,sum( case gubun when 2 then cnt else 0 end )	as	Clt " + "\n");
                sbQuery.Append(" from (                                       \n");
                sbQuery.Append("		select	1 as gubun ,count(*) as cnt   \n");
                sbQuery.Append("		from	StbUser with(noLock)          \n");
                sbQuery.Append("		where	UserId = @iUserId     \n");
                sbQuery.Append("		and		ServiceStatusCode in(0,1,3)   \n"); // 0,1,3
                sbQuery.Append("		union all                             \n");
                // 기등록된 데이터인지 검색
                sbQuery.Append("		select  2 as gubun, count(*) as cnt        \n");
                sbQuery.Append("		from    ClientListHome with(noLock)            \n");
                sbQuery.Append("		where	CollectionCode	= @iCollectionCode \n");
                sbQuery.Append("		and     UserId      = @iUserId     \n");
                sbQuery.Append("	 ) v1       \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                #endregion

                #region 고객군에 insert 쿼리...
                StringBuilder sbQuery2 = new StringBuilder();
                sbQuery2.Append(" insert into ClientListHome ( CollectionCode, UserId, RegDt )    \n");
                sbQuery2.Append(" select   @iCollectionCode          \n");
                sbQuery2.Append("         ,UserId                    \n");
                sbQuery2.Append("         ,GetDate()                 \n");
                sbQuery2.Append(" from	StbUser noLock               \n");
                sbQuery2.Append(" where	UserId = @iUserId    \n");
                sbQuery2.Append(" and	ServiceStatusCode in (0,1,3) \n"); // 가입중,사용중, 일시중지(query1과 동일조건)


                // __DEBUG__
                _log.Debug(sbQuery2.ToString());
                // __DEBUG__
                #endregion

                totalCnt = collectionhomeModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count;

                SqlParameter[] sqlParams = new SqlParameter[2];
                x = 0;
                sqlParams[x++] = new SqlParameter("@iCollectionCode", SqlDbType.Int);
                sqlParams[x++] = new SqlParameter("@iServiceNum", SqlDbType.VarChar, 12);

                int groupCnt = 1;    // 그룹 인덱스
                int processCnt = 10; // 그룹 별 처리 할 데이터 수
                if (totalCnt < processCnt) processCnt = 1;

                for (int i = 0; i < totalCnt; i += processCnt) // 구간별 처리할 데이터수 만큼 증가
                {
                    _log.Debug("[SetClientCollectionCreate] 각 그룹별 처리 수:" + i.ToString());
                    for (int inx = i; inx < (processCnt * groupCnt); inx++) // 구간 내부 loop
                    {
                        if (inx < totalCnt)
                        {
                            readCnt++;
                            try
                            {
                                DataRow row = collectionhomeModel.ClientListDataSet.Tables["CollectionUsers"].Rows[inx]; // 너무 길어서....
                                x = 0;
                                sqlParams[x++].Value = Convert.ToInt32(collectionhomeModel.CollectionCode);
                                sqlParams[x++].Value = row["ServiceNum"].ToString();

                                #region [존재여부 확인및 등록여부 확인] DataReader 이용
                                using (SqlCommand cmd = new SqlCommand(sbQuery.ToString(), _db.SqlConn))
                                {
                                    cmd.Parameters.Add("@iCollectionCode", SqlDbType.Int, 4);
                                    cmd.Parameters.Add("@iServiceNum", SqlDbType.VarChar, 12);
                                    
                                    cmd.Parameters[0].Value = sqlParams[0].Value;
                                    cmd.Parameters[1].Value = sqlParams[1].Value;
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            userFound = Convert.ToInt32(reader[0]);
                                            collFound = Convert.ToInt32(reader[1]);
                                        }
                                    }
                                    reader.Close();
                                }
                                if (userFound == 0) // 가입중, 사용중, 일시정지 상태의 가입자가 없다면 등록 할 수 없음.
                                    throw new Exception("3101"); // 해당가번으로 등록된 고객이 없습니다."광고DB에 존재하지 않는 가번입니다.";

                                if (collFound > 0) // 고객군에 사용자 정보가 등록되어 있음                        
                                    throw new Exception("3100"); //"등록되어 있는 정보임으로 SKIP합니다.";
                                #endregion
                                     
                                #region 등록 실행
                                try
                                {
                                    _db.BeginTran();
                                    rc = _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams);
                                    _db.CommitTran();
                                }
                                catch (Exception ex)
                                {
                                    _log.Debug("등록 예외 발생:" + ex.Message);
                                    _db.RollbackTran();
                                    throw new Exception("3000");
                                }

                                addCnt++; // 정상적인 추가
                                #endregion                               
                            }
                            catch (Exception ex)
                            {
                                _log.Debug("[SetClientCollectionCreate] 예외:" + ex.Message);
                                // 예외 정보 파싱해서 결과문자열로 클라이언트에 전달
                                if (ex.Message.Equals("3101")) // 존재 하지 않는 정보
                                    nonCnt++;
                                else if (ex.Message.Equals("3100")) // skip 정보
                                    skipCnt++;
                                else if (ex.Message.Equals("3000")) // 등록 실패
                                {
                                    failCnt++;
                                    _db.RollbackTran();
                                }
                                else
                                {
                                    failCnt++;                                   
                                }
                            }
                        } // end if(inx < totalCnt)
                    }
                    ++groupCnt; // 다음 구간
                    // 한 구간이 끝나면 다음 구간 처리시 일시정지?       
                    //Thread.Sleep(5);
                    //_log.Debug("[Sleep] 100]");
                }                                

                collectionhomeModel.ResultCD = "0000";  // 정상
                collectionhomeModel.ResultDesc = string.Format("{0:0},{1:0},{2:0},{3:0},{4:0},{5:0}", readCnt, totalCnt, addCnt, failCnt , nonCnt, skipCnt);
            }
            catch (Exception ex)
            {
                collectionhomeModel.ResultCD = "3109";
                collectionhomeModel.ResultDesc = ex.Message; //"고객리스트정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
                
            }
        }

        /// <summary>
        /// [E_02] 특정고객군에 등록 할 사용자들 처리 openxml 이용한 프로시져 실행
        /// 1 천 건씩 받아서 내부에서 몇 백 건씩 등록 처리..
        /// </summary>                
        public void SetClientCollectionCreateProc(HeaderModel header, CollectionHomeModel collectionhomeModel)
        {
            try
            {
                int iCollectionCode = Convert.ToInt32(collectionhomeModel.CollectionCode);

                int x, rc = 0;              

                int totalCnt = 0; // 처리 할 전체 데이터 수
                int readCnt = 0; // 처리한 데이터 수
                int addCnt = 0;  // 고객군에 추가된 등록 수
                int nonCnt = 0;  // 셋탑등록이 안된 정보 수
                int skipCnt = 0; // skip 수
                int failCnt = 0; // 등록 실패 수

                // 데이터 검수
                if (collectionhomeModel.ClientListDataSet.Tables == null
                    || collectionhomeModel.ClientListDataSet.Tables["CollectionUsers"] == null
                    || collectionhomeModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count == 0)
                    throw new Exception("추가할 데이터가 없습니다!");

                StringBuilder sbQuery = new StringBuilder();

                // 데이터베이스를 OPEN한다
                _db.Open();
                
                totalCnt = collectionhomeModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count;

                SqlParameter[] sqlParams = new SqlParameter[2];
                x = 0;
                sqlParams[x++] = new SqlParameter("@CollectionCode", SqlDbType.Int);               
                sqlParams[x++] = new SqlParameter("@xmlDocument", SqlDbType.VarChar,-1);

                x = 0;
                sqlParams[x++].Direction = ParameterDirection.Input;              
                sqlParams[x++].Direction = ParameterDirection.Input;                
                

                int groupCnt = 1;    // 그룹 인덱스
                int processCnt = 400; // 그룹 별 처리 할 데이터 수
                int sendCount = 0;

                
                _log.Debug("[SetClientCollectionCreateProc] Start...");
                for (int i = 0; i < totalCnt; i += processCnt) // 구간별 처리할 데이터수 만큼 증가
                {
                    sbQuery.Clear();
                    sbQuery.Append("<ROOT>");
                   
                    for (int inx = i; inx < (processCnt * groupCnt); inx++) // 구간 내부 loop
                    {
                        if (inx < totalCnt)
                        {
                            readCnt++;                            
                            try
                            {
                                sendCount++;
                                DataRow row = collectionhomeModel.ClientListDataSet.Tables["CollectionUsers"].Rows[inx]; // 너무 길어서....
                                sbQuery.Append("<Cus Code=\"" + collectionhomeModel.CollectionCode + "\" SNum=\"" + row["ServiceNum"].ToString() + "\" />");
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("데이터 구성 예외:" + ex.Message);
                            }
                        } // end if(inx < totalCnt)
                    }
                    sbQuery.Append("</ROOT>");

                    DataSet ds = new DataSet();
                    #region 등록 실행
                    try
                    {
                        x = 0;
                        sqlParams[x++].Value = collectionhomeModel.CollectionCode;                       
                        sqlParams[x++].Value = sbQuery.ToString();

                        _db.BeginTran();                        
                        _db.ExecuteProcedure(ds, "[dbo].[CollectionHome_Create]", sqlParams);
                        _db.CommitTran();

                        if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                        {
                            //nonCnt += sendCount - Convert.ToInt32(ds.Tables[0].Rows[0][0]); // 회원 아닌 수(전송한 수 - 가번등록 수)
                            //skipCnt += Convert.ToInt32(ds.Tables[0].Rows[0][1]);           // 이미 등록된 회원 
                            addCnt += Convert.ToInt32(ds.Tables[0].Rows[0][2]);            // 실제 추가된 수
                        }
                        else
                            throw new Exception("3000");
                    }
                    catch (Exception ex)
                    {
                        _log.Debug("등록 예외 발생:" + ex.Message);
                        _db.RollbackTran();
                        throw new Exception("3000");
                    }
                    finally
                    {
                        sendCount = 0;
                        if (ds != null) ds = null;
                    }
                    #endregion                    
                    
                    ++groupCnt; // 다음 그룹  
                    Thread.Sleep(1000);
                }               
                collectionhomeModel.ResultCD = "0000";  // 정상
                collectionhomeModel.ResultDesc = string.Format("{0:0},{1:0},{2:0},{3:0},{4:0},{5:0}", readCnt, totalCnt, addCnt, failCnt, nonCnt, skipCnt);
            }
            catch (Exception ex)
            {
                collectionhomeModel.ResultCD = "3109";
                collectionhomeModel.ResultDesc = ex.Message; //"고객리스트정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
                _log.Debug("[SetClientCollectionCreateProc] End...");
            }
        }

		public void SetCollectionHomeDelete(HeaderModel header, CollectionHomeModel collectionhomeModel)
		{
			int ClientListCount = 0;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCollectionHomeDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQueryClientListCount = new StringBuilder();
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[1];

				sbQueryClientListCount.Append( "\n"
					+ "        SELECT COUNT(*) FROM    ClientListHome with(noLock)  \n"
					+ "              WHERE CollectionCode  = @CollectionCode          	\n"
					);  

				sbQuery.Append(""
					+ "DELETE CollectionHome         \n"
					+ " WHERE CollectionCode  = @CollectionCode  \n"
					);

				sqlParams[i++] = new SqlParameter("@CollectionCode"       , SqlDbType.Int);

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(collectionhomeModel.CollectionCode);

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
					_log.Message("타겟군정보삭제:[" + collectionhomeModel.CollectionCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				collectionhomeModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCollectionHomeDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				collectionhomeModel.ResultCD   = "3301";
				// 이미 다른테이블에 사용중인 데이터가 있다면
				if(ClientListCount > 0 )
				{
					collectionhomeModel.ResultDesc = "등록된 매체대행사가 있으므로 미디어렙정보를 삭제할수 없습니다.";
				}
				else
				{
					collectionhomeModel.ResultDesc = "미디어렙정보 삭제중 오류가 발생하였습니다";
				}
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

        /// <summary>
        /// 1개의 고객 데이터만 삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="collectionhomeModel"></param>
		public void SetClientDelete(HeaderModel header, CollectionHomeModel collectionhomeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientDelete() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("CollectionCode  :[" + collectionhomeModel.CollectionCode       + "]");
				_log.Debug("UserId          :[" + collectionhomeModel.UserId       + "]");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];

				sbQuery.Append(""
					+ "DELETE ClientListHome         \n"
					+ " WHERE CollectionCode  = @CollectionCode  \n"
					+ " AND UserId  = @UserId  \n"
					);

				sqlParams[i++] = new SqlParameter("@CollectionCode"     , SqlDbType.Int);				
				sqlParams[i++] = new SqlParameter("@UserId"     , SqlDbType.Int);				
				
				i = 0;								
				sqlParams[i++].Value = Convert.ToInt32(collectionhomeModel.CollectionCode);
				sqlParams[i++].Value = Convert.ToInt32(collectionhomeModel.UserId);
				// 쿼리실행

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				try
				{
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("카테고리정보삭제:[" + collectionhomeModel.CollectionCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					throw ex;
				}

				collectionhomeModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				collectionhomeModel.ResultCD   = "3301";
				collectionhomeModel.ResultDesc = "고객정보 삭제중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

        /// <summary>
        /// 전달 된 여러 고객들의 데이터 삭제[E_01]
        /// </summary>      
        public void SetClientCollectionDelete(HeaderModel header, CollectionHomeModel collectionhomeModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientCollectionDelete() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("CollectionCode  :[" + collectionhomeModel.CollectionCode + "]");
                _log.Debug("UserId          :[" + collectionhomeModel.UserId + "]");

                if (collectionhomeModel.ClientListDataSet.Tables.Count == 0
                   || collectionhomeModel.ClientListDataSet.Tables["Clients"] == null
                   || collectionhomeModel.ClientListDataSet.Tables["Clients"].Rows.Count == 0)
                    throw new Exception("처리할 데이터가 없습니다.!");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                int i = 0;
                int rc = 0;

                SqlParameter[] sqlParams = new SqlParameter[2];

                sbQuery.Append(""
                    + "DELETE ClientListHome         \n"
                    + " WHERE CollectionCode  = @CollectionCode  \n"
                    + " AND UserId  = @UserId  \n"
                    );

                _db.BeginTran();
                
                foreach (DataRow row in collectionhomeModel.ClientListDataSet.Tables[0].Rows)
                {
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@UserId", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(row["CollectionCode"]);
                    sqlParams[i++].Value = Convert.ToInt32(row["UserId"]);
                                                            
                   
                   rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                   _log.Message("고객군 사용자 정보 :[" + collectionhomeModel.CollectionCode + "] 등록자:[" + header.UserID + "]");                   
                }

                _db.CommitTran();

                collectionhomeModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientCollectionDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                collectionhomeModel.ResultCD = "3301";
                collectionhomeModel.ResultDesc = "고객정보 삭제중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


        /// <summary>
        /// 전체 고객 삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="collectionhomeModel"></param>
        public void SetAllClientDelete(HeaderModel header, CollectionHomeModel collectionhomeModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAllClientDelete() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("CollectionCode  :[" + collectionhomeModel.CollectionCode + "]");
                _log.Debug("UserId          :[" + collectionhomeModel.UserId + "]");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[1];

                _db.Timeout = 60 * 3000;

                //1000건씩 삭제
                sbQuery.Append(""
                    + "--WHILE 1> 0        \n"
                    + "BEGIN        \n"
                    + "    DELETE TOP (10000)        \n"
                    + "    FROM   ClientListHome         \n"
                    + "    WHERE  CollectionCode = " + collectionhomeModel.CollectionCode + "       \n"
                    + "    --PRINT @@ROWCOUNT        \n"
                    + "    --IF @@ROWCOUNT = 0 BREAK        \n"
                    + "    --WAITFOR DELAY '00:00:01' --1초딜레이후수행        \n"
                    + "END        \n"



                    );

                sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(collectionhomeModel.CollectionCode);
                // 쿼리실행

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
                try
                {
                    rc = _db.ExecuteNonQuery(sbQuery.ToString());
                    collectionhomeModel.ResultCnt = rc;
                    _log.Message("고객정보삭제:[" + collectionhomeModel.CollectionCode + "] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                collectionhomeModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAllClientDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                collectionhomeModel.ResultCD = "3301";
                collectionhomeModel.ResultDesc = "고객정보 삭제중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


        /// <summary> 
        /// 특정 고객군의 고객리스트를 대상 고객군의 고객리스토로 복사/이동
        /// </summary>
        /// <param name="header"></param>
        /// <param name="collectionhomeModel"></param>
        public void SetClientListCopyMove(HeaderModel header, CollectionHomeModel collectionhomeModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientListCopy() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("FromCode  :[" + collectionhomeModel.FromCode + "]");
                _log.Debug("ToCode    :[" + collectionhomeModel.ToCode + "]");
                _log.Debug("CopyMove  :[" + collectionhomeModel.CopyMove + "]");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = null;
                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = null;

                _db.BeginTran();
                try
                {
                    // 복사전에 TO이 고객리스트를 삭제

                    #region TO이 고객리스트를 삭제

                    sbQuery = new StringBuilder();
                    sqlParams = new SqlParameter[1];

                    sbQuery.Append("--고객리스트 이동용 삭제\n"
                        + "DELETE ClientListHome         \n"
                        + " WHERE CollectionCode  = @ToCode  \n"
                        );

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@ToCode", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(collectionhomeModel.ToCode);
                    // 쿼리실행

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    //사용여부가 N일경우만 기존데이터 삭제함
                    if(collectionhomeModel.UseYn.Equals("N"))
                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    #endregion

                    // 복사 From 에서 To 로
                    #region From 에서 To 로 복사
                    sbQuery = new StringBuilder();
                    sqlParams = new SqlParameter[2];

                    // 클러스터드 인덱스의 경우 인덱스 컬럼순으로 맞춰줘야한다.
                    // 맞추지 않을경우 중복키 삽입불가 에러메세지를 내보낸다.
                    sbQuery.Append("--고객리스트 복사\n"
                        + "INSERT INTO ClientListHome         \n"
                        + "     SELECT A.UserId     \n"
                        + "           ,A.CollectionCode \n"
                        + "           ,A.RegDt \n"
                        + "     FROM          \n"
                        + "    (SELECT @ToCode AS CollectionCode \n"
                        + "           ,UserId \n"
                        + "           ,RegDt \n"
                        + "       FROM ClientListHome with(nolock)  \n"
                        + "      WHERE CollectionCode = @FromCode \n"
                        + "      EXCEPT \n"                                 //EXCEPT를 이용하여 키중복 방지
                        + "     SELECT CollectionCode \n"
                        + "           ,UserId \n"
                        + "           ,RegDt \n"
                        + "       FROM ClientListHome with(nolock)  \n"
                        + "      WHERE CollectionCode = @ToCode) A \n"
                        );

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@FromCode", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@ToCode", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(collectionhomeModel.FromCode);
                    sqlParams[i++].Value = Convert.ToInt32(collectionhomeModel.ToCode);
                    // 쿼리실행
                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    #endregion

                    // 이동인지 검사
                    if(collectionhomeModel.CopyMove.Equals("M"))
                    {
                        // 이동이므로 From의 고객리스트를 삭제한다.
                        #region From 삭제
                        sbQuery = new StringBuilder();
                        sqlParams = new SqlParameter[1];

                        sbQuery.Append("--고객리스트 이동용 삭제\n"
                            + "DELETE ClientListHome         \n"
                            + " WHERE CollectionCode  = @FromCode  \n"
                            );

                        i = 0;
                        sqlParams[i++] = new SqlParameter("@FromCode", SqlDbType.Int);

                        i = 0;
                        sqlParams[i++].Value = Convert.ToInt32(collectionhomeModel.FromCode);
                        // 쿼리실행

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        // __DEBUG__

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                        #endregion
                    }

                    string at = "";
                    if (collectionhomeModel.CopyMove.Equals("M")) at = "이동";
                    else at = "복사";

                    _log.Message("고객리스트 " + at + ":From[" + collectionhomeModel.FromCode + "] To[" + collectionhomeModel.FromCode + "] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _db.CommitTran();

                collectionhomeModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientListCopyMove() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                collectionhomeModel.ResultCD = "3301";
                collectionhomeModel.ResultDesc = "고객리스트 복사/이동 중 오류가 발생하였습니다";
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
