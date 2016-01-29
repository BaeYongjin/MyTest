// ===============================================================================
// 편성업무에서 공동으로 사용되는 비지니스 로직들
// ===============================================================================
// 2009.09.13 YS.Jang	S1프로젝트진행하면서 생성함
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
		#region 생성자
		public SchCommBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region 현재승인상태의 승인번호를 구함
		/// <summary>
		/// 현재 승인번호를 가져온다
		/// 현재 승인번호의 상태가 30:배포승인이면 신규상태로 채번후 AckNo를 리턴한다
		/// </summary>
		/// <param name="MediaCode"></param>
		/// <param name="AdSchType">편성승인이 홈/상업/매체로 구분됨</param>
		/// <returns>편성중인 승인번호</returns>
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
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode	    :[" + MediaCode     + "]");		// 검색 매체
				_log.Debug("AdSchType		:[" + AdSchType     + "]");
			
				
				// 쿼리생성
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
				
				// 쿼리실행
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

		#region 광고상태를 편성상태로 변경
		/// <summary>
		/// 광고상태를 편성상태로 변경한다.
		/// 편성 추가,삭제,수정시 광고상태가 편성상태가 아닌경우 편성상태로 변경한다
		/// </summary>
		/// <param name="itemNo">작업대상 광고</param>
		/// <param name="userId">변경작업자</param>
		internal	void SetItemActive( int itemNo, string userId )
		{
			StringBuilder	sbQuery		= new StringBuilder();				
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetItemActive() Start");
				_log.Debug("ItemNo	    :[" + itemNo     + "]");		// 검색 매체
				_log.Debug("UserId		:[" + userId     + "]");
			
				
				// 쿼리생성
				sbQuery.Append( "\n"
					+ " UPDATE	ContractItem        \n"
					+ "    SET  AdState = '20'      \n"   // 광고상태 - 20:편성
					+ "        ,ModDt   = GETDATE() \n"
					+ "        ,RegID   = '" + userId + "' \n" 
					+ " WHERE ItemNo  = " + itemNo + " \n");

				_log.Debug(sbQuery.ToString());

				int rc = _db.ExecuteNonQuery( sbQuery.ToString() );

				if( rc < 1 )	throw new Exception("업데이트 대상 광고내역이 없습니다!!!");

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