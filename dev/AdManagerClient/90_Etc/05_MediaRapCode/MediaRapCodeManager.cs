// ===============================================================================
//
// MediaRapCodeManager.cs
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

/*
 * -------------------------------------------------------
 * Class Name: MediaRapCodeManager
 * �ֿ���  : �����ڵ���ȸ ���� ȣ��
 * �ۼ���    : ��
 * �ۼ���    : ��
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : �躸��
 * ������    : 2013.02
 * ��������  :        
 *            - �˻��� ��ȸ ��� �߰�
 * �����Լ�  :
 *            - GetMediaRapCodeList();
 * --------------------------------------------------------
 */
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
	public class MediaRapCodeManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public MediaRapCodeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/MediaRapCodeService.asmx";
		}

		/// <summary>
		/// ����� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetMediaRapCodeList(MediaRapCodeModel mediarapcodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				MediaRapCodeServicePloxy.MediaRapCodeService svc = new MediaRapCodeServicePloxy.MediaRapCodeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				MediaRapCodeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaRapCodeServicePloxy.HeaderModel();
				MediaRapCodeServicePloxy.MediaRapCodeModel     remoteData   = new AdManagerClient.MediaRapCodeServicePloxy.MediaRapCodeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.RapCode         = mediarapcodeModel.RapCode;
                remoteData.SearchKey       = mediarapcodeModel.SearchKey; // [E_01]
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetMediaRapCodeList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				mediarapcodeModel.MediaRapCodeDataSet = remoteData.MediaRapCodeDataSet.Copy();
				mediarapcodeModel.ResultCnt   = remoteData.ResultCnt;
				mediarapcodeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				//_log.Warning( this.ToString() + ":GetMediaRapCodeList():" + fe.ErrMediaRapCode + ":" + fe.ResultMsg);
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
