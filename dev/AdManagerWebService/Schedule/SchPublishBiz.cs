/*
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�	: [E_01]
 * ������		: YJ.Park
 * ������		: 2014.11.13
 * ��������	: Ȩ����(Ű��) �߰��� ���� �� �߰� (adSchType =30)
 * --------------------------------------------------------
 * �����ڵ�	: [E_02]
 * ������		: YJ.Park
 * ������		: 2015.05.29
 * ��������	: Ȩ����(Ÿ�ٱ�) �߰��� ���� �� �߰� (adSchType =40)
 * --------------------------------------------------------
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

namespace AdManagerWebService.Schedule
{
	/// <summary>
	/// SchPublishBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchPublishBiz : BaseBiz
	{

		#region ������
		public SchPublishBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}
		#endregion 

		#region ������λ��� ��ȸ
		/// <summary>
		/// ������λ��� ��ȸ
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void GetPublishState(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishState() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	       :[" + schPublishModel.SearchMediaCode      + "]");		// �˻� ��ü				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
                /*
				sbQuery.Append("\n"
					+ " SELECT TOP 1 AckNo ,State \n"
					+ "   FROM SchPublish         \n"
					+ "  WHERE MediaCode = " + schPublishModel.SearchMediaCode + " \n"
					+ "  ORDER BY AckNo DESC      \n"
					);
                */
                sbQuery.Append("\n"
                    + " SELECT ack_no as AckNo ,ack_stt as State \n"
                    + " From (            \n"
                    + "       Select ack_no, ack_stt   \n"
                    + "       FROM SCHD_DIST_MST      \n"
                    + "       ORDER BY Ack_No DESC    \n"
                    + " ) \n" 
                    + "  WHERE ROWNUM <= 1         \n"
                    
                    );
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					schPublishModel.AckNo =  ds.Tables[0].Rows[0]["AckNo"].ToString();
					schPublishModel.State =  ds.Tables[0].Rows[0]["State"].ToString();
				}
				else
				{
					schPublishModel.AckNo =  "";
					schPublishModel.State =  "";
				}

				ds.Dispose();

				// ����ڵ� ��Ʈ
				schPublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schPublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishState() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD = "3000";
				schPublishModel.ResultDesc = "������λ��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}

		/// <summary>
		/// [S1] Ȩ���� ���λ��¸� �����´�
		/// </summary>
		/// <param name="header"></param>
		/// <param name="schPublishModel"></param>
		public void GetHomePublishState(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetHomePublishState() Start");
				_log.Debug("-----------------------------------------");
				_db.Open();
				_log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	       :[" + schPublishModel.SearchMediaCode      + "]");		// �˻� ��ü				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				/*
				// ��������
				sbQuery.Append("\n"
					+ " SELECT TOP 1 AckNo ,State \n"
					+ "   FROM SchPublish         \n"
					+ "  WHERE MediaCode = " + schPublishModel.SearchMediaCode + " \n"
					+ "    AND AdSchType	= 0  \n"
					+ "  ORDER BY AckNo DESC     \n");
                */
                sbQuery.Append("\n"
                    + " SELECT ack_no as AckNo ,ack_stt as State \n"
                    + " From (                       \n"
                    + "      Select ack_no, ack_stt \n"
                    + "      FROM SCHD_DIST_MST     \n"                   
                    + "      WHERE SCHD_TP	= '0'    \n"
                    + "      ORDER BY Ack_No DESC    \n"
                    + "     )                     \n"
                    + "  Where ROWNUM <=1         \n");
                    

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					schPublishModel.AckNo =  ds.Tables[0].Rows[0]["AckNo"].ToString();
					schPublishModel.State =  ds.Tables[0].Rows[0]["State"].ToString();
				}
				else
				{
					schPublishModel.AckNo =  "0";
					schPublishModel.State =  "10";
				}

				ds.Dispose();
				ds = null;

				// ����ڵ� ��Ʈ
				schPublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schPublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetPublishState() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD = "3000";
				schPublishModel.ResultDesc = "Ȩ���� ������λ��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}

		/// <summary>
		/// [S1] ������� ���λ��¸� �����´�
		/// </summary>
		/// <param name="header"></param>
		/// <param name="schPublishModel"></param>
		public void GetCMPublishState(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCMPublishState() Start");
				_log.Debug("-----------------------------------------");
				_db.Open();
				_log.Debug("<�Է�����>");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				// ��������
				sbQuery.Append("\n SELECT  ACK_NO AS AckNo, ACK_STT AS State");
				sbQuery.Append("\n FROM SCHD_DIST_MST");
				sbQuery.Append("\n WHERE ACK_NO = (SELECT  MAX(ACK_NO) ");
				sbQuery.Append("\n                 FROM    SCHD_DIST_MST");
				sbQuery.Append("\n                 WHERE   SCHD_TP = '10' )	");

				_log.Debug(sbQuery.ToString());
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					schPublishModel.AckNo =  ds.Tables[0].Rows[0]["AckNo"].ToString();
					schPublishModel.State =  ds.Tables[0].Rows[0]["State"].ToString();
				}
				else
				{
					schPublishModel.AckNo =  "0";
					schPublishModel.State =  "10";
				}

				ds.Dispose();
				ds = null;

				// ����ڵ� ��Ʈ
				schPublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schPublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCMPublishState() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD = "3000";
				schPublishModel.ResultDesc = "������� ������λ��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}

		/// <summary>
		/// [S1] �������� ���λ��¸� �����´�
		/// </summary>
		/// <param name="header"></param>
		/// <param name="schPublishModel"></param>
		public void GetOAPPublishState(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetOAPPublishState() Start");
				_log.Debug("-----------------------------------------");
				_db.Open();
				_log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	       :[" + schPublishModel.SearchMediaCode      + "]");		// �˻� ��ü				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
                /*
				sbQuery.Append("\n"
					+ " SELECT TOP 1 AckNo ,State \n"
					+ "   FROM SchPublish         \n"
					+ "  WHERE MediaCode = " + schPublishModel.SearchMediaCode + " \n"
					+ "    AND AdSchType	= 20  \n"
					+ "  ORDER BY AckNo DESC     \n");
                */
                sbQuery.Append("\n"
                                + " SELECT ack_no as AckNo ,ack_stt as State \n"
                                + "   FROM SCHD_DIST_MST    \n"                                
                                + "   WHERE SCHD_TP	= '20'  \n"
                                + "   AND ROWNUM <= 1       \n"
                                + "  ORDER BY ack_no DESC   \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if(ds.Tables[0].Rows.Count > 0)
				{
					schPublishModel.AckNo =  ds.Tables[0].Rows[0]["AckNo"].ToString();
					schPublishModel.State =  ds.Tables[0].Rows[0]["State"].ToString();
				}
				else
				{
					schPublishModel.AckNo =  "0";
					schPublishModel.State =  "10";
				}

				ds.Dispose();
				ds = null;

				// ����ڵ� ��Ʈ
				schPublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schPublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetOAPPublishState() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD = "3000";
				schPublishModel.ResultDesc = "Ȩ���� ������λ��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();			
			}		
		}


		/// <summary>
		/// Ȩ����(Ű��) ���λ��¸� �����´� [E_01]
		/// </summary>
		/// <param name="header"></param>
		/// <param name="schPublishModel"></param>
		public void GetHomeKidsPublishState(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetHomeKidsPublishState() Start");
				_log.Debug("-----------------------------------------");
				_db.Open();
				_log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	       :[" + schPublishModel.SearchMediaCode + "]");		// �˻� ��ü				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
                /*
				sbQuery.Append("\n"
					+ " SELECT TOP 1 AckNo ,State \n"
					+ "   FROM SchPublish         \n"
					+ "  WHERE MediaCode = " + schPublishModel.SearchMediaCode + " \n"
					+ "    AND AdSchType	= 30  \n"
					+ "  ORDER BY AckNo DESC     \n");
                */
              
                sbQuery.Append("\n"
                    + " SELECT ack_no as AckNo ,ack_stt as State \n"
                    + " From (                       \n"
                    + "      Select ack_no, ack_stt \n"
                    + "      FROM SCHD_DIST_MST     \n"
                    + "      WHERE SCHD_TP	= '30'    \n"
                    + "      ORDER BY Ack_No DESC    \n"
                    + "     )                     \n"
                    + "  Where ROWNUM <=1         \n");
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				if (ds.Tables[0].Rows.Count > 0)
				{
					schPublishModel.AckNo = ds.Tables[0].Rows[0]["AckNo"].ToString();
					schPublishModel.State = ds.Tables[0].Rows[0]["State"].ToString();
				}
				else
				{
					schPublishModel.AckNo = "0";
					schPublishModel.State = "10";
				}

				ds.Dispose();
				ds = null;

				// ����ڵ� ��Ʈ
				schPublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schPublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetHomeKidsPublishState() End");
				_log.Debug("-----------------------------------------");
			}
			catch (Exception ex)
			{
				schPublishModel.ResultCD = "3000";
				schPublishModel.ResultDesc = "Ȩ����(Ű��) ������λ��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}


        /// <summary>
        /// Ȩ����(����) ���λ��¸� �����´� [E_02]
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schPublishModel"></param>
        public void GetHomeTargetPublishState(HeaderModel header, SchPublishModel schPublishModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetHomeTargetPublishState() Start");
                _log.Debug("-----------------------------------------");
                _db.Open();
                _log.Debug("<�Է�����>");
                _log.Debug("SearchMediaCode	       :[" + schPublishModel.SearchMediaCode + "]");		// �˻� ��ü				
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                //sbQuery.Append("\n"
                //    + " SELECT ack_no as AckNo ,ack_stt as State \n"
                //    + "   FROM SCHD_DIST_MST         \n"
                //    + "  WHERE ROWNUM <= 1 \n"
                //    + "    AND schd_tp	= '40'  \n"
                //    + "  ORDER BY Ack_No DESC     \n");

                sbQuery.Append("\n"
                    + " SELECT ack_no as AckNo ,ack_stt as State \n"
                    + " From (                       \n"
                    + "      Select ack_no, ack_stt \n"
                    + "      FROM SCHD_DIST_MST     \n"
                    + "      WHERE SCHD_TP	= '40'    \n"
                    + "      ORDER BY Ack_No DESC    \n"
                    + "     )                     \n"
                    + "  Where ROWNUM <=1         \n");
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    schPublishModel.AckNo = ds.Tables[0].Rows[0]["AckNo"].ToString();
                    schPublishModel.State = ds.Tables[0].Rows[0]["State"].ToString();
                }
                else
                {
                    schPublishModel.AckNo = "0";
                    schPublishModel.State = "10";
                }

                ds.Dispose();
                ds = null;

                // ����ڵ� ��Ʈ
                schPublishModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + schPublishModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetHomeTargetPublishState() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                schPublishModel.ResultCD = "3000";
                schPublishModel.ResultDesc = "Ȩ����(����) ������λ��� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }



		#endregion
        
		#region ��������̷� ��ȸ

		/// <summary>
		/// ��������̷� ��ȸ
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void GetSchPublishList(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSchPublishList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchMediaCode	       :[" + schPublishModel.SearchMediaCode      + "]");		// �˻� ��ü				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n"
					+ " SELECT A.ACK_NO AS AckNo                                                        \n"
                    + "       ,A.ACK_STT AS AckState                                            \n"
                    + "       ,B.STM_COD_NM AS AckStateName                                     \n"
                    + "       ,TO_CHAR(A.MDF_BEGIN_DT, 'YYYY-MM-DD') AS ModifyStartDay  \n"
                    + "       ,TO_CHAR(A.ACK_DT, 'YYYY-MM-DD') AS AckDay                  \n"
                    + "       ,A.ACK_ID AS AckUser                                                      \n"
                    + "       ,C.USER_NM AS AckUserName                                      \n"
                    + "       ,A.ACK_DESC AS AckDesc                                                      \n"
                    + "       ,TO_CHAR(A.CHK_DT, 'YYYY-MM-DD') AS ChkDay                  \n"
                    + "       ,A.CHK_ID AS ChkUser                                                      \n"
                    + "       ,D.USER_NM AS ChkUserName                                      \n"
                    + "       ,A.CHK_DESC AS ChkDesc                                                      \n"
                    + "       ,TO_CHAR(A.DIST_DT, 'YYYY-MM-DD') AS PublishDay			\n"
                    + "       ,A.DIST_ID AS PublishUser													\n"
                    + "       ,E.USER_NM AS PublichUserName									\n"
                    + "       ,A.DIST_DESC AS PublishDesc													\n"
                    + "       ,A.SCHD_TP AS AdSchType														\n"
                    + "   FROM SCHD_DIST_MST A LEFT JOIN STM_COD B ON (A.ACK_STT = B.STM_COD AND B.STM_COD_CLS = '32')  \n"  // 32:�������λ���
                    + "                        LEFT JOIN STM_USER C ON (A.ACK_ID  = C.USER_ID)  \n"
                    + "                        LEFT JOIN STM_USER D ON (A.CHK_ID  = D.USER_ID)  \n"
                    + "                        LEFT JOIN STM_USER E ON (A.DIST_ID = E.USER_ID)  \n"
                    + "  WHERE ROWNUM < 100                                                \n"
                    + "  ORDER BY A.ACK_NO DESC                                                \n"
					);
	

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �ش�𵨿� ����
				schPublishModel.PublishDataSet = ds.Copy();
				ds.Dispose();

				// ���
				schPublishModel.ResultCnt = Utility.GetDatasetCount(schPublishModel.PublishDataSet);
				// ����ڵ� ��Ʈ
				schPublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schPublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetSchPublishList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD = "3000";
				schPublishModel.ResultDesc = "��������̷� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}
		#endregion

		#region ��������Ȳ ��ȸ

		/// <summary>
		/// ��������Ȳ ��ȸ
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void GetScheduleList(HeaderModel header, SchPublishModel schPublishModel)
		{
			//bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetScheduleList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey              :[" + schPublishModel.SearchKey            + "]");		// �˻� ��
				_log.Debug("SearchMediaCode	       :[" + schPublishModel.SearchMediaCode      + "]");		// �˻� ��ü				
				_log.Debug("AckNo	       :[" + schPublishModel.AckNo      + "]");		// �˻� ��ü				

				// __DEBUG__

				OracleParameter[] sqlParams = new OracleParameter[3];
		    
				int i = 0;
				sqlParams[i++] = new OracleParameter(":MediaCode"          , OracleDbType.Int32          );
				sqlParams[i++] = new OracleParameter(":SearchKey"          , OracleDbType.Varchar2,  50 );
				sqlParams[i++] = new OracleParameter(":AckNo"				, OracleDbType.Int32			 );

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32( schPublishModel.SearchMediaCode);
				sqlParams[i++].Value = schPublishModel.SearchKey ;
				sqlParams[i++].Value = Convert.ToInt32( schPublishModel.AckNo);

				StringBuilder sbQuery = new StringBuilder();				
				
				// ��������
				sbQuery.Append("\n"
					+ " SELECT  TA.MediaCode                                                    \n"
					+ "        ,TA.ORD                                                          \n"
					+ "        ,TA.Flag                                                         \n"
					+ "        ,TA.CugName                                                      \n"
					+ "        ,TA.CategoryCode                                                 \n"
					+ "        ,ISNULL(TG.MenuName,'') AS CategoryName                          \n"
					+ "        ,TA.GenreCode                                                    \n"
					+ "        ,ISNULL(TH.MenuName,'') AS GenreName                             \n"
					+ "        ,TA.ChannelNo                                                    \n"
					+ "        ,TA.Title                                                        \n"
					+ "        ,TA.ItemNo                                                       \n"
					+ "        ,TA.ItemName                                                     \n"
					+ "        ,TB.CodeName AS AdStateName                                      \n"
					+ "        ,TA.FileName                                                     \n"
					+ "        ,TA.FileType                                                     \n"
					+ "        ,TC.CodeName AS FileTypeName                                     \n"
					+ "        ,TA.FileState                                                    \n"
					+ "        ,TD.CodeName AS FileStateName                                    \n"
					+ "        ,TA.FileLength                                                   \n"
					+ "        ,TA.FilePath                                                     \n"
					+ "        ,TA.DownLevel                                                    \n"
					+ "        ,CONVERT(VarChar(3),TA.DownLevel) +  ' ����' AS DownLevelName    \n"
					+ "        ,TA.ScheduleOrder                                                \n"
					+ "        ,TA.AdType                                                       \n"
					+ "        ,TE.CodeName AS AdTypeName                                       \n"
					+ "        ,TA.AckNo                                                        \n"
					+ "        ,TF.State AS AckState                                            \n"
					+ "  FROM                                                                   \n"
					+ "(                                                                        \n"
					+ "  SELECT B.MediaCode                                                     \n"
					+ "        ,'0' AS ORD                                                      \n"
					+ "        ,'Ȩ' AS Flag                                                    \n"
					+ "        ,0 AS CategoryCode                                               \n"
					+ "        ,0 AS GenreCode                                                  \n"
					+ "        ,0 AS ChannelNo                                                  \n"
					+ "        ,'' AS Title                                                     \n"
					+ "        ,A.ItemNo                                                        \n"
					+ "        ,(SELECT CugName FROM Cug    WHERE CugCode = A.CugCode AND A.CugCode!=0) AS CugName                                                        \n"
					+ "        ,A.ScheduleOrder                                                 \n"
					+ "        ,A.AckNo                                                         \n"
					+ "        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType   \n"
					+ "    from SchPublishDetail A INNER JOIN ContractItem B ON(A.ItemNo = B.ItemNo AND A.SchType in ('10','20') AND A.AckNo=:AckNo)                  \n"
					+ "                                                                         \n"
					+ "  UNION                                                                  \n"
					+ "                                                                         \n"
					+ "  SELECT B.MediaCode                                                     \n"
					+ "        ,'1' AS ORD                                                      \n"
					+ "        ,'�޴�' AS Flag                                                  \n"
					+ "        ,C.UpperMenuCode AS CategoryCode                                 \n"
					+ "        ,A.GenreCode                                                     \n"
					+ "        ,0 AS ChannelNo                                                  \n"
					+ "        ,'' AS Title                                                     \n"
					+ "        ,A.ItemNo                                                        \n"
					+ "        ,(SELECT CugName FROM Cug    WHERE CugCode = A.CugCode AND A.CugCode!=0) AS CugName                                                        \n"
					+ "        ,A.ScheduleOrder                                                 \n"
					+ "        ,A.AckNo                                                         \n"
					+ "        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType   \n"
					+ "   FROM SchPublishDetail A INNER JOIN ContractItem B ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = :MediaCode AND A.SchType in ('11','21') AND A.AckNo=:AckNo) \n"
					+ "                              INNER JOIN Menu         C ON (A.GenreCode = C.MenuCode  AND C.MediaCode = :MediaCode) \n"
					+ "   UNION                                                                 \n"
					+ "                                                                         \n"
					+ " SELECT B.MediaCode                                                      \n"
					+ "       ,'2' AS ORD                                                       \n"
					+ "       ,'ä��' AS Flag                                                   \n"
					+ "       ,C.Category AS CatagoryCode                                       \n"
					+ "       ,C.Genre    AS GenreCode                                          \n"
					+ "       ,A.ChannelNo                                                      \n"
					+ "       ,C.ProgramNm as Title                                             \n"
					+ "       ,A.ItemNo                                                         \n"
					+ "       ,(SELECT CugName FROM Cug    WHERE CugCode = A.CugCode AND A.CugCode!=0) AS CugName                                                         \n"
					+ "       ,A.ScheduleOrder                                                  \n"
					+ "       ,A.AckNo                                                          \n"
					+ "       ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType       \n"
					+ "   FROM SchPublishDetail A INNER JOIN ContractItem B ON (A.ItemNo    = B.ItemNo    AND B.MediaCode = :MediaCode AND A.SchType in ('12','22') AND A.AckNo=:AckNo) \n"
					+ "                                 INNER JOIN Program      C ON (A.ChannelNo = C.Channel   AND C.MediaCode = :MediaCode) \n"
					+ "                                                                         \n"
					+ "   UNION ALL                                                             \n"
					+ "                                                                         \n"
					+ "  SELECT B.MediaCode                                                     \n"
					+ "        ,'3' AS ORD                                                      \n"
					+ "        ,'�߰�' AS Flag                                                    \n"
					+ "        ,0 AS CategoryCode                                               \n"
					+ "        ,0 AS GenreCode                                                  \n"
					+ "        ,0 AS ChannelNo                                                  \n"
					+ "        ,'' AS Title                                                     \n"
					+ "        ,A.ItemNo                                                        \n"
					+ "        ,(SELECT CugName FROM Cug    WHERE CugCode = A.CugCode AND A.CugCode!=0) AS CugName                                                        \n"
					+ "        ,A.ScheduleOrder                                                 \n"
					+ "        ,A.AckNo                                                         \n"
					+ "        ,B.ItemName, B.AdState, B.FileName, B.FileType, B.FileState, B.FileLength,B.FilePath,B.DownLevel,B.AdType   \n"
					+ "    from SchPublishDetail A INNER JOIN ContractItem B ON(A.ItemNo = B.ItemNo AND A.SchType in ('13') AND A.AckNo=:AckNo)                  \n"
					+ "                                                                                                                       \n"                                                  
					+ " ) TA   LEFT JOIN SystemCode TB ON (TA.AdState      = TB.Code     AND TB.Section = '25') -- �������                   \n"
					+ "        LEFT JOIN SystemCode TC ON (TA.FileType     = TC.Code     AND TC.Section = '24') -- ���ϱ���                   \n"
					+ "        LEFT JOIN SystemCode TD ON (TA.FileState    = TD.Code     AND TD.Section = '31') -- ���ϻ���                   \n"
					+ "        LEFT JOIN SystemCode TE ON (TA.AdType       = TE.Code     AND TE.Section = '26') -- ��������                   \n"
					+ "        LEFT JOIN SchPublish TF ON (TA.AckNo        = TF.AckNo    AND TF.MediaCode = :MediaCode)  -- ���λ���          \n"
					+ "        LEFT JOIN Menu       TG ON (TA.CategoryCode = TG.MenuCode AND TG.MediaCode = :MediaCode)  -- ī�װ���        \n"
					+ "        LEFT JOIN Menu       TH ON (TA.GenreCode    = TH.MenuCode AND TH.MediaCode = :MediaCode)  -- �帣��            \n"
					);

				// �˻�� ������
				if (schPublishModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" WHERE ("
						+ "    TA.FileName    LIKE '%'+ :SearchKey + '%' \n"		
						+ " OR TA.ItemName    LIKE '%'+ :SearchKey + '%' \n"													
						+ " OR TG.MenuName    LIKE '%'+ :SearchKey + '%' \n"													
						+ " OR TH.MenuName    LIKE '%'+ :SearchKey + '%' \n"													
						+ " OR TA.ChannelNo   LIKE '%'+ :SearchKey + '%' \n"													
						+ " OR TA.Title       LIKE '%'+ :SearchKey + '%' \n"														
						+ " ) \n"
						);
				}			

				sbQuery.Append(" ORDER BY ORD, CategoryCode, GenreCode, ChannelNo, ScheduleOrder, FileName  \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// ��� DataSet�� �ش�𵨿� ����
				schPublishModel.ScheduleDataSet = ds.Copy();
				ds.Dispose();

				// ���
				schPublishModel.ResultCnt = Utility.GetDatasetCount(schPublishModel.ScheduleDataSet);
				// ����ڵ� ��Ʈ
				schPublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schPublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetScheduleList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD = "3000";
				schPublishModel.ResultDesc = "��������Ȳ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		public void GetScheduleList_S1(HeaderModel header, SchPublishModel schPublishModel)
		{
			//bool isState = false;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetScheduleList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey              :[" + schPublishModel.SearchKey            + "]");		// �˻� ��
				_log.Debug("SearchMediaCode	       :[" + schPublishModel.SearchMediaCode      + "]");		// �˻� ��ü				
				_log.Debug("AckNo	       :[" + schPublishModel.AckNo      + "]");		// �˻� ��ü				

				// __DEBUG__

                OracleParameter[] sqlParams = new OracleParameter[1];
		    
				int i = 0;
                sqlParams[i++] = new OracleParameter(":AckNo", OracleDbType.Int32);

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32( schPublishModel.AckNo);

				StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append("\n select	case a.SCHD_TP ");
				sbQuery.Append("\n 			when '10'	then	'�޴�' ");
                sbQuery.Append("\n 			when '11'	then	'Ÿ��Ʋ' ");
                sbQuery.Append("\n 			when '12'	then	'ȸ��' ");
				sbQuery.Append("\n 			when '30'	then	'ä��' ");
				sbQuery.Append("\n 			when '40'	then	'���' ");
				sbQuery.Append("\n 			else			'' ");
                sbQuery.Append("\n 		    end	AS SchTypeNm ");
				sbQuery.Append("\n 		    ,'' AS CugNm ");
                sbQuery.Append("\n 		    ,a.CAT_COD AS CategoryNm ");
                sbQuery.Append("\n 			,a.GNR_COD AS GenreNm ");
                sbQuery.Append("\n 			,a.CHNL_COD	AS ChannelNm ");
                sbQuery.Append("\n 			,a.SERIES_NO											AS	Series ");
                sbQuery.Append("\n 			, case EXCLS_YN when 'Y' then '����' else '' end	AS IsExclusive ");
                sbQuery.Append("\n 			,a.ITEM_NO AS ItemNo, b.ITEM_NM AS ItemName ");
                sbQuery.Append("\n 			,a.SCHD_SEQ AS SchSeq ");
                sbQuery.Append("\n 			,c1.STM_COD_NM												AS  AdType ");
                sbQuery.Append("\n 			,c2.STM_COD_NM												AS  AdStatus ");
                sbQuery.Append("\n 			,c3.STM_COD_NM												AS  FileStatus ");
                sbQuery.Append("\n 			,b.FILE_NM AS FileName ");
                sbQuery.Append("\n from	SCHD_DIST_HST a		 ");
                sbQuery.Append("\n inner join ADVT_MST b		on b.ITEM_NO = a.ITEM_NO ");
                sbQuery.Append("\n inner join STM_COD		c1	on b.ADVT_TYP	=	c1.STM_COD	and c1.STM_COD_CLS = '26' -- �������� ");
                sbQuery.Append("\n inner join STM_COD		c2	on b.ADVT_STT	=	c2.STM_COD	and c2.STM_COD_CLS = '25' -- ������� ");
                sbQuery.Append("\n inner join STM_COD		c3	on b.FILE_STT	=	c3.STM_COD	and c3.STM_COD_CLS = '31' -- ���ϻ��� ");
                sbQuery.Append("\n left outer join MENU_COD		c   on c.MENU_COD	= a.GNR_COD ");
                sbQuery.Append("\n where	ACK_NO = :AckNo ");
                sbQuery.Append("\n order by CAT_COD,GNR_COD,CHNL_COD,SERIES_NO,SCHD_SEQ ");



				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// ��� DataSet�� �ش�𵨿� ����
				schPublishModel.ScheduleDataSet = ds.Copy();
				ds.Dispose();

				// ���
				schPublishModel.ResultCnt = Utility.GetDatasetCount(schPublishModel.ScheduleDataSet);
				// ����ڵ� ��Ʈ
				schPublishModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + schPublishModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetScheduleList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD = "3000";
				schPublishModel.ResultDesc = "��������Ȳ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}


		#endregion
        
		#region ����������
		/// <summary>
		/// ����������
		/// </summary>
		/// <returns></returns>
		public void SetScheduleAck(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetScheduleAck() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int i  = 0;
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
                        + "UPDATE SCHD_DIST_MST          \n"
                        + "   SET ACK_STT   = '20'      \n"  // ���λ��� 20:������
                        + "      ,ACK_ID = :AckUser  \n"
                        + "      ,ACK_DT  = SYSDATE \n"
                        + "      ,ACK_DESC = :AckDesc  \n"
                        + " WHERE ACK_NO   = :AckNo    \n"
                        + "   AND ACK_STT   = '10'      \n"
						);

                    OracleParameter[] sqlParams = new OracleParameter[3];

					i = 0;
                    sqlParams[i++] = new OracleParameter(":AckUser", OracleDbType.Varchar2, 10);
                    sqlParams[i++] = new OracleParameter(":AckDesc", OracleDbType.Varchar2, 200);
                    sqlParams[i++] = new OracleParameter(":AckNo", OracleDbType.Int32);
                    
                    

					i = 0;
                    sqlParams[i++].Value = header.UserID;
                    sqlParams[i++].Value = schPublishModel.AckDesc;
                    sqlParams[i++].Value = Convert.ToInt32(schPublishModel.AckNo);
					
					
						
					_db.BeginTran();

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("���������� : ���ι�ȣ[" + schPublishModel.AckNo + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPublishModel.ResultCD = "0000";  // ����
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetScheduleAck() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD   = "3101";
				schPublishModel.ResultDesc = "���������� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion

		#region ���������� ���
		/// <summary>
		/// ���������� ���
		/// </summary>
		/// <returns></returns>
		public void SetScheduleAckCancel(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetScheduleAckCancel() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int i  = 0;
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
                        + "UPDATE SCHD_DIST_MST          \n"
                        + "   SET ACK_STT   = '10'      \n"  // ���λ��� 10:����
                        + "      ,ACK_ID = :AckUser  \n"
                        + "      ,ACK_DT  = SYSDATE \n"
                        + "      ,ACK_DESC = :AckDesc  \n"
                        + " WHERE ACK_NO   = :AckNo    \n"
                        + "   AND ACK_STT   = '20'      \n"
						);

                    OracleParameter[] sqlParams = new OracleParameter[3];

					i = 0;
                    sqlParams[i++] = new OracleParameter(":AckUser", OracleDbType.Varchar2, 10);
                    sqlParams[i++] = new OracleParameter(":AckDesc", OracleDbType.Varchar2, 200);
                    sqlParams[i++] = new OracleParameter(":AckNo", OracleDbType.Int32);
                    
                    

					i = 0;
                    sqlParams[i++].Value = header.UserID;
					sqlParams[i++].Value = schPublishModel.AckDesc;
                    sqlParams[i++].Value = Convert.ToInt32(schPublishModel.AckNo);

					_db.BeginTran();

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("���������� ���: ���ι�ȣ[" + schPublishModel.AckNo + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPublishModel.ResultCD = "0000";  // ����
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetScheduleAckCancel() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD   = "3101";
				schPublishModel.ResultDesc = "���������� ��� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion
        
		#region ����˼�����
		/// <summary>
		/// ����˼�����
		/// </summary>
		/// <returns></returns>
		public void SetScheduleChk(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetScheduleChk() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int i  = 0;
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
                        + "UPDATE SCHD_DIST_MST          \n"
                        + "   SET ACK_STT   = '25'      \n"  // ���λ��� 25:�������
                        + "      ,CHK_ID = :ChkUser  \n"
                        + "      ,CHK_DT  = SYSDATE \n"
                        + "      ,CHK_DESC = :ChkDesc  \n"
                        + " WHERE ACK_NO   = :AckNo    \n"
                        + "   AND ACK_STT   = '20'      \n"
						);

                    OracleParameter[] sqlParams = new OracleParameter[3];

					i = 0;
                    sqlParams[i++] = new OracleParameter(":ChkUser", OracleDbType.Varchar2, 10);
                    sqlParams[i++] = new OracleParameter(":ChkDesc", OracleDbType.Varchar2, 200);
                    sqlParams[i++] = new OracleParameter(":AckNo", OracleDbType.Int32);
                    
					i = 0;
                    sqlParams[i++].Value = header.UserID;
                    sqlParams[i++].Value = schPublishModel.ChkDesc;
					sqlParams[i++].Value = Convert.ToInt32(schPublishModel.AckNo);
					
						
					_db.BeginTran();

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("����˼����� : ���ι�ȣ[" + schPublishModel.AckNo + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPublishModel.ResultCD = "0000";  // ����
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetScheduleChk() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD   = "3101";
				schPublishModel.ResultDesc = "����˼����� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion

		#region ����˼����� ���
		/// <summary>
		/// ����˼����� ���
		/// </summary>
		/// <returns></returns>
		public void SetScheduleChkCancel(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetScheduleChkCancel() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int i  = 0;
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
                        + "UPDATE SCHD_DIST_MST          \n"
                        + "   SET ACK_STT   = '20'      \n"  // ���λ��� 20:������
                        + "      ,CHK_ID = :ChkUser  \n"
                        + "      ,CHK_DT  = SYSDATE \n"
                        + "      ,CHK_DESC = :ChkDesc  \n"
                        + " WHERE ACK_NO   = :AckNo    \n"
                        + "   AND ACK_STT   = '25'      \n"
						);

					OracleParameter[] sqlParams = new OracleParameter[3];

					i = 0;
                    sqlParams[i++] = new OracleParameter(":ChkUser", OracleDbType.Varchar2, 10);
                    sqlParams[i++] = new OracleParameter(":ChkDesc", OracleDbType.Varchar2, 200);
					sqlParams[i++] = new OracleParameter(":AckNo"       , OracleDbType.Int32         );
					

					i = 0;
                    sqlParams[i++].Value = header.UserID;
                    sqlParams[i++].Value = schPublishModel.ChkDesc;
					sqlParams[i++].Value = Convert.ToInt32(schPublishModel.AckNo);
					
						
					_db.BeginTran();

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("����˼����� ���: ���ι�ȣ[" + schPublishModel.AckNo + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPublishModel.ResultCD = "0000";  // ����
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetScheduleChkCancel() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD   = "3101";
				schPublishModel.ResultDesc = "����˼����� ��� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion
        
		#region �����������
		/// <summary>
		/// ����������� ó��
		/// S1������Ʈ��
		/// </summary>
		/// <param name="header"></param>
		/// <param name="schPublishModel"></param>
		public void SetSchedulePublish_S1(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchedulePublish() Start");
				_log.Debug("-----------------------------------------");

				try
				{
					int rc			= 0;
					int	adSchType	= 0;

					StringBuilder  sbQuery   = new StringBuilder();
						
					_db.BeginTran();


					#region [ ������ Ÿ�� ���ϱ� ]
                    sbQuery.Append("\n select	SCHD_TP AS AdSchType");
                    sbQuery.Append("\n from		SCHD_DIST_MST");
                    sbQuery.Append("\n where	ACK_NO	= " + schPublishModel.AckNo);
                    sbQuery.Append("\n and		ACK_STT	= '25'");

					// ��������
					DataSet ds = new DataSet();
					_db.ExecuteQuery(ds,sbQuery.ToString());

					if( ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
					{
						adSchType	= Convert.ToInt32( ds.Tables[0].Rows[0]["AdSchType"].ToString() );
					}
					else
					{
						adSchType	= 99;
					}
					ds.Dispose();

					if ( adSchType == 99 )
					{
						throw new Exception("������Ÿ���� �߸��Ǿ����ϴ�!!!");
					}
					#endregion

					#region [ �Ķ���� ���� ]
					OracleParameter[] sqlParams = new OracleParameter[3];

					
					sqlParams[0] = new OracleParameter(":PublishUser"	, OracleDbType.Varchar2,  10);
					sqlParams[1] = new OracleParameter(":PublishDesc"	, OracleDbType.Varchar2, 200);
                    sqlParams[2] = new OracleParameter(":AckNo", OracleDbType.Int32);
					
					sqlParams[0].Value	= header.UserID;
					sqlParams[1].Value	= schPublishModel.PublishDesc;
                    sqlParams[2].Value	= Convert.ToInt32(schPublishModel.AckNo);
					#endregion

					#region [ ���� ������������ ���� ]
					sbQuery = new StringBuilder();
                    sbQuery.Append("\n UPDATE SCHD_DIST_MST	");
                    sbQuery.Append("\n SET	 ACK_STT   = '30'	");
                    sbQuery.Append("\n		,DIST_ID = :PublishUser	");
                    sbQuery.Append("\n		,DIST_DT  = SYSDATE	");
                    sbQuery.Append("\n		,DIST_DESC = :PublishDesc	");
                    sbQuery.Append("\n WHERE ACK_NO	= :AckNo	");
                    sbQuery.Append("\n AND	 ACK_STT  = '25' 	");
			
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					if ( rc < 1 )
					{
						throw new Exception("������ ���� ���濡 �����߽��ϴ�!!!");
					}
					#endregion
			
					if ( adSchType == 10 )
					{
						#region [ Ȩ���������γ��� �߰� ]
                        // 2012.3 CommonYn�κ� �߰���

                        sqlParams = new OracleParameter[1];

                        sqlParams[0] = new OracleParameter(":AckNo", OracleDbType.Int32);
                        sqlParams[0].Value = Convert.ToInt32(schPublishModel.AckNo);

						sbQuery = new StringBuilder();
                        sbQuery.Append("\n	INSERT INTO SCHD_DIST_HST ");
                        sbQuery.Append("\n	(				 ACK_NO");
                        sbQuery.Append("\n	 				,SCHD_TP");
                        sbQuery.Append("\n	 				,CAT_COD");
                        sbQuery.Append("\n	 				,GNR_COD");
                        sbQuery.Append("\n	 				,CHNL_COD");
                        sbQuery.Append("\n	 				,SERIES_NO");
                        sbQuery.Append("\n	 				,EXCLS_YN");
                        sbQuery.Append("\n	 				,ITEM_NO");
                        sbQuery.Append("\n					,SCHD_SEQ ");
                        sbQuery.Append("\n					,COMM_YN )");
                        sbQuery.Append("\n	select AckNo,SchType,NVL(Category,'0'),NVL(Genre,'0'),NVL(Channel,'0'),NVL(Series,'0'),IsExclusive,ItemNo,NVL(SchSeq,'0'),max(CommonYn) ");
						sbQuery.Append("\n	from (	select   :AckNo				as  AckNo");
						sbQuery.Append("\n					,'10'				as	SchType");
						sbQuery.Append("\n					,''				as	Category");
                        sbQuery.Append("\n					,A.MENU_COD			as	Genre");
						sbQuery.Append("\n					,''				as	Channel");
						sbQuery.Append("\n					,''				as  Series");
						sbQuery.Append("\n					,'N'				as	IsExclusive");
                        sbQuery.Append("\n					,a.ITEM_NO			as	ItemNo");
                        sbQuery.Append("\n					,a.SCHD_ORD     	as  SchSeq");
                        sbQuery.Append("\n					,1                  as  CommonYn");
                        sbQuery.Append("\n			from	SCHD_MENU	a ");
                        sbQuery.Append("\n			left outer join ADVT_MST	b ");
                        sbQuery.Append("\n						on	b.ITEM_NO = a.ITEM_NO and b.ADVT_STT < '40' and b.FILE_STT < '90' ");
						sbQuery.Append("\n			union all										");
						sbQuery.Append("\n			select	 :AckNo				as  AckNo");
						sbQuery.Append("\n					,'10'				as	SchType");
						sbQuery.Append("\n					,''				as	Category");
						sbQuery.Append("\n					,''				as	Genre");
                        sbQuery.Append("\n					,A.CH_NO			as	Channel");
						sbQuery.Append("\n					,''				as  Series");
						sbQuery.Append("\n					,'N'				as	IsExclusive");
                        sbQuery.Append("\n					,a.ITEM_NO			as	ItemNo");
                        sbQuery.Append("\n					,a.SCHD_ORD     	as  SchSeq");
                        sbQuery.Append("\n					,1                  as  CommonYn");
                        sbQuery.Append("\n			from	SCHD_CHNL	a ");
                        sbQuery.Append("\n			left outer join ADVT_MST	b  ");
                        sbQuery.Append("\n						on	b.ITEM_NO = a.ITEM_NO and b.ADVT_STT < '40' and b.FILE_STT < '90' ");
						sbQuery.Append("\n	) v2");
						sbQuery.Append("\n	group by AckNo,SchType,Category,Genre,Channel,Series,IsExclusive,ItemNo,SchSeq");
						#endregion
                    }
					else
					{
						throw new Exception("������ Ÿ���� ��ȿ�� ���� �ƴմϴ�!!!");
					}
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();
					_log.Message("����������� : ���ι�ȣ[" + schPublishModel.AckNo + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPublishModel.ResultCD = "0000";  // ����
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchedulePublish() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD   = "3101";
				schPublishModel.ResultDesc = "����������� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion

		#region ����������� ���
		/// <summary>
		/// ����������� ���
		/// </summary>
		/// <returns></returns>
		public void SetSchedulePublishCancel(HeaderModel header, SchPublishModel schPublishModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchedulePublishCancel() Start");
				_log.Debug("-----------------------------------------");

				
				// ��������
				try
				{
					int i  = 0;
					int rc = 0;
					StringBuilder  sbQuery   = new StringBuilder();
					sbQuery.Append( "\n"
                        + "UPDATE SCHD_DIST_MST          \n"
                        + "   SET ACK_STT   = '25'      \n"  // ���λ��� 25:�������
                        + "      ,DIST_ID = :PublishUser  \n"
                        + "      ,DIST_DT  = SYSDATE \n"
                        + "      ,DIST_DESC = :PublishDesc  \n"
                        + " WHERE ACK_NO   = :AckNo    \n"
                        + "   AND ACK_STT   = '30'      \n"
						);

					OracleParameter[] sqlParams = new OracleParameter[3];

					i = 0;
                    sqlParams[i++] = new OracleParameter(":PublishUser", OracleDbType.Varchar2, 10);
                    sqlParams[i++] = new OracleParameter(":PublishDesc", OracleDbType.Varchar2, 200);
					sqlParams[i++] = new OracleParameter(":AckNo"       , OracleDbType.Int32         );
					

					i = 0;
                    sqlParams[i++].Value = header.UserID;
                    sqlParams[i++].Value = schPublishModel.PublishDesc;
					sqlParams[i++].Value = Convert.ToInt32(schPublishModel.AckNo);
					
						
					_db.BeginTran();

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					_db.CommitTran();

					// __MESSAGE__
					_log.Message("����������� ���: ���ι�ȣ[" + schPublishModel.AckNo + "] �����:[" + header.UserID + "]");
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				schPublishModel.ResultCD = "0000";  // ����
				

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetSchedulePublishCancel() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				schPublishModel.ResultCD   = "3101";
				schPublishModel.ResultDesc = "����������� ��� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}
		}

		#endregion
	}
}