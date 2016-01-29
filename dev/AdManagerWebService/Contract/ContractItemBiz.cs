/*
 * -------------------------------------------------------
 * Class Name: ContractItemBiz
 * 주요기능  : 계약관리 처리 로직
 * 작성자    :  
 * 작성일    : 
 * 특이사항  : 
 * -------------------------------------------------------
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
using System.Configuration;

namespace AdManagerWebService.ContractItem
{
    /// <summary>
    /// ContractItemBiz에 대한 요약 설명입니다.
    /// </summary>
    public class ContractItemBiz : BaseBiz
    {
        public ContractItemBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }


		/// <summary>
		///  등급콤보조회
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetGradeCodeList(HeaderModel header, ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGradeList() Start");
				_log.Debug("-----------------------------------------");
				
				// 데이터베이스를 OPEN한다
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("SELECT Code, CodeName FROM GradeCode with(noLock)  \n");
				sbQuery.Append(" ORDER BY Code \n");
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				contractItemModel.GradeDataSet = ds.Copy();
				contractItemModel.ResultCnt = Utility.GetDatasetCount(contractItemModel.GradeDataSet);
				contractItemModel.ResultCD = "0000";
			}
			catch(Exception ex)
			{
				contractItemModel.ResultCD = "3000";
				contractItemModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}


        /// <summary>
        /// 광고내역목록조회
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetContractItemList(HeaderModel header, ContractItemModel contractItemModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + contractItemModel.SearchKey       + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                #region 삭제 할 것
                /*
                // 쿼리생성
                sbQuery.Append("\n");
				sbQuery.Append("SELECT * FROM ( 	                                \n");     
				sbQuery.Append("SELECT a.ItemNo 	                                \n");     
                sbQuery.Append("      ,a.ItemName	                                \n");     
                sbQuery.Append("      ,e.ContractName                               \n");     
                sbQuery.Append("	  ,a.AdvertiserCode	                            \n");
                sbQuery.Append("      ,b.AdvertiserName	                            \n");
                sbQuery.Append("      ,a.ExcuteStartDay	                            \n");
                sbQuery.Append("	  ,a.ExcuteEndDay	                            \n");
				sbQuery.Append("	  ,a.RealEndDay	                                \n");
                sbQuery.Append("	  ,a.AdState 	                                \n");				
                sbQuery.Append("      ,c.CodeName AdStateName	                    \n");
				sbQuery.Append("	  ,a.AdClass 	                                \n");
				sbQuery.Append("      ,a.FilePath	                                \n");
				sbQuery.Append("      ,a.FileName	                                \n");
                sbQuery.Append("      ,a.FileState		                            \n");
                sbQuery.Append("      ,d.CodeName FileStateName		                \n");
				sbQuery.Append("      ,(SELECT COUNT(*) FROM SchHome                WHERE ItemNo = a.ItemNo)");
				sbQuery.Append("			+(SELECT COUNT(*) FROM SchHomeKids            WHERE ItemNo = a.ItemNo) AS HomeCount      \n");  //[E_03]
				sbQuery.Append("      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail    WHERE ItemNo = a.ItemNo) AS MenuCount      \n");
                sbQuery.Append("      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail WHERE ItemNo = a.ItemNo) AS ChannelCount   \n");
				sbQuery.Append("      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay     \n");
				sbQuery.Append("      ,a.AdType		                                \n");
				sbQuery.Append("      ,f.CodeName AdTypeName		                \n");
				sbQuery.Append("      ,a.LinkChannel		                \n");				
				sbQuery.Append("      ,(SELECT TOP 1 ProgramNm FROM Program    WHERE Channel = a.LinkChannel ORDER BY ProgramKey DESC) AS LinkChannelNm      \n");
				sbQuery.Append("      ,a.Mgrade		                \n");
				sbQuery.Append("      ,a.HomeYn		                \n");
				sbQuery.Append("      ,a.ChannelYn		            \n");
				sbQuery.Append("      ,a.CugYn                      \n");
                sbQuery.Append("      ,g.CodeName ScheduleTypeName  \n");
				sbQuery.Append("      ,a.STBType					\n");
				sbQuery.Append("      ,a.AdRate						\n");
				sbQuery.Append("      ,a.AdTime						\n");
				sbQuery.Append("FROM   ContractItem a with(NoLock)  \n");
				sbQuery.Append("       LEFT JOIN Advertiser b with(NoLock) ON (a.AdvertiserCode = b.AdvertiserCode)             \n");
				sbQuery.Append("       LEFT JOIN Contract   e with(NoLock) ON (a.ContractSeq    = e.ContractSeq)                \n");    
				sbQuery.Append("       LEFT JOIN SystemCode c with(NoLock) ON (a.AdState        = c.Code AND c.Section = '25' ) \n");      
                sbQuery.Append("       LEFT JOIN SystemCode d with(NoLock) ON (a.FileState      = d.Code AND d.Section = '31' ) \n");
				sbQuery.Append("       LEFT JOIN SystemCode f with(NoLock) ON (a.AdType         = f.Code AND f.Section = '26' ) \n");
                sbQuery.Append("       LEFT JOIN SystemCode g with(NoLock) ON (a.ScheduleType   = g.Code AND g.Section = '27' ) \n");    				
				sbQuery.Append(" WHERE 1=1	                                        \n");
				
                if(!contractItemModel.MediaCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.MediaCode = '"+contractItemModel.MediaCode+"'  \n");
                }
                if(!contractItemModel.RapCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.RapCode = '"+contractItemModel.RapCode+"'  \n");
                }        
                if(!contractItemModel.AgencyCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.AgencyCode = '"+contractItemModel.AgencyCode+"'  \n");
                }     
                if(!contractItemModel.AdvertiserCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.AdvertiserCode = '"+contractItemModel.AdvertiserCode+"'  \n");
                } 
				if(!contractItemModel.AdType.Equals("00"))
				{
					sbQuery.Append("  AND    a.AdType = '"+contractItemModel.AdType+"'  \n");
				} 
                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if(contractItemModel.SearchchkAdState_10.Trim().Length > 0 && contractItemModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.AdState  = '10' \n");
                    isState = true;
                }	
                // 광고상태 편성
                if(contractItemModel.SearchchkAdState_20.Trim().Length > 0 && contractItemModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '20' \n");
                    isState = true;
                }	
                // 광고상태 중지
                if(contractItemModel.SearchchkAdState_30.Trim().Length > 0 && contractItemModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '30' \n");
                    isState = true;
                }	
                // 광고상태 종료
                if(contractItemModel.SearchchkAdState_40.Trim().Length > 0 && contractItemModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.AdState  = '40' \n");
                    isState = true;
                }	

                if(isState) sbQuery.Append(" ) \n");

                // 검색어가 있으면
                if (contractItemModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( a.ItemName LIKE '%" + contractItemModel.SearchKey.Trim() + "%' OR		\n"
                        + "        a.FileName LIKE '%" + contractItemModel.SearchKey.Trim() + "%'     \n"
                        + " OR b.AdvertiserName    LIKE '%" + contractItemModel.SearchKey.Trim() + "%'			\n"
                        + "		)        \n"
                        );
                }

				sbQuery.Append(""
						+ ") TA "
						+ "WHERE 1 = 1"
						);

				// 편성여부가 Y/N일 때 
				if(contractItemModel.SearchChkSch_YN.Trim().Length > 0)
				{
					if(contractItemModel.SearchChkSch_YN.Trim().Equals("Y"))
					{
						// 편성된것만
						sbQuery.Append(" AND ( HomeCount > 0 OR MenuCount > 0 OR  ChannelCount > 0 ) \n");
					}
					else if(contractItemModel.SearchChkSch_YN.Trim().Equals("N"))
					{
						//편성안된것만
						sbQuery.Append(" AND ( HomeCount = 0 AND MenuCount = 0 AND  ChannelCount = 0 )\n");
					}
				}
				// 아니면 전체
       
                sbQuery.Append("ORDER BY ItemNo Desc ");
                */
                #endregion

                // 쿼리생성
                sbQuery.Append("\n");
                sbQuery.Append("\n SELECT * FROM (  ");
                sbQuery.Append("\n          SELECT  a.item_no as ItemNo     ");
                sbQuery.Append("\n              ,a.item_nm as ItemName	    ");
                sbQuery.Append("\n              ,t.cntr_nm as ContractName	");
                sbQuery.Append("\n              ,t.advter_cod as AdvertiserCode ");
                sbQuery.Append("\n              ,t.advter_nm as AdvertiserName	");
                sbQuery.Append("\n              ,a.begin_dy as ExcuteStartDay	");
                sbQuery.Append("\n              ,a.end_dy as ExcuteEndDay		");
                sbQuery.Append("\n              ,a.rl_end_dy as RealEndDay		");
                sbQuery.Append("\n              ,a.advt_stt as AdState 			");
                sbQuery.Append("\n              ,c.stm_cod_nm as AdStateName	");
                sbQuery.Append("\n	            ,a.advt_clss as AdClass 	    ");
                sbQuery.Append("\n              ,a.file_path  as FilePath		");
                sbQuery.Append("\n              ,a.file_nm  as FileName			");
                sbQuery.Append("\n              ,a.file_stt as FileState		");
                sbQuery.Append("\n              ,d.stm_cod_nm as FileStateName	");
                sbQuery.Append("\n              ,0 as HomeCount                 ");
                sbQuery.Append("\n              ,(SELECT COUNT(*) FROM SCHD_MENU    WHERE item_no = a.item_no) AS MenuCount		");
                sbQuery.Append("\n              ,(SELECT COUNT(*) FROM SCHD_TITLE WHERE item_no = a.item_no) AS ChannelCount    ");
                sbQuery.Append("\n              ,TO_CHAR(SYSDATE,'YYYYMMDD') AS NowDay    ");
                sbQuery.Append("\n              ,a.advt_typ   as AdType			");
                sbQuery.Append("\n              ,f.stm_cod_nm as AdTypeName		");
                sbQuery.Append("\n              ,'' as LinkChannel	        ");
                sbQuery.Append("\n              ,'' as LinkChannelNm        ");
                sbQuery.Append("\n              ,'' as Mgrade               ");
                sbQuery.Append("\n              ,'' as HomeYn	            ");
                sbQuery.Append("\n              ,'' as ChannelYn            ");
                sbQuery.Append("\n              ,'' as CugYn                ");
                sbQuery.Append("\n              ,g.stm_cod_nm as ScheduleTypeName  ");
                sbQuery.Append("\n              ,'' as STBType    			\n");
                sbQuery.Append("\n              ,a.advt_rate as AdRate	    \n");
                sbQuery.Append("\n              ,a.advt_tm as AdTime		\n");
                sbQuery.Append("\n          FROM   ADVT_MST a             \n");
                sbQuery.Append("\n          LEFT JOIN ( SELECT x.mda_cod, x.agnc_cod, x.rep_cod ");
                sbQuery.Append("\n                             ,y.advter_cod ,y.advter_nm       ");
                sbQuery.Append("\n                             ,x.cntr_seq   ,x.cntr_nm         ");
                sbQuery.Append("\n                      FROM CNTR x                             ");
                sbQuery.Append("\n                      LEFT JOIN ADVTER y ON (x.advter_cod = y.advter_cod) ");
                sbQuery.Append("\n          ) t ON (a.cntr_seq = t.cntr_seq)    ");
                sbQuery.Append("\n          LEFT JOIN STM_COD c ON (a.advt_stt      = c.stm_cod AND c.stm_cod_cls = '25' ) ");
                sbQuery.Append("\n          LEFT JOIN STM_COD d ON (a.file_stt      = d.stm_cod AND d.stm_cod_cls = '31' ) ");
                sbQuery.Append("\n          LEFT JOIN STM_COD f ON (a.advt_typ      = f.stm_cod AND f.stm_cod_cls = '26' ) ");
                sbQuery.Append("\n          LEFT JOIN STM_COD g ON (a.sch_typ       = g.stm_cod AND g.stm_cod_cls = '27' ) ");
                sbQuery.Append("\n          WHERE 1=1	  ");

				//if (!contractItemModel.MediaCode.Equals("00"))
				//{
				//    sbQuery.Append("\n  AND    t.mda_cod = '" + contractItemModel.MediaCode + "' ");
				//}
                if (!contractItemModel.RapCode.Equals("00"))
                {
                    sbQuery.Append("\n  AND    t.rep_cod = '" + contractItemModel.RapCode + "' ");
                }
                if (!contractItemModel.AgencyCode.Equals("00"))
                {
                    sbQuery.Append("\n  AND    t.agnc_cod = '" + contractItemModel.AgencyCode + "' ");
                }
                if (!contractItemModel.AdvertiserCode.Equals("00"))
                {
                    sbQuery.Append("\n  AND    t.advter_cod = '" + contractItemModel.AdvertiserCode + "'  ");
                }
                if (!contractItemModel.AdType.Equals("00"))
                {
                    sbQuery.Append("\n  AND    a.advt_typ = '" + contractItemModel.AdType + "' ");
                }

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if (contractItemModel.SearchchkAdState_10.Trim().Length > 0 && contractItemModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( A.advt_stt  = '10' \n");
                    isState = true;
                }
                // 광고상태 편성
                if (contractItemModel.SearchchkAdState_20.Trim().Length > 0 && contractItemModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (contractItemModel.SearchchkAdState_30.Trim().Length > 0 && contractItemModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (contractItemModel.SearchchkAdState_40.Trim().Length > 0 && contractItemModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.advt_stt  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");

                // 검색어가 있으면
                if (contractItemModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "   AND ( a.item_nm LIKE '%" + contractItemModel.SearchKey.Trim() + "%' OR	"
                        + "\n      a.file_nm LIKE '%" + contractItemModel.SearchKey.Trim() + "%'    "
                        + "\n OR t.advter_nm    LIKE '%" + contractItemModel.SearchKey.Trim() + "%'	"
                        + "\n		)  "
                        );
                }

                sbQuery.Append(""
                        + "\n ) TA "
                        + "\n WHERE 1 = 1"
                        );

                // 편성여부가 Y/N일 때 
                if (contractItemModel.SearchChkSch_YN.Trim().Length > 0)
                {
                    if (contractItemModel.SearchChkSch_YN.Trim().Equals("Y"))
                    {
                        // 편성된것만
                        sbQuery.Append("\n AND ( HomeCount > 0 OR MenuCount > 0 OR  ChannelCount > 0 ) ");
                    }
                    else if (contractItemModel.SearchChkSch_YN.Trim().Equals("N"))
                    {
                        //편성안된것만
                        sbQuery.Append("\n AND ( HomeCount = 0 AND MenuCount = 0 AND  ChannelCount = 0 )");
                    }
                }
                // 아니면 전체

                sbQuery.Append("\n ORDER BY ItemNo Desc ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 광고내역모델에 복사
                contractItemModel.ContractItemDataSet = ds.Copy();
                // 결과
                contractItemModel.ResultCnt = Utility.GetDatasetCount(contractItemModel.ContractItemDataSet);
                // 결과코드 셋트
                contractItemModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + contractItemModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                contractItemModel.ResultCD = "3000";
                contractItemModel.ResultDesc = "광고내역정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


        /// <summary>
        /// 광고내역상세조회
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetContractItemDetail(HeaderModel header, ContractItemModel contractItemModel)
        {
            try
            {
                _db.Open();

				_log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemDetail() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("<입력정보>");
             
                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();

                #region 삭제할 것
                /*
                // 쿼리생성
                sbQuery.Append("\n");									
                sbQuery.Append("SELECT a.ItemNo			-- 광고내역정보 시작	\n");
                sbQuery.Append("	  ,a.ItemName								\n");
                sbQuery.Append("      ,a.AdState								\n");
                sbQuery.Append("      ,a.ExcuteStartDay							\n");
                sbQuery.Append("      ,a.ExcuteEndDay							\n");
                sbQuery.Append("      ,a.RealEndDay								\n");
                sbQuery.Append("      ,a.AgencyCode								\n");
                sbQuery.Append("	  ,a.ScheduleType							\n");
                sbQuery.Append("	  ,a.AdClass								\n");
                sbQuery.Append("	  ,a.AdType									\n");
                sbQuery.Append("	  ,a.AdRate									\n");
                sbQuery.Append("	  ,a.AdTime									\n");
                sbQuery.Append("	  ,convert(char(19),a.RegDt,120) RegDt 	    \n");
                sbQuery.Append("      ,convert(char(19),a.ModDt,120) ModDt	    \n");
                sbQuery.Append("      ,c.UserName RegName 	                    \n");
                sbQuery.Append("	  ,a.FileName       --파일정보 시작        	\n");
                sbQuery.Append("	  ,d.CodeName FileTypeName	                \n");
                sbQuery.Append("	  ,a.FileLength	                            \n");
                sbQuery.Append("	  ,a.FilePath                               \n");
                sbQuery.Append("	  ,e.CodeName FileStateName	                \n");
                sbQuery.Append("	  ,a.DownLevel	                            \n");
                sbQuery.Append("	  ,convert(char(19),a.FileRegDt,120) FileRegDt   \n");
                sbQuery.Append("	  ,k.UserName FileRegName                   \n");
                sbQuery.Append("	  ,b.ContractSeq    --계약정보 시작            \n");
                sbQuery.Append("	  ,b.ContractName                    	    \n");
                sbQuery.Append("	  ,f.CodeName State	                        \n");
                sbQuery.Append("	  ,g.MediaName	                            \n");
                sbQuery.Append("	  ,h.RapName	                            \n");
                sbQuery.Append("	  ,i.AgencyName	                            \n");
                sbQuery.Append("	  ,b.ContStartDay	                        \n");
                sbQuery.Append("	  ,b.ContEndDay	                            \n");
                sbQuery.Append("	  ,b.Comment	                            \n");
                sbQuery.Append("	  ,convert(char(19),b.RegDt,120) ContractRegDt	 \n");
                sbQuery.Append("	  ,convert(char(19),b.ModDt,120) ContractModDt	 \n");
                sbQuery.Append("	  ,j.UserName ContractRegName	    \n");
				sbQuery.Append("	  ,a.MediaCode                	    \n");
				sbQuery.Append("      ,a.LinkChannel		            \n");
				sbQuery.Append("      ,l.ProgramNm LinkChannelNm		\n");
				sbQuery.Append("      ,a.Mgrade		                    \n");
				sbQuery.Append("      ,a.HomeYn		                    \n");
				sbQuery.Append("      ,a.ChannelYn		                \n");
				sbQuery.Append("      ,a.CugYn                          \n");
				sbQuery.Append("      ,isnull(a.STBType,0) as STBType	\n");
                sbQuery.Append("  FROM ContractItem a with(NoLock)      \n");				
                sbQuery.Append("       INNER JOIN Contract   b with(NoLock) ON (a.ContractSeq  = b.ContractSeq)	           \n");
                sbQuery.Append("       LEFT  JOIN SystemUser c with(NoLock) ON (a.RegId        = c.UserId)                  \n");
                sbQuery.Append("	   LEFT  JOIN SystemCode d with(NoLock) ON (a.FileType     = d.Code AND d.Section='24') \n");
                sbQuery.Append("	   LEFT  JOIN SystemCode e with(NoLock) ON (a.FileState    = e.Code AND e.Section='31') \n");
                sbQuery.Append("	   LEFT  JOIN SystemCode f with(NoLock) ON (b.State        = f.Code AND f.Section='23') \n");
                sbQuery.Append("	   LEFT  JOIN Media      g with(NoLock) ON (b.MediaCode    = g.MediaCode)	           \n");
                sbQuery.Append("	   LEFT  JOIN MediaRap   h with(NoLock) ON (b.RapCode      = h.RapCode)	               \n");
                sbQuery.Append("	   LEFT  JOIN Agency     i with(NoLock) ON (b.AgencyCode   = i.AgencyCode)	           \n");
                sbQuery.Append("	   LEFT  JOIN SystemUser j with(NoLock) ON (b.RegId        = j.UserId)	               \n");
                sbQuery.Append("	   LEFT  JOIN SystemUser k with(NoLock) ON (a.FileRegID    = k.UserId)	               \n");
				sbQuery.Append("       LEFT  JOIN Program l with(NoLock) ON (a.LinkChannel         = l.channel) \n");    
                sbQuery.Append(" WHERE a.ItemNo = '"+contractItemModel.ItemNo.Trim().ToString()+"'\n");
				
                */
                #endregion

                sbQuery.Append("\n SELECT ");
                sbQuery.Append("\n      a.item_no as ItemNo			");
                sbQuery.Append("\n     ,a.item_nm as ItemName		");						
                sbQuery.Append("\n     ,a.advt_stt as AdState		");						
                sbQuery.Append("\n     ,a.begin_dy as ExcuteStartDay	");						
                sbQuery.Append("\n     ,a.end_dy as ExcuteEndDay		");					
                sbQuery.Append("\n     ,a.rl_end_dy as RealEndDay		");						
                sbQuery.Append("\n     ,i.agnc_cod as AgencyCode		");						
                sbQuery.Append("\n     ,a.sch_typ as ScheduleType		");					
                sbQuery.Append("\n     ,a.advt_clss as AdClass			");					
                sbQuery.Append("\n     ,a.advt_typ as AdType			");						
                sbQuery.Append("\n     ,a.advt_rate as AdRate			");						
                sbQuery.Append("\n     ,a.advt_tm as AdTime				");					
                sbQuery.Append("\n     ,TO_CHAR(a.dt_insert,'YYYY-MM-DD HH:MM:SS')  as  RegDt  "); 	    
                sbQuery.Append("\n     ,TO_CHAR(a.dt_update,'YYYY-MM-DD HH:MM:SS')  as ModDt   ");	    
                sbQuery.Append("\n     ,c.user_nm as RegName 	        ");            
                sbQuery.Append("\n     ,a.file_nm as FileName           "); //파일정보 시작        	
                sbQuery.Append("\n     ,d.stm_cod_nm as FileTypeName    ");	                
                sbQuery.Append("\n     ,a.file_len as FileLength	    ");                        
                sbQuery.Append("\n     ,a.file_path as FilePath         ");                      
                sbQuery.Append("\n     ,e.stm_cod_nm as FileStateName	");                
                sbQuery.Append("\n      ,'' as DownLevel	            ");                
                sbQuery.Append("\n     ,TO_CHAR(a.file_reg_dt,'YYYY-MM-DD HH:MM:SS') as  FileRegDt  ");      
                sbQuery.Append("\n     ,k.user_nm as FileRegName        ");           
                sbQuery.Append("\n     ,b.cntr_seq as ContractSeq       "); //--계약정보 시작            
                sbQuery.Append("\n     ,b.cntr_nm as ContractName       ");             	    
                sbQuery.Append("\n     ,f.stm_cod_nm as State	        ");                
                sbQuery.Append("\n     ,g.mda_nm as MediaName	        ");                    
                sbQuery.Append("\n     ,h.rep_nm as RapName	            ");                
                sbQuery.Append("\n     ,i.agnc_nm as AgencyName	        ");                    
                sbQuery.Append("\n     ,b.cntr_begin_dy as ContStartDay	");                        
                sbQuery.Append("\n     ,b.cntr_end_dy    as ContEndDay	");                            
                sbQuery.Append("\n     ,b.cntr_memo as \"Comment\"      ");
                sbQuery.Append("\n     ,TO_CHAR(a.dt_insert,'YYYY-MM-DD HH:MM:SS')  as ContractRegDt ");	  /*CNTR 에 없어서 ADVT_MST 있는 것으로 */
                sbQuery.Append("\n     ,TO_CHAR(a.dt_update,'YYYY-MM-DD HH:MM:SS')  as ContractModDt  ");	 /* CNTR 에 없어서 ADVT_MST 있는 것으로*/
                sbQuery.Append("\n     ,'' as ContractRegName	        ");
                sbQuery.Append("\n     ,b.mda_cod as MediaCode          ");      	    
                sbQuery.Append("\n     ,'' as LinkChannel		        ");    
                sbQuery.Append("\n     ,'' as LinkChannelNm		        ");
                sbQuery.Append("\n     ,'' as Mgrade		            ");        
                sbQuery.Append("\n     ,'' as HomeYn		            ");        
                sbQuery.Append("\n     ,'' as ChannelYn		            ");    
                sbQuery.Append("\n     ,'' as CugYn                     ");     
                sbQuery.Append("\n     ,'' as STBType                   "); 
                sbQuery.Append("\n FROM ADVT_MST a                      "); 
                sbQuery.Append("\n INNER JOIN CNTR      b  ON (a.cntr_seq   = b.cntr_seq) ");	           
                sbQuery.Append("\n LEFT  JOIN STM_USER c  ON (a.id_insert   = c.user_id) ");                 
                sbQuery.Append("\n LEFT  JOIN STM_COD  d  ON (a.file_typ    = d.stm_cod AND d.stm_cod_cls ='24')     ");
                sbQuery.Append("\n LEFT  JOIN STM_COD  e  ON (a.file_stt    = e.stm_cod AND e.stm_cod_cls = '31')   ");
                sbQuery.Append("\n LEFT  JOIN STM_COD  f  ON (b.cntr_stt    = f.stm_cod AND f.stm_cod_cls ='23')     "); 
                sbQuery.Append("\n LEFT  JOIN MDA      g  ON (b.mda_cod     = g.mda_cod)    ");	           
                sbQuery.Append("\n LEFT  JOIN MDA_REP  h  ON (b.rep_cod     = h.rep_cod)	");               
                sbQuery.Append("\n LEFT  JOIN AGNC     i  ON (b.agnc_cod    = i.agnc_cod)   ");	           
                /*LEFT  JOIN STM_USER j  ON (b.RegId        = j.user_id)	               */
                sbQuery.Append("\n LEFT  JOIN STM_USER k  ON (a.file_reg_id = k.user_id)	");
                /*LEFT  JOIN Program l  ON (a.LinkChannel         = l.channel)     */
                sbQuery.Append("\n WHERE a.item_no = " + contractItemModel.ItemNo.Trim()    );


                _log.Debug(sbQuery.ToString());
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 광고내역모델에 복사
                contractItemModel.ContractItemDataSet = ds.Copy();
                contractItemModel.ResultCnt = Utility.GetDatasetCount(contractItemModel.ContractItemDataSet);
                contractItemModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + contractItemModel.ResultCnt + "]");
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                contractItemModel.ResultCD = "3000";
                contractItemModel.ResultDesc = "광고내역정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


        /// <summary>
        /// 광고내역히스토리목록조회
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetContractItemHIstoryList(HeaderModel header, ContractItemModel contractItemModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemHIstoryList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
             
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                #region 삭제 할 것
                /*
                // 쿼리생성
                sbQuery.Append("\n"														
                    + "SELECT a.ItemNo                              \n"
                    + "      ,a.ItemName                            \n"
                    + "      ,a.ExcuteStartDay                      \n"
                    + "      ,a.ExcuteEndDay                        \n"
					+ "      ,a.RealEndDay                        \n"
                    + "	     ,b.CodeName AS ScheduleTypeName        \n"
                    + "	     ,c.CodeName AS AdStateName             \n"
                    + "	     ,d.CodeName AS AdClassName             \n"
                    + "      ,a.AdRate                              \n"
                    + "      ,e.CodeName AS AdTypeName              \n"
                    + "      ,a.AdTime                              \n"
                    + "      ,f.CodeName AS HistoryTypeName         \n"
                    + "	     ,convert(char(19),ISNULL(a.ModDt,a.RegDt) ,120) ModDt \n"
                    + "      ,g.UserName RegName                    \n"
                    + "  FROM ContractHistory a with(NoLock)        \n"
                    + "       LEFT JOIN SystemCode b with(NoLock) ON (a.ScheduleType = b.Code AND b.Section='27') \n"
                    + "       LEFT JOIN SystemCode c with(NoLock) ON (a.AdState      = c.Code AND c.Section='25') \n"
                    + "       LEFT JOIN SystemCode d with(NoLock) ON (a.AdClass      = d.Code AND d.Section='29') \n"
                    + "       LEFT JOIN SystemCode e with(NoLock) ON (a.AdType       = e.Code AND e.Section='26') \n"
                    + "       LEFT JOIN SystemCode f with(NoLock) ON (a.HistoryType  = f.Code AND f.Section='28') \n"
                    + "       LEFT JOIN SystemUser g with(NoLock) ON (a.RegId        = g.UserId)	                 \n"
                    + " WHERE A.Itemno ='" + contractItemModel.ItemNo.Trim().ToString() +         "' \n"
                    + " ORDER BY HistorySeq DESC	 \n"
                    );
				   */
                   #endregion

                sbQuery.Append("\n SELECT   "); 
                sbQuery.Append("\n       a.item_no as ItemNo            "); 
                sbQuery.Append("\n      ,a.item_nm as ItemName          ");                   
                sbQuery.Append("\n      ,a.begin_dy as ExcuteStartDay   ");                    
                sbQuery.Append("\n      ,a.end_dy as ExcuteEndDay       ");                  
                sbQuery.Append("\n      ,a.rl_end_dy as RealEndDay      ");                   
                sbQuery.Append("\n      ,b.stm_cod_nm as ScheduleTypeName ");        
                sbQuery.Append("\n      ,c.stm_cod_nm as AdStateName    ");          
                sbQuery.Append("\n      ,d.stm_cod_nm as AdClassName    ");          
                sbQuery.Append("\n      ,a.advt_rt as AdRate            ");                   
                sbQuery.Append("\n      ,e.stm_cod_nm as AdTypeName     ");          
                sbQuery.Append("\n      ,a.advt_tm as AdTime            ");                   
                sbQuery.Append("\n      ,f.stm_cod_nm as HistoryTypeName ");         
                sbQuery.Append("\n      ,TO_CHAR(NVL(a.file_reg_dt, SYSDATE), 'YYYY-MM-DD HH:MM:SS') as ModDt "); 
                sbQuery.Append("\n      ,g.user_nm as RegName           ");          
                sbQuery.Append("\n FROM ADVT_HST a                      ");      
                sbQuery.Append("\n LEFT JOIN STM_COD b  ON (a.sch_tp    = b.stm_cod AND b.stm_cod_cls ='27')  "); 
                sbQuery.Append("\n LEFT JOIN STM_COD c  ON (a.advt_stt  = c.stm_cod AND c.stm_cod_cls='25')  "); 
                sbQuery.Append("\n LEFT JOIN STM_COD d  ON (a.advt_clss = d.stm_cod AND d.stm_cod_cls='29')   "); 
                sbQuery.Append("\n LEFT JOIN STM_COD e  ON (a.advt_tp   = e.stm_cod AND e.stm_cod_cls='26')  ");  
                sbQuery.Append("\n LEFT JOIN STM_COD f  ON (a.hst_tp     = f.stm_cod AND f.stm_cod_cls='28')");  
                sbQuery.Append("\n LEFT JOIN STM_USER g ON (a.file_reg_id  = g.user_id)	            "); 
                sbQuery.Append("\n WHERE a.item_no =" + contractItemModel.ItemNo.Trim() );
                sbQuery.Append("\n ORDER BY a.hst_seq DESC	"); 

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 광고내역모델에 복사
                contractItemModel.ContractItemDataSet = ds.Copy();
                // 결과
                contractItemModel.ResultCnt = Utility.GetDatasetCount(contractItemModel.ContractItemDataSet);
                // 결과코드 셋트
                contractItemModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + contractItemModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemHistoryList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                contractItemModel.ResultCD = "3000";
                contractItemModel.ResultDesc = "광고내역정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


        /// <summary>
        /// 관련자료첨부조회 (사용 안함)
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetLinkChannel(HeaderModel header, ContractItemModel contractItemModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetLinkChannel() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + "SELECT A.ItemNo                              \n"
                    + "      ,A.Channel                              \n"
                    + "      ,B.ProgramNm ChannelNm                           \n"
                    + "  FROM LinkChannel A with(NoLock)             \n"
                    + "			LEFT  JOIN Program B with(NoLock) ON (A.Channel         = B.channel) \n"
                    + " WHERE A.Itemno ='" + contractItemModel.ItemNo.Trim().ToString() + "' \n"
                    + " ORDER BY A.ItemNo DESC	 \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 광고내역모델에 복사
                contractItemModel.LinkChannelDataSet = ds.Copy();
                // 결과
                contractItemModel.ResultCnt = Utility.GetDatasetCount(contractItemModel.LinkChannelDataSet);
                // 결과코드 셋트
                contractItemModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + contractItemModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetLinkChannel() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                contractItemModel.ResultCD = "3000";
                contractItemModel.ResultDesc = "연결채널 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


		/// <summary>
		/// 관련자료첨부조회(사용무)
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetLinkChannel2(HeaderModel header, ContractItemModel contractItemModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLinkChannel() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
                String itemNo = contractItemModel.ItemNo.Trim().ToString();
                int linkType = contractItemModel.LinkType;
                _log.Debug("-----------------------------------------");
                _log.Debug("ItemNo:["+itemNo+"]");
                _log.Debug("LinkType:[" + linkType + "]");
                _log.Debug("-----------------------------------------");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성

                if(linkType == 1)
				    sbQuery.Append("\n"														
					    + "SELECT A.ItemNo                              \n"
					    + "      ,A.Channel                              \n"
					    + "      ,B.ProgramNm ChannelNm                           \n"								
					    + "  FROM LinkChannel A with(NoLock)             \n"			
					    + "			LEFT  JOIN Program B with(NoLock) ON (A.Channel         = B.channel) \n"
                        + " WHERE A.Itemno ='" + itemNo + "' \n"
                        + " ORDER BY A.ItemNo DESC	 \n"
					    );
                else if(linkType == 2)
                    sbQuery.Append("\n"
                        + "SELECT A.ItemNo                              \n"
                        + "      ,A.Channel                              \n"
                        + "      ,B.ProgramNm ChannelNm                           \n"
                        + "  FROM LinkChannel1 A with(NoLock)             \n"
                        + "			LEFT  JOIN Program B with(NoLock) ON (A.Channel         = B.channel) \n"
                        + " WHERE A.Itemno ='" + itemNo + "' \n"
                        + " ORDER BY A.ItemNo DESC	 \n"
                        );
				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고내역모델에 복사
				contractItemModel.LinkChannelDataSet = ds.Copy();
				// 결과
				contractItemModel.ResultCnt = Utility.GetDatasetCount(contractItemModel.LinkChannelDataSet);
				// 결과코드 셋트
				contractItemModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + contractItemModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLinkChannel() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				contractItemModel.ResultCD = "3000";
				contractItemModel.ResultDesc = "연결채널 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}



		/// <summary>
		/// 관련자료첨부조회
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetFileList(HeaderModel header, ContractItemModel contractItemModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFileList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
              
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"														
					+ "SELECT ItemNo                              \n"
					+ "      ,FileNo                              \n"
					+ "      ,FileTitle                           \n"
					+ "      ,FileName                           \n"
					+ "      ,FilePath                            \n"
					+ "      ,convert(char(19),RegDt,120) RegDt    \n"
					+ "      ,RegID                               \n"					
					+ "  FROM AttachFile with(NoLock)             \n"					
					+ " WHERE Itemno ='" + contractItemModel.ItemNo.Trim().ToString() +         "' \n"
					+ " ORDER BY FileNo DESC	 \n"
					);
				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고내역모델에 복사
				contractItemModel.ContractItemDataSet = ds.Copy();
				// 결과
				contractItemModel.ResultCnt = Utility.GetDatasetCount(contractItemModel.ContractItemDataSet);
				// 결과코드 셋트
				contractItemModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + contractItemModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFileList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				contractItemModel.ResultCD = "3000";
				contractItemModel.ResultDesc = "자료첨부 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

        /// <summary>
        /// 듀얼광고 연결광고목록
        /// </summary>
        /// <param name="header"></param>
        /// <param name="contractItemModel"></param>
        public void GetLinkItem(HeaderModel header, ContractItemModel contractItemModel)
        {
            try
            {
                #region [쿼리]
                StringBuilder sbQuery = new StringBuilder();
                if (contractItemModel.ScheduleType.Equals("40"))
                {
                    // 엔딩듀얼에 설정된 로딩광고를 가져온다( 기본 )
                    sbQuery.Append("\n select   b.ItemNo, b.ItemName,b.ExcuteStartDay,b.RealEndDay");
                    sbQuery.Append("\n         ,ca.CodeName    as  AdStatus");
                    sbQuery.Append("\n         ,cb.CodeName    as  AdType");
                    sbQuery.Append("\n         ,cc.CodeName    as  FileStatus");
                    sbQuery.Append("\n         ,b.AdRate");
                    sbQuery.Append("\n         ,b.AdTime");
                    sbQuery.Append("\n from    LinkItem a with(noLock)");
                    sbQuery.Append("\n inner join ContractItem b with(noLock) on b.ItemNo = a.LinkItemNo");
                    sbQuery.Append("\n inner join SystemCode   ca with(noLock) on ca.Code = b.AdState   and ca.Section = '25'");
                    sbQuery.Append("\n inner join SystemCode   cb with(noLock) on cb.Code = b.AdType    and cb.Section = '26'");
                    sbQuery.Append("\n inner join SystemCode   cc with(noLock) on cc.Code = b.FileState and cc.Section = '31'");
                    sbQuery.Append("\n where   a.LinkType = 1");
                    sbQuery.Append("\n and     a.ItemNo = '" + contractItemModel.ItemNo.Trim().ToString() + "'");
                    sbQuery.Append("\n order by ItemNo desc");
                }
                else
                {
                    // 로딩광고에 설정된 엔딩듀얼을 가져온다.( 참조 )
                    sbQuery.Append("\n select   b.ItemNo, b.ItemName,b.ExcuteStartDay,b.RealEndDay");
                    sbQuery.Append("\n         ,ca.CodeName    as  AdStatus");
                    sbQuery.Append("\n         ,cb.CodeName    as  AdType");
                    sbQuery.Append("\n         ,cc.CodeName    as  FileStatus");
                    sbQuery.Append("\n         ,b.AdRate");
                    sbQuery.Append("\n         ,b.AdTime");
                    sbQuery.Append("\n from    LinkItem a with(noLock)");
                    sbQuery.Append("\n inner join ContractItem b with(noLock) on b.ItemNo = a.ItemNo");
                    sbQuery.Append("\n inner join SystemCode   ca with(noLock) on ca.Code = b.AdState   and ca.Section = '25'");
                    sbQuery.Append("\n inner join SystemCode   cb with(noLock) on cb.Code = b.AdType    and cb.Section = '26'");
                    sbQuery.Append("\n inner join SystemCode   cc with(noLock) on cc.Code = b.FileState and cc.Section = '31'");
                    sbQuery.Append("\n where   a.LinkType = 1");
                    sbQuery.Append("\n and     a.LinkItemNo = '" + contractItemModel.ItemNo.Trim().ToString() + "'");
                    sbQuery.Append("\n order by ItemNo desc");
                }
                #endregion

                // __DEBUG__
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetLinkItem() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("<입력정보>");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                _db.Open();
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 광고내역모델에 복사
                contractItemModel.ContractItemDataSet = ds.Copy();
                contractItemModel.ResultCnt = Utility.GetDatasetCount(contractItemModel.ContractItemDataSet);
                contractItemModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + contractItemModel.ResultCnt + "]");
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetLinkItem() End");
                _log.Debug("-----------------------------------------");
                // __DEBUG__
            }
            catch (Exception ex)
            {
                contractItemModel.ResultCD = "3000";
                contractItemModel.ResultDesc = "광고내역정보 조회중 듀얼광고조회 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }


		/// <summary>
		/// 채널검색
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetChannelList(HeaderModel header,  ContractItemModel contractItemModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList() Start");
				_log.Debug("-----------------------------------------");
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey           :[" + contractItemModel.SearchKey       + "]");
				_log.Debug("SearchMediaCode     :[" + contractItemModel.SearchMediaCode      + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT A.GenreCode                                     \n"
					+ "      ,C.CategoryName + '>' + D.GenreName AS GenreName \n"
					+ "      ,A.ChannelNo                                     \n"
					+ "      ,B.Title AS ChannelName                          \n"
					+ "      ,MAX(B.TotalSeries) AS SeriCnt                   \n"
					+ "  FROM ChannelSet A          with(NoLock)              \n"
					+ "       INNER JOIN Channel  B with(NoLock) ON (A.ChannelNo    = B.ChannelNo     AND A.SeriesNo  = B.SeriesNo AND A.MediaCode = B.MediaCode) \n"
					+ "       INNER JOIN Category C with(NoLock) ON (A.CategoryCode = C. CategoryCode AND A.MediaCode = C.MediaCode) \n"
					+ "       INNER JOIN Genre    D with(NoLock) ON (A.GenreCode    = D. GenreCode    AND A.MediaCode = D.MediaCode) \n"
					+ " WHERE A.MediaCode = " + contractItemModel.SearchMediaCode + " \n"
					);

				// 검색어가 있으면
				if (contractItemModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("  AND ( B.Title  LIKE '%" + contractItemModel.SearchKey.Trim() + "%') \n");
				}

				sbQuery.Append(""
					+ " GROUP BY C.CategoryName, A.GenreCode, D.GenreName, A.ChannelNo, B.Title \n"
					+ " ORDER BY A.GenreCode, A.ChannelNo \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				contractItemModel.ContractItemDataSet = ds.Copy();
				// 결과
				contractItemModel.ResultCnt = Utility.GetDatasetCount(contractItemModel.ContractItemDataSet);
				// 결과코드 셋트
				contractItemModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + contractItemModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				contractItemModel.ResultCD = "3000";
				contractItemModel.ResultDesc = "채널검색중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}


        /// <summary>
        /// 채널검색
        /// 2015/05/12 Youngil.Yi 작성
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetContentsList(HeaderModel header, ContractItemModel contractItemModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelList() Start");
                _log.Debug("-----------------------------------------");
                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey           :[" + contractItemModel.SearchKey + "]");
                _log.Debug("SearchMediaCode     :[" + contractItemModel.SearchMediaCode + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + "SELECT A.GenreCode                                     \n"
                    + "      ,C.CategoryName + '>' + D.GenreName AS GenreName \n"
                    + "      ,A.ChannelNo                                     \n"
                    + "      ,B.Title AS ChannelName                          \n"
                    + "      ,MAX(B.TotalSeries) AS SeriCnt                   \n"
                    + "  FROM ChannelSet A          with(NoLock)              \n"
                    + "       INNER JOIN Channel  B with(NoLock) ON (A.ChannelNo    = B.ChannelNo     AND A.SeriesNo  = B.SeriesNo AND A.MediaCode = B.MediaCode) \n"
                    + "       INNER JOIN Category C with(NoLock) ON (A.CategoryCode = C. CategoryCode AND A.MediaCode = C.MediaCode) \n"
                    + "       INNER JOIN Genre    D with(NoLock) ON (A.GenreCode    = D. GenreCode    AND A.MediaCode = D.MediaCode) \n"
                    + " WHERE A.MediaCode = " + contractItemModel.SearchMediaCode + " \n"
                    );

                // 검색어가 있으면
                if (contractItemModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("  AND ( B.Title  LIKE '%" + contractItemModel.SearchKey.Trim() + "%') \n");
                }

                sbQuery.Append(""
                    + " GROUP BY C.CategoryName, A.GenreCode, D.GenreName, A.ChannelNo, B.Title \n"
                    + " ORDER BY A.GenreCode, A.ChannelNo \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 모델에 복사
                contractItemModel.ContractItemDataSet = ds.Copy();
                // 결과
                contractItemModel.ResultCnt = Utility.GetDatasetCount(contractItemModel.ContractItemDataSet);
                // 결과코드 셋트
                contractItemModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + contractItemModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                contractItemModel.ResultCD = "3000";
                contractItemModel.ResultDesc = "채널검색중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

		/// <summary>
		/// 전체파일의 건수/사이즈, 홈광고대상 파일건수/사이즈, 셋탑별 건수/사이즈등.
		/// </summary>
		/// <param name="header"></param>
		/// <param name="contractItemModel"></param>
		public void GetFileInfo(HeaderModel header, ContractItemModel contractItemModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFileInfo() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("<입력정보>");

				// __DEBUG__
				StringBuilder sbQuery = new StringBuilder();
				sbQuery.Append("\n select  count(*)                                                                                    as  totalCount");
				sbQuery.Append("\n     ,   sum(case when FileName is null  then 1       else 0 end)                                    as  totalCountNoFile");
				sbQuery.Append("\n     ,   sum(FileLen)                                                                                as  totalFile");
				sbQuery.Append("\n     ,   sum(case when HomeYn = 'Y'  then 1       else 0 end)                                        as  homeCount");
				sbQuery.Append("\n     ,   sum(case when HomeYn = 'Y'  then FileLen else 0 end)                                        as  homeFile");
				sbQuery.Append("\n     ,   sum(case when HomeYn = 'Y'  then case STBType when 0 then 1         else 0 end else 0 end)  as  homeCount0");
				sbQuery.Append("\n     ,   sum(case when HomeYn = 'Y'  then case STBType when 0 then FileLen   else 0 end else 0 end)  as  homeFile0");
				sbQuery.Append("\n     ,   sum(case when HomeYn = 'Y'  then case STBType when 1 then 1         else 0 end else 0 end)  as  homeCount1");
				sbQuery.Append("\n     ,   sum(case when HomeYn = 'Y'  then case STBType when 1 then FileLen   else 0 end else 0 end)  as  homeFile1");
				sbQuery.Append("\n     ,   sum(case when HomeYn = 'Y'  then case STBType when 2 then 1         else 0 end else 0 end)  as  homeCount2");
				sbQuery.Append("\n     ,   sum(case when HomeYn = 'Y'  then case STBType when 2 then FileLen   else 0 end else 0 end)  as  homeFile2");
				sbQuery.Append("\n from (  select   AdTime,FileName");
				sbQuery.Append("\n                 ,cast(isnull(FileLength,case when adTime > 5 then AdTime*300000 else 0 end ) as float) as FileLen");
				sbQuery.Append("\n                 ,isnull(STBType,0) as STBType");
				sbQuery.Append("\n                 ,HomeYn");
				sbQuery.Append("\n         from    ContractItem a with(nolock)");
				sbQuery.Append("\n         where   ItemNo in(  select  ItemNo");
				sbQuery.Append("\n                             from    ContractItem a with(nolock)");
				sbQuery.Append("\n                             where   FileState < '90')");
				//sbQuery.Append("\n         where   ItemNo in(  select  max(ItemNo)");
				//sbQuery.Append("\n                             from    ContractItem a with(nolock)");
				//sbQuery.Append("\n                             where   FileState < '90'");
				//sbQuery.Append("\n                             group by isnull(FileName, 't'+cast(itemNo as varchar)) )");
				sbQuery.Append("\n      ) as v");
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// 결과 DataSet의 광고내역모델에 복사
				contractItemModel.ContractItemDataSet = ds.Copy();
				// 결과
				contractItemModel.ResultCnt = Utility.GetDatasetCount(contractItemModel.ContractItemDataSet);
				// 결과코드 셋트
				contractItemModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + contractItemModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFileInfo() End");
				_log.Debug("-----------------------------------------");
			}
			catch (Exception ex)
			{
				contractItemModel.ResultCD = "3000";
				contractItemModel.ResultDesc = "파일현황 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}

		}

		/// <summary>
		/// 자료정보s 생성
		/// </summary>
		/// <param name="header"></param>
		/// <param name="contractItemModel"></param>
		public void SetFileCreate(HeaderModel header, ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFileCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();
                
				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[6];

				sbQuery.Append( ""
					+ "INSERT INTO AttachFile (\n"
					+ "       ItemNo   \n"
					+ "      ,FileNo   \n"
					+ "      ,FileTitle   \n"
					+ "      ,FileName   \n" 
					+ "      ,FilePath     \n"					
					+ "      ,RegDt       \n"										
					+ "      ,RegID       \n"
					+ "      )            \n"
					+ " SELECT            \n"
					+ "       @ItemNo \n"
					+ "      ,ISNULL(MAX(FileNo),0) + 1 \n"
					+ "      ,@FileTitle  \n"
					+ "      ,@FileName  \n"
					+ "      ,@FilePath  \n" 					
					+ "      ,GETDATE()   \n"	
					+ "      ,@RegID      \n"
					+ " FROM AttachFile		  \n"					
					+ " WHERE ItemNo = @ItemNo		  \n"					
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				sqlParams[i++] = new SqlParameter("@ItemNo"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@FileTitle"  , SqlDbType.VarChar , 200);
				sqlParams[i++] = new SqlParameter("@FileName"  , SqlDbType.VarChar , 500);
				sqlParams[i++] = new SqlParameter("@FilePath"    , SqlDbType.VarChar , 2000);				
				sqlParams[i++] = new SqlParameter("@RegID"      , SqlDbType.VarChar , 10);
				
				_log.Debug("contractItemModel.ItemNo		:[" + contractItemModel.ItemNo		 + "]");
				_log.Debug("contractItemModel.FileTitle		:[" + contractItemModel.FileTitle		 + "]");
				_log.Debug("contractItemModel.FileName		:[" + contractItemModel.FileName		 + "]");
				_log.Debug("contractItemModel.FilePath	:[" + contractItemModel.FilePath	 + "]");				
				_log.Debug("contractItemModel.ItemNo	:[" + contractItemModel.ItemNo	 + "]");
				
				i = 0;
				sqlParams[i++].Value = contractItemModel.ItemNo;				
				sqlParams[i++].Value = contractItemModel.FileTitle;
				sqlParams[i++].Value = contractItemModel.FileName;
				sqlParams[i++].Value = contractItemModel.FilePath;				
				sqlParams[i++].Value = header.UserID;      // 등록자
				
				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("대행사정보생성:[" + contractItemModel.ItemNo + "(" + contractItemModel.ItemNo + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				contractItemModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFileCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				contractItemModel.ResultCD   = "3101";
				contractItemModel.ResultDesc = "자료정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 자료정보 저장
		/// </summary>
		/// <param name="header"></param>
		/// <param name="contractItemModel"></param>
		public void SetFileUpdate(HeaderModel header, ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFileUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();
								
				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[6];

				sbQuery.Append(""
					+ "UPDATE AttachFile                     \n"
					+ "   SET FileTitle  = @FileTitle  \n"
					+ "      ,FileName     = @FileName  \n" 
					+ "      ,FilePath  = @FilePath  \n" 					
					+ "      ,ModDt      = GETDATE()     \n"
					+ "      ,RegID      = @RegID        \n"
					+ " WHERE ItemNo  = @ItemNo  \n"
					+ "	  AND FileNo  = @FileNo  \n"
					);
				
				sqlParams[i++] = new SqlParameter("@FileTitle"  , SqlDbType.VarChar , 200);
				sqlParams[i++] = new SqlParameter("@FileName"  , SqlDbType.VarChar , 500);
				sqlParams[i++] = new SqlParameter("@FilePath"    , SqlDbType.VarChar , 2000);				
				sqlParams[i++] = new SqlParameter("@RegID"      , SqlDbType.VarChar , 10);
				sqlParams[i++] = new SqlParameter("@ItemNo"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@FileNo"  , SqlDbType.Int);

				i = 0;				
				sqlParams[i++].Value = contractItemModel.FileTitle;
				sqlParams[i++].Value = contractItemModel.FileName;
				sqlParams[i++].Value = contractItemModel.FilePath;				
				sqlParams[i++].Value = header.UserID;      // 등록자
				sqlParams[i++].Value = contractItemModel.ItemNo;				
				sqlParams[i++].Value = contractItemModel.FileNo;				

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("대행사정보수정:["+contractItemModel.ItemNo + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				contractItemModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFileUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				contractItemModel.ResultCD   = "3201";
				contractItemModel.ResultDesc = "자료정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		//자료정보 삭제
		public void SetFileDelete(HeaderModel header, ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFileDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];

				sbQuery.Append(""
					+ "DELETE AttachFile         \n"
					+ " WHERE ItemNo  = @ItemNo  \n"
					+ "   AND FileNo  = @FileNo  \n"					
					);

				sqlParams[i++] = new SqlParameter("@ItemNo"       , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@FileNo"       , SqlDbType.Int);

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(contractItemModel.ItemNo);
				sqlParams[i++].Value = Convert.ToInt32(contractItemModel.FileNo);		

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("카테고리정보삭제:[" + contractItemModel.ItemNo + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				contractItemModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetFileDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				contractItemModel.ResultCD   = "3301";
				contractItemModel.ResultDesc = "자료정보 삭제중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		/// <summary>
		/// 광고내역 전체 복사
		/// </summary>
		/// <param name="header"></param>
		/// <param name="data"></param>
		public void SetContractItemCopy(HeaderModel	header, ItemCopyModel	data)
		{
			StringBuilder sbQuery = new StringBuilder();
			DataSet ds = new DataSet();

			try
			{
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetContractItemCopy() Start");
				_log.Debug("-----------------------------------------");

				#region [ 1. 동일 광고명 확인 ]
				sbQuery.Append("\n select	count(*)	from ContractItem with(noLock) ");
				sbQuery.Append("\n where	ItemName = '" + data.ItemName.Trim() + "';" );	 
				_db.ExecuteQuery(ds,sbQuery.ToString());

				int	NameCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
				if( NameCnt > 0 )	throw new Exception("동일한 광고내역명이 존재 합니다...");

				ds.Dispose();
				ds = null;
				#endregion

				sbQuery = new StringBuilder();
				sbQuery.Append("\n  ");
				sbQuery.Append("\n DECLARE @RC			int;");
				sbQuery.Append("\n DECLARE @itemNo		int;");
				sbQuery.Append("\n DECLARE @itemName	varchar(50);");
				sbQuery.Append("\n DECLARE @itemNoNew	int");

				sbQuery.Append("\n set @itemNo		= " + data.ItemNoSou + ";");
				sbQuery.Append("\n set @itemName	= '" + data.ItemName.Trim() + "';");
				sbQuery.Append("\n set @itemNoNew	= 0;");

				sbQuery.Append("\n EXECUTE @RC = [dbo].[ContractItemCopy] ");
   				sbQuery.Append("\n				 @itemNo, @itemName");
  				sbQuery.Append("\n				,@itemNoNew output");
				sbQuery.Append("\n");
				sbQuery.Append("\n Select @itemNoNew as newItem;");

				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					data.ResultCD	= "0000";
					data.ResultCnt	= 1;
					data.ResultDesc = "";
					data.ItemNoDes	= Convert.ToInt32( ds.Tables[0].Rows[0]["newItem"].ToString() );
				}
				else
				{
					data.ResultCD	= "3000";
					data.ResultCnt	= 0;
					data.ResultDesc = "광고내역 복사 오류, 결과 값이 없습니다.";
					data.ItemNoDes	= 0;
				}
			}
			catch(Exception ex)
			{
				data.ItemNoDes	= 0;
				data.ResultCD   = "3101";
				data.ResultDesc = ex.Message;
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

		/// <summary>
		/// 광고내역 생성
		/// </summary>
		/// <param name="header"></param>
		/// <param name="contractItemModel"></param>
        public void SetContractItemCreate(HeaderModel header, ContractItemModel contractItemModel)
        {
			int NameCnt = 0;

            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractItemCreate() Start");
                _log.Debug("-----------------------------------------");
       
				#region [동일 광고명 확인]
				// 동일한 광고내역명이 있는지 검사한다.
				StringBuilder sbQueryItemName = new StringBuilder();
				sbQueryItemName.Append("\n"
					+ " SELECT COUNT(*) FROM ADVT_MST  \n"
					+ "  WHERE item_nm = '" + contractItemModel.ItemName.Trim() + "' \n"
					);

				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQueryItemName.ToString());

				NameCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
				if(NameCnt > 0)	throw new Exception();
				#endregion

				#region [광고내역번호 채번]
                StringBuilder sbQueryItemNo = new StringBuilder();
                sbQueryItemNo.Append("\n SELECT NVL(MAX(item_no),0)+1 ItemNo FROM ADVT_MST ");
			
                ds = new DataSet();
                _db.ExecuteQuery(ds,sbQueryItemNo.ToString());
                contractItemModel.ItemNo = ds.Tables[0].Rows[0][0].ToString();
				ds.Dispose();
				#endregion

                //AdGroup Insert Query
                StringBuilder sbQuery = new StringBuilder();
                #region 삭제 할것
                /*                                                 
                sbQuery.Append("\n");
                sbQuery.Append("INSERT INTO ContractItem				\n");
                sbQuery.Append("           (ItemNo						\n");
                sbQuery.Append("           ,MediaCode					\n");
                sbQuery.Append("           ,RapCode						\n");
                sbQuery.Append("           ,AgencyCode					\n");
                sbQuery.Append("           ,AdvertiserCode				\n");
                sbQuery.Append("           ,ContractSeq					\n");
                sbQuery.Append("           ,ItemName					\n");
                sbQuery.Append("           ,ExcuteStartDay				\n");
                sbQuery.Append("           ,ExcuteEndDay				\n");
                sbQuery.Append("           ,RealEndDay					\n");
                sbQuery.Append("           ,AdTime						\n");
                sbQuery.Append("           ,AdState						\n");
                sbQuery.Append("           ,AdClass						\n");
                sbQuery.Append("           ,AdType						\n");
                sbQuery.Append("           ,ScheduleType				\n");
                sbQuery.Append("           ,AdRate						\n");
                sbQuery.Append("           ,FileState		  		    \n");
                sbQuery.Append("           ,RegDt						\n");
                sbQuery.Append("           ,ModDt						\n");
                sbQuery.Append("           ,RegID						\n");
				sbQuery.Append("		   ,LinkChannel				    \n");
				sbQuery.Append("		   ,Mgrade				        \n");
				sbQuery.Append("		   ,HomeYn				        \n");
				sbQuery.Append("		   ,ChannelYn				    \n");
				sbQuery.Append("           ,CugYn		                \n");
				sbQuery.Append("           ,STBType  )                  \n");
                sbQuery.Append("     VALUES(							\n");
                sbQuery.Append("            @ItemNo                 	\n");
                sbQuery.Append("           ,@MediaCode					\n");
                sbQuery.Append("           ,@RapCode					\n");
                sbQuery.Append("           ,@AgencyCode					\n");
                sbQuery.Append("           ,@AdvertiserCode				\n");
                sbQuery.Append("           ,@ContractSeq				\n");
                sbQuery.Append("           ,@ItemName					\n");
                sbQuery.Append("           ,@ExcuteStartDay				\n");
                sbQuery.Append("           ,@ExcuteEndDay				\n");
                sbQuery.Append("           ,@RealEndDay					\n");
                sbQuery.Append("           ,@AdTime						\n");
                sbQuery.Append("           ,@AdState					\n");
                sbQuery.Append("           ,@AdClass					\n");
                sbQuery.Append("           ,@AdType						\n");
                sbQuery.Append("           ,@ScheduleType				\n");
                sbQuery.Append("           ,@AdRate						\n");
                sbQuery.Append("           ,'10'   						\n");  // FileState 10:준비
                sbQuery.Append("           ,GETDATE()					\n");
                sbQuery.Append("           ,GETDATE()					\n");
                sbQuery.Append("           ,@RegID						\n");		
				sbQuery.Append("           ,@LinkChannel				\n");		
				sbQuery.Append("           ,@Mgrade				        \n");
				sbQuery.Append("           ,@HomeYn				        \n");
				sbQuery.Append("           ,@ChannelYn	 			    \n");
				sbQuery.Append("           ,@CugYn			            \n");
				sbQuery.Append("           ,@STBType       )            \n");
                */
                #endregion

                sbQuery.Append("\n INSERT INTO ADVT_MST	 (  ");
                sbQuery.Append("\n         item_no          "); 
                sbQuery.Append("\n         ,cntr_seq        ");
                sbQuery.Append("\n         ,item_nm         ");
                sbQuery.Append("\n         ,begin_dy        ");
                sbQuery.Append("\n         ,end_dy          ");
                sbQuery.Append("\n         ,rl_end_dy       ");
                sbQuery.Append("\n         ,advt_tm         ");
                sbQuery.Append("\n         ,advt_stt        ");    
                sbQuery.Append("\n         ,advt_clss       ");    
                sbQuery.Append("\n         ,advt_typ        ");    
                sbQuery.Append("\n         ,sch_typ         ");
                sbQuery.Append("\n         ,advt_rate       ");
                sbQuery.Append("\n         ,file_stt        ");
                sbQuery.Append("\n         ,id_insert       ");
                sbQuery.Append("\n         ,dt_insert       ");
                sbQuery.Append("\n         ,dt_update       ");
                sbQuery.Append("\n         )                ");  
                sbQuery.Append("\n VALUES(					");		
                sbQuery.Append("\n         :ItemNo          ");       	          
                sbQuery.Append("\n         ,:ContractSeq	");			
                sbQuery.Append("\n         ,:ItemName		");			
                sbQuery.Append("\n         ,:ExcuteStartDay	");			
                sbQuery.Append("\n         ,:ExcuteEndDay	");			
                sbQuery.Append("\n         ,:RealEndDay		");			
                sbQuery.Append("\n         ,:AdTime			");			
                sbQuery.Append("\n         ,:AdState		");			
                sbQuery.Append("\n         ,:AdClass		");			
                sbQuery.Append("\n         ,:AdType			");			
                sbQuery.Append("\n         ,:ScheduleType	");			
                sbQuery.Append("\n         ,:AdRate			");			
                sbQuery.Append("\n         ,'10'   			");			  /* FileState 10:준비*/
                sbQuery.Append("\n         ,:RegID			");					
                sbQuery.Append("\n         ,SYSDATE         ");
                sbQuery.Append("\n         ,SYSDATE         ");
                sbQuery.Append("\n )                        ");
                // 쿼리실행
                try
                {
                                                
                    int i = 0;
                    int rc = 0;
                    //광고 그룹 Insert
                    OracleParameter[] sqlParams = new OracleParameter[13];
                    _db.BeginTran();
                                
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":ItemNo"		       , OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":ContractSeq"        , OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":ItemName"		   , OracleDbType.Varchar2,50);
                    sqlParams[i++] = new OracleParameter(":ExcuteStartDay"	   , OracleDbType.Char , 8);
                    sqlParams[i++] = new OracleParameter(":ExcuteEndDay"	   , OracleDbType.Char , 8);
                    sqlParams[i++] = new OracleParameter(":RealEndDay"		   , OracleDbType.Char , 8);
                    sqlParams[i++] = new OracleParameter(":AdTime"             , OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":AdState"		       , OracleDbType.Char , 2);
                    sqlParams[i++] = new OracleParameter(":AdClass"		       , OracleDbType.Char , 2);
                    sqlParams[i++] = new OracleParameter(":AdType"			   , OracleDbType.Char , 2);
                    sqlParams[i++] = new OracleParameter(":ScheduleType"	   , OracleDbType.Char , 2);
                    sqlParams[i++] = new OracleParameter(":AdRate"             , OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":RegID"			   , OracleDbType.Varchar2 , 10);
					

                    _log.Debug("contractItemModel.ItemNo		:[" + contractItemModel.ItemNo		    + "]");
                    _log.Debug("contractItemModel.ContractSeq	:[" + contractItemModel.ContractSeq	    + "]");
                    _log.Debug("contractItemModel.ItemName		:[" + contractItemModel.ItemName.Trim() + "]");
                    _log.Debug("contractItemModel.ExcuteStartDay:[" + contractItemModel.ExcuteStartDay  + "]");
                    _log.Debug("contractItemModel.ExcuteEndDay	:[" + contractItemModel.ExcuteEndDay	+ "]");
                    _log.Debug("contractItemModel.RealEndDay	:[" + contractItemModel.RealEndDay	 + "]");
                    _log.Debug("contractItemModel.AdTime		:[" + contractItemModel.AdTime		 + "]");
                    _log.Debug("contractItemModel.AdState		:[" + contractItemModel.AdState		 + "]");
                    _log.Debug("contractItemModel.AdClass		:[" + contractItemModel.AdClass		 + "]");
                    _log.Debug("contractItemModel.AdType		:[" + contractItemModel.AdType		 + "]");
                    _log.Debug("contractItemModel.ScheduleType	:[" + contractItemModel.ScheduleType + "]");
                    _log.Debug("contractItemModel.AdRate		:[" + contractItemModel.AdRate		 + "]");
					
                    i = 0;
                    sqlParams[i++].Value = contractItemModel.ItemNo;		
                    sqlParams[i++].Value = contractItemModel.ContractSeq;	
                    sqlParams[i++].Value = contractItemModel.ItemName;		
                    sqlParams[i++].Value = contractItemModel.ExcuteStartDay;	
                    sqlParams[i++].Value = contractItemModel.ExcuteEndDay;	

                    if(contractItemModel.RealEndDay.Trim().Length > 0)
                    {
                        sqlParams[i++].Value = contractItemModel.RealEndDay;			
                    }
                    else
                    {
                        _log.Debug("AdTime:["+contractItemModel.ExcuteEndDay+"]");

                        sqlParams[i++].Value = contractItemModel.ExcuteEndDay;			
                    }

                    if(contractItemModel.AdTime.Trim().Length > 0)
                    {
                        sqlParams[i++].Value = Convert.ToInt32(contractItemModel.AdTime);			
                    }
                    else
                    {
                        _log.Debug("AdTime:["+contractItemModel.AdTime+"]");

                        sqlParams[i++].Value = null;			
                    }
                    sqlParams[i++].Value = contractItemModel.AdState;		
                    sqlParams[i++].Value = contractItemModel.AdClass;		
                    sqlParams[i++].Value = contractItemModel.AdType;			
                    sqlParams[i++].Value = contractItemModel.ScheduleType;	
                    sqlParams[i++].Value = contractItemModel.AdRate;			
                    sqlParams[i++].Value = header.UserID;
					
                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__
                                
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                   
                    //변경내역을 신규:10으로 셋팅
                    contractItemModel.HistoryType = "10";

                    ContractITemHistoryCreate(contractItemModel);
                    // __MESSAGE__
                    _log.Message("광고내역정보생성:["+ contractItemModel.ItemNo + "]["+ contractItemModel.ItemName + "] 등록자:[" + header.UserID + "]");
            
                    _db.CommitTran();
            
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                                                    
                contractItemModel.ResultCD = "0000";  // 정상
                                                    
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractItemCreate() End");
                _log.Debug("-----------------------------------------");	
                                        
            }
            catch(Exception ex)
            {
                contractItemModel.ResultCD   = "3101";

				if(NameCnt > 0)
				{
					contractItemModel.ResultDesc = "동일한 광고내역명이 이미 존재합니다.";
				}
				else
				{
					contractItemModel.ResultDesc = "광고내역정보 생성 중 오류가 발생하였습니다";
					_log.Exception(ex);
				}
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 광고내역 변경히스토리 생성
        /// </summary>
        /// <returns></returns>
        public void ContractITemHistoryCreate(ContractItemModel contractItemModel)
        {
            _log.Debug("-----------------------------------------");
            _log.Debug(this.ToString() + "SetContractITemHistoryInsert() Start");
            _log.Debug("-----------------------------------------");

            //AdGroup Insert Query
            StringBuilder sbQuery = new StringBuilder();
            #region 삭제 할 것
            /*                                                     
            sbQuery.Append("\n");
            sbQuery.Append("insert  into ContractHistory				\n");	
            sbQuery.Append("SELECT ItemNo								\n");
            sbQuery.Append("      ,(SELECT ISNULL(MAX(HistorySeq),0)+1 	\n");
            sbQuery.Append("       FROM ContractHistory 				\n");
			sbQuery.Append("       WHERE  ItemNo = @ItemNo )            \n");
			sbQuery.Append("	  ,@HistoryType	    					\n");
            sbQuery.Append("      ,MediaCode							\n");
            sbQuery.Append("      ,RapCode								\n");
            sbQuery.Append("      ,AgencyCode							\n");
            sbQuery.Append("      ,AdvertiserCode						\n");
            sbQuery.Append("      ,ContractSeq							\n");
            sbQuery.Append("      ,ItemName								\n");
            sbQuery.Append("      ,ExcuteStartDay						\n");
            sbQuery.Append("      ,ExcuteEndDay							\n");
            sbQuery.Append("      ,RealEndDay							\n");
            sbQuery.Append("      ,AdTime								\n");
            sbQuery.Append("      ,AdState								\n");
            sbQuery.Append("      ,AdClass								\n");
            sbQuery.Append("      ,AdType								\n");
            sbQuery.Append("      ,ScheduleType							\n");
            sbQuery.Append("      ,AdRate								\n");
            sbQuery.Append("      ,RegDt								\n");
            sbQuery.Append("      ,ModDt								\n");
            sbQuery.Append("      ,RegID								\n");
            sbQuery.Append("      ,FileState							\n");
            sbQuery.Append("      ,FileType								\n");
            sbQuery.Append("      ,FileLength							\n");
            sbQuery.Append("      ,FilePath								\n");
            sbQuery.Append("      ,FileName								\n");
            sbQuery.Append("      ,DownLevel							\n");
            sbQuery.Append("      ,FileRegDt							\n");
            sbQuery.Append("      ,FileRegID							\n");			
            sbQuery.Append("  FROM ContractItem							\n");
            sbQuery.Append("WHERE  ItemNo = @ItemNo     				\n");		
            */
            #endregion

            sbQuery.Append("\n INSERT  INTO ADVT_HST (  ");
            sbQuery.Append("\n  item_no         ");
            sbQuery.Append("\n ,hst_seq         ");
            sbQuery.Append("\n ,hst_tp          ");
            sbQuery.Append("\n ,rep_cod         ");
            sbQuery.Append("\n ,agnc_cod        ");
            sbQuery.Append("\n ,advter_cod      ");
            sbQuery.Append("\n ,cntr_seq        ");    
            sbQuery.Append("\n ,item_nm         ");
            sbQuery.Append("\n ,begin_dy        ");
            sbQuery.Append("\n ,end_dy          ");    
            sbQuery.Append("\n ,rl_end_dy       ");
            sbQuery.Append("\n ,advt_tm         ");
            sbQuery.Append("\n ,advt_stt        ");
            sbQuery.Append("\n ,advt_clss       ");
            sbQuery.Append("\n ,advt_tp         ");
            sbQuery.Append("\n ,sch_tp          ");
            sbQuery.Append("\n ,advt_rt         ");
            sbQuery.Append("\n ,file_stt        ");
            sbQuery.Append("\n ,file_typ        ");
            sbQuery.Append("\n ,file_len        ");
            sbQuery.Append("\n ,file_path       ");
            sbQuery.Append("\n ,file_nm         ");
            sbQuery.Append("\n ,downld_lvl      ");
            sbQuery.Append("\n ,file_reg_dt     ");
            sbQuery.Append("\n ,file_reg_id     ");
            sbQuery.Append("\n )                ");
            sbQuery.Append("\n SELECT           ");
            sbQuery.Append("\n     a.item_no    ");
            sbQuery.Append("\n     ,(SELECT NVL(MAX(hst_seq),0)+1 FROM ADVT_HST WHERE  item_no = :ItemNo ) ");
            sbQuery.Append("\n     ,:HistoryType	 ");
            //sbQuery.Append("\n     /*현재 컬럼에 없음 ,b.mda_cod*/ ");
            sbQuery.Append("\n     ,b.rep_cod   ");
            sbQuery.Append("\n     ,b.agnc_cod  ");
            sbQuery.Append("\n     ,b.advter_cod    ");
            sbQuery.Append("\n     ,a.cntr_seq  ");
            sbQuery.Append("\n     ,a.item_nm	");							
            sbQuery.Append("\n     ,a.begin_dy	");					
            sbQuery.Append("\n     ,a.end_dy	");						
            sbQuery.Append("\n     ,a.rl_end_dy	");						
            sbQuery.Append("\n     ,a.advt_tm   ");
            sbQuery.Append("\n     ,a.advt_stt  ");
            sbQuery.Append("\n     ,a.advt_clss ");    
            sbQuery.Append("\n     ,a.advt_typ  ");
            sbQuery.Append("\n     ,a.sch_typ   ");
            sbQuery.Append("\n     ,a.advt_rate ");
            sbQuery.Append("\n     ,a.file_stt  ");
            sbQuery.Append("\n     ,a.file_typ  ");
            sbQuery.Append("\n     ,a.file_len  ");
            sbQuery.Append("\n     ,a.file_path ");
            sbQuery.Append("\n     ,a.file_nm   ");
            sbQuery.Append("\n     ,0           ");  /*DownLevel	*/
            sbQuery.Append("\n     ,a.file_reg_dt   ");   
            sbQuery.Append("\n     ,a.file_reg_id   ");
            sbQuery.Append("\n FROM ADVT_MST a      ");
            sbQuery.Append("\n INNER JOIN CNTR b ON (a.cntr_seq = b.cntr_seq)   ");
            sbQuery.Append("\n WHERE  a.item_no = :ItemNo ");

            OracleParameter[] sqlParams = new OracleParameter[2];
                                
            int i = 0;
            sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
            sqlParams[i++] = new OracleParameter(":HistoryType", OracleDbType.Char,2);
                                                                                 

            i = 0;
            sqlParams[i++].Value = Convert.ToInt32(contractItemModel.ItemNo);
            sqlParams[i++].Value = contractItemModel.HistoryType;
            
                                    
            // __DEBUG__
            _log.Debug(sbQuery.ToString());
            // __DEBUG__
                                
            int rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
        }

        // 광고내역정보 수정
        public void SetContractItemUpdate(HeaderModel header, ContractItemModel contractItemModel)
        {
			int NameCnt = 0;

            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractItemUpdate() Start");
                _log.Debug("-----------------------------------------");
                        

				// 동일한 광고내역명이 다른광고내역에 있는지 검사한다.
				StringBuilder sbQueryItemName = new StringBuilder();
				sbQueryItemName.Append("\n"
					+ " SELECT COUNT(*) FROM ADVT_MST   \n"
					+ "  WHERE item_nm = '" + contractItemModel.ItemName.Trim() + "' \n"
					+ "    AND item_no   <>  " + contractItemModel.ItemNo          + "  \n"
					);

				// __DEBUG__
				_log.Debug(sbQueryItemName.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQueryItemName.ToString());


				NameCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

				if(NameCnt > 0)
				{
					throw new Exception();
				}


				// 광고내역을 업데이트한다.
				StringBuilder sbQuery = new StringBuilder();
                        
                int i = 0;
                int rc = 0;

                #region 삭제 할 것
                /* 삭제할것
                sbQuery.Append("\n");
                sbQuery.Append("UPDATE ContractItem						\n");
                sbQuery.Append("   SET ItemName		=	@ItemName		\n");
                sbQuery.Append("      ,ExcuteStartDay = @ExcuteStartDay \n");
                sbQuery.Append("      ,ExcuteEndDay =   @ExcuteEndDay   \n");
                sbQuery.Append("      ,RealEndDay	=   @RealEndDay     \n");
                sbQuery.Append("      ,AdTime		=   @AdTime         \n");
                sbQuery.Append("      ,AdState		=   @AdState        \n");
                sbQuery.Append("      ,AdClass		=   @AdClass        \n");
                sbQuery.Append("      ,AdType		=   @AdType         \n");
                sbQuery.Append("      ,ScheduleType =   @ScheduleType   \n");
                sbQuery.Append("      ,AdRate		=   @AdRate         \n");
                sbQuery.Append("	  ,ModDt		=   GETDATE()       \n");   
                sbQuery.Append("	  ,RegID		=   @RegID          \n");            
				sbQuery.Append("	  ,LinkChannel	=   @LinkChannel    \n");            
				sbQuery.Append("	  ,Mgrade		=   @Mgrade         \n");            
				sbQuery.Append("	  ,HomeYn		=   @HomeYn         \n");            
				sbQuery.Append("	  ,ChannelYn	=   @ChannelYn      \n");            
				sbQuery.Append("      ,CugYn		=   @CugYn          \n");
				sbQuery.Append("      ,STBType		=   @STBType        \n");
                sbQuery.Append(" WHERE ItemNo = @ItemNo					\n");
                */
                #endregion

                sbQuery.Append("\n");
                sbQuery.Append("UPDATE ADVT_MST 					  \n");
                sbQuery.Append("   SET item_nm	    = :ItemName		  \n");
                sbQuery.Append("      ,begin_dy     = :ExcuteStartDay \n");
                sbQuery.Append("      ,end_dy       = :ExcuteEndDay   \n");
                sbQuery.Append("      ,rl_end_dy	= :RealEndDay     \n");
                sbQuery.Append("      ,advt_tm		= :AdTime         \n");
                sbQuery.Append("      ,advt_stt		= :AdState        \n");
                sbQuery.Append("      ,advt_clss	= :AdClass        \n");
                sbQuery.Append("      ,advt_typ		= :AdType         \n");
                sbQuery.Append("      ,sch_typ      = :ScheduleType   \n");
                sbQuery.Append("      ,advt_rate	= :AdRate         \n");
                sbQuery.Append("	  ,dt_update	= SYSDATE         \n");
                sbQuery.Append("	  ,id_update	= :RegID          \n");
                
                //sbQuery.Append("	  ,Mgrade		=   :Mgrade         \n");
                sbQuery.Append(" WHERE item_no = :ItemNo			  \n");

                // 쿼리실행
                try
                {
                    OracleParameter[] sqlParams = new OracleParameter[12];

                    _db.BeginTran();
                    sqlParams[i++] = new OracleParameter(":ItemName"       , OracleDbType.Varchar2,50);
                    sqlParams[i++] = new OracleParameter(":ExcuteStartDay" , OracleDbType.Char,8);
                    sqlParams[i++] = new OracleParameter(":ExcuteEndDay"   , OracleDbType.Char,8);
                    sqlParams[i++] = new OracleParameter(":RealEndDay"     , OracleDbType.Char,8);
                    sqlParams[i++] = new OracleParameter(":AdTime"         , OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":AdState"        , OracleDbType.Char,2);
                    sqlParams[i++] = new OracleParameter(":AdClass"        , OracleDbType.Char,2);
                    sqlParams[i++] = new OracleParameter(":AdType"         , OracleDbType.Char,2);
                    sqlParams[i++] = new OracleParameter(":ScheduleType"   , OracleDbType.Char,2);
                    sqlParams[i++] = new OracleParameter(":AdRate"         , OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":RegID"          , OracleDbType.Varchar2,10);					
                    //sqlParams[i++] = new OracleParameter(":Mgrade"         , OracleDbType.Char,2);					
                    sqlParams[i++] = new OracleParameter(":ItemNo"         , OracleDbType.Int32); 
                    
                    i = 0;
                    sqlParams[i++].Value = contractItemModel.ItemName;     
                    sqlParams[i++].Value = contractItemModel.ExcuteStartDay;
                    sqlParams[i++].Value = contractItemModel.ExcuteEndDay; 

                    if(contractItemModel.RealEndDay.Trim().Length > 0)
                    {
                        sqlParams[i++].Value = contractItemModel.RealEndDay;			
                    }
                    else
                    {
                        _log.Debug("AdTime:["+contractItemModel.ExcuteEndDay+"]");

                        sqlParams[i++].Value = contractItemModel.ExcuteEndDay;			
                    }

					_log.Debug("contractItemModel.ItemNo		:[" + contractItemModel.ItemNo		 + "]");				
					_log.Debug("contractItemModel.ItemName		:[" + contractItemModel.ItemName.Trim() + "]");
					_log.Debug("contractItemModel.ExcuteStartDay:[" + contractItemModel.ExcuteStartDay + "]");
					_log.Debug("contractItemModel.ExcuteEndDay	:[" + contractItemModel.ExcuteEndDay	 + "]");
					_log.Debug("contractItemModel.RealEndDay	:[" + contractItemModel.RealEndDay	 + "]");
					_log.Debug("contractItemModel.AdTime		:[" + contractItemModel.AdTime		 + "]");				
					_log.Debug("contractItemModel.AdState		:[" + contractItemModel.AdState		 + "]");
					_log.Debug("contractItemModel.AdClass		:[" + contractItemModel.AdClass		 + "]");
					_log.Debug("contractItemModel.AdType		:[" + contractItemModel.AdType		 + "]");
					_log.Debug("contractItemModel.ScheduleType	:[" + contractItemModel.ScheduleType + "]");
					_log.Debug("contractItemModel.AdRate		:[" + contractItemModel.AdRate		 + "]");        
					//_log.Debug("contractItemModel.Mgrade		:[" + contractItemModel.Mgrade		 + "]");  
					
                    sqlParams[i++].Value = contractItemModel.AdTime;       
                    sqlParams[i++].Value = contractItemModel.AdState;      
                    sqlParams[i++].Value = contractItemModel.AdClass;      
                    sqlParams[i++].Value = contractItemModel.AdType;       
                    sqlParams[i++].Value = contractItemModel.ScheduleType; 
                    sqlParams[i++].Value = contractItemModel.AdRate;       
                    sqlParams[i++].Value = header.UserID;					

					//sqlParams[i++].Value = contractItemModel.Mgrade;  
					sqlParams[i++].Value = contractItemModel.ItemNo;       
					
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);


                    //변경내역을  내용변경:20으로 셋팅
                    //만약 광고상태가 종료일경우 종료일 처리 값 설정
                    if(contractItemModel.AdState.Equals("40"))
                    {
                        contractItemModel.HistoryType = "30";
                    }
                    else
                    {
                        contractItemModel.HistoryType = "20";
                    }
                    ContractITemHistoryCreate(contractItemModel);


					// 광고종류가 수정되었다면 
					#region 
					/*
					if(contractItemModel.AdTypeChangeType.Equals("1"))	// 필수->옵션
					{
						// 마지막승인번호
						string AckNo = GetLastAckNo(contractItemModel.MediaCode);

						// 해당광고가 편성되어있는 메뉴를 조회하여 편성순서를 마지막으로 변경한다.
						sbQuery = new StringBuilder();
						sbQuery.Append("\n"
							+ " SELECT GenreCode FROM SchChoiceMenuDetail        \n"
							+ "  WHERE ItemNo = " + contractItemModel.ItemNo + " \n"
							+ "  ORDER BY GenreCode                              \n"
							);

						// __DEBUG__
						_log.Debug(sbQuery.ToString());
						// __DEBUG__
				
						// 쿼리실행
						ds = new DataSet();
						_db.ExecuteQuery(ds,sbQuery.ToString());

						foreach (DataRow row in ds.Tables[0].Rows)
						{					
							string GenreCode = row["GenreCode"].ToString();

							sbQuery = new StringBuilder();
							sbQuery.Append("\n"
								+ " UPDATE SchChoiceMenuDetail                                       \n"
								+ "    SET ScheduleOrder = (SELECT ISNULL(MAX(ScheduleOrder),0) + 1 \n"
								+ "                           FROM SchChoiceMenuDetail               \n"
								+ "                          WHERE MediaCode = " + contractItemModel.MediaCode + "  \n"
								+ "                            AND GenreCode = " + GenreCode                   + ") \n"
								+ "       ,AckNo     = " + AckNo                       + " \n"
								+ "  WHERE MediaCode = " + contractItemModel.MediaCode + " \n"
								+ "    AND GenreCode = " + GenreCode                   + " \n"
								+ "    AND ItemNo    = " + contractItemModel.ItemNo    + " \n"
								);

							// __DEBUG__
							_log.Debug(sbQuery.ToString());
							// __DEBUG__

							rc =  _db.ExecuteNonQuery(sbQuery.ToString());
						}
						ds.Dispose();


						// 해당광고가 편성되어있는 채널를 조회하여 편성순서를 마지막으로 변경한다.
						sbQuery = new StringBuilder();
						sbQuery.Append("\n"
							+ " SELECT ChannelNo FROM SchChoiceChannelDetail     \n"
							+ "  WHERE ItemNo = " + contractItemModel.ItemNo + " \n"
							+ "  ORDER BY ChannelNo                              \n"
							);

						// __DEBUG__
						_log.Debug(sbQuery.ToString());
						// __DEBUG__
				
						// 쿼리실행
						ds = new DataSet();
						_db.ExecuteQuery(ds,sbQuery.ToString());

						foreach (DataRow row in ds.Tables[0].Rows)
						{					
							string ChannelNo = row["ChannelNo"].ToString();

							sbQuery = new StringBuilder();
							sbQuery.Append("\n"
								+ " UPDATE SchChoiceChannelDetail                                    \n"
								+ "    SET ScheduleOrder = (SELECT ISNULL(MAX(ScheduleOrder),0) + 1 \n"
								+ "                           FROM SchChoiceMenuDetail               \n"
								+ "                          WHERE MediaCode = " + contractItemModel.MediaCode + "  \n"
								+ "                            AND GenreCode = " + ChannelNo                   + ") \n"
								+ "       ,AckNo     = " + AckNo                       + " \n"
								+ "  WHERE MediaCode = " + contractItemModel.MediaCode + " \n"
								+ "    AND ChannelNo = " + ChannelNo                   + " \n"
								+ "    AND ItemNo    = " + contractItemModel.ItemNo    + " \n"
								);

							// __DEBUG__
							_log.Debug(sbQuery.ToString());
							// __DEBUG__


							rc =  _db.ExecuteNonQuery(sbQuery.ToString());
						}
						ds.Dispose();

					}
					else if(contractItemModel.AdTypeChangeType.Equals("2")) // 옵션->필수
					{
						// 마지막승인번호
						string AckNo = GetLastAckNo(contractItemModel.MediaCode);

						// 해당광고가 편성되어있는 메뉴를 조회하여 편성순서를 0으로 변경한다.
						sbQuery = new StringBuilder();
						sbQuery.Append("\n"
							+ " SELECT GenreCode, ScheduleOrder FROM SchChoiceMenuDetail        \n"
							+ "  WHERE ItemNo = " + contractItemModel.ItemNo + " \n"
							+ "  ORDER BY GenreCode                              \n"
							);

						// __DEBUG__
						_log.Debug(sbQuery.ToString());
						// __DEBUG__
				
						// 쿼리실행
						ds = new DataSet();
						_db.ExecuteQuery(ds,sbQuery.ToString());

						foreach (DataRow row in ds.Tables[0].Rows)
						{					
							string GenreCode     = row["GenreCode"].ToString();
							string ScheduleOrder = row["ScheduleOrder"].ToString();

							// 순서를 0으로 변경
							sbQuery = new StringBuilder();
							sbQuery.Append("\n"
								+ " UPDATE SchChoiceMenuDetail                             \n"
								+ "    SET ScheduleOrder = 0                               \n"
								+ "       ,AckNo     = " + AckNo                       + " \n"
								+ "  WHERE MediaCode = " + contractItemModel.MediaCode + " \n"
								+ "    AND GenreCode = " + GenreCode                   + " \n"
								+ "    AND ItemNo    = " + contractItemModel.ItemNo    + " \n"
								);

							// __DEBUG__
							_log.Debug(sbQuery.ToString());
							// __DEBUG__

							rc =  _db.ExecuteNonQuery(sbQuery.ToString());

							// 해당순서보다 큰 순서를 1씩 감산
							sbQuery = new StringBuilder();
							sbQuery.Append("\n"
								+ " UPDATE SchChoiceMenuDetail                                 \n"
								+ "    SET ScheduleOrder = ScheduleOrder - 1                   \n"
								+ "       ,AckNo         = " + AckNo                       + " \n"
								+ "  WHERE MediaCode     = " + contractItemModel.MediaCode + " \n"
								+ "    AND GenreCode     = " + GenreCode                   + " \n"
								+ "    AND ScheduleOrder > " + ScheduleOrder               + " \n"
								);

							// __DEBUG__
							_log.Debug(sbQuery.ToString());
							// __DEBUG__

							rc =  _db.ExecuteNonQuery(sbQuery.ToString());
						}
						ds.Dispose();


						// 해당광고가 편성되어있는 채널를 조회하여 편성순서를 0으로 변경한다.
						sbQuery = new StringBuilder();
						sbQuery.Append("\n"
							+ " SELECT ChannelNo, ScheduleOrder FROM SchChoiceChannelDetail        \n"
							+ "  WHERE ItemNo = " + contractItemModel.ItemNo + " \n"
							+ "  ORDER BY ChannelNo                              \n"
							);

						// __DEBUG__
						_log.Debug(sbQuery.ToString());
						// __DEBUG__
				
						// 쿼리실행
						ds = new DataSet();
						_db.ExecuteQuery(ds,sbQuery.ToString());

						foreach (DataRow row in ds.Tables[0].Rows)
						{					
							string ChannelNo     = row["ChannelNo"].ToString();
							string ScheduleOrder = row["ScheduleOrder"].ToString();

							// 순서를 0으로 변경
							sbQuery = new StringBuilder();
							sbQuery.Append("\n"
								+ " UPDATE SchChoiceChannelDetail                          \n"
								+ "    SET ScheduleOrder = 0                               \n"
								+ "       ,AckNo     = " + AckNo                       + " \n"
								+ "  WHERE MediaCode = " + contractItemModel.MediaCode + " \n"
								+ "    AND ChannelNo = " + ChannelNo                   + " \n"
								+ "    AND ItemNo    = " + contractItemModel.ItemNo    + " \n"
								);

							// __DEBUG__
							_log.Debug(sbQuery.ToString());
							// __DEBUG__

							rc =  _db.ExecuteNonQuery(sbQuery.ToString());

							// 해당순서보다 큰 순서를 1씩 감산
							sbQuery = new StringBuilder();
							sbQuery.Append("\n"
								+ " UPDATE SchChoiceChannelDetail                              \n"
								+ "    SET ScheduleOrder = ScheduleOrder - 1                   \n"
								+ "       ,AckNo         = " + AckNo                       + " \n"
								+ "  WHERE MediaCode     = " + contractItemModel.MediaCode + " \n"
								+ "    AND ChannelNo     = " + ChannelNo                   + " \n"
								+ "    AND ScheduleOrder > " + ScheduleOrder               + " \n"
								);

							// __DEBUG__
							_log.Debug(sbQuery.ToString());
							// __DEBUG__

							rc =  _db.ExecuteNonQuery(sbQuery.ToString());
						}
						ds.Dispose();
					}
					*/
					#endregion
                    _db.CommitTran();
                        
                    //__MESSAGE__
                    _log.Message("광고내역정보수정:["+contractItemModel.ItemNo+ "]["+ contractItemModel.ItemName + "] 등록자:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                contractItemModel.ResultCD = "0000";  // 정상
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractItemUpdate() End");
                _log.Debug("-----------------------------------------");
                        
            }
            catch(Exception ex)
            {
                contractItemModel.ResultCD   = "3201";
				if(NameCnt > 0)
				{
					contractItemModel.ResultDesc = "동일한 광고명이 이미 존재합니다.";
				}
				else
				{
					contractItemModel.ResultDesc = "광고내역정보 수정중 오류가 발생하였습니다";
					_log.Exception(ex);
				}
            }
            finally
            {
                _db.Close();
            }
        }

		/// <summary>
		/// 광고상태 일괄변경 처리(수정코드(E_01])
		/// </summary>		
		public void SetMultiAdState(HeaderModel header, ContractItemModel contractItemModel)
		{
			#region 처리 Process....
			// 1. 다수의 광고 컨텐츠 [광고상태] 업데이트 처리
			//    [광고상태:종료] 인 경우에는 RealEndDay 는 금일(yyyyMMdd)로 업데이트
			//                                파일상태도 셋탑삭제로 업데이트
			// 2. History 내역 처리[광고상태:종료-HistoryType=30(종료처리)]
			//                     [그 외는 내용변경 HistoryType=20(내용변경)]				
			#endregion
									
			int i = 0;
			int rc = 0;

			// 광고상태 코드:삭제
            string stAdDel = ConfigurationManager.AppSettings.Get("StAdDel");
			StringBuilder sbQuery = new StringBuilder(200);

			try
			{
				if (contractItemModel.ContractItemDataSet == null)
					throw new Exception("구성 DataSet Null 입니다.");
			    if (contractItemModel.ContractItemDataSet.Tables == null)
					throw new Exception("DataSet 의 table이 0 입니다.");
				if (contractItemModel.ContractItemDataSet.Tables[0].Rows.Count ==0)
					throw new Exception("DataRow 수가 0 입니다.");

				_db.Open();
				_db.BeginTran();																				                   				 				

				//광고상태:삭제(40)
				if (contractItemModel.AdState.Equals(stAdDel))  
				{   
					sbQuery.Remove(0, sbQuery.Length);
					sbQuery.Append("UPDATE ContractItem	      				     \n");
					sbQuery.Append("Set    AdState    =    @AdState              \n");
					sbQuery.Append("      ,RealEndDay =    @RealEndDay           \n");						
					sbQuery.Append("	  ,ModDt      =    GETDATE()             \n");   
					sbQuery.Append("	  ,RegID      =    @RegID                \n");				
					sbQuery.Append("      ,FileState  =    '90'                  \n"); //파일상태 셋탑삭제로 수정
					sbQuery.Append("      ,STBDelDt   =    GetDate()             \n");
					sbQuery.Append("      ,STBDelId   =    @RegId                \n");
					sbQuery.Append(" WHERE ItemNo = @ItemNo					 	 \n");

					foreach(DataRow row in contractItemModel.ContractItemDataSet.Tables[0].Rows)
					{						
						i = 0;
						SqlParameter[] sqlParams = new SqlParameter[4];
						sqlParams[i++] = new SqlParameter("@ItemNo"        , SqlDbType.Int);
						sqlParams[i++] = new SqlParameter("@AdState"       , SqlDbType.Char,2);
					    sqlParams[i++] = new SqlParameter("@RealEndDay"    , SqlDbType.Char ,8);
						sqlParams[i++] = new SqlParameter("@RegID"         , SqlDbType.VarChar ,10);						

						i = 0;
						sqlParams[i++].Value = Convert.ToInt32(row["ItemNo"]);
                        sqlParams[i++].Value = contractItemModel.AdState;
                        sqlParams[i++].Value = row["RealEndDay"].ToString();
						sqlParams[i++].Value = header.UserID;

						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
						
						// History 내역처리: 종료처리
                        contractItemModel.HistoryType = ConfigurationManager.AppSettings.Get("HistoryDel");
						contractItemModel.ItemNo = row["ItemNo"].ToString();
						ContractITemHistoryCreate(contractItemModel);
					}
				}
				else
				{
					sbQuery.Remove(0, sbQuery.Length);
					sbQuery.Append("UPDATE ContractItem	      				     \n");						
					sbQuery.Append("Set    AdState    =    @AdState              \n");						
					sbQuery.Append("	  ,ModDt      =    GETDATE()             \n");   
					sbQuery.Append("	  ,RegID      =    @RegID                \n");						
					sbQuery.Append(" WHERE ItemNo = @ItemNo					 	 \n");

					foreach(DataRow row in contractItemModel.ContractItemDataSet.Tables[0].Rows)
					{						
						i = 0;
						SqlParameter[] sqlParams = new SqlParameter[3];
						sqlParams[i++] = new SqlParameter("@ItemNo"        , SqlDbType.Int);
						sqlParams[i++] = new SqlParameter("@AdState"       , SqlDbType.Char,2);
						sqlParams[i++] = new SqlParameter("@RegID"         , SqlDbType.VarChar ,10);
						
						i = 0;
						sqlParams[i++].Value = Convert.ToInt32(row["ItemNo"]);
						sqlParams[i++].Value = contractItemModel.AdState;
						sqlParams[i++].Value = header.UserID;
						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
						
						// History 내역처리: 내용변경
                        contractItemModel.HistoryType = ConfigurationManager.AppSettings.Get("HistoryEdit");
						contractItemModel.ItemNo = row["ItemNo"].ToString();
						ContractITemHistoryCreate(contractItemModel);
					}
				}				

				_db.CommitTran();
				contractItemModel.ResultCD = FrameSystem.DBSuccess;
			}
			catch(Exception ex)
			{
				contractItemModel.ResultCD = FrameSystem.DBEditFail;
				contractItemModel.ResultDesc = ex.Message;
				_db.RollbackTran();
			}
			finally
			{
				_db.Close();
			}
		}


		#region 현재승인상태의 승인번호를 구함

		/// <summary>
		/// 현재승인상태의 승인번호를 구함
		/// 상태가 30:배포승인이면 신규(상태 10:편성중) 으로 생성후 AckNo 리턴
		/// </summary>
		/// <returns>string</returns>
		private string GetLastAckNo(string MediaCode)
		{

			string AckNo    = "";
			string AckState = "";

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLastAckNo() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode	       :[" + MediaCode     + "]");		// 검색 매체				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ " DECLARE @AckNo int, @AckState Char(2), @MediaCode int    \n"
					+ "                                                          \n"
					+ " SELECT @MediaCode = " + MediaCode                    + " \n"
					+ "                                                          \n"
					+ " SELECT TOP 1 @AckState = State, @AckNo = AckNo           \n"
					+ "   FROM SchPublish                                        \n"
					+ "  WHERE MediaCode = @MediaCode                            \n"
					+ "  ORDER BY AckNo DESC                                     \n"
					+ "                                                          \n"
					+ " IF @AckState = '30' OR @AckState IS NULL                 \n"
					+ " BEGIN                                                    \n"
					+ "	    INSERT INTO SchPublish                               \n"
					+ "                (AckNo, MediaCode, State, ModifyStartDay) \n"
					+ "         SELECT ISNULL(MAX(AckNo),0) + 1                  \n"
					+ "               ,@MediaCode                                \n"
					+ "               ,'10'                                      \n"
					+ "               ,GETDATE()                                 \n"
					+ "          FROM SchPublish                                 \n"
					+ "         WHERE MediaCode = @MediaCode                     \n"
					+ "                                                          \n"
					+ "     SELECT TOP 1 @AckState = State, @AckNo = AckNo       \n"
					+ "       FROM SchPublish                                    \n"
					+ "      WHERE MediaCode = @MediaCode                        \n"
					+ "      ORDER BY AckNo DESC                                 \n"
					+ " END                                                      \n"
					+ "                                                          \n"
					+ " SELECT @AckNo AS AckNo, @AckState AS AckState            \n"                             
					);
	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					AckNo    =  ds.Tables[0].Rows[0]["AckNo"].ToString();
					AckState =  ds.Tables[0].Rows[0]["AckState"].ToString();
				}

				ds.Dispose();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLastAckNo() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}
			return AckNo;
		}

		#endregion

        /// <summary>
        /// 연결채널 생성
        /// </summary>
        /// <param name="header"></param>
        /// <param name="contractItemModel"></param>
        public void SetLinkChannelCreate(HeaderModel header, ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkChannelCreate() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbQuery2 = new StringBuilder();


                sbQuery.Append(""
                    + "        DELETE FROM  LinkChannel			        \n"
                    + "              WHERE ItemNo = @ItemNo1			\n"
                    + "              AND Channel = @LinkChannel1			\n"
                    );
                sbQuery2.Append(""
                    + "INSERT INTO LinkChannel (\n"
                    + "       ItemNo			\n"
                    + "      ,Channel			\n"
                    + "      )					\n"
                    + " VALUES(					\n"
                    + "       @ItemNo			\n"
                    + "      ,@LinkChannel		\n"
                    + " );						\n"

                    );

                // 쿼리실행
                try
                {
                    int i = 0;
                    int rc = 0;
                    SqlParameter[] sqlParams = new SqlParameter[2];
                    _db.BeginTran();

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@ItemNo1", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@LinkChannel1", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(contractItemModel.ItemNo);
                    if (contractItemModel.LinkChannel.Trim().Length > 0)
                    {
                        sqlParams[i++].Value = contractItemModel.LinkChannel;
                    }
                    else
                    {
                        sqlParams[i++].Value = 0;
                    }
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _log.Debug("ItemNo      :[" + contractItemModel.ItemNo + "]");
                    _log.Debug(sbQuery.ToString());



                    sqlParams = new SqlParameter[2];
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@LinkChannel", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(contractItemModel.ItemNo);
                    if (contractItemModel.LinkChannel.Trim().Length > 0)
                    {
                        sqlParams[i++].Value = contractItemModel.LinkChannel;
                    }
                    else
                    {
                        sqlParams[i++].Value = 0;
                    }
                    rc = _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams);
                    _log.Debug("ItemNo      :[" + contractItemModel.ItemNo + "]");
                    _log.Debug("Channel     :[" + contractItemModel.LinkChannel + "]");
                    _log.Debug(sbQuery2.ToString());


                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("연결채널정보생성:[" + contractItemModel.LinkChannel + "(" + contractItemModel.LinkChannel + ")] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                contractItemModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkChannelCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                contractItemModel.ResultCD = "3101";
                contractItemModel.ResultDesc = "연결채널정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 연결채널 생성
        /// </summary>
        /// <param name="header"></param>
        /// <param name="contractItemModel"></param>
        public void SetLinkChannelCreate2(HeaderModel header, ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkChannelCreate() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                String itemNo = contractItemModel.ItemNo.Trim().ToString();
                int linkType = contractItemModel.LinkType;
                string linkChannel = contractItemModel.LinkChannel;
                _log.Debug("-----------------------------------------");
                _log.Debug("ItemNo:     [" + itemNo + "]");
                _log.Debug("LinkChannel:[" + linkChannel + "]");
                _log.Debug("LinkType:   [" + linkType + "]");
                _log.Debug("-----------------------------------------");
                // __DEBUG__

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbQuery2 = new StringBuilder();

                if (linkType == 1)
                {
                    sbQuery.Append(""
                        + "        DELETE FROM  LinkChannel			        \n"
                        + "              WHERE ItemNo = @ItemNo1			\n"
                        + "              AND Channel = @LinkChannel1		\n"
                        );
                    sbQuery2.Append(""
                        + "INSERT INTO LinkChannel (\n"
                        + "       ItemNo			\n"
                        + "      ,Channel			\n"
                        + "      )					\n"
                        + " VALUES(					\n"
                        + "       @ItemNo			\n"
                        + "      ,@LinkChannel		\n"
                        + " );						\n"

                        );

                }
                else if (linkType == 2)
                {
                    sbQuery.Append(""
                        + "        DELETE FROM  LinkChannel1			        \n"
                        + "              WHERE ItemNo = @ItemNo1			\n"
                        + "              AND Channel = @LinkChannel1		\n"
                        );
                    sbQuery2.Append(""
                        + "INSERT INTO LinkChannel1 (\n"
                        + "       ItemNo			\n"
                        + "      ,Channel			\n"
                        + "      )					\n"
                        + " VALUES(					\n"
                        + "       @ItemNo			\n"
                        + "      ,@LinkChannel		\n"
                        + " );						\n"
                        );
                }


                // 쿼리실행
                try
                {
                    int rc = 0;
                    SqlParameter[] sqlParams = new SqlParameter[2];
                    _db.BeginTran();

                    sqlParams[0] = new SqlParameter("@ItemNo1", SqlDbType.Int);
                    sqlParams[1] = new SqlParameter("@LinkChannel1", SqlDbType.Int);

					sqlParams[0].Value = Convert.ToInt32(contractItemModel.ItemNo);
                    if (contractItemModel.LinkChannel.Trim().Length > 0)
                    {
                        sqlParams[1].Value = contractItemModel.LinkChannel;
                    }
                    else
                    {
                        sqlParams[1].Value = 0;
                    }

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _log.Debug("ItemNo      :[" + contractItemModel.ItemNo + "]");
                    _log.Debug(sbQuery.ToString());

                    sqlParams = new SqlParameter[2];
                    sqlParams[0] = new SqlParameter("@ItemNo", SqlDbType.Int);
                    sqlParams[1] = new SqlParameter("@LinkChannel", SqlDbType.Int);
                    
                    sqlParams[0].Value = Convert.ToInt32(contractItemModel.ItemNo);
                    if (contractItemModel.LinkChannel.Trim().Length > 0)
                    {
                        sqlParams[1].Value = contractItemModel.LinkChannel;
                    }
                    else
                    {
                        sqlParams[1].Value = 0;
                    }           

                    rc = _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams);
                    _log.Debug("ItemNo      :[" + contractItemModel.ItemNo + "]");
                    _log.Debug("Channel     :[" + contractItemModel.LinkChannel + "]");
                    _log.Debug("LinkType    :[" + contractItemModel.LinkType + "]");
                    _log.Debug(sbQuery2.ToString());

                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("연결채널정보생성:[" + contractItemModel.LinkChannel + "(" + contractItemModel.LinkChannel + ")] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                contractItemModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkChannelCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                contractItemModel.ResultCD = "3101";
                contractItemModel.ResultDesc = "연결채널정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 연결채널 생성
        /// </summary>
        /// <param name="header"></param>
        /// <param name="contractItemModel"></param>
        public void SetLinkChannelDelete(HeaderModel header, ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkChannelDelete() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append(""
                    + "        DELETE FROM  LinkChannel			        \n"
                    + "              WHERE ItemNo = @ItemNo			\n"
                    + "              AND Channel = @LinkChannel			\n"
                    );


                // 쿼리실행
                try
                {
                    int i = 0;
                    int rc = 0;
                    SqlParameter[] sqlParams = new SqlParameter[2];
                    _db.BeginTran();

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@LinkChannel", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(contractItemModel.ItemNo);
                    if (contractItemModel.LinkChannel.Trim().Length > 0)
                    {
                        sqlParams[i++].Value = contractItemModel.LinkChannel;
                    }
                    else
                    {
                        sqlParams[i++].Value = 0;
                    }
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _log.Debug("ItemNo      :[" + contractItemModel.ItemNo + "]");
                    _log.Debug(sbQuery.ToString());

                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("연결채널정보생성:[" + contractItemModel.LinkChannel + "(" + contractItemModel.LinkChannel + ")] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                contractItemModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkChannelDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                contractItemModel.ResultCD = "3101";
                contractItemModel.ResultDesc = "연결채널정보 삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

		/// <summary>
		/// 연결채널 생성
		/// </summary>
		/// <param name="header"></param>
		/// <param name="contractItemModel"></param>
		public void SetLinkChannelDelete2(HeaderModel header, ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetLinkChannelDelete() Start");
				_log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                String itemNo = contractItemModel.ItemNo.Trim().ToString();
                int linkType = contractItemModel.LinkType;
                string linkChannel = contractItemModel.LinkChannel;
                _log.Debug("-----------------------------------------");
                _log.Debug("ItemNo:     [" + itemNo + "]");
                _log.Debug("LinkChannel:[" + linkChannel + "]");
                _log.Debug("LinkType:   [" + linkType + "]");
                _log.Debug("-----------------------------------------");
                // __DEBUG__

				// 데이터베이스를 OPEN한다
				_db.Open();
                
				StringBuilder sbQuery = new StringBuilder();

                if (linkType == 1)
                {
                    sbQuery.Append(""
                        + "        DELETE FROM  LinkChannel			        \n"
                        + "              WHERE ItemNo = @ItemNo			    \n"
                        + "              AND Channel = @LinkChannel			\n"
                    );        
                }
                else if (linkType == 2)
                {
                    sbQuery.Append(""
                        + "        DELETE FROM  LinkChannel1			        \n"
                        + "              WHERE ItemNo = @ItemNo			    \n"
                        + "              AND Channel = @LinkChannel			\n"
                    );        
                }
                                               
				
				
				// 쿼리실행
				try
				{
					int rc = 0;
					SqlParameter[] sqlParams = new SqlParameter[2]; 
					_db.BeginTran();

					sqlParams[0] = new SqlParameter("@ItemNo"  , SqlDbType.Int);
                    sqlParams[1] = new SqlParameter("@LinkChannel", SqlDbType.Int);	
							
					sqlParams[0].Value = Convert.ToInt32(contractItemModel.ItemNo);	
					if(contractItemModel.LinkChannel.Trim().Length > 0)
					{
						sqlParams[1].Value = contractItemModel.LinkChannel;
					}
					else
					{
						sqlParams[1].Value = 0;		
					}

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _log.Debug("ItemNo      :[" + contractItemModel.ItemNo + "]");
                    _log.Debug("LinkChannel      :[" + contractItemModel.LinkChannel + "]");
                    _log.Debug("LinkType      :[" + contractItemModel.LinkType + "]");
                    _log.Debug(sbQuery.ToString());
					
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("연결채널정보생성:[" + contractItemModel.LinkChannel + "(" + contractItemModel.LinkChannel + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				contractItemModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetLinkChannelDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				contractItemModel.ResultCD   = "3101";
				contractItemModel.ResultDesc = "연결채널정보 삭제 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


        public void SetContractItemDelete(HeaderModel header, ContractItemModel contractItemModel)
        {
            int FileDistCount = 0;
            int SchHomeCount = 0;
            int SchChoiceMenuCount = 0;    
            int SchChoiceChannelCount = 0;
            int SchDesignatedCount = 0;
            int SchTotalCount = 0;

			int SchHomeKidsCount = 0;	//[E_03]
          
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractItemDelete() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbQuerySchHome = new StringBuilder();
                StringBuilder sbQuerySchHomeKids = new StringBuilder(); // [E_03]
                StringBuilder sbQuerySchChoiceMenu = new StringBuilder();
                StringBuilder sbQuerySchChoiceChannel = new StringBuilder();
                StringBuilder SbQuerySchDesinatedDetail = new StringBuilder(); // [E_02]
                StringBuilder sbQueryTargetDelete			= new StringBuilder();
                StringBuilder sbQueryHistoryDelete			= new StringBuilder();
                StringBuilder sbQueryContractItemDelete	= new StringBuilder();

                #region 삭제 할 것
                /*
                sbQuerySchHome.Append( "\n"
                    + "        SELECT COUNT(*) FROM    SchHome		                \n"
                    + "              WHERE ItemNo = @ItemNo                     			\n"
					);
				// [E_03]
				sbQuerySchHomeKids.Append("\n"
					+ "        SELECT COUNT(*) FROM    SchHomeKids                  \n"
					+ "              WHERE ItemNo = @ItemNo                     			\n"
					);                                      
                sbQuerySchChoiceMenu.Append( "\n"
                    + "        SELECT COUNT(*) FROM    SchChoiceMenu	            \n"
                    + "              WHERE ItemNo = @ItemNo                     			\n"
                    );                                       
                sbQuerySchChoiceChannel.Append( "\n"
                    + "        SELECT COUNT(*) FROM    SchChoiceChannel           \n"
                    + "              WHERE ItemNo = @ItemNo                     			\n"
                    );
                // [E_02]
                SbQuerySchDesinatedDetail.Append("\n"
                    + "        SELECT COUNT(*) FROM SchDesignatedDetail			\n"
                    + "               WHERE ItemNo = @ItemNo                              \n"
                    );               
                sbQueryTargetDelete.Append( "\n"
                    + "        DELETE FROM  Targeting										\n"
                    + "              WHERE ItemNo = @ItemNo                     			\n"
                    );
                sbQueryHistoryDelete.Append( "\n"
                    + "        DELETE FROM  ContractHistory								\n"
                    + "              WHERE ItemNo = @ItemNo                     			\n"
                    );      
                		
                sbQueryContractItemDelete.Append( "\n"
                    + "        DELETE FROM  ContractItem			                        \n"
                    + "              WHERE ItemNo = @ItemNo                     			\n"
                    );      
                */
                #endregion
                //파일배포내역
                sbQuery.Append("\n SELECT COUNT(*) FROM FILEDIST_DTL \n"
                             + " Where item_no = :ItemNo \n");
                // 캠페인 상세
                sbQuerySchHome.Append("\n"
                    + "        SELECT COUNT(*) FROM    CAMP_DTL		                \n"
                    + "              WHERE item_no = :ItemNo                     			\n"
                    );
                // 광고링크
                sbQuerySchHomeKids.Append("\n"
                    + "        SELECT COUNT(*) FROM   ADVT_LINK                  \n"
                    + "              WHERE item_no = :ItemNo                     			\n"
                    );
                sbQuerySchChoiceMenu.Append("\n"
                    + "        SELECT COUNT(*) FROM    SCHD_MENU	            \n"
                    + "              WHERE item_no = :ItemNo                     			\n"
                    );
                sbQuerySchChoiceChannel.Append("\n"
                    + "        SELECT COUNT(*) FROM    SCHD_TITLE           \n"
                    + "              WHERE item_no = :ItemNo       			\n"
                    );
                // 편성비율마스터
                SbQuerySchDesinatedDetail.Append("\n"
                    + "        SELECT COUNT(*) FROM SCHDRT_MST			\n"
                    + "               WHERE item_no = :ItemNo                              \n"
                    );               
                //타겟팅
                sbQueryTargetDelete.Append("\n"
                    + "        DELETE FROM  TAR_MST										\n"
                    + "              WHERE item_no = :ItemNo                     			\n"
                    );
                //광고변경내역
                sbQueryHistoryDelete.Append("\n"
                    + "        DELETE FROM  ADVT_HST								\n"
                    + "              WHERE item_no = :ItemNo               			\n"
                    );
                // 광고내역
                sbQueryContractItemDelete.Append("\n"
                    + "        DELETE FROM  ADVT_MST		        \n"
                    + "              WHERE item_no = :ItemNo		\n"
                    );      

                // 쿼리실행
                try
                {
                    int rc = 0;
                    int i = 0;
                    OracleParameter[] sqlParams = new OracleParameter[1];
                                         
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                                                   
                    i = 0;
                    sqlParams[i++].Value = contractItemModel.ItemNo;

                    #region 모바일편성 사용안함..
                    
                    // 파일 배포 내역
                    _log.Debug(sbQuery.ToString());

                    //홈광고편성 관계 Count조사///////////////////////////////////////////////
                    // __DEBUG__
                    _log.Debug(sbQuerySchHome.ToString());
                    // __DEBUG__

                    // 쿼리실행
                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);
                    FileDistCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                    
                    ds = new DataSet();                    
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                    i = 0;                    
                    sqlParams[i++].Value = contractItemModel.ItemNo;
                    _db.ExecuteQueryParams(ds,sbQuerySchHome.ToString(),sqlParams);

                    SchHomeCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					// [E_03]
					//홈광고(키즈)편성 관계 Count조사///////////////////////////////////////////////
					// __DEBUG__
					_log.Debug(sbQuerySchHomeKids.ToString());
					// __DEBUG__

					// 쿼리실행
					ds = new DataSet();
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                    i = 0;
                    sqlParams[i++].Value = contractItemModel.ItemNo;
					_db.ExecuteQueryParams(ds, sbQuerySchHomeKids.ToString(), sqlParams);

					SchHomeKidsCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                                                         
                    //지정메뉴편성 관계 Count조사///////////////////////////////////////////////
                    // __DEBUG__
                    _log.Debug(sbQuerySchChoiceMenu.ToString());
                    // __DEBUG__

                    // 쿼리실행
                    ds = new DataSet();
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                    i = 0;
                    sqlParams[i++].Value = contractItemModel.ItemNo;
                    _db.ExecuteQueryParams(ds,sbQuerySchChoiceMenu.ToString(),sqlParams);

                    SchChoiceMenuCount =  Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());                                       

                    //채널별 지정편성 관계 Count조사///////////////////////////////////////////////
                    // __DEBUG__
                    _log.Debug(sbQuerySchChoiceChannel.ToString());
                    // __DEBUG__

                    // 쿼리실행
                    ds = new DataSet();
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                    i = 0;
                    sqlParams[i++].Value = contractItemModel.ItemNo;
                    _db.ExecuteQueryParams(ds,sbQuerySchChoiceChannel.ToString(),sqlParams);

                    // [E_02]
                    //지정채널편성 관계 Count조사///////////////////////////////////////////////
                    // __DEBUG__
                    _log.Debug(SbQuerySchDesinatedDetail.ToString());
                    // __DEBUG__

                    // 쿼리실행
                    ds = new DataSet();
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                    i = 0;
                    sqlParams[i++].Value = contractItemModel.ItemNo;
                    _db.ExecuteQueryParams(ds, SbQuerySchDesinatedDetail.ToString(), sqlParams);

                    SchDesignatedCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());


					ds.Dispose();
                   

					// __DEBUG__                                      
					_log.Debug("SchHomeCount				-->" + SchHomeCount);
					_log.Debug("SchHomeKidsCount			-->" + SchHomeKidsCount);// [E_03]
                    _log.Debug("SchChoiceMenuCount		-->"+SchChoiceMenuCount);
                    _log.Debug("SchChoiceChannelCount	-->"+SchChoiceChannelCount );
                    _log.Debug("SchDescignatedCount		-->"+SchDesignatedCount);// [E_02]
					// __DEBUG__


                    SchTotalCount = FileDistCount + SchHomeCount + SchHomeKidsCount + SchChoiceMenuCount + SchChoiceChannelCount + SchDesignatedCount;


					// 이미 사용중인 데이터가 있다면 Exception를 발생시킨다.
					if(SchTotalCount > 0) throw new Exception();
                    
                    #endregion

                    _db.BeginTran();
          
                    //타겟팅 정보를 삭제한다.
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                    i = 0;
                    sqlParams[i++].Value = contractItemModel.ItemNo;
                    rc =  _db.ExecuteNonQueryParams(sbQueryTargetDelete.ToString(), sqlParams);
                    // __DEBUG__
                    _log.Debug(sbQueryTargetDelete.ToString());
                    // __DEBUG__
                        
                    //광고 계약 이력 정보를 삭제 한다.
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                    i = 0;
                    sqlParams[i++].Value = contractItemModel.ItemNo;
                    rc =  _db.ExecuteNonQueryParams(sbQueryHistoryDelete.ToString(), sqlParams);
                
                    // __DEBUG__
                    _log.Debug(sbQueryHistoryDelete.ToString());
                    // __DEBUG__
                        
                    //광고 계약 정보 삭제 한다.
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                    i = 0;
                    sqlParams[i++].Value = contractItemModel.ItemNo;
                    rc =  _db.ExecuteNonQueryParams(sbQueryContractItemDelete.ToString(), sqlParams);
                    // __DEBUG__
                    _log.Debug(sbQueryContractItemDelete.ToString());
                    // __DEBUG__
                        

                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("광고내역 정보 삭제:[" + contractItemModel.ItemNo + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                contractItemModel.ResultCD = "0000";  // 정상
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractItemDelete() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                contractItemModel.ResultCD   = "3101";

				// 이미 사용중인 데이터가 있다면  
                if(SchTotalCount > 0 )
                {
					if(SchHomeCount > 0)
					{
						contractItemModel.ResultDesc = "이미 <홈광고>에 편성된 광고내역 이므로 삭제할 수 없습니다.";
					}
					// [E_03]
					else if (SchHomeKidsCount > 0)
					{
						contractItemModel.ResultDesc = "이미 <홈광고(키즈)>에 편성된 광고내역 이므로 삭제할 수 없습니다.";
					}
					else if (SchChoiceMenuCount > 0)
					{
						contractItemModel.ResultDesc = "이미 <지정메뉴>에 편성된 광고내역 이므로 삭제할 수 없습니다.";
					}
					else if (SchChoiceChannelCount > 0)
					{
						contractItemModel.ResultDesc = "이미 <지정채널>에 편성된 광고내역 이므로 삭제할 수 없습니다.";
					}
					// [E_02]
					else if (SchDesignatedCount > 0)
					{
						contractItemModel.ResultDesc = "이미 <지정편성>에 편성된 광고내역 이므로 삭제할 수 없습니다.";
					}
				}
                else
                {
					_log.Exception(ex);
					contractItemModel.ResultDesc = "광고계약 정보 삭제 중 오류가 발생하였습니다";
                }
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


        public void SetLinkItemCreate(HeaderModel header, ContractItemModel contractItemModel)
        {
            try
            {
                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[2];

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkItemCreate() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("ItemNo      :[" + contractItemModel.ItemNo + "]");
                _log.Debug("LinkItem    :[" + contractItemModel.LinkChannel + "]");
                                
                sbQuery.Append("\n insert into LinkItem");
                sbQuery.Append("\n      ( LinkType, ItemNo, LinkItemNo,RegDt )");
                sbQuery.Append("\n values( 1, @ItemNo,@LinkItemNo,GetDate() );");


                // 쿼리실행
                try
                {
                    int rc = 0;
                    
                    sqlParams[0] = new SqlParameter("@ItemNo", SqlDbType.Int);
                    sqlParams[0].Value = contractItemModel.ItemNo;
                    sqlParams[1] = new SqlParameter("@LinkItemNo", SqlDbType.Int);
                    sqlParams[1].Value = contractItemModel.LinkChannel;

                    _db.Open();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    
                    _log.Debug(sbQuery.ToString());
                    _log.Message("듀얼설정 생성:[" + contractItemModel.ItemNo + "(" + contractItemModel.LinkChannel + ")] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                contractItemModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkItemCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                contractItemModel.ResultCD = "3101";
                contractItemModel.ResultDesc = ex.Message;
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        public void SetLinkItemDelete(HeaderModel header, ContractItemModel contractItemModel)
        {
            try
            {
                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[2];

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkItemDelete() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("ItemNo      :[" + contractItemModel.ItemNo + "]");
                _log.Debug("LinkItem    :[" + contractItemModel.LinkChannel + "]");

                sbQuery.Append("\n delete from LinkItem");
                sbQuery.Append("\n where LinkType   = 1");
                sbQuery.Append("\n and   ItemNo     = @ItemNo");
                sbQuery.Append("\n and   LinkItemNo = @LinkItemNo");
                
                // 쿼리실행
                try
                {
                    int rc = 0;

                    sqlParams[0] = new SqlParameter("@ItemNo", SqlDbType.Int);
                    sqlParams[0].Value = contractItemModel.ItemNo;
                    sqlParams[1] = new SqlParameter("@LinkItemNo", SqlDbType.Int);
                    sqlParams[1].Value = contractItemModel.LinkChannel;

                    _db.Open();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _log.Debug(sbQuery.ToString());
                    _log.Message("듀얼설정 삭제:[" + contractItemModel.ItemNo + "(" + contractItemModel.LinkChannel + ")] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                contractItemModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkItemDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                contractItemModel.ResultCD = "3101";
                contractItemModel.ResultDesc = ex.Message;
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
    }
}