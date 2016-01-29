// ===============================================================================
// AdFile Manager  for Charites Project
//
// AdFileManager.cs
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
	/// ������������ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class AdFileManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>
		public AdFileManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "AdFile";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/AdFileService.asmx";
		}

		/// <summary>
		/// ��������������ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdFileList(AdFileModel adFileModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������ϸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel   remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             = adFileModel.SearchKey;
				remoteData.SearchMediaCode		 = adFileModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 = adFileModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     = adFileModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  = adFileModel.SearchAdvertiserCode;
				remoteData.SearchAdType			 = adFileModel.SearchAdType;       
				remoteData.SearchFileType		 = adFileModel.SearchFileType;
				remoteData.SearchchkAdState_10	 = adFileModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 = adFileModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 = adFileModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 = adFileModel.SearchchkAdState_40; 
				remoteData.SearchchkFileState_10 = adFileModel.SearchchkFileState_10; 
				remoteData.SearchchkFileState_11 = adFileModel.SearchchkFileState_11; 
				remoteData.SearchchkFileState_12 = adFileModel.SearchchkFileState_12; 
				remoteData.SearchchkFileState_15 = adFileModel.SearchchkFileState_15; 
				remoteData.SearchchkFileState_20 = adFileModel.SearchchkFileState_20; 
				remoteData.SearchchkFileState_30 = adFileModel.SearchchkFileState_30; 
				remoteData.SearchchkFileState_90 = adFileModel.SearchchkFileState_90; 
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				
                // ������ �޼ҵ� ȣ��
				remoteData = svc.GetAdFileList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileModel.AdFileDataSet = remoteData.AdFileDataSet.Copy();
				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������ϸ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdFileList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ��������������ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdFileSearch(AdFileModel adFileModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������ϸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel   remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             = adFileModel.SearchKey;
				remoteData.SearchMediaCode		 = adFileModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 = adFileModel.SearchRapCode;       
								
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAdFileSearch(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileModel.AdFileDataSet = remoteData.AdFileDataSet.Copy();
				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������ϸ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdFileSearch():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ���Ϲ����̷� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetPublishHistory(AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���Ϲ����̷���ȸ Start");
				_log.Debug("-----------------------------------------");
				
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();
				svc.Url = _WebServiceUrl;

				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel   remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				remoteHeader.ClientKey		= Header.ClientKey;
				remoteHeader.UserID			= Header.UserID;
				remoteHeader.UserLevel		= Header.UserLevel;

				remoteData.SearchMediaCode	= adFileModel.SearchMediaCode;	  
				remoteData.ItemNo			= adFileModel.ItemNo;       
								
				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.GetPublishHistory(remoteHeader, remoteData);

				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileModel.AdFileDataSet = remoteData.AdFileDataSet.Copy();
				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���Ϲ����̷���ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetPublishHistory():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� ��ü�̷� ��ȸ
		/// </summary>
		/// <param name="adFileModel"></param>
		public void GetFileRePublishHistory(AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���ϱ�ü�̷���ȸ Start");
				_log.Debug("-----------------------------------------");
				
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();
				svc.Url = _WebServiceUrl;

				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel   remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				remoteHeader.ClientKey		= Header.ClientKey;
				remoteHeader.UserID			= Header.UserID;
				remoteHeader.UserLevel		= Header.UserLevel;

				remoteData.SearchMediaCode	= adFileModel.SearchMediaCode;	  
				remoteData.ItemNo			= adFileModel.ItemNo;       
								
				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.GetFileRepHistory(remoteHeader, remoteData);

				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileModel.AdFileDataSet	= remoteData.AdFileDataSet.Copy();
				adFileModel.ResultCnt		= remoteData.ResultCnt;
				adFileModel.ResultCD		= remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���ϱ�ü�̷���ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetFileRePublishHistory():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ������������ ����
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileUpdate(AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���������������� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}
				if(adFileModel.FileName.Length > 40) 
				{
					throw new FrameException("�������ϸ��� ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel    remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.ItemNo	       = adFileModel.ItemNo;
				remoteData.ItemName        = adFileModel.ItemName;
				remoteData.FileType        = adFileModel.FileType;
				remoteData.FileLength      = adFileModel.FileLength;
				remoteData.FilePath        = adFileModel.FilePath;
				remoteData.PreFileName     = adFileModel.PreFileName;
				remoteData.FileName        = adFileModel.FileName;
				remoteData.DownLevel       = adFileModel.DownLevel;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���������������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAgntUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ������������ ����
		/// </summary>
		/// <param name="userModel"></param>
		public void SetFileUpdate(AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���������������� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel    remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.ItemNo	       = adFileModel.ItemNo;
				remoteData.newItemNo        = adFileModel.newItemNo;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetFileUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���������������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAgntUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// FTP���ε� ���� ��ȸ
		/// </summary>
		public void GetFtpConfig(AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("FTP���ε� ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel   remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetFtpConfig(remoteHeader, remoteData);


				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileModel.FtpUploadID   = remoteData.FtpUploadID;
				adFileModel.FtpUploadPW   = remoteData.FtpUploadPW;
				adFileModel.FtpUploadHost = remoteData.FtpUploadHost;
				adFileModel.FtpUploadPort = remoteData.FtpUploadPort;
				adFileModel.FtpUploadPath = remoteData.FtpUploadPath;
				adFileModel.FtpMovePath   = remoteData.FtpMovePath;
				adFileModel.FtpMoveUseYn  = remoteData.FtpMoveUseYn;
				adFileModel.FtpCdnID   = remoteData.FtpCdnID;
				adFileModel.FtpCdnPW   = remoteData.FtpCdnPW;
				adFileModel.FtpCdnHost = remoteData.FtpCdnHost;
				adFileModel.FtpCdnPort = remoteData.FtpCdnPort;
				adFileModel.CmsMasUrl	= remoteData.CmsMasUrl;
				adFileModel.CmsMasQuery = remoteData.CmsMasQuery;

				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("FTP���ε� ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetFtpConfig():" + fe.ErrCode + ":" + fe.ResultMsg);
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
