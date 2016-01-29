// ===============================================================================
//
// OAPWeekSummaryRptManager.cs
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
	/// OAPWeekSummaryRptManager�� ���� ��� �����Դϴ�.
	/// </summary>
	public class OAPWeekSummaryRptManager : BaseManager
	{
		public OAPWeekSummaryRptManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyAdExecSummaryRpt";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/OAPWeekSummaryRptService.asmx";
		}

		/// <summary>
		/// OAP�ְ�Ȩ���� ��ȸ
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetOAPWeekHomeAd(OAPWeekSummaryRptModel oAPWeekSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("OAP�ְ�Ȩ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService svc = new AdManagerClient.OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				OAPWeekSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.OAPWeekSummaryRptServicePloxy.HeaderModel();
				OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptModel remoteData = new AdManagerClient.OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.LogDay1 = oAPWeekSummaryRptModel.LogDay1;
				remoteData.LogDay2 = oAPWeekSummaryRptModel.LogDay2;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetOAPWeekHomeAd(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				oAPWeekSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				oAPWeekSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				oAPWeekSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				oAPWeekSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("OAP�ְ�Ȩ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetOAPWeekHomeAd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// OAP�ְ�ä������ ��ȸ
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetOAPWeekChannelJump(OAPWeekSummaryRptModel oAPWeekSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("OAP�ְ�ä������ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService svc = new AdManagerClient.OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				OAPWeekSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.OAPWeekSummaryRptServicePloxy.HeaderModel();
				OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptModel remoteData = new AdManagerClient.OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.LogDay1 = oAPWeekSummaryRptModel.LogDay1;
				remoteData.LogDay2 = oAPWeekSummaryRptModel.LogDay2;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetOAPWeekChannelJump(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				oAPWeekSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				oAPWeekSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				oAPWeekSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				oAPWeekSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("OAP�ְ�ä������ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetOAPWeekChannelJump():" + fe.ErrCode + ":" + fe.ResultMsg);
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


        public DataSet	mngGetHomeCmReport(string Day1, string Day2, string mediaRep)
        {
            DataSet rtnDs = new DataSet();

            try
            {
                OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService svc = new AdManagerClient.OAPWeekSummaryRptServicePloxy.OAPWeekSummaryRptService();
                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;  

                // ������ �޼ҵ� ȣ��
                rtnDs = svc.mGetHomeCmReport(Day1, Day2, mediaRep);

                if( rtnDs == null )
                {
                    throw new FrameException("Ȩ������� ��ȸ ����!!!", _module, "0001");
                }
                _log.Debug("-----------------------------------------");
                _log.Debug("Ȩ������� ��ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":Ȩ�������:" + fe.ErrCode + ":" + fe.ResultMsg);
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
            return rtnDs;
        }

	}
}
