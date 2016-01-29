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
	/// AgencyCodeService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class CampaignCodeBiz : BaseBiz
	{
		public CampaignCodeBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  �ڵ�����ȸ
		/// </summary>
		/// <param name="campaigncodeModel"></param>
		public void GetCampaignCodeList(HeaderModel header, CampaignCodeModel campaigncodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignCodeList() Start");
				_log.Debug("-----------------------------------------");

				// ����Ʈ���̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("Section :[" + campaigncodeModel.ContractSeq + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT CampaignCode, CampaignName  \n"
					+ "   FROM CampaignMaster with(NoLock)              \n"					
					+ "   WHERE MediaCode = '"+campaigncodeModel.MediaCode+"'         \n"					
					);

				// �ڵ�з��� ����������				
				if(!campaigncodeModel.ContractSeq.Equals("00"))
				{
					sbQuery.Append("  AND(  ContractSeq = '"+campaigncodeModel.ContractSeq+"') \n");
				}    

				sbQuery.Append(" ORDER BY CampaignCode \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				campaigncodeModel.CampaignCodeDataSet = ds.Copy();
				// ���
				campaigncodeModel.ResultCnt = Utility.GetDatasetCount(campaigncodeModel.CampaignCodeDataSet);
				// ����ڵ� ��Ʈ
				campaigncodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + campaigncodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCampaignCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				campaigncodeModel.ResultCD = "3000";
				campaigncodeModel.ResultDesc = "ķ���� �ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);

				// ����Ʈ���̽���  Close�Ѵ�
				_db.Close();
			}
			// ����Ʈ���̽���  Close�Ѵ�
			_db.Close();
		}
	}
}
