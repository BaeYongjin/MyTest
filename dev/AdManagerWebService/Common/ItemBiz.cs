/*
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : YJ.Park
 * 수정일    : 2014.08.08
 * 수정내용  : 기편성 광고조회 추가(GetSchAdItemList()함수 추가) 
 * --------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자		: YJ.Park
 * 수정일		: 2014.11.13
 * 수정내용	: GetContractKidsItemList() 함수 추가
 *				  HomeCount에 홈광고(키즈) 편성 추가
 * --------------------------------------------------------*/
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

namespace AdManagerWebService.Common
{
    public class ItemBiz : BaseBiz
    {
        #region 생성자
        public ItemBiz()
            : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        //SchHomeAdControl.cs 기서 호출 했었음
        /// <summary>
        /// 홈광고편성대상조회
        /// </summary>
        /// <param name="itemModel"></param>
        public void GetContractItemList(HeaderModel header, ItemModel itemModel)
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
                _log.Debug("SearchKey            :[" + itemModel.SearchKey + "]");		// 검색어
                _log.Debug("SearchMediaCode	     :[" + itemModel.SearchMediaCode + "]");		// 검색 매체

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                // CSS,시청지도,중간광고,기본광고등은 조회조건에서 제외됨
                sbQuery.AppendLine("     SELECT 'False' AS CheckYn            ");
                sbQuery.AppendLine("          ,A.item_no as ItemNo            ");
                sbQuery.AppendLine("          ,A.item_nm as ItemName          ");
                sbQuery.AppendLine("          ,B.cntr_nm as ContractName      ");
                sbQuery.AppendLine("          ,C.advter_nm as AdvertiserName  ");
                sbQuery.AppendLine("          ,A.begin_dy as ExcuteStartDay   ");
                sbQuery.AppendLine("          ,A.end_dy as ExcuteEndDay       ");
                sbQuery.AppendLine("          ,A.rl_end_dy as RealEndDay      ");
                sbQuery.AppendLine("          ,A.advt_stt as AdState          ");
                sbQuery.AppendLine("          ,D.stm_cod_nm as  AdStateName   ");
                sbQuery.AppendLine("          ,0 AS HomeCount                 ");
                sbQuery.AppendLine("          ,(SELECT COUNT(*) FROM SCHD_MENU WHERE item_no = A.item_no) AS MenuCount      ");
                sbQuery.AppendLine("          ,(SELECT COUNT(*) FROM SCHD_TITLE   WHERE item_no = A.item_no) AS ChannelCount  ");
                sbQuery.AppendLine("          ,TO_CHAR(SYSDATE,'YYYY-MM-DD') AS NowDay          ");
                sbQuery.AppendLine("          ,a.advt_typ as AdType           ");
                sbQuery.AppendLine("          ,E.stm_cod_nm AS AdTypeName     ");
                sbQuery.AppendLine("    FROM ADVT_MST    A                      ");
                sbQuery.AppendLine("    INNER JOIN CNTR	 B  ON (A.cntr_seq = B.cntr_seq)    ");
                sbQuery.AppendLine("    LEFT JOIN ADVTER	 C  ON (B.advter_cod = C.advter_cod)");
                sbQuery.AppendLine("    LEFT JOIN STM_COD 	D  ON (A.advt_stt        = D.stm_cod      AND D.stm_cod_cls = '25') ");
                sbQuery.AppendLine("    LEFT JOIN STM_COD	  E  ON (A.advt_typ        = E.stm_cod      AND E.stm_cod_cls = '26') ");
                sbQuery.AppendLine("    WHERE 1=1       ");
                sbQuery.AppendLine("    AND A.advt_typ not in('13','16','17','90','99')  ");

                // 매체을 선택했으면
                if (itemModel.SearchMediaCode.Trim().Length > 0 && !itemModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND b.mda_cod  = '" + itemModel.SearchMediaCode.Trim() + "' \n");
                }

                // 매체을 선택했으면
                if (itemModel.RapCode.Trim().Length > 0 && !itemModel.RapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.rep_cod  = '" + itemModel.RapCode.Trim() + "' \n");
                }

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if (itemModel.SearchchkAdState_10.Trim().Length > 0 && itemModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.advt_stt  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (itemModel.SearchchkAdState_20.Trim().Length > 0 && itemModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (itemModel.SearchchkAdState_30.Trim().Length > 0 && itemModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (itemModel.SearchchkAdState_40.Trim().Length > 0 && itemModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");


                // 검색어가 있으면
                if (itemModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( A.item_nm       LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.cntr_nm   LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY A.item_no Desc ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                itemModel.ScheduleDataSet = ds.Copy();
                // 결과
                itemModel.ResultCnt = Utility.GetDatasetCount(itemModel.ScheduleDataSet);
                // 결과코드 셋트
                itemModel.ResultCD = "0000";


                // 2007.10.02 RH.Jun 파일리스트 건수검사용

                sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"                    
                    + "  SELECT COUNT(*) AS FileListCnt   \n"
                    + "    FROM ADVT_MST          \n"
                    + "   WHERE file_stt = '30'      \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                itemModel.FileListCount = Convert.ToInt32(ds.Tables[0].Rows[0]["FileListCnt"].ToString());
                ds.Dispose();


                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + itemModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                itemModel.ResultCD = "3000";
                itemModel.ResultDesc = "홈광고편성현황 조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }



        // SchChoiceAdControl.cs 기서 호출했었음
        /// <summary>
        /// 광고기준편성, 편성단위기준 편성 업무에서 광고목록을 찾아온다 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="itemModel"></param>
        public void GetContractItemList_0907a(HeaderModel header, ItemModel itemModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList_0907a() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey            :[" + itemModel.SearchKey + "]");	// 검색어
                _log.Debug("SearchMediaCode	     :[" + itemModel.SearchMediaCode + "]");		// 검색 매체
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n SELECT 'False'                                                      AS CheckYn");
                sbQuery.Append("\n     ,   A.ITEM_NO                                                   AS ItemNo");
                sbQuery.Append("\n     ,   A.ITEM_NM                                                   AS ItemName");
                sbQuery.Append("\n     ,   B.CNTR_NM                                                   AS ContractName");
                sbQuery.Append("\n     ,   C.ADVTER_NM                                                 AS AdvertiserName");
                sbQuery.Append("\n     ,   A.BEGIN_DY                                                  AS ExcuteStartDay");
                sbQuery.Append("\n     ,   A.END_DY                                                    AS ExcuteEndDay");
                sbQuery.Append("\n     ,   A.RL_END_DY                                                 AS RealEndDay");
                sbQuery.Append("\n     ,   A.ADVT_STT                                                  AS AdState");
                sbQuery.Append("\n     ,   GET_STMCODNM('25', A.ADVT_STT)                              AS AdStateName");
                sbQuery.Append("\n     ,   0                                                           AS HomeCount");
                sbQuery.Append("\n     , ( SELECT COUNT(*) FROM SCHD_MENU  WHERE ITEM_NO = A.ITEM_NO)  AS MenuCount");
                sbQuery.Append("\n     , ( SELECT COUNT(*) FROM SCHD_TITLE WHERE ITEM_NO = A.ITEM_NO)  AS ChannelCount");
                sbQuery.Append("\n     ,   TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS')                    AS NowDay");
                sbQuery.Append("\n     ,   A.ADVT_TYP                                                  AS AdType");
                sbQuery.Append("\n     ,   GET_STMCODNM('26', A.ADVT_TYP)                              AS AdTypeName");
                sbQuery.Append("\n FROM    ADVT_MST A ");
                sbQuery.Append("\n     INNER JOIN  CNTR    B ON    B.CNTR_SEQ = A.CNTR_SEQ");
                sbQuery.Append("\n     LEFT  JOIN  ADVTER  C ON    C.ADVTER_COD = B.ADVTER_COD");
                sbQuery.Append("\n WHERE 1 = 1");

                // 광고타입별은 일단 보류
                //if (itemModel.SearchAdType.Length > 0 && itemModel.SearchAdType.Trim().Equals("000"))
                //{
                //    //전체
                //    sbQuery.Append(" AND a.AdType In(10,11,12,16,17,19,20) \n");
                //}
                //else if (itemModel.SearchAdType.Length > 0 && itemModel.SearchAdType.Trim().Equals("100"))
                //{
                //    // 상업광고류
                //    sbQuery.Append(" AND a.AdType In(10,16,17,19) \n");
                //}
                //else if (itemModel.SearchAdType.Length > 0 && itemModel.SearchAdType.Trim().Equals("200"))
                //{
                //    // 매체광고류
                //    sbQuery.Append(" AND a.AdType In(11,12,20) \n");
                //}
                //else
                //{
                //    sbQuery.Append(" AND a.AdType = " + itemModel.SearchAdType.Trim() + " \n");
                //}


                bool isState = false;

                // 광고상태 준비
                if (itemModel.SearchchkAdState_10.Trim().Length > 0 && itemModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.ADVT_STT  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (itemModel.SearchchkAdState_20.Trim().Length > 0 && itemModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.ADVT_STT  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (itemModel.SearchchkAdState_30.Trim().Length > 0 && itemModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.ADVT_STT  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (itemModel.SearchchkAdState_40.Trim().Length > 0 && itemModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.ADVT_STT  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");



                // 검색어가 있으면
                if (itemModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( A.ITEM_NM    LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.CNTR_NM	LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n");
                }

                sbQuery.Append("  ORDER BY A.ITEM_NO DESC ");

                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                itemModel.ScheduleDataSet = ds.Copy();
                // 결과
                itemModel.ResultCnt = Utility.GetDatasetCount(itemModel.ScheduleDataSet);
                // 결과코드 셋트
                itemModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + itemModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                itemModel.ResultCD = "3000";
                itemModel.ResultDesc = "지정광고 편성대상조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        #region 홈광고편성대상조회


        /// <summary>
        /// 홈상업광고용 편성대상 광고내역리스트 조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="itemModel"></param>
        public void GetContractItemListCm(HeaderModel header, ItemModel itemModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey            :[" + itemModel.SearchKey + "]");		// 검색어
                _log.Debug("SearchMediaCode	     :[" + itemModel.SearchMediaCode + "]");		// 검색 매체

                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                // CSS,시청지도,중간광고,기본광고등은 조회조건에서 제외됨
                sbQuery.AppendLine("     SELECT 'False' AS CheckYn            ");
                sbQuery.AppendLine("          ,A.item_no as ItemNo            ");
                sbQuery.AppendLine("          ,A.item_nm as ItemName          ");
                sbQuery.AppendLine("          ,B.cntr_nm as ContractName      ");
                sbQuery.AppendLine("          ,C.advter_nm as AdvertiserName  ");
                sbQuery.AppendLine("          ,A.begin_dy as ExcuteStartDay   ");
                sbQuery.AppendLine("          ,A.end_dy as ExcuteEndDay       ");
                sbQuery.AppendLine("          ,A.rl_end_dy as RealEndDay      ");
                sbQuery.AppendLine("          ,A.advt_stt as AdState          ");
                sbQuery.AppendLine("          ,D.stm_cod_nm as  AdStateName   ");
                sbQuery.AppendLine("          ,0 AS HomeCount                 ");
                sbQuery.AppendLine("          ,(SELECT COUNT(*) FROM SCHD_MENU WHERE item_no = A.item_no) AS MenuCount      ");
                sbQuery.AppendLine("          ,(SELECT COUNT(*) FROM SCHD_TITLE   WHERE item_no = A.item_no) AS ChannelCount  ");
                sbQuery.AppendLine("          ,TO_CHAR(SYSDATE,'YYYY-MM-DD') AS NowDay          ");
                sbQuery.AppendLine("          ,a.advt_typ as AdType           ");
                sbQuery.AppendLine("          ,E.stm_cod_nm AS AdTypeName     ");
                sbQuery.AppendLine("    FROM ADVT_MST    A                      ");
                sbQuery.AppendLine("    INNER JOIN CNTR	 B  ON (A.cntr_seq = B.cntr_seq)    ");
                sbQuery.AppendLine("    LEFT JOIN ADVTER	 C  ON (B.advter_cod = C.advter_cod)");
                sbQuery.AppendLine("    LEFT JOIN STM_COD 	D  ON (A.advt_stt        = D.stm_cod      AND D.stm_cod_cls = '25') ");
                sbQuery.AppendLine("    LEFT JOIN STM_COD	  E  ON (A.advt_typ        = E.stm_cod      AND E.stm_cod_cls = '26') ");
                sbQuery.AppendLine("    WHERE 1=1       ");
                sbQuery.AppendLine("    AND A.advt_typ not in('11','12','13','16','17','20','90','99') ");


                // 매체을 선택했으면
                if (itemModel.SearchMediaCode.Trim().Length > 0 && !itemModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND b.mda_cod  = '" + itemModel.SearchMediaCode.Trim() + "' \n");
                }

                // 매체을 선택했으면
                if (itemModel.RapCode.Trim().Length > 0 && !itemModel.RapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.rep_cod  = '" + itemModel.RapCode.Trim() + "' \n");
                }

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if (itemModel.SearchchkAdState_10.Trim().Length > 0 && itemModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.advt_stt  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (itemModel.SearchchkAdState_20.Trim().Length > 0 && itemModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (itemModel.SearchchkAdState_30.Trim().Length > 0 && itemModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (itemModel.SearchchkAdState_40.Trim().Length > 0 && itemModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");


                // 검색어가 있으면
                if (itemModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( A.item_nm       LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.cntr_nm   LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY A.Item_No Desc ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                itemModel.ScheduleDataSet = ds.Copy();
                itemModel.ResultCnt = Utility.GetDatasetCount(itemModel.ScheduleDataSet);
                itemModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                itemModel.ResultCD = "3000";
                itemModel.ResultDesc = "홈광고편성현황 조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        #endregion

        #region 지정광고 편성대상조회

        /// <summary>
        /// 지정광고 편성대상조회
        /// </summary>
        /// <param name="itemModel"></param>
        public void GetContractItemListForCug(HeaderModel header, ItemModel itemModel)
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
                _log.Debug("SearchKey            :[" + itemModel.SearchKey + "]");		// 검색어
                _log.Debug("SearchMediaCode	     :[" + itemModel.SearchMediaCode + "]");		// 검색 매체

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.AppendLine("     SELECT 'False' AS CheckYn            ");
                sbQuery.AppendLine("          ,A.item_no as ItemNo            ");
                sbQuery.AppendLine("          ,A.item_nm as ItemName          ");
                sbQuery.AppendLine("          ,B.cntr_nm as ContractName      ");
                sbQuery.AppendLine("          ,C.advter_nm as AdvertiserName  ");
                sbQuery.AppendLine("          ,A.begin_dy as ExcuteStartDay   ");
                sbQuery.AppendLine("          ,A.end_dy as ExcuteEndDay       ");
                sbQuery.AppendLine("          ,A.rl_end_dy as RealEndDay      ");
                sbQuery.AppendLine("          ,A.advt_stt as AdState          ");
                sbQuery.AppendLine("          ,D.stm_cod_nm as  AdStateName   ");
                sbQuery.AppendLine("          ,0 AS HomeCount                 ");
                sbQuery.AppendLine("          ,0 AS MenuCount      ");
                sbQuery.AppendLine("          ,0 AS ChannelCount  ");
                sbQuery.AppendLine("          ,TO_CHAR(SYSDATE,'YYYY-MM-DD') AS NowDay          ");
                sbQuery.AppendLine("          ,a.advt_typ as AdType           ");
                sbQuery.AppendLine("          ,E.stm_cod_nm AS AdTypeName     ");
                sbQuery.AppendLine("          ,'' as CugYn                    ");
                sbQuery.AppendLine("    FROM ADVT_MST    A                      ");
                sbQuery.AppendLine("    INNER JOIN CNTR	 B  ON (A.cntr_seq = B.cntr_seq)    ");
                sbQuery.AppendLine("    LEFT JOIN ADVTER	 C  ON (B.advter_cod = C.advter_cod)");
                sbQuery.AppendLine("    LEFT JOIN STM_COD 	D  ON (A.advt_stt        = D.stm_cod      AND D.stm_cod_cls = '25') ");
                sbQuery.AppendLine("    LEFT JOIN STM_COD	  E  ON (A.advt_typ        = E.stm_cod      AND E.stm_cod_cls = '26') ");
                sbQuery.AppendLine("    WHERE 1=1       ");

                // 매체을 선택했으면
                if (itemModel.SearchMediaCode.Trim().Length > 0 && !itemModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND b.mda_cod  = '" + itemModel.SearchMediaCode.Trim() + "' \n");
                }

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if (itemModel.SearchchkAdState_10.Trim().Length > 0 && itemModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.advt_stt  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (itemModel.SearchchkAdState_20.Trim().Length > 0 && itemModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (itemModel.SearchchkAdState_30.Trim().Length > 0 && itemModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (itemModel.SearchchkAdState_40.Trim().Length > 0 && itemModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");



                // 검색어가 있으면
                if (itemModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( A.item_nm       LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.cntr_nm   LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY A.item_no DESC ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                itemModel.ScheduleDataSet = ds.Copy();
                // 결과
                itemModel.ResultCnt = Utility.GetDatasetCount(itemModel.ScheduleDataSet);
                // 결과코드 셋트
                itemModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + itemModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                itemModel.ResultCD = "3000";
                itemModel.ResultDesc = "지정광고 편성대상조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }
        #endregion

        #region 듀얼광고 연결 대상조회
        /// <summary>
        /// 듀얼광고 설정대상 조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="itemModel"></param>
        public void GetContractItemListDual(HeaderModel header, ItemModel itemModel)
        {
            try
            {
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemListDual() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey            :[" + itemModel.SearchKey + "]");		// 검색어
                _log.Debug("SearchMediaCode	     :[" + itemModel.SearchMediaCode + "]");		// 검색 매체


                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                // 상업광고중 해당 광고주의 광고만 대상임
                sbQuery.AppendLine("     SELECT 'False' AS CheckYn            ");
                sbQuery.AppendLine("          ,A.item_no as ItemNo            ");
                sbQuery.AppendLine("          ,A.item_nm as ItemName          ");
                sbQuery.AppendLine("          ,B.cntr_nm as ContractName      ");
                sbQuery.AppendLine("          ,C.advter_nm as AdvertiserName  ");
                sbQuery.AppendLine("          ,A.begin_dy as ExcuteStartDay   ");
                sbQuery.AppendLine("          ,A.end_dy as ExcuteEndDay       ");
                sbQuery.AppendLine("          ,A.rl_end_dy as RealEndDay      ");
                sbQuery.AppendLine("          ,A.advt_stt as AdState          ");
                sbQuery.AppendLine("          ,D.stm_cod_nm as  AdStateName   ");
                sbQuery.AppendLine("          ,0 AS HomeCount                 ");
                sbQuery.AppendLine("          ,(SELECT COUNT(*) FROM SCHD_MENU WHERE item_no = A.item_no) AS MenuCount      ");
                sbQuery.AppendLine("          ,(SELECT COUNT(*) FROM SCHD_TITLE   WHERE item_no = A.item_no) AS ChannelCount  ");
                sbQuery.AppendLine("          ,TO_CHAR(SYSDATE,'YYYY-MM-DD') AS NowDay          ");
                sbQuery.AppendLine("          ,a.advt_typ as AdType           ");
                sbQuery.AppendLine("          ,E.stm_cod_nm AS AdTypeName     ");
                sbQuery.AppendLine("    FROM ADVT_MST    A                      ");
                sbQuery.AppendLine("    INNER JOIN CNTR	 B  ON (A.cntr_seq = B.cntr_seq)    ");
                sbQuery.AppendLine("    LEFT JOIN ADVTER	 C  ON (B.advter_cod = C.advter_cod)");
                sbQuery.AppendLine("    LEFT JOIN STM_COD 	D  ON (A.advt_stt        = D.stm_cod      AND D.stm_cod_cls = '25') ");
                sbQuery.AppendLine("    LEFT JOIN STM_COD	  E  ON (A.advt_typ        = E.stm_cod      AND E.stm_cod_cls = '26') ");
                sbQuery.AppendLine("    WHERE 1=1       ");
                sbQuery.AppendLine("    AND a.advt_typ = '10' ");
                sbQuery.AppendLine("    AND c.advter_cod = '" + itemModel.SearchAdvertiserCode + "' ");
                // 렙
                if (itemModel.RapCode.Trim().Length > 0 && !itemModel.RapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.rep_cod  = '" + itemModel.RapCode.Trim() + "' \n");
                }

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if (itemModel.SearchchkAdState_10.Trim().Length > 0 && itemModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.advt_stt  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (itemModel.SearchchkAdState_20.Trim().Length > 0 && itemModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (itemModel.SearchchkAdState_30.Trim().Length > 0 && itemModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (itemModel.SearchchkAdState_40.Trim().Length > 0 && itemModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");


                // 검색어가 있으면
                if (itemModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( A.item_nm       LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.cntr_nm   LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY A.item_no Desc ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                itemModel.ScheduleDataSet = ds.Copy();
                itemModel.ResultCnt = Utility.GetDatasetCount(itemModel.ScheduleDataSet);
                itemModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                itemModel.ResultCD = "3000";
                itemModel.ResultDesc = "홈광고편성현황 조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }
        #endregion

        #region 기편성 광고조회 [E_01]

        /// <summary>
        /// 편성정보가 존재하는 광고목록(선택된 광고를 제외하고) 조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="itemModel"></param>
        public void GetSchAdItemList(HeaderModel header, ItemModel itemModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchAdItemList() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey            :[" + itemModel.SearchKey + "]");		        // 검색어
                _log.Debug("SearchMediaCode	     :[" + itemModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("ItemNo               :[" + itemModel.ItemNo + "]");

                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();
                SqlParameter sqlParamItemNo = new SqlParameter("@ItemNo", SqlDbType.Int);

                sqlParamItemNo.Value = Convert.ToInt32(itemModel.ItemNo);

                DataSet ds = new DataSet();
                // 쿼리생성
                sbQuery.AppendLine("     SELECT 'False' AS CheckYn            ");
                sbQuery.AppendLine("          ,A.item_no as ItemNo            ");
                sbQuery.AppendLine("          ,A.item_nm as ItemName          ");
                sbQuery.AppendLine("          ,B.cntr_nm as ContractName      ");
                sbQuery.AppendLine("          ,C.advter_nm as AdvertiserName  ");
                sbQuery.AppendLine("          ,A.begin_dy as ExcuteStartDay   ");
                sbQuery.AppendLine("          ,A.end_dy as ExcuteEndDay       ");
                sbQuery.AppendLine("          ,A.rl_end_dy as RealEndDay      ");
                sbQuery.AppendLine("          ,A.advt_stt as AdState          ");
                sbQuery.AppendLine("          ,D.stm_cod_nm as  AdStateName   ");
                sbQuery.AppendLine("          ,0 AS HomeCount                 ");
                sbQuery.AppendLine("          ,(SELECT COUNT(*) FROM SCHD_MENU WHERE item_no = A.item_no) AS MenuCount      ");
                sbQuery.AppendLine("          ,(SELECT COUNT(*) FROM SCHD_TITLE   WHERE item_no = A.item_no) AS ChannelCount  ");
                sbQuery.AppendLine("          ,TO_CHAR(SYSDATE,'YYYY-MM-DD') AS NowDay          ");
                sbQuery.AppendLine("          ,a.advt_typ as AdType           ");
                sbQuery.AppendLine("          ,E.stm_cod_nm AS AdTypeName     ");
                sbQuery.AppendLine("    FROM ADVT_MST    A                      ");
                sbQuery.AppendLine("    INNER JOIN CNTR	 B  ON (A.cntr_seq = B.cntr_seq)    ");
                sbQuery.AppendLine("    LEFT JOIN ADVTER	 C  ON (B.advter_cod = C.advter_cod)");
                sbQuery.AppendLine("    LEFT JOIN STM_COD 	D  ON (A.advt_stt        = D.stm_cod      AND D.stm_cod_cls = '25') ");
                sbQuery.AppendLine("    LEFT JOIN STM_COD	  E  ON (A.advt_typ        = E.stm_cod      AND E.stm_cod_cls = '26') ");
                sbQuery.AppendLine("    WHERE 1=1       ");
                sbQuery.AppendLine("    AND a.advt_typ not in('11','12','13','16','17','20','90','99')");
                sbQuery.AppendLine("    AND a.item_no not in (:ItemNo) ");
                sbQuery.AppendLine("    AND ((SELECT COUNT(*) FROM SCHD_MENU F WHERE A.item_no = f.item_no) > 0 ");
                sbQuery.AppendLine("    OR   (SELECT COUNT(*) FROM SCHD_TITLE G WHERE a.item_no = g.item_no) > 0) ");

                //광고 매체...
                if (itemModel.SearchMediaCode.Trim().Length > 0 && !itemModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND b.mda_cod  = '" + itemModel.SearchMediaCode.Trim() + "' \n");
                }

                // 광고상태 선택에 따라
                bool isState = false;

                // 광고상태 준비
                if (itemModel.SearchchkAdState_10.Trim().Length > 0 && itemModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.advt_stt  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (itemModel.SearchchkAdState_20.Trim().Length > 0 && itemModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (itemModel.SearchchkAdState_30.Trim().Length > 0 && itemModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (itemModel.SearchchkAdState_40.Trim().Length > 0 && itemModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");


                // 검색어가 있으면
                if (itemModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n");
                    sbQuery.Append("  AND ( A.item_nm       LIKE '%" + itemModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR B.cntr_nm   LIKE '%" + itemModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("  ) \n");
                }

                sbQuery.Append("  ORDER BY A.item_no Desc ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParamItemNo);

                // 결과 DataSet의 장르그룹모델에 복사
                itemModel.ScheduleDataSet = ds.Copy();
                itemModel.ResultCnt = Utility.GetDatasetCount(itemModel.ScheduleDataSet);
                itemModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchAdItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                itemModel.ResultCD = "3000";
                itemModel.ResultDesc = "기편성 광고 목록 조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        #endregion

        #region 홈광고(키즈)편성 대상조회 [E_02]
        /// <summary>
        /// 홈광고(키즈)편성 대상 조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="itemModel"></param>
        public void GetContractKidsItemList(HeaderModel header, ItemModel itemModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractKidsItemList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey            :[" + itemModel.SearchKey + "]");		// 검색어
                _log.Debug("SearchMediaCode	     :[" + itemModel.SearchMediaCode + "]");		// 검색 매체

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                // OAP광고& 셋탑박스 타입이 가온(스마트셋탑전용)인것만 가져옴
                sbQuery.AppendLine("     SELECT 'False' AS CheckYn            ");
                sbQuery.AppendLine("          ,A.item_no as ItemNo            ");
                sbQuery.AppendLine("          ,A.item_nm as ItemName          ");
                sbQuery.AppendLine("          ,B.cntr_nm as ContractName      ");
                sbQuery.AppendLine("          ,C.advter_nm as AdvertiserName  ");
                sbQuery.AppendLine("          ,A.begin_dy as ExcuteStartDay   ");
                sbQuery.AppendLine("          ,A.end_dy as ExcuteEndDay       ");
                sbQuery.AppendLine("          ,A.rl_end_dy as RealEndDay      ");
                sbQuery.AppendLine("          ,A.advt_stt as AdState          ");
                sbQuery.AppendLine("          ,D.stm_cod_nm as  AdStateName   ");
                sbQuery.AppendLine("          ,0 AS HomeCount                 ");
                sbQuery.AppendLine("          ,(SELECT COUNT(*) FROM SCHD_MENU WHERE item_no = A.item_no) AS MenuCount      ");
                sbQuery.AppendLine("          ,(SELECT COUNT(*) FROM SCHD_TITLE   WHERE item_no = A.item_no) AS ChannelCount  ");
                sbQuery.AppendLine("          ,TO_CHAR(SYSDATE,'YYYY-MM-DD') AS NowDay          ");
                sbQuery.AppendLine("          ,a.advt_typ as AdType           ");
                sbQuery.AppendLine("          ,E.stm_cod_nm AS AdTypeName     ");
                sbQuery.AppendLine("    FROM ADVT_MST    A                      ");
                sbQuery.AppendLine("    INNER JOIN CNTR	 B  ON (A.cntr_seq = B.cntr_seq)    ");
                sbQuery.AppendLine("    LEFT JOIN ADVTER	 C  ON (B.advter_cod = C.advter_cod)");
                sbQuery.AppendLine("    LEFT JOIN STM_COD 	D  ON (A.advt_stt        = D.stm_cod      AND D.stm_cod_cls = '25') ");
                sbQuery.AppendLine("    LEFT JOIN STM_COD	  E  ON (A.advt_typ        = E.stm_cod      AND E.stm_cod_cls = '26') ");
                sbQuery.AppendLine("    WHERE 1=1       ");
                sbQuery.AppendLine("    AND A.advt_typ = '20' ");

                // 매체을 선택했으면
                if (itemModel.SearchMediaCode.Trim().Length > 0 && !itemModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.mda_cod  = '" + itemModel.SearchMediaCode.Trim() + "' \n");
                }

                // 매체을 선택했으면
                if (itemModel.RapCode.Trim().Length > 0 && !itemModel.RapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.RapCode  = '" + itemModel.RapCode.Trim() + "' \n");
                }

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if (itemModel.SearchchkAdState_10.Trim().Length > 0 && itemModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.advt_stt  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (itemModel.SearchchkAdState_20.Trim().Length > 0 && itemModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (itemModel.SearchchkAdState_30.Trim().Length > 0 && itemModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (itemModel.SearchchkAdState_40.Trim().Length > 0 && itemModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");


                // 검색어가 있으면
                if (itemModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( A.item_nm       LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.cntr_nm   LIKE '%" + itemModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY A.item_no Desc ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                itemModel.ScheduleDataSet = ds.Copy();
                // 결과
                itemModel.ResultCnt = Utility.GetDatasetCount(itemModel.ScheduleDataSet);
                // 결과코드 셋트
                itemModel.ResultCD = "0000";


                // 2007.10.02 RH.Jun 파일리스트 건수검사용

                sbQuery = new StringBuilder();

                // 쿼리생성
                //sbQuery.Append("\n SELECT (																								");
                //sbQuery.Append("\n   SELECT COUNT(*) AS HomeCnt																");
                //sbQuery.Append("\n     FROM SchHomeKids A INNER JOIN ContractItem B ON (A.ItemNo = B.ItemNo)	");
                //sbQuery.Append("\n      AND B.ExcuteStartDay	<= Convert(varchar(8),getdate(),112)					");
                //sbQuery.Append("\n      AND B.RealEndDay		>= Convert(varchar(8),getdate(),112)					");
                //sbQuery.Append("\n      AND B.AdState   = '20'																		");
                //sbQuery.Append("\n      AND B.FileState = '30'																		");
                //sbQuery.Append("\n ) + (																									");
                //sbQuery.Append("\n   SELECT COUNT(*) AS FileCnt																");
                //sbQuery.Append("\n     FROM ContractItem          																	");
                //sbQuery.Append("\n    WHERE FileState = '30'      																	");
                //sbQuery.Append("\n ) AS FileListCnt               																		");
                sbQuery.AppendLine(" SELECT COUNT(*) as FileListCnt FROM ADVT_MST ");
                sbQuery.AppendLine(" WHERE file_stt = '30' ");
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                itemModel.FileListCount = Convert.ToInt32(ds.Tables[0].Rows[0]["FileListCnt"].ToString());
                ds.Dispose();


                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + itemModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractKidsItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                itemModel.ResultCD = "3000";
                itemModel.ResultDesc = "홈광고(키즈)편성대상 조회 중 오류가 발생하였습니다";
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