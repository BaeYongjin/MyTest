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
	/// Class1�� ���� ��� �����Դϴ�.
	/// </summary>
	public class CategoryBiz : BaseBiz
	{
		public CategoryBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		/// <summary>
		/// ī�װ������ȸ
		/// </summary>
		/// <param name="categoryModel"></param>
		public void GetCategoryList(HeaderModel header, CategoryModel categoryModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCategoryList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + categoryModel.SearchKey       + "]");
				_log.Debug("SearchCategoryLevel:[" + categoryModel.SearchCategoryLevel + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n	SELECT	  A.MediaCode ");
				sbQuery.Append("\n			, A.CategoryCode ");
				sbQuery.Append("\n			, A.CategoryName ");
				sbQuery.Append("\n			, A.ModDt ");
				sbQuery.Append("\n			, B.MediaName ");
				sbQuery.Append("\n			, CASE A.Flag			WHEN 'Y' THEN 'True' Else 'False' END AS Flag ");
				sbQuery.Append("\n			, CASE A.CSSFlag		WHEN 'Y' THEN 'True' Else 'False' END AS CheckYn ");
				sbQuery.Append("\n			, CASE A.CSSFlag		WHEN 'Y' THEN 'True' Else 'False' END AS FlagName ");
				sbQuery.Append("\n			, CASE A.InventoryYn	WHEN 'Y' THEN 'True' Else 'False' END AS InventoryYn ");
				sbQuery.Append("\n			, A.SortNo ");
				sbQuery.Append("\n			, isnull(A.InventoryRate,0.0) as InventoryRate ");
				sbQuery.Append("\n	FROM	Category A with(noLock) , Media B with(noLock) ");
				sbQuery.Append("\n	WHERE A.MediaCode = B.MediaCode ");
					
				// �˻�� ������
				if (categoryModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
						+ "    A.CategoryName      LIKE '%" + categoryModel.SearchKey.Trim() + "%' \n"
						+ " OR B.MediaName    LIKE '%" + categoryModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}

				// ī�װ������� ����������
				if(categoryModel.SearchCategoryLevel.Trim().Length > 0 && !categoryModel.SearchCategoryLevel.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode = '" + categoryModel.SearchCategoryLevel.Trim() + "' \n");
				}			

				sbQuery.Append(" ORDER BY A.MediaCode,case A.SortNo when 0 then 99999 else A.SortNo end, A.CategoryCode \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ī�װ��𵨿� ����
				categoryModel.UserDataSet = ds.Copy();
				// ���
				categoryModel.ResultCnt = Utility.GetDatasetCount(categoryModel.UserDataSet);
				// ����ڵ� ��Ʈ
				categoryModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + categoryModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUsersList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				categoryModel.ResultCD = "3000";
				categoryModel.ResultDesc = "ī�װ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}


		/// <summary>
		/// ī�װ����� ����
		/// 2010�� 7��11�� ��뼮 �κ��丮���� ����
		/// </summary>
		public void SetCategoryUpdate(HeaderModel header, CategoryModel categoryModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCategoryUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[8];

				sbQuery.Append(""
					+ "UPDATE Category							\n"
					+ "   SET CategoryName	= @CategoryName     \n"					
					+ "      ,ModDt			= GETDATE()			\n"
					+ "      ,Flag			= @Flag				\n"
					+ "      ,SortNo		= @SortNo			\n"
					+ "      ,CSSFlag		= @CheckYn			\n"
					+ "      ,InventoryYn	= @InventoryYn		\n"
					+ "      ,InventoryRate = @InventoryRate	\n"
					+ " WHERE MediaCode     = @MediaCode        \n"
					+ " AND CategoryCode    = @CategoryCode     \n"	);

				i = 0;
				sqlParams[i++] = new SqlParameter("@CategoryName"	, SqlDbType.VarChar,20);				
				sqlParams[i++] = new SqlParameter("@Flag"			, SqlDbType.Char,	1);
				sqlParams[i++] = new SqlParameter("@SortNo"			, SqlDbType.Int,	4);
				sqlParams[i++] = new SqlParameter("@CheckYn"		, SqlDbType.Char,	1);
				sqlParams[i++] = new SqlParameter("@InventoryYn"	, SqlDbType.Char,	1);
				sqlParams[i++] = new SqlParameter("@InventoryRate"	, SqlDbType.Decimal,8);
				sqlParams[i++] = new SqlParameter("@MediaCode"		, SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@CategoryCode"	, SqlDbType.Int);
						
				i = 0;
				sqlParams[i++].Value = categoryModel.CategoryName;						
				sqlParams[i++].Value = categoryModel.Flag;
				sqlParams[i++].Value = Convert.ToInt32(categoryModel.SortNo);
				sqlParams[i++].Value = categoryModel.CssFlag;
				sqlParams[i++].Value = categoryModel.InventoryYn;
				sqlParams[i++].Value = Convert.ToDecimal(categoryModel.InventoryRate);
				sqlParams[i++].Value = Convert.ToInt32(categoryModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(categoryModel.CategoryCode);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("ī�װ���������:["+categoryModel.CategoryCode + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				categoryModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCategoryUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				categoryModel.ResultCD   = "3201";
				categoryModel.ResultDesc = "ī�װ����� ������ ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		/// ī�װ� ����
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
		public void SetCategoryCreate(HeaderModel header, CategoryModel categoryModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCategoryCreate() Start");
				_log.Debug("-----------------------------------------");

//				// �����ͺ��̽��� OPEN�Ѵ�
//				_db.Open();
//
//
//				StringBuilder sbQuery = new StringBuilder();
//				_log.Debug("Flag:[" + categoryModel.CheckYn + "]");
//			
//				int i = 0;
//				int rc = 0;
//				SqlParameter[] sqlParams = new SqlParameter[3];
//
//				sbQuery.Append( ""
//					+ "INSERT INTO Category (                         \n"
//					+ "       MediaCode ,CategoryCode ,CategoryName,ModDt, Flag \n"					
//					+ "      )                                          \n"
//					+ " SELECT                                         \n"
//					+ "      @MediaCode      \n"						
//					+ "      ,ISNULL(MAX(CategoryCode),0)+1        \n"
//					+ "      ,@CategoryName      \n"					
//					+ "      ,GETDATE()      \n"					
//					+ "      ,@CheckYn      \n"					
//					+ " FROM Category		\n"					
//					);
//
//				
//				sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt);
//				sqlParams[i++] = new SqlParameter("@CategoryName" , SqlDbType.VarChar , 20);
//				sqlParams[i++] = new SqlParameter("@CheckYn" , SqlDbType.Char , 1);
//								
//				i = 0;
//				sqlParams[i++].Value = Convert.ToInt32(categoryModel.MediaCode);
//				sqlParams[i++].Value = categoryModel.CategoryName;
//				sqlParams[i++].Value = categoryModel.CheckYn;
//				
//				// __DEBUG__
//				_log.Debug(sbQuery.ToString());
//				// __DEBUG__
//
//
//				// ��������
//				try
//				{
//					_db.BeginTran();
//					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
//					_db.CommitTran();
//
//					// __MESSAGE__
//					_log.Message("ī�װ���������:[" + categoryModel.CategoryCode + "(" + categoryModel.CategoryName + ")] �����:[" + header.UserID + "]");
//
//				}
//				catch(Exception ex)
//				{
//					_db.RollbackTran();
//					throw ex;
//				}

				categoryModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCategoryCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				categoryModel.ResultCD   = "3101";
				categoryModel.ResultDesc = "ī�װ����� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		public void SetCategoryDelete(HeaderModel header, CategoryModel categoryModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCategoryDelete() Start");
				_log.Debug("-----------------------------------------");

//				// �����ͺ��̽��� OPEN�Ѵ�
//				_db.Open();
//
//				StringBuilder sbQuery = new StringBuilder();
//
//				int i = 0;
//				int rc = 0;
//				SqlParameter[] sqlParams = new SqlParameter[2];
//
//				sbQuery.Append(""
//					+ "DELETE Category         \n"
//					+ " WHERE MediaCode  = @MediaCode  \n"
//					+ " AND CategoryCode  = @CategoryCode  \n"
//					);
//
//				sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.TinyInt);
//				sqlParams[i++] = new SqlParameter("@CategoryCode"       , SqlDbType.Int);
//
//				i = 0;
//				sqlParams[i++].Value = categoryModel.MediaCode;
//				sqlParams[i++].Value = categoryModel.CategoryCode;
//
//				// ��������
//				try
//				{
//					_db.BeginTran();
//					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
//					_db.CommitTran();
//
//					// __MESSAGE__
//					_log.Message("ī�װ���������:[" + categoryModel.CategoryCode + "] �����:[" + header.UserID + "]");
//
//				}
//				catch(Exception ex)
//				{
//					_db.RollbackTran();
//					throw ex;
//				}

				categoryModel.ResultCD = "0000";  // ����
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetCategoryDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				categoryModel.ResultCD   = "3301";
				categoryModel.ResultDesc = "ī�װ����� ������ ������ �߻��Ͽ����ϴ�";
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