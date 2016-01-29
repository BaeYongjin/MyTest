using System;
using System.Data;

using WinFramework.Base;
using System.Collections.Generic;
using System.Collections;

namespace AdManagerModel
{
    /// <summary>
    /// SchRecommModel에 대한 요약 설명입니다.
    /// </summary>
    public class SchRecommModel : BaseModel
    {
		// 조회용
        private string _SearchStartDay         = null;
        private string _SearchEndDay           = null;
        private ArrayList _SearchSexCode          = null;   // SummaryType 4 SummaryCode 남성 1, 여성 2, 기타 3
        private ArrayList _SearchCategoryCode     = null;   // Category 검색용 카테고리 코드
        private ArrayList _SearchTargetRegionCode = null;   // SummaryType 5 TargetRegion 검색용 지역 코드
        private ArrayList _SearchAgeCode          = null;   // SummaryType 3 SummaryCode 연령대 1000(19세이하), 2000(20대), 3000(30대), 4000(40대), 5000(50대), 6000(60세 이상)
 
		// 목록조회용
        private DataSet _SchRecommDataSet;

        public SchRecommModel()
            : base() 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
            _SearchStartDay         = "";
            _SearchEndDay           = "";
            _SearchSexCode          = new ArrayList();
            _SearchCategoryCode     = new ArrayList();
            _SearchTargetRegionCode = new ArrayList();
            _SearchAgeCode          = new ArrayList();

            _SchRecommDataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

        public DataSet SchRecommDataSet
        {
            get { return _SchRecommDataSet; }
            set { _SchRecommDataSet = value; }
        }

        public string SearchStartDay
        {
            get { return _SearchStartDay; }
            set { _SearchStartDay = value; }
        }

        public string SearchEndDay
        {
            get { return _SearchEndDay; }
            set { _SearchEndDay = value; }
        }

        public ArrayList SearchSexCode
        {
            get { return _SearchSexCode; }
            set { _SearchSexCode = value; }
        }

        public ArrayList SearchCategoryCode 
		{
			get { return _SearchCategoryCode;	}
			set { _SearchCategoryCode = value;	}
		}

        public ArrayList SearchTargetRegionCode
        {
            get { return _SearchTargetRegionCode; }
            set { _SearchTargetRegionCode = value; }
        }

        public ArrayList SearchAgeCode
        {
            get { return _SearchAgeCode; }
            set { _SearchAgeCode = value; }
        }

		#endregion

    }
}
