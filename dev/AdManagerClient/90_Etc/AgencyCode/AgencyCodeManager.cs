// ===============================================================================
//
// AgencyCodeManager.cs
//
// 공통코드조회 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: AgencyCodeManager
 * 주요기능  : 그룹정보관리 서비스 호출
 * 작성자    : 모름
 * 작성일    : 모름
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.02
 * 수정내용  :        
 *            - 검색어 조회 기능 추가
 * 수정함수  :
 *            - GetAgencyCodeList
 * --------------------------------------------------------
 */

using System;
using System.Data;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// 사용자정보 웹서비스를 호출합니다. 
	/// </summary>
	public class AgencyCodeManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AgencyCodeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/AgencyCodeService.asmx";
		}

		/// <summary>
		/// 사용자 삭제
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetAgencyCodeList(AgencyCodeModel agencycodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("공통코드조회 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				AgencyCodeServicePloxy.AgencyCodeService svc = new AgencyCodeServicePloxy.AgencyCodeService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AgencyCodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AgencyCodeServicePloxy.HeaderModel();
				AgencyCodeServicePloxy.AgencyCodeModel     remoteData   = new AdManagerClient.AgencyCodeServicePloxy.AgencyCodeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchRap         = agencycodeModel.SearchRap;
				remoteData.AgencyCode        = agencycodeModel.AgencyCode;
                remoteData.SearchKey         = agencycodeModel.SearchKey;  // [E_01]
				 
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetAgencyCodeList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				agencycodeModel.AgencyCodeDataSet = remoteData.AgencyCodeDataSet.Copy();
				agencycodeModel.ResultCnt   = remoteData.ResultCnt;
				agencycodeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("공통코드조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				//_log.Warning( this.ToString() + ":GetAgencyCodeList():" + fe.ErrAgencyCode + ":" + fe.ResultMsg);
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
