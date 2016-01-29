// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// ä������ ���� ���񽺸� ȣ���մϴ�. 
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
	/// ä������ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class SchMidAdManager : BaseManager
	{

		// �������汸��
		const int ORDER_FIRST = 1;
		const int ORDER_LAST  = 2;
		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;


		/// <summary>
		/// 0.������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SchMidAdManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Schedule/SchMidAdService.asmx";
		}


		/// <summary>
		/// 1.�޴� �����ȸ
		/// </summary>
		/// <param name="chooseAdScheduleModel"></param>
		public void GetMenuList(ChooseAdScheduleModel chooseAdScheduleModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
                SchMidAdServiceProxy.SchMidAdService    svc = new AdManagerClient.SchMidAdServiceProxy.SchMidAdService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SchMidAdServiceProxy.HeaderModel            remoteHeader = new AdManagerClient.SchMidAdServiceProxy.HeaderModel();
				SchMidAdServiceProxy.ChooseAdScheduleModel  remoteData   = new AdManagerClient.SchMidAdServiceProxy.ChooseAdScheduleModel();

				// ������� ��Ʈ
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode = chooseAdScheduleModel.SearchMediaCode;			
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetMenuList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				chooseAdScheduleModel.ChooseAdScheduleDataSet = remoteData.ChooseAdScheduleDataSet.Copy();
				chooseAdScheduleModel.ResultCnt   = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD    = remoteData.ResultCD;

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
		/// 2.ä��������ȸ
		/// </summary>
		/// <param name="chooseAdScheduleModel"></param>
		public void GetChannelList(ChooseAdScheduleModel chooseAdScheduleModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�θ����ȸ Start");
				_log.Debug("-----------------------------------------");
				// ������ �ν��Ͻ� ����
                SchMidAdServiceProxy.SchMidAdService    svc = new AdManagerClient.SchMidAdServiceProxy.SchMidAdService();
                svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                SchMidAdServiceProxy.HeaderModel            remoteHeader = new AdManagerClient.SchMidAdServiceProxy.HeaderModel();
                SchMidAdServiceProxy.ChooseAdScheduleModel  remoteData   = new AdManagerClient.SchMidAdServiceProxy.ChooseAdScheduleModel();
                
				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.MediaCode       = chooseAdScheduleModel.MediaCode;
				remoteData.CategoryCode    = chooseAdScheduleModel.CategoryCode;
				remoteData.GenreCode       = chooseAdScheduleModel.GenreCode;
							
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetChannelList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				chooseAdScheduleModel.ChooseAdScheduleDataSet = remoteData.ChooseAdScheduleDataSet.Copy();
				chooseAdScheduleModel.ResultCnt   = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�θ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 3.ä�� ����Ȳ��ȸ
		/// </summary>
		/// <param name="chooseAdScheduleModel"></param>
		public void GetScheduleListChannel(ChooseAdScheduleModel chooseAdScheduleModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä������Ȳ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
                SchMidAdServiceProxy.SchMidAdService    svc = new AdManagerClient.SchMidAdServiceProxy.SchMidAdService();
                svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                SchMidAdServiceProxy.HeaderModel            remoteHeader = new AdManagerClient.SchMidAdServiceProxy.HeaderModel();
                SchMidAdServiceProxy.ChooseAdScheduleModel  remoteData   = new AdManagerClient.SchMidAdServiceProxy.ChooseAdScheduleModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.MediaCode        =  chooseAdScheduleModel.MediaCode;       
				remoteData.GenreCode		=  chooseAdScheduleModel.GenreCode;       
				remoteData.ChannelNo		=  chooseAdScheduleModel.ChannelNo;       
                remoteData.SearchchkAdState_10  = chooseAdScheduleModel.SearchchkAdState_10;
                remoteData.SearchchkAdState_20  = chooseAdScheduleModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30  = chooseAdScheduleModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40  = chooseAdScheduleModel.SearchchkAdState_40;
                

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetScheduleListChannel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				chooseAdScheduleModel.ChooseAdScheduleDataSet = remoteData.ChooseAdScheduleDataSet.Copy();
				chooseAdScheduleModel.LastOrder   = remoteData.LastOrder;				
				chooseAdScheduleModel.ResultCnt   = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä������Ȳ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChooseAdScheduleList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 4. ä�� �ø��� ��Ȳ��ȸ(�߰���������)
        /// </summary>
        /// <param name="chooseAdScheduleModel"></param>
        public void GetMidAdInfoListSeries(ChooseAdScheduleModel chooseAdScheduleModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�ø�� �߰��������� ��ȸ Start");
                _log.Debug("-----------------------------------------");
				
                SchMidAdServiceProxy.SchMidAdService    svc = new AdManagerClient.SchMidAdServiceProxy.SchMidAdService();
                svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                SchMidAdServiceProxy.HeaderModel            remoteHeader = new AdManagerClient.SchMidAdServiceProxy.HeaderModel();
                SchMidAdServiceProxy.ChooseAdScheduleModel  remoteData   = new AdManagerClient.SchMidAdServiceProxy.ChooseAdScheduleModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;
                remoteHeader.UserLevel  = Header.UserLevel;
                remoteHeader.UserClass  = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.MediaCode    =  chooseAdScheduleModel.MediaCode;       
                remoteData.CategoryCode =  chooseAdScheduleModel.CategoryCode;
                remoteData.GenreCode	=  chooseAdScheduleModel.GenreCode;       
                remoteData.ChannelNo	=  chooseAdScheduleModel.ChannelNo;       
                

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetMidAdInfoListSeries(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                chooseAdScheduleModel.ChooseAdScheduleDataSet = remoteData.ChooseAdScheduleDataSet.Copy();
                chooseAdScheduleModel.ResultCnt   = remoteData.ResultCnt;
                chooseAdScheduleModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("ä������Ȳ��ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChooseAdScheduleList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����ä�α����� ���� ����
		/// </summary>
		/// <param name="schHomeAdModel"></param>
		/// <returns></returns>
		public void SetSchChannelAdOrderSet(ChooseAdScheduleModel chooseAdScheduleModel, int OrderSet)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ä�α����� ���� ���� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(chooseAdScheduleModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				ChooseAdScheduleServiceProxy.ChooseAdScheduleService svc = new ChooseAdScheduleServiceProxy.ChooseAdScheduleService();

				// ������ URL���� ����
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ChooseAdScheduleServiceProxy.HeaderModel   remoteHeader = new AdManagerClient.ChooseAdScheduleServiceProxy.HeaderModel();
				ChooseAdScheduleServiceProxy.ChooseAdScheduleModel remoteData   = new AdManagerClient.ChooseAdScheduleServiceProxy.ChooseAdScheduleModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ

				remoteData.MediaCode       =  chooseAdScheduleModel.MediaCode;
				remoteData.ChannelNo       =  chooseAdScheduleModel.ChannelNo;
				remoteData.ItemNo          =  chooseAdScheduleModel.ItemNo;
				remoteData.ItemName        =  chooseAdScheduleModel.ItemName;
				remoteData.ScheduleOrder   =  chooseAdScheduleModel.ScheduleOrder;
 
				switch(OrderSet)
				{
					case ORDER_FIRST:
                        // ������ ȣ�� Ÿ�Ӿƿ�����
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// ������ �޼ҵ� ȣ��
						remoteData = svc.SetSchChannelAdOrderFirst(remoteHeader, remoteData);
						
						break;
					case ORDER_UP:
                        // ������ ȣ�� Ÿ�Ӿƿ�����
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// ������ �޼ҵ� ȣ��
						remoteData = svc.SetSchChannelAdOrderUp(remoteHeader, remoteData);						
						break;
					case ORDER_DOWN:
                        // ������ ȣ�� Ÿ�Ӿƿ�����
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// ������ �޼ҵ� ȣ��
						remoteData = svc.SetSchChannelAdOrderDown(remoteHeader, remoteData);						
						break;
					case ORDER_LAST:
                        // ������ ȣ�� Ÿ�Ӿƿ�����
                        svc.Timeout = FrameSystem.m_SystemTimeout;
						// ������ �޼ҵ� ȣ��
						remoteData = svc.SetSchChannelAdOrderLast(remoteHeader, remoteData);						
						break;
					default:
						throw new FrameException("�������� ������ ���õ��� �ʾҽ��ϴ�.");
				}


				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				chooseAdScheduleModel.ResultCnt     = remoteData.ResultCnt;
				chooseAdScheduleModel.ResultCD      = remoteData.ResultCD;		
				chooseAdScheduleModel.ScheduleOrder = remoteData.ScheduleOrder;
								
				_log.Debug("-----------------------------------------");
				_log.Debug("����ä�α����� ���� ���� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchMenuAdOrderSet():" + fe.ErrCode + ":" + fe.ResultMsg);
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