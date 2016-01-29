using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Target
{
	/// <summary>
	/// TargetingCollectionBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class TargetingCollectionBiz : BaseBiz
	{
		public TargetingCollectionBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		/// ���������ȸ
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetCollectionList(HeaderModel header, TargetingCollectionModel model)
		{
			try
			{
				_log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + model.SearchKey + "]");
                _log.Debug("SearchNonuseYn :[" + model.SearchNonuseYn + "]");
                // __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append("\nSELECT  A.CollectionCode, A.CollectionName, A.Comment ");
				sbQuery.Append("\n       ,(Select count(UserId) from ClientList where CollectionCode = A.CollectionCode AND CollectionCode = A.CollectionCode) AS STBCnt ");
				sbQuery.Append("\n       ,COUNT(B.ItemNo) AS ItemCnt ");
				sbQuery.Append("\n       ,SUM(CASE WHEN B.SetType = '1' AND C.AdType IN ('10','15','16','17','19') THEN 1 ELSE 0 END) AS CMCnt ");
				sbQuery.Append("\n       ,SUM(CASE WHEN B.SetType = '1' AND C.AdType IN ('11','12','20') THEN 1 ELSE 0 END) AS OAPCnt ");
				sbQuery.Append("\n       ,SUM(CASE WHEN B.SetType = '2' THEN 1 ELSE 0 END) AS HomeCnt ");
				sbQuery.Append("\n  FROM Collection A LEFT JOIN TargetingCollection B ON (A.CollectionCode = B.CollectionCode) ");
				sbQuery.Append("\n                    LEFT JOIN ContractItem C ON (B.ItemNo = C.ItemNo) ");
				sbQuery.Append("\n WHERE 1 = 1  ");

                if ((model.SearchNonuseYn.Trim().Length > 0) && model.SearchNonuseYn.Trim().Equals("N"))
                {
                    sbQuery.Append("\n   AND A.UseYn = 'Y' \n");
                }

				// �˻�� ������
                if (model.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ( A.CollectionName LIKE '%" + model.SearchKey.Trim() + "%' \n"
                                 + "    OR A.Comment        LIKE '%" + model.SearchKey.Trim() + "%' \n"
						         + " ) ");
				}

                sbQuery.Append(" GROUP BY A.CollectionCode, A.CollectionName, A.Comment\n");
                sbQuery.Append(" ORDER BY A.CollectionCode DESC \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
                model.CollectionsDataSet = ds.Copy();
				// ���
                model.ResultCnt = Utility.GetDatasetCount(model.CollectionsDataSet);
				// ����ڵ� ��Ʈ
                model.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
                model.ResultCD = "3000";
                model.ResultDesc = "���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}




        /// <summary>
        /// Ÿ���� ������ ��ȸ
        /// </summary>
        /// <param name="targetcollectionModel"></param>
        public void GetTargetingCMList(HeaderModel header, TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + model.SearchKey + "]");
                _log.Debug("SearchKey      :[" + model.SetType + "]");
                _log.Debug("CollectionCode :[" + model.CollectionCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("\nSELECT 'False' AS CheckYn, A.ItemNo, B.ItemName ");
                sbQuery.Append("\n  FROM TargetingCollection A LEFT  JOIN ContractItem B ON (A.ItemNo = B.ItemNo) ");
                sbQuery.Append("\n WHERE 1 = 1   ");
                sbQuery.Append("\n   AND A.SetType = '1' -- 1:�Ϲ� 2:Ȩ���� ");
                sbQuery.Append("\n   AND B.AdType IN ('10','15','16','17','19') -- ��������");
                sbQuery.Append("\n   AND A.CollectionCode = " + model.CollectionCode + " " );
                sbQuery.Append("\n ORDER BY A.ItemNo DESC  ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �𵨿� ����
                model.CMDataSet = ds.Copy();
                // ���
                model.ResultCnt = Utility.GetDatasetCount(model.CMDataSet);
                // ����ڵ� ��Ʈ
                model.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "Ÿ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }




        /// <summary>
        /// ��ü������ ��ȸ
        /// </summary>
        /// <param name="targetcollectionModel"></param>
        public void GetTargetingOAPList(HeaderModel header, TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + model.SearchKey + "]");
                _log.Debug("CollectionCode :[" + model.CollectionCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("\nSELECT 'False' AS CheckYn,  A.ItemNo, B.ItemName ");
                sbQuery.Append("\n  FROM TargetingCollection A LEFT  JOIN ContractItem B ON (A.ItemNo = B.ItemNo) ");
                sbQuery.Append("\n WHERE 1 = 1   ");
                sbQuery.Append("\n   AND A.SetType = '1' -- 1:�Ϲ� 2:Ȩ���� ");
                sbQuery.Append("\n   AND B.AdType IN ('11','12','20')  -- ��ü����� ");
                sbQuery.Append("\n   AND A.CollectionCode = " + model.CollectionCode + " ");
                sbQuery.Append("\n ORDER BY A.ItemNo DESC  ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �𵨿� ����
                model.OAPDataSet = ds.Copy();
                // ���
                model.ResultCnt = Utility.GetDatasetCount(model.OAPDataSet);
                // ����ڵ� ��Ʈ
                model.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "Ÿ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }



        /// <summary>
        /// Ȩ������ ��ȸ
        /// </summary>
        /// <param name="targetcollectionModel"></param>
        public void GetTargetingHomeList(HeaderModel header, TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + model.SearchKey + "]");
                _log.Debug("CollectionCode :[" + model.CollectionCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("\nSELECT 'False' AS CheckYn,  A.ItemNo, B.ItemName ");
                sbQuery.Append("\n  FROM TargetingCollection A LEFT  JOIN ContractItem B ON (A.ItemNo = B.ItemNo) ");
                sbQuery.Append("\n WHERE 1 = 1   ");
                sbQuery.Append("\n   AND A.SetType = '2' -- 1:�Ϲ� 2:Ȩ���� ");
                sbQuery.Append("\n   AND A.CollectionCode = " + model.CollectionCode + " ");
                sbQuery.Append("\n ORDER BY A.ItemNo DESC  ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �𵨿� ����
                model.HomeDataSet = ds.Copy();
                // ���
                model.ResultCnt = Utility.GetDatasetCount(model.HomeDataSet);
                // ����ڵ� ��Ʈ
                model.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "Ÿ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }


        #region ����Ÿ���� �߰�
        /// <summary>
        /// ���� ���
        /// </summary>
        /// <param name="header"></param>
        /// <param name="ratioModel"></param>
        public void SetTargetingCollectionAdd(HeaderModel header, TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("SetType        :[" + model.SetType + "]");
                _log.Debug("ItemNo         :[" + model.ItemNo + "]");
                _log.Debug("CollectionCode :[" + model.CollectionCode + "]");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[3];

                sbQuery.Append(""
                    + "INSERT INTO TargetingCollection ( \n"
                    + "      SetType                  \n"
                    + "		,ItemNo                   \n"
                    + "		,CollectionCode           \n"
                    + "      )                        \n"
                    + " VALUES(                       \n"
                    + "       @SetType  -- 1:�Ϲ� 2:Ȩ���� \n"
                    + "      ,@ItemNo				  \n"
                    + "      ,@CollectionCode	      \n"
                    + "		)						  \n"

                    );

                sqlParams[i++] = new SqlParameter("@SetType", SqlDbType.Char,1);
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = model.SetType;
                sqlParams[i++].Value = Convert.ToInt32(model.ItemNo);
                sqlParams[i++].Value = Convert.ToInt32(model.CollectionCode);


                _log.Debug(sbQuery.ToString());

                // ��������
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                model.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3101";
                model.ResultDesc = "����Ÿ���� �߰��� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }


        #endregion


        #region ����Ÿ���� ����
        /// <summary>
        /// ���� ���
        /// </summary>
        /// <param name="header"></param>
        /// <param name="ratioModel"></param>
        public void SetTargetingCollectionDelete(HeaderModel header, TargetingCollectionModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("SetType        :[" + model.SetType + "]");
                _log.Debug("ItemNo         :[" + model.ItemNo + "]");
                _log.Debug("CollectionCode :[" + model.CollectionCode + "]");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[3];

                sbQuery.Append(""
                    + "DELETE FROM TargetingCollection         \n"
                    + "	WHERE SetType        = @SetType   -- 1:�Ϲ� 2:Ȩ����  \n"
                    + "   AND ItemNo         = @ItemNo         \n"
                    + "	  AND CollectionCode = @CollectionCode \n"
                    );

                sqlParams[i++] = new SqlParameter("@SetType", SqlDbType.Char, 1);
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = model.SetType;
                sqlParams[i++].Value = Convert.ToInt32(model.ItemNo);
                sqlParams[i++].Value = Convert.ToInt32(model.CollectionCode);


                _log.Debug(sbQuery.ToString());

                // ��������
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                model.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3101";
                model.ResultDesc = "����Ÿ���� ������ ������ �߻��Ͽ����ϴ�";
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
