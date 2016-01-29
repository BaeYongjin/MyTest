// ===============================================================================
//
// DailyAdExecSummaryRptBiz.cs
//
// ���� �Ѱ����� ���� 
//
// ===============================================================================
// Release history
// 2007.10.26 BJ.PARK OAP�� ���谡����� �� ��������̿� �޼ҵ� => GetContractItemList()
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: DailyAdExecSummaryRptBiz
 * �ֿ���  : ���� �Ѱ����� ����
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
	/// DailyAdExecSummaryRptBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class DailyAdExecSummaryRptBiz : BaseBiz
	{

		#region  ������
		public DailyAdExecSummaryRptBiz() : base(FrameSystem.connSummaryDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region �ϰ� �������� ���� ����
		/// <summary>
		///  �ѱⰣ �������� ����
		/// </summary>
		/// <param name="dailyAdExecSummaryRptModel"></param>
		public void GetDailyAdExecSummary(HeaderModel header, DailyAdExecSummaryRptModel dailyAdExecSummaryRptModel)
		{
			bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSummaryAdDaily() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(dailyAdExecSummaryRptModel.LogDay1.Length > 6) dailyAdExecSummaryRptModel.LogDay1 = dailyAdExecSummaryRptModel.LogDay1.Substring(2,6);
//				if(dailyAdExecSummaryRptModel.LogDay2.Length > 6) dailyAdExecSummaryRptModel.LogDay2   = dailyAdExecSummaryRptModel.LogDay2.Substring(2,6);
//				if(dailyAdExecSummaryRptModel.WeekDay.Length > 6) dailyAdExecSummaryRptModel.WeekDay   = dailyAdExecSummaryRptModel.WeekDay.Substring(2,6);
//				if(dailyAdExecSummaryRptModel.MonthDay.Length > 6) dailyAdExecSummaryRptModel.MonthDay   = dailyAdExecSummaryRptModel.MonthDay.Substring(2,6);
				
				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("LogDay1	  :[" + dailyAdExecSummaryRptModel.LogDay1   + "]");		// �˻� ��ü
//				_log.Debug("SearchContractSeq :[" + dailyAdExecSummaryRptModel.SearchContractSeq + "]");		// �˻� �����ȣ           
//				_log.Debug("SearchItemNo      :[" + dailyAdExecSummaryRptModel.SearchItemNo      + "]");		// �˻� �����ȣ           
//				_log.Debug("SearchStartDay    :[" + dailyAdExecSummaryRptModel.SearchStartDay    + "]");		// �˻� ������� ����          
//				_log.Debug("SearchEndDay      :[" + dailyAdExecSummaryRptModel.SearchEndDay      + "]");		// �˻� �������� ����          
				// __DEBUG__

				string logDay1   = dailyAdExecSummaryRptModel.LogDay1;
//				string logDay2   = dailyAdExecSummaryRptModel.LogDay2;
//				string weekDay   = dailyAdExecSummaryRptModel.WeekDay;
//				string monthDay  = dailyAdExecSummaryRptModel.MonthDay;

				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@LogDay", SqlDbType.VarChar, 6);
				sqlParams[0].Value = dailyAdExecSummaryRptModel.LogDay1;
								
				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* �ѱⰣ �������� ����																				\n"
					+ "   ��ȸ���� : ��ü�ڵ�, ����ȣ �Ǵ� �����ȣ */														\n"
					+ "																									\n"
					+ "DECLARE @LogDay1 varchar(6);   -- ��ü�ڵ�															\n"
					+ "DECLARE @LogDay2 varchar(6);  -- ��������															\n"
					+ "DECLARE @WeekDay varchar(6);      -- �����ȣ										 				\n"
					+ "DECLARE @MonthDay varchar(6); -- ����ȣ															\n"
					+ "DECLARE @totalAdCnt int;																			\n"
					+ "DECLARE @totalHouse int;																			\n"
					+ "DECLARE @actHouse   int;															 				\n"
										
					+ "SET @LogDay1   = '" + logDay1  + "'\n"
					+ "SET @LogDay2    = convert(varchar(6),dateadd( week,  -1, cast('" + logDay1  + "' as datetime)  ) ,12)		\n"
					+ "SET @WeekDay    = convert(varchar(6),dateadd( week,  -1, cast('" + logDay1  + "' as datetime)+1) ,12)      \n"
					+ "SET @MonthDay   = convert(varchar(6),dateadd( month, -1, cast('" + logDay1  + "' as datetime) + 1) ,12)    \n"
				
					+ "-- ����																									\n"
					+ "select '1.����' as [���ڱ���]                                                                    \n"
					+ "       ,HouseTotal as [�Ѱ�����]                                                                                                  \n"
					+ "       ,HouseHold  as [Ȱ�������]                                                               \n"
					+ "       ,cast(HouseHold as real) / HouseTotal * 100  as [����]                                 \n"
					+ "       ,ChannelHit as [ä��Hit]                                                                 \n"
					+ "       ,ChannelAdHit as [����Hit]                                                               \n"
					+ "       ,cast(ChannelAdHit as real) /  ChannelHit   as [�����]                                 \n"
					+ "       from  dbo.SummaryBase a with(noLock)                                                    \n"
					+ "		  Where   LogDay = @LogDay1																  \n"
					+ "																	                              \n"
					+ "UNION ALL                                 \n"
					+ "                                          \n"
					+ "-- ����	                                 \n"
					+ "select '2.����'		                     \n"
					+ "       ,HouseTotal		                 \n"
					+ "       ,HouseHold	                     \n"
					+ "       ,cast(HouseHold as real) /HouseTotal * 100   as RateActiveHouse						  \n"
					+ "       ,ChannelHit																			  \n"
					+ "       ,ChannelAdHit																			  \n"
					+ "       ,cast(ChannelAdHit as real) /  ChannelHit     as HitAd								  \n"
					+ "       from  dbo.SummaryBase a with(noLock)													  \n"
					+ "       Where LogDay = @LogDay2																  \n"										
					+ "                                                                                               \n"
					+ "UNION ALL                                                                                      \n"
					+ "                                                                                               \n"
					+ "-- �ְ����																					  \n"
					+ "select '3.�ְ����'																			  \n"
					+ "       ,avg(HouseTotal)																		  \n"
					+ "       ,avg(HouseHold)																		  \n"
					+ "       ,cast(avg(HouseHold) as real) /avg(HouseTotal) * 100   as RateActiveHouse				  \n"
					+ "       ,avg(ChannelHit)																		  \n"
					+ "       ,avg(ChannelAdHit)																	  \n"
					+ "       ,cast(avg(ChannelAdHit) as real) /  avg(ChannelHit)     as HitAd						  \n"					
					+ "  from  dbo.SummaryBase a with(noLock)														  \n"
					+ "  Where LogDay between @WeekDay and @LogDay1													  \n"					
					+ "																								  \n"
					+ "UNION ALL																					  \n"
					+ "																								  \n"
					+ "-- �ʼ��� ����																					  \n"
					+ "select '4.�������'																			  \n"
					+ "       ,avg(HouseTotal)	  \n"
					+ "       ,avg(HouseHold)   \n"
					+ "       ,cast(avg(HouseHold) as real) /avg(HouseTotal) * 100   as RateActiveHouse				  \n"
					+ "       ,avg(ChannelHit)																		  \n"
					+ "       ,avg(ChannelAdHit)																	  \n"
					+ "       ,cast(avg(ChannelAdHit) as real) /  avg(ChannelHit)     as HitAd						  \n"					
					+ "  from  dbo.SummaryBase a with(noLock)														  \n"
					+ "  Where LogDay between @MonthDay and @LogDay1												  \n"						
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				dailyAdExecSummaryRptModel.ReportDataSet = ds.Copy();

				// ���
				dailyAdExecSummaryRptModel.ResultCnt = Utility.GetDatasetCount(dailyAdExecSummaryRptModel.ReportDataSet);

				ds.Dispose();

				// ��������
				sbQuery = new StringBuilder();				
				sbQuery.Append(""					
					+ "DECLARE @LogDay1 varchar(6);																		\n"
					+ "DECLARE @totalAdCnt int;																			\n"
					+ "DECLARE @totalHouse int;																			\n"
					+ "DECLARE @actHouse   int;															 				\n"
					+ "																									\n"
					+ "SET @LogDay1   = '" + logDay1  + "'																\n"
					+ "-- ���Ϻ� ����						                                                         \n"
					+ "SELECT  @totalAdCnt = ChannelAdHit                                                        \n"
					+ "       ,@totalHouse = HouseTotal                                                          \n"
					+ "       ,@actHouse = HouseHold                                                             \n"					
					+ "  FROM dbo.SummaryBase a with(noLock)	                                                 \n"
					+ "  Where   LogDay = @LogDay1															     \n"					
					+ "																							 \n"	
					+ "-- ���ɺ� ����                                                                                \n"
                    + "SELECT  '[' + cast(hou.TypeCode as varchar(2)) + '] ' + (select CodeName from AdTargetsHanaTV.dbo.SystemCode sy where sy.Section = hou.TypeSection and sy.Code = hou.TypeCode)      as [������]                                                                        \n"
					+ "       , itm.AdCnt         as [���⹰��]													   \n"
					+ "       , cast(itm.AdCnt as real) / @totalAdCnt * 100 as [������]							   \n"
					+ "       , hou.DayUsers  as [������]															   \n"
					+ "       , cast(hou.DayUsers as real) / @totalHouse * 100 as [���޷�]						   \n"
					+ "       , cast(itm.AdCnt  as real) / hou.DayUsers  as [�����]							   \n"
					+ "       , cast(hou.DayUsers as real) / @actHouse * 100 as [����������]						   \n"
					+ " from (			                                                                           \n"
					+ "       SELECT  *																			   \n"
					+ "       FROM    SummaryHouseHoldType a with(noLock)                 \n"
					+ "       Where   LogDay = @LogDay1                                                            \n"
					+ "       and     TypeSection = 26 ) hou                                                       \n"
					+ "   inner join				                                                               \n"
					+ "     (  select  b.AdType																       \n"
					+ "				,sum(a.AdCnt) as AdCnt		                                                   \n"
					+ "       from  SummaryAdDaily0 a with(noLock)												   \n"
                    + " 	  inner join AdTargetsHanaTV.dbo.ContractItem b with(noLock) on a.ItemNo = b.ItemNo and b.AdType < '90'          \n"					
					+ " 	  Where a.LogDay = @LogDay1                                                                  \n"
					+ " 	    and   a.ItemNo  > 0                                                                      \n"
					+ " 	    and   a.ContractSeq = 0                                                                  \n"
					+ " 	    and   a.SummaryType = 1                                                                  \n"
					+ " 	    and   a.SummaryCode = 1                                                                  \n"
					+ " 		Group by b.AdType ) itm on hou.TypeCode = itm.AdType                                     \n"
					+ "union all                                                                                         \n"
					+ "select  '[99] ��ü'																				 \n"
					+ "        ,@totalAdCnt																				 \n"
					+ "        ,0																					     \n"
					+ "        ,@actHouse																				 \n"
					+ "        ,0																						 \n"
					+ "        ,cast(@totalAdCnt  as real) / @actHouse													 \n"
					+ "        ,0																						 \n"					
					);
				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				dailyAdExecSummaryRptModel.ItemDataSet = ds.Copy();

				// ���
				dailyAdExecSummaryRptModel.ResultCnt = Utility.GetDatasetCount(dailyAdExecSummaryRptModel.ItemDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				dailyAdExecSummaryRptModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + dailyAdExecSummaryRptModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDailyAdExecSummary() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				dailyAdExecSummaryRptModel.ResultCD = "3000";
				if(isNotReady)
				{
					dailyAdExecSummaryRptModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					dailyAdExecSummaryRptModel.ResultDesc = "�ѱⰣ �������� ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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