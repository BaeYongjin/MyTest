// ===============================================================================
//
// CategoryRatingBiz.cs
//
// ī�װ��� ��û������ ���� 
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
 * Class Name: CategoryRatingBiz
 * �ֿ���  : ī�װ��� ��û������ ó�� ����
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
    public class CategoryRatingBiz : BaseBiz
    {

		#region  ������
        public CategoryRatingBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region ī�װ��� ��û������
        /// <summary>
        /// ī�װ��� ��û������
        /// </summary>
        /// <param name="categoryRating"></param>
        public void GetCategoryRating(HeaderModel header, CategoryRatingModel categoryRating)
        {

            try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryRating() Start");
                _log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻��̸� 6�ڸ��� �����.
				if(categoryRating.SearchBgnDay.Length > 6) categoryRating.SearchBgnDay = categoryRating.SearchBgnDay.Substring(2,6);
				if(categoryRating.SearchEndDay.Length > 6) categoryRating.SearchEndDay = categoryRating.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	:[" + categoryRating.SearchMediaCode + "]");		// �˻� ��ü
				_log.Debug("SearchType      :[" + categoryRating.SearchType      + "]");		// �˻� ����           
				_log.Debug("SearchBgnDay    :[" + categoryRating.SearchBgnDay    + "]");		// �˻� �����������           
				_log.Debug("SearchEndDay    :[" + categoryRating.SearchEndDay    + "]");		// �˻� ������������           
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
					+ " -- ī�װ��� ��û������                                \n"
					+ "                                                       \n"
					+ "DECLARE @MediaCode int                                 \n"
					+ "DECLARE @SumHit int, @SumHouse int, @SumSetop int      \n"
					+ "DECLARE @BgnDay Char(6), @EndDay Char(6)               \n"
					+ "                                                       \n"
					+ "SET @MediaCode =  " + categoryRating.SearchMediaCode + "  \n"
					+ "SET @BgnDay    = '" + categoryRating.SearchBgnDay    + "' \n"
					+ "SET @EndDay    = '" + categoryRating.SearchEndDay    + "' \n"
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
					+ "      ,C.CategoryName            -- ī�װ���        \n"
					+ "      ,CASE WHEN @SumSetop > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitHouseHold)) / @SumSetop) * 100.0) ELSE 0 END AS UseRate   -- �̿��     = �̿밡���� / �����ڼ� * 100        \n"
					+ "      ,CASE WHEN @SumHouse > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitHouseHold)) / @SumHouse) * 100.0) ELSE 0 END AS UseShare  -- �̿������� = �̿밡���� / ��ü�̿밡���� * 100  \n"
					+ "	     ,SUM(A.HitHouseHold) AS HitHouse                                                                                                        -- �̿밡����                                      \n"
					+ "      ,CASE WHEN @SumHit   > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitCnt)) / @SumHit) * 100.0)         ELSE 0 END AS HitShare  -- ��û������ = ��ûȽ�� / ��ü��ûȽ�� * 100      \n"
					+ "      ,SUM(A.HitCnt) AS HitCnt                                                                     -- ��ûȽ��                                                                                \n"
					+ "      ,CASE WHEN SUM(A.HitHouseHold) > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitCnt)) / SUM(A.HitHouseHold)))  ELSE 0 END AS HitFreq       -- ��û��   = ��ûȽ�� / �̿밡����   \n"
                    + "  FROM SummaryPg A INNER JOIN AdTargetsHanaTV.dbo.Program   B ON (A.ProgKey    = B.ProgramKey)    \n"
                    + "                   INNER JOIN AdTargetsHanaTV.dbo.Category  C ON (A.Category   = C.CategoryCode)  \n"
					+ " WHERE B.MediaCode = @MediaCode                                               \n"
					+ "   AND A.ProgKey   > 0                                                        \n"
					+ "   AND A.LogDay BETWEEN @BgnDay AND @EndDay                                   \n"
					+ "GROUP BY CategoryCode, CategoryName                                           \n"
					+ "ORDER BY CategoryCode                                                         \n"
					);


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// ��� DataSet ����
				categoryRating.ReportDataSet = ds.Copy();

				// ���
				categoryRating.ResultCnt = Utility.GetDatasetCount(categoryRating.ReportDataSet);
				// ����ڵ� ��Ʈ
				categoryRating.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + categoryRating.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryRating() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                categoryRating.ResultCD = "3000";
                categoryRating.ResultDesc = "ī�װ��� ��û������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
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