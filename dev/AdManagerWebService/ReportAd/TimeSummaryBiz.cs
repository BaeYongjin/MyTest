// ===============================================================================
//
// TimeSummaryBiz.cs
//
// �����û��Ȳ ���� 
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
 * Class Name: TimeSummaryBiz
 * �ֿ���  : �����û��Ȳ ó�� ����
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

namespace AdManagerWebService.ReportAd
{
	/// <summary>
	/// TimeSummaryBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class TimeSummaryBiz : BaseBiz
	{

		#region  ������
		public TimeSummaryBiz() : base(FrameSystem.connSummaryDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region ����-�ð��� ����
		/// <summary>
		/// ���� ��û��Ȳ ����
		/// </summary>admin	
		/// <param name="timeSummaryModel"></param>
		public void GetAreaTime(HeaderModel header, TimeSummaryModel data)
		{
			try
			{
				StringBuilder sbQuery = null;
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAreaTime() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);

                string sqlItems = "";
                if( data.AdList.Count > 0 )
                {
                    for( int i = 0; i < data.AdList.Count;i++)
                    {
                        if( i == 0 )    sqlItems = data.AdList[i].ToString();
                        else            sqlItems += "," + data.AdList[i].ToString();
                    }
                }
				
				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");		// �˻� ��������           
				_log.Debug("EndDay   :[" + data.EndDay      + "]");		// �˻� Ű����   
                _log.Debug("Items    :[" + sqlItems                     + "]");
				// __DEBUG__


				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- ���� ��û��Ȳ        \n"					
					+ "SELECT v1.region         -- �����ȣ																\n"   
					+ "      ,( select summaryName from SummaryCode nolock where SummaryType = 5 and SummaryCode = v1.region) as RegionName										\n"
					+ "      ,SUM([H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM([H02]) AS  H02 ,SUM([H03]) AS  H03										\n"		
					+ "      ,SUM([H04]) AS  H04 ,SUM([H05]) AS  H05 ,SUM([H06]) AS  H06 ,SUM([H07]) AS  H07									\n" 					
					+ "      ,SUM([H08]) AS  H08 ,SUM([H09]) AS  H09 ,SUM([H10]) AS  H10 ,SUM([H11]) AS  H11								    \n"
					+ "      ,SUM([H12]) AS  H12 ,SUM([H13]) AS  H13 ,SUM([H14]) AS  H14 ,SUM([H15]) AS  H15									\n"
					+ "      ,SUM([H16]) AS  H16 ,SUM([H17]) AS  H17 ,SUM([H18]) AS  H18 ,SUM([H19]) AS  H19									\n"
					+ "      ,SUM([H20]) AS  H20 ,SUM([H21]) AS  H21 ,SUM([H22]) AS  H22 ,SUM([H23]) AS  H23									\n"					
					+ "from (																																\n"
					+ "      select  SUM(isnull(H00,0)) AS  H00 ,SUM(isnull(H01,0)) AS  H01 ,SUM(isnull(H02,0)) AS  H02 ,SUM(isnull(H03,0)) AS  H03												\n"
					+ "             ,SUM(isnull(H04,0)) AS  H04 ,SUM(isnull(H05,0)) AS  H05 ,SUM(isnull(H06,0)) AS  H06 ,SUM(isnull(H07,0)) AS  H07												\n"
					+ "             ,SUM(isnull(H08,0)) AS  H08 ,SUM(isnull(H09,0)) AS  H09 ,SUM(isnull(H10,0)) AS  H10 ,SUM(isnull(H11,0)) AS  H11												\n"
					+ "             ,SUM(isnull(H12,0)) AS  H12 ,SUM(isnull(H13,0)) AS  H13 ,SUM(isnull(H14,0)) AS  H14 ,SUM(isnull(H15,0)) AS  H15												\n"
					+ "             ,SUM(isnull(H16,0)) AS  H16 ,SUM(isnull(H17,0)) AS  H17 ,SUM(isnull(H18,0)) AS  H18 ,SUM(isnull(H19,0)) AS  H19												\n"
					+ "             ,SUM(isnull(H20,0)) AS  H20 ,SUM(isnull(H21,0)) AS  H21 ,SUM(isnull(H22,0)) AS  H22 ,SUM(isnull(H23,0)) AS  H23												\n"
					+ "             ,case b.Level	when 1 then b.RegionCode else b.UpperCode end as region													\n"
					+ "             ,sum(a.AdCnt) as Cnt																											\n"					
					+ "      FROM dbo.SummaryAdDaily0 a with(NoLock)																						\n"
                    + "      inner join AdTargetsHanaTV.dbo.TargetRegion b with(NoLock) on a.SummaryCode = b.RegionCode		\n"
                    + "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
                    + "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "      and   a.ContractSeq = 0																										\n"
					+ "      and   a.SummaryType = 5																										\n" 
					+ "      group by case b.Level	when 1 then b.RegionCode else b.UpperCode end )v1													\n"					
					+ "group by v1.region																													\n"					
					+ "order by v1.region																													\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				data.ReportDataSet = ds.Copy();

				// ���
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// ����ڵ� ��Ʈ
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAreaTime() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "������ �ð����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
		#endregion

		#region ����-�ð��� ����
		/// <summary>
		/// ���� ��û��Ȳ ����
		/// </summary>
		/// <param name="timeSummaryModel"></param>
		public void GetDateTime(HeaderModel header, TimeSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateTime() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);
				
                string sqlItems = "";
                if( data.AdList.Count > 0 )
                {
                    for( int i = 0; i < data.AdList.Count;i++)
                    {
                        if( i == 0 )    sqlItems = data.AdList[i].ToString();
                        else            sqlItems += "," + data.AdList[i].ToString();
                    }
                }

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("StartDay :[" + data.StartDay    + "]");
                _log.Debug("EndDay   :[" + data.EndDay      + "]");
                _log.Debug("Items    :[" + sqlItems         + "]");
                // __DEBUG__


				// ��������
				sbQuery = new StringBuilder();

				// 2.2.7 JOIN���� ����
				// �ش� �����ڵ�� 1�ܰ��̰�, �����ڵ�� 3�ܰ������� ���� ����.
				sbQuery.Append("\n"
					+ "-- ���� ��û��Ȳ        \n"					
					+ "SELECT '20' + v1.LogDay as LogDay         -- �����ȣ								      																	\n"   
					+ "      ,SUM(v1.[H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM(v1.[H02]) AS  H02 ,SUM(v1.[H03]) AS  H03 											\n"
					+ "      ,SUM(v1.[H04]) AS  H04 ,SUM(v1.[H05]) AS  H05 ,SUM(v1.[H06]) AS  H06 ,SUM(v1.[H07]) AS  H07										\n"		
					+ "      ,SUM(v1.[H08]) AS  H08 ,SUM(v1.[H09]) AS  H09 ,SUM(v1.[H10]) AS  H10 ,SUM(v1.[H11]) AS  H11										\n" 					
					+ "      ,SUM(v1.[H12]) AS  H12 ,SUM(v1.[H13]) AS  H13 ,SUM(v1.[H14]) AS  H14 ,SUM(v1.[H15]) AS  H15									    \n"
					+ "      ,SUM(v1.[H16]) AS  H16 ,SUM(v1.[H17]) AS  H17 ,SUM(v1.[H18]) AS  H18 ,SUM(v1.[H19]) AS  H19										\n"
					+ "      ,SUM(v1.[H20]) AS  H20 ,SUM(v1.[H21]) AS  H21 ,SUM(v1.[H22]) AS  H22 ,SUM(v1.[H23]) AS  H23										\n"					
					+ "from (																									\n"
					+ "      SELECT a.LogDay																					\n"					
					+ "			,SUM(isnull(H00,0)) AS  H00 ,SUM(isnull(H01,0)) AS  H01 ,SUM(isnull(H02,0)) AS  H02 ,SUM(isnull(H03,0)) AS  H03					\n"
					+ "			,SUM(isnull(H04,0)) AS  H04 ,SUM(isnull(H05,0)) AS  H05 ,SUM(isnull(H06,0)) AS  H06 ,SUM(isnull(H07,0)) AS  H07					\n"
					+ "			,SUM(isnull(H08,0)) AS  H08 ,SUM(isnull(H09,0)) AS  H09 ,SUM(isnull(H10,0)) AS  H10 ,SUM(isnull(H11,0)) AS  H11					\n"
					+ "			,SUM(isnull(H12,0)) AS  H12 ,SUM(isnull(H13,0)) AS  H13 ,SUM(isnull(H14,0)) AS  H14 ,SUM(isnull(H15,0)) AS  H15					\n"
					+ "			,SUM(isnull(H16,0)) AS  H16 ,SUM(isnull(H17,0)) AS  H17 ,SUM(isnull(H18,0)) AS  H18 ,SUM(isnull(H19,0)) AS  H19					\n"
					+ "			,SUM(isnull(H20,0)) AS  H20 ,SUM(isnull(H21,0)) AS  H21 ,SUM(isnull(H22,0)) AS  H22 ,SUM(isnull(H23,0)) AS  H23					\n"
					+ "			,sum(a.AdCnt) as Cnt																	\n" 					
					+ "     FROM dbo.SummaryAdDaily0 a with(NoLock)													    \n"
					+ "     --inner join dbo.SummaryCode b with(NoLock) on a.Summarytype = b.SummaryType and a.SummaryCode = b.SummaryCode					    \n"
                    + "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
                    + "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "      and   a.ContractSeq = 0																	\n" 
					+ "      and   a.SummaryType = 5																	\n"
					+ "      group by a.LogDay)v1																	    \n"
					+ "group by v1.LogDay																				\n"					
					+ "order by v1.LogDay																				\n"					
					);
			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				data.ReportDataSet = ds.Copy();

				// ���
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// ����ڵ� ��Ʈ
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateTime() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "���ں� �ð����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
		#endregion

		#region �帣�� �ð�����
		/// <summary>
		/// ���� ��û��Ȳ ����
		/// </summary>
		/// <param name="timeSummaryModel"></param>
		public void GetGenreTime(HeaderModel header, TimeSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreTime() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);
				
                string sqlItems = "";
                if( data.AdList.Count > 0 )
                {
                    for( int i = 0; i < data.AdList.Count;i++)
                    {
                        if( i == 0 )    sqlItems = data.AdList[i].ToString();
                        else            sqlItems += "," + data.AdList[i].ToString();
                    }
                }

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("StartDay :[" + data.StartDay    + "]");
                _log.Debug("EndDay   :[" + data.EndDay      + "]");
                _log.Debug("Items    :[" + sqlItems         + "]");
                // __DEBUG__

				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- ���� ��û��Ȳ        \n"
                    + "SELECT v1.Category, (select CategoryName  from AdTargetsHanaTV.dbo.Category where CategoryCode = v1.Category) as CategoryNm         -- �����ȣ								      								\n"
                    + "      ,v1.Genre,    (select GenreName     from AdTargetsHanaTV.dbo.Genre    where GenreCode = v1.Genre)       as GenreNm 									\n"
					+ "      ,SUM(v1.[H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM(v1.[H02]) AS  H02 ,SUM(v1.[H03]) AS  H03											\n"		
					+ "      ,SUM(v1.[H04]) AS  H04 ,SUM(v1.[H05]) AS  H05 ,SUM(v1.[H06]) AS  H06 ,SUM(v1.[H07]) AS  H07										\n" 					
					+ "      ,SUM(v1.[H08]) AS  H08 ,SUM(v1.[H09]) AS  H09 ,SUM(v1.[H10]) AS  H10 ,SUM(v1.[H11]) AS  H11									    \n"
					+ "      ,SUM(v1.[H12]) AS  H12 ,SUM(v1.[H13]) AS  H13 ,SUM(v1.[H14]) AS  H14 ,SUM(v1.[H15]) AS  H15										\n"
					+ "      ,SUM(v1.[H16]) AS  H16 ,SUM(v1.[H17]) AS  H17 ,SUM(v1.[H18]) AS  H18 ,SUM(v1.[H19]) AS  H19										\n"
					+ "      ,SUM(v1.[H20]) AS  H20 ,SUM(v1.[H21]) AS  H21 ,SUM(v1.[H22]) AS  H22 ,SUM(v1.[H23]) AS  H23										\n"										
					+ "from (																							\n"
					+ "      SELECT																						\n"
					+ "			 a.Category																				\n"
					+ "			,a.Genre																				\n"
					+ "			,SUM([H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM([H02]) AS  H02 ,SUM([H03]) AS  H03																	\n"
					+ "			,SUM([H04]) AS  H04 ,SUM([H05]) AS  H05 ,SUM([H06]) AS  H06 ,SUM([H07]) AS  H07																	\n"
					+ "			,SUM([H08]) AS  H08 ,SUM([H09]) AS  H09 ,SUM([H10]) AS  H10 ,SUM([H11]) AS  H11																	\n"
					+ "			,SUM([H12]) AS  H12 ,SUM([H13]) AS  H13 ,SUM([H14]) AS  H14 ,SUM([H15]) AS  H15																	\n"
					+ "			,SUM([H16]) AS  H16 ,SUM([H17]) AS  H17 ,SUM([H18]) AS  H18 ,SUM([H19]) AS  H19																	\n"
					+ "			,SUM([H20]) AS  H20 ,SUM([H21]) AS  H21 ,SUM([H22]) AS  H22 ,SUM([H23]) AS  H23																	\n"					
					+ "		 FROM dbo.SummaryAdDaily3 a with(NoLock)													\n"					
                    + "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
                    + "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "		   and   a.ContractSeq = 0																	\n" 					
					+ "			group by a.Category,a.Genre ) v1													\n"					
					+ "group by v1.Category, v1.Genre																	\n"					
					+ "order by v1.Category, v1.Genre																	\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				data.ReportDataSet = ds.Copy();

				// ���
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// ����ڵ� ��Ʈ
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreTime() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "�帣�� �ð����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
		#endregion

		#region ä�κ� ��������
		/// <summary>
		/// ���� ��û��Ȳ ����
		/// </summary>
		/// <param name="timeSummaryModel"></param>
		public void GetChannelTime(HeaderModel header, TimeSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelTime() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);

                string sqlItems = "";
                if( data.AdList.Count > 0 )
                {
                    for( int i = 0; i < data.AdList.Count;i++)
                    {
                        if( i == 0 )    sqlItems = data.AdList[i].ToString();
                        else            sqlItems += "," + data.AdList[i].ToString();
                    }
                }

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("StartDay :[" + data.StartDay    + "]");
                _log.Debug("EndDay   :[" + data.EndDay      + "]");
                _log.Debug("Items    :[" + sqlItems         + "]");
                // __DEBUG__
				


				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- ���� ��û��Ȳ        \n"
                    + "select v1.Category, (select CategoryName  from AdTargetsHanaTV.dbo.Category where CategoryCode = v1.Category) as CategoryNm         -- �����ȣ								      								\n"
                    + "      ,v1.Genre,    (select GenreName     from AdTargetsHanaTV.dbo.Genre    where GenreCode = v1.Genre)       as GenreNm 			\n"
                    + "      ,v1.progKey,  (select ProgramNm     from AdTargetsHanaTV.dbo.Program  where Programkey= v1.Progkey)       as ChannelNm			\n"		
					+ "      ,SUM(v1.[H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM(v1.[H02]) AS  H02 ,SUM(v1.[H03]) AS  H03				\n" 					
					+ "      ,SUM(v1.[H04]) AS  H04 ,SUM(v1.[H05]) AS  H05 ,SUM(v1.[H06]) AS  H06 ,SUM(v1.[H07]) AS  H07		    \n"
					+ "      ,SUM(v1.[H08]) AS  H08 ,SUM(v1.[H09]) AS  H09 ,SUM(v1.[H10]) AS  H10 ,SUM(v1.[H11]) AS  H11			\n"
					+ "      ,SUM(v1.[H12]) AS  H12 ,SUM(v1.[H13]) AS  H13 ,SUM(v1.[H14]) AS  H14 ,SUM(v1.[H15]) AS  H15			\n"
					+ "      ,SUM(v1.[H16]) AS  H16 ,SUM(v1.[H17]) AS  H17 ,SUM(v1.[H18]) AS  H18 ,SUM(v1.[H19]) AS  H19			\n"					
					+ "      ,SUM(v1.[H20]) AS  H20 ,SUM(v1.[H21]) AS  H21 ,SUM(v1.[H22]) AS  H22 ,SUM(v1.[H23]) AS  H23			\n"										
					+ "from (																										\n"
					+ "      SELECT 																\n"
					+ "			 a.Category															\n"
					+ "			,a.Genre															\n"
					+ "			,a.Progkey															\n"
					+ "			,sum(a.AdCnt) as Cnt												\n"
					+ "			,SUM([H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM([H02]) AS  H02 ,SUM([H03]) AS  H03												\n"
					+ "			,SUM([H04]) AS  H04 ,SUM([H05]) AS  H05 ,SUM([H06]) AS  H06 ,SUM([H07]) AS  H07												\n"
					+ "			,SUM([H08]) AS  H08 ,SUM([H09]) AS  H09 ,SUM([H10]) AS  H10 ,SUM([H11]) AS  H11												\n"
					+ "			,SUM([H12]) AS  H12 ,SUM([H13]) AS  H13 ,SUM([H14]) AS  H14 ,SUM([H15]) AS  H15												\n"
					+ "			,SUM([H16]) AS  H16 ,SUM([H17]) AS  H17 ,SUM([H18]) AS  H18 ,SUM([H19]) AS  H19												\n"
					+ "			,SUM([H20]) AS  H20 ,SUM([H21]) AS  H21 ,SUM([H22]) AS  H22 ,SUM([H23]) AS  H23												\n"
					+ "			FROM dbo.SummaryAdDaily3 a with(NoLock)								\n"					
                    + "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
                    + "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "			and   a.ContractSeq = 0																	\n" 
					+ "			and   a.progKey > 0																		\n"
					+ "			group by a.Category,a.Genre,a.ProgKey ) v1										\n"										
					+ "group by v1.Category, v1.Genre,v1.ProgKey																	\n"					
					+ "order by v1.Category, v1.Genre,v1.ProgKey																	\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				data.ReportDataSet = ds.Copy();

				// ���
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// ����ڵ� ��Ʈ
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelTime() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "ä�κ� �������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
		#endregion

		#region ����-�ð��� ����(�ڹٿ�)
		/// <summary>
		/// ���� ��û��Ȳ ����(�ڹٿ�)
		/// </summary>admin	
		/// <param name="timeSummaryModel"></param>
		public void GetAreaTimeByJava(HeaderModel header, TimeSummaryModel data)
		{
			try
			{
				StringBuilder sbQuery = null;
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAreaTime() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);

				string sqlItems = "";
				if( data.AdListStr.Length > 0 )
				{
					for( int i = 0; i < data.AdListStr.Length;i++)
					{
						if( i == 0 )    sqlItems = data.AdListStr[i];
						else            sqlItems += "," + data.AdListStr[i];
					}
				}
				
				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");		// �˻� ��������           
				_log.Debug("EndDay   :[" + data.EndDay      + "]");		// �˻� Ű����   
				_log.Debug("Items    :[" + sqlItems                     + "]");
				// __DEBUG__


				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- ���� ��û��Ȳ        \n"					
					+ "SELECT v1.region         -- �����ȣ																\n"   
					+ "      ,( select summaryName from SummaryCode nolock where SummaryType = 5 and SummaryCode = v1.region) as RegionName										\n"
					+ "      ,SUM([H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM([H02]) AS  H02 ,SUM([H03]) AS  H03										\n"		
					+ "      ,SUM([H04]) AS  H04 ,SUM([H05]) AS  H05 ,SUM([H06]) AS  H06 ,SUM([H07]) AS  H07									\n" 					
					+ "      ,SUM([H08]) AS  H08 ,SUM([H09]) AS  H09 ,SUM([H10]) AS  H10 ,SUM([H11]) AS  H11								    \n"
					+ "      ,SUM([H12]) AS  H12 ,SUM([H13]) AS  H13 ,SUM([H14]) AS  H14 ,SUM([H15]) AS  H15									\n"
					+ "      ,SUM([H16]) AS  H16 ,SUM([H17]) AS  H17 ,SUM([H18]) AS  H18 ,SUM([H19]) AS  H19									\n"
					+ "      ,SUM([H20]) AS  H20 ,SUM([H21]) AS  H21 ,SUM([H22]) AS  H22 ,SUM([H23]) AS  H23									\n"					
					+ "from (																																\n"
					+ "      select  SUM(isnull(H00,0)) AS  H00 ,SUM(isnull(H01,0)) AS  H01 ,SUM(isnull(H02,0)) AS  H02 ,SUM(isnull(H03,0)) AS  H03												\n"
					+ "             ,SUM(isnull(H04,0)) AS  H04 ,SUM(isnull(H05,0)) AS  H05 ,SUM(isnull(H06,0)) AS  H06 ,SUM(isnull(H07,0)) AS  H07												\n"
					+ "             ,SUM(isnull(H08,0)) AS  H08 ,SUM(isnull(H09,0)) AS  H09 ,SUM(isnull(H10,0)) AS  H10 ,SUM(isnull(H11,0)) AS  H11												\n"
					+ "             ,SUM(isnull(H12,0)) AS  H12 ,SUM(isnull(H13,0)) AS  H13 ,SUM(isnull(H14,0)) AS  H14 ,SUM(isnull(H15,0)) AS  H15												\n"
					+ "             ,SUM(isnull(H16,0)) AS  H16 ,SUM(isnull(H17,0)) AS  H17 ,SUM(isnull(H18,0)) AS  H18 ,SUM(isnull(H19,0)) AS  H19												\n"
					+ "             ,SUM(isnull(H20,0)) AS  H20 ,SUM(isnull(H21,0)) AS  H21 ,SUM(isnull(H22,0)) AS  H22 ,SUM(isnull(H23,0)) AS  H23												\n"
					+ "             ,case b.UpperCode when 0 then b.SummaryCode else b.UpperCode end  as region													\n"
					+ "             ,sum(a.AdCnt) as Cnt																											\n"					
					+ "      FROM dbo.SummaryAdDaily0 a with(NoLock)																						\n"
					+ "      inner join dbo.SummaryCode b with(NoLock) on a.Summarytype = b.SummaryType and a.SummaryCode = b.SummaryCode					\n"
					+ "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
					+ "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "      and   a.ContractSeq = 0																										\n"
					+ "      and   a.SummaryType = 5																										\n" 
					+ "      group by case b.UpperCode when 0 then b.SummaryCode else b.UpperCode end )v1													\n"					
					+ "group by v1.region																													\n"					
					+ "order by v1.region																													\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				data.ReportDataSet = ds.Copy();

				// ���
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// ����ڵ� ��Ʈ
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAreaTime() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "������ �ð����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
		#endregion

		#region ����-�ð��� ����(�ڹٿ�)
		/// <summary>
		/// ���� ��û��Ȳ ����(�ڹٿ�)
		/// </summary>
		/// <param name="timeSummaryModel"></param>
		public void GetDateTimeByJava(HeaderModel header, TimeSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateTime() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);
				
				string sqlItems = "";
				if( data.AdListStr.Length > 0 )
				{
					for( int i = 0; i < data.AdListStr.Length;i++)
					{
						if( i == 0 )    sqlItems = data.AdListStr[i];
						else            sqlItems += "," + data.AdListStr[i];
					}
				}

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");
				_log.Debug("EndDay   :[" + data.EndDay      + "]");
				_log.Debug("Items    :[" + sqlItems         + "]");
				// __DEBUG__


				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- ���� ��û��Ȳ        \n"					
					+ "SELECT '20' + v1.LogDay as LogDay         -- �����ȣ								      																	\n"   
					+ "      ,SUM(v1.[H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM(v1.[H02]) AS  H02 ,SUM(v1.[H03]) AS  H03 											\n"
					+ "      ,SUM(v1.[H04]) AS  H04 ,SUM(v1.[H05]) AS  H05 ,SUM(v1.[H06]) AS  H06 ,SUM(v1.[H07]) AS  H07										\n"		
					+ "      ,SUM(v1.[H08]) AS  H08 ,SUM(v1.[H09]) AS  H09 ,SUM(v1.[H10]) AS  H10 ,SUM(v1.[H11]) AS  H11										\n" 					
					+ "      ,SUM(v1.[H12]) AS  H12 ,SUM(v1.[H13]) AS  H13 ,SUM(v1.[H14]) AS  H14 ,SUM(v1.[H15]) AS  H15									    \n"
					+ "      ,SUM(v1.[H16]) AS  H16 ,SUM(v1.[H17]) AS  H17 ,SUM(v1.[H18]) AS  H18 ,SUM(v1.[H19]) AS  H19										\n"
					+ "      ,SUM(v1.[H20]) AS  H20 ,SUM(v1.[H21]) AS  H21 ,SUM(v1.[H22]) AS  H22 ,SUM(v1.[H23]) AS  H23										\n"					
					+ "from (																									\n"
					+ "      SELECT a.LogDay																					\n"					
					+ "			,SUM(isnull(H00,0)) AS  H00 ,SUM(isnull(H01,0)) AS  H01 ,SUM(isnull(H02,0)) AS  H02 ,SUM(isnull(H03,0)) AS  H03					\n"
					+ "			,SUM(isnull(H04,0)) AS  H04 ,SUM(isnull(H05,0)) AS  H05 ,SUM(isnull(H06,0)) AS  H06 ,SUM(isnull(H07,0)) AS  H07					\n"
					+ "			,SUM(isnull(H08,0)) AS  H08 ,SUM(isnull(H09,0)) AS  H09 ,SUM(isnull(H10,0)) AS  H10 ,SUM(isnull(H11,0)) AS  H11					\n"
					+ "			,SUM(isnull(H12,0)) AS  H12 ,SUM(isnull(H13,0)) AS  H13 ,SUM(isnull(H14,0)) AS  H14 ,SUM(isnull(H15,0)) AS  H15					\n"
					+ "			,SUM(isnull(H16,0)) AS  H16 ,SUM(isnull(H17,0)) AS  H17 ,SUM(isnull(H18,0)) AS  H18 ,SUM(isnull(H19,0)) AS  H19					\n"
					+ "			,SUM(isnull(H20,0)) AS  H20 ,SUM(isnull(H21,0)) AS  H21 ,SUM(isnull(H22,0)) AS  H22 ,SUM(isnull(H23,0)) AS  H23					\n"
					+ "			,sum(a.AdCnt) as Cnt																	\n" 					
					+ "     FROM dbo.SummaryAdDaily0 a with(NoLock)													    \n"
					+ "      inner join dbo.SummaryCode b with(NoLock) on a.Summarytype = b.SummaryType and a.SummaryCode = b.SummaryCode					        \n"
					+ "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
					+ "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "      and   a.ContractSeq = 0																	\n" 
					+ "      and   a.SummaryType = 5																	\n"
					+ "      group by a.LogDay)v1																	    \n"
					+ "group by v1.LogDay																				\n"					
					+ "order by v1.LogDay																				\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				data.ReportDataSet = ds.Copy();

				// ���
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// ����ڵ� ��Ʈ
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateTime() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "���ں� �ð����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
		#endregion

		#region �帣�� �ð�����(�ڹٿ�)
		/// <summary>
		/// ���� ��û��Ȳ ����(�ڹٿ�)
		/// </summary>
		/// <param name="timeSummaryModel"></param>
		public void GetGenreTimeByJava(HeaderModel header, TimeSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreTime() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);
				
				string sqlItems = "";
				if( data.AdListStr.Length > 0 )
				{
					for( int i = 0; i < data.AdListStr.Length;i++)
					{
						if( i == 0 )    sqlItems = data.AdListStr[i];
						else            sqlItems += "," + data.AdListStr[i];
					}
				}

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");
				_log.Debug("EndDay   :[" + data.EndDay      + "]");
				_log.Debug("Items    :[" + sqlItems         + "]");
				// __DEBUG__

				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- ���� ��û��Ȳ        \n"
                    + "SELECT v1.Category, (select CategoryName  from AdTargetsHanaTV.dbo.Category where CategoryCode = v1.Category) as CategoryNm         -- �����ȣ								      								\n"
                    + "      ,v1.Genre,    (select GenreName     from AdTargetsHanaTV.dbo.Genre    where GenreCode = v1.Genre)       as GenreNm 									\n"
					+ "      ,SUM(v1.[H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM(v1.[H02]) AS  H02 ,SUM(v1.[H03]) AS  H03											\n"		
					+ "      ,SUM(v1.[H04]) AS  H04 ,SUM(v1.[H05]) AS  H05 ,SUM(v1.[H06]) AS  H06 ,SUM(v1.[H07]) AS  H07										\n" 					
					+ "      ,SUM(v1.[H08]) AS  H08 ,SUM(v1.[H09]) AS  H09 ,SUM(v1.[H10]) AS  H10 ,SUM(v1.[H11]) AS  H11									    \n"
					+ "      ,SUM(v1.[H12]) AS  H12 ,SUM(v1.[H13]) AS  H13 ,SUM(v1.[H14]) AS  H14 ,SUM(v1.[H15]) AS  H15										\n"
					+ "      ,SUM(v1.[H16]) AS  H16 ,SUM(v1.[H17]) AS  H17 ,SUM(v1.[H18]) AS  H18 ,SUM(v1.[H19]) AS  H19										\n"
					+ "      ,SUM(v1.[H20]) AS  H20 ,SUM(v1.[H21]) AS  H21 ,SUM(v1.[H22]) AS  H22 ,SUM(v1.[H23]) AS  H23										\n"										
					+ "from (																							\n"
					+ "      SELECT																						\n"
					+ "			 a.Category																				\n"
					+ "			,a.Genre																				\n"
					+ "			,SUM([H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM([H02]) AS  H02 ,SUM([H03]) AS  H03																	\n"
					+ "			,SUM([H04]) AS  H04 ,SUM([H05]) AS  H05 ,SUM([H06]) AS  H06 ,SUM([H07]) AS  H07																	\n"
					+ "			,SUM([H08]) AS  H08 ,SUM([H09]) AS  H09 ,SUM([H10]) AS  H10 ,SUM([H11]) AS  H11																	\n"
					+ "			,SUM([H12]) AS  H12 ,SUM([H13]) AS  H13 ,SUM([H14]) AS  H14 ,SUM([H15]) AS  H15																	\n"
					+ "			,SUM([H16]) AS  H16 ,SUM([H17]) AS  H17 ,SUM([H18]) AS  H18 ,SUM([H19]) AS  H19																	\n"
					+ "			,SUM([H20]) AS  H20 ,SUM([H21]) AS  H21 ,SUM([H22]) AS  H22 ,SUM([H23]) AS  H23																	\n"					
					+ "		 FROM dbo.SummaryAdDaily3 a with(NoLock)													\n"					
					+ "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
					+ "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "		   and   a.ContractSeq = 0																	\n" 					
					+ "			group by a.Category,a.Genre ) v1													\n"					
					+ "group by v1.Category, v1.Genre																	\n"					
					+ "order by v1.Category, v1.Genre																	\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				data.ReportDataSet = ds.Copy();

				// ���
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// ����ڵ� ��Ʈ
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreTime() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "�帣�� �ð����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
		#endregion

		#region ä�κ� ��������(�ڹٿ�)
		/// <summary>
		/// ���� ��û��Ȳ ����(�ڹٿ�)
		/// </summary>
		/// <param name="timeSummaryModel"></param>
		public void GetChannelTimeByJava(HeaderModel header, TimeSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelTime() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);

				string sqlItems = "";
				if( data.AdListStr.Length > 0 )
				{
					for( int i = 0; i < data.AdListStr.Length;i++)
					{
						if( i == 0 )    sqlItems = data.AdListStr[i];
						else            sqlItems += "," + data.AdListStr[i];
					}
				}

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");
				_log.Debug("EndDay   :[" + data.EndDay      + "]");
				_log.Debug("Items    :[" + sqlItems         + "]");
				// __DEBUG__
				


				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- ���� ��û��Ȳ        \n"
                    + "select v1.Category, (select CategoryName  from AdTargetsHanaTV.dbo.Category where CategoryCode = v1.Category) as CategoryNm         -- �����ȣ								      								\n"
                    + "      ,v1.Genre,    (select GenreName     from AdTargetsHanaTV.dbo.Genre    where GenreCode = v1.Genre)       as GenreNm 			\n"
                    + "      ,v1.progKey,  (select ProgramNm     from AdTargetsHanaTV.dbo.Program  where Programkey= v1.Progkey)       as ChannelNm			\n"		
					+ "      ,SUM(v1.[H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM(v1.[H02]) AS  H02 ,SUM(v1.[H03]) AS  H03				\n" 					
					+ "      ,SUM(v1.[H04]) AS  H04 ,SUM(v1.[H05]) AS  H05 ,SUM(v1.[H06]) AS  H06 ,SUM(v1.[H07]) AS  H07		    \n"
					+ "      ,SUM(v1.[H08]) AS  H08 ,SUM(v1.[H09]) AS  H09 ,SUM(v1.[H10]) AS  H10 ,SUM(v1.[H11]) AS  H11			\n"
					+ "      ,SUM(v1.[H12]) AS  H12 ,SUM(v1.[H13]) AS  H13 ,SUM(v1.[H14]) AS  H14 ,SUM(v1.[H15]) AS  H15			\n"
					+ "      ,SUM(v1.[H16]) AS  H16 ,SUM(v1.[H17]) AS  H17 ,SUM(v1.[H18]) AS  H18 ,SUM(v1.[H19]) AS  H19			\n"					
					+ "      ,SUM(v1.[H20]) AS  H20 ,SUM(v1.[H21]) AS  H21 ,SUM(v1.[H22]) AS  H22 ,SUM(v1.[H23]) AS  H23			\n"										
					+ "from (																										\n"
					+ "      SELECT 																\n"
					+ "			 a.Category															\n"
					+ "			,a.Genre															\n"
					+ "			,a.Progkey															\n"
					+ "			,sum(a.AdCnt) as Cnt												\n"
					+ "			,SUM([H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM([H02]) AS  H02 ,SUM([H03]) AS  H03												\n"
					+ "			,SUM([H04]) AS  H04 ,SUM([H05]) AS  H05 ,SUM([H06]) AS  H06 ,SUM([H07]) AS  H07												\n"
					+ "			,SUM([H08]) AS  H08 ,SUM([H09]) AS  H09 ,SUM([H10]) AS  H10 ,SUM([H11]) AS  H11												\n"
					+ "			,SUM([H12]) AS  H12 ,SUM([H13]) AS  H13 ,SUM([H14]) AS  H14 ,SUM([H15]) AS  H15												\n"
					+ "			,SUM([H16]) AS  H16 ,SUM([H17]) AS  H17 ,SUM([H18]) AS  H18 ,SUM([H19]) AS  H19												\n"
					+ "			,SUM([H20]) AS  H20 ,SUM([H21]) AS  H21 ,SUM([H22]) AS  H22 ,SUM([H23]) AS  H23												\n"
					+ "			FROM dbo.SummaryAdDaily3 a with(NoLock)								\n"					
					+ "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
					+ "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "			and   a.ContractSeq = 0																	\n" 
					+ "			and   a.progKey > 0																		\n"
					+ "			group by a.Category,a.Genre,a.ProgKey ) v1										\n"										
					+ "group by v1.Category, v1.Genre,v1.ProgKey																	\n"					
					+ "order by v1.Category, v1.Genre,v1.ProgKey																	\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				data.ReportDataSet = ds.Copy();

				// ���
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// ����ڵ� ��Ʈ
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelTime() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "ä�κ� �������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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