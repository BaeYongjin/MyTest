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
	/// SlotOrganizationBiz에 대한 요약 설명입니다.
	/// </summary>
	public class SlotOrganizationBiz : BaseBiz
	{
		public SlotOrganizationBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

		/// <summary>
		///  카테고리콤보조회
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		public void GetCategoryList(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
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
				_log.Debug("Section :[" + slotOrganizationModel.MediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT CategoryCode, CategoryName  \n"
					+ "   FROM Category               \n"
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
				slotOrganizationModel.CategoryDataSet = ds.Copy();
				// 결과
				slotOrganizationModel.ResultCnt = Utility.GetDatasetCount(slotOrganizationModel.CategoryDataSet);
				// 결과코드 셋트
				slotOrganizationModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + slotOrganizationModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCategoryList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				slotOrganizationModel.ResultCD = "3000";
				slotOrganizationModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
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
		/// <param name="slotOrganizationModel"></param>
		public void GetGenreList(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
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
				_log.Debug("Section :[" + slotOrganizationModel.MediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
			
				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT a.GenreCode						     \n"					
					+ "       ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.GenreCode)))    +  CONVERT(VARCHAR(10),a.GenreCode)    + ' ' + d.GenreName   ) AS GenreName    		        \n"
					+ "   FROM (											\n" 
					+ "				SELECT    a.MediaCode							\n" 
					+ "							 ,a.CategoryCode							\n" 
					+ "							 ,a.GenreCode							\n" 
					+ "				FROM      ChannelSet a							\n" 
					+ "				WHERE     a.MediaCode = '" + slotOrganizationModel.SearchMediaName.Trim() + "'							\n" 
					+ "				GROUP BY  a.MediaCode							\n" 
					+ "							,a.CategoryCode							\n" 
					+ "							,a.GenreCode							\n" 
					+ "				)  a,Media b, Category c, Genre d							\n" 					
					+ "			 WHERE a.MediaCode = b.MediaCode           		 \n"					
					+ "			 AND a.CategoryCode = c.CategoryCode           		 \n"					
					+ "			 AND a.GenreCode = d.GenreCode           		 \n"					
					);
				if(slotOrganizationModel.SearchKey.Trim().Length > 0 && !slotOrganizationModel.SearchKey.Trim().Equals("00"))
				{
					sbQuery.Append(" AND D.GenreName LIKE '%" + slotOrganizationModel.SearchKey.Trim() + "%' \n");
				}	
				if(slotOrganizationModel.SearchMediaName.Trim().Length > 0 && !slotOrganizationModel.SearchMediaName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode = '" + slotOrganizationModel.SearchMediaName.Trim() + "' \n");
				}		
				if(slotOrganizationModel.SearchCategoryName.Trim().Length > 0 && !slotOrganizationModel.SearchCategoryName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.CategoryCode = '" + slotOrganizationModel.SearchCategoryName.Trim() + "' \n");
				}		

				sbQuery.Append(" ORDER BY C.CategoryCode,D.GenreCode \n");

				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				slotOrganizationModel.GenreDataSet = ds.Copy();
				// 결과
				slotOrganizationModel.ResultCnt = Utility.GetDatasetCount(slotOrganizationModel.GenreDataSet);
				// 결과코드 셋트
				slotOrganizationModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + slotOrganizationModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				slotOrganizationModel.ResultCD = "3000";
				slotOrganizationModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		/// <summary>
		/// 사용자목록조회
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		public void GetSlotList(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSlotList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + slotOrganizationModel.SearchKey       + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT	    a.MediaCode							 \n"
					+ "			   ,c.MediaName							 \n"
					+ "			   ,a.CategoryCode						 \n"
					+ "			   ,CASE(SPACE(5 - LEN(CONVERT(VARCHAR(10),CASE a.CategoryCode WHEN 0 THEN '0' ELSE a.CategoryCode END))) +  CONVERT(VARCHAR(10),CASE a.CategoryCode WHEN 0 THEN '0' ELSE a.CategoryCode END))WHEN 0 THEN '전체' ELSE (SPACE(5 - LEN(CONVERT(VARCHAR(10),a.CategoryCode))) +  CONVERT(VARCHAR(10),a.CategoryCode) + ' ' + e.CategoryName) END AS CategoryName\n"	
					+ "			   ,a.GenreCode							 \n"
					//+ "			   ,d.GenreName							 \n"
					+ "			   ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.GenreCode)))    +  CONVERT(VARCHAR(10),a.GenreCode)    + ' ' + d.GenreName   ) AS GenreName    		        \n"
					+ "			   ,a.ChannelNo							 \n"										
					+ "			   ,b.Title							 \n"										
					+ " 		   ,a.Slot1								 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot1) AS SlotName1								 \n"
					+ " 		   ,a.Slot2								 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot2) AS SlotName2								 \n"
					+ " 		   ,a.Slot3								 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot3) AS SlotName3								 \n"
					+ " 		   ,a.Slot4								 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot4) AS SlotName4								 \n"
					+ " 		   ,a.Slot5								 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot5) AS SlotName5								 \n"
					+ " 		   ,a.Slot6								 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot6) AS SlotName6								 \n"
					+ " 		   ,a.Slot7								 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot7) AS SlotName7								 \n"
					+ " 		   ,a.Slot8								 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot8) AS SlotName8								 \n"
					+ " 		   ,a.Slot9								 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot9) AS SlotName9								 \n"
					+ " 		   ,a.Slot10							 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot10) AS SlotName10								 \n"
					+ " 		   ,a.Slot11							 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot11) AS SlotName11								 \n"
					+ " 		   ,a.Slot12							 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot12) AS SlotName12								 \n"
					+ " 		   ,a.Slot13							 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot13) AS SlotName13								 \n"
					+ " 		   ,a.Slot14							 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot14) AS SlotName14								 \n"
					+ " 		   ,a.Slot15							 \n"
					+ " 		   ,(SELECT SlotName FROM SlotCode WHERE SlotCode = a.Slot15) AS SlotName15								 \n"
					+ " 		   ,a.UseYn								 \n"
					+ "		       ,CASE a.UseYn  WHEN 'Y'  THEN '사용' WHEN 'N' THEN '사용안함' END UseName \n"
					+ " 		   ,convert(char(10), a.RegDt, 120) AS RegDt		 \n"
					+ " 		   ,convert(char(10), a.ModDt, 120) AS ModDt		 \n"
					+ "			   ,f.UserName AS RegName                           \n"					
					+ "   FROM SlotOrganization a LEFT JOIN Channel b            \n"
					+ "			ON a.MediaCode = b.MediaCode            		 \n"
					+ "			and a.ChannelNo = b.ChannelNo            		 \n"
					+ "   LEFT JOIN Media c            							 \n"
					+ "			ON a.MediaCode = c.MediaCode            		 \n"
					+ "   LEFT JOIN Genre d            							 \n"
					+ "			ON a.GenreCode = d.GenreCode            		 \n"	
					+ "   LEFT JOIN Category e            						 \n"
					+ "			ON a.CategoryCode = e.CategoryCode            	 \n"
					+ "   LEFT JOIN SystemUser f            					 \n"
					+ "			ON a.RegID = f.UserId            				 \n"
					);								

				// 채널셋레벨을 선택했으면
				if(slotOrganizationModel.SearchMediaName.Trim().Length > 0 && !slotOrganizationModel.SearchMediaName.Trim().Equals("00"))
				{
					sbQuery.Append(" WHERE A.MediaCode = '" + slotOrganizationModel.SearchMediaName.Trim() + "' \n");
				}		
				if(slotOrganizationModel.SearchCategoryName.Trim().Length > 0 && !slotOrganizationModel.SearchCategoryName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.CategoryCode = '" + slotOrganizationModel.SearchCategoryName.Trim() + "' \n");
				}		
				if(slotOrganizationModel.SearchGenreName.Trim().Length > 0 && !slotOrganizationModel.SearchGenreName.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.GenreCode = '" + slotOrganizationModel.SearchGenreName.Trim() + "' \n");
				}		
				if(slotOrganizationModel.SearchchkUseYn.Trim().Length > 0 && slotOrganizationModel.SearchchkUseYn.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND A.UseYn = 'Y' OR A.UseYn = 'N' OR A.UseYn = NULL \n");
				}	
				if(slotOrganizationModel.SearchchkUseYn.Trim().Length > 0 && slotOrganizationModel.SearchchkUseYn.Trim().Equals("N"))
				{
					sbQuery.Append(" AND  A.UseYn  = 'Y' \n");					
				}	
								
				sbQuery.Append("\n  GROUP BY a.ChannelNo,b.Title,convert(char(10), a.ModDt, 120),a.MediaCode,c.MediaName,a.CategoryCode\n"
											 + ",CategoryName,a.GenreCode,d.GenreName,genreName,a.Slot1,a.Slot2,a.Slot3,a.Slot4,a.Slot5\n"
											 + ",a.Slot6,a.Slot7,a.Slot8,a.Slot9,a.Slot10,a.Slot11,a.Slot12,a.Slot13,a.Slot14,a.Slot15 \n"
											 + ",a.UseYn, a.RegDt, a.ModDt, f.UserName");

				sbQuery.Append("\n ORDER BY A.MediaCode, A.CategoryCode, A.GenreCode, A.ChannelNo");

				// __DEBUG__
				_log.Debug("MediaCode:[" + slotOrganizationModel.SearchMediaName + "]");
				_log.Debug("CategoryCode:[" + slotOrganizationModel.SearchCategoryName + "]");
				_log.Debug("GenreCode:[" + slotOrganizationModel.SearchGenreName + "]");
				
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 사용자모델에 복사
				slotOrganizationModel.SlotDataSet = ds.Copy();
				// 결과
				slotOrganizationModel.ResultCnt = Utility.GetDatasetCount(slotOrganizationModel.SlotDataSet);
				// 결과코드 셋트
				slotOrganizationModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + slotOrganizationModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSlotList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				slotOrganizationModel.ResultCD = "3000";
				slotOrganizationModel.ResultDesc = "사용자정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}

		}

		/// <summary>
		///  코드목록조회
		/// </summary>
		/// <param name="slotOrganizationModel"></param>
		public void GetSlotCodeList(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSlotCodeList() Start");
				_log.Debug("-----------------------------------------");

				// 데이트베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT SlotCode, SlotName  \n"
					+ "   FROM SlotCode            \n"										
					);

				// 코드분류가 선택했으면
				
				sbQuery.Append(" ORDER BY SlotCode \n");
				
				_log.Debug(sbQuery.ToString());			
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				slotOrganizationModel.SlotCodeDataSet = ds.Copy();
				// 결과
				slotOrganizationModel.ResultCnt = Utility.GetDatasetCount(slotOrganizationModel.SlotCodeDataSet);
				// 결과코드 셋트
				slotOrganizationModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + slotOrganizationModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSlotCodeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				slotOrganizationModel.ResultCD = "3000";
				slotOrganizationModel.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
				
				// 데이트베이스를  Close한다
				_db.Close();

			}

			// 데이트베이스를  Close한다
			_db.Close();

		}


		/// <summary>
		/// 사용자정보 저장
		/// </summary>
		/// <param name="UserID"></param>
		/// <param name="UserName"></param>
		/// <param name="UserPassword"></param>
		/// <param name="UserLevel"></param>
		/// <param name="UserDept"></param>
		/// <param name="UserTitle"></param>
		/// <param name="UserTell"></param>
		/// <param name="UserMobile"></param>
		/// <param name="UserComment"></param>
		/// <returns></returns>
		public void SetSlotUpdate(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSlotUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("MediaCode      :[" + slotOrganizationModel.MediaCode       + "]");				
				_log.Debug("CategoryCode      :[" + slotOrganizationModel.CategoryCode       + "]");				
				_log.Debug("GenreCode      :[" + slotOrganizationModel.GenreCode       + "]");				
				_log.Debug("ChannelNo      :[" + slotOrganizationModel.ChannelNo       + "]");	
	
				_log.Debug("Slot1      :[" + slotOrganizationModel.Slot1       + "]");				
				_log.Debug("Slot2      :[" + slotOrganizationModel.Slot2       + "]");				
				_log.Debug("Slot3      :[" + slotOrganizationModel.Slot3       + "]");				
				_log.Debug("Slot4      :[" + slotOrganizationModel.Slot4       + "]");	
				_log.Debug("Slot5      :[" + slotOrganizationModel.Slot5       + "]");				
				_log.Debug("Slot6      :[" + slotOrganizationModel.Slot6       + "]");				
				_log.Debug("Slot7      :[" + slotOrganizationModel.Slot7       + "]");				
				_log.Debug("Slot8      :[" + slotOrganizationModel.Slot8       + "]");	
				_log.Debug("Slot9      :[" + slotOrganizationModel.Slot9       + "]");				
				_log.Debug("Slot10      :[" + slotOrganizationModel.Slot10       + "]");				
				_log.Debug("Slot11      :[" + slotOrganizationModel.Slot11       + "]");				
				_log.Debug("Slot12      :[" + slotOrganizationModel.Slot12       + "]");	
				_log.Debug("Slot13      :[" + slotOrganizationModel.Slot13       + "]");				
				_log.Debug("Slot14      :[" + slotOrganizationModel.Slot14       + "]");				
				_log.Debug("Slot15      :[" + slotOrganizationModel.Slot15       + "]");				
				

		
				_log.Debug("MediaCode_old      :[" + slotOrganizationModel.MediaCode_old       + "]");				
				_log.Debug("CategoryCode_old      :[" + slotOrganizationModel.CategoryCode_old       + "]");				
				_log.Debug("GenreCode_old      :[" + slotOrganizationModel.GenreCode_old       + "]");				
				_log.Debug("ChannelNo_old      :[" + slotOrganizationModel.ChannelNo_old       + "]");		
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[25];

				sbQuery.Append(""
					+ "UPDATE SlotOrganization               \n"
					+ "   SET MediaCode     = @MediaCode     \n"
					+ "      ,CategoryCode  = @CategoryCode  \n" 
					+ "      ,GenreCode     = @GenreCode     \n"
					+ "      ,ChannelNo     = @ChannelNo     \n"
					+ "      ,Slot1			= @Slot1         \n"
					+ "      ,Slot2			= @Slot2         \n"
					+ "      ,Slot3			= @Slot3		 \n"
					+ "      ,Slot4			= @Slot4         \n"
					+ "      ,Slot5			= @Slot5         \n"
					+ "      ,Slot6			= @Slot6         \n"
					+ "      ,Slot7			= @Slot7         \n"
					+ "      ,Slot8			= @Slot8         \n"
					+ "      ,Slot9			= @Slot9		 \n"
					+ "      ,Slot10		= @Slot10   \n"
					+ "      ,Slot11		= @Slot11   \n"
					+ "      ,Slot12		= @Slot12   \n"
					+ "      ,Slot13		= @Slot13   \n"
					+ "      ,Slot14		= @Slot14   \n"
					+ "      ,Slot15		= @Slot15   \n"
					+ "      ,UseYn         = @UseYn         \n"
					+ "      ,ModDt         = GETDATE()      \n"
					+ "      ,RegID         = @RegID         \n"
					+ " WHERE MediaCode     = @MediaCode_old        \n"
					+ "   AND CategoryCode  = @CategoryCode_old     \n"
					+ "   AND GenreCode     = @GenreCode_old        \n"
					+ "   AND ChannelNo     = @ChannelNo_old        \n"
					);

				i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@CategoryCode" , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@GenreCode"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int);

				sqlParams[i++] = new SqlParameter("@Slot1"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot2"      , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot3"   , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot4"     , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot5"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot6"     , SqlDbType.Int);				
				sqlParams[i++] = new SqlParameter("@Slot7"   , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot8"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot9"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot10"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot11"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot12"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot13"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot14"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot15"  , SqlDbType.Int);

				sqlParams[i++] = new SqlParameter("@UseYn"        , SqlDbType.Char , 1);
				sqlParams[i++] = new SqlParameter("@RegID"        , SqlDbType.VarChar , 50);

				sqlParams[i++] = new SqlParameter("@MediaCode_old"    , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@CategoryCode_old" , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@GenreCode_old"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@ChannelNo_old"    , SqlDbType.Int);
				
				i = 0;				
				if(slotOrganizationModel.MediaCode.Equals(null))	// 매체코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.MediaCode);
				}
				if(slotOrganizationModel.CategoryCode.Equals(null))		// 미디어렙코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.CategoryCode);
				}
				if(slotOrganizationModel.GenreCode.Equals(null))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.GenreCode);
				}
				if(slotOrganizationModel.ChannelNo.Equals(null))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.ChannelNo);
				}				
				if(slotOrganizationModel.Slot1.Equals(null) || slotOrganizationModel.Slot1.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot1);
				}				
				if(slotOrganizationModel.Slot2.Equals(null) || slotOrganizationModel.Slot2.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot2);
				}				
				if(slotOrganizationModel.Slot3.Equals(null) || slotOrganizationModel.Slot3.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot3);
				}
				if(slotOrganizationModel.Slot4.Equals(null) || slotOrganizationModel.Slot4.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot4);
				}
				if(slotOrganizationModel.Slot5.Equals(null) || slotOrganizationModel.Slot5.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot5);
				}
				if(slotOrganizationModel.Slot6.Equals(null) || slotOrganizationModel.Slot6.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot6);
				}
				if(slotOrganizationModel.Slot7.Equals(null) || slotOrganizationModel.Slot7.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot7);
				}
				if(slotOrganizationModel.Slot8.Equals(null) || slotOrganizationModel.Slot8.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot8);
				}
				if(slotOrganizationModel.Slot9.Equals(null) || slotOrganizationModel.Slot9.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot9);
				}
				if(slotOrganizationModel.Slot10.Equals(null) || slotOrganizationModel.Slot10.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot10);
				}
				if(slotOrganizationModel.Slot11.Equals(null) || slotOrganizationModel.Slot11.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot11);
				}
				if(slotOrganizationModel.Slot12.Equals(null) || slotOrganizationModel.Slot12.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot12);
				}
				if(slotOrganizationModel.Slot13.Equals(null) || slotOrganizationModel.Slot13.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot13);
				}
				if(slotOrganizationModel.Slot14.Equals(null) || slotOrganizationModel.Slot14.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot14);
				}
				if(slotOrganizationModel.Slot15.Equals(null) || slotOrganizationModel.Slot15.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot15);
				}				
			
				sqlParams[i++].Value = slotOrganizationModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // 등록자

				if(slotOrganizationModel.MediaCode_old.Equals(null) || slotOrganizationModel.MediaCode_old.Equals(""))	// 매체코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.MediaCode_old);
				}
				if(slotOrganizationModel.CategoryCode_old.Equals(null) || slotOrganizationModel.CategoryCode_old.Equals(""))		// 미디어렙코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.CategoryCode_old);
				}
				if(slotOrganizationModel.GenreCode_old.Equals(null) || slotOrganizationModel.GenreCode_old.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.GenreCode_old);
				}
				if(slotOrganizationModel.ChannelNo_old.Equals(null) || slotOrganizationModel.ChannelNo_old.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.ChannelNo_old);
				}
				
				_log.Debug("UserID:[" + slotOrganizationModel.MediaCode_old + "]");			
				_log.Debug("UserID:[" + slotOrganizationModel.CategoryCode_old + "]");			
				_log.Debug("UserID:[" + slotOrganizationModel.GenreCode_old + "]");			
				_log.Debug("UserID:[" + slotOrganizationModel.ChannelNo_old + "]");			

				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				slotOrganizationModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSlotUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				slotOrganizationModel.ResultCD   = "3201";
				slotOrganizationModel.ResultDesc = "사용자정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		/// <summary>
		/// 사용자 생성
		/// </summary>
		/// <param name="UserID"></param>
		/// <param name="UserName"></param>
		/// <param name="UserPassword"></param>
		/// <param name="UserLevel"></param>
		/// <param name="UserDept"></param>
		/// <param name="UserTitle"></param>
		/// <param name="UserTell"></param>
		/// <param name="UserMobile"></param>
		/// <param name="UserComment"></param>
		/// <returns></returns>
		public void SetSlotCreate(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSlotCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[20];

				sbQuery.Append( ""
					+ "INSERT INTO SlotOrganization (                         \n"
					+ "       MediaCode ,CategoryCode ,GenreCode ,ChannelNo, Slot1 \n"
					+ "      ,Slot2 ,Slot3 ,Slot4 \n"
					+ "      ,Slot5 ,Slot6 ,Slot7 ,Slot8, Slot9 \n"
					+ "      ,Slot10 ,Slot11 ,Slot12 ,Slot13 ,Slot14 ,Slot15 ,UseYn ,RegDt         \n"
					+ "      ,RegID                                     \n"
					+ "      )                                          \n"
					+ " VALUES (                                        \n"
					+ "       @MediaCode        \n"
					+ "      ,@CategoryCode      \n"
					+ "      ,@GenreCode  \n" 
					+ "      ,@ChannelNo     \n"
					+ "      ,@Slot1     \n"
					+ "      ,@Slot2     \n"
					+ "      ,@Slot3       \n"
					+ "      ,@Slot4    \n"
					+ "      ,@Slot5      \n"
					+ "      ,@Slot6     \n"
					+ "      ,@Slot7      \n"
					+ "      ,@Slot8    \n"
					+ "      ,@Slot9     \n"
					+ "      ,@Slot10   \n"
					+ "      ,@Slot11   \n"
					+ "      ,@Slot12   \n"
					+ "      ,@Slot13   \n"
					+ "      ,@Slot14   \n"
					+ "      ,@Slot15   \n"
					+ "      ,'Y'            \n"
					+ "      ,GETDATE()      \n"					
					+ "      ,@RegID         \n"
					+ "      )               \n"
					);

				i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@CategoryCode" , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@GenreCode"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int);

				sqlParams[i++] = new SqlParameter("@Slot1"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot2"      , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot3"   , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot4"     , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot5"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot6"     , SqlDbType.Int);				
				sqlParams[i++] = new SqlParameter("@Slot7"   , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot8"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot9"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot10"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot11"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot12"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot13"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot14"  , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Slot15"  , SqlDbType.Int);
				
				sqlParams[i++] = new SqlParameter("@RegID"        , SqlDbType.VarChar , 50);
				
				i = 0;				
				if(slotOrganizationModel.MediaCode.Equals(null) || slotOrganizationModel.MediaCode.Equals(""))	// 매체코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.MediaCode);
				}
				if(slotOrganizationModel.CategoryCode.Equals(null) || slotOrganizationModel.CategoryCode.Equals(""))		// 미디어렙코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.CategoryCode);
				}
				if(slotOrganizationModel.GenreCode.Equals(null) || slotOrganizationModel.GenreCode.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.GenreCode);
				}
				if(slotOrganizationModel.ChannelNo.Equals(null) || slotOrganizationModel.ChannelNo.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.ChannelNo);
				}				
				if(slotOrganizationModel.Slot1.Equals(null) || slotOrganizationModel.Slot1.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot1);
				}				
				if(slotOrganizationModel.Slot2.Equals(null) || slotOrganizationModel.Slot2.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot2);
				}				
				if(slotOrganizationModel.Slot3.Equals(null) || slotOrganizationModel.Slot3.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot3);
				}
				if(slotOrganizationModel.Slot4.Equals(null) || slotOrganizationModel.Slot4.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot4);
				}
				if(slotOrganizationModel.Slot5.Equals(null) || slotOrganizationModel.Slot5.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot5);
				}
				if(slotOrganizationModel.Slot6.Equals(null) || slotOrganizationModel.Slot6.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot6);
				}
				if(slotOrganizationModel.Slot7.Equals(null) || slotOrganizationModel.Slot7.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot7);
				}
				if(slotOrganizationModel.Slot8.Equals(null) || slotOrganizationModel.Slot8.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot8);
				}
				if(slotOrganizationModel.Slot9.Equals(null) || slotOrganizationModel.Slot9.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot9);
				}
				if(slotOrganizationModel.Slot10.Equals(null) || slotOrganizationModel.Slot10.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot10);
				}
				if(slotOrganizationModel.Slot11.Equals(null) || slotOrganizationModel.Slot11.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot11);
				}
				if(slotOrganizationModel.Slot12.Equals(null) || slotOrganizationModel.Slot12.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot12);
				}
				if(slotOrganizationModel.Slot13.Equals(null) || slotOrganizationModel.Slot13.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot13);
				}
				if(slotOrganizationModel.Slot14.Equals(null) || slotOrganizationModel.Slot14.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot14);
				}
				if(slotOrganizationModel.Slot15.Equals(null) || slotOrganizationModel.Slot15.Equals(""))	// 대행사코드가 0이면 NULL로 셋트한다.
				{
					sqlParams[i++].Value = 0;
				}
				else
				{
					sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.Slot15);
				}				
						
				sqlParams[i++].Value = header.UserID;      // 등록자
				
				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				slotOrganizationModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSlotCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				slotOrganizationModel.ResultCD   = "3101";
				slotOrganizationModel.ResultDesc = "사용자정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}

		}


		public void SetSlotDelete(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSlotDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
				SqlParameter[] sqlParams = new SqlParameter[4];
				
				sbQuery.Append(""
					+ "DELETE SlotOrganization         \n"
					+ " WHERE MediaCode  = @MediaCode  \n"
					+ "   AND CategoryCode  = @CategoryCode  \n"
					+ "   AND GenreCode  = @GenreCode  \n"
					+ "   AND ChannelNo  = @ChannelNo  \n"
					);

				sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@CategoryCode" , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@GenreCode"    , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int);

				i = 0;				
				sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.MediaCode);
				sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.CategoryCode);
				sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.GenreCode);
				sqlParams[i++].Value = Convert.ToInt32(slotOrganizationModel.ChannelNo);

				_log.Debug("MediaCode:[" + slotOrganizationModel.MediaCode + "]");			
				_log.Debug("CategoryCode:[" + slotOrganizationModel.CategoryCode + "]");			
				_log.Debug("GenreCode:[" + slotOrganizationModel.GenreCode + "]");			
				_log.Debug("ChannelNo:[" + slotOrganizationModel.ChannelNo + "]");			

				_log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;

				}

				slotOrganizationModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetSlotDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				slotOrganizationModel.ResultCD   = "3301";
				slotOrganizationModel.ResultDesc = "슬롯정보 삭제중 오류가 발생하였습니다";
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
