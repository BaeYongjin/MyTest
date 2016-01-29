// ===============================================================================
// SchChoiceAd Manager  for Charites Project
//
// ItemManager.cs
//
// ������������ ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : YJ.Park
 * ������    : 2014.08.8
 * ��������  :        
 *            - �� ������ �����ϴ� ������ȸ.
 * -------------------------------------------------------
 * �����ڵ�	: [E_02]
 * ������		: YJ.Park
 * ������		: 2014.11.13
 * ��������	: Ȩ����(Ű��) �߰��� ���� �޼ҵ� �и�
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
	/// �������������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class ItemManager : BaseManager
	{
		/// <summary>
		/// ������
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
		/// ���������������ȸ
		/// </summary>
		/// <param name="schChoiceAdModel"></param>
        public void GetContractItemList(ItemModel itemModel, String callType)//SchChoiceAdModel schChoiceAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���������������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
                ItemServicePloxy.ItemService svc = new ItemServicePloxy.ItemService();

				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
				
				// ����Ʈ �� ����
                ItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ItemServicePloxy.HeaderModel();
                ItemServicePloxy.ItemModel remoteData = new AdManagerClient.ItemServicePloxy.ItemModel();
                
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
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

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                if (callType.Equals("SchHomeAdControl")
                    ||callType.Equals("AreaDayControl")
                    || callType.Equals("ChannelDayControl")
                    || callType.Equals("GenreDayControl")
                    || callType.Equals("TimeDayControl")
                    || callType.Equals("CugHomeAdControl")
                    || callType.Equals("GroupOrganizationControl"))
                {                        
                    // ������ �޼ҵ� ȣ��
				    remoteData = svc.GetContractItemList(remoteHeader, remoteData);
                }
                else if (callType.Equals("SchChoiceAdControl") //30_04
                    || callType.Equals("ChooseAdScheduleControl")// 30_05
                    || callType.Equals("SchGroupControl")// 30_09
                    || callType.Equals("CugChooseAdControl"))// 70_04
                {
                    // ������ �޼ҵ� ȣ��
                    remoteData = svc.GetContractItemList_0907a(remoteHeader, remoteData);
                }
				//else if (callType.Equals("SchHomeCmControl"))// 30_03
				//{
				//    // ������ �޼ҵ� ȣ��
				//    remoteData = svc.GetContractItemListCm(remoteHeader, remoteData);
				//}
				//else if (callType.Equals("CugChoiceAdControl"))// 70_03
				//{
				//    // ������ �޼ҵ� ȣ��
				//    remoteData = svc.GetContractItemListForCug(remoteHeader, remoteData);
				//}
				//else if (callType.Equals("ContractItemControl"))// 70_03
				//{
				//    // ������ �޼ҵ� ȣ��
				//    remoteData = svc.GetContractItemListDual(remoteHeader, remoteData);
				//}
				//// [E_02]
				//else if (callType.Equals("SchHomeKidsControl"))
				//{
				//    remoteData = svc.GetContractKidsItemList(remoteHeader, remoteData);
				//}
				else
				{
					// ������ �޼ҵ� ȣ��
					remoteData = svc.GetContractItemList(remoteHeader, remoteData);
				}

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				itemModel.FileListCount = remoteData.FileListCount;
				itemModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				itemModel.ResultCnt   = remoteData.ResultCnt;
				itemModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������ϸ����ȸ End");
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
        /// �������� �����ϴ� ������ȸ[E_01]
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetSchAdItemList(ItemModel itemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(" ���� ���� ��� ��ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                ItemServicePloxy.ItemService svc = new ItemServicePloxy.ItemService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                ItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ItemServicePloxy.HeaderModel();
                ItemServicePloxy.ItemModel remoteData = new AdManagerClient.ItemServicePloxy.ItemModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchKey = itemModel.SearchKey;
                remoteData.SearchMediaCode = itemModel.SearchMediaCode;
                remoteData.SearchchkAdState_10 = itemModel.SearchchkAdState_10;
                remoteData.SearchchkAdState_20 = itemModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = itemModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = itemModel.SearchchkAdState_40;
                remoteData.ItemNo = itemModel.ItemNo;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetSchAdItemList(remoteHeader, remoteData);


                // ��� ��Ʈ
                itemModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                itemModel.ResultCnt = remoteData.ResultCnt;
                itemModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ��� ��ȸ End");
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
