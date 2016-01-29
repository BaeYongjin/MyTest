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
    /// SchExclusiveZoneBiz 시간대독점편성 비지니스 로직 입니다. 
    /// </summary>
    public class SchExclusiveZoneBiz : BaseBiz
    {
        public SchExclusiveZoneBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// 타겟팅 설정이 된 CM 광고를 가지고 온다. 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schExclusiveZoneModel"></param>
        public void GetSchExclusiveList(HeaderModel header, SchExclusiveZoneModel schExclusiveZoneModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchExclusiveList() Start");
				_log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();
                
                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + " SELECT   B.ItemNo 	                \n"
                    + "        , B.ItemName                 \n"
                    + "        , B.ExcuteStartDay           \n"
                    + "        , B.RealEndDay               \n"
                    + "        , A.TgtTime                  \n"
                    + "        , A.TgtWeekYn                \n"
                    + "        , A.TgtWeek                  \n"
                    + " FROM Targeting A with(nolock)       \n"
                    + " LEFT JOIN ContractItem B with(nolock) ON (A.ItemNo = B.ItemNo)  \n"
                    + " WHERE A.Itemno > 0                  \n"
                    + "     And A.TgtTimeYn = 'Y'           \n"
                    + "     AND B.AdType = 10               \n"
                    + " 	AND B.AdState >= '20' AND B.AdState <= '40'                 \n"
                    + "     AND B.ScheduleType = '30'       \n");


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 모델에 복사
                schExclusiveZoneModel.SchExclusiveDataSet = ds.Copy();

                // 결과
                schExclusiveZoneModel.ResultCnt = Utility.GetDatasetCount(schExclusiveZoneModel.SchExclusiveDataSet);

                // 결과코드 셋트
                schExclusiveZoneModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schExclusiveZoneModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchExclusiveList() End");
                _log.Debug("-----------------------------------------");
                
            }
            catch (Exception ex)
            {

                schExclusiveZoneModel.ResultCD = "3000";
                schExclusiveZoneModel.ResultDesc = "시간대독점편성 목록 조회중 오류가 발생했습니다.";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 추가 부분에 타겟 목록 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schExclusiveZoneModel"></param>
        public void GetTagetingList(HeaderModel header, SchExclusiveZoneModel schExclusiveZoneModel)
        {
            bool isState = false;

            try
            {
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey            :[" + schExclusiveZoneModel.SearchKey + "]");		// 검색어
                _log.Debug("SearchMediaCode	     :[" + schExclusiveZoneModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchRapCode        :[" + schExclusiveZoneModel.SearchRapCode + "]");		// 검색 랩
                _log.Debug("SearchAgencyCode     :[" + schExclusiveZoneModel.SearchAgencyCode + "]");		// 검색 대행사
                _log.Debug("SearchchkAdState_20  :[" + schExclusiveZoneModel.SearchchkAdState_20 + "]");		// 검색 광고상태 : 편성
                _log.Debug("SearchchkAdState_30  :[" + schExclusiveZoneModel.SearchchkAdState_30 + "]");		// 검색 광고상태 : 중지	
                _log.Debug("SearchchkAdState_40  :[" + schExclusiveZoneModel.SearchchkAdState_40 + "]");		// 검색 광고상태 : 종료           
                // __DEBUG__
                #region 쿼리 
                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append(@"
                    SELECT B.ItemNo                      
                      ,B.ItemName                    
                      ,C.ContractName                
                      ,D.AdvertiserName              
                      ,'*' as ContStateName   
                      ,B.ExcuteStartDay              
                      ,B.ExcuteEndDay                
                      ,B.RealEndDay                  
                      ,F.CodeName as AdTypeName      
                      ,G.CodeName as AdStatename     
                      ,C.ContractAmt                 
                      ,H.PriorityCd                  
                      ,B.MediaCode                   
                      ,ISNULL(H.ItemNo,0)  As TgtItemNo 
		                 , case when h.ItemNo is null																					then 0 	
                             when h.ItemNo > 0 and b.AdType in(10,15,16,17) and ( h.tgtRegion1Yn = 'N' and h.tgtTimeYn = 'N' and h.tgtWeekYn = 'N' )  then 0 
				                when h.ItemNo > 0 and b.AdType in(10,15,16,17) and ( h.tgtRegion1Yn = 'Y' or h.tgtTimeYn = 'Y' or h.tgtWeekYn = 'Y' )  	
      			                then case when ( select count(*) from TargetingRate with(noLock) where ItemNo = b.ItemNo ) > 0 then 2 else 1 end	
                             when h.ItemNo > 0 and b.AdType in(11,12,20)  then 0 
      		                else																																																										     2	
                        end as RateItemNo	
                      ,H.ContractAmt AS TgtAmount    
                      ,H.TgtCollection			    
                      ,B.AdType      			    
                      ,I.CodeName ScheduleTypeName   
                  FROM ContractItem B with(nolock)  
                       INNER JOIN Contract   C with(nolock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq) 
                       LEFT  JOIN Advertiser D with(nolock) ON (B.AdvertiserCode = D.AdvertiserCode)      
                       LEFT  JOIN SystemCode F with(nolock) ON (B.AdType         = F.Code AND F.Section = '26')  
                       LEFT  JOIN SystemCode G with(nolock) ON (B.AdState        = G.Code AND G.Section = '25')  
                       LEFT  JOIN Targeting  H with(nolock) ON (B.ItemNo         = H.ItemNo)  
                       LEFT  JOIN SystemCode I with(NoLock) ON (B.ScheduleType   = I.Code AND I.Section = '27' ) 
                 WHERE B.ItemNo > 0  
                 AND B.AdType  = '10'    
                 AND B.ScheduleType = '30'
                 And ( H.TgtTimeYn = 'N' or H.TgtTimeYn is null )
                 and ( H.TgtWeekYn = 'N' or H.TgtWeekYn is null ) ");

                // 매체을 선택했으면
                if (schExclusiveZoneModel.SearchMediaCode.Trim().Length > 0 && !schExclusiveZoneModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.MediaCode  = " + schExclusiveZoneModel.SearchMediaCode.Trim() + " \n");
                }

                // 랩사를 선택했으면
                if (schExclusiveZoneModel.SearchRapCode.Trim().Length > 0 && !schExclusiveZoneModel.SearchRapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.RapCode  = " + schExclusiveZoneModel.SearchRapCode.Trim() + " \n");
                }

                // 대행사를 선택했으면
                if (schExclusiveZoneModel.SearchAgencyCode.Trim().Length > 0 && !schExclusiveZoneModel.SearchAgencyCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.AgencyCode  = " + schExclusiveZoneModel.SearchAgencyCode.Trim() + " \n");
                }

                // 광고상태는 20:편성 과 40:종료 사이에 있는 것만 조회한다.
                sbQuery.Append(" AND B.AdState >= '20' AND B.AdState <= '40' \n");

                // 광고상태 편성
                if (schExclusiveZoneModel.SearchchkAdState_20.Trim().Length > 0 && schExclusiveZoneModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( B.AdState  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (schExclusiveZoneModel.SearchchkAdState_30.Trim().Length > 0 && schExclusiveZoneModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (schExclusiveZoneModel.SearchchkAdState_40.Trim().Length > 0 && schExclusiveZoneModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");

                // 검색어가 있으면
                if (schExclusiveZoneModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( B.ItemNo         LIKE '%" + schExclusiveZoneModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.ItemName       LIKE '%" + schExclusiveZoneModel.SearchKey.Trim() + "%' \n"
                        + "     OR C.ContractName   LIKE '%" + schExclusiveZoneModel.SearchKey.Trim() + "%' \n"
                        + "     OR D.AdvertiserName LIKE '%" + schExclusiveZoneModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n");
                }

                sbQuery.Append("  ORDER BY B.ItemNo DESC ");
                #endregion

                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사                			
                schExclusiveZoneModel.TargetingDataSet = ds.Copy();
                // 결과
                schExclusiveZoneModel.ResultCnt = Utility.GetDatasetCount(schExclusiveZoneModel.TargetingDataSet);
                // 결과코드 셋트
                schExclusiveZoneModel.ResultCD = "0000";

                ds.Dispose();

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schExclusiveZoneModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingList() End");
                _log.Debug("-----------------------------------------");

            }
            catch (Exception ex)
            {

                schExclusiveZoneModel.ResultCD = "3000";
                schExclusiveZoneModel.ResultDesc = "지정광고편성현황 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 시간대 독점 내역 추가 
        /// </summary>
        /// <returns></returns>
        public void SetTargetingDetailUpdate(HeaderModel header, SchExclusiveZoneModel schExclusiveZoneModel)
        {

            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetTargetingDetailUpdate() Start");
                _log.Debug("-----------------------------------------");

                _log.Debug("ItemNo         :[" + schExclusiveZoneModel.ItemNo + "]");
                _log.Debug("ItemName       :[" + schExclusiveZoneModel.ItemName + "]");
                _log.Debug("TgtTime        :[" + schExclusiveZoneModel.TgtTime + "]");

                int i = 0;
                int rc = 0;
                StringBuilder sbQuery;
                SqlParameter[] sqlParams = new SqlParameter[30];

                i = 0;
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int, 4);
                sqlParams[i++] = new SqlParameter("@ContractAmt", SqlDbType.Decimal);
                sqlParams[i++] = new SqlParameter("@TgtTime", SqlDbType.VarChar, 148);  

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schExclusiveZoneModel.ItemNo);
                sqlParams[i++].Value = Convert.ToDecimal(schExclusiveZoneModel.ContractAmt);
                sqlParams[i++].Value = schExclusiveZoneModel.TgtTime;
                

                // 쿼리실행
                try
                {
                    _db.BeginTran();

                    sbQuery = new StringBuilder();
                    sbQuery.Append(" UPDATE Targeting							\n");
                    sbQuery.Append("    SET ContractAmt		= @ContractAmt		\n");
                    sbQuery.Append("       ,TgtTimeYn     	= 'Y'		        \n");
                    sbQuery.Append("       ,TgtTime       	= @TgtTime			\n");
                    sbQuery.Append("  WHERE ItemNo          = @ItemNo	        \n");

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    if (rc < 1)
                    {
                        // 지정채널 편성 테이블에 추가
                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n");
                        sbQuery.Append(" INSERT INTO Targeting		  \n");
                        sbQuery.Append("            (ItemNo			  \n");
                        sbQuery.Append("            ,ContractAmt	  \n");
                        sbQuery.Append("            ,PriorityAmt	  \n");
                        sbQuery.Append("            ,PriorityValue	  \n");
                        sbQuery.Append("            ,PriorityCd		  \n");
                        sbQuery.Append("            ,AmtControlYn    \n");
                        sbQuery.Append("            ,AmtControlRate  \n");
                        sbQuery.Append("            ,TgtRegion1Yn    \n");
                        sbQuery.Append("            ,TgtRegion1      \n");
                        sbQuery.Append("            ,TgtTimeYn       \n");
                        sbQuery.Append("            ,TgtTime         \n");
                        sbQuery.Append("            ,TgtAgeYn        \n");
                        sbQuery.Append("            ,TgtAge          \n");
                        sbQuery.Append("            ,TgtAgeBtnYn     \n");
                        sbQuery.Append("            ,TgtAgeBtnBegin  \n");
                        sbQuery.Append("            ,TgtAgeBtnEnd    \n");
                        sbQuery.Append("            ,TgtSexYn        \n");
                        sbQuery.Append("            ,TgtSexMan       \n");
                        sbQuery.Append("            ,TgtSexWoman     \n");
                        sbQuery.Append("            ,TgtRateYn       \n");
                        sbQuery.Append("            ,TgtRate         \n");
                        sbQuery.Append("            ,TgtWeekYn       \n");
                        sbQuery.Append("            ,TgtWeek         \n");
                        sbQuery.Append("            ,TgtCollectionYn       \n");
                        sbQuery.Append("            ,TgtCollection         \n");
                        sbQuery.Append("			,TgtZipYn		\n");
                        sbQuery.Append("			,TgtZip			\n");
                        sbQuery.Append("			,TgtPPxYn		\n");
                        sbQuery.Append("			,TgtFreqYn		\n");
                        sbQuery.Append("			,TgtFreqDay		\n");
                        sbQuery.Append("			,TgtFreqPeriod	\n");
                        sbQuery.Append("			,TgtPVDBYn		\n");
                        sbQuery.Append("            )				  \n");
                        sbQuery.Append("    VALUES	(@ItemNo		  \n");
                        sbQuery.Append("            ,@ContractAmt	  \n");
                        sbQuery.Append("            ,10000000		  \n");		// 기본값 설정
                        sbQuery.Append("            ,0           	  \n");
                        sbQuery.Append("            ,6          	  \n");
                        sbQuery.Append("            ,'N'              \n");
                        sbQuery.Append("            ,100              \n");
                        sbQuery.Append("            ,'N'              \n");
                        sbQuery.Append("            ,''               \n");
                        sbQuery.Append("            ,'Y'              \n");
                        sbQuery.Append("            ,@TgtTime         \n");
                        sbQuery.Append("            ,'N'              \n");
                        sbQuery.Append("            ,''               \n");
                        sbQuery.Append("            ,'N'              \n");
                        sbQuery.Append("            ,0                \n");
                        sbQuery.Append("            ,0                \n");
                        sbQuery.Append("            ,'N'              \n");
                        sbQuery.Append("            ,''               \n");
                        sbQuery.Append("            ,''               \n");
                        sbQuery.Append("            ,'N'              \n");
                        sbQuery.Append("            ,0                \n");
                        sbQuery.Append("            ,'N'              \n");
                        sbQuery.Append("            ,''		          \n");
                        sbQuery.Append("            ,'N'	          \n");
                        sbQuery.Append("            ,0		          \n");
                        sbQuery.Append("			,'N'		      \n");
                        sbQuery.Append("			,''			      \n");
                        sbQuery.Append("			,'N'		      \n");
                        sbQuery.Append("			,'N'		      \n");
                        sbQuery.Append("			,0		          \n");
                        sbQuery.Append("			,0	              \n");
                        sbQuery.Append("			,'N'	          \n");
                        sbQuery.Append(" 			)			      \n");

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    }

                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("광고 타겟팅 수정:[" + schExclusiveZoneModel.ItemNo + "] " + schExclusiveZoneModel.ItemName + " 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schExclusiveZoneModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetTargetingDetailUpdate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schExclusiveZoneModel.ResultCD = "3101";
                schExclusiveZoneModel.ResultDesc = "광고 타겟팅 수정 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


        #region 선택 시간대 독점 편성 값
        /// <summary>
        /// 선택 시간대 독점 편성 값
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schExclusiveZoneModel"></param>
        public void GetTimeTargetDetail(HeaderModel header, SchExclusiveZoneModel schExclusiveZoneModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTimeTargetDetail() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("ItemNo :[" + schExclusiveZoneModel.ItemNo + "]");		// 광고번호

                //===================================================================
                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.AppendFormat(@"
                    SELECT    ITEMNO
                            , TGTTIMEYN
                            , TGTTIME
                            , TGTWEEKYN
                            , TGTWEEK
                    FROM TARGETING 
                    WHERE ITEMNO = '{0}'
                
                ", schExclusiveZoneModel.ItemNo);

                // __DEBUG__
                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                schExclusiveZoneModel.SchExDetailDataSet = ds.Copy();
                schExclusiveZoneModel.ResultCnt = Utility.GetDatasetCount(schExclusiveZoneModel.SchExDetailDataSet);
                schExclusiveZoneModel.ResultCD = "0000";
                ds.Dispose();

                //===================================================================
                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schExclusiveZoneModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTimeTargetDetail() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schExclusiveZoneModel.ResultCD = "3000";
                schExclusiveZoneModel.ResultDesc = "시간대 독점 편성 상세 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
        #endregion

        #region 시간대 독점 내역 수정
        /// <summary>
        /// 시간대 독점 내역 수정
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schExclusiveZoneModel"></param>
        public void SetSchExclusivUpdate(HeaderModel header, SchExclusiveZoneModel schExclusiveZoneModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchExclusivUpdate() Start");
                _log.Debug("-----------------------------------------");

                _log.Debug("ItemNo         :[" + schExclusiveZoneModel.ItemNo + "]");
                _log.Debug("TgtTimeYn      :[" + schExclusiveZoneModel.TgtTimeYn + "]");
                _log.Debug("TgtTime        :[" + schExclusiveZoneModel.TgtTime + "]");
                _log.Debug("TgtWeekYn        :[" + schExclusiveZoneModel.TgtWeekYn + "]");
                _log.Debug("TgtWeek        :[" + schExclusiveZoneModel.TgtWeek + "]");

                int i = 0;
                StringBuilder sbQuery;
                SqlParameter[] sqlParams = new SqlParameter[30];

                i = 0;
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int, 4);
                sqlParams[i++] = new SqlParameter("@TgtTimeYn", SqlDbType.Char, 1);
                sqlParams[i++] = new SqlParameter("@TgtTime", SqlDbType.VarChar, 148);
                sqlParams[i++] = new SqlParameter("@TgtWeekYn", SqlDbType.Char, 1);
                sqlParams[i++] = new SqlParameter("@TgtWeek", SqlDbType.VarChar, 13);


                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schExclusiveZoneModel.ItemNo);
                sqlParams[i++].Value = schExclusiveZoneModel.TgtTimeYn;
                sqlParams[i++].Value = schExclusiveZoneModel.TgtTime;
                sqlParams[i++].Value = schExclusiveZoneModel.TgtWeekYn;
                sqlParams[i++].Value = schExclusiveZoneModel.TgtWeek;

                sbQuery = new StringBuilder();

                sbQuery.Append(" UPDATE Targeting							\n");
                sbQuery.Append("    SET TgtTimeYn     	= @TgtTimeYn        \n");
                sbQuery.Append("       ,TgtTime       	= @TgtTime			\n");
                sbQuery.Append("       ,TgtWeekYn       = @TgtWeekYn		\n");
                sbQuery.Append("       ,TgtWeek       	= @TgtWeek			\n");
                sbQuery.Append("  WHERE ItemNo          = @ItemNo	        \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                // __MESSAGE__
                _log.Message("광고 타겟팅 수정:[" + schExclusiveZoneModel.ItemNo + "] 등록자:[" + header.UserID + "]");

                schExclusiveZoneModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetSchExclusivUpdate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schExclusiveZoneModel.ResultCD = "3101";
                schExclusiveZoneModel.ResultDesc = "광고 타겟팅 수정 중 오류가 발생하였습니다";
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