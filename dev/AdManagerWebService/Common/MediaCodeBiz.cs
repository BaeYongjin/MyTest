using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Common
{
	/// <summary>
	/// MediaCodeService에 대한 요약 설명입니다.
	/// </summary>
	public class MediaCodeBiz : BaseBiz
	{
		public MediaCodeBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  코드목록조회
		/// </summary>
		/// <param name="mediacodeModel"></param>
		public void GetMediaCodeList(HeaderModel header, MediaCodeModel mediacodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaCodeList() Start");
				_log.Debug("-----------------------------------------");

				// 데이트베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode :[" + mediacodeModel.MediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT mda_cod as MediaCode, mda_nm as MediaName  \n"
					+ "   FROM MDA                 \n"					
					+ "   WHERE use_yn = 'Y'         \n"					
					);

				// 코드분류가 선택했으면
				if (mediacodeModel.SearchKey.Trim().Length > 0)
				{
					sbQuery.Append("  AND mda_name    LIKE '%" + mediacodeModel.SearchKey.Trim() + "%' \n");
				}				
				

				sbQuery.Append(" ORDER BY mda_cod \n");
				
				_log.Debug(sbQuery.ToString());			
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				mediacodeModel.MediaCodeDataSet = ds.Copy();
				// 결과
				mediacodeModel.ResultCnt = Utility.GetDatasetCount(mediacodeModel.MediaCodeDataSet);
				// 결과코드 셋트
				mediacodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + mediacodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				mediacodeModel.ResultCD = "3000";
				mediacodeModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
				
				// 데이트베이스를  Close한다
				_db.Close();

			}

			// 데이트베이스를  Close한다
			_db.Close();

		}
	}
}
