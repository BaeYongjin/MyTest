using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Media
{
	/// <summary>
	/// MediaMenuSetBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MediaMenuSetBiz : BaseBiz  
	{
        public MediaMenuSetBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// ������������ ���Ǵ� ī�װ������� �����´�
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GetCategoryList(HeaderModel header, MediaMenuSetModel data)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryList() Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" select  a.CategoryCode" + "\n");
                sbQuery.Append("        ,a.CategoryName" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = a.CategoryCode and b.MenuLevel2 = 0 and Channel = 0 and UseYN='Y' and AdSlotCode='" + data.SearchAdType + "') as SetCategory" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = a.CategoryCode and b.MenuLevel2 > 0 and Channel = 0 and UseYN='Y' and AdSlotCode='" + data.SearchAdType + "') as SetGenre" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = a.CategoryCode and b.MenuLevel2 > 0 and Channel > 0 and UseYN='Y' and AdSlotCode='" + data.SearchAdType + "') as SetChannel" + "\n");
                sbQuery.Append(" from	Category a with(noLock)" + "\n");
                sbQuery.Append(" where	a.MediaCode = 1" + "\n");
                sbQuery.Append(" and	a.Flag = 'Y'" + "\n");
                sbQuery.Append(" and	a.CategoryCode < 9999" + "\n");
                sbQuery.Append(" order by a.SortNo" + "\n");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + data.SearchAdType  + "]");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // ��� DataSet�� ��ü�𵨿� ����
                data.CategoryDataSet    = ds.Copy();
                data.ResultCnt          = Utility.GetDatasetCount( data.CategoryDataSet );
                data.ResultCD           = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + data.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                data.ResultCD = "3000";
                data.ResultDesc = "ī�װ����� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();			
            }
        }


        /// <summary>
        /// ������������ ���Ǵ� �帣������ �����´�
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GetGenreList(HeaderModel header, MediaMenuSetModel data)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreList() Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" select	x.CategoryCode	as	Category" + "\n");
                sbQuery.Append("        , d.GenreCode			as	Genre" + "\n");
                sbQuery.Append(" 		, d.GenreName			as	GenreName" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = x.CategoryCode and b.MenuLevel2 = 0           and b.MenuLevel3 = 0 and Channel = 0 and UseYN='Y' and AdSlotCode=  '" + data.SearchAdType + "')   as SetCategory" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = x.CategoryCode and b.MenuLevel2 = x.GenreCode and b.MenuLevel3 = 0 and Channel = 0 and UseYN='Y' and AdSlotCode = '" + data.SearchAdType + "')	as SetGenre" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = x.CategoryCode and b.MenuLevel2 = x.GenreCode and b.MenuLevel3 = 0 and Channel > 0 and UseYN='Y' and AdSlotCode = '" + data.SearchAdType + "')	as SetChannel" + "\n");
                sbQuery.Append(" from (  select a.CategoryCode, a.GenreCode" + "\n");
                sbQuery.Append(" 		from	ChannelSet a with(noLock)" + "\n");
                sbQuery.Append(" 		inner join Channel b with(noLock) on a.MediaCode = b.MediaCode and a.ChannelNo = b.ChannelNo and a.SeriesNo = b.SeriesNo" + "\n");
                sbQuery.Append(" 		inner join Contents c with(noLock) on b.ContentId = c.ContentId and c.ContentsState <= '65' and c.SeriesId is not null" + "\n");
                sbQuery.Append(" 		where	a.CategoryCode = " + data.Category + "\n");
                sbQuery.Append(" 		group by a.CategoryCode, a.GenreCode) x" + "\n");
                sbQuery.Append(" inner join Genre d with(noLock) on d.GenreCode = x.GenreCode" + "\n");
                sbQuery.Append(" order by d.SortOrder;" + "\n");
                              

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("AdType      :[" + data.SearchAdType  + "]");
                _log.Debug("Category    :[" + data.Category  + "]");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // ��� DataSet�� ��ü�𵨿� ����
                data.GenreDataSet   = ds.Copy();
                data.ResultCnt      = Utility.GetDatasetCount( data.GenreDataSet );
                data.ResultCD       = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + data.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                data.ResultCD = "3000";
                data.ResultDesc = "�帣���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();			
            }
        }


        /// <summary>
        /// ����� ī�װ��� �����Ѵ�
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void CategoryDelete(HeaderModel header, MediaMenuSetModel data)
        {
            int rc = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryDelete() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" delete from adContents" + "\n");
                sbQuery.Append(" where	MediaCode = 0" + "\n");
                sbQuery.Append(" and    AdSlotCode  = '" + data.SearchAdType + "'" + "\n");
                sbQuery.Append(" and	MenuLevel1	= " + data.Category + "\n");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("AdType      :[" + data.SearchAdType  + "]");
                _log.Debug("Category    :[" + data.Category  + "]");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                _db.Open();
                _db.BeginTran();
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                if( rc > 0 )
                {
                    data.ResultDesc = "[Step1] ī�װ� �����Ϸ�";
                    data.ResultCD   = "0000";
                    _db.CommitTran();
                }
                else
                {
                    data.ResultDesc = "[Step1] ī�װ� �������, ó���� �����Ͱ� �����ϴ�";
                    data.ResultCD   = "0000";
                    _db.RollbackTran();
                }
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                _db.RollbackTran();
                data.ResultCD = "3000";
                data.ResultDesc = "ī�װ� ����� ������ ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();			
            }
        }


        /// <summary>
        /// ����� ī�װ��� �߰��Ѵ�
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void CategoryInsert(HeaderModel header, MediaMenuSetModel data)
        {
            int rc = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryDelete() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" delete from adContents" + "\n");
                sbQuery.Append(" where	MediaCode = 0" + "\n");
                sbQuery.Append(" and    AdSlotCode  = '" + data.SearchAdType + "'" + "\n");
                sbQuery.Append(" and	MenuLevel1	= " + data.Category + "\n");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("AdType      :[" + data.SearchAdType  + "]");
                _log.Debug("Category    :[" + data.Category  + "]");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                _db.Open();
                _db.BeginTran();
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.Append(" INSERT INTO AdContents" + "\n");
                sbQuery.Append("            ([MediaCode]" + "\n");
                sbQuery.Append("            ,[AdSlotCode]" + "\n");
                sbQuery.Append("            ,[MenuLevel1]" + "\n");
                sbQuery.Append("            ,[MenuLevel2]" + "\n");
                sbQuery.Append("            ,[MenuLevel3]" + "\n");
                sbQuery.Append("            ,[MenuLevel4]" + "\n");
                sbQuery.Append("            ,[Channel]" + "\n");
                sbQuery.Append("            ,[SeriesNo]" + "\n");
                sbQuery.Append("            ,[UseYn])" + "\n");
                sbQuery.Append("      VALUES" + "\n");
                sbQuery.Append("            (0" + "\n");
                sbQuery.Append("            ,'" + data.SearchAdType + "'" + "\n");
                sbQuery.Append("            ,"  + data.Category + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,'Y') " + "\n");
                
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                if( rc > 0 )
                {
                    data.ResultDesc = "[Step1] ����� ī�װ� �߰��Ϸ�";
                    data.ResultCD   = "0000";
                    _db.CommitTran();
                }
                else
                {
                    data.ResultDesc = "[Step1] ī�װ� �߰����, ó���� �����Ͱ� �����ϴ�";
                    data.ResultCD   = "0000";
                    _db.RollbackTran();
                }
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryInsert() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                _db.RollbackTran();
                data.ResultCD = "3000";
                data.ResultDesc = "ī�װ� ����� �߰��� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();			
            }
        }

        /// <summary>
        /// ����� �帣 ����
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GenreDelete(HeaderModel header, MediaMenuSetModel data)
        {
            int rc = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GenreDelete() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" delete from adContents" + "\n");
                sbQuery.Append(" where	MediaCode = 0" + "\n");
                sbQuery.Append(" and    AdSlotCode  = '" + data.SearchAdType + "'" + "\n");
                sbQuery.Append(" and	MenuLevel1	= " + data.Category + "\n");
                sbQuery.Append(" and	MenuLevel2	= " + data.Genre    + "\n");
                sbQuery.Append(" and	MenuLevel3	= 0" +  "\n");
                sbQuery.Append(" and	MenuLevel4	= 0" +  "\n");
                sbQuery.Append(" and	Channel 	= 0" +  "\n");
                sbQuery.Append(" and	SeriesNo    = 0;" +  "\n");


                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("AdType      :[" + data.SearchAdType  + "]");
                _log.Debug("Category    :[" + data.Category  + "]");
                _log.Debug("Category    :[" + data.Genre  + "]");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                _db.Open();
                _db.BeginTran();
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                if( rc > 0 )
                {
                    data.ResultDesc = "[Step1] ī�װ� �����Ϸ�";
                    data.ResultCD   = "0000";
                    _db.CommitTran();
                }
                else
                {
                    data.ResultDesc = "[Step1] ī�װ� �������, ó���� �����Ͱ� �����ϴ�";
                    data.ResultCD   = "0000";
                    _db.RollbackTran();
                }
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                _db.RollbackTran();
                data.ResultCD = "3000";
                data.ResultDesc = "ī�װ� ����� ������ ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();			
            }
        }


        /// <summary>
        /// ����� �帣 �߰�
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GenreInsert(HeaderModel header, MediaMenuSetModel data)
        {
            int rc = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GenreInsert() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();
                // �帣�߰��� ���� ī�װ��� ����Ǿ� ������ ����ó���Ѵ�
                // ä�ε� ����� ��쿣, ä�ε� �����ϴ� �κ��� �־�� �Ѵ�
                sbQuery.Append("\n");
                sbQuery.Append(" delete from adContents" + "\n");
                sbQuery.Append(" where	MediaCode = 0" + "\n");
                sbQuery.Append(" and    AdSlotCode  = '" + data.SearchAdType + "'" + "\n");
                sbQuery.Append(" and	MenuLevel1	= " + data.Category + "\n");
                sbQuery.Append(" and	MenuLevel2	= 0" +  "\n");
                sbQuery.Append(" and	MenuLevel3	= 0" +  "\n");
                sbQuery.Append(" and	MenuLevel4	= 0" +  "\n");
                sbQuery.Append(" and	Channel 	= 0" +  "\n");
                sbQuery.Append(" and	SeriesNo    = 0;" +  "\n");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("AdType      :[" + data.SearchAdType  + "]");
                _log.Debug("Category    :[" + data.Category  + "]");
                _log.Debug("Genre       :[" + data.Genre  + "]");
                
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                _db.Open();
                _db.BeginTran();
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.Append(" INSERT INTO AdContents" + "\n");
                sbQuery.Append("            ([MediaCode]" + "\n");
                sbQuery.Append("            ,[AdSlotCode]" + "\n");
                sbQuery.Append("            ,[MenuLevel1]" + "\n");
                sbQuery.Append("            ,[MenuLevel2]" + "\n");
                sbQuery.Append("            ,[MenuLevel3]" + "\n");
                sbQuery.Append("            ,[MenuLevel4]" + "\n");
                sbQuery.Append("            ,[Channel]" + "\n");
                sbQuery.Append("            ,[SeriesNo]" + "\n");
                sbQuery.Append("            ,[UseYn])" + "\n");
                sbQuery.Append("      VALUES" + "\n");
                sbQuery.Append("            (0" + "\n");
                sbQuery.Append("            ,'" + data.SearchAdType + "'" + "\n");
                sbQuery.Append("            ,"  + data.Category + "\n");
                sbQuery.Append("            ,"  + data.Genre + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,'Y') " + "\n");
                
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                if( rc > 0 )
                {
                    data.ResultDesc = "[Step1] ����� �帣 �߰��Ϸ�";
                    data.ResultCD   = "0000";
                    _db.CommitTran();
                }
                else
                {
                    data.ResultDesc = "[Step1] ����� �帣 �߰����, ó���� �����Ͱ� �����ϴ�";
                    data.ResultCD   = "0000";
                    _db.RollbackTran();
                }
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryInsert() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                _db.RollbackTran();
                data.ResultCD = "3000";
                data.ResultDesc = "[����� �帣 �߰� ����]\n" + ex.Message;
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();			
            }
        }

	}
}
