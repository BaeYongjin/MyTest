using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Common
{
	/// <summary>
	/// MediaCodeService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MediaCodeBiz : BaseBiz
	{
		public MediaCodeBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  �ڵ�����ȸ
		/// </summary>
		/// <param name="mediacodeModel"></param>
		public void GetMediaCodeList(HeaderModel header, MediaCodeModel mediacodeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaCodeList() Start");
				_log.Debug("-----------------------------------------");

				// ����Ʈ���̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("MediaCode :[" + mediacodeModel.MediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ " SELECT mda_cod as MediaCode, mda_nm as MediaName  \n"
					+ "   FROM MDA                 \n"					
					+ "   WHERE use_yn = 'Y'         \n"					
					);

				// �ڵ�з��� ����������
				if (mediacodeModel.SearchKey.Trim().Length > 0)
				{
					sbQuery.Append("  AND mda_name    LIKE '%" + mediacodeModel.SearchKey.Trim() + "%' \n");
				}				
				

				sbQuery.Append(" ORDER BY mda_cod \n");
				
				_log.Debug(sbQuery.ToString());			
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				mediacodeModel.MediaCodeDataSet = ds.Copy();
				// ���
				mediacodeModel.ResultCnt = Utility.GetDatasetCount(mediacodeModel.MediaCodeDataSet);
				// ����ڵ� ��Ʈ
				mediacodeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + mediacodeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				mediacodeModel.ResultCD = "3000";
				mediacodeModel.ResultDesc = "�ڵ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
				
				// ����Ʈ���̽���  Close�Ѵ�
				_db.Close();

			}

			// ����Ʈ���̽���  Close�Ѵ�
			_db.Close();

		}
	}
}
