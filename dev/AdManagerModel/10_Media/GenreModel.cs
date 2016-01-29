using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// GenreModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class GenreModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey       = null;		// �˻���
		private string _SearchGenreLevel = null;		// �˻�����

		// ��������
		private string _MediaCode       = null;		// ����� ���̵�
        private string _CategoryCode    = null;
		private string _GenreCode       = null;		// ����� ���̵�
		private string _GenreName       = null;		// ����� ��		
		private string _ModDt           = null;		// ����� ��å����

		// �����ȸ��
		private DataSet  _UserDataSet;

		public GenreModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey		= "";
			_SearchGenreLevel = "";

			_MediaCode			= "";
			_GenreCode			= "";
			_GenreName		= "";			
			_ModDt		= "";
			            
			_UserDataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet UserDataSet
		{
			get { return _UserDataSet;	}
			set { _UserDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchGenreLevel 
		{
			get { return _SearchGenreLevel;	}
			set { _SearchGenreLevel = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

        public string CategoryCode
        {
            get
            {
                return _CategoryCode;
            }
            set
            {
                _CategoryCode = value;
            }
        }

		public string GenreCode 
		{
			get { return _GenreCode;		}
			set { _GenreCode = value;		}
		}

		public string GenreName 
		{
			get { return _GenreName;		}
			set { _GenreName = value;	}
		}
		
		public string ModDt 
		{
			get { return _ModDt;	}
			set { _ModDt = value;	}
		}
		
		#endregion

	}
}
