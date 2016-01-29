// ===============================================================================
// MediaRap Data Model for Charites Project
//
// MediaRapModel.cs
//
// 사용자정보 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
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
	public class MediaRapModel : BaseModel
	{

		// 조회용
		private string _SearchKey       = null;		// 검색어
		private string _SearchMediaRap = null;		// 검색레벨
		private string _SearchchkAdState_10    = null;		// 검색 사용여부상태

		// 상세정보용
		private string _RapCode          = null;		// 랩 아이디
		private string _RapName        = null;		// 랩 명		
		private string _RapType        = null;		// 랩 타입		
		private string _Tell        = null;		// 전화번호		
		private string _RegDt			= null;		// 등록일자
		private string _Comment     = null;		// 비고
		private string _UseYn     = null;		// 사용여부
		
		// 목록조회용
		private DataSet  _MediaRapDataSet;
	
		public MediaRapModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		= "";
			_SearchMediaRap = "";
			_SearchchkAdState_10 = "";

			_RapCode			= "";
			_RapName		= "";			
			_RapType		= "";			
			_Tell		= "";		
			_RegDt			= "";
			_Comment 	= "";
			_UseYn 	= "";
			
			_MediaRapDataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet MediaRapDataSet
		{
			get { return _MediaRapDataSet;	}
			set { _MediaRapDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchMediaRap
		{
			get { return _SearchMediaRap;	}
			set { _SearchMediaRap = value;	}
		}

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;	}
			set { _SearchchkAdState_10 = value;	}
		}

		public string RapCode 
		{
			get { return _RapCode;		}
			set { _RapCode = value;		}
		}

		public string RapName 
		{
			get { return _RapName;		}
			set { _RapName = value;	}
		}

		public string RapType 
		{
			get { return _RapType;		}
			set { _RapType = value;	}
		}

		public string Tell 
		{
			get { return _Tell;		}
			set { _Tell = value;	}
		}


		public string RegDt 
		{
			get { return _RegDt;		}
			set { _RegDt = value;		}
		}

		public string Comment 
		{
			get { return _Comment;  }
			set { _Comment = value; }
		}

		public string UseYn
		{
			get { return _UseYn;	}
			set { _UseYn = value;	}
		}


		#endregion

	}
}
