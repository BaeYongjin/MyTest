// ===============================================================================
//
// GenreRatingBiz.cs
//
// �帣�� ��û������ ���� 
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
 * Class Name: GenreRatingBiz
 * �ֿ���  : �帣�� ��û������ ó�� ����
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
    public class GenreRatingBiz : BaseBiz
    {

		#region  ������
        public GenreRatingBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region �帣�� ��û������
        /// <summary>
        /// �帣�� ��û������
        /// </summary>
        /// <param name="genreRating"></param>
        public void GetGenreRating(HeaderModel header, GenreRatingModel genreRating)
        {

            try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreRating() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(genreRating.SearchBgnDay.Length > 6) genreRating.SearchBgnDay = genreRating.SearchBgnDay.Substring(2,6);
				if(genreRating.SearchEndDay.Length > 6) genreRating.SearchEndDay = genreRating.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	:[" + genreRating.SearchMediaCode + "]");		// �˻� ��ü
				_log.Debug("SearchType      :[" + genreRating.SearchType      + "]");		// �˻� ����           
				_log.Debug("SearchBgnDay    :[" + genreRating.SearchBgnDay    + "]");		// �˻� �����������           
				_log.Debug("SearchEndDay    :[" + genreRating.SearchEndDay    + "]");		// �˻� ������������           
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
					+ " -- �帣�� ��û������                                  \n"
					+ "                                                       \n"
					+ "DECLARE @MediaCode int                                 \n"
					+ "DECLARE @SumHit int, @SumHouse int, @SumSetop int      \n"
					+ "DECLARE @BgnDay Char(6), @EndDay Char(6)               \n"
					+ "                                                       \n"
					+ "SET @MediaCode =  " + genreRating.SearchMediaCode + "  \n"
					+ "SET @BgnDay    = '" + genreRating.SearchBgnDay    + "' \n"
					+ "SET @EndDay    = '" + genreRating.SearchEndDay    + "' \n"
					+ "                                                       \n"
					+ "SELECT @SumHit   = SUM(HitCnt)       -- ��ü��ûȽ��   \n"
					+ "      ,@SumHouse = SUM(HitHouseHold) -- ��ü�̿밡���� \n"
                    + "  FROM SummaryPg A INNER JOIN AdTargetsHanaTV.dbo.Program   B ON (A.ProgKey    = B.ProgramKey) \n"
					+ " WHERE B.MediaCode =  @MediaCode                       \n"
					+ "   AND A.ProgKey > 0                                   \n"
					+ "   AND A.LogDay BETWEEN @BgnDay AND @EndDay            \n"
					+ "                                                       \n"
					+ "SELECT @SumSetop = ISNULL(HouseTotal,0)   -- �����ڼ�  \n"  
					+ "  FROM SummaryBase                                     \n"
					+ " WHERE LogDay = (SELECT MAX(LogDay) FROM SummaryBase WHERE LogDay < @BgnDay AND HouseTotal > 0) \n"
					+ "                                                       \n"
					+ "SELECT C.CategoryCode             -- ī�װ��ڵ�      \n"
					+ "       ,C.CategoryName            -- ī�װ���        \n"
					+ "       ,D.GenreCode               -- �帣�ڵ�          \n"
					+ "       ,D.GenreName               -- �帣��            \n"
					+ "       ,CASE WHEN @SumSetop > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitHouseHold)) / @SumSetop) * 100.0) ELSE 0 END AS UseRate   -- �̿��     = �̿밡���� / �����ڼ� * 100        \n"
					+ "       ,CASE WHEN @SumHouse > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitHouseHold)) / @SumHouse) * 100.0) ELSE 0 END AS UseShare  -- �̿������� = �̿밡���� / ��ü�̿밡���� * 100  \n"
					+ "	   ,SUM(A.HitHouseHold) AS HitHouse                                                                                                        -- �̿밡����                                      \n"
					+ "       ,CASE WHEN @SumHit   > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitCnt)) / @SumHit) * 100.0)         ELSE 0 END AS HitShare  -- ��û������ = ��ûȽ�� / ��ü��ûȽ�� * 100      \n"
					+ "       ,SUM(A.HitCnt) AS HitCnt                                                                     -- ��ûȽ��                                                                                \n"
					+ "       ,CASE WHEN SUM(A.HitHouseHold) > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitCnt)) / SUM(A.HitHouseHold)))  ELSE 0 END AS HitFreq       -- ��û��   = ��ûȽ�� / �̿밡����   \n"
                    + "   FROM SummaryPg A INNER JOIN AdTargetsHanaTV.dbo.Program   B ON (A.ProgKey    = B.ProgramKey)    \n"
                    + "                    INNER JOIN AdTargetsHanaTV.dbo.Category  C ON (A.Category   = C.CategoryCode)  \n"
                    + "                    INNER JOIN AdTargetsHanaTV.dbo.Genre     D ON (A.Genre      = D.GenreCode)     \n"
					+ "  WHERE B.MediaCode = @MediaCode                                               \n"
					+ "    AND A.ProgKey   > 0                                                        \n"
					+ "    AND A.LogDay BETWEEN @BgnDay AND @EndDay                                   \n"
					+ " GROUP BY CategoryCode, CategoryName, GenreCode, GenreName                     \n"
					+ " ORDER BY CategoryCode, HitCnt DESC                                            \n"
					);


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// ��� DataSet ����
				genreRating.ReportDataSet = ds.Copy();

				// ���
				genreRating.ResultCnt = Utility.GetDatasetCount(genreRating.ReportDataSet);
				// ����ڵ� ��Ʈ
				genreRating.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + genreRating.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreRating() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                genreRating.ResultCD = "3000";
                genreRating.ResultDesc = "�帣�� ��û������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
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