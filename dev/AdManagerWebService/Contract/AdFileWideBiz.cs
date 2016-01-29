/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드	: [E_01]
 * 수정자		: YJ.Park
 * 수정일		: 2014.11.13
 * 수정내용	: 홈광고 편성 Count할때 홈광고(키즈) 추가
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
    /// AdFileWideBiz에 대한 요약 설명입니다.
    /// </summary>
    public class AdFileWideBiz : BaseBiz
    {
        public AdFileWideBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }

		/// <summary>
		/// 광고파일건수 조회
		/// </summary>
		/// <param name="adFileWideModel"></param>
		public void GetFileCount(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileWideList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey              :[" + adFileWideModel.SearchKey      + "]");				// 검색어
				_log.Debug("SearchMediaCode        :[" + adFileWideModel.SearchMediaCode      + "]");		// 검색 매체
				_log.Debug("SearchchkAdState_10    :[" + adFileWideModel.SearchchkAdState_10  + "]");		// 검색 광고상태 : 준비
				_log.Debug("SearchchkAdState_20    :[" + adFileWideModel.SearchchkAdState_20  + "]");		// 검색 광고상태 : 편성
				_log.Debug("SearchchkAdState_30    :[" + adFileWideModel.SearchchkAdState_30  + "]");		// 검색 광고상태 : 중지	
				_log.Debug("SearchchkAdState_40    :[" + adFileWideModel.SearchchkAdState_40  + "]");		// 검색 광고상태 : 종료   
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n"
                    + " SELECT A.STM_COD AS FileState, A.STM_COD_NM AS FileStateName, NVL(B.FileCount,0) AS FileCount \n"
                    + "  FROM STM_COD A LEFT JOIN             \n"
                    + "      (SELECT FILE_STT AS FileState, COUNT(*) FileCount             \n"
                    + "         FROM ADVT_MST                  \n"
					+ "        WHERE 1 = 1                                     \n"
					);           

				
				// 광고상태 선택에 따라

				// 광고상태 준비
				if(adFileWideModel.SearchchkAdState_10.Trim().Length > 0 && adFileWideModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append("          AND ( ADVT_STT  = '10' \n");
					isState = true;
				}	
				// 광고상태 편성
				if(adFileWideModel.SearchchkAdState_20.Trim().Length > 0 && adFileWideModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          ADVT_STT  = '20' \n");
					isState = true;
				}	
				// 광고상태 중지
				if(adFileWideModel.SearchchkAdState_30.Trim().Length > 0 && adFileWideModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          ADVT_STT  = '30' \n");
					isState = true;
				}	
				// 광고상태 종료
				if(adFileWideModel.SearchchkAdState_40.Trim().Length > 0 && adFileWideModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          ADVT_STT  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				// 검색어가 있으면
				if (adFileWideModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
                        + "    FILE_NM    LIKE '%" + adFileWideModel.SearchKey.Trim() + "%' \n"
                        + " OR ITEM_NM    LIKE '%" + adFileWideModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}

				sbQuery.Append(""
                    + " GROUP BY FILE_STT) B ON (A.STM_COD = B.FileState) \n"
                    + " WHERE A.STM_COD_CLS = '31'  -- 파일상태                    \n"
                    + "   AND A.STM_COD <> '0000'  -- 구분이아닌것                \n"
                    + "ORDER BY A.STM_COD  \n" 
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 해당모델에 복사
				adFileWideModel.CountDataSet = ds.Copy();
				ds.Dispose();


				// 결과
				adFileWideModel.ResultCnt = Utility.GetDatasetCount(adFileWideModel.CountDataSet);
				// 결과코드 셋트
				adFileWideModel.ResultCD = "0000";


				// 2007.10.01 RH.Jung 파일리스트 건수검사용
				// 2007.10.10 RH.Jung 홈광고 리스트 합산시 파일상태가 셋탑삭제가 아닌것으로 변경

				sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ "  SELECT COUNT(*) AS FileCnt			\n"
                    + "  FROM	ADVT_MST	\n"
                    + "  WHERE	FILE_STT = '30'			\n"
					);           

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				adFileWideModel.FileListCount = Convert.ToInt32(ds.Tables[0].Rows[0]["FileCnt"].ToString());
				ds.Dispose();

				// 2007.10.01 

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + adFileWideModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileWideList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD = "3000";
				adFileWideModel.ResultDesc = "광고파일건수 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


        /// <summary>
        /// 광고파일 목록조회
        /// </summary>
        /// <param name="adFileWideModel"></param>
        public void GetAdFileWideList(HeaderModel header, AdFileWideModel adFileWideModel)
        {
			bool isState = false;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdFileWideList() Start");
                _log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchKey              :[" + adFileWideModel.SearchKey      + "]");				// 검색어
				_log.Debug("SearchMediaCode	       :[" + adFileWideModel.SearchMediaCode  + "]");			// 검색 매체
				_log.Debug("SearchFileState        :[" + adFileWideModel.SearchFileState  + "]");			// 검색 파일상태 
				_log.Debug("SearchchkAdState_10    :[" + adFileWideModel.SearchchkAdState_10  + "]");		// 검색 광고상태 : 준비
				_log.Debug("SearchchkAdState_20    :[" + adFileWideModel.SearchchkAdState_20  + "]");		// 검색 광고상태 : 편성
				_log.Debug("SearchchkAdState_30    :[" + adFileWideModel.SearchchkAdState_30  + "]");		// 검색 광고상태 : 중지	
				_log.Debug("SearchchkAdState_40    :[" + adFileWideModel.SearchchkAdState_40  + "]");		// 검색 광고상태 : 종료   
				// __DEBUG__

                StringBuilder sbQuery = new StringBuilder();				
				
                // 쿼리생성
                sbQuery.Append("\n"
                    + " SELECT 'False' AS CheckYn		\n"
                    + "       ,A.ITEM_NO AS ItemNo             \n"
                    + "       ,A.ITEM_NM AS ItemName           \n"
                    + "       ,B.STM_COD_NM AS AdStateName   \n"
                    + "       ,A.FILE_NM AS FileName           \n"
                    + "       ,A.FILE_TYP AS FileType           \n"
                    + "       ,C.STM_COD_NM AS FileTypeName  \n"
                    + "       ,A.FILE_STT AS FileState           \n"
                    + "       ,D.STM_COD_NM AS FileStateName \n"
                    + "	      ,A.FILE_LEN AS FileLength         \n"
                    + "       ,A.FILE_PATH AS FilePath           \n"
                    + "       ,'' AS DownLevel          \n"
                    + "       ,' 순위' AS DownLevelName  \n"
                    + "       ,A.FILE_REG_DT AS FileRegDt               \n"
                    + "       ,A.FILE_REG_ID AS FileRegID               \n"
                    + "       ,E.USER_NM AS FileRegName \n"
                    + "       ,A.FILE_CHK_DT AS CheckDt   , F.USER_NM AS CheckName   \n"
                    + "       ,A.FILE_SYNC_DT AS CDNSyncDt , G.USER_NM AS CDNSyncName \n"
                    + "       ,A.FILE_PUB_DT AS CDNPubDt  , H.USER_NM AS CDNPubName  \n"
                    + "       ,A.FILE_DEL_DT AS STBDelDt  , I.USER_NM AS STBDelName  \n"
                    + "  FROM ADVT_MST A   \n"
                    + "       LEFT JOIN STM_COD B  ON (A.ADVT_STT   = B.STM_COD and B.STM_COD_CLS = '25') \n" // 25:광고상태
                    + "       LEFT JOIN STM_COD C  ON (A.FILE_TYP  = C.STM_COD and C.STM_COD_CLS = '24') \n" // 24:파일구분
                    + "       LEFT JOIN STM_COD D  ON (A.FILE_STT = D.STM_COD and D.STM_COD_CLS = '31') \n" // 31:파일상태
                    + "       LEFT JOIN STM_USER E ON (A.FILE_REG_ID = E.USER_ID)         \n"
                    + "       LEFT JOIN STM_USER F ON (A.FILE_CHK_ID   = F.USER_ID)    \n"
                    + "       LEFT JOIN STM_USER G ON (A.FILE_SYNC_ID = G.USER_ID)    \n"
                    + "       LEFT JOIN STM_USER H ON (A.FILE_PUB_ID  = H.USER_ID)    \n"
                    + "       LEFT JOIN STM_USER I ON (A.FILE_DEL_ID  = I.USER_ID)    \n"
					+ " WHERE 1=1  \n"
                    + "   AND A.FILE_STT  = '" + adFileWideModel.SearchFileState + "' \n"
					);

				// 광고상태 선택에 따라

				// 광고상태 준비
				if(adFileWideModel.SearchchkAdState_10.Trim().Length > 0 && adFileWideModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append("          AND ( A.ADVT_STT  = '10' \n");
					isState = true;
				}	
				// 광고상태 편성
				if(adFileWideModel.SearchchkAdState_20.Trim().Length > 0 && adFileWideModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          A.ADVT_STT  = '20' \n");
					isState = true;
				}	
				// 광고상태 중지
				if(adFileWideModel.SearchchkAdState_30.Trim().Length > 0 && adFileWideModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          A.ADVT_STT  = '30' \n");
					isState = true;
				}	
				// 광고상태 종료
				if(adFileWideModel.SearchchkAdState_40.Trim().Length > 0 && adFileWideModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          A.ADVT_STT  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				// 검색어가 있으면
				if (adFileWideModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
                        + "    A.FILE_NM    LIKE '%" + adFileWideModel.SearchKey.Trim() + "%' \n"
                        + " OR A.ITEM_NM    LIKE '%" + adFileWideModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}

                sbQuery.Append(" ORDER BY A.ITEM_NO DESC \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 해당모델에 복사
                adFileWideModel.FileDataSet = ds.Copy();

				ds.Dispose();

                // 결과
                adFileWideModel.ResultCnt = Utility.GetDatasetCount(adFileWideModel.FileDataSet);
                // 결과코드 셋트
                adFileWideModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + adFileWideModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdFileWideList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                adFileWideModel.ResultCD = "3000";
                adFileWideModel.ResultDesc = "광고파일 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 해당광고의 편성현황 조회
		/// </summary>
		/// <param name="adStatusModel"></param>
		public void GetAdFileSchedule(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			//bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileSchedule() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("ItemNo                 :[" + adFileWideModel.ItemNo            + "]");		// 광고번호
				_log.Debug("SearchMediaCode	       :[" + adFileWideModel.SearchMediaCode      + "]");		// 검색 매체				

				// __DEBUG__

				SqlParameter[] sqlParams = new SqlParameter[2];
		    
				int i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"  , SqlDbType.Int );
				sqlParams[i++] = new SqlParameter("@ItemNo"     , SqlDbType.Int );

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32( adFileWideModel.SearchMediaCode);
				sqlParams[i++].Value = Convert.ToInt32( adFileWideModel.ItemNo);


				StringBuilder sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n																	");
				sbQuery.Append("\n  SELECT  TA.MediaCode											");
				sbQuery.Append("\n         ,TA.ORD													"); 
				sbQuery.Append("\n         ,TA.Flag													"); 
				sbQuery.Append("\n         ,TA.CategoryCode											");
				sbQuery.Append("\n         ,ISNULL(TG.MenuName,'') AS CategoryName					");
				sbQuery.Append("\n         ,TA.GenreCode											");
				sbQuery.Append("\n         ,ISNULL(TH.MenuName,'') AS GenreName						");
				sbQuery.Append("\n         ,TA.ChannelNo											");
				sbQuery.Append("\n         ,TA.Title												");
				sbQuery.Append("\n         ,TA.ItemNo												");
				sbQuery.Append("\n         ,TA.ItemName												");
				sbQuery.Append("\n         ,TB.CodeName AS AdStateName								");
				sbQuery.Append("\n         ,TA.FileName												");
				sbQuery.Append("\n         ,TA.FileType												");
				sbQuery.Append("\n         ,TC.CodeName AS FileTypeName								");
				sbQuery.Append("\n         ,TA.FileState											");
				sbQuery.Append("\n         ,TD.CodeName AS FileStateName							");
				sbQuery.Append("\n         ,TA.FileLength											");
				sbQuery.Append("\n         ,TA.FilePath												");
				sbQuery.Append("\n         ,'' AS DownLevel											");
				sbQuery.Append("\n         ,' 순위' AS DownLevelName		");
				sbQuery.Append("\n         ,TA.ScheduleOrder										");
				sbQuery.Append("\n         ,TA.AdType												");
				sbQuery.Append("\n         ,TE.CodeName AS AdTypeName								");
				sbQuery.Append("\n         ,TA.AckNo												");
				sbQuery.Append("\n         ,TF.State AS AckState									");
				sbQuery.Append("\n   FROM															");
				sbQuery.Append("\n (																");
				sbQuery.Append("\n   SELECT B.MediaCode												");	
				sbQuery.Append("\n         ,'0' AS ORD												");
				sbQuery.Append("\n         ,'홈' AS Flag											");
				sbQuery.Append("\n         ,0 AS CategoryCode										");
				sbQuery.Append("\n         ,0 AS GenreCode											");
				sbQuery.Append("\n         ,0 AS ChannelNo											");	
				sbQuery.Append("\n         ,'' AS Title												");	
				sbQuery.Append("\n         ,A.ItemNo												");
				sbQuery.Append("\n         ,A.ScheduleOrder											");	
				sbQuery.Append("\n         ,A.AckNo													");
				sbQuery.Append("\n         ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType			");
				sbQuery.Append("\n     FROM SchHome A with(NoLock) INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo AND B.MediaCode = @MediaCode)	");
				sbQuery.Append("\n    WHERE A.ItemNo = @ItemNo										");
				
				//[E_01]
				sbQuery.Append("\n																	");
				sbQuery.Append("\n  UNION															");
				sbQuery.Append("\n																	");
				sbQuery.Append("\n  SELECT B.MediaCode												");
				sbQuery.Append("\n        ,'0' AS ORD												");
				sbQuery.Append("\n        ,'홈(키즈)' AS Flag										");
				sbQuery.Append("\n        ,0 AS CategoryCode										");
				sbQuery.Append("\n        ,0 AS GenreCode											");
				sbQuery.Append("\n        ,0 AS ChannelNo											");
				sbQuery.Append("\n        ,'' AS Title												");
				sbQuery.Append("\n        ,A.ItemNo													");
				sbQuery.Append("\n        ,A.ScheduleOrder											");
				sbQuery.Append("\n        ,A.AckNo													");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType					");
				sbQuery.Append("\n    FROM SchHomeKids A with(NoLock) INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo AND B.MediaCode = @MediaCode)	");
				sbQuery.Append("\n   WHERE A.ItemNo = @ItemNo										");
				sbQuery.Append("\n																	");

				sbQuery.Append("\n  UNION															");	
				sbQuery.Append("\n																	");
				sbQuery.Append("\n  SELECT B.MediaCode												");	
				sbQuery.Append("\n        ,'1' AS ORD												");
				sbQuery.Append("\n        ,'메뉴' AS Flag											");	
				sbQuery.Append("\n        ,C.UpperMenuCode AS CategoryCode							");
				sbQuery.Append("\n        ,A.GenreCode												");	
				sbQuery.Append("\n        ,0 AS ChannelNo											");	
				sbQuery.Append("\n        ,'' AS Title												");	 
				sbQuery.Append("\n        ,A.ItemNo													");
				sbQuery.Append("\n        ,A.ScheduleOrder											");	
				sbQuery.Append("\n        ,A.AckNo													");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n   FROM SchChoiceMenuDetail     A with(NoLock)																		");
				sbQuery.Append("\n        INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode)			");
				sbQuery.Append("\n        INNER JOIN Menu         C with(NoLock) ON (A.GenreCode = C.MenuCode  AND C.MediaCode = @MediaCode)			");
				sbQuery.Append("\n   WHERE A.ItemNo = @ItemNo										");
				sbQuery.Append("\n																	");
				sbQuery.Append("\n   UNION															"); 
				sbQuery.Append("\n																	");
				sbQuery.Append("\n SELECT B.MediaCode												");
				sbQuery.Append("\n       ,'2' AS ORD												");
				sbQuery.Append("\n       ,'채널' AS Flag											");
				sbQuery.Append("\n       ,C.Category AS CatagoryCode								");
				sbQuery.Append("\n       ,C.Genre    AS GenreCode									");
				sbQuery.Append("\n       ,A.ChannelNo												");
				sbQuery.Append("\n       ,C.ProgramNm as Title										");
				sbQuery.Append("\n       ,A.ItemNo													");
				sbQuery.Append("\n       ,A.ScheduleOrder											");
				sbQuery.Append("\n       ,A.AckNo													");
				sbQuery.Append("\n       ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType							");
				sbQuery.Append("\n   FROM SchChoiceChannelDetail  A with(NoLock)																							");
				sbQuery.Append("\n        INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode)								");
				sbQuery.Append("\n        INNER JOIN Program      C with(NoLock) ON (A.ChannelNo = C.Channel   AND C.MediaCode = @MediaCode)								");
				sbQuery.Append("\n   WHERE A.ItemNo = @ItemNo																												");
				sbQuery.Append("\n																																			");
				sbQuery.Append("\n ) TA   LEFT JOIN SystemCode TB with(NoLock) ON (TA.AdState      = TB.Code     AND TB.Section = '25') -- 광고상태							");
				sbQuery.Append("\n        LEFT JOIN SystemCode TC with(NoLock) ON (TA.FileType     = TC.Code     AND TC.Section = '24') -- 파일구분							");
				sbQuery.Append("\n        LEFT JOIN SystemCode TD with(NoLock) ON (TA.FileState    = TD.Code     AND TD.Section = '31') -- 파일상태							");  
				sbQuery.Append("\n        LEFT JOIN SystemCode TE with(NoLock) ON (TA.AdType       = TE.Code     AND TE.Section = '26') -- 광고종류							");  
				sbQuery.Append("\n        LEFT JOIN SchPublish TF with(NoLock) ON (TA.AckNo        = TF.AckNo    AND TF.MediaCode = @MediaCode)  -- 승인상태				");
				sbQuery.Append("\n        LEFT JOIN Menu       TG with(NoLock) ON (TA.CategoryCode = TG.MenuCode AND TG.MediaCode = @MediaCode)  -- 카테고리명				");
				sbQuery.Append("\n        LEFT JOIN Menu       TH with(NoLock) ON (TA.GenreCode    = TH.MenuCode AND TH.MediaCode = @MediaCode)  -- 장르명					");

				sbQuery.Append(" ORDER BY ORD, CategoryCode, GenreCode, ChannelNo, ScheduleOrder, FileName \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// 결과 DataSet의 해당모델에 복사
				adFileWideModel.ScheduleDataSet = ds.Copy();
				// 결과
				adFileWideModel.ResultCnt = Utility.GetDatasetCount(adFileWideModel.ScheduleDataSet);
				// 결과코드 셋트
				adFileWideModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + adFileWideModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileSchedule() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD = "3000";
				adFileWideModel.ResultDesc = "[편성현황조회]" + ex.Message;
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 광고파일 검수요청(미등록->검수대기)
		/// </summary>
		/// <returns></returns>
		public void SetAdFileChkRequest(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkRequest() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '12'         \n"	// 파일상태 12:검수대기
                    + "      ,FILE_REG_DT   = SYSDATE    \n"
                    + "      ,FILE_REG_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;			
				sqlParams[i++].Value = header.UserID;                   // 등록자
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("광고파일 검수요청:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkRequest() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "광고파일 검수요청 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 광고파일 검수요청 취소
		/// </summary>
		/// <returns></returns>
		public void SetAdFileChkRequestCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkRequestCancel() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery1 = new StringBuilder();
				int rc = 0;
                OracleParameter[] sqlParams1 = new OracleParameter[2];

                sbQuery1.Append("\n UPDATE ADVT_MST ");
                sbQuery1.Append("\n SET	 FILE_STT   = '10'			");
                sbQuery1.Append("\n		,FILE_REG_DT   = SYSDATE	");
                sbQuery1.Append("\n      ,FILE_REG_ID   = :RegID		");
                sbQuery1.Append("\n WHERE ITEM_NO      = :ItemNo		");


                sqlParams1[0] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams1[1] = new OracleParameter(":ItemNo", OracleDbType.Int32);
						
				sqlParams1[0].Value = header.UserID;
                sqlParams1[1].Value = Convert.ToInt32(adFileWideModel.ItemNo);		
				_log.Debug(sbQuery1.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery1.ToString(), sqlParams1);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("광고파일 검수요청취소:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkRequestCancel() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "광고파일 검수요청 취소 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}



		/// <summary>
		/// 광고파일 검수완료 (검수대기->배포대기)
		/// 2010/07/15 검수대기(12) -> CDN동기화(20): 배포대기상태 삭제요청
		/// 배포대기(15)가 실제 작업은 발생하지 않으나, CMS에 동기화 요청하고 완료처리는
		/// 비동기적으로 발생하기 때문에 중간단계를 사용해야 한다.
		/// </summary>
		/// <returns></returns>
		public void SetAdFileChkComplete(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkComplete() Start");
				_log.Debug("-----------------------------------------");

				_db.Open();

				#region 파일상태 변경 쿼리 설정
				StringBuilder	sbQuery1	= new StringBuilder();
                OracleParameter[] sqlParams1 = new OracleParameter[2];
				int rc1 = 0;

                sbQuery1.Append("\n UPDATE ADVT_MST			");
                sbQuery1.Append("\n SET   FILE_STT	= '15'		");
                sbQuery1.Append("\n		,FILE_CHK_DT	= SYSDATE	");
                sbQuery1.Append("\n      ,FILE_CHK_ID	= :RegID	");
                sbQuery1.Append("\n WHERE ITEM_NO		= :ItemNo	");


                sqlParams1[0] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams1[1] = new OracleParameter(":ItemNo", OracleDbType.Int32);

                sqlParams1[0].Value = header.UserID;
				sqlParams1[1].Value = Convert.ToInt32(adFileWideModel.ItemNo);				
				                   // 등록자
				_log.Debug(sbQuery1.ToString());
				#endregion

				#region CMS작업내역 쿼리 설정
				/*int rc2 = 0;
				StringBuilder sbQuery2 = new StringBuilder();
				SqlParameter[] sqlParams2 = new SqlParameter[7];

				sbQuery2.Append("\n INSERT INTO dbo.AdFileDistribute ");
				sbQuery2.Append("\n            ([ItemNo] ");
				sbQuery2.Append("\n            ,[WorkDt] ");
				sbQuery2.Append("\n            ,[WorkId] ");
				sbQuery2.Append("\n            ,[FilePath] ");
				sbQuery2.Append("\n            ,[FileName] ");
				sbQuery2.Append("\n            ,[cId] ");
				sbQuery2.Append("\n            ,[cmd] ");
				sbQuery2.Append("\n            ,[RequestStatus] ");
				sbQuery2.Append("\n            ,[ProcStatus],[SyncServer],[DescServer]) "); // 결과에서 처리함
				sbQuery2.Append("\n      VALUES ");
				sbQuery2.Append("\n            (@ItemNo ");
				sbQuery2.Append("\n            ,GetDate() ");
				sbQuery2.Append("\n            ,@WorkId ");
				sbQuery2.Append("\n            ,@FilePath ");
				sbQuery2.Append("\n            ,@FileName ");
				sbQuery2.Append("\n            ,@cId ");
				sbQuery2.Append("\n            ,@cmd ");
				sbQuery2.Append("\n            ,@RequestStatus ");
				sbQuery2.Append("\n            ,0,0,0) ");
				
				sqlParams2[0] = new SqlParameter("@ItemNo"		, SqlDbType.Int          );
				sqlParams2[1] = new SqlParameter("@WorkId"		, SqlDbType.VarChar ,  10);
				sqlParams2[2] = new SqlParameter("@FilePath"    , SqlDbType.VarChar ,  100);
				sqlParams2[3] = new SqlParameter("@FileName"    , SqlDbType.VarChar ,  50);
				sqlParams2[4] = new SqlParameter("@cId"			, SqlDbType.VarChar ,  40);
				sqlParams2[5] = new SqlParameter("@cmd"			, SqlDbType.VarChar ,  10);
				sqlParams2[6] = new SqlParameter("@RequestStatus",SqlDbType.VarChar ,  1);
		
				sqlParams2[0].Value	= Convert.ToInt32(adFileWideModel.ItemNo);	
				sqlParams2[1].Value	= header.UserID;                   
				sqlParams2[2].Value	= adFileWideModel.FilePath.Trim();
				sqlParams2[3].Value	= adFileWideModel.FileName.Trim();
				sqlParams2[4].Value	= adFileWideModel.CmsCid.Trim();
				sqlParams2[5].Value	= adFileWideModel.CmsCmd.Trim();
				sqlParams2[6].Value	= adFileWideModel.CmsRequestStatus.Trim();
				_log.Debug(sbQuery2.ToString());*/
				#endregion

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc1 =  _db.ExecuteNonQueryParams(sbQuery1.ToString(), sqlParams1);
                    /*
					if ( !adFileWideModel.CmsCid.Trim().Equals("0000") )
					{
						rc2 =  _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams2);
					}
                    */
					_db.CommitTran();
					_log.Message("광고파일 검수완료:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkComplete() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "광고파일 검수완료 처리 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// CMS연동 호출처리
		/// </summary>
		/// <param name="header"></param>
		/// <param name="adFileWideModel"></param>
		public void SetCmsRequestBegin(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCmsRequestBegin() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();

				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[7];

				sbQuery.Append("\n INSERT INTO dbo.AdFileDistribute ");
				sbQuery.Append("\n            ([ItemNo] ");
				sbQuery.Append("\n            ,[WorkDt] ");
				sbQuery.Append("\n            ,[WorkId] ");
				sbQuery.Append("\n            ,[FilePath] ");
				sbQuery.Append("\n            ,[FileName] ");
				sbQuery.Append("\n            ,[cId] ");
				sbQuery.Append("\n            ,[cmd] ");
				sbQuery.Append("\n            ,[RequestStatus] ");
				sbQuery.Append("\n            ,[ProcStatus],[SyncServer],[DescServer]) "); // 결과에서 처리함
				sbQuery.Append("\n      VALUES ");
				sbQuery.Append("\n            (@ItemNo ");
				sbQuery.Append("\n            ,GetDate() ");
				sbQuery.Append("\n            ,@WorkId ");
				sbQuery.Append("\n            ,@FilePath ");
				sbQuery.Append("\n            ,@FileName ");
				sbQuery.Append("\n            ,@cId ");
				sbQuery.Append("\n            ,@cmd ");
				sbQuery.Append("\n            ,@RequestStatus ");
				sbQuery.Append("\n            ,0,0,0) ");
				
				sqlParams[0] = new SqlParameter("@ItemNo"		, SqlDbType.Int          );
				sqlParams[1] = new SqlParameter("@WorkId"		, SqlDbType.VarChar ,  10);
				sqlParams[2] = new SqlParameter("@FilePath"     , SqlDbType.VarChar ,  100);
				sqlParams[3] = new SqlParameter("@FileName"     , SqlDbType.VarChar ,  50);
				sqlParams[4] = new SqlParameter("@cId"			, SqlDbType.VarChar ,  20);
				sqlParams[5] = new SqlParameter("@cmd"			, SqlDbType.VarChar ,  10);
				sqlParams[6] = new SqlParameter("@RequestStatus", SqlDbType.VarChar ,  1);
		
				sqlParams[0].Value	= Convert.ToInt32(adFileWideModel.ItemNo);	
				sqlParams[1].Value	= header.UserID;                   
				sqlParams[2].Value	= adFileWideModel.FilePath.Trim();
				sqlParams[3].Value	= adFileWideModel.FileName.Trim();
				sqlParams[4].Value	= adFileWideModel.CmsCid.Trim();
				sqlParams[5].Value	= adFileWideModel.CmsCmd.Trim();
				sqlParams[6].Value	= adFileWideModel.CmsRequestStatus.Trim();
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("광고파일 CMS연동 :["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCmsRequestBegin() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "광고파일 검수완료 처리 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 광고파일 검수완료 취소
		/// </summary>
		/// <returns></returns>
		public void SetAdFileChkCompleteCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkCompleteCancel() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '12'         \n"	// 파일상태 12:검수대기
                    + "      ,FILE_CHK_DT   = SYSDATE    \n"
                    + "      ,FILE_CHK_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;			
				sqlParams[i++].Value = header.UserID;                   // 등록자
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("광고파일 검수완료취소:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkCompleteCancel() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "광고파일 검수완료 취소 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 광고파일 CDN동기확인
		/// </summary>
		/// <returns></returns>		
		public void SetAdFileCDNSync(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNSync() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '20'         \n"	// 파일상태 20:CDN동기화							
                    + "      ,FILE_SYNC_DT   = SYSDATE    \n"
                    + "      ,FILE_SYNC_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;
				sqlParams[i++].Value = header.UserID;                   // 등록자
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("광고파일 CDN동기확인:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNSync() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "광고파일 CDN동기확인 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}



		/// <summary>
		/// 광고파일 CDN동기확인 취소
		/// </summary>
		/// <returns></returns>		
		public void SetAdFileCDNSyncCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNSyncCancel() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '15'         \n"	// 파일상태 15:배포대기						
                    + "      ,FILE_SYNC_DT   = SYSDATE    \n"
                    + "      ,FILE_SYNC_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;      
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;
				sqlParams[i++].Value = header.UserID;                   // 등록자
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("광고파일 CDN동기확인 취소:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNSyncCancel() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "광고파일 CDN동기확인 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}



        /// <summary>
        /// 광고파일 CDN배포확인
        /// </summary>
        /// <returns></returns>		
		public void SetAdFileCDNPublish(HeaderModel header, AdFileWideModel adFileWideModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAdFileCDNPublish() Start");
                _log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

                int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

                sbQuery.Append("\n");
                sbQuery.Append("\n UPDATE ADVT_MST");
                sbQuery.Append("\n    SET FILE_STT	= '30'");
                sbQuery.Append("\n       ,FILE_PUB_DT  = SYSDATE");
                sbQuery.Append("\n       ,FILE_PUB_ID  = :RegID");
                sbQuery.Append("\n WHERE ITEM_NO     = :ItemNo");

                
                sqlParams[0] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[1] = new OracleParameter(":ItemNo", OracleDbType.Int32, 4);
                			
                sqlParams[0].Value = header.UserID;
                sqlParams[1].Value = Convert.ToInt32(adFileWideModel.ItemNo);	

                _log.Debug(sbQuery.ToString());

				try
                {
                    _db.BeginTran();
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("광고파일 배포완료:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");
					string AckNo = GetFileAckNo(adFileWideModel.MediaCode);
					SetFilePublishHistory(header, adFileWideModel, AckNo, "+");
                    _db.CommitTran();
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                adFileWideModel.ResultCD = "0000";  // 정상
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAdFileCDNPublish() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                adFileWideModel.ResultCD   = "3201";
                adFileWideModel.ResultDesc = "광고파일 배포완료 처리 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
			finally
			{
				_db.Close();
			}		
        }


		/// <summary> 
		/// 광고파일 CDN배포확인 취소
		/// </summary>
		/// <returns></returns>
		public void SetAdFileCDNPublishCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNPublishCancel() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
					+ "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '20'         \n"	// 파일상태 20:CDN동기화
                    + "      ,FILE_PUB_DT   = SYSDATE    \n"
                    + "      ,FILE_PUB_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;
                
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;		
				sqlParams[i++].Value = header.UserID;                   // 등록자
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	

				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// __MESSAGE__
					_log.Message("광고파일 CDN배포확인취소:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");

					// 현재 승인번호를 구함
					string AckNo = GetFileAckNo(adFileWideModel.MediaCode);
					
					SetFilePublishHistory(header, adFileWideModel, AckNo, "-");		// 삭제됨

					_db.CommitTran();


				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNPublishCancel() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "광고파일 배포완료 취소 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}





        /// <summary>
        /// 광고파일 셋탑삭제
        /// </summary>
        /// <returns></returns>
        public void SetAdFileSTBDelete(HeaderModel header, AdFileWideModel adFileWideModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAdFileSTBDelete() Start");
                _log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

                sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '90'         \n"	// 파일상태 90:셋탑삭제							
//                    + "      ,RealEndDay  = CONVERT(CHAR(8),GETDATE(),112)  \n"	// 삭제시 실제종료일을 셋트한다. 2007-10-28 아니다 안한다.
                    + "      ,FILE_DEL_DT   = SYSDATE    \n"
                    + "      ,FILE_DEL_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
                    );

                i = 0;
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

                i = 0;	
                sqlParams[i++].Value = header.UserID;                   // 등록자
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	
                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                try
                {
                    _db.BeginTran();

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// __MESSAGE__
					_log.Message("광고파일 셋탑삭제:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");


					// 현재 승인번호를 구함
					string AckNo = GetFileAckNo(adFileWideModel.MediaCode);
					
					SetFilePublishHistory(header, adFileWideModel, AckNo, "-");		// 삭제됨

                    _db.CommitTran();

                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                adFileWideModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAdFileSTBDelete() End");
                _log.Debug("-----------------------------------------");

            }
            catch(Exception ex)
            {
                adFileWideModel.ResultCD   = "3201";
                adFileWideModel.ResultDesc = "광고파일 셋탑삭제 처리중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 광고파일 셋탓삭제 취소
		/// </summary>
		/// <returns></returns>
        
		public void SetAdFileSTBDeleteCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileSTBDeleteCancel() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '30'         \n"	// 파일상태 30:배포완료
                    + "      ,FILE_DEL_DT   = SYSDATE    \n"
                    + "      ,FILE_DEL_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;	
				sqlParams[i++].Value = header.UserID;                   // 등록자
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// __MESSAGE__
					_log.Message("광고파일 셋탑삭제취소:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");

					// 현재 승인번호를 구함
					string AckNo = GetFileAckNo(adFileWideModel.MediaCode);
					
					SetFilePublishHistory(header, adFileWideModel, AckNo, "+");		// 추가됨

					_db.CommitTran();


				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileSTBDeleteCancel() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "광고파일 셋탑삭제취소 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 광고파일 소재교체
		/// 2010/07/19일 History내역 추가
		/// </summary>
		/// <returns></returns>
		public void SetAdFileChange(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			/*
				10:미등록
			 *	12:검수대기
			 *	15:배포대기
			 *	20:CDN동기화
			 *	30:배포완료
			 */
			try
			{
				int rc = 0;

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChange() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();
				_db.BeginTran();

				#region [ 광고파일 소재교체 설정 ]
				StringBuilder sbQuery		= new StringBuilder();

                OracleParameter[] sqlParams = new OracleParameter[1];

                sqlParams[0] = new OracleParameter(":ItemNo", OracleDbType.Int32);
				//sqlParams[1] = new SqlParameter(":RegID"      , SqlDbType.VarChar	,  10);				
				
				sqlParams[0].Value = Convert.ToInt32(adFileWideModel.ItemNo);				
				//sqlParams[1].Value = header.UserID;


				sbQuery.Append("\n");
                sbQuery.Append("\n UPDATE ADVT_MST");
                sbQuery.Append("\n    SET FILE_STT		= '10'");		// 파일상태 11:교체대기에서 10:미등록으로 설정하기로 합의함(10년 고도화)							
                sbQuery.Append("\n		, FILE_LEN	= 0");
                sbQuery.Append("\n WHERE  ITEM_NO		= :ItemNo");
				_log.Debug(sbQuery.ToString());

				try
				{
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("광고파일 소재교체대기:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");

					// 해당 파일의 상태가 CDN 배포완료인 경우에만 
					if(adFileWideModel.FileState.Equals("30"))
					{
						// 현재 승인번호를 구함
						string AckNo = GetFileAckNo(adFileWideModel.MediaCode);
						SetFilePublishHistory(header, adFileWideModel, AckNo, "-");		// 삭제됨
					}
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
				#endregion

				#region [ 파일교체 History저장 ]
				sbQuery.Remove(0, sbQuery.Length );
                sbQuery.Append("\n insert into ADVT_FILEHST");
                sbQuery.Append("\n     (	ITEM_NO	,HST_SEQ	,REP_DT		,REP_ID");
                sbQuery.Append("\n 		,	FILE_STT	,FILE_TYP		,FILE_LEN");
                sbQuery.Append("\n 		,	FILE_PATH	,FILE_NM_PRE	,FILE_NM");
                sbQuery.Append("\n 		,	FILE_REG_DT	,FILE_REG_ID");
                sbQuery.Append("\n 		,	FILE_CHK_DT	,FILE_CHK_ID		,FILE_SYNC_DT");
                sbQuery.Append("\n 		,	FILE_SYNC_ID	,FILE_PUB_DT		,FILE_PUB_ID");
                sbQuery.Append("\n 		,	FILE_DEL_DT  ,FILE_DEL_ID		)");
                sbQuery.Append("\n select	ITEM_NO");
                sbQuery.Append("\n 		,	( select nvl(count(*),0)+1 from ADVT_FILEHST where ITEM_NO = " + Convert.ToInt32(adFileWideModel.ItemNo) + ")");
				sbQuery.Append("\n 		,	SYSDATE	, '" + header.UserID + "'");
                sbQuery.Append("\n 		,	FILE_STT	,FILE_TYP		,FILE_LEN");
                sbQuery.Append("\n 		,	FILE_PATH	,FILE_NM_PRE	,FILE_NM");
                sbQuery.Append("\n 		,	FILE_REG_DT	,FILE_REG_ID");
                sbQuery.Append("\n 		,	FILE_CHK_DT	,FILE_CHK_ID		,FILE_SYNC_DT");
                sbQuery.Append("\n 		,	FILE_SYNC_ID	,FILE_PUB_DT		,FILE_PUB_ID");
                sbQuery.Append("\n 		,	FILE_DEL_DT  ,FILE_DEL_ID");
                sbQuery.Append("\n from		ADVT_MST ");
                sbQuery.Append("\n where	ITEM_NO = " + Convert.ToInt32(adFileWideModel.ItemNo) );
				_log.Debug(sbQuery.ToString());

				try
				{
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
				#endregion

				if( rc == 0 )
				{
					_db.RollbackTran();
					throw new Exception("파일교체이력 데이터 추가가 되지 않았습니다..");
				}
				else
				{
					_db.CommitTran();
					adFileWideModel.ResultCD = "0000";  // 정상
				}

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChange() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "광고파일 소재교체대기 처리중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}


		#region 현재승인상태의 승인번호를 구함

		/// <summary>
		/// 현재 파일배포승인상태의 승인번호를 구함
		/// 상태가 30:배포승인이면 신규(상태 10:승인대기) 으로 생성후 AckNo 리턴
		/// </summary>
		/// <returns>string</returns>
		private string GetFileAckNo(string MediaCode)
		{

			string AckNo    = "";
			string AckState = "";

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFileAckNo() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode	       :[" + MediaCode     + "]");		// 검색 매체				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("SELECT * FROM TABLE(GET_ACKNO('00'))");

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
				_log.Debug(this.ToString() + "GetFileAckNo() End");
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

		#region 파일배포이력에 넣기

		private bool SetFilePublishHistory(HeaderModel header, AdFileWideModel adFileWideModel, string AckNo, string AddDel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFilePublishHistory() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[5];

				sbQuery.Append( ""
                    + " INSERT INTO FILEDIST_HST (\n"
                    + "        ACK_NO       \n"
                    + "       ,ITEM_SEQ         \n"
                    + "       ,ITEM_NO      \n"
                    + "       ,FILE_NM    \n"
                    + "       ,FILE_OPER      \n"
                    + "       ,ID_INSERT       \n"
                    + "       ,DT_INSERT       \n"					
					+ "       )            \n"
					+ " SELECT            \n"
                    + "        :AckNo      \n"
                    + "       ,NVL(MAX(ITEM_SEQ),0) + 1 \n"
					+ "       ,:ItemNo     \n"
					+ "       ,:FileName   \n"					
					+ "       ,:AddDel     \n"
                    + "       ,:RegID      \n"
					+ "       ,SYSDATE   \n"	
                    + "  FROM FILEDIST_HST       \n"
                    + " WHERE ACK_NO     = :AckNo     \n"
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":AckNo", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":FileName", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":AddDel", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                //sqlParams[i++] = new OracleParameter("@AckNoTest", OracleDbType.Int32);

				i = 0;
                sqlParams[i++].Value = Convert.ToInt32(AckNo);
				sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);				
				sqlParams[i++].Value = adFileWideModel.FileName;				
				sqlParams[i++].Value = AddDel;				
				sqlParams[i++].Value = header.UserID;				              // 등록자
                //sqlParams[i++].Value = Convert.ToInt32(AckNo);

				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// __MESSAGE__
					_log.Message("파일배포이력등록:승인번호["+AckNo+"]["+adFileWideModel.ItemNo+"]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFilePublishHistory() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				return false;
			}
			
			return true;

		}

		#endregion

    }
}