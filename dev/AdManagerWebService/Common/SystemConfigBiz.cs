using System;
using System.Data;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Common
{
	/// <summary>
	/// SystemConfigBiz
	/// </summary>
	public class SystemConfigBiz : BaseBiz
	{
		public SystemConfigBiz() : base(FrameSystem.connDbString,true)
		{
			_log = FrameSystem.oLog;
		}

        #region 환경설정 조회	
		/// <summary>
		///  환경설정 조회
		/// </summary>
		/// <param name="systemConfigModel"></param>
		public void GetSystemConfigList(HeaderModel header, SystemConfigModel systemConfigModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSystemConfigList() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("<입력정보>");
				_log.Debug("FtpUploadID :[" + systemConfigModel.FtpUploadID + "]");

				StringBuilder sbQuery = new StringBuilder();
				// 쿼리생성
				sbQuery.Append("\n " + "SELECT	FTP_ULD_ID		AS FtpUploadID");
				sbQuery.Append("\n " + "	,	FTP_ULD_PWD		AS FtpUploadPW");
				sbQuery.Append("\n " + "	,	FTP_ULD_HOST	AS FtpUploadHost");
				sbQuery.Append("\n " + "	,	FTP_ULD_PORT	AS FtpUploadPort");
				sbQuery.Append("\n " + "	,	FTP_ULD_PATH	AS FtpUploadPath");
				sbQuery.Append("\n " + "	,	FTP_MV_PATH		AS FtpMovePath");
				sbQuery.Append("\n " + "	,	FTP_MV_YN		AS FtpMoveUseYn");
				sbQuery.Append("\n " + "	,	FTP_CDN_ID		AS FtpCdnID");
				sbQuery.Append("\n " + "	,	FTP_CDN_PWD		AS FtpCdnPW");
				sbQuery.Append("\n " + "	,	FTP_CDN_HOST	AS FtpCdnHost");
				sbQuery.Append("\n " + "	,	FTP_CDN_PORT	AS FtpCdnPort");
				sbQuery.Append("\n " + "	,	'N'				AS FileQueueUseYn");
				sbQuery.Append("\n " + "	,	'0'				AS FileQueueInterval");
				sbQuery.Append("\n " + "	,	'0'				AS FileQueueCnt");
				sbQuery.Append("\n " + "	,	''				AS URLGetAdPopList");
				sbQuery.Append("\n " + "	,	''				AS URLSetAdPop");
				sbQuery.Append("\n " + "	,	''				AS CmsMasUrl");
				sbQuery.Append("\n " + "	,	''				AS CmsMasQuery");
				sbQuery.Append("\n " + "FROM	STM_PROP");
				sbQuery.Append("\n " + "WHERE	MDA_COD = 1");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.Open();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				systemConfigModel.SystemConfigDataSet = ds.Copy();
				systemConfigModel.ResultCnt = Utility.GetDatasetCount(systemConfigModel.SystemConfigDataSet);
				systemConfigModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + systemConfigModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSystemConfigList() End");
				_log.Debug("-----------------------------------------");
			}
			catch (Exception ex)
			{
				systemConfigModel.ResultCD = "3000";
				systemConfigModel.ResultDesc = "환경설정정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
				_db.Close();
			}
			finally
			{
				_db.Close();
			}

		}
		#endregion	

		#region 환경설정		
		/// <summary>
		/// 환경설정 저장
		/// </summary>		
		/// <returns></returns>
		public void SetSystemConfigUpdate(HeaderModel header, SystemConfigModel systemConfigModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetTargetingDetailUpdate() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("FtpUploadID     :[" + systemConfigModel.FtpUploadID       + "]");
				_log.Debug("FtpUploadPW     :[" + systemConfigModel.FtpUploadPW       + "]");
				_log.Debug("FtpUploadHost   :[" + systemConfigModel.FtpUploadHost     + "]");
				_log.Debug("FtpUploadPort   :[" + systemConfigModel.FtpUploadPort     + "]");
				_log.Debug("FtpUploadPath   :[" + systemConfigModel.FtpUploadPath     + "]");
				_log.Debug("FtpMovePath     :[" + systemConfigModel.FtpMovePath       + "]");
				_log.Debug("FtpMoveUseYn    :[" + systemConfigModel.FtpMoveUseYn      + "]");
				_log.Debug("FtpCdnID        :[" + systemConfigModel.FtpCdnID          + "]");
				_log.Debug("FtpCdnPW        :[" + systemConfigModel.FtpCdnPW          + "]");
				_log.Debug("FtpCdnHost      :[" + systemConfigModel.FtpCdnHost        + "]");
				_log.Debug("FtpCdnPort      :[" + systemConfigModel.FtpCdnPort        + "]");

				int rc = 0;
				StringBuilder  sbQuery;
				OracleParameter[] sqlParams = new OracleParameter[11];

				sqlParams[0] = new OracleParameter(":FtpUploadID"      , OracleDbType.Varchar2 ,  20);
				sqlParams[1] = new OracleParameter(":FtpUploadPW"      , OracleDbType.Varchar2 ,  30);
				sqlParams[2] = new OracleParameter(":FtpUploadHost"    , OracleDbType.Varchar2 ,  20);
				sqlParams[3] = new OracleParameter(":FtpUploadPort"    , OracleDbType.Varchar2 ,   5);				
				sqlParams[4] = new OracleParameter(":FtpUploadPath"    , OracleDbType.Varchar2 ,  50);
				sqlParams[5] = new OracleParameter(":FtpMovePath"      , OracleDbType.Varchar2 ,  50);
				sqlParams[6] = new OracleParameter(":FtpMoveUseYn"     , OracleDbType.Char    ,   1);				
				sqlParams[7] = new OracleParameter(":FtpCdnID"         , OracleDbType.Varchar2 ,  20);				
				sqlParams[8] = new OracleParameter(":FtpCdnPW"         , OracleDbType.Varchar2 ,  30);
				sqlParams[9] = new OracleParameter(":FtpCdnHost"       , OracleDbType.Varchar2 ,  20);
				sqlParams[10] = new OracleParameter(":FtpCdnPort"      , OracleDbType.Varchar2 ,   5);				
				//sqlParams[11] = new OracleParameter("@CmsMasUrl"		, OracleDbType.Varchar2 , 200);
				//sqlParams[12] = new OracleParameter("@CmsMasQuery"		, OracleDbType.Varchar2 , 200);

				sqlParams[0].Value = systemConfigModel.FtpUploadID.Trim();
				sqlParams[1].Value = systemConfigModel.FtpUploadPW;
				sqlParams[2].Value = systemConfigModel.FtpUploadHost;
				sqlParams[3].Value = systemConfigModel.FtpUploadPort;
				sqlParams[4].Value = systemConfigModel.FtpUploadPath;
				sqlParams[5].Value = systemConfigModel.FtpMovePath;
				sqlParams[6].Value = systemConfigModel.FtpMoveUseYn;
				sqlParams[7].Value = systemConfigModel.FtpCdnID.Trim();
				sqlParams[8].Value = systemConfigModel.FtpCdnPW;
				sqlParams[9].Value = systemConfigModel.FtpCdnHost;
				sqlParams[10].Value = systemConfigModel.FtpCdnPort;
				//sqlParams[11].Value = systemConfigModel.CmsMasUrl;
				//sqlParams[12].Value = systemConfigModel.CmsMasQuery;
    
				// 쿼리실행
				try
				{
					_db.Open();

					sbQuery = new StringBuilder();
					sbQuery.Append(" UPDATE STM_PROP							\n");
					sbQuery.Append("    SET Ftp_ULD_ID		= :FtpUploadID      \n");
					sbQuery.Append("       ,Ftp_ULD_PWD		= :FtpUploadPW      \n");
					sbQuery.Append("       ,Ftp_ULD_HOST  	= :FtpUploadHost	\n");
					sbQuery.Append("       ,Ftp_ULD_PORT	= :FtpUploadPort    \n");
					sbQuery.Append("       ,Ftp_ULD_PATH  	= :FtpUploadPath    \n");
					sbQuery.Append("       ,Ftp_MV_PATH    	= :FtpMovePath      \n");
					sbQuery.Append("       ,Ftp_MV_YN		= :FtpMoveUseYn     \n");
					sbQuery.Append("       ,Ftp_CDN_ID      = :FtpCdnID         \n");
					sbQuery.Append("       ,Ftp_CDN_PWD     = :FtpCdnPW         \n");
					sbQuery.Append("       ,Ftp_CDN_HOST    = :FtpCdnHost       \n");
					sbQuery.Append("       ,Ftp_CDN_PORT	= :FtpCdnPort       \n");
					sbQuery.Append(" WHERE  MDA_COD = 1");
					
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Debug(sbQuery.ToString());

					if(rc < 1)
					{
						// 지정채널 편성 테이블에 추가
						sbQuery = new StringBuilder();
						sbQuery.Append("\n");
						sbQuery.Append(" INSERT INTO STM_PROP			\n");
						sbQuery.Append("            (MDA_COD			\n");
						sbQuery.Append("            ,Ftp_ULD_ID			\n");
						sbQuery.Append("            ,Ftp_ULD_PWD		\n");
						sbQuery.Append("            ,Ftp_ULD_HOST  		\n");
						sbQuery.Append("            ,Ftp_ULD_PORT		\n");
						sbQuery.Append("            ,Ftp_ULD_PATH  		\n");
						sbQuery.Append("            ,Ftp_MV_PATH    	\n");
						sbQuery.Append("            ,Ftp_MV_YN			\n");
						sbQuery.Append("            ,Ftp_CDN_ID         \n");
						sbQuery.Append("            ,Ftp_CDN_PWD        \n");
						sbQuery.Append("            ,Ftp_CDN_HOST       \n");
						sbQuery.Append("            ,Ftp_CDN_PORT		\n");
						sbQuery.Append("            )				    \n");
						sbQuery.Append("      VALUES				    \n");
						sbQuery.Append("            (1					\n");
						sbQuery.Append("            ,:FtpUploadID       \n");
						sbQuery.Append("            ,:FtpUploadPW       \n");
						sbQuery.Append("            ,:FtpUploadHost     \n");
						sbQuery.Append("            ,:FtpUploadPort     \n");
						sbQuery.Append("            ,:FtpUploadPath     \n");
						sbQuery.Append("            ,:FtpMovePath       \n");
						sbQuery.Append("            ,:FtpMoveUseYn      \n");
						sbQuery.Append("            ,:FtpCdnID          \n");
						sbQuery.Append("            ,:FtpCdnPW          \n");
						sbQuery.Append("            ,:FtpCdnHost	    \n");
						sbQuery.Append("            ,:FtpCdnPort	    \n");
						sbQuery.Append(" 			)				    \n");
						_log.Debug(sbQuery.ToString());

						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
						
					}
					_log.Message("환경설정 수정:등록자:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					throw ex;
				}

				systemConfigModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSystemConfigUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				systemConfigModel.ResultCD   = "3201";
				systemConfigModel.ResultDesc = "환경설정 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}
		#endregion	
	}
}
