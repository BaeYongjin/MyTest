using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Schedule
{
	/// <summary>
	/// SchPopularChannelBiz에 대한 요약 설명입니다.
	/// </summary>
	public class SchPopularChannelBiz : BaseBiz
	{
		public SchPopularChannelBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

		#region 메뉴 목록조회
		/// <summary>
		/// 메뉴 목록조회
		/// </summary>
		/// <param name="schPopularChannelModel"></param>
		/// 
		public void GetMenuList(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode      :[" + schPopularChannelModel.SearchMediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
					
				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT A.MediaCode    	  \n"
					+ "       ,B.MediaName	      \n"
					+ "       ,A.CategoryCode	  \n"
					+ "       ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),A.CategoryCode))) + CONVERT(VARCHAR(10),A.CategoryCode) + ' ' + C.CategoryName) AS CategoryName		\n"
					+ "       ,A.GenreCode		  \n"
					+ "       ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),A.GenreCode)))    + CONVERT(VARCHAR(10),A.GenreCode)    + ' ' + D.GenreName   ) AS GenreName         \n"
					+ "       ,(SELECT COUNT(*) FROM SchChoiceMenuDetail WHERE MediaCode = A.MediaCode AND GenreCode = A.GenreCode) AS AdCount \n"
					+ "   FROM (											 \n"
					+ "         SELECT MediaCode							 \n"
					+ "		 	   ,CategoryCode							 \n"
					+ "               ,GenreCode							 \n"
					+ "           FROM ChannelSet with(NoLock)               \n"
					+ "          WHERE MediaCode = '" + schPopularChannelModel.SearchMediaCode.Trim() + "' \n"
					+ "          GROUP BY MediaCode, CategoryCode, GenreCode \n"							 
					+ "		) A INNER JOIN Media    B with(NoLock) ON (A.MediaCode    = B.MediaCode   )    \n"
					+ "         INNER JOIN Category C with(NoLock) ON (A.CategoryCode = C.CategoryCode) \n"
					+ "         INNER JOIN Genre    D with(NoLock) ON (A.GenreCode    = D.GenreCode   ) \n"
					);
								
				sbQuery.Append(" ORDER BY C.CategoryCode,D.GenreCode   \n");
			
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 매체대행사광고주모델에 복사
				schPopularChannelModel.SchPopularChannelDataSet = ds.Copy();
				// 결과
				schPopularChannelModel.ResultCnt = Utility.GetDatasetCount(schPopularChannelModel.SchPopularChannelDataSet);
				// 결과코드 셋트
				schPopularChannelModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schPopularChannelModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() End");
				_log.Debug("-----------------------------------------");


			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD = "3000";
				schPopularChannelModel.ResultDesc = "메뉴정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		#endregion

		#region 채널목록조회
		
		/// <summary>
		/// 채널셋목록조회
		/// </summary>
		/// <param name="schPopularChannelModel"></param>
		public void GetChannelList(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(schPopularChannelModel.SearchStartDay.Length > 6) schPopularChannelModel.SearchStartDay = schPopularChannelModel.SearchStartDay.Substring(2,6);
				if(schPopularChannelModel.SearchEndDay.Length   > 6) schPopularChannelModel.SearchEndDay   = schPopularChannelModel.SearchEndDay.Substring(2,6);

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode    :[" + schPopularChannelModel.MediaCode    + "]");
				_log.Debug("SearchStartDay    :[" + schPopularChannelModel.SearchStartDay    + "]");		// 검색 집계시작 일자          
				_log.Debug("SearchEndDay      :[" + schPopularChannelModel.SearchEndDay      + "]");		// 검색 집계종료 일자          
				// __DEBUG__

				string StartDay    = schPopularChannelModel.SearchStartDay;
				string EndDay      = schPopularChannelModel.SearchEndDay;

				StringBuilder sbQuery = new StringBuilder();
			
				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT ROW_NUMBER() OVER(ORDER BY SUM(A.PgCnt) DESC ) AS Rank	   \n"
					+ "		  ,A.MediaCode        \n"
					+ "       ,E.MediaName	      \n"
					+ "       ,A.Category		  \n"
					+ "       ,B.CategoryName	  \n"
					+ "       ,A.Genre			  \n"
					+ "       ,C.GenreName        \n"
					+ "       ,A.Channel		  \n"
					+ "       ,D.TotalSeries      \n"
					+ "       ,D.Title	          \n"
					+ "       ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock)                            \n"
					+ "                        WHERE MediaCode = A.MediaCode AND ChannelNo = A.Channel)            \n" 
					+ "         +                                                                                    \n"  // 채널편성광고수 +  메뉴편성광고 중 상업광고수
					+ "        (SELECT COUNT(*) FROM SchChoiceMenuDetail SM with(NoLock)                             \n"
					+ "                   INNER JOIN ContractItem IT ON (SM.ItemNo = IT.ItemNo AND IT.AdType = '10') \n"  // AdType:10 상업광고
					+ "                        WHERE SM.MediaCode = A.MediaCode AND SM.GenreCode = A.Genre)      \n"
					+ "        AS AdCount             \n"
					+ "       ,A.PgCnt                \n"					
					+ "       ,F.Comment              \n"
					+ "       ,CASE WHEN ProdTypeCnt > 0 THEN 'PPx' ELSE '' END AS ProdType \n"
					+ "  FROM (                       \n"
					+ "       SELECT TA.MediaCode     \n"
					+ "             ,TA.Category	  \n"
					+ "             ,TA.Genre		  \n"
					+ "             ,TA.Channel		  \n"
					+ "             ,MIN(TC.SeriesNo) AS SeriesNo	  \n"
					+ "             ,ISNULL(SUM(TB.HitCnt),0)  AS PgCnt  \n"					
					+ "             ,SUM(CASE WHEN ProdType IS NOT NULL AND ProdType <> '' THEN 1 ELSE 0 END) AS ProdTypeCnt \n"
					+ "         FROM Program TA with(NoLock)   \n"
					+ "              LEFT JOIN SummaryPgDaily3  TB with(NoLock) ON (TA.ProgramKey = TB.ProgKey	    \n"
					+ "													AND TB.LogDay BETWEEN '"+ StartDay  + "'	\n"
					+ "																		AND '"+ EndDay    + "') \n"	
					+ "              LEFT JOIN Channel  TC with(NoLock) ON (TA.MediaCode = TC.MediaCode AND TA.Channel = TC.ChannelNo) \n"
					+ "              LEFT JOIN Contents TD with(NoLock) ON (TC.ContentID = TD.ContentID) \n"
					+ "        WHERE TA.MediaCode    = '" + schPopularChannelModel.MediaCode    + "' \n"					
					+ "        GROUP BY TA.MediaCode, TA.Category, TA.Genre, TA.Channel   \n"
					+ "       ) A INNER JOIN Category B with(NoLock) ON A.MediaCode = B.MediaCode AND A.Category = B.CategoryCode  \n"
					+ "           INNER JOIN Genre    C with(NoLock) ON A.MediaCode = C.MediaCode AND A.Genre    = C.GenreCode     \n"
					+ "           INNER JOIN Channel  D with(NoLock) ON A.MediaCode = D.MediaCode AND A.Channel    = D.ChannelNo AND A.SeriesNO = D.SeriesNo \n"
					+ "           INNER JOIN Media    E with(NoLock) ON A.MediaCode = E.MediaCode \n"
					+ "           INNER JOIN PopularChannel    F with(NoLock) ON A.MediaCode = F.MediaCode AND A.Channel    = F.ChannelNo \n");									
						// 검색어가 있으면
					if (schPopularChannelModel.SearchKey.Trim().Length > 0)
					{
						// 여러컬럼에 대하여 LIKE 검색을 한다.
						sbQuery.Append(" WHERE ("
							+ "    D.Title      LIKE '%" + schPopularChannelModel.SearchKey.Trim() + "%' \n"						
							+ " ) ");
					}
					sbQuery.Append( " GROUP BY   A.MediaCode      \n");				
					sbQuery.Append( "			,E.MediaName      \n");				
					sbQuery.Append( "			,A.Category       \n");				
					sbQuery.Append( "			,B.CategoryName   \n");				
					sbQuery.Append( "			,A.Genre		  \n");				
					sbQuery.Append( "			,C.GenreName      \n");				
					sbQuery.Append( "			,A.Channel        \n");				
					sbQuery.Append( "			,D.TotalSeries    \n");				
					sbQuery.Append( "			,D.Title          \n");							
					sbQuery.Append( "			,A.PgCnt          \n");												
					sbQuery.Append( "			,F.Comment        \n");							
					sbQuery.Append( "			,ProdTypeCnt      \n");							
					sbQuery.Append( " ORDER BY Rank,A.Channel          \n");				

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 매체대행사광고주모델에 복사
				schPopularChannelModel.SchPopularChannelDataSet = ds.Copy();
				// 결과
				schPopularChannelModel.ResultCnt = Utility.GetDatasetCount(schPopularChannelModel.SchPopularChannelDataSet);
				// 결과코드 셋트
				schPopularChannelModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schPopularChannelModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD = "3000";
				schPopularChannelModel.ResultDesc = "채널정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion
	
		#region 메뉴별 편성현황 조회
		/// <summary>
		/// 메뉴/채널별 편성현황 조회
		/// </summary>
		/// <param name="schPopularChannelModel"></param>
		public void GetChooseAdScheduleListMenu(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{

			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChooseAdScheduleListMenu() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode    :[" + schPopularChannelModel.MediaCode  + "]");
				_log.Debug("GenreCode    :[" + schPopularChannelModel.GenreCode + "]");
				// __DEBUG__



				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT 'M' AS ViewType              \n"
					+ "      ,A.CmType                     \n"
					+ "      ,CASE WHEN CmType = 'M' THEN '메뉴' ELSE '채널' END AS CmName \n"
					+ "      ,B.AdType                     \n" 
					+ "      ,G.CodeName as AdTypeName     \n"
					+ "      ,A.ScheduleOrder              \n"
					+ "      ,A.ItemNo                     \n"  
					+ "      ,B.ItemName                   \n" 
					+ "      ,C.ContractName               \n" 
					+ "      ,E.AdvertiserName             \n" 
					+ "      ,C.State    AS ContState      \n"
					+ "      ,F.CodeName AS ContStateName  \n" 
					+ "      ,B.RealEndDay                 \n"
					+ "      ,B.AdState                    \n"
					+ "      ,H.CodeName AS AdStatename    \n" 
					+ "      ,B.MediaCode                  \n" 
					+ "      ,A.GenreCode                  \n" 
					+ "      ,'0' AS ChannelNo             \n" 
					+ "      ,B.FileState                  \n"
					+ "      ,I.CodeName AS FileStatename  \n"
					+ "      ,J.State AS AckState          \n"
					+ "      ,'False' AS CheckYn           \n"
					+ "  FROM (                            \n" 
					+ "         SELECT 'M' AS CmType, ItemNo, ScheduleOrder, GenreCode, AckNo \n"
					+ "           FROM SchChoiceMenuDetail with(NoLock)                       \n" 
					+ "          WHERE MediaCode = '" + schPopularChannelModel.MediaCode  + "' \n"
					+ "            AND GenreCode = '" + schPopularChannelModel.GenreCode  + "' \n"
					+ "       ) AS A    INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo         = B.ItemNo)                     \n"    
					+ "                 INNER JOIN Contract     C with(NoLock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq)  \n"
					+ "                 LEFT  JOIN Advertiser   E with(NoLock) ON (B.AdvertiserCode = E.AdvertiserCode)             \n"
					+ "                 LEFT  JOIN SystemCode   F with(NoLock) ON (C.State          = F.Code AND F.Section = '23')  \n"	// 23 : 계약상태
					+ "                 LEFT  JOIN SystemCode   G with(NoLock) ON (B.AdType         = G.Code AND G.Section = '26')  \n"	// 26 : 광고종류
					+ "                 LEFT  JOIN SystemCode   H with(NoLock) ON (B.AdState        = H.Code AND H.Section = '25')  \n"	// 25 : 광고상태
					+ "                 LEFT  JOIN SystemCode   I with(NoLock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 31 : 파일상태
					+ "                 LEFT  JOIN SchPublish   J with(NoLock) ON (A.AckNo          = J.AckNo)                      \n"
					+ " ORDER BY CmType DESC, AdType, ScheduleOrder   \n"
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				schPopularChannelModel.SchPopularChannelDataSet = ds.Copy();

				ds.Dispose();

				//지정메뉴편성의 마지막  Order를 구함
				string LastOrder = "1";
				sbQuery = new StringBuilder();
				sbQuery.Append( "\n"
					+ "SELECT ISNULL(MAX(ScheduleOrder),0) AS MaxOrder                \n"
					+ "  FROM SchChoiceMenuDetail with(NoLock)                        \n"
					+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
					+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode + " \n"
					);

				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					LastOrder = Utility.GetDatasetString(ds, 0, "MaxOrder");					
				}
				schPopularChannelModel.LastOrder = LastOrder;
				ds.Dispose();

				// 결과
				schPopularChannelModel.ResultCnt = Utility.GetDatasetCount(schPopularChannelModel.SchPopularChannelDataSet);
				// 결과코드 셋트
				schPopularChannelModel.ResultCD = "0000";

				

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schPopularChannelModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChooseAdScheduleListMenu() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD = "3000";
				schPopularChannelModel.ResultDesc = "메뉴별 편성현황 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion
		
		#region 채널별 편성현황 조회
		/// <summary>
		/// 채널별 편성현황 조회
		/// </summary>
		/// <param name="schPopularChannelModel"></param>
		public void GetChooseAdScheduleListChannel(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{

			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChooseAdScheduleListChannel() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode    :[" + schPopularChannelModel.MediaCode + "]");
				_log.Debug("GenreCode    :[" + schPopularChannelModel.GenreCode + "]");
				_log.Debug("ChannelNo    :[" + schPopularChannelModel.ChannelNo + "]");
				// __DEBUG__


				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT 'C' AS ViewType              \n"
					+ "      ,A.CmType                     \n"
					//+ "      ,CASE WHEN CmType = 'M' THEN '메뉴' ELSE '채널' END AS CmName \n"
					+ "      ,B.AdType                     \n" 
					+ "      ,G.CodeName as AdTypeName     \n"
					+ "      ,A.ScheduleOrder              \n"
					+ "      ,A.ItemNo                     \n"  
					+ "      ,B.ItemName                   \n" 
					+ "      ,C.ContractName               \n" 
					+ "      ,E.AdvertiserName             \n" 
					+ "      ,C.State    AS ContState      \n"
					+ "      ,F.CodeName as ContStateName  \n" 
					+ "      ,B.RealEndDay                 \n" 
					+ "      ,B.AdState                    \n"
					+ "      ,H.CodeName as AdStatename    \n" 
					+ "      ,B.MediaCode                  \n" 
					+ "      ,A.GenreCode                  \n" 
					+ "      ,A.ChannelNo                  \n" 
					+ "      ,B.FileState                  \n"
					+ "      ,I.CodeName AS FileStatename  \n"
					+ "      ,J.State AS AckState          \n"
					+ "      ,'False' AS CheckYn           \n"
					+ "  FROM (                            \n" 
					+ "         SELECT 'M' AS CmType, SM.ItemNo, SM.ScheduleOrder, SM.GenreCode, '' AS ChannelNo, AckNo  \n"               
					+ "           FROM SchChoiceMenuDetail SM with(NoLock) INNER JOIN ContractItem IT with(NoLock) ON (SM.ItemNo = IT.ItemNo AND IT.AdType BETWEEN '10' AND '19') \n"  // AdType:광고종류 10:상업광고
					+ "          WHERE SM.MediaCode = '" + schPopularChannelModel.MediaCode  + "' \n"
					+ "            AND SM.GenreCode = '" + schPopularChannelModel.GenreCode  + "' \n"
					+ "         UNION                                                         \n"
					+ "         SELECT 'C' AS CmType, SC.ItemNo, SC.ScheduleOrder,            \n"
					+ "                '" + schPopularChannelModel.GenreCode  + "' AS GenreCode, SC.ChannelNo, AckNo \n"  
					+ "           FROM SchChoiceChannelDetail SC with(NoLock) \n"
					+ "          WHERE SC.MediaCode = '" + schPopularChannelModel.MediaCode + "' \n"
					+ "            AND SC.ChannelNo = '" + schPopularChannelModel.ChannelNo + "' \n"
					+ "       ) AS A    INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo         = B.ItemNo)                      \n"    
					+ "                 INNER JOIN Contract     C with(NoLock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq)  \n"
					+ "                 LEFT  JOIN Advertiser   E with(NoLock) ON (B.AdvertiserCode = E.AdvertiserCode)      \n"
					+ "                 LEFT  JOIN SystemCode   F with(NoLock) ON (C.State          = F.Code AND F.Section = '23')  \n"	// 23 : 계약상태
					+ "                 LEFT  JOIN SystemCode   G with(NoLock) ON (B.AdType         = G.Code AND G.Section = '26')  \n"	// 26 : 광고종류
					+ "                 LEFT  JOIN SystemCode   H with(NoLock) ON (B.AdState        = H.Code AND H.Section = '25')  \n"	// 25 : 광고상태
					+ "                 LEFT  JOIN SystemCode   I with(NoLock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 31 : 파일상태
					+ "                 LEFT  JOIN SchPublish   J with(NoLock) ON (A.AckNo          = J.AckNo)                      \n"
					+ " ORDER BY CmType DESC, AdType, ScheduleOrder                           \n"
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				schPopularChannelModel.SchPopularChannelDataSet = ds.Copy();

				ds.Dispose();

				//지정메뉴편성의 마지막  Order를 구함
				string LastOrder = "1";
				sbQuery = new StringBuilder();
				sbQuery.Append( "\n"
					+ "SELECT ISNULL(MAX(ScheduleOrder),0) AS MaxOrder                \n"
					+ "  FROM SchChoiceChannelDetail with(NoLock)                     \n"
					+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
					+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo + " \n"
					);

				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					LastOrder = Utility.GetDatasetString(ds, 0, "MaxOrder");					
				}
				schPopularChannelModel.LastOrder = LastOrder;
				ds.Dispose();


				// 결과
				schPopularChannelModel.ResultCnt = Utility.GetDatasetCount(schPopularChannelModel.SchPopularChannelDataSet);
				// 결과코드 셋트
				schPopularChannelModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schPopularChannelModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChooseAdScheduleListChannel() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD = "3000";
				schPopularChannelModel.ResultDesc = "채널별 편성현황 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}


		}

		#endregion

		#region 장르목록조회(POP)
		/// <summary>
		/// 장르목록조회
		/// </summary>
		/// <param name="schPopularChannelModel"></param>
		public void GetGenreGroupList_p(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreGroupList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + schPopularChannelModel.SearchKey       + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				int i = 0;
				SqlParameter[] sqlParams = new SqlParameter[1];
				// 쿼리생성
				sbQuery.Append("\n");											 
				sbQuery.Append("   SELECT  'False' AS CheckYn					  \n");
				sbQuery.Append("          ,a.MediaCode							  \n");
				sbQuery.Append("          ,a.CategoryCode						  \n");				
				sbQuery.Append("   		  ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.CategoryCode))) +  CONVERT(VARCHAR(10),a.CategoryCode) + ' ' + b.CategoryName) AS CategoryName \n");
				sbQuery.Append("   		  ,a.GenreCode       					  \n");
				sbQuery.Append("   		  ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.GenreCode))) +  CONVERT(VARCHAR(10),a.GenreCode) + ' ' + c.GenreName) AS GenreName \n");				
				sbQuery.Append("   FROM (					    				  \n");
				sbQuery.Append("   		    SELECT    a.MediaCode				  \n");
				sbQuery.Append("                     ,a.CategoryCode			  \n");
				sbQuery.Append("   					 ,a.GenreCode				  \n");
				sbQuery.Append("   			FROM      ChannelSet a				  \n");
				sbQuery.Append("   			WHERE     a.MediaCode = @MediaCode	  \n");
				sbQuery.Append("   			GROUP BY  a.MediaCode				  \n");
				sbQuery.Append("                     ,a.CategoryCode			  \n");
				sbQuery.Append("                     ,a.GenreCode   			  \n");
				sbQuery.Append("   	    )  a,Category b, Genre c				  \n");
				sbQuery.Append("   WHERE   a.CategoryCode = b.CategoryCode		  \n");
				sbQuery.Append("     AND   a.GenreCode = c.GenreCode			  \n");
                
				
				// 검색어가 있으면
				if (schPopularChannelModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("   AND ( b.CategoryName      LIKE '%" + schPopularChannelModel.SearchKey.Trim() + "%' \n");
					sbQuery.Append("   OR c.GenreName    LIKE '%" + schPopularChannelModel.SearchKey.Trim() + "%' )       \n");
                      
				}
				sbQuery.Append("   ORDER BY b.CategoryCode,c.GenreCode			  \n");

				i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt);
                               
				i = 0;
				sqlParams[i++].Value = schPopularChannelModel.MediaCode;	


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// 결과 DataSet의 장르모델에 복사
				schPopularChannelModel.SchPopularChannelDataSet = ds.Copy();
				// 결과
				schPopularChannelModel.ResultCnt = Utility.GetDatasetCount(schPopularChannelModel.SchPopularChannelDataSet);
				// 결과코드 셋트
				schPopularChannelModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schPopularChannelModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreGroupListPopUp() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD = "3000";
				schPopularChannelModel.ResultDesc = "장르정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		#endregion

		#region 채널목록조회(POP)
		/// <summary>
		/// 채널목록조회
		/// </summary>
		/// <param name="schPopularChannelModel"></param>
		public void GetChannelList_p(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelGroupList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + schPopularChannelModel.SearchKey       + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				int i = 0;
				SqlParameter[] sqlParams = new SqlParameter[3];
				// 쿼리생성
				sbQuery.Append("\n");											 
				sbQuery.Append("   SELECT 'False' AS CheckYn                \n");
				sbQuery.Append("         ,a.ChannelNo,b.Title               \n");
				sbQuery.Append("   FROM   channelSet a, Channel b           \n");
				sbQuery.Append("   WHERE  a.ChannelNo = b.ChannelNo         \n");
				sbQuery.Append("     AND  a.MediaCode = @MediaCode          \n");
				sbQuery.Append("     AND  a.CategoryCode = @CategoryCode    \n");
				sbQuery.Append("     AND  a.GenreCode = @GenreCode          \n");
				sbQuery.Append(" GROUP BY a.ChannelNo,b.Title	            \n");
				sbQuery.Append(" ORDER BY a.ChannelNo			            \n");

				i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@CategoryCode", SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@GenreCode"    , SqlDbType.Int);
                               
				i = 0;
				sqlParams[i++].Value = schPopularChannelModel.MediaCode;	
				sqlParams[i++].Value = schPopularChannelModel.CategoryCode;	
				sqlParams[i++].Value = schPopularChannelModel.GenreCode;	


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// 결과 DataSet의 채널모델에 복사
				schPopularChannelModel.SchPopularChannelDataSet = ds.Copy();
				// 결과
				schPopularChannelModel.ResultCnt = Utility.GetDatasetCount(schPopularChannelModel.SchPopularChannelDataSet);
				// 결과코드 셋트
				schPopularChannelModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schPopularChannelModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelGroupListPopUp() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD = "3000";
				schPopularChannelModel.ResultDesc = "채널정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 채널 통계
		/// <summary>
		///  기간내 채널통계
		/// </summary>
		/// <param name="schPopularChannelModel"></param>
		public void GetStatisticsPgChannel(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
			bool isNotReady  = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsPgChannel() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(schPopularChannelModel.SearchStartDay.Length > 6) schPopularChannelModel.SearchStartDay = schPopularChannelModel.SearchStartDay.Substring(2,6);
				if(schPopularChannelModel.SearchEndDay.Length   > 6) schPopularChannelModel.SearchEndDay   = schPopularChannelModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	  :[" + schPopularChannelModel.SearchMediaCode   + "]");		// 검색 매체
				_log.Debug("SearchStartDay    :[" + schPopularChannelModel.SearchStartDay    + "]");		// 검색 집계시작 일자          
				_log.Debug("SearchEndDay      :[" + schPopularChannelModel.SearchEndDay      + "]");		// 검색 집계종료 일자          				    
				// __DEBUG__

				string MediaCode   = schPopularChannelModel.SearchMediaCode;
				string StartDay    = schPopularChannelModel.SearchStartDay;
				string EndDay      = schPopularChannelModel.SearchEndDay;
				
				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* 기간내 컨텐츠 채널 통계    */           \n"
					+ "DECLARE @TotPgHit int;    -- 전체 컨텐츠노출수  \n"
					+ "SET @TotPgHit    = 0;                      \n"
					+ "                                           \n"
					+ "-- 전체 컨텐츠Hit                          \n"
					+ "SELECT @TotPgHit = ISNULL(SUM(A.HitCnt),0) \n"
					+ "  FROM SummaryPgDaily3 A                   \n"
					+ " WHERE A.LogDay BETWEEN '"+ StartDay  + "' \n"
					+ "                    AND '"+ EndDay    + "' \n"
					+ "                                           \n"
					+ "-- 채널 집계                                 \n"
					+ "SELECT TOP 100                             \n"
					+ "       'False' AS CheckYn                  \n"
					+ "      ,ROW_NUMBER() OVER(ORDER BY SUM(A.HitCnt) DESC ) AS Rank   \n"
					+ "      ,B.Channel	                          \n"
					+ "	     ,B.ProgramNm                         \n"
					+ "      ,SUM(A.HitCnt)  AS PgCnt             \n"					
					+ "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(A.HitCnt),0) / CONVERT(float,@TotPgHit)) * 100)   \n"
					+ "                               ELSE 0 END AS PgRate                                                                   \n"					
					+ " FROM SummaryPgDaily3 A INNER JOIN Program  B with(NoLock) ON (A.ProgKey  = B.ProgramKey AND B.MediaCode = 1)                      \n"
					+ "   WHERE A.LogDay BETWEEN '"+ StartDay  + "' \n"
					+ "                    AND '"+ EndDay    + "' \n"					
					+ " GROUP BY B.ProgramNm, B.Channel           \n"
					+ "                                           \n"
					+ "ORDER BY Rank, PgCnt                    \n"
					); 


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				schPopularChannelModel.SchPopularChannelDataSet = ds.Copy();

				// 결과
				schPopularChannelModel.ResultCnt = Utility.GetDatasetCount(schPopularChannelModel.SchPopularChannelDataSet);

				ds.Dispose();

				// 결과코드 셋트
				schPopularChannelModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schPopularChannelModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsPgChannel() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD = "3000";
				if(isNotTarget)
				{
					schPopularChannelModel.ResultDesc = "해당 기간의 정보가 존재하지 않습니다.";
				}
				else if(isNotReady)
				{
					schPopularChannelModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					schPopularChannelModel.ResultDesc = "채널통계 조회중 오류가 발생하였습니다";
					_log.Exception(ex);
				}
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 채널 저장
		/// <summary>
		/// 채널 저장
		/// </summary>
		/// <returns></returns>
		public void SetSchChoiceChannelDetailCreate(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{			
			try
			{					

				// 데이터베이스를 OPEN한다
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode         :[" + schPopularChannelModel.MediaCode + "]");
				_log.Debug("ChannelNo         :[" + schPopularChannelModel.ChannelNo + "]");				
				// __DEBUG__				

				
				// 데이터베이스를 OPEN한다
				_db.Open();

				// 지정채널 편성 테이블에 추가
				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
						
				SqlParameter[] sqlParams = new SqlParameter[4];		
										
				sbQuery.Append( "\n"
					+ "INSERT INTO PopularChannel (      \n"
					+ "            MediaCode                     \n"
					+ "           ,ChannelNo                     \n"
					+ "           ,Comment                     \n"								
					+ "           ,RegDt                         \n"
					+ "           ,RegID                 \n"
					+ "       )                                  \n"
					+ "       VALUES (                           \n"					
					+ "           @MediaCode                     \n"
					+ "          ,@ChannelNo                     \n"
					+ "          ,@Comment                        \n"
					+ "          ,GETDATE()                   \n"
					+ "          ,@RegID                              \n" // 필수광고의 순서는 0
					+ "       )                                  \n"	
					);					
				                    
					//먼저 인서트 된 데이터를 지워주고 그이후에 인서트를 한다.
					i = 0;
					sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt   );						
					sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int       );
					sqlParams[i++] = new SqlParameter("@Comment"       , SqlDbType.VarChar , 200);
					sqlParams[i++] = new SqlParameter("@RegID"        , SqlDbType.VarChar , 10);
                    
					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.MediaCode);
					sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.ChannelNo);
					sqlParams[i++].Value = schPopularChannelModel.Comment;
					sqlParams[i++].Value = header.UserID;				// 등록자
					
					// __DEBUG__
					_log.Debug("MediaCode:[" + schPopularChannelModel.MediaCode + "]");
					_log.Debug("ChannelNo:[" + schPopularChannelModel.ChannelNo + "]");
					_log.Debug("Comment:[" + schPopularChannelModel.Comment + "]");
						
					_log.Debug(sbQuery.ToString());											
				
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("채널정보생성:[" + schPopularChannelModel.ChannelNo + "(" + schPopularChannelModel.ChannelNo + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}	
				schPopularChannelModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3101";
				schPopularChannelModel.ResultDesc = "채널 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}			
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion 

		#region 채널광고 편성 삭제
		/// <summary>
		/// 채널 삭제
		/// </summary>
		/// <returns></returns>
		public void SetPopularChannelDelete(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetPopularChannelDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();
                        
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];
                        
				sbQuery.Append( ""
					+ "        DELETE FROM  PopularChannel			                    \n"					
					+ "				   WHERE MediaCode = @MediaCode				\n"
					+ "                AND ChannelNo = @ChannelNo				\n"								
					);                             
                                        
				i = 0;				
				sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt   );						
				sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int       );
				                    
				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.Channel);
				
				_log.Debug("MediaCode:[" + schPopularChannelModel.MediaCode + "]");
				_log.Debug("ChannelNo:[" + schPopularChannelModel.Channel + "]");
				
				_log.Debug(sbQuery.ToString());
                        
				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();
                        
					// __MESSAGE__
					_log.Message("채널정보삭제:[" + schPopularChannelModel.ChannelNo + "] 등록자:[" + header.UserID + "]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				schPopularChannelModel.ResultCD = "0000";  // 정상
                        	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetPopularChannelDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3301";
				schPopularChannelModel.ResultDesc = "채널 삭제중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}  
		#endregion 

		#region 채널광고 편성 저장
		/// <summary>
		/// 채널광고 편성 저장
		/// </summary>
		/// <returns></returns>
		public void SetSchPopularChannelDetailCreate(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{			
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchPopularChannelDetailCreate() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("ItemNo         :[" + schPopularChannelModel.ItemNo + "]");
				_log.Debug("MediaCode         :[" + schPopularChannelModel.MediaCode + "]");
				_log.Debug("ChannelNo         :[" + schPopularChannelModel.ChannelNo + "]");				
				// __DEBUG__				

				
				// 데이터베이스를 OPEN한다
				_db.Open();

				// 현재 승인번호를 구함
				string AckNo = GetLastAckNo(schPopularChannelModel.MediaCode);

				// 지정채널 편성 테이블에 추가
				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;

								
				SqlParameter[] sqlParams = new SqlParameter[3];		
										
				sbQuery.Append( "\n"
					+ "INSERT INTO SchPopularChannel (      \n"
					+ "            ItemNo                     \n"
					+ "           ,MediaCode                     \n"
					+ "           ,ChannelNo                     \n"
					+ "           ,AckNo                         \n"					
					+ "       )                                  \n"
					+ "       VALUES (                           \n"					
					+ "           @ItemNo                     \n"
					+ "          ,@MediaCode                     \n"
					+ "          ,@ChannelNo                     \n"
					+ "          ," + AckNo                 + "  \n"
					+ "       )                                  \n"	
					);					
				                    
				//먼저 인서트 된 데이터를 지워주고 그이후에 인서트를 한다.
				i = 0;
				sqlParams[i++] = new SqlParameter("@ItemNo"       , SqlDbType.Int       );
				sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt   );						
				sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int       );
				                    
				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.ItemNo);
				sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.ChannelNo);
									
				// __DEBUG__
				_log.Debug("ItemNo:[" + schPopularChannelModel.ItemNo + "]");
				_log.Debug("MediaCode:[" + schPopularChannelModel.MediaCode + "]");
				_log.Debug("ChannelNo:[" + schPopularChannelModel.ChannelNo + "]");
										
				_log.Debug(sbQuery.ToString());											
				
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("채널정보생성:[" + schPopularChannelModel.ChannelNo + "(" + schPopularChannelModel.ChannelNo + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}	
				schPopularChannelModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchPopularChannelDetailCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3101";
				schPopularChannelModel.ResultDesc = "채널광고 편성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}			
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion 

		#region 채널광고 편성 삭제
		/// <summary>
		/// 채널광고 편성 삭제
		/// </summary>
		/// <returns></returns>
		public void SetSchPopularChannelDelete(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchPopularChannelDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();
                        
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[3];
                        
				sbQuery.Append( ""
					+ "        DELETE FROM  SchPopularChannel			                    \n"
					+ "              WHERE ItemNo = @ItemNo				\n"
					+ "				   AND MediaCode = @MediaCode				\n"
					+ "                AND ChannelNo = @ChannelNo				\n"					
					);                             
                                        
				i = 0;
				sqlParams[i++] = new SqlParameter("@ItemNo"       , SqlDbType.Int       );
				sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt   );						
				sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int       );
				                    
				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.ItemNo);
				sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.ChannelNo);

				_log.Debug("ItemNo:[" + schPopularChannelModel.ItemNo + "]");
				_log.Debug("MediaCode:[" + schPopularChannelModel.MediaCode + "]");
				_log.Debug("ChannelNo:[" + schPopularChannelModel.ChannelNo + "]");
				
				_log.Debug(sbQuery.ToString());
                        
				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();
                        
					// __MESSAGE__
					_log.Message("채널정보삭제:[" + schPopularChannelModel.ChannelNo + "] 등록자:[" + header.UserID + "]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				schPopularChannelModel.ResultCD = "0000";  // 정상
                        	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchPopularChannelDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3301";
				schPopularChannelModel.ResultDesc = "채널광고 삭제중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}  
		#endregion 

		#region 채널 비고 저장
		/// <summary>
		/// 채널 비고 저장
		/// </summary>
		/// <returns></returns>
		public void SetPopularChannelUpdate(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetPopularChannelUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[6];

				sbQuery.Append(""
					+ "UPDATE PopularChannel                     \n"
					+ "   SET Comment      = @Comment      \n"								
					+ " WHERE MediaCode        = @MediaCode        \n"
					+ " AND	  ChannelNo        = @ChannelNo        \n"
					);

				i = 0;
				sqlParams[i++] = new SqlParameter("@Comment"       , SqlDbType.VarChar , 200);
				sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt   );						
				sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int       );		

				i = 0;
				sqlParams[i++].Value = schPopularChannelModel.Comment;
				sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(schPopularChannelModel.Channel);				

				_log.Debug("Comment:[" + schPopularChannelModel.Comment + "]");
				_log.Debug("MediaCode:[" + schPopularChannelModel.MediaCode + "]");
				_log.Debug("ChannelNo:[" + schPopularChannelModel.Channel + "]");				
				
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("매체정보수정:["+schPopularChannelModel.MediaCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPopularChannelModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetPopularChannelUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3201";
				schPopularChannelModel.ResultDesc = "매체정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}
		#endregion 

		#region 지정메뉴편성  첫번째 순위로
		/// <summary>
		/// 지정메뉴편성  첫번째 순위로
		/// </summary>
		/// <returns></returns>
		public void SetSchMenuAdOrderFirst(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderFirst() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schPopularChannelModel.MediaCode);


					// 변경할 순위
					string ToOrder = "1"; 

					// 해당 매체중 MIN값
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS MinOrder   \n"
						+ "  FROM SchChoiceMenuDetail                                     \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode + " \n"
						+ "   AND ScheduleOrder > 0                                       \n"
						);

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "MinOrder");					
					}

					ds.Dispose();
		
					_db.BeginTran();

					// 해당 순위보다 작은 순위의 내역들을 +1하여 조정
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                \n"
						+ "   SET ScheduleOrder = ScheduleOrder + 1   \n"
						+ "      ,AckNo  = " + AckNo              + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder < " + schPopularChannelModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                                           \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                                     \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode + " \n"
						+ "   AND ItemNo        = " + schPopularChannelModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정메뉴편성 첫번째 순위로 변경:[" + schPopularChannelModel.ItemName + "] 등록자:[" + header.UserID + "]");

					schPopularChannelModel.ScheduleOrder = ToOrder;  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
					_log.Debug("ScheduleOrder:[" + schPopularChannelModel.ScheduleOrder + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPopularChannelModel.ResultCD = "0000";  // 정상
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderFirst() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3101";
				schPopularChannelModel.ResultDesc = " 지정메뉴편성  첫번째 순위로 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		#endregion

		#region 지정메뉴편성  순위올림
		/// <summary>
		/// 지정메뉴편성  순위올림
		/// </summary>
		/// <returns></returns>
		public void SetSchMenuAdOrderUp(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderUp() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schPopularChannelModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS UpOrder                    \n"
						+ "  FROM SchChoiceMenuDetail                                         \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder < " + schPopularChannelModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                                           \n"
						);

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "UpOrder");					
					}

					ds.Dispose();

					_db.BeginTran();

					// 변경할 순위를 해당순위로 변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                \n"
						+ "   SET ScheduleOrder = " + schPopularChannelModel.ScheduleOrder + " \n"
						+ "      ,AckNo         = " + AckNo                               + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder = " + ToOrder                             + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                                     \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode + " \n"
						+ "   AND ItemNo        = " + schPopularChannelModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정메뉴편성 순위올림 변경:[" + schPopularChannelModel.ItemName + "] 등록자:[" + header.UserID + "]");
					schPopularChannelModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPopularChannelModel.ResultCD = "0000";  // 정상
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchHomeAdOrderUp() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3101";
				schPopularChannelModel.ResultDesc = " 지정메뉴편성 순위올림 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion

		#region 지정메뉴편성  순위내림
		/// <summary>
		/// 지정메뉴편성  순위내림
		/// </summary>
		/// <returns></returns>
		public void SetSchMenuAdOrderDown(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderDown() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schPopularChannelModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS DownOrder                  \n"
						+ "  FROM SchChoiceMenuDetail                                         \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder > " + schPopularChannelModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                                           \n"
						);

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "DownOrder");					
					}

					ds.Dispose();


					_db.BeginTran();

					// 변경할 순위를 +1하여 조정
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                \n"
						+ "   SET ScheduleOrder = " + schPopularChannelModel.ScheduleOrder + " \n"
						+ "      ,AckNo         = " + AckNo                               + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder = " + ToOrder                             + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                                     \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode + " \n"
						+ "   AND ItemNo        = " + schPopularChannelModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정메뉴편성 순위올림 변경:[" + schPopularChannelModel.ItemName + "] 등록자:[" + header.UserID + "]");
					schPopularChannelModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPopularChannelModel.ResultCD = "0000";  // 정상
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderDown() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3101";
				schPopularChannelModel.ResultDesc = " 지정메뉴편성 순위내림 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		#endregion

		#region 지정메뉴편성  마지막 순위로

		/// <summary>
		/// 지정메뉴편성  마지막 순위로
		/// </summary>
		/// <returns></returns>
		public void SetSchMenuAdOrderLast(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderLast() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schPopularChannelModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder              \n"
						+ "  FROM SchChoiceMenuDetail                                     \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode + " \n"
						+ "   AND ScheduleOrder > 0                                       \n"
						);

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "LastOrder");					
					}

					ds.Dispose();

					_db.BeginTran();

					// 해당 순위보다 큰 순위의 내역들을 -1하여 조정
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                                         \n"
						+ "   SET ScheduleOrder = ScheduleOrder - 1                           \n"
						+ "      ,AckNo         = " + AckNo                               + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder > " + schPopularChannelModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                                       \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                                     \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + schPopularChannelModel.GenreCode + " \n"
						+ "   AND ItemNo        = " + schPopularChannelModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정메뉴편성 마지막 순위로 변경:[" + schPopularChannelModel.ItemName + "] 등록자:[" + header.UserID + "]");
					schPopularChannelModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
					_log.Debug("ScheduleOrder:[" + schPopularChannelModel.ScheduleOrder + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPopularChannelModel.ResultCD = "0000";  // 정상
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderLast() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3101";
				schPopularChannelModel.ResultDesc = " 지정메뉴편성 마지막 순위로 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion 


		#region 지정채널편성  첫번째 순위로
		/// <summary>
		/// 지정채널편성  첫번째 순위로
		/// </summary>
		/// <returns></returns>
		public void SetSchChannelAdOrderFirst(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChannelAdOrderFirst() Start");
				_log.Debug("-----------------------------------------");

				_log.Debug("MediaCode      ["+ schPopularChannelModel.MediaCode +"]");
				_log.Debug("ChannelNo      ["+ schPopularChannelModel.ChannelNo +"]");
				_log.Debug("ScheduleOrder  ["+ schPopularChannelModel.ScheduleOrder +"]");
				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schPopularChannelModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1"; 

					// 해당 매체중 MIN값
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS MinOrder   \n"
						+ "  FROM SchChoiceChannelDetail                                  \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo + " \n"
						+ "   AND ScheduleOrder > 0                                       \n"
						);

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "MinOrder");					
					}

					ds.Dispose();
		
					_db.BeginTran();



					// 해당 순위보다 작은 순위의 내역들을 +1하여 조정
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail                                  \n"
						+ "   SET ScheduleOrder = ScheduleOrder + 1                       \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo + " \n"
						+ "   AND ScheduleOrder < " + schPopularChannelModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                  \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail                                  \n"
						+ "   SET ScheduleOrder = " + ToOrder.ToString()              + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo + " \n"
						+ "   AND ItemNo        = " + schPopularChannelModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정채널편성 첫번째 순위로 변경:[" + schPopularChannelModel.ItemName + "] 등록자:[" + header.UserID + "]");

					schPopularChannelModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPopularChannelModel.ResultCD = "0000";  // 정상
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChannelAdOrderFirst() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3101";
				schPopularChannelModel.ResultDesc = " 지정채널편성  첫번째 순위로 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		#endregion

		#region 지정채널편성  순위올림
		/// <summary>
		/// 지정채널편성  순위올림
		/// </summary>
		/// <returns></returns>
		public void SetSchChannelAdOrderUp(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderUp() Start");
				_log.Debug("-----------------------------------------");

				_log.Debug("MediaCode      ["+ schPopularChannelModel.MediaCode +"]");
				_log.Debug("ChannelNo      ["+ schPopularChannelModel.ChannelNo +"]");
				_log.Debug("ScheduleOrder  ["+ schPopularChannelModel.ScheduleOrder +"]");
				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schPopularChannelModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS UpOrder                    \n"
						+ "  FROM SchChoiceChannelDetail                                      \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode     + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo     + " \n"
						+ "   AND ScheduleOrder < " + schPopularChannelModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                                       \n"
						);

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "UpOrder");					
					}

					ds.Dispose();

					_db.BeginTran();

					// 변경할 순위를 +1하여 조정
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail             \n"
						+ "   SET ScheduleOrder = " + schPopularChannelModel.ScheduleOrder + " \n"
						+ "      ,AckNo         = " + AckNo                               + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode     + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo     + " \n"
						+ "   AND ScheduleOrder = " + ToOrder                             + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail                                  \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo + " \n"
						+ "   AND ItemNo        = " + schPopularChannelModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정채널편성 순위올림 변경:[" + schPopularChannelModel.ItemName + "] 등록자:[" + header.UserID + "]");
					schPopularChannelModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPopularChannelModel.ResultCD = "0000";  // 정상
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChannelAdOrderUp() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3101";
				schPopularChannelModel.ResultDesc = " 지정채널편성 순위올림 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion

		#region 지정채널편성  순위내림
		/// <summary>
		/// 지정채널편성  순위내림
		/// </summary>
		/// <returns></returns>
		public void SetSchChannelAdOrderDown(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderDown() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schPopularChannelModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS DownOrder                  \n"
						+ "  FROM SchChoiceChannelDetail                                      \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode     + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo     + " \n"
						+ "   AND ScheduleOrder > " + schPopularChannelModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                                       \n"
						);

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "DownOrder");					
					}

					ds.Dispose();

					_db.BeginTran();

					// 변경할 순위를 +1하여 조정
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail                                      \n"
						+ "   SET ScheduleOrder = " + schPopularChannelModel.ScheduleOrder + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode     + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo     + " \n"
						+ "   AND ScheduleOrder = " + ToOrder                             + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail                     \n"
						+ "   SET ScheduleOrder = " + ToOrder.ToString() + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo + " \n"
						+ "   AND ItemNo        = " + schPopularChannelModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정채널편성 순위올림 변경:[" + schPopularChannelModel.ItemName + "] 등록자:[" + header.UserID + "]");
					schPopularChannelModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPopularChannelModel.ResultCD = "0000";  // 정상
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChannelAdOrderDown() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3101";
				schPopularChannelModel.ResultDesc = " 지정채널편성 순위내림 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion

		#region 지정채널편성  마지막 순위로

		/// <summary>
		/// 지정채널편성  마지막 순위로
		/// </summary>
		/// <returns></returns>
		public void SetSchChannelAdOrderLast(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChannelAdOrderLast() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schPopularChannelModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					//지정채널편성 마지막  Order를 구함
					sbQuery.Append( "\n"
						+ "SELECT ISNULL(MAX(ScheduleOrder),1) AS MaxOrder                \n"
						+ "  FROM SchChoiceChannelDetail                                  \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo + " \n"
						+ "   AND ScheduleOrder > 0                                       \n"
						);

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "MaxOrder");					
					}

					ds.Dispose();	

					_db.BeginTran();

					// 해당 순위보다 큰 순위의 내역들을 -1하여 조정
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail                                      \n"
						+ "   SET ScheduleOrder = ScheduleOrder - 1                           \n"
						+ "      ,AckNo         = " + AckNo                               + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode     + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo     + " \n"
						+ "   AND ScheduleOrder > " + schPopularChannelModel.ScheduleOrder + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail                                  \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + schPopularChannelModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + schPopularChannelModel.ChannelNo + " \n"
						+ "   AND ItemNo        = " + schPopularChannelModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정채널편성 마지막 순위로 변경:[" + schPopularChannelModel.ItemName + "] 등록자:[" + header.UserID + "]");
					schPopularChannelModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPopularChannelModel.ResultCD = "0000";  // 정상
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChannelAdOrderLast() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPopularChannelModel.ResultCD   = "3101";
				schPopularChannelModel.ResultDesc = " 지정채널편성 마지막 순위로 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion 


		#region 현재승인상태의 승인번호를 구함

		/// <summary>
		/// 현재승인상태의 승인번호를 구함
		/// 상태가 30:배포승인이면 신규(상태 10:편성중) 으로 생성후 AckNo 리턴
		/// </summary>
		/// <returns>string</returns>
		private string GetLastAckNo(string MediaCode)
		{

			string AckNo    = "";
			string AckState = "";

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLastAckNo() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode	       :[" + MediaCode     + "]");		// 검색 매체				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ " DECLARE @AckNo int, @AckState Char(2), @MediaCode int    \n"
					+ "                                                          \n"
					+ " SELECT @MediaCode = " + MediaCode                    + " \n"
					+ "                                                          \n"
					+ " SELECT TOP 1 @AckState = State, @AckNo = AckNo           \n"
					+ "   FROM SchPublish                                        \n"
					+ "  WHERE MediaCode = @MediaCode                            \n"
					+ "  ORDER BY AckNo DESC                                     \n"
					+ "                                                          \n"
					+ " IF @AckState = '30' OR @AckState IS NULL                 \n"
					+ " BEGIN                                                    \n"
					+ "	    INSERT INTO SchPublish                               \n"
					+ "                (AckNo, MediaCode, State, ModifyStartDay) \n"
					+ "         SELECT ISNULL(MAX(AckNo),0) + 1                  \n"
					+ "               ,@MediaCode                                \n"
					+ "               ,'10'                                      \n"
					+ "               ,GETDATE()                                 \n"
					+ "          FROM SchPublish                                 \n"
					+ "         WHERE MediaCode = @MediaCode                     \n"
					+ "                                                          \n"
					+ "     SELECT TOP 1 @AckState = State, @AckNo = AckNo       \n"
					+ "       FROM SchPublish                                    \n"
					+ "      WHERE MediaCode = @MediaCode                        \n"
					+ "      ORDER BY AckNo DESC                                 \n"
					+ " END                                                      \n"
					+ "                                                          \n"
					+ " SELECT @AckNo AS AckNo, @AckState AS AckState            \n"                             
					);
	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					AckNo    =  ds.Tables[0].Rows[0]["AckNo"].ToString();
					AckState =  ds.Tables[0].Rows[0]["AckState"].ToString();
				}

				ds.Dispose();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLastAckNo() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}
			return AckNo;
		}

		#endregion

	}
}