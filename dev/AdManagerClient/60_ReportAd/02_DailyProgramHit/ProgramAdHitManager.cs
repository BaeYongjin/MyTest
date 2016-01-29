// ===============================================================================
//
// ProgramAdHitManager.cs
//
// ���α׷��� ��ûȽ������ ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// ���α׷��� ��ûȽ������ ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class ProgramAdHitManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public ProgramAdHitManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "ProgramAdHit";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/ProgramAdHitService.asmx";
		}

		/// <summary>
		/// ���α׷��� �����û���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetProgramAdHitReport(ProgramAdHitModel programAdHitModel)
		{
			try
			{
				_log.Debug("---------------------------------------");
				_log.Debug("���α׷��� �����û���� ��ȸ Start"     );
				_log.Debug("---------------------------------------");
				
				ProgramAdHitServicePloxy.ProgramAdHitService svc = new ProgramAdHitServicePloxy.ProgramAdHitService();
				svc.Url = _WebServiceUrl;

				ProgramAdHitServicePloxy.HeaderModel        remoteHeader = new AdManagerClient.ProgramAdHitServicePloxy.HeaderModel();
				ProgramAdHitServicePloxy.ProgramAdHitModel  remoteData   = new AdManagerClient.ProgramAdHitServicePloxy.ProgramAdHitModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode		 = programAdHitModel.SearchMediaCode;	  
				remoteData.SearchContractSeq	 = programAdHitModel.SearchContractSeq;	  
				remoteData.SearchItemNo  		 = programAdHitModel.SearchItemNo;	  
				remoteData.CampaignCode  		 = programAdHitModel.CampaignCode;	  
				remoteData.SearchType 			 = programAdHitModel.SearchType;       
				remoteData.SearchBgnDay 		 = programAdHitModel.SearchBgnDay;       
				remoteData.SearchEndDay 		 = programAdHitModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetProgramAdHit(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				programAdHitModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				programAdHitModel.ResultCnt     = remoteData.ResultCnt;
				programAdHitModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���α׷��� ��ûȽ������ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetProgramAdHit():" + fe.ErrCode + ":" + fe.ResultMsg);
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
