using System;
using System.Collections;
using AdManagerModel;

namespace AdManagerClient.Schedule
{
	#region [ ���߼��� ������ �Ű����� ]

	public class SchedulePerItemEventArgs : EventArgs
	{
		#region [�Ӽ�] ������ ������
		private	ArrayList	_schPerItem;

		/// <summary>
		/// ������ �������� �����´�
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
	/// ���õ� ���� �������͸� �߰��Ҷ� �߻��ϴ� �̺�Ʈ �ڵ鷯
	/// </summary>
	public  delegate void SchedulePerItemInsertEventHandler( object sender, SchedulePerItemEventArgs e );

	#endregion
}
