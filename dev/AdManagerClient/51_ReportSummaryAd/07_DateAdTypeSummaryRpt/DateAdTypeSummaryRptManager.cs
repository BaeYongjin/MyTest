// ===============================================================================
//
// DateAdTypeSummaryRptManager.cs
//
// �����ϰ�����Ʈ ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// DateAdTypeSummaryRptManager�� ���� ��� �����Դϴ�.
	/// </summary>
	public class DateAdTypeSummaryRptManager : BaseManager
	{
		public DateAdTypeSummaryRptManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyAdExecSummaryRpt";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/DateAdTypeSummaryRptService.asmx";
		}

		/// <summary>
		/// �Ϻ� ���������� ��ȸ
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetDateAdTypeSummaryRpt(DateAdTypeSummaryRptModel dateAdTypeSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�Ϻ� ���������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService svc = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				DateAdTypeSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.HeaderModel();
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel remoteData = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.LogDay1 = dateAdTypeSummaryRptModel.LogDay1;
				remoteData.LogDay2 = dateAdTypeSummaryRptModel.LogDay2;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetDateAdTypeSummaryRpt(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				dateAdTypeSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dateAdTypeSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				dateAdTypeSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				dateAdTypeSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�Ϻ� ���������� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDateAdTypeSummaryRpt():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �ֺ� ���������� ��ȸ
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetWeeklyAdTypeSummaryRpt(DateAdTypeSummaryRptModel dateAdTypeSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�ֺ� ���������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService svc = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				DateAdTypeSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.HeaderModel();
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel remoteData = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.LogDay1 = dateAdTypeSummaryRptModel.LogDay1;
				remoteData.LogDay2 = dateAdTypeSummaryRptModel.LogDay2;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetWeeklyAdTypeSummaryRpt(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				dateAdTypeSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dateAdTypeSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				dateAdTypeSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				dateAdTypeSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�ֺ� ���������� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDateAdTypeSummaryRpt():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ���� ���������� ��ȸ
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetMonthlyAdTypeSummaryRpt(DateAdTypeSummaryRptModel dateAdTypeSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���� ���������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService svc = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				DateAdTypeSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.HeaderModel();
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel remoteData = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.LogDay1 = dateAdTypeSummaryRptModel.LogDay1;
				remoteData.LogDay2 = dateAdTypeSummaryRptModel.LogDay2;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetMonthlyAdTypeSummaryRpt(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				dateAdTypeSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dateAdTypeSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				dateAdTypeSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				dateAdTypeSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���� ���������� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDateAdTypeSummaryRpt():" + fe.ErrCode + ":" + fe.ResultMsg);
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
