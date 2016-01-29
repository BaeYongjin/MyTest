// ===============================================================================
// Targeting Manager  for Charites Project
//
// TargetingManager.cs
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
 * -------------------------------------------------------
 * Class Name: TargetingManager
 * �ֿ���  : ������� Ÿ����
 * �ۼ���    : ��
 * �ۼ���    : ��
 * Ư�̻���  : 2SLOT ó�� �߰�
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.10.04
 * ��������  :        
 *            - 2slot ó�����ؼ� TargetingModel�� SlotExt ó��.
 *              
 * �����Լ�  :
 *            - SetTargetingDetailUpdate(..) 
 *            - 
 * -------------------------------------------------------- 
 * �����ڵ�  : [E_03]
 * ������    : �躸��
 * ������    : 2013.07.09
 * ��������  :        
 *            - ��ȣ�������˾� ��뿩��, �������, �����ڹ̼��⿩�� �߰�
 * �����Լ�  :
 *            - SetTargetingDetailUpdate()
 * --------------------------------------------------------  
 * �����ڵ�  : [E_04]
 * ������    : �躸��
 * ������    : 2013.10.16
 * ��������  :        
 *            - �������� Ÿ���� ��뿩��, ���ɴ�, �ŷڵ� �߰�
 * �����Լ�  :
 *            - SetTargetingDetailUpdate() ����
 *            - SetTargetingProfileAdd() �߰�
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
	/// Ÿ���� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class TargetingManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public TargetingManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "Targeting";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Target/TargetingService.asmx";
		}


		/// <summary>
		/// Ÿ���� ��󱤰� ����Ʈ ��ȸ
		/// </summary>
		/// <param name="targetingModel"></param>
		/// <param name="adType">10:��������, 20:��ü�����</param>
		public void GetTargetingList(TargetingModel targetingModel, string adType )
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� �����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				TargetingServicePloxy.HeaderModel    remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             =  targetingModel.SearchKey;               
				remoteData.SearchMediaCode		 =  targetingModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 =  targetingModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     =  targetingModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  =  targetingModel.SearchAdvertiserCode;
				remoteData.SearchContractState	 =  targetingModel.SearchContractState;
				remoteData.SearchAdType  		 =  adType;
                remoteData.SearchAdClass  		 =  targetingModel.SearchAdClass;           // ����Ÿ�������ϴ� ���

				remoteData.SearchchkAdState_20	 =  targetingModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 =  targetingModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 =  targetingModel.SearchchkAdState_40; 
                
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
				targetingModel.TargetingDataSet = remoteData.TargetingDataSet.Copy();
				targetingModel.ResultCnt   = remoteData.ResultCnt;
				targetingModel.ResultCD    = remoteData.ResultCD;

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
        /// ���� �����ȸ
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetCollectionsList(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("���� �����ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetCollectionList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                targetingModel.CollectionsDataSet = remoteData.CollectionsDataSet.Copy();
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("���� �����ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetCollectionsList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ÿ���� �� ��ȸ
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetTargetingDetail(TargetingModel targetingModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� �� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// ����Ʈ �� ����
				TargetingServicePloxy.HeaderModel		remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel	remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteHeader.UserLevel  = Header.UserLevel;
				remoteHeader.UserClass  = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.ItemNo		= targetingModel.ItemNo;
                
				svc.Timeout = FrameSystem.m_SystemTimeout;
                svc.Url     = _WebServiceUrl;
				remoteData  = svc.GetTargetingDetail(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				targetingModel.DetailDataSet = remoteData.DetailDataSet.Copy();
				targetingModel.ResultCnt   = remoteData.ResultCnt;
				targetingModel.ResultCD    = remoteData.ResultCD;

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
        /// <param name="targetingModel"></param>
        public void GetTargetingDetail2(TargetingModel targetingModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("Ÿ���� �� ��ȸ Start");
                _log.Debug("-----------------------------------------");

                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // ����Ʈ �� ����
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = targetingModel.ItemNo;

                svc.Timeout = FrameSystem.m_SystemTimeout;
                svc.Url = _WebServiceUrl;
                remoteData = svc.GetTargetingDetail2(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                targetingModel.DetailDataSet = remoteData.DetailDataSet.Copy();
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

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
		/// Ÿ���� ���� ��ȸ
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetTargetingRate(TargetingModel targetingModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				TargetingServicePloxy.HeaderModel		remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel	remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.ItemNo             =  targetingModel.ItemNo;               
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetTargetingRate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				targetingModel.RateDataSet = remoteData.RateDataSet.Copy();
				targetingModel.ResultCnt   = remoteData.ResultCnt;
				targetingModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetTargetingRate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Ÿ���� ������ ����
		/// </summary>
		/// <param name="targetingModel"></param>
		public void SetTargetingDetailUpdate(TargetingModel targetingModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� ������ ���� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// ����Ʈ �� ����
				TargetingServicePloxy.HeaderModel       remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel    remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
                remoteData.ItemNo          = targetingModel.ItemNo;
				remoteData.ItemName        = targetingModel.ItemName;
				remoteData.ContractAmt     = targetingModel.ContractAmt;
                remoteData.PriorityCd      = targetingModel.PriorityCd;
				remoteData.AmtControlYn    = targetingModel.AmtControlYn;
				remoteData.AmtControlRate  = targetingModel.AmtControlRate;
				remoteData.TgtRegion1Yn    = targetingModel.TgtRegion1Yn;
                remoteData.TgtRegion1      = targetingModel.TgtRegion1;
				remoteData.TgtTimeYn       = targetingModel.TgtTimeYn;
				remoteData.TgtTime         = targetingModel.TgtTime;
				remoteData.TgtAgeYn        = targetingModel.TgtAgeYn;
				remoteData.TgtAge          = targetingModel.TgtAge;
				remoteData.TgtAgeBtnYn     = targetingModel.TgtAgeBtnYn;
				remoteData.TgtAgeBtnBegin  = targetingModel.TgtAgeBtnBegin;
				remoteData.TgtAgeBtnEnd    = targetingModel.TgtAgeBtnEnd;
				remoteData.TgtSexYn        = targetingModel.TgtSexYn;
				remoteData.TgtSexMan       = targetingModel.TgtSexMan;
				remoteData.TgtSexWoman     = targetingModel.TgtSexWoman;
				remoteData.TgtRateYn       = targetingModel.TgtRateYn;
				remoteData.TgtRate         = targetingModel.TgtRate;
				remoteData.TgtWeekYn       = targetingModel.TgtWeekYn;
				remoteData.TgtWeek         = targetingModel.TgtWeek;
				remoteData.TgtCollectionYn = targetingModel.TgtCollectionYn;
				remoteData.TgtCollection   = targetingModel.TgtCollection;

				remoteData.TgtZipYn			= targetingModel.TgtZipYn;
				remoteData.TgtZip			= targetingModel.TgtZip;
				remoteData.TgtPPxYn			= targetingModel.TgtPPxYn;
				remoteData.TgtFreqYn		= targetingModel.TgtFreqYn;
				remoteData.TgtFreqDay		= targetingModel.TgtFreqDay;
				remoteData.TgtFreqFeriod	= targetingModel.TgtFreqFeriod;
				remoteData.TgtPVSYn			= targetingModel.TgtPVSYn;

                remoteData.SlotExt          = targetingModel.SlotExt;			//[E_01]
                remoteData.TgtStbModelYn    = targetingModel.TgtStbModelYn;  // [E_08]
                remoteData.TgtStbModel      = targetingModel.TgtStbModel;    // [E_08]

                remoteData.TgtPrefYn        = targetingModel.TgtPrefYn;      // [E_09]
                remoteData.TgtPrefRate      = targetingModel.TgtPrefRate;    // [E_09]
                remoteData.TgtPrefNosend    = targetingModel.TgtPrefNosend;  // [E_09]
                remoteData.TgtProfileYn     = targetingModel.TgtProfileYn;   // [E_04]
                remoteData.TgtProfile       = targetingModel.TgtProfile;     // [E_04]
                remoteData.TgtReliablilty   = targetingModel.TgtReliablilty; // [E_04]

                remoteData.TgtPocYn         = targetingModel.TgtPocYn;
                remoteData.TgtPoc           = targetingModel.TgtPoc;

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
                remoteData = svc.SetTargetingDetailUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				targetingModel.ResultCnt   = remoteData.ResultCnt;
				targetingModel.ResultCD    = remoteData.ResultCD;

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
		/// Ÿ���� ���� ����
		/// </summary>
		/// <param name="targetingModel"></param>
		public void SetTargetingRateUpdate(TargetingModel targetingModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� ������ ���� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				TargetingServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.ItemNo     = targetingModel.ItemNo;
				remoteData.Type		  = targetingModel.Type;	
				remoteData.Rate1	  = targetingModel.Rate1;	
				remoteData.Rate2	  = targetingModel.Rate2;	
				remoteData.Rate3	  = targetingModel.Rate3;	
				remoteData.Rate4	  = targetingModel.Rate4;	
				remoteData.Rate5	  = targetingModel.Rate5;	
				remoteData.Rate6	  = targetingModel.Rate6;	
				remoteData.Rate7	  = targetingModel.Rate7;	
				remoteData.Rate8	  = targetingModel.Rate8;	
				remoteData.Rate9	  = targetingModel.Rate9;	
				remoteData.Rate10	  = targetingModel.Rate10;	
				remoteData.Rate11	  = targetingModel.Rate11;	
				remoteData.Rate12	  = targetingModel.Rate12;	
				remoteData.Rate13	  = targetingModel.Rate13;	
				remoteData.Rate14	  = targetingModel.Rate14;	
				remoteData.Rate15	  = targetingModel.Rate15;	
				remoteData.Rate16	  = targetingModel.Rate16;	
				remoteData.Rate17	  = targetingModel.Rate17;	
				remoteData.Rate18	  = targetingModel.Rate18;	
				remoteData.Rate19	  = targetingModel.Rate19;	
				remoteData.Rate20	  = targetingModel.Rate20;	
				remoteData.Rate21	  = targetingModel.Rate21;	
				remoteData.Rate22	  = targetingModel.Rate22;	
				remoteData.Rate23	  = targetingModel.Rate23;
				remoteData.Rate24	  = targetingModel.Rate24;	
				

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetTargetingRateUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				targetingModel.ResultCnt   = remoteData.ResultCnt;
				targetingModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���� ������ ���� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetTargetingRateUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// <param name="targetingModel"></param>
		public void GetRegionList(TargetingModel targetingModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ��������������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				TargetingServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

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
				targetingModel.RegionDataSet = remoteData.RegionDataSet.Copy();
				targetingModel.ResultCnt     = remoteData.ResultCnt;
				targetingModel.ResultCD      = remoteData.ResultCD;

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
		/// <param name="targetingModel"></param>
		public void GetAgeList(TargetingModel targetingModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("Ÿ���ÿ��ɴ���ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			
			
				// ����Ʈ �� ����
				TargetingServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

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
				targetingModel.AgeDataSet = remoteData.AgeDataSet.Copy();
				targetingModel.ResultCnt     = remoteData.ResultCnt;
				targetingModel.ResultCD      = remoteData.ResultCD;

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
        public void GetTargetingCollectionList(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����Ÿ�ٱ� �����ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = targetingModel.ItemNo;

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
                targetingModel.TargetingCollectionDataSet = remoteData.TargetingCollectionDataSet.Copy();
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

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
        public void SetTargetingCollectionAdd(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����Ÿ���� �߰� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = targetingModel.ItemNo;
                remoteData.CollectionCode = targetingModel.CollectionCode;
				remoteData.TgtCollectionYn = targetingModel.TgtCollectionYn;

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
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

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
        public void SetTargetingCollectionDelete(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����Ÿ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = targetingModel.ItemNo;
                remoteData.CollectionCode = targetingModel.CollectionCode;

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
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

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

        /// <summary>
        /// [E_08] ��ž�� ��ȸ
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetStbList(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("��ž����ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetStbList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                targetingModel.TargetingDataSet = remoteData.TargetingDataSet.Copy();
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("��ž�𵨸���Ʈ��ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetStbList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// [E_04] �������� Ÿ���� ����
        /// </summary>
        /// <param name="targetingModel"></param>
        public void SetTargetingProfileAdd(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�������� Ÿ���� ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // ������ URL���� ����
                svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = targetingModel.ItemNo;
                remoteData.TgtProfile = targetingModel.TgtProfile;
                remoteData.TgtReliablilty = targetingModel.TgtReliablilty;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetTargetingProfileAdd(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�������� Ÿ���� ���� End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetTargetingProfileAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
