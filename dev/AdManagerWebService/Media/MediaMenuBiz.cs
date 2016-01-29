/*
 * -------------------------------------------------------
 * Class Name: MediaMenuBiz
 * 주요기능  : MediaMenu 웹 서비스
 * 작성자    : YJ.Park
 * 작성일    : 2014.08.20
 * -------------------------------------------------------
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;
using Oracle.DataAccess.Client;
using System.Collections;

namespace AdManagerWebService.Media
{
    public class MediaMenuBiz : BaseBiz
    {
        public MediaMenuBiz()
            : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// 카테고리 목록조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GetCategoryList(HeaderModel header, MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryList() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode      :[" + mediaMenuModel.SearchMediaCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                //sbQuery.Append("\n  SELECT	A.MenuCode AS CategoryCode									");
                //sbQuery.Append("\n		,	A.MenuName AS CategoryName									");
                //sbQuery.Append("\n      ,	A.MenuOrder													");
                //sbQuery.Append("\n      ,	substring(convert(varchar(19), A.ModDt, 120),3,14) AS ModDt	");
                //sbQuery.Append("\n      ,	B.MediaName													");
                //sbQuery.Append("\n		,	A.MediaCode													");
                //// 카테고리가 이어보기 광고 적용 되어있는 경우 ReplayYn =1
                //// 해당 카테고리의 하위 메뉴중 이어보기가 적용된 메뉴가 존재하는 경우 ReplayYn=2
                //// 적용된 메뉴가 없는 경우 ReplayYn=0
                //sbQuery.Append("\n		,	CASE WHEN  A.ReplayYn = 1 THEN ReplayYn						");
                //sbQuery.Append("\n				 WHEN (	SELECT	MAX(ReplayYn) FROM Menu with(nolock)	");
                //sbQuery.Append("\n						WHERE	uppermenucode = A.Menucode				");
                //sbQuery.Append("\n						AND		ModDt > Getdate() - 30					"); 
                //sbQuery.Append("\n						AND		Menulevel = 2) = 1 THEN 2");
                //sbQuery.Append("\n				 ELSE  0 END as ReplayYn													");
                //// 카테고리가 추천엔딩이 적용 되어있는 경우 REndingYn =1
                //// 해당 카테고리의 하위 메뉴중 추천 엔딩이 적용된 메뉴가 존재하는 경우 REndingYn=2
                //// 적용된 메뉴가 없는 경우 REndingYn=3
                //sbQuery.Append("\n		,	CASE WHEN  A.REndingYn =1 THEN REndingYn										");
                //sbQuery.Append("\n				 WHEN (SELECT MAX(REndingYn) FROM Menu with(nolock) WHERE uppermenucode = A.Menucode AND Menulevel = 2) = 1 THEN 2");
                //sbQuery.Append("\n				 ELSE 0 END as REndingYn													");
                //// 카테고리가 이어보기 광고 적용 되어있는 경우 ReplayYn =1
                //// 해당 카테고리의 하위 메뉴중 이어보기가 적용된 메뉴가 존재하는 경우 ReplayYn=2
                //// 적용된 메뉴가 없는 경우 ReplayYn=3
                //sbQuery.Append("\n		,	CASE WHEN  A.ReplayPPx = 1 THEN ReplayPPx							");
                //sbQuery.Append("\n				 WHEN (SELECT MAX(ReplayPPx) FROM Menu with(nolock) WHERE uppermenucode = A.Menucode AND Menulevel = 2) = 1 THEN 2");
                //sbQuery.Append("\n				 ELSE  0 END as ReplayPPx													");
                //sbQuery.Append("\n  FROM        Menu	A With(noLock)		");
                //sbQuery.Append("\n  inner join	Media	B with(noLock)		");
                //sbQuery.Append("\n			on	A.MediaCode = B.MediaCode	");
                //sbQuery.Append("\n  WHERE		A.MenuLevel = '1'			");
                //sbQuery.Append("\n  AND			A.MenuOrder < '99999990'	");

                //if (mediaMenuModel.SearchMediaCode.Trim().Length > 0 && !mediaMenuModel.SearchMediaCode.Trim().Equals("00"))
                //{
                //    sbQuery.Append("\n AND A.MediaCode = '" + mediaMenuModel.SearchMediaCode.Trim() + "' \n");
                //}

                //sbQuery.Append(" ORDER BY A.MediaCode,CASE A.MenuOrder WHEN 0 THEN 99999 ELSE A.MenuOrder END \n");
                sbQuery.AppendLine();
                sbQuery.AppendLine("    SELECT  MENU_COD        ");
                sbQuery.AppendLine("        ,   MENU_COD_PAR    ");
                sbQuery.AppendLine("        ,   MENU_NM         ");
                sbQuery.AppendLine("        ,   MENU_LVL        ");
                sbQuery.AppendLine("        ,   MENU_ORD        ");
                sbQuery.AppendLine("        ,   MENU_ID_REF     ");
                sbQuery.AppendLine("        ,   AD_PRE_YN       ");
                sbQuery.AppendLine("        ,   AD_MID_YN       ");
                sbQuery.AppendLine("        ,   AD_POST_YN      ");
                sbQuery.AppendLine("        ,   AD_PAY_YN       ");
                sbQuery.AppendLine("        ,   AD_RATE         ");
                sbQuery.AppendLine("        ,   ADN_RATE        ");
                sbQuery.AppendLine("    FROM MENU_COD           ");
                sbQuery.AppendLine("    WHERE MENU_LVL = 2      ");
                sbQuery.AppendLine("    ORDER BY MENU_ORD       ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());
                // 결과 DataSet의 카테고리모델에 복사
                mediaMenuModel.UserDataset = ds.Copy();
                // 결과
                mediaMenuModel.ResultCnt = Utility.GetDatasetCount(mediaMenuModel.UserDataset);
                //결과 코드 셋트
                mediaMenuModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + mediaMenuModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryList() End");
                _log.Debug("-----------------------------------------");

            }
            catch (Exception ex)
            {
                mediaMenuModel.ResultCD = "3000";
                mediaMenuModel.ResultDesc = "카테고리정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 메뉴 목록조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GetMenuList(HeaderModel header, MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetMenuList() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode      :[" + mediaMenuModel.MediaCode + "]");
                _log.Debug("CategoryCode         :[" + mediaMenuModel.CategoryCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                //SqlParameter[] sqlParams = new SqlParameter[2];

                //sqlParams[0] = new SqlParameter("@MediaCode", SqlDbType.Int);
                //sqlParams[0].Value = Convert.ToInt16(mediaMenuModel.MediaCode);

                //sqlParams[1] = new SqlParameter("@CategoryCode", SqlDbType.Int);
                //sqlParams[1].Value = Convert.ToInt32(mediaMenuModel.CategoryCode);

                //sbQuery.Append("\n  SELECT	B.MenuCode                                                              ");
                //sbQuery.Append("\n      ,	B.MenuName                                                              ");
                //sbQuery.Append("\n      ,	B.MenuOrder                                                             ");
                //sbQuery.Append("\n      ,	substring(convert(varchar(19), b.ModDt, 120),3,14) AS ModDt				");
                //sbQuery.Append("\n      ,	(Select MediaName FROM Media WHERE MediaCode = @MediaCode) AS MediaName ");
                //sbQuery.Append("\n      ,	A.MenuName    AS CategoryName                                           ");
                //sbQuery.Append("\n		,	CASE WHEN (B.ModDt>GETDATE()-30)   THEN 'N' ELSE 'Y' END AS Invalidity  ");
                //sbQuery.Append("\n      ,	B.ReplayYn							");
                //sbQuery.Append("\n      ,	B.REndingYn							");
                //sbQuery.Append("\n      ,	isnull(B.ReplayPPx,0) as ReplayPPx	");
                //sbQuery.Append("\n  FROM        Menu A With(noLock)         ");
                //sbQuery.Append("\n  inner join  Menu B with(noLock)			");
                //sbQuery.Append("\n		on	A.MenuCode  = B.UpperMenuCode	");
                //sbQuery.Append("\n  WHERE   A.MediaCode = @MediaCode        ");
                //sbQuery.Append("\n  AND     A.MenuCode  = @CategoryCode     ");
                //sbQuery.Append("\n  AND     B.MenuLevel	= 2                 ");

                //if (!mediaMenuModel.InvalidityMenu)
                //{
                //    sbQuery.Append("\n  AND B.ModDt > GETDATE() - 30    ");
                //}
                //else
                //{
                //    sbQuery.Append("\n AND B.ModDt > GETDATE() - 300		");
                //}
                //sbQuery.Append("\n ORDER BY B.MenuOrder     ");

                sbQuery.AppendLine();
                sbQuery.AppendLine("    SELECT  MENU_COD        ");
                sbQuery.AppendLine("        ,   MENU_COD_PAR    ");
                sbQuery.AppendLine("        ,   MENU_NM         ");
                sbQuery.AppendLine("        ,   MENU_LVL        ");
                sbQuery.AppendLine("        ,   MENU_ORD        ");
                sbQuery.AppendLine("        ,   MENU_ID_REF     ");
                sbQuery.AppendLine("        ,   AD_PRE_YN       ");
                sbQuery.AppendLine("        ,   AD_MID_YN       ");
                sbQuery.AppendLine("        ,   AD_POST_YN      ");
                sbQuery.AppendLine("        ,   AD_PAY_YN       ");
                sbQuery.AppendLine("        ,   AD_RATE         ");
                sbQuery.AppendLine("        ,   ADN_RATE        ");
                sbQuery.AppendLine("    FROM MENU_COD           ");
                sbQuery.AppendLine("    WHERE   MENU_LVL = 3    ");
                sbQuery.AppendLine("        AND MENU_COD_PAR = " + mediaMenuModel.CategoryCode);
                sbQuery.AppendLine("    ORDER BY MENU_ORD       ");

                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                mediaMenuModel.UserDataset = ds.Copy();
                mediaMenuModel.ResultCnt = Utility.GetDatasetCount(mediaMenuModel.UserDataset);
                mediaMenuModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + mediaMenuModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetMenuList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                mediaMenuModel.ResultCD = "3000";
                mediaMenuModel.ResultDesc = "메뉴정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 카테고리 정보 수정
        /// </summary>
        /// <param name="header"></param>
        /// <param name="mediaMenuModel"></param>
        public void SetCategoryUpdate(HeaderModel header, MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetCategoryUpdate() Start");
                _log.Debug("-----------------------------------------");

                //DB OPEN
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("CategoryName      :[" + mediaMenuModel.CategoryName + "]");
                //_log.Debug("CategorySortNo    :[" + mediaMenuModel.CategorySortNo + "]");
                _log.Debug("MediaCode         :[" + mediaMenuModel.MediaCode + "]");
                _log.Debug("CategoryCode      :[" + mediaMenuModel.CategoryCode + "]");
                // __DEBUG__

                StringBuilder sbCategoryQuery = new StringBuilder();
                StringBuilder sbMenuQuery = new StringBuilder();
                ArrayList sbMenuQuerys = new ArrayList();

                int rc = 0;

                sbCategoryQuery.AppendLine();
                sbCategoryQuery.AppendLine("    UPDATE  MENU_COD								");
                sbCategoryQuery.AppendLine("        SET AD_RATE	    = " + mediaMenuModel.CategoryAdRate);
                sbCategoryQuery.AppendLine("        ,   ADN_RATE    = " + mediaMenuModel.CategoryAdNRate);

                if (!mediaMenuModel.CategoryAdPreRollYn.Equals(string.Empty))
                {
                    sbCategoryQuery.AppendLine("        ,   AD_PRE_YN    = '" + mediaMenuModel.CategoryAdPreRollYn + "'");
                }

                if (!mediaMenuModel.CategoryAdMidRollYn.Equals(string.Empty))
                {
                    sbCategoryQuery.AppendLine("        ,   AD_MID_YN    = '" + mediaMenuModel.CategoryAdMidRollYn + "'");
                }

                if (!mediaMenuModel.CategoryAdPostRollYn.Equals(string.Empty))
                {
                    sbCategoryQuery.AppendLine("        ,   AD_POST_YN    = '" + mediaMenuModel.CategoryAdPostRollYn + "'");
                }

                if (!mediaMenuModel.CategoryAdPayYn.Equals(string.Empty))
                {
                    sbCategoryQuery.AppendLine("        ,   AD_PAY_YN    = '" + mediaMenuModel.CategoryAdPayYn + "'");
                }
                sbCategoryQuery.AppendLine("    WHERE MENU_COD     = '" + mediaMenuModel.CategoryCode + "'");

                if (!mediaMenuModel.MenuAdPreRollYn.Equals(string.Empty))
                {
                    sbMenuQuery = new StringBuilder();

                    sbMenuQuery.AppendLine();
                    sbMenuQuery.AppendLine("    UPDATE  MENU_COD							");
                    sbMenuQuery.AppendLine("        SET AD_PRE_YN    = '" + mediaMenuModel.MenuAdPreRollYn + "'");
                    sbMenuQuery.AppendLine("    WHERE   MENU_COD_PAR = '" + mediaMenuModel.CategoryCode + "'");
                    sbMenuQuery.AppendLine("        AND MENU_LVL     = 3				");

                    sbMenuQuerys.Add(sbMenuQuery.ToString());
                }

                if (!mediaMenuModel.MenuAdMidRollYn.Equals(string.Empty))
                {
                    sbMenuQuery = new StringBuilder();

                    sbMenuQuery.AppendLine();
                    sbMenuQuery.AppendLine("    UPDATE  MENU_COD							");
                    sbMenuQuery.AppendLine("        SET AD_MID_YN    = '" + mediaMenuModel.MenuAdMidRollYn + "'");
                    sbMenuQuery.AppendLine("    WHERE   MENU_COD_PAR = '" + mediaMenuModel.CategoryCode + "'");
                    sbMenuQuery.AppendLine("        AND MENU_LVL     = 3				");

                    sbMenuQuerys.Add(sbMenuQuery.ToString());
                }

                if (!mediaMenuModel.MenuAdPostRollYn.Equals(string.Empty))
                {
                    sbMenuQuery = new StringBuilder();

                    sbMenuQuery.AppendLine();
                    sbMenuQuery.AppendLine("    UPDATE  MENU_COD				");
                    sbMenuQuery.AppendLine("        SET AD_POST_YN   = '" + mediaMenuModel.MenuAdPostRollYn + "'");
                    sbMenuQuery.AppendLine("    WHERE   MENU_COD_PAR = '" + mediaMenuModel.CategoryCode + "'");
                    sbMenuQuery.AppendLine("        AND MENU_LVL     = 3		");

                    sbMenuQuerys.Add(sbMenuQuery.ToString());
                }

                if (!mediaMenuModel.MenuAdPayYn.Equals(string.Empty))
                {
                    sbMenuQuery = new StringBuilder();

                    sbMenuQuery.AppendLine();
                    sbMenuQuery.AppendLine("    UPDATE  MENU_COD				");
                    sbMenuQuery.AppendLine("        SET AD_PAY_YN    = '" + mediaMenuModel.MenuAdPayYn + "'");
                    sbMenuQuery.AppendLine("    WHERE   MENU_COD_PAR = '" + mediaMenuModel.CategoryCode + "'");
                    sbMenuQuery.AppendLine("        AND MENU_LVL     = 3		");

                    sbMenuQuerys.Add(sbMenuQuery.ToString());
                }

                //if (mediaMenuModel.CategoryReplayPPx.Equals("1"))
                //{
                //    // 카테고리 설정이기 때문에 하위 메뉴는 전부 초기화 시킴
                //    sbQuery.Append("\n ");
                //    sbQuery.Append("\n UPDATE   Menu							");
                //    sbQuery.Append("\n SET		ReplayPPx	  = 0				");
                //    sbQuery.Append("\n WHERE	MediaCode	  = @MediaCode		");
                //    sbQuery.Append("\n AND		UpperMenuCode = @CategoryCode	");
                //    sbQuery.Append("\n AND		MenuLevel     = 2;				");
                //}

                //if (mediaMenuModel.CategoryREndingYn.Equals("1"))
                //{
                //    sbQuery.Append("\n UPDATE   Menu							");
                //    sbQuery.Append("\n	SET		REndingYn	  = 0				");
                //    sbQuery.Append("\n WHERE	MediaCode	  = @MediaCode		");
                //    sbQuery.Append("\n AND		UpperMenuCode = @CategoryCode	");
                //    sbQuery.Append("\n AND		MenuLevel     = 2;				");
                //}

                // __DEBUG__
                _log.Debug(sbCategoryQuery.ToString());
                foreach (string str in sbMenuQuerys)
                {
                    _log.Debug(str);
                }
                // __DEBUG__

                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQuery(sbCategoryQuery.ToString());

                    if (sbMenuQuerys.Count > 0)
                    {
                        foreach (string query in sbMenuQuerys)
                        {
                            rc = _db.ExecuteNonQuery(query);
                        }
                    }

                    _db.CommitTran();

                    // __DEBUG__
                    _log.Message("카테고리 정보 수정:[" + mediaMenuModel.CategoryCode + "] 등록자 : [" + header.UserID + "]");
                    // __DEBUG__
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                mediaMenuModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetCategoryUpdate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                mediaMenuModel.ResultCD = "3201";
                mediaMenuModel.ResultDesc = "카테고리정보 수정중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

		public void SetCategoryUpdateOption(HeaderModel header, MediaMenuModel mediaMenuModel)
		{
			try
			{
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();
				SqlParameter[] sqlParams = new SqlParameter[3];
				int rc = 0;

				sqlParams[0] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
				sqlParams[1] = new SqlParameter("@CategoryCode", SqlDbType.Int);
				sqlParams[2] = new SqlParameter("@mValue", SqlDbType.TinyInt);

				sqlParams[0].Value = Convert.ToInt32(mediaMenuModel.MediaCode);
				sqlParams[1].Value = Convert.ToInt32(mediaMenuModel.CategoryCode);
				sqlParams[2].Value = Convert.ToInt16(mediaMenuModel.mValue);

				if (mediaMenuModel.mType == MediaMenuType.Reply)
				{
					// 카테고리 설정이기 때문에, Enabled인 경우엔 하위메뉴를 초기화 시킴
					//                       Disabled는 하위메뉴는 그냥 둠
					sbQuery.Append("\n  UPDATE	Menu						");
					sbQuery.Append("\n	SET		ReplayYn	= @mValue		");
					sbQuery.Append("\n	WHERE	MediaCode	= @MediaCode	");
					sbQuery.Append("\n	AND		MenuCode	= @CategoryCode;");

					if (mediaMenuModel.mValue == 1)
					{
						sbQuery.Append("\n ");
						sbQuery.Append("\n UPDATE   Menu							");
						sbQuery.Append("\n SET		ReplayYn	  = 0				");
						sbQuery.Append("\n WHERE	MediaCode	  = @MediaCode		");
						sbQuery.Append("\n AND		UpperMenuCode = @CategoryCode	");
						sbQuery.Append("\n AND		MenuLevel     = 2;				");
					}
				}
				else if (mediaMenuModel.mType == MediaMenuType.ReplyPPx)
				{
					// 카테고리 설정이기 때문에, Enabled인 경우엔 하위메뉴를 초기화 시킴
					//                       Disabled는 하위메뉴는 그냥 둠
					sbQuery.Append("\n  UPDATE	Menu						");
					sbQuery.Append("\n	SET		ReplayPPx	= @mValue		");
					sbQuery.Append("\n	WHERE	MediaCode	= @MediaCode	");
					sbQuery.Append("\n	AND		MenuCode	= @CategoryCode;");

					if (mediaMenuModel.mValue == 1)
					{
						sbQuery.Append("\n ");
						sbQuery.Append("\n UPDATE   Menu							");
						sbQuery.Append("\n SET		ReplayPPx	  = 0				");
						sbQuery.Append("\n WHERE	MediaCode	  = @MediaCode		");
						sbQuery.Append("\n AND		UpperMenuCode = @CategoryCode	");
						sbQuery.Append("\n AND		MenuLevel     = 2;				");
					}
				}
				else if (mediaMenuModel.mType == MediaMenuType.REnding)
				{
					// 카테고리 설정이기 때문에, Enabled인 경우엔 하위메뉴를 초기화 시킴
					//                       Disabled는 하위메뉴는 그냥 둠
					sbQuery.Append("\n  UPDATE	Menu						");
					sbQuery.Append("\n	SET		REndingYn	= @mValue		");
					sbQuery.Append("\n	WHERE	MediaCode	= @MediaCode	");
					sbQuery.Append("\n	AND		MenuCode	= @CategoryCode;");

					if (mediaMenuModel.mValue == 1)
					{
						sbQuery.Append("\n ");
						sbQuery.Append("\n UPDATE   Menu							");
						sbQuery.Append("\n SET		REndingYn	  = 0				");
						sbQuery.Append("\n WHERE	MediaCode	  = @MediaCode		");
						sbQuery.Append("\n AND		UpperMenuCode = @CategoryCode	");
						sbQuery.Append("\n AND		MenuLevel     = 2;				");
					}
				}

				_log.Debug(sbQuery.ToString());

				try
				{
					_db.BeginTran();
					rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __DEBUG__
					_log.Message(string.Format("카테고리 항목 수정:{0},{1},{2},{3}	작업자:{4}"
												, mediaMenuModel.CategoryCode
												, mediaMenuModel.CategoryName
												, mediaMenuModel.mType.ToString()
												, mediaMenuModel.mValue
												, header.UserID));
					// __DEBUG__
				}
				catch (Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediaMenuModel.ResultCD = "0000";
			}
			catch (Exception ex)
			{
				mediaMenuModel.ResultCD = "3201";
				mediaMenuModel.ResultDesc = "카테고리정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

        /// <summary>
        /// 메뉴 정보 수정
        /// </summary>
        /// <param name="header"></param>
        /// <param name="mediaMenuModel"></param>
        public void SetMenuUpdate(HeaderModel header, MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuUpdate() Start");
                _log.Debug("-----------------------------------------");

                //DB OPEN
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MenuName        :[" + mediaMenuModel.MenuName + "]");
                //_log.Debug("MenuOrder       :[" + mediaMenuModel.MenuOrder + "]");
                _log.Debug("CategoryCode   :[" + mediaMenuModel.CategoryCode + "]");
                _log.Debug("MediaCode       :[" + mediaMenuModel.MediaCode + "]");
                _log.Debug("MenuCode        :[" + mediaMenuModel.MenuCode + "]");
                // __DEBUG__

                StringBuilder sbCategoryQuery = new StringBuilder();
                StringBuilder sbMenuQuery = new StringBuilder();

                int rc = 0;

                sbMenuQuery.AppendLine();
                sbMenuQuery.AppendLine("    UPDATE  MENU_COD								");
                sbMenuQuery.AppendLine("        SET AD_RATE	    = " + mediaMenuModel.MenuAdRate);
                sbMenuQuery.AppendLine("        ,   ADN_RATE    = " + mediaMenuModel.MenuAdNRate);

                if (!mediaMenuModel.MenuAdPreRollYn.Equals(string.Empty))
                {
                    sbMenuQuery.AppendLine("        ,   AD_PRE_YN    = '" + mediaMenuModel.MenuAdPreRollYn + "'");
                }

                if (!mediaMenuModel.MenuAdMidRollYn.Equals(string.Empty))
                {
                    sbMenuQuery.AppendLine("        ,   AD_MID_YN    = '" + mediaMenuModel.MenuAdMidRollYn + "'");
                }

                if (!mediaMenuModel.MenuAdPostRollYn.Equals(string.Empty))
                {
                    sbMenuQuery.AppendLine("        ,   AD_POST_YN    = '" + mediaMenuModel.MenuAdPostRollYn + "'");
                }

                if (!mediaMenuModel.MenuAdPayYn.Equals(string.Empty))
                {
                    sbMenuQuery.AppendLine("        ,   AD_PAY_YN    = '" + mediaMenuModel.MenuAdPayYn + "'");
                }
                sbMenuQuery.AppendLine("    WHERE MENU_COD     = '" + mediaMenuModel.MenuCode + "'");

                sbCategoryQuery.AppendLine();
                sbCategoryQuery.AppendLine("    UPDATE  MENU_COD								");
                sbCategoryQuery.AppendLine("        SET AD_RATE	    = " + mediaMenuModel.CategoryAdRate);
                sbCategoryQuery.AppendLine("        ,   ADN_RATE    = " + mediaMenuModel.CategoryAdNRate);

                if (!mediaMenuModel.CategoryAdPreRollYn.Equals(string.Empty))
                {
                    sbCategoryQuery.AppendLine("        ,   AD_PRE_YN    = '" + mediaMenuModel.CategoryAdPreRollYn + "'");
                }

                if (!mediaMenuModel.CategoryAdMidRollYn.Equals(string.Empty))
                {
                    sbCategoryQuery.AppendLine("        ,   AD_MID_YN    = '" + mediaMenuModel.CategoryAdMidRollYn + "'");
                }

                if (!mediaMenuModel.CategoryAdPostRollYn.Equals(string.Empty))
                {
                    sbCategoryQuery.AppendLine("        ,   AD_POST_YN    = '" + mediaMenuModel.CategoryAdPostRollYn + "'");
                }

                if (!mediaMenuModel.CategoryAdPayYn.Equals(string.Empty))
                {
                    sbCategoryQuery.AppendLine("        ,   AD_PAY_YN    = '" + mediaMenuModel.CategoryAdPayYn + "'");
                }
                sbCategoryQuery.AppendLine("    WHERE MENU_COD     = '" + mediaMenuModel.CategoryCode + "'");

				//카테고리의 하위 메뉴중 1개라도 이어보기 광고/추천 엔딩광고가 해제(0) 되는 경우 카테고리의 해당 설정도 해제 됨
                //if (mediaMenuModel.MenuReplayYn.Equals("0"))
                //{
                //    sbQuery.Append("\n UPDATE MENU										");
                //    sbQuery.Append("\n SET		ReplayYn		= @ReplayYn			");
                //    sbQuery.Append("\n WHERE	MenuCode	= @CategoryCode	");
                //}
                //if (mediaMenuModel.MenuREndingYn.Equals("0"))
                //{
                //    sbQuery.Append("\n UPDATE MENU										");
                //    sbQuery.Append("\n SET		REndingYn	= @REndingYn			");
                //    sbQuery.Append("\n WHERE	MenuCode	= @CategoryCode	");
                //}
                // __DEBUG__
                _log.Debug(sbCategoryQuery.ToString());
                _log.Debug(sbMenuQuery.ToString());
                // __DEBUG__

                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQuery(sbCategoryQuery.ToString());
                    rc = _db.ExecuteNonQuery(sbMenuQuery.ToString());
                    _db.CommitTran();

                    // __DEBUG__
                    _log.Message("메뉴 정보 수정:[" + mediaMenuModel.CategoryCode + "] 등록자 : [" + header.UserID + "]");
                    // __DEBUG__
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                mediaMenuModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuUpdate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                mediaMenuModel.ResultCD = "3201";
                mediaMenuModel.ResultDesc = "메뉴정보 수정중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

		public void SetMenuUpdateOption(HeaderModel header, MediaMenuModel mediaMenuModel)
		{
			try
			{
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();
				SqlParameter[] sqlParams = new SqlParameter[4];
				int rc = 0;

				sqlParams[0] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
				sqlParams[1] = new SqlParameter("@CategoryCode", SqlDbType.Int);
				sqlParams[2] = new SqlParameter("@MenuCode", SqlDbType.Int);
				sqlParams[3] = new SqlParameter("@mValue", SqlDbType.TinyInt);

				sqlParams[0].Value = Convert.ToInt32(mediaMenuModel.MediaCode);
				sqlParams[1].Value = Convert.ToInt32(mediaMenuModel.CategoryCode);
				sqlParams[2].Value = Convert.ToInt32(mediaMenuModel.MenuCode);
				sqlParams[3].Value = Convert.ToInt16(mediaMenuModel.mValue);

				if (mediaMenuModel.mType == MediaMenuType.Reply)
				{
					// 메뉴설정이기 때문에, Enabled인 경우엔 상위메뉴를 초기화 시킴
					sbQuery.Append("\n  UPDATE	Menu						");
					sbQuery.Append("\n	SET		ReplayYn	= @mValue		");
					sbQuery.Append("\n	WHERE	MediaCode	 = @MediaCode	");
					sbQuery.Append("\n	AND		MenuCode	 = @MenuCode	");
					sbQuery.Append("\n	AND		UpperMenuCode= @CategoryCode");

					if (mediaMenuModel.mValue == 1)
					{
						sbQuery.Append("\n ");
						sbQuery.Append("\n UPDATE   Menu							");
						sbQuery.Append("\n SET		ReplayYn	  = 0				");
						sbQuery.Append("\n WHERE	MediaCode	  = @MediaCode		");
						sbQuery.Append("\n AND		MenuCode	  = @CategoryCode	");
						sbQuery.Append("\n AND		UpperMenuCode = @CategoryCode	");
						sbQuery.Append("\n AND		MenuLevel     = 1;				");
					}
				}
				else if (mediaMenuModel.mType == MediaMenuType.ReplyPPx)
				{
					// 카테고리 설정이기 때문에, Enabled인 경우엔 하위메뉴를 초기화 시킴
					//                       Disabled는 하위메뉴는 그냥 둠
					sbQuery.Append("\n  UPDATE	Menu						");
					sbQuery.Append("\n	SET		ReplayPPx	= @mValue		");
					sbQuery.Append("\n	WHERE	MediaCode	 = @MediaCode	");
					sbQuery.Append("\n	AND		MenuCode	 = @MenuCode	");
					sbQuery.Append("\n	AND		UpperMenuCode= @CategoryCode");

					if (mediaMenuModel.mValue == 1)
					{
						sbQuery.Append("\n ");
						sbQuery.Append("\n UPDATE   Menu							");
						sbQuery.Append("\n SET		ReplayPPx	  = 0				");
						sbQuery.Append("\n WHERE	MediaCode	  = @MediaCode		");
						sbQuery.Append("\n AND		MenuCode	  = @CategoryCode	");
						sbQuery.Append("\n AND		UpperMenuCode = @CategoryCode	");
						sbQuery.Append("\n AND		MenuLevel     = 1;				");
					}
				}
				else if (mediaMenuModel.mType == MediaMenuType.REnding)
				{
					// 카테고리 설정이기 때문에, Enabled인 경우엔 하위메뉴를 초기화 시킴
					//                       Disabled는 하위메뉴는 그냥 둠
					sbQuery.Append("\n  UPDATE	Menu						");
					sbQuery.Append("\n	SET		REndingYn	= @mValue		");
					sbQuery.Append("\n	WHERE	MediaCode	 = @MediaCode	");
					sbQuery.Append("\n	AND		MenuCode	 = @MenuCode	");
					sbQuery.Append("\n	AND		UpperMenuCode= @CategoryCode");

					if (mediaMenuModel.mValue == 1)
					{
						sbQuery.Append("\n ");
						sbQuery.Append("\n UPDATE   Menu							");
						sbQuery.Append("\n SET		REndingYn	  = 0				");
						sbQuery.Append("\n WHERE	MediaCode	  = @MediaCode		");
						sbQuery.Append("\n AND		MenuCode	  = @CategoryCode	");
						sbQuery.Append("\n AND		UpperMenuCode = @CategoryCode	");
						sbQuery.Append("\n AND		MenuLevel     = 1;				");
					}
				}

				_log.Debug(sbQuery.ToString());

				try
				{
					_db.BeginTran();
					rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __DEBUG__
					_log.Message(string.Format("메뉴 항목 수정:{0},{1},{2},{3} Type{4}  작업자:{4}"
												, mediaMenuModel.CategoryCode
												, mediaMenuModel.MenuCode
												, mediaMenuModel.MenuName
												, mediaMenuModel.mType.ToString()
												, mediaMenuModel.mValue
												, header.UserID));
					// __DEBUG__
				}
				catch (Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediaMenuModel.ResultCD = "0000";
			}
			catch (Exception ex)
			{
				mediaMenuModel.ResultCD = "3201";
				mediaMenuModel.ResultDesc = "카테고리정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

        /// <summary>
        /// 메뉴 정보 삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="mediaMenuModel"></param>
        public void DeleteMenu(HeaderModel header, MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "DeleteMenu() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MediaCode        :[" + mediaMenuModel.MediaCode + "]");
                _log.Debug("MenuCode         :[" + mediaMenuModel.MenuCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[2];

                int rc = 0;

                sbQuery.Append("\n  DELETE Menu                         \n");
                sbQuery.Append("\n  WHERE MediaCode     = @MediaCode    \n");
                sbQuery.Append("\n  AND   MenuCode      = @MenuCode     \n");

                sqlParams[0] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[1] = new SqlParameter("@MenuCode", SqlDbType.SmallInt);

                sqlParams[0].Value = mediaMenuModel.MediaCode;
                sqlParams[1].Value = mediaMenuModel.MenuCode;

                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __DEBUG__
                    _log.Message("메뉴정보삭제:[" + mediaMenuModel.MenuCode + "] 등록자:[" + header.UserID + "]");
                    // __DEBUG__

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                mediaMenuModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "DeleteMenu() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                mediaMenuModel.ResultCD = "3301";
                mediaMenuModel.ResultDesc = "메뉴정보 삭제중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 카테고리 생성
        /// </summary>
        /// <param name="header"></param>
        /// <param name="mediaMenuModel"></param>
        public void SetCategoryCreate(HeaderModel header, MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetCategoryCreate() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MediaCode        :[" + mediaMenuModel.MediaCode + "]");
                _log.Debug("CategoryName     :[" + mediaMenuModel.CategoryName + "]");
                //_log.Debug("CategorySortNo   :[" + mediaMenuModel.CategorySortNo + "]");
                //_log.Debug("ReplayYn         :[" + mediaMenuModel.CategoryReplayYn + "]");
                //_log.Debug("ReplayPPx        :[" + mediaMenuModel.CategoryReplayPPx + "]");
                //_log.Debug("REndingYn        :[" + mediaMenuModel.CategoryREndingYn + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[6];

                int rc = 0;
				
                sqlParams[0] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[1] = new SqlParameter("@CategoryName", SqlDbType.VarChar, 50);
                sqlParams[2] = new SqlParameter("@MenuOrder", SqlDbType.Int);
				sqlParams[3] = new SqlParameter("@ReplayYn", SqlDbType.TinyInt);
				sqlParams[4] = new SqlParameter("@ReplayPPx", SqlDbType.TinyInt);
				sqlParams[5] = new SqlParameter("@REndingYn", SqlDbType.TinyInt);

                sqlParams[0].Value = Convert.ToInt16(mediaMenuModel.MediaCode);
                sqlParams[1].Value = mediaMenuModel.CategoryName;
                //sqlParams[2].Value = Convert.ToInt32(mediaMenuModel.CategorySortNo);
                //sqlParams[3].Value = Convert.ToInt16(mediaMenuModel.CategoryReplayYn);
                //sqlParams[4].Value = Convert.ToInt16(mediaMenuModel.CategoryReplayPPx);
                //sqlParams[5].Value = Convert.ToInt16(mediaMenuModel.CategoryREndingYn);

				sbQuery.Append("\n  INSERT INTO Menu (                      ");
				sbQuery.Append("\n          MediaCode                       ");
				sbQuery.Append("\n         ,MenuCode                        ");
				sbQuery.Append("\n         ,MenuName                        ");
				sbQuery.Append("\n         ,UpperMenuCode                   ");
				sbQuery.Append("\n         ,MenuLevel                       ");
				sbQuery.Append("\n         ,ModDt                           ");
				sbQuery.Append("\n         ,MenuOrder                       ");
				sbQuery.Append("\n         ,ReplayYn                        ");
				sbQuery.Append("\n         ,ReplayPPx                       ");
				sbQuery.Append("\n         ,REndingYn                       ");
				sbQuery.Append("\n      )                                   ");
				sbQuery.Append("\n      SELECT                              ");
				sbQuery.Append("\n         @MediaCode                       ");
				sbQuery.Append("\n         ,ISNULL(MAX(MenuCode),0)+1       ");
				sbQuery.Append("\n         ,@CategoryName                   ");
				sbQuery.Append("\n         ,ISNULL(MAX(MenuCode),0)+1       ");
				sbQuery.Append("\n         ,'1'                             ");
				sbQuery.Append("\n         ,GETDATE()                       ");
				sbQuery.Append("\n         ,@MenuOrder                      ");
				sbQuery.Append("\n         ,@ReplayYn                       ");
				sbQuery.Append("\n         ,@ReplayPPx                      ");
				sbQuery.Append("\n         ,@REndingYn                      ");
				sbQuery.Append("\n      FROM Menu		                    ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __DEBUG__
                    _log.Message("카테고리정보생성:[" + mediaMenuModel.CategoryCode + "(" + mediaMenuModel.CategoryName + ")] 등록자:[" + header.UserID + "]");
                    // __DEBUG__
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                mediaMenuModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetCategoryCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                mediaMenuModel.ResultCD = "3101";
                mediaMenuModel.ResultDesc = "카테고리정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 메뉴 생성
        /// </summary>
        /// <param name="header"></param>
        /// <param name="mediaMenuModer"></param>
        public void SetMenuCreate(HeaderModel header, MediaMenuModel mediaMenuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuCreate() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MediaCode        :[" + mediaMenuModel.MediaCode + "]");
                _log.Debug("MenuName         :[" + mediaMenuModel.MenuName + "]");
                _log.Debug("UpperMenuCode    :[" + mediaMenuModel.CategoryCode + "]");
                //_log.Debug("MenuOrder        :[" + mediaMenuModel.MenuOrder + "]");
                //_log.Debug("ReplayYn         :[" + mediaMenuModel.MenuReplayYn + "]");
                //_log.Debug("REndingYn         :[" + mediaMenuModel.MenuREndingYn + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[6];

                int rc = 0;
				
                sqlParams[0] = new SqlParameter("@MediaCode", SqlDbType.TinyInt);
                sqlParams[1] = new SqlParameter("@MenuName", SqlDbType.VarChar, 50);
                sqlParams[2] = new SqlParameter("@UpperMenuCode", SqlDbType.Int);
                sqlParams[3] = new SqlParameter("@MenuOrder", SqlDbType.Int);
                sqlParams[4] = new SqlParameter("@ReplayYn", SqlDbType.TinyInt);
                sqlParams[5] = new SqlParameter("@REndingYn", SqlDbType.TinyInt);

                sqlParams[0].Value = Convert.ToInt16(mediaMenuModel.MediaCode);
                sqlParams[1].Value = mediaMenuModel.MenuName;
                sqlParams[2].Value = Convert.ToInt32(mediaMenuModel.CategoryCode);
                //sqlParams[3].Value = Convert.ToInt32(mediaMenuModel.MenuOrder);
                //sqlParams[4].Value = Convert.ToInt16(mediaMenuModel.MenuReplayYn);
                //sqlParams[5].Value = Convert.ToInt16(mediaMenuModel.MenuREndingYn);

				sbQuery.Append("\n  INSERT INTO Menu (                      ");
				sbQuery.Append("\n          MediaCode                       ");
				sbQuery.Append("\n         ,MenuCode                        ");
				sbQuery.Append("\n         ,MenuName                        ");
				sbQuery.Append("\n         ,UpperMenuCode                   ");
				sbQuery.Append("\n         ,MenuLevel                       ");
				sbQuery.Append("\n         ,ModDt                           ");
				sbQuery.Append("\n         ,MenuOrder                       ");
				sbQuery.Append("\n         ,ReplayYn                        ");
				sbQuery.Append("\n         ,REndingYn                        ");
				sbQuery.Append("\n      )                                   ");
				sbQuery.Append("\n      SELECT                              ");
				sbQuery.Append("\n         @MediaCode                       ");
				sbQuery.Append("\n         ,ISNULL(MAX(MenuCode),0)+1       ");
				sbQuery.Append("\n         ,@MenuName                       ");
				sbQuery.Append("\n         ,@UpperMenuCode                  ");
				sbQuery.Append("\n         ,'2'                             ");
				sbQuery.Append("\n         ,GETDATE()                       ");
				sbQuery.Append("\n         ,@MenuOrder                      ");
				sbQuery.Append("\n         ,@ReplayYn                       ");
				sbQuery.Append("\n         ,@REndingYn                       ");
				sbQuery.Append("\n      FROM Menu		                    ");

                //if (mediaMenuModel.MenuReplayYn.ToString().Equals("0"))
                //{
                //    sbQuery.Append("\n UPDATE MENU										");
                //    sbQuery.Append("\n SET		ReplayYn		= @ReplayYn			");
                //    sbQuery.Append("\n WHERE	MenuCode	= @UpperMenuCode	");
                //}

                //if (mediaMenuModel.MenuREndingYn.ToString().Equals("0"))
                //{
                //    sbQuery.Append("\n UPDATE MENU										");
                //    sbQuery.Append("\n SET		REndingYn	= @REndingYn			");
                //    sbQuery.Append("\n WHERE	MenuCode	= @UpperMenuCode	");
                //}
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("카테고리정보생성:[" + mediaMenuModel.MenuCode + "(" + mediaMenuModel.MenuName + ")] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                mediaMenuModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                mediaMenuModel.ResultCD = "3101";
                mediaMenuModel.ResultDesc = "메뉴정보 생성 중 오류가 발생하였습니다";
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