// ===============================================================================
// SchChoiceAd Manager  for Charites Project
//
// ItemManager.cs
//
// 광고파일정보 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : YJ.Park
 * 수정일    : 2014.08.8
 * 수정내용  :        
 *            - 편성 정보가 존해하는 광고조회.
 * -------------------------------------------------------
 * 수정코드	: [E_02]
 * 수정자		: YJ.Park
 * 수정일		: 2014.11.13
 * 수정내용	: 홈광고(키즈) 추가에 따른 메소드 분리
 * --------------------------------------------------------
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
	/// 지정광고편성정보 웹서비스를 호출합니다. 
	/// </summary>
	public class ItemManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public ItemManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "SchChoiceAd";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/ItemService.asmx";
		}

		/// <summary>
		/// 지정광고편성대상조회
		/// </summary>
		/// <param name="schChoiceAdModel"></param>
        public void GetContractItemList(ItemModel itemModel, String callType)//SchChoiceAdModel schChoiceAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("지정광고편성대상조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
                ItemServicePloxy.ItemService svc = new ItemServicePloxy.ItemService();

				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
				
				// 리모트 모델 생성
                ItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ItemServicePloxy.HeaderModel();
                ItemServicePloxy.ItemModel remoteData = new AdManagerClient.ItemServicePloxy.ItemModel();
                
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey             =  itemModel.SearchKey;               
				remoteData.SearchMediaCode		 =  itemModel.SearchMediaCode;
                remoteData.SearchCugCode         =  itemModel.SearchCugCode;
				remoteData.SearchRapCode		 =  itemModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     =  itemModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  =  itemModel.SearchAdvertiserCode;
				remoteData.SearchchkAdState_10	 =  itemModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 =  itemModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 =  itemModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 =  itemModel.SearchchkAdState_40; 
                remoteData.SearchAdType          =  itemModel.SearchAdType;
				remoteData.AdType				 =	itemModel.AdType;
                remoteData.RapCode               =  itemModel.RapCode; 

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                if (callType.Equals("SchHomeAdControl")
                    ||callType.Equals("AreaDayControl")
                    || callType.Equals("ChannelDayControl")
                    || callType.Equals("GenreDayControl")
                    || callType.Equals("TimeDayControl")
                    || callType.Equals("CugHomeAdControl")
                    || callType.Equals("GroupOrganizationControl"))
                {                        
                    // 웹서비스 메소드 호출
				    remoteData = svc.GetContractItemList(remoteHeader, remoteData);
                }
                else if (callType.Equals("SchChoiceAdControl") //30_04
                    || callType.Equals("ChooseAdScheduleControl")// 30_05
                    || callType.Equals("SchGroupControl")// 30_09
                    || callType.Equals("CugChooseAdControl"))// 70_04
                {
                    // 웹서비스 메소드 호출
                    remoteData = svc.GetContractItemList_0907a(remoteHeader, remoteData);
                }
				//else if (callType.Equals("SchHomeCmControl"))// 30_03
				//{
				//    // 웹서비스 메소드 호출
				//    remoteData = svc.GetContractItemListCm(remoteHeader, remoteData);
				//}
				//else if (callType.Equals("CugChoiceAdControl"))// 70_03
				//{
				//    // 웹서비스 메소드 호출
				//    remoteData = svc.GetContractItemListForCug(remoteHeader, remoteData);
				//}
				//else if (callType.Equals("ContractItemControl"))// 70_03
				//{
				//    // 웹서비스 메소드 호출
				//    remoteData = svc.GetContractItemListDual(remoteHeader, remoteData);
				//}
				//// [E_02]
				//else if (callType.Equals("SchHomeKidsControl"))
				//{
				//    remoteData = svc.GetContractKidsItemList(remoteHeader, remoteData);
				//}
				else
				{
					// 웹서비스 메소드 호출
					remoteData = svc.GetContractItemList(remoteHeader, remoteData);
				}

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				itemModel.FileListCount = remoteData.FileListCount;
				itemModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				itemModel.ResultCnt   = remoteData.ResultCnt;
				itemModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContractItemList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 편성정보가 존재하는 광고조회[E_01]
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetSchAdItemList(ItemModel itemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(" 기편성 광고 목록 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                ItemServicePloxy.ItemService svc = new ItemServicePloxy.ItemService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                ItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ItemServicePloxy.HeaderModel();
                ItemServicePloxy.ItemModel remoteData = new AdManagerClient.ItemServicePloxy.ItemModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = itemModel.SearchKey;
                remoteData.SearchMediaCode = itemModel.SearchMediaCode;
                remoteData.SearchchkAdState_10 = itemModel.SearchchkAdState_10;
                remoteData.SearchchkAdState_20 = itemModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = itemModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = itemModel.SearchchkAdState_40;
                remoteData.ItemNo = itemModel.ItemNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetSchAdItemList(remoteHeader, remoteData);


                // 결과 셋트
                itemModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                itemModel.ResultCnt = remoteData.ResultCnt;
                itemModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("기편성 광고 목록 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchAdItemList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
