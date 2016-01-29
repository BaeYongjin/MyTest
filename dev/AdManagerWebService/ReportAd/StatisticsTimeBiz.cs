// ===============================================================================
//
// StatisticsTimeBiz.cs
//
// ������Ʈ �ð��뺰��� ���� 
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
 * Class Name: StatisticsTimeBiz
 * �ֿ���  : ������Ʈ �ð��뺰��� ó�� ����
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
    /// StatisticsTimeBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class StatisticsTimeBiz : BaseBiz
    {
		#region  ������
        public StatisticsTimeBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion
		#region �Ⱓ�� �ð��뺰���
        /// <summary>
        ///  �Ⱓ�� �ð��뺰���
        /// </summary>
        /// <param name="statisticsTimeModel"></param>
        public void GetStatisticsTime(HeaderModel header, StatisticsTimeModel statisticsTimeModel)
        {
			bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
			bool isNotReady  = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsTime() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(statisticsTimeModel.SearchStartDay.Length > 6) statisticsTimeModel.SearchStartDay = statisticsTimeModel.SearchStartDay.Substring(2,6);
				if(statisticsTimeModel.SearchEndDay.Length   > 6) statisticsTimeModel.SearchEndDay   = statisticsTimeModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	  :[" + statisticsTimeModel.SearchMediaCode   + "]");		// �˻� ��ü
				_log.Debug("SearchContractSeq      :[" + statisticsTimeModel.SearchContractSeq      + "]");		// �˻� �����ȣ           
				_log.Debug("SearchItemNo      :[" + statisticsTimeModel.SearchItemNo      + "]");		// �˻� �����ȣ           
				_log.Debug("SearchStartDay    :[" + statisticsTimeModel.SearchStartDay    + "]");		// �˻� ������� ����          
				_log.Debug("SearchEndDay      :[" + statisticsTimeModel.SearchEndDay      + "]");		// �˻� �������� ����          
				// __DEBUG__

				string MediaCode   = statisticsTimeModel.SearchMediaCode;
                int	    ContractSeq = Convert.ToInt32( statisticsTimeModel.SearchContractSeq );
                int	    CampaignCd	= Convert.ToInt32( statisticsTimeModel.CampaignCode );
                int	    ItemNo      = Convert.ToInt32( statisticsTimeModel.SearchItemNo );
				string StartDay    = statisticsTimeModel.SearchStartDay;
				string EndDay      = statisticsTimeModel.SearchEndDay;


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
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem with(noLock) where CampaignCode = " + CampaignCd + " )" + "\n");
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
                statisticsTimeModel.ContractAmt = ContractAmt;
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
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem with(noLock) where CampaignCode = " + CampaignCd + " )" + "\n");

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
                statisticsTimeModel.TotalAdCnt = TotalAdCnt;
                #endregion


                // ��������
				sbQuery = new StringBuilder();
	
				sbQuery.Append("\n"
					+ "-- �Ⱓ�� �ð��뺰 ���� ���                \n"
                    + "                                           \n"
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
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem with(noLock) where CampaignCode = " + CampaignCd + " )" + "\n");
				sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                  \n"
					+ "                                           \n"
					+ "--�ð��� ������ ���� �ӽ����̺�  \n"
					+ "-- 0���� 1�� ���� 24����  \n"
					+ "SELECT IDENTITY(INT, 0, 1) AS Rownum  \n"
					+ "INTO #TempTime  \n"
					+ "FROM (SELECT TOP 24 * FROM SummaryCode) T  \n"
					+ "                                           \n"
					+ "-- �ð��뺰 ���                           \n"
					+ "SELECT TA.TimeOrder                        \n"
					+ "      ,TA.TimeName                         \n"
					+ "      ,ISNULL(SUM(TA.AdCnt),0) AS AdCnt                                                                                 \n"
					+ "      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TA.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)    \n"
					+ "                               ELSE 0 END AS PgRate                                                                     \n"
					+ "      ,REPLICATE('��', CASE WHEN @TotAdHit  > 0 THEN ROUND((ISNULL(SUM(TA.AdCnt),0)/CONVERT(float,@TotAdHit) * 100),0)  \n"
					+ "                                                ELSE 0 END) AS RateBar                                                  \n"
					+ "  FROM (                                                 \n"
					+ "        SELECT TC.RowNum AS TimeOrder                    \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN '00��~01��'   \n"
					+ "                              WHEN  1 THEN '01��~02��'   \n"
					+ "                              WHEN  2 THEN '02��~03��'   \n"
					+ "                              WHEN  3 THEN '03��~04��'   \n"
					+ "                              WHEN  4 THEN '04��~05��'   \n"
					+ "                              WHEN  5 THEN '05��~06��'   \n"
					+ "                              WHEN  6 THEN '06��~07��'   \n"
					+ "                              WHEN  7 THEN '07��~08��'   \n"
					+ "                              WHEN  8 THEN '08��~09��'   \n"
					+ "                              WHEN  9 THEN '09��~10��'   \n"
					+ "                              WHEN 10 THEN '10��~11��'   \n"
					+ "                              WHEN 11 THEN '11��~12��'   \n"
					+ "                              WHEN 12 THEN '12��~13��'   \n"
					+ "                              WHEN 13 THEN '13��~14��'   \n"
					+ "                              WHEN 14 THEN '14��~15��'   \n"
					+ "                              WHEN 15 THEN '15��~16��'   \n"
					+ "                              WHEN 16 THEN '16��~17��'   \n"
					+ "                              WHEN 17 THEN '17��~18��'   \n"
					+ "                              WHEN 18 THEN '18��~19��'   \n"
					+ "                              WHEN 19 THEN '19��~20��'   \n"
					+ "                              WHEN 20 THEN '20��~21��'   \n"
					+ "                              WHEN 21 THEN '21��~22��'   \n"
					+ "                              WHEN 22 THEN '22��~23��'   \n"
					+ "                              WHEN 23 THEN '23��~24��' END AS TimeName \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN SUM(H00)      \n"
					+ "                              WHEN  1 THEN SUM(H01)      \n"
					+ "                              WHEN  2 THEN SUM(H02)      \n"
					+ "                              WHEN  3 THEN SUM(H03)      \n"
					+ "                              WHEN  4 THEN SUM(H04)      \n"
					+ "                              WHEN  5 THEN SUM(H05)      \n"
					+ "                              WHEN  6 THEN SUM(H06)      \n"
					+ "                              WHEN  7 THEN SUM(H07)      \n"
					+ "                              WHEN  8 THEN SUM(H08)      \n"
					+ "                              WHEN  9 THEN SUM(H09)      \n"
					+ "                              WHEN 10 THEN SUM(H10)      \n"
					+ "                              WHEN 11 THEN SUM(H11)      \n"
					+ "                              WHEN 12 THEN SUM(H12)      \n"
					+ "                              WHEN 13 THEN SUM(H13)      \n"
					+ "                              WHEN 14 THEN SUM(H14)      \n"
					+ "                              WHEN 15 THEN SUM(H15)      \n"
					+ "                              WHEN 16 THEN SUM(H16)      \n"
					+ "                              WHEN 17 THEN SUM(H17)      \n"
					+ "                              WHEN 18 THEN SUM(H18)      \n"
					+ "                              WHEN 19 THEN SUM(H19)      \n"
					+ "                              WHEN 20 THEN SUM(H20)      \n"
					+ "                              WHEN 21 THEN SUM(H21)      \n"
					+ "                              WHEN 22 THEN SUM(H22)      \n"
					+ "                              WHEN 23 THEN SUM(H23) END AdCnt \n"
					+ "          FROM SummaryAdDaily0 A with(NoLock)            \n"
					+ "              ,#TempTime TC                              \n"
					+ "         WHERE A.LogDay BETWEEN '"+ StartDay    + "'     \n"
                    + "                            AND '"+ EndDay      + "'     \n");
                if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				sbQuery.Append(""
                    + "         AND SummaryType = 1                             \n"
					+ "         GROUP BY TC.RowNum                              \n"
					+ "       ) TA                                              \n"
					+ " GROUP BY TimeOrder, TimeName                            \n"
					+ "                                                         \n"
					+ " ORDER BY TimeOrder                                      \n"
					+ "                                                         \n"
					+ " DROP Table #TempTime                                    \n"
					);
					
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				statisticsTimeModel.ReportDataSet = ds.Copy();

				// ���
				statisticsTimeModel.ResultCnt = Utility.GetDatasetCount(statisticsTimeModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				statisticsTimeModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsTimeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsTime() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsTimeModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsTimeModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
				}
				else if(isNotReady)
				{
					statisticsTimeModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					statisticsTimeModel.ResultDesc = "�ð��뺰��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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