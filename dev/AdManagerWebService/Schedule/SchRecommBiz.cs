using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Schedule
{
    /// <summary>
    /// 편성 추천 서비스
    /// </summary>
    public class SchRecommBiz : BaseBiz
    {
        public SchRecommBiz() : base(FrameSystem.connSummaryDbString)
		{
            _log = FrameSystem.oLog;
		}

        /// <summary>
        /// 편성 추천 목록조회
        /// </summary>
        /// <param name="schExcelModel"></param>
        public void GetSchRecommList(HeaderModel header, SchRecommModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchRecommList() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                string shortStartDay = model.SearchStartDay.Substring(2, 6); // yyMMdd
                string shortEndDay = model.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchStartKey  :[" + shortStartDay + "]");
                _log.Debug("SearchEndKey    :[" + shortEndDay + "]");

                string searchSexCode = string.Empty;
                foreach (string key in model.SearchSexCode)
                {
                    searchSexCode += key + ",";
                }
                if (searchSexCode.Length > 0)
                {
                    searchSexCode = searchSexCode.Substring(0, searchSexCode.Length - 1);
                    _log.Debug("SearchSexCode   :[" + searchSexCode + "]");
                }

                string searchAgeCode = string.Empty;
                foreach (string key in model.SearchAgeCode)
                {
                    searchAgeCode += key + ",";
                }
                if (searchAgeCode.Length > 0)
                {
                    searchAgeCode = searchAgeCode.Substring(0, searchAgeCode.Length - 1);
                    _log.Debug("SearchAgeCode   :[" + searchAgeCode + "]");
                }

                string searchCategoryCode = string.Empty;
                foreach (string key in model.SearchCategoryCode)
                {
                    searchCategoryCode += key + ",";
                }
                if (searchCategoryCode.Length > 0)
                {
                    searchCategoryCode = searchCategoryCode.Substring(0, searchCategoryCode.Length - 1);
                    _log.Debug("SearchCategoryCode   :[" + searchCategoryCode + "]");
                }

                string searchTargetRegionCode = string.Empty;
                foreach (string key in model.SearchTargetRegionCode)
                {
                    searchTargetRegionCode += key + ",";
                }
                if (searchTargetRegionCode.Length > 0)
                {
                    searchTargetRegionCode = searchTargetRegionCode.Substring(0, searchTargetRegionCode.Length - 1);
                    _log.Debug("SearchTargetRegionCode   :[" + searchTargetRegionCode + "]");
                } 
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                // 쿼리생성
                sbQuery.AppendLine("");
                sbQuery.AppendLine("    select	top 100 rank() over(order by totalHit DESC) as Rank                                 ");
                sbQuery.AppendLine("        ,	ProgramNm                                                                           ");
	            sbQuery.AppendLine("        ,	totalHit as PgCnt                                                                   ");
	            sbQuery.AppendLine("        ,	GenreName as GenreNm                                                                ");
                sbQuery.AppendLine("    from (                                                                                      ");
		        sbQuery.AppendLine("            select	Progkey                                                                     ");
			    sbQuery.AppendLine("                ,	sum(HitCnt) as totalHit                                                     ");
		        sbQuery.AppendLine("            from SummaryPgDaily0 with(nolock)                                                   ");
		        sbQuery.AppendLine("            where 1=1                                                                           ");
		        sbQuery.AppendLine("            and LogDay between '" + shortStartDay + "' and '" + shortEndDay + "' -- 기간 조건   ");

                int conditionsNum = (searchAgeCode.Length > 0 ? 1 : 0) + (searchSexCode.Length > 0 ? 1 : 0) + (searchTargetRegionCode.Length > 0 ? 1 : 0); // 조건의 개수
                if (conditionsNum > 1)
                {
                    sbQuery.Append("            and (");
                }

                // 연령 조건
                if (searchAgeCode.Length > 0)
                {
                    if (conditionsNum == 1) // 연령대 조건이 유효상태인데 conditionNum이 1인 경우는 조건이 하나이므로 and 추가
                    {
                        sbQuery.Append("            and");
                    }

                    sbQuery.AppendLine(" (SummaryType = 3 and SummaryCode in (" + searchAgeCode + ")) -- 연령대 조건  ");
                }

                // 성별 조건
                if (searchSexCode.Length > 0)
                {
                    if (searchAgeCode.Length > 0) // 선등장하는 연령대 조건의 존재 여부 확인
                    {
                        sbQuery.Append("                or");
                    }
                    else if (conditionsNum == 1)
                    {
                        sbQuery.Append("            and"); // 성별 조건이 존재하는데 conditionNum이 1이라는 것은 성별 조건만 있는 것이므로 and 추가
                    }

                    sbQuery.AppendLine(" (SummaryType = 4 and SummaryCode in (" + searchSexCode + ")) -- 성별 조건    ");
                }

                // 지역 조건
                if (searchTargetRegionCode.Length > 0)
                {
                    if (conditionsNum > 1) // 조건의 수가 1개 이상이라는 것은 선등장 조건들이 존재한다는 것으로 판단
                    {
                        sbQuery.Append("                or");
                    }
                    else if (conditionsNum == 1)
                    {
                        sbQuery.Append("            and");
                    }

                    sbQuery.AppendLine(" (SummaryType = 5 and SummaryCode in (" + searchTargetRegionCode + ")) -- 지역 조건    ");
                }

                if (conditionsNum > 1)
                {
                    sbQuery.AppendLine("                )");
                }
		        
		        sbQuery.AppendLine("            group by Progkey                                                                    ");
	            sbQuery.AppendLine("        ) as v                                                                                  ");
                sbQuery.AppendLine("    inner join AdtargetsHanaTV.dbo.Program as p with(nolock) on p.ProgramKey = v.Progkey        ");
                sbQuery.AppendLine("    inner join AdtargetsHanaTV.dbo.Genre as g with(nolock) on g.GenreCode = p.Genre             ");
                sbQuery.AppendLine("    where 1=1                                                                                   ");
                if (searchCategoryCode.Length > 0)
                {
                    sbQuery.AppendLine("    and Category in (" + searchCategoryCode + ") -- 카테고리 조건                           ");
                }
                sbQuery.AppendLine("    order by Rank;                                                                              ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 대행사모델에 복사
                model.SchRecommDataSet = ds.Copy();
                // 결과
                model.ResultCnt = Utility.GetDatasetCount(model.SchRecommDataSet);
                // 결과코드 셋트
                model.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSchRecommList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "편성 추천 목록 조회중 오류가 발생하였습니다";
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