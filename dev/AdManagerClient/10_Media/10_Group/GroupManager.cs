// ===============================================================================
// GroupUpdate Manager  for Charites Project
//
// GroupUpdateManager.cs
//
// ��������� ���� ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: GroupManager
 * �ֿ���  : �׷��������� ���� ȣ��
 * �ۼ���    : ��
 * �ۼ���    : ��
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : �躸��
 * ������    : 2013.02
 * ��������  :        
 *            - �˻���� ��ȸ ����
 * �����Լ�  :
 *            - SearchGroupList �˻��� 
 * --------------------------------------------------------
 */



/*
 * -------------------------------------------------------
 * Class Name: GroupManager
 * �ֿ���  : �׷��������� ���� ȣ��
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : �̿���
 * ������    : 2015.05.26
 * ��������  :        
 *            - ��ȿó���� �޴��� �޴�����Ʈ�� �����Ұ����� ���� �߰�
 *            InvalidYn
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
	/// ��������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class GroupManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public GroupManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/GroupService.asmx";
		}

		/// <summary>
		/// �����������ȸ
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetGroupList(GroupModel groupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// ȣ������ ��Ʈ
                //remoteData.SearchKey       = groupModel.SearchKey; // [E_01]
				remoteData.SearchMedia       = groupModel.SearchMedia;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGroupList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.GroupDataSet = remoteData.GroupDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�׷�����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGroupList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �׷����ȸ
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetGroupDetailList(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�׷����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchGroup       = groupModel.SearchGroup;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGroupDetailList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.GroupDetailDataSet = remoteData.GroupDetailDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�׷����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGroupDetailList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ī�װ���ȸ
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetCategoryList(GroupModel groupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ���ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

				// ȣ������ ��Ʈ
                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.SearchType = groupModel.SearchType;      //�˻� Ÿ��
                remoteData.InvalidYn = groupModel.InvalidYn;        //��ȿ�޴� ��� ���� ����

				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetCategoryList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ���ȸ End");
				_log.Debug("-----------------------------------------");
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
        /// ī�װ���ȸ
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetCategoryList2(GroupModel groupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ī�װ���ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                GroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
                GroupServicePloxy.GroupModel remoteData = new AdManagerClient.GroupServicePloxy.GroupModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.SearchType = groupModel.SearchType;      //�˻� Ÿ��
                remoteData.InvalidYn = groupModel.InvalidYn;        //��ȿ�޴� ��� ���� ����


                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetCategoryList2(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                groupModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
                groupModel.ResultCnt = remoteData.ResultCnt;
                groupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("ī�װ���ȸ End");
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
		/// �帣��ȸ
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetGenreList(GroupModel groupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�帣��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteHeader.UserLevel  = Header.UserLevel;
				remoteHeader.UserClass	= Header.UserClass;

				// ȣ������ ��Ʈ
                remoteData.SearchType = groupModel.SearchType;
                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.CategoryCode = groupModel.CategoryCode;
                remoteData.InvalidYn = groupModel.InvalidYn;        //��ȿ�޴� ��� ���� ����
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGenreList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.GenreDataSet = remoteData.GenreDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�帣��ȸ End");
				_log.Debug("-----------------------------------------");
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
        /// �帣��ȸ
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetGenreList2(GroupModel groupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣��ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                GroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
                GroupServicePloxy.GroupModel remoteData = new AdManagerClient.GroupServicePloxy.GroupModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchType = groupModel.SearchType;
                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.CategoryCode = groupModel.CategoryCode;
                remoteData.InvalidYn = groupModel.InvalidYn;        //��ȿ�޴� ��� ���� ����

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetGenreList2(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                groupModel.GenreDataSet = remoteData.GenreDataSet.Copy();
                groupModel.ResultCnt = remoteData.ResultCnt;
                groupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�帣��ȸ End");
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
        /// �׷�������� ��ȸ
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetGroupMapList(GroupModel groupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(" �׷�������� ��ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
                svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
                GroupServicePloxy.GroupModel    remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchKey = "";
                remoteData.CategoryCode = "";
                remoteData.InvalidYn = groupModel.InvalidYn;        //��ȿ�޴� ��� ���� ����
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetGroupMapList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                groupModel.GroupMapDataSet = remoteData.GroupMapDataSet.Copy();
                groupModel.ResultCnt   = remoteData.ResultCnt;
                groupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�׷�������� End");
                _log.Debug("-----------------------------------------");
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
		/// ä����ȸ
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetChannelNoPopList(GroupModel groupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// ȣ������ ��Ʈ

				remoteData.SearchKey       = groupModel.SearchKey;				
				remoteData.MediaCode       = groupModel.MediaCode;
				remoteData.CategoryCode    = groupModel.CategoryCode;
				remoteData.GenreCode       = groupModel.GenreCode;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetChannelNoPopList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.ChannelDataSet = remoteData.ChannelDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä����ȸ End");
				_log.Debug("-----------------------------------------");
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
        /// ä����ȸ
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetChannelNoPopList2(GroupModel groupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ä����ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                GroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
                GroupServicePloxy.GroupModel remoteData = new AdManagerClient.GroupServicePloxy.GroupModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ

                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.MediaCode = groupModel.MediaCode;
                remoteData.CategoryCode = groupModel.CategoryCode;
                remoteData.GenreCode = groupModel.GenreCode;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetChannelNoPopList2(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                groupModel.ChannelDataSet = remoteData.ChannelDataSet.Copy();
                groupModel.ResultCnt = remoteData.ResultCnt;
                groupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("ä����ȸ End");
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
		/// [S1] ȸ���������� �����񽺸� ȣ���մϴ�
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetSeriesList(GroupModel groupModel)
		{
			try
			{
				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel	remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteHeader.UserLevel  = Header.UserLevel;
				remoteHeader.UserClass	= Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey    = groupModel.SearchKey;				
				remoteData.MediaCode    = groupModel.MediaCode;
				remoteData.CategoryCode = groupModel.CategoryCode;
				remoteData.GenreCode	= groupModel.GenreCode;
				remoteData.ChannelNo	= groupModel.ChannelNo;
                remoteData.InvalidYn = groupModel.InvalidYn;        //��ȿ�޴� ��� ���� ����
				
				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.GetSeriesList(remoteHeader, remoteData);

				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.SeriesDataSet = remoteData.SeriesDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSeriesList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// [S1] ȸ���������� �����񽺸� ȣ���մϴ�
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetSeriesList2(GroupModel groupModel)
        {
            try
            {
                // ������ �ν��Ͻ� ����
                GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                GroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
                GroupServicePloxy.GroupModel remoteData = new AdManagerClient.GroupServicePloxy.GroupModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.MediaCode = groupModel.MediaCode;
                remoteData.CategoryCode = groupModel.CategoryCode;
                remoteData.GenreCode = groupModel.GenreCode;
                remoteData.ChannelNo = groupModel.ChannelNo;
                remoteData.InvalidYn = groupModel.InvalidYn;        //��ȿ�޴� ��� ���� ����

                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.GetSeriesList2(remoteHeader, remoteData);

                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                groupModel.SeriesDataSet = remoteData.SeriesDataSet.Copy();
                groupModel.ResultCnt = remoteData.ResultCnt;
                groupModel.ResultCD = remoteData.ResultCD;
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSeriesList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Service ȣ���� ���� �޼ҵ�
		/// </summary>
		public bool GetGroupDetail(BaseModel baseModel)
		{
			
			_log.Debug("-----------------------------------------");
			_log.Debug( this.ToString() + " Start");
			_log.Debug("-----------------------------------------");

			_log.Debug("-----------------------------------------");
			_log.Debug( this.ToString() + " End");
			_log.Debug("-----------------------------------------");

			return true;
		}



		/// <summary>
		/// ��������� ����
		/// </summary>
		/// <param name="groupModel"></param>
		public void SetGroupUpdate(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�			
				if(groupModel.GroupName.Length > 1000) 
				{
					throw new FrameException("�׷���� ���̴� 1000Bytes�� �ʰ��� �� �����ϴ�.");
				}						
				if(groupModel.Comment.Length > 1000) 
				{
					throw new FrameException("������ ���̴� 1000Bytes�� �ʰ��� �� �����ϴ�.");
				}

	

				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = groupModel.MediaCode;
				remoteData.GroupCode       = groupModel.GroupCode;
				remoteData.GroupName     = groupModel.GroupName;		
				remoteData.Comment  = groupModel.Comment;		
				remoteData.UseYn     = groupModel.UseYn;		
		
				

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGroupUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ��������� ����
		/// </summary>
		/// <param name="groupModel"></param>
		public void SetGroupGenreUpdate(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�			
				if(groupModel.GroupName.Length > 1000) 
				{
					throw new FrameException("�׷���� ���̴� 1000Bytes�� �ʰ��� �� �����ϴ�.");
				}						
				if(groupModel.Comment.Length > 1000) 
				{
					throw new FrameException("������ ���̴� 1000Bytes�� �ʰ��� �� �����ϴ�.");
				}

	

				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ				
				remoteData.GroupCode       = groupModel.GroupCode;
				remoteData.CategoryCode     = groupModel.CategoryCode;		
				remoteData.GenreCode     = groupModel.GenreCode;							

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGroupGenreUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ��������� ����
		/// </summary>
		/// <param name="groupModel"></param>
		public void SetGroupChannelUpdate(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�			
				if(groupModel.GroupName.Length > 1000) 
				{
					throw new FrameException("�׷���� ���̴� 1000Bytes�� �ʰ��� �� �����ϴ�.");
				}						
				if(groupModel.Comment.Length > 1000) 
				{
					throw new FrameException("������ ���̴� 1000Bytes�� �ʰ��� �� �����ϴ�.");
				}

	

				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.GroupCode       = groupModel.GroupCode;
				remoteData.CategoryCode     = groupModel.CategoryCode;		
				remoteData.GenreCode     = groupModel.GenreCode;
				remoteData.ChannelNo     = groupModel.ChannelNo;			

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGroupChannelUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �׷쿡 ȸ���� �߰��մϴ�
		/// </summary>
		/// <param name="groupModel"></param>
		public void SetGroupSerieslUpdate(GroupModel groupModel)
		{
			try
			{
				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ��������Ʈ
				remoteData.GroupCode    = groupModel.GroupCode;
				remoteData.CategoryCode	= groupModel.CategoryCode;		
				remoteData.GenreCode    = groupModel.GenreCode;
				remoteData.ChannelNo    = groupModel.ChannelNo;			
				remoteData.SeriesNo		= groupModel.SeriesNo;

				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.SetGroupSeriesUpdate(remoteHeader, remoteData);

				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupSerieslUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ������߰�
		/// </summary>
		/// <param name="groupModel"></param>
		/// <returns></returns>
		public void SetGroupAdd(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� Start");
				_log.Debug("-----------------------------------------");
				
				if(groupModel.GroupName.Length > 1000) 
				{
					throw new FrameException("�׷���� ���̴� 1000Bytes�� �ʰ��� �� �����ϴ�.");
				}						
				if(groupModel.Comment.Length > 1000) 
				{
					throw new FrameException("������ ���̴� 1000Bytes�� �ʰ��� �� �����ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ				
				remoteData.MediaCode       = groupModel.MediaCode;
				remoteData.GroupName     = groupModel.GroupName;					
				remoteData.Comment  = groupModel.Comment;		
				remoteData.UseYn     = groupModel.UseYn;				
					
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGroupCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ������߰�
		/// </summary>
		/// <param name="groupModel"></param>
		/// <returns></returns>
		public void SetGroupDetailAdd(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� Start");
				_log.Debug("-----------------------------------------");
				
				if(groupModel.GroupName.Length > 1000) 
				{
					throw new FrameException("�׷���� ���̴� 1000Bytes�� �ʰ��� �� �����ϴ�.");
				}						
				if(groupModel.Comment.Length > 1000) 
				{
					throw new FrameException("������ ���̴� 1000Bytes�� �ʰ��� �� �����ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ				
				remoteData.GroupCode     = groupModel.GroupCode;					
				remoteData.CategoryCode  = groupModel.CategoryCode;		
									
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGroupDetailCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetGroupDelete(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�׷���� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				// ȣ��������Ʈ
				remoteData.MediaCode       = groupModel.MediaCode;
				remoteData.GroupCode       = groupModel.GroupCode;
					
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGroupDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�׷���� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetGroupDetailDelete(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���׷� �������� Start");
				_log.Debug("-----------------------------------------");

				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;
				
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();


				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteData.GroupCode    = groupModel.GroupCode;
				remoteData.CategoryCode	= groupModel.CategoryCode;
				remoteData.GenreCode    = groupModel.GenreCode;
				remoteData.ChannelNo    = groupModel.ChannelNo;
				remoteData.SeriesNo		= groupModel.SeriesNo;
					

				remoteData = svc.SetGroupDetailDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���׷� �������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
