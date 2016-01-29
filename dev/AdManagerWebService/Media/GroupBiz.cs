/*
 * -------------------------------------------------------
 * Class Name: GroupBiz
 * �ֿ���  : �׷��������� ����
 * �ۼ���    : ��
 * �ۼ���    : ��
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : �躸��
 * ������    : 2013.02
 * ��������  :        
 *            - �˻���� ��ȸ ����
 * �����Լ�  :
 *            - GetGroupList();
 * --------------------------------------------------------
 */
/*
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : Yi Youngil
 * ������    : 2015.06.24
 * ��������  :        
 *            - �˻���� ��ȸ ����
 * �����Լ�  :
 *            
 * --------------------------------------------------------
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Media
{
	/// <summary>
	/// GroupBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class GroupBiz : BaseBiz
	{
		public GroupBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		/// �׷�����ȸ
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetGroupList(HeaderModel header, GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroupList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + groupModel.SearchKey       + "]");
				//_log.Debug("SearchMedia:[" + groupModel.SearchMedia + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������,������
                //sbQuery.Append("\n"
                //    + " SELECT A.MediaCode, A.GroupCode, A.GroupName, A.Comment  \n"								
                //    + "       ,A.UseYn              \n"
                //    + "       ,CASE A.UseYn WHEN 'Y' THEN '���' WHEN 'N' THEN '������' END AS UseYn_N  \n"
                //    + "       ,convert(char(19), A.RegDt, 120) RegDt              \n"
                //    + "       ,convert(char(19), A.ModDt, 120) ModDt              \n"					
                //    + "       ,B.UserName RegName                                 \n"					
                //    + "  FROM GroupMaster A LEFT JOIN SystemUser B ON A.RegId = B.UserId \n                 \n"					
                //    + " WHERE 1 = 1  \n");
                sbQuery.Append("\n select   A.MediaCode     ");
                sbQuery.Append("\n 		,A.GroupCode        ");
                sbQuery.Append("\n 		,A.GroupName        ");
                sbQuery.Append("\n 		,A.Comment 		    ");				
                sbQuery.Append("\n 		,A.UseYn            ");          
                sbQuery.Append("\n 		,CASE A.UseYn WHEN 'Y' THEN '���' WHEN 'N' THEN '������' END AS UseYn_N   ");
                sbQuery.Append("\n 		,convert(char(19), A.RegDt, 120) RegDt              ");
                sbQuery.Append("\n 		,convert(char(19), A.ModDt, 120) ModDt           				 ");
                sbQuery.Append("\n 		,B.UserName RegName ");
                sbQuery.Append("\n 		,isnull( ( select count(*) from GroupDetail c with(nolock) where c.GroupCode = a.GroupCode),0) as SchCount ");
                sbQuery.Append("\n from	GroupMaster a with(noLock)  ");
                sbQuery.Append("\n left outer join SystemUser B with(noLock) on a.RegId = b.UserId ");
                				
				if(groupModel.SearchMedia.Trim().Length > 0 && !groupModel.SearchMedia.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode = '" + groupModel.SearchMedia.Trim() + "' \n");
				}			
                /*  �˻� ����.
				// �˻�� ������
				if (groupModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND A.GroupName      LIKE '%" + groupModel.SearchKey.Trim() + "%' \n");									
				}
				*/
				sbQuery.Append(" ORDER BY A.GroupCode desc \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �׷�𵨿� ����
				groupModel.GroupDataSet = ds.Copy();
				// ���
				groupModel.ResultCnt = Utility.GetDatasetCount(groupModel.GroupDataSet);
				// ����ڵ� ��Ʈ
				groupModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + groupModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroupList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				groupModel.ResultCD = "3000";
				groupModel.ResultDesc = "�׷����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// �׷�����ȸ
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetGroupDetailList(HeaderModel header, GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroupDetailList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + groupModel.SearchKey       + "]");
				_log.Debug("SearchMedia:[" + groupModel.SearchMedia + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n");
				sbQuery.Append(" SELECT  'False' AS CheckYn \n");
				sbQuery.Append("		,A.CategoryCode	as Category		,dbo.ufnGetCategoryName( 1, a.CategoryCode)	as CategoryName \n");
				sbQuery.Append(" 		,A.GenreCode	as Genre		,dbo.ufnGetGenreName( 1, a.GenreCode)		as GenreName \n");
				sbQuery.Append(" 		,A.ChannelNo	as Channel		,dbo.ufnGetChannelName( 1, a.ChannelNo)		as ChannelName \n");
				sbQuery.Append(" 		,A.SeriesNo		as Series		,'' 	as SeriesName \n");
				sbQuery.Append(" 		,A.SchType \n");
				sbQuery.Append(" FROM ( \n");
				sbQuery.Append("		SELECT	 CategoryCode \n");
				sbQuery.Append(" 				,0	as GenreCode \n");
				sbQuery.Append(" 				,0	as ChannelNo \n");
				sbQuery.Append(" 				,0	as SeriesNo \n");
				sbQuery.Append(" 				,0	as SchType \n");
				sbQuery.Append(" 		FROM	GroupDetail a with(noLock) \n");
				sbQuery.Append(" 		where	GroupCode	= " + groupModel.SearchGroup.Trim() + " \n");
				sbQuery.Append(" 		and		CategoryCode> 0 \n");
				sbQuery.Append(" 		and		GenreCode	= 0 \n");
				sbQuery.Append(" 		and		ChannelNo	= 0 \n");
				sbQuery.Append(" 		and		SeriesNo	= 0 \n");
				sbQuery.Append(" 		union all \n");
				sbQuery.Append(" 		SELECT	 CategoryCode \n");
				sbQuery.Append(" 				,GenreCode \n");
				sbQuery.Append(" 				,0	as ChannelNo \n");
				sbQuery.Append(" 				,0	as SeriesNo \n");
				sbQuery.Append(" 				,1	as SchType \n");
				sbQuery.Append(" 		FROM GroupDetail a with(noLock) \n");
				sbQuery.Append(" 		where	GroupCode	= " + groupModel.SearchGroup.Trim() + " \n");
				sbQuery.Append(" 		and		CategoryCode> 0 \n");
				sbQuery.Append(" 		and		GenreCode	> 0 \n");
				sbQuery.Append(" 		and		ChannelNo	= 0 \n");
				sbQuery.Append(" 		and		SeriesNo	= 0 \n");
				sbQuery.Append(" 		union all \n");
				sbQuery.Append(" 		SELECT	 CategoryCode \n");
				sbQuery.Append(" 				,GenreCode \n");
				sbQuery.Append(" 				,ChannelNo \n");
				sbQuery.Append(" 				,0	as SeriesNo \n");
				sbQuery.Append(" 				,2	as SchType \n");
				sbQuery.Append(" 		FROM GroupDetail a with(noLock) \n");
				sbQuery.Append(" 		where	GroupCode	= " + groupModel.SearchGroup.Trim() + " \n");
				sbQuery.Append(" 		and		CategoryCode> 0 \n");
				sbQuery.Append(" 		and		GenreCode	> 0 \n");
				sbQuery.Append(" 		and		ChannelNo	> 0 \n");
				sbQuery.Append(" 		and		SeriesNo	= 0 \n");
				sbQuery.Append(" 		union all \n");
				sbQuery.Append(" 		SELECT	 CategoryCode \n");
				sbQuery.Append(" 				,GenreCode \n");
				sbQuery.Append(" 				,ChannelNo \n");
				sbQuery.Append(" 				, SeriesNo \n");
				sbQuery.Append(" 				, 3 as SchType \n");
				sbQuery.Append(" 		FROM GroupDetail a with(noLock) \n");
				sbQuery.Append(" 		where	GroupCode	= " + groupModel.SearchGroup.Trim() + " \n");
				sbQuery.Append(" 		and		CategoryCode> 0 \n");
				sbQuery.Append(" 		and		GenreCode	> 0 \n");
				sbQuery.Append(" 		and		ChannelNo	> 0 \n");
				sbQuery.Append(" 		and		SeriesNo	> 0 ) as a   \n");
				sbQuery.Append(" Order by a.CategoryCode, a.GenreCode, a.ChannelNo, a.SeriesNo;   \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �׷�𵨿� ����
				groupModel.GroupDetailDataSet = ds.Copy();
				// ���
				groupModel.ResultCnt = Utility.GetDatasetCount(groupModel.GroupDetailDataSet);
				// ����ڵ� ��Ʈ
				groupModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + groupModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGroupDetailList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				groupModel.ResultCD = "3000";
				groupModel.ResultDesc = "�׷����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


        /// <summary>
        /// ī�װ������ȸ
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetCategoryList(HeaderModel header, GroupModel groupModel)
        {
            try
            {
                _db.Open();
                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + " SELECT A.MediaCode				  \n"
                    + "		  ,B.MediaName                \n"
                    + "		  ,A.CategoryCode                \n"
                    + "		  ,A.CategoryName                \n"
                    + "  FROM Category A with(noLock) , Media B with(noLock)  \n"
                    + "  WHERE A.MediaCode = B.MediaCode    \n"
                    + "  AND   A.CategoryCode = 1 \n"
                    + " Union all \n"
                    + " SELECT A.MediaCode				  \n"
                    + "		  ,B.MediaName                \n"
                    + "		  ,A.CategoryCode                \n"
                    + "		  ,A.CategoryName                \n"
                    + "  FROM Category A with(noLock) , Media B with(noLock)  \n"
                    + "  WHERE A.MediaCode = B.MediaCode    \n"
                    + "  AND   A.CategoryCode > 1 \n"
                    + "  AND   A.Flag = 'Y' \n");

                // �˻�� ������
                if (groupModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append(" AND ("
                        + "    A.CategoryName      LIKE '%" + groupModel.SearchKey.Trim() + "%' \n"
                        + " OR B.MediaName    LIKE '%" + groupModel.SearchKey.Trim() + "%' \n"
                        + " ) ");
                }

                sbQuery.Append(" ORDER BY A.MediaCode, A.CategoryCode \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� ī�װ��𵨿� ����
                groupModel.CategoryDataSet = ds.Copy();
                // ���
                groupModel.ResultCnt = Utility.GetDatasetCount(groupModel.CategoryDataSet);
                // ����ڵ� ��Ʈ
                groupModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + groupModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUsersList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                groupModel.ResultCD = "3000";
                groupModel.ResultDesc = "ī�װ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        /// <summary>
        /// ī�װ������ȸ E02
        /// ī�װ������ȸ ���� - SearchType �߰� �˻�� ���Ե� �帣 ��ϸ� ����
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetCategoryList2(HeaderModel header, GroupModel groupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryList() Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchType      :[" + groupModel.SearchType + "]");
                _log.Debug("SearchKey      :[" + groupModel.SearchKey + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������


                /* ���� ���� 30�� ����  ������Ʈ ���� ���� �޴��� ��ȿ �޴��� ���� - �޴� ������ �����ϰ� ���� - 2015.05.26 - Youngil.Yi*/

                sbQuery.Append("    SELECT     Distinct                                                                                         \n");
                sbQuery.Append("         A.CategoryCode						                                                                    \n");
                sbQuery.Append("        ,C.CategoryName    		                                                                                \n");
                sbQuery.Append("        ,'Y'  AS InvalidYn                                                                                      \n");
                sbQuery.Append("    FROM ChannelSet A	with(noLock)                                                                            \n");
                sbQuery.Append("		INNER JOIN Category C with(noLock) ON (A.CategoryCode = C.CategoryCode)                             \n");

                if (groupModel.SearchType.Trim().Equals("G") && groupModel.SearchKey.Trim().Length > 0 && !groupModel.SearchKey.Trim().Equals("00"))
                {
                    sbQuery.Append("	INNER JOIN Genre    D with(noLock) ON (A.GenreCode = D.GenreCode )                                  \n");
                }

                if (groupModel.SearchType.Trim().Equals("P") && groupModel.SearchKey.Trim().Length > 0 && !groupModel.SearchKey.Trim().Equals("00"))
                {
                    sbQuery.Append("	INNER  JOIN Channel  E with(noLock) ON (A.ChannelNo = E.ChannelNo)                                   \n");
                }

                if (!groupModel.InvalidYn)
                {
                    if (groupModel.SearchType.Trim().Equals("G") || groupModel.SearchType.Trim().Equals("P"))
                    {
                        sbQuery.Append("    INNER JOIN MENU     M with(noLock) ON (A.GenreCode = M.MenuCode and M.MenuLevel = 2 )            \n");
                    }
                }

                sbQuery.Append("			WHERE C.Flag = 'Y'					 					                                            \n");

                if (groupModel.SearchType.Trim().Equals("C") && groupModel.SearchKey.Trim().Length > 0 && !groupModel.SearchKey.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND C.CategoryName LIKE '%" + groupModel.SearchKey.Trim() + "%' \n");
                }

                if (groupModel.SearchType.Trim().Equals("G") && groupModel.SearchKey.Trim().Length > 0 && !groupModel.SearchKey.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND D.GenreName LIKE '%" + groupModel.SearchKey.Trim() + "%' \n");
                }

                if (groupModel.SearchType.Trim().Equals("P") && groupModel.SearchKey.Trim().Length > 0 && !groupModel.SearchKey.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND E.Title LIKE '%" + groupModel.SearchKey.Trim() + "%' \n");
                }

                if (!groupModel.InvalidYn && (groupModel.SearchType.Trim().Equals("G") || groupModel.SearchType.Trim().Equals("P")))
                {
                    //                    sbQuery.Append("\n  AND M.ModDt > ((Select max(ModDt) From Menu  ) - 10 )  \n");
                    sbQuery.Append("\n  AND M.ModDt > Getdate() - 30  \n");

                }

                sbQuery.Append(" ORDER BY A.CategoryCode \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �𵨿� ����
                groupModel.CategoryDataSet = ds.Copy();
                groupModel.ResultCnt = Utility.GetDatasetCount(groupModel.CategoryDataSet);
                groupModel.ResultCD = "0000";

                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + groupModel.ResultCnt + "]");
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                groupModel.ResultCD = "3000";
                groupModel.ResultDesc = "ī�װ� ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }


        /// <summary>
        ///  �帣����ȸ
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetGenreList(HeaderModel header, GroupModel groupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreList() Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + " SELECT a.GenreCode						     \n"
                    + "       ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.GenreCode)))    +  CONVERT(VARCHAR(10),a.GenreCode)    + ' ' + d.GenreName   ) AS GenreName    		        \n"
                    + "   FROM (											\n"
                    + "				SELECT    a.MediaCode							\n"
                    + "							 ,a.CategoryCode							\n"
                    + "							 ,a.GenreCode							\n"
                    + "				FROM      ChannelSet a							\n"
                    //					+ "				WHERE     a.MediaCode = '" + groupModel.SearchMediaName.Trim() + "'							\n" 
                    + "				GROUP BY  a.MediaCode							\n"
                    + "							,a.CategoryCode							\n"
                    + "							,a.GenreCode							\n"
                    + "				)  a,Media b, Category c, Genre d							\n"
                    + "			 WHERE a.MediaCode = b.MediaCode           		 \n"
                    + "			 AND a.CategoryCode = c.CategoryCode           		 \n"
                    + "			 AND a.GenreCode = d.GenreCode           		 \n"
                    );
                if (groupModel.SearchKey.Trim().Length > 0 && !groupModel.SearchKey.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND D.GenreName LIKE '%" + groupModel.SearchKey.Trim() + "%' \n");
                }
                //				if(groupModel.SearchMediaName.Trim().Length > 0 && !groupModel.SearchMediaName.Trim().Equals("00"))
                //				{
                //					sbQuery.Append(" AND A.MediaCode = '" + groupModel.SearchMediaName.Trim() + "' \n");
                //				}		
                if (groupModel.CategoryCode.Trim().Length > 0 && !groupModel.CategoryCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND A.CategoryCode = '" + groupModel.CategoryCode.Trim() + "' \n");
                }

                sbQuery.Append(" ORDER BY C.CategoryCode,D.GenreCode \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �𵨿� ����
                groupModel.GenreDataSet = ds.Copy();
                groupModel.ResultCnt = Utility.GetDatasetCount(groupModel.GenreDataSet);
                groupModel.ResultCD = "0000";

                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + groupModel.ResultCnt + "]");
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                groupModel.ResultCD = "3000";
                groupModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        ///  �帣 ��ȸ
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetGenreList2(HeaderModel header, GroupModel groupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreList() Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchType      :[" + groupModel.SearchType + "]");
                _log.Debug("SearchKey      :[" + groupModel.SearchKey + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������


                /* ���� ���� 30�� ����  ������Ʈ ���� ���� �޴��� ��ȿ �޴��� ���� - �޴� ������ �����ϰ� ���� - 2015.05.26 - Youngil.Yi*/

                sbQuery.Append("    SELECT  Distinct                                                                                         \n");
                sbQuery.Append("         'False' AS CheckYn                                                                                     \n");
                sbQuery.Append("        ,A.GenreCode						                                                                    \n");
                sbQuery.Append("        ,D.GenreName    		                                                                                \n");
                sbQuery.Append("        ,CASE WHEN (M.ModDt > Getdate() - 30)   THEN 'N' ELSE 'Y' END AS InvalidYn                              \n");
                sbQuery.Append("    FROM ChannelSet A	with(noLock)                                                                            \n");
                sbQuery.Append("			INNER JOIN Genre    D with(noLock) ON (A.GenreCode = D.GenreCode )                                  \n");

                if (groupModel.SearchType.Trim().Equals("P") && groupModel.SearchKey.Trim().Length > 0 && !groupModel.SearchKey.Trim().Equals("00"))
                {
                    sbQuery.Append("			INNER  JOIN Channel  E with(noLock) ON (A.ChannelNo = E.ChannelNo)                                   \n");
                }

                sbQuery.Append("		    INNER  JOIN MENU     M with(noLock) ON (A.GenreCode = M.MenuCode and M.MenuLevel = 2 )            \n");
                sbQuery.Append("			WHERE 1=1					 					                                            \n");

                if (groupModel.CategoryCode.Trim().Length > 0 && !groupModel.CategoryCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND A.CategoryCode = '" + groupModel.CategoryCode.Trim() + "' \n");
                }

                if (groupModel.SearchType.Trim().Equals("G") && groupModel.SearchKey.Trim().Length > 0 && !groupModel.SearchKey.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND D.GenreName LIKE '%" + groupModel.SearchKey.Trim() + "%' \n");
                }

                if (groupModel.SearchType.Trim().Equals("P") && groupModel.SearchKey.Trim().Length > 0 && !groupModel.SearchKey.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND E.Title LIKE '%" + groupModel.SearchKey.Trim() + "%' \n");
                }

                if (!groupModel.InvalidYn)
                {

                    //                    sbQuery.Append("\n  AND M.ModDt > ((Select max(ModDt) From Menu  ) - 10 )  \n");
                    sbQuery.Append("\n  AND M.ModDt > Getdate() - 30  \n");
                }

                sbQuery.Append(" ORDER BY A.GenreCode \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �𵨿� ����
                groupModel.GenreDataSet = ds.Copy();
                groupModel.ResultCnt = Utility.GetDatasetCount(groupModel.GenreDataSet);
                groupModel.ResultCD = "0000";

                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + groupModel.ResultCnt + "]");
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                groupModel.ResultCD = "3000";
                groupModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// ä�μ¸����ȸ
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetChannelNoPopList(HeaderModel header, GroupModel groupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelNoPopList() Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + groupModel.SearchKey + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������				
                sbQuery.Append("\n"
                    + " SELECT a.ChannelNo								\n"
                    + "			,max(a.SeriesNo)		as SeriesNo		\n"
                    + "			,count(b.TotalSeries)	as TotalSeries	\n"
                    + " 		,min(b.Title)			as Title		\n"
                    + " 		,max(convert(char(10), a.ModDt, 120)) as ModDt	 \n"
//                    + "   FROM ChannelSet a LEFT JOIN Channel b            		 \n"
                    + "   FROM ChannelSet a JOIN Channel b            		 \n"
                    + "			ON a.MediaCode = b.MediaCode            		 \n"
                    + "			and a.ChannelNo = b.ChannelNo            		 \n"
                    + "			and a.SeriesNo = b.SeriesNo            		 \n"
                    );

                // ä�μ·����� ����������
                if (groupModel.MediaCode.Trim().Length > 0 && !groupModel.MediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" WHERE A.MediaCode = '" + groupModel.MediaCode.Trim() + "' \n");
                }
                if (groupModel.CategoryCode.Trim().Length > 0 && !groupModel.CategoryCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND A.CategoryCode = '" + groupModel.CategoryCode.Trim() + "' \n");
                }
                if (groupModel.GenreCode.Trim().Length > 0 && !groupModel.GenreCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND A.GenreCode = '" + groupModel.GenreCode.Trim() + "' \n");
                }

                sbQuery.Append(" GROUP BY a.ChannelNo ");
                sbQuery.Append(" ORDER BY A.ChannelNo  \n");

                // __DEBUG__
                _log.Debug("MediaCode:[" + groupModel.MediaCode + "]");
                _log.Debug("CategoryCode:[" + groupModel.CategoryCode + "]");
                _log.Debug("GenreCode:[" + groupModel.GenreCode + "]");
                //_log.Debug("AdvertiserCode:[" + clientModel.AdvertiserCode + "]");

                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� ��ü����籤���ָ𵨿� ����
                groupModel.ChannelDataSet = ds.Copy();
                // ���
                groupModel.ResultCnt = Utility.GetDatasetCount(groupModel.ChannelDataSet);
                // ����ڵ� ��Ʈ
                groupModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelNoPopList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                groupModel.ResultCD = "3000";
                groupModel.ResultDesc = "ä�μ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }


		/// <summary>
		/// ä�μ¸����ȸ
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetChannelNoPopList2(HeaderModel header, GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelNoPopList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
                _log.Debug("SearchType      :[" + groupModel.SearchType + "]");
                _log.Debug("SearchKey      :[" + groupModel.SearchKey + "]");
                // __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// ��������				
				sbQuery.Append("\n"
                    + " SELECT 'False' AS CheckYn                           \n"						
                    + "         ,a.ChannelNo								\n"
                    + "			,max(a.SeriesNo)		as SeriesNo		    \n"
					+ "			,count(b.TotalSeries)	as TotalSeries	    \n"
					+ " 		,min(b.Title)			as Title		    \n"					
					+ " 		,max(convert(char(10), a.ModDt, 120)) as ModDt	 \n"
//                    + "   FROM ChannelSet a LEFT JOIN Channel b with(noLock)           		 \n"
                    + "   FROM ChannelSet a JOIN Channel b with(noLock)           		 \n"
                    + "			ON a.MediaCode = b.MediaCode            		 \n"
					+ "			and a.ChannelNo = b.ChannelNo            		 \n"												
					+ "			and a.SeriesNo = b.SeriesNo            		    \n"												
					);								

				// ä�μ·����� ����������
				if(groupModel.MediaCode.Trim().Length > 0 && !groupModel.MediaCode.Trim().Equals("00"))
				{
					sbQuery.Append(" WHERE A.MediaCode = '" + groupModel.MediaCode.Trim() + "' \n");
				}		
				if(groupModel.CategoryCode.Trim().Length > 0 && !groupModel.CategoryCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.CategoryCode = '" + groupModel.CategoryCode.Trim() + "' \n");
				}		
				if(groupModel.GenreCode.Trim().Length > 0 && !groupModel.GenreCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.GenreCode = '" + groupModel.GenreCode.Trim() + "' \n");
				}

                // �˻�� ������
                if (groupModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append(" AND ("
                        + "   b.Title      LIKE '%" + groupModel.SearchKey.Trim() + "%' \n"
                        + " ) ");
                }

				sbQuery.Append(" GROUP BY a.ChannelNo ");
				sbQuery.Append(" ORDER BY A.ChannelNo  \n");

				// __DEBUG__
				_log.Debug("MediaCode:[" + groupModel.MediaCode + "]");
				_log.Debug("CategoryCode:[" + groupModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + groupModel.GenreCode + "]");
				//_log.Debug("AdvertiserCode:[" + clientModel.AdvertiserCode + "]");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				groupModel.ChannelDataSet = ds.Copy();
				// ���
				groupModel.ResultCnt = Utility.GetDatasetCount(groupModel.ChannelDataSet);
				// ����ڵ� ��Ʈ
				groupModel.ResultCD = "0000";

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelNoPopList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				groupModel.ResultCD = "3000";
				groupModel.ResultDesc = "ä�μ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}
		}

		#region [3] �ø�������ȸ

        /// <summary>
        /// ä�μ¸����ȸ
        /// �޴�/ä����-�޴����ý� �ҷ��� ä�θ���Ʈ������
        /// </summary>
        public void GetSeriesList(HeaderModel header, GroupModel model)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append(" select	 a.SeriesNo " + "\n");
                sbQuery.Append(" 		,c.SubTitle " + "\n");
                sbQuery.Append(" 		,c.ContentsState as cState " + "\n");
                sbQuery.Append(" 		,c.Rate " + "\n");
                sbQuery.Append(" 		,c.Hits " + "\n");
                sbQuery.Append(" 		,c.AdUse " + "\n");
                sbQuery.Append("		,'False'	as IsCheck " + "\n");
                sbQuery.Append("        ,0 AS AdCount	" + "\n");
                sbQuery.Append("        ,0 AS AdFound   " + "\n");
                sbQuery.Append(" from	ChannelSet	a	with(noLock) " + "\n");
                sbQuery.Append(" inner join Channel b	with(noLock)	on b.MediaCode = a.MediaCode and b.ChannelNo = a.ChannelNo and b.SeriesNo = a.SeriesNo " + "\n");
                sbQuery.Append(" inner join Contents c	with(noLock)	on c.ContentId = b.ContentId and c.ContentsState between '30' and '70' " + "\n");
                sbQuery.Append(" where	a.MediaCode     = " + model.MediaCode + "\n");
                sbQuery.Append(" and	a.CategoryCode	= " + model.CategoryCode + "\n");
                sbQuery.Append(" and	a.GenreCode     = " + model.GenreCode + "\n");
                sbQuery.Append(" and	a.ChannelNo		= " + model.ChannelNo + "\n");
                sbQuery.Append(" order by a.SeriesNo desc " + "\n");

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� ��ü����籤���ָ𵨿� ����
                model.SeriesDataSet = ds.Copy();
                model.ResultCnt = Utility.GetDatasetCount(model.SeriesDataSet);
                model.ResultCD = "0000";
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "ȸ������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

		/// <summary>
		/// ä�μ¸����ȸ
		/// �޴�/ä����-�޴����ý� �ҷ��� ä�θ���Ʈ������
		/// </summary>
		public void GetSeriesList2(HeaderModel header, GroupModel groupModel)
		{
			try
			{   // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + groupModel.SearchKey + "]");
                // __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append(" select	 'False' AS CheckYn \n");
                sbQuery.Append("        ,a.SeriesNo " + "\n");
				sbQuery.Append(" 		,c.SubTitle " + "\n");
				sbQuery.Append(" 		,c.ContentsState as cState " + "\n");
				sbQuery.Append(" 		,c.Rate " + "\n");
				sbQuery.Append(" 		,c.Hits " + "\n");
				sbQuery.Append(" 		,c.AdUse " + "\n");
				sbQuery.Append("		,'False'	as IsCheck " + "\n");
				sbQuery.Append("        ,0 AS AdCount	" + "\n");
				sbQuery.Append("        ,0 AS AdFound   " + "\n");
				sbQuery.Append(" from	ChannelSet	a	with(noLock) " + "\n");
				sbQuery.Append(" inner join Channel b	with(noLock)	on b.MediaCode = a.MediaCode and b.ChannelNo = a.ChannelNo and b.SeriesNo = a.SeriesNo " + "\n");
				sbQuery.Append(" inner join Contents c	with(noLock)	on c.ContentId = b.ContentId and c.ContentsState between '30' and '70' " + "\n");
				sbQuery.Append(" where	a.MediaCode     = " + groupModel.MediaCode + "\n");
				sbQuery.Append(" and	a.CategoryCode	= " + groupModel.CategoryCode + "\n");
				sbQuery.Append(" and	a.GenreCode     = " + groupModel.GenreCode + "\n");
				sbQuery.Append(" and	a.ChannelNo		= " + groupModel.ChannelNo + "\n");
				
                
                // �˻�� ������
                if (groupModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append(" AND ("
                        + "   c.SubTitle      LIKE '%" + groupModel.SearchKey.Trim() + "%' \n"
                        + " ) ");
                }

                sbQuery.Append(" order by a.SeriesNo desc " + "\n");

                _log.Debug(sbQuery.ToString());
                // __DEBUG__

				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				groupModel.SeriesDataSet	=	ds.Copy();
				groupModel.ResultCnt		=	Utility.GetDatasetCount( groupModel.SeriesDataSet );
				groupModel.ResultCD		=	"0000";
			}
			catch(Exception ex)
			{
				groupModel.ResultCD = "3000";
				groupModel.ResultDesc = "ȸ������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}
		#endregion


        /// <summary>
        /// �׷캰 ��ü ���������� �����´�
        /// 2009/02/23
        /// </summary>
        /// <param name="header"></param>
        /// <param name="groupModel"></param>
        public void GetGroupMapList(HeaderModel header, GroupModel data)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGroupMapList() Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n");
                sbQuery.Append(" select  v2.Menu1 " + "\n" );
                sbQuery.Append(" 		,v2.Menu2 " + "\n" );
                sbQuery.Append(" 		,v2.MenuName " + "\n" );
                sbQuery.Append(" 		,v2.MenuLevel " + "\n" );
                sbQuery.Append(" 		,v2.Group1  ,isnull( ( select GroupName from GroupMaster noLock where groupCode = v2.Group1),'') as Group1Nm " + "\n" );
                sbQuery.Append(" 		,v2.Group2  ,isnull( ( select GroupName from GroupMaster noLock where groupCode = v2.Group2),'') as Group2Nm " + "\n" );
                sbQuery.Append(" 		,v2.Group3  ,isnull( ( select GroupName from GroupMaster noLock where groupCode = v2.Group3),'') as Group3Nm " + "\n" );
                sbQuery.Append(" 		,v2.Group4  ,isnull( ( select GroupName from GroupMaster noLock where groupCode = v2.Group4),'') as Group4Nm " + "\n" );
                sbQuery.Append(" 		,v2.Group5  ,isnull( ( select GroupName from GroupMaster noLock where groupCode = v2.Group5),'') as Group5Nm " + "\n" );
                sbQuery.Append(" 		,v2.Group6  ,isnull( ( select GroupName from GroupMaster noLock where groupCode = v2.Group6),'') as Group6Nm " + "\n" );
                sbQuery.Append(" 		,v2.Group7  ,isnull( ( select GroupName from GroupMaster noLock where groupCode = v2.Group7),'') as Group7Nm " + "\n" );
                sbQuery.Append(" 		,v2.Group8  ,isnull( ( select GroupName from GroupMaster noLock where groupCode = v2.Group8),'') as Group8Nm " + "\n" );
                sbQuery.Append(" 		,v2.Group9  ,isnull( ( select GroupName from GroupMaster noLock where groupCode = v2.Group9),'') as Group9Nm " + "\n" );
                sbQuery.Append(" from (	select   Menu1 " + "\n" );
                sbQuery.Append("                ,Menu2 " + "\n" );
                sbQuery.Append(" 				,MenuName " + "\n" );
                sbQuery.Append(" 				,MenuLevel " + "\n" );
                sbQuery.Append(" 				,sum(Case RowNum when 1 then isnull(GroupCode,0) else 0 end)	as Group1 " + "\n" );
                sbQuery.Append(" 				,sum(Case RowNum when 2 then isnull(GroupCode,0) else 0 end)	as Group2 " + "\n" );
                sbQuery.Append(" 				,sum(Case RowNum when 3 then isnull(GroupCode,0) else 0 end)	as Group3 " + "\n" );
                sbQuery.Append(" 				,sum(Case RowNum when 4 then isnull(GroupCode,0) else 0 end)	as Group4 " + "\n" );
                sbQuery.Append(" 				,sum(Case RowNum when 5 then isnull(GroupCode,0) else 0 end)	as Group5 " + "\n" );
                sbQuery.Append(" 				,sum(Case RowNum when 6 then isnull(GroupCode,0) else 0 end)	as Group6 " + "\n" );
                sbQuery.Append(" 				,sum(Case RowNum when 7	then isnull(GroupCode,0) else 0 end)	as Group7 " + "\n" );
                sbQuery.Append(" 				,sum(Case RowNum when 8	then isnull(GroupCode,0) else 0 end)	as Group8 " + "\n" );
                sbQuery.Append(" 				,sum(Case RowNum when 9	then isnull(GroupCode,0) else 0 end)	as Group9 " + "\n" );
                sbQuery.Append(" 		from (  select   a.UppermenuCode    as Menu1 " + "\n" );
                sbQuery.Append(" 				        ,a.MenuCode			as Menu2 " + "\n" );
                sbQuery.Append(" 						,a.MenuName			as MenuName " + "\n" );
                sbQuery.Append(" 						,a.MenuLevel		as MenuLevel " + "\n" );
                sbQuery.Append(" 						,b.GroupCode		 " + "\n" );
                sbQuery.Append(" 						,row_number() over( Partition by a.UpperMenuCode,a.MenuCode order by b.GroupCode) as RowNum " + "\n" );
                sbQuery.Append(" 				from  ( select   UpperMenuCode " + "\n" );
                sbQuery.Append(" 						        ,MenuCode " + "\n" );
                sbQuery.Append(" 								,MenuName " + "\n" );
                sbQuery.Append(" 								,MenuLevel " + "\n" );
                sbQuery.Append(" 						from	Menu noLock " + "\n" );
                sbQuery.Append(" 						where	MenuLevel = 1 " + "\n" );
                sbQuery.Append(" 						union all " + "\n" );
                sbQuery.Append(" 						select   UpperMenuCode " + "\n" );
                sbQuery.Append(" 								,MenuCode " + "\n" );
                sbQuery.Append(" 								,'   ' + MenuName " + "\n" );
                sbQuery.Append(" 								,MenuLevel " + "\n" );
                sbQuery.Append(" 						from	Menu noLock " + "\n" );
                sbQuery.Append(" 						where	MenuLevel = 2 ) as a " + "\n" );
                sbQuery.Append(" 				left outer join " + "\n" );
                sbQuery.Append(" 					(	select   GroupCode " + "\n" );
                sbQuery.Append(" 					            ,CategoryCode		as Category " + "\n" );
                sbQuery.Append(" 								,case GenreCode when 0 then CategoryCode else GenreCode end	as Genre " + "\n" );
                sbQuery.Append(" 						from	GroupDetail noLock ) as b " + "\n" );
                sbQuery.Append(" 				on	a.UpperMenuCode = b.Category " + "\n" );
                sbQuery.Append(" 				and	a.MenuCode			= b.Genre ) v " + "\n" );
                sbQuery.Append(" 		inner join " + "\n" );
                sbQuery.Append(" 			(	select   CategoryCode " + "\n" );
                sbQuery.Append(" 						,CategoryCode as GenreCode " + "\n" );
                sbQuery.Append(" 				from	ChannelSet noLock " + "\n" );
                sbQuery.Append(" 				Group By CategoryCode " + "\n" );
                sbQuery.Append(" 				union all " + "\n" );
                sbQuery.Append(" 				select CategoryCode,GenreCode " + "\n" );
                sbQuery.Append(" 				from	ChannelSet noLock " + "\n" );
                sbQuery.Append(" 				Group By CategoryCode,GenreCode) m " + "\n" );
                sbQuery.Append(" 		on	m.CategoryCode = v.Menu1 " + "\n" );
                sbQuery.Append(" 		and	m.GenreCode			= v.Menu2 " + "\n" );
                sbQuery.Append(" 		group by Menu1,Menu2,MenuName,MenuLevel ) v2 " + "\n" );
                sbQuery.Append(" order by Menu1,MenuLevel,Menu2 " + "\n" );
				
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // ��� DataSet�� ī�װ��𵨿� ����
                data.GroupMapDataSet = ds.Copy();
                // ���
                data.ResultCnt = Utility.GetDatasetCount(data.GroupMapDataSet);
                // ����ڵ� ��Ʈ
                data.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + data.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGroupMapList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                data.ResultCD = "3000";
                data.ResultDesc = "�׷�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();			
            }
        }



		/// <summary>
		/// �׷����� ����
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetGroupUpdate(HeaderModel header, GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[6];

				sbQuery.Append(""
					+ "UPDATE GroupMaster                     \n"
					+ "   SET GroupName      = @GroupName      \n"								
					+ "      ,Comment        = @Comment         \n"			
					+ "      ,UseYn		     = @UseYn           \n"	
					+ "      ,ModDt          = GETDATE()         \n"
					+ "      ,RegID          = @RegID           \n"
					+ " WHERE MediaCode      = @MediaCode        \n"
					+ "   AND GroupCode      = @GroupCode        \n"
					);

				i = 0;
				sqlParams[i++] = new SqlParameter("@GroupName"     , SqlDbType.VarChar , 2000);									
				sqlParams[i++] = new SqlParameter("@Comment"  , SqlDbType.VarChar , 2000);		
				sqlParams[i++] = new SqlParameter("@UseYn"  , SqlDbType.Char , 1);
				sqlParams[i++] = new SqlParameter("@RegID"        , SqlDbType.VarChar , 10);
				sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@GroupCode"       , SqlDbType.Int);

				i = 0;
				sqlParams[i++].Value = groupModel.GroupName;							
				sqlParams[i++].Value = groupModel.Comment;				
				sqlParams[i++].Value = groupModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // �����
				sqlParams[i++].Value = Convert.ToInt32(groupModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(groupModel.GroupCode);

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�׷���������:["+groupModel.GroupCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				groupModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				groupModel.ResultCD   = "3201";
				groupModel.ResultDesc = "�׷����� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			// �����ͺ��̽���  Close�Ѵ�
			_db.Close();

		}

		/// <summary>
		/// �帣���� ����
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetGroupGenreUpdate(HeaderModel header, GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupGenreUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[3];

				sbQuery.Append( ""
					+ "INSERT INTO GroupDetail (                         \n"					
					+ "       GroupCode         \n"
					+ "      ,CategoryCode      \n"					
					+ "      ,GenreCode         \n"
					+ "		 ,ChannelNo         \n"																				
					+ "      )                  \n"
					+ " VALUES(                 \n"					
					+ "       @GroupCode        \n"
					+ "      ,@CategoryCode     \n"					
					+ "      ,@GenreCode		\n"					
					+ "      ,0					\n"										
					+ "      )					\n"
					);	

				i = 0;												
				sqlParams[i++] = new SqlParameter("@GroupCode"       , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@CategoryCode"       , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@GenreCode"       , SqlDbType.Int);

				i = 0;																
				sqlParams[i++].Value = Convert.ToInt32(groupModel.GroupCode);
				sqlParams[i++].Value = Convert.ToInt32(groupModel.CategoryCode);
				sqlParams[i++].Value = Convert.ToInt32(groupModel.GenreCode);

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�׷���������:["+groupModel.GroupCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				groupModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupGenreUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				groupModel.ResultCD   = "3201";
				groupModel.ResultDesc = "�׷����� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			// �����ͺ��̽���  Close�Ѵ�
			_db.Close();

		}

		
		/// <summary>
		/// �׷쿡 ä���� �����Ѵ�
		/// </summary>
		/// <param name="header"></param>
		/// <param name="groupModel"></param>
		public void SetGroupChannelUpdate(HeaderModel header, GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupChannelUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[4];

				sbQuery.Append( ""
					+ "INSERT INTO GroupDetail (                         \n"
					+ "       GroupCode         \n"
					+ "      ,CategoryCode      \n"					
					+ "      ,GenreCode         \n"
					+ "		 ,ChannelNo         \n"																				
					+ "      )                  \n"
					+ " VALUES(                 \n"
					+ "       @GroupCode        \n"
					+ "      ,@CategoryCode     \n"					
					+ "      ,@GenreCode		\n"					
					+ "      ,@ChannelNo		\n"										
					+ "      )					\n"
					);	

				i = 0;									
				sqlParams[i++] = new SqlParameter("@GroupCode"       , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@CategoryCode"       , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@GenreCode"       , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@ChannelNo"  , SqlDbType.Int);				

				i = 0;												
				sqlParams[i++].Value = Convert.ToInt32(groupModel.GroupCode);
				sqlParams[i++].Value = Convert.ToInt32(groupModel.CategoryCode);
				sqlParams[i++].Value = Convert.ToInt32(groupModel.GenreCode);
				sqlParams[i++].Value = Convert.ToInt32(groupModel.ChannelNo);

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�׷���������:["+groupModel.GroupCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				groupModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupChannelUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				groupModel.ResultCD   = "3201";
				groupModel.ResultDesc = "�׷����� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			// �����ͺ��̽���  Close�Ѵ�
			_db.Close();

		}



		/// <summary>
		/// �׷쿡 ȸ���� �����Ѵ�
		/// </summary>
		/// <param name="header"></param>
		/// <param name="groupModel"></param>
		public void SetGroupSeriesUpdate(HeaderModel header, GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupSeriesUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				int rc = 0;

				StringBuilder sbQuery = new StringBuilder();
				SqlParameter[] sqlParams = new SqlParameter[5];

				sbQuery.Append( ""
					+ "INSERT INTO GroupDetail (\n"
					+ "			 GroupCode		\n"
					+ "			,CategoryCode	\n"					
					+ "			,GenreCode		\n"
					+ "			,ChannelNo		\n"
					+ "			,SeriesNo	)	\n"
					+ " VALUES(	 @GroupCode		\n"
					+ "			,@CategoryCode  \n"					
					+ "			,@GenreCode		\n"					
					+ "			,@ChannelNo		\n"										
					+ "			,@SeriesNo );	\n" );	

				sqlParams[0] = new SqlParameter("@GroupCode"	, SqlDbType.Int);
				sqlParams[1] = new SqlParameter("@CategoryCode" , SqlDbType.Int);
				sqlParams[2] = new SqlParameter("@GenreCode"    , SqlDbType.Int);
				sqlParams[3] = new SqlParameter("@ChannelNo"	, SqlDbType.Int);				
				sqlParams[4] = new SqlParameter("@SeriesNo"		, SqlDbType.Int);				

				sqlParams[0].Value = Convert.ToInt32(groupModel.GroupCode);
				sqlParams[1].Value = Convert.ToInt32(groupModel.CategoryCode);
				sqlParams[2].Value = Convert.ToInt32(groupModel.GenreCode);
				sqlParams[3].Value = Convert.ToInt32(groupModel.ChannelNo);
				sqlParams[4].Value = Convert.ToInt32(groupModel.SeriesNo);

				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();
					_log.Message("�׷���������:["+groupModel.GroupCode + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				groupModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupSeriesUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				groupModel.ResultCD   = "3201";
				groupModel.ResultDesc = "�׷����� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			// �����ͺ��̽���  Close�Ѵ�
			_db.Close();
		}

		/// <summary>
		/// �׷� ����
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetGroupCreate(HeaderModel header, GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[4];

				sbQuery.Append( ""
					+ "INSERT INTO GroupMaster (                         \n"
					+ "       MediaCode         \n"
					+ "      ,GroupCode         \n"
					+ "      ,GroupName        \n"					
					+ "      ,Comment         \n"
					+ "		 ,UseYn                \n"															
					+ "      ,RegDt         \n"
					+ "      ,ModDt         \n"
					+ "      ,RegID                                     \n"
					+ "      )                                          \n"
					+ " SELECT                                        \n"
					+ "       @MediaCode      \n"				
					+ "      ,ISNULL(MAX(GroupCode),0)+1        \n"
					+ "      ,@GroupName      \n"					
					+ "      ,@Comment      \n"					
					+ "      ,'Y'      \n"					
					+ "      ,GETDATE()      \n"
					+ "      ,GETDATE()      \n"
					+ "      ,@RegID         \n"
					+ "      FROM GroupMaster               \n"
					);
				
				sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt);				
				sqlParams[i++] = new SqlParameter("@GroupName"     , SqlDbType.VarChar , 2000);				
				sqlParams[i++] = new SqlParameter("@Comment"     , SqlDbType.VarChar , 2000);				
				sqlParams[i++] = new SqlParameter("@RegID"        , SqlDbType.VarChar , 10);

				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(groupModel.MediaCode);
				sqlParams[i++].Value = groupModel.GroupName;	
				sqlParams[i++].Value = groupModel.Comment;				
				sqlParams[i++].Value = header.UserID;				// �����

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�׷���������:[" + groupModel.GroupCode + "(" + groupModel.GroupName + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				groupModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				groupModel.ResultCD   = "3101";
				groupModel.ResultDesc = "�׷����� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		/// �׷� ����
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetGroupDetailCreate(HeaderModel header, GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupDetailCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];

				sbQuery.Append( ""
					+ "INSERT INTO GroupDetail (                         \n"
					+ "       GroupCode         \n"
					+ "      ,CategoryCode      \n"					
					+ "      ,GenreCode         \n"
					+ "		 ,ChannelNo         \n"																				
					+ "      )                  \n"
					+ " VALUES(                 \n"
					+ "       @GroupCode        \n"
					+ "      ,@CategoryCode     \n"					
					+ "      ,0					\n"					
					+ "      ,0					\n"										
					+ "      )					\n"
					);
				
				sqlParams[i++] = new SqlParameter("@GroupCode"     , SqlDbType.Int);				
				sqlParams[i++] = new SqlParameter("@CategoryCode"     , SqlDbType.Int);				
				
				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(groupModel.GroupCode);
				sqlParams[i++].Value = Convert.ToInt32(groupModel.CategoryCode);

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�׷���������:[" + groupModel.GroupCode + "(" + groupModel.GroupName + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				groupModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupDetailCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				groupModel.ResultCD   = "3101";
				groupModel.ResultDesc = "�׷����� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}


		public void SetGroupDelete(HeaderModel header, GroupModel groupModel)
		{
			int GroupDetailCount = 0;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQueryGroupDetailCount = new StringBuilder();
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];

				sbQueryGroupDetailCount.Append( "\n"
					+ "        SELECT COUNT(*) FROM    GroupDetail			    \n"
					+ "              WHERE GroupCode  = @GroupCode          	\n"
					);  

				sbQuery.Append(""
					+ "DELETE GroupMaster         \n"
					+ " WHERE MediaCode  = @MediaCode  \n"
					+ "   AND GroupCode  = @GroupCode  \n"
					);

				sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@GroupCode"       , SqlDbType.Int);

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(groupModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(groupModel.GroupCode);

				// ��������
				try
				{
					//��ü���౤���� ���� Count����///////////////////////////////////////////////
					// __DEBUG__
					_log.Debug(sbQueryGroupDetailCount.ToString());
					// __DEBUG__

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQueryGroupDetailCount.ToString(),sqlParams);
                    
					GroupDetailCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					_log.Debug("GroupDetailCount          -->" + GroupDetailCount);

					// �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ� Exception�� �߻���Ų��.
					if(GroupDetailCount > 0) throw new Exception();


					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�׷���������:[" + groupModel.GroupCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				groupModel.ResultCD = "0000";  // ����
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				groupModel.ResultCD   = "3301";
				// �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ�
				if(GroupDetailCount > 0 )
				{
					groupModel.ResultDesc = "��ϵ� ��ü����簡 �����Ƿ� �׷������� �����Ҽ� �����ϴ�.";
				}
				else
				{
					groupModel.ResultDesc = "�׷����� ������ ������ �߻��Ͽ����ϴ�";
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
		/// [S1] ���׷� ���� ����ó��
		/// </summary>
		/// <param name="header"></param>
		/// <param name="groupModel"></param>
		public void SetGroupDetailDelete(HeaderModel header, GroupModel groupModel)
		{
			//int GroupDetailCount	= 0;
			StringBuilder sbQuery	= new StringBuilder();				
			SqlParameter[] sqlParams= new SqlParameter[5];

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupDetailDelete() Start");
				_log.Debug("-----------------------------------------");
				_db.Open();

				#region ��������

				sbQuery.Append(""
					+ "DELETE GroupDetail					\n"
					+ " WHERE GroupCode		= @GroupCode	\n"
					+ "   AND CategoryCode  = @CategoryCode \n"
					+ "   AND GenreCode		= @GenreCode	\n"
					+ "   AND ChannelNo		= @ChannelNo	\n"
					+ "   AND SeriesNo		= @SeriesNo;	\n"	);

				sqlParams[0] = new SqlParameter("@GroupCode"		, SqlDbType.Int);
				sqlParams[1] = new SqlParameter("@CategoryCode"		, SqlDbType.Int);
				sqlParams[2] = new SqlParameter("@GenreCode"		, SqlDbType.Int);
				sqlParams[3] = new SqlParameter("@ChannelNo"		, SqlDbType.Int);
				sqlParams[4] = new SqlParameter("@SeriesNo"			, SqlDbType.Int);

				sqlParams[0].Value = Convert.ToInt32(groupModel.GroupCode);
				sqlParams[1].Value = Convert.ToInt32(groupModel.CategoryCode);
				sqlParams[2].Value = Convert.ToInt32(groupModel.GenreCode);
				sqlParams[3].Value = Convert.ToInt32(groupModel.ChannelNo);
				sqlParams[4].Value = Convert.ToInt32(groupModel.SeriesNo);
				#endregion

				// ��������
				try
				{										
					int rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("�׷���������:[" + groupModel.GroupCode + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					throw ex;
				}

				groupModel.ResultCD		= "0000";
				groupModel.ResultDesc	= "�����Ϸ�";
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGroupDetailDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				groupModel.ResultCD   = "3000";
				groupModel.ResultDesc = "���׷� �������� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}
		}
	}
}
