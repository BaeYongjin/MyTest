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
	/// SchExcelBiz에 대한 요약 설명입니다.
	/// </summary>
	public class SchExcelBiz : BaseBiz
	{
		public SchExcelBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		/// <summary>
		/// 광고파일 목록조회
		/// </summary>
		/// <param name="schExcelModel"></param>
		public void GetExcelList(HeaderModel header, SchExcelModel schExcelModel)
		{
			//bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetExcelList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey              :[" + schExcelModel.SearchKey            + "]");		// 검색 어
				_log.Debug("SearchMediaCode	       :[" + schExcelModel.SearchMediaCode      + "]");		// 검색 매체				

				// __DEBUG__

				SqlParameter[] sqlParams = new SqlParameter[2];
		    
				int i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"          , SqlDbType.Int          );
				sqlParams[i++] = new SqlParameter("@SearchKey"          , SqlDbType.VarChar,  50 );

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32( schExcelModel.SearchMediaCode);
				sqlParams[i++].Value = schExcelModel.SearchKey ;


				StringBuilder sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT  TA.MediaCode                                                    \n"
					+ "        ,TA.ORD                                                          \n"
					+ "        ,TA.Flag                                                         \n"
					+ "        ,TA.CategoryCode                                                 \n"
					+ "        ,ISNULL(TG.MenuName,'') AS CategoryName                          \n"
					+ "        ,TA.GenreCode                                                    \n"
					+ "        ,ISNULL(TH.MenuName,'') AS GenreName                             \n"
					+ "        ,TA.ChannelNo                                                    \n"
					+ "        ,TA.Title                                                        \n"
					+ "        ,TA.ItemNo                                                       \n"
					+ "        ,TA.ItemName                                                     \n"
					+ "        ,TB.CodeName AS AdStateName                                      \n"
					+ "        ,TA.FileName                                                     \n"
					+ "        ,TA.FileType                                                     \n"
					+ "        ,TC.CodeName AS FileTypeName                                     \n"
					+ "        ,TA.FileState                                                    \n"
					+ "        ,TD.CodeName AS FileStateName                                    \n"
					+ "        ,TA.FileLength                                                   \n"
					+ "        ,TA.FilePath                                                     \n"
					+ "        ,TA.DownLevel                                                    \n"
					+ "        ,CONVERT(VarChar(3),TA.DownLevel) +  ' 순위' AS DownLevelName    \n"
					+ "        ,TA.ScheduleOrder                                                \n"
					+ "        ,TA.AdType                                                       \n"
					+ "        ,TE.CodeName AS AdTypeName                                       \n"
					+ "        ,TA.AckNo                                                        \n"
					+ "        ,TF.State AS AckState                                            \n"
					+ "  FROM                                                                   \n"
					+ "(                                                                        \n"
					+ "  SELECT B.MediaCode                                                     \n"
					+ "        ,'0' AS ORD                                                      \n"
					+ "        ,'홈' AS Flag                                                    \n"
					+ "        ,0 AS CategoryCode                                               \n"
					+ "        ,0 AS GenreCode                                                  \n"
					+ "        ,0 AS ChannelNo                                                  \n"
					+ "        ,'' AS Title                                                     \n"
					+ "        ,A.ItemNo                                                        \n"
					+ "        ,A.ScheduleOrder                                                 \n"
					+ "        ,A.AckNo                                                         \n"
					+ "        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType   \n"
					+ "    FROM SchHome A INNER JOIN ContractItem B ON (A.ItemNo = B.ItemNo AND B.MediaCode = @MediaCode)                  \n"
					+ "                                                                         \n"
					+ "  UNION                                                                  \n"
					+ "                                                                         \n"
					+ "  SELECT B.MediaCode                                                     \n"
					+ "        ,'1' AS ORD                                                      \n"
					+ "        ,'메뉴' AS Flag                                                  \n"
					+ "        ,C.UpperMenuCode AS CategoryCode                                 \n"
					+ "        ,A.GenreCode                                                     \n"
					+ "        ,0 AS ChannelNo                                                  \n"
					+ "        ,'' AS Title                                                     \n"
					+ "        ,A.ItemNo                                                        \n"
					+ "        ,A.ScheduleOrder                                                 \n"
					+ "        ,A.AckNo                                                         \n"
					+ "        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType   \n"
					+ "   FROM SchChoiceMenuDetail A INNER JOIN ContractItem B ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode) \n"
					+ "                              INNER JOIN Menu         C ON (A.GenreCode = C.MenuCode  AND C.MediaCode = @MediaCode) \n"
					+ "   UNION                                                                 \n"
					+ "                                                                         \n"
					+ " SELECT B.MediaCode                                                      \n"
					+ "       ,'2' AS ORD                                                       \n"
					+ "       ,'채널' AS Flag                                                   \n"
					+ "       ,C.Category AS CatagoryCode                                       \n"
					+ "       ,C.Genre    AS GenreCode                                          \n"
					+ "       ,A.ChannelNo                                                      \n"
					+ "       ,C.ProgramNm as Title                                             \n"
					+ "       ,A.ItemNo                                                         \n"
					+ "       ,A.ScheduleOrder                                                  \n"
					+ "       ,A.AckNo                                                          \n"
					+ "       ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType       \n"
					+ "   FROM SchChoiceChannelDetail A INNER JOIN ContractItem B ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode) \n"
					+ "                                 INNER JOIN Program      C ON (A.ChannelNo = C.Channel   AND C.MediaCode = @MediaCode) \n"
					+ "                                                                                                                       \n"                                                  
					+ " ) TA   LEFT JOIN SystemCode TB ON (TA.AdState      = TB.Code     AND TB.Section = '25') -- 광고상태                   \n"
					+ "        LEFT JOIN SystemCode TC ON (TA.FileType     = TC.Code     AND TC.Section = '24') -- 파일구분                   \n"
					+ "        LEFT JOIN SystemCode TD ON (TA.FileState    = TD.Code     AND TD.Section = '31') -- 파일상태                   \n"
					+ "        LEFT JOIN SystemCode TE ON (TA.AdType       = TE.Code     AND TE.Section = '26') -- 광고종류                   \n"
					+ "        LEFT JOIN SchPublish TF ON (TA.AckNo        = TF.AckNo    AND TF.MediaCode = @MediaCode)  -- 승인상태          \n"
					+ "        LEFT JOIN Menu       TG ON (TA.CategoryCode = TG.MenuCode AND TG.MediaCode = @MediaCode)  -- 카테고리명        \n"
					+ "        LEFT JOIN Menu       TH ON (TA.GenreCode    = TH.MenuCode AND TH.MediaCode = @MediaCode)  -- 장르명            \n"
					);

				// 검색어가 있으면
				if (schExcelModel.SearchKey.Trim().Length > 0)
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
				schExcelModel.SchExcelDataSet = ds.Copy();
				// 결과
				schExcelModel.ResultCnt = Utility.GetDatasetCount(schExcelModel.SchExcelDataSet);
				// 결과코드 셋트
				schExcelModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schExcelModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetExcelList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schExcelModel.ResultCD = "3000";
				schExcelModel.ResultDesc = "광고파일 조회중 오류가 발생하였습니다";
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