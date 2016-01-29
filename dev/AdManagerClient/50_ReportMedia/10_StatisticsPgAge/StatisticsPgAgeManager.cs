// ===============================================================================
//
// StatisticsPgAgeManager.cs
//
// ����������Ʈ ���ɺ���� ��ȸ ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

using System;

using WinFramework.Base;

using AdManagerModel;
using WinFramework.Misc;

namespace AdManagerClient
{
	/// <summary>
	/// ����������Ʈ ���ɺ���� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class StatisticsPgAgeManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsPgAgeManager(SystemModel systemModel, CommonModel commonModel, String kind) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;

            if ("Age".Equals(kind))
            {
                _module = "StatisticsPg";
                _Path = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgService.asmx";
            }
            else
            {
                _module = "StatisticsPgWeek";
                _Path = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgWeekService.asmx";
            }
		}

        public StatisticsPgAgeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "StatisticsPgWeek";
            _Host = FrameSystem.m_WebServer_Host;
            _Port = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgWeekService.asmx";
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
		/// ����������Ʈ ���ɺ���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgAgeReport(StatisticsPgModel statisticsPgModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ���ɺ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
                StatisticsPgServicePloxy.StatisticsPgService svc = new StatisticsPgServicePloxy.StatisticsPgService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
                StatisticsPgServicePloxy.HeaderModel remoteHeader = new AdManagerClient.StatisticsPgServicePloxy.HeaderModel();
                StatisticsPgServicePloxy.StatisticsPgModel remoteData = new AdManagerClient.StatisticsPgServicePloxy.StatisticsPgModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsPgAge(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgModel.ResultCD      = remoteData.ResultCD;

                statisticsPgModel.ResultCnt = Utility.GetDatasetCount(statisticsPgModel.ReportDataSet);

				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ���ɺ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgAgeReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����������Ʈ ���ɺ���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgAgeReportAVG(StatisticsPgModel statisticsPgModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ���ɺ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
                StatisticsPgServicePloxy.StatisticsPgService svc = new StatisticsPgServicePloxy.StatisticsPgService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
                StatisticsPgServicePloxy.HeaderModel remoteHeader = new AdManagerClient.StatisticsPgServicePloxy.HeaderModel();
                StatisticsPgServicePloxy.StatisticsPgModel remoteData = new AdManagerClient.StatisticsPgServicePloxy.StatisticsPgModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgModel.SearchEndDay;       
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsPgAgeAVG(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ���ɺ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgAgeReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
