// ===============================================================================
//
// StatisticsPgChannelBiz.cs
//
// ����������Ʈ ä����� ���� 
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
 * Class Name: StatisticsPgChannelBiz
 * �ֿ���  : ����������Ʈ ä����� ó�� ����
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

namespace AdManagerWebService.ReportMedia
{
    /// <summary>
    /// StatisticsPgChannelBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class StatisticsPgChannelBiz : BaseBiz
    {

        #region  ������
        public StatisticsPgChannelBiz()
            : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region ä�� ���
        /// <summary>
        ///  �Ⱓ�� ä�����
        /// </summary>
        /// <param name="statisticsPgChannelModel"></param>
        public void GetStatisticsPgChannel(HeaderModel header, StatisticsPgChannelModel statisticsPgChannelModel)
        {
            bool isNotTarget = false; // Ÿ���������� �Էµ����ʾ� �������� ������.
            bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
            {
                StringBuilder sbQuery = null;

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgChannel() Start");
                _log.Debug("-----------------------------------------");

                // ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
                if (statisticsPgChannelModel.SearchStartDay.Length > 6) statisticsPgChannelModel.SearchStartDay = statisticsPgChannelModel.SearchStartDay.Substring(2, 6);
                if (statisticsPgChannelModel.SearchEndDay.Length > 6) statisticsPgChannelModel.SearchEndDay = statisticsPgChannelModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchMediaCode	  :[" + statisticsPgChannelModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchStartDay    :[" + statisticsPgChannelModel.SearchStartDay + "]");		// �˻� ������� ����          
                _log.Debug("SearchEndDay      :[" + statisticsPgChannelModel.SearchEndDay + "]");		// �˻� �������� ����          
                // __DEBUG__

                string MediaCode = statisticsPgChannelModel.SearchMediaCode;
                string StartDay = statisticsPgChannelModel.SearchStartDay;
                string EndDay = statisticsPgChannelModel.SearchEndDay;

                // ��������
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "/* �Ⱓ�� ������ ä�� ���    */           \n"
                    + "DECLARE @TotPgHit int;    -- ��ü �����������  \n"
                    + "SET @TotPgHit    = 0;                      \n"
                    + "                                           \n"
                    + "-- ��ü ������Hit                          \n"
                    + "SELECT @TotPgHit = ISNULL(SUM(A.HitCnt),0) \n"
                    + "  FROM SummaryPgDaily3 A with(noLock)      \n"
                    + " WHERE A.LogDay BETWEEN '" + StartDay + "' \n"
                    + "                    AND '" + EndDay + "' \n"
                    + "                                           \n"
                    + "-- ä�� ����                               \n"
                    + "	SELECT   ROW_NUMBER() OVER(ORDER BY SUM(A.HitCnt) DESC ) AS Rank \n"
                    + "			,Category \n"
                    + "			,AdTargetsHanaTV.dbo.ufnGetCategoryName(1,Category) as CategoryNm \n"
                    + "			,Genre \n"
                    + "			,AdTargetsHanaTV.dbo.ufnGetGenreName(1,Genre) as GenreNm \n"
                    + "			,ProgKey \n"
                    + "			,( select top 1 b.ProgramNm from AdTargetsHanaTV.dbo.Program b with(noLock) where b.ProgramKey = a.ProgKey ) as ProgramNm \n"
                    + "			,Sum(isnull(HitCnt,0))	as PgCnt \n"
                    + "			,CONVERT(DECIMAL(9,2),(ISNULL(SUM(A.HitCnt),0) / CONVERT(float,@TotPgHit)) * 100)		AS PgRate  \n"
                    + "			,Sum(isnull(PPx,0))		as PPxCnt \n"
                    + "         ,CONVERT(DECIMAL(9,2),(SUM(isnull(PPx,0))) / CONVERT(float,sum(a.HitCnt)) * 100 )	    AS PPxRate  \n"
                    + "			,Sum(isnull(Replay,0))	as RePlayCnt \n"
                    + "         ,CONVERT(DECIMAL(9,2),(SUM(isnull(Replay,0))) / CONVERT(float,sum(a.HitCnt)) * 100 )	AS RePlayRate  \n"
                    + "	FROM	SummaryPgDaily3 A with(noLock) \n"
                    + "	WHERE A.LogDay BETWEEN '" + StartDay + "' AND '" + EndDay + "' \n"
                    + "	group by Category,Genre,ProgKey \n"
                    + "	ORDER BY 1  \n"

//                    + "SELECT TOP 100 ROW_NUMBER() OVER(ORDER BY SUM(A.HitCnt) DESC ) AS Rank   \n"            
                    //                    + "	     ,B.ProgramNm                         \n"
                    //					+ "      ,SUM(A.HitCnt)  AS PgCnt             \n"
                    //                    + "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(A.HitCnt),0) / CONVERT(float,@TotPgHit)) * 100)   \n"
                    //                    + "                               ELSE 0 END AS PgRate                                                                   \n"
                    //                    + "      ,REPLICATE('��', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(A.HitCnt),0)/CONVERT(float,@TotPgHit) * 100),0) \n"
                    //                    + "                                                ELSE 0 END) AS RateBar                                                \n"
                    //                    + " FROM SummaryPgDaily3 A INNER JOIN AdTargetsHanaTV.dbo.Program  B with(NoLock) ON (A.ProgKey  = B.ProgramKey AND B.MediaCode = 1)                      \n"
                    //					+ "   AND A.LogDay BETWEEN '"+ StartDay  + "' AND '"+ EndDay    + "' \n"
                    //                    + " GROUP BY B.ProgramNm                      \n"
                    //                    + "                                           \n"
                    //                    + "ORDER BY Rank, PgCnt                    \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �����͸𵨿� ����
                statisticsPgChannelModel.ReportDataSet = ds.Copy();

                // ���
                statisticsPgChannelModel.ResultCnt = Utility.GetDatasetCount(statisticsPgChannelModel.ReportDataSet);

                ds.Dispose();

                // ����ڵ� ��Ʈ
                statisticsPgChannelModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + statisticsPgChannelModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgChannel() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgChannelModel.ResultCD = "3000";
                if (isNotTarget)
                {
                    statisticsPgChannelModel.ResultDesc = "�ش� �Ⱓ�� ������ �������� �ʽ��ϴ�.";
                }
                else if (isNotReady)
                {
                    statisticsPgChannelModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
                }
                else
                {
                    statisticsPgChannelModel.ResultDesc = "ä����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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