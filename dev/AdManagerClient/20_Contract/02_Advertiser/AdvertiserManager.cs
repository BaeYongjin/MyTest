// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
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
	public class AdvertiserManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AdvertiserManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/AdvertiserService.asmx";
		}

		/// <summary>
		/// �����������ȸ
		/// </summary>
		/// <param name="advertiserModel"></param>
		public void GetAdvertiserList(AdvertiserModel advertiserModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AdvertiserServicePloxy.AdvertiserService svc = new AdvertiserServicePloxy.AdvertiserService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdvertiserServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdvertiserServicePloxy.HeaderModel();
				AdvertiserServicePloxy.AdvertiserModel remoteData   = new AdManagerClient.AdvertiserServicePloxy.AdvertiserModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = advertiserModel.SearchKey;
				remoteData.SearchAdvertiserLevel = advertiserModel.SearchAdvertiserLevel;
				remoteData.SearchchkAdState_10	 = advertiserModel.SearchchkAdState_10; 
				remoteData.SearchRap         = advertiserModel.SearchRap;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAdvertiserList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				advertiserModel.AdvertiserDataSet = remoteData.AdvertiserDataSet.Copy();
				advertiserModel.ResultCnt   = remoteData.ResultCnt;
				advertiserModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetUserList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public bool GetUserDetail(BaseModel baseModel)
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
		/// <param name="advertiserModel"></param>
		public void SetAdvertiserUpdate(AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�
				/*if(advertiserModel.MediaCode.Length < 1) 
				{
					throw new FrameException("�����ID�� �������� �ʽ��ϴ�.");
				}*/
				if(advertiserModel.AdvertiserName.Length > 40) 
				{
					throw new FrameException("�����ָ��� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(advertiserModel.Comment.Length > 50) 
				{
					throw new FrameException("������ ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}
				/*if(advertiserModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}
				if(advertiserModel.Email.Length > 40) 
				{
					throw new FrameException("Email�� ���̴� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}*/
				

	

				// ������ �ν��Ͻ� ����
				AdvertiserServicePloxy.AdvertiserService svc = new AdvertiserServicePloxy.AdvertiserService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdvertiserServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdvertiserServicePloxy.HeaderModel();
				AdvertiserServicePloxy.AdvertiserModel remoteData   = new AdManagerClient.AdvertiserServicePloxy.AdvertiserModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				//remoteData.MediaCode      = advertiserModel.MediaCode;
				remoteData.AdvertiserCode   = advertiserModel.AdvertiserCode;
				remoteData.AdvertiserName   = advertiserModel.AdvertiserName;		
				remoteData.RapCode          = advertiserModel.RapCode;
                remoteData.JobCode          = advertiserModel.JobCode;
				remoteData.Comment          = advertiserModel.Comment;				
				remoteData.UseYn            = advertiserModel.UseYn;				
				remoteData.ModDt            = advertiserModel.ModDt;
								
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdvertiserUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				advertiserModel.ResultCnt   = remoteData.ResultCnt;
				advertiserModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// <param name="advertiserModel"></param>
		/// <returns></returns>
		public void SetAdvertiserAdd(AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� Start");
				_log.Debug("-----------------------------------------");

				/*if(advertiserModel.MediaCode.Trim().Length < 1) 
				{
					throw new FrameException("��ü�ڵ尡 �������� �ʽ��ϴ�.");
				}
				if(advertiserModel.MediaCode.Trim().Length > 10) 
				{
					throw new FrameException("��ü�ڵ�� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}*/
				if(advertiserModel.AdvertiserName.Length > 40) 
				{
					throw new FrameException("�����ָ��� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(advertiserModel.Comment.Length > 50) 
				{
					throw new FrameException("������ ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}
				/*if(advertiserModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}				
				if(advertiserModel.Email.Length > 40) 
				{
					throw new FrameException("Email�� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}*/


				// ������ �ν��Ͻ� ����
				AdvertiserServicePloxy.AdvertiserService svc = new AdvertiserServicePloxy.AdvertiserService();
				
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdvertiserServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdvertiserServicePloxy.HeaderModel();
				AdvertiserServicePloxy.AdvertiserModel remoteData   = new AdManagerClient.AdvertiserServicePloxy.AdvertiserModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				//remoteData.MediaCode      = advertiserModel.MediaCode;
				remoteData.AdvertiserCode   = advertiserModel.AdvertiserCode;
				remoteData.AdvertiserName   = advertiserModel.AdvertiserName;	
				remoteData.RapCode          = advertiserModel.RapCode;
                remoteData.JobCode          = advertiserModel.JobCode;
				remoteData.Comment          = advertiserModel.Comment;				
				remoteData.UseYn            = advertiserModel.UseYn;
				remoteData.RegDt            = advertiserModel.RegDt;
				
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdvertiserCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				advertiserModel.ResultCnt   = remoteData.ResultCnt;
				advertiserModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				advertiserModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				advertiserModel.ResultCD    = "3101";
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
		public void SetAdvertiserDelete(AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				AdvertiserServicePloxy.AdvertiserService svc = new AdvertiserServicePloxy.AdvertiserService();
				
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdvertiserServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdvertiserServicePloxy.HeaderModel();
				AdvertiserServicePloxy.AdvertiserModel remoteData   = new AdManagerClient.AdvertiserServicePloxy.AdvertiserModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ��������Ʈ
				//remoteData.MediaCode       = advertiserModel.MediaCode;
				remoteData.AdvertiserCode       = advertiserModel.AdvertiserCode;
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdvertiserDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				advertiserModel.ResultCnt   = remoteData.ResultCnt;
				advertiserModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdvertiserDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ������ȸ
        /// </summary>
        /// <param name="contractModel"></param>
        public void GetJobClassList(AdvertiserModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���������ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                AdvertiserServicePloxy.AdvertiserService svc = new AdvertiserServicePloxy.AdvertiserService();

                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                AdvertiserServicePloxy.HeaderModel remoteHeader = new AdvertiserServicePloxy.HeaderModel();
                AdvertiserServicePloxy.AdvertiserModel remoteData = new AdvertiserServicePloxy.AdvertiserModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetJobClassList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.AdvertiserDataSet = remoteData.AdvertiserDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD  = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("���������ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetJobClassList():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }
	}
}
