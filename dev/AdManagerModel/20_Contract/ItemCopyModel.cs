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
    /// 사용자정보의 클래스 모델.
    /// </summary>
    public class ItemCopyModel : BaseModel
    {
        // 상세정보용
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

        #region Public 메소드
        public void Init()
        {
			_ItemNoSou	=	0;
			_ItemNoDes	=	0;
			_ItemName	=	"";
			_ExcuteStartDay = "";
			_ExcuteEndDay	= "";
        }

        #endregion

        #region  프로퍼티 
		/// <summary>
		/// 복사원본 광고번호
		/// </summary>
		public	int		ItemNoSou
		{
			get	{	return _ItemNoSou;		}
			set	{	_ItemNoSou	=	value;	}
		}

		/// <summary>
		/// 복사대상 광고번호
		/// </summary>
		public	int		ItemNoDes
		{
			get	{	return _ItemNoDes;		}
			set	{	_ItemNoDes	=	value;	}
		}

		/// <summary>
		/// 복사대상 광고명
		/// </summary>
        public string ItemName 
        {
            get { return _ItemName; }
            set { _ItemName = value;}
        }

		/// <summary>
		/// 복사대상 광고시작일
		/// </summary>
		public string ExcuteStartDay 
        {
            get { return _ExcuteStartDay; }
            set { _ExcuteStartDay = value;}
        }
        
		/// <summary>
		/// 복사대상 광고종료일
		/// </summary>
		public string ExcuteEndDay 
        {
            get { return _ExcuteEndDay; }
            set { _ExcuteEndDay = value;}
        }
        #endregion

    }
}
