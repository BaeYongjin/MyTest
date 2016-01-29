/*
 * -------------------------------------------------------
 * Ŭ���� �� : TargetCollectionBiz
 * �ֿ���  : Ÿ�� ���� ���� ����
 * �ۼ���    : ��
 * �ۼ���    : ��
 * Ư�̻���  : 
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2013.05.23
 * ��������  :        
                - �Ǵ� ó���ϴ� ������ ����� ���,������ 
 *                ��ü �����͸� �޾Ƽ� ó�� �ϴ� �������� ����
 *              - ��ü�����͸� �������� ������ ������ ó�� �� ������ ���� 10����
 *                 �����ؼ� �� ���� ó�� �� �������� ó�� �� �� sleep ������ ����.
 *                 �׽�Ʈ �� �����ؾ� �� �� �� ����.
 * �����Լ�  : 
 *            [add] 
 *              using System.Threading
 *              SetClientCollectionCreate(..) ������ ��� �� ����ڵ� ���
 *              SetClientCollectionDelete(..) ������ ���� �� ����ڵ� ����
 *            [edit]
 *            
 * --------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : bae
 * ������    : 2013.06.10
 * �����κ�  : 
 *            [add]
 *              SetClientCollectionCreateProc(..) ���� ����� ���ν��� �̿��� �Լ� �߰�
 *              SetClientCollectionCreate(..) �Լ� ��ſ� ���ν��� �̿��� �Լ� ȣ��� ���� ó�� ��.
 *            
 * ��������  : 
 * --------------------------------------------------------
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;
using System.Threading;

using AdManagerModel;


namespace AdManagerWebService.Target
{
	/// <summary>
	/// TargetCollectionBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class TargetCollectionBiz : BaseBiz
	{
		public TargetCollectionBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		/// Ÿ�ٱ������ȸ
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetTargetCollectionList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTargetCollectionList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey       + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
                // Ÿ�� ���� ���� �������� //
                // �����ڵ�, ������, Ŀ��Ʈ, �������
                // �����, ������
                // ������ ����� ��
                // ������ Ÿ���õ� ���� ��
                // ����ȭ ���� 
				sbQuery.Append("\n SELECT A.CollectionCode, A.CollectionName, A.Comment ");
				sbQuery.Append("\n        ,A.UseYn ");
				sbQuery.Append("\n        ,CASE A.UseYn WHEN 'Y' THEN '���' WHEN 'N' THEN '������' END AS UseYn_N ");
				sbQuery.Append("\n        ,convert(char(19), A.RegDt, 120) RegDt ");
				sbQuery.Append("\n        ,convert(char(19), A.ModDt, 120) ModDt ");
				sbQuery.Append("\n        ,B.UserName RegName ");
				sbQuery.Append("\n        ,( Select count(*) from ClientList with(noLock) where CollectionCode = A.CollectionCode AND CollectionCode = A.CollectionCode) Cnt ");
				sbQuery.Append("\n 		  ,( select count(*)  ");
				sbQuery.Append("\n 			 from	Targeting x with(noLock) ");
				sbQuery.Append("\n 			 inner join ContractItem y with(noLock) on x.ItemNo = y.ItemNo and y.AdState <> '40' ");
				sbQuery.Append("\n 			 where	x.TgtCollectionYn = 'Y'  ");
				sbQuery.Append("\n 			 and		x.TgtCollection = a.CollectionCode ) as UseCnt ");
				sbQuery.Append("\n 		  , case a.PvsYn when 'Y' then 'True' else 'False' end  as PvsYn");
				sbQuery.Append("\n 		  , case a.PvsYn when 'Y' then isnull(PVSSeq,0) else 0 end  as PvsSeq");
				sbQuery.Append("\n FROM	Collection			A with(noLock) ");
				sbQuery.Append("\n LEFT JOIN SystemUser	B with(NoLock) ON (A.RegId          = B.UserId) ");
				sbQuery.Append("\n WHERE 1 = 1  ");			
				// �˻�� ������
				if (targetcollectionModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
						+ "  A.CollectionName      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"	
						+ " OR A.Comment    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
						+ " OR B.UserName    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}
				
				if(targetcollectionModel.SearchchkAdState_10.Trim().Length > 0 && targetcollectionModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND A.UseYn = 'Y' OR A.UseYn = 'N' \n");
				}	
				if(targetcollectionModel.SearchchkAdState_10.Trim().Length > 0 && targetcollectionModel.SearchchkAdState_10.Trim().Equals("N"))
				{
					sbQuery.Append(" AND  A.UseYn  = 'Y' \n");					
				}	

				sbQuery.Append(" ORDER BY A.CollectionCode desc \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �̵��𵨿� ����
				targetcollectionModel.TargetCollectionDataSet = ds.Copy();
				// ���
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.TargetCollectionDataSet);
				// ����ڵ� ��Ʈ
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTargetCollectionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "Ÿ�ٰ������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// PVS�׷�����ȸ
		/// </summary>
		/// <param name="header"></param>
		/// <param name="targetcollectionModel"></param>
		public void GetPvsGroupList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{  // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open(); 
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPvsGroupList() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append("\n select Seq_No		as SeqNo ");
				sbQuery.Append("\n 		 ,Group_Name	as GroupName ");
				sbQuery.Append("\n 		 ,( select count(*) from PVDB.dbo.Favorite_group_stb_list with(noLock) where seq_no = fgm.seq_no ) as stbCnt ");
				sbQuery.Append("\n from	PVDB.dbo.Favorite_group_master fgm with(noLock) ");
				sbQuery.Append("\n where	fgm.Used_Yn = 'Y' ");
				sbQuery.Append("\n order by Seq_No desc ");
				_log.Debug(sbQuery.ToString());
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ������Ű���𵨿� ����
				targetcollectionModel.TargetCollectionDataSet = ds.Copy();
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.TargetCollectionDataSet);
				targetcollectionModel.ResultCD = "0000";

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPvsGroupList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "GetPvsGroupList ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}


		/// <summary>
		/// PVS�󼼳��� ����ȭ
		/// </summary>
		/// <param name="header"></param>
		/// <param name="targetcollectionModel"></param>
		public void SetPvsSync(HeaderModel header, TargetCollectionModel data)
		{
			try
			{  // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open(); 
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetPvsSync() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();
				string	pvsName = data.CollectionName + "�� ����ȭ ��Ŵ";

				sbQuery.Append("\n declare @i_CollCode	int;	");
				sbQuery.Append("\n declare @i_PvsSeq	int;	");
				sbQuery.Append("\n declare @i_PvsName	varchar(50);	");

				sbQuery.Append("\n set @i_CollCode	= " + data.CollectionCode );
				sbQuery.Append("\n set @i_PvsSeq	= " + data.SeqNo.ToString() );
				sbQuery.Append("\n set @i_PvsName	= '" + pvsName + "'");

				sbQuery.Append("\n -- 1. ���� AdTargets�� ������ �����Ѵ�	");
				sbQuery.Append("\n delete from ClientList where CollectionCode = @i_CollCode;	");

				sbQuery.Append("\n -- 2. PVS���� ������ �����ͼ�..AdTargets�� �Է��Ѵ�	");
				sbQuery.Append("\n insert into ClientList	");
				sbQuery.Append("\n select @i_CollCode	");
				sbQuery.Append("\n		 ,stb.UserId	");
				sbQuery.Append("\n 		 ,getDate()	");
				sbQuery.Append("\n 		 ,stb.ServiceNum	");
				sbQuery.Append("\n from	StbUser stb with(noLock)	");
				sbQuery.Append("\n inner join ( select	Stb_Id	as StbId	");
				sbQuery.Append("\n 				from	pvdb.dbo.Favorite_Group_Stb_List with(noLock)	");
				sbQuery.Append("\n 				where	Seq_No = @i_PvsSeq ) pvs	");
				sbQuery.Append("\n 		 on pvs.stbId = stb.stbId	");

				sbQuery.Append("\n if @@rowcount > 0	");
				sbQuery.Append("\n 	update Collection	");
				sbQuery.Append("\n 	set	 PVSSeq = @i_PvsSeq	");
				sbQuery.Append("\n 		,Comment = @i_PvsName	");
				sbQuery.Append("\n 		,ModDt	= GetDate()	");
				sbQuery.Append("\n 	where CollectionCode = @i_CollCode;	");
				sbQuery.Append("\n 	");
				_log.Debug(sbQuery.ToString());
				
				// ��������
				DataSet ds = new DataSet();
				int rc = _db.ExecuteNonQuery( sbQuery.ToString());

				// ��� DataSet�� ������Ű���𵨿� ����
				data.ResultCnt = rc;
				data.ResultCD = "0000";

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetPvsSync() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "SetPvsSync ó���� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		/// <summary>
		/// ��ž����Ʈ�� �о�´�
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetStbList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{  // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStbList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
                    + "Select Top 1000                  \n"
					+ "       'False' AS CheckYn        \n"
					+ "		  ,ServiceNum as    UserId  \n"     
					+ "		  ,StbId					\n"
					+ "       ,PostNo					\n"
					+ "       ,ServiceCode				\n"
					+ "		  ,ResidentNo				\n"
					+ "		  ,Sex						\n"
					+ "       ,CASE Sex WHEN 'M' THEN '��' WHEN 'F' THEN '��' END AS Sex_N  \n"
					+ "		  ,RegDt					\n"					
					+ "  FROM StbUser with(noLock)      \n"				
					+ " WHERE  1 = 1					\n"
					);
				
				// �˻�� ������
				if (targetcollectionModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append("\n"
						+ "  AND ( StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
						+ "     OR PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
						+ "     OR ServiceNum   LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
						+ " 	)       \n"
						);
				}
       
				sbQuery.Append("  ORDER BY ServiceNum, PostNo, ResidentNo, Sex");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ������Ű���𵨿� ����
				targetcollectionModel.StbListDataSet = ds.Copy();
				// ���
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.StbListDataSet);
				// ����ڵ� ��Ʈ
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStbList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "Stb ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}

        /// <summary>
        /// ��ž������ Ư����ž�� ��ȸ�Ѵ�.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="targetcollectionModel"></param>
        public void GetStbListColl(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {  // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open(); 

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbListColl() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey       + "]");
                _log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n Select Top 1000          "); // 1000 ���� ���̱�
                sbQuery.Append("\n       'False' AS CheckYn ");
                sbQuery.Append("\n        ,A.CollectionCode ");
                sbQuery.Append("\n 		  ,A.UserId ");
                sbQuery.Append("\n 		  ,B.StbId ");
                sbQuery.Append("\n 		  ,B.PostNo ");
                sbQuery.Append("\n 		  ,B.ServiceCode ");
                sbQuery.Append("\n 		  ,B.ResidentNo ");
                sbQuery.Append("\n 		  ,B.Sex ");
                sbQuery.Append("\n        ,CASE B.Sex WHEN 'M' THEN '��' WHEN 'F' THEN '��' END AS Sex_N ");
                sbQuery.Append("\n 		  ,A.RegDt ");
                sbQuery.Append("\n from	ClientList      a with(nolock) ");
                sbQuery.Append("\n inner join StbUser   b with(nolock) on b.UserId = a.UserId");
                sbQuery.Append("\n where    a.CollectionCode = " + targetcollectionModel.CollectionCode );
				
                // �˻�� ������
                if (targetcollectionModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("\n"
                        + "     AND (   b.StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.ServiceNum  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + " 	    )  \n"
                        );
                }
       
                sbQuery.Append("  ORDER BY b.ServiceNum, b.PostNo, b.ResidentNo, b.Sex");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // ��� DataSet�� ������Ű���𵨿� ����
                targetcollectionModel.StbListDataSet = ds.Copy();                
                // ���
                targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.StbListDataSet);
                              
                // ����ڵ� ��Ʈ
                targetcollectionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbListColl() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                targetcollectionModel.ResultCD = "3000";
                targetcollectionModel.ResultDesc = "Stb ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }


        /// <summary>
        /// ��ž ����Ʈ ����¡ ó��.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="targetcollectionModel"></param>
        public void GetStbPageList(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {  // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbPageList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey + "]");
                _log.Debug("JobCode      :[" + targetcollectionModel.UserId + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
          
                // ��������                    
                sbQuery.Append(" Select Top (" + targetcollectionModel.PageSize.Trim() + ") \n");
                sbQuery.Append("     'False' AS CheckYn         \n");
                sbQuery.Append("	 ,ServiceNum as    UserId   \n");
                sbQuery.Append("	 ,StbId					    \n");
                sbQuery.Append("     ,PostNo					\n");
                sbQuery.Append("     ,ServiceCode				\n");
                sbQuery.Append("	 ,ResidentNo				\n");
                sbQuery.Append("	 ,Sex						\n");
                sbQuery.Append("     ,CASE Sex WHEN 'M' THEN '��' WHEN 'F' THEN '��' END AS Sex_N  \n");
                sbQuery.Append("	 ,RegDt					    \n");
                sbQuery.Append(" FROM StbUser with(noLock)      \n");
                sbQuery.Append(" WHERE  1 = 1					\n");
                    
                if (targetcollectionModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("  AND (    StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("        OR PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("        OR ServiceNum   LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("      ) \n");
                }
    
                sbQuery.Append(" And UserId Not IN (Select Top((" + targetcollectionModel.Page.Trim() + "-1)*" + targetcollectionModel.PageSize.Trim() + ")UserId \n");
                sbQuery.Append("                    From StbUser \n");
                sbQuery.Append("                    Where 1=1    \n");

                if (targetcollectionModel.SearchKey.Trim().Length > 0) // �˻�� ������
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.                        
                    sbQuery.Append("  AND (    StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("        OR PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("        OR ServiceNum   LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("      )  \n");
                }                   
                sbQuery.Append("  ) ORDER BY ServiceNum, PostNo, ResidentNo, Sex");
                
                
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� ������Ű���𵨿� ����
                targetcollectionModel.StbListDataSet = ds.Copy();
                // ���
                targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.StbListDataSet);
                // ����ڵ� ��Ʈ
                targetcollectionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetcollectionModel.ResultCD = "3000";
                targetcollectionModel.ResultDesc = "Stb ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }



		/// <summary>
		/// ������Ʈ
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetClientList(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            #region ���� ��� �ּ�...
            /*
			try
			{  // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				
				// ��������
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn            \n"
					+ "       ,A.CollectionCode			\n"     
					+ "		  ,A.UserId					\n"	
					+ "		  ,B.StbId					\n"	
					+ "		  ,B.PostNo					\n"	
					+ "		  ,B.ServiceCode			\n"	
					+ "		  ,B.ResidentNo				\n"	
					+ "		  ,B.Sex					\n"	
					+ "       ,CASE B.Sex WHEN 'M' THEN '��' WHEN 'F' THEN '��' END AS Sex_N  \n"
					+ "		  ,A.RegDt					\n"										
					+ "  FROM ClientList A with(noLock) LEFT JOIN StbUser B with(NoLock) ON (A.UserId = B.UserId)        \n"					
					+ " WHERE  1 = 1					\n"
					);
				
				// �˻�� ������
//				if (targetcollectionModel.SearchKey.Trim().Length > 0)
//				{
//					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
//					sbQuery.Append("\n"
//						+ "  AND ( StbId		  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR PostNo	      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR ResidentNo     LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR Sex		      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ " 	)       \n"
//						);
//				}

				// ��ü����籤���ַ����� ����������
				if(targetcollectionModel.CollectionCode.Trim().Length > 0 && !targetcollectionModel.CollectionCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "' \n");
				}		
       
				sbQuery.Append("  ORDER BY A.CollectionCode, A.UserId");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ������Ű���𵨿� ����
				targetcollectionModel.ClientListDataSet = ds.Copy();
				// ���
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);
				// ����ڵ� ��Ʈ
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "�� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
            */
            #endregion
            try
            {  // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey + "]");
                _log.Debug("JobCode      :[" + targetcollectionModel.UserId + "]");

                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n Select Top 1000          "); // 1000 ���� ���̱�
                sbQuery.Append("\n       'False' AS CheckYn ");
                sbQuery.Append("\n        ,A.CollectionCode ");
                sbQuery.Append("\n 		  ,A.UserId ");
                sbQuery.Append("\n 		  ,B.StbId ");
                sbQuery.Append("\n 		  ,B.PostNo ");
                sbQuery.Append("\n 		  ,B.ServiceCode ");
                sbQuery.Append("\n 		  ,B.ResidentNo ");
                sbQuery.Append("\n 		  ,B.Sex ");
                sbQuery.Append("\n        ,CASE B.Sex WHEN 'M' THEN '��' WHEN 'F' THEN '��' END AS Sex_N ");
                sbQuery.Append("\n 		  ,A.RegDt ");
                sbQuery.Append("\n from	ClientList      a with(nolock) ");
                sbQuery.Append("\n inner join StbUser   b with(nolock) on b.UserId = a.UserId");
                sbQuery.Append("\n where    a.CollectionCode = " + targetcollectionModel.CollectionCode);

                // �˻�� ������
                if (targetcollectionModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("\n"
                        + "     AND (   b.StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.ServiceNum  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + " 	    )  \n"
                        );
                }

                sbQuery.Append("  ORDER BY b.ServiceNum, b.PostNo, b.ResidentNo, b.Sex");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� ������Ű���𵨿� ����
                targetcollectionModel.ClientListDataSet = ds.Copy();
                // ���
                targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);

                // ����ڵ� ��Ʈ
                targetcollectionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetcollectionModel.ResultCD = "3000";
                targetcollectionModel.ResultDesc = "�� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

		/// <summary>
		/// ������Ʈ-����¡
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetClientPageList(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            #region ���� ���..
            /*
			try
			{  // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientPageList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				
				// ��������
				sbQuery.Append("\n"
					+ "SELECT top (" + targetcollectionModel.PageSize.Trim() + ")            \n"
					+ "       'False' AS CheckYn			\n"     
					+ "       ,A.CollectionCode			\n"     
					+ "		  ,A.UserId					\n"	
					+ "		  ,B.StbId					\n"	
					+ "		  ,B.PostNo					\n"						
					+ "		  ,B.ServiceCode			\n"	
					+ "		  ,B.ResidentNo				\n"	
					+ "		  ,B.Sex					\n"	
					+ "       ,CASE B.Sex WHEN 'M' THEN '��' WHEN 'F' THEN '��' END AS Sex_N  \n"
					+ "		  ,A.RegDt					\n"										
					+ "		  ,C.SummaryName DongName					\n"										
					+ "  FROM ClientList A with(noLock) LEFT JOIN StbUser B with(NoLock) ON (A.UserId          = B.UserId)        \n"					
					+ "					   LEFT JOIN SummaryCode C with(NoLock) ON ((select dbo.ufnGetTargetRegionCode2(substring(B.PostNo,0,4))) = C.SummaryCode AND C.SummaryType=5)        \n"					
					+ " WHERE  1 = 1					\n"
					);
				
				// �˻�� ������
				//				if (targetcollectionModel.SearchKey.Trim().Length > 0)
				//				{
				//					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
				//					sbQuery.Append("\n"
				//						+ "  AND ( StbId		  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR PostNo	      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR ResidentNo     LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR Sex		      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ " 	)       \n"
				//						);
				//				}

				sbQuery.Append(" AND A.UserId not in(Select top ((" + targetcollectionModel.Page.Trim() + "-1)*" + targetcollectionModel.PageSize.Trim() + ")UserId \n");
				sbQuery.Append("					 From 	ClientList  \n");                 
			    sbQuery.Append("                     Where  CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "') \n");
				
				if(targetcollectionModel.CollectionCode.Trim().Length > 0 && !targetcollectionModel.CollectionCode.Trim().Equals("00"))			
					sbQuery.Append(" AND A.CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "' \n");       
				sbQuery.Append("  ORDER BY A.CollectionCode, A.UserId");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ������Ű���𵨿� ����
				targetcollectionModel.ClientListDataSet = ds.Copy();
				// ���
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);
				// ����ڵ� ��Ʈ
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientPageList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "�� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
            */
            #endregion
            try
            {  // �����ͺ��̽��� OPEN�Ѵ�
                _db.ConnectionString = FrameSystem.connSummaryDbString;

                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientPageList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey + "]");
                _log.Debug("JobCode      :[" + targetcollectionModel.UserId + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                               
                // Ư�� ����
                sbQuery.Append("\n Select Top (" + targetcollectionModel.PageSize + ") ");
                sbQuery.Append("\n        'False' AS CheckYn ");
                sbQuery.Append("\n        ,A.CollectionCode ");
                sbQuery.Append("\n 		  ,A.UserId ");
                sbQuery.Append("\n 		  ,B.StbId ");
                sbQuery.Append("\n 		  ,B.PostNo ");
                sbQuery.Append("\n 		  ,B.ServiceCode ");
                sbQuery.Append("\n 		  ,B.ResidentNo ");
                sbQuery.Append("\n 		  ,B.Sex ");
                sbQuery.Append("\n        ,CASE B.Sex WHEN 'M' THEN '��' WHEN 'F' THEN '��' END AS Sex_N ");
                sbQuery.Append("\n 		  ,A.RegDt ");
                sbQuery.Append("\n        ,C.SummaryName DongName       ");
                sbQuery.Append("\n From	AdTargetsHanaTV.dbo.ClientList      a with(nolock)  ");
                sbQuery.Append("\n INNER join AdTargetsHanaTV.dbo.StbUser   b with(nolock) on b.UserId = a.UserId");
                sbQuery.Append("\n LEFT  JOIN SummaryCode C with(NoLock) ON ((select AdTargetsHanaTV.dbo.ufnGetTargetRegionCode2(substring(B.PostNo,0,4))) = C.SummaryCode AND C.SummaryType=5)        \n");
                sbQuery.Append("\n where    a.CollectionCode = " + targetcollectionModel.CollectionCode);
                // �˻�� ������
                if (targetcollectionModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.                        
                    sbQuery.Append("     AND (   b.StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.ServiceNum  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append(" 	    )  \n");
                }

                //sbQuery.Append("\n AND A.UserId not in(select top ((" + targetcollectionModel.Page.Trim() + "-1)*" + targetcollectionModel.PageSize.Trim() + ")UserId \n");
                //sbQuery.Append("\n  				   from 	ClientList  \n");
                //sbQuery.Append("\n                     Where CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "' \n");

                sbQuery.Append("\n AND A.UserId > ( select isnull( max(userId), 0)  \n");
                sbQuery.Append("\n                  from (  select top ((" + targetcollectionModel.Page.Trim() + "-1)*" + targetcollectionModel.PageSize.Trim() + ")UserId \n");
                sbQuery.Append("\n  				        from    AdTargetsHanaTV.dbo.ClientList nolock  \n");
                sbQuery.Append("\n                          Where   CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "' \n");
                // �˻�� ������
                if (targetcollectionModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("     AND (   b.StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.ServiceNum  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append(" 	    )  \n");
                }
                sbQuery.Append("        order by userId  ) v ) ");
                sbQuery.Append(" ORDER BY a.UserId");
               

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� ������Ű���𵨿� ����
                targetcollectionModel.ClientListDataSet = ds.Copy();
                // ���
                targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);
                // ����ڵ� ��Ʈ
                targetcollectionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientPageList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetcollectionModel.ResultCD = "3000";
                targetcollectionModel.ResultDesc = "���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

		/// <summary>
		/// Ÿ�ٱ����� ����
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetTargetCollectionUpdate(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[6];

				sbQuery.Append(""
					+ " UPDATE  Collection  \n"
					+ " SET	  CollectionName= @CollectionName	\n"										
					+ "      ,Comment		= @Comment  \n"			
					+ "      ,UseYn			= @UseYn    \n"	
					+ "      ,ModDt         = GETDATE() \n"
					+ "      ,RegID         = @RegID    \n"
					+ "      ,PvsYn         = @PvsYn	\n"
					+ " WHERE CollectionCode        = @CollectionCode        \n"
					);

				sqlParams[0] = new SqlParameter("@CollectionName" , SqlDbType.VarChar	, 500);												
				sqlParams[1] = new SqlParameter("@Comment"		, SqlDbType.VarChar , 2000);		
				sqlParams[2] = new SqlParameter("@UseYn"			, SqlDbType.Char	, 1);
				sqlParams[3] = new SqlParameter("@RegID"			, SqlDbType.VarChar , 10);
				sqlParams[4] = new SqlParameter("@CollectionCode" , SqlDbType.Int);
				sqlParams[5] = new SqlParameter("@PvsYn"			, SqlDbType.Char	, 1);

				sqlParams[0].Value = targetcollectionModel.CollectionName;								
				sqlParams[1].Value = targetcollectionModel.Comment;				
				sqlParams[2].Value = targetcollectionModel.UseYn;
				sqlParams[3].Value = header.UserID;      // �����
				sqlParams[4].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);
				sqlParams[5].Value = targetcollectionModel.PvsYn;

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ÿ�ٱ���������:["+targetcollectionModel.CollectionCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3201";
				targetcollectionModel.ResultDesc = "Ÿ�ٱ����� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			// �����ͺ��̽���  Close�Ѵ�
			_db.Close();

		}

		/// <summary>
		/// Ÿ�ٱ� ����
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetTargetCollectionCreate(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[4];

				sbQuery.Append( ""
					+ "INSERT INTO Collection (	\n"
					+ "       CollectionCode    \n"
					+ "      ,CollectionName    \n"					
					+ "      ,Comment			\n"
					+ "		 ,UseYn				\n"															
					+ "      ,RegDt				\n"
					+ "      ,ModDt				\n"
					+ "      ,RegID             \n"
					+ "      ,PvsYn				\n"
					+ "      )                  \n"
					+ " SELECT                  \n"
					+ "       ISNULL(MAX(CollectionCode),0)+1        \n"
					+ "      ,@CollectionName   \n"					
					+ "      ,@Comment			\n"					
					+ "      ,'Y'				\n"					
					+ "      ,GETDATE()			\n"
					+ "      ,GETDATE()			\n"
					+ "      ,@RegID			\n"
					+ "      ,@PvsYn			\n"
					+ " FROM Collection               \n"
					);
				
				sqlParams[i++] = new SqlParameter("@CollectionName" , SqlDbType.VarChar , 40);				
				sqlParams[i++] = new SqlParameter("@Comment"		, SqlDbType.VarChar , 50);				
				sqlParams[i++] = new SqlParameter("@RegID"			, SqlDbType.VarChar , 10);
				sqlParams[i++] = new SqlParameter("@PvsYn"			, SqlDbType.VarChar , 1 );

				i = 0;				
				sqlParams[i++].Value = targetcollectionModel.CollectionName;				
				sqlParams[i++].Value = targetcollectionModel.Comment;				
				sqlParams[i++].Value = header.UserID;				// �����
				sqlParams[i++].Value = targetcollectionModel.PvsYn;


				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ÿ�ٱ���������:[" + targetcollectionModel.CollectionCode + "(" + targetcollectionModel.CollectionName + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3101";
				targetcollectionModel.ResultDesc = "Ÿ�ٱ����� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}


		/// <summary>
		/// ������Ʈ ����
		/// </summary>
		public void SetClientCreate(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

                int     iCollectionCode =   Convert.ToInt32(targetcollectionModel.CollectionCode);
                string  iServiceNum     =   targetcollectionModel.UserId.ToString();

				StringBuilder sbQuery = new StringBuilder();

                #region [ ���翩�� Ȯ�ι� ��Ͽ��� Ȯ�� ]
                // �ش簡���� ����������� �����ϴ��� �˻�
                // 0:������, 1:�����, 2:����,3:�Ͻ��ߴ�,4:�������,5:������������,7:���డ��,9:�����ߴ�, 10:�������
                sbQuery.Append(" select  sum( case gubun when 1 then cnt else 0 end )	as	Stb " + "\n");
                sbQuery.Append("		,sum( case gubun when 2 then cnt else 0 end )	as	Clt " + "\n");
                sbQuery.Append(" from ( " + "\n");
                sbQuery.Append("		select	1 as gubun ,count(*) as cnt " + "\n");
                sbQuery.Append("		from	StbUser with(noLock) "                  + "\n");
                sbQuery.Append("		where	ServiceNum = '" + iServiceNum + "'"     + "\n");
                sbQuery.Append("		and		ServiceStatusCode in(1) " + "\n");
                sbQuery.Append("		union all " + "\n");
                // ���ϵ� ���������� �˻�
                sbQuery.Append("		select  2 as gubun, count(*) as cnt " + "\n");
                sbQuery.Append("		from    ClientList with(noLock) " + "\n");
                sbQuery.Append("		where	CollectionCode	= " +  iCollectionCode  + "\n");
                sbQuery.Append("		and     ServiceNum      = '" + iServiceNum + "'"    + "\n");
                 sbQuery.Append("	 ) v1 " + "\n");
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                if(ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception();
                }
                #endregion

                int userFound   =   Convert.ToInt32(ds.Tables[0].Rows[0]["Stb"].ToString());    // ����������� ��ϵǾ� �ִ� ��������
                int collFound   =   Convert.ToInt32(ds.Tables[0].Rows[0]["Clt"].ToString());    // ���ϵ� ��������
                int rc          = 0;
                ds.Dispose();
			
                //
                if( userFound == 0 )
                {
                    // �ش簡������ ��ϵ� ���� �����ϴ�.
                    targetcollectionModel.ResultCD   = "3101";
                    targetcollectionModel.ResultDesc = "����DB�� �������� �ʴ� �����Դϴ�.";
                    return;
                }

                if( collFound > 0 )
                {
                    targetcollectionModel.ResultCD   = "3100";
                    targetcollectionModel.ResultDesc = "��ϵǾ� �ִ� ���������� SKIP�մϴ�.";
                    return;
                }
                sbQuery = new StringBuilder();
                sbQuery.Append(" insert into ClientList ( CollectionCode, UserId, ServiceNum, RegDt ) " + "\n");
                sbQuery.Append(" select   " + iCollectionCode + "\n");
                sbQuery.Append("         ,UserId " + "\n");
                sbQuery.Append("         ,ServiceNum " + "\n");
                sbQuery.Append("         ,GetDate() " + "\n");
                sbQuery.Append(" from	StbUser noLock " + "\n");
                sbQuery.Append(" where	ServiceNum = '" + iServiceNum + "'" + "\n");
				sbQuery.Append(" and	ServiceStatusCode = 1"  + "\n");

				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// ��������
				try
				{
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
				}
				catch(Exception ex)
				{
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // ����
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3109";
				targetcollectionModel.ResultDesc = "������Ʈ���� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}
		}

        /// <summary>
        /// [E_01] Ư�� ������ ����� ����ڵ� ó��...
        /// OPENXML �̿��Ϸ� ������ ���� ó���� ���� ��� �м��� �����ؼ� ������ �� ROW�� ó�� �ϴ� ������� �� ��.(Ʈ����� ����)
        /// </summary>      
        public void SetClientCollectionCreate(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {
                int iCollectionCode = Convert.ToInt32(targetcollectionModel.CollectionCode);
                
                int x, rc = 0;
                int userFound = 0; // ����������� ��ϵǾ� �ִ� ��������
                int collFound = 0; // ���ϵ� ��������

                int totalCnt = 0; // ó�� �� ��ü ������ ��
                int readCnt = 0; // ó���� ������ ��
                int addCnt = 0;  // ������ �߰��� ��� ��
                int nonCnt = 0;  // ��ž����� �ȵ� ���� ��
                int skipCnt = 0; // skip ��
                int failCnt = 0; // ��� ���� ��

                // ������ �˼�
                if (targetcollectionModel.ClientListDataSet.Tables == null
                    || targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"] == null
                    || targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count == 0)
                    throw new Exception("�߰��� �����Ͱ� �����ϴ�!");

                StringBuilder sbQuery = new StringBuilder();
                
                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                #region �ش簡���� ����������� �����ϴ��� �˻� ����
                
                // 0:������, 1:�����, 2:����,3:�Ͻ��ߴ�,4:�������,5:������������,7:���డ��,9:�����ߴ�, 10:�������
                sbQuery.Append(" select  sum( case gubun when 1 then cnt else 0 end )	as	Stb " + "\n");
                sbQuery.Append("		,sum( case gubun when 2 then cnt else 0 end )	as	Clt " + "\n");
                sbQuery.Append(" from (                                       \n");
                sbQuery.Append("		select	1 as gubun ,count(*) as cnt   \n");
                sbQuery.Append("		from	StbUser with(noLock)          \n");
                sbQuery.Append("		where	ServiceNum = @iServiceNum     \n");
                sbQuery.Append("		and		ServiceStatusCode in(0,1,3)   \n"); // 0,1,3
                sbQuery.Append("		union all                             \n");
                // ���ϵ� ���������� �˻�
                sbQuery.Append("		select  2 as gubun, count(*) as cnt        \n");
                sbQuery.Append("		from    ClientList with(noLock)            \n");
                sbQuery.Append("		where	CollectionCode	= @iCollectionCode \n");
                sbQuery.Append("		and     ServiceNum      = @iServiceNum     \n");
                sbQuery.Append("	 ) v1       \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                #endregion

                #region ������ insert ����...
                StringBuilder sbQuery2 = new StringBuilder();
                sbQuery2.Append(" insert into ClientList ( CollectionCode, UserId, ServiceNum, RegDt )    \n");
                sbQuery2.Append(" select   @iCollectionCode          \n");
                sbQuery2.Append("         ,UserId                    \n");
                sbQuery2.Append("         ,ServiceNum                \n");
                sbQuery2.Append("         ,GetDate()                 \n");
                sbQuery2.Append(" from	StbUser noLock               \n");
                sbQuery2.Append(" where	ServiceNum = @iServiceNum    \n");
                sbQuery2.Append(" and	ServiceStatusCode in (0,1,3) \n"); // ������,�����, �Ͻ�����(query1�� ��������)


                // __DEBUG__
                _log.Debug(sbQuery2.ToString());
                // __DEBUG__
                #endregion

                totalCnt = targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count;

                SqlParameter[] sqlParams = new SqlParameter[2];
                x = 0;
                sqlParams[x++] = new SqlParameter("@iCollectionCode", SqlDbType.Int);
                sqlParams[x++] = new SqlParameter("@iServiceNum", SqlDbType.VarChar,12);               

                int groupCnt = 1;    // �׷� �ε���
                int processCnt = 10; // �׷� �� ó�� �� ������ ��
                if (totalCnt < processCnt) processCnt = 1;

                for (int i = 0; i < totalCnt; i+= processCnt) // ������ ó���� �����ͼ� ��ŭ ����
                {
                    _log.Debug("[SetClientCollectionCreate] �� �׷캰 ó�� ��:" + i.ToString());
                    for (int inx = i; inx < (processCnt * groupCnt); inx++) // ���� ���� loop
                    {
                        if (inx < totalCnt)
                        {                           
                            readCnt++;
                            try
                            {
                                DataRow row = targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows[inx]; // �ʹ� ��....
                                x = 0;
                                sqlParams[x++].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);
                                sqlParams[x++].Value = row["ServiceNum"].ToString();

                                #region [���翩�� Ȯ�ι� ��Ͽ��� Ȯ��] DataReader �̿�
                                using (SqlCommand cmd = new SqlCommand(sbQuery.ToString(), _db.SqlConn))
                                {                                    
                                    cmd.Parameters.Add("@iCollectionCode", SqlDbType.Int, 4);
                                    cmd.Parameters.Add("@iServiceNum", SqlDbType.VarChar, 12);
                                    
                                    cmd.Parameters[0].Value = sqlParams[0].Value;
                                    cmd.Parameters[1].Value = sqlParams[1].Value;
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            userFound = Convert.ToInt32(reader[0]);
                                            collFound = Convert.ToInt32(reader[1]);
                                        }
                                    }
                                    reader.Close();
                                }
                                if (userFound == 0) // ������, �����, �Ͻ����� ������ �����ڰ� ���ٸ� ��� �� �� ����.
                                    throw new Exception("3101"); // �ش簡������ ��ϵ� ���� �����ϴ�."����DB�� �������� �ʴ� �����Դϴ�.";

                                if (collFound > 0) // ������ ����� ������ ��ϵǾ� ����                        
                                    throw new Exception("3100"); //"��ϵǾ� �ִ� ���������� SKIP�մϴ�.";
                                #endregion
                                     
                                #region ��� ����
                                try
                                {
                                    _db.BeginTran();
                                    rc = _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams);
                                    _db.CommitTran();
                                }
                                catch (Exception ex)
                                {
                                    _log.Debug("��� ���� �߻�:" + ex.Message);
                                    _db.RollbackTran();
                                    throw new Exception("3000");
                                }

                                addCnt++; // �������� �߰�
                                #endregion                               
                            }
                            catch (Exception ex)
                            {
                                _log.Debug("[SetClientCollectionCreate] ����:" + ex.Message);
                                // ���� ���� �Ľ��ؼ� ������ڿ��� Ŭ���̾�Ʈ�� ����
                                if (ex.Message.Equals("3101")) // ���� ���� �ʴ� ����
                                    nonCnt++;
                                else if (ex.Message.Equals("3100")) // skip ����
                                    skipCnt++;
                                else if (ex.Message.Equals("3000")) // ��� ����
                                {
                                    failCnt++;
                                    _db.RollbackTran();
                                }
                                else
                                {
                                    failCnt++;                                   
                                }
                            }
                        } // end if(inx < totalCnt)
                    }
                    ++groupCnt; // ���� ����
                    // �� ������ ������ ���� ���� ó���� �Ͻ�����?       
                    //Thread.Sleep(5);
                    //_log.Debug("[Sleep] 100]");
                }                                

                targetcollectionModel.ResultCD = "0000";  // ����
                targetcollectionModel.ResultDesc = string.Format("{0:0},{1:0},{2:0},{3:0},{4:0},{5:0}", readCnt, totalCnt, addCnt, failCnt , nonCnt, skipCnt);
            }
            catch (Exception ex)
            {
                targetcollectionModel.ResultCD = "3109";
                targetcollectionModel.ResultDesc = ex.Message; //"������Ʈ���� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
                
            }
        }

        /// <summary>
        /// [E_02] Ư�������� ��� �� ����ڵ� ó�� openxml �̿��� ���ν��� ����
        /// 1 õ �Ǿ� �޾Ƽ� ���ο��� �� �� �Ǿ� ��� ó��..
        /// </summary>                
        public void SetClientCollectionCreateProc(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {
                int iCollectionCode = Convert.ToInt32(targetcollectionModel.CollectionCode);

                int x, rc = 0;              

                int totalCnt = 0; // ó�� �� ��ü ������ ��
                int readCnt = 0; // ó���� ������ ��
                int addCnt = 0;  // ������ �߰��� ��� ��
                int nonCnt = 0;  // ��ž����� �ȵ� ���� ��
                int skipCnt = 0; // skip ��
                int failCnt = 0; // ��� ���� ��

                // ������ �˼�
                if (targetcollectionModel.ClientListDataSet.Tables == null
                    || targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"] == null
                    || targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count == 0)
                    throw new Exception("�߰��� �����Ͱ� �����ϴ�!");

                StringBuilder sbQuery = new StringBuilder();

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();
                
                totalCnt = targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count;

                SqlParameter[] sqlParams = new SqlParameter[2];
                x = 0;
                sqlParams[x++] = new SqlParameter("@CollectionCode", SqlDbType.Int);               
                sqlParams[x++] = new SqlParameter("@xmlDocument", SqlDbType.VarChar,-1);

                x = 0;
                sqlParams[x++].Direction = ParameterDirection.Input;              
                sqlParams[x++].Direction = ParameterDirection.Input;                
                

                int groupCnt = 1;    // �׷� �ε���
                int processCnt = 400; // �׷� �� ó�� �� ������ ��
                int sendCount = 0;

                
                _log.Debug("[SetClientCollectionCreateProc] Start...");
                for (int i = 0; i < totalCnt; i += processCnt) // ������ ó���� �����ͼ� ��ŭ ����
                {
                    sbQuery.Clear();
                    sbQuery.Append("<ROOT>");
                   
                    for (int inx = i; inx < (processCnt * groupCnt); inx++) // ���� ���� loop
                    {
                        if (inx < totalCnt)
                        {
                            readCnt++;                            
                            try
                            {
                                sendCount++;
                                DataRow row = targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows[inx]; // �ʹ� ��....
                                sbQuery.Append("<Cus Code=\"" + targetcollectionModel.CollectionCode + "\" SNum=\"" + row["ServiceNum"].ToString() + "\" />");
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("������ ���� ����:" + ex.Message);
                            }
                        } // end if(inx < totalCnt)
                    }
                    sbQuery.Append("</ROOT>");

                    DataSet ds = new DataSet();
                    #region ��� ����
                    try
                    {
                        x = 0;
                        sqlParams[x++].Value = targetcollectionModel.CollectionCode;                       
                        sqlParams[x++].Value = sbQuery.ToString();

                        _db.BeginTran();                        
                        _db.ExecuteProcedure(ds, "[dbo].[TargetCollection_Create]", sqlParams);
                        _db.CommitTran();

                        if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                        {
                            nonCnt += sendCount - Convert.ToInt32(ds.Tables[0].Rows[0][0]); // ȸ�� �ƴ� ��(������ �� - ������� ��)
                            skipCnt += Convert.ToInt32(ds.Tables[0].Rows[0][1]);           // �̹� ��ϵ� ȸ�� 
                            addCnt += Convert.ToInt32(ds.Tables[0].Rows[0][2]);            // ���� �߰��� ��
                        }
                        else
                            throw new Exception("3000");
                    }
                    catch (Exception ex)
                    {
                        _log.Debug("��� ���� �߻�:" + ex.Message);
                        _db.RollbackTran();
                        throw new Exception("3000");
                    }
                    finally
                    {
                        sendCount = 0;
                        if (ds != null) ds = null;
                    }
                    #endregion                    
                    
                    ++groupCnt; // ���� �׷�  
                    Thread.Sleep(1000);
                }               
                targetcollectionModel.ResultCD = "0000";  // ����
                targetcollectionModel.ResultDesc = string.Format("{0:0},{1:0},{2:0},{3:0},{4:0},{5:0}", readCnt, totalCnt, addCnt, failCnt, nonCnt, skipCnt);
            }
            catch (Exception ex)
            {
                targetcollectionModel.ResultCD = "3109";
                targetcollectionModel.ResultDesc = ex.Message; //"������Ʈ���� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
                _log.Debug("[SetClientCollectionCreateProc] End...");
            }
        }

		public void SetTargetCollectionDelete(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			int ClientListCount = 0;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQueryClientListCount = new StringBuilder();
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[1];

				sbQueryClientListCount.Append( "\n"
					+ "        SELECT COUNT(*) FROM    ClientList with(noLock)  \n"
					+ "              WHERE CollectionCode  = @CollectionCode          	\n"
					);  

				sbQuery.Append(""
					+ "DELETE Collection         \n"
					+ " WHERE CollectionCode  = @CollectionCode  \n"
					);

				sqlParams[i++] = new SqlParameter("@CollectionCode"       , SqlDbType.Int);

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);

				// ��������
				try
				{
					//��ü���౤���� ���� Count����///////////////////////////////////////////////
					// __DEBUG__
					_log.Debug(sbQueryClientListCount.ToString());
					// __DEBUG__

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQueryClientListCount.ToString(),sqlParams);
                    
					ClientListCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					_log.Debug("ClientListCount          -->" + ClientListCount);

					// �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ� Exception�� �߻���Ų��.
					if(ClientListCount > 0) throw new Exception();


					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("Ÿ�ٱ���������:[" + targetcollectionModel.CollectionCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // ����
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3301";
				// �̹� �ٸ����̺� ������� �����Ͱ� �ִٸ�
				if(ClientListCount > 0 )
				{
					targetcollectionModel.ResultDesc = "��ϵ� ��ü����簡 �����Ƿ� �̵������� �����Ҽ� �����ϴ�.";
				}
				else
				{
					targetcollectionModel.ResultDesc = "�̵����� ������ ������ �߻��Ͽ����ϴ�";
				}
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

        /// <summary>
        /// 1���� �� �����͸� ����
        /// </summary>
        /// <param name="header"></param>
        /// <param name="targetcollectionModel"></param>
		public void SetClientDelete(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientDelete() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("CollectionCode  :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("UserId          :[" + targetcollectionModel.UserId       + "]");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];

				sbQuery.Append(""
					+ "DELETE ClientList         \n"
					+ " WHERE CollectionCode  = @CollectionCode  \n"
					+ " AND UserId  = @UserId  \n"
					);

				sqlParams[i++] = new SqlParameter("@CollectionCode"     , SqlDbType.Int);				
				sqlParams[i++] = new SqlParameter("@UserId"     , SqlDbType.Int);				
				
				i = 0;								
				sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);
				sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.UserId);
				// ��������

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				try
				{
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("ī�װ���������:[" + targetcollectionModel.CollectionCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // ����
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3301";
				targetcollectionModel.ResultDesc = "������ ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

        /// <summary>
        /// ���� �� ���� ������ ������ ����[E_01]
        /// </summary>      
        public void SetClientCollectionDelete(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientCollectionDelete() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("CollectionCode  :[" + targetcollectionModel.CollectionCode + "]");
                _log.Debug("UserId          :[" + targetcollectionModel.UserId + "]");

                if (targetcollectionModel.ClientListDataSet.Tables.Count == 0
                   || targetcollectionModel.ClientListDataSet.Tables["Clients"] == null
                   || targetcollectionModel.ClientListDataSet.Tables["Clients"].Rows.Count == 0)
                    throw new Exception("ó���� �����Ͱ� �����ϴ�.!");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                int i = 0;
                int rc = 0;

                SqlParameter[] sqlParams = new SqlParameter[2];

                sbQuery.Append(""
                    + "DELETE ClientList         \n"
                    + " WHERE CollectionCode  = @CollectionCode  \n"
                    + " AND UserId  = @UserId  \n"
                    );

                _db.BeginTran();
                
                foreach (DataRow row in targetcollectionModel.ClientListDataSet.Tables[0].Rows)
                {
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@UserId", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(row["CollectionCode"]);
                    sqlParams[i++].Value = Convert.ToInt32(row["UserId"]);
                                                            
                   
                   rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                   _log.Message("���� ����� ���� :[" + targetcollectionModel.CollectionCode + "] �����:[" + header.UserID + "]");                   
                }

                _db.CommitTran();

                targetcollectionModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientCollectionDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                targetcollectionModel.ResultCD = "3301";
                targetcollectionModel.ResultDesc = "������ ������ ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }


        /// <summary> 
        /// Ư�� ������ ������Ʈ�� ��� ������ ��������� ����/�̵�
        /// </summary>
        /// <param name="header"></param>
        /// <param name="targetcollectionModel"></param>
        public void SetClientListCopyMove(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientListCopy() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("FromCode  :[" + targetcollectionModel.FromCode + "]");
                _log.Debug("ToCode    :[" + targetcollectionModel.ToCode + "]");
                _log.Debug("CopyMove  :[" + targetcollectionModel.CopyMove + "]");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = null;
                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = null;

                _db.BeginTran();
                try
                {
                    // �������� TO�� ������Ʈ�� ����

                    #region TO�� ������Ʈ�� ����

                    sbQuery = new StringBuilder();
                    sqlParams = new SqlParameter[1];

                    sbQuery.Append("--������Ʈ �̵��� ����\n"
                        + "DELETE ClientList         \n"
                        + " WHERE CollectionCode  = @ToCode  \n"
                        );

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@ToCode", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.ToCode);
                    // ��������

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    #endregion

                    // ���� From ���� To ��
                    #region From ���� To �� ����
                    sbQuery = new StringBuilder();
                    sqlParams = new SqlParameter[2];

                    sbQuery.Append("--������Ʈ ����\n"
                        + "INSERT INTO ClientList         \n"
                        + "     SELECT A.CollectionCode     \n"
                        + "           ,A.UserId \n"
                        + "           ,A.RegDt \n"
                        + "           ,A.ServiceNum \n"
                        + "     FROM          \n"
                        + "     (SELECT @ToCode AS CollectionCode \n"
                        + "           ,UserId \n"
                        + "           ,RegDt \n"
                        + "           ,ServiceNum \n"
                        + "       FROM ClientList   \n"
                        + "      WHERE CollectionCode = @FromCode \n"
                        + "      EXCEPT \n"                             //EXCEPT�� �̿��Ͽ� Ű�ߺ� ����
                        + "     SELECT CollectionCode \n"
                        + "           ,UserId \n"
                        + "           ,RegDt \n"
                        + "           ,ServiceNum \n"
                        + "       FROM ClientList   \n"
                        + "      WHERE CollectionCode = @ToCode) A \n"
                        );

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@FromCode", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@ToCode", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.FromCode);
                    sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.ToCode);
                    // ��������
                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    #endregion

                    // �̵����� �˻�
                    if(targetcollectionModel.CopyMove.Equals("M"))
                    {
                        // �̵��̹Ƿ� From�� ������Ʈ�� �����Ѵ�.
                        #region From ����
                        sbQuery = new StringBuilder();
                        sqlParams = new SqlParameter[1];

                        sbQuery.Append("--������Ʈ �̵��� ����\n"
                            + "DELETE ClientList         \n"
                            + " WHERE CollectionCode  = @FromCode  \n"
                            );

                        i = 0;
                        sqlParams[i++] = new SqlParameter("@FromCode", SqlDbType.Int);

                        i = 0;
                        sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.FromCode);
                        // ��������

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        // __DEBUG__

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                        #endregion
                    }

                    string at = "";
                    if (targetcollectionModel.CopyMove.Equals("M")) at = "�̵�";
                    else at = "����";

                    _log.Message("������Ʈ " + at + ":From[" + targetcollectionModel.FromCode + "] To[" + targetcollectionModel.FromCode + "] �����:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _db.CommitTran();

                targetcollectionModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientListCopyMove() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                targetcollectionModel.ResultCD = "3301";
                targetcollectionModel.ResultDesc = "������Ʈ ����/�̵� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }
	}
}
