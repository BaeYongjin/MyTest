using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Common
{
	/// <summary>
	/// CodeService에 대한 요약 설명입니다.
	/// </summary>
	public class ZipCodeBiz : BaseBiz
	{
		public ZipCodeBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}

		public void GetAddressList(HeaderModel header, ZipCodeModel data)
		{
			try
			{
				// 데이트베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append("\n select top 500 ZipCode ");
				sbQuery.Append("\n 			,Sido ");
				sbQuery.Append("\n 			,Gugun ");
				sbQuery.Append("\n 			,Dong ");
				sbQuery.Append("\n 			,Bunji ");
				sbQuery.Append("\n 			,Sido + ' ' + gugun + ' ' + Dong + ' ' +isnull(Bunji,'') as AddrFull ");
				sbQuery.Append("\n from	SystemZip noLock ");
				
				// 코드분류가 선택했으면
				if(data.SearchKey.Length > 0 )
				{
					sbQuery.Append("  WHERE Dong like '" + data.SearchKey + "' \n");
				}						

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				data.DsAddr= ds.Copy();
				data.ResultCnt	= Utility.GetDatasetCount(data.DsAddr);
				data.ResultCD	= "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSectionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}
	

		/// <summary>
		/// 우편번호 앞 3자리로만 목록 검색
		/// </summary>		
		public void GetPreZipList(HeaderModel header, ZipCodeModel data)
		{
			try
			{
				// 데이트베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append("\n select    ZipCode ");
				sbQuery.Append("\n 			,Sido ");
				sbQuery.Append("\n 			,Gugun ");
				sbQuery.Append("\n 			,Dong ");
				sbQuery.Append("\n 			,Bunji ");
				sbQuery.Append("\n 			,Sido + ' ' + gugun + ' ' + Dong + ' ' +isnull(Bunji,'') as AddrFull ");
				sbQuery.Append("\n from	SystemZip noLock ");
				
				// 코드분류가 선택했으면
				if(data.SearchZip.Length > 0 )
				{
					sbQuery.Append("  Where Substring(ZipCode, 1,3) In('" + data.SearchZip + "') \n");
				}

				// 동 검사
				if (data.SearchKey != null && data.SearchKey.Length > 0)
				{
					sbQuery.Append(" Union all ");

					sbQuery.Append("\n select top 500 ZipCode ");
					sbQuery.Append("\n 			,Sido ");
					sbQuery.Append("\n 			,Gugun ");
					sbQuery.Append("\n 			,Dong ");
					sbQuery.Append("\n 			,Bunji ");
					sbQuery.Append("\n 			,Sido + ' ' + gugun + ' ' + Dong + ' ' +isnull(Bunji,'') as AddrFull ");
					sbQuery.Append("\n from	SystemZip noLock ");
				
					// 코드분류가 선택했으면
					if(data.SearchKey.Length > 0 )
					{
						sbQuery.Append("  WHERE Dong like '" + data.SearchKey + "' \n");
					}
				}



				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				data.DsAddr= ds.Copy();
				data.ResultCnt	= Utility.GetDatasetCount(data.DsAddr);
				data.ResultCD	= "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSectionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}
	

		/// <summary>
		/// 타겟팅 된 우편번호만 가져오기
		/// </summary>		
		public void GetIncludeZipList(HeaderModel header, ZipCodeModel data)
		{
			try
			{
				// 데이트베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				sbQuery.Append("\n Select    ZipCode ");
				sbQuery.Append("\n 			,Sido    ");
				sbQuery.Append("\n 			,Gugun   ");
				sbQuery.Append("\n 			,Dong    ");
				sbQuery.Append("\n 			,Bunji   ");
				sbQuery.Append("\n 			,Sido + ' ' + gugun + ' ' + Dong + ' ' +isnull(Bunji,'') as AddrFull ");
				sbQuery.Append("\n From	SystemZip noLock ");
				
				// 코드분류가 선택했으면
				if(data.SearchZip.Length > 0 )
					sbQuery.Append("  Where ZipCode In(" + data.SearchZip + ") \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				data.DsAddr= ds.Copy();
				data.ResultCnt	= Utility.GetDatasetCount(data.DsAddr);
				data.ResultCD	= "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSectionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "코드정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

	}
}
