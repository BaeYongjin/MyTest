// ===============================================================================
//
// RptAdCategorySummaryBiz.cs
//
// ���� �Ѱ����� ���� 
//
// ===============================================================================
// Release history
// 2007.10.26 BJ.PARK OAP�� ���谡����� �� ��������̿� �޼ҵ� => GetContractItemList()
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: RptAdCategorySummaryBiz
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
 * �����ڵ�  : [E_02]
 * ������    : H.J.LEE
 * ������    : 2015.06.01
 * �����κ�  :
 *			  - ���� ����
 * ��������  : 
 *            - �޴� �񵿱�ȭ�� ���� 0������ ��������
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

namespace AdManagerWebService.ReportSummaryAd
{
    /// <summary>
    /// RptAdCategorySummaryBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class RptAdCategorySummaryBiz : BaseBiz
    {

        #region  ������
        public RptAdCategorySummaryBiz()
            : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region [ī�װ��� �������� ����]
        /// <summary>
        ///  �ѱⰣ �������� ����
        /// </summary>
        /// <param name="rptAdCategorySummaryModel"></param>
        public void GetRptAdCategorySummary(HeaderModel header, RptAdCategorySummaryModel rptAdCategorySummaryModel)
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
                if (rptAdCategorySummaryModel.LogDay.Length > 6) rptAdCategorySummaryModel.LogDay = rptAdCategorySummaryModel.LogDay.Substring(2, 6);
                if (rptAdCategorySummaryModel.LogDayEnd.Length > 6) rptAdCategorySummaryModel.LogDayEnd = rptAdCategorySummaryModel.LogDayEnd.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("LogDay	  :[" + rptAdCategorySummaryModel.LogDay + "]");		// �˻� ��ü
                _log.Debug("LogDayEnd :[" + rptAdCategorySummaryModel.LogDayEnd + "]");
                _log.Debug("AdType	  :[" + rptAdCategorySummaryModel.AdType + "]");
                // __DEBUG__

                string logDay = rptAdCategorySummaryModel.LogDay;
                string logDayEnd = rptAdCategorySummaryModel.LogDayEnd;
                string adType = rptAdCategorySummaryModel.AdType;

                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@LogDay", SqlDbType.VarChar, 6);
                sqlParams[0].Value = rptAdCategorySummaryModel.LogDay;

                // ��������
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "/* �ѱⰣ �������� ����																				\n"
                    + "   ��ȸ���� : ��ü�ڵ�, ����ȣ �Ǵ� �����ȣ */														\n"
                    + "																									\n"
                    + "DECLARE @LogDay varchar(6);   -- ��ü�ڵ�															\n"
                    + "DECLARE @LogDayEnd varchar(6);  -- ��������														\n"
                    + "DECLARE @AdType varchar(2);      -- �����ȣ										 				\n"

                    + "SET @LogDay   =		'" + logDay + "'\n"
                    + "SET @LogDayEnd    = '" + logDayEnd + "'\n"
                    + "SET @AdType    =	   '" + adType + "';\n"

                    + "with AdData(ItemNo, ItemName, ContSeq, Tot, C99												  \n"
                    + "						,C1, C2, C3, C4, C5, C6, C7, C8, C9, C10                                  \n"
                    + "						,C11,C12,C13,C14,C15,C16,C17,C18,C19,C20                                  \n"
                    + "						,C21,C22,C23,C24,C25,C26,C27,C28,C29,C30 ) as                             \n"
                    + " (																							  \n"
                    + " 	select		 0 as ItemNo																  \n"
                    + " 				,'ī�װ��� ä����Ʈ'		as ItemName											  \n"
                    + " 				,0 as ContSeq																  \n"
                    + " 				,sum(v1.HitCnt)	as Tot																													\n"
                    + " 				,sum(Case v1.SortNo when 99 then v1.HitCnt	else	0	end)	as	C99                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 1  then v1.HitCnt	else	0	end)	as	C1                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 2  then v1.HitCnt	else	0	end)	as	C2                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 3  then v1.HitCnt	else	0	end)	as	C3                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 4  then v1.HitCnt	else	0	end)	as	C4                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 5  then v1.HitCnt	else	0	end)	as	C5                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 6  then v1.HitCnt	else	0	end)	as	C6                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 7  then v1.HitCnt	else	0	end)	as	C7                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 8  then v1.HitCnt	else	0	end)	as	C8                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 9  then v1.HitCnt	else	0	end)	as	C9                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 10 then v1.HitCnt	else	0	end)	as	C10                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 11 then v1.HitCnt	else	0	end)	as	C11                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 12 then v1.HitCnt	else	0	end)	as	C12                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 13 then v1.HitCnt	else	0	end)	as	C13                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 14 then v1.HitCnt	else	0	end)	as	C14                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 15 then v1.HitCnt	else	0	end)	as	C15                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 16 then v1.HitCnt	else	0	end)	as	C16                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 17 then v1.HitCnt	else	0	end)	as	C17                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 18 then v1.HitCnt	else	0	end)	as	C18                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 19 then v1.HitCnt	else	0	end)	as	C19                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 20 then v1.HitCnt	else	0	end)	as	C20                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 21 then v1.HitCnt	else	0	end)	as	C21                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 22 then v1.HitCnt	else	0	end)	as	C22                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 23 then v1.HitCnt	else	0	end)	as	C23                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 24 then v1.HitCnt	else	0	end)	as	C24                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 25 then v1.HitCnt	else	0	end)	as	C25                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 26 then v1.HitCnt	else	0	end)	as	C26                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 27 then v1.HitCnt	else	0	end)	as	C27                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 28 then v1.HitCnt	else	0	end)	as	C28                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 29 then v1.HitCnt	else	0	end)	as	C29                                                                 \n"
                    + " 				,sum(Case v1.SortNo when 30 then v1.HitCnt	else	0	end)	as	C30                                                                 \n"
                    + " from (	select case Flag when 'Y' then SortNo else 99 end as SortNo																						\n"
                    + " 							,HitCnt																														\n"
                    + " 				from	SummaryPgDaily1 with(NoLock)																									\n"
                    + " 				inner join AdTargetsHanaTV.dbo.Category c on c.CategoryCode = category																						\n"
                    + " 				where	LogDay between @LogDay and @LogDayEnd) v1																						\n"
                    + " union all								                                                       \n"
                    + "       select 1                              \n"
                    + "       			,'ī�װ��� �����հ�'                              \n"
                    + "		  			,0							  \n"
                    + "		  			,sum(v1.HitCnt)	as Tot							  \n"
                    + "		  			,sum(Case v1.SortNo when 99 then v1.HitCnt	else	0	end)	as	C99							  \n"
                    + "		  			,sum(Case v1.SortNo when 1  then v1.HitCnt	else	0	end)	as	C1							  \n"
                    + "		  			,sum(Case v1.SortNo when 2  then v1.HitCnt	else	0	end)	as	C2							  \n"
                    + "		  			,sum(Case v1.SortNo when 3  then v1.HitCnt	else	0	end)	as	C3							  \n"
                    + "		  			,sum(Case v1.SortNo when 4  then v1.HitCnt	else	0	end)	as	C4							  \n"
                    + "		  			,sum(Case v1.SortNo when 5  then v1.HitCnt	else	0	end)	as	C5							  \n"
                    + "		  			,sum(Case v1.SortNo when 6  then v1.HitCnt	else	0	end)	as	C6							  \n"
                    + "		  			,sum(Case v1.SortNo when 7  then v1.HitCnt	else	0	end)	as	C7							  \n"
                    + "		  			,sum(Case v1.SortNo when 8  then v1.HitCnt	else	0	end)	as	C8							  \n"
                    + "		  			,sum(Case v1.SortNo when 9  then v1.HitCnt	else	0	end)	as	C9							  \n"
                    + "		  			,sum(Case v1.SortNo when 10 then v1.HitCnt	else	0	end)	as	C10							  \n"
                    + "		  			,sum(Case v1.SortNo when 11 then v1.HitCnt	else	0	end)	as	C11							  \n"
                    + "		  			,sum(Case v1.SortNo when 12 then v1.HitCnt	else	0	end)	as	C12							  \n"
                    + "		  			,sum(Case v1.SortNo when 13 then v1.HitCnt	else	0	end)	as	C13							  \n"
                    + "		  			,sum(Case v1.SortNo when 14 then v1.HitCnt	else	0	end)	as	C14							  \n"
                    + "		  			,sum(Case v1.SortNo when 15 then v1.HitCnt	else	0	end)	as	C15							  \n"
                    + "		  			,sum(Case v1.SortNo when 16 then v1.HitCnt	else	0	end)	as	C16							  \n"
                    + "		  			,sum(Case v1.SortNo when 17 then v1.HitCnt	else	0	end)	as	C17							  \n"
                    + "		  			,sum(Case v1.SortNo when 18 then v1.HitCnt	else	0	end)	as	C18							  \n"
                    + "		  			,sum(Case v1.SortNo when 19 then v1.HitCnt	else	0	end)	as	C19							  \n"
                    + "		  			,sum(Case v1.SortNo when 20 then v1.HitCnt	else	0	end)	as	C20							  \n"
                    + "		  			,sum(Case v1.SortNo when 21 then v1.HitCnt	else	0	end)	as	C21							  \n"
                    + "		  			,sum(Case v1.SortNo when 22 then v1.HitCnt	else	0	end)	as	C22							  \n"
                    + "		  			,sum(Case v1.SortNo when 23 then v1.HitCnt	else	0	end)	as	C23							  \n"
                    + "		  			,sum(Case v1.SortNo when 24 then v1.HitCnt	else	0	end)	as	C24							  \n"
                    + "		  			,sum(Case v1.SortNo when 25 then v1.HitCnt	else	0	end)	as	C25							  \n"
                    + "		  			,sum(Case v1.SortNo when 26 then v1.HitCnt	else	0	end)	as	C26							  \n"
                    + "		  			,sum(Case v1.SortNo when 27 then v1.HitCnt	else	0	end)	as	C27							  \n"
                    + "		  			,sum(Case v1.SortNo when 28 then v1.HitCnt	else	0	end)	as	C28							  \n"
                    + "		  			,sum(Case v1.SortNo when 29 then v1.HitCnt	else	0	end)	as	C29							  \n"
                    + "		  			,sum(Case v1.SortNo when 30 then v1.HitCnt	else	0	end)	as	C30							  \n"
                    + "		  from (	select case Flag when 'Y' then SortNo else 99 end as SortNo										  \n"
                    + "		  							,AdCnt as HitCnt																  \n"
                    + "		  				from	SummaryAdDaily1	a	with(NoLock)													  \n"
                    + "		  				inner join AdTargetsHanaTV.dbo.ContractItem i	with(NoLock) on a.ItemNo = i.ItemNo and i.AdType = @AdType		  \n"
                    + "		  				inner join AdTargetsHanaTV.dbo.Category c on c.CategoryCode = category											  \n"
                    + "		  				where	LogDay between @LogDay and @LogDayEnd												  \n"
                    + "		  				and		a.ItemNo > 0 ) v1																  										  \n"
                    + "		  union all																								  										  \n"
                    + "		  select v1.ItemNo																																  \n"
                    + "		  			,( select cd.ItemName from AdTargetsHanaTV.dbo.ContractItem cd with(nolock) where cd.ItemNo = v1.ItemNo )	as ItemNm								  \n"
                    + "		  			,( select cd.ContractSeq from AdTargetsHanaTV.dbo.ContractItem cd with(nolock) where cd.ItemNo = v1.ItemNo )	as Contract							  \n"
                    + "		  			,sum(v1.AdCnt)	as Tot																												  \n"
                    + "		  			,sum(Case v1.SortNo when 99 then v1.AdCnt	else	0	end)	as	C99																  \n"
                    + "		  			,sum(Case v1.SortNo when 1  then v1.AdCnt	else	0	end)	as	C1																  \n"
                    + "		  			,sum(Case v1.SortNo when 2  then v1.AdCnt	else	0	end)	as	C2																  \n"
                    + "		  			,sum(Case v1.SortNo when 3  then v1.AdCnt	else	0	end)	as	C3																  \n"
                    + "		  			,sum(Case v1.SortNo when 4  then v1.AdCnt	else	0	end)	as	C4																  \n"
                    + "		  			,sum(Case v1.SortNo when 5  then v1.AdCnt	else	0	end)	as	C5																  \n"
                    + "		  			,sum(Case v1.SortNo when 6  then v1.AdCnt	else	0	end)	as	C6																  \n"
                    + "		  			,sum(Case v1.SortNo when 7  then v1.AdCnt	else	0	end)	as	C7																  \n"
                    + "		  			,sum(Case v1.SortNo when 8  then v1.AdCnt	else	0	end)	as	C8																  \n"
                    + "		  			,sum(Case v1.SortNo when 9  then v1.AdCnt	else	0	end)	as	C9																  \n"
                    + "		  			,sum(Case v1.SortNo when 10 then v1.AdCnt	else	0	end)	as	C10																  \n"
                    + "		  			,sum(Case v1.SortNo when 11 then v1.AdCnt	else	0	end)	as	C11																  \n"
                    + "		  			,sum(Case v1.SortNo when 12 then v1.AdCnt	else	0	end)	as	C12																  \n"
                    + "		  			,sum(Case v1.SortNo when 13 then v1.AdCnt	else	0	end)	as	C13																  \n"
                    + "		  			,sum(Case v1.SortNo when 14 then v1.AdCnt	else	0	end)	as	C14																  \n"
                    + "		  			,sum(Case v1.SortNo when 15 then v1.AdCnt	else	0	end)	as	C15																  \n"
                    + "		  			,sum(Case v1.SortNo when 16 then v1.AdCnt	else	0	end)	as	C16																  \n"
                    + "		  			,sum(Case v1.SortNo when 17 then v1.AdCnt	else	0	end)	as	C17																  \n"
                    + "		  			,sum(Case v1.SortNo when 18 then v1.AdCnt	else	0	end)	as	C18																  \n"
                    + "		  			,sum(Case v1.SortNo when 19 then v1.AdCnt	else	0	end)	as	C19																  \n"
                    + "		  			,sum(Case v1.SortNo when 20 then v1.AdCnt	else	0	end)	as	C20																  \n"
                    + "		  			,sum(Case v1.SortNo when 21 then v1.AdCnt	else	0	end)	as	C21																  \n"
                    + "		  			,sum(Case v1.SortNo when 22 then v1.AdCnt	else	0	end)	as	C22																  \n"
                    + "		  			,sum(Case v1.SortNo when 23 then v1.AdCnt	else	0	end)	as	C23																  \n"
                    + "		  			,sum(Case v1.SortNo when 24 then v1.AdCnt	else	0	end)	as	C24																  \n"
                    + "		  			,sum(Case v1.SortNo when 25 then v1.AdCnt	else	0	end)	as	C25																  \n"
                    + "		  			,sum(Case v1.SortNo when 26 then v1.AdCnt	else	0	end)	as	C26																  \n"
                    + "		  			,sum(Case v1.SortNo when 27 then v1.AdCnt	else	0	end)	as	C27																  \n"
                    + "		  			,sum(Case v1.SortNo when 28 then v1.AdCnt	else	0	end)	as	C28																  \n"
                    + "		  			,sum(Case v1.SortNo when 29 then v1.AdCnt	else	0	end)	as	C29																  \n"
                    + "		  			,sum(Case v1.SortNo when 30 then v1.AdCnt	else	0	end)	as	C30																  \n"
                    + "		 from	(	/* ����ʵ� ī�װ��� 0�� ������ �Ѵ�. */																								  \n"
                    + "		 				select case when cd.ItemNo				is null then ad.ItemNo		else cd.ItemNo				end	as ItemNo					  \n"
                    + "		 							,case when cd.CategoryCode	is null then ad.Category	else cd.CategoryCode	end	as Category						  \n"
                    + "		 							,case when cd.SortNo				is null then 99						else cd.SortNo				end as SortNo	  \n"
                    + "		 							,isnull(ad.AdSum,0)		as AdCnt																					  \n"
                    + "		 							,isnull(ad.HitSum,0)	as PgCnt																					  \n"
                    + "		 				from	(																														  \n"
                    + "		 								select am1.ItemNo																								  \n"
                    + "		 											,am1.Category																						  \n"
                    + "		 											,Sum(am1.AdCnt)		as AdSum																		  \n"
                    + "		 											,Sum(am1.HitCnt)	as HitSum																		  \n"
                    + "		 								from	dbo.SummaryAdDaily1 am1 with(NoLock)																	  \n"
                    + "		 								inner join	AdTargetsHanaTV.dbo.ContractItem i	 with(NoLock)	ON am1.ItemNo = i.ItemNo and i.AdType = @AdType					  \n"
                    + "		 								where	am1.LogDay between @LogDay and @LogDayEnd																  \n"
                    + "		 								and		am1.ItemNo > 0																							  \n"
                    + "		 								group by am1.ItemNo,am1.Category ) ad																			  \n"
                    + "		 				full outer join																													  \n"
                    + "		 							( /* ������ ��üī�װ��� ��ȣ�����Ѵ� */																				  \n"
                    + "		 								select b.ItemNo, a.CategoryCode, a.SortNo																		  \n"
                    + "		 								from	AdTargetsHanaTV.dbo.Category a																								  \n"
                    + "		 								cross join 																										  \n"
                    + "		 										(	select am1.ItemNo																					  \n"
                    + "		 											from	dbo.SummaryAdDaily1  am1 with(NoLock)														  \n"
                    + "		 											inner join	AdTargetsHanaTV.dbo.ContractItem i	 with(NoLock)	ON am1.ItemNo = i.ItemNo and i.AdType = @AdType		  \n"
                    + "		 											where	am1.LogDay between @LogDay and @LogDayEnd													  \n"
                    + "		 											and		am1.ItemNo > 0																				  \n"
                    + "		 											group by am1.ItemNo ) b																				  \n"
                    + "		 								where	a.Flag = 'Y' ) cd																						  \n"
                    + "		 				on ad.ItemNo		= cd.ItemNo																									  \n"
                    + "		 				and	ad.Category = cd.CategoryCode ) v1																							  \n"
                    + "		 group by v1.ItemNo																																  \n"
                    + "	)	 																																				  \n"
                    + "		  select ' �����ȣ'	as	ItemNo																  \n"
                    + "		  			,' �����'		as	ItemNm																  \n"
                    + "		  			,' ����ȣ'	as	ContSeq																  \n"
                    + "		  			,' �հ�'			as	Tot																  \n"
                    + "		  			,max(Case SortNo when 99 then CategoryName	else	''	end)	as	C99																  \n"
                    + "		  			,max(Case SortNo when 1  then CategoryName	else	''	end)	as	C1																  \n"
                    + "		  			,max(Case SortNo when 2  then CategoryName	else	''	end)	as	C2																  \n"
                    + "		  			,max(Case SortNo when 3  then CategoryName	else	''	end)	as	C3																  \n"
                    + "		  			,max(Case SortNo when 4  then CategoryName	else	''	end)	as	C4																  \n"
                    + "		  			,max(Case SortNo when 5  then CategoryName	else	''	end)	as	C5																  \n"
                    + "		  			,max(Case SortNo when 6  then CategoryName	else	''	end)	as	C6																  \n"
                    + "		  			,max(Case SortNo when 7  then CategoryName	else	''	end)	as	C7																  \n"
                    + "		  			,max(Case SortNo when 8  then CategoryName	else	''	end)	as	C8																  \n"
                    + "		  			,max(Case SortNo when 9  then CategoryName	else	''	end)	as	C9																  \n"
                    + "		  			,max(Case SortNo when 10 then CategoryName	else	''	end)	as	C10																  \n"
                    + "		  			,max(Case SortNo when 11 then CategoryName	else	''	end)	as	C11																  \n"
                    + "		  			,max(Case SortNo when 12 then CategoryName	else	''	end)	as	C12																  \n"
                    + "		  			,max(Case SortNo when 13 then CategoryName	else	''	end)	as	C13																  \n"
                    + "		  			,max(Case SortNo when 14 then CategoryName	else	''	end)	as	C14																  \n"
                    + "		  			,max(Case SortNo when 15 then CategoryName	else	''	end)	as	C15																  \n"
                    + "		  			,max(Case SortNo when 16 then CategoryName	else	''	end)	as	C16																  \n"
                    + "		  			,max(Case SortNo when 17 then CategoryName	else	''	end)	as	C17																  \n"
                    + "		  			,max(Case SortNo when 18 then CategoryName	else	''	end)	as	C18																  \n"
                    + "		  			,max(Case SortNo when 19 then CategoryName	else	''	end)	as	C19																  \n"
                    + "		  			,max(Case SortNo when 20 then CategoryName	else	''	end)	as	C20																  \n"
                    + "		  			,max(Case SortNo when 21 then CategoryName	else	''	end)	as	C21																  \n"
                    + "		  			,max(Case SortNo when 22 then CategoryName	else	''	end)	as	C22																  \n"
                    + "		  			,max(Case SortNo when 23 then CategoryName	else	''	end)	as	C23																  \n"
                    + "		  			,max(Case SortNo when 24 then CategoryName	else	''	end)	as	C24																  \n"
                    + "		  			,max(Case SortNo when 25 then CategoryName	else	''	end)	as	C25																  \n"
                    + "		  			,max(Case SortNo when 26 then CategoryName	else	''	end)	as	C26																  \n"
                    + "		  			,max(Case SortNo when 27 then CategoryName	else	''	end)	as	C27																  \n"
                    + "		  			,max(Case SortNo when 28 then CategoryName	else	''	end)	as	C28																  \n"
                    + "		  			,max(Case SortNo when 29 then CategoryName	else	''	end)	as	C29																  \n"
                    + "		  			,max(Case SortNo when 30 then CategoryName	else	''	end)	as	C30																  \n"
                    + "		  from	AdTargetsHanaTV.dbo.category																		  \n"
                    + "		  where	Flag='Y'																		  \n"
                    + "		  union all																				  \n"
                    + "		  select	replace( Convert( varchar(16),Convert(money,3),1),'.00','')																  \n"
                    + "		  			,'���������(%)'																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,0),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.Tot),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C99),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C1 ),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C2 ),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C3 ),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C4 ),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C5 ),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C6 ),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C7 ),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C8 ),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C9 ),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C10),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C11),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C12),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C13),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C14),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C15),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C16),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C17),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C18),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C19),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C20),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C21),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C22),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C23),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C24),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C25),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C26),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C27),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C28),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C29),1),'.00','')																  \n"
                    + "		  			,replace( Convert( varchar(16),Convert(money,v.C30),1),'.00','')																  \n"
                    + "		from	(select	  case when A.Tot > 0 then cast( A.tot as real) / AdDatas.Tot * 100 else 0 end   as Tot                                       \n" //������ ���� �� 0������ �����߰�
                    + "                      ,case when A.C99 > 0 and AdDatas.C99 >0 then cast(A.C99 as real ) / AdDatas.C99 * 100 else 0 end      as C99                 \n"
                    + "                      ,case when A.C1  > 0 and AdDatas.C1  >0 then cast(A.C1  as real ) / AdDatas.C1  * 100 else 0 end      as C1                  \n"
                    + "                      ,case when A.C2  > 0 and AdDatas.C2  >0 then cast(A.C2  as real ) / AdDatas.C2  * 100 else 0 end      as C2                  \n"
                    + "                      ,case when A.C3  > 0 and AdDatas.C3  >0 then cast(A.C3  as real ) / AdDatas.C3  * 100 else 0 end      as C3                  \n"
                    + "                      ,case when A.C4  > 0 and AdDatas.C4  >0 then cast(A.C4  as real ) / AdDatas.C4  * 100 else 0 end      as C4                  \n"
                    + "                      ,case when A.C5  > 0 and AdDatas.C5  >0 then cast(A.C5  as real ) / AdDatas.C5  * 100 else 0 end      as C5                  \n"
                    + "                      ,case when A.C6  > 0 and AdDatas.C6  >0 then cast(A.C6  as real ) / AdDatas.C6  * 100 else 0 end      as C6                  \n"
                    + "                      ,case when A.C7  > 0 and AdDatas.C7  >0 then cast(A.C7  as real ) / AdDatas.C7  * 100 else 0 end      as C7                  \n"
                    + "                      ,case when A.C8  > 0 and AdDatas.C8  >0 then cast(A.C8  as real ) / AdDatas.C8  * 100 else 0 end      as C8                  \n"
                    + "                      ,case when A.C9  > 0 and AdDatas.C9  >0 then cast(A.C9  as real ) / AdDatas.C9  * 100 else 0 end      as C9                  \n"
                    + "                      ,case when A.C10 > 0 and AdDatas.C10 >0 then cast(A.C10 as real ) / AdDatas.C10 * 100 else 0 end      as C10                 \n"
                    + "                      ,case when A.C11 > 0 and AdDatas.C11 >0 then cast(A.C11 as real ) / AdDatas.C11 * 100 else 0 end      as C11                 \n"
                    + "                      ,case when A.C12 > 0 and AdDatas.C12 >0 then cast(A.C12 as real ) / AdDatas.C12 * 100 else 0 end      as C12                 \n"
                    + "                      ,case when A.C13 > 0 and AdDatas.C13 >0 then cast(A.C13 as real ) / AdDatas.C13 * 100 else 0 end      as C13                 \n"
                    + "                      ,case when A.C14 > 0 and AdDatas.C14 >0 then cast(A.C14 as real ) / AdDatas.C14 * 100 else 0 end      as C14                 \n"
                    + "                      ,case when A.C15 > 0 and AdDatas.C15 >0 then cast(A.C15 as real ) / AdDatas.C15 * 100 else 0 end      as C15                 \n"
                    + "                      ,case when A.C16 > 0 and AdDatas.C16 >0 then cast(A.C16 as real ) / AdDatas.C16 * 100 else 0 end      as C16                 \n"
                    + "                      ,case when A.C17 > 0 and AdDatas.C17 >0 then cast(A.C17 as real ) / AdDatas.C17 * 100 else 0 end      as C17                 \n"
                    + "                      ,case when A.C18 > 0 and AdDatas.C18 >0 then cast(A.C18 as real ) / AdDatas.C18 * 100 else 0 end      as C18                 \n"
                    + "                      ,case when A.C19 > 0 and AdDatas.C19 >0 then cast(A.C19 as real ) / AdDatas.C19 * 100 else 0 end      as C19                 \n"
                    + "                      ,case when A.C20 > 0 and AdDatas.C20 >0 then cast(A.C20 as real ) / AdDatas.C20 * 100 else 0 end      as C20                 \n"
                    + "                      ,case when A.C21 > 0 and AdDatas.C21 >0 then cast(A.C21 as real ) / AdDatas.C21 * 100 else 0 end      as C21                 \n"
                    + "                      ,case when A.C22 > 0 and AdDatas.C22 >0 then cast(A.C22 as real ) / AdDatas.C22 * 100 else 0 end      as C22                 \n"
                    + "                      ,case when A.C23 > 0 and AdDatas.C23 >0 then cast(A.C23 as real ) / AdDatas.C23 * 100 else 0 end      as C23                 \n"
                    + "                      ,case when A.C24 > 0 and AdDatas.C24 >0 then cast(A.C24 as real ) / AdDatas.C24 * 100 else 0 end      as C24                 \n"
                    + "                      ,case when A.C25 > 0 and AdDatas.C25 >0 then cast(A.C25 as real ) / AdDatas.C25 * 100 else 0 end      as C25                 \n"
                    + "                      ,case when A.C26 > 0 and AdDatas.C26 >0 then cast(A.C26 as real ) / AdDatas.C26 * 100 else 0 end      as C26                 \n"
                    + "                      ,case when A.C27 > 0 and AdDatas.C27 >0 then cast(A.C27 as real ) / AdDatas.C27 * 100 else 0 end      as C27                 \n"
                    + "                      ,case when A.C28 > 0 and AdDatas.C28 >0 then cast(A.C28 as real ) / AdDatas.C28 * 100 else 0 end      as C28                 \n"
                    + "                      ,case when A.C29 > 0 and AdDatas.C29 >0 then cast(A.C29 as real ) / AdDatas.C29 * 100 else 0 end      as C29                 \n"
                    + "                      ,case when A.C30 > 0 and AdDatas.C30 >0 then cast(A.C30 as real ) / AdDatas.C30 * 100 else 0 end      as C30                 \n"
                    + "            from       AdData as A, ( select * from AdData where ItemNo = 0 and ContSeq = 0 )  as AdDatas                                          \n"
                    + "				where       A.ItemNo = 1																											  \n"
                    + "				and			A.ContSeq = 0 ) v																										  \n"
                    + "UNION ALL																					  \n"
                    + "																								  \n"
                    + "-- �ְ����																					  \n"
                    + "select replace( Convert( varchar(16),Convert(money,AdData.ItemNo),1),'.00','')												  \n"
                    + "			,ItemName																											  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.ContSeq),1),'.00','')											  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.Tot),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C99),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C1 ),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C2 ),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C3 ),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C4 ),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C5 ),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C6 ),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C7 ),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C8 ),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C9 ),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C10),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C11),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C12),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C13),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C14),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C15),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C16),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C17),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C18),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C19),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C20),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C21),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C22),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C23),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C24),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C25),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C26),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C27),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C28),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C29),1),'.00','')												  \n"
                    + "			,replace( Convert( varchar(16),Convert(money,AdData.C30),1),'.00','')												  \n"
                    + "from		AdData																				  \n"
                    + "order by ContSeq,ItemNo																					  \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �����͸𵨿� ����
                rptAdCategorySummaryModel.ReportDataSet = ds.Copy();

                // ���
                rptAdCategorySummaryModel.ResultCnt = Utility.GetDatasetCount(rptAdCategorySummaryModel.ReportDataSet);

                ds.Dispose();

                // ����ڵ� ��Ʈ
                rptAdCategorySummaryModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + rptAdCategorySummaryModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetRptAdCategorySummary() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                rptAdCategorySummaryModel.ResultCD = "3000";
                if (isNotReady)
                {
                    rptAdCategorySummaryModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
                }
                else
                {
                    rptAdCategorySummaryModel.ResultDesc = "�ѱⰣ �������� ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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