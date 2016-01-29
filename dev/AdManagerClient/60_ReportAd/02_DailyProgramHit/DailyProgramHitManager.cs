// ===============================================================================
//
// DailyProgramHitManager.cs
//
// �Ϻ� ���α׷� ��ûȽ������ ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// �Ϻ� ���α׷� ��ûȽ������ ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class DailyProgramHitManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public DailyProgramHitManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyProgramHit";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/DailyProgramHitService.asmx";
		}

		/// <summary>
		/// �Ϻ� ���α׷� ��ûȽ������ ��ȸ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetDailyProgramHitReport(DailyProgramHitModel dailyProgramHitModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�Ϻ� ���α׷� ��ûȽ������ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				DailyProgramHitServicePloxy.DailyProgramHitService svc = new DailyProgramHitServicePloxy.DailyProgramHitService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				DailyProgramHitServicePloxy.HeaderModel          remoteHeader = new AdManagerClient.DailyProgramHitServicePloxy.HeaderModel();
				DailyProgramHitServicePloxy.DailyProgramHitModel   remoteData   = new AdManagerClient.DailyProgramHitServicePloxy.DailyProgramHitModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode		 = dailyProgramHitModel.SearchMediaCode;	  
				remoteData.SearchContractSeq	 = dailyProgramHitModel.SearchContractSeq;	  
				remoteData.SearchItemNo  		 = dailyProgramHitModel.SearchItemNo;	
				remoteData.CampaignCode  		 = dailyProgramHitModel.CampaignCode;	
				remoteData.SearchType 			 = dailyProgramHitModel.SearchType;       
				remoteData.SearchBgnDay 		 = dailyProgramHitModel.SearchBgnDay;       
				remoteData.SearchEndDay 		 = dailyProgramHitModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetDailyProgramHit(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				dailyProgramHitModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dailyProgramHitModel.HeaderDataSet = remoteData.HeaderDataSet.Copy();
				dailyProgramHitModel.ResultCnt     = remoteData.ResultCnt;
				dailyProgramHitModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�Ϻ� ���α׷� ��ûȽ������ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDailyProgramHit():" + fe.ErrCode + ":" + fe.ResultMsg);
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
