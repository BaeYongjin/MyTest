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
	public class GradeManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public GradeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Target/GradeService.asmx";
		}

		/// <summary>
		/// �̵���޺�������ȸ
		/// </summary>
		/// <param name="gradeModel"></param>
		public void GetGradeCodeList(GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ��޺� �����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;
								
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGradeCodeList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				gradeModel.GradeDataSet = remoteData.GradeDataSet.Copy();
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;

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
		/// ä��������ȸ
		/// </summary>
		/// <param name="gradeModel"></param>
		public void GetGradeList(GradeModel gradeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�θ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.MediaCode       = gradeModel.MediaCode;
								
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGradeList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				gradeModel.GradeDataSet = remoteData.GradeDataSet.Copy();
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�θ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelSetList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ä�� ������ �����ȸ
		/// </summary>
		/// <param name="gradeModel"></param>
		public void GetContractItemList(GradeModel gradeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�� ������ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             = gradeModel.SearchKey;
				remoteData.MediaCode             = gradeModel.MediaCode;
				remoteData.Code					 = gradeModel.Code;        				
				remoteData.SearchchkAdState_10	 = gradeModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 = gradeModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 = gradeModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 = gradeModel.SearchchkAdState_40; 
				
				//remoteData.SearchChkSch_YN		 = contractItemModel.SearchChkSch_YN; 
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetContractItemList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				gradeModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�� ������ �����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelSetList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ä�μ� ����
		/// </summary>
		/// <param name="gradeModel"></param>
		public void SetGradeUpdate(GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�
				if(gradeModel.CodeName.Length > 50) 
				{
					throw new FrameException("�ڵ���� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}				

				// ������ �ν��Ͻ� ����
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.Code       = gradeModel.Code;
				remoteData.CodeName     = gradeModel.CodeName;				
				remoteData.Grade       = gradeModel.Grade;
								
				remoteData.Code_O       = gradeModel.Code_O;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGradeUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�μ��������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGradeUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �����ü���౤�����߰�
		/// </summary>
		/// <param name="gradeModel"></param>
		/// <returns></returns>
		public void SetGradeCreate(GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�μ��߰� Start");
				_log.Debug("-----------------------------------------");
				
				if(gradeModel.CodeName.Length > 50) 
				{
					throw new FrameException("ä��No�� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}			
				
				// ������ �ν��Ͻ� ����
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();

				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.Code       = gradeModel.Code;
				remoteData.CodeName     = gradeModel.CodeName;				
				remoteData.Grade       = gradeModel.Grade;	
								
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGradeCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�μ��߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				gradeModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGradeCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				gradeModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		
		/// <summary>
		/// ä�μ� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetGradeDelete(GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�μ»��� start");
				_log.Debug("-----------------------------------------");
            
				// ������ �ν��Ͻ� ����
				GradeServicePloxy.GradeService svc = new GradeServicePloxy.GradeService();
				svc.Url = _WebServiceUrl;
            				
				// ����Ʈ �� ����
				GradeServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GradeServicePloxy.HeaderModel();
				GradeServicePloxy.GradeModel remoteData   = new AdManagerClient.GradeServicePloxy.GradeModel();
            
				// ������� ��Ʈ
				//remoteHeader.SearchKey     = Header.SearchKey;
				remoteHeader.UserID        = Header.UserID;
            
				// ȣ��������Ʈ
				remoteData.Code       = gradeModel.Code;	
            					
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetGradeDelete(remoteHeader, remoteData);
            
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// ��� ��Ʈ
				gradeModel.ResultCnt   = remoteData.ResultCnt;
				gradeModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�μ»��� end");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGradeDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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