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
	/// MediaAgencyBiz에 대한 요약 설명입니다.
	/// </summary>
	public class MediaAgencyBiz : BaseBiz
	{
		public MediaAgencyBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}
		/// <summary>
		/// 매체대행사목록조회
		/// </summary>
		/// <param name="mediaAgencyModel"></param>
		public void GetMediaAgencyList(HeaderModel header, MediaAgencyModel mediaAgencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaAgencyList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + mediaAgencyModel.SearchKey       + "]");
				_log.Debug("SearchMediaAgency:[" + mediaAgencyModel.SearchMediaAgency + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("\n" + " SELECT  ");
                sbQuery.Append("\n" + "     a.mda_cod as MediaCode      ");
                sbQuery.Append("\n" + "     ,a.rep_cod as RapCode       ");
                sbQuery.Append("\n" + "     ,a.agnc_cod as AgencyCode   ");
                sbQuery.Append("\n" + "     ,b.mda_nm as MediaName      ");
                sbQuery.Append("\n" + "     ,c.rep_nm as RapName        ");
                sbQuery.Append("\n" + "     ,d.agnc_nm as AgencyName    ");
                sbQuery.Append("\n" + "     ,a.begin_dy as ContStartDay ");
                sbQuery.Append("\n" + "     ,a.end_dy as ContEndDay     ");
                sbQuery.Append("\n" + "     ,a.dutyr as Charger         ");
                sbQuery.Append("\n" + "     ,a.phone_no as Tell         ");
                sbQuery.Append("\n" + "     ,a.email as Email           ");
                sbQuery.Append("\n" + "     ,a.use_yn as UseYn          ");
                sbQuery.Append("\n" + "     ,CASE a.use_yn WHEN 'Y' THEN '' WHEN 'N' THEN '사용안함' END as UseYn_N ");
                sbQuery.Append("\n" + "     ,a.dt_insert as RegDt       ");
                sbQuery.Append("\n" + "     ,a.dt_update as ModDt       ");
                sbQuery.Append("\n" + "     ,e.user_nm as RegName       ");
                sbQuery.Append("\n" + " FROM MDA_AGNC a                 ");    
                sbQuery.Append("\n" + " LEFT JOIN MDA b ON a.mda_cod = b.mda_cod        ");
                sbQuery.Append("\n" + " LEFT JOIN MDA_REP c ON a.rep_cod = c.rep_cod    ");
                sbQuery.Append("\n" + " LEFT JOIN AGNC d ON a.agnc_cod = d.agnc_cod     ");
                sbQuery.Append("\n" + " LEFT JOIN STM_USER e ON a.id_insert = e.user_id ");
                sbQuery.Append("\n" + " WHERE 1 = 1 ");

                // 검색어가 있으면
                if (mediaAgencyModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append(" AND ("
                        + "\n    a.dutyr      LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + "\n OR b.mda_nm     LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + "\n OR c.rep_nm     LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + "\n OR d.agnc_nm    LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + "\n OR a.phone_no   LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + "\n OR a.email      LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + " ) ");
                }

                // 매체대행사레벨을 선택했으면
                if (mediaAgencyModel.SearchMediaName.Trim().Length > 0 && !mediaAgencyModel.SearchMediaName.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND a.mda_cod = '" + mediaAgencyModel.SearchMediaName.Trim() + "' \n");
                }
                if (mediaAgencyModel.SearchRapName.Trim().Length > 0 && !mediaAgencyModel.SearchRapName.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND a.rep_cod = '" + mediaAgencyModel.SearchRapName.Trim() + "' \n");
                }
                if (mediaAgencyModel.SearchMediaAgency.Trim().Length > 0 && !mediaAgencyModel.SearchMediaAgency.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND a.agnc_cod = '" + mediaAgencyModel.SearchMediaAgency.Trim() + "' \n");
                }
                sbQuery.Append(" ORDER BY A.mda_cod, a.rep_cod, A.agnc_cod DESC  \n");

                _log.Debug("UserClass      :[" + header.UserClass+ "]");
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 매체대행사모델에 복사
				mediaAgencyModel.MediaAgencyDataSet = ds.Copy();
				// 결과
				mediaAgencyModel.ResultCnt = Utility.GetDatasetCount(mediaAgencyModel.MediaAgencyDataSet);
				// 결과코드 셋트
				mediaAgencyModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + mediaAgencyModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaAgencyList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				mediaAgencyModel.ResultCD = "3000";
				mediaAgencyModel.ResultDesc = "매체대행사정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}


		/// <summary>
		/// 매체대행사팝업에서목록조회(매체대행사별 광고주관리에서 사용)
		/// </summary>
		/// <param name="mediaAgencyModel"></param>
		public void GetMediaAgencyPop(HeaderModel header, MediaAgencyModel mediaAgencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaAgencyPop() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + mediaAgencyModel.SearchKey       + "]");
				_log.Debug("SearchMediaAgency:[" + mediaAgencyModel.SearchMediaAgency + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
                #region 삭제 할 것
                /*
				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT					 \n"							
					+ "   A.MediaCode            \n"															
					+ "  ,A.RapCode              \n"															
					+ "  ,A.AgencyCode           \n"	
					+ "  ,B.MediaName            \n"							
					+ "  ,C.RapName              \n"			
					+ "  ,D.AgencyName           \n"							
					+ "  ,A.ContStartDay         \n"															
					+ "  ,A.ContEndDay           \n"															
					+ "  ,A.Charger              \n"															
					+ "  ,A.Tell                 \n"															
					+ "  ,A.Email                \n"															
					+ "  ,A.UseYn                \n"															
					+ "  ,A.RegDt                \n"															
					+ "  ,A.ModDt                \n"															
					+ "  ,E.UserName RegName     \n"															
					+ "  FROM MediaAgency A LEFT JOIN Media B ON A.MediaCode = B.MediaCode \n"	
					+ "                     LEFT JOIN MediaRap C ON A.RapCode = C.RapCode \n"	
					+ "                     LEFT JOIN Agency D ON A.AgencyCode = D.AgencyCode \n"	
					+ "                     LEFT JOIN SystemUser E ON A.RegId = E.UserId \n"	
					+ " WHERE 1 = 1  \n"				
					+ " AND A.UseYn = 'Y'  \n"				
					+ " AND B.UseYn = 'Y'  \n"				
					+ " AND C.UseYn = 'Y'  \n"				
					+ " AND D.UseYn = 'Y'  \n"				
					);
				
				// 검색어가 있으면
				if (mediaAgencyModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
						+ "    B.Charger      LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%' \n"	
						+ " OR B.MediaName    LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%' \n"
						+ " OR C.RapName    LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%' \n"
						+ " OR D.AgencyName    LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%' \n"
						+ " OR A.Tell    LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%' \n"
						+ " OR A.Email    LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%' \n"
						+ " ) ");
				}
				
				// 매체대행사레벨을 선택했으면
				if(mediaAgencyModel.SearchMediaName.Trim().Length > 0 && !mediaAgencyModel.SearchMediaName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode = '" + mediaAgencyModel.SearchMediaName.Trim() + "' \n");
				}
				if(mediaAgencyModel.SearchRapName.Trim().Length > 0 && !mediaAgencyModel.SearchRapName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.RapCode = '" + mediaAgencyModel.SearchRapName.Trim() + "' \n");
				}		
				if(mediaAgencyModel.SearchMediaAgency.Trim().Length > 0 && !mediaAgencyModel.SearchMediaAgency.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.AgencyCode = '" + mediaAgencyModel.SearchMediaAgency.Trim() + "' \n");
				}		

				sbQuery.Append(" ORDER BY A.MediaCode, A.RapCode, A.AgencyCode DESC  \n");
                */
                #endregion
                sbQuery.Append("\n" + " SELECT  ");
                sbQuery.Append("\n" + "     a.mda_cod as MediaCode      ");
                sbQuery.Append("\n" + "     ,a.rep_cod as RapCode       ");
                sbQuery.Append("\n" + "     ,a.agnc_cod as AgencyCode   ");
                sbQuery.Append("\n" + "     ,b.mda_nm as MediaName      ");
                sbQuery.Append("\n" + "     ,c.rep_nm as RapName        ");
                sbQuery.Append("\n" + "     ,d.agnc_nm as AgencyName    ");
                sbQuery.Append("\n" + "     ,a.begin_dy as ContStartDay ");
                sbQuery.Append("\n" + "     ,a.end_dy as ContEndDay     ");
                sbQuery.Append("\n" + "     ,a.dutyr as Charger         ");
                sbQuery.Append("\n" + "     ,a.phone_no as Tell         ");
                sbQuery.Append("\n" + "     ,a.email as Email           ");
                sbQuery.Append("\n" + "     ,a.use_yn as UseYn          ");                
                sbQuery.Append("\n" + "     ,a.dt_insert as RegDt       ");
                sbQuery.Append("\n" + "     ,a.dt_update as ModDt       ");
                sbQuery.Append("\n" + "     ,e.user_nm as RegName       ");
                sbQuery.Append("\n" + " FROM MDA_AGNC a                 ");
                sbQuery.Append("\n" + " LEFT JOIN MDA b ON a.mda_cod = b.mda_cod        ");
                sbQuery.Append("\n" + " LEFT JOIN MDA_REP c ON a.rep_cod = c.rep_cod    ");
                sbQuery.Append("\n" + " LEFT JOIN AGNC d ON a.agnc_cod = d.agnc_cod     ");
                sbQuery.Append("\n" + " LEFT JOIN STM_USER e ON a.id_insert = e.user_id ");
                sbQuery.Append("\n" + " WHERE 1 = 1 ");                
				sbQuery.Append("\n"	+ " AND a.use_yn = 'Y' ");
				sbQuery.Append("\n"	+ " AND b.use_yn = 'Y' ");				
				sbQuery.Append("\n"	+ " AND c.use_yn = 'Y' ");
                sbQuery.Append("\n" + " AND d.use_yn = 'Y' ");	

                // 검색어가 있으면
                if (mediaAgencyModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append(" AND ("
                        + "\n    a.dutyr      LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + "\n OR b.mda_nm     LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + "\n OR c.rep_nm     LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + "\n OR d.agnc_nm    LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + "\n OR a.phone_no   LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + "\n OR a.email      LIKE '%" + mediaAgencyModel.SearchKey.Trim() + "%'"
                        + " ) ");
                }

                // 매체대행사레벨을 선택했으면
                if (mediaAgencyModel.SearchMediaName.Trim().Length > 0 && !mediaAgencyModel.SearchMediaName.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND a.mda_cod = '" + mediaAgencyModel.SearchMediaName.Trim() + "' \n");
                }
                if (mediaAgencyModel.SearchRapName.Trim().Length > 0 && !mediaAgencyModel.SearchRapName.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND a.rep_cod = '" + mediaAgencyModel.SearchRapName.Trim() + "' \n");
                }
                if (mediaAgencyModel.SearchMediaAgency.Trim().Length > 0 && !mediaAgencyModel.SearchMediaAgency.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND a.agnc_cod = '" + mediaAgencyModel.SearchMediaAgency.Trim() + "' \n");
                }
                sbQuery.Append(" ORDER BY A.mda_cod, a.rep_cod, A.agnc_cod DESC  \n");

                _log.Debug("UserClass      :[" + header.UserClass+ "]");
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 매체대행사모델에 복사
				mediaAgencyModel.MediaAgencyDataSet = ds.Copy();
				// 결과
				mediaAgencyModel.ResultCnt = Utility.GetDatasetCount(mediaAgencyModel.MediaAgencyDataSet);
				// 결과코드 셋트
				mediaAgencyModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + mediaAgencyModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaAgencyPop() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				mediaAgencyModel.ResultCD = "3000";
				mediaAgencyModel.ResultDesc = "매체대행사정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}


		/// <summary>
		/// 매체대행사정보 저장
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
		public void SetMediaAgencyUpdate(HeaderModel header, MediaAgencyModel mediaAgencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaAgencyUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[10];

				sbQuery.Append("\n" + " UPDATE MDA_AGNC       ");
                sbQuery.Append("\n" + "   SET begin_dy      = :ContStartDay  ");
                sbQuery.Append("\n" + "      ,end_dy		= :ContEndDay    ");
                sbQuery.Append("\n" + "      ,dutyr			= :Charger    "); 
                sbQuery.Append("\n" + "      ,phone_no		= :Tell       ");
                sbQuery.Append("\n" + "      ,email			= :Email      ");					
				sbQuery.Append("\n" + "      ,use_yn		= :UseYn      ");														
				sbQuery.Append("\n" + "      ,dt_update		= SYSDATE   ");
				sbQuery.Append("\n" + "      ,id_update		= :RegID      ");
				sbQuery.Append("\n" + " WHERE mda_cod     = :MediaCode  ");
                sbQuery.Append("\n" + " AND rep_cod	  	  = :RapCode    ");					
				sbQuery.Append("\n" + " AND agnc_cod      = :AgencyCode ");					
                
                i = 0;
                sqlParams[i++] = new OracleParameter(":ContStartDay", OracleDbType.Char, 8);
                sqlParams[i++] = new OracleParameter(":ContEndDay", OracleDbType.Char, 8);
                sqlParams[i++] = new OracleParameter(":Charger", OracleDbType.Varchar2, 20);
                sqlParams[i++] = new OracleParameter(":Tell", OracleDbType.Varchar2, 15);
                sqlParams[i++] = new OracleParameter(":Email", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);

                sqlParams[i++] = new OracleParameter(":MediaCode", OracleDbType.Int16);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int16);
                sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int16);
		

				i = 0;
				sqlParams[i++].Value = mediaAgencyModel.ContStartDay;										
				sqlParams[i++].Value = mediaAgencyModel.ContEndDay;
				sqlParams[i++].Value = mediaAgencyModel.Charger;
				sqlParams[i++].Value = mediaAgencyModel.Tell;										
				sqlParams[i++].Value = mediaAgencyModel.Email;								
				sqlParams[i++].Value = mediaAgencyModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // 등록자

				sqlParams[i++].Value = Convert.ToInt16(mediaAgencyModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt16(mediaAgencyModel.RapCode);
				sqlParams[i++].Value = Convert.ToInt16(mediaAgencyModel.AgencyCode);

				_log.Debug("MediaCode:[" + mediaAgencyModel.MediaCode + "]");
				_log.Debug("RapCode:[" + mediaAgencyModel.RapCode + "]");
				_log.Debug("AgencyCode:[" + mediaAgencyModel.AgencyCode + "]");
				_log.Debug("ContStartDay:[" + mediaAgencyModel.ContStartDay + "]");
				_log.Debug("ContEndDay:[" + mediaAgencyModel.ContEndDay + "]");
				_log.Debug("Charger:[" + mediaAgencyModel.Charger + "]");
				_log.Debug("Tell:[" + mediaAgencyModel.Tell + "]");
				_log.Debug("Email:[" + mediaAgencyModel.Email + "]");
				_log.Debug("UseYn:[" + mediaAgencyModel.UseYn + "]");
				
				_log.Debug(sbQuery.ToString());
				

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("매체대행사정보수정:["+mediaAgencyModel.AgencyCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediaAgencyModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaAgencyUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				mediaAgencyModel.ResultCD   = "3201";
				mediaAgencyModel.ResultDesc = "매체대행사정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		/// <summary>
		/// 매체대행사 생성
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
		public void SetMediaAgencyCreate(HeaderModel header, MediaAgencyModel mediaAgencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaAgencyCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				OracleParameter[] sqlParams = new OracleParameter[10];
		
				
				sbQuery.Append("\n  INSERT INTO MDA_AGNC (  ");
				sbQuery.Append("\n 		 mda_cod            ");															
				sbQuery.Append("\n 		,rep_cod            ");															
				sbQuery.Append("\n 		,agnc_cod           ");															
				sbQuery.Append("\n 		,begin_dy           ");															
				sbQuery.Append("\n 		,end_dy             ");															
				sbQuery.Append("\n 		,dutyr              ");															
				sbQuery.Append("\n 		,phone_no           ");															
				sbQuery.Append("\n 		,email              ");															
				sbQuery.Append("\n 		,use_yn             ");															
                sbQuery.Append("\n 		,dt_insert          ");																				
                sbQuery.Append("\n 		,dt_update          ");																				
                sbQuery.Append("\n 		,id_insert          ");							
				sbQuery.Append("\n       )                  ");
				sbQuery.Append("\n  VALUES(                 ");			
				sbQuery.Append("\n        :MediaCode        ");						
				sbQuery.Append("\n       ,:RapCode          ");		
				sbQuery.Append("\n       ,:AgencyCode       ");		
				sbQuery.Append("\n       ,:ContStartDay     ");						
				sbQuery.Append("\n       ,:ContEndDay       ");						
				sbQuery.Append("\n       ,:Charger          ");						
				sbQuery.Append("\n       ,:Tell             ");						
				sbQuery.Append("\n       ,:Email            ");						
				sbQuery.Append("\n       ,:UseYn            ");	
                sbQuery.Append("\n       ,SYSDATE           ");	
                sbQuery.Append("\n       ,SYSDATE           ");	
                sbQuery.Append("\n       ,:RegID            ");
				sbQuery.Append("\n  )		                ");					
				
								
				sqlParams[i++] = new OracleParameter(":MediaCode"   , OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);

				sqlParams[i++] = new OracleParameter(":ContStartDay" , OracleDbType.Char , 8);
				sqlParams[i++] = new OracleParameter(":ContEndDay"   , OracleDbType.Char , 8);
				sqlParams[i++] = new OracleParameter(":Charger" , OracleDbType.Varchar2 , 10);
				sqlParams[i++] = new OracleParameter(":Tell"    , OracleDbType.Varchar2 , 15);	
				sqlParams[i++] = new OracleParameter(":Email"   , OracleDbType.Varchar2 , 40);				
				sqlParams[i++] = new OracleParameter(":UseYn"   , OracleDbType.Char , 1);
				sqlParams[i++] = new OracleParameter(":RegID"   , OracleDbType.Varchar2 , 10);		
	

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(mediaAgencyModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(mediaAgencyModel.RapCode);
				sqlParams[i++].Value = Convert.ToInt32(mediaAgencyModel.AgencyCode);

				sqlParams[i++].Value = mediaAgencyModel.ContStartDay;										
				sqlParams[i++].Value = mediaAgencyModel.ContEndDay;
				sqlParams[i++].Value = mediaAgencyModel.Charger;
				sqlParams[i++].Value = mediaAgencyModel.Tell;										
				sqlParams[i++].Value = mediaAgencyModel.Email;								
				sqlParams[i++].Value = mediaAgencyModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // 등록자
		
				
				_log.Debug("MediaCode:[" + mediaAgencyModel.MediaCode + "]");
				_log.Debug("RapCode:[" + mediaAgencyModel.RapCode + "]");
				_log.Debug("AgencyCode:[" + mediaAgencyModel.AgencyCode + "]");
				_log.Debug("ContStartDay:[" + mediaAgencyModel.ContStartDay + "]");
				_log.Debug("ContEndDay:[" + mediaAgencyModel.ContEndDay + "]");
				_log.Debug("Charger:[" + mediaAgencyModel.Charger + "]");
				_log.Debug("Tell:[" + mediaAgencyModel.Tell + "]");
				_log.Debug("Email:[" + mediaAgencyModel.Email + "]");
				_log.Debug("UseYn:[" + mediaAgencyModel.UseYn + "]");
				
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("매체대행사정보생성:[" + mediaAgencyModel.AgencyCode + "(" + mediaAgencyModel.AgencyCode + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediaAgencyModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaAgencyCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				mediaAgencyModel.ResultCD   = "3101";
				mediaAgencyModel.ResultDesc = "매체대행사정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

        /// <summary>
        /// 매체대행사 삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="mediaAgencyModel"></param>
		public void SetMediaAgencyDelete(HeaderModel header, MediaAgencyModel mediaAgencyModel)
		{
            int ClientCount = 0;
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaAgencyDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbQueryClientCount = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[3];
                
                sbQueryClientCount.Append("\n   SELECT COUNT(*) FROM    CLNT    ");
                sbQueryClientCount.Append("\n   WHERE mda_cod  = :MediaCode   ");
                sbQueryClientCount.Append("\n   AND rep_cod  = :RapCode         ");
                sbQueryClientCount.Append("\n   AND agnc_cod  = :AgencyCode   ");
                

				sbQuery.Append("\n DELETE MDA_AGNC         ");
				sbQuery.Append("\n WHERE mda_cod  = :MediaCode  ");
				sbQuery.Append("\n AND rep_cod    = :RapCode    ");
                sbQuery.Append("\n AND agnc_cod   = :AgencyCode ");


                sqlParams[i++] = new OracleParameter(":MediaCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);

				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(mediaAgencyModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(mediaAgencyModel.RapCode);
				sqlParams[i++].Value = Convert.ToInt32(mediaAgencyModel.AgencyCode);

				_log.Debug("MediaCode:[" + mediaAgencyModel.MediaCode + "]");
				_log.Debug("MediaCode:[" + mediaAgencyModel.RapCode + "]");
				_log.Debug("MediaCode:[" + mediaAgencyModel.AgencyCode + "]");

				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
                    //매체대행광고주 관계 Count조사///////////////////////////////////////////////
                    // __DEBUG__
                    _log.Debug(sbQueryClientCount.ToString());
                    // __DEBUG__

                    // 쿼리실행
                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds,sbQueryClientCount.ToString(),sqlParams);
                    
                    ClientCount =  Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					ds.Dispose();

                    _log.Debug("ClientCount          -->" + ClientCount);

                    // 이미 다른테이블에 사용중인 데이터가 있다면 Exception를 발생시킨다.
                    if(ClientCount > 0) throw new Exception();

                    OracleParameter[] sqlParams2 = new OracleParameter[3];
                    i = 0;
                    sqlParams2[i++] = new OracleParameter(":MediaCode", OracleDbType.Int32);
                    sqlParams2[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                    sqlParams2[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);

                    i = 0;
                    sqlParams2[i++].Value = Convert.ToInt32(mediaAgencyModel.MediaCode);
                    sqlParams2[i++].Value = Convert.ToInt32(mediaAgencyModel.RapCode);
                    sqlParams2[i++].Value = Convert.ToInt32(mediaAgencyModel.AgencyCode);
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams2);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("매체대행사정보삭제:[" + mediaAgencyModel.AgencyCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediaAgencyModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaAgencyDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				mediaAgencyModel.ResultCD   = "3301";
                // 이미 다른테이블에 사용중인 데이터가 있다면 
                if(ClientCount > 0 )
                {
                    mediaAgencyModel.ResultDesc = "등록된 매체대행광고주가 있으므로 매체대행사정보를 삭제할수 없습니다.";
                }
                else
                {
					_log.Exception(ex);
					mediaAgencyModel.ResultDesc = "매체대행사정보 삭제중 오류가 발생하였습니다";
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
