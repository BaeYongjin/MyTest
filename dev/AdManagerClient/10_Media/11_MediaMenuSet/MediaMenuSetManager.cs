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
    public class MediaMenuSetManager : BaseManager
    {
        public MediaMenuSetManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "MediaMenuSet";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Media/MediaMenuSetService.asmx";
        }

        /// <summary>
        /// ī�װ������� �����´�
        /// </summary>
        /// <param name="model"></param>
        public void GetCategoryList(MediaMenuSetModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ī�װ������ȸ Start");
                _log.Debug("-----------------------------------------");
				
                MediaMenuSetServiceProxy.MediaMenuSetService    svc = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetService();
                svc.Url = _WebServiceUrl;
                
                MediaMenuSetServiceProxy.HeaderModel        remoteHeader = new AdManagerClient.MediaMenuSetServiceProxy.HeaderModel();
                MediaMenuSetServiceProxy.MediaMenuSetModel  remoteData   = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetModel();
			
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchAdType = "M";
			
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetCategoryList( remoteHeader, remoteData );

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.CategoryDataSet   = remoteData.CategoryDataSet.Copy();
                model.ResultCnt         = remoteData.ResultCnt;
                model.ResultCD          = remoteData.ResultCD;
                model.ResultDesc        = remoteData.ResultDesc;

                _log.Debug("-----------------------------------------");
                _log.Debug("ī�װ������ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �帣������ �����´�
        /// </summary>
        /// <param name="model"></param>
        public void GetGenreList(MediaMenuSetModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�����ȸ Start");
                _log.Debug("-----------------------------------------");
				
                MediaMenuSetServiceProxy.MediaMenuSetService    svc = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetService();
                svc.Url = _WebServiceUrl;
                
                MediaMenuSetServiceProxy.HeaderModel        remoteHeader = new AdManagerClient.MediaMenuSetServiceProxy.HeaderModel();
                MediaMenuSetServiceProxy.MediaMenuSetModel  remoteData   = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetModel();
			
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchAdType = "M";
                remoteData.Category     =   model.Category;
			
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetGenreList( remoteHeader, remoteData );

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.GenreDataSet  = remoteData.GenreDataSet.Copy();
                model.ResultCnt     = remoteData.ResultCnt;
                model.ResultCD      = remoteData.ResultCD;
                model.ResultDesc    = remoteData.ResultDesc;

                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetGenreList():" + fe.ErrCode + ":" + fe.ResultMsg);
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


        public void CategoryInsert(MediaMenuSetModel model)
        {
            try
            {
				
                MediaMenuSetServiceProxy.MediaMenuSetService    svc = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetService();
                svc.Url = _WebServiceUrl;
                
                MediaMenuSetServiceProxy.HeaderModel        remoteHeader = new AdManagerClient.MediaMenuSetServiceProxy.HeaderModel();
                MediaMenuSetServiceProxy.MediaMenuSetModel  remoteData   = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetModel();
			
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchAdType = "M";
                remoteData.Category     =   model.Category;
			
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.CategoryInsert( remoteHeader, remoteData );

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.ResultCnt     = remoteData.ResultCnt;
                model.ResultCD      = remoteData.ResultCD;
                model.ResultDesc    = remoteData.ResultDesc;

            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetGenreList():" + fe.ErrCode + ":" + fe.ResultMsg);
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


        public void CategoryDelete(MediaMenuSetModel model)
        {
            try
            {
                MediaMenuSetServiceProxy.MediaMenuSetService    svc = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetService();
                svc.Url = _WebServiceUrl;
                
                MediaMenuSetServiceProxy.HeaderModel        remoteHeader = new AdManagerClient.MediaMenuSetServiceProxy.HeaderModel();
                MediaMenuSetServiceProxy.MediaMenuSetModel  remoteData   = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetModel();
			
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchAdType = "M";
                remoteData.Category     =   model.Category;
			
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.CategoryDelete( remoteHeader, remoteData );

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.ResultCnt     = remoteData.ResultCnt;
                model.ResultCD      = remoteData.ResultCD;
                model.ResultDesc    = remoteData.ResultDesc;

            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetGenreList():" + fe.ErrCode + ":" + fe.ResultMsg);
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


        public void GenreInsert(MediaMenuSetModel model)
        {
            try
            {
                MediaMenuSetServiceProxy.MediaMenuSetService    svc = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetService();
                svc.Url = _WebServiceUrl;
                
                MediaMenuSetServiceProxy.HeaderModel        remoteHeader = new AdManagerClient.MediaMenuSetServiceProxy.HeaderModel();
                MediaMenuSetServiceProxy.MediaMenuSetModel  remoteData   = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetModel();
			
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchAdType = "M";
                remoteData.Category     =   model.Category;
                remoteData.Genre        =   model.Genre;
			
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GenreInsert( remoteHeader, remoteData );

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.ResultCnt     = remoteData.ResultCnt;
                model.ResultCD      = remoteData.ResultCD;
                model.ResultDesc    = remoteData.ResultDesc;

            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GenreInsert():" + fe.ErrCode + ":" + fe.ResultMsg);
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


        public void GenreDelete(MediaMenuSetModel model)
        {
            try
            {
                MediaMenuSetServiceProxy.MediaMenuSetService    svc = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetService();
                svc.Url = _WebServiceUrl;
                
                MediaMenuSetServiceProxy.HeaderModel        remoteHeader = new AdManagerClient.MediaMenuSetServiceProxy.HeaderModel();
                MediaMenuSetServiceProxy.MediaMenuSetModel  remoteData   = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetModel();
			
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchAdType = "M";
                remoteData.Category     =   model.Category;
                remoteData.Genre        =   model.Genre;
			
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GenreDelete( remoteHeader, remoteData );

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.ResultCnt     = remoteData.ResultCnt;
                model.ResultCD      = remoteData.ResultCD;
                model.ResultDesc    = remoteData.ResultDesc;

            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GenreDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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


    }   /* Ŭ���� ���� */
}