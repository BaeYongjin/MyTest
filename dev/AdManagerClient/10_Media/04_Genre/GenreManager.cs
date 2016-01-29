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
	public class GenreManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public GenreManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";			
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/GenreService.asmx";
		}

		/// <summary>
		/// �����������ȸ
		/// </summary>
		/// <param name="genreModel"></param>
		public void GetGenreList(GenreModel genreModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				GenreServicePloxy.GenreService svc = new GenreServicePloxy.GenreService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GenreServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreServicePloxy.HeaderModel();
				GenreServicePloxy.GenreModel remoteData   = new AdManagerClient.GenreServicePloxy.GenreModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = genreModel.SearchKey;
				remoteData.SearchGenreLevel = genreModel.SearchGenreLevel;
				

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
				genreModel.UserDataSet = remoteData.UserDataSet.Copy();
				genreModel.ResultCnt   = remoteData.ResultCnt;
				genreModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="genreModel"></param>
		public void SetGenreUpdate(GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�
				/*if(genreModel.MediaCode.Length < 1) 
				{
					throw new FrameException("�����ID�� �������� �ʽ��ϴ�.");
				}*/
				if(genreModel.GenreName.Length > 20) 
				{
					throw new FrameException("�帣���� 20Bytes�� �ʰ��� �� �����ϴ�.");
				}
				/*if(genreModel.Charger.Length > 10) 
				{
					throw new FrameException("����ڸ��� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(genreModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}
				if(genreModel.Email.Length > 40) 
				{
					throw new FrameException("Email�� ���̴� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}*/
				

	

				// ������ �ν��Ͻ� ����
				GenreServicePloxy.GenreService svc = new GenreServicePloxy.GenreService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GenreServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreServicePloxy.HeaderModel();
				GenreServicePloxy.GenreModel remoteData   = new AdManagerClient.GenreServicePloxy.GenreModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = genreModel.MediaCode;
				remoteData.GenreCode       = genreModel.GenreCode;
				remoteData.GenreName     = genreModel.GenreName;				
				remoteData.ModDt     = genreModel.ModDt;
								
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGenreUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				genreModel.ResultCnt   = remoteData.ResultCnt;
				genreModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="genreModel"></param>
		/// <returns></returns>
		public void SetGenreAdd(GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� Start");
				_log.Debug("-----------------------------------------");

				/*if(genreModel.MediaCode.Trim().Length < 1) 
				{
					throw new FrameException("��ü�ڵ尡 �������� �ʽ��ϴ�.");
				}
				if(genreModel.MediaCode.Trim().Length > 10) 
				{
					throw new FrameException("��ü�ڵ�� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}*/
				if(genreModel.GenreName.Length > 20) 
				{
					throw new FrameException("�帣���� 20Bytes�� �ʰ��� �� �����ϴ�.");
				}
				/*if(genreModel.Charger.Length > 10) 
				{
					throw new FrameException("����ڸ��� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(genreModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}				
				if(genreModel.Email.Length > 40) 
				{
					throw new FrameException("Email�� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}*/


				// ������ �ν��Ͻ� ����
				GenreServicePloxy.GenreService svc = new GenreServicePloxy.GenreService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				GenreServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreServicePloxy.HeaderModel();
				GenreServicePloxy.GenreModel remoteData   = new AdManagerClient.GenreServicePloxy.GenreModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = genreModel.MediaCode;
				remoteData.GenreCode       = genreModel.GenreCode;
				remoteData.GenreName     = genreModel.GenreName;				
				remoteData.ModDt     = genreModel.ModDt;
				
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGenreCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				genreModel.ResultCnt   = remoteData.ResultCnt;
				genreModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				genreModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				genreModel.ResultCD    = "3101";
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
		public void SetGenreDelete(GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				GenreServicePloxy.GenreService svc = new GenreServicePloxy.GenreService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				GenreServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreServicePloxy.HeaderModel();
				GenreServicePloxy.GenreModel remoteData   = new AdManagerClient.GenreServicePloxy.GenreModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = genreModel.MediaCode;
				remoteData.GenreCode       = genreModel.GenreCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGenreDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				genreModel.ResultCnt   = remoteData.ResultCnt;
				genreModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGenreDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
