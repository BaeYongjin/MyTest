// ===============================================================================
// Contract Data Model for Charites Project
// ������� Ȯ�ο� ��
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
	/// ����������� Ŭ���� ��.
	/// </summary>
	public class AdFileDistributeModel : BaseModel
	{		
		// ��������
		//  yyyy-mm-dd �������� ����Ұ�
		private string _SearchDateBegin = null;
		private string _SearchDateEnd	= null;
		
		// �����ȸ��
		private DataSet  _DataSet;

		public AdFileDistributeModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{			
			_SearchDateBegin	= "";		
			_SearchDateEnd		= "";
			_DataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		/// <summary>
		/// ��ȸ������ �����
		/// </summary>
		public DataSet ListDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		/// <summary>
		/// ��ȸ�Ⱓ ������
		/// </summary>
		public string BeginDate
		{
			get { return	_SearchDateBegin;	}
			set { _SearchDateBegin	= value;	}
		}

		/// <summary>
		/// ��ȸ�Ⱓ ������
		/// </summary>
		public string EndDate
		{
			get { return	_SearchDateEnd;	}
			set { _SearchDateEnd	= value;	}
		}
	 
		#endregion

	}
}
