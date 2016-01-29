/*
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�	: [E_01]
 * ������		: YJ.Park
 * ������		: 2014.11.13
 * ��������	: HomeCount�� Ȩ����(Ű��) �� �߰�
 * --------------------------------------------------------
 */
using System;
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
	/// GradeBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class GradeBiz : BaseBiz
	{
		public GradeBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		///  ����޺���ȸ
		/// </summary>
		/// <param name="gradeModel"></param>
		public void GetGradeCodeList(HeaderModel header, GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGradeList() Start");
				_log.Debug("-----------------------------------------");

				
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("Section :[" + gradeModel.MediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT Code, CodeName  \n"
					+ "   FROM GradeCode               \n"				
					);
			

				sbQuery.Append(" ORDER BY Code \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				gradeModel.GradeDataSet = ds.Copy();
				// ���
				gradeModel.ResultCnt = Utility.GetDatasetCount(gradeModel.GradeDataSet);
				// ����ڵ� ��Ʈ
				gradeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + gradeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGradeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				gradeModel.ResultCD = "3000";
				gradeModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}

		/// <summary>
		/// ä�μ¸����ȸ
		/// </summary>
		/// <param name="gradeModel"></param>
		public void GetGradeList(HeaderModel header, GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGradeList() Start");
				_log.Debug("-----------------------------------------");
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");				
				_log.Debug("Code:[" + gradeModel.Code + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// ��������
				sbQuery.Append("\n"
					+ " SELECT		Code											\n"
					+ "			   ,CodeName										\n"					
					+ "			   ,Grade											\n"
					+ "			   ,RegID											\n"
					+ " 		   ,convert(char(10), RegDt, 120) RegDt				\n"
					+ " 		   ,convert(char(10), ModDt, 120) ModDt				\n"
					+ "   FROM GradeCode with(noLock)          						\n"					
					);								

				// ä�μ·����� ����������
				if(gradeModel.MediaCode.Trim().Length > 0 && !gradeModel.MediaCode.Trim().Equals("00"))
				{
					sbQuery.Append(" WHERE Code = '" + gradeModel.Code.Trim() + "' \n");
				}		
								
				sbQuery.Append("\n ORDER BY Code");

				// __DEBUG__
				_log.Debug("MediaCode:[" + gradeModel.Code + "]");
				
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				gradeModel.GradeDataSet = ds.Copy();
				// ���
				gradeModel.ResultCnt = Utility.GetDatasetCount(gradeModel.GradeDataSet);
				// ����ڵ� ��Ʈ
				gradeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + gradeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGradeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				gradeModel.ResultCD = "3000";
				gradeModel.ResultDesc = "������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}
		}
		

		/// <summary>
		/// ī�װ� �帣 �����ȸ
		/// </summary>
		/// <param name="gradeModel"></param>
		/// 
		public void GetContractItemList(HeaderModel header, GradeModel gradeModel)
		{
			try
			{   // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + gradeModel.SearchKey       + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n");
				sbQuery.Append("SELECT * FROM ( 																													\n");     
				sbQuery.Append("SELECT a.ItemNo 																														\n");     
				sbQuery.Append("      ,a.ItemName																														\n");     
				sbQuery.Append("      ,e.ContractName																												\n");     
				sbQuery.Append("	  ,a.AdvertiserCode																													\n");
				sbQuery.Append("      ,b.AdvertiserName																												\n");
				sbQuery.Append("      ,a.ExcuteStartDay																												\n");
				sbQuery.Append("	  ,a.ExcuteEndDay																													\n");
				sbQuery.Append("	  ,a.RealEndDay																														\n");
				sbQuery.Append("	  ,a.AdState 																															\n");				
				sbQuery.Append("      ,c.CodeName AdStateName																									\n");
				sbQuery.Append("	  ,a.AdClass 																															\n");
				sbQuery.Append("      ,a.FilePath																															\n");
				sbQuery.Append("      ,a.FileName																														\n");
				sbQuery.Append("      ,a.FileState																														\n");
				sbQuery.Append("      ,d.CodeName FileStateName																									\n");
				sbQuery.Append("      ,(SELECT COUNT(*) FROM SchHome						WHERE ItemNo = a.ItemNo)");
				sbQuery.Append("			+(SELECT COUNT(*) FROM SchHomeKids				WHERE ItemNo = a.ItemNo)AS HomeCount		\n");	// [E_01]
				sbQuery.Append("      ,(SELECT COUNT(*) FROM SchChoiceMenuDetail		WHERE ItemNo = a.ItemNo) AS MenuCount		\n");
				sbQuery.Append("      ,(SELECT COUNT(*) FROM SchChoiceChannelDetail	WHERE ItemNo = a.ItemNo) AS ChannelCount	\n");
				sbQuery.Append("      ,CONVERT(CHAR(8),GETDATE(),112) AS NowDay																		\n");
				sbQuery.Append("      ,a.AdType																															\n");
				sbQuery.Append("      ,f.CodeName AdTypeName																									\n");
				sbQuery.Append("      ,a.LinkChannel																													\n");				
				sbQuery.Append("      ,(SELECT TOP 1 ProgramNm FROM Program    WHERE Channel = a.LinkChannel ORDER BY ProgramKey DESC) AS LinkChannelNm      \n");
				sbQuery.Append("      ,a.Mgrade																															\n");
				sbQuery.Append("      ,a.HomeYn																															\n");
				sbQuery.Append("      ,a.ChannelYn																														\n");
				sbQuery.Append("FROM   ContractItem a with(NoLock)																							\n");
				sbQuery.Append("       LEFT JOIN Advertiser b with(NoLock) ON (a.AdvertiserCode = b.AdvertiserCode)							\n");
				sbQuery.Append("       LEFT JOIN Contract   e with(NoLock) ON (a.ContractSeq    = e.ContractSeq)								\n");    
				sbQuery.Append("       LEFT JOIN SystemCode c with(NoLock) ON (a.AdState        = c.Code AND c.Section = '25' )			\n");      
				sbQuery.Append("       LEFT JOIN SystemCode d with(NoLock) ON (a.FileState      = d.Code AND d.Section = '31' )			\n");
				sbQuery.Append("       LEFT JOIN SystemCode f with(NoLock) ON (a.AdType         = f.Code AND f.Section = '26' )				\n");    				
				sbQuery.Append(" WHERE a.AdType = '20'																												\n");
				
				if(!gradeModel.MediaCode.Equals("00"))
				{
					sbQuery.Append("  AND    a.MediaCode = '"+gradeModel.MediaCode+"'  \n");
				}
				if(!gradeModel.Code.Equals("00"))
				{
					sbQuery.Append("  AND    a.Mgrade = '"+gradeModel.Code+"'  \n");
				}        				
				bool isState = false;
				// ������� ���ÿ� ����

				// ������� �غ�
				if(gradeModel.SearchchkAdState_10.Trim().Length > 0 && gradeModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND ( A.AdState  = '10' \n");
					isState = true;
				}	
				// ������� ��
				if(gradeModel.SearchchkAdState_20.Trim().Length > 0 && gradeModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '20' \n");
					isState = true;
				}	
				// ������� ����
				if(gradeModel.SearchchkAdState_30.Trim().Length > 0 && gradeModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '30' \n");
					isState = true;
				}	
				// ������� ����
				if(gradeModel.SearchchkAdState_40.Trim().Length > 0 && gradeModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				// �˻�� ������
				if (gradeModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append("\n"
						+ "  AND ( a.ItemName LIKE '%" + gradeModel.SearchKey.Trim() + "%' OR		\n"
						+ "        a.FileName LIKE '%" + gradeModel.SearchKey.Trim() + "%'     \n"
						+ " OR b.AdvertiserName    LIKE '%" + gradeModel.SearchKey.Trim() + "%'			\n"
						+ "		)        \n"
						);
				}

				sbQuery.Append(""
					+ ") TA "
					+ "WHERE 1 = 1"
					);

				// �����ΰ� Y/N�� �� 
//				if(gradeModel.SearchChkSch_YN.Trim().Length > 0)
//				{
//					if(gradeModel.SearchChkSch_YN.Trim().Equals("Y"))
//					{
//						// ���Ȱ͸�
//						sbQuery.Append(" AND ( HomeCount > 0 OR MenuCount > 0 OR  ChannelCount > 0 ) \n");
//					}
//					else if(gradeModel.SearchChkSch_YN.Trim().Equals("N"))
//					{
//						//���ȵȰ͸�
//						sbQuery.Append(" AND ( HomeCount = 0 AND MenuCount = 0 AND  ChannelCount = 0 )\n");
//					}
//				}
				// �ƴϸ� ��ü
       
				sbQuery.Append("ORDER BY ItemNo Desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �������𵨿� ����
				gradeModel.ContractItemDataSet = ds.Copy();
				// ���
				gradeModel.ResultCnt = Utility.GetDatasetCount(gradeModel.ContractItemDataSet);
				// ����ڵ� ��Ʈ
				gradeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + gradeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				gradeModel.ResultCD = "3000";
				gradeModel.ResultDesc = "���������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		/// <summary>
		///  ä������ ����
		/// </summary>
		/// <param name="header"></param>
		/// <param name="gradeModel"></param>
		public void SetGradeUpdate(HeaderModel header, GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGradeUpdate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				
				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[4];

				sbQuery.Append(""
					+ "UPDATE GradeCode                     \n"
					+ "   SET Code          = @Code      \n"					
					+ "      ,CodeName      = @CodeName      \n"
					+ "      ,Grade         = @Grade      \n"
					+ "      ,ModDt         = GETDATE()      \n"					
					+ " WHERE Code          = @Code        \n"					
					);

				i = 0;
				sqlParams[i++] = new SqlParameter("@Code"     , SqlDbType.VarChar , 4);				
				sqlParams[i++] = new SqlParameter("@CodeName"     , SqlDbType.VarChar, 50);
				sqlParams[i++] = new SqlParameter("@Grade"     , SqlDbType.VarChar , 4);
				sqlParams[i++] = new SqlParameter("@Code_O"     , SqlDbType.VarChar , 4);
						

				i = 0;
				sqlParams[i++].Value = gradeModel.Code;						
				sqlParams[i++].Value = gradeModel.CodeName;					
				sqlParams[i++].Value = gradeModel.Grade;					
				sqlParams[i++].Value = gradeModel.Code_O;					
				

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
					_log.Message("ī�װ���������:["+gradeModel.Code_O + "] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				gradeModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGradeUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				gradeModel.ResultCD   = "3101";
				gradeModel.ResultDesc = "������� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		/// <summary>
		/// ä�μ� ���
		/// </summary>
		/// <param name="header"></param>
		/// <param name="gradeModel"></param>
		public void SetGradeCreate(HeaderModel header, GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGradeCreate() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[4];
				
				sbQuery.Append( ""
					+ "INSERT INTO GradeCode (                         \n"
					+ "		 Code                \n"															
					+ "		,CodeName                \n"															
					+ "		,Grade                \n"															
					+ "		,RegDt                \n"															
					+ "		,RegID                \n"																																									
					+ "      )                                          \n"
					+ " VALUES(                                        \n"			
					+ "       @Code      \n"						
					+ "      ,@CodeName      \n"		
					+ "      ,@Grade      \n"		
					+ "      ,GETDATE()      \n"		
					+ "      ,@RegID         \n"									
					+ "  )  	\n"					
					);                		
								
				sqlParams[i++] = new SqlParameter("@Code"     , SqlDbType.VarChar , 4);				
				sqlParams[i++] = new SqlParameter("@CodeName"     , SqlDbType.VarChar, 50);
				sqlParams[i++] = new SqlParameter("@Grade"     , SqlDbType.VarChar , 4);
				sqlParams[i++] = new SqlParameter("@RegID"     , SqlDbType.VarChar , 10);
						

				i = 0;
				sqlParams[i++].Value = gradeModel.Code;						
				sqlParams[i++].Value = gradeModel.CodeName;					
				sqlParams[i++].Value = gradeModel.Grade;					
				sqlParams[i++].Value = header.UserID;				// �����		
								
				
				_log.Debug("Code:[" + gradeModel.Code + "]");
				_log.Debug("CodeName:[" + gradeModel.CodeName + "]");
				_log.Debug("Grade:[" + gradeModel.Grade + "]");
								
				_log.Debug(sbQuery.ToString());

				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("ä�μ���������:[" + gradeModel.Code + "(" + gradeModel.Code + ")] �����:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				gradeModel.ResultCD = "0000";  // ����

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGradeCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				gradeModel.ResultCD   = "3101";
				gradeModel.ResultDesc = "ä�μ��������� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		public void SetGradeDelete(HeaderModel header, GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGradeDelete() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();
                        
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[1];
                        
				sbQuery.Append( ""
					+ "        DELETE FROM  GradeCode			                    \n"
					+ "              WHERE Code = @Code				\n"					
					);                             
                                        
				sqlParams[i++] = new SqlParameter("@Code"     , SqlDbType.VarChar , 4);	
			        
				i = 0;				
				sqlParams[i++].Value = gradeModel.Code;		

				_log.Debug("Code:[" + gradeModel.Code + "]");				
				
				_log.Debug(sbQuery.ToString());
                        
				// ��������
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();
                        
					// __MESSAGE__
					_log.Message("ä����������:[" + gradeModel.Code + "] �����:[" + header.UserID + "]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				gradeModel.ResultCD = "0000";  // ����
                        	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGradeDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				gradeModel.ResultCD   = "3301";
				gradeModel.ResultDesc = "������� ������ ������ �߻��Ͽ����ϴ�";
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