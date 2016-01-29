// ===============================================================================
// Targeting Collection Manager Project
//
// TargetingCollectionManager.cs
//
// ���� Ÿ���� ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
// 2012.02.21 RH.Jung ó���ۼ�
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
	/// Ÿ���� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class TargetingCollectionManager : BaseManager
	{
		/// <summary>
		/// ������
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
		/// Ÿ���� ��󱤰� ����Ʈ ��ȸ
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetCollectionList(TargetingCollectionModel model)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� �����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
                remoteData.SearchKey = model.SearchKey;
                remoteData.SearchNonuseYn = model.SearchNonuseYn;    
                
                
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetCollectionList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
                model.CollectionsDataSet = remoteData.CollectionsDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���� �����ȸ End");
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
        /// ������� Ÿ���� ����Ʈ ��ȸ
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingCMList(TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("Ÿ���� �����ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.CollectionCode = model.CollectionCode;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetTargetingCMList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.CMDataSet = remoteData.CMDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("������� Ÿ���� �����ȸ End");
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
        /// ��ü���� Ÿ���� ����Ʈ ��ȸ
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingOAPList(TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("Ÿ���� �����ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.CollectionCode = model.CollectionCode;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetTargetingOAPList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.OAPDataSet = remoteData.OAPDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("��ü���� Ÿ���� �����ȸ End");
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
        /// Ȩ���� Ÿ���� ����Ʈ ��ȸ
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingHomeList(TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("Ÿ���� �����ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.CollectionCode = model.CollectionCode;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetTargetingHomeList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.HomeDataSet = remoteData.HomeDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("Ȩ���� Ÿ���� �����ȸ End");
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
        /// ����Ÿ���� �߰�
        /// </summary>
        /// <param name="targetingModel"></param>
        public void SetTargetingCollectionAdd(TargetingCollectionModel model)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����Ÿ���� �߰� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SetType = model.SetType;
                remoteData.ItemNo = model.ItemNo;
                remoteData.CollectionCode = model.CollectionCode;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetTargetingCollectionAdd(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����Ÿ���� �߰� End");
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
        /// ����Ÿ���� ����
        /// </summary>
        /// <param name="targetingModel"></param>
        public void SetTargetingCollectionDelete(TargetingCollectionModel model)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����Ÿ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingCollectionServiceProxy.TargetingCollectionService svc = new TargetingCollectionServiceProxy.TargetingCollectionService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingCollectionServiceProxy.HeaderModel remoteHeader = new AdManagerClient.TargetingCollectionServiceProxy.HeaderModel();
                TargetingCollectionServiceProxy.TargetingCollectionModel remoteData = new AdManagerClient.TargetingCollectionServiceProxy.TargetingCollectionModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SetType = model.SetType;
                remoteData.ItemNo = model.ItemNo;
                remoteData.CollectionCode = model.CollectionCode;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetTargetingCollectionDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����Ÿ���� ���� End");
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
