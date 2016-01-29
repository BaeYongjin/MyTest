// ===============================================================================
// Channel Data Model for Charites Project
//
// ChannelModel.cs
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
    public class ChannelModel : BaseModel
    {

        // 조회용
        private string _SearchKey       = null;		// 검색어


        // 상세정보용
        private string _MediaCode       = null;		// 매체코드
        private string _MediaName       = null;		// 매체명칭
        private string _ChannelNo       = null;		// 컨텐츠 등급
        private string _Title           = null;		// 컨텐츠명
        private string _SeriesNo		= null;		// 시리즈 편수
        private string _ContentId       = null;		// 컨텐츠 아이디
        private string _TotalSeries	    = null;		// 시리즈편수
        private string _ModDt           = null;		// 최종수정일시
        private string _CheckYn         = null;		// 체크여부
        
        private string _svcId = null;               // 서비스아이디
        private string _ch_no = null;               // 채널번호
        private string _gnr_cod = null;             // 장르코드 
        private string _useYn = null;               // 사용유무
        private string _adYn = null;                // 광고적용유무
        private string _adRate = null;              // 편성비율
        private string _adnRate = null;             // adNetwork 편성비율

        // 목록조회용
        private DataSet  _ChannelDataSet;

        // 미디어 콤보 조회용
        private DataSet  _MediaDataSet;
	
        public ChannelModel() : base () 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchKey		= "";

            _MediaCode       = "";		// 매체코드
            _MediaName       = "";
            _ChannelNo       = "";		// 채널코드
            _Title           = "";      // 컨텐츠명
            _SeriesNo        = "";		// 시리즈 번호
            _ContentId       = "";		// 컨텐츠 아이디
            _TotalSeries	 = "";		// 시리즈 편수
            _ModDt           = "";		// 등록일자
            _CheckYn         = "";

            _svcId = "";
            _ch_no = "";
            _useYn = "";
            _adYn = "";
            _adRate = "";
            _adnRate = "";

            _ChannelDataSet = new DataSet();
            _MediaDataSet   = new DataSet();
        }

        #endregion

        #region  프로퍼티 

        public DataSet ChannelDataSet
        {
            get { return _ChannelDataSet;	}
            set { _ChannelDataSet = value;	}
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
	

        public string ContentId 
        {
            get { return _ContentId;		}
            set { _ContentId = value;		}
        }

        public string MediaCode
        {
            get { return _MediaCode;		}
            set { _MediaCode= value;	}
        }

        public string MediaName
        {
            get { return _MediaName;		}
            set { _MediaName= value;	}
        }

        public string ChannelNo
        {
            get { return _ChannelNo; }
            set { _ChannelNo = value;}
        }

        public string Title
        {
            get { return _Title; }
            set { _Title = value;}
        }

        public string SeriesNo 
        {
            get { return _SeriesNo;	}
            set { _SeriesNo = value;	}
        }


        public string ModDt
        {
            get { return _ModDt;		}
            set { _ModDt= value;		}
        }

        public string TotalSeries 
        {
            get { return _TotalSeries;		}
            set { _TotalSeries = value;		}
        }

        public string CheckYn 
        {
            get { return _CheckYn;		}
            set { _CheckYn = value;		}
        }

        public string ServiceID
        {
            get { return _svcId; }
            set { _svcId = value; }
        }
        public string GenreCode
        {
            get { return _gnr_cod; }
            set { _gnr_cod = value; }
        }
        public string ChannelNumber
        {
            get { return _ch_no; }
            set { _ch_no = value; }
        }
        public string UseYn
        {
            get { return _useYn; }
            set { _useYn = value; }
        }
        public string AdYn
        {
            get{return _adYn;}
            set{_adYn = value;}
        }
        public string AdRate
        {
            get { return _adRate; }
            set { _adRate = value; }
        }
        public string AdnRate
        {
            get { return _adnRate; }
            set { _adnRate = value; }
        }

        #endregion

    }
}
