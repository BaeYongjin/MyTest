using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using WinFramework;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Target
{
	/// <summary>
	/// RatioBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RatioBiz : BaseBiz
	{
		public RatioBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}

		#region �����޴����� ���� ��ȸ

		/// <summary>
		/// �����޴����� ���� ��ȸ
		/// </summary>
		/// <returns></returns>
		public void GetSchChoiceMenuDetailList(HeaderModel header, RatioModel ratioModel)
		{
			try
			{   // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".GetSchChoiceMenuDetailList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("ItemNo            :[" + ratioModel.ItemNo + "]");
				// __DEBUG__

				// ��������
				int i = 0;
				StringBuilder sbQuery = new StringBuilder();
				OracleParameter[] sqlParams = new OracleParameter[1];

				i = 0;
				sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.ItemNo);
                
                #region ������ ��
                //
                /*
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn                        \n"
					+ "		  ,A.CatagoryCode	                        \n"
					+ "		  ,A.Catagory		                        \n"
					+ "		  ,A.Genre			                        \n"
					+ "		  ,A.GenreCode		                        \n"
					+ "	FROM (					                        \n"
					+ "SELECT 'False' AS CheckYn                        \n"
					+ "      ,(SELECT CategoryCode					    \n"
					+ "                  FROM Category                  \n"
					+ "                WHERE MediaCode    = A.MediaCode \n"
					+ "                AND CategoryCode = (SELECT TOP 1 CategoryCode FROM ChannelSet WHERE GenreCode = A.GenreCode) \n"
					+ "       ) AS CatagoryCode                         \n"
					+ "      ,(SELECT (SPACE(5 - LEN(CONVERT(VARCHAR(5),CategoryCode))) +  CONVERT(VARCHAR(10),CategoryCode) + ' ' + CategoryName)  \n"
					+ "                  FROM Category                  \n"
					+ "                WHERE MediaCode    = A.MediaCode \n"
					+ "                AND CategoryCode = (SELECT TOP 1 CategoryCode FROM ChannelSet WHERE GenreCode = A.GenreCode) \n"
					+ "       ) AS Catagory                             \n"
					+ "      ,(SELECT (SPACE(5 - LEN(CONVERT(VARCHAR(5),GenreCode)))    +  CONVERT(VARCHAR(10),GenreCode)    + ' ' + GenreName   )  \n"
					+ "          FROM Genre                             \n"
					+ "         WHERE MediaCode = A.MediaCode           \n"
					+ "           AND GenreCode = A.GenreCode           \n"
					+ "       ) AS Genre                                \n"
					+ "      ,A.GenreCode                               \n"
					//					+ "      ,C.State AS AckState                       \n"
					+ "  FROM SchChoiceMenuDetail A INNER JOIN ContractItem B ON (A.MediaCode = B.MediaCode AND A.ItemNo = B.ItemNo) \n"
					+ "                              LEFT JOIN SchPublish   C ON (A.AckNo      = C.AckNo)                            \n"
					+ "	WHERE A.ItemNo = @ItemNo  \n"
					+ "	UNION	\n"
					+ "SELECT	\n"
					+ "	'False' AS CheckYn	\n"
					+ "	, B.CategoryCode	\n"
					+ "	, SPACE(5 - LEN(CONVERT(VARCHAR(5), B.CategoryCode))) +  CONVERT(VARCHAR(10), B.CategoryCode) + ' ' + C.CategoryName AS Catagory	\n"
					+ "		, SPACE(5 - LEN(CONVERT(VARCHAR(5), B.GenreCode))) + CONVERT(VARCHAR(10), B.GenreCode) + ' ' + D.GenreName AS Genre	\n"
					+ "	, B.GenreCode	\n"
					+ "FROM SchChoiceChannelDetail A	\n"
					+ "JOIN ChannelSet B WITH (NOLOCK) ON B.ChannelNo = A.ChannelNo	\n"
					+ "JOIN Category C WITH (NOLOCK) ON C.MediaCode = A.MediaCode AND C.CategoryCode = B.CategoryCode	\n"
					+ "JOIN Genre D WITH (NOLOCK) ON D.MediaCode = A.MediaCode AND D.GenreCode = B.GenreCode	\n"
					+ "WHERE ItemNo = @ItemNo	\n"
					+ "	AND B.SeriesNo = (SELECT MIN(SeriesNo) FROM ChannelSet WHERE MediaCode = B.MediaCode AND ChannelNo = B.ChannelNo)	\n"
					+ " ) A  \n"
					+ " WHERE NOT EXISTS (SELECT 'A'  \n"
					+ "						FROM(  \n"
					+ "							SELECT 'False' AS CheckYn                        \n"
					+ "								  ,B.CategoryCode	  \n"
					+ "							      ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),b.CategoryCode))) +  CONVERT(VARCHAR(10),b.CategoryCode) + ' ' + e.CategoryName) AS Catagory	  \n"
					+ "								  ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),b.GenreCode)))    +  CONVERT(VARCHAR(10),b.GenreCode)    + ' ' + d.GenreName   ) AS Genre  \n"
					+ "								  ,B.GenreCode  \n"
					+ "							FROM SchRateDetail b  \n"
					+ "							LEFT JOIN Genre d ON (b.GenreCode = d.GenreCode)  \n"
					+ "							LEFT JOIN Category e ON (b.CategoryCode = e.CategoryCode)  \n"
					+ "							WHERE 1=1    \n"
					+ "							  AND B.ItemNo = @ItemNo  \n"
					+ "							) B  \n"
					+ "					WHERE A.GenreCode = B.GenreCode ) \n"
					+ " ORDER BY A.Catagory, A.Genre  \n"
					);
                */
                #endregion

                sbQuery.AppendLine(" SELECT     ");
                sbQuery.AppendLine("         'False' AS CheckYn                     ");
                sbQuery.AppendLine("         ,X.CatagoryCode	                    ");
                sbQuery.AppendLine("         ,X.Catagory		                    "); 
                sbQuery.AppendLine("         ,X.Genre			                    "); 
                sbQuery.AppendLine("         ,X.GenreCode		                    "); 
                sbQuery.AppendLine(" FROM (					                        "); 
                sbQuery.AppendLine("           SELECT 'False' AS CheckYn            ");            
                sbQuery.AppendLine("                     ,C.MENU_COD AS CatagoryCode ");                         
                sbQuery.AppendLine("                     ,C.MENU_NM AS Catagory     ");                        
                sbQuery.AppendLine("                     ,D.MENU_NM AS Genre        ");                        
                sbQuery.AppendLine("                     ,D.MENU_COD AS GenreCode   ");
                sbQuery.AppendLine("         FROM SCHD_MENU A                       ");    
                sbQuery.AppendLine("         INNER JOIN ADVT_MST        B ON (A.item_no = B.item_no)    ");
                sbQuery.AppendLine("         LEFT JOIN SCHD_DIST_MST   C ON (A.Ack_No      = C.Ack_No)  ");                           
                sbQuery.AppendLine("         LEFT JOIN (SELECT MENU_NM, MENU_COD, MENU_COD_PAR      ");
                sbQuery.AppendLine("                        FROM MENU_COD WHERE MENU_LVL=2          ");   
                sbQuery.AppendLine("                        ) C  ON (a.menu_cod_par = c.menu_cod)   ");
                sbQuery.AppendLine("         LEFT JOIN (SELECT MENU_NM, MENU_COD, MENU_COD_PAR      ");
                sbQuery.AppendLine("                        FROM MENU_COD WHERE MENU_LVL = 3        ");    
                sbQuery.AppendLine("                        ) D ON (a.menu_cod = d.menu_cod)        ");
                sbQuery.AppendLine("         WHERE A.Item_No = :ItemNo              ");
                sbQuery.AppendLine(" ) X                                            ");
                sbQuery.AppendLine(" WHERE NOT EXISTS                               ");        
                sbQuery.AppendLine("                (SELECT 'A'                     ");
                sbQuery.AppendLine("                 FROM(                          ");
                sbQuery.AppendLine("                      SELECT 'False' AS CheckYn ");                       
                sbQuery.AppendLine("                             ,c.menu_cod as CategoryCode	");
                sbQuery.AppendLine("                             ,c.menu_nm AS Catagory	        ");
                sbQuery.AppendLine("                             ,d.menu_nm AS Genre            ");
                sbQuery.AppendLine("                             ,d.menu_cod as GenreCode       ");
                sbQuery.AppendLine("                      FROM SCHDRT_DTL b                     ");
                sbQuery.AppendLine("                      LEFT JOIN (SELECT MENU_NM, MENU_COD, MENU_COD_PAR ");
                sbQuery.AppendLine("                                 FROM MENU_COD WHERE MENU_LVL = 3       ");
                sbQuery.AppendLine("                                ) D ON (b.menu_cod = d.menu_cod)        ");
                sbQuery.AppendLine("                      LEFT JOIN (SELECT MENU_NM, MENU_COD, MENU_COD_PAR ");    
                sbQuery.AppendLine("                                 FROM MENU_COD WHERE MENU_LVL=2         ");
                sbQuery.AppendLine("                                ) C  ON (b.menu_cod_par = c.menu_cod)   ");
                sbQuery.AppendLine("                      WHERE 1=1                 ");
                sbQuery.AppendLine("                      AND B.Item_No = :ItemNo   ");
                sbQuery.AppendLine("                      ) B                       ");
                sbQuery.AppendLine("                   WHERE x.GenreCode = B.GenreCode  ");
                sbQuery.AppendLine("                 )              ");
                sbQuery.AppendLine(" ORDER BY x.Catagory, x.Genre   ");
                // __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

				// ��� DataSet�� �𵨿� ����
				ratioModel.MenuDataSet = ds.Copy();
				// ���
				ratioModel.ResultCnt = Utility.GetDatasetCount(ratioModel.MenuDataSet);
				ratioModel.ResultCD = "0000";  // ����

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ratioModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".GetSchChoiceMenuDetailList() End");
				_log.Debug("-----------------------------------------");
			}
			catch (Exception ex)
			{
				ratioModel.ResultCD = "3101";
				ratioModel.ResultDesc = "�����޴����� ���� ��ȸ �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion

		/// <summary>
		/// �׷�1�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetSchRateList(HeaderModel header, RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSchRateList() Start");
				_log.Debug("-----------------------------------------");
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
//				_log.Debug("ItemNo      :[" + ratioModel.ItemNo       + "]");
//				_log.Debug("EntrySeq:[" + ratioModel.EntrySeq + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// ��������
				sbQuery.Append("\n"
					+ " SELECT   item_no as ItemNo                 \n"	
				    + "         ,'' as MediaCode                   \n"
					+ "			,entry_seq as EntrySeq				\n"
					+ "			,entry_rt  as EntryRate				\n"
					+ "			,use_yn		as EntryYn		\n"					
					+ " FROM    SCHDRT_MST    \n"		
					+ " WHERE   1=1   \n"						
					);

				// ä�μ·����� ����������
				if(ratioModel.ItemNo.Trim().Length > 0 && !ratioModel.ItemNo.Trim().Equals("00"))
				{
					sbQuery.Append(" AND item_no = '" + ratioModel.ItemNo.Trim() + "' \n");
				}
//		
//				// ä�μ·����� ����������
//				if(ratioModel.EntrySeq.Trim().Length > 0 && !ratioModel.EntrySeq.Trim().Equals("00"))
//				{
//					sbQuery.Append(" AND EntrySeq = '" + ratioModel.EntrySeq.Trim() + "' \n");
//				}		
												
				//sbQuery.Append("\n  GROUP BY a.ChannelNo,b.TotalSeries,b.Title,convert(char(10), a.ModDt, 120),a.MediaCode,a.CategoryCode,CategoryName,a.GenreCode,d.GenreName,genreName ");
				sbQuery.Append("\n ORDER BY entry_seq, item_no");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				ratioModel.SchRateDataSet = ds.Copy();
				// ���
				ratioModel.ResultCnt = Utility.GetDatasetCount(ratioModel.SchRateDataSet);
				// ����ڵ� ��Ʈ
				ratioModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ratioModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSchRateList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ratioModel.ResultCD = "3000";
				ratioModel.ResultDesc = "�׷�1��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		/// �׷�1�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetSchRateDetailList(HeaderModel header, RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSchRateDetailList() Start");
				_log.Debug("-----------------------------------------");
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// ��������
				sbQuery.Append("\n"
					+ " SELECT   item_no as ItemNo         \n"
					+ "	        ,'' as MediaCode		\n"
					+ "			,entry_seq as EntrySeq		\n"
					+ "			,menu_cod_par as CategoryCode	\n"
					+ "			,menu_cod as  GenreCode      \n"					
					+ "   FROM SCHDRT_DTL    \n"		
					+ " WHERE   Item_No = '" + ratioModel.ItemNo + "' \n");		
				sbQuery.Append("\n ORDER BY Entry_Seq");

				_log.Debug(sbQuery.ToString());
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				ratioModel.SchRateDetailDataSet = ds.Copy();
				// ���
				ratioModel.ResultCnt = Utility.GetDatasetCount(ratioModel.SchRateDetailDataSet);
				// ����ڵ� ��Ʈ
				ratioModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ratioModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSchRateDetailList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ratioModel.ResultCD = "3000";
				ratioModel.ResultDesc = "�׷�1��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}
	
		/// <summary>
		/// �׷�1�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup1List(HeaderModel header, RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroup1List() Start");
				_log.Debug("-----------------------------------------");
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
//				_log.Debug("SearchKey      :[" + ratioModel.SearchKey       + "]");
//				_log.Debug("SearchClient:[" + ratioModel.MediaName + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

                // ��������
                #region ���� �Ұ�
                /*
				sbQuery.Append("\n"
					+ " SELECT	   'False' AS CheckYn                    \n"
					+ "			   ,a.ItemNo                 			 \n"
					+ "			   ,a.MediaCode							 \n"
					+ "			   ,a.EntrySeq							 \n"
					+ "			   ,a.EntryRate							 \n"
					+ "			   ,isnull(v.GenreCode,-1) as EntryYN    \n"
					+ "			   ,b.CategoryCode						 \n"
					+ "			   ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),b.CategoryCode))) +  CONVERT(VARCHAR(10),b.CategoryCode) + ' ' + e.CategoryName) AS CategoryName				\n"	
					+ "			   ,b.GenreCode							 \n"
					+ "			   ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),b.GenreCode)))    +  CONVERT(VARCHAR(10),b.GenreCode)    + ' ' + d.GenreName   ) AS GenreName1    		    \n"												 
					+ "   FROM  SchRate a with(noLock) LEFT JOIN SchRateDetail b with(noLock)        		  \n"
					+ "			ON (a.ItemNo = b.ItemNo AND a.MediaCode = b.MediaCode AND a.EntrySeq = b.EntrySeq)            		 \n"				
					+ "   LEFT JOIN Genre d with(noLock)    ON (b.GenreCode = d.GenreCode)            \n"	
					+ "   LEFT JOIN Category e with(noLock) ON (b.CategoryCode = e.CategoryCode)   \n"	
					+ "   left join ( select * \n"
					+ "   			  from	SchRateDetail \n"
					+ "   			  where	GenreCode in(	select GenreCode from dbo.SchRateDetail nolock where ItemNo = '" + ratioModel.ItemNo.Trim() + "' \n"
					+ "   									except \n"
					+ "   									select Genre \n"
					+ "   									from ( \n"
					+ "   											select  GenreCode as Genre \n"
					+ "   											from	SchChoiceMenuDetail    with(noLock)  \n"
					+ "   											where	ItemNo = '" + ratioModel.ItemNo.Trim() + "'\n"
					+ "   											union all \n"
					+ "   											select  Genre \n"
					+ "   											from	v_adv_contentset a with(nolock) \n"
					+ "   											inner join SchChoiceChannelDetail b with(nolock) on b.ChannelNo = a.Channel \n"
					+ "   											where	b.ItemNo = '" + ratioModel.ItemNo.Trim() + "' ) v ) \n"
					+ "   			and		ItemNo = '" + ratioModel.ItemNo.Trim() + "' ) v on b.GenreCode = v.GenreCode  \n"
					+ "   WHERE 1=1   \n"	
					+ "   AND a.EntrySeq = 1   \n"	
					);								
                */
                #endregion


                sbQuery.AppendLine(" SELECT	   'False' AS CheckYn                   ");               
                sbQuery.AppendLine("             ,a.item_no as ItemNo               ");                 			 
                sbQuery.AppendLine("             ,'' as MediaCode					");               		 
                sbQuery.AppendLine("             ,a.entry_seq as EntrySeq			");				 
                sbQuery.AppendLine("             ,a.entry_rt as EntryRate			");				 
                sbQuery.AppendLine("             ,NVL(v.menu_cod,-1) as EntryYN     "); 
                sbQuery.AppendLine("             ,b.menu_cod_par as CategoryCode	");					 
                sbQuery.AppendLine("             ,(SUBSTR(b.menu_cod_par,5,5) || ' ' || e.menu_nm) AS CategoryName	");				
                sbQuery.AppendLine("             ,b.menu_cod as GenreCode			                                ");				 
                sbQuery.AppendLine("             ,( SUBSTR(b.menu_cod,5,5) ||  ' ' || d.menu_nm   ) AS GenreName1   "); 		    												 
                sbQuery.AppendLine(" FROM  SCHDRT_MST a                                                             ");
                sbQuery.AppendLine(" LEFT JOIN SCHDRT_DTL b 	ON (a.item_no = b.item_no AND a.entry_seq = b.entry_seq) ");
                sbQuery.AppendLine(" LEFT JOIN MENU_COD d ON (b.menu_cod = d.menu_cod)            	    ");
                sbQuery.AppendLine(" LEFT JOIN MENU_COD e ON (b.menu_cod_par = e.menu_cod)              ");	
                sbQuery.AppendLine(" LEFT JOIN ( SELECT *                           ");     
                sbQuery.AppendLine("             FROM	SCHDRT_DTL                  ");     
                sbQuery.AppendLine("             WHERE 	menu_cod in (               ");    	
                sbQuery.AppendLine("                             SELECT menu_cod as GenreCode   ");
                sbQuery.AppendLine("                             FROM SCHDRT_DTL                ");
                sbQuery.AppendLine("                             WHERE item_no = '" + ratioModel.ItemNo.Trim() +"' ");
                sbQuery.AppendLine("                             MINUS              ");
                sbQuery.AppendLine("                             SELECT Genre       "); 
                sbQuery.AppendLine("                             FROM (             ");
                sbQuery.AppendLine("                                 SELECT  menu_cod as Genre  ");
                sbQuery.AppendLine("                                 FROM	SCHD_MENU           ");
                sbQuery.AppendLine("                                 WHERE 	item_no = '" + ratioModel.ItemNo.Trim() +"' ");
                sbQuery.AppendLine("                                 UNION ALL                  ");
                sbQuery.AppendLine("                                 SELECT  title_no as  Genre ");    
                sbQuery.AppendLine("                                 FROM   SCHD_TITLE          ");
                sbQuery.AppendLine("                                 WHERE 	item_no = '" + ratioModel.ItemNo.Trim() +"' ");
                sbQuery.AppendLine("                             )                  ");
                sbQuery.AppendLine("                     )                          ");
                sbQuery.AppendLine("              AND	 item_no = '" + ratioModel.ItemNo.Trim() +"' ");
                sbQuery.AppendLine("         ) v on b.menu_cod = v.menu_cod         ");
                sbQuery.AppendLine(" WHERE 1=1   	                ");
                sbQuery.AppendLine(" AND a.entry_seq = 1            ");

                // ä�μ·����� ����������
				if(ratioModel.ItemNo.Trim().Length > 0 && !ratioModel.ItemNo.Trim().Equals("00"))
				{
					sbQuery.AppendLine(" AND A.item_no = '" + ratioModel.ItemNo.Trim() + "' \n");
				}		
												
				//sbQuery.Append("\n  GROUP BY a.ChannelNo,b.TotalSeries,b.Title,convert(char(10), a.ModDt, 120),a.MediaCode,a.CategoryCode,CategoryName,a.GenreCode,d.GenreName,genreName ");
				sbQuery.Append("\n ORDER BY A.entry_seq, A.item_no");

				// __DEBUG__
				_log.Debug("MediaCode:[" + ratioModel.MediaCode + "]");
				_log.Debug("CategoryCode:[" + ratioModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + ratioModel.GenreCode + "]");
				_log.Debug("ItemNo:[" + ratioModel.ItemNo + "]");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				ratioModel.Group1DataSet = ds.Copy();
				// ���
				ratioModel.ResultCnt = Utility.GetDatasetCount(ratioModel.Group1DataSet);
				// ����ڵ� ��Ʈ
				ratioModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ratioModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroup1List() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ratioModel.ResultCD = "3000";
				ratioModel.ResultDesc = "�׷�1��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		/// �׷�2�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup2List(HeaderModel header, RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroup2List() Start");
				_log.Debug("-----------------------------------------");
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");				
				_log.Debug("MediaCode:[" + ratioModel.MediaCode + "]");
				_log.Debug("CategoryCode:[" + ratioModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + ratioModel.GenreCode + "]");
				_log.Debug("ItemNo:[" + ratioModel.ItemNo + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

                // ��������
                #region ���� �Ұ�
                /*
                sbQuery.Append("\n"
                    + " SELECT	   'False' AS CheckYn                    \n"
                    + "			   ,a.ItemNo                 			 \n"
                    + "			   ,a.MediaCode							 \n"
                    + "			   ,a.EntrySeq							 \n"
                    + "			   ,a.EntryRate							 \n"
                    + "			   ,isnull(v.GenreCode,-1) as EntryYN    \n"
                    + "			   ,b.CategoryCode						 \n"
                    + "			   ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),b.CategoryCode))) +  CONVERT(VARCHAR(10),b.CategoryCode) + ' ' + e.CategoryName) AS CategoryName				\n"	
                    + "			   ,b.GenreCode							 \n"
                    + "			   ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),b.GenreCode)))    +  CONVERT(VARCHAR(10),b.GenreCode)    + ' ' + d.GenreName   ) AS GenreName1    		    \n"												 
                    + "   FROM  SchRate a with(noLock) LEFT JOIN SchRateDetail b with(noLock)        		  \n"
                    + "			ON (a.ItemNo = b.ItemNo AND a.MediaCode = b.MediaCode AND a.EntrySeq = b.EntrySeq)            		 \n"				
                    + "   LEFT JOIN Genre d with(noLock)    ON (b.GenreCode = d.GenreCode)            \n"	
                    + "   LEFT JOIN Category e with(noLock) ON (b.CategoryCode = e.CategoryCode)   \n"	
                    + "   left join ( select * \n"
                    + "   			  from	SchRateDetail \n"
                    + "   			  where	GenreCode in(	select GenreCode from dbo.SchRateDetail nolock where ItemNo = '" + ratioModel.ItemNo.Trim() + "' \n"
                    + "   									except \n"
                    + "   									select Genre \n"
                    + "   									from ( \n"
                    + "   											select  GenreCode as Genre \n"
                    + "   											from	SchChoiceMenuDetail    with(noLock)  \n"
                    + "   											where	ItemNo = '" + ratioModel.ItemNo.Trim() + "'\n"
                    + "   											union all \n"
                    + "   											select  Genre \n"
                    + "   											from	v_adv_contentset a with(nolock) \n"
                    + "   											inner join SchChoiceChannelDetail b with(nolock) on b.ChannelNo = a.Channel \n"
                    + "   											where	b.ItemNo = '" + ratioModel.ItemNo.Trim() + "' ) v ) \n"
                    + "   			and		ItemNo = '" + ratioModel.ItemNo.Trim() + "' ) v on b.GenreCode = v.GenreCode  \n"
                    + "   WHERE 1=1   \n"	
                    + "   AND a.EntrySeq = 2   \n"	
                    );													

				// ä�μ·����� ����������
				if(ratioModel.ItemNo.Trim().Length > 0 && !ratioModel.ItemNo.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.ItemNo = '" + ratioModel.ItemNo.Trim() + "' \n");
				}		
												
//				sbQuery.Append("\n  GROUP BY a.ChannelNo,b.TotalSeries,b.Title,convert(char(10), a.ModDt, 120),a.MediaCode,a.CategoryCode,CategoryName,a.GenreCode,d.GenreName,genreName ");
				sbQuery.Append("\n ORDER BY A.EntrySeq, A.ItemNo");
			    */
                #endregion
                sbQuery.AppendLine(" SELECT	   'False' AS CheckYn                   ");
                sbQuery.AppendLine("             ,a.item_no as ItemNo               ");
                sbQuery.AppendLine("             ,'' as MediaCode					");
                sbQuery.AppendLine("             ,a.entry_seq as EntrySeq			");
                sbQuery.AppendLine("             ,a.entry_rt as EntryRate			");
                sbQuery.AppendLine("             ,NVL(v.menu_cod,-1) as EntryYN     ");
                sbQuery.AppendLine("             ,b.menu_cod_par as CategoryCode	");
                sbQuery.AppendLine("             ,(SUBSTR(b.menu_cod_par,5,5) || ' ' || e.menu_nm) AS CategoryName	");
                sbQuery.AppendLine("             ,b.menu_cod as GenreCode			                                ");
                sbQuery.AppendLine("             ,( SUBSTR(b.menu_cod,5,5) ||  ' ' || d.menu_nm   ) AS GenreName1   ");
                sbQuery.AppendLine(" FROM  SCHDRT_MST a                                                             ");
                sbQuery.AppendLine(" LEFT JOIN SCHDRT_DTL b 	ON (a.item_no = b.item_no AND a.entry_seq = b.entry_seq) ");
                sbQuery.AppendLine(" LEFT JOIN MENU_COD d ON (b.menu_cod = d.menu_cod)            	    ");
                sbQuery.AppendLine(" LEFT JOIN MENU_COD e ON (b.menu_cod_par = e.menu_cod)              ");
                sbQuery.AppendLine(" LEFT JOIN ( SELECT *                           ");
                sbQuery.AppendLine("             FROM	SCHDRT_DTL                  ");
                sbQuery.AppendLine("             WHERE 	menu_cod in (               ");
                sbQuery.AppendLine("                             SELECT menu_cod as GenreCode   ");
                sbQuery.AppendLine("                             FROM SCHDRT_DTL                ");
                sbQuery.AppendLine("                             WHERE item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                             MINUS              ");
                sbQuery.AppendLine("                             SELECT Genre       ");
                sbQuery.AppendLine("                             FROM (             ");
                sbQuery.AppendLine("                                 SELECT  menu_cod as Genre  ");
                sbQuery.AppendLine("                                 FROM	SCHD_MENU           ");
                sbQuery.AppendLine("                                 WHERE 	item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                                 UNION ALL                  ");
                sbQuery.AppendLine("                                 SELECT  title_no as  Genre ");
                sbQuery.AppendLine("                                 FROM   SCHD_TITLE          ");
                sbQuery.AppendLine("                                 WHERE 	item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                             )                  ");
                sbQuery.AppendLine("                     )                          ");
                sbQuery.AppendLine("              AND	 item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("         ) v on b.menu_cod = v.menu_cod         ");
                sbQuery.AppendLine(" WHERE 1=1   	                ");
                sbQuery.AppendLine(" AND a.entry_seq = 2            ");

                // ä�μ·����� ����������
                if (ratioModel.ItemNo.Trim().Length > 0 && !ratioModel.ItemNo.Trim().Equals("00"))
                {
                    sbQuery.AppendLine(" AND A.item_no = '" + ratioModel.ItemNo.Trim() + "' \n");
                }

                //sbQuery.Append("\n  GROUP BY a.ChannelNo,b.TotalSeries,b.Title,convert(char(10), a.ModDt, 120),a.MediaCode,a.CategoryCode,CategoryName,a.GenreCode,d.GenreName,genreName ");
                sbQuery.Append("\n ORDER BY A.entry_seq, A.item_no");
                _log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				ratioModel.Group2DataSet = ds.Copy();
				// ���
				ratioModel.ResultCnt = Utility.GetDatasetCount(ratioModel.Group2DataSet);
				// ����ڵ� ��Ʈ
				ratioModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ratioModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroup2List() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ratioModel.ResultCD = "3000";
				ratioModel.ResultDesc = "�׷�2��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		/// �׷�3�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup3List(HeaderModel header, RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroup3List() Start");
				_log.Debug("-----------------------------------------");
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");				
				_log.Debug("MediaCode:[" + ratioModel.MediaCode + "]");
				_log.Debug("CategoryCode:[" + ratioModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + ratioModel.GenreCode + "]");
				_log.Debug("ItemNo:[" + ratioModel.ItemNo + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// ��������                
                sbQuery.AppendLine(" SELECT	   'False' AS CheckYn                   ");
                sbQuery.AppendLine("             ,a.item_no as ItemNo               ");
                sbQuery.AppendLine("             ,'' as MediaCode					");
                sbQuery.AppendLine("             ,a.entry_seq as EntrySeq			");
                sbQuery.AppendLine("             ,a.entry_rt as EntryRate			");
                sbQuery.AppendLine("             ,NVL(v.menu_cod,-1) as EntryYN     ");
                sbQuery.AppendLine("             ,b.menu_cod_par as CategoryCode	");
                sbQuery.AppendLine("             ,(SUBSTR(b.menu_cod_par,5,5) || ' ' || e.menu_nm) AS CategoryName	");
                sbQuery.AppendLine("             ,b.menu_cod as GenreCode			                                ");
                sbQuery.AppendLine("             ,( SUBSTR(b.menu_cod,5,5) ||  ' ' || d.menu_nm   ) AS GenreName1   ");
                sbQuery.AppendLine(" FROM  SCHDRT_MST a                                                             ");
                sbQuery.AppendLine(" LEFT JOIN SCHDRT_DTL b 	ON (a.item_no = b.item_no AND a.entry_seq = b.entry_seq) ");
                sbQuery.AppendLine(" LEFT JOIN MENU_COD d ON (b.menu_cod = d.menu_cod)            	    ");
                sbQuery.AppendLine(" LEFT JOIN MENU_COD e ON (b.menu_cod_par = e.menu_cod)              ");
                sbQuery.AppendLine(" LEFT JOIN ( SELECT *                           ");
                sbQuery.AppendLine("             FROM	SCHDRT_DTL                  ");
                sbQuery.AppendLine("             WHERE 	menu_cod in (               ");
                sbQuery.AppendLine("                             SELECT menu_cod as GenreCode   ");
                sbQuery.AppendLine("                             FROM SCHDRT_DTL                ");
                sbQuery.AppendLine("                             WHERE item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                             MINUS              ");
                sbQuery.AppendLine("                             SELECT Genre       ");
                sbQuery.AppendLine("                             FROM (             ");
                sbQuery.AppendLine("                                 SELECT  menu_cod as Genre  ");
                sbQuery.AppendLine("                                 FROM	SCHD_MENU           ");
                sbQuery.AppendLine("                                 WHERE 	item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                                 UNION ALL                  ");
                sbQuery.AppendLine("                                 SELECT  title_no as  Genre ");
                sbQuery.AppendLine("                                 FROM   SCHD_TITLE          ");
                sbQuery.AppendLine("                                 WHERE 	item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                             )                  ");
                sbQuery.AppendLine("                     )                          ");
                sbQuery.AppendLine("              AND	 item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("         ) v on b.menu_cod = v.menu_cod         ");
                sbQuery.AppendLine(" WHERE 1=1   	                ");
                sbQuery.AppendLine(" AND a.entry_seq = 3            ");

                // ä�μ·����� ����������
                if (ratioModel.ItemNo.Trim().Length > 0 && !ratioModel.ItemNo.Trim().Equals("00"))
                {
                    sbQuery.AppendLine(" AND A.item_no = '" + ratioModel.ItemNo.Trim() + "' \n");
                }

                //sbQuery.Append("\n  GROUP BY a.ChannelNo,b.TotalSeries,b.Title,convert(char(10), a.ModDt, 120),a.MediaCode,a.CategoryCode,CategoryName,a.GenreCode,d.GenreName,genreName ");
                sbQuery.Append("\n ORDER BY A.entry_seq, A.item_no");
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				ratioModel.Group3DataSet = ds.Copy();
				// ���
				ratioModel.ResultCnt = Utility.GetDatasetCount(ratioModel.Group3DataSet);
				// ����ڵ� ��Ʈ
				ratioModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ratioModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroup3List() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ratioModel.ResultCD = "3000";
				ratioModel.ResultDesc = "�׷�3��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		/// �׷�4�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup4List(HeaderModel header, RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroup4List() Start");
				_log.Debug("-----------------------------------------");
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");				
				_log.Debug("MediaCode:[" + ratioModel.MediaCode + "]");
				_log.Debug("CategoryCode:[" + ratioModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + ratioModel.GenreCode + "]");
				_log.Debug("ItemNo:[" + ratioModel.ItemNo + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

                sbQuery.AppendLine(" SELECT	   'False' AS CheckYn                   ");
                sbQuery.AppendLine("             ,a.item_no as ItemNo               ");
                sbQuery.AppendLine("             ,'' as MediaCode					");
                sbQuery.AppendLine("             ,a.entry_seq as EntrySeq			");
                sbQuery.AppendLine("             ,a.entry_rt as EntryRate			");
                sbQuery.AppendLine("             ,NVL(v.menu_cod,-1) as EntryYN     ");
                sbQuery.AppendLine("             ,b.menu_cod_par as CategoryCode	");
                sbQuery.AppendLine("             ,(SUBSTR(b.menu_cod_par,5,5) || ' ' || e.menu_nm) AS CategoryName	");
                sbQuery.AppendLine("             ,b.menu_cod as GenreCode			                                ");
                sbQuery.AppendLine("             ,( SUBSTR(b.menu_cod,5,5) ||  ' ' || d.menu_nm   ) AS GenreName1   ");
                sbQuery.AppendLine(" FROM  SCHDRT_MST a                                                             ");
                sbQuery.AppendLine(" LEFT JOIN SCHDRT_DTL b 	ON (a.item_no = b.item_no AND a.entry_seq = b.entry_seq) ");
                sbQuery.AppendLine(" LEFT JOIN MENU_COD d ON (b.menu_cod = d.menu_cod)            	    ");
                sbQuery.AppendLine(" LEFT JOIN MENU_COD e ON (b.menu_cod_par = e.menu_cod)              ");
                sbQuery.AppendLine(" LEFT JOIN ( SELECT *                           ");
                sbQuery.AppendLine("             FROM	SCHDRT_DTL                  ");
                sbQuery.AppendLine("             WHERE 	menu_cod in (               ");
                sbQuery.AppendLine("                             SELECT menu_cod as GenreCode   ");
                sbQuery.AppendLine("                             FROM SCHDRT_DTL                ");
                sbQuery.AppendLine("                             WHERE item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                             MINUS              ");
                sbQuery.AppendLine("                             SELECT Genre       ");
                sbQuery.AppendLine("                             FROM (             ");
                sbQuery.AppendLine("                                 SELECT  menu_cod as Genre  ");
                sbQuery.AppendLine("                                 FROM	SCHD_MENU           ");
                sbQuery.AppendLine("                                 WHERE 	item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                                 UNION ALL                  ");
                sbQuery.AppendLine("                                 SELECT  title_no as  Genre ");
                sbQuery.AppendLine("                                 FROM   SCHD_TITLE          ");
                sbQuery.AppendLine("                                 WHERE 	item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                             )                  ");
                sbQuery.AppendLine("                     )                          ");
                sbQuery.AppendLine("              AND	 item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("         ) v on b.menu_cod = v.menu_cod         ");
                sbQuery.AppendLine(" WHERE 1=1   	                ");
                sbQuery.AppendLine(" AND a.entry_seq = 4            ");

                // ä�μ·����� ����������
                if (ratioModel.ItemNo.Trim().Length > 0 && !ratioModel.ItemNo.Trim().Equals("00"))
                {
                    sbQuery.AppendLine(" AND A.item_no = '" + ratioModel.ItemNo.Trim() + "' \n");
                }

                //sbQuery.Append("\n  GROUP BY a.ChannelNo,b.TotalSeries,b.Title,convert(char(10), a.ModDt, 120),a.MediaCode,a.CategoryCode,CategoryName,a.GenreCode,d.GenreName,genreName ");
                sbQuery.Append("\n ORDER BY A.entry_seq, A.item_no");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				ratioModel.Group4DataSet = ds.Copy();
				// ���
				ratioModel.ResultCnt = Utility.GetDatasetCount(ratioModel.Group4DataSet);
				// ����ڵ� ��Ʈ
				ratioModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ratioModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroup4List() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ratioModel.ResultCD = "3000";
				ratioModel.ResultDesc = "�׷�4��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		/// �׷�5�����ȸ
		/// </summary>
		/// <param name="ratioModel"></param>
		public void GetGroup5List(HeaderModel header, RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroup5List() Start");
				_log.Debug("-----------------------------------------");
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");				
				_log.Debug("MediaCode:[" + ratioModel.MediaCode + "]");
				_log.Debug("CategoryCode:[" + ratioModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + ratioModel.GenreCode + "]");
				_log.Debug("ItemNo:[" + ratioModel.ItemNo + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// ��������
                sbQuery.AppendLine(" SELECT	   'False' AS CheckYn                   ");
                sbQuery.AppendLine("             ,a.item_no as ItemNo               ");
                sbQuery.AppendLine("             ,'' as MediaCode					");
                sbQuery.AppendLine("             ,a.entry_seq as EntrySeq			");
                sbQuery.AppendLine("             ,a.entry_rt as EntryRate			");
                sbQuery.AppendLine("             ,NVL(v.menu_cod,-1) as EntryYN     ");
                sbQuery.AppendLine("             ,b.menu_cod_par as CategoryCode	");
                sbQuery.AppendLine("             ,(SUBSTR(b.menu_cod_par,5,5) || ' ' || e.menu_nm) AS CategoryName	");
                sbQuery.AppendLine("             ,b.menu_cod as GenreCode			                                ");
                sbQuery.AppendLine("             ,( SUBSTR(b.menu_cod,5,5) ||  ' ' || d.menu_nm   ) AS GenreName1   ");
                sbQuery.AppendLine(" FROM  SCHDRT_MST a                                                             ");
                sbQuery.AppendLine(" LEFT JOIN SCHDRT_DTL b 	ON (a.item_no = b.item_no AND a.entry_seq = b.entry_seq) ");
                sbQuery.AppendLine(" LEFT JOIN MENU_COD d ON (b.menu_cod = d.menu_cod)            	    ");
                sbQuery.AppendLine(" LEFT JOIN MENU_COD e ON (b.menu_cod_par = e.menu_cod)              ");
                sbQuery.AppendLine(" LEFT JOIN ( SELECT *                           ");
                sbQuery.AppendLine("             FROM	SCHDRT_DTL                  ");
                sbQuery.AppendLine("             WHERE 	menu_cod in (               ");
                sbQuery.AppendLine("                             SELECT menu_cod as GenreCode   ");
                sbQuery.AppendLine("                             FROM SCHDRT_DTL                ");
                sbQuery.AppendLine("                             WHERE item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                             MINUS              ");
                sbQuery.AppendLine("                             SELECT Genre       ");
                sbQuery.AppendLine("                             FROM (             ");
                sbQuery.AppendLine("                                 SELECT  menu_cod as Genre  ");
                sbQuery.AppendLine("                                 FROM	SCHD_MENU           ");
                sbQuery.AppendLine("                                 WHERE 	item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                                 UNION ALL                  ");
                sbQuery.AppendLine("                                 SELECT  title_no as  Genre ");
                sbQuery.AppendLine("                                 FROM   SCHD_TITLE          ");
                sbQuery.AppendLine("                                 WHERE 	item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("                             )                  ");
                sbQuery.AppendLine("                     )                          ");
                sbQuery.AppendLine("              AND	 item_no = '" + ratioModel.ItemNo.Trim() + "' ");
                sbQuery.AppendLine("         ) v on b.menu_cod = v.menu_cod         ");
                sbQuery.AppendLine(" WHERE 1=1   	                ");
                sbQuery.AppendLine(" AND a.entry_seq = 5            ");

                // ä�μ·����� ����������
                if (ratioModel.ItemNo.Trim().Length > 0 && !ratioModel.ItemNo.Trim().Equals("00"))
                {
                    sbQuery.AppendLine(" AND A.item_no = '" + ratioModel.ItemNo.Trim() + "' \n");
                }

                //sbQuery.Append("\n  GROUP BY a.ChannelNo,b.TotalSeries,b.Title,convert(char(10), a.ModDt, 120),a.MediaCode,a.CategoryCode,CategoryName,a.GenreCode,d.GenreName,genreName ");
                sbQuery.Append("\n ORDER BY A.entry_seq, A.item_no");
			

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				ratioModel.Group5DataSet = ds.Copy();
				// ���
				ratioModel.ResultCnt = Utility.GetDatasetCount(ratioModel.Group5DataSet);
				// ����ڵ� ��Ʈ
				ratioModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ratioModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroup5List() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ratioModel.ResultCD = "3000";
				ratioModel.ResultDesc = "�׷�5��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		///  �������� ����
		/// </summary>
		/// <param name="header"></param>
		/// <param name="ratioModel"></param>
		public void SetSchRateUpdate(HeaderModel header, RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchRateUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				
				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				OracleParameter[] sqlParams = new OracleParameter[3];

				sbQuery.Append(""
					+ "UPDATE SCHDRT_MST							\n"
					+ "   SET entry_rt          = :EntryRate       \n"												
					+ " WHERE item_no			 = :ItemNo          \n"										
					+ "   AND entry_seq			 = :EntrySeq        \n"					
					);

				sqlParams[i++] = new OracleParameter(":EntryRate"     , OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);                
                sqlParams[i++] = new OracleParameter(":EntrySeq", OracleDbType.Int32);				
				
				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.EntryRate);	
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.ItemNo);								
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.EntrySeq);				
								
				_log.Debug("ItemNo:[" + ratioModel.ItemNo + "]");				
				_log.Debug("EntrySeq:[" + ratioModel.EntrySeq + "]");
				_log.Debug("EntryRate:[" + ratioModel.EntryRate + "]");
								
				_log.Debug(sbQuery.ToString());			
				

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
					_log.Message("ī�װ���������:["+ratioModel.ItemNo + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				ratioModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchRateUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				ratioModel.ResultCD   = "3101";
				ratioModel.ResultDesc = "������� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}
				
		
		/// <summary>
		/// ���� ���
		/// </summary>
		/// <param name="header"></param>
		/// <param name="ratioModel"></param>
		public void SetSchRateCreate(HeaderModel header, RatioModel ratioModel)
		{
            int count = 0;
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchRateCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();
                StringBuilder SbQueryCount = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[3];

                //SbQueryCount.Append(" \n"
                //    + " SELECT COUNT(*)              \n"
                //    + "   FROM SchDesignatedDetail   \n"
                //    + "  WHERE ItemNo = :ItemNo      \n"                   
                //    );

				sbQuery.Append( ""
					+ "INSERT INTO SCHDRT_MST (          \n"
					+ "		 item_no                   \n"																				
					+ "		,entry_seq                 \n"															
					+ "		,entry_rt                \n"															
					+ "		,use_yn                  \n"																																									
					+ "      )                        \n"
					+ " VALUES(                       \n"			
					+ "       :ItemNo				  \n"											
					+ "      ,:EntrySeq				  \n"		
					+ "      ,:EntryRate			  \n"		
					+ "      ,'Y'					  \n"																			
					+ "		)						  \n"					
								
					);                		
								
				sqlParams[i++] = new OracleParameter(":ItemNo"     , OracleDbType.Int32);								
				sqlParams[i++] = new OracleParameter(":EntrySeq"     , OracleDbType.Int32);
				sqlParams[i++] = new OracleParameter(":EntryRate"     , OracleDbType.Int32);
				
				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.ItemNo);								
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.EntrySeq);
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.EntryRate);	
								
				_log.Debug("ItemNo:[" + ratioModel.ItemNo + "]");				
				_log.Debug("EntrySeq:[" + ratioModel.EntrySeq + "]");
				_log.Debug("EntryRate:[" + ratioModel.EntryRate + "]");
								
				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
                    //DataSet ds = new DataSet();
                    //_db.ExecuteQueryParams(ds, SbQueryCount.ToString(), sqlParams);

                    //count = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                    //// �̹� ������� �����Ͱ� �ִٸ� Exception�� �߻���Ų��.
                    //if (count > 0) throw new Exception();

                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
//					_log.Message("ä�μ���������:[" + ratioModel.ItemNo + "(" + ratioModel.EntrySeq + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				ratioModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchRateCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				ratioModel.ResultCD   = "3101";
                if (count > 0)
                {
                    ratioModel.ResultDesc = "�������� ��ϵ� ����� �������� �����Ҽ� �����ϴ�.";
                }
                else
                {
                    ratioModel.ResultDesc = "������� �� ������ �߻��Ͽ����ϴ�";
                }
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// ���� ���
		/// </summary>
		/// <param name="header"></param>
		/// <param name="ratioModel"></param>
		public void SetSchRateDetailCreate(HeaderModel header, RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchRateDetailCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				OracleParameter[] sqlParams = new OracleParameter[4];
				
				sbQuery.Append( ""
					+ "INSERT INTO SCHDRT_DTL (          \n"
					+ "		 item_no                   \n"																				
					+ "		,entry_seq                 \n"															
					+ "		,menu_cod_par             \n"															
					+ "		,menu_cod                \n"																																									
					+ "      )                        \n"
					+ " VALUES(                       \n"			
					+ "       :ItemNo				  \n"											
					+ "      ,:EntrySeq				  \n"		
					+ "      ,:CategoryCode			  \n"		
					+ "      ,:GenreCode			  \n"																			
					+ "		)						  \n"					
								
					);                		
								
				sqlParams[i++] = new OracleParameter(":ItemNo"     , OracleDbType.Int32);								
				sqlParams[i++] = new OracleParameter(":EntrySeq"     , OracleDbType.Int32);
				sqlParams[i++] = new OracleParameter(":CategoryCode"     , OracleDbType.Varchar2,10);
				sqlParams[i++] = new OracleParameter(":GenreCode"     , OracleDbType.Varchar2,10);
				
				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.ItemNo);								
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.EntrySeq);
				sqlParams[i++].Value = ratioModel.CategoryCode;	
				sqlParams[i++].Value = ratioModel.GenreCode;	
								
				_log.Debug("ItemNo:[" + ratioModel.ItemNo + "]");				
				_log.Debug("EntrySeq:[" + ratioModel.EntrySeq + "]");
				_log.Debug("CategoryCode:[" + ratioModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + ratioModel.GenreCode + "]");
								
				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					//					_log.Message("ä�μ���������:[" + ratioModel.ItemNo + "(" + ratioModel.EntrySeq + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				ratioModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchRateDetailCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				ratioModel.ResultCD   = "3101";
				ratioModel.ResultDesc = "������� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		public void SetSchRateDelete(HeaderModel header, RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchRateDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();
                        
				int i = 0;
				int rc = 0;
				OracleParameter[] sqlParams = new OracleParameter[2];
                        
				sbQuery.Append( ""
					+ "        DELETE FROM  SCHDRT_MST			                    \n"
					+ "              WHERE item_no = :ItemNo				\n"					
					+ "                AND entry_seq = :EntrySeq				\n"					
					);                             
                                        
				sqlParams[i++] = new OracleParameter(":ItemNo"     , OracleDbType.Int32);								
				sqlParams[i++] = new OracleParameter(":EntrySeq"     , OracleDbType.Int32);
											        
				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.ItemNo);								
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.EntrySeq);


				_log.Debug("ItemNo:[" + ratioModel.ItemNo + "]");				
				_log.Debug("EntrySeq:[" + ratioModel.EntrySeq + "]");
				
				_log.Debug(sbQuery.ToString());
                        
				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();
                        
					// __MESSAGE__
//					_log.Message("ä����������:[" + ratioModel.ChannelNo + "] �����:[" + header.UserID + "]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				ratioModel.ResultCD = "0000";  // ����
                        	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchRateDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				ratioModel.ResultCD   = "3301";
				ratioModel.ResultDesc = "���� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}  


        // ��������, Ư���׷��� �帣 ����
		public void SetSchRateDetailDelete(HeaderModel header, RatioModel ratioModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchRateDetailDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();
                        
				int i = 0;
				int rc = 0;
				OracleParameter[] sqlParams = new OracleParameter[4];
                        
				sbQuery.Append( ""
					+ "        DELETE FROM  SCHDRT_DTL			    \n"
					+ "              WHERE item_no = :ItemNo				\n"					
					+ "                AND entry_seq = :EntrySeq			\n"					
					+ "                AND menu_cod_par = :CategoryCode	\n"					
					+ "                AND menu_cod = :GenreCode		\n"					
					);                             
                                        
				sqlParams[i++] = new OracleParameter(":ItemNo"         , OracleDbType.Int32);								
				sqlParams[i++] = new OracleParameter(":EntrySeq"       , OracleDbType.Int32);
				sqlParams[i++] = new OracleParameter(":CategoryCode"   , OracleDbType.Varchar2,10);
				sqlParams[i++] = new OracleParameter(":GenreCode"      , OracleDbType.Varchar2,10);
				
				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.ItemNo);								
				sqlParams[i++].Value = Convert.ToInt32(ratioModel.EntrySeq);
				sqlParams[i++].Value = ratioModel.CategoryCode;	
				sqlParams[i++].Value = ratioModel.GenreCode;	


				_log.Debug("ItemNo:[" + ratioModel.ItemNo + "]");				
				_log.Debug("EntrySeq:[" + ratioModel.EntrySeq + "]");
				_log.Debug("CategoryCode:[" + ratioModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + ratioModel.GenreCode + "]");
				
				_log.Debug(sbQuery.ToString());
                        
				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();
                        
					// __MESSAGE__
					//					_log.Message("ä����������:[" + ratioModel.ChannelNo + "] �����:[" + header.UserID + "]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				ratioModel.ResultCD = "0000";  // ����
                        	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchRateDetailDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				ratioModel.ResultCD   = "3301";
				ratioModel.ResultDesc = "���� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}  	

        /// <summary>
        /// ���� ���� �׷쳻������ �����մϴ�(�������)
        /// </summary>
        /// <param name="header"></param>
        /// <param name="ratioModel"></param>
        public void DeleteSync(HeaderModel header, RatioModel ratioModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "DeleteSync() Start");
                _log.Debug("-----------------------------------------");

                _db.Open();
                        
                StringBuilder sbQuery = new StringBuilder();
                        
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[1];
                /*
                sbQuery.Append("\n delete   SchRateDetail ");
                sbQuery.Append("\n where    GenreCode in(   select GenreCode from dbo.SchRateDetail nolock where ItemNo = @ItemNo ");
                sbQuery.Append("\n 						    except ");
                sbQuery.Append("\n 						    select Genre ");
                sbQuery.Append("\n 						    from ( ");
                sbQuery.Append("\n 								    select  GenreCode as Genre ");
                sbQuery.Append("\n 								    from	SchChoiceMenuDetail    with(noLock)  ");
                sbQuery.Append("\n 								    where	ItemNo = @ItemNo ");
                sbQuery.Append("\n 								    union all ");
                sbQuery.Append("\n 								    select  Genre ");
                sbQuery.Append("\n 								    from	v_adv_contentset a with(nolock) ");
                sbQuery.Append("\n 								    inner join SchChoiceChannelDetail b with(nolock) on b.ChannelNo = a.Channel ");
                sbQuery.Append("\n 								    where	b.ItemNo = @ItemNo ) v ) ");
                sbQuery.Append("\n and		ItemNo = @ItemNo ");
                */
                sbQuery.Append("\n delete   SCHDRT_DTL ");
                sbQuery.Append("\n where    menu_cod in(   select menu_cod from SCHDRT_DTL where item_no = :ItemNo ");
                sbQuery.Append("\n 						    MINUS ");
                sbQuery.Append("\n 						    select Genre ");
                sbQuery.Append("\n 						    from ( ");
                sbQuery.Append("\n 								    select  menu_cod as Genre ");
                sbQuery.Append("\n 								    from	SCHD_MENU          ");
                sbQuery.Append("\n 								    where	item_no = :ItemNo ");                
                sbQuery.Append("\n 								  ) v ) ");
                sbQuery.Append("\n and		item_no = :ItemNo ");

                sqlParams[0] = new SqlParameter(":ItemNo"         , SqlDbType.Int);				
                sqlParams[0].Value = Convert.ToInt32(ratioModel.ItemNo);				

                _log.Debug("ItemNo:[" + ratioModel.ItemNo + "]");
                _log.Debug(sbQuery.ToString());
                        
                // ��������
                try
                {
                    _db.BeginTran();
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();
                        
                    // __MESSAGE__
                    _log.Message("����������:[" + ratioModel.ItemNo + "] �����:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                ratioModel.ResultCD = "0000";  // ����
                        	
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "DeleteSync() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                ratioModel.ResultCD   = "3301";
                ratioModel.ResultDesc = "���� ������ ������ �߻��Ͽ����ϴ�";
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