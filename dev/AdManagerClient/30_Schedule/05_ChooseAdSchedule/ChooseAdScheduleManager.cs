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
	public class ChooseAdScheduleManager : BaseManager
	{

		// 순위변경구분
		const int ORDER_FIRST = 1;
		const int ORDER_LAST  = 2;
		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;


		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public ChooseAdScheduleManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Schedule/ChooseAdScheduleService.asmx";
		}


		/// <summary>
		/// 메뉴 목록조회
		/// </summary>
		/// <param name="chooseAdScheduleModel"></param>
		public void GetMenuList(ChooseAdScheduleModel chooseAdScheduleModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChooseAdScheduleServiceProxy.ChooseAdScheduleService svc = new ChooseAdScheduleServiceProxy.ChooseAdScheduleService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChooseAdScheduleServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.ChooseAdScheduleServiceProxy.HeaderModel();
				ChooseAdScheduleServiceProxy.ChooseAdScheduleModel remoteData   = new AdManagerClient.ChooseAdScheduleServiceProxy.ChooseAdScheduleModel();

				remoteHeader.UserID        = Header.UserID;
				remoteData.SearchMediaCode = chooseAdScheduleModel.SearchMediaCode;			
				
                svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.wMenuList1(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				chooseAdScheduleModel.ChooseAdScheduleDataSet = remoteData.ChooseAdScheduleDataSet.Copy();
				chooseAdScheduleModel.ResultCnt   = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCategenList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// <param name="chooseAdScheduleModel"></param>
		public void GetChannelList(ChooseAdScheduleModel chooseAdScheduleModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널목록조회 Start");
				_log.Debug("-----------------------------------------");
				// 웹서비스 인스턴스 생성
				ChooseAdScheduleServiceProxy.ChooseAdScheduleService svc = new ChooseAdScheduleServiceProxy.ChooseAdScheduleService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChooseAdScheduleServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.ChooseAdScheduleServiceProxy.HeaderModel();
				ChooseAdScheduleServiceProxy.ChooseAdScheduleModel remoteData   = new AdManagerClient.ChooseAdScheduleServiceProxy.ChooseAdScheduleModel();
                
				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.MediaCode       = chooseAdScheduleModel.MediaCode;
				remoteData.CategoryCode    = chooseAdScheduleModel.CategoryCode;
				remoteData.GenreCode       = chooseAdScheduleModel.GenreCode;
							
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetChannelList_0907a(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				chooseAdScheduleModel.ChooseAdScheduleDataSet = remoteData.ChooseAdScheduleDataSet.Copy();
				chooseAdScheduleModel.ResultCnt   = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 회차별 리스트 조회
		/// </summary>
		/// <param name="chooseAdScheduleModel"></param>
		public void GetChannelSerList(ChooseAdScheduleModel chooseAdScheduleModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("회차별 목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChooseAdScheduleServiceProxy.ChooseAdScheduleService svc = new ChooseAdScheduleServiceProxy.ChooseAdScheduleService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChooseAdScheduleServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.ChooseAdScheduleServiceProxy.HeaderModel();
				ChooseAdScheduleServiceProxy.ChooseAdScheduleModel remoteData   = new AdManagerClient.ChooseAdScheduleServiceProxy.ChooseAdScheduleModel();
                
				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.MediaCode       = chooseAdScheduleModel.MediaCode;
				remoteData.CategoryCode    = chooseAdScheduleModel.CategoryCode;
				remoteData.GenreCode       = chooseAdScheduleModel.GenreCode;
							
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetSeriesList_0907a(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				chooseAdScheduleModel.ChooseAdScheduleDataSet = remoteData.ChooseAdScheduleDataSet.Copy();
				chooseAdScheduleModel.ResultCnt   = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 메뉴 편성현황조회
		/// </summary>
		/// <param name="chooseAdScheduleModel"></param>
		public void GetChooseAdScheduleListMenu(ChooseAdScheduleModel chooseAdScheduleModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴편성현황조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChooseAdScheduleServiceProxy.ChooseAdScheduleService svc = new ChooseAdScheduleServiceProxy.ChooseAdScheduleService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChooseAdScheduleServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.ChooseAdScheduleServiceProxy.HeaderModel();
				ChooseAdScheduleServiceProxy.ChooseAdScheduleModel remoteData   = new AdManagerClient.ChooseAdScheduleServiceProxy.ChooseAdScheduleModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass     = Header.UserClass;

                // 호출정보 셋트
				remoteData.MediaCode		     =  chooseAdScheduleModel.MediaCode;       
				remoteData.GenreCode		     =  chooseAdScheduleModel.GenreCode;       
                remoteData.SearchchkAdState_10  =   chooseAdScheduleModel.SearchchkAdState_10;
                remoteData.SearchchkAdState_20  =   chooseAdScheduleModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30  =   chooseAdScheduleModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40  =   chooseAdScheduleModel.SearchchkAdState_40;
                
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetChooseAdScheduleListMenu(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				chooseAdScheduleModel.ChooseAdScheduleDataSet = remoteData.ChooseAdScheduleDataSet.Copy();
				chooseAdScheduleModel.LastOrder   = remoteData.LastOrder;				
				chooseAdScheduleModel.ResultCnt   = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴편성현황조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChooseAdScheduleList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 채널 편성현황조회
		/// </summary>
		/// <param name="chooseAdScheduleModel"></param>
		public void GetChooseAdScheduleListChannel(ChooseAdScheduleModel chooseAdScheduleModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널편성현황조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChooseAdScheduleServiceProxy.ChooseAdScheduleService svc = new ChooseAdScheduleServiceProxy.ChooseAdScheduleService();

				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChooseAdScheduleServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.ChooseAdScheduleServiceProxy.HeaderModel();
				ChooseAdScheduleServiceProxy.ChooseAdScheduleModel remoteData   = new AdManagerClient.ChooseAdScheduleServiceProxy.ChooseAdScheduleModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.MediaCode        =  chooseAdScheduleModel.MediaCode;       
				remoteData.GenreCode		=  chooseAdScheduleModel.GenreCode;       
				remoteData.ChannelNo		=  chooseAdScheduleModel.ChannelNo;       
                remoteData.SearchchkAdState_10  = chooseAdScheduleModel.SearchchkAdState_10;
                remoteData.SearchchkAdState_20  = chooseAdScheduleModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30  = chooseAdScheduleModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40  = chooseAdScheduleModel.SearchchkAdState_40;
                

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetChooseAdScheduleListChannel(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				chooseAdScheduleModel.ChooseAdScheduleDataSet = remoteData.ChooseAdScheduleDataSet.Copy();
				chooseAdScheduleModel.LastOrder   = remoteData.LastOrder;				
				chooseAdScheduleModel.ResultCnt   = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널편성현황조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChooseAdScheduleList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 회차 편성현황조회
		/// </summary>
		/// <param name="chooseAdScheduleModel"></param>
		public void GetChooseAdScheduleListSeries(ChooseAdScheduleModel chooseAdScheduleModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널회차 편성현황조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChooseAdScheduleServiceProxy.ChooseAdScheduleService svc = new ChooseAdScheduleServiceProxy.ChooseAdScheduleService();

				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChooseAdScheduleServiceProxy.HeaderModel			remoteHeader = new AdManagerClient.ChooseAdScheduleServiceProxy.HeaderModel();
				ChooseAdScheduleServiceProxy.ChooseAdScheduleModel	remoteData   = new AdManagerClient.ChooseAdScheduleServiceProxy.ChooseAdScheduleModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.MediaCode        =  chooseAdScheduleModel.MediaCode;
				remoteData.GenreCode		=  chooseAdScheduleModel.GenreCode;
				remoteData.ChannelNo		=  chooseAdScheduleModel.ChannelNo;
				remoteData.SeriesNo			=  chooseAdScheduleModel.SeriesNo;

				remoteData.SearchchkAdState_10  = chooseAdScheduleModel.SearchchkAdState_10;
				remoteData.SearchchkAdState_20  = chooseAdScheduleModel.SearchchkAdState_20;
				remoteData.SearchchkAdState_30  = chooseAdScheduleModel.SearchchkAdState_30;
				remoteData.SearchchkAdState_40  = chooseAdScheduleModel.SearchchkAdState_40;
                

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetChooseAdScheduleListSeries(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				chooseAdScheduleModel.ChooseAdScheduleDataSet = remoteData.ChooseAdScheduleDataSet.Copy();
				chooseAdScheduleModel.LastOrder   = remoteData.LastOrder;				
				chooseAdScheduleModel.ResultCnt   = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널편성현황조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChooseAdScheduleList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 지정메뉴광고편성 순위 변경
		/// </summary>
		/// <param name="schHomeAdModel"></param>
		/// <returns></returns>
		public void SetSchMenuAdOrderSet(ChooseAdScheduleModel chooseAdScheduleModel, int OrderSet)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("지정메뉴광고편성 순위 변경 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(chooseAdScheduleModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}

				// 웹서비스 인스턴스 생성
				ChooseAdScheduleServiceProxy.ChooseAdScheduleService svc = new ChooseAdScheduleServiceProxy.ChooseAdScheduleService();

				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChooseAdScheduleServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.ChooseAdScheduleServiceProxy.HeaderModel();
				ChooseAdScheduleServiceProxy.ChooseAdScheduleModel remoteData   = new AdManagerClient.ChooseAdScheduleServiceProxy.ChooseAdScheduleModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트

				remoteData.MediaCode       =  chooseAdScheduleModel.MediaCode;
				remoteData.GenreCode       =  chooseAdScheduleModel.GenreCode;
				remoteData.ItemNo          =  chooseAdScheduleModel.ItemNo;
				remoteData.ItemName        =  chooseAdScheduleModel.ItemName;
				remoteData.ScheduleOrder   =  chooseAdScheduleModel.ScheduleOrder;
 
				switch(OrderSet)
				{
					case ORDER_FIRST:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchMenuAdOrderFirst(remoteHeader, remoteData);
						
						break;
					case ORDER_UP:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchMenuAdOrderUp(remoteHeader, remoteData);						
						break;
					case ORDER_DOWN:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchMenuAdOrderDown(remoteHeader, remoteData);						
						break;
					case ORDER_LAST:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchMenuAdOrderLast(remoteHeader, remoteData);						
						break;
					default:
						throw new FrameException("순위변경 구분이 선택되지 않았습니다.");
				}


				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				chooseAdScheduleModel.ResultCnt     = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD      = remoteData.ResultCD;		
				chooseAdScheduleModel.ScheduleOrder = remoteData.ScheduleOrder;
								
				_log.Debug("-----------------------------------------");
				_log.Debug("지정메뉴광고편성 순위 변경 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchMenuAdOrderSet():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 지정채널광고편성 순위 변경
		/// </summary>
		/// <param name="schHomeAdModel"></param>
		/// <returns></returns>
		public void SetSchChannelAdOrderSet(ChooseAdScheduleModel chooseAdScheduleModel, int OrderSet)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("지정채널광고편성 순위 변경 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(chooseAdScheduleModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}

				// 웹서비스 인스턴스 생성
				ChooseAdScheduleServiceProxy.ChooseAdScheduleService svc = new ChooseAdScheduleServiceProxy.ChooseAdScheduleService();

				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ChooseAdScheduleServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.ChooseAdScheduleServiceProxy.HeaderModel();
				ChooseAdScheduleServiceProxy.ChooseAdScheduleModel remoteData   = new AdManagerClient.ChooseAdScheduleServiceProxy.ChooseAdScheduleModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트

				remoteData.MediaCode       =  chooseAdScheduleModel.MediaCode;
				remoteData.ChannelNo       =  chooseAdScheduleModel.ChannelNo;
				remoteData.ItemNo          =  chooseAdScheduleModel.ItemNo;
				remoteData.ItemName        =  chooseAdScheduleModel.ItemName;
				remoteData.ScheduleOrder   =  chooseAdScheduleModel.ScheduleOrder;
 
				switch(OrderSet)
				{
					case ORDER_FIRST:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchChannelAdOrderFirst(remoteHeader, remoteData);
						
						break;
					case ORDER_UP:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchChannelAdOrderUp(remoteHeader, remoteData);						
						break;
					case ORDER_DOWN:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchChannelAdOrderDown(remoteHeader, remoteData);						
						break;
					case ORDER_LAST:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchChannelAdOrderLast(remoteHeader, remoteData);						
						break;
					default:
						throw new FrameException("순위변경 구분이 선택되지 않았습니다.");
				}


				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				chooseAdScheduleModel.ResultCnt     = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD      = remoteData.ResultCD;		
				chooseAdScheduleModel.ScheduleOrder = remoteData.ScheduleOrder;
								
				_log.Debug("-----------------------------------------");
				_log.Debug("지정채널광고편성 순위 변경 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchMenuAdOrderSet():" + fe.ErrCode + ":" + fe.ResultMsg);
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