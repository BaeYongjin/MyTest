/*
 * -------------------------------------------------------
 * Class Name: MenuManager
 * 주요기능  : 메뉴관리 서비스 호출
 * 작성자    : YJ.Park
 * 작성일    : 2014.08.20
 * -------------------------------------------------------
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
    /// 채널정보 웹서비스를 호출합니다.
    /// </summary>
    class MediaMenuManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>
        public MediaMenuManager(SystemModel systemModel, CommonModel commonModel)
            : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host = FrameSystem.m_WebServer_Host;
            _Port = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/Media/MediaMenuService.asmx";
        }

        /// <summary>
        /// 카테고리 정보조회
        /// </summary>
        /// <param name="mediaMenuModel"></param>
        public void GetCategoryList(MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("카테고리 목록 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                MediaMenuServiceRef.MediaMenuService svc = new MediaMenuServiceRef.MediaMenuService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                MediaMenuServiceRef.HeaderModel remoteHeader = new AdManagerClient.MediaMenuServiceRef.HeaderModel();
                MediaMenuServiceRef.MediaMenuModel remoteData = new AdManagerClient.MediaMenuServiceRef.MediaMenuModel();

                // 헤더정보 셋트
                remoteHeader.UserID = Header.UserID;

                // 호출정보 셋트				
                remoteData.SearchMediaCode = mediaMenuModel.SearchMediaCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetCategoryList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                mediaMenuModel.UserDataset = remoteData.UserDataset.Copy();
                mediaMenuModel.ResultCnt = remoteData.ResultCnt;
                mediaMenuModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("카테고리 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }
        /// <summary>
        /// 메뉴 정보조회
        /// </summary>
        /// <param name="mediaMenuModel"></param>
        public void GetMenuList(MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴 목록 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                MediaMenuServiceRef.MediaMenuService svc = new MediaMenuServiceRef.MediaMenuService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                MediaMenuServiceRef.HeaderModel remoteHeader = new AdManagerClient.MediaMenuServiceRef.HeaderModel();
                MediaMenuServiceRef.MediaMenuModel remoteData = new AdManagerClient.MediaMenuServiceRef.MediaMenuModel();

                // 헤더정보 셋트
                remoteHeader.UserID = Header.UserID;

                // 호출정보 셋트				
                remoteData.MediaCode = mediaMenuModel.MediaCode;
                remoteData.CategoryCode = mediaMenuModel.CategoryCode;
                remoteData.InvalidityMenu = mediaMenuModel.InvalidityMenu;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetMenuList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                mediaMenuModel.UserDataset = remoteData.UserDataset.Copy();
                mediaMenuModel.ResultCnt = remoteData.ResultCnt;
                mediaMenuModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetMenuList():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// 카테고리 정보 수정
        /// </summary>
        /// <param name="mediaMenuModel"></param>
        public void SetCategoryUpdate(MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("카테고리수정 Start");
                _log.Debug("-----------------------------------------");

                //if (mediaMenuModel.CategoryName.Length > 20)
                //{
                //    throw new FrameException("카테고리명칭은 20Bytes를 초과할 수 없습니다.");
                //}

                // 웹서비스 인스턴스 생성
                MediaMenuServiceRef.MediaMenuService svc = new MediaMenuServiceRef.MediaMenuService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                MediaMenuServiceRef.HeaderModel remoteHeader = new AdManagerClient.MediaMenuServiceRef.HeaderModel();
                MediaMenuServiceRef.MediaMenuModel remoteData = new AdManagerClient.MediaMenuServiceRef.MediaMenuModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;

                remoteData.MediaCode    = mediaMenuModel.MediaCode;
                remoteData.CategoryCode = mediaMenuModel.CategoryCode;
                
                remoteData.CategoryAdPreRollYn  = mediaMenuModel.CategoryAdPreRollYn;
                remoteData.CategoryAdMidRollYn  = mediaMenuModel.CategoryAdMidRollYn;
                remoteData.CategoryAdPostRollYn = mediaMenuModel.CategoryAdPostRollYn;
                remoteData.CategoryAdPayYn      = mediaMenuModel.CategoryAdPayYn;

                remoteData.MenuAdPreRollYn  = mediaMenuModel.MenuAdPreRollYn;
                remoteData.MenuAdMidRollYn  = mediaMenuModel.MenuAdMidRollYn;
                remoteData.MenuAdPostRollYn = mediaMenuModel.MenuAdPostRollYn;
                remoteData.MenuAdPayYn      = mediaMenuModel.MenuAdPayYn;

                remoteData.CategoryAdRate   = mediaMenuModel.CategoryAdRate;
                remoteData.CategoryAdNRate  = mediaMenuModel.CategoryAdNRate;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetCategoryUpdate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                mediaMenuModel.ResultCnt = remoteData.ResultCnt;
                mediaMenuModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("카테고리수정 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetCategoryUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

		public void SetCategoryUpdateOption(MediaMenuModel mediaMenuModel)
		{
			try
			{
				MediaMenuServiceRef.MediaMenuService svc = new MediaMenuServiceRef.MediaMenuService();
				
				MediaMenuServiceRef.HeaderModel remoteHeader = new AdManagerClient.MediaMenuServiceRef.HeaderModel();
				MediaMenuServiceRef.MediaMenuModel remoteData = new AdManagerClient.MediaMenuServiceRef.MediaMenuModel();

				remoteHeader.ClientKey = Header.ClientKey;
				remoteHeader.UserID = Header.UserID;

				remoteData.MediaCode = mediaMenuModel.MediaCode;
				remoteData.CategoryCode = mediaMenuModel.CategoryCode;
				remoteData.CategoryName = mediaMenuModel.CategoryName;
				remoteData.mType = (MediaMenuServiceRef.MediaMenuType)mediaMenuModel.mType;
				remoteData.mValue = mediaMenuModel.mValue;

				// 웹서비스 호출 타임아웃설정
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetCategoryUpdateOption(remoteHeader, remoteData);

				// 결과코드검사
				if (!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				mediaMenuModel.ResultCnt = remoteData.ResultCnt;
				mediaMenuModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("카테고리수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch (FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning(this.ToString() + ":SetCategoryUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch (Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

        /// <summary>
        /// 메뉴 정보 수정
        /// </summary>
        /// <param name="mediaMenuModel"></param>
        public void SetMenuUpdate(MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴수정 Start");
                _log.Debug("-----------------------------------------");

                //if (mediaMenuModel.MenuName.Length > 30)
                //{
                //    throw new FrameException("메뉴명칭은 30Bytes를 초과할 수 없습니다.");
                //}

                // 웹서비스 인스턴스 생성
                MediaMenuServiceRef.MediaMenuService svc = new MediaMenuServiceRef.MediaMenuService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                MediaMenuServiceRef.HeaderModel remoteHeader = new AdManagerClient.MediaMenuServiceRef.HeaderModel();
                MediaMenuServiceRef.MediaMenuModel remoteData = new AdManagerClient.MediaMenuServiceRef.MediaMenuModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                remoteData.MenuCode = mediaMenuModel.MenuCode;
                remoteData.CategoryCode = mediaMenuModel.CategoryCode;

                remoteData.CategoryAdPreRollYn = mediaMenuModel.CategoryAdPreRollYn;
                remoteData.CategoryAdMidRollYn = mediaMenuModel.CategoryAdMidRollYn;
                remoteData.CategoryAdPostRollYn = mediaMenuModel.CategoryAdPostRollYn;
                remoteData.CategoryAdPayYn = mediaMenuModel.CategoryAdPayYn;

                remoteData.MenuAdPreRollYn = mediaMenuModel.MenuAdPreRollYn;
                remoteData.MenuAdMidRollYn = mediaMenuModel.MenuAdMidRollYn;
                remoteData.MenuAdPostRollYn = mediaMenuModel.MenuAdPostRollYn;
                remoteData.MenuAdPayYn = mediaMenuModel.MenuAdPayYn;

                remoteData.MenuAdRate = mediaMenuModel.MenuAdRate;
                remoteData.MenuAdNRate = mediaMenuModel.MenuAdNRate;

                remoteData.CategoryAdRate = mediaMenuModel.CategoryAdRate;
                remoteData.CategoryAdNRate = mediaMenuModel.CategoryAdNRate;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetMenuUpdate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                mediaMenuModel.ResultCnt = remoteData.ResultCnt;
                mediaMenuModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴수정 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetMenuUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

		public void SetMenuUpdateOption(MediaMenuModel mediaMenuModel)
		{
			try
			{
				MediaMenuServiceRef.MediaMenuService svc = new MediaMenuServiceRef.MediaMenuService();

				MediaMenuServiceRef.HeaderModel remoteHeader = new AdManagerClient.MediaMenuServiceRef.HeaderModel();
				MediaMenuServiceRef.MediaMenuModel remoteData = new AdManagerClient.MediaMenuServiceRef.MediaMenuModel();

				remoteHeader.ClientKey = Header.ClientKey;
				remoteHeader.UserID = Header.UserID;

				remoteData.MediaCode = mediaMenuModel.MediaCode;
				remoteData.CategoryCode = mediaMenuModel.CategoryCode;
				remoteData.MenuCode = mediaMenuModel.MenuCode;
				remoteData.MenuName = mediaMenuModel.MenuName;
				remoteData.mType = (MediaMenuServiceRef.MediaMenuType)mediaMenuModel.mType;
				remoteData.mValue = mediaMenuModel.mValue;

				// 웹서비스 호출 타임아웃설정
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetMenuUpdateOption(remoteHeader, remoteData);

				// 결과코드검사
				if (!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				mediaMenuModel.ResultCnt = remoteData.ResultCnt;
				mediaMenuModel.ResultCD = remoteData.ResultCD;
			}
			catch (FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning(this.ToString() + ":SetCategoryUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch (Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

        /// <summary>
        /// 메뉴 정보 삭제
        /// </summary>
        /// <param name="mediaMenuModel"></param>
        public void DeleteMenu(MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴 정보삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                MediaMenuServiceRef.MediaMenuService svc = new MediaMenuServiceRef.MediaMenuService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                MediaMenuServiceRef.HeaderModel remoteHeader = new AdManagerClient.MediaMenuServiceRef.HeaderModel();
                MediaMenuServiceRef.MediaMenuModel remoteData = new AdManagerClient.MediaMenuServiceRef.MediaMenuModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                remoteData.MediaCode = mediaMenuModel.MediaCode;
                remoteData.MenuCode = mediaMenuModel.MenuCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.DeleteMenu(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                mediaMenuModel.ResultCnt = remoteData.ResultCnt;
                mediaMenuModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴 정보삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":DeleteMenu():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// 카테고리 추가
        /// </summary>
        /// <param name="mediaMenuModel"></param>
        public void SetCategoryCreate(MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("카테고리 추가 Start");
                _log.Debug("-----------------------------------------");

                if (mediaMenuModel.CategoryName.Length > 20)
                {
                    throw new FrameException("카테고리명칭은 50Bytes를 초과할 수 없습니다.");
                }
               
                // 웹서비스 인스턴스 생성
                MediaMenuServiceRef.MediaMenuService svc = new MediaMenuServiceRef.MediaMenuService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                MediaMenuServiceRef.HeaderModel remoteHeader = new AdManagerClient.MediaMenuServiceRef.HeaderModel();
                MediaMenuServiceRef.MediaMenuModel remoteData = new AdManagerClient.MediaMenuServiceRef.MediaMenuModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보셋트
                remoteData.MediaCode = mediaMenuModel.MediaCode;
                remoteData.CategoryName = mediaMenuModel.CategoryName;
                //remoteData.CategorySortNo = mediaMenuModel.CategorySortNo;
                //remoteData.CategoryReplayYn = mediaMenuModel.CategoryReplayYn;
                //remoteData.CategoryReplayPPx = mediaMenuModel.CategoryReplayPPx;
                //remoteData.CategoryREndingYn = mediaMenuModel.CategoryREndingYn;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetCategoryCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                mediaMenuModel.ResultCnt = remoteData.ResultCnt;
                mediaMenuModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("카테고리 추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                mediaMenuModel.ResultCD = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetCategoryCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                mediaMenuModel.ResultCD = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }
        /// <summary>
        /// 메뉴 추가
        /// </summary>
        /// <param name="mediaMenuModel"></param>
        public void SetMenuCreate(MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴추가 Start");
                _log.Debug("-----------------------------------------");

                if (mediaMenuModel.MenuName.Length > 50)
                {
                    throw new FrameException("메뉴명칭은 50Bytes를 초과할 수 없습니다.");
                }
                

                // 웹서비스 인스턴스 생성
                MediaMenuServiceRef.MediaMenuService svc = new MediaMenuServiceRef.MediaMenuService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                MediaMenuServiceRef.HeaderModel remoteHeader = new AdManagerClient.MediaMenuServiceRef.HeaderModel();
                MediaMenuServiceRef.MediaMenuModel remoteData = new AdManagerClient.MediaMenuServiceRef.MediaMenuModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보셋트
                remoteData.MediaCode = mediaMenuModel.MediaCode;
                remoteData.MenuName = mediaMenuModel.MenuName;
                remoteData.CategoryCode = mediaMenuModel.CategoryCode;
                //remoteData.MenuOrder = mediaMenuModel.MenuOrder;
                //remoteData.MenuReplayYn = mediaMenuModel.MenuReplayYn;
                //remoteData.MenuREndingYn = mediaMenuModel.MenuREndingYn;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetMenuCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                mediaMenuModel.ResultCnt = remoteData.ResultCnt;
                mediaMenuModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                mediaMenuModel.ResultCD = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetMenuCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                mediaMenuModel.ResultCD = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

    }
}
