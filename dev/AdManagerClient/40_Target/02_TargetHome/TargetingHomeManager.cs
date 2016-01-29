// ===============================================================================
// Targeting Manager  for Charites Project
//
// TargetingHomeManager.cs
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
	public class TargetingHomeManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public TargetingHomeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "TargetingHome";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Target/TargetingHomeService.asmx";
		}

		/// <summary>
		/// Ÿ���� �����ȸ
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetTargetingList(TargetingHomeModel targetingHomeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� �����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             =  targetingHomeModel.SearchKey;               
				remoteData.SearchMediaCode		 =  targetingHomeModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 =  targetingHomeModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     =  targetingHomeModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  =  targetingHomeModel.SearchAdvertiserCode;
				remoteData.SearchContractState	 =  targetingHomeModel.SearchContractState;
				remoteData.SearchAdType  		 =  targetingHomeModel.SearchAdType;       
				remoteData.SearchchkAdState_20	 =  targetingHomeModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 =  targetingHomeModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 =  targetingHomeModel.SearchchkAdState_40; 
				remoteData.SearchchkTimeY		 =  targetingHomeModel.SearchchkTimeY; 
				remoteData.SearchchkTimeN		 =  targetingHomeModel.SearchchkTimeN; 
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetTargetingList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				targetingHomeModel.TargetingDataSet = remoteData.TargetingDataSet.Copy();
				targetingHomeModel.ResultCnt   = remoteData.ResultCnt;
				targetingHomeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� �����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetTargetingList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// Ÿ���� �����ȸ
        /// </summary>
        /// <param name="targetingHomeModel"></param>
        public void GetTargetingList2(TargetingHomeModel targetingHomeModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("Ÿ���� �����ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingHomeServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
                TargetingHomeServicePloxy.TargetingHomeModel remoteData = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchKey = targetingHomeModel.SearchKey;
                remoteData.SearchMediaCode = targetingHomeModel.SearchMediaCode;
                remoteData.SearchRapCode = targetingHomeModel.SearchRapCode;
                remoteData.SearchAgencyCode = targetingHomeModel.SearchAgencyCode;
                remoteData.SearchAdvertiserCode = targetingHomeModel.SearchAdvertiserCode;
                remoteData.SearchContractState = targetingHomeModel.SearchContractState;
                remoteData.SearchAdType = targetingHomeModel.SearchAdType;
                remoteData.SearchchkAdState_20 = targetingHomeModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = targetingHomeModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = targetingHomeModel.SearchchkAdState_40;
                remoteData.SearchchkTimeY = targetingHomeModel.SearchchkTimeY;
                remoteData.SearchchkTimeN = targetingHomeModel.SearchchkTimeN;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetTargetingList2(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                targetingHomeModel.TargetingDataSet = remoteData.TargetingDataSet.Copy();
                targetingHomeModel.ResultCnt = remoteData.ResultCnt;
                targetingHomeModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("Ÿ���� �����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetTargetingList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ÿ���� �����ȸ
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetCollectionList(TargetingHomeModel targetingHomeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ�ٱ� �����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;
                
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
				targetingHomeModel.CollectionsDataSet = remoteData.CollectionsDataSet.Copy();
				targetingHomeModel.ResultCnt   = remoteData.ResultCnt;
				targetingHomeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ�ٱ� �����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetTargetCollectionList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ÿ���� �� ��ȸ
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetTargetingDetail(TargetingHomeModel targetingHomeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� �� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.ItemNo             =  targetingHomeModel.ItemNo;               
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetTargetingDetail(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				targetingHomeModel.DetailDataSet = remoteData.DetailDataSet.Copy();
				targetingHomeModel.ResultCnt   = remoteData.ResultCnt;
				targetingHomeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� �� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetTargetingDetail():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// Ÿ���� �� ��ȸ
        /// </summary>
        /// <param name="targetingHomeModel"></param>
        public void GetTargetingDetail2(TargetingHomeModel targetingHomeModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("Ÿ���� �� ��ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingHomeServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
                TargetingHomeServicePloxy.TargetingHomeModel remoteData = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = targetingHomeModel.ItemNo;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetTargetingDetail2(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                targetingHomeModel.DetailDataSet = remoteData.DetailDataSet.Copy();
                targetingHomeModel.ResultCnt = remoteData.ResultCnt;
                targetingHomeModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("Ÿ���� �� ��ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetTargetingDetail():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ÿ���� ������ ����
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void SetTargetingDetailUpdate(TargetingHomeModel targetingHomeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� ������ ���� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.ItemNo          = targetingHomeModel.ItemNo;
				remoteData.ItemName        = targetingHomeModel.ItemName;
				remoteData.ContractAmt     = targetingHomeModel.ContractAmt;
				remoteData.PriorityCd      = targetingHomeModel.PriorityCd;
				remoteData.AmtControlYn    = targetingHomeModel.AmtControlYn;
				remoteData.AmtControlRate  = targetingHomeModel.AmtControlRate;
				remoteData.TgtRegion1Yn    = targetingHomeModel.TgtRegion1Yn;
				remoteData.TgtRegion1      = targetingHomeModel.TgtRegion1;
				remoteData.TgtTimeYn       = targetingHomeModel.TgtTimeYn;
				remoteData.TgtTime         = targetingHomeModel.TgtTime;
				remoteData.TgtAgeYn        = targetingHomeModel.TgtAgeYn;
				remoteData.TgtAge          = targetingHomeModel.TgtAge;
				remoteData.TgtAgeBtnYn     = targetingHomeModel.TgtAgeBtnYn;
				remoteData.TgtAgeBtnBegin  = targetingHomeModel.TgtAgeBtnBegin;
				remoteData.TgtAgeBtnEnd    = targetingHomeModel.TgtAgeBtnEnd;
				remoteData.TgtSexYn        = targetingHomeModel.TgtSexYn;
				remoteData.TgtSexMan       = targetingHomeModel.TgtSexMan;
				remoteData.TgtSexWoman     = targetingHomeModel.TgtSexWoman;
//				remoteData.TgtRateYn       = targetingHomeModel.TgtRateYn;
//				remoteData.TgtRate         = targetingHomeModel.TgtRate;
				remoteData.TgtWeekYn       = targetingHomeModel.TgtWeekYn;
				remoteData.TgtWeek         = targetingHomeModel.TgtWeek;
				remoteData.TgtCollectionYn       = targetingHomeModel.TgtCollectionYn;
				remoteData.TgtCollection         = targetingHomeModel.TgtCollection;

                remoteData.TgtStbModelYn    = targetingHomeModel.TgtStbModelYn;
                remoteData.TgtStbModel      = targetingHomeModel.TgtStbModel;
                remoteData.TgtPocYn         = targetingHomeModel.TgtPocYn;
                remoteData.TgtPoc           = targetingHomeModel.TgtPoc;

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetTargetingDetailUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				targetingHomeModel.ResultCnt   = remoteData.ResultCnt;
				targetingHomeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� ������ ���� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchChoiceMenuDetailAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ÿ��������������ȸ
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetRegionList(TargetingHomeModel targetingHomeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ��������������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetRegionList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				targetingHomeModel.RegionDataSet = remoteData.RegionDataSet.Copy();
				targetingHomeModel.ResultCnt     = remoteData.ResultCnt;
				targetingHomeModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ��������������ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetRegionList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ÿ���ÿ��ɴ���ȸ
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetAgeList(TargetingHomeModel targetingHomeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���ÿ��ɴ���ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAgeList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				targetingHomeModel.AgeDataSet = remoteData.AgeDataSet.Copy();
				targetingHomeModel.ResultCnt     = remoteData.ResultCnt;
				targetingHomeModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���ÿ��ɴ���ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAgeList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// ����Ÿ���� �����ȸ
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingCollectionList(TargetingHomeModel targetingHomeModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����Ÿ�ٱ� �����ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingHomeServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
                TargetingHomeServicePloxy.TargetingHomeModel remoteData = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = targetingHomeModel.ItemNo;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetTargetingCollectionList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                targetingHomeModel.TargetingCollectionDataSet = remoteData.TargetingCollectionDataSet.Copy();
                targetingHomeModel.ResultCnt = remoteData.ResultCnt;
                targetingHomeModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("Ÿ�ٱ� �����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetTargetingCollectionList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        public void SetTargetingCollectionAdd(TargetingHomeModel targetingHomeModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����Ÿ���� �߰� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingHomeServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
                TargetingHomeServicePloxy.TargetingHomeModel remoteData = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = targetingHomeModel.ItemNo;
                remoteData.CollectionCode = targetingHomeModel.CollectionCode;
				remoteData.TgtCollectionYn = targetingHomeModel.TgtCollectionYn;

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
                targetingHomeModel.ResultCnt = remoteData.ResultCnt;
                targetingHomeModel.ResultCD = remoteData.ResultCD;

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
        public void SetTargetingCollectionDelete(TargetingHomeModel targetingHomeModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����Ÿ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingHomeServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
                TargetingHomeServicePloxy.TargetingHomeModel remoteData = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = targetingHomeModel.ItemNo;
                remoteData.CollectionCode = targetingHomeModel.CollectionCode;

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
                targetingHomeModel.ResultCnt = remoteData.ResultCnt;
                targetingHomeModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����Ÿ���� �߰� End");
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
