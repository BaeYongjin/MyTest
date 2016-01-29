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
	public class ChannelSetManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public ChannelSetManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/ChannelSetService.asmx";
		}

		/// <summary>
		/// 미디어콤보정보조회
		/// </summary>
		/// <param name="channelSetModel"></param>
		public void GetCategoryList(ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("카테고리콤보 목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.CategoryCode = channelSetModel.MediaCode;
				
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
				channelSetModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

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
		/// 미디어콤보정보조회
		/// </summary>
		/// <param name="channelSetModel"></param>
		public void GetGenreList(ChannelSetModel channelSetModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("장르콤보 목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// 헤더정보 셋트				
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.SearchKey = channelSetModel.SearchKey;
				remoteData.GenreCode = channelSetModel.GenreCode;
				
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
				channelSetModel.GenreDataSet = remoteData.GenreDataSet.Copy();
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

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


		/// <summary>
		/// 채널정보조회
		/// </summary>
		/// <param name="channelSetModel"></param>
		public void GetChannelSetList(ChannelSetModel channelSetModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey       = channelSetModel.SearchKey;
				remoteData.MediaCode       = channelSetModel.MediaCode;
				remoteData.CategoryCode       = channelSetModel.CategoryCode;
				remoteData.GenreCode       = channelSetModel.GenreCode;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetChannelSetList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				channelSetModel.ChannelSetDataSet = remoteData.ChannelSetDataSet.Copy();
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="channelSetModel"></param>
		public void GetCategenList(ChannelSetModel channelSetModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널 디테일 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey       = channelSetModel.SearchKey;
				remoteData.SearchMediaName       = channelSetModel.SearchMediaName;
				remoteData.SearchCategoryName       = channelSetModel.SearchCategoryName;
				remoteData.SearchGenreName       = channelSetModel.SearchGenreName;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetCategenList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				channelSetModel.ChannelSetDataSet = remoteData.ChannelSetDataSet.Copy();
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

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

		public void GetChannelNoPopList(ChannelSetModel channelSetModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠팝업 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey       = channelSetModel.SearchKey;
				remoteData.MediaCode_P       = channelSetModel.MediaCode_P;
				remoteData.CategoryCode_P       = channelSetModel.CategoryCode_P;
				remoteData.GenreCode_P       = channelSetModel.GenreCode_P;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetChannelNoPopList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				channelSetModel.ChannelSetDataSet = remoteData.ChannelSetDataSet.Copy();
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠팝업 목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelNoPopList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// <param name="channelSetModel"></param>
		public void SetChannelSetUpdate(ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널셋정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사
				if(channelSetModel.ChannelNo.Length > 50) 
				{
					throw new FrameException("채널번호는 50Bytes를 초과할 수 없습니다.");
				}				

				// 웹서비스 인스턴스 생성
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.ChannelNo       = channelSetModel.ChannelNo;
				remoteData.SeriesNo     = channelSetModel.SeriesNo;				
				remoteData.MediaCode       = channelSetModel.MediaCode;
				remoteData.CategoryCode       = channelSetModel.CategoryCode;
				remoteData.GenreCode       = channelSetModel.GenreCode;
				
				remoteData.MediaCode_old       = channelSetModel.MediaCode_old;
				remoteData.CategoryCode_old       = channelSetModel.CategoryCode_old;
				remoteData.GenreCode_old       = channelSetModel.GenreCode_old;
				remoteData.ChannelNo_old       = channelSetModel.ChannelNo_old;
								
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetChannelSetUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널셋정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetChannelSetUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// <param name="channelSetModel"></param>
		/// <returns></returns>
		public void SetChannelSetAdd(ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널셋추가 Start");
				_log.Debug("-----------------------------------------");
				
				if(channelSetModel.ChannelNo.Length > 50) 
				{
					throw new FrameException("채널No는 50Bytes를 초과할 수 없습니다.");
				}			
				
				// 웹서비스 인스턴스 생성
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode       = channelSetModel.MediaCode;
				remoteData.CategoryCode       = channelSetModel.CategoryCode;
				remoteData.ChannelNo       = channelSetModel.ChannelNo;
				remoteData.SeriesNo     = channelSetModel.SeriesNo;								
				remoteData.GenreCode       = channelSetModel.GenreCode;		
								
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetChannelSetCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널셋추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				channelSetModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetChannelSetCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				channelSetModel.ResultCD    = "3101";
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
		public void SetChannelSetDelete(ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널셋삭제 start");
				_log.Debug("-----------------------------------------");
            
				// 웹서비스 인스턴스 생성
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
            				
				// 리모트 모델 생성
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();
            
				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;
            
				// 호출정보셋트
				remoteData.MediaCode       = channelSetModel.MediaCode;
				remoteData.CategoryCode       = channelSetModel.CategoryCode;
				remoteData.GenreCode       = channelSetModel.GenreCode;		
				remoteData.ChannelNo       = channelSetModel.ChannelNo;		
            					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetChannelSetDelete(remoteHeader, remoteData);
            
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// 결과 셋트
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("채널셋삭제 end");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":setchannelSetdelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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