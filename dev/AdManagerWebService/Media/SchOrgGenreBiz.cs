using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Media
{
    /// <summary>
    /// SchOrgGenreBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class SchOrgGenreBiz : BaseBiz
    {
        public SchOrgGenreBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

		#region �޴� �����ȸ
		/// <summary>
		/// �޴� �����ȸ
		/// </summary>
		/// <param name="SchOrgGenreModel"></param>
		/// 
		public void GetMenuList(HeaderModel header, SchOrgGenreModel SchOrgGenreModel)
		{
			try
			{
                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
                _log.Debug("SearchMediaCode         :[" + SchOrgGenreModel.SearchMediaCode + "]");
                _log.Debug("IsSetDataOnly           :[" + SchOrgGenreModel.IsSetDataOnly + "]");
                _log.Debug("SearchKey               :[" + SchOrgGenreModel.SearchKey + "]");

                // __DEBUG__

                bool IsSetDataOnly = SchOrgGenreModel.IsSetDataOnly;
				StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append(" SELECT  	                                        \n");
                sbQuery.Append("         B.CategoryCode	AS CategoryCode      	    \n");
                sbQuery.Append("  		 ,B.CategoryName AS CategoryName	    	\n");
                sbQuery.Append("         ,B.GenreCode AS MenuCode	    	        \n");
                sbQuery.Append("         ,B.GenreName AS MenuName         	        \n");
                sbQuery.Append("         ,CONVERT(CHAR(19), A.UseDate, 120) AS UseDate     	 \n");
                sbQuery.Append("   FROM SchOrgGenre A     	 \n");
                //���� ���õ� �޴��� �����ð����� ��� �޴� ����� �ҷ� �ð�����....
                if(IsSetDataOnly)
                    sbQuery.Append(" 		INNER JOIN vMenuList    B with(NoLock)      \n");
                else
                    sbQuery.Append("  		RIGHT OUTER JOIN vMenuList    B with(NoLock) \n");
                sbQuery.Append("  		       ON (A.Menu1    = B.CategoryCode   AND A.Menu2    = B.GenreCode )        	 \n");
                sbQuery.Append("   WHERE  B.MediaCode ='" + SchOrgGenreModel.SearchMediaCode.Trim() + "'        	         \n");

                // �˻�� ������
                if (SchOrgGenreModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append(" AND ("
                        + "    B.CategoryName      LIKE '%" + SchOrgGenreModel.SearchKey.Trim() + "%' \n"
                        + " OR B.GenreName    LIKE '%" + SchOrgGenreModel.SearchKey.Trim() + "%' \n"
                        + " ) ");
                }

                sbQuery.Append("   ORDER BY B.CategoryCode, B.GenreCode     \n");

			
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ���������𵨿� ����
				SchOrgGenreModel.SchOrgGenreDataSet = ds.Copy();
				// ���
				SchOrgGenreModel.ResultCnt = Utility.GetDatasetCount(SchOrgGenreModel.SchOrgGenreDataSet);
				// ����ڵ� ��Ʈ
				SchOrgGenreModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + SchOrgGenreModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMenuList() End");
				_log.Debug("-----------------------------------------");


			}
			catch(Exception ex)
			{
				SchOrgGenreModel.ResultCD = "3000";
				SchOrgGenreModel.ResultDesc = "�޴����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
		}

        #endregion

        #region ���帣��� OAP �� ����
        /// <summary>
        /// ���帣��� OAP �� ����
        /// </summary>
        /// <param name="header"></param>
        /// <param name="SchOrgGenreModel"></param>
        public void UpdateSchOrgGenre(HeaderModel header, SchOrgGenreModel SchOrgGenreModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "UpdateSchOrgGenre() Start");
                _log.Debug("-----------------------------------------");

                //DB OPEN
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("MediaCode      :[" + SchOrgGenreModel.MediaCode + "]");
                _log.Debug("CategoryCode   :[" + SchOrgGenreModel.CategoryCode + "]");
                _log.Debug("MenuCode       :[" + SchOrgGenreModel.MenuCode + "]");
                _log.Debug("UseDate        :[" + SchOrgGenreModel.UseDate + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[4];

                int rc = 0;

                sbQuery.Append("\n  UPDATE SchOrgGenre                ");
                sbQuery.Append("\n  SET   UseDate         = @UseDate     ");
                sbQuery.Append("\n        ,ModDate         = getdate()    ");
                sbQuery.Append("\n        ,RegId         = @RegId    ");
                sbQuery.Append("\n WHERE    ");
                sbQuery.Append("\n      Menu1    = @CategoryCode"    );
                sbQuery.Append("\n    AND Menu2        = @MenuCode"  );

                sqlParams[0] = new SqlParameter("@UseDate", SqlDbType.DateTime);
                sqlParams[1] = new SqlParameter("@RegId", SqlDbType.VarChar);
                sqlParams[2] = new SqlParameter("@CategoryCode", SqlDbType.Int);
                sqlParams[3] = new SqlParameter("@MenuCode", SqlDbType.Int);

                sqlParams[0].Value = Convert.ToDateTime(SchOrgGenreModel.UseDate);
                sqlParams[1].Value = header.UserID;
                sqlParams[2].Value = Convert.ToInt32(SchOrgGenreModel.CategoryCode);
                sqlParams[3].Value = Convert.ToInt32(SchOrgGenreModel.MenuCode);

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __DEBUG__
                    _log.Message("���帣��� OAP �� ����:[" + SchOrgGenreModel.CategoryCode + "]");
                    // __DEBUG__
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                SchOrgGenreModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "UpdateSchOrgGenre() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                SchOrgGenreModel.ResultCD = "3201";
                SchOrgGenreModel.ResultDesc = "���帣��� OAP �� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }
        #endregion

        #region ���帣��� OAP �� ����
        /// <summary>
        /// ���帣��� OAP �� ����
        /// </summary>
        /// <param name="header"></param>
        /// <param name="SchOrgGenreModel"></param>
        public void InsertSchOrgGenre(HeaderModel header, SchOrgGenreModel SchOrgGenreModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "InsertSchOrgGenre() Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("CategoryCode   :[" + SchOrgGenreModel.CategoryCode + "]");
                _log.Debug("MenuCode       :[" + SchOrgGenreModel.MenuCode + "]");
                _log.Debug("UseDate        :[" + SchOrgGenreModel.UseDate + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[4];

                int rc = 0;

                sbQuery.Append("\n  INSERT INTO SchOrgGenre (                      ");
                sbQuery.Append("\n          CugCode                       ");
                sbQuery.Append("\n         ,Menu1                        ");
                sbQuery.Append("\n         ,Menu2                        ");
                sbQuery.Append("\n         ,Menu3                        ");
                sbQuery.Append("\n         ,Menu4                        ");
                sbQuery.Append("\n         ,ChannelNo                   ");
                sbQuery.Append("\n         ,UseDate                        ");
                sbQuery.Append("\n         ,ModDate                        ");
                sbQuery.Append("\n         ,RegDate                        ");
                sbQuery.Append("\n         ,RegId                        ");
                sbQuery.Append("\n      )                                   ");
                sbQuery.Append("\n      Values (                              ");
                sbQuery.Append("\n         1                       ");
                sbQuery.Append("\n         ,@CategoryCode       ");
                sbQuery.Append("\n         ,@MenuCode                       ");
                sbQuery.Append("\n         ,0                  ");
                sbQuery.Append("\n         ,0                             ");
                sbQuery.Append("\n         ,0                   ");
                sbQuery.Append("\n         ,@UseDate                        ");
                sbQuery.Append("\n         ,getdate()                        ");
                sbQuery.Append("\n         ,getdate()                        ");
                sbQuery.Append("\n         ,@RegId                        ");
                sbQuery.Append("\n      )		                    ");


                sqlParams[0] = new SqlParameter("@UseDate", SqlDbType.DateTime);
                sqlParams[1] = new SqlParameter("@CategoryCode", SqlDbType.Int);
                sqlParams[2] = new SqlParameter("@MenuCode", SqlDbType.Int);
                sqlParams[3] = new SqlParameter("@RegId", SqlDbType.VarChar);

                sqlParams[0].Value = Convert.ToDateTime(SchOrgGenreModel.UseDate);
                sqlParams[1].Value = Convert.ToInt32(SchOrgGenreModel.CategoryCode);
                sqlParams[2].Value = Convert.ToInt32(SchOrgGenreModel.MenuCode);
                sqlParams[3].Value = header.UserID;

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("���帣��� OAP �� ����:[" + SchOrgGenreModel.CategoryCode + "-" + SchOrgGenreModel.MenuCode + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                SchOrgGenreModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "InsertSchOrgGenre() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                SchOrgGenreModel.ResultCD = "3101";
                SchOrgGenreModel.ResultDesc = "���帣��� OAP �� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }
        #endregion

        #region ���帣��� OAP �� ����
        /// <summary>
        /// ���帣��� OAP �� ����
        /// </summary>
        /// <param name="header"></param>
        /// <param name="SchOrgGenreModel"></param>
        public void DeleteSchOrgGenre(HeaderModel header, SchOrgGenreModel SchOrgGenreModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "DeleteSchOrgGenre() Start");
                _log.Debug("-----------------------------------------");

                //DB OPEN
                _db.Open();

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("MediaCode      :[" + SchOrgGenreModel.MediaCode + "]");
                _log.Debug("CategoryCode   :[" + SchOrgGenreModel.CategoryCode + "]");
               // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                SqlParameter[] sqlParams = new SqlParameter[2];

                int rc = 0;

                sbQuery.Append("\n  DELETE SchOrgGenre         ");
                sbQuery.Append("\n WHERE                            ");
                sbQuery.Append("\n      Menu1    = @CategoryCode    ");
                sbQuery.Append("\n    AND Menu2        = @MenuCode  ");

                sqlParams[0] = new SqlParameter("@CategoryCode", SqlDbType.Int);
                sqlParams[1] = new SqlParameter("@MenuCode", SqlDbType.Int);

                sqlParams[0].Value = Convert.ToInt32(SchOrgGenreModel.CategoryCode);
                sqlParams[1].Value = Convert.ToInt32(SchOrgGenreModel.MenuCode);

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __DEBUG__
                    _log.Message("���帣��� OAP �� ����:[" + SchOrgGenreModel.CategoryCode + "]");
                    // __DEBUG__
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                SchOrgGenreModel.ResultCD = "0000";

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "DeleteSchOrgGenre() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                SchOrgGenreModel.ResultCD = "3201";
                SchOrgGenreModel.ResultDesc = "���帣��� OAP �� ���� �� ������ �߻��Ͽ����ϴ�";
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