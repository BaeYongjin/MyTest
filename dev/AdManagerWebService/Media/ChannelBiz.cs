/*
 * -------------------------------------------------------
 * Class Name: ChannelBiz
 * �ֿ���  : �ǽð�ä������ ����
 * �ۼ���    : 
 * �ۼ���    : 2015
 * Ư�̻���  : �ǽð�ä�������� ���� ��.
 * -------------------------------------------------------
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Media
{
    /// <summary>
    /// ChannelBiz�� ���� ��� �����Դϴ�.
    /// </summary>
    public class ChannelBiz : BaseBiz
    {
        public ChannelBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }


        /// <summary>
        /// �帣������������ ����
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelList(HeaderModel header, ChannelModel channelModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey      :[" + channelModel.SearchKey       + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                // stm_cod ���� ä���帣������ �ִ� �ڵ常 ������.
                sbQuery.AppendLine(" SELECT                 ");
                sbQuery.AppendLine("      stm_cod           ");
                sbQuery.AppendLine("     ,stm_cod_cls       ");
                sbQuery.AppendLine("     ,stm_cod_nm        ");
                sbQuery.AppendLine(" FROM STM_COD           ");
                sbQuery.AppendLine(" WHERE 1=1              ");
                sbQuery.AppendLine(" AND stm_cod_cls = '78' ");
                sbQuery.AppendLine(" AND stm_cod >'00'      ");
                sbQuery.AppendLine(" ORDER BY stm_cod_nm    ");
    
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // ��� DataSet�� ä�θ𵨿� ����
                channelModel.ChannelDataSet = ds.Copy();
                // ���
                channelModel.ResultCnt = Utility.GetDatasetCount(channelModel.ChannelDataSet);
                // ����ڵ� ��Ʈ
                channelModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + channelModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                channelModel.ResultCD = "3000";
                channelModel.ResultDesc = "ä���帣 ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }

        /// <summary>
        /// ä���帣�� �ǽð�ä������
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelDetailList(HeaderModel header, ChannelModel channelModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelDetailList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");

                _log.Debug("GenreCode :[" + channelModel.ChannelNo + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                OracleParameter[] sqlParams = new OracleParameter[1];
                // ��������               
                sbQuery.AppendLine(" SELECT         ");
                sbQuery.AppendLine("        svc_id  ");
                sbQuery.AppendLine("       ,ch_no   ");
                sbQuery.AppendLine("       ,ch_nm   ");
                sbQuery.AppendLine("       ,gnr_cod ");
                sbQuery.AppendLine("       ,ch_rank ");
                sbQuery.AppendLine("       ,use_yn  ");
                sbQuery.AppendLine("       ,ad_yn   ");
                sbQuery.AppendLine("       ,ad_rate ");
                sbQuery.AppendLine("       ,adn_rate ");
                sbQuery.AppendLine(" FROM CHNL_COD  ");
                sbQuery.AppendLine(" WHERE 1=1      ");    
                sbQuery.AppendLine(" AND GNR_COD = :GenreCode ");
                
                if (channelModel.CheckYn.Equals("Y"))
                    sbQuery.AppendLine(" AND use_yn = 'Y' ");

                sbQuery.AppendLine(" ORDER BY ch_nm ");

                i = 0;                
                sqlParams[i++] = new OracleParameter(":GenreCode"    , OracleDbType.Varchar2,4);
                               
                i = 0;
                sqlParams[i++].Value = channelModel.GenreCode; //�帣�ڵ���


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

                // ��� DataSet�� ä�θ𵨿� ����
                channelModel.ChannelDataSet = ds.Copy();
                // ���
                channelModel.ResultCnt = Utility.GetDatasetCount(channelModel.ChannelDataSet);
                // ����ڵ� ��Ʈ
                channelModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + channelModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelDetailList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                channelModel.ResultCD = "3000";
                channelModel.ResultDesc = "�ǽð�ä�� ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }

        /// <summary>
        /// ä�α��� ������ �����ȸ - ������
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelSetDetailList(HeaderModel header, ChannelModel channelModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelSetDetailList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("MediaCode :[" + channelModel.MediaCode       + "]");
                _log.Debug("ChannelNo :[" + channelModel.ChannelNo       + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                SqlParameter[] sqlParams = new SqlParameter[2];
                // ��������
                sbQuery.Append("\n"
                    + " SELECT a.CategoryCode ,a.GenreCode	\n"
                    + " ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.CategoryCode))) +  CONVERT(VARCHAR(10),a.CategoryCode) + ' ' + b.CategoryName) AS CategoryName \n"
                    + " ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.GenreCode)))    +  CONVERT(VARCHAR(10),a.GenreCode)    + ' ' + C.GenreName   ) AS GenreName 	 \n"
                    + " FROM (													 \n"
                    + " 	   SELECT MediaCode,CategoryCode ,GenreCode			 \n"
                    + " 	   FROM ChannelSet a with(nolock)					 \n"
                    + "        WHERE 1=1										 \n"
                    + "          AND MediaCode = @MediaCode						 \n"
                    + "          AND ChannelNo = @ChannelNo						 \n"
                    + "     GROUP BY MediaCode,CategoryCode,GenreCode			 \n"
                    + " 	  ) a												 \n"
                    + " 	LEFT JOIN Category	b with(nolock) ON(a.CategoryCode = b.CategoryCode)	\n"
                    + " 	LEFT JOIN Genre		c with(nolock) ON (a.GenreCode = c.GenreCode)		\n"	
                    );	 
                sbQuery.Append("  ORDER BY a.CategoryCode,a.GenreCode ");

                i = 0;
                sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int);
                               
                i = 0;
                sqlParams[i++].Value = channelModel.MediaCode;	
                sqlParams[i++].Value = channelModel.ChannelNo;


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

                // ��� DataSet�� ä�θ𵨿� ����
                channelModel.ChannelDataSet = ds.Copy();
                // ���
                channelModel.ResultCnt = Utility.GetDatasetCount(channelModel.ChannelDataSet);
                // ����ڵ� ��Ʈ
                channelModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + channelModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelSetDetailList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                channelModel.ResultCD = "3000";
                channelModel.ResultDesc = "ä�α��� ������ ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }


           
        /// <summary>
        /// �ǽð�ä������ ������Ʈ
        /// </summary>
        /// <param name="header"></param>
        /// <param name="channelModel"></param>
        public void SetChannelUpdate(HeaderModel header, ChannelModel channelModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelUpdate() Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.AppendLine(" UPDATE CHNL_COD        ");
                sbQuery.AppendLine("   SET use_yn = :UseYn  ");
                sbQuery.AppendLine("        ,ad_yn  = :AdYn ");
                sbQuery.AppendLine("        ,ad_rate = :AdRate      ");
                sbQuery.AppendLine("        ,adn_rate = :AdnRate    ");
                sbQuery.AppendLine(" WHERE svc_id = :SvcId  ");
                sbQuery.AppendLine(" AND    ch_no = :ChNo   ");

                int i = 0;
                int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[6];
                               

                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char,1);
                sqlParams[i++] = new OracleParameter(":AdYn", OracleDbType.Char,1);
                sqlParams[i++] = new OracleParameter(":AdRate", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AdnRate", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":SvcId", OracleDbType.Varchar2,10);
                sqlParams[i++] = new OracleParameter(":ChNo", OracleDbType.Varchar2,10);

                i = 0;
                sqlParams[i++].Value = channelModel.UseYn;
                sqlParams[i++].Value = channelModel.AdYn;
                sqlParams[i++].Value = Convert.ToInt32(channelModel.AdRate);
                sqlParams[i++].Value = Convert.ToInt32(channelModel.AdnRate);
                sqlParams[i++].Value = channelModel.ServiceID;
                sqlParams[i++].Value = channelModel.ChannelNumber;
                               

                _log.Debug(sbQuery.ToString());


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("�ǽð�ä������ ����:[" + channelModel.ServiceID + "] �����:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                channelModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetSchRateUpdate() End");
                _log.Debug("-----------------------------------------");

            }
            catch (Exception ex)
            {
                channelModel.ResultCD = "3101";
                channelModel.ResultDesc = "������� ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }		

        }

        /// <summary>
        /// ä�� ���� - ������
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="UserName"></param>
        /// <param name="UserPassword"></param>
        /// <param name="UserLevel"></param>
        /// <param name="UserDept"></param>
        /// <param name="UserTitle"></param>
        /// <param name="UserTell"></param>
        /// <param name="UserMobile"></param>
        /// <param name="UserComment"></param>
        /// <returns></returns>
        public void SetChannelCreate(HeaderModel header, ChannelModel channelModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelCreate() Start");
                _log.Debug("-----------------------------------------");
                        
                StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbQuery2 = new StringBuilder();
                        			

                sbQuery.Append( ""
                    + "        DELETE FROM  Channel			                    \n"
                    + "              WHERE MediaCode = @MediaCode				\n"
                    + "                AND ChannelNo = @ChannelNo				\n"
                    );                                       
                sbQuery2.Append( ""
                    + "        INSERT INTO Channel			                    \n"
                    + "                   (                                     \n"
                    + "                    MediaCode							\n"
                    + "                   ,ChannelNo							\n"
                    + "                   ,SeriesNo								\n"
                    + "                   ,Title								\n"
                    + "                   ,ContentId							\n"
                    + "                   ,TotalSeries							\n"
                    + "                   ,ModDt								\n"
                    + "                   )                                     \n"
                    + "             VALUES										\n"
                    + "                   (                                     \n"
                    + "                    @MediaCode							\n"
                    + "                   ,@ChannelNo							\n"
                    + "                   ,@SeriesNo							\n"
                    + "                   ,@Title								\n"
                    + "                   ,@ContentId							\n"
                    + "                   ,@TotalSeries							\n"
                    + "                   ,GETDATE()							\n"
                    + "                   )                                     \n"
                    );                                       
                               
                        
                		
                        
                // ��������
                try
                {
                    
                    int i = 0;
                    int rc = 0;
                    SqlParameter[] sqlParams = new SqlParameter[2];
                    _db.BeginTran();
                     
                    //���� �μ�Ʈ �� �����͸� �����ְ� �����Ŀ� �μ�Ʈ�� �Ѵ�.
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.TinyInt);
                    sqlParams[i++] = new SqlParameter("@ChannelNo"     , SqlDbType.Int );
                                                   
                    i = 0;
                    sqlParams[i++].Value = channelModel.MediaCode;
                    sqlParams[i++].Value = channelModel.ChannelNo;
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    // __DEBUG__
                    _log.Debug("MediaCode = "+channelModel.MediaCode);
                    _log.Debug("ChannelNo = "+channelModel.ChannelNo);
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    sqlParams = new SqlParameter[10];

                    for(int count=0;count < channelModel.ChannelDataSet.Tables["Contents"].Rows.Count;count++)

                    {
                        i = 0;
                        sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.VarChar , 120);
                        sqlParams[i++] = new SqlParameter("@ChannelNo"     , SqlDbType.Int );
                        sqlParams[i++] = new SqlParameter("@SeriesNo" , SqlDbType.TinyInt);
                        sqlParams[i++] = new SqlParameter("@Title"    , SqlDbType.VarChar , 120);
                        sqlParams[i++] = new SqlParameter("@ContentId"    , SqlDbType.VarChar,36);
                        sqlParams[i++] = new SqlParameter("@TotalSeries"    , SqlDbType.Int );
                       
                         
                          
                        
                        i = 0;
                        sqlParams[i++].Value = channelModel.MediaCode;
                        sqlParams[i++].Value = channelModel.ChannelNo;
                        sqlParams[i++].Value = channelModel.ChannelDataSet.Tables["Contents"].Rows[count][3].ToString();
                        sqlParams[i++].Value = channelModel.Title;
                        sqlParams[i++].Value = channelModel.ChannelDataSet.Tables["Contents"].Rows[count][1].ToString();
                        
                        if(channelModel.TotalSeries.Trim().Length > 0)
                        {
                            sqlParams[i++].Value = channelModel.TotalSeries;
                        }
                        else
                        {
                            sqlParams[i++].Value = null;		
                        }
                        

                        rc =  _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams);
                        // __DEBUG__
                        _log.Debug(sbQuery2.ToString());
                        // __DEBUG__
                    }
                    _db.CommitTran();
                        
                    // __MESSAGE__
                    _log.Message("ä����������:[����]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                channelModel.ResultCD = "0000";  // ����
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelCreate() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                channelModel.ResultCD   = "3101";
                channelModel.ResultDesc = "ä������ ���� �� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="header"></param>
        /// <param name="channelModel"></param>
        public void SetChannelDelete(HeaderModel header, ChannelModel channelModel)
        {
            try
            {   // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelDelete() Start");
                _log.Debug("-----------------------------------------");
                        
                StringBuilder sbQuery = new StringBuilder();
                        
                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[2];
                        
                sbQuery.Append( ""
                    + "        DELETE FROM  Channel			                    \n"
                    + "              WHERE MediaCode = @MediaCode				\n"
                    + "                AND ChannelNo = @ChannelNo				\n"
                    );                             
                                        
                sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@ChannelNo"       , SqlDbType.Int );
                        
                i = 0;
                sqlParams[i++].Value = channelModel.MediaCode;
                sqlParams[i++].Value = channelModel.ChannelNo;
                        
                // ��������
                try
                {
                    _db.BeginTran();
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();
                        
                    // __MESSAGE__
                    _log.Message("ä����������:[" + channelModel.ChannelNo + "] �����:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                channelModel.ResultCD = "0000";  // ����
                        	
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelDelete() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                channelModel.ResultCD   = "3301";
                channelModel.ResultDesc = "ä������ ������ ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }
    }
}