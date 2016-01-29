/*
 * -------------------------------------------------------
 * Class Name: ReserveModel
 * 주요기능  : 예약처리에 대한 history data model
 * 작성자    : bae 
 * 작성일    : 2010.06.07
 * 특이사항  : 
 * -------------------------------------------------------
 * [수정사항]
 * 수정코드  : 
 * 수정자    : 
 * 수정일    : 
 * 수정내용  :             
 * --------------------------------------------------------
 * 
 */

using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// ReserveModel에 대한 요약 설명입니다.
	/// </summary>
	public class ReserveHistoryModel : BaseModel
	{
		private string ackNo = string.Empty;
		private string seq = string.Empty;
		private string rvType = string.Empty;
		private string regDt = string.Empty;
		private string rvDesc = string.Empty;
		private string isSuccess = string.Empty;

		private DataSet dsList = new DataSet();

		public ReserveHistoryModel() : base()
		{			
		}
		
		/// <summary>
		/// 파일배포승인 번호
		/// </summary>
		public string AckNo
		{
			set{this.ackNo = value;}
			get{return this.ackNo;}
		}
		/// <summary>
		/// 파일배포승인 seq
		/// </summary>
		public string Seq
		{
			set{this.seq = value;}
			get{return this.seq;}
		}
		/// <summary>
		/// 예약처리 구분(01-파일배포승인,미래에 확장가능성 때문에 추가)
		/// </summary>
		public string RvType
		{
			set{this.rvType = value;}
			get{return this.rvType;}
		}
		/// <summary>
		/// 데이터 등록일
		/// </summary>
		public string RegDt
		{
			set{this.regDt = value;}
			get{return this.regDt;}
		}
		/// <summary>
		/// 처리예약에 대한 설명
		/// </summary>
		public string RvDesc
		{
			set{this.rvDesc = value;}
			get{return this.rvDesc;}
		}

		/// <summary>
		/// 예약처리의 성공(Y),실패(N) 값
		/// </summary>
		public string IsSuccess
		{
			set{this.isSuccess = value;}
			get{return this.isSuccess;}
		}

		/// <summary>
		/// History 목록
		/// </summary>
		public DataSet DataList
		{
			set{this.dsList = value;}
			get{return this.dsList;}
		}
	}
}
