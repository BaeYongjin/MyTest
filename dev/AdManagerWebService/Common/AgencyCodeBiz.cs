/*
 * -------------------------------------------------------
 * Class Name: AgencyCodeBiz
 * 주요기능  : 그룹정보관리 서비스
 * 작성자    : 모름
 * 작성일    : 모름
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.02
 * 수정내용  :        
 *            - 미디어랩 컨트롤 Disable로 검색 안함
 * 수정함수  :
 *            - GetAgencyCodeList
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

namespace AdManagerWebService.Common
{
	/// <summary>
	/// AgencyCodeService에 대한 요약 설명입니다.
	/// </summary>
	public class AgencyCodeBiz : BaseBiz
	{
		public AgencyCodeBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  코드목록조회
		/// </summary>
		/// <param name="agencycodeModel"></param>
		public void GetAgencyCodeList(HeaderModel header, AgencyCodeModel agencycodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgencyCodeList() Start");
				_log.Debug("-----------------------------------------");

				// 데이트베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("Section :[" + agencycodeModel.AgencyCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
                    + " SELECT a.AGNC_COD AS AgencyCode, a.AGNC_NM AS AgencyName  \n"
                    + "   FROM AGNC a              \n"
                    + "   LEFT JOIN MDA_REP   b  ON (a.REP_COD = b.REP_COD)               \n"
                    + "   WHERE a.USE_YN = 'Y'         \n"					
					);

				// 코드분류가 선택했으면
				if (agencycodeModel.SearchKey.Trim().Length > 0)
				{
                    sbQuery.Append("  AND a.AGNC_NM    LIKE '%" + agencycodeModel.SearchKey.Trim() + "%' \n");
				}
                // [E_01]
                //if(!agencycodeModel.SearchRap.Equals("00"))
                //{
                //    sbQuery.Append("  AND(  a.RapCode = '"+agencycodeModel.SearchRap+"' OR a.RapCode = 0 ) \n");
                //}    

                sbQuery.Append(" ORDER BY a.AGNC_COD \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				agencycodeModel.AgencyCodeDataSet = ds.Copy();
				// 결과
				agencycodeModel.ResultCnt = Utility.GetDatasetCount(agencycodeModel.AgencyCodeDataSet);
				// 결과코드 셋트
				agencycodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + agencycodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgencyCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				agencycodeModel.ResultCD = "3000";
				agencycodeModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);

				// 데이트베이스를  Close한다
				_db.Close();
			}
			// 데이트베이스를  Close한다
			_db.Close();
		}
	}
}
