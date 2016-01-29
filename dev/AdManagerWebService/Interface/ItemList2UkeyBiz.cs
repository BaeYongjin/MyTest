using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

namespace AdManagerWebService.Interface
{
	/// <summary>
	/// ItemList2UkeyBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ItemList2UkeyBiz : BaseBiz
	{
        public ItemList2UkeyBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// ������ �������� �о�´�( OAP�迭�� �о����
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataList()
        {
            DataSet ds = new DataSet();

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "PeriodViewCntAdver_EachProgram() Start");
                _log.Debug("-----------------------------------------");
			
                #region [SQL]
                StringBuilder sb = new StringBuilder();
                
                sb.Append("\n select ItemNo ");
                sb.Append("\n 		,ItemName ");
                sb.Append("\n 		,ExcuteStartDay ");
                sb.Append("\n 		,RealEndDay ");
                sb.Append("\n 		,'[' + AdType + '] '	+ d.CodeName	as AdType ");
                sb.Append("\n 		,'[' + AdState + '] '	+ b.CodeName	as AdState ");
                sb.Append("\n 		,'[' + FileState + '] ' + c.CodeName	as FileState ");
                sb.Append("\n from	ContractItem	a with(noLock) ");
                sb.Append("\n inner join SystemCode	b with(noLock)	on a.AdState = b.Code and b.Section = '25' ");
                sb.Append("\n inner join SystemCode	c with(noLock)	on a.FileState = c.Code and c.Section = '31' ");
                sb.Append("\n inner join SystemCode	d with(noLock)	on a.AdType  = d.Code and d.Section = '26' ");
                sb.Append("\n where	a.AdState	< '40'		-- ������°� ���ᰡ �ƴѰ� ");
                sb.Append("\n and   a.FileState < '90'		-- ���ϻ��°� ��ž������ �ƴѰ� ");
                sb.Append("\n and	a.AdType	in('11','12','20') ");
                sb.Append("\n and	a.RealEndDay > Convert(varchar(8),GetDate()-15,112) ");
                sb.Append("\n order by ItemNo desc \n"); 
                #endregion

                #region [ �Ķ���� DEBUG ]
                _log.Debug("-----------------------------------------");
                _log.Debug(sb.ToString());
                _log.Debug("-----------------------------------------");
                #endregion
				
                // ��������
                _db.Open();
                _db.ExecuteQuery(ds,sb.ToString());

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDataList() End");
                _log.Debug("-----------------------------------------");

                return ds.Copy();
            }
            catch(Exception ex)
            {
                _log.Exception(ex);
                return null;
            }
            finally
            {
                _db.Close();
                if( ds != null )
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }
	}
}
