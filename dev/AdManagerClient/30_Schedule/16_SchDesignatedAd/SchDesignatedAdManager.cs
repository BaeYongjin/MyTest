// ===============================================================================
// SchChoiceAd Manager  for Charites Project
//
// SchChoiceAdManager.cs
//
// ������������ ���񽺸� ȣ���մϴ�. 
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
	/// ���������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class SchDesignatedAdManager : BaseManager
	{
		/// <summary>
		/// ������
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
		/// ������ ����Ʈ�� �����´�
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
				

				// ������� ��Ʈ
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				rData.Media		=	model.Media;
				rData.MediaRep	=	model.MediaRep;
				rData.AdState10	=	model.AdState10;
				rData.AdState20	=	model.AdState20;
				rData.AdState30	=	model.AdState30;
				rData.AdState40	=	model.AdState40;
				
				// ������ �޼ҵ� ȣ��
				rData	= svc.GetList(rHeader, rData );

				// ����ڵ�˻�
				if(!rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// ��� ��Ʈ
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
		/// ����� ���� ����Ʈ�� �����´�
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
				

				// ������� ��Ʈ
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				rData.Media		=	model.Media;
				rData.MediaRep	=	model.MediaRep;
				rData.AdState10	=	model.AdState10;
				rData.AdState20	=	model.AdState20;
				rData.AdState30	=	model.AdState30;
				rData.AdState40	=	model.AdState40;
				
				// ������ �޼ҵ� ȣ��
				rData	= svc.GetItemList(rHeader, rData );

				// ����ڵ�˻�
				if(!rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// ��� ��Ʈ
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
		/// ������ Row�� �����մϴ�(����)
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
				

				// ������� ��Ʈ
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				rData.Media		=	model.Media;
				rData.Category	=	model.Category;
				rData.Genre		=	model.Genre;
				rData.Channel	=	model.Channel;
				rData.Series	=	model.Series;
				rData.ItemNo	=	model.ItemNo;
				
				// ������ �޼ҵ� ȣ��
				rData	= svc.DeleteData(rHeader, rData );

				// ����ڵ�˻�
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
				

				// ������� ��Ʈ
				rHeader.ClientKey     = Header.ClientKey;
				rHeader.UserID        = Header.UserID;
				rHeader.UserLevel     = Header.UserLevel;
				rHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				rData.Media		=	model.Media;
				rData.Category	=	model.Category;
				rData.Genre		=	model.Genre;
				rData.Channel	=	model.Channel;
				rData.Series	=	model.Series;
				rData.ItemNo	=	model.ItemNo;
				
				// ������ �޼ҵ� ȣ��
				rData	= svc.InsertData(rHeader, rData );

				// ����ڵ�˻�
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
