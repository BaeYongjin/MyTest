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
	/// SchAppendAdBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchAppendAdBiz : BaseBiz
	{
		#region ������
		public SchAppendAdBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region Ȩ��������Ȳ��ȸ
		/// <summary>
		/// Ȩ��������Ȳ��ȸ
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		public void GetSchAppendAdList(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSchAppendAdList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	     :[" + schAppendAdModel.SearchMediaCode	   + "]");		// �˻� ��ü

				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ "SELECT B.MediaCode                   \n"
					+ "      ,B.AdType                      \n"
					+ "      ,H.CodeName AS AdTypeName      \n"
					+ "      ,A.ScheduleOrder               \n"
					+ "      ,A.ItemNo                      \n"
					+ "      ,B.ItemName                    \n"
					+ "      ,C.ContractName                \n"
					+ "      ,D.AdvertiserName              \n"
					+ "      ,C.State AS ContState          \n"
					+ "      ,E.CodeName AS ContStateName   \n"
					+ "      ,B.RealEndDay                  \n"
					+ "      ,F.CodeName AS AdClassName     \n"
					+ "      ,B.AdState                     \n"
					+ "      ,G.CodeName AS AdStatename     \n"
					+ "      ,B.FileState                   \n"
					+ "      ,I.CodeName AS FileStatename   \n"
					+ "      ,'False' AS CheckYn            \n"
					+ "      ,J.State AS AckState           \n"
					+ "      ,CASE K.TgtTimeYn WHEN 'Y' THEN '����' WHEN 'N' THEN '����' ELSE '' END AS TgtTimeYn           \n"
					+ "      ,M.CodeName AS JumpTypeName           \n"
					+ "  FROM SchAppend A with(NoLock) INNER JOIN ContractItem B  with(NoLock) ON (A.ItemNo         = B.ItemNo)                     \n"
					+ "                 INNER JOIN Contract     C  with(NoLock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq) \n"
					+ "                 LEFT  JOIN Advertiser   D  with(NoLock) ON (B.AdvertiserCode = D.AdvertiserCode)             \n"
					+ "                 LEFT  JOIN SystemCode   E  with(NoLock) ON (C.State          = E.Code AND E.Section = '23')  \n"	// 23 : ������
					+ "                 LEFT  JOIN SystemCode   F  with(NoLock) ON (B.AdClass        = F.Code AND F.Section = '29')  \n"	// 29 : ����뵵
					+ "                 LEFT  JOIN SystemCode   G  with(NoLock) ON (B.AdState        = G.Code AND G.Section = '25')  \n"	// 25 : �������
					+ "                 LEFT  JOIN SystemCode   H  with(NoLock) ON (B.AdType         = H.Code AND H.Section = '26')  \n"	// 26 : ��������
					+ "                 LEFT  JOIN SystemCode   I  with(NoLock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 31 : ���ϻ���
					+ "                 LEFT  JOIN SchPublish   J  with(NoLock) ON (A.AckNo          = J.AckNo)                      \n"
					+ "                 LEFT  JOIN TargetingHome    K  with(NoLock) ON (A.ItemNo          = K.ItemNo)                      \n"
					+ "                 LEFT  JOIN ChannelJump    L  with(NoLock) ON (A.ItemNo          = L.ItemNo)                      \n"
					+ "                 LEFT  JOIN SystemCode    M with(NoLock) ON (L.JumpType       = M.Code        AND M.Section   = '34' )                      \n"
					+ " WHERE 1 = 1   \n"
					);

				// ��ü�� ����������
				if(schAppendAdModel.SearchMediaCode.Trim().Length > 0 && !schAppendAdModel.SearchMediaCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode  = " + schAppendAdModel.SearchMediaCode.Trim() + " \n");
				}	
				
       
				sbQuery.Append("  ORDER BY A.ScheduleOrder ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �帣�׷�𵨿� ����
				schAppendAdModel.ScheduleDataSet = ds.Copy();

				//�����޴����� ������  Order�� ����
				string LastOrder = "1";
				sbQuery = new StringBuilder();
				sbQuery.Append( "\n"
					+ "SELECT ISNULL(MAX(ScheduleOrder),0) AS MaxOrder   \n"
					+ "  FROM SchAppend with(NoLock)                              \n"
					+ " WHERE MediaCode = " + schAppendAdModel.SearchMediaCode + " \n"
					);

				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					LastOrder = Utility.GetDatasetString(ds, 0, "MaxOrder");					
				}
				schAppendAdModel.LastOrder = LastOrder;
				ds.Dispose();

				// ���
				schAppendAdModel.ResultCnt = Utility.GetDatasetCount(schAppendAdModel.ScheduleDataSet);
				// ����ڵ� ��Ʈ
				schAppendAdModel.ResultCD = "0000";


				// 2007.10.02 RH.Jun ���ϸ���Ʈ �Ǽ��˻��
				// 2007.10.10 RH.Jung Ȩ���� ����Ʈ �ջ�� ���ϻ��°� ��ž������ �ƴѰ����� ����

				sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n"
					+ "SELECT (                                                               \n"
					+ "  SELECT COUNT(*) AS HomeCnt                                           \n"
					+ "    FROM SchAppend A  with(NoLock) INNER JOIN ContractItem B  with(NoLock) ON (A.ItemNo = B.ItemNo)  \n"
					+ "     AND B.ExcuteStartDay	<= Convert(varchar(8),getdate(),112)      \n"
					+ "     AND B.RealEndDay		>= Convert(varchar(8),getdate(),112)      \n"
					+ "     AND B.AdState   = '20'    \n"            
					+ "     AND B.FileState < '90'    \n"
					+ ") + (                          \n"
					+ "  SELECT COUNT(*) AS FileCnt   \n"
					+ "    FROM ContractItem with(NoLock)          \n"
					+ "   WHERE FileState = '30'      \n"
					+ ") AS FileListCnt               \n"
					);           

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				schAppendAdModel.FileListCount = Convert.ToInt32(ds.Tables[0].Rows[0]["FileListCnt"].ToString());
				ds.Dispose();

				// 2007.10.02 

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schAppendAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSchAppendAdList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD = "3000";
				schAppendAdModel.ResultDesc = "Ȩ��������Ȳ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}

		#endregion

		#region �߰������������ȸ

		/// <summary>
		/// �߰�����
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		public void GetContractItemList(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey            :[" + schAppendAdModel.SearchKey            + "]");		// �˻���
				_log.Debug("SearchMediaCode	     :[" + schAppendAdModel.SearchMediaCode	   + "]");		// �˻� ��ü

				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn            \n"
					+ "      ,A.ItemNo                      \n"
					+ "      ,A.ItemName                    \n"
					+ "      ,B.ContractName                \n"
					+ "      ,C.AdvertiserName              \n"
					+ "      ,A.ExcuteStartDay              \n"
					+ "      ,A.ExcuteEndDay                \n"
					+ "      ,A.RealEndDay                  \n"
					+ "      ,A.AdState                     \n"
					+ "      ,D.CodeName AdStateName        \n"
					+ "      ,(SELECT COUNT(*) FROM SchAppend with(NoLock)                 WHERE ItemNo = A.ItemNo) AS HomeCount      \n"
					+ "      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail with(NoLock)     WHERE ItemNo = A.ItemNo) AS MenuCount      \n"
					+ "      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail with(NoLock)  WHERE ItemNo = A.ItemNo) AS ChannelCount   \n"
					+ "      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay                                                \n"
					+ "      ,A.AdType                      \n"
					+ "      ,E.CodeName AS AdTypeName      \n"
					+ "  FROM ContractItem A with(NoLock) INNER JOIN Contract   B with(NoLock) ON (B.MediaCode      = B.MediaCode AND A.RapCode = B.RapCode AND A.AgencyCode = B.AgencyCode AND A.AdvertiserCode = B.AdvertiserCode AND A.ContractSeq = B.ContractSeq) \n"
					+ "                       LEFT JOIN Advertiser C with(NoLock) ON (B.AdvertiserCode = C.AdvertiserCode)                \n"
					+ "	                      LEFT JOIN SystemCode D with(NoLock) ON (A.AdState        = D.Code      AND D.Section = '25')  \n"  // 25 : �������
					+ "                       LEFT JOIN SystemCode E with(NoLock) ON (A.AdType         = E.Code      AND E.Section = '26')  \n"	// 26 : ��������
					+ " WHERE 1=1   \n"    
					+ "   AND A.AdType  = '14'   \n"    
					);

				// ��ü�� ����������
				if(schAppendAdModel.SearchMediaCode.Trim().Length > 0 && !schAppendAdModel.SearchMediaCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode  = '" + schAppendAdModel.SearchMediaCode.Trim() + "' \n");
				}	

				bool isState = false;
				// ������� ���ÿ� ����

				// ������� �غ�
				if(schAppendAdModel.SearchchkAdState_10.Trim().Length > 0 && schAppendAdModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND ( A.AdState  = '10' \n");
					isState = true;
				}	
				// ������� ��
				if(schAppendAdModel.SearchchkAdState_20.Trim().Length > 0 && schAppendAdModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '20' \n");
					isState = true;
				}	
				// ������� ����
				if(schAppendAdModel.SearchchkAdState_30.Trim().Length > 0 && schAppendAdModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '30' \n");
					isState = true;
				}	
				// ������� ����
				if(schAppendAdModel.SearchchkAdState_40.Trim().Length > 0 && schAppendAdModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				
				// �˻�� ������
				if (schAppendAdModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append("\n"
						+ "  AND ( A.ItemName       LIKE '%" + schAppendAdModel.SearchKey.Trim() + "%' \n"
						+ "     OR B.ContractName   LIKE '%" + schAppendAdModel.SearchKey.Trim() + "%' \n"
						+ "  ) \n"
						);
				}
       
				sbQuery.Append("  ORDER BY A.ItemNo Desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �帣�׷�𵨿� ����
				schAppendAdModel.ScheduleDataSet = ds.Copy();
				// ���
				schAppendAdModel.ResultCnt = Utility.GetDatasetCount(schAppendAdModel.ScheduleDataSet);
				// ����ڵ� ��Ʈ
				schAppendAdModel.ResultCD = "0000";


				// 2007.10.02 RH.Jun ���ϸ���Ʈ �Ǽ��˻��

				sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n"
					+ "SELECT (                                                               \n"
					+ "  SELECT COUNT(*) AS HomeCnt                                           \n"
					+ "    FROM SchAppend A INNER JOIN ContractItem B ON (A.ItemNo = B.ItemNo)  \n"
					+ "     AND B.ExcuteStartDay	<= Convert(varchar(8),getdate(),112)      \n"
					+ "     AND B.RealEndDay		>= Convert(varchar(8),getdate(),112)      \n"
					+ "     AND B.AdState   = '20'    \n"            
					+ "     AND B.FileState = '30'    \n"
					+ ") + (                          \n"
					+ "  SELECT COUNT(*) AS FileCnt   \n"
					+ "    FROM ContractItem          \n"
					+ "   WHERE FileState = '30'      \n"
					+ ") AS FileListCnt               \n"
					);           

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				schAppendAdModel.FileListCount = Convert.ToInt32(ds.Tables[0].Rows[0]["FileListCnt"].ToString());
				ds.Dispose();


				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schAppendAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD = "3000";
				schAppendAdModel.ResultDesc = "Ȩ��������Ȳ ��ȸ �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}

		#endregion

		#region  Ȩ������ ����� ��ü row ���������Ͻ� ��������
		/// <summary>
		/// Ȩ������ ����� ��ü row ���������Ͻ� ��������
		/// </summary>
		/// <returns></returns>
		private void SetLastUpdate(string MediaCode)
		{
			string sQuery = "\n"
				+ "UPDATE SchAppend             \n"
				+ "   SET ModDt   = GETDATE() \n"
				+ " WHERE MediaCode = " + MediaCode + " \n"
				;

			int rc =  _db.ExecuteNonQuery(sQuery);
		}

		#endregion

		#region �߰������� ����
		/// <summary>
		/// �߰�������
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdCreate(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdCreate() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int i = 0;
					int rc = 0;

					// ���� ���ι�ȣ�� ����
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					StringBuilder  sbQuery   = new StringBuilder();
					SqlParameter[] sqlParams = new SqlParameter[1];
		
					i = 0;
					sqlParams[i++] = new SqlParameter("@ItemNo"        , SqlDbType.Int          );

					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(schAppendAdModel.ItemNo);

					_db.BeginTran();

					// Ȩ���� �� ���̺� �߰�
					sbQuery.Append( "\n"
						+ "INSERT INTO SchAppend (                   \n"
						+ "       MediaCode                        \n"
						+ "      ,ScheduleOrder                    \n"
						+ "      ,ItemNo                           \n"
						+ "      ,ModDt                            \n"
						+ "      ,AckNo                            \n"
						+ "      )                                 \n"
						+ " SELECT                                 \n"					
						+ "       " + schAppendAdModel.MediaCode + " \n"
						+ "      ,ISNULL(MAX(ScheduleOrder),0)+1   \n"
						+ "      ,@ItemNo                          \n"
						+ "      ,GETDATE()                        \n"
						+ "      ," + AckNo                    + " \n"
						+ "  FROM SchAppend                          \n"
						+ " WHERE MediaCode = " + schAppendAdModel.MediaCode + " \n"
						);

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// Ȩ���� �� ���̺� �߰�
					sbQuery   = new StringBuilder();
					
					sbQuery.Append( "\n"
						+ "UPDATE ContractItem        \n"
						+ "   SET AdState = '20'      \n"   // ������� - 20:��
						+ "      ,ModDt   = GETDATE() \n"
						+ "      ,RegID   = '" + header.UserID + "' \n" 
						+ " WHERE ItemNo  = @ItemNo   \n"
						);

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// ���������Ͻ� ��Ʈ
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// ���� ����
					string LastOrder = "1";

					// �ش� ���� ���� ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder    \n"
						+ "   FROM SchAppend                                      \n"
						+ "  WHERE MediaCode = " + schAppendAdModel.MediaCode + " \n"
						);

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						LastOrder = Utility.GetDatasetString(ds, 0, "LastOrder");					
					}

					ds.Dispose();

					schAppendAdModel.ScheduleOrder = LastOrder;

					// __MESSAGE__
					_log.Message("Ȩ������:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = "Ȩ������ ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion 

		#region  Ȩ������ ����
		/// <summary>
		/// Ȩ������  ����
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdDelete(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdDelete() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int i = 0;
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();
					SqlParameter[] sqlParams = new SqlParameter[2];
	
					i = 0;
					sqlParams[i++] = new SqlParameter("@ItemNo"          , SqlDbType.Int          );
					sqlParams[i++] = new SqlParameter("@ScheduleOrder"   , SqlDbType.Int          );

					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(schAppendAdModel.ItemNo);
					sqlParams[i++].Value = Convert.ToInt32(schAppendAdModel.ScheduleOrder);

					_db.BeginTran();

					// 2007.10.11 RH.Jung �����ÿ��� �����ι�ȣ�� �����ϱ� ����
					// ���� ���ι�ȣ�� ����
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					// Ȩ���� �� ���̺� �߰�
					sbQuery.Append( "\n"
						+ "DELETE SchAppend                        \n"
						+ " WHERE ItemNo        = @ItemNo        \n"
						+ "   AND ScheduleOrder = @ScheduleOrder \n"
						);

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// ������ �������� ���� ����
					sbQuery   = new StringBuilder();

					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                            \n"
						+ "   SET ScheduleOrder = ScheduleOrder - 1  \n"
						+ " WHERE ScheduleOrder > " + schAppendAdModel.ScheduleOrder + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode     + " \n"	
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// ���������Ͻ� ��Ʈ
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// ���� ����
					string LastOrder = "1";

					// �ش� ���� ���� ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder           \n"
						+ "  FROM SchAppend                                              \n"
						+ " WHERE ScheduleOrder < " + schAppendAdModel.ScheduleOrder + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode     + " \n"
						);

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						LastOrder = Utility.GetDatasetString(ds, 0, "LastOrder");					
					}

					ds.Dispose();

					schAppendAdModel.ScheduleOrder = LastOrder;

					// __MESSAGE__
					_log.Message("Ȩ������ ����:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = "Ȩ���� ������ ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}

		#endregion



		#region Ȩ������ ù��° ������
		/// <summary>
		/// Ȩ������  ù��° ������
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdOrderFirst(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderFirst() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = null;

					// ���� ���ι�ȣ�� ����
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					// ������ ����
					string ToOrder = "1"; 

					// �ش� ��ü�� MIN��
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS MinOrder     \n"
						+ "   FROM SchAppend                                      \n"
						+ "  WHERE MediaCode = " + schAppendAdModel.MediaCode + " \n"
						);

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "MinOrder");					
					}

					ds.Dispose();


					_db.BeginTran();

					// �ش� ������ 0������ �ӽú���
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                \n"
						+ "   SET ScheduleOrder = 0                      \n"
						+ "      ,AckNo  = " + AckNo                 + " \n"
						+ " WHERE ItemNo = " + schAppendAdModel.ItemNo + " \n"
						+ "   AND ScheduleOrder = " + schAppendAdModel.ScheduleOrder + " \n"
						);

					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					// ������ �ش� �������� ���� ������ �������� +1�Ͽ� ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                            \n"
						+ "   SET ScheduleOrder = ScheduleOrder + 1  \n"
						+ "      ,AckNo         = " + AckNo      + " \n"
						+ " WHERE MediaCode     = " + schAppendAdModel.MediaCode     + " \n"					     
						+ "   AND ScheduleOrder < " + schAppendAdModel.ScheduleOrder + " \n"
						+ "   AND ScheduleOrder > 0                  \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// �ش� ������  ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                       \n"
						+ "   SET ScheduleOrder = " + ToOrder               + " \n"
						+ "      ,AckNo         = " + AckNo                 + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo + " \n"
						+ "   AND ScheduleOrder = 0  \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// ���������Ͻ� ��Ʈ
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ȩ������ ù��° ������ ����:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] �����:[" + header.UserID + "]");
					schAppendAdModel.ScheduleOrder = ToOrder.ToString();  // ���� ROW�� ã������ ��Ʈ�ѷ� �����ִ� Ű��
					_log.Debug("ScheduleOrder:[" + schAppendAdModel.ScheduleOrder + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // ����
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderFirst() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = " Ȩ������  ù��° ������ ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion

		#region Ȩ������ �����ø�
		/// <summary>
		/// Ȩ������  �����ø�
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdOrderUp(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderUp() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("MediaCode     :[" + schAppendAdModel.MediaCode           + "]");		// �˻���
				_log.Debug("ScheduleOrder :[" + schAppendAdModel.ScheduleOrder	   + "]");		// �˻� ��ü
				_log.Debug("ItemNo        :[" + schAppendAdModel.ItemNo       + "]");		// �˻� ��
				// __DEBUG__

				
				// ��������
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// ���� ���ι�ȣ�� ����
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					// ������ ����
					string ToOrder = "1";

					// �ش� ������ ���� ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS UpOrder               \n"
						+ "  FROM SchAppend                                                \n"
						+ " WHERE ScheduleOrder < " + schAppendAdModel.ScheduleOrder   + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode       + " \n"
						);

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "UpOrder");					
					}

					ds.Dispose();


					_db.BeginTran();

					// �ش� ������ 0������ �ӽú���
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                \n"
						+ "   SET ScheduleOrder = 0                      \n"
						+ "      ,AckNo  = " + AckNo                 + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo        + " \n"
						+ "   AND ScheduleOrder = " + schAppendAdModel.ScheduleOrder + " \n"
						);

					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					// ������ �ش� �������� ������ ������ ������ �ش� ������ ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                               \n"
						+ "   SET ScheduleOrder = " + schAppendAdModel.ScheduleOrder  + " \n"
						+ "      ,AckNo         = " + AckNo                         + " \n"
						+ " WHERE ScheduleOrder = " + ToOrder                       + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode      + " \n"	
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// �ش� ������  ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                       \n"
						+ "   SET ScheduleOrder = " + ToOrder               + " \n"
						+ "      ,AckNo         = " + AckNo                 + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo + " \n"
						+ "   AND ScheduleOrder = 0 \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// ���������Ͻ� ��Ʈ
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ȩ������ �����ø� ����:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] ����:[" + schAppendAdModel.ScheduleOrder + ">" + ToOrder + "] �����:[" + header.UserID + "]");
					schAppendAdModel.ScheduleOrder = ToOrder.ToString();  // ���� ROW�� ã������ ��Ʈ�ѷ� �����ִ� Ű��

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // ����
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderUp() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = " Ȩ������ �����ø� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion

		#region Ȩ������  ��������
		/// <summary>
		/// Ȩ������  ��������
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdOrderDown(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderDown() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// ���� ���ι�ȣ�� ����
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					// ������ ����
					string ToOrder = "1";

					// �ش� ������ ���� ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MIN(ScheduleOrder),1) AS DownOrder           \n"
						+ "  FROM SchAppend                                              \n"
						+ " WHERE ScheduleOrder > " + schAppendAdModel.ScheduleOrder + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode     + " \n"
						);

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "DownOrder");					
					}

					ds.Dispose();
 		
					_db.BeginTran();

					// �ش� ������ 0������ �ӽú���
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                \n"
						+ "   SET ScheduleOrder = 0                      \n"
						+ "      ,AckNo         = " + AckNo          + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo        + " \n"
						+ "   AND ScheduleOrder = " + schAppendAdModel.ScheduleOrder + " \n"
						);

					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					// ������ �ش� �������� ������ ������ ������ �ش� ������ ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                              \n"
						+ "   SET ScheduleOrder = " + schAppendAdModel.ScheduleOrder + " \n"
						+ "      ,AckNo         = " + AckNo                        + " \n"
						+ " WHERE ScheduleOrder = " + ToOrder                      + " \n"
						+ "   AND MediaCode     = " + schAppendAdModel.MediaCode     + " \n"	
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// �ش� ������  ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                \n"
						+ "   SET ScheduleOrder = " + ToOrder        + " \n"
						+ "      ,AckNo         = " + AckNo          + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo + " \n"
						+ "   AND ScheduleOrder = 0  \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// ���������Ͻ� ��Ʈ
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ȩ������ �������� ����:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] ����:[" + schAppendAdModel.ScheduleOrder + ">" + ToOrder + "]  �����:[" + header.UserID + "]");
					schAppendAdModel.ScheduleOrder = ToOrder.ToString();  // ���� ROW�� ã������ ��Ʈ�ѷ� �����ִ� Ű��

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // ����
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderDown() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = " Ȩ������ �������� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion

		#region Ȩ������  ������ ������

		/// <summary>
		/// Ȩ������  ������ ������
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdOrderLast(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderLast() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();

					// ���� ���ι�ȣ�� ����
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					// ������ ����
					string ToOrder = "1";

					// �ش� ������ ���� ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder   \n"
						+ "  FROM SchAppend                                      \n"
						+ " WHERE MediaCode = " + schAppendAdModel.MediaCode + " \n"
						);

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						ToOrder = Utility.GetDatasetString(ds, 0, "LastOrder");					
					}

					ds.Dispose();

					_db.BeginTran();

					// �ش� ������ 0������ �ӽú���
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                \n"
						+ "   SET ScheduleOrder = 0                      \n"
						+ "      ,AckNo         = " + AckNo          + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo        + " \n"
						+ "   AND ScheduleOrder = " + schAppendAdModel.ScheduleOrder + " \n"
						);

					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					// �ش� �������� ū ������ �������� -1�Ͽ� ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                            \n"
						+ "   SET ScheduleOrder = ScheduleOrder - 1  \n"
						+ "      ,AckNo         = " + AckNo      + " \n"
						+ " WHERE MediaCode     = " + schAppendAdModel.MediaCode     + " \n"					     
						+ "   AND ScheduleOrder > " + schAppendAdModel.ScheduleOrder + " \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// �ش� ������  ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ "UPDATE SchAppend                                       \n"
						+ "   SET ScheduleOrder = " + ToOrder               + " \n"
						+ "      ,AckNo         = " + AckNo                 + " \n"
						+ " WHERE ItemNo        = " + schAppendAdModel.ItemNo + " \n"
						+ "   AND ScheduleOrder = 0 \n"
						);

					rc = _db.ExecuteNonQuery(sbQuery.ToString());

					// ���������Ͻ� ��Ʈ
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ȩ������ ������ ������ ����:[" + schAppendAdModel.ItemName + "] �����:[" + header.UserID + "]");
					schAppendAdModel.ScheduleOrder = ToOrder.ToString();  // ���� ROW�� ã������ ��Ʈ�ѷ� �����ִ� Ű��
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // ����
				
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdOrderLast() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = " Ȩ������ ������ ������ ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion 


		#region Ȩ�������� ����
		/// <summary>
		/// Ȩ�������� ����
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdDelete_To(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();
                        
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[1];
                        
				sbQuery.Append( ""
					+ "        DELETE FROM  SchAppend			                    \n"
					+ "              WHERE MediaCode = @MediaCode				\n"					
					);                             
                                        
				sqlParams[i++] = new SqlParameter("@MediaCode" , SqlDbType.TinyInt);				
			        
				i = 0;				
				sqlParams[i++].Value = schAppendAdModel.MediaCode;				

				_log.Debug("MediaCode:[" + schAppendAdModel.MediaCode + "]");				
				
				_log.Debug(sbQuery.ToString());
                        
				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();
                        
					// __MESSAGE__
					_log.Message("Ȩ������������:[" + schAppendAdModel.MediaCode + "] �����:[" + header.UserID + "]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				schAppendAdModel.ResultCD = "0000";  // ����
                        	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchAppendAdDelete_To() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3301";
				schAppendAdModel.ResultDesc = "Ȩ�������� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}
		#endregion

		#region Ȩ�������� ��ȸ(���� �������� ���°͸� �μ�Ʈ ��Ű�� ����)
		/// <summary>
		/// Ȩ��������ȸ
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		public void SetSchAppendSearch(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchAppendSearch() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT ItemNo  \n"							
					+ "			   ,ScheduleOrder  \n"							
					+ "		FROM SchAppend                \n"					
					);
	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ī�װ��𵨿� ����
				schAppendAdModel.ScheduleDataSet = ds.Copy();
				// ���
				schAppendAdModel.ResultCnt = Utility.GetDatasetCount(schAppendAdModel.ScheduleDataSet);
				// ����ڵ� ��Ʈ
				schAppendAdModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schAppendAdModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSchAppendSearch() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD = "3000";
				schAppendAdModel.ResultDesc = "Ȩ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}
		#endregion

		#region Ȩ�������� ����
		/// <summary>
		/// Ȩ�������� ����
		/// </summary>
		/// <returns></returns>
		public void SetSchAppendAdCreate_To(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdCreate_To() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int i = 0;
					int rc = 0;

					// ���� ���ι�ȣ�� ����
					string AckNo = GetLastAckNo(schAppendAdModel.MediaCode);

					StringBuilder  sbQuery   = new StringBuilder();			
					SqlParameter[] sqlParams = new SqlParameter[2];					

					// Ȩ���� �� ���̺� �߰�
					sbQuery.Append( "\n"
						+ "INSERT INTO SchAppend (                   \n"
						+ "       MediaCode                        \n"
						+ "      ,ScheduleOrder                    \n"
						+ "      ,ItemNo                           \n"
						+ "      ,ModDt                            \n"
						+ "      ,AckNo                            \n"
						+ "      )                                 \n"
						+ " VALUES(                                 \n"					
						+ "       " + schAppendAdModel.MediaCode + " \n"
						+ "      ,@ScheduleOrder   \n"
						+ "      ,@ItemNo                          \n"
						+ "      ,GETDATE()                        \n"
						+ "      ," + AckNo                    + " )\n"						
						);

					i = 0;
					sqlParams[i++] = new SqlParameter("@ScheduleOrder"        , SqlDbType.Int          );
					sqlParams[i++] = new SqlParameter("@ItemNo"        , SqlDbType.Int          );

					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(schAppendAdModel.ScheduleOrder);
					sqlParams[i++].Value = Convert.ToInt32(schAppendAdModel.ItemNo);

					_db.BeginTran();

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					
					_log.Debug("MediaCode:[" + schAppendAdModel.MediaCode + "]");
					_log.Debug("ScheduleOrder:[" + schAppendAdModel.ScheduleOrder + "]");
					_log.Debug("ItemNo:[" + schAppendAdModel.ItemNo + "]");
					// __DEBUG__
					_log.Debug(sbQuery.ToString());
					// __DEBUG__
					
					///=================================================================================================
				
					// Ȩ���� �� ���̺� �߰�
					sbQuery   = new StringBuilder();
					
					sbQuery.Append( "\n"
						+ "UPDATE ContractItem        \n"
						+ "   SET AdState = '20'      \n"   // ������� - 20:��
						+ "      ,ModDt   = GETDATE() \n"
						+ "      ,RegID   = '" + header.UserID + "' \n" 
						+ " WHERE ItemNo  = @ItemNo   \n"
						);

					// __DEBUG__
					_log.Debug(sbQuery.ToString());
					// __DEBUG__

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// ���������Ͻ� ��Ʈ
					SetLastUpdate(schAppendAdModel.MediaCode);

					_db.CommitTran();

					// ���� ����
					string LastOrder = "1";

					// �ش� ���� ���� ����
					sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
						+ " SELECT ISNULL(MAX(ScheduleOrder),1) AS LastOrder    \n"
						+ "   FROM SchAppend                                      \n"
						+ "  WHERE MediaCode = " + schAppendAdModel.MediaCode + " \n"
						);

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if (Utility.GetDatasetCount(ds) != 0)
					{
						LastOrder = Utility.GetDatasetString(ds, 0, "LastOrder");					
					}

					ds.Dispose();

					schAppendAdModel.ScheduleOrder = LastOrder;


					// __MESSAGE__
					_log.Message("Ȩ��������:[" + schAppendAdModel.ItemNo + "][" + schAppendAdModel.ItemName + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schAppendAdModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchAppendAdCreate_To() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schAppendAdModel.ResultCD   = "3101";
				schAppendAdModel.ResultDesc = "Ȩ������ ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion 


		#region ������λ����� ���ι�ȣ�� ����

		/// <summary>
		/// ������λ����� ���ι�ȣ�� ����
		/// ���°� 30:���������̸� �ű�(���� 10:����) ���� ������ AckNo ����
		/// </summary>
		/// <returns>string</returns>
		private string GetLastAckNo(string MediaCode)
		{

			string AckNo    = "";
			string AckState = "";

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLastAckNo() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("MediaCode	       :[" + MediaCode     + "]");		// �˻� ��ü				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n"
					+ " DECLARE @AckNo int, @AckState Char(2), @MediaCode int    \n"
					+ "                                                          \n"
					+ " SELECT @MediaCode = " + MediaCode                    + " \n"
					+ "                                                          \n"
					+ " SELECT TOP 1 @AckState = State, @AckNo = AckNo           \n"
					+ "   FROM SchPublish                                        \n"
					+ "  WHERE MediaCode = @MediaCode And AdSchType = 10         \n"
					+ "  ORDER BY AckNo DESC                                     \n"
					+ "                                                          \n"
					+ " IF @AckState = '30' OR @AckState IS NULL                 \n"
					+ " BEGIN                                                    \n"
					+ "	    INSERT INTO SchPublish                               \n"
					+ "                (AckNo, MediaCode, State, ModifyStartDay, AdSchType) \n"
					+ "         SELECT ISNULL(MAX(AckNo),0) + 1                  \n"
					+ "               ,@MediaCode                                \n"
					+ "               ,'10'                                      \n"
					+ "               ,GETDATE(),10                              \n"
					+ "          FROM SchPublish                                 \n"
					+ "         WHERE MediaCode = @MediaCode                     \n"
					+ "                                                          \n"
					+ "     SELECT TOP 1 @AckState = State, @AckNo = AckNo       \n"
					+ "       FROM SchPublish                                    \n"
					+ "      WHERE MediaCode = @MediaCode and AdSchType=10       \n"
					+ "      ORDER BY AckNo DESC                                 \n"
					+ " END                                                      \n"
					+ "                                                          \n"
					+ " SELECT @AckNo AS AckNo, @AckState AS AckState            \n"                             
					);
	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					AckNo    =  ds.Tables[0].Rows[0]["AckNo"].ToString();
					AckState =  ds.Tables[0].Rows[0]["AckState"].ToString();
				}

				ds.Dispose();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLastAckNo() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}
			return AckNo;
		}

		#endregion
	}

}