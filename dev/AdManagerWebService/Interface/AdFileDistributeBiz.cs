// ===============================================================================
// ===============================================================================
// Release history
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Interface
{
	/// <summary>
	/// AreaDayBiz에 대한 요약 설명입니다.
	/// </summary>
	public class AdFileDistributeBiz : BaseBiz
	{
		#region  생성자
		public AdFileDistributeBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region [함수] 지역-요일별 집계
		/// <summary>
		/// CDN 배포요청데이터 리스트를 가져옵니다
		/// </summary>
		/// <param name="data"></param>
		public void GetFileDistributeList(AdFileDistributeModel data)
		{
			try
			{
				StringBuilder sbQuery = null;
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFileDistributeList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("StartDay :[" + data.BeginDate   + "]");
				_log.Debug("EndDay   :[" + data.EndDate     + "]");
				// __DEBUG__
                
				
				sbQuery = new StringBuilder();
				sbQuery.Append("\n select	b.ItemNo ");
				sbQuery.Append("\n 		,b.ItemName ");
				sbQuery.Append("\n 		,a.WorkDt ");
				sbQuery.Append("\n 		,a.FilePath ");
				sbQuery.Append("\n 		,a.FileName ");
				sbQuery.Append("\n 		,a.Cid ");
				sbQuery.Append("\n 		,a.RequestStatus ");
				sbQuery.Append("\n 		,a.ProcStatus ");
				sbQuery.Append("\n 		,a.SyncServer ");
				sbQuery.Append("\n 		,a.DescServer ");
				sbQuery.Append("\n 		,a.ResultDt ");
				sbQuery.Append("\n 		,a.ResultMsg ");
				sbQuery.Append("\n from	AdFileDistribute a with(noLock) ");
				sbQuery.Append("\n inner join ContractItem	 b with(noLock) on b.ItemNo = a.ItemNo ");
				sbQuery.Append("\n where	WorkDt between cast('" + data.BeginDate + "' as datetime) and cast('" + data.EndDate + "' as datetime) + 1.0 ");
				sbQuery.Append("\n order by a.WorkDt desc");

				_log.Debug(sbQuery.ToString());
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				data.ListDataSet = ds.Copy();

				// 결과
				data.ResultCnt = Utility.GetDatasetCount(data.ListDataSet);
				ds.Dispose();
				// 결과코드 셋트
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFileDistributeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "CDN배포요청데이터 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}
		#endregion
	}
}