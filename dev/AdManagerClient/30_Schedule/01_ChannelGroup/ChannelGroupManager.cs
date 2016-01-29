// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// �帣�׷����� ���� ���񽺸� ȣ���մϴ�. 
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
    /// �帣�׷����� �����񽺸� ȣ���մϴ�. 
    /// </summary>
    public class ChannelGroupManager : BaseManager
    {
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ChannelGroupManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Schedule/ChannelGroupService.asmx";
        }

        /// <summary>
        /// �̵���޺�������ȸ
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetMediaList(ChannelGroupModel channelGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�̵���޺� �����ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                MediaInfoServicePloxy.MediaInfoService svc = new MediaInfoServicePloxy.MediaInfoService();
			    svc.Url = "http://" + _Host + ":" + _Port + "/AdManagerWebService/Media/MediaInfoService.asmx";
                // ����Ʈ �� ����
                MediaInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaInfoServicePloxy.HeaderModel();
                MediaInfoServicePloxy.MediaInfoModel remoteData   = new AdManagerClient.MediaInfoServicePloxy.MediaInfoModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = channelGroupModel.SearchKey;
                remoteData.MediaCode = channelGroupModel.MediaCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetUsersList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                channelGroupModel.MediaDataSet = remoteData.UserDataSet.Copy();
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�̵���޺� �����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetMediaList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �帣�׷�������ȸ
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetChannelGroupList(ChannelGroupModel channelGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷�����ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ChannelGroupServicePloxy.ChannelGroupService svc = new ChannelGroupServicePloxy.ChannelGroupService();
			    svc.Url = _WebServiceUrl;
                // ����Ʈ �� ����
                ChannelGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelGroupServicePloxy.HeaderModel();
                ChannelGroupServicePloxy.ChannelGroupModel remoteData   = new AdManagerClient.ChannelGroupServicePloxy.ChannelGroupModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = channelGroupModel.SearchKey;
                remoteData.MediaCode       = channelGroupModel.MediaCode;
				
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetChannelGroupList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                channelGroupModel.ChannelGroupDataSet = remoteData.ChannelGroupDataSet.Copy();
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷�����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelGroupList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �帣�׷� ������ �����ȸ
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetChannelGroupDetailList(ChannelGroupModel channelGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷� ������ ��ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ChannelGroupServicePloxy.ChannelGroupService svc = new ChannelGroupServicePloxy.ChannelGroupService();
			    svc.Url = _WebServiceUrl;
                // ����Ʈ �� ����
                ChannelGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelGroupServicePloxy.HeaderModel();
                ChannelGroupServicePloxy.ChannelGroupModel remoteData   = new AdManagerClient.ChannelGroupServicePloxy.ChannelGroupModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.MediaCode       = channelGroupModel.MediaCode;
                remoteData.AdGroupCode      = channelGroupModel.AdGroupCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetChannelGroupDetailList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                channelGroupModel.ChannelGroupDataSet = remoteData.ChannelGroupDataSet.Copy();
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷� ������ �����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelGroupList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
   
        public void SetChannelGroupAdd(ChannelGroupModel channelGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷��߰� Start");
                _log.Debug("-----------------------------------------");         
            
                // ������ �ν��Ͻ� ����
                ChannelGroupServicePloxy.ChannelGroupService svc = new ChannelGroupServicePloxy.ChannelGroupService();
            	svc.Url = _WebServiceUrl;			
                // ����Ʈ �� ����
                ChannelGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelGroupServicePloxy.HeaderModel();
                ChannelGroupServicePloxy.ChannelGroupModel remoteData   = new AdManagerClient.ChannelGroupServicePloxy.ChannelGroupModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ��������Ʈ
                remoteData.MediaCode            = channelGroupModel.MediaCode;
                remoteData.AdGroupName          = channelGroupModel.AdGroupName;
                remoteData.Comment              = channelGroupModel.Comment;
                remoteData.UseYn                = channelGroupModel.UseYn;
                remoteData.ChannelGroupDataSet    = channelGroupModel.ChannelGroupDataSet.Copy();

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetChannelGroupCreate(remoteHeader, remoteData);
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("channelGroupModel.ResultCnt = "+channelGroupModel.ResultCnt);
            			
                channelGroupModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷��߰� End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                channelGroupModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetChannelGroupCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                channelGroupModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// �帣�׷����� ����
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void SetChannelGroupUpdate(ChannelGroupModel channelGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷��������� Start");
                _log.Debug("-----------------------------------------");
            
            
                //				�Էµ������� Validation �˻�
                if(channelGroupModel.AdGroupName.Trim().Length < 1) 
                {
                    throw new FrameException("���� ��ǰ���� �������� �ʽ��ϴ�.");
                }
                if(channelGroupModel.AdGroupName.Trim().Length > 50) 
                {
                    throw new FrameException("���� ��ǰ���� 50Byte�� �ʰ��� �� �����ϴ�.");
                }
                if(channelGroupModel.Comment.Trim().Length > 100) 
                {
                    throw new FrameException("�׷켳���� 100Byte�� �ʰ��� �� �����ϴ�.");
                }     
            	
            
                // ������ �ν��Ͻ� ����
                ChannelGroupServicePloxy.ChannelGroupService svc = new ChannelGroupServicePloxy.ChannelGroupService();
            	svc.Url = _WebServiceUrl;		
                // ����Ʈ �� ����
                ChannelGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelGroupServicePloxy.HeaderModel();
                ChannelGroupServicePloxy.ChannelGroupModel remoteData   = new AdManagerClient.ChannelGroupServicePloxy.ChannelGroupModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ��������Ʈ
                remoteData.MediaCode            = channelGroupModel.MediaCode;
                remoteData.AdGroupCode          = channelGroupModel.AdGroupCode;
                remoteData.AdGroupName          = channelGroupModel.AdGroupName;
                remoteData.Comment              = channelGroupModel.Comment;
                remoteData.UseYn                = channelGroupModel.UseYn;
                remoteData.ChannelGroupDataSet    = channelGroupModel.ChannelGroupDataSet.Copy();
            				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetChannelGroupUpdate(remoteHeader, remoteData);
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷��������� End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetChannelGroupUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �帣�׷��߰�
        /// </summary>
        /// <param name="channelGroupModel"></param>
        /// <returns></returns>	
        /// <summary>
        /// �帣�׷� ����
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetChannelGroupDelete(ChannelGroupModel channelGroupModel)
        {
          
        }

    }
}