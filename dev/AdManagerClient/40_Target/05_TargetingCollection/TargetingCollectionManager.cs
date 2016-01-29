// ===============================================================================
// Targeting Collection Manager Project
//
// TargetingCollectionManager.cs
//
// 고객군 타겟팅 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
// 2012.02.21 RH.Jung 처음작성
// ===============================================================================
// Copyright (C) 2012 DARTmeda
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
	/// 타겟팅 웹서비스를 호출합니다. 
	/// </summary>
	public class TargetingCollectionManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public TargetingCollectionManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "Targeting";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Target/TargetingCollectionService.asmx";
		}


		/// <summary>
		/// 타겟팅 대상광고 리스트 조회
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetCollectionList(TargetingCollectionModel model)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
                remoteData.SearchKey = model.SearchKey;
                remoteData.SearchNonuseYn = model.SearchNonuseYn;    
                
                
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetCollectionList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
                model.CollectionsDataSet = remoteData.CollectionsDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("고객군 목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCollectionList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 상업광고 타겟팅 리스트 조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingCMList(TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("타겟팅 목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.CollectionCode = model.CollectionCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetTargetingCMList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.CMDataSet = remoteData.CMDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("상업광고 타겟팅 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetTargetingCMList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 매체광고 타겟팅 리스트 조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingOAPList(TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("타겟팅 목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.CollectionCode = model.CollectionCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetTargetingOAPList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.OAPDataSet = remoteData.OAPDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("매체광고 타겟팅 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetTargetingHomeList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 홈광고 타겟팅 리스트 조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingHomeList(TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("타겟팅 목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.CollectionCode = model.CollectionCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetTargetingHomeList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.HomeDataSet = remoteData.HomeDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고 타겟팅 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetTargetingHomeList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 고객군타켓팅 추가
        /// </summary>
        /// <param name="targetingModel"></param>
        public void SetTargetingCollectionAdd(TargetingCollectionModel model)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("고객군타겟팅 추가 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SetType = model.SetType;
                remoteData.ItemNo = model.ItemNo;
                remoteData.CollectionCode = model.CollectionCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetTargetingCollectionAdd(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("고객군타겟팅 추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetTargetingCollectionAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 고객군타켓팅 삭제
        /// </summary>
        /// <param name="targetingModel"></param>
        public void SetTargetingCollectionDelete(TargetingCollectionModel model)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("고객군타겟팅 삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SetType = model.SetType;
                remoteData.ItemNo = model.ItemNo;
                remoteData.CollectionCode = model.CollectionCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetTargetingCollectionDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("고객군타겟팅 삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetTargetingCollectionDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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


	}
}
