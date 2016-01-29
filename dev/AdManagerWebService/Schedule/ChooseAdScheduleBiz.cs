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
    /// ChooseAdScheduleBiz에 대한 요약 설명입니다.
    /// </summary>
    public class ChooseAdScheduleBiz : BaseBiz
    {
        public ChooseAdScheduleBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }

		#region 메뉴 목록조회
		/// <summary>
		/// 메뉴 목록조회
		/// </summary>
		/// <param name="chooseAdScheduleModel"></param>
		/// 
		public void GetMenuList(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
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
				_log.Debug("SearchMediaCode      :[" + chooseAdScheduleModel.SearchMediaCode + "]");
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
					+ "          WHERE MediaCode = '" + chooseAdScheduleModel.SearchMediaCode.Trim() + "' \n"
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
				chooseAdScheduleModel.ChooseAdScheduleDataSet = ds.Copy();
				// 결과
				chooseAdScheduleModel.ResultCnt = Utility.GetDatasetCount(chooseAdScheduleModel.ChooseAdScheduleDataSet);
				// 결과코드 셋트
				chooseAdScheduleModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + chooseAdScheduleModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() End");
				_log.Debug("-----------------------------------------");


			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD = "3000";
				chooseAdScheduleModel.ResultDesc = "메뉴정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
		}

        /// <summary>
        /// 메뉴목록조회, 편성광고건수 포함됨, 메뉴별편성에서 사용됨
        /// </summary>
        /// <param name="header"></param>
        /// <param name="chooseAdScheduleModel"></param>
        public void svcMenuList1(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "vMenuList1() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode      :[" + chooseAdScheduleModel.SearchMediaCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
					
                // 쿼리생성
                sbQuery.Append("\n select   MediaCode, MediaName, CategoryCode, CategoryName, GenreCode,GenreName,AdCount");
                sbQuery.Append("\n from     vMenuList");
                sbQuery.Append("\n order by Sort1,Sort2");
			     
                // __DEBUG__
                //sbQuery.AppendLine(" Select ");
                //sbQuery.AppendLine("   1 as MediaCode ");
                //sbQuery.AppendLine("  ,'' as MediaName ");
                //sbQuery.AppendLine("   ,a.menu_cod as CategoryCode   ");
                //sbQuery.AppendLine("   ,a.menu_nm as CategoryName   ");
                //sbQuery.AppendLine("   ,b.menu_cod as GenreCode     ");
                //sbQuery.AppendLine("   ,b.menu_nm as GenreName      ");
                //sbQuery.AppendLine("   ,0 as adCount                ");                
                //sbQuery.AppendLine(" From MENU_COD a                ");
                //sbQuery.AppendLine(" LEFT JOIN MENU_COD b ON (a.menu_cod = b.menu_cod_par AND b.menu_lvl=3) ");
                //sbQuery.AppendLine(" Where a.menu_lvl =2            ");
                //sbQuery.AppendLine(" Order By a.menu_cod ");
                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();

                _db.Open();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 매체대행사광고주모델에 복사
                chooseAdScheduleModel.ChooseAdScheduleDataSet = ds.Copy();
                chooseAdScheduleModel.ResultCnt = Utility.GetDatasetCount(chooseAdScheduleModel.ChooseAdScheduleDataSet);
                chooseAdScheduleModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + chooseAdScheduleModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetMenuList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                chooseAdScheduleModel.ResultCD = "3000";
                chooseAdScheduleModel.ResultDesc = "메뉴정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

		#endregion

		#region 채널목록조회
		
		/// <summary>
		/// 채널셋목록조회
		/// 메뉴/채널편성-메뉴선택시 불러올 채널리스트데이터
		/// </summary>
		/// <param name="chooseAdScheduleModel"></param>
		public void GetChannelList(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode    :[" + chooseAdScheduleModel.MediaCode    + "]");
				_log.Debug("CategoryCode :[" + chooseAdScheduleModel.CategoryCode + "]");
				_log.Debug("GenreCode    :[" + chooseAdScheduleModel.GenreCode    + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT A.MediaCode        \n"
					+ "       ,E.MediaName	      \n"
					+ "       ,A.CategoryCode	  \n"
					+ "       ,B.CategoryName	  \n"
					+ "       ,A.GenreCode	      \n"
					+ "       ,C.GenreName        \n"
					+ "       ,A.ChannelNo        \n"
					+ "       ,D.TotalSeries      \n"
					+ "       ,D.Title	          \n"
					+ "       ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock)                             \n"
					+ "                        WHERE MediaCode = A.MediaCode AND ChannelNo = A.ChannelNo)            \n" 
					+ "         +                                                                                    \n"  // 채널편성광고수 +  메뉴편성광고 중 상업광고수
					+ "        (SELECT COUNT(*) FROM SchChoiceMenuDetail SM with(NoLock)                             \n"
					+ "                   INNER JOIN ContractItem IT ON (SM.ItemNo = IT.ItemNo AND IT.AdType = '10') \n"  // AdType:10 상업광고
					+ "                        WHERE SM.MediaCode = A.MediaCode AND SM.GenreCode = A.GenreCode)      \n"
					+ "        AS AdCount             \n"
					+ "       ,A.Hits                 \n"
					+ "       ,CASE WHEN ProdTypeCnt > 0 THEN 'PPx' ELSE '' END AS ProdType \n"
 					+ "  FROM (                       \n"
					+ "       SELECT TA.MediaCode     \n"
					+ "             ,TA.CategoryCode  \n"
					+ "             ,TA.GenreCode	  \n"
					+ "             ,TA.ChannelNo     \n"
					+ "             ,MIN(TA.SeriesNo) AS SeriesNo \n"
					+ "             ,SUM(TC.Hits) AS Hits         \n"
					+ "             ,SUM(CASE WHEN ProdType IS NOT NULL AND ProdType <> '' THEN 1 ELSE 0 END) AS ProdTypeCnt \n"
					+ "         FROM ChannelSet TA with(NoLock)   \n"
					+ "              INNER JOIN Channel  TB with(NoLock) ON (TA.MediaCode = TB.MediaCode AND TA.ChannelNo = TB.ChannelNo AND TA.SeriesNO = TB.SeriesNo) \n"
                    + "              INNER JOIN Contents TC with(NoLock) ON (TB.ContentID = TC.ContentID) \n"
					+ "        WHERE TA.MediaCode    = '" + chooseAdScheduleModel.MediaCode    + "' \n"
					+ "          AND TA.CategoryCode = '" + chooseAdScheduleModel.CategoryCode + "' \n"
					+ "          AND TA.GenreCode    = '" + chooseAdScheduleModel.GenreCode    + "' \n"
					+ "        GROUP BY TA.MediaCode, TA.CategoryCode, TA.GenreCode, TA.ChannelNo   \n"
					+ "       ) A INNER JOIN Category B with(NoLock) ON A.MediaCode = B.MediaCode AND A.CategoryCode = B.CategoryCode  \n"
					+ "           INNER JOIN Genre    C with(NoLock) ON A.MediaCode = C.MediaCode AND A.GenreCode    = C.GenreCode     \n"
					+ "           INNER JOIN Channel  D with(NoLock) ON A.MediaCode = D.MediaCode AND A.ChannelNo    = D.ChannelNo AND A.SeriesNO = D.SeriesNo \n"
					+ "           INNER JOIN Media    E with(NoLock) ON A.MediaCode = E.MediaCode \n"
					+ " ORDER BY A.ChannelNo       \n"
					);

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 매체대행사광고주모델에 복사
				chooseAdScheduleModel.ChooseAdScheduleDataSet = ds.Copy();
				// 결과
				chooseAdScheduleModel.ResultCnt = Utility.GetDatasetCount(chooseAdScheduleModel.ChooseAdScheduleDataSet);
				// 결과코드 셋트
				chooseAdScheduleModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + chooseAdScheduleModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD = "3000";
				chooseAdScheduleModel.ResultDesc = "채널정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
		}


        /// <summary>
        /// 신규버젼..위에 꺼진 패치후에 삭제할것
        /// </summary>
        /// <param name="header"></param>
        /// <param name="chooseAdScheduleModel"></param>
        public void GetChannelList_0907a(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "ChooseAdSchedule.GetChannelList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MediaCode    :[" + chooseAdScheduleModel.MediaCode    + "]");
                _log.Debug("CategoryCode :[" + chooseAdScheduleModel.CategoryCode + "]");
                _log.Debug("GenreCode    :[" + chooseAdScheduleModel.GenreCode    + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                #region 오리지날 쿼리...확인필요
                /*
                sbQuery.Append("\n"
                    + " SELECT A.MediaCode        \n"
                    + "       ,E.MediaName	      \n"
                    + "       ,A.CategoryCode	  \n"
                    + "       ,B.CategoryName	  \n"
                    + "       ,A.GenreCode	      \n"
                    + "       ,C.GenreName        \n"
                    + "       ,A.ChannelNo        \n"
                    + "       ,D.TotalSeries      \n"
                    + "       ,D.Title	          \n"
                    + "       ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock)                             \n"
                    + "                        WHERE MediaCode = A.MediaCode AND ChannelNo = A.ChannelNo)            \n" 
//                    + "         +                                                                                    \n"  // 채널편성광고수 +  메뉴편성광고 중 상업광고수
//                    + "        (SELECT COUNT(*) FROM SchChoiceMenuDetail SM with(NoLock)                             \n"
//                    + "                   INNER JOIN ContractItem IT ON (SM.ItemNo = IT.ItemNo AND IT.AdType = '10') \n"  // AdType:10 상업광고
//                    + "                        WHERE SM.MediaCode = A.MediaCode AND SM.GenreCode = A.GenreCode)      \n"
                    + "        AS AdCount             \n"
                    + "       ,A.Hits                 \n"
                    + "       ,CASE WHEN ProdTypeCnt > 0 THEN 'PPx' ELSE '' END AS ProdType \n"
                    + "       ,A.Rate \n"
                    + "  FROM (                       \n"
                    + "       SELECT TA.MediaCode     \n"
                    + "             ,TA.CategoryCode  \n"
                    + "             ,TA.GenreCode	  \n"
                    + "             ,TA.ChannelNo     \n"
                    + "             ,MIN(TA.SeriesNo) AS SeriesNo \n"
                    + "             ,SUM(TC.Hits) AS Hits         \n"
                    + "             ,SUM(CASE WHEN ProdType IS NOT NULL AND ProdType <> '' THEN 1 ELSE 0 END) AS ProdTypeCnt \n"
                    + "             ,MAX(TC.Rate)  as Rate \n"
                    + "         FROM ChannelSet TA with(NoLock)   \n"
                    + "              INNER JOIN Channel  TB with(NoLock) ON (TA.MediaCode = TB.MediaCode AND TA.ChannelNo = TB.ChannelNo AND TA.SeriesNO = TB.SeriesNo) \n"
                    + "              INNER JOIN Contents TC with(NoLock) ON (TB.ContentID = TC.ContentID) \n"
                    + "        WHERE TA.MediaCode    = '" + chooseAdScheduleModel.MediaCode    + "' \n"
                    + "          AND TA.CategoryCode = '" + chooseAdScheduleModel.CategoryCode + "' \n"
                    + "          AND TA.GenreCode    = '" + chooseAdScheduleModel.GenreCode    + "' \n"
                    + "        GROUP BY TA.MediaCode, TA.CategoryCode, TA.GenreCode, TA.ChannelNo   \n"
                    + "       ) A INNER JOIN Category B with(NoLock) ON A.MediaCode = B.MediaCode AND A.CategoryCode = B.CategoryCode  \n"
                    + "           INNER JOIN Genre    C with(NoLock) ON A.MediaCode = C.MediaCode AND A.GenreCode    = C.GenreCode     \n"
                    + "           INNER JOIN Channel  D with(NoLock) ON A.MediaCode = D.MediaCode AND A.ChannelNo    = D.ChannelNo AND A.SeriesNO = D.SeriesNo \n"
                    + "           INNER JOIN Media    E with(NoLock) ON A.MediaCode = E.MediaCode \n"
                    + " ORDER BY A.ChannelNo       \n"
                    );
                */
                #endregion

                
                /*선택한 카테고리,장르정보로 프로그램에 등록된 타이틀들의 최소 타이틀id값(시리즈) 구함
                 * 그 타이틀로 편성된타이틀 테이블에서 광고수를 구함
                 * 기존 쿼리와 유사하게 했으나 매칭되는 테이블이 기존것과 달라서 100% 같지는 않음.검수 필요.
                 * 
                 */
                sbQuery.AppendLine(" SELECT ");
                sbQuery.AppendLine(         chooseAdScheduleModel.MediaCode + " as MediaCode ");
                sbQuery.AppendLine("        ,'' as MediaName   ");
                sbQuery.AppendLine("        ,a.categoryName  ");
                sbQuery.AppendLine("        ,a.categoryCode    ");
                sbQuery.AppendLine("        ,a.genreName       ");
                sbQuery.AppendLine("        ,a.genrecode       ");
                sbQuery.AppendLine("        ,a.rate            ");
                sbQuery.AppendLine("         ,b.title_no        ");
                sbQuery.AppendLine("         ,b.title_nm        ");
                sbQuery.AppendLine("         ,(SELECT COUNT(k.item_no) FROM SCHD_TITLE k   ");
                sbQuery.AppendLine("           WHERE k.title_no = title_no) as AdCount    ");
                sbQuery.AppendLine("  FROM               ");
                sbQuery.AppendLine("     (                  ");
                sbQuery.AppendLine("       SELECT          ");
                sbQuery.AppendLine("            x.menu_nm as CategoryName      ");
                sbQuery.AppendLine("            ,x.menu_cod as CategoryCode    ");
                sbQuery.AppendLine("            ,y.menu_nm as GenreName        ");
                sbQuery.AppendLine("            ,y.menu_cod as GenreCode       ");
                sbQuery.AppendLine("            ,y.ad_rate as Rate             ");
                sbQuery.AppendLine("        FROM MENU_COD x "); ///*카테고리*/      
                sbQuery.AppendLine("        LEFT JOIN MENU_COD y ON(x.menu_cod = y.menu_cod_par AND x.menu_lvl=2 ) "); ///*장르*/
                sbQuery.AppendLine("        WHERE y.menu_lvl = 3                   ");
                sbQuery.AppendLine("        AND  x.menu_cod = '" + chooseAdScheduleModel.CategoryCode + "'");
                sbQuery.AppendLine("        AND  y.menu_cod = '" + chooseAdScheduleModel.GenreCode + "'");                
                sbQuery.AppendLine("       ) a                    ");
                sbQuery.AppendLine("  LEFT JOIN TITLE_COD b ON (a.genreCode = b.menu_cod_org)");
                sbQuery.AppendLine("  ORDER BY b.title_no ");
                
                
    
   
    
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 매체대행사광고주모델에 복사
                chooseAdScheduleModel.ChooseAdScheduleDataSet = ds.Copy();
                // 결과
                chooseAdScheduleModel.ResultCnt = Utility.GetDatasetCount(chooseAdScheduleModel.ChooseAdScheduleDataSet);
                // 결과코드 셋트
                chooseAdScheduleModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + chooseAdScheduleModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                chooseAdScheduleModel.ResultCD = "3000";
                chooseAdScheduleModel.ResultDesc = "채널정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


		/// <summary>
		/// 회차별 목록을 가져온다
		/// </summary>
		/// <param name="header"></param>
		/// <param name="chooseAdScheduleModel"></param>
		public void GetSeriesList_0907a(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "ChooseAdSchedule.GetChannelList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode    :[" + chooseAdScheduleModel.MediaCode    + "]");
				_log.Debug("CategoryCode :[" + chooseAdScheduleModel.CategoryCode + "]");
				_log.Debug("GenreCode    :[" + chooseAdScheduleModel.GenreCode    + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// 쿼리생성
				sbQuery.Append(" select	 a.CategoryCode " + "\n");
				sbQuery.Append("		,a.GenreCode " + "\n");
				sbQuery.Append("		,a.ChannelNo " + "\n");
				sbQuery.Append("		,a.SeriesNo " + "\n");
				sbQuery.Append(" 		,c.SubTitle " + "\n");
				sbQuery.Append(" 		,c.ContentsState as cState " + "\n");
				sbQuery.Append(" 		,c.Rate " + "\n");
				sbQuery.Append(" 		,c.Hits " + "\n");
				sbQuery.Append(" 		,c.AdUse " + "\n");
				sbQuery.Append("        ,(	SELECT COUNT(*) " + "\n");
				sbQuery.Append(" 					FROM SchChoiceSeriesDetail x with(NoLock)" + "\n");
				sbQuery.Append(" 					inner   join ContractItem   y with(noLock)  on x.ItemNo = y.ItemNo and x.MediaCode = y.MediaCode " + "\n");
				sbQuery.Append("           WHERE   x.MediaCode = A.MediaCode" + "\n");
				sbQuery.Append("           AND     x.ChannelNo = A.ChannelNo and x.SeriesNo = a.SeriesNo ) AS AdCount" + "\n");
				sbQuery.Append("        ,0 AdFound   " + "\n");
				sbQuery.Append(" from	ChannelSet	a	with(noLock) " + "\n");
				sbQuery.Append(" inner join Channel b	with(noLock)	on b.MediaCode = a.MediaCode and b.ChannelNo = a.ChannelNo and b.SeriesNo = a.SeriesNo " + "\n");
				sbQuery.Append(" inner join Contents c	with(noLock)	on c.ContentId = b.ContentId and c.ContentsState between '30' and '70' " + "\n");
				sbQuery.Append(" where	a.MediaCode     = " + chooseAdScheduleModel.MediaCode + "\n");
				sbQuery.Append(" and	a.CategoryCode	= " + chooseAdScheduleModel.CategoryCode + "\n");
				sbQuery.Append(" and	a.GenreCode     = " + chooseAdScheduleModel.GenreCode	 + "\n");
				sbQuery.Append(" order by a.ChannelNo,a.SeriesNo desc " + "\n");
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 매체대행사광고주모델에 복사
				chooseAdScheduleModel.ChooseAdScheduleDataSet = ds.Copy();
				// 결과
				chooseAdScheduleModel.ResultCnt = Utility.GetDatasetCount(chooseAdScheduleModel.ChooseAdScheduleDataSet);
				// 결과코드 셋트
				chooseAdScheduleModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + chooseAdScheduleModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD = "3000";
				chooseAdScheduleModel.ResultDesc = "채널정보 조회중 오류가 발생하였습니다";
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
        /// <param name="chooseAdScheduleModel"></param>
        public void GetChooseAdScheduleListMenu(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
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
				_log.Debug("MediaCode    :[" + chooseAdScheduleModel.MediaCode  + "]");
				_log.Debug("GenreCode    :[" + chooseAdScheduleModel.GenreCode + "]");
				// __DEBUG__



                StringBuilder sbQuery = new StringBuilder();
                #region 삭제 할 것
                // 쿼리생성
                /*
                sbQuery.Append("\n"
					+ "SELECT 'M' AS ViewType              \n"
					+ "      ,A.CmType                     \n"
					+ "      ,CASE WHEN CmType = '1' THEN '장르' WHEN CmType='2' Then '채널' ELSE '회차' END AS CmName \n"
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
                    + "         SELECT '1' AS CmType, ItemNo, ScheduleOrder, GenreCode, AckNo \n"
                    + "           FROM SchChoiceMenuDetail with(NoLock)                       \n" 
					+ "          WHERE MediaCode = '" + chooseAdScheduleModel.MediaCode  + "' \n"
					+ "            AND GenreCode = '" + chooseAdScheduleModel.GenreCode  + "' \n"
                    + "       ) AS A    INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo         = B.ItemNo)                     \n"    
                    + "                 INNER JOIN Contract     C with(NoLock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq)  \n"
                    + "                 LEFT  JOIN Advertiser   E with(NoLock) ON (B.AdvertiserCode = E.AdvertiserCode)             \n"
                    + "                 LEFT  JOIN SystemCode   F with(NoLock) ON (C.State          = F.Code AND F.Section = '23')  \n"	// 23 : 계약상태
                    + "                 LEFT  JOIN SystemCode   G with(NoLock) ON (B.AdType         = G.Code AND G.Section = '26')  \n"	// 26 : 광고종류
                    + "                 LEFT  JOIN SystemCode   H with(NoLock) ON (B.AdState        = H.Code AND H.Section = '25')  \n"	// 25 : 광고상태
					+ "                 LEFT  JOIN SystemCode   I with(NoLock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 31 : 파일상태
					+ "                 LEFT  JOIN SchPublish   J with(NoLock) ON (A.AckNo          = J.AckNo)                      \n"
                    + " WHERE 1=1   \n"   
                    );

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                sbQuery.Append(" AND ( B.AdState  = '10' \n");
                isState = true;

                // 광고상태 편성
                if(chooseAdScheduleModel.SearchchkAdState_20.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '20' \n");
                    isState = true;
                }	
                // 광고상태 중지
                if(chooseAdScheduleModel.SearchchkAdState_30.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '30' \n");
                    isState = true;
                }	
                // 광고상태 종료
                if(chooseAdScheduleModel.SearchchkAdState_40.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '40' \n");
                    isState = true;
                }	

                if(isState) sbQuery.Append(" ) \n");

                sbQuery.Append(" ORDER BY CmType DESC, ScheduleOrder, CASE B.AdType WHEN '13' THEN 1 WHEN '10' THEN 2 WHEN '12' THEN 3 WHEN '11' THEN 4 ELSE 9 END");
                */
                #endregion

                sbQuery.AppendLine(" SELECT 'M' AS ViewType                 ");
                sbQuery.AppendLine("        ,A.CmType                       ");
                sbQuery.AppendLine("        ,CASE WHEN CmType = '1' THEN '장르' WHEN CmType='2' Then '채널' ELSE '회차' END AS CmName ");
                sbQuery.AppendLine("        ,B.advt_typ as AdType           ");           
                sbQuery.AppendLine("        ,G.stm_cod_nm as AdTypeName     ");
                sbQuery.AppendLine("        ,A.ScheduleOrder                ");
                sbQuery.AppendLine("        ,A.ItemNo                       "); 
                sbQuery.AppendLine("        ,B.item_nm as ItemName          ");          
                sbQuery.AppendLine("        ,C.cntr_nm as ContractName      ");          
                sbQuery.AppendLine("        ,E.advter_nm as AdvertiserName  ");            
                sbQuery.AppendLine("				,C.cntr_stt  as ContState   ");      
                sbQuery.AppendLine("        ,F.stm_cod_nm as ContStateName      "); 
                sbQuery.AppendLine("        ,B.rl_end_dy as RealEndDay          ");         
                sbQuery.AppendLine("				,B.advt_stt as AdState      ");              
                sbQuery.AppendLine("        ,H.stm_cod_nm as AdStatename        "); 
                sbQuery.AppendLine("        ,c.mda_cod as MediaCode             ");      
                sbQuery.AppendLine("        ,A.GenreCode                        "); 
                sbQuery.AppendLine("        ,'0' AS ChannelNo                   "); 
                sbQuery.AppendLine("        ,B.file_stt as FileState            ");      
                sbQuery.AppendLine("        ,I.stm_cod_nm as FileStatename      "); 
                sbQuery.AppendLine("        ,J.ack_stt as AckState              "); 
                sbQuery.AppendLine("        ,'False' as CheckYn                 "); 
                sbQuery.AppendLine(" FROM (                                     ");
                sbQuery.AppendLine("                    SELECT '1' AS CmType    ");
                sbQuery.AppendLine("                            ,ITEM_NO AS ItemNo  ");
                sbQuery.AppendLine("                            ,SCHD_ORD AS ScheduleOrder  ");
                sbQuery.AppendLine("                            ,MENU_COD AS GenreCode      ");
                sbQuery.AppendLine("                            ,ACK_NO as AckNo    ");
                sbQuery.AppendLine("                    FROM SCHD_MENU              ");                                       
                sbQuery.AppendLine("                    WHERE    MENU_COD = '" + chooseAdScheduleModel.GenreCode + "'");
                sbQuery.AppendLine("            )  A                            ");
                sbQuery.AppendLine("    INNER JOIN ADVT_MST B  ON (A.ItemNo     = B.Item_No)    ");
                sbQuery.AppendLine("    INNER JOIN CNTR     C  ON (B.cntr_seq = C.cntr_seq)     ");
                sbQuery.AppendLine("    LEFT  JOIN ADVTER   E  ON (C.advter_cod = E.advter_cod) ");            
                sbQuery.AppendLine("    LEFT  JOIN STM_COD  F  ON (C.cntr_stt      = F.stm_cod  AND F.stm_cod_cls = '23') "); 	/* 계약상태*/
                sbQuery.AppendLine("    LEFT  JOIN STM_COD  G ON (B.advt_typ      = G.stm_cod AND G.stm_cod_cls = '26')   ");  /* 26 : 광고종류*/
                sbQuery.AppendLine("    LEFT  JOIN STM_COD  H ON (B.advt_stt     = H.stm_cod AND H.stm_cod_cls = '25')    ");	/* 25 : 광고상태*/
                sbQuery.AppendLine("	LEFT  JOIN STM_COD  I  ON (B.file_stt     = I.stm_cod AND I.stm_cod_cls = '31')   ");	/* 31 : 파일상태*/
                sbQuery.AppendLine("    LEFT  JOIN SCHD_DIST_MST   J ON (A.ackno  = J.ack_no)                             ");
                sbQuery.AppendLine("WHERE 1=1           ");

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                sbQuery.Append(" AND ( B.advt_stt  = '10' \n");
                isState = true;

                // 광고상태 편성
                if(chooseAdScheduleModel.SearchchkAdState_20.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.advt_stt  = '20' \n");
                    isState = true;
                }	
                // 광고상태 중지
                if(chooseAdScheduleModel.SearchchkAdState_30.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.advt_stt  = '30' \n");
                    isState = true;
                }	
                // 광고상태 종료
                if(chooseAdScheduleModel.SearchchkAdState_40.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.advt_stt  = '40' \n");
                    isState = true;
                }	

                if(isState) sbQuery.Append(" ) \n");
                sbQuery.AppendLine(" ORDER BY CmType DESC, ScheduleOrder    ");
                sbQuery.AppendLine(" ,CASE B.advt_typ WHEN '13' THEN 1 WHEN '10' THEN 2 WHEN '12' THEN 3 WHEN '11' THEN 4 ELSE 9 END ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                chooseAdScheduleModel.ChooseAdScheduleDataSet = ds.Copy();

				ds.Dispose();

				//지정메뉴편성의 마지막  Order를 구함
				string LastOrder = "1";
				sbQuery = new StringBuilder();
				sbQuery.Append( "\n"
					+ "  SELECT NVL(MAX(schd_ord),0) AS MaxOrder \n"
					+ "  FROM SCHD_MENU                           \n"					
					+ "  WHERE menu_cod  = '" + chooseAdScheduleModel.GenreCode + "' \n"
					);

				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					LastOrder = Utility.GetDatasetString(ds, 0, "MaxOrder");					
				}
				chooseAdScheduleModel.LastOrder = LastOrder;
				ds.Dispose();

                // 결과
                chooseAdScheduleModel.ResultCnt = Utility.GetDatasetCount(chooseAdScheduleModel.ChooseAdScheduleDataSet);
                // 결과코드 셋트
                chooseAdScheduleModel.ResultCD = "0000";

				

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + chooseAdScheduleModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChooseAdScheduleListMenu() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                chooseAdScheduleModel.ResultCD = "3000";
                chooseAdScheduleModel.ResultDesc = "메뉴별 편성현황 조회중 오류가 발생하였습니다";
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
		/// <param name="chooseAdScheduleModel"></param>
		public void GetChooseAdScheduleListChannel(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
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
				_log.Debug("MediaCode    :[" + chooseAdScheduleModel.MediaCode + "]");
				_log.Debug("GenreCode    :[" + chooseAdScheduleModel.GenreCode + "]");
				_log.Debug("ChannelNo    :[" + chooseAdScheduleModel.ChannelNo + "]");
				// __DEBUG__


				StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
					+ "SELECT 'C' AS ViewType              \n"
					+ "      ,A.CmType                     \n"
					+ "      ,CASE WHEN CmType = '1' THEN '장르' WHEN CmType='2' Then '채널' ELSE '회차' END AS CmName \n"
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
                    + "         SELECT '1' AS CmType, SM.ItemNo, SM.ScheduleOrder, SM.GenreCode, '' AS ChannelNo, AckNo  \n"               
                    + "           FROM SchChoiceMenuDetail SM with(NoLock) INNER JOIN ContractItem IT with(NoLock) ON (SM.ItemNo = IT.ItemNo AND IT.AdType BETWEEN '10' AND '19') \n"  // AdType:광고종류 10:상업광고
					+ "          WHERE SM.MediaCode = '" + chooseAdScheduleModel.MediaCode  + "' \n"
					+ "            AND SM.GenreCode = '" + chooseAdScheduleModel.GenreCode  + "' \n"
					+ "         UNION                                                         \n"
					+ "         SELECT '2' AS CmType, SC.ItemNo, SC.ScheduleOrder,            \n"
				    + "                '" + chooseAdScheduleModel.GenreCode  + "' AS GenreCode, SC.ChannelNo, AckNo \n"  
					+ "           FROM SchChoiceChannelDetail SC with(NoLock) \n"
					+ "          WHERE SC.MediaCode = '" + chooseAdScheduleModel.MediaCode + "' \n"
					+ "            AND SC.ChannelNo = '" + chooseAdScheduleModel.ChannelNo + "' \n"
                    + "       ) AS A    INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo         = B.ItemNo)                      \n"    
                    + "                 INNER JOIN Contract     C with(NoLock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq)  \n"
                    + "                 LEFT  JOIN Advertiser   E with(NoLock) ON (B.AdvertiserCode = E.AdvertiserCode)      \n"
                    + "                 LEFT  JOIN SystemCode   F with(NoLock) ON (C.State          = F.Code AND F.Section = '23')  \n"	// 23 : 계약상태
                    + "                 LEFT  JOIN SystemCode   G with(NoLock) ON (B.AdType         = G.Code AND G.Section = '26')  \n"	// 26 : 광고종류
                    + "                 LEFT  JOIN SystemCode   H with(NoLock) ON (B.AdState        = H.Code AND H.Section = '25')  \n"	// 25 : 광고상태
					+ "                 LEFT  JOIN SystemCode   I with(NoLock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 31 : 파일상태
					+ "                 LEFT  JOIN SchPublish   J with(NoLock) ON (A.AckNo          = J.AckNo)                      \n"
   					+ " WHERE 1=1   \n"   
					);

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                sbQuery.Append(" AND ( B.AdState  = '10' \n");
                isState = true;

                // 광고상태 편성
                if(chooseAdScheduleModel.SearchchkAdState_20.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '20' \n");
                    isState = true;
                }	
                // 광고상태 중지
                if(chooseAdScheduleModel.SearchchkAdState_30.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '30' \n");
                    isState = true;
                }	
                // 광고상태 종료
                if(chooseAdScheduleModel.SearchchkAdState_40.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '40' \n");
                    isState = true;
                }	

                if(isState) sbQuery.Append(" ) \n");

                sbQuery.Append(" ORDER BY CmType DESC, AdType, ScheduleOrder");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				chooseAdScheduleModel.ChooseAdScheduleDataSet = ds.Copy();

				ds.Dispose();

				//지정메뉴편성의 마지막  Order를 구함
				string LastOrder = "1";
				sbQuery = new StringBuilder();
				sbQuery.Append( "\n"
					+ "SELECT ISNULL(MAX(ScheduleOrder),0) AS MaxOrder                \n"
					+ "  FROM SchChoiceChannelDetail with(NoLock)                     \n"
					+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
					+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo + " \n"
					);

				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					LastOrder = Utility.GetDatasetString(ds, 0, "MaxOrder");					
				}
				chooseAdScheduleModel.LastOrder = LastOrder;
				ds.Dispose();


				// 결과
				chooseAdScheduleModel.ResultCnt = Utility.GetDatasetCount(chooseAdScheduleModel.ChooseAdScheduleDataSet);
				// 결과코드 셋트
				chooseAdScheduleModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + chooseAdScheduleModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChooseAdScheduleListChannel() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD = "3000";
				chooseAdScheduleModel.ResultDesc = "채널별 편성현황 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }


		}

        #endregion

		#region 회차별 편성현황 조회
		/// <summary>
		/// 회차별 편성현황 조회
		/// </summary>
		/// <param name="header"></param>
		/// <param name="chooseAdScheduleModel"></param>
		public void GetChooseAdScheduleListSeries(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			try
			{
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChooseAdScheduleListSeries() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode   :[" + chooseAdScheduleModel.MediaCode	+ "]");
				_log.Debug("GenreCode   :[" + chooseAdScheduleModel.GenreCode	+ "]");
				_log.Debug("ChannelNo   :[" + chooseAdScheduleModel.ChannelNo	+ "]");
				_log.Debug("SeriesNo	:[" + chooseAdScheduleModel.SeriesNo	+ "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT 'C' AS ViewType              \n"
					+ "      ,A.CmType                     \n"
					+ "      ,CASE WHEN CmType = '1' THEN '장르' WHEN CmType='2' Then '채널' ELSE '회차' END AS CmName \n"
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
					+ "         SELECT '1' AS CmType, SM.ItemNo, SM.ScheduleOrder, SM.GenreCode, '' AS ChannelNo, AckNo  \n"               
					+ "           FROM SchChoiceMenuDetail SM with(NoLock) INNER JOIN ContractItem IT with(NoLock) ON (SM.ItemNo = IT.ItemNo AND IT.AdType BETWEEN '10' AND '19') \n"  // AdType:광고종류 10:상업광고
					+ "          WHERE SM.MediaCode = '" + chooseAdScheduleModel.MediaCode  + "' \n"
					+ "            AND SM.GenreCode = '" + chooseAdScheduleModel.GenreCode  + "' \n"
					+ "         UNION                                                         \n"
					+ "         SELECT '2' AS CmType, SC.ItemNo, SC.ScheduleOrder,            \n"
					+ "                '" + chooseAdScheduleModel.GenreCode  + "' AS GenreCode, SC.ChannelNo, AckNo \n"  
					+ "           FROM SchChoiceChannelDetail SC with(NoLock) \n"
					+ "          WHERE SC.MediaCode = '" + chooseAdScheduleModel.MediaCode + "' \n"
					+ "            AND SC.ChannelNo = '" + chooseAdScheduleModel.ChannelNo + "' \n"
					+ "         UNION	\n"	
					+ "         SELECT '3' AS CmType, SS.ItemNo, SS.ScheduleOrder,            \n"
					+ "                '" + chooseAdScheduleModel.GenreCode  + "' AS GenreCode, SS.ChannelNo, AckNo \n"  
					+ "           FROM SchChoiceSeriesDetail SS with(NoLock) \n"
					+ "          WHERE SS.MediaCode = '" + chooseAdScheduleModel.MediaCode	+ "' \n"
					+ "            AND SS.ChannelNo = '" + chooseAdScheduleModel.ChannelNo	+ "' \n"
					+ "            AND SS.SeriesNo  = '" + chooseAdScheduleModel.SeriesNo	+ "' \n"
					+ "       ) AS A    INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo         = B.ItemNo)                      \n"    
					+ "                 INNER JOIN Contract     C with(NoLock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq)  \n"
					+ "                 LEFT  JOIN Advertiser   E with(NoLock) ON (B.AdvertiserCode = E.AdvertiserCode)      \n"
					+ "                 LEFT  JOIN SystemCode   F with(NoLock) ON (C.State          = F.Code AND F.Section = '23')  \n"	// 23 : 계약상태
					+ "                 LEFT  JOIN SystemCode   G with(NoLock) ON (B.AdType         = G.Code AND G.Section = '26')  \n"	// 26 : 광고종류
					+ "                 LEFT  JOIN SystemCode   H with(NoLock) ON (B.AdState        = H.Code AND H.Section = '25')  \n"	// 25 : 광고상태
					+ "                 LEFT  JOIN SystemCode   I with(NoLock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 31 : 파일상태
					+ "                 LEFT  JOIN SchPublish   J with(NoLock) ON (A.AckNo          = J.AckNo)                      \n"
					+ " WHERE 1=1   \n"   
					);

				bool isState = false;
				// 광고상태 선택에 따라

				// 광고상태 준비
				sbQuery.Append(" AND ( B.AdState  = '10' \n");
				isState = true;

				// 광고상태 편성
				if(chooseAdScheduleModel.SearchchkAdState_20.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" B.AdState  = '20' \n");
					isState = true;
				}	
				// 광고상태 중지
				if(chooseAdScheduleModel.SearchchkAdState_30.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" B.AdState  = '30' \n");
					isState = true;
				}	
				// 광고상태 종료
				if(chooseAdScheduleModel.SearchchkAdState_40.Trim().Length > 0 && chooseAdScheduleModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" B.AdState  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				sbQuery.Append(" ORDER BY CmType DESC, AdType, ScheduleOrder");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				chooseAdScheduleModel.ChooseAdScheduleDataSet = ds.Copy();

				ds.Dispose();

				//지정메뉴편성의 마지막  Order를 구함
				string LastOrder = "1";
				sbQuery = new StringBuilder();
				sbQuery.Append( "\n"
					+ "SELECT ISNULL(MAX(ScheduleOrder),0) AS MaxOrder                \n"
					+ "  FROM SchChoiceChannelDetail with(NoLock)                     \n"
					+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
					+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo + " \n"
					);

				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					LastOrder = Utility.GetDatasetString(ds, 0, "MaxOrder");					
				}
				chooseAdScheduleModel.LastOrder = LastOrder;
				ds.Dispose();


				// 결과
				chooseAdScheduleModel.ResultCnt = Utility.GetDatasetCount(chooseAdScheduleModel.ChooseAdScheduleDataSet);
				// 결과코드 셋트
				chooseAdScheduleModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + chooseAdScheduleModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChooseAdScheduleListChannel() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD = "3000";
				chooseAdScheduleModel.ResultDesc = "채널별 편성현황 조회중 오류가 발생하였습니다";
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
		public void SetSchMenuAdOrderFirst(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
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
					string AckNo = GetLastAckNo(chooseAdScheduleModel.MediaCode);


					// 변경할 순위
					string ToOrder = "1"; 

					// 해당 매체중 MIN값
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS MinOrder   \n"
						+ "  FROM SchChoiceMenuDetail                                     \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode + " \n"
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
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder < " + chooseAdScheduleModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                                           \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                                     \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode + " \n"
						+ "   AND ItemNo        = " + chooseAdScheduleModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정메뉴편성 첫번째 순위로 변경:[" + chooseAdScheduleModel.ItemName + "] 등록자:[" + header.UserID + "]");

					chooseAdScheduleModel.ScheduleOrder = ToOrder;  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
					_log.Debug("ScheduleOrder:[" + chooseAdScheduleModel.ScheduleOrder + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				chooseAdScheduleModel.ResultCD = "0000";  // 정상
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderFirst() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD   = "3101";
				chooseAdScheduleModel.ResultDesc = " 지정메뉴편성  첫번째 순위로 변경 중 오류가 발생하였습니다";
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
		public void SetSchMenuAdOrderUp(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
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
					string AckNo = GetLastAckNo(chooseAdScheduleModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS UpOrder                    \n"
						+ "  FROM SchChoiceMenuDetail                                         \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder < " + chooseAdScheduleModel.ScheduleOrder + " \n"
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
						+ "   SET ScheduleOrder = " + chooseAdScheduleModel.ScheduleOrder + " \n"
						+ "      ,AckNo         = " + AckNo                               + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder = " + ToOrder                             + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                                     \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode + " \n"
						+ "   AND ItemNo        = " + chooseAdScheduleModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정메뉴편성 순위올림 변경:[" + chooseAdScheduleModel.ItemName + "] 등록자:[" + header.UserID + "]");
					chooseAdScheduleModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				chooseAdScheduleModel.ResultCD = "0000";  // 정상
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchHomeAdOrderUp() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD   = "3101";
				chooseAdScheduleModel.ResultDesc = " 지정메뉴편성 순위올림 변경 중 오류가 발생하였습니다";
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
		public void SetSchMenuAdOrderDown(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
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
					string AckNo = GetLastAckNo(chooseAdScheduleModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS DownOrder                  \n"
						+ "  FROM SchChoiceMenuDetail                                         \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder > " + chooseAdScheduleModel.ScheduleOrder + " \n"
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
						+ "   SET ScheduleOrder = " + chooseAdScheduleModel.ScheduleOrder + " \n"
						+ "      ,AckNo         = " + AckNo                               + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder = " + ToOrder                             + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                                     \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode + " \n"
						+ "   AND ItemNo        = " + chooseAdScheduleModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정메뉴편성 순위올림 변경:[" + chooseAdScheduleModel.ItemName + "] 등록자:[" + header.UserID + "]");
					chooseAdScheduleModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				chooseAdScheduleModel.ResultCD = "0000";  // 정상
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderDown() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD   = "3101";
				chooseAdScheduleModel.ResultDesc = " 지정메뉴편성 순위내림 변경 중 오류가 발생하였습니다";
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
		public void SetSchMenuAdOrderLast(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
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
					string AckNo = GetLastAckNo(chooseAdScheduleModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder              \n"
						+ "  FROM SchChoiceMenuDetail                                     \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode + " \n"
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
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode     + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode     + " \n"
						+ "   AND ScheduleOrder > " + chooseAdScheduleModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                                       \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceMenuDetail                                     \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND GenreCode     = " + chooseAdScheduleModel.GenreCode + " \n"
						+ "   AND ItemNo        = " + chooseAdScheduleModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정메뉴편성 마지막 순위로 변경:[" + chooseAdScheduleModel.ItemName + "] 등록자:[" + header.UserID + "]");
					chooseAdScheduleModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
					_log.Debug("ScheduleOrder:[" + chooseAdScheduleModel.ScheduleOrder + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				chooseAdScheduleModel.ResultCD = "0000";  // 정상
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderLast() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD   = "3101";
				chooseAdScheduleModel.ResultDesc = " 지정메뉴편성 마지막 순위로 변경 중 오류가 발생하였습니다";
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
		public void SetSchChannelAdOrderFirst(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChannelAdOrderFirst() Start");
				_log.Debug("-----------------------------------------");

				_log.Debug("MediaCode      ["+ chooseAdScheduleModel.MediaCode +"]");
				_log.Debug("ChannelNo      ["+ chooseAdScheduleModel.ChannelNo +"]");
				_log.Debug("ScheduleOrder  ["+ chooseAdScheduleModel.ScheduleOrder +"]");
				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(chooseAdScheduleModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1"; 

					// 해당 매체중 MIN값
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS MinOrder   \n"
						+ "  FROM SchChoiceChannelDetail                                  \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo + " \n"
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
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo + " \n"
						+ "   AND ScheduleOrder < " + chooseAdScheduleModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                  \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail                                  \n"
						+ "   SET ScheduleOrder = " + ToOrder.ToString()              + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo + " \n"
						+ "   AND ItemNo        = " + chooseAdScheduleModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정채널편성 첫번째 순위로 변경:[" + chooseAdScheduleModel.ItemName + "] 등록자:[" + header.UserID + "]");

					chooseAdScheduleModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				chooseAdScheduleModel.ResultCD = "0000";  // 정상
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChannelAdOrderFirst() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD   = "3101";
				chooseAdScheduleModel.ResultDesc = " 지정채널편성  첫번째 순위로 변경 중 오류가 발생하였습니다";
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
		public void SetSchChannelAdOrderUp(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchMenuAdOrderUp() Start");
				_log.Debug("-----------------------------------------");

				_log.Debug("MediaCode      ["+ chooseAdScheduleModel.MediaCode +"]");
				_log.Debug("ChannelNo      ["+ chooseAdScheduleModel.ChannelNo +"]");
				_log.Debug("ScheduleOrder  ["+ chooseAdScheduleModel.ScheduleOrder +"]");
				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(chooseAdScheduleModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS UpOrder                    \n"
						+ "  FROM SchChoiceChannelDetail                                      \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode     + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo     + " \n"
						+ "   AND ScheduleOrder < " + chooseAdScheduleModel.ScheduleOrder + " \n"
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
						+ "   SET ScheduleOrder = " + chooseAdScheduleModel.ScheduleOrder + " \n"
						+ "      ,AckNo         = " + AckNo                               + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode     + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo     + " \n"
						+ "   AND ScheduleOrder = " + ToOrder                             + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail                                  \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo + " \n"
						+ "   AND ItemNo        = " + chooseAdScheduleModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정채널편성 순위올림 변경:[" + chooseAdScheduleModel.ItemName + "] 등록자:[" + header.UserID + "]");
					chooseAdScheduleModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				chooseAdScheduleModel.ResultCD = "0000";  // 정상
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChannelAdOrderUp() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD   = "3101";
				chooseAdScheduleModel.ResultDesc = " 지정채널편성 순위올림 변경 중 오류가 발생하였습니다";
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
		public void SetSchChannelAdOrderDown(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
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
					string AckNo = GetLastAckNo(chooseAdScheduleModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS DownOrder                  \n"
						+ "  FROM SchChoiceChannelDetail                                      \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode     + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo     + " \n"
						+ "   AND ScheduleOrder > " + chooseAdScheduleModel.ScheduleOrder + " \n"
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
						+ "   SET ScheduleOrder = " + chooseAdScheduleModel.ScheduleOrder + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode     + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo     + " \n"
						+ "   AND ScheduleOrder = " + ToOrder                             + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail                     \n"
						+ "   SET ScheduleOrder = " + ToOrder.ToString() + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo + " \n"
						+ "   AND ItemNo        = " + chooseAdScheduleModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정채널편성 순위올림 변경:[" + chooseAdScheduleModel.ItemName + "] 등록자:[" + header.UserID + "]");
					chooseAdScheduleModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				chooseAdScheduleModel.ResultCD = "0000";  // 정상
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChannelAdOrderDown() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD   = "3101";
				chooseAdScheduleModel.ResultDesc = " 지정채널편성 순위내림 변경 중 오류가 발생하였습니다";
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
		public void SetSchChannelAdOrderLast(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
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
					string AckNo = GetLastAckNo(chooseAdScheduleModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					//지정채널편성 마지막  Order를 구함
					sbQuery.Append( "\n"
						+ "SELECT ISNULL(MAX(ScheduleOrder),1) AS MaxOrder                \n"
						+ "  FROM SchChoiceChannelDetail                                  \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo + " \n"
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
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode     + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo     + " \n"
						+ "   AND ScheduleOrder > " + chooseAdScheduleModel.ScheduleOrder + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchChoiceChannelDetail                                  \n"
						+ "   SET ScheduleOrder = " + ToOrder                         + " \n"
						+ "      ,AckNo         = " + AckNo                           + " \n"
						+ " WHERE MediaCode     = " + chooseAdScheduleModel.MediaCode + " \n"
						+ "   AND ChannelNo     = " + chooseAdScheduleModel.ChannelNo + " \n"
						+ "   AND ItemNo        = " + chooseAdScheduleModel.ItemNo    + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("지정채널편성 마지막 순위로 변경:[" + chooseAdScheduleModel.ItemName + "] 등록자:[" + header.UserID + "]");
					chooseAdScheduleModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				chooseAdScheduleModel.ResultCD = "0000";  // 정상
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchChannelAdOrderLast() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				chooseAdScheduleModel.ResultCD   = "3101";
				chooseAdScheduleModel.ResultDesc = " 지정채널편성 마지막 순위로 변경 중 오류가 발생하였습니다";
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