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
    /// ContractBiz에 대한 요약 설명입니다.
    /// </summary>
    public class ContractBiz : BaseBiz
    {
        public ContractBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }


        /// <summary>
        /// 광고계약목록조회
        /// </summary>
        /// <param name="contractModel"></param>
        public void GetContractList(HeaderModel header, ContractModel contractModel)
        {
            try
            {  // 데이터베이스를 OPEN한다
                _db.Open(); 

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + contractModel.SearchKey       + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                
                #region 삭제 할 것
                /*
                // 쿼리생성
                sbQuery.Append("\n");
                sbQuery.Append("SELECT   a.MediaCode								  \n");     
                sbQuery.Append("		,c.MediaName								  \n");
                sbQuery.Append("		,a.RapCode									  \n");
                sbQuery.Append("		,d.RapName									  \n");
                sbQuery.Append("		,a.AgencyCode								  \n");
                sbQuery.Append("		,e.AgencyName								  \n");
                sbQuery.Append("		,a.AdvertiserCode							  \n");
                sbQuery.Append("		,f.AdvertiserName							  \n");
                sbQuery.Append("		,a.ContractSeq								  \n");
                sbQuery.Append("		,a.ContractName								  \n");
				sbQuery.Append("		,(Select count(*) from ContractItem where ContractSeq = a.ContractSeq) Row								  \n");
                sbQuery.Append("		,a.ContStartDay								  \n");
                sbQuery.Append("		,a.ContEndDay								  \n");
				sbQuery.Append("		,a.ContractAmt								  \n");
                sbQuery.Append("		,a.State									  \n");				
                sbQuery.Append("		,g.CodeName StateName						  \n");
				sbQuery.Append("		,a.AdTime									  \n");
				sbQuery.Append("		,a.BonusRate								  \n");
				sbQuery.Append("		,a.LongBonus								  \n");
				sbQuery.Append("		,a.SpecialBonus								  \n");
				sbQuery.Append("		,a.TotalBonus								  \n");
				sbQuery.Append("		,a.SecurityTgt								  \n");
				sbQuery.Append("		,a.packageName								  \n");
				sbQuery.Append("		,a.Price									  \n");
                sbQuery.Append("		,a.Comment          						  \n");
				sbQuery.Append("		,j.JobCode          						  \n");				
				sbQuery.Append("		,j.JobCode2          						  \n");				
				sbQuery.Append("		,j.JobCode3          						  \n");
				sbQuery.Append("		,j.JobName          						  \n");				
				sbQuery.Append("		,j.JobName1          						  \n");				
				sbQuery.Append("		,j.JobName2          						  \n");				
				sbQuery.Append("		,j.JobName3          						  \n");				
                sbQuery.Append("       	,convert(char(19), a.RegDt, 120) RegDt        \n");      
                sbQuery.Append("		,convert(char(19), a.ModDt, 120) ModDt		  \n");
                sbQuery.Append("        ,b.UserName RegId							  \n");
                sbQuery.Append("  FROM  Contract a with(NoLock) LEFT JOIN (                          \n");
				sbQuery.Append("					SELECT a.JobCode								 \n");				
				sbQuery.Append("						  ,b.JobCode AS JobCode2                      \n");				
				sbQuery.Append("						  ,c.JobCode AS JobCode3                       \n");
				sbQuery.Append("						  ,CONVERT(VARCHAR(100),a.JobName) +'▶'+ CONVERT(VARCHAR(100),b.JobName) +'▶'+ CONVERT(VARCHAR(100),c.JobName) AS JobName  		       \n");
				sbQuery.Append("						  ,a.JobName AS JobName1                       \n");
				sbQuery.Append("						  ,b.JobName AS JobName2                       \n");
				sbQuery.Append("						  ,c.JobName AS JobName3                       \n");
				sbQuery.Append("					FROM JobClass a with(NoLock)                      \n");
				sbQuery.Append("  INNER JOIN JobClass b with(NoLock) ON (a.JobCode   = b.Level1Code  AND  b.Level=2)                      \n");
				sbQuery.Append("  INNER JOIN JobClass c with(NoLock) ON (a.JobCode   = c.Level1Code AND b.JobCode   = c.Level2Code  AND  c.Level=3)                      \n");
				sbQuery.Append("  WHERE  1 = 1)j ON (a.JobClass       = j.JobCode3)                      \n");
                sbQuery.Append("        LEFT JOIN SystemUser b with(NoLock) ON (a.RegId          = b.UserId)	        			  \n");
                sbQuery.Append("        LEFT JOIN Media      c with(NoLock) ON (a.Mediacode      = c.MediaCode)				  \n");
                sbQuery.Append("        LEFT JOIN MediaRap   d with(NoLock) ON (a.RapCode        = d.RapCode)					  \n");
                sbQuery.Append("        LEFT JOIN Agency     e with(NoLock) ON (a.AgencyCode     = e.AgencyCode)				  \n");
                sbQuery.Append("	    LEFT JOIN Advertiser f with(NoLock) ON (a.AdvertiserCode = f.AdvertiserCode)		  \n");
                sbQuery.Append("        LEFT JOIN SystemCode g with(NoLock) ON (a.State          = g.Code AND g.Section='23' )						  \n");				
                sbQuery.Append(" WHERE  1 = 1             						  \n");
				

                if(!contractModel.MediaCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.MediaCode = '"+contractModel.MediaCode+"'  \n");
                }
                if(!contractModel.RapCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.RapCode = '"+contractModel.RapCode+"'  \n");
                }        
                if(!contractModel.AgencyCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.AgencyCode = '"+contractModel.AgencyCode+"'  \n");
                }     
                if(!contractModel.AdvertiserCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.AdvertiserCode = '"+contractModel.AdvertiserCode+"'  \n");
                } 
                if(!contractModel.State.Equals("00"))
                {
                    sbQuery.Append("  AND    a.State = '"+contractModel.State+"'  \n");
                }          

                // 검색어가 있으면
                if (contractModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( a.ContractName LIKE '%" + contractModel.SearchKey.Trim() + "%' OR		\n"
                        + "        a.Comment LIKE '%" + contractModel.SearchKey.Trim() + "%'	\n"
						+ " OR e.AgencyName    LIKE '%" + contractModel.SearchKey.Trim() + "%'			\n"
						+ " OR f.AdvertiserName    LIKE '%" + contractModel.SearchKey.Trim() + "%'			\n"
					+ " 	)       \n"
                        );
                }
       
                sbQuery.Append("  ORDER BY A.ContractSeq Desc");
                */
                #endregion

                
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

                if (!contractModel.MediaCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.mda_cod = '" + contractModel.MediaCode + "'  \n");
                }
                if (!contractModel.RapCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.rep_cod = '" + contractModel.RapCode + "'  \n");
                }
                if (!contractModel.AgencyCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.agnc_cod = '" + contractModel.AgencyCode + "'  \n");
                }
                if (!contractModel.AdvertiserCode.Equals("00"))
                {
                    sbQuery.Append("  AND    a.advter_cod = '" + contractModel.AdvertiserCode + "'  \n");
                }
                if (!contractModel.State.Equals("00"))
                {
                    sbQuery.Append("  AND    a.cntr_stt = '" + contractModel.State + "'  \n");
                }

                // 검색어가 있으면
                if (contractModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( a.cntr_nm LIKE '%" + contractModel.SearchKey.Trim() + "%' OR	\n"
                        + "        a.cntr_memo LIKE '%" + contractModel.SearchKey.Trim() + "%'	\n"
                        + " OR e.agnc_nm    LIKE '%" + contractModel.SearchKey.Trim() + "%'		\n"
                        + " OR f.advter_nm    LIKE '%" + contractModel.SearchKey.Trim() + "%'	\n"
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
                contractModel.ContractDataSet = ds.Copy();
                // 결과
                contractModel.ResultCnt = Utility.GetDatasetCount(contractModel.ContractDataSet);
                // 결과코드 셋트
                contractModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + contractModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                contractModel.ResultCD = "3000";
                contractModel.ResultDesc = "광고계약정보 조회중 오류가 발생하였습니다";
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
		/// <param name="contractModel"></param>
		public void GetContractList2(HeaderModel header, ContractModel contractModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractList2() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + contractModel.SearchKey       + "]");
				_log.Debug("SearchState_10 :[" + contractModel.SearchState_10  + "]");
				_log.Debug("SearchState_20 :[" + contractModel.SearchState_20  + "]");
               
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
				

				if(!contractModel.MediaCode.Equals("00"))
				{
					sbQuery.Append("  AND    a.mda_cod = '"+contractModel.MediaCode+"'  \n");
				}
				if(!contractModel.RapCode.Equals("00"))
				{
                    sbQuery.Append("  AND    a.mda_cod = '" + contractModel.RapCode + "'  \n");
				}        
				if(!contractModel.AgencyCode.Equals("00"))
				{
                    sbQuery.Append("  AND    a.mda_cod = '" + contractModel.AgencyCode + "'  \n");
				}     

				// 검색어가 있으면
				if (contractModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("\n"
						+ "  AND ( a.cntr_nm   LIKE '%" + contractModel.SearchKey.Trim() + "%' \n"
						+ "     OR e.agnc_nm     LIKE '%" + contractModel.SearchKey.Trim() + "%' \n"
						+ "     OR f.advter_nm LIKE '%" + contractModel.SearchKey.Trim() + "%'	\n"
						+ "  )      \n"
						);
				}
       
				// 광고계약상태 선택에 따라

				bool isState = false;

				// 광고계약상태 운영중
				if(contractModel.SearchState_10.Trim().Length > 0 && contractModel.SearchState_10.Trim().Equals("Y"))
				{
					sbQuery.Append("  AND ( a.cntr_stt  = '10' \n");
					isState = true;
				}	
				// 광고상태 편성
				if(contractModel.SearchState_20.Trim().Length > 0 && contractModel.SearchState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append("             OR ");
					else sbQuery.Append("  AND ( ");
					sbQuery.Append(" a.cntr_stt = '20' \n");
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
				contractModel.ContractDataSet = ds.Copy();
				// 결과
				contractModel.ResultCnt = Utility.GetDatasetCount(contractModel.ContractDataSet);
				// 결과코드 셋트
				contractModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + contractModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				contractModel.ResultCD = "3000";
				contractModel.ResultDesc = "광고계약정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}


		/// <summary>
		/// 광고팩키지목록조회
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetContractPackageList(HeaderModel header, ContractModel contractModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractPackageList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + contractModel.SearchKey       + "]");
				_log.Debug("SearchRap      :[" + contractModel.SearchRap       + "]");
				_log.Debug("SearchUse      :[" + contractModel.SearchUse       + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT a.PackageNo     \n"     
					+ "		  ,a.PackageName   \n"
					+ "       ,a.RapCode       \n"
					+ "       ,ISNULL(c.RapName,'공용') AS RapName \n"
					+ "		  ,a.AdTime        \n"
					+ "		  ,a.ContractAmt   \n"
					+ "		  ,a.BonusRate     \n"
					+ "		  ,a.Price         \n"
					+ "		  ,a.Comment          					 \n"
					+ "		  ,a.UseYn         \n"
					+ "		  ,CASE a.UseYn  WHEN 'N'  THEN '사용안함' ELSE '' END UseNo  \n"
					+ "       ,convert(char(19), a.RegDt, 120) RegDt \n"      
					+ "	 	  ,convert(char(19), a.ModDt, 120) ModDt \n"
					+ "       ,b.UserName RegName					 \n"
					+ "  FROM ContractPackage a with(NoLock)         \n"
					+ "       LEFT JOIN SystemUser b with(NoLock) ON (a.RegId   = b.UserId)  \n"
					+ "       LEFT JOIN MediaRap   c with(NoLock) ON (a.RapCode = c.RapCode) \n"
					+ " WHERE  1 = 1  \n"
					);
				       
				if(!contractModel.SearchRap.Equals("00"))
				{
					sbQuery.Append("  AND(  a.RapCode = '"+contractModel.SearchRap+"' OR a.RapCode = 0 ) \n");
				}        


				// 검색어가 있으면
				if (contractModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("\n"
						+ "  AND ( a.PackageName LIKE '%" + contractModel.SearchKey.Trim() + "%' \n"
						+ "     OR c.RapName     LIKE '%" + contractModel.SearchKey.Trim() + "%' \n"
						+ "     OR a.Comment     LIKE '%" + contractModel.SearchKey.Trim() + "%' \n"
						+ " 	)       \n"
						);
				}

				if(!contractModel.SearchUse.Equals("N"))
				{
					sbQuery.Append("  AND a.UseYN = 'Y' \n");
				}        

       
				sbQuery.Append("  ORDER BY A.PackageNo");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				contractModel.ContractDataSet = ds.Copy();
				// 결과
				contractModel.ResultCnt = Utility.GetDatasetCount(contractModel.ContractDataSet);
				// 결과코드 셋트
				contractModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + contractModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractPackageList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				contractModel.ResultCD = "3000";
				contractModel.ResultDesc = "광고팩키지정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		/// <summary>
		/// 대분류 콤보선택
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetLevel1List(HeaderModel header, ContractModel contractModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLevel1List() Start");
				_log.Debug("-----------------------------------------");
				
				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT JobCode					\n"     
					+ "		  ,JobName					\n"								
					+ "  FROM JobClass					\n"					
					+ " WHERE  1 = 1  \n"
					);
				
				sbQuery.Append("  AND(  Level=1 ) \n");				
		

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				contractModel.ContractDataSet = ds.Copy();
				// 결과
				contractModel.ResultCnt = Utility.GetDatasetCount(contractModel.ContractDataSet);
				// 결과코드 셋트
				contractModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + contractModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLevel1List() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				contractModel.ResultCD = "3000";
				contractModel.ResultDesc = "업종 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		/// <summary>
		/// 업종선택
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetJobList(HeaderModel header, ContractModel contractModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetJobList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + contractModel.SearchKey       + "]");
				_log.Debug("JobCode      :[" + contractModel.JobCode       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT a.JobCode					\n"     
					+ "		  ,a.JobName					\n"
					+ "       ,b.Level1Code					\n"
					+ "       ,b.Level2Code					\n"
					+ "		  ,b.JobName AS JobName2        \n"
					+ "		  ,CONVERT(VARCHAR(10),a.JobCode) + ' ' + a.JobName AS JobName3        \n"
					+ "		  ,b.Level						\n"					
					+ "  FROM JobClass a with(NoLock)         \n"
					+ "       LEFT JOIN JobClass b with(NoLock) ON (a.JobCode   = b.Level1Code)  \n"					
					+ " WHERE  1 = 1  \n"
					);
				
				sbQuery.Append("  AND  b.Level=2  \n");				

				if(!contractModel.JobCode.Equals("00"))
				{
					sbQuery.Append("  AND  b.Level1Code = '"+contractModel.JobCode+"'  \n");
				}        


				// 검색어가 있으면
				if (contractModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("\n"
						+ "  AND ( a.JobName	 LIKE '%" + contractModel.SearchKey.Trim() + "%' \n"
						+ "     OR b.Def	     LIKE '%" + contractModel.SearchKey.Trim() + "%' \n"
						+ "     OR b.Example     LIKE '%" + contractModel.SearchKey.Trim() + "%' \n"
						+ " 	)       \n"
						);
				}
       
				sbQuery.Append("  ORDER BY a.JobCode, b.Level2Code");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				contractModel.ContractDataSet = ds.Copy();
				// 결과
				contractModel.ResultCnt = Utility.GetDatasetCount(contractModel.ContractDataSet);
				// 결과코드 셋트
				contractModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + contractModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetJobList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				contractModel.ResultCD = "3000";
				contractModel.ResultDesc = "업종 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}


		/// <summary>
		/// 소분류선택
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetLevel3List(HeaderModel header, ContractModel contractModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLevel3List() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");				
				_log.Debug("Level2Code      :[" + contractModel.Level2Code       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT c.JobCode					\n"     
					+ "		  ,a.JobName					\n"
					+ "       ,b.Level1Code				 \n"
					+ "       ,b.JobName AS JobName2  				 \n"
					+ "       ,c.Level2Code				 \n"
					+ "       ,c.JobName AS JobName3				 \n"
					+ "		  ,c.Def						 \n"
					+ "		  ,c.Example						\n"					
					+ "  FROM JobClass a with(NoLock)			         \n"					
					+ "  INNER JOIN JobClass b with(NoLock) ON (a.JobCode   = b.Level1Code  AND  b.Level=2)			         \n"					
					+ "  INNER JOIN JobClass c with(NoLock) ON (a.JobCode   = c.Level1Code AND b.JobCode   = c.Level2Code  AND  c.Level=3)			         \n"					
					+ " WHERE  1 = 1  \n"
					);
				
				sbQuery.Append("  AND(  c.Level=3 ) \n");				
				
				sbQuery.Append("  AND(  c.Level2Code = '"+contractModel.Level2Code+"' ) \n");


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				contractModel.ContractDataSet = ds.Copy();
				// 결과
				contractModel.ResultCnt = Utility.GetDatasetCount(contractModel.ContractDataSet);
				// 결과코드 셋트
				contractModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + contractModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLevel3List() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				contractModel.ResultCD = "3000";
				contractModel.ResultDesc = "업종 조회중 오류가 발생하였습니다";
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
		/// <param name="contractModel"></param>
		public void GetContractItemList(HeaderModel header, ContractModel contractModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + contractModel.SearchKey       + "]");
				_log.Debug("ContractSeq      :[" + contractModel.ContractSeq       + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
                
                #region 삭제 할 것
                // 쿼리생성
                /*
				sbQuery.Append("\n");
				sbQuery.Append("SELECT * FROM ( 																												\n");     
				sbQuery.Append("SELECT a.ItemNo 																													\n");     
				sbQuery.Append("      ,a.ItemName																													\n");     
				sbQuery.Append("      ,e.ContractName																											\n");
				sbQuery.Append("	  ,a.AdvertiserCode																												\n");
				sbQuery.Append("      ,b.AdvertiserName																											\n");
				sbQuery.Append("      ,a.ExcuteStartDay																											\n");
				sbQuery.Append("	  ,a.ExcuteEndDay																												\n");
				sbQuery.Append("	  ,a.RealEndDay																													\n");
				sbQuery.Append("	  ,a.AdState 																														\n");
				sbQuery.Append("      ,c.CodeName AdStateName																								\n");
				sbQuery.Append("      ,a.FilePath																														\n");
				sbQuery.Append("      ,a.FileName																													\n");
				sbQuery.Append("      ,a.FileState																													\n");
				sbQuery.Append("      ,d.CodeName FileStateName																								\n");
				sbQuery.Append("      ,(SELECT COUNT(*) FROM SchHome	WHERE ItemNo = a.ItemNo)");
				sbQuery.Append("			+(SELECT COUNT(*) FROM SchHomeKids	WHERE ItemNo = a.ItemNo) AS HomeCount				\n");
				sbQuery.Append("      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail    WHERE ItemNo = a.ItemNo) AS MenuCount		\n");
				sbQuery.Append("      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail WHERE ItemNo = a.ItemNo) AS ChannelCount	\n");
				sbQuery.Append("      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay																	\n");
				sbQuery.Append("      ,a.AdType																														\n");
				sbQuery.Append("      ,f.CodeName AdTypeName																								\n");
				sbQuery.Append("FROM   ContractItem a with(NoLock)																						\n");
				sbQuery.Append("       LEFT JOIN Advertiser b with(NoLock) ON (a.AdvertiserCode = b.AdvertiserCode)						\n");
				sbQuery.Append("       LEFT JOIN Contract   e with(NoLock) ON (a.ContractSeq    = e.ContractSeq)							\n");    
				sbQuery.Append("       LEFT JOIN SystemCode c with(NoLock) ON (a.AdState        = c.Code AND c.Section = '25' )		\n");      
				sbQuery.Append("       LEFT JOIN SystemCode d with(NoLock) ON (a.FileState      = d.Code AND d.Section = '31' )		\n");
				sbQuery.Append("       LEFT JOIN SystemCode f with(NoLock) ON (a.AdType         = f.Code AND f.Section = '26' )			\n");    
				sbQuery.Append(" WHERE 1=1																														\n");
				
				if(!contractModel.MediaCode.Equals("00"))
				{
					sbQuery.Append("  AND    a.MediaCode = '"+contractModel.MediaCode+"'  \n");
				}
				if(!contractModel.RapCode.Equals("00"))
				{
					sbQuery.Append("  AND    a.RapCode = '"+contractModel.RapCode+"'  \n");
				}        
				if(!contractModel.AgencyCode.Equals("00"))
				{
					sbQuery.Append("  AND    a.AgencyCode = '"+contractModel.AgencyCode+"'  \n");
				}     
				if(!contractModel.AdvertiserCode.Equals("00"))
				{
					sbQuery.Append("  AND    a.AdvertiserCode = '"+contractModel.AdvertiserCode+"'  \n");
				} 

				sbQuery.Append("  AND    a.ContractSeq = '"+contractModel.ContractSeq+"'  \n");
//				if(!contractModel.AdType.Equals("00"))
//				{
//					sbQuery.Append("  AND    a.AdType = '"+contractModel.AdType+"'  \n");
//				} 
//				bool isState = false;
//				// 광고상태 선택에 따라
//
//				// 광고상태 준비
//				if(contractModel.SearchchkAdState_10.Trim().Length > 0 && contractModel.SearchchkAdState_10.Trim().Equals("Y"))
//				{
//					sbQuery.Append(" AND ( A.AdState  = '10' \n");
//					isState = true;
//				}	
//				// 광고상태 편성
//				if(contractModel.SearchchkAdState_20.Trim().Length > 0 && contractModel.SearchchkAdState_20.Trim().Equals("Y"))
//				{
//					if(isState) sbQuery.Append(" OR ");
//					else sbQuery.Append(" AND ( ");
//					sbQuery.Append(" A.AdState  = '20' \n");
//					isState = true;
//				}	
//				// 광고상태 중지
//				if(contractModel.SearchchkAdState_30.Trim().Length > 0 && contractModel.SearchchkAdState_30.Trim().Equals("Y"))
//				{
//					if(isState) sbQuery.Append(" OR ");
//					else sbQuery.Append(" AND ( ");
//					sbQuery.Append(" A.AdState  = '30' \n");
//					isState = true;
//				}	
//				// 광고상태 종료
//				if(contractModel.SearchchkAdState_40.Trim().Length > 0 && contractModel.SearchchkAdState_40.Trim().Equals("Y"))
//				{
//					if(isState) sbQuery.Append(" OR ");
//					else sbQuery.Append(" AND ( ");
//					sbQuery.Append(" A.AdState  = '40' \n");
//					isState = true;
//				}	
//
//				if(isState) sbQuery.Append(" ) \n");
//
//				// 검색어가 있으면
//				if (contractModel.SearchKey.Trim().Length > 0)
//				{
//					// 여러컬럼에 대하여 LIKE 검색을 한다.
//					sbQuery.Append("\n"
//						+ "  AND ( a.ItemName LIKE '%" + contractModel.SearchKey.Trim() + "%' OR		\n"
//						+ "        a.FileName LIKE '%" + contractModel.SearchKey.Trim() + "%'     \n"
//						+ " OR b.AdvertiserName    LIKE '%" + contractModel.SearchKey.Trim() + "%'			\n"
//						+ "		)        \n"
//						);
//				}

				sbQuery.Append(""
					+ ") TA "
					+ "WHERE 1 = 1"
					);

                */
                #endregion

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
				sbQuery.Append("\n          FROM  ADVT_MST a 				    ");
                sbQuery.Append("\n          LEFT JOIN ( SELECT x.mda_cod, x.agnc_cod, x.rep_cod ");
                sbQuery.Append("\n                             ,y.advter_cod ,y.advter_nm       ");
                sbQuery.Append("\n                             ,x.cntr_seq   ,x.cntr_nm         ");
                sbQuery.Append("\n                      FROM CNTR x                             ");      
                sbQuery.Append("\n                      LEFT JOIN ADVTER y ON (x.advter_cod = y.advter_cod) ");
                sbQuery.Append("\n          ) t ON (a.cntr_seq = t.cntr_seq)    ");    
				sbQuery.Append("\n          LEFT JOIN STM_COD c ON (a.advt_stt   = c.stm_cod AND c.stm_cod_cls = '25' ) ");
				sbQuery.Append("\n          LEFT JOIN STM_COD d ON (a.file_stt   = d.stm_cod AND d.stm_cod_cls = '31' )	");	
				sbQuery.Append("\n          LEFT JOIN STM_COD f ON (a.advt_typ   = f.stm_cod AND f.stm_cod_cls = '26' )	");
                sbQuery.Append("\n          WHERE 1 = 1     ");


                if (!contractModel.MediaCode.Equals("00"))
                {
                    sbQuery.Append("  AND    t.mda_cod = '" + contractModel.MediaCode + "'  \n");
                }
                if (!contractModel.RapCode.Equals("00"))
                {
                    sbQuery.Append("  AND    t.rep_cod = '" + contractModel.RapCode + "'  \n");
                }
                if (!contractModel.AgencyCode.Equals("00"))
                {
                    sbQuery.Append("  AND    t.agnc_cod = '" + contractModel.AgencyCode + "'  \n");
                }
                if (!contractModel.AdvertiserCode.Equals("00"))
                {
                    sbQuery.Append("  AND    t.advter_cod = '" + contractModel.AdvertiserCode + "'  \n");
                }

                sbQuery.Append("  AND    t.cntr_seq = '" + contractModel.ContractSeq + "'  \n");


                sbQuery.Append("\n ) ta                     ");
                sbQuery.Append("\n WHERE 1=1                ");


//				// 편성여부가 Y/N일 때 
//				if(contractModel.SearchChkSch_YN.Trim().Length > 0)
//				{
//					if(contractModel.SearchChkSch_YN.Trim().Equals("Y"))
//					{
//						// 편성된것만
//						sbQuery.Append(" AND ( HomeCount > 0 OR MenuCount > 0 OR  ChannelCount > 0 ) \n");
//					}
//					else if(contractModel.SearchChkSch_YN.Trim().Equals("N"))
//					{
//						//편성안된것만
//						sbQuery.Append(" AND ( HomeCount = 0 AND MenuCount = 0 AND  ChannelCount = 0 )\n");
//					}
//				}
				// 아니면 전체
       
				sbQuery.Append("ORDER BY ta.ItemNo Desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고내역모델에 복사
				contractModel.ContractDataSet = ds.Copy();
				// 결과
				contractModel.ResultCnt = Utility.GetDatasetCount(contractModel.ContractDataSet);
				// 결과코드 셋트
				contractModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + contractModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				contractModel.ResultCD = "3000";
				contractModel.ResultDesc = "광고내역정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}      

        /// <summary>
        /// 광고계약 생성
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="UserName"></param>
        /// <param name="UserPassword"></param>
        /// <param name="UserLevel"></param>
        /// <param name="UserDept"></param>
        /// <param name="UserTitle"></param>
        /// <param name="UserTell"></param>
        /// <param name="UserMobile"></param>
        /// <param name="UserComment"></param>
        /// <returns></returns>
        public void SetContractCreate(HeaderModel header, ContractModel contractModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractCreate() Start");
                _log.Debug("-----------------------------------------");
                                 
                    
                //AdGroup Insert Query
                StringBuilder sbQuery = new StringBuilder();
                                                                 
                sbQuery.Append("\n");
                sbQuery.Append("INSERT INTO CNTR (			    \n");
                sbQuery.Append("            mda_cod			    \n");
                sbQuery.Append("           ,rep_cod			    \n");
                sbQuery.Append("           ,agnc_cod		    \n");
                sbQuery.Append("           ,advter_cod		    \n");
                sbQuery.Append("           ,cntr_seq			\n");
                sbQuery.Append("           ,cntr_nm     		\n");
				sbQuery.Append("           ,advt_tm				\n");
				sbQuery.Append("           ,bns_rt  			\n");
				sbQuery.Append("           ,long_bns			\n");
				sbQuery.Append("           ,spcl_bns    		\n");
				sbQuery.Append("           ,tot_bns 			\n");
				sbQuery.Append("           ,grte_imps			\n");								
				sbQuery.Append("           ,cntr_prc			\n");
                sbQuery.Append("           ,cntr_stt			\n");
                sbQuery.Append("           ,cntr_begin_dy		\n");
                sbQuery.Append("           ,cntr_end_dy			\n");
				sbQuery.Append("           ,cntr_amt			\n");
                sbQuery.Append("           ,cntr_memo			\n");
                sbQuery.Append("           )        			\n");
                sbQuery.Append("     SELECT						\n");
                sbQuery.Append("           :MediaCode			\n");
                sbQuery.Append("           ,:RapCode			\n");
                sbQuery.Append("           ,:AgencyCode			\n");
                sbQuery.Append("           ,:AdvertiserCode		\n");
                sbQuery.Append("           ,NVL(MAX(cntr_seq),0)+1 \n");
                sbQuery.Append("           ,:ContractName		\n");
				sbQuery.Append("           ,:AdTime				\n");
				sbQuery.Append("           ,:BonusRate			\n");
				sbQuery.Append("           ,:LongBonus			\n");
				sbQuery.Append("           ,:SpecialBonus		\n");
				sbQuery.Append("           ,:TotalBonus			\n");
				sbQuery.Append("           ,:SecurityTgt		\n");				
				sbQuery.Append("           ,:price				\n");
                sbQuery.Append("           ,:State				\n");
                sbQuery.Append("           ,:ContStartDay		\n");
				sbQuery.Append("           ,:ContEndDay			\n");
				sbQuery.Append("           ,:ContractAmt		\n");
				sbQuery.Append("           ,:Comments			\n");                
                sbQuery.Append("     FROM CNTR  \n");                                      
                // 쿼리실행
                try
                {
                                                
                    int i = 0;
                    int rc = 0;
                    //광고 그룹 Insert
                    OracleParameter[] sqlParams = new OracleParameter[17];
                    
                                
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":MediaCode"       ,OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":RapCode"         ,OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":AgencyCode"      ,OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":AdvertiserCode"  ,OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":ContractName"    ,OracleDbType.Varchar2, 50);
                    sqlParams[i++] = new OracleParameter(":AdTime"          ,OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":BonusRate", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":LongBonus", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":SpecialBonus", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":TotalBonus", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":SecurityTgt", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":Price", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":State"           ,OracleDbType.Char	 , 2);
                    sqlParams[i++] = new OracleParameter(":ContStartDay"    ,OracleDbType.Char	 , 8);
                    sqlParams[i++] = new OracleParameter(":ContEndDay"      ,OracleDbType.Char	 , 8);
					sqlParams[i++] = new OracleParameter(":ContractAmt"     ,OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":Comments"        ,OracleDbType.Varchar2	 , 50);
                    
                                                     
                                                      
                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(contractModel.MediaCode);
                    sqlParams[i++].Value = Convert.ToInt32(contractModel.RapCode);
                    sqlParams[i++].Value = Convert.ToInt32(contractModel.AgencyCode);
                    sqlParams[i++].Value = Convert.ToInt32(contractModel.AdvertiserCode);
                    sqlParams[i++].Value = contractModel.ContractName;
					if(contractModel.AdTime.Trim().Length > 0)
					{
                        sqlParams[i++].Value = Convert.ToDecimal(contractModel.AdTime);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}					
					if(contractModel.BonusRate.Trim().Length > 0)
					{
                        sqlParams[i++].Value = Convert.ToDecimal(contractModel.BonusRate);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractModel.LongBonus.Trim().Length > 0)
					{
                        sqlParams[i++].Value = Convert.ToDecimal(contractModel.LongBonus);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractModel.SpecialBonus.Trim().Length > 0)
					{
                        sqlParams[i++].Value = Convert.ToDecimal(contractModel.SpecialBonus);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractModel.TotalBonus.Trim().Length > 0)
					{
                        sqlParams[i++].Value = Convert.ToDecimal(contractModel.TotalBonus);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractModel.SecurityTgt.Trim().Length > 0)
					{
                        sqlParams[i++].Value = Convert.ToDecimal(contractModel.SecurityTgt);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}					
					if(contractModel.Price.Trim().Length > 0)
					{
                        sqlParams[i++].Value = Convert.ToDecimal(contractModel.Price);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
                    sqlParams[i++].Value = contractModel.State;
                    sqlParams[i++].Value = contractModel.ContStartDay;
                    sqlParams[i++].Value = contractModel.ContEndDay;
					if(contractModel.ContractAmt.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToDecimal(contractModel.ContractAmt);		
					}
					else
					{
						sqlParams[i++].Value = null;		
					}
                    sqlParams[i++].Value = contractModel.Comment;
                    
                                    
                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());

                    // __DEBUG__
                    _db.BeginTran();             
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                   
                    // __MESSAGE__
					_log.Message("광고계약정보생성:["+contractModel.ContractName + "] 등록자:[" + header.UserID + "]");
            
                    _db.CommitTran();
            
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                                                    
                contractModel.ResultCD = "0000";  // 정상
                                                    
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractCreate() End");
                _log.Debug("-----------------------------------------");	
                                        
            }
            catch(Exception ex)
            {
                contractModel.ResultCD   = "3101";
                contractModel.ResultDesc = "광고계약정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        // 광고계약정보 수정

        public void SetContractUpdate(HeaderModel header, ContractModel contractModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractUpdate() Start");
                _log.Debug("-----------------------------------------");
                        


				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("ContractName     :[" + contractModel.ContractName   + "]");
				_log.Debug("AdTime	         :[" + contractModel.AdTime	        + "]");		
				_log.Debug("BonusRate	         :[" + contractModel.BonusRate	        + "]");	
				_log.Debug("JobClass	         :[" + contractModel.JobClass	        + "]");	
				_log.Debug("Price	         :[" + contractModel.Price	        + "]");		
				_log.Debug("State	         :[" + contractModel.State	        + "]");		
				_log.Debug("ContStartDay     :[" + contractModel.ContStartDay   + "]");	
				_log.Debug("ContEndDay       :[" + contractModel.ContEndDay     + "]");		
				_log.Debug("ContractAmt      :[" + contractModel.ContractAmt    + "]");		
				_log.Debug("Comment          :[" + contractModel.Comment        + "]");		
				_log.Debug("MediaCode        :[" + contractModel.MediaCode      + "]");		
				_log.Debug("RapCode          :[" + contractModel.RapCode        + "]");		
				_log.Debug("AgencyCode       :[" + contractModel.AgencyCode     + "]");		
				_log.Debug("AdvertiserCode   :[" + contractModel.AdvertiserCode + "]");		
				_log.Debug("ContractSeq      :[" + contractModel.ContractSeq    + "]");		
				// __DEBUG__


                StringBuilder sbQuery = new StringBuilder();
                        
                int i = 0;
                int rc = 0;
               
                sbQuery.Append("\n");
                sbQuery.Append("UPDATE CNTR							    \n");
                sbQuery.Append("   SET  cntr_nm = :ContractName		\n");
				sbQuery.Append("       ,advt_tm = :AdTime					\n");
				sbQuery.Append("       ,bns_rt = :BonusRate				\n");
				sbQuery.Append("       ,long_bns = :LongBonus				\n");
				sbQuery.Append("       ,spcl_bns = :SpecialBonus				\n");
				sbQuery.Append("       ,tot_bns  = :TotalBonus				\n");
				sbQuery.Append("       ,grte_imps = :SecurityTgt				\n");				
				sbQuery.Append("       ,cntr_prc = :Price						\n");
                sbQuery.Append("       ,cntr_stt = :State						\n");
                sbQuery.Append("       ,cntr_begin_dy = :ContStartDay		\n");
                sbQuery.Append("       ,cntr_end_dy = :ContEndDay			\n");
				sbQuery.Append("       ,cntr_amt = :ContractAmt			\n");
                sbQuery.Append("       ,cntr_memo = :Comments					\n");                
                sbQuery.Append(" WHERE  mda_cod = :MediaCode				\n");
                sbQuery.Append("   AND  rep_cod = :RapCode					\n");
                sbQuery.Append("   AND  agnc_cod = :AgencyCode			\n");
                sbQuery.Append("   AND  advter_cod = :AdvertiserCode	\n");
                sbQuery.Append("   AND  cntr_seq = :ContractSeq			\n");

                // 쿼리실행
                try
                {
                    OracleParameter[] sqlParams = new OracleParameter[18];

                    _db.BeginTran();
                    sqlParams[i++] = new OracleParameter(":ContractName"  , OracleDbType.Varchar2,50);
					sqlParams[i++] = new OracleParameter(":AdTime"       , OracleDbType.Int32              );
                    sqlParams[i++] = new OracleParameter(":BonusRate", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":LongBonus", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":SpecialBonus", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":TotalBonus", OracleDbType.Int32);
					sqlParams[i++] = new OracleParameter(":SecurityTgt"    , OracleDbType.Int32              );
					sqlParams[i++] = new OracleParameter(":Price"        , OracleDbType.Int32              );
                    sqlParams[i++] = new OracleParameter(":State"		   , OracleDbType.Char,20);
                    sqlParams[i++] = new OracleParameter(":ContStartDay"  , OracleDbType.Char,8);
                    sqlParams[i++] = new OracleParameter(":ContEndDay"	   , OracleDbType.Char,8);
					sqlParams[i++] = new OracleParameter(":ContractAmt"	   , OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":Comments"       , OracleDbType.Varchar2,50);
                    sqlParams[i++] = new OracleParameter(":MediaCode"     , OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":RapCode"       , OracleDbType.Int32);
					sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":AdvertiserCode", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":ContractSeq"   , OracleDbType.Int32);	

                    i = 0;
                    sqlParams[i++].Value = contractModel.ContractName;
					if(contractModel.AdTime.Trim().Length > 0)
					{
                        sqlParams[i++].Value = Convert.ToDecimal(contractModel.AdTime);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}					
					if(contractModel.BonusRate.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToDecimal(contractModel.BonusRate);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractModel.LongBonus.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToDecimal(contractModel.LongBonus);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractModel.SpecialBonus.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToDecimal(contractModel.SpecialBonus);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractModel.TotalBonus.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToDecimal(contractModel.TotalBonus);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractModel.SecurityTgt.Trim().Length > 0)
					{
                        sqlParams[i++].Value = Convert.ToDecimal(contractModel.SecurityTgt);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					
					if(contractModel.Price.Trim().Length > 0)
					{
                        sqlParams[i++].Value = Convert.ToDecimal(contractModel.Price);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
                    sqlParams[i++].Value = contractModel.State;			
                    sqlParams[i++].Value = contractModel.ContStartDay;	
                    sqlParams[i++].Value = contractModel.ContEndDay;
					if(contractModel.ContractAmt.Trim().Length > 0)
					{
                        sqlParams[i++].Value = Convert.ToDecimal(contractModel.ContractAmt);		
					}
					else
					{
						sqlParams[i++].Value = null;		
					}

                    sqlParams[i++].Value = contractModel.Comment;                    
                    sqlParams[i++].Value = contractModel.MediaCode;
                    sqlParams[i++].Value = contractModel.RapCode;
                    sqlParams[i++].Value = contractModel.AgencyCode;
                    sqlParams[i++].Value = contractModel.AdvertiserCode;
                    sqlParams[i++].Value = contractModel.ContractSeq;


                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _db.CommitTran();
                        
                    //__MESSAGE__
                    _log.Message("광고계약정보수정:[" + contractModel.ContractSeq.ToString() + "]["+contractModel.ContractName + "] 등록자:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                contractModel.ResultCD = "0000";  // 정상
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractUpdate() End");
                _log.Debug("-----------------------------------------");
                        
            }
            catch(Exception ex)
            {
                contractModel.ResultCD   = "3201";
                contractModel.ResultDesc = "광고계약정보 수정중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        public void SetContractDelete(HeaderModel header, ContractModel contractModel)
        {
            int ContractItemCount = 0;
          
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractDelete() Start");
                _log.Debug("-----------------------------------------");
                        
                StringBuilder sbQueryContractItemCount       = new StringBuilder();
                StringBuilder sbQueryContractDelete          = new StringBuilder();

            
                sbQueryContractItemCount.Append( "\n"
                    + "        SELECT COUNT(*) FROM    ADVT_MST	\n"
                    + "              WHERE cntr_seq = :ContractSeq          	\n"
                    );      

                sbQueryContractDelete.Append( "\n"
                    + "        DELETE FROM  CNTR			           \n"
                    + "              WHERE cntr_seq = :ContractSeq     \n"
                    );      
                		

                        
                // 쿼리실행
                try
                {
                    int rc = 0;
                    int i = 0;
                    OracleParameter[] sqlParams = new OracleParameter[1];
                    OracleParameter[] sqlParams2 = new OracleParameter[1];
                     
                    i = 0;
                    sqlParams[i++] = new OracleParameter(":ContractSeq", OracleDbType.Int32);
                    i = 0;
                    sqlParams2[i++] = new OracleParameter(":ContractSeq", OracleDbType.Int32);
                    
                    i = 0;
                    sqlParams[i++].Value = contractModel.ContractSeq;
                    i = 0;
                    sqlParams2[i++].Value = contractModel.ContractSeq;


                    //광고내 관계 Count조사///////////////////////////////////////////////
                    // __DEBUG__
                    _log.Debug(sbQueryContractItemCount.ToString());
                    // __DEBUG__

                    // 쿼리실행
                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds,sbQueryContractItemCount.ToString(),sqlParams);
                    
                    ContractItemCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					ds.Dispose();

                    _log.Debug("ContractItemCount          -->" + ContractItemCount);
                
                    // 이미 사용중인 데이터가 있다면 Exception를 발생시킨다.
                    if(ContractItemCount > 0) throw new Exception();

                    _db.BeginTran();
          
                    //광고 내역 테이블을 삭제 한다.
                    rc =  _db.ExecuteNonQueryParams(sbQueryContractDelete.ToString(), sqlParams2);
                    // __DEBUG__
                    _log.Debug(sbQueryContractDelete.ToString());
                    // __DEBUG__
                        
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("광고계약 정보 삭제:[" + contractModel.ContractSeq.ToString() + "] 등록자:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                contractModel.ResultCD = "0000";  // 정상
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractDelete() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                contractModel.ResultCD   = "3101";
                //이미 테이블에 사용즁인 데이터가 있을경우.
                if(ContractItemCount > 0 )
                {
                    contractModel.ResultDesc = "등록된 광고 내역이 있으므로 광고정보를 삭제할수 없습니다.";
                }
                else
                {
					_log.Exception(ex);
					contractModel.ResultDesc = "광고정보 삭제 중 오류가 발생하였습니다";
                }
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

    }
}