// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// ���������� ���� ���񽺸� ȣ���մϴ�. 
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
    /// ���������� �����񽺸� ȣ���մϴ�. 
    /// </summary>
    public class ContentsManager : BaseManager
    {
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ContentsManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/ContentsService.asmx";
        }

        /// <summary>
        /// ������������ȸ(����)
        /// </summary>
        /// <param name="contentsModel"></param>
        public void GetContentsListCommon(ContentsModel contentsModel)
        {
            try
            {
                // ������ �ν��Ͻ� ����
                ContentsServicePloxy.ContentsService svc = new ContentsServicePloxy.ContentsService();
                svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                ContentsServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContentsServicePloxy.HeaderModel();
                ContentsServicePloxy.ContentsModel remoteData   = new AdManagerClient.ContentsServicePloxy.ContentsModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = contentsModel.SearchKey;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetContentsListCommon(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contentsModel.ContentsDataSet = remoteData.ContentsDataSet.Copy();
                contentsModel.ResultCnt   = remoteData.ResultCnt;
                contentsModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����������ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetContentsList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ������������ȸ
        /// </summary>
        /// <param name="contentsModel"></param>
        public void GetContentsList(ContentsModel contentsModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�����������ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ContentsServicePloxy.ContentsService svc = new ContentsServicePloxy.ContentsService();
				svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                ContentsServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContentsServicePloxy.HeaderModel();
                ContentsServicePloxy.ContentsModel remoteData   = new AdManagerClient.ContentsServicePloxy.ContentsModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = contentsModel.SearchKey;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetContentsList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contentsModel.ContentsDataSet = remoteData.ContentsDataSet.Copy();
                contentsModel.ResultCnt   = remoteData.ResultCnt;
                contentsModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����������ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetContentsList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// Service ȣ���� ���� �޼ҵ�
        /// </summary>
        public bool GetContentsDetail(BaseModel baseModel)
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
        /// �������߰�
        /// </summary>
        /// <param name="contentsModel"></param>
        /// <returns></returns>
        public void SetContentsAdd(ContentsModel contentsModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�������߰� Start");
                _log.Debug("-----------------------------------------");

                if(contentsModel.Title.Trim().Length < 1) 
                {
                    throw new FrameException("������ �������� �ʽ��ϴ�.");
                }
                if(contentsModel.Title.Trim().Length > 120) 
                {
                    throw new FrameException("������ 120Byte�� �ʰ��� �� �����ϴ�.");
                }
                if(contentsModel.Rate.Length >1 &&  Convert.ToInt32(contentsModel.Rate) > 255) 
                {
                    throw new FrameException("��ް��� 255�� �ʰ��� �� �����ϴ�.");
                }
                if(contentsModel.ContentsState.Length >1 &&  Convert.ToInt32(contentsModel.ContentsState) > 255) 
                {
                    throw new FrameException("���°��� 255�� �ʰ��� �� �����ϴ�.");
                }
                if(contentsModel.SubTitle.Trim().Length > 40) 
                {
                    throw new FrameException("Sub������ 40Byte�� �ʰ��� �� �����ϴ�.");
                }
                if(contentsModel.OrgTitle.Trim().Length > 40) 
                {
                    throw new FrameException("���������帣�� 40Byte�� �ʰ��� �� �����ϴ�.");
                }


                // ������ �ν��Ͻ� ����
                ContentsServicePloxy.ContentsService svc = new ContentsServicePloxy.ContentsService();
				svc.Url = _WebServiceUrl;
				
                // ����Ʈ �� ����
                ContentsServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContentsServicePloxy.HeaderModel();
                ContentsServicePloxy.ContentsModel remoteData   = new AdManagerClient.ContentsServicePloxy.ContentsModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ��������Ʈ
                remoteData.Title      = contentsModel.Title;
                remoteData.ContentsState    = contentsModel.ContentsState;
                remoteData.Rate     = contentsModel.Rate;
                remoteData.SubTitle     = contentsModel.SubTitle;
                remoteData.OrgTitle     = contentsModel.OrgTitle;
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetContentsCreate(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contentsModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("contentsModel.ResultCnt = "+contentsModel.ResultCnt);
			
                contentsModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�������߰� End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                contentsModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetContentsCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                contentsModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// ���������� ����
        /// </summary>
        /// <param name="contentsModel"></param>
        public void SetContentsUpdate(ContentsModel contentsModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�������������� Start");
                _log.Debug("-----------------------------------------");


                //				�Էµ������� Validation �˻�
                if(contentsModel.Title.Trim().Length < 1) 
                {
                    throw new FrameException("������ �������� �ʽ��ϴ�.");
                }
                if(contentsModel.Title.Trim().Length > 120) 
                {
                    throw new FrameException("������ 120Byte�� �ʰ��� �� �����ϴ�.");
                }
                if(contentsModel.Rate.Length >1 &&  Convert.ToInt32(contentsModel.Rate) > 255) 
                {
                    throw new FrameException("��ް��� 255�̻��� �ʰ��� �� �����ϴ�.");
                }
                if(contentsModel.ContentsState.Length >1 &&  Convert.ToInt32(contentsModel.ContentsState) > 255) 
                {
                    throw new FrameException("���°��� 255�̻��� �ʰ��� �� �����ϴ�.");
                }
                if(contentsModel.SubTitle.Trim().Length > 40) 
                {
                    throw new FrameException("Sub������ 40Byte�� �ʰ��� �� �����ϴ�.");
                }
                if(contentsModel.OrgTitle.Trim().Length > 40) 
                {
                    throw new FrameException("���������帣�� 40Byte�� �ʰ��� �� �����ϴ�.");
                }

                // ������ �ν��Ͻ� ����
                ContentsServicePloxy.ContentsService svc = new ContentsServicePloxy.ContentsService();
				svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                ContentsServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContentsServicePloxy.HeaderModel();
                ContentsServicePloxy.ContentsModel remoteData   = new AdManagerClient.ContentsServicePloxy.ContentsModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ��������Ʈ
                remoteData.ContentKey      = contentsModel.ContentKey;
                remoteData.Title           = contentsModel.Title;
                remoteData.ContentsState   = contentsModel.ContentsState;
                remoteData.Rate            = contentsModel.Rate;
                remoteData.SubTitle        = contentsModel.SubTitle;
                remoteData.OrgTitle        = contentsModel.OrgTitle;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetContentsUpdate(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contentsModel.ResultCnt   = remoteData.ResultCnt;
                contentsModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�������������� End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                contentsModel.ResultCD   = "3201";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetContentsUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                contentsModel.ResultCD   = "3201";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

		
        /// <summary>
        /// ������ ����
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetContentsDelete(ContentsModel contentsModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���������� start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                ContentsServicePloxy.ContentsService svc = new ContentsServicePloxy.ContentsService();
				svc.Url = _WebServiceUrl;
				
                // ����Ʈ �� ����
                ContentsServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContentsServicePloxy.HeaderModel();
                ContentsServicePloxy.ContentsModel remoteData   = new AdManagerClient.ContentsServicePloxy.ContentsModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ��������Ʈ
                remoteData.ContentKey       = contentsModel.ContentKey;
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetContentsDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contentsModel.ResultCnt   = remoteData.ResultCnt;
                contentsModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("���������� end");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                contentsModel.ResultCD   = "3301";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":setcontentsdelete():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                contentsModel.ResultCD   = "3301";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

    }
}
