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
	/// GenreBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class GenreBiz : BaseBiz
	{
		public GenreBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		/// <summary>
		/// �帣�����ȸ
		/// </summary>
		/// <param name="genreModel"></param>
		public void GetGenreList(HeaderModel header, GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + genreModel.SearchKey       + "]");
				_log.Debug("SearchGenreLevel:[" + genreModel.SearchGenreLevel + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT A.MediaCode, A.GenreCode, A.GenreName, A.ModDt, B.MediaName  \n"							
					+ "  FROM Genre A, Media B                \n"										
					+ "  WHERE A.MediaCode = B.MediaCode    \n"										
					);
				
				// �˻�� ������
				if (genreModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
						+ "    A.GenreName      LIKE '%" + genreModel.SearchKey.Trim() + "%' \n"	
						+ " OR B.MediaName    LIKE '%" + genreModel.SearchKey.Trim() + "%' \n"							
						+ " ) ");
				}

				// �帣������ ����������
				if(genreModel.SearchGenreLevel.Trim().Length > 0 && !genreModel.SearchGenreLevel.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode = '" + genreModel.SearchGenreLevel.Trim() + "' \n");
				}			

				sbQuery.Append(" ORDER BY A.MediaCode, A.GenreCode \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �帣�𵨿� ����
				genreModel.UserDataSet = ds.Copy();
				// ���
				genreModel.ResultCnt = Utility.GetDatasetCount(genreModel.UserDataSet);
				// ����ڵ� ��Ʈ
				genreModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + genreModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUsersList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				genreModel.ResultCD = "3000";
				genreModel.ResultDesc = "�帣���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}


		/// <summary>
		/// �帣���� ����
		/// </summary>
		/// <param name="UserID"></param>
		/// <param name="UserName"></param>
		/// <param name="UserPassword"></param>
		/// <param name="UserLevel"></param>
		/// <param name="UserDept"></param>
		/// <param name="UserTitle"></param>
		/// <param name="UserTell"></param>
		/// <param name="UserMobile"></param>
		/// <param name="UserComment"></param>
		/// <returns></returns>
		public void SetGenreUpdate(HeaderModel header, GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[3];

				sbQuery.Append(""
					+ "UPDATE Genre                     \n"
					+ "   SET GenreName      = @GenreName      \n"					
					+ "      ,ModDt      = GETDATE()      \n"					
					+ " WHERE MediaCode        = @MediaCode        \n"
					+ " AND GenreCode        = @GenreCode        \n"
					);

				i = 0;
				sqlParams[i++] = new SqlParameter("@GenreName"     , SqlDbType.VarChar , 20);				
				sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@GenreCode"     , SqlDbType.Int);
		

				i = 0;
				sqlParams[i++].Value = genreModel.GenreName;						
				sqlParams[i++].Value = Convert.ToInt32(genreModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(genreModel.GenreCode);

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�帣��������:["+genreModel.GenreCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				genreModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				genreModel.ResultCD   = "3201";
				genreModel.ResultDesc = "�帣���� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// �帣 ����
		/// </summary>
		/// <param name="UserID"></param>
		/// <param name="UserName"></param>
		/// <param name="UserPassword"></param>
		/// <param name="UserLevel"></param>
		/// <param name="UserDept"></param>
		/// <param name="UserTitle"></param>
		/// <param name="UserTell"></param>
		/// <param name="UserMobile"></param>
		/// <param name="UserComment"></param>
		/// <returns></returns>
		public void SetGenreCreate(HeaderModel header, GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];

				sbQuery.Append( ""
					+ "INSERT INTO Genre (                         \n"
					+ "       MediaCode ,GenreCode ,GenreName,ModDt \n"					
					+ "      )                                          \n"
					+ " SELECT                                         \n"
					+ "      @MediaCode      \n"						
					+ "      ,ISNULL(MAX(GenreCode),0)+1        \n"
					+ "      ,@GenreName,GETDATE()      \n"						
					+ " FROM Genre		\n"					
					);

				
				sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@GenreName" , SqlDbType.VarChar , 20);
								
				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(genreModel.MediaCode);
				sqlParams[i++].Value = genreModel.GenreName;
				
				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�帣��������:[" + genreModel.GenreCode + "(" + genreModel.GenreName + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				genreModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				genreModel.ResultCD   = "3101";
				genreModel.ResultDesc = "�帣���� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		public void SetGenreDelete(HeaderModel header, GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];

				sbQuery.Append(""
					+ "DELETE Genre         \n"
					+ " WHERE MediaCode  = @MediaCode  \n"
					+ " AND GenreCode  = @GenreCode  \n"
					);

				sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@GenreCode"       , SqlDbType.Int);

				i = 0;
				sqlParams[i++].Value = genreModel.MediaCode;
				sqlParams[i++].Value = genreModel.GenreCode;

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("�帣��������:[" + genreModel.GenreCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				genreModel.ResultCD = "0000";  // ����
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGenreDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				genreModel.ResultCD   = "3301";
				genreModel.ResultDesc = "�帣���� ������ ������ �߻��Ͽ����ϴ�";
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

