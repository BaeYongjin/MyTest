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
	/// CugCodeService에 대한 요약 설명입니다.
	/// </summary>
	public class CugCodeBiz : BaseBiz
	{
		public CugCodeBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  코드목록조회
		/// </summary>
		/// <param name="agencycodeModel"></param>
		public void GetCugCodeList(HeaderModel header, CugCodeModel cugCodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCugCodeList() Start");
				_log.Debug("-----------------------------------------");

				// 데이트베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("Section :[" + cugCodeModel.CugCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT CugCode, CugName  \n"
					+ "   FROM Cug with(noLock)                \n"
					+ "   WHERE UseYn = 'Y'      \n"					
					+ "     AND CugCode != '0'      \n"					
					);

				// 코드분류가 선택했으면
				if (cugCodeModel.SearchKey.Trim().Length > 0)
				{
					sbQuery.Append("  AND AgencyName    LIKE '%" + cugCodeModel.SearchKey.Trim() + "%' \n");
				}	

				sbQuery.Append(" ORDER BY CugCode \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				cugCodeModel.CugCodeDataSet = ds.Copy();
				// 결과
				cugCodeModel.ResultCnt = Utility.GetDatasetCount(cugCodeModel.CugCodeDataSet);
				// 결과코드 셋트
				cugCodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + cugCodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCugCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				cugCodeModel.ResultCD = "3000";
				cugCodeModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);

				// 데이트베이스를  Close한다
				_db.Close();
			}
			// 데이트베이스를  Close한다
			_db.Close();
		}
	}
}
