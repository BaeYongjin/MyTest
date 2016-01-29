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
	/// MediaRapCodeService에 대한 요약 설명입니다.
	/// </summary>
	public class MediaRapCodeBiz : BaseBiz
	{
		public MediaRapCodeBiz() : base(FrameSystem.connDbString, true)
		{

			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  코드목록조회
		/// </summary>
		/// <param name="mediacodeModel"></param>
		public void GetMediaRapCodeList(HeaderModel header, MediaRapCodeModel mediacodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapCodeList() Start");
				_log.Debug("-----------------------------------------");		

				// 데이트베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("Section :[" + mediacodeModel.RapCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
                    + " SELECT REP_COD AS RapCode, REP_NM AS RapName  \n"
                    + "   FROM MDA_REP               \n"
                    + "   WHERE USE_YN = 'Y'         \n"					
					);

				// 코드분류가 선택했으면
				if (mediacodeModel.SearchKey.Trim().Length > 0)
				{
                    sbQuery.Append("  AND REP_NM    LIKE '%" + mediacodeModel.SearchKey.Trim() + "%' \n");
				}

                sbQuery.Append(" ORDER BY REP_COD \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				mediacodeModel.MediaRapCodeDataSet = ds.Copy();
				// 결과
				mediacodeModel.ResultCnt = Utility.GetDatasetCount(mediacodeModel.MediaRapCodeDataSet);
				// 결과코드 셋트
				mediacodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + mediacodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapCodeList() End");
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


		/// <summary>
		///  코드목록조회(ID를 체크하여 해당하는 아이디 권한만 조회 시킨다.
		/// </summary>
		/// <param name="GetMediaRapCodeListIdCheck"></param>
		public void GetMediaRapCodeListIdCheck(HeaderModel header, MediaRapCodeModel mediacodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapCodeList() Start");
				_log.Debug("-----------------------------------------");		

				// 데이트베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("Section :[" + mediacodeModel.RapCode + "]");
				_log.Debug("Section :[" + header.UserID + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				String RapCode = "0";
				//user레벨코드 생성
				if(header.UserID != null && !header.UserID.Equals(""))
				{
                    OracleParameter[] sqlParams = new OracleParameter[1];
					int i = 0;
					sbQuery.Append("\n"
                        + " SELECT ISNULL(a.REP_COD,'')    AS RapCode	\n"
                        + "   FROM STM_USER a LEFT JOIN STM_COD b ON (a.USER_LVL = b.STM_COD and b.STM_COD_CLS = '11')	\n"
                        + "                     LEFT JOIN STM_COD c ON (a.USER_CLS = c.STM_COD and c.STM_COD_CLS = '12')	\n"
                        + " WHERE USER_ID = @UserID   \n"
                        + "    AND USE_YN  = 'Y'	\n");

                    sqlParams[i] = new OracleParameter("@UserID", OracleDbType.Varchar2, 10);
					sqlParams[i++].Value = header.UserID;
					
					// __DEBUG__
					_log.Debug(sbQuery.ToString());

					DataSet dsID = new DataSet();
					_db.ExecuteQueryParams(dsID, sbQuery.ToString(), sqlParams);
				
					if (Utility.GetDatasetCount(dsID) == 0)
					{
						mediacodeModel.ResultCD = "2000"; // 해당 ID가 DB에 존재하지 않음
						mediacodeModel.ResultDesc = "해당하는 ID가 존재하지 않습니다.";
						dsID.Dispose();

						// 데이트베이스를  Close한다
						_db.Close();

						throw new FrameException("해당ID가 존재하지 않습니다.");
					}

					// RapCode를 가지고 온다.
					RapCode = Utility.GetDatasetString(dsID, 0, "RapCode");
					sbQuery.Length = 0;
				}
				_log.Debug("##############################"+ RapCode);

				// 쿼리생성
				sbQuery.Append("\n"
                    + " SELECT REP_COD, REP_NM  \n"
                    + "   FROM MDA_REP               \n"
                    + "   WHERE USE_YN = 'Y'         \n"					
					);

				// 코드분류가 선택했으면
				if (mediacodeModel.SearchKey.Trim().Length > 0)
				{
                    sbQuery.Append("  AND REP_NM    LIKE '%" + mediacodeModel.SearchKey.Trim() + "%' \n");
				}
				// 랩코드가 전체권한이 아니라면 해당하는 랩코드만 읽어온다.
				if (!RapCode.Equals("0"))
				{
                    sbQuery.Append("  AND REP_COD    = '" + RapCode + "' \n");
				}


                sbQuery.Append(" ORDER BY REP_COD \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				mediacodeModel.MediaRapCodeDataSet = ds.Copy();
				// 결과
				mediacodeModel.ResultCnt = Utility.GetDatasetCount(mediacodeModel.MediaRapCodeDataSet);
				// 결과코드 셋트
				mediacodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + mediacodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				if(!"2000".Equals(mediacodeModel.ResultCD))
				{
					mediacodeModel.ResultCD = "3000";
					mediacodeModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				}
				_log.Exception(ex);
				// 데이트베이스를  Close한다
				_db.Close();
			}
			// 데이트베이스를  Close한다
			_db.Close();
		}
	}
}
