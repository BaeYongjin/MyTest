// ===============================================================================
//
// SchChoiceAdBiz.cs
//
// �������� ��ó�� ���� 
//
// ===============================================================================
// Release history
// 2007.09.04 RH.Jung ������� ���Ұ� �������� ����ó��
// 2007.10.11 RH.Jung ��������� �����ι�ȣ�� �������� �ʾ� �������� �� �� ���� �������� ����
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// 

/*
 * -------------------------------------------------------
 * Class Name: SchChoiceAdBiz.cs
 * �ֿ���  : �������� ��ó�� ����
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : �躸��
 * ������    : 2013.05.27
 * ��������  :        
 *            - ���������� ��ȸ�ǵ��� ��������ȸ ������ ����
 * �����Լ�  :
 *            - GetAdList10()
 * -------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : YJ.PARK
 * ������    : 2014.08.8
 * ��������  :        
 *            - �� ���� ��� �߰�
 * �����Լ�  :
 *            - SetSchChoiceAdCopy()
 * -------------------------------------------------------
 */

using System;
using System.Data;
using System.Data.SqlClient;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Schedule
{
    /// <summary>
    /// SchChoiceAdBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class SchChoiceAdBiz : BaseBiz
    {
        #region ������
        public SchChoiceAdBiz()
            : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region �������� ����Ȳ ��ȸ
        /// <summary>
        /// �������� ����Ȳ ��ȸ(���� ��)
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetSchChoiceAdList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            bool isState = false;

            try
            {
                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchChoiceAdList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey            :[" + schChoiceAdModel.SearchKey + "]");		// �˻���
                _log.Debug("SearchMediaCode	     :[" + schChoiceAdModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchRapCode        :[" + schChoiceAdModel.SearchRapCode + "]");		// �˻� ��
                _log.Debug("SearchAgencyCode     :[" + schChoiceAdModel.SearchAgencyCode + "]");		// �˻� �����
                _log.Debug("SearchAdvertiserCode :[" + schChoiceAdModel.SearchAdvertiserCode + "]");		// �˻� ������
                _log.Debug("SearchContractState  :[" + schChoiceAdModel.SearchContractState + "]");		// �˻� ������
                _log.Debug("SearchAdClass        :[" + schChoiceAdModel.SearchAdClass + "]");		// �˻� ����뵵
                _log.Debug("SearchchkAdState_20  :[" + schChoiceAdModel.SearchchkAdState_20 + "]");		// �˻� ������� : ��
                _log.Debug("SearchchkAdState_30  :[" + schChoiceAdModel.SearchchkAdState_30 + "]");		// �˻� ������� : ����	
                _log.Debug("SearchchkAdState_40  :[" + schChoiceAdModel.SearchchkAdState_40 + "]");		// �˻� ������� : ����           

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n SELECT  B.ADVT_TYP  AS AdType");
                sbQuery.Append("\n     ,   GET_STMCODNM('26', B.ADVT_TYP)  AS AdTypeName       -- 26:��������");
                sbQuery.Append("\n     ,   A.ITEM_NO     AS ItemNo");
                sbQuery.Append("\n     ,   B.ITEM_NM     AS ItemName");
                sbQuery.Append("\n     ,   C.CNTR_NM     AS ContractName");
                sbQuery.Append("\n     ,   D.ADVTER_NM    AS AdvertiserName");
                sbQuery.Append("\n     ,   C.CNTR_STT    AS ContState");
                sbQuery.Append("\n     ,   GET_STMCODNM('23', C.CNTR_STT)  AS ContStateName        -- 23:������");
                sbQuery.Append("\n     ,   B.RL_END_DY   AS RealEndDay");
                sbQuery.Append("\n     ,   GET_STMCODNM('29', B.ADVT_CLSS) AS AdClassName         -- 29:����뵵");
                sbQuery.Append("\n     ,   B.ADVT_STT    AS AdState");
                sbQuery.Append("\n     ,   B.ADVT_RATE   AS AdRate");
                sbQuery.Append("\n     ,   GET_STMCODNM('25',B.ADVT_STT)   AS AdStatename          -- 25:�������");
                sbQuery.Append("\n     , ( SELECT COUNT(*) FROM SCHD_MENU  WHERE ITEM_NO = A.ITEM_NO) AS MenuCount");
                sbQuery.Append("\n     , ( SELECT COUNT(*) FROM SCHD_TITLE WHERE ITEM_NO = A.ITEM_NO) AS ChannelCount");
                sbQuery.Append("\n     ,   1             AS MediaCode");
                sbQuery.Append("\n     ,   B.FILE_STT    AS FileState");
                sbQuery.Append("\n     ,   GET_STMCODNM('31', B.FILE_STT)    AS FileStatename        -- 31. ���ϻ���");
                sbQuery.Append("\n     ,   'False' AS CheckYn");
                sbQuery.Append("\n     ,   GET_STMCODNM('27', B.SCH_TYP)     AS ScheduleTypeName     -- 27:������");
                sbQuery.Append("\n  FROM   ( -- ���Ǿ� �ִ� ��󱤰� �̾Ƴ���");
                sbQuery.Append("\n             SELECT   DISTINCT ITEM_NO FROM   SCHD_MENU");
                sbQuery.Append("\n             UNION ");
                sbQuery.Append("\n             SELECT   DISTINCT ITEM_NO FROM   SCHD_TITLE");
                sbQuery.Append("\n         ) A");
                sbQuery.Append("\n         INNER JOIN  ADVT_MST    B ON B.ITEM_NO  = A.ITEM_NO      -- ������");
                sbQuery.Append("\n         INNER JOIN  CNTR        C ON C.CNTR_SEQ = B.CNTR_SEQ    -- ������");
                sbQuery.Append("\n         LEFT  JOIN  ADVTER      D ON D.ADVTER_COD= C.ADVTER_COD   -- ������");
                sbQuery.Append("\n WHERE 1 = 1");

                if (schChoiceAdModel.AdType.Trim().Length > 0 && !schChoiceAdModel.AdType.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.ADVT_TYP  = " + schChoiceAdModel.AdType.Trim() + " \n");
                }

                // ���縦 ����������
                if (schChoiceAdModel.SearchRapCode.Trim().Length > 0 && !schChoiceAdModel.SearchRapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND C.REP_COD  = " + schChoiceAdModel.SearchRapCode.Trim() + " \n");
                }

                // ������� ���ÿ� ����
                // ������´� 20:�� �� 40:���� ���̿� �ִ� �͸� ��ȸ�Ѵ�.
                sbQuery.Append(" AND B.ADVT_STT >= '20' AND B.ADVT_STT <= '40' \n");

                // ������� ��
                if (schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( B.ADVT_STT  = '20' \n");
                    isState = true;
                }
                // ������� ����
                if (schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.ADVT_STT  = '30' \n");
                    isState = true;
                }
                // ������� ����
                if (schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.ADVT_STT  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");

                // �˻�� ������
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("\n"
                        + "  AND ( B.ITEM_NM    LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "     OR C.CNTR_NM	LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n");
                }
                sbQuery.Append("  ORDER BY A.ITEM_NO DESC");

                _log.Debug(sbQuery.ToString());

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                schChoiceAdModel.ResultCD = "0000";

                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchChoiceAdList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "��������Ȳ ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
        #endregion

        #region ������Ʈ
        /// <summary>
        /// ������ ��������
        /// �������������� ��ȸ������ ���������
        /// ������������ �������, ����� ������
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void GetAdList10(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            bool isState = false;

            try
            {
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdList10() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey            :[" + schChoiceAdModel.SearchKey + "]");		// �˻���
                _log.Debug("SearchMediaCode	     :[" + schChoiceAdModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchRapCode        :[" + schChoiceAdModel.SearchRapCode + "]");		// �˻� ��
                _log.Debug("SearchAgencyCode     :[" + schChoiceAdModel.SearchAgencyCode + "]");		// �˻� �����
                _log.Debug("SearchAdvertiserCode :[" + schChoiceAdModel.SearchAdvertiserCode + "]");		// �˻� ������
                _log.Debug("SearchContractState  :[" + schChoiceAdModel.SearchContractState + "]");		// �˻� ������
                _log.Debug("SearchAdClass        :[" + schChoiceAdModel.SearchAdClass + "]");		// �˻� ����뵵
                _log.Debug("SearchchkAdState_20  :[" + schChoiceAdModel.SearchchkAdState_20 + "]");		// �˻� ������� : ��
                _log.Debug("SearchchkAdState_30  :[" + schChoiceAdModel.SearchchkAdState_30 + "]");		// �˻� ������� : ����	
                _log.Debug("SearchchkAdState_40  :[" + schChoiceAdModel.SearchchkAdState_40 + "]");		// �˻� ������� : ����           

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                #region ������ ����
                /*
                sbQuery.Append("\n SELECT   A.ItemNo ");
                sbQuery.Append("\n 		,B.ItemName ");
                sbQuery.Append("\n 		,B.RealEndDay ");
                sbQuery.Append("\n 		,B.AdState ");
                sbQuery.Append("\n 		,G.CodeName AS AdStatename ");
                sbQuery.Append("\n 		,B.AdRate ");
                sbQuery.Append("\n 		,(select count(distinct Genre)");
                sbQuery.Append("\n 		  from (");
                sbQuery.Append("\n 				select  GenreCode as Genre");
                sbQuery.Append("\n 				from	SchChoiceMenuDetail with(noLock) ");
                sbQuery.Append("\n 				where	ItemNo = A.ItemNo");
                sbQuery.Append("\n 				union all");
                sbQuery.Append("\n 				select  Genre");
                sbQuery.Append("\n 				from	v_adv_contentset aa with(nolock)");
                sbQuery.Append("\n 				inner join SchChoiceChannelDetail bb with(nolock) on bb.ChannelNo = aa.Channel");
                sbQuery.Append("\n 				where	bb.ItemNo = A.ItemNo ) v ) as MenuCount ");
                sbQuery.Append("\n 		,( select count(*) from SchChoiceChannelDetail with(noLock) where ItemNo = A.ItemNo)	as ChannelCount ");
                sbQuery.Append("\n 		,( select count(*) from SchRate nolock where ItemNo = a.ItemNo )	as GroupCount ");
                sbQuery.Append("\n 		,( select isnull(sum(EntryRate),0) from SchRate nolock where ItemNo = a.ItemNo )	as GroupSum ");
                sbQuery.Append("\n 		,( select count(*) from SchRateDetail nolock where ItemNo = a.ItemNo )	as DetailCount ");
                sbQuery.Append("\n  	,B.AdType "); // [E_01]
                sbQuery.Append("\n 	    ,C.CodeName as AdTypeName "); // [E_01]
                sbQuery.Append("\n FROM (	SELECT ItemNo	FROM SchChoiceMenu with(NoLock) ");
                sbQuery.Append("\n         UNION ");
                sbQuery.Append("\n         SELECT ItemNo	FROM SchChoiceChannel with(NoLock) ) AS A ");
                sbQuery.Append("\n INNER JOIN ContractItem    B with(NoLock) ON (A.ItemNo         = B.ItemNo) ");
                sbQuery.Append("\n LEFT  JOIN SystemCode      G with(NoLock) ON (B.AdState        = G.Code AND G.Section = '25') ");
                sbQuery.Append("\n LEFT OUTER JOIN SystemCode C with(NoLock) ON (B.AdType         = C.Code AND C.Section = '26') "); // [E_01]
                sbQuery.Append("\n WHERE b.AdType in('10','19') ");

                // ��ü�� ����������
                if(schChoiceAdModel.SearchMediaCode.Trim().Length > 0 && !schChoiceAdModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.MediaCode  = " + schChoiceAdModel.SearchMediaCode.Trim() + " \n");
                }	
				
                // ���縦 ����������
                if(schChoiceAdModel.SearchRapCode.Trim().Length > 0 && !schChoiceAdModel.SearchRapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.RapCode  = " + schChoiceAdModel.SearchRapCode.Trim() + " \n");
                }	

                //// ����縦 ���������� - ��ȸ���� ������
                //if(schChoiceAdModel.SearchAgencyCode.Trim().Length > 0 && !schChoiceAdModel.SearchAgencyCode.Trim().Equals("00"))
                //{
                //    sbQuery.Append(" AND B.AgencyCode  = " + schChoiceAdModel.SearchAgencyCode.Trim() + " \n");
                //}	

                //// �����ָ� ����������
                //if(schChoiceAdModel.SearchAdvertiserCode.Trim().Length > 0 && !schChoiceAdModel.SearchAdvertiserCode.Trim().Equals("00"))
                //{
                //    sbQuery.Append(" AND B.AdvertiserCode  = " + schChoiceAdModel.SearchAdvertiserCode.Trim() + " \n");
                //}	


                // ������� ���ÿ� ����
                // ������´� 20:�� �� 40:���� ���̿� �ִ� �͸� ��ȸ�Ѵ�.
                sbQuery.Append(" AND B.AdState >= '20' AND B.AdState <= '40' \n");

                // ������� ��
                if(schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( B.AdState  = '20' \n");
                    isState = true;
                }	
                // ������� ����
                if(schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '30' \n");
                    isState = true;
                }	
                // ������� ����
                if(schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '40' \n");
                    isState = true;
                }	

                if(isState) sbQuery.Append(" ) \n");
				
                // �˻�� ������
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("\n"
                        + "  AND B.ItemName  LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%'" );
                }
       
                sbQuery.Append("  ORDER BY A.ItemNo DESC");
                */
                #endregion
                sbQuery.Append("\n  SELECT   a.item_no as ItemNo            ");
                sbQuery.Append("\n          ,b.item_nm as ItemName          ");
                sbQuery.Append("\n          ,b.rl_end_dy as RealEndDay      ");
                sbQuery.Append("\n          ,b.advt_stt as AdState          ");
                sbQuery.Append("\n          ,g.stm_cod_nm as AdStatename    ");
                sbQuery.Append("\n          ,b.advt_rate as AdRate          ");
                sbQuery.Append("\n          ,( select count(distinct Genre) ");
                sbQuery.Append("\n          from (                          ");
                sbQuery.Append("\n              select  menu_cod as Genre, x.item_no    ");
                sbQuery.Append("\n              from	SCHD_MENU x                     ");
                sbQuery.Append("\n              union all                               ");
                sbQuery.Append("\n              select  y.title_no as Genre, y.item_no  ");
                sbQuery.Append("\n              from	  SCHD_TITLE y                  ");
                sbQuery.Append("\n          ) v                             ");
                sbQuery.Append("\n          where v.item_no = a.item_no     ");
                sbQuery.Append("\n          ) as MenuCount                  ");
                sbQuery.Append("\n          ,( select count(*) from SCHD_TITLE   WHERE item_no = a.item_no)	as ChannelCount   ");
                sbQuery.Append("\n          ,( select count(*) from SCHDRT_MST WHERE item_no = a.item_no )	as GroupCount   ");
                sbQuery.Append("\n          ,( select NVL(SUM(entry_rt),0) from SCHDRT_MST WHERE item_no = a.item_no )	as GroupSum ");
                sbQuery.Append("\n          ,( select count(*) from SCHDRT_DTL WHERE item_no = a.item_no )	as DetailCount ");
                sbQuery.Append("\n          ,b.advt_typ as AdType           ");
                sbQuery.Append("\n          ,c.stm_cod_nm as AdTypeName     ");
                sbQuery.Append("\n  FROM                                    ");
                sbQuery.Append("\n  (	SELECT item_no FROM SCHD_MENU       ");
                sbQuery.Append("\n      UNION                               ");
                sbQuery.Append("\n      SELECT item_no FROM SCHD_TITLE      ");
                sbQuery.Append("\n  )   a                                   ");
                sbQuery.Append("\n  INNER JOIN ADVT_MST       b  ON (a.item_no  = b.item_no)    ");
                sbQuery.Append("\n  INNER JOIN CNTR           k ON (b.cntr_seq = k.cntr_seq)    ");
                sbQuery.Append("\n  LEFT  JOIN STM_COD          g  ON (b.advt_stt   = g.stm_cod AND g.stm_cod_cls = '25')   ");
                sbQuery.Append("\n  LEFT OUTER JOIN STM_COD c  ON (b.advt_typ  = c.stm_cod AND c.stm_cod_cls = '26')    ");
                sbQuery.Append("\n  WHERE b.advt_typ IN('10' ,'19')         ");

                // ��ü�� ����������
                if (schChoiceAdModel.SearchMediaCode.Trim().Length > 0 && !schChoiceAdModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND k.mda_cod  = " + schChoiceAdModel.SearchMediaCode.Trim() + " \n");
                }

                // ���縦 ����������
                if (schChoiceAdModel.SearchRapCode.Trim().Length > 0 && !schChoiceAdModel.SearchRapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND k.rep_cod  = " + schChoiceAdModel.SearchRapCode.Trim() + " \n");
                }


                // ������� ���ÿ� ����
                // ������´� 20:�� �� 40:���� ���̿� �ִ� �͸� ��ȸ�Ѵ�.
                sbQuery.Append(" AND B.advt_stt >= '20' AND B.advt_stt <= '40' \n");

                // ������� ��
                if (schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( B.advt_stt  = '20' \n");
                    isState = true;
                }
                // ������� ����
                if (schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.advt_stt  = '30' \n");
                    isState = true;
                }
                // ������� ����
                if (schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.advt_stt  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");

                // �˻�� ������
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("\n"
                        + "  AND B.item_nm  LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%'");
                }

                sbQuery.Append("  ORDER BY b.item_nm DESC");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �帣�׷�𵨿� ����
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // ���
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // ����ڵ� ��Ʈ
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdList10() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }

        }
        #endregion

        #region �������� ���ϸ�� ��ġ�� �����ϱ� ���� �Լ�

        /// <summary>
        /// �������� �������ȸ
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetInspectItemList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetInspectItemList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey            :[" + schChoiceAdModel.SearchKey + "]");		// �˻���
                _log.Debug("SearchMediaCode	     :[" + schChoiceAdModel.SearchMediaCode + "]");		// �˻� ��ü

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + "SELECT A.ItemNo                      \n"
                    + "      ,A.ItemName                    \n"
                    + "      ,B.ContractName                \n"
                    + "      ,C.AdvertiserName              \n"
                    + "      ,A.ExcuteStartDay              \n"
                    + "      ,A.ExcuteEndDay                \n"
                    + "      ,A.RealEndDay                  \n"
                    + "      ,A.AdState                     \n"
                    + "      ,D.CodeName AdStateName        \n"
                    + "      ,(SELECT COUNT(*) FROM SchHome with(NoLock)                WHERE ItemNo = A.ItemNo) AS HomeCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail with(NoLock)    WHERE ItemNo = A.ItemNo) AS MenuCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock) WHERE ItemNo = A.ItemNo) AS ChannelCount   \n"
                    + "      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay                                                \n"
                    + "      ,A.AdType                      \n"
                    + "      ,E.CodeName AS AdTypeName      \n"
                    + "      ,A.FilePath      \n"
                    + "      ,A.FileName      \n"
                    + "  FROM ContractItem A INNER JOIN Contract   B with(NoLock) ON (B.MediaCode      = B.MediaCode AND A.RapCode = B.RapCode AND A.AgencyCode = B.AgencyCode AND A.AdvertiserCode = B.AdvertiserCode AND A.ContractSeq = B.ContractSeq) \n"
                    + "                       LEFT JOIN Advertiser C with(NoLock) ON (B.AdvertiserCode = C.AdvertiserCode)                \n"
                    + "	                      LEFT JOIN SystemCode D with(NoLock) ON (A.AdState        = D.Code      AND D.Section = '25')  \n"  // 25 : �������
                    + "                       LEFT JOIN SystemCode E with(NoLock) ON (A.AdType         = E.Code      AND E.Section = '26')  \n"	// 26 : ��������
                    + " WHERE A.AdClass IN ('10','30')   \n"    // ����뵵 AdClass 10:Ȩ���� 30:����

                    );

                // ��ü�� ����������
                if (schChoiceAdModel.SearchMediaCode.Trim().Length > 0 && !schChoiceAdModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND A.MediaCode  = '" + schChoiceAdModel.SearchMediaCode.Trim() + "' \n");
                }

                bool isState = false;
                // ������� ���ÿ� ����

                // ������� �غ�
                if (schChoiceAdModel.SearchchkAdState_10.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.AdState  = '10' \n");
                    isState = true;
                }
                // ������� ��
                if (schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '20' \n");
                    isState = true;
                }
                // ������� ����
                if (schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '30' \n");
                    isState = true;
                }
                // ������� ����
                if (schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");



                // �˻�� ������
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("\n"
                        + "  AND ( A.ItemName       LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.ContractName   LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY A.ItemNo DESC ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �帣�׷�𵨿� ����
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // ���
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // ����ڵ� ��Ʈ
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetInspectItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "�������� �������ȸ �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }
        #endregion

        #region �������� �������ȸ

        /// <summary>
        /// �������� �������ȸ
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetContractItemList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey            :[" + schChoiceAdModel.SearchKey + "]");	// �˻���
                _log.Debug("SearchMediaCode	     :[" + schChoiceAdModel.SearchMediaCode + "]");		// �˻� ��ü
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + "SELECT 'False' AS CheckYn            \n"
                    + "      ,A.ItemNo                      \n"
                    + "      ,A.ItemName                    \n"
                    + "      ,B.ContractName                \n"
                    + "      ,C.AdvertiserName              \n"
                    + "      ,A.ExcuteStartDay              \n"
                    + "      ,A.ExcuteEndDay                \n"
                    + "      ,A.RealEndDay                  \n"
                    + "      ,A.AdState                     \n"
                    + "      ,D.CodeName AdStateName        \n"
                    + "      ,(SELECT COUNT(*) FROM SchHome with(NoLock)                WHERE ItemNo = A.ItemNo) AS HomeCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail with(NoLock)    WHERE ItemNo = A.ItemNo) AS MenuCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock) WHERE ItemNo = A.ItemNo) AS ChannelCount   \n"
                    + "      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay                                                \n"
                    + "      ,A.AdType                      \n"
                    + "      ,E.CodeName AS AdTypeName      \n"
                    + "  FROM ContractItem A INNER JOIN Contract   B with(NoLock) ON (B.MediaCode      = B.MediaCode AND A.RapCode = B.RapCode AND A.AgencyCode = B.AgencyCode AND A.AdvertiserCode = B.AdvertiserCode AND A.ContractSeq = B.ContractSeq) \n"
                    + "                       LEFT JOIN Advertiser C with(NoLock) ON (B.AdvertiserCode = C.AdvertiserCode)                \n"
                    + "	                      LEFT JOIN SystemCode D with(NoLock) ON (A.AdState        = D.Code      AND D.Section = '25')  \n"  // 25 : �������
                    + "                       LEFT JOIN SystemCode E with(NoLock) ON (A.AdType         = E.Code      AND E.Section = '26')  \n"	// 26 : ��������
                    + " WHERE 1=1   \n"
                    );

                // ��ü�� ����������
                if (schChoiceAdModel.SearchMediaCode.Trim().Length > 0 && !schChoiceAdModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND A.MediaCode  = '" + schChoiceAdModel.SearchMediaCode.Trim() + "' \n");
                }

                bool isState = false;
                // ������� ���ÿ� ����

                // ������� �غ�
                if (schChoiceAdModel.SearchchkAdState_10.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.AdState  = '10' \n");
                    isState = true;
                }
                // ������� ��
                if (schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '20' \n");
                    isState = true;
                }
                // ������� ����
                if (schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '30' \n");
                    isState = true;
                }
                // ������� ����
                if (schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");



                // �˻�� ������
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("\n"
                        + "  AND ( A.ItemName       LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.ContractName   LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY A.ItemNo DESC ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �帣�׷�𵨿� ����
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // ���
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // ����ڵ� ��Ʈ
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "�������� �������ȸ �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }

        /// <summary>
        /// ���������, ���������� �� �������� �������� ã�ƿ´� 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void GetContractItemList_0907a(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey            :[" + schChoiceAdModel.SearchKey + "]");	// �˻���
                _log.Debug("SearchMediaCode	     :[" + schChoiceAdModel.SearchMediaCode + "]");		// �˻� ��ü
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + "SELECT 'False' AS CheckYn            \n"
                    + "      ,A.ItemNo                      \n"
                    + "      ,A.ItemName                    \n"
                    + "      ,B.ContractName                \n"
                    + "      ,C.AdvertiserName              \n"
                    + "      ,A.ExcuteStartDay              \n"
                    + "      ,A.ExcuteEndDay                \n"
                    + "      ,A.RealEndDay                  \n"
                    + "      ,A.AdState                     \n"
                    + "      ,D.CodeName AdStateName        \n"
                    + "      ,(SELECT COUNT(*) FROM SchHome with(NoLock)                WHERE ItemNo = A.ItemNo) AS HomeCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail with(NoLock)    WHERE ItemNo = A.ItemNo) AS MenuCount      \n"
                    + "      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock) WHERE ItemNo = A.ItemNo) AS ChannelCount   \n"
                    + "      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay                                                \n"
                    + "      ,A.AdType                      \n"
                    + "      ,E.CodeName AS AdTypeName      \n"
                    + "  FROM ContractItem A with(noLock) INNER JOIN Contract   B with(NoLock) ON (B.MediaCode      = B.MediaCode AND A.RapCode = B.RapCode AND A.AgencyCode = B.AgencyCode AND A.AdvertiserCode = B.AdvertiserCode AND A.ContractSeq = B.ContractSeq) \n"
                    + "                       LEFT JOIN Advertiser C with(NoLock) ON (B.AdvertiserCode = C.AdvertiserCode)                \n"
                    + "	                      LEFT JOIN SystemCode D with(NoLock) ON (A.AdState        = D.Code      AND D.Section = '25') \n"  // 25 : �������
                    + "                       LEFT JOIN SystemCode E with(NoLock) ON (A.AdType         = E.Code      AND E.Section = '26') \n"  // 26 : ��������
                    + "  WHERE 1=1 \n");
                //+ " WHERE ItemNo not in( select distinct ItemNo   \n"   
                //+ "                      from ( select ItemNo from SchChoiceMenu noLock \n"   
                //+ "                             union all \n"   
                //+ "                             select ItemNo from SchChoiceChannel noLock ) a ) \n");
                //+ " AND AdType In(10,11,12,16,20) \n" );
                if (schChoiceAdModel.SearchAdType.Length > 0 && schChoiceAdModel.SearchAdType.Trim().Equals("000"))
                {
                    sbQuery.Append(" AND a.AdType In(10,11,12,16,17,19,20) \n");
                }
                else if (schChoiceAdModel.SearchAdType.Length > 0 && schChoiceAdModel.SearchAdType.Trim().Equals("100"))
                {
                    // ��������
                    sbQuery.Append(" AND a.AdType In(10,16,17,19) \n");
                }
                else if (schChoiceAdModel.SearchAdType.Length > 0 && schChoiceAdModel.SearchAdType.Trim().Equals("200"))
                {
                    // ��ü�����
                    sbQuery.Append(" AND a.AdType In(11,12,20) \n");
                }
                else
                {
                    sbQuery.Append(" AND a.AdType = " + schChoiceAdModel.SearchAdType.Trim() + " \n");
                }

                // ��ü�� ����������
                if (schChoiceAdModel.SearchMediaCode.Trim().Length > 0 && !schChoiceAdModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND A.MediaCode  = '" + schChoiceAdModel.SearchMediaCode.Trim() + "' \n");
                }

                bool isState = false;
                // ������� ���ÿ� ����

                // ������� �غ�
                if (schChoiceAdModel.SearchchkAdState_10.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.AdState  = '10' \n");
                    isState = true;
                }
                // ������� ��
                if (schChoiceAdModel.SearchchkAdState_20.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '20' \n");
                    isState = true;
                }
                // ������� ����
                if (schChoiceAdModel.SearchchkAdState_30.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '30' \n");
                    isState = true;
                }
                // ������� ����
                if (schChoiceAdModel.SearchchkAdState_40.Trim().Length > 0 && schChoiceAdModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");



                // �˻�� ������
                if (schChoiceAdModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("\n"
                        + "  AND ( A.ItemName       LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.ContractName   LIKE '%" + schChoiceAdModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY A.ItemNo DESC ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �帣�׷�𵨿� ����
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // ���
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // ����ڵ� ��Ʈ
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "�������� �������ȸ �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }
        #endregion


        #region ����ä�α��� ��

        /// <summary>
        /// ����ä�α��� ��
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() Start");
                _log.Debug("-----------------------------------------");


                // ��������
                try
                {
                    int i = 0;
                    int rc = 0;
                    int adSchType = 0;

                    if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                    {
                        adSchType = 10;
                    }
                    else
                        adSchType = 20;

                    // ���� ���ι�ȣ�� ����
                    string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                    StringBuilder sbQuery = new StringBuilder();
                    SqlParameter[] sqlParams = new SqlParameter[1];

                    // �Ķ���� ��Ʈ
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                    // ������ �Ǿ��ִ��� �˻�
                    sbQuery.Append("\n"
                        + "SELECT * FROM SchChoiceChannel  \n"
                        + " WHERE ItemNo =  @ItemNo         \n"
                        );

                    // ��������
                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    int cnt = Utility.GetDatasetCount(ds);

                    ds.Dispose();

                    // ���
                    // 0���̸� ������ ����
                    if (cnt == 0)
                    {
                        _db.BeginTran();

                        // ����ä�� �� ���̺� �߰�
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceChannel(  \n"
                            + "                ItemNo         \n"
                            + "      ) VALUES (               \n"
                            + "                @ItemNo        \n"
                            + "      )                        \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        // ������� ������ ����
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "UPDATE ContractItem        \n"
                            + "   SET AdState = '20'      \n"   // ������� - 20:��
                            + "      ,ModDt   = GETDATE() \n"
                            + "      ,RegID   = '" + header.UserID + "' \n"
                            + " WHERE ItemNo  = @ItemNo   \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        _db.CommitTran();


                        // __MESSAGE__
                        _log.Message("�ű��� ����ä�α���:[" + schChoiceAdModel.ItemName + "] �����:[" + header.UserID + "]");
                    }
                    else
                    {
                        _log.Message("���� ����ä�α���:[" + schChoiceAdModel.ItemName + "]");
                    }

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "����ä�α��� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }
        #endregion

        #region ����ä�α��� ����
        /// <summary>
        /// ����ä�α���  ����
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete() Start");
                _log.Debug("-----------------------------------------");


                // ��������
                try
                {
                    int i = 0;
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();
                    SqlParameter[] sqlParams = new SqlParameter[2];

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);
                    sqlParams[i++].Value = Convert.ToInt16(schChoiceAdModel.MediaCode);

                    // ����ä�α��� �� �� ���̺��� �ش� ���� �ִ� ä���� ��ȸ�Ѵ�.
                    sbQuery.Append("\n"
                        + "SELECT ChannelNo                            \n"
                        + "  FROM SchChoiceChannelDetail with(nolock) \n"
                        + " WHERE ItemNo    = @ItemNo       \n"
                        + "   AND MediaCode = @MediaCode    \n"
                        + " ORDER BY ChannelNo           \n"
                        );

                    DataSet ds = _db.ExecuteQueryParams(sbQuery.ToString(), sqlParams);

                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        DataRow row = ds.Tables[0].Rows[j];
                        string ChannelNo = row["ChannelNo"].ToString();

                        schChoiceAdModel.ChannelNo = ChannelNo;

                        // �� ä�κ��� �󼼳����� �����Ѵ�.
                        SetSchChoiceChannelDetailDelete2(header, schChoiceAdModel);
                    }
                    ds.Dispose();


                    _db.BeginTran();

                    // �����޴����� �� ���̺��� ����
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceChannel          \n"
                        + " WHERE ItemNo        = " + schChoiceAdModel.ItemNo + " \n"
                        );

                    _log.Debug(sbQuery.ToString());

                    rc = _db.ExecuteNonQuery(sbQuery.ToString());

                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("����ä�α��� ����:[" + schChoiceAdModel.ItemName + "] �����:[" + header.UserID + "]" + rc.ToString());

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "����ä�α��� ������ ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        #endregion

        #region ����ä�α��� ���� ��ȸ

        /// <summary>
        /// ����ä�α��� ���� ��ȸ
        /// </summary>
        /// <returns></returns>
        public void GetSchChoiceChannelDetailList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceChannelDetailList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                OracleParameter[] sqlParams = new OracleParameter[1];

                sqlParams[0] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                sqlParams[0].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // ����ä�� �� ���̺� �߰�
                sbQuery.Append("\n SELECT 'False'		AS CheckYn");
                sbQuery.Append("\n     ,   A.TITLE_NO  AS ChannelNo");
                sbQuery.Append("\n     ,   T.TITLE_NM  AS Title");
                sbQuery.Append("\n     ,   C.ACK_STT   AS AckState");
                sbQuery.Append("\n     ,   0           AS Hits         -- ������� ����");
                sbQuery.Append("\n     ,   ''          AS ProdType     -- ���Ŀ�, �������ϰ� �����ؾ� ��");
                sbQuery.Append("\n FROM    SCHD_TITLE A");
                sbQuery.Append("\n     INNER JOIN ADVT_MST         B ON B.ITEM_NO  = A.ITEM_NO");
                sbQuery.Append("\n     INNER JOIN TITLE_COD        T ON T.TITLE_NO = A.TITLE_NO");
                sbQuery.Append("\n     LEFT  JOIN SCHD_DIST_MST    C ON C.ACK_NO   = A.ACK_NO");
                sbQuery.Append("\n WHERE   A.ITEM_NO = :ItemNo");
                sbQuery.Append("\n ORDER BY T.TITLE_NM");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                // ��� DataSet�� �𵨿� ����
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                schChoiceAdModel.ResultCD = "0000";  // ����

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceChannelDetailList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "Ÿ��Ʋ ���� ��ȸ �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        #endregion

        #region ����ä�α��� ���� ����
        /// <summary>
        /// ����ä�α��� ���� ����
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            int err = 0;

            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("MediaCode         :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("ChannelNo         :[" + schChoiceAdModel.ChannelNo + "]");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("AdType            :[" + schChoiceAdModel.AdType + "]");
                // __DEBUG__				

                int i = 0;
                int rc = 0;
                int adSchType = 0;

                if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                {
                    adSchType = 10;
                }
                else
                    adSchType = 20;

                // ���� ���ι�ȣ�� ����
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                StringBuilder sbQuery = new StringBuilder();

                // �ش� ä���� �����ϴ��� �˻�
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT *           \n"
                    + "  FROM Channel with(NoLock)     \n"
                    + " WHERE MediaCode  = " + schChoiceAdModel.MediaCode + " \n"
                    + "   AND ChannelNo  = " + schChoiceAdModel.ChannelNo + " \n"
                    );

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                rc = Utility.GetDatasetCount(ds);

                // ���
                // 0���̸� ä���� �������� �ʴ°��̴�.
                if (rc == 0)
                {
                    ds.Dispose();
                    err = 1;	// ä���� �������� ����
                    throw new Exception();
                }
                ds.Dispose();


                string AdType = schChoiceAdModel.AdType;	// �������� : 10:CM 11:EAP(10~19�� �ʼ�����) 20:OAP 

                #region [����: ����ä�� ���Ұ�]
                // 2007.09.04 ������� �� �Ұ� ������ ó��
                // 2010,01.01 ����ä�� �� ���� �ɼ����� ó����
                // �ش� ä���� �������� �˻��Ͽ� �������(�ʼ�����) �����θ� �����Ѵ�.
                // if(AdType.StartsWith("1")) // 10~19���̰� �ʼ������̴�. �ڵ尡 1�� ������ �����? ��.��
                // ������� 10��
                //                if(AdType.Equals("10")) // 10~19���̰� �ʼ������̴�. �ڵ尡 1�� ������ �����? ��.��
                //				{
                //					// �ش� ä���� �����ϴ��� �˻�
                //					sbQuery   = new StringBuilder();
                //					sbQuery.Append( "\n"
                //						+ "SELECT *   \n"
                //						+ "  FROM Channel A with(NoLock) INNER JOIN Contents B with(NoLock) ON (A.ContentID = B.ContentID) \n"
                //						+ " WHERE A.MediaCode  = " + schChoiceAdModel.MediaCode + " \n"
                //						+ "   AND A.ChannelNo  = " + schChoiceAdModel.ChannelNo + " \n"
                //						+ "   AND ProdType IS NOT NULL     \n"
                //						+ "   AND ProdType <> '' \n"
                //						);
                //
                //					// ��������
                //					ds = new DataSet();
                //					_db.ExecuteQuery(ds,sbQuery.ToString());
                //				
                //					rc = Utility.GetDatasetCount(ds);
                //				
                //					// ���
                //					// 1���̶� �ִٸ� ä�ο� �ʼ����� ���� �� ����.
                //					if(rc > 0)
                //					{							
                //						err = 2;
                //						ds.Dispose();
                //						throw new Exception();
                //					}
                //					ds.Dispose();
                //				}
                #endregion

                SqlParameter[] sqlParams = new SqlParameter[1];

                //����ä�������̺� �����Ͱ� ���� ��� Insert
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT ItemNo FROM SchChoiceChannel with(NoLock)  \n"
                    + " WHERE ItemNo    =  @ItemNo           \n"
                    );

                i = 0;
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // ��������
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                rc = Utility.GetDatasetCount(ds);
                ds.Dispose();

                // ���
                // 0���̸� ����ä�������̿� ItemNo Insert
                if (rc == 0)
                {
                    // ��������
                    try
                    {
                        _db.BeginTran();


                        // ����ä�� �� ���̺� �߰�
                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceChannel ( \n"
                            + "            ItemNo            \n"
                            + "       )                      \n"
                            + "       VALUES (               \n"
                            + "           @ItemNo            \n"
                            + "       )                      \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        // ������� ������ ����
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "UPDATE ContractItem        \n"
                            + "   SET AdState = '20'      \n"   // ������� - 20:��
                            + "      ,ModDt   = GETDATE() \n"
                            + "      ,RegID   = '" + header.UserID + "' \n"
                            + " WHERE ItemNo  = @ItemNo   \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);


                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("�ű��� ����ä�α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] �����:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }


                sqlParams = new SqlParameter[3];

                i = 0;
                sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@ChannelNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ChannelNo);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // ������ �Ǿ��ִ��� �˻�
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT * FROM SchChoiceChannelDetail  \n"
                    + " WHERE MediaCode =  @MediaCode        \n"
                    + "   AND ChannelNo =  @ChannelNo        \n"
                    + "   AND ItemNo    =  @ItemNo           \n"
                    );

                // ��������
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                rc = Utility.GetDatasetCount(ds);

                ds.Dispose();

                // ���
                // 0���̸� ������ ����
                if (rc == 0)
                {
                    // ��������
                    try
                    {
                        _db.BeginTran();

                        // ����ä�� �� ���̺� �߰�
                        sbQuery = new StringBuilder();


                        if (AdType.StartsWith("1")) // 10~19���̰� �ʼ������̴�. �ڵ尡 1�� ������ �����? ��.��
                        {
                            sbQuery.Append("\n"
                                + "INSERT INTO SchChoiceChannelDetail (      \n"
                                + "            MediaCode                     \n"
                                + "           ,ChannelNo                     \n"
                                + "           ,ItemNo                        \n"
                                + "           ,AckNo                         \n"
                                + "           ,ScheduleOrder                 \n"
                                + "       )                                  \n"
                                + "       VALUES (                           \n"
                                + "           @MediaCode                     \n"
                                + "          ,@ChannelNo                     \n"
                                + "          ,@ItemNo                        \n"
                                + "          ," + AckNo + "  \n"
                                + "          ,0                              \n" // �ʼ������� ������ 0
                                + "       )                                  \n"
                                );
                        }
                        else
                        {
                            sbQuery.Append("\n"
                                + "INSERT INTO SchChoiceChannelDetail (      \n"
                                + "            MediaCode                     \n"
                                + "           ,ChannelNo                     \n"
                                + "           ,ItemNo                        \n"
                                + "           ,AckNo                         \n"
                                + "           ,ScheduleOrder         \n"
                                + "       )                                  \n"
                                + "       SELECT                             \n"
                                + "           @MediaCode                     \n"
                                + "          ,@ChannelNo                     \n"
                                + "          ,@ItemNo                        \n"
                                + "          ," + AckNo + "  \n"
                                + "          ,ISNULL(MAX(ScheduleOrder),0)+1 \n" // �ɼǱ���(OAP��)�� ������ MAX+1
                                + "      FROM SchChoiceChannelDetail         \n"
                                + "      WHERE MediaCode = @MediaCode        \n"
                                + "        AND Channelno = @ChannelNo        \n"
                                );
                        }

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("�ű��� ����ä�α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] �����:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    // __MESSAGE__
                    _log.Message("���� ����ä�α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] �����:[" + header.UserID + "]");
                }


                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                if (err == 1)
                {
                    schChoiceAdModel.ResultDesc = "�Է��Ͻ� ä���� �������� �ʴ� ä�ι�ȣ�Դϴ�.";
                }
                else if (err == 2)
                {
                    schChoiceAdModel.ResultDesc = "�ش� ä���� �����������̹Ƿ� ������� ���� �����մϴ�.";
                }
                else
                {
                    schChoiceAdModel.ResultDesc = "����ä�α��� ���� ���� �� ������ �߻��Ͽ����ϴ�";
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

        #region ����ä�α��� ���� ����
        /// <summary>
        /// ����ä�α��� ���� ����
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                SetSchChoiceChannelDetailDelete2(header, schChoiceAdModel);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        public void SetSchChoiceChannelDetailDelete2(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete2() Start");
                _log.Debug("-----------------------------------------");


                // ��������
                try
                {
                    int i = 0;
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();
                    SqlParameter[] sqlParams = new SqlParameter[3];

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                    sqlParams[i++] = new SqlParameter("@ChannelNo", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ChannelNo);
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                    int adSchType = 0;

                    if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                    {
                        adSchType = 10;
                    }
                    else
                        adSchType = 20;

                    // ���� ���ι�ȣ�� ����
                    string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                    // ������ ����
                    string DeleteOrder = "0";

                    // �ش� ������ �׸��� ���� ����
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + " SELECT ScheduleOrder           \n"
                        + "  FROM SchChoiceChannelDetail   \n"
                        + " WHERE MediaCode = @MediaCode   \n"
                        + "   AND ChannelNo = @ChannelNo   \n"
                        + "   AND ItemNo    = @ItemNo      \n"
                        );

                    // ��������
                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    if (Utility.GetDatasetCount(ds) != 0)
                    {
                        DeleteOrder = Utility.GetDatasetString(ds, 0, "ScheduleOrder");
                    }

                    ds.Dispose();

                    _db.BeginTran();

                    // ����ä�� �� ���̺� ����
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceChannelDetail   \n"
                        + " WHERE MediaCode = @MediaCode   \n"
                        + "   AND ChannelNo = @ChannelNo   \n"
                        + "   AND ItemNo    = @ItemNo      \n"
                        );

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    sbQuery.Length = 0;
                    // ����ä�� �� ���̺��� �����Ǿ� �� �̻� �������� ������ ������ �Ѵ�.
                    sbQuery.Append("WITH CA (CategoryCode, GenreCode)	\n");
                    sbQuery.Append("AS (	\n");
                    sbQuery.Append("	SELECT	\n");
                    sbQuery.Append("		B.CategoryCode, A.GenreCode--CategoryCode, GenreCode	\n");
                    sbQuery.Append("	FROM SchChoiceMenuDetail A	\n");
                    sbQuery.Append("	JOIN Category B ON B.MediaCode = A.MediaCode	\n");
                    sbQuery.Append("		AND B.CategoryCode = (SELECT TOP 1 CategoryCode FROM ChannelSet WHERE GenreCode = A.GenreCode)	\n");
                    sbQuery.Append("	WHERE ItemNo = @ItemNo	\n");
                    sbQuery.Append("	UNION	\n");
                    sbQuery.Append("	SELECT	\n");
                    sbQuery.Append("		B.CategoryCode, B.GenreCode	\n");
                    sbQuery.Append("	FROM SchChoiceChannelDetail A	\n");
                    sbQuery.Append("	JOIN ChannelSet B WITH (NOLOCK) ON B.ChannelNo = A.ChannelNo	\n");
                    sbQuery.Append("	JOIN Category C WITH (NOLOCK) ON C.MediaCode = A.MediaCode AND C.CategoryCode = B.CategoryCode	\n");
                    sbQuery.Append("	WHERE ItemNo = @ItemNo	\n");
                    sbQuery.Append("	AND B.SeriesNo = (SELECT MIN(SeriesNo) FROM ChannelSet WHERE MediaCode = B.MediaCode AND ChannelNo = B.ChannelNo)	\n");
                    sbQuery.Append(")	\n");
                    sbQuery.Append("DELETE SchRateDetail	\n");
                    sbQuery.Append("--SELECT *	\n");
                    sbQuery.Append("FROM SchRateDetail A	\n");
                    sbQuery.Append("WHERE	\n");
                    sbQuery.Append("	NOT EXISTS (	\n");
                    sbQuery.Append("		SELECT 1 FROM CA	\n");
                    sbQuery.Append("		WHERE MediaCode = A.MediaCode AND ItemNo = A.ItemNo	\n");
                    sbQuery.Append("			AND CategoryCode = A.CategoryCode AND GenreCode = A.GenreCode)	\n");
                    sbQuery.Append("	AND MediaCode = @MediaCode	\n");
                    sbQuery.Append("	AND ItemNo = @ItemNo	\n");
                    //					sbQuery.Append("DELETE SchRate	\n");
                    //					sbQuery.Append("--SELECT *	\n");
                    //					sbQuery.Append("FROM SchRate A	\n");
                    //					sbQuery.Append("WHERE	\n");
                    //					sbQuery.Append("	NOT EXISTS (	\n");
                    //					sbQuery.Append("		SELECT 1 FROM SchRateDetail	\n");
                    //					sbQuery.Append("		WHERE MediaCode = A.MediaCode AND ItemNo = A.ItemNo	\n");
                    //					sbQuery.Append("			AND EntrySeq = A.EntrySeq)	\n");
                    //					sbQuery.Append("	AND MediaCode = @MediaCode	\n");
                    //					sbQuery.Append("	AND ItemNo = @ItemNo	\n");
                    _log.Debug(sbQuery.ToString());

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    // ������ 0�� �ƴ� �Ϳ� ���Ͽ�
                    if (!DeleteOrder.Equals("0"))
                    {
                        // �ش� �������� ū ������ �������� -1�Ͽ� ����
                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n"
                            + "UPDATE SchChoiceChannelDetail                              \n"
                            + "   SET ScheduleOrder = ScheduleOrder - 1                   \n"
                            + "      ,AckNo         = " + AckNo + " \n"
                            + " WHERE MediaCode     = " + schChoiceAdModel.MediaCode + " \n"
                            + "   AND ChannelNo     = " + schChoiceAdModel.ChannelNo + " \n"
                            + "   AND ScheduleOrder > " + DeleteOrder + " \n"
                            + "   AND ScheduleOrder > 0                                       \n"
                            );

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());
                    }
                    _db.CommitTran();

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete2() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "����ä�α��� ���� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
        }

        #endregion

        #region ����ä�α��� ��������

        /// <summary>
        /// ����ä�α��� ��������
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelDelete_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete_To() Start");
                _log.Debug("-----------------------------------------");


                // ��������
                try
                {
                    int i = 0;
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();
                    SqlParameter[] sqlParams = new SqlParameter[1];

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);

                    _db.BeginTran();

                    // ����ä�α��� �� �� ���̺��� ����
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceChannelDetail            \n"
                        + " WHERE MediaCode        = @MediaCode        \n"
                        );

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    // ����ä�α��� �� ���̺��� ����
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceChannel                  \n"
                        + " WHERE ItemNo  IN (SELECT ItemNo FROM ContractItem WHERE MediaCode = @MediaCode)         \n"
                        );

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("�����޴����� ����:[" + schChoiceAdModel.MediaCode + "] �����:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDelete_To() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "����ä�α��� ������ ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        #endregion

        #region [S1]�������� ȸ�� ���� ����
        /// <summary>
        /// �������� ȸ�� ���� ����
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceSeriesDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            int err = 0;

            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("MediaCode   :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("ChannelNo   :[" + schChoiceAdModel.ChannelNo + "]");
                _log.Debug("ItemNo      :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("AdType      :[" + schChoiceAdModel.AdType + "]");

                #region �����ι�ȣ ���ϱ�
                int rc = 0;
                int adSchType = 0;

                if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                {
                    adSchType = 10;
                }
                else
                    adSchType = 20;

                // ���� ���ι�ȣ�� ����
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                StringBuilder sbQuery = new StringBuilder();
                DataSet ds = new DataSet();

                string AdType = schChoiceAdModel.AdType;	// �������� : 10:CM 11:EAP(10~19�� �ʼ�����) 20:OAP 
                #endregion

                #region �ʼ������� Ȯ��
                // 2007.09.04 ������� �� �Ұ� ������ ó��
                // �ش� ä���� �������� �˻��Ͽ� �������(�ʼ�����) �����θ� �����Ѵ�.
                // if(AdType.StartsWith("1")) // 10~19���̰� �ʼ������̴�. �ڵ尡 1�� ������ �����? ��.��
                // ������� 10��
                if (AdType.Equals("10")) // 10~19���̰� �ʼ������̴�. �ڵ尡 1�� ������ �����? ��.��
                {
                    // �ش� ä���� �����ϴ��� �˻�
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT *    ");
                    sbQuery.Append("\n FROM Channel A with(NoLock)  ");
                    sbQuery.Append("\n INNER JOIN Contents B with(NoLock) ON A.ContentID = B.ContentID  ");
                    sbQuery.Append("\n WHERE A.MediaCode  = " + schChoiceAdModel.MediaCode);
                    sbQuery.Append("\n AND	 A.ChannelNo  = " + schChoiceAdModel.ChannelNo);
                    sbQuery.Append("\n AND	 A.SeriesNo	  = " + schChoiceAdModel.SeriesNo);
                    sbQuery.Append("\n AND   B.ProdType IS NOT NULL ");
                    sbQuery.Append("\n AND   B.ProdType <> '' ");
                    ds = new DataSet();
                    _db.ExecuteQuery(ds, sbQuery.ToString());
                    rc = Utility.GetDatasetCount(ds);

                    // ���
                    // 1���̶� �ִٸ� ä�ο� �ʼ����� ���� �� ����.
                    if (rc > 0)
                    {
                        err = 2;
                        ds.Dispose();
                        throw new Exception();
                    }
                    ds.Dispose();
                }
                #endregion

                #region �������� Ȯ��
                SqlParameter[] sqlParams = new SqlParameter[5];
                sqlParams[0] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[1] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[2] = new SqlParameter("@ChannelNo", SqlDbType.Int);
                sqlParams[3] = new SqlParameter("@SeriesNo", SqlDbType.Int);
                sqlParams[4] = new SqlParameter("@AckNo", SqlDbType.Int);

                sqlParams[0].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);
                sqlParams[1].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);
                sqlParams[2].Value = Convert.ToInt32(schChoiceAdModel.ChannelNo);
                sqlParams[3].Value = Convert.ToInt32(schChoiceAdModel.SeriesNo);
                sqlParams[4].Value = AckNo;

                // ������ �Ǿ��ִ��� �˻�
                sbQuery = new StringBuilder();
                sbQuery.Append("\n SELECT * FROM SchChoiceSeriesDetail with(noLock) ");
                sbQuery.Append("\n WHERE	ItemNo		= @ItemNo ");
                sbQuery.Append("\n AND		MediaCode	=  @MediaCode ");
                sbQuery.Append("\n AND		ChannelNo	=  @ChannelNo ");
                sbQuery.Append("\n AND		SeriesNo	= @SeriesNo; ");

                // ��������
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);
                rc = Utility.GetDatasetCount(ds);
                ds.Dispose();
                #endregion

                #region �� �߰�
                // 0���̸� ������ ����
                if (rc == 0)
                {
                    // ��������
                    try
                    {
                        _db.BeginTran();

                        // ����ä�� �� ���̺� �߰�
                        sbQuery = new StringBuilder();
                        if (AdType.StartsWith("1")) // 10~19���̰� �ʼ������̴�. �ڵ尡 1�� ������ �����? ��.��
                        {
                            sbQuery.Append("\n"
                                + "INSERT INTO SchChoiceSeriesDetail (	\n"
                                + "            MediaCode                \n"
                                + "           ,ChannelNo                \n"
                                + "           ,SeriesNo                 \n"
                                + "           ,ItemNo                   \n"
                                + "           ,AckNo                    \n"
                                + "           ,ScheduleOrder            \n"
                                + "       )                             \n"
                                + " VALUES (  @MediaCode,@ChannelNo,@SeriesNo,@ItemNo,@AckNo,0 )");
                        }
                        else
                        {
                            sbQuery.Append("\n INSERT INTO SchChoiceSeriesDetail ");
                            sbQuery.Append("\n				(    MediaCode ");
                            sbQuery.Append("\n					,ChannelNo ");
                            sbQuery.Append("\n					,SeriesNo ");
                            sbQuery.Append("\n					,ItemNo ");
                            sbQuery.Append("\n					,AckNo ");
                            sbQuery.Append("\n					,ScheduleOrder ) ");
                            sbQuery.Append("\n SELECT	@MediaCode   ");
                            sbQuery.Append("\n 		,@ChannelNo  ");
                            sbQuery.Append("\n 		,@SeriesNo	 ");
                            sbQuery.Append("\n         ,@ItemNo     ");
                            sbQuery.Append("\n         ,@AckNo		 ");
                            sbQuery.Append("\n         ,ISNULL(MAX(ScheduleOrder),0)+1  ");
                            sbQuery.Append("\n FROM SchChoiceSeriesDetail noLock ");
                            sbQuery.Append("\n WHERE	ItemNo		= @ItemNo ");
                            sbQuery.Append("\n AND		MediaCode	=  @MediaCode ");
                            sbQuery.Append("\n AND		ChannelNo	=  @ChannelNo ");
                            sbQuery.Append("\n AND		SeriesNo	= @SeriesNo; ");
                        }

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("�ű��� ����ä�α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] �����:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    // __MESSAGE__
                    _log.Message("���� ����ä�α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] �����:[" + header.UserID + "]");
                }
                #endregion


                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                if (err == 1)
                {
                    schChoiceAdModel.ResultDesc = "�Է��Ͻ� ȸ���� �������� �ʴ� ȸ����ȣ�Դϴ�.";
                }
                else if (err == 2)
                {
                    schChoiceAdModel.ResultDesc = "�ش� ȸ���� �����������̹Ƿ� ������� ���� �����մϴ�.";
                }
                else
                {
                    schChoiceAdModel.ResultDesc = "�������� ȸ���� ���� �� ������ �߻��Ͽ����ϴ�";
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


        #region �޴��������� ��ȸ(���� �������� ���°͸� �μ�Ʈ ��Ű�� ����)
        /// <summary>
        /// Ȩ��������ȸ
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        public void SetSchChoiceSearch(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetSchChoiceSearch() Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + " SELECT   \n"
                    + "        MediaCode             \n"
                    + "       ,GenreCode             \n"
                    + "       ,ItemNo                \n"
                    + "       ,AckNo                 \n"
                    + "       ,ScheduleOrder         \n"
                    + "		FROM SchChoiceMenuDetail                \n"
                    );


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� ī�װ��𵨿� ����
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // ���
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // ����ڵ� ��Ʈ
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetSchChoiceSearch() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "�޴����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }
        #endregion

        #region ä�α������� ��ȸ(���� �������� ���°͸� �μ�Ʈ ��Ű�� ����)
        /// <summary>
        /// Ȩ��������ȸ
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        public void SetSchChoiceChannelSearch(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetSchChoiceChannelSearch() Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + " SELECT   \n"
                    + "        MediaCode             \n"
                    + "       ,ChannelNo             \n"
                    + "       ,ItemNo                \n"
                    + "       ,AckNo                 \n"
                    + "       ,ScheduleOrder         \n"
                    + "		FROM SchChoiceChannelDetail                \n"
                    );


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� ī�װ��𵨿� ����
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                // ���
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                // ����ڵ� ��Ʈ
                schChoiceAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetSchChoiceChannelSearch() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "ä�α��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }
        #endregion

        #region ����ä�α��� ���� ��������
        /// <summary>
        /// ����ä�α��� ���� ��������
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceChannelDetailCreate_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            int err = 0;

            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDetailCreate_To() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("MediaCode         :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("ChannelNo         :[" + schChoiceAdModel.ChannelNo + "]");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("AdType            :[" + schChoiceAdModel.AdType + "]");
                // __DEBUG__				

                int i = 0;
                int rc = 0;
                int adSchType = 0;

                if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                {
                    adSchType = 10;
                }
                else
                    adSchType = 20;

                // ���� ���ι�ȣ�� ����
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);
                StringBuilder sbQuery = new StringBuilder();

                // �ش� ä���� �����ϴ��� �˻�
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT *           \n"
                    + "  FROM Channel     \n"
                    + " WHERE MediaCode  = " + schChoiceAdModel.MediaCode + " \n"
                    + "   AND ChannelNo  = " + schChoiceAdModel.ChannelNo + " \n"
                    );

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                rc = Utility.GetDatasetCount(ds);

                // ���
                // 0���̸� ä���� �������� �ʴ°��̴�.
                if (rc == 0)
                {
                    ds.Dispose();
                    err = 1;	// ä���� �������� ����
                    throw new Exception();
                }
                ds.Dispose();


                string AdType = schChoiceAdModel.AdType;	// �������� : 10:CM 11:EAP(10~19�� �ʼ�����) 20:OAP 

                // 2007.09.04 ������� �� �Ұ� ������ ó��
                // �ش� ä���� �������� �˻��Ͽ� �������(�ʼ�����) �����θ� �����Ѵ�.
                if (AdType.Equals("10")) // 10~19���̰� �ʼ������̴�. �ڵ尡 1�� ������ �����? ��.��
                {
                    // �ش� ä���� �����ϴ��� �˻�
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + "SELECT *   \n"
                        + "  FROM Channel A INNER JOIN Contents B ON (A.ContentID = B.ContentID) \n"
                        + " WHERE A.MediaCode  = " + schChoiceAdModel.MediaCode + " \n"
                        + "   AND A.ChannelNo  = " + schChoiceAdModel.ChannelNo + " \n"
                        + "   AND ProdType IS NOT NULL     \n"
                        + "   AND ProdType <> '' \n"
                        );

                    // ��������
                    ds = new DataSet();
                    _db.ExecuteQuery(ds, sbQuery.ToString());

                    rc = Utility.GetDatasetCount(ds);

                    // ���
                    // 1���̶� �ִٸ� ä�ο� �ʼ����� ���� �� ����.
                    if (rc > 0)
                    {
                        err = 2;
                        ds.Dispose();
                        throw new Exception();
                    }
                    ds.Dispose();


                }


                SqlParameter[] sqlParams = new SqlParameter[1];

                //����ä�������̺� �����Ͱ� ���� ��� Insert
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT ItemNo FROM SchChoiceChannel  \n"
                    + " WHERE ItemNo    =  @ItemNo           \n"
                    );

                i = 0;
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // ��������
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                rc = Utility.GetDatasetCount(ds);
                ds.Dispose();

                // ���
                // 0���̸� ����ä�������̿� ItemNo Insert
                if (rc == 0)
                {
                    // ��������
                    try
                    {
                        _db.BeginTran();


                        // ����ä�� �� ���̺� �߰�
                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceChannel ( \n"
                            + "            ItemNo            \n"
                            + "       )                      \n"
                            + "       VALUES (               \n"
                            + "           @ItemNo            \n"
                            + "       )                      \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        // ������� ������ ����
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "UPDATE ContractItem        \n"
                            + "   SET AdState = '20'      \n"   // ������� - 20:��
                            + "      ,ModDt   = GETDATE() \n"
                            + "      ,RegID   = '" + header.UserID + "' \n"
                            + " WHERE ItemNo  = @ItemNo   \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);


                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("�ű��� ����ä�α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] �����:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }


                sqlParams = new SqlParameter[4];

                i = 0;
                sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@ChannelNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@ScheduleOrder", SqlDbType.SmallInt);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ChannelNo);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);
                sqlParams[i++].Value = Convert.ToInt16(schChoiceAdModel.ScheduleOrder);

                // ������ �Ǿ��ִ��� �˻�
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT * FROM SchChoiceChannelDetail  \n"
                    + " WHERE MediaCode =  @MediaCode        \n"
                    + "   AND ChannelNo =  @ChannelNo        \n"
                    + "   AND ItemNo    =  @ItemNo           \n"
                    );

                // ��������
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                rc = Utility.GetDatasetCount(ds);

                ds.Dispose();

                // ���
                // 0���̸� ������ ����
                if (rc == 0)
                {
                    // ��������
                    try
                    {
                        _db.BeginTran();

                        // ����ä�� �� ���̺� �߰�
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceChannelDetail (      \n"
                            + "            MediaCode                     \n"
                            + "           ,ChannelNo                     \n"
                            + "           ,ItemNo                        \n"
                            + "           ,AckNo                         \n"
                            + "           ,ScheduleOrder                 \n"
                            + "       )                                  \n"
                            + "       VALUES (                           \n"
                            + "           @MediaCode                     \n"
                            + "          ,@ChannelNo                     \n"
                            + "          ,@ItemNo                        \n"
                            + "          ," + AckNo + "  \n"
                            + "          ,@ScheduleOrder                              \n" // �ʼ������� ������ 0
                            + "       )                                  \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("�ű��� ����ä�α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] �����:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    // __MESSAGE__
                    _log.Message("���� ����ä�α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] �����:[" + header.UserID + "]");
                }


                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDetailCreate_To() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                if (err == 1)
                {
                    schChoiceAdModel.ResultDesc = "�Է��Ͻ� ä���� �������� �ʴ� ä�ι�ȣ�Դϴ�.";
                }
                else if (err == 2)
                {
                    schChoiceAdModel.ResultDesc = "�ش� ä���� �����������̹Ƿ� ������� ���� �����մϴ�.";
                }
                else
                {
                    schChoiceAdModel.ResultDesc = "����ä�α��� ���� ���� �� ������ �߻��Ͽ����ϴ�";
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


        #region �޴����� ��

        /// <summary>
        /// �޴��� 1�� �۾�
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceMenuCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuCreate() Start");
                _log.Debug("-----------------------------------------");

                try
                {
                    int rc = 0;
                    int adSchType = 0;

                    adSchType = 10;

                    // ���� ���ι�ȣ�� ����
                    string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                    StringBuilder sbQuery = new StringBuilder();
                    OracleParameter[] sqlParams = new OracleParameter[2];

                    // �Ķ���� ��Ʈ
                    sqlParams[0] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                    sqlParams[1] = new OracleParameter(":AckNo", OracleDbType.Int32);
                    sqlParams[0].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);
                    sqlParams[1].Value = Convert.ToInt32(AckNo);

                    // ������ �Ǿ��ִ��� �˻�
                    sbQuery.AppendFormat("\n SELECT * FROM SCHD_MENU WHERE ITEM_NO = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));
                    DataSet ds = new DataSet();
                    _db.ExecuteQuery(ds, sbQuery.ToString());

                    int cnt = Utility.GetDatasetCount(ds);

                    // 0���̸� ������ ����
                    if (cnt == 0)
                    {
                        _db.BeginTran();

                        // �����޴����� �� ���̺� �߰�
                        sbQuery.Clear();
                        sbQuery.Append("\n INSERT INTO SCHD_MENU");
                        sbQuery.Append("\n		  (	ITEM_NO, MENU_COD, MENU_COD_PAR, ACK_NO, SCHD_ORD )");
                        sbQuery.AppendFormat("\n VALUES ( {0}, '0000000000','0000000000',{1}, 0 )", Convert.ToInt32(schChoiceAdModel.ItemNo), Convert.ToInt32(AckNo));

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());

                        // �����޴����� �� ���̺� �߰�
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n UPDATE  ADVT_MST");
                        sbQuery.Append("\n SET		ADVT_STT	= '20'");		// ������� - 20:��
                        sbQuery.Append("\n		,	DT_UPDATE	= SYSDATE");
                        sbQuery.Append("\n		,	ID_UPDATE   = '" + header.UserID + "'");
                        sbQuery.Append("\n WHERE	ITEM_NO     = " + schChoiceAdModel.ItemNo);

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());
                        _db.CommitTran();
                        _log.Message("�ű��� �����޴�����:[" + schChoiceAdModel.ItemName + "] �����:[" + header.UserID + "]");
                    }
                    else
                    {
                        _log.Message("���� �����޴�����:[" + schChoiceAdModel.ItemName + "]");
                    }
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // ����
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "�����޴����� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// �޴����� ����
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceMenuDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDelete() Start");
                _log.Debug("-----------------------------------------");

                try
                {
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();

                    // �����޴����� �� �� ���̺��� �ش� ���� �ִ� �޴��� ��ȸ�Ѵ�.
                    sbQuery.AppendFormat("\n SELECT  MENU_COD");
                    sbQuery.AppendFormat("\n FROM    SCHD_MENU");
                    sbQuery.AppendFormat("\n WHERE   ITEM_NO = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));
                    sbQuery.AppendFormat("\n ORDER BY MENU_COD");

                    DataSet ds = _db.ExecuteQuery(sbQuery.ToString());

                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        DataRow row = ds.Tables[0].Rows[j];
                        string GenreCode = row["MENU_COD"].ToString();

                        schChoiceAdModel.GenreCode = GenreCode;

                        // �� �޴����� �󼼳����� �����Ѵ�.
                        SetSchChoiceMenuDetailDelete2(header, schChoiceAdModel);
                    }
                    ds.Dispose();

                    // __MESSAGE__
                    _log.Message("�����޴����� ����:[" + schChoiceAdModel.ItemName + "] �����:[" + header.UserID + "]" + rc.ToString());

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "�޴����� ������ ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        #endregion

        #region �����޴����� ���� ��ȸ

        /// <summary>
        /// �����޴����� ���� ��ȸ
        /// </summary>
        /// <returns></returns>
        public void GetSchChoiceMenuDetailList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceMenuDetailList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                // __DEBUG__

                // ��������
                StringBuilder sbQuery = new StringBuilder();
                OracleParameter[] sqlParams = new OracleParameter[1];

                sqlParams[0] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                sqlParams[0].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // �޴� �� ���̺�
                sbQuery.Append("\n SELECT  'False'     AS CheckYn");
                sbQuery.Append("\n     ,   CA.MENU_NM  AS CategoryName");
                sbQuery.Append("\n     ,   GE.MENU_NM  AS GenreName");
                sbQuery.Append("\n     ,   A.MENU_COD  AS GenreCode");
                sbQuery.Append("\n     ,   C.ACK_STT   AS AckState");
                sbQuery.Append("\n   FROM SCHD_MENU A");
                sbQuery.Append("\n         INNER JOIN ADVT_MST B       ON B.ITEM_NO    = A.ITEM_NO");
                sbQuery.Append("\n         INNER JOIN MENU_COD CA      ON CA.MENU_COD  = A.MENU_COD_PAR");
                sbQuery.Append("\n         INNER JOIN MENU_COD GE      ON GE.MENU_COD  = A.MENU_COD");
                sbQuery.Append("\n         LEFT  JOIN SCHD_DIST_MST C  ON C.ACK_NO     = A.ACK_NO");
                sbQuery.Append("\n WHERE   A.ITEM_NO = :ItemNo");
                sbQuery.Append("\n ORDER BY CategoryName, GenreName");

                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                // ��� DataSet�� �𵨿� ����
                schChoiceAdModel.ScheduleDataSet = ds.Copy();
                schChoiceAdModel.ResultCnt = Utility.GetDatasetCount(schChoiceAdModel.ScheduleDataSet);
                schChoiceAdModel.ResultCD = "0000";  // ����

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + schChoiceAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceMenuDetailList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "�޴����� ���� ��ȸ �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
        #endregion

        #region �����޴����� ���� ����

        /// <summary>
        /// �����޴����� ���� ����
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceMenuDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDetailCreate() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("MediaCode         :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("GenreCode         :[" + schChoiceAdModel.GenreCode + "]");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("AdType            :[" + schChoiceAdModel.AdType + "]");
                // __DEBUG__				
                int rc = 0;
                int adSchType = 0;
                adSchType = 10;
                //if(  schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                //{
                //	adSchType = 10;
                //}
                //else
                //	adSchType = 20;

                // ���� ���ι�ȣ�� ����
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                StringBuilder sbQuery = new StringBuilder();

                //����ä�������̺� �����Ͱ� ���� ��� Insert
                sbQuery.AppendFormat("\n SELECT ITEM_NO FROM SCHD_MENU WHERE ITEM_NO = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));
                //_log.Debug(sbQuery.ToString());

                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ���, 0���̸� ����ä�������̿� ItemNo Insert
                if (Utility.GetDatasetCount(ds) == 0)
                {
                    // ��������
                    try
                    {
                        _db.BeginTran();

                        sbQuery.Clear();
                        sbQuery.Append("\n INSERT INTO SCHD_MENU");
                        sbQuery.Append("\n		  (	ITEM_NO, MENU_COD, MENU_COD_PAR, ACK_NO, SCHD_ORD )");
                        sbQuery.AppendFormat("\n VALUES ( {0}, '0000000000','0000000000',{1}, 0 )", Convert.ToInt32(schChoiceAdModel.ItemNo), Convert.ToInt32(AckNo));

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());

                        // �����޴����� �� ���̺� �߰�
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n UPDATE  ADVT_MST");
                        sbQuery.Append("\n SET		ADVT_STT	= '20'");		// ������� - 20:��
                        sbQuery.Append("\n		,	DT_UPDATE	= SYSDATE");
                        sbQuery.Append("\n		,	ID_UPDATE   = '" + header.UserID + "'");
                        sbQuery.Append("\n WHERE	ITEM_NO     = " + schChoiceAdModel.ItemNo);

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());
                        _db.CommitTran();
                        _log.Message("�ű��� �����޴�����:[" + schChoiceAdModel.ItemName + "] �����:[" + header.UserID + "]");

                        // __MESSAGE__
                        _log.Message("�ű��� ����ä�α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] �����:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }

                // ������ �Ǿ��ִ��� �˻�
                sbQuery = new StringBuilder();
                sbQuery.AppendFormat("\n SELECT * FROM SCHD_MENU");
                sbQuery.AppendFormat("\n WHERE	MENU_COD = {0}", schChoiceAdModel.GenreCode);
                sbQuery.AppendFormat("\n AND	ITEM_NO  = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));

                _log.Debug(sbQuery.ToString());
                ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ���, 0���̸� ������ ���������� ����ó��
                if (Utility.GetDatasetCount(ds) == 0)
                {
                    try
                    {
                        _db.BeginTran();

                        sbQuery = new StringBuilder();
                        sbQuery.AppendFormat("\n INSERT INTO SCHD_MENU");
                        sbQuery.AppendFormat("\n 			( ITEM_NO, MENU_COD, MENU_COD_PAR, ACK_NO, SCHD_ORD )");
                        sbQuery.AppendFormat("\n VALUES ( {0},{1},{2},{3},{4} )", schChoiceAdModel.ItemNo, schChoiceAdModel.GenreCode, schChoiceAdModel.CategoryCode, AckNo, 0);

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());

                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("�ű��� �����޴�����:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] �����:[" + header.UserID + "]");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        // __DEBUG__
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    // __MESSAGE__
                    _log.Message("���� �����޴�����:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] �����:[" + header.UserID + "]");
                }

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDetailCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "�����޴����� ���� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        /// <summary>
        /// ���α��� ���� ����
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceRealChDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceRealChDetailCreate() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("Ch		:[" + schChoiceAdModel.GenreCode + "]");
                _log.Debug("ItemNo	:[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("AdType	:[" + schChoiceAdModel.AdType + "]");
                // __DEBUG__				
                int rc = 0;
                int adSchType = 0;
                adSchType = 10;

                // ���� ���ι�ȣ�� ����
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                StringBuilder sbQuery = new StringBuilder();
                DataSet ds = new DataSet();

                // ������ �Ǿ��ִ��� �˻�
                sbQuery = new StringBuilder();
                sbQuery.AppendFormat("\n SELECT * FROM SCHD_CHNL");
                sbQuery.AppendFormat("\n WHERE	CH_NO    = {0}", schChoiceAdModel.GenreCode);
                sbQuery.AppendFormat("\n AND	ITEM_NO  = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));

                _log.Debug(sbQuery.ToString());
                ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ���, 0���̸� ������ ���������� ����ó��
                if (Utility.GetDatasetCount(ds) == 0)
                {
                    try
                    {
                        _db.BeginTran();

                        sbQuery = new StringBuilder();
                        sbQuery.AppendFormat("\n INSERT INTO SCHD_CHNL");
                        sbQuery.AppendFormat("\n 			( ITEM_NO, CH_NO, ACK_NO, SCHD_ORD )");
                        sbQuery.AppendFormat("\n VALUES ( {0},{1},{2},{3})", schChoiceAdModel.ItemNo, schChoiceAdModel.GenreCode, AckNo, 0);

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());

                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n UPDATE  ADVT_MST");
                        sbQuery.Append("\n SET		ADVT_STT	= '20'");	// ������� - 20:��
                        sbQuery.Append("\n		,	DT_UPDATE	= SYSDATE");
                        sbQuery.Append("\n		,	ID_UPDATE   = '" + header.UserID + "'");
                        sbQuery.Append("\n WHERE	ITEM_NO     = " + schChoiceAdModel.ItemNo);
                        sbQuery.Append("\n AND		ADVT_STT    = '10'");	// ���°� ����� ��쿡�� ����

                        rc = _db.ExecuteNonQuery(sbQuery.ToString());

                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("�ű��� ���α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] �����:[" + header.UserID + "]");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        // __DEBUG__
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    _log.Message("���� ���α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] �����:[" + header.UserID + "]");
                }

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceRealChDetailCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "���α��� ���� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        #endregion

        #region �����޴����� ���� ����

        /// <summary>
        /// �����޴����� ���� ����
        /// </summary>
        /// <returns></returns>
        ///
        public void SetSchChoiceMenuDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                SetSchChoiceMenuDetailDelete2(header, schChoiceAdModel);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        public void SetSchChoiceRealChDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _db.Open();
                SetSchChoiceRealChDetailDelete2(header, schChoiceAdModel);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// �޴����� 1�Ǵ����� �����ϱ�
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceMenuDetailDelete2(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDetailDelete() Start");
                _log.Debug("-----------------------------------------");

                // ��������
                try
                {
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();
                    OracleParameter[] sqlParams = new OracleParameter[2];

                    sqlParams[0] = new OracleParameter(":GenreCode", OracleDbType.Varchar2, 10);
                    sqlParams[1] = new OracleParameter(":ItemNo", OracleDbType.Int32);

                    sqlParams[0].Value = schChoiceAdModel.GenreCode;
                    sqlParams[1].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                    // ������ ����
                    string DeleteOrder = "0";

                    // �ش� ������ �׸��� ���� ����
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT	SCHD_ORD				");
                    sbQuery.Append("\n FROM	SCHD_MENU				");
                    sbQuery.Append("\n WHERE	MENU_COD = :GenreCode	");
                    sbQuery.Append("\n AND		ITEM_NO  = :ItemNo		");

                    _db.BeginTran();
                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    if (Utility.GetDatasetCount(ds) != 0)
                    {
                        DeleteOrder = Utility.GetDatasetString(ds, 0, "SCHD_ORD");
                    }
                    ds.Dispose();


                    // �����޴� �� ���̺��� ����
                    sbQuery = new StringBuilder();
                    sbQuery.AppendFormat("\n DELETE	SCHD_MENU				");
                    sbQuery.AppendFormat("\n WHERE	MENU_COD = {0}", schChoiceAdModel.GenreCode);
                    //sbQuery.AppendFormat("\n AND    MENU_COD_PAR = {0}", schChoiceAdModel.CategoryCode);
                    sbQuery.AppendFormat("\n AND	ITEM_NO  = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));

                    rc = _db.ExecuteNonQuery(sbQuery.ToString());
                    _log.Debug(sbQuery.ToString());

                    //// ������ ����
                    //sbQuery.Append( "\n"
                    //    + "DELETE SchRateDetail    \n"
                    //    + " WHERE MediaCode = "+ schChoiceAdModel.MediaCode +" \n" 
                    //    + "   AND GenreCode = "+ schChoiceAdModel.GenreCode +" \n"
                    //    + "   AND ItemNo    = "+ schChoiceAdModel.ItemNo +"    \n"
                    //    );

                    //rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    //_log.Debug(sbQuery.ToString());


                    // ������ 0�� �ƴ� �Ϳ� ���Ͽ�
                    //if(!DeleteOrder.Equals("0"))
                    //{
                    //    // �ش� �������� ū ������ �������� -1�Ͽ� ����
                    //    sbQuery   = new StringBuilder();
                    //    sbQuery.Append( "\n"
                    //        + "UPDATE SchChoiceMenuDetail                                 \n"
                    //        + "   SET ScheduleOrder = ScheduleOrder - 1                   \n"
                    //        + "      ,AckNo         = " + AckNo                       + " \n"
                    //        + " WHERE MediaCode     = " + schChoiceAdModel.MediaCode  + " \n"
                    //        + "   AND GenreCode     = " + schChoiceAdModel.GenreCode  + " \n"
                    //        + "   AND ScheduleOrder > " + DeleteOrder + " \n"
                    //        + "   AND ScheduleOrder > 0                                       \n"
                    //        );

                    //    _log.Debug(sbQuery.ToString());

                    //    rc = _db.ExecuteNonQuery(sbQuery.ToString());

                    //}	

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                _db.CommitTran();
                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceChannelDetailDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "�����޴����� ���� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }

        }

        /// <summary>
        /// �������� 1�Ǵ����� �����ϱ�
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceRealChDetailDelete2(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceRealChDetailDelete2() Start");
                _log.Debug("-----------------------------------------");

                // ��������
                try
                {
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();

                    // �����޴� �� ���̺��� ����
                    sbQuery = new StringBuilder();
                    sbQuery.AppendFormat("\n DELETE	SCHD_CHNL				");
                    sbQuery.AppendFormat("\n WHERE	CH_NO	= {0}", schChoiceAdModel.GenreCode);
                    sbQuery.AppendFormat("\n AND	ITEM_NO = {0}", Convert.ToInt32(schChoiceAdModel.ItemNo));

                    _db.BeginTran();
                    rc = _db.ExecuteNonQuery(sbQuery.ToString());
                    _log.Debug(sbQuery.ToString());
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                _db.CommitTran();
                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceRealChDetailDelete2() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "�����޴����� ���� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }

        }

        #endregion


        #region ���������� ��ȸ

        /// <summary>
        /// ���������� ��ȸ
        /// </summary>
        /// <returns></returns>
        public void GetSchChoiceLastItemNo(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceLastItemNo() Start");
                _log.Debug("-----------------------------------------");


                // ��������
                try
                {
                    StringBuilder sbQuery = new StringBuilder();

                    // ������ ����
                    string LastItemNo = "1";

                    // �ش� ������ ���� ����
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + " SELECT ISNULL(MAX(TA.ItemNo),1) AS LastItemNo                \n"
                        + "  FROM (                                                     \n"
                        + "        SELECT ISNULL(MAX(A.ItemNo),1) AS ItemNo               \n"
                        + "          FROM SchChoiceMenu    A INNER JOIN ContractItem B  \n"
                        + "                                          ON (A.ItemNo = B.ItemNo AND B.MediaCode = " + schChoiceAdModel.MediaCode + ") \n"
                        + "        UNION                                                \n"
                        + "        SELECT ISNULL(MAX(A.ItemNo),1) AS ItemNo               \n"
                        + "          FROM SchChoiceChannel A INNER JOIN ContractItem B  \n"
                        + "                                          ON (A.ItemNo = B.ItemNo AND B.MediaCode = " + schChoiceAdModel.MediaCode + ") \n"
                        + "       ) TA \n"
                        );

                    //__DEBUG__
                    _log.Debug(sbQuery.ToString());
                    //__DEBUG__

                    // ��������
                    DataSet ds = new DataSet();
                    _db.ExecuteQuery(ds, sbQuery.ToString());


                    if (Utility.GetDatasetCount(ds) != 0)
                    {
                        LastItemNo = Utility.GetDatasetString(ds, 0, "LastItemNo");
                    }

                    ds.Dispose();

                    schChoiceAdModel.ItemNo = LastItemNo;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceLastItemNo() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "���������� ��ȸ �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        #endregion

        #region ������ ���������� ��ȸ

        /// <summary>
        /// ���������� ��ȸ
        /// </summary>
        /// <returns></returns>
        public void GetSchChoiceLastItemNoDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceLastItemNoDelete() Start");
                _log.Debug("-----------------------------------------");


                // ��������
                try
                {
                    StringBuilder sbQuery = new StringBuilder();

                    // ������ ����
                    string LastItemNo = "1";

                    // �ش� ������ ���� ����
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + " SELECT ISNULL(MAX(TA.ItemNo),1) AS LastItemNo               \n"
                        + "  FROM (                                                     \n"
                        + "        SELECT ISNULL(MAX(A.ItemNo),1) AS ItemNo               \n"
                        + "          FROM SchChoiceMenu    A INNER JOIN ContractItem B  \n"
                        + "                                          ON (A.ItemNo = B.ItemNo AND B.MediaCode = " + schChoiceAdModel.MediaCode + ") \n"
                        + "           AND A.ItemNo < " + schChoiceAdModel.ItemNo + " \n"
                        + "        UNION                                                \n"
                        + "        SELECT ISNULL(MAX(A.ItemNo),1) AS ItemNo               \n"
                        + "          FROM SchChoiceChannel A INNER JOIN ContractItem B  \n"
                        + "                                          ON (A.ItemNo = B.ItemNo AND B.MediaCode = " + schChoiceAdModel.MediaCode + ") \n"
                        + "           AND A.ItemNo < " + schChoiceAdModel.ItemNo + " \n"
                        + "       )  TA \n"
                        );

                    //__DEBUG__
                    _log.Debug(sbQuery.ToString());
                    //__DEBUG__


                    // ��������
                    DataSet ds = new DataSet();
                    _db.ExecuteQuery(ds, sbQuery.ToString());

                    if (Utility.GetDatasetCount(ds) != 0)
                    {
                        LastItemNo = Utility.GetDatasetString(ds, 0, "LastItemNo");
                    }

                    ds.Dispose();

                    schChoiceAdModel.ItemNo = LastItemNo;

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetSchChoiceLastItemNoDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "������ ���������� ��ȸ �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        #endregion

        #region �����޴����� ��������

        /// <summary>
        /// �����޴����� ��������
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceMenuDelete_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDelete() Start");
                _log.Debug("-----------------------------------------");


                // ��������
                try
                {
                    int i = 0;
                    int rc = 0;
                    StringBuilder sbQuery = new StringBuilder();
                    SqlParameter[] sqlParams = new SqlParameter[1];

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);

                    _db.BeginTran();

                    // �����޴����� �� �� ���̺��� ����
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceMenuDetail            \n"
                        + " WHERE MediaCode        = @MediaCode        \n"
                        );

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    // �����޴����� �� ���̺��� ����
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n"
                        + "DELETE SchChoiceMenu                  \n"
                        + " WHERE ItemNo  IN (SELECT ItemNo FROM ContractItem WHERE MediaCode = @MediaCode)         \n"
                        );

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("�����޴����� ����:[" + schChoiceAdModel.MediaCode + "] �����:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "�����޴����� ������ ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        #endregion

        #region �����޴����� �������� ����

        /// <summary>
        /// �����޴����� �������� ����
        /// </summary>
        /// <returns></returns>
        public void SetSchChoiceMenuDetailCreate_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDetailCreate() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("MediaCode         :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("GenreCode         :[" + schChoiceAdModel.GenreCode + "]");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("ScheduleOrder            :[" + schChoiceAdModel.ScheduleOrder + "]");
                _log.Debug("AdType            :[" + schChoiceAdModel.AdType + "]");
                // __DEBUG__				

                int i = 0;
                int rc = 0;
                int adSchType = 0;

                if (schChoiceAdModel.AdType == "10" || schChoiceAdModel.AdType == "13" || schChoiceAdModel.AdType == "16" || schChoiceAdModel.AdType == "17" || schChoiceAdModel.AdType == "19")
                {
                    adSchType = 10;
                }
                else
                    adSchType = 20;

                // ���� ���ι�ȣ�� ����
                string AckNo = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[1];

                //����ä�������̺� �����Ͱ� ���� ��� Insert
                sbQuery.Append("\n"
                    + "SELECT ItemNo FROM SchChoiceMenu  \n"
                    + " WHERE ItemNo    =  @ItemNo           \n"
                    );

                i = 0;
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                // ���
                // 0���̸� ����ä�������̿� ItemNo Insert
                if (Utility.GetDatasetCount(ds) == 0)
                {
                    // ��������
                    try
                    {
                        _db.BeginTran();


                        // ����ä�� �� ���̺� �߰�
                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceMenu ( \n"
                            + "            ItemNo            \n"
                            + "       )                      \n"
                            + "       VALUES (               \n"
                            + "           @ItemNo            \n"
                            + "       )                      \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        // ������� ������ ����
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "UPDATE ContractItem        \n"
                            + "   SET AdState = '20'      \n"   // ������� - 20:��
                            + "      ,ModDt   = GETDATE() \n"
                            + "      ,RegID   = '" + header.UserID + "' \n"
                            + " WHERE ItemNo  = @ItemNo   \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);


                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("�ű��� ����ä�α���:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.ChannelNo + "][" + schChoiceAdModel.Title + "] �����:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }


                sqlParams = new SqlParameter[4];

                i = 0;
                sqlParams[i++] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@GenreCode", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@ScheduleOrder", SqlDbType.SmallInt);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.MediaCode);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.GenreCode);
                sqlParams[i++].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);
                sqlParams[i++].Value = Convert.ToInt16(schChoiceAdModel.ScheduleOrder);

                // ������ �Ǿ��ִ��� �˻�
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "SELECT * FROM SchChoiceMenuDetail  \n"
                    + " WHERE MediaCode =  @MediaCode        \n"
                    + "   AND GenreCode =  @GenreCode        \n"
                    + "   AND ItemNo    =  @ItemNo           \n"
                    );

                // ��������
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                // ���
                // 0���̸� ������ ����
                if (Utility.GetDatasetCount(ds) == 0)
                {
                    // ��������
                    try
                    {
                        _db.BeginTran();

                        // ����ä�� �� ���̺� �߰�
                        sbQuery = new StringBuilder();

                        sbQuery.Append("\n"
                            + "INSERT INTO SchChoiceMenuDetail ( \n"
                            + "            MediaCode             \n"
                            + "           ,GenreCode             \n"
                            + "           ,ItemNo                \n"
                            + "           ,AckNo                 \n"
                            + "           ,ScheduleOrder         \n"
                            + "       )                          \n"
                            + "       VALUES(                    \n"
                            + "           @MediaCode             \n"
                            + "          ,@GenreCode             \n"
                            + "          ,@ItemNo                \n"
                            + "          ," + AckNo + "  \n"
                            + "          ,@ScheduleOrder                      \n" // �ʼ������� ������ 0
                            + "      )                           \n"
                            );

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        _db.CommitTran();

                        // __MESSAGE__
                        _log.Message("�ű��� �����޴�����:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] �����:[" + header.UserID + "]");
                    }
                    catch (Exception ex)
                    {
                        _db.RollbackTran();
                        throw ex;
                    }
                }
                else
                {
                    // __MESSAGE__
                    _log.Message("���� �����޴�����:[" + schChoiceAdModel.ItemName + "][" + schChoiceAdModel.GenreCode + "][" + schChoiceAdModel.GenreName + "] �����:[" + header.UserID + "]");
                }

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceMenuDetailCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3101";
                schChoiceAdModel.ResultDesc = "�����޴����� ���� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        #endregion

        #region ������λ����� ���ι�ȣ�� ����

        /// <summary>
        /// ������λ����� ���ι�ȣ�� ����
        /// ���°� 30:���������̸� �ű�(���� 10:����) ���� ������ AckNo ����
        /// </summary>
        /// <returns>string</returns>
        private string GetLastAckNo(string MediaCode, int AdSchType)
        {
            string AckNo = "";
            string AckState = "";
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetLastAckNo() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("MediaCode	    :[" + MediaCode + "]");		// �˻� ��ü
                _log.Debug("AdSchType		:[" + AdSchType + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.AppendFormat("\n SELECT * FROM TABLE(GET_ACKNO( {0} ))", AdSchType.ToString());
                _log.Debug(sbQuery.ToString());

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    AckNo = ds.Tables[0].Rows[0]["AckNo"].ToString();
                    AckState = ds.Tables[0].Rows[0]["AckState"].ToString();
                }

                ds.Dispose();

                _log.Debug("-----------------------------------------");
                _log.Debug("AckNo:" + AckNo.ToString() + ", State:" + AckState.ToString());
                _log.Debug(this.ToString() + "GetLastAckNo() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _log.Exception(ex);
                throw ex;
            }
            return AckNo;
        }

        #endregion

        #region �� ���� �����ϱ� [E_02]
        /// <summary>
        /// ������� ��û ���� ���� ���� �Ǿ��ִ� ������ �������� �����ϴ� ��ɰ�
        /// ������ ����(ItemNoCopy)�� �������� �����ϴ� ��� ���� 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceAdCopy(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceAdCopy() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                _log.Debug("ItemNoCopy        :[" + schChoiceAdModel.ItemNoCopy + "]");
                _log.Debug("AdType            :[" + schChoiceAdModel.AdType + "]");
                _log.Debug("MediaCode         :[" + schChoiceAdModel.MediaCode + "]");
                _log.Debug("CheckSchResult    :[" + schChoiceAdModel.CheckSchResult + "]");
                // __DEBUG__				

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[4];
                DataSet ds = new DataSet();

                int rc = 0;
                int adSchType = 0;
                string AdType = schChoiceAdModel.AdType;

                if (AdType == "10" || AdType == "13" || AdType == "16" || AdType == "17" || AdType == "19")
                {
                    adSchType = 10;
                }
                else
                    adSchType = 20;


                //���� �� �߰� �۾��� ������ ��츦 ����Ͽ� Ʈ����� ���
                _db.BeginTran();

                sqlParams[0] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[0].Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                sqlParams[1] = new SqlParameter("@UserID", SqlDbType.VarChar, 8);
                sqlParams[1].Value = header.UserID;

                sqlParams[2] = new SqlParameter("@ItemNoCopy", SqlDbType.Int);
                sqlParams[2].Value = Convert.ToInt32(schChoiceAdModel.ItemNoCopy);

                sqlParams[3] = new SqlParameter("@AckNo", SqlDbType.Int);
                // ���� ���ι�ȣ�� ����
                sqlParams[3].Value = GetLastAckNo(schChoiceAdModel.MediaCode, adSchType);

                #region ���� ����
                //������ ���� �� �߰��� ���

                if (schChoiceAdModel.CheckSchResult.Equals("DELETE"))
                {
                    try
                    {
                        _log.Debug("-----------------------------------------");
                        _log.Debug("���� �� ���� ���� Start");
                        _log.Debug("-----------------------------------------");

                        if (!AdType.StartsWith("1"))//10~19 ������ �ʼ� ���� �����ϰ� ScheduleOrder�� ����
                        {
                            //�޴���
                            sbQuery.Append("\n UPDATE A                                     \n");
                            sbQuery.Append(" SET A.ScheduleOrder = A.ScheduleOrder-1        \n");
                            sbQuery.Append(" FROM SchChoiceMenuDetail A                     \n");
                            sbQuery.Append(" INNER JOIN SchChoiceMenuDetail B               \n");
                            sbQuery.Append("            ON B.ItemNo=@ItemNo                 \n");
                            sbQuery.Append("            AND A.MediaCode=B.MediaCode         \n");
                            sbQuery.Append("            AND A.GenreCode=B.GenreCode         \n");
                            sbQuery.Append(" WHERE A.ScheduleOrder > B.ScheduleOrder        \n");
                            //ä����
                            sbQuery.Append("\n UPDATE A                                     \n");
                            sbQuery.Append(" SET A.ScheduleOrder = A.ScheduleOrder-1        \n");
                            sbQuery.Append(" FROM SchchoiceChannelDetail A                  \n");
                            sbQuery.Append(" INNER JOIN SchchoiceChannelDetail B            \n");
                            sbQuery.Append("            ON B.ItemNo=@ItemNo                 \n");
                            sbQuery.Append("            AND A.MediaCode=B.MediaCode         \n");
                            sbQuery.Append("            AND A.ChannelNo=B.ChannelNo         \n");
                            sbQuery.Append(" WHERE A.ScheduleOrder > B.ScheduleOrder        \n");
                            //ȸ����
                            sbQuery.Append("\n UPDATE A                                     \n");
                            sbQuery.Append(" SET A.ScheduleOrder = A.ScheduleOrder-1        \n");
                            sbQuery.Append(" FROM SchChoiceSeriesDetail A                   \n");
                            sbQuery.Append(" INNER JOIN SchChoiceSeriesDetail B             \n");
                            sbQuery.Append("            ON B.ItemNo=@ItemNo                 \n");
                            sbQuery.Append("            AND A.MediaCode=B.MediaCode         \n");
                            sbQuery.Append("            AND A.ChannelNo=B.ChannelNo         \n");
                            sbQuery.Append("            AND A.SeriesNo=B.SeriesNo           \n");
                            sbQuery.Append(" WHERE A.ScheduleOrder > B.ScheduleOrder        \n");

                            rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                            _log.Debug(sbQuery.ToString());
                        }

                        sbQuery = new StringBuilder();
                        //�޴�, ä��, ȸ��, ���� �����̺��� ������ ����
                        sbQuery.Append("\n DELETE FROM SchChoiceMenuDetail      \n");
                        sbQuery.Append("    WHERE ItemNo =  @ItemNo             \n");
                        sbQuery.Append("\n DELETE FROM SchChoiceChannelDetail   \n");
                        sbQuery.Append("    WHERE ItemNo =  @ItemNo             \n");
                        sbQuery.Append("\n DELETE FROM SchChoiceSeriesDetail    \n");
                        sbQuery.Append("    WHERE ItemNo =  @ItemNo             \n");
                        sbQuery.Append("\n DELETE FROM SchDesignatedDetail      \n");
                        sbQuery.Append("    WHERE ItemNo =  @ItemNo             \n");

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        _log.Debug("-----------------------------------------");
                        _log.Debug("���� �� ���� ���� End");
                        _log.Debug("-----------------------------------------");
                        // __DEBUG__

                    }
                    catch (Exception ex)
                    {
                        schChoiceAdModel.ResultCD = "3301";
                        schChoiceAdModel.ResultDesc = "�� ������ ������ �߻��Ͽ����ϴ�.";
                        _log.Exception(ex);
                        throw ex;
                    }
                }
                #endregion

                try
                {
                    #region �޴������̺�
                    //�����޴������̺� �����Ͱ� ���� ��� Insert
                    _log.Debug("-----------------------------------------");
                    _log.Debug("�޴��� ���̺� INSERT START");
                    _log.Debug("-----------------------------------------");

                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT ItemNo FROM SchChoiceMenu with(noLock)    \n");
                    sbQuery.Append("    WHERE ItemNo =  @ItemNo                         \n");

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // ��������
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // ��� 0���̸� �����޴������̺� ItemNo Insert
                    if (Utility.GetDatasetCount(ds) == 0)
                    {
                        // ��������
                        try
                        {
                            // ����ä�� �� ���̺� �߰�
                            sbQuery = new StringBuilder();
                            sbQuery.Append("\n");
                            sbQuery.Append("INSERT INTO SchChoiceMenu (   \n");
                            sbQuery.Append("            ItemNo            \n");
                            sbQuery.Append("       )                      \n");
                            sbQuery.Append("       VALUES (               \n");
                            sbQuery.Append("           @ItemNo            \n");
                            sbQuery.Append("       )                      \n");

                            rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                            // ������� ������ ����
                            sbQuery = new StringBuilder();

                            sbQuery.Append("\n");
                            sbQuery.Append("UPDATE ContractItem                      \n");
                            sbQuery.Append("   SET AdState = '20'                    \n");   // ������� - 20:��
                            sbQuery.Append("      ,ModDt   = GETDATE()               \n");
                            sbQuery.Append("      ,RegID   = @UserID                 \n");
                            sbQuery.Append(" WHERE ItemNo  = @ItemNo                 \n");

                            rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                            // __DEBUG__
                            _log.Debug(sbQuery.ToString());
                            _log.Debug("-----------------------------------------");
                            _log.Debug("�޴��� ���̺� INSERT END");
                            _log.Debug("-----------------------------------------");
                            // __DEBUG__
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    #endregion
                    #region ä�������̺�
                    //����ä�������̺� �����Ͱ� ���� ��� Insert
                    _log.Debug("-----------------------------------------");
                    _log.Debug("ä���� ���̺� INSERT START");
                    _log.Debug("-----------------------------------------");
                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n");
                    sbQuery.Append("SELECT ItemNo FROM SchChoiceChannel with(NoLock)  \n");
                    sbQuery.Append(" WHERE ItemNo    =  @ItemNo                       \n");

                    // ��������
                    ds = new DataSet();
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // ��� 0���̸� ����ä�������̺� ItemNo Insert
                    if (Utility.GetDatasetCount(ds) == 0)
                    {
                        // ��������
                        try
                        {
                            // ����ä�� �� ���̺� �߰�
                            sbQuery = new StringBuilder();
                            sbQuery.Append("\n");
                            sbQuery.Append("INSERT INTO SchChoiceChannel (       \n");
                            sbQuery.Append("            ItemNo                   \n");
                            sbQuery.Append("       )                             \n");
                            sbQuery.Append("       VALUES (                      \n");
                            sbQuery.Append("           @ItemNo                   \n");
                            sbQuery.Append("       )                             \n");

                            rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                            // ������� ������ ����
                            sbQuery = new StringBuilder();

                            sbQuery.Append("\n");
                            sbQuery.Append("UPDATE ContractItem                      \n");
                            sbQuery.Append("   SET AdState = '20'                    \n");   // ������� - 20:��
                            sbQuery.Append("      ,ModDt   = GETDATE()               \n");
                            sbQuery.Append("      ,RegID   = @UserID                 \n");
                            sbQuery.Append(" WHERE ItemNo  = @ItemNo                 \n");

                            rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                            // __DEBUG__
                            _log.Debug(sbQuery.ToString());
                            _log.Debug("-----------------------------------------");
                            _log.Debug("ä���� ���̺� INSERT END");
                            _log.Debug("-----------------------------------------");
                            // __DEBUG__
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    #endregion

                    #region �󼼸޴��� insert

                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT ItemNo FROM SchChoiceMenuDetail with(noLock)    \n");
                    sbQuery.Append("    WHERE ItemNo =  @ItemNoCopy                           \n");

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // ��������
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // ����� �����ϸ� ��ä�������̺� Insert
                    if (Utility.GetDatasetCount(ds) > 0)
                    {
                        _log.Debug("-----------------------------------------");
                        _log.Debug("�󼼸޴��� INSERT START");
                        _log.Debug("-----------------------------------------");

                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n");
                        sbQuery.Append(" INSERT INTO SchChoiceMenuDetail(               \n");
                        sbQuery.Append(" 	    ItemNo                                  \n");
                        sbQuery.Append(" 	    ,MediaCode                              \n");
                        sbQuery.Append(" 	    ,GenreCode                              \n");
                        sbQuery.Append(" 	    ,AckNo                                  \n");
                        sbQuery.Append(" 	    ,ScheduleOrder                          \n");
                        sbQuery.Append(" 	)                                           \n");
                        sbQuery.Append(" 	SELECT                                      \n");
                        sbQuery.Append(" 		@ItemNo     ItemNo                      \n");
                        sbQuery.Append(" 		,MediaCode                              \n");
                        sbQuery.Append(" 		,GenreCode                              \n");
                        sbQuery.Append(" 		,@AckNo     AckNo                       \n");

                        if (AdType.StartsWith("1")) //10~19���� �ʼ� ����
                        {
                            sbQuery.Append(" 	,0 ScheduleOrder                        \n");
                        }
                        else
                        {
                            sbQuery.Append(" 	,(SELECT                                \n");
                            sbQuery.Append("      ISNULL(MAX(ScheduleOrder),0)+1 AS ScheduleOrder  \n");// �ɼǱ���(OAP��)�� ������ MAX+1
                            sbQuery.Append("      FROM SchChoiceMenuDetail              \n");
                            sbQuery.Append("      WHERE MediaCode   =   A.MediaCode     \n");
                            sbQuery.Append("      AND   GenreCode   =   A.GenreCode)    \n");
                        }
                        sbQuery.Append(" 	FROM SchChoiceMenuDetail A                  \n");
                        sbQuery.Append(" 	WHERE not exists(                           \n"); //exists�� ���� ���θ� Ȯ���ϱ� ����. �ϳ��� �����ϸ� true�� ����.
                        sbQuery.Append(" 		SELECT 1 FROM SchChoiceMenuDetail B     \n"); //where�������� �ش��ϴ� ���� �����ϸ� �� ���� ����� ���⶧���� 1�� �����
                        sbQuery.Append(" 			WHERE   B.MediaCode=A.MediaCode     \n");
                        sbQuery.Append(" 			AND     B.GenreCode=A.GenreCode     \n");
                        sbQuery.Append(" 			AND     B.ItemNo   =@ItemNo)        \n");
                        sbQuery.Append("   AND A.ItemNo = @ItemNoCopy                   \n");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        _log.Debug("-----------------------------------------");
                        _log.Debug("�󼼸޴��� INSERT END");
                        _log.Debug("-----------------------------------------");
                        // __DEBUG__

                        // ��������
                        ds = new DataSet();
                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    }
                    #endregion
                    #region ��ä���� insert

                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT ItemNo FROM SchChoiceChannelDetail with(noLock) \n");
                    sbQuery.Append("    WHERE ItemNo =  @ItemNoCopy                           \n");

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // ��������
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // ����� �����ϸ� �󼼸޴������̺� Insert
                    if (Utility.GetDatasetCount(ds) > 0)
                    {
                        _log.Debug("-----------------------------------------");
                        _log.Debug("��ä���� INSERT START");
                        _log.Debug("-----------------------------------------");

                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n");
                        sbQuery.Append(" INSERT INTO SchChoiceChannelDetail(            \n");
                        sbQuery.Append(" 	    ItemNo                                  \n");
                        sbQuery.Append(" 	    ,MediaCode                              \n");
                        sbQuery.Append(" 	    ,ChannelNo                              \n");
                        sbQuery.Append(" 	    ,AckNo                                  \n");
                        sbQuery.Append(" 	    ,ScheduleOrder                          \n");
                        sbQuery.Append(" 	)                                           \n");
                        sbQuery.Append(" 	SELECT                                      \n");
                        sbQuery.Append(" 		@ItemNo     ItemNo                      \n");
                        sbQuery.Append(" 		,MediaCode                              \n");
                        sbQuery.Append(" 		,ChannelNo                              \n");
                        sbQuery.Append(" 		,@AckNo     AckNo                       \n");

                        if (AdType.StartsWith("1")) //10~19���� �ʼ� ����
                        {
                            sbQuery.Append(" 		,0 ScheduleOrder                    \n");
                        }
                        else
                        {
                            sbQuery.Append(" 		,(SELECT                                            \n");
                            sbQuery.Append("           ISNULL(MAX(ScheduleOrder),0)+1 AS ScheduleOrder  \n");// �ɼǱ���(OAP��)�� ������ MAX+1
                            sbQuery.Append("         FROM SchChoiceChannelDetail           \n");
                            sbQuery.Append("         WHERE MediaCode   =   A.MediaCode     \n");
                            sbQuery.Append("         AND   ChannelNo   =   A.ChannelNo)    \n");
                        }
                        sbQuery.Append(" 	FROM SchChoiceChannelDetail A               \n");
                        sbQuery.Append(" 	WHERE not exists(                           \n");
                        sbQuery.Append(" 		SELECT 1 FROM SchChoiceChannelDetail B  \n");
                        sbQuery.Append(" 			WHERE B.MediaCode=A.MediaCode       \n");
                        sbQuery.Append(" 			AND B.ChannelNo=A.ChannelNo         \n");
                        sbQuery.Append(" 			AND B.ItemNo = @ItemNo)             \n");
                        sbQuery.Append("   AND A.ItemNo = @ItemNoCopy                   \n");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        _log.Debug("-----------------------------------------");
                        _log.Debug("��ä���� INSERT END");
                        _log.Debug("-----------------------------------------");
                        // __DEBUG__

                        // ��������
                        ds = new DataSet();
                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    }
                    #endregion
                    #region ��ȸ���� insert

                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT ItemNo FROM SchChoiceSeriesDetail with(noLock)  \n");
                    sbQuery.Append("    WHERE ItemNo =  @ItemNoCopy                           \n");

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // ��������
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // ����� �����ϸ� ��ȸ�������̺� Insert
                    if (Utility.GetDatasetCount(ds) > 0)
                    {
                        _log.Debug("-----------------------------------------");
                        _log.Debug("��ȸ���� INSERT START");
                        _log.Debug("-----------------------------------------");

                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n");
                        sbQuery.Append(" INSERT INTO SchChoiceSeriesDetail(             \n");
                        sbQuery.Append(" 	    ItemNo                                  \n");
                        sbQuery.Append(" 	    ,MediaCode                              \n");
                        sbQuery.Append(" 	    ,ChannelNo                              \n");
                        sbQuery.Append("        ,SeriesNo                               \n");
                        sbQuery.Append(" 	    ,AckNo                                  \n");
                        sbQuery.Append(" 	    ,ScheduleOrder                          \n");
                        sbQuery.Append(" 	)                                           \n");
                        sbQuery.Append(" 	SELECT                                      \n");
                        sbQuery.Append(" 		@ItemNo     ItemNo                      \n");
                        sbQuery.Append(" 		,MediaCode                              \n");
                        sbQuery.Append(" 		,ChannelNo                              \n");
                        sbQuery.Append("        ,SeriesNo                               \n");
                        sbQuery.Append(" 		,@AckNo     AckNo                       \n");

                        if (AdType.StartsWith("1")) //10~19���� �ʼ� ����
                        {
                            sbQuery.Append(" 		,0 ScheduleOrder                    \n");
                        }
                        else
                        {
                            sbQuery.Append(" 		,(SELECT                                            \n");
                            sbQuery.Append("           ISNULL(MAX(ScheduleOrder),0)+1 AS ScheduleOrder  \n");// �ɼǱ���(OAP��)�� ������ MAX+1
                            sbQuery.Append("         FROM SchChoiceSeriesDetail         \n");
                            sbQuery.Append("         WHERE MediaCode   =   A.MediaCode  \n");
                            sbQuery.Append("         AND   SeriesNo    =   A.SeriesNo   \n");
                            sbQuery.Append("         AND   ChannelNo   =   A.ChannelNo) \n");
                        }
                        sbQuery.Append(" 	FROM SchChoiceSeriesDetail A                \n");
                        sbQuery.Append(" 	WHERE not exists(                           \n");
                        sbQuery.Append(" 		SELECT 1 FROM SchChoiceSeriesDetail B   \n");
                        sbQuery.Append(" 			WHERE   B.MediaCode=A.MediaCode     \n");
                        sbQuery.Append(" 			AND     B.ChannelNo=A.ChannelNo     \n");
                        sbQuery.Append(" 			AND     B.SeriesNo =A.SeriesNo      \n");
                        sbQuery.Append(" 			AND     B.ItemNo   =@ItemNo)        \n");
                        sbQuery.Append("   AND A.ItemNo = @ItemNoCopy                   \n");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        _log.Debug("-----------------------------------------");
                        _log.Debug("��ȸ���� INSERT END");
                        _log.Debug("-----------------------------------------");
                        // __DEBUG__

                        // ��������
                        ds = new DataSet();
                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    }
                    #endregion
                    #region ������ insert

                    sbQuery = new StringBuilder();
                    sbQuery.Append("\n SELECT ItemNo FROM SchDesignatedDetail with(noLock)    \n");
                    sbQuery.Append("    WHERE ItemNo =  @ItemNoCopy                           \n");

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // ��������
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                    // ����� �����ϸ� ��ȸ�������̺� Insert
                    if (Utility.GetDatasetCount(ds) > 0)
                    {

                        _log.Debug("-----------------------------------------");
                        _log.Debug("�������� INSERT START");
                        _log.Debug("-----------------------------------------");

                        sbQuery = new StringBuilder();
                        sbQuery.Append("\n");
                        sbQuery.Append(" INSERT INTO SchDesignatedDetail(              \n");
                        sbQuery.Append(" 	    ItemNo                                 \n");
                        sbQuery.Append(" 	    ,MediaCode                             \n");
                        sbQuery.Append("       ,Category                               \n");
                        sbQuery.Append("       ,Genre                                  \n");
                        sbQuery.Append("       ,Channel                                \n");
                        sbQuery.Append("       ,Series                                 \n");
                        sbQuery.Append("       ,AckNo                                  \n");
                        sbQuery.Append(" 	)                                          \n");
                        sbQuery.Append(" 	SELECT                                     \n");
                        sbQuery.Append(" 		@ItemNo     ItemNo                     \n");
                        sbQuery.Append(" 		,MediaCode                             \n");
                        sbQuery.Append("       ,Category                               \n");
                        sbQuery.Append("       ,Genre                                  \n");
                        sbQuery.Append("       ,Channel                                \n");
                        sbQuery.Append("       ,Series                                 \n");
                        sbQuery.Append(" 		,@AckNo     AckNo                      \n");
                        sbQuery.Append(" 	FROM SchDesignatedDetail A                 \n");
                        sbQuery.Append(" 	WHERE not exists(                          \n");
                        sbQuery.Append(" 		SELECT 1 FROM SchDesignatedDetail B    \n");
                        sbQuery.Append(" 			WHERE   B.MediaCode=A.MediaCode    \n");
                        sbQuery.Append(" 			AND     B.Category =A.Category     \n");
                        sbQuery.Append(" 			AND     B.Genre    =A.Genre        \n");
                        sbQuery.Append(" 			AND     B.Channel  =A.Channel      \n");
                        sbQuery.Append(" 			AND     B.Series   =A.Series       \n");
                        sbQuery.Append(" 			AND     B.ItemNo   =@ItemNo)       \n");
                        sbQuery.Append("   AND A.ItemNo = @ItemNoCopy                  \n");

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        _log.Debug("-----------------------------------------");
                        _log.Debug("�������� INSERT END");
                        _log.Debug("-----------------------------------------");
                        // __DEBUG__

                        // ��������
                        ds = new DataSet();
                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    schChoiceAdModel.ResultCD = "3101";
                    schChoiceAdModel.ResultDesc = "�� ����� ������ �߻��Ͽ����ϴ�.";
                    _log.Exception(ex);
                    throw ex;
                }

                _db.CommitTran();   //Ʈ����� ����
                schChoiceAdModel.ResultCD = "0000";  // ����
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchChoiceAdCopy() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran(); //exception�� �߻��� ��� �ѹ�
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }
        #endregion

        #region �������� ��ȸ [E_02]
        /// <summary>
        /// ������ ������ �������� �����ϴ��� Ȯ��
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        public void CheckSchChoice(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".CheckSchChoice() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("ItemNo            :[" + schChoiceAdModel.ItemNo + "]");
                // __DEBUG__				

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter sqlParamItemNo = new SqlParameter("@ItemNo", SqlDbType.Int);

                sqlParamItemNo.Value = Convert.ToInt32(schChoiceAdModel.ItemNo);

                DataSet ds = new DataSet();

                #region �޴� �� ��ȸ

                sbQuery.Append("\n SELECT ItemNo FROM SchChoiceMenuDetail with(noLock)  \n");
                sbQuery.Append("    WHERE ItemNo =  @ItemNo                             \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
                // ��������
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParamItemNo);

                // ��� 0���̻��̸� �̹� ���� ����

                if (Utility.GetDatasetCount(ds) > 0)
                {
                    schChoiceAdModel.CheckMenu = true;
                }
                #endregion

                #region ä�� �� ��ȸ
                sbQuery = new StringBuilder();

                sbQuery.Append("\n SELECT ItemNo FROM SchChoiceChannelDetail with(noLock)  \n");
                sbQuery.Append("    WHERE ItemNo =  @ItemNo                                \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
                // ��������
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParamItemNo);

                // ��� 0���̻��̸� �̹� ���� ����

                if (Utility.GetDatasetCount(ds) > 0)
                {
                    schChoiceAdModel.CheckChannel = true;
                }
                #endregion

                #region �ø��� �� ��ȸ

                sbQuery = new StringBuilder();

                sbQuery.Append("\n SELECT ItemNo FROM SchChoiceSeriesDetail with(noLock)  \n");
                sbQuery.Append("    WHERE ItemNo =  @ItemNo                              \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
                // ��������
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParamItemNo);

                // ��� 0���̻��̸� �̹� ���� ����

                if (Utility.GetDatasetCount(ds) > 0)
                {
                    schChoiceAdModel.CheckSeries = true;
                }
                #endregion

                #region ���� �� ��ȸ

                sbQuery = new StringBuilder();

                sbQuery.Append("\n SELECT ItemNo FROM SchDesignatedDetail with(noLock)  \n");
                sbQuery.Append("    WHERE ItemNo =  @ItemNo                             \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
                // ��������
                ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParamItemNo);

                // ��� 0���̻��̸� �̹� ���� ����

                if (Utility.GetDatasetCount(ds) > 0)
                {
                    schChoiceAdModel.CheckDetail = true;
                }
                #endregion
                _log.Debug("�޴��� " + schChoiceAdModel.CheckMenu + "\n");
                _log.Debug("ä���� " + schChoiceAdModel.CheckChannel + "\n");
                _log.Debug("�ø����� " + schChoiceAdModel.CheckSeries + "\n");
                _log.Debug("������ " + schChoiceAdModel.CheckDetail + "\n");

                schChoiceAdModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".CheckSchChoice() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schChoiceAdModel.ResultCD = "3000";
                schChoiceAdModel.ResultDesc = "���� ���� �ҷ������� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }
        #endregion [E_02]
    }
}