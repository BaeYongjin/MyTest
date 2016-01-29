using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Net;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Interface
{
	/// <summary>
	/// RequestAdPopContentListBiz에 대한 요약 설명입니다.
	/// </summary>
	public class RequestAdPopContentListBiz : BaseBiz
	{
		public RequestAdPopContentListBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

        #region [ 채널점핑 등록 ]
		/// <summary>
		/// 점핑등록
		/// </summary>
		/// <returns></returns>
		public void SetChannelJumpCreate(AdPopModel adpopModel)
		{

			int NameCnt = 0;
			//int KeyChannelNo = 0;
//			bool isAdPop = false;

			try
			{  				
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelJumpCreate() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("ItemNo     :[" + adpopModel.ItemNo     + "]");				
				_log.Debug("MediaCode  :[" + adpopModel.MediaCode  + "]");
				_log.Debug("JumpType   :[" + adpopModel.JumpType   + "]");
				_log.Debug("GenreCode1  :[" + adpopModel.GenreCode1  + "]");
				_log.Debug("GenreCode2  :[" + adpopModel.GenreCode2  + "]");
				_log.Debug("GenreCode3  :[" + adpopModel.GenreCode3  + "]");
				_log.Debug("GenreCode4  :[" + adpopModel.GenreCode4  + "]");
				_log.Debug("GenreCode5  :[" + adpopModel.GenreCode5  + "]");
				_log.Debug("ContentID  :[" + adpopModel.ContentID  + "]");
				_log.Debug("ContentID1  :[" + adpopModel.ContentID1  + "]");
				_log.Debug("ContentID2  :[" + adpopModel.ContentID2  + "]");
				_log.Debug("ContentID3  :[" + adpopModel.ContentID3  + "]");
				_log.Debug("ContentID4  :[" + adpopModel.ContentID4  + "]");
				_log.Debug("ContentID5  :[" + adpopModel.ContentID5  + "]");
				_log.Debug("AdPopID    :[" + adpopModel.AdPopID    + "]");				
				_log.Debug("HomeYn     :[" + adpopModel.HomeYn     + "]");
				_log.Debug("ChannelYn  :[" + adpopModel.ChannelYn  + "]");
       
				// 동일한 광고내역명이 있는지 검사한다.
				StringBuilder sbQueryItemName = new StringBuilder();
				sbQueryItemName.Append("\n"
					+ " SELECT COUNT(*) FROM ChannelJump  with(NoLock)  \n"
					+ "  WHERE ItemNo    = " + adpopModel.ItemNo    + " \n"
					+ "    AND MediaCode = " + adpopModel.MediaCode + " \n"
					);

				// __DEBUG__
				_log.Debug(sbQueryItemName.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQueryItemName.ToString());


				NameCnt = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

				if(NameCnt > 0)
				{
					throw new Exception();
				}				

				//AdGroup Insert Query
				StringBuilder sbQuery = new StringBuilder();
                                                                 
				sbQuery.Append("\n"
					+ "INSERT INTO ChannelJump		\n"
					+ "           (ItemNo			\n"
					+ "           ,MediaCode		\n"
					+ "           ,JumpType			\n"
					+ "           ,GenreCode		\n"
					+ "           ,ChannelNo		\n"
					+ "           ,ContentID		\n"
					+ "           ,PopupID          \n"
					+ "           ,PopupTitle       \n"
					+ "           ,HomeYn		    \n"
					+ "           ,ChannelYn   		\n"
					+ "           ,ModDt			\n"
					+ "           )                 \n"
					+ "     VALUES(					\n"
					+ "            @ItemNo			\n"
					+ "           ,@MediaCode		\n"
					+ "           ,@JumpType		\n"
					+ "           ,null         	\n"
					+ "           ,null     		\n"
					+ "           ,null             \n"
					+ "           ,@AdPopID         \n"
					+ "           ,''		        \n"
					+ "           ,'Y'				\n"
					+ "           ,'N'				\n"
					+ "           ,GETDATE()		\n"
					+ "           )                 \n"
					);
	
                                                    
				// 쿼리실행
				try
				{				
					int i = 0;
					int rc = 0;
					//광고 그룹 Insert
					SqlParameter[] sqlParams = new SqlParameter[6];
                                
					i = 0;
					sqlParams[i++] = new SqlParameter("@ItemNo"		   , SqlDbType.Int             );
					sqlParams[i++] = new SqlParameter("@MediaCode"	   , SqlDbType.TinyInt         );
					sqlParams[i++] = new SqlParameter("@JumpType"	   , SqlDbType.TinyInt         );
					sqlParams[i++] = new SqlParameter("@AdPopID"	   , SqlDbType.VarChar ,    8);
                                                                                         
					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(adpopModel.ItemNo)   ;
					sqlParams[i++].Value = Convert.ToInt16(adpopModel.MediaCode);
					sqlParams[i++].Value = Convert.ToInt16(adpopModel.JumpType) ;
					
					if(adpopModel.AdPopID.Equals(""))
					{
						sqlParams[i++].Value = "";
					}
					else
					{
						sqlParams[i++].Value = adpopModel.AdPopID;
					}
                                   
					// __DEBUG__
					_log.Debug(sbQuery.ToString());
					// __DEBUG__
                                
					_db.BeginTran();
					
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                   
					_db.CommitTran();

					
					//if(isAdPop) SetAdPop(adpopModel.AdPopID);


					// __MESSAGE__
					_log.Message("채널점핑생성:["+ adpopModel.ItemNo + "]["+ adpopModel.ContentID + "]");
            
            
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                                                    
				adpopModel.ResultCD = "0000";  // 정상
                                                    
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelJumpCreate() End");
				_log.Debug("-----------------------------------------");	
                                        
			}
			catch(Exception ex)
			{
				adpopModel.ResultCD   = "3101";

				if(NameCnt > 0)
				{
					adpopModel.ResultDesc = "해당광고는 이미 존재합니다.";
				}
				else
				{
					adpopModel.ResultDesc = "채널점핑정보 생성 중 오류가 발생하였습니다";
					_log.Exception(ex);
				}
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

        #endregion

		/// <summary>
		/// 연결채널 생성
		/// </summary>
		/// <param name="header"></param>
		/// <param name="adpopModel"></param>
		/// 
		public void SetLinkChannelCreate(AdPopModel adpopModel)
		{
			int KeyChannelNo = 0;
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetLinkChannelCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// 동일한 광고내역명이 있는지 검사한다.
				StringBuilder sbQueryChannelNo = new StringBuilder();
				sbQueryChannelNo.Append("\n"
					+ " SELECT ChannelNo FROM Channel  with(NoLock)  \n"
					+ "  WHERE ContentID    = '" + new Guid(adpopModel.ContentID)    + "' \n"
					+ "    AND MediaCode = " + adpopModel.MediaCode + " \n"
					);

				// __DEBUG__
				_log.Debug(sbQueryChannelNo.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds1 = new DataSet();
				_db.ExecuteQuery(ds1,sbQueryChannelNo.ToString());


				KeyChannelNo = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString());

				_log.Debug("KeyChannelNo  :[" + KeyChannelNo  + "]");
                
				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQuery2 = new StringBuilder();
				             			

				sbQuery.Append( ""
					+ "        DELETE FROM  LinkChannel			        \n"
					+ "              WHERE ItemNo = @ItemNo1			\n"					
					+ "              AND Channel = '"+KeyChannelNo+"'   \n"					
					);                                                        
				sbQuery2.Append( ""
					+ "INSERT INTO LinkChannel (\n"
					+ "       ItemNo			\n"					
					+ "      ,Channel			\n"					
					+ "      )					\n"
					+ " VALUES(					\n"					
					+ "       @ItemNo			\n"
					+ "      ,'"+KeyChannelNo+"'		\n"						
					+ " )						\n"				
					
					);				
				
				// 쿼리실행
				try
				{
					int i = 0;
					int rc = 0;
					SqlParameter[] sqlParams = new SqlParameter[1]; 
					_db.BeginTran();

					i = 0;			
					sqlParams[i++] = new SqlParameter("@ItemNo1"  , SqlDbType.Int);		
							
					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(adpopModel.ItemNo);	

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Debug("ItemNo      :[" + adpopModel.ItemNo       + "]");
					_log.Debug(sbQuery.ToString());



					sqlParams = new SqlParameter[1];	
					i = 0;
					sqlParams[i++] = new SqlParameter("@ItemNo"  , SqlDbType.Int);		

					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(adpopModel.ItemNo);				

					rc =  _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams);
					_log.Debug("ItemNo      :[" + adpopModel.ItemNo       + "]");
					_log.Debug("KeyChannelNo     :[" + KeyChannelNo      + "]");
					_log.Debug(sbQuery2.ToString());
					
					
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("연결채널정보생성:[" + adpopModel.ItemNo + "(" + KeyChannelNo + ")]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adpopModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetLinkChannelCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				adpopModel.ResultCD   = "3101";
				adpopModel.ResultDesc = "연결채널정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

		// 채널점핑정보 수정
		public void SetChannelJumpUpdate(AdPopModel adpopModel)
		{

//			bool isAdPop = false;
			int KeyChannelNo = 0;

			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelJumpUpdate() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("ItemNo     :[" + adpopModel.ItemNo     + "]");				
				_log.Debug("MediaCode  :[" + adpopModel.MediaCode  + "]");
				_log.Debug("JumpType   :[" + adpopModel.JumpType   + "]");
				_log.Debug("GenreCode  :[" + adpopModel.GenreCode1  + "]");
				_log.Debug("ChannelNo  :[" + adpopModel.ChannelNo  + "]");
				_log.Debug("ContentID  :[" + adpopModel.ContentID + "]");
				_log.Debug("PopupID    :[" + adpopModel.AdPopID    + "]");
				_log.Debug("PopupTitle :[" + adpopModel.PopupTitle + "]");
				_log.Debug("HomeYn     :[" + adpopModel.HomeYn     + "]");
				_log.Debug("ChannelYn  :[" + adpopModel.ChannelYn  + "]");

				// 동일한 광고내역명이 있는지 검사한다.
				StringBuilder sbQueryChannelNo = new StringBuilder();
				sbQueryChannelNo.Append("\n"
					+ " SELECT ChannelNo FROM Channel  with(NoLock)  \n"
					+ "  WHERE ContentID    = '" + new Guid(adpopModel.ContentID)    + "' \n"
					+ "    AND MediaCode = " + adpopModel.MediaCode + " \n"
					);

				// __DEBUG__
				_log.Debug(sbQueryChannelNo.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds1 = new DataSet();
				_db.ExecuteQuery(ds1,sbQueryChannelNo.ToString());


				KeyChannelNo = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString());

				_log.Debug("KeyChannelNo  :[" + KeyChannelNo  + "]");
                        

				// 광고내역을 업데이트한다.
				StringBuilder sbQuery = new StringBuilder();
                        
				int i = 0;
				int rc = 0;
                
                        
				sbQuery.Append("\n"
					+ "UPDATE ChannelJump              \n"
					+ "   SET JumpType	 = @JumpType   \n"
					+ "      ,PopupID    = @AdPopID    \n"
					+ "      ,PopupTitle = @PopupTitle \n"
					+ "      ,ModDt      = GETDATE()   \n"   
					+ " WHERE ItemNo     = @ItemNo     \n"
					+ "   AND MediaCode  = @MediaCode  \n"
					);

				// 쿼리실행
				try
				{
					SqlParameter[] sqlParams = new SqlParameter[10];

					i = 0;
					sqlParams[i++] = new SqlParameter("@ItemNo"		   , SqlDbType.Int             );
					sqlParams[i++] = new SqlParameter("@MediaCode"	   , SqlDbType.TinyInt         );
					sqlParams[i++] = new SqlParameter("@JumpType"	   , SqlDbType.TinyInt         );
					sqlParams[i++] = new SqlParameter("@AdPopID"	   , SqlDbType.VarChar ,    8);
					sqlParams[i++] = new SqlParameter("@PopupTitle"	   , SqlDbType.VarChar ,    100);
                                                                                         
					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(adpopModel.ItemNo)   ;
					sqlParams[i++].Value = Convert.ToInt16(adpopModel.MediaCode);
					sqlParams[i++].Value = Convert.ToInt16(adpopModel.JumpType) ;
					if(adpopModel.AdPopID.Equals(""))
					{
						sqlParams[i++].Value = "";
					}
					else
					{
						sqlParams[i++].Value = adpopModel.AdPopID;
					}					
					sqlParams[i++].Value = adpopModel.PopupTitle;

					_db.BeginTran();

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					_db.CommitTran();

//					if(isAdPop) SetAdPop(adpopModel.AdPopID);
                        
					//__MESSAGE__
					_log.Message("채널점핑정보수정:["+adpopModel.ItemNo+ "]["+ adpopModel.AdPopID + "]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				adpopModel.ResultCD = "0000";  // 정상
                        
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelJumpUpdate() End");
				_log.Debug("-----------------------------------------");
         
			}
			catch(Exception ex)
			{
				adpopModel.ResultCD   = "3201";
				adpopModel.ResultDesc = "채널점핑정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}
		
		public void SetChannelJumpDelete(AdPopModel adpopModel)
		{
            try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelJumpDelete() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("ItemNo     :[" + adpopModel.ItemNo    + "]");				
				_log.Debug("MediaCode  :[" + adpopModel.MediaCode + "]");
                        
				StringBuilder sbQuery                       = new StringBuilder();
               
                       
				// 쿼리실행
				try
				{
					int rc = 0;
					int i = 0;

                        
					sbQuery.Append("\n"
						+ "DELETE ChannelJump             \n"
						+ " WHERE ItemNo    = @ItemNo     \n"
						+ "   AND MediaCode = @MediaCode  \n"
						);

					SqlParameter[] sqlParams = new SqlParameter[2];
                    
                     
					i = 0;
					sqlParams[i++] = new SqlParameter("@ItemNo"		   , SqlDbType.Int             );
					sqlParams[i++] = new SqlParameter("@MediaCode"	   , SqlDbType.TinyInt         );
                                                                                         
					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(adpopModel.ItemNo)   ;
					sqlParams[i++].Value = Convert.ToInt16(adpopModel.MediaCode);

         
					_db.BeginTran();
          

					//광고 계약 정보 삭제 한다.
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					// __DEBUG__
					_log.Debug(sbQuery.ToString());
					// __DEBUG__
                        

					_db.CommitTran();

					//__MESSAGE__
					_log.Message("채널점핑정보삭제:["+adpopModel.ItemNo+ "]["+ adpopModel.AdPopID + "]");
                        
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
                        
				adpopModel.ResultCD = "0000";  // 정상
                        
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetChannelJumpDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				adpopModel.ResultCD   = "3101";
				_log.Exception(ex);
				adpopModel.ResultDesc = "채널점핑 정보 삭제 중 오류가 발생하였습니다";
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}

		/// <summary>
		/// 연결채널 삭제(해당광고 해당채널삭제)
		/// </summary>
		/// <param name="header"></param>
		/// <param name="adpopModel"></param>
		public void SetLinkChannelDelete(AdPopModel adpopModel)
		{

			int KeyChannelNo = 0;
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetLinkChannelDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// 동일한 광고내역명이 있는지 검사한다.
				StringBuilder sbQueryChannelNo = new StringBuilder();
				sbQueryChannelNo.Append("\n"
					+ " SELECT ChannelNo FROM Channel  with(NoLock)  \n"
					+ "  WHERE ContentID    = '" + adpopModel.ContentID1    + "' \n"
					+ "    AND MediaCode = " + adpopModel.MediaCode + " \n"
					);

				// __DEBUG__
				_log.Debug(sbQueryChannelNo.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds1 = new DataSet();
				_db.ExecuteQuery(ds1,sbQueryChannelNo.ToString());


				KeyChannelNo = Convert.ToInt32(ds1.Tables[0].Rows[0][0].ToString());

				_log.Debug("KeyChannelNo  :[" + KeyChannelNo  + "]");
                
				StringBuilder sbQuery = new StringBuilder();			             			

				sbQuery.Append( ""
					+ "        DELETE FROM  LinkChannel			        \n"
					+ "              WHERE ItemNo = @ItemNo				\n"					
					+ "              AND Channel = '"+KeyChannelNo+"'	\n"					
					);                                                       
				
				
				// 쿼리실행
				try
				{
					int i = 0;
					int rc = 0;
					SqlParameter[] sqlParams = new SqlParameter[1]; 
					_db.BeginTran();

					i = 0;			
					sqlParams[i++] = new SqlParameter("@ItemNo"  , SqlDbType.Int);	
							
					i = 0;
					sqlParams[i++].Value = Convert.ToInt32(adpopModel.ItemNo);	

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_log.Debug("ItemNo      :[" + adpopModel.ItemNo       + "]");
					_log.Debug(sbQuery.ToString());
					
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("연결채널정보생성:[" + adpopModel.ItemNo + "(" + KeyChannelNo + ")]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				adpopModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetLinkChannelDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				adpopModel.ResultCD   = "3101";
				adpopModel.ResultDesc = "연결채널정보 삭제 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


        /// <summary>
        /// 연결채널 삭제(해당광고 전체삭제)
        /// </summary>
        /// <param name="adpopModel"></param>
        public void SetLinkChannelDeleteAll(AdPopModel adpopModel)
        {
            //int KeyChannelNo = 0;
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkChannelDeleteAll() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n " + "DELETE  FROM  LinkChannel");
                sbQuery.Append("\n " + "WHERE   ItemNo = @ItemNo");
				
                // 쿼리실행
                try
                {
                    int rc = 0;
                    SqlParameter[] sqlParams = new SqlParameter[1]; 
                    _db.BeginTran();
                    sqlParams[0] = new SqlParameter("@ItemNo"  , SqlDbType.Int);
					sqlParams[0].Value = Convert.ToInt32(adpopModel.ItemNo);	

                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _log.Debug("ItemNo      :[" + adpopModel.ItemNo       + "]");
                    _log.Debug(sbQuery.ToString());
					
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("연결채널정보삭제:[" + adpopModel.ItemNo + "]");
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                adpopModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetLinkChannelDeleteAll() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                adpopModel.ResultCD   = "3101";
                adpopModel.ResultDesc = "연결채널정보전체삭제 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();			
            }		
        }
	
		#region HTTP String Get / Set

		private string GetHTTPString(string url)
		{
			string  responseText  = "";
			try
			{				
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "GET";
				request.Timeout = 15000;
				request.ProtocolVersion = HttpVersion.Version11;


				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				Stream stream = response.GetResponseStream();
				StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("utf-8"));
				responseText  = reader.ReadToEnd();
			}
			catch (Exception ex) 
			{
				_log.Debug("Exception in getThumbnail. Url: " + url + ". Info: " + ex.Message + Environment.NewLine + "Stack: " + ex.StackTrace);
			}
			return responseText;
		}

		#endregion


	}
}