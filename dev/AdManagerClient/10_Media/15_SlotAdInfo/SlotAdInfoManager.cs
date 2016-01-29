// ===============================================================================
// 
// SlotAdInfoManager.cs
//
// 광고 슬롯 정보 관리 서비스를 호출합니다. 
//
// ===============================================================================
// Copyright (C) Dartmedia.co.
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
    /// 광고 슬롯 정보 관리
	/// </summary>
	public class SlotAdInfoManager : BaseManager
	{


		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SlotAdInfoManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/SlotAdInfoService.asmx";
		}


		/// <summary>
		/// 메뉴 목록조회
		/// </summary>
		/// <param name="slotAdInfoModel"></param>
		public void GetMenuList(SlotAdInfoModel slotAdInfoModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SlotAdInfoServiceProxy.SlotAdInfoService svc = new SlotAdInfoServiceProxy.SlotAdInfoService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SlotAdInfoServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.SlotAdInfoServiceProxy.HeaderModel();
				SlotAdInfoServiceProxy.SlotAdInfoModel remoteData   = new AdManagerClient.SlotAdInfoServiceProxy.SlotAdInfoModel();

				remoteHeader.UserID        = Header.UserID;
                remoteData.SearchMediaCode = slotAdInfoModel.SearchMediaCode;
                remoteData.IsSetDataOnly = slotAdInfoModel.IsSetDataOnly;	
		
				
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.GetMenuList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				slotAdInfoModel.SlotAdInfoDataSet = remoteData.SlotAdInfoDataSet.Copy();
				slotAdInfoModel.ResultCnt   = remoteData.ResultCnt;
				slotAdInfoModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCategenList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 광고 슬롯 정보 생성
        /// </summary>
        /// <param name="slotAdInfoModel"></param>
        public void InsertSlotAdInfo(SlotAdInfoModel slotAdInfoModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고 슬롯 정보 생성 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SlotAdInfoServiceProxy.SlotAdInfoService svc = new SlotAdInfoServiceProxy.SlotAdInfoService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SlotAdInfoServiceProxy.HeaderModel remoteHeader = new AdManagerClient.SlotAdInfoServiceProxy.HeaderModel();
                SlotAdInfoServiceProxy.SlotAdInfoModel remoteData = new AdManagerClient.SlotAdInfoServiceProxy.SlotAdInfoModel();

                remoteHeader.UserID = Header.UserID;
                remoteData.CategoryCode = slotAdInfoModel.CategoryCode;
                remoteData.MenuCode = slotAdInfoModel.MenuCode;
                remoteData.MaxCount = slotAdInfoModel.MaxCount;
                remoteData.MaxTime = slotAdInfoModel.MaxTime;
                remoteData.MaxCountPay = slotAdInfoModel.MaxCountPay;
                remoteData.MaxTimePay = slotAdInfoModel.MaxTimePay;
                remoteData.PromotionYn = slotAdInfoModel.PromotionYn;
                remoteData.UseYn = slotAdInfoModel.UseYn;

                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.InsertSlotAdTypeAssign(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                slotAdInfoModel.SlotAdInfoDataSet = remoteData.SlotAdInfoDataSet.Copy();
                slotAdInfoModel.ResultCnt = remoteData.ResultCnt;
                slotAdInfoModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고 슬롯 정보 생성 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":InsertSlotAdTypeAssign():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 광고 슬롯 정보 수정
        /// </summary>
        /// <param name="slotAdInfoModel"></param>
        public void UpdateSlotAdInfo(SlotAdInfoModel slotAdInfoModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고 슬롯 정보 수정 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SlotAdInfoServiceProxy.SlotAdInfoService svc = new SlotAdInfoServiceProxy.SlotAdInfoService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SlotAdInfoServiceProxy.HeaderModel remoteHeader = new AdManagerClient.SlotAdInfoServiceProxy.HeaderModel();
                SlotAdInfoServiceProxy.SlotAdInfoModel remoteData = new AdManagerClient.SlotAdInfoServiceProxy.SlotAdInfoModel();

                remoteHeader.UserID = Header.UserID;
                remoteData.CategoryCode = slotAdInfoModel.CategoryCode;
                remoteData.MenuCode = slotAdInfoModel.MenuCode;
                remoteData.MaxCount = slotAdInfoModel.MaxCount;
                remoteData.MaxTime = slotAdInfoModel.MaxTime;
                remoteData.MaxCountPay = slotAdInfoModel.MaxCountPay;
                remoteData.MaxTimePay = slotAdInfoModel.MaxTimePay;
                remoteData.PromotionYn = slotAdInfoModel.PromotionYn;
                remoteData.UseYn = slotAdInfoModel.UseYn;

                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.UpdateSlotAdTypeAssign(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                slotAdInfoModel.SlotAdInfoDataSet = remoteData.SlotAdInfoDataSet.Copy();
                slotAdInfoModel.ResultCnt = remoteData.ResultCnt;
                slotAdInfoModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고 슬롯 정보 수정 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":UpdateSlotAdTypeAssign():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 광고 슬롯 정보 삭제
        /// </summary>
        /// <param name="slotAdInfoModel"></param>
        public void DeleteSlotAdInfo(SlotAdInfoModel slotAdInfoModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고 슬롯 정보 삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SlotAdInfoServiceProxy.SlotAdInfoService svc = new SlotAdInfoServiceProxy.SlotAdInfoService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SlotAdInfoServiceProxy.HeaderModel remoteHeader = new AdManagerClient.SlotAdInfoServiceProxy.HeaderModel();
                SlotAdInfoServiceProxy.SlotAdInfoModel remoteData = new AdManagerClient.SlotAdInfoServiceProxy.SlotAdInfoModel();

                remoteHeader.UserID = Header.UserID;
                remoteData.CategoryCode = slotAdInfoModel.CategoryCode;
                remoteData.MenuCode = slotAdInfoModel.MenuCode;
                remoteData.MaxCount = slotAdInfoModel.MaxCount;
                remoteData.MaxTime = slotAdInfoModel.MaxTime;
                remoteData.MaxCountPay = slotAdInfoModel.MaxCountPay;
                remoteData.MaxTimePay = slotAdInfoModel.MaxTimePay;
                remoteData.UseDate = slotAdInfoModel.UseDate;

                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.DeleteSlotAdTypeAssign(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                slotAdInfoModel.SlotAdInfoDataSet = remoteData.SlotAdInfoDataSet.Copy();
                slotAdInfoModel.ResultCnt = remoteData.ResultCnt;
                slotAdInfoModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고 슬롯 정보 삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":UpdateSlotAdTypeAssign():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 광고 슬롯 정보 기본 값 조회
        /// </summary>
        /// <param name="slotAdInfoModel"></param>
        public void GetDefaultSlotAdInfo(SlotAdInfoModel slotAdInfoModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고 슬롯 정보 기본 값 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SlotAdInfoServiceProxy.SlotAdInfoService svc = new SlotAdInfoServiceProxy.SlotAdInfoService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SlotAdInfoServiceProxy.HeaderModel remoteHeader = new AdManagerClient.SlotAdInfoServiceProxy.HeaderModel();
                SlotAdInfoServiceProxy.SlotAdInfoModel remoteData = new AdManagerClient.SlotAdInfoServiceProxy.SlotAdInfoModel();

                remoteHeader.UserID = Header.UserID;
                
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.GetDefaultSlotAdInfo(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                slotAdInfoModel.MaxCount = remoteData.MaxCount;
                slotAdInfoModel.MaxTime = remoteData.MaxTime;
                slotAdInfoModel.MaxCountPay = remoteData.MaxCountPay;
                slotAdInfoModel.MaxTimePay = remoteData.MaxTimePay;
                slotAdInfoModel.PromotionYn = remoteData.PromotionYn;
                slotAdInfoModel.UseYn = remoteData.UseYn;

                slotAdInfoModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고 슬롯 정보 기본 값 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetDefaultSlotAdInfo():" + fe.ErrCode + ":" + fe.ResultMsg);
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