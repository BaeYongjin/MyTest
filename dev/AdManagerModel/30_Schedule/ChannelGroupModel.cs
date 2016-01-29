// ===============================================================================
// ChannelGroup Data Model for Charites Project
//
// ChannelGroupModel.cs
//
// 컨텐츠정보 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
// 2007.06.06 송명환 v1.0
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
    public class ChannelGroupModel : AdGroupModel
    {


        // 상세정보용
        private string _ChannelNo		= null;		//장르코드
        private string _GenreName		= null;		//장르명
        private string _CategoryCode    = null;     //카테고리코드	
        private string _GenreCode       = null;	    //장르코드
   
	
        // 목록조회용
        private DataSet  _ChannelGroupDataSet;

        //팝업목록 조회용
        private DataSet  _ChannelGroup_pDataSet;

        

        public ChannelGroupModel() : base () 
        {
            InitGenre();
        }

        #region Public 메소드
        public void InitGenre()
        {
            _ChannelNo       = "";	  //장르코드
            _GenreName       = "";	  //장르이름
            _CategoryCode    = "";    //카테고리코드
            _GenreCode       = "";    //장르코드


            _ChannelGroupDataSet = new DataSet();
            _ChannelGroup_pDataSet = new DataSet();
        }

        #endregion

        #region  프로퍼티 

        public DataSet ChannelGroupDataSet
        {
            get { return _ChannelGroupDataSet;	}
            set { _ChannelGroupDataSet = value;	}
        }
        public DataSet ChannelGroup_pDataSet
        {
            get { return _ChannelGroup_pDataSet;	}
            set { _ChannelGroup_pDataSet = value;	}
        }
        public string ChannelNo
        {
            get { return _ChannelNo; }
            set { _ChannelNo = value;}
        }
        public string GenreName
        {
            get { return _GenreName; }
            set { _GenreName = value;}
        }
        public string CategoryCode
        {
            get { return _CategoryCode; }
            set { _CategoryCode = value;}
        }
        public string GenreCode
        {
            get { return _GenreCode; }
            set { _GenreCode = value;}
        }


        

        #endregion

    }
}
