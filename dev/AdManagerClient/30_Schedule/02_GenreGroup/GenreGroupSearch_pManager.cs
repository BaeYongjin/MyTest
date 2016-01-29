// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// �帣���� ���� ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
// 2007.06.26 �۸�ȯ v1.0
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
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
    /// �帣���� �����񽺸� ȣ���մϴ�. 
    /// </summary>
    public class GenreGroupSearch_pManager : BaseManager
    {
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public GenreGroupSearch_pManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Schedule/GenreGroupService.asmx";
        }

		/// <summary>
		/// �帣������ȸ(����)
		/// </summary>
		/// <param name="genreGroupModel"></param>
		public void GetInspectGenreGroupList_p(GenreGroupModel genreGroupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�帣�����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
				svc.Url = _WebServiceUrl;			
				// ����Ʈ �� ����
				GenreGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
				GenreGroupServicePloxy.GenreGroupModel remoteData   = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = genreGroupModel.SearchKey;
				remoteData.MediaCode       = genreGroupModel.MediaCode;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetInspectGenreGroupList_p(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				genreGroupModel.GenreGroupDataSet = remoteData.GenreGroupDataSet.Copy();
				genreGroupModel.ResultCnt   = remoteData.ResultCnt;
				genreGroupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�帣�����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreGroupListPopUp():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �帣������ȸ
        /// </summary>
        /// <param name="genreGroupModel"></param>
        public void GetGenreGroupList_p(GenreGroupModel genreGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�����ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
                svc.Url = _WebServiceUrl;			
                // ����Ʈ �� ����
                GenreGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
                GenreGroupServicePloxy.GenreGroupModel remoteData   = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = genreGroupModel.SearchKey;
                remoteData.MediaCode       = genreGroupModel.MediaCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
				remoteData = svc.GetGenreGroupList_p(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
				genreGroupModel.GenreGroup_pDataSet	= remoteData.GenreGroup_pDataSet.Copy();
                genreGroupModel.GenreGroupDataSet	= remoteData.GenreGroupDataSet.Copy();
                genreGroupModel.ResultCnt   = remoteData.ResultCnt;
                genreGroupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetGenreGroupListPopUp():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �ǽð�ä�� ���� ��ȸ
		/// </summary>
		/// <param name="genreGroupModel"></param>
		public void GetChannelList_p(GenreGroupModel genreGroupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�θ����ȸ Start");
				_log.Debug("-----------------------------------------");

				GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
				
				// ����Ʈ �� ����
				GenreGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
				GenreGroupServicePloxy.GenreGroupModel remoteData = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey = Header.ClientKey;
				remoteHeader.UserID = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey = genreGroupModel.SearchKey;
				remoteData.MediaCode = genreGroupModel.MediaCode;

				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetChannelList_p(remoteHeader, remoteData);

				// ����ڵ�˻�
				if (!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				genreGroupModel.GenreGroup_pDataSet = remoteData.GenreGroup_pDataSet.Copy();
				genreGroupModel.GenreGroupDataSet = remoteData.GenreGroupDataSet.Copy();
				genreGroupModel.ResultCnt = remoteData.ResultCnt;
				genreGroupModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�θ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch (FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning(this.ToString() + ":GetChannelList_p():" + fe.ErrCode + ":" + fe.ResultMsg);
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