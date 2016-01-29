using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Schedule
{
	/// <summary>
	/// SchExcelBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchExcelBiz : BaseBiz
	{
		public SchExcelBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		/// <summary>
		/// �������� �����ȸ
		/// </summary>
		/// <param name="schExcelModel"></param>
		public void GetExcelList(HeaderModel header, SchExcelModel schExcelModel)
		{
			//bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetExcelList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey              :[" + schExcelModel.SearchKey            + "]");		// �˻� ��
				_log.Debug("SearchMediaCode	       :[" + schExcelModel.SearchMediaCode      + "]");		// �˻� ��ü				

				// __DEBUG__

				SqlParameter[] sqlParams = new SqlParameter[2];
		    
				int i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"          , SqlDbType.Int          );
				sqlParams[i++] = new SqlParameter("@SearchKey"          , SqlDbType.VarChar,  50 );

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32( schExcelModel.SearchMediaCode);
				sqlParams[i++].Value = schExcelModel.SearchKey ;


				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n"
					+ " SELECT  TA.MediaCode                                                    \n"
					+ "        ,TA.ORD                                                          \n"
					+ "        ,TA.Flag                                                         \n"
					+ "        ,TA.CategoryCode                                                 \n"
					+ "        ,ISNULL(TG.MenuName,'') AS CategoryName                          \n"
					+ "        ,TA.GenreCode                                                    \n"
					+ "        ,ISNULL(TH.MenuName,'') AS GenreName                             \n"
					+ "        ,TA.ChannelNo                                                    \n"
					+ "        ,TA.Title                                                        \n"
					+ "        ,TA.ItemNo                                                       \n"
					+ "        ,TA.ItemName                                                     \n"
					+ "        ,TB.CodeName AS AdStateName                                      \n"
					+ "        ,TA.FileName                                                     \n"
					+ "        ,TA.FileType                                                     \n"
					+ "        ,TC.CodeName AS FileTypeName                                     \n"
					+ "        ,TA.FileState                                                    \n"
					+ "        ,TD.CodeName AS FileStateName                                    \n"
					+ "        ,TA.FileLength                                                   \n"
					+ "        ,TA.FilePath                                                     \n"
					+ "        ,TA.DownLevel                                                    \n"
					+ "        ,CONVERT(VarChar(3),TA.DownLevel) +  ' ����' AS DownLevelName    \n"
					+ "        ,TA.ScheduleOrder                                                \n"
					+ "        ,TA.AdType                                                       \n"
					+ "        ,TE.CodeName AS AdTypeName                                       \n"
					+ "        ,TA.AckNo                                                        \n"
					+ "        ,TF.State AS AckState                                            \n"
					+ "  FROM                                                                   \n"
					+ "(                                                                        \n"
					+ "  SELECT B.MediaCode                                                     \n"
					+ "        ,'0' AS ORD                                                      \n"
					+ "        ,'Ȩ' AS Flag                                                    \n"
					+ "        ,0 AS CategoryCode                                               \n"
					+ "        ,0 AS GenreCode                                                  \n"
					+ "        ,0 AS ChannelNo                                                  \n"
					+ "        ,'' AS Title                                                     \n"
					+ "        ,A.ItemNo                                                        \n"
					+ "        ,A.ScheduleOrder                                                 \n"
					+ "        ,A.AckNo                                                         \n"
					+ "        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType   \n"
					+ "    FROM SchHome A INNER JOIN ContractItem B ON (A.ItemNo = B.ItemNo AND B.MediaCode = @MediaCode)                  \n"
					+ "                                                                         \n"
					+ "  UNION                                                                  \n"
					+ "                                                                         \n"
					+ "  SELECT B.MediaCode                                                     \n"
					+ "        ,'1' AS ORD                                                      \n"
					+ "        ,'�޴�' AS Flag                                                  \n"
					+ "        ,C.UpperMenuCode AS CategoryCode                                 \n"
					+ "        ,A.GenreCode                                                     \n"
					+ "        ,0 AS ChannelNo                                                  \n"
					+ "        ,'' AS Title                                                     \n"
					+ "        ,A.ItemNo                                                        \n"
					+ "        ,A.ScheduleOrder                                                 \n"
					+ "        ,A.AckNo                                                         \n"
					+ "        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType   \n"
					+ "   FROM SchChoiceMenuDetail A INNER JOIN ContractItem B ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode) \n"
					+ "                              INNER JOIN Menu         C ON (A.GenreCode = C.MenuCode  AND C.MediaCode = @MediaCode) \n"
					+ "   UNION                                                                 \n"
					+ "                                                                         \n"
					+ " SELECT B.MediaCode                                                      \n"
					+ "       ,'2' AS ORD                                                       \n"
					+ "       ,'ä��' AS Flag                                                   \n"
					+ "       ,C.Category AS CatagoryCode                                       \n"
					+ "       ,C.Genre    AS GenreCode                                          \n"
					+ "       ,A.ChannelNo                                                      \n"
					+ "       ,C.ProgramNm as Title                                             \n"
					+ "       ,A.ItemNo                                                         \n"
					+ "       ,A.ScheduleOrder                                                  \n"
					+ "       ,A.AckNo                                                          \n"
					+ "       ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType       \n"
					+ "   FROM SchChoiceChannelDetail A INNER JOIN ContractItem B ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = @MediaCode) \n"
					+ "                                 INNER JOIN Program      C ON (A.ChannelNo = C.Channel   AND C.MediaCode = @MediaCode) \n"
					+ "                                                                                                                       \n"                                                  
					+ " ) TA   LEFT JOIN SystemCode TB ON (TA.AdState      = TB.Code     AND TB.Section = '25') -- �������                   \n"
					+ "        LEFT JOIN SystemCode TC ON (TA.FileType     = TC.Code     AND TC.Section = '24') -- ���ϱ���                   \n"
					+ "        LEFT JOIN SystemCode TD ON (TA.FileState    = TD.Code     AND TD.Section = '31') -- ���ϻ���                   \n"
					+ "        LEFT JOIN SystemCode TE ON (TA.AdType       = TE.Code     AND TE.Section = '26') -- ��������                   \n"
					+ "        LEFT JOIN SchPublish TF ON (TA.AckNo        = TF.AckNo    AND TF.MediaCode = @MediaCode)  -- ���λ���          \n"
					+ "        LEFT JOIN Menu       TG ON (TA.CategoryCode = TG.MenuCode AND TG.MediaCode = @MediaCode)  -- ī�װ���        \n"
					+ "        LEFT JOIN Menu       TH ON (TA.GenreCode    = TH.MenuCode AND TH.MediaCode = @MediaCode)  -- �帣��            \n"
					);

				// �˻�� ������
				if (schExcelModel.SearchKey.Trim().Length > 0)
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
				schExcelModel.SchExcelDataSet = ds.Copy();
				// ���
				schExcelModel.ResultCnt = Utility.GetDatasetCount(schExcelModel.SchExcelDataSet);
				// ����ڵ� ��Ʈ
				schExcelModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schExcelModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetExcelList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schExcelModel.ResultCD = "3000";
				schExcelModel.ResultDesc = "�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}
	}
}