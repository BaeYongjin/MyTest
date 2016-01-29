// ===============================================================================
// SchChoiceAd Manager  for Charites Project
//
// SchChoiceAdManager.cs
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
 *            - ������ ��� �߰�
 * --------------------------------------------------------
 */
using System;
using System.Data;

using AdManagerModel;
using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

namespace AdManagerClient
{
    /// <summary>
    /// �������������� �����񽺸� ȣ���մϴ�. 
    /// </summary>
    public class SchChoiceAdManager : BaseManager
    {
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public SchChoiceAdManager(SystemModel systemModel, CommonModel commonModel)
            : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "SchChoiceAd";
            _Host = FrameSystem.m_WebServer_Host;
            _Port = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/Schedule/SchChoiceAdService.asmx";
        }

        /// <summary>
        /// ��������ȸ
        /// �������������� ȣ���Ѵ�
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetAdList10(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchKey = schChoiceAdModel.SearchKey;
                remoteData.SearchMediaCode = schChoiceAdModel.SearchMediaCode;
                remoteData.SearchRapCode = schChoiceAdModel.SearchRapCode;
                remoteData.SearchAgencyCode = schChoiceAdModel.SearchAgencyCode;
                remoteData.SearchAdvertiserCode = schChoiceAdModel.SearchAdvertiserCode;
                remoteData.SearchContractState = schChoiceAdModel.SearchContractState;
                remoteData.SearchAdClass = schChoiceAdModel.SearchAdClass;
                remoteData.SearchchkAdState_20 = schChoiceAdModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = schChoiceAdModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = schChoiceAdModel.SearchchkAdState_40;
                remoteData.AdType = schChoiceAdModel.AdType;

                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;

                remoteData = svc.mGetAdList10(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchChoiceAdList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ������������Ȳ��ȸ
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetSchChoiceAdList(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("������������Ȳ��ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchKey = schChoiceAdModel.SearchKey;
                remoteData.SearchMediaCode = schChoiceAdModel.SearchMediaCode;
                remoteData.SearchRapCode = schChoiceAdModel.SearchRapCode;
                remoteData.SearchAgencyCode = schChoiceAdModel.SearchAgencyCode;
                remoteData.SearchAdvertiserCode = schChoiceAdModel.SearchAdvertiserCode;
                remoteData.SearchContractState = schChoiceAdModel.SearchContractState;
                remoteData.SearchAdClass = schChoiceAdModel.SearchAdClass;
                remoteData.SearchchkAdState_20 = schChoiceAdModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = schChoiceAdModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = schChoiceAdModel.SearchchkAdState_40;
                remoteData.AdType = schChoiceAdModel.AdType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetSchChoiceAdList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�������ϸ����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchChoiceAdList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ���������� ����-���ϸ�� ������ġ
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetInspectItemList(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���������������ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchKey = schChoiceAdModel.SearchKey;
                remoteData.SearchMediaCode = schChoiceAdModel.SearchMediaCode;
                remoteData.SearchRapCode = schChoiceAdModel.SearchRapCode;
                remoteData.SearchAgencyCode = schChoiceAdModel.SearchAgencyCode;
                remoteData.SearchAdvertiserCode = schChoiceAdModel.SearchAdvertiserCode;
                remoteData.SearchchkAdState_10 = schChoiceAdModel.SearchchkAdState_10;
                remoteData.SearchchkAdState_20 = schChoiceAdModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = schChoiceAdModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = schChoiceAdModel.SearchchkAdState_40;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetInspectItemList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�������ϸ����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetInspectItemList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ���������������ȸ
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetContractItemList(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���������������ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchKey = schChoiceAdModel.SearchKey;
                remoteData.SearchMediaCode = schChoiceAdModel.SearchMediaCode;
                remoteData.SearchRapCode = schChoiceAdModel.SearchRapCode;
                remoteData.SearchAgencyCode = schChoiceAdModel.SearchAgencyCode;
                remoteData.SearchAdvertiserCode = schChoiceAdModel.SearchAdvertiserCode;
                remoteData.SearchchkAdState_10 = schChoiceAdModel.SearchchkAdState_10;
                remoteData.SearchchkAdState_20 = schChoiceAdModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = schChoiceAdModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = schChoiceAdModel.SearchchkAdState_40;
                remoteData.SearchAdType = schChoiceAdModel.SearchAdType;
                remoteData.AdType = schChoiceAdModel.AdType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetContractItemList_0907a(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�������ϸ����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetContractItemList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �������� ���߰�
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceAdAdd(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�������� ���߰� Start");
                _log.Debug("-----------------------------------------");

                //�Էµ������� Validation �˻�
                if (schChoiceAdModel.ItemNo.Length < 1)
                {
                    throw new FrameException("���� ���õ��� �ʾҽ��ϴ�.");
                }

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ

                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.ItemName = schChoiceAdModel.ItemName;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                // �����޴����� ����
                remoteData = svc.SetSchChoiceMenuCreate(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ������ ȣ�� Ÿ�Ӿƿ�����
                //svc.Timeout = FrameSystem.m_SystemTimeout;
                //remoteData = svc.SetSchChoiceChannelCreate(remoteHeader, remoteData);

                //// ����ڵ�˻�
                //if(!remoteData.ResultCD.Equals("0000"))
                //{
                //    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                //}

                // ������ ȣ�� Ÿ�Ӿƿ�����
                //svc.Timeout = FrameSystem.m_SystemTimeout;
                //remoteData = svc.GetSchChoiceLastItemNo(remoteHeader, remoteData);

                //// ����ڵ�˻�
                //if(!remoteData.ResultCD.Equals("0000"))
                //{
                //    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                //}

                // ��� ��Ʈ
                schChoiceAdModel.ItemNo = remoteData.ItemNo;
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�������� ���߰� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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

        #region ������ [E_01]
        /// <summary>
        /// �������� ������ 
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceAdCopy(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�������� �� ���� Start");
                _log.Debug("-----------------------------------------");

                //�Էµ������� Validation �˻�
                if (schChoiceAdModel.ItemNo.Length < 1)
                {
                    throw new FrameException("���� ���õ��� �ʾҽ��ϴ�.");
                }

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ

                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.ItemNoCopy = schChoiceAdModel.ItemNoCopy;
                remoteData.AdType = schChoiceAdModel.AdType;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.CheckSchResult = schChoiceAdModel.CheckSchResult;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetSchChoiceAdCopy(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                //schChoiceAdModel.ItemNo = remoteData.ItemNo;
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�������� �� ���� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceAdCopy():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        #endregion

        #region ������ ��ȸ
        /// <summary>
        /// ���� ���� ���� ������ ��ȸ
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void CheckSchChoice(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� �� ��ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.CheckSchChoice(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;
                schChoiceAdModel.CheckMenu = remoteData.CheckMenu;
                schChoiceAdModel.CheckChannel = remoteData.CheckChannel;
                schChoiceAdModel.CheckSeries = remoteData.CheckSeries;
                schChoiceAdModel.CheckDetail = remoteData.CheckDetail;

                _log.Debug("-----------------------------------------");
                _log.Debug("���� ���� �� ��ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":CheckSchChoice():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        #endregion


        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceAdDelete(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�������� ������ Start");
                _log.Debug("-----------------------------------------");

                //�Էµ������� Validation �˻�
                if (schChoiceAdModel.ItemNo.Length < 1)
                {
                    throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
                }

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.ItemName = schChoiceAdModel.ItemName;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                // �����޴����� ����
                remoteData = svc.SetSchChoiceMenuDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                //// ȣ������ ��Ʈ
                //remoteData.MediaCode       =  schChoiceAdModel.MediaCode;
                //remoteData.ItemNo          =  schChoiceAdModel.ItemNo;
                //remoteData.ItemName        =  schChoiceAdModel.ItemName;

                //// ������ ȣ�� Ÿ�Ӿƿ�����
                //svc.Timeout = FrameSystem.m_SystemTimeout;

                //// ����ä�α��� ����
                //remoteData = svc.SetSchChoiceChannelDelete(remoteHeader, remoteData);

                //// ����ڵ�˻�
                //if(!remoteData.ResultCD.Equals("0000"))
                //{
                //    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                //}

                //// ������ ȣ�� Ÿ�Ӿƿ�����
                //svc.Timeout = FrameSystem.m_SystemTimeout;
                //// ������ ������ ���� ��ȸ
                //remoteData = svc.GetSchChoiceLastItemNoDelete(remoteHeader, remoteData);

                //// ����ڵ�˻�
                //if(!remoteData.ResultCD.Equals("0000"))
                //{
                //    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                //}


                // ��� ��Ʈ
                schChoiceAdModel.ItemNo = remoteData.ItemNo;
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�������� ������ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceAdDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ������������������
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceAdDelete_To(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�������� ���������� Start");
                _log.Debug("-----------------------------------------");

                //�Էµ������� Validation �˻�
                //				if(schChoiceAdModel.ItemNo.Length < 1) 
                //				{
                //					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
                //				}

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ

                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ItemNo = schChoiceAdModel.ItemNo;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                // �����޴����� ����
                remoteData = svc.SetSchChoiceMenuDelete_To(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ ������ ���� ��ȸ
                remoteData = svc.GetSchChoiceLastItemNoDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }


                // ��� ��Ʈ
                schChoiceAdModel.ItemNo = remoteData.ItemNo;
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�������� ���������� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceAdDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �޴����� ��ȸ(���������� ����Ʈ�Ͽ� DB�� ���� �����͸� �μ�Ʈ �ϱ�����...�޴� ���̺� ����Ʈ)
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceList(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ��ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;
                remoteData.ScheduleOrder = schChoiceAdModel.ScheduleOrder;
                remoteData.AdType = schChoiceAdModel.AdType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetSchChoiceSearch(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ��ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ä�α��� ��ȸ(���������� ����Ʈ�Ͽ� DB�� ���� �����͸� �μ�Ʈ �ϱ�����...ä�� ���̺� ����Ʈ)
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceChannelList(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����ä�α��� ���� ��ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ChannelNo = schChoiceAdModel.ChannelNo;
                remoteData.ScheduleOrder = schChoiceAdModel.ScheduleOrder;
                remoteData.AdType = schChoiceAdModel.AdType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetSchChoiceChannelSearch(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����ä�α��� ���� ��ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceChannelList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �����޴����� ���� ��������
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceMenuDetailAdd_To(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� �������� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;
                remoteData.ScheduleOrder = schChoiceAdModel.ScheduleOrder;
                remoteData.AdType = schChoiceAdModel.AdType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetSchChoiceMenuDetailCreate_To(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� �������� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuDetailAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �����޴����� ���� ��ȸ
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetSchChoiceMenuDetailList(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ��ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetSchChoiceMenuDetailList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ��ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchChoiceMenuDetailList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ����ä�α��� ���� ��ȸ
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetSchChoiceChannelDetailList(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����ä�α��� ���� ��ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetSchChoiceChannelDetailList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����ä�α��� ���� ��ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchChoiceMenuDetailList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �����޴����� ���� ����
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceMenuDetailAdd(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;
                remoteData.GenreName = schChoiceAdModel.GenreName;
                remoteData.CategoryCode = schChoiceAdModel.CategoryCode;
                remoteData.AdType = schChoiceAdModel.AdType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetSchChoiceMenuDetailCreate(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ���� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuDetailAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ���α��� ���� ����
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceRealChDetailAdd(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;
                remoteData.GenreName = schChoiceAdModel.GenreName;
                remoteData.CategoryCode = schChoiceAdModel.CategoryCode;
                remoteData.AdType = schChoiceAdModel.AdType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetSchChoiceRealChDetailCreate(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ���� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuDetailAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ����ä�α��� ���� ����
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceChannelDetailAdd(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����ä�α��� ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.ChannelNo = schChoiceAdModel.ChannelNo;
                remoteData.Title = schChoiceAdModel.Title;
                remoteData.AdType = schChoiceAdModel.AdType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetSchChoiceChannelDetailCreate(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����ä�α��� ���� ���� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuDetailAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ȸ���� ����
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceSeriesDetailAdd(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�������� ȸ������ ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.ChannelNo = schChoiceAdModel.ChannelNo;
                remoteData.SeriesNo = schChoiceAdModel.SeriesNo;
                remoteData.Title = schChoiceAdModel.Title;
                remoteData.AdType = schChoiceAdModel.AdType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetSchChoiceSeriesDetailCreate(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����ä�α��� ���� ���� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuDetailAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �����޴����� ���� ����
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceMenuDetailDelete(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;
                remoteData.GenreName = schChoiceAdModel.GenreName;
                remoteData.CategoryCode = schChoiceAdModel.CategoryCode;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetSchChoiceMenuDetailDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ���� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuDetailDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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

        public void SetSchChoiceRealChDetailDelete(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetSchChoiceRealChDetailDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����޴����� ���� ���� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuDetailDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ä�α�������������
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void SetSchChannelAdDelete_To(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ä�α��� ���������� Start");
                _log.Debug("-----------------------------------------");

                //�Էµ������� Validation �˻�
                //				if(schChoiceAdModel.ItemNo.Length < 1) 
                //				{
                //					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
                //				}

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ

                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ItemNo = schChoiceAdModel.ItemNo;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                // �����޴����� ����
                remoteData = svc.SetSchChoiceChannelDelete_To(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ ������ ���� ��ȸ
                remoteData = svc.GetSchChoiceLastItemNoDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }


                // ��� ��Ʈ
                schChoiceAdModel.ItemNo = remoteData.ItemNo;
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("ä�α��� ���������� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceAdDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ����ä�α��� ���� ��������
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceChannelDetailAdd_To(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����ä�α��� ���� �������� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ChannelNo = schChoiceAdModel.ChannelNo;
                remoteData.ScheduleOrder = schChoiceAdModel.ScheduleOrder;
                remoteData.AdType = schChoiceAdModel.AdType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetSchChoiceChannelDetailCreate_To(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����ä�α��� ���� �������� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceChannelDetailAdd_To():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ����ä�α��� ���� ����
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceChannelDetailDelete(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                // ������ �ν��Ͻ� ����
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.ChannelNo = schChoiceAdModel.ChannelNo;
                remoteData.Title = schChoiceAdModel.Title;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetSchChoiceChannelDetailDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceChannelDetailDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
