// ===============================================================================
// GenreGroup Data Model for Charites Project
//
// GenreGroupModel.cs
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
    public class GenreGroupModel : AdGroupModel
    {


        // ��������
        private string _GenreCode		= null;		//�帣�ڵ�
        private string _GenreName		= null;		//�帣��
   
	
        // �����ȸ��
        private DataSet  _GenreGroupDataSet;

        //�˾���� ��ȸ��
        private DataSet  _GenreGroup_pDataSet;

        

        public GenreGroupModel() : base () 
        {
            InitGenre();
        }

        #region Public �޼ҵ�
        public void InitGenre()
        {
            _GenreCode       = "";	  //�帣�ڵ�
            _GenreName       = "";	  //�帣�̸�

            _GenreGroupDataSet = new DataSet();
            _GenreGroup_pDataSet = new DataSet();
        }

        #endregion

        #region  ������Ƽ 

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
