/*
 * -------------------------------------------------------
 * Class Name: TargetingAdDenyBiz
 * 주요기능  : 광고거부자관리 처리 로직
 * 작성자    : 김보배
 * 작성일    : 2013.12.16
 * 특이사항  : 
 * -------------------------------------------------------
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Target
{
    public class TargetingAdDenyBiz : BaseBiz
    {
        public TargetingAdDenyBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// 광고거부자관리 리스트 조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingAdDenyList(HeaderModel header, TargetingAdDenyModel targetingAdDenyModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("GetTargetingAdDenyList() Start");
                _log.Debug("-----------------------------------------");

                _db.Open();

                _log.Debug("<입력정보>");
                _log.Debug("SearchKey :[" + targetingAdDenyModel.KeySearch + "]");
                
                SqlParameter[] sqlParams = new SqlParameter[1];
                StringBuilder sbQuery = new StringBuilder();

                sqlParams[0] = new SqlParameter("@SearchKey", SqlDbType.VarChar);

                sqlParams[0].Value = targetingAdDenyModel.KeySearch;

                sbQuery.Append(" SELECT 'FALSE' AS CheckYn          \n");
                sbQuery.Append("       ,UserId                      \n");
                sbQuery.Append(" 	   ,StbId                       \n");
                sbQuery.Append("       ,CASE DenyCode  WHEN 'A' THEN '전체광고' WHEN 'B' THEN '상업광고' END DenyCode  \n");
                sbQuery.Append("       ,Comment                     \n");
                sbQuery.Append("       ,CASE UseYn WHEN 'Y' THEN 'O' WHEN 'N' THEN 'X' END UseYn           \n");
                sbQuery.Append("       ,RegDate                     \n");
                sbQuery.Append("       ,ModDate                     \n");
                sbQuery.Append("  	   ,RegId                       \n");
                sbQuery.Append("  FROM StbUserAdDeny WITH(NoLock)   \n");
                sbQuery.Append(" WHERE 1 = 1                        \n");
                if (targetingAdDenyModel.KeySearch.Trim().Length > 0)
                {
                    sbQuery.Append(" AND ( UserId LIKE '%' + @SearchKey + '%' OR StbId LIKE '%' + @SearchKey + '%' )      \n");
                }

                _log.Debug(sbQuery.ToString());

                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);
	
                targetingAdDenyModel.AdDenyDataset = ds.Copy();
                targetingAdDenyModel.ResultCnt = Utility.GetDatasetCount(targetingAdDenyModel.AdDenyDataset);
                targetingAdDenyModel.ResultCD = "0000";

                ds.Dispose();

                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetingAdDenyModel.ResultCnt + "]");

                _log.Debug("-----------------------------------------");
                _log.Debug("GetTargetingAdDenyList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingAdDenyModel.ResultCD = "3000";
                targetingAdDenyModel.ResultDesc = "광고거부자관리 리스트 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 팝업창 리스트 조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="targetingAdDenyModel"></param>
        public void GetPopupList(HeaderModel header, TargetingAdDenyModel targetingAdDenyModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("GetPopupList() Start");
                _log.Debug("-----------------------------------------");

                _db.Open();

                _log.Debug("<입력정보>");
                _log.Debug("SearchKey : [" + targetingAdDenyModel.KeyPopupSearch + "]");

                SqlParameter[] sqlParams = new SqlParameter[1];
                StringBuilder sbQuery = new StringBuilder();

                sqlParams[0] = new SqlParameter("@KeyPopupSearch", SqlDbType.VarChar);

                sqlParams[0].Value = targetingAdDenyModel.KeyPopupSearch;
                
                sbQuery.Append(" SELECT UserId                                                       \n");
                sbQuery.Append(" 	   ,StbId	                                                     \n");
                sbQuery.Append("       ,PostNo	                                                     \n");
                sbQuery.Append("       ,ServiceCode	                                                 \n");
                sbQuery.Append("       ,ResidentNo		                                             \n");
                sbQuery.Append("       ,Sex	                                                         \n");
                sbQuery.Append("       ,CASE Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N  \n");
                sbQuery.Append("    FROM StbUser with(noLock)                                        \n");
                sbQuery.Append("  WHERE  1 = 1	                                                     \n");
                if(targetingAdDenyModel.KeyPopupSearch.Trim().Length > 0)
                {
                    sbQuery.Append(" AND (UserId LIKE '%' + @KeyPopupSearch + '%' OR StbId LIKE '%' + @KeyPopupSearch + '%' )      \n");
                }
                sbQuery.Append(" ORDER BY ServiceNum, PostNo, ResidentNo, Sex                        \n");
                
                _log.Debug(sbQuery.ToString());

                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                targetingAdDenyModel.AdDenyDataset = ds.Copy();
                targetingAdDenyModel.ResultCnt = Utility.GetDatasetCount(targetingAdDenyModel.AdDenyDataset);
                targetingAdDenyModel.ResultCD = "0000";

                ds.Dispose();

                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetingAdDenyModel.ResultCnt + "]");

                _log.Debug("-----------------------------------------");
                _log.Debug("GetPopupList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingAdDenyModel.ResultCD = "3000";
                targetingAdDenyModel.ResultDesc = "광고거부자관리 리스트 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
        
        /// <summary>
        /// 광고거부자관리 추가
        /// </summary>
        /// <param name="header"></param>
        /// <param name="gradeModel"></param>
        public void SetTargetingAdDenyCreate(HeaderModel header, TargetingAdDenyModel targetingAdDenyModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("SetTargetingAdDenyCreate() Start");
                _log.Debug("-----------------------------------------");

                _db.Open();

                _log.Debug("<입력정보>");
                _log.Debug("UserID   : [" + targetingAdDenyModel.UserID + "]");
                _log.Debug("StbId    : [" + targetingAdDenyModel.StbId + "]");
                _log.Debug("DenyCode : [" + targetingAdDenyModel.DenyCode + "]");
                _log.Debug("Comment  : [" + targetingAdDenyModel.Comment + "]");
                _log.Debug("UseYn    : [" + targetingAdDenyModel.UseYn + "]");
                _log.Debug("RegID    : [" + header.UserID + "]");

                SqlParameter[] sqlParams = new SqlParameter[6];
                StringBuilder sbQuery = new StringBuilder();

                sqlParams[0] = new SqlParameter("@UserId", SqlDbType.Int);
                sqlParams[1] = new SqlParameter("@StbId", SqlDbType.Char);
                sqlParams[2] = new SqlParameter("@DenyCode", SqlDbType.VarChar, 1);
                sqlParams[3] = new SqlParameter("@Comment", SqlDbType.VarChar, 1024);
                sqlParams[4] = new SqlParameter("@UseYn", SqlDbType.Char, 1);
                sqlParams[5] = new SqlParameter("@RegID", SqlDbType.VarChar, 10);

                sqlParams[0].Value = targetingAdDenyModel.UserID;
                sqlParams[1].Value = targetingAdDenyModel.StbId;
                sqlParams[2].Value = targetingAdDenyModel.DenyCode;
                sqlParams[3].Value = targetingAdDenyModel.Comment;
                sqlParams[4].Value = targetingAdDenyModel.UseYn;
                sqlParams[5].Value = header.UserID;				// 등록자		

                sbQuery.Append(" INSERT INTO StbUserAdDeny (   \n");
                sbQuery.Append("   	         UserId            \n");
                sbQuery.Append("            ,StbId             \n");
                sbQuery.Append("            ,DenyCode          \n");
                sbQuery.Append("            ,Comment           \n");
                sbQuery.Append("            ,UseYn             \n");
                sbQuery.Append("            ,RegDate           \n");
                sbQuery.Append("            ,RegId         )   \n");
                sbQuery.Append("  VALUES                   (   \n");
                sbQuery.Append("            @UserId            \n");
                sbQuery.Append("           ,@StbId             \n");
                sbQuery.Append("           ,@DenyCode          \n");
                sbQuery.Append("           ,@Comment           \n");
                sbQuery.Append("           ,@UseYn             \n");
                sbQuery.Append("           ,GETDATE()          \n");
                sbQuery.Append("           ,@RegId         )   \n");

                _log.Debug(sbQuery.ToString());

                try
                {
                    _db.BeginTran();
                    _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                targetingAdDenyModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug("SetTargetingAdDenyCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingAdDenyModel.ResultCD = "3101";
                targetingAdDenyModel.ResultDesc = "광고거부자관리 리스트 추가 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 광고거부자관리 저장
        /// </summary>
        /// <param name="header"></param>
        /// <param name="gradeModel"></param>
        public void SetTargetingAdDenyUpdate(HeaderModel header, TargetingAdDenyModel targetingAdDenyModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("SetTargetingAdDenyUpdate() Start");
                _log.Debug("-----------------------------------------");

                _db.Open();

                _log.Debug("<입력정보>");
                _log.Debug("DenyCode : [" + targetingAdDenyModel.DenyCode + "]");
                _log.Debug("Comment  : [" + targetingAdDenyModel.Comment + "]");
                _log.Debug("UseYn    : [" + targetingAdDenyModel.UseYn + "]");
                _log.Debug("UserID   : [" + targetingAdDenyModel.UserID + "]");
                _log.Debug("StbId    : [" + targetingAdDenyModel.StbId + "]");

                SqlParameter[] sqlParams = new SqlParameter[5];
                StringBuilder sbQuery = new StringBuilder();

                sqlParams[0] = new SqlParameter("@DenyCode", SqlDbType.VarChar, 1);
                sqlParams[1] = new SqlParameter("@Comment", SqlDbType.VarChar, 1024);
                sqlParams[2] = new SqlParameter("@UseYn", SqlDbType.Char, 1);
                sqlParams[3] = new SqlParameter("@UserId", SqlDbType.Int);
                sqlParams[4] = new SqlParameter("@StbId", SqlDbType.Char);

                sqlParams[0].Value = targetingAdDenyModel.DenyCode;
                sqlParams[1].Value = targetingAdDenyModel.Comment;
                sqlParams[2].Value = targetingAdDenyModel.UseYn;
                sqlParams[3].Value = targetingAdDenyModel.UserID;
                sqlParams[4].Value = targetingAdDenyModel.StbId;

                sbQuery.Append(" UPDATE StbUserAdDeny         \n");
                sbQuery.Append("    SET DenyCode = @DenyCode  \n");
                sbQuery.Append("       ,Comment  = @Comment   \n");
                sbQuery.Append("       ,UseYn    = @UseYn     \n");
                sbQuery.Append("       ,ModDate  = GETDATE()  \n");
                sbQuery.Append("  WHERE UserId   = @UserId    \n");
                sbQuery.Append("	AND StbId    = @StbId     \n");

                _log.Debug(sbQuery.ToString());

                try
                {
                    _db.BeginTran();
                    _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                targetingAdDenyModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug("SetTargetingAdDenyUpdate() End");
                _log.Debug("-----------------------------------------");

            }
            catch (Exception ex)
            {
                targetingAdDenyModel.ResultCD = "3101";
                targetingAdDenyModel.ResultDesc = "광고거부자관리 리스트 저장 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 광고거부자관리 삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="gradeModel"></param>
        public void SetTargetingAdDenyDelete(HeaderModel header, TargetingAdDenyModel targetingAdDenyModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("SetTargetingAdDenyDelete() Start");
                _log.Debug("-----------------------------------------");

                _db.Open();

                _log.Debug("<입력정보>");
                _log.Debug("UserID   : [" + targetingAdDenyModel.UserID + "]");
                _log.Debug("StbId    : [" + targetingAdDenyModel.StbId + "]");

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[2];

                sqlParams[0] = new SqlParameter("@UserId", SqlDbType.Int);
                sqlParams[1] = new SqlParameter("@StbId", SqlDbType.Char);

                sqlParams[0].Value = targetingAdDenyModel.UserID;
                sqlParams[1].Value = targetingAdDenyModel.StbId;

                sbQuery.Append(" DELETE StbUserAdDeny          \n");
                sbQuery.Append("  WHERE UserId = @UserId       \n");
                sbQuery.Append("    AND StbId = @StbId         \n");

                _log.Debug(sbQuery.ToString());

                try
                {
                    _db.BeginTran();
                    _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                targetingAdDenyModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug("SetTargetingAdDenyDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingAdDenyModel.ResultCD = "3301";
                targetingAdDenyModel.ResultDesc = "광고거부자관리 리스트 삭제중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }  

    }
}