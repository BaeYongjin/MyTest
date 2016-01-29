
#region 주석...
/*
 * -------------------------------------------------------
 * Class Name: IUserControl
 * 주요기능  : 사용자 정의 컨트롤의 공통 interface
 * 작성자    : Bae 
 * 작성일    : 2010.05.11
 * 특이사항  : 모든 사용자 정의 컨트롤은 이 인터페이스 상속
 *             공통적인 이벤트와 속성만 정의 해 놓고
 *             이를 실제 사용하는(호출)하는 곳에서
 *             공통코드로 처리하기 위해서 사용함
 * -------------------------------------------------------
 * [수정사항]-bae
 * -------------------------------------------------------
 * 코드      : 
 * 수정자    : 
 * 수정일    : 
 * 수정내용  : 
 * --------------------------------------------------------
 */
#endregion

using System;
using System.Windows.Forms;

using WinFramework.Base;

namespace AdManagerClient
{
	/// <summary>
	/// IUserControl에 대한 요약 설명입니다.
	/// </summary>
	public interface IUserControl
	{
        #region 이벤트핸들러
        event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러
        event ProgressEventHandler ProgressEvent;	    // 처리중이벤트 핸들러
        #endregion

        /// <summary>
        /// 메뉴 코드-보안이 필요한 화면에 필요함-구현대상
        /// </summary>
        string MenuCode
        { set; get; }
        /// <summary>
        /// 부모 컨트롤 지정-구현대상
        /// </summary>
        /// <param name="control"></param>
        void SetParent(Control control);
        /// <summary>
        /// DockStype 지정-구현대상
        /// </summary>
        /// <param name="style"></param>
        void SetDockStyle(DockStyle style);	
	}
}
