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
	public class CampaignCodeBiz : BaseBiz
	{
		public CampaignCodeBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  코드목록조회
		/// </summary>
		/// <param name="campaigncodeModel"></param>
		public void GetCampaignCodeList(HeaderModel header, CampaignCodeModel campaigncodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignCodeList() Start");
				_log.Debug("-----------------------------------------");

				// 데이트베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("Section :[" + campaigncodeModel.ContractSeq + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT CampaignCode, CampaignName  \n"
					+ "   FROM CampaignMaster with(NoLock)              \n"					
					+ "   WHERE MediaCode = '"+campaigncodeModel.MediaCode+"'         \n"					
					);

				// 코드분류가 선택했으면				
				if(!campaigncodeModel.ContractSeq.Equals("00"))
				{
					sbQuery.Append("  AND(  ContractSeq = '"+campaigncodeModel.ContractSeq+"') \n");
				}    

				sbQuery.Append(" ORDER BY CampaignCode \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				campaigncodeModel.CampaignCodeDataSet = ds.Copy();
				// 결과
				campaigncodeModel.ResultCnt = Utility.GetDatasetCount(campaigncodeModel.CampaignCodeDataSet);
				// 결과코드 셋트
				campaigncodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + campaigncodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				campaigncodeModel.ResultCD = "3000";
				campaigncodeModel.ResultDesc = "캠페인 코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);

				// 데이트베이스를  Close한다
				_db.Close();
			}
			// 데이트베이스를  Close한다
			_db.Close();
		}
	}
}
