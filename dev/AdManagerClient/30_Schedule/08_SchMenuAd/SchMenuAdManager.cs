// ===============================================================================
// SchMenuAdManager  for Charites Project
//
// SchMenuAdManager.cs
//
// �޴��� ���񽺸� ȣ���մϴ�. 
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
	/// ä������ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class SchMenuAdManager : BaseManager
	{
		#region [ ������ ]
		/// <summary>
		/// ������
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

		#region [ ������ ��ȸ�� ]
		/// <summary>
		/// Ư������ ��ü ����Ȳ ��ȸ��
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetItemScheduleList(SchMenuAdModel schMenuAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ư������ ��ü ����Ȳ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();

				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				SchMenuAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.ItemNo	=  schMenuAdModel.ItemNo;
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.GetItemScheduleList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))	throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);

				// ��� ��Ʈ
				schMenuAdModel.ItemScheduleDataSet = remoteData.ItemScheduleDataSet.Copy();
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����޴����� ���� ��ȸ End");
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
		/// [S1] ������ �������� �����Ѵ�
		/// </summary>
		/// <param name="model"></param>
		public void DeleteItemSchedule(SchedulePerItemModel	model)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ư������ ������ ���� ����Start");
				_log.Debug("-----------------------------------------");
				
				SchMenuAdServicePloxy.SchMenuAdService		svc		= new SchMenuAdServicePloxy.SchMenuAdService();
				SchMenuAdServicePloxy.HeaderModel			rHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchedulePerItemModel	rData	= new AdManagerClient.SchMenuAdServicePloxy.SchedulePerItemModel();

				// ������� ��Ʈ
				rHeader.ClientKey   = Header.ClientKey;
				rHeader.UserID      = Header.UserID;
				rHeader.UserLevel   = Header.UserLevel;
				rHeader.UserClass	= Header.UserClass;

				// ȣ������ ��Ʈ
				rData.ItemNo		=	model.ItemNo;
				rData.Media			=	model.Media;
				rData.Category		=	model.Category;
				rData.Genre			=	model.Genre;
				rData.Channel		=	model.Channel;
				rData.Series		=	model.Series;
				rData.DeleteJobType	=	(AdManagerClient.SchMenuAdServicePloxy.TYPE_ScheduleDelete)model.DeleteJobType;
                
				// ������ ����
				svc.Url		= _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout * 5;

				// ������ ȣ��
				rData	= svc.SetSchedulePerItemDelete( rHeader, rData );

				if( !rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// ��� ��Ʈ
				model.ResultCD		=	rData.ResultCD;
				model.ResultCnt		=	rData.ResultCnt;
				model.ResultDesc	=	rData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ư������ ������ ���� ���� End");
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
		/// [S1] ������ �������� �߰��Ѵ�
		/// </summary>
		/// <param name="model"></param>
		public void InsertItemSchedule(SchedulePerItemModel	model)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ư������ ������ ���� �߰�Start");
				_log.Debug("-----------------------------------------");
				
				SchMenuAdServicePloxy.SchMenuAdService		svc		= new SchMenuAdServicePloxy.SchMenuAdService();
				SchMenuAdServicePloxy.HeaderModel			rHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchedulePerItemModel	rData	= new AdManagerClient.SchMenuAdServicePloxy.SchedulePerItemModel();

				// ������� ��Ʈ
				rHeader.ClientKey   = Header.ClientKey;
				rHeader.UserID      = Header.UserID;
				rHeader.UserLevel   = Header.UserLevel;
				rHeader.UserClass	= Header.UserClass;

				// ȣ������ ��Ʈ
				rData.ItemNo		=	model.ItemNo;
				rData.Media			=	model.Media;
				rData.Category		=	model.Category;
				rData.Genre			=	model.Genre;
				rData.Channel		=	model.Channel;
				rData.Series		=	model.Series;
				rData.DeleteJobType	=	(AdManagerClient.SchMenuAdServicePloxy.TYPE_ScheduleDelete)model.DeleteJobType;
                
				// ������ ����
				svc.Url		= _WebServiceUrl;			
				svc.Timeout = FrameSystem.m_SystemTimeout * 5;

				// ������ ȣ��
				rData	= svc.SetSchedulePerItemInsert( rHeader, rData );

				if( !rData.ResultCD.Equals("0000"))	throw new FrameException(rData.ResultDesc, _module, rData.ResultCD);

				// ��� ��Ʈ
				model.ResultCD		=	rData.ResultCD;
				model.ResultCnt		=	rData.ResultCnt;
				model.ResultDesc	=	rData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ư������ ������ ���� �߰� End");
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
		/// �޴� �����ȸ
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetMenuList(SchMenuAdModel schMenuAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SchMenuAdServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode = schMenuAdModel.SearchMediaCode;			
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetMenuList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schMenuAdModel.MenuAdDataSet = remoteData.MenuAdDataSet.Copy();
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴� ��ȸ End");
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
		/// ���α��� �������ȸ
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetContractItemList(SchMenuAdModel schMenuAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���α��� ����� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();
				svc.Url = _WebServiceUrl;			
				
				// ����Ʈ �� ����
				SchMenuAdServicePloxy.HeaderModel    remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             =  schMenuAdModel.SearchKey;               
				remoteData.SearchMediaCode		 =  schMenuAdModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 =  schMenuAdModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     =  schMenuAdModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  =  schMenuAdModel.SearchAdvertiserCode;
				remoteData.SearchchkAdState_10	 =  schMenuAdModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 =  schMenuAdModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 =  schMenuAdModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 =  schMenuAdModel.SearchchkAdState_40; 

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetContractItemList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schMenuAdModel.MenuAdDataSet = remoteData.MenuAdDataSet.Copy();
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���α��� ����� End");
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
		/// �����޴����� ���� ��ȸ
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetSchChoiceMenuDetailList(SchMenuAdModel schMenuAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����޴����� ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();

				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				SchMenuAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.ItemNo             =  schMenuAdModel.ItemNo;               
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetSchChoiceMenuDetailList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schMenuAdModel.MenuDataSet = remoteData.MenuDataSet.Copy();
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����޴����� ���� ��ȸ End");
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
		/// �����޴����� ���� ��ȸ
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetSchChoiceMenuDetailContractSeq(SchMenuAdModel schMenuAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����޴����� ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();

				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				SchMenuAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.ContractSeq             =  schMenuAdModel.ContractSeq;               
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetSchChoiceMenuDetailContractSeq(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schMenuAdModel.ChoiceMenuContractDataSet = remoteData.ChoiceMenuContractDataSet.Copy();
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����޴����� ���� ��ȸ End");
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
		/// �޴� ����Ȳ��ȸ
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetChooseAdScheduleListMenu(SchMenuAdModel schMenuAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴�����Ȳ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SchMenuAdServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.MediaCode		     =  schMenuAdModel.MediaCode;       
				remoteData.GenreCode		     =  schMenuAdModel.GenreCode;       
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetChooseAdScheduleListMenu(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schMenuAdModel.MenuAdDataSet = remoteData.MenuAdDataSet.Copy();
				schMenuAdModel.LastOrder   = remoteData.LastOrder;				
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴�����Ȳ��ȸ End");
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
		/// �޴� ����Ȳ��ȸ
		/// </summary>
		/// <param name="schMenuAdModel"></param>
		public void GetChooseAdScheduleListContract(SchMenuAdModel schMenuAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴�����Ȳ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchMenuAdServicePloxy.SchMenuAdService svc = new SchMenuAdServicePloxy.SchMenuAdService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SchMenuAdServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchMenuAdServicePloxy.HeaderModel();
				SchMenuAdServicePloxy.SchMenuAdModel remoteData   = new AdManagerClient.SchMenuAdServicePloxy.SchMenuAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.MediaCode		     =  schMenuAdModel.MediaCode;       
				remoteData.GenreCode		     =  schMenuAdModel.GenreCode;       
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetChooseAdScheduleListContract(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schMenuAdModel.MenuContractDataSet = remoteData.MenuContractDataSet.Copy();
				schMenuAdModel.LastOrder   = remoteData.LastOrder;				
				schMenuAdModel.ResultCnt   = remoteData.ResultCnt;
				schMenuAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴�����Ȳ��ȸ End");
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