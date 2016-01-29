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

namespace AdManagerWebService.Contract
{
	/// <summary>
	/// FilePublishBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class FilePublishBiz : BaseBiz
	{

		#region ������
        public FilePublishBiz()
            : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}
		#endregion 

		#region ������λ��� ��ȸ
		/// <summary>
		/// ������λ��� ��ȸ
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishState(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishState() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	       :[" + filePublishModel.SearchMediaCode      + "]");		// �˻� ��ü				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n"
                    + " SELECT ACK_NO AS AckNo ,ACK_STT AS State \n"
                    + "   FROM FILEDIST_MST         \n"
                    + "  ORDER BY ACK_NO DESC      \n"
					);
	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					filePublishModel.AckNo =  ds.Tables[0].Rows[0]["AckNo"].ToString();
					filePublishModel.State =  ds.Tables[0].Rows[0]["State"].ToString();
				}
				else
				{
					filePublishModel.AckNo =  "";
					filePublishModel.State =  "";
				}

				ds.Dispose();

				// ����ڵ� ��Ʈ
				filePublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishState() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "������λ��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		#endregion

		#region ���Ϲ������θ�� ��ȸ

		/// <summary>
		/// ���Ϲ������θ�� ��ȸ
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishList(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFilePublishList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	       :[" + filePublishModel.SearchMediaCode      + "]");		// �˻� ��ü				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n"
					+ " SELECT A.ACK_NO AS AckNo                                                         \n"
                    + "       ,A.ACK_STT AS AckState                                             \n"
                    + "       ,B.STM_COD_NM AS AckStateName                                      \n"
                    + "       ,TO_CHAR(A.BEGIN_MOD_DT, 'YYYY-MM-DD') AS ModifyStartDay   \n"
                    + "       ,TO_CHAR(A.ACK_DT, 'YYYY-MM-DD') AS PublishDay           \n"
					+ "       ,A.ACK_ID AS PublishUser                                                   \n"
                    + "       ,C.USER_NM AS PublichUserName                                   \n"
                    + "       ,A.ACK_MEMO AS PublishDesc                                                   \n"
                    + "   FROM FILEDIST_MST         A                             \n"
                    + "        LEFT JOIN STM_COD    B  ON (A.ACK_STT       = B.STM_COD AND B.STM_COD_CLS = '33')  \n"  // 33:���Ϲ������λ���
                    + "        LEFT JOIN STM_USER C  ON (A.ACK_ID = C.USER_ID) \n"
                    + "  ORDER BY A.ACK_NO DESC                                                 \n"
					);
	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �ش�𵨿� ����
				filePublishModel.PublishDataSet = ds.Copy();
				ds.Dispose();

				// ���
				filePublishModel.ResultCnt = Utility.GetDatasetCount(filePublishModel.PublishDataSet);
				// ����ڵ� ��Ʈ
				filePublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetFilePublishList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "���Ϲ��������̷� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}
		#endregion

		#region ���Ϲ��� ����� ��ȸ

		/// <summary>
		/// ���Ϲ��� ����� ��ȸ
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishHistory(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishHistory() Start");
				_log.Debug("-----------------------------------------");

				_db.Open();
				_log.Debug("<�Է�����>");
				_log.Debug("MediaCode :[" + filePublishModel.MediaCode      + "]");		// ��ü�ڵ�				
				_log.Debug("AckNo     :[" + filePublishModel.AckNo          + "]");		// ���ι�ȣ				


                OracleParameter[] sqlParams = new OracleParameter[1];
                sqlParams[0] = new OracleParameter(":AckNo", OracleDbType.Int32);

				sqlParams[0].Value = Convert.ToInt32( filePublishModel.AckNo);

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n"
                    + "SELECT B.ITEM_SEQ  AS Seq         \n"
                    + "      ,B.ITEM_NO   AS ItemNo      \n"
                    + "      ,C.ITEM_NM   AS ItemName    \n"
                    + "      ,B.FILE_NM   AS FileName    \n"
                    + "      ,B.FILE_OPER AS AddDel      \n"
                    + "      ,TO_CHAR(A.ACK_DT, 'YYYY-MM-DD') As RegDt  \n"
					+ "      ,D.USER_NM AS RegName                        \n"
                    + "  FROM FILEDIST_MST            A                                                             \n"
                    + "       INNER JOIN FILEDIST_HST B ON (A.ACK_NO     = B.ACK_NO) \n"
                    + "       LEFT  JOIN ADVT_MST     C ON (B.ITEM_NO    = C.ITEM_NO)                          \n"
                    + "       LEFT  JOIN STM_USER     D ON (B.ID_INSERT  = D.USER_ID)                          \n"
                    + " WHERE A.ACK_NO     = :AckNo          \n"
                    + " ORDER BY B.ITEM_SEQ                      \n"
					);


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// ��� DataSet�� �ش�𵨿� ����
				filePublishModel.HistoryDataSet = ds.Copy();
				ds.Dispose();

				// ���
				filePublishModel.ResultCnt = Utility.GetDatasetCount(filePublishModel.HistoryDataSet);
				// ����ڵ� ��Ʈ
				filePublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetScheduleList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "���Ϲ�������Ȳ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		#endregion

		#region ���Ϲ�������
		/// <summary>
		/// ���Ϲ�������
		/// </summary>
		/// <returns></returns>
		public void SetFilePublish(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetFilePublish() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int i  = 0;
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();
						
					_db.BeginTran();

					// �����λ����� ����
					sbQuery.Append( "\n"
                        + "UPDATE FILEDIST_MST                 \n"
                        + "   SET ACK_STT  = '30'              \n"  // ���λ��� 30:��������
						+ "      ,ACK_ID   = '" + header.UserID + "'  \n"
						+ "      ,ACK_DT   = SYSDATE     \n"
                        + "      ,ACK_MEMO = :PublishDesc  \n"
                        + " WHERE ACK_NO   = :AckNo        \n"
                        + "   AND ACK_STT  = '10'          \n"
						);

                    OracleParameter[] sqlParams = new OracleParameter[2];

					i = 0;
                    sqlParams[i++] = new OracleParameter(":PublishDesc", OracleDbType.Varchar2, 200);
                    sqlParams[i++] = new OracleParameter(":AckNo", OracleDbType.Int32);


					i = 0;
					sqlParams[i++].Value = filePublishModel.PublishDesc;
                    sqlParams[i++].Value = Convert.ToInt32(filePublishModel.AckNo);

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    sbQuery = new StringBuilder();
					// ���ε� ���Ϲ��� ���ϸ���Ʈ�� �����Ѵ�.
					sbQuery.Append( "\n"
                        + " INSERT INTO FILEDIST_DTL      \n"
                        + "        (ACK_NO, ITEM_NO, FILE_NM) \n"
						+ " SELECT " + filePublishModel.AckNo + " \n"
                        + "        ,ITEM_NO \n"
                        + "        ,FILE_NM         \n"
                        + "    FROM ADVT_MST \n"
                        + "   WHERE FILE_STT = '30' \n"
                        + "   ORDER BY ITEM_NO        \n"

						);

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("���Ϲ������� : ���ι�ȣ[" + filePublishModel.AckNo + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				filePublishModel.ResultCD = "0000";  // ����
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetFilePublish() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD   = "3101";
				filePublishModel.ResultDesc = "���Ϲ������� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion

		#region ���Ϲ��� ���ϸ���Ʈ ��ȸ
		/// <summary>
		/// ���Ϲ��� ���ϸ���Ʈ ��ȸ
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishFileList(HeaderModel header, FilePublishModel filePublishModel)
		{
			//bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishFileList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("MediaCode :[" + filePublishModel.MediaCode  + "]"); // ��ü�ڵ�				
				_log.Debug("AckNo     :[" + filePublishModel.AckNo      + "]");	// ���ι�ȣ				
   				_log.Debug("ReserveDt :[" + filePublishModel.ReserveDt  + "]");	// �������� �����ȣ

				// __DEBUG__

                OracleParameter[] sqlParams = new OracleParameter[1];
		    
				int i = 0;
				//sqlParams[i++] = new SqlParameter("@MediaCode" , SqlDbType.Int);
                sqlParams[i++] = new OracleParameter("@AckNo", OracleDbType.Int32);
                //sqlParams[i++] = new SqlParameter("@ReserveDt" , SqlDbType.VarChar);

				i = 0;
				//sqlParams[i++].Value = Convert.ToInt32( filePublishModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32( filePublishModel.AckNo);
                //sqlParams[i++].Value = filePublishModel.ReserveDt.ToString();

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
                // ���� ���ι�ȣ���� ������ �͵��� 2, �� �����ǰ� ���Ϲ�ȣ ���� ���°� ���Ե�. �ƹ�ư ���� ������ �߻������� ����
                sbQuery.Append("\n SELECT B.ITEM_NO AS ItemNo ");
                sbQuery.Append("\n       ,C.ITEM_NM AS ItemName ");
                sbQuery.Append("\n       ,B.FILE_NM AS FileName ");
                sbQuery.Append("\n       ,C.ADVT_STT AS AdState ");
                sbQuery.Append("\n       ,D.STM_COD_NM AdStateName ");
				sbQuery.Append("\n 	     ,2	as Flag ");
                sbQuery.Append("\n   FROM FILEDIST_MST                  A                                                      ");
                sbQuery.Append("\n        INNER JOIN FILEDIST_DTL  B ON (A.ACK_NO = B.ACK_NO) ");
                sbQuery.Append("\n        LEFT  JOIN ADVT_MST      C ON (B.ITEM_NO    = C.ITEM_NO)                          ");
                sbQuery.Append("\n        LEFT  JOIN STM_COD       D ON (C.ADVT_STT   = D.STM_COD      AND D.STM_COD_CLS = '25' ) ");
                sbQuery.Append("\n  WHERE A.ACK_NO     = :AckNo ");
				//sbQuery.Append("\n    and B.ReserveDt in('',NULL) ");
                //sbQuery.Append("\n union all ");
                // ������ ���ι�ȣ���� ������ �͵�, ���õ� �����ȣ�� �����Ȱ��� 0, ���� ���ι�ȣ���� ���� �ð��� ��������Ȱ��� 1
                /* ���� ���ó��� ����
                sbQuery.Append("\n SELECT B.ITEM_NO AS ItemNo ");
				sbQuery.Append("\n       ,C.ItemName ");
				sbQuery.Append("\n       ,B.FileName ");
				sbQuery.Append("\n       ,C.AdState ");
				sbQuery.Append("\n       ,D.CodeName AdStateName ");
				sbQuery.Append("\n 	  ,0 AS Flag ");
                sbQuery.Append("\n   FROM FILEDIST_MST                  A                                                      ");
                sbQuery.Append("\n        INNER JOIN FILEDIST_DTL  B ON (A.ACK_NO = B.ACK_NO) ");
                sbQuery.Append("\n        LEFT  JOIN ContractItem       C ON (B.ITEM_NO    = C.ItemNo)                          ");
				sbQuery.Append("\n        LEFT  JOIN SystemCode         D ON (C.AdState   = D.Code      AND D.Section = '25' ) ");
				sbQuery.Append("\n  WHERE A.ACK_NO    = :AckNo ");
				sbQuery.Append("\n  ORDER BY Flag ,B.ItemNo DESC ");
                */
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// ��� DataSet�� �ش�𵨿� ����
				filePublishModel.FileListDataSet = ds.Copy();
				ds.Dispose();

				// ���
				filePublishModel.ResultCnt = Utility.GetDatasetCount(filePublishModel.FileListDataSet);
				// ����ڵ� ��Ʈ
				filePublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishFileList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "���Ϲ��� ���ϸ���Ʈ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		#endregion

		#region �������� �۾�����

		/// <summary>
		/// �������� ���ϸ���Ʈ
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void GetReserveFiles(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveFiles() Start");
				_log.Debug("-----------------------------------------");

				_db.Open();

				_log.Debug("<�Է�����>");
				_log.Debug("MediaCode :[" + filePublishModel.MediaCode      + "]");	// ��ü�ڵ�
				_log.Debug("AckNo     :[" + filePublishModel.AckNo          + "]");	// ���ι�ȣ


				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@AckNo"     , SqlDbType.Int);
				sqlParams[0].Value = Convert.ToInt32( filePublishModel.AckNo);

				StringBuilder sbQuery = new StringBuilder();				

				sbQuery.Append("\n select  m.ItemNo	as ItemNo");
				sbQuery.Append("\n 		,( select ItemName from ContractItem with(noLock) where itemNo = m.ItemNo ) as ItemNm");
				sbQuery.Append("\n 		,( select isnull(ReserveDt ,'')");
				sbQuery.Append("\n 		   from   FilePublishReserveDetail with(noLock)");
				sbQuery.Append("\n 		   where  ackNo = m.AckNo");
				sbQuery.Append("\n 			and	  ItemNo = m.ItemNo ) as ReserveDt");
                sbQuery.Append("\n      , avg(isnull(c.FileLength,0))  as FileSize ");
                sbQuery.Append("\n      , max(c.FileState)	as FileState ");
				sbQuery.Append("\n      , max(c.FileName)	as FileName ");
				sbQuery.Append("\n from	FilePublishHistory m with(nolock) ");
                sbQuery.Append("\n inner join ContractItem c with(nolock) on m.ItemNo = c.ItemNo ");
				sbQuery.Append("\n where	m.AckNo = @AckNo");
				sbQuery.Append("\n and		m.AddDel = '+'");
				sbQuery.Append("\n group by m.AckNo,m.ItemNo");
				_log.Debug(sbQuery.ToString());
				
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				filePublishModel.FileListDataSet = ds.Copy();
				ds.Dispose();

				// ���
				filePublishModel.ResultCnt = Utility.GetDatasetCount(filePublishModel.FileListDataSet);
				filePublishModel.ResultCD = "0000";

				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveFiles() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "�������ο��� ���ϸ���Ʈ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// �������� �۾�����Ʈ
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void GetReserveWorks(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveWorks() Start");
				_log.Debug("-----------------------------------------");

				_db.Open();

				_log.Debug("<�Է�����>");
				_log.Debug("MediaCode :[" + filePublishModel.MediaCode      + "]");	// ��ü�ڵ�
				_log.Debug("AckNo     :[" + filePublishModel.AckNo          + "]");	// ���ι�ȣ


				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@AckNo"     , SqlDbType.Int);
				sqlParams[0].Value = Convert.ToInt32( filePublishModel.AckNo);

				StringBuilder sbQuery = new StringBuilder();				
				sbQuery.Append("\n select ReserveDt");
				sbQuery.Append("\n 		 ,ReserveState");
				sbQuery.Append("\n 		 ,b.CodeName		as ReserveStatNm");
				sbQuery.Append("\n 		 ,ReserveUserId	as ReserveuserId");
				sbQuery.Append("\n 		 ,isnull(c.UserName,'')		as ReserveuserNm");
				sbQuery.Append("\n 		 ,( select count(distinct ItemNo ) from FilePublishReserveDetail nolock where ackNo = a.AckNo and ReserveDt = a.ReserveDt) as ReserveCount");
				sbQuery.Append("\n       ,(  select  sum(isnull(FileLength,0))           ");
				sbQuery.Append("\n            from    ContractItem l with(nolock)");
				sbQuery.Append("\n            where   ItemNo in(  select distinct ItemNo");
				sbQuery.Append("\n                                from    FilePublishReserveDetail m with(nolock)");
				sbQuery.Append("\n                                where   m.ackNo     = a.AckNo ");
				sbQuery.Append("\n                                and     m.ReserveDt = a.ReserveDt ) ) as FileSize");
				sbQuery.Append("\n		, substring(ReserveDt,1,4) + '-' + substring(ReserveDt,5,2) + '-' + substring(ReserveDt,7,2) + ' ' + substring(ReserveDt,9,2) + ':' + substring(ReserveDt,11,2) as ReserveDtStr");
				sbQuery.Append("\n from	FilePublishReserve a with(noLock)");
				sbQuery.Append("\n inner join SystemCode b with(noLock) on a.ReserveState	= b.Code and b.Section = '36'");
				sbQuery.Append("\n left outer join SystemUser c with(noLock) on a.ReserveUserId	= c.UserId");
				sbQuery.Append("\n where	a.AckNo = @AckNo");

				sbQuery.Append("\n union all");
	
				sbQuery.Append("\n select	 NULL ReserveDt");
				sbQuery.Append("\n 			,'90' ReserveState");
				sbQuery.Append("\n 			,'����' 	as ReserverStatNm");
				sbQuery.Append("\n 			,'system'	as ReserveuserId");
				sbQuery.Append("\n 			,'�ý���'	as ReserveuserNm");
				sbQuery.Append("\n 			,(  select count(*)");
				sbQuery.Append("\n 				from (	select	m.ItemNo");
				sbQuery.Append("\n 						from	FilePublishHistory m with(noLock)");
				sbQuery.Append("\n 						where	m.AckNo = @AckNo");
				sbQuery.Append("\n 						and		m.AddDel = '+'");
				sbQuery.Append("\n 						group by m.ItemNo");
				sbQuery.Append("\n 						EXCEPT ");
				sbQuery.Append("\n 						select  ItemNo");
				sbQuery.Append("\n 						from	FilePublishReserveDetail with(noLock)");
				sbQuery.Append("\n 						where	AckNo = @AckNo");
				sbQuery.Append("\n 					 ) v ) as ReserverCount");
				sbQuery.Append("\n 		 ,(  select  sum(isnull(FileLength,0))           ");
				sbQuery.Append("\n            from    ContractItem l with(nolock)");
				sbQuery.Append("\n            where   ItemNo in(  select	m.ItemNo");
				sbQuery.Append("\n					            from	FilePublishHistory m with(noLock)");
				sbQuery.Append("\n					            where	m.AckNo = @AckNo");
				sbQuery.Append("\n					            and		m.AddDel = '+'");
				sbQuery.Append("\n					            group by m.ItemNo");
				sbQuery.Append("\n					            EXCEPT ");
				sbQuery.Append("\n					            select  ItemNo");
				sbQuery.Append("\n					            from	FilePublishReserveDetail with(noLock)");
				sbQuery.Append("\n					            where	AckNo = @AckNo ) ) as FileSize");
				sbQuery.Append("\n		, NULL	as ReserveDtStr");
				_log.Debug(sbQuery.ToString());
				
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				filePublishModel.FileListDataSet = ds.Copy();
				ds.Dispose();

				// ���
				filePublishModel.ResultCnt = Utility.GetDatasetCount(filePublishModel.FileListDataSet);
				filePublishModel.ResultCD = "0000";

				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveWorks() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "�������ο��� �۾�����Ʈ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// �����۾� ��ȸ
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void GetReserveWorkSelect(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveWorkDetail() Start");
				_log.Debug("-----------------------------------------");

				_db.Open();
				StringBuilder sb = new StringBuilder();				
				SqlParameter[] sqlParams = new SqlParameter[2];

				sqlParams[0] = new SqlParameter("@AckNo"     , SqlDbType.Int);
				sqlParams[0].Value = Convert.ToInt32( filePublishModel.AckNo);

				sqlParams[1] = new SqlParameter("@ReserveDt"	, SqlDbType.VarChar, 12);
				sqlParams[1].Value = filePublishModel.ReserveDt;

				sb.Append("\n select a.AckNo					");
				sb.Append("\n 	    ,a.ReserveDt				");
				sb.Append("\n 	    ,a.ReserveState				");
				sb.Append("\n 	    ,b.UserName		as RegUser	");
				sb.Append("\n 	    ,c.UserName		as ModUser	");
				sb.Append("\n 	    ,a.ModDt					");
				sb.Append("\n 	    ,a.ReserveMsg				");
				sb.Append("\n from	FilePublishReserve a with(noLock) ");
				sb.Append("\n left outer join SystemUser b with(noLock) on a.ReserveUserId = b.UserId ");
				sb.Append("\n left outer join SystemUser c with(noLock) on a.ModUserId		= c.UserId ");
				sb.Append("\n where	a.AckNo		= @AckNo ");
				sb.Append("\n and	a.ReserveDt = @ReserveDt ");

				_log.Debug(sb.ToString());
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sb.ToString(),sqlParams);

				if(ds.Tables[0].Rows.Count > 0)
				{
					filePublishModel.State		=  ds.Tables[0].Rows[0]["ReserveState"].ToString();
					filePublishModel.ReserveDt	=  ds.Tables[0].Rows[0]["ReserveDt"].ToString();
					filePublishModel.ReserveUserName = ds.Tables[0].Rows[0]["RegUser"].ToString();
					filePublishModel.ModifyUserName  = ds.Tables[0].Rows[0]["ModUser"].ToString();
					filePublishModel.ModDt		= ds.Tables[0].Rows[0]["ModDt"].ToString();
					filePublishModel.PublishDesc	= ds.Tables[0].Rows[0]["ReserveMsg"].ToString();
				}
				else
				{
					filePublishModel.State = "00";
				}
				ds.Dispose();

				// ����ڵ� ��Ʈ
				filePublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + filePublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveWorkDetail() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "���Ϲ��������۾� ����ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}


		/// <summary>
		/// �����۾� �Է�
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void SetReserveWorkInsert(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();

				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[6];

				sqlParams[0] = new SqlParameter("@AckNo"	, SqlDbType.Int				);
				sqlParams[1] = new SqlParameter("@ResDt"	, SqlDbType.VarChar	,	12	);
				sqlParams[2] = new SqlParameter("@ResState" , SqlDbType.VarChar ,	 2	);
				sqlParams[3] = new SqlParameter("@ResUser"  , SqlDbType.VarChar ,	10	);
				sqlParams[4] = new SqlParameter("@ModUser"	, SqlDbType.VarChar ,	10	);
				sqlParams[5] = new SqlParameter("@Msg"		, SqlDbType.VarChar ,  100	);

				sqlParams[0].Value = Convert.ToInt32(filePublishModel.AckNo);
				sqlParams[1].Value = filePublishModel.ReserveDt;
				sqlParams[2].Value = filePublishModel.State;
				sqlParams[3].Value = header.UserID;							// ������ID �����ÿ� ����
				sqlParams[4].Value = header.UserID;
				sqlParams[5].Value = filePublishModel.PublishDesc;
			
				try
				{
					_db.BeginTran();

					sbQuery.Append("\n INSERT INTO FilePublishReserve ");
					sbQuery.Append("\n            (AckNo ");
					sbQuery.Append("\n            ,ReserveDt ");
					sbQuery.Append("\n            ,ReserveState ");
					sbQuery.Append("\n            ,ReserveUserId ");
					sbQuery.Append("\n            ,ModDt ");
					sbQuery.Append("\n            ,ModUserId ");
					sbQuery.Append("\n            ,ReserveMsg) ");
					sbQuery.Append("\n      VALUES ");
					sbQuery.Append("\n            (@AckNo ");
					sbQuery.Append("\n            ,@ResDt ");
					sbQuery.Append("\n            ,@ResState ");
					sbQuery.Append("\n            ,@ResUser ");
					sbQuery.Append("\n            ,GetDate() ");
					sbQuery.Append("\n            ,@ModUser ");
					sbQuery.Append("\n            ,@Msg ); ");

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("���Ϲ������� �Է�:["+filePublishModel.AckNo + "]["+filePublishModel.ReserveDt + "] �۾���:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				filePublishModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD   = "3201";
				filePublishModel.ResultDesc = "�������� ����� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}
		}

		/// <summary>
		/// �����۾� ����
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void SetReserveWorkUpdate(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();

				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[7];

				sqlParams[0] = new SqlParameter("@AckNo"	, SqlDbType.Int				);
				sqlParams[1] = new SqlParameter("@ResDt"	, SqlDbType.VarChar	,	12	);
				sqlParams[2] = new SqlParameter("@ResState" , SqlDbType.VarChar ,	 2	);
				sqlParams[3] = new SqlParameter("@ResUser"  , SqlDbType.VarChar ,	10	);
				sqlParams[4] = new SqlParameter("@ModUser"	, SqlDbType.VarChar ,	10	);
				sqlParams[5] = new SqlParameter("@Msg"		, SqlDbType.VarChar ,  100	);
				sqlParams[6] = new SqlParameter("@ResDtOrg" , SqlDbType.VarChar	,	12  );

				sqlParams[0].Value = Convert.ToInt32(filePublishModel.AckNo);
				sqlParams[1].Value = filePublishModel.ReserveDt;
				sqlParams[2].Value = filePublishModel.State;
				sqlParams[3].Value = header.UserID;							// ������ID �����ÿ� ����
				sqlParams[4].Value = header.UserID;
				sqlParams[5].Value = filePublishModel.PublishDesc;
				sqlParams[6].Value = filePublishModel.SearchReserveKey;
			
				try
				{
					_db.BeginTran();
					
					// Ű�� ���� ��쿣 ����Ű ������� ���븸 ����Ȱ�
					// Ű�� �ٸ� ��쿣 ����Ű�� ����Ȱ������� Detail���̺� �����ؾ� �Ѵ�
					if( filePublishModel.SearchReserveKey.Equals(filePublishModel.ReserveDt) )
					{
						sbQuery.Append("\n UPDATE FilePublishReserve ");
						sbQuery.Append("\n    SET ReserveState	= @ResState ");
						//sbQuery.Append("\n       ,ReserveUserId	= @ResUser ");
						sbQuery.Append("\n       ,ModDt			= GetDate() ");
						sbQuery.Append("\n       ,ModUserId		= @ModUser ");
						sbQuery.Append("\n       ,ReserveMsg	= @Msg ");
						sbQuery.Append("\n  WHERE AckNo		= @AckNo ");
						sbQuery.Append("\n    AND ReserveDt	= @ResDt ");
					}
					else
					{
						sbQuery.Append("\n UPDATE FilePublishReserve ");
						sbQuery.Append("\n    SET ReserveDt		= @ResDt ");
						sbQuery.Append("\n       ,ReserveState	= @ResState ");
						//sbQuery.Append("\n       ,ReserveUserId	= @ResUser ");
						sbQuery.Append("\n       ,ModDt			= GetDate() ");
						sbQuery.Append("\n       ,ModUserId		= @ModUser ");
						sbQuery.Append("\n       ,ReserveMsg	= @Msg ");
						sbQuery.Append("\n  WHERE AckNo		= @AckNo ");
						sbQuery.Append("\n    AND ReserveDt	= @ResDtOrg ");
					}
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					if( !filePublishModel.SearchReserveKey.Equals(filePublishModel.ReserveDt) )
					{
						sbQuery = new StringBuilder();
						sbQuery.Append("\n UPDATE FilePublishReserveDetail ");
						sbQuery.Append("\n    SET ReserveDt	= @ResDt ");
						sbQuery.Append("\n  WHERE AckNo		= @AckNo ");
						sbQuery.Append("\n    AND ReserveDt	= @ResDtOrg ");

						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					}
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("���Ϲ������� ����:["+filePublishModel.AckNo + "]["+filePublishModel.ReserveDt + "] �۾���:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				filePublishModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD   = "3201";
				filePublishModel.ResultDesc = "�������� ����� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}


		/// <summary>
		/// �������� ��ȸ
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void SetReserveFileSelect(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetReserveWorkDetail() Start");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				filePublishModel.ResultCD = "3000";
				filePublishModel.ResultDesc = "���Ϲ��������۾� ����ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}


		/// <summary>
		/// �������� ����( �Է�Ȥ�� ����ó�� )
		/// </summary>
		/// <param name="header"></param>
		/// <param name="filePublishModel"></param>
		public void SetReserveFileUpdate(HeaderModel header, FilePublishModel filePublishModel)
		{
			try
			{
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();
				DataSet			ds    = null;

				string	orgResDt	= "";	// ���� ����Ű
				string	orgResSt	= "";	// ���� �����۾�����
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[4];

				sqlParams[0] = new SqlParameter("@AckNo"	, SqlDbType.Int				);
				sqlParams[1] = new SqlParameter("@ResDt"	, SqlDbType.VarChar	,	12	);
				sqlParams[2] = new SqlParameter("@ItemNo"	, SqlDbType.Int				);
				sqlParams[3] = new SqlParameter("@regId"	, SqlDbType.VarChar ,	10	);

				sqlParams[0].Value = Convert.ToInt32(filePublishModel.AckNo);
				sqlParams[1].Value = filePublishModel.ReserveDt;
				sqlParams[2].Value = filePublishModel.ItemNo;
				sqlParams[3].Value = header.UserID;							// ������ID �����ÿ� ����
			
				try
				{
					_db.BeginTran();

					#region [ ���� ������ ����: ���� �����۾��� ���¸� ���� ]
					/*
					 * ���������Ͱ� �����ϴ��� �����Ѵ�.
					 * �����ϴ� ��쿣 �����Ϳ� Ű�� �����.
					 * ���� �������ϴ� �����ȣ���� �Ϸ���̸� ������ �Ұ��� �ϰ� ó���Ѵ�.
					 * */
					sbQuery.Append("\n select ReserveState,ReserveDt ");
					sbQuery.Append("\n from	  FilePublishReserve b with(noLock) ");
					sbQuery.Append("\n where  AckNo		= @AckNo ");
					sbQuery.Append("\n and	  ReserveDt = @ResDt ");

					ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

					if(ds.Tables[0].Rows.Count > 0)
					{
						orgResDt	=	ds.Tables[0].Rows[0]["ReserveDt"].ToString();
						orgResSt	=   ds.Tables[0].Rows[0]["ReserveState"].ToString();
					}
					else
					{
						orgResDt	=	"00";
						orgResSt	=   "00";
					}
					ds.Dispose();

					// 20�̸� �Ϸ�� �۾��� ���������� ������ �Ұ���.
					if( orgResSt.Equals("20") )
					{
						filePublishModel.ResultCD   = "0009";
						throw new Exception("[�ȳ�] �����Ϸ�� �����۾� �Դϴ�.\n�۾�ó���� �Ұ��� �մϴ�.");
					}
					#endregion

					#region [ ���� ������ ���� : �ش� �����ȣ�� �����۾� ��Ͽ��� ����]
					sbQuery = new StringBuilder();
					sbQuery.Append("\n select ReserveState,ReserveDt ");
					sbQuery.Append("\n from	  FilePublishReserve b with(noLock) ");
					sbQuery.Append("\n where  AckNo = @AckNo ");
					sbQuery.Append("\n and	  ReserveDt = (	select  ReserveDt ");
					sbQuery.Append("\n 						from	FilePublishReserveDetail a with(noLock) ");
					sbQuery.Append("\n 						where	a.ItemNo = @ItemNo ");
					sbQuery.Append("\n 						and		a.AckNo  = @AckNo ) ");

					ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

					if(ds.Tables[0].Rows.Count > 0)
					{
						orgResDt	=	ds.Tables[0].Rows[0]["ReserveDt"].ToString();
						orgResSt	=   ds.Tables[0].Rows[0]["ReserveState"].ToString();
					}
					else
					{
						orgResDt	=	"00";
						orgResSt	=   "00";
					}
					ds.Dispose();

					// 20�̸� �Ϸ�� �۾��� ���������� ������ �Ұ���.
					if( orgResSt.Equals("20") )
					{
						filePublishModel.ResultCD   = "0001";
						throw new Exception("[�ȳ�] �����Ϸ�� �����۾��� �����ִ� ���������Դϴ�.\n�۾�ó���� �Ұ��� �մϴ�.");
					}
					#endregion
				

					sbQuery = new StringBuilder();
					if( filePublishModel.JobType.Equals("+") )
					{
						sbQuery = new StringBuilder();
						if( orgResDt.Equals("00") )
						{
							// �űԼ����� ���
							sbQuery.Append("\n insert into FilePublishReserveDetail");
							sbQuery.Append("\n			( AckNo,	ReserveDt,	ItemNo,		regId,		regDt )");
							sbQuery.Append("\n values   ( @AckNo,   @ResDt,		@ItemNo,	@regId,		GetDate());");
							rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

							if( rc == 0 )
							{
								filePublishModel.ResultCD   = "0002";
								throw new Exception("[�ȳ�] �������� �űԵ�Ͽ� �����߽��ϴ�");
							}
						}
						else
						{
							// ���漳���� ���
							sbQuery.Append("\n update FilePublishReserveDetail	");
							sbQuery.Append("\n	  set ReserveDt	= @ResDt	");
							sbQuery.Append("\n		 ,regId		= @regId	");
							sbQuery.Append("\n	     ,regDt		= GetDate()	");
							sbQuery.Append("\n	where AckNo		= @AckNo	");
							sbQuery.Append("\n	  and ReserveDt = '" + orgResDt + "'");
							sbQuery.Append("\n    and ItemNo    = @ItemNo   ");
						
							rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

							if( rc == 0 )
							{
								filePublishModel.ResultCD   = "0003";
								throw new Exception("[�ȳ�] �������� �����Ͽ� �����߽��ϴ�");
							}
						}
					}
					else
					{
						// ������ ���
						sbQuery.Append("\n delete from FilePublishReserveDetail	");
						sbQuery.Append("\n where AckNo	= @AckNo	");
						sbQuery.Append("\n and ReserveDt= @ResDt	");
						sbQuery.Append("\n and ItemNo   = @ItemNo   ");
						
						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

						if( rc == 0 )
						{
							filePublishModel.ResultCD   = "0004";
							throw new Exception("[�ȳ�] �������� ����ó���� �����߽��ϴ�");
						}
					}
					
					_db.CommitTran();
					_log.Message("���Ϲ������� ����:["+filePublishModel.AckNo + "]["+filePublishModel.ReserveDt + "]["+filePublishModel.ItemNo + "] �۾���:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				filePublishModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdFileUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				if( filePublishModel.ResultCD.Equals("0000") )
				{
					filePublishModel.ResultCD = "3201";
				}

				filePublishModel.ResultDesc = ex.Message;
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}



		#endregion
	}
}