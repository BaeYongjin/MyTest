/*
 * -------------------------------------------------------
 * Class Name: TargetingService
 * 주요기능  : Targeting WebService 함수노출
 * 작성자    : 모름
 * 작성일    : 모름
 * 특이사항  : 
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : bae
 * 수정일    : 2010.10.04
 * 수정부분  :
 *			  - GetTargetingDetail(..) 내부 호출 함수 수정
 *            - SetTargetingDetailUpdate(..) 내부 호출 함수 수정
 * 수정내용  : 
 *            - 2Slot 처리위한 내부 method 호출 변경
 *            - 
 * --------------------------------------------------------
 * 2012.02.16 고객군타겟팅 추가 RH.Jung
 * -------------------------------------------------------
 * 수정코드  : [E_03]
 * 수정자    : 김보배
 * 수정일    : 2013.04.01
 * 수정부분  :
 *			  - GetStbList() 내부 호출 함수 추가
 * 수정내용  : 
 *            - 셋탑목록조회시 사용하는 함수 추가
 * --------------------------------------------------------
 * 수정코드  : [E_04]
 * 수정자    : 김보배
 * 수정일    : 2013.10.16
 * 수정부분  :
 *			  - SetTargetingProfileAdd() 내부 호출 함수 추가
 * 수정내용  : 
 *            - 프로파일 타겟팅 수정 함수 추가
 * --------------------------------------------------------
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Target
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// TargetingService에 대한 요약 설명입니다.
	/// </summary>
	public class TargetingService : System.Web.Services.WebService
	{
		private TargetingBiz targetingBiz = null;

		public TargetingService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			targetingBiz = new TargetingBiz();
		}

		#region 구성 요소 디자이너에서 생성한 코드
		
		//웹 서비스 디자이너에 필요합니다. 
		private IContainer components = null;
				
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		[WebMethod]
		public TargetingModel GetTargetingList(HeaderModel header, TargetingModel targetingModel)
		{
			targetingBiz.GetTargetingList(header, targetingModel);
			return targetingModel;			
		}

		[WebMethod]
		public TargetingModel GetCollectionList(HeaderModel header, TargetingModel targetingModel)
		{
            targetingBiz.GetCollectionList(header, targetingModel);
			return targetingModel;
		}

		[WebMethod]
		public TargetingModel GetTargetingDetail(HeaderModel header, TargetingModel targetingModel)
		{
			//[E_01] 이전 호출
			//targetingBiz.GetTargetingDetail(header, targetingModel);
			//return targetingModel;

			//[E_01] 2SLOT
			targetingBiz.GetTargetingDetail_10_04(header, targetingModel);
			return targetingModel;
		}

        //GetTargetingDetail2

        [WebMethod]
        public TargetingModel GetTargetingDetail2(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.GetTargetingDetail2(header, targetingModel);
            return targetingModel;
        }

		[WebMethod]
		public TargetingModel GetTargetingRate(HeaderModel header, TargetingModel targetingModel)
		{
			targetingBiz.GetTargetingRate(header, targetingModel);
			return targetingModel;
		}

        [WebMethod]
        public TargetingModel SetTargetingDetailUpdate(HeaderModel header, TargetingModel targetingModel)
        {
			//[E_01] 이전 호출
            //targetingBiz.SetTargetingDetailUpdate(header, targetingModel);
            //return targetingModel;

			//[E_01] 2SLOT
			targetingBiz.SetTargetingDetailUpdate_10_04(header, targetingModel);
			return targetingModel;
        }

		[WebMethod]
		public TargetingModel SetTargetingRateUpdate(HeaderModel header, TargetingModel targetingModel)
		{
			
			targetingBiz.SetTargetingRateUpdate(header, targetingModel);
			return targetingModel;
			
		}

		[WebMethod]
		public TargetingModel GetRegionList(HeaderModel header, TargetingModel targetingModel)
		{
			targetingBiz.GetRegionList(header, targetingModel);
			return targetingModel;
		}      

		[WebMethod]
		public TargetingModel GetAgeList(HeaderModel header, TargetingModel targetingModel)
		{
			targetingBiz.GetAgeList(header, targetingModel);
			return targetingModel;
		}

        // 고객군타겟팅 관련 추가 2012.02.14 RH.Jung 
        [WebMethod]
        public TargetingModel GetTargetingCollectionList(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.GetTargetingCollectionList(header, targetingModel);
            return targetingModel;
        }

        [WebMethod]
        public TargetingModel SetTargetingCollectionAdd(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.SetTargetingCollectionAdd(header, targetingModel);
            return targetingModel;
        }

        [WebMethod]
        public TargetingModel SetTargetingCollectionDelete(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.SetTargetingCollectionDelete(header, targetingModel);
            return targetingModel;
        }

        [WebMethod]
        public TargetingModel GetStbList(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.GetStbList(header, targetingModel);
            return targetingModel;
        }

        // [E_04]
        [WebMethod]
        public TargetingModel SetTargetingProfileAdd(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.SetTargetingProfileAdd(header, targetingModel);
            return targetingModel;
        }

	}
}
