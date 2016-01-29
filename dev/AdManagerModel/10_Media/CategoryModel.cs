// ===============================================================================
// UserInfo Data Model for Charites Project
//
// UserInfoModel.cs
//
// ��������� Ŭ������ �����մϴ�. 
//
// ===============================================================================
// Release history
// 2010/07/12 ��뼮 ī�װ����� �߰�(Inventory����)
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
	/// ����������� Ŭ���� ��.
	/// </summary>
	public class CategoryModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey           = null;		// �˻���
		private string _SearchCategoryLevel = null;		// �˻�����

		// ��������
		private string _MediaCode       = null;
		private string _CategoryCode    = null;
		private string _CategoryName    = null;
		private string _ModDt           = null;

		private	string	_Flag			= null;
		private	int		_SortNo			=	0;
		private	string  _CssFlag		= null;
		private string	_InventoryYn	= null;
		private	decimal _InventoryRate	=   0.0m;
	

		// �����ȸ��
		private DataSet  _UserDataSet;
	
		public CategoryModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
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

		#region  ������Ƽ 

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
		/// ī�װ���뿩�� : Y�ΰ�� �����ڷῡ ��Ÿ����, N�̸鼭 ������ 0�̸� ��Ÿ�� ��������
		/// </summary>
		public string Flag
		{
			get { return _Flag;	}
			set { _Flag = value;	}
		}


		/// <summary>
		/// ī�װ� ���ļ���, 0�̸� ������� �ʴ� ������ �ν���
		/// </summary>
		public int	  SortNo 
		{
			get { return _SortNo;	}
			set { _SortNo = value;	}
		}


		/// <summary>
		/// CSS���� ��������, Y�ΰ�츸 ���� �����ϴ�
		/// </summary>
		public string CssFlag
		{
			get { return _CssFlag;	}
			set { _CssFlag = value;	}
		}


		/// <summary>
		/// �κ��丮���� ���뿩��, ��뿩�ο� ���� ����ϳ�, �ణ Ʋ����, ����,Ȩ�޴����� �κ��丮���� ������ �ʴ´�
		/// </summary>
		public string InventoryYn 
		{
			get { return _InventoryYn;	}
			set { _InventoryYn = value;	}
		}

		/// <summary>
		/// �κ��丮 ���� ���󹰷������ �����. 1.0�������� +-��,
		/// </summary>
		public decimal InventoryRate 
		{
			get { return _InventoryRate;	}
			set { _InventoryRate = value;	}
		}
		#endregion

	}
}