/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드	: [E_01]
 * 수정자		: YJ.Park
 * 수정일		: 2014.11.13
 * 수정내용	: 홈광고 편성 Count할때 홈광고(키즈) 추가
 * --------------------------------------------------------
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Net;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Schedule
{
    /// <summary>
    /// ChannelJumpBiz에 대한 요약 설명입니다.
    /// </summary>
    public class ChannelJumpBiz : BaseBiz
    {
        public ChannelJumpBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }


        /// <summary>
        /// 점핑광고목록조회
        /// </summary>
        /// <param name="channelJumpModel"></param>
        public void GetChannelJumpList(HeaderModel header, ChannelJumpModel channelJumpModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelJumpList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchKey           :[" + channelJumpModel.SearchKey            + "]");
				_log.Debug("SearchMediaCode     :[" + channelJumpModel.SearchMediaCode      + "]");
				_log.Debug("SearchRapCode       :[" + channelJumpModel.SearchRapCode        + "]");
				_log.Debug("SearchAdType        :[" + channelJumpModel.SearchAdType         + "]");
				_log.Debug("SearchJumpType      :[" + channelJumpModel.SearchJumpType       + "]");
				_log.Debug("SearchchkAdState_10 :[" + channelJumpModel.SearchchkAdState_10  + "]");
				_log.Debug("SearchchkAdState_20 :[" + channelJumpModel.SearchchkAdState_20  + "]");
				_log.Debug("SearchchkAdState_30 :[" + channelJumpModel.SearchchkAdState_30  + "]");
				_log.Debug("SearchchkAdState_40 :[" + channelJumpModel.SearchchkAdState_40  + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + "SELECT a.ITEM_NO AS ItemNo 	                \n"
                    + "      ,b.ITEM_NM AS ItemName	            \n"
                    + "      ,e.CNTR_NM AS ContractName            \n"
                    + "      ,b.BEGIN_DY AS ExcuteStartDay	        \n"
                    + "      ,b.END_DY AS ExcuteEndDay	        \n"
                    + "      ,b.RL_END_DY AS RealEndDay	            \n"
                    + "      ,a.JUMP_TYP AS JumpType                \n"
                    + "      ,g.STM_COD_NM AS JumpTypeName \n"
                    + "      ,a.GNR_COD AS GenreCode               \n"
                    + "      ,d.MENU_NM + '>' + c.MENU_NM AS GenreName   \n"
                    + "      ,a.CHNL_NO AS ChannelNo               \n"
                    + "      ,a.CNTS_LOC AS ContentID               \n"
					+ "      ,'' AS PopupID                 \n"
                    + "      ,a.CNTS_LOC AS ContentName \n"
					+ "      ,'' AS HomeYn                  \n"
					+ "      ,'' AS ChannelYn               \n"
                    + "      ,b.ADVT_STT AS AdState 	            \n"
                    + "      ,f.STM_COD_NM AdStateName    \n"
                    + "      ,b.ADVT_TYP AS AdType		                                \n"
                    + "      ,h.STM_COD_NM AdTypeName		                    \n"
                    + "      ,TO_CHAR(a.DT_UPDATE, 'YYYY-MM-DD') AS ModDt     \n"
                    + "      ,'' AS ChannelManager    \n"
					+ "      ,'' AS IsSTB			\n"
					+ "      ,'' AS STBInfo			\n"
					+ "FROM   ADVT_LINK a                    \n"
                    + "       INNER JOIN ADVT_MST b  ON (a.ITEM_NO         = b.ITEM_NO)                              \n"
                    + "       LEFT JOIN MENU_COD  c  ON (a.GNR_COD      = c.MENU_COD    AND c.MENU_LVL = 2    ) \n"
                    + "       LEFT JOIN MENU_COD  d  ON (c.MENU_COD_PAR  = d.MENU_COD    AND d.MENU_LVL = 1    ) \n"
                    + "       LEFT JOIN CNTR      e  ON (b.CNTR_SEQ    = e.CNTR_SEQ)                         \n"
                    + "       LEFT JOIN STM_COD   f  ON (b.ADVT_STT        = f.STM_COD        AND f.STM_COD_CLS   = '25' ) \n"  // 25:광고상태   
                    + "       LEFT JOIN STM_COD   g  ON (a.JUMP_TYP       = substr(g.STM_COD,-1,1)    AND g.STM_COD_CLS   = '34' ) \n"	// 34:점핑구분
                    + "       LEFT JOIN STM_COD   h  ON (b.ADVT_TYP       = h.STM_COD        AND h.STM_COD_CLS   = '26' ) \n"	// 26:광고구분
					+ " WHERE 1=1	                                        \n"
					);
				           
				if(!channelJumpModel.SearchAdType.Equals("00"))
				{
                    sbQuery.Append("  AND    b.ADVT_TYP = '" + channelJumpModel.SearchAdType + "'  \n");
				} 
				if(!channelJumpModel.SearchJumpType.Equals("00"))
				{
                    sbQuery.Append("  AND    a.JUMP_TYP = '" + Int32.Parse(channelJumpModel.SearchJumpType) + "'  \n");
				}

                bool isState = false;
                // 광고상태 선택에 따라

                // 광고상태 준비
                if(channelJumpModel.SearchchkAdState_10.Trim().Length > 0 && channelJumpModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append("  AND ( b.ADVT_STT  = '10' \n");
                    isState = true;
                }	
                // 광고상태 편성
                if(channelJumpModel.SearchchkAdState_20.Trim().Length > 0 && channelJumpModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append("  OR ");
                    else sbQuery.Append("  AND ( ");
                    sbQuery.Append(" b.ADVT_STT  = '20' \n");
                    isState = true;
                }	
                // 광고상태 중지
                if(channelJumpModel.SearchchkAdState_30.Trim().Length > 0 && channelJumpModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append("  OR ");
                    else sbQuery.Append("  AND ( ");
                    sbQuery.Append(" b.ADVT_STT  = '30' \n");
                    isState = true;
                }	
                // 광고상태 종료
                if(channelJumpModel.SearchchkAdState_40.Trim().Length > 0 && channelJumpModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if(isState) sbQuery.Append("  OR ");
                    else sbQuery.Append("  AND ( ");
                    sbQuery.Append(" b.ADVT_STT  = '40' \n");
                    isState = true;
                }	

                if(isState) sbQuery.Append(" ) \n");

                // 검색어가 있으면
                if (channelJumpModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( b.ITEM_NM LIKE '%" + channelJumpModel.SearchKey.Trim() + "%' 		\n"
                        + "		)        \n"
                        );
                }

				sbQuery.Append(""
                    + "ORDER BY A.ITEM_NO Desc \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 모델에 복사
                channelJumpModel.ChannelJumpDataSet = ds.Copy();
                // 결과
                channelJumpModel.ResultCnt = Utility.GetDatasetCount(channelJumpModel.ChannelJumpDataSet);
                // 결과코드 셋트
                channelJumpModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + channelJumpModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelJumpList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                channelJumpModel.ResultCD = "3000";
                channelJumpModel.ResultDesc = "점핑광고 목록 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 컨텐츠리스트팝업 URL가져오기
        /// </summary>
        /// <returns></returns>
        public string GetContentListPopUrl()
        {
            string rtnValue = "";
            try
            {   
                _db.Open();
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n" + "select top 1 URLSetAdPop    from	SystemConfig noLock");
    				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                if( ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
                {
                    rtnValue = ds.Tables[0].Rows[0]["URLSetAdPop"].ToString();
                }
                else
                {
                    rtnValue = "";
                }
            }
            catch(Exception ex)
            {
                rtnValue = "";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
            return rtnValue;
        }

        /// <summary>
        /// 채널점핑중 컨텐츠리스트정보가 등록되었는지 확인
        /// </summary>
        /// <param name="header"></param>
        /// <param name="channelJumpModel"></param>
        public void GetChannelJump(HeaderModel header, ChannelJumpModel channelJumpModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + " SELECT a.CNTS_LOC          \n"
                    + " FROM   ADVT_LINK a     \n"
                    + " INNER JOIN ADVT_MST b  ON (a.ITEM_NO = b.ITEM_NO) \n"
                    + " WHERE a.ITEM_NO =" + channelJumpModel.ItemNo);

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                if( ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
                {
                    channelJumpModel.PopupID    = ds.Tables[0].Rows[0]["PopUpId"].ToString();
                    channelJumpModel.ResultCnt  = ds.Tables[0].Rows.Count;
                    channelJumpModel.ResultCD   = "0000";
                    channelJumpModel.ResultDesc = "점핑광고 목록 조회중 오류가 발생하였습니다";
                }
                else
                {
                    channelJumpModel.PopupID    = "";
                    channelJumpModel.ResultCnt  = 0;
                    channelJumpModel.ResultCD   = "3000";
                    channelJumpModel.ResultDesc = "해당광고ID로 등록된 채널점핑 정보가 없습니다.";
                }
                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + channelJumpModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelJumpList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                channelJumpModel.ResultCD = "3000";
                channelJumpModel.ResultDesc = "점핑광고 목록 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


        /// <summary>
        /// 점핑등록
        /// </summary>
        /// <returns></returns>
        public void SetChannelJumpCreate(HeaderModel header, ChannelJumpModel channelJumpModel)
        {
			int NameCnt = 0;
			bool isAdPop = false;

            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                #region [파라메터 로그기록]
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelJumpCreate() Start");
                _log.Debug("-----------------------------------------");
				_log.Debug("ItemNo		:[" + channelJumpModel.ItemNo     + "]");
				_log.Debug("ItemName	:[" + channelJumpModel.ItemName   + "]");
				_log.Debug("MediaCode	:[" + channelJumpModel.MediaCode  + "]");
				_log.Debug("JumpType	:[" + channelJumpModel.JumpType   + "]");
				_log.Debug("GenreCode	:[" + channelJumpModel.GenreCode  + "]");
				_log.Debug("ChannelNo	:[" + channelJumpModel.ChannelNo  + "]");
				_log.Debug("ContentID	:[" + channelJumpModel.ContentID  + "]");

                #endregion

                #region 중복등록여부 검증하기
				// 동일광고로 점핑이 등록되어 있는지 검증한다
				StringBuilder sbQueryItemName = new StringBuilder();
                sbQueryItemName.Append(" \n");
                sbQueryItemName.Append(" SELECT COUNT(*) " + "\n");
                sbQueryItemName.Append(" FROM   ADVT_LINK   " + "\n");
                sbQueryItemName.Append(" WHERE  ITEM_NO    = " + channelJumpModel.ItemNo + " \n");
				_log.Debug(sbQueryItemName.ToString());
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQueryItemName.ToString());
				NameCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

				if(NameCnt > 0) throw new Exception("이미 채널점핑이 설정되어 있는 광고 입니다!!!");
                #endregion

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "INSERT INTO ADVT_LINK		\n"
                    + "           (ITEM_NO           ,JUMP_TYP		\n"
                    + "           ,CAT_COD     ,GNR_COD      ,CHNL_NO      \n"
                    + "           ,CNTS_LOC	   \n"
                    + "           ,DT_INSERT	   \n"
                    + "           ,ID_INSERT	   \n"
                    + "           ,DT_UPDATE	   \n"
                    + "           ,ID_UPDATE				)	\n"
					+ "   VALUES ( :ItemNo			\n"
					+ "           ,:JumpType		\n"
					+ "           ,:CategoryCode    \n"
                    + "           ,:GenreCode		\n"
					+ "           ,:ChannelNo		\n"
					+ "           ,:ContentID		\n"
                    + "           ,SYSDATE		\n"
                    + "           ,:UserID		\n"
					+ "           ,SYSDATE		\n"
                    + "           ,:UserID		    )      \n");
                try
                {
                    int i = 0;
                    int rc = 0;
                    //광고 그룹 Insert
                    OracleParameter[] sqlParams = new OracleParameter[7];

                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                    sqlParams[i++] = new OracleParameter(":JumpType", OracleDbType.Char, 1);
                    sqlParams[i++] = new OracleParameter(":CategoryCode", OracleDbType.Varchar2,10);
                    sqlParams[i++] = new OracleParameter(":GenreCode", OracleDbType.Varchar2, 10);
                    sqlParams[i++] = new OracleParameter(":ChannelNo", OracleDbType.Varchar2, 10);
                    sqlParams[i++] = new OracleParameter(":ContentID", OracleDbType.Varchar2, 300);
                    sqlParams[i++] = new OracleParameter(":UserID", OracleDbType.Varchar2, 10);

                                                                     
                    i = 0;
					sqlParams[i++].Value = Convert.ToInt32(channelJumpModel.ItemNo)   ;
					sqlParams[i++].Value = channelJumpModel.JumpType ;
                    sqlParams[i++].Value = "";   // 카테고리코드 현재 사용하지 않음

                    sqlParams[i++].Value = channelJumpModel.GenreCode;
                    sqlParams[i++].Value = channelJumpModel.ChannelNo;
					sqlParams[i++].Value = channelJumpModel.ContentID;

                    sqlParams[i++].Value = header.UserID;

                    _log.Debug(sbQuery.ToString());
					_db.BeginTran();
					
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                   
					_db.CommitTran();

					
					if(isAdPop) SetAdPop(channelJumpModel.PopupID);
                    _log.Message("채널점핑생성:["+ channelJumpModel.ItemNo + "]["+ channelJumpModel.ItemName + "] 등록자:[" + header.UserID + "]");
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                                                    
                channelJumpModel.ResultCD = "0000";  // 정상
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelJumpCreate() End");
                _log.Debug("-----------------------------------------");	
                                        
            }
            catch(Exception ex)
            {
                channelJumpModel.ResultCD   = "3101";

				if(NameCnt > 0)
				{
					channelJumpModel.ResultDesc = "해당광고는 이미 존재합니다.";
				}
				else
				{
					channelJumpModel.ResultDesc = "채널점핑정보 생성 중 오류가 발생하였습니다";
					_log.Exception(ex);
				}
            }
            finally
            {
                _db.Close();
            }
        }


        /// <summary>
        /// 점핑수정
        /// </summary>
        /// <param name="header"></param>
        /// <param name="channelJumpModel"></param>
        public void SetChannelJumpUpdate(HeaderModel header, ChannelJumpModel channelJumpModel)
        {
			bool isAdPop = false;
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelJumpUpdate() Start");
                _log.Debug("-----------------------------------------");
				_log.Debug("ItemNo     :[" + channelJumpModel.ItemNo     + "]");
                _log.Debug("ItemName	:[" + channelJumpModel.ItemName + "]");
				_log.Debug("MediaCode  :[" + channelJumpModel.MediaCode  + "]");
				_log.Debug("JumpType   :[" + channelJumpModel.JumpType   + "]");
				_log.Debug("GenreCode  :[" + channelJumpModel.GenreCode  + "]");
				_log.Debug("ChannelNo  :[" + channelJumpModel.ChannelNo  + "]");
				_log.Debug("ContentID  :[" + channelJumpModel.ContentID  + "]");

				// 광고내역을 업데이트한다.
				StringBuilder sbQuery = new StringBuilder();
                        
                int i = 0;
                int rc = 0;
                                        
                sbQuery.Append("\n"
                    + "UPDATE ADVT_LINK              \n"
                    + "   SET JUMP_TYP	 = :JumpType   \n"
                    + "      ,GNR_COD  = :GenreCode  \n"
                    + "      ,CHNL_NO  = :ChannelNo  \n"
                    + "      ,CNTS_LOC  = :ContentID  \n"
                    + "      ,ID_UPDATE  = :UserID  \n"
                    + "      ,DT_UPDATE      = SYSDATE   \n"
                    + " WHERE ITEM_NO     = :ItemNo     \n");

                try
                {
                    OracleParameter[] sqlParams = new OracleParameter[6];

					i = 0;
                    sqlParams[i++] = new OracleParameter(":JumpType", OracleDbType.Char, 1);
                    sqlParams[i++] = new OracleParameter(":GenreCode", OracleDbType.Varchar2, 10);
                    sqlParams[i++] = new OracleParameter(":ChannelNo", OracleDbType.Varchar2, 10);
                    sqlParams[i++] = new OracleParameter(":ContentID", OracleDbType.Varchar2,300);
                    sqlParams[i++] = new OracleParameter(":UserID", OracleDbType.Varchar2, 10);
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                                       
					i = 0;
					
					sqlParams[i++].Value = channelJumpModel.JumpType ;
                    sqlParams[i++].Value = channelJumpModel.GenreCode;
					sqlParams[i++].Value = channelJumpModel.ChannelNo;
                    sqlParams[i++].Value = channelJumpModel.ContentID;
                    sqlParams[i++].Value = header.UserID;
                    sqlParams[i++].Value = Convert.ToInt32(channelJumpModel.ItemNo);

					_db.BeginTran();
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

					if(isAdPop) SetAdPop(channelJumpModel.PopupID);
                        
                    //__MESSAGE__
                    _log.Message("채널점핑정보수정:["+channelJumpModel.ItemNo+ "]["+ channelJumpModel.ItemName + "] 등록자:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                channelJumpModel.ResultCD = "0000";  // 정상
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelJumpUpdate() End");
                _log.Debug("-----------------------------------------");
                        
            }
            catch(Exception ex)
            {
                channelJumpModel.ResultCD   = "3201";
				channelJumpModel.ResultDesc = "채널점핑정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


        public void SetChannelJumpDelete(HeaderModel header, ChannelJumpModel channelJumpModel)
        {
         
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelJumpDelete() Start");
                _log.Debug("-----------------------------------------");
				_log.Debug("ItemNo     :[" + channelJumpModel.ItemNo    + "]");
				_log.Debug("ItemName   :[" + channelJumpModel.ItemName    + "]");
				_log.Debug("MediaCode  :[" + channelJumpModel.MediaCode + "]");
                        
                StringBuilder sbQuery                       = new StringBuilder();
               
                       
                // 쿼리실행
                try
                {
                    int rc = 0;
                    int i = 0;

                        
					sbQuery.Append("\n"
                        + "DELETE ADVT_LINK             \n"
                        + " WHERE ITEM_NO    = :ItemNo     \n"
						);

                    OracleParameter[] sqlParams = new OracleParameter[1];
                    
                     
					i = 0;
                    sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                                                                                         
					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(channelJumpModel.ItemNo)   ;

         
                    _db.BeginTran();
          

                    //광고 계약 정보 삭제 한다.
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__
                        

                    _db.CommitTran();

					//__MESSAGE__
					_log.Message("채널점핑정보삭제:["+channelJumpModel.ItemNo+ "]["+ channelJumpModel.ItemName + "] 등록자:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                channelJumpModel.ResultCD = "0000";  // 정상
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelJumpDelete() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                channelJumpModel.ResultCD   = "3101";
				_log.Exception(ex);
				channelJumpModel.ResultDesc = "채널점핑 정보 삭제 중 오류가 발생하였습니다";
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
		/// <param name="channelJumpModel"></param>
		public void GetContractItemList(HeaderModel header,  ChannelJumpModel channelJumpModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() Start");
				_log.Debug("-----------------------------------------");
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + channelJumpModel.SearchKey       + "]");
				_log.Debug("SearchMediaCode     :[" + channelJumpModel.SearchMediaCode      + "]");
				_log.Debug("SearchRapCode       :[" + channelJumpModel.SearchRapCode        + "]");
				_log.Debug("SearchchkAdState_10 :[" + channelJumpModel.SearchchkAdState_10  + "]");
				_log.Debug("SearchchkAdState_20 :[" + channelJumpModel.SearchchkAdState_20  + "]");
				_log.Debug("SearchchkAdState_30 :[" + channelJumpModel.SearchchkAdState_30  + "]");
				_log.Debug("SearchchkAdState_40 :[" + channelJumpModel.SearchchkAdState_40  + "]");

               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
                    + "SELECT A.ITEM_NO AS ItemNo                      \n"
                    + "      ,A.ITEM_NM AS ItemName                    \n"
                    + "      ,B.CNTR_NM AS ContractName                \n"
                    + "      ,C.ADVTER_NM AS AdvertiserName              \n"
                    + "      ,A.BEGIN_DY AS ExcuteStartDay              \n"
                    + "      ,A.END_DY AS ExcuteEndDay                \n"
                    + "      ,A.RL_END_DY AS RealEndDay                  \n"
                    + "      ,A.ADVT_STT AS AdState                     \n"
                    + "      ,D.STM_COD_NM AS AdStateName        \n"
					+ "      ,'' HomeCount   \n"
                    + "      ,(SELECT COUNT(*) FROM ADVT_LINK WHERE ITEM_NO = A.ITEM_NO) AS JumpCount  \n"
                    + "      ,A.ADVT_TYP AS AdType                      \n"
                    + "      ,E.STM_COD_NM AS AdTypeName      \n"
                    + "  FROM ADVT_MST A INNER JOIN CNTR   B ON (A.CNTR_SEQ = B.CNTR_SEQ) \n"
                    + "                       LEFT JOIN ADVTER C ON (B.ADVTER_COD = C.ADVTER_COD)                \n"
                    + "	                      LEFT JOIN STM_COD D ON (A.ADVT_STT        = D.STM_COD      AND D.STM_COD_CLS = '25')  \n"  // 25 : 광고상태
                    + "                       LEFT JOIN STM_COD E ON (A.ADVT_TYP         = E.STM_COD      AND E.STM_COD_CLS = '26')  \n"	// 26 : 광고종류
                    + " WHERE A.ADVT_TYP between '01' and '89'  \n"    // 광고용도 AdClass 10:홈광고 30:복합
					);
     

				bool isState = false;
				// 광고상태 선택에 따라

				// 광고상태 준비
				if(channelJumpModel.SearchchkAdState_10.Trim().Length > 0 && channelJumpModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append(" AND ( A.ADVT_STT  = '10' \n");
					isState = true;
				}	
				// 광고상태 편성
				if(channelJumpModel.SearchchkAdState_20.Trim().Length > 0 && channelJumpModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.ADVT_STT  = '20' \n");
					isState = true;
				}	
				// 광고상태 중지
				if(channelJumpModel.SearchchkAdState_30.Trim().Length > 0 && channelJumpModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.ADVT_STT  = '30' \n");
					isState = true;
				}	
				// 광고상태 종료
				if(channelJumpModel.SearchchkAdState_40.Trim().Length > 0 && channelJumpModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" A.ADVT_STT  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				// 검색어가 있으면
				if (channelJumpModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("\n"
                        + "  AND ( a.ITEM_NM LIKE '%" + channelJumpModel.SearchKey.Trim() + "%' \n"
						+ "		)        \n"
						);
				}

                sbQuery.Append("ORDER BY ITEM_NO Desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고내역모델에 복사
				channelJumpModel.ContractItemDataSet = ds.Copy();
				// 결과
				channelJumpModel.ResultCnt = Utility.GetDatasetCount(channelJumpModel.ContractItemDataSet);
				// 결과코드 셋트
				channelJumpModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + channelJumpModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				channelJumpModel.ResultCD = "3000";
				channelJumpModel.ResultDesc = "광고내역정보 조회중 오류가 발생하였습니다";
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
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void GetChannelList(HeaderModel header,  ChannelJumpModel channelJumpModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList() Start");
				_log.Debug("-----------------------------------------");
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey           :[" + channelJumpModel.SearchKey       + "]");
				_log.Debug("SearchMediaCode     :[" + channelJumpModel.SearchMediaCode      + "]");
               
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
					+ " WHERE A.MediaCode = " + channelJumpModel.SearchMediaCode + " \n"
					);

				// 검색어가 있으면
				if (channelJumpModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("  AND ( B.Title  LIKE '%" + channelJumpModel.SearchKey.Trim() + "%') \n");
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
				channelJumpModel.ChannelListDataSet = ds.Copy();
				// 결과
				channelJumpModel.ResultCnt = Utility.GetDatasetCount(channelJumpModel.ChannelListDataSet);
				// 결과코드 셋트
				channelJumpModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + channelJumpModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				channelJumpModel.ResultCD = "3000";
				channelJumpModel.ResultDesc = "채널검색중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}



		/// <summary>
		///  컨텐츠검색
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void GetContentList(HeaderModel header,  ChannelJumpModel channelJumpModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCententList() Start");
				_log.Debug("-----------------------------------------");
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey           :[" + channelJumpModel.SearchKey       + "]");
				_log.Debug("SearchMediaCode     :[" + channelJumpModel.SearchMediaCode      + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT C.GenreCode                                     \n"
					+ "      ,D.CategoryName + '>' + E.GenreName AS GenreName \n"
					+ "      ,A.ContentID                                     \n"
					+ "      ,A.Title                                         \n"
					+ "  FROM Contents              A with(NoLock)            \n"
					+ "       INNER JOIN Channel    B with(NoLock) ON (A.ContentID    = B.ContentID) \n"
					+ "       INNER JOIN ChannelSet C with(NoLock) ON (B.ChannelNo    = C.ChannelNo     AND B.SeriesNo  = C.SeriesNo AND B.MediaCode = C.MediaCode) \n"
					+ "       INNER JOIN Category   D with(NoLock) ON (C.CategoryCode = D. CategoryCode AND C.MediaCode = D.MediaCode) \n"
					+ "       INNER JOIN Genre      E with(NoLock) ON (C.GenreCode    = E. GenreCode    AND C.MediaCode = E.MediaCode) \n"
					+ " WHERE B.MediaCode = " + channelJumpModel.SearchMediaCode + " \n"
					);

				// 검색어가 있으면
				if (channelJumpModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("  AND ( A.Title  LIKE '%" + channelJumpModel.SearchKey.Trim() + "%') \n");
				}

				sbQuery.Append(""
					+ " ORDER BY C.GenreCode, A.Title \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				channelJumpModel.ContentListDataSet = ds.Copy();
				// 결과
				channelJumpModel.ResultCnt = Utility.GetDatasetCount(channelJumpModel.ContentListDataSet);
				// 결과코드 셋트
				channelJumpModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + channelJumpModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCententList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				channelJumpModel.ResultCD = "3000";
				channelJumpModel.ResultDesc = "컨텐츠검색중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}



		/// <summary>
		///  팝업광고검색
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void GetAdPopList(HeaderModel header,  ChannelJumpModel channelJumpModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdPopList() Start");
				_log.Debug("-----------------------------------------");
				// __DEBUG__

				_log.Debug("Type :[" + channelJumpModel.Type  + "]");
				
				// 쿼리실행
				DataSet ds = new DataSet();


				// 쿼리생성
				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append("\n"
					+ " SELECT URLGetAdPopList   \n"
					+ "       ,URLSetAdPop       \n"
					+ "   FROM SystemConfig      \n"						
					);
					
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				string getURL = ds.Tables[0].Rows[0]["URLGetAdPopList"].ToString();
				
				getURL += channelJumpModel.Type;

				ds.Dispose();				
				
//				string  strXML = GetHTTPString("http://218.237.55.123:8080/adpop/getAdPopList.jsp");
				string  strXML = GetHTTPString(getURL);
				_log.Debug("URL : "+getURL);
				_log.Debug(strXML);     
				
				System.IO.StringReader xmlSR = new System.IO.StringReader(strXML);

				ds = new DataSet();
				ds.ReadXml(xmlSR, System.Data.XmlReadMode.Auto);

				// 결과
				if(ds.Tables.Count > 0)
				{
					// 결과 DataSet의 모델에 복사
					channelJumpModel.AdPopListDataSet = ds.Copy();
					channelJumpModel.ResultCnt = Utility.GetDatasetCount(channelJumpModel.AdPopListDataSet);
				}
				else
				{
					channelJumpModel.ResultCnt = 0;
				}
				// 결과코드 셋트
				channelJumpModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + channelJumpModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdPopList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				channelJumpModel.ResultCD = "3000";
				channelJumpModel.ResultDesc = "팝업공지리스트 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}



		/// <summary>
		///  팝업광설정
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void SetAdPop(string PopID)
		{

			// 쿼리실행
			DataSet ds = new DataSet();


			// 쿼리생성
			StringBuilder sbQuery = new StringBuilder();

			sbQuery.Append("\n"
				+ " SELECT URLGetAdPopList   \n"
				+ "       ,URLSetAdPop       \n"
				+ "   FROM SystemConfig      \n"						
				);
					
			// __DEBUG__
			_log.Debug(sbQuery.ToString());
			// __DEBUG__
				
			// 쿼리실행
			ds = new DataSet();
			_db.ExecuteQuery(ds,sbQuery.ToString());

			string setURL = ds.Tables[0].Rows[0]["URLSetAdPop"].ToString();

			setURL += PopID;

			ds.Dispose();

			string  strXML = GetHTTPString(setURL);

		}


		#region HTTP String Get / Set

		private string GetHTTPString(string url)
		{
			string  responseText  = "";
			try
			{				
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";
				request.Timeout = 15000;
				request.ProtocolVersion = HttpVersion.Version11;


				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream stream = response.GetResponseStream();
				StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8"));
				responseText  = reader.ReadToEnd();
			}
			catch (Exception ex) 
			{
				_log.Debug("Exception in getThumbnail. Url: " + url + ". Info: " + ex.Message + Environment.NewLine + "Stack: " + ex.StackTrace);
			}
			return responseText;
		}

		#endregion


    }
}