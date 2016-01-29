using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerWebService.Interface
{			
	/// <summary>
	/// RequestAdPopContentList에 대한 요약 설명입니다.
	/// </summary>
	public partial class RequestAdPopContentList : System.Web.UI.Page
	{
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private Logger         _log         = FrameSystem.oLog;

		AdPopModel adpopModel  = new AdPopModel();	// 매체정보모델

		private string itemno = "";
		private string flag = "";
		private string adpop_id = "";

		private string content_id1 = "";
		private string genre_code1 = "";

		private string content_id2 = "";
		private string genre_code2 = "";

		private string content_id3 = "";
		private string genre_code3 = "";

		private string content_id4 = "";
		private string genre_code4 = "";

		private string content_id5 = "";
		private string genre_code5 = "";


		protected void Page_Load(object sender, System.EventArgs e)
		{
			string cont_id_list = "";
			string genre_cd_list = "";

			try
			{
                try
                {
                    itemno = Request.QueryString["adpop_id"].ToString();
                }
                catch(Exception ex)
                {
                    Response.Write("\n" + "------------------------확인해 주세요-----------------------------------" + "<br>");
                    Response.Write("\n" +  ex.Message + "<br>");
                    Response.Write("\n" + "------------------------확인해 주세요-----------------------------------" + "<br>");
                }
				flag = Request.QueryString["flag"].ToString();
				adpop_id = Request.QueryString["notice_id"].ToString();
				cont_id_list = Request.QueryString["cont_id_list"].ToString();
				genre_cd_list = Request.QueryString["genre_cd_list"].ToString();

				string[] contentSplit = cont_id_list.Split('^');
				string[] genreSplit = genre_cd_list.Split('^');

				content_id1 = contentSplit[0].ToString();
				genre_code1 = genreSplit[0].ToString();

				content_id2 = contentSplit[1].ToString();
				genre_code2 = genreSplit[1].ToString();

				content_id3 = contentSplit[2].ToString();
				genre_code3 = genreSplit[2].ToString();

				content_id4 = contentSplit[3].ToString();
				genre_code4 = genreSplit[3].ToString();

				content_id5 = contentSplit[4].ToString();
				genre_code5 = genreSplit[4].ToString();


				adpopModel.ItemNo = itemno;					
				adpopModel.MediaCode = "1";
				adpopModel.JumpType = "4";
				adpopModel.AdPopID = adpop_id;
				adpopModel.ContentID1 = content_id1;
				adpopModel.ContentID2 = content_id2;
				adpopModel.ContentID3 = content_id3;
				adpopModel.ContentID4 = content_id4;
				adpopModel.ContentID5 = content_id5;

				adpopModel.GenreCode1 = genre_code1;
				adpopModel.GenreCode2 = genre_code2;
				adpopModel.GenreCode3 = genre_code3;
				adpopModel.GenreCode4 = genre_code4;
				adpopModel.GenreCode5 = genre_code5;

				Response.Write("------------------------OK-----------------------------------------");
				Response.Write(flag);
				Response.Write(itemno);
				Response.Write(adpop_id);
				Response.Write(content_id1);
				Response.Write(content_id2);
				Response.Write(content_id3);
				Response.Write(content_id4);
				Response.Write(content_id5);
				Response.Write(genre_code1);
				Response.Write(genre_code2);
				Response.Write(genre_code3);					
				Response.Write(genre_code4);					
				Response.Write(genre_code5);					
				Response.Write("------------------------OK----------------------------------------");

				_log.Debug("itemno					 :[" + itemno  + "]");	
				_log.Debug("flag					 :[" + flag  + "]");	
				_log.Debug("adpop_id				 :[" + adpop_id  + "]");	
				_log.Debug("content_id1              :[" + content_id1  + "]");	
				_log.Debug("content_id2              :[" + content_id2  + "]");	
				_log.Debug("content_id3              :[" + content_id3  + "]");	
				_log.Debug("content_id4              :[" + content_id4  + "]");	
				_log.Debug("content_id5              :[" + content_id5  + "]");	

				_log.Debug("genre_code1              :[" + genre_code1  + "]");	
				_log.Debug("genre_code2              :[" + genre_code2  + "]");	
				_log.Debug("genre_code3              :[" + genre_code3  + "]");	
				_log.Debug("genre_code4              :[" + genre_code4  + "]");	
				_log.Debug("genre_code5              :[" + genre_code5  + "]");	

				if(flag.Equals("I"))
				{
					//ChannelJump테이블에 인서트
					adpopModel.ContentID = content_id1;
					new RequestAdPopContentListBiz().SetChannelJumpCreate(adpopModel);
										
					if(!content_id1.Equals(null) && !content_id1.Equals(""))
					{									
						adpopModel.ContentID = content_id1;
						//LinkChnnel에 인서트							
						new RequestAdPopContentListBiz().SetLinkChannelCreate(adpopModel);						
						adpopModel.ContentID = "";
					}			
					if(!content_id2.Equals(null) && !content_id2.Equals(""))
					{									
						adpopModel.ContentID = content_id2;
			
						new RequestAdPopContentListBiz().SetLinkChannelCreate(adpopModel);				
						adpopModel.ContentID = "";
					}			
					if(!content_id3.Equals(null) && !content_id3.Equals(""))
					{									
						adpopModel.ContentID = content_id3;
			
						new RequestAdPopContentListBiz().SetLinkChannelCreate(adpopModel);				
						adpopModel.ContentID = "";
					}			
					if(!content_id4.Equals(null) && !content_id4.Equals(""))
					{									
						adpopModel.ContentID = content_id4;
			
						new RequestAdPopContentListBiz().SetLinkChannelCreate(adpopModel);					
						adpopModel.ContentID = "";
					}			
					if(!content_id5.Equals(null) && !content_id5.Equals(""))
					{									
						adpopModel.ContentID = content_id5;
			
						new RequestAdPopContentListBiz().SetLinkChannelCreate(adpopModel);					
						adpopModel.ContentID = "";
					}			
				}
				else if(flag.Equals("U"))
				{
                    /*
                     * 수정모듈
                     * 해당 LinkChannel의 데이터를 전부 삭제 한후에
                     * 다시 입력처리한다
                     */
					adpopModel.ContentID = content_id1;
					new RequestAdPopContentListBiz().SetChannelJumpUpdate(adpopModel);
                    new RequestAdPopContentListBiz().SetLinkChannelDeleteAll( adpopModel );

					if(!content_id1.Equals(null) && !content_id1.Equals(""))
					{									
						adpopModel.ContentID = content_id1;
						new RequestAdPopContentListBiz().SetLinkChannelCreate(adpopModel);
						adpopModel.ContentID = "";
					}			
					if(!content_id2.Equals(null) && !content_id2.Equals(""))
					{									
						adpopModel.ContentID = content_id2;
						new RequestAdPopContentListBiz().SetLinkChannelCreate(adpopModel);
						adpopModel.ContentID = "";
					}			
					if(!content_id3.Equals(null) && !content_id3.Equals(""))
					{									
						adpopModel.ContentID = content_id3;
						new RequestAdPopContentListBiz().SetLinkChannelCreate(adpopModel);
						adpopModel.ContentID = "";
					}			
					if(!content_id4.Equals(null) && !content_id4.Equals(""))
					{									
						adpopModel.ContentID = content_id4;
						new RequestAdPopContentListBiz().SetLinkChannelCreate(adpopModel);
						adpopModel.ContentID = "";
					}			
					if(!content_id5.Equals(null) && !content_id5.Equals(""))
					{									
						adpopModel.ContentID = content_id5;
						new RequestAdPopContentListBiz().SetLinkChannelCreate(adpopModel);
						adpopModel.ContentID = "";
					}	
				}
				if(flag.Equals("D"))
				{
					new RequestAdPopContentListBiz().SetChannelJumpDelete(adpopModel);
                    new RequestAdPopContentListBiz().SetLinkChannelDeleteAll( adpopModel );
				}					
			}
			catch(Exception ex)
			{
				Response.Write("------------------------ERROR-----------------------------------------");								
                Response.Write( ex.Message );
				Response.Write("------------------------ERROR-----------------------------------------");			
			}
			finally
			{
				Response.End();
			}
		}

		#region Web Form 디자이너에서 생성한 코드
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 이 호출은 ASP.NET Web Form 디자이너에 필요합니다.
			//
			InitializeComponent();
			base.OnInit(e);
		}
	
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion

	
	}

}
