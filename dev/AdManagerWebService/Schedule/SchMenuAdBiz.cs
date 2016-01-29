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

namespace AdManagerWebService.Schedule
{
    public class SchMenuAdBiz : BaseBiz
    {
        public SchMenuAdBiz() : base(FrameSystem.connDbString, true)
        {
            _log	= FrameSystem.oLog;
        }

		#region 메뉴 목록조회
		/// <summary>
		/// 메뉴 목록조회
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		/// 
		public void GetMenuList(HeaderModel header, SchMenuAdModel schMenuAdModel)
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
				_log.Debug("SearchMediaCode	       :[" + schMenuAdModel.SearchMediaCode      + "]");		// 검색 매체				

				// __DEBUG__

				SqlParameter[] sqlParams = new SqlParameter[1];
		    
				int i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"          , SqlDbType.Int          );

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32( schMenuAdModel.SearchMediaCode);

				StringBuilder sbQuery = new StringBuilder();				
			
				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn										  \n"
					+ "		 ,TA.MediaCode                                                \n"
					+ "      ,TB.MediaName                                                \n"                                                
					+ "      ,TA.CategoryCode                                             \n" 
					+ "      ,TC.MenuName AS CategoryName                                 \n"
					+ "      ,TA.GenreCode                                                \n"
					+ "      ,TD.MenuName AS GenreName                                    \n"
					+ "      ,TA.AdCount                                                  \n"
					+ "  FROM                                                             \n"
					+ "  (    SELECT A.MediaCode                                          \n"                      
					+ "             ,B.UpperMenuCode AS CategoryCode                      \n"
					+ "             ,A.GenreCode                                          \n"
					+ "             ,COUNT(*) AS AdCount                                  \n"
					+ "        FROM SchChoiceMenuDetail A INNER JOIN Menu B ON (A.GenreCode = B.MenuCode  AND A.MediaCode = B.MediaCode ) \n"
					+ "									  INNER JOIN Category C ON (B.UpperMenuCode = C.CategoryCode  AND B.MediaCode = C.MediaCode ) \n"
					+ "       WHERE A.MediaCode = @MediaCode                              \n"
					+ "			AND C.CSSFlag = 'Y' AND C.Flag = 'Y'		              \n"
					+ "       GROUP BY A.MediaCode, B.UpperMenuCode, A.GenreCode          \n"
					+ " ) TA LEFT JOIN Media   TB ON (TA.MediaCode    = TB.MediaCode   )  \n"
					+ "      LEFT JOIN Menu    TC ON (TA.CategoryCode = TC.MenuCode AND TA.MediaCode = TC.MediaCode)  -- 카테고리명 \n" 
					+ "      LEFT JOIN Menu    TD ON (TA.GenreCode    = TD.MenuCode AND TA.MediaCode = TD.MediaCode)  -- 장르명     \n"
					+ "ORDER BY TA.MediaCode, TA.CategoryCode, TA.GenreCode               \n"
					);
											
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// 결과 DataSet의 모델에 복사
				schMenuAdModel.MenuAdDataSet = ds.Copy();
				// 결과
				schMenuAdModel.ResultCnt = Utility.GetDatasetCount(schMenuAdModel.MenuAdDataSet);
				// 결과코드 셋트
				schMenuAdModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schMenuAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() End");
				_log.Debug("-----------------------------------------");


			}
			catch(Exception ex)
			{
				schMenuAdModel.ResultCD = "3000";
				schMenuAdModel.ResultDesc = "메뉴정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

		}

		#endregion

		#region [MOB] 재핑광고 편성대상조회
		public void GetContractItemList(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey            :[" + schMenuAdModel.SearchKey            + "]");		// 검색어
				_log.Debug("SearchMediaCode	     :[" + schMenuAdModel.SearchMediaCode	   + "]");		// 검색 매체

				// __DEBUG__
				StringBuilder sbQuery = new StringBuilder();
				// 재핑광고는 Channel카운트만 사용한다.
				sbQuery.Append("\n SELECT 'False'                                                      AS CheckYn");
				sbQuery.Append("\n     ,   A.ITEM_NO                                                   AS ItemNo");
				sbQuery.Append("\n     ,   A.ITEM_NM                                                   AS ItemName");
				sbQuery.Append("\n     ,   B.CNTR_SEQ                                                  AS ContractSeq");
				sbQuery.Append("\n     ,   B.CNTR_NM                                                   AS ContractName");
				sbQuery.Append("\n     ,   C.ADVTER_NM                                                 AS AdvertiserName");
				sbQuery.Append("\n     ,   A.BEGIN_DY                                                  AS ExcuteStartDay");
				sbQuery.Append("\n     ,   A.END_DY                                                    AS ExcuteEndDay");
				sbQuery.Append("\n     ,   A.RL_END_DY                                                 AS RealEndDay");
				sbQuery.Append("\n     ,   A.ADVT_STT                                                  AS AdState");
				sbQuery.Append("\n     ,   GET_STMCODNM('25', A.ADVT_STT)                              AS AdStateName");
				sbQuery.Append("\n     ,   0                                                           AS HomeCount");
				sbQuery.Append("\n     ,   0														   AS MenuCount");
				sbQuery.Append("\n     , ( SELECT COUNT(*) FROM SCHD_CHNL  WHERE ITEM_NO = A.ITEM_NO)  AS ChannelCount");
				sbQuery.Append("\n     ,   0														   AS SeriesCount");
				sbQuery.Append("\n     ,   TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS')                    AS NowDay");
				sbQuery.Append("\n     ,   A.ADVT_TYP                                                  AS AdType");
				sbQuery.Append("\n     ,   GET_STMCODNM('26', A.ADVT_TYP)                              AS AdTypeName");
				sbQuery.Append("\n FROM    ADVT_MST A ");
				sbQuery.Append("\n     INNER JOIN  CNTR    B ON    B.CNTR_SEQ = A.CNTR_SEQ");
				sbQuery.Append("\n     LEFT  JOIN  ADVTER  C ON    C.ADVTER_COD = B.ADVTER_COD");
				sbQuery.Append("\n WHERE A.ADVT_TYP = '31'");

				bool isState = false;
				// 광고상태 준비
				if(schMenuAdModel.SearchchkAdState_10.Trim().Length > 0 && schMenuAdModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND ( A.ADVT_STT  = '10' \n");
					isState = true;
				}	
				// 광고상태 편성
				if(schMenuAdModel.SearchchkAdState_20.Trim().Length > 0 && schMenuAdModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.ADVT_STT  = '20' \n");
					isState = true;
				}	
				// 광고상태 중지
				if(schMenuAdModel.SearchchkAdState_30.Trim().Length > 0 && schMenuAdModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.ADVT_STT  = '30' \n");
					isState = true;
				}	
				// 광고상태 종료
				if(schMenuAdModel.SearchchkAdState_40.Trim().Length > 0 && schMenuAdModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.ADVT_STT  = '40' \n");
					isState = true;
				}	
				if(isState) sbQuery.Append(" ) \n");

				// 검색어가 있으면
				if (schMenuAdModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("\n"
						+ "  AND ( A.ITEM_NM	LIKE '%" + schMenuAdModel.SearchKey.Trim() + "%' \n"
						+ "     OR B.CNTR_NM	LIKE '%" + schMenuAdModel.SearchKey.Trim() + "%' \n"
						+ "  ) \n");
				}
       
				sbQuery.Append("  ORDER BY A.ITEM_NO DESC ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				schMenuAdModel.MenuAdDataSet = ds.Copy();
				schMenuAdModel.ResultCnt = Utility.GetDatasetCount(schMenuAdModel.MenuAdDataSet);
				schMenuAdModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schMenuAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schMenuAdModel.ResultCD = "3000";
				schMenuAdModel.ResultDesc = "지정광고 편성대상조회 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region [MOB] 광고별 편성내역 조회
		/// <summary>
		/// 광고별 편성내역 조회
		/// </summary>
		/// <param name="header"></param>
		/// <param name="schMenuAdModel"></param>
		public void GetItemScheduleList(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();
				// DEBUG
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetItemScheduleList() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("<입력정보>");
				_log.Debug("ItemNo	:[" + schMenuAdModel.ItemNo + "]");		// 검색어

				StringBuilder sbQuery = new StringBuilder();
				sbQuery.Append("\n");
				sbQuery.Append("\n SELECT  'False'		AS  CheckYn");
				sbQuery.Append("\n     ,   B.GNR_COD	AS  Category");
				sbQuery.Append("\n     ,   C.STM_COD_NM AS CategoryName");
				sbQuery.Append("\n     ,   A.CH_NO      AS Genre");
				sbQuery.Append("\n     ,   B.CH_NM      AS GenreName");
				sbQuery.Append("\n     ,   ''	        AS Channel");
				sbQuery.Append("\n     ,   ''           AS ChannelName");
				sbQuery.Append("\n     ,   SVC_ID       AS Series");
				sbQuery.Append("\n     ,   ''           AS SeriesName");
				sbQuery.Append("\n     ,   D.ACK_STT    AS AckNo");
				sbQuery.Append("\n     ,   '1'          AS SchType");
				sbQuery.Append("\n FROM    SCHD_CHNL A");
				sbQuery.Append("\n     LEFT  JOIN CHNL_COD B ON B.CH_NO = A.CH_NO");
				sbQuery.Append("\n     LEFT  JOIN STM_COD  C ON C.STM_COD = B.GNR_COD AND C.STM_COD_CLS = '78'");
				sbQuery.Append("\n     LEFT  JOIN SCHD_DIST_MST D ON D.ACK_NO = A.ACK_NO");
				sbQuery.AppendFormat("\n WHERE A.ITEM_NO = {0}",schMenuAdModel.ItemNo);
				sbQuery.Append("\n ORDER BY A.CH_NO");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				schMenuAdModel.ItemScheduleDataSet = ds.Copy();
				schMenuAdModel.ResultCnt = Utility.GetDatasetCount(schMenuAdModel.ItemScheduleDataSet);
				schMenuAdModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schMenuAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetItemScheduleList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schMenuAdModel.ResultCD = "3000";
				schMenuAdModel.ResultDesc = "지정광고 편성대상조회 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}
		#endregion

		#region [S1]지정광고 상세편성 조회

		/// <summary>
		/// 지정메뉴광고 상세편성 조회
		/// </summary>
		/// <returns></returns>
		public void GetSchChoiceMenuDetailList(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".GetSchChoiceMenuDetailList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("ItemNo            :[" + schMenuAdModel.ItemNo + "]");
				// __DEBUG__
				
				// 쿼리실행
				int i = 0;
				StringBuilder  sbQuery   = new StringBuilder();
				SqlParameter[] sqlParams = new SqlParameter[1];
	
				i = 0;
				sqlParams[i++] = new SqlParameter("@ItemNo"       , SqlDbType.Int       );

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(schMenuAdModel.ItemNo);

				// 지정채널 편성 테이블에 추가
				sbQuery.Append( "\n"
					+ "SELECT 'False' AS CheckYn                        \n"
					+ "      ,(SELECT CategoryCode					    \n"	
					+ "                  FROM Category                  \n"
					+ "                WHERE MediaCode    = A.MediaCode \n"	
					+ "                AND CategoryCode = (SELECT TOP 1 CategoryCode FROM ChannelSet WHERE GenreCode = A.GenreCode) \n"
					+ "       ) AS CatagoryCode                             \n"
					+ "      ,(SELECT (SPACE(5 - LEN(CONVERT(VARCHAR(5),CategoryCode))) +  CONVERT(VARCHAR(10),CategoryCode) + ' ' + CategoryName)  \n"	
					+ "                  FROM Category                  \n"
					+ "                WHERE MediaCode    = A.MediaCode \n"	
					+ "                AND CategoryCode = (SELECT TOP 1 CategoryCode FROM ChannelSet WHERE GenreCode = A.GenreCode) \n"
					+ "       ) AS Catagory                             \n"
					+ "      ,(SELECT (SPACE(5 - LEN(CONVERT(VARCHAR(5),GenreCode)))    +  CONVERT(VARCHAR(10),GenreCode)    + ' ' + GenreName   )  \n"
					+ "          FROM Genre                             \n"
					+ "         WHERE MediaCode = A.MediaCode           \n"
					+ "           AND GenreCode = A.GenreCode           \n"
					+ "       ) AS Genre                                \n"
					+ "      ,A.GenreCode                               \n"
					+ "      ,C.State AS AckState                       \n"
					+ "  FROM SchChoiceMenuDetail A INNER JOIN ContractItem B ON (A.MediaCode = B.MediaCode AND A.ItemNo = B.ItemNo) \n"
					+ "                              LEFT JOIN SchPublish   C ON (A.AckNo      = C.AckNo)                            \n"
					+ " WHERE A.ItemNo = @ItemNo  \n"
					+ " ORDER BY Catagory, Genre  \n"
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

				// 결과 DataSet의 모델에 복사
				schMenuAdModel.MenuDataSet = ds.Copy();
				// 결과
				schMenuAdModel.ResultCnt = Utility.GetDatasetCount(schMenuAdModel.MenuDataSet);
				schMenuAdModel.ResultCD = "0000";  // 정상

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schMenuAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".GetSchChoiceMenuDetailList() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schMenuAdModel.ResultCD   = "3101";
				schMenuAdModel.ResultDesc = "지정메뉴광고 상세편성 조회 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion

		#region [S1] 특정광고 편성데이터 삭제

		/// <summary>
		/// 특정광고에 선택한 편성데이터 삭제[장르/채널/회차 별 삭제처리 지원]
		/// </summary>
		/// <param name="header"></param>
		/// <param name="data"></param>
		public void SetSchedulePerItemDelete(HeaderModel header,  SchedulePerItemModel	data)
		{
			try
			{  
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchedulePerItemDelete() Start");
				_log.Debug("-----------------------------------------");

				#region [ 파라메터 설정 ]
				StringBuilder	sb		= new StringBuilder();
				SqlParameter[]	param	= new SqlParameter[5];

				param[0] = new SqlParameter("@ItemNo"	,SqlDbType.Int	);
				param[1] = new SqlParameter("@Media"	,SqlDbType.Int	);
				param[2] = new SqlParameter("@Genre"	,SqlDbType.Int	);
				param[3] = new SqlParameter("@Channel"	,SqlDbType.Int	);
				param[4] = new SqlParameter("@Series"	,SqlDbType.Int	);

				param[0].Value	=	data.ItemNo;
				param[1].Value	=	data.Media;
				param[2].Value	=	data.Genre;
				param[3].Value	=	data.Channel;
				param[4].Value	=	data.Series;
				#endregion

				#region [ 쿼리문 작성 ]
				/*
				 * 작업타입으로 장르/채널/회차 편성에 맞는 데이터를 삭제한다.
				 */
				if( data.DeleteJobType == TYPE_ScheduleDelete.Genre )
				{
					sb.Append("\n");
					sb.Append(" delete	from	SchChoiceMenuDetail	\n");
					sb.Append(" where	ItemNo		= @ItemNo	\n");
					sb.Append(" and		MediaCode	= @Media	\n");
					sb.Append(" and		GenreCode	= @Genre;	\n");
				}
				else if( data.DeleteJobType == TYPE_ScheduleDelete.Channel )
				{
					sb.Append("\n");
					sb.Append(" delete	from	SchChoiceChannelDetail	\n");
					sb.Append(" where	ItemNo		= @ItemNo	\n");
					sb.Append(" and		MediaCode	= @Media	\n");
					sb.Append(" and		ChannelNo	= @Channel;	\n");
				}
				else if( data.DeleteJobType == TYPE_ScheduleDelete.Series )
				{
					sb.Append("\n");
					sb.Append(" delete	from	SchChoiceSeriesDetail	\n");
					sb.Append(" where	ItemNo		= @ItemNo	\n");
					sb.Append(" and		MediaCode	= @Media	\n");
					sb.Append(" and		ChannelNo	= @Channel	\n");
					sb.Append(" and		SeriesNo	= @Series;	\n");
				}
				else
				{
					data.ResultCD	=	"3000";
					data.ResultCnt	=	0;
					data.ResultDesc	=	"작업타입이 사용되지 않는 타입입니다.";
					return;
				}
				#endregion

				#region [ 퀴리문 실행 및 결과 처리 ]

				_db.Open();
				int	rc = _db.ExecuteNonQueryParams( sb.ToString(), param );

				if( rc > 0 )
				{
					data.ResultCD	=	"0000";
					//data.ResultCnt	=	rc;
					// _tracking테이블의 영향으로 @RowCount가 2배로 넘어옴
					data.ResultCnt = rc / 2;
					data.ResultDesc	=	rc.ToString() + "건의 편성정보를 삭제하였습니다.";
				}
				else
				{
					data.ResultCD	=	"0000";
					data.ResultCnt	=	0;
					data.ResultDesc	=	"DB작업은 성공하였으나, 삭제된 데이터가 존재하지 않습니다.";
				}
				#endregion

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchedulePerItemDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				data.ResultCD	=	"3101";
				data.ResultDesc	=	"편성정보 삭제 오류 발생, 로그를 확인하십시요.";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

		#endregion

		#region [S1] 특정광고 편성데이터 추가

		/// <summary>
		/// 특정광고에 선택한 편성데이터 추가한다[장르/채널/회차 별 추가처리 지원]
		/// </summary>
		/// <param name="header"></param>
		/// <param name="data"></param>
		public void SetSchedulePerItemInsert(HeaderModel header,  SchedulePerItemModel	data)
		{
			bool	isFound = false;
			try
			{  
				_db.Open();
				_db.BeginTran();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchedulePerItemInsert() Start");
				_log.Debug("-----------------------------------------");

				#region [ 파라메터 설정 ]
				int	AckNo	=	GetLastAckNo( data.Media, 10 );
				StringBuilder	sb		= new StringBuilder();
				SqlParameter[]	param	= new SqlParameter[6];

				param[0] = new SqlParameter("@ItemNo"	,SqlDbType.Int	);
				param[1] = new SqlParameter("@Media"	,SqlDbType.Int	);
				param[2] = new SqlParameter("@Genre"	,SqlDbType.Int	);
				param[3] = new SqlParameter("@Channel"	,SqlDbType.Int	);
				param[4] = new SqlParameter("@Series"	,SqlDbType.Int	);
				param[5] = new SqlParameter("@AckNo"	,SqlDbType.Int	);

				param[0].Value	=	data.ItemNo;
				param[1].Value	=	data.Media;
				param[2].Value	=	data.Genre;
				param[3].Value	=	data.Channel;
				param[4].Value	=	data.Series;
				param[5].Value	=	AckNo;
				#endregion

				#region [ 쿼리문 작성 ]
				/*
				 * 작업타입으로 장르/채널/회차 편성에 맞는 데이터를 삭제한다.
				 */
				if( data.DeleteJobType == TYPE_ScheduleDelete.Genre )
				{
					isFound = IsExistMenu( data.ItemNo, data.Media, data.Genre );

					sb.Append("\n");
					sb.Append(" insert into SchChoiceMenuDetail	( ItemNo, MediaCode, GenreCode, AckNo, ScheduleOrder ) \n");
					sb.Append(" values ( @ItemNo, @Media,  @Genre, @AckNo, 0 ); \n");
				}
				else if( data.DeleteJobType == TYPE_ScheduleDelete.Channel )
				{
					isFound = IsExistChannel( data.ItemNo, data.Media, data.Channel );

					sb.Append("\n");
					sb.Append(" insert into SchChoiceChannelDetail	( ItemNo, MediaCode, ChannelNo, AckNo, ScheduleOrder ) \n");
					sb.Append(" values ( @ItemNo, @Media,  @Channel, @AckNo, 0 ); \n");
				}
				else if( data.DeleteJobType == TYPE_ScheduleDelete.Series )
				{
					isFound = IsExistSeries( data.ItemNo, data.Media, data.Channel, data.Series );

					sb.Append("\n");
					sb.Append(" insert into SchChoiceSeriesDetail	( ItemNo, MediaCode, ChannelNo, SeriesNo, AckNo, ScheduleOrder ) \n");
					sb.Append(" values ( @ItemNo, @Media,  @Channel, @Series, @AckNo, 0 ); \n");
				}
				else
				{
					data.ResultCD	=	"3000";
					data.ResultCnt	=	0;
					data.ResultDesc	=	"작업타입이 사용되지 않는 타입입니다.";
					return;
				}
				#endregion

				#region [ 퀴리문 실행 및 결과 처리 ]
				if( isFound == false )
				{
					int	rc = _db.ExecuteNonQueryParams( sb.ToString(), param );

					if( rc > 0 )
					{
						SetItemActive( data.ItemNo, header.UserID );
						data.ResultCD	=	"0000";
						//data.ResultCnt	=	rc;
						// _tracking테이블의 영향으로 @RowCount가 2배로 넘어옴
						data.ResultCnt = rc / 2;
						data.ResultDesc	=	rc.ToString() + "건의 편성정보를 추가하였습니다.";
					}
					else
					{
						data.ResultCD	=	"0000";
						data.ResultCnt	=	0;
						data.ResultDesc	=	"DB작업은 성공하였으나, 추가된 데이터가 존재하지 않습니다.";
					}
				}
				else
				{
					data.ResultCD	=	"0000";
					data.ResultCnt	=	0;
					data.ResultDesc	=	"DB작업은 성공하였으나, 이미 편성되어 있는 데이터 입니다";
				}
				#endregion

				_db.CommitTran();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchedulePerItemInsert() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				_db.RollbackTran();
				data.ResultCD	=	"3101";
				data.ResultDesc	=	"편성정보 추가 오류 발생, 로그를 확인하십시요.";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

		#endregion



		#region 지정메뉴광고 상세편성 조회

		/// <summary>
		/// 지정메뉴광고 상세편성 조회
		/// </summary>
		/// <returns></returns>
		public void GetSchChoiceMenuDetailContractSeq(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".GetSchChoiceMenuDetailContractSeq() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("ContractSeq            :[" + schMenuAdModel.ContractSeq + "]");
				// __DEBUG__
				
				// 쿼리실행
				int i = 0;
				StringBuilder  sbQuery   = new StringBuilder();
				SqlParameter[] sqlParams = new SqlParameter[1];
	
				i = 0;
				sqlParams[i++] = new SqlParameter("@ContractSeq"       , SqlDbType.Int       );

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(schMenuAdModel.ContractSeq);

				// 지정채널 편성 테이블에 추가
				sbQuery.Append( "\n"
					+ "SELECT A.ItemNo			                        \n"
					+ "      ,B.ItemName							    \n"	
					+ "      ,(SELECT CategoryCode					    \n"	
					+ "                  FROM Category                  \n"
					+ "                WHERE MediaCode    = A.MediaCode \n"	
					+ "                AND CategoryCode = (SELECT TOP 1 CategoryCode FROM ChannelSet WHERE GenreCode = A.GenreCode) \n"
					+ "       ) AS CatagoryCode                             \n"
					+ "      ,(SELECT (SPACE(5 - LEN(CONVERT(VARCHAR(5),CategoryCode))) +  CONVERT(VARCHAR(10),CategoryCode) + ' ' + CategoryName)  \n"	
					+ "                  FROM Category                  \n"
					+ "                WHERE MediaCode    = A.MediaCode \n"	
					+ "                AND CategoryCode = (SELECT TOP 1 CategoryCode FROM ChannelSet WHERE GenreCode = A.GenreCode) \n"
					+ "       ) AS CatagoryName                         \n"	//08.08.01수정
					+ "      ,(SELECT (SPACE(5 - LEN(CONVERT(VARCHAR(5),GenreCode)))    +  CONVERT(VARCHAR(10),GenreCode)    + ' ' + GenreName   )  \n"
					+ "          FROM Genre		                        \n"	
					+ "         WHERE MediaCode = A.MediaCode           \n"
					+ "           AND GenreCode = A.GenreCode           \n"
					+ "       ) AS GenreName                            \n"	//08.08.01수정
					+ "      ,A.GenreCode                               \n"
					+ "      ,C.State AS AckState                       \n"
					+ "  FROM SchChoiceMenuDetail A INNER JOIN ContractItem B ON (A.MediaCode = B.MediaCode AND A.ItemNo = B.ItemNo) \n"
					+ "                              LEFT JOIN SchPublish   C ON (A.AckNo      = C.AckNo)                            \n"
					//+ " WHERE B.ContractSeq = @ContractSeq  \n"
					+ " WHERE B.AdType = 13  \n"
					+ " ORDER BY CatagoryCode, GenreCode  \n"
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

				// 결과 DataSet의 모델에 복사
				schMenuAdModel.ChoiceMenuContractDataSet = ds.Copy();
				// 결과
				schMenuAdModel.ResultCnt = Utility.GetDatasetCount(schMenuAdModel.ChoiceMenuContractDataSet);
				schMenuAdModel.ResultCD = "0000";  // 정상

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schMenuAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".GetSchChoiceMenuDetailContractSeq() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schMenuAdModel.ResultCD   = "3101";
				schMenuAdModel.ResultDesc = "지정메뉴광고 상세편성 조회 중 오류가 발생하였습니다";
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
		/// <param name="schMenuAdModel"></param>
		public void GetChooseAdScheduleListMenu(HeaderModel header, SchMenuAdModel schMenuAdModel)
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
				_log.Debug("MediaCode    :[" + schMenuAdModel.MediaCode  + "]");
				_log.Debug("GenreCode    :[" + schMenuAdModel.GenreCode + "]");
				// __DEBUG__



				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT distinct					\n"
					+ "		 'M' AS ViewType              \n"
					+ "      ,A.CmType                     \n"
					+ "      ,CASE WHEN CmType = 'M' THEN '메뉴' ELSE '채널' END AS CmName \n"
					+ "      ,B.ContractSeq                \n" 
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
					+ "      ,K.Category                  \n" 
					+ "      ,'0' AS ChannelNo             \n" 
					+ "      ,B.FileState                  \n"
					+ "      ,I.CodeName AS FileStatename  \n"
					+ "      ,J.State AS AckState          \n"
					+ "      ,'False' AS CheckYn           \n"
					+ "  FROM (                            \n" 
					+ "         SELECT 'M' AS CmType, ItemNo, ScheduleOrder, GenreCode, AckNo, MediaCode \n"
					+ "           FROM SchChoiceMenuDetail with(NoLock)                       \n" 
					+ "          WHERE MediaCode = '" + schMenuAdModel.MediaCode  + "' \n"
					+ "            AND GenreCode = '" + schMenuAdModel.GenreCode  + "' \n"
					+ "       ) AS A    INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo         = B.ItemNo)                     \n"    
					+ "                 INNER JOIN Contract     C with(NoLock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq)  \n"
					+ "                 LEFT  JOIN Advertiser   E with(NoLock) ON (B.AdvertiserCode = E.AdvertiserCode)             \n"
					+ "                 LEFT  JOIN SystemCode   F with(NoLock) ON (C.State          = F.Code AND F.Section = '23')  \n"	// 23 : 계약상태
					+ "                 LEFT  JOIN SystemCode   G with(NoLock) ON (B.AdType         = G.Code AND G.Section = '26')  \n"	// 26 : 광고종류
					+ "                 LEFT  JOIN SystemCode   H with(NoLock) ON (B.AdState        = H.Code AND H.Section = '25')  \n"	// 25 : 광고상태
					+ "                 LEFT  JOIN SystemCode   I with(NoLock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 31 : 파일상태
					+ "                 LEFT  JOIN SchPublish   J with(NoLock) ON (A.AckNo          = J.AckNo)                      \n"
					+ "                 INNER JOIN Program K with(NoLock) ON (A.MediaCode	= K.MediaCode AND A.GenreCode	= K.Genre)                      \n"
					+ "	WHERE b.AdType = '13'							  \n"
					+ " ORDER BY CmType DESC, b.AdType, ScheduleOrder   \n"
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				schMenuAdModel.MenuAdDataSet = ds.Copy();

				ds.Dispose();

				//지정메뉴편성의 마지막  Order를 구함
				string LastOrder = "1";
				sbQuery = new StringBuilder();
				sbQuery.Append( "\n"
					+ "SELECT ISNULL(MAX(ScheduleOrder),0) AS MaxOrder                \n"
					+ "  FROM SchChoiceMenuDetail with(NoLock)                        \n"
					+ " WHERE MediaCode     = " + schMenuAdModel.MediaCode + " \n"
					+ "   AND GenreCode     = " + schMenuAdModel.GenreCode + " \n"
					);

				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					LastOrder = Utility.GetDatasetString(ds, 0, "MaxOrder");					
				}
				schMenuAdModel.LastOrder = LastOrder;
				ds.Dispose();

				// 결과
				schMenuAdModel.ResultCnt = Utility.GetDatasetCount(schMenuAdModel.MenuAdDataSet);
				// 결과코드 셋트
				schMenuAdModel.ResultCD = "0000";

				

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schMenuAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChooseAdScheduleListMenu() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schMenuAdModel.ResultCD = "3000";
				schMenuAdModel.ResultDesc = "메뉴별 편성현황 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion

		#region 메뉴별 편성현황 조회(계약번호로 카테고리코드를 알아오기 위해..)
		/// <summary>
		/// 메뉴/채널별 편성현황 조회
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetChooseAdScheduleListContract(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{

			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChooseAdScheduleListContract() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode    :[" + schMenuAdModel.MediaCode  + "]");
				_log.Debug("GenreCode    :[" + schMenuAdModel.GenreCode + "]");
				// __DEBUG__



				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT distinct					\n"
					+ "		 'M' AS ViewType              \n"
					+ "      ,A.CmType                     \n"
					+ "      ,CASE WHEN CmType = 'M' THEN '메뉴' ELSE '채널' END AS CmName \n"
					+ "      ,B.ContractSeq                \n" 
					+ "      ,B.AdType                     \n" 
					+ "      ,G.CodeName as AdTypeName     \n"
					+ "      ,A.ScheduleOrder              \n"
					+ "      ,A.ItemNo                     \n"  
					+ "      ,B.ItemName                   \n" 
					+ "      ,C.ContractName               \n" 
					+ "      ,E.AdvertiserName             \n" 
					+ "      ,C.State    AS ContState      \n"
					+ "      ,F.CodeName AS ContStateName  \n" 
					+ "      ,B.ExcuteStartDay             \n"
					+ "      ,B.RealEndDay                 \n"
					+ "      ,B.AdState                    \n"
					+ "      ,H.CodeName AS AdStatename    \n" 
					+ "      ,B.MediaCode                  \n" 
					+ "      ,A.GenreCode                  \n" 
					+ "      ,K.Category                  \n" 
					+ "      ,'0' AS ChannelNo             \n" 
					+ "      ,B.FileState                  \n"
					+ "      ,I.CodeName AS FileStatename  \n"
					+ "      ,J.State AS AckState          \n"
					+ "      ,'False' AS CheckYn           \n"
					+ "  FROM (                            \n" 
					+ "         SELECT 'M' AS CmType, ItemNo, ScheduleOrder, GenreCode, AckNo, MediaCode \n"
					+ "           FROM SchChoiceMenuDetail with(NoLock)                       \n" 
					+ "          WHERE MediaCode = '" + schMenuAdModel.MediaCode  + "' \n"					
					//+ "            AND GenreCode = '" + schMenuAdModel.GenreCode  + "' \n"					
					+ "       ) AS A    INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo         = B.ItemNo)                     \n"    
					+ "                 INNER JOIN Contract     C with(NoLock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq)  \n"
					+ "                 LEFT  JOIN Advertiser   E with(NoLock) ON (B.AdvertiserCode = E.AdvertiserCode)             \n"
					+ "                 LEFT  JOIN SystemCode   F with(NoLock) ON (C.State          = F.Code AND F.Section = '23')  \n"	// 23 : 계약상태
					+ "                 LEFT  JOIN SystemCode   G with(NoLock) ON (B.AdType         = G.Code AND G.Section = '26')  \n"	// 26 : 광고종류
					+ "                 LEFT  JOIN SystemCode   H with(NoLock) ON (B.AdState        = H.Code AND H.Section = '25')  \n"	// 25 : 광고상태
					+ "                 LEFT  JOIN SystemCode   I with(NoLock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 31 : 파일상태
					+ "                 LEFT  JOIN SchPublish   J with(NoLock) ON (A.AckNo          = J.AckNo)                      \n"
					+ "                 INNER JOIN Program K with(NoLock) ON (A.MediaCode	= K.MediaCode AND A.GenreCode	= K.Genre)                      \n"
					+ "	WHERE b.AdType = '13'							  \n"
					+ " ORDER BY RealEndDay DESC				      \n"
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				schMenuAdModel.MenuContractDataSet = ds.Copy();

				ds.Dispose();

				//지정메뉴편성의 마지막  Order를 구함
				string LastOrder = "1";
				sbQuery = new StringBuilder();
				sbQuery.Append( "\n"
					+ "SELECT ISNULL(MAX(ScheduleOrder),0) AS MaxOrder                \n"
					+ "  FROM SchChoiceMenuDetail with(NoLock)                        \n"
					+ " WHERE MediaCode     = " + schMenuAdModel.MediaCode + " \n"
					+ "   AND GenreCode     = " + schMenuAdModel.GenreCode + " \n"
					);

				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					LastOrder = Utility.GetDatasetString(ds, 0, "MaxOrder");					
				}
				schMenuAdModel.LastOrder = LastOrder;
				ds.Dispose();

				// 결과
				schMenuAdModel.ResultCnt = Utility.GetDatasetCount(schMenuAdModel.MenuContractDataSet);
				// 결과코드 셋트
				schMenuAdModel.ResultCD = "0000";

				

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schMenuAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChooseAdScheduleListContract() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schMenuAdModel.ResultCD = "3000";
				schMenuAdModel.ResultDesc = "메뉴별 편성현황 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion

		#region [S1] 현재승인상태의 승인번호를 구함
		/// <summary>
		/// 현재 승인번호를 가져온다
		/// 현재 승인번호의 상태가 30:배포승인이면 신규상태로 채번후 AckNo를 리턴한다
		/// </summary>
		/// <param name="MediaCode"></param>
		/// <param name="AdSchType">편성승인이 홈/상업/매체로 구분됨</param>
		/// <returns>편성중인 승인번호</returns>
		internal	int	GetLastAckNo(int MediaCode, int AdSchType)
		{
			int				AckNo		= 0;
			string			AckState	= "";
			StringBuilder	sbQuery		= new StringBuilder();				
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLastAckNo() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode	    :[" + MediaCode     + "]");		// 검색 매체
				_log.Debug("AdSchType		:[" + AdSchType     + "]");
			
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ " DECLARE @AckNo int, @AckState Char(2), @MediaCode int, @AdSchType int    \n"
					+ "                                                          \n"
					+ " SELECT @MediaCode = " + MediaCode                    + " \n"
					+ " SELECT @AdSchType = " + AdSchType                    + " \n"
					+ "                                                          \n"
					+ " SELECT TOP 1 @AckState = State, @AckNo = AckNo           \n"
					+ "   FROM SchPublish                                        \n"
					+ "  WHERE MediaCode = @MediaCode  AND	AdSchType=@AdSchType \n"
					+ "  ORDER BY AckNo DESC                                     \n"
					+ "                                                          \n"
					+ " IF @AckState = '30' OR @AckState IS NULL                 \n"
					+ " BEGIN                                                    \n"
					+ "	    INSERT INTO SchPublish                               \n"
					+ "                (AckNo, MediaCode, State, ModifyStartDay,AdSchType) \n"
					+ "         SELECT ISNULL(MAX(AckNo),0) + 1                  \n"
					+ "               ,@MediaCode                                \n"
					+ "               ,'10'                                      \n"
					+ "               ,GETDATE(), @AdSchType                     \n"
					+ "          FROM SchPublish                                 \n"
					+ "         WHERE MediaCode = @MediaCode                     \n"
					+ "                                                          \n"
					+ "     SELECT TOP 1 @AckState = State, @AckNo = AckNo       \n"
					+ "       FROM SchPublish                                    \n"
					+ "      WHERE MediaCode=@MediaCode AND	AdSchType=@AdSchType \n"
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
					AckNo    =  Convert.ToInt32( ds.Tables[0].Rows[0]["AckNo"].ToString() );
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

		#region [S1] 광고상태를 편성상태로 변경
		/// <summary>
		/// 광고상태를 편성상태로 변경한다.
		/// 편성 추가,삭제,수정시 광고상태가 편성상태가 아닌경우 편성상태로 변경한다
		/// </summary>
		/// <param name="itemNo">작업대상 광고</param>
		/// <param name="userId">변경작업자</param>
		internal	void SetItemActive( int itemNo, string userId )
		{

			StringBuilder	sbQuery		= new StringBuilder();				
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetItemActive() Start");
				_log.Debug("ItemNo	    :[" + itemNo     + "]");		// 검색 매체
				_log.Debug("UserId		:[" + userId     + "]");
			
				
				// 쿼리생성, 20보다 작은 경우에만 처리함
				sbQuery.Append( "\n"
					+ " UPDATE	ContractItem        \n"
					+ "    SET  AdState = '20'      \n"   // 광고상태 - 20:편성
					+ "        ,ModDt   = GETDATE() \n"
					+ "        ,RegID   = '" + userId + "' \n" 
					+ " WHERE	ItemNo	= " + itemNo + " \n"
					+ " AND		AdState	< '20';  \n");

				_log.Debug(sbQuery.ToString());

				int rc = _db.ExecuteNonQuery( sbQuery.ToString() );

				_log.Debug(this.ToString() + "SetItemActive() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}
		}
		#endregion

		#region [S1] 중복편성인지 확인함
		/// <summary>
		/// 장르편성시 중복여부 확인
		/// </summary>
		/// <param name="itemNo"></param>
		/// <param name="media"></param>
		/// <param name="genre"></param>
		/// <returns></returns>
		internal	bool IsExistMenu( int itemNo, int media, int genre )
		{
			bool	rtnValue = false;
			int		rows	 = 0;

			StringBuilder	sb	= new StringBuilder();				
			try
			{
				sb.Append(" select count(*) as cnt from SchChoiceMenuDetail noLock ");
				sb.Append(" where	ItemNo		= " + itemNo );
				sb.Append(" and		MediaCode	= " + media );
				sb.Append(" and		GenreCode	= " + genre );

				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sb.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					rows    =  Convert.ToInt32( ds.Tables[0].Rows[0]["cnt"].ToString() );
				}
				ds.Dispose();

				if( rows > 0 )
				{
					rtnValue = true;
				}
			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}
			return rtnValue;
		}


		/// <summary>
		/// 채널편성시 중복여부 확인
		/// </summary>
		/// <param name="itemNo"></param>
		/// <param name="media"></param>
		/// <param name="channel"></param>
		/// <returns></returns>
		internal	bool IsExistChannel( int itemNo, int media, int channel )
		{
			bool	rtnValue = false;
			int		rows	 = 0;

			StringBuilder	sb	= new StringBuilder();				
			try
			{
				sb.Append(" select count(*) as cnt from SchChoiceChannelDetail noLock ");
				sb.Append(" where	ItemNo		= " + itemNo );
				sb.Append(" and		MediaCode	= " + media );
				sb.Append(" and		ChannelNo	= " + channel );

				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sb.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					rows    =  Convert.ToInt32( ds.Tables[0].Rows[0]["cnt"].ToString() );
				}
				ds.Dispose();

				if( rows > 0 )
				{
					rtnValue = true;
				}
			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}
			return rtnValue;
		}

		/// <summary>
		/// 시리즈편성시 중복여부 확인
		/// </summary>
		/// <param name="itemNo"></param>
		/// <param name="media"></param>
		/// <param name="channel"></param>
		/// <param name="series"></param>
		/// <returns></returns>
		internal	bool IsExistSeries( int itemNo, int media, int channel ,int series)
		{
			bool	rtnValue = false;
			int		rows	 = 0;

			StringBuilder	sb	= new StringBuilder();				
			try
			{
				sb.Append(" select count(*) as cnt from SchChoiceSeriesDetail noLock ");
				sb.Append(" where	ItemNo		= " + itemNo );
				sb.Append(" and		MediaCode	= " + media );
				sb.Append(" and		ChannelNo	= " + channel );
				sb.Append(" and		SeriesNo	= " + series );

				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sb.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					rows    =  Convert.ToInt32( ds.Tables[0].Rows[0]["cnt"].ToString() );
				}
				ds.Dispose();

				if( rows > 0 )
				{
					rtnValue = true;
				}
			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}
			return rtnValue;
		}
		#endregion

    }
}