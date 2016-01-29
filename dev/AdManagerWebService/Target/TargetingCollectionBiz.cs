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
	/// <summary>
	/// TargetingCollectionBiz에 대한 요약 설명입니다.
	/// </summary>
	public class TargetingCollectionBiz : BaseBiz
	{
		public TargetingCollectionBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		/// 고객군목록조회
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetCollectionList(HeaderModel header, TargetingCollectionModel model)
		{
			try
			{
				_log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + model.SearchKey + "]");
                _log.Debug("SearchNonuseYn :[" + model.SearchNonuseYn + "]");
                // __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append("\nSELECT  A.CollectionCode, A.CollectionName, A.Comment ");
				sbQuery.Append("\n       ,(Select count(UserId) from ClientList where CollectionCode = A.CollectionCode AND CollectionCode = A.CollectionCode) AS STBCnt ");
				sbQuery.Append("\n       ,COUNT(B.ItemNo) AS ItemCnt ");
				sbQuery.Append("\n       ,SUM(CASE WHEN B.SetType = '1' AND C.AdType IN ('10','15','16','17','19') THEN 1 ELSE 0 END) AS CMCnt ");
				sbQuery.Append("\n       ,SUM(CASE WHEN B.SetType = '1' AND C.AdType IN ('11','12','20') THEN 1 ELSE 0 END) AS OAPCnt ");
				sbQuery.Append("\n       ,SUM(CASE WHEN B.SetType = '2' THEN 1 ELSE 0 END) AS HomeCnt ");
				sbQuery.Append("\n  FROM Collection A LEFT JOIN TargetingCollection B ON (A.CollectionCode = B.CollectionCode) ");
				sbQuery.Append("\n                    LEFT JOIN ContractItem C ON (B.ItemNo = C.ItemNo) ");
				sbQuery.Append("\n WHERE 1 = 1  ");

                if ((model.SearchNonuseYn.Trim().Length > 0) && model.SearchNonuseYn.Trim().Equals("N"))
                {
                    sbQuery.Append("\n   AND A.UseYn = 'Y' \n");
                }

				// 검색어가 있으면
                if (model.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ( A.CollectionName LIKE '%" + model.SearchKey.Trim() + "%' \n"
                                 + "    OR A.Comment        LIKE '%" + model.SearchKey.Trim() + "%' \n"
						         + " ) ");
				}

                sbQuery.Append(" GROUP BY A.CollectionCode, A.CollectionName, A.Comment\n");
                sbQuery.Append(" ORDER BY A.CollectionCode DESC \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
                model.CollectionsDataSet = ds.Copy();
				// 결과
                model.ResultCnt = Utility.GetDatasetCount(model.CollectionsDataSet);
				// 결과코드 셋트
                model.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
                model.ResultCD = "3000";
                model.ResultDesc = "고객군 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}




        /// <summary>
        /// 타겟팅 광고목록 조회
        /// </summary>
        /// <param name="targetcollectionModel"></param>
        public void GetTargetingCMList(HeaderModel header, TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + model.SearchKey + "]");
                _log.Debug("SearchKey      :[" + model.SetType + "]");
                _log.Debug("CollectionCode :[" + model.CollectionCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("\nSELECT 'False' AS CheckYn, A.ItemNo, B.ItemName ");
                sbQuery.Append("\n  FROM TargetingCollection A LEFT  JOIN ContractItem B ON (A.ItemNo = B.ItemNo) ");
                sbQuery.Append("\n WHERE 1 = 1   ");
                sbQuery.Append("\n   AND A.SetType = '1' -- 1:일반 2:홈광고 ");
                sbQuery.Append("\n   AND B.AdType IN ('10','15','16','17','19') -- 상업광고류");
                sbQuery.Append("\n   AND A.CollectionCode = " + model.CollectionCode + " " );
                sbQuery.Append("\n ORDER BY A.ItemNo DESC  ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 모델에 복사
                model.CMDataSet = ds.Copy();
                // 결과
                model.ResultCnt = Utility.GetDatasetCount(model.CMDataSet);
                // 결과코드 셋트
                model.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "타겟팅 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }




        /// <summary>
        /// 매체광고목록 조회
        /// </summary>
        /// <param name="targetcollectionModel"></param>
        public void GetTargetingOAPList(HeaderModel header, TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + model.SearchKey + "]");
                _log.Debug("CollectionCode :[" + model.CollectionCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("\nSELECT 'False' AS CheckYn,  A.ItemNo, B.ItemName ");
                sbQuery.Append("\n  FROM TargetingCollection A LEFT  JOIN ContractItem B ON (A.ItemNo = B.ItemNo) ");
                sbQuery.Append("\n WHERE 1 = 1   ");
                sbQuery.Append("\n   AND A.SetType = '1' -- 1:일반 2:홈광고 ");
                sbQuery.Append("\n   AND B.AdType IN ('11','12','20')  -- 매체광고류 ");
                sbQuery.Append("\n   AND A.CollectionCode = " + model.CollectionCode + " ");
                sbQuery.Append("\n ORDER BY A.ItemNo DESC  ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 모델에 복사
                model.OAPDataSet = ds.Copy();
                // 결과
                model.ResultCnt = Utility.GetDatasetCount(model.OAPDataSet);
                // 결과코드 셋트
                model.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "타겟팅 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }



        /// <summary>
        /// 홈광고목록 조회
        /// </summary>
        /// <param name="targetcollectionModel"></param>
        public void GetTargetingHomeList(HeaderModel header, TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + model.SearchKey + "]");
                _log.Debug("CollectionCode :[" + model.CollectionCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("\nSELECT 'False' AS CheckYn,  A.ItemNo, B.ItemName ");
                sbQuery.Append("\n  FROM TargetingCollection A LEFT  JOIN ContractItem B ON (A.ItemNo = B.ItemNo) ");
                sbQuery.Append("\n WHERE 1 = 1   ");
                sbQuery.Append("\n   AND A.SetType = '2' -- 1:일반 2:홈광고 ");
                sbQuery.Append("\n   AND A.CollectionCode = " + model.CollectionCode + " ");
                sbQuery.Append("\n ORDER BY A.ItemNo DESC  ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 모델에 복사
                model.HomeDataSet = ds.Copy();
                // 결과
                model.ResultCnt = Utility.GetDatasetCount(model.HomeDataSet);
                // 결과코드 셋트
                model.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "타겟팅 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


        #region 고객군타겟팅 추가
        /// <summary>
        /// 비율 등록
        /// </summary>
        /// <param name="header"></param>
        /// <param name="ratioModel"></param>
        public void SetTargetingCollectionAdd(HeaderModel header, TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("SetType        :[" + model.SetType + "]");
                _log.Debug("ItemNo         :[" + model.ItemNo + "]");
                _log.Debug("CollectionCode :[" + model.CollectionCode + "]");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[3];

                sbQuery.Append(""
                    + "INSERT INTO TargetingCollection ( \n"
                    + "      SetType                  \n"
                    + "		,ItemNo                   \n"
                    + "		,CollectionCode           \n"
                    + "      )                        \n"
                    + " VALUES(                       \n"
                    + "       @SetType  -- 1:일반 2:홈광고 \n"
                    + "      ,@ItemNo				  \n"
                    + "      ,@CollectionCode	      \n"
                    + "		)						  \n"

                    );

                sqlParams[i++] = new SqlParameter("@SetType", SqlDbType.Char,1);
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = model.SetType;
                sqlParams[i++].Value = Convert.ToInt32(model.ItemNo);
                sqlParams[i++].Value = Convert.ToInt32(model.CollectionCode);


                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                model.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3101";
                model.ResultDesc = "고객군타겟팅 추가중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


        #endregion


        #region 고객군타겟팅 삭제
        /// <summary>
        /// 비율 등록
        /// </summary>
        /// <param name="header"></param>
        /// <param name="ratioModel"></param>
        public void SetTargetingCollectionDelete(HeaderModel header, TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("SetType        :[" + model.SetType + "]");
                _log.Debug("ItemNo         :[" + model.ItemNo + "]");
                _log.Debug("CollectionCode :[" + model.CollectionCode + "]");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[3];

                sbQuery.Append(""
                    + "DELETE FROM TargetingCollection         \n"
                    + "	WHERE SetType        = @SetType   -- 1:일반 2:홈광고  \n"
                    + "   AND ItemNo         = @ItemNo         \n"
                    + "	  AND CollectionCode = @CollectionCode \n"
                    );

                sqlParams[i++] = new SqlParameter("@SetType", SqlDbType.Char, 1);
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = model.SetType;
                sqlParams[i++].Value = Convert.ToInt32(model.ItemNo);
                sqlParams[i++].Value = Convert.ToInt32(model.CollectionCode);


                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                model.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3101";
                model.ResultDesc = "고객군타겟팅 삭제중 오류가 발생하였습니다";
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
