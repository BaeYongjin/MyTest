// ===============================================================================
// SchAppendAd Manager  for Charites Project
//
// SchAppendAdManager.cs
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

using System.Diagnostics;
using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// Ȩ���������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class SchAppendAdManager : BaseManager
	{

		// �������汸��
		const int ORDER_FIRST = 1;
		const int ORDER_LAST  = 2;
		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;


		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SchAppendAdManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "SchAppendAd";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Schedule/SchAppendAdService.asmx";
		}

		/// <summary>
		/// Ȩ��������Ȳ��ȸ
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		public void GetSchAppendAdList(SchAppendAdModel schAppendAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ��������Ȳ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();

				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			

				// ����Ʈ �� ����
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode		 =  schAppendAdModel.SearchMediaCode;	  

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetSchAppendAdList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schAppendAdModel.FileListCount = remoteData.FileListCount;  // 2007.10.02 RH.Jung ���ϸ���Ʈ�Ǽ��˻�

				schAppendAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				schAppendAdModel.LastOrder   = remoteData.LastOrder;
				schAppendAdModel.ResultCnt   = remoteData.ResultCnt;
				schAppendAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������ϸ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSchAppendAdList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ȩ�����������ȸ
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		public void GetContractItemList(SchAppendAdModel schAppendAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ�����������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
				
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			

				// ����Ʈ �� ����
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             =  schAppendAdModel.SearchKey;               
				remoteData.SearchMediaCode		 =  schAppendAdModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 =  schAppendAdModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     =  schAppendAdModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  =  schAppendAdModel.SearchAdvertiserCode;
				remoteData.SearchchkAdState_10	 =  schAppendAdModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 =  schAppendAdModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 =  schAppendAdModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 =  schAppendAdModel.SearchchkAdState_40; 

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

				schAppendAdModel.FileListCount = remoteData.FileListCount;  // 2007.10.02 RH.Jung ���ϸ���Ʈ�Ǽ��˻�

				schAppendAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				schAppendAdModel.ResultCnt   = remoteData.ResultCnt;
				schAppendAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������ϸ����ȸ End");
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
		/// Ȩ���� ��ȸ(���������� ����Ʈ�Ͽ� DB�� ���� �����͸� �μ�Ʈ �ϱ�����...Ȩ ���̺� ����Ʈ)
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendList(SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ������ȸ Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				//				if(schAppendAdModel.ItemNo.Length < 1) 
				//				{
				//					throw new FrameException("���� ���õ��� �ʾҽ��ϴ�.");
				//				}

				// ������ �ν��Ͻ� ����
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			

				// ����Ʈ �� ����
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				//				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				//				remoteData.ItemNo          =  schAppendAdModel.ItemNo;
				//				remoteData.ItemName        =  schAppendAdModel.ItemName;

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSchAppendSearch(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				//schAppendAdModel.ScheduleOrder = remoteData.ScheduleOrder;
				schAppendAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				schAppendAdModel.ResultCnt     = remoteData.ResultCnt;
				schAppendAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ������ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ȩ�������߰�
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendAdAdd(SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ�������߰� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(schAppendAdModel.ItemNo.Length < 1) 
				{
					throw new FrameException("���� ���õ��� �ʾҽ��ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			

				// ����Ʈ �� ����
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				remoteData.ItemNo          =  schAppendAdModel.ItemNo;
				remoteData.ItemName        =  schAppendAdModel.ItemName;

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSchAppendAdCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schAppendAdModel.ScheduleOrder = remoteData.ScheduleOrder;
				schAppendAdModel.ResultCnt     = remoteData.ResultCnt;
				schAppendAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ�������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendAdAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ȩ������ ����
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendAdDelete_To(SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ���������� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				//				if(schAppendAdModel.ItemNo.Length < 1) 
				//				{
				//					throw new FrameException("���� ���õ��� �ʾҽ��ϴ�.");
				//				}

				// ������ �ν��Ͻ� ����
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			

				// ����Ʈ �� ����
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSchAppendAdDelete_To(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schAppendAdModel.ScheduleOrder = remoteData.ScheduleOrder;
				schAppendAdModel.ResultCnt     = remoteData.ResultCnt;
				schAppendAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ�������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendAdDelete_To():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ȩ������ �߰�
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendAdCreate_To(SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ�������߰� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				//				if(schAppendAdModel.ItemNo.Length < 1) 
				//				{
				//					throw new FrameException("���� ���õ��� �ʾҽ��ϴ�.");
				//				}

				// ������ �ν��Ͻ� ����
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			

				// ����Ʈ �� ����
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				remoteData.ItemNo          =  schAppendAdModel.ItemNo;
				remoteData.ScheduleOrder        =  schAppendAdModel.ScheduleOrder;

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSchAppendAdCreate_To(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schAppendAdModel.ScheduleOrder = remoteData.ScheduleOrder;
				schAppendAdModel.ResultCnt     = remoteData.ResultCnt;
				schAppendAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ�������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendAd_DeleteCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ȩ����������
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendAdDelete(SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ���������� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(schAppendAdModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			

				// ����Ʈ �� ����
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ

				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				remoteData.ItemNo          =  schAppendAdModel.ItemNo;
				remoteData.ItemName        =  schAppendAdModel.ItemName;
				remoteData.ScheduleOrder   =  schAppendAdModel.ScheduleOrder;
 
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSchAppendAdDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schAppendAdModel.ScheduleOrder = remoteData.ScheduleOrder;
				schAppendAdModel.ResultCnt     = remoteData.ResultCnt;
				schAppendAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ���������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendAdDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ȩ������ ù��° ���� ����
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendAdOrderSet(SchAppendAdModel schAppendAdModel, int OrderSet)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ������ ���� ���� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(schAppendAdModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
	
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			

				// ����Ʈ �� ����
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ

				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				remoteData.ItemNo          =  schAppendAdModel.ItemNo;
				remoteData.ItemName        =  schAppendAdModel.ItemName;
				remoteData.ScheduleOrder   =  schAppendAdModel.ScheduleOrder;
 
				switch(OrderSet)
				{
					case ORDER_FIRST:
						// ������ ȣ�� Ÿ�Ӿƿ�����
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// ������ �޼ҵ� ȣ��
						remoteData = svc.SetSchAppendAdOrderFirst(remoteHeader, remoteData);
						break;
					case ORDER_UP:
						// ������ ȣ�� Ÿ�Ӿƿ�����
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// ������ �޼ҵ� ȣ��
						remoteData = svc.SetSchAppendAdOrderUp(remoteHeader, remoteData);						
						break;
					case ORDER_DOWN:
						// ������ ȣ�� Ÿ�Ӿƿ�����
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// ������ �޼ҵ� ȣ��
						remoteData = svc.SetSchAppendAdOrderDown(remoteHeader, remoteData);						
						break;
					case ORDER_LAST:
						// ������ ȣ�� Ÿ�Ӿƿ�����
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// ������ �޼ҵ� ȣ��
						remoteData = svc.SetSchAppendAdOrderLast(remoteHeader, remoteData);
						break;
					default:
						throw new FrameException("�������� ������ ���õ��� �ʾҽ��ϴ�.");
				}


				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schAppendAdModel.ResultCnt      = remoteData.ResultCnt;
				schAppendAdModel.ResultCD       = remoteData.ResultCD;		
				schAppendAdModel.ScheduleOrder  = remoteData.ScheduleOrder;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ȩ������ ù��° ���� ���� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendAdOrderFirst():" + fe.ErrCode + ":" + fe.ResultMsg);
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
