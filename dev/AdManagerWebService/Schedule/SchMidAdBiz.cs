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
    public class SchMidAdBiz : BaseBiz
    {
        public SchMidAdBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        #region [1] 메뉴 목록조회
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
                sbQuery.Append(" SELECT  A.MediaCode " + "\n");
                sbQuery.Append("        ,B.MediaName " + "\n");
                sbQuery.Append("        ,A.CategoryCode " + "\n");
                sbQuery.Append("        ,dbo.ufnPadding('L',A.CategoryCode,'5',' ') + ' ' + C.CategoryName AS CategoryName " + "\n");
                sbQuery.Append("        ,A.GenreCode " + "\n");
                sbQuery.Append(" 		,dbo.ufnPadding('L',A.GenreCode,'5',' ')+ ' ' + D.GenreName AS GenreName          " + "\n");
                sbQuery.Append("        ,(  SELECT  COUNT(*) " + "\n"); 
                sbQuery.Append("            FROM    SchChoiceMenuDetail	x with(noLock) " + "\n");
                sbQuery.Append(" 			inner   join ContractItem	y with(noLock)	on x.ItemNo = y.ItemNo and x.MediaCode = y.MediaCode and y.AdType='12' " + "\n");
                sbQuery.Append(" 			WHERE   x.MediaCode = A.MediaCode  " + "\n");
                sbQuery.Append(" 			AND     x.GenreCode = A.GenreCode) AS AdCount  " + "\n");
                sbQuery.Append(" FROM (	SELECT a.MediaCode " + "\n");
                sbQuery.Append(" 							,a.CategoryCode " + "\n");
                sbQuery.Append(" 							,a.GenreCode " + "\n");
                sbQuery.Append(" 				FROM ChannelSet a with(NoLock) " + "\n");
                sbQuery.Append(" 				inner join AdContents b with(noLock)  " + "\n");
                sbQuery.Append(" 								on	a.CategoryCode = b.MenuLevel1 " + "\n");
                sbQuery.Append(" 								and	b.MediaCode=0  " + "\n");
                sbQuery.Append(" 								and	b.AdSlotCode='M'  " + "\n");
                sbQuery.Append(" 								and b.UseYn = 'Y'  " + "\n");
                sbQuery.Append(" 								and b.MenuLevel2 = 0 " + "\n");
                sbQuery.Append(" 				WHERE a.MediaCode = '" + chooseAdScheduleModel.SearchMediaCode.Trim() + "' \n");
                sbQuery.Append(" 				GROUP BY a.MediaCode, a.CategoryCode, a.GenreCode " + "\n");
                sbQuery.Append(" 				Union all " + "\n");
                sbQuery.Append(" 				SELECT a.MediaCode " + "\n");
                sbQuery.Append(" 							,a.CategoryCode " + "\n");
                sbQuery.Append(" 							,a.GenreCode " + "\n");
                sbQuery.Append(" 				FROM ChannelSet a with(NoLock) " + "\n");
                sbQuery.Append(" 				inner join AdContents b with(noLock)  " + "\n");
                sbQuery.Append(" 								on	a.CategoryCode = b.MenuLevel1 " + "\n");
                sbQuery.Append(" 								and	a.GenreCode			= b.MenuLevel2 " + "\n");
                sbQuery.Append(" 								and	b.MediaCode=0  " + "\n");
                sbQuery.Append(" 								and	b.AdSlotCode='M'  " + "\n");
                sbQuery.Append(" 								and b.UseYn = 'Y'  " + "\n");
                sbQuery.Append(" 								and b.MenuLevel2 > 0 " + "\n");
                sbQuery.Append(" 				WHERE a.MediaCode = '" + chooseAdScheduleModel.SearchMediaCode.Trim() + "' \n");
                sbQuery.Append(" 				GROUP BY a.MediaCode, a.CategoryCode, a.GenreCode  " + "\n");
                sbQuery.Append(" 			) A  " + "\n");
                sbQuery.Append(" 			INNER JOIN Media    B with(NoLock) ON (A.MediaCode    = B.MediaCode   )    " + "\n");
                sbQuery.Append(" 			INNER JOIN Category C with(NoLock) ON (A.CategoryCode = C.CategoryCode)  " + "\n");
                sbQuery.Append("       INNER JOIN Genre    D with(NoLock) ON (A.GenreCode    = D.GenreCode   )  " + "\n");
                sbQuery.Append(" ORDER BY C.CategoryCode,D.GenreCode " + "\n");
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

        #endregion

        #region [2] 채널목록조회
		
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
			
                sbQuery.Append(" SELECT A.MediaCode        " + "\n");
                sbQuery.Append("        ,E.MediaName	      " + "\n");
                sbQuery.Append("        ,A.CategoryCode	  " + "\n");
                sbQuery.Append("        ,B.CategoryName	  " + "\n");
                sbQuery.Append("        ,A.GenreCode	      " + "\n");
                sbQuery.Append("        ,C.GenreName        " + "\n");
                sbQuery.Append("        ,A.ChannelNo        " + "\n");
                sbQuery.Append("        ,A.TotalSeries      " + "\n");
                sbQuery.Append("        ,D.Title	          " + "\n");
                sbQuery.Append("        ,(	SELECT COUNT(*) " + "\n");
                sbQuery.Append(" 					FROM SchChoiceChannelDetail x with(NoLock)" + "\n");
                sbQuery.Append(" 					inner   join ContractItem   y with(noLock)  on x.ItemNo = y.ItemNo and x.MediaCode = y.MediaCode and y.AdType='17'" + "\n");
                sbQuery.Append("           WHERE   x.MediaCode = A.MediaCode" + "\n");
                sbQuery.Append("           AND     x.ChannelNo = A.ChannelNo) AS AdCount" + "\n");
                sbQuery.Append("        ,A.Hits                 " + "\n");
                sbQuery.Append("        ,CASE WHEN ProdTypeCnt > 0 THEN 'PPx' ELSE '' END AS ProdType " + "\n");
                sbQuery.Append("        ,A.Rate " + "\n");
                sbQuery.Append(" 			 ,0 as MidAdSet -- 중간광고정보데이터 테이블 생성" + "\n");
                sbQuery.Append("   FROM (" + "\n");
                sbQuery.Append("        SELECT TA.MediaCode" + "\n");
                sbQuery.Append("              ,TA.CategoryCode" + "\n");
                sbQuery.Append("              ,TA.GenreCode" + "\n");
                sbQuery.Append("              ,TA.ChannelNo" + "\n");
                sbQuery.Append("              ,MIN(TA.SeriesNo) AS SeriesNo" + "\n");
                sbQuery.Append("              ,SUM(TC.Hits) AS Hits" + "\n");
                sbQuery.Append("              ,SUM(CASE WHEN ProdType IS NOT NULL AND ProdType <> '' THEN 1 ELSE 0 END) AS ProdTypeCnt" + "\n");
                sbQuery.Append("              ,MAX(TC.Rate)  as Rate" + "\n");
                sbQuery.Append("              ,Count(*)  as TotalSeries" + "\n");
                sbQuery.Append("          FROM ChannelSet TA with(NoLock)" + "\n");
                sbQuery.Append("               INNER JOIN Channel  TB with(NoLock) ON (TA.MediaCode = TB.MediaCode AND TA.ChannelNo = TB.ChannelNo AND TA.SeriesNO = TB.SeriesNo)" + "\n");
                sbQuery.Append("               INNER JOIN Contents TC with(NoLock) ON (TB.ContentID = TC.ContentID AND TC.ContentsState <='70' )" + "\n");
                sbQuery.Append("         WHERE TA.MediaCode    = '" + chooseAdScheduleModel.MediaCode+ "'" + "\n");
                sbQuery.Append("           AND TA.CategoryCode = '" + chooseAdScheduleModel.CategoryCode+ "'" + "\n");
                sbQuery.Append("           AND TA.GenreCode    = '" + chooseAdScheduleModel.GenreCode   + "'" + "\n");
                sbQuery.Append("         GROUP BY TA.MediaCode, TA.CategoryCode, TA.GenreCode, TA.ChannelNo" + "\n");
                sbQuery.Append("        ) A INNER JOIN Category B with(NoLock) ON A.MediaCode = B.MediaCode AND A.CategoryCode = B.CategoryCode" + "\n");
                sbQuery.Append("            INNER JOIN Genre    C with(NoLock) ON A.MediaCode = C.MediaCode AND A.GenreCode    = C.GenreCode" + "\n");
                sbQuery.Append("            INNER JOIN Channel  D with(NoLock) ON A.MediaCode = D.MediaCode AND A.ChannelNo    = D.ChannelNo AND A.SeriesNO = D.SeriesNo" + "\n");
                sbQuery.Append("            INNER JOIN Media    E with(NoLock) ON A.MediaCode = E.MediaCode" + "\n");
                sbQuery.Append("  ORDER BY A.ChannelNo;" + "\n");

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
			
        #region [3] 채널별 편성현황 조회
        /// <summary>
        /// 채널별 편성현황 조회
        /// </summary>
        /// <param name="chooseAdScheduleModel"></param>
        public void GetScheduleListChannel(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetScheduleListChannel() Start");
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
                    + "      ,CASE WHEN CmType = 'M' THEN '메뉴' ELSE '채널' END AS CmName \n"
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
                    + "           FROM SchChoiceMenuDetail SM with(NoLock) INNER JOIN ContractItem IT with(NoLock) ON (SM.ItemNo = IT.ItemNo AND IT.AdType = '17') \n"  // 중간광고만
                    + "          WHERE SM.MediaCode = '" + chooseAdScheduleModel.MediaCode  + "' \n"
                    + "            AND SM.GenreCode = '" + chooseAdScheduleModel.GenreCode  + "' \n"
                    + "         UNION                                                         \n"
                    + "         SELECT 'C' AS CmType, SC.ItemNo, SC.ScheduleOrder,            \n"
                    + "                '" + chooseAdScheduleModel.GenreCode  + "' AS GenreCode, SC.ChannelNo, AckNo \n"  
                    + "           FROM SchChoiceChannelDetail SC with(NoLock) \n"
                    + "           INNER JOIN ContractItem IT with(NoLock) ON (SC.ItemNo = IT.ItemNo AND IT.AdType = '17') \n"  // 중간광고만
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
                _log.Debug(this.ToString() + "GetScheduleListChannel() End");
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

        #region [4] 회차별 중간광고현황 조회
        /// <summary>
        /// 시리즈별 중간광고정보리스트
        /// </summary>
        /// <param name="header"></param>
        /// <param name="chooseAdScheduleModel"></param>
        public void GetMidAdInfoListSeries(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetMidAdInfoListSeries() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug(" 매체     :[" + chooseAdScheduleModel.MediaCode + "]");
                _log.Debug(" 카테고리 :[" + chooseAdScheduleModel.CategoryCode + "]");
                _log.Debug(" 장르     :[" + chooseAdScheduleModel.GenreCode + "]");
                _log.Debug(" 채널     :[" + chooseAdScheduleModel.ChannelNo + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(" select  x.SeriesNo " + "\n");
                sbQuery.Append("        ,x.Subtitle " + "\n");
                sbQuery.Append(" 		, x.ContentKey as ContKey1 " + "\n");
                sbQuery.Append(" 		, isnull( y.ContentKey , 0) as ContKey2	/* 1,2가 다르면 설정이 않된것임 */ " + "\n");
                sbQuery.Append(" from ( " + "\n");
                sbQuery.Append(" 	    select   a.SeriesNo " + "\n");
                sbQuery.Append(" 		        ,c.ContentKey " + "\n");
                sbQuery.Append(" 				,c.SubTitle " + "\n");
                sbQuery.Append(" 		from	ChannelSet	a		with(noLock) " + "\n");
                sbQuery.Append(" 		inner join Channel b	with(noLock)	on b.MediaCode = a.MediaCode and b.ChannelNo = a.ChannelNo and b.SeriesNo = a.SeriesNo " + "\n");
                sbQuery.Append(" 		inner join Contents c	with(noLock)	on c.ContentId = b.ContentId and c.ContentsState <= '70' " + "\n");
                sbQuery.Append(" 		where	a.MediaCode     = " + chooseAdScheduleModel.MediaCode + "\n");
                sbQuery.Append(" 		and		a.CategoryCode	= " + chooseAdScheduleModel.CategoryCode + "\n");
                sbQuery.Append(" 		and		a.GenreCode     = " + chooseAdScheduleModel.GenreCode + "\n");
                sbQuery.Append(" 		and		a.ChannelNo		= " + chooseAdScheduleModel.ChannelNo + "\n");
                sbQuery.Append("      ) as x " + "\n");         
                sbQuery.Append(" left outer join AdMidContentsInfo y with(noLock) on y.ContentKey = x.ContentKey " + "\n");
                sbQuery.Append(" order by x.SeriesNo " + "\n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                chooseAdScheduleModel.ChooseAdScheduleDataSet = ds.Copy();
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
                _log.Debug(this.ToString() + "GetMidAdInfoListSeries() End");
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