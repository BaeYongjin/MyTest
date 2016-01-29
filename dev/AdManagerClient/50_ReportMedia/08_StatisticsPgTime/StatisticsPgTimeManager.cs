// ===============================================================================
//
// StatisticsPgTimeManager.cs
//
// ����������Ʈ �ð��뺰��� ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// ����������Ʈ �ð��뺰��� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class StatisticsPgTimeManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsPgTimeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsPgTime";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgTimeService.asmx";
		}

		/// <summary>
		/// ī�װ���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetCategoryList(StatisticsPgTimeModel statisticsPgTimeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgTimeServicePloxy.StatisticsPgTimeService svc = new StatisticsPgTimeServicePloxy.StatisticsPgTimeService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgTimeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgTimeServicePloxy.HeaderModel();
				StatisticsPgTimeServicePloxy.StatisticsPgTimeModel   remoteData   = new AdManagerClient.StatisticsPgTimeServicePloxy.StatisticsPgTimeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgTimeModel.SearchMediaCode;	  
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetCategoryList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgTimeModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
				statisticsPgTimeModel.ResultCnt       = remoteData.ResultCnt;
				statisticsPgTimeModel.ResultCD        = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �帣��� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetGenreList(StatisticsPgTimeModel statisticsPgTimeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�帣��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgTimeServicePloxy.StatisticsPgTimeService svc = new StatisticsPgTimeServicePloxy.StatisticsPgTimeService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgTimeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgTimeServicePloxy.HeaderModel();
				StatisticsPgTimeServicePloxy.StatisticsPgTimeModel   remoteData   = new AdManagerClient.StatisticsPgTimeServicePloxy.StatisticsPgTimeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgTimeModel.SearchMediaCode;	  
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGenreList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgTimeModel.GenreDataSet = remoteData.GenreDataSet.Copy();
				statisticsPgTimeModel.ResultCnt    = remoteData.ResultCnt;
				statisticsPgTimeModel.ResultCD     = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�帣��� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����������Ʈ �ð��뺰��� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgTimeReport(StatisticsPgTimeModel statisticsPgTimeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ �ð��뺰��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgTimeServicePloxy.StatisticsPgTimeService svc = new StatisticsPgTimeServicePloxy.StatisticsPgTimeService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgTimeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgTimeServicePloxy.HeaderModel();
				StatisticsPgTimeServicePloxy.StatisticsPgTimeModel   remoteData   = new AdManagerClient.StatisticsPgTimeServicePloxy.StatisticsPgTimeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgTimeModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgTimeModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgTimeModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgTimeModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgTimeModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgTimeModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgTimeModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsPgTime(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgTimeModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgTimeModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgTimeModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ �ð��뺰��� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgTimeReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����������Ʈ �ð��뺰��� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgTimeReportAVG(StatisticsPgTimeModel statisticsPgTimeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ �ð��뺰��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgTimeServicePloxy.StatisticsPgTimeService svc = new StatisticsPgTimeServicePloxy.StatisticsPgTimeService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgTimeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgTimeServicePloxy.HeaderModel();
				StatisticsPgTimeServicePloxy.StatisticsPgTimeModel   remoteData   = new AdManagerClient.StatisticsPgTimeServicePloxy.StatisticsPgTimeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgTimeModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgTimeModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgTimeModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgTimeModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgTimeModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgTimeModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgTimeModel.SearchEndDay;       
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsPgTimeAVG(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgTimeModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgTimeModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgTimeModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ �ð��뺰��� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgTimeReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
