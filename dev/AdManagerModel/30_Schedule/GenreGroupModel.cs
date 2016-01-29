// ===============================================================================
// GenreGroup Data Model for Charites Project
//
// GenreGroupModel.cs
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
    public class GenreGroupModel : AdGroupModel
    {


        // 상세정보용
        private string _GenreCode		= null;		//장르코드
        private string _GenreName		= null;		//장르명
   
	
        // 목록조회용
        private DataSet  _GenreGroupDataSet;

        //팝업목록 조회용
        private DataSet  _GenreGroup_pDataSet;

        

        public GenreGroupModel() : base () 
        {
            InitGenre();
        }

        #region Public 메소드
        public void InitGenre()
        {
            _GenreCode       = "";	  //장르코드
            _GenreName       = "";	  //장르이름

            _GenreGroupDataSet = new DataSet();
            _GenreGroup_pDataSet = new DataSet();
        }

        #endregion

        #region  프로퍼티 

        public DataSet GenreGroupDataSet
        {
            get { return _GenreGroupDataSet;	}
            set { _GenreGroupDataSet = value;	}
        }
        public DataSet GenreGroup_pDataSet
        {
            get { return _GenreGroup_pDataSet;	}
            set { _GenreGroup_pDataSet = value;	}
        }





        public string GenreCode
        {
            get { return _GenreCode; }
            set { _GenreCode = value;}
        }
        public string GenreName
        {
            get { return _GenreName; }
            set { _GenreName = value;}
        }

        

        #endregion

    }
}
