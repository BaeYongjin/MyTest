/*
 * -------------------------------------------------------
 * Class Name: PreferenceTotalizeBiz
 * 주요기능  : 선호도 조사 팝업 집계 처리 로직
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.09.05
 * 수정부분  :
 *            - AdExpCount(), PopExpCount() 추가
 * 수정내용  : 
 *            - 광고노출수, 팝업노출수 계산 추가
 * ---------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : H.J.LEE
 * 수정일    : 2014.08.19
 * 수정부분  :
 *			  - 생성자
 *            - 모든 쿼리
 * 수정내용  : 
 *            - DB 이중화 작업으로 HanaTV , Summary로 분리됨
 *            - Summary가 아닌 HanaTV를 참조하는 모든 테이블,
 *              프로시저 등을 AdTargetsHanaTV.dbo.XX로 수정
 * --------------------------------------------------------
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Base;
using WinFramework.Misc;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerWebService.ReportSummaryAd
{
    /// <summary>
    /// PreferenceTotalizeBiz에 대한 요약 설명입니다.
    /// </summary>
    public class PreferenceTotalizeBiz : BaseBiz
    {
        public PreferenceTotalizeBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// 광고목록 리스트,확인
        /// </summary>
        /// <param name="header"></param>
        /// <param name="model"></param>
        public void GetAdList(HeaderModel header, PreferenceTotalizeModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdList() Start");
                _log.Debug("-----------------------------------------");

                _log.Debug("<입력정보>");
                _log.Debug("AdName   :[" + model.KeySearch.Trim() + "]");

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n" + " SELECT B.ItemNo                                                                ");
                sbQuery.Append("\n" + "       ,C.CodeName AS AdType                                                    ");
                sbQuery.Append("\n" + "       ,B.ItemName AS AdName                                                    ");
                sbQuery.Append("\n" + "       ,A.notice_id AS PopCode                                                  ");
                sbQuery.Append("\n" + "       ,A.notice_title AS PopName                                               ");
                sbQuery.Append("\n" + "       ,CONVERT(varchar(10), A.notice_start_date,111) AS StartDt                ");
                sbQuery.Append("\n" + "       ,CONVERT(varchar(10), A.notice_end_date,111) AS EndDt                    ");
                sbQuery.Append("\n" + "       ,CASE A.State WHEN 'Y' THEN '편성' WHEN 'N' THEN '대기' END AS PopState  ");
                sbQuery.Append("\n" + "       ,A.advt_exm_no                                                           ");
                sbQuery.Append("\n" + "  FROM AdTargetsHanaTV.dbo.vPNS_AdPopup A with(noLock)                                              ");
                sbQuery.Append("\n" + "  LEFT OUTER JOIN AdTargetsHanaTV.dbo.ContractItem B WITH(NoLock) ON A.Adpop_id = B.ItemNo          ");
                sbQuery.Append("\n" + "  LEFT JOIN AdTargetsHanaTV.dbo.SystemCode C with(noLock) ON B.AdType = C.Code AND C.Section ='26'  ");
                sbQuery.Append("\n" + " WHERE 1=1                                                                      ");
                sbQuery.Append("\n" + "   AND A.del_yn = 'N'                                                           ");
                sbQuery.Append("\n" + "   AND A.advt_exm_no > 0                                                        ");
                if (model.KeySearch.Trim().Length > 0)
                {
                    sbQuery.Append("\n" + "   AND B.ItemName LIKE '%" + model.KeySearch.Trim() + "%'                   ");
                }
                _log.Debug(sbQuery.ToString());

                _db.Open();

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                model.PreferenceDataSet = ds.Copy();
                model.ResultCnt = Utility.GetDatasetCount(model.PreferenceDataSet);
                model.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "광고목록 리스트 조회중 오류가 발생하였습니다.";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }


        /// <summary>
        /// 선호도 상세정보 가져오기
        /// 결과는 여러건의 필드와 1개의 DataSet으로 구성됨
        /// </summary>
        /// <param name="header"></param>
        /// <param name="model"></param>
        public void getPopupList(HeaderModel header, PreferenceTotalizeModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "getPopupList() Start   ");
                _log.Debug("-----------------------------------------");
                _log.Debug("<입력정보>");
                _log.Debug("광고번호       :[" + model.KeyItemNo.Trim()   + "]");
                _log.Debug("팝업번호       :[" + model.KeyNoticeId.Trim() + "]");
                _log.Debug("이벤트번호     :[" + model.KeyExmNo.Trim()    + "]");
                _log.Debug("시작일         :[" + model.KeyStartDay.Trim() + "]");
                _log.Debug("종료일         :[" + model.KeyEndDay.Trim()   + "]");
                _log.Debug("-----------------------------------------");

                StringBuilder sb = new StringBuilder();
                DataSet ds = new DataSet();
                // 선호도타입
                // 01 : 숫자형, 02:바형
                string exm_type = "";

                _db.Open();
                _db.BeginTran();

                #region [1] 팝업정보가져오기
                try
                {
                    sb.Append("\n" + "-- 1. 팝업이벤트 정보                                                                           ");
                    sb.Append("\n" + "select	a.Advt_Exm_Title                                                                      ");
                    sb.Append("\n" + "  ,       case a.Advt_Exm_Typ when '01' then '숫자형' when '02' then '바형' end AS Advt_Exm_Nm  ");
                    sb.Append("\n" + "	,	    a.Exm_Cnts                                                                            ");
                    sb.Append("\n" + "	,	    a.Advt_Exm_Typ                                                                        ");
                    sb.Append("\n" + "from      AdTargetsHanaTV.dbo.vPNS_Preference a                                                                     ");
                    sb.Append("\n" + "inner join AdTargetsHanaTV.dbo.vPNS_AdPopup b on b.advt_exm_no = a.advt_exm_no                                      ");
                    sb.Append("\n" + "where	    b.adpop_id = '" + model.KeyItemNo + "'                                                ");
                    sb.Append("\n" + "and       b.notice_id = '" + model.KeyNoticeId + "'                                             ");

                    _log.Debug(sb.ToString());

                    // 쿼리 실행
                    _db.ExecuteQuery(ds, sb.ToString());

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        model.EventName = ds.Tables[0].Rows[0]["Exm_Cnts"].ToString();
                        model.PopExpType = ds.Tables[0].Rows[0]["Advt_Exm_Nm"].ToString();
                        exm_type = ds.Tables[0].Rows[0]["Advt_Exm_Typ"].ToString();
                    }
                    else
                    {
                        model.EventName = "";
                        model.PopExpType = "";
                        exm_type = "01";
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("선호도 정보 조회중 오류가 발생하였습니다. " + ex.Message);
                }
                finally
                {
                    if (null != sb) sb = null;
                    if (null != ds) ds = null;
                }
                #endregion

                #region [2] 광고노출수 구하기
                try
                {
                    sb = new StringBuilder();
                    sb.Append(" -- 2. 광고노출수 구하기                             \n");
                    sb.Append(" SELECT ISNULL(SUM(AdCnt), 0) AS AdExpCount          \n");
                    sb.Append("   FROM SummaryAdDaily0 WITH(NoLock)                 \n");
                    sb.Append("  WHERE LogDay between '" + model.KeyStartDay + "' AND '" + model.KeyEndDay + "'  \n");
                    sb.Append("    AND ItemNo = " + model.KeyItemNo + "             \n");
                    sb.Append("    AND SummaryType = 1                              \n");

                    _log.Debug(sb.ToString());

                    // 쿼리 실행
                    ds = new DataSet();
                    _db.ExecuteQuery(ds, sb.ToString());

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        model.AdExpCount = Convert.ToInt32(ds.Tables[0].Rows[0]["AdExpCount"].ToString());
                    }
                    else
                    {
                        model.AdExpCount = 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("선호도용 광고노출수 조회중 오류가 발생하였습니다. " + ex.Message);
                }
                finally
                {
                    if (null != sb) sb = null;
                    if (null != ds) ds = null;
                }
                #endregion

                #region [3] 팝업노출수 구하기
                try
                {
                    sb = new StringBuilder();
                    sb.Append(" -- 3. 팝업노출수 구하기 \n");
                    sb.Append(" SELECT isnull(sum(H00+H01+H02+H03+H04+H05+H06+H07+H08+H09+H10+H11+H12+H13+H14+H15+H16+H17+H18+H19+H20+H21+H22+H23),0) AS PopExpCount \n");
                    sb.Append("   FROM PNS.Adinteractive.dbo.SummeryPopDaily  WITH(NoLock)                        \n");
                    sb.Append("  WHERE LogDay between '" + model.KeyStartDay + "' AND '" + model.KeyEndDay + "'   \n");
                    sb.Append("    AND PopupID = '" + model.KeyNoticeId + "'                                      \n");
                    _log.Debug(sb.ToString());

                    // 쿼리 실행
                    ds = new DataSet();
                    _db.ExecuteQuery(ds, sb.ToString());

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        model.PopExpCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PopExpCount"].ToString());
                    }
                    else
                    {
                        model.PopExpCount = 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("선호도 팝업노출수 조회중 오류가 발생하였습니다. \n" + ex.Message);
                }
                finally
                {
                    if (null != sb) sb = null;
                    if (null != ds) ds = null;
                }
                #endregion

                #region [4] 선호도항목별 데이터 리스트 구하기
                try
                {
                    sb = new StringBuilder();

                    sb.Append("\n" + " -- 4. 선호도항목별 데이터 리스트구하기 ");
                    sb.Append("\n" + "select	j.rowNum	as Num_Cnt ");
                    sb.Append("\n" + "	,	j.Exm_Cnts ");
                    sb.Append("\n" + "	,	isnull(k.PrefCnt,0) as PrefCnt ");
                    sb.Append("\n" + "	,	isnull(k.PrefRate,0) as PrefRate ");
                    sb.Append("\n" + "from (  -- 유효한 항목정보를 가져옴 ");
                    sb.Append("\n" + "		select	t.rowNum ");
                    sb.Append("\n" + "	,	case advt_Exm_Typ ");
                    sb.Append("\n" + "			when '01' then ");
                    sb.Append("\n" + "			    case t.rowNum  ");
                    sb.Append("\n" + "				    when 1 then v.Num1_cnts  ");
                    sb.Append("\n" + "					when 2 then v.Num2_cnts  ");
                    sb.Append("\n" + "					when 3 then v.Num3_cnts  ");
                    sb.Append("\n" + "					when 4 then v.Num4_cnts  ");
                    sb.Append("\n" + "					when 5 then v.Num5_cnts  ");
                    sb.Append("\n" + "			    end	 ");
                    sb.Append("\n" + "			when  '02'  then ");
                    sb.Append("\n" + "		        case t.rowNum ");
                    sb.Append("\n" + "				    when 1 then '00점' ");
                    sb.Append("\n" + "					when 2 then '10점' ");
                    sb.Append("\n" + "					when 3 then '20점' ");
                    sb.Append("\n" + "					when 4 then '30점' ");
                    sb.Append("\n" + "					when 5 then '40점' ");
                    sb.Append("\n" + "					when 6 then '50점' ");
                    sb.Append("\n" + "					when 7 then '60점' ");
                    sb.Append("\n" + "					when 8 then '70점' ");
                    sb.Append("\n" + "					when 9 then '80점' ");
                    sb.Append("\n" + "					when 10 then '90점' ");
                    sb.Append("\n" + "					when 11 then '100점' ");
                    sb.Append("\n" + "			    end				 ");
                    sb.Append("\n" + "			end as Exm_Cnts  ");
                    sb.Append("\n" + "		from ( 	select	num_cnt ");
                    sb.Append("\n" + "					,	Num1_Cnts,	Num2_Cnts,	Num3_Cnts,	Num4_Cnts,	Num5_Cnts, advt_Exm_Typ ");
                    sb.Append("\n" + "				from	AdTargetsHanaTV.dbo.vPNS_Preference a ");
                    sb.Append("\n" + "				inner join AdTargetsHanaTV.dbo.vPNS_AdPopup b on b.advt_exm_no = a.advt_exm_no ");
                    sb.Append("\n" + "				where	b.adpop_id = '" + model.KeyItemNo + "'");
                    sb.Append("\n" + "				and		b.notice_id = '" + model.KeyNoticeId + "' ) v ");
                    sb.Append("\n" + "		cross join AdTargetsHanaTV.dbo.copy_t t ");
                    sb.Append("\n" + "		where t.rownum <= 	case advt_Exm_Typ when '01' then v.num_cnt when '02' then 11 end ) j ");
                    sb.Append("\n" + "left outer join  ");
                    sb.Append("\n" + "	(	-- 실제로 참여한 데이터 ");
                    // 바형은 0점부터 있기 때문에 해당번호+1을 해야 위에 항목정보와 정합됨
                    sb.Append("\n" + "		select	case '" +  exm_type + "' when '01' then t2.Preference when '02' then t2.Preference + 1 end as Preference ");
                    sb.Append("\n" + "			,	t2.PrefCnt ");
                    sb.Append("\n" + "			,	cast(t2.PrefCnt as float) / (PrefTot + 1) * 100	as PrefRate ");
                    sb.Append("\n" + "		from ( 	select sum(PrefCnt) over() as PrefTot ");
                    sb.Append("\n" + "					,	t1.Preference ");
                    sb.Append("\n" + "					,	t1.PrefCnt ");
                    sb.Append("\n" + "				from ( 	select	b.Preference,count(*) as PrefCnt ");
                    sb.Append("\n" + "						from	LogAdEventdata b with(noLock) ");
                    sb.Append("\n" + "						where	b.ItemNo = '" + model.KeyItemNo + "'");
                    sb.Append("\n" + "						and		b.NotyId = '" + model.KeyNoticeId + "'");
                    sb.Append("\n" + "						group by Preference ) t1 ) t2 ) k ");
                    sb.Append("\n" + "			on j.rowNum = k.Preference ");

                    _log.Debug(sb.ToString());

                    // 쿼리 실행
                    ds = new DataSet();
                    _db.ExecuteQuery(ds, sb.ToString());

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        
                        int totPrefCnt = 0;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            totPrefCnt += Convert.ToInt32(row["PrefCnt"].ToString());
                        }
                        model.RepCount = totPrefCnt;
                        model.RepRate  = (float)totPrefCnt / model.PopExpCount * 100;

                        model.PreferenceDataSet = ds.Copy();
                        model.ResultCnt = Utility.GetDatasetCount(model.PreferenceDataSet);
                        model.ResultCD = "0000";
                    }
                    else
                    {
                        model.PreferenceDataSet = null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("선호도 참여내용 목록 조회중 오류가 발생하였습니다. \n" + ex.Message);
                }
                finally
                {
                    if (null != sb) sb = null;
                    if (null != ds) ds = null;
                }
                #endregion
                

                _db.CommitTran();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "getPopupList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();
                model.ResultCD = "3000";
                model.ResultDesc = ex.Message;
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
    }
}