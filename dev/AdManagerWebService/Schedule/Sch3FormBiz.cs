using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Schedule
{
    /// <summary>
    /// ChooseAdScheduleBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class Sch3FormBiz : BaseBiz
    {
        public Sch3FormBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        #region [1] �޴� �����ȸ
		/// <summary>
		/// ī�װ�/�帣 ����� �����ɴϴ�(CSS��)
		/// </summary>
		/// <param name="header"></param>
		/// <param name="model"></param>
        public void GetGenreListCSS(HeaderModel header, Sch3FormModel	model)
        {
            try
            {
                _db.Open();
                StringBuilder sbQuery = new StringBuilder();
					
                // ��������
                sbQuery.Append(" SELECT  A.MediaCode " + "\n");
                sbQuery.Append("        ,B.MediaName  " + "\n");
                sbQuery.Append("        ,A.CategoryCode  " + "\n");
                sbQuery.Append("        ,dbo.ufnPadding('L',A.CategoryCode,'5',' ') + ' ' + C.CategoryName AS CategoryName  " + "\n");
                sbQuery.Append("        ,A.GenreCode  " + "\n");
                sbQuery.Append(" 		,dbo.ufnPadding('L',A.GenreCode,'5',' ')+ ' ' + D.GenreName AS GenreName           " + "\n");
                sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
                sbQuery.Append(" 			FROM    SchChoiceMenuDetail	x with(noLock)  " + "\n");
                sbQuery.Append(" 			inner   join ContractItem	y with(noLock)	on x.ItemNo = y.ItemNo and x.MediaCode = y.MediaCode and y.AdType='13'  " + "\n");
                sbQuery.Append(" 			WHERE   x.MediaCode = A.MediaCode   " + "\n");
                sbQuery.Append(" 			AND     x.GenreCode = A.GenreCode) AS AdCount   " + "\n");
                sbQuery.Append("		,'False'	as IsCheck " + "\n");
				sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
				sbQuery.Append(" 			FROM    SchChoiceMenuDetail	x with(noLock)  " + "\n");
				sbQuery.Append(" 			inner   join ContractItem	y with(noLock)	on x.ItemNo = y.ItemNo and x.MediaCode = y.MediaCode and y.AdType='13'  " + "\n");
				sbQuery.Append(" 			WHERE   x.MediaCode = A.MediaCode   " + "\n");
				sbQuery.Append(" 			AND     x.GenreCode = A.GenreCode   " + "\n");
				sbQuery.Append(" 			AND     x.ItemNo    = " + model.ItemNo + " ) AS AdFound1   " + "\n");
				sbQuery.Append("		,(	SELECT  COUNT(*)    " + "\n");
				sbQuery.Append("            FROM    SchChoiceChannelDetail	x with(noLock)   " + "\n");
				sbQuery.Append(" 			inner   join (	select MediaCode, ChannelNo " + "\n");
				sbQuery.Append(" 							from	ChannelSet with(noLock) " + "\n");
				sbQuery.Append(" 							where	MediaCode = 1 " + "\n");
				sbQuery.Append(" 							and		GenreCode = A.GenreCode " + "\n");
				sbQuery.Append(" 							group by MediaCode,ChannelNo)	y	on  x.MediaCode = y.MediaCode and x.ChannelNo = y.ChannelNo " + "\n");
				sbQuery.Append("			WHERE   x.MediaCode = A.MediaCode   " + "\n");
				sbQuery.Append(" 			and		x.ItemNo = " + model.ItemNo + " ) AS AdFound2   " + "\n");
				sbQuery.Append("		,(	SELECT  COUNT(*)    " + "\n");
				sbQuery.Append("            FROM    SchChoiceSeriesDetail	x with(noLock)   " + "\n");
				sbQuery.Append(" 			inner   join (	select MediaCode, ChannelNo, SeriesNo " + "\n");
				sbQuery.Append(" 							from	ChannelSet with(noLock) " + "\n");
				sbQuery.Append(" 							where	MediaCode = 1 " + "\n");
				sbQuery.Append(" 							and		GenreCode = A.GenreCode " + "\n");
				sbQuery.Append(" 							group by MediaCode,ChannelNo,SeriesNo )	y	on  x.MediaCode = y.MediaCode and x.ChannelNo = y.ChannelNo and  x.SeriesNo = y.SeriesNo " + "\n");
				sbQuery.Append("			WHERE   x.MediaCode = A.MediaCode   " + "\n");
				sbQuery.Append(" 			and		x.ItemNo = " + model.ItemNo + " ) AS AdFound3   " + "\n");
                sbQuery.Append(" FROM (	SELECT   a.MediaCode  " + "\n");
                sbQuery.Append(" 				,a.CategoryCode  " + "\n");
                sbQuery.Append(" 				,a.GenreCode  " + "\n");
                sbQuery.Append(" 		FROM ChannelSet a with(NoLock)  " + "\n");
                sbQuery.Append(" 		WHERE a.MediaCode = '1' " + "\n");
                sbQuery.Append(" 		GROUP BY a.MediaCode, a.CategoryCode, a.GenreCode  " + "\n");
                sbQuery.Append(" 	  ) A   " + "\n");
                sbQuery.Append("		INNER JOIN Media    B with(NoLock) ON (A.MediaCode    = B.MediaCode   )     " + "\n");
                sbQuery.Append(" 		INNER JOIN Category C with(NoLock) ON (A.CategoryCode = C.CategoryCode	AND C.CSSFlag = 'Y' AND C.Flag='Y')   " + "\n");
                sbQuery.Append("		INNER JOIN Genre    D with(NoLock) ON (A.GenreCode    = D.GenreCode   )   " + "\n");
                sbQuery.Append(" ORDER BY C.SortNo, C.CategoryCode,D.GenreCode  " + "\n");
			
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // ��� DataSet�� �𵨿� ����
				model.DsGenre	=	ds.Copy();
				model.ResultCnt	=	Utility.GetDatasetCount( model.DsGenre );
				model.ResultCD	=	"0000";
				model.ResultDesc=	"��ȸ�Ϸ�";	
            }
            catch(Exception ex)
            {
				model.ResultCD	=	"3000";
				model.ResultCnt	=	0;
				model.ResultDesc=	"ī�װ�/�帣���� ��ȸ�� �����߻�(CSS��)";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }



		/// <summary>
		/// ī�װ�/�帣 ����� �����ɴϴ�(��ü�����)
		/// </summary>
		/// <param name="header"></param>
		/// <param name="model"></param>
		public void GetGenreListTot(HeaderModel header, Sch3FormModel	model)
		{
			try
			{
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();
					
				// ��������
				sbQuery.Append(" SELECT  A.MediaCode " + "\n");
				sbQuery.Append("        ,B.MediaName  " + "\n");
				sbQuery.Append("        ,A.CategoryCode  " + "\n");
				sbQuery.Append("        ,dbo.ufnPadding('L',A.CategoryCode,'5',' ') + ' ' + C.CategoryName AS CategoryName  " + "\n");
				sbQuery.Append("        ,A.GenreCode  " + "\n");
				sbQuery.Append(" 		,dbo.ufnPadding('L',A.GenreCode,'5',' ')+ ' ' + D.GenreName AS GenreName           " + "\n");
				sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
				sbQuery.Append(" 			FROM    SchChoiceMenuDetail	x with(noLock)  " + "\n");
				//sbQuery.Append(" 			inner   join ContractItem	y with(noLock)	on x.ItemNo = y.ItemNo and x.MediaCode = y.MediaCode and y.AdType='13'  " + "\n");
				sbQuery.Append(" 			WHERE   x.MediaCode = A.MediaCode   " + "\n");
				sbQuery.Append(" 			AND     x.GenreCode = A.GenreCode) AS AdCount   " + "\n");
				sbQuery.Append("		,'False'	as IsCheck " + "\n");
				sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
				sbQuery.Append(" 			FROM    SchChoiceMenuDetail	x with(noLock)  " + "\n");
				//sbQuery.Append(" 			inner   join ContractItem	y with(noLock)	on x.ItemNo = y.ItemNo and x.MediaCode = y.MediaCode and y.AdType='13'  " + "\n");
				sbQuery.Append(" 			WHERE   x.MediaCode = A.MediaCode   " + "\n");
				sbQuery.Append(" 			AND     x.GenreCode = A.GenreCode   " + "\n");
				sbQuery.Append(" 			AND     x.ItemNo    = " + model.ItemNo + " ) AS AdFound1   " + "\n");
				sbQuery.Append("		,(	SELECT  COUNT(*)    " + "\n");
				sbQuery.Append("            FROM    SchChoiceChannelDetail	x with(noLock)   " + "\n");
				sbQuery.Append(" 			inner   join (	select MediaCode, ChannelNo " + "\n");
				sbQuery.Append(" 							from	ChannelSet with(noLock) " + "\n");
				sbQuery.Append(" 							where	MediaCode = 1 " + "\n");
				sbQuery.Append(" 							and		GenreCode = A.GenreCode " + "\n");
				sbQuery.Append(" 							group by MediaCode,ChannelNo)	y	on  x.MediaCode = y.MediaCode and x.ChannelNo = y.ChannelNo " + "\n");
				sbQuery.Append("			WHERE   x.MediaCode = A.MediaCode   " + "\n");
				sbQuery.Append(" 			and		x.ItemNo = " + model.ItemNo + " ) AS AdFound2   " + "\n");
				sbQuery.Append("		,(	SELECT  COUNT(*)    " + "\n");
				sbQuery.Append("            FROM    SchChoiceSeriesDetail	x with(noLock)   " + "\n");
				sbQuery.Append(" 			inner   join (	select MediaCode, ChannelNo, SeriesNo " + "\n");
				sbQuery.Append(" 							from	ChannelSet with(noLock) " + "\n");
				sbQuery.Append(" 							where	MediaCode = 1 " + "\n");
				sbQuery.Append(" 							and		GenreCode = A.GenreCode " + "\n");
				sbQuery.Append(" 							group by MediaCode,ChannelNo,SeriesNo )	y	on  x.MediaCode = y.MediaCode and x.ChannelNo = y.ChannelNo and  x.SeriesNo = y.SeriesNo " + "\n");
				sbQuery.Append("			WHERE   x.MediaCode = A.MediaCode   " + "\n");
				sbQuery.Append(" 			and		x.ItemNo = " + model.ItemNo + " ) AS AdFound3   " + "\n");
				sbQuery.Append(" FROM (	SELECT   a.MediaCode  " + "\n");
				sbQuery.Append(" 				,a.CategoryCode  " + "\n");
				sbQuery.Append(" 				,a.GenreCode  " + "\n");
				sbQuery.Append(" 		FROM ChannelSet a with(NoLock)  " + "\n");
				sbQuery.Append(" 		WHERE a.MediaCode = '1' " + "\n");
				sbQuery.Append(" 		GROUP BY a.MediaCode, a.CategoryCode, a.GenreCode  " + "\n");
				sbQuery.Append(" 	  ) A   " + "\n");
				sbQuery.Append("		INNER JOIN Media    B with(NoLock) ON (A.MediaCode    = B.MediaCode   )     " + "\n");
				sbQuery.Append(" 		INNER JOIN Category C with(NoLock) ON (A.CategoryCode = C.CategoryCode	AND C.Flag='Y')   " + "\n");
				sbQuery.Append("		INNER JOIN Genre    D with(NoLock) ON (A.GenreCode    = D.GenreCode   )   " + "\n");
				sbQuery.Append(" ORDER BY C.SortNo, C.CategoryCode,D.GenreCode  " + "\n");
			
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				model.DsGenre	=	ds.Copy();
				model.ResultCnt	=	Utility.GetDatasetCount( model.DsGenre );
				model.ResultCD	=	"0000";
				model.ResultDesc=	"��ȸ�Ϸ�";	
			}
			catch(Exception ex)
			{
				model.ResultCD	=	"3000";
				model.ResultCnt	=	0;
				model.ResultDesc=	"ī�װ�/�帣���� ��ȸ�� �����߻�(��ü��)";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}


		/// <summary>
		/// Ư���������� ��ȸ��
		/// �����̺��� Ʋ����
		/// </summary>
		/// <param name="header"></param>
		/// <param name="model"></param>
		public void GetGenreListDesign(HeaderModel header, Sch3FormModel	model)
		{
			try
			{
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();
					
				// ��������
				sbQuery.Append(" SELECT  A.MediaCode " + "\n");
				sbQuery.Append("        ,B.MediaName  " + "\n");
				sbQuery.Append("        ,A.CategoryCode  " + "\n");
				sbQuery.Append("        ,dbo.ufnPadding('L',A.CategoryCode,'5',' ') + ' ' + C.CategoryName AS CategoryName  " + "\n");
				sbQuery.Append("        ,A.GenreCode  " + "\n");
				sbQuery.Append(" 		,dbo.ufnPadding('L',A.GenreCode,'5',' ')+ ' ' + D.GenreName AS GenreName           " + "\n");
				sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
				sbQuery.Append(" 			FROM    SchChoiceMenuDetail	x with(noLock)  " + "\n");
				//sbQuery.Append(" 			inner   join ContractItem	y with(noLock)	on x.ItemNo = y.ItemNo and x.MediaCode = y.MediaCode and y.AdType='13'  " + "\n");
				sbQuery.Append(" 			WHERE   x.MediaCode = A.MediaCode   " + "\n");
				sbQuery.Append(" 			AND     x.GenreCode = A.GenreCode) AS AdCount   " + "\n");
				sbQuery.Append("		,'False'	as IsCheck " + "\n");
				sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
				sbQuery.Append(" 			FROM    SchDesignatedDetail	x with(noLock)  " + "\n");
				sbQuery.Append(" 			WHERE   x.ItemNo	= " + model.ItemNo	  + "\n");
				sbQuery.Append(" 			AND     x.MediaCode = A.MediaCode		" + "\n");
				sbQuery.Append(" 			AND     x.Category	= A.CategoryCode	" + "\n");
				sbQuery.Append(" 			AND     x.Genre		= A.GenreCode		" + "\n");
				sbQuery.Append(" 			AND     x.Channel	= 0                 " + "\n");
				sbQuery.Append(" 			AND     x.Series    = 0 ) AS AdFound1   " + "\n");
				sbQuery.Append("		,(	SELECT  COUNT(*)    " + "\n");
				sbQuery.Append("            FROM    SchDesignatedDetail	x with(noLock)   " + "\n");
				sbQuery.Append(" 			inner   join (	select MediaCode,CategoryCode, GenreCode, ChannelNo " + "\n");
				sbQuery.Append(" 							from	ChannelSet with(noLock) " + "\n");
				sbQuery.Append(" 							where	MediaCode = 1 " + "\n");
				sbQuery.Append(" 							and		CategoryCode = A.CategoryCode " + "\n");
				sbQuery.Append(" 							and		GenreCode = A.GenreCode " + "\n");
				sbQuery.Append(" 							group by MediaCode,CategoryCode, GenreCode,ChannelNo)	y	on  x.MediaCode = y.MediaCode and x.Category=y.CategoryCode and x.Genre = y.GenreCode and x.Channel = y.ChannelNo and x.Series=0 " + "\n");
				sbQuery.Append("			WHERE   x.MediaCode = A.MediaCode   " + "\n");
				sbQuery.Append(" 			and		x.ItemNo = " + model.ItemNo + " ) AS AdFound2   " + "\n");
				sbQuery.Append("		,(	SELECT  COUNT(*)    " + "\n");
				sbQuery.Append("            FROM    SchDesignatedDetail	x with(noLock)   " + "\n");
				sbQuery.Append(" 			inner   join (	select MediaCode,CategoryCode, GenreCode, ChannelNo,SeriesNo " + "\n");
				sbQuery.Append(" 							from	ChannelSet with(noLock) " + "\n");
				sbQuery.Append(" 							where	MediaCode = 1 " + "\n");
				sbQuery.Append(" 							and		CategoryCode = A.CategoryCode " + "\n");
				sbQuery.Append(" 							and		GenreCode = A.GenreCode " + "\n");
				sbQuery.Append(" 							group by MediaCode,CategoryCode, GenreCode,ChannelNo,SeriesNo)	y	on  x.MediaCode = y.MediaCode and x.Category=y.CategoryCode and x.Genre = y.GenreCode and x.Channel = y.ChannelNo and x.Series= y.SeriesNo " + "\n");
				sbQuery.Append("			WHERE   x.MediaCode = A.MediaCode   " + "\n");
				sbQuery.Append(" 			and		x.ItemNo = " + model.ItemNo + " ) AS AdFound3   " + "\n");
				sbQuery.Append(" FROM (	SELECT   a.MediaCode  " + "\n");
				sbQuery.Append(" 				,a.CategoryCode  " + "\n");
				sbQuery.Append(" 				,a.GenreCode  " + "\n");
				sbQuery.Append(" 		FROM ChannelSet a with(NoLock)  " + "\n");
				sbQuery.Append(" 		WHERE a.MediaCode = '1' " + "\n");
				sbQuery.Append(" 		GROUP BY a.MediaCode, a.CategoryCode, a.GenreCode  " + "\n");
				sbQuery.Append(" 	  ) A   " + "\n");
				sbQuery.Append("		INNER JOIN Media    B with(NoLock) ON (A.MediaCode    = B.MediaCode   )     " + "\n");
				sbQuery.Append(" 		INNER JOIN Category C with(NoLock) ON (A.CategoryCode = C.CategoryCode	AND C.Flag='Y')   " + "\n");
				sbQuery.Append("		INNER JOIN Genre    D with(NoLock) ON (A.GenreCode    = D.GenreCode   )   " + "\n");
				sbQuery.Append(" ORDER BY C.SortNo, C.CategoryCode,D.GenreCode  " + "\n");
			
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �𵨿� ����
				model.DsGenre	=	ds.Copy();
				model.ResultCnt	=	Utility.GetDatasetCount( model.DsGenre );
				model.ResultCD	=	"0000";
				model.ResultDesc=	"��ȸ�Ϸ�";	
			}
			catch(Exception ex)
			{
				model.ResultCD	=	"3000";
				model.ResultCnt	=	0;
				model.ResultDesc=	"ī�װ�/�帣���� ��ȸ�� �����߻�(��ü��)";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

        #endregion

		#region [2] ä�θ����ȸ
		
		/// <summary>
		/// ä�μ¸����ȸ
		/// �޴�/ä����-�޴����ý� �ҷ��� ä�θ���Ʈ������
		/// </summary>
		public void GetChannelList(HeaderModel header, Sch3FormModel	model)
		{
			try
			{   // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();
		
				sbQuery.Append(" SELECT A.MediaCode        " + "\n");
				sbQuery.Append("        ,E.MediaName	      " + "\n");
				sbQuery.Append("        ,A.CategoryCode	  " + "\n");
				sbQuery.Append("        ,B.CategoryName	  " + "\n");
				sbQuery.Append("        ,A.GenreCode	      " + "\n");
				sbQuery.Append("        ,C.GenreName        " + "\n");
				sbQuery.Append("        ,A.ChannelNo        " + "\n");
				sbQuery.Append("        ,A.TotalSeries      " + "\n");
				sbQuery.Append("        ,D.Title	          " + "\n");


				if ( model.DataType == 13 )
				{
					sbQuery.Append("        ,(	SELECT COUNT(*) " + "\n");
					sbQuery.Append(" 					FROM SchChoiceChannelDetail x with(NoLock)" + "\n");
					sbQuery.Append(" 					inner   join ContractItem   y with(noLock)  on x.ItemNo = y.ItemNo and x.MediaCode = y.MediaCode and y.AdType='13'" + "\n");
					sbQuery.Append("           WHERE   x.MediaCode = A.MediaCode" + "\n");
					sbQuery.Append("           AND     x.ChannelNo = A.ChannelNo) AS AdCount" + "\n");
				}
				else
				{
					sbQuery.Append("        ,(	SELECT COUNT(*) " + "\n");
					sbQuery.Append(" 					FROM SchChoiceChannelDetail x with(NoLock)" + "\n");
					sbQuery.Append("           WHERE   x.MediaCode = A.MediaCode" + "\n");
					sbQuery.Append("           AND     x.ChannelNo = A.ChannelNo) AS AdCount" + "\n");
				}

				sbQuery.Append("        ,A.Hits                 " + "\n");
				sbQuery.Append("        ,CASE WHEN ProdTypeCnt > 0 THEN 'PPx' ELSE '' END AS ProdType " + "\n");
				sbQuery.Append("        ,A.Rate " + "\n");
                sbQuery.Append("		,'False'	as IsCheck " + "\n");

				if ( model.DataType == 13 )
				{
					sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
					sbQuery.Append(" 			FROM    SchChoiceChannelDetail	x with(noLock)  " + "\n");
					sbQuery.Append("           WHERE   x.MediaCode = A.MediaCode " + "\n");
					sbQuery.Append(" 			AND    x.ChannelNo = A.ChannelNo " + "\n");
					sbQuery.Append(" 			AND    x.ItemNo    = " + model.ItemNo + " ) AS AdFound2   " + "\n");
				}
				else
				{
					sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
					sbQuery.Append(" 			FROM    SchDesignatedDetail	x with(noLock)  " + "\n");
					sbQuery.Append("           WHERE   x.MediaCode	= A.MediaCode " + "\n");
					sbQuery.Append(" 			AND    x.Category	= A.CategoryCode " + "\n");
					sbQuery.Append(" 			AND    x.Genre		= A.GenreCode " + "\n");
					sbQuery.Append(" 			AND    x.Channel	= A.ChannelNo " + "\n");
					sbQuery.Append(" 			AND    x.Series		= 0 " + "\n");
					sbQuery.Append(" 			AND    x.ItemNo		= " + model.ItemNo + " ) AS AdFound2   " + "\n");
				}

				if ( model.DataType == 13 )
				{
					sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
					sbQuery.Append(" 			FROM    SchChoiceSeriesDetail	x with(noLock)  " + "\n");
					sbQuery.Append("           WHERE   x.MediaCode = A.MediaCode " + "\n");
					sbQuery.Append(" 			AND    x.ChannelNo = A.ChannelNo " + "\n");
					sbQuery.Append(" 			AND    x.ItemNo    = " + model.ItemNo + " ) AS AdFound3   " + "\n");
				}
				else
				{
					sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
					sbQuery.Append(" 			FROM    SchDesignatedDetail	x with(noLock)  " + "\n");
					sbQuery.Append("           WHERE   x.MediaCode = A.MediaCode " + "\n");
					sbQuery.Append(" 			AND    x.Category	= A.CategoryCode " + "\n");
					sbQuery.Append(" 			AND    x.Genre		= A.GenreCode " + "\n");
					sbQuery.Append(" 			AND    x.Channel	= A.ChannelNo " + "\n");
					sbQuery.Append(" 			AND    x.Series		> 0 " + "\n");
					sbQuery.Append(" 			AND    x.ItemNo     = " + model.ItemNo + " ) AS AdFound3   " + "\n");
				}

				sbQuery.Append("   FROM (" + "\n");
				sbQuery.Append("        SELECT TA.MediaCode" + "\n");
				sbQuery.Append("              ,TA.CategoryCode" + "\n");
				sbQuery.Append("              ,TA.GenreCode" + "\n");
				sbQuery.Append("              ,TA.ChannelNo" + "\n");
				sbQuery.Append("              ,MIN(TA.SeriesNo) AS SeriesNo" + "\n");
				sbQuery.Append("              ,SUM(TC.Hits) AS Hits" + "\n");
				sbQuery.Append("              ,SUM(CASE WHEN ProdType IS NOT NULL AND ProdType <> '' THEN 1 ELSE 0 END) AS ProdTypeCnt" + "\n");
				sbQuery.Append("              ,MAX(TC.Rate)  as Rate" + "\n");
				sbQuery.Append("              ,Count(*)  as TotalSeries" + "\n");
				sbQuery.Append("          FROM ChannelSet TA with(NoLock)" + "\n");
				sbQuery.Append("               INNER JOIN Channel  TB with(NoLock) ON (TA.MediaCode = TB.MediaCode AND TA.ChannelNo = TB.ChannelNo AND TA.SeriesNO = TB.SeriesNo)" + "\n");
				sbQuery.Append("               INNER JOIN Contents TC with(NoLock) ON (TB.ContentID = TC.ContentID AND TC.ContentsState <='70' )" + "\n");
				sbQuery.Append("         WHERE TA.MediaCode    = '" + model.Media + "'" + "\n");
				sbQuery.Append("           AND TA.CategoryCode = '" + model.Category + "'" + "\n");
				sbQuery.Append("           AND TA.GenreCode    = '" + model.Genre	 + "'" + "\n");
				sbQuery.Append("         GROUP BY TA.MediaCode, TA.CategoryCode, TA.GenreCode, TA.ChannelNo" + "\n");
				sbQuery.Append("        ) A INNER JOIN Category B with(NoLock) ON A.MediaCode = B.MediaCode AND A.CategoryCode = B.CategoryCode" + "\n");
				sbQuery.Append("            INNER JOIN Genre    C with(NoLock) ON A.MediaCode = C.MediaCode AND A.GenreCode    = C.GenreCode" + "\n");
				sbQuery.Append("            INNER JOIN Channel  D with(NoLock) ON A.MediaCode = D.MediaCode AND A.ChannelNo    = D.ChannelNo AND A.SeriesNO = D.SeriesNo" + "\n");
				sbQuery.Append("            INNER JOIN Media    E with(NoLock) ON A.MediaCode = E.MediaCode" + "\n");
				sbQuery.Append("  ORDER BY AdFound2 desc ,AdFound3 desc,A.ChannelNo asc;" + "\n");

				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				model.DsChannel =	ds.Copy();
				model.ResultCnt	=	Utility.GetDatasetCount( model.DsChannel );
				model.ResultCD	=	"0000";
			}
			catch(Exception ex)
			{
				model.ResultCD = "3000";
				model.ResultDesc = "ä������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}
		#endregion

		#region [3] �ø�������ȸ
		
		/// <summary>
		/// ä�μ¸����ȸ
		/// �޴�/ä����-�޴����ý� �ҷ��� ä�θ���Ʈ������
		/// </summary>
		public void GetSeriesList(HeaderModel header, Sch3FormModel	model)
		{
			try
			{   // �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				StringBuilder sbQuery = new StringBuilder();
		
				sbQuery.Append(" select	 a.SeriesNo " + "\n");
				sbQuery.Append(" 		,c.SubTitle " + "\n");
				sbQuery.Append(" 		,c.ContentsState as cState " + "\n");
				sbQuery.Append(" 		,c.Rate " + "\n");
				sbQuery.Append(" 		,c.Hits " + "\n");
				sbQuery.Append(" 		,c.AdUse " + "\n");
                sbQuery.Append("		,'False'	as IsCheck " + "\n");
				sbQuery.Append("        ,(	SELECT COUNT(*) " + "\n");
				sbQuery.Append(" 					FROM SchChoiceSeriesDetail x with(NoLock)" + "\n");
				sbQuery.Append(" 					inner   join ContractItem   y with(noLock)  on x.ItemNo = y.ItemNo and x.MediaCode = y.MediaCode and y.AdType='13'" + "\n");
				sbQuery.Append("           WHERE   x.MediaCode = A.MediaCode" + "\n");
				sbQuery.Append("           AND     x.ChannelNo = A.ChannelNo and x.SeriesNo = a.SeriesNo ) AS AdCount" + "\n");

				if( model.DataType == 13 )
				{
					sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
					sbQuery.Append(" 			FROM    SchChoiceSeriesDetail	x with(noLock)  " + "\n");
					sbQuery.Append(" 					inner   join ContractItem   y with(noLock)  on x.ItemNo = y.ItemNo and x.MediaCode = y.MediaCode and y.AdType='13'" + "\n");
					sbQuery.Append("           WHERE   x.MediaCode = A.MediaCode " + "\n");
					sbQuery.Append(" 			AND    x.ChannelNo = A.ChannelNo and x.SeriesNo = a.SeriesNo  " + "\n");
					sbQuery.Append(" 			AND    x.ItemNo    = " + model.ItemNo + " ) AS AdFound   " + "\n");
				}
				else
				{
					sbQuery.Append("        ,(	SELECT  COUNT(*)   " + "\n");
					sbQuery.Append(" 			FROM    SchDesignatedDetail	x with(noLock)  " + "\n");
					sbQuery.Append("           WHERE   x.MediaCode = A.MediaCode " + "\n");
					sbQuery.Append(" 			AND    x.Category	= A.CategoryCode " + "\n");
					sbQuery.Append(" 			AND    x.Genre		= A.GenreCode " + "\n");
					sbQuery.Append(" 			AND    x.Channel	= A.ChannelNo " + "\n");
					sbQuery.Append(" 			AND    x.Series		= A.SeriesNo " + "\n");
					sbQuery.Append(" 			AND    x.ItemNo     = " + model.ItemNo + " ) AS AdFound   " + "\n");
				}

				sbQuery.Append(" from	ChannelSet	a		with(noLock) " + "\n");
				sbQuery.Append(" inner join Channel b	with(noLock)	on b.MediaCode = a.MediaCode and b.ChannelNo = a.ChannelNo and b.SeriesNo = a.SeriesNo " + "\n");
				sbQuery.Append(" inner join Contents c	with(noLock)	on c.ContentId = b.ContentId and c.ContentsState between '30' and '70' " + "\n");
				sbQuery.Append(" where	a.MediaCode     = " + model.Media + "\n");
				sbQuery.Append(" and	a.CategoryCode	= " + model.Category + "\n");
				sbQuery.Append(" and	a.GenreCode     = " + model.Genre + "\n");
				sbQuery.Append(" and	a.ChannelNo		= " + model.Channel + "\n");
				sbQuery.Append(" order by a.SeriesNo desc " + "\n");
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� ��ü����籤���ָ𵨿� ����
				model.DsSeries =	ds.Copy();
				model.ResultCnt	=	Utility.GetDatasetCount( model.DsSeries );
				model.ResultCD	=	"0000";
			}
			catch(Exception ex)
			{
				model.ResultCD = "3000";
				model.ResultDesc = "ä������ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}
		#endregion
    }
}