// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// 채널정보 저장 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
// 2007.06.26 송명환 v1.0
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
	/// 채널정보 웹서비스를 호출합니다. 
	/// </summary>
	public class GradeManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public GradeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Target/GradeService.asmx";
		}

		/// <summary>
		/// 미디어콤보정보조회
		/// </summary>
		/// <param name="gradeModel"></param>
		public void GetGradeCodeList(GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("카테고리콤보 목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;
								
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetGradeCodeList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				gradeModel.GradeDataSet = remoteData.GradeDataSet.Copy();
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("카테고리콤보 목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 채널정보조회
		/// </summary>
		/// <param name="gradeModel"></param>
		public void GetGradeList(GradeModel gradeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.MediaCode       = gradeModel.MediaCode;
								
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetGradeList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				gradeModel.GradeDataSet = remoteData.GradeDataSet.Copy();
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelSetList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 채널 디테일 목록조회
		/// </summary>
		/// <param name="gradeModel"></param>
		public void GetContractItemList(GradeModel gradeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널 디테일 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey             = gradeModel.SearchKey;
				remoteData.MediaCode             = gradeModel.MediaCode;
				remoteData.Code					 = gradeModel.Code;        				
				remoteData.SearchchkAdState_10	 = gradeModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 = gradeModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 = gradeModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 = gradeModel.SearchchkAdState_40; 
				
				//remoteData.SearchChkSch_YN		 = contractItemModel.SearchChkSch_YN; 
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetContractItemList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				gradeModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널 디테일 목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelSetList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 채널셋 수정
		/// </summary>
		/// <param name="gradeModel"></param>
		public void SetGradeUpdate(GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("등급정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사
				if(gradeModel.CodeName.Length > 50) 
				{
					throw new FrameException("코드명은 50Bytes를 초과할 수 없습니다.");
				}				

				// 웹서비스 인스턴스 생성
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.Code       = gradeModel.Code;
				remoteData.CodeName     = gradeModel.CodeName;				
				remoteData.Grade       = gradeModel.Grade;
								
				remoteData.Code_O       = gradeModel.Code_O;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGradeUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널셋정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGradeUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고매체대행광고주추가
		/// </summary>
		/// <param name="gradeModel"></param>
		/// <returns></returns>
		public void SetGradeCreate(GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널셋추가 Start");
				_log.Debug("-----------------------------------------");
				
				if(gradeModel.CodeName.Length > 50) 
				{
					throw new FrameException("채널No는 50Bytes를 초과할 수 없습니다.");
				}			
				
				// 웹서비스 인스턴스 생성
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.Code       = gradeModel.Code;
				remoteData.CodeName     = gradeModel.CodeName;				
				remoteData.Grade       = gradeModel.Grade;	
								
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGradeCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널셋추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				gradeModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGradeCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				gradeModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		
		/// <summary>
		/// 채널셋 삭제
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetGradeDelete(GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널셋삭제 start");
				_log.Debug("-----------------------------------------");
            
				// 웹서비스 인스턴스 생성
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
            				
				// 리모트 모델 생성
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();
            
				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;
            
				// 호출정보셋트
				remoteData.Code       = gradeModel.Code;	
            					
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGradeDelete(remoteHeader, remoteData);
            
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// 결과 셋트
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("채널셋삭제 end");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGradeDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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