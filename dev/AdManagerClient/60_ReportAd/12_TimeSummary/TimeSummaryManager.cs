// ===============================================================================
//
// TimeSummaryManager.cs
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
	public class TimeSummaryManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public TimeSummaryManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsDaily";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/TimeSummaryService.asmx";
		}

		/// <summary>
		/// ������ �ð���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAreaTime(TimeSummaryModel timeSummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������ �ð���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TimeSummaryServicePloxy.TimeSummaryService svc = new TimeSummaryServicePloxy.TimeSummaryService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				TimeSummaryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TimeSummaryServicePloxy.HeaderModel();
				TimeSummaryServicePloxy.TimeSummaryModel   remoteData   = new AdManagerClient.TimeSummaryServicePloxy.TimeSummaryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteHeader.UserLevel  = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.StartDay     = timeSummaryModel.StartDay;       
				remoteData.EndDay       = timeSummaryModel.EndDay;       
                remoteData.AdList       = timeSummaryModel.AdList.ToArray();
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAreaTime(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ				
				timeSummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				timeSummaryModel.ResultCnt     = remoteData.ResultCnt;
				timeSummaryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������ �ð���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAreaTime():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ���ں� �ð���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetDateTime(TimeSummaryModel timeSummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���ں� �ð���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TimeSummaryServicePloxy.TimeSummaryService svc = new TimeSummaryServicePloxy.TimeSummaryService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				TimeSummaryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TimeSummaryServicePloxy.HeaderModel();
				TimeSummaryServicePloxy.TimeSummaryModel   remoteData   = new AdManagerClient.TimeSummaryServicePloxy.TimeSummaryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ				  
 
				remoteData.StartDay     = timeSummaryModel.StartDay;       
				remoteData.EndDay       = timeSummaryModel.EndDay;       
                remoteData.AdList       = timeSummaryModel.AdList.ToArray();
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetDateTime(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ				
				timeSummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				timeSummaryModel.ResultCnt     = remoteData.ResultCnt;
				timeSummaryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���ں� �ð���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDateTime():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �帣�� ������� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetGenreTime(TimeSummaryModel timeSummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�帣�� �ð���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TimeSummaryServicePloxy.TimeSummaryService svc = new TimeSummaryServicePloxy.TimeSummaryService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				TimeSummaryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TimeSummaryServicePloxy.HeaderModel();
				TimeSummaryServicePloxy.TimeSummaryModel   remoteData   = new AdManagerClient.TimeSummaryServicePloxy.TimeSummaryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ				  
				remoteData.StartDay        = timeSummaryModel.StartDay;       
				remoteData.EndDay          = timeSummaryModel.EndDay;       
                remoteData.AdList       = timeSummaryModel.AdList.ToArray();
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGenreTime(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ				
				timeSummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				timeSummaryModel.ResultCnt     = remoteData.ResultCnt;
				timeSummaryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�帣�� �ð���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreTime():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �帣�� �ð���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetChannelTime(TimeSummaryModel timeSummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�κ� �ð���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TimeSummaryServicePloxy.TimeSummaryService svc = new TimeSummaryServicePloxy.TimeSummaryService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				TimeSummaryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TimeSummaryServicePloxy.HeaderModel();
				TimeSummaryServicePloxy.TimeSummaryModel   remoteData   = new AdManagerClient.TimeSummaryServicePloxy.TimeSummaryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ				  
				remoteData.StartDay        = timeSummaryModel.StartDay;       
				remoteData.EndDay          = timeSummaryModel.EndDay;       
                remoteData.AdList       = timeSummaryModel.AdList.ToArray();
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetChannelTime(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ				
				timeSummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				timeSummaryModel.ResultCnt     = remoteData.ResultCnt;
				timeSummaryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�κ� �ð���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelTime():" + fe.ErrCode + ":" + fe.ResultMsg);
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
