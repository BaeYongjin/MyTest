/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.06.04
 * 수정내용  : 
 *            - GetAdStatusList_Compress() 함수 복원
 * --------------------------------------------------------
 * 수정코드  : [E_02] 
 * 수정자		: YJ.Park
 * 수정일		: 2014.11.13
 * 수정내용	: 홈광고(키즈)관련 조회 쿼리 추가
 * --------------------------------------------------------  */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using AdManagerModel;

namespace AdManagerWebService.Schedule
{
	/// <summary>
	/// AdStatusBiz에 대한 요약 설명입니다.
	/// </summary>
	public class AdStatusBiz : BaseBiz
	{
		public AdStatusBiz() : base(FrameSystem.connDbString,true)
		{
			_log = FrameSystem.oLog;
		}
		/// <summary>
		/// 광고파일 목록조회
		/// </summary>
		/// <param name="adStatusModel"></param>
		public void GetAdStatusList(HeaderModel header, AdStatusModel adStatusModel)
		{
			//bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdStatusList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey              :[" + adStatusModel.SearchKey            + "]");		// 검색 어
				_log.Debug("SearchMediaCode	       :[" + adStatusModel.SearchMediaCode      + "]");		// 검색 매체				

				// __DEBUG__

				SqlParameter[] sqlParams = new SqlParameter[2];
		    
				int i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"          , SqlDbType.Int          );
				sqlParams[i++] = new SqlParameter("@SearchKey"          , SqlDbType.VarChar,  50 );

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32( adStatusModel.SearchMediaCode);
				sqlParams[i++].Value = adStatusModel.SearchKey ;


				StringBuilder sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n SELECT  TA.MediaCode																		");
				sbQuery.Append("\n        ,TA.ORD																					");
				sbQuery.Append("\n        ,TA.Flag																					");
				sbQuery.Append("\n        ,TA.CategoryCode																		");
				sbQuery.Append("\n        ,ISNULL(TG.MenuName,'') AS CategoryName									");
				sbQuery.Append("\n        ,TA.GenreCode																			");
				sbQuery.Append("\n        ,ISNULL(TH.MenuName,'') AS GenreName										");
				sbQuery.Append("\n        ,TA.ChannelNo																			");
				sbQuery.Append("\n        ,TA.Title																					");
				sbQuery.Append("\n        ,TA.ItemNo																				");
				sbQuery.Append("\n        ,TA.ItemName																			");
				sbQuery.Append("\n        ,TB.CodeName AS AdStateName													");
				sbQuery.Append("\n        ,TA.FileName																				");
				sbQuery.Append("\n        ,TA.FileType																				");
				sbQuery.Append("\n        ,TC.CodeName AS FileTypeName													");
				sbQuery.Append("\n        ,TA.FileState																				");
				sbQuery.Append("\n        ,TD.CodeName AS FileStateName													");
				sbQuery.Append("\n        ,TA.FileLength																			");
				sbQuery.Append("\n        ,TA.FilePath																				");
				sbQuery.Append("\n        ,TA.DownLevel																			");
				sbQuery.Append("\n        ,CONVERT(VarChar(3),TA.DownLevel) +  ' 순위' AS DownLevelName		");
				sbQuery.Append("\n        ,TA.ScheduleOrder																		");
				sbQuery.Append("\n        ,TA.AdType																				");
				sbQuery.Append("\n        ,TE.CodeName AS AdTypeName									                ");
				sbQuery.Append("\n        ,TA.AckNo																				");
				sbQuery.Append("\n        ,TF.State AS AckState																");
				sbQuery.Append("\n  FROM																							");
				sbQuery.Append("\n(																										");
				sbQuery.Append("\n  SELECT B.MediaCode																		");
				sbQuery.Append("\n        ,'0' AS ORD																				");
				sbQuery.Append("\n        ,'홈' AS Flag																				");
				sbQuery.Append("\n        ,0 AS CategoryCode																	");
				sbQuery.Append("\n        ,0 AS GenreCode																		");
				sbQuery.Append("\n        ,0 AS ChannelNo																		");
				sbQuery.Append("\n        ,'' AS Title																				");
				sbQuery.Append("\n        ,A.ItemNo																					");
				sbQuery.Append("\n        ,A.ScheduleOrder																		");
				sbQuery.Append("\n        ,A.AckNo																					");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n    FROM SchHome A with(NoLock)																												");
				sbQuery.Append("\n         INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo AND A.MediaCode = B.MediaCode)				");
				sbQuery.Append("\n   WHERE A.MediaCode = @MediaCode													");
				sbQuery.Append("\n																										");

				// [E_02]
				sbQuery.Append("\n  UNION ALL																						");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n  SELECT B.MediaCode																        ");
				sbQuery.Append("\n        ,'0' AS ORD																				");
				sbQuery.Append("\n        ,'홈(키즈)' AS Flag																		");
				sbQuery.Append("\n        ,0 AS CategoryCode																	");
				sbQuery.Append("\n        ,0 AS GenreCode																		");
				sbQuery.Append("\n        ,0 AS ChannelNo																		");
				sbQuery.Append("\n        ,'' AS Title																				");
				sbQuery.Append("\n        ,A.ItemNo																					");	
				sbQuery.Append("\n        ,A.ScheduleOrder																		");
				sbQuery.Append("\n        ,A.AckNo																					");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n    FROM SchHomeKids A with(NoLock)																											");
				sbQuery.Append("\n         INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo AND A.MediaCode = B.MediaCode)				");
				sbQuery.Append("\n   WHERE A.MediaCode = @MediaCode													");
				sbQuery.Append("\n																										");

				sbQuery.Append("\n  UNION ALL																						");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n  SELECT B.MediaCode																		");
				sbQuery.Append("\n        ,'1' AS ORD																				");
				sbQuery.Append("\n        ,'메뉴' AS Flag																			");
				sbQuery.Append("\n        ,C.UpperMenuCode AS CategoryCode												");
				sbQuery.Append("\n        ,A.GenreCode																			");
				sbQuery.Append("\n        ,0 AS ChannelNo																		");
				sbQuery.Append("\n        ,'' AS Title																				");
				sbQuery.Append("\n        ,A.ItemNo																					");
				sbQuery.Append("\n        ,A.ScheduleOrder																		");
				sbQuery.Append("\n        ,A.AckNo																					");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n   FROM SchChoiceMenuDetail A with(NoLock)																									");
				sbQuery.Append("\n        INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode)		");
				sbQuery.Append("\n        INNER JOIN Menu         C with(NoLock) ON (A.GenreCode = C.MenuCode  AND C.MediaCode = @MediaCode)		");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n   UNION ALL																						");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n SELECT B.MediaCode																			");
				sbQuery.Append("\n       ,'2' AS ORD																				");
				sbQuery.Append("\n       ,'채널' AS Flag																			");
				sbQuery.Append("\n       ,C.Category AS CatagoryCode														");
				sbQuery.Append("\n       ,C.Genre    AS GenreCode																");
				sbQuery.Append("\n       ,A.ChannelNo																				");
				sbQuery.Append("\n       ,C.ProgramNm as Title																	");
				sbQuery.Append("\n       ,A.ItemNo																					");
				sbQuery.Append("\n       ,A.ScheduleOrder																		");
				sbQuery.Append("\n       ,A.AckNo																					");
				sbQuery.Append("\n       ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n   FROM SchChoiceChannelDetail A with(NoLock)																								");
				sbQuery.Append("\n        INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode)		");
				sbQuery.Append("\n        INNER JOIN Program      C with(NoLock) ON (A.ChannelNo = C.Channel   AND C.MediaCode = @MediaCode)		");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n   UNION ALL																						");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n  SELECT B.MediaCode																		");
				sbQuery.Append("\n        ,'3' AS ORD																				");
				sbQuery.Append("\n        ,'추가' AS Flag																			");
				sbQuery.Append("\n        ,0 AS CategoryCode																	");
				sbQuery.Append("\n        ,0 AS GenreCode																		");
				sbQuery.Append("\n        ,0 AS ChannelNo																		");
				sbQuery.Append("\n        ,'' AS Title																				");
				sbQuery.Append("\n        ,A.ItemNo																					");
				sbQuery.Append("\n        ,A.ScheduleOrder																		");
				sbQuery.Append("\n        ,A.AckNo																					");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n    FROM SchAppend A with(NoLock)																												");
				sbQuery.Append("\n         INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo AND A.MediaCode = B.MediaCode)				");
				sbQuery.Append("\n   WHERE A.MediaCode = @MediaCode																																	");
				sbQuery.Append("\n																																														");
				sbQuery.Append("\n ) TA   LEFT JOIN SystemCode TB with(NoLock) ON (TA.AdState      = TB.Code     AND TB.Section = '25') -- 광고상태							");
				sbQuery.Append("\n        LEFT JOIN SystemCode TC with(NoLock) ON (TA.FileType     = TC.Code     AND TC.Section = '24') -- 파일구분							");
				sbQuery.Append("\n        LEFT JOIN SystemCode TD with(NoLock) ON (TA.FileState    = TD.Code     AND TD.Section = '31') -- 파일상태							");
				sbQuery.Append("\n        LEFT JOIN SystemCode TE with(NoLock) ON (TA.AdType       = TE.Code     AND TE.Section = '26') -- 광고종류							");
				sbQuery.Append("\n        LEFT JOIN SchPublish TF with(NoLock) ON (TA.AckNo        = TF.AckNo    AND TF.MediaCode = @MediaCode)  -- 승인상태				");
				sbQuery.Append("\n        LEFT JOIN Menu       TG with(NoLock) ON (TA.CategoryCode = TG.MenuCode AND TG.MediaCode = @MediaCode)  -- 카테고리명		");
				sbQuery.Append("\n        LEFT JOIN Menu       TH with(NoLock) ON (TA.GenreCode    = TH.MenuCode AND TH.MediaCode = @MediaCode)  -- 장르명			");

				// 검색어가 있으면
				if (adStatusModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" WHERE ("
						+ "    TA.FileName    LIKE '%'+ @SearchKey + '%' \n"		
						+ " OR TA.ItemName    LIKE '%'+ @SearchKey + '%' \n"													
						+ " OR TG.MenuName    LIKE '%'+ @SearchKey + '%' \n"													
						+ " OR TH.MenuName    LIKE '%'+ @SearchKey + '%' \n"													
						+ " OR TA.ChannelNo   LIKE '%'+ @SearchKey + '%' \n"													
						+ " OR TA.Title       LIKE '%'+ @SearchKey + '%' \n"														
						+ " ) \n"
						);
				}			

				sbQuery.Append(" ORDER BY ORD, CategoryCode, GenreCode, ChannelNo, ScheduleOrder, FileName  \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// 결과 DataSet의 해당모델에 복사
				adStatusModel.AdStatusDataSet = ds.Copy();
				// 결과
				adStatusModel.ResultCnt = Utility.GetDatasetCount(adStatusModel.AdStatusDataSet);
				// 결과코드 셋트
				adStatusModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + adStatusModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdStatusList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adStatusModel.ResultCD = "3000";
				adStatusModel.ResultDesc = "광고파일 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

        /// <summary>
        /// 위에것과 동일한 기능을 수행하는데, 압축을 사용한 것임.
        /// 2013.05.02 일단 이 기능만 압축테스트 하는 것임.
        /// 잘 되면, 다음 고도화 때 일괄적으로 압축사용할 것임
        /// </summary>
        /// <param name="header"></param>
        /// <param name="adStatusModel"></param>
        public void GetAdStatusList_Compress(HeaderModel header, AdStatusModel adStatusModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdStatusList() Start");
                _log.Debug("-----------------------------------------");

                _db.Open();
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey              :[" + adStatusModel.SearchKey + "]");		// 검색어
                _log.Debug("SearchMediaCode	       :[" + adStatusModel.SearchMediaCode + "]");		// 검색 매체				

                OracleParameter[] sqlParams = new OracleParameter[1];

                sqlParams[0] = new OracleParameter("@SearchKey", OracleDbType.Varchar2, 50);

                sqlParams[0].Value = adStatusModel.SearchKey;

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n SELECT  TA.MediaCode													");
                sbQuery.Append("\n        ,TA.ORD																");
                sbQuery.Append("\n        ,TA.Flag																");
                sbQuery.Append("\n        ,TA.CategoryCode													");
                sbQuery.Append("\n        ,NVL(TG.MENU_NM,'') AS CategoryName				");
                sbQuery.Append("\n        ,TA.GenreCode														");
                sbQuery.Append("\n        ,NVL(TH.MENU_NM,'') AS GenreName					");
                sbQuery.Append("\n        ,TA.ChannelNo														");
                sbQuery.Append("\n        ,TA.Title																");
                sbQuery.Append("\n        ,TA.ItemNo															");
                sbQuery.Append("\n        ,TA.ItemName														");
                sbQuery.Append("\n        ,TB.STM_COD_NM AS AdStateName								");
                sbQuery.Append("\n        ,TA.FileName															");
                sbQuery.Append("\n        ,TA.FileType															");
                sbQuery.Append("\n        ,TC.STM_COD_NM AS FileTypeName								");
                sbQuery.Append("\n        ,TA.FileState															");
                sbQuery.Append("\n        ,TD.STM_COD_NM AS FileStateName								");
                sbQuery.Append("\n        ,TA.FileLength														");
                sbQuery.Append("\n        ,TA.FilePath															");
                sbQuery.Append("\n        ,TA.DownLevel														");
                sbQuery.Append("\n        ,' 순위' AS DownLevelName    ");
                sbQuery.Append("\n        ,TA.ScheduleOrder											        ");
                sbQuery.Append("\n        ,TA.AdType															");
                sbQuery.Append("\n        ,TE.STM_COD_NM AS AdTypeName						        ");
                sbQuery.Append("\n        ,TA.AckNo														    ");
                sbQuery.Append("\n        ,TF.ACK_STT AS AckState										    ");
                sbQuery.Append("\n  FROM																		");
                sbQuery.Append("\n(																					");
                sbQuery.Append("\n  SELECT 1 AS MediaCode												    ");
                sbQuery.Append("\n        ,'0' AS ORD															");
                sbQuery.Append("\n        ,'메뉴' AS Flag															");
                sbQuery.Append("\n        ,'' AS CategoryCode										        ");
                sbQuery.Append("\n        ,A.MENU_COD AS GenreCode											        ");
                sbQuery.Append("\n        ,'' AS ChannelNo												    ");
                sbQuery.Append("\n        ,'' AS Title															");
                sbQuery.Append("\n        ,A.ITEM_NO AS ItemNo																");
                sbQuery.Append("\n        ,A.SCHD_ORD AS ScheduleOrder												    ");
                sbQuery.Append("\n        ,A.ACK_NO AS AckNo																");
                sbQuery.Append("\n        ,B.ITEM_NM AS ItemName, B.ADVT_STT AS AdState, B.FILE_NM AS FileName, B.FILE_TYP AS FileType, B.FILE_STT AS FileState, B.FILE_LEN AS FileLength,B.FILE_PATH AS FilePath,'' AS DownLevel,B.ADVT_TYP AS AdType		");
                sbQuery.Append("\n    FROM SCHD_MENU A 																												");
                sbQuery.Append("\n         INNER JOIN ADVT_MST B ON (A.ITEM_NO = B.ITEM_NO)				");
                sbQuery.Append("\n   								");
                sbQuery.Append("\n																					");
				
				//[E_02]
                sbQuery.Append("\n  UNION ALL																    ");
				sbQuery.Append("\n																					");
                sbQuery.Append("\n  SELECT 1 AS MediaCode												    ");
                sbQuery.Append("\n        ,'1' AS ORD															");
                sbQuery.Append("\n        ,'재핑' AS Flag															");
                sbQuery.Append("\n        ,'' AS CategoryCode										        ");
                sbQuery.Append("\n        ,'' AS GenreCode											        ");
                sbQuery.Append("\n        ,CH_NO AS ChannelNo												    ");
                sbQuery.Append("\n        ,'' AS Title															");
                sbQuery.Append("\n        ,A.ITEM_NO AS ItemNo																");
                sbQuery.Append("\n        ,A.SCHD_ORD AS ScheduleOrder												    ");
                sbQuery.Append("\n        ,A.ACK_NO AS AckNo																");
                sbQuery.Append("\n        ,B.ITEM_NM AS ItemName, B.ADVT_STT AS AdState, B.FILE_NM AS FileName, B.FILE_TYP AS FileType, B.FILE_STT AS FileState, B.FILE_LEN AS FileLength,B.FILE_PATH AS FilePath,'' AS DownLevel,B.ADVT_TYP AS AdType		");
                sbQuery.Append("\n    FROM SCHD_CHNL A 																												");
                sbQuery.Append("\n         INNER JOIN ADVT_MST B ON (A.ITEM_NO = B.ITEM_NO)				");
				sbQuery.Append("\n																					");
                sbQuery.Append("\n																																								");
                sbQuery.Append("\n ) TA   LEFT JOIN STM_COD TB ON (TA.AdState      = TB.STM_COD     AND TB.STM_COD_CLS = '25') -- 광고상태	");
                sbQuery.Append("\n        LEFT JOIN STM_COD TC ON (TA.FileType     = TC.STM_COD     AND TC.STM_COD_CLS = '24') -- 파일구분	");
                sbQuery.Append("\n        LEFT JOIN STM_COD TD ON (TA.FileState    = TD.STM_COD     AND TD.STM_COD_CLS = '31') -- 파일상태	");
                sbQuery.Append("\n        LEFT JOIN STM_COD TE ON (TA.AdType       = TE.STM_COD     AND TE.STM_COD_CLS = '26') -- 광고종류	");
                sbQuery.Append("\n        LEFT JOIN SCHD_DIST_MST TF ON (TA.AckNo        = TF.ACK_NO)  -- 승인상태				");
                sbQuery.Append("\n        LEFT JOIN MENU_COD       TG ON (TA.CategoryCode = TG.MENU_COD)  -- 카테고리명		");
                sbQuery.Append("\n        LEFT JOIN MENU_COD       TH ON (TA.GenreCode    = TH.MENU_COD)  -- 장르명			");
                sbQuery.Append("\n where TA.FileState <> '90' -- 파일삭제는 제외함					");
                sbQuery.Append("\n and   TA.AdState   <> '40' -- 광고상태 종료는 제외함				");

                // 검색어가 있으면
                if (adStatusModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append(" AND (	TA.FileName    LIKE '%'+ @SearchKey + '%'	");
                    sbQuery.Append("\n OR TA.ItemName    LIKE '%'+ @SearchKey + '%'		");
                    sbQuery.Append("\n OR TG.MenuName    LIKE '%'+ @SearchKey + '%'	");
                    sbQuery.Append("\n OR TH.MenuName    LIKE '%'+ @SearchKey + '%'	");
                    sbQuery.Append("\n OR TA.ChannelNo   LIKE '%'+ @SearchKey + '%'		");
					sbQuery.Append("\n OR TA.Title       LIKE '%'+ @SearchKey + '%'			");
                    sbQuery.Append("\n )																	");
                }

                sbQuery.Append(" ORDER BY ORD, CategoryCode, GenreCode, ChannelNo, ScheduleOrder, FileName  \n");
                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                byte[] data = FrameSystem.CompressData(ds);

                //adStatusModel.AdStatusDataSet = ds;
                //adStatusModel.AdStatusDataSet.RemotingFormat = SerializationFormat.Binary;

                adStatusModel.FileName = Convert.ToBase64String(data, 0, data.Length);
                adStatusModel.ResultCnt = ds.Tables[0].Rows.Count;
                adStatusModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + adStatusModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdStatusList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                adStatusModel.ResultCD = "3000";
                adStatusModel.ResultDesc = "광고파일 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
	}
}