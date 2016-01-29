// ===============================================================================
//
// OAPWeekSummaryRptManager.cs
//
// 광고일간레포트 조회 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
//
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

namespace AdManagerClient
{
	/// <summary>
	/// OAPWeekSummaryRptManager에 대한 요약 설명입니다.
	/// </summary>
	public class OAPWeekSummaryRptManager : BaseManager
	{
		public OAPWeekSummaryRptManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyAdExecSummaryRpt";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/OAPWeekSummaryRptService.asmx";
		}

		/// <summary>
		/// OAP주간홈광고 조회
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetOAPWeekHomeAd(OAPWeekSummaryRptModel oAPWeekSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("OAP주간홈광고 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService svc = new AdManagerClient.OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				OAPWeekSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.OAPWeekSummaryRptServicePloxy.HeaderModel();
				OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptModel remoteData = new AdManagerClient.OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.LogDay1 = oAPWeekSummaryRptModel.LogDay1;
				remoteData.LogDay2 = oAPWeekSummaryRptModel.LogDay2;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// 웹서비스 메소드 호출
				remoteData = svc.GetOAPWeekHomeAd(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				oAPWeekSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				oAPWeekSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				oAPWeekSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				oAPWeekSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("OAP주간홈광고 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetOAPWeekHomeAd():" + fe.ErrCode + ":" + fe.ResultMsg);
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

		/// <summary>
		/// OAP주간채널점핑 조회
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetOAPWeekChannelJump(OAPWeekSummaryRptModel oAPWeekSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("OAP주간채널점핑 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService svc = new AdManagerClient.OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				OAPWeekSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.OAPWeekSummaryRptServicePloxy.HeaderModel();
				OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptModel remoteData = new AdManagerClient.OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.LogDay1 = oAPWeekSummaryRptModel.LogDay1;
				remoteData.LogDay2 = oAPWeekSummaryRptModel.LogDay2;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// 웹서비스 메소드 호출
				remoteData = svc.GetOAPWeekChannelJump(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				oAPWeekSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				oAPWeekSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				oAPWeekSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				oAPWeekSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("OAP주간채널점핑 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetOAPWeekChannelJump():" + fe.ErrCode + ":" + fe.ResultMsg);
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


        public DataSet	mngGetHomeCmReport(string Day1, string Day2, string mediaRep)
        {
            DataSet rtnDs = new DataSet();

            try
            {
                OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService svc = new AdManagerClient.OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService();
                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;  

                // 웹서비스 메소드 호출
                rtnDs = svc.mGetHomeCmReport(Day1, Day2, mediaRep);

                if( rtnDs == null )
                {
                    throw new FrameException("홈상업광고 조회 오류!!!", _module, "0001");
                }
                _log.Debug("-----------------------------------------");
                _log.Debug("홈상업광고 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":홈상업광고:" + fe.ErrCode + ":" + fe.ResultMsg);
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
            return rtnDs;
        }

	}
}
