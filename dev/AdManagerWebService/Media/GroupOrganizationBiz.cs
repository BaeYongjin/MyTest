/*
 * -------------------------------------------------------
 * Class Name: GroupOrganizationBiz.cs
 * 주요기능  : OAP편성그룹관리 서비스
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.05.21
 * 수정내용  :        
 *            - 홈광고편성편황조회시 셋탑종류별 조회 수정
 * 수정함수  :
 *            - GetSchHomeAdList()
 *            - SetSchHomeAdCreate()
 * --------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : 김보배
 * 수정일    : 2013.06.13
 * 수정내용  :        
 *            - 그룹별 편성상태가 조회&저장할수있도록 수정
 * 수정함수  :
 *            - GetGroupList()
 *            - SetGroupAdd()
 * --------------------------------------------------------
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Base;
using WinFramework.Misc;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerWebService.Media
{
    /// <summary>
    /// GroupOrganizationBiz에 대한 요약 설명입니다.
    /// </summary>
    public class GroupOrganizationBiz : BaseBiz
    {
        public GroupOrganizationBiz()
            : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// 그룹목록 조회
        /// </summary>
        public void GetGroupList(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGroupList() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n SELECT A.GroupCode                                                 \n");
                sbQuery.Append("\n 		 ,A.GroupName                                                 \n");
                sbQuery.Append("\n 		 ,A.Comment 	                                              \n");
                sbQuery.Append("\n 		 ,A.UseYn                                                     \n");
                sbQuery.Append("\n 		 ,CASE A.UseYn WHEN 'Y' THEN '사용' WHEN 'N' THEN '사용안함' END AS UseYn_N   \n");
                sbQuery.Append("\n 		 ,convert(char(19), A.RegDt, 120) RegDt                       \n");
                sbQuery.Append("\n 		 ,convert(char(19), A.ModDt, 120) ModDt           	          \n");
                sbQuery.Append("\n 		 ,B.UserName RegName                                          \n");
                sbQuery.Append("\n 		 ,isnull( ( select count(*) from HomeGroupDetail c with(nolock) where c.GroupCode = a.GroupCode),0) as SchCount \n");
                sbQuery.Append("\n       ,A.AdState                                                   \n"); // [E_02]
                sbQuery.Append("\n       ,C.CodeName AS AdStateame                                    \n"); // [E_02]
                sbQuery.Append("\n  FROM HomeGroup A with(noLock)                                     \n");
                sbQuery.Append("\n  LEFT OUTER JOIN SystemUser B with(noLock) ON A.RegId = B.UserId   \n");
                sbQuery.Append("\n  LEFT JOIN SystemCode C with(noLock) ON A.AdState = C.Code AND C.Section = '25'      \n"); // [E_02]
                sbQuery.Append("\n ORDER BY A.GroupCode DESC                                          \n");

                _db.Open();

                // 쿼리실행
                DataSet ds = new DataSet();

                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 그룹모델에 복사
                groupOrganizationModel.GroupOrganizationDataSet = ds.Copy();
                // 결과
                groupOrganizationModel.ResultCnt = Utility.GetDatasetCount(groupOrganizationModel.GroupOrganizationDataSet);
                // 결과코드 셋트
                groupOrganizationModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGroupList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                groupOrganizationModel.ResultCD = "3000";
                groupOrganizationModel.ResultDesc = "그룹목록 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 그룹목록 추가
        /// </summary>
        public void SetGroupAdd(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            int count = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetGroupAdd() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[4];
                StringBuilder sbNamecount = new StringBuilder();
                StringBuilder sbQuery = new StringBuilder();

                sqlParams[0] = new SqlParameter("@GroupName", SqlDbType.VarChar, 200);
                sqlParams[1] = new SqlParameter("@Comment", SqlDbType.VarChar, 2000);
                sqlParams[2] = new SqlParameter("@UseYn", SqlDbType.Char, 1);
                sqlParams[3] = new SqlParameter("@RegID", SqlDbType.VarChar, 10);

                sqlParams[0].Value = groupOrganizationModel.GroupName;
                sqlParams[1].Value = groupOrganizationModel.Comment;
                sqlParams[2].Value = groupOrganizationModel.UseYn;
                sqlParams[3].Value = header.UserID;				// 등록자

                sbNamecount.Append(" SELECT COUNT(*) \n");
                sbNamecount.Append("  FROM HomeGroup \n");
                sbNamecount.Append("  WHERE GroupName = @GroupName \n");

                sbQuery.Append("INSERT INTO HomeGroup (             \n");
                sbQuery.Append("        GroupCode                   \n");
                sbQuery.Append("       ,GroupName                   \n");
                sbQuery.Append("       ,Comment                     \n");
                sbQuery.Append("       ,UseYn                       \n");
                sbQuery.Append("       ,RegDt                       \n");
                sbQuery.Append("       ,RegID                       \n");
                sbQuery.Append("       ,AdState                     \n"); //[E_02]
                sbQuery.Append("        )                           \n");
                sbQuery.Append(" SELECT ISNULL(MAX(GroupCode),0)+1  \n");
                sbQuery.Append("       ,@GroupName                  \n");
                sbQuery.Append("       ,@Comment                    \n");
                sbQuery.Append("       ,@UseYn                      \n");
                sbQuery.Append("       ,GETDATE()                   \n");
                sbQuery.Append("       ,@RegID                      \n");
                sbQuery.Append("       ,10                          \n"); //[E_02]
                sbQuery.Append(" FROM  HomeGroup                    \n");

                _db.Open();

                DataSet ds = new DataSet();

                _db.ExecuteQueryParams(ds, sbNamecount.ToString(), sqlParams);
                count = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                // 같은 그룹이름이 있을 경우  Exception 발생
                if (count > 0) throw new Exception();

                if (count == 0)
                {
                    _db.BeginTran();
                    _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                }
                groupOrganizationModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetGroupCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                groupOrganizationModel.ResultCD = "3101";
                if (count > 0)
                {
                    groupOrganizationModel.ResultDesc = "그룹목록에 같은 이름이 있으므로 등록할 수 없습니다.";
                }
                else
                {
                    _db.RollbackTran();

                    groupOrganizationModel.ResultDesc = "그룹목록 생성 중 오류가 발생하였습니다";
                }
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 그룹목록 저장
        /// </summary>
        public void SetGroupUpdate(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            int GroupCount = 0;
            int NameCount = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetGroupUpdate() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[5];
                StringBuilder sbUseYnCount = new StringBuilder();
                StringBuilder sbQuery = new StringBuilder();

                sqlParams[0] = new SqlParameter("@GroupName", SqlDbType.VarChar, 200);
                sqlParams[1] = new SqlParameter("@Comment", SqlDbType.VarChar, 2000);
                sqlParams[2] = new SqlParameter("@UseYn", SqlDbType.Char, 1);
                sqlParams[3] = new SqlParameter("@RegID", SqlDbType.VarChar, 10);
                sqlParams[4] = new SqlParameter("@GroupCode", SqlDbType.Int);

                sqlParams[0].Value = groupOrganizationModel.GroupName;
                sqlParams[1].Value = groupOrganizationModel.Comment;
                sqlParams[2].Value = groupOrganizationModel.UseYn;
                sqlParams[3].Value = header.UserID;      // 등록자
                sqlParams[4].Value = Convert.ToInt32(groupOrganizationModel.GroupCode);

                sbUseYnCount.Append(" SELECT COUNT(*)                 \n");
                sbUseYnCount.Append("   FROM SchHomeGroup             \n");
                sbUseYnCount.Append("  WHERE GroupCode = @GroupCode   \n");

                sbQuery.Append(" UPDATE HomeGroup                     \n");
                sbQuery.Append("    SET GroupName   = @GroupName      \n");
                sbQuery.Append("       ,Comment     = @Comment        \n");
                sbQuery.Append("       ,UseYn		= @UseYn          \n");
                sbQuery.Append("       ,ModDt       = GETDATE()       \n");
                sbQuery.Append("       ,RegID       = @RegID          \n");
                sbQuery.Append(" WHERE GroupCode    = @GroupCode      \n");

                _db.Open();

                if (groupOrganizationModel.UseYn == "N")
                {
                    // 그룹별 사용여부를 N으로 변경시
                    // 홈OAP그룹편성에 편성이 되어있는 경우 사용여부를 변경하지 못하게 수정
                    DataSet ds = new DataSet();

                    _db.ExecuteQueryParams(ds, sbUseYnCount.ToString(), sqlParams);
                    GroupCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                    // 등록된 편성이 있으면 수정하지 못하도록
                    if (GroupCount > 0) throw new Exception();
                }

                if (GroupCount == 0)
                {
                    _db.BeginTran();
                    _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();
                }

                // __MESSAGE__
                _log.Message("그룹정보수정:[" + groupOrganizationModel.GroupCode + "] 등록자:[" + header.UserID + "]");

                groupOrganizationModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetGroupUpdate() End");
                _log.Debug("-----------------------------------------");

            }
            catch (Exception ex)
            {
                groupOrganizationModel.ResultCD = "3201";
                if (NameCount > 0)
                {
                    groupOrganizationModel.ResultDesc = "그룹목록에 같은 이름이 있으므로 수정할 수 없습니다.";
                }
                else if (GroupCount > 0)
                {
                    groupOrganizationModel.ResultDesc = "홈OAP그룹편성에서 사용중이므로 사용여부를 수정할 수 없습니다.";
                }
                else
                {
                    _db.RollbackTran();
                    groupOrganizationModel.ResultDesc = "그룹정보 수정중 오류가 발생하였습니다";
                }
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 그룹목록 삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="groupModel"></param>
        public void SetGroupDelete(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            int GroupDetailCount = 0;
            int SchHomeCount = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetGroupDelete() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams     = new SqlParameter[1];
                StringBuilder sbDetailCount  = new StringBuilder();
                StringBuilder sbSchHomeCount = new StringBuilder();
                StringBuilder sbQuery        = new StringBuilder();

                sqlParams[0] = new SqlParameter("@GroupCode", SqlDbType.Int);

                sqlParams[0].Value = Convert.ToInt32(groupOrganizationModel.GroupCode);

                sbDetailCount.Append(" SELECT COUNT(*) FROM HomeGroupDetail 		\n");
                sbDetailCount.Append("  WHERE GroupCode  = @GroupCode            	\n");

                sbSchHomeCount.Append(" SELECT COUNT(*) FROM SchHomeGroup           \n");
                sbSchHomeCount.Append("  WHERE GroupCode = @GroupCode               \n");

                sbQuery.Append("DELETE HomeGroup                \n");
                sbQuery.Append(" WHERE GroupCode  = @GroupCode  \n");

                _db.Open();

                // 쿼리실행
                DataSet ds = new DataSet();

                _db.BeginTran();

                _db.ExecuteQueryParams(ds, sbDetailCount.ToString(), sqlParams);
                GroupDetailCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                ds.Clear();
                _db.ExecuteQueryParams(ds, sbSchHomeCount.ToString(), sqlParams);
                SchHomeCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                if (GroupDetailCount > 0) throw new Exception(); // 등록된 편성이 있으면 삭제하지 못하도록
                if (SchHomeCount > 0) throw new Exception();     // 홈OAP그룹편성에 편성이 되어있으면 삭제하지 못하도록

                if (GroupDetailCount == 0 && SchHomeCount == 0)
                {
                    _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                }
                _db.CommitTran();

                groupOrganizationModel.ResultCD = "0000";  // 정상
                
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetGroupDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                groupOrganizationModel.ResultCD = "3301";

                if (GroupDetailCount > 0)
                {
                    // 이미 다른테이블에 사용중인 데이터가 있다면
                    groupOrganizationModel.ResultDesc = "등록된 편성이 있으므로 그룹정보를 삭제할 수 없습니다.";
                }
                else if (SchHomeCount > 0)
                {
                    // 홈OAP그룹편성에 편성이 되어있는 경우
                    groupOrganizationModel.ResultDesc = "홈OAP그룹편성에 편성이 되어있으므로 그룹정보를 삭제할 수 없습니다.";
                }
                else
                {
                    groupOrganizationModel.ResultDesc = "그룹정보 삭제중 오류가 발생하였습니다";
                }
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 홈광고편성편황조회
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        public void GetSchHomeAdList(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchHomeAdList() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[1];
                StringBuilder sbQuery = new StringBuilder();

                sqlParams[0] = new SqlParameter("@GroupCode", SqlDbType.Int);

                sqlParams[0].Value = groupOrganizationModel.GroupCode;

                // 쿼리생성
                sbQuery.Append(" SELECT 1			    as MediaCode        \n");
                sbQuery.Append("       ,@GroupCode      as GroupCode        \n");
                sbQuery.Append("       ,'10'			as AdType			\n");
                sbQuery.Append("       ,'Slot'			as AdTypeName		\n");
                sbQuery.Append("       ,A.ScheduleOrder	    				\n");
                sbQuery.Append("       ,A.ItemNo							\n");
                sbQuery.Append("       ,'상업광고'		as ItemName         \n");
                sbQuery.Append("       ,''				as ContractName     \n");
                sbQuery.Append("       ,''				as AdvertiserName   \n");
                sbQuery.Append("       ,'10'			as ContState        \n");
                sbQuery.Append("       ,'운영'			as ContStateName	\n");
                sbQuery.Append("       ,''				as RealEndDay       \n");
                sbQuery.Append("       ,'광고'			as AdClassName		\n");
                sbQuery.Append("       ,'30'			as AdState          \n");
                sbQuery.Append("       ,'편성'			as AdStatename		\n");
                sbQuery.Append("       ,'30'			as FileState        \n");
                sbQuery.Append("       ,''				as FileStatename	\n");
                sbQuery.Append("       ,'False'			as CheckYn			\n");
                sbQuery.Append("       ,J.State			as AckState			\n");
                sbQuery.Append("       ,''				as TgtTimeYn		\n");
                sbQuery.Append("       ,''				as JumpTypeName		\n");
                sbQuery.Append("       ,0		        as LogYn    		\n");
                sbQuery.Append("       ,0               as CommonYn         \n");
                sbQuery.Append("       ,0               as FileSize         \n");
                sbQuery.Append("  FROM HomeGroupDetail     A  with(NoLock)	\n");
                sbQuery.Append("  LEFT JOIN SchPublish	J  with(NoLock) ON A.AckNo	= J.AckNo	\n");
                sbQuery.Append(" WHERE A.ItemNo = 0                         \n");
                sbQuery.Append("   AND a.GroupCode = @GroupCode             \n");
                sbQuery.Append(" UNION ALL                                  \n");
                sbQuery.Append(" SELECT B.MediaCode                         \n");
                sbQuery.Append("       ,@GroupCode   as GroupCode           \n");
                sbQuery.Append("       ,B.AdType                            \n");
                sbQuery.Append("       ,H.CodeName AS AdTypeName            \n");
                sbQuery.Append("       ,A.ScheduleOrder                     \n");
                sbQuery.Append("       ,A.ItemNo                            \n");
                sbQuery.Append("       ,B.ItemName                          \n");
                sbQuery.Append("       ,C.ContractName                      \n");
                sbQuery.Append("       ,D.AdvertiserName                    \n");
                sbQuery.Append("       ,C.State AS ContState                \n");
                sbQuery.Append("       ,E.CodeName AS ContStateName         \n");
                sbQuery.Append("       ,B.RealEndDay                        \n");
                sbQuery.Append("       ,F.CodeName AS AdClassName           \n");
                sbQuery.Append("       ,B.AdState                           \n");
                sbQuery.Append("       ,G.CodeName AS AdStatename           \n");
                sbQuery.Append("       ,B.FileState                         \n");
                sbQuery.Append("       ,I.CodeName AS FileStatename         \n");
                sbQuery.Append("       ,'False' AS CheckYn                  \n");
                sbQuery.Append("       ,J.State AS AckState                 \n");
                sbQuery.Append("       ,CASE K.TgtTimeYn WHEN 'Y' THEN '설정' WHEN 'N' THEN '설정' ELSE '' END AS TgtTimeYn   \n");
                sbQuery.Append("       ,M.CodeName AS JumpTypeName                                                            \n");
                sbQuery.Append("       ,isnull(A.LogYn,0)         as LogYn                                                    \n");
                //sbQuery.Append("       ,A.CommonYn                                                                            \n");   //[E_01]
                sbQuery.Append("       ,isnull(B.STBType, 0)     as CommonYn                                                  \n");     //[E_01]
                sbQuery.Append("       ,isnull(B.FileLength, 0)   as FileSize                                                 \n");
                sbQuery.Append("  FROM HomeGroupDetail A with(NoLock)                                                         \n");
                sbQuery.Append("     INNER JOIN ContractItem     B  with(NoLock) ON (A.ItemNo         = B.ItemNo)             \n");
                sbQuery.Append("     INNER JOIN Contract         C  with(NoLock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq) \n");
                sbQuery.Append("     LEFT  JOIN Advertiser       D  with(NoLock) ON (B.AdvertiserCode = D.AdvertiserCode)                \n");
                sbQuery.Append("     LEFT  JOIN SystemCode       E  with(NoLock) ON (C.State          = E.Code AND E.Section = '23')     \n");	// 23 : 계약상태
                sbQuery.Append("     LEFT  JOIN SystemCode       F  with(NoLock) ON (B.AdClass        = F.Code AND F.Section = '29')     \n");	// 29 : 광고용도
                sbQuery.Append("     LEFT  JOIN SystemCode       G  with(NoLock) ON (B.AdState        = G.Code AND G.Section = '25')     \n");	// 25 : 광고상태
                sbQuery.Append("     LEFT  JOIN SystemCode       H  with(NoLock) ON (B.AdType         = H.Code AND H.Section = '26')     \n");	// 26 : 광고종류
                sbQuery.Append("     LEFT  JOIN SystemCode       I  with(NoLock) ON (B.FileState      = I.Code AND I.Section = '31')     \n");  // 31 : 파일상태
                sbQuery.Append("     LEFT  JOIN SchPublish       J  with(NoLock) ON (A.AckNo          = J.AckNo)                         \n");
                sbQuery.Append("     LEFT  JOIN TargetingHome    K  with(NoLock) ON (A.ItemNo         = K.ItemNo)                        \n");
                sbQuery.Append("     LEFT  JOIN ChannelJump      L  with(NoLock) ON (A.ItemNo         = L.ItemNo)                        \n");
                sbQuery.Append("     LEFT  JOIN SystemCode       M with(NoLock) ON (L.JumpType        = M.Code AND M.Section   = '34' )  \n");
                sbQuery.Append(" WHERE A.ItemNo > 0               \n");
                sbQuery.Append("   AND A.GroupCode = @GroupCode   \n");
                sbQuery.Append(" ORDER BY A.ScheduleOrder         \n");

                _db.Open();

                // 쿼리실행
                DataSet ds = new DataSet();

                _db.BeginTran();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);
                
                // 결과 DataSet의 모델에 복사
                groupOrganizationModel.GroupOrganizationDataSet = ds.Copy();

                //지정메뉴편성의 마지막  Order를 구함
                string LastOrder = "1";

                sbQuery = new StringBuilder();
                sbQuery.Append(" SELECT ISNULL(MAX(ScheduleOrder),0) AS MaxOrder  \n");
                sbQuery.Append("   FROM HomeGroupDetail with(NoLock)              \n");
                sbQuery.Append("  WHERE GroupCode = @GroupCode                    \n");

                // 쿼리실행
                ds = new DataSet();

                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);
                _db.CommitTran();

                if (Utility.GetDatasetCount(ds) != 0)
                {
                    LastOrder = Utility.GetDatasetString(ds, 0, "MaxOrder");
                }
                groupOrganizationModel.LastOrder = LastOrder;
                ds.Dispose();

                sbQuery = new StringBuilder();

                // 파일리스트 건수검사용
                sbQuery.Append(" SELECT (                                                         \n");
                sbQuery.Append("   SELECT COUNT(*) AS HomeCnt                                     \n");
                sbQuery.Append("     FROM HomeGroupDetail A  with(NoLock) INNER JOIN ContractItem B  with(NoLock) ON (A.ItemNo = B.ItemNo)  \n");
                sbQuery.Append("      AND B.ExcuteStartDay	<= Convert(varchar(8),getdate(),112)  \n");
                sbQuery.Append("      AND B.RealEndDay		>= Convert(varchar(8),getdate(),112)  \n");
                sbQuery.Append("      AND B.AdState          = '20'                               \n");
                sbQuery.Append("      AND B.FileState        < '90'                               \n");
                sbQuery.Append(" ) + (                                                            \n");
                sbQuery.Append("   SELECT COUNT(*) AS FileCnt                                     \n");
                sbQuery.Append("     FROM ContractItem with(NoLock)                               \n");
                sbQuery.Append("    WHERE FileState = '30'                                        \n");
                sbQuery.Append(" ) AS FileListCnt                                                 \n");

                // 쿼리실행
                ds = new DataSet();

                _db.ExecuteQuery(ds, sbQuery.ToString());

                groupOrganizationModel.FileListCount = Convert.ToInt32(ds.Tables[0].Rows[0]["FileListCnt"].ToString());
                ds.Dispose();

                groupOrganizationModel.ResultCnt = Utility.GetDatasetCount(groupOrganizationModel.GroupOrganizationDataSet);

                groupOrganizationModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchHomeAdList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                groupOrganizationModel.ResultCD = "3000";
                groupOrganizationModel.ResultDesc = "홈광고편성현황 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 홈광고 승인상태를 가져온다
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schPublishModel"></param>
        public void GetHomePublishState(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetHomePublishState() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append(" SELECT TOP 1 AckNo ,State \n");
                sbQuery.Append("   FROM SchPublish         \n");
                sbQuery.Append("  WHERE AdSchType	= 0    \n");
                sbQuery.Append("  ORDER BY AckNo DESC      \n");

                _db.Open();

                // 쿼리실행
                DataSet ds = new DataSet();

                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    groupOrganizationModel.AckNo = ds.Tables[0].Rows[0]["AckNo"].ToString();
                    groupOrganizationModel.State = ds.Tables[0].Rows[0]["State"].ToString();
                }
                else
                {
                    groupOrganizationModel.AckNo = "0";
                    groupOrganizationModel.State = "10";
                }

                ds.Dispose();
                ds = null;

                // 결과코드 셋트
                groupOrganizationModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetPublishState() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                groupOrganizationModel.ResultCD = "3000";
                groupOrganizationModel.ResultDesc = "홈광고 현재승인상태 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 홈광고편성 추가
        /// </summary>
        /// <returns></returns>
        public void SetSchHomeAdCreate(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            int HomeGroupCount = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdCreate() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[6];
                StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbQueryCount = new StringBuilder();

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(groupOrganizationModel.MediaCode);

                sqlParams[0] = new SqlParameter("@GroupCode", SqlDbType.Int);
                sqlParams[1] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[2] = new SqlParameter("@AckNo", SqlDbType.Int);
                sqlParams[3] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[4] = new SqlParameter("@LogYn", SqlDbType.TinyInt);
                sqlParams[5] = new SqlParameter("@UserId", SqlDbType.VarChar, 10);

                sqlParams[0].Value = groupOrganizationModel.GroupCode;
                sqlParams[1].Value = groupOrganizationModel.ItemNo;
                sqlParams[2].Value = Convert.ToInt32(AckNo);
                sqlParams[3].Value = groupOrganizationModel.MediaCode;
                sqlParams[4].Value = groupOrganizationModel.LogYn;
                sqlParams[5].Value = header.UserID;

                sbQuery.Append(" Declare @SchOrder int       \n");
                sbQuery.Append(" Select  @SchOrder = isNull(max(ScheduleOrder),0) From HomeGroupDetail WHERE GroupCode = @GroupCode    \n");
                sbQuery.Append("                             \n");
                #region 수정전쿼리 [E_01]
                //sbQuery.Append(" INSERT INTO HomeGroupDetail    \n");
                //sbQuery.Append(" 	     ( GroupCode         \n");
                //sbQuery.Append(" 		  ,ScheduleOrder     \n");
                //sbQuery.Append(" 		  ,ItemNo            \n");
                //sbQuery.Append(" 		  ,ModDt             \n");
                //sbQuery.Append("          ,AckNo             \n");
                //sbQuery.Append("          ,MediaCode         \n");
                //sbQuery.Append(" 		  ,LogYn             \n");
                //sbQuery.Append("          ,CommonYn )        \n");
                //sbQuery.Append(" VALUES  ( @GroupCode        \n");
                //sbQuery.Append(" 		  ,@SchOrder + 1     \n");
                //sbQuery.Append("          ,@ItemNo           \n");
                //sbQuery.Append(" 		  ,GetDate()         \n");
                //sbQuery.Append(" 		  ,@AckNo            \n");
                //sbQuery.Append(" 		  ,@MediaCode        \n");
                //sbQuery.Append(" 		  ,@LogYn            \n");
                //sbQuery.Append(" 		  ,0 )               \n");
                #endregion
                sbQuery.Append(" INSERT INTO HomeGroupDetail \n");
                sbQuery.Append(" 	     ( GroupCode         \n");
                sbQuery.Append(" 		  ,ScheduleOrder     \n");
                sbQuery.Append(" 		  ,ItemNo            \n");
                sbQuery.Append(" 		  ,ModDt             \n");
                sbQuery.Append("          ,AckNo             \n");
                sbQuery.Append("          ,MediaCode         \n");
                sbQuery.Append(" 		  ,LogYn             \n");
                sbQuery.Append("          ,CommonYn )        \n");
                sbQuery.Append(" VALUES  ( @GroupCode        \n");
                sbQuery.Append(" 		  ,@SchOrder + 1     \n");
                sbQuery.Append("          ,@ItemNo           \n");
                sbQuery.Append(" 		  ,GetDate()         \n");
                sbQuery.Append(" 		  ,@AckNo            \n");
                sbQuery.Append(" 		  ,@MediaCode        \n");
                sbQuery.Append(" 		  ,@LogYn            \n");
                sbQuery.Append(" 		  ,0 )               \n");

                // 그룹광고가 대기상태인 경우에는 광고상태 그대로 들어가고,
                // 그룹광고가 편성상태인 경우에는 편성상태로 변경.
                sbQueryCount.Append(" SELECT count(*)                \n");
                sbQueryCount.Append("  FROM HomeGroup                \n");
                sbQueryCount.Append(" WHERE GroupCode = @GroupCode   \n");
                sbQueryCount.Append("  AND AdState = '20'            \n");
                _db.Open();

                _db.BeginTran();
                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds, sbQueryCount.ToString(), sqlParams);
              
                HomeGroupCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                //if (Convert.ToInt32(groupOrganizationModel.ItemNo) > 0)
                if(HomeGroupCount > 0)
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append(" UPDATE	ContractItem        \n");
                    sbQuery.Append("   SET	AdState	= '20'      \n");
                    sbQuery.Append(" 	   ,ModDt	= GetDate() \n");
                    sbQuery.Append("       ,RegId	= @UserId   \n");
                    sbQuery.Append(" WHERE  ItemNo	= @ItemNo   \n");
                    sbQuery.Append("   AND	AdState	= '10';     \n");
                   
                    _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                }

                _db.CommitTran();

                groupOrganizationModel.ScheduleOrder = Convert.ToString(groupOrganizationModel.ScheduleOrder + 1);

                groupOrganizationModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                groupOrganizationModel.ResultCD = "3101";
                groupOrganizationModel.ResultDesc = "홈광고편성 저장 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 홈광고편성 삭제
        /// </summary>
        /// <returns></returns>
        public void SetSchHomeAdDelete(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdDelete() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[3];
                StringBuilder sbQuery = new StringBuilder();

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(groupOrganizationModel.MediaCode);

                sqlParams[0] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[1] = new SqlParameter("@GroupCode", SqlDbType.Int);
                sqlParams[2] = new SqlParameter("@ScheduleOrder", SqlDbType.Int);

                sqlParams[0].Value = Convert.ToInt32(groupOrganizationModel.ItemNo);
                sqlParams[1].Value = Convert.ToInt32(groupOrganizationModel.GroupCode);
                sqlParams[2].Value = Convert.ToInt32(groupOrganizationModel.ScheduleOrder);

                sbQuery.Append(" DELETE HomeGroupDetail                 \n");
                sbQuery.Append("  WHERE ItemNo        = @ItemNo         \n");
                sbQuery.Append("    AND GroupCode     = @GroupCode      \n");
                sbQuery.Append("    AND ScheduleOrder = @ScheduleOrder  \n");

                _db.Open();

                _db.BeginTran();
                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                // 삭제된 편성정보의 순위 조정
                sbQuery = new StringBuilder();
                sbQuery.Append(" UPDATE HomeGroupDetail                    \n");
                sbQuery.Append("    SET ScheduleOrder = ScheduleOrder - 1  \n");
                sbQuery.Append("  WHERE ScheduleOrder > @ScheduleOrder     \n");
                sbQuery.Append("    AND GroupCode     = @GroupCode         \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                // 편성한 순위
                string LastOrder = "1";

                // 해당 편성한 순서 구함
                sbQuery = new StringBuilder();
                sbQuery.Append(" SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder \n");
                sbQuery.Append("   FROM HomeGroupDetail                           \n");
                sbQuery.Append("  WHERE ScheduleOrder < @ScheduleOrder            \n");
                sbQuery.Append("    AND ItemNo        = @ItemNo                   \n");
                sbQuery.Append("    AND GroupCode     = @GroupCode                \n");

                DataSet ds = new DataSet();

                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);
                _db.CommitTran();

                if (Utility.GetDatasetCount(ds) != 0)
                {
                    LastOrder = Utility.GetDatasetString(ds, 0, "LastOrder");
                }

                ds.Dispose();

                groupOrganizationModel.ScheduleOrder = LastOrder;

                groupOrganizationModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                groupOrganizationModel.ResultCD = "3101";
                groupOrganizationModel.ResultDesc = "홈광고 편성내역 삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 셋탑구분여부 편성여부변경
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schHomeAdModel"></param>
        public void SetSchHomeCommonYn(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdCommonYn() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[3];
                StringBuilder sbQuery = new StringBuilder();

                sqlParams[0] = new SqlParameter("@LogYn", SqlDbType.TinyInt);
                sqlParams[1] = new SqlParameter("@GroupCode", SqlDbType.Int);
                sqlParams[2] = new SqlParameter("@ScheduleOrder", SqlDbType.Int);

                sqlParams[0].Value = groupOrganizationModel.LogYn;
                sqlParams[1].Value = groupOrganizationModel.GroupCode;
                sqlParams[2].Value = groupOrganizationModel.ScheduleOrder;

                sbQuery.Append(" UPDATE HomeGroupDetail                \n");
                sbQuery.Append("	  SET CommonYn   = @LogYn          \n");
                sbQuery.Append(" WHERE GroupCode     = @GroupCode      \n");
                sbQuery.Append("   AND ScheduleOrder = @ScheduleOrder  \n");

                _db.Open();

                _db.BeginTran();
                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                _db.CommitTran();

                groupOrganizationModel.ResultCD = "0000";  // 정상

                _log.Debug("----------------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdCommonYn() End");
                _log.Debug("----------------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                groupOrganizationModel.ResultCD = "3101";
                groupOrganizationModel.ResultDesc = " 홈광고편성  셋탑구분별 변경시 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 홈광고편성 로그변경
        /// </summary>
        /// <returns></returns>
        public void SetSchHomeAdLogYn(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdLogYn() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[4];
                StringBuilder sbQuery = new StringBuilder();

                sqlParams[0] = new SqlParameter("@LogYn", SqlDbType.TinyInt);
                sqlParams[1] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[2] = new SqlParameter("@GroupCode", SqlDbType.Int);
                sqlParams[3] = new SqlParameter("@ScheduleOrder", SqlDbType.Int);

                sqlParams[0].Value = groupOrganizationModel.LogYn;
                sqlParams[1].Value = groupOrganizationModel.ItemNo;
                sqlParams[2].Value = groupOrganizationModel.GroupCode;
                sqlParams[3].Value = groupOrganizationModel.ScheduleOrder;

                sbQuery.Append("UPDATE HomeGroupDetail                 \n");
                sbQuery.Append("   SET LogYn        = @LogYn           \n");
                sbQuery.Append(" WHERE ItemNo       = @ItemNo          \n");
                sbQuery.Append("  AND GroupCode     = @GroupCode       \n");
                sbQuery.Append("  AND ScheduleOrder = @ScheduleOrder   \n");

                _db.Open();

                _db.BeginTran();
                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                _db.CommitTran();

                groupOrganizationModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdLogYn() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                groupOrganizationModel.ResultCD = "3101";
                groupOrganizationModel.ResultDesc = " 홈광고편성  로그변경오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 홈광고편성  첫번째 순위로
        /// </summary>
        /// <returns></returns>
        public void SetSchHomeAdOrderFirst(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdOrderFirst() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[5];
                StringBuilder sbQuery = new StringBuilder();

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(groupOrganizationModel.MediaCode);

                // 변경할 순위
                string ToOrder = "1";

                sqlParams[0] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[1] = new SqlParameter("@GroupCode", SqlDbType.Int);
                sqlParams[2] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[3] = new SqlParameter("@ScheduleOrder", SqlDbType.Int);
                sqlParams[4] = new SqlParameter("@AckNo",AckNo);//추가

                sqlParams[0].Value = groupOrganizationModel.MediaCode;
                sqlParams[1].Value = groupOrganizationModel.GroupCode;
                sqlParams[2].Value = groupOrganizationModel.ItemNo;
                sqlParams[3].Value = groupOrganizationModel.ScheduleOrder;
                sqlParams[4].Value = AckNo;

                // 해당 매체중 MIN값
                sbQuery.Append(" SELECT ISNULL(MIN(ScheduleOrder),1) AS MinOrder  \n");
                sbQuery.Append("   FROM HomeGroupDetail                           \n");
                sbQuery.Append("  WHERE MediaCode = @MediaCode                    \n");
                sbQuery.Append("    AND GroupCode = @GroupCode                    \n");

                _db.Open();

                // 쿼리실행
                DataSet ds = new DataSet();

                _db.BeginTran();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                if (Utility.GetDatasetCount(ds) != 0)
                {
                    ToOrder = Utility.GetDatasetString(ds, 0, "MinOrder");
                }
                ds.Dispose();

                // 해당 순위를 0순위로 임시변경
                sbQuery.Append("UPDATE HomeGroupDetail                 \n");
                sbQuery.Append("   SET ScheduleOrder = 0               \n");
                sbQuery.Append("      ,AckNo         = @AckNo          \n");
                sbQuery.Append(" WHERE ItemNo        = @ItemNo         \n");
                sbQuery.Append("   AND ScheduleOrder = @ScheduleOrder  \n");
                sbQuery.Append("   AND GroupCode     = @GroupCode      \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                // 해당 순위보다 작은 순위의 내역들을 +1하여 조정
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE HomeGroupDetail                       \n");
                sbQuery.Append("   SET ScheduleOrder  = ScheduleOrder + 1    \n");
                sbQuery.Append("      ,AckNo          = @AckNo               \n");
                sbQuery.Append(" WHERE MediaCode      = @MediaCode           \n");
                sbQuery.Append("   AND GroupCode      = @GroupCode           \n");
                sbQuery.Append("   AND ScheduleOrder  < @ScheduleOrder       \n");
                sbQuery.Append("   AND ScheduleOrder  > 0                    \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                // 해당 순위로  변경
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE HomeGroupDetail                 \n");
                sbQuery.Append("   SET ScheduleOrder = " + ToOrder + " \n");
                sbQuery.Append("      ,AckNo         = @AckNo          \n");
                sbQuery.Append(" WHERE ItemNo        = @ItemNo         \n");
                sbQuery.Append("   AND ScheduleOrder = 0               \n");
                sbQuery.Append("   AND GroupCode     = @GroupCode      \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                _db.CommitTran();

                groupOrganizationModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

                groupOrganizationModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdOrderFirst() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                groupOrganizationModel.ResultCD = "3101";
                groupOrganizationModel.ResultDesc = " 홈광고편성  첫번째 순위로 변경 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 홈광고편성  순위올림
        /// </summary>
        /// <returns></returns>
        public void SetSchHomeAdOrderUp(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdOrderUp() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[5];
                StringBuilder sbQuery = new StringBuilder();

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(groupOrganizationModel.MediaCode);

                // 변경할 순위
                string ToOrder = "1";

                sqlParams[0] = new SqlParameter("@ScheduleOrder", SqlDbType.TinyInt);
                sqlParams[1] = new SqlParameter("@MediaCode", SqlDbType.Int);
                sqlParams[2] = new SqlParameter("@GroupCode", SqlDbType.Int);
                sqlParams[3] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[4] = new SqlParameter("@AckNo", AckNo);

                sqlParams[0].Value = groupOrganizationModel.ScheduleOrder;
                sqlParams[1].Value = groupOrganizationModel.MediaCode;
                sqlParams[2].Value = groupOrganizationModel.GroupCode;
                sqlParams[3].Value = groupOrganizationModel.ItemNo;
                sqlParams[4].Value = AckNo;

                // 해당 변경할 순서 구함
                sbQuery = new StringBuilder();
                sbQuery.Append(" SELECT ISNULL(MAX(ScheduleOrder),1) AS UpOrder  \n");
                sbQuery.Append("  FROM  HomeGroupDetail                          \n");
                sbQuery.Append(" WHERE  ScheduleOrder < @ScheduleOrder           \n");
                sbQuery.Append("   AND  MediaCode     = @MediaCode               \n");
                sbQuery.Append("   AND  GroupCode     = @GroupCode               \n");

                _db.Open();

                DataSet ds = new DataSet();

                _db.BeginTran();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                if (Utility.GetDatasetCount(ds) != 0)
                {
                    ToOrder = Utility.GetDatasetString(ds, 0, "UpOrder");
                }
                ds.Dispose();

                // 해당 순위를 0순위로 임시변경
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE HomeGroupDetail                 \n");
                sbQuery.Append("   SET ScheduleOrder = 0               \n");
                sbQuery.Append("      ,AckNo         = @AckNo          \n");
                sbQuery.Append(" WHERE ItemNo        = @ItemNo         \n");
                sbQuery.Append("   AND ScheduleOrder = @ScheduleOrder  \n");
                sbQuery.Append("   AND GroupCode     = @GroupCode      \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                // 해당 순위보다 변경할 순위의 내역을 해당 순위로 조정
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE HomeGroupDetail                 \n");
                sbQuery.Append("   SET ScheduleOrder = @ScheduleOrder  \n");
                sbQuery.Append("      ,AckNo         = @AckNo          \n");
                sbQuery.Append(" WHERE ScheduleOrder = " + ToOrder + " \n");
                sbQuery.Append("   AND MediaCode     = @MediaCode      \n");
                sbQuery.Append("   AND GroupCode     = @GroupCode      \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                // 해당 순위로  변경
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE HomeGroupDetail                 \n");
                sbQuery.Append("   SET ScheduleOrder = " + ToOrder + " \n");
                sbQuery.Append("      ,AckNo         = @AckNo          \n");
                sbQuery.Append(" WHERE ItemNo        = @ItemNo         \n");
                sbQuery.Append("   AND ScheduleOrder = 0               \n");
                sbQuery.Append("   AND GroupCode     = @GroupCode      \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                _db.CommitTran();

                groupOrganizationModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

                groupOrganizationModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdOrderUp() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                groupOrganizationModel.ResultCD = "3101";
                groupOrganizationModel.ResultDesc = " 홈광고편성 순위올림 변경 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 홈광고편성  순위내림
        /// </summary>
        /// <returns></returns>
        public void SetSchHomeAdOrderDown(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdOrderDown() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[5];
                StringBuilder sbQuery = new StringBuilder();

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(groupOrganizationModel.MediaCode);

                // 변경할 순위
                string ToOrder = "1";

                sqlParams[0] = new SqlParameter("@ScheduleOrder", SqlDbType.TinyInt);
                sqlParams[1] = new SqlParameter("@MediaCode", SqlDbType.Int);
                sqlParams[2] = new SqlParameter("@GroupCode", SqlDbType.Int);
                sqlParams[3] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[4] = new SqlParameter("@AckNo", AckNo);

                sqlParams[0].Value = groupOrganizationModel.ScheduleOrder;
                sqlParams[1].Value = groupOrganizationModel.MediaCode;
                sqlParams[2].Value = groupOrganizationModel.GroupCode;
                sqlParams[3].Value = groupOrganizationModel.ItemNo;
                sqlParams[4].Value = AckNo;

                // 해당 변경할 순서 구함
                sbQuery = new StringBuilder();
                sbQuery.Append(" SELECT ISNULL(MIN(ScheduleOrder),1) AS DownOrder  \n");
                sbQuery.Append("   FROM HomeGroupDetail                            \n");
                sbQuery.Append("  WHERE ScheduleOrder > @ScheduleOrder             \n");
                sbQuery.Append("    AND MediaCode     = @MediaCode                 \n");
                sbQuery.Append("    AND GroupCode     = @GroupCode                 \n");

                _db.Open();

                // 쿼리실행
                DataSet ds = new DataSet();

                _db.BeginTran();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);                

                if (Utility.GetDatasetCount(ds) != 0)
                {
                    ToOrder = Utility.GetDatasetString(ds, 0, "DownOrder");
                }
                ds.Dispose();

                // 해당 순위를 0순위로 임시변경
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE HomeGroupDetail                  \n");
                sbQuery.Append("    SET ScheduleOrder = 0               \n");
                sbQuery.Append("       ,AckNo         = @AckNo          \n");
                sbQuery.Append("  WHERE ItemNo        = @ItemNo         \n");
                sbQuery.Append("    AND ScheduleOrder = @ScheduleOrder  \n");
                sbQuery.Append("    AND GroupCode     = @GroupCode      \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);  

                // 해당 순위보다 변경할 순위의 내역을 해당 순위로 조정
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE HomeGroupDetail                  \n");
                sbQuery.Append("   SET ScheduleOrder = @ScheduleOrder   \n");
                sbQuery.Append("      ,AckNo         = @AckNo           \n");
                sbQuery.Append(" WHERE ScheduleOrder = " + ToOrder + "  \n");
                sbQuery.Append("   AND MediaCode     = @MediaCode       \n");
                sbQuery.Append("   AND Groupcode     = @GroupCode       \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                // 해당 순위로  변경
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE HomeGroupDetail                 \n");
                sbQuery.Append("   SET ScheduleOrder = " + ToOrder + " \n");
                sbQuery.Append("      ,AckNo         = @AckNo          \n");
                sbQuery.Append(" WHERE ItemNo        = @ItemNo         \n");
                sbQuery.Append("   AND ScheduleOrder = 0               \n");
                sbQuery.Append("   AND GroupCode     = @GroupCode      \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                _db.CommitTran();

                groupOrganizationModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

                groupOrganizationModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdOrderDown() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                groupOrganizationModel.ResultCD = "3101";
                groupOrganizationModel.ResultDesc = " 홈광고편성 순위내림 변경 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 홈광고편성  마지막 순위로
        /// </summary>
        /// <returns></returns>
        public void SetSchHomeAdOrderLast(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdOrderLast() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[5];
                StringBuilder sbQuery = new StringBuilder();

                // 현재 승인번호를 구함
                string AckNo = GetLastAckNo(groupOrganizationModel.MediaCode);

                // 변경할 순위
                string ToOrder = "1";

                sqlParams[0] = new SqlParameter("@ScheduleOrder", SqlDbType.TinyInt);
                sqlParams[1] = new SqlParameter("@MediaCode", SqlDbType.Int);
                sqlParams[2] = new SqlParameter("@GroupCode", SqlDbType.Int);
                sqlParams[3] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[4] = new SqlParameter("@AckNo", AckNo);

                sqlParams[0].Value = groupOrganizationModel.ScheduleOrder;
                sqlParams[1].Value = groupOrganizationModel.MediaCode;
                sqlParams[2].Value = groupOrganizationModel.GroupCode;
                sqlParams[3].Value = groupOrganizationModel.ItemNo;
                sqlParams[4].Value = AckNo;

                // 해당 변경할 순서 구함
                sbQuery.Append(" SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder  \n");
                sbQuery.Append("   FROM HomeGroupDetail                            \n");
                sbQuery.Append("  WHERE MediaCode = @MediaCode                     \n");
                sbQuery.Append("    AND GroupCode = @GroupCode                     \n");

                _db.Open();

                // 쿼리실행
                DataSet ds = new DataSet();

                _db.BeginTran();
                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);

                if (Utility.GetDatasetCount(ds) != 0)
                {
                    ToOrder = Utility.GetDatasetString(ds, 0, "LastOrder");
                }
                ds.Dispose();

                // 해당 순위를 0순위로 임시변경
                sbQuery = new StringBuilder();
                sbQuery.Append(" UPDATE HomeGroupDetail                 \n");
                sbQuery.Append("    SET ScheduleOrder = 0               \n");
                sbQuery.Append("       ,AckNo         = @AckNo          \n");
                sbQuery.Append("  WHERE ItemNo        = @ItemNo         \n");
                sbQuery.Append("    AND ScheduleOrder = @ScheduleOrder  \n");
                sbQuery.Append("    AND GroupCode     = @GroupCode      \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                // 해당 순위보다 큰 순위의 내역들을 -1하여 조정
                sbQuery = new StringBuilder();
                sbQuery.Append(" UPDATE HomeGroupDetail                     \n");
                sbQuery.Append("    SET ScheduleOrder = ScheduleOrder - 1   \n");
                sbQuery.Append("       ,AckNo         = @AckNo              \n");
                sbQuery.Append("  WHERE MediaCode     = @MediaCode          \n");
                sbQuery.Append("    AND ScheduleOrder > @ScheduleOrder      \n");
                sbQuery.Append("    AND GroupCode     = @GroupCode          \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                // 해당 순위로  변경
                sbQuery = new StringBuilder();
                sbQuery.Append(" UPDATE HomeGroupDetail                 \n");
                sbQuery.Append("    SET ScheduleOrder = " + ToOrder + " \n");
                sbQuery.Append("       ,AckNo         = @AckNo          \n");
                sbQuery.Append("  WHERE ItemNo        = @ItemNo         \n");
                sbQuery.Append("    AND ScheduleOrder = 0               \n");
                sbQuery.Append("    AND GroupCode = @GroupCode          \n");

                _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                _db.CommitTran();

                groupOrganizationModel.ScheduleOrder = ToOrder.ToString();  // 현재 ROW를 찾기위해 컨트롤로 보내주는 키값

                groupOrganizationModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetSchHomeAdOrderLast() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                groupOrganizationModel.ResultCD = "3101";
                groupOrganizationModel.ResultDesc = " 홈광고편성 마지막 순위로 변경 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 현재승인상태의 승인번호를 구함
        /// 상태가 30:배포승인이면 신규(상태 10:편성중)로 생성후 AckNo 리턴
        /// </summary>
        /// <returns></returns>
        private string GetLastAckNo(string MediaCode)
        {
            string AckNo = "";
            string AckState = "";

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetLastAckNo() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append(" DECLARE @AckNo int, @AckState Char(2), @MediaCode int   \n");
                sbQuery.Append("                                                         \n");
                sbQuery.Append(" SELECT @MediaCode = " + MediaCode + "                   \n");
                sbQuery.Append("                                                         \n");
                sbQuery.Append(" SELECT TOP 1 @AckState = State, @AckNo = AckNo          \n");
                sbQuery.Append("   FROM	SchPublish noLock                                \n");
                sbQuery.Append("  WHERE	MediaCode	= @MediaCode                         \n");
                sbQuery.Append("    AND	AdSchType	= 0			                         \n");	//	홈광고형식만
                sbQuery.Append("  ORDER BY AckNo DESC                                    \n");
                sbQuery.Append("                                                         \n");
                sbQuery.Append(" IF @AckState = '30' OR @AckState IS NULL                \n");
                sbQuery.Append("  BEGIN                                                  \n");
                sbQuery.Append("	    INSERT INTO SchPublish                           \n");
                sbQuery.Append("               (AckNo, MediaCode, State,AdSchType, ModifyStartDay)  \n");
                sbQuery.Append("        SELECT ISNULL(MAX(AckNo),0) + 1                  \n");
                sbQuery.Append("              ,@MediaCode								 \n");
                sbQuery.Append("              ,'10'                                      \n");
                sbQuery.Append("              ,0										 \n");
                sbQuery.Append("              ,GETDATE()                                 \n");
                sbQuery.Append("         FROM SchPublish                                 \n");
                sbQuery.Append("        WHERE MediaCode = @MediaCode                     \n");
                sbQuery.Append("                                                         \n");
                sbQuery.Append("     SELECT TOP 1 @AckState = State, @AckNo = AckNo      \n");
                sbQuery.Append("       FROM SchPublish noLock                            \n");
                sbQuery.Append("      WHERE MediaCode = @MediaCode                       \n");
                sbQuery.Append("	    AND AdSchType = 0			                     \n");		//	홈광고형식만
                sbQuery.Append("      ORDER BY AckNo DESC                                \n");
                sbQuery.Append(" END                                                     \n");
                sbQuery.Append("                                                         \n");
                sbQuery.Append(" SELECT @AckNo AS AckNo, @AckState AS AckState           \n");

                _db.Open();

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    AckNo = ds.Tables[0].Rows[0]["AckNo"].ToString();
                    AckState = ds.Tables[0].Rows[0]["AckState"].ToString();
                }
                ds.Dispose();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetLastAckNo() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _log.Exception(ex);
                throw ex;
            }
            finally
            {
                _db.Close();
            }

            return AckNo;
        }

    }
}