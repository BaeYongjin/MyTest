// ===============================================================================
//
// DailyProgramHitBiz.cs
//
// �Ϻ� ���α׷� ��ûȽ������ ���� 
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
 * Class Name: DailyProgramHitBiz
 * �ֿ���  : �Ϻ� ���α׷� ��ûȽ������ ó�� ����
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
    /// DailyProgramHitBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class DailyProgramHitBiz : BaseBiz
    {
		#region  ������
        public DailyProgramHitBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region ���α׷��� ��ûȽ������
        /// <summary>
        /// ���α׷��� ��ûȽ������
        /// 20100419 ����ȯ ����. ķ������ �������Ƿ� �αװ� �ߺ��� �Ǵ� ���� �߻�. ���� �̺κ��� �ߺ��Ǵ� �� ��ŭ ������ ���.
        /// </summary>
        /// <param name="dailyProgramHitModel"></param>
        public void GetDailyProgramHit(HeaderModel header, DailyProgramHitModel dailyProgramHitModel)
        {

            try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDailyProgramHit() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(dailyProgramHitModel.SearchBgnDay.Length > 6) dailyProgramHitModel.SearchBgnDay = dailyProgramHitModel.SearchBgnDay.Substring(2,6);
				if(dailyProgramHitModel.SearchEndDay.Length > 6) dailyProgramHitModel.SearchEndDay = dailyProgramHitModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode   :[" + dailyProgramHitModel.SearchMediaCode   + "]");		// �˻� ��ü
				_log.Debug("SearchContractSeq :[" + dailyProgramHitModel.SearchContractSeq + "]");		// �˻� ����ȣ           
				_log.Debug("SearchItemNo      :[" + dailyProgramHitModel.SearchItemNo      + "]");		// �˻� �����ȣ           
				_log.Debug("SearchType        :[" + dailyProgramHitModel.SearchType        + "]");		// �˻� ���� D:���ñⰣ C:���Ⱓ(��������Ⱓ)        
				_log.Debug("SearchBgnDay      :[" + dailyProgramHitModel.SearchBgnDay      + "]");		// �˻� �����������           
				_log.Debug("SearchEndDay      :[" + dailyProgramHitModel.SearchEndDay      + "]");		// �˻� ������������           
                // __DEBUG__

				string  MediaCode   = dailyProgramHitModel.SearchMediaCode;
                int     ContractSeq = Convert.ToInt32(dailyProgramHitModel.SearchContractSeq);
                int     CampaignCd  = Convert.ToInt32(dailyProgramHitModel.CampaignCode);
                int     ItemNo      = Convert.ToInt32(dailyProgramHitModel.SearchItemNo);

				string  BgnDay      = dailyProgramHitModel.SearchBgnDay;
				string  EndDay      = dailyProgramHitModel.SearchEndDay;

                // ��������
				sbQuery = new StringBuilder();

                sbQuery.Append ("\n"
                    + " SELECT   x.LogDay"  + "\n"
                    + "         ,x.Category" + "\n"
                    + "         ,ct.CategoryName" + "\n"
                    + "         ,x.Genre" + "\n"
                    + "         ,gr.GenreName" + "\n"
                    + "         ,ISNULL(y.AdCnt,0) as HitCnt" + "\n"
                    + " FROM (   /* �ش�Ⱓ�� �Ϻ��� ī�װ�-�帣 �����ϱ�, ����ʵ� ���ڵ� ǥ���ϱ� ���ؼ� */" + "\n"
                    + "			SELECT	 bs.LogDay" + "\n"
                    + "				    ,ad.Category" + "\n"
                    + "					,ad.Genre" + "\n"
                    + "         FROM    SummaryBase bs with(noLock)" + "\n"
                    + "			      , (   /* �ش�Ⱓ�� ����� ī�װ�-�帣 ���ϱ� */" + "\n"
                    + "                     SELECT	Category, Genre" + "\n"
                    + "                     FROM    SummaryAdDaily3 a with(NoLock)" + "\n"
                    + "						WHERE   A.LogDay  BETWEEN '" + BgnDay + "' AND '" + EndDay + "'" + "\n");
                #region ��ȸ����(���,ķ����,����)
                if( ItemNo > 0 )
                {
                    // �����ȣ�� ����������, ķ���μ��ÿ��ο� �����ϰ� 1���� ���� ���� ������
                    // ķ������ ��ü�̸鼭 �������� �����̸� ����������
                    sbQuery.Append("        AND		A.ItemNo  = " + ItemNo + "\n");
                
                }
                else if( CampaignCd == 0 && ItemNo == 0 )
                {
                    // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("        AND		A.ItemNo in(select  ItemNo ");
                    sbQuery.Append("                            from    AdTargetsHanaTV.dbo.ContractItem with(noLock)");
                    sbQuery.Append("        		            where   MediaCode   = 1");
                    sbQuery.Append("        					and		ContractSeq = " + ContractSeq + ")");
                }
                else if( CampaignCd > 0 && ItemNo == 0 )
                {
                    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("        AND     A.ItemNo in(select ItemNo");
                    sbQuery.Append("        				    from	AdTargetsHanaTV.dbo.CampaignDetail d with(noLock)");
                    sbQuery.Append("        					inner join AdTargetsHanaTV.dbo.CampaignMaster m with(noLock) ");
                    sbQuery.Append("        							on m.CampaignCode = d.CampaignCode");
                    sbQuery.Append("        							and m.MediaCode = 1");
                    sbQuery.Append("        							and	m.ContractSeq	= " + ContractSeq );
                    sbQuery.Append("        							and m.CampaignCode	= " + CampaignCd + ")");
                }
                #endregion

                sbQuery.Append("            GROUP BY a.Category,a.Genre ) ad" + "\n"
                    + "			WHERE	bs.LogDay	BETWEEN '" + BgnDay + "' AND '" + EndDay + "'" + "\n"
                    + "     ) as x" + "\n"
                    + " LEFT OUTER JOIN" + "\n"
                    + "     (   /* �Ϻ�-ī�װ��帣������ ���⹰�� ���ϱ� */" + "\n"
                    + "			SELECT	 LogDay" + "\n"
                    + "			        ,Category" + "\n"
                    + "					,Genre" + "\n"
                    + "					,sum(HitCnt)/COUNT(DISTINCT ItemNo)	as AdCnt" + "\n"	//�ߺ��Ǵ� �� ��ŭ ������ �� �� �ֵ��� ����.
                    + "			FROM SummaryAdDaily3 a with(NoLock) " + "\n"
                    + "			WHERE A.LogDay	BETWEEN '" + BgnDay + "' AND '" + EndDay + "'" + "\n");

                #region ��ȸ����(���,ķ����,����)
                if( ItemNo > 0 )
                {
                    // �����ȣ�� ����������, ķ���μ��ÿ��ο� �����ϰ� 1���� ���� ���� ������
                    // ķ������ ��ü�̸鼭 �������� �����̸� ����������
                    sbQuery.Append("        AND		A.ItemNo  = " + ItemNo + "\n");
                
                }
                else if( CampaignCd == 0 && ItemNo == 0 )
                {
                    // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("        AND		A.ItemNo in(select  ItemNo ");
                    sbQuery.Append("                            from    AdTargetsHanaTV.dbo.ContractItem with(noLock)");
                    sbQuery.Append("        		            where   MediaCode   = 1");
                    sbQuery.Append("        					and		ContractSeq = " + ContractSeq  + ")");
                }
                else if( CampaignCd > 0 && ItemNo == 0 )
                {
                    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("        AND     A.ItemNo in(select ItemNo");
                    sbQuery.Append("        				    from	AdTargetsHanaTV.dbo.CampaignDetail d with(noLock)");
                    sbQuery.Append("        					inner join AdTargetsHanaTV.dbo.CampaignMaster m with(noLock) ");
                    sbQuery.Append("        							on m.CampaignCode = d.CampaignCode");
                    sbQuery.Append("        							and m.MediaCode = 1");
                    sbQuery.Append("        							and	m.ContractSeq	= " + ContractSeq );
                    sbQuery.Append("        							and m.CampaignCode	= " + CampaignCd + ")");
                }
                #endregion

                sbQuery.Append(""
                    + "			GROUP BY a.LogDay,a.Category,a.Genre" + "\n"
                    + "     ) as y	ON  x.LogDay    = y.LogDay" + "\n"
                    + "		        AND x.Category	= y.Category" + "\n"
                    + " 			AND x.Genre		= y.Genre" + "\n"
                    + " INNER JOIN AdTargetsHanaTV.dbo.Category	ct	with(noLock)    on x.Category   = ct.CategoryCode" + "\n"
                    + " INNER JOIN AdTargetsHanaTV.dbo.Genre		gr	with(noLock)	on x.Genre		= gr.GenreCode" + "\n"
                    + " ORDER BY x.LogDay, x.Category, x.Genre");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				dailyProgramHitModel.ReportDataSet = ds.Copy();

				// ���
				dailyProgramHitModel.ResultCnt = Utility.GetDatasetCount(dailyProgramHitModel.ReportDataSet);

				ds.Dispose();

				// ��������
				sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "-- ����� �帣��ȸ                                                                          \n"
                    + " SELECT  x.Category                                                              \n"
                    + "		   ,ct.CategoryName                                                         \n"
                    + "		   ,x.Genre                                                                 \n"
                    + "		   ,gr.GenreName                                                            \n"
                    + " FROM  (                                                                         \n"
                    + "         SELECT	Category, Genre                                                 \n"
                    + "         FROM SummaryAdDaily3 a with(NoLock)                                     \n"
                    + "         WHERE A.LogDay	BETWEEN '" + BgnDay + "' AND '" + EndDay + "'           \n");
                
                #region ��ȸ����(���,ķ����,����)
                if( ItemNo > 0 )
                {
                    // �����ȣ�� ����������, ķ���μ��ÿ��ο� �����ϰ� 1���� ���� ���� ������
                    // ķ������ ��ü�̸鼭 �������� �����̸� ����������
                    sbQuery.Append("        AND		A.ItemNo  = " + ItemNo + "\n");
                
                }
                else if( CampaignCd == 0 && ItemNo == 0 )
                {
                    // ķ������ ��ü�̸鼭 �������� ��ü�̸� ��ü �����
                    sbQuery.Append("        AND		A.ItemNo in(select  ItemNo ");
                    sbQuery.Append("                            from    AdTargetsHanaTV.dbo.ContractItem with(noLock)");
                    sbQuery.Append("        		            where   MediaCode   = 1");
                    sbQuery.Append("        					and		ContractSeq = " + ContractSeq  + ")");
                }
                else if( CampaignCd > 0 && ItemNo == 0 )
                {
                    // ķ������ ���õǰ�, ��ü����� �ش�ķ���� ��ü��
                    sbQuery.Append("        AND     A.ItemNo in(select ItemNo");
                    sbQuery.Append("        				    from	AdTargetsHanaTV.dbo.CampaignDetail d with(noLock)");
                    sbQuery.Append("        					inner join AdTargetsHanaTV.dbo.CampaignMaster m with(noLock) ");
                    sbQuery.Append("        							on m.CampaignCode = d.CampaignCode");
                    sbQuery.Append("        							and m.MediaCode = 1");
                    sbQuery.Append("        							and	m.ContractSeq	= " + ContractSeq );
                    sbQuery.Append("        							and m.CampaignCode	= " + CampaignCd + ")");
                }
                #endregion

                sbQuery.Append(""
                    + "         GROUP BY a.Category,a.Genre ) as x                                      \n"
                    + "INNER JOIN AdTargetsHanaTV.dbo.Category	ct	with(noLock)	on x.Category   = ct.CategoryCode       \n"
                    + "INNER JOIN AdTargetsHanaTV.dbo.Genre		gr	with(noLock)	on x.Genre		= gr.GenreCode          \n"
                    + "ORDER BY x.Category, x.Genre \n");


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				dailyProgramHitModel.HeaderDataSet = ds.Copy();

				ds.Dispose();

				// ����ڵ� ��Ʈ
				dailyProgramHitModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + dailyProgramHitModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDailyProgramHit() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                dailyProgramHitModel.ResultCD = "3000";
                dailyProgramHitModel.ResultDesc = "���α׷��� ��ûȽ������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
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