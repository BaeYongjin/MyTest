using System;
using System.Data;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;
using System.Collections;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// 광고리포트 일별통계 조회 웹서비스를 호출합니다. 
	/// </summary>
	public class HouseHoldAccountManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public HouseHoldAccountManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log	= FrameSystem.oLog;
			_module = "RegionGenreService";
			_Host	= FrameSystem.m_WebServer_Host;
			_Port	= FrameSystem.m_WebServer_Port;
			_Path	= FrameSystem.m_WebServer_App + "/ReportAd/SummaryAdService.asmx";
		}

		/// <summary>
		/// 지역별 장르
		/// </summary>
		/// <param name="houseHoldAccountModel"></param>
		public void GetHouseHoldAccount(HouseHoldAccountModel houseHoldAccountModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("지역별 장르통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SummaryAdServicePloxy.SummaryAdService svc = new AdManagerClient.SummaryAdServicePloxy.SummaryAdService();
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				SummaryAdServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.SummaryAdServicePloxy.HeaderModel();
				SummaryAdServicePloxy.SummaryAdModel		remoteData   = new AdManagerClient.SummaryAdServicePloxy.SummaryAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchStartDay     = houseHoldAccountModel.SearchStartDay;
				remoteData.SearchEndDay       = houseHoldAccountModel.SearchEndDay;       
				remoteData.SearchMediaCode    = houseHoldAccountModel.SearchMediaCode;       
				remoteData.SearchContractSeq  = houseHoldAccountModel.SearchContractSeq;       
				remoteData.CampaignCode       = houseHoldAccountModel.CampaignCode;       
				remoteData.SearchItemNo       = houseHoldAccountModel.SearchItemNo;       

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout * 240;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetDateAccountHouseHold(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트								
				houseHoldAccountModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				houseHoldAccountModel.ResultCnt     = remoteData.ResultCnt;
				houseHoldAccountModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("지역별 시간통계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetRegionDate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
