using System;
using System.Data;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;
using System.Collections;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// ������Ʈ �Ϻ���� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class HouseHoldAccountManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public HouseHoldAccountManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log	= FrameSystem.oLog;
			_module = "RegionGenreService";
			_Host	= FrameSystem.m_WebServer_Host;
			_Port	= FrameSystem.m_WebServer_Port;
			_Path	= FrameSystem.m_WebServer_App + "/ReportAd/SummaryAdService.asmx";
		}

		/// <summary>
		/// ������ �帣
		/// </summary>
		/// <param name="houseHoldAccountModel"></param>
		public void GetHouseHoldAccount(HouseHoldAccountModel houseHoldAccountModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������ �帣��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SummaryAdServicePloxy.SummaryAdService svc = new AdManagerClient.SummaryAdServicePloxy.SummaryAdService();
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				SummaryAdServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.SummaryAdServicePloxy.HeaderModel();
				SummaryAdServicePloxy.SummaryAdModel		remoteData   = new AdManagerClient.SummaryAdServicePloxy.SummaryAdModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchStartDay     = houseHoldAccountModel.SearchStartDay;
				remoteData.SearchEndDay       = houseHoldAccountModel.SearchEndDay;       
				remoteData.SearchMediaCode    = houseHoldAccountModel.SearchMediaCode;       
				remoteData.SearchContractSeq  = houseHoldAccountModel.SearchContractSeq;       
				remoteData.CampaignCode       = houseHoldAccountModel.CampaignCode;       
				remoteData.SearchItemNo       = houseHoldAccountModel.SearchItemNo;       

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 240;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetDateAccountHouseHold(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ								
				houseHoldAccountModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				houseHoldAccountModel.ResultCnt     = remoteData.ResultCnt;
				houseHoldAccountModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������ �ð���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetRegionDate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
