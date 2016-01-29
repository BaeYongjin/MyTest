using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.ContractPackage
{
    /// <summary>
    /// ContractPackageBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class ContractPackageBiz : BaseBiz
    {
        public ContractPackageBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }


        /// <summary>
        /// ������Ű�������ȸ
        /// </summary>
        /// <param name="contractPackageModel"></param>
        public void GetContractPackageList(HeaderModel header, ContractPackageModel contractPackageModel)
        {
            try
            {  // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open(); 

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractPackageList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + contractPackageModel.SearchKey       + "]");
				_log.Debug("SearchRap      :[" + contractPackageModel.SearchRap       + "]");
				_log.Debug("SearchUse      :[" + contractPackageModel.SearchUse       + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
					+ " SELECT a.PackageNo     \n"     
					+ "		  ,a.PackageName   \n"
					+ "       ,a.RapCode       \n"
				    + "       ,ISNULL(c.RapName,'����') AS RapName \n"
					+ "		  ,a.AdTime        \n"
					+ "		  ,a.ContractAmt   \n"
					+ "		  ,a.BonusRate     \n"
					+ "		  ,a.Price         \n"
					+ "		  ,a.Comment          					 \n"
					+ "		  ,a.UseYn         \n"
					+ "		  ,CASE a.UseYn  WHEN 'N'  THEN '������' ELSE '' END UseNo  \n"
					+ "       ,convert(char(19), a.RegDt, 120) RegDt \n"      
					+ "	 	  ,convert(char(19), a.ModDt, 120) ModDt \n"
					+ "       ,b.UserName RegName					 \n"
					+ "  FROM ContractPackage a with(NoLock)         \n"
					+ "       LEFT JOIN SystemUser b with(NoLock) ON (a.RegId   = b.UserId)  \n"
					+ "       LEFT JOIN MediaRap   c with(NoLock) ON (a.RapCode = c.RapCode) \n"
					+ " WHERE  1 = 1  \n"
					);
				       
				if(!contractPackageModel.SearchRap.Equals("00"))
				{
					sbQuery.Append("  AND(  a.RapCode = '"+contractPackageModel.SearchRap+"' OR a.RapCode = 0 ) \n");
				}        


                // �˻�� ������
                if (contractPackageModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("\n"
                        + "  AND ( a.PackageName LIKE '%" + contractPackageModel.SearchKey.Trim() + "%' \n"
						+ "     OR c.RapName     LIKE '%" + contractPackageModel.SearchKey.Trim() + "%' \n"
						+ "     OR a.Comment     LIKE '%" + contractPackageModel.SearchKey.Trim() + "%' \n"
						+ " 	)       \n"
                        );
                }

				if(!contractPackageModel.SearchUse.Equals("N"))
				{
					sbQuery.Append("  AND a.UseYN = 'Y' \n");
				}        

       
                sbQuery.Append("  ORDER BY A.PackageNo");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // ��� DataSet�� ������Ű���𵨿� ����
                contractPackageModel.PackageDataSet = ds.Copy();
                // ���
                contractPackageModel.ResultCnt = Utility.GetDatasetCount(contractPackageModel.PackageDataSet);
                // ����ڵ� ��Ʈ
                contractPackageModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + contractPackageModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractPackageList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                contractPackageModel.ResultCD = "3000";
                contractPackageModel.ResultDesc = "������Ű������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }



        /// <summary>
        /// ������Ű�� ����
        /// </summary>
        /// <returns></returns>
        public void SetContractPackageCreate(HeaderModel header, ContractPackageModel contractPackageModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractPackageCreate() Start");
                _log.Debug("-----------------------------------------");
				_log.Debug("PackageName      :[" + contractPackageModel.PackageName   + "]");
				_log.Debug("RapCode          :[" + contractPackageModel.RapCode       + "]");
				_log.Debug("AdTime	         :[" + contractPackageModel.AdTime	      + "]");		
				_log.Debug("ContractAmt      :[" + contractPackageModel.ContractAmt   + "]");	
				_log.Debug("BonusRate        :[" + contractPackageModel.BonusRate     + "]");		
				_log.Debug("Price            :[" + contractPackageModel.Price         + "]");		
				_log.Debug("UseYn            :[" + contractPackageModel.UseYn         + "]");		
				_log.Debug("Comment          :[" + contractPackageModel.Comment       + "]");			
                               
                    
                //AdGroup Insert Query
                StringBuilder sbQuery = new StringBuilder();
                                                                 
                // ��������
                try
                {
                                                
                    int i = 0;
                    int rc = 0;
                    //���� �׷� Insert
                    SqlParameter[] sqlParams = new SqlParameter[11];

					sbQuery.Append("\n"
						+ "INSERT INTO ContractPackage ( \n"
						+ "            PackageNo     \n"
						+ "           ,PackageName	 \n"
						+ "           ,RapCode       \n"
						+ "		      ,AdTime        \n"
						+ "		      ,ContractAmt   \n"
						+ "		      ,BonusRate     \n"
						+ "		      ,Price         \n"
						+ "		      ,UseYn         \n"
						+ "		      ,Comment 		 \n"
						+ "           ,RegDt         \n"      
						+ "	 	      ,ModDt         \n"
						+ "           ,RegId         \n"
						+ "           )              \n"
						+ "     SELECT               \n"
						+ "            (SELECT ISNULL(Max(PackageNo),'0')+1 FROM ContractPackage) \n"
						+ "           ,@PackageName  \n"
						+ "           ,@RapCode      \n"
						+ "           ,@AdTime       \n"
						+ "           ,@ContractAmt  \n"
						+ "           ,@BonusRate    \n"
						+ "           ,@Price        \n"
						+ "           ,@UseYn        \n"
						+ "           ,@Comment      \n"
						+ "           ,GETDATE()     \n"
						+ "           ,GETDATE()     \n"
						+ "           ,@RegID        \n"
						);
                                                    
                                
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@PackageName"  , SqlDbType.VarChar    ,   50);
					sqlParams[i++] = new SqlParameter("@RapCode"      , SqlDbType.Int              );
					sqlParams[i++] = new SqlParameter("@AdTime"       , SqlDbType.Int              );
					sqlParams[i++] = new SqlParameter("@ContractAmt"  , SqlDbType.Int              );
                    sqlParams[i++] = new SqlParameter("@BonusRate"    , SqlDbType.Int              );
					sqlParams[i++] = new SqlParameter("@Price"        , SqlDbType.Int              );
					sqlParams[i++] = new SqlParameter("@UseYn"        , SqlDbType.Char       ,    1);
					sqlParams[i++] = new SqlParameter("@Comment"      , SqlDbType.VarChar    , 2000);
                    sqlParams[i++] = new SqlParameter("@RegID"        , SqlDbType.VarChar    ,   10);
                                                     
                                                      
                    i = 0;
					sqlParams[i++].Value = contractPackageModel.PackageName;
					if(contractPackageModel.RapCode.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToInt16(contractPackageModel.RapCode);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractPackageModel.AdTime.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToInt32(contractPackageModel.AdTime);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractPackageModel.ContractAmt.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToInt32(contractPackageModel.ContractAmt);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractPackageModel.BonusRate.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToInt16(contractPackageModel.BonusRate);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractPackageModel.Price.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToInt32(contractPackageModel.Price);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					sqlParams[i++].Value = contractPackageModel.UseYn;
					sqlParams[i++].Value = contractPackageModel.Comment;
					sqlParams[i++].Value = header.UserID;
                                    
                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

					_db.BeginTran();
                                
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                   
                    // __MESSAGE__
					_log.Message("������Ű����������:["+contractPackageModel.PackageName + "] �����:[" + header.UserID + "]");
            
                    _db.CommitTran();
            
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                                                    
                contractPackageModel.ResultCD = "0000";  // ����
                                                    
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractPackageCreate() End");
                _log.Debug("-----------------------------------------");	
                                        
            }
            catch(Exception ex)
            {
                contractPackageModel.ResultCD   = "3101";
                contractPackageModel.ResultDesc = "������Ű������ ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        // ������Ű������ ����

        public void SetContractPackageUpdate(HeaderModel header, ContractPackageModel contractPackageModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractPackageUpdate() Start");
                _log.Debug("-----------------------------------------");
                        


				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("PackageNo        :[" + contractPackageModel.PackageNo     + "]");
				_log.Debug("PackageName      :[" + contractPackageModel.PackageName   + "]");
				_log.Debug("RapCode          :[" + contractPackageModel.RapCode       + "]");
				_log.Debug("AdTime	         :[" + contractPackageModel.AdTime	      + "]");		
				_log.Debug("ContractAmt      :[" + contractPackageModel.ContractAmt   + "]");	
				_log.Debug("BonusRate        :[" + contractPackageModel.BonusRate     + "]");		
				_log.Debug("Price            :[" + contractPackageModel.Price         + "]");		
				_log.Debug("UseYn            :[" + contractPackageModel.UseYn         + "]");		
				_log.Debug("Comment          :[" + contractPackageModel.Comment       + "]");			
				// __DEBUG__


                StringBuilder sbQuery = new StringBuilder();
                        
                int i = 0;
                int rc = 0;

				// ��������
                try
                {
                    SqlParameter[] sqlParams = new SqlParameter[12];

 					sbQuery.Append("\n"
						+ "UPDATE ContractPackage              \n"
						+ "   SET  PackageName = @PackageName  \n"		
						+ "       ,RapCode     = @RapCode       \n"
						+ "       ,AdTime      = @AdTime       \n"
						+ "       ,ContractAmt = @ContractAmt  \n"
						+ "       ,BonusRate   = @BonusRate    \n"
						+ "       ,Price       = @Price        \n"
						+ "       ,UseYn       = @UseYn        \n"
						+ "       ,Comment     = @Comment      \n"
						+ "       ,ModDt       = GETDATE()     \n"
						+ "       ,RegID       = @RegID        \n"
						+ " WHERE  PackageNo   = @PackageNo    \n"
						);

					i = 0;
					sqlParams[i++] = new SqlParameter("@PackageNo"    , SqlDbType.Int              );	
					sqlParams[i++] = new SqlParameter("@PackageName"  , SqlDbType.VarChar    ,   50);
					sqlParams[i++] = new SqlParameter("@RapCode"      , SqlDbType.Int              );
					sqlParams[i++] = new SqlParameter("@AdTime"       , SqlDbType.Int              );
					sqlParams[i++] = new SqlParameter("@ContractAmt"  , SqlDbType.Int              );
					sqlParams[i++] = new SqlParameter("@BonusRate"    , SqlDbType.Int              );
					sqlParams[i++] = new SqlParameter("@Price"        , SqlDbType.Int              );
					sqlParams[i++] = new SqlParameter("@UseYn"        , SqlDbType.Char       ,    1);
					sqlParams[i++] = new SqlParameter("@Comment"      , SqlDbType.VarChar    , 2000);
					sqlParams[i++] = new SqlParameter("@RegID"        , SqlDbType.VarChar    ,   10);

                    i = 0;
					sqlParams[i++].Value = Convert.ToInt32(contractPackageModel.PackageNo);		
					sqlParams[i++].Value = contractPackageModel.PackageName;
					if(contractPackageModel.RapCode.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToInt16(contractPackageModel.RapCode);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractPackageModel.AdTime.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToInt32(contractPackageModel.AdTime);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractPackageModel.ContractAmt.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToInt32(contractPackageModel.ContractAmt);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractPackageModel.BonusRate.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToInt16(contractPackageModel.BonusRate);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					if(contractPackageModel.Price.Trim().Length > 0)
					{
						sqlParams[i++].Value = Convert.ToInt32(contractPackageModel.Price);		
					}
					else
					{
						sqlParams[i++].Value = 0;		
					}
					sqlParams[i++].Value = contractPackageModel.UseYn;
					sqlParams[i++].Value = contractPackageModel.Comment;
					sqlParams[i++].Value = header.UserID;



					_db.BeginTran();

                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _db.CommitTran();
                        
                    //__MESSAGE__
                    _log.Message("������Ű����������:[" + contractPackageModel.PackageNo.ToString() + "]["+contractPackageModel.PackageName + "] �����:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                contractPackageModel.ResultCD = "0000";  // ����
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractPackageUpdate() End");
                _log.Debug("-----------------------------------------");
                        
            }
            catch(Exception ex)
            {
                contractPackageModel.ResultCD   = "3201";
                contractPackageModel.ResultDesc = "������Ű������ ������ ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }



        public void SetContractPackageDelete(HeaderModel header, ContractPackageModel contractPackageModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractPackageDelete() Start");
                _log.Debug("-----------------------------------------");

				_log.Debug("PackageNo        :[" + contractPackageModel.PackageNo     + "]");
				_log.Debug("PackageName      :[" + contractPackageModel.PackageName   + "]");
                        
                StringBuilder sbQuery    = new StringBuilder();

                // ��������
                try
                {
                    int rc = 0;
                    int i = 0;
                    SqlParameter[] sqlParams = new SqlParameter[1];

					sbQuery.Append( "\n"
						+ " DELETE FROM  ContractPackage   \n"
						+ "  WHERE PackageNo = @PackageNo  \n"
						);      
                	                
                     
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@PackageNo"     , SqlDbType.Int );
                                                   
                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(contractPackageModel.PackageNo);

                    _db.BeginTran();
          
                    //���� ��Ű�� ������ ���� �Ѵ�.
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__
                        
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("������Ű�� ���� ����:[" + contractPackageModel.PackageNo.ToString() + "] " + contractPackageModel.PackageName   + " �����:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                contractPackageModel.ResultCD = "0000";  // ����
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContractPackageDelete() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                contractPackageModel.ResultCD   = "3101";
				_log.Exception(ex);
				contractPackageModel.ResultDesc = "������Ű�� ���� �� ������ �߻��Ͽ����ϴ�";
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

          
        }
    }
}