// ===============================================================================
// SchMenuAdManager  for Charites Project
//
// SchMenuAdManager.cs
//
// 메뉴편성 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
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
	public class SchMenuAdManager : BaseManager
	{
		#region [ 생성자 ]
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>
		public SchMenuAdManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Schedule/SchMenuAdService.asmx";
		}

		#endregion

		#region [ 데이터 조회용 ]
		/// <summary>
		/// 특정광고별 전체 편성현황 조회용
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetItemScheduleList(SchMenuAdModel schMenuAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("특정광고별 전체 편성현황 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();

				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				SchMenuAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.ItemNo	=  schMenuAdModel.ItemNo;
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.GetItemScheduleList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))	throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);

				// 결과 셋트
				schMenuAdModel.ItemScheduleDataSet = remoteData.ItemScheduleDataSet.Copy();
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

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
		#endregion

		/// <summary>
		/// [S1] 선택한 편성정보를 삭제한다
		/// </summary>
		/// <param name="model"></param>
		public void DeleteItemSchedule(SchedulePerItemModel	model)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("특정광고별 선택편성 정보 삭제Start");
				_log.Debug("-----------------------------------------");
				
				SchMenuAdServicePloxy.SchMenuAdService		svc		= new SchMenuAdServicePloxy.SchMenuAdService();
				SchMenuAdServicePloxy.HeaderModel			rHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchedulePerItemModel	rData	= new AdManagerClient.SchMenuAdServicePloxy.SchedulePerItemModel();

				// 헤더정보 셋트
				rHeader.ClientKey   = Header.ClientKey;
				rHeader.UserID      = Header.UserID;
				rHeader.UserLevel   = Header.UserLevel;
				rHeader.UserClass	= Header.UserClass;

				// 호출정보 셋트
				rData.ItemNo		=	model.ItemNo;
				rData.Media			=	model.Media;
				rData.Category		=	model.Category;
				rData.Genre			=	model.Genre;
				rData.Channel		=	model.Channel;
				rData.Series		=	model.Series;
				rData.DeleteJobType	=	(AdManagerClient.SchMenuAdServicePloxy.TYPE_ScheduleDelete)model.DeleteJobType;
                
				// 웹서비스 설정
				svc.Url		= _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout * 5;

				// 웹서비스 호출
				rData	= svc.SetSchedulePerItemDelete( rHeader, rData );

				if( !rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// 결과 셋트
				model.ResultCD		=	rData.ResultCD;
				model.ResultCnt		=	rData.ResultCnt;
				model.ResultDesc	=	rData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("특정광고별 선택편성 정보 삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":DeleteItemSchedule():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// [S1] 선택한 편성정보를 추가한다
		/// </summary>
		/// <param name="model"></param>
		public void InsertItemSchedule(SchedulePerItemModel	model)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("특정광고별 선택편성 정보 추가Start");
				_log.Debug("-----------------------------------------");
				
				SchMenuAdServicePloxy.SchMenuAdService		svc		= new SchMenuAdServicePloxy.SchMenuAdService();
				SchMenuAdServicePloxy.HeaderModel			rHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchedulePerItemModel	rData	= new AdManagerClient.SchMenuAdServicePloxy.SchedulePerItemModel();

				// 헤더정보 셋트
				rHeader.ClientKey   = Header.ClientKey;
				rHeader.UserID      = Header.UserID;
				rHeader.UserLevel   = Header.UserLevel;
				rHeader.UserClass	= Header.UserClass;

				// 호출정보 셋트
				rData.ItemNo		=	model.ItemNo;
				rData.Media			=	model.Media;
				rData.Category		=	model.Category;
				rData.Genre			=	model.Genre;
				rData.Channel		=	model.Channel;
				rData.Series		=	model.Series;
				rData.DeleteJobType	=	(AdManagerClient.SchMenuAdServicePloxy.TYPE_ScheduleDelete)model.DeleteJobType;
                
				// 웹서비스 설정
				svc.Url		= _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout * 5;

				// 웹서비스 호출
				rData	= svc.SetSchedulePerItemInsert( rHeader, rData );

				if( !rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// 결과 셋트
				model.ResultCD		=	rData.ResultCD;
				model.ResultCnt		=	rData.ResultCnt;
				model.ResultDesc	=	rData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("특정광고별 선택편성 정보 추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":InsertItemSchedule():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 메뉴 목록조회
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetMenuList(SchMenuAdModel schMenuAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SchMenuAdServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// 헤더정보 셋트
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchMediaCode = schMenuAdModel.SearchMediaCode;			
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetMenuList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schMenuAdModel.MenuAdDataSet = remoteData.MenuAdDataSet.Copy();
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

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
		/// 재핑광고 편성대상조회
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetContractItemList(SchMenuAdModel schMenuAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("재핑광고 편성대상 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();
				svc.Url = _WebServiceUrl;			
				
				// 리모트 모델 생성
				SchMenuAdServicePloxy.HeaderModel    remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey             =  schMenuAdModel.SearchKey;               
				remoteData.SearchMediaCode		 =  schMenuAdModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 =  schMenuAdModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     =  schMenuAdModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  =  schMenuAdModel.SearchAdvertiserCode;
				remoteData.SearchchkAdState_10	 =  schMenuAdModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 =  schMenuAdModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 =  schMenuAdModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 =  schMenuAdModel.SearchchkAdState_40; 

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
				schMenuAdModel.MenuAdDataSet = remoteData.MenuAdDataSet.Copy();
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("재핑광고 편성대상 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContractItemList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 지정메뉴광고 상세편성 조회
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetSchChoiceMenuDetailList(SchMenuAdModel schMenuAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("지정메뉴광고 상세편성 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();

				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				SchMenuAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.ItemNo             =  schMenuAdModel.ItemNo;               
                
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
				schMenuAdModel.MenuDataSet = remoteData.MenuDataSet.Copy();
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

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
		/// 지정메뉴광고 상세편성 조회
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetSchChoiceMenuDetailContractSeq(SchMenuAdModel schMenuAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("지정메뉴광고 상세편성 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();

				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				SchMenuAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.ContractSeq             =  schMenuAdModel.ContractSeq;               
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetSchChoiceMenuDetailContractSeq(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schMenuAdModel.ChoiceMenuContractDataSet = remoteData.ChoiceMenuContractDataSet.Copy();
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

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
		/// 메뉴 편성현황조회
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetChooseAdScheduleListMenu(SchMenuAdModel schMenuAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴편성현황조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SchMenuAdServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.MediaCode		     =  schMenuAdModel.MediaCode;       
				remoteData.GenreCode		     =  schMenuAdModel.GenreCode;       
                
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
				schMenuAdModel.MenuAdDataSet = remoteData.MenuAdDataSet.Copy();
				schMenuAdModel.LastOrder   = remoteData.LastOrder;				
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

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
		/// 메뉴 편성현황조회
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetChooseAdScheduleListContract(SchMenuAdModel schMenuAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴편성현황조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SchMenuAdServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.MediaCode		     =  schMenuAdModel.MediaCode;       
				remoteData.GenreCode		     =  schMenuAdModel.GenreCode;       
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetChooseAdScheduleListContract(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schMenuAdModel.MenuContractDataSet = remoteData.MenuContractDataSet.Copy();
				schMenuAdModel.LastOrder   = remoteData.LastOrder;				
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴편성현황조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChooseAdScheduleListContract():" + fe.ErrCode + ":" + fe.ResultMsg);
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