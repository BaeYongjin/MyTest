// ===============================================================================
//
// SchChoiceAdBiz.cs
//
// 지정광고 편성처리 서비스 
//
// ===============================================================================
// Release history
// 2007.09.04 RH.Jung 상업광고 편성불가 컨텐츠에 대한처리
// 2007.10.11 RH.Jung 광고삭제시 편성승인번호가 생성되지 않아 편성승인을 할 수 없는 로직오류 수정
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// 

/*
 * -------------------------------------------------------
 * Class Name: SchChoiceAdBiz.cs
 * 주요기능  : 지정광고 편성처리 서비스
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.05.27
 * 수정내용  :        
 *            - 광고종류도 조회되도록 광고목록조회 쿼리문 수정
 * 수정함수  :
 *            - GetAdList10()
 * -------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : YJ.PARK
 * 수정일    : 2014.08.8
 * 수정내용  :        
 *            - 편성 복사 기능 추가
 * 수정함수  :
 *            - SetSchChoiceAdCopy()
 * -------------------------------------------------------
 */

using System;
using System.Data;
using System.Data.SqlClient;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Schedule
{
    /// <summary>
    /// SchChoiceAdBiz에 대한 요약 설명입니다.
    /// </summary>
    public class SchChoiceAdBiz : BaseBiz
    {
        #region 생성자
        public SchChoiceAdBiz()
            : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region 지정광고 편성현황 조회
        /// <summary>
        /// 지정광고 편성현황 조회(광고별 편성)
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetSchChoiceAdList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            bool isState = false;

            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchChoiceAdList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey            :[" + schChoiceAdModel.SearchKey + "]");		// 검색어
                _log.Debug("SearchMediaCode	     :[" + schChoiceAdModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchRapCode        :[" + schChoiceAdModel.SearchRapCode + "]");		// 검색 랩
                _log.Debug("SearchAgencyCode     :[" + schChoiceAdModel.SearchAgencyCode + "]");		// 검색 대행사
                _log.Debug("SearchAdvertiserCode :[" + schChoiceAdModel.SearchAdvertiserCode + "]");		// 검색 광고주
                _log.Debug("SearchContractState  :[" + schChoiceAdModel.SearchContractState + "]");		// 검색 계약상태
                _log.Debug("SearchAdClass        :[" + schChoiceAdModel.SearchAdClass + "]");		// 검색 광고용도
                _log.Debug("SearchchkAdState_20  :[" + schChoiceAdModel.SearchchkAdState_20 + "]");		// 검색 광고상태 : 편성
                _log.Debug("SearchchkAdState_30  :[" + schChoiceAdModel.SearchchkAdState_30 + "]");		// 검색 광고상태 : 중지	
                _log.Debug("SearchchkAdState_40  :[" + schChoiceAdModel.SearchchkAdState_40 + "]");		// 검색 광고상태 : 종료           

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n SELECT  B.ADVT_TYP  AS AdType");
                sbQuery.Append("\n     ,   GET_STMCODNM('26', B.ADVT_TYP)  AS AdTypeName       -- 26:광고종류");
                sbQuery.Append("\n     ,   A.ITEM_NO     AS ItemNo");
                sbQuery.Append("\n     ,   B.ITEM_NM     AS ItemName");
                sbQuery.Append("\n     ,   C.CNTR_NM     AS ContractName");
                sbQuery.Append("\n     ,   D.ADVTER_NM    AS AdvertiserName");
                sbQuery.Append("\n     ,   C.CNTR_STT    AS ContState");
                sbQuery.Append("\n     ,   GET_STMCODNM('23', C.CNTR_STT)  AS ContStateName        -- 23:계약상태");
                sbQuery.Append("\n     ,   B.RL_END_DY   AS RealEndDay");
                sbQuery.Append("\n     ,   GET_STMCODNM('29', B.ADVT_CLSS) AS AdClassName         -- 29:광고용도");
                sbQuery.Append("\n     ,   B.ADVT_STT    AS AdState");
                sbQuery.Append("\n     ,   B.ADVT_RATE   AS AdRate");
                sbQuery.Append("\n     ,   GET_STMCODNM('25',B.ADVT_STT)   AS AdStatename          -- 25:광고상태");
                sbQuery.Append("\n     , ( SELECT COUNT(*) FROM SCHD_MENU  WHERE ITEM_NO = A.ITEM_NO) AS MenuCount");
                sbQuery.Append("\n     , ( SELECT COUNT(*) FROM SCHD_TITLE WHERE ITEM_NO = A.ITEM_NO) AS ChannelCount");
                sbQuery.Append("\n     ,   1             AS MediaCode");
                sbQuery.Append("\n     ,   B.FILE_STT    AS FileState");
                sbQuery.Append("\n     ,   GET_STMCODNM('31', B.FILE_STT)    AS FileStatename        -- 31. 파일상태");
                sbQuery.Append("\n     ,   'False' AS CheckYn");
                sbQuery.Append("\n     ,   GET_STMCODNM('27', B.SCH_TYP)     AS ScheduleTypeName     -- 27:편성구분");
                sbQuery.Append("\n  FROM   ( -- 편성되어 있는 대상광고를 뽑아낸다");
                sbQuery.Append("\n             SELECT   DISTINCT ITEM_NO FROM   SCHD_MENU");
                sbQuery.Append("\n             UNION ");
                sbQuery.Append("\n             SELECT   DISTINCT ITEM_NO FROM   SCHD_TITLE");
                sbQuery.Append("\n         ) A");
                sbQuery.Append("\n         INNER JOIN  ADVT_MST    B ON B.ITEM_NO  = A.ITEM_NO      -- 광고내역");
                sbQuery.Append("\n         INNER JOIN  CNTR        C ON C.CNTR_SEQ = B.CNTR_SEQ    -- 광고계약");
                sbQuery.Append("\n         LEFT  JOIN  ADVTER      D ON D.ADVTER_COD= C.ADVTER_COD   -- 광고주");
                sbQuery.Append("\n WHERE 1 = 1");

                if (schChoiceAdModel.AdType.Trim().Length > 0 && !schChoiceAdModel.AdType.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.ADVT_TYP  = " + schChoiceAdModel.AdType.Trim() + " \n");
                }

                // 랩사를 선택했으면
                if (schChoiceAdModel.SearchRapCode.Trim().Length > 0 && !schChoiceAdModel.SearchRapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND C.REP_COD  = " + schChoiceAdModel.SearchRapCode.Trim() + " \n");
                }

                // 광고상태 선택에 따라
                // 광고상태는 20:편성 과 40:종료 사이에 있는 것만 조회한다.
                sbQuery.Append(" AND B.ADVT_STT >= '20' AND B.ADVT_STT <= '40' \n");

                // 광고상태 편성
                if (schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( B.ADVT_STT  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.ADVT_STT  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.ADVT_STT  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");

                // 검색어가 있으면
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( B.ITEM_NM    LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "     OR C.CNTR_NM	LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n");
                }
                sbQuery.Append("  ORDER BY A.ITEM_NO DESC");

                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                schChoiceAdModel.ResultCD = "0000";

                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchChoiceAdList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "광고편성현황 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
        #endregion

        #region 광고리스트
        /// <summary>
        /// 광고목록 가져오기
        /// 편성비율조정에서 조회용으로 만들었으며
        /// 상업광고용으로 만들었슴, 현재는 전용임
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void GetAdList10(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            bool isState = false;

            try
            {
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdList10() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey            :[" + schChoiceAdModel.SearchKey + "]");		// 검색어
                _log.Debug("SearchMediaCode	     :[" + schChoiceAdModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchRapCode        :[" + schChoiceAdModel.SearchRapCode + "]");		// 검색 랩
                _log.Debug("SearchAgencyCode     :[" + schChoiceAdModel.SearchAgencyCode + "]");		// 검색 대행사
                _log.Debug("SearchAdvertiserCode :[" + schChoiceAdModel.SearchAdvertiserCode + "]");		// 검색 광고주
                _log.Debug("SearchContractState  :[" + schChoiceAdModel.SearchContractState + "]");		// 검색 계약상태
                _log.Debug("SearchAdClass        :[" + schChoiceAdModel.SearchAdClass + "]");		// 검색 광고용도
                _log.Debug("SearchchkAdState_20  :[" + schChoiceAdModel.SearchchkAdState_20 + "]");		// 검색 광고상태 : 편성
                _log.Debug("SearchchkAdState_30  :[" + schChoiceAdModel.SearchchkAdState_30 + "]");		// 검색 광고상태 : 중지	
                _log.Debug("SearchchkAdState_40  :[" + schChoiceAdModel.SearchchkAdState_40 + "]");		// 검색 광고상태 : 종료           

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                #region 사용안함 삭제
                /*
                sbQuery.Append("\n SELECT   A.ItemNo ");
                sbQuery.Append("\n 		,B.ItemName ");
                sbQuery.Append("\n 		,B.RealEndDay ");
                sbQuery.Append("\n 		,B.AdState ");
                sbQuery.Append("\n 		,G.CodeName AS AdStatename ");
                sbQuery.Append("\n 		,B.AdRate ");
                sbQuery.Append("\n 		,(select count(distinct Genre)");
                sbQuery.Append("\n 		  from (");
                sbQuery.Append("\n 				select  GenreCode as Genre");
                sbQuery.Append("\n 				from	SchChoiceMenuDetail with(noLock) ");
                sbQuery.Append("\n 				where	ItemNo = A.ItemNo");
                sbQuery.Append("\n 				union all");
                sbQuery.Append("\n 				select  Genre");
                sbQuery.Append("\n 				from	v_adv_contentset aa with(nolock)");
                sbQuery.Append("\n 				inner join SchChoiceChannelDetail bb with(nolock) on bb.ChannelNo = aa.Channel");
                sbQuery.Append("\n 				where	bb.ItemNo = A.ItemNo ) v ) as MenuCount ");
                sbQuery.Append("\n 		,( select count(*) from SchChoiceChannelDetail with(noLock) where ItemNo = A.ItemNo)	as ChannelCount ");
                sbQuery.Append("\n 		,( select count(*) from SchRate nolock where ItemNo = a.ItemNo )	as GroupCount ");
                sbQuery.Append("\n 		,( select isnull(sum(EntryRate),0) from SchRate nolock where ItemNo = a.ItemNo )	as GroupSum ");
                sbQuery.Append("\n 		,( select count(*) from SchRateDetail nolock where ItemNo = a.ItemNo )	as DetailCount ");
                sbQuery.Append("\n  	,B.AdType "); // [E_01]
                sbQuery.Append("\n 	    ,C.CodeName as AdTypeName "); // [E_01]
                sbQuery.Append("\n FROM (	SELECT ItemNo	FROM SchChoiceMenu with(NoLock) ");
                sbQuery.Append("\n         UNION ");
                sbQuery.Append("\n         SELECT ItemNo	FROM SchChoiceChannel with(NoLock) ) AS A ");
                sbQuery.Append("\n INNER JOIN ContractItem    B with(NoLock) ON (A.ItemNo         = B.ItemNo) ");
                sbQuery.Append("\n LEFT  JOIN SystemCode      G with(NoLock) ON (B.AdState        = G.Code AND G.Section = '25') ");
                sbQuery.Append("\n LEFT OUTER JOIN SystemCode C with(NoLock) ON (B.AdType         = C.Code AND C.Section = '26') "); // [E_01]
                sbQuery.Append("\n WHERE b.AdType in('10','19') ");

                // 매체을 선택했으면
                if(schChoiceAdModel.SearchMediaCode.Trim().Length > 0 && !schChoiceAdModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.MediaCode  = " + schChoiceAdModel.SearchMediaCode.Trim() + " \n");
                }	
				
                // 랩사를 선택했으면
                if(schChoiceAdModel.SearchRapCode.Trim().Length > 0 && !schChoiceAdModel.SearchRapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.RapCode  = " + schChoiceAdModel.SearchRapCode.Trim() + " \n");
                }	

                //// 대행사를 선택했으면 - 조회조건 사용안함
                //if(schChoiceAdModel.SearchAgencyCode.Trim().Length > 0 && !schChoiceAdModel.SearchAgencyCode.Trim().Equals("00"))
                //{
                //    sbQuery.Append(" AND B.AgencyCode  = " + schChoiceAdModel.SearchAgencyCode.Trim() + " \n");
                //}	

                //// 광고주를 선택했으면
                //if(schChoiceAdModel.SearchAdvertiserCode.Trim().Length > 0 && !schChoiceAdModel.SearchAdvertiserCode.Trim().Equals("00"))
                //{
                //    sbQuery.Append(" AND B.AdvertiserCode  = " + schChoiceAdModel.SearchAdvertiserCode.Trim() + " \n");
                //}	


                // 광고상태 선택에 따라
                // 광고상태는 20:편성 과 40:종료 사이에 있는 것만 조회한다.
                sbQuery.Append(" AND B.AdState >= '20' AND B.AdState <= '40' \n");

                // 광고상태 편성
                if(schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( B.AdState  = '20' \n");
                    isState = true;
                }	
                // 광고상태 중지
                if(schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '30' \n");
                    isState = true;
                }	
                // 광고상태 종료
                if(schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '40' \n");
                    isState = true;
                }	

                if(isState) sbQuery.Append(" ) \n");
				
                // 검색어가 있으면
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND B.ItemName  LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%'" );
                }
       
                sbQuery.Append("  ORDER BY A.ItemNo DESC");
                */
                #endregion
                sbQuery.Append("\n  SELECT   a.item_no as ItemNo            ");
                sbQuery.Append("\n          ,b.item_nm as ItemName          ");
                sbQuery.Append("\n          ,b.rl_end_dy as RealEndDay      ");
                sbQuery.Append("\n          ,b.advt_stt as AdState          ");
                sbQuery.Append("\n          ,g.stm_cod_nm as AdStatename    ");
                sbQuery.Append("\n          ,b.advt_rate as AdRate          ");
                sbQuery.Append("\n          ,( select count(distinct Genre) ");
                sbQuery.Append("\n          from (                          ");
                sbQuery.Append("\n              select  menu_cod as Genre, x.item_no    ");
                sbQuery.Append("\n              from	SCHD_MENU x                     ");
                sbQuery.Append("\n              union all                               ");
                sbQuery.Append("\n              select  y.title_no as Genre, y.item_no  ");
                sbQuery.Append("\n              from	  SCHD_TITLE y                  ");
                sbQuery.Append("\n          ) v                             ");
                sbQuery.Append("\n          where v.item_no = a.item_no     ");
                sbQuery.Append("\n          ) as MenuCount                  ");
                sbQuery.Append("\n          ,( select count(*) from SCHD_TITLE   WHERE item_no = a.item_no)	as ChannelCount   ");
                sbQuery.Append("\n          ,( select count(*) from SCHDRT_MST WHERE item_no = a.item_no )	as GroupCount   ");
                sbQuery.Append("\n          ,( select NVL(SUM(entry_rt),0) from SCHDRT_MST WHERE item_no = a.item_no )	as GroupSum ");
                sbQuery.Append("\n          ,( select count(*) from SCHDRT_DTL WHERE item_no = a.item_no )	as DetailCount ");
                sbQuery.Append("\n          ,b.advt_typ as AdType           ");
                sbQuery.Append("\n          ,c.stm_cod_nm as AdTypeName     ");
                sbQuery.Append("\n  FROM                                    ");
                sbQuery.Append("\n  (	SELECT item_no FROM SCHD_MENU       ");
                sbQuery.Append("\n      UNION                               ");
                sbQuery.Append("\n      SELECT item_no FROM SCHD_TITLE      ");
                sbQuery.Append("\n  )   a                                   ");
                sbQuery.Append("\n  INNER JOIN ADVT_MST       b  ON (a.item_no  = b.item_no)    ");
                sbQuery.Append("\n  INNER JOIN CNTR           k ON (b.cntr_seq = k.cntr_seq)    ");
                sbQuery.Append("\n  LEFT  JOIN STM_COD          g  ON (b.advt_stt   = g.stm_cod AND g.stm_cod_cls = '25')   ");
                sbQuery.Append("\n  LEFT OUTER JOIN STM_COD c  ON (b.advt_typ  = c.stm_cod AND c.stm_cod_cls = '26')    ");
                sbQuery.Append("\n  WHERE b.advt_typ IN('10' ,'19')         ");

                // 매체을 선택했으면
                if (schChoiceAdModel.SearchMediaCode.Trim().Length > 0 && !schChoiceAdModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND k.mda_cod  = " + schChoiceAdModel.SearchMediaCode.Trim() + " \n");
                }

                // 랩사를 선택했으면
                if (schChoiceAdModel.SearchRapCode.Trim().Length > 0 && !schChoiceAdModel.SearchRapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND k.rep_cod  = " + schChoiceAdModel.SearchRapCode.Trim() + " \n");
                }


                // 광고상태 선택에 따라
                // 광고상태는 20:편성 과 40:종료 사이에 있는 것만 조회한다.
                sbQuery.Append(" AND B.advt_stt >= '20' AND B.advt_stt <= '40' \n");

                // 광고상태 편성
                if (schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( B.advt_stt  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.advt_stt  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.advt_stt  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");

                // 검색어가 있으면
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND B.item_nm  LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%'");
                }

                sbQuery.Append("  ORDER BY b.item_nm DESC");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // 결과
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // 결과코드 셋트
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdList10() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "광고목록 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }

        }
        #endregion

        #region 엑셀에서 파일명과 위치를 검증하기 위한 함수

        /// <summary>
        /// 지정광고 편성대상조회
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetInspectItemList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetInspectItemList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey            :[" + schChoiceAdModel.SearchKey + "]");		// 검색어
                _log.Debug("SearchMediaCode	     :[" + schChoiceAdModel.SearchMediaCode + "]");		// 검색 매체

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + "SELECT A.ItemNo                      \n"
                    + "      ,A.ItemName                    \n"
                    + "      ,B.ContractName                \n"
                    + "      ,C.AdvertiserName              \n"
                    + "      ,A.ExcuteStartDay              \n"
                    + "      ,A.ExcuteEndDay                \n"
                    + "      ,A.RealEndDay                  \n"
                    + "      ,A.AdState                     \n"
                    + "      ,D.CodeName AdStateName        \n"
                    + "      ,(SELECT COUNT(*) FROM SchHome with(NoLock)                WHERE ItemNo = A.ItemNo) AS HomeCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail with(NoLock)    WHERE ItemNo = A.ItemNo) AS MenuCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock) WHERE ItemNo = A.ItemNo) AS ChannelCount   \n"
                    + "      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay                                                \n"
                    + "      ,A.AdType                      \n"
                    + "      ,E.CodeName AS AdTypeName      \n"
                    + "      ,A.FilePath      \n"
                    + "      ,A.FileName      \n"
                    + "  FROM ContractItem A INNER JOIN Contract   B with(NoLock) ON (B.MediaCode      = B.MediaCode AND A.RapCode = B.RapCode AND A.AgencyCode = B.AgencyCode AND A.AdvertiserCode = B.AdvertiserCode AND A.ContractSeq = B.ContractSeq) \n"
                    + "                       LEFT JOIN Advertiser C with(NoLock) ON (B.AdvertiserCode = C.AdvertiserCode)                \n"
                    + "	                      LEFT JOIN SystemCode D with(NoLock) ON (A.AdState        = D.Code      AND D.Section = '25')  \n"  // 25 : 광고상태
                    + "                       LEFT JOIN SystemCode E with(NoLock) ON (A.AdType         = E.Code      AND E.Section = '26')  \n"	// 26 : 광고종류
                    + " WHERE A.AdClass IN ('10','30')   \n"    // 광고용도 AdClass 10:홈광고 30:복합

                    );

                // 매체을 선택했으면
                if (schChoiceAdModel.SearchMediaCode.Trim().Length > 0 && !schChoiceAdModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND A.MediaCode  = '" + schChoiceAdModel.SearchMediaCode.Trim() + "' \n");
                }

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if (schChoiceAdModel.SearchchkAdState_10.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.AdState  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");



                // 검색어가 있으면
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( A.ItemName       LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.ContractName   LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY A.ItemNo DESC ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // 결과
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // 결과코드 셋트
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetInspectItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "지정광고 편성대상조회 중 오류가 발생하였습니다";
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
        /// <param name="schChoiceAdModel"></param>
        public void GetContractItemList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
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
                _log.Debug("SearchKey            :[" + schChoiceAdModel.SearchKey + "]");	// 검색어
                _log.Debug("SearchMediaCode	     :[" + schChoiceAdModel.SearchMediaCode + "]");		// 검색 매체
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
                    + "      ,(SELECT COUNT(*) FROM SchHome with(NoLock)                WHERE ItemNo = A.ItemNo) AS HomeCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail with(NoLock)    WHERE ItemNo = A.ItemNo) AS MenuCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock) WHERE ItemNo = A.ItemNo) AS ChannelCount   \n"
                    + "      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay                                                \n"
                    + "      ,A.AdType                      \n"
                    + "      ,E.CodeName AS AdTypeName      \n"
                    + "  FROM ContractItem A INNER JOIN Contract   B with(NoLock) ON (B.MediaCode      = B.MediaCode AND A.RapCode = B.RapCode AND A.AgencyCode = B.AgencyCode AND A.AdvertiserCode = B.AdvertiserCode AND A.ContractSeq = B.ContractSeq) \n"
                    + "                       LEFT JOIN Advertiser C with(NoLock) ON (B.AdvertiserCode = C.AdvertiserCode)                \n"
                    + "	                      LEFT JOIN SystemCode D with(NoLock) ON (A.AdState        = D.Code      AND D.Section = '25')  \n"  // 25 : 광고상태
                    + "                       LEFT JOIN SystemCode E with(NoLock) ON (A.AdType         = E.Code      AND E.Section = '26')  \n"	// 26 : 광고종류
                    + " WHERE 1=1   \n"
                    );

                // 매체을 선택했으면
                if (schChoiceAdModel.SearchMediaCode.Trim().Length > 0 && !schChoiceAdModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND A.MediaCode  = '" + schChoiceAdModel.SearchMediaCode.Trim() + "' \n");
                }

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if (schChoiceAdModel.SearchchkAdState_10.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.AdState  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");



                // 검색어가 있으면
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( A.ItemName       LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.ContractName   LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY A.ItemNo DESC ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // 결과
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // 결과코드 셋트
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "지정광고 편성대상조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        /// <summary>
        /// 광고기준편성, 편성단위기준 편성 업무에서 광고목록을 찾아온다 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void GetContractItemList_0907a(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
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
                _log.Debug("SearchKey            :[" + schChoiceAdModel.SearchKey + "]");	// 검색어
                _log.Debug("SearchMediaCode	     :[" + schChoiceAdModel.SearchMediaCode + "]");		// 검색 매체
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
                    + "      ,(SELECT COUNT(*) FROM SchHome with(NoLock)                WHERE ItemNo = A.ItemNo) AS HomeCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail with(NoLock)    WHERE ItemNo = A.ItemNo) AS MenuCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock) WHERE ItemNo = A.ItemNo) AS ChannelCount   \n"
                    + "      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay                                                \n"
                    + "      ,A.AdType                      \n"
                    + "      ,E.CodeName AS AdTypeName      \n"
                    + "  FROM ContractItem A with(noLock) INNER JOIN Contract   B with(NoLock) ON (B.MediaCode      = B.MediaCode AND A.RapCode = B.RapCode AND A.AgencyCode = B.AgencyCode AND A.AdvertiserCode = B.AdvertiserCode AND A.ContractSeq = B.ContractSeq) \n"
                    + "                       LEFT JOIN Advertiser C with(NoLock) ON (B.AdvertiserCode = C.AdvertiserCode)                \n"
                    + "	                      LEFT JOIN SystemCode D with(NoLock) ON (A.AdState        = D.Code      AND D.Section = '25') \n"  // 25 : 광고상태
                    + "                       LEFT JOIN SystemCode E with(NoLock) ON (A.AdType         = E.Code      AND E.Section = '26') \n"  // 26 : 광고종류
                    + "  WHERE 1=1 \n");
                //+ " WHERE ItemNo not in( select distinct ItemNo   \n"   
                //+ "                      from ( select ItemNo from SchChoiceMenu noLock \n"   
                //+ "                             union all \n"   
                //+ "                             select ItemNo from SchChoiceChannel noLock ) a ) \n");
                //+ " AND AdType In(10,11,12,16,20) \n" );
                if (schChoiceAdModel.SearchAdType.Length > 0 && schChoiceAdModel.SearchAdType.Trim().Equals("000"))
                {
                    sbQuery.Append(" AND a.AdType In(10,11,12,16,17,19,20) \n");
                }
                else if (schChoiceAdModel.SearchAdType.Length > 0 && schChoiceAdModel.SearchAdType.Trim().Equals("100"))
                {
                    // 상업광고류
                    sbQuery.Append(" AND a.AdType In(10,16,17,19) \n");
                }
                else if (schChoiceAdModel.SearchAdType.Length > 0 && schChoiceAdModel.SearchAdType.Trim().Equals("200"))
                {
                    // 매체광고류
                    sbQuery.Append(" AND a.AdType In(11,12,20) \n");
                }
                else
                {
                    sbQuery.Append(" AND a.AdType = " + schChoiceAdModel.SearchAdType.Trim() + " \n");
                }

                // 매체을 선택했으면
                if (schChoiceAdModel.SearchMediaCode.Trim().Length > 0 && !schChoiceAdModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND A.MediaCode  = '" + schChoiceAdModel.SearchMediaCode.Trim() + "' \n");
                }

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if (schChoiceAdModel.SearchchkAdState_10.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.AdState  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");



                // 검색어가 있으면
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( A.ItemName       LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.ContractName   LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY A.ItemNo DESC ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // 결과
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // 결과코드 셋트
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "지정광고 편성대상조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }
        #endregion


        #region 지정채널광고 편성

        /// <summary>
        /// 지정채널광고 편성
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() Start");
                _log.Debug("-----------------------------------------");


                // 쿼리실행
                try
                {
                    int i = 0;
                    int rc = 0;
                    int adSchType = 0;

                    if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                    {
                        adSchType = 10;
                    }
                    else
                        adSchType = 20;

                    // 현재 승인번호를 구함
                    string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                    StringBuilder sbQuery = new StringBuilder();
                    SqlParameter[] sqlParams = new SqlParameter[1];

                    // 파라메터 셋트
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                    // 기편성이 되어있는지 검사
                    sbQuery.Append("\n"
                        + "SELECT * FROM SchChoiceChannel  \n"
                        + " WHERE ItemNo =  @ItemNo         \n"
                        );

                    // 쿼리실행
                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    int cnt = Utility.GetDatasetCount(ds);

                    ds.Dispose();

                    // 결과
                    // 0건이면 미편성된 광고
                    if (cnt == 0)
                    {
                        _db.BeginTran();

                        // 지정채널 편성 테이블에 추가
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceChannel(  \n"
                            + "                ItemNo         \n"
                            + "      ) VALUES (               \n"
                            + "                @ItemNo        \n"
                            + "      )                        \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        // 광고상태 편성으로 변경
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "UPDATE ContractItem        \n"
                            + "   SET AdState = '20'      \n"   // 광고상태 - 20:편성
                            + "      ,ModDt   = GETDATE() \n"
                            + "      ,RegID   = '" + header.UserID + "' \n"
                            + " WHERE ItemNo  = @ItemNo   \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        _db.CommitTran();


                        // __MESSAGE__
                        _log.Message("신규편성 지정채널광고:[" + schChoiceAdModel.ItemName + "] 등록자:[" + header.UserID + "]");
                    }
                    else
                    {
                        _log.Message("기편성 지정채널광고:[" + schChoiceAdModel.ItemName + "]");
                    }

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "지정채널광고 저장 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }
        #endregion

        #region 지정채널광고 삭제
        /// <summary>
        /// 지정채널광고  삭제
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete() Start");
                _log.Debug("-----------------------------------------");


                // 쿼리실행
                try
                {
                    int i = 0;
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();
                    SqlParameter[] sqlParams = new SqlParameter[2];

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);
                    sqlParams[i++].Value = Convert.ToInt16(schChoiceAdModel.MediaCode);

                    // 지정채널광고 편성 상세 테이블에서 해당 광고가 있는 채널을 조회한다.
                    sbQuery.Append("\n"
                        + "SELECT ChannelNo                            \n"
                        + "  FROM SchChoiceChannelDetail with(nolock) \n"
                        + " WHERE ItemNo    = @ItemNo       \n"
                        + "   AND MediaCode = @MediaCode    \n"
                        + " ORDER BY ChannelNo           \n"
                        );

                    DataSet ds = _db.ExecuteQueryParams(sbQuery.ToString(), sqlParams);

                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        DataRow row = ds.Tables[0].Rows[j];
                        string ChannelNo = row["ChannelNo"].ToString();

                        schChoiceAdModel.ChannelNo = ChannelNo;

                        // 각 채널별로 상세내역을 삭제한다.
                        SetSchChoiceChannelDetailDelete2(header, schChoiceAdModel);
                    }
                    ds.Dispose();


                    _db.BeginTran();

                    // 지정메뉴광고 편성 테이블에서 삭제
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceChannel          \n"
                        + " WHERE ItemNo        = " + schChoiceAdModel.ItemNo + " \n"
                        );

                    _log.Debug(sbQuery.ToString());

                    rc = _db.ExecuteNonQuery(sbQuery.ToString());

                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("지정채널광고 삭제:[" + schChoiceAdModel.ItemName + "] 등록자:[" + header.UserID + "]" + rc.ToString());

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "지정채널광고 편성내역 삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion

        #region 지정채널광고 상세편성 조회

        /// <summary>
        /// 지정채널광고 상세편성 조회
        /// </summary>
        /// <returns></returns>
        public void GetSchChoiceChannelDetailList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceChannelDetailList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                OracleParameter[] sqlParams = new OracleParameter[1];

                sqlParams[0] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                sqlParams[0].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // 지정채널 편성 테이블에 추가
                sbQuery.Append("\n SELECT 'False'		AS CheckYn");
                sbQuery.Append("\n     ,   A.TITLE_NO  AS ChannelNo");
                sbQuery.Append("\n     ,   T.TITLE_NM  AS Title");
                sbQuery.Append("\n     ,   C.ACK_STT   AS AckState");
                sbQuery.Append("\n     ,   0           AS Hits         -- 사용하지 않음");
                sbQuery.Append("\n     ,   ''          AS ProdType     -- 차후에, 컨텐츠하고 조인해야 함");
                sbQuery.Append("\n FROM    SCHD_TITLE A");
                sbQuery.Append("\n     INNER JOIN ADVT_MST         B ON B.ITEM_NO  = A.ITEM_NO");
                sbQuery.Append("\n     INNER JOIN TITLE_COD        T ON T.TITLE_NO = A.TITLE_NO");
                sbQuery.Append("\n     LEFT  JOIN SCHD_DIST_MST    C ON C.ACK_NO   = A.ACK_NO");
                sbQuery.Append("\n WHERE   A.ITEM_NO = :ItemNo");
                sbQuery.Append("\n ORDER BY T.TITLE_NM");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                // 결과 DataSet의 모델에 복사
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                schChoiceAdModel.ResultCD = "0000";  // 정상

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceChannelDetailList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "타이틀 상세편성 조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion

        #region 지정채널광고 상세편성 저장
        /// <summary>
        /// 지정채널광고 상세편성 저장
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            int err = 0;

            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MediaCode         :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("ChannelNo         :[" + schChoiceAdModel.ChannelNo + "]");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("AdType            :[" + schChoiceAdModel.AdType + "]");
                // __DEBUG__				

                int i = 0;
                int rc = 0;
                int adSchType = 0;

                if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                {
                    adSchType = 10;
                }
                else
                    adSchType = 20;

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                StringBuilder sbQuery = new StringBuilder();

                // 해당 채널이 존재하는지 검사
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT *           \n"
                    + "  FROM Channel with(NoLock)     \n"
                    + " WHERE MediaCode  = " + schChoiceAdModel.MediaCode + " \n"
                    + "   AND ChannelNo  = " + schChoiceAdModel.ChannelNo + " \n"
                    );

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                rc = Utility.GetDatasetCount(ds);

                // 결과
                // 0건이면 채널이 존재하지 않는것이다.
                if (rc == 0)
                {
                    ds.Dispose();
                    err = 1;	// 채널이 존재하지 않음
                    throw new Exception();
                }
                ds.Dispose();


                string AdType = schChoiceAdModel.AdType;	// 광고종류 : 10:CM 11:EAP(10~19는 필수광고) 20:OAP 

                #region [삭제: 유료채널 편성불가]
                // 2007.09.04 상업광고 편성 불가 컨텐츠 처리
                // 2010,01.01 유료채널 편성 여부 옵션으로 처리함
                // 해당 채널의 컨텐츠를 검사하여 상업광고(필수광고) 편성여부를 결정한다.
                // if(AdType.StartsWith("1")) // 10~19사이가 필수광고이다. 코드가 1로 들어오면 어떻하지? ㅠ.ㅠ
                // 상업광고만 10번
                //                if(AdType.Equals("10")) // 10~19사이가 필수광고이다. 코드가 1로 들어오면 어떻하지? ㅠ.ㅠ
                //				{
                //					// 해당 채널이 존재하는지 검사
                //					sbQuery   = new StringBuilder();
                //					sbQuery.Append( "\n"
                //						+ "SELECT *   \n"
                //						+ "  FROM Channel A with(NoLock) INNER JOIN Contents B with(NoLock) ON (A.ContentID = B.ContentID) \n"
                //						+ " WHERE A.MediaCode  = " + schChoiceAdModel.MediaCode + " \n"
                //						+ "   AND A.ChannelNo  = " + schChoiceAdModel.ChannelNo + " \n"
                //						+ "   AND ProdType IS NOT NULL     \n"
                //						+ "   AND ProdType <> '' \n"
                //						);
                //
                //					// 쿼리실행
                //					ds = new DataSet();
                //					_db.ExecuteQuery(ds,sbQuery.ToString());
                //				
                //					rc = Utility.GetDatasetCount(ds);
                //				
                //					// 결과
                //					// 1건이라도 있다면 채널에 필수광고를 편성할 수 없다.
                //					if(rc > 0)
                //					{							
                //						err = 2;
                //						ds.Dispose();
                //						throw new Exception();
                //					}
                //					ds.Dispose();
                //				}
                #endregion

                SqlParameter[] sqlParams = new SqlParameter[1];

                //지정채널편성테이블에 데이터가 없을 경우 Insert
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT ItemNo FROM SchChoiceChannel with(NoLock)  \n"
                    + " WHERE ItemNo    =  @ItemNo           \n"
                    );

                i = 0;
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                rc = Utility.GetDatasetCount(ds);
                ds.Dispose();

                // 결과
                // 0건이면 지정채널편성테이에 ItemNo Insert
                if (rc == 0)
                {
                    // 쿼리실행
                    try
                    {
                        _db.BeginTran();


                        // 지정채널 편성 테이블에 추가
                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceChannel ( \n"
                            + "            ItemNo            \n"
                            + "       )                      \n"
                            + "       VALUES (               \n"
                            + "           @ItemNo            \n"
                            + "       )                      \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        // 광고상태 편성으로 변경
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "UPDATE ContractItem        \n"
                            + "   SET AdState = '20'      \n"   // 광고상태 - 20:편성
                            + "      ,ModDt   = GETDATE() \n"
                            + "      ,RegID   = '" + header.UserID + "' \n"
                            + " WHERE ItemNo  = @ItemNo   \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);


                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("신규편성 지정채널광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] 등록자:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }


                sqlParams = new SqlParameter[3];

                i = 0;
                sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@ChannelNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ChannelNo);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // 기편성이 되어있는지 검사
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT * FROM SchChoiceChannelDetail  \n"
                    + " WHERE MediaCode =  @MediaCode        \n"
                    + "   AND ChannelNo =  @ChannelNo        \n"
                    + "   AND ItemNo    =  @ItemNo           \n"
                    );

                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                rc = Utility.GetDatasetCount(ds);

                ds.Dispose();

                // 결과
                // 0건이면 미편성된 광고
                if (rc == 0)
                {
                    // 쿼리실행
                    try
                    {
                        _db.BeginTran();

                        // 지정채널 편성 테이블에 추가
                        sbQuery = new StringBuilder();


                        if (AdType.StartsWith("1")) // 10~19사이가 필수광고이다. 코드가 1로 들어오면 어떻하지? ㅠ.ㅠ
                        {
                            sbQuery.Append("\n"
                                + "INSERT INTO SchChoiceChannelDetail (      \n"
                                + "            MediaCode                     \n"
                                + "           ,ChannelNo                     \n"
                                + "           ,ItemNo                        \n"
                                + "           ,AckNo                         \n"
                                + "           ,ScheduleOrder                 \n"
                                + "       )                                  \n"
                                + "       VALUES (                           \n"
                                + "           @MediaCode                     \n"
                                + "          ,@ChannelNo                     \n"
                                + "          ,@ItemNo                        \n"
                                + "          ," + AckNo + "  \n"
                                + "          ,0                              \n" // 필수광고의 순서는 0
                                + "       )                                  \n"
                                );
                        }
                        else
                        {
                            sbQuery.Append("\n"
                                + "INSERT INTO SchChoiceChannelDetail (      \n"
                                + "            MediaCode                     \n"
                                + "           ,ChannelNo                     \n"
                                + "           ,ItemNo                        \n"
                                + "           ,AckNo                         \n"
                                + "           ,ScheduleOrder         \n"
                                + "       )                                  \n"
                                + "       SELECT                             \n"
                                + "           @MediaCode                     \n"
                                + "          ,@ChannelNo                     \n"
                                + "          ,@ItemNo                        \n"
                                + "          ," + AckNo + "  \n"
                                + "          ,ISNULL(MAX(ScheduleOrder),0)+1 \n" // 옵션광고(OAP등)의 순서는 MAX+1
                                + "      FROM SchChoiceChannelDetail         \n"
                                + "      WHERE MediaCode = @MediaCode        \n"
                                + "        AND Channelno = @ChannelNo        \n"
                                );
                        }

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("신규편성 지정채널광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] 등록자:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    // __MESSAGE__
                    _log.Message("기편성 지정채널광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] 등록자:[" + header.UserID + "]");
                }


                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                if (err == 1)
                {
                    schChoiceAdModel.ResultDesc = "입력하신 채널은 존재하지 않는 채널번호입니다.";
                }
                else if (err == 2)
                {
                    schChoiceAdModel.ResultDesc = "해당 채널은 유료컨텐츠이므로 상업광고 편성을 불허합니다.";
                }
                else
                {
                    schChoiceAdModel.ResultDesc = "지정채널광고 상세편성 저장 중 오류가 발생하였습니다";
                    _log.Exception(ex);
                }
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion

        #region 지정채널광고 상세편성 삭제
        /// <summary>
        /// 지정채널광고 상세편성 삭제
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                SetSchChoiceChannelDetailDelete2(header, schChoiceAdModel);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        public void SetSchChoiceChannelDetailDelete2(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete2() Start");
                _log.Debug("-----------------------------------------");


                // 쿼리실행
                try
                {
                    int i = 0;
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();
                    SqlParameter[] sqlParams = new SqlParameter[3];

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                    sqlParams[i++] = new SqlParameter("@ChannelNo", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ChannelNo);
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                    int adSchType = 0;

                    if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                    {
                        adSchType = 10;
                    }
                    else
                        adSchType = 20;

                    // 현재 승인번호를 구함
                    string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                    // 삭제할 순서
                    string DeleteOrder = "0";

                    // 해당 삭제할 항목의 순서 구함
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + " SELECT ScheduleOrder           \n"
                        + "  FROM SchChoiceChannelDetail   \n"
                        + " WHERE MediaCode = @MediaCode   \n"
                        + "   AND ChannelNo = @ChannelNo   \n"
                        + "   AND ItemNo    = @ItemNo      \n"
                        );

                    // 쿼리실행
                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    if (Utility.GetDatasetCount(ds) != 0)
                    {
                        DeleteOrder = Utility.GetDatasetString(ds, 0, "ScheduleOrder");
                    }

                    ds.Dispose();

                    _db.BeginTran();

                    // 지정채널 편성 테이블에 삭제
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceChannelDetail   \n"
                        + " WHERE MediaCode = @MediaCode   \n"
                        + "   AND ChannelNo = @ChannelNo   \n"
                        + "   AND ItemNo    = @ItemNo      \n"
                        );

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    sbQuery.Length = 0;
                    // 지정채널 편성 테이블에서 삭제되어 더 이상 존재하지 않으면 삭제를 한다.
                    sbQuery.Append("WITH CA (CategoryCode, GenreCode)	\n");
                    sbQuery.Append("AS (	\n");
                    sbQuery.Append("	SELECT	\n");
                    sbQuery.Append("		B.CategoryCode, A.GenreCode--CategoryCode, GenreCode	\n");
                    sbQuery.Append("	FROM SchChoiceMenuDetail A	\n");
                    sbQuery.Append("	JOIN Category B ON B.MediaCode = A.MediaCode	\n");
                    sbQuery.Append("		AND B.CategoryCode = (SELECT TOP 1 CategoryCode FROM ChannelSet WHERE GenreCode = A.GenreCode)	\n");
                    sbQuery.Append("	WHERE ItemNo = @ItemNo	\n");
                    sbQuery.Append("	UNION	\n");
                    sbQuery.Append("	SELECT	\n");
                    sbQuery.Append("		B.CategoryCode, B.GenreCode	\n");
                    sbQuery.Append("	FROM SchChoiceChannelDetail A	\n");
                    sbQuery.Append("	JOIN ChannelSet B WITH (NOLOCK) ON B.ChannelNo = A.ChannelNo	\n");
                    sbQuery.Append("	JOIN Category C WITH (NOLOCK) ON C.MediaCode = A.MediaCode AND C.CategoryCode = B.CategoryCode	\n");
                    sbQuery.Append("	WHERE ItemNo = @ItemNo	\n");
                    sbQuery.Append("	AND B.SeriesNo = (SELECT MIN(SeriesNo) FROM ChannelSet WHERE MediaCode = B.MediaCode AND ChannelNo = B.ChannelNo)	\n");
                    sbQuery.Append(")	\n");
                    sbQuery.Append("DELETE SchRateDetail	\n");
                    sbQuery.Append("--SELECT *	\n");
                    sbQuery.Append("FROM SchRateDetail A	\n");
                    sbQuery.Append("WHERE	\n");
                    sbQuery.Append("	NOT EXISTS (	\n");
                    sbQuery.Append("		SELECT 1 FROM CA	\n");
                    sbQuery.Append("		WHERE MediaCode = A.MediaCode AND ItemNo = A.ItemNo	\n");
                    sbQuery.Append("			AND CategoryCode = A.CategoryCode AND GenreCode = A.GenreCode)	\n");
                    sbQuery.Append("	AND MediaCode = @MediaCode	\n");
                    sbQuery.Append("	AND ItemNo = @ItemNo	\n");
                    //					sbQuery.Append("DELETE SchRate	\n");
                    //					sbQuery.Append("--SELECT *	\n");
                    //					sbQuery.Append("FROM SchRate A	\n");
                    //					sbQuery.Append("WHERE	\n");
                    //					sbQuery.Append("	NOT EXISTS (	\n");
                    //					sbQuery.Append("		SELECT 1 FROM SchRateDetail	\n");
                    //					sbQuery.Append("		WHERE MediaCode = A.MediaCode AND ItemNo = A.ItemNo	\n");
                    //					sbQuery.Append("			AND EntrySeq = A.EntrySeq)	\n");
                    //					sbQuery.Append("	AND MediaCode = @MediaCode	\n");
                    //					sbQuery.Append("	AND ItemNo = @ItemNo	\n");
                    _log.Debug(sbQuery.ToString());

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    // 순위가 0이 아닌 것에 대하여
                    if (!DeleteOrder.Equals("0"))
                    {
                        // 해당 순위보다 큰 순위의 내역들을 -1하여 조정
                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n"
                            + "UPDATE SchChoiceChannelDetail                              \n"
                            + "   SET ScheduleOrder = ScheduleOrder - 1                   \n"
                            + "      ,AckNo         = " + AckNo + " \n"
                            + " WHERE MediaCode     = " + schChoiceAdModel.MediaCode + " \n"
                            + "   AND ChannelNo     = " + schChoiceAdModel.ChannelNo + " \n"
                            + "   AND ScheduleOrder > " + DeleteOrder + " \n"
                            + "   AND ScheduleOrder > 0                                       \n"
                            );

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());
                    }
                    _db.CommitTran();

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete2() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "지정채널광고 상세편성 삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
        }

        #endregion

        #region 지정채널광고 엑셀삭제

        /// <summary>
        /// 지정채널광고 엑셀삭제
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelDelete_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete_To() Start");
                _log.Debug("-----------------------------------------");


                // 쿼리실행
                try
                {
                    int i = 0;
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();
                    SqlParameter[] sqlParams = new SqlParameter[1];

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);

                    _db.BeginTran();

                    // 지정채널광고 편성 상세 테이블에서 삭제
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceChannelDetail            \n"
                        + " WHERE MediaCode        = @MediaCode        \n"
                        );

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    // 지정채널광고 편성 테이블에서 삭제
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceChannel                  \n"
                        + " WHERE ItemNo  IN (SELECT ItemNo FROM ContractItem WHERE MediaCode = @MediaCode)         \n"
                        );

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("지정메뉴광고 삭제:[" + schChoiceAdModel.MediaCode + "] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete_To() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "지정채널광고 편성내역 삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion

        #region [S1]편성단위별 회차 상세편성 저장
        /// <summary>
        /// 편성단위별 회차 상세편성 저장
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceSeriesDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            int err = 0;

            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("MediaCode   :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("ChannelNo   :[" + schChoiceAdModel.ChannelNo + "]");
                _log.Debug("ItemNo      :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("AdType      :[" + schChoiceAdModel.AdType + "]");

                #region 편성승인번호 구하기
                int rc = 0;
                int adSchType = 0;

                if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                {
                    adSchType = 10;
                }
                else
                    adSchType = 20;

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                StringBuilder sbQuery = new StringBuilder();
                DataSet ds = new DataSet();

                string AdType = schChoiceAdModel.AdType;	// 광고종류 : 10:CM 11:EAP(10~19는 필수광고) 20:OAP 
                #endregion

                #region 필수콘텐츠 확인
                // 2007.09.04 상업광고 편성 불가 컨텐츠 처리
                // 해당 채널의 컨텐츠를 검사하여 상업광고(필수광고) 편성여부를 결정한다.
                // if(AdType.StartsWith("1")) // 10~19사이가 필수광고이다. 코드가 1로 들어오면 어떻하지? ㅠ.ㅠ
                // 상업광고만 10번
                if (AdType.Equals("10")) // 10~19사이가 필수광고이다. 코드가 1로 들어오면 어떻하지? ㅠ.ㅠ
                {
                    // 해당 채널이 존재하는지 검사
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT *    ");
                    sbQuery.Append("\n FROM Channel A with(NoLock)  ");
                    sbQuery.Append("\n INNER JOIN Contents B with(NoLock) ON A.ContentID = B.ContentID  ");
                    sbQuery.Append("\n WHERE A.MediaCode  = " + schChoiceAdModel.MediaCode);
                    sbQuery.Append("\n AND	 A.ChannelNo  = " + schChoiceAdModel.ChannelNo);
                    sbQuery.Append("\n AND	 A.SeriesNo	  = " + schChoiceAdModel.SeriesNo);
                    sbQuery.Append("\n AND   B.ProdType IS NOT NULL ");
                    sbQuery.Append("\n AND   B.ProdType <> '' ");
                    ds = new DataSet();
                    _db.ExecuteQuery(ds, sbQuery.ToString());
                    rc = Utility.GetDatasetCount(ds);

                    // 결과
                    // 1건이라도 있다면 채널에 필수광고를 편성할 수 없다.
                    if (rc > 0)
                    {
                        err = 2;
                        ds.Dispose();
                        throw new Exception();
                    }
                    ds.Dispose();
                }
                #endregion

                #region 기편성여부 확인
                SqlParameter[] sqlParams = new SqlParameter[5];
                sqlParams[0] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[1] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[2] = new SqlParameter("@ChannelNo", SqlDbType.Int);
                sqlParams[3] = new SqlParameter("@SeriesNo", SqlDbType.Int);
                sqlParams[4] = new SqlParameter("@AckNo", SqlDbType.Int);

                sqlParams[0].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);
                sqlParams[1].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);
                sqlParams[2].Value = Convert.ToInt32(schChoiceAdModel.ChannelNo);
                sqlParams[3].Value = Convert.ToInt32(schChoiceAdModel.SeriesNo);
                sqlParams[4].Value = AckNo;

                // 기편성이 되어있는지 검사
                sbQuery = new StringBuilder();
                sbQuery.Append("\n SELECT * FROM SchChoiceSeriesDetail with(noLock) ");
                sbQuery.Append("\n WHERE	ItemNo		= @ItemNo ");
                sbQuery.Append("\n AND		MediaCode	=  @MediaCode ");
                sbQuery.Append("\n AND		ChannelNo	=  @ChannelNo ");
                sbQuery.Append("\n AND		SeriesNo	= @SeriesNo; ");

                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);
                rc = Utility.GetDatasetCount(ds);
                ds.Dispose();
                #endregion

                #region 편성 추가
                // 0건이면 미편성된 광고
                if (rc == 0)
                {
                    // 쿼리실행
                    try
                    {
                        _db.BeginTran();

                        // 지정채널 편성 테이블에 추가
                        sbQuery = new StringBuilder();
                        if (AdType.StartsWith("1")) // 10~19사이가 필수광고이다. 코드가 1로 들어오면 어떻하지? ㅠ.ㅠ
                        {
                            sbQuery.Append("\n"
                                + "INSERT INTO SchChoiceSeriesDetail (	\n"
                                + "            MediaCode                \n"
                                + "           ,ChannelNo                \n"
                                + "           ,SeriesNo                 \n"
                                + "           ,ItemNo                   \n"
                                + "           ,AckNo                    \n"
                                + "           ,ScheduleOrder            \n"
                                + "       )                             \n"
                                + " VALUES (  @MediaCode,@ChannelNo,@SeriesNo,@ItemNo,@AckNo,0 )");
                        }
                        else
                        {
                            sbQuery.Append("\n INSERT INTO SchChoiceSeriesDetail ");
                            sbQuery.Append("\n				(    MediaCode ");
                            sbQuery.Append("\n					,ChannelNo ");
                            sbQuery.Append("\n					,SeriesNo ");
                            sbQuery.Append("\n					,ItemNo ");
                            sbQuery.Append("\n					,AckNo ");
                            sbQuery.Append("\n					,ScheduleOrder ) ");
                            sbQuery.Append("\n SELECT	@MediaCode   ");
                            sbQuery.Append("\n 		,@ChannelNo  ");
                            sbQuery.Append("\n 		,@SeriesNo	 ");
                            sbQuery.Append("\n         ,@ItemNo     ");
                            sbQuery.Append("\n         ,@AckNo		 ");
                            sbQuery.Append("\n         ,ISNULL(MAX(ScheduleOrder),0)+1  ");
                            sbQuery.Append("\n FROM SchChoiceSeriesDetail noLock ");
                            sbQuery.Append("\n WHERE	ItemNo		= @ItemNo ");
                            sbQuery.Append("\n AND		MediaCode	=  @MediaCode ");
                            sbQuery.Append("\n AND		ChannelNo	=  @ChannelNo ");
                            sbQuery.Append("\n AND		SeriesNo	= @SeriesNo; ");
                        }

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("신규편성 지정채널광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] 등록자:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    // __MESSAGE__
                    _log.Message("기편성 지정채널광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] 등록자:[" + header.UserID + "]");
                }
                #endregion


                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                if (err == 1)
                {
                    schChoiceAdModel.ResultDesc = "입력하신 회차는 존재하지 않는 회차번호입니다.";
                }
                else if (err == 2)
                {
                    schChoiceAdModel.ResultDesc = "해당 회차는 유료컨텐츠이므로 상업광고 편성을 불허합니다.";
                }
                else
                {
                    schChoiceAdModel.ResultDesc = "편성단위별 회차편성 저장 중 오류가 발생하였습니다";
                    _log.Exception(ex);
                }
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion


        #region 메뉴광고엑셀편성 조회(엑셀 데이터중 없는것만 인서트 시키기 위해)
        /// <summary>
        /// 홈광고목록조회
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        public void SetSchChoiceSearch(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetSchChoiceSearch() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + " SELECT   \n"
                    + "        MediaCode             \n"
                    + "       ,GenreCode             \n"
                    + "       ,ItemNo                \n"
                    + "       ,AckNo                 \n"
                    + "       ,ScheduleOrder         \n"
                    + "		FROM SchChoiceMenuDetail                \n"
                    );


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 카테고리모델에 복사
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // 결과
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // 결과코드 셋트
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetSchChoiceSearch() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "메뉴광고 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }
        #endregion

        #region 채널광고엑셀편성 조회(엑셀 데이터중 없는것만 인서트 시키기 위해)
        /// <summary>
        /// 홈광고목록조회
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        public void SetSchChoiceChannelSearch(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetSchChoiceChannelSearch() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + " SELECT   \n"
                    + "        MediaCode             \n"
                    + "       ,ChannelNo             \n"
                    + "       ,ItemNo                \n"
                    + "       ,AckNo                 \n"
                    + "       ,ScheduleOrder         \n"
                    + "		FROM SchChoiceChannelDetail                \n"
                    );


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 카테고리모델에 복사
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // 결과
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // 결과코드 셋트
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetSchChoiceChannelSearch() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "채널광고 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }
        #endregion

        #region 지정채널광고 상세편성 엑셀저장
        /// <summary>
        /// 지정채널광고 상세편성 엑셀저장
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelDetailCreate_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            int err = 0;

            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDetailCreate_To() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MediaCode         :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("ChannelNo         :[" + schChoiceAdModel.ChannelNo + "]");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("AdType            :[" + schChoiceAdModel.AdType + "]");
                // __DEBUG__				

                int i = 0;
                int rc = 0;
                int adSchType = 0;

                if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                {
                    adSchType = 10;
                }
                else
                    adSchType = 20;

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);
                StringBuilder sbQuery = new StringBuilder();

                // 해당 채널이 존재하는지 검사
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT *           \n"
                    + "  FROM Channel     \n"
                    + " WHERE MediaCode  = " + schChoiceAdModel.MediaCode + " \n"
                    + "   AND ChannelNo  = " + schChoiceAdModel.ChannelNo + " \n"
                    );

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                rc = Utility.GetDatasetCount(ds);

                // 결과
                // 0건이면 채널이 존재하지 않는것이다.
                if (rc == 0)
                {
                    ds.Dispose();
                    err = 1;	// 채널이 존재하지 않음
                    throw new Exception();
                }
                ds.Dispose();


                string AdType = schChoiceAdModel.AdType;	// 광고종류 : 10:CM 11:EAP(10~19는 필수광고) 20:OAP 

                // 2007.09.04 상업광고 편성 불가 컨텐츠 처리
                // 해당 채널의 컨텐츠를 검사하여 상업광고(필수광고) 편성여부를 결정한다.
                if (AdType.Equals("10")) // 10~19사이가 필수광고이다. 코드가 1로 들어오면 어떻하지? ㅠ.ㅠ
                {
                    // 해당 채널이 존재하는지 검사
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + "SELECT *   \n"
                        + "  FROM Channel A INNER JOIN Contents B ON (A.ContentID = B.ContentID) \n"
                        + " WHERE A.MediaCode  = " + schChoiceAdModel.MediaCode + " \n"
                        + "   AND A.ChannelNo  = " + schChoiceAdModel.ChannelNo + " \n"
                        + "   AND ProdType IS NOT NULL     \n"
                        + "   AND ProdType <> '' \n"
                        );

                    // 쿼리실행
                    ds = new DataSet();
                    _db.ExecuteQuery(ds, sbQuery.ToString());

                    rc = Utility.GetDatasetCount(ds);

                    // 결과
                    // 1건이라도 있다면 채널에 필수광고를 편성할 수 없다.
                    if (rc > 0)
                    {
                        err = 2;
                        ds.Dispose();
                        throw new Exception();
                    }
                    ds.Dispose();


                }


                SqlParameter[] sqlParams = new SqlParameter[1];

                //지정채널편성테이블에 데이터가 없을 경우 Insert
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT ItemNo FROM SchChoiceChannel  \n"
                    + " WHERE ItemNo    =  @ItemNo           \n"
                    );

                i = 0;
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                rc = Utility.GetDatasetCount(ds);
                ds.Dispose();

                // 결과
                // 0건이면 지정채널편성테이에 ItemNo Insert
                if (rc == 0)
                {
                    // 쿼리실행
                    try
                    {
                        _db.BeginTran();


                        // 지정채널 편성 테이블에 추가
                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceChannel ( \n"
                            + "            ItemNo            \n"
                            + "       )                      \n"
                            + "       VALUES (               \n"
                            + "           @ItemNo            \n"
                            + "       )                      \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        // 광고상태 편성으로 변경
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "UPDATE ContractItem        \n"
                            + "   SET AdState = '20'      \n"   // 광고상태 - 20:편성
                            + "      ,ModDt   = GETDATE() \n"
                            + "      ,RegID   = '" + header.UserID + "' \n"
                            + " WHERE ItemNo  = @ItemNo   \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);


                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("신규편성 지정채널광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] 등록자:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }


                sqlParams = new SqlParameter[4];

                i = 0;
                sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@ChannelNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@ScheduleOrder", SqlDbType.SmallInt);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ChannelNo);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);
                sqlParams[i++].Value = Convert.ToInt16(schChoiceAdModel.ScheduleOrder);

                // 기편성이 되어있는지 검사
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT * FROM SchChoiceChannelDetail  \n"
                    + " WHERE MediaCode =  @MediaCode        \n"
                    + "   AND ChannelNo =  @ChannelNo        \n"
                    + "   AND ItemNo    =  @ItemNo           \n"
                    );

                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                rc = Utility.GetDatasetCount(ds);

                ds.Dispose();

                // 결과
                // 0건이면 미편성된 광고
                if (rc == 0)
                {
                    // 쿼리실행
                    try
                    {
                        _db.BeginTran();

                        // 지정채널 편성 테이블에 추가
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceChannelDetail (      \n"
                            + "            MediaCode                     \n"
                            + "           ,ChannelNo                     \n"
                            + "           ,ItemNo                        \n"
                            + "           ,AckNo                         \n"
                            + "           ,ScheduleOrder                 \n"
                            + "       )                                  \n"
                            + "       VALUES (                           \n"
                            + "           @MediaCode                     \n"
                            + "          ,@ChannelNo                     \n"
                            + "          ,@ItemNo                        \n"
                            + "          ," + AckNo + "  \n"
                            + "          ,@ScheduleOrder                              \n" // 필수광고의 순서는 0
                            + "       )                                  \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("신규편성 지정채널광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] 등록자:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    // __MESSAGE__
                    _log.Message("기편성 지정채널광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] 등록자:[" + header.UserID + "]");
                }


                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDetailCreate_To() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                if (err == 1)
                {
                    schChoiceAdModel.ResultDesc = "입력하신 채널은 존재하지 않는 채널번호입니다.";
                }
                else if (err == 2)
                {
                    schChoiceAdModel.ResultDesc = "해당 채널은 유료컨텐츠이므로 상업광고 편성을 불허합니다.";
                }
                else
                {
                    schChoiceAdModel.ResultDesc = "지정채널광고 상세편성 저장 중 오류가 발생하였습니다";
                    _log.Exception(ex);
                }
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion


        #region 메뉴광고 편성

        /// <summary>
        /// 메뉴편성 1차 작업
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceMenuCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuCreate() Start");
                _log.Debug("-----------------------------------------");

                try
                {
                    int rc = 0;
                    int adSchType = 0;

                    adSchType = 10;

                    // 현재 승인번호를 구함
                    string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                    StringBuilder sbQuery = new StringBuilder();
                    OracleParameter[] sqlParams = new OracleParameter[2];

                    // 파라메터 셋트
                    sqlParams[0] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                    sqlParams[1] = new OracleParameter(":AckNo", OracleDbType.Int32);
                    sqlParams[0].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);
                    sqlParams[1].Value = Convert.ToInt32(AckNo);

                    // 기편성이 되어있는지 검사
                    sbQuery.AppendFormat("\n SELECT * FROM SCHD_MENU WHERE ITEM_NO = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));
                    DataSet ds = new DataSet();
                    _db.ExecuteQuery(ds, sbQuery.ToString());

                    int cnt = Utility.GetDatasetCount(ds);

                    // 0건이면 미편성된 광고
                    if (cnt == 0)
                    {
                        _db.BeginTran();

                        // 지정메뉴광고 편성 테이블에 추가
                        sbQuery.Clear();
                        sbQuery.Append("\n INSERT INTO SCHD_MENU");
                        sbQuery.Append("\n		  (	ITEM_NO, MENU_COD, MENU_COD_PAR, ACK_NO, SCHD_ORD )");
                        sbQuery.AppendFormat("\n VALUES ( {0}, '0000000000','0000000000',{1}, 0 )", Convert.ToInt32(schChoiceAdModel.ItemNo), Convert.ToInt32(AckNo));

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());

                        // 지정메뉴광고 편성 테이블에 추가
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n UPDATE  ADVT_MST");
                        sbQuery.Append("\n SET		ADVT_STT	= '20'");		// 광고상태 - 20:편성
                        sbQuery.Append("\n		,	DT_UPDATE	= SYSDATE");
                        sbQuery.Append("\n		,	ID_UPDATE   = '" + header.UserID + "'");
                        sbQuery.Append("\n WHERE	ITEM_NO     = " + schChoiceAdModel.ItemNo);

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());
                        _db.CommitTran();
                        _log.Message("신규편성 지정메뉴광고:[" + schChoiceAdModel.ItemName + "] 등록자:[" + header.UserID + "]");
                    }
                    else
                    {
                        _log.Message("기편성 지정메뉴광고:[" + schChoiceAdModel.ItemName + "]");
                    }
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "지정메뉴광고 저장 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 메뉴광고 삭제
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceMenuDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDelete() Start");
                _log.Debug("-----------------------------------------");

                try
                {
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();

                    // 지정메뉴광고 편성 상세 테이블에서 해당 광고가 있는 메뉴을 조회한다.
                    sbQuery.AppendFormat("\n SELECT  MENU_COD");
                    sbQuery.AppendFormat("\n FROM    SCHD_MENU");
                    sbQuery.AppendFormat("\n WHERE   ITEM_NO = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));
                    sbQuery.AppendFormat("\n ORDER BY MENU_COD");

                    DataSet ds = _db.ExecuteQuery(sbQuery.ToString());

                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        DataRow row = ds.Tables[0].Rows[j];
                        string GenreCode = row["MENU_COD"].ToString();

                        schChoiceAdModel.GenreCode = GenreCode;

                        // 각 메뉴별로 상세내역을 삭제한다.
                        SetSchChoiceMenuDetailDelete2(header, schChoiceAdModel);
                    }
                    ds.Dispose();

                    // __MESSAGE__
                    _log.Message("지정메뉴광고 삭제:[" + schChoiceAdModel.ItemName + "] 등록자:[" + header.UserID + "]" + rc.ToString());

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "메뉴광고 편성내역 삭제 중 오류가 발생하였습니다";
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
        public void GetSchChoiceMenuDetailList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceMenuDetailList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                // __DEBUG__

                // 쿼리실행
                StringBuilder sbQuery = new StringBuilder();
                OracleParameter[] sqlParams = new OracleParameter[1];

                sqlParams[0] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                sqlParams[0].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // 메뉴 편성 테이블
                sbQuery.Append("\n SELECT  'False'     AS CheckYn");
                sbQuery.Append("\n     ,   CA.MENU_NM  AS CategoryName");
                sbQuery.Append("\n     ,   GE.MENU_NM  AS GenreName");
                sbQuery.Append("\n     ,   A.MENU_COD  AS GenreCode");
                sbQuery.Append("\n     ,   C.ACK_STT   AS AckState");
                sbQuery.Append("\n   FROM SCHD_MENU A");
                sbQuery.Append("\n         INNER JOIN ADVT_MST B       ON B.ITEM_NO    = A.ITEM_NO");
                sbQuery.Append("\n         INNER JOIN MENU_COD CA      ON CA.MENU_COD  = A.MENU_COD_PAR");
                sbQuery.Append("\n         INNER JOIN MENU_COD GE      ON GE.MENU_COD  = A.MENU_COD");
                sbQuery.Append("\n         LEFT  JOIN SCHD_DIST_MST C  ON C.ACK_NO     = A.ACK_NO");
                sbQuery.Append("\n WHERE   A.ITEM_NO = :ItemNo");
                sbQuery.Append("\n ORDER BY CategoryName, GenreName");

                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                // 결과 DataSet의 모델에 복사
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                schChoiceAdModel.ResultCD = "0000";  // 정상

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceMenuDetailList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "메뉴광고 상세편성 조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
        #endregion

        #region 지정메뉴광고 상세편성 저장

        /// <summary>
        /// 지정메뉴광고 상세편성 저장
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceMenuDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDetailCreate() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MediaCode         :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("GenreCode         :[" + schChoiceAdModel.GenreCode + "]");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("AdType            :[" + schChoiceAdModel.AdType + "]");
                // __DEBUG__				
                int rc = 0;
                int adSchType = 0;
                adSchType = 10;
                //if(  schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                //{
                //	adSchType = 10;
                //}
                //else
                //	adSchType = 20;

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                StringBuilder sbQuery = new StringBuilder();

                //지정채널편성테이블에 데이터가 없을 경우 Insert
                sbQuery.AppendFormat("\n SELECT ITEM_NO FROM SCHD_MENU WHERE ITEM_NO = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));
                //_log.Debug(sbQuery.ToString());

                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과, 0건이면 지정채널편성테이에 ItemNo Insert
                if (Utility.GetDatasetCount(ds) == 0)
                {
                    // 쿼리실행
                    try
                    {
                        _db.BeginTran();

                        sbQuery.Clear();
                        sbQuery.Append("\n INSERT INTO SCHD_MENU");
                        sbQuery.Append("\n		  (	ITEM_NO, MENU_COD, MENU_COD_PAR, ACK_NO, SCHD_ORD )");
                        sbQuery.AppendFormat("\n VALUES ( {0}, '0000000000','0000000000',{1}, 0 )", Convert.ToInt32(schChoiceAdModel.ItemNo), Convert.ToInt32(AckNo));

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());

                        // 지정메뉴광고 편성 테이블에 추가
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n UPDATE  ADVT_MST");
                        sbQuery.Append("\n SET		ADVT_STT	= '20'");		// 광고상태 - 20:편성
                        sbQuery.Append("\n		,	DT_UPDATE	= SYSDATE");
                        sbQuery.Append("\n		,	ID_UPDATE   = '" + header.UserID + "'");
                        sbQuery.Append("\n WHERE	ITEM_NO     = " + schChoiceAdModel.ItemNo);

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());
                        _db.CommitTran();
                        _log.Message("신규편성 지정메뉴광고:[" + schChoiceAdModel.ItemName + "] 등록자:[" + header.UserID + "]");

                        // __MESSAGE__
                        _log.Message("신규편성 지정채널광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] 등록자:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }

                // 기편성이 되어있는지 검사
                sbQuery = new StringBuilder();
                sbQuery.AppendFormat("\n SELECT * FROM SCHD_MENU");
                sbQuery.AppendFormat("\n WHERE	MENU_COD = {0}", schChoiceAdModel.GenreCode);
                sbQuery.AppendFormat("\n AND	ITEM_NO  = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));

                _log.Debug(sbQuery.ToString());
                ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과, 0건이면 미편성된 광고임으로 정상처리
                if (Utility.GetDatasetCount(ds) == 0)
                {
                    try
                    {
                        _db.BeginTran();

                        sbQuery = new StringBuilder();
                        sbQuery.AppendFormat("\n INSERT INTO SCHD_MENU");
                        sbQuery.AppendFormat("\n 			( ITEM_NO, MENU_COD, MENU_COD_PAR, ACK_NO, SCHD_ORD )");
                        sbQuery.AppendFormat("\n VALUES ( {0},{1},{2},{3},{4} )", schChoiceAdModel.ItemNo, schChoiceAdModel.GenreCode, schChoiceAdModel.CategoryCode, AckNo, 0);

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());

                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("신규편성 지정메뉴광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] 등록자:[" + header.UserID + "]");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        // __DEBUG__
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    // __MESSAGE__
                    _log.Message("기편성 지정메뉴광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] 등록자:[" + header.UserID + "]");
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDetailCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "지정메뉴광고 상세편성 저장 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 재핑광고 상세편성 저장
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceRealChDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceRealChDetailCreate() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("Ch		:[" + schChoiceAdModel.GenreCode + "]");
                _log.Debug("ItemNo	:[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("AdType	:[" + schChoiceAdModel.AdType + "]");
                // __DEBUG__				
                int rc = 0;
                int adSchType = 0;
                adSchType = 10;

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                StringBuilder sbQuery = new StringBuilder();
                DataSet ds = new DataSet();

                // 기편성이 되어있는지 검사
                sbQuery = new StringBuilder();
                sbQuery.AppendFormat("\n SELECT * FROM SCHD_CHNL");
                sbQuery.AppendFormat("\n WHERE	CH_NO    = {0}", schChoiceAdModel.GenreCode);
                sbQuery.AppendFormat("\n AND	ITEM_NO  = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));

                _log.Debug(sbQuery.ToString());
                ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과, 0건이면 미편성된 광고임으로 정상처리
                if (Utility.GetDatasetCount(ds) == 0)
                {
                    try
                    {
                        _db.BeginTran();

                        sbQuery = new StringBuilder();
                        sbQuery.AppendFormat("\n INSERT INTO SCHD_CHNL");
                        sbQuery.AppendFormat("\n 			( ITEM_NO, CH_NO, ACK_NO, SCHD_ORD )");
                        sbQuery.AppendFormat("\n VALUES ( {0},{1},{2},{3})", schChoiceAdModel.ItemNo, schChoiceAdModel.GenreCode, AckNo, 0);

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());

                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n UPDATE  ADVT_MST");
                        sbQuery.Append("\n SET		ADVT_STT	= '20'");	// 광고상태 - 20:편성
                        sbQuery.Append("\n		,	DT_UPDATE	= SYSDATE");
                        sbQuery.Append("\n		,	ID_UPDATE   = '" + header.UserID + "'");
                        sbQuery.Append("\n WHERE	ITEM_NO     = " + schChoiceAdModel.ItemNo);
                        sbQuery.Append("\n AND		ADVT_STT    = '10'");	// 상태가 대기인 경우에만 변경

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());

                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("신규편성 재핑광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] 등록자:[" + header.UserID + "]");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        // __DEBUG__
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    _log.Message("기편성 재핑광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] 등록자:[" + header.UserID + "]");
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceRealChDetailCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "재핑광고 상세편성 저장 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        #endregion

        #region 지정메뉴광고 상세편성 삭제

        /// <summary>
        /// 지정메뉴광고 상세편성 삭제
        /// </summary>
        /// <returns></returns>
        ///
        public void SetSchChoiceMenuDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                SetSchChoiceMenuDetailDelete2(header, schChoiceAdModel);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        public void SetSchChoiceRealChDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _db.Open();
                SetSchChoiceRealChDetailDelete2(header, schChoiceAdModel);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 메뉴편성건 1건단위로 삭제하기
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceMenuDetailDelete2(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDetailDelete() Start");
                _log.Debug("-----------------------------------------");

                // 쿼리실행
                try
                {
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();
                    OracleParameter[] sqlParams = new OracleParameter[2];

                    sqlParams[0] = new OracleParameter(":GenreCode", OracleDbType.Varchar2, 10);
                    sqlParams[1] = new OracleParameter(":ItemNo", OracleDbType.Int32);

                    sqlParams[0].Value = schChoiceAdModel.GenreCode;
                    sqlParams[1].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                    // 삭제할 순서
                    string DeleteOrder = "0";

                    // 해당 삭제할 항목의 순서 구함
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT	SCHD_ORD				");
                    sbQuery.Append("\n FROM	SCHD_MENU				");
                    sbQuery.Append("\n WHERE	MENU_COD = :GenreCode	");
                    sbQuery.Append("\n AND		ITEM_NO  = :ItemNo		");

                    _db.BeginTran();
                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    if (Utility.GetDatasetCount(ds) != 0)
                    {
                        DeleteOrder = Utility.GetDatasetString(ds, 0, "SCHD_ORD");
                    }
                    ds.Dispose();


                    // 지정메뉴 편성 테이블에서 삭제
                    sbQuery = new StringBuilder();
                    sbQuery.AppendFormat("\n DELETE	SCHD_MENU				");
                    sbQuery.AppendFormat("\n WHERE	MENU_COD = {0}", schChoiceAdModel.GenreCode);
                    //sbQuery.AppendFormat("\n AND    MENU_COD_PAR = {0}", schChoiceAdModel.CategoryCode);
                    sbQuery.AppendFormat("\n AND	ITEM_NO  = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));

                    rc = _db.ExecuteNonQuery(sbQuery.ToString());
                    _log.Debug(sbQuery.ToString());

                    //// 비율상세 삭제
                    //sbQuery.Append( "\n"
                    //    + "DELETE SchRateDetail    \n"
                    //    + " WHERE MediaCode = "+ schChoiceAdModel.MediaCode +" \n" 
                    //    + "   AND GenreCode = "+ schChoiceAdModel.GenreCode +" \n"
                    //    + "   AND ItemNo    = "+ schChoiceAdModel.ItemNo +"    \n"
                    //    );

                    //rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    //_log.Debug(sbQuery.ToString());


                    // 순위가 0이 아닌 것에 대하여
                    //if(!DeleteOrder.Equals("0"))
                    //{
                    //    // 해당 순위보다 큰 순위의 내역들을 -1하여 조정
                    //    sbQuery   = new StringBuilder();
                    //    sbQuery.Append( "\n"
                    //        + "UPDATE SchChoiceMenuDetail                                 \n"
                    //        + "   SET ScheduleOrder = ScheduleOrder - 1                   \n"
                    //        + "      ,AckNo         = " + AckNo                       + " \n"
                    //        + " WHERE MediaCode     = " + schChoiceAdModel.MediaCode  + " \n"
                    //        + "   AND GenreCode     = " + schChoiceAdModel.GenreCode  + " \n"
                    //        + "   AND ScheduleOrder > " + DeleteOrder + " \n"
                    //        + "   AND ScheduleOrder > 0                                       \n"
                    //        );

                    //    _log.Debug(sbQuery.ToString());

                    //    rc = _db.ExecuteNonQuery(sbQuery.ToString());

                    //}	

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                _db.CommitTran();
                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDetailDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "지정메뉴광고 상세편성 삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }

        }

        /// <summary>
        /// 재핑편성건 1건단위로 삭제하기
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceRealChDetailDelete2(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceRealChDetailDelete2() Start");
                _log.Debug("-----------------------------------------");

                // 쿼리실행
                try
                {
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();

                    // 지정메뉴 편성 테이블에서 삭제
                    sbQuery = new StringBuilder();
                    sbQuery.AppendFormat("\n DELETE	SCHD_CHNL				");
                    sbQuery.AppendFormat("\n WHERE	CH_NO	= {0}", schChoiceAdModel.GenreCode);
                    sbQuery.AppendFormat("\n AND	ITEM_NO = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));

                    _db.BeginTran();
                    rc = _db.ExecuteNonQuery(sbQuery.ToString());
                    _log.Debug(sbQuery.ToString());
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                _db.CommitTran();
                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceRealChDetailDelete2() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "지정메뉴광고 상세편성 삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }

        }

        #endregion


        #region 마지막광고 조회

        /// <summary>
        /// 마지막광고 조회
        /// </summary>
        /// <returns></returns>
        public void GetSchChoiceLastItemNo(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceLastItemNo() Start");
                _log.Debug("-----------------------------------------");


                // 쿼리실행
                try
                {
                    StringBuilder sbQuery = new StringBuilder();

                    // 마지막 광고
                    string LastItemNo = "1";

                    // 해당 마지막 광고 구함
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + " SELECT ISNULL(MAX(TA.ItemNo),1) AS LastItemNo                \n"
                        + "  FROM (                                                     \n"
                        + "        SELECT ISNULL(MAX(A.ItemNo),1) AS ItemNo               \n"
                        + "          FROM SchChoiceMenu    A INNER JOIN ContractItem B  \n"
                        + "                                          ON (A.ItemNo = B.ItemNo AND B.MediaCode = " + schChoiceAdModel.MediaCode + ") \n"
                        + "        UNION                                                \n"
                        + "        SELECT ISNULL(MAX(A.ItemNo),1) AS ItemNo               \n"
                        + "          FROM SchChoiceChannel A INNER JOIN ContractItem B  \n"
                        + "                                          ON (A.ItemNo = B.ItemNo AND B.MediaCode = " + schChoiceAdModel.MediaCode + ") \n"
                        + "       ) TA \n"
                        );

                    //__DEBUG__
                    _log.Debug(sbQuery.ToString());
                    //__DEBUG__

                    // 쿼리실행
                    DataSet ds = new DataSet();
                    _db.ExecuteQuery(ds, sbQuery.ToString());


                    if (Utility.GetDatasetCount(ds) != 0)
                    {
                        LastItemNo = Utility.GetDatasetString(ds, 0, "LastItemNo");
                    }

                    ds.Dispose();

                    schChoiceAdModel.ItemNo = LastItemNo;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceLastItemNo() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "마지막광고 조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion

        #region 삭제후 마지막광고 조회

        /// <summary>
        /// 마지막광고 조회
        /// </summary>
        /// <returns></returns>
        public void GetSchChoiceLastItemNoDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceLastItemNoDelete() Start");
                _log.Debug("-----------------------------------------");


                // 쿼리실행
                try
                {
                    StringBuilder sbQuery = new StringBuilder();

                    // 마지막 광고
                    string LastItemNo = "1";

                    // 해당 마지막 광고 구함
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + " SELECT ISNULL(MAX(TA.ItemNo),1) AS LastItemNo               \n"
                        + "  FROM (                                                     \n"
                        + "        SELECT ISNULL(MAX(A.ItemNo),1) AS ItemNo               \n"
                        + "          FROM SchChoiceMenu    A INNER JOIN ContractItem B  \n"
                        + "                                          ON (A.ItemNo = B.ItemNo AND B.MediaCode = " + schChoiceAdModel.MediaCode + ") \n"
                        + "           AND A.ItemNo < " + schChoiceAdModel.ItemNo + " \n"
                        + "        UNION                                                \n"
                        + "        SELECT ISNULL(MAX(A.ItemNo),1) AS ItemNo               \n"
                        + "          FROM SchChoiceChannel A INNER JOIN ContractItem B  \n"
                        + "                                          ON (A.ItemNo = B.ItemNo AND B.MediaCode = " + schChoiceAdModel.MediaCode + ") \n"
                        + "           AND A.ItemNo < " + schChoiceAdModel.ItemNo + " \n"
                        + "       )  TA \n"
                        );

                    //__DEBUG__
                    _log.Debug(sbQuery.ToString());
                    //__DEBUG__


                    // 쿼리실행
                    DataSet ds = new DataSet();
                    _db.ExecuteQuery(ds, sbQuery.ToString());

                    if (Utility.GetDatasetCount(ds) != 0)
                    {
                        LastItemNo = Utility.GetDatasetString(ds, 0, "LastItemNo");
                    }

                    ds.Dispose();

                    schChoiceAdModel.ItemNo = LastItemNo;

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceLastItemNoDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "삭제후 마지막광고 조회 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion

        #region 지정메뉴광고 엑셀삭제

        /// <summary>
        /// 지정메뉴광고 엑셀삭제
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceMenuDelete_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDelete() Start");
                _log.Debug("-----------------------------------------");


                // 쿼리실행
                try
                {
                    int i = 0;
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();
                    SqlParameter[] sqlParams = new SqlParameter[1];

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);

                    _db.BeginTran();

                    // 지정메뉴광고 편성 상세 테이블에서 삭제
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceMenuDetail            \n"
                        + " WHERE MediaCode        = @MediaCode        \n"
                        );

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    // 지정메뉴광고 편성 테이블에서 삭제
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceMenu                  \n"
                        + " WHERE ItemNo  IN (SELECT ItemNo FROM ContractItem WHERE MediaCode = @MediaCode)         \n"
                        );

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("지정메뉴광고 삭제:[" + schChoiceAdModel.MediaCode + "] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "지정메뉴광고 편성내역 삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion

        #region 지정메뉴광고 엑셀상세편성 저장

        /// <summary>
        /// 지정메뉴광고 엑셀상세편성 저장
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceMenuDetailCreate_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDetailCreate() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MediaCode         :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("GenreCode         :[" + schChoiceAdModel.GenreCode + "]");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("ScheduleOrder            :[" + schChoiceAdModel.ScheduleOrder + "]");
                _log.Debug("AdType            :[" + schChoiceAdModel.AdType + "]");
                // __DEBUG__				

                int i = 0;
                int rc = 0;
                int adSchType = 0;

                if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                {
                    adSchType = 10;
                }
                else
                    adSchType = 20;

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[1];

                //지정채널편성테이블에 데이터가 없을 경우 Insert
                sbQuery.Append("\n"
                    + "SELECT ItemNo FROM SchChoiceMenu  \n"
                    + " WHERE ItemNo    =  @ItemNo           \n"
                    );

                i = 0;
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                // 결과
                // 0건이면 지정채널편성테이에 ItemNo Insert
                if (Utility.GetDatasetCount(ds) == 0)
                {
                    // 쿼리실행
                    try
                    {
                        _db.BeginTran();


                        // 지정채널 편성 테이블에 추가
                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceMenu ( \n"
                            + "            ItemNo            \n"
                            + "       )                      \n"
                            + "       VALUES (               \n"
                            + "           @ItemNo            \n"
                            + "       )                      \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        // 광고상태 편성으로 변경
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "UPDATE ContractItem        \n"
                            + "   SET AdState = '20'      \n"   // 광고상태 - 20:편성
                            + "      ,ModDt   = GETDATE() \n"
                            + "      ,RegID   = '" + header.UserID + "' \n"
                            + " WHERE ItemNo  = @ItemNo   \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);


                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("신규편성 지정채널광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] 등록자:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }


                sqlParams = new SqlParameter[4];

                i = 0;
                sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@GenreCode", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@ScheduleOrder", SqlDbType.SmallInt);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.GenreCode);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);
                sqlParams[i++].Value = Convert.ToInt16(schChoiceAdModel.ScheduleOrder);

                // 기편성이 되어있는지 검사
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT * FROM SchChoiceMenuDetail  \n"
                    + " WHERE MediaCode =  @MediaCode        \n"
                    + "   AND GenreCode =  @GenreCode        \n"
                    + "   AND ItemNo    =  @ItemNo           \n"
                    );

                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                // 결과
                // 0건이면 미편성된 광고
                if (Utility.GetDatasetCount(ds) == 0)
                {
                    // 쿼리실행
                    try
                    {
                        _db.BeginTran();

                        // 지정채널 편성 테이블에 추가
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceMenuDetail ( \n"
                            + "            MediaCode             \n"
                            + "           ,GenreCode             \n"
                            + "           ,ItemNo                \n"
                            + "           ,AckNo                 \n"
                            + "           ,ScheduleOrder         \n"
                            + "       )                          \n"
                            + "       VALUES(                    \n"
                            + "           @MediaCode             \n"
                            + "          ,@GenreCode             \n"
                            + "          ,@ItemNo                \n"
                            + "          ," + AckNo + "  \n"
                            + "          ,@ScheduleOrder                      \n" // 필수광고의 순서는 0
                            + "      )                           \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("신규편성 지정메뉴광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] 등록자:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    // __MESSAGE__
                    _log.Message("기편성 지정메뉴광고:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] 등록자:[" + header.UserID + "]");
                }

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDetailCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "지정메뉴광고 상세편성 저장 중 오류가 발생하였습니다";
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
        private string GetLastAckNo(string MediaCode, int AdSchType)
        {
            string AckNo = "";
            string AckState = "";
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetLastAckNo() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MediaCode	    :[" + MediaCode + "]");		// 검색 매체
                _log.Debug("AdSchType		:[" + AdSchType + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.AppendFormat("\n SELECT * FROM TABLE(GET_ACKNO( {0} ))", AdSchType.ToString());
                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    AckNo = ds.Tables[0].Rows[0]["AckNo"].ToString();
                    AckState = ds.Tables[0].Rows[0]["AckState"].ToString();
                }

                ds.Dispose();

                _log.Debug("-----------------------------------------");
                _log.Debug("AckNo:" + AckNo.ToString() + ", State:" + AckState.ToString());
                _log.Debug(this.ToString() + "GetLastAckNo() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _log.Exception(ex);
                throw ex;
            }
            return AckNo;
        }

        #endregion

        #region 편성 내역 복사하기 [E_02]
        /// <summary>
        /// 사용자의 요청 값에 따라 기편성 되어있는 광고의 편성내역을 삭제하는 기능과
        /// 선택한 광고(ItemNoCopy)의 편성내역을 복사하는 기능 구현 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceAdCopy(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceAdCopy() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("ItemNoCopy        :[" + schChoiceAdModel.ItemNoCopy + "]");
                _log.Debug("AdType            :[" + schChoiceAdModel.AdType + "]");
                _log.Debug("MediaCode         :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("CheckSchResult    :[" + schChoiceAdModel.CheckSchResult + "]");
                // __DEBUG__				

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[4];
                DataSet ds = new DataSet();

                int rc = 0;
                int adSchType = 0;
                string AdType = schChoiceAdModel.AdType;

                if (AdType == "10" || AdType == "13" || AdType == "16" || AdType == "17" || AdType == "19")
                {
                    adSchType = 10;
                }
                else
                    adSchType = 20;


                //삭제 및 추가 작업이 실패할 경우를 대비하여 트랜잭션 사용
                _db.BeginTran();

                sqlParams[0] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[0].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                sqlParams[1] = new SqlParameter("@UserID", SqlDbType.VarChar, 8);
                sqlParams[1].Value = header.UserID;

                sqlParams[2] = new SqlParameter("@ItemNoCopy", SqlDbType.Int);
                sqlParams[2].Value = Convert.ToInt32(schChoiceAdModel.ItemNoCopy);

                sqlParams[3] = new SqlParameter("@AckNo", SqlDbType.Int);
                // 현재 승인번호를 구함
                sqlParams[3].Value = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                #region 기편성 삭제
                //기편성을 삭제 후 추가할 경우

                if (schChoiceAdModel.CheckSchResult.Equals("DELETE"))
                {
                    try
                    {
                        _log.Debug("-----------------------------------------");
                        _log.Debug("광고 편성 내역 삭제 Start");
                        _log.Debug("-----------------------------------------");

                        if (!AdType.StartsWith("1"))//10~19 사이의 필수 광고를 제외하고 ScheduleOrder를 조정
                        {
                            //메뉴편성
                            sbQuery.Append("\n UPDATE A                                     \n");
                            sbQuery.Append(" SET A.ScheduleOrder = A.ScheduleOrder-1        \n");
                            sbQuery.Append(" FROM SchChoiceMenuDetail A                     \n");
                            sbQuery.Append(" INNER JOIN SchChoiceMenuDetail B               \n");
                            sbQuery.Append("            ON B.ItemNo=@ItemNo                 \n");
                            sbQuery.Append("            AND A.MediaCode=B.MediaCode         \n");
                            sbQuery.Append("            AND A.GenreCode=B.GenreCode         \n");
                            sbQuery.Append(" WHERE A.ScheduleOrder > B.ScheduleOrder        \n");
                            //채널편성
                            sbQuery.Append("\n UPDATE A                                     \n");
                            sbQuery.Append(" SET A.ScheduleOrder = A.ScheduleOrder-1        \n");
                            sbQuery.Append(" FROM SchchoiceChannelDetail A                  \n");
                            sbQuery.Append(" INNER JOIN SchchoiceChannelDetail B            \n");
                            sbQuery.Append("            ON B.ItemNo=@ItemNo                 \n");
                            sbQuery.Append("            AND A.MediaCode=B.MediaCode         \n");
                            sbQuery.Append("            AND A.ChannelNo=B.ChannelNo         \n");
                            sbQuery.Append(" WHERE A.ScheduleOrder > B.ScheduleOrder        \n");
                            //회차편성
                            sbQuery.Append("\n UPDATE A                                     \n");
                            sbQuery.Append(" SET A.ScheduleOrder = A.ScheduleOrder-1        \n");
                            sbQuery.Append(" FROM SchChoiceSeriesDetail A                   \n");
                            sbQuery.Append(" INNER JOIN SchChoiceSeriesDetail B             \n");
                            sbQuery.Append("            ON B.ItemNo=@ItemNo                 \n");
                            sbQuery.Append("            AND A.MediaCode=B.MediaCode         \n");
                            sbQuery.Append("            AND A.ChannelNo=B.ChannelNo         \n");
                            sbQuery.Append("            AND A.SeriesNo=B.SeriesNo           \n");
                            sbQuery.Append(" WHERE A.ScheduleOrder > B.ScheduleOrder        \n");

                            rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                            _log.Debug(sbQuery.ToString());
                        }

                        sbQuery = new StringBuilder();
                        //메뉴, 채널, 회차, 지정 상세테이블에서 편성내역 삭제
                        sbQuery.Append("\n DELETE FROM SchChoiceMenuDetail      \n");
                        sbQuery.Append("    WHERE ItemNo =  @ItemNo             \n");
                        sbQuery.Append("\n DELETE FROM SchChoiceChannelDetail   \n");
                        sbQuery.Append("    WHERE ItemNo =  @ItemNo             \n");
                        sbQuery.Append("\n DELETE FROM SchChoiceSeriesDetail    \n");
                        sbQuery.Append("    WHERE ItemNo =  @ItemNo             \n");
                        sbQuery.Append("\n DELETE FROM SchDesignatedDetail      \n");
                        sbQuery.Append("    WHERE ItemNo =  @ItemNo             \n");

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        _log.Debug("-----------------------------------------");
                        _log.Debug("광고 편성 내역 삭제 End");
                        _log.Debug("-----------------------------------------");
                        // __DEBUG__

                    }
                    catch (Exception ex)
                    {
                        schChoiceAdModel.ResultCD = "3301";
                        schChoiceAdModel.ResultDesc = "편성 삭제중 오류가 발생하였습니다.";
                        _log.Exception(ex);
                        throw ex;
                    }
                }
                #endregion

                try
                {
                    #region 메뉴편성테이블
                    //지정메뉴편성테이블에 데이터가 없을 경우 Insert
                    _log.Debug("-----------------------------------------");
                    _log.Debug("메뉴편성 테이블 INSERT START");
                    _log.Debug("-----------------------------------------");

                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT ItemNo FROM SchChoiceMenu with(noLock)    \n");
                    sbQuery.Append("    WHERE ItemNo =  @ItemNo                         \n");

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // 쿼리실행
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // 결과 0건이면 지정메뉴편성테이블에 ItemNo Insert
                    if (Utility.GetDatasetCount(ds) == 0)
                    {
                        // 쿼리실행
                        try
                        {
                            // 지정채널 편성 테이블에 추가
                            sbQuery = new StringBuilder();
                            sbQuery.Append("\n");
                            sbQuery.Append("INSERT INTO SchChoiceMenu (   \n");
                            sbQuery.Append("            ItemNo            \n");
                            sbQuery.Append("       )                      \n");
                            sbQuery.Append("       VALUES (               \n");
                            sbQuery.Append("           @ItemNo            \n");
                            sbQuery.Append("       )                      \n");

                            rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                            // 광고상태 편성으로 변경
                            sbQuery = new StringBuilder();

                            sbQuery.Append("\n");
                            sbQuery.Append("UPDATE ContractItem                      \n");
                            sbQuery.Append("   SET AdState = '20'                    \n");   // 광고상태 - 20:편성
                            sbQuery.Append("      ,ModDt   = GETDATE()               \n");
                            sbQuery.Append("      ,RegID   = @UserID                 \n");
                            sbQuery.Append(" WHERE ItemNo  = @ItemNo                 \n");

                            rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                            // __DEBUG__
                            _log.Debug(sbQuery.ToString());
                            _log.Debug("-----------------------------------------");
                            _log.Debug("메뉴편성 테이블 INSERT END");
                            _log.Debug("-----------------------------------------");
                            // __DEBUG__
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    #endregion
                    #region 채널편성테이블
                    //지정채널편성테이블에 데이터가 없을 경우 Insert
                    _log.Debug("-----------------------------------------");
                    _log.Debug("채널편성 테이블 INSERT START");
                    _log.Debug("-----------------------------------------");
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n");
                    sbQuery.Append("SELECT ItemNo FROM SchChoiceChannel with(NoLock)  \n");
                    sbQuery.Append(" WHERE ItemNo    =  @ItemNo                       \n");

                    // 쿼리실행
                    ds = new DataSet();
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // 결과 0건이면 지정채널편성테이블에 ItemNo Insert
                    if (Utility.GetDatasetCount(ds) == 0)
                    {
                        // 쿼리실행
                        try
                        {
                            // 지정채널 편성 테이블에 추가
                            sbQuery = new StringBuilder();
                            sbQuery.Append("\n");
                            sbQuery.Append("INSERT INTO SchChoiceChannel (       \n");
                            sbQuery.Append("            ItemNo                   \n");
                            sbQuery.Append("       )                             \n");
                            sbQuery.Append("       VALUES (                      \n");
                            sbQuery.Append("           @ItemNo                   \n");
                            sbQuery.Append("       )                             \n");

                            rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                            // 광고상태 편성으로 변경
                            sbQuery = new StringBuilder();

                            sbQuery.Append("\n");
                            sbQuery.Append("UPDATE ContractItem                      \n");
                            sbQuery.Append("   SET AdState = '20'                    \n");   // 광고상태 - 20:편성
                            sbQuery.Append("      ,ModDt   = GETDATE()               \n");
                            sbQuery.Append("      ,RegID   = @UserID                 \n");
                            sbQuery.Append(" WHERE ItemNo  = @ItemNo                 \n");

                            rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                            // __DEBUG__
                            _log.Debug(sbQuery.ToString());
                            _log.Debug("-----------------------------------------");
                            _log.Debug("채널편성 테이블 INSERT END");
                            _log.Debug("-----------------------------------------");
                            // __DEBUG__
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    #endregion

                    #region 상세메뉴편성 insert

                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT ItemNo FROM SchChoiceMenuDetail with(noLock)    \n");
                    sbQuery.Append("    WHERE ItemNo =  @ItemNoCopy                           \n");

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // 쿼리실행
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // 결과가 존재하면 상세채널편성테이블에 Insert
                    if (Utility.GetDatasetCount(ds) > 0)
                    {
                        _log.Debug("-----------------------------------------");
                        _log.Debug("상세메뉴편성 INSERT START");
                        _log.Debug("-----------------------------------------");

                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n");
                        sbQuery.Append(" INSERT INTO SchChoiceMenuDetail(               \n");
                        sbQuery.Append(" 	    ItemNo                                  \n");
                        sbQuery.Append(" 	    ,MediaCode                              \n");
                        sbQuery.Append(" 	    ,GenreCode                              \n");
                        sbQuery.Append(" 	    ,AckNo                                  \n");
                        sbQuery.Append(" 	    ,ScheduleOrder                          \n");
                        sbQuery.Append(" 	)                                           \n");
                        sbQuery.Append(" 	SELECT                                      \n");
                        sbQuery.Append(" 		@ItemNo     ItemNo                      \n");
                        sbQuery.Append(" 		,MediaCode                              \n");
                        sbQuery.Append(" 		,GenreCode                              \n");
                        sbQuery.Append(" 		,@AckNo     AckNo                       \n");

                        if (AdType.StartsWith("1")) //10~19사이 필수 광고
                        {
                            sbQuery.Append(" 	,0 ScheduleOrder                        \n");
                        }
                        else
                        {
                            sbQuery.Append(" 	,(SELECT                                \n");
                            sbQuery.Append("      ISNULL(MAX(ScheduleOrder),0)+1 AS ScheduleOrder  \n");// 옵션광고(OAP등)의 순서는 MAX+1
                            sbQuery.Append("      FROM SchChoiceMenuDetail              \n");
                            sbQuery.Append("      WHERE MediaCode   =   A.MediaCode     \n");
                            sbQuery.Append("      AND   GenreCode   =   A.GenreCode)    \n");
                        }
                        sbQuery.Append(" 	FROM SchChoiceMenuDetail A                  \n");
                        sbQuery.Append(" 	WHERE not exists(                           \n"); //exists는 존재 여부를 확인하기 위함. 하나라도 존재하면 true값 리턴.
                        sbQuery.Append(" 		SELECT 1 FROM SchChoiceMenuDetail B     \n"); //where조건절에 해당하는 값이 존재하면 그 값은 상관이 없기때문에 1을 사용함
                        sbQuery.Append(" 			WHERE   B.MediaCode=A.MediaCode     \n");
                        sbQuery.Append(" 			AND     B.GenreCode=A.GenreCode     \n");
                        sbQuery.Append(" 			AND     B.ItemNo   =@ItemNo)        \n");
                        sbQuery.Append("   AND A.ItemNo = @ItemNoCopy                   \n");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        _log.Debug("-----------------------------------------");
                        _log.Debug("상세메뉴편성 INSERT END");
                        _log.Debug("-----------------------------------------");
                        // __DEBUG__

                        // 쿼리실행
                        ds = new DataSet();
                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    }
                    #endregion
                    #region 상세채널편성 insert

                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT ItemNo FROM SchChoiceChannelDetail with(noLock) \n");
                    sbQuery.Append("    WHERE ItemNo =  @ItemNoCopy                           \n");

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // 쿼리실행
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // 결과가 존재하면 상세메뉴편성테이블에 Insert
                    if (Utility.GetDatasetCount(ds) > 0)
                    {
                        _log.Debug("-----------------------------------------");
                        _log.Debug("상세채널편성 INSERT START");
                        _log.Debug("-----------------------------------------");

                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n");
                        sbQuery.Append(" INSERT INTO SchChoiceChannelDetail(            \n");
                        sbQuery.Append(" 	    ItemNo                                  \n");
                        sbQuery.Append(" 	    ,MediaCode                              \n");
                        sbQuery.Append(" 	    ,ChannelNo                              \n");
                        sbQuery.Append(" 	    ,AckNo                                  \n");
                        sbQuery.Append(" 	    ,ScheduleOrder                          \n");
                        sbQuery.Append(" 	)                                           \n");
                        sbQuery.Append(" 	SELECT                                      \n");
                        sbQuery.Append(" 		@ItemNo     ItemNo                      \n");
                        sbQuery.Append(" 		,MediaCode                              \n");
                        sbQuery.Append(" 		,ChannelNo                              \n");
                        sbQuery.Append(" 		,@AckNo     AckNo                       \n");

                        if (AdType.StartsWith("1")) //10~19사이 필수 광고
                        {
                            sbQuery.Append(" 		,0 ScheduleOrder                    \n");
                        }
                        else
                        {
                            sbQuery.Append(" 		,(SELECT                                            \n");
                            sbQuery.Append("           ISNULL(MAX(ScheduleOrder),0)+1 AS ScheduleOrder  \n");// 옵션광고(OAP등)의 순서는 MAX+1
                            sbQuery.Append("         FROM SchChoiceChannelDetail           \n");
                            sbQuery.Append("         WHERE MediaCode   =   A.MediaCode     \n");
                            sbQuery.Append("         AND   ChannelNo   =   A.ChannelNo)    \n");
                        }
                        sbQuery.Append(" 	FROM SchChoiceChannelDetail A               \n");
                        sbQuery.Append(" 	WHERE not exists(                           \n");
                        sbQuery.Append(" 		SELECT 1 FROM SchChoiceChannelDetail B  \n");
                        sbQuery.Append(" 			WHERE B.MediaCode=A.MediaCode       \n");
                        sbQuery.Append(" 			AND B.ChannelNo=A.ChannelNo         \n");
                        sbQuery.Append(" 			AND B.ItemNo = @ItemNo)             \n");
                        sbQuery.Append("   AND A.ItemNo = @ItemNoCopy                   \n");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        _log.Debug("-----------------------------------------");
                        _log.Debug("상세채널편성 INSERT END");
                        _log.Debug("-----------------------------------------");
                        // __DEBUG__

                        // 쿼리실행
                        ds = new DataSet();
                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    }
                    #endregion
                    #region 상세회차편성 insert

                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT ItemNo FROM SchChoiceSeriesDetail with(noLock)  \n");
                    sbQuery.Append("    WHERE ItemNo =  @ItemNoCopy                           \n");

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // 쿼리실행
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // 결과가 존재하면 상세회차편성테이블에 Insert
                    if (Utility.GetDatasetCount(ds) > 0)
                    {
                        _log.Debug("-----------------------------------------");
                        _log.Debug("상세회차편성 INSERT START");
                        _log.Debug("-----------------------------------------");

                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n");
                        sbQuery.Append(" INSERT INTO SchChoiceSeriesDetail(             \n");
                        sbQuery.Append(" 	    ItemNo                                  \n");
                        sbQuery.Append(" 	    ,MediaCode                              \n");
                        sbQuery.Append(" 	    ,ChannelNo                              \n");
                        sbQuery.Append("        ,SeriesNo                               \n");
                        sbQuery.Append(" 	    ,AckNo                                  \n");
                        sbQuery.Append(" 	    ,ScheduleOrder                          \n");
                        sbQuery.Append(" 	)                                           \n");
                        sbQuery.Append(" 	SELECT                                      \n");
                        sbQuery.Append(" 		@ItemNo     ItemNo                      \n");
                        sbQuery.Append(" 		,MediaCode                              \n");
                        sbQuery.Append(" 		,ChannelNo                              \n");
                        sbQuery.Append("        ,SeriesNo                               \n");
                        sbQuery.Append(" 		,@AckNo     AckNo                       \n");

                        if (AdType.StartsWith("1")) //10~19사이 필수 광고
                        {
                            sbQuery.Append(" 		,0 ScheduleOrder                    \n");
                        }
                        else
                        {
                            sbQuery.Append(" 		,(SELECT                                            \n");
                            sbQuery.Append("           ISNULL(MAX(ScheduleOrder),0)+1 AS ScheduleOrder  \n");// 옵션광고(OAP등)의 순서는 MAX+1
                            sbQuery.Append("         FROM SchChoiceSeriesDetail         \n");
                            sbQuery.Append("         WHERE MediaCode   =   A.MediaCode  \n");
                            sbQuery.Append("         AND   SeriesNo    =   A.SeriesNo   \n");
                            sbQuery.Append("         AND   ChannelNo   =   A.ChannelNo) \n");
                        }
                        sbQuery.Append(" 	FROM SchChoiceSeriesDetail A                \n");
                        sbQuery.Append(" 	WHERE not exists(                           \n");
                        sbQuery.Append(" 		SELECT 1 FROM SchChoiceSeriesDetail B   \n");
                        sbQuery.Append(" 			WHERE   B.MediaCode=A.MediaCode     \n");
                        sbQuery.Append(" 			AND     B.ChannelNo=A.ChannelNo     \n");
                        sbQuery.Append(" 			AND     B.SeriesNo =A.SeriesNo      \n");
                        sbQuery.Append(" 			AND     B.ItemNo   =@ItemNo)        \n");
                        sbQuery.Append("   AND A.ItemNo = @ItemNoCopy                   \n");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        _log.Debug("-----------------------------------------");
                        _log.Debug("상세회차편성 INSERT END");
                        _log.Debug("-----------------------------------------");
                        // __DEBUG__

                        // 쿼리실행
                        ds = new DataSet();
                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    }
                    #endregion
                    #region 지정편성 insert

                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT ItemNo FROM SchDesignatedDetail with(noLock)    \n");
                    sbQuery.Append("    WHERE ItemNo =  @ItemNoCopy                           \n");

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // 쿼리실행
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // 결과가 존재하면 상세회차편성테이블에 Insert
                    if (Utility.GetDatasetCount(ds) > 0)
                    {

                        _log.Debug("-----------------------------------------");
                        _log.Debug("상세지정편성 INSERT START");
                        _log.Debug("-----------------------------------------");

                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n");
                        sbQuery.Append(" INSERT INTO SchDesignatedDetail(              \n");
                        sbQuery.Append(" 	    ItemNo                                 \n");
                        sbQuery.Append(" 	    ,MediaCode                             \n");
                        sbQuery.Append("       ,Category                               \n");
                        sbQuery.Append("       ,Genre                                  \n");
                        sbQuery.Append("       ,Channel                                \n");
                        sbQuery.Append("       ,Series                                 \n");
                        sbQuery.Append("       ,AckNo                                  \n");
                        sbQuery.Append(" 	)                                          \n");
                        sbQuery.Append(" 	SELECT                                     \n");
                        sbQuery.Append(" 		@ItemNo     ItemNo                     \n");
                        sbQuery.Append(" 		,MediaCode                             \n");
                        sbQuery.Append("       ,Category                               \n");
                        sbQuery.Append("       ,Genre                                  \n");
                        sbQuery.Append("       ,Channel                                \n");
                        sbQuery.Append("       ,Series                                 \n");
                        sbQuery.Append(" 		,@AckNo     AckNo                      \n");
                        sbQuery.Append(" 	FROM SchDesignatedDetail A                 \n");
                        sbQuery.Append(" 	WHERE not exists(                          \n");
                        sbQuery.Append(" 		SELECT 1 FROM SchDesignatedDetail B    \n");
                        sbQuery.Append(" 			WHERE   B.MediaCode=A.MediaCode    \n");
                        sbQuery.Append(" 			AND     B.Category =A.Category     \n");
                        sbQuery.Append(" 			AND     B.Genre    =A.Genre        \n");
                        sbQuery.Append(" 			AND     B.Channel  =A.Channel      \n");
                        sbQuery.Append(" 			AND     B.Series   =A.Series       \n");
                        sbQuery.Append(" 			AND     B.ItemNo   =@ItemNo)       \n");
                        sbQuery.Append("   AND A.ItemNo = @ItemNoCopy                  \n");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        _log.Debug("-----------------------------------------");
                        _log.Debug("상세지정편성 INSERT END");
                        _log.Debug("-----------------------------------------");
                        // __DEBUG__

                        // 쿼리실행
                        ds = new DataSet();
                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    schChoiceAdModel.ResultCD = "3101";
                    schChoiceAdModel.ResultDesc = "편성 등록중 오류가 발생하였습니다.";
                    _log.Exception(ex);
                    throw ex;
                }

                _db.CommitTran();   //트랜잭션 실행
                schChoiceAdModel.ResultCD = "0000";  // 정상
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceAdCopy() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran(); //exception이 발생한 경우 롤백
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }
        #endregion

        #region 기편성내역 조회 [E_02]
        /// <summary>
        /// 선택한 광고의 편성내역이 존재하는지 확인
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void CheckSchChoice(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".CheckSchChoice() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                // __DEBUG__				

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter sqlParamItemNo = new SqlParameter("@ItemNo", SqlDbType.Int);

                sqlParamItemNo.Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                DataSet ds = new DataSet();

                #region 메뉴 편성 조회

                sbQuery.Append("\n SELECT ItemNo FROM SchChoiceMenuDetail with(noLock)  \n");
                sbQuery.Append("    WHERE ItemNo =  @ItemNo                             \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
                // 쿼리실행
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParamItemNo);

                // 결과 0건이상이면 이미 편성된 광고

                if (Utility.GetDatasetCount(ds) > 0)
                {
                    schChoiceAdModel.CheckMenu = true;
                }
                #endregion

                #region 채널 편성 조회
                sbQuery = new StringBuilder();

                sbQuery.Append("\n SELECT ItemNo FROM SchChoiceChannelDetail with(noLock)  \n");
                sbQuery.Append("    WHERE ItemNo =  @ItemNo                                \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParamItemNo);

                // 결과 0건이상이면 이미 편성된 광고

                if (Utility.GetDatasetCount(ds) > 0)
                {
                    schChoiceAdModel.CheckChannel = true;
                }
                #endregion

                #region 시리즈 편성 조회

                sbQuery = new StringBuilder();

                sbQuery.Append("\n SELECT ItemNo FROM SchChoiceSeriesDetail with(noLock)  \n");
                sbQuery.Append("    WHERE ItemNo =  @ItemNo                              \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParamItemNo);

                // 결과 0건이상이면 이미 편성된 광고

                if (Utility.GetDatasetCount(ds) > 0)
                {
                    schChoiceAdModel.CheckSeries = true;
                }
                #endregion

                #region 지정 편성 조회

                sbQuery = new StringBuilder();

                sbQuery.Append("\n SELECT ItemNo FROM SchDesignatedDetail with(noLock)  \n");
                sbQuery.Append("    WHERE ItemNo =  @ItemNo                             \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParamItemNo);

                // 결과 0건이상이면 이미 편성된 광고

                if (Utility.GetDatasetCount(ds) > 0)
                {
                    schChoiceAdModel.CheckDetail = true;
                }
                #endregion
                _log.Debug("메뉴편성 " + schChoiceAdModel.CheckMenu + "\n");
                _log.Debug("채널편성 " + schChoiceAdModel.CheckChannel + "\n");
                _log.Debug("시리즈편성 " + schChoiceAdModel.CheckSeries + "\n");
                _log.Debug("지정편성 " + schChoiceAdModel.CheckDetail + "\n");

                schChoiceAdModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".CheckSchChoice() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "광고 편성을 불러오는중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }
        #endregion [E_02]
    }
}