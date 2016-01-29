using System;
using System.Data;
using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;
using AdManagerModel;

namespace AdManagerClient
{
	public class Sch3FormManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>
		public Sch3FormManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "Sch3Form";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Schedule/Sch3FormService.asmx";
		}

		public void GetChannelList(Sch3FormModel model)
		{
			try
			{
				Sch3FormServiceProxy.Sch3FormService	svc = new AdManagerClient.Sch3FormServiceProxy.Sch3FormService();
				Sch3FormServiceProxy.HeaderModel	rHeader = new AdManagerClient.Sch3FormServiceProxy.HeaderModel();
				Sch3FormServiceProxy.Sch3FormModel	rData	= new AdManagerClient.Sch3FormServiceProxy.Sch3FormModel();

				svc.Url = _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout;
				

				// 헤더정보 셋트
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				rData.Media		= model.Media;
				rData.Category	= model.Category;
				rData.Genre		= model.Genre;
				rData.Channel	= 0;
				rData.DataType	= model.DataType;
				rData.ItemNo	=	model.ItemNo;
				
				// 웹서비스 메소드 호출
				rData	= svc.GetChannelList(rHeader, rData );

				// 결과코드검사
				if(!rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// 결과 셋트
				model.DsChannel	=	rData.DsChannel.Copy();
				model.ResultCnt	=	rData.ResultCnt;
				model.ResultCD	=	rData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreListCss():" + fe.ErrCode + ":" + fe.ResultMsg);
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


		public void GetGenreListCss(Sch3FormModel	model)
		{
			try
			{
				Sch3FormServiceProxy.Sch3FormService	svc = new AdManagerClient.Sch3FormServiceProxy.Sch3FormService();
				Sch3FormServiceProxy.HeaderModel	rHeader = new AdManagerClient.Sch3FormServiceProxy.HeaderModel();
				Sch3FormServiceProxy.Sch3FormModel	rData	= new AdManagerClient.Sch3FormServiceProxy.Sch3FormModel();

				svc.Url = _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout;
				

				// 헤더정보 셋트
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				rData.Media		= 1;
				rData.Category	= 0;
				rData.Genre		= 0;
				rData.Channel	= 0;
				rData.ItemNo	=	model.ItemNo;
				
				
				// 웹서비스 메소드 호출
				rData	= svc.GetGenreListCSS(rHeader, rData );

				// 결과코드검사
				if(!rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// 결과 셋트
				model.DsGenre	=	rData.DsGenre.Copy();
				model.ResultCnt	=	rData.ResultCnt;
				model.ResultCD	=	rData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreListCss():" + fe.ErrCode + ":" + fe.ResultMsg);
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


		public void GetGenreListTot(Sch3FormModel	model)
		{
			try
			{
				Sch3FormServiceProxy.Sch3FormService	svc = new AdManagerClient.Sch3FormServiceProxy.Sch3FormService();
				Sch3FormServiceProxy.HeaderModel	rHeader = new AdManagerClient.Sch3FormServiceProxy.HeaderModel();
				Sch3FormServiceProxy.Sch3FormModel	rData	= new AdManagerClient.Sch3FormServiceProxy.Sch3FormModel();

				svc.Url = _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout;
				

				// 헤더정보 셋트
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				rData.Media		= 1;
				rData.Category	= 0;
				rData.Genre		= 0;
				rData.Channel	= 0;
				rData.ItemNo	=	model.ItemNo;
				
				
				// 웹서비스 메소드 호출
				rData	= svc.GetGenreListTot(rHeader, rData );

				// 결과코드검사
				if(!rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// 결과 셋트
				model.DsGenre	=	rData.DsGenre.Copy();
				model.ResultCnt	=	rData.ResultCnt;
				model.ResultCD	=	rData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreListTot():" + fe.ErrCode + ":" + fe.ResultMsg);
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


		public void GetGenreListDesign(Sch3FormModel	model)
		{
			try
			{
				Sch3FormServiceProxy.Sch3FormService	svc = new AdManagerClient.Sch3FormServiceProxy.Sch3FormService();
				Sch3FormServiceProxy.HeaderModel	rHeader = new AdManagerClient.Sch3FormServiceProxy.HeaderModel();
				Sch3FormServiceProxy.Sch3FormModel	rData	= new AdManagerClient.Sch3FormServiceProxy.Sch3FormModel();

				svc.Url = _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout;
				

				// 헤더정보 셋트
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				rData.Media		= 1;
				rData.Category	= 0;
				rData.Genre		= 0;
				rData.Channel	= 0;
				rData.ItemNo	=	model.ItemNo;
				
				
				// 웹서비스 메소드 호출
				rData	= svc.GetGenreListDesign(rHeader, rData );

				// 결과코드검사
				if(!rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// 결과 셋트
				model.DsGenre	=	rData.DsGenre.Copy();
				model.ResultCnt	=	rData.ResultCnt;
				model.ResultCD	=	rData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreListTot():" + fe.ErrCode + ":" + fe.ResultMsg);
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


		public void GetSeriesList(Sch3FormModel	model)
		{
			try
			{
				Sch3FormServiceProxy.Sch3FormService	svc = new AdManagerClient.Sch3FormServiceProxy.Sch3FormService();
				Sch3FormServiceProxy.HeaderModel	rHeader = new AdManagerClient.Sch3FormServiceProxy.HeaderModel();
				Sch3FormServiceProxy.Sch3FormModel	rData	= new AdManagerClient.Sch3FormServiceProxy.Sch3FormModel();

				svc.Url = _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout;
				

				// 헤더정보 셋트
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				rData.Media		= model.Media;
				rData.Category	= model.Category;
				rData.Genre		= model.Genre;
				rData.Channel	= model.Channel;
				rData.ItemNo	=	model.ItemNo;
				rData.DataType	= model.DataType;
				
				// 웹서비스 메소드 호출
				rData	= svc.GetSeriesList(rHeader, rData );

				// 결과코드검사
				if(!rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// 결과 셋트
				model.DsSeries	=	rData.DsSeries.Copy();
				model.ResultCnt	=	rData.ResultCnt;
				model.ResultCD	=	rData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreListCss():" + fe.ErrCode + ":" + fe.ResultMsg);
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
