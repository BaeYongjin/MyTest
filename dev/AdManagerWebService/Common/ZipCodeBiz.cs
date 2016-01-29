using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Common
{
	/// <summary>
	/// CodeService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ZipCodeBiz : BaseBiz
	{
		public ZipCodeBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

		public void GetAddressList(HeaderModel header, ZipCodeModel data)
		{
			try
			{
				// ����Ʈ���̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append("\n select top 500 ZipCode ");
				sbQuery.Append("\n 			,Sido ");
				sbQuery.Append("\n 			,Gugun ");
				sbQuery.Append("\n 			,Dong ");
				sbQuery.Append("\n 			,Bunji ");
				sbQuery.Append("\n 			,Sido + ' ' + gugun + ' ' + Dong + ' ' +isnull(Bunji,'') as AddrFull ");
				sbQuery.Append("\n from	SystemZip noLock ");
				
				// �ڵ�з��� ����������
				if(data.SearchKey.Length > 0 )
				{
					sbQuery.Append("  WHERE Dong like '" + data.SearchKey + "' \n");
				}						

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				data.DsAddr= ds.Copy();
				data.ResultCnt	= Utility.GetDatasetCount(data.DsAddr);
				data.ResultCD	= "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSectionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}
	

		/// <summary>
		/// �����ȣ �� 3�ڸ��θ� ��� �˻�
		/// </summary>		
		public void GetPreZipList(HeaderModel header, ZipCodeModel data)
		{
			try
			{
				// ����Ʈ���̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append("\n select    ZipCode ");
				sbQuery.Append("\n 			,Sido ");
				sbQuery.Append("\n 			,Gugun ");
				sbQuery.Append("\n 			,Dong ");
				sbQuery.Append("\n 			,Bunji ");
				sbQuery.Append("\n 			,Sido + ' ' + gugun + ' ' + Dong + ' ' +isnull(Bunji,'') as AddrFull ");
				sbQuery.Append("\n from	SystemZip noLock ");
				
				// �ڵ�з��� ����������
				if(data.SearchZip.Length > 0 )
				{
					sbQuery.Append("  Where Substring(ZipCode, 1,3) In('" + data.SearchZip + "') \n");
				}

				// �� �˻�
				if (data.SearchKey != null && data.SearchKey.Length > 0)
				{
					sbQuery.Append(" Union all ");

					sbQuery.Append("\n select top 500 ZipCode ");
					sbQuery.Append("\n 			,Sido ");
					sbQuery.Append("\n 			,Gugun ");
					sbQuery.Append("\n 			,Dong ");
					sbQuery.Append("\n 			,Bunji ");
					sbQuery.Append("\n 			,Sido + ' ' + gugun + ' ' + Dong + ' ' +isnull(Bunji,'') as AddrFull ");
					sbQuery.Append("\n from	SystemZip noLock ");
				
					// �ڵ�з��� ����������
					if(data.SearchKey.Length > 0 )
					{
						sbQuery.Append("  WHERE Dong like '" + data.SearchKey + "' \n");
					}
				}



				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				data.DsAddr= ds.Copy();
				data.ResultCnt	= Utility.GetDatasetCount(data.DsAddr);
				data.ResultCD	= "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSectionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}
	

		/// <summary>
		/// Ÿ���� �� �����ȣ�� ��������
		/// </summary>		
		public void GetIncludeZipList(HeaderModel header, ZipCodeModel data)
		{
			try
			{
				// ����Ʈ���̽��� OPEN�Ѵ�
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append("\n Select    ZipCode ");
				sbQuery.Append("\n 			,Sido    ");
				sbQuery.Append("\n 			,Gugun   ");
				sbQuery.Append("\n 			,Dong    ");
				sbQuery.Append("\n 			,Bunji   ");
				sbQuery.Append("\n 			,Sido + ' ' + gugun + ' ' + Dong + ' ' +isnull(Bunji,'') as AddrFull ");
				sbQuery.Append("\n From	SystemZip noLock ");
				
				// �ڵ�з��� ����������
				if(data.SearchZip.Length > 0 )
					sbQuery.Append("  Where ZipCode In(" + data.SearchZip + ") \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				data.DsAddr= ds.Copy();
				data.ResultCnt	= Utility.GetDatasetCount(data.DsAddr);
				data.ResultCD	= "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSectionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

	}
}
