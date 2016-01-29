// ===============================================================================
// 
// SlotAdInfoManager.cs
//
// ���� ���� ���� ���� ���񽺸� ȣ���մϴ�. 
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
    /// ���� ���� ���� ����
	/// </summary>
	public class SlotAdInfoManager : BaseManager
	{


		/// <summary>
		/// ������
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
		/// �޴� �����ȸ
		/// </summary>
		/// <param name="slotAdInfoModel"></param>
		public void GetMenuList(SlotAdInfoModel slotAdInfoModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SlotAdInfoServiceProxy.SlotAdInfoService svc = new SlotAdInfoServiceProxy.SlotAdInfoService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SlotAdInfoServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.SlotAdInfoServiceProxy.HeaderModel();
				SlotAdInfoServiceProxy.SlotAdInfoModel remoteData   = new AdManagerClient.SlotAdInfoServiceProxy.SlotAdInfoModel();

				remoteHeader.UserID        = Header.UserID;
                remoteData.SearchMediaCode = slotAdInfoModel.SearchMediaCode;
                remoteData.IsSetDataOnly = slotAdInfoModel.IsSetDataOnly;	
		
				
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.GetMenuList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				slotAdInfoModel.SlotAdInfoDataSet = remoteData.SlotAdInfoDataSet.Copy();
				slotAdInfoModel.ResultCnt   = remoteData.ResultCnt;
				slotAdInfoModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴� ��ȸ End");
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
        /// ���� ���� ���� ����
        /// </summary>
        /// <param name="slotAdInfoModel"></param>
        public void InsertSlotAdInfo(SlotAdInfoModel slotAdInfoModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SlotAdInfoServiceProxy.SlotAdInfoService svc = new SlotAdInfoServiceProxy.SlotAdInfoService();
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
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

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                slotAdInfoModel.SlotAdInfoDataSet = remoteData.SlotAdInfoDataSet.Copy();
                slotAdInfoModel.ResultCnt = remoteData.ResultCnt;
                slotAdInfoModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ���� ���� End");
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
        /// ���� ���� ���� ����
        /// </summary>
        /// <param name="slotAdInfoModel"></param>
        public void UpdateSlotAdInfo(SlotAdInfoModel slotAdInfoModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SlotAdInfoServiceProxy.SlotAdInfoService svc = new SlotAdInfoServiceProxy.SlotAdInfoService();
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
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

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                slotAdInfoModel.SlotAdInfoDataSet = remoteData.SlotAdInfoDataSet.Copy();
                slotAdInfoModel.ResultCnt = remoteData.ResultCnt;
                slotAdInfoModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ���� ���� End");
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
        /// ���� ���� ���� ����
        /// </summary>
        /// <param name="slotAdInfoModel"></param>
        public void DeleteSlotAdInfo(SlotAdInfoModel slotAdInfoModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SlotAdInfoServiceProxy.SlotAdInfoService svc = new SlotAdInfoServiceProxy.SlotAdInfoService();
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
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

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                slotAdInfoModel.SlotAdInfoDataSet = remoteData.SlotAdInfoDataSet.Copy();
                slotAdInfoModel.ResultCnt = remoteData.ResultCnt;
                slotAdInfoModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ���� ���� End");
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
        /// ���� ���� ���� �⺻ �� ��ȸ
        /// </summary>
        /// <param name="slotAdInfoModel"></param>
        public void GetDefaultSlotAdInfo(SlotAdInfoModel slotAdInfoModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ���� �⺻ �� ��ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SlotAdInfoServiceProxy.SlotAdInfoService svc = new SlotAdInfoServiceProxy.SlotAdInfoService();
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SlotAdInfoServiceProxy.HeaderModel remoteHeader = new AdManagerClient.SlotAdInfoServiceProxy.HeaderModel();
                SlotAdInfoServiceProxy.SlotAdInfoModel remoteData = new AdManagerClient.SlotAdInfoServiceProxy.SlotAdInfoModel();

                remoteHeader.UserID = Header.UserID;
                
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.GetDefaultSlotAdInfo(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                slotAdInfoModel.MaxCount = remoteData.MaxCount;
                slotAdInfoModel.MaxTime = remoteData.MaxTime;
                slotAdInfoModel.MaxCountPay = remoteData.MaxCountPay;
                slotAdInfoModel.MaxTimePay = remoteData.MaxTimePay;
                slotAdInfoModel.PromotionYn = remoteData.PromotionYn;
                slotAdInfoModel.UseYn = remoteData.UseYn;

                slotAdInfoModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ���� �⺻ �� ��ȸ End");
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