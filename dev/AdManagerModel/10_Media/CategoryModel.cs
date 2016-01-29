// ===============================================================================
// UserInfo Data Model for Charites Project
//
// UserInfoModel.cs
//
// 사용자정보 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
// 2010/07/12 장용석 카테고리정보 추가(Inventory관련)
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
	public class CategoryModel : BaseModel
	{

		// 조회용
		private string _SearchKey           = null;		// 검색어
		private string _SearchCategoryLevel = null;		// 검색레벨

		// 상세정보용
		private string _MediaCode       = null;
		private string _CategoryCode    = null;
		private string _CategoryName    = null;
		private string _ModDt           = null;

		private	string	_Flag			= null;
		private	int		_SortNo			=	0;
		private	string  _CssFlag		= null;
		private string	_InventoryYn	= null;
		private	decimal _InventoryRate	=   0.0m;
	

		// 목록조회용
		private DataSet  _UserDataSet;
	
		public CategoryModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey              = "";
			_SearchCategoryLevel    = "";

			_MediaCode			    = "";
			_CategoryCode			= "";
			_CategoryName		    = "";
			_ModDt		            = "";

			_Flag			= "N";
			_SortNo			=  0;
			_CssFlag		= "N";
			_InventoryYn	= "N";
			_InventoryRate	=  0.0m;
			            
			_UserDataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet UserDataSet
		{
			get { return _UserDataSet;	}
			set { _UserDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchCategoryLevel 
		{
			get { return _SearchCategoryLevel;	}
			set { _SearchCategoryLevel = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		public string CategoryCode 
		{
			get { return _CategoryCode;		}
			set { _CategoryCode = value;		}
		}

		public string CategoryName 
		{
			get { return _CategoryName;		}
			set { _CategoryName = value;	}
		}
		
		public string ModDt 
		{
			get { return _ModDt;	}
			set { _ModDt = value;	}
		}


		/// <summary>
		/// 카테고리사용여부 : Y인경우 집계자료에 나타난다, N이면서 순서가 0이면 기타로 합쳐진다
		/// </summary>
		public string Flag
		{
			get { return _Flag;	}
			set { _Flag = value;	}
		}


		/// <summary>
		/// 카테고리 정렬순서, 0이면 사용하지 않는 것으로 인식함
		/// </summary>
		public int	  SortNo 
		{
			get { return _SortNo;	}
			set { _SortNo = value;	}
		}


		/// <summary>
		/// CSS광고 적용유무, Y인경우만 편성이 가능하다
		/// </summary>
		public string CssFlag
		{
			get { return _CssFlag;	}
			set { _CssFlag = value;	}
		}


		/// <summary>
		/// 인벤토리계산시 적용여부, 사용여부와 거의 비슷하나, 약간 틀리다, 쇼핑,홈메뉴등은 인벤토리계산시 사용되지 않는다
		/// </summary>
		public string InventoryYn 
		{
			get { return _InventoryYn;	}
			set { _InventoryYn = value;	}
		}

		/// <summary>
		/// 인벤토리 계산시 예상물량산출시 계수임. 1.0기준으로 +-됨,
		/// </summary>
		public decimal InventoryRate 
		{
			get { return _InventoryRate;	}
			set { _InventoryRate = value;	}
		}
		#endregion

	}
}