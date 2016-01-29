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
	public class ChannelSetManager : BaseManager
	{
		/// <summary>
		/// ������
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
		/// �̵���޺�������ȸ
		/// </summary>
		/// <param name="channelSetModel"></param>
		public void GetCategoryList(ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ��޺� �����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.CategoryCode = channelSetModel.MediaCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetCategoryList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				channelSetModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ��޺� �����ȸ End");
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
		/// �̵���޺�������ȸ
		/// </summary>
		/// <param name="channelSetModel"></param>
		public void GetGenreList(ChannelSetModel channelSetModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�帣�޺� �����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// ������� ��Ʈ				
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.SearchKey = channelSetModel.SearchKey;
				remoteData.GenreCode = channelSetModel.GenreCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGenreList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				channelSetModel.GenreDataSet = remoteData.GenreDataSet.Copy();
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�帣�޺� �����ȸ End");
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
		/// ä��������ȸ
		/// </summary>
		/// <param name="channelSetModel"></param>
		public void GetChannelSetList(ChannelSetModel channelSetModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�θ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = channelSetModel.SearchKey;
				remoteData.MediaCode       = channelSetModel.MediaCode;
				remoteData.CategoryCode       = channelSetModel.CategoryCode;
				remoteData.GenreCode       = channelSetModel.GenreCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetChannelSetList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				channelSetModel.ChannelSetDataSet = remoteData.ChannelSetDataSet.Copy();
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�θ����ȸ End");
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
		/// ä�� ������ �����ȸ
		/// </summary>
		/// <param name="channelSetModel"></param>
		public void GetCategenList(ChannelSetModel channelSetModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�� ������ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = channelSetModel.SearchKey;
				remoteData.SearchMediaName       = channelSetModel.SearchMediaName;
				remoteData.SearchCategoryName       = channelSetModel.SearchCategoryName;
				remoteData.SearchGenreName       = channelSetModel.SearchGenreName;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetCategenList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				channelSetModel.ChannelSetDataSet = remoteData.ChannelSetDataSet.Copy();
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�� ������ �����ȸ End");
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
				_log.Debug("�������˾� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = channelSetModel.SearchKey;
				remoteData.MediaCode_P       = channelSetModel.MediaCode_P;
				remoteData.CategoryCode_P       = channelSetModel.CategoryCode_P;
				remoteData.GenreCode_P       = channelSetModel.GenreCode_P;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetChannelNoPopList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				channelSetModel.ChannelSetDataSet = remoteData.ChannelSetDataSet.Copy();
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������˾� �����ȸ End");
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
		/// ä�μ� ����
		/// </summary>
		/// <param name="channelSetModel"></param>
		public void SetChannelSetUpdate(ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�μ��������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�
				if(channelSetModel.ChannelNo.Length > 50) 
				{
					throw new FrameException("ä�ι�ȣ�� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}				

				// ������ �ν��Ͻ� ����
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.ChannelNo       = channelSetModel.ChannelNo;
				remoteData.SeriesNo     = channelSetModel.SeriesNo;				
				remoteData.MediaCode       = channelSetModel.MediaCode;
				remoteData.CategoryCode       = channelSetModel.CategoryCode;
				remoteData.GenreCode       = channelSetModel.GenreCode;
				
				remoteData.MediaCode_old       = channelSetModel.MediaCode_old;
				remoteData.CategoryCode_old       = channelSetModel.CategoryCode_old;
				remoteData.GenreCode_old       = channelSetModel.GenreCode_old;
				remoteData.ChannelNo_old       = channelSetModel.ChannelNo_old;
								
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetChannelSetUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�μ��������� End");
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
		/// �����ü���౤�����߰�
		/// </summary>
		/// <param name="channelSetModel"></param>
		/// <returns></returns>
		public void SetChannelSetAdd(ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�μ��߰� Start");
				_log.Debug("-----------------------------------------");
				
				if(channelSetModel.ChannelNo.Length > 50) 
				{
					throw new FrameException("ä��No�� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}			
				
				// ������ �ν��Ͻ� ����
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = channelSetModel.MediaCode;
				remoteData.CategoryCode       = channelSetModel.CategoryCode;
				remoteData.ChannelNo       = channelSetModel.ChannelNo;
				remoteData.SeriesNo     = channelSetModel.SeriesNo;								
				remoteData.GenreCode       = channelSetModel.GenreCode;		
								
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetChannelSetCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�μ��߰� End");
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
		/// ä�μ� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetChannelSetDelete(ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�μ»��� start");
				_log.Debug("-----------------------------------------");
            
				// ������ �ν��Ͻ� ����
				ChannelSetServicePloxy.ChannelSetService svc = new ChannelSetServicePloxy.ChannelSetService();
				svc.Url = _WebServiceUrl;
            				
				// ����Ʈ �� ����
				ChannelSetServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelSetServicePloxy.HeaderModel();
				ChannelSetServicePloxy.ChannelSetModel remoteData   = new AdManagerClient.ChannelSetServicePloxy.ChannelSetModel();
            
				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;
            
				// ȣ��������Ʈ
				remoteData.MediaCode       = channelSetModel.MediaCode;
				remoteData.CategoryCode       = channelSetModel.CategoryCode;
				remoteData.GenreCode       = channelSetModel.GenreCode;		
				remoteData.ChannelNo       = channelSetModel.ChannelNo;		
            					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetChannelSetDelete(remoteHeader, remoteData);
            
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// ��� ��Ʈ
				channelSetModel.ResultCnt   = remoteData.ResultCnt;
				channelSetModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�μ»��� end");
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