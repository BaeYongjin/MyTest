// ===============================================================================
// 
// SchOrgGenreManager.cs
//
// ���帣 ��� OAP �� ���� ���񽺸� ȣ���մϴ�. 
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
	public class SchOrgGenreManager : BaseManager
	{


		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SchOrgGenreManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/SchOrgGenreService.asmx";
		}


		/// <summary>
		/// �޴� �����ȸ
		/// </summary>
		/// <param name="schOrgGenreModel"></param>
		public void GetMenuList(SchOrgGenreModel schOrgGenreModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchOrgGenreServiceProxy.SchOrgGenreService svc = new SchOrgGenreServiceProxy.SchOrgGenreService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SchOrgGenreServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.SchOrgGenreServiceProxy.HeaderModel();
				SchOrgGenreServiceProxy.SchOrgGenreModel remoteData   = new AdManagerClient.SchOrgGenreServiceProxy.SchOrgGenreModel();

				remoteHeader.UserID        = Header.UserID;
                remoteData.SearchMediaCode = schOrgGenreModel.SearchMediaCode;
                remoteData.IsSetDataOnly = schOrgGenreModel.IsSetDataOnly;
                remoteData.SearchKey = schOrgGenreModel.SearchKey;
		
				
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.GetMenuList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schOrgGenreModel.SchOrgGenreDataSet = remoteData.SchOrgGenreDataSet.Copy();
				schOrgGenreModel.ResultCnt   = remoteData.ResultCnt;
				schOrgGenreModel.ResultCD    = remoteData.ResultCD;

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
        /// <param name="schOrgGenreModel"></param>
        public void InsertSchOrgGenre(SchOrgGenreModel schOrgGenreModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchOrgGenreServiceProxy.SchOrgGenreService svc = new SchOrgGenreServiceProxy.SchOrgGenreService();
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchOrgGenreServiceProxy.HeaderModel remoteHeader = new AdManagerClient.SchOrgGenreServiceProxy.HeaderModel();
                SchOrgGenreServiceProxy.SchOrgGenreModel remoteData = new AdManagerClient.SchOrgGenreServiceProxy.SchOrgGenreModel();

                remoteHeader.UserID = Header.UserID;
                remoteData.CategoryCode = schOrgGenreModel.CategoryCode;
                remoteData.MenuCode = schOrgGenreModel.MenuCode;
                remoteData.MaxCount = schOrgGenreModel.MaxCount;
                remoteData.MaxTime = schOrgGenreModel.MaxTime;
                remoteData.MaxCountPay = schOrgGenreModel.MaxCountPay;
                remoteData.MaxTimePay = schOrgGenreModel.MaxTimePay;
                remoteData.UseDate = schOrgGenreModel.UseDate;

                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.InsertSchOrgGenre(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schOrgGenreModel.SchOrgGenreDataSet = remoteData.SchOrgGenreDataSet.Copy();
                schOrgGenreModel.ResultCnt = remoteData.ResultCnt;
                schOrgGenreModel.ResultCD = remoteData.ResultCD;

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
        /// <param name="schOrgGenreModel"></param>
        public void UpdateSchOrgGenre(SchOrgGenreModel schOrgGenreModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchOrgGenreServiceProxy.SchOrgGenreService svc = new SchOrgGenreServiceProxy.SchOrgGenreService();
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchOrgGenreServiceProxy.HeaderModel remoteHeader = new AdManagerClient.SchOrgGenreServiceProxy.HeaderModel();
                SchOrgGenreServiceProxy.SchOrgGenreModel remoteData = new AdManagerClient.SchOrgGenreServiceProxy.SchOrgGenreModel();

                remoteHeader.UserID = Header.UserID;
                remoteData.CategoryCode = schOrgGenreModel.CategoryCode;
                remoteData.MenuCode = schOrgGenreModel.MenuCode;
                remoteData.MaxCount = schOrgGenreModel.MaxCount;
                remoteData.MaxTime = schOrgGenreModel.MaxTime;
                remoteData.MaxCountPay = schOrgGenreModel.MaxCountPay;
                remoteData.MaxTimePay = schOrgGenreModel.MaxTimePay;
                remoteData.UseDate = schOrgGenreModel.UseDate;

                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.UpdateSchOrgGenre(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schOrgGenreModel.SchOrgGenreDataSet = remoteData.SchOrgGenreDataSet.Copy();
                schOrgGenreModel.ResultCnt = remoteData.ResultCnt;
                schOrgGenreModel.ResultCD = remoteData.ResultCD;

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
        /// <param name="schOrgGenreModel"></param>
        public void DeleteSchOrgGenre(SchOrgGenreModel schOrgGenreModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchOrgGenreServiceProxy.SchOrgGenreService svc = new SchOrgGenreServiceProxy.SchOrgGenreService();
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchOrgGenreServiceProxy.HeaderModel remoteHeader = new AdManagerClient.SchOrgGenreServiceProxy.HeaderModel();
                SchOrgGenreServiceProxy.SchOrgGenreModel remoteData = new AdManagerClient.SchOrgGenreServiceProxy.SchOrgGenreModel();

                remoteHeader.UserID = Header.UserID;
                remoteData.CategoryCode = schOrgGenreModel.CategoryCode;
                remoteData.MenuCode = schOrgGenreModel.MenuCode;
                remoteData.MaxCount = schOrgGenreModel.MaxCount;
                remoteData.MaxTime = schOrgGenreModel.MaxTime;
                remoteData.MaxCountPay = schOrgGenreModel.MaxCountPay;
                remoteData.MaxTimePay = schOrgGenreModel.MaxTimePay;
                remoteData.UseDate = schOrgGenreModel.UseDate;

                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.DeleteSchOrgGenre(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schOrgGenreModel.SchOrgGenreDataSet = remoteData.SchOrgGenreDataSet.Copy();
                schOrgGenreModel.ResultCnt = remoteData.ResultCnt;
                schOrgGenreModel.ResultCD = remoteData.ResultCD;

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



	}
 

}