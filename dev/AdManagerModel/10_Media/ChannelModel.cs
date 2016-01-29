// ===============================================================================
// Channel Data Model for Charites Project
//
// ChannelModel.cs
//
// ���������� Ŭ������ �����մϴ�. 
//
// ===============================================================================
// Release history
// 2007.06.06 �۸�ȯ v1.0
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
    public class ChannelModel : BaseModel
    {

        // ��ȸ��
        private string _SearchKey       = null;		// �˻���


        // ��������
        private string _MediaCode       = null;		// ��ü�ڵ�
        private string _MediaName       = null;		// ��ü��Ī
        private string _ChannelNo       = null;		// ������ ���
        private string _Title           = null;		// ��������
        private string _SeriesNo		= null;		// �ø��� ���
        private string _ContentId       = null;		// ������ ���̵�
        private string _TotalSeries	    = null;		// �ø������
        private string _ModDt           = null;		// ���������Ͻ�
        private string _CheckYn         = null;		// üũ����
        
        private string _svcId = null;               // ���񽺾��̵�
        private string _ch_no = null;               // ä�ι�ȣ
        private string _gnr_cod = null;             // �帣�ڵ� 
        private string _useYn = null;               // �������
        private string _adYn = null;                // ������������
        private string _adRate = null;              // ������
        private string _adnRate = null;             // adNetwork ������

        // �����ȸ��
        private DataSet  _ChannelDataSet;

        // �̵�� �޺� ��ȸ��
        private DataSet  _MediaDataSet;
	
        public ChannelModel() : base () 
        {
            Init();
        }

        #region Public �޼ҵ�
        public void Init()
        {
            _SearchKey		= "";

            _MediaCode       = "";		// ��ü�ڵ�
            _MediaName       = "";
            _ChannelNo       = "";		// ä���ڵ�
            _Title           = "";      // ��������
            _SeriesNo        = "";		// �ø��� ��ȣ
            _ContentId       = "";		// ������ ���̵�
            _TotalSeries	 = "";		// �ø��� ���
            _ModDt           = "";		// �������
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

        #region  ������Ƽ 

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
