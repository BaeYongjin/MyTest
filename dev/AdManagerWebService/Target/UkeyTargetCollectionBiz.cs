using System;
//�߰� ���ӽ����̽�
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
	/// UkeyTargetCollectionBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class UkeyTargetCollectionBiz : BaseBiz
	{

		#region [������]
		public UkeyTargetCollectionBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region [ķ���� ���� ����Ʈ]
		/// <summary>
		/// ķ���� ���� ����Ʈ 
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

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
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
											when Cmpgn_State = 1 then '�غ�'
										    when Cmpgn_State = 2 then '���'
											when Cmpgn_State = 3 then '����'
											when Cmpgn_State = 4 then '����'
											when Cmpgn_State = 4 then '����'
										  end  Cmpgn_State
										from dbo.Ukey_CampaignMaster with(NoLock)
								where 1=1 ");
				//�˻��� ��� �� ����. 

				if (ukeytargetcollectionModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.AppendFormat(" and cmpgn_nm like '%{0}%' ",ukeytargetcollectionModel.SearchKey.Trim());
				}

				sbQuery.Append(" Order by Cmpgn_Num desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ukeytargetcollectionModel �� ���� 

				ukeytargetcollectionModel.CampaingnMasterDataSet = ds.Copy();
				
				// ��� 
				ukeytargetcollectionModel.ResultCnt = Utility.GetDatasetCount(ukeytargetcollectionModel.CampaingnMasterDataSet);
				
				// ����ڵ� ��Ʈ
				ukeytargetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ukeytargetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignMasterList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD = "3000";
				ukeytargetcollectionModel.ResultDesc = "Ukey�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}

		}

		#endregion

		#region [ķ���� ��� ����Ʈ]
		/// <summary>
		/// ķ���� ��� ����Ʈ 
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

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
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
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.AppendFormat(" and Cust_Nm like '%{0}%' ",ukeytargetcollectionModel.SearchKey.Trim());
				}

				sbQuery.Append(" Order by Cmpgn_Num desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				ukeytargetcollectionModel.CampaingnItemDataSet = ds.Copy();

				// ��� 
				ukeytargetcollectionModel.ResultCnt = Utility.GetDatasetCount(ukeytargetcollectionModel.CampaingnItemDataSet);
				
				// ����ڵ� ��Ʈ
				ukeytargetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ukeytargetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD = "3000";
				ukeytargetcollectionModel.ResultDesc = "Ukey ������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally 
			{
				_db.Close();
			}
		}

		#endregion

		#region [ ����, ���� ����]

		/// <summary>
		/// ���� ����
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

				//�����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				
				StringBuilder sbQuery = new StringBuilder();

				sbQuery.AppendFormat(@" UPDATE 	Ukey_CampaignMaster
										SET		Cmpgn_State = 3
										WHERE		Cmpgn_Num = '{0}' 
											and Strtgy_Ts = {1} 
											and Cell_Num = '{2}' "
					,ukeytargetcollectionModel.CmpgnNum,ukeytargetcollectionModel.StrtgyTs,ukeytargetcollectionModel.CellNum);

				//��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ÿ�ٱ���������:["+ukeytargetcollectionModel.CmpgnNum + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				ukeytargetcollectionModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetRunStateUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD   = "3201";
				ukeytargetcollectionModel.ResultDesc = "Ukey ���� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			// �����ͺ��̽���  Close�Ѵ�
			_db.Close();
		}

		/// <summary>
		/// ���� ���� 
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

				//�����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				
				StringBuilder sbQuery = new StringBuilder();

				sbQuery.AppendFormat(@" UPDATE 	Ukey_CampaignMaster
										SET		Cmpgn_State = 4
										WHERE		Cmpgn_Num = '{0}' 
											and Strtgy_Ts = {1} 
											and Cell_Num = '{2}' "
									,ukeytargetcollectionModel.CmpgnNum,ukeytargetcollectionModel.StrtgyTs,ukeytargetcollectionModel.CellNum);

				 //��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ÿ�ٱ���������:["+ukeytargetcollectionModel.CmpgnNum + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				ukeytargetcollectionModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetStopStateUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD   = "3201";
				ukeytargetcollectionModel.ResultDesc = "Ukey ���� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			// �����ͺ��̽���  Close�Ѵ�
			_db.Close();
		}

		

		#endregion

		#region [���� ���� ����Ʈ]
		/// <summary>
		/// ķ���� ���� ���� ����Ʈ 
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

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
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

				//����� �˻� ������ ����

				sbQuery.Append(" Order by Cmpgn_Num desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				ukeytargetcollectionModel.CampaingnResultDataSet = ds.Copy();

				// ��� 
				ukeytargetcollectionModel.ResultCnt = Utility.GetDatasetCount(ukeytargetcollectionModel.CampaingnResultDataSet);
				
				// ����ڵ� ��Ʈ
				ukeytargetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ukeytargetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD = "3000";
				ukeytargetcollectionModel.ResultDesc = "Ukey �������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally 
			{
				_db.Close();
			}
		}

		#endregion


		#region �������� ����¡
		public void GetCampaignItemPageList(HeaderModel header, UkeyTargetCollectionModel ukeytargetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignItemPageList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
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
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.AppendFormat(" and Cust_Nm like '%{0}%' ",ukeytargetcollectionModel.SearchKey.Trim());
				}

				sbQuery.Append(" Order by Svc_Mgmt_Num desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				ukeytargetcollectionModel.CampaingnItemDataSet = ds.Copy();

				// ��� 
				ukeytargetcollectionModel.ResultCnt = Utility.GetDatasetCount(ukeytargetcollectionModel.CampaingnItemDataSet);
				
				// ����ڵ� ��Ʈ
				ukeytargetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + ukeytargetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignItemPageList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				ukeytargetcollectionModel.ResultCD = "3000";
				ukeytargetcollectionModel.ResultDesc = "Ukey ������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
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
