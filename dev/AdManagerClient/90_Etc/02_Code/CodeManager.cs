// ===============================================================================
//
// CodeManager.cs
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
	public class CodeManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public CodeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/CodeService.asmx";
		}

		/// <summary>
		/// 코드구분목록
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetSectionList(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("코드구분목록 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.SearchSection         = codeModel.SearchSection;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetSectionList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				codeModel.CodeDataSet = remoteData.CodeDataSet.Copy();
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("코드구분목록 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSectionList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 코드목록
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetCodeList(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("공통코드조회 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey       = codeModel.SearchKey;
				remoteData.Section         = codeModel.Section;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetCodeList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				codeModel.Code = remoteData.Code;
				codeModel.CodeDataSet = remoteData.CodeDataSet.Copy();
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("공통코드조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCodeList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 코드구분정보 수정
		/// </summary>
		/// <param name="codeModel"></param>
		public void SetSectionUpdate(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("코드구분정보수정 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.Section       = codeModel.Section;
				remoteData.CodeName       = codeModel.CodeName;
				remoteData.Section_old     = codeModel.Section_old;				
											
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetSectionUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("코드구분정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSectionUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 코드정보 수정
		/// </summary>
		/// <param name="codeModel"></param>
		public void SetCodeUpdate(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("코드정보수정 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.Code       = codeModel.Code;
				remoteData.CodeName       = codeModel.CodeName;
				remoteData.Section       = codeModel.Section;
				remoteData.Code_old     = codeModel.Code_old;				
											
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetCodeUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("코드정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCodeUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 코드정보 등록
		/// </summary>
		/// <param name="codeModel"></param>
		public void SetCodeCreate(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("코드정보등록 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.Section       = codeModel.Section;
				remoteData.Code       = codeModel.Code;
				remoteData.CodeName       = codeModel.CodeName;				
															
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetCodeCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("코드정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCodeCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 코드정보 삭제
		/// </summary>
		/// <param name="codeModel"></param>
		public void SetCodeDelete(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("코드정보삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트				
				remoteData.Section       = codeModel.Section;
				remoteData.Code_old     = codeModel.Code_old;				
											
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetCodeDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("코드정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCodeDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
