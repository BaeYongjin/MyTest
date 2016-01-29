// ===============================================================================
//
// SummaryAdBiz.cs
//
// ���� �Ѱ����� ���� 
//
// ===============================================================================
// Release history
// 2007.10.26 RH.Jung OAP�� ���谡����� �� ��������̿� �޼ҵ� => GetContractItemList()
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: SummaryAdBiz
 * �ֿ���  : ���� �Ѱ����� ó�� ����
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
    /// �Ѱ� ���� ����
    /// </summary>
    public class SummaryAdBiz : BaseBiz
    {

        #region  ������
        public SummaryAdBiz()
            : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region �����࿡ �ش��ϴ� ��������ȸ
        /// <summary>
        /// �����������ȸ
        /// </summary>
        /// <param name="summaryAdModel"></param>
        public void GetContractItemList(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("_SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");
                _log.Debug("_CampaignCode :[" + summaryAdModel.CampaignCode + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                //				sbQuery.Append("\n"
                //					+ "SELECT a.ItemNo 	        \n"   
                //					+ "      ,a.ItemName	    \n"     
                //					+ "      ,a.ExcuteStartDay  \n"
                //					+ "      ,a.RealEndDay      \n"
                //					+ "  FROM AdTargetsHanaTV.dbo.ContractItem a with(NoLock)  \n"
                //					+ "  LEFT JOIN AdTargetsHanaTV.dbo.CampaignMaster     B with(NoLock) ON (A.ContractSeq    = B.ContractSeq) \n"
                //					+ "  LEFT JOIN AdTargetsHanaTV.dbo.CampaignDetail     C with(NoLock) ON (B.CampaignCode = C.CampaignCode AND A.Itemno    = C.Itemno) \n"
                //					+ " WHERE a.ContractSeq = " + summaryAdModel.SearchContractSeq + "\n"										
                //					+ "--   AND a.AdType BETWEEN '10'  AND '19'  \n"
                //					);
                //					if(!summaryAdModel.CampaignCode.Equals("00"))
                //					{
                //						sbQuery.Append("  AND C.CampaignCode = '"+summaryAdModel.CampaignCode+"' \n");
                //					}    
                //					sbQuery.Append(""
                //					+ " ORDER BY a.ItemName     \n"
                //					);
                sbQuery.Append("\n");
                sbQuery.Append(" SELECT a.ItemNo         \n");
                sbQuery.Append("       ,a.ItemName       \n");
                sbQuery.Append("       ,a.ExcuteStartDay \n");
                sbQuery.Append("       ,a.RealEndDay     \n");
                sbQuery.Append(" FROM   AdTargetsHanaTV.dbo.ContractItem a with(noLock)  \n");
                sbQuery.Append(" WHERE  a.ContractSeq = " + summaryAdModel.SearchContractSeq + "\n");

                if (!summaryAdModel.CampaignCode.Equals("00"))
                {
                    sbQuery.Append(" AND a.ItemNo in(select distinct ItemNo     \n");
                    sbQuery.Append("                 from   AdTargetsHanaTV.dbo.CampaignMaster m with(noLock)   \n");
                    sbQuery.Append("		         inner join AdTargetsHanaTV.dbo.CampaignDetail d with(noLock) on m.CampaignCode = d.CampaignCode    \n");
                    sbQuery.Append("				 where  m.ContractSeq = a.ContractSeq   \n");
                    sbQuery.Append("				 and    m.CampaignCode = '" + summaryAdModel.CampaignCode + "') \n");
                }
                sbQuery.Append(" ORDER BY a.ItemName");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �������𵨿� ����
                summaryAdModel.ItemDataSet = ds.Copy();
                // ���
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ItemDataSet);
                // ����ڵ� ��Ʈ
                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                summaryAdModel.ResultDesc = "���������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        #endregion

        #region �ѱⰣ �������� ����
        /// <summary>
        ///  �ѱⰣ �������� ����
        /// </summary>
        /// <param name="summaryAdModel"></param>
        public void GetSummaryAdTotality(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
            {
                StringBuilder sbQuery = null;

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdDaily() Start");
                _log.Debug("-----------------------------------------");

                // ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
                if (summaryAdModel.SearchStartDay.Length > 6) summaryAdModel.SearchStartDay = summaryAdModel.SearchStartDay.Substring(2, 6);
                if (summaryAdModel.SearchEndDay.Length > 6) summaryAdModel.SearchEndDay = summaryAdModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchMediaCode	  :[" + summaryAdModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");		// �˻� �����ȣ           
                _log.Debug("SearchItemNo      :[" + summaryAdModel.SearchItemNo + "]");		// �˻� �����ȣ           
                _log.Debug("SearchStartDay    :[" + summaryAdModel.SearchStartDay + "]");		// �˻� ������� ����          
                _log.Debug("SearchEndDay      :[" + summaryAdModel.SearchEndDay + "]");		// �˻� �������� ����          
                // __DEBUG__

                string MediaCode = summaryAdModel.SearchMediaCode;
                string ContractSeq = summaryAdModel.SearchContractSeq;
                string ItemNo = summaryAdModel.SearchItemNo;
                string StartDay = summaryAdModel.SearchStartDay;
                string EndDay = summaryAdModel.SearchEndDay;

                string Type = "C"; // ���ǹ�

                // �����ȣ�� ������ �������Ǻ� ��ȸ�̴�.
                if (!ItemNo.Equals("") && !ItemNo.Equals("00"))
                {
                    Type = "I";
                }

                // ��������
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "-- ��ü �̿밡����             \n"
                    + "SELECT ISNULL(HouseTotal,0) AS TotUsr \n"
                    + "  FROM SummaryBase  with(NoLock)   \n"
                    + " WHERE LogDay = '" + StartDay + "' \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count == 0)
                {
                    isNotReady = true;
                    throw new Exception();
                }

                int TotUsr = Convert.ToInt32(ds.Tables[0].Rows[0]["TotUsr"].ToString());
                ds.Dispose();

                // �����ϱ��� �̿��ڼ�
                summaryAdModel.TotalUser = TotUsr;

                // ��������
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "/* �ѱⰣ �������� ����                                     \n"
                    + "   ��ȸ���� : ��ü�ڵ�, ����ȣ �Ǵ� �����ȣ */        \n"
                    + "                                                    \n"
                    + "DECLARE @MediaCode int;   -- ��ü�ڵ�                  \n"
                    + "DECLARE @LogDay CHAR(6);  -- ��������                  \n"
                    + "DECLARE @ItemNo int;      -- �����ȣ                  \n"
                    + "DECLARE @ContractSeq int; -- ����ȣ                    \n"
                    + "DECLARE @TotUsr int;      -- ��ü �̿밡����              \n"
                    + "DECLARE @TotAdHit int;    -- ��ü ��������              \n"
                    + "DECLARE @TotPgHit int;    -- ��ü ���α׷���û��           \n"
                    + "                                          \n"
                    + "SET @MediaCode   =  " + MediaCode + "  \n"
                    + "SET @LogDay      = '" + StartDay + "' \n"
                    + "SET @ItemNo      =  " + ItemNo + "  \n"
                    + "SET @ContractSeq =  " + ContractSeq + "  \n"
                    + "SET @TotUsr      =  " + TotUsr.ToString() + " \n"
                    + "SET @TotAdHit    = 0;                                                         \n"
                    + "SET @TotPgHit    = 0;                                                         \n"
                    + "                                                                              \n"
                    + "                                                                              \n"
                    + "-- ��ü ����Hit                                                               \n"
                    + "SELECT @TotAdHit = SUM(A.AdCnt)                                               \n"
                    + "      ,@TotPgHit = SUM(A.HitCnt)                                              \n"
                    + "  FROM SummaryAdTotality0 A  with(NoLock)                                      \n"
                    + " WHERE 1 = 1                                                                 \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo     = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                                                                                               \n"
                    + "                                                                                                                        \n"
                    + "-- ��ǰ�� �հ�                                                                                                          \n"
                    + "SELECT  1 AS TypeCode                                                                                                   \n"
                    + "       ,'1 ��ǰ' AS TypeName                                                                                                   \n"
                    + "       ,'��ǰ' AS SumType                                                                                               \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                                                     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP                                      \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdTotality0     A with(NoLock)                                                                            \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)      \n"
                    + " WHERE 1 = 1                                                                                                           \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:��ǰ��     \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName    \n"
                    + "                                          \n"
                    + "UNION ALL                                 \n"
                    + "                                          \n"
                    + "-- ��ü �հ�                              \n"
                    + "SELECT  2 AS TypeCode                     \n"
                    + "       ,'2 �հ�' AS TypeName              \n"
                    + "       ,'�հ�' AS SumType                 \n"
                    + "       ,0 AS SumCode                      \n"
                    + "       ,'' AS SumName                     \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt             \n"
                    + "       ,null AS AdRate                    \n"
                    + "       ,null AS HsCnt                     \n"
                    + "       ,null AS Reach                     \n"
                    + "       ,null AS Freq                      \n"
                    + "       ,null AS GRP                       \n"
                    + "       ,null AS PgCnt                     \n"
                    + "       ,null AS HitCnt                    \n"
                    + "       ,null AS PgRate                    \n"
                    + "  FROM SummaryAdTotality0     A with(NoLock)  \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE 1 = 1                                                                                          \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo     = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:��ǰ��                                                          \n"
                    + "                                                                                               \n"
                    + "UNION ALL                                                                                      \n"
                    + "                                                                                               \n"
                    + "-- ī�װ��� ����                                                                             \n"
                    + "SELECT  3 AS TypeCode                                                                          \n"
                    + "       ,'3 �ε�������' AS TypeName                                                           \n"
                    + "       ,'�ε�������' AS SumType                                                              \n"
                    + "       ,B.CategoryCode AS SumCode                                                              \n"
                    + "       ,B.CategoryName AS SumName                                                              \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                  \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate        \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                            \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq       \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP             \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                  \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate       \n"
                    + "  FROM SummaryAdTotality1  A with(NoLock)                                                      \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.Category B with(NoLock) ON (A.Category = B.CategoryCode) \n"
                    + " WHERE 1 = 1                                                                                   \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + " GROUP BY B.CategoryCode, B.CategoryName                                                   \n"
                    + "                                                                                           \n"
                    + "UNION ALL                                                                                  \n"
                    + "                                                                                           \n"
                    + "-- �ʼ��� ����                                                                             \n"
                    + "SELECT  4 AS TypeCode                                                                      \n"
                    + "       ,'4 �ʼ�' AS TypeName                                                               \n"
                    + "       ,'�ʼ�' AS SumType                                                                  \n"
                    + "       ,B.AdTime AS SumCode                                                                \n"
                    + "       ,Convert(VARCHAR(5),B.AdTime) + '��' AS SumName                                     \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate    \n"
                    + "       ,null AS HsCnt                                                                      \n"
                    + "       ,null AS Reach                                                                      \n"
                    + "       ,null AS Freq                                                                       \n"
                    + "       ,null AS GRP                                                                        \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                              \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                            \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate   \n"
                    + "  FROM SummaryAdTotality0      A with(NoLock)                                              \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo) \n"
                    + " WHERE 1 = 1                                                                               \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:��ǰ��                                                     \n"
                    + " GROUP BY B.AdTime                                                                        \n"
                    + "                                                                                          \n"
                    + "UNION ALL                                                                                 \n"
                    + "                                                                                          \n"
                    + "-- ���Ϻ� ����                                                                            \n"
                    + "SELECT  5 AS TypeCode                                                                     \n"
                    + "       ,'5 ����' AS TypeName                                                              \n"
                    + "       ,'����' AS SumType                                                                 \n"
                    + "       ,B.SummaryCode AS SumCode                                                          \n"
                    + "       ,B.SummaryName AS SumName                                                          \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                             \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate   \n"
                    + "       ,null AS HsCnt                                                                     \n"
                    + "       ,null AS Reach                                                                     \n"
                    + "       ,null AS Freq                                                                      \n"
                    + "       ,null AS GRP                                                                       \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                             \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate  \n"
                    + "  FROM SummaryAdTotality0     A with(NoLock)                                              \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)   \n"
                    + " WHERE 1 = 1                                                                              \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo     = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 6   -- 6:���Ϻ�                                                      \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                       \n"
                    + "                                                                                             \n"
                    + "UNION ALL                                                                                    \n"
                    + "                                                                                             \n"
                    + "-- ���ɺ� ����                                                                               \n"
                    + "SELECT  6 AS TypeCode                                                                        \n"
                    + "       ,'6 ���ɺ�' AS TypeName                                                               \n"
                    + "       ,'���ɺ�' AS SumType                                                                  \n"
                    + "       ,B.SummaryCode AS SumCode                                                             \n"
                    + "       ,B.SummaryName AS SumName                                                             \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate      \n"
                    + "       ,null AS HsCnt                                                                        \n"
                    + "       ,null AS Reach                                                                        \n"
                    + "       ,null AS Freq                                                                         \n"
                    + "       ,null AS GRP                                                                          \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate     \n"
                    + "  FROM SummaryAdTotality0     A with(NoLock)                                                 \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE 1 = 1                                                                                 \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo     = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 3   -- 3:���ɺ�                                                                                 \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION ALL                                                                                                              \n"
                    + "                                                                                                                        \n"
                    + "-- ������ ����                                                                                                          \n"
                    + "SELECT  7 AS TypeCode                                                                                                   \n"
                    + "       ,'7 ������' AS TypeName                                                                                                   \n"
                    + "       ,'������' AS SumType                                                                                             \n"
                    + "       ,TA.SummaryCode AS SumCode                                                                                       \n"
                    + "       ,TA.SummaryName AS SumName                                                                                       \n"
                    + "       ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt                                                                                \n"
                    + "       ,ISNULL(CONVERT(DECIMAL(9,2),(SUM(TB.AdCnt) / CONVERT(float,@TotAdHit)) * 100),0) AS AdRate                      \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,MAX(TB.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(TB.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(TB.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM (SELECT SummaryCode, SummaryName FROM SummaryCode with(NoLock)  WHERE SummaryType = 5 AND Level = 1) TA        \n"
                    + "       LEFT JOIN                                                                                                        \n"
                    + "       (                                                                                                                \n"
                    + "        SELECT CASE B.Level WHEN 1 THEN B.SummaryCode                                                                   \n"
                    + "                            ELSE B.UpperCode END AS SummaryCode                                                         \n"
                    + "              ,A.AdCnt                                                                                                  \n"
                    + "              ,A.HitCnt                                                                                                  \n"
                    + "              ,A.PgCnt                                                                                                  \n"
                    + "              ,B.Level                                                                                                  \n"
                    + "          FROM SummaryAdTotality0 A with(NoLock)                                                                        \n"
                    + "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode) \n"
                    + "          WHERE 1 = 1                                                                                          \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("           AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("           AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "           AND A.SummaryType  = 5  -- 5:������                                                                          \n"
                    + "       ) TB                                                                                                             \n"
                    + "       ON (TA.SummaryCode = TB.SummaryCode)                                                                             \n"
                    + " GROUP BY TA.SummaryCode, TA.SummaryName                                                                                \n"
                    + "                                                                                                                        \n"
                    + "ORDER BY TypeCode, SumCode                                                                                              \n"
                    );


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                ds = new DataSet();
                _db.Timeout = 600;
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �����͸𵨿� ����
                summaryAdModel.ReportDataSet = ds.Copy();

                // ���
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ReportDataSet);

                ds.Dispose();

                // ����ڵ� ��Ʈ
                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdTotality() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                if (isNotReady)
                {
                    summaryAdModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
                }
                else
                {
                    summaryAdModel.ResultDesc = "�ѱⰣ �������� ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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

        #region �ϰ� �������� ����

        /// <summary>
        ///  �ϰ� �������� ����
        /// </summary>
        /// <param name="summaryAdModel"></param>
        public void GetSummaryAdDaily(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
            {
                StringBuilder sb = new StringBuilder();
                DataSet ds = new DataSet();

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdDaily() Start");
                _log.Debug("-----------------------------------------");

                // ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
                if (summaryAdModel.SearchStartDay.Length > 6) summaryAdModel.SearchStartDay = summaryAdModel.SearchStartDay.Substring(2, 6);
                if (summaryAdModel.SearchEndDay.Length > 6) summaryAdModel.SearchEndDay = summaryAdModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchMediaCode	  :[" + summaryAdModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");		// �˻� �����ȣ
                _log.Debug("SearchCampaign    :[" + summaryAdModel.CampaignCode + "]");		// �˻� �����ȣ
                _log.Debug("SearchItemNo      :[" + summaryAdModel.SearchItemNo + "]");		// �˻� �����ȣ           
                _log.Debug("SearchStartDay    :[" + summaryAdModel.SearchStartDay + "]");		// �˻� ������� ����          
                _log.Debug("SearchEndDay      :[" + summaryAdModel.SearchEndDay + "]");		// �˻� �������� ����          
                // __DEBUG__

                #region �������� ������ ���Կ��� ýũ
                bool IsProfile = false;
                if (summaryAdModel.TotalUser == 1)
                {
                    IsProfile = true;
                }
                #endregion

                #region ��ž�� ��������
                sb.Append("\n " + "select   top 1 isnull(HouseTotal,0) as TotUsr");
                sb.Append("\n " + "from     SummaryBase where logday <= '" + summaryAdModel.SearchEndDay + "'");
                sb.Append("\n " + "order by logday desc");

                _db.ExecuteQuery(ds, sb.ToString());

                if (ds.Tables[0].Rows.Count == 0)
                {
                    isNotReady = true;
                    throw new Exception();
                }

                int TotUsr = Convert.ToInt32(ds.Tables[0].Rows[0]["TotUsr"].ToString());
                // �����ϱ��� �̿��ڼ�
                summaryAdModel.TotalUser = TotUsr;

                ds.Dispose();
                #endregion

                // ��������
                ds = new DataSet();

                #region [ �Ķ���� ���� ]
                SqlParameter[] sqlParam = new SqlParameter[8];
                sqlParam[0] = new SqlParameter("@MenuLevel", SqlDbType.Int, 4);
                sqlParam[1] = new SqlParameter("@MediaCode", SqlDbType.Int, 4);
                sqlParam[2] = new SqlParameter("@LogDay", SqlDbType.Char, 6);
                sqlParam[3] = new SqlParameter("@BeginDay", SqlDbType.Char, 6);
                sqlParam[4] = new SqlParameter("@EndDay", SqlDbType.Char, 6);
                sqlParam[5] = new SqlParameter("@ItemNo", SqlDbType.Int, 4);
                sqlParam[6] = new SqlParameter("@ContractSeq", SqlDbType.Int, 4);
                sqlParam[7] = new SqlParameter("@CampaignCd", SqlDbType.Int, 4);

                sqlParam[0].Value = summaryAdModel.MenuLevel;
                sqlParam[1].Value = summaryAdModel.SearchMediaCode;
                sqlParam[2].Value = summaryAdModel.SearchStartDay;
                sqlParam[3].Value = summaryAdModel.SearchStartDay;
                sqlParam[4].Value = summaryAdModel.SearchEndDay;
                sqlParam[5].Value = Convert.ToInt32(summaryAdModel.SearchItemNo);
                sqlParam[6].Value = Convert.ToInt32(summaryAdModel.SearchContractSeq);
                sqlParam[7].Value = Convert.ToInt32(summaryAdModel.CampaignCode);
                #endregion

                _db.Timeout = 60 * 30;

                if (IsProfile)
                {
                    _db.ExecuteProcedureParams(ds, "dbo.dao_SummaryAd_Pro", sqlParam);
                }
                else
                {
                    _db.ExecuteProcedureParams(ds, "dbo.dao_SummaryAd", sqlParam);
                }

                summaryAdModel.ReportDataSet = ds.Copy();
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ReportDataSet);

                ds.Dispose();

                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdDaily() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                if (isNotReady)
                {
                    summaryAdModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
                }
                else
                {
                    summaryAdModel.ResultDesc = ex.Message;
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

        #region �ְ� �������� ����
        /// <summary>
        ///  �ְ� �������� ����
        /// </summary>
        /// <param name="summaryAdModel"></param>
        public void GetSummaryAdWeekly(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
            {
                StringBuilder sbQuery = null;

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdWeekly() Start");
                _log.Debug("-----------------------------------------");

                // ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
                if (summaryAdModel.SearchStartDay.Length > 6) summaryAdModel.SearchStartDay = summaryAdModel.SearchStartDay.Substring(2, 6);
                if (summaryAdModel.SearchEndDay.Length > 6) summaryAdModel.SearchEndDay = summaryAdModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchMediaCode	  :[" + summaryAdModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");		// �˻� �����ȣ           
                _log.Debug("SearchItemNo      :[" + summaryAdModel.SearchItemNo + "]");		// �˻� �����ȣ           
                _log.Debug("SearchStartDay    :[" + summaryAdModel.SearchStartDay + "]");		// �˻� ������� ����          
                _log.Debug("SearchEndDay      :[" + summaryAdModel.SearchEndDay + "]");		// �˻� �������� ����          
                // __DEBUG__

                string MediaCode = summaryAdModel.SearchMediaCode;
                string ContractSeq = summaryAdModel.SearchContractSeq;
                string ItemNo = summaryAdModel.SearchItemNo;
                string StartDay = summaryAdModel.SearchStartDay;
                string EndDay = summaryAdModel.SearchEndDay;

                string Type = "C"; // ���ǹ�

                // �����ȣ�� ������ �������Ǻ� ��ȸ�̴�.
                if (!ItemNo.Equals("") && !ItemNo.Equals("00"))
                {
                    Type = "I";
                }

                // ��������
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "-- ��ü �̿밡����             \n"
                    + "SELECT ISNULL(HouseTotal,0) AS TotUsr \n"
                    + "  FROM SummaryBase  with(NoLock)  \n"
                    + " WHERE LogDay = '" + StartDay + "' \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count == 0)
                {
                    isNotReady = true;
                    throw new Exception();
                }

                int TotUsr = Convert.ToInt32(ds.Tables[0].Rows[0]["TotUsr"].ToString());
                ds.Dispose();

                // �����ϱ��� �̿��ڼ�
                summaryAdModel.TotalUser = TotUsr;

                // ��������
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "/* �ְ� �������� ����                                     \n"
                    + "   ��ȸ���� : ��ü�ڵ�, ����, ����ȣ �Ǵ� �����ȣ */        \n"
                    + "                                                       \n"
                    + "DECLARE @MediaCode int;   -- ��ü�ڵ�                    \n"
                    + "DECLARE @LogDay CHAR(6);  -- ��������                    \n"
                    + "DECLARE @ItemNo int;      -- �����ȣ                    \n"
                    + "DECLARE @ContractSeq int; -- ����ȣ                    \n"
                    + "DECLARE @TotUsr int;      -- ��ü �̿밡����              \n"
                    + "DECLARE @TotAdHit int;    -- ��ü ��������              \n"
                    + "DECLARE @TotPgHit int;    -- ��ü ���α׷���û��           \n"
                    + "                                          \n"
                    + "SET @MediaCode   =  " + MediaCode + "  \n"
                    + "SET @LogDay      = '" + StartDay + "' \n"
                    + "SET @ItemNo      =  " + ItemNo + "  \n"
                    + "SET @ContractSeq =  " + ContractSeq + "  \n"
                    + "SET @TotUsr      =  " + TotUsr.ToString() + " \n"
                    + "SET @TotAdHit    = 0;                                                         \n"
                    + "SET @TotPgHit    = 0;                                                         \n"
                    + "                                                                              \n"
                    + "                                                                              \n"
                    + "-- ��ü ����Hit                                                               \n"
                    + "SELECT @TotAdHit = SUM(A.AdCnt)                                               \n"
                    + "      ,@TotPgHit = SUM(A.HitCnt)                                              \n"
                    + "  FROM SummaryAdWeekly0 A with(NoLock)                              \n"
                    + " WHERE LogDay       = @LogDay                                                 \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                                                                                               \n"
                    + "                                                                                                                        \n"
                    + "-- ��ǰ�� �հ�                                                                                                          \n"
                    + "SELECT  1 AS TypeCode                                                                                                   \n"
                    + "       ,'1 ��ǰ' AS TypeName                                                                                                   \n"
                    + "       ,'��ǰ' AS SumType                                                                                               \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                                                     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP                                      \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdWeekly0       A with(NoLock) \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:��ǰ��                                                                                   \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- ��ü �հ�                                                                                                            \n"
                    + "SELECT  2 AS TypeCode                                                                                                   \n"
                    + "       ,'2 �հ�' AS TypeName                                                                                                   \n"
                    + "       ,'�հ�' AS SumType                                                                                               \n"
                    + "       ,0 AS SumCode                                                                                                    \n"
                    + "       ,'' AS SumName                                                                                                   \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,null AS AdRate                                                                                                  \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,null AS PgCnt                                                                                                   \n"
                    + "       ,null AS HitCnt                                                                                                  \n"
                    + "       ,null AS PgRate                                                                                                  \n"
                    + "  FROM SummaryAdWeekly0       A with(NoLock)  \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:��ǰ��                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- ī�װ��� ����                                                                                                      \n"
                    + "SELECT  3 AS TypeCode                                                                                                   \n"
                    + "       ,'3 �ε�������' AS TypeName                                                                                                   \n"
                    + "       ,'�ε�������' AS SumType                                                                                       \n"
                    + "       ,B.CategoryCode AS SumCode                                                                                       \n"
                    + "       ,B.CategoryName AS SumName                                                                                       \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                                                     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP                                      \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdWeekly1    A with(NoLock) \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.Category B with(NoLock) ON (A.Category = B.CategoryCode)                                         \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + " GROUP BY B.CategoryCode, B.CategoryName                                                                                \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- �ʼ��� ����                                                                                                          \n"
                    + "SELECT  4 AS TypeCode                                                                                                   \n"
                    + "       ,'4 �ʼ�' AS TypeName                                                                                                   \n"
                    + "       ,'�ʼ�' AS SumType                                                                                               \n"
                    + "       ,B.AdTime AS SumCode                                                                                             \n"
                    + "       ,Convert(VARCHAR(5),B.AdTime) + '��' AS SumName                                                                  \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdWeekly0        A with(NoLock)  \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.ContractItem B with(NoLock)  ON (A.ItemNo = B.ItemNo)                                             \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:��ǰ��                                                                                   \n"
                    + " GROUP BY B.AdTime                                                                                                      \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- ���Ϻ� ����                                                                                                          \n"
                    + "SELECT  5 AS TypeCode                                                                                                   \n"
                    + "       ,'5 ����' AS TypeName                                                                                                   \n"
                    + "       ,'����' AS SumType                                                                                               \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdWeekly0 A with(NoLock)  \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)                   \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 6   -- 6:���Ϻ�                                                                                 \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- ���ɺ� ����                                                                                                          \n"
                    + "SELECT  6 AS TypeCode                                                                                                   \n"
                    + "       ,'6 ���ɺ�' AS TypeName                                                                                                   \n"
                    + "       ,'���ɺ�' AS SumType                                                                                             \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdWeekly0       A with(NoLock)  \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)                   \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 3   -- 3:���ɺ�                                                                                 \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- ������ ����                                                                                                          \n"
                    + "SELECT  7 AS TypeCode                                                                                                   \n"
                    + "       ,'7 ������' AS TypeName                                                                                                   \n"
                    + "       ,'������' AS SumType                                                                                             \n"
                    + "       ,TA.SummaryCode AS SumCode                                                                                       \n"
                    + "       ,TA.SummaryName AS SumName                                                                                       \n"
                    + "       ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt                                                                                \n"
                    + "       ,ISNULL(CONVERT(DECIMAL(9,2),(SUM(TB.AdCnt) / CONVERT(float,@TotAdHit)) * 100),0) AS AdRate                      \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,MAX(TB.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(TB.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(TB.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM (SELECT SummaryCode, SummaryName FROM SummaryCode with(NoLock) WHERE SummaryType = 5 AND Level = 1) TA                        \n"
                    + "       LEFT JOIN                                                                                                        \n"
                    + "       (                                                                                                                \n"
                    + "        SELECT CASE B.Level WHEN 1 THEN B.SummaryCode                                                                   \n"
                    + "                            ELSE B.UpperCode END AS SummaryCode                                                         \n"
                    + "              ,A.AdCnt                                                                                                  \n"
                    + "              ,A.HitCnt                                                                                                  \n"
                    + "              ,A.PgCnt                                                                                                  \n"
                    + "              ,B.Level                                                                                                  \n"
                    + "          FROM SummaryAdWeekly0       A with(NoLock) \n"
                    + "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)           \n"
                    + "         WHERE A.LogDay       = @LogDay                                                                                 \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("           AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("           AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "           AND A.SummaryType  = 5  -- 5:������                                                                          \n"
                    + "       ) TB                                                                                                             \n"
                    + "       ON (TA.SummaryCode = TB.SummaryCode)                                                                             \n"
                    + " GROUP BY TA.SummaryCode, TA.SummaryName                                                                                \n"
                    + "                                                                                                                        \n"
                    + "ORDER BY TypeCode, SumCode                                                                                              \n"
                    );


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �����͸𵨿� ����
                summaryAdModel.ReportDataSet = ds.Copy();

                // ���
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ReportDataSet);

                ds.Dispose();

                // ����ڵ� ��Ʈ
                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdWeekly() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                if (isNotReady)
                {
                    summaryAdModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
                }
                else
                {
                    summaryAdModel.ResultDesc = "�ְ� �������� ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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

        #region ���� �������� ����
        /// <summary>
        ///  ���� �������� ����
        /// </summary>
        /// <param name="summaryAdModel"></param>
        public void GetSummaryAdMonthly(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
            {
                StringBuilder sbQuery = null;

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdMonthly() Start");
                _log.Debug("-----------------------------------------");

                // ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
                if (summaryAdModel.SearchStartDay.Length > 6) summaryAdModel.SearchStartDay = summaryAdModel.SearchStartDay.Substring(2, 6);
                if (summaryAdModel.SearchEndDay.Length > 6) summaryAdModel.SearchEndDay = summaryAdModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchMediaCode	  :[" + summaryAdModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");		// �˻� �����ȣ           
                _log.Debug("SearchItemNo      :[" + summaryAdModel.SearchItemNo + "]");		// �˻� �����ȣ           
                _log.Debug("SearchStartDay    :[" + summaryAdModel.SearchStartDay + "]");		// �˻� ������� ����          
                _log.Debug("SearchEndDay      :[" + summaryAdModel.SearchEndDay + "]");		// �˻� �������� ����          
                // __DEBUG__

                string MediaCode = summaryAdModel.SearchMediaCode;
                string ContractSeq = summaryAdModel.SearchContractSeq;
                string ItemNo = summaryAdModel.SearchItemNo;
                string StartDay = summaryAdModel.SearchStartDay;
                string EndDay = summaryAdModel.SearchEndDay;

                string Type = "C"; // ���ǹ�

                // �����ȣ�� ������ �������Ǻ� ��ȸ�̴�.
                if (!ItemNo.Equals("") && !ItemNo.Equals("00"))
                {
                    Type = "I";
                }

                // ��������
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "-- ��ü �̿밡����             \n"
                    + "SELECT ISNULL(HouseTotal,0) AS TotUsr \n"
                    + "  FROM SummaryBase with(NoLock)  \n"
                    + " WHERE LogDay = '" + StartDay + "' \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count == 0)
                {
                    isNotReady = true;
                    throw new Exception();
                }

                int TotUsr = Convert.ToInt32(ds.Tables[0].Rows[0]["TotUsr"].ToString());
                ds.Dispose();

                // �����ϱ��� �̿��ڼ�
                summaryAdModel.TotalUser = TotUsr;

                // ��������
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "/* �ְ� �������� ����                                     \n"
                    + "   ��ȸ���� : ��ü�ڵ�, ����, ����ȣ �Ǵ� �����ȣ */        \n"
                    + "                                                       \n"
                    + "DECLARE @MediaCode int;   -- ��ü�ڵ�                    \n"
                    + "DECLARE @LogDay CHAR(6);  -- ��������                    \n"
                    + "DECLARE @ItemNo int;      -- �����ȣ                    \n"
                    + "DECLARE @ContractSeq int; -- ����ȣ                    \n"
                    + "DECLARE @TotUsr int;      -- ��ü �̿밡����              \n"
                    + "DECLARE @TotAdHit int;    -- ��ü ��������              \n"
                    + "DECLARE @TotPgHit int;    -- ��ü ���α׷���û��           \n"
                    + "                                          \n"
                    + "SET @MediaCode   =  " + MediaCode + "  \n"
                    + "SET @LogDay      = '" + StartDay + "' \n"
                    + "SET @ItemNo      =  " + ItemNo + "  \n"
                    + "SET @ContractSeq =  " + ContractSeq + "  \n"
                    + "SET @TotUsr      =  " + TotUsr.ToString() + " \n"
                    + "SET @TotAdHit    = 0;                                                         \n"
                    + "SET @TotPgHit    = 0;                                                         \n"
                    + "                                                                              \n"
                    + "                                                                              \n"
                    + "-- ��ü ����Hit                                                               \n"
                    + "SELECT @TotAdHit = SUM(A.AdCnt)                                               \n"
                    + "      ,@TotPgHit = SUM(A.HitCnt)                                              \n"
                    + "  FROM SummaryAdMonthly0 A with(NoLock)                                     \n"
                    + " WHERE LogDay       = @LogDay                                                 \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                                                                                               \n"
                    + "                                                                                                                        \n"
                    + "-- ��ǰ�� �հ�                                                                                                          \n"
                    + "SELECT  1 AS TypeCode                                                                                                   \n"
                    + "       ,'1 ��ǰ' AS TypeName                                                                                                   \n"
                    + "       ,'��ǰ' AS SumType                                                                                               \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                                                     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP                                      \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdMonthly0      A with(NoLock) \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:��ǰ��                                                                                   \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- ��ü �հ�                                                                                                            \n"
                    + "SELECT  2 AS TypeCode                                                                                                   \n"
                    + "       ,'2 �հ�' AS TypeName                                                                                                   \n"
                    + "       ,'�հ�' AS SumType                                                                                               \n"
                    + "       ,0 AS SumCode                                                                                                    \n"
                    + "       ,'' AS SumName                                                                                                   \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,null AS AdRate                                                                                                  \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,null AS PgCnt                                                                                                   \n"
                    + "       ,null AS HitCnt                                                                                                  \n"
                    + "       ,null AS PgRate                                                                                                  \n"
                    + "  FROM SummaryAdMonthly0      A with(NoLock) \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:��ǰ��                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- ī�װ��� ����                                                                                                      \n"
                    + "SELECT  3 AS TypeCode                                                                                                   \n"
                    + "       ,'3 �ε�������' AS TypeName                                                                                                   \n"
                    + "       ,'�ε�������' AS SumType                                                                                       \n"
                    + "       ,B.CategoryCode AS SumCode                                                                                       \n"
                    + "       ,B.CategoryName AS SumName                                                                                       \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                                                     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP                                      \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdMonthly1   A with(NoLock) \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.Category B with(NoLock) ON (A.Category = B.CategoryCode)                                         \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + " GROUP BY B.CategoryCode, B.CategoryName                                                                                \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- �ʼ��� ����                                                                                                          \n"
                    + "SELECT  4 AS TypeCode                                                                                                   \n"
                    + "       ,'4 �ʼ�' AS TypeName                                                                                                   \n"
                    + "       ,'�ʼ�' AS SumType                                                                                               \n"
                    + "       ,B.AdTime AS SumCode                                                                                             \n"
                    + "       ,Convert(VARCHAR(5),B.AdTime) + '��' AS SumName                                                                  \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdMonthly0       A with(NoLock)  \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo)                                             \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:��ǰ��                                                                                   \n"
                    + " GROUP BY B.AdTime                                                                                                      \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- ���Ϻ� ����                                                                                                          \n"
                    + "SELECT  5 AS TypeCode                                                                                                   \n"
                    + "       ,'5 ����' AS TypeName                                                                                                   \n"
                    + "       ,'����' AS SumType                                                                                               \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdMonthly0      A with(NoLock) \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)                   \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 6   -- 6:���Ϻ�                                                                                 \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- ���ɺ� ����                                                                                                          \n"
                    + "SELECT  6 AS TypeCode                                                                                                   \n"
                    + "       ,'6 ���ɺ�' AS TypeName                                                                                                   \n"
                    + "       ,'���ɺ�' AS SumType                                                                                             \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdMonthly0      A with(NoLock) \n "
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)                   \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 3   -- 3:���ɺ�                                                                                 \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- ������ ����                                                                                                          \n"
                    + "SELECT  7 AS TypeCode                                                                                                   \n"
                    + "       ,'7 ������' AS TypeName                                                                                                   \n"
                    + "       ,'������' AS SumType                                                                                             \n"
                    + "       ,TA.SummaryCode AS SumCode                                                                                       \n"
                    + "       ,TA.SummaryName AS SumName                                                                                       \n"
                    + "       ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt                                                                                \n"
                    + "       ,ISNULL(CONVERT(DECIMAL(9,2),(SUM(TB.AdCnt) / CONVERT(float,@TotAdHit)) * 100),0) AS AdRate                      \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,MAX(TB.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(TB.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(TB.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM (SELECT SummaryCode, SummaryName FROM SummaryCode with(NoLock) WHERE SummaryType = 5 AND Level = 1) TA                        \n"
                    + "       LEFT JOIN                                                                                                        \n"
                    + "       (                                                                                                                \n"
                    + "        SELECT CASE B.Level WHEN 1 THEN B.SummaryCode                                                                   \n"
                    + "                            ELSE B.UpperCode END AS SummaryCode                                                         \n"
                    + "              ,A.AdCnt                                                                                                  \n"
                    + "              ,A.HitCnt                                                                                                  \n"
                    + "              ,A.PgCnt                                                                                                  \n"
                    + "              ,B.Level                                                                                                  \n"
                    + "          FROM SummaryAdMonthly0      A with(NoLock) \n"
                    + "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)           \n"
                    + "         WHERE A.LogDay       = @LogDay                                                                                 \n"
                    );
                // �����ȸ���� ����������ȸ����
                if (Type.Equals("I"))
                {
                    sbQuery.Append("           AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("           AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "           AND A.SummaryType  = 5  -- 5:������                                                                          \n"
                    + "       ) TB                                                                                                             \n"
                    + "       ON (TA.SummaryCode = TB.SummaryCode)                                                                             \n"
                    + " GROUP BY TA.SummaryCode, TA.SummaryName                                                                                \n"
                    + "                                                                                                                        \n"
                    + "ORDER BY TypeCode, SumCode                                                                                              \n"
                    );


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �����͸𵨿� ����
                summaryAdModel.ReportDataSet = ds.Copy();

                // ���
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ReportDataSet);

                ds.Dispose();

                // ����ڵ� ��Ʈ
                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdMonthly() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                if (isNotReady)
                {
                    summaryAdModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
                }
                else
                {
                    summaryAdModel.ResultDesc = "���� �������� ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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

        #region �Ϻ� �Ͽ콺Ȧ�� �������� ����
        /// <summary>
        ///  �Ϻ� �Ͽ콺Ȧ�� �������� ����
        /// </summary>
        /// <param name="dateAdTypeSummaryRptModel"></param>
        public void GetDateAccountHouseHold(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
            try
            {
                StringBuilder sbQuery = null;

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDateAccountHouseHold() Start");
                _log.Debug("-----------------------------------------");

                // ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
                if (summaryAdModel.SearchStartDay.Length > 6) summaryAdModel.SearchStartDay = summaryAdModel.SearchStartDay.Substring(2, 6);
                if (summaryAdModel.SearchEndDay.Length > 6) summaryAdModel.SearchEndDay = summaryAdModel.SearchEndDay.Substring(2, 6);


                _log.Debug("<�Է�����>");
                _log.Debug("CampaignCode	  :[" + summaryAdModel.CampaignCode + "]");		// ķ���� �ڵ�
                _log.Debug("SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");		// �˻� �����ȣ           
                _log.Debug("SearchItemNo      :[" + summaryAdModel.SearchItemNo + "]");		// �˻� �����ȣ           
                _log.Debug("SearchStartDay    :[" + summaryAdModel.SearchStartDay + "]");		// �˻� ������� ����          
                _log.Debug("SearchEndDay      :[" + summaryAdModel.SearchEndDay + "]");		// �˻� �������� ����          
                // __DEBUG__

                string ContractSeq = summaryAdModel.SearchContractSeq;
                string CampaignCode = summaryAdModel.CampaignCode;
                string ItemNo = summaryAdModel.SearchItemNo;
                string StartDay = summaryAdModel.SearchStartDay;
                string EndDay = summaryAdModel.SearchEndDay;

                // ��������
                sbQuery = new StringBuilder();
                sbQuery.Append("																							\n");
                sbQuery.Append("DECLARE @LOGSTARTDAY CHAR(6);																\n");
                sbQuery.Append("DECLARE @LOGENDDAY CHAR(6);																	\n");
                sbQuery.Append("SELECT @LOGSTARTDAY = '" + StartDay + "';													\n");
                sbQuery.Append("SELECT @LOGENDDAY = '" + EndDay + "';														\n");
                sbQuery.Append("																							\n");
                sbQuery.Append("WITH ItemList (ItemNo)																		\n");
                sbQuery.Append("AS (																						\n");
                sbQuery.Append("		SELECT	a.ItemNo																	\n");
                sbQuery.Append("		FROM	AdTargetsHanaTV.dbo.ContractItem a WITH(NOLOCK)													\n");
                if (CampaignCode != null && !CampaignCode.Equals("00"))
                {
                    sbQuery.Append("		INNER JOIN AdTargetsHanaTV.dbo.CampaignDetail b ON (a.ItemNo = b.ItemNo AND b.CampaignCode = " + CampaignCode + ")	\n");
                }
                sbQuery.Append("		WHERE ContractSeq = " + ContractSeq + "												\n");
                if (ItemNo != null && !ItemNo.Equals("00"))
                {
                    sbQuery.Append("		AND		a.ItemNo = " + ItemNo + "													\n");
                }
                sbQuery.Append("), RowData(LogDay, Week, Rnum, PRnum, Cnt)														\n");
                sbQuery.Append("AS (																						\n");
                sbQuery.Append("		SELECT																				\n");
                sbQuery.Append("				LogDay, Week																\n");
                sbQuery.Append("				, ROW_NUMBER() OVER(ORDER BY LogDay) Rnum									\n");
                sbQuery.Append("				, ROW_NUMBER() OVER(ORDER BY LogDay) -1 PRnum								\n");
                sbQuery.Append("				, (SELECT COUNT(*) FROM HouseHoldAdDaily V1 WITH(NOLOCK)					\n");
                sbQuery.Append("						WHERE LogDay >= @LOGSTARTDAY AND LogDay <= A.LogDay AND ItemNo IN (	\n");
                sbQuery.Append("						SELECT ItemNo FROM ItemList)) Cnt									\n");
                sbQuery.Append("		FROM SummaryBase A WITH(NOLOCK)														\n");
                sbQuery.Append("		WHERE																				\n");
                sbQuery.Append("				LogDay >= @LOGSTARTDAY														\n");
                sbQuery.Append("				AND LogDay <= @LOGENDDAY													\n");
                sbQuery.Append(")																							\n");
                sbQuery.Append("SELECT																						\n");
                sbQuery.Append("		CONVERT(CHAR(10), CONVERT(datetime, '20' + A.LogDay, 112),120) +					\n");
                sbQuery.Append("				CASE A.week WHEN 1 THEN ' (��)'												\n");
                sbQuery.Append("						 WHEN 2 THEN ' (��)'												\n");
                sbQuery.Append("						 WHEN 3 THEN ' (ȭ)'												\n");
                sbQuery.Append("						 WHEN 4 THEN ' (��)'												\n");
                sbQuery.Append("						 WHEN 5 THEN ' (��)'												\n");
                sbQuery.Append("						 WHEN 6 THEN ' (��)'												\n");
                sbQuery.Append("						 WHEN 7 THEN ' (��)'												\n");
                sbQuery.Append("				 ELSE '' END AS LogDay														\n");
                sbQuery.Append("		, A.Cnt, (A.Cnt - ISNULL(B.Cnt, 0)) Inc, A.Week										\n");
                sbQuery.Append("FROM RowData A																				\n");
                sbQuery.Append("		LEFT OUTER JOIN RowData B ON (B.Rnum = A.PRnum)										\n");
                sbQuery.Append("ORDER BY A.LogDay																			\n");
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �����͸𵨿� ����
                summaryAdModel.ReportDataSet = ds.Copy();

                // ���
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ReportDataSet);

                ds.Dispose();

                // ����ڵ� ��Ʈ
                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDateAccountHouseHold() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                if (isNotReady)
                {
                    summaryAdModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
                }
                else
                {
                    summaryAdModel.ResultDesc = "�Ϻ� �Ͽ콺Ȧ�� �������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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