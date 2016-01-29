// ===============================================================================
//
// AdTypeMoniteringManager.cs
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
	/// AdTypeMoniteringManager�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdTypeMoniteringManager : BaseManager
	{
		public AdTypeMoniteringManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "AdTypeMoniteringService";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/AdTypeMoniteringService.asmx";
		}

		/// <summary>
		/// �����������͸� ������ ��ȸ
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void GetAdTypeMaster(AdTypeMoniteringModel adTypeMoniteringModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����������͸� ������ ��ȸ Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				AdTypeMoniteringServicePloxy.AdTypeMoniteringService svc = new AdManagerClient.AdTypeMoniteringServicePloxy.AdTypeMoniteringService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdTypeMoniteringServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AdTypeMoniteringServicePloxy.HeaderModel();
				AdTypeMoniteringServicePloxy.AdTypeMoniteringModel remoteData = new AdManagerClient.AdTypeMoniteringServicePloxy.AdTypeMoniteringModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey = Header.ClientKey;
				remoteHeader.UserID = Header.UserID;
				remoteHeader.UserLevel = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.LogDay = adTypeMoniteringModel.LogDay;

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAdTypeMaster(remoteHeader, remoteData);

				// ����ڵ�˻�
				if (!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adTypeMoniteringModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				adTypeMoniteringModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				adTypeMoniteringModel.ResultCnt = remoteData.ResultCnt;
				adTypeMoniteringModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����������͸� ������ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch (FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning(this.ToString() + ":GetAdTypeMaster():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch (Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		/// <summary>
		/// �����������͸� ������ ��ȸ
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetAdTypeDetail(AdTypeMoniteringModel adTypeMoniteringModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����������͸� ������ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AdTypeMoniteringServicePloxy.AdTypeMoniteringService svc = new AdManagerClient.AdTypeMoniteringServicePloxy.AdTypeMoniteringService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdTypeMoniteringServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AdTypeMoniteringServicePloxy.HeaderModel();
				AdTypeMoniteringServicePloxy.AdTypeMoniteringModel remoteData = new AdManagerClient.AdTypeMoniteringServicePloxy.AdTypeMoniteringModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.LogDay   = adTypeMoniteringModel.LogDay;
				remoteData.AdType   = adTypeMoniteringModel.AdType;
				remoteData.Rap      = adTypeMoniteringModel.Rap;

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAdTypeDetail(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adTypeMoniteringModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				adTypeMoniteringModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				adTypeMoniteringModel.ResultCnt = remoteData.ResultCnt;
				adTypeMoniteringModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����������͸� ������ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdTypeDetail():" + fe.ErrCode + ":" + fe.ResultMsg);
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
