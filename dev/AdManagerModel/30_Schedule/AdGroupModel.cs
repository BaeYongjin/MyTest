// ===============================================================================
// AdGroup Data Model for Charites Project
//
// AdGroupModel.cs
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
    public class AdGroupModel : BaseModel
    {

        // ��ȸ��
        private string _SearchKey       = null;		// �˻���


        // ��������
        private string _MediaCode		= null;		//��üID
        private string _MediaName		= null;		//��ü��
        private string _AdGroupCode		= null;		//����׷��ڵ�
        private string _AdGroupName		= null;		//����׷�� 
        private string _MenuType			= null;		//ä�θ޴�����
        private string _Comment			= null;		//�׷켳��
        private string _UseYn			= null;		//��뿩��
        private string _UseYnName			= null;		//��뿩�� �̸�

        // �����ȸ��
        private DataSet  _AdGroupDataSet;

        
        // �̵�� �޺� ��ȸ��
        private DataSet  _MediaDataSet;

        public AdGroupModel() : base () 
        {
            Init();
        }

        #region Public �޼ҵ�
        public void Init()
        {
            _SearchKey		= "";

            _MediaCode		= "";		//��üID
            _MediaName		= "";		//��ü��
            _AdGroupCode	= "";		//����׷��ڵ�
            _AdGroupName	= "";		//����׷�� 
            _MenuType		= "";	    //ä�θ޴�����
            _Comment		= "";		//�׷켳��
            _UseYn			= "";		//��뿩��
            _UseYnName			= "";	//��뿩���̸�

	

            _AdGroupDataSet = new DataSet();
             _MediaDataSet   = new DataSet();
        }

        #endregion

        #region  ������Ƽ 

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
