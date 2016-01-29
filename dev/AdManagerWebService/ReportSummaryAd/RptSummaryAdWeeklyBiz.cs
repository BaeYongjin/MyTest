// ===============================================================================
//
// RptSummaryAdWeeklyBiz.cs
//
// �����ְ�����Ʈ ���� 
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
 * Class Name: RptSummaryAdWeeklyBiz
 * �ֿ���  : �����ְ�����Ʈ ó�� ����
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
 *			  - GetRptSummaryAdWeeklyList()
 * ��������  : 
 * Exception : "0���� ������ ������ �߻��߽��ϴ�."
 * ó���� ���ؼ� CASE������ ó��
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

namespace AdManagerWebService.ReportSummaryAd
{
    /// <summary>
    /// RptSummaryAdDailyBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class RptSummaryAdWeeklyBiz : BaseBiz
    {
        #region  ������
        public RptSummaryAdWeeklyBiz()
            : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region �����ְ�����Ʈ
        /// <summary>
        /// �����ְ�����Ʈ
        /// </summary>
        /// <param name="RptSummaryAdWeekly"></param>
        public void GetRptSummaryAdWeeklyList(HeaderModel header, RptSummaryAdWeeklyModel rptSummaryAdWeekly)
        {
            try
            {
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetRptSummaryAdWeekly() Start");
                _log.Debug("-----------------------------------------");

                // ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
                if (rptSummaryAdWeekly.SearchStartDay.Length > 6) rptSummaryAdWeekly.SearchStartDay = rptSummaryAdWeekly.SearchStartDay.Substring(2, 6);
                if (rptSummaryAdWeekly.SearchDay.Length > 6) rptSummaryAdWeekly.SearchDay = rptSummaryAdWeekly.SearchDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchStartDay  :[" + rptSummaryAdWeekly.SearchStartDay + "]");		// �˻� ��������
                _log.Debug("SearchEndDay    :[" + rptSummaryAdWeekly.SearchDay + "]");			// �˻� ��������
                // __DEBUG__

                SqlParameter[] sqlParams = new SqlParameter[2];

                sqlParams[0] = new SqlParameter("@LogStartDay", SqlDbType.VarChar, 6);
                sqlParams[0].Value = rptSummaryAdWeekly.SearchStartDay;

                sqlParams[1] = new SqlParameter("@LogEndDay", SqlDbType.VarChar, 6);
                sqlParams[1].Value = rptSummaryAdWeekly.SearchDay;

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + "SELECT																															\n"
                    + "		y.TotNum																													\n"
                    + "		, y.TypeNum																													\n"
                    + "		, y.ItemNo																													\n"
                    + "		, y.ItemName																												\n"
                    + "		, Replace(y.ItemNo1, '999999999', '') as ItemNo1																			\n"
                    + "		, y.DayAdCnt1																												\n"
                    + "		, y.DayAdCnt2																												\n"
                    + "		, y.DayAdCnt3																												\n"
                    + "		, y.DayAdCnt4																												\n"
                    + "		, y.DayAdCnt5																												\n"
                    + "		, y.DayAdCnt6																												\n"
                    + "		, y.DayAdCnt7																												\n"
                    + "		, y.SumDayAdCnt																												\n"
                    + "		, y.DayAdUsers1																												\n"
                    + "		, y.DayAdUsers2																												\n"
                    + "		, y.DayAdUsers3																												\n"
                    + "		, y.DayAdUsers4																												\n"
                    + "		, y.DayAdUsers5																												\n"
                    + "		, y.DayAdUsers6																												\n"
                    + "		, y.DayAdUsers7																												\n"
                    + "		, y.SumDayUsers																												\n"

// -- 2014/10/23 - Youngil.Yi ���� --  "
// Exception : 0���� ������ ������ �߻��߽��ϴ�.
// ó���� ���ؼ� CASE������ ó��
//                    + "		, Cast(y.SumDayUsers / Cast(y.HouseTotal as real) * 100 as decimal(10, 2)) [DayCovr]										\n"
//                    + "		, Cast(y.SumDayAdCnt / Cast(y.SumDayUsers as real) as decimal(10, 2)) [DayFreq]												\n"
              
                    + "     , CASE Cast(y.HouseTotal as real) WHEN 0 THEN 0 ELSE Cast(y.SumDayUsers / Cast(y.HouseTotal as real) * 100 as decimal(10, 2))  END	[DayCovr]		\n"
                    + "     , CASE Cast(y.SumDayUsers as real)  WHEN 0 THEN 0 ELSE Cast(y.SumDayAdCnt / Cast(y.SumDayUsers as real) as decimal(10, 2)) END   [DayFreq]	    	\n"																									
                    + "		, y.AccuAdCnt																												\n"
                    + "		, Replace(y.AccuAdUsers, '999999999', 0) as AccuAdUsers																		\n"
                    + "		, Replace(y.RunDay, '999999999', '') as RunDay																				\n"
                    + "FROM																																\n"
                    + "	(																																\n"
                    + "		SELECT																														\n"
                    + "				x.TotNum																											\n"
                    + "				, x.TypeNum																											\n"
                    + "				, x.ItemNo																											\n"
                    + "				, CASE x.ItemNo																										\n"
                    + "						WHEN 88888888 THEN (SELECT CodeName FROM AdTargetsHanaTV.dbo.SystemCode cd with(NoLock) WHERE cd.Section=26 AND cd.code=x.AdType) + ' ' + cast(x.AdTypeCnt as varchar(10)) + '��' \n"
                    + "						WHEN 99999999 THEN '�հ�'																					\n"
                    + "						ELSE x.ItemName																								\n"
                    + "				  END as ItemName																									\n"
                    + "				, CASE x.ItemNo																										\n"
                    + "						WHEN 88888888 THEN '999999999'																				\n"
                    + "						WHEN 99999999 THEN '999999999'																				\n"
                    + "						ELSE x.ItemNo																								\n"
                    + "				  End AS ItemNo1																									\n"
                    + "				, x.DayAdCnt1																										\n"
                    + "				, x.DayAdCnt2																										\n"
                    + "				, x.DayAdCnt3																										\n"
                    + "				, x.DayAdCnt4																										\n"
                    + "				, x.DayAdCnt5																										\n"
                    + "				, x.DayAdCnt6																										\n"
                    + "				, x.DayAdCnt7																										\n"
                    + "				, (x.DayAdCnt1 + x.DayAdCnt2 + x.DayAdCnt3 + x.DayAdCnt4 + x.DayAdCnt5 + x.DayAdCnt6 + x.DayAdCnt7) AS SumDayAdCnt	\n"
                    + "				, x.DayAdUsers1																										\n"
                    + "				, x.DayAdUsers2																										\n"
                    + "				, x.DayAdUsers3																										\n"
                    + "				, x.DayAdUsers4																										\n"
                    + "				, x.DayAdUsers5																										\n"
                    + "				, x.DayAdUsers6																										\n"
                    + "				, x.DayAdUsers7																										\n"
                    + "				, CASE x.ItemNo																										\n"
                    + "						WHEN 88888888 THEN (SELECT TOP 1 WeekUsers FROM SummaryHouseHoldType a with(NoLock) WHERE a.LogDay between @LogStartDay AND @LogEndDay AND a.TypeSection=26 AND a.TypeCode=x.AdType ORDER BY LogDay DESC)  \n"
                    + "						WHEN 99999999 THEN (SELECT TOP 1 WeekUsers FROM SummaryHouseHoldType a with(NoLock) WHERE a.LogDay between @LogStartDay AND @LogEndDay AND a.TypeSection=26 AND a.TypeCode=0 ORDER BY LogDay DESC)        \n"
                    + "                     ELSE isnull( (select WeekUsers from SummaryHouseHold hou with(NoLock)                      \n"
                    + " 									where hou.LogDay = @LogEndDay                                                               \n"
                    + " 									and		hou.ItemNo = x.ItemNo                                                               \n"
                    + " 									and		hou.Category=0                                                                      \n"
                    + " 									and		hou.Genre=0                                                                         \n"
                    + " 									AND		hou.program=0 ) ,0)                                                                 \n"
                    + "				  END as SumDayUsers																								\n"
                    + "				, x.AccuAdCnt																										\n"
                    + "				, CASE x.ItemNo																										\n"
                    + "						WHEN 88888888 THEN '999999999'																				\n"
                    + "						WHEN 99999999 THEN '999999999'																				\n"
                    + "						ELSE x.AccuAdUsers                                                                                          \n"
                    + "				  End AS AccuAdUsers																								\n"
                    + "				, CASE x.ItemNo																										\n"
                    + "						WHEN 88888888 THEN '999999999'																				\n"
                    + "						WHEN 99999999 THEN '999999999'																				\n"
                    + "						ELSE x.RunDay																								\n"
                    + "				  End AS RunDay																										\n"
                    + "				, x.HouseTotal																										\n"
                    + "		FROM																														\n"
                    + "			(   SELECT    max(v.TotNum) as TotNum																					\n"
                    + "						, max(v.TypeNum) as TypeNum																					\n"
                    + "						, CASE t.RowNum WHEN 1 THEN v.AdType WHEN 2 THEN v.AdType WHEN 3 THEN 99 END as adType						\n"
                    + "						, CASE t.RowNum WHEN 1 THEN v.ItemNO WHEN 2 THEN 88888888 WHEN 3 THEN 99999999 END as ItemNO				\n"
                    + "						, min(v.ItemName) as ItemName																				\n"
                    + "						, count(*) as adTypeCnt																						\n"
                    + "						, sum(v.DayAdCnt1) as DayAdCnt1																				\n"
                    + "						, sum(v.DayAdCnt2) as DayAdCnt2																				\n"
                    + "						, sum(v.DayAdCnt3) as DayAdCnt3																				\n"
                    + "						, sum(v.DayAdCnt4) as DayAdCnt4																				\n"
                    + "						, sum(v.DayAdCnt5) as DayAdCnt5																				\n"
                    + "						, sum(v.DayAdCnt6) as DayAdCnt6																				\n"
                    + "						, sum(v.DayAdCnt7) as DayAdCnt7																				\n"
                    + "						, Sum(v.DayAdUsers1) as DayAdUsers1																			\n"
                    + "						, Sum(v.DayAdUsers2) as DayAdUsers2																			\n"
                    + "						, Sum(v.DayAdUsers3) as DayAdUsers3																			\n"
                    + "						, Sum(v.DayAdUsers4) as DayAdUsers4																			\n"
                    + "						, Sum(v.DayAdUsers5) as DayAdUsers5																			\n"
                    + "						, Sum(v.DayAdUsers6) as DayAdUsers6																			\n"
                    + "						, Sum(v.DayAdUsers7) as DayAdUsers7																			\n"
                    + "						, Sum(v.AccuAdCnt) as AccuAdCnt																				\n"
                    + "						, Sum(v.AccuAdusers) as AccuAdUsers																			\n"
                    + "						, Sum(v.RunDay) as RunDay																					\n"
                    + "						, Max(v.HouseTotal) as HouseTotal																			\n"
                    + "						, Max(v.WeekUsers) as WeekUsers																				\n"
                    + "				FROM																												\n"
                    + "					(   SELECT    A.Adtype                                                                                          \n"
                    + "								, ROW_NUMBER() OVER( ORDER BY A.AdType, A.itemNo) as [TotNum]		                                \n"
                    + "								, ROW_NUMBER() OVER (PARTITION BY A.AdType ORDER BY A.itemNo) AS [TypeNum]	                        \n"
                    + "								, A.ItemNo                                                                                          \n"
                    + "								, A.ITemName                                                                                        \n"
                    + "								, A.RunDay                                                                                          \n"
                    + "								, ISNULL(Max(DayAdCnt1), 0) as DayAdCnt1                                                            \n"
                    + "								, ISNULL(Max(DayAdCnt2), 0) as DayAdCnt2	                                                        \n"
                    + "								, ISNULL(Max(DayAdCnt3), 0) as DayAdCnt3                                                            \n"
                    + "								, ISNULL(Max(DayAdCnt4), 0) as DayAdCnt4                                                            \n"
                    + "								, ISNULL(Max(DayAdCnt5), 0) as DayAdCnt5                                                            \n"
                    + "								, ISNULL(Max(DayAdCnt6), 0) as DayAdCnt6                                                            \n"
                    + "								, ISNULL(Max(DayAdCnt7), 0) as DayAdCnt7                                                            \n"
                    + "								, ISNULL(Max(DayAdUsers1), 0) as DayAdUsers1                                                        \n"
                    + "								, ISNULL(Max(DayAdUsers2), 0) as DayAdUsers2                                                        \n"
                    + "								, ISNULL(Max(DayAdUsers3), 0) as DayAdUsers3                                                        \n"
                    + "								, ISNULL(Max(DayAdUsers4), 0) as DayAdUsers4                                                        \n"
                    + "								, ISNULL(Max(DayAdUsers5), 0) as DayAdUsers5                                                        \n"
                    + "								, ISNULL(Max(DayAdUsers6), 0) as DayAdUsers6                                                        \n"
                    + "								, ISNULL(Max(DayAdUsers7), 0) as DayAdUsers7                                                        \n"
                    + "								, Max(AccuAdCnt) as AccuAdCnt                                                                       \n"
                    + "								, Max(AccuAdusers) as AccuAdusers                                                                   \n"
                    + "								, Substring(Max(WeekUsers), 7, Len(Max(WeekUsers))) WeekUsers										\n"
                    + "								, Substring(Max(HouseTotal), 7, Len(Max(HouseTotal))) HouseTotal                                    \n"
                    + "						FROM                                                                                                        \n"
                    + "							(   SELECT	  item.Adtype                                                                               \n"
                    + "										, ad0.ItemNo                                                                                \n"
                    + "										, item.ItemName                                                                             \n"
                    + "										, ROW_NUMBER() OVER( ORDER BY item.AdType, ad0.itemNo) as [TotNum]		                    \n"
                    + "										, isnull(ad0.AdCntAccu,0) + isnull(ad0.AdCnt,0) as AccuAdCnt								\n"
                    + "										, hou.AccuUsers as AccuAdusers                                                              \n"
                    + "										, datediff(day, cast('20' +                                                                 \n"
                    + "											(select min(LogDay) from SummaryAdDaily0  a with(nolock) where itemNO=ad0.ItemNo ) as datetime),     \n"
                    + "											cast(@LogEndDay as datetime) ) + 1 as [RunDay]                                          \n"
                    + "										, CASE Base.Week WHEN 2 THEN ad0.AdCnt END AS DayAdCnt1                                     \n"
                    + "										, CASE Base.Week WHEN 3 THEN ad0.AdCnt END AS DayAdCnt2                                     \n"
                    + "										, CASE Base.Week WHEN 4 THEN ad0.AdCnt END AS DayAdCnt3                                     \n"
                    + "										, CASE Base.Week WHEN 5 THEN ad0.AdCnt END AS DayAdCnt4                                     \n"
                    + "										, CASE Base.Week WHEN 6 THEN ad0.AdCnt END AS DayAdCnt5                                     \n"
                    + "										, CASE Base.Week WHEN 7 THEN ad0.AdCnt END AS DayAdCnt6                                     \n"
                    + "										, CASE Base.Week WHEN 1 THEN ad0.AdCnt END AS DayAdCnt7                                     \n"
                    + "										, CASE Base.Week WHEN 2 THEN hou.Dayusers END AS DayAdUsers1                                \n"
                    + "										, CASE Base.Week WHEN 3 THEN hou.Dayusers END AS DayAdUsers2                                \n"
                    + "										, CASE Base.Week WHEN 4 THEN hou.Dayusers END AS DayAdUsers3                                \n"
                    + "										, CASE Base.Week WHEN 5 THEN hou.Dayusers END AS DayAdUsers4                                \n"
                    + "										, CASE Base.Week WHEN 6 THEN hou.Dayusers END AS DayAdUsers5                                \n"
                    + "										, CASE Base.Week WHEN 7 THEN hou.Dayusers END AS DayAdUsers6                        \n"
                    + "										, CASE Base.Week WHEN 1 THEN hou.Dayusers END AS DayAdUsers7                        \n"
                    + "										, ad0.LogDay+cast(hou.WeekUsers as varchar(10)) as WeekUsers						\n"
                    + "										, ad0.LogDay+cast(Base.HouseTotal as varchar(20)) as HouseTotal						\n"
                    + "								FROM	SummaryAdDaily0 ad0 with(NoLock)                                                    \n"
                    + "								INNER JOIN dbo.SummaryHouseHold hou with(NoLock)											\n"
                    + "									    ON  hou.LogDay = ad0.LogDay                                                         \n"
                    + "                                     and hou.ItemNo = ad0.ItemNo and hou.Category=0 and hou.Genre=0 AND hou.program=0    \n"
                    + "								INNER JOIN AdTargetsHanaTV.dbo.ContractItem item with(NoLock)                                                   \n"
                    + "									    ON item.ItemNO = ad0.ItemNo                                                         \n"
                    + "								LEFT JOIN SummaryBase Base with(NoLock) ON ad0.LogDay = Base.LogDay							\n"
                    + "								WHERE   ad0.LogDay Between @LogStartDay and @LogEndDay                                              \n"
                    + "                             and     ad0.ItemNo>0                                                                                \n"
                    + "                             and     ad0.ContractSeq=0                                                                           \n"
                    + "                             and     ad0.SummaryType=1                                                                           \n"
                    + "                             and     ad0.SummaryCode=1                                                                           \n"
                    + "								) A                                                                                                 \n"
                    + "								GROUP By A.Adtype, A.ItemNo, A.ITemName, A.RunDay                                                   \n"
                    + "						) v                                                                                                         \n"
                    + "						INNER JOIN AdTargetsHanaTV.dbo.COPY_T t on t.RowNum <=3                                                                         \n"
                    + "							GROUP BY CASE t.RowNum WHEN 1 THEN v.AdType WHEN 2 THEN v.AdType WHEN 3 THEN 99 END                     \n"
                    + "								   , CASE t.RowNum WHEN 1 THEN v.ItemNO WHEN 2 THEN 88888888 WHEN 3 THEN 99999999 END				\n"
                    + "			) x																														\n"
                    + "	) y																																\n"
                    + "ORDER BY 1, 2, 3, 4																												\n"
                    );
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.Timeout = 60;
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                // ��� DataSet ����
                rptSummaryAdWeekly.RptWeeklyDataSet = ds.Copy();
                rptSummaryAdWeekly.ResultCnt = Utility.GetDatasetCount(rptSummaryAdWeekly.RptWeeklyDataSet);
                rptSummaryAdWeekly.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + rptSummaryAdWeekly.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetRptSummaryAdWeeklyList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                rptSummaryAdWeekly.ResultCD = "3000";
                rptSummaryAdWeekly.ResultDesc = "�ְ� ��û������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
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
