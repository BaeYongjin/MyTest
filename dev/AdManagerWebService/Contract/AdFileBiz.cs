using System;
using System.Data;
using System.Data.SqlClient;
using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Contract
{
	/// <summary>
	/// AdFileBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdFileBiz : BaseBiz
	{
		public AdFileBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}
		/// <summary>
		/// �������� �����ȸ
		/// </summary>
		/// <param name="adFileModel"></param>
		public void GetAdFileList(HeaderModel header, AdFileModel adFileModel)
		{
			bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey              :[" + adFileModel.SearchKey            + "]");		// �˻� ��
				_log.Debug("SearchMediaCode	       :[" + adFileModel.SearchMediaCode	    + "]");		// �˻� ��ü
				_log.Debug("SearchRapCode          :[" + adFileModel.SearchRapCode        + "]");		// �˻� ��
				_log.Debug("SearchAgencyCode       :[" + adFileModel.SearchAgencyCode     + "]");		// �˻� �����
				_log.Debug("SearchAdvertiserCode   :[" + adFileModel.SearchAdvertiserCode + "]");		// �˻� ������
				_log.Debug("SearchAdType           :[" + adFileModel.SearchAdType        + "]");		// �˻� ��������
				_log.Debug("SearchFileType         :[" + adFileModel.SearchFileType       + "]");		// �˻� ������
				_log.Debug("SearchchkAdState_10    :[" + adFileModel.SearchchkAdState_10  + "]");		// �˻� ������� : �غ�
				_log.Debug("SearchchkAdState_20    :[" + adFileModel.SearchchkAdState_20  + "]");		// �˻� ������� : ��
				_log.Debug("SearchchkAdState_30    :[" + adFileModel.SearchchkAdState_30  + "]");		// �˻� ������� : ����	
				_log.Debug("SearchchkAdState_40    :[" + adFileModel.SearchchkAdState_40  + "]");		// �˻� ������� : ����   
				_log.Debug("SearchchkFileState_10  :[" + adFileModel.SearchchkFileState_10  + "]");		// �˻� ���ϻ��� : �̵��
				_log.Debug("SearchchkFileState_11  :[" + adFileModel.SearchchkFileState_11  + "]");		// �˻� ���ϻ��� : ���米ü���
				_log.Debug("SearchchkFileState_12  :[" + adFileModel.SearchchkFileState_12  + "]");		// �˻� ���ϻ��� : ������
				_log.Debug("SearchchkFileState_15  :[" + adFileModel.SearchchkFileState_15  + "]");		// �˻� ���ϻ��� : �������
				_log.Debug("SearchchkFileState_20  :[" + adFileModel.SearchchkFileState_20  + "]");		// �˻� ���ϻ��� : ��������(FTP���)
				_log.Debug("SearchchkFileState_30  :[" + adFileModel.SearchchkFileState_30  + "]");		// �˻� ���ϻ��� : CDN����
				_log.Debug("SearchchkFileState_40  :[" + adFileModel.SearchchkFileState_40  + "]");		// �˻� ���ϻ��� : ��ž����
				_log.Debug("SearchchkFileState_90  :[" + adFileModel.SearchchkFileState_90  + "]");		// �˻� ���ϻ��� : ��ž����
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n " + "SELECT  A.ITEM_NO       AS ItemNo		");
				sbQuery.Append("\n " + "    ,   A.ITEM_NM       AS ItemName		");
				sbQuery.Append("\n " + "	,   A.ADVT_TYP      AS AdType		");
				sbQuery.Append("\n " + "	,   J.STM_COD_NM    AS AdTypeName	");
				sbQuery.Append("\n " + "	,   A.ADVT_STT      AS AdState		");
				sbQuery.Append("\n " + "	,   B.STM_COD_NM    AS AdStateName	");
				sbQuery.Append("\n " + "	,   A.FILE_NM_PRE   AS PreFileName	");
				sbQuery.Append("\n " + "	,   A.FILE_NM       AS FileName		");
				sbQuery.Append("\n " + "	,   A.FILE_TYP      AS FileType		");
				sbQuery.Append("\n " + "	,   C.STM_COD_NM    AS FileTypeName	");
				sbQuery.Append("\n " + "	,   A.FILE_STT      AS FileState	");
				sbQuery.Append("\n " + "	,   D.STM_COD_NM    AS FileStateName");
				sbQuery.Append("\n " + "	,   A.FILE_LEN      AS FileLength	");
				sbQuery.Append("\n " + "	,   A.FILE_PATH     AS FilePath		");
				sbQuery.Append("\n " + "	,   '0'             AS DownLevel	");
				sbQuery.Append("\n " + "	,   '0����'         AS DownLevelName");
				sbQuery.Append("\n " + "	,   A.FILE_REG_DT   AS FileRegDt	");
				sbQuery.Append("\n " + "	,   A.FILE_REG_ID   AS FileRegID	");
				sbQuery.Append("\n " + "	,   E.USER_NM       AS FileRegName	");
				sbQuery.Append("\n " + "	,   A.ADVT_TM       AS AdTime		");
				sbQuery.Append("\n " + "	,   A.FILE_CHK_DT   AS CheckDt		");
				sbQuery.Append("\n " + "	,   F.USER_NM       AS CheckName	");
				sbQuery.Append("\n " + "	,   A.FILE_SYNC_DT  AS CDNSyncDt	");
				sbQuery.Append("\n " + "	,   G.USER_NM       AS CDNSyncName	");
				sbQuery.Append("\n " + "	,   A.FILE_PUB_DT   AS CDNPubDt		");
				sbQuery.Append("\n " + "	,   H.USER_NM       AS CDNPubName	");
				sbQuery.Append("\n " + "	,   A.FILE_DEL_DT   AS STBDelDt		");
				sbQuery.Append("\n " + "	,   I.USER_NM       AS STBDelName	");
				sbQuery.Append("\n " + "FROM    ADVT_MST A						");
				sbQuery.Append("\n " + "LEFT JOIN STM_COD B ON (A.ADVT_STT = B.STM_COD and B.STM_COD_CLS = '25')    -- 25:�������");
				sbQuery.Append("\n " + "LEFT JOIN STM_COD J ON (A.ADVT_TYP = J.STM_COD and J.STM_COD_CLS = '26')    -- 26:��������");
				sbQuery.Append("\n " + "LEFT JOIN STM_COD C ON (A.FILE_TYP = C.STM_COD and C.STM_COD_CLS = '24')    -- 24:���ϱ���");
				sbQuery.Append("\n " + "LEFT JOIN STM_COD D ON (A.FILE_STT = D.STM_COD and D.STM_COD_CLS = '31')    -- 31:���ϻ���");
				sbQuery.Append("\n " + "LEFT JOIN STM_USER E ON (A.FILE_REG_ID  = E.User_ID)");
				sbQuery.Append("\n " + "LEFT JOIN STM_USER F ON (A.FILE_CHK_ID  = F.User_ID)");
				sbQuery.Append("\n " + "LEFT JOIN STM_USER G ON (A.FILE_SYNC_ID = G.User_ID)");
				sbQuery.Append("\n " + "LEFT JOIN STM_USER H ON (A.FILE_PUB_ID  = H.User_ID)");
				sbQuery.Append("\n " + "LEFT JOIN STM_USER I ON (A.FILE_DEL_ID  = I.User_ID)");
				sbQuery.Append("\n " + "WHERE 1 = 1");
				
				// �˻�� ������
				if (adFileModel.SearchKey.Trim().Length > 0)
				{
					sbQuery.Append("\n AND ("
						+ "    A.FILE_NM LIKE '%" + adFileModel.SearchKey.Trim() + "%' \n"
						+ " OR A.ITEM_NM LIKE '%" + adFileModel.SearchKey.Trim() + "%' \n"+ " ) ");
				}
				
				// ���������� ����������
				if(adFileModel.SearchAdType.Trim().Length > 0 && !adFileModel.SearchAdType.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.ADVT_TYP = '" + adFileModel.SearchAdType.Trim() + "' \n");
				}

				// ����Ÿ���� ����������
				if(adFileModel.SearchFileType.Trim().Length > 0 && !adFileModel.SearchFileType.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.FILE_TYP = '" + adFileModel.SearchFileType.Trim() + "' \n");
				}		

				// ������� �غ�
				if(adFileModel.SearchchkAdState_10.Trim().Length > 0 && adFileModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND ( A.ADVT_STT = '10' \n");
					isState = true;
				}	
				// ������� ��
				if(adFileModel.SearchchkAdState_20.Trim().Length > 0 && adFileModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.ADVT_STT = '20' \n");
					isState = true;
				}	
				// ������� ����
				if(adFileModel.SearchchkAdState_30.Trim().Length > 0 && adFileModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.ADVT_STT = '30' \n");
					isState = true;
				}	
				// ������� ����
				if(adFileModel.SearchchkAdState_40.Trim().Length > 0 && adFileModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.ADVT_STT = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				// ���ϻ��� ���ÿ� ����
				isState = false;

				// ���ϻ��� �̵��
				if(adFileModel.SearchchkFileState_10.Trim().Length > 0 && adFileModel.SearchchkFileState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND ( A.FILE_STT = '10' \n");
					isState = true;
				}	
				// ���ϻ��� ���米ü���
				if(adFileModel.SearchchkFileState_11.Trim().Length > 0 && adFileModel.SearchchkFileState_11.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.FILE_STT  = '11' \n");
					isState = true;
				}	
				// ���ϻ��� �˼����
				if(adFileModel.SearchchkFileState_12.Trim().Length > 0 && adFileModel.SearchchkFileState_12.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.FILE_STT  = '12' \n");
					isState = true;
				}	
				// ���ϻ��� ������� (FTP���)
				if(adFileModel.SearchchkFileState_15.Trim().Length > 0 && adFileModel.SearchchkFileState_15.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.FILE_STT  = '15' \n");
					isState = true;
				}	
				// ���ϻ��� CDN����ȭ
				if(adFileModel.SearchchkFileState_20.Trim().Length > 0 && adFileModel.SearchchkFileState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.FILE_STT  = '20' \n");
					isState = true;
				}	
				// ���ϻ��� CDN����(CDN���)
				if(adFileModel.SearchchkFileState_30.Trim().Length > 0 && adFileModel.SearchchkFileState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.FILE_STT  = '30' \n");
					isState = true;
				}	
				// ���ϻ��� ��ž����
				if(adFileModel.SearchchkFileState_40.Trim().Length > 0 && adFileModel.SearchchkFileState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.FILE_STT  = '40' \n");
					isState = true;
				}	
				// ���ϻ��� ����
				if(adFileModel.SearchchkFileState_90.Trim().Length > 0 && adFileModel.SearchchkFileState_90.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.FILE_STT  = '90' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				sbQuery.Append(" ORDER BY A.ITEM_NO Desc \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �ش�𵨿� ����
				adFileModel.AdFileDataSet = ds.Copy();
				adFileModel.ResultCnt = Utility.GetDatasetCount(adFileModel.AdFileDataSet);
				adFileModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + adFileModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adFileModel.ResultCD = "3000";
				adFileModel.ResultDesc = "�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}

		/// <summary>
		/// ���ϰ˻� �����ȸ
		/// </summary>
		/// <param name="adFileModel"></param>
		public void GetAdFileSearch(HeaderModel header, AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileSearch() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey              :[" + adFileModel.SearchKey            + "]");		// �˻� ��
				_log.Debug("SearchMediaCode	       :[" + adFileModel.SearchMediaCode	    + "]");		// �˻� ��ü
				_log.Debug("SearchRapCode          :[" + adFileModel.SearchRapCode        + "]");		// �˻� ��
				_log.Debug("SearchAgencyCode       :[" + adFileModel.SearchAgencyCode     + "]");		// �˻� �����
				_log.Debug("SearchAdvertiserCode   :[" + adFileModel.SearchAdvertiserCode + "]");		// �˻� ������
				_log.Debug("SearchAdType           :[" + adFileModel.SearchAdType        + "]");		// �˻� ��������
				_log.Debug("SearchFileType         :[" + adFileModel.SearchFileType       + "]");		// �˻� ������
				_log.Debug("SearchchkAdState_10    :[" + adFileModel.SearchchkAdState_10  + "]");		// �˻� ������� : �غ�
				_log.Debug("SearchchkAdState_20    :[" + adFileModel.SearchchkAdState_20  + "]");		// �˻� ������� : ��
				_log.Debug("SearchchkAdState_30    :[" + adFileModel.SearchchkAdState_30  + "]");		// �˻� ������� : ����	
				_log.Debug("SearchchkAdState_40    :[" + adFileModel.SearchchkAdState_40  + "]");		// �˻� ������� : ����   
				_log.Debug("SearchchkFileState_10  :[" + adFileModel.SearchchkFileState_10  + "]");		// �˻� ���ϻ��� : �̵��
				_log.Debug("SearchchkFileState_11  :[" + adFileModel.SearchchkFileState_11  + "]");		// �˻� ���ϻ��� : ���米ü���
				_log.Debug("SearchchkFileState_12  :[" + adFileModel.SearchchkFileState_12  + "]");		// �˻� ���ϻ��� : ������
				_log.Debug("SearchchkFileState_15  :[" + adFileModel.SearchchkFileState_15  + "]");		// �˻� ���ϻ��� : �������
				_log.Debug("SearchchkFileState_20  :[" + adFileModel.SearchchkFileState_20  + "]");		// �˻� ���ϻ��� : ��������(FTP���)
				_log.Debug("SearchchkFileState_30  :[" + adFileModel.SearchchkFileState_30  + "]");		// �˻� ���ϻ��� : CDN����
				_log.Debug("SearchchkFileState_40  :[" + adFileModel.SearchchkFileState_40  + "]");		// �˻� ���ϻ��� : ��ž����
				_log.Debug("SearchchkFileState_90  :[" + adFileModel.SearchchkFileState_90  + "]");		// �˻� ���ϻ��� : ��ž����
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
                    + " SELECT A.ITEM_NO AS ItemNo             \n"
                    + "       ,A.ITEM_NM AS ItemName           \n"
                    + "       ,A.ADVT_TYP AS AdType           \n"
                    + "       ,J.STM_COD_NM AS AdTypeName  \n"
                    + "       ,A.ADVT_STT AS AdState            \n"
                    + "       ,B.STM_COD_NM AS AdStateName   \n"
                    + "       ,A.FILE_NM AS FileName           \n"
                    + "       ,A.FILE_TYP AS FileType           \n"
                    + "       ,C.STM_COD_NM AS FileTypeName  \n"
                    + "       ,A.FILE_STT AS FileState           \n"
                    + "       ,D.STM_COD_NM AS FileStateName \n"
                    + "	      ,A.FILE_LEN AS FileLength         \n"
                    + "       ,A.FILE_PATH AS FilePath           \n"
					+ "       ,'' AS DownLevel          \n"
					+ "       ,' ����' AS DownLevelName  \n"
                    + "       ,A.FILE_REG_DT AS FileRegDt               \n"
                    + "       ,A.FILE_REG_ID AS FileRegID               \n"
                    + "       ,E.USER_NM AS FileRegName \n"
                    + "       ,A.ADVT_TM AS AdTime                  \n"
                    + "       ,A.FILE_CHK_DT AS CheckDt   , F.USER_NM AS CheckName   \n"
                    + "       ,A.FILE_SYNC_DT AS CDNSyncDt , G.USER_NM AS CDNSyncName \n"
                    + "       ,A.FILE_PUB_DT AS CDNPubDt  , H.USER_NM AS CDNPubName  \n"
                    + "       ,A.FILE_DEL_DT AS STBDelDt  , I.USER_NM AS STBDelName  \n"
                    + "  FROM ADVT_MST A   \n"
                    + "       LEFT JOIN STM_COD B ON (A.ADVT_STT   = B.STM_COD and B.STM_COD_CLS = '25') \n" // 25:�������
                    + "       LEFT JOIN STM_COD C ON (A.FILE_TYP  = C.STM_COD and C.STM_COD_CLS = '24') \n" // 24:���ϱ���
                    + "       LEFT JOIN STM_COD D ON (A.FILE_STT = D.STM_COD and D.STM_COD_CLS = '31') \n" // 31:���ϻ���
                    + "       LEFT JOIN STM_USER E ON (A.FILE_REG_ID = E.USER_ID)    \n"
                    + "       LEFT JOIN STM_USER F ON (A.FILE_CHK_ID   = F.USER_ID)    \n"
                    + "       LEFT JOIN STM_USER G ON (A.FILE_SYNC_ID = G.USER_ID)    \n"
                    + "       LEFT JOIN STM_USER H ON (A.FILE_PUB_ID  = H.USER_ID)    \n"
                    + "       LEFT JOIN STM_USER I ON (A.FILE_DEL_ID  = I.USER_ID)    \n"
                    + "       LEFT JOIN STM_COD J ON (A.ADVT_TYP    = J.STM_COD and J.STM_COD_CLS = '26') \n" // 26:��������
					+ " WHERE 1 = 1  \n"
					);

				
				// �˻�� ������
				if (adFileModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
                        + "    A.ITEM_NM      LIKE '%" + adFileModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}		
				
				// ���ϻ��� �̵��

                sbQuery.Append(" AND ( A.FILE_STT  = '15' \n");
									
				sbQuery.Append(" OR ");
                sbQuery.Append(" A.FILE_STT  = '20' \n");	
				sbQuery.Append(" OR ");
                sbQuery.Append(" A.FILE_STT  = '30' \n");		
				sbQuery.Append(" ) \n");

                sbQuery.Append(" ORDER BY A.ITEM_NO Desc \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �ش�𵨿� ����
				adFileModel.AdFileDataSet = ds.Copy();
				// ���
				adFileModel.ResultCnt = Utility.GetDatasetCount(adFileModel.AdFileDataSet);
				// ����ڵ� ��Ʈ
				adFileModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + adFileModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileSearch() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adFileModel.ResultCD = "3000";
				adFileModel.ResultDesc = "�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		#region ���Ϲ��� ����� ��ȸ

		/// <summary>
		/// ���Ϲ��� ����� ��ȸ
		/// </summary>
		/// <param name="adFileModel"></param>
		public void GetPublishHistory(HeaderModel header, AdFileModel adFileModel)
		{
			//bool isState = false;
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishHistory() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("<�Է�����>");
				_log.Debug("MediaCode :[" + adFileModel.SearchMediaCode	+ "]");		// ��ü�ڵ�				
				_log.Debug("ItemNo     :[" + adFileModel.ItemNo			+ "]");		// ���ι�ȣ				

				OracleParameter[] sqlParams = new OracleParameter[1];

				sqlParams[0] = new OracleParameter(":ItemNo", OracleDbType.Int32);
				sqlParams[0].Value = Convert.ToInt32( adFileModel.ItemNo);

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n " + "SELECT  B.ITEM_SEQ  AS Seq");
				sbQuery.Append("\n " + "	,   B.ITEM_NO   AS ItemNo");
				sbQuery.Append("\n " + "    ,   C.ITEM_NM   AS ItemName");
				sbQuery.Append("\n " + "    ,   B.FILE_NM   AS FileName");
				sbQuery.Append("\n " + "    ,   B.FILE_OPER AS AddDel");
				sbQuery.Append("\n " + "    ,   TO_CHAR( B.DT_INSERT,'YYYY-MM-DD HH24:MI:SS') AS RegDt");
				sbQuery.Append("\n " + "    ,   D.USER_NM   AS RegName");
				sbQuery.Append("\n " + "  FROM FILEDIST_MST             A");
				sbQuery.Append("\n " + "       INNER JOIN FILEDIST_HST  B ON (A.ACK_NO    = B.ACK_NO)");
				sbQuery.Append("\n " + "       LEFT  JOIN ADVT_MST      C ON (B.ITEM_NO   = C.ITEM_NO)");
				sbQuery.Append("\n " + "       LEFT  JOIN STM_USER      D ON (B.ID_INSERT = D.USER_ID)");
				sbQuery.Append("\n " + " WHERE B.ITEM_NO = :ItemNo");
				sbQuery.Append("\n " + " ORDER BY B.ITEM_SEQ");
				
				_log.Debug(sbQuery.ToString());
				
				// ��������
				DataSet ds = new DataSet();
				_db.Open();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// ��� DataSet�� �ش�𵨿� ����
				adFileModel.HistoryDataSet = ds.Copy();
				ds.Dispose();

				// ���
				adFileModel.ResultCnt = Utility.GetDatasetCount(adFileModel.AdFileDataSet);
				adFileModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + adFileModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishHistory() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adFileModel.ResultCD = "3000";
				adFileModel.ResultDesc = "���Ϲ����̷� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}
		#endregion


		/// <summary>
		/// �������� ����
		/// </summary>
		/// <returns></returns>
		public void SetAdFileUpdate(HeaderModel header, AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int rc = 0;
				OracleParameter[] sqlParams = new OracleParameter[7];

				
				sqlParams[0] = new OracleParameter(":FileType", OracleDbType.Char, 2);
				sqlParams[1] = new OracleParameter(":FileLength", OracleDbType.Decimal);
				sqlParams[2] = new OracleParameter(":FilePath", OracleDbType.Varchar2, 100);
				sqlParams[3] = new OracleParameter(":PreFileName", OracleDbType.Varchar2, 50);
				sqlParams[4] = new OracleParameter(":FileName", OracleDbType.Varchar2, 50);
				sqlParams[5] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
				sqlParams[6] = new OracleParameter(":ItemNo", OracleDbType.Decimal);
								
				sqlParams[0].Value = adFileModel.FileType;
				sqlParams[1].Value = Convert.ToDecimal(adFileModel.FileLength);
				sqlParams[2].Value = adFileModel.FilePath;
				sqlParams[3].Value = adFileModel.PreFileName;
				sqlParams[4].Value = adFileModel.FileName;
				sqlParams[5].Value = header.UserID;
				sqlParams[6].Value = Convert.ToDecimal(adFileModel.ItemNo);
			
				// ��������
				try
				{
					_db.BeginTran();
					
					sbQuery.Append("\n " + "UPDATE	ADVT_MST");
					sbQuery.Append("\n " + "SET		FILE_STT	= '12'			");	// �˼�����
					sbQuery.Append("\n " + "	,	FILE_TYP	= :FileType		");
					sbQuery.Append("\n " + "    ,	FILE_LEN	= :FileLength	");
					sbQuery.Append("\n " + "    ,	FILE_PATH	= :FilePath		");
					sbQuery.Append("\n " + "    ,	FILE_NM_PRE	= :PreFileName	");
					sbQuery.Append("\n " + "    ,	FILE_NM		= :FileName		");
					sbQuery.Append("\n " + "    ,	FILE_REG_DT	= SYSDATE		");
					sbQuery.Append("\n " + "    ,	FILE_REG_ID	= :RegID		");
					sbQuery.Append("\n " + " WHERE ITEM_NO		= :ItemNo		");
					sbQuery.Append("\n " + "   AND (FILE_STT	= '10'			");	// �̵�ϰ�
					sbQuery.Append("\n " + "     OR FILE_STT	= '11')			");	// ���米ü��
					
					_log.Debug(sbQuery.ToString());
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// �Ϲ����� �����ε�, �̰� ����� ������� �ʴ´�.
					//if(rc == 0)	// ������Ʈ�� �ȵȰ��� �̵�ϰ��� �ƴϰ��̴�.
					//{
					//    // ���¸� �������� �ʴ´�.
					//    sbQuery = new StringBuilder();
					//    sbQuery.Append("\n " + "UPDATE	ADVT_MST");
					//    sbQuery.Append("\n " + "SET		FILE_TYP	= :FileType		");
					//    sbQuery.Append("\n " + "    ,	FILE_LEN	= :FileLength	");
					//    sbQuery.Append("\n " + "    ,	FILE_PATH	= :FilePath		");
					//    sbQuery.Append("\n " + "    ,	FILE_NM_PRE	= :PreFileName	");
					//    sbQuery.Append("\n " + "    ,	FILE_NM		= :FileName		");
					//    sbQuery.Append("\n " + "    ,	FILE_REG_DT	= SYSDATE		");
					//    sbQuery.Append("\n " + "    ,	FILE_REG_ID	= :RegID		");
					//    sbQuery.Append("\n " + " WHERE ITEM_NO		= :ItemNo		");

					//    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					//}
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�������� ���:["+adFileModel.ItemName + "]["+adFileModel.FileName + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileModel.ResultCD   = "3201";
				adFileModel.ResultDesc = "�������� ����� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		

		}


		public void SetFileUpdate(HeaderModel header, AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFileUpdate() Start");
				_log.Debug("-----------------------------------------");
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append(""
                    
                    
                    + "DECLARE 					 \n"
                    + "V_ITEM_NO NUMBER;					 \n"
                    + "BEGIN					 \n"
                    + "V_ITEM_NO := :newItemNo;					 \n"
                    + "UPDATE ADVT_MST							 \n"
                    + "   SET FILE_STT  = (SELECT FILE_STT FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)	    \n"
                    + "      ,FILE_TYP  = (SELECT FILE_TYP FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)	    \n"
                    + "      ,FILE_LEN  = (SELECT FILE_LEN FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)	\n"
                    + "      ,FILE_PATH  = (SELECT FILE_PATH FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)	    \n"
                    + "      ,FILE_NM_PRE  = (SELECT FILE_NM_PRE FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)	    \n"
                    + "      ,FILE_NM  =     (SELECT FILE_NM FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)	    \n"
                    + "      ,FILE_REG_DT  = (SELECT FILE_REG_DT FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)	    \n"
                    + "      ,FILE_REG_ID  = (SELECT FILE_REG_ID FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)	    \n"
                    + "      ,FILE_CHK_DT  = (SELECT FILE_CHK_DT FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)	        \n"
                    + "      ,FILE_CHK_ID  = (SELECT FILE_CHK_ID FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)			\n"
                    + "      ,FILE_SYNC_DT  = (SELECT FILE_SYNC_DT FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)		\n"
                    + "      ,FILE_SYNC_ID  = (SELECT FILE_SYNC_ID FROM ADVT_MST WHERE ITEM_NO = V_ITEM_NO)		\n"
                    + " WHERE ITEM_NO        = :ItemNo;        \n"
                    + " END;        \n"						
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":newItemNo", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
						
				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(adFileModel.newItemNo);
				sqlParams[i++].Value = Convert.ToInt32(adFileModel.ItemNo);				
				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�������ã��� ���ϵ��:[����: "+adFileModel.newItemNo + "] [���: " + adFileModel.ItemNo + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFileUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileModel.ResultCD   = "3201";
				adFileModel.ResultDesc = "������� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}
		
		/// <summary>
		/// FTP���ε� ������ȸ
		/// </summary>
		/// <returns></returns>
		public void GetFtpConfig(HeaderModel header, AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFtpConfig() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();
				sbQuery.Append("\n " + "SELECT	FTP_ULD_ID		AS FtpUploadID		");
				sbQuery.Append("\n " + "	,	FTP_ULD_PWD		AS FtpUploadPW		");
				sbQuery.Append("\n " + "	,	FTP_ULD_HOST	AS FtpUploadHost	");
				sbQuery.Append("\n " + "	,	FTP_ULD_PORT	AS FtpUploadPort	");
				sbQuery.Append("\n " + "	,	FTP_ULD_PATH	AS FtpUploadPath	");
				sbQuery.Append("\n " + "	,	FTP_MV_PATH		AS FtpMovePath		");
				sbQuery.Append("\n " + "	,	FTP_MV_YN		AS FtpMoveUseYn		");
				sbQuery.Append("\n " + "	,	FTP_CDN_ID		AS FtpCdnID			");
				sbQuery.Append("\n " + "	,	FTP_CDN_PWD		AS FtpCdnPW			");
				sbQuery.Append("\n " + "	,	FTP_CDN_HOST	AS FtpCdnHost		");
				sbQuery.Append("\n " + "	,	FTP_CDN_PORT	AS FtpCdnPort		");
				sbQuery.Append("\n " + "	,	''				AS CmsMasUrl		");
				sbQuery.Append("\n " + "    ,	''				AS CmsMasQuery		");
				sbQuery.Append("\n " + " FROM STM_PROP\n");
				_log.Debug(sbQuery.ToString());
				
				// ��������
				DataSet ds = new DataSet();
				_db.Open();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				adFileModel.FtpUploadID   = Utility.GetDatasetString(ds, 0, "FtpUploadID");
				adFileModel.FtpUploadPW   = Utility.GetDatasetString(ds, 0, "FtpUploadPW");
				adFileModel.FtpUploadHost = Utility.GetDatasetString(ds, 0, "FtpUploadHost");
				adFileModel.FtpUploadPort = Utility.GetDatasetString(ds, 0, "FtpUploadPort");
				adFileModel.FtpUploadPath = Utility.GetDatasetString(ds, 0, "FtpUploadPath");
				adFileModel.FtpMovePath   = Utility.GetDatasetString(ds, 0, "FtpMovePath");
				adFileModel.FtpMoveUseYn  = Utility.GetDatasetString(ds, 0, "FtpMoveUseYn");
				adFileModel.FtpCdnID      = Utility.GetDatasetString(ds, 0, "FtpCdnID");
				adFileModel.FtpCdnPW      = Utility.GetDatasetString(ds, 0, "FtpCdnPW");
				adFileModel.FtpCdnHost    = Utility.GetDatasetString(ds, 0, "FtpCdnHost");
				adFileModel.FtpCdnPort    = Utility.GetDatasetString(ds, 0, "FtpCdnPort");
				adFileModel.CmsMasUrl		= Utility.GetDatasetString(ds, 0, "CmsMasUrl");
				adFileModel.CmsMasQuery		= Utility.GetDatasetString(ds, 0, "CmsMasQuery");
				adFileModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFtpConfig() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adFileModel.ResultCD   = "3001";
				adFileModel.ResultDesc = "FTP���ε� ���� ��ȸ �� ������ �߻��Ͽ����ϴ�, " + ex.Message;
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}

		/// <summary>
		/// �������� ��ü 
		/// </summary>
		/// <param name="header"></param>
		/// <param name="adFileModel"></param>
		public void GetFileRepHistory(HeaderModel header, AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFileRepHistory() Start");
				_log.Debug("-----------------------------------------");

				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
				OracleParameter[] sqlParams = new OracleParameter[1];

				sqlParams[0] = new OracleParameter(":ItemNo" , OracleDbType.Int32);
				sqlParams[0].Value = Convert.ToInt32(adFileModel.ItemNo);
	
				try
				{
					sbQuery.Append("\n SELECT	HST_SEQ		AS HistoryNum	");
					sbQuery.Append("\n		,	REP_DT		AS RepDt		");
					sbQuery.Append("\n      ,	C.USER_NM	AS RepID		");
					sbQuery.Append("\n      ,	B.STM_COD_NM AS FileState	");
					sbQuery.Append("\n      ,	FILE_LEN	AS FileLength	");
					sbQuery.Append("\n      ,	FILE_PATH	AS FilePath		");
					sbQuery.Append("\n      ,	FILE_NM		AS FileName		");
					sbQuery.Append("\n 		,	FILE_NM_PRE	AS PreFileName	");
					sbQuery.Append("\n      ,	FILE_REG_DT AS FileRegDt, D.USER_NM		AS FileRegID");
					sbQuery.Append("\n      ,	FILE_CHK_DT AS CheckDt	, E.USER_NM		AS CheckName");
					sbQuery.Append("\n      ,	FILE_PUB_DT AS CDNPubDt	, F.USER_NM		AS CDNPubName");
					sbQuery.Append("\n      ,	FILE_DEL_DT AS STBDelDt	, G.USER_NM		AS STBDelName");
					sbQuery.Append("\n FROM	ADVT_FILEHST A");
					sbQuery.Append("\n LEFT OUTER JOIN STM_COD	B ON (a.FILE_STT	= B.STM_COD AND B.STM_COD_CLS = '31')");
					sbQuery.Append("\n LEFT OUTER JOIN STM_USER	C ON (a.REP_ID		= C.USER_ID)");
					sbQuery.Append("\n LEFT OUTER JOIN STM_USER	D ON (a.FILE_REG_ID	= D.USER_ID)");
					sbQuery.Append("\n LEFT OUTER JOIN STM_USER	E ON (a.FILE_CHK_ID = E.USER_ID)");
					sbQuery.Append("\n LEFT OUTER JOIN STM_USER	F ON (a.FILE_PUB_ID	= F.USER_ID)");
					sbQuery.Append("\n LEFT OUTER JOIN STM_USER	G ON (a.FILE_DEL_ID = G.USER_ID)");
					sbQuery.Append("\n WHERE ITEM_NO = :ItemNo");

					_log.Debug(sbQuery.ToString());
					DataSet ds = new DataSet();
					_db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

					adFileModel.AdFileDataSet = ds.Copy();
					adFileModel.ResultCnt = Utility.GetDatasetCount(adFileModel.AdFileDataSet);
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
				adFileModel.ResultCD = "0000";  // ����
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFileRepHistory() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adFileModel.ResultCD   = "3201";
				adFileModel.ResultDesc = "����ü�̷� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}
	}
}