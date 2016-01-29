// ===============================================================================
//
// DailyAdHitBiz.cs
//
// �Ϻ� ���� ��ûȽ������ ���� 
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
 * Class Name: DailyAdHitBiz
 * �ֿ���  : �Ϻ� ���� ��ûȽ������ ó�� ����
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
    /// DailyAdHitBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class DailyAdHitBiz : BaseBiz
    {

		#region  ������
        public DailyAdHitBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region �帣�� ��ûȽ������
        /// <summary>
        /// �帣�� ��ûȽ������
        /// </summary>
        /// <param name="dailyAdHitModel"></param>
        public void GetDailyAdHit(HeaderModel header, DailyAdHitModel dailyAdHitModel)
        {
            /*
             * 
             */
            try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDailyAdHit() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(dailyAdHitModel.SearchBgnDay.Length > 6) dailyAdHitModel.SearchBgnDay = dailyAdHitModel.SearchBgnDay.Substring(2,6);
				if(dailyAdHitModel.SearchEndDay.Length > 6) dailyAdHitModel.SearchEndDay = dailyAdHitModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	  :[" + dailyAdHitModel.SearchMediaCode   + "]");		// �˻� ��ü
				_log.Debug("SearchContractSeq :[" + dailyAdHitModel.SearchContractSeq + "]");		// �˻� ����ȣ           
				_log.Debug("SearchItemNo      :[" + dailyAdHitModel.SearchItemNo      + "]");		// �˻� �����ȣ           
				_log.Debug("SearchType        :[" + dailyAdHitModel.SearchType        + "]");		// �˻� ���� D:���ñⰣ C:���Ⱓ(��������Ⱓ)        
				_log.Debug("SearchBgnDay      :[" + dailyAdHitModel.SearchBgnDay      + "]");		// �˻� �����������           
				_log.Debug("SearchEndDay      :[" + dailyAdHitModel.SearchEndDay      + "]");		// �˻� ������������           
                // __DEBUG__

				string MediaCode    = dailyAdHitModel.SearchMediaCode;
				int     ContractSeq = Convert.ToInt32(dailyAdHitModel.SearchContractSeq);
                int     CampaignCd  = Convert.ToInt32(dailyAdHitModel.CampaignCode);
				int     ItemNo      = Convert.ToInt32(dailyAdHitModel.SearchItemNo);

				string BgnDay      = dailyAdHitModel.SearchBgnDay;
				string EndDay      = dailyAdHitModel.SearchEndDay;


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
                    + "					,sum(AdCnt)	as AdCnt" + "\n"
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

                #region ������                
//				sbQuery.Append("\n"
//					+ "-- �Ϻ� �帣�� �����ûȽ��                                                                  \n"
//					+ "SELECT T.LogDay, T.Category,  ct.CategoryName, T.Genre, gr.GenreName                     \n"
//					+ "      ,ISNULL(pg.AdCnt,0) AS HitCnt                                                     \n"
//					+ " FROM                                                                                    \n"
//					+ "    (                                                                                    \n"
//					+ "      SELECT bs.LogDay, ad.Category,ad.Genre, ad.ItemNo                                  \n"
//					+ "        FROM SummaryBase bs  with(NoLock) ,                                              \n"
//					+ "             ( SELECT A.Genre, A.ItemNo, B.UpperMenuCode AS Category,  SUM(A.AdCnt) AS AdCnt       \n"
                //					+ "                 FROM SummaryAdDaily2 A  with(NoLock) INNER JOIN AdTargetsHanaTV.dbo.MENU B                  \n"
//					+ "                                                ON (A.Genre   = B.MenuCode               \n"
//					+ "                                                AND B.MenuLevel =2                       \n" 
//					+ "                                                AND B.MediaCode = " + MediaCode + ")     \n"
//					+ "                INNER JOIN AdTargetsHanaTV.dbo.ContractItem     C with(NoLock) ON (A.itemno    = C.itemno)   \n"
//					+ "                INNER JOIN AdTargetsHanaTV.dbo.CampaignMaster     D with(NoLock) ON (C.ContractSeq    = D.ContractSeq)		\n"
//					+ "                INNER JOIN AdTargetsHanaTV.dbo.CampaignDetail     E with(NoLock) ON (D.CampaignCode = E.CampaignCode AND A.Itemno    = E.Itemno)     \n"
//					+ "                WHERE A.LogDay BETWEEN '" + BgnDay + "' \n"
//					+ "                                   AND '" + EndDay + "' \n"
//					);
//				if(!dailyAdHitModel.CampaignCode.Equals("00"))
//				{
//					sbQuery.Append("  AND E.CampaignCode = '"+dailyAdHitModel.CampaignCode+"' \n");
//				}    
//				// �����ȸ���� ����������ȸ����
//				if(Type.Equals("I") && !ItemNo.Equals("01"))
//				{
//					sbQuery.Append("       AND A.ItemNo       = " + ItemNo + " \n");
//				}
//				//if(Type.Equals("I") && ItemNo.Equals("01") && !dailyAdHitModel.CampaignCode.Equals("00"))
//				//{
//				//	sbQuery.Append("       AND A.ItemNo IN (Select ItemNo From AdTargetsHanaTV.dbo.CampaignDetail Where CampaignCode = '"+dailyAdHitModel.CampaignCode+"') \n");
//				//}
//				//if(Type.Equals("I") && ItemNo.Equals("01") && dailyAdHitModel.CampaignCode.Equals("00"))
//				//{
//				//	sbQuery.Append("       AND A.ItemNo IN (Select B.ItemNo From AdTargetsHanaTV.dbo.CampaignMaster A with(NoLock) INNER JOIN AdTargetsHanaTV.dbo.CampaignDetail B with(NoLock) ON (A.CampaignCode = B.CampaignCode)Where A.Contractseq = '"+dailyAdHitModel.CampaignCode+"') \n");
//				//}
//				if(Type.Equals("C"))
//				{
//					sbQuery.Append("       AND A.ContractSeq  = " + ContractSeq + " \n");
//				}
//				sbQuery.Append(""
//					+ "                GROUP BY A.Genre, A.ItemNo, B.UpperMenuCode                                        \n"
//					+ "             ) ad                                                                        \n"
//					+ "        WHERE bs.LogDay BETWEEN '" + BgnDay + "' \n"
//					+ "                            AND '" + EndDay + "' \n"
//					+ "    ) T LEFT  JOIN ( SELECT A.LogDay, A.Genre, SUM(A.AdCnt) AS AdCnt                   \n"
//					+ "                       FROM SummaryAdDaily2 A  with(NoLock)                            \n"
//	                + "       INNER JOIN AdTargetsHanaTV.dbo.ContractItem     B with(NoLock) ON (A.itemno    = B.itemno)          \n"
//                	+ "       INNER JOIN AdTargetsHanaTV.dbo.CampaignMaster     C with(NoLock) ON (B.ContractSeq    = C.ContractSeq)   \n"
//                  + "       INNER JOIN AdTargetsHanaTV.dbo.CampaignDetail     D with(NoLock) ON (C.CampaignCode = D.CampaignCode AND A.Itemno    = D.Itemno)     \n"
//					+ "                      WHERE A.LogDay BETWEEN '" + BgnDay + "' \n"
//					+ "                                         AND '" + EndDay + "' \n"
//					);
//				//if(!dailyAdHitModel.CampaignCode.Equals("00"))
//				//{
//				//	sbQuery.Append("  AND D.CampaignCode = '"+dailyAdHitModel.CampaignCode+"' \n");
//				//}    
//				// �����ȸ���� ����������ȸ����
//				if(Type.Equals("I") && !ItemNo.Equals("01"))
//				{
//					sbQuery.Append("       AND A.ItemNo       = " + ItemNo + " \n");
//				}
//				//if(Type.Equals("I") && ItemNo.Equals("01") && !dailyAdHitModel.CampaignCode.Equals("00"))
//				//{
//				//	sbQuery.Append("       AND A.ItemNo IN (Select ItemNo From AdTargetsHanaTV.dbo.CampaignDetail Where CampaignCode = '"+dailyAdHitModel.CampaignCode+"') \n");
//				//}
////				if(Type.Equals("I") && ItemNo.Equals("01") && dailyAdHitModel.CampaignCode.Equals("00"))
////				{
////					sbQuery.Append("       AND A.ItemNo IN (Select B.ItemNo From AdTargetsHanaTV.dbo.CampaignMaster A with(NoLock) INNER JOIN AdTargetsHanaTV.dbo.CampaignDetail B with(NoLock) ON (A.CampaignCode = B.CampaignCode)Where A.Contractseq = '"+dailyAdHitModel.CampaignCode+"') \n");
////				}
//				if(Type.Equals("C"))
//				{
//					sbQuery.Append("       AND A.ContractSeq  = " + ContractSeq + " \n");
//				}
//				sbQuery.Append(""
//					+ "                      GROUP BY A.LogDay, A.ItemNo, A.Genre                                         \n"
//					+ "                   ) pg ON (T.LogDay = pg.LogDay AND T.Genre = pg.Genre)                 \n"
//					+ "        INNER JOIN AdTargetsHanaTV.dbo.Category  ct  with(NoLock) ON (T.Category   = ct.CategoryCode)                      \n"
//					+ "        INNER JOIN AdTargetsHanaTV.dbo.Genre     gr  with(NoLock) ON (T.Genre      = gr.GenreCode)                         \n"
//					+ "        INNER JOIN AdTargetsHanaTV.dbo.ContractItem     ci with(NoLock) ON (T.itemno    = ci.itemno)                         \n"
////					+ "        INNER JOIN AdTargetsHanaTV.dbo.CampaignMaster     cm with(NoLock) ON (ci.ContractSeq    = cm.ContractSeq)            \n"
////					+ "        INNER JOIN AdTargetsHanaTV.dbo.CampaignDetail     cd with(NoLock) ON (cm.CampaignCode = cd.CampaignCode AND T.Itemno    = cd.Itemno)     \n"
////					+ "        WHERE cm.CampaignCode       = '"+dailyAdHitModel.CampaignCode+"'						            \n"
//					+ " ORDER BY T.LogDay, T.Category, T.Genre                                                  \n"
//					);
                #endregion

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				dailyAdHitModel.ReportDataSet = ds.Copy();

				// ���
				dailyAdHitModel.ResultCnt = Utility.GetDatasetCount(dailyAdHitModel.ReportDataSet);

				ds.Dispose();

				// ��������
				sbQuery = new StringBuilder(1024);
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
				dailyAdHitModel.HeaderDataSet = ds.Copy();

				ds.Dispose();

				// ����ڵ� ��Ʈ
				dailyAdHitModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + dailyAdHitModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDailyAdHit() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                dailyAdHitModel.ResultCD = "3000";
                dailyAdHitModel.ResultDesc = "���α׷��� ��ûȽ������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
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