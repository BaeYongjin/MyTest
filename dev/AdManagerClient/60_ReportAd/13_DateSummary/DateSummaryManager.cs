// ===============================================================================
//
// DateSummaryManager.cs
//
// ������Ʈ �Ϻ���� ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// ������Ʈ �Ϻ���� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class DateSummaryManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public DateSummaryManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsDaily";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/DateSummaryService.asmx";
		}

		/// <summary>
		/// ������ �ð���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetDateGenre(DateSummaryModel dateSummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������ �ð���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				DateSummaryServicePloxy.DateSummaryService svc = new DateSummaryServicePloxy.DateSummaryService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				DateSummaryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.DateSummaryServicePloxy.HeaderModel();
				DateSummaryServicePloxy.DateSummaryModel   remoteData   = new AdManagerClient.DateSummaryServicePloxy.DateSummaryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.StartDay     = dateSummaryModel.StartDay;       
				remoteData.EndDay       = dateSummaryModel.EndDay;       
                remoteData.AdList       = dateSummaryModel.AdList.ToArray();
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetDateGenre(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ								
				dateSummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dateSummaryModel.ResultCnt     = remoteData.ResultCnt;
				dateSummaryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������ �ð���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDateGenre():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �Ⱓ�� ä����� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetDateChannel(DateSummaryModel dateSummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�Ⱓ�� ä����� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				DateSummaryServicePloxy.DateSummaryService svc = new DateSummaryServicePloxy.DateSummaryService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				DateSummaryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.DateSummaryServicePloxy.HeaderModel();
				DateSummaryServicePloxy.DateSummaryModel   remoteData   = new AdManagerClient.DateSummaryServicePloxy.DateSummaryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ				  
				remoteData.StartDay     = dateSummaryModel.StartDay;       
				remoteData.EndDay       = dateSummaryModel.EndDay;       
                remoteData.AdList       = dateSummaryModel.AdList.ToArray();
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetDateChannel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ				
				dateSummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dateSummaryModel.ResultCnt     = remoteData.ResultCnt;
				dateSummaryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�Ⱓ�� ä����� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDateChannel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
