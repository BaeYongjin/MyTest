// ===============================================================================
// AdFile Manager  for Charites Project
//
// AdStatusManager.cs
//
// �������Ϲ������� ���񽺸� ȣ���մϴ�. 
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
	/// �������Ϲ������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class AdStatusManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AdStatusManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "AdStatus";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Schedule/AdStatusService.asmx";
		}

		/// <summary>
		/// �������Ϲ���������ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdStatusList(AdStatusModel adStatusModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������Ϲ��������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AdStatusServicePloxy.AdStatusService svc = new AdStatusServicePloxy.AdStatusService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdStatusServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdStatusServicePloxy.HeaderModel();
				AdStatusServicePloxy.AdStatusModel   remoteData   = new AdManagerClient.AdStatusServicePloxy.AdStatusModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             = adStatusModel.SearchKey;
				remoteData.SearchMediaCode		 = adStatusModel.SearchMediaCode;	  				
				remoteData.SearchchkAdState_10	 = adStatusModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 = adStatusModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 = adStatusModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 = adStatusModel.SearchchkAdState_40; 
				remoteData.SearchFileState_20	 = adStatusModel.SearchFileState_20;				
				remoteData.SearchFileState_30	 = adStatusModel.SearchFileState_30;				
				remoteData.SearchFileState_90	 = adStatusModel.SearchFileState_90;				
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 10;
				// ������ �޼ҵ� ȣ��
                _log.Debug("����ȣ�����");
                remoteData = svc.GetAdStatusList_Comp(remoteHeader, remoteData);
                _log.Debug("����ȣ�ⳡ");

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                

				// ��� ��Ʈ
				//adStatusModel.AdStatusDataSet = remoteData.AdStatusDataSet.Copy();
                adStatusModel.AdStatusDataSet = FrameSystem.DeCompressData(Convert.FromBase64String(remoteData.FileName));
				adStatusModel.ResultCnt   = remoteData.ResultCnt;
				adStatusModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������Ϲ��������ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdStatusList():" + fe.ErrCode + ":" + fe.ResultMsg);
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