using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Media
{
	/// <summary>
	/// ChannelSetBiz에 대한 요약 설명입니다.
	/// </summary>
	public class ChannelSetBiz : BaseBiz
	{
		public ChannelSetBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		///  카테고리콤보조회
		/// </summary>
		/// <param name="channelSetModel"></param>
		public void GetCategoryList(HeaderModel header, ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCategoryList() Start");
				_log.Debug("-----------------------------------------");

				
				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("Section :[" + channelSetModel.MediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT CategoryCode, CategoryName  \n"
					+ "   FROM Category with(nolock)       \n"
					//+ "  WHERE MediaCode <> '00'             \n"
					);
			

				sbQuery.Append(" ORDER BY MediaCode \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				channelSetModel.CategoryDataSet = ds.Copy();
				// 결과
				channelSetModel.ResultCnt = Utility.GetDatasetCount(channelSetModel.CategoryDataSet);
				// 결과코드 셋트
				channelSetModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + channelSetModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCategoryList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				channelSetModel.ResultCD = "3000";
				channelSetModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		/// <summary>
		///  장르콤보조회
		/// </summary>
		/// <param name="channelSetModel"></param>
		public void GetGenreList(HeaderModel header, ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("Section :[" + channelSetModel.MediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT GenreCode, GenreName  \n"
					+ "   FROM Genre with(nolock)    \n"					
					);		
				if(channelSetModel.SearchKey.Trim().Length > 0 && !channelSetModel.SearchKey.Trim().Equals("00"))
				{
					sbQuery.Append(" WHERE GenreName LIKE '%" + channelSetModel.SearchKey.Trim() + "%' \n");
				}		

				sbQuery.Append(" ORDER BY MediaCode \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				channelSetModel.GenreDataSet = ds.Copy();
				// 결과
				channelSetModel.ResultCnt = Utility.GetDatasetCount(channelSetModel.GenreDataSet);
				// 결과코드 셋트
				channelSetModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + channelSetModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				channelSetModel.ResultCD = "3000";
				channelSetModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
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
		/// <param name="channelSetModel"></param>
		public void GetChannelSetList(HeaderModel header, ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelSetList() Start");
				_log.Debug("-----------------------------------------");
				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + channelSetModel.SearchKey       + "]");
				_log.Debug("SearchClient:[" + channelSetModel.MediaName + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT	a.MediaCode											\n"
					+ "			   ,a.CategoryCode							 \n"
					+ "			   ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.CategoryCode))) +  CONVERT(VARCHAR(10),a.CategoryCode) + ' ' + e.CategoryName) AS CategoryName				\n"	
					+ "			   ,a.GenreCode							 \n"
					+ "			   ,d.GenreName							 \n"
					+ "       ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.GenreCode)))    +  CONVERT(VARCHAR(10),a.GenreCode)    + ' ' + d.GenreName   ) AS genreName    		        \n"
					+ "			   ,a.ChannelNo							 \n"
					//+ "			   ,a.SeriesNo							 \n"
					+ "			   ,b.TotalSeries							 \n"
					+ " 		   ,b.Title								 \n"
					+ " 		   ,convert(char(10), a.ModDt, 120) ModDt								 \n"
					+ "   FROM ChannelSet a with(nolock) LEFT JOIN Channel b with(nolock) \n"
					+ "			ON a.MediaCode = b.MediaCode            		 \n"
					+ "			and a.ChannelNo = b.ChannelNo            		 \n"				
					+ "   LEFT JOIN Genre d with(nolock)        		 \n"
					+ "			ON a.GenreCode = d.GenreCode            		 \n"	
					+ "   LEFT JOIN Category e with(nolock)        		 \n"
					+ "			ON a.CategoryCode = e.CategoryCode            		 \n"	
					);								

				// 채널셋레벨을 선택했으면
				if(channelSetModel.MediaCode.Trim().Length > 0 && !channelSetModel.MediaCode.Trim().Equals("00"))
				{
					sbQuery.Append(" WHERE A.MediaCode = '" + channelSetModel.MediaCode.Trim() + "' \n");
				}		
				if(channelSetModel.CategoryCode.Trim().Length > 0 && !channelSetModel.CategoryCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.CategoryCode = '" + channelSetModel.CategoryCode.Trim() + "' \n");
				}		
				if(channelSetModel.GenreCode.Trim().Length > 0 && !channelSetModel.GenreCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.GenreCode = '" + channelSetModel.GenreCode.Trim() + "' \n");
				}		
								
				 sbQuery.Append("\n  GROUP BY a.ChannelNo,b.TotalSeries,b.Title,convert(char(10), a.ModDt, 120),a.MediaCode,a.CategoryCode,CategoryName,a.GenreCode,d.GenreName,genreName ");
				sbQuery.Append("\n ORDER BY A.MediaCode, A.CategoryCode, A.GenreCode, A.ChannelNo");

				// __DEBUG__
				_log.Debug("MediaCode:[" + channelSetModel.MediaCode + "]");
				_log.Debug("CategoryCode:[" + channelSetModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + channelSetModel.GenreCode + "]");
				//_log.Debug("AdvertiserCode:[" + clientModel.AdvertiserCode + "]");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 매체대행사광고주모델에 복사
				channelSetModel.ChannelSetDataSet = ds.Copy();
				// 결과
				channelSetModel.ResultCnt = Utility.GetDatasetCount(channelSetModel.ChannelSetDataSet);
				// 결과코드 셋트
				channelSetModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + channelSetModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelSetList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				channelSetModel.ResultCD = "3000";
				channelSetModel.ResultDesc = "채널셋정보 조회중 오류가 발생하였습니다";
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
		/// <param name="channelSetModel"></param>
		/// 
		public void GetCategenList(HeaderModel header, ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCategenList() Start");
				_log.Debug("-----------------------------------------");
				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + channelSetModel.SearchKey       + "]");
				_log.Debug("SearchClient:[" + channelSetModel.CategoryName + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				int i = 0;
				SqlParameter[] sqlParams = new SqlParameter[1];
					
				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT a.MediaCode    				 \n"
					+ "       ,b.MediaName							 \n"
					+ "       ,a.CategoryCode							 \n"
					+ "		  ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.CategoryCode))) +  CONVERT(VARCHAR(10),a.CategoryCode) + ' ' + c.CategoryName) AS CategoryName				\n"	
					+ "       ,a.GenreCode						     \n"
					+ "       ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.GenreCode)))    +  CONVERT(VARCHAR(10),a.GenreCode)    + ' ' + d.GenreName   ) AS GenreName    		        \n"
					+ "   FROM (											\n" 
					+ "			SELECT    a.MediaCode							\n" 
				   + "					 ,a.CategoryCode							\n" 
				   + "					 ,a.GenreCode							\n" 
				   + "			FROM      ChannelSet a with(nolock)				\n" 
				   + "			WHERE     a.MediaCode = @SearchMediaName							\n" 
				    + "			GROUP BY  a.MediaCode							\n" 
					+ "					,a.CategoryCode							\n" 
					+ "					,a.GenreCode							\n" 
					+ "			)  a,Media b with(nolock), Category c with(nolock), Genre d with(nolock)	\n" 					
					+ "	WHERE	a.MediaCode		= b.MediaCode           		 \n"					
					+ "	AND		a.CategoryCode	= c.CategoryCode           		 \n"					
					+ "	AND		a.GenreCode		= d.GenreCode           		 \n"					
					);
								

				// 채널셋레벨을 선택했으면
				if(channelSetModel.SearchMediaName.Trim().Length > 0 && !channelSetModel.SearchMediaName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode = '" + channelSetModel.SearchMediaName.Trim() + "' \n");
				}		
				if(channelSetModel.SearchCategoryName.Trim().Length > 0 && !channelSetModel.SearchCategoryName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.CategoryCode = '" + channelSetModel.SearchCategoryName.Trim() + "' \n");
				}		
				if(channelSetModel.SearchGenreName.Trim().Length > 0 && !channelSetModel.SearchGenreName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.GenreCode = '" + channelSetModel.SearchGenreName.Trim() + "' \n");
				}		
				if (channelSetModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
						+ "    C.CategoryName      LIKE '%" + channelSetModel.SearchKey.Trim() + "%' \n"
						+ " OR D.GenreName    LIKE '%" + channelSetModel.SearchKey.Trim() + "%' \n"											
						+ " ) ");
				}
								
				//sbQuery.Append(" GROUP BY A.MediaCode,b.MediaName,a.CategoryCode,c.CategoryName,d.GenreCode,d.GenreName  \n");
				sbQuery.Append(" ORDER BY C.CategoryCode,D.GenreCode   \n");

				i = 0;
				sqlParams[i++] = new SqlParameter("@SearchMediaName"    , SqlDbType.TinyInt);
                               
				i = 0;
				sqlParams[i++].Value = channelSetModel.SearchMediaName;	

				// __DEBUG__
				_log.Debug("MediaCode:[" + channelSetModel.MediaCode + "]");
				_log.Debug("CategoryCode:[" + channelSetModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + channelSetModel.GenreCode + "]");
				
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				 _db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// 결과 DataSet의 매체대행사광고주모델에 복사
				channelSetModel.ChannelSetDataSet = ds.Copy();
				// 결과
				channelSetModel.ResultCnt = Utility.GetDatasetCount(channelSetModel.ChannelSetDataSet);
				// 결과코드 셋트
				channelSetModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + channelSetModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCategenList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				channelSetModel.ResultCD = "3000";
				channelSetModel.ResultDesc = "채널셋정보 조회중 오류가 발생하였습니다";
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
		/// <param name="channelSetModel"></param>
		public void GetChannelNoPopList(HeaderModel header, ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelNoPopList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + channelSetModel.SearchKey       + "]");
				_log.Debug("SearchClient:[" + channelSetModel.MediaName + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// 쿼리생성				
				sbQuery.Append("\n"
					+ " SELECT a.ChannelNo							 \n"
					+ "			   ,a.SeriesNo							 \n"
					+ "			   ,b.TotalSeries							 \n"
					+ " 		   ,b.Title								 \n"					
					+ " 		   ,convert(char(10), a.ModDt, 120) ModDt								 \n"					
					+ "   FROM ChannelSet a with(nolock) LEFT JOIN Channel b with(nolock)  		 \n"
					+ "			ON a.MediaCode = b.MediaCode            		 \n"
					+ "			and a.ChannelNo = b.ChannelNo            		 \n"												
					+ "			and a.SeriesNo = b.SeriesNo            		 \n"												
					);								

				// 채널셋레벨을 선택했으면
				if(channelSetModel.MediaCode_P.Trim().Length > 0 && !channelSetModel.MediaCode_P.Trim().Equals("00"))
				{
					sbQuery.Append(" WHERE A.MediaCode = '" + channelSetModel.MediaCode_P.Trim() + "' \n");
				}		
				if(channelSetModel.CategoryCode_P.Trim().Length > 0 && !channelSetModel.CategoryCode_P.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.CategoryCode = '" + channelSetModel.CategoryCode_P.Trim() + "' \n");
				}		
				if(channelSetModel.GenreCode_P.Trim().Length > 0 && !channelSetModel.GenreCode_P.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.GenreCode = '" + channelSetModel.GenreCode_P.Trim() + "' \n");
				}		
								
				sbQuery.Append("  GROUP BY a.ChannelNo,a.SeriesNo,b.TotalSeries,b.Title,convert(char(10), a.ModDt, 120),a.MediaCode,a.CategoryCode,a.GenreCode ");
				sbQuery.Append(" ORDER BY A.MediaCode DESC  \n");

				// __DEBUG__
				_log.Debug("MediaCode:[" + channelSetModel.MediaCode + "]");
				_log.Debug("CategoryCode:[" + channelSetModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + channelSetModel.GenreCode + "]");
				//_log.Debug("AdvertiserCode:[" + clientModel.AdvertiserCode + "]");

				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 매체대행사광고주모델에 복사
				channelSetModel.ChannelSetDataSet = ds.Copy();
				// 결과
				channelSetModel.ResultCnt = Utility.GetDatasetCount(channelSetModel.ChannelSetDataSet);
				// 결과코드 셋트
				channelSetModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + channelSetModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelNoPopList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				channelSetModel.ResultCD = "3000";
				channelSetModel.ResultDesc = "채널셋정보 조회중 오류가 발생하였습니다";
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
		/// <param name="channelSetModel"></param>
		public void SetChannelSetUpdate(HeaderModel header, ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelSetUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQuery2 = new StringBuilder();
                        			

				sbQuery.Append( ""
					+ "        DELETE FROM  ChannelSet			                    \n"
					+ "              WHERE MediaCode = @MediaCode_old				\n"
					+ "                AND CategoryCode = @CategoryCode_old				\n"
					+ "                AND GenreCode = @GenreCode_old				\n"
					+ "                AND ChannelNo = @ChannelNo_old				\n"
					);                                                             
				sbQuery2.Append( ""
					+ "INSERT INTO ChannelSet (                         \n"
					+ "		 MediaCode                \n"															
					+ "		,CategoryCode                \n"															
					+ "		,ChannelNo                \n"															
					+ "		,SeriesNo                \n"															
					+ "		,GenreCode                \n"																																									
					+ "      )                                          \n"
					+ " SELECT                                        \n"			
					+ "       MediaCode      \n"						
					+ "      ,@CategoryCode      \n"		
					+ "      ,ChannelNo      \n"		
					+ "      ,SeriesNo      \n"		
					+ "      ,@GenreCode      \n"																			
					+ " FROM Channel with(nolock)	\n"					
					+ " WHERE MediaCode	=	@MediaCode\n"					
					+ " AND ChannelNo	=	@ChannelNo	\n"					
					);                		
                        
				// 쿼리실행
				try
				{                    
					int i = 0;
					int rc = 0;
					SqlParameter[] sqlParams = new SqlParameter[4];
					_db.BeginTran();
                     
					//먼저 인서트 된 데이터를 지워주고 그이후에 인서트를 한다.
					i = 0;
					sqlParams[i++] = new SqlParameter("@MediaCode_old" , SqlDbType.TinyInt);
					sqlParams[i++] = new SqlParameter("@CategoryCode_old"    , SqlDbType.Int);
					sqlParams[i++] = new SqlParameter("@GenreCode_old"    , SqlDbType.Int);
					sqlParams[i++] = new SqlParameter("@ChannelNo_old"    , SqlDbType.Int);
                                                   
					i = 0;
					sqlParams[i++].Value = Convert.ToInt16(channelSetModel.MediaCode_old);
					sqlParams[i++].Value = Convert.ToInt32(channelSetModel.CategoryCode_old);
					sqlParams[i++].Value = Convert.ToInt32(channelSetModel.GenreCode_old);
					sqlParams[i++].Value = Convert.ToInt32(channelSetModel.ChannelNo_old);	
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// __DEBUG__
					_log.Debug("MediaCode_old:[" + channelSetModel.MediaCode_old + "]");
					_log.Debug("CategoryCode_old:[" + channelSetModel.CategoryCode_old + "]");
					_log.Debug("GenreCode_old:[" + channelSetModel.GenreCode_old + "]");
					_log.Debug("ChannelNo_old:[" + channelSetModel.ChannelNo_old + "]");
					_log.Debug(sbQuery.ToString());
					// __DEBUG__

					sqlParams = new SqlParameter[4];
					
						i = 0;
					sqlParams[i++] = new SqlParameter("@CategoryCode"     , SqlDbType.Int);				
					sqlParams[i++] = new SqlParameter("@GenreCode"     , SqlDbType.Int);				
					sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt);
					sqlParams[i++] = new SqlParameter("@ChannelNo"     , SqlDbType.Int);                  
                        
						i = 0;
					sqlParams[i++].Value = Convert.ToInt16(channelSetModel.CategoryCode);				
					sqlParams[i++].Value = Convert.ToInt16(channelSetModel.GenreCode);
					sqlParams[i++].Value = Convert.ToInt16(channelSetModel.MediaCode);
					sqlParams[i++].Value = Convert.ToInt32(channelSetModel.ChannelNo);	

						

					_log.Debug("MediaCode:[" + channelSetModel.MediaCode + "]");
					_log.Debug("CategoryCode:[" + channelSetModel.CategoryCode + "]");
					_log.Debug("GenreCode:[" + channelSetModel.GenreCode + "]");
					_log.Debug("ChannelNo:[" + channelSetModel.ChannelNo + "]");
					_log.Debug("SeriesNo:[" + channelSetModel.SeriesNo + "]");
						// __DEBUG__
						_log.Debug(sbQuery2.ToString());
						// __DEBUG__
                        rc = _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams);
					_db.CommitTran();
                        
					// __MESSAGE__
					_log.Message("채널셋정보생성:[성공]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				channelSetModel.ResultCD = "0000";  // 정상
                        
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelSetUpdate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				channelSetModel.ResultCD   = "3101";
				channelSetModel.ResultDesc = "채널셋정보 생성 중 오류가 발생하였습니다";
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
		/// <param name="channelSetModel"></param>
		public void SetChannelSetCreate(HeaderModel header, ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelSetCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[4];
				
				sbQuery.Append( ""
					+ "INSERT INTO ChannelSet (                         \n"
					+ "		 MediaCode                \n"															
					+ "		,CategoryCode                \n"															
					+ "		,ChannelNo                \n"															
					+ "		,SeriesNo                \n"															
					+ "		,GenreCode                \n"																																									
					+ "      )                                          \n"
					+ " SELECT                                        \n"			
					+ "       MediaCode      \n"						
					+ "      ,@CategoryCode      \n"		
					+ "      ,ChannelNo      \n"		
					+ "      ,SeriesNo      \n"		
					+ "      ,@GenreCode      \n"																			
					+ " FROM Channel with(nolock)		\n"					
					+ " WHERE MediaCode	=	@MediaCode\n"					
					+ " AND ChannelNo	=	@ChannelNo	\n"					
					);                		
								
				sqlParams[i++] = new SqlParameter("@CategoryCode"     , SqlDbType.Int);				
				sqlParams[i++] = new SqlParameter("@GenreCode"     , SqlDbType.Int);
				
				sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@ChannelNo"     , SqlDbType.Int);
				
				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(channelSetModel.CategoryCode);				
				sqlParams[i++].Value = Convert.ToInt32(channelSetModel.GenreCode);

				sqlParams[i++].Value = Convert.ToInt32(channelSetModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(channelSetModel.ChannelNo);	
								
				
				_log.Debug("MediaCode:[" + channelSetModel.MediaCode + "]");
				_log.Debug("CategoryCode:[" + channelSetModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + channelSetModel.GenreCode + "]");
				_log.Debug("ChannelNo:[" + channelSetModel.ChannelNo + "]");
				_log.Debug("SeriesNo:[" + channelSetModel.SeriesNo + "]");
				
				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("채널셋정보생성:[" + channelSetModel.ChannelNo + "(" + channelSetModel.ChannelNo + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				channelSetModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelSetCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				channelSetModel.ResultCD   = "3101";
				channelSetModel.ResultDesc = "채널셋정보생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		public void SetChannelSetDelete(HeaderModel header, ChannelSetModel channelSetModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelSetDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();
                        
				StringBuilder sbQuery = new StringBuilder();
                        
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[4];
                        
				sbQuery.Append( ""
					+ "        DELETE FROM  ChannelSet			                    \n"
					+ "              WHERE MediaCode = @MediaCode				\n"
					+ "                AND CategoryCode = @CategoryCode				\n"
					+ "                AND GenreCode = @GenreCode				\n"
					+ "                AND ChannelNo = @ChannelNo				\n"
					);                             
                                        
				sqlParams[i++] = new SqlParameter("@MediaCode" , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@CategoryCode"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@GenreCode"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int);
			        
				i = 0;				
				sqlParams[i++].Value = Convert.ToInt16(channelSetModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(channelSetModel.CategoryCode);
				sqlParams[i++].Value = Convert.ToInt32(channelSetModel.GenreCode);
				sqlParams[i++].Value = Convert.ToInt32(channelSetModel.ChannelNo);

				_log.Debug("MediaCode:[" + channelSetModel.MediaCode + "]");
				_log.Debug("CategoryCode:[" + channelSetModel.CategoryCode + "]");
				_log.Debug("GenreCode:[" + channelSetModel.GenreCode + "]");
				_log.Debug("ChannelNo:[" + channelSetModel.ChannelNo + "]");
				
				_log.Debug(sbQuery.ToString());
                        
				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();
                        
					// __MESSAGE__
					_log.Message("채널정보삭제:[" + channelSetModel.ChannelNo + "] 등록자:[" + header.UserID + "]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				channelSetModel.ResultCD = "0000";  // 정상
                        	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelSetDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				channelSetModel.ResultCD   = "3301";
				channelSetModel.ResultDesc = "채널셋정보 삭제중 오류가 발생하였습니다";
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