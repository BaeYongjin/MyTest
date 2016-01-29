using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

namespace AdManagerWebService.Interface
{
	/// <summary>
	/// InterfaceBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class InterfaceBiz : BaseBiz
	{
		public InterfaceBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		/// �����ָ����ȸ for CMS
		/// </summary>
		/// <param name="SearchKey"></param>
		public string GetAdvertiserList(string SearchKey)
		{

			StringBuilder sbReturn = new StringBuilder();

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".GetAdvertiserList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + SearchKey       + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT AdvertiserCode, AdvertiserName, Comment, UseYn  \n"
					+ "  FROM Advertiser with(NoLock)  \n"			
					+ " WHERE 1 = 1  \n"							
					+ "   AND UseYn = 'Y' \n" 
					);
				
				// �˻�� ������
				if (SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
						+ "    AdvertiserName      LIKE '%" + SearchKey.Trim() + "%' \n"	
						+ " ) ");
				}

				sbQuery.Append(" ORDER BY AdvertiserCode DESC  \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				System.IO.StringWriter xmlSW = new System.IO.StringWriter(sbReturn);

				ds.WriteXml(xmlSW, System.Data.XmlWriteMode.IgnoreSchema);

				ds.Dispose();

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("Return---------\n");
				_log.Debug(sbReturn.ToString());
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdvertiserList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				sbReturn.Append("���������� ��ȸ�� ������ �߻��Ͽ����ϴ�.(CMS)");
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}		

			return sbReturn.ToString();
		}

		
		/// <summary>
		/// ����������Ʈ for CMS
		/// </summary>
		/// <param name="SearchKey"></param>
		public string GetContractItemList()
		{

			StringBuilder sbReturn = new StringBuilder();

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".GetContractItemList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				//_log.Debug("SearchKey      :[" + SearchKey       + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " select    ItemNo  \n"
					+ "			 ,ItemName  \n"
					+ "			 ,ExcuteStartDay  \n"
					+ "			 ,case AdType  \n"
					+ "				when '10' then 'c'  \n"					
					+ "				when '11' then 'o'  \n"
					+ "				when '12' then 's'  \n"
					+ "				when '13' then 'c'  \n"
					+ "				when '14' then 'c'  \n"
					+ "				when '15' then 'c'  \n"
					+ "				when '20' then 'o'  \n"
					+ "				when '99' then 'd'  \n"
					+ "				else   'x'  \n"
					+ "			  end as AdType  \n"
					+ "		from dbo.ContractItem  \n"
					+ "		where FileState in('10','11')  \n"
					+ "		  and    AdState < '40'  \n"										
					);
								

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				System.IO.StringWriter xmlSW = new System.IO.StringWriter(sbReturn);

				ds.WriteXml(xmlSW, System.Data.XmlWriteMode.IgnoreSchema);

				ds.Dispose();

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("Return---------\n");
				_log.Debug(sbReturn.ToString());
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				sbReturn.Append("����������Ʈ ��ȸ�� ������ �߻��Ͽ����ϴ�.(CMS)");
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}		

			return sbReturn.ToString();
		}


		// 2007.12.27 RH.Jung �����ȸ �Լ� �߰� for CMS
		/// <summary>
		/// ����������ȸ for CMS
		/// </summary>
		/// <param name="string"></param>
		public string GetContractList(string SearchKey)
		{

			StringBuilder sbReturn = new StringBuilder();

			try
			{  // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".GetContractList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + SearchKey       + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ "SELECT  a.ContractSeq							  \n"
					+ "       ,a.ContractName							  \n"
					+ "       ,a.ContStartDay							  \n"
					+ "       ,a.ContEndDay								  \n"
					+ "       ,g.CodeName StateName						  \n"
					+ "  FROM  Contract a with(NoLock)                    \n"
					+ "        LEFT JOIN SystemCode g with(NoLock) ON (a.State          = g.Code AND g.Section='23' ) \n"
					+ " WHERE 1=1 \n"
					);

				// �˻�� ������
				if (SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append("\n"
						+ "  AND ( a.ContractName   LIKE '%" + SearchKey.Trim() + "%' \n"
						+ "  )      \n"
						);
				}
       
				sbQuery.Append("  ORDER BY a.ContractSeq Desc");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				System.IO.StringWriter xmlSW = new System.IO.StringWriter(sbReturn);

				ds.WriteXml(xmlSW, System.Data.XmlWriteMode.IgnoreSchema);

				ds.Dispose();

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("Return---------\n");
				_log.Debug(sbReturn.ToString());
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".GetContractList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				sbReturn.Append("���������� ��ȸ�� ������ �߻��Ͽ����ϴ�.(CMS)");
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

			return sbReturn.ToString();

		}

		/// <summary>
		/// ����������Ʈ �˼����� ������Ʈ for CMS
		/// </summary>
		/// <param name="SearchKey"></param>
		public string SetFileCheckReady(int itemNo, string FileName)
		{

			StringBuilder sbReturn = new StringBuilder();

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetFileCheckReady() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				//_log.Debug("SearchKey      :[" + SearchKey       + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
										
				int rc = 0;
				
					sbQuery.Append(""
						+ "UPDATE ContractItem                     \n"
						+ "   SET FileState      = '12'			   \n"					
						+ "      ,FileName       = '" + FileName + "'   \n"					
						+ "      ,ChekckDt       = GETDATE()			\n"					
						+ "      ,ChekckID       = 'CMS'				\n"					
						+ " WHERE ItemNo         = " + itemNo	+ "		\n"					
						);				
				
				// ��������
				try
				{
					_db.BeginTran();					
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
					_db.CommitTran();	

					if(rc!=1)
					{
						sbReturn.Append("1");		
					}
					else
					{
						sbReturn.Append("0");		
					}					
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}					

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("Return---------\n");
				_log.Debug(sbReturn.ToString());
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetFileCheckReady() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				sbReturn.Append("����������Ʈ �˼���� ������ ������ �߻��Ͽ����ϴ�.(CMS)");
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}		

			return sbReturn.ToString();
		}

		/// <summary>
		/// ����������Ʈ CDN����� ������Ʈ for CMS
		/// </summary>
		/// <param name="SearchKey"></param>
		public string SetFileCDNSync(int itemNo, string SuccessYN, string contents_state)
		{

			StringBuilder sbReturn = new StringBuilder();

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetFileCDNSync() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
												
				int rc = 0;
				if(SuccessYN.Equals("Y"))
				{
					sbQuery.Append(""
						+ "UPDATE ContractItem                     \n"
						+ "   SET FileState      = '20'				  \n"	
						+ "      ,CDNSyncDt       = GETDATE()	      \n"					
						+ "      ,CDNSyncID       = 'CMS'		      \n"											
						+ " WHERE ItemNo         = " + itemNo + "     \n"					
						);
				}
				else
				{
					sbQuery.Append(""
						+ "UPDATE ContractItem                     \n"
						+ "   SET FileState      = '16'      \n"										
						+ " WHERE ItemNo         = " + itemNo     + "   \n"					
						);
				}

				// ��������
				try
				{
					_db.BeginTran();					
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
					_db.CommitTran();
	
					if(rc!=1)
					{
						sbReturn.Append("0");
					}
					else
					{
						sbReturn.Append("1");
					}
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}					

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("Return---------\n");
				_log.Debug(sbReturn.ToString());
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetFileCDNSync() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				sbReturn.Append("����������Ʈ CDN���� ������ ������ �߻��Ͽ����ϴ�.(CMS)");
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}		

			return sbReturn.ToString();
		}
	}

}