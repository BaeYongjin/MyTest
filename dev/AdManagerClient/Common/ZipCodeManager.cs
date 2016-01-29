/*
 * -------------------------------------------------------
 * Class Name: ZipCodeManager
 * �ֿ���  : �����ȣ �˻� ���� client �κ�.
 * �ۼ���    : ? 
 * �ۼ���    : ?
 * Ư�̻���  : 
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.08.13
 * �����κ�  :
 *				- GetZipPreCodeList(..)              
 * ��������  :           
				- �����ȣ �� 3�ڸ��θ� �˻� ��� �߰�
 * --------------------------------------------------------
 * 
 */

using System;
using System.Data;
using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;
using AdManagerModel;

namespace AdManagerClient.Common
{
	/// <summary>
	/// CategoryGenreManager�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ZipCodeManager : BaseManager 
	{
		public ZipCodeManager( SystemModel systemModel, CommonModel commonModel) : base( systemModel, commonModel)
		{
            _log = FrameSystem.oLog;
            _module = "ZipCode";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Common/ZipCodeService.asmx";
		}

        
		public void GetZipCodeList(ZipCodeModel  model)
        {
            try
            {
                ZipCodeServiceProxy.ZipCodeService	svc = new AdManagerClient.ZipCodeServiceProxy.ZipCodeService();
                ZipCodeServiceProxy.HeaderModel		remoteHeader = new AdManagerClient.ZipCodeServiceProxy.HeaderModel();
                ZipCodeServiceProxy.ZipCodeModel	remoteData   = new AdManagerClient.ZipCodeServiceProxy.ZipCodeModel();
			
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;
			
                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;

				remoteData.SearchKey	=	"%" + model.SearchKey + "%";
                remoteData = svc.GetZipList( remoteHeader, remoteData );

                if(!remoteData.ResultCD.Equals("0000")) throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);

                model.DsAddr		= remoteData.DsAddr.Copy();
                model.ResultCnt     = remoteData.ResultCnt;
                model.ResultCD      = remoteData.ResultCD;
                model.ResultDesc    = remoteData.ResultDesc;
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
		/// �����ȣ �� 3�ڸ��θ� �˻�[E_01]
		/// </summary>		
		public void GetZipPreCodeList(ZipCodeModel  model)
		{
			try
			{
				ZipCodeServiceProxy.ZipCodeService	svc = new AdManagerClient.ZipCodeServiceProxy.ZipCodeService();
				ZipCodeServiceProxy.HeaderModel		remoteHeader = new AdManagerClient.ZipCodeServiceProxy.HeaderModel();
				ZipCodeServiceProxy.ZipCodeModel	remoteData   = new AdManagerClient.ZipCodeServiceProxy.ZipCodeModel();
			
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;
			
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;
				if (model.SearchKey != null && model.SearchKey.Length >3)
					remoteData.SearchKey	=	"%" + model.SearchKey + "%";
				else
					remoteData.SearchKey = "";
				remoteData.SearchZip    =   model.SearchZip;
				remoteData = svc.GetPreZipList( remoteHeader, remoteData );

				if(!remoteData.ResultCD.Equals("0000")) throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);

				model.DsAddr		= remoteData.DsAddr.Copy();
				model.ResultCnt     = remoteData.ResultCnt;
				model.ResultCD      = remoteData.ResultCD;
				model.ResultDesc    = remoteData.ResultDesc;
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
		/// Ÿ���� ����� �����ȣ�� �˻�
		/// </summary>
		/// <param name="model"></param>
		public void GetZipIncludeList(ZipCodeModel model)
		{
			try
			{
				ZipCodeServiceProxy.ZipCodeService	svc = new AdManagerClient.ZipCodeServiceProxy.ZipCodeService();
				ZipCodeServiceProxy.HeaderModel		remoteHeader = new AdManagerClient.ZipCodeServiceProxy.HeaderModel();
				ZipCodeServiceProxy.ZipCodeModel	remoteData   = new AdManagerClient.ZipCodeServiceProxy.ZipCodeModel();
			
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;
			
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;
				
				remoteData.SearchZip    =   model.SearchZip;
				remoteData = svc.GetIncludeZipList( remoteHeader, remoteData );
				
				if(!remoteData.ResultCD.Equals("0000")) throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);

				model.DsAddr		= remoteData.DsAddr.Copy();
				model.ResultCnt     = remoteData.ResultCnt;
				model.ResultCD      = remoteData.ResultCD;
				model.ResultDesc    = remoteData.ResultDesc;
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

	}
}
