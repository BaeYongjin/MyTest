// ===============================================================================
// SchChoiceAd Manager  for Charites Project
//
// SchChoiceAdManager.cs
//
// 광고파일정보 서비스를 호출합니다. 
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
	/// 지정편성정보 웹서비스를 호출합니다. 
	/// </summary>
	public class SchDesignatedAdManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SchDesignatedAdManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "SchDesignated";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Schedule/SchDesignateService.asmx";
		}

		/// <summary>
		/// 지정편성 리스트를 가져온다
		/// </summary>
		/// <param name="model"></param>
		public void GetDataList(SchDesignateModel model)
		{
			try
			{
				SchDesignateServiceProxy.SchDesignateService svc	= new AdManagerClient.SchDesignateServiceProxy.SchDesignateService();
				SchDesignateServiceProxy.HeaderModel		rHeader = new AdManagerClient.SchDesignateServiceProxy.HeaderModel();
				SchDesignateServiceProxy.SchDesignateModel	rData	= new AdManagerClient.SchDesignateServiceProxy.SchDesignateModel();

				svc.Url = _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout;
				

				// 헤더정보 셋트
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				rData.Media		=	model.Media;
				rData.MediaRep	=	model.MediaRep;
				rData.AdState10	=	model.AdState10;
				rData.AdState20	=	model.AdState20;
				rData.AdState30	=	model.AdState30;
				rData.AdState40	=	model.AdState40;
				
				// 웹서비스 메소드 호출
				rData	= svc.GetList(rHeader, rData );

				// 결과코드검사
				if(!rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// 결과 셋트
				model.DsSchedule=	rData.DsSchedule.Copy();
				model.ResultCnt	=	rData.ResultCnt;
				model.ResultCD	=	rData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDataList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 편성대상 광고 리스트를 가져온다
		/// </summary>
		/// <param name="model"></param>
		public void GetItemList(SchDesignateModel model)
		{
			try
			{
				SchDesignateServiceProxy.SchDesignateService svc	= new AdManagerClient.SchDesignateServiceProxy.SchDesignateService();
				SchDesignateServiceProxy.HeaderModel		rHeader = new AdManagerClient.SchDesignateServiceProxy.HeaderModel();
				SchDesignateServiceProxy.SchDesignateModel	rData	= new AdManagerClient.SchDesignateServiceProxy.SchDesignateModel();

				svc.Url = _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout;
				

				// 헤더정보 셋트
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				rData.Media		=	model.Media;
				rData.MediaRep	=	model.MediaRep;
				rData.AdState10	=	model.AdState10;
				rData.AdState20	=	model.AdState20;
				rData.AdState30	=	model.AdState30;
				rData.AdState40	=	model.AdState40;
				
				// 웹서비스 메소드 호출
				rData	= svc.GetItemList(rHeader, rData );

				// 결과코드검사
				if(!rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// 결과 셋트
				model.DsItem=	rData.DsItem.Copy();
				model.ResultCnt	=	rData.ResultCnt;
				model.ResultCD	=	rData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDataList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 선택한 Row를 삭제합니다(단일)
		/// </summary>
		/// <param name="model"></param>
		public void	DeleteRow(SchDesignateModel model)
		{
			try
			{
				SchDesignateServiceProxy.SchDesignateService svc	= new AdManagerClient.SchDesignateServiceProxy.SchDesignateService();
				SchDesignateServiceProxy.HeaderModel		rHeader = new AdManagerClient.SchDesignateServiceProxy.HeaderModel();
				SchDesignateServiceProxy.SchDesignateModel	rData	= new AdManagerClient.SchDesignateServiceProxy.SchDesignateModel();

				svc.Url = _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout;
				

				// 헤더정보 셋트
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				rData.Media		=	model.Media;
				rData.Category	=	model.Category;
				rData.Genre		=	model.Genre;
				rData.Channel	=	model.Channel;
				rData.Series	=	model.Series;
				rData.ItemNo	=	model.ItemNo;
				
				// 웹서비스 메소드 호출
				rData	= svc.DeleteData(rHeader, rData );

				// 결과코드검사
				if(!rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				model.ResultCnt	=	rData.ResultCnt;
				model.ResultCD	=	rData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":DeleteRow():" + fe.ErrCode + ":" + fe.ResultMsg);
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

		public void	InsertRow(SchDesignateModel model)
		{
			try
			{
				SchDesignateServiceProxy.SchDesignateService svc	= new AdManagerClient.SchDesignateServiceProxy.SchDesignateService();
				SchDesignateServiceProxy.HeaderModel		rHeader = new AdManagerClient.SchDesignateServiceProxy.HeaderModel();
				SchDesignateServiceProxy.SchDesignateModel	rData	= new AdManagerClient.SchDesignateServiceProxy.SchDesignateModel();

				svc.Url = _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout;
				

				// 헤더정보 셋트
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				rData.Media		=	model.Media;
				rData.Category	=	model.Category;
				rData.Genre		=	model.Genre;
				rData.Channel	=	model.Channel;
				rData.Series	=	model.Series;
				rData.ItemNo	=	model.ItemNo;
				
				// 웹서비스 메소드 호출
				rData	= svc.InsertData(rHeader, rData );

				// 결과코드검사
				if(!rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				model.ResultCnt	=	rData.ResultCnt;
				model.ResultCD	=	rData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":DeleteRow():" + fe.ErrCode + ":" + fe.ResultMsg);
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
