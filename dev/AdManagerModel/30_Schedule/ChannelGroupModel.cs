// ===============================================================================
// ChannelGroup Data Model for Charites Project
//
// ChannelGroupModel.cs
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
    public class ChannelGroupModel : AdGroupModel
    {


        // ��������
        private string _ChannelNo		= null;		//�帣�ڵ�
        private string _GenreName		= null;		//�帣��
        private string _CategoryCode    = null;     //ī�װ��ڵ�	
        private string _GenreCode       = null;	    //�帣�ڵ�
   
	
        // �����ȸ��
        private DataSet  _ChannelGroupDataSet;

        //�˾���� ��ȸ��
        private DataSet  _ChannelGroup_pDataSet;

        

        public ChannelGroupModel() : base () 
        {
            InitGenre();
        }

        #region Public �޼ҵ�
        public void InitGenre()
        {
            _ChannelNo       = "";	  //�帣�ڵ�
            _GenreName       = "";	  //�帣�̸�
            _CategoryCode    = "";    //ī�װ��ڵ�
            _GenreCode       = "";    //�帣�ڵ�


            _ChannelGroupDataSet = new DataSet();
            _ChannelGroup_pDataSet = new DataSet();
        }

        #endregion

        #region  ������Ƽ 

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
