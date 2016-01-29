/*
 * -------------------------------------------------------
 * 클래스 명 : TargetCollectionBiz
 * 주요기능  : 타겟 고객군 관리 로직
 * 작성자    : 모름
 * 작성일    : 모름
 * 특이사항  : 
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : bae
 * 수정일    : 2013.05.23
 * 수정내용  :        
                - 건당 처리하던 고객군이 사용자 등록,삭제를 
 *                전체 데이터를 받아서 처리 하는 로직으로 변경
 *              - 전체데이터를 구간별로 나누고 구가별 처리 할 데이터 양을 10개로
 *                 지정해서 한 구간 처리 후 다음구간 처리 할 시 sleep 설정해 놓음.
 *                 테스트 후 변경해야 할 수 도 있음.
 * 수정함수  : 
 *            [add] 
 *              using System.Threading
 *              SetClientCollectionCreate(..) 고객군에 등록 할 사용자들 등록
 *              SetClientCollectionDelete(..) 고객군의 선택 된 사용자들 삭제
 *            [edit]
 *            
 * --------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : bae
 * 수정일    : 2013.06.10
 * 수정부분  : 
 *            [add]
 *              SetClientCollectionCreateProc(..) 고객군 등록을 프로시져 이용한 함수 추가
 *              SetClientCollectionCreate(..) 함수 대신에 프로시져 이용한 함수 호출로 수정 처리 함.
 *            
 * 수정내용  : 
 * --------------------------------------------------------
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;
using System.Threading;

using AdManagerModel;


namespace AdManagerWebService.Target
{
	/// <summary>
	/// TargetCollectionBiz에 대한 요약 설명입니다.
	/// </summary>
	public class TargetCollectionBiz : BaseBiz
	{
		public TargetCollectionBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		/// 타겟군목록조회
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetTargetCollectionList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTargetCollectionList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey       + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
                // 타겟 고객군 정보 가져오기 //
                // 고객군코드, 고객군명, 커멘트, 사용유무
                // 등록일, 수정일
                // 고객군별 사용자 수
                // 고객군별 타겟팅된 광고 수
                // 개인화 정보 
				sbQuery.Append("\n SELECT A.CollectionCode, A.CollectionName, A.Comment ");
				sbQuery.Append("\n        ,A.UseYn ");
				sbQuery.Append("\n        ,CASE A.UseYn WHEN 'Y' THEN '사용' WHEN 'N' THEN '사용안함' END AS UseYn_N ");
				sbQuery.Append("\n        ,convert(char(19), A.RegDt, 120) RegDt ");
				sbQuery.Append("\n        ,convert(char(19), A.ModDt, 120) ModDt ");
				sbQuery.Append("\n        ,B.UserName RegName ");
				sbQuery.Append("\n        ,( Select count(*) from ClientList with(noLock) where CollectionCode = A.CollectionCode AND CollectionCode = A.CollectionCode) Cnt ");
				sbQuery.Append("\n 		  ,( select count(*)  ");
				sbQuery.Append("\n 			 from	Targeting x with(noLock) ");
				sbQuery.Append("\n 			 inner join ContractItem y with(noLock) on x.ItemNo = y.ItemNo and y.AdState <> '40' ");
				sbQuery.Append("\n 			 where	x.TgtCollectionYn = 'Y'  ");
				sbQuery.Append("\n 			 and		x.TgtCollection = a.CollectionCode ) as UseCnt ");
				sbQuery.Append("\n 		  , case a.PvsYn when 'Y' then 'True' else 'False' end  as PvsYn");
				sbQuery.Append("\n 		  , case a.PvsYn when 'Y' then isnull(PVSSeq,0) else 0 end  as PvsSeq");
				sbQuery.Append("\n FROM	Collection			A with(noLock) ");
				sbQuery.Append("\n LEFT JOIN SystemUser	B with(NoLock) ON (A.RegId          = B.UserId) ");
				sbQuery.Append("\n WHERE 1 = 1  ");			
				// 검색어가 있으면
				if (targetcollectionModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
						+ "  A.CollectionName      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"	
						+ " OR A.Comment    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
						+ " OR B.UserName    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}
				
				if(targetcollectionModel.SearchchkAdState_10.Trim().Length > 0 && targetcollectionModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND A.UseYn = 'Y' OR A.UseYn = 'N' \n");
				}	
				if(targetcollectionModel.SearchchkAdState_10.Trim().Length > 0 && targetcollectionModel.SearchchkAdState_10.Trim().Equals("N"))
				{
					sbQuery.Append(" AND  A.UseYn  = 'Y' \n");					
				}	

				sbQuery.Append(" ORDER BY A.CollectionCode desc \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 미디어렙모델에 복사
				targetcollectionModel.TargetCollectionDataSet = ds.Copy();
				// 결과
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.TargetCollectionDataSet);
				// 결과코드 셋트
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTargetCollectionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "타겟고객군정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// PVS그룹목록조회
		/// </summary>
		/// <param name="header"></param>
		/// <param name="targetcollectionModel"></param>
		public void GetPvsGroupList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPvsGroupList() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append("\n select Seq_No		as SeqNo ");
				sbQuery.Append("\n 		 ,Group_Name	as GroupName ");
				sbQuery.Append("\n 		 ,( select count(*) from PVDB.dbo.Favorite_group_stb_list with(noLock) where seq_no = fgm.seq_no ) as stbCnt ");
				sbQuery.Append("\n from	PVDB.dbo.Favorite_group_master fgm with(noLock) ");
				sbQuery.Append("\n where	fgm.Used_Yn = 'Y' ");
				sbQuery.Append("\n order by Seq_No desc ");
				_log.Debug(sbQuery.ToString());
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				targetcollectionModel.TargetCollectionDataSet = ds.Copy();
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.TargetCollectionDataSet);
				targetcollectionModel.ResultCD = "0000";

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPvsGroupList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "GetPvsGroupList 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}


		/// <summary>
		/// PVS상세내역 동기화
		/// </summary>
		/// <param name="header"></param>
		/// <param name="targetcollectionModel"></param>
		public void SetPvsSync(HeaderModel header, TargetCollectionModel data)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetPvsSync() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();
				string	pvsName = data.CollectionName + "과 동기화 시킴";

				sbQuery.Append("\n declare @i_CollCode	int;	");
				sbQuery.Append("\n declare @i_PvsSeq	int;	");
				sbQuery.Append("\n declare @i_PvsName	varchar(50);	");

				sbQuery.Append("\n set @i_CollCode	= " + data.CollectionCode );
				sbQuery.Append("\n set @i_PvsSeq	= " + data.SeqNo.ToString() );
				sbQuery.Append("\n set @i_PvsName	= '" + pvsName + "'");

				sbQuery.Append("\n -- 1. 기존 AdTargets의 정보를 삭제한다	");
				sbQuery.Append("\n delete from ClientList where CollectionCode = @i_CollCode;	");

				sbQuery.Append("\n -- 2. PVS에서 정보를 가져와서..AdTargets에 입력한다	");
				sbQuery.Append("\n insert into ClientList	");
				sbQuery.Append("\n select @i_CollCode	");
				sbQuery.Append("\n		 ,stb.UserId	");
				sbQuery.Append("\n 		 ,getDate()	");
				sbQuery.Append("\n 		 ,stb.ServiceNum	");
				sbQuery.Append("\n from	StbUser stb with(noLock)	");
				sbQuery.Append("\n inner join ( select	Stb_Id	as StbId	");
				sbQuery.Append("\n 				from	pvdb.dbo.Favorite_Group_Stb_List with(noLock)	");
				sbQuery.Append("\n 				where	Seq_No = @i_PvsSeq ) pvs	");
				sbQuery.Append("\n 		 on pvs.stbId = stb.stbId	");

				sbQuery.Append("\n if @@rowcount > 0	");
				sbQuery.Append("\n 	update Collection	");
				sbQuery.Append("\n 	set	 PVSSeq = @i_PvsSeq	");
				sbQuery.Append("\n 		,Comment = @i_PvsName	");
				sbQuery.Append("\n 		,ModDt	= GetDate()	");
				sbQuery.Append("\n 	where CollectionCode = @i_CollCode;	");
				sbQuery.Append("\n 	");
				_log.Debug(sbQuery.ToString());
				
				// 쿼리실행
				DataSet ds = new DataSet();
				int rc = _db.ExecuteNonQuery( sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				data.ResultCnt = rc;
				data.ResultCD = "0000";

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetPvsSync() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "SetPvsSync 처리중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		/// <summary>
		/// 셋탑리스트를 읽어온다
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetStbList(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStbList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
                    + "Select Top 1000                  \n"
					+ "       'False' AS CheckYn        \n"
					+ "		  ,ServiceNum as    UserId  \n"     
					+ "		  ,StbId					\n"
					+ "       ,PostNo					\n"
					+ "       ,ServiceCode				\n"
					+ "		  ,ResidentNo				\n"
					+ "		  ,Sex						\n"
					+ "       ,CASE Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N  \n"
					+ "		  ,RegDt					\n"					
					+ "  FROM StbUser with(noLock)      \n"				
					+ " WHERE  1 = 1					\n"
					);
				
				// 검색어가 있으면
				if (targetcollectionModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("\n"
						+ "  AND ( StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
						+ "     OR PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
						+ "     OR ServiceNum   LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
						+ " 	)       \n"
						);
				}
       
				sbQuery.Append("  ORDER BY ServiceNum, PostNo, ResidentNo, Sex");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				targetcollectionModel.StbListDataSet = ds.Copy();
				// 결과
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.StbListDataSet);
				// 결과코드 셋트
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStbList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "Stb 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

        /// <summary>
        /// 셋탑군에서 특정셋탑을 조회한다.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="targetcollectionModel"></param>
        public void GetStbListColl(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {  // 데이터베이스를 OPEN한다
                _db.Open(); 

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbListColl() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey       + "]");
                _log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n Select Top 1000          "); // 1000 개씩 보이기
                sbQuery.Append("\n       'False' AS CheckYn ");
                sbQuery.Append("\n        ,A.CollectionCode ");
                sbQuery.Append("\n 		  ,A.UserId ");
                sbQuery.Append("\n 		  ,B.StbId ");
                sbQuery.Append("\n 		  ,B.PostNo ");
                sbQuery.Append("\n 		  ,B.ServiceCode ");
                sbQuery.Append("\n 		  ,B.ResidentNo ");
                sbQuery.Append("\n 		  ,B.Sex ");
                sbQuery.Append("\n        ,CASE B.Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N ");
                sbQuery.Append("\n 		  ,A.RegDt ");
                sbQuery.Append("\n from	ClientList      a with(nolock) ");
                sbQuery.Append("\n inner join StbUser   b with(nolock) on b.UserId = a.UserId");
                sbQuery.Append("\n where    a.CollectionCode = " + targetcollectionModel.CollectionCode );
				
                // 검색어가 있으면
                if (targetcollectionModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "     AND (   b.StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.ServiceNum  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + " 	    )  \n"
                        );
                }
       
                sbQuery.Append("  ORDER BY b.ServiceNum, b.PostNo, b.ResidentNo, b.Sex");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 광고팩키지모델에 복사
                targetcollectionModel.StbListDataSet = ds.Copy();                
                // 결과
                targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.StbListDataSet);
                              
                // 결과코드 셋트
                targetcollectionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbListColl() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                targetcollectionModel.ResultCD = "3000";
                targetcollectionModel.ResultDesc = "Stb 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }


        /// <summary>
        /// 셋탑 리스트 페이징 처리.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="targetcollectionModel"></param>
        public void GetStbPageList(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {  // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbPageList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey + "]");
                _log.Debug("JobCode      :[" + targetcollectionModel.UserId + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
          
                // 쿼리생성                    
                sbQuery.Append(" Select Top (" + targetcollectionModel.PageSize.Trim() + ") \n");
                sbQuery.Append("     'False' AS CheckYn         \n");
                sbQuery.Append("	 ,ServiceNum as    UserId   \n");
                sbQuery.Append("	 ,StbId					    \n");
                sbQuery.Append("     ,PostNo					\n");
                sbQuery.Append("     ,ServiceCode				\n");
                sbQuery.Append("	 ,ResidentNo				\n");
                sbQuery.Append("	 ,Sex						\n");
                sbQuery.Append("     ,CASE Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N  \n");
                sbQuery.Append("	 ,RegDt					    \n");
                sbQuery.Append(" FROM StbUser with(noLock)      \n");
                sbQuery.Append(" WHERE  1 = 1					\n");
                    
                if (targetcollectionModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("  AND (    StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("        OR PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("        OR ServiceNum   LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("      ) \n");
                }
    
                sbQuery.Append(" And UserId Not IN (Select Top((" + targetcollectionModel.Page.Trim() + "-1)*" + targetcollectionModel.PageSize.Trim() + ")UserId \n");
                sbQuery.Append("                    From StbUser \n");
                sbQuery.Append("                    Where 1=1    \n");

                if (targetcollectionModel.SearchKey.Trim().Length > 0) // 검색어가 있으면
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.                        
                    sbQuery.Append("  AND (    StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("        OR PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("        OR ServiceNum   LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("      )  \n");
                }                   
                sbQuery.Append("  ) ORDER BY ServiceNum, PostNo, ResidentNo, Sex");
                
                
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 광고팩키지모델에 복사
                targetcollectionModel.StbListDataSet = ds.Copy();
                // 결과
                targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.StbListDataSet);
                // 결과코드 셋트
                targetcollectionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetcollectionModel.ResultCD = "3000";
                targetcollectionModel.ResultDesc = "Stb 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }



		/// <summary>
		/// 고객리스트
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetClientList(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            #region 기존 방식 주석...
            /*
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn            \n"
					+ "       ,A.CollectionCode			\n"     
					+ "		  ,A.UserId					\n"	
					+ "		  ,B.StbId					\n"	
					+ "		  ,B.PostNo					\n"	
					+ "		  ,B.ServiceCode			\n"	
					+ "		  ,B.ResidentNo				\n"	
					+ "		  ,B.Sex					\n"	
					+ "       ,CASE B.Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N  \n"
					+ "		  ,A.RegDt					\n"										
					+ "  FROM ClientList A with(noLock) LEFT JOIN StbUser B with(NoLock) ON (A.UserId = B.UserId)        \n"					
					+ " WHERE  1 = 1					\n"
					);
				
				// 검색어가 있으면
//				if (targetcollectionModel.SearchKey.Trim().Length > 0)
//				{
//					// 여러컬럼에 대하여 LIKE 검색을 한다.
//					sbQuery.Append("\n"
//						+ "  AND ( StbId		  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR PostNo	      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR ResidentNo     LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ "     OR Sex		      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
//						+ " 	)       \n"
//						);
//				}

				// 매체대행사광고주레벨을 선택했으면
				if(targetcollectionModel.CollectionCode.Trim().Length > 0 && !targetcollectionModel.CollectionCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "' \n");
				}		
       
				sbQuery.Append("  ORDER BY A.CollectionCode, A.UserId");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				targetcollectionModel.ClientListDataSet = ds.Copy();
				// 결과
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);
				// 결과코드 셋트
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "고객 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
            */
            #endregion
            try
            {  // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey + "]");
                _log.Debug("JobCode      :[" + targetcollectionModel.UserId + "]");

                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n Select Top 1000          "); // 1000 개씩 보이기
                sbQuery.Append("\n       'False' AS CheckYn ");
                sbQuery.Append("\n        ,A.CollectionCode ");
                sbQuery.Append("\n 		  ,A.UserId ");
                sbQuery.Append("\n 		  ,B.StbId ");
                sbQuery.Append("\n 		  ,B.PostNo ");
                sbQuery.Append("\n 		  ,B.ServiceCode ");
                sbQuery.Append("\n 		  ,B.ResidentNo ");
                sbQuery.Append("\n 		  ,B.Sex ");
                sbQuery.Append("\n        ,CASE B.Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N ");
                sbQuery.Append("\n 		  ,A.RegDt ");
                sbQuery.Append("\n from	ClientList      a with(nolock) ");
                sbQuery.Append("\n inner join StbUser   b with(nolock) on b.UserId = a.UserId");
                sbQuery.Append("\n where    a.CollectionCode = " + targetcollectionModel.CollectionCode);

                // 검색어가 있으면
                if (targetcollectionModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "     AND (   b.StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + "     OR      b.ServiceNum  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
                        + " 	    )  \n"
                        );
                }

                sbQuery.Append("  ORDER BY b.ServiceNum, b.PostNo, b.ResidentNo, b.Sex");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 광고팩키지모델에 복사
                targetcollectionModel.ClientListDataSet = ds.Copy();
                // 결과
                targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);

                // 결과코드 셋트
                targetcollectionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetcollectionModel.ResultCD = "3000";
                targetcollectionModel.ResultDesc = "고객 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

		/// <summary>
		/// 고객리스트-페이징
		/// </summary>
		/// <param name="targetcollectionModel"></param>
		public void GetClientPageList(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            #region 기존 방식..
            /*
			try
			{  // 데이터베이스를 OPEN한다
				_db.Open(); 

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientPageList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("JobCode      :[" + targetcollectionModel.UserId       + "]");
				               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				
				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT top (" + targetcollectionModel.PageSize.Trim() + ")            \n"
					+ "       'False' AS CheckYn			\n"     
					+ "       ,A.CollectionCode			\n"     
					+ "		  ,A.UserId					\n"	
					+ "		  ,B.StbId					\n"	
					+ "		  ,B.PostNo					\n"						
					+ "		  ,B.ServiceCode			\n"	
					+ "		  ,B.ResidentNo				\n"	
					+ "		  ,B.Sex					\n"	
					+ "       ,CASE B.Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N  \n"
					+ "		  ,A.RegDt					\n"										
					+ "		  ,C.SummaryName DongName					\n"										
					+ "  FROM ClientList A with(noLock) LEFT JOIN StbUser B with(NoLock) ON (A.UserId          = B.UserId)        \n"					
					+ "					   LEFT JOIN SummaryCode C with(NoLock) ON ((select dbo.ufnGetTargetRegionCode2(substring(B.PostNo,0,4))) = C.SummaryCode AND C.SummaryType=5)        \n"					
					+ " WHERE  1 = 1					\n"
					);
				
				// 검색어가 있으면
				//				if (targetcollectionModel.SearchKey.Trim().Length > 0)
				//				{
				//					// 여러컬럼에 대하여 LIKE 검색을 한다.
				//					sbQuery.Append("\n"
				//						+ "  AND ( StbId		  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR PostNo	      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR ResidentNo     LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ "     OR Sex		      LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n"
				//						+ " 	)       \n"
				//						);
				//				}

				sbQuery.Append(" AND A.UserId not in(Select top ((" + targetcollectionModel.Page.Trim() + "-1)*" + targetcollectionModel.PageSize.Trim() + ")UserId \n");
				sbQuery.Append("					 From 	ClientList  \n");                 
			    sbQuery.Append("                     Where  CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "') \n");
				
				if(targetcollectionModel.CollectionCode.Trim().Length > 0 && !targetcollectionModel.CollectionCode.Trim().Equals("00"))			
					sbQuery.Append(" AND A.CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "' \n");       
				sbQuery.Append("  ORDER BY A.CollectionCode, A.UserId");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고팩키지모델에 복사
				targetcollectionModel.ClientListDataSet = ds.Copy();
				// 결과
				targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);
				// 결과코드 셋트
				targetcollectionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetClientPageList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD = "3000";
				targetcollectionModel.ResultDesc = "고객 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
            */
            #endregion
            try
            {  // 데이터베이스를 OPEN한다
                _db.ConnectionString = FrameSystem.connSummaryDbString;

                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientPageList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + targetcollectionModel.SearchKey + "]");
                _log.Debug("JobCode      :[" + targetcollectionModel.UserId + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                               
                // 특정 고객군
                sbQuery.Append("\n Select Top (" + targetcollectionModel.PageSize + ") ");
                sbQuery.Append("\n        'False' AS CheckYn ");
                sbQuery.Append("\n        ,A.CollectionCode ");
                sbQuery.Append("\n 		  ,A.UserId ");
                sbQuery.Append("\n 		  ,B.StbId ");
                sbQuery.Append("\n 		  ,B.PostNo ");
                sbQuery.Append("\n 		  ,B.ServiceCode ");
                sbQuery.Append("\n 		  ,B.ResidentNo ");
                sbQuery.Append("\n 		  ,B.Sex ");
                sbQuery.Append("\n        ,CASE B.Sex WHEN 'M' THEN '남' WHEN 'F' THEN '여' END AS Sex_N ");
                sbQuery.Append("\n 		  ,A.RegDt ");
                sbQuery.Append("\n        ,C.SummaryName DongName       ");
                sbQuery.Append("\n From	AdTargetsHanaTV.dbo.ClientList      a with(nolock)  ");
                sbQuery.Append("\n INNER join AdTargetsHanaTV.dbo.StbUser   b with(nolock) on b.UserId = a.UserId");
                sbQuery.Append("\n LEFT  JOIN SummaryCode C with(NoLock) ON ((select AdTargetsHanaTV.dbo.ufnGetTargetRegionCode2(substring(B.PostNo,0,4))) = C.SummaryCode AND C.SummaryType=5)        \n");
                sbQuery.Append("\n where    a.CollectionCode = " + targetcollectionModel.CollectionCode);
                // 검색어가 있으면
                if (targetcollectionModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.                        
                    sbQuery.Append("     AND (   b.StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.ServiceNum  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append(" 	    )  \n");
                }

                //sbQuery.Append("\n AND A.UserId not in(select top ((" + targetcollectionModel.Page.Trim() + "-1)*" + targetcollectionModel.PageSize.Trim() + ")UserId \n");
                //sbQuery.Append("\n  				   from 	ClientList  \n");
                //sbQuery.Append("\n                     Where CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "' \n");

                sbQuery.Append("\n AND A.UserId > ( select isnull( max(userId), 0)  \n");
                sbQuery.Append("\n                  from (  select top ((" + targetcollectionModel.Page.Trim() + "-1)*" + targetcollectionModel.PageSize.Trim() + ")UserId \n");
                sbQuery.Append("\n  				        from    AdTargetsHanaTV.dbo.ClientList nolock  \n");
                sbQuery.Append("\n                          Where   CollectionCode = '" + targetcollectionModel.CollectionCode.Trim() + "' \n");
                // 검색어가 있으면
                if (targetcollectionModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("     AND (   b.StbId		LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.PostNo	    LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append("     OR      b.ServiceNum  LIKE '%" + targetcollectionModel.SearchKey.Trim() + "%' \n");
                    sbQuery.Append(" 	    )  \n");
                }
                sbQuery.Append("        order by userId  ) v ) ");
                sbQuery.Append(" ORDER BY a.UserId");
               

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 광고팩키지모델에 복사
                targetcollectionModel.ClientListDataSet = ds.Copy();
                // 결과
                targetcollectionModel.ResultCnt = Utility.GetDatasetCount(targetcollectionModel.ClientListDataSet);
                // 결과코드 셋트
                targetcollectionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetcollectionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetClientPageList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetcollectionModel.ResultCD = "3000";
                targetcollectionModel.ResultDesc = "고객군 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

		/// <summary>
		/// 타겟군정보 저장
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetTargetCollectionUpdate(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[6];

				sbQuery.Append(""
					+ " UPDATE  Collection  \n"
					+ " SET	  CollectionName= @CollectionName	\n"										
					+ "      ,Comment		= @Comment  \n"			
					+ "      ,UseYn			= @UseYn    \n"	
					+ "      ,ModDt         = GETDATE() \n"
					+ "      ,RegID         = @RegID    \n"
					+ "      ,PvsYn         = @PvsYn	\n"
					+ " WHERE CollectionCode        = @CollectionCode        \n"
					);

				sqlParams[0] = new SqlParameter("@CollectionName" , SqlDbType.VarChar	, 500);												
				sqlParams[1] = new SqlParameter("@Comment"		, SqlDbType.VarChar , 2000);		
				sqlParams[2] = new SqlParameter("@UseYn"			, SqlDbType.Char	, 1);
				sqlParams[3] = new SqlParameter("@RegID"			, SqlDbType.VarChar , 10);
				sqlParams[4] = new SqlParameter("@CollectionCode" , SqlDbType.Int);
				sqlParams[5] = new SqlParameter("@PvsYn"			, SqlDbType.Char	, 1);

				sqlParams[0].Value = targetcollectionModel.CollectionName;								
				sqlParams[1].Value = targetcollectionModel.Comment;				
				sqlParams[2].Value = targetcollectionModel.UseYn;
				sqlParams[3].Value = header.UserID;      // 등록자
				sqlParams[4].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);
				sqlParams[5].Value = targetcollectionModel.PvsYn;

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("타겟군정보수정:["+targetcollectionModel.CollectionCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3201";
				targetcollectionModel.ResultDesc = "타겟군정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			// 데이터베이스를  Close한다
			_db.Close();

		}

		/// <summary>
		/// 타겟군 생성
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetTargetCollectionCreate(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[4];

				sbQuery.Append( ""
					+ "INSERT INTO Collection (	\n"
					+ "       CollectionCode    \n"
					+ "      ,CollectionName    \n"					
					+ "      ,Comment			\n"
					+ "		 ,UseYn				\n"															
					+ "      ,RegDt				\n"
					+ "      ,ModDt				\n"
					+ "      ,RegID             \n"
					+ "      ,PvsYn				\n"
					+ "      )                  \n"
					+ " SELECT                  \n"
					+ "       ISNULL(MAX(CollectionCode),0)+1        \n"
					+ "      ,@CollectionName   \n"					
					+ "      ,@Comment			\n"					
					+ "      ,'Y'				\n"					
					+ "      ,GETDATE()			\n"
					+ "      ,GETDATE()			\n"
					+ "      ,@RegID			\n"
					+ "      ,@PvsYn			\n"
					+ " FROM Collection               \n"
					);
				
				sqlParams[i++] = new SqlParameter("@CollectionName" , SqlDbType.VarChar , 40);				
				sqlParams[i++] = new SqlParameter("@Comment"		, SqlDbType.VarChar , 50);				
				sqlParams[i++] = new SqlParameter("@RegID"			, SqlDbType.VarChar , 10);
				sqlParams[i++] = new SqlParameter("@PvsYn"			, SqlDbType.VarChar , 1 );

				i = 0;				
				sqlParams[i++].Value = targetcollectionModel.CollectionName;				
				sqlParams[i++].Value = targetcollectionModel.Comment;				
				sqlParams[i++].Value = header.UserID;				// 등록자
				sqlParams[i++].Value = targetcollectionModel.PvsYn;


				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("타겟군정보생성:[" + targetcollectionModel.CollectionCode + "(" + targetcollectionModel.CollectionName + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3101";
				targetcollectionModel.ResultDesc = "타겟군정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}


		/// <summary>
		/// 고객리스트 생성
		/// </summary>
		public void SetClientCreate(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

                int     iCollectionCode =   Convert.ToInt32(targetcollectionModel.CollectionCode);
                string  iServiceNum     =   targetcollectionModel.UserId.ToString();

				StringBuilder sbQuery = new StringBuilder();

                #region [ 존재여부 확인및 등록여부 확인 ]
                // 해당가번이 사용자정보에 존재하는지 검색
                // 0:가입중, 1:사용중, 2:해지,3:일시중단,4:계약휴지,5:서비스제공휴지,7:예약가입,9:서비스중단, 10:접수취소
                sbQuery.Append(" select  sum( case gubun when 1 then cnt else 0 end )	as	Stb " + "\n");
                sbQuery.Append("		,sum( case gubun when 2 then cnt else 0 end )	as	Clt " + "\n");
                sbQuery.Append(" from ( " + "\n");
                sbQuery.Append("		select	1 as gubun ,count(*) as cnt " + "\n");
                sbQuery.Append("		from	StbUser with(noLock) "                  + "\n");
                sbQuery.Append("		where	ServiceNum = '" + iServiceNum + "'"     + "\n");
                sbQuery.Append("		and		ServiceStatusCode in(1) " + "\n");
                sbQuery.Append("		union all " + "\n");
                // 기등록된 데이터인지 검색
                sbQuery.Append("		select  2 as gubun, count(*) as cnt " + "\n");
                sbQuery.Append("		from    ClientList with(noLock) " + "\n");
                sbQuery.Append("		where	CollectionCode	= " +  iCollectionCode  + "\n");
                sbQuery.Append("		and     ServiceNum      = '" + iServiceNum + "'"    + "\n");
                 sbQuery.Append("	 ) v1 " + "\n");
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                if(ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception();
                }
                #endregion

                int userFound   =   Convert.ToInt32(ds.Tables[0].Rows[0]["Stb"].ToString());    // 광고고객정보에 등록되어 있는 가번인지
                int collFound   =   Convert.ToInt32(ds.Tables[0].Rows[0]["Clt"].ToString());    // 기등록된 가번인지
                int rc          = 0;
                ds.Dispose();
			
                //
                if( userFound == 0 )
                {
                    // 해당가번으로 등록된 고객이 없습니다.
                    targetcollectionModel.ResultCD   = "3101";
                    targetcollectionModel.ResultDesc = "광고DB에 존재하지 않는 가번입니다.";
                    return;
                }

                if( collFound > 0 )
                {
                    targetcollectionModel.ResultCD   = "3100";
                    targetcollectionModel.ResultDesc = "등록되어 있는 정보임으로 SKIP합니다.";
                    return;
                }
                sbQuery = new StringBuilder();
                sbQuery.Append(" insert into ClientList ( CollectionCode, UserId, ServiceNum, RegDt ) " + "\n");
                sbQuery.Append(" select   " + iCollectionCode + "\n");
                sbQuery.Append("         ,UserId " + "\n");
                sbQuery.Append("         ,ServiceNum " + "\n");
                sbQuery.Append("         ,GetDate() " + "\n");
                sbQuery.Append(" from	StbUser noLock " + "\n");
                sbQuery.Append(" where	ServiceNum = '" + iServiceNum + "'" + "\n");
				sbQuery.Append(" and	ServiceStatusCode = 1"  + "\n");

				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				try
				{
					rc =  _db.ExecuteNonQuery(sbQuery.ToString());
				}
				catch(Exception ex)
				{
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // 정상
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3109";
				targetcollectionModel.ResultDesc = "고객리스트정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}
		}

        /// <summary>
        /// [E_01] 특정 고객군에 등록할 사용자들 처리...
        /// OPENXML 이용하려 했으나 개별 처리에 대한 결과 분석이 난해해서 데이터 한 ROW씩 처리 하는 방식으로 해 둠.(트랜잭션 실행)
        /// </summary>      
        public void SetClientCollectionCreate(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {
                int iCollectionCode = Convert.ToInt32(targetcollectionModel.CollectionCode);
                
                int x, rc = 0;
                int userFound = 0; // 광고고객정보에 등록되어 있는 가번인지
                int collFound = 0; // 기등록된 가번인지

                int totalCnt = 0; // 처리 할 전체 데이터 수
                int readCnt = 0; // 처리한 데이터 수
                int addCnt = 0;  // 고객군에 추가된 등록 수
                int nonCnt = 0;  // 셋탑등록이 안된 정보 수
                int skipCnt = 0; // skip 수
                int failCnt = 0; // 등록 실패 수

                // 데이터 검수
                if (targetcollectionModel.ClientListDataSet.Tables == null
                    || targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"] == null
                    || targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count == 0)
                    throw new Exception("추가할 데이터가 없습니다!");

                StringBuilder sbQuery = new StringBuilder();
                
                // 데이터베이스를 OPEN한다
                _db.Open();

                #region 해당가번이 사용자정보에 존재하는지 검색 쿼리
                
                // 0:가입중, 1:사용중, 2:해지,3:일시중단,4:계약휴지,5:서비스제공휴지,7:예약가입,9:서비스중단, 10:접수취소
                sbQuery.Append(" select  sum( case gubun when 1 then cnt else 0 end )	as	Stb " + "\n");
                sbQuery.Append("		,sum( case gubun when 2 then cnt else 0 end )	as	Clt " + "\n");
                sbQuery.Append(" from (                                       \n");
                sbQuery.Append("		select	1 as gubun ,count(*) as cnt   \n");
                sbQuery.Append("		from	StbUser with(noLock)          \n");
                sbQuery.Append("		where	ServiceNum = @iServiceNum     \n");
                sbQuery.Append("		and		ServiceStatusCode in(0,1,3)   \n"); // 0,1,3
                sbQuery.Append("		union all                             \n");
                // 기등록된 데이터인지 검색
                sbQuery.Append("		select  2 as gubun, count(*) as cnt        \n");
                sbQuery.Append("		from    ClientList with(noLock)            \n");
                sbQuery.Append("		where	CollectionCode	= @iCollectionCode \n");
                sbQuery.Append("		and     ServiceNum      = @iServiceNum     \n");
                sbQuery.Append("	 ) v1       \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                #endregion

                #region 고객군에 insert 쿼리...
                StringBuilder sbQuery2 = new StringBuilder();
                sbQuery2.Append(" insert into ClientList ( CollectionCode, UserId, ServiceNum, RegDt )    \n");
                sbQuery2.Append(" select   @iCollectionCode          \n");
                sbQuery2.Append("         ,UserId                    \n");
                sbQuery2.Append("         ,ServiceNum                \n");
                sbQuery2.Append("         ,GetDate()                 \n");
                sbQuery2.Append(" from	StbUser noLock               \n");
                sbQuery2.Append(" where	ServiceNum = @iServiceNum    \n");
                sbQuery2.Append(" and	ServiceStatusCode in (0,1,3) \n"); // 가입중,사용중, 일시중지(query1과 동일조건)


                // __DEBUG__
                _log.Debug(sbQuery2.ToString());
                // __DEBUG__
                #endregion

                totalCnt = targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count;

                SqlParameter[] sqlParams = new SqlParameter[2];
                x = 0;
                sqlParams[x++] = new SqlParameter("@iCollectionCode", SqlDbType.Int);
                sqlParams[x++] = new SqlParameter("@iServiceNum", SqlDbType.VarChar,12);               

                int groupCnt = 1;    // 그룹 인덱스
                int processCnt = 10; // 그룹 별 처리 할 데이터 수
                if (totalCnt < processCnt) processCnt = 1;

                for (int i = 0; i < totalCnt; i+= processCnt) // 구간별 처리할 데이터수 만큼 증가
                {
                    _log.Debug("[SetClientCollectionCreate] 각 그룹별 처리 수:" + i.ToString());
                    for (int inx = i; inx < (processCnt * groupCnt); inx++) // 구간 내부 loop
                    {
                        if (inx < totalCnt)
                        {                           
                            readCnt++;
                            try
                            {
                                DataRow row = targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows[inx]; // 너무 길어서....
                                x = 0;
                                sqlParams[x++].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);
                                sqlParams[x++].Value = row["ServiceNum"].ToString();

                                #region [존재여부 확인및 등록여부 확인] DataReader 이용
                                using (SqlCommand cmd = new SqlCommand(sbQuery.ToString(), _db.SqlConn))
                                {                                    
                                    cmd.Parameters.Add("@iCollectionCode", SqlDbType.Int, 4);
                                    cmd.Parameters.Add("@iServiceNum", SqlDbType.VarChar, 12);
                                    
                                    cmd.Parameters[0].Value = sqlParams[0].Value;
                                    cmd.Parameters[1].Value = sqlParams[1].Value;
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            userFound = Convert.ToInt32(reader[0]);
                                            collFound = Convert.ToInt32(reader[1]);
                                        }
                                    }
                                    reader.Close();
                                }
                                if (userFound == 0) // 가입중, 사용중, 일시정지 상태의 가입자가 없다면 등록 할 수 없음.
                                    throw new Exception("3101"); // 해당가번으로 등록된 고객이 없습니다."광고DB에 존재하지 않는 가번입니다.";

                                if (collFound > 0) // 고객군에 사용자 정보가 등록되어 있음                        
                                    throw new Exception("3100"); //"등록되어 있는 정보임으로 SKIP합니다.";
                                #endregion
                                     
                                #region 등록 실행
                                try
                                {
                                    _db.BeginTran();
                                    rc = _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams);
                                    _db.CommitTran();
                                }
                                catch (Exception ex)
                                {
                                    _log.Debug("등록 예외 발생:" + ex.Message);
                                    _db.RollbackTran();
                                    throw new Exception("3000");
                                }

                                addCnt++; // 정상적인 추가
                                #endregion                               
                            }
                            catch (Exception ex)
                            {
                                _log.Debug("[SetClientCollectionCreate] 예외:" + ex.Message);
                                // 예외 정보 파싱해서 결과문자열로 클라이언트에 전달
                                if (ex.Message.Equals("3101")) // 존재 하지 않는 정보
                                    nonCnt++;
                                else if (ex.Message.Equals("3100")) // skip 정보
                                    skipCnt++;
                                else if (ex.Message.Equals("3000")) // 등록 실패
                                {
                                    failCnt++;
                                    _db.RollbackTran();
                                }
                                else
                                {
                                    failCnt++;                                   
                                }
                            }
                        } // end if(inx < totalCnt)
                    }
                    ++groupCnt; // 다음 구간
                    // 한 구간이 끝나면 다음 구간 처리시 일시정지?       
                    //Thread.Sleep(5);
                    //_log.Debug("[Sleep] 100]");
                }                                

                targetcollectionModel.ResultCD = "0000";  // 정상
                targetcollectionModel.ResultDesc = string.Format("{0:0},{1:0},{2:0},{3:0},{4:0},{5:0}", readCnt, totalCnt, addCnt, failCnt , nonCnt, skipCnt);
            }
            catch (Exception ex)
            {
                targetcollectionModel.ResultCD = "3109";
                targetcollectionModel.ResultDesc = ex.Message; //"고객리스트정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
                
            }
        }

        /// <summary>
        /// [E_02] 특정고객군에 등록 할 사용자들 처리 openxml 이용한 프로시져 실행
        /// 1 천 건씩 받아서 내부에서 몇 백 건씩 등록 처리..
        /// </summary>                
        public void SetClientCollectionCreateProc(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {
                int iCollectionCode = Convert.ToInt32(targetcollectionModel.CollectionCode);

                int x, rc = 0;              

                int totalCnt = 0; // 처리 할 전체 데이터 수
                int readCnt = 0; // 처리한 데이터 수
                int addCnt = 0;  // 고객군에 추가된 등록 수
                int nonCnt = 0;  // 셋탑등록이 안된 정보 수
                int skipCnt = 0; // skip 수
                int failCnt = 0; // 등록 실패 수

                // 데이터 검수
                if (targetcollectionModel.ClientListDataSet.Tables == null
                    || targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"] == null
                    || targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count == 0)
                    throw new Exception("추가할 데이터가 없습니다!");

                StringBuilder sbQuery = new StringBuilder();

                // 데이터베이스를 OPEN한다
                _db.Open();
                
                totalCnt = targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows.Count;

                SqlParameter[] sqlParams = new SqlParameter[2];
                x = 0;
                sqlParams[x++] = new SqlParameter("@CollectionCode", SqlDbType.Int);               
                sqlParams[x++] = new SqlParameter("@xmlDocument", SqlDbType.VarChar,-1);

                x = 0;
                sqlParams[x++].Direction = ParameterDirection.Input;              
                sqlParams[x++].Direction = ParameterDirection.Input;                
                

                int groupCnt = 1;    // 그룹 인덱스
                int processCnt = 400; // 그룹 별 처리 할 데이터 수
                int sendCount = 0;

                
                _log.Debug("[SetClientCollectionCreateProc] Start...");
                for (int i = 0; i < totalCnt; i += processCnt) // 구간별 처리할 데이터수 만큼 증가
                {
                    sbQuery.Clear();
                    sbQuery.Append("<ROOT>");
                   
                    for (int inx = i; inx < (processCnt * groupCnt); inx++) // 구간 내부 loop
                    {
                        if (inx < totalCnt)
                        {
                            readCnt++;                            
                            try
                            {
                                sendCount++;
                                DataRow row = targetcollectionModel.ClientListDataSet.Tables["CollectionUsers"].Rows[inx]; // 너무 길어서....
                                sbQuery.Append("<Cus Code=\"" + targetcollectionModel.CollectionCode + "\" SNum=\"" + row["ServiceNum"].ToString() + "\" />");
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("데이터 구성 예외:" + ex.Message);
                            }
                        } // end if(inx < totalCnt)
                    }
                    sbQuery.Append("</ROOT>");

                    DataSet ds = new DataSet();
                    #region 등록 실행
                    try
                    {
                        x = 0;
                        sqlParams[x++].Value = targetcollectionModel.CollectionCode;                       
                        sqlParams[x++].Value = sbQuery.ToString();

                        _db.BeginTran();                        
                        _db.ExecuteProcedure(ds, "[dbo].[TargetCollection_Create]", sqlParams);
                        _db.CommitTran();

                        if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                        {
                            nonCnt += sendCount - Convert.ToInt32(ds.Tables[0].Rows[0][0]); // 회원 아닌 수(전송한 수 - 가번등록 수)
                            skipCnt += Convert.ToInt32(ds.Tables[0].Rows[0][1]);           // 이미 등록된 회원 
                            addCnt += Convert.ToInt32(ds.Tables[0].Rows[0][2]);            // 실제 추가된 수
                        }
                        else
                            throw new Exception("3000");
                    }
                    catch (Exception ex)
                    {
                        _log.Debug("등록 예외 발생:" + ex.Message);
                        _db.RollbackTran();
                        throw new Exception("3000");
                    }
                    finally
                    {
                        sendCount = 0;
                        if (ds != null) ds = null;
                    }
                    #endregion                    
                    
                    ++groupCnt; // 다음 그룹  
                    Thread.Sleep(1000);
                }               
                targetcollectionModel.ResultCD = "0000";  // 정상
                targetcollectionModel.ResultDesc = string.Format("{0:0},{1:0},{2:0},{3:0},{4:0},{5:0}", readCnt, totalCnt, addCnt, failCnt, nonCnt, skipCnt);
            }
            catch (Exception ex)
            {
                targetcollectionModel.ResultCD = "3109";
                targetcollectionModel.ResultDesc = ex.Message; //"고객리스트정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
                _log.Debug("[SetClientCollectionCreateProc] End...");
            }
        }

		public void SetTargetCollectionDelete(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			int ClientListCount = 0;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQueryClientListCount = new StringBuilder();
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[1];

				sbQueryClientListCount.Append( "\n"
					+ "        SELECT COUNT(*) FROM    ClientList with(noLock)  \n"
					+ "              WHERE CollectionCode  = @CollectionCode          	\n"
					);  

				sbQuery.Append(""
					+ "DELETE Collection         \n"
					+ " WHERE CollectionCode  = @CollectionCode  \n"
					);

				sqlParams[i++] = new SqlParameter("@CollectionCode"       , SqlDbType.Int);

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);

				// 쿼리실행
				try
				{
					//매체대행광고주 관계 Count조사///////////////////////////////////////////////
					// __DEBUG__
					_log.Debug(sbQueryClientListCount.ToString());
					// __DEBUG__

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQueryClientListCount.ToString(),sqlParams);
                    
					ClientListCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					_log.Debug("ClientListCount          -->" + ClientListCount);

					// 이미 다른테이블에 사용중인 데이터가 있다면 Exception를 발생시킨다.
					if(ClientListCount > 0) throw new Exception();


					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("타겟군정보삭제:[" + targetcollectionModel.CollectionCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetCollectionDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3301";
				// 이미 다른테이블에 사용중인 데이터가 있다면
				if(ClientListCount > 0 )
				{
					targetcollectionModel.ResultDesc = "등록된 매체대행사가 있으므로 미디어렙정보를 삭제할수 없습니다.";
				}
				else
				{
					targetcollectionModel.ResultDesc = "미디어렙정보 삭제중 오류가 발생하였습니다";
				}
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

        /// <summary>
        /// 1개의 고객 데이터만 삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="targetcollectionModel"></param>
		public void SetClientDelete(HeaderModel header, TargetCollectionModel targetcollectionModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientDelete() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("CollectionCode  :[" + targetcollectionModel.CollectionCode       + "]");
				_log.Debug("UserId          :[" + targetcollectionModel.UserId       + "]");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[2];

				sbQuery.Append(""
					+ "DELETE ClientList         \n"
					+ " WHERE CollectionCode  = @CollectionCode  \n"
					+ " AND UserId  = @UserId  \n"
					);

				sqlParams[i++] = new SqlParameter("@CollectionCode"     , SqlDbType.Int);				
				sqlParams[i++] = new SqlParameter("@UserId"     , SqlDbType.Int);				
				
				i = 0;								
				sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.CollectionCode);
				sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.UserId);
				// 쿼리실행

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				try
				{
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Message("카테고리정보삭제:[" + targetcollectionModel.CollectionCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					throw ex;
				}

				targetcollectionModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetClientDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetcollectionModel.ResultCD   = "3301";
				targetcollectionModel.ResultDesc = "고객정보 삭제중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

        /// <summary>
        /// 전달 된 여러 고객들의 데이터 삭제[E_01]
        /// </summary>      
        public void SetClientCollectionDelete(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientCollectionDelete() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("CollectionCode  :[" + targetcollectionModel.CollectionCode + "]");
                _log.Debug("UserId          :[" + targetcollectionModel.UserId + "]");

                if (targetcollectionModel.ClientListDataSet.Tables.Count == 0
                   || targetcollectionModel.ClientListDataSet.Tables["Clients"] == null
                   || targetcollectionModel.ClientListDataSet.Tables["Clients"].Rows.Count == 0)
                    throw new Exception("처리할 데이터가 없습니다.!");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                int i = 0;
                int rc = 0;

                SqlParameter[] sqlParams = new SqlParameter[2];

                sbQuery.Append(""
                    + "DELETE ClientList         \n"
                    + " WHERE CollectionCode  = @CollectionCode  \n"
                    + " AND UserId  = @UserId  \n"
                    );

                _db.BeginTran();
                
                foreach (DataRow row in targetcollectionModel.ClientListDataSet.Tables[0].Rows)
                {
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@UserId", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(row["CollectionCode"]);
                    sqlParams[i++].Value = Convert.ToInt32(row["UserId"]);
                                                            
                   
                   rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                   _log.Message("고객군 사용자 정보 :[" + targetcollectionModel.CollectionCode + "] 등록자:[" + header.UserID + "]");                   
                }

                _db.CommitTran();

                targetcollectionModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientCollectionDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                targetcollectionModel.ResultCD = "3301";
                targetcollectionModel.ResultDesc = "고객정보 삭제중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


        /// <summary> 
        /// 특정 고객군의 고객리스트를 대상 고객군의 고객리스토로 복사/이동
        /// </summary>
        /// <param name="header"></param>
        /// <param name="targetcollectionModel"></param>
        public void SetClientListCopyMove(HeaderModel header, TargetCollectionModel targetcollectionModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientListCopy() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("FromCode  :[" + targetcollectionModel.FromCode + "]");
                _log.Debug("ToCode    :[" + targetcollectionModel.ToCode + "]");
                _log.Debug("CopyMove  :[" + targetcollectionModel.CopyMove + "]");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = null;
                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = null;

                _db.BeginTran();
                try
                {
                    // 복사전에 TO이 고객리스트를 삭제

                    #region TO이 고객리스트를 삭제

                    sbQuery = new StringBuilder();
                    sqlParams = new SqlParameter[1];

                    sbQuery.Append("--고객리스트 이동용 삭제\n"
                        + "DELETE ClientList         \n"
                        + " WHERE CollectionCode  = @ToCode  \n"
                        );

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@ToCode", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.ToCode);
                    // 쿼리실행

                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    #endregion

                    // 복사 From 에서 To 로
                    #region From 에서 To 로 복사
                    sbQuery = new StringBuilder();
                    sqlParams = new SqlParameter[2];

                    sbQuery.Append("--고객리스트 복사\n"
                        + "INSERT INTO ClientList         \n"
                        + "     SELECT A.CollectionCode     \n"
                        + "           ,A.UserId \n"
                        + "           ,A.RegDt \n"
                        + "           ,A.ServiceNum \n"
                        + "     FROM          \n"
                        + "     (SELECT @ToCode AS CollectionCode \n"
                        + "           ,UserId \n"
                        + "           ,RegDt \n"
                        + "           ,ServiceNum \n"
                        + "       FROM ClientList   \n"
                        + "      WHERE CollectionCode = @FromCode \n"
                        + "      EXCEPT \n"                             //EXCEPT를 이용하여 키중복 방지
                        + "     SELECT CollectionCode \n"
                        + "           ,UserId \n"
                        + "           ,RegDt \n"
                        + "           ,ServiceNum \n"
                        + "       FROM ClientList   \n"
                        + "      WHERE CollectionCode = @ToCode) A \n"
                        );

                    i = 0;
                    sqlParams[i++] = new SqlParameter("@FromCode", SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@ToCode", SqlDbType.Int);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.FromCode);
                    sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.ToCode);
                    // 쿼리실행
                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    #endregion

                    // 이동인지 검사
                    if(targetcollectionModel.CopyMove.Equals("M"))
                    {
                        // 이동이므로 From의 고객리스트를 삭제한다.
                        #region From 삭제
                        sbQuery = new StringBuilder();
                        sqlParams = new SqlParameter[1];

                        sbQuery.Append("--고객리스트 이동용 삭제\n"
                            + "DELETE ClientList         \n"
                            + " WHERE CollectionCode  = @FromCode  \n"
                            );

                        i = 0;
                        sqlParams[i++] = new SqlParameter("@FromCode", SqlDbType.Int);

                        i = 0;
                        sqlParams[i++].Value = Convert.ToInt32(targetcollectionModel.FromCode);
                        // 쿼리실행

                        // __DEBUG__
                        _log.Debug(sbQuery.ToString());
                        // __DEBUG__

                        rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                        #endregion
                    }

                    string at = "";
                    if (targetcollectionModel.CopyMove.Equals("M")) at = "이동";
                    else at = "복사";

                    _log.Message("고객리스트 " + at + ":From[" + targetcollectionModel.FromCode + "] To[" + targetcollectionModel.FromCode + "] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                _db.CommitTran();

                targetcollectionModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetClientListCopyMove() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                _db.RollbackTran();

                targetcollectionModel.ResultCD = "3301";
                targetcollectionModel.ResultDesc = "고객리스트 복사/이동 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }
	}
}
