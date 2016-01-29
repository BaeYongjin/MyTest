// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// �帣���� ���� ���񽺸� ȣ���մϴ�. 
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
    /// �帣���� �����񽺸� ȣ���մϴ�. 
    /// </summary>
    public class ChannelGroupSearch_pManager : BaseManager
    {
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ChannelGroupSearch_pManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Schedule/ChannelGroupService.asmx";
        }

        /// <summary>
        /// �帣������ȸ
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetGenreList(ChannelGroupModel channelGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�����ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
                svc.Url = "http://" + _Host + ":" + _Port + "/" + FrameSystem.m_WebServer_App + "/Schedule/GenreGroupService.asmx";			
                // ����Ʈ �� ����
                GenreGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
                GenreGroupServicePloxy.GenreGroupModel remoteData   = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = channelGroupModel.SearchKey;
                remoteData.MediaCode       = channelGroupModel.MediaCode;
				

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetGenreGroupList_p(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                channelGroupModel.ChannelGroup_pDataSet = remoteData.GenreGroupDataSet.Copy();
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;

                
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelGroupListPopUp():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ä��������ȸ
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetChannelList(ChannelGroupModel channelGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�����ȸ Start");
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
                remoteData.CategoryCode    = channelGroupModel.CategoryCode;
                remoteData.GenreCode    = channelGroupModel.GenreCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetChannelList_p(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                channelGroupModel.ChannelGroup_pDataSet = remoteData.ChannelGroupDataSet.Copy();
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;

                
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelGroupListPopUp():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����_ä��������ȸ
		/// </summary>
		/// <param name="channelGroupModel"></param>
		public void GetChannelList_Excel(ChannelGroupModel channelGroupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���� ä�θ����ȸ Start");
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
				remoteData = svc.GetChannelList_Excel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				channelGroupModel.ChannelGroup_pDataSet = remoteData.ChannelGroupDataSet.Copy();
				channelGroupModel.ResultCnt   = remoteData.ResultCnt;
				channelGroupModel.ResultCD    = remoteData.ResultCD;

                
				_log.Debug("-----------------------------------------");
				_log.Debug("���� ä�θ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelList_Excel():" + fe.ErrCode + ":" + fe.ResultMsg);
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