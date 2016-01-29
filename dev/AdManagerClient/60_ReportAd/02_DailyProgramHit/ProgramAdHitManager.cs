// ===============================================================================
//
// ProgramAdHitManager.cs
//
// 프로그램별 시청횟수집계 조회 서비스를 호출합니다. 
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
	/// 프로그램별 시청횟수집계 조회 웹서비스를 호출합니다. 
	/// </summary>
	public class ProgramAdHitManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public ProgramAdHitManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "ProgramAdHit";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/ProgramAdHitService.asmx";
		}

		/// <summary>
		/// 프로그램별 광고시청집계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetProgramAdHitReport(ProgramAdHitModel programAdHitModel)
		{
			try
			{
				_log.Debug("---------------------------------------");
				_log.Debug("프로그램별 광고시청집계 조회 Start"     );
				_log.Debug("---------------------------------------");
				
				ProgramAdHitServicePloxy.ProgramAdHitService svc = new ProgramAdHitServicePloxy.ProgramAdHitService();
				svc.Url = _WebServiceUrl;

				ProgramAdHitServicePloxy.HeaderModel        remoteHeader = new AdManagerClient.ProgramAdHitServicePloxy.HeaderModel();
				ProgramAdHitServicePloxy.ProgramAdHitModel  remoteData   = new AdManagerClient.ProgramAdHitServicePloxy.ProgramAdHitModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode		 = programAdHitModel.SearchMediaCode;	  
				remoteData.SearchContractSeq	 = programAdHitModel.SearchContractSeq;	  
				remoteData.SearchItemNo  		 = programAdHitModel.SearchItemNo;	  
				remoteData.CampaignCode  		 = programAdHitModel.CampaignCode;	  
				remoteData.SearchType 			 = programAdHitModel.SearchType;       
				remoteData.SearchBgnDay 		 = programAdHitModel.SearchBgnDay;       
				remoteData.SearchEndDay 		 = programAdHitModel.SearchEndDay;       
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...
				// 웹서비스 메소드 호출
				remoteData = svc.GetProgramAdHit(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				programAdHitModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				programAdHitModel.ResultCnt     = remoteData.ResultCnt;
				programAdHitModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("프로그램별 시청횟수집계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetProgramAdHit():" + fe.ErrCode + ":" + fe.ResultMsg);
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
