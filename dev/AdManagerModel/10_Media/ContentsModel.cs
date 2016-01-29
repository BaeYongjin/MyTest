// ===============================================================================
// Contents Data Model for Charites Project
//
// ContentsModel.cs
//
// 컨텐츠정보 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
// 2007.06.26 송명환 v1.0
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
    public class ContentsModel : BaseModel
    {

        // 조회용
        private string _SearchKey       = null;		// 검색어


        // 상세정보용
        private string _CheckYn         = null;	    // 체크여부
        private string _ContentKey      = null;	    // 컨텐츠 Key
        private string _ContentId       = null;		// 컨텐츠 아이디
        private string _Title           = null;		// 컨텐츠 명
        private string _ContentsState   = null;		// 컨텐츠 상태
        private string _RegDate         = null;		// 등록일자
        private string _Rate            = null;		// 등급
        private string _SubTitle		= null;		// Sub제목
        private string _OrgTitle		= null;		// 오리지널 장르
        private string _ModDt           = null;		// 최종수정일시
        
        

        
	
        // 목록조회용
        private DataSet  _ContentsDataSet;
	
        public ContentsModel() : base () 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchKey		= "";

            _ContentKey      =    "";   // 컨텐츠 Key
            _ContentId       =    "";   // 컨텐츠 아이디
            _Title           =    "";   // 컨텐츠 명
            _ContentsState   =    "";   // 컨텐츠 상태
            _RegDate         =    "";   // 등록일자
            _Rate           =    "";   // 등급
            _SubTitle		 =    "";   // Sub제목
            _OrgTitle		 =    "";   // 오리지널 장르
            _ModDt           =    "";   // 최종수정일시
	

            _ContentsDataSet = new DataSet();
        }

        #endregion

        #region  프로퍼티 

        public DataSet ContentsDataSet
        {
            get { return _ContentsDataSet;	}
            set { _ContentsDataSet = value;	}
        }

        
            public string CheckYn 
            {
                get { return _CheckYn;	}
                set { _CheckYn = value;	}
            }
        
        public string SearchKey 
        {
            get { return _SearchKey;	}
            set { _SearchKey = value;	}
        }
	

        public string ContentKey
        {
            get { return _ContentKey;		}
            set { _ContentKey = value;		}
        }

        public string ContentId 
        {
            get { return _ContentId;		}
            set { _ContentId = value;		}
        }

        public string Title
        {
            get { return _Title;		}
            set { _Title = value;	}
        }

        public string ContentsState
        {
            get { return _ContentsState;	}
            set { _ContentsState = value;	}
        }

        public string RegDate 
        {
            get { return _RegDate;		}
            set { _RegDate = value;		}
        }

        public string Rate
        {
            get { return _Rate; }
            set { _Rate = value;}
        }

        public string SubTitle 
        {
            get { return _SubTitle;		}
            set { _SubTitle = value;		}
        }

        public string OrgTitle 
        {
            get { return _OrgTitle;		}
            set { _OrgTitle = value;		}
        }

        public string ModDt 
        {
            get { return _ModDt;		}
            set { _ModDt = value;		}
        }

        #endregion

    }
}
