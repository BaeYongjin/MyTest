// ===============================================================================
// 
//
// 
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
	/// MediaMenuSetModel에 대한 요약 설명입니다.
	/// </summary>
	public class MediaMenuSetModel : BaseModel  
	{
		public MediaMenuSetModel() : base()
		{
			Init();
		}

        public void Init()
        {
            _CategoryDataSet    = new DataSet();
            _GenreDataSet       = new DataSet();
            _ChannelDataSet     = new DataSet();
        }

        #region [ 속 성 들 ]

        #region [P] 조회용 광고슬롯타입
        private string _searchAdType    = null;
        /// <summary>
        /// 조회할 광고타입을 설정하거나 가져온다
        /// </summary>
        public string SearchAdType
        {
            set
            {
                _searchAdType = value;
            }
            get
            {
                return _searchAdType;
            }
        }
        #endregion

        #region [P] 카테고리
        private int _category    =  0;
        /// <summary>
        /// 카테고리코드
        /// </summary>
        public int  Category
        {
            set
            {
                _category = value;
            }
            get
            {
                return _category;
            }
        }
        #endregion

        #region [P] 장르
        private int _genre    = 0;
        /// <summary>
        /// 장르코드
        /// </summary>
        public int Genre
        {
            set
            {
                _genre = value;
            }
            get
            {
                return _genre;
            }
        }
        #endregion

        #region [P] 채널
        private int _channel    = 0;
        /// <summary>
        /// 채널코드
        /// </summary>
        public int Channel
        {
            set
            {
                _channel = value;
            }
            get
            {
                return _channel;
            }
        }
        #endregion

        #region [P] 카테고리 DataSet
        private DataSet _CategoryDataSet;
        /// <summary>
        /// 카테고리 DataSet를 가져오거나 설정한다
        /// </summary>
        public DataSet CategoryDataSet
        {
            set
            {
                _CategoryDataSet = value;
            }
            get
            {
                return _CategoryDataSet;
            }
        }
        #endregion

        #region [P] 장르 DataSet
        private DataSet _GenreDataSet;
        /// <summary>
        /// 장르 DataSet를 가져오거나 설정한다
        /// </summary>
        public DataSet GenreDataSet
        {
            set
            {
                _GenreDataSet = value;
            }
            get
            {
                return _GenreDataSet;
            }
        }
        #endregion

        #region [P] 채널 DataSet
        private DataSet _ChannelDataSet;
        /// <summary>
        /// 장르 DataSet를 가져오거나 설정한다
        /// </summary>
        public DataSet ChannelDataSet
        {
            set
            {
                _ChannelDataSet = value;
            }
            get
            {
                return _ChannelDataSet;
            }
        }
        #endregion

        #endregion
	}
}
