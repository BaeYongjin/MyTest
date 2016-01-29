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
    public class GenreGroupManager : BaseManager
    {
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public GenreGroupManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Schedule/GenreGroupService.asmx";
        }

        /// <summary>
        /// �̵���޺�������ȸ
        /// </summary>
        /// <param name="genreGroupModel"></param>
        public void GetMediaList(GenreGroupModel genreGroupModel)
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
                remoteData.SearchKey       = genreGroupModel.SearchKey;
                remoteData.MediaCode = genreGroupModel.MediaCode;
				
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
                genreGroupModel.MediaDataSet = remoteData.UserDataSet.Copy();
                genreGroupModel.ResultCnt   = remoteData.ResultCnt;
                genreGroupModel.ResultCD    = remoteData.ResultCD;

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
        /// <param name="genreGroupModel"></param>
        public void GetGenreGroupList(GenreGroupModel genreGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷�����ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
			    svc.Url = _WebServiceUrl;
                // ����Ʈ �� ����
                GenreGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
                GenreGroupServicePloxy.GenreGroupModel remoteData   = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = genreGroupModel.SearchKey;
                remoteData.MediaCode       = genreGroupModel.MediaCode;
				
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetGenreGroupList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                genreGroupModel.GenreGroupDataSet = remoteData.GenreGroupDataSet.Copy();
                genreGroupModel.ResultCnt   = remoteData.ResultCnt;
                genreGroupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷�����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetGenreGroupList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// <param name="genreGroupModel"></param>
        public void GetGenreGroupDetailList(GenreGroupModel genreGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷� ������ ��ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
			    svc.Url = _WebServiceUrl;
                // ����Ʈ �� ����
                GenreGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
                GenreGroupServicePloxy.GenreGroupModel remoteData   = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.MediaCode       = genreGroupModel.MediaCode;
                remoteData.AdGroupCode      = genreGroupModel.AdGroupCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetGenreGroupDetailList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                genreGroupModel.GenreGroupDataSet = remoteData.GenreGroupDataSet.Copy();
                genreGroupModel.ResultCnt   = remoteData.ResultCnt;
                genreGroupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷� ������ �����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetGenreGroupList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
   
        public void SetGenreGroupAdd(GenreGroupModel genreGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷��߰� Start");
                _log.Debug("-----------------------------------------");

            
                // ������ �ν��Ͻ� ����
                GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
            	svc.Url = _WebServiceUrl;			
                // ����Ʈ �� ����
                GenreGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
                GenreGroupServicePloxy.GenreGroupModel remoteData   = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ��������Ʈ
                remoteData.MediaCode            = genreGroupModel.MediaCode;
                remoteData.AdGroupName          = genreGroupModel.AdGroupName;
                remoteData.Comment              = genreGroupModel.Comment;
                remoteData.UseYn                = genreGroupModel.UseYn;
                remoteData.GenreGroupDataSet    = genreGroupModel.GenreGroupDataSet.Copy();

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetGenreGroupCreate(remoteHeader, remoteData);
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                genreGroupModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("genreGroupModel.ResultCnt = "+genreGroupModel.ResultCnt);
            			
                genreGroupModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷��߰� End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                genreGroupModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetGenreGroupCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                genreGroupModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// �帣�׷����� ����
        /// </summary>
        /// <param name="genreGroupModel"></param>
        public void SetGenreGroupUpdate(GenreGroupModel genreGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷��������� Start");
                _log.Debug("-----------------------------------------");
            
            
                //				�Էµ������� Validation �˻�
                if(genreGroupModel.AdGroupName.Trim().Length < 1) 
                {
                    throw new FrameException("���� ��ǰ���� �������� �ʽ��ϴ�.");
                }
                if(genreGroupModel.AdGroupName.Trim().Length > 50) 
                {
                    throw new FrameException("���� ��ǰ���� 50Byte�� �ʰ��� �� �����ϴ�.");
                }
                if(genreGroupModel.Comment.Trim().Length > 100) 
                {
                    throw new FrameException("�׷켳���� 100Byte�� �ʰ��� �� �����ϴ�.");
                }     
            	
            
                // ������ �ν��Ͻ� ����
                GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
            	svc.Url = _WebServiceUrl;		
                // ����Ʈ �� ����
                GenreGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
                GenreGroupServicePloxy.GenreGroupModel remoteData   = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ��������Ʈ
                remoteData.MediaCode            = genreGroupModel.MediaCode;
                remoteData.AdGroupCode          = genreGroupModel.AdGroupCode;
                remoteData.AdGroupName          = genreGroupModel.AdGroupName;
                remoteData.Comment              = genreGroupModel.Comment;
                remoteData.UseYn                = genreGroupModel.UseYn;
                remoteData.GenreGroupDataSet    = genreGroupModel.GenreGroupDataSet.Copy();
            				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetGenreGroupUpdate(remoteHeader, remoteData);
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                genreGroupModel.ResultCnt   = remoteData.ResultCnt;
                genreGroupModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷��������� End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetGenreGroupUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// <param name="genreGroupModel"></param>
        /// <returns></returns>	
        /// <summary>
        /// �帣�׷� ����
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetGenreGroupDelete(GenreGroupModel genreGroupModel)
        {
            
        }

    }
}