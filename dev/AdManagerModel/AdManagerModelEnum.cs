using System;

namespace AdManagerModel
{
	/// <summary>
	/// 스케쥴삭제시 작업구분
	/// </summary>
	public enum	TYPE_ScheduleDelete
	{
		/// <summary>
		/// 카테고리편성 삭제 형식
		/// </summary>
		Category	= 0,
		/// <summary>
		/// 장르편성 삭제 형식
		/// </summary>
		Genre		= 1,
		/// <summary>
		/// 채널편성 삭제 형식
		/// </summary>
		Channel		= 2,
		/// <summary>
		/// 시리즈편성 삭제 형식
		/// </summary>
		Series		= 3
	}

	/// <summary>
	/// 편성타입 Enum
	/// </summary>
	public enum	TYPE_Schedule
	{
		/// <summary>
		/// 카테고리편성
		/// </summary>
		Category	= 0,
		/// <summary>
		/// 장르편성
		/// </summary>
		Genre		= 1,
		/// <summary>
		/// 채널편성
		/// </summary>
		Channel		= 2,
		/// <summary>
		/// 시리즈편성
		/// </summary>
		Series		= 3
	}
}
