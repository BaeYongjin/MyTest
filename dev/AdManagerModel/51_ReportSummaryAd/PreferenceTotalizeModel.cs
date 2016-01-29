/*
 * -------------------------------------------------------
 * Class Name: PreferenceTotalizeModel
 * 주요기능  : 선호도조사 팝업 집계 Model
 * 작성일    : 2013.06.26
 * 특이사항  : 목록조회시엔 조회키를 이용해서 선호도팝업목록을 DS에 담아온다
 *             상세조회시엔 여러키를 이용해서 각 항목및 반응목록을 담아온다
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.09.04
 * 수정내용  : 
 *            - 모델 추가
 * --------------------------------------------------------
 */
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// 선호도조사팝업집계 모델
    /// </summary>
    public class PreferenceTotalizeModel : BaseModel
    {

        #region 프로퍼티

        #region 조회용(Input)
        /// <summary>
        /// 광고번호
        /// </summary>
        public string KeyItemNo { set; get; }

        /// <summary>
        /// 팝업공지 아이디
        /// </summary>
        public string KeyNoticeId { set; get; }

        /// <summary>
        /// 광고조사번호
        /// </summary>
        public string KeyExmNo { set; get; }

        /// <summary>
        /// 조회조건
        /// </summary>
        public string KeySearch { set; get; }

        /// <summary>
        /// 광고 시작일
        /// </summary>
        public string KeyStartDay { set; get; }

        /// <summary>
        /// 광고 종료일
        /// </summary>
        public string KeyEndDay { set; get; }
        #endregion


        /// <summary>
        /// 이벤트(선호도) 명
        /// </summary>
        public string EventName { set; get; }
                       
                /// <summary>
        /// 광고 노출수
        /// </summary>
        public int AdExpCount { set; get; }

        /// <summary>
        /// 팝업 노출수
        /// </summary>
        public int PopExpCount { set; get; }

        /// <summary>
        /// 응답수
        /// </summary>
        public int RepCount { set; get; }

        /// <summary>
        /// 응답률
        /// </summary>
        public float RepRate { set; get; }

        /// <summary>
        /// 팝업형식
        /// </summary>
        public string PopExpType { set; get; }

        public DataSet PreferenceDataSet { set; get; }
        #endregion


        #region 생성자
        public PreferenceTotalizeModel() : base()
        {
            Init();
        }
        #endregion

        #region 노출함수
        /// <summary>
        /// 모델초기화
        /// </summary>
        public void Init()
        {
            // 조회용-목록조회시
            this.KeySearch   = "";

            // 조회용-상세조회시
            this.KeyItemNo   = "0";
            this.KeyNoticeId = "";
            this.KeyExmNo    = "";
            this.KeyStartDay = "";
            this.KeyEndDay   = "";
            
            // 결과용 필드
            this.AdExpCount = 0;
            this.PopExpCount= 0;
            this.RepCount=0;
            this.RepRate = 0.0f;
            this.PopExpType = "";

            // 결과용 DataSet
            this.PreferenceDataSet = null;
        }
        #endregion
    }
}
