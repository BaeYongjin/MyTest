// ===============================================================================
//
// AdHitStatusBiz.cs
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
 * Class Name: AdHitStatusBiz
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
    /// AdHitStatusBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class AdHitStatusBiz : BaseBiz
    {
		#region  ������
        public AdHitStatusBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

        #region ���� ��û��Ȳ ����
        /// <summary>
        /// ���� ��û��Ȳ ����
        /// </summary>
		/// <param name="adHitStatusModel"></param>
        public void GetAdHitStatus(HeaderModel header, AdHitStatusModel adHitStatusModel)
        {
			try
			{
                StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdHitStatus() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(adHitStatusModel.SearchDay.Length > 6) adHitStatusModel.SearchDay = adHitStatusModel.SearchDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	 :[" + adHitStatusModel.SearchMediaCode + "]");		// �˻� ��ü
				_log.Debug("SearchRapCode	 :[" + adHitStatusModel.SearchRapCode + "]");		// �˻� �̵�
				_log.Debug("SearchAgencyCode :[" + adHitStatusModel.SearchAgencyCode + "]");		// �˻� �����
				_log.Debug("SearchDay        :[" + adHitStatusModel.SearchDay       + "]");		// �˻� ��������           
				_log.Debug("SearchKey        :[" + adHitStatusModel.SearchKey       + "]");		// �˻� Ű����           
                // __DEBUG__

                // ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n-- ���� ��û��Ȳ" + "\n");
				sbQuery.Append(" SELECT  ItemNo         -- �����ȣ" + "\n");
				sbQuery.Append("        ,ItemName       -- �����(�����)" + "\n");
				sbQuery.Append(" 		,AdCnt          -- ���� �������" + "\n");
				sbQuery.Append(" 		,TotAdCnt       -- ���ϱ����� ���� �������" + "\n");
				sbQuery.Append(" 		,CASE WHEN ExDays <> 0 THEN FLOOR((TotAdCnt/CONVERT(float,ExDays)) * TotDaysExcu) ELSE 0 END AS ExpCntExcu -- ��������ϱ��� �������ġ" + "\n");
				sbQuery.Append(" 		,CASE WHEN ExDays <> 0 THEN FLOOR((TotAdCnt/CONVERT(float,ExDays)) * TotDaysReal) ELSE 0 END AS ExpCntReal -- ���������ϱ��� �������ġ" + "\n");
				sbQuery.Append(" 		,ContractAmt     -- ���������" + "\n");
				sbQuery.Append(" 		,ExcuteStartDay  -- ���������" + "\n");
				sbQuery.Append(" 		,ExcuteEndDay    -- ����������(����)" + "\n");
				sbQuery.Append(" 		,RealEndDay      -- ����������(����)" + "\n");
				sbQuery.Append(" 		,ExDays          -- ���ϱ����� �����ϼ�" + "\n");
				sbQuery.Append(" 		,TotDaysExcu     -- ������ �������ϼ�" + "\n");
				sbQuery.Append(" 		,TotDaysReal     -- �������� �������ϼ�" + "\n");
				sbQuery.Append(" 		,ContractSeq     -- �������ȣ" + "\n");
				sbQuery.Append(" 		,ContractName    -- �������" + "\n");
				sbQuery.Append(" 		,AgencyName      -- ������" + "\n");
				sbQuery.Append(" 		,AdvertiserName  -- �����ָ�" + "\n");
				// ��������� ���� ����ġ ��� �����ϼ�*���ϳ���� + ���������
				sbQuery.Append(" 		,CASE WHEN ExDays <> 0 THEN (AdCnt * ( TotDaysExcu - ExDays) + TotAdCnt ) ELSE 0 END AS ExpCntDay   -- ���ϳ������ �������ġ" + "\n");
				sbQuery.Append(" FROM (	SELECT   itm.ItemNo, itm.ItemName" + "\n");
				sbQuery.Append(" 		        ,(SELECT ISNULL(SUM(AdCnt),0)" + "\n");
				sbQuery.Append(" 				  FROM SummaryAdDaily0 with(NoLock)" + "\n");
				sbQuery.Append(" 				  WHERE LogDay = '" + adHitStatusModel.SearchDay +"'" + "\n");
				sbQuery.Append(" 				  AND ItemNo = itm.ItemNo" + "\n");
				sbQuery.Append(" 				  AND SummaryType = 1" + "\n");
				sbQuery.Append(" 					 ) AS AdCnt" + "\n");
				sbQuery.Append(" 				,ISNULL(tgr.ContractAmt,0) AS ContractAmt" + "\n");
				sbQuery.Append(" 				,IsNull( (SELECT top 1 isnull(AdCnt,0) + Isnull(AdCntAccu,0)" + "\n");
				sbQuery.Append(" 					          FROM SummaryAdDaily0 with(NoLock)" + "\n");
				sbQuery.Append(" 					          WHERE LogDay BETWEEN SUBSTRING(itm.ExcuteStartDay,3,6)" + "\n");
				sbQuery.Append(" 					                           AND CASE WHEN SUBSTRING(itm.RealEndDay,3,6) < '" + adHitStatusModel.SearchDay +"'" + "\n");
				sbQuery.Append(" 																	THEN SUBSTRING(itm.RealEndDay,3,6)" + "\n");
				sbQuery.Append(" 					                                    ELSE '" + adHitStatusModel.SearchDay +"' END" + "\n");
				sbQuery.Append(" 					          AND ItemNo = itm.ItemNo" + "\n");
				sbQuery.Append(" 					          AND SummaryType = 1" + "\n");
				sbQuery.Append(" 					          ORDER BY LogDay desc" + "\n");
				sbQuery.Append(" 					 ),0) AS TotAdCnt" + "\n");
				sbQuery.Append(" 					,itm.ExcuteStartDay, itm.ExcuteEndDay, itm.RealEndDay" + "\n");
				sbQuery.Append(" 					,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, '20" + adHitStatusModel.SearchDay +"', 112)) + 1 ExDays" + "\n");
				sbQuery.Append(" 					,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.ExcuteEndDay, 112)) + 1 TotDaysExcu" + "\n");
				sbQuery.Append(" 					,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.RealEndDay  , 112)) + 1 TotDaysReal" + "\n");
				sbQuery.Append(" 					,ctr.ContractSeq, ctr.ContractName" + "\n");
				sbQuery.Append(" 					,agn.AgencyName ,adv.AdvertiserName" + "\n");
				sbQuery.Append(" 					,itm.RapCode, itm.AgencyCode, itm.AdType " + "\n");
                sbQuery.Append("			FROM AdTargetsHanaTV.dbo.ContractItem itm with(nolock)" + "\n");
                sbQuery.Append(" 			INNER JOIN AdTargetsHanaTV.dbo.Contract     ctr with(nolock) ON (itm.ContractSeq    = ctr.ContractSeq)" + "\n");
                sbQuery.Append(" 			INNER JOIN AdTargetsHanaTV.dbo.Agency       agn with(nolock) ON (itm.AgencyCode     = agn.AgencyCode)" + "\n");
                sbQuery.Append(" 			INNER JOIN AdTargetsHanaTV.dbo.Advertiser   adv with(nolock) ON (itm.AdvertiserCode = adv.AdvertiserCode)" + "\n");
                sbQuery.Append(" 			LEFT  JOIN AdTargetsHanaTV.dbo.Targeting    tgr with(nolock) ON (itm.ItemNo         = tgr.ItemNo)" + "\n");
				sbQuery.Append("		WHERE itm.ExcuteStartDay <= '20" + adHitStatusModel.SearchDay +"'" + "\n");
				sbQuery.Append("		AND   itm.RealEndDay     >= '20" + adHitStatusModel.SearchDay +"'" + "\n");
				sbQuery.Append("		AND   itm.AdType < '90'" + "\n");
				sbQuery.Append("  ) T" + "\n");
				sbQuery.Append(" WHERE 1 = 1" + "\n");
				// 2015-04-01 ���� �߰���
				sbQuery.Append(" AND	AdCnt >5\n ");

				// �˻�� ������
				if (adHitStatusModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND (    T.ItemName       LIKE '%"+ adHitStatusModel.SearchKey+"%'" + "\n");
					sbQuery.Append(" 		  OR T.ContractName   LIKE '%"+ adHitStatusModel.SearchKey+"%'" + "\n");
					sbQuery.Append(" 		  OR T.AgencyName     LIKE '%"+ adHitStatusModel.SearchKey+"%'" + "\n");
					sbQuery.Append(" 		  OR T.AdvertiserName LIKE '%"+ adHitStatusModel.SearchKey+"%'" + "\n");
					sbQuery.Append("      )" + "\n");
				}

				if(!adHitStatusModel.SearchRapCode.Equals("00"))
				{
					sbQuery.Append("  AND T.RapCode = '"+adHitStatusModel.SearchRapCode+"'  \n");
				}        
				if(!adHitStatusModel.SearchAgencyCode.Equals("00"))
				{
					sbQuery.Append("  AND T.AgencyCode = '"+adHitStatusModel.SearchAgencyCode+"'  \n");
				}

                if (!adHitStatusModel.SearchAdType.Equals("00"))
                {
                    sbQuery.Append("  AND T.AdType = '"+adHitStatusModel.SearchAdType+"'  \n");
                }

                if (!adHitStatusModel.SearchAgencyCode.Equals("00"))
                {
                    sbQuery.Append("  AND T.AgencyCode = '"+adHitStatusModel.SearchAgencyCode+"'  \n");
                }     

				sbQuery.Append(" ORDER BY AdCnt DESC" + "\n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
                _db.Timeout = 60 * 5;
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				adHitStatusModel.ReportDataSet = ds.Copy();

				// ���
				adHitStatusModel.ResultCnt = Utility.GetDatasetCount(adHitStatusModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				adHitStatusModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + adHitStatusModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdHitStatus() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                adHitStatusModel.ResultCD = "3000";
                adHitStatusModel.ResultDesc = "���� ��û��Ȳ ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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