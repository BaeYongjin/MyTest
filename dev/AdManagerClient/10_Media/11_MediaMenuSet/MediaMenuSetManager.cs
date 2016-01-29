using System;
using System.Data;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
    /// <summary>
    /// 사용자정보 웹서비스를 호출합니다. 
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
        /// 카테고리정보를 가져온다
        /// </summary>
        /// <param name="model"></param>
        public void GetCategoryList(MediaMenuSetModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("카테고리목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                MediaMenuSetServiceProxy.MediaMenuSetService    svc = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetService();
                svc.Url = _WebServiceUrl;
                
                MediaMenuSetServiceProxy.HeaderModel        remoteHeader = new AdManagerClient.MediaMenuSetServiceProxy.HeaderModel();
                MediaMenuSetServiceProxy.MediaMenuSetModel  remoteData   = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetModel();
			
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchAdType = "M";
			
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetCategoryList( remoteHeader, remoteData );

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.CategoryDataSet   = remoteData.CategoryDataSet.Copy();
                model.ResultCnt         = remoteData.ResultCnt;
                model.ResultCD          = remoteData.ResultCD;
                model.ResultDesc        = remoteData.ResultDesc;

                _log.Debug("-----------------------------------------");
                _log.Debug("카테고리목록조회 End");
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
        /// 장르정보를 가져온다
        /// </summary>
        /// <param name="model"></param>
        public void GetGenreList(MediaMenuSetModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("장르목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                MediaMenuSetServiceProxy.MediaMenuSetService    svc = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetService();
                svc.Url = _WebServiceUrl;
                
                MediaMenuSetServiceProxy.HeaderModel        remoteHeader = new AdManagerClient.MediaMenuSetServiceProxy.HeaderModel();
                MediaMenuSetServiceProxy.MediaMenuSetModel  remoteData   = new AdManagerClient.MediaMenuSetServiceProxy.MediaMenuSetModel();
			
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchAdType = "M";
                remoteData.Category     =   model.Category;
			
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetGenreList( remoteHeader, remoteData );

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.GenreDataSet  = remoteData.GenreDataSet.Copy();
                model.ResultCnt     = remoteData.ResultCnt;
                model.ResultCD      = remoteData.ResultCD;
                model.ResultDesc    = remoteData.ResultDesc;

                _log.Debug("-----------------------------------------");
                _log.Debug("장르목록조회 End");
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
			
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchAdType = "M";
                remoteData.Category     =   model.Category;
			
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.CategoryInsert( remoteHeader, remoteData );

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
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
			
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchAdType = "M";
                remoteData.Category     =   model.Category;
			
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.CategoryDelete( remoteHeader, remoteData );

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
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
			
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchAdType = "M";
                remoteData.Category     =   model.Category;
                remoteData.Genre        =   model.Genre;
			
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GenreInsert( remoteHeader, remoteData );

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
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
			
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchAdType = "M";
                remoteData.Category     =   model.Category;
                remoteData.Genre        =   model.Genre;
			
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GenreDelete( remoteHeader, remoteData );

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
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


    }   /* 클래스 종료 */
}