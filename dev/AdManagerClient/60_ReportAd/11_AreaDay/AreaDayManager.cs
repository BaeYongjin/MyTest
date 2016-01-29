// ===============================================================================
//
// AreaDayManager.cs
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
	public class AreaDayManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AreaDayManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsDaily";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/AreaDayService.asmx";
		}


		/// <summary>
		/// ����-���Ϻ� ������� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAreaDay(AreaDayModel areaDayModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������ ������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AreaDayServicePloxy.AreaDayService svc = new AreaDayServicePloxy.AreaDayService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AreaDayServicePloxy.HeaderModel     remoteHeader = new AdManagerClient.AreaDayServicePloxy.HeaderModel();
				AreaDayServicePloxy.AreaDayModel    remoteData   = new AdManagerClient.AreaDayServicePloxy.AreaDayModel();
                AreaDayModel test = new AreaDayModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.StartDay = areaDayModel.StartDay;       
				remoteData.EndDay   = areaDayModel.EndDay;
                remoteData.AdList   = areaDayModel.AdList.ToArray();
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAreaDay(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ				
				areaDayModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				areaDayModel.ResultCnt     = remoteData.ResultCnt;
				areaDayModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������ ������� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAreaDay():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �ð��� ������� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetTimeDay(AreaDayModel areaDayModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�ð��� ������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AreaDayServicePloxy.AreaDayService svc = new AreaDayServicePloxy.AreaDayService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AreaDayServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.AreaDayServicePloxy.HeaderModel();
				AreaDayServicePloxy.AreaDayModel   remoteData   = new AdManagerClient.AreaDayServicePloxy.AreaDayModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ				  
 
				remoteData.StartDay = areaDayModel.StartDay;       
				remoteData.EndDay   = areaDayModel.EndDay;       
                remoteData.AdList   = areaDayModel.AdList.ToArray();
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetTimeDay(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ				
				areaDayModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				areaDayModel.ResultCnt     = remoteData.ResultCnt;
				areaDayModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�ð��� ������� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetTimeDay():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public void GetGenreDay(AreaDayModel areaDayModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�帣�� ������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AreaDayServicePloxy.AreaDayService svc = new AreaDayServicePloxy.AreaDayService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AreaDayServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.AreaDayServicePloxy.HeaderModel();
				AreaDayServicePloxy.AreaDayModel   remoteData   = new AdManagerClient.AreaDayServicePloxy.AreaDayModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ				  
				remoteData.StartDay        = areaDayModel.StartDay;       
				remoteData.EndDay          = areaDayModel.EndDay;       
                remoteData.AdList   = areaDayModel.AdList.ToArray();
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGenreDay(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ				
				areaDayModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				areaDayModel.ResultCnt     = remoteData.ResultCnt;
				areaDayModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�帣�� ������� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreDay():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public void GetChannelDay(AreaDayModel areaDayModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�κ� ������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AreaDayServicePloxy.AreaDayService svc = new AreaDayServicePloxy.AreaDayService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AreaDayServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.AreaDayServicePloxy.HeaderModel();
				AreaDayServicePloxy.AreaDayModel   remoteData   = new AdManagerClient.AreaDayServicePloxy.AreaDayModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ				  
				remoteData.StartDay        = areaDayModel.StartDay;       
				remoteData.EndDay          = areaDayModel.EndDay;     
                remoteData.AdList   = areaDayModel.AdList.ToArray();
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetChannelDay(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ				
				areaDayModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				areaDayModel.ResultCnt     = remoteData.ResultCnt;
				areaDayModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�κ� ������� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelDay():" + fe.ErrCode + ":" + fe.ResultMsg);
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
