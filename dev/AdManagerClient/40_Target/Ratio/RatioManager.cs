// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// ä������ ���� ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
// 2007.06.26 �۸�ȯ v1.0
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
	public class RatioManager : BaseManager
	{
		/// <summary>
		/// ������
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
		/// �����޴����� ���� ��ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetSchChoiceMenuDetailList(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����޴����� ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();

				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.ItemNo             =  ratioModel.ItemNo;               
                
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
				ratioModel.MenuDataSet = remoteData.MenuDataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

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
		/// �׷�1�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetSchRateList(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�1�����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.ItemNo		   = ratioModel.ItemNo;
//				remoteData.EntrySeq         = ratioModel.EntrySeq;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetSchRateList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				ratioModel.SchRateDataSet = remoteData.SchRateDataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�1�����ȸ End");
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
		/// �׷�1�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetSchRateDetailList(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�1�����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
                remoteData.ItemNo       = ratioModel.ItemNo;
                remoteData.MediaCode    = ratioModel.MediaCode;
                remoteData.EntrySeq     = ratioModel.EntrySeq;
							
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetSchRateDetailList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				ratioModel.SchRateDetailDataSet = remoteData.SchRateDetailDataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�1�����ȸ End");
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
		/// �׷�1�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup1List(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�1�����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.ItemNo		   = ratioModel.ItemNo;
				remoteData.EntryYN       = ratioModel.EntryYN;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGroup1List(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				ratioModel.Group1DataSet = remoteData.Group1DataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�1�����ȸ End");
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
		/// �׷�2�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup2List(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�2�����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.ItemNo		   = ratioModel.ItemNo;
				remoteData.EntryYN       = ratioModel.EntryYN;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGroup2List(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				ratioModel.Group2DataSet = remoteData.Group2DataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�2�����ȸ End");
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
		/// �׷�3�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup3List(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�3�����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.ItemNo		   = ratioModel.ItemNo;
				remoteData.EntryYN       = ratioModel.EntryYN;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGroup3List(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				ratioModel.Group3DataSet = remoteData.Group3DataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�3�����ȸ End");
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
		/// �׷�4�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup4List(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�4�����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.ItemNo		   = ratioModel.ItemNo;
				remoteData.EntryYN       = ratioModel.EntryYN;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGroup4List(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				ratioModel.Group4DataSet = remoteData.Group4DataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�4�����ȸ End");
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
		/// �׷�2�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup5List(RatioModel ratioModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�5�����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.ItemNo		   = ratioModel.ItemNo;
				remoteData.EntryYN       = ratioModel.EntryYN;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGroup5List(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				ratioModel.Group5DataSet = remoteData.Group5DataSet.Copy();
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�5�����ȸ End");
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
		/// ��������
		/// </summary>
		/// <param name="ratioModel"></param>
		/// <returns></returns>
		public void SetSchRateUpdate(RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� Start");
				_log.Debug("-----------------------------------------");
				
				if(ratioModel.EntryRate.Length > 50) 
				{
					throw new FrameException("ä��No�� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}			
				
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.EntryRate     = ratioModel.EntryRate;	
				remoteData.ItemNo       = ratioModel.ItemNo;
				remoteData.MediaCode       = ratioModel.MediaCode;
				remoteData.EntrySeq       = ratioModel.EntrySeq;											
												
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSchRateUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� End");
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
		/// �����߰�
		/// </summary>
		/// <param name="ratioModel"></param>
		/// <returns></returns>
		public void SetSchRateCreate(RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����߰� Start");
				_log.Debug("-----------------------------------------");
				
				if(ratioModel.EntryRate.Length > 50) 
				{
					throw new FrameException("ä��No�� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}			
				
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.ItemNo       = ratioModel.ItemNo;
				remoteData.MediaCode       = ratioModel.MediaCode;
				remoteData.EntrySeq       = ratioModel.EntrySeq;
				remoteData.EntryRate     = ratioModel.EntryRate;								
												
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSchRateCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����߰� End");
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
		/// �����߰�
		/// </summary>
		/// <param name="ratioModel"></param>
		/// <returns></returns>
		public void SetSchRateDetailCreate(RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����߰� Start");
				_log.Debug("-----------------------------------------");
				
				if(ratioModel.EntryRate.Length > 50) 
				{
					throw new FrameException("ä��No�� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}			
				
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.ItemNo       = ratioModel.ItemNo;
				remoteData.MediaCode       = ratioModel.MediaCode;
				remoteData.EntrySeq       = ratioModel.EntrySeq;
				remoteData.CategoryCode     = ratioModel.CategoryCode;	
				remoteData.GenreCode     = ratioModel.GenreCode;				
												
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSchRateDetailCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����߰� End");
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
		/// ���� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetSchRateDelete(RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� start");
				_log.Debug("-----------------------------------------");
            
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
            				
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();
            
				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;
            
				// ȣ��������Ʈ
				remoteData.ItemNo       = ratioModel.ItemNo;
				remoteData.MediaCode       = ratioModel.MediaCode;
				remoteData.EntrySeq       = ratioModel.EntrySeq;
            					
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSchRateDelete(remoteHeader, remoteData);
            
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// ��� ��Ʈ
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� end");
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
		/// ���� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetSchRateDetailDelete(RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� start");
				_log.Debug("-----------------------------------------");
            
				// ������ �ν��Ͻ� ����
				RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
				svc.Url = _WebServiceUrl;
            				
				// ����Ʈ �� ����
				RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
				RatioServicePloxy.RatioModel remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();
            
				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;
            
				// ȣ��������Ʈ
				remoteData.ItemNo       = ratioModel.ItemNo;
				remoteData.MediaCode       = ratioModel.MediaCode;
				remoteData.EntrySeq       = ratioModel.EntrySeq;
				remoteData.CategoryCode     = ratioModel.CategoryCode;	
				remoteData.GenreCode     = ratioModel.GenreCode;
            					
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSchRateDetailDelete(remoteHeader, remoteData);
            
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// ��� ��Ʈ
				ratioModel.ResultCnt   = remoteData.ResultCnt;
				ratioModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� end");
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
        /// ����������
        /// </summary>
        /// <param name="ratioModel"></param>
        public void mDeleteSync(RatioModel ratioModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�������� start");
                _log.Debug("-----------------------------------------");
            
                // ������ �ν��Ͻ� ����
                RatioServicePloxy.RatioService svc = new RatioServicePloxy.RatioService();
                svc.Url = _WebServiceUrl;
            				
                // ����Ʈ �� ����
                RatioServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.RatioServicePloxy.HeaderModel();
                RatioServicePloxy.RatioModel    remoteData   = new AdManagerClient.RatioServicePloxy.RatioModel();
            
                // ������� ��Ʈ
                //remoteHeader.SearchKey     = Header.SearchKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ��������Ʈ
                remoteData.ItemNo       = ratioModel.ItemNo;
            					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.mDeleteSync(remoteHeader, remoteData);
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                ratioModel.ResultCnt   = remoteData.ResultCnt;
                ratioModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("�������� end");
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