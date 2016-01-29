// ===============================================================================
//
// CodeManager.cs
//
// �����ڵ���ȸ ���񽺸� ȣ���մϴ�. 
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
	public class CodeManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public CodeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/CodeService.asmx";
		}

		/// <summary>
		/// �ڵ屸�и��
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetSectionList(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�ڵ屸�и�� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.SearchSection         = codeModel.SearchSection;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetSectionList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				codeModel.CodeDataSet = remoteData.CodeDataSet.Copy();
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�ڵ屸�и�� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSectionList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �ڵ���
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetCodeList(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = codeModel.SearchKey;
				remoteData.Section         = codeModel.Section;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetCodeList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				codeModel.Code = remoteData.Code;
				codeModel.CodeDataSet = remoteData.CodeDataSet.Copy();
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCodeList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �ڵ屸������ ����
		/// </summary>
		/// <param name="codeModel"></param>
		public void SetSectionUpdate(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�ڵ屸���������� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.Section       = codeModel.Section;
				remoteData.CodeName       = codeModel.CodeName;
				remoteData.Section_old     = codeModel.Section_old;				
											
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSectionUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�ڵ屸���������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSectionUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �ڵ����� ����
		/// </summary>
		/// <param name="codeModel"></param>
		public void SetCodeUpdate(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�ڵ��������� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.Code       = codeModel.Code;
				remoteData.CodeName       = codeModel.CodeName;
				remoteData.Section       = codeModel.Section;
				remoteData.Code_old     = codeModel.Code_old;				
											
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCodeUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�ڵ��������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCodeUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �ڵ����� ���
		/// </summary>
		/// <param name="codeModel"></param>
		public void SetCodeCreate(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�ڵ�������� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.Section       = codeModel.Section;
				remoteData.Code       = codeModel.Code;
				remoteData.CodeName       = codeModel.CodeName;				
															
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCodeCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�ڵ��������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCodeCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �ڵ����� ����
		/// </summary>
		/// <param name="codeModel"></param>
		public void SetCodeDelete(CodeModel codeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�ڵ��������� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				CodeServicePloxy.CodeService svc = new CodeServicePloxy.CodeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				CodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CodeServicePloxy.HeaderModel();
				CodeServicePloxy.CodeModel     remoteData   = new AdManagerClient.CodeServicePloxy.CodeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ				
				remoteData.Section       = codeModel.Section;
				remoteData.Code_old     = codeModel.Code_old;				
											
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCodeDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				codeModel.ResultCnt   = remoteData.ResultCnt;
				codeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�ڵ��������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCodeDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
