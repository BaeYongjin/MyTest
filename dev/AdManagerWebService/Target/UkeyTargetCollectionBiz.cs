using System;
//추가 네임스페이스
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Target
{
	/// <summary>
	/// UkeyTargetCollectionBiz에 대한 요약 설명입니다.
	/// </summary>
	public class UkeyTargetCollectionBiz : BaseBiz
	{

		#region [생성자]
		public UkeyTargetCollectionBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region [캠페인 개요 리스트]
		/// <summary>
		/// 캠페인 개요 리스트 
		/// </summary>
		/// <param name="header"></param>
		/// <param name="ukeytargetcollectionModel"></param>
		public void GetCampaignMasterList(HeaderModel header, UkeyTargetCollectionModel ukeytargetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignMasterList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + ukeytargetcollectionModel.SearchKey       + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append(@" select 
										  Cmpgn_Num
										, Strtgy_Ts
										, Cell_Num
										, Cmpgn_Nm
										, Co_Cl_Cd
										, Cmpgn_Purp_Ept_Eft_Ctt
										, Cmpgn_Typ_Cd
										, Obj_Typ_Cd
										, CmpGn_St_Cd
										, Planr_Id
										, Planr_Nm
										, Mbl_Phon_Num
										, Obj_Cust_Cnt
										, Cmpgn_Exec_Sta_Dt
										, Cmpgn_Exec_End_Dt
										, Cell_Cnt
										, Iptv_Tmplt_Id
										, Iptv_Tmplt_Nm
										, case 
											when Cmpgn_State = 1 then '준비'
										    when Cmpgn_State = 2 then '대기'
											when Cmpgn_State = 3 then '실행'
											when Cmpgn_State = 4 then '정지'
											when Cmpgn_State = 4 then '종료'
										  end  Cmpgn_State
										from dbo.Ukey_CampaignMaster with(NoLock)
								where 1=1 ");
				//검색어 들어 갈 것임. 

				if (ukeytargetcollectionModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.AppendFormat(" and cmpgn_nm like '%{0}%' ",ukeytargetcollectionModel.SearchKey.Trim());
				}

				sbQuery.Append(" Order by Cmpgn_Num desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 ukeytargetcollectionModel 에 복사 

				ukeytargetcollectionModel.CampaingnMasterDataSet = ds.Copy();
				
				// 결과 
				ukeytargetcollectionModel.ResultCnt = Utility.GetDatasetCount(ukeytargetcollectionModel.CampaingnMasterDataSet);
				
				// 결과코드 셋트
				ukeytargetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + ukeytargetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignMasterList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD = "3000";
				ukeytargetcollectionModel.ResultDesc = "Ukey고객군정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}

		}

		#endregion

		#region [캠페인 대상 리스트]
		/// <summary>
		/// 캠페인 대상 리스트 
		/// </summary>
		/// <param name="header"></param>
		/// <param name="ukeytargetcollectionModel"></param>
		public void GetCampaignItemList(HeaderModel header, UkeyTargetCollectionModel ukeytargetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignItemList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + ukeytargetcollectionModel.SearchKey       + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				sbQuery.Append(@" SELECT Cmpgn_Num
									,Strtgy_Ts
									,Cell_Num
									,Cmpgn_Obj_Num
									,Extrt_Seq
									,Svc_Mgmt_Num
									,Cust_Nm
									,Psnl_Text
									,Mod_Dt
								FROM Ukey_CampaignItem with(NoLock)
								where 1=1 ");


				if(ukeytargetcollectionModel.CmpgnNum.Trim().Length > 0 &&  ukeytargetcollectionModel.StrtgyTs.Trim().Length > 0 && ukeytargetcollectionModel.CellNum.Trim().Length > 0 )
				{
					sbQuery.AppendFormat(" and Cmpgn_Num ='{0}' and  Strtgy_Ts = {1} and Cell_Num = '{2}' ",ukeytargetcollectionModel.CmpgnNum.Trim(),ukeytargetcollectionModel.StrtgyTs.Trim(),ukeytargetcollectionModel.CellNum.Trim());
				}

				



				if (ukeytargetcollectionModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.AppendFormat(" and Cust_Nm like '%{0}%' ",ukeytargetcollectionModel.SearchKey.Trim());
				}

				sbQuery.Append(" Order by Cmpgn_Num desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				ukeytargetcollectionModel.CampaingnItemDataSet = ds.Copy();

				// 결과 
				ukeytargetcollectionModel.ResultCnt = Utility.GetDatasetCount(ukeytargetcollectionModel.CampaingnItemDataSet);
				
				// 결과코드 셋트
				ukeytargetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + ukeytargetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD = "3000";
				ukeytargetcollectionModel.ResultDesc = "Ukey 대상정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally 
			{
				_db.Close();
			}
		}

		#endregion

		#region [ 실행, 정지 설정]

		/// <summary>
		/// 실행 설정
		/// </summary>
		/// <param name="header"></param>
		/// <param name="ukeytargetcollectionModel"></param>
		public void SetRunStateUpdate(HeaderModel header, UkeyTargetCollectionModel ukeytargetcollectionModel)
		{
			try
			{
				int rc = 0;

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetRunStateUpdate() Start");
				_log.Debug("-----------------------------------------");

				//데이터베이스를 OPEN한다
				_db.Open();
				
				StringBuilder sbQuery = new StringBuilder();

				sbQuery.AppendFormat(@" UPDATE 	Ukey_CampaignMaster
										SET		Cmpgn_State = 3
										WHERE		Cmpgn_Num = '{0}' 
											and Strtgy_Ts = {1} 
											and Cell_Num = '{2}' "
					,ukeytargetcollectionModel.CmpgnNum,ukeytargetcollectionModel.StrtgyTs,ukeytargetcollectionModel.CellNum);

				//쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("타겟군정보수정:["+ukeytargetcollectionModel.CmpgnNum + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				ukeytargetcollectionModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetRunStateUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD   = "3201";
				ukeytargetcollectionModel.ResultDesc = "Ukey 상태 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			// 데이터베이스를  Close한다
			_db.Close();
		}

		/// <summary>
		/// 정지 설정 
		/// </summary>
		/// <param name="header"></param>
		/// <param name="ukeytargetcollectionModel"></param>
		public void SetStopStateUpdate(HeaderModel header, UkeyTargetCollectionModel ukeytargetcollectionModel)
		{
			try
			{
				int rc = 0;

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetStopStateUpdate() Start");
				_log.Debug("-----------------------------------------");

				//데이터베이스를 OPEN한다
				_db.Open();
				
				StringBuilder sbQuery = new StringBuilder();

				sbQuery.AppendFormat(@" UPDATE 	Ukey_CampaignMaster
										SET		Cmpgn_State = 4
										WHERE		Cmpgn_Num = '{0}' 
											and Strtgy_Ts = {1} 
											and Cell_Num = '{2}' "
									,ukeytargetcollectionModel.CmpgnNum,ukeytargetcollectionModel.StrtgyTs,ukeytargetcollectionModel.CellNum);

				 //쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("타겟군정보수정:["+ukeytargetcollectionModel.CmpgnNum + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				ukeytargetcollectionModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetStopStateUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD   = "3201";
				ukeytargetcollectionModel.ResultDesc = "Ukey 상태 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			// 데이터베이스를  Close한다
			_db.Close();
		}

		

		#endregion

		#region [반응 정보 리스트]
		/// <summary>
		/// 캠페인 반응 정보 리스트 
		/// </summary>
		/// <param name="header"></param>
		/// <param name="ukeytargetcollectionModel"></param>
		public void GetCampaignResultList(HeaderModel header, UkeyTargetCollectionModel ukeytargetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignResultList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + ukeytargetcollectionModel.SearchKey       + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				sbQuery.Append(@" SELECT Cmpgn_Num
									,Strtgy_Ts
									,Cell_Num
									,Cmpgn_Obj_Num
									,Extrt_Seq
									,Cont_Dt
									,Cont_Tm
									,Ract_Typ_Cd
								FROM Ukey_CampaignItemResult with(NoLock)
								where 1=1 ");


				if(ukeytargetcollectionModel.CmpgnNum.Trim().Length > 0 &&  ukeytargetcollectionModel.StrtgyTs.Trim().Length > 0 && ukeytargetcollectionModel.CellNum.Trim().Length > 0 )
				{
					sbQuery.AppendFormat(" and Cmpgn_Num ='{0}' and  Strtgy_Ts = {1} and Cell_Num = '{2}' ",ukeytargetcollectionModel.CmpgnNum.Trim(),ukeytargetcollectionModel.StrtgyTs.Trim(),ukeytargetcollectionModel.CellNum.Trim());
				}

				//현재는 검색 기준이 없음

				sbQuery.Append(" Order by Cmpgn_Num desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				ukeytargetcollectionModel.CampaingnResultDataSet = ds.Copy();

				// 결과 
				ukeytargetcollectionModel.ResultCnt = Utility.GetDatasetCount(ukeytargetcollectionModel.CampaingnResultDataSet);
				
				// 결과코드 셋트
				ukeytargetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + ukeytargetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD = "3000";
				ukeytargetcollectionModel.ResultDesc = "Ukey 반응정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally 
			{
				_db.Close();
			}
		}

		#endregion


		#region 반응정보 페이징
		public void GetCampaignItemPageList(HeaderModel header, UkeyTargetCollectionModel ukeytargetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignItemPageList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + ukeytargetcollectionModel.SearchKey       + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				sbQuery.AppendFormat(@" SELECT top {0} 
									 Cmpgn_Num
									,Strtgy_Ts
									,Cell_Num
									,Cmpgn_Obj_Num
									,Extrt_Seq
									,Svc_Mgmt_Num
									,Cust_Nm
									,Psnl_Text
									,Mod_Dt
								FROM Ukey_CampaignItem with(NoLock)
								where 1=1 
								and Svc_Mgmt_Num not in
								(
									select top (( {1} - 1 ) * {0}) Svc_Mgmt_Num  from Ukey_CampaignItem order by Svc_Mgmt_Num desc
								) ",ukeytargetcollectionModel.PageSize.Trim(),ukeytargetcollectionModel.Page.Trim() );


				if(ukeytargetcollectionModel.CmpgnNum.Trim().Length > 0 &&  ukeytargetcollectionModel.StrtgyTs.Trim().Length > 0 && ukeytargetcollectionModel.CellNum.Trim().Length > 0 )
				{
					sbQuery.AppendFormat(" and Cmpgn_Num ='{0}' and  Strtgy_Ts = {1} and Cell_Num = '{2}' ",ukeytargetcollectionModel.CmpgnNum.Trim(),ukeytargetcollectionModel.StrtgyTs.Trim(),ukeytargetcollectionModel.CellNum.Trim());
				}


				if (ukeytargetcollectionModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.AppendFormat(" and Cust_Nm like '%{0}%' ",ukeytargetcollectionModel.SearchKey.Trim());
				}

				sbQuery.Append(" Order by Svc_Mgmt_Num desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				ukeytargetcollectionModel.CampaingnItemDataSet = ds.Copy();

				// 결과 
				ukeytargetcollectionModel.ResultCnt = Utility.GetDatasetCount(ukeytargetcollectionModel.CampaingnItemDataSet);
				
				// 결과코드 셋트
				ukeytargetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + ukeytargetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignItemPageList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD = "3000";
				ukeytargetcollectionModel.ResultDesc = "Ukey 대상정보 조회중 오류가 발생하였습니다";
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
