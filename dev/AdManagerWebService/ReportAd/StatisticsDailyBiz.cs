// ===============================================================================
//
// StatisticsDailyBiz.cs
//
// ������Ʈ �Ϻ���� ���� 
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
 * Class Name: StatisticsDailyBiz
 * �ֿ���  : ������Ʈ ó�� ����
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
    /// StatisticsDailyBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class StatisticsDailyBiz : BaseBiz
    {

	    #region  ������
        public StatisticsDailyBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region �Ⱓ�� �Ϻ����
        /// <summary>
        ///  �Ⱓ�� �Ϻ����
        /// </summary>
        /// <param name="statisticsDailyModel"></param>
		public void GetStatisticsDaily(HeaderModel header, StatisticsDailyModel statisticsDailyModel)
        {
			bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
			bool isNotReady  = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
         
			try
			{
				StringBuilder sbQuery = null;
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsDaily() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(statisticsDailyModel.SearchStartDay.Length > 6) statisticsDailyModel.SearchStartDay = statisticsDailyModel.SearchStartDay.Substring(2,6);
				if(statisticsDailyModel.SearchEndDay.Length   > 6) statisticsDailyModel.SearchEndDay   = statisticsDailyModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	:[" + statisticsDailyModel.SearchMediaCode   + "]");		// �˻� ��ü
				_log.Debug("SearchContractSeq :[" + statisticsDailyModel.SearchContractSeq + "]");		// �˻� �����ȣ           
				_log.Debug("SearchItemNo      :[" + statisticsDailyModel.SearchItemNo      + "]");		// �˻� �����ȣ           
				_log.Debug("SearchStartDay    :[" + statisticsDailyModel.SearchStartDay    + "]");		// �˻� ������� ����          
				_log.Debug("SearchEndDay      :[" + statisticsDailyModel.SearchEndDay      + "]");		// �˻� �������� ����          
				// __DEBUG__

				string  MediaCode   = statisticsDailyModel.SearchMediaCode;
				int	    ContractSeq = Convert.ToInt32( statisticsDailyModel.SearchContractSeq );
				int	    CampaignCd	= Convert.ToInt32( statisticsDailyModel.CampaignCode );
				int	    ItemNo      = Convert.ToInt32( statisticsDailyModel.SearchItemNo );
				string  StartDay    = statisticsDailyModel.SearchStartDay;
				string  EndDay      = statisticsDailyModel.SearchEndDay;

				// ��������
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
				statisticsDailyModel.ContractAmt = ContractAmt;
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
				statisticsDailyModel.TotalAdCnt = TotalAdCnt;
				#endregion

                // ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* �Ⱓ�� �Ϻ� ���� ���    */                 \n"
                    + "DECLARE @TotAdHit int;    -- ��ü ��������  \n"
                    + "SET @TotAdHit    = 0;                      \n"
                    + "                                           \n"
                    + "-- ��ü ����Hit                              \n"
                    + "SELECT @TotAdHit = ISNULL(SUM(A.AdCnt),0)  \n"
                    + "  FROM SummaryAdDaily0 A with(NoLock)      \n"
					+ " WHERE A.LogDay BETWEEN '"+ StartDay  + "' \n"
					+ "                    AND '"+ EndDay    + "' \n");
				#region ��ȸ����(���,ķ����,����)
				if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				#endregion
				sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                  \n"
                    + "                                           \n"
                    + "-- �Ϻ� ���                                 \n"
                    + "SELECT CONVERT(CHAR(10), CONVERT(datetime, '20' + T.LogDay, 112),120) \n"
                    + "       + CASE T.week WHEN 1 THEN ' (��)'            \n"
                    + "                     WHEN 2 THEN ' (��)'            \n"
                    + "                     WHEN 3 THEN ' (ȭ)'            \n"
                    + "                     WHEN 4 THEN ' (��)'            \n"
                    + "                     WHEN 5 THEN ' (��)'            \n"
                    + "                     WHEN 6 THEN ' (��)'            \n"
                    + "                     WHEN 7 THEN ' (��)'            \n"
                    + "                     ELSE '' END AS LogDay          \n"
                    + "      , T.week AS Week                              \n"
                    + "      ,ISNULL(cnt.AdCnt,0) AS AdCnt                 \n"
                    + "      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(cnt.AdCnt,0) / CONVERT(float,@TotAdHit)) * 100) \n"
					+ "                               ELSE 0 END AS AdRate  \n"
					+ "      ,REPLICATE('��', CASE WHEN @TotAdHit  > 0 THEN ROUND((ISNULL(cnt.AdCnt,0)/CONVERT(float,@TotAdHit) * 100),0) \n"
                    + "                                                ELSE 0 END) AS RateBar   \n"
                    + " FROM                                                \n"
                    + "    (                                                \n"
                    + "      SELECT bs.LogDay                               \n"
					+ "            ,bs.week                                 \n"
                    + "        FROM SummaryBase bs  with(NoLock)            \n"
                    + "        WHERE bs.LogDay BETWEEN '"+ StartDay    + "' \n"
                    + "                            AND '"+ EndDay      + "' \n"
                    + "    ) T                                              \n"
                    + "    LEFT  JOIN                                       \n"
                    + "    ( SELECT A.LogDay, ISNULL(SUM(A.AdCnt),0) AS AdCnt \n"
                    + "        FROM SummaryAdDaily0 A  with(NoLock)           \n"
					+ "       WHERE A.LogDay BETWEEN '"+ StartDay        + "' \n"
					+ "                          AND '"+ EndDay          + "' \n");
				#region ��ȸ����(���,ķ����,����)
				if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				#endregion
				sbQuery.Append(""
                    + "         AND SummaryType = 1                           \n"
                    + "       GROUP BY A.LogDay                               \n"
                    + "    ) cnt                                              \n"
					+ "    ON (T.LogDay = cnt.LogDay)                         \n"
                    + " ORDER BY T.LogDay                                     \n"
					); 


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				statisticsDailyModel.ReportDataSet = ds.Copy();

				// ���
				statisticsDailyModel.ResultCnt = Utility.GetDatasetCount(statisticsDailyModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				statisticsDailyModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsDailyModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsDaily() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsDailyModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsDailyModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
				}
				else if(isNotReady)
				{
					statisticsDailyModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					statisticsDailyModel.ResultDesc = "�Ϻ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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