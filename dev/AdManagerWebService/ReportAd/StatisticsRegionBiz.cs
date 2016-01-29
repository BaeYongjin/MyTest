// ===============================================================================
//
// StatisticsRegionBiz.cs
//
// ������Ʈ ��������� ���� 
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
 * Class Name: StatisticsRegionBiz
 * �ֿ���  : ������Ʈ ��������� ó�� ����
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
    /// StatisticsRegionBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class StatisticsRegionBiz : BaseBiz
    {

		#region  ������
        public StatisticsRegionBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region �Ⱓ�� ���������
        /// <summary>
        ///  �Ⱓ�� ���������=-���� method
        /// </summary>
        /// <param name="statisticsRegionModel"></param>
        public void GetStatisticsRegion_Org(HeaderModel header, StatisticsRegionModel statisticsRegionModel)
        {
			bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
			bool isNotReady  = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsRegion() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(statisticsRegionModel.SearchStartDay.Length > 6) statisticsRegionModel.SearchStartDay = statisticsRegionModel.SearchStartDay.Substring(2,6);
				if(statisticsRegionModel.SearchEndDay.Length   > 6) statisticsRegionModel.SearchEndDay   = statisticsRegionModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	  :[" + statisticsRegionModel.SearchMediaCode   + "]");		// �˻� ��ü
				_log.Debug("SearchContractSeq :[" + statisticsRegionModel.SearchContractSeq      + "]");		// �˻� �����ȣ           
				_log.Debug("SearchItemNo      :[" + statisticsRegionModel.SearchItemNo      + "]");		// �˻� �����ȣ           
				_log.Debug("SearchStartDay    :[" + statisticsRegionModel.SearchStartDay    + "]");		// �˻� ������� ����          
				_log.Debug("SearchEndDay      :[" + statisticsRegionModel.SearchEndDay      + "]");		// �˻� �������� ����          
				// __DEBUG__

				string  MediaCode   = statisticsRegionModel.SearchMediaCode;
                int	    ContractSeq = Convert.ToInt32( statisticsRegionModel.SearchContractSeq );
                int	    CampaignCd	= Convert.ToInt32( statisticsRegionModel.CampaignCode );
                int	    ItemNo      = Convert.ToInt32( statisticsRegionModel.SearchItemNo );
				string  StartDay    = statisticsRegionModel.SearchStartDay;
				string  EndDay      = statisticsRegionModel.SearchEndDay;

                #region [ �������Ǽ� ���ϱ� ]
                sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" -- �ش籤���� ����Ⱓ��ȸ" + "\n");
                sbQuery.Append(" -- �ش籤���� ������(�������)��ȸ" + "\n");
                sbQuery.Append(" SELECT sum(ISNULL(b.ContractAmt,0)) AS ContractAmt" + "\n");
                sbQuery.Append(" FROM AdTargetsHanaTV.dbo.ContractItem a with(nolock)" + "\n");
                sbQuery.Append(" LEFT JOIN AdTargetsHanaTV.dbo.Targeting b with(nolock)" + "\n");
                sbQuery.Append("        ON (a.ItemNo = b.ItemNo)" + "\n");
                sbQuery.Append(" WHERE 1=1" + "\n");
                #region ��ȸ����(���,ķ����,����)
                if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
                #endregion

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                int    ContractAmt    = 0;

                if(ds.Tables[0].Rows.Count == 0)
                {
                    isNotTarget = true;
                    throw new Exception();
                }
                ContractAmt    = Convert.ToInt32(ds.Tables[0].Rows[0]["ContractAmt"].ToString());

                ds.Dispose();

                // �ش籤���� ������(�������)��
                statisticsRegionModel.ContractAmt = ContractAmt;
                #endregion

                #region [ ��������Ǽ� ���ϱ� ]
                sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" -- �ش籤���� ���Ⱓ�� �� ���������" + "\n" );
                sbQuery.Append(" SELECT ISNULL(SUM(A.AdCnt),0) AS TotalAdCnt" + "\n" );
                sbQuery.Append(" FROM   SummaryAdDaily0 A with(NoLock)" + "\n" );
                sbQuery.Append(" WHERE  A.LogDay BETWEEN '"+ StartDay + "' AND '"+ EndDay + "'" + "\n" );
                sbQuery.Append(" AND		A.SummaryType = 1" + "\n" );
                if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                if(ds.Tables[0].Rows.Count == 0)
                {
                    isNotReady = true;
                    throw new Exception();
                }

                int TotalAdCnt = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalAdCnt"].ToString());
                ds.Dispose();

                // �ش籤���� ������(�������)��
                statisticsRegionModel.TotalAdCnt = TotalAdCnt;
                #endregion

                // ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* �Ⱓ�� ���� ���������    */                 \n"
                    + "DECLARE @TotAdHit int;    -- ��ü ��������  \n"
                    + "SET @TotAdHit    = 0;                      \n"
                    + "                                           \n"
                    + "-- ��ü ����Hit                              \n"
                    + "SELECT @TotAdHit = ISNULL(SUM(A.AdCnt),0)  \n"
                    + "  FROM SummaryAdDaily0 A with(NoLock)      \n"
					+ " WHERE A.LogDay BETWEEN '"+ StartDay  + "' \n"
					+ "                    AND '"+ EndDay    + "' \n");
                if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
                    // �����ȸ���� ����������ȸ����
				sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                  \n"
                    + "                                           \n"
                    + "-- ������ ������ ���� �ӽ����̺�                \n"
                    + "SELECT IDENTITY(INT, 1, 1) AS Rownum, T.*  \n"
                    + "INTO #TempRegion                           \n"
                    + "FROM (SELECT A.SummaryCode                 \n"
                    + "              ,A.SummaryName               \n"
                    + "          FROM SummaryCode A  with(NoLock) \n"
                    + "         WHERE A.SummaryType = 5           \n"
                    + "           AND A.Level = 1 ) T             \n"
					+ "                                           \n"
					+ "-- ������ ����                               \n"
                    + "SELECT '1' AS ORD                          \n"
					+ "      ,(SPACE(2 - LEN(CONVERT(VARCHAR(2),TT.Rownum))) \n"
					+ "               + CONVERT(VARCHAR(2),TT.Rownum) + ' ' + TA.SummaryName ) AS GrpName \n"
					+ "      ,TA.SummaryCode AS SumCode           \n"
                    + "      ,TA.SummaryName AS SumName           \n"
                    + "      ,(SELECT COUNT(*) FROM SummaryCode WHERE SummaryType = 5 AND Level = 2 AND UpperCode = TA.SummaryCode) AS SubCount \n"
                    + "      ,0 AS subSumCode                     \n"
                    + "      ,'�հ�' AS subSumName                 \n"
                    + "      ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt    \n"
                    + "      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)     \n"
                    + "                               ELSE 0 END AS AdRate                                                                      \n"
                    + "      ,REPLICATE('��', CASE WHEN @TotAdHit  > 0 THEN ROUND((ISNULL(SUM(TB.AdCnt),0)/CONVERT(float,@TotAdHit) * 100),0)   \n"
                    + "                                                ELSE 0 END) AS RateBar                                                   \n"
                    + "  FROM (SELECT A.SummaryCode               \n"
                    + "              ,A.SummaryName               \n"
                    + "          FROM SummaryCode A  with(NoLock) \n"
                    + "         WHERE A.SummaryType = 5           \n"
                    + "           AND A.Level = 1                 \n"
                    + "       ) TA                                \n"
                    + "       LEFT JOIN                           \n"
                    + "       (                                   \n"
                    + "        SELECT CASE B.Level WHEN 1 THEN B.SummaryCode                      \n"
                    + "                                   ELSE B.UpperCode END AS SummaryCode     \n"
                    + "              ,A.AdCnt                                                     \n"
                    + "              ,B.Level                                                     \n"
                    + "          FROM SummaryAdDaily0        A with(NoLock)      \n"
					+ "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode) \n"
					+ "         WHERE A.LogDay BETWEEN '"+ StartDay  + "' \n"
					+ "                            AND '"+ EndDay    + "' \n");
                if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				sbQuery.Append(""
                    + "           AND A.SummaryType  = 5  -- 5:������      \n"
                    + "       ) TB                                        \n"
					+ "       ON (TA.SummaryCode = TB.SummaryCode)        \n"
					+ "       LEFT JOIN #TempRegion TT ON (TA.SummaryCode = TT.SummaryCode)   \n"
					+ " GROUP BY TT.Rownum, TA.SummaryCode, TA.SummaryName           \n"
                    + "                                                   \n"
                    + "UNION ALL                                          \n"
                    + "                                                   \n"
                    + "-- �������� ����                                    \n"
                    + "SELECT '2' AS ORD                                  \n"
					+ "      ,(SPACE(2 - LEN(CONVERT(VARCHAR(2),TT.Rownum)))  \n"
					+ "               + CONVERT(VARCHAR(2),TT.Rownum) + ' ' + TC.SummaryName ) AS GrpName \n"
                    + "      ,TA.UpperCode   AS SumCode                   \n"
                    + "      ,TC.SummaryName AS SumName                   \n"
                    + "      ,0 AS SubCount                               \n"
                    + "      ,TA.SummaryCode AS subSumCode                \n"
                    + "      ,TA.SummaryName AS subSumName                \n"
                    + "      ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt            \n" 
                    + "      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)       \n"
                     + "                              ELSE 0 END AS AdRate                                                                        \n"
                    + "      ,REPLICATE('��', CASE WHEN @TotAdHit  > 0 THEN ROUND((ISNULL(SUM(TB.AdCnt),0)/CONVERT(float,@TotAdHit) * 100),0)     \n"
                    + "                                                ELSE 0 END) AS RateBar                                                     \n"
                    + "  FROM (SELECT SummaryCode, SummaryName, UpperCode FROM SummaryCode with(NoLock)  WHERE SummaryType = 5 AND Level = 2) TA                           \n"
                    + "       LEFT JOIN                                   \n"
                    + "       (                                           \n"
                    + "        SELECT A.SummaryCode                       \n"
                    + "              ,B.UpperCode                         \n"
                    + "              ,A.AdCnt                             \n"
                    + "              ,B.Level                             \n"
                    + "          FROM SummaryAdDaily0        A with(NoLock)      \n"
					+ "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)      \n"
	                + "         WHERE A.LogDay BETWEEN '"+ StartDay  + "' \n"
					+ "                            AND '"+ EndDay    + "' \n");
                if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				sbQuery.Append(""
                    + "           AND A.SummaryType  = 5  -- 5:������      \n"
                    + "       ) TB                                        \n"
                    + "       ON (TA.SummaryCode = TB.SummaryCode)                                                                         \n"
					+ "       LEFT JOIN SummaryCode TC with(NoLock) ON (TA.UpperCode = TC.SummaryCode AND TC.SummaryType = 5 AND TC.Level = 1)          \n"
					+ "       LEFT JOIN #TempRegion TT ON (TA.UpperCode = TT.SummaryCode)                                                  \n"
					+ " GROUP BY TT.Rownum, TA.UpperCode, TC.SummaryName, TA.SummaryCode, TA.SummaryName                                              \n"
                    + "                                                   \n"
                    + " ORDER BY SumCode, AdCnt DESC                      \n"
					+ "                                                   \n"
					+ " DROP Table #TempRegion                            \n"
					);  

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				statisticsRegionModel.ReportDataSet = ds.Copy();

				// ���
				statisticsRegionModel.ResultCnt = Utility.GetDatasetCount(statisticsRegionModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				statisticsRegionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsRegionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsRegion() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsRegionModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsRegionModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
				}
				else if(isNotReady)
				{
					statisticsRegionModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					statisticsRegionModel.ResultDesc = "��������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
					_log.Exception(ex);
				}
            }
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

        }



		/// <summary>
		/// �Ⱓ�� ���������=Ȯ�庻
		/// </summary>
		/// <param name="header"></param>
		/// <param name="statisticsRegionModel"></param>
		public void GetStatisticsRegion(HeaderModel header, StatisticsRegionModel statisticsRegionModel)
		{
			bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
			bool isNotReady  = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsRegion() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(statisticsRegionModel.SearchStartDay.Length > 6) statisticsRegionModel.SearchStartDay = statisticsRegionModel.SearchStartDay.Substring(2,6);
				if(statisticsRegionModel.SearchEndDay.Length   > 6) statisticsRegionModel.SearchEndDay   = statisticsRegionModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	  :[" + statisticsRegionModel.SearchMediaCode   + "]");		// �˻� ��ü
				_log.Debug("SearchContractSeq :[" + statisticsRegionModel.SearchContractSeq + "]");		// �˻� �����ȣ           
				_log.Debug("SearchItemNo      :[" + statisticsRegionModel.SearchItemNo      + "]");		// �˻� �����ȣ           
				_log.Debug("SearchStartDay    :[" + statisticsRegionModel.SearchStartDay    + "]");		// �˻� ������� ����          
				_log.Debug("SearchEndDay      :[" + statisticsRegionModel.SearchEndDay      + "]");		// �˻� �������� ����          
				// __DEBUG__

				string  MediaCode   = statisticsRegionModel.SearchMediaCode;
				int	    ContractSeq = Convert.ToInt32( statisticsRegionModel.SearchContractSeq );
				int	    CampaignCd	= Convert.ToInt32( statisticsRegionModel.CampaignCode );
				int	    ItemNo      = Convert.ToInt32( statisticsRegionModel.SearchItemNo );
				string  StartDay    = statisticsRegionModel.SearchStartDay;
				string  EndDay      = statisticsRegionModel.SearchEndDay;

				#region [ �������Ǽ� ���ϱ� ]
				sbQuery = new StringBuilder();
				sbQuery.Append("\n");
				sbQuery.Append(" -- �ش籤���� ����Ⱓ��ȸ" + "\n");
				sbQuery.Append(" -- �ش籤���� ������(�������)��ȸ" + "\n");
				sbQuery.Append(" SELECT sum(ISNULL(b.ContractAmt,0)) AS ContractAmt" + "\n");
                sbQuery.Append(" FROM AdTargetsHanaTV.dbo.ContractItem a with(nolock)" + "\n");
                sbQuery.Append(" LEFT JOIN AdTargetsHanaTV.dbo.Targeting b with(nolock)" + "\n");
				sbQuery.Append("        ON (a.ItemNo = b.ItemNo)"    + "\n");
				sbQuery.Append(" WHERE 1=1" + "\n");
				#region ��ȸ����(���,ķ����,����)
				if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				#endregion

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				int    ContractAmt    = 0;

				if(ds.Tables[0].Rows.Count == 0)
				{
					isNotTarget = true;
					throw new Exception();
				}
				ContractAmt    = Convert.ToInt32(ds.Tables[0].Rows[0]["ContractAmt"].ToString());

				ds.Dispose();

				// �ش籤���� ������(�������)��
				statisticsRegionModel.ContractAmt = ContractAmt;
				#endregion

				#region [ ��������Ǽ� ���ϱ� ]
				sbQuery = new StringBuilder();
				sbQuery.Append("\n");
				sbQuery.Append(" -- �ش籤���� ���Ⱓ�� �� ���������" + "\n" );
				sbQuery.Append(" SELECT ISNULL(SUM(A.AdCnt),0) AS TotalAdCnt" + "\n" );
				sbQuery.Append(" FROM   SummaryAdDaily0 A with(NoLock)" + "\n" );
				sbQuery.Append(" WHERE  A.LogDay BETWEEN '"+ StartDay + "' AND '"+ EndDay + "'" + "\n" );
				sbQuery.Append(" AND		A.SummaryType = 1" + "\n" );
				if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count == 0)
				{
					isNotReady = true;
					throw new Exception();
				}

				int TotalAdCnt = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalAdCnt"].ToString());
				ds.Dispose();

				// �ش籤���� ������(�������)��
				statisticsRegionModel.TotalAdCnt = TotalAdCnt;
				#endregion

				// ��������
				sbQuery = new StringBuilder();
				
				sbQuery.Append("/* �Ⱓ�� ���� ���������    */              \n");
				sbQuery.Append("DECLARE @TotAdHit int;    -- ��ü �������� \n");
				sbQuery.Append("SET @TotAdHit    = 0;                        \n");
				sbQuery.Append("                                             \n");

				#region [ ��ü����Hit ]
				sbQuery.Append("-- ��ü ����Hit                              \n");
				sbQuery.Append(" SELECT @TotAdHit = ISNULL(SUM(A.AdCnt),0)    \n");
				sbQuery.Append(" From   SummaryAdDaily0 a with(NoLock)			\n");
				sbQuery.Append(" Where  a.LogDay BETWEEN '"+ StartDay  + "' And '"+ EndDay + "'   \n");
				sbQuery.Append(" And    a.SummaryType = 1                     \n");

				if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				#endregion
									
				#region [������ ������ ���� �ӽ����̺� ]
				sbQuery.Append("-- ������ ������ ���� �ӽ����̺�                    \n");
				sbQuery.Append("	Select IDENTITY(INT, 1, 1) AS Rownum, T.*       \n");
				sbQuery.Append("	Into #TempRegion                                \n");
				sbQuery.Append("	From (	select	 A.SummaryCode                  \n");
				sbQuery.Append("					,A.SummaryName                  \n");
				sbQuery.Append("			From   SummaryCode A  with(NoLock)      \n");
				sbQuery.Append("			Where  A.SummaryType = 5                \n"); 
				sbQuery.Append("			And    A.Level       = 1 ) T;           \n");
				#endregion

				#region [1�ܰ�- �����õ� ��]
				sbQuery.Append("-- ������ ����                             \n");
				sbQuery.Append("SELECT '1' AS ORD                          \n");
                sbQuery.Append("	  ,AdTargetsHanaTV.dbo.ufnPadding('L',tt.RowNum, 2,'0') + ' ' + TA.SummaryName AS GrpName \n");
				sbQuery.Append("      ,TA.SummaryCode AS SumCode           \n");
				sbQuery.Append("      ,TA.SummaryName AS SumName           \n");
                sbQuery.Append("      ,(SELECT COUNT(*) FROM AdTargetsHanaTV.dbo.TargetRegion WHERE Level = 2 AND UpperCode = TA.SummaryCode) AS SubCount \n");
				sbQuery.Append("      ,0 AS subSumCode                     \n");
				sbQuery.Append("      ,'[�հ�]' AS subSumName                \n");
				sbQuery.Append("      ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt    \n");
				sbQuery.Append("      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)    \n");
				sbQuery.Append("                               ELSE 0 END AS AdRate                                                                     \n");
				sbQuery.Append("      ,''   AS RateBar						\n");
				sbQuery.Append("	  ,TA.Orders      As Orders				\n");
				sbQuery.Append("      ,TA.ParentCode  As ParentCode			\n");
				sbQuery.Append("      ,TA.SummaryDesc As SummaryDesc	\n");
				sbQuery.Append("  FROM (SELECT a.regionCode	as SummaryCode  \n");
				sbQuery.Append("              ,a.regionName as SummaryName  \n");
				sbQuery.Append("			  ,a.UpperCode					\n");
				sbQuery.Append("              ,a.Orders						\n");
				sbQuery.Append("              ,a.ParentCode					\n");
                sbQuery.Append("              ,a.regionName as SummaryDesc	\n");
                sbQuery.Append("        from	AdTargetsHanaTV.dbo.TargetRegion A  with(NoLock)\n");
				sbQuery.Append("        where	a.Level = 1 ) TA				\n");
				sbQuery.Append("  LEFT JOIN                           \n");
				sbQuery.Append("	  (	SELECT CASE B.Level WHEN 1 THEN B.regionCode                      \n");
				sbQuery.Append("                                   ELSE B.UpperCode END AS SummaryCode     \n");
				sbQuery.Append("              ,A.AdCnt                                                     \n");
				sbQuery.Append("              ,B.Level                                                     \n");
				sbQuery.Append("        FROM SummaryAdDaily0    A with(NoLock)									\n");
                sbQuery.Append("        INNER JOIN AdTargetsHanaTV.dbo.TargetRegion B with(NoLock) ON A.SummaryCode = B.regionCode	\n");
				sbQuery.Append("        WHERE A.LogDay BETWEEN '"+ StartDay  + "' AND '"+ EndDay + "' \n");
				sbQuery.Append("		And   a.SummaryType = 5		\n");

				if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");

				sbQuery.Append("        ) TB    		                  \n");

				sbQuery.Append("       ON (TA.SummaryCode = TB.SummaryCode)        \n");
				sbQuery.Append("       LEFT JOIN #TempRegion TT ON (TA.SummaryCode = TT.SummaryCode)   \n");
				sbQuery.Append(" GROUP BY TT.Rownum, TA.SummaryCode, TA.SummaryName, TA.Orders ,TA.ParentCode, TA.SummaryDesc           \n");
				sbQuery.Append("                                                   \n");
				#endregion
				#region[2�ܰ�-�ñ���]
				sbQuery.Append("UNION ALL                                          \n");
				sbQuery.Append("                                                   \n");
				sbQuery.Append("-- �������� ����                                    \n");
				sbQuery.Append("SELECT '2' AS ORD                                  \n");
                sbQuery.Append("	  ,AdTargetsHanaTV.dbo.ufnPadding('L',tt.RowNum, 2,'0') + ' ' + TC.RegionName AS GrpName \n");
				sbQuery.Append("      ,TA.UpperCode   AS SumCode                   \n");
				sbQuery.Append("      ,TC.RegionName AS SumName                   \n");
                sbQuery.Append("      ,(SELECT COUNT(*) FROM AdTargetsHanaTV.dbo.TargetRegion WHERE Level = 3 AND ParentCode = TA.SummaryCode) AS SubCount \n");
				sbQuery.Append("      ,TA.SummaryCode AS subSumCode                \n");
				sbQuery.Append("      ,'   ' + TA.SummaryName AS subSumName                \n");
				sbQuery.Append("      ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt            \n"); 
				sbQuery.Append("      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)       \n");
				sbQuery.Append("                              ELSE 0 END AS AdRate                                                                         \n");
				sbQuery.Append("      ,'' AS RateBar                                                     \n");
				sbQuery.Append("	  ,TA.Orders      As Orders                    \n");
				sbQuery.Append("      ,TA.ParentCode  As ParentCode				   \n");
				sbQuery.Append("      ,TA.SummaryDesc As SummaryDesc			   \n");
				sbQuery.Append("  FROM (SELECT a.regionCode	as SummaryCode  \n");
				sbQuery.Append("              ,a.regionName as SummaryName  \n");
				sbQuery.Append("			  ,a.UpperCode					\n");
				sbQuery.Append("              ,a.Orders						\n");
				sbQuery.Append("              ,a.ParentCode					\n");
				sbQuery.Append("              ,a.regionName as SummaryDesc	\n");
                sbQuery.Append("        from	AdTargetsHanaTV.dbo.TargetRegion A  with(NoLock)\n");
				sbQuery.Append("        where	a.Level = 2 ) TA				\n");
				sbQuery.Append("       LEFT JOIN                                   \n");
				sbQuery.Append("       (                                           \n");
				sbQuery.Append("        SELECT A.SummaryCode                       \n");
				sbQuery.Append("              ,B.UpperCode                         \n");
				sbQuery.Append("              ,A.AdCnt                             \n");
				sbQuery.Append("              ,B.Level                             \n");
				sbQuery.Append("              ,B.ParentCode                        \n");
				sbQuery.Append("        FROM SummaryAdDaily0	A with(NoLock)      \n");
                sbQuery.Append("		INNER JOIN AdTargetsHanaTV.dbo.TargetRegion B with(NoLock) ON A.SummaryCode = B.RegionCode  \n");
				sbQuery.Append("        WHERE A.LogDay BETWEEN '"+ StartDay  + "' AND '"+ EndDay + "' \n");
				sbQuery.Append("		And   a.SummaryType = 5		\n");

				if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				
				sbQuery.Append("       ) TB                                         \n");
				sbQuery.Append("       ON (TA.SummaryCode = TB.ParentCode)          \n");
                sbQuery.Append("       LEFT JOIN AdTargetsHanaTV.dbo.TargetRegion TC with(NoLock) ON (TA.UpperCode = TC.RegionCode AND TC.Level = 1) \n");
				sbQuery.Append("       LEFT JOIN #TempRegion TT ON (TA.UpperCode = TT.SummaryCode)                                                      \n");
				sbQuery.Append(" GROUP BY TT.Rownum, TA.UpperCode, TC.RegionName, TA.SummaryCode, TA.SummaryName                                       \n");
                sbQuery.Append("         ,TA.Orders ,TA.ParentCode, TA.SummaryDesc \n");                        
				sbQuery.Append("                                                   \n");
				#endregion
//
				sbQuery.Append("UNION ALL                                          \n");                                          
                                                   
				sbQuery.Append("-- �������� ����                                 \n");    
				sbQuery.Append("SELECT                                             \n");
				sbQuery.Append("	'3' AS ORD                                     \n");
                sbQuery.Append("	,AdTargetsHanaTV.dbo.ufnPadding('L',tt.RowNum, 2,'0') + ' ' + TC.RegionName AS GrpName \n");
				sbQuery.Append("	,TA.UpperCode   AS SumCode                   \n");
				sbQuery.Append("	,TC.RegionName AS SumName                   \n");
				sbQuery.Append("	,0 AS SubCount                               \n");
				sbQuery.Append("	,TA.SummaryCode AS subSumCode                \n");
				sbQuery.Append("	,'   ' + TD.RegionName + '>>' + TA.SummaryName AS subSumName                \n");
				sbQuery.Append("	,ISNULL(SUM(TB.AdCnt),0) AS AdCnt            \n");
				sbQuery.Append("	,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)      \n");
				sbQuery.Append("							ELSE 0 END AS AdRate                                                                        \n");
				sbQuery.Append("	,'' AS RateBar       \n");
				sbQuery.Append("	,TA.Orders      As Orders		             \n");
				sbQuery.Append("	,TA.ParentCode  As ParentCode                \n");
				sbQuery.Append("	,TD.RegionName As SummaryDesc               \n");
				sbQuery.Append("  FROM (SELECT a.regionCode	as SummaryCode  \n");
				sbQuery.Append("              ,a.regionName as SummaryName  \n");
				sbQuery.Append("			  ,a.UpperCode					\n");
				sbQuery.Append("              ,a.Orders						\n");
				sbQuery.Append("              ,a.ParentCode					\n");
				sbQuery.Append("              ,a.regionName as SummaryDesc	\n");
                sbQuery.Append("        from	AdTargetsHanaTV.dbo.TargetRegion A  with(NoLock)\n");
				sbQuery.Append("        where	a.Level = 3 ) TA				\n");
				sbQuery.Append("	LEFT JOIN									 \n");	
				sbQuery.Append("	(                                            \n");
				sbQuery.Append("        SELECT A.SummaryCode                       \n");
				sbQuery.Append("              ,B.UpperCode                         \n");
				sbQuery.Append("              ,A.AdCnt                             \n");
				sbQuery.Append("              ,B.Level                             \n");
				sbQuery.Append("              ,B.ParentCode                        \n");
				sbQuery.Append("        FROM SummaryAdDaily0	A with(NoLock)      \n");
                sbQuery.Append("		INNER JOIN AdTargetsHanaTV.dbo.TargetRegion B with(NoLock) ON A.SummaryCode = B.RegionCode  \n");
				sbQuery.Append("        WHERE A.LogDay BETWEEN '"+ StartDay  + "' AND '"+ EndDay + "' \n");
				sbQuery.Append("		And   a.SummaryType = 5		\n");

				if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem with(noLock) where CampaignCode = " + CampaignCd + " )" + "\n");

				
				sbQuery.Append("	) TB                                        \n");
				sbQuery.Append("	ON (TA.SummaryCode = TB.SummaryCode)         \n");
                sbQuery.Append("       LEFT JOIN AdTargetsHanaTV.dbo.TargetRegion TC with(NoLock) ON (TA.UpperCode = TC.RegionCode AND TC.Level = 1) \n");
                sbQuery.Append("       LEFT JOIN AdTargetsHanaTV.dbo.TargetRegion TD with(NoLock) ON (TA.ParentCode = TD.RegionCode AND TD.Level = 2) \n");
				sbQuery.Append("       LEFT JOIN #TempRegion TT ON (TA.UpperCode = TT.SummaryCode)                                                      \n");
				sbQuery.Append(" GROUP BY TT.Rownum, TA.UpperCode, TC.RegionName, TA.SummaryCode, TA.SummaryName                                       \n");
				sbQuery.Append("         ,TA.Orders ,TA.ParentCode, TD.RegionName \n");                        
				sbQuery.Append(" ORDER BY Orders, AdCnt DESC                       \n");
				sbQuery.Append("                                                   \n");
				sbQuery.Append(" DROP Table #TempRegion                            \n");
					

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				statisticsRegionModel.ReportDataSet = ds.Copy();

				// ���
				statisticsRegionModel.ResultCnt = Utility.GetDatasetCount(statisticsRegionModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				statisticsRegionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + statisticsRegionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsRegion() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsRegionModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsRegionModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
				}
				else if(isNotReady)
				{
					statisticsRegionModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					statisticsRegionModel.ResultDesc = "��������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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