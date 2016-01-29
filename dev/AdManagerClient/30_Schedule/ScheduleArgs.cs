using System;
using System.Collections;
using AdManagerModel;

namespace AdManagerClient.Schedule
{
	#region [ 다중선택 편성내역 매개변수 ]

	public class SchedulePerItemEventArgs : EventArgs
	{
		#region [속성] 선택한 편성정보
		private	ArrayList	_schPerItem;

		/// <summary>
		/// 선택한 편성정보를 가져온다
		/// </summary>
		public ArrayList ScheduleData
		{
			get
			{
				return _schPerItem;
			}
		}
		#endregion

		public SchedulePerItemEventArgs( ArrayList scheduleData)
		{
			_schPerItem = scheduleData;
		}
	}


	/// <summary>
	/// 선택된 광고에 편성데이터를 추가할때 발생하는 이벤트 핸들러
	/// </summary>
	public  delegate void SchedulePerItemInsertEventHandler( object sender, SchedulePerItemEventArgs e );

	#endregion
}
