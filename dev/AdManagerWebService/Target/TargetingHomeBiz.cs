
/*
 * -------------------------------------------------------
 * Class Name: TargetingHomeBiz
 * �ֿ���  : Ȩ Ÿ���� ó�� ����
 * �ۼ���    : ��
 * �ۼ���    : ��
 * Ư�̻���  : �������� Ȯ���� ���� ��� �߰� -bae
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.07.29
 * �����κ�  : 
 *             - GetRegionList(..)
 * ��������  : 
 *            - �������� 3�ܰ� ���� Ȯ������ ���ǹ� ����
 *              
 * --------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : �躸��
 * ������    : 2012.01.17
 * �����κ�  : 
 *             - GetRegionList(..)
 * ��������  : 
 *            - �������� ��Ʈ�� �������� ����
 *   
 * --------------------------------------------------------
 * �����ڵ�  : [E_03]
 * ������    : H.J.LEE
 * ������    : 2014.08.19
 * �����κ�  :
 *			  - GetAgeList(..)
 * ��������  : 
 *            - DB ����ȭ �۾����� HanaTV , Summary�� �и���
 *            - Summary�� ���̺��� ���� �Լ��� �־ �ش�
 *            �Լ��� Summary�� ������ ������
 * --------------------------------------------------------
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Target
{
	/// <summary>
	/// TargetingHomeBiz�� ���� ��� �����Դϴ�.
	/// </summary>
	public class TargetingHomeBiz : BaseBiz
	{

		#region  ������
		public TargetingHomeBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region ������ ��ȸ
		/// <summary>
		/// Ȩ���� �����ȸ,��� �׸����
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetTargetingList(HeaderModel header, TargetingHomeModel targetingHomeModel)
		{
			bool isState = false;

			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTargetingList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey            :[" + targetingHomeModel.SearchKey            + "]");		// �˻���
				_log.Debug("SearchMediaCode	     :[" + targetingHomeModel.SearchMediaCode	   + "]");		// �˻� ��ü
				_log.Debug("SearchRapCode        :[" + targetingHomeModel.SearchRapCode        + "]");		// �˻� ��
				_log.Debug("SearchAgencyCode     :[" + targetingHomeModel.SearchAgencyCode     + "]");		// �˻� �����
				_log.Debug("SearchAdvertiserCode :[" + targetingHomeModel.SearchAdvertiserCode + "]");		// �˻� ������
				_log.Debug("SearchContractState  :[" + targetingHomeModel.SearchContractState  + "]");		// �˻� ������
				_log.Debug("SearchAdType         :[" + targetingHomeModel.SearchAdType         + "]");		// �˻� ��������
				_log.Debug("SearchchkAdState_20  :[" + targetingHomeModel.SearchchkAdState_20  + "]");		// �˻� ������� : ��
				_log.Debug("SearchchkAdState_30  :[" + targetingHomeModel.SearchchkAdState_30  + "]");		// �˻� ������� : ����	
				_log.Debug("SearchchkAdState_40  :[" + targetingHomeModel.SearchchkAdState_40  + "]");		// �˻� ������� : ����           
				_log.Debug("SearchchkTimeY  :[" + targetingHomeModel.SearchchkTimeY  + "]");		// �˻� Ÿ���ÿ��� : ����           
				_log.Debug("SearchchkTimeN  :[" + targetingHomeModel.SearchchkTimeN  + "]");		// �˻� Ÿ���ÿ��� : �̼���           

				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ "SELECT B.ItemNo                      \n"
					+ "      ,B.ItemName                    \n"
					+ "      ,C.ContractName                \n"
					+ "      ,D.AdvertiserName              \n"
					+ "      ,E.CodeName as ContStateName   \n"
					+ "      ,B.ExcuteStartDay              \n"
					+ "      ,B.ExcuteEndDay                \n"
					+ "      ,B.RealEndDay                  \n"
					+ "      ,F.CodeName as AdTypeName      \n"
					+ "      ,G.CodeName as AdStatename     \n"
					+ "      ,I.CodeName as FileStateName   \n"
					+ "      ,C.ContractAmt                 \n"
					+ "      ,H.PriorityCd                  \n"
					+ "      ,B.MediaCode                   \n"
					+ "      ,ISNULL(H.ItemNo,0)  As TgtItemNo \n"
					+ "      ,H.ContractAmt AS TgtAmount    \n"
					+ "		 ,isNull(H.TgtCollectionYn,'N')	as TgtCollection \n"
					+ "		 ,H.TgtRegion1Yn \n"
					+ "		 ,H.TgtTimeYn \n"
					+ "		 ,H.TgtWeekYn \n"
					+ "  FROM ContractItem B with(nolock)  \n"
					+ "       INNER JOIN Contract   C with(nolock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq) \n"
					+ "       LEFT  JOIN Advertiser D with(nolock) ON (B.AdvertiserCode = D.AdvertiserCode)      \n"
					+ "       LEFT  JOIN SystemCode E with(nolock) ON (C.State          = E.Code AND E.Section = '23')  \n"	// 23 : ������
					+ "       LEFT  JOIN SystemCode F with(nolock) ON (B.AdType         = F.Code AND F.Section = '26')  \n"	// 26 : ��������
					+ "       LEFT  JOIN SystemCode G with(nolock) ON (B.AdState        = G.Code AND G.Section = '25')  \n"	// 25 : �������
					+ "       LEFT  JOIN SystemCode I with(nolock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 25 : �������
					+ "       LEFT  JOIN TargetingHome  H with(nolock) ON (B.ItemNo         = H.ItemNo)  \n"	
					+ " WHERE 1 = 1   \n"
					+ "   AND B.AdType != '13'" // ��������  10~19:�ʼ�����
					);

				// ��ü�� ����������
				if(targetingHomeModel.SearchMediaCode.Trim().Length > 0 && !targetingHomeModel.SearchMediaCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND B.MediaCode  = " + targetingHomeModel.SearchMediaCode.Trim() + " \n");
				}	
				
				// ���縦 ����������
				if(targetingHomeModel.SearchRapCode.Trim().Length > 0 && !targetingHomeModel.SearchRapCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND B.RapCode  = " + targetingHomeModel.SearchRapCode.Trim() + " \n");
				}	

				// ����縦 ����������
				if(targetingHomeModel.SearchAgencyCode.Trim().Length > 0 && !targetingHomeModel.SearchAgencyCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND B.AgencyCode  = " + targetingHomeModel.SearchAgencyCode.Trim() + " \n");
				}	

				// �����ָ� ����������
				if(targetingHomeModel.SearchAdvertiserCode.Trim().Length > 0 && !targetingHomeModel.SearchAdvertiserCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND B.AdvertiserCode  = " + targetingHomeModel.SearchAdvertiserCode.Trim() + " \n");
				}	

				// �����¸� ����������
				if(targetingHomeModel.SearchContractState.Trim().Length > 0 && !targetingHomeModel.SearchContractState.Trim().Equals("00"))
				{
					sbQuery.Append(" AND C.State  = '" + targetingHomeModel.SearchContractState.Trim() + "' \n");
				}	

				// ���������� ����������
				if(targetingHomeModel.SearchAdType.Trim().Length > 0 && !targetingHomeModel.SearchAdType.Trim().Equals("00"))
				{
					sbQuery.Append(" AND B.AdType  = '" + targetingHomeModel.SearchAdType.Trim() + "' \n");
				}

				// ������� ���ÿ� ����
				// ������´� 20:�� �� 40:���� ���̿� �ִ� �͸� ��ȸ�Ѵ�.
				sbQuery.Append(" AND B.AdState >= '20' AND B.AdState <= '40' \n");

				// ������� ��
				if(targetingHomeModel.SearchchkAdState_20.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND ( B.AdState  = '20' \n");
					isState = true;
				}	
				// ������� ����
				if(targetingHomeModel.SearchchkAdState_30.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" B.AdState  = '30' \n");
					isState = true;
				}	
				// ������� ����
				if(targetingHomeModel.SearchchkAdState_40.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" B.AdState  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");
				
				// �˻�� ������
				if (targetingHomeModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append("\n"
						+ "  AND ( B.ItemNo         LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
						+ "     OR B.ItemName       LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
						+ "     OR C.ContractName   LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
						+ "     OR D.AdvertiserName LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
						+ "  ) \n"
						);
				}

				// Ÿ���ÿ���-������ ����������
				if(targetingHomeModel.SearchchkTimeY.Trim().Length > 0 && targetingHomeModel.SearchchkTimeY.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeN.Trim().Equals(""))
				{
					sbQuery.Append(" AND H.TgtTimeYn IS NOT NULL \n");
				}

				// Ÿ���ÿ���-�̼����� ����������
				if(targetingHomeModel.SearchchkTimeN.Trim().Length > 0 && targetingHomeModel.SearchchkTimeN.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeY.Trim().Equals(""))
				{
					sbQuery.Append(" AND H.TgtTimeYn IS NULL \n");
				}
				// Ÿ���ÿ���-����,�̼����� ����������..��ü�� �����ش�.
				if(targetingHomeModel.SearchchkTimeY.Trim().Length > 0 && targetingHomeModel.SearchchkTimeN.Trim().Length > 0 && targetingHomeModel.SearchchkTimeY.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeN.Trim().Equals("Y"))
				{					
				}
       
				sbQuery.Append("  ORDER BY B.ItemNo DESC ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �帣�׷�𵨿� ����
				targetingHomeModel.TargetingDataSet = ds.Copy();
				// ���
				targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.TargetingDataSet);
				// ����ڵ� ��Ʈ
				targetingHomeModel.ResultCD = "0000";

				ds.Dispose();

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTargetingList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD = "3000";
				targetingHomeModel.ResultDesc = "������������Ȳ ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
        /// <summary>
        /// Ȩ���� �����ȸ,��� �׸����
        /// </summary>
        /// <param name="targetingHomeModel"></param>
        public void GetTargetingList2(HeaderModel header, TargetingHomeModel targetingHomeModel)
        {
            bool isState = false;

            try
            {
                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("SearchKey            :[" + targetingHomeModel.SearchKey + "]");		// �˻���
                _log.Debug("SearchMediaCode	     :[" + targetingHomeModel.SearchMediaCode + "]");		// �˻� ��ü
                _log.Debug("SearchRapCode        :[" + targetingHomeModel.SearchRapCode + "]");		// �˻� ��
                _log.Debug("SearchAgencyCode     :[" + targetingHomeModel.SearchAgencyCode + "]");		// �˻� �����
                _log.Debug("SearchAdvertiserCode :[" + targetingHomeModel.SearchAdvertiserCode + "]");		// �˻� ������
                _log.Debug("SearchContractState  :[" + targetingHomeModel.SearchContractState + "]");		// �˻� ������
                _log.Debug("SearchAdType         :[" + targetingHomeModel.SearchAdType + "]");		// �˻� ��������
                _log.Debug("SearchchkAdState_20  :[" + targetingHomeModel.SearchchkAdState_20 + "]");		// �˻� ������� : ��
                _log.Debug("SearchchkAdState_30  :[" + targetingHomeModel.SearchchkAdState_30 + "]");		// �˻� ������� : ����	
                _log.Debug("SearchchkAdState_40  :[" + targetingHomeModel.SearchchkAdState_40 + "]");		// �˻� ������� : ����           
                _log.Debug("SearchchkTimeY  :[" + targetingHomeModel.SearchchkTimeY + "]");		// �˻� Ÿ���ÿ��� : ����           
                _log.Debug("SearchchkTimeN  :[" + targetingHomeModel.SearchchkTimeN + "]");		// �˻� Ÿ���ÿ��� : �̼���           

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + "SELECT B.ItemNo                      \n"
                    + "      ,B.ItemName                    \n"
                    + "      ,C.ContractName                \n"
                    + "      ,D.AdvertiserName              \n"
                    + "      ,E.CodeName as ContStateName   \n"
                    + "      ,B.ExcuteStartDay              \n"
                    + "      ,B.ExcuteEndDay                \n"
                    + "      ,B.RealEndDay                  \n"
                    + "      ,F.CodeName as AdTypeName      \n"
                    + "      ,G.CodeName as AdStatename     \n"
                    + "      ,I.CodeName as FileStateName   \n"
                    + "      ,C.ContractAmt                 \n"
                    + "      ,H.PriorityCd                  \n"
                    + "      ,B.MediaCode                   \n"
                    + "      ,ISNULL(H.ItemNo,0)  As TgtItemNo \n"
                    + "      ,H.ContractAmt AS TgtAmount    \n"
                    + "		 ,isNull(H.TgtCollectionYn,'N')	as TgtCollection \n"
                    + "		 ,H.TgtRegion1Yn \n"
                    + "		 ,H.TgtTimeYn \n"
                    + "		 ,H.TgtWeekYn \n"
                    + "      ,H.TgtStbTypeYn  \n"
                    + "  FROM ContractItem B with(nolock)  \n"
                    + "       INNER JOIN Contract   C with(nolock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq) \n"
                    + "       LEFT  JOIN Advertiser D with(nolock) ON (B.AdvertiserCode = D.AdvertiserCode)      \n"
                    + "       LEFT  JOIN SystemCode E with(nolock) ON (C.State          = E.Code AND E.Section = '23')  \n"	// 23 : ������
                    + "       LEFT  JOIN SystemCode F with(nolock) ON (B.AdType         = F.Code AND F.Section = '26')  \n"	// 26 : ��������
                    + "       LEFT  JOIN SystemCode G with(nolock) ON (B.AdState        = G.Code AND G.Section = '25')  \n"	// 25 : �������
                    + "       LEFT  JOIN SystemCode I with(nolock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 25 : �������
                    + "       LEFT  JOIN TargetingHome  H with(nolock) ON (B.ItemNo         = H.ItemNo)  \n"
                    + " WHERE 1 = 1   \n"
                    + "   AND B.AdType != '13'" // ��������  10~19:�ʼ�����
                    );

                // ��ü�� ����������
                if (targetingHomeModel.SearchMediaCode.Trim().Length > 0 && !targetingHomeModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.MediaCode  = " + targetingHomeModel.SearchMediaCode.Trim() + " \n");
                }

                // ���縦 ����������
                if (targetingHomeModel.SearchRapCode.Trim().Length > 0 && !targetingHomeModel.SearchRapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.RapCode  = " + targetingHomeModel.SearchRapCode.Trim() + " \n");
                }

                // ����縦 ����������
                if (targetingHomeModel.SearchAgencyCode.Trim().Length > 0 && !targetingHomeModel.SearchAgencyCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.AgencyCode  = " + targetingHomeModel.SearchAgencyCode.Trim() + " \n");
                }

                // �����ָ� ����������
                if (targetingHomeModel.SearchAdvertiserCode.Trim().Length > 0 && !targetingHomeModel.SearchAdvertiserCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.AdvertiserCode  = " + targetingHomeModel.SearchAdvertiserCode.Trim() + " \n");
                }

                // �����¸� ����������
                if (targetingHomeModel.SearchContractState.Trim().Length > 0 && !targetingHomeModel.SearchContractState.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND C.State  = '" + targetingHomeModel.SearchContractState.Trim() + "' \n");
                }

                // ���������� ����������
                if (targetingHomeModel.SearchAdType.Trim().Length > 0 && !targetingHomeModel.SearchAdType.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.AdType  = '" + targetingHomeModel.SearchAdType.Trim() + "' \n");
                }

                // ������� ���ÿ� ����
                // ������´� 20:�� �� 40:���� ���̿� �ִ� �͸� ��ȸ�Ѵ�.
                sbQuery.Append(" AND B.AdState >= '20' AND B.AdState <= '40' \n");

                // ������� ��
                if (targetingHomeModel.SearchchkAdState_20.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( B.AdState  = '20' \n");
                    isState = true;
                }
                // ������� ����
                if (targetingHomeModel.SearchchkAdState_30.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '30' \n");
                    isState = true;
                }
                // ������� ����
                if (targetingHomeModel.SearchchkAdState_40.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");

                // �˻�� ������
                if (targetingHomeModel.SearchKey.Trim().Length > 0)
                {
                    // �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
                    sbQuery.Append("\n"
                        + "  AND ( B.ItemNo         LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.ItemName       LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
                        + "     OR C.ContractName   LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
                        + "     OR D.AdvertiserName LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                // Ÿ���ÿ���-������ ����������
                if (targetingHomeModel.SearchchkTimeY.Trim().Length > 0 && targetingHomeModel.SearchchkTimeY.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeN.Trim().Equals(""))
                {
                    sbQuery.Append(" AND H.TgtTimeYn IS NOT NULL \n");
                }

                // Ÿ���ÿ���-�̼����� ����������
                if (targetingHomeModel.SearchchkTimeN.Trim().Length > 0 && targetingHomeModel.SearchchkTimeN.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeY.Trim().Equals(""))
                {
                    sbQuery.Append(" AND H.TgtTimeYn IS NULL \n");
                }
                // Ÿ���ÿ���-����,�̼����� ����������..��ü�� �����ش�.
                if (targetingHomeModel.SearchchkTimeY.Trim().Length > 0 && targetingHomeModel.SearchchkTimeN.Trim().Length > 0 && targetingHomeModel.SearchchkTimeY.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeN.Trim().Equals("Y"))
                {
                }

                sbQuery.Append("  ORDER BY B.ItemNo DESC ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �帣�׷�𵨿� ����
                targetingHomeModel.TargetingDataSet = ds.Copy();
                // ���
                targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.TargetingDataSet);
                // ����ڵ� ��Ʈ
                targetingHomeModel.ResultCD = "0000";

                ds.Dispose();

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingHomeModel.ResultCD = "3000";
                targetingHomeModel.ResultDesc = "������������Ȳ ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }
		#endregion

		#region Ÿ�ٱ������ȸ

		/// <summary>
		/// Ÿ�ٱ������ȸ
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetCollectionList(HeaderModel header, TargetingHomeModel targetingHomeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCollectionList() Start");
				_log.Debug("-----------------------------------------");

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("SearchKey      :[" + targetingHomeModel.SearchKey       + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn            \n"
					+ "		  ,A.CollectionCode, A.CollectionName, A.Comment  \n"								
					+ "       ,A.UseYn              \n"
					+ "       ,CASE A.UseYn WHEN 'Y' THEN '���' WHEN 'N' THEN '������' END AS UseYn_N  \n"
					+ "       ,convert(char(19), A.RegDt, 120) RegDt              \n"
					+ "       ,convert(char(19), A.ModDt, 120) ModDt              \n"					
					+ "       ,B.UserName RegName                                 \n"
					+ "       ,(Select count(*) from ClientList where CollectionCode = A.CollectionCode AND CollectionCode = A.CollectionCode) Cnt           \n"
					+ "  FROM Collection A LEFT JOIN SystemUser B with(NoLock) ON (A.RegId          = B.UserId)        \n"					
					+ " WHERE 1 = 1  \n"
                    + "   AND A.UseYn = 'Y' "
                    );
			
				// �˻�� ������
				if (targetingHomeModel.SearchKey.Trim().Length > 0)
				{
					// �����÷��� ���Ͽ� LIKE �˻��� �Ѵ�.
					sbQuery.Append(" AND ("
						+ "  A.CollectionName      LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"	
						+ " OR A.Comment    LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
						+ " OR B.UserName    LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}
				
				sbQuery.Append(" ORDER BY A.CollectionCode desc \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �̵��𵨿� ����
				targetingHomeModel.CollectionsDataSet = ds.Copy();
				// ���
				targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.CollectionsDataSet);
				// ����ڵ� ��Ʈ
				targetingHomeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCollectionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD = "3000";
				targetingHomeModel.ResultDesc = "Ÿ�ٰ������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();			
			}		
		}
		#endregion

		#region ���� Ÿ���� �� ��ȸ
		/// <summary>
		/// ���� Ÿ���� �� ��ȸ
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetTargetingDetail(HeaderModel header, TargetingHomeModel targetingHomeModel)
		{
            try
            {
                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("ItemNo :[" + targetingHomeModel.ItemNo + "]");		// �����ȣ


                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + "SELECT ItemNo                     \n"
                    + "      ,ContractAmt                 \n"
                    + "      ,PriorityValue               \n"
                    + "      ,PriorityCd                  \n"
                    + "      ,AmtVariableHour             \n"
                    + "      ,AmtControlYn                \n"
                    + "      ,AmtControlRate              \n"
                    + "      ,TgtRegion1Yn                \n"
                    + "      ,TgtRegion1                  \n"
                    + "      ,TgtTimeYn                   \n"
                    + "      ,TgtTime                     \n"
                    + "      ,TgtAgeYn                    \n"
                    + "      ,TgtAge                      \n"
                    + "      ,TgtAgeBtnYn                 \n"
                    + "      ,TgtAgeBtnBegin              \n"
                    + "      ,TgtAgeBtnEnd                \n"
                    + "      ,TgtSexYn                    \n"
                    + "      ,TgtSexMan                   \n"
                    + "      ,TgtSexWoman                 \n"
                    + "      ,TgtWeekYn                   \n"
                    + "      ,TgtWeek                     \n"
                    + "      ,TgtCollectionYn                   \n"
                    + "      ,( select count(*) from  TargetingCollection x with(nolock) where x.ItemNo = y.ItemNo and x.SetType = '2') as TgtCollection    \n"
                    + "  FROM TargetingHome y with(nolock)     \n"
                    + " WHERE ItemNo = " + targetingHomeModel.ItemNo + "   \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �帣�׷�𵨿� ����
                targetingHomeModel.DetailDataSet = ds.Copy();
                // ���
                targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.DetailDataSet);
                // ����ڵ� ��Ʈ
                targetingHomeModel.ResultCD = "0000";

                ds.Dispose();

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() End");
                _log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD = "3000";
				targetingHomeModel.ResultDesc = "���� Ÿ���� �� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}

        /// <summary>
        /// ���� Ÿ���� �� ��ȸ
        /// </summary>
        /// <param name="targetingHomeModel"></param>
        public void GetTargetingDetail2(HeaderModel header, TargetingHomeModel targetingHomeModel)
        {
            try
            {
                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<�Է�����>");
                _log.Debug("ItemNo :[" + targetingHomeModel.ItemNo + "]");		// �����ȣ


                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + "SELECT ItemNo                     \n"
                    + "      ,ContractAmt                 \n"
                    + "      ,PriorityValue               \n"
                    + "      ,PriorityCd                  \n"
                    + "      ,AmtVariableHour             \n"
                    + "      ,AmtControlYn                \n"
                    + "      ,AmtControlRate              \n"
                    + "      ,TgtRegion1Yn                \n"
                    + "      ,TgtRegion1                  \n"
                    + "      ,TgtTimeYn                   \n"
                    + "      ,TgtTime                     \n"
                    + "      ,TgtAgeYn                    \n"
                    + "      ,TgtAge                      \n"
                    + "      ,TgtAgeBtnYn                 \n"
                    + "      ,TgtAgeBtnBegin              \n"
                    + "      ,TgtAgeBtnEnd                \n"
                    + "      ,TgtSexYn                    \n"
                    + "      ,TgtSexMan                   \n"
                    + "      ,TgtSexWoman                 \n"
                    + "      ,TgtWeekYn                   \n"
                    + "      ,TgtWeek                     \n"
                    + "      ,TgtCollectionYn             \n"
                    + "      ,( select count(*) from  TargetingCollection x with(nolock) where x.ItemNo = y.ItemNo and x.SetType = '2') as TgtCollection    \n"
                    + "      ,TgtStbTypeYn                \n"
                    + "      ,TgtStbType                  \n"
                    + "      ,TgtPocYn                    \n"
                    + "      ,TgtPoc       	              \n"
                    + "  FROM TargetingHome y with(nolock)     \n"
                    + " WHERE ItemNo = " + targetingHomeModel.ItemNo + "   \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �帣�׷�𵨿� ����
                targetingHomeModel.DetailDataSet = ds.Copy();
                // ���
                targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.DetailDataSet);
                // ����ڵ� ��Ʈ
                targetingHomeModel.ResultCD = "0000";

                ds.Dispose();

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingHomeModel.ResultCD = "3000";
                targetingHomeModel.ResultDesc = "���� Ÿ���� �� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }
		#endregion

		#region ���� Ÿ���� ����

		/// <summary>
		/// ���� Ÿ���� �� ����
		/// </summary>
		/// <returns></returns>
		public void SetTargetingDetailUpdate(HeaderModel header, TargetingHomeModel targetingHomeModel)
		{

			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetTargetingDetailUpdate() Start");
				_log.Debug("-----------------------------------------");

				_log.Debug("ItemNo         :[" + targetingHomeModel.ItemNo         + "]");		
				_log.Debug("ItemName       :[" + targetingHomeModel.ItemName       + "]");		
				_log.Debug("ContractAmt    :[" + targetingHomeModel.ContractAmt    + "]");		
				_log.Debug("PriorityCd     :[" + targetingHomeModel.PriorityCd     + "]");		
				_log.Debug("AmtControlYn   :[" + targetingHomeModel.AmtControlYn   + "]");		
				_log.Debug("AmtControlRate :[" + targetingHomeModel.AmtControlRate + "]");		
				_log.Debug("TgtRegion1Yn   :[" + targetingHomeModel.TgtRegion1Yn   + "]");		
				_log.Debug("TgtRegion1     :[" + targetingHomeModel.TgtRegion1     + "]");		
				_log.Debug("TgtTimeYn      :[" + targetingHomeModel.TgtTimeYn      + "]");		
				_log.Debug("TgtTime        :[" + targetingHomeModel.TgtTime        + "]");		
				_log.Debug("TgtAgeYn       :[" + targetingHomeModel.TgtAgeYn       + "]");		
				_log.Debug("TgtAge         :[" + targetingHomeModel.TgtAge         + "]");		
				_log.Debug("TgtAgeBtnYn    :[" + targetingHomeModel.TgtAgeBtnYn    + "]");		
				_log.Debug("TgtAgeBtnBegin :[" + targetingHomeModel.TgtAgeBtnBegin + "]");		
				_log.Debug("TgtAgeBtnEnd   :[" + targetingHomeModel.TgtAgeBtnEnd   + "]");		
				_log.Debug("TgtSexYn       :[" + targetingHomeModel.TgtSexYn       + "]");		
				_log.Debug("TgtSexMan      :[" + targetingHomeModel.TgtSexMan      + "]");		
				_log.Debug("TgtSexWoman    :[" + targetingHomeModel.TgtSexWoman    + "]");		
//				_log.Debug("TgtRateYn      :[" + targetingHomeModel.TgtRateYn      + "]");		
//				_log.Debug("TgtRate        :[" + targetingHomeModel.TgtRate        + "]");		
				_log.Debug("TgtWeekYn      :[" + targetingHomeModel.TgtWeekYn      + "]");		
				_log.Debug("TgtWeek        :[" + targetingHomeModel.TgtWeek        + "]");		
				_log.Debug("TgtCollectionYn      :[" + targetingHomeModel.TgtCollectionYn      + "]");		
                //_log.Debug("TgtCollection        :[" + targetingHomeModel.TgtCollection        + "]");	
                _log.Debug("TgtStbYn        :[" + targetingHomeModel.TgtStbModelYn  + "]");
                _log.Debug("TgtStb          :[" + targetingHomeModel.TgtStbModel    + "]");
                _log.Debug("TgtPocYn        :[" + targetingHomeModel.TgtPocYn       + "]");
                _log.Debug("TgtPoc          :[" + targetingHomeModel.TgtPoc         + "]");	

				int i = 0;
				int rc = 0;
				StringBuilder  sbQuery;
				SqlParameter[] sqlParams = new SqlParameter[24];
	            
				i = 0;
				sqlParams[i++] = new SqlParameter("@ItemNo"          , SqlDbType.Int        );
				sqlParams[i++] = new SqlParameter("@ContractAmt"     , SqlDbType.Decimal    );
				sqlParams[i++] = new SqlParameter("@PriorityCd"      , SqlDbType.TinyInt    );
				sqlParams[i++] = new SqlParameter("@AmtControlYn"    , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@AmtControlRate"  , SqlDbType.SmallInt   );
				sqlParams[i++] = new SqlParameter("@TgtRegion1Yn"    , SqlDbType.Char     ,1);
				//sqlParams[i++] = new SqlParameter("@TgtRegion1"      , SqlDbType.VarChar,512);
				sqlParams[i++] = new SqlParameter("@TgtRegion1"      , SqlDbType.VarChar,2000); // [E_01] �������� Ȯ��� ���� Length Ȯ��
                sqlParams[i++] = new SqlParameter("@TgtTimeYn"       , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtTime"         , SqlDbType.VarChar,128);
				sqlParams[i++] = new SqlParameter("@TgtAgeYn"        , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtAge"          , SqlDbType.VarChar,128);
				sqlParams[i++] = new SqlParameter("@TgtAgeBtnYn"     , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtAgeBtnBegin"  , SqlDbType.TinyInt    );
				sqlParams[i++] = new SqlParameter("@TgtAgeBtnEnd"    , SqlDbType.TinyInt    );
				sqlParams[i++] = new SqlParameter("@TgtSexYn"        , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtSexMan"       , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtSexWoman"     , SqlDbType.Char     ,1);
//				sqlParams[i++] = new SqlParameter("@TgtRateYn"       , SqlDbType.Char     ,1);
//				sqlParams[i++] = new SqlParameter("@TgtRate"         , SqlDbType.SmallInt   );
				sqlParams[i++] = new SqlParameter("@TgtWeekYn"       , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtWeek"         , SqlDbType.VarChar ,13);
				sqlParams[i++] = new SqlParameter("@TgtCollectionYn"       , SqlDbType.Char     ,1);
                //sqlParams[i++] = new SqlParameter("@TgtCollection"         , SqlDbType.Int		  );
                sqlParams[i++] = new SqlParameter("@TgtStbYn", SqlDbType.Char, 1);
                sqlParams[i++] = new SqlParameter("@TgtStb", SqlDbType.VarChar, 512);
                sqlParams[i++] = new SqlParameter("@TgtPocYn", SqlDbType.Char, 1);
                sqlParams[i++] = new SqlParameter("@TgtPoc", SqlDbType.VarChar, 512);

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.ItemNo);        
				sqlParams[i++].Value = Convert.ToDecimal(targetingHomeModel.ContractAmt);   
				sqlParams[i++].Value = Convert.ToInt16(targetingHomeModel.PriorityCd);    
				sqlParams[i++].Value = targetingHomeModel.AmtControlYn;    
				sqlParams[i++].Value = Convert.ToInt16(targetingHomeModel.AmtControlRate);    
				sqlParams[i++].Value = targetingHomeModel.TgtRegion1Yn;    
				sqlParams[i++].Value = targetingHomeModel.TgtRegion1;    
				sqlParams[i++].Value = targetingHomeModel.TgtTimeYn;    
				sqlParams[i++].Value = targetingHomeModel.TgtTime;    
				sqlParams[i++].Value = targetingHomeModel.TgtAgeYn;    
				sqlParams[i++].Value = targetingHomeModel.TgtAge;    
				sqlParams[i++].Value = targetingHomeModel.TgtAgeBtnYn;    
				sqlParams[i++].Value = Convert.ToInt16(targetingHomeModel.TgtAgeBtnBegin);    
				sqlParams[i++].Value = Convert.ToInt16(targetingHomeModel.TgtAgeBtnEnd);    
				sqlParams[i++].Value = targetingHomeModel.TgtSexYn;    
				sqlParams[i++].Value = targetingHomeModel.TgtSexMan;    
				sqlParams[i++].Value = targetingHomeModel.TgtSexWoman;    
//				sqlParams[i++].Value = targetingHomeModel.TgtRateYn;    
//				sqlParams[i++].Value = Convert.ToInt16(targetingHomeModel.TgtRate);    
				sqlParams[i++].Value = targetingHomeModel.TgtWeekYn;    
				sqlParams[i++].Value = targetingHomeModel.TgtWeek;    
				sqlParams[i++].Value = targetingHomeModel.TgtCollectionYn;    
                //sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.TgtCollection);  
                sqlParams[i++].Value = targetingHomeModel.TgtStbModelYn;
                sqlParams[i++].Value = targetingHomeModel.TgtStbModel;
                sqlParams[i++].Value = targetingHomeModel.TgtPocYn;
                sqlParams[i++].Value = targetingHomeModel.TgtPoc;    

				// ��������
				try
				{
					_db.BeginTran();


					sbQuery = new StringBuilder();
					sbQuery.Append(" UPDATE TargetingHome						  \n");
					sbQuery.Append("    SET ContractAmt		= @ContractAmt	  \n");
					sbQuery.Append("       ,PriorityCd		= @PriorityCd	  \n");
					sbQuery.Append("       ,AmtControlYn  	= @AmtControlYn    \n");
					sbQuery.Append("       ,AmtControlRate	= @AmtControlRate  \n");
					sbQuery.Append("       ,TgtRegion1Yn  	= @TgtRegion1Yn    \n");
					sbQuery.Append("       ,TgtRegion1    	= @TgtRegion1      \n");
					sbQuery.Append("       ,TgtTimeYn     	= @TgtTimeYn       \n");
					sbQuery.Append("       ,TgtTime       	= @TgtTime         \n");
					sbQuery.Append("       ,TgtAgeYn      	= @TgtAgeYn        \n");
					sbQuery.Append("       ,TgtAge        	= @TgtAge          \n");
					sbQuery.Append("       ,TgtAgeBtnYn   	= @TgtAgeBtnYn     \n");
					sbQuery.Append("       ,TgtAgeBtnBegin	= @TgtAgeBtnBegin  \n");
					sbQuery.Append("       ,TgtAgeBtnEnd  	= @TgtAgeBtnEnd    \n");
					sbQuery.Append("       ,TgtSexYn      	= @TgtSexYn        \n");
					sbQuery.Append("       ,TgtSexMan     	= @TgtSexMan       \n");
					sbQuery.Append("       ,TgtSexWoman   	= @TgtSexWoman     \n");
//					sbQuery.Append("       ,TgtRateYn     	= @TgtRateYn       \n");
//					sbQuery.Append("       ,TgtRate       	= @TgtRate         \n");
					sbQuery.Append("       ,TgtWeekYn     	= @TgtWeekYn       \n");
					sbQuery.Append("       ,TgtWeek       	= @TgtWeek         \n");
					sbQuery.Append("       ,TgtCollectionYn     	= @TgtCollectionYn       \n");

                    sbQuery.Append("       ,TgtStbTypeYn    = @TgtStbYn         \n");
                    sbQuery.Append("       ,TgtStbType      = @TgtStb           \n");
                    sbQuery.Append("       ,TgtPocYn       	= @TgtPocYn         \n");
                    sbQuery.Append("       ,TgtPoc       	= @TgtPoc           \n");

                    //sbQuery.Append("       ,TgtCollection       	= @TgtCollection         \n");
					sbQuery.Append("  WHERE ItemNo          = @ItemNo		  \n");


					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					if(rc < 1)
					{
						// ����ä�� �� ���̺� �߰�
						sbQuery = new StringBuilder();
						sbQuery.Append("\n");
						sbQuery.Append(" INSERT INTO TargetingHome    \n");
						sbQuery.Append("            (ItemNo			  \n");
						sbQuery.Append("            ,ContractAmt	  \n");
						sbQuery.Append("            ,PriorityAmt	  \n");
                        sbQuery.Append("            ,PriorityValue	  \n");
						sbQuery.Append("            ,PriorityCd		  \n");
						sbQuery.Append("            ,AmtControlYn    \n");
						sbQuery.Append("            ,AmtControlRate  \n");
						sbQuery.Append("            ,TgtRegion1Yn    \n");
						sbQuery.Append("            ,TgtRegion1      \n");
						sbQuery.Append("            ,TgtTimeYn       \n");
						sbQuery.Append("            ,TgtTime         \n");
						sbQuery.Append("            ,TgtAgeYn        \n");
						sbQuery.Append("            ,TgtAge          \n");
						sbQuery.Append("            ,TgtAgeBtnYn     \n");
						sbQuery.Append("            ,TgtAgeBtnBegin  \n");
						sbQuery.Append("            ,TgtAgeBtnEnd    \n");
						sbQuery.Append("            ,TgtSexYn        \n");
						sbQuery.Append("            ,TgtSexMan       \n");
						sbQuery.Append("            ,TgtSexWoman     \n");
//						sbQuery.Append("            ,TgtRateYn       \n");
//						sbQuery.Append("            ,TgtRate         \n");
						sbQuery.Append("            ,TgtWeekYn       \n");
						sbQuery.Append("            ,TgtWeek         \n");
						sbQuery.Append("            ,TgtCollectionYn       \n");
                        sbQuery.Append("            ,TgtStbTypeYn    \n");
                        sbQuery.Append("            ,TgtStbType      \n");
                        sbQuery.Append("            ,TgtPocYn        \n");
                        sbQuery.Append("            ,TgtPoc       	 \n");
                        //sbQuery.Append("            ,TgtCollection         \n");
						sbQuery.Append("            )				  \n");
						sbQuery.Append("      VALUES				  \n");
						sbQuery.Append("            (@ItemNo		  \n");
						sbQuery.Append("            ,@ContractAmt	  \n");
                        sbQuery.Append("            ,1000000            \n");
						sbQuery.Append("            ,0           	  \n");
						sbQuery.Append("            ,@PriorityCd	  \n");
						sbQuery.Append("            ,@AmtControlYn    \n");
						sbQuery.Append("            ,@AmtControlRate  \n");
						sbQuery.Append("            ,@TgtRegion1Yn    \n");
						sbQuery.Append("            ,@TgtRegion1      \n");
						sbQuery.Append("            ,@TgtTimeYn       \n");
						sbQuery.Append("            ,@TgtTime         \n");
						sbQuery.Append("            ,@TgtAgeYn        \n");
						sbQuery.Append("            ,@TgtAge          \n");
						sbQuery.Append("            ,@TgtAgeBtnYn     \n");
						sbQuery.Append("            ,@TgtAgeBtnBegin  \n");
						sbQuery.Append("            ,@TgtAgeBtnEnd    \n");
						sbQuery.Append("            ,@TgtSexYn        \n");
						sbQuery.Append("            ,@TgtSexMan       \n");
						sbQuery.Append("            ,@TgtSexWoman     \n");
//						sbQuery.Append("            ,@TgtRateYn       \n");
//						sbQuery.Append("            ,@TgtRate         \n");
						sbQuery.Append("            ,@TgtWeekYn       \n");
						sbQuery.Append("            ,@TgtWeek         \n");
						sbQuery.Append("            ,@TgtCollectionYn       \n");
                        sbQuery.Append("            ,@TgtStbYn         \n");
                        sbQuery.Append("            ,@TgtStb           \n");
                        sbQuery.Append("            ,@TgtPocYn         \n");
                        sbQuery.Append("            ,@TgtPoc           \n");
                        //sbQuery.Append("            ,@TgtCollection         \n");
						sbQuery.Append(" 			)				  \n");

						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					}

					// 2007.10.25 RH.Jung ���Ⱚ�缳���� ������ ���ص� ��. �������� �ǽð� ó����
					//SetPriorityValues(targetingHomeModel.ItemNo);

					_db.CommitTran();
            
					// __MESSAGE__
					_log.Message("���� Ÿ���� ����:["+targetingHomeModel.ItemNo +"] " + targetingHomeModel.ItemName + " �����:[" + header.UserID + "]");
            
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
            
				targetingHomeModel.ResultCD = "0000";  // ����
            
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetingDetailUpdate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD   = "3101";
				targetingHomeModel.ResultDesc = "���� Ÿ���� ���� �� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}

		#endregion

		#region ���Ⱚ �缳��

		/// <summary>
		/// ���Ⱚ �缳��
		/// </summary>
		/// <returns></returns>
		private void SetPriorityValues(string ItemNo)
		{

			try
			{
				string MediaCode = "";

				StringBuilder  sbQuery   = null;

				// �ش� ���� ���Ͽ� �ִ� ��ü�� ã�´�.
				sbQuery   = new StringBuilder();
				sbQuery.Append( "\n"
					+ " SELECT B.MediaCode \n"
					+ "  FROM TargetingHome  A INNER JOIN ContractItem B ON (A.ItemNo = B.ItemNo ) \n"
					);

				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					MediaCode = Utility.GetDatasetString(ds, 0, "MediaCode");					
				}
				else
				{
					throw new Exception();
				}
				ds.Dispose();


				// �ش� ��ü�� ����̸� ������ ������� ����󵵰��� �հ踦 ���Ѵ�.
				// �ش� ��ü�� ����̸� ������ ������� ���Ⱚ�� �����Ѵ�.
				// ���Ⱚ = ������� �հ� / ����� * 10

				sbQuery   = new StringBuilder();
				sbQuery.Append( "\n"
					+ " DECLARE @SumCd int, @ItemNo int;  \n"
					+ "                                   \n"
					+ "SELECT @SumCd  = SUM(A.PriorityCd) \n"
					+ "  FROM TargetingHome A with(nolock)    \n"
					+ "       INNER JOIN ContractItem B  with(nolock) ON (A.ItemNo = B.ItemNo AND B.AdState = '20' AND B.AdType BETWEEN '10' AND '19') -- ������� 20:��  �������� 10~19:�ʼ����� \n"
					+ "       INNER JOIN Contract     C  with(nolock) ON (B.MediaCode      = C.MediaCode      AND \n"
					+ "                                                 B.RapCode        = C.RapCode        AND \n"
					+ "                                                 B.AgencyCode     = C.AgencyCode     AND \n"
					+ "                                                 B.AdvertiserCode = C.AdvertiserCode AND \n"
					+ "                                                 B.ContractSeq    = C.ContractSeq    AND \n"
					+ "                                                 C.State  = '10' ) -- ������ 10:��� \n"
					+ " WHERE B.MediaCode = " + MediaCode + "\n"
					+ "                                      \n"
					+ "DECLARE SRC_CUR CURSOR                \n"
					+ "    FOR SELECT A.ItemNo               \n"
					+ "          FROM TargetingHome A  with(nolock) \n"
					+ "               INNER JOIN ContractItem B with(nolock) ON (A.ItemNo = B.ItemNo AND B.AdState = '20' AND B.AdType BETWEEN '10' AND '19') -- ������� 20:��  �� �������� 10~19:�ʼ�����\n"
					+ "               INNER JOIN Contract     C with(nolock) ON (B.MediaCode      = C.MediaCode      AND \n"
					+ "                                                         B.RapCode        = C.RapCode        AND \n" 
					+ "                                                         B.AgencyCode     = C.AgencyCode     AND \n"
					+ "                                                         B.AdvertiserCode = C.AdvertiserCode AND \n"
					+ "                                                         B.ContractSeq    = C.ContractSeq    AND \n"
					+ "                                                         C.State  = '10' ) -- ������ 10:��� \n"
					+ "        WHERE B.MediaCode = " + MediaCode + "\n"
					+ "                                     \n"
					+ "OPEN SRC_CUR                         \n"
					+ "                                     \n"
					+ "FETCH NEXT FROM SRC_CUR INTO @ItemNo \n"
					+ "                                     \n"
					+ "WHILE @@FETCH_STATUS = 0             \n"
					+ "BEGIN                                \n"
					+ "                                     \n"
					+ "    UPDATE TargetingHome                 \n"
					+ "       SET PriorityValue =  FLOOR( (@SumCd/CAST(PriorityCd as float)) * 10)  \n"
					+ "	    WHERE ItemNo = @ItemNo          \n"
					+ "                                      \n"
					+ "	FETCH NEXT FROM SRC_CUR INTO @ItemNo \n"
					+ "                                      \n"    
					+ "END                                   \n"
					+ "                                      \n"
					+ "CLOSE SRC_CUR                         \n"
					+ "DEALLOCATE SRC_CUR                    \n"
					);

				int rc =  _db.ExecuteNonQuery(sbQuery.ToString());

			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}

		}


		
		

		#endregion

        #region [E_02] �������� ��ȸ
        /// <summary>
		/// �������� ��ȸ
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetRegionList(HeaderModel header, TargetingHomeModel targetingHomeModel)
		{
			try
			{
				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetRegionList() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();
				
				/* [E_01]�� ���ؼ� �ּ�
				// ��������
				sbQuery.Append("\n"
					+ "-- �����ڵ���ȸ      \n"
					+ "SELECT SummaryCode AS RegionCode    \n"
					+ "      ,SummaryName AS RegionName    \n"
					+ "      ,UpperCode       \n"
					+ "      ,Level           \n"
					+ "  FROM SummaryCode   with(nolock)    \n"	
					+ " WHERE SummaryType = 5 \n"
					+ " ORDER BY SummaryCode  \n"
					);
				*/

				// [E_01] ��� �߰�
                /*
                sbQuery.Append(" Select                           \n");
                sbQuery.Append(" 	 SummaryCode As RegionCode    \n");
                sbQuery.Append(" 	,SummaryName As RegionName    \n");
                sbQuery.Append(" 	,UpperCode                    \n");
                sbQuery.Append(" 	,[Level]                      \n");
                sbQuery.Append(" From  SummaryCode                \n"); 
                sbQuery.Append(" Where 1 = 1                      \n");
                sbQuery.Append(" And   SummaryType = 5            \n");
                sbQuery.Append(" Order By Orders                  \n");
                */

                // [E_02] ����
                sbQuery.Append(" Select                           \n");
                sbQuery.Append(" 	 RegionCode As RegionCode    \n");
                sbQuery.Append(" 	,RegionName As RegionName    \n");
                sbQuery.Append(" 	,UpperCode                    \n");
                sbQuery.Append(" 	,[Level]                      \n");
                sbQuery.Append(" From  TargetRegion nolock        \n");
                sbQuery.Append(" Order By Orders                  \n");
       
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �帣�׷�𵨿� ����
				targetingHomeModel.RegionDataSet = ds.Copy();
				// ���
				targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.RegionDataSet);
				// ����ڵ� ��Ʈ
				targetingHomeModel.ResultCD = "0000";

				ds.Dispose();

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetRegionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD = "3000";
				targetingHomeModel.ResultDesc = "�������� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
		#endregion

		#region ���ɴ� ��ȸ
		/// <summary>
		/// ���ɴ� ��ȸ
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetAgeList(HeaderModel header, TargetingHomeModel targetingHomeModel)
		{
			try
			{
                // [E_03] GetAgeList() �Լ��� AdtargetsSummary�� ������ ����
                _db.ConnectionString = FrameSystem.connSummaryDbString;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgeList() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();

				// ��������
				sbQuery.Append("\n"
					+ "-- ���ɴ� ��ȸ      \n"
					+ "SELECT SummaryCode AS AgeCode    \n"
					+ "      ,SummaryName AS AgeName    \n"
					+ "      ,UpperCode       \n"
					+ "      ,Level           \n"
					+ "  FROM SummaryCode   with(nolock)    \n"	
					+ " WHERE SummaryType = 3 -- 3:���ɴ�\n"
					+ "   AND SummaryCode < 9000        \n"
					+ " ORDER BY SummaryCode  \n"
					);
       
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �帣�׷�𵨿� ����
				targetingHomeModel.AgeDataSet = ds.Copy();
				// ���
				targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.AgeDataSet);
				// ����ڵ� ��Ʈ
				targetingHomeModel.ResultCD = "0000";

				ds.Dispose();

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD = "3000";
				targetingHomeModel.ResultDesc = "���ɴ� ��ȸ�� ������ �߻��Ͽ����ϴ�";
				_log.Exception(ex);
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
		#endregion

        #region ����Ÿ���� ��ȸ
        /// <summary>
        /// ���ɴ� ��ȸ
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingCollectionList(HeaderModel header, TargetingHomeModel targetingHomeModel)
        {
            try
            {
                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTagetingCollectionList() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();

                // ��������
                sbQuery.Append("\n"
                    + "-- ����Ÿ���� ��ȸ					\n"
                    + "SELECT 'False' AS CheckYn            \n"
                    + "		  ,A.CollectionCode, B.CollectionName, isnull(A.Sign,'-') as Sign   \n"
                    + "  FROM TargetingCollection A with(nolock) LEFT JOIN Collection B with(nolock) ON (A.CollectionCode = B.CollectionCode)   \n"
                    + " WHERE A.ItemNo = " + targetingHomeModel.ItemNo + "   \n"
                    + "   AND A.SetType = '2' -- 1:�Ϲ� 2:Ȩ����  \n"
                    + " ORDER BY CollectionCode  \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // ��������
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // ��� DataSet�� �帣�׷�𵨿� ����
                targetingHomeModel.TargetingCollectionDataSet = ds.Copy();
                // ���
                targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.TargetingCollectionDataSet);
                // ����ڵ� ��Ʈ
                targetingHomeModel.ResultCD = "0000";

                ds.Dispose();

                // __DEBUG__
                _log.Debug("<�������>");
                _log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTagetingCollectionList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingHomeModel.ResultCD = "3000";
                targetingHomeModel.ResultDesc = "����Ÿ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }

        }
        #endregion

        #region ����Ÿ���� �߰�
        /// <summary>
        /// ���� ���
        /// </summary>
        /// <param name="header"></param>
        /// <param name="ratioModel"></param>
        public void SetTargetingCollectionAdd(HeaderModel header, TargetingHomeModel targetingHomeModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[3];

                sbQuery.Append(""
                    + "INSERT INTO TargetingCollection ( \n"
                    + "      SetType                  \n"
                    + "		,ItemNo                   \n"
                    + "		,CollectionCode           \n"
					+ "		,Sign			           \n"
                    + "      )                        \n"
                    + " VALUES(                       \n"
                    + "       '2'  -- 1:�Ϲ� 2:Ȩ���� \n"
                    + "      ,@ItemNo				  \n"
                    + "      ,@CollectionCode	      \n"
					+ "      ,@Sign				      \n"
                    + "		)						  \n"

                    );

                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Sign", SqlDbType.Char, 1);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.ItemNo);
                sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.CollectionCode);
				sqlParams[i++].Value = targetingHomeModel.TgtCollectionYn.Trim();

                _log.Debug("ItemNo      :[" + targetingHomeModel.ItemNo + "]");
                _log.Debug("CollCode	:[" + targetingHomeModel.CollectionCode + "]");
				_log.Debug("Sign		:[" + targetingHomeModel.TgtCollectionYn + "]");

                _log.Debug(sbQuery.ToString());

                // ��������
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();
                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                targetingHomeModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingHomeModel.ResultCD = "3101";
                targetingHomeModel.ResultDesc = "����Ÿ���� �߰��� ������ �߻��Ͽ����ϴ�";
                _log.Exception(ex);
            }
            finally
            {
                // �����ͺ��̽���  Close�Ѵ�
                _db.Close();
            }
        }


        #endregion

        #region ����Ÿ���� ����
        /// <summary>
        /// ���� ���
        /// </summary>
        /// <param name="header"></param>
        /// <param name="ratioModel"></param>
        public void SetTargetingCollectionDelete(HeaderModel header, TargetingHomeModel targetingHomeModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

                // �����ͺ��̽��� OPEN�Ѵ�
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[2];

                sbQuery.Append(""
                    + "DELETE FROM TargetingCollection         \n"
                    + "	WHERE SetType        = '2'  -- 1:�Ϲ� 2:Ȩ����  \n"
                    + "   AND ItemNo         = @ItemNo         \n" 
                    + "	  AND CollectionCode = @CollectionCode \n"
                    );

                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.ItemNo);
                sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.CollectionCode);

                _log.Debug("ItemNo        :[" + targetingHomeModel.ItemNo + "]");
                _log.Debug("CollectionCode:[" + targetingHomeModel.CollectionCode + "]");

                _log.Debug(sbQuery.ToString());

                // ��������
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                targetingHomeModel.ResultCD = "0000";  // ����

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingHomeModel.ResultCD = "3101";
                targetingHomeModel.ResultDesc = "����Ÿ���� �߰��� ������ �߻��Ͽ����ϴ�";
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