// ===============================================================================
// ���������� �������� ���Ǵ� �����Ͻ� ������
// ===============================================================================
// 2009.09.13 YS.Jang	S1������Ʈ�����ϸ鼭 ������
// ===============================================================================
// Copyright (C) 2009 DARTmedia Inc.
// All rights reserved.
// ===============================================================================
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Schedule
{
	public class SchCommBiz : BaseBiz
	{
		#region ������
		public SchCommBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region ������λ����� ���ι�ȣ�� ����
		/// <summary>
		/// ���� ���ι�ȣ�� �����´�
		/// ���� ���ι�ȣ�� ���°� 30:���������̸� �űԻ��·� ä���� AckNo�� �����Ѵ�
		/// </summary>
		/// <param name="MediaCode"></param>
		/// <param name="AdSchType">�������� Ȩ/���/��ü�� ���е�</param>
		/// <returns>������ ���ι�ȣ</returns>
		internal	int	GetLastAckNo(int MediaCode, int AdSchType)
		{
			int				AckNo		= 0;
			string			AckState	= "";
			StringBuilder	sbQuery		= new StringBuilder();				
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLastAckNo() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("<�Է�����>");
				_log.Debug("MediaCode	    :[" + MediaCode     + "]");		// �˻� ��ü
				_log.Debug("AdSchType		:[" + AdSchType     + "]");
			
				
				// ��������
				sbQuery.Append("\n"
					+ " DECLARE @AckNo int, @AckState Char(2), @MediaCode int, @AdSchType int    \n"
					+ "                                                          \n"
					+ " SELECT @MediaCode = " + MediaCode                    + " \n"
					+ " SELECT @AdSchType = " + AdSchType                    + " \n"
					+ "                                                          \n"
					+ " SELECT TOP 1 @AckState = State, @AckNo = AckNo           \n"
					+ "   FROM SchPublish with(noLock)                           \n"
					+ "  WHERE MediaCode = @MediaCode  AND	AdSchType=@AdSchType \n"
					+ "  ORDER BY AckNo DESC                                     \n"
					+ "                                                          \n"
					+ " IF @AckState = '30' OR @AckState IS NULL                 \n"
					+ " BEGIN                                                    \n"
					+ "	    INSERT INTO SchPublish                               \n"
					+ "                (AckNo, MediaCode, State, ModifyStartDay,AdSchType) \n"
					+ "         SELECT ISNULL(MAX(AckNo),0) + 1                  \n"
					+ "               ,@MediaCode                                \n"
					+ "               ,'10'                                      \n"
					+ "               ,GETDATE(), @AdSchType                     \n"
					+ "          FROM SchPublish with(noLock)                    \n"
					+ "         WHERE MediaCode = @MediaCode                     \n"
					+ "                                                          \n"
					+ "     SELECT TOP 1 @AckState = State, @AckNo = AckNo       \n"
					+ "       FROM SchPublish with(noLock)                       \n"
					+ "      WHERE MediaCode=@MediaCode AND	AdSchType=@AdSchType \n"
					+ "      ORDER BY AckNo DESC                                 \n"
					+ " END                                                      \n"
					+ "                                                          \n"
					+ " SELECT @AckNo AS AckNo, @AckState AS AckState            \n"                             
					);
	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				_db.Open();
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					AckNo    =  Convert.ToInt32( ds.Tables[0].Rows[0]["AckNo"].ToString() );
					AckState =  ds.Tables[0].Rows[0]["AckState"].ToString();
				}

				ds.Dispose();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetLastAckNo() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}
			finally
			{
				_db.Close();
			}
			return AckNo;
		}
		#endregion

		#region ������¸� �����·� ����
		/// <summary>
		/// ������¸� �����·� �����Ѵ�.
		/// �� �߰�,����,������ ������°� �����°� �ƴѰ�� �����·� �����Ѵ�
		/// </summary>
		/// <param name="itemNo">�۾���� ����</param>
		/// <param name="userId">�����۾���</param>
		internal	void SetItemActive( int itemNo, string userId )
		{
			StringBuilder	sbQuery		= new StringBuilder();				
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetItemActive() Start");
				_log.Debug("ItemNo	    :[" + itemNo     + "]");		// �˻� ��ü
				_log.Debug("UserId		:[" + userId     + "]");
			
				
				// ��������
				sbQuery.Append( "\n"
					+ " UPDATE	ContractItem        \n"
					+ "    SET  AdState = '20'      \n"   // ������� - 20:��
					+ "        ,ModDt   = GETDATE() \n"
					+ "        ,RegID   = '" + userId + "' \n" 
					+ " WHERE ItemNo  = " + itemNo + " \n");

				_log.Debug(sbQuery.ToString());

				int rc = _db.ExecuteNonQuery( sbQuery.ToString() );

				if( rc < 1 )	throw new Exception("������Ʈ ��� �������� �����ϴ�!!!");

				_log.Debug(this.ToString() + "SetItemActive() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}
		}
		#endregion
	}
}