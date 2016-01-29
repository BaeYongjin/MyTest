// ===============================================================================
// Contents Data Model for Charites Project
//
// ContentsModel.cs
//
// ���������� Ŭ������ �����մϴ�. 
//
// ===============================================================================
// Release history
// 2007.06.26 �۸�ȯ v1.0
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
    public class ContentsModel : BaseModel
    {

        // ��ȸ��
        private string _SearchKey       = null;		// �˻���


        // ��������
        private string _CheckYn         = null;	    // üũ����
        private string _ContentKey      = null;	    // ������ Key
        private string _ContentId       = null;		// ������ ���̵�
        private string _Title           = null;		// ������ ��
        private string _ContentsState   = null;		// ������ ����
        private string _RegDate         = null;		// �������
        private string _Rate            = null;		// ���
        private string _SubTitle		= null;		// Sub����
        private string _OrgTitle		= null;		// �������� �帣
        private string _ModDt           = null;		// ���������Ͻ�
        
        

        
	
        // �����ȸ��
        private DataSet  _ContentsDataSet;
	
        public ContentsModel() : base () 
        {
            Init();
        }

        #region Public �޼ҵ�
        public void Init()
        {
            _SearchKey		= "";

            _ContentKey      =    "";   // ������ Key
            _ContentId       =    "";   // ������ ���̵�
            _Title           =    "";   // ������ ��
            _ContentsState   =    "";   // ������ ����
            _RegDate         =    "";   // �������
            _Rate           =    "";   // ���
            _SubTitle		 =    "";   // Sub����
            _OrgTitle		 =    "";   // �������� �帣
            _ModDt           =    "";   // ���������Ͻ�
	

            _ContentsDataSet = new DataSet();
        }

        #endregion

        #region  ������Ƽ 

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
