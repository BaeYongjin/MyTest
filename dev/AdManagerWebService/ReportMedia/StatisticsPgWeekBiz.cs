// ===============================================================================
//
// StatisticsPgWeekBiz.cs
//
// ����������Ʈ ���Ϻ���� ���� 
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
 * Class Name: StatisticsPgWeekBiz
 * �ֿ���  : ����������Ʈ ���Ϻ���� ó�� ����
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
 * -------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : Youngil.Yi
 * ������    : 2014.10.23
 * �����κ�  :
 *			  - GetStatisticsPgWeek()
 * ��������  : 
 * Exception : "�� ������ ���ǵ� ��Ʈ�� ���� ���� ���μ������� ���� ��ȹ�� ������ �� �����ϴ�. ��Ʈ�� �����ϰų� SET FORCEPLAN�� ������� �ʰ� ������ �ٽ� �����Ͻʽÿ�."
 * ó���� ���ؼ� merge�� ����
 * --
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
    /// StatisticsPgWeekBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class StatisticsPgWeekBiz : BaseBiz
    {

		#region  ������
        public StatisticsPgWeekBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region ī�װ���� ��ȸ
		/// <summary>
		/// ī�װ������ȸ
		/// </summary>
		/// <param name="categoryModel"></param>
		public void GetCategoryList(HeaderModel header, StatisticsPgWeekModel statisticsPgWeekModel)
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
				_log.Debug("SearchMediaCode    :[" + statisticsPgWeekModel.SearchMediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT A.MenuCode AS CategoryCode, A.MenuName AS CategoryName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A with(NoLock)                \n"										
					+ "  WHERE A.MediaCode = " + statisticsPgWeekModel.SearchMediaCode +"   \n"			
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
				statisticsPgWeekModel.CategoryDataSet = ds.Copy();
				// ���
				statisticsPgWeekModel.ResultCnt = Utility.GetDatasetCount(statisticsPgWeekModel.CategoryDataSet);
				// ����ڵ� ��Ʈ
				statisticsPgWeekModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + statisticsPgWeekModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUsersList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgWeekModel.ResultCD = "3000";
				statisticsPgWeekModel.ResultDesc = "ī�װ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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
		public void GetGenreList(HeaderModel header, StatisticsPgWeekModel statisticsPgWeekModel)
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
				_log.Debug("SearchMediaCode    :[" + statisticsPgWeekModel.SearchMediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT A.UpperMenuCode AS CategoryCode, A.MenuCode AS GenreCode, A.MenuName AS GenreName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A  with(NoLock)                \n"										
					+ "  WHERE A.MediaCode = " + statisticsPgWeekModel.SearchMediaCode +"   \n"			
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
				statisticsPgWeekModel.GenreDataSet = ds.Copy();
				// ���
				statisticsPgWeekModel.ResultCnt = Utility.GetDatasetCount(statisticsPgWeekModel.GenreDataSet);
				// ����ڵ� ��Ʈ
				statisticsPgWeekModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + statisticsPgWeekModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgWeekModel.ResultCD = "3000";
				statisticsPgWeekModel.ResultDesc = "�帣���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		#endregion

		#region �Ⱓ�� ���Ϻ����
        /// <summary>
        ///  �Ⱓ�� ���Ϻ����
        /// </summary>
        /// <param name="statisticsPgWeekModel"></param>
        public void GetStatisticsPgWeek(HeaderModel header, StatisticsPgWeekModel statisticsPgWeekModel)
        {
			bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
			bool isNotReady  = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgWeek() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(statisticsPgWeekModel.SearchStartDay.Length > 6) statisticsPgWeekModel.SearchStartDay = statisticsPgWeekModel.SearchStartDay.Substring(2,6);
				if(statisticsPgWeekModel.SearchEndDay.Length   > 6) statisticsPgWeekModel.SearchEndDay   = statisticsPgWeekModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	   :[" + statisticsPgWeekModel.SearchMediaCode    + "]");		// �˻� ��ü
				_log.Debug("SearchCategoryCode :[" + statisticsPgWeekModel.SearchCategoryCode + "]");		// �˻� ī�װ��ڵ� 
				_log.Debug("SearchGenreCode    :[" + statisticsPgWeekModel.SearchGenreCode    + "]");		// �˻� �帣�ڵ�           
				_log.Debug("SearchKey          :[" + statisticsPgWeekModel.SearchKey          + "]");		// �˻� Ű(���α׷���)           
				_log.Debug("SearchStartDay     :[" + statisticsPgWeekModel.SearchStartDay     + "]");		// �˻� ������� ����          
				_log.Debug("SearchEndDay       :[" + statisticsPgWeekModel.SearchEndDay       + "]");		// �˻� �������� ����          
				// __DEBUG__

				string MediaCode    = statisticsPgWeekModel.SearchMediaCode;
				string CategoryCode = statisticsPgWeekModel.SearchCategoryCode;
				string GenreCode    = statisticsPgWeekModel.SearchGenreCode;
				string ProgramName  = statisticsPgWeekModel.SearchKey;
				string StartDay     = statisticsPgWeekModel.SearchStartDay;
				string EndDay       = statisticsPgWeekModel.SearchEndDay;

				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* ������ ���Ϻ����    */                     \n"
                    + "DECLARE @TotPgHit int;    -- ��ü ����������� \n"
                    + "SET @TotPgHit    = 0;                       \n"
                    + "                                            \n"
                    + "-- ��ü ����Hit                               \n"
                    + "SELECT @TotPgHit = ISNULL(SUM(A.HitCnt),0)  \n"
                    + "  FROM dbo.SumPgDaily0 A   with(NoLock)     \n"
					+ " WHERE A.LogDay BETWEEN "+ StartDay  + "    \n"
					+ "                    AND "+ EndDay    + "    \n"
                    + " 	  AND A.SummaryType = 5		           \n"
                    + "                                            \n"
					+ "-- ���Ϻ� ����                                \n"
                    + "SELECT TA.WeekOrder                         \n"
                    + "      ,TA.WeekName                          \n"
                    + "      ,ISNULL(SUM(TA.PgCnt),0) AS PgCnt     \n"
                    + "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TA.PgCnt),0) / CONVERT(float,@TotPgHit)) * 100)     \n"
                    + "                               ELSE 0 END AS PgRate                                                                      \n"
                    + "      ,REPLICATE('��', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(TA.PgCnt),0)/CONVERT(float,@TotPgHit) * 100),0)   \n"
                    + "                                                ELSE 0 END) AS RateBar                                                   \n"
                    + "  FROM (                                       \n"
					+ "        SELECT CASE D.week WHEN  1 THEN 7      \n"
					+ "                           WHEN  2 THEN 1      \n"
					+ "                           WHEN  3 THEN 2      \n"
					+ "                           WHEN  4 THEN 3      \n"
					+ "                           WHEN  5 THEN 4      \n"
					+ "                           WHEN  6 THEN 5      \n"
					+ "                           WHEN  7 THEN 6 END AS WeekOrder      \n"
					+ "              ,CASE D.week WHEN  1 THEN '��'      \n"
					+ "                           WHEN  2 THEN '��'      \n"
					+ "                           WHEN  3 THEN 'ȭ'      \n"
					+ "                           WHEN  4 THEN '��'      \n"
					+ "                           WHEN  5 THEN '��'      \n"
					+ "                           WHEN  6 THEN '��'      \n"
					+ "                           WHEN  7 THEN '��' END AS WeekName      \n"
					+ "              ,SUM(HitCnt) AS PgCnt      \n"
                    + "          FROM dbo.SumPgDaily0 A  with(NoLock)                  \n"

// -- 2014/10/23 - Youngil.Yi ���� --  "
// Exception : �� ������ ���ǵ� ��Ʈ�� ���� ���� ���μ������� ���� ��ȹ�� ������ �� �����ϴ�. ��Ʈ�� �����ϰų� SET FORCEPLAN�� ������� �ʰ� ������ �ٽ� �����Ͻʽÿ�.
// ó���� ���ؼ� merge�� ����
//                   + "          INNER merge JOIN SummaryBase D with(NoLock) ON (A.LogDay      = D.LogDay)     \n"
// --

                    + "          INNER JOIN SummaryBase D with(NoLock) ON (A.LogDay      = D.LogDay)     \n"
                    + "         WHERE A.LogDay BETWEEN "+ StartDay  + " \n"
					+ "                            AND "+ EndDay    + " \n"
                    + " 		and A.SummaryType = 5		           \n"
					);

                if (!CategoryCode.Equals("00")) // Ư��ī�װ����ý�
                {
                    sbQuery.Append(" AND A.Menu1 = " + CategoryCode + "  \n");

                    if (!GenreCode.Equals("00")) // Ư���帣���ý�
                    {
                        sbQuery.Append(" AND A.Menu2 = " + GenreCode + "  \n");
                    }
                }
                if (ProgramName.Trim().Length > 0)
                {
                    sbQuery.Append(" AND A.ProgKey IN (SELECT ProgramKey FROM AdTargetsHanaTV.dbo.Program with(NoLock) WHERE MediaCode   = " + MediaCode + " AND ProgramNm = '" + ProgramName + "') \n");
                }
				
				sbQuery.Append(""
					+ "         GROUP BY D.week                           \n"
					+ "       ) TA                                        \n"
					+ " GROUP BY WeekOrder, WeekName                      \n"
                    + "                                                   \n"
                    + " ORDER BY WeekOrder                                \n"
					);  

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				statisticsPgWeekModel.ReportDataSet = ds.Copy();

				// ���
				statisticsPgWeekModel.ResultCnt = Utility.GetDatasetCount(statisticsPgWeekModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				statisticsPgWeekModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsPgWeekModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgWeek() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsPgWeekModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsPgWeekModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
				}
				else if(isNotReady)
				{
					statisticsPgWeekModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					statisticsPgWeekModel.ResultDesc = "���Ϻ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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

		#region �Ⱓ�� ���Ϻ���� ��� 
		/// <summary>
		///  �Ⱓ�� ���Ϻ���� ���
		/// </summary>
		/// <param name="statisticsPgWeekModel"></param>
		public void GetStatisticsPgWeekAVG(HeaderModel header, StatisticsPgWeekModel statisticsPgWeekModel)
		{
			bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
			bool isNotReady  = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsPgWeekAVG() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(statisticsPgWeekModel.SearchStartDay.Length > 6) statisticsPgWeekModel.SearchStartDay = statisticsPgWeekModel.SearchStartDay.Substring(2,6);
				if(statisticsPgWeekModel.SearchEndDay.Length   > 6) statisticsPgWeekModel.SearchEndDay   = statisticsPgWeekModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	   :[" + statisticsPgWeekModel.SearchMediaCode    + "]");		// �˻� ��ü
				_log.Debug("SearchCategoryCode :[" + statisticsPgWeekModel.SearchCategoryCode + "]");		// �˻� ī�װ��ڵ� 
				_log.Debug("SearchGenreCode    :[" + statisticsPgWeekModel.SearchGenreCode    + "]");		// �˻� �帣�ڵ�           
				_log.Debug("SearchKey          :[" + statisticsPgWeekModel.SearchKey          + "]");		// �˻� Ű(���α׷���)           
				_log.Debug("SearchStartDay     :[" + statisticsPgWeekModel.SearchStartDay     + "]");		// �˻� ������� ����          
				_log.Debug("SearchEndDay       :[" + statisticsPgWeekModel.SearchEndDay       + "]");		// �˻� �������� ����          
				// __DEBUG__

				string MediaCode    = statisticsPgWeekModel.SearchMediaCode;
				string CategoryCode = statisticsPgWeekModel.SearchCategoryCode;
				string GenreCode    = statisticsPgWeekModel.SearchGenreCode;
				string ProgramName  = statisticsPgWeekModel.SearchKey;
				string StartDay     = statisticsPgWeekModel.SearchStartDay;
				string EndDay       = statisticsPgWeekModel.SearchEndDay;

				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* ������ ���Ϻ����    */                    \n"
					+ "DECLARE @TotPgHit int;    -- ��ü �����������\n"
					+ "SET @TotPgHit    = 0;                      \n"
					+ "                                           \n"
					+ "-- ��ü ����Hit                                           \n"
					+ "SELECT @TotPgHit = ISNULL(SUM(TA.PgCnt),0)              \n"
					+ "  FROM (                                                \n"
					+ "        SELECT TC.week, AVG(PgCnt) PgCnt                \n"
					+ "          FROM (                                        \n"
					+ "               SELECT LogDay, SUM(HitCnt) PgCnt         \n"
                    + "                 FROM dbo.SumPgDaily0 with(NoLock)      \n"
					+ "                WHERE LogDay BETWEEN "+ StartDay  + "   \n"
					+ "                                 AND "+ EndDay    + "   \n"
                    + " 	             AND SummaryType = 5		           \n"
					+ "                GROUP BY LogDay \n"
					+ "               ) TB \n"
					+ "               INNER JOIN SummaryBase TC with(NoLock) ON (TB.LogDay = TC.LogDay) \n"
					+ "        GROUP BY TC.week \n"
					+ "        ) TA \n"
					+ "                                                        \n"
					+ "-- ���Ϻ� ����                               \n"
					+ "SELECT TA.WeekOrder           \n"
					+ "      ,TA.WeekName            \n"
					+ "      ,ISNULL(AVG(TA.PgCnt),0) AS PgCnt    \n"
					+ "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(AVG(TA.PgCnt),0) / CONVERT(float,@TotPgHit)) * 100)     \n"
					+ "                               ELSE 0 END AS PgRate                                                                      \n"
					+ "      ,REPLICATE('��', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(AVG(TA.PgCnt),0)/CONVERT(float,@TotPgHit) * 100),0)   \n"
					+ "                                                ELSE 0 END) AS RateBar                                                   \n"
					+ "  FROM (                                                     \n"
					+ "        SELECT A.LogDAY                                      \n"
					+ "              ,CASE D.week WHEN  1 THEN 7                    \n"
					+ "                           WHEN  2 THEN 1                    \n"
					+ "                           WHEN  3 THEN 2                    \n"
					+ "                           WHEN  4 THEN 3                    \n"
					+ "                           WHEN  5 THEN 4                    \n"
					+ "                           WHEN  6 THEN 5                    \n"
					+ "                           WHEN  7 THEN 6 END AS WeekOrder   \n"
					+ "              ,CASE D.week WHEN  1 THEN '��'                 \n"
					+ "                           WHEN  2 THEN '��'                 \n"
					+ "                           WHEN  3 THEN 'ȭ'                 \n"
					+ "                           WHEN  4 THEN '��'                 \n"
					+ "                           WHEN  5 THEN '��'                 \n"
					+ "                           WHEN  6 THEN '��'                 \n"
					+ "                           WHEN  7 THEN '��' END AS WeekName \n"
					+ "              ,SUM(A.HitCnt) AS PgCnt \n"
                    + "          FROM dbo.SumPgDaily0 A  with(NoLock)                          \n"
                    + "               INNER merge JOIN SummaryBase D with(NoLock) ON (A.LogDay      = D.LogDay)     \n"
					+ "         WHERE A.LogDay BETWEEN "+ StartDay  + "   \n"
					+ "                            AND "+ EndDay    + "   \n"
                    + " 	      AND A.SummaryType = 5		              \n"
					);

                if (!CategoryCode.Equals("00")) // Ư��ī�װ����ý�
                {
                    sbQuery.Append(" AND A.Menu1 = " + CategoryCode + "  \n");

                    if (!GenreCode.Equals("00")) // Ư���帣���ý�
                    {
                        sbQuery.Append(" AND A.Menu2 = " + GenreCode + "  \n");
                    }
                }

                if (ProgramName.Trim().Length > 0)
                {
                    sbQuery.Append(" AND A.ProgKey IN (SELECT ProgramKey FROM AdTargetsHanaTV.dbo.Program with(NoLock) WHERE MediaCode   = " + MediaCode + " AND ProgramNm = '" + ProgramName + "') \n");
                }
				
				sbQuery.Append(""
					+ "         GROUP BY A.LogDay, D.week \n"
					+ "       ) TA                                        \n"
					+ " GROUP BY WeekOrder, WeekName                      \n"
					+ "                                                   \n"
					+ " ORDER BY WeekOrder                                \n"
					);  

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				statisticsPgWeekModel.ReportDataSet = ds.Copy();

				// ���
				statisticsPgWeekModel.ResultCnt = Utility.GetDatasetCount(statisticsPgWeekModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				statisticsPgWeekModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + statisticsPgWeekModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsPgWeekAVG() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgWeekModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsPgWeekModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
				}
				else if(isNotReady)
				{
					statisticsPgWeekModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					statisticsPgWeekModel.ResultDesc = "���Ϻ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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