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
	/// SchAppendAdBiz에 대한 요약 설명입니다.
	/// </summary>
	public class SchAppendAdBiz : BaseBiz
	{
		#region 생성자
		public SchAppendAdBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region 홈광고편성편황조회
		/// <summary>
		/// 홈광고편성편황조회
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		public void GetSchAppendAdList(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSchAppendAdList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	     :[" + schAppendAdModel.SearchMediaCode	   + "]");		// 검색 매체

				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT B.MediaCode                   \n"
					+ "      ,B.AdType                      \n"
					+ "      ,H.CodeName AS AdTypeName      \n"
					+ "      ,A.ScheduleOrder               \n"
					+ "      ,A.ItemNo                      \n"
					+ "      ,B.ItemName                    \n"
					+ "      ,C.ContractName                \n"
					+ "      ,D.AdvertiserName              \n"
					+ "      ,C.State AS ContState          \n"
					+ "      ,E.CodeName AS ContStateName   \n"
					+ "      ,B.RealEndDay                  \n"
					+ "      ,F.CodeName AS AdClassName     \n"
					+ "      ,B.AdState                     \n"
					+ "      ,G.CodeName AS AdStatename     \n"
					+ "      ,B.FileState                   \n"
					+ "      ,I.CodeName AS FileStatename   \n"
					+ "      ,'False' AS CheckYn            \n"
					+ "      ,J.State AS AckState           \n"
					+ "      ,CASE K.TgtTimeYn WHEN 'Y' THEN '설정' WHEN 'N' THEN '설정' ELSE '' END AS TgtTimeYn           \n"
					+ "      ,M.CodeName AS JumpTypeName           \n"
					+ "  FROM SchAppend A with(NoLock) INNER JOIN ContractItem B  with(NoLock) ON (A.ItemNo         = B.ItemNo)                     \n"
					+ "                 INNER JOIN Contract     C  with(NoLock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq) \n"
					+ "                 LEFT  JOIN Advertiser   D  with(NoLock) ON (B.AdvertiserCode = D.AdvertiserCode)             \n"
					+ "                 LEFT  JOIN SystemCode   E  with(NoLock) ON (C.State          = E.Code AND E.Section = '23')  \n"	// 23 : 계약상태
					+ "                 LEFT  JOIN SystemCode   F  with(NoLock) ON (B.AdClass        = F.Code AND F.Section = '29')  \n"	// 29 : 광고용도
					+ "                 LEFT  JOIN SystemCode   G  with(NoLock) ON (B.AdState        = G.Code AND G.Section = '25')  \n"	// 25 : 광고상태
					+ "                 LEFT  JOIN SystemCode   H  with(NoLock) ON (B.AdType         = H.Code AND H.Section = '26')  \n"	// 26 : 광고종류
					+ "                 LEFT  JOIN SystemCode   I  with(NoLock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 31 : 파일상태
					+ "                 LEFT  JOIN SchPublish   J  with(NoLock) ON (A.AckNo          = J.AckNo)                      \n"
					+ "                 LEFT  JOIN TargetingHome    K  with(NoLock) ON (A.ItemNo          = K.ItemNo)                      \n"
					+ "                 LEFT  JOIN ChannelJump    L  with(NoLock) ON (A.ItemNo          = L.ItemNo)                      \n"
					+ "                 LEFT  JOIN SystemCode    M with(NoLock) ON (L.JumpType       = M.Code        AND M.Section   = '34' )                      \n"
					+ " WHERE 1 = 1   \n"
					);

				// 매체을 선택했으면
				if(schAppendAdModel.SearchMediaCode.Trim().Length > 0 && !schAppendAdModel.SearchMediaCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode  = " + schAppendAdModel.SearchMediaCode.Trim() + " \n");
				}	
				
       
				sbQuery.Append("  ORDER BY A.ScheduleOrder ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				schAppendAdModel.ScheduleDataSet = ds.Copy();

				//지정메뉴편성의 마지막  Order를 구함
				string LastOrder = "1";
				sbQuery = new StringBuilder();
				sbQuery.Append( "\n"
					+ "SELECT ISNULL(MAX(ScheduleOrder),0) AS MaxOrder   \n"
					+ "  FROM SchAppend with(NoLock)                              \n"
					+ " WHERE MediaCode = " + schAppendAdModel.SearchMediaCode + " \n"
					);

				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					LastOrder = Utility.GetDatasetString(ds, 0, "MaxOrder");					
				}
				schAppendAdModel.LastOrder = LastOrder;
				ds.Dispose();

				// 결과
				schAppendAdModel.ResultCnt = Utility.GetDatasetCount(schAppendAdModel.ScheduleDataSet);
				// 결과코드 셋트
				schAppendAdModel.ResultCD = "0000";


				// 2007.10.02 RH.Jun 파일리스트 건수검사용
				// 2007.10.10 RH.Jung 홈광고 리스트 합산시 파일상태가 셋탑삭제가 아닌것으로 변경

				sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT (                                                               \n"
					+ "  SELECT COUNT(*) AS HomeCnt                                           \n"
					+ "    FROM SchAppend A  with(NoLock) INNER JOIN ContractItem B  with(NoLock) ON (A.ItemNo = B.ItemNo)  \n"
					+ "     AND B.ExcuteStartDay	<= Convert(varchar(8),getdate(),112)      \n"
					+ "     AND B.RealEndDay		>= Convert(varchar(8),getdate(),112)      \n"
					+ "     AND B.AdState   = '20'    \n"            
					+ "     AND B.FileState < '90'    \n"
					+ ") + (                          \n"
					+ "  SELECT COUNT(*) AS FileCnt   \n"
					+ "    FROM ContractItem with(NoLock)          \n"
					+ "   WHERE FileState = '30'      \n"
					+ ") AS FileListCnt               \n"
					);           

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				schAppendAdModel.FileListCount = Convert.ToInt32(ds.Tables[0].Rows[0]["FileListCnt"].ToString());
				ds.Dispose();

				// 2007.10.02 

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schAppendAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSchAppendAdList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD = "3000";
				schAppendAdModel.ResultDesc = "홈광고편성현황 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		#endregion

		#region 추가광고편성대상조회

		/// <summary>
		/// 추가광고
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		public void GetContractItemList(HeaderModel header, SchAppendAdModel schAppendAdModel)
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
				_log.Debug("SearchKey            :[" + schAppendAdModel.SearchKey            + "]");		// 검색어
				_log.Debug("SearchMediaCode	     :[" + schAppendAdModel.SearchMediaCode	   + "]");		// 검색 매체

				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn            \n"
					+ "      ,A.ItemNo                      \n"
					+ "      ,A.ItemName                    \n"
					+ "      ,B.ContractName                \n"
					+ "      ,C.AdvertiserName              \n"
					+ "      ,A.ExcuteStartDay              \n"
					+ "      ,A.ExcuteEndDay                \n"
					+ "      ,A.RealEndDay                  \n"
					+ "      ,A.AdState                     \n"
					+ "      ,D.CodeName AdStateName        \n"
					+ "      ,(SELECT COUNT(*) FROM SchAppend with(NoLock)                 WHERE ItemNo = A.ItemNo) AS HomeCount      \n"
					+ "      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail with(NoLock)     WHERE ItemNo = A.ItemNo) AS MenuCount      \n"
					+ "      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock)  WHERE ItemNo = A.ItemNo) AS ChannelCount   \n"
					+ "      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay                                                \n"
					+ "      ,A.AdType                      \n"
					+ "      ,E.CodeName AS AdTypeName      \n"
					+ "  FROM ContractItem A with(NoLock) INNER JOIN Contract   B with(NoLock) ON (B.MediaCode      = B.MediaCode AND A.RapCode = B.RapCode AND A.AgencyCode = B.AgencyCode AND A.AdvertiserCode = B.AdvertiserCode AND A.ContractSeq = B.ContractSeq) \n"
					+ "                       LEFT JOIN Advertiser C with(NoLock) ON (B.AdvertiserCode = C.AdvertiserCode)                \n"
					+ "	                      LEFT JOIN SystemCode D with(NoLock) ON (A.AdState        = D.Code      AND D.Section = '25')  \n"  // 25 : 광고상태
					+ "                       LEFT JOIN SystemCode E with(NoLock) ON (A.AdType         = E.Code      AND E.Section = '26')  \n"	// 26 : 광고종류
					+ " WHERE 1=1   \n"    
					+ "   AND A.AdType  = '14'   \n"    
					);

				// 매체을 선택했으면
				if(schAppendAdModel.SearchMediaCode.Trim().Length > 0 && !schAppendAdModel.SearchMediaCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode  = '" + schAppendAdModel.SearchMediaCode.Trim() + "' \n");
				}	

				bool isState = false;
				// 광고상태 선택에 따라

				// 광고상태 준비
				if(schAppendAdModel.SearchchkAdState_10.Trim().Length > 0 && schAppendAdModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND ( A.AdState  = '10' \n");
					isState = true;
				}	
				// 광고상태 편성
				if(schAppendAdModel.SearchchkAdState_20.Trim().Length > 0 && schAppendAdModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '20' \n");
					isState = true;
				}	
				// 광고상태 중지
				if(schAppendAdModel.SearchchkAdState_30.Trim().Length > 0 && schAppendAdModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '30' \n");
					isState = true;
				}	
				// 광고상태 종료
				if(schAppendAdModel.SearchchkAdState_40.Trim().Length > 0 && schAppendAdModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				
				// 검색어가 있으면
				if (schAppendAdModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("\n"
						+ "  AND ( A.ItemName       LIKE '%" + schAppendAdModel.SearchKey.Trim() + "%' \n"
						+ "     OR B.ContractName   LIKE '%" + schAppendAdModel.SearchKey.Trim() + "%' \n"
						+ "  ) \n"
						);
				}
       
				sbQuery.Append("  ORDER BY A.ItemNo Desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				schAppendAdModel.ScheduleDataSet = ds.Copy();
				// 결과
				schAppendAdModel.ResultCnt = Utility.GetDatasetCount(schAppendAdModel.ScheduleDataSet);
				// 결과코드 셋트
				schAppendAdModel.ResultCD = "0000";


				// 2007.10.02 RH.Jun 파일리스트 건수검사용

				sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT (                                                               \n"
					+ "  SELECT COUNT(*) AS HomeCnt                                           \n"
					+ "    FROM SchAppend A INNER JOIN ContractItem B ON (A.ItemNo = B.ItemNo)  \n"
					+ "     AND B.ExcuteStartDay	<= Convert(varchar(8),getdate(),112)      \n"
					+ "     AND B.RealEndDay		>= Convert(varchar(8),getdate(),112)      \n"
					+ "     AND B.AdState   = '20'    \n"            
					+ "     AND B.FileState = '30'    \n"
					+ ") + (                          \n"
					+ "  SELECT COUNT(*) AS FileCnt   \n"
					+ "    FROM ContractItem          \n"
					+ "   WHERE FileState = '30'      \n"
					+ ") AS FileListCnt               \n"
					);           

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				schAppendAdModel.FileListCount = Convert.ToInt32(ds.Tables[0].Rows[0]["FileListCnt"].ToString());
				ds.Dispose();


				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schAppendAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD = "3000";
				schAppendAdModel.ResultDesc = "홈광고편성현황 조회 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		#endregion

		#region  홈광고편성 변경시 전체 row 최종수정일시 업데이터
		/// <summary>
		/// 홈광고편성 변경시 전체 row 최종수정일시 업데이터
		/// </summary>
		/// <returns></returns>
		private void SetLastUpdate(string MediaCode)
		{
			string sQuery = "\n"
				+ "UPDATE SchAppend             \n"
				+ "   SET ModDt   = GETDATE() \n"
				+ " WHERE MediaCode = " + MediaCode + " \n"
				;

			int rc =  _db.ExecuteNonQuery(sQuery);
		}

		#endregion

		#region 추가광고편성 생성
		/// <summary>
		/// 추가광고편성
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdCreate(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdCreate() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int i = 0;
					int rc = 0;

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					StringBuilder  sbQuery   = new StringBuilder();
					SqlParameter[] sqlParams = new SqlParameter[1];
		
					i = 0;
					sqlParams[i++] = new SqlParameter("@ItemNo"        , SqlDbType.Int          );

					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(schAppendAdModel.ItemNo);

					_db.BeginTran();

					// 홈광고 편성 테이블에 추가
					sbQuery.Append( "\n"
						+ "INSERT INTO SchAppend (                   \n"
						+ "       MediaCode                        \n"
						+ "      ,ScheduleOrder                    \n"
						+ "      ,ItemNo                           \n"
						+ "      ,ModDt                            \n"
						+ "      ,AckNo                            \n"
						+ "      )                                 \n"
						+ " SELECT                                 \n"					
						+ "       " + schAppendAdModel.MediaCode + " \n"
						+ "      ,ISNULL(MAX(ScheduleOrder),0)+1   \n"
						+ "      ,@ItemNo                          \n"
						+ "      ,GETDATE()                        \n"
						+ "      ," + AckNo                    + " \n"
						+ "  FROM SchAppend                          \n"
						+ " WHERE MediaCode = " + schAppendAdModel.MediaCode + " \n"
						);

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// 홈광고 편성 테이블에 추가
					sbQuery   = new StringBuilder();
					
					sbQuery.Append( "\n"
						+ "UPDATE ContractItem        \n"
						+ "   SET AdState = '20'      \n"   // 광고상태 - 20:편성
						+ "      ,ModDt   = GETDATE() \n"
						+ "      ,RegID   = '" + header.UserID + "' \n" 
						+ " WHERE ItemNo  = @ItemNo   \n"
						);

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// 최종수정일시 셋트
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// 편성한 순위
					string LastOrder = "1";

					// 해당 편성한 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder    \n"
						+ "   FROM SchAppend                                      \n"
						+ "  WHERE MediaCode = " + schAppendAdModel.MediaCode + " \n"
						);

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						LastOrder = Utility.GetDatasetString(ds, 0, "LastOrder");					
					}

					ds.Dispose();

					schAppendAdModel.ScheduleOrder = LastOrder;

					// __MESSAGE__
					_log.Message("홈광고편성:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = "홈광고편성 저장 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion 

		#region  홈광고편성 삭제
		/// <summary>
		/// 홈광고편성  삭제
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdDelete(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdDelete() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int i = 0;
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();
					SqlParameter[] sqlParams = new SqlParameter[2];
	
					i = 0;
					sqlParams[i++] = new SqlParameter("@ItemNo"          , SqlDbType.Int          );
					sqlParams[i++] = new SqlParameter("@ScheduleOrder"   , SqlDbType.Int          );

					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(schAppendAdModel.ItemNo);
					sqlParams[i++].Value = Convert.ToInt32(schAppendAdModel.ScheduleOrder);

					_db.BeginTran();

					// 2007.10.11 RH.Jung 삭제시에도 편성승인번호를 생성하기 위함
					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					// 홈광고 편성 테이블에 추가
					sbQuery.Append( "\n"
						+ "DELETE SchAppend                        \n"
						+ " WHERE ItemNo        = @ItemNo        \n"
						+ "   AND ScheduleOrder = @ScheduleOrder \n"
						);

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// 삭제된 편성정보의 순위 조정
					sbQuery   = new StringBuilder();

					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                            \n"
						+ "   SET ScheduleOrder = ScheduleOrder - 1  \n"
						+ " WHERE ScheduleOrder > " + schAppendAdModel.ScheduleOrder + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode     + " \n"	
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 최종수정일시 셋트
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// 편성한 순위
					string LastOrder = "1";

					// 해당 편성한 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder           \n"
						+ "  FROM SchAppend                                              \n"
						+ " WHERE ScheduleOrder < " + schAppendAdModel.ScheduleOrder + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode     + " \n"
						);

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						LastOrder = Utility.GetDatasetString(ds, 0, "LastOrder");					
					}

					ds.Dispose();

					schAppendAdModel.ScheduleOrder = LastOrder;

					// __MESSAGE__
					_log.Message("홈광고편성 삭제:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = "홈광고 편성내역 삭제 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		#endregion



		#region 홈광고편성 첫번째 순위로
		/// <summary>
		/// 홈광고편성  첫번째 순위로
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdOrderFirst(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderFirst() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = null;

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1"; 

					// 해당 매체중 MIN값
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS MinOrder     \n"
						+ "   FROM SchAppend                                      \n"
						+ "  WHERE MediaCode = " + schAppendAdModel.MediaCode + " \n"
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

					// 해당 순위를 0순위로 임시변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                \n"
						+ "   SET ScheduleOrder = 0                      \n"
						+ "      ,AckNo  = " + AckNo                 + " \n"
						+ " WHERE ItemNo = " + schAppendAdModel.ItemNo + " \n"
						+ "   AND ScheduleOrder = " + schAppendAdModel.ScheduleOrder + " \n"
						);

					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					// 삭제후 해당 순위보다 작은 순위의 내역들을 +1하여 조정
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                            \n"
						+ "   SET ScheduleOrder = ScheduleOrder + 1  \n"
						+ "      ,AckNo         = " + AckNo      + " \n"
						+ " WHERE MediaCode     = " + schAppendAdModel.MediaCode     + " \n"					     
						+ "   AND ScheduleOrder < " + schAppendAdModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                  \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                       \n"
						+ "   SET ScheduleOrder = " + ToOrder               + " \n"
						+ "      ,AckNo         = " + AckNo                 + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo + " \n"
						+ "   AND ScheduleOrder = 0  \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 최종수정일시 셋트
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("홈광고편성 첫번째 순위로 변경:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] 등록자:[" + header.UserID + "]");
					schAppendAdModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
					_log.Debug("ScheduleOrder:[" + schAppendAdModel.ScheduleOrder + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // 정상
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderFirst() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = " 홈광고편성  첫번째 순위로 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion

		#region 홈광고편성 순위올림
		/// <summary>
		/// 홈광고편성  순위올림
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdOrderUp(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderUp() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode     :[" + schAppendAdModel.MediaCode           + "]");		// 검색어
				_log.Debug("ScheduleOrder :[" + schAppendAdModel.ScheduleOrder	   + "]");		// 검색 매체
				_log.Debug("ItemNo        :[" + schAppendAdModel.ItemNo       + "]");		// 검색 랩
				// __DEBUG__

				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS UpOrder               \n"
						+ "  FROM SchAppend                                                \n"
						+ " WHERE ScheduleOrder < " + schAppendAdModel.ScheduleOrder   + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode       + " \n"
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

					// 해당 순위를 0순위로 임시변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                \n"
						+ "   SET ScheduleOrder = 0                      \n"
						+ "      ,AckNo  = " + AckNo                 + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo        + " \n"
						+ "   AND ScheduleOrder = " + schAppendAdModel.ScheduleOrder + " \n"
						);

					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					// 삭제후 해당 순위보다 변경할 순위의 내역을 해당 순위로 조정
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                               \n"
						+ "   SET ScheduleOrder = " + schAppendAdModel.ScheduleOrder  + " \n"
						+ "      ,AckNo         = " + AckNo                         + " \n"
						+ " WHERE ScheduleOrder = " + ToOrder                       + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode      + " \n"	
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                       \n"
						+ "   SET ScheduleOrder = " + ToOrder               + " \n"
						+ "      ,AckNo         = " + AckNo                 + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo + " \n"
						+ "   AND ScheduleOrder = 0 \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 최종수정일시 셋트
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("홈광고편성 순위올림 변경:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] 순위:[" + schAppendAdModel.ScheduleOrder + ">" + ToOrder + "] 등록자:[" + header.UserID + "]");
					schAppendAdModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // 정상
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderUp() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = " 홈광고편성 순위올림 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion

		#region 홈광고편성  순위내림
		/// <summary>
		/// 홈광고편성  순위내림
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdOrderDown(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderDown() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS DownOrder           \n"
						+ "  FROM SchAppend                                              \n"
						+ " WHERE ScheduleOrder > " + schAppendAdModel.ScheduleOrder + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode     + " \n"
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

					// 해당 순위를 0순위로 임시변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                \n"
						+ "   SET ScheduleOrder = 0                      \n"
						+ "      ,AckNo         = " + AckNo          + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo        + " \n"
						+ "   AND ScheduleOrder = " + schAppendAdModel.ScheduleOrder + " \n"
						);

					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					// 삭제후 해당 순위보다 변경할 순위의 내역을 해당 순위로 조정
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                              \n"
						+ "   SET ScheduleOrder = " + schAppendAdModel.ScheduleOrder + " \n"
						+ "      ,AckNo         = " + AckNo                        + " \n"
						+ " WHERE ScheduleOrder = " + ToOrder                      + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode     + " \n"	
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                \n"
						+ "   SET ScheduleOrder = " + ToOrder        + " \n"
						+ "      ,AckNo         = " + AckNo          + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo + " \n"
						+ "   AND ScheduleOrder = 0  \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 최종수정일시 셋트
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("홈광고편성 순위내림 변경:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] 순위:[" + schAppendAdModel.ScheduleOrder + ">" + ToOrder + "]  등록자:[" + header.UserID + "]");
					schAppendAdModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // 정상
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderDown() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = " 홈광고편성 순위내림 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion

		#region 홈광고편성  마지막 순위로

		/// <summary>
		/// 홈광고편성  마지막 순위로
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdOrderLast(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderLast() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					// 변경할 순위
					string ToOrder = "1";

					// 해당 변경할 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder   \n"
						+ "  FROM SchAppend                                      \n"
						+ " WHERE MediaCode = " + schAppendAdModel.MediaCode + " \n"
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

					// 해당 순위를 0순위로 임시변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                \n"
						+ "   SET ScheduleOrder = 0                      \n"
						+ "      ,AckNo         = " + AckNo          + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo        + " \n"
						+ "   AND ScheduleOrder = " + schAppendAdModel.ScheduleOrder + " \n"
						);

					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위보다 큰 순위의 내역들을 -1하여 조정
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                            \n"
						+ "   SET ScheduleOrder = ScheduleOrder - 1  \n"
						+ "      ,AckNo         = " + AckNo      + " \n"
						+ " WHERE MediaCode     = " + schAppendAdModel.MediaCode     + " \n"					     
						+ "   AND ScheduleOrder > " + schAppendAdModel.ScheduleOrder + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 해당 순위로  변경
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                       \n"
						+ "   SET ScheduleOrder = " + ToOrder               + " \n"
						+ "      ,AckNo         = " + AckNo                 + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo + " \n"
						+ "   AND ScheduleOrder = 0 \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// 최종수정일시 셋트
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("홈광고편성 마지막 순위로 변경:[" + schAppendAdModel.ItemName + "] 등록자:[" + header.UserID + "]");
					schAppendAdModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // 정상
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderLast() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = " 홈광고편성 마지막 순위로 변경 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion 


		#region 홈광고엑셀편성 삭제
		/// <summary>
		/// 홈광고엑셀편성 삭제
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdDelete_To(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();
                        
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[1];
                        
				sbQuery.Append( ""
					+ "        DELETE FROM  SchAppend			                    \n"
					+ "              WHERE MediaCode = @MediaCode				\n"					
					);                             
                                        
				sqlParams[i++] = new SqlParameter("@MediaCode" , SqlDbType.TinyInt);				
			        
				i = 0;				
				sqlParams[i++].Value = schAppendAdModel.MediaCode;				

				_log.Debug("MediaCode:[" + schAppendAdModel.MediaCode + "]");				
				
				_log.Debug(sbQuery.ToString());
                        
				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();
                        
					// __MESSAGE__
					_log.Message("홈광고엑셀편성삭제:[" + schAppendAdModel.MediaCode + "] 등록자:[" + header.UserID + "]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				schAppendAdModel.ResultCD = "0000";  // 정상
                        	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchAppendAdDelete_To() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3301";
				schAppendAdModel.ResultDesc = "홈광고엑셀편성 삭제중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}
		#endregion

		#region 홈광고엑셀편성 조회(엑셀 데이터중 없는것만 인서트 시키기 위해)
		/// <summary>
		/// 홈광고목록조회
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		public void SetSchAppendSearch(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchAppendSearch() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT ItemNo  \n"							
					+ "			   ,ScheduleOrder  \n"							
					+ "		FROM SchAppend                \n"					
					);
	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 카테고리모델에 복사
				schAppendAdModel.ScheduleDataSet = ds.Copy();
				// 결과
				schAppendAdModel.ResultCnt = Utility.GetDatasetCount(schAppendAdModel.ScheduleDataSet);
				// 결과코드 셋트
				schAppendAdModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + schAppendAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchAppendSearch() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD = "3000";
				schAppendAdModel.ResultDesc = "홈광고 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}
		#endregion

		#region 홈광고엑셀편성 생성
		/// <summary>
		/// 홈광고엑셀편성 생성
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdCreate_To(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdCreate_To() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int i = 0;
					int rc = 0;

					// 현재 승인번호를 구함
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					StringBuilder  sbQuery   = new StringBuilder();			
					SqlParameter[] sqlParams = new SqlParameter[2];					

					// 홈광고 편성 테이블에 추가
					sbQuery.Append( "\n"
						+ "INSERT INTO SchAppend (                   \n"
						+ "       MediaCode                        \n"
						+ "      ,ScheduleOrder                    \n"
						+ "      ,ItemNo                           \n"
						+ "      ,ModDt                            \n"
						+ "      ,AckNo                            \n"
						+ "      )                                 \n"
						+ " VALUES(                                 \n"					
						+ "       " + schAppendAdModel.MediaCode + " \n"
						+ "      ,@ScheduleOrder   \n"
						+ "      ,@ItemNo                          \n"
						+ "      ,GETDATE()                        \n"
						+ "      ," + AckNo                    + " )\n"						
						);

					i = 0;
					sqlParams[i++] = new SqlParameter("@ScheduleOrder"        , SqlDbType.Int          );
					sqlParams[i++] = new SqlParameter("@ItemNo"        , SqlDbType.Int          );

					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(schAppendAdModel.ScheduleOrder);
					sqlParams[i++].Value = Convert.ToInt32(schAppendAdModel.ItemNo);

					_db.BeginTran();

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					
					_log.Debug("MediaCode:[" + schAppendAdModel.MediaCode + "]");
					_log.Debug("ScheduleOrder:[" + schAppendAdModel.ScheduleOrder + "]");
					_log.Debug("ItemNo:[" + schAppendAdModel.ItemNo + "]");
					// __DEBUG__
					_log.Debug(sbQuery.ToString());
					// __DEBUG__
					
					///=================================================================================================
				
					// 홈광고 편성 테이블에 추가
					sbQuery   = new StringBuilder();
					
					sbQuery.Append( "\n"
						+ "UPDATE ContractItem        \n"
						+ "   SET AdState = '20'      \n"   // 광고상태 - 20:편성
						+ "      ,ModDt   = GETDATE() \n"
						+ "      ,RegID   = '" + header.UserID + "' \n" 
						+ " WHERE ItemNo  = @ItemNo   \n"
						);

					// __DEBUG__
					_log.Debug(sbQuery.ToString());
					// __DEBUG__

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// 최종수정일시 셋트
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// 편성한 순위
					string LastOrder = "1";

					// 해당 편성한 순서 구함
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder    \n"
						+ "   FROM SchAppend                                      \n"
						+ "  WHERE MediaCode = " + schAppendAdModel.MediaCode + " \n"
						);

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						LastOrder = Utility.GetDatasetString(ds, 0, "LastOrder");					
					}

					ds.Dispose();

					schAppendAdModel.ScheduleOrder = LastOrder;


					// __MESSAGE__
					_log.Message("홈광고엑셀편성:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdCreate_To() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = "홈광고편성 저장 중 오류가 발생하였습니다";
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
					+ "  WHERE MediaCode = @MediaCode And AdSchType = 10         \n"
					+ "  ORDER BY AckNo DESC                                     \n"
					+ "                                                          \n"
					+ " IF @AckState = '30' OR @AckState IS NULL                 \n"
					+ " BEGIN                                                    \n"
					+ "	    INSERT INTO SchPublish                               \n"
					+ "                (AckNo, MediaCode, State, ModifyStartDay, AdSchType) \n"
					+ "         SELECT ISNULL(MAX(AckNo),0) + 1                  \n"
					+ "               ,@MediaCode                                \n"
					+ "               ,'10'                                      \n"
					+ "               ,GETDATE(),10                              \n"
					+ "          FROM SchPublish                                 \n"
					+ "         WHERE MediaCode = @MediaCode                     \n"
					+ "                                                          \n"
					+ "     SELECT TOP 1 @AckState = State, @AckNo = AckNo       \n"
					+ "       FROM SchPublish                                    \n"
					+ "      WHERE MediaCode = @MediaCode and AdSchType=10       \n"
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