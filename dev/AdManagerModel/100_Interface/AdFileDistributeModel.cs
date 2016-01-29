// ===============================================================================
// Contract Data Model for Charites Project
// 광고배포 확인용 모델
// ===============================================================================
// Release history
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// 사용자정보의 클래스 모델.
	/// </summary>
	public class AdFileDistributeModel : BaseModel
	{		
		// 상세정보용
		//  yyyy-mm-dd 형식으로 사용할것
		private string _SearchDateBegin = null;
		private string _SearchDateEnd	= null;
		
		// 목록조회용
		private DataSet  _DataSet;

		public AdFileDistributeModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{			
			_SearchDateBegin	= "";		
			_SearchDateEnd		= "";
			_DataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		/// <summary>
		/// 조회데이터 결과셋
		/// </summary>
		public DataSet ListDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		/// <summary>
		/// 조회기간 시작일
		/// </summary>
		public string BeginDate
		{
			get { return	_SearchDateBegin;	}
			set { _SearchDateBegin	= value;	}
		}

		/// <summary>
		/// 조회기간 종료일
		/// </summary>
		public string EndDate
		{
			get { return	_SearchDateEnd;	}
			set { _SearchDateEnd	= value;	}
		}
	 
		#endregion

	}
}
