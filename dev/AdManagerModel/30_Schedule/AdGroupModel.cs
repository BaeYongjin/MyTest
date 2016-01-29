// ===============================================================================
// AdGroup Data Model for Charites Project
//
// AdGroupModel.cs
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
    public class AdGroupModel : BaseModel
    {

        // 조회용
        private string _SearchKey       = null;		// 검색어


        // 상세정보용
        private string _MediaCode		= null;		//매체ID
        private string _MediaName		= null;		//매체명
        private string _AdGroupCode		= null;		//광고그룹코드
        private string _AdGroupName		= null;		//광고그룹명 
        private string _MenuType			= null;		//채널메뉴구분
        private string _Comment			= null;		//그룹설명
        private string _UseYn			= null;		//사용여부
        private string _UseYnName			= null;		//사용여부 이름

        // 목록조회용
        private DataSet  _AdGroupDataSet;

        
        // 미디어 콤보 조회용
        private DataSet  _MediaDataSet;

        public AdGroupModel() : base () 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchKey		= "";

            _MediaCode		= "";		//매체ID
            _MediaName		= "";		//매체명
            _AdGroupCode	= "";		//광고그룹코드
            _AdGroupName	= "";		//광고그룹명 
            _MenuType		= "";	    //채널메뉴구분
            _Comment		= "";		//그룹설명
            _UseYn			= "";		//사용여부
            _UseYnName			= "";	//사용여부이름

	

            _AdGroupDataSet = new DataSet();
             _MediaDataSet   = new DataSet();
        }

        #endregion

        #region  프로퍼티 

        public DataSet AdGroupDataSet
        {
            get { return _AdGroupDataSet;	}
            set { _AdGroupDataSet = value;	}
        }
        public DataSet MediaDataSet
        {
            get { return _MediaDataSet;	}
            set { _MediaDataSet = value;	}
        }

        public string SearchKey 
        {
            get { return _SearchKey;	}
            set { _SearchKey = value;	}
        }
	
        public string MediaCode
        {
            get { return _MediaCode; }
            set { _MediaCode = value;}
        }
        public string MediaName
        {
            get { return _MediaName; }
            set { _MediaName = value;}
        }

        public string AdGroupCode 
        {
            get { return _AdGroupCode;		}
            set { _AdGroupCode = value;		}
        }

        public string AdGroupName
        {
            get { return _AdGroupName;		}
            set { _AdGroupName= value;	}
        }
        public string MenuType 
        {
            get { return _MenuType;	}
            set { _MenuType = value;	}
        }
        public string Comment 
        {
            get { return _Comment;	}
            set { _Comment = value;	}
        }
        public string UseYn 
        {
            get { return _UseYn;	}
            set { _UseYn = value;	}
        }
        public string UseYnName 
        {
            get { return _UseYnName;	}
            set { _UseYnName = value;	}
        }
        
        


        #endregion

    }
}
