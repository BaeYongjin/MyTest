/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [A_01]
 * 수정자    : JH.Kim
 * 수정일    : 2015.11.
 * 수정내용  : 영업관리 대상 플래그 추가
 * --------------------------------------------------------
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Contract
{
	/// <summary>
	/// CampaignBiz에 대한 요약 설명입니다.
	/// </summary>
	public class CampaignBiz : BaseBiz
	{
		public CampaignBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}
        
		/// <summary>
		/// 광고계약목록조회
		/// </summary>
		/// <param name="campaignModel"></param>
		public void GetContractList(HeaderModel header, CampaignModel campaignModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + campaignModel.SearchKey       + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
              

                sbQuery.Append("  SELECT    a.mda_cod as MediaCode  ");
                sbQuery.Append("\n          ,c.mda_nm as MediaName	");
                sbQuery.Append("\n          ,a.rep_cod as RapCode	");
                sbQuery.Append("\n          ,d.rep_nm as RapName	");
                sbQuery.Append("\n          ,a.agnc_cod as AgencyCode	");
                sbQuery.Append("\n          ,e.agnc_nm as AgencyName	");
                sbQuery.Append("\n          ,a.advter_cod as AdvertiserCode	");
                sbQuery.Append("\n          ,f.advter_nm as AdvertiserName  ");
                sbQuery.Append("\n          ,a.cntr_seq as ContractSeq		");
                sbQuery.Append("\n          ,a.cntr_nm as ContractName      ");
                sbQuery.Append("\n          ,(Select count(*) from ADVT_MST where cntr_seq = a.cntr_seq) \"Row\"  ");
                sbQuery.Append("\n          ,a.cntr_begin_dy as ContStartDay    ");
                sbQuery.Append("\n          ,a.cntr_end_dy as ContEndDay		");
                sbQuery.Append("\n          ,a.cntr_amt as ContractAmt			");
                sbQuery.Append("\n          ,a.cntr_stt as State				");
                sbQuery.Append("\n          ,g.stm_cod_nm as StateName			");
                sbQuery.Append("\n          ,a.advt_tm as AdTime				");
                sbQuery.Append("\n          ,a.bns_rt as BonusRate				");
                sbQuery.Append("\n          ,a.long_bns as LongBonus			");
                sbQuery.Append("\n          ,a.spcl_bns as SpecialBonus			");
                sbQuery.Append("\n          ,a.tot_bns as TotalBonus			");
                sbQuery.Append("\n          ,a.grte_imps as SecurityTgt			");
                sbQuery.Append("\n          ,'' as packageName          ");
                sbQuery.Append("\n          ,a.cntr_prc as Price        ");
                sbQuery.Append("\n          ,a.cntr_memo as \"Comment\" ");
                sbQuery.Append("\n          ,'' as JobCode          	");
                sbQuery.Append("\n          ,'' as JobCode2          	");
                sbQuery.Append("\n          ,'' as JobCode3          	");
                sbQuery.Append("\n          ,'' as JobName          	");
                sbQuery.Append("\n          ,'' as JobName1          	");
                sbQuery.Append("\n          ,'' as JobName2          	");
                sbQuery.Append("\n          ,'' as JobName3          	");
                sbQuery.Append("\n          ,'' as RegDt                ");
                sbQuery.Append("\n          ,'' as ModDt		        ");
                sbQuery.Append("\n          ,'' as RegId				");
                sbQuery.Append("\n FROM  CNTR a                         ");
                /*LEFT JOIN STM_USER   b ON (a.RegId      = b.UserId)*/
                sbQuery.Append("\n LEFT JOIN MDA        c ON (a.mda_cod    = c.mda_cod) ");
                sbQuery.Append("\n LEFT JOIN MDA_REP    d ON (a.rep_cod    = d.rep_cod)	");
                sbQuery.Append("\n LEFT JOIN AGNC       e ON (a.agnc_cod   = e.agnc_cod)");
                sbQuery.Append("\n LEFT JOIN ADVTER     f ON (a.advter_cod = f.advter_cod)  ");
                sbQuery.Append("\n LEFT JOIN STM_COD    g ON (a.cntr_stt   = g.stm_cod AND g.stm_cod_cls='23' )	");
                sbQuery.Append("\n WHERE  1 = 1 ");

                if (!campaignModel.MediaCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.mda_cod = '" + campaignModel.MediaCode + "'  \n");
                }
                if (!campaignModel.RapCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.rep_cod = '" + campaignModel.RapCode + "'  \n");
                }
                if (!campaignModel.AgencyCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.agnc_cod = '" + campaignModel.AgencyCode + "'  \n");
                }
                if (!campaignModel.AdvertiserCode.Equals("00") && campaignModel.AdvertiserCode.Length > 0)
                {
                    sbQuery.Append("  AND    a.advter_cod = '" + campaignModel.AdvertiserCode + "'  \n");
                }
                if (!campaignModel.State.Equals("00"))
                {
                    sbQuery.Append("  AND    a.cntr_stt = '" + campaignModel.State + "'  \n");
                }

                // 검색어가 있으면
                if (campaignModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( a.cntr_nm LIKE '%" + campaignModel.SearchKey.Trim() + "%' OR	\n"
                        + "        a.cntr_memo LIKE '%" + campaignModel.SearchKey.Trim() + "%'	\n"
                        + " OR e.agnc_nm    LIKE '%" + campaignModel.SearchKey.Trim() + "%'		\n"
                        + " OR f.advter_nm    LIKE '%" + campaignModel.SearchKey.Trim() + "%'	\n"
                    + " 	)       \n"
                        );
                }

                sbQuery.Append("  ORDER BY a.cntr_seq Desc");

                // __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고계약모델에 복사
				campaignModel.ContractDataSet = ds.Copy();
				// 결과
				campaignModel.ResultCnt = Utility.GetDatasetCount(campaignModel.ContractDataSet);
				// 결과코드 셋트
				campaignModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + campaignModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				campaignModel.ResultCD = "3000";
				campaignModel.ResultDesc = "광고계약정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
        

		// 2007.10.03 RH.Jung 목록조회 함수 추가
		/// <summary>
		/// 광고계약목록조회
		/// </summary>
		/// <param name="campaignModel"></param>
		public void GetContractList2(HeaderModel header, CampaignModel campaignModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractList2() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + campaignModel.SearchKey       + "]");
				_log.Debug("SearchState_10 :[" + campaignModel.SearchState_10  + "]");
				_log.Debug("SearchState_20 :[" + campaignModel.SearchState_20  + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + "SELECT  e.agnc_nm    as   AgencyName			\n"
                    + "       ,f.advter_nm  as  AdvertiserName		\n"
                    + "       ,a.cntr_seq   as  ContractSeq			\n"
                    + "       ,a.cntr_nm    as  ContractName		\n"
                    + "       ,a.cntr_begin_dy  as ContStartDay		\n"
                    + "       ,a.cnt_end_dy     as ContEndDay		\n"
                    + "       ,a.cntr_amt   as ContractAmt			\n"
                    + "       ,a.cntr_stt   as State				\n"
                    + "       ,g.stm_cod_nm as StateName			\n"
                    + "       ,(SELECT COUNT(*) FROM ADVT_MST WHERE cntr_seq = a.cntr_seq) AS AdCount \n"
                    + "  FROM  CNTR a                     \n"
                    + "        LEFT JOIN AGNC   e  ON (a.agnc_cod   = e.agnc_cod)     \n"
                    + "        LEFT JOIN ADVTER f  ON (a.advter_cod = f.advter_cod) \n"
                    + "        LEFT JOIN STM_COD g ON (a.cntr_stt   = g.stm_cod AND g.stm_cod_cls ='23' ) \n"
                    + " WHERE 1=1 \n"
                    );

				if(!campaignModel.MediaCode.Equals("00"))
				{
                    sbQuery.Append("  AND    a.mda_cod = '" + campaignModel.MediaCode + "'  \n");
				}
				if(!campaignModel.RapCode.Equals("00"))
				{
                    sbQuery.Append("  AND    a.mda_cod = '" + campaignModel.RapCode + "'  \n");
				}        
				if(!campaignModel.AgencyCode.Equals("00"))
				{
                    sbQuery.Append("  AND    a.mda_cod = '" + campaignModel.AgencyCode + "'  \n");
				}     

				// 검색어가 있으면
				if (campaignModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("\n"
                        + "  AND ( a.cntr_nm   LIKE '%" + campaignModel.SearchKey.Trim() + "%' \n"
                        + "     OR e.agnc_nm     LIKE '%" + campaignModel.SearchKey.Trim() + "%' \n"
                        + "     OR f.advter_nm LIKE '%" + campaignModel.SearchKey.Trim() + "%'	\n"
						+ "  )      \n"
						);
				}
       
				// 광고계약상태 선택에 따라

				bool isState = false;

				// 광고계약상태 운영중
				if(campaignModel.SearchState_10.Trim().Length > 0 && campaignModel.SearchState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append("  AND ( a.cntr_stt  = '10' \n");
					isState = true;
				}	
				// 광고상태 편성
				if(campaignModel.SearchState_20.Trim().Length > 0 && campaignModel.SearchState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append("             OR ");
					else sbQuery.Append("  AND ( ");
                    sbQuery.Append(" a.cntr_stt  = '20' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

                sbQuery.Append("  ORDER BY a.cntr_seq Desc");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고계약모델에 복사
				campaignModel.ContractDataSet = ds.Copy();
				// 결과
				campaignModel.ResultCnt = Utility.GetDatasetCount(campaignModel.ContractDataSet);
				// 결과코드 셋트
				campaignModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + campaignModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				campaignModel.ResultCD = "3000";
				campaignModel.ResultDesc = "광고계약정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		/// <summary>
		/// 캠페인목록조회
		/// </summary>
		/// <param name="campaignModel"></param>
		public void GetCampaignList(HeaderModel header, CampaignModel campaignModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");				
				_log.Debug("ContractSeq	:[" + campaignModel.ContractSeq + "]");
				_log.Debug("SearchUse   :[" + campaignModel.SearchUse	+ "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
              
                sbQuery.AppendLine(" SELECT     ");
                sbQuery.AppendLine("     '' as  MediaCode		        ");
                sbQuery.AppendLine("     ,a.camp_cod as CampaignCode	");                                                                                            
                sbQuery.AppendLine("     ,a.cntr_seq   as ContractSeq   ");                                                                                                
                sbQuery.AppendLine("     ,a.camp_nm as CampaignName     ");                                                                                          
                sbQuery.AppendLine("     ,NVL(a.camp_prc,0) as Price    ");                                                                               
                sbQuery.AppendLine("     ,a.use_yn as UseYn			    ");                                                                                        
                sbQuery.AppendLine("     ,CASE a.use_yn  WHEN 'N'  THEN '사용안함' ELSE '' END UseNo ");                                                   
                sbQuery.AppendLine("     ,'' as  RegDt                  ");                                                      
                sbQuery.AppendLine("     ,'' as  ModDt                  ");                                                      
                sbQuery.AppendLine("     ,'' as  RegName				");	                                                                        
                sbQuery.AppendLine("     ,( select count(*) from CAMP_DTL c WHERE c.camp_cod = a.camp_cod ) as AdsCnt ");      
                sbQuery.AppendLine("     ,0 as PnsCnt               ");
                sbQuery.AppendLine("     ,'' as BizManageTarget     ");
                sbQuery.AppendLine(" FROM CAMP_MST a                ");
                sbQuery.AppendLine(" WHERE  1 = 1   ");
                if (campaignModel.ContractSeq.Trim().Length > 0 && !campaignModel.ContractSeq.Equals("00"))
                {
                    sbQuery.AppendLine("  AND  a.cntr_seq = '" + campaignModel.ContractSeq.Trim() + "' \n");
                }

                if (!campaignModel.SearchUse.Equals("N"))
                {
                    sbQuery.AppendLine("  AND a.use_yn = 'Y' \n");
                }

                sbQuery.AppendLine("  ORDER BY a.camp_cod, a.cntr_seq");
                // __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				campaignModel.CampaignDataSet = ds.Copy();
				// 결과
				campaignModel.ResultCnt = Utility.GetDatasetCount(campaignModel.CampaignDataSet);
				// 결과코드 셋트
				campaignModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + campaignModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				campaignModel.ResultCD = "3000";
				campaignModel.ResultDesc = "캠페인정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		
		/// <summary>
		/// 캠페인에 설정된 하위 광고내역을 가져온다.
        /// 추가: 팝업내역도 같이 가져온다
		/// </summary>
		/// <param name="campaignModel"></param>
		public void GetContractItemList(HeaderModel header, CampaignModel campaignModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");				
				_log.Debug("CampaignCode      :[" + campaignModel.CampaignCode       + "]");
               
				// __DEBUG__
				StringBuilder sbQuery = new StringBuilder();
                

                sbQuery.AppendLine(" SELECT     ");
                sbQuery.AppendLine("         'False' AS CheckYn                 ");                             
                sbQuery.AppendLine("         ,a.item_no as ItemNo 	            ");                      
                sbQuery.AppendLine("         ,b.item_nm as ItemName	            ");                         
                sbQuery.AppendLine("         ,t.cntr_nm as ContractName         ");                           
                sbQuery.AppendLine("         ,t.advter_cod as AdvertiserCode	");                            
                sbQuery.AppendLine("         ,t.advter_nm as AdvertiserName	    ");                        
                sbQuery.AppendLine("         ,b.begin_dy as ExcuteStartDay	    ");                        
                sbQuery.AppendLine("         ,b.end_dy as ExcuteEndDay	        ");                    
                sbQuery.AppendLine("         ,b.rl_end_dy as RealEndDay	        ");                        
                sbQuery.AppendLine("         ,b.advt_stt as AdState 	        ");                        
                sbQuery.AppendLine("         ,e.stm_cod_nm as AdStateName	    ");                
                sbQuery.AppendLine("         ,b.file_path as FilePath	        ");                        
                sbQuery.AppendLine("         ,b.file_nm as FileName	            ");                    
                sbQuery.AppendLine("         ,b.file_stt as FileState		    ");                        
                sbQuery.AppendLine("         ,f.stm_cod_nm as FileStateName		");                
                sbQuery.AppendLine("         ,0 as HomeCount                    ");    
                sbQuery.AppendLine("         ,(SELECT COUNT(*) FROM SCHD_MENU WHERE item_no = a.item_no) AS MenuCount      ");
                sbQuery.AppendLine("         ,(SELECT COUNT(*) FROM SCHD_TITLE   WHERE item_no = a.item_no) AS ChannelCount");   
                sbQuery.AppendLine("         ,TO_CHAR(SYSDATE, 'YYYYMMDD') as NowDay    "); 
                sbQuery.AppendLine("         ,b.advt_typ as AdType		        ");                        
                sbQuery.AppendLine("         ,g.stm_cod_nm as AdTypeName		");                
                sbQuery.AppendLine(" FROM       CAMP_DTL   a                    "); 
                sbQuery.AppendLine(" LEFT JOIN ADVT_MST  b ON (a.item_no = b.item_no)   ");                  
                sbQuery.AppendLine(" LEFT JOIN (                                ");
                sbQuery.AppendLine("         SELECT x.cntr_seq ,x.mda_cod, x.agnc_cod, x.rep_cod, x.advter_cod  ");
                sbQuery.AppendLine("         ,x.cntr_nm ,y.advter_nm            ");
                sbQuery.AppendLine("         FROM CNTR x                        ");
                sbQuery.AppendLine("         INNER JOIN ADVTER y ON (x.advter_cod = y.advter_cod)   ");
                sbQuery.AppendLine(" )  t  ON (b.cntr_seq = t.cntr_seq)         ");      
                sbQuery.AppendLine(" LEFT JOIN STM_COD    e ON (b.advt_stt   = e.stm_cod AND e.stm_cod_cls = '25' )  ");
                sbQuery.AppendLine(" LEFT JOIN STM_COD    f ON (b.file_stt   = f.stm_cod AND f.stm_cod_cls = '31' )  ");
                sbQuery.AppendLine(" LEFT JOIN STM_COD    g ON (b.advt_typ   = g.stm_cod AND g.stm_cod_cls = '26' )  ");
                sbQuery.AppendLine(" WHERE 1=1");
                
                sbQuery.AppendLine(" AND a.camp_cod = '" + campaignModel.CampaignCode + "' ");
				
                if(!campaignModel.MediaCode.Equals("00"))   sbQuery.AppendLine("  AND    t.mda_cod = '"+campaignModel.MediaCode+"'    ");
                if(!campaignModel.RapCode.Equals("00"))     sbQuery.AppendLine("  AND    t.rep_cod = '"+campaignModel.RapCode+"'        ");
                if(!campaignModel.AgencyCode.Equals("00"))  sbQuery.AppendLine("  AND    t.agnc_cod = '"+campaignModel.AgencyCode+"'  ");
                sbQuery.AppendLine(" ORDER BY a.item_no Desc ");
                _log.Debug(sbQuery.ToString());
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고내역모델에 복사
				campaignModel.ContractDataSet = ds.Copy();
				campaignModel.ResultCnt = Utility.GetDatasetCount(campaignModel.ContractDataSet);
				campaignModel.ResultCD = "0000";

                /* 관련 테이블 없음 모바일 편성에서 사용안함.
                StringBuilder sbQuery2 = new StringBuilder();

                // 쿼리생성
                #region [광고내역 쿼리]
                sbQuery2.Append("\n");
                sbQuery2.Append("SELECT 'False' AS CheckYn  \n");
                sbQuery2.Append("   ,   PopupID             \n");
                sbQuery2.Append("   ,   CampaignID          \n");
                sbQuery2.Append("   ,   MeterialID          \n");
                sbQuery2.Append("   ,   PopupName           \n");
                sbQuery2.Append("FROM   CampaignDetailPns a with(noLock) \n");
                sbQuery2.Append("WHERE  CampaignCode = '" + campaignModel.CampaignCode + "'  \n");
                sbQuery2.Append("ORDER BY CampaignID Desc ");
                #endregion

                _log.Debug(sbQuery2.ToString());

                // 쿼리실행
                DataSet ds2 = new DataSet();
                _db.ExecuteQuery(ds2, sbQuery2.ToString());

                // 결과 DataSet의 광고내역모델에 복사
                campaignModel.CampaignDataSet = ds2.Copy();
                campaignModel.ResultCD = "0000";
                */

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + campaignModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				campaignModel.ResultCD = "3000";
				campaignModel.ResultDesc = "광고내역정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}      

		/// <summary>
		/// 광고내역팝업목록조회
		/// </summary>
		/// <param name="campaignModel"></param>
		public void GetContractItemPopList(HeaderModel header, CampaignModel campaignModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemPopList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + campaignModel.SearchKey       + "]");
				_log.Debug("ContractSeq      :[" + campaignModel.ContractSeq       + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
                #region 삭제 할 것
                /*
				// 쿼리생성
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
					+ "      ,(SELECT COUNT(*) FROM SchHome with(NoLock)                 WHERE ItemNo = A.ItemNo) AS HomeCount      \n"
					+ "      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail with(NoLock)     WHERE ItemNo = A.ItemNo) AS MenuCount      \n"
					+ "      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock)  WHERE ItemNo = A.ItemNo) AS ChannelCount   \n"
					+ "      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay                                                \n"
					+ "      ,A.AdType                      \n"
					+ "      ,E.CodeName AS AdTypeName      \n"
					+ "  FROM ContractItem A with(NoLock) INNER JOIN Contract   B with(NoLock) ON (B.MediaCode      = B.MediaCode AND A.RapCode = B.RapCode AND A.AgencyCode = B.AgencyCode AND A.AdvertiserCode = B.AdvertiserCode AND A.ContractSeq = B.ContractSeq) \n"
					+ "                       LEFT JOIN Advertiser C with(NoLock) ON (B.AdvertiserCode = C.AdvertiserCode)                \n"
					+ "	                      LEFT JOIN SystemCode D with(NoLock) ON (A.AdState        = D.Code      AND D.Section = '25')  \n"  // 25 : 광고상태
					+ "                       LEFT JOIN SystemCode E with(NoLock) ON (A.AdType         = E.Code      AND E.Section = '26')  \n"	// 26 : 광고종류
					+ " WHERE 1=1   \n"    
//					+ "   AND A.AdType  != '13'   \n"    
					);

				// 매체을 선택했으면
				if(campaignModel.SearchMediaCode.Trim().Length > 0 && !campaignModel.SearchMediaCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode  = '" + campaignModel.SearchMediaCode.Trim() + "' \n");
				}
	
				// 매체을 선택했으면
				if(campaignModel.ContractSeq.Trim().Length > 0 && !campaignModel.ContractSeq.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.ContractSeq  = '" + campaignModel.ContractSeq.Trim() + "' \n");
				}	

				bool isState = false;
				// 광고상태 선택에 따라

				// 광고상태 준비
				if(campaignModel.SearchchkAdState_10.Trim().Length > 0 && campaignModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND ( A.AdState  = '10' \n");
					isState = true;
				}	
				// 광고상태 편성
				if(campaignModel.SearchchkAdState_20.Trim().Length > 0 && campaignModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '20' \n");
					isState = true;
				}	
				// 광고상태 중지
				if(campaignModel.SearchchkAdState_30.Trim().Length > 0 && campaignModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '30' \n");
					isState = true;
				}	
				// 광고상태 종료
				if(campaignModel.SearchchkAdState_40.Trim().Length > 0 && campaignModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				
				// 검색어가 있으면
				if (campaignModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("\n"
						+ "  AND ( A.ItemName       LIKE '%" + campaignModel.SearchKey.Trim() + "%' \n"
						+ "     OR B.ContractName   LIKE '%" + campaignModel.SearchKey.Trim() + "%' \n"
						+ "  ) \n"
						);
				}
       
				sbQuery.Append("  ORDER BY A.ItemNo Desc ");
                */
                #endregion
                sbQuery.AppendLine(" SELECT     ");
                sbQuery.AppendLine("         'False' AS CheckYn                 ");
                sbQuery.AppendLine("         ,b.item_no as ItemNo 	            ");
                sbQuery.AppendLine("         ,b.item_nm as ItemName	            ");
                sbQuery.AppendLine("         ,t.cntr_nm as ContractName         ");                
                sbQuery.AppendLine("         ,t.advter_nm as AdvertiserName	    ");
                sbQuery.AppendLine("         ,b.begin_dy as ExcuteStartDay	    ");
                sbQuery.AppendLine("         ,b.end_dy as ExcuteEndDay	        ");
                sbQuery.AppendLine("         ,b.rl_end_dy as RealEndDay	        ");
                sbQuery.AppendLine("         ,b.advt_stt as AdState 	        ");
                sbQuery.AppendLine("         ,e.stm_cod_nm as AdStateName	    ");                
                sbQuery.AppendLine("         ,0 as HomeCount                    ");
                sbQuery.AppendLine("         ,(SELECT COUNT(*) FROM SCHD_MENU WHERE item_no = b.item_no) AS MenuCount      ");
                sbQuery.AppendLine("         ,(SELECT COUNT(*) FROM SCHD_TITLE   WHERE item_no = b.item_no) AS ChannelCount");
                sbQuery.AppendLine("         ,TO_CHAR(SYSDATE, 'YYYYMMDD') as NowDay    ");
                sbQuery.AppendLine("         ,b.advt_typ as AdType		        ");
                sbQuery.AppendLine("         ,g.stm_cod_nm as AdTypeName		");                
                sbQuery.AppendLine(" FROM ADVT_MST b                            ");
                sbQuery.AppendLine(" LEFT JOIN (                                ");
                sbQuery.AppendLine("         SELECT x.cntr_seq ,x.mda_cod, x.agnc_cod, x.rep_cod, x.advter_cod  ");
                sbQuery.AppendLine("         ,x.cntr_nm ,y.advter_nm            ");
                sbQuery.AppendLine("         FROM CNTR x                        ");
                sbQuery.AppendLine("         INNER JOIN ADVTER y ON (x.advter_cod = y.advter_cod)   ");
                sbQuery.AppendLine(" )  t  ON (b.cntr_seq = t.cntr_seq)         ");
                sbQuery.AppendLine(" LEFT JOIN STM_COD    e ON (b.advt_stt   = e.stm_cod AND e.stm_cod_cls = '25' )  ");
                sbQuery.AppendLine(" LEFT JOIN STM_COD    f ON (b.file_stt   = f.stm_cod AND f.stm_cod_cls = '31' )  ");
                sbQuery.AppendLine(" LEFT JOIN STM_COD    g ON (b.advt_typ   = g.stm_cod AND g.stm_cod_cls = '26' )  ");
                sbQuery.AppendLine(" WHERE 1=1 ");

                // 매체을 선택했으면
                if (campaignModel.SearchMediaCode.Trim().Length > 0 && !campaignModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND t.mda_cod  = '" + campaignModel.SearchMediaCode.Trim() + "' \n");
                }

                // 매체을 선택했으면
                if (campaignModel.ContractSeq.Trim().Length > 0 && !campaignModel.ContractSeq.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND t.cntr_seq  = '" + campaignModel.ContractSeq.Trim() + "' \n");
                }

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if (campaignModel.SearchchkAdState_10.Trim().Length > 0 && campaignModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( b.advt_stt  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (campaignModel.SearchchkAdState_20.Trim().Length > 0 && campaignModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" b.advt_stt  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (campaignModel.SearchchkAdState_30.Trim().Length > 0 && campaignModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" b.advt_stt  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (campaignModel.SearchchkAdState_40.Trim().Length > 0 && campaignModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" b.advt_stt  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");


                // 검색어가 있으면
                if (campaignModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( b.item_nm       LIKE '%" + campaignModel.SearchKey.Trim() + "%' \n"
                        + "     OR t.cntr_nm   LIKE '%" + campaignModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                sbQuery.Append("  ORDER BY b.item_no Desc ");

                // __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고내역모델에 복사
				campaignModel.ContractDataSet = ds.Copy();
				// 결과
				campaignModel.ResultCnt = Utility.GetDatasetCount(campaignModel.ContractDataSet);
				// 결과코드 셋트
				campaignModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + campaignModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemPopList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				campaignModel.ResultCD = "3000";
				campaignModel.ResultDesc = "광고내역정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

        /// <summary>
        /// 팝업내역목록 조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="campaignModel"></param>
        public void GetPnsList(HeaderModel header, CampaignModel campaignModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetPnsList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey   :[" + campaignModel.SearchKey + "]");
                _log.Debug("홈팝업      :[" + campaignModel.SearchchkAdState_10 + "]");
                _log.Debug("광고팝업    :[" + campaignModel.SearchchkAdState_20 + "]");
                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n " + "select *");
                sbQuery.Append("\n " + "from (");
                sbQuery.Append("\n " + "        select  'False'             as  CheckYn");
                sbQuery.Append("\n " + "            ,   'A'                 as  PnsType");
                sbQuery.Append("\n " + "            ,   notice_id           as  PnsId");
                sbQuery.Append("\n " + "            ,   notice_title        as  PnsName");
                sbQuery.Append("\n " + "            ,   notice_start_date   as  PnsBeginDt");
                sbQuery.Append("\n " + "            ,   notice_end_date     as  PnsEndDt");
                sbQuery.Append("\n " + "            ,   notice_gen_date     as  PnsRegDt");
                sbQuery.Append("\n " + "            ,   adpop_id            as  PnsItemNo");
                sbQuery.Append("\n " + "            ,   state               as  PnsState");
                sbQuery.Append("\n " + "            ,   campaign            as  PnsCampaign");
                sbQuery.Append("\n " + "            ,   material            as  PnsMaterial");
                sbQuery.Append("\n " + "        from IPTVPNS..HANARO_SMS.PNS_BRS_ADPOPUP"); 
                sbQuery.Append("\n " + "        where   del_yn = 'N'");
                sbQuery.Append("\n " + "        union all");
                sbQuery.Append("\n " + "        select  'False'             as  CheckYn");
                sbQuery.Append("\n " + "            ,   'H'     as PnsType");
                sbQuery.Append("\n " + "            ,   notice_id");
                sbQuery.Append("\n " + "            ,   notice_title");
                sbQuery.Append("\n " + "            ,   notice_start_date");
                sbQuery.Append("\n " + "            ,   notice_end_date");
                sbQuery.Append("\n " + "            ,   notice_gen_date");
                sbQuery.Append("\n " + "            ,   0   adpop_id");
                sbQuery.Append("\n " + "            ,   state");
                sbQuery.Append("\n " + "            ,   campaign            as  PnsCampaign");
                sbQuery.Append("\n " + "            ,   material            as  PnsMaterial");
                sbQuery.Append("\n " + "        from IPTVPNS..HANARO_SMS.PNS_BRS_POPUP ");
                sbQuery.Append("\n " + "        where   del_yn = 'N' ) v");
                sbQuery.Append("\n " + "where   1=1");

                bool isState = false;
                // 광고상태 준비
                if (campaignModel.SearchchkAdState_10.Trim().Length > 0 && campaignModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( PnsType  = 'H' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (campaignModel.SearchchkAdState_20.Trim().Length > 0 && campaignModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" PnsType  = 'A' \n");
                    isState = true;
                }
                if (isState) sbQuery.Append(" ) \n");


                // 검색어가 있으면
                if (campaignModel.SearchKey.Trim().Length > 0)
                {
                    sbQuery.Append("\n " + "  AND Pnsname like '%" + campaignModel.SearchKey.Trim() + "%' ");
                }

                sbQuery.Append("  ORDER BY PnsBeginDt Desc ");

                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 광고내역모델에 복사
                campaignModel.ContractDataSet = ds.Copy();
                campaignModel.ResultCnt = Utility.GetDatasetCount(campaignModel.ContractDataSet);
                campaignModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + campaignModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetPnsList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                campaignModel.ResultCD = "3000";
                campaignModel.ResultDesc = "팝업내역 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

		/// <summary>
		/// 캠페인 생성
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetCampaignCreate(HeaderModel header, CampaignModel campaignModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCampaignCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[3];
                
                sbQuery.AppendLine("    INSERT INTO CAMP_MST (		        ");                
                sbQuery.AppendLine("            camp_cod			        ");
                sbQuery.AppendLine("        ,   cntr_seq				    ");
                sbQuery.AppendLine("        ,   camp_nm				        ");
                sbQuery.AppendLine("        ,   camp_prc					");                
                sbQuery.AppendLine("     	,   use_yn					    ");
                sbQuery.AppendLine("          )                             ");
                sbQuery.AppendLine("     SELECT						        ");
                sbQuery.AppendLine("            NVL(MAX(camp_cod),0)+1      ");
                sbQuery.AppendLine("        ,   :ContractSeq				");
                sbQuery.AppendLine("        ,   :CampaignName		        ");
                sbQuery.AppendLine("        ,   :Price					    ");                
                sbQuery.AppendLine("        ,   'Y'						    ");                
                sbQuery.AppendLine("     FROM CAMP_MST                    ");

                sqlParams[i++] = new OracleParameter(":ContractSeq", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":CampaignName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":Price", OracleDbType.Int32);                				

				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(campaignModel.ContractSeq);
				sqlParams[i++].Value = campaignModel.CampaignName;
				sqlParams[i++].Value = Convert.ToInt32(	campaignModel.Price	);                

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("캠페인:[" + campaignModel.CampaignCode + "(" + campaignModel.CampaignName + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				campaignModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCampaignCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				campaignModel.ResultCD   = "3101";
				campaignModel.ResultDesc = "캠페인 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		/// <summary>
		/// 캠페인정보수정
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetCampaignUpdate(HeaderModel header, CampaignModel campaignModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCampaignUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[5];               
                sbQuery.AppendLine("UPDATE CAMP_MST						    ");
                sbQuery.AppendLine("   SET camp_nm      = :CampaignName     ");
                sbQuery.AppendLine("      ,use_yn	    = :UseYn		    ");
                sbQuery.AppendLine("      ,camp_prc		= :Price		    ");                
                sbQuery.AppendLine("WHERE camp_cod		= :CampaignCode     ");
                sbQuery.AppendLine("AND   cntr_seq		= :ContractSeq	    ");

				i = 0;
				sqlParams[i++] = new OracleParameter(":CampaignName"	, OracleDbType.Varchar2	,40);													
				sqlParams[i++] = new OracleParameter(":UseYn"			, OracleDbType.Char,1);
				sqlParams[i++] = new OracleParameter(":Price"			, OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":CampaignCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":ContractSeq", OracleDbType.Int32);
                
				

				i = 0;
				sqlParams[i++].Value = campaignModel.CampaignName;													
				sqlParams[i++].Value = campaignModel.UseYn;
				sqlParams[i++].Value = Convert.ToInt32(	campaignModel.Price	);
                sqlParams[i++].Value = Convert.ToInt32(campaignModel.CampaignCode);
                sqlParams[i++].Value = Convert.ToInt32(campaignModel.ContractSeq);
				

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("캠페인정보수정:["+campaignModel.CampaignCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				campaignModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCampaignUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				campaignModel.ResultCD   = "3201";
				campaignModel.ResultDesc = "캠페인 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			// 데이터베이스를  Close한다
			_db.Close();

		}

		public void SetCampaignDelete(HeaderModel header, CampaignModel campaignModel)
		{
			int GroupDetailCount = 0;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCampaignDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQueryCmapaignDetailCount = new StringBuilder();
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[1];
                OracleParameter[] sqlParams2= new OracleParameter[2];

				sbQueryCmapaignDetailCount.Append( "\n"
					+ "        SELECT COUNT(*) FROM    CAMP_DTL			    \n"
					+ "              WHERE camp_cod  = :CampaignCode          	\n"
					);  

				sbQuery.Append(""
					+ "DELETE CAMP_MST				 \n"					
					+ "   WHERE camp_cod  = :CampaignCode  \n"
					+ "   AND   cntr_seq  = :ContractSeq	 \n"
					);

                i = 0;
                sqlParams[i++] = new OracleParameter(":CampaignCode", OracleDbType.Int32);

                i = 0;
                sqlParams2[i++] = new OracleParameter(":CampaignCode", OracleDbType.Int32);
                sqlParams2[i++] = new OracleParameter(":ContractSeq", OracleDbType.Int32);

				i = 0;
                sqlParams[i++].Value = Convert.ToInt32(campaignModel.CampaignCode);

                i = 0;
				sqlParams2[i++].Value = Convert.ToInt32(campaignModel.CampaignCode);
				sqlParams2[i++].Value = Convert.ToInt32(campaignModel.ContractSeq);

				// 쿼리실행
				try
				{
					//매체대행광고주 관계 Count조사///////////////////////////////////////////////
					// __DEBUG__
					_log.Debug(sbQueryCmapaignDetailCount.ToString());
					// __DEBUG__

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQueryCmapaignDetailCount.ToString(),sqlParams);
                    
					GroupDetailCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					_log.Debug("GroupDetailCount          -->" + GroupDetailCount);

					// 이미 다른테이블에 사용중인 데이터가 있다면 Exception를 발생시킨다.
					if(GroupDetailCount > 0) throw new Exception();


					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams2);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("캠페인정보삭제:[" + campaignModel.CampaignCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				campaignModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCampaignDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				campaignModel.ResultCD   = "3301";
				// 이미 다른테이블에 사용중인 데이터가 있다면
				if(GroupDetailCount > 0 )
				{
					campaignModel.ResultDesc = "등록된 캠페인디테일이 있으므로 그룹정보를 삭제할수 없습니다.";
				}
				else
				{
					campaignModel.ResultDesc = "캠페인정보 삭제중 오류가 발생하였습니다";
				}
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		#region 캠페인 디테일 생성
		/// <summary>
		/// 캠페인 디테일-광고 생성
		/// </summary>
		/// <returns></returns>
		public void SetCampaignDetailCreate(HeaderModel header, CampaignModel campaignModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetCampaignDetailCreate() Start");
				_log.Debug("-----------------------------------------");

				
				// 쿼리실행
				try
				{
					int i = 0;
					int rc = 0;

					// 현재 승인번호를 구함
					
					StringBuilder  sbQuery   = new StringBuilder();
                    OracleParameter[] sqlParams = new OracleParameter[2];
		
					i = 0;
                    sqlParams[i++] = new OracleParameter(":CampaignCode", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);					

					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(campaignModel.CampaignCode);
					sqlParams[i++].Value = Convert.ToInt32(campaignModel.ItemNo);

					_db.BeginTran();

					// 홈광고 편성 테이블에 추가
					sbQuery.Append( "\n"
						+ "INSERT INTO CAMP_DTL (                   \n"
						+ "       camp_cod                     \n"						
						+ "      ,item_no                           \n"						
						+ "      )                                 \n"
						+ " VALUES(                                \n"					
						+ "       :CampaignCode					   \n"						
						+ "      ,:ItemNo                          \n"						
						+ "		 )								   \n"						
						);

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					
					_db.CommitTran();
					
					// __MESSAGE__
					_log.Message("캠페인 디테일:[" + campaignModel.ItemNo + "][" + campaignModel.CampaignCode + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				campaignModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetCampaignDetailCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				campaignModel.ResultCD   = "3101";
				campaignModel.ResultDesc = "캠페인 디테일 저장 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

        /// <summary>
        /// 캠페인 디테일-팝업 생성
        /// </summary>
        /// <param name="header"></param>
        /// <param name="campaignModel"></param>
        public void SetCampaignPnsCreate(HeaderModel header, CampaignModel campaignModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetCampaignPnsCreate() Start");
				_log.Debug("-----------------------------------------");
                				
				// 쿼리실행
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();
					SqlParameter[] sqlParams = new SqlParameter[5];
		
					sqlParams[0] = new SqlParameter(":CampaignCode" ,SqlDbType.Int      ,4);
                    sqlParams[1] = new SqlParameter(":CampaignID"   ,SqlDbType.Int      ,4);
                    sqlParams[2] = new SqlParameter(":MeterialID"   ,SqlDbType.Int      ,4);
                    sqlParams[3] = new SqlParameter(":PopupID"      ,SqlDbType.VarChar  ,12);
                    sqlParams[4] = new SqlParameter(":PopUpName"    ,SqlDbType.VarChar  ,50);

					sqlParams[0].Value = Convert.ToInt32(campaignModel.CampaignCode);
                    sqlParams[1].Value = Convert.ToInt32(campaignModel.AgencyCode);
                    sqlParams[2].Value = Convert.ToInt32(campaignModel.AdvertiserCode);
                    sqlParams[3].Value = campaignModel.ItemNo;
                    sqlParams[4].Value = campaignModel.CampaignName;

					// 홈광고 편성 테이블에 추가
                    sbQuery.Append("\n insert into CampaignDetailPns");
                    sbQuery.Append("\n      (   CampaignCode        ");
                    sbQuery.Append("\n      ,   CampaignID          ");
                    sbQuery.Append("\n      ,   MeterialID          ");
                    sbQuery.Append("\n      ,   PopupID             ");
                    sbQuery.Append("\n      ,   PopUpName   )       ");
                    sbQuery.Append("\n values ( :CampaignCode       ");
                    sbQuery.Append("\n      ,   :CampaignID         ");
                    sbQuery.Append("\n      ,   :MeterialID         ");
                    sbQuery.Append("\n      ,   :PopupID            ");
                    sbQuery.Append("\n      ,   :PopUpName   )      ");

                    _db.Open();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    
					_log.Message("캠페인_팝업 디테일:[" + campaignModel.ItemNo + "][" + campaignModel.CampaignCode + "]");

				}
				catch(Exception ex)
				{
					throw ex;
				}

				campaignModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetCampaignPnsCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				campaignModel.ResultCD   = "3101";
				campaignModel.ResultDesc = "캠페인_팝업 디테일 저장 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}
		#endregion 	
	
		#region 캠페인 디테일 삭제
		/// <summary>
		/// 캠페인 디테일-광고 삭제
		/// </summary>
		/// <returns></returns>
		public void SetCampaignDetailDelete(HeaderModel header, CampaignModel campaignModel)
		{			
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCampaignDetailDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();				
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[2];
				
				sbQuery.Append(""
					+ "DELETE CAMP_DTL         \n"
					+ " WHERE camp_cod  = :CampaignCode  \n"
					+ "   AND item_no	= :ItemNo  \n"					
					);

                sqlParams[i++] = new OracleParameter(":CampaignCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);				

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(campaignModel.CampaignCode);
				sqlParams[i++].Value = Convert.ToInt32(campaignModel.ItemNo);				

				// 쿼리실행
				try
				{										
					// __DEBUG__

					// 쿼리실행
					DataSet ds = new DataSet();					

					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("그룹정보삭제:[" + campaignModel.CampaignCode + "] [" + campaignModel.ItemNo + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				campaignModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCampaignDetailDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				campaignModel.ResultCD   = "3301";
				// 이미 다른테이블에 사용중인 데이터가 있다면				
				campaignModel.ResultDesc = "캠페인정보 삭제중 오류가 발생하였습니다";				
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

        /// <summary>
        /// 캠페인 디테일-팝업 삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="campaignModel"></param>
        public void SetCampaignPnsDelete(HeaderModel header, CampaignModel campaignModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetCampaignPnsDelete() Start");
                _log.Debug("-----------------------------------------");

                int rc = 0;
                
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n DELETE   CampaignDetailPns ");
                sbQuery.Append("\n WHERE    CampaignCode  = :CampaignCode");
                sbQuery.Append("\n AND      PopupID       = :PopupID");

                SqlParameter[] sqlParams = new SqlParameter[2];
                sqlParams[0] = new SqlParameter(":CampaignCode", SqlDbType.Int, 4);
                sqlParams[1] = new SqlParameter(":PopupID", SqlDbType.VarChar, 12);

                sqlParams[0].Value = Convert.ToInt32(campaignModel.CampaignCode);
                sqlParams[1].Value = campaignModel.ItemNo;
                
                // 쿼리실행
                try
                {
                    _db.Open();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _log.Message("캠페인내역-팝업정보삭제:[" + campaignModel.CampaignCode + "] [" + campaignModel.ItemNo + "] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                campaignModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetCampaignPnsDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                campaignModel.ResultCD = "3301";
                campaignModel.ResultDesc = "캠페인내역-팝업 삭제중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
		#endregion 	

	}
}