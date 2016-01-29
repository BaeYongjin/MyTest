// ===============================================================================
//
// MediaCodeManager.cs
//
// �����ڵ���ȸ ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
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
	/// ��������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class MediaCodeManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public MediaCodeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/MediaCodeService.asmx";
		}

		/// <summary>
		/// ����� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetMediaCodeList(MediaCodeModel mediacodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				MediaCodeServicePloxy.MediaCodeService svc = new MediaCodeServicePloxy.MediaCodeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				MediaCodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaCodeServicePloxy.HeaderModel();
				MediaCodeServicePloxy.MediaCodeModel     remoteData   = new AdManagerClient.MediaCodeServicePloxy.MediaCodeModel();

				// ������� ��Ʈ
				//remoteHeader.ClientKey     = Header.ClientKey;
				//remoteHeader.UserID        = Header.UserID;
				
				// ȣ������ ��Ʈ
				remoteData.MediaCode         = mediacodeModel.MediaCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetMediaCodeList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				mediacodeModel.MediaCodeDataSet = remoteData.MediaCodeDataSet.Copy();
				mediacodeModel.ResultCnt   = remoteData.ResultCnt;
				mediacodeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				//_log.Warning( this.ToString() + ":GetMediaCodeList():" + fe.ErrMediaCode + ":" + fe.ResultMsg);
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
