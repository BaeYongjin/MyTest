// ===============================================================================
//
// StatisticsPgTimeBiz.cs
//
// ����������Ʈ �ð��뺰��� ���� 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: StatisticsPgTimeBiz
 * �ֿ���  : ����������Ʈ �ð��뺰��� ó�� ����
 * �ۼ���    : ��
 * �ۼ���    : ��
 * Ư�̻���  : ����
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : H.J.LEE
 * ������    : 2014.08.19
 * �����κ�  :
 *			  - ������
 *            - ��� ����
 * ��������  : 
 *            - DB ����ȭ �۾����� HanaTV , Summary�� �и���
 *            - Summary�� �ƴ� HanaTV�� �����ϴ� ��� ���̺�,
 *              ���ν��� ���� AdTargetsHanaTV.dbo.XX�� ����
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

namespace AdManagerWebService.ReportMedia
{
    /// <summary>
    /// StatisticsPgTimeBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class StatisticsPgTimeBiz : BaseBiz
    {

		#region  ������
        public StatisticsPgTimeBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region ī�װ���� ��ȸ
		/// <summary>
		/// ī�װ������ȸ
		/// </summary>
		/// <param name="categoryModel"></param>
		public void GetCategoryList(HeaderModel header, StatisticsPgTimeModel statisticsPgTimeModel)
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
				_log.Debug("SearchMediaCode    :[" + statisticsPgTimeModel.SearchMediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT A.MenuCode AS CategoryCode, A.MenuName AS CategoryName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A with(NoLock)                \n"										
					+ "  WHERE A.MediaCode = " + statisticsPgTimeModel.SearchMediaCode +"   \n"			
					+ "    AND A.MenuLevel = 1        \n" 
					+ "  ORDER BY A.MenuCode          \n"
					);
				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ī�װ��𵨿� ����
				statisticsPgTimeModel.CategoryDataSet = ds.Copy();
				// ���
				statisticsPgTimeModel.ResultCnt = Utility.GetDatasetCount(statisticsPgTimeModel.CategoryDataSet);
				// ����ڵ� ��Ʈ
				statisticsPgTimeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + statisticsPgTimeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUsersList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgTimeModel.ResultCD = "3000";
				statisticsPgTimeModel.ResultDesc = "ī�װ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		#endregion

		#region �帣��� ��ȸ
		/// <summary>
		/// �帣�����ȸ
		/// </summary>
		/// <param name="categoryModel"></param>
		public void GetGenreList(HeaderModel header, StatisticsPgTimeModel statisticsPgTimeModel)
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
				_log.Debug("SearchMediaCode    :[" + statisticsPgTimeModel.SearchMediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT A.UpperMenuCode AS CategoryCode, A.MenuCode AS GenreCode, A.MenuName AS GenreName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A  with(NoLock)                \n"										
					+ "  WHERE A.MediaCode = " + statisticsPgTimeModel.SearchMediaCode +"   \n"			
					+ "    AND A.MenuLevel = 2        \n" 
					+ "  ORDER BY A.UpperMenuCode, A.MenuCode \n"
					);
				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				statisticsPgTimeModel.GenreDataSet = ds.Copy();
				// ���
				statisticsPgTimeModel.ResultCnt = Utility.GetDatasetCount(statisticsPgTimeModel.GenreDataSet);
				// ����ڵ� ��Ʈ
				statisticsPgTimeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + statisticsPgTimeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgTimeModel.ResultCD = "3000";
				statisticsPgTimeModel.ResultDesc = "�帣���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		#endregion

		#region �Ⱓ�� �ð��뺰���
        /// <summary>
        ///  �Ⱓ�� �ð��뺰���
        /// </summary>
        /// <param name="statisticsPgTimeModel"></param>
        public void GetStatisticsPgTime(HeaderModel header, StatisticsPgTimeModel statisticsPgTimeModel)
        {
			bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
			bool isNotReady  = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgTime() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(statisticsPgTimeModel.SearchStartDay.Length > 6) statisticsPgTimeModel.SearchStartDay = statisticsPgTimeModel.SearchStartDay.Substring(2,6);
				if(statisticsPgTimeModel.SearchEndDay.Length   > 6) statisticsPgTimeModel.SearchEndDay   = statisticsPgTimeModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	   :[" + statisticsPgTimeModel.SearchMediaCode    + "]");		// �˻� ��ü
				_log.Debug("SearchCategoryCode :[" + statisticsPgTimeModel.SearchCategoryCode + "]");		// �˻� ī�װ��ڵ� 
				_log.Debug("SearchGenreCode    :[" + statisticsPgTimeModel.SearchGenreCode    + "]");		// �˻� �帣�ڵ�           
				_log.Debug("SearchKey          :[" + statisticsPgTimeModel.SearchKey          + "]");		// �˻� Ű(���α׷���)           
				_log.Debug("SearchStartDay     :[" + statisticsPgTimeModel.SearchStartDay     + "]");		// �˻� ������� ����          
				_log.Debug("SearchEndDay       :[" + statisticsPgTimeModel.SearchEndDay       + "]");		// �˻� �������� ����          
				// __DEBUG__

				string MediaCode    = statisticsPgTimeModel.SearchMediaCode;
				string CategoryCode = statisticsPgTimeModel.SearchCategoryCode;
				string GenreCode    = statisticsPgTimeModel.SearchGenreCode;
				string ProgramName  = statisticsPgTimeModel.SearchKey;
				string StartDay     = statisticsPgTimeModel.SearchStartDay;
				string EndDay       = statisticsPgTimeModel.SearchEndDay;

				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* ������ �ð��뺰���    */                 \n"
                    + "DECLARE @TotPgHit int;    -- ��ü �����������  \n"
                    + "SET @TotPgHit    = 0;                      \n"
                    + "                                           \n"
                    + "-- ��ü ����Hit                              \n"
                    + "SELECT @TotPgHit = ISNULL(SUM(A.HitCnt),0) \n"
                    + "  FROM dbo.SumPgDaily0 A   with(NoLock)    \n"
					+ " WHERE A.LogDay BETWEEN "+ StartDay  + "   \n"
					+ "                    AND "+ EndDay    + "   \n"
                    + "   AND A.SummaryType = 5 		          \n"
					+ "                                           \n"
					+ "-- �ð��뺰 ����                             \n"
                    + "SELECT TA.TimeOrder           \n"
                    + "      ,TA.TimeName            \n"
                    + "      ,ISNULL(SUM(TA.PgCnt),0) AS PgCnt    \n"
                    + "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TA.PgCnt),0) / CONVERT(float,@TotPgHit)) * 100)     \n"
                    + "                               ELSE 0 END AS PgRate                                                                      \n"
                    + "      ,REPLICATE('��', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(TA.PgCnt),0)/CONVERT(float,@TotPgHit) * 100),0)   \n"
                    + "                                                ELSE 0 END) AS RateBar                                                   \n"
                    + "  FROM (                                   \n"
					+ "        SELECT TC.RowNum AS TimeOrder     \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN '00��~01��'       \n"
					+ "                              WHEN  1 THEN '01��~02��'       \n"
					+ "                              WHEN  2 THEN '02��~03��'       \n"
					+ "                              WHEN  3 THEN '03��~04��'       \n"
					+ "                              WHEN  4 THEN '04��~05��'       \n"
					+ "                              WHEN  5 THEN '05��~06��'       \n"
					+ "                              WHEN  6 THEN '06��~07��'       \n"
					+ "                              WHEN  7 THEN '07��~08��'       \n"
					+ "                              WHEN  8 THEN '08��~09��'       \n"
					+ "                              WHEN  9 THEN '09��~10��'       \n"
					+ "                              WHEN 10 THEN '10��~11��'       \n"
					+ "                              WHEN 11 THEN '11��~12��'       \n"
					+ "                              WHEN 12 THEN '12��~13��'       \n"
					+ "                              WHEN 13 THEN '13��~14��'       \n"
					+ "                              WHEN 14 THEN '14��~15��'       \n"
					+ "                              WHEN 15 THEN '15��~16��'       \n"
					+ "                              WHEN 16 THEN '16��~17��'       \n"
					+ "                              WHEN 17 THEN '17��~18��'       \n"
					+ "                              WHEN 18 THEN '18��~19��'       \n"
					+ "                              WHEN 19 THEN '19��~20��'       \n"
					+ "                              WHEN 20 THEN '20��~21��'       \n"
					+ "                              WHEN 21 THEN '21��~22��'       \n"
					+ "                              WHEN 22 THEN '22��~23��'       \n"
					+ "                              WHEN 23 THEN '23��~24��' END AS TimeName  \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN SUM(H00)       \n"
					+ "                              WHEN  1 THEN SUM(H01)       \n"
					+ "                              WHEN  2 THEN SUM(H02)       \n"
					+ "                              WHEN  3 THEN SUM(H03)       \n"
					+ "                              WHEN  4 THEN SUM(H04)       \n"
					+ "                              WHEN  5 THEN SUM(H05)       \n"
					+ "                              WHEN  6 THEN SUM(H06)       \n"
					+ "                              WHEN  7 THEN SUM(H07)       \n"
					+ "                              WHEN  8 THEN SUM(H08)       \n"
					+ "                              WHEN  9 THEN SUM(H09)       \n"
					+ "                              WHEN 10 THEN SUM(H10)       \n"
					+ "                              WHEN 11 THEN SUM(H11)       \n"
					+ "                              WHEN 12 THEN SUM(H12)       \n"
					+ "                              WHEN 13 THEN SUM(H13)       \n"
					+ "                              WHEN 14 THEN SUM(H14)       \n"
					+ "                              WHEN 15 THEN SUM(H15)       \n"
					+ "                              WHEN 16 THEN SUM(H16)       \n"
					+ "                              WHEN 17 THEN SUM(H17)       \n"
					+ "                              WHEN 18 THEN SUM(H18)       \n"
					+ "                              WHEN 19 THEN SUM(H19)       \n"
					+ "                              WHEN 20 THEN SUM(H20)       \n"
					+ "                              WHEN 21 THEN SUM(H21)       \n"
					+ "                              WHEN 22 THEN SUM(H22)       \n"
					+ "                              WHEN 23 THEN SUM(H23) END PgCnt \n"
          	        + "     FROM ( \n"
			        + "		        select LogDay, SUM(H00) H00, SUM(H01) H01, SUM(H02) H02, SUM(H03) H03, SUM(H04) H04, SUM(H05) H05, SUM(H06) H06, SUM(H07) H07, SUM(H08) H08, SUM(H09) H09, SUM(H10) H10, SUM(H11) H11,  \n"
			        + "		        	SUM(H12) H12, SUM(H13) H13, SUM(H14) H14, SUM(H15) H15, SUM(H16) H16, SUM(H17) H17, SUM(H18) H18, SUM(H19) H19, SUM(H20) H20, SUM(H21) H21, SUM(H22) H22, SUM(H23) H23  \n"
			        + "		        from dbo.SumPgDaily0 AA with(nolock)  \n"
                    + "		        WHERE AA.LogDay BETWEEN " + StartDay + " AND " + EndDay + "  \n"
			        + "		        AND AA.SummaryType = 5 \n" );
                    if (!CategoryCode.Equals("00")) // Ư��ī�װ����ý�
                    {
                        sbQuery.Append(" AND AA.Menu1 = " + CategoryCode + "  \n");

                        if (!GenreCode.Equals("00")) // Ư���帣���ý�
                        {
                            sbQuery.Append(" AND AA.Menu2 = " + GenreCode + "  \n");
                        }
                    }
                    if (ProgramName.Trim().Length > 0)
                    {
                        sbQuery.Append(" AND AA.ProgKey IN (SELECT ProgramKey FROM AdTargetsHanaTV.dbo.Program with(NoLock) WHERE MediaCode   = 1 AND ProgramNm = '" + ProgramName + "') \n");
                    }
				sbQuery.Append(""
                    + "		        group by AA.LogDay                    \n"
                    + "         ) A ,(select top 24 (RowNum-1)Rownum from AdTargetsHanaTV.dbo.copy_t) TC                         \n"
					+ "         GROUP BY TC.RowNum                        \n"
					+ "       ) TA                                        \n"
					+ " GROUP BY TimeOrder, TimeName                      \n"
                    + "                                                   \n"
                    + " ORDER BY TimeOrder                                \n"
					+ "                                                   \n"
					);  

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				statisticsPgTimeModel.ReportDataSet = ds.Copy();

				// ���
				statisticsPgTimeModel.ResultCnt = Utility.GetDatasetCount(statisticsPgTimeModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				statisticsPgTimeModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsPgTimeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgTime() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsPgTimeModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsPgTimeModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
				}
				else if(isNotReady)
				{
					statisticsPgTimeModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					statisticsPgTimeModel.ResultDesc = "�ð��뺰��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
					_log.Exception(ex);
				}
            }
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

        }
		#endregion

		#region �Ⱓ�� �ð��뺰��� ��� 
		/// <summary>
		///  �Ⱓ�� �ð��뺰��� ���
		/// </summary>
		/// <param name="statisticsPgTimeModel"></param>
		public void GetStatisticsPgTimeAVG(HeaderModel header, StatisticsPgTimeModel statisticsPgTimeModel)
		{
			bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
			bool isNotReady  = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsPgTimeAVG() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(statisticsPgTimeModel.SearchStartDay.Length > 6) statisticsPgTimeModel.SearchStartDay = statisticsPgTimeModel.SearchStartDay.Substring(2,6);
				if(statisticsPgTimeModel.SearchEndDay.Length   > 6) statisticsPgTimeModel.SearchEndDay   = statisticsPgTimeModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	   :[" + statisticsPgTimeModel.SearchMediaCode    + "]");		// �˻� ��ü
				_log.Debug("SearchCategoryCode :[" + statisticsPgTimeModel.SearchCategoryCode + "]");		// �˻� ī�װ��ڵ� 
				_log.Debug("SearchGenreCode    :[" + statisticsPgTimeModel.SearchGenreCode    + "]");		// �˻� �帣�ڵ�           
				_log.Debug("SearchKey          :[" + statisticsPgTimeModel.SearchKey          + "]");		// �˻� Ű(���α׷���)           
				_log.Debug("SearchStartDay     :[" + statisticsPgTimeModel.SearchStartDay     + "]");		// �˻� ������� ����          
				_log.Debug("SearchEndDay       :[" + statisticsPgTimeModel.SearchEndDay       + "]");		// �˻� �������� ����          
				// __DEBUG__

				string MediaCode    = statisticsPgTimeModel.SearchMediaCode;
				string CategoryCode = statisticsPgTimeModel.SearchCategoryCode;
				string GenreCode    = statisticsPgTimeModel.SearchGenreCode;
				string ProgramName  = statisticsPgTimeModel.SearchKey;
				string StartDay     = statisticsPgTimeModel.SearchStartDay;
				string EndDay       = statisticsPgTimeModel.SearchEndDay;

				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* ������ �ð��뺰���    */                 \n"
					+ "DECLARE @TotPgHit int;    -- ��ü �����������  \n"
					+ "SET @TotPgHit    = 0;                      \n"
					+ "                                           \n"
					+ "                                           \n"
					+ "-- ��ü ����Hit                                 \n"
					+ "SELECT @TotPgHit = ISNULL(AVG(PgCnt),0)         \n"
					+ "  FROM (                                        \n"
					+ "       SELECT LogDay,                           \n"
					+ "              SUM(HitCnt) PgCnt                 \n"
                    + "         FROM dbo.SumPgDaily0  with(NoLock)     \n"
					+ "        WHERE LogDay BETWEEN "+ StartDay  + "   \n"
					+ "                         AND "+ EndDay    + "   \n"
                    + " 	     AND SummaryType = 5		           \n"
					+ "        GROUP BY LogDay                         \n"
					+ "        ) TA                                    \n"
					+ "                                                \n"
					+ "-- �ð��뺰 ����                                  \n"
					+ "SELECT TA.TimeOrder           \n"
					+ "      ,TA.TimeName            \n"
					+ "      ,ISNULL(AVG(TA.PgCnt),0) AS PgCnt    \n"
					+ "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(AVG(TA.PgCnt),0) / CONVERT(float,@TotPgHit)) * 100)     \n"
					+ "                               ELSE 0 END AS PgRate                                                                      \n"
					+ "      ,REPLICATE('��', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(AVG(TA.PgCnt),0)/CONVERT(float,@TotPgHit) * 100),0)   \n"
					+ "                                                ELSE 0 END) AS RateBar                                                   \n"
					+ "  FROM (                                                     \n"
					+ "        SELECT A.LogDAY                                      \n"
					+ "              ,TC.RowNum AS TimeOrder                        \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN '00��~01��'       \n"
					+ "                              WHEN  1 THEN '01��~02��'       \n"
					+ "                              WHEN  2 THEN '02��~03��'       \n"
					+ "                              WHEN  3 THEN '03��~04��'       \n"
					+ "                              WHEN  4 THEN '04��~05��'       \n"
					+ "                              WHEN  5 THEN '05��~06��'       \n"
					+ "                              WHEN  6 THEN '06��~07��'       \n"
					+ "                              WHEN  7 THEN '07��~08��'       \n"
					+ "                              WHEN  8 THEN '08��~09��'       \n"
					+ "                              WHEN  9 THEN '09��~10��'       \n"
					+ "                              WHEN 10 THEN '10��~11��'       \n"
					+ "                              WHEN 11 THEN '11��~12��'       \n"
					+ "                              WHEN 12 THEN '12��~13��'       \n"
					+ "                              WHEN 13 THEN '13��~14��'       \n"
					+ "                              WHEN 14 THEN '14��~15��'       \n"
					+ "                              WHEN 15 THEN '15��~16��'       \n"
					+ "                              WHEN 16 THEN '16��~17��'       \n"
					+ "                              WHEN 17 THEN '17��~18��'       \n"
					+ "                              WHEN 18 THEN '18��~19��'       \n"
					+ "                              WHEN 19 THEN '19��~20��'       \n"
					+ "                              WHEN 20 THEN '20��~21��'       \n"
					+ "                              WHEN 21 THEN '21��~22��'       \n"
					+ "                              WHEN 22 THEN '22��~23��'       \n"
					+ "                              WHEN 23 THEN '23��~24��' END AS TimeName  \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN SUM(H00)       \n"
					+ "                              WHEN  1 THEN SUM(H01)       \n"
					+ "                              WHEN  2 THEN SUM(H02)       \n"
					+ "                              WHEN  3 THEN SUM(H03)       \n"
					+ "                              WHEN  4 THEN SUM(H04)       \n"
					+ "                              WHEN  5 THEN SUM(H05)       \n"
					+ "                              WHEN  6 THEN SUM(H06)       \n"
					+ "                              WHEN  7 THEN SUM(H07)       \n"
					+ "                              WHEN  8 THEN SUM(H08)       \n"
					+ "                              WHEN  9 THEN SUM(H09)       \n"
					+ "                              WHEN 10 THEN SUM(H10)       \n"
					+ "                              WHEN 11 THEN SUM(H11)       \n"
					+ "                              WHEN 12 THEN SUM(H12)       \n"
					+ "                              WHEN 13 THEN SUM(H13)       \n"
					+ "                              WHEN 14 THEN SUM(H14)       \n"
					+ "                              WHEN 15 THEN SUM(H15)       \n"
					+ "                              WHEN 16 THEN SUM(H16)       \n"
					+ "                              WHEN 17 THEN SUM(H17)       \n"
					+ "                              WHEN 18 THEN SUM(H18)       \n"
					+ "                              WHEN 19 THEN SUM(H19)       \n"
					+ "                              WHEN 20 THEN SUM(H20)       \n"
					+ "                              WHEN 21 THEN SUM(H21)       \n"
					+ "                              WHEN 22 THEN SUM(H22)       \n"
					+ "                              WHEN 23 THEN SUM(H23) END PgCnt \n"
          	        + "     FROM ( \n"
			        + "		        select LogDay, SUM(H00) H00, SUM(H01) H01, SUM(H02) H02, SUM(H03) H03, SUM(H04) H04, SUM(H05) H05, SUM(H06) H06, SUM(H07) H07, SUM(H08) H08, SUM(H09) H09, SUM(H10) H10, SUM(H11) H11,  \n"
			        + "		        	SUM(H12) H12, SUM(H13) H13, SUM(H14) H14, SUM(H15) H15, SUM(H16) H16, SUM(H17) H17, SUM(H18) H18, SUM(H19) H19, SUM(H20) H20, SUM(H21) H21, SUM(H22) H22, SUM(H23) H23  \n"
			        + "		        from dbo.SumPgDaily0 AA with(nolock)  \n"
                    + "		        WHERE AA.LogDay BETWEEN " + StartDay + " AND " + EndDay + "  \n"
			        + "		        AND AA.SummaryType = 5 \n" );
                    if (!CategoryCode.Equals("00")) // Ư��ī�װ����ý�
                    {
                        sbQuery.Append(" AND AA.Menu1 = " + CategoryCode + "  \n");

                        if (!GenreCode.Equals("00")) // Ư���帣���ý�
                        {
                            sbQuery.Append(" AND AA.Menu2 = " + GenreCode + "  \n");
                        }
                    }
                    if (ProgramName.Trim().Length > 0)
                    {
                        sbQuery.Append(" AND AA.ProgKey IN (SELECT ProgramKey FROM AdTargetsHanaTV.dbo.Program with(NoLock) WHERE MediaCode   = 1 AND ProgramNm = '" + ProgramName + "') \n");
                    }
				
				sbQuery.Append(""
                    + "		        group by AA.LogDay                    \n"
                    + "         ) A ,(select top 24 (RowNum-1)Rownum from AdTargetsHanaTV.dbo.copy_t) TC                         \n"
					+ "         GROUP BY LogDay, TC.RowNum                \n"
					+ "       ) TA                                        \n"
					+ " GROUP BY TimeOrder, TimeName                      \n"
					+ "                                                   \n"
					+ " ORDER BY TimeOrder                                \n"
					+ "                                                   \n"
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				statisticsPgTimeModel.ReportDataSet = ds.Copy();

				// ���
				statisticsPgTimeModel.ResultCnt = Utility.GetDatasetCount(statisticsPgTimeModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				statisticsPgTimeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + statisticsPgTimeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsPgTimeAVG() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgTimeModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsPgTimeModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
				}
				else if(isNotReady)
				{
					statisticsPgTimeModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					statisticsPgTimeModel.ResultDesc = "�ð��뺰��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
					_log.Exception(ex);
				}
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
		#endregion

    }
}