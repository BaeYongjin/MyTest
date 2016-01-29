// ===============================================================================
// MediaRapUpdate Manager  for Charites Project
//
// MediaRapUpdateManager.cs
//
// ��������� ���� ���񽺸� ȣ���մϴ�. 
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
	/// ��������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class MediaRapManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public MediaRapManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/MediaRapService.asmx";
		}

		/// <summary>
		/// �����������ȸ
		/// </summary>
		/// <param name="mediarapModel"></param>
		public void GetMediaRapList(MediaRapModel mediarapModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				MediaRapServicePloxy.MediaRapService svc = new MediaRapServicePloxy.MediaRapService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				MediaRapServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaRapServicePloxy.HeaderModel();
				MediaRapServicePloxy.MediaRapModel remoteData   = new AdManagerClient.MediaRapServicePloxy.MediaRapModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = mediarapModel.SearchKey;
				remoteData.SearchMediaRap = mediarapModel.SearchMediaRap;
				remoteData.SearchchkAdState_10	 = mediarapModel.SearchchkAdState_10; 

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetMediaRapList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				mediarapModel.MediaRapDataSet = remoteData.MediaRapDataSet.Copy();
				mediarapModel.ResultCnt   = remoteData.ResultCnt;
				mediarapModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetMediaRapList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Service ȣ���� ���� �޼ҵ�
		/// </summary>
		public bool GetMediaRapDetail(BaseModel baseModel)
		{
			
			_log.Debug("-----------------------------------------");
			_log.Debug( this.ToString() + " Start");
			_log.Debug("-----------------------------------------");

			_log.Debug("-----------------------------------------");
			_log.Debug( this.ToString() + " End");
			_log.Debug("-----------------------------------------");

			return true;
		}

		/// <summary>
		/// ��������� ����
		/// </summary>
		/// <param name="mediarapModel"></param>
		public void SetMediaRapUpdate(MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�
				if(mediarapModel.RapCode.Length < 1) 
				{
					throw new FrameException("��ID�� �������� �ʽ��ϴ�.");
				}
				if(mediarapModel.RapName.Length > 10) 
				{
					throw new FrameException("������ ���̴� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				
				if(mediarapModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}				
				if(mediarapModel.Comment.Length > 50) 
				{
					throw new FrameException("������ ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}

	

				// ������ �ν��Ͻ� ����
				MediaRapServicePloxy.MediaRapService svc = new MediaRapServicePloxy.MediaRapService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				MediaRapServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaRapServicePloxy.HeaderModel();
				MediaRapServicePloxy.MediaRapModel remoteData   = new AdManagerClient.MediaRapServicePloxy.MediaRapModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.RapCode       = mediarapModel.RapCode;
				remoteData.RapName     = mediarapModel.RapName;
				remoteData.RapType = mediarapModel.RapType;
				remoteData.Tell     = mediarapModel.Tell;				
				remoteData.Comment  = mediarapModel.Comment;		
				remoteData.UseYn     = mediarapModel.UseYn;				
				

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetMediaRapUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				mediarapModel.ResultCnt   = remoteData.ResultCnt;
				mediarapModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMediaRapUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ������߰�
		/// </summary>
		/// <param name="mediarapModel"></param>
		/// <returns></returns>
		public void SetMediaRapAdd(MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� Start");
				_log.Debug("-----------------------------------------");
				
				if(mediarapModel.RapName.Length > 10) 
				{
					throw new FrameException("������ ���̴� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				
				if(mediarapModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}				
				if(mediarapModel.Comment.Length > 50) 
				{
					throw new FrameException("������ ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				MediaRapServicePloxy.MediaRapService svc = new MediaRapServicePloxy.MediaRapService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				MediaRapServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaRapServicePloxy.HeaderModel();
				MediaRapServicePloxy.MediaRapModel remoteData   = new AdManagerClient.MediaRapServicePloxy.MediaRapModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.RapCode       = mediarapModel.RapCode;
				remoteData.RapName     = mediarapModel.RapName;
				remoteData.RapType = mediarapModel.RapType;
				remoteData.Tell     = mediarapModel.Tell;				
				remoteData.Comment  = mediarapModel.Comment;		
				remoteData.UseYn     = mediarapModel.UseYn;				
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetMediaRapCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				mediarapModel.ResultCnt   = remoteData.ResultCnt;
				mediarapModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMediaRapCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetMediaRapDelete(MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				MediaRapServicePloxy.MediaRapService svc = new MediaRapServicePloxy.MediaRapService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				MediaRapServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaRapServicePloxy.HeaderModel();
				MediaRapServicePloxy.MediaRapModel remoteData   = new AdManagerClient.MediaRapServicePloxy.MediaRapModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				// ȣ��������Ʈ
				remoteData.RapCode       = mediarapModel.RapCode;
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetMediaRapDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				mediarapModel.ResultCnt   = remoteData.ResultCnt;
				mediarapModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMediaRapDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
