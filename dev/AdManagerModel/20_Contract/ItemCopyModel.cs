// ===============================================================================
// Contract Data Model for Charites Project
// ItemCopyModel.cs
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
    public class ItemCopyModel : BaseModel
    {
        // ��������
        private int		_ItemNoSou		=	0;

		//
		private	int		_ItemNoDes		=	0;
        private string	_ItemName       =	null;	
        private string	_ExcuteStartDay =	null;
        private string	_ExcuteEndDay   =	null;

        public ItemCopyModel() : base () 
        {
            Init();
        }

        #region Public �޼ҵ�
        public void Init()
        {
			_ItemNoSou	=	0;
			_ItemNoDes	=	0;
			_ItemName	=	"";
			_ExcuteStartDay = "";
			_ExcuteEndDay	= "";
        }

        #endregion

        #region  ������Ƽ 
		/// <summary>
		/// ������� �����ȣ
		/// </summary>
		public	int		ItemNoSou
		{
			get	{	return _ItemNoSou;		}
			set	{	_ItemNoSou	=	value;	}
		}

		/// <summary>
		/// ������ �����ȣ
		/// </summary>
		public	int		ItemNoDes
		{
			get	{	return _ItemNoDes;		}
			set	{	_ItemNoDes	=	value;	}
		}

		/// <summary>
		/// ������ �����
		/// </summary>
        public string ItemName 
        {
            get { return _ItemName; }
            set { _ItemName = value;}
        }

		/// <summary>
		/// ������ ���������
		/// </summary>
		public string ExcuteStartDay 
        {
            get { return _ExcuteStartDay; }
            set { _ExcuteStartDay = value;}
        }
        
		/// <summary>
		/// ������ ����������
		/// </summary>
		public string ExcuteEndDay 
        {
            get { return _ExcuteEndDay; }
            set { _ExcuteEndDay = value;}
        }
        #endregion

    }
}
