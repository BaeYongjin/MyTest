// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
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
	public class SlotOrganizationManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SlotOrganizationManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "SLOTORGANIZATION";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/SlotOrganizationService.asmx";
		}

		/// <summary>
		/// �����������ȸ
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		public void GetSlotList(SlotOrganizationModel slotOrganizationModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaName       = slotOrganizationModel.SearchMediaName;
				remoteData.SearchCategoryName    = slotOrganizationModel.SearchCategoryName;
				remoteData.SearchGenreName       = slotOrganizationModel.SearchGenreName;

				remoteData.SearchchkUseYn	 = slotOrganizationModel.SearchchkUseYn; 
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetSlotList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				slotOrganizationModel.SlotDataSet = remoteData.SlotDataSet.Copy();
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSlotList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �����ڵ�
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetSlotCodeList(SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel     remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetSlotCodeList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				slotOrganizationModel.SlotCodeDataSet = remoteData.SlotCodeDataSet.Copy();
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����ڵ���ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				//_log.Warning( this.ToString() + ":GetSlotCodeList():" + fe.ErrMediaCode + ":" + fe.ResultMsg);
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
		/// ī�װ��޺�������ȸ
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		public void GetCategoryList(SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ��޺� �����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.CategoryCode = slotOrganizationModel.MediaCode;
				
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
				slotOrganizationModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ��޺� �����ȸ End");
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
		/// �帣�޺�������ȸ
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		public void GetGenreList(SlotOrganizationModel slotOrganizationModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�帣�޺� �����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// ������� ��Ʈ				
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.SearchKey = slotOrganizationModel.SearchKey;				
				remoteData.SearchMediaName = slotOrganizationModel.SearchMediaName;
				remoteData.SearchCategoryName = slotOrganizationModel.SearchCategoryName;				
				
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
				slotOrganizationModel.GenreDataSet = remoteData.GenreDataSet.Copy();
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�帣�޺� �����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreList():" + fe.ErrCode + ":" + fe.ResultMsg);
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

//		/// <summary>
//		/// Service ȣ���� ���� �޼ҵ�
//		/// </summary>
//		public bool GetUserDetail(BaseModel baseModel)
//		{
//			
//			_log.Debug("-----------------------------------------");
//			_log.Debug( this.ToString() + " Start");
//			_log.Debug("-----------------------------------------");
//
//			_log.Debug("-----------------------------------------");
//			_log.Debug( this.ToString() + " End");
//			_log.Debug("-----------------------------------------");
//
//			return true;
//		}
//
		/// <summary>
		/// ��������� ����
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		public void SetSlotUpdate(SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�
//				if(slotOrganizationModel.UserID.Length < 1) 
//				{
//					throw new FrameException("�����ID�� �������� �ʽ��ϴ�.");
//				}
//				if(slotOrganizationModel.UserDept.Length > 20) 
//				{
//					throw new FrameException("�ҼӺμ����� ���̴� 20Bytes�� �ʰ��� �� �����ϴ�.");
//				}
//				if(slotOrganizationModel.UserTitle.Length > 20) 
//				{
//					throw new FrameException("��å���Ը��� ���̴� 20Bytes�� �ʰ��� �� �����ϴ�.");
//				}
//				if(slotOrganizationModel.UserTell.Length > 15) 
//				{
//					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
//				}
//				if(slotOrganizationModel.UserTell.Length > 15) 
//				{
//					throw new FrameException("�޴���ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�.");
//				}
//				if(slotOrganizationModel.UserComment.Length > 50) 
//				{
//					throw new FrameException("������ ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
//				}

	

				// ������ �ν��Ͻ� ����
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode    = slotOrganizationModel.MediaCode;
				remoteData.CategoryCode = slotOrganizationModel.CategoryCode;
				remoteData.GenreCode	= slotOrganizationModel.GenreCode;
				remoteData.ChannelNo    = slotOrganizationModel.ChannelNo;
				remoteData.Slot1    = slotOrganizationModel.Slot1;
				remoteData.Slot2    = slotOrganizationModel.Slot2;
				remoteData.Slot3    = slotOrganizationModel.Slot3;
				remoteData.Slot4    = slotOrganizationModel.Slot4;
				remoteData.Slot5    = slotOrganizationModel.Slot5;
				remoteData.Slot6    = slotOrganizationModel.Slot6;
				remoteData.Slot7    = slotOrganizationModel.Slot7;
				remoteData.Slot8    = slotOrganizationModel.Slot8;
				remoteData.Slot9    = slotOrganizationModel.Slot9;
				remoteData.Slot10   = slotOrganizationModel.Slot10;
				remoteData.Slot11   = slotOrganizationModel.Slot11;
				remoteData.Slot12   = slotOrganizationModel.Slot12;
				remoteData.Slot13   = slotOrganizationModel.Slot13;
				remoteData.Slot14   = slotOrganizationModel.Slot14;
				remoteData.Slot15   = slotOrganizationModel.Slot15;
				remoteData.UseYn    = slotOrganizationModel.UseYn;	
			
				remoteData.MediaCode_old    = slotOrganizationModel.MediaCode_old;
				remoteData.CategoryCode_old = slotOrganizationModel.CategoryCode_old;
				remoteData.GenreCode_old	= slotOrganizationModel.GenreCode_old;
				remoteData.ChannelNo_old    = slotOrganizationModel.ChannelNo_old;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSlotUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSlotUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// <param name="slotOrganizationModel"></param>
		/// <returns></returns>
		public void SetSlotAdd(SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� Start");
				_log.Debug("-----------------------------------------");

//				if(slotOrganizationModel.UserID.Trim().Length < 1) 
//				{
//					throw new FrameException("�����ID�� �������� �ʽ��ϴ�.");
//				}
//				if(slotOrganizationModel.UserID.Trim().Length > 10) 
//				{
//					throw new FrameException("�����ID�� 10Bytes�� �ʰ��� �� �����ϴ�.");
//				}
//				if(slotOrganizationModel.UserDept.Length > 20) 
//				{
//					throw new FrameException("�ҼӺμ����� ���̴� 20Bytes�� �ʰ��� �� �����ϴ�.");
//				}
//				if(slotOrganizationModel.UserTitle.Length > 20) 
//				{
//					throw new FrameException("��å���Ը��� ���̴� 20Bytes�� �ʰ��� �� �����ϴ�.");
//				}
//				if(slotOrganizationModel.UserTell.Length > 15) 
//				{
//					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
//				}
//				if(slotOrganizationModel.UserTell.Length > 15) 
//				{
//					throw new FrameException("�޴���ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�.");
//				}
//				if(slotOrganizationModel.UserComment.Length > 50) 
//				{
//					throw new FrameException("������ ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
//				}


				// ������ �ν��Ͻ� ����
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode    = slotOrganizationModel.MediaCode;
				remoteData.CategoryCode = slotOrganizationModel.CategoryCode;
				remoteData.GenreCode	= slotOrganizationModel.GenreCode;
				remoteData.ChannelNo    = slotOrganizationModel.ChannelNo;
				remoteData.Slot1    = slotOrganizationModel.Slot1;
				remoteData.Slot2    = slotOrganizationModel.Slot2;
				remoteData.Slot3    = slotOrganizationModel.Slot3;
				remoteData.Slot4    = slotOrganizationModel.Slot4;
				remoteData.Slot5    = slotOrganizationModel.Slot5;
				remoteData.Slot6    = slotOrganizationModel.Slot6;
				remoteData.Slot7    = slotOrganizationModel.Slot7;
				remoteData.Slot8    = slotOrganizationModel.Slot8;
				remoteData.Slot9    = slotOrganizationModel.Slot9;
				remoteData.Slot10   = slotOrganizationModel.Slot10;
				remoteData.Slot11   = slotOrganizationModel.Slot11;
				remoteData.Slot12   = slotOrganizationModel.Slot12;
				remoteData.Slot13   = slotOrganizationModel.Slot13;
				remoteData.Slot14   = slotOrganizationModel.Slot14;
				remoteData.Slot15   = slotOrganizationModel.Slot15;
									
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSlotCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public void SetSlotDelete(SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SlotOrganizationServicePloxy.SlotOrganizationService svc = new SlotOrganizationServicePloxy.SlotOrganizationService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				SlotOrganizationServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SlotOrganizationServicePloxy.HeaderModel();
				SlotOrganizationServicePloxy.SlotOrganizationModel remoteData   = new AdManagerClient.SlotOrganizationServicePloxy.SlotOrganizationModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode    = slotOrganizationModel.MediaCode;
				remoteData.CategoryCode = slotOrganizationModel.CategoryCode;
				remoteData.GenreCode	= slotOrganizationModel.GenreCode;
				remoteData.ChannelNo    = slotOrganizationModel.ChannelNo;
					
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSlotDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				slotOrganizationModel.ResultCnt   = remoteData.ResultCnt;
				slotOrganizationModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSlotDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
