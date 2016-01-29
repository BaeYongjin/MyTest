// ===============================================================================
//
// StatisticsPgWeekManager.cs
//
// ����������Ʈ ���Ϻ���� ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// ����������Ʈ ���Ϻ���� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class StatisticsPgWeekManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsPgWeekManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsPgWeek";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgWeekService.asmx";
		}

		/// <summary>
		/// ī�װ���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetCategoryList(StatisticsPgWeekModel statisticsPgWeekModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgWeekServicePloxy.StatisticsPgWeekService svc = new StatisticsPgWeekServicePloxy.StatisticsPgWeekService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgWeekServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgWeekServicePloxy.HeaderModel();
				StatisticsPgWeekServicePloxy.StatisticsPgWeekModel   remoteData   = new AdManagerClient.StatisticsPgWeekServicePloxy.StatisticsPgWeekModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgWeekModel.SearchMediaCode;	  
				
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
				statisticsPgWeekModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
				statisticsPgWeekModel.ResultCnt       = remoteData.ResultCnt;
				statisticsPgWeekModel.ResultCD        = remoteData.ResultCD;

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
		public void GetGenreList(StatisticsPgWeekModel statisticsPgWeekModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�帣��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgWeekServicePloxy.StatisticsPgWeekService svc = new StatisticsPgWeekServicePloxy.StatisticsPgWeekService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgWeekServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgWeekServicePloxy.HeaderModel();
				StatisticsPgWeekServicePloxy.StatisticsPgWeekModel   remoteData   = new AdManagerClient.StatisticsPgWeekServicePloxy.StatisticsPgWeekModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgWeekModel.SearchMediaCode;	  
				
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
				statisticsPgWeekModel.GenreDataSet = remoteData.GenreDataSet.Copy();
				statisticsPgWeekModel.ResultCnt    = remoteData.ResultCnt;
				statisticsPgWeekModel.ResultCD     = remoteData.ResultCD;

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
		/// ����������Ʈ ���Ϻ���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgWeekReport(StatisticsPgWeekModel statisticsPgWeekModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ���Ϻ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgWeekServicePloxy.StatisticsPgWeekService svc = new StatisticsPgWeekServicePloxy.StatisticsPgWeekService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgWeekServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgWeekServicePloxy.HeaderModel();
				StatisticsPgWeekServicePloxy.StatisticsPgWeekModel   remoteData   = new AdManagerClient.StatisticsPgWeekServicePloxy.StatisticsPgWeekModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgWeekModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgWeekModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgWeekModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgWeekModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgWeekModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgWeekModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgWeekModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsPgWeek(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgWeekModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgWeekModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgWeekModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ���Ϻ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgWeekReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����������Ʈ ���Ϻ���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgWeekReportAVG(StatisticsPgWeekModel statisticsPgWeekModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ���Ϻ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgWeekServicePloxy.StatisticsPgWeekService svc = new StatisticsPgWeekServicePloxy.StatisticsPgWeekService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgWeekServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgWeekServicePloxy.HeaderModel();
				StatisticsPgWeekServicePloxy.StatisticsPgWeekModel   remoteData   = new AdManagerClient.StatisticsPgWeekServicePloxy.StatisticsPgWeekModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgWeekModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgWeekModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgWeekModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgWeekModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgWeekModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgWeekModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgWeekModel.SearchEndDay;       
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsPgWeekAVG(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgWeekModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgWeekModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgWeekModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ���Ϻ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgWeekReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
