/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드	: [E_01]
 * 수정자		: YJ.Park
 * 수정일		: 2014.11.13
 * 수정내용	: HomeCount에 홈광고(키즈) 편성 추가
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
	/// GradeBiz에 대한 요약 설명입니다.
	/// </summary>
	public class GradeBiz : BaseBiz
	{
		public GradeBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		///  등급콤보조회
		/// </summary>
		/// <param name="gradeModel"></param>
		public void GetGradeCodeList(HeaderModel header, GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGradeList() Start");
				_log.Debug("-----------------------------------------");

				
				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("Section :[" + gradeModel.MediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT Code, CodeName  \n"
					+ "   FROM GradeCode               \n"				
					);
			

				sbQuery.Append(" ORDER BY Code \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				gradeModel.GradeDataSet = ds.Copy();
				// 결과
				gradeModel.ResultCnt = Utility.GetDatasetCount(gradeModel.GradeDataSet);
				// 결과코드 셋트
				gradeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + gradeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGradeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				gradeModel.ResultCD = "3000";
				gradeModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		/// <summary>
		/// 채널셋목록조회
		/// </summary>
		/// <param name="gradeModel"></param>
		public void GetGradeList(HeaderModel header, GradeModel gradeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGradeList() Start");
				_log.Debug("-----------------------------------------");
				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");				
				_log.Debug("Code:[" + gradeModel.Code + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT		Code											\n"
					+ "			   ,CodeName										\n"					
					+ "			   ,Grade											\n"
					+ "			   ,RegID											\n"
					+ " 		   ,convert(char(10), RegDt, 120) RegDt				\n"
					+ " 		   ,convert(char(10), ModDt, 120) ModDt				\n"
					+ "   FROM GradeCode with(noLock)          						\n"					
					);								

				// 채널셋레벨을 선택했으면
				if(gradeModel.MediaCode.Trim().Length > 0 && !gradeModel.MediaCode.Trim().Equals("00"))
				{
					sbQuery.Append(" WHERE Code = '" + gradeModel.Code.Trim() + "' \n");
				}		
								
				sbQuery.Append("\n ORDER BY Code");

				// __DEBUG__
				_log.Debug("MediaCode:[" + gradeModel.Code + "]");
				
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 매체대행사광고주모델에 복사
				gradeModel.GradeDataSet = ds.Copy();
				// 결과
				gradeModel.ResultCnt = Utility.GetDatasetCount(gradeModel.GradeDataSet);
				// 결과코드 셋트
				gradeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + gradeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGradeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				gradeModel.ResultCD = "3000";
				gradeModel.ResultDesc = "등급정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}
		

		/// <summary>
		/// 카테고리 장르 목록조회
		/// </summary>
		/// <param name="gradeModel"></param>
		/// 
		public void GetContractItemList(HeaderModel header, GradeModel gradeModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + gradeModel.SearchKey       + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
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
				// 광고상태 선택에 따라

				// 광고상태 준비
				if(gradeModel.SearchchkAdState_10.Trim().Length > 0 && gradeModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND ( A.AdState  = '10' \n");
					isState = true;
				}	
				// 광고상태 편성
				if(gradeModel.SearchchkAdState_20.Trim().Length > 0 && gradeModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '20' \n");
					isState = true;
				}	
				// 광고상태 중지
				if(gradeModel.SearchchkAdState_30.Trim().Length > 0 && gradeModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '30' \n");
					isState = true;
				}	
				// 광고상태 종료
				if(gradeModel.SearchchkAdState_40.Trim().Length > 0 && gradeModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" A.AdState  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");

				// 검색어가 있으면
				if (gradeModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
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

				// 편성여부가 Y/N일 때 
//				if(gradeModel.SearchChkSch_YN.Trim().Length > 0)
//				{
//					if(gradeModel.SearchChkSch_YN.Trim().Equals("Y"))
//					{
//						// 편성된것만
//						sbQuery.Append(" AND ( HomeCount > 0 OR MenuCount > 0 OR  ChannelCount > 0 ) \n");
//					}
//					else if(gradeModel.SearchChkSch_YN.Trim().Equals("N"))
//					{
//						//편성안된것만
//						sbQuery.Append(" AND ( HomeCount = 0 AND MenuCount = 0 AND  ChannelCount = 0 )\n");
//					}
//				}
				// 아니면 전체
       
				sbQuery.Append("ORDER BY ItemNo Desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고내역모델에 복사
				gradeModel.ContractItemDataSet = ds.Copy();
				// 결과
				gradeModel.ResultCnt = Utility.GetDatasetCount(gradeModel.ContractItemDataSet);
				// 결과코드 셋트
				gradeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + gradeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetContractItemList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				gradeModel.ResultCD = "3000";
				gradeModel.ResultDesc = "광고내역정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		/// <summary>
		///  채널정보 저장
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

				// 데이터베이스를 OPEN한다
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

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("카테고리정보수정:["+gradeModel.Code_O + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				gradeModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGradeUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				gradeModel.ResultCD   = "3101";
				gradeModel.ResultDesc = "등급정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 채널셋 등록
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

				// 데이터베이스를 OPEN한다
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
				sqlParams[i++].Value = header.UserID;				// 등록자		
								
				
				_log.Debug("Code:[" + gradeModel.Code + "]");
				_log.Debug("CodeName:[" + gradeModel.CodeName + "]");
				_log.Debug("Grade:[" + gradeModel.Grade + "]");
								
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("채널셋정보생성:[" + gradeModel.Code + "(" + gradeModel.Code + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				gradeModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGradeCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				gradeModel.ResultCD   = "3101";
				gradeModel.ResultDesc = "채널셋정보생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
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

				// 데이터베이스를 OPEN한다
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
                        
				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();
                        
					// __MESSAGE__
					_log.Message("채널정보삭제:[" + gradeModel.Code + "] 등록자:[" + header.UserID + "]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				gradeModel.ResultCD = "0000";  // 정상
                        	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetGradeDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				gradeModel.ResultCD   = "3301";
				gradeModel.ResultDesc = "등급정보 삭제중 오류가 발생하였습니다";
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