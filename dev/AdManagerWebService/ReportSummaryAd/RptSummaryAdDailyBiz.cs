// ===============================================================================
//
// RptSummaryAdDailyBiz.cs
//
// �����ϰ�����Ʈ ���� 
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
 * Class Name: RptSummaryAdDailyBiz
 * �ֿ���  : �����ϰ�����Ʈ ó�� ����
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

namespace AdManagerWebService.ReportSummaryAd
{
	/// <summary>
	/// RptSummaryAdDailyBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RptSummaryAdDailyBiz : BaseBiz
	{
		#region  ������
		public RptSummaryAdDailyBiz() : base(FrameSystem.connSummaryDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region �����ϰ�����Ʈ
		/// <summary>
		/// �����ϰ�����Ʈ
		/// </summary>
		/// <param name="RptSummaryAdDaily"></param>
		public void	GetRptSummaryAdDailyList(HeaderModel header, RptSummaryAdDailyModel rptSummaryAdDaily)
		{

			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetRptSummaryAdDaily() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(rptSummaryAdDaily.SearchDay.Length > 6) rptSummaryAdDaily.SearchDay = rptSummaryAdDaily.SearchDay.Substring(2, 6);
				
				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchBgnDay    :[" + rptSummaryAdDaily.SearchDay + "]");		// �˻� ��������
				// __DEBUG__

				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@LogDay", SqlDbType.VarChar, 6);
				sqlParams[0].Value = rptSummaryAdDaily.SearchDay;
				
				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ "SELECT																										\n"                                                                                                                                                                                 
					+ "		y.TotNum                                                                                                \n"
					+ "		, y.TypeNum                                                                                             \n"
					+ "		, y.ItemNo                                                                                              \n"
					+ "		, y.ItemName                                                                                            \n"
					+ "		, Replace(y.ItemNo1, '999999999', '') as ItemNo1														\n"
					+ "		, y.dayAdCnt                                                                                            \n"
					+ "		, y.dayAdUsers                                                                                          \n"
					+ "		, Cast(y.DayAdCnt / Cast(base.ChannelAdHit as real) * 100 as decimal(10, 2)) [DayRate]                  \n"
					+ "		, Cast(y.DayAdUsers / Cast(base.HouseTotal as real) * 100 as decimal(10, 2)) [DayCovr]                  \n"
					+ "		, Cast(y.DayAdCnt / Cast(y.DayAdUsers as real) as decimal(10, 2)) [DayFreq]                             \n"
					+ "		, y.AccuAdCnt                                                                                           \n"
					+ "		, y.AccuAdUsers                                                                                         \n"
					+ "		, Cast(y.AccuAdUsers / Cast(base.HouseTotal as real)*100 as decimal(10, 2)) [AccuCovr]                  \n"
					+ "		, Cast(y.AccuAdCnt / Cast(y.AccuAdUsers as real) as decimal(10, 2)) [AccuFreq]                          \n"
					+ "		, Replace(y.RunDay, '999999999', '') as RunDay                                                          \n"
					+ "FROM                                                                                                         \n"
					+ "	(                                                                                                           \n"
					+ "		SELECT                                                                                                  \n"
					+ "				x.TotNum                                                                                        \n"
					+ "				, x.TypeNum                                                                                     \n"
					+ "				, x.ItemNo                                                                                      \n"
					+ "				, CASE x.ItemNo                                                                                 \n"
                    + "								WHEN 88888888 THEN (SELECT CodeName FROM AdTargetsHanaTV.dbo.SystemCode cd with(NoLock) WHERE cd.Section=26 AND cd.code=x.AdType) + ' ' + cast(x.AdTypeCnt as varchar(10)) + '��' \n"
					+ "								WHEN 99999999 THEN '�հ�'                                                       \n"
					+ "								ELSE x.ItemName                                                                 \n"
					+ "				  END as ItemName                                                                               \n"
					+ "				, CASE x.ItemNo																					\n"	
					+ "								WHEN 88888888 THEN '999999999'													\n"
					+ "								WHEN 99999999 THEN '999999999'													\n"
					+ "								ELSE x.ItemNo																	\n"	
					+ "				  End AS ItemNo1																				\n"	
					+ "				, x.DayAdCnt                                                                                    \n"
					+ "				, CASE x.ItemNo                                                                                 \n"
					+ "								WHEN 88888888 THEN (SELECT Dayusers FROM SummaryHouseHoldType a with(NoLock) WHERE a.LogDay=@LogDay AND a.TypeSection=26 AND a.TypeCode=x.AdType) \n"
					+ "								WHEN 99999999 THEN (SELECT Dayusers FROM SummaryHouseHoldType a with(NoLock) WHERE a.LogDay=@LogDay AND a.TypeSection=26 AND a.TypeCode=0)        \n"
					+ "								ELSE x.DayAdUsers                                                               \n"
					+ "				  END AS DayAdUsers                                                                             \n"
					+ "				, CASE x.ItemNo																					\n"	
					+ "					WHEN 88888888 THEN 0																		\n"	
					+ "					WHEN 99999999 THEN 0																		\n"
					+ "					ELSE x.AccuAdCnt																			\n"	
					+ "				End AS AccuAdCnt																				\n"
					+ "				, CASE x.ItemNo                                                                                 \n"
					+ "						WHEN 88888888 THEN 1                                                                    \n"
					+ "						WHEN 99999999 THEN 1	                                                                \n"
					+ "						ELSE x.AccAdUsers                                                                       \n"
					+ "				  End AS AccuAdUsers                                                                            \n"
					+ "				, CASE x.ItemNo																					\n"
					+ "						WHEN 88888888 THEN '999999999'															\n"		
					+ "						WHEN 99999999 THEN '999999999'															\n"	
					+ "						ELSE x.RunDay																			\n"
					+ "				  End AS RunDay																					\n"	
					+ "		FROM                                                                                                    \n"
					+ "				(                                                                                               \n"
					+ "					SELECT                                                                                      \n"
					+ "							max(v.TotNum) as TotNum                                                             \n"
					+ "							, max(v.TypeNum) as TypeNum                                                         \n"
					+ "							, CASE t.RowNum WHEN 1 THEN v.AdType WHEN 2 THEN v.AdType WHEN 3 THEN 99 END as adType          \n"
					+ "							, CASE t.RowNum WHEN 1 THEN v.ItemNO WHEN 2 THEN 88888888 WHEN 3 THEN 99999999 END as ItemNO	\n"
					+ "							, min(v.ItemName) as ItemName                                                                   \n"    
					+ "							, count(*) as adTypeCnt                                                                         \n"
					+ "							, sum(v.DayAdCnt) as DayAdCnt                                                                   \n"
					+ "							, Sum(v.DayAdUsers) as DayAdUsers                                                               \n"
					+ "							, Sum(v.AccuAdCnt) as AccuAdCnt                                                                 \n"
					+ "							, Sum(v.AccuAdusers) as AccAdUsers                                                              \n"
					+ "							, Sum(v.RunDay) as RunDay                                                                       \n"
					+ "					FROM																									\n"
					+ "						(																									\n"
					+ "							SELECT                                                                                          \n"
					+ "									item.Adtype                                                                             \n"
					+ "									, ROW_NUMBER() OVER( ORDER BY item.AdType, ad0.itemNo) as [TotNum]		                \n"
					+ "									, ROW_NUMBER() OVER (PARTITION BY item.AdType ORDER BY ad0.itemNo) AS [TypeNum]         \n"
					+ "									, ad0.ItemNo                                                                            \n"
					+ "									, item.ItemName                                                                         \n"
					+ "									, ad0.AdCnt as DayAdCnt                                                                 \n"
					+ "									, hou.Dayusers as DayAdUsers                                                            \n"
					+ "									, ad0.AdCntAccu as AccuAdCnt                                                            \n"
					+ "									, hou.AccuUsers as AccuAdusers                                                          \n"
					+ "									, datediff(day, cast(item.ExcuteStartDay as datetime), cast('20' + @LogDay as datetime)) + 1 as [RunDay]        \n"
					+ "							FROM	SummaryAdDaily0 ad0 with(NoLock)                                                                                \n"
					+ "							INNER Merge JOIN SummaryHouseHold hou with(NoLock)                                                                    \n"
					+ "									ON hou.LogDay = ad0.LogDay and hou.ItemNo = ad0.ItemNo and hou.Category=0 and hou.Genre=0 AND hou.program=0 \n"
                    + "							INNER Merge JOIN AdTargetsHanaTV.dbo.ContractItem item with(NoLock)                                                                       \n"
					+ "									ON item.ItemNO = ad0.ItemNo                                                                                 \n"
					+ "							WHERE ad0.LogDay = @LogDay and ad0.ItemNo>0 and ad0.ContractSeq=0 and ad0.SummaryType=1 and ad0.SummaryCode=1			\n"
					+ "					) v                                                                                                     \n"
                    + "					INNER JOIN AdTargetsHanaTV.dbo.COPY_T t on t.RowNum <=3                                                                     \n"
					+ "					GROUP BY CASE t.RowNum WHEN 1 THEN v.AdType WHEN 2 THEN v.AdType WHEN 3 THEN 99 END                     \n"
					+ "							, CASE t.RowNum WHEN 1 THEN v.ItemNO WHEN 2 THEN 88888888 WHEN 3 THEN 99999999 END              \n"
					+ "		) x                                                                                                                 \n"
					+ "	) y                                                                                                                     \n"
					+ "	INNER JOIN SummaryBase base with(NoLock) on base.LogDay = @LogDay                                                       \n"
					+ "ORDER BY 1, 2, 3, 4                                                                                                      \n"
					);
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);


				// ��� DataSet ����
				rptSummaryAdDaily.RptDailyDataSet = ds.Copy();

				// ���
				rptSummaryAdDaily.ResultCnt = Utility.GetDatasetCount(rptSummaryAdDaily.RptDailyDataSet);

				// ����ڵ� ��Ʈ
				rptSummaryAdDaily.ResultCD = "0000";
				
				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + rptSummaryAdDaily.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetRptSummaryAdDailyList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				rptSummaryAdDaily.ResultCD = "3000";
				rptSummaryAdDaily.ResultDesc = "�ϰ� ��û������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
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
