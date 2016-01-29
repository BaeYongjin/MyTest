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
	public class RatioManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public RatioManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Target/RatioService.asmx";
		}

		/// <summary>
		/// 지정메뉴광고 상세편성 조회
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetSchChoiceMenuDetailList(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("지정메뉴광고 상세편성 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();

				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.ItemNo             =  ratioModel.ItemNo;               
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetSchChoiceMenuDetailList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				ratioModel.MenuDataSet = remoteData.MenuDataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("지정메뉴광고 상세편성 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSchChoiceMenuDetailList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 그룹1목록조회
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetSchRateList(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("그룹1목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.ItemNo		   = ratioModel.ItemNo;
//				remoteData.EntrySeq         = ratioModel.EntrySeq;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetSchRateList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				ratioModel.SchRateDataSet = remoteData.SchRateDataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("그룹1목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSchRateList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 그룹1목록조회
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetSchRateDetailList(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("그룹1목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
                remoteData.ItemNo       = ratioModel.ItemNo;
                remoteData.MediaCode    = ratioModel.MediaCode;
                remoteData.EntrySeq     = ratioModel.EntrySeq;
							
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetSchRateDetailList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				ratioModel.SchRateDetailDataSet = remoteData.SchRateDetailDataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("그룹1목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSchRateDetailList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 그룹1목록조회
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup1List(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("그룹1목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.ItemNo		   = ratioModel.ItemNo;
				remoteData.EntryYN       = ratioModel.EntryYN;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetGroup1List(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				ratioModel.Group1DataSet = remoteData.Group1DataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("그룹1목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGroup1List():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 그룹2목록조회
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup2List(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("그룹2목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.ItemNo		   = ratioModel.ItemNo;
				remoteData.EntryYN       = ratioModel.EntryYN;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetGroup2List(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				ratioModel.Group2DataSet = remoteData.Group2DataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("그룹2목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGroup2List():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 그룹3목록조회
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup3List(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("그룹3목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.ItemNo		   = ratioModel.ItemNo;
				remoteData.EntryYN       = ratioModel.EntryYN;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetGroup3List(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				ratioModel.Group3DataSet = remoteData.Group3DataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("그룹3목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGroup3List():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 그룹4목록조회
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup4List(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("그룹4목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.ItemNo		   = ratioModel.ItemNo;
				remoteData.EntryYN       = ratioModel.EntryYN;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetGroup4List(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				ratioModel.Group4DataSet = remoteData.Group4DataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("그룹4목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGroup4List():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 그룹2목록조회
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup5List(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("그룹5목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.ItemNo		   = ratioModel.ItemNo;
				remoteData.EntryYN       = ratioModel.EntryYN;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetGroup5List(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				ratioModel.Group5DataSet = remoteData.Group5DataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("그룹5목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGroup5List():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 비율저장
		/// </summary>
		/// <param name="ratioModel"></param>
		/// <returns></returns>
		public void SetSchRateUpdate(RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("비율저장 Start");
				_log.Debug("-----------------------------------------");
				
				if(ratioModel.EntryRate.Length > 50) 
				{
					throw new FrameException("채널No는 50Bytes를 초과할 수 없습니다.");
				}			
				
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.EntryRate     = ratioModel.EntryRate;	
				remoteData.ItemNo       = ratioModel.ItemNo;
				remoteData.MediaCode       = ratioModel.MediaCode;
				remoteData.EntrySeq       = ratioModel.EntrySeq;											
												
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetSchRateUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("비율저장 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				ratioModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchRateUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				ratioModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

	
		/// <summary>
		/// 비율추가
		/// </summary>
		/// <param name="ratioModel"></param>
		/// <returns></returns>
		public void SetSchRateCreate(RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("비율추가 Start");
				_log.Debug("-----------------------------------------");
				
				if(ratioModel.EntryRate.Length > 50) 
				{
					throw new FrameException("채널No는 50Bytes를 초과할 수 없습니다.");
				}			
				
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.ItemNo       = ratioModel.ItemNo;
				remoteData.MediaCode       = ratioModel.MediaCode;
				remoteData.EntrySeq       = ratioModel.EntrySeq;
				remoteData.EntryRate     = ratioModel.EntryRate;								
												
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetSchRateCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("비율추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				ratioModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchRateCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				ratioModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		/// <summary>
		/// 비율추가
		/// </summary>
		/// <param name="ratioModel"></param>
		/// <returns></returns>
		public void SetSchRateDetailCreate(RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("비율추가 Start");
				_log.Debug("-----------------------------------------");
				
				if(ratioModel.EntryRate.Length > 50) 
				{
					throw new FrameException("채널No는 50Bytes를 초과할 수 없습니다.");
				}			
				
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.ItemNo       = ratioModel.ItemNo;
				remoteData.MediaCode       = ratioModel.MediaCode;
				remoteData.EntrySeq       = ratioModel.EntrySeq;
				remoteData.CategoryCode     = ratioModel.CategoryCode;	
				remoteData.GenreCode     = ratioModel.GenreCode;				
												
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetSchRateDetailCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("비율추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				ratioModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchRateDetailCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				ratioModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		
		/// <summary>
		/// 비율 삭제
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetSchRateDelete(RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("비율삭제 start");
				_log.Debug("-----------------------------------------");
            
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
            				
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();
            
				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;
            
				// 호출정보셋트
				remoteData.ItemNo       = ratioModel.ItemNo;
				remoteData.MediaCode       = ratioModel.MediaCode;
				remoteData.EntrySeq       = ratioModel.EntrySeq;
            					
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetSchRateDelete(remoteHeader, remoteData);
            
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// 결과 셋트
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("비율삭제 end");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchRateDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 비율 삭제
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetSchRateDetailDelete(RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("비율삭제 start");
				_log.Debug("-----------------------------------------");
            
				// 웹서비스 인스턴스 생성
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
            				
				// 리모트 모델 생성
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();
            
				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;
            
				// 호출정보셋트
				remoteData.ItemNo       = ratioModel.ItemNo;
				remoteData.MediaCode       = ratioModel.MediaCode;
				remoteData.EntrySeq       = ratioModel.EntrySeq;
				remoteData.CategoryCode     = ratioModel.CategoryCode;	
				remoteData.GenreCode     = ratioModel.GenreCode;
            					
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetSchRateDetailDelete(remoteHeader, remoteData);
            
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// 결과 셋트
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("비율삭제 end");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchRateDetailDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 편성비율정리
        /// </summary>
        /// <param name="ratioModel"></param>
        public void mDeleteSync(RatioModel ratioModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("비율정리 start");
                _log.Debug("-----------------------------------------");
            
                // 웹서비스 인스턴스 생성
                RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
                svc.Url = _WebServiceUrl;
            				
                // 리모트 모델 생성
                RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
                RatioServicePloxy.RatioModel    remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();
            
                // 헤더정보 셋트
                //remoteHeader.SearchKey     = Header.SearchKey;
                remoteHeader.UserID        = Header.UserID;
            
                // 호출정보셋트
                remoteData.ItemNo       = ratioModel.ItemNo;
            					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.mDeleteSync(remoteHeader, remoteData);
            
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // 결과 셋트
                ratioModel.ResultCnt   = remoteData.ResultCnt;
                ratioModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("비율정리 end");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":mDeleteSync():" + fe.ErrCode + ":" + fe.ResultMsg);
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