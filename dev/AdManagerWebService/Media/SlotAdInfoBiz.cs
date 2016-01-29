using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;
using Oracle.DataAccess.Client;

namespace AdManagerWebService.Media
{
    /// <summary>
    /// SlotAdInfoBiz에 대한 요약 설명입니다.
    /// </summary>
    public class SlotAdInfoBiz : BaseBiz
    {
        public SlotAdInfoBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }

		#region 메뉴 목록조회
		/// <summary>
		/// 메뉴 목록조회
		/// </summary>
		/// <param name="slotAdInfoModel"></param>
		/// 
		public void GetMenuList(HeaderModel header, SlotAdInfoModel slotAdInfoModel)
		{
			try
			{
                // 데이터베이스를 OPEN한다
                _db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode      :[" + slotAdInfoModel.SearchMediaCode + "]");
                _log.Debug("IsSetDataOnly        :[" + slotAdInfoModel.IsSetDataOnly + "]");
                // __DEBUG__

                bool IsSetDataOnly = slotAdInfoModel.IsSetDataOnly;
				StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                //sbQuery.Append(" SELECT  	                                        \n");
                //sbQuery.Append("         B.CategoryCode	AS CategoryCode      	    \n");
                //sbQuery.Append("  		 ,B.CategoryName AS CategoryName	    	\n");
                //sbQuery.Append("         ,B.GenreCode AS MenuCode	    	        \n");
                //sbQuery.Append("         ,B.GenreName AS MenuName         	        \n");
                //sbQuery.Append("         ,ISNULL(A.MaxCount,'0') AS MaxCount     	\n");
                //sbQuery.Append("         ,ISNULL(A.MaxTime,'0') AS MaxTime     	    \n");
                //sbQuery.Append("         ,ISNULL(A.MaxCountPay,'0') AS MaxCountPay  \n");
                //sbQuery.Append("         ,ISNULL(A.MaxTimePay,'0') AS MaxTimePay     	 \n");
                //sbQuery.Append("         ,CONVERT(CHAR(19), A.UseDate, 120) AS UseDate     	 \n");
                //sbQuery.Append("   FROM SlotAdTypeAssign A     	 \n");
                ////값이 세팅된 메뉴만 가져올것인지 모든 메뉴 목록을 불러 올것인지....
                //if(IsSetDataOnly)
                //    sbQuery.Append(" 		INNER JOIN vMenuList    B with(NoLock)      \n");
                //else
                //    sbQuery.Append("  		RIGHT OUTER JOIN vMenuList    B with(NoLock) \n");
                //sbQuery.Append("  		       ON (A.Menu1    = B.CategoryCode   AND A.Menu2    = B.GenreCode )        	 \n");
                //sbQuery.Append("   WHERE  B.MediaCode ='" + slotAdInfoModel.SearchMediaCode.Trim() + "'        	         \n");
                //sbQuery.Append("   ORDER BY B.CategoryCode, B.GenreCode     \n");

                sbQuery.AppendLine();                                                                                                                                                
                sbQuery.AppendLine("    SELECT  B.CATEGORYCODE  AS CategoryCode                                     ");
	            sbQuery.AppendLine("        ,   B.CATEGORYNAME  AS CategoryName	                                    ");
	            sbQuery.AppendLine("        ,   B.GENRECODE     AS MenuCode	    	                                ");
	            sbQuery.AppendLine("        ,   B.GENRENAME     AS MenuName         	                            ");
	            sbQuery.AppendLine("        ,   NVL(A.ADVT_MAX_CNT,'0')     AS MaxCount                             ");
	            sbQuery.AppendLine("        ,   NVL(A.ADVT_MAX_TM,'0')      AS MaxTime     	                        ");
	            sbQuery.AppendLine("        ,   NVL(A.ADVT_MAX_PAY_CNT,'0') AS MaxCountPay                          ");
	            sbQuery.AppendLine("        ,   NVL(A.ADVT_MAX_PAY_TM,'0')  AS MaxTimePay     	                    ");
                sbQuery.AppendLine("        ,   A.ADVT_PROM_YN              AS PromotionYn     	                    ");
	            sbQuery.AppendLine("        ,   A.USE_YN                    AS UseYn     	                        ");
                sbQuery.AppendLine("    FROM ADVT_SLOT A     	                                                    ");
                if (IsSetDataOnly)
                    sbQuery.Append(" 		INNER JOIN VMENULIST B ");
                else
                    sbQuery.Append("  		RIGHT OUTER JOIN VMENULIST B ");
                sbQuery.AppendLine("ON (A.MENU_COD = B.GENRECODE AND A.MENU_COD_PAR = B.CATEGORYCODE )              ");
                
                sbQuery.AppendLine("    WHERE  B.MEDIACODE = 1                                                      ");
                sbQuery.AppendLine("    ORDER BY B.CATEGORYCODE, B.GENRECODE                                        ");

			
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 슬롯정보모델에 복사
				slotAdInfoModel.SlotAdInfoDataSet = ds.Copy();
				// 결과
				slotAdInfoModel.ResultCnt = Utility.GetDatasetCount(slotAdInfoModel.SlotAdInfoDataSet);
				// 결과코드 셋트
				slotAdInfoModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + slotAdInfoModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() End");
				_log.Debug("-----------------------------------------");


			}
			catch(Exception ex)
			{
				slotAdInfoModel.ResultCD = "3000";
				slotAdInfoModel.ResultDesc = "메뉴정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
		}

        #endregion

        #region 광고 슬롯 정보 수정
        /// <summary>
        /// 광고 슬롯 정보 수정
        /// </summary>
        /// <param name="header"></param>
        /// <param name="slotAdInfoModel"></param>
        public void UpdateSlotAdTypeAssign(HeaderModel header, SlotAdInfoModel slotAdInfoModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "UpdateSlotAdTypeAssign() Start");
                _log.Debug("-----------------------------------------");

                //DB OPEN
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MediaCode      :[" + slotAdInfoModel.MediaCode + "]");
                _log.Debug("CategoryCode   :[" + slotAdInfoModel.CategoryCode + "]");
                _log.Debug("MenuCode       :[" + slotAdInfoModel.MenuCode + "]");
                _log.Debug("MaxCount       :[" + slotAdInfoModel.MaxCount + "]");
                _log.Debug("MaxTime        :[" + slotAdInfoModel.MaxTime + "]");
                _log.Debug("MaxCountPay    :[" + slotAdInfoModel.MaxCountPay + "]");
                _log.Debug("MaxTimePay     :[" + slotAdInfoModel.MaxTimePay + "]");
                _log.Debug("UseDate        :[" + slotAdInfoModel.UseDate + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                OracleParameter[] sqlParams = new OracleParameter[8];

                int rc = 0;

                //sbQuery.Append("\n  UPDATE SlotAdTypeAssign                ");
                //sbQuery.Append("\n  SET    MaxCount         = @MaxCount    ");
                //sbQuery.Append("\n         ,MaxTime         = @MaxTime     ");
                //sbQuery.Append("\n         ,MaxCountPay     = @MaxCountPay ");
                //sbQuery.Append("\n         ,MaxTimePay      = @MaxTimePay  ");
                //sbQuery.Append("\n         ,UseDate         = @UseDate     ");
                //sbQuery.Append("\n         ,ModDate         = getdate()    ");
                //sbQuery.Append("\n WHERE    ");
                //sbQuery.Append("\n      Menu1    = @CategoryCode"    );
                //sbQuery.Append("\n    AND Menu2        = @MenuCode"  );

                sbQuery.AppendLine();
                sbQuery.AppendLine("    UPDATE ADVT_SLOT                            ");
                sbQuery.AppendLine("    SET     ADVT_MAX_CNT     = :MaxCount        ");
                sbQuery.AppendLine("        ,   ADVT_MAX_TM      = :MaxTime         ");
                sbQuery.AppendLine("        ,   ADVT_MAX_PAY_CNT = :MaxCountPay     ");
                sbQuery.AppendLine("        ,   ADVT_MAX_PAY_TM  = :MaxTimePay      ");
                sbQuery.AppendLine("        ,   ADVT_PROM_YN     = :PromotionYn     ");
                sbQuery.AppendLine("        ,   USE_YN           = :UseYn           ");
                sbQuery.AppendLine("    WHERE   MENU_COD_PAR     = :CategoryCode    ");
                sbQuery.AppendLine("        AND MENU_COD         = :MenuCode        ");

                int i = 0;
                sqlParams[i++] = new OracleParameter(":MaxCount", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":MaxTime", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":MaxCountPay", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":MaxTimePay", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":PromotionYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":CategoryCode", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 10);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(slotAdInfoModel.MaxCount);
                sqlParams[i++].Value = Convert.ToInt32(slotAdInfoModel.MaxTime);
                sqlParams[i++].Value = Convert.ToInt32(slotAdInfoModel.MaxCountPay);
                sqlParams[i++].Value = Convert.ToInt32(slotAdInfoModel.MaxTimePay);
                sqlParams[i++].Value = slotAdInfoModel.PromotionYn;
                sqlParams[i++].Value = slotAdInfoModel.UseYn;
                sqlParams[i++].Value = slotAdInfoModel.CategoryCode;
                sqlParams[i++].Value = slotAdInfoModel.MenuCode;

                i = 0;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __DEBUG__
                    _log.Message("광고 슬롯 정보 수정:[" + slotAdInfoModel.CategoryCode + "]");
                    // __DEBUG__
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                slotAdInfoModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "UpdateSlotAdTypeAssign() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                slotAdInfoModel.ResultCD = "3201";
                slotAdInfoModel.ResultDesc = "광고 슬롯 정보 수정 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }
        #endregion

        #region 광고 슬롯 정보 생성
        /// <summary>
        /// 광고 슬롯 정보 생성
        /// </summary>
        /// <param name="header"></param>
        /// <param name="slotAdInfoModel"></param>
        public void InsertSlotAdTypeAssign(HeaderModel header, SlotAdInfoModel slotAdInfoModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "InsertSlotAdTypeAssign() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("CategoryCode   :[" + slotAdInfoModel.CategoryCode + "]");
                _log.Debug("MenuCode       :[" + slotAdInfoModel.MenuCode + "]");
                _log.Debug("MaxCount       :[" + slotAdInfoModel.MaxCount + "]");
                _log.Debug("MaxTime        :[" + slotAdInfoModel.MaxTime + "]");
                _log.Debug("MaxCountPay    :[" + slotAdInfoModel.MaxCountPay + "]");
                _log.Debug("MaxTimePay     :[" + slotAdInfoModel.MaxTimePay + "]");
                _log.Debug("UseDate        :[" + slotAdInfoModel.UseDate + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                OracleParameter[] sqlParams = new OracleParameter[8];

                int rc = 0;

                //sbQuery.Append("\n  INSERT INTO SlotAdTypeAssign (                      ");
                //sbQuery.Append("\n          CugCode                       ");
                //sbQuery.Append("\n         ,Menu1                        ");
                //sbQuery.Append("\n         ,Menu2                        ");
                //sbQuery.Append("\n         ,Menu3                        ");
                //sbQuery.Append("\n         ,Menu4                        ");
                //sbQuery.Append("\n         ,ChannelNo                   ");
                //sbQuery.Append("\n         ,MaxCount                       ");
                //sbQuery.Append("\n         ,MaxTime                           ");
                //sbQuery.Append("\n         ,MaxCountPay                       ");
                //sbQuery.Append("\n         ,MaxTimePay                           ");
                //sbQuery.Append("\n         ,UsePromotionAd                       ");
                //sbQuery.Append("\n         ,UseDate                        ");
                //sbQuery.Append("\n         ,ModDate                        ");
                //sbQuery.Append("\n         ,RegDate                        ");
                //sbQuery.Append("\n      )                                   ");
                //sbQuery.Append("\n      Values (                              ");
                //sbQuery.Append("\n         1                       ");
                //sbQuery.Append("\n         ,@CategoryCode       ");
                //sbQuery.Append("\n         ,@MenuCode                       ");
                //sbQuery.Append("\n         ,0                  ");
                //sbQuery.Append("\n         ,0                             ");
                //sbQuery.Append("\n         ,0                   ");
                //sbQuery.Append("\n         ,@MaxCount                       ");
                //sbQuery.Append("\n         ,@MaxTime                           ");
                //sbQuery.Append("\n         ,@MaxCountPay                       ");
                //sbQuery.Append("\n         ,@MaxTimePay                           ");
                //sbQuery.Append("\n         ,'Y'                       ");
                //sbQuery.Append("\n         ,@UseDate                        ");
                //sbQuery.Append("\n         ,getdate()                        ");
                //sbQuery.Append("\n         ,getdate()                        ");
                //sbQuery.Append("\n      )		                    ");
                sbQuery.AppendLine();
                sbQuery.AppendLine("    INSERT INTO ADVT_SLOT (     MDA_CD                  ");
                sbQuery.AppendLine("                            ,   MENU_COD                ");
                sbQuery.AppendLine("                            ,   MENU_COD_PAR            ");
                sbQuery.AppendLine("                            ,   TITLE_NO                ");
                sbQuery.AppendLine("                            ,   ADVT_MAX_CNT            ");
                sbQuery.AppendLine("                            ,   ADVT_MAX_TM             ");
                sbQuery.AppendLine("                            ,   ADVT_MAX_PAY_CNT        ");
                sbQuery.AppendLine("                            ,   ADVT_MAX_PAY_TM         ");
                sbQuery.AppendLine("                            ,   ADVT_PROM_YN            ");
                sbQuery.AppendLine("                            ,   USE_YN )                ");
                sbQuery.AppendLine("                  VALUES (      1                       ");
                sbQuery.AppendLine("                            ,   :CategoryCode           ");
                sbQuery.AppendLine("                            ,   :MenuCode               ");
                sbQuery.AppendLine("                            ,   '0000000000'            ");
                sbQuery.AppendLine("                            ,   :MaxCount               ");
                sbQuery.AppendLine("                            ,   :MaxTime                ");
                sbQuery.AppendLine("                            ,   :MaxCountPay            ");
                sbQuery.AppendLine("                            ,   :MaxTimePay             ");
                sbQuery.AppendLine("                            ,   :PromotionYn            ");
                sbQuery.AppendLine("                            ,   :UseYn)                 ");

                int i = 0;
                sqlParams[i++] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":CategoryCode", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":MaxCount", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":MaxTime", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":MaxCountPay", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":MaxTimePay", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":PromotionYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);                

                i = 0;
                sqlParams[i++].Value = slotAdInfoModel.MenuCode;
                sqlParams[i++].Value = slotAdInfoModel.CategoryCode;
                sqlParams[i++].Value = Convert.ToInt32(slotAdInfoModel.MaxCount);
                sqlParams[i++].Value = Convert.ToInt32(slotAdInfoModel.MaxTime);
                sqlParams[i++].Value = Convert.ToInt32(slotAdInfoModel.MaxCountPay);
                sqlParams[i++].Value = Convert.ToInt32(slotAdInfoModel.MaxTimePay);
                sqlParams[i++].Value = slotAdInfoModel.PromotionYn;
                sqlParams[i++].Value = slotAdInfoModel.UseYn;

                i = 0;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("광고 슬롯 정보 생성:[" + slotAdInfoModel.CategoryCode + "-" + slotAdInfoModel.MenuCode + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                slotAdInfoModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "InsertSlotAdTypeAssign() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                slotAdInfoModel.ResultCD = "3101";
                slotAdInfoModel.ResultDesc = "광고 슬롯 정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }
        #endregion

        #region 광고 슬롯 정보 삭제
        /// <summary>
        /// 광고 슬롯 정보 삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="slotAdInfoModel"></param>
        public void DeleteSlotAdTypeAssign(HeaderModel header, SlotAdInfoModel slotAdInfoModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "DeleteSlotAdTypeAssign() Start");
                _log.Debug("-----------------------------------------");

                //DB OPEN
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MenuCode      :[" + slotAdInfoModel.MenuCode + "]");
                _log.Debug("CategoryCode   :[" + slotAdInfoModel.CategoryCode + "]");
               // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                OracleParameter[] sqlParams = new OracleParameter[2];

                int rc = 0;

                sbQuery.AppendLine("    DELETE FROM ADVT_SLOT                   ");
                sbQuery.AppendLine("    WHERE   MDA_CD       = 1                ");
                sbQuery.AppendLine("        AND MENU_COD     = :MenuCode        ");
                sbQuery.AppendLine("        AND MENU_COD_PAR = :CategoryCode    ");

                int i = 0;
                sqlParams[i++] = new OracleParameter(":MenuCode", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":CategoryCode", OracleDbType.Varchar2, 10);

                i = 0;
                sqlParams[i++].Value = slotAdInfoModel.MenuCode;
                sqlParams[i++].Value = slotAdInfoModel.CategoryCode;

                i = 0;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __DEBUG__
                    _log.Message("광고 슬롯 정보 삭제:[" + slotAdInfoModel.CategoryCode + "]");
                    // __DEBUG__
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                slotAdInfoModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "DeleteSlotAdTypeAssign() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                slotAdInfoModel.ResultCD = "3201";
                slotAdInfoModel.ResultDesc = "광고 슬롯 정보 삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }
        #endregion

        #region 광고 슬롯 정보 기본 값 조회
        /// <summary>
        /// 광고 슬롯 정보 기본 값 조회
        /// </summary>
        /// <param name="slotAdInfoModel"></param>
        /// 
        public void GetDefaultSlotAdInfo(HeaderModel header, SlotAdInfoModel slotAdInfoModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDefaultSlotAdInfo Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode      :[" + slotAdInfoModel.SearchMediaCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                
                // 쿼리생성
                //sbQuery.Append("\n"
                //                   + " SELECT  Top 1  \n"
                //                   + "        ISNULL(A.MaxCount,'0') AS MaxCount     	  \n"
                //                   + "        ,ISNULL(A.MaxTime,'0') AS MaxTime     	  \n"
                //                   + "        ,ISNULL(A.MaxCountPay,'0') AS MaxCountPay     	  \n"
                //                   + "        ,ISNULL(A.MaxTimePay,'0') AS MaxTimePay     	  \n"
                //                   + "        ,CONVERT(CHAR(19), A.UseDate, 120) AS UseDate     	  \n"
                //                   + "  FROM SlotAdTypeAssign A     	  \n"
                //                   + "  WHERE  CugCode = 0 \n"
                //                   + "       AND Menu1 = 0   \n"
                //                   + "       AND Menu2 = 0   \n"
                //                   + "       AND Menu3 = 0   \n"
                //                   + "       AND Menu4 = 0   \n"
                //                   + "       AND ChannelNo = 0   \n"
                //              );
                sbQuery.AppendLine();
                sbQuery.AppendLine("    SELECT  NVL(A.ADVT_MAX_CNT,'0')     AS MaxCount     	");
                sbQuery.AppendLine("        ,   NVL(A.ADVT_MAX_TM,'0')      AS MaxTime     	    ");
                sbQuery.AppendLine("        ,   NVL(A.ADVT_MAX_PAY_CNT,'0') AS MaxCountPay      ");
                sbQuery.AppendLine("        ,   NVL(A.ADVT_MAX_PAY_TM,'0')  AS MaxTimePay       ");
                sbQuery.AppendLine("        ,   A.ADVT_PROM_YN              AS PromotionYn      ");
                sbQuery.AppendLine("        ,   A.USE_YN                    AS UseYn            ");
                sbQuery.AppendLine("    FROM ADVT_SLOT A     	                                ");
                sbQuery.AppendLine("    WHERE   ROWNUM <= 1                                     ");
                sbQuery.AppendLine("        AND MDA_CD = 0                                      ");
                sbQuery.AppendLine("        AND MENU_COD = 0                                    ");
                sbQuery.AppendLine("        AND MENU_COD_PAR = 0                                ");
                sbQuery.AppendLine("        AND TITLE_NO = 0                                    ");

                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 쿼리실행
                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception();
                }

                int MaxCount = Convert.ToInt32(ds.Tables[0].Rows[0]["MaxCount"].ToString());
                int MaxTime = Convert.ToInt32(ds.Tables[0].Rows[0]["MaxTime"].ToString());
                int MaxCountPay = Convert.ToInt32(ds.Tables[0].Rows[0]["MaxCountPay"].ToString());
                int MaxTimePay = Convert.ToInt32(ds.Tables[0].Rows[0]["MaxTimePay"].ToString());

                //결과를 슬롯 정보 모델에 세팅
                slotAdInfoModel.MaxCount = MaxCount;
                slotAdInfoModel.MaxTime = MaxTime;
                slotAdInfoModel.MaxCountPay = MaxCountPay;
                slotAdInfoModel.MaxTimePay = MaxTimePay;
                slotAdInfoModel.PromotionYn = ds.Tables[0].Rows[0]["PromotionYn"].ToString();
                slotAdInfoModel.UseYn = ds.Tables[0].Rows[0]["UseYn"].ToString();

                ds.Dispose();
                
                // 결과코드 셋트
                slotAdInfoModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCD:[" + slotAdInfoModel.ResultCD + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDefaultSlotAdInfo() End");
                _log.Debug("-----------------------------------------");


            }
            catch (Exception ex)
            {
                slotAdInfoModel.ResultCD = "3000";
                slotAdInfoModel.ResultDesc = "광고 슬롯 정보 기본 값 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion
        
    }
}