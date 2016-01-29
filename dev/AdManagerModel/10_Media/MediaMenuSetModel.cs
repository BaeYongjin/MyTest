// ===============================================================================
// 
//
// 
//
// ��������� Ŭ������ �����մϴ�. 
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
	/// MediaMenuSetModel�� ���� ��� �����Դϴ�.
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

        #region [ �� �� �� ]

        #region [P] ��ȸ�� ������Ÿ��
        private string _searchAdType    = null;
        /// <summary>
        /// ��ȸ�� ����Ÿ���� �����ϰų� �����´�
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

        #region [P] ī�װ�
        private int _category    =  0;
        /// <summary>
        /// ī�װ��ڵ�
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

        #region [P] �帣
        private int _genre    = 0;
        /// <summary>
        /// �帣�ڵ�
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

        #region [P] ä��
        private int _channel    = 0;
        /// <summary>
        /// ä���ڵ�
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

        #region [P] ī�װ� DataSet
        private DataSet _CategoryDataSet;
        /// <summary>
        /// ī�װ� DataSet�� �������ų� �����Ѵ�
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

        #region [P] �帣 DataSet
        private DataSet _GenreDataSet;
        /// <summary>
        /// �帣 DataSet�� �������ų� �����Ѵ�
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

        #region [P] ä�� DataSet
        private DataSet _ChannelDataSet;
        /// <summary>
        /// �帣 DataSet�� �������ų� �����Ѵ�
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
