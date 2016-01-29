// ===============================================================================
//
// StatisticsPgRegionManager.cs
//
// ����������Ʈ ��������� ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// ����������Ʈ ��������� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class StatisticsPgRegionManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsPgRegionManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsPgRegion";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgRegionService.asmx";
		}

		/// <summary>
		/// ī�װ���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetCategoryList(StatisticsPgRegionModel statisticsPgRegionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgRegionServicePloxy.StatisticsPgRegionService svc = new StatisticsPgRegionServicePloxy.StatisticsPgRegionService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgRegionServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgRegionServicePloxy.HeaderModel();
				StatisticsPgRegionServicePloxy.StatisticsPgRegionModel   remoteData   = new AdManagerClient.StatisticsPgRegionServicePloxy.StatisticsPgRegionModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgRegionModel.SearchMediaCode;	  
				
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
				statisticsPgRegionModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
				statisticsPgRegionModel.ResultCnt       = remoteData.ResultCnt;
				statisticsPgRegionModel.ResultCD        = remoteData.ResultCD;

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
		public void GetGenreList(StatisticsPgRegionModel statisticsPgRegionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�帣��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgRegionServicePloxy.StatisticsPgRegionService svc = new StatisticsPgRegionServicePloxy.StatisticsPgRegionService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgRegionServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgRegionServicePloxy.HeaderModel();
				StatisticsPgRegionServicePloxy.StatisticsPgRegionModel   remoteData   = new AdManagerClient.StatisticsPgRegionServicePloxy.StatisticsPgRegionModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgRegionModel.SearchMediaCode;	  
				
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
				statisticsPgRegionModel.GenreDataSet = remoteData.GenreDataSet.Copy();
				statisticsPgRegionModel.ResultCnt    = remoteData.ResultCnt;
				statisticsPgRegionModel.ResultCD     = remoteData.ResultCD;

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
		/// ����������Ʈ ��������� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgRegionReport(StatisticsPgRegionModel statisticsPgRegionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ��������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgRegionServicePloxy.StatisticsPgRegionService svc = new StatisticsPgRegionServicePloxy.StatisticsPgRegionService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgRegionServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgRegionServicePloxy.HeaderModel();
				StatisticsPgRegionServicePloxy.StatisticsPgRegionModel   remoteData   = new AdManagerClient.StatisticsPgRegionServicePloxy.StatisticsPgRegionModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgRegionModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgRegionModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgRegionModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgRegionModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgRegionModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgRegionModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgRegionModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsPgRegion(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgRegionModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgRegionModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgRegionModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ��������� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgRegionReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����������Ʈ ��������� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgRegionReportAVG(StatisticsPgRegionModel statisticsPgRegionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ��������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgRegionServicePloxy.StatisticsPgRegionService svc = new StatisticsPgRegionServicePloxy.StatisticsPgRegionService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgRegionServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgRegionServicePloxy.HeaderModel();
				StatisticsPgRegionServicePloxy.StatisticsPgRegionModel   remoteData   = new AdManagerClient.StatisticsPgRegionServicePloxy.StatisticsPgRegionModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgRegionModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgRegionModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgRegionModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgRegionModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgRegionModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgRegionModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgRegionModel.SearchEndDay;       
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsPgRegionAVG(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgRegionModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgRegionModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgRegionModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ��������� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgRegionReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
