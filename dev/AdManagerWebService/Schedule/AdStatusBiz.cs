/*
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : �躸��
 * ������    : 2013.06.04
 * ��������  : 
 *            - GetAdStatusList_Compress() �Լ� ����
 * --------------------------------------------------------
 * �����ڵ�  : [E_02] 
 * ������		: YJ.Park
 * ������		: 2014.11.13
 * ��������	: Ȩ����(Ű��)���� ��ȸ ���� �߰�
 * --------------------------------------------------------  */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using AdManagerModel;

namespace AdManagerWebService.Schedule
{
	/// <summary>
	/// AdStatusBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdStatusBiz : BaseBiz
	{
		public AdStatusBiz() : base(FrameSystem.connDbString,true)
		{
			_log = FrameSystem.oLog;
		}
		/// <summary>
		/// �������� �����ȸ
		/// </summary>
		/// <param name="adStatusModel"></param>
		public void GetAdStatusList(HeaderModel header, AdStatusModel adStatusModel)
		{
			//bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdStatusList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey              :[" + adStatusModel.SearchKey            + "]");		// �˻� ��
				_log.Debug("SearchMediaCode	       :[" + adStatusModel.SearchMediaCode      + "]");		// �˻� ��ü				

				// __DEBUG__

				SqlParameter[] sqlParams = new SqlParameter[2];
		    
				int i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"          , SqlDbType.Int          );
				sqlParams[i++] = new SqlParameter("@SearchKey"          , SqlDbType.VarChar,  50 );

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32( adStatusModel.SearchMediaCode);
				sqlParams[i++].Value = adStatusModel.SearchKey ;


				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n SELECT  TA.MediaCode																		");
				sbQuery.Append("\n        ,TA.ORD																					");
				sbQuery.Append("\n        ,TA.Flag																					");
				sbQuery.Append("\n        ,TA.CategoryCode																		");
				sbQuery.Append("\n        ,ISNULL(TG.MenuName,'') AS CategoryName									");
				sbQuery.Append("\n        ,TA.GenreCode																			");
				sbQuery.Append("\n        ,ISNULL(TH.MenuName,'') AS GenreName										");
				sbQuery.Append("\n        ,TA.ChannelNo																			");
				sbQuery.Append("\n        ,TA.Title																					");
				sbQuery.Append("\n        ,TA.ItemNo																				");
				sbQuery.Append("\n        ,TA.ItemName																			");
				sbQuery.Append("\n        ,TB.CodeName AS AdStateName													");
				sbQuery.Append("\n        ,TA.FileName																				");
				sbQuery.Append("\n        ,TA.FileType																				");
				sbQuery.Append("\n        ,TC.CodeName AS FileTypeName													");
				sbQuery.Append("\n        ,TA.FileState																				");
				sbQuery.Append("\n        ,TD.CodeName AS FileStateName													");
				sbQuery.Append("\n        ,TA.FileLength																			");
				sbQuery.Append("\n        ,TA.FilePath																				");
				sbQuery.Append("\n        ,TA.DownLevel																			");
				sbQuery.Append("\n        ,CONVERT(VarChar(3),TA.DownLevel) +  ' ����' AS DownLevelName		");
				sbQuery.Append("\n        ,TA.ScheduleOrder																		");
				sbQuery.Append("\n        ,TA.AdType																				");
				sbQuery.Append("\n        ,TE.CodeName AS AdTypeName									                ");
				sbQuery.Append("\n        ,TA.AckNo																				");
				sbQuery.Append("\n        ,TF.State AS AckState																");
				sbQuery.Append("\n  FROM																							");
				sbQuery.Append("\n(																										");
				sbQuery.Append("\n  SELECT B.MediaCode																		");
				sbQuery.Append("\n        ,'0' AS ORD																				");
				sbQuery.Append("\n        ,'Ȩ' AS Flag																				");
				sbQuery.Append("\n        ,0 AS CategoryCode																	");
				sbQuery.Append("\n        ,0 AS GenreCode																		");
				sbQuery.Append("\n        ,0 AS ChannelNo																		");
				sbQuery.Append("\n        ,'' AS Title																				");
				sbQuery.Append("\n        ,A.ItemNo																					");
				sbQuery.Append("\n        ,A.ScheduleOrder																		");
				sbQuery.Append("\n        ,A.AckNo																					");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n    FROM SchHome A with(NoLock)																												");
				sbQuery.Append("\n         INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo AND A.MediaCode = B.MediaCode)				");
				sbQuery.Append("\n   WHERE A.MediaCode = @MediaCode													");
				sbQuery.Append("\n																										");

				// [E_02]
				sbQuery.Append("\n  UNION ALL																						");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n  SELECT B.MediaCode																        ");
				sbQuery.Append("\n        ,'0' AS ORD																				");
				sbQuery.Append("\n        ,'Ȩ(Ű��)' AS Flag																		");
				sbQuery.Append("\n        ,0 AS CategoryCode																	");
				sbQuery.Append("\n        ,0 AS GenreCode																		");
				sbQuery.Append("\n        ,0 AS ChannelNo																		");
				sbQuery.Append("\n        ,'' AS Title																				");
				sbQuery.Append("\n        ,A.ItemNo																					");	
				sbQuery.Append("\n        ,A.ScheduleOrder																		");
				sbQuery.Append("\n        ,A.AckNo																					");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n    FROM SchHomeKids A with(NoLock)																											");
				sbQuery.Append("\n         INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo AND A.MediaCode = B.MediaCode)				");
				sbQuery.Append("\n   WHERE A.MediaCode = @MediaCode													");
				sbQuery.Append("\n																										");

				sbQuery.Append("\n  UNION ALL																						");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n  SELECT B.MediaCode																		");
				sbQuery.Append("\n        ,'1' AS ORD																				");
				sbQuery.Append("\n        ,'�޴�' AS Flag																			");
				sbQuery.Append("\n        ,C.UpperMenuCode AS CategoryCode												");
				sbQuery.Append("\n        ,A.GenreCode																			");
				sbQuery.Append("\n        ,0 AS ChannelNo																		");
				sbQuery.Append("\n        ,'' AS Title																				");
				sbQuery.Append("\n        ,A.ItemNo																					");
				sbQuery.Append("\n        ,A.ScheduleOrder																		");
				sbQuery.Append("\n        ,A.AckNo																					");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n   FROM SchChoiceMenuDetail A with(NoLock)																									");
				sbQuery.Append("\n        INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode)		");
				sbQuery.Append("\n        INNER JOIN Menu         C with(NoLock) ON (A.GenreCode = C.MenuCode  AND C.MediaCode = @MediaCode)		");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n   UNION ALL																						");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n SELECT B.MediaCode																			");
				sbQuery.Append("\n       ,'2' AS ORD																				");
				sbQuery.Append("\n       ,'ä��' AS Flag																			");
				sbQuery.Append("\n       ,C.Category AS CatagoryCode														");
				sbQuery.Append("\n       ,C.Genre    AS GenreCode																");
				sbQuery.Append("\n       ,A.ChannelNo																				");
				sbQuery.Append("\n       ,C.ProgramNm as Title																	");
				sbQuery.Append("\n       ,A.ItemNo																					");
				sbQuery.Append("\n       ,A.ScheduleOrder																		");
				sbQuery.Append("\n       ,A.AckNo																					");
				sbQuery.Append("\n       ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n   FROM SchChoiceChannelDetail A with(NoLock)																								");
				sbQuery.Append("\n        INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode)		");
				sbQuery.Append("\n        INNER JOIN Program      C with(NoLock) ON (A.ChannelNo = C.Channel   AND C.MediaCode = @MediaCode)		");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n   UNION ALL																						");
				sbQuery.Append("\n																										");
				sbQuery.Append("\n  SELECT B.MediaCode																		");
				sbQuery.Append("\n        ,'3' AS ORD																				");
				sbQuery.Append("\n        ,'�߰�' AS Flag																			");
				sbQuery.Append("\n        ,0 AS CategoryCode																	");
				sbQuery.Append("\n        ,0 AS GenreCode																		");
				sbQuery.Append("\n        ,0 AS ChannelNo																		");
				sbQuery.Append("\n        ,'' AS Title																				");
				sbQuery.Append("\n        ,A.ItemNo																					");
				sbQuery.Append("\n        ,A.ScheduleOrder																		");
				sbQuery.Append("\n        ,A.AckNo																					");
				sbQuery.Append("\n        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType		");
				sbQuery.Append("\n    FROM SchAppend A with(NoLock)																												");
				sbQuery.Append("\n         INNER JOIN ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo AND A.MediaCode = B.MediaCode)				");
				sbQuery.Append("\n   WHERE A.MediaCode = @MediaCode																																	");
				sbQuery.Append("\n																																														");
				sbQuery.Append("\n ) TA   LEFT JOIN SystemCode TB with(NoLock) ON (TA.AdState      = TB.Code     AND TB.Section = '25') -- �������							");
				sbQuery.Append("\n        LEFT JOIN SystemCode TC with(NoLock) ON (TA.FileType     = TC.Code     AND TC.Section = '24') -- ���ϱ���							");
				sbQuery.Append("\n        LEFT JOIN SystemCode TD with(NoLock) ON (TA.FileState    = TD.Code     AND TD.Section = '31') -- ���ϻ���							");
				sbQuery.Append("\n        LEFT JOIN SystemCode TE with(NoLock) ON (TA.AdType       = TE.Code     AND TE.Section = '26') -- ��������							");
				sbQuery.Append("\n        LEFT JOIN SchPublish TF with(NoLock) ON (TA.AckNo        = TF.AckNo    AND TF.MediaCode = @MediaCode)  -- ���λ���				");
				sbQuery.Append("\n        LEFT JOIN Menu       TG with(NoLock) ON (TA.CategoryCode = TG.MenuCode AND TG.MediaCode = @MediaCode)  -- ī�װ���		");
				sbQuery.Append("\n        LEFT JOIN Menu       TH with(NoLock) ON (TA.GenreCode    = TH.MenuCode AND TH.MediaCode = @MediaCode)  -- �帣��			");

				// �˻�� ������
				if (adStatusModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" WHERE ("
						+ "    TA.FileName    LIKE '%'+ @SearchKey + '%' \n"		
						+ " OR TA.ItemName    LIKE '%'+ @SearchKey + '%' \n"													
						+ " OR TG.MenuName    LIKE '%'+ @SearchKey + '%' \n"													
						+ " OR TH.MenuName    LIKE '%'+ @SearchKey + '%' \n"													
						+ " OR TA.ChannelNo   LIKE '%'+ @SearchKey + '%' \n"													
						+ " OR TA.Title       LIKE '%'+ @SearchKey + '%' \n"														
						+ " ) \n"
						);
				}			

				sbQuery.Append(" ORDER BY ORD, CategoryCode, GenreCode, ChannelNo, ScheduleOrder, FileName  \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// ��� DataSet�� �ش�𵨿� ����
				adStatusModel.AdStatusDataSet = ds.Copy();
				// ���
				adStatusModel.ResultCnt = Utility.GetDatasetCount(adStatusModel.AdStatusDataSet);
				// ����ڵ� ��Ʈ
				adStatusModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + adStatusModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdStatusList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adStatusModel.ResultCD = "3000";
				adStatusModel.ResultDesc = "�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

        /// <summary>
        /// �����Ͱ� ������ ����� �����ϴµ�, ������ ����� ����.
        /// 2013.05.02 �ϴ� �� ��ɸ� �����׽�Ʈ �ϴ� ����.
        /// �� �Ǹ�, ���� ��ȭ �� �ϰ������� �������� ����
        /// </summary>
        /// <param name="header"></param>
        /// <param name="adStatusModel"></param>
        public void GetAdStatusList_Compress(HeaderModel header, AdStatusModel adStatusModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdStatusList() Start");
                _log.Debug("-----------------------------------------");

                _db.Open();
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey              :[" + adStatusModel.SearchKey + "]");		// �˻���
                _log.Debug("SearchMediaCode	       :[" + adStatusModel.SearchMediaCode + "]");		// �˻� ��ü				

                OracleParameter[] sqlParams = new OracleParameter[1];

                sqlParams[0] = new OracleParameter("@SearchKey", OracleDbType.Varchar2, 50);

                sqlParams[0].Value = adStatusModel.SearchKey;

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n SELECT  TA.MediaCode													");
                sbQuery.Append("\n        ,TA.ORD																");
                sbQuery.Append("\n        ,TA.Flag																");
                sbQuery.Append("\n        ,TA.CategoryCode													");
                sbQuery.Append("\n        ,NVL(TG.MENU_NM,'') AS CategoryName				");
                sbQuery.Append("\n        ,TA.GenreCode														");
                sbQuery.Append("\n        ,NVL(TH.MENU_NM,'') AS GenreName					");
                sbQuery.Append("\n        ,TA.ChannelNo														");
                sbQuery.Append("\n        ,TA.Title																");
                sbQuery.Append("\n        ,TA.ItemNo															");
                sbQuery.Append("\n        ,TA.ItemName														");
                sbQuery.Append("\n        ,TB.STM_COD_NM AS AdStateName								");
                sbQuery.Append("\n        ,TA.FileName															");
                sbQuery.Append("\n        ,TA.FileType															");
                sbQuery.Append("\n        ,TC.STM_COD_NM AS FileTypeName								");
                sbQuery.Append("\n        ,TA.FileState															");
                sbQuery.Append("\n        ,TD.STM_COD_NM AS FileStateName								");
                sbQuery.Append("\n        ,TA.FileLength														");
                sbQuery.Append("\n        ,TA.FilePath															");
                sbQuery.Append("\n        ,TA.DownLevel														");
                sbQuery.Append("\n        ,' ����' AS DownLevelName    ");
                sbQuery.Append("\n        ,TA.ScheduleOrder											        ");
                sbQuery.Append("\n        ,TA.AdType															");
                sbQuery.Append("\n        ,TE.STM_COD_NM AS AdTypeName						        ");
                sbQuery.Append("\n        ,TA.AckNo														    ");
                sbQuery.Append("\n        ,TF.ACK_STT AS AckState										    ");
                sbQuery.Append("\n  FROM																		");
                sbQuery.Append("\n(																					");
                sbQuery.Append("\n  SELECT 1 AS MediaCode												    ");
                sbQuery.Append("\n        ,'0' AS ORD															");
                sbQuery.Append("\n        ,'�޴�' AS Flag															");
                sbQuery.Append("\n        ,'' AS CategoryCode										        ");
                sbQuery.Append("\n        ,A.MENU_COD AS GenreCode											        ");
                sbQuery.Append("\n        ,'' AS ChannelNo												    ");
                sbQuery.Append("\n        ,'' AS Title															");
                sbQuery.Append("\n        ,A.ITEM_NO AS ItemNo																");
                sbQuery.Append("\n        ,A.SCHD_ORD AS ScheduleOrder												    ");
                sbQuery.Append("\n        ,A.ACK_NO AS AckNo																");
                sbQuery.Append("\n        ,B.ITEM_NM AS ItemName, B.ADVT_STT AS AdState, B.FILE_NM AS FileName, B.FILE_TYP AS FileType, B.FILE_STT AS FileState, B.FILE_LEN AS FileLength,B.FILE_PATH AS FilePath,'' AS DownLevel,B.ADVT_TYP AS AdType		");
                sbQuery.Append("\n    FROM SCHD_MENU A 																												");
                sbQuery.Append("\n         INNER JOIN ADVT_MST B ON (A.ITEM_NO = B.ITEM_NO)				");
                sbQuery.Append("\n   								");
                sbQuery.Append("\n																					");
				
				//[E_02]
                sbQuery.Append("\n  UNION ALL																    ");
				sbQuery.Append("\n																					");
                sbQuery.Append("\n  SELECT 1 AS MediaCode												    ");
                sbQuery.Append("\n        ,'1' AS ORD															");
                sbQuery.Append("\n        ,'����' AS Flag															");
                sbQuery.Append("\n        ,'' AS CategoryCode										        ");
                sbQuery.Append("\n        ,'' AS GenreCode											        ");
                sbQuery.Append("\n        ,CH_NO AS ChannelNo												    ");
                sbQuery.Append("\n        ,'' AS Title															");
                sbQuery.Append("\n        ,A.ITEM_NO AS ItemNo																");
                sbQuery.Append("\n        ,A.SCHD_ORD AS ScheduleOrder												    ");
                sbQuery.Append("\n        ,A.ACK_NO AS AckNo																");
                sbQuery.Append("\n        ,B.ITEM_NM AS ItemName, B.ADVT_STT AS AdState, B.FILE_NM AS FileName, B.FILE_TYP AS FileType, B.FILE_STT AS FileState, B.FILE_LEN AS FileLength,B.FILE_PATH AS FilePath,'' AS DownLevel,B.ADVT_TYP AS AdType		");
                sbQuery.Append("\n    FROM SCHD_CHNL A 																												");
                sbQuery.Append("\n         INNER JOIN ADVT_MST B ON (A.ITEM_NO = B.ITEM_NO)				");
				sbQuery.Append("\n																					");
                sbQuery.Append("\n																																								");
                sbQuery.Append("\n ) TA   LEFT JOIN STM_COD TB ON (TA.AdState      = TB.STM_COD     AND TB.STM_COD_CLS = '25') -- �������	");
                sbQuery.Append("\n        LEFT JOIN STM_COD TC ON (TA.FileType     = TC.STM_COD     AND TC.STM_COD_CLS = '24') -- ���ϱ���	");
                sbQuery.Append("\n        LEFT JOIN STM_COD TD ON (TA.FileState    = TD.STM_COD     AND TD.STM_COD_CLS = '31') -- ���ϻ���	");
                sbQuery.Append("\n        LEFT JOIN STM_COD TE ON (TA.AdType       = TE.STM_COD     AND TE.STM_COD_CLS = '26') -- ��������	");
                sbQuery.Append("\n        LEFT JOIN SCHD_DIST_MST TF ON (TA.AckNo        = TF.ACK_NO)  -- ���λ���				");
                sbQuery.Append("\n        LEFT JOIN MENU_COD       TG ON (TA.CategoryCode = TG.MENU_COD)  -- ī�װ���		");
                sbQuery.Append("\n        LEFT JOIN MENU_COD       TH ON (TA.GenreCode    = TH.MENU_COD)  -- �帣��			");
                sbQuery.Append("\n where TA.FileState <> '90' -- ���ϻ����� ������					");
                sbQuery.Append("\n and   TA.AdState   <> '40' -- ������� ����� ������				");

                // �˻�� ������
                if (adStatusModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append(" AND (	TA.FileName    LIKE '%'+ @SearchKey + '%'	");
                    sbQuery.Append("\n OR TA.ItemName    LIKE '%'+ @SearchKey + '%'		");
                    sbQuery.Append("\n OR TG.MenuName    LIKE '%'+ @SearchKey + '%'	");
                    sbQuery.Append("\n OR TH.MenuName    LIKE '%'+ @SearchKey + '%'	");
                    sbQuery.Append("\n OR TA.ChannelNo   LIKE '%'+ @SearchKey + '%'		");
					sbQuery.Append("\n OR TA.Title       LIKE '%'+ @SearchKey + '%'			");
                    sbQuery.Append("\n )																	");
                }

                sbQuery.Append(" ORDER BY ORD, CategoryCode, GenreCode, ChannelNo, ScheduleOrder, FileName  \n");
                _log.Debug(sbQuery.ToString());

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                byte[] data = FrameSystem.CompressData(ds);

                //adStatusModel.AdStatusDataSet = ds;
                //adStatusModel.AdStatusDataSet.RemotingFormat = SerializationFormat.Binary;

                adStatusModel.FileName = Convert.ToBase64String(data, 0, data.Length);
                adStatusModel.ResultCnt = ds.Tables[0].Rows.Count;
                adStatusModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + adStatusModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdStatusList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                adStatusModel.ResultCD = "3000";
                adStatusModel.ResultDesc = "�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
	}
}