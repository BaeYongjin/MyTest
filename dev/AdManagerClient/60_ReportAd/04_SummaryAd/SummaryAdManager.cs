// ===============================================================================
//
// SummaryAdManager.cs
//
// ���� �Ѱ����� ���� ��ȸ ���񽺸� ȣ���մϴ�. 
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
using System.Threading;
using System.Diagnostics;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// ���� �Ѱ����� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class SummaryAdManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SummaryAdManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "SummaryAd";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/SummaryAdService.asmx";
		}


		/// <summary>
		/// ������ ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetContractItemList(SummaryAdModel summaryAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������ ��ȸ Start");
				_log.Debug("-----------------------------------------");
					
				// ������ �ν��Ͻ� ����
				SummaryAdServicePloxy.SummaryAdService svc = new SummaryAdServicePloxy.SummaryAdService();
				
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				SummaryAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SummaryAdServicePloxy.HeaderModel();
				SummaryAdServicePloxy.SummaryAdModel   remoteData   = new AdManagerClient.SummaryAdServicePloxy.SummaryAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchContractSeq     = summaryAdModel.SearchContractSeq;
				remoteData.CampaignCode     = summaryAdModel.CampaignCode;
					
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;  
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetContractItemList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				summaryAdModel.ItemDataSet   = remoteData.ItemDataSet.Copy();
				summaryAdModel.ResultCnt     = remoteData.ResultCnt;
				summaryAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContractItemList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �ϰ� ���� �Ѱ����� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetSummaryAdReport(SummaryAdModel summaryAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���� �Ѱ����� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SummaryAdServicePloxy.SummaryAdService svc = new SummaryAdServicePloxy.SummaryAdService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				SummaryAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SummaryAdServicePloxy.HeaderModel();
				SummaryAdServicePloxy.SummaryAdModel   remoteData   = new AdManagerClient.SummaryAdServicePloxy.SummaryAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
                remoteData.MenuLevel            = summaryAdModel.MenuLevel;
                remoteData.SearchMediaCode      = summaryAdModel.SearchMediaCode;
				remoteData.SearchContractSeq    = summaryAdModel.SearchContractSeq;
                remoteData.CampaignCode         = summaryAdModel.CampaignCode;
				remoteData.SearchItemNo         = summaryAdModel.SearchItemNo;
				remoteData.SearchType           = summaryAdModel.SearchType;
				remoteData.SearchStartDay       = summaryAdModel.SearchStartDay;
				remoteData.SearchEndDay         = summaryAdModel.SearchEndDay;

				remoteData.TotalUser = summaryAdModel.TotalUser;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = 1000 * 60 * 30;   // 30��
                
                remoteData = svc.GetSummaryAdDaily(remoteHeader, remoteData);

				#region [ ��� ���� ]
				//				�ϰ�,�Ⱓ,�ְ�,������ ���� �Լ��� ���������, S1��ȭ ���� �����Լ��� ������
				//				// ������ �޼ҵ� ȣ��
				//				if(summaryAdModel.SearchType.Equals("C"))
				//				{
				//					remoteData = svc.GetSummaryAdTotality(remoteHeader, remoteData);
				//				}
				//				else if(summaryAdModel.SearchType.Equals("D"))
				//				{
				//					remoteData = svc.GetSummaryAdDaily(remoteHeader, remoteData);
				//				}
				//				else if(summaryAdModel.SearchType.Equals("W"))
				//				{
				//					remoteData = svc.GetSummaryAdWeekly(remoteHeader, remoteData);
				//				}
				//				else if(summaryAdModel.SearchType.Equals("M"))
				//				{
				//					remoteData = svc.GetSummaryAdMonthly(remoteHeader, remoteData);
				//				}
				#endregion

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				summaryAdModel.TotalUser     = remoteData.TotalUser;
				summaryAdModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				summaryAdModel.ResultCnt     = remoteData.ResultCnt;
				summaryAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���� ��ûȽ�� ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSummaryAdReport():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
                Debug.WriteLine("���� : " + DateTime.Now.ToString());
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
                Debug.WriteLine("���� : " + DateTime.Now.ToString());
				throw new FrameException(e.Message);
			}
            Debug.WriteLine("���� : " + DateTime.Now.ToString());
		}
	}
}
