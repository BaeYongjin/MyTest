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
	/// FilePublishBiz에 대한 요약 설명입니다.
	/// </summary>
	public class FilePublishBiz : BaseBiz
	{

		#region 생성자
        public FilePublishBiz()
            : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}
		#endregion 

		#region 현재승인상태 조회
		/// <summary>
		/// 현재승인상태 조회
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishState(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishState() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	       :[" + filePublishModel.SearchMediaCode      + "]");		// 검색 매체				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n"
                    + " SELECT ACK_NO AS AckNo ,ACK_STT AS State \n"
                    + "   FROM FILEDIST_MST         \n"
                    + "  ORDER BY ACK_NO DESC      \n"
					);
	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					filePublishModel.AckNo =  ds.Tables[0].Rows[0]["AckNo"].ToString();
					filePublishModel.State =  ds.Tables[0].Rows[0]["State"].ToString();
				}
				else
				{
					filePublishModel.AckNo =  "";
					filePublishModel.State =  "";
				}

				ds.Dispose();

				// 결과코드 셋트
				filePublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishState() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "현재승인상태 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		#endregion

		#region 파일배포승인목록 조회

		/// <summary>
		/// 파일배포승인목록 조회
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishList(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFilePublishList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	       :[" + filePublishModel.SearchMediaCode      + "]");		// 검색 매체				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT A.ACK_NO AS AckNo                                                         \n"
                    + "       ,A.ACK_STT AS AckState                                             \n"
                    + "       ,B.STM_COD_NM AS AckStateName                                      \n"
                    + "       ,TO_CHAR(A.BEGIN_MOD_DT, 'YYYY-MM-DD') AS ModifyStartDay   \n"
                    + "       ,TO_CHAR(A.ACK_DT, 'YYYY-MM-DD') AS PublishDay           \n"
					+ "       ,A.ACK_ID AS PublishUser                                                   \n"
                    + "       ,C.USER_NM AS PublichUserName                                   \n"
                    + "       ,A.ACK_MEMO AS PublishDesc                                                   \n"
                    + "   FROM FILEDIST_MST         A                             \n"
                    + "        LEFT JOIN STM_COD    B  ON (A.ACK_STT       = B.STM_COD AND B.STM_COD_CLS = '33')  \n"  // 33:파일배포승인상태
                    + "        LEFT JOIN STM_USER C  ON (A.ACK_ID = C.USER_ID) \n"
                    + "  ORDER BY A.ACK_NO DESC                                                 \n"
					);
	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 해당모델에 복사
				filePublishModel.PublishDataSet = ds.Copy();
				ds.Dispose();

				// 결과
				filePublishModel.ResultCnt = Utility.GetDatasetCount(filePublishModel.PublishDataSet);
				// 결과코드 셋트
				filePublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFilePublishList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "파일배포승인이력 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}
		#endregion

		#region 파일배포 변경상세 조회

		/// <summary>
		/// 파일배포 변경상세 조회
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishHistory(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishHistory() Start");
				_log.Debug("-----------------------------------------");

				_db.Open();
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode :[" + filePublishModel.MediaCode      + "]");		// 매체코드				
				_log.Debug("AckNo     :[" + filePublishModel.AckNo          + "]");		// 승인번호				


                OracleParameter[] sqlParams = new OracleParameter[1];
                sqlParams[0] = new OracleParameter(":AckNo", OracleDbType.Int32);

				sqlParams[0].Value = Convert.ToInt32( filePublishModel.AckNo);

				StringBuilder sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n"
                    + "SELECT B.ITEM_SEQ  AS Seq         \n"
                    + "      ,B.ITEM_NO   AS ItemNo      \n"
                    + "      ,C.ITEM_NM   AS ItemName    \n"
                    + "      ,B.FILE_NM   AS FileName    \n"
                    + "      ,B.FILE_OPER AS AddDel      \n"
                    + "      ,TO_CHAR(A.ACK_DT, 'YYYY-MM-DD') As RegDt  \n"
					+ "      ,D.USER_NM AS RegName                        \n"
                    + "  FROM FILEDIST_MST            A                                                             \n"
                    + "       INNER JOIN FILEDIST_HST B ON (A.ACK_NO     = B.ACK_NO) \n"
                    + "       LEFT  JOIN ADVT_MST     C ON (B.ITEM_NO    = C.ITEM_NO)                          \n"
                    + "       LEFT  JOIN STM_USER     D ON (B.ID_INSERT  = D.USER_ID)                          \n"
                    + " WHERE A.ACK_NO     = :AckNo          \n"
                    + " ORDER BY B.ITEM_SEQ                      \n"
					);


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// 결과 DataSet의 해당모델에 복사
				filePublishModel.HistoryDataSet = ds.Copy();
				ds.Dispose();

				// 결과
				filePublishModel.ResultCnt = Utility.GetDatasetCount(filePublishModel.HistoryDataSet);
				// 결과코드 셋트
				filePublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetScheduleList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "파일배포편성현황 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		#endregion

		#region 파일배포승인
		/// <summary>
		/// 파일배포승인
		/// </summary>
		/// <returns></returns>
		public void SetFilePublish(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetFilePublish() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int i  = 0;
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();
						
					_db.BeginTran();

					// 편성승인상태의 변경
					sbQuery.Append( "\n"
                        + "UPDATE FILEDIST_MST                 \n"
                        + "   SET ACK_STT  = '30'              \n"  // 승인상태 30:배포승인
						+ "      ,ACK_ID   = '" + header.UserID + "'  \n"
						+ "      ,ACK_DT   = SYSDATE     \n"
                        + "      ,ACK_MEMO = :PublishDesc  \n"
                        + " WHERE ACK_NO   = :AckNo        \n"
                        + "   AND ACK_STT  = '10'          \n"
						);

                    OracleParameter[] sqlParams = new OracleParameter[2];

					i = 0;
                    sqlParams[i++] = new OracleParameter(":PublishDesc", OracleDbType.Varchar2, 200);
                    sqlParams[i++] = new OracleParameter(":AckNo", OracleDbType.Int32);


					i = 0;
					sqlParams[i++].Value = filePublishModel.PublishDesc;
                    sqlParams[i++].Value = Convert.ToInt32(filePublishModel.AckNo);

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    sbQuery = new StringBuilder();
					// 승인된 파일배포 파일리스트를 생성한다.
					sbQuery.Append( "\n"
                        + " INSERT INTO FILEDIST_DTL      \n"
                        + "        (ACK_NO, ITEM_NO, FILE_NM) \n"
						+ " SELECT " + filePublishModel.AckNo + " \n"
                        + "        ,ITEM_NO \n"
                        + "        ,FILE_NM         \n"
                        + "    FROM ADVT_MST \n"
                        + "   WHERE FILE_STT = '30' \n"
                        + "   ORDER BY ITEM_NO        \n"

						);

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("파일배포승인 : 승인번호[" + filePublishModel.AckNo + "] 등록자:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				filePublishModel.ResultCD = "0000";  // 정상
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetFilePublish() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD   = "3101";
				filePublishModel.ResultDesc = "파일배포승인 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		#endregion

		#region 파일배포 파일리스트 조회
		/// <summary>
		/// 파일배포 파일리스트 조회
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishFileList(HeaderModel header, FilePublishModel filePublishModel)
		{
			//bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishFileList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode :[" + filePublishModel.MediaCode  + "]"); // 매체코드				
				_log.Debug("AckNo     :[" + filePublishModel.AckNo      + "]");	// 승인번호				
   				_log.Debug("ReserveDt :[" + filePublishModel.ReserveDt  + "]");	// 배포승인 예약번호

				// __DEBUG__

                OracleParameter[] sqlParams = new OracleParameter[1];
		    
				int i = 0;
				//sqlParams[i++] = new SqlParameter("@MediaCode" , SqlDbType.Int);
                sqlParams[i++] = new OracleParameter("@AckNo", OracleDbType.Int32);
                //sqlParams[i++] = new SqlParameter("@ReserveDt" , SqlDbType.VarChar);

				i = 0;
				//sqlParams[i++].Value = Convert.ToInt32( filePublishModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32( filePublishModel.AckNo);
                //sqlParams[i++].Value = filePublishModel.ReserveDt.ToString();

				StringBuilder sbQuery = new StringBuilder();				
				
				// 쿼리생성
                // 지난 승인번호에서 배포된 것들은 2, 기 배포건과 파일번호 같은 쓰는것 포함됨. 아무튼 실제 배포가 발생하지는 않음
                sbQuery.Append("\n SELECT B.ITEM_NO AS ItemNo ");
                sbQuery.Append("\n       ,C.ITEM_NM AS ItemName ");
                sbQuery.Append("\n       ,B.FILE_NM AS FileName ");
                sbQuery.Append("\n       ,C.ADVT_STT AS AdState ");
                sbQuery.Append("\n       ,D.STM_COD_NM AdStateName ");
				sbQuery.Append("\n 	     ,2	as Flag ");
                sbQuery.Append("\n   FROM FILEDIST_MST                  A                                                      ");
                sbQuery.Append("\n        INNER JOIN FILEDIST_DTL  B ON (A.ACK_NO = B.ACK_NO) ");
                sbQuery.Append("\n        LEFT  JOIN ADVT_MST      C ON (B.ITEM_NO    = C.ITEM_NO)                          ");
                sbQuery.Append("\n        LEFT  JOIN STM_COD       D ON (C.ADVT_STT   = D.STM_COD      AND D.STM_COD_CLS = '25' ) ");
                sbQuery.Append("\n  WHERE A.ACK_NO     = :AckNo ");
				//sbQuery.Append("\n    and B.ReserveDt in('',NULL) ");
                //sbQuery.Append("\n union all ");
                // 선택한 승인번호에서 배포된 것들, 선택된 예약번호에 배포된것은 0, 같은 승인번호지만 이전 시간에 예약배포된것은 1
                /* 예약 관련내용 삭제
                sbQuery.Append("\n SELECT B.ITEM_NO AS ItemNo ");
				sbQuery.Append("\n       ,C.ItemName ");
				sbQuery.Append("\n       ,B.FileName ");
				sbQuery.Append("\n       ,C.AdState ");
				sbQuery.Append("\n       ,D.CodeName AdStateName ");
				sbQuery.Append("\n 	  ,0 AS Flag ");
                sbQuery.Append("\n   FROM FILEDIST_MST                  A                                                      ");
                sbQuery.Append("\n        INNER JOIN FILEDIST_DTL  B ON (A.ACK_NO = B.ACK_NO) ");
                sbQuery.Append("\n        LEFT  JOIN ContractItem       C ON (B.ITEM_NO    = C.ItemNo)                          ");
				sbQuery.Append("\n        LEFT  JOIN SystemCode         D ON (C.AdState   = D.Code      AND D.Section = '25' ) ");
				sbQuery.Append("\n  WHERE A.ACK_NO    = :AckNo ");
				sbQuery.Append("\n  ORDER BY Flag ,B.ItemNo DESC ");
                */
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// 결과 DataSet의 해당모델에 복사
				filePublishModel.FileListDataSet = ds.Copy();
				ds.Dispose();

				// 결과
				filePublishModel.ResultCnt = Utility.GetDatasetCount(filePublishModel.FileListDataSet);
				// 결과코드 셋트
				filePublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishFileList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "파일배포 파일리스트 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		#endregion

		#region 배포예약 작업관련

		/// <summary>
		/// 배포예약 파일리스트
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void GetReserveFiles(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveFiles() Start");
				_log.Debug("-----------------------------------------");

				_db.Open();

				_log.Debug("<입력정보>");
				_log.Debug("MediaCode :[" + filePublishModel.MediaCode      + "]");	// 매체코드
				_log.Debug("AckNo     :[" + filePublishModel.AckNo          + "]");	// 승인번호


				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@AckNo"     , SqlDbType.Int);
				sqlParams[0].Value = Convert.ToInt32( filePublishModel.AckNo);

				StringBuilder sbQuery = new StringBuilder();				

				sbQuery.Append("\n select  m.ItemNo	as ItemNo");
				sbQuery.Append("\n 		,( select ItemName from ContractItem with(noLock) where itemNo = m.ItemNo ) as ItemNm");
				sbQuery.Append("\n 		,( select isnull(ReserveDt ,'')");
				sbQuery.Append("\n 		   from   FilePublishReserveDetail with(noLock)");
				sbQuery.Append("\n 		   where  ackNo = m.AckNo");
				sbQuery.Append("\n 			and	  ItemNo = m.ItemNo ) as ReserveDt");
                sbQuery.Append("\n      , avg(isnull(c.FileLength,0))  as FileSize ");
                sbQuery.Append("\n      , max(c.FileState)	as FileState ");
				sbQuery.Append("\n      , max(c.FileName)	as FileName ");
				sbQuery.Append("\n from	FilePublishHistory m with(nolock) ");
                sbQuery.Append("\n inner join ContractItem c with(nolock) on m.ItemNo = c.ItemNo ");
				sbQuery.Append("\n where	m.AckNo = @AckNo");
				sbQuery.Append("\n and		m.AddDel = '+'");
				sbQuery.Append("\n group by m.AckNo,m.ItemNo");
				_log.Debug(sbQuery.ToString());
				
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				filePublishModel.FileListDataSet = ds.Copy();
				ds.Dispose();

				// 결과
				filePublishModel.ResultCnt = Utility.GetDatasetCount(filePublishModel.FileListDataSet);
				filePublishModel.ResultCD = "0000";

				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveFiles() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "배포승인예약 파일리스트 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 배포예약 작업리스트
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void GetReserveWorks(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveWorks() Start");
				_log.Debug("-----------------------------------------");

				_db.Open();

				_log.Debug("<입력정보>");
				_log.Debug("MediaCode :[" + filePublishModel.MediaCode      + "]");	// 매체코드
				_log.Debug("AckNo     :[" + filePublishModel.AckNo          + "]");	// 승인번호


				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@AckNo"     , SqlDbType.Int);
				sqlParams[0].Value = Convert.ToInt32( filePublishModel.AckNo);

				StringBuilder sbQuery = new StringBuilder();				
				sbQuery.Append("\n select ReserveDt");
				sbQuery.Append("\n 		 ,ReserveState");
				sbQuery.Append("\n 		 ,b.CodeName		as ReserveStatNm");
				sbQuery.Append("\n 		 ,ReserveUserId	as ReserveuserId");
				sbQuery.Append("\n 		 ,isnull(c.UserName,'')		as ReserveuserNm");
				sbQuery.Append("\n 		 ,( select count(distinct ItemNo ) from FilePublishReserveDetail nolock where ackNo = a.AckNo and ReserveDt = a.ReserveDt) as ReserveCount");
				sbQuery.Append("\n       ,(  select  sum(isnull(FileLength,0))           ");
				sbQuery.Append("\n            from    ContractItem l with(nolock)");
				sbQuery.Append("\n            where   ItemNo in(  select distinct ItemNo");
				sbQuery.Append("\n                                from    FilePublishReserveDetail m with(nolock)");
				sbQuery.Append("\n                                where   m.ackNo     = a.AckNo ");
				sbQuery.Append("\n                                and     m.ReserveDt = a.ReserveDt ) ) as FileSize");
				sbQuery.Append("\n		, substring(ReserveDt,1,4) + '-' + substring(ReserveDt,5,2) + '-' + substring(ReserveDt,7,2) + ' ' + substring(ReserveDt,9,2) + ':' + substring(ReserveDt,11,2) as ReserveDtStr");
				sbQuery.Append("\n from	FilePublishReserve a with(noLock)");
				sbQuery.Append("\n inner join SystemCode b with(noLock) on a.ReserveState	= b.Code and b.Section = '36'");
				sbQuery.Append("\n left outer join SystemUser c with(noLock) on a.ReserveUserId	= c.UserId");
				sbQuery.Append("\n where	a.AckNo = @AckNo");

				sbQuery.Append("\n union all");
	
				sbQuery.Append("\n select	 NULL ReserveDt");
				sbQuery.Append("\n 			,'90' ReserveState");
				sbQuery.Append("\n 			,'미정' 	as ReserverStatNm");
				sbQuery.Append("\n 			,'system'	as ReserveuserId");
				sbQuery.Append("\n 			,'시스템'	as ReserveuserNm");
				sbQuery.Append("\n 			,(  select count(*)");
				sbQuery.Append("\n 				from (	select	m.ItemNo");
				sbQuery.Append("\n 						from	FilePublishHistory m with(noLock)");
				sbQuery.Append("\n 						where	m.AckNo = @AckNo");
				sbQuery.Append("\n 						and		m.AddDel = '+'");
				sbQuery.Append("\n 						group by m.ItemNo");
				sbQuery.Append("\n 						EXCEPT ");
				sbQuery.Append("\n 						select  ItemNo");
				sbQuery.Append("\n 						from	FilePublishReserveDetail with(noLock)");
				sbQuery.Append("\n 						where	AckNo = @AckNo");
				sbQuery.Append("\n 					 ) v ) as ReserverCount");
				sbQuery.Append("\n 		 ,(  select  sum(isnull(FileLength,0))           ");
				sbQuery.Append("\n            from    ContractItem l with(nolock)");
				sbQuery.Append("\n            where   ItemNo in(  select	m.ItemNo");
				sbQuery.Append("\n					            from	FilePublishHistory m with(noLock)");
				sbQuery.Append("\n					            where	m.AckNo = @AckNo");
				sbQuery.Append("\n					            and		m.AddDel = '+'");
				sbQuery.Append("\n					            group by m.ItemNo");
				sbQuery.Append("\n					            EXCEPT ");
				sbQuery.Append("\n					            select  ItemNo");
				sbQuery.Append("\n					            from	FilePublishReserveDetail with(noLock)");
				sbQuery.Append("\n					            where	AckNo = @AckNo ) ) as FileSize");
				sbQuery.Append("\n		, NULL	as ReserveDtStr");
				_log.Debug(sbQuery.ToString());
				
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				filePublishModel.FileListDataSet = ds.Copy();
				ds.Dispose();

				// 결과
				filePublishModel.ResultCnt = Utility.GetDatasetCount(filePublishModel.FileListDataSet);
				filePublishModel.ResultCD = "0000";

				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveWorks() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "배포승인예약 작업리스트 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 예약작업 조회
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void GetReserveWorkSelect(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveWorkDetail() Start");
				_log.Debug("-----------------------------------------");

				_db.Open();
				StringBuilder sb = new StringBuilder();				
				SqlParameter[] sqlParams = new SqlParameter[2];

				sqlParams[0] = new SqlParameter("@AckNo"     , SqlDbType.Int);
				sqlParams[0].Value = Convert.ToInt32( filePublishModel.AckNo);

				sqlParams[1] = new SqlParameter("@ReserveDt"	, SqlDbType.VarChar, 12);
				sqlParams[1].Value = filePublishModel.ReserveDt;

				sb.Append("\n select a.AckNo					");
				sb.Append("\n 	    ,a.ReserveDt				");
				sb.Append("\n 	    ,a.ReserveState				");
				sb.Append("\n 	    ,b.UserName		as RegUser	");
				sb.Append("\n 	    ,c.UserName		as ModUser	");
				sb.Append("\n 	    ,a.ModDt					");
				sb.Append("\n 	    ,a.ReserveMsg				");
				sb.Append("\n from	FilePublishReserve a with(noLock) ");
				sb.Append("\n left outer join SystemUser b with(noLock) on a.ReserveUserId = b.UserId ");
				sb.Append("\n left outer join SystemUser c with(noLock) on a.ModUserId		= c.UserId ");
				sb.Append("\n where	a.AckNo		= @AckNo ");
				sb.Append("\n and	a.ReserveDt = @ReserveDt ");

				_log.Debug(sb.ToString());
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sb.ToString(),sqlParams);

				if(ds.Tables[0].Rows.Count > 0)
				{
					filePublishModel.State		=  ds.Tables[0].Rows[0]["ReserveState"].ToString();
					filePublishModel.ReserveDt	=  ds.Tables[0].Rows[0]["ReserveDt"].ToString();
					filePublishModel.ReserveUserName = ds.Tables[0].Rows[0]["RegUser"].ToString();
					filePublishModel.ModifyUserName  = ds.Tables[0].Rows[0]["ModUser"].ToString();
					filePublishModel.ModDt		= ds.Tables[0].Rows[0]["ModDt"].ToString();
					filePublishModel.PublishDesc	= ds.Tables[0].Rows[0]["ReserveMsg"].ToString();
				}
				else
				{
					filePublishModel.State = "00";
				}
				ds.Dispose();

				// 결과코드 셋트
				filePublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveWorkDetail() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "파일배포예약작업 상세조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}


		/// <summary>
		/// 예약작업 입력
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void SetReserveWorkInsert(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();

				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[6];

				sqlParams[0] = new SqlParameter("@AckNo"	, SqlDbType.Int				);
				sqlParams[1] = new SqlParameter("@ResDt"	, SqlDbType.VarChar	,	12	);
				sqlParams[2] = new SqlParameter("@ResState" , SqlDbType.VarChar ,	 2	);
				sqlParams[3] = new SqlParameter("@ResUser"  , SqlDbType.VarChar ,	10	);
				sqlParams[4] = new SqlParameter("@ModUser"	, SqlDbType.VarChar ,	10	);
				sqlParams[5] = new SqlParameter("@Msg"		, SqlDbType.VarChar ,  100	);

				sqlParams[0].Value = Convert.ToInt32(filePublishModel.AckNo);
				sqlParams[1].Value = filePublishModel.ReserveDt;
				sqlParams[2].Value = filePublishModel.State;
				sqlParams[3].Value = header.UserID;							// 예약자ID 수정시엔 무시
				sqlParams[4].Value = header.UserID;
				sqlParams[5].Value = filePublishModel.PublishDesc;
			
				try
				{
					_db.BeginTran();

					sbQuery.Append("\n INSERT INTO FilePublishReserve ");
					sbQuery.Append("\n            (AckNo ");
					sbQuery.Append("\n            ,ReserveDt ");
					sbQuery.Append("\n            ,ReserveState ");
					sbQuery.Append("\n            ,ReserveUserId ");
					sbQuery.Append("\n            ,ModDt ");
					sbQuery.Append("\n            ,ModUserId ");
					sbQuery.Append("\n            ,ReserveMsg) ");
					sbQuery.Append("\n      VALUES ");
					sbQuery.Append("\n            (@AckNo ");
					sbQuery.Append("\n            ,@ResDt ");
					sbQuery.Append("\n            ,@ResState ");
					sbQuery.Append("\n            ,@ResUser ");
					sbQuery.Append("\n            ,GetDate() ");
					sbQuery.Append("\n            ,@ModUser ");
					sbQuery.Append("\n            ,@Msg ); ");

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("파일배포예약 입력:["+filePublishModel.AckNo + "]["+filePublishModel.ReserveDt + "] 작업자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				filePublishModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD   = "3201";
				filePublishModel.ResultDesc = "광고파일 등록중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}
		}

		/// <summary>
		/// 예약작업 수정
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void SetReserveWorkUpdate(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();

				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[7];

				sqlParams[0] = new SqlParameter("@AckNo"	, SqlDbType.Int				);
				sqlParams[1] = new SqlParameter("@ResDt"	, SqlDbType.VarChar	,	12	);
				sqlParams[2] = new SqlParameter("@ResState" , SqlDbType.VarChar ,	 2	);
				sqlParams[3] = new SqlParameter("@ResUser"  , SqlDbType.VarChar ,	10	);
				sqlParams[4] = new SqlParameter("@ModUser"	, SqlDbType.VarChar ,	10	);
				sqlParams[5] = new SqlParameter("@Msg"		, SqlDbType.VarChar ,  100	);
				sqlParams[6] = new SqlParameter("@ResDtOrg" , SqlDbType.VarChar	,	12  );

				sqlParams[0].Value = Convert.ToInt32(filePublishModel.AckNo);
				sqlParams[1].Value = filePublishModel.ReserveDt;
				sqlParams[2].Value = filePublishModel.State;
				sqlParams[3].Value = header.UserID;							// 예약자ID 수정시엔 무시
				sqlParams[4].Value = header.UserID;
				sqlParams[5].Value = filePublishModel.PublishDesc;
				sqlParams[6].Value = filePublishModel.SearchReserveKey;
			
				try
				{
					_db.BeginTran();
					
					// 키가 같은 경우엔 예약키 변경없이 내용만 변경된거
					// 키가 다른 경우엔 예약키도 변경된것임으로 Detail테이블도 변경해야 한다
					if( filePublishModel.SearchReserveKey.Equals(filePublishModel.ReserveDt) )
					{
						sbQuery.Append("\n UPDATE FilePublishReserve ");
						sbQuery.Append("\n    SET ReserveState	= @ResState ");
						//sbQuery.Append("\n       ,ReserveUserId	= @ResUser ");
						sbQuery.Append("\n       ,ModDt			= GetDate() ");
						sbQuery.Append("\n       ,ModUserId		= @ModUser ");
						sbQuery.Append("\n       ,ReserveMsg	= @Msg ");
						sbQuery.Append("\n  WHERE AckNo		= @AckNo ");
						sbQuery.Append("\n    AND ReserveDt	= @ResDt ");
					}
					else
					{
						sbQuery.Append("\n UPDATE FilePublishReserve ");
						sbQuery.Append("\n    SET ReserveDt		= @ResDt ");
						sbQuery.Append("\n       ,ReserveState	= @ResState ");
						//sbQuery.Append("\n       ,ReserveUserId	= @ResUser ");
						sbQuery.Append("\n       ,ModDt			= GetDate() ");
						sbQuery.Append("\n       ,ModUserId		= @ModUser ");
						sbQuery.Append("\n       ,ReserveMsg	= @Msg ");
						sbQuery.Append("\n  WHERE AckNo		= @AckNo ");
						sbQuery.Append("\n    AND ReserveDt	= @ResDtOrg ");
					}
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					if( !filePublishModel.SearchReserveKey.Equals(filePublishModel.ReserveDt) )
					{
						sbQuery = new StringBuilder();
						sbQuery.Append("\n UPDATE FilePublishReserveDetail ");
						sbQuery.Append("\n    SET ReserveDt	= @ResDt ");
						sbQuery.Append("\n  WHERE AckNo		= @AckNo ");
						sbQuery.Append("\n    AND ReserveDt	= @ResDtOrg ");

						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					}
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("파일배포예약 수정:["+filePublishModel.AckNo + "]["+filePublishModel.ReserveDt + "] 작업자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				filePublishModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD   = "3201";
				filePublishModel.ResultDesc = "광고파일 등록중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}


		/// <summary>
		/// 예약파일 조회
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void SetReserveFileSelect(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveWorkDetail() Start");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "파일배포예약작업 상세조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}


		/// <summary>
		/// 예약파일 수정( 입력혹은 삭제처리 )
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void SetReserveFileUpdate(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();
				DataSet			ds    = null;

				string	orgResDt	= "";	// 기존 예약키
				string	orgResSt	= "";	// 기존 예약작업상태
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[4];

				sqlParams[0] = new SqlParameter("@AckNo"	, SqlDbType.Int				);
				sqlParams[1] = new SqlParameter("@ResDt"	, SqlDbType.VarChar	,	12	);
				sqlParams[2] = new SqlParameter("@ItemNo"	, SqlDbType.Int				);
				sqlParams[3] = new SqlParameter("@regId"	, SqlDbType.VarChar ,	10	);

				sqlParams[0].Value = Convert.ToInt32(filePublishModel.AckNo);
				sqlParams[1].Value = filePublishModel.ReserveDt;
				sqlParams[2].Value = filePublishModel.ItemNo;
				sqlParams[3].Value = header.UserID;							// 예약자ID 수정시엔 무시
			
				try
				{
					_db.BeginTran();

					#region [ 기존 데이터 검증: 속한 예약작업의 상태를 검증 ]
					/*
					 * 기존데이터가 존재하는지 검증한다.
					 * 존재하는 경우엔 기존것에 키만 덮어쓴다.
					 * 만약 기존재하는 예약번호건이 완료건이면 변경이 불가능 하게 처리한다.
					 * */
					sbQuery.Append("\n select ReserveState,ReserveDt ");
					sbQuery.Append("\n from	  FilePublishReserve b with(noLock) ");
					sbQuery.Append("\n where  AckNo		= @AckNo ");
					sbQuery.Append("\n and	  ReserveDt = @ResDt ");

					ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

					if(ds.Tables[0].Rows.Count > 0)
					{
						orgResDt	=	ds.Tables[0].Rows[0]["ReserveDt"].ToString();
						orgResSt	=   ds.Tables[0].Rows[0]["ReserveState"].ToString();
					}
					else
					{
						orgResDt	=	"00";
						orgResSt	=   "00";
					}
					ds.Dispose();

					// 20이면 완료된 작업의 파일임으로 변경이 불가함.
					if( orgResSt.Equals("20") )
					{
						filePublishModel.ResultCD   = "0009";
						throw new Exception("[안내] 배포완료된 예약작업 입니다.\n작업처리가 불가능 합니다.");
					}
					#endregion

					#region [ 기존 데이터 검증 : 해당 광고번호로 기존작업 등록여부 검증]
					sbQuery = new StringBuilder();
					sbQuery.Append("\n select ReserveState,ReserveDt ");
					sbQuery.Append("\n from	  FilePublishReserve b with(noLock) ");
					sbQuery.Append("\n where  AckNo = @AckNo ");
					sbQuery.Append("\n and	  ReserveDt = (	select  ReserveDt ");
					sbQuery.Append("\n 						from	FilePublishReserveDetail a with(noLock) ");
					sbQuery.Append("\n 						where	a.ItemNo = @ItemNo ");
					sbQuery.Append("\n 						and		a.AckNo  = @AckNo ) ");

					ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

					if(ds.Tables[0].Rows.Count > 0)
					{
						orgResDt	=	ds.Tables[0].Rows[0]["ReserveDt"].ToString();
						orgResSt	=   ds.Tables[0].Rows[0]["ReserveState"].ToString();
					}
					else
					{
						orgResDt	=	"00";
						orgResSt	=   "00";
					}
					ds.Dispose();

					// 20이면 완료된 작업의 파일임으로 변경이 불가함.
					if( orgResSt.Equals("20") )
					{
						filePublishModel.ResultCD   = "0001";
						throw new Exception("[안내] 배포완료된 예약작업에 속해있는 광고파일입니다.\n작업처리가 불가능 합니다.");
					}
					#endregion
				

					sbQuery = new StringBuilder();
					if( filePublishModel.JobType.Equals("+") )
					{
						sbQuery = new StringBuilder();
						if( orgResDt.Equals("00") )
						{
							// 신규설정인 경우
							sbQuery.Append("\n insert into FilePublishReserveDetail");
							sbQuery.Append("\n			( AckNo,	ReserveDt,	ItemNo,		regId,		regDt )");
							sbQuery.Append("\n values   ( @AckNo,   @ResDt,		@ItemNo,	@regId,		GetDate());");
							rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

							if( rc == 0 )
							{
								filePublishModel.ResultCD   = "0002";
								throw new Exception("[안내] 예약파일 신규등록에 실패했습니다");
							}
						}
						else
						{
							// 변경설정인 경우
							sbQuery.Append("\n update FilePublishReserveDetail	");
							sbQuery.Append("\n	  set ReserveDt	= @ResDt	");
							sbQuery.Append("\n		 ,regId		= @regId	");
							sbQuery.Append("\n	     ,regDt		= GetDate()	");
							sbQuery.Append("\n	where AckNo		= @AckNo	");
							sbQuery.Append("\n	  and ReserveDt = '" + orgResDt + "'");
							sbQuery.Append("\n    and ItemNo    = @ItemNo   ");
						
							rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

							if( rc == 0 )
							{
								filePublishModel.ResultCD   = "0003";
								throw new Exception("[안내] 예약파일 변경등록에 실패했습니다");
							}
						}
					}
					else
					{
						// 삭제인 경우
						sbQuery.Append("\n delete from FilePublishReserveDetail	");
						sbQuery.Append("\n where AckNo	= @AckNo	");
						sbQuery.Append("\n and ReserveDt= @ResDt	");
						sbQuery.Append("\n and ItemNo   = @ItemNo   ");
						
						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

						if( rc == 0 )
						{
							filePublishModel.ResultCD   = "0004";
							throw new Exception("[안내] 예약파일 삭제처리에 실패했습니다");
						}
					}
					
					_db.CommitTran();
					_log.Message("파일배포예약 파일:["+filePublishModel.AckNo + "]["+filePublishModel.ReserveDt + "]["+filePublishModel.ItemNo + "] 작업자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				filePublishModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				if( filePublishModel.ResultCD.Equals("0000") )
				{
					filePublishModel.ResultCD = "3201";
				}

				filePublishModel.ResultDesc = ex.Message;
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}



		#endregion
	}
}