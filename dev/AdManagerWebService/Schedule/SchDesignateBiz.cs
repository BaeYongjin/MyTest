/*
 * -------------------------------------------------------
 * Class Name: SchDesignateBiz
 * 주요기능  : 채널별 지정편성 서비스
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.05.27
 * 수정내용  :        
 *            - 광고종류별 조회 쿼리문 수정
 * 수정함수  :
 *            - GetList()
 *            - GetItemList()
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

namespace AdManagerWebService.Schedule
{
	/// <summary>
	/// 특정지정 편성 비지니즈로직
	/// </summary>
	public class SchDesignateBiz : BaseBiz
    {

        public SchDesignateBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        #region [1] 지정 편성리스트 가져오기
        public void GetList(HeaderModel header, SchDesignateModel	model)
        {
            try
            {
                _db.Open();
                StringBuilder sbQuery = new StringBuilder();
					
                // 쿼리생성
				sbQuery.Append(" select a.ItemNo " + "\n");
				sbQuery.Append("		,a.Category	, dbo.ufnGetCategoryName( a.MediaCode, a.Category)	as CategoryNm " + "\n");
				sbQuery.Append(" 		,a.Genre	, dbo.ufnGetGenreName( a.MediaCode, a.Genre)		as GenreNm " + "\n");
				sbQuery.Append(" 		,a.Channel	, dbo.ufnGetChannelName( a.MediaCode, a.Channel)	as ChannelNm " + "\n");
				sbQuery.Append(" 		,a.Series			as	Series " + "\n");
				sbQuery.Append(" 		,b.ItemName			as	ItemNm " + "\n");
				sbQuery.Append(" 		,b.ExcuteStartDay	as	StartDay " + "\n");
				sbQuery.Append(" 		,b.RealEndDay		as	EndDay " + "\n");
				sbQuery.Append(" 		, case  " + "\n");
				sbQuery.Append(" 				when Category > 0 and Genre = 0 and Channel = 0 and Series = 0	then 0 " + "\n");
				sbQuery.Append(" 				when Category > 0 and Genre > 0 and Channel = 0 and Series = 0	then 1 " + "\n");
				sbQuery.Append(" 				when Category > 0 and Genre > 0 and Channel > 0 and Series = 0	then 2 " + "\n");
				sbQuery.Append(" 				when Category > 0 and Genre > 0 and Channel > 0 and Series > 0	then 3 " + "\n");
				sbQuery.Append(" 				else	9 " + "\n");
				sbQuery.Append(" 		  end	as SchType " + "\n");
				sbQuery.Append(" 		,b.AdState " + "\n");
				sbQuery.Append(" 		,c.State	as  SchState " + "\n");
				sbQuery.Append(" 		,b.FileState " + "\n");
                sbQuery.Append(" 		,b.AdType " + "\n"); // [E_01]
                sbQuery.Append(" 		,d.CodeName as AdTypeName " + "\n"); // [E_01]
				sbQuery.Append(" from	SchDesignatedDetail	a with(noLock) " + "\n");
				sbQuery.Append(" inner join ContractItem	b with(noLock) on b.ItemNo = a.ItemNo " + "\n");
				sbQuery.Append(" inner join SchPublish		c with(noLock) on c.MediaCode = a.MediaCode and c.AckNo = a.AckNo and c.AdSchType = 10 " + "\n");
                sbQuery.Append(" left outer join SystemCode d with(noLock) on b.AdType = d.Code and d.Section = 26 " + "\n"); // [E_01]
				sbQuery.Append(" where	b.MediaCode	= " + model.Media + "\n");
				if( model.MediaRep > 0 )	sbQuery.Append(" and b.RapCode = " + model.MediaRep + "\n");

				#region [ 광고상태에 따른 조회조건 ]
				if( model.AdState10 )
				{
					if( model.AdState20 )
					{
						if( model.AdState30 )
						{
							if( model.AdState40 )	sbQuery.Append(" and b.AdState in(10,20,30,40) \n");
							else					sbQuery.Append(" and b.AdState in(10,20,30) \n");
						}
						else
						{
							if( model.AdState40 )	sbQuery.Append(" and b.AdState in(10,20,40) \n");
							else					sbQuery.Append(" and b.AdState in(10,20) \n");
						}
					}
					else
					{
						if( model.AdState30 )
						{
							if( model.AdState40 )	sbQuery.Append(" and b.AdState in(10,30,40) \n");
							else					sbQuery.Append(" and b.AdState in(10,30) \n");
						}
						else
						{
							if( model.AdState40 )	sbQuery.Append(" and b.AdState in(10,40) \n");
							else					sbQuery.Append(" and b.AdState in(10) \n");
						}
					}
				}
				else
				{
					if( model.AdState20 )
					{
						if( model.AdState30 )
						{
							if( model.AdState40 )	sbQuery.Append(" and b.AdState in(20,30,40) \n");
							else					sbQuery.Append(" and b.AdState in(20,30) \n");
						}
						else
						{
							if( model.AdState40 )	sbQuery.Append(" and b.AdState in(20,40) \n");
							else					sbQuery.Append(" and b.AdState in(20) \n");
						}
					}
					else
					{
						if( model.AdState30 )
						{
							if( model.AdState40 )	sbQuery.Append(" and b.AdState in(30,40) \n");
							else					sbQuery.Append(" and b.AdState in(30) \n");
						}
						else
						{
							if( model.AdState40 )	sbQuery.Append(" and b.AdState in(40) \n");
							else					sbQuery.Append(" and b.AdState in(10,20,30,40) \n");
						}
					}
				}
				#endregion

                sbQuery.Append(" order by a.ItemNo, a.Category, a.Genre, a.Channel, a.Series  " + "\n");
			
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 모델에 복사
				model.DsSchedule	=	ds.Copy();
				model.ResultCnt		=	Utility.GetDatasetCount( model.DsSchedule );
				model.ResultCD		=	"0000";
				model.ResultDesc	=	"조회완료";	
            }
            catch(Exception ex)
            {
				model.ResultCD	=	"3000";
				model.ResultCnt	=	0;
				model.ResultDesc=	"카테고리/장르정보 조회중 오류발생(CSS용)";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        #endregion

		#region [2] 지정 편성리스트 추가
		public void InsertData(HeaderModel header, SchDesignateModel	model)
		{
			try
			{
				_db.Open();
				
				int ackNo = new SchCommBiz().GetLastAckNo(model.Media, 10 );
				if ( ackNo == 0 )
				{
					throw new Exception("편성승인번호가 잘못되었습니다!!!");
				}

				SqlParameter[] sqlParams = new SqlParameter[7];
				sqlParams[0] = new SqlParameter("@Media"	, SqlDbType.Int	);
				sqlParams[1] = new SqlParameter("@Category" , SqlDbType.Int );
				sqlParams[2] = new SqlParameter("@Genre"    , SqlDbType.Int );
				sqlParams[3] = new SqlParameter("@Channel"  , SqlDbType.Int );
				sqlParams[4] = new SqlParameter("@Series"	, SqlDbType.Int );
				sqlParams[5] = new SqlParameter("@Item"		, SqlDbType.Int );
				sqlParams[6] = new SqlParameter("@AckNo"	, SqlDbType.Int );

				sqlParams[0].Value = model.Media;
				sqlParams[1].Value = model.Category;
				sqlParams[2].Value = model.Genre;
				sqlParams[3].Value = model.Channel;
				sqlParams[4].Value = model.Series;
				sqlParams[5].Value = model.ItemNo;
				sqlParams[6].Value = ackNo;

				StringBuilder sbQuery = new StringBuilder();
				sbQuery.Append("\n INSERT INTO SchDesignatedDetail");
				sbQuery.Append("\n            ([ItemNo]");
				sbQuery.Append("\n            ,[MediaCode]");
				sbQuery.Append("\n            ,[Category]");
				sbQuery.Append("\n            ,[Genre]");
				sbQuery.Append("\n            ,[Channel]");
				sbQuery.Append("\n            ,[Series]");
				sbQuery.Append("\n            ,[AckNo])");
				sbQuery.Append("\n      VALUES(@Item ");
				sbQuery.Append("\n            ,@Media");
				sbQuery.Append("\n            ,@Category");
				sbQuery.Append("\n            ,@Genre");
				sbQuery.Append("\n            ,@Channel");
				sbQuery.Append("\n            ,@Series");
				sbQuery.Append("\n            ,@AckNo )");
			
				// 쿼리실행
				int rc = _db.ExecuteNonQueryParams( sbQuery.ToString(), sqlParams );
				if( rc > 0 )
				{
					model.ResultCnt		=	rc;
					model.ResultCD		=	"0000";
					model.ResultDesc	=	"추가완료";	
				}
			}
			catch(Exception ex)
			{
				model.ResultCD	=	"3000";
				model.ResultCnt	=	0;
				model.ResultDesc=	"지정광고편성 추가중 오류발생";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

		#endregion

		#region [3] 지정 편성리스트 삭제
		public void DeleteData(HeaderModel header, SchDesignateModel	model)
		{
			try
			{
				_db.Open();
				
				SqlParameter[] sqlParams = new SqlParameter[6];
		
				sqlParams[0] = new SqlParameter("@Media"   , SqlDbType.Int	);
				sqlParams[1] = new SqlParameter("@Category" , SqlDbType.Int );
				sqlParams[2] = new SqlParameter("@Genre"    , SqlDbType.Int );
				sqlParams[3] = new SqlParameter("@Channel"  , SqlDbType.Int );
				sqlParams[4] = new SqlParameter("@Series"	, SqlDbType.Int );
				sqlParams[5] = new SqlParameter("@Item"		, SqlDbType.Int );

				sqlParams[0].Value = model.Media;
				sqlParams[1].Value = model.Category;
				sqlParams[2].Value = model.Genre;
				sqlParams[3].Value = model.Channel;
				sqlParams[4].Value = model.Series;
				sqlParams[5].Value = model.ItemNo;



				StringBuilder sbQuery = new StringBuilder();
				sbQuery.Append(" delete	from SchDesignatedDetail " + "\n");
				sbQuery.Append(" where	ItemNo		= @Item" + "\n");
				sbQuery.Append(" and	MediaCode	= @Media" + "\n");
				sbQuery.Append(" and	Category	= @Category" + "\n");
				sbQuery.Append(" and	Genre		= @Genre" + "\n");
				sbQuery.Append(" and	Channel		= @Channel" + "\n");
				sbQuery.Append(" and	Series		= @Series" + "\n");

			
				// 쿼리실행
				int rc = _db.ExecuteNonQueryParams( sbQuery.ToString(), sqlParams );
				if( rc > 0 )
				{
					model.ResultCnt		=	rc;
					model.ResultCD		=	"0000";
					model.ResultDesc	=	"삭제완료";	
				}
			}
			catch(Exception ex)
			{
				model.ResultCD	=	"3000";
				model.ResultCnt	=	0;
				model.ResultDesc=	ex.Message;
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

		#endregion

		#region [4] 추가대상 광고리스트 조회
		/// <summary>
		/// 추가대상 광고목록 가져오기
		/// </summary>
		/// <param name="header"></param>
		/// <param name="model"></param>
		public void GetItemList(HeaderModel header, SchDesignateModel	model)
		{
			try
			{
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();
					
				// 쿼리생성
				sbQuery.Append(" select  a.ItemNo                            " + "\n");
				sbQuery.Append(" 		,a.ItemName                          " + "\n");
				sbQuery.Append(" 		,a.ExcuteStartDay	as	BeginDay     " + "\n");
				sbQuery.Append(" 		,a.RealEndDay		as	EndDay       " + "\n");
				sbQuery.Append(" 		,a.AdState                           " + "\n");
				sbQuery.Append(" 		,a.FileState                         " + "\n");
				sbQuery.Append(" 		,x.CodeName			as	AdStateName  " + "\n");
				sbQuery.Append(" 		,y.CodeName			as	FileStatName " + "\n");
                sbQuery.Append(" 		,a.AdType                            " + "\n"); // [E_01]
                sbQuery.Append(" 		,z.CodeName         as  AdTypeName   " + "\n"); // [E_01]
				sbQuery.Append(" from	ContractItem a with(noLock)          " + "\n");
				sbQuery.Append(" left outer join SystemCode x with(noLock) on a.AdState = x.Code and x.Section = 25  " + "\n");
				sbQuery.Append(" left outer join SystemCode y with(noLock) on a.FileState= y.Code and y.Section = 31 " + "\n");
                sbQuery.Append(" left outer join SystemCode z with(noLock) on a.AdType = z.Code and z.Section = 26   " + "\n"); // [E_01]
				sbQuery.Append(" where	a.MediaCode	= " + model.Media +"\n");
				if( model.MediaRep > 0 )	sbQuery.Append(" and a.RapCode = " + model.MediaRep + "\n");

				#region [ 광고상태에 따른 조회조건 ]
				if( model.AdState10 )
				{
					if( model.AdState20 )
					{
						if( model.AdState30 )
						{
							if( model.AdState40 )	sbQuery.Append(" and a.AdState in(10,20,30,40) \n");
							else					sbQuery.Append(" and a.AdState in(10,20,30) \n");
						}
						else
						{
							if( model.AdState40 )	sbQuery.Append(" and a.AdState in(10,20,40) \n");
							else					sbQuery.Append(" and a.AdState in(10,20) \n");
						}
					}
					else
					{
						if( model.AdState30 )
						{
							if( model.AdState40 )	sbQuery.Append(" and a.AdState in(10,30,40) \n");
							else					sbQuery.Append(" and a.AdState in(10,30) \n");
						}
						else
						{
							if( model.AdState40 )	sbQuery.Append(" and a.AdState in(10,40) \n");
							else					sbQuery.Append(" and a.AdState in(10) \n");
						}
					}
				}
				else
				{
					if( model.AdState20 )
					{
						if( model.AdState30 )
						{
							if( model.AdState40 )	sbQuery.Append(" and a.AdState in(20,30,40) \n");
							else					sbQuery.Append(" and a.AdState in(20,30) \n");
						}
						else
						{
							if( model.AdState40 )	sbQuery.Append(" and a.AdState in(20,40) \n");
							else					sbQuery.Append(" and a.AdState in(20) \n");
						}
					}
					else
					{
						if( model.AdState30 )
						{
							if( model.AdState40 )	sbQuery.Append(" and a.AdState in(30,40) \n");
							else					sbQuery.Append(" and a.AdState in(30) \n");
						}
						else
						{
							if( model.AdState40 )	sbQuery.Append(" and a.AdState in(40) \n");
							else					sbQuery.Append(" and a.AdState in(10,20,30,40) \n");
						}
					}
				}
				#endregion

				sbQuery.Append(" order by ItemNo desc;  " + "\n");
			
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				model.DsItem	=	ds.Copy();
				model.ResultCnt	=	Utility.GetDatasetCount( model.DsItem );
				model.ResultCD	=	"0000";
				model.ResultDesc=	"조회완료";	
			}
			catch(Exception ex)
			{
				model.ResultCD	=	"3000";
				model.ResultCnt	=	0;
				model.ResultDesc=	"지정편성대상 광고목록 조회중 오류발생";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

		#endregion

    }
}