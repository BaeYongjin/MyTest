// ===============================================================================
//
// ProgramAdHitBiz.cs
//
// ���α׷� ��ûȽ������ ���� 
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
 * Class Name: ProgramAdHitBiz
 * �ֿ���  : ���α׷� ��ûȽ������ ó�� ����
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
    /// DailyRatingBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class ProgramAdHitBiz : BaseBiz
    {
		#region  ������
        public ProgramAdHitBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region ���α׷��� ��ûȽ������
        /// <summary>
        /// ���α׷��� ��ûȽ������
        /// 2010/02/20�� ���� ��뼮
        /// </summary>
        /// <param name="programAdHitModel"></param>
        public void GetProgramAdHit(HeaderModel header, ProgramAdHitModel programAdHitModel)
        {
            try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetProgramAdHit() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(programAdHitModel.SearchBgnDay.Length > 6) programAdHitModel.SearchBgnDay = programAdHitModel.SearchBgnDay.Substring(2,6);
				if(programAdHitModel.SearchEndDay.Length > 6) programAdHitModel.SearchEndDay = programAdHitModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode   :[" + programAdHitModel.SearchMediaCode   + "]");		// �˻� ��ü
				_log.Debug("SearchContractSeq :[" + programAdHitModel.SearchContractSeq + "]");		// �˻� ����ȣ           
				_log.Debug("SearchItemNo      :[" + programAdHitModel.SearchItemNo      + "]");		// �˻� ����           
				_log.Debug("SearchType        :[" + programAdHitModel.SearchType        + "]");		// �˻� ���� D:���ñⰣ C:���Ⱓ(��������Ⱓ)        
				_log.Debug("SearchBgnDay      :[" + programAdHitModel.SearchBgnDay      + "]");		// �˻� �����������           
				_log.Debug("SearchEndDay      :[" + programAdHitModel.SearchEndDay      + "]");		// �˻� ������������           
                // __DEBUG__
				
				string MediaCode   = programAdHitModel.SearchMediaCode;
				string BgnDay      = programAdHitModel.SearchBgnDay;
				string EndDay      = programAdHitModel.SearchEndDay;

                int     ContractSeq = Convert.ToInt32(programAdHitModel.SearchContractSeq);
                int     CampaignCd  = Convert.ToInt32(programAdHitModel.CampaignCode);
                int     ItemNo      = Convert.ToInt32(programAdHitModel.SearchItemNo);

                // ��������
				sbQuery = new StringBuilder();

				#region ������
//				sbQuery.Append("\n"
//					+ "-- ���α׷��� ��û����                                                           \n"
//					+ "SELECT (SPACE(5 - LEN(CONVERT(VARCHAR(5),ad.CategoryCode)))                      \n"
//					+ "        + CONVERT(VARCHAR(10),ad.CategoryCode) + ' ' + ad.CategoryName)          \n"
//					+ "       AS CategoryCode     -- ī�װ��ڵ�                                       \n"
//					+ "      ,ad.CategoryName     -- ī�װ���                                         \n"
//					+ "      ,(SPACE(5 - LEN(CONVERT(VARCHAR(5),ad.GenreCode)))                         \n"
//					+ "        + CONVERT(VARCHAR(10),ad.GenreCode) + ' ' + ad.GenreName)                \n"
//					+ "       AS GenreCode        -- �帣�ڵ�                                           \n"
//					+ "      ,ad.GenreName        -- �帣��                                             \n"
//					+ "      ,ad.ChannelNo        -- ä�ι�ȣ                                           \n"
//					+ "      ,ad.ProgramNm        -- ���α׷���                                         \n"
//					+ "      ,ad.AdHitCnt         -- �����û��                                         \n"
//					+ "      ,ad.PgHitCnt         -- ���α׷���û��                                     \n"
//					+ "  FROM                                                                           \n"
//					+ "  ( SELECT C.CategoryCode                                                        \n"
//					+ "          ,C.CategoryName                                                        \n"
//					+ "          ,D.GenreCode                                                           \n"
//					+ "          ,D.GenreName                                                           \n"
//					+ "          ,B.Channel AS ChannelNo                                                \n"
//					+ "          ,B.ProgramNm                                                           \n"
//					+ "          ,A.ProgKey                                                             \n"
//					//			�ʱ⿣ ��������,��ະ�� ���踦 ������, ���Ŀ� ķ���κ� ���谡 �߰���
//					//			ķ������ ���߱�����������, ����Ǽ���ŭ ������� ��Ȯ�� ���� ����, Ȥ�� min,max
//					+ "          ,sum(A.AdCnt)  AS AdHitCnt                                             \n"
//					+ "          ,min(A.HitCnt) AS PgHitCnt                                             \n"
//					+ "	     FROM SummaryAdDaily3		A with(NoLock)                                        \n"
//					+ "           INNER JOIN AdTargetsHanaTV.dbo.Program	B with(NoLock) ON (A.ProgKey  = B.ProgramKey)    \n"
//					+ "	          INNER JOIN AdTargetsHanaTV.dbo.Category	C with(NoLock) ON (A.Category = C.CategoryCode)  \n"
//					+ "           INNER JOIN AdTargetsHanaTV.dbo.Genre		D with(NoLock) ON (A.Genre    = D.GenreCode)     \n"
//					+ "	    WHERE B.MediaCode = " + MediaCode              + "                          \n"
//					+ "       AND A.LogDay BETWEEN '" + BgnDay + "' AND '" + EndDay + "'                \n"	);
				#endregion

                    sbQuery.Append(" SELECT (SPACE(5 - LEN(CONVERT(VARCHAR(5),ad.CategoryCode))) + CONVERT(VARCHAR(10),ad.CategoryCode) + ' ' + ad.CategoryName) AS CategoryCode \n");
                    sbQuery.Append("       ,ad.CategoryName \n");
                    sbQuery.Append("       ,(SPACE(5 - LEN(CONVERT(VARCHAR(5),ad.GenreCode))) + CONVERT(VARCHAR(10),ad.GenreCode) + ' ' + ad.GenreName) AS GenreCode \n");
                    sbQuery.Append("       ,ad.GenreName \n");
                    sbQuery.Append("       ,ad.ChannelNo \n");
                    sbQuery.Append("       ,ad.ProgramNm \n");
                    sbQuery.Append(" 			,sum(AdHitCnt)  AS AdHitCnt \n");
                    sbQuery.Append(" 			,sum(PgHitCnt) AS PgHitCnt \n");
                    sbQuery.Append(" FROM (	SELECT \n");
                    sbQuery.Append("             C.CategoryCode \n");
                    sbQuery.Append("            ,C.CategoryName \n");
                    sbQuery.Append(" 			,D.GenreCode \n");
                    sbQuery.Append(" 			,D.GenreName \n");
                    sbQuery.Append(" 			,B.Channel	AS ChannelNo \n");
                    sbQuery.Append(" 			,B.ProgramNm \n");
                    sbQuery.Append(" 			,A.ProgKey \n");
                    sbQuery.Append(" 			,A.LogDay \n");
                    sbQuery.Append(" 			,sum(A.AdCnt)  AS AdHitCnt \n");
                    sbQuery.Append("        	,min(A.HitCnt) AS PgHitCnt \n");
                    sbQuery.Append(" 		FROM SummaryAdDaily3	A with(NoLock) \n");
                    sbQuery.Append("        INNER JOIN AdTargetsHanaTV.dbo.Program	    B with(NoLock) ON (A.ProgKey  = B.ProgramKey) \n");
                    sbQuery.Append(" 	    INNER JOIN AdTargetsHanaTV.dbo.Category	    C with(NoLock) ON (A.Category = C.CategoryCode) \n");
                    sbQuery.Append("        INNER JOIN AdTargetsHanaTV.dbo.Genre        D with(NoLock) ON (A.Genre    = D.GenreCode) \n");
                    sbQuery.Append(" 		WHERE	B.MediaCode = " + MediaCode              + "                          \n");
                    sbQuery.Append(" 		AND		A.LogDay BETWEEN '" + BgnDay + "' AND '" + EndDay + "'                \n");

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

                    sbQuery.Append(" 				AND		A.ProgKey   > 0 \n");
                    sbQuery.Append(" 				GROUP BY CategoryCode, CategoryName, GenreCode, GenreName,Channel, ProgramNm ,ProgKey,LogDay ) AD \n");
                    sbQuery.Append(" GROUP BY CategoryCode, CategoryName, GenreCode, GenreName,ChannelNo, ProgramNm ,ProgKey \n");
                    sbQuery.Append(" ORDER BY ad.CategoryCode, ad.GenreCode \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// ��� DataSet�� �帣�׷�𵨿� ����
				programAdHitModel.ReportDataSet = ds.Copy();

				// ���
				programAdHitModel.ResultCnt = Utility.GetDatasetCount(programAdHitModel.ReportDataSet);
				// ����ڵ� ��Ʈ
				programAdHitModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + programAdHitModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetProgramAdHit() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                programAdHitModel.ResultCD = "3000";
                programAdHitModel.ResultDesc = "���α׷��� ��ûȽ������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
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