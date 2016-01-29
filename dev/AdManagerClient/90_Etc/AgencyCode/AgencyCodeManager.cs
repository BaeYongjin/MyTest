// ===============================================================================
//
// AgencyCodeManager.cs
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

/*
 * -------------------------------------------------------
 * Class Name: AgencyCodeManager
 * �ֿ���  : �׷��������� ���� ȣ��
 * �ۼ���    : ��
 * �ۼ���    : ��
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : �躸��
 * ������    : 2013.02
 * ��������  :        
 *            - �˻��� ��ȸ ��� �߰�
 * �����Լ�  :
 *            - GetAgencyCodeList
 * --------------------------------------------------------
 */

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
	public class AgencyCodeManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AgencyCodeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/AgencyCodeService.asmx";
		}

		/// <summary>
		/// ����� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetAgencyCodeList(AgencyCodeModel agencycodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				AgencyCodeServicePloxy.AgencyCodeService svc = new AgencyCodeServicePloxy.AgencyCodeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AgencyCodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AgencyCodeServicePloxy.HeaderModel();
				AgencyCodeServicePloxy.AgencyCodeModel     remoteData   = new AdManagerClient.AgencyCodeServicePloxy.AgencyCodeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchRap         = agencycodeModel.SearchRap;
				remoteData.AgencyCode        = agencycodeModel.AgencyCode;
                remoteData.SearchKey         = agencycodeModel.SearchKey;  // [E_01]
				 
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAgencyCodeList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				agencycodeModel.AgencyCodeDataSet = remoteData.AgencyCodeDataSet.Copy();
				agencycodeModel.ResultCnt   = remoteData.ResultCnt;
				agencycodeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				//_log.Warning( this.ToString() + ":GetAgencyCodeList():" + fe.ErrAgencyCode + ":" + fe.ResultMsg);
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
