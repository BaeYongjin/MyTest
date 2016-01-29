using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Media
{
	/// <summary>
	/// Class1에 대한 요약 설명입니다.
	/// </summary>
	public class MenuMapBiz : BaseBiz
	{
		public MenuMapBiz()
			: base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		/// 카테고리목록조회
		/// </summary>
		/// <param name="categoryModel"></param>
		public void GetCategoryList(HeaderModel header, MenuMapModel model)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCategoryList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n	select	c.Category			as category ");
				sbQuery.Append("\n		,	c.CategoryName		as categoryName ");
				sbQuery.Append("\n		,	c.ordr				as ordr ");
				sbQuery.Append("\n		,	c.ui40				as category4 ");
				sbQuery.Append("\n		,	isnull(x.unmapCnt,0) as unmapCnt ");
				sbQuery.Append("\n	from	xpgcategory c with(nolock) ");
				sbQuery.Append("\n	left outer join ( ");
				sbQuery.Append("\n				select	v.UpperMenuCode as category ");
				sbQuery.Append("\n					,	COUNT(*) as unmapCnt ");
				sbQuery.Append("\n				from ( ");
				sbQuery.Append("\n						select	MenuCode, MenuName, UpperMenuCode, AdGenre,MenuOrder ");
				sbQuery.Append("\n						from	Menu4 s with(nolock) ");
				sbQuery.Append("\n						where	ModDt > GETDATE() - 30 ");
				sbQuery.Append("\n						and		not exists ( select 1 from MenuMap m with(nolock) ");
				sbQuery.Append("\n											 where m.MenuCode = s.MenuCode ) ) v ");
				sbQuery.Append("\n				group by v.UpperMenuCode ) x ");
				sbQuery.Append("\n		on x.category = c.UI40 ");
				sbQuery.Append("\n	order by ordr ");	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());
				
				// 결과 DataSet의 카테고리모델에 복사
				model.CategoryDs = ds.Copy();
				model.ResultCnt = Utility.GetDatasetCount(model.CategoryDs);
				model.ResultCD = "0000";
				model.ResultDesc = "";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + model.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCategoryList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				model.ResultCD = "3000";
				model.ResultDesc = "카테고리정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}
		}

        /// <summary>
        /// 메뉴매핑목록조회
        /// </summary>
        /// <param name="categoryModel"></param>
        public void GetMenuMapList(HeaderModel header, MenuMapModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetMenuMapList() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();
                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.AppendLine("declare @stdCategory int;                                                                   ");
                sbQuery.AppendLine("declare @ui40Category int;                                                                  ");
                sbQuery.AppendLine("                                                                                            ");
                sbQuery.AppendLine("set @stdCategory =" + model.SearchCategory                                                   );
                sbQuery.AppendLine("                                                                                            ");
                sbQuery.AppendLine("select @ui40Category = UI40                                                                 ");
                sbQuery.AppendLine("from   XpgCategory                                                                          ");
                sbQuery.AppendLine("where  Category = @stdCategory                                                              ");
                sbQuery.AppendLine("                                                                                            ");
                sbQuery.AppendLine("select *                                                                                    ");
                sbQuery.AppendLine("from (                                                                                      ");
                sbQuery.AppendLine("             select	a.UpperMenuCode	as UpperMenu                                            ");
                sbQuery.AppendLine("				,   a.MenuCode		as MenuCode                                             ");
                sbQuery.AppendLine("                ,   a.MenuName		as MenuName                                             ");
                sbQuery.AppendLine("                ,   a.AdGenre		as AdGenre                                              ");
                sbQuery.AppendLine("                ,   a.MenuOrder		as MenuOrder                                            ");
                sbQuery.AppendLine("                ,   b.code40		as MapCode                                              ");
                sbQuery.AppendLine("             from (                                                                         ");
                sbQuery.AppendLine("                           select MenuCode, MenuName, UpperMenuCode, AdGenre,MenuOrder      ");
                sbQuery.AppendLine("                           from   Menu s with(nolock)                                       ");
                sbQuery.AppendLine("                           where  ModDt > GETDATE() - 30                                    ");
                sbQuery.AppendLine("                           and          UpperMenuCode = @stdCategory ) a                    ");
                sbQuery.AppendLine("             left outer join                                                                ");
                sbQuery.AppendLine("                    (                                                                       ");
                sbQuery.AppendLine("							select	menuCodeStd as codeStd                                  ");
                sbQuery.AppendLine("								,   MenuCode as code40                                      ");
                sbQuery.AppendLine("							from	menuMap with(nolock)                                    ");
                sbQuery.AppendLine("							where	UiName = 'NXNEWUI' ) b on b.codeStd = a.adGenre ) UI20  ");
                sbQuery.AppendLine("full outer join                                                                             ");
                sbQuery.AppendLine("       (                                                                                    ");
                sbQuery.AppendLine("             select	a.UpperMenuCode	as UpperMenu4                                           ");
                sbQuery.AppendLine("				,   a.MenuCode		as MenuCode4                                            ");
                sbQuery.AppendLine("                ,   a.MenuName		as MenuName4                                            ");
                sbQuery.AppendLine("                ,   a.AdGenre		as AdGenre4                                             ");
                sbQuery.AppendLine("                ,   a.MenuOrder		as MenuOrder4                                           ");
                sbQuery.AppendLine("                ,   b.code40		as MapCode4                                             ");
                sbQuery.AppendLine("             from (                                                                         ");
                sbQuery.AppendLine("                           select MenuCode, MenuName, UpperMenuCode, AdGenre,MenuOrder      ");
                sbQuery.AppendLine("                           from   Menu4 s with(nolock)                                      ");
                sbQuery.AppendLine("                           where  ModDt > GETDATE() - 30                                    ");
                sbQuery.AppendLine("                           and          UpperMenuCode = @ui40Category ) a                   ");
                sbQuery.AppendLine("             left outer join                                                                ");
                sbQuery.AppendLine("                    (                                                                       ");
                sbQuery.AppendLine("							select	menuCodeStd as codeStd                                  ");
                sbQuery.AppendLine("								,	MenuCode as code40                                      ");
                sbQuery.AppendLine("							from	menuMap with(nolock)                                    ");
                sbQuery.AppendLine("							where	UiName = 'NXNEWUI' ) b on b.code40 = a.adGenre ) UI40   ");
                sbQuery.AppendLine("       on UI20.MapCode = UI40.MapCode4                                                      ");
                sbQuery.AppendLine("order by MenuOrder, MenuOrder4                                                              ");
                
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 카테고리모델에 복사
                model.MenuMapDs = ds.Copy();
                model.ResultCnt = Utility.GetDatasetCount(model.MenuMapDs);
                model.ResultCD = "0000";
                model.ResultDesc = "";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetMenuMapList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "메뉴매핑 정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 메뉴매핑 수정
        /// </summary>
        /// <param name="header"></param>
        /// <param name="mediarapModel"></param>
        public void SetMenuMapUpdate(HeaderModel header, MenuMapModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuMapUpdate() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[2];

                sbQuery.AppendLine(" UPDATE MenuMap SET MenuCode = @MenuCode    ");
                sbQuery.AppendLine(" WHERE MenuCodeStd = @MenuCodeStd;          ");

                i = 0;
                sqlParams[i++] = new SqlParameter("@MenuCode", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@MenuCodeStd", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(model.MenuCode4);
                sqlParams[i++].Value = Convert.ToInt32(model.AdGenre);

                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("메뉴매핑정보수정:[기준코드(" + model.AdGenre + ")] 등록자:[" + header.UserID + "]");
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                model.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuMapUpdate() End");
                _log.Debug("-----------------------------------------");

            }
            catch (Exception ex)
            {
                model.ResultCD = "3201";
                model.ResultDesc = "메뉴매핑정보 수정중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            // 데이터베이스를  Close한다
            _db.Close();

        }

        /// <summary>
        /// 메뉴매핑 설정
        /// </summary>
        /// <param name="header"></param>
        /// <param name="mediarapModel"></param>
        public void SetMenuMapCreate(HeaderModel header, MenuMapModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuMapCreate() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[2];

                sbQuery.AppendLine("    INSERT INTO MenuMap (	UiName      ");
				sbQuery.AppendLine("    				,	MenuCode        ");
				sbQuery.AppendLine("    				,	MenuCodeStd )   ");
				sbQuery.AppendLine("    	VALUES  (	'NXNEWUI'           ");
				sbQuery.AppendLine("    				,	@MenuCode       ");
				sbQuery.AppendLine("    				,	@MenuCodeStd);  ");

                sqlParams[i++] = new SqlParameter("@MenuCode", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@MenuCodeStd", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(model.MenuCode4);
                sqlParams[i++].Value = Convert.ToInt32(model.AdGenre);

                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("메뉴매핑 정보생성:[기준코드(" + model.AdGenre + ") : 매핑코드(" + model.MenuCode4 + ")] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                model.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuMapCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3101";
                model.ResultDesc = "메뉴매핑정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 메뉴 매핑 해제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="mediarapModel"></param>
        public void SetMenuMapDelete(HeaderModel header, MenuMapModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuMapDelete() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[1];

                sbQuery.AppendLine("delete MenuMap where MenuCodeStd = @MenuCodeStd;");

                sqlParams[i++] = new SqlParameter("@MenuCodeStd", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(model.AdGenre);

                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("메뉴매핑 정보삭제:[기준코드(" + model.AdGenre + ")] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                model.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuMapDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3301";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 기준 정보 메뉴 생성
        /// </summary>
        /// <param name="header"></param>
        /// <param name="mediarapModel"></param>
        public void SetMenuCreate(HeaderModel header, MenuMapModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuCreate() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[3];

                sbQuery.AppendLine("   declare @categoryNew	    int;                                                                                          ");
                sbQuery.AppendLine("   declare @genre			int;                                                                                          ");
                sbQuery.AppendLine("   declare @WorkDt			datetime;                                                                                     ");
                sbQuery.AppendLine("   declare @ord			    varchar(10);                                                                                  ");
                sbQuery.AppendLine("                                                                                                                          ");
                sbQuery.AppendLine("   -- UI4.0 카테고리를 공통카테고리로 변환                                                                                ");
                sbQuery.AppendLine("   select @categoryNew = category from xpgCategory with(nolock) where UI40 = @category;                                   ");
                sbQuery.AppendLine("                                                                                                                          ");
                sbQuery.AppendLine("   set @WorkDt	= GetDate();                                                                                              ");
                sbQuery.AppendLine("                                                                                                                          ");
                sbQuery.AppendLine("   -- 신규 메뉴의 Order 획득                                                                                              ");
                sbQuery.AppendLine("   select Top 1                                                                                                           ");
                sbQuery.AppendLine("   @ord = convert(varchar(10), convert(int, MenuOrder) + 100)                                                             ");
                sbQuery.AppendLine("   from Menu with(nolock)                                                                                                 ");
                sbQuery.AppendLine("   where ModDt > GETDATE() - 30 and UpperMenuCode = @categoryNew                                                          ");
                sbQuery.AppendLine("   order by MenuOrder DESC;                                                                                               ");
                sbQuery.AppendLine("                                                                                                                          ");
                sbQuery.AppendLine("   if @MenuCode4 > 32767                                                                                                  ");
                sbQuery.AppendLine("   begin                                                                                                                  ");
                sbQuery.AppendLine("       -- 현재 메뉴코드 smallInt형식임, 변환이 필요함                                                                     ");
                sbQuery.AppendLine("       select top 1 @genre = MenuCode from MenuTemp with(nolock) where AdGenre = 0 order by MenuCode;                     ");
                sbQuery.AppendLine("                                                                                                                          ");
                sbQuery.AppendLine("       update MenuTemp                                                                                                    ");
                sbQuery.AppendLine("       set     AdGenre = @MenuCode4                                                                                       ");
                sbQuery.AppendLine("           ,   ModDt   = GetDate()                                                                                        ");
                sbQuery.AppendLine("       where   MenuCode = @genre;                                                                                         ");
                sbQuery.AppendLine("   end                                                                                                                    ");
                sbQuery.AppendLine("   else                                                                                                                   ");
                sbQuery.AppendLine("       select @genre = @MenuCode4;                                                                                        ");
                sbQuery.AppendLine("                                                                                                                          ");
                sbQuery.AppendLine("   -- 메뉴신규처리                                                                                                        ");
                sbQuery.AppendLine("   insert into dbo.Menu ( MediaCode , MenuCode  , MenuName	, UpperMenuCode	, MenuLevel	, ModDt	 , MenuOrder, AdGenre )   ");
                sbQuery.AppendLine("   			    values	( 1		    , @genre	, @genreName, @categoryNew	, 2			, @WorkDt, @ord		, @MenuCode4);");

                sqlParams[i++] = new SqlParameter("@MenuCode4", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@genreName", SqlDbType.VarChar, 50);
                sqlParams[i++] = new SqlParameter("@category", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(model.MenuCode4);
                sqlParams[i++].Value = model.MenuName4;
                sqlParams[i++].Value = Convert.ToInt32(model.UpperMenuCode4);

                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("기준 메뉴 정보생성:[UI4.0 메뉴코드(" + model.MenuCode4 + ")] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                model.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetMenuCreate() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3101";
                model.ResultDesc = "기준 메뉴 정보 생성 중 오류가 발생하였습니다";
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