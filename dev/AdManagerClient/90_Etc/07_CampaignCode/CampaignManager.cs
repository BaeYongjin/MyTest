// ===============================================================================
//
// CampaignCodeManager.cs
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
	public class CampaignCodeManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public CampaignCodeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/CampaignCodeService.asmx";
		}

		/// <summary>
		/// ����� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetCampaignCodeList(CampaignCodeModel campaigncodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				CampaignCodeServicePloxy.CampaignCodeService svc = new CampaignCodeServicePloxy.CampaignCodeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				CampaignCodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignCodeServicePloxy.HeaderModel();
				CampaignCodeServicePloxy.CampaignCodeModel     remoteData   = new AdManagerClient.CampaignCodeServicePloxy.CampaignCodeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.MediaCode         = campaigncodeModel.MediaCode;
				remoteData.ContractSeq         = campaigncodeModel.ContractSeq;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetCampaignCodeList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				campaigncodeModel.CampaignCodeDataSet = remoteData.CampaignCodeDataSet.Copy();
				campaigncodeModel.ResultCnt   = remoteData.ResultCnt;
				campaigncodeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				//_log.Warning( this.ToString() + ":GetMediaRapCodeList():" + fe.ErrMediaRapCode + ":" + fe.ResultMsg);
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
