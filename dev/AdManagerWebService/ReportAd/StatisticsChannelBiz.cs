// ===============================================================================
//
// StatisticsChannelBiz.cs
//
// ������Ʈ ä����� ���� 
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
 * Class Name: StatisticsChannelBiz
 * �ֿ���  : ������Ʈ ä����� ó�� ����
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
    /// StatisticsChannelBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class StatisticsChannelBiz : BaseBiz
    {

		#region  ������
        public StatisticsChannelBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region �Ⱓ�� ä�����
        /// <summary>
        ///  �Ⱓ�� ä�����
        /// </summary>
        /// <param name="statisticsChannelModel"></param>
        public void GetStatisticsChannel(HeaderModel header, StatisticsChannelModel statisticsChannelModel)
        {
			bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
			bool isNotReady  = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsChannel() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(statisticsChannelModel.SearchStartDay.Length > 6) statisticsChannelModel.SearchStartDay = statisticsChannelModel.SearchStartDay.Substring(2,6);
				if(statisticsChannelModel.SearchEndDay.Length   > 6) statisticsChannelModel.SearchEndDay   = statisticsChannelModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	  :[" + statisticsChannelModel.SearchMediaCode   + "]");		// �˻� ��ü
				_log.Debug("SearchContractSeq :[" + statisticsChannelModel.SearchContractSeq + "]");		// �˻� �����ȣ           
				_log.Debug("SearchItemNo      :[" + statisticsChannelModel.SearchItemNo      + "]");		// �˻� �����ȣ           
				_log.Debug("SearchStartDay    :[" + statisticsChannelModel.SearchStartDay    + "]");		// �˻� ������� ����          
				_log.Debug("SearchEndDay      :[" + statisticsChannelModel.SearchEndDay      + "]");		// �˻� �������� ����          
				// __DEBUG__

				string  MediaCode   = statisticsChannelModel.SearchMediaCode;
                int	    ContractSeq = Convert.ToInt32( statisticsChannelModel.SearchContractSeq );
                int	    CampaignCd	= Convert.ToInt32( statisticsChannelModel.CampaignCode );
                int	    ItemNo      = Convert.ToInt32( statisticsChannelModel.SearchItemNo );
                string  StartDay    = statisticsChannelModel.SearchStartDay;
				string  EndDay      = statisticsChannelModel.SearchEndDay;

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
                statisticsChannelModel.ContractAmt = ContractAmt;
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
                statisticsChannelModel.TotalAdCnt = TotalAdCnt;
                #endregion

                #region [ ���� ���� ]
                sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" /* �Ⱓ�� ���� ä�����*/  \n");
                sbQuery.Append(" declare @TotAdHit int;     \n");
                sbQuery.Append(" set     @TotAdHit    = " + TotalAdCnt + "; \n");
                sbQuery.Append(" with AdData( Category, Genre, Program, AdCnt )" + "\n");
                sbQuery.Append(" as ( SELECT a.Category" + "\n");
                sbQuery.Append(" 			,a.Genre" + "\n");
                sbQuery.Append(" 			,a.ProgKey" + "\n");
                sbQuery.Append(" 			,sum(A.AdCnt) as AdCnt" + "\n");
                sbQuery.Append(" 	    FROM SummaryAdDaily3 A with(NoLock)       " + "\n");
                sbQuery.Append(" 		LEFT JOIN AdTargetsHanaTV.dbo.Program	 B with(NoLock) ON A.ProgKey    = B.ProgramKey" + "\n");
                sbQuery.Append(" 		LEFT JOIN AdTargetsHanaTV.dbo.Category	 C with(NoLock) ON A.Category   = C.CategoryCode" + "\n");
                sbQuery.Append(" 		LEFT JOIN AdTargetsHanaTV.dbo.Genre		 D with(NoLock) ON A.Genre		= D.GenreCode" + "\n");
                sbQuery.Append(" 		WHERE	a.LogDay	between  '"+ StartDay + "' AND '"+ EndDay + "'" + "\n" );
                #region ��ȸ����(���,ķ����,����)
                if( ItemNo > 0 )                            // ķ������ ��ü�̸鼭 �������� �����̸� ����������
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
                #endregion
                sbQuery.Append(" 		AND		a.ProgKey   > 0                     " + "\n");
                sbQuery.Append(" 		GROUP BY a.Category,a.Genre,a.ProgKey" + "\n");
                sbQuery.Append(" )" + "\n");
                sbQuery.Append(" select	AdTargetsHanaTV.dbo.ufnPadding('L',v.Category,5,' ') + ' ' + v.CategoryName as Category" + "\n");
                sbQuery.Append(" 			, v.CategoryName" + "\n");
                sbQuery.Append(" 			, v.OrdCode" + "\n");
                sbQuery.Append(" 			, v.Ord" + "\n");
                sbQuery.Append(" 			, v.OrdName" + "\n");
                sbQuery.Append(" 			, v.Title" + "\n");
                sbQuery.Append(" 			, v.AdCnt" + "\n");
                sbQuery.Append(" 			, convert( decimal(5,2),( v.AdCnt / cast(@TotAdHit as float) * 100) )	as AdRate" + "\n");
                sbQuery.Append(" 			, Replicate('*', round( v.AdCnt / cast(@TotAdHit as float) * 100,0) ) as RateBar     " + "\n");
                sbQuery.Append(" from	(" + "\n");
                sbQuery.Append(" 				select	a.Category			as	Category" + "\n");
                sbQuery.Append(" 							, c.CategoryName	as CategoryName" + "\n");
                sbQuery.Append(" 							, '1'							as	OrdCode" + "\n");
                sbQuery.Append(" 							, '1 ī�װ�'			as	Ord" + "\n");
                sbQuery.Append(" 							, 'ī�װ�'				as	OrdName" + "\n");
                sbQuery.Append(" 							, ''							as Title" + "\n");
                sbQuery.Append(" 							, sum(AdCnt)			as	AdCnt" + "\n");
                sbQuery.Append(" 				from	AdData a" + "\n");
                sbQuery.Append(" 				left join AdTargetsHanaTV.dbo.Category c with(NoLock) on A.Category = C.CategoryCode" + "\n");
                sbQuery.Append(" 				group by a.Category,c.CategoryName" + "\n");
                sbQuery.Append(" 				union all" + "\n");
                sbQuery.Append(" 				select	a.Category" + "\n");
                sbQuery.Append(" 							, c.CategoryName	as CategoryName" + "\n");
                sbQuery.Append(" 							, '2'						as	OrdCode" + "\n");
                sbQuery.Append(" 							, '2 �帣	'			as	Ord" + "\n");
                sbQuery.Append(" 							, '�帣'					as	OrdName" + "\n");
                sbQuery.Append(" 							, min(d.GenreName)   as Title" + "\n");
                sbQuery.Append(" 							, sum(AdCnt)" + "\n");
                sbQuery.Append(" 				from	AdData a" + "\n");
                sbQuery.Append(" 				left join AdTargetsHanaTV.dbo.Category c with(NoLock) on A.Category = C.CategoryCode" + "\n");
                sbQuery.Append(" 				left join	AdTargetsHanaTV.dbo.Genre d with(NoLock) on a.Genre = d.GenreCode" + "\n");
                sbQuery.Append(" 				group by a.Category,c.CategoryName, a.Genre" + "\n");
                sbQuery.Append(" 				union all" + "\n");
                sbQuery.Append(" 				select	a.Category" + "\n");
                sbQuery.Append(" 							, c.CategoryName	as CategoryName" + "\n");
                sbQuery.Append(" 							, '3'				as	OrdCode" + "\n");
                sbQuery.Append(" 							, '3 ä��'		as	Ord" + "\n");
                sbQuery.Append(" 							, 'ä��'			as	OrdName" + "\n");
                sbQuery.Append(" 							, b.ProgramNm as Title" + "\n");
                sbQuery.Append(" 							, sum(AdCnt)" + "\n");
                sbQuery.Append(" 				from	AdData a" + "\n");
                sbQuery.Append(" 				left join AdTargetsHanaTV.dbo.Category c with(NoLock) on A.Category = C.CategoryCode" + "\n");
                sbQuery.Append(" 				left	join AdTargetsHanaTV.dbo.Program	b with(NoLock) on a.Program = b.ProgramKey" + "\n");
                sbQuery.Append(" 				group by a.Category, c.CategoryName, b.ProgramNm ) v" + "\n");
                sbQuery.Append(" order by Category,OrdCode, AdCnt desc" + "\n");
                #endregion

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				statisticsChannelModel.ReportDataSet = ds.Copy();

				// ���
				statisticsChannelModel.ResultCnt = Utility.GetDatasetCount(statisticsChannelModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				statisticsChannelModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsChannelModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsChannel() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsChannelModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsChannelModel.ResultDesc = "�ش籤���� ������ �������� �ʽ��ϴ�.";
				}
				else if(isNotReady)
				{
					statisticsChannelModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					statisticsChannelModel.ResultDesc = "ä����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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