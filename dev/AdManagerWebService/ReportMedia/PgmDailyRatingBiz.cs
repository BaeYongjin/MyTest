// ===============================================================================
//
// PgmDailyRatingBiz.cs
//
// ���α׷��� ��û������ ���� 
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
 * Class Name: PgmDailyRatingBiz
 * �ֿ���  : ���α׷��� ��û������ ó�� ����
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
    /// DailyRatingBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class PgmDailyRatingBiz : BaseBiz
    {

		#region  ������
        public PgmDailyRatingBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region ä�κ���û���ϰ� ��ȸ
        /// <summary>
        /// ä�κ���û���ϰ� ��ȸ
        /// </summary>
        /// <param name="pgmDailyRatingModel"></param>
        public void GetPgmDailyRatingReport(HeaderModel header, PgmDailyRatingModel pgmDailyRatingModel)
        {

            try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDailyRatingList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	:[" + pgmDailyRatingModel.SearchMediaCode + "]");		// �˻� ��ü
				_log.Debug("SearchDateFrom  :[" + pgmDailyRatingModel.SearchDate      + "]");		// �˻� �Ⱓ����           

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
					+ " DECLARE @SumHit int;                            \n"
					+ "                                                 \n"
					+ " SELECT @SumHit = SUM(HitCnt)                    \n"
                    + "   FROM SummaryPg A INNER JOIN AdTargetsHanaTV.dbo.Program   B ON (A.ProgKey    = B.ProgramKey)  \n"
					+ "  WHERE B.MediaCode =  " + pgmDailyRatingModel.SearchMediaCode + " \n"
					+ "    AND A.ProgKey > 0                            \n"
   					+ "    AND A.LogDay    = '" + pgmDailyRatingModel.SearchDate + "' \n"
					+ "                                                 \n"
					+ " SELECT A.LogDay                                 \n" 
					+ "       ,C.CategoryCode                           \n"
					+ "       ,C.CategoryName                           \n"
					+ "       ,D.GenreCode                              \n"
					+ "       ,D.GenreName                              \n"  
					+ "       ,B.Channel AS ChannelNo                   \n"
					+ "       ,B.ProgramNm, A.HitCnt                    \n"
					+ "       ,CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,A.HitCnt) / @SumHit) * 100.0) AS HitRate \n"
					+ "       ,A.Hit00 ,A.Hit01 ,A.Hit02 ,A.Hit03 ,A.Hit04 ,A.Hit05 ,A.Hit06 ,A.Hit07       \n"
					+ "       ,A.Hit08 ,A.Hit09 ,A.Hit10 ,A.Hit11 ,A.Hit12 ,A.Hit13 ,A.Hit14 ,A.Hit15       \n"
					+ "       ,A.Hit16 ,A.Hit17 ,A.Hit18 ,A.Hit19 ,A.Hit20 ,A.Hit21 ,A.Hit22 ,A.Hit23       \n"
                    + "   FROM SummaryPg A INNER JOIN AdTargetsHanaTV.dbo.Program   B ON (A.ProgKey    = B.ProgramKey)          \n"
                    + "                     LEFT JOIN AdTargetsHanaTV.dbo.Category  C ON (A.Category   = C.CategoryCode)        \n"
                    + "                     LEFT JOIN AdTargetsHanaTV.dbo.Genre     D ON (A.Genre      = D.GenreCode)           \n"
					+ "  WHERE B.MediaCode = " + pgmDailyRatingModel.SearchMediaCode + " \n"
					+ "    AND A.ProgKey   > 0                                           \n"
					+ "    AND A.LogDay = '" + pgmDailyRatingModel.SearchDate        + "'\n"
 					+ " ORDER BY LogDay, CategoryCode, GenreCode, ChannelNo              \n"
					);


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// ��� DataSet�� �帣�׷�𵨿� ����
				pgmDailyRatingModel.ReportDataSet = ds.Copy();

				// ���
				pgmDailyRatingModel.ResultCnt = Utility.GetDatasetCount(pgmDailyRatingModel.ReportDataSet);
				// ����ڵ� ��Ʈ
				pgmDailyRatingModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + pgmDailyRatingModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDailyRatingList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                pgmDailyRatingModel.ResultCD = "3000";
                pgmDailyRatingModel.ResultDesc = "�Ϻ���û�� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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