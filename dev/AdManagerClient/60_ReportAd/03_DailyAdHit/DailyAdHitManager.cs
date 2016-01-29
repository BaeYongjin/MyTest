// ===============================================================================
//
// DailyAdHitManager.cs
//
// �Ϻ� ���� ��ûȽ�� ���� ��ȸ ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
//
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
	/// �Ϻ� ���� ��ûȽ������ ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class DailyAdHitManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public DailyAdHitManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyAdHit";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/DailyAdHitService.asmx";
		}

		/// <summary>
		/// �Ϻ� ���� ��ûȽ������ ��ȸ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetDailyAdHitReport(DailyAdHitModel dailyAdHitModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�Ϻ� ���� ��ûȽ�� ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				DailyAdHitServicePloxy.DailyAdHitService svc = new DailyAdHitServicePloxy.DailyAdHitService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				DailyAdHitServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.DailyAdHitServicePloxy.HeaderModel();
				DailyAdHitServicePloxy.DailyAdHitModel  remoteData   = new AdManagerClient.DailyAdHitServicePloxy.DailyAdHitModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode		 = dailyAdHitModel.SearchMediaCode;	  
				remoteData.SearchContractSeq	 = dailyAdHitModel.SearchContractSeq;	  
				remoteData.SearchItemNo  		 = dailyAdHitModel.SearchItemNo;	
				remoteData.CampaignCode  		 = dailyAdHitModel.CampaignCode;	
				remoteData.SearchType 			 = dailyAdHitModel.SearchType;       
				remoteData.SearchBgnDay 		 = dailyAdHitModel.SearchBgnDay;       
				remoteData.SearchEndDay 		 = dailyAdHitModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetDailyAdHit(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				dailyAdHitModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dailyAdHitModel.HeaderDataSet = remoteData.HeaderDataSet.Copy();
				dailyAdHitModel.ResultCnt     = remoteData.ResultCnt;
				dailyAdHitModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�Ϻ� ���� ��ûȽ�� ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDailyAdHit():" + fe.ErrCode + ":" + fe.ResultMsg);
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
