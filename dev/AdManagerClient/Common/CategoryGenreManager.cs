using System;
using System.Data;
using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;
using AdManagerModel;

namespace AdManagerClient.Common
{
	/// <summary>
	/// CategoryGenreManager에 대한 요약 설명입니다.
	/// </summary>
	public class CategoryGenreManager : BaseManager 
	{
		public CategoryGenreManager( SystemModel systemModel, CommonModel commonModel) : base( systemModel, commonModel)
		{
            _log = FrameSystem.oLog;
            _module = "CategoryGenre";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Common/CategoryGenreService.asmx";
		}


        /// <summary>
        /// 카테고리리스트를 읽어온다
        /// </summary>
        /// <param name="model"></param>
        public void GetCategoryList(CategoryModel  model)
        {
            try
            {
                CategoryGenreServiceProxy.CategoryGenreService  svc = new AdManagerClient.CategoryGenreServiceProxy.CategoryGenreService();
                CategoryGenreServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.CategoryGenreServiceProxy.HeaderModel();
                CategoryGenreServiceProxy.CategoryModel remoteData   = new AdManagerClient.CategoryGenreServiceProxy.CategoryModel();
			
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;
			
                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.GetCategoryList( remoteHeader, remoteData );

                if(!remoteData.ResultCD.Equals("0000")) throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);

                model.UserDataSet   = remoteData.UserDataSet.Copy();
                model.ResultCnt     = remoteData.ResultCnt;
                model.ResultCD      = remoteData.ResultCD;
                model.ResultDesc    = remoteData.ResultDesc;
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
        public void GetGenreList(GenreModel model)
        {
            try
            {
                CategoryGenreServiceProxy.CategoryGenreService  svc = new AdManagerClient.CategoryGenreServiceProxy.CategoryGenreService();
                CategoryGenreServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.CategoryGenreServiceProxy.HeaderModel();
                CategoryGenreServiceProxy.GenreModel    remoteData   = new AdManagerClient.CategoryGenreServiceProxy.GenreModel();
			
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                remoteData.CategoryCode     = model.CategoryCode;
			
                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.GetGenreList( remoteHeader, remoteData );

                if(!remoteData.ResultCD.Equals("0000")) throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);

                model.UserDataSet   = remoteData.UserDataSet.Copy();
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
	}
}
