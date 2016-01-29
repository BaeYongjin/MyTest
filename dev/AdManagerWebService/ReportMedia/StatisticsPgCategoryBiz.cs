// ===============================================================================
//
// StatisticsPgCategoryBiz.cs
//
// ����������Ʈ ī�װ���� ���� 
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
 * Class Name: StatisticsPgCategoryBiz
 * �ֿ���  : ����������Ʈ ī�װ���� ó�� ����
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
    /// StatisticsPgCategoryBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class StatisticsPgCategoryBiz : BaseBiz
    {

        #region  ������
        public StatisticsPgCategoryBiz()
            : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region ī�װ� ���
        /// <summary>
        ///  �Ⱓ�� ī�װ����
        /// </summary>
        /// <param name="statisticsPgCategoryModel"></param>
        public void GetStatisticsPgCategory(HeaderModel header, StatisticsPgCategoryModel statisticsPgCategoryModel)
        {
            bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
            bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
            {
                StringBuilder sbQuery = null;

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgCategory() Start");
                _log.Debug("-----------------------------------------");

                // ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
                if (statisticsPgCategoryModel.SearchStartDay.Length > 6) statisticsPgCategoryModel.SearchStartDay = statisticsPgCategoryModel.SearchStartDay.Substring(2, 6);
                if (statisticsPgCategoryModel.SearchEndDay.Length > 6) statisticsPgCategoryModel.SearchEndDay = statisticsPgCategoryModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchMediaCode	  :[" + statisticsPgCategoryModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchStartDay    :[" + statisticsPgCategoryModel.SearchStartDay + "]");		// �˻� ������� ����          
                _log.Debug("SearchEndDay      :[" + statisticsPgCategoryModel.SearchEndDay + "]");		// �˻� �������� ����          
                // __DEBUG__

                string MediaCode = statisticsPgCategoryModel.SearchMediaCode;
                string StartDay = statisticsPgCategoryModel.SearchStartDay;
                string EndDay = statisticsPgCategoryModel.SearchEndDay;

                // ��������
                sbQuery = new StringBuilder();
                sbQuery.Append("\n  /* �Ⱓ�� ������ ī�װ� ���    */		    \n");
                sbQuery.Append("\n  DECLARE @TotPgHit int;    -- ��ü �����������	\n");
                sbQuery.Append("\n  SET @TotPgHit    = 0;                           \n");
                sbQuery.Append("\n -- ��ü ������Hit                                \n");
                sbQuery.Append("\n  SELECT @TotPgHit = ISNULL(SUM(A.HitCnt),0)      \n");
                sbQuery.Append("\n  FROM SummaryPgDaily1 A with(noLock)             \n");
                sbQuery.Append("\n  WHERE A.LogDay BETWEEN '" + StartDay + "'       \n");
                sbQuery.Append("\n                    AND '" + EndDay + "'        \n");

                sbQuery.Append("\n  -- ī�װ� ����                                \n");
                sbQuery.Append("\n  SELECT	AdTargetsHanaTV.dbo.ufnPadding('L',a.Category,5,'0') + ' ' +  b.MenuName AS Category     \n");
                sbQuery.Append("\n      ,B.MenuName AS CategoryName	        		\n");
                sbQuery.Append("\n      ,'1' AS ORDCode						        \n");
                sbQuery.Append("\n      ,'1 ī�װ��հ�' AS ORD                    \n");
                sbQuery.Append("\n      ,'ī�װ��հ�' AS ORDName                  \n");
                sbQuery.Append("\n      ,'' AS Title							    \n");
                sbQuery.Append("\n      ,SUM(A.HitCnt)				AS PgCnt	    \n");
                sbQuery.Append("\n      ,sum(isnull(a.PPx,0))		as PPxCnt	    \n");
                sbQuery.Append("\n      ,sum(isnull(a.Replay,0))	as RePlayCnt    \n");
                sbQuery.Append("\n		,CONVERT(DECIMAL(9,2),(sum(isnull(a.PPx,0)) / CONVERT(float,SUM(A.HitCnt))) * 100) as PPxRate                       \n");
                sbQuery.Append("\n      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(A.HitCnt),0) / CONVERT(float,@TotPgHit)) * 100)      \n");
                sbQuery.Append("\n                               ELSE 0 END AS PgRate                                                                       \n");
                sbQuery.Append("\n      ,REPLICATE('��', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(A.HitCnt),0)/CONVERT(float,@TotPgHit) * 50),0)     \n");
                sbQuery.Append("\n                                                ELSE 0 END) AS RateBar                                                    \n");
                sbQuery.Append("\n		,CONVERT(DECIMAL(9,2),(sum(isnull(a.Replay,0)) / CONVERT(float,SUM(A.HitCnt))) * 100) as RePlayRate                 \n");
                sbQuery.Append("\n  FROM  SummaryPgDaily3 A with(noLock)                                                                                    \n");
                sbQuery.Append("\n INNER JOIN AdTargetsHanaTV.dbo.Menu  B with(noLock) ON (A.Category   = B.MenuCode AND B.MenuLevel = 1 AND B.MediaCode = " + MediaCode + ")   \n");
                sbQuery.Append("\n   AND A.LogDay BETWEEN '" + StartDay + "'        \n");
                sbQuery.Append("\n                    AND '" + EndDay + "'          \n");
                sbQuery.Append("\n GROUP BY A.Category, B.MenuName                  \n");

                sbQuery.Append("\n  UNION ALL                                       \n");

                sbQuery.Append("\n  -- �帣 ����                                    \n");
                sbQuery.Append("\n  SELECT AdTargetsHanaTV.dbo.ufnPadding('L',a.Category,5,'0') + ' ' +  C.MenuName  AS Category \n");
                sbQuery.Append("\n      ,C.MenuName AS CatagoryName                 \n");
                sbQuery.Append("\n      ,'2' AS ORDCode                             \n");
                sbQuery.Append("\n      ,'2 �帣' AS ORD                            \n");
                sbQuery.Append("\n      ,'�帣' AS ORDName                          \n");
                sbQuery.Append("\n      ,B.MenuName AS Title                        \n");
                sbQuery.Append("\n      ,SUM(A.HitCnt)  AS PgCnt                    \n");
                sbQuery.Append("\n      ,sum(isnull(a.PPx,0))		as PPxCnt       \n");
                sbQuery.Append("\n      ,sum(isnull(a.Replay,0))	as RePlayCnt    \n");
                sbQuery.Append("\n		 ,CONVERT(DECIMAL(9,2),(sum(isnull(a.PPx,0)) / CONVERT(float,SUM(A.HitCnt))) * 100) as PPxRate                      \n");
                sbQuery.Append("\n      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(A.HitCnt),0) / CONVERT(float,@TotPgHit)) * 100)      \n");
                sbQuery.Append("\n                               ELSE 0 END AS PgRate                                                                       \n");
                sbQuery.Append("\n     ,REPLICATE('��', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(A.HitCnt),0)/CONVERT(float,@TotPgHit) * 50),0)      \n");
                sbQuery.Append("\n                                               ELSE 0 END) AS RateBar                                                     \n");
                sbQuery.Append("\n		,CONVERT(DECIMAL(9,2),(sum(isnull(a.Replay,0)) / CONVERT(float,SUM(A.HitCnt))) * 100) as RePlayRate                 \n");
                sbQuery.Append("\n  FROM SummaryPgDaily3 A with(noLock) INNER JOIN AdTargetsHanaTV.dbo.Menu  B with(noLock) ON (A.Genre    = B.MenuCode AND B.MenuLevel = 2 AND B.MediaCode = " + MediaCode + ")    \n");
                sbQuery.Append("\n                         INNER JOIN AdTargetsHanaTV.dbo.Menu  C with(noLock) ON (A.Category = C.MenuCode AND C.MenuLevel = 1 AND B.MediaCode = " + MediaCode + ")                 \n");
                sbQuery.Append("\n  WHERE A.LogDay BETWEEN '" + StartDay + "'       \n");
                sbQuery.Append("\n                   AND '" + EndDay + "'         \n");
                sbQuery.Append("\n GROUP BY A.Category, C.MenuName, B.MenuName      \n");

                sbQuery.Append("\nORDER BY Category, ORDCode, PgCnt DESC    \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �����͸𵨿� ����
                statisticsPgCategoryModel.ReportDataSet = ds.Copy();

                // ���
                statisticsPgCategoryModel.ResultCnt = Utility.GetDatasetCount(statisticsPgCategoryModel.ReportDataSet);

                ds.Dispose();

                // ����ڵ� ��Ʈ
                statisticsPgCategoryModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsPgCategoryModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgCategory() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgCategoryModel.ResultCD = "3000";
                if (isNotTarget)
                {
                    statisticsPgCategoryModel.ResultDesc = "�ش�Ⱓ�� ������ �������� �ʽ��ϴ�.";
                }
                else if (isNotReady)
                {
                    statisticsPgCategoryModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
                }
                else
                {
                    statisticsPgCategoryModel.ResultDesc = "ī�װ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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