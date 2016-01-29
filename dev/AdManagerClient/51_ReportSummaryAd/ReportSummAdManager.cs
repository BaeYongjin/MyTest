// ===============================================================================
//
// RptSummaryAdDailyManager.cs
//
// 광고일간레포트 조회 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
using System;
using System.Data;
using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;
using AdManagerModel;

namespace AdManagerClient.ReportSummAd
{
	public class ReportSummAdManager : BaseManager
	{
        public ReportSummAdManager(SystemModel systemModel, CommonModel commonModel)
            : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
            _module = "ReportSummAdService";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/ReportSummaryAd/ReportSummAdService.asmx";
		}

        public void GetListCombine( AdManagerModel.ReportAd.RptAdBaseModel model)
		{
			try
			{
                object resultObj = null;

                ReportSummAdProxy.HeaderModel rHeader = new ReportSummAdProxy.HeaderModel();
                ReportSummAdProxy.RptAdBaseModel rData = new ReportSummAdProxy.RptAdBaseModel();
                ReportSummAdProxy.ReportSummAdService svc = new ReportSummAdProxy.ReportSummAdService();

				// 데이터 전달
                rHeader.ClientKey = Header.ClientKey;
                rHeader.UserID = Header.UserID;
                rHeader.UserLevel = Header.UserLevel;

                rData.SearchContractSeq = model.SearchContractSeq;
                rData.CampaignCode = model.CampaignCode;
                rData.SearchBgnDay = model.SearchBgnDay;
                rData.SearchEndDay = model.SearchEndDay;
                                
				// 웹서비스 호출 타임아웃설정
                svc.Url = _WebServiceUrl;
                svc.Timeout = 60 * 3 * 1000;

				// 웹서비스 메소드 호출

                rData = svc.GetList(rHeader, rData);

				// 결과코드검사
                if (!rData.ResultCD.Equals("0000"))
				{
                    throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);
				}

                model.ReportDataSet = rData.ReportDataSet.Copy();
                model.ResultCnt = rData.ResultCnt;
                model.ResultCD = rData.ResultCD;

				// 결과 셋트
                _log.Debug("-----------------------------------------");
				_log.Debug("광고일간레포트목록 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetRptSummaryAdDailyList():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}
	}
}
