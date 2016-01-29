/*
 * -------------------------------------------------------
 * Class Name: ReportBiz
 * �ֿ���  : ������ ó�� ����
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

namespace AdManagerWebService.Interface
{
	/// <summary>
	/// ReportBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ReportBiz : BaseBiz
	{
        public ReportBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }

        #region �ϰ� ���� ��û ����

        /// <summary>
        /// �ϰ� �������� ���� ����Ʈ
        /// </summary>
        /// <param name="mediaRep"></param>
//        public void DailyViewCnt_EachAd( int mediaRep, string viewDay )
//        {
//            try
//            {
//                StringBuilder sb = new StringBuilder();
//                sb.Append("\n");
//                sb.Append(" select   ItemNo         -- �����ȣ" + "\n");
//                sb.Append("         ,ItemName       -- �����(�����)" + "\n");
//                sb.Append(" 		,AdCnt          -- ���� �����" + "\n");
//                sb.Append(" 		,TotAdCnt       -- ���� �����" + "\n");
//                sb.Append(" 		,CASE WHEN ExDays <> 0 THEN FLOOR((TotAdCnt/CONVERT(float,ExDays)) * TotDaysExcu) ELSE 0 END AS ExpCntExcu -- ��������ϱ��� �������ġ" + "\n");
//                sb.Append(" 		,CASE WHEN ExDays <> 0 THEN FLOOR((TotAdCnt/CONVERT(float,ExDays)) * TotDaysReal) ELSE 0 END AS ExpCntReal -- ���������ϱ��� �������ġ" + "\n");
//                sb.Append(" 		,CASE WHEN ExDays <> 0 THEN (AdCnt * ( TotDaysExcu - ExDays) + TotAdCnt ) ELSE 0 END AS ExpCntDay   -- ���ϳ������ �������ġ" + "\n");
//                sb.Append(" 		,ContractAmt     -- ���������" + "\n");
//                sb.Append(" 		,ExcuteStartDay  -- ���������" + "\n");
//                sb.Append(" 		,ExcuteEndDay    -- ����������(����)" + "\n");
//                sb.Append(" 		,RealEndDay      -- ����������(����)" + "\n");
//                sb.Append(" 		,ExDays          -- ���ϱ����� �����ϼ�" + "\n");
//                sb.Append(" 		,TotDaysExcu     -- ������ �������ϼ�" + "\n");
//                sb.Append(" 		,TotDaysReal     -- �������� �������ϼ�" + "\n");
//                sb.Append(" 		,ContractSeq     -- �������ȣ" + "\n");
//                sb.Append(" 		,ContractName    -- �������" + "\n");
//                sb.Append(" 		,AgencyName      -- ������" + "\n");
//                sb.Append(" 		,AdvertiserName  -- �����ָ�" + "\n");
//                // ��������� ���� ����ġ ��� �����ϼ�*���ϳ���� + ���������
//                sb.Append(" from (  select   itm.ItemNo, itm.ItemName" + "\n");
//                sb.Append(" 		        ,( select   ISNULL(SUM(AdCnt),0)" + "\n");
//                sb.Append(" 				    from    SummaryAdDaily0 with(NoLock)" + "\n");
//                sb.Append(" 				    where   LogDay = '" + viewDay +"'" + "\n");
//                sb.Append(" 					and     ItemNo = itm.ItemNo" + "\n");
//                sb.Append(" 					and     SummaryType = 1" + "\n");
//                sb.Append(" 				  ) AS AdCnt" + "\n");
//                sb.Append(" 				,ISNULL(tgr.ContractAmt,0) AS ContractAmt" + "\n");
//                sb.Append(" 				,IsNull( (  SELECT top 1 isnull(AdCnt,0) + Isnull(AdCntAccu,0)" + "\n");
//                sb.Append(" 					        FROM    SummaryAdDaily0 with(NoLock)" + "\n");
//                sb.Append(" 					        WHERE   LogDay BETWEEN SUBSTRING(itm.ExcuteStartDay,3,6)" + "\n");
//                sb.Append(" 					                           AND CASE WHEN SUBSTRING(itm.RealEndDay,3,6) < '" + viewDay +"'" + "\n");
//                sb.Append(" 																	THEN SUBSTRING(itm.RealEndDay,3,6)" + "\n");
//                sb.Append(" 					                                    ELSE '" + viewDay +"' END" + "\n");
//                sb.Append(" 					        AND ItemNo = itm.ItemNo" + "\n");
//                sb.Append(" 					        AND SummaryType = 1" + "\n");
//                sb.Append(" 					        ORDER BY LogDay desc" + "\n");
//                sb.Append(" 					 ),0) AS TotAdCnt" + "\n");
//                sb.Append("                 ,itm.ExcuteStartDay, itm.ExcuteEndDay, itm.RealEndDay" + "\n");
//                sb.Append(" 				,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, '20" + adHitStatusModel.SearchDay +"', 112)) + 1 ExDays" + "\n");
//                sb.Append(" 				,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.ExcuteEndDay, 112)) + 1 TotDaysExcu" + "\n");
//                sb.Append(" 				,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.RealEndDay  , 112)) + 1 TotDaysReal" + "\n");
//                sb.Append(" 				,ctr.ContractSeq, ctr.ContractName" + "\n");
//                sb.Append(" 				,agn.AgencyName ,adv.AdvertiserName" + "\n");
//                sb.Append(" 				,itm.RapCode, itm.AgencyCode" + "\n");
//                sb.Append("         from    ContractItem itm with(nolock)" + "\n");
//                sb.Append(" 		INNER JOIN Contract     ctr with(nolock) ON (itm.ContractSeq    = ctr.ContractSeq)" + "\n");
//                sb.Append(" 		INNER JOIN Agency       agn with(nolock) ON (itm.AgencyCode     = agn.AgencyCode)" + "\n");
//                sb.Append(" 		INNER JOIN Advertiser   adv with(nolock) ON (itm.AdvertiserCode = adv.AdvertiserCode)" + "\n");
//                sb.Append(" 		LEFT  JOIN Targeting    tgr with(nolock) ON (itm.ItemNo         = tgr.ItemNo)" + "\n");
//                sb.Append(" WHERE itm.ExcuteStartDay <= '20" + viewDay +"'" + "\n");
//                sb.Append(" AND   itm.RealEndDay     >= '20" + viewDay +"'" + "\n");
//                sb.Append(" AND   itm.AdType < '90'" + "\n");
//                sb.Append("  ) T" + "\n");
//                sb.Append(" Where   T.RapCode = " + mediaRep +" \n");
//                sb.Append(" ORDER BY AdCnt DESC" + "\n");
//
//                _log.Debug("-----------------------------------------");
//                _log.Debug(this.ToString() + ".DailyViewCntEachAd() Start");
//                _log.Debug("-----------------------------------------");
//                _log.Debug("<�Է�����>");
//                _log.Debug("MediaRep    :[" + mediaRep  + "]");
//                _log.Debug("ViewDay     :[" + viewDay   + "]");
//                _log.Debug(sbQuery.ToString());
//				
//                // ��������
//                DataSet ds = new DataSet();
//                _db.ExecuteQuery(ds,sbQuery.ToString());
//
//                // ��� DataSet�� �����͸𵨿� ����
//                adHitStatusModel.ReportDataSet = ds.Copy();
//
//                // ���
//                adHitStatusModel.ResultCnt = Utility.GetDatasetCount(adHitStatusModel.ReportDataSet);
//
//                ds.Dispose();
//
//
//                // ����ڵ� ��Ʈ
//                adHitStatusModel.ResultCD = "0000";
//
//                // __DEBUG__
//                _log.Debug("<�������>");
//                _log.Debug("ResultCnt:[" + adHitStatusModel.ResultCnt + "]");
//                // __DEBUG__
//
//                _log.Debug("-----------------------------------------");
//                _log.Debug(this.ToString() + "GetAdHitStatus() End");
//                _log.Debug("-----------------------------------------");
//            }
//            catch(Exception ex)
//            {
//                adHitStatusModel.ResultCD = "3000";
//                adHitStatusModel.ResultDesc = "���� ��û��Ȳ ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
//                _log.Exception(ex);
//            }
//            finally
//            {
//                // �����ͺ��̽���  Close�Ѵ�
//                _db.Close();
//            }
//
//        }
        #endregion

        /// <summary>
        /// ���α׷��� �����û ����
        /// </summary>
        public DataSet PeriodViewCntAdver_EachProgram( int ContractSeq, int CampaignCd, int ItemNo, string BgnDay, string EndDay)
        {
            DataSet ds = new DataSet();

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "PeriodViewCntAdver_EachProgram() Start");
                _log.Debug("-----------------------------------------");
			
                #region [SQL]
                StringBuilder sb = new StringBuilder();
                sb.Append( " SELECT  ad.CategoryCode \n");
                sb.Append( "        ,ad.CategoryName \n");
                sb.Append( "        ,ad.GenreCode \n");
                sb.Append( "        ,ad.GenreName \n");
                sb.Append( "        ,ad.ChannelNo \n");
                sb.Append( "        ,ad.ProgramNm \n");
                sb.Append( "        ,ad.AdHitCnt \n");
                sb.Append( "        ,ad.PgHitCnt \n");
                sb.Append( " FROM ( SELECT   C.CategoryCode \n");
                sb.Append( "                ,C.CategoryName \n");
                sb.Append( "                ,D.GenreCode \n");
                sb.Append( "                ,D.GenreName \n");
                sb.Append( "                ,B.Channel AS ChannelNo \n");
                sb.Append( "                ,B.ProgramNm \n");
                sb.Append( "                ,A.ProgKey \n");
                sb.Append( "                ,SUM(A.AdCnt)  AS AdHitCnt \n");
                sb.Append( "                ,SUM(A.HitCnt) AS PgHitCnt \n");
                sb.Append( "        FROM SummaryAdDaily3 A with(NoLock) \n");
                sb.Append( "        INNER JOIN AdTargetsHanaTV.dbo.Program   B with(NoLock) ON (A.ProgKey  = B.ProgramKey) \n");
                sb.Append( "        INNER JOIN AdTargetsHanaTV.dbo.Category  C with(NoLock) ON (A.Category = C.CategoryCode) \n");
                sb.Append( "        INNER JOIN AdTargetsHanaTV.dbo.Genre     D with(NoLock) ON (A.Genre    = D.GenreCode) \n");
                sb.Append( "        WHERE   B.MediaCode = 1 \n");
                sb.Append( "        AND     A.LogDay BETWEEN '" + BgnDay + "' AND '" + EndDay + "' \n");
                #region ��ȸ����(���,ķ����,����)
                if( ItemNo > 0 )
                {
                    // �����ȣ�� ����������, ķ���μ��ÿ��ο� �����ϰ� 1���� ���� ���� ������
                    // ķ������ ��ü�̸鼭 �������� �����̸� ����������
                    sb.Append("        AND		A.ItemNo  = " + ItemNo + "\n");
                
                }
                else if( CampaignCd == 0 && ItemNo == 0 )
                {
                    // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sb.Append("        AND		A.ItemNo in(select  ItemNo ");
                    sb.Append("                            from    AdTargetsHanaTV.dbo.ContractItem with(noLock)");
                    sb.Append("        		            where   MediaCode   = 1");
                    sb.Append("        					and		ContractSeq = " + ContractSeq + ")");
                }
                else if( CampaignCd > 0 && ItemNo == 0 )
                {
                    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sb.Append("        AND     A.ItemNo in(select ItemNo");
                    sb.Append("        				    from	AdTargetsHanaTV.dbo.CampaignDetail d with(noLock)");
                    sb.Append("        					inner join AdTargetsHanaTV.dbo.CampaignMaster m with(noLock) ");
                    sb.Append("        							on m.CampaignCode = d.CampaignCode");
                    sb.Append("        							and m.MediaCode = 1");
                    sb.Append("        							and	m.ContractSeq	= " + ContractSeq );
                    sb.Append("        							and m.CampaignCode	= " + CampaignCd + ")");
                }
                #endregion
                sb.Append( "        AND     A.ProgKey   > 0 \n");
                sb.Append( "        GROUP BY CategoryCode, CategoryName, GenreCode, GenreName,Channel,ProgramNm,ProgKey \n");
                sb.Append( "      ) as ad \n");
                sb.Append( " ORDER BY ad.CategoryCode, ad.GenreCode,ad.ProgramNm \n"); 
                #endregion

                #region [ �Ķ���� DEBUG ]
                _log.Debug("ContractSeq :[" + ContractSeq.ToString() + "]");
                _log.Debug("CampaignCd  :[" + CampaignCd.ToString() + "]");
                _log.Debug("ItemNo      :[" + ItemNo.ToString() + "]");
                _log.Debug("BgnDay      :[" + BgnDay      + "]");
                _log.Debug("EndDay      :[" + EndDay      + "]");
                _log.Debug("-----------------------------------------");
                _log.Debug(sb.ToString());
                _log.Debug("-----------------------------------------");
                #endregion
				
                // ��������
                _db.Open();
				_db.ExecuteQuery(ds, sb.ToString());

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "PeriodViewCntAdver_EachProgram() End");
                _log.Debug("-----------------------------------------");

                return ds.Copy();
            }
            catch(Exception ex)
            {
                _log.Exception(ex);
                return null;
            }
            finally
            {
                _db.Close();
                if( ds != null )
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }

	}
}
