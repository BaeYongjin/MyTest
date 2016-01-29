// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// ���������� ���� ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
// 2007.06.26 �۸�ȯ v1.0
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
    /// ���������� �����񽺸� ȣ���մϴ�. 
    /// </summary>
    public class ContentsSearch_pManager : BaseManager
    {
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ContentsSearch_pManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/ContentsService.asmx";
        }

        /// <summary>
        /// ������������ȸ
        /// </summary>
        /// <param name="contentsModel"></param>
        public void GetContentsListPopUp(ContentsModel contentsModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�����������ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ContentsServicePloxy.ContentsService svc = new ContentsServicePloxy.ContentsService();
				svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                ContentsServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContentsServicePloxy.HeaderModel();
                ContentsServicePloxy.ContentsModel remoteData   = new AdManagerClient.ContentsServicePloxy.ContentsModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = contentsModel.SearchKey;
               
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetContentsListPopUp(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contentsModel.ContentsDataSet = remoteData.ContentsDataSet.Copy();
                contentsModel.ResultCnt   = remoteData.ResultCnt;
                contentsModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����������ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetContentsListPopUp():" + fe.ErrCode + ":" + fe.ResultMsg);
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