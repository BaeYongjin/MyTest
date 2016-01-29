// ===============================================================================
//
// StatisticsTotalBiz.cs
//
// ��ü��� ���� 
//
// ===============================================================================
// Release history
// 2007.10.26 RH.Jung OAP�� ��ȸ �������� ����
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: StatisticsTotalBiz
 * �ֿ���  : ��ü��� ó�� ����
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
    /// StatisticsTotalBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class StatisticsTotalBiz : BaseBiz
    {

		#region  ������
        public StatisticsTotalBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region ��ü���
		/// <summary>
      /// ��ü���
      /// </summary>
      /// <param name="statisticsTotalModel"></param>
      public void GetStatisticsTotal(HeaderModel header, StatisticsTotalModel statisticsTotalModel)
      {
			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
            _log.Debug("-----------------------------------------");
            _log.Debug(this.ToString() + "GetStatisticsTotal() Start");
            _log.Debug("-----------------------------------------");
				
				// __DEBUG__
            _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	 :[" + statisticsTotalModel.SearchMediaCode  + "]");	// �˻� ��ü
				_log.Debug("SearchRapCode	 :[" + statisticsTotalModel.SearchRapCode    + "]");	// �˻� �̵�
				_log.Debug("SearchAgencyCode :[" + statisticsTotalModel.SearchAgencyCode + "]");	// �˻� �����
				_log.Debug("SearchKey        :[" + statisticsTotalModel.SearchKey        + "]");	// �˻� Ű����           
            // __DEBUG__

            // ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- ��ü���                                                     \n"
					+ "SELECT ItemNo          -- �����ȣ                              \n"
					+ "      ,ItemName        -- �����(�����)                        \n"
					+ "      ,ExcuteEndDay    -- ����������(����)                    \n"
					+ "      ,ContractAmt     -- ���(����)�������                    \n"
					+ "      ,CASE WHEN TotDayCnt   > 0 THEN FLOOR((TotAdCnt/TotDayCnt) * (TotRealDays- ExDays + 1) + TotAdCnt )  \n"
                    + "            ELSE 0 END  AS AvgExpAmt -- ������ճ��ⷮ(����)     \n"
					+ "      ,TotAdCnt        -- ���� �������                         \n"
					+ "      ,REPLICATE('��', CASE WHEN ContractAmt > 0 THEN ROUND((TotAdCnt/CONVERT(float,ContractAmt)*10),0)      \n"
                    + "                                                 ELSE 0 END) AS ExcRateBar                              \n"
					+ "      ,CASE WHEN ContractAmt > 0 THEN CONVERT(DECIMAL(9,2),(TotAdCnt/CONVERT(float,ContractAmt)) * 100) \n"
					+ "                                 ELSE 0 END AS ExcRate       \n"
					+ "      ,ExcuteStartDay  -- ���������                            \n"
					+ "      ,RealEndDay      -- ����������                            \n"
					+ "      ,TotExcDays      -- ������ �������ϼ�                   \n"
					+ "      ,TotRealDays     -- �������� �������ϼ�                   \n"
					+ "      ,TotDayCnt       -- ���� �����ϼ�                         \n"
					+ "      ,ContractName    -- ����                                \n"
					+ "      ,AgencyName      -- ������ \n"
					+ "      ,AdvertiserName  -- �����ָ� \n"
					+ " FROM (                                                         \n"
					+ "      SELECT itm.ItemNo, itm.ItemName                           \n"
					+ "            ,ISNULL(tgr.ContractAmt,0) AS ContractAmt           \n"
					+ "            ,(SELECT ISNULL(SUM(AdCnt),0)                       \n"
					+ "                FROM SummaryAdDaily0 with(nolock)               \n"
					+ "               WHERE LogDay BETWEEN SUBSTRING(itm.ExcuteStartDay,3,6) \n"
					+ "                                AND SUBSTRING(itm.RealEndDay,3,6)     \n"
					+ "                 AND ItemNo = itm.ItemNo                        \n"
					+ "                 AND SummaryType = 1   -- ��ǰ��                \n"
					+ "             ) AS TotAdCnt                                      \n"
					+ "            ,(SELECT COUNT(*)                                   \n"
					+ "               FROM (SELECT Distinct LogDay                     \n"
					+ "                       FROM SummaryAdDaily0 with(nolock)        \n"
					+ "                      WHERE LogDay BETWEEN SUBSTRING(itm.ExcuteStartDay,3,6) \n"
					+ "                                       AND SUBSTRING(itm.RealEndDay,3,6)     \n"
					+ "                        AND ItemNo = itm.ItemNo                 \n"
					+ "                        AND SummaryType = 1   -- ��ǰ��         \n"
					+ "                    ) TL                                        \n"
					+ "             ) AS TotDayCnt                                     \n"
					+ "            ,itm.ExcuteStartDay, itm.ExcuteEndDay, itm.RealEndDay  \n"
					+ "            ,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),GetDate() - 1 ) + 1 as ExDays  \n"
					+ "            ,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.ExcuteEndDay, 112)) TotExcDays  \n"
					+ "            ,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.RealEndDay, 112))   TotRealDays \n"
					+ "            ,itm.RapCode                                        \n"
					+ "            ,ctr.ContractName                                   \n"
					+ "            ,agn.AgencyName                                     \n"
					+ "            ,adv.AdvertiserName                                 \n"
                    + "            ,agn.AgencyCode                                     \n"
                    + "        FROM AdTargetsHanaTV.dbo.ContractItem itm with(nolock)                      \n"
                    + "             INNER JOIN AdTargetsHanaTV.dbo.Contract     ctr with(nolock) ON (itm.ContractSeq    = ctr.ContractSeq) \n"
                    + "             LEFT  JOIN AdTargetsHanaTV.dbo.Targeting    tgr with(nolock) ON (itm.ItemNo         = tgr.ItemNo)      \n"
                    + "             INNER JOIN AdTargetsHanaTV.dbo.Agency       agn with(nolock) ON (itm.AgencyCode     = agn.AgencyCode)     \n"
                    + "             INNER JOIN AdTargetsHanaTV.dbo.Advertiser   adv with(nolock) ON (itm.AdvertiserCode = adv.AdvertiserCode) \n"
					+ "       WHERE itm.AdState IN ('20','30')       -- �� �� ���� ����                            \n"
					+ "   --    AND	itm.AdType BETWEEN '10' AND '19' -- �ʼ�����                                     \n"
 					+ "     ) T                                                        \n"
					+ " WHERE 1 = 1                                                    \n"
					);

				// �˻�� ������
				if (statisticsTotalModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
						+ "    T.ItemName       LIKE '%"+ statisticsTotalModel.SearchKey+"%' \n"													
						+ " OR T.ContractName   LIKE '%"+ statisticsTotalModel.SearchKey+"%' \n"													
						+ " OR T.AgencyName     LIKE '%"+ statisticsTotalModel.SearchKey+"%' \n"													
						+ " OR T.AdvertiserName LIKE '%"+ statisticsTotalModel.SearchKey+"%' \n"													
						+ " ) \n"
						);
				}

				if(!statisticsTotalModel.SearchRapCode.Equals("00"))
				{
					sbQuery.Append("  AND T.RapCode = '"+statisticsTotalModel.SearchRapCode+"'  \n");
				}        
				if(!statisticsTotalModel.SearchAgencyCode.Equals("00"))
				{
					sbQuery.Append("  AND T.AgencyCode = '"+statisticsTotalModel.SearchAgencyCode+"'  \n");
				}     


				sbQuery.Append(""
					+ " ORDER BY ExcuteEndDay, ItemNo      \n"
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				statisticsTotalModel.ReportDataSet = ds.Copy();

				// ���
				statisticsTotalModel.ResultCnt = Utility.GetDatasetCount(statisticsTotalModel.ReportDataSet);

				ds.Dispose();


				// ����ڵ� ��Ʈ
				statisticsTotalModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsTotalModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsTotal() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsTotalModel.ResultCD = "3000";
                statisticsTotalModel.ResultDesc = "��ü��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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