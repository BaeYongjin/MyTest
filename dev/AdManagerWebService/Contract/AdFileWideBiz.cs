/*
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�	: [E_01]
 * ������		: YJ.Park
 * ������		: 2014.11.13
 * ��������	: Ȩ���� �� Count�Ҷ� Ȩ����(Ű��) �߰�
 * --------------------------------------------------------
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Contract
{
    /// <summary>
    /// AdFileWideBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class AdFileWideBiz : BaseBiz
    {
        public AdFileWideBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }

		/// <summary>
		/// �������ϰǼ� ��ȸ
		/// </summary>
		/// <param name="adFileWideModel"></param>
		public void GetFileCount(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileWideList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey              :[" + adFileWideModel.SearchKey      + "]");				// �˻���
				_log.Debug("SearchMediaCode        :[" + adFileWideModel.SearchMediaCode      + "]");		// �˻� ��ü
				_log.Debug("SearchchkAdState_10    :[" + adFileWideModel.SearchchkAdState_10  + "]");		// �˻� ������� : �غ�
				_log.Debug("SearchchkAdState_20    :[" + adFileWideModel.SearchchkAdState_20  + "]");		// �˻� ������� : ��
				_log.Debug("SearchchkAdState_30    :[" + adFileWideModel.SearchchkAdState_30  + "]");		// �˻� ������� : ����	
				_log.Debug("SearchchkAdState_40    :[" + adFileWideModel.SearchchkAdState_40  + "]");		// �˻� ������� : ����   
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n"
                    + " SELECT A.STM_COD AS FileState, A.STM_COD_NM AS FileStateName, NVL(B.FileCount,0) AS FileCount \n"
                    + "  FROM STM_COD A LEFT JOIN             \n"
                    + "      (SELECT FILE_STT AS FileState, COUNT(*) FileCount             \n"
                    + "         FROM ADVT_MST                  \n"
					+ "        WHERE 1 = 1                                     \n"
					);           

				
				// ������� ���ÿ� ����

				// ������� �غ�
				if(adFileWideModel.SearchchkAdState_10.Trim().Length > 0 && adFileWideModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append("          AND ( ADVT_STT  = '10' \n");
					isState = true;
				}	
				// ������� ��
				if(adFileWideModel.SearchchkAdState_20.Trim().Length > 0 && adFileWideModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          ADVT_STT  = '20' \n");
					isState = true;
				}	
				// ������� ����
				if(adFileWideModel.SearchchkAdState_30.Trim().Length > 0 && adFileWideModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          ADVT_STT  = '30' \n");
					isState = true;
				}	
				// ������� ����
				if(adFileWideModel.SearchchkAdState_40.Trim().Length > 0 && adFileWideModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          ADVT_STT  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				// �˻�� ������
				if (adFileWideModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
                        + "    FILE_NM    LIKE '%" + adFileWideModel.SearchKey.Trim() + "%' \n"
                        + " OR ITEM_NM    LIKE '%" + adFileWideModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}

				sbQuery.Append(""
                    + " GROUP BY FILE_STT) B ON (A.STM_COD = B.FileState) \n"
                    + " WHERE A.STM_COD_CLS = '31'  -- ���ϻ���                    \n"
                    + "   AND A.STM_COD <> '0000'  -- �����̾ƴѰ�                \n"
                    + "ORDER BY A.STM_COD  \n" 
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �ش�𵨿� ����
				adFileWideModel.CountDataSet = ds.Copy();
				ds.Dispose();


				// ���
				adFileWideModel.ResultCnt = Utility.GetDatasetCount(adFileWideModel.CountDataSet);
				// ����ڵ� ��Ʈ
				adFileWideModel.ResultCD = "0000";


				// 2007.10.01 RH.Jung ���ϸ���Ʈ �Ǽ��˻��
				// 2007.10.10 RH.Jung Ȩ���� ����Ʈ �ջ�� ���ϻ��°� ��ž������ �ƴѰ����� ����

				sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n"
					+ "  SELECT COUNT(*) AS FileCnt			\n"
                    + "  FROM	ADVT_MST	\n"
                    + "  WHERE	FILE_STT = '30'			\n"
					);           

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				adFileWideModel.FileListCount = Convert.ToInt32(ds.Tables[0].Rows[0]["FileCnt"].ToString());
				ds.Dispose();

				// 2007.10.01 

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + adFileWideModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileWideList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD = "3000";
				adFileWideModel.ResultDesc = "�������ϰǼ� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


        /// <summary>
        /// �������� �����ȸ
        /// </summary>
        /// <param name="adFileWideModel"></param>
        public void GetAdFileWideList(HeaderModel header, AdFileWideModel adFileWideModel)
        {
			bool isState = false;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdFileWideList() Start");
                _log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchKey              :[" + adFileWideModel.SearchKey      + "]");				// �˻���
				_log.Debug("SearchMediaCode	       :[" + adFileWideModel.SearchMediaCode  + "]");			// �˻� ��ü
				_log.Debug("SearchFileState        :[" + adFileWideModel.SearchFileState  + "]");			// �˻� ���ϻ��� 
				_log.Debug("SearchchkAdState_10    :[" + adFileWideModel.SearchchkAdState_10  + "]");		// �˻� ������� : �غ�
				_log.Debug("SearchchkAdState_20    :[" + adFileWideModel.SearchchkAdState_20  + "]");		// �˻� ������� : ��
				_log.Debug("SearchchkAdState_30    :[" + adFileWideModel.SearchchkAdState_30  + "]");		// �˻� ������� : ����	
				_log.Debug("SearchchkAdState_40    :[" + adFileWideModel.SearchchkAdState_40  + "]");		// �˻� ������� : ����   
				// __DEBUG__

                StringBuilder sbQuery = new StringBuilder();				
				
                // ��������
                sbQuery.Append("\n"
                    + " SELECT 'False' AS CheckYn		\n"
                    + "       ,A.ITEM_NO AS ItemNo             \n"
                    + "       ,A.ITEM_NM AS ItemName           \n"
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
                    + "       ,A.FILE_CHK_DT AS CheckDt   , F.USER_NM AS CheckName   \n"
                    + "       ,A.FILE_SYNC_DT AS CDNSyncDt , G.USER_NM AS CDNSyncName \n"
                    + "       ,A.FILE_PUB_DT AS CDNPubDt  , H.USER_NM AS CDNPubName  \n"
                    + "       ,A.FILE_DEL_DT AS STBDelDt  , I.USER_NM AS STBDelName  \n"
                    + "  FROM ADVT_MST A   \n"
                    + "       LEFT JOIN STM_COD B  ON (A.ADVT_STT   = B.STM_COD and B.STM_COD_CLS = '25') \n" // 25:�������
                    + "       LEFT JOIN STM_COD C  ON (A.FILE_TYP  = C.STM_COD and C.STM_COD_CLS = '24') \n" // 24:���ϱ���
                    + "       LEFT JOIN STM_COD D  ON (A.FILE_STT = D.STM_COD and D.STM_COD_CLS = '31') \n" // 31:���ϻ���
                    + "       LEFT JOIN STM_USER E ON (A.FILE_REG_ID = E.USER_ID)         \n"
                    + "       LEFT JOIN STM_USER F ON (A.FILE_CHK_ID   = F.USER_ID)    \n"
                    + "       LEFT JOIN STM_USER G ON (A.FILE_SYNC_ID = G.USER_ID)    \n"
                    + "       LEFT JOIN STM_USER H ON (A.FILE_PUB_ID  = H.USER_ID)    \n"
                    + "       LEFT JOIN STM_USER I ON (A.FILE_DEL_ID  = I.USER_ID)    \n"
					+ " WHERE 1=1  \n"
                    + "   AND A.FILE_STT  = '" + adFileWideModel.SearchFileState + "' \n"
					);

				// ������� ���ÿ� ����

				// ������� �غ�
				if(adFileWideModel.SearchchkAdState_10.Trim().Length > 0 && adFileWideModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append("          AND ( A.ADVT_STT  = '10' \n");
					isState = true;
				}	
				// ������� ��
				if(adFileWideModel.SearchchkAdState_20.Trim().Length > 0 && adFileWideModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          A.ADVT_STT  = '20' \n");
					isState = true;
				}	
				// ������� ����
				if(adFileWideModel.SearchchkAdState_30.Trim().Length > 0 && adFileWideModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          A.ADVT_STT  = '30' \n");
					isState = true;
				}	
				// ������� ����
				if(adFileWideModel.SearchchkAdState_40.Trim().Length > 0 && adFileWideModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append("          A.ADVT_STT  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				// �˻�� ������
				if (adFileWideModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
                        + "    A.FILE_NM    LIKE '%" + adFileWideModel.SearchKey.Trim() + "%' \n"
                        + " OR A.ITEM_NM    LIKE '%" + adFileWideModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}

                sbQuery.Append(" ORDER BY A.ITEM_NO DESC \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // ��� DataSet�� �ش�𵨿� ����
                adFileWideModel.FileDataSet = ds.Copy();

				ds.Dispose();

                // ���
                adFileWideModel.ResultCnt = Utility.GetDatasetCount(adFileWideModel.FileDataSet);
                // ����ڵ� ��Ʈ
                adFileWideModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + adFileWideModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdFileWideList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                adFileWideModel.ResultCD = "3000";
                adFileWideModel.ResultDesc = "�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// �ش籤���� ����Ȳ ��ȸ
		/// </summary>
		/// <param name="adStatusModel"></param>
		public void GetAdFileSchedule(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			//bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileSchedule() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("ItemNo                 :[" + adFileWideModel.ItemNo            + "]");		// �����ȣ
				_log.Debug("SearchMediaCode	       :[" + adFileWideModel.SearchMediaCode      + "]");		// �˻� ��ü				

				// __DEBUG__

				SqlParameter[] sqlParams = new SqlParameter[2];
		    
				int i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"  , SqlDbType.Int );
				sqlParams[i++] = new SqlParameter("@ItemNo"     , SqlDbType.Int );

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32( adFileWideModel.SearchMediaCode);
				sqlParams[i++].Value = Convert.ToInt32( adFileWideModel.ItemNo);


				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n																	");
				sbQuery.Append("\n  SELECT  TA.MediaCode											");
				sbQuery.Append("\n         ,TA.ORD													"); 
				sbQuery.Append("\n         ,TA.Flag													"); 
				sbQuery.Append("\n         ,TA.CategoryCode											");
				sbQuery.Append("\n         ,ISNULL(TG.MenuName,'') AS CategoryName					");
				sbQuery.Append("\n         ,TA.GenreCode											");
				sbQuery.Append("\n         ,ISNULL(TH.MenuName,'') AS GenreName						");
				sbQuery.Append("\n         ,TA.ChannelNo											");
				sbQuery.Append("\n         ,TA.Title												");
				sbQuery.Append("\n         ,TA.ItemNo												");
				sbQuery.Append("\n         ,TA.ItemName												");
				sbQuery.Append("\n         ,TB.CodeName AS AdStateName								");
				sbQuery.Append("\n         ,TA.FileName												");
				sbQuery.Append("\n         ,TA.FileType												");
				sbQuery.Append("\n         ,TC.CodeName AS FileTypeName								");
				sbQuery.Append("\n         ,TA.FileState											");
				sbQuery.Append("\n         ,TD.CodeName AS FileStateName							");
				sbQuery.Append("\n         ,TA.FileLength											");
				sbQuery.Append("\n         ,TA.FilePath												");
				sbQuery.Append("\n         ,'' AS DownLevel											");
				sbQuery.Append("\n         ,' ����' AS DownLevelName		");
				sbQuery.Append("\n         ,TA.ScheduleOrder										");
				sbQuery.Append("\n         ,TA.AdType												");
				sbQuery.Append("\n         ,TE.CodeName AS AdTypeName								");
				sbQuery.Append("\n         ,TA.AckNo												");
				sbQuery.Append("\n         ,TF.State AS AckState									");
				sbQuery.Append("\n   FROM															");
				sbQuery.Append("\n (																");
				sbQuery.Append("\n   SELECT B.MediaCode												");	
				sbQuery.Append("\n         ,'0' AS ORD												");
				sbQuery.Append("\n         ,'Ȩ' AS Flag											");
				sbQuery.Append("\n         ,0 AS CategoryCode										");
				sbQuery.Append("\n         ,0 AS GenreCode											");
				sbQuery.Append("\n         ,0 AS ChannelNo											");	
				sbQuery.Append("\n         ,'' AS Title												");	
				sbQuery.Append("\n         ,A.ItemNo												");
				sbQuery.Append("\n         ,A.ScheduleOrder											");	
				sbQuery.Append("\n         ,A.AckNo													");
				sbQuery.Append("\n         ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType			");
				sbQuery.Append("\n     FROM SchHome A with(NoLock) INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo AND B.MediaCode = @MediaCode)	");
				sbQuery.Append("\n    WHERE A.ItemNo = @ItemNo										");
				
				//[E_01]
				sbQuery.Append("\n																	");
				sbQuery.Append("\n  UNION															");
				sbQuery.Append("\n																	");
				sbQuery.Append("\n  SELECT B.MediaCode												");
				sbQuery.Append("\n        ,'0' AS ORD												");
				sbQuery.Append("\n        ,'Ȩ(Ű��)' AS Flag										");
				sbQuery.Append("\n        ,0 AS CategoryCode										");
				sbQuery.Append("\n        ,0 AS GenreCode											");
				sbQuery.Append("\n        ,0 AS ChannelNo											");
				sbQuery.Append("\n        ,'' AS Title												");
				sbQuery.Append("\n        ,A.ItemNo													");
				sbQuery.Append("\n        ,A.ScheduleOrder											");
				sbQuery.Append("\n        ,A.AckNo													");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType					");
				sbQuery.Append("\n    FROM SchHomeKids A with(NoLock) INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo AND B.MediaCode = @MediaCode)	");
				sbQuery.Append("\n   WHERE A.ItemNo = @ItemNo										");
				sbQuery.Append("\n																	");

				sbQuery.Append("\n  UNION															");	
				sbQuery.Append("\n																	");
				sbQuery.Append("\n  SELECT B.MediaCode												");	
				sbQuery.Append("\n        ,'1' AS ORD												");
				sbQuery.Append("\n        ,'�޴�' AS Flag											");	
				sbQuery.Append("\n        ,C.UpperMenuCode AS CategoryCode							");
				sbQuery.Append("\n        ,A.GenreCode												");	
				sbQuery.Append("\n        ,0 AS ChannelNo											");	
				sbQuery.Append("\n        ,'' AS Title												");	 
				sbQuery.Append("\n        ,A.ItemNo													");
				sbQuery.Append("\n        ,A.ScheduleOrder											");	
				sbQuery.Append("\n        ,A.AckNo													");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n   FROM SchChoiceMenuDetail     A with(NoLock)																		");
				sbQuery.Append("\n        INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode)			");
				sbQuery.Append("\n        INNER JOIN Menu         C with(NoLock) ON (A.GenreCode = C.MenuCode  AND C.MediaCode = @MediaCode)			");
				sbQuery.Append("\n   WHERE A.ItemNo = @ItemNo										");
				sbQuery.Append("\n																	");
				sbQuery.Append("\n   UNION															"); 
				sbQuery.Append("\n																	");
				sbQuery.Append("\n SELECT B.MediaCode												");
				sbQuery.Append("\n       ,'2' AS ORD												");
				sbQuery.Append("\n       ,'ä��' AS Flag											");
				sbQuery.Append("\n       ,C.Category AS CatagoryCode								");
				sbQuery.Append("\n       ,C.Genre    AS GenreCode									");
				sbQuery.Append("\n       ,A.ChannelNo												");
				sbQuery.Append("\n       ,C.ProgramNm as Title										");
				sbQuery.Append("\n       ,A.ItemNo													");
				sbQuery.Append("\n       ,A.ScheduleOrder											");
				sbQuery.Append("\n       ,A.AckNo													");
				sbQuery.Append("\n       ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType							");
				sbQuery.Append("\n   FROM SchChoiceChannelDetail  A with(NoLock)																							");
				sbQuery.Append("\n        INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode)								");
				sbQuery.Append("\n        INNER JOIN Program      C with(NoLock) ON (A.ChannelNo = C.Channel   AND C.MediaCode = @MediaCode)								");
				sbQuery.Append("\n   WHERE A.ItemNo = @ItemNo																												");
				sbQuery.Append("\n																																			");
				sbQuery.Append("\n ) TA   LEFT JOIN SystemCode TB with(NoLock) ON (TA.AdState      = TB.Code     AND TB.Section = '25') -- �������							");
				sbQuery.Append("\n        LEFT JOIN SystemCode TC with(NoLock) ON (TA.FileType     = TC.Code     AND TC.Section = '24') -- ���ϱ���							");
				sbQuery.Append("\n        LEFT JOIN SystemCode TD with(NoLock) ON (TA.FileState    = TD.Code     AND TD.Section = '31') -- ���ϻ���							");  
				sbQuery.Append("\n        LEFT JOIN SystemCode TE with(NoLock) ON (TA.AdType       = TE.Code     AND TE.Section = '26') -- ��������							");  
				sbQuery.Append("\n        LEFT JOIN SchPublish TF with(NoLock) ON (TA.AckNo        = TF.AckNo    AND TF.MediaCode = @MediaCode)  -- ���λ���				");
				sbQuery.Append("\n        LEFT JOIN Menu       TG with(NoLock) ON (TA.CategoryCode = TG.MenuCode AND TG.MediaCode = @MediaCode)  -- ī�װ���				");
				sbQuery.Append("\n        LEFT JOIN Menu       TH with(NoLock) ON (TA.GenreCode    = TH.MenuCode AND TH.MediaCode = @MediaCode)  -- �帣��					");

				sbQuery.Append(" ORDER BY ORD, CategoryCode, GenreCode, ChannelNo, ScheduleOrder, FileName \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// ��� DataSet�� �ش�𵨿� ����
				adFileWideModel.ScheduleDataSet = ds.Copy();
				// ���
				adFileWideModel.ResultCnt = Utility.GetDatasetCount(adFileWideModel.ScheduleDataSet);
				// ����ڵ� ��Ʈ
				adFileWideModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + adFileWideModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdFileSchedule() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD = "3000";
				adFileWideModel.ResultDesc = "[����Ȳ��ȸ]" + ex.Message;
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// �������� �˼���û(�̵��->�˼����)
		/// </summary>
		/// <returns></returns>
		public void SetAdFileChkRequest(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkRequest() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '12'         \n"	// ���ϻ��� 12:�˼����
                    + "      ,FILE_REG_DT   = SYSDATE    \n"
                    + "      ,FILE_REG_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;			
				sqlParams[i++].Value = header.UserID;                   // �����
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	
				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�������� �˼���û:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkRequest() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "�������� �˼���û �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// �������� �˼���û ���
		/// </summary>
		/// <returns></returns>
		public void SetAdFileChkRequestCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkRequestCancel() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery1 = new StringBuilder();
				int rc = 0;
                OracleParameter[] sqlParams1 = new OracleParameter[2];

                sbQuery1.Append("\n UPDATE ADVT_MST ");
                sbQuery1.Append("\n SET	 FILE_STT   = '10'			");
                sbQuery1.Append("\n		,FILE_REG_DT   = SYSDATE	");
                sbQuery1.Append("\n      ,FILE_REG_ID   = :RegID		");
                sbQuery1.Append("\n WHERE ITEM_NO      = :ItemNo		");


                sqlParams1[0] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams1[1] = new OracleParameter(":ItemNo", OracleDbType.Int32);
						
				sqlParams1[0].Value = header.UserID;
                sqlParams1[1].Value = Convert.ToInt32(adFileWideModel.ItemNo);		
				_log.Debug(sbQuery1.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery1.ToString(), sqlParams1);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�������� �˼���û���:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkRequestCancel() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "�������� �˼���û ��� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}



		/// <summary>
		/// �������� �˼��Ϸ� (�˼����->�������)
		/// 2010/07/15 �˼����(12) -> CDN����ȭ(20): ���������� ������û
		/// �������(15)�� ���� �۾��� �߻����� ������, CMS�� ����ȭ ��û�ϰ� �Ϸ�ó����
		/// �񵿱������� �߻��ϱ� ������ �߰��ܰ踦 ����ؾ� �Ѵ�.
		/// </summary>
		/// <returns></returns>
		public void SetAdFileChkComplete(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkComplete() Start");
				_log.Debug("-----------------------------------------");

				_db.Open();

				#region ���ϻ��� ���� ���� ����
				StringBuilder	sbQuery1	= new StringBuilder();
                OracleParameter[] sqlParams1 = new OracleParameter[2];
				int rc1 = 0;

                sbQuery1.Append("\n UPDATE ADVT_MST			");
                sbQuery1.Append("\n SET   FILE_STT	= '15'		");
                sbQuery1.Append("\n		,FILE_CHK_DT	= SYSDATE	");
                sbQuery1.Append("\n      ,FILE_CHK_ID	= :RegID	");
                sbQuery1.Append("\n WHERE ITEM_NO		= :ItemNo	");


                sqlParams1[0] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams1[1] = new OracleParameter(":ItemNo", OracleDbType.Int32);

                sqlParams1[0].Value = header.UserID;
				sqlParams1[1].Value = Convert.ToInt32(adFileWideModel.ItemNo);				
				                   // �����
				_log.Debug(sbQuery1.ToString());
				#endregion

				#region CMS�۾����� ���� ����
				/*int rc2 = 0;
				StringBuilder sbQuery2 = new StringBuilder();
				SqlParameter[] sqlParams2 = new SqlParameter[7];

				sbQuery2.Append("\n INSERT INTO dbo.AdFileDistribute ");
				sbQuery2.Append("\n            ([ItemNo] ");
				sbQuery2.Append("\n            ,[WorkDt] ");
				sbQuery2.Append("\n            ,[WorkId] ");
				sbQuery2.Append("\n            ,[FilePath] ");
				sbQuery2.Append("\n            ,[FileName] ");
				sbQuery2.Append("\n            ,[cId] ");
				sbQuery2.Append("\n            ,[cmd] ");
				sbQuery2.Append("\n            ,[RequestStatus] ");
				sbQuery2.Append("\n            ,[ProcStatus],[SyncServer],[DescServer]) "); // ������� ó����
				sbQuery2.Append("\n      VALUES ");
				sbQuery2.Append("\n            (@ItemNo ");
				sbQuery2.Append("\n            ,GetDate() ");
				sbQuery2.Append("\n            ,@WorkId ");
				sbQuery2.Append("\n            ,@FilePath ");
				sbQuery2.Append("\n            ,@FileName ");
				sbQuery2.Append("\n            ,@cId ");
				sbQuery2.Append("\n            ,@cmd ");
				sbQuery2.Append("\n            ,@RequestStatus ");
				sbQuery2.Append("\n            ,0,0,0) ");
				
				sqlParams2[0] = new SqlParameter("@ItemNo"		, SqlDbType.Int          );
				sqlParams2[1] = new SqlParameter("@WorkId"		, SqlDbType.VarChar ,  10);
				sqlParams2[2] = new SqlParameter("@FilePath"    , SqlDbType.VarChar ,  100);
				sqlParams2[3] = new SqlParameter("@FileName"    , SqlDbType.VarChar ,  50);
				sqlParams2[4] = new SqlParameter("@cId"			, SqlDbType.VarChar ,  40);
				sqlParams2[5] = new SqlParameter("@cmd"			, SqlDbType.VarChar ,  10);
				sqlParams2[6] = new SqlParameter("@RequestStatus",SqlDbType.VarChar ,  1);
		
				sqlParams2[0].Value	= Convert.ToInt32(adFileWideModel.ItemNo);	
				sqlParams2[1].Value	= header.UserID;                   
				sqlParams2[2].Value	= adFileWideModel.FilePath.Trim();
				sqlParams2[3].Value	= adFileWideModel.FileName.Trim();
				sqlParams2[4].Value	= adFileWideModel.CmsCid.Trim();
				sqlParams2[5].Value	= adFileWideModel.CmsCmd.Trim();
				sqlParams2[6].Value	= adFileWideModel.CmsRequestStatus.Trim();
				_log.Debug(sbQuery2.ToString());*/
				#endregion

				// ��������
				try
				{
					_db.BeginTran();
					rc1 =  _db.ExecuteNonQueryParams(sbQuery1.ToString(), sqlParams1);
                    /*
					if ( !adFileWideModel.CmsCid.Trim().Equals("0000") )
					{
						rc2 =  _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams2);
					}
                    */
					_db.CommitTran();
					_log.Message("�������� �˼��Ϸ�:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkComplete() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "�������� �˼��Ϸ� ó�� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// CMS���� ȣ��ó��
		/// </summary>
		/// <param name="header"></param>
		/// <param name="adFileWideModel"></param>
		public void SetCmsRequestBegin(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCmsRequestBegin() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();

				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[7];

				sbQuery.Append("\n INSERT INTO dbo.AdFileDistribute ");
				sbQuery.Append("\n            ([ItemNo] ");
				sbQuery.Append("\n            ,[WorkDt] ");
				sbQuery.Append("\n            ,[WorkId] ");
				sbQuery.Append("\n            ,[FilePath] ");
				sbQuery.Append("\n            ,[FileName] ");
				sbQuery.Append("\n            ,[cId] ");
				sbQuery.Append("\n            ,[cmd] ");
				sbQuery.Append("\n            ,[RequestStatus] ");
				sbQuery.Append("\n            ,[ProcStatus],[SyncServer],[DescServer]) "); // ������� ó����
				sbQuery.Append("\n      VALUES ");
				sbQuery.Append("\n            (@ItemNo ");
				sbQuery.Append("\n            ,GetDate() ");
				sbQuery.Append("\n            ,@WorkId ");
				sbQuery.Append("\n            ,@FilePath ");
				sbQuery.Append("\n            ,@FileName ");
				sbQuery.Append("\n            ,@cId ");
				sbQuery.Append("\n            ,@cmd ");
				sbQuery.Append("\n            ,@RequestStatus ");
				sbQuery.Append("\n            ,0,0,0) ");
				
				sqlParams[0] = new SqlParameter("@ItemNo"		, SqlDbType.Int          );
				sqlParams[1] = new SqlParameter("@WorkId"		, SqlDbType.VarChar ,  10);
				sqlParams[2] = new SqlParameter("@FilePath"     , SqlDbType.VarChar ,  100);
				sqlParams[3] = new SqlParameter("@FileName"     , SqlDbType.VarChar ,  50);
				sqlParams[4] = new SqlParameter("@cId"			, SqlDbType.VarChar ,  20);
				sqlParams[5] = new SqlParameter("@cmd"			, SqlDbType.VarChar ,  10);
				sqlParams[6] = new SqlParameter("@RequestStatus", SqlDbType.VarChar ,  1);
		
				sqlParams[0].Value	= Convert.ToInt32(adFileWideModel.ItemNo);	
				sqlParams[1].Value	= header.UserID;                   
				sqlParams[2].Value	= adFileWideModel.FilePath.Trim();
				sqlParams[3].Value	= adFileWideModel.FileName.Trim();
				sqlParams[4].Value	= adFileWideModel.CmsCid.Trim();
				sqlParams[5].Value	= adFileWideModel.CmsCmd.Trim();
				sqlParams[6].Value	= adFileWideModel.CmsRequestStatus.Trim();
				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("�������� CMS���� :["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCmsRequestBegin() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "�������� �˼��Ϸ� ó�� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// �������� �˼��Ϸ� ���
		/// </summary>
		/// <returns></returns>
		public void SetAdFileChkCompleteCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkCompleteCancel() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '12'         \n"	// ���ϻ��� 12:�˼����
                    + "      ,FILE_CHK_DT   = SYSDATE    \n"
                    + "      ,FILE_CHK_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;			
				sqlParams[i++].Value = header.UserID;                   // �����
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);
				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�������� �˼��Ϸ����:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChkCompleteCancel() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "�������� �˼��Ϸ� ��� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// �������� CDN����Ȯ��
		/// </summary>
		/// <returns></returns>		
		public void SetAdFileCDNSync(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNSync() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '20'         \n"	// ���ϻ��� 20:CDN����ȭ							
                    + "      ,FILE_SYNC_DT   = SYSDATE    \n"
                    + "      ,FILE_SYNC_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;
				sqlParams[i++].Value = header.UserID;                   // �����
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	
				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�������� CDN����Ȯ��:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNSync() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "�������� CDN����Ȯ�� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}



		/// <summary>
		/// �������� CDN����Ȯ�� ���
		/// </summary>
		/// <returns></returns>		
		public void SetAdFileCDNSyncCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNSyncCancel() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '15'         \n"	// ���ϻ��� 15:�������						
                    + "      ,FILE_SYNC_DT   = SYSDATE    \n"
                    + "      ,FILE_SYNC_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;      
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;
				sqlParams[i++].Value = header.UserID;                   // �����
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	
				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�������� CDN����Ȯ�� ���:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNSyncCancel() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "�������� CDN����Ȯ�� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}



        /// <summary>
        /// �������� CDN����Ȯ��
        /// </summary>
        /// <returns></returns>		
		public void SetAdFileCDNPublish(HeaderModel header, AdFileWideModel adFileWideModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAdFileCDNPublish() Start");
                _log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

                int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

                sbQuery.Append("\n");
                sbQuery.Append("\n UPDATE ADVT_MST");
                sbQuery.Append("\n    SET FILE_STT	= '30'");
                sbQuery.Append("\n       ,FILE_PUB_DT  = SYSDATE");
                sbQuery.Append("\n       ,FILE_PUB_ID  = :RegID");
                sbQuery.Append("\n WHERE ITEM_NO     = :ItemNo");

                
                sqlParams[0] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[1] = new OracleParameter(":ItemNo", OracleDbType.Int32, 4);
                			
                sqlParams[0].Value = header.UserID;
                sqlParams[1].Value = Convert.ToInt32(adFileWideModel.ItemNo);	

                _log.Debug(sbQuery.ToString());

				try
                {
                    _db.BeginTran();
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("�������� �����Ϸ�:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");
					string AckNo = GetFileAckNo(adFileWideModel.MediaCode);
					SetFilePublishHistory(header, adFileWideModel, AckNo, "+");
                    _db.CommitTran();
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                adFileWideModel.ResultCD = "0000";  // ����
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAdFileCDNPublish() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                adFileWideModel.ResultCD   = "3201";
                adFileWideModel.ResultDesc = "�������� �����Ϸ� ó�� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
			finally
			{
				_db.Close();
			}		
        }


		/// <summary> 
		/// �������� CDN����Ȯ�� ���
		/// </summary>
		/// <returns></returns>
		public void SetAdFileCDNPublishCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNPublishCancel() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
					+ "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '20'         \n"	// ���ϻ��� 20:CDN����ȭ
                    + "      ,FILE_PUB_DT   = SYSDATE    \n"
                    + "      ,FILE_PUB_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;
                
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;		
				sqlParams[i++].Value = header.UserID;                   // �����
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	

				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// __MESSAGE__
					_log.Message("�������� CDN����Ȯ�����:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");

					// ���� ���ι�ȣ�� ����
					string AckNo = GetFileAckNo(adFileWideModel.MediaCode);
					
					SetFilePublishHistory(header, adFileWideModel, AckNo, "-");		// ������

					_db.CommitTran();


				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileCDNPublishCancel() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "�������� �����Ϸ� ��� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}





        /// <summary>
        /// �������� ��ž����
        /// </summary>
        /// <returns></returns>
        public void SetAdFileSTBDelete(HeaderModel header, AdFileWideModel adFileWideModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAdFileSTBDelete() Start");
                _log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

                sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '90'         \n"	// ���ϻ��� 90:��ž����							
//                    + "      ,RealEndDay  = CONVERT(CHAR(8),GETDATE(),112)  \n"	// ������ ������������ ��Ʈ�Ѵ�. 2007-10-28 �ƴϴ� ���Ѵ�.
                    + "      ,FILE_DEL_DT   = SYSDATE    \n"
                    + "      ,FILE_DEL_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
                    );

                i = 0;
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

                i = 0;	
                sqlParams[i++].Value = header.UserID;                   // �����
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	
                _log.Debug(sbQuery.ToString());

                // ��������
                try
                {
                    _db.BeginTran();

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// __MESSAGE__
					_log.Message("�������� ��ž����:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");


					// ���� ���ι�ȣ�� ����
					string AckNo = GetFileAckNo(adFileWideModel.MediaCode);
					
					SetFilePublishHistory(header, adFileWideModel, AckNo, "-");		// ������

                    _db.CommitTran();

                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                adFileWideModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAdFileSTBDelete() End");
                _log.Debug("-----------------------------------------");

            }
            catch(Exception ex)
            {
                adFileWideModel.ResultCD   = "3201";
                adFileWideModel.ResultDesc = "�������� ��ž���� ó���� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// �������� ��ſ���� ���
		/// </summary>
		/// <returns></returns>
        
		public void SetAdFileSTBDeleteCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileSTBDeleteCancel() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];

				sbQuery.Append("\n"
                    + "UPDATE ADVT_MST               \n"
                    + "   SET FILE_STT   = '30'         \n"	// ���ϻ��� 30:�����Ϸ�
                    + "      ,FILE_DEL_DT   = SYSDATE    \n"
                    + "      ,FILE_DEL_ID   = :RegID       \n"
                    + " WHERE ITEM_NO      = :ItemNo      \n"					
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;	
				sqlParams[i++].Value = header.UserID;                   // �����
                sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);	
				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// __MESSAGE__
					_log.Message("�������� ��ž�������:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");

					// ���� ���ι�ȣ�� ����
					string AckNo = GetFileAckNo(adFileWideModel.MediaCode);
					
					SetFilePublishHistory(header, adFileWideModel, AckNo, "+");		// �߰���

					_db.CommitTran();


				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileSTBDeleteCancel() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "�������� ��ž������� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// �������� ���米ü
		/// 2010/07/19�� History���� �߰�
		/// </summary>
		/// <returns></returns>
		public void SetAdFileChange(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			/*
				10:�̵��
			 *	12:�˼����
			 *	15:�������
			 *	20:CDN����ȭ
			 *	30:�����Ϸ�
			 */
			try
			{
				int rc = 0;

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChange() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				_db.BeginTran();

				#region [ �������� ���米ü ���� ]
				StringBuilder sbQuery		= new StringBuilder();

                OracleParameter[] sqlParams = new OracleParameter[1];

                sqlParams[0] = new OracleParameter(":ItemNo", OracleDbType.Int32);
				//sqlParams[1] = new SqlParameter(":RegID"      , SqlDbType.VarChar	,  10);				
				
				sqlParams[0].Value = Convert.ToInt32(adFileWideModel.ItemNo);				
				//sqlParams[1].Value = header.UserID;


				sbQuery.Append("\n");
                sbQuery.Append("\n UPDATE ADVT_MST");
                sbQuery.Append("\n    SET FILE_STT		= '10'");		// ���ϻ��� 11:��ü��⿡�� 10:�̵������ �����ϱ�� ������(10�� ��ȭ)							
                sbQuery.Append("\n		, FILE_LEN	= 0");
                sbQuery.Append("\n WHERE  ITEM_NO		= :ItemNo");
				_log.Debug(sbQuery.ToString());

				try
				{
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("�������� ���米ü���:["+adFileWideModel.ItemNo + "]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");

					// �ش� ������ ���°� CDN �����Ϸ��� ��쿡�� 
					if(adFileWideModel.FileState.Equals("30"))
					{
						// ���� ���ι�ȣ�� ����
						string AckNo = GetFileAckNo(adFileWideModel.MediaCode);
						SetFilePublishHistory(header, adFileWideModel, AckNo, "-");		// ������
					}
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
				#endregion

				#region [ ���ϱ�ü History���� ]
				sbQuery.Remove(0, sbQuery.Length );
                sbQuery.Append("\n insert into ADVT_FILEHST");
                sbQuery.Append("\n     (	ITEM_NO	,HST_SEQ	,REP_DT		,REP_ID");
                sbQuery.Append("\n 		,	FILE_STT	,FILE_TYP		,FILE_LEN");
                sbQuery.Append("\n 		,	FILE_PATH	,FILE_NM_PRE	,FILE_NM");
                sbQuery.Append("\n 		,	FILE_REG_DT	,FILE_REG_ID");
                sbQuery.Append("\n 		,	FILE_CHK_DT	,FILE_CHK_ID		,FILE_SYNC_DT");
                sbQuery.Append("\n 		,	FILE_SYNC_ID	,FILE_PUB_DT		,FILE_PUB_ID");
                sbQuery.Append("\n 		,	FILE_DEL_DT  ,FILE_DEL_ID		)");
                sbQuery.Append("\n select	ITEM_NO");
                sbQuery.Append("\n 		,	( select nvl(count(*),0)+1 from ADVT_FILEHST where ITEM_NO = " + Convert.ToInt32(adFileWideModel.ItemNo) + ")");
				sbQuery.Append("\n 		,	SYSDATE	, '" + header.UserID + "'");
                sbQuery.Append("\n 		,	FILE_STT	,FILE_TYP		,FILE_LEN");
                sbQuery.Append("\n 		,	FILE_PATH	,FILE_NM_PRE	,FILE_NM");
                sbQuery.Append("\n 		,	FILE_REG_DT	,FILE_REG_ID");
                sbQuery.Append("\n 		,	FILE_CHK_DT	,FILE_CHK_ID		,FILE_SYNC_DT");
                sbQuery.Append("\n 		,	FILE_SYNC_ID	,FILE_PUB_DT		,FILE_PUB_ID");
                sbQuery.Append("\n 		,	FILE_DEL_DT  ,FILE_DEL_ID");
                sbQuery.Append("\n from		ADVT_MST ");
                sbQuery.Append("\n where	ITEM_NO = " + Convert.ToInt32(adFileWideModel.ItemNo) );
				_log.Debug(sbQuery.ToString());

				try
				{
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
				#endregion

				if( rc == 0 )
				{
					_db.RollbackTran();
					throw new Exception("���ϱ�ü�̷� ������ �߰��� ���� �ʾҽ��ϴ�..");
				}
				else
				{
					_db.CommitTran();
					adFileWideModel.ResultCD = "0000";  // ����
				}

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileChange() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adFileWideModel.ResultCD   = "3201";
				adFileWideModel.ResultDesc = "�������� ���米ü��� ó���� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}


		#region ������λ����� ���ι�ȣ�� ����

		/// <summary>
		/// ���� ���Ϲ������λ����� ���ι�ȣ�� ����
		/// ���°� 30:���������̸� �ű�(���� 10:���δ��) ���� ������ AckNo ����
		/// </summary>
		/// <returns>string</returns>
		private string GetFileAckNo(string MediaCode)
		{

			string AckNo    = "";
			string AckState = "";

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFileAckNo() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("MediaCode	       :[" + MediaCode     + "]");		// �˻� ��ü				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("SELECT * FROM TABLE(GET_ACKNO('00'))");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					AckNo    =  ds.Tables[0].Rows[0]["AckNo"].ToString();
					AckState =  ds.Tables[0].Rows[0]["AckState"].ToString();
				}

				ds.Dispose();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFileAckNo() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}
			return AckNo;
		}

		#endregion

		#region ���Ϲ����̷¿� �ֱ�

		private bool SetFilePublishHistory(HeaderModel header, AdFileWideModel adFileWideModel, string AckNo, string AddDel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFilePublishHistory() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[5];

				sbQuery.Append( ""
                    + " INSERT INTO FILEDIST_HST (\n"
                    + "        ACK_NO       \n"
                    + "       ,ITEM_SEQ         \n"
                    + "       ,ITEM_NO      \n"
                    + "       ,FILE_NM    \n"
                    + "       ,FILE_OPER      \n"
                    + "       ,ID_INSERT       \n"
                    + "       ,DT_INSERT       \n"					
					+ "       )            \n"
					+ " SELECT            \n"
                    + "        :AckNo      \n"
                    + "       ,NVL(MAX(ITEM_SEQ),0) + 1 \n"
					+ "       ,:ItemNo     \n"
					+ "       ,:FileName   \n"					
					+ "       ,:AddDel     \n"
                    + "       ,:RegID      \n"
					+ "       ,SYSDATE   \n"	
                    + "  FROM FILEDIST_HST       \n"
                    + " WHERE ACK_NO     = :AckNo     \n"
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":AckNo", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":FileName", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":AddDel", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                //sqlParams[i++] = new OracleParameter("@AckNoTest", OracleDbType.Int32);

				i = 0;
                sqlParams[i++].Value = Convert.ToInt32(AckNo);
				sqlParams[i++].Value = Convert.ToInt32(adFileWideModel.ItemNo);				
				sqlParams[i++].Value = adFileWideModel.FileName;				
				sqlParams[i++].Value = AddDel;				
				sqlParams[i++].Value = header.UserID;				              // �����
                //sqlParams[i++].Value = Convert.ToInt32(AckNo);

				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// __MESSAGE__
					_log.Message("���Ϲ����̷µ��:���ι�ȣ["+AckNo+"]["+adFileWideModel.ItemNo+"]["+adFileWideModel.ItemName + "]["+adFileWideModel.FileName + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adFileWideModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFilePublishHistory() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				return false;
			}
			
			return true;

		}

		#endregion

    }
}