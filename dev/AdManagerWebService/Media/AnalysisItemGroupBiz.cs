/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 이호준
 * 수정일    : 2014.07.30
 * 수정내용  : 개발완료
 *  
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

namespace AdManagerWebService.Dev
{
    /// <summary>
    /// AnalysisItemGroupBiz에 대한 요약 설명입니다.
    /// </summary>
    public class AnalysisItemGroupBiz : BaseBiz
    {
        public AnalysisItemGroupBiz()
            : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// 분석용후보자광고내역조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="analysisItemGroupModel"></param>
        public void GetContractItemList(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMonth      :[" + "20" + analysisItemGroupModel.SearchMonth + "01" + "]");
                _log.Debug("AnalysisItemGroupMonth      :[" + analysisItemGroupModel.SearchMonth + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                if (analysisItemGroupModel.SearchMonth.Trim().Length > 0 && analysisItemGroupModel.SearchMonth.Trim().Length <= 6)
                {
                    sbQuery.Append("DECLARE @DateA VARCHAR( 8)  \n");
                    sbQuery.Append("SET @DateA = @SearchMonth \n");
                }
                sbQuery.Append(" SELECT       A.ItemNo, \n");
                sbQuery.Append("              A.AdType, \n");
                sbQuery.Append("              F.CodeName AS AdTypeName, \n");
                sbQuery.Append("              A.ItemName, \n");
                sbQuery.Append("              A.RapCode, \n");
                sbQuery.Append("              C.RapName, \n");
                sbQuery.Append("              A.AgencyCode, \n");
                sbQuery.Append("              D.AgencyName, \n");
                sbQuery.Append("              A.AdvertiserCode, \n");
                sbQuery.Append("              E.AdvertiserName, \n");
                sbQuery.Append("              A.ExcuteStartDay, \n");
                sbQuery.Append("              A.ExcuteEndDay, \n");
                sbQuery.Append("              A.RealEndDay \n");
                sbQuery.Append(" FROM (SELECT * FROM contractitem \n");
                sbQuery.Append(" -- 여기서는IF 문조건으로추가 \n");
                sbQuery.Append(" WHERE 1=1 \n");

                //광고묶음과 비교하여 등록된 데이터는 제외한다.
                if (analysisItemGroupModel.SearchMonth.Trim().Length > 0 && analysisItemGroupModel.SearchMonth.Trim().Length <= 6)
                {
                    sbQuery.Append(" AND RealEndDay >= @DateA and ExcuteStartDay < CONVERT (VARCHAR, dateadd(MM ,1, @DateA),112 ) \n");
                    sbQuery.Append(" AND ExcuteStartDay <= RealEndDay \n");
                    sbQuery.Append(" AND  \n");
                    sbQuery.Append(" NOT EXISTS(  \n");
                    sbQuery.Append("			SELECT AD.Itemno   \n");
                    sbQuery.Append("			FROM (  \n");
                    sbQuery.Append("							SELECT AB.Itemno  \n");
                    sbQuery.Append("							FROM   (SELECT AnalysisItemGroupNo  \n");
                    sbQuery.Append("									FROM AnalysisItemGroup  \n");
                    sbQuery.Append("									WHERE AnalysisItemGroupMonth = @AnalysisItemGroupMonth ) AA  \n");
                    sbQuery.Append("							LEFT JOIN AnalysisItemgroupdetail AB WITH (NoLock) ON (AA.AnalysisItemGroupNo = AB.AnalysisItemGroupNo)  \n");
                    sbQuery.Append("							GROUP BY AB.ItemNO) AD  \n");
                    sbQuery.Append("			WHERE contractitem.ItemNO = AD.ItemNO  \n");
                    sbQuery.Append(" )  \n");
                }

                // 검색어가 있으면
                if (analysisItemGroupModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append(" AND (");
                    sbQuery.Append("   ItemName      LIKE '%" + analysisItemGroupModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append(" ) ");
                }

                sbQuery.Append(" ) A \n");
                sbQuery.Append(" LEFT JOIN MediaRap   C with (NoLock) ON ( A.RapCode        = C .RapCode)     \n");
                sbQuery.Append(" LEFT JOIN Agency     D with (NoLock) ON ( A.AgencyCode     = D.AgencyCode) \n");
                sbQuery.Append(" LEFT JOIN Advertiser E with (NoLock) ON ( A.AdvertiserCode = E.AdvertiserCode ) \n");
                sbQuery.Append(" LEFT JOIN SystemCode F with (NoLock) ON ( A.AdType         = F .Code and F .Section = '26' ) \n");
                sbQuery.Append(" WHERE   A.AdType = '10' \n");
                sbQuery.Append(" ORDER BY A.ItemNo DESC  \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();



                SqlParameter[] sqlParams = new SqlParameter[2];

                //먼저 인서트 된 데이터를 지워주고 그이후에 인서트를 한다.
                sqlParams[0] = new SqlParameter("@SearchMonth", SqlDbType.VarChar);
                sqlParams[1] = new SqlParameter("@AnalysisItemGroupMonth", SqlDbType.Char);

                sqlParams[0].Value = "20" + analysisItemGroupModel.SearchMonth + "01";
                sqlParams[1].Value = analysisItemGroupModel.SearchMonth;

                _db.ExecuteQueryParams(ds, sbQuery.ToString(), sqlParams);


                // 결과 DataSet의 광고주모델에 복사
                analysisItemGroupModel.ContractItemDataSet = ds.Copy();
                // 결과
                analysisItemGroupModel.ResultCnt = Utility.GetDatasetCount(analysisItemGroupModel.ContractItemDataSet);
                // 결과코드 셋트
                analysisItemGroupModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + analysisItemGroupModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUsersList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                analysisItemGroupModel.ResultCD = "3000";
                analysisItemGroupModel.ResultDesc = "광고내역 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


        /// <summary>
        /// 분석용광고묶음조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="analysisItemGroupModel"></param>
        public void GetAnalysisItemGroup(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAnalysisItemGroup() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + analysisItemGroupModel.SearchKey + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("SELECT A .AnalysisItemGroupNo,\n");
                sbQuery.Append("       AnalysisItemGroupName,\n");
                sbQuery.Append("       AnalysisItemGroupMonth,\n");
                sbQuery.Append("       AnalysisItemGroupType,\n");
                sbQuery.Append("       (SELECT COUNT(ItemNo ) FROM analysisitemgroupDetail B with (NoLock) WHERE A.AnalysisItemGroupNo = B.AnalysisItemGroupNo ) AS AdCount,\n");
                sbQuery.Append("       Comment,\n");
                sbQuery.Append("       A.RegID,\n");
                sbQuery.Append("       A.RegDt,\n");
                sbQuery.Append("       A.ModDt,\n");
                sbQuery.Append("       C.UserName, \n");
                sbQuery.Append("       'False' AS CheckYn  \n");
                sbQuery.Append("FROM analysisitemgroup A with (NoLock)\n");
                sbQuery.Append("LEFT JOIN SystemUser C with (NoLock) ON ( A.RegID   = C.UserID)\n");
                sbQuery.Append("WHERE 1=1 \n");
                if (analysisItemGroupModel.SearchMonth.Trim().Length == 4)
                {
                    sbQuery.Append("AND AnalysisItemGroupMonth = '" + analysisItemGroupModel.SearchMonth + "'\n");
                }
                // 검색어가 있으면
                if (analysisItemGroupModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append(" AND (");
                    sbQuery.Append("   AnalysisItemGroupName      LIKE '%" + analysisItemGroupModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append(" ) ");
                }

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                analysisItemGroupModel.AnalysisItemGroupDataSet = ds.Copy();
                // 결과
                analysisItemGroupModel.ResultCnt = Utility.GetDatasetCount(analysisItemGroupModel.AnalysisItemGroupDataSet);
                // 결과코드 셋트
                analysisItemGroupModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + analysisItemGroupModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "분석용광고묶음() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                analysisItemGroupModel.ResultCD = "3000";
                analysisItemGroupModel.ResultDesc = "분석용광고묶음 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


        /// <summary>
        /// 분석용광고묶음상세조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="analysisItemGroupModel"></param>
        public void GetAnalysisItemGroupDetail(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAnalysisItemGroupDetail() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + analysisItemGroupModel.AnalysisItemGroupNo + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성


                sbQuery.Append("SELECT A.AnalysisItemGroupNo,  \n");
                sbQuery.Append("             A.ItemNo,\n");
                sbQuery.Append("             B.AdType,\n");
                sbQuery.Append("             F.CodeName  AS AdTypeName,\n");
                sbQuery.Append("             B.ItemName,\n");
                sbQuery.Append("             B.RapCode,\n");
                sbQuery.Append("             C.RapName ,\n");
                sbQuery.Append("             B.AgencyCode,\n");
                sbQuery.Append("             D.AgencyName,\n");
                sbQuery.Append("             B.AdvertiserCode,\n");
                sbQuery.Append("             E.AdvertiserName,\n");
                sbQuery.Append("             B.ExcuteStartDay,\n");
                sbQuery.Append("             B.ExcuteEndDay,\n");
                sbQuery.Append("             B.RealEndDay, \n");
                sbQuery.Append("             'False' AS CheckYn  \n");
                sbQuery.Append("FROM (                      \n");
                sbQuery.Append("             SELECT *\n");
                sbQuery.Append("             FROM AnalysisItemGroupDetail with(NoLock)\n");

                if (analysisItemGroupModel.AnalysisItemGroupNo > 0)
                {
                    sbQuery.Append("where AnalysisItemGroupNo = " + analysisItemGroupModel.AnalysisItemGroupNo + "\n");
                }

                sbQuery.Append("       ) A\n");
                sbQuery.Append("LEFT JOIN contractitem B with (NoLock) ON A .itemno = B .itemno\n");
                sbQuery.Append("LEFT JOIN MediaRap   C with (NoLock) ON ( B.RapCode        = C .RapCode)    \n");
                sbQuery.Append("LEFT JOIN Agency     D with (NoLock) ON ( B.AgencyCode     = D.AgencyCode)\n");
                sbQuery.Append("LEFT JOIN Advertiser E with (NoLock) ON ( B.AdvertiserCode = E.AdvertiserCode )\n");
                sbQuery.Append("LEFT JOIN SystemCode F with (NoLock) ON ( B.AdType         = F .Code and F .Section = '26' )\n");


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                analysisItemGroupModel.AnalysisItemGroupDetailDataSet = ds.Copy();
                // 결과
                analysisItemGroupModel.ResultCnt = Utility.GetDatasetCount(analysisItemGroupModel.AnalysisItemGroupDetailDataSet);
                // 결과코드 셋트
                analysisItemGroupModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + analysisItemGroupModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "분석용광고묶음상세() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                analysisItemGroupModel.ResultCD = "3000";
                analysisItemGroupModel.ResultDesc = "분석용광고묶음상세 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }



        /// <summary>
        /// 광고묶음수행월조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="analysisItemGroupModel"></param>
        public void GetAnalysisMonths(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAnalysisMonths() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("SELECT   AnalysisItemGroupMonth AS AnalysisMonth, \n");
                sbQuery.Append("            COUNT (AnalysisItemGroupMonth ) AS  AnalysisItemGroupCount \n");
                sbQuery.Append("FROM [AdTargetsHanaTV] .[dbo] . [AnalysisItemGroup] \n");
                sbQuery.Append("GROUP BY AnalysisItemGroupMonth \n");
                sbQuery.Append("order by AnalysisItemGroupMonth \n");


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                analysisItemGroupModel.AnalysisMonthsDataSet = ds.Copy();
                // 결과
                analysisItemGroupModel.ResultCnt = Utility.GetDatasetCount(analysisItemGroupModel.AnalysisMonthsDataSet);
                // 결과코드 셋트
                analysisItemGroupModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + analysisItemGroupModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "광고묶음수행월() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                analysisItemGroupModel.ResultCD = "3000";
                analysisItemGroupModel.ResultDesc = "광고묶음수행월 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


        /// <summary>
        /// 분석용광고묶음생성
        /// </summary>
        /// <param name="header"></param>
        /// <param name="analysisItemGroupModel"></param>
        public void SetAnalysisItemGroupCreate(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAnalysisItemGroupCreate() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQueryMaxno = new StringBuilder();
                StringBuilder sbQuery = new StringBuilder();
                int rc = 0;

                sbQueryMaxno.Append("SELECT ISNULL(MAX(AnalysisItemGroupNo), 0) +1  AS MAXNO  \n");
                sbQueryMaxno.Append("FROM AnalysisItemGroup  \n");

                sbQuery.Append("INSERT INTO AnalysisItemGroup     \n");
                sbQuery.Append("           (AnalysisItemGroupNo   \n");
                sbQuery.Append("           ,AnalysisItemGroupMonth  \n");
                sbQuery.Append("           ,AnalysisItemGroupName  \n");
                sbQuery.Append("           ,AnalysisItemGroupType  \n");
                sbQuery.Append("           ,Comment  \n");
                sbQuery.Append("           ,RegID  \n");
                sbQuery.Append("           ,RegDt  \n");
                sbQuery.Append("           ,ModDt)  \n");
                sbQuery.Append("     VALUES    \n");
                sbQuery.Append("           (@AnalysisItemGroupNo  \n");
                sbQuery.Append("           ,@AnalysisItemGroupMonth   \n");
                sbQuery.Append("           ,@AnalysisItemGroupName   \n");
                sbQuery.Append("           ,@AnalysisItemGroupType   \n");
                sbQuery.Append("           ,@Comment  \n");
                sbQuery.Append("           ,@RegID  \n");
                sbQuery.Append("           ,getdate()    \n");
                sbQuery.Append("           ,getdate())    \n");


                // 쿼리실행
                try
                {
                    DataSet ds = new DataSet();
                    _db.ExecuteQuery(ds, sbQueryMaxno.ToString());
                    int maxNo = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                    SqlParameter[] sqlParams = new SqlParameter[6];

                    //먼저 인서트 된 데이터를 지워주고 그이후에 인서트를 한다.
                    sqlParams[0] = new SqlParameter("@AnalysisItemGroupNo", SqlDbType.Int);
                    sqlParams[1] = new SqlParameter("@AnalysisItemGroupMonth", SqlDbType.Char);
                    sqlParams[2] = new SqlParameter("@AnalysisItemGroupName", SqlDbType.VarChar);
                    sqlParams[3] = new SqlParameter("@AnalysisItemGroupType", SqlDbType.TinyInt);
                    sqlParams[4] = new SqlParameter("@Comment", SqlDbType.VarChar);
                    sqlParams[5] = new SqlParameter("@RegID", SqlDbType.VarChar);

                    sqlParams[0].Value = Convert.ToInt16(maxNo);
                    sqlParams[1].Value = analysisItemGroupModel.AnalysisItemGroupMonth;
                    sqlParams[2].Value = analysisItemGroupModel.AnalysisItemGroupName;
                    sqlParams[3].Value = analysisItemGroupModel.AnalysisItemGroupType;
                    sqlParams[4].Value = analysisItemGroupModel.Comment;
                    sqlParams[5].Value = header.UserID;

                    _db.BeginTran();

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _db.CommitTran();

                    analysisItemGroupModel.AnalysisItemGroupNo = maxNo;

                    // __DEBUG__
                    _log.Debug("AnalysisItemGroupNo:[" + maxNo + "]");
                    _log.Debug("AnalysisItemGroupMonth:[" + analysisItemGroupModel.AnalysisItemGroupMonth + "]");
                    _log.Debug("AnalysisItemGroupName:[" + analysisItemGroupModel.AnalysisItemGroupName + "]");
                    _log.Debug("AnalysisItemGroupType:[" + analysisItemGroupModel.AnalysisItemGroupType + "]");
                    _log.Debug("Comment:[" + analysisItemGroupModel.Comment + "]");
                    _log.Debug("RegID:[" + header.UserID + "]");
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // __MESSAGE__
                    _log.Message("분석용광고묶음생성:[성공]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                analysisItemGroupModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAnalysisItemGroupCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                analysisItemGroupModel.ResultCD = "3101";
                analysisItemGroupModel.ResultDesc = "분석용광고묶음 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


        /// <summary>
        /// 분석용광고묶음상세생성
        /// </summary>
        /// <param name="header"></param>
        /// <param name="analysisItemGroupModel"></param>
        public void SetAnalysisItemGroupDetailCreate(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAnalysisItemGroupDetailCreate() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbQueryValidation = new StringBuilder();
                int rc = 0;



                sbQueryValidation.Append("SELECT COUNT(B.itemno)    \n");
                sbQueryValidation.Append("FROM   (SELECT AnalysisItemGroupNo    \n");
                sbQueryValidation.Append("		FROM AnalysisItemGroup    \n");
                sbQueryValidation.Append("		WHERE AnalysisItemGroupMonth = @AnalysisItemGroupMonth) A    \n");
                sbQueryValidation.Append("LEFT JOIN AnalysisItemgroupdetail B with (NoLock) ON (A.AnalysisItemGroupNo = B.AnalysisItemGroupNo)    \n");
                sbQueryValidation.Append("WHERE ItemNo = @ItemNo    \n");



                sbQuery.Append("INSERT INTO AnalysisItemGroupDetail     \n");
                sbQuery.Append("           (AnalysisItemGroupNo     \n");
                sbQuery.Append("           ,ItemNo)     \n");
                sbQuery.Append("     VALUES     \n");
                sbQuery.Append("           (@AnalysisItemGroupNo     \n");
                sbQuery.Append("           ,@ItemNo)     \n");


                // 쿼리실행
                try
                {
                    SqlParameter[] sqlParamsValidation = new SqlParameter[2];

                    //먼저 인서트 된 데이터를 지워주고 그이후에 인서트를 한다.
                    sqlParamsValidation[0] = new SqlParameter("@AnalysisItemGroupMonth", SqlDbType.Char);
                    sqlParamsValidation[1] = new SqlParameter("@ItemNo", SqlDbType.Int);

                    sqlParamsValidation[0].Value = Convert.ToInt16(analysisItemGroupModel.AnalysisItemGroupMonth);
					sqlParamsValidation[1].Value = Convert.ToInt32(analysisItemGroupModel.ItemNo);


                    DataSet ds = new DataSet();
                    _db.ExecuteQueryParams(ds, sbQueryValidation.ToString(), sqlParamsValidation);
                    int validationCheck = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());


                    if (validationCheck > 0)
                    {
                        //중복된 데이터가 들어옴
                    }
                    else
                    {
                        SqlParameter[] sqlParams = new SqlParameter[2];

                        //먼저 인서트 된 데이터를 지워주고 그이후에 인서트를 한다.
                        sqlParams[0] = new SqlParameter("@AnalysisItemGroupNo", SqlDbType.Int);
                        sqlParams[1] = new SqlParameter("@ItemNo", SqlDbType.Int);


                        sqlParams[0].Value = Convert.ToInt16(analysisItemGroupModel.AnalysisItemGroupNo);
						sqlParams[1].Value = Convert.ToInt32(analysisItemGroupModel.ItemNo);


                        _db.BeginTran();

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                        _db.CommitTran();

                        // __DEBUG__
                        _log.Debug("AnalysisItemGroupNo:[" + analysisItemGroupModel.AnalysisItemGroupNo + "]");
                        _log.Debug("ItemNo:[" + analysisItemGroupModel.ItemNo + "]");
                        _log.Debug(sbQuery.ToString());
                        // __DEBUG__

                        // __MESSAGE__
                        _log.Message("분석용광고묶음상세 생성:[성공]");

                    }



                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                analysisItemGroupModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAnalysisItemGroupDetailCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                analysisItemGroupModel.ResultCD = "3101";
                analysisItemGroupModel.ResultDesc = "분석용광고묶음상세 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        /// <summary>
        /// 분석용광고묶음수정
        /// </summary>
        /// <param name="header"></param>
        /// <param name="analysisItemGroupModel"></param>
        public void SetAnalysisItemGroupUpdate(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAnalysisItemGroupUpdate() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQueryMaxno = new StringBuilder();
                StringBuilder sbQuery = new StringBuilder();
                int rc = 0;

                sbQuery.Append("UPDATE AnalysisItemGroup    \n");
                sbQuery.Append("   SET AnalysisItemGroupName = @AnalysisItemGroupName    \n");
                sbQuery.Append("      ,AnalysisItemGroupType = @AnalysisItemGroupType    \n");
                sbQuery.Append("      ,Comment = @Comment    \n");
                sbQuery.Append("      ,ModDt = getdate()    \n");
                sbQuery.Append(" WHERE AnalysisItemGroupNo = @AnalysisItemGroupNo    \n");

                // 쿼리실행
                try
                {
                    DataSet ds = new DataSet();

                    SqlParameter[] sqlParams = new SqlParameter[4];

                    sqlParams[0] = new SqlParameter("@AnalysisItemGroupNo", SqlDbType.Int);
                    sqlParams[1] = new SqlParameter("@AnalysisItemGroupName", SqlDbType.VarChar);
                    sqlParams[2] = new SqlParameter("@AnalysisItemGroupType", SqlDbType.TinyInt);
                    sqlParams[3] = new SqlParameter("@Comment", SqlDbType.VarChar);

                    sqlParams[0].Value = analysisItemGroupModel.AnalysisItemGroupNo;
                    sqlParams[1].Value = analysisItemGroupModel.AnalysisItemGroupName;
                    sqlParams[2].Value = analysisItemGroupModel.AnalysisItemGroupType;
                    sqlParams[3].Value = analysisItemGroupModel.Comment;

                    _db.BeginTran();

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _db.CommitTran();

                    // __DEBUG__
                    _log.Debug("AnalysisItemGroupNo:[" + analysisItemGroupModel.AnalysisItemGroupNo + "]");
                    _log.Debug("AnalysisItemGroupName:[" + analysisItemGroupModel.AnalysisItemGroupName + "]");
                    _log.Debug("AnalysisItemGroupType:[" + analysisItemGroupModel.AnalysisItemGroupType + "]");
                    _log.Debug("Comment:[" + analysisItemGroupModel.Comment + "]");
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // __MESSAGE__
                    _log.Message("분석용광고묶음수정:[성공]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                analysisItemGroupModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAnalysisItemGroupUpdate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                analysisItemGroupModel.ResultCD = "3101";
                analysisItemGroupModel.ResultDesc = "분석용광고묶음 수정 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }



        /// <summary>
        /// 분석용광고묶음삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="analysisItemGroupModel"></param>
        public void SetAnalysisItemGroupDelete(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            try
            {

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAnalysisItemGroupDelete() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                int rc = 0;

                sbQuery.Append("DELETE FROM AnalysisItemGroupDetail     \n");
                sbQuery.Append("WHERE AnalysisItemGroupNo = @AnalysisItemGroupNo  \n");

                sbQuery.Append("DELETE FROM AnalysisItemGroup     \n");
                sbQuery.Append("WHERE AnalysisItemGroupNo = @AnalysisItemGroupNo  \n");

                // 쿼리실행
                try
                {
                    DataSet ds = new DataSet();

                    SqlParameter[] sqlParams = new SqlParameter[1];

                    sqlParams[0] = new SqlParameter("@AnalysisItemGroupNo", SqlDbType.Int);

                    sqlParams[0].Value = analysisItemGroupModel.AnalysisItemGroupNo;

                    _db.BeginTran();

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _db.CommitTran();

                    // __DEBUG__
                    _log.Debug("AnalysisItemGroupNo:[" + analysisItemGroupModel.AnalysisItemGroupNo + "]");
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // __MESSAGE__
                    _log.Message("분석용광고묶음삭제:[성공]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                analysisItemGroupModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAnalysisItemGroupDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                analysisItemGroupModel.ResultCD = "3101";
                analysisItemGroupModel.ResultDesc = "분석용광고묶음 삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


        /// <summary>
        /// 분석용광고묶음상세삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="analysisItemGroupModel"></param>
        public void SetAnalysisItemGroupDetailDelete(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            try
            {

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAnalysisItemGroupDetailDelete() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                int rc = 0;

                sbQuery.Append("DELETE FROM AnalysisItemGroupDetail     \n");
                sbQuery.Append("WHERE AnalysisItemGroupNo = @AnalysisItemGroupNo  \n");
                sbQuery.Append("AND ItemNo = @ItemNo  \n");
                // 쿼리실행
                try
                {
                    DataSet ds = new DataSet();

                    SqlParameter[] sqlParams = new SqlParameter[2];

                    sqlParams[0] = new SqlParameter("@AnalysisItemGroupNo", SqlDbType.Int);
                    sqlParams[1] = new SqlParameter("@ItemNo", SqlDbType.Int);

                    sqlParams[0].Value = analysisItemGroupModel.AnalysisItemGroupNo;
                    sqlParams[1].Value = analysisItemGroupModel.ItemNo;
                    _db.BeginTran();

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _db.CommitTran();

                    // __DEBUG__
                    _log.Debug("AnalysisItemGroupNo:[" + analysisItemGroupModel.AnalysisItemGroupNo + "]");
                    _log.Debug("ItemNo:[" + analysisItemGroupModel.ItemNo + "]");
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    // __MESSAGE__
                    _log.Message("분석용광고묶음상세삭제:[성공]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                analysisItemGroupModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetAnalysisItemGroupDetailDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                analysisItemGroupModel.ResultCD = "3101";
                analysisItemGroupModel.ResultDesc = "분석용광고묶음상세 삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }




    }

}