
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// Btv메뉴정보 모델
	/// 2015-04-09 : 이어보기 유료송출 항목 추가 ReplayPPx
	/// </summary>
    public class MediaMenuModel : BaseModel
    {
        //조회용
        private string _SearchMediaCode = null;

        //상세정보용
        private string	_MediaCode = null;
        private string	_CategoryCode = null;
        private string	_CategoryName = null;
        private string	_CategoryAdPreRollYn = null;
        private string	_CategoryAdMidRollYn = null;
		private string  _CategoryAdPostRollYn = null;
		private string	_CategoryAdPayYn = null;
        private int		_CategoryAdRate = 0;
        private int     _CategoryAdNRate = 0;


        private string	_MenuCode = null;
        private string	_MenuName = null;
        private string	_MenuAdPreRollYn = null;
        private string	_MenuAdMidRollYn = null;
		private string  _MenuAdPostRollYn = null;
		private string	_MenuAdPayYn = null;
        private int		_MenuAdRate = 0;
        private int     _MenuAdNRate = 0;

        private bool	_InvalidityMenu = false;
		private bool	_ReplayYnCheck = false;
		private bool	_ReplayPPxCheck = false;
		private bool	_RendingYnCheck = false;

		/// <summary>
		/// 변경할 항목 타입
		/// </summary>
		public MediaMenuType mType { get; set; }

		/// <summary>
		/// 변경할 항목 값 0:미사용, 1:사용
		/// </summary>
		public int mValue { get; set; }

        //목록 조회용
        private DataSet _UserDataset;

        public MediaMenuModel() : base()
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchMediaCode = "";

            _MediaCode = "";
            _CategoryCode = "";
            _CategoryName = "";
            _CategoryAdPreRollYn = "";
            _CategoryAdMidRollYn = "";
			_CategoryAdPostRollYn = "";
			_CategoryAdPayYn = "";
            _CategoryAdRate = 0;
            _CategoryAdNRate = 0;

            _MenuCode = "";
            _MenuName = "";
            _MenuAdPreRollYn = "";
            _MenuAdMidRollYn = "";
			_MenuAdPostRollYn = "";
			_MenuAdPayYn = "";
            _MenuAdRate = 0;
            _MenuAdNRate = 0;

			_ReplayYnCheck = false;
			_ReplayPPxCheck = false;
			_RendingYnCheck = false;
            _InvalidityMenu = false;

			mType = MediaMenuType.None;
			mValue = -1;
			
            _UserDataset = new DataSet();
        }
        #endregion

        #region 프로퍼티
        public DataSet UserDataset
        {
            get { return _UserDataset; }
            set { _UserDataset = value; }
        }
        
		public string SearchMediaCode
        {
            get { return _SearchMediaCode; }
            set { _SearchMediaCode = value; }
        }

        public string MediaCode
        {
            get { return _MediaCode; }
            set { _MediaCode = value; }
        }
        
		public string CategoryCode
        {
            get { return _CategoryCode; }
            set { _CategoryCode = value; }
        }
        
		public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        public string CategoryAdPreRollYn
        {
            get { return _CategoryAdPreRollYn; }
            set { _CategoryAdPreRollYn = value; }
        }

        public string CategoryAdMidRollYn
        {
            get { return _CategoryAdMidRollYn; }
            set { _CategoryAdMidRollYn = value; }
        }

        public string CategoryAdPostRollYn
		{
			get { return _CategoryAdPostRollYn; }
			set { _CategoryAdPostRollYn = value; }
		}

        public string CategoryAdPayYn
		{
			get { return _CategoryAdPayYn; }
			set { _CategoryAdPayYn = value; }
		}

        public int CategoryAdRate
        {
            get { return _CategoryAdRate; }
            set { _CategoryAdRate = value; }
        }

        public int CategoryAdNRate
        {
            get { return _CategoryAdNRate; }
            set { _CategoryAdNRate = value; }
        }
        
		public string MenuCode
        {
            get { return _MenuCode; }
            set { _MenuCode = value; }
        }
        public string MenuName
        {
            get { return _MenuName; }
            set { _MenuName = value; }
        }
        public string MenuAdPreRollYn
        {
            get { return _MenuAdPreRollYn; }
            set { _MenuAdPreRollYn = value; }
        }
        public string MenuAdMidRollYn
        {
            get { return _MenuAdMidRollYn; }
            set { _MenuAdMidRollYn = value; }
        }

        public string MenuAdPostRollYn
		{
			get { return _MenuAdPostRollYn; }
			set { _MenuAdPostRollYn = value; }
		}

        public string MenuAdPayYn
		{
			get { return _MenuAdPayYn; }
			set { _MenuAdPayYn = value; }
		}

        public int MenuAdRate
        {
            get { return _MenuAdRate; }
            set { _MenuAdRate = value; }
        }

        public int MenuAdNRate
        {
            get { return _MenuAdNRate; }
            set { _MenuAdNRate = value; }
        }
        
		public bool ReplayYnCheck
        {
            get { return _ReplayYnCheck; }
			set { _ReplayYnCheck = value; }
        }

		public bool ReplayPPxCheck
		{
			get { return _ReplayPPxCheck; }
			set { _ReplayPPxCheck = value; }
		}

		public bool RendingYnCheck
		{
			get { return _RendingYnCheck; }
			set { _RendingYnCheck = value; }
		}
		public bool InvalidityMenu
		{
			get { return _InvalidityMenu; }
			set { _InvalidityMenu = value; }
		}
        #endregion
    }

	/// <summary>
	/// 옵션처리 타입
	/// 이어보기, 유료이어보기, 추천엔딩
	/// </summary>
	public enum MediaMenuType
	{ 
		Reply, ReplyPPx, REnding,None
	}
}
