// ===============================================================================
//
// StatisticsPgRegionBiz.cs
//
// ����������Ʈ ��������� ���� 
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
 * Class Name: StatisticsPgRegionBiz
 * �ֿ���  : ����������Ʈ ä����� ó�� ����
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
    /// StatisticsPgRegionBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class StatisticsPgRegionBiz : BaseBiz
    {

        #region  ������
        public StatisticsPgRegionBiz()
            : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region ī�װ���� ��ȸ
        /// <summary>
        /// ī�װ������ȸ
        /// </summary>
        /// <param name="categoryModel"></param>
        public void GetCategoryList(HeaderModel header, StatisticsPgRegionModel statisticsPgRegionModel)
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
                _log.Debug("SearchMediaCode    :[" + statisticsPgRegionModel.SearchMediaCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + " SELECT A.MenuCode AS CategoryCode, A.MenuName AS CategoryName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A                 \n"
                    + "  WHERE A.MediaCode = " + statisticsPgRegionModel.SearchMediaCode + "   \n"
                    + "    AND A.MenuLevel = 1        \n"
                    + "  ORDER BY A.MenuCode          \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� ī�װ��𵨿� ����
                statisticsPgRegionModel.CategoryDataSet = ds.Copy();
                // ���
                statisticsPgRegionModel.ResultCnt = Utility.GetDatasetCount(statisticsPgRegionModel.CategoryDataSet);
                // ����ڵ� ��Ʈ
                statisticsPgRegionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsPgRegionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUsersList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgRegionModel.ResultCD = "3000";
                statisticsPgRegionModel.ResultDesc = "ī�װ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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
        public void GetGenreList(HeaderModel header, StatisticsPgRegionModel statisticsPgRegionModel)
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
                _log.Debug("SearchMediaCode    :[" + statisticsPgRegionModel.SearchMediaCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + " SELECT A.UpperMenuCode AS CategoryCode, A.MenuCode AS GenreCode, A.MenuName AS GenreName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A                 \n"
                    + "  WHERE A.MediaCode = " + statisticsPgRegionModel.SearchMediaCode + "   \n"
                    + "    AND A.MenuLevel = 2        \n"
                    + "  ORDER BY A.UpperMenuCode, A.MenuCode \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �𵨿� ����
                statisticsPgRegionModel.GenreDataSet = ds.Copy();
                // ���
                statisticsPgRegionModel.ResultCnt = Utility.GetDatasetCount(statisticsPgRegionModel.GenreDataSet);
                // ����ڵ� ��Ʈ
                statisticsPgRegionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsPgRegionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgRegionModel.ResultCD = "3000";
                statisticsPgRegionModel.ResultDesc = "�帣���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        #endregion

        #region �Ⱓ�� ���������

        /// <summary>
        ///  �Ⱓ�� ���������
        /// </summary>
        /// <param name="statisticsPgRegionModel"></param>        
        public void GetStatisticsPgRegion_Org(HeaderModel header, StatisticsPgRegionModel statisticsPgRegionModel)
        {
            bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
            bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
            {
                StringBuilder sbQuery = null;

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgRegion() Start");
                _log.Debug("-----------------------------------------");

                // ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
                if (statisticsPgRegionModel.SearchStartDay.Length > 6) statisticsPgRegionModel.SearchStartDay = statisticsPgRegionModel.SearchStartDay.Substring(2, 6);
                if (statisticsPgRegionModel.SearchEndDay.Length > 6) statisticsPgRegionModel.SearchEndDay = statisticsPgRegionModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchMediaCode	   :[" + statisticsPgRegionModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchCategoryCode :[" + statisticsPgRegionModel.SearchCategoryCode + "]");		// �˻� ī�װ��ڵ� 
                _log.Debug("SearchGenreCode    :[" + statisticsPgRegionModel.SearchGenreCode + "]");		// �˻� �帣�ڵ�           
                _log.Debug("SearchKey          :[" + statisticsPgRegionModel.SearchKey + "]");		// �˻� Ű(���α׷���)           
                _log.Debug("SearchStartDay     :[" + statisticsPgRegionModel.SearchStartDay + "]");		// �˻� ������� ����          
                _log.Debug("SearchEndDay       :[" + statisticsPgRegionModel.SearchEndDay + "]");		// �˻� �������� ����          
                // __DEBUG__

                string MediaCode = statisticsPgRegionModel.SearchMediaCode;
                string CategoryCode = statisticsPgRegionModel.SearchCategoryCode;
                string GenreCode = statisticsPgRegionModel.SearchGenreCode;
                string ProgramName = statisticsPgRegionModel.SearchKey;
                string StartDay = statisticsPgRegionModel.SearchStartDay;
                string EndDay = statisticsPgRegionModel.SearchEndDay;

                // ��������
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "/* ������ ���������    */                 \n"
                    + "DECLARE @TotPgHit int;    -- ��ü �����������  \n"
                    + "SET @TotPgHit    = 0;                      \n"
                    + "                                           \n"
                    + "-- ��ü ����Hit                              \n"
                    + "SELECT @TotPgHit = ISNULL(SUM(A.HitCnt),0)  \n"
                    + "  FROM SummaryPgDaily0 A with(noLock)       \n"
                    + " WHERE A.LogDay BETWEEN '" + StartDay + "' \n"
                    + "                    AND '" + EndDay + "' \n"
                    + "   AND A.SummaryType  = 5                  \n"
                    + "   AND A.Genre > 0                         \n" // �帣�з��� �Ǿ��ִ°͸� ��ȸ �帣�� ������(0�̸�) ���α׷� ��Ʈ���̴�.
                    + "                                           \n"
                    + "-- ������ ������ ���� �ӽ����̺�                \n"
                    + "SELECT IDENTITY(INT, 1, 1) AS Rownum, T.*  \n"
                    + "INTO #TempRegion                           \n"
                    + "FROM (SELECT A.SummaryCode                 \n"
                    + "              ,A.SummaryName               \n"
                    + "          FROM SummaryCode A  with(noLock) \n"
                    + "         WHERE A.SummaryType = 5           \n"
                    + "           AND A.Level = 1 ) T             \n"
                    + "                                           \n"
                    + "-- ������ ����                               \n"
                    + "SELECT '1' AS ORD                          \n"
                    + "      ,(SPACE(2 - LEN(CONVERT(VARCHAR(2),TT.Rownum))) + CONVERT(VARCHAR(2),TT.Rownum) + ' ' + TA.SummaryName ) AS GrpName \n"
                    + "      ,TA.SummaryCode AS SumCode           \n"
                    + "      ,TA.SummaryName AS SumName           \n"
                    + "      ,(SELECT COUNT(*) FROM SummaryCode WHERE SummaryType = 5 AND Level = 2 AND UpperCode = TA.SummaryCode) AS SubCount \n"
                    + "      ,0 AS subSumCode                     \n"
                    + "      ,'�հ�' AS subSumName                 \n"
                    + "      ,ISNULL(SUM(TB.PgCnt),0) AS PgCnt    \n"
                    + "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.PgCnt),0) / CONVERT(float,@TotPgHit)) * 100)     \n"
                    + "                               ELSE 0 END AS PgRate                                                                      \n"
                    + "      ,REPLICATE('��', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(TB.PgCnt),0)/CONVERT(float,@TotPgHit) * 50),0)   \n"
                    + "                                                ELSE 0 END) AS RateBar                                                   \n"
                    + "  FROM (SELECT A.SummaryCode               \n"
                    + "              ,A.SummaryName               \n"
                    + "          FROM SummaryCode A               \n"
                    + "         WHERE A.SummaryType = 5           \n"
                    + "           AND A.Level = 1                 \n"
                    + "       ) TA                                \n"
                    + "       LEFT JOIN                           \n"
                    + "       (                                   \n"
                    + "        SELECT CASE B.Level WHEN 1 THEN B.SummaryCode                      \n"
                    + "                                   ELSE B.UpperCode END AS SummaryCode     \n"
                    + "              ,A.HitCnt AS PgCnt                                            \n"
                    + "              ,B.Level                                                     \n"
                    + "          FROM SummaryPgDaily0 A  with(noLock)              \n"
                    + "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode) \n"
                    + "               LEFT  JOIN AdTargetsHanaTV.dbo.Menu        C with(NoLock) ON (A.Genre       = C.MenuCode    AND C.MenuLevel   = 2)             \n"
                    + "               LEFT  JOIN AdTargetsHanaTV.dbo.Program     D with(NoLock) ON (A.ProgKey     = D.ProgramKey  AND D.MediaCode   = " + MediaCode + ") \n"
                    + "         WHERE A.LogDay BETWEEN '" + StartDay + "' \n"
                    + "                            AND '" + EndDay + "' \n"
                    + "           AND A.SummaryType  = 5  -- 5:������     \n"
                    + "           AND ( (A.Genre > 0 "
                    );

                if (CategoryCode.Equals("00")) // ī�װ���ü 
                {
                    sbQuery.Append(" ) \n");
                }
                else
                {
                    if (GenreCode.Equals("00")) // �帣 ��ü
                    {
                        sbQuery.Append(" AND C.UpperMenuCode = " + CategoryCode + " ) \n");
                    }
                    else
                    {
                        sbQuery.Append(" AND A.Genre = " + GenreCode + " ) \n");
                    }
                }

                if (ProgramName.Trim().Length > 0)
                {
                    sbQuery.Append(" OR ( A.ProgKey > 0 AND D.ProgramNm LIKE '" + ProgramName + "' ) \n");
                }

                sbQuery.Append(""
                    + "               )                                   \n"
                    + "       ) TB                                        \n"
                    + "       ON (TA.SummaryCode = TB.SummaryCode)        \n"
                    + "       LEFT JOIN #TempRegion TT ON (TA.SummaryCode = TT.SummaryCode)   \n"
                    + " GROUP BY TT.Rownum, TA.SummaryCode, TA.SummaryName           \n"
                    + "                                                   \n"
                    + "UNION ALL                                          \n"
                    + "                                                   \n"
                    + "-- �������� ����                                    \n"
                    + "SELECT '2' AS ORD                                  \n"
                    + "      ,(SPACE(2 - LEN(CONVERT(VARCHAR(2),TT.Rownum))) + CONVERT(VARCHAR(2),TT.Rownum) + ' ' + TC.SummaryName ) AS GrpName \n"
                    + "      ,TA.UpperCode   AS SumCode                   \n"
                    + "      ,TC.SummaryName AS SumName                   \n"
                    + "      ,0 AS SubCount                               \n"
                    + "      ,TA.SummaryCode AS subSumCode                \n"
                    + "      ,TA.SummaryName AS subSumName                \n"
                    + "      ,ISNULL(SUM(TB.PgCnt),0) AS PgCnt            \n"
                    + "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.PgCnt),0) / CONVERT(float,@TotPgHit)) * 100)       \n"
                     + "                              ELSE 0 END AS PgRate                                                                        \n"
                    + "      ,REPLICATE('��', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(TB.PgCnt),0)/CONVERT(float,@TotPgHit) * 50),0)     \n"
                    + "                                                ELSE 0 END) AS RateBar                                                     \n"
                    + "  FROM (SELECT SummaryCode, SummaryName, UpperCode FROM SummaryCode WHERE SummaryType = 5 AND Level = 2) TA                           \n"
                    + "       LEFT JOIN                                   \n"
                    + "       (                                           \n"
                    + "        SELECT A.SummaryCode                       \n"
                    + "              ,B.UpperCode                         \n"
                    + "              ,A.HitCnt AS PgCnt                   \n"
                    + "              ,B.Level                             \n"
                    + "          FROM SummaryPgDaily0 A with(noLock)      \n"
                    + "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode) \n"
                    + "               LEFT  JOIN AdTargetsHanaTV.dbo.Menu        C with(NoLock) ON (A.Genre       = C.MenuCode    AND C.MenuLevel   = 2)             \n"
                    + "               LEFT  JOIN AdTargetsHanaTV.dbo.Program     D with(NoLock) ON (A.ProgKey     = D.ProgramKey  AND D.MediaCode   = " + MediaCode + ") \n"
                    + "         WHERE A.LogDay BETWEEN '" + StartDay + "' \n"
                    + "                            AND '" + EndDay + "' \n"
                    + "           AND A.SummaryType  = 5  -- 5:������      \n"
                    + "           AND ( (A.Genre > 0 "
                    );

                if (CategoryCode.Equals("00")) // ī�װ���ü 
                {
                    sbQuery.Append(" ) \n");
                }
                else
                {
                    if (GenreCode.Equals("00")) // �帣 ��ü
                    {
                        sbQuery.Append(" AND C.UpperMenuCode = " + CategoryCode + " ) \n");
                    }
                    else
                    {
                        sbQuery.Append(" AND A.Genre = " + GenreCode + " ) \n");
                    }
                }

                if (ProgramName.Trim().Length > 0)
                {
                    sbQuery.Append(" OR ( A.ProgKey > 0 AND D.ProgramNm LIKE '" + ProgramName + "' ) \n");
                }

                sbQuery.Append(""
                    + "               )                                   \n"
                    + "       ) TB                                        \n"
                    + "       ON (TA.SummaryCode = TB.SummaryCode)                                                                         \n"
                    + "       LEFT JOIN SummaryCode TC ON (TA.UpperCode = TC.SummaryCode AND TC.SummaryType = 5 AND TC.Level = 1)          \n"
                    + "       LEFT JOIN #TempRegion TT ON (TA.UpperCode = TT.SummaryCode)                                                  \n"
                    + " GROUP BY TT.Rownum, TA.UpperCode, TC.SummaryName, TA.SummaryCode, TA.SummaryName                                              \n"
                    + "                                                   \n"
                    + " ORDER BY SumCode, PgCnt DESC                      \n"
                    + "                                                   \n"
                    + " DROP Table #TempRegion                            \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �����͸𵨿� ����
                statisticsPgRegionModel.ReportDataSet = ds.Copy();

                // ���
                statisticsPgRegionModel.ResultCnt = Utility.GetDatasetCount(statisticsPgRegionModel.ReportDataSet);

                ds.Dispose();

                // ����ڵ� ��Ʈ
                statisticsPgRegionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsPgRegionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgRegion() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgRegionModel.ResultCD = "3000";
                if (isNotTarget)
                {
                    statisticsPgRegionModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
                }
                else if (isNotReady)
                {
                    statisticsPgRegionModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
                }
                else
                {
                    statisticsPgRegionModel.ResultDesc = "��������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                    _log.Exception(ex);
                }
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }


        public void GetStatisticsPgRegion(HeaderModel header, StatisticsPgRegionModel statisticsPgRegionModel)
        {
            bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
            bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
            {
                StringBuilder sbQuery = null;

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgRegion() Start");
                _log.Debug("-----------------------------------------");

                // ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
                if (statisticsPgRegionModel.SearchStartDay.Length > 6) statisticsPgRegionModel.SearchStartDay = statisticsPgRegionModel.SearchStartDay.Substring(2, 6);
                if (statisticsPgRegionModel.SearchEndDay.Length > 6) statisticsPgRegionModel.SearchEndDay = statisticsPgRegionModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchMediaCode	   :[" + statisticsPgRegionModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchCategoryCode :[" + statisticsPgRegionModel.SearchCategoryCode + "]");		// �˻� ī�װ��ڵ� 
                _log.Debug("SearchGenreCode    :[" + statisticsPgRegionModel.SearchGenreCode + "]");		// �˻� �帣�ڵ�           
                _log.Debug("SearchKey          :[" + statisticsPgRegionModel.SearchKey + "]");		// �˻� Ű(���α׷���)           
                _log.Debug("SearchStartDay     :[" + statisticsPgRegionModel.SearchStartDay + "]");		// �˻� ������� ����          
                _log.Debug("SearchEndDay       :[" + statisticsPgRegionModel.SearchEndDay + "]");		// �˻� �������� ����          
                // __DEBUG__

                string MediaCode = statisticsPgRegionModel.SearchMediaCode;
                string CategoryCode = statisticsPgRegionModel.SearchCategoryCode;
                string GenreCode = statisticsPgRegionModel.SearchGenreCode;
                string ProgramName = statisticsPgRegionModel.SearchKey;
                string StartDay = statisticsPgRegionModel.SearchStartDay;
                string EndDay = statisticsPgRegionModel.SearchEndDay;

                // ��������
                sbQuery = new StringBuilder();
				sbQuery.Append("Declare	@TotPgHit		int		\n");
				sbQuery.Append("Set		@TotPgHit		= 0	\n");

				//��ü ����Hit
				sbQuery.Append("Select	@TotPgHit = isnull(sum(A.HitCnt),0)								\n");
				sbQuery.Append("From		dbo.SumPgDaily0 A with(noLock)									\n");
				sbQuery.Append("Where	A.LogDay Between " + StartDay + "	And " + EndDay + "	\n");
				sbQuery.Append("And		A.SummaryType = 5													\n");

				//������ ������ ���� �ӽ����̺�           
				sbQuery.Append("Select	identity(int, 1, 1) As RowNum, T.*  	\n");
				sbQuery.Append("Into		#TempRegion                           	\n");
				sbQuery.Append("From														\n");
				sbQuery.Append("	(	Select	A.SummaryCode						\n");
				sbQuery.Append("		,			A.SummaryName						\n");
				sbQuery.Append("		From		SummaryCode A						\n");
				sbQuery.Append("    Where	A.SummaryType = 5					\n");
				sbQuery.Append("    And		A.Level = 1								\n");
				sbQuery.Append(")  T														\n");

				sbQuery.Append("Select	TD.Ord	\n");
                sbQuery.Append(",			AdTargetsHanaTV.dbo.ufnPadding('L',TT.RowNum, 2,'0') + ' ' + TA.SummaryName As GrpName 	\n");
				sbQuery.Append(",			TD.UpperCode As SumCode																	\n");
				sbQuery.Append(",			TA.SummaryName as SumName																\n");
				sbQuery.Append(",			(Select count(*) From SummaryCode With(NoLock) Where SummaryType = 5 And UpperCode = TD.UpperCode and TD.SummCode = 0 ) As SubCount 	\n");
				sbQuery.Append(",			TD.SummCode As subSumCode																																								\n");
				sbQuery.Append(",			isnull(tb.summaryName,'�հ�') As subSumName																																			\n");
				sbQuery.Append(",			isnull(sum(TD.PgCnt), 0) As PgCnt																																			\n");
				sbQuery.Append(",			Case When @TotPgHit > 0 Then convert(decimal(9,2),(isnull(sum(TD.PgCnt),0) / convert(float,@TotPgHit)) * 100) Else 0 End  As PgRate					\n");
				sbQuery.Append(",			replicate('��', Case When @TotPgHit > 0 Then round((isnull(sum(TD.PgCnt),0) / convert(float,@TotPgHit) *50),0) Else 0 End) As RateBar					\n");
				sbQuery.Append(",			TC.Orders As Orders								\n");
				sbQuery.Append(",			TC.ParentCode	 As ParentCode				\n");
				sbQuery.Append(",			' '+TC.SummaryDesc	As SummaryDesc		\n");
				sbQuery.Append("From																\n");
				sbQuery.Append("	(		Select	SummaryCode								\n");
				sbQuery.Append("			,			SummaryName								\n");
				sbQuery.Append("			,			UpperCode									\n");
				sbQuery.Append("			,			Orders										\n");
				sbQuery.Append("			,			ParentCode									\n");
				sbQuery.Append("			,			SummaryDesc								\n");
				sbQuery.Append("			From		SummaryCode with(nolock)			\n");
				sbQuery.Append("			Where	SummaryType = 5 						\n");
				sbQuery.Append("	) TC																\n");
				sbQuery.Append("	Inner Merge Join												\n");
				sbQuery.Append("	(		Select	Case When SummCode = 0 Then 1 Else 2 End ORD	\n");
				sbQuery.Append("			,			UpperCode														\n");
				sbQuery.Append("			,			SummCode														\n");
				sbQuery.Append("			,			min(SummCodeOrg) as SummCodeOrg					\n");
				sbQuery.Append("			,			sum(pgCnt) As pgCnt										\n");
				sbQuery.Append("			From																			\n");
				sbQuery.Append("			(		Select	UpperCode												\n");
				sbQuery.Append("					,			Case When T.RowNum = 2 Then 0 Else SummaryCode End As SummCode							\n");
				sbQuery.Append("					,			Case When T.RowNum = 2 Then UpperCode Else SummaryCode End  As SummCodeOrg			\n");
				sbQuery.Append("					,			pgCnt																														\n");
				sbQuery.Append("					,			RowNum																													\n");
				sbQuery.Append("					From																																	\n");
				sbQuery.Append("					(	Select	Case When C.UpperCode = 0 Then C.SummaryCode Else C.UpperCode End As UpperCode	\n");
				sbQuery.Append("						,			C.SummaryCode																									\n");
				sbQuery.Append("						,			sum(S.PgCnt) as PgCnt																							\n");
				sbQuery.Append("						From																																\n");
				sbQuery.Append("						(	SELECT		A.SummaryType																							\n");
				sbQuery.Append("							,				A.SummaryCode																							\n");
				sbQuery.Append("							,				sum(A.HitCnt)	AS PgCnt																				\n");
				sbQuery.Append("							From			dbo.SumPgDaily0 A With(NoLock)																	\n");
				sbQuery.Append("							Where		A.LogDay Between " + StartDay + "	And " + EndDay + "										\n");
				sbQuery.Append("							And			A.SummaryType  = 5																						\n");
				if (!CategoryCode.Equals("00")) // Ư��ī�װ����ý�
				{
					sbQuery.Append("						And			A.Menu1 = " + CategoryCode + "	\n");

					if (!GenreCode.Equals("00")) // Ư���帣���ý�
					{
						sbQuery.Append("					And			A.Menu2 = " + GenreCode + "		\n");
					}
				}
				if (ProgramName.Trim().Length > 0)
				{
                    sbQuery.Append("						And A.ProgKey In (Select ProgramKey From AdTargetsHanaTV.dbo.Program With(NoLock) Where MediaCode   = 1 And ProgramNm = '" + ProgramName + "') \n");
				}
				sbQuery.Append("							Group By		A.SummaryType, A.SummaryCode																											\n");
				sbQuery.Append("						) S																																										\n");
				sbQuery.Append("						Inner Join SummaryCode C With(NoLock) On (S.SummaryType = C.SummaryType And S.SummaryCode = C.SummaryCode)			\n");
				sbQuery.Append("						Group By C.UpperCode,C.SummaryCode																														\n");
				sbQuery.Append("					) D																																											\n");
                sbQuery.Append("					Inner Join AdTargetsHanaTV.dbo.copy_t T On T.RowNum <= 2																															\n");
				sbQuery.Append("					Where Case When UpperCode > 5000 Then Case When RowNum = 1 Then 'N' Else 'Y' End  Else 'Y' End = 'Y'									\n");
				sbQuery.Append("			)	X																																													\n");
				sbQuery.Append("			Group By UpperCode,SummCode 																																				\n");
				sbQuery.Append("	) TD On TC.SummaryCode = TD.SummCodeOrg																																	\n");
				sbQuery.Append("	Left Join SummaryCode	TA With(NoLock) On TA.SummaryCode = TD.UpperCode And TA.summaryType = 5													\n");
				sbQuery.Append("	Left Join SummaryCode	TB With(NoLock) On TB.SummaryCode = TD.SummCode And TB.UpperCode = TD.UpperCode And TA.summaryType = 5	\n");
				sbQuery.Append("	Left Join #TempRegion	TT ON TA.SummaryCode = TT.SummaryCode																										\n");
				sbQuery.Append("	Group By TD.Ord,TT.RowNum, TD.UpperCode, TD.SummCode, TA.SummaryName,tb.summaryName,TC.Orders ,TC.ParentCode ,TC.SummaryDesc 	\n");

				sbQuery.Append("DROP Table #TempRegion	\n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �����͸𵨿� ����
                statisticsPgRegionModel.ReportDataSet = ds.Copy();

                // ���
                statisticsPgRegionModel.ResultCnt = Utility.GetDatasetCount(statisticsPgRegionModel.ReportDataSet);

                ds.Dispose();

                // ����ڵ� ��Ʈ
                statisticsPgRegionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsPgRegionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgRegion() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgRegionModel.ResultCD = "3000";
                if (isNotTarget)
                {
                    statisticsPgRegionModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
                }
                else if (isNotReady)
                {
                    statisticsPgRegionModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
                }
                else
                {
                    statisticsPgRegionModel.ResultDesc = "��������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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

        #region �Ⱓ�� ��������� ���
        /// <summary>
        ///  �Ⱓ�� ��������� ���
        /// </summary>
        /// <param name="statisticsPgRegionModel"></param>
        public void GetStatisticsPgRegionAVG(HeaderModel header, StatisticsPgRegionModel statisticsPgRegionModel)
        {
            bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
            bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
            {
                StringBuilder sbQuery = null;

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgRegionAVG() Start");
                _log.Debug("-----------------------------------------");

                // ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
                if (statisticsPgRegionModel.SearchStartDay.Length > 6) statisticsPgRegionModel.SearchStartDay = statisticsPgRegionModel.SearchStartDay.Substring(2, 6);
                if (statisticsPgRegionModel.SearchEndDay.Length > 6) statisticsPgRegionModel.SearchEndDay = statisticsPgRegionModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchMediaCode	   :[" + statisticsPgRegionModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchCategoryCode :[" + statisticsPgRegionModel.SearchCategoryCode + "]");		// �˻� ī�װ��ڵ� 
                _log.Debug("SearchGenreCode    :[" + statisticsPgRegionModel.SearchGenreCode + "]");		// �˻� �帣�ڵ�           
                _log.Debug("SearchKey          :[" + statisticsPgRegionModel.SearchKey + "]");		// �˻� Ű(���α׷���)           
                _log.Debug("SearchStartDay     :[" + statisticsPgRegionModel.SearchStartDay + "]");		// �˻� ������� ����          
                _log.Debug("SearchEndDay       :[" + statisticsPgRegionModel.SearchEndDay + "]");		// �˻� �������� ����          
                // __DEBUG__

                string MediaCode = statisticsPgRegionModel.SearchMediaCode;
                string CategoryCode = statisticsPgRegionModel.SearchCategoryCode;
                string GenreCode = statisticsPgRegionModel.SearchGenreCode;
                string ProgramName = statisticsPgRegionModel.SearchKey;
                string StartDay = statisticsPgRegionModel.SearchStartDay;
                string EndDay = statisticsPgRegionModel.SearchEndDay;

                // ��������
                sbQuery = new StringBuilder();
				sbQuery.Append("Declare	@TotPgHit	int		\n");
				sbQuery.Append("Declare	@DateDiff	int		\n");
				
				sbQuery.Append("Set		@TotPgHit	= 0	\n");
				sbQuery.Append("Set		@DateDiff	=	" + EndDay + " - " + StartDay + " + 1	\n");	

				//��ü ����Hit
				sbQuery.Append("Select	@TotPgHit = isnull(sum(A.HitCnt),0)								\n");
				sbQuery.Append("From		dbo.SumPgDaily0 A with(noLock)									\n");
				sbQuery.Append("Where	A.LogDay Between " + StartDay + "	And " + EndDay + "	\n");
				sbQuery.Append("And		A.SummaryType = 5													\n");

				//������ ������ ���� �ӽ����̺�           
				sbQuery.Append("Select	identity(int, 1, 1) As RowNum, T.*  	\n");
				sbQuery.Append("Into		#TempRegion                           	\n");
				sbQuery.Append("From														\n");
				sbQuery.Append("	(	Select	A.SummaryCode						\n");
				sbQuery.Append("		,			A.SummaryName						\n");
				sbQuery.Append("		From		SummaryCode A						\n");
				sbQuery.Append("    Where	A.SummaryType = 5					\n");
				sbQuery.Append("    And		A.Level = 1								\n");
				sbQuery.Append(")  T														\n");

				sbQuery.Append("Select	TD.Ord	\n");
                sbQuery.Append(",			AdTargetsHanaTV.dbo.ufnPadding('L',TT.RowNum, 2,'0') + ' ' + TA.SummaryName As GrpName 	\n");
				sbQuery.Append(",			TD.UpperCode As SumCode																	\n");
				sbQuery.Append(",			TA.SummaryName as SumName																\n");
				sbQuery.Append(",			(Select count(*) From SummaryCode With(NoLock) Where SummaryType = 5 And UpperCode = TD.UpperCode and TD.SummCode = 0 ) As SubCount 	\n");
				sbQuery.Append(",			TD.SummCode As subSumCode																																								\n");
				sbQuery.Append(",			isnull(tb.summaryName,'�հ�') As subSumName																																			\n");
				sbQuery.Append(",			isnull(sum(TD.PgCnt) / @DateDiff,0) As PgCnt																																							\n");
				sbQuery.Append(",			Case When @TotPgHit > 0 Then convert(decimal(9,2),(isnull(sum(TD.PgCnt),0) / convert(float,@TotPgHit)) * 100) Else 0 End  As PgRate					\n");
				sbQuery.Append(",			replicate('��', Case When @TotPgHit > 0 Then round((isnull(sum(TD.PgCnt),0) / convert(float,@TotPgHit) *50),0) Else 0 End) As RateBar					\n");
				sbQuery.Append(",			TC.Orders As Orders								\n");
				sbQuery.Append(",			TC.ParentCode	 As ParentCode				\n");
				sbQuery.Append(",			' '+TC.SummaryDesc	As SummaryDesc		\n");
				sbQuery.Append("From																\n");
				sbQuery.Append("	(		Select	SummaryCode								\n");
				sbQuery.Append("			,			SummaryName								\n");
				sbQuery.Append("			,			UpperCode									\n");
				sbQuery.Append("			,			Orders										\n");
				sbQuery.Append("			,			ParentCode									\n");
				sbQuery.Append("			,			SummaryDesc								\n");
				sbQuery.Append("			From		SummaryCode with(nolock)			\n");
				sbQuery.Append("			Where	SummaryType = 5 						\n");
				sbQuery.Append("	) TC																\n");
				sbQuery.Append("	Inner Merge Join												\n");
				sbQuery.Append("	(		Select	Case When SummCode = 0 Then 1 Else 2 End ORD	\n");
				sbQuery.Append("			,			UpperCode														\n");
				sbQuery.Append("			,			SummCode														\n");
				sbQuery.Append("			,			min(SummCodeOrg) as SummCodeOrg					\n");
				sbQuery.Append("			,			sum(pgCnt) As pgCnt										\n");
				sbQuery.Append("			From																			\n");
				sbQuery.Append("			(		Select	UpperCode												\n");
				sbQuery.Append("					,			Case When T.RowNum = 2 Then 0 Else SummaryCode End As SummCode							\n");
				sbQuery.Append("					,			Case When T.RowNum = 2 Then UpperCode Else SummaryCode End  As SummCodeOrg			\n");
				sbQuery.Append("					,			pgCnt																														\n");
				sbQuery.Append("					,			RowNum																													\n");
				sbQuery.Append("					From																																	\n");
				sbQuery.Append("					(	Select	Case When C.UpperCode = 0 Then C.SummaryCode Else C.UpperCode End As UpperCode	\n");
				sbQuery.Append("						,			C.SummaryCode																									\n");
				sbQuery.Append("						,			sum(S.PgCnt) as PgCnt																							\n");
				sbQuery.Append("						From																																\n");
				sbQuery.Append("						(	SELECT		A.SummaryType																							\n");
				sbQuery.Append("							,				A.SummaryCode																							\n");
				sbQuery.Append("							,				sum(A.HitCnt)	AS PgCnt																				\n");
				sbQuery.Append("							From			dbo.SumPgDaily0 A With(NoLock)																	\n");
				sbQuery.Append("							Where		A.LogDay Between " + StartDay + "	And " + EndDay + "										\n");
				sbQuery.Append("							And			A.SummaryType  = 5																						\n");
				if (!CategoryCode.Equals("00")) // Ư��ī�װ����ý�
				{
					sbQuery.Append("						And			A.Menu1 = " + CategoryCode + "	\n");

					if (!GenreCode.Equals("00")) // Ư���帣���ý�
					{
						sbQuery.Append("					And			A.Menu2 = " + GenreCode + "		\n");
					}
				}
				if (ProgramName.Trim().Length > 0)
				{
                    sbQuery.Append("						And A.ProgKey In (Select ProgramKey From AdTargetsHanaTV.dbo.Program With(NoLock) Where MediaCode   = 1 And ProgramNm = '" + ProgramName + "') \n");
				}
				sbQuery.Append("							Group By		A.SummaryType, A.SummaryCode																											\n");
				sbQuery.Append("						) S																																										\n");
				sbQuery.Append("						Inner Join SummaryCode C With(NoLock) On (S.SummaryType = C.SummaryType And S.SummaryCode = C.SummaryCode)			\n");
				sbQuery.Append("						Group By C.UpperCode,C.SummaryCode																														\n");
				sbQuery.Append("					) D																																											\n");
                sbQuery.Append("					Inner Join AdTargetsHanaTV.dbo.copy_t T On T.RowNum <= 2																															\n");
				sbQuery.Append("					Where Case When UpperCode > 5000 Then Case When RowNum = 1 Then 'N' Else 'Y' End  Else 'Y' End = 'Y'									\n");
				sbQuery.Append("			)	X																																													\n");
				sbQuery.Append("			Group By UpperCode,SummCode 																																				\n");
				sbQuery.Append("	) TD On TC.SummaryCode = TD.SummCodeOrg																																	\n");
				sbQuery.Append("	Left Join SummaryCode	TA With(NoLock) On TA.SummaryCode = TD.UpperCode And TA.summaryType = 5													\n");
				sbQuery.Append("	Left Join SummaryCode	TB With(NoLock) On TB.SummaryCode = TD.SummCode And TB.UpperCode = TD.UpperCode And TA.summaryType = 5	\n");
				sbQuery.Append("	Left Join #TempRegion	TT ON TA.SummaryCode = TT.SummaryCode																										\n");
				sbQuery.Append("	Group By TD.Ord,TT.RowNum, TD.UpperCode, TD.SummCode, TA.SummaryName,tb.summaryName,TC.Orders ,TC.ParentCode ,TC.SummaryDesc 	\n");

				sbQuery.Append("DROP Table #TempRegion	\n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �����͸𵨿� ����
                statisticsPgRegionModel.ReportDataSet = ds.Copy();

                // ���
                statisticsPgRegionModel.ResultCnt = Utility.GetDatasetCount(statisticsPgRegionModel.ReportDataSet);

                ds.Dispose();

                // ����ڵ� ��Ʈ
                statisticsPgRegionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsPgRegionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgRegionAVG() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgRegionModel.ResultCD = "3000";
                if (isNotTarget)
                {
                    statisticsPgRegionModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
                }
                else if (isNotReady)
                {
                    statisticsPgRegionModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
                }
                else
                {
                    statisticsPgRegionModel.ResultDesc = "��������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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