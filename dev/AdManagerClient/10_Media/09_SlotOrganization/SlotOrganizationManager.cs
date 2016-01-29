// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// 사용자정보 저장 서비스를 호출합니다. 
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
	public class SlotOrganizationManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SlotOrganizationManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "SLOTORGANIZATION";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/SlotOrganizationService.asmx";
		}

		/// <summary>
		/// 사용자정보조회
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		public void GetSlotList(SlotOrganizationModel slotOrganizationModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchMediaName       = slotOrganizationModel.SearchMediaName;
				remoteData.SearchCategoryName    = slotOrganizationModel.SearchCategoryName;
				remoteData.SearchGenreName       = slotOrganizationModel.SearchGenreName;

				remoteData.SearchchkUseYn	 = slotOrganizationModel.SearchchkUseYn; 
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetSlotList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				slotOrganizationModel.SlotDataSet = remoteData.SlotDataSet.Copy();
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSlotList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 슬롯코드
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetSlotCodeList(SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("공통코드조회 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel     remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetSlotCodeList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				slotOrganizationModel.SlotCodeDataSet = remoteData.SlotCodeDataSet.Copy();
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("공통코드조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				//_log.Warning( this.ToString() + ":GetSlotCodeList():" + fe.ErrMediaCode + ":" + fe.ResultMsg);
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
		/// 카테고리콤보정보조회
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		public void GetCategoryList(SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("카테고리콤보 목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.CategoryCode = slotOrganizationModel.MediaCode;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetCategoryList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				slotOrganizationModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

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
		/// 장르콤보정보조회
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		public void GetGenreList(SlotOrganizationModel slotOrganizationModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("장르콤보 목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// 헤더정보 셋트				
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.SearchKey = slotOrganizationModel.SearchKey;				
				remoteData.SearchMediaName = slotOrganizationModel.SearchMediaName;
				remoteData.SearchCategoryName = slotOrganizationModel.SearchCategoryName;				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetGenreList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				slotOrganizationModel.GenreDataSet = remoteData.GenreDataSet.Copy();
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("장르콤보 목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreList():" + fe.ErrCode + ":" + fe.ResultMsg);
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

//		/// <summary>
//		/// Service 호출을 위한 메소드
//		/// </summary>
//		public bool GetUserDetail(BaseModel baseModel)
//		{
//			
//			_log.Debug("-----------------------------------------");
//			_log.Debug( this.ToString() + " Start");
//			_log.Debug("-----------------------------------------");
//
//			_log.Debug("-----------------------------------------");
//			_log.Debug( this.ToString() + " End");
//			_log.Debug("-----------------------------------------");
//
//			return true;
//		}
//
		/// <summary>
		/// 사용자정보 수정
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		public void SetSlotUpdate(SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사
//				if(slotOrganizationModel.UserID.Length < 1) 
//				{
//					throw new FrameException("사용자ID가 존재하지 않습니다.");
//				}
//				if(slotOrganizationModel.UserDept.Length > 20) 
//				{
//					throw new FrameException("소속부서명의 길이는 20Bytes를 초과할 수 없습니다.");
//				}
//				if(slotOrganizationModel.UserTitle.Length > 20) 
//				{
//					throw new FrameException("직책직함명의 길이는 20Bytes를 초과할 수 없습니다.");
//				}
//				if(slotOrganizationModel.UserTell.Length > 15) 
//				{
//					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
//				}
//				if(slotOrganizationModel.UserTell.Length > 15) 
//				{
//					throw new FrameException("휴대전화번호의 길이는 15Bytes를 초과할 수 없습니다.");
//				}
//				if(slotOrganizationModel.UserComment.Length > 50) 
//				{
//					throw new FrameException("비고란의 길이는 50Bytes를 초과할 수 없습니다.");
//				}

	

				// 웹서비스 인스턴스 생성
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode    = slotOrganizationModel.MediaCode;
				remoteData.CategoryCode = slotOrganizationModel.CategoryCode;
				remoteData.GenreCode	= slotOrganizationModel.GenreCode;
				remoteData.ChannelNo    = slotOrganizationModel.ChannelNo;
				remoteData.Slot1    = slotOrganizationModel.Slot1;
				remoteData.Slot2    = slotOrganizationModel.Slot2;
				remoteData.Slot3    = slotOrganizationModel.Slot3;
				remoteData.Slot4    = slotOrganizationModel.Slot4;
				remoteData.Slot5    = slotOrganizationModel.Slot5;
				remoteData.Slot6    = slotOrganizationModel.Slot6;
				remoteData.Slot7    = slotOrganizationModel.Slot7;
				remoteData.Slot8    = slotOrganizationModel.Slot8;
				remoteData.Slot9    = slotOrganizationModel.Slot9;
				remoteData.Slot10   = slotOrganizationModel.Slot10;
				remoteData.Slot11   = slotOrganizationModel.Slot11;
				remoteData.Slot12   = slotOrganizationModel.Slot12;
				remoteData.Slot13   = slotOrganizationModel.Slot13;
				remoteData.Slot14   = slotOrganizationModel.Slot14;
				remoteData.Slot15   = slotOrganizationModel.Slot15;
				remoteData.UseYn    = slotOrganizationModel.UseYn;	
			
				remoteData.MediaCode_old    = slotOrganizationModel.MediaCode_old;
				remoteData.CategoryCode_old = slotOrganizationModel.CategoryCode_old;
				remoteData.GenreCode_old	= slotOrganizationModel.GenreCode_old;
				remoteData.ChannelNo_old    = slotOrganizationModel.ChannelNo_old;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetSlotUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSlotUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 사용자추가
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		/// <returns></returns>
		public void SetSlotAdd(SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 Start");
				_log.Debug("-----------------------------------------");

//				if(slotOrganizationModel.UserID.Trim().Length < 1) 
//				{
//					throw new FrameException("사용자ID가 존재하지 않습니다.");
//				}
//				if(slotOrganizationModel.UserID.Trim().Length > 10) 
//				{
//					throw new FrameException("사용자ID는 10Bytes를 초과할 수 없습니다.");
//				}
//				if(slotOrganizationModel.UserDept.Length > 20) 
//				{
//					throw new FrameException("소속부서명의 길이는 20Bytes를 초과할 수 없습니다.");
//				}
//				if(slotOrganizationModel.UserTitle.Length > 20) 
//				{
//					throw new FrameException("직책직함명의 길이는 20Bytes를 초과할 수 없습니다.");
//				}
//				if(slotOrganizationModel.UserTell.Length > 15) 
//				{
//					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
//				}
//				if(slotOrganizationModel.UserTell.Length > 15) 
//				{
//					throw new FrameException("휴대전화번호의 길이는 15Bytes를 초과할 수 없습니다.");
//				}
//				if(slotOrganizationModel.UserComment.Length > 50) 
//				{
//					throw new FrameException("비고란의 길이는 50Bytes를 초과할 수 없습니다.");
//				}


				// 웹서비스 인스턴스 생성
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode    = slotOrganizationModel.MediaCode;
				remoteData.CategoryCode = slotOrganizationModel.CategoryCode;
				remoteData.GenreCode	= slotOrganizationModel.GenreCode;
				remoteData.ChannelNo    = slotOrganizationModel.ChannelNo;
				remoteData.Slot1    = slotOrganizationModel.Slot1;
				remoteData.Slot2    = slotOrganizationModel.Slot2;
				remoteData.Slot3    = slotOrganizationModel.Slot3;
				remoteData.Slot4    = slotOrganizationModel.Slot4;
				remoteData.Slot5    = slotOrganizationModel.Slot5;
				remoteData.Slot6    = slotOrganizationModel.Slot6;
				remoteData.Slot7    = slotOrganizationModel.Slot7;
				remoteData.Slot8    = slotOrganizationModel.Slot8;
				remoteData.Slot9    = slotOrganizationModel.Slot9;
				remoteData.Slot10   = slotOrganizationModel.Slot10;
				remoteData.Slot11   = slotOrganizationModel.Slot11;
				remoteData.Slot12   = slotOrganizationModel.Slot12;
				remoteData.Slot13   = slotOrganizationModel.Slot13;
				remoteData.Slot14   = slotOrganizationModel.Slot14;
				remoteData.Slot15   = slotOrganizationModel.Slot15;
									
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetSlotCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 사용자 삭제
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetSlotDelete(SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode    = slotOrganizationModel.MediaCode;
				remoteData.CategoryCode = slotOrganizationModel.CategoryCode;
				remoteData.GenreCode	= slotOrganizationModel.GenreCode;
				remoteData.ChannelNo    = slotOrganizationModel.ChannelNo;
					
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetSlotDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSlotDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
