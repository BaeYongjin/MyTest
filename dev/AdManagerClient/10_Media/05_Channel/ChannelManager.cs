// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// ä������ ���� ���񽺸� ȣ���մϴ�. 
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
    /// ä������ �����񽺸� ȣ���մϴ�. 
    /// </summary>
    public class ChannelManager : BaseManager
    {
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ChannelManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/ChannelService.asmx";
        }

        /// <summary>
        /// �̵���޺�������ȸ
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetMediaList(ChannelModel channelModel)
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
                remoteData.SearchKey       = channelModel.SearchKey;
                remoteData.MediaCode = channelModel.MediaCode;
				
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
                channelModel.MediaDataSet = remoteData.UserDataSet.Copy();
                channelModel.ResultCnt   = remoteData.ResultCnt;
                channelModel.ResultCD    = remoteData.ResultCD;

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
        /// ä���帣������ȸ
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelList(ChannelModel channelModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ä�θ����ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ChannelServicePloxy.ChannelService svc = new ChannelServicePloxy.ChannelService();
				svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                ChannelServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelServicePloxy.HeaderModel();
                ChannelServicePloxy.ChannelModel remoteData   = new AdManagerClient.ChannelServicePloxy.ChannelModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = channelModel.SearchKey;
                remoteData.MediaCode       = channelModel.MediaCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetChannelList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                channelModel.ChannelDataSet = remoteData.ChannelDataSet.Copy();
                channelModel.ResultCnt   = remoteData.ResultCnt;
                channelModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("ä�θ����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ä�� ������ �����ȸ
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelDetailList(ChannelModel channelModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ä�� ������ ��ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ChannelServicePloxy.ChannelService svc = new ChannelServicePloxy.ChannelService();
                svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                ChannelServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelServicePloxy.HeaderModel();
                ChannelServicePloxy.ChannelModel remoteData   = new AdManagerClient.ChannelServicePloxy.ChannelModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = channelModel.SearchKey;
                remoteData.GenreCode       = channelModel.GenreCode;
                remoteData.CheckYn         = channelModel.CheckYn;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetChannelDetailList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                channelModel.ChannelDataSet = remoteData.ChannelDataSet.Copy();
                channelModel.ResultCnt   = remoteData.ResultCnt;
                channelModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("ä�� ������ �����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ä�� ������ �����ȸ - ������
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelSetDetailList(ChannelModel channelModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ä�α��� ������ ��ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ChannelServicePloxy.ChannelService svc = new ChannelServicePloxy.ChannelService();
                svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                ChannelServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelServicePloxy.HeaderModel();
                ChannelServicePloxy.ChannelModel remoteData   = new AdManagerClient.ChannelServicePloxy.ChannelModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = channelModel.SearchKey;
                remoteData.MediaCode       = channelModel.MediaCode;
                remoteData.ChannelNo       = channelModel.ChannelNo;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetChannelSetDetailList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                channelModel.ChannelDataSet = remoteData.ChannelDataSet.Copy();
                channelModel.ResultCnt   = remoteData.ResultCnt;
                channelModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("ä�α��� ������ �����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelSetDetailList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ä���߰�
        /// </summary>
        /// <param name="channelModel"></param>
        /// <returns></returns>
        public void SetChannelAdd(ChannelModel channelModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ä���߰� Start");
                _log.Debug("-----------------------------------------");
            
                if(channelModel.TotalSeries.Length > 0 &&  Convert.ToInt32(channelModel.TotalSeries) > 255) 
                {
                    throw new FrameException("�ø�������� 255�� �ʰ��� �� �����ϴ�.");
                }

                // ������ �ν��Ͻ� ����
                ChannelServicePloxy.ChannelService svc = new ChannelServicePloxy.ChannelService();
				svc.Url = _WebServiceUrl;
            				
                // ����Ʈ �� ����
                ChannelServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelServicePloxy.HeaderModel();
                ChannelServicePloxy.ChannelModel remoteData   = new AdManagerClient.ChannelServicePloxy.ChannelModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ��������Ʈ
                remoteData.ServiceID = channelModel.ServiceID;
                remoteData.ChannelNumber     = channelModel.ChannelNumber;
                remoteData.UseYn     = channelModel.UseYn;
                remoteData.AdYn = channelModel.AdYn;
                remoteData.AdRate = channelModel.AdRate;
                remoteData.AdnRate = channelModel.AdnRate;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetChannelUpdate(remoteHeader, remoteData);
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                channelModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("channelModel.ResultCnt = "+channelModel.ResultCnt);
            			
                channelModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("ä���߰� End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                channelModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetChannelCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                channelModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

		
        /// <summary>
        /// ä�� ����
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetChannelDelete(ChannelModel channelModel)
        {
            			try
            			{
            				_log.Debug("-----------------------------------------");
            				_log.Debug("ä�λ��� start");
            				_log.Debug("-----------------------------------------");
            
                            // ������ �ν��Ͻ� ����
                            ChannelServicePloxy.ChannelService svc = new ChannelServicePloxy.ChannelService();
							svc.Url = _WebServiceUrl;
            				
                            // ����Ʈ �� ����
                            ChannelServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelServicePloxy.HeaderModel();
                            ChannelServicePloxy.ChannelModel remoteData   = new AdManagerClient.ChannelServicePloxy.ChannelModel();
            
                            // ������� ��Ʈ
                            remoteHeader.ClientKey     = Header.ClientKey;
                            remoteHeader.UserID        = Header.UserID;
            
                            // ȣ��������Ʈ
                            remoteData.MediaCode     = channelModel.MediaCode;
                            remoteData.ChannelNo     = channelModel.ChannelNo;
                            
                            // ������ ȣ�� Ÿ�Ӿƿ�����
                            svc.Timeout = FrameSystem.m_SystemTimeout;
	
            				// ������ �޼ҵ� ȣ��
            				remoteData = svc.SetChannelDelete(remoteHeader, remoteData);
            
            				// ����ڵ�˻�
            				if(!remoteData.ResultCD.Equals("0000"))
            				{
            					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
            				}
            
            				// ��� ��Ʈ
            				channelModel.ResultCnt   = remoteData.ResultCnt;
            				channelModel.ResultCD    = remoteData.ResultCD;
            
            				_log.Debug("-----------------------------------------");
            				_log.Debug("ä�λ��� end");
            				_log.Debug("-----------------------------------------");
            			}
            			catch(FrameException fe)
            			{
            				_log.Warning("-----------------------------------------");
            				_log.Warning( this.ToString() + ":setchanneldelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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