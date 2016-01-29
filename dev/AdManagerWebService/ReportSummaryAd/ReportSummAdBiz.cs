// ===============================================================================
//
// ReportSummAdBiz.cs
//
// 통합리포팅 서비스 
//
// ===============================================================================
// Release history
// 2007.10.26 BJ.PARK OAP도 집계가능토록 함 집계공통이용 메소드 => GetContractItemList()
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: ReportSummAdBiz
 * 주요기능  : 통합리포팅 처리 로직
 * 작성자    : 모름
 * 작성일    : 모름
 * 특이사항  : 없음
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
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
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Base;
using WinFramework.Misc;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerWebService.ReportSummaryAd
{
    public class ReportSummAdBiz : BaseBiz
    {
        public ReportSummAdBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// 통합리포팅 가져오기
        /// </summary>
        /// <param name="header"></param>
        /// <param name="model"></param>
        public void GetList_Combine(HeaderModel header, AdManagerModel.ReportAd.RptAdBaseModel model)
        {
            DataSet sqlDs = new DataSet();
            SqlParameter[] sqlParam = new SqlParameter[4];

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetList_Combine() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("<조회조건>");
                _log.Debug(string.Format("계약      : [{0}]", model.SearchContractSeq));
                _log.Debug(string.Format("캠페인    : [{0}]", model.CampaignCode));
                _log.Debug(string.Format("광고내역  : [{0}]", model.SearchItemNo));
                _log.Debug(string.Format("일자      : [{0} ~ {1}]", model.SearchBgnDay , model.SearchEndDay));
                
                sqlParam[0] = new SqlParameter("@BeginDay", SqlDbType.Char, 6);
                sqlParam[1] = new SqlParameter("@EndDay", SqlDbType.Char, 6);
                sqlParam[2] = new SqlParameter("@ContractSeq", SqlDbType.Int, 4);
                sqlParam[3] = new SqlParameter("@CampaignCd", SqlDbType.Int, 4);
                
                sqlParam[0].Value = model.SearchBgnDay;
                sqlParam[1].Value = model.SearchEndDay;
                sqlParam[2].Value = Convert.ToInt32(model.SearchContractSeq);
                sqlParam[3].Value = Convert.ToInt32(model.CampaignCode);

                _db.Timeout = 60 * 10;

                _db.Open();
                _db.ExecuteProcedureParams(sqlDs, "dao_CombineReport", sqlParam);

                model.ReportDataSet = sqlDs.Copy();
                model.ResultCD = "0000";
                model.ResultCnt = Utility.GetDatasetCount(model.ReportDataSet);
                model.ResultDesc = "리포팅조회 완료";
                
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultCnt = 0;
                model.ResultDesc = ex.Message;
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();

                if (sqlDs != null)
                {
                    sqlDs.Clear();
                    sqlDs.Dispose();
                    sqlDs = null;
                }

                _log.Debug("-----------------------------------------");
                _log.Debug(string.Format("ResultCd  : [{0}]", model.ResultCD));
                _log.Debug(string.Format("ResultCnt : [{0}]", model.ResultCnt));
                _log.Debug(string.Format("ResultMsg : [{0}]", model.ResultDesc));
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetList_Combine() End");
                _log.Debug("-----------------------------------------");
            }
        }
    }
}