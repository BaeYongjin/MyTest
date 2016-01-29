// ===============================================================================
//
// AdTypeMoniteringBiz.cs
//
// ���� �Ѱ����� ���� 
//
// ===============================================================================
// Release history
// 2007.10.26 BJ.PARK OAP�� ���谡����� �� ��������̿� �޼ҵ� => GetContractItemList()
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: AdTypeMoniteringBiz
 * �ֿ���  : ���� �Ѱ����� ����
 * �ۼ���    : ��
 * �ۼ���    : ��
 * Ư�̻���  : ����
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : H.J.LEE
 * ������    : 2014.08.19
 * �����κ�  :
 *			  - ������
 *            - ��� ����
 * ��������  : 
 *            - DB ����ȭ �۾����� HanaTV , Summary�� �и���
 *            - Summary�� �ƴ� HanaTV�� �����ϴ� ��� ���̺�,
 *              ���ν��� ���� AdTargetsHanaTV.dbo.XX�� ����
 * --------------------------------------------------------
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Diagnostics;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.ReportSummaryAd
{
	/// <summary>
	/// AdTypeMoniteringBiz�� ���� ��� �����Դϴ�.
	/// TODO :	������������ ����DB�����.
	///			���� �α����赵 ������� �����ϸ� ������DB�� ����ؾ���.
	/// </summary>
	public class AdTypeMoniteringBiz : BaseBiz
	{

		#region  ������
		public AdTypeMoniteringBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region ���������� ������ ����
		/// <summary>
		///  ���������� ������ ����
		/// </summary>
		/// <param name="adTypeMoniteringModel"></param>
		public void GetAdTypeMaster(HeaderModel header, AdTypeMoniteringModel adTypeMoniteringModel)
		{
			bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdTypeMaster() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(adTypeMoniteringModel.LogDay.Length > 6) adTypeMoniteringModel.LogDay = adTypeMoniteringModel.LogDay.Substring(2,6);
								
				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("LogDay	  :[" + adTypeMoniteringModel.LogDay   + "]");		// �˻� ��ü						
				// __DEBUG__

				string logDay   = adTypeMoniteringModel.LogDay;
				
				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@LogDay", SqlDbType.VarChar, 6);
				sqlParams[0].Value = adTypeMoniteringModel.LogDay;
								
				// ��������
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* �ѱⰣ �������� ����																				\n"
					+ "   ��ȸ���� : ��ü�ڵ�, ����ȣ �Ǵ� �����ȣ */														\n"
					+ "																									\n"
					+ "DECLARE @LogDay varchar(6);   -- ��ü�ڵ�															\n"
																				
					+ "SET @LogDay   =		'" + logDay  + "'\n"					
					+ " 	select case when grouping(itm.AdType) = 1  then '99'   else itm.AdType end as adType																  \n"
                    + " 				,case when grouping(itm.AdType) = 1  then '��ü'  else ( select CodeName from AdTargetsHanaTV.dbo.SystemCode with(NoLock) where Section = '26' and itm.AdType = Code ) end as adTypeName	  \n"
					+ " 				,sum(dat.totCnt)	as Tot	  \n"
					+ " 				,sum(dat.h00) as h00 ,sum(dat.h01) as h01 ,sum(dat.h02) as h02																\n"
					+ " 				,sum(dat.h03) as h03 ,sum(dat.h04) as h04 ,sum(dat.h05) as h05                                                                \n"
					+ " 				,sum(dat.h06) as h06 ,sum(dat.h07) as h07 ,sum(dat.h08) as h08                                                               \n"
					+ " 				,sum(dat.h09) as h09 ,sum(dat.h10) as h10 ,sum(dat.h11) as h11                                                               \n"
					+ " 				,sum(dat.h12) as h12 ,sum(dat.h13) as h13 ,sum(dat.h14) as h14                                                               \n"
					+ " 				,sum(dat.h15) as h15 ,sum(dat.h16) as h16 ,sum(dat.h17) as h17                                                               \n"
					+ " 				,sum(dat.h18) as h18 ,sum(dat.h19) as h19 ,sum(dat.h20) as h20                                                               \n"
					+ " 				,sum(dat.h21) as h21 ,sum(dat.h22) as h22 ,sum(dat.h23) as h23                                                               \n"					
					+ " 		from  (	select ItemNo,sum(HitSum) as totCnt                                                                 \n"
					+ " 					  ,sum(hit00) h00,sum(Hit01) h01,sum(Hit02) h02,sum(Hit03) h03,sum(Hit04) h04                                                                \n"
					+ " 					  ,sum(hit05) h05,sum(Hit06) h06,sum(Hit07) h07,sum(Hit08) h08,sum(Hit09) h09                                                                \n"
					+ " 					  ,sum(Hit10) h10,sum(hit11) h11,sum(Hit12) h12,sum(Hit13) h13,sum(Hit14) h14                                                                \n"
					+ " 					  ,sum(Hit15) h15,sum(Hit16) h16,sum(hit17) h17,sum(Hit18) h18,sum(Hit19) h19                                                                \n"
					+ " 					  ,sum(Hit20) h20,sum(Hit21) h21,sum(Hit22) h22,sum(Hit23) h23                                                                \n"
                    + " 				from	SummaryAdGenre with(NoLock)                                                                \n"
					+ " 				where	LogDay = @LogDay                                                                \n"
					+ " 				group by ItemNo ) dat                                                                \n"
                    + " 		inner join AdTargetsHanaTV.dbo.ContractItem itm with(NoLock)	on itm.ItemNo = dat.ItemNo                                                                 \n"
					+ " 		group by itm.AdType with rollup;                                                                 \n"													
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				adTypeMoniteringModel.ReportDataSet = ds.Copy();

				// ���
				adTypeMoniteringModel.ResultCnt = Utility.GetDatasetCount(adTypeMoniteringModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				adTypeMoniteringModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + adTypeMoniteringModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdTypeMaster() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adTypeMoniteringModel.ResultCD = "3000";
				if(isNotReady)
				{
					adTypeMoniteringModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					adTypeMoniteringModel.ResultDesc = "���������� ������ ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
					_log.Exception(ex);
				}
			}
			finally
			{
				// �����ͺ��̽���  Close�Ѵ�
				_db.Close();
			}

		}
		#endregion

		#region ���������� ������ ����
		/// <summary>
		///  ���������� ������ ����
		/// </summary>
		/// <param name="adTypeMoniteringModel"></param>
		public void GetAdTypeDetail(HeaderModel header, AdTypeMoniteringModel adTypeMoniteringModel)
		{
			bool isNotReady = false; // �����Ͱ� ���谡 �����ʾ� �������� ������.
			try
			{
				StringBuilder sbQuery = null;

				// �����ͺ��̽��� OPEN�Ѵ�
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdTypeDetail() Start");
				_log.Debug("-----------------------------------------");

				// ���ڰ� 6�ڸ� �̻�(yyyymmdd)�̸� 6�ڸ��� �����.
				if(adTypeMoniteringModel.LogDay.Length > 6) adTypeMoniteringModel.LogDay = adTypeMoniteringModel.LogDay.Substring(2,6);
								
				// __DEBUG__
				_log.Debug("<�Է�����>");
				_log.Debug("LogDay	    :[" + adTypeMoniteringModel.LogDay  + "]");		// �˻� ��ü	
				_log.Debug("AdType	    :[" + adTypeMoniteringModel.AdType  + "]");					
                _log.Debug("AdRap       :[" + adTypeMoniteringModel.Rap     + "]");					
				// __DEBUG__

                
				string  logDay  = adTypeMoniteringModel.LogDay;
				string  adType  = adTypeMoniteringModel.AdType;				
                string  adRap   = adTypeMoniteringModel.Rap;
				
				// ��������
                sbQuery = new StringBuilder();
                sbQuery.Append(" " + "\n");
                sbQuery.Append(" /* �ѱⰣ �������� ���� " + "\n");
                sbQuery.Append("    ��ȸ���� : ��ü�ڵ�, ����ȣ �Ǵ� �����ȣ */ " + "\n");
                sbQuery.Append(" " + "\n");
                sbQuery.Append(" DECLARE @LogDay  varchar(6);      -- ��ü�ڵ� " + "\n");
                sbQuery.Append(" DECLARE @AdType  varchar(2);      -- �����ȣ " + "\n");
                sbQuery.Append(" DECLARE @AdRap   varchar(2);      -- �����ȣ " + "\n");
                sbQuery.Append(" SET @LogDay  =   '" + logDay     + "' " + "\n");
                sbQuery.Append(" SET @AdType  =	'" + adType     + "'; " + "\n");
                sbQuery.Append(" SET @AdRap   =	'" + adRap      + "'; " + "\n");
                sbQuery.Append(" select	 itm.ItemNo " + "\n");
                sbQuery.Append(" 		,itm.ItemName " + "\n");
                sbQuery.Append(" 		,dat.totCnt " + "\n");
                sbQuery.Append(" 		,dat.h00,dat.h01,dat.h02 " + "\n");
                sbQuery.Append(" 		,dat.h03,dat.h04,dat.h05 " + "\n");
                sbQuery.Append(" 		,dat.h06,dat.h07,dat.h08 " + "\n");
                sbQuery.Append(" 		,dat.h09,dat.h10,dat.h11 " + "\n");
                sbQuery.Append(" 		,dat.h12,dat.h13,dat.h14 " + "\n");
                sbQuery.Append(" 		,dat.h15,dat.h16,dat.h17 " + "\n");
                sbQuery.Append(" 		,dat.h18,dat.h19,dat.h20 " + "\n");
                sbQuery.Append(" 		,dat.h21,dat.h22,dat.h23 " + "\n");
                sbQuery.Append(" from (	select ItemNo,sum(HitSum) as totCnt " + "\n");
                sbQuery.Append(" 			  ,sum(hit00) h00,sum(Hit01) h01,sum(Hit02) h02,sum(Hit03) h03,sum(Hit04) h04 " + "\n");
                sbQuery.Append(" 			  ,sum(hit05) h05,sum(Hit06) h06,sum(Hit07) h07,sum(Hit08) h08,sum(Hit09) h09 " + "\n");
                sbQuery.Append(" 			  ,sum(Hit10) h10,sum(hit11) h11,sum(Hit12) h12,sum(Hit13) h13,sum(Hit14) h14 " + "\n");
                sbQuery.Append(" 			  ,sum(Hit15) h15,sum(Hit16) h16,sum(hit17) h17,sum(Hit18) h18,sum(Hit19) h19 " + "\n");
                sbQuery.Append(" 			  ,sum(Hit20) h20,sum(Hit21) h21,sum(Hit22) h22,sum(Hit23) h23 " + "\n");
                sbQuery.Append(" 		from	dbo.SummaryAdGenre with(NoLock) " + "\n");
                sbQuery.Append(" 		where	LogDay = @LogDay " + "\n");
                sbQuery.Append(" 		group by ItemNo ) dat " + "\n");
                sbQuery.Append(" inner join AdTargetsHanaTV.dbo.ContractItem itm with(NoLock) on itm.ItemNo = dat.ItemNo " + "\n");
                if ( !adType.Equals("99") )
                {
                    sbQuery.Append(" where itm.adType    = @AdType " + "\n");
                }
                if ( !adRap.Equals("00") )
                {
                    sbQuery.Append(" and   itm.RapCode   = @AdRap " + "\n");
                }
                sbQuery.Append(" order by itm.AdType " + "\n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// ��������
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// ��� DataSet�� �����͸𵨿� ����
				adTypeMoniteringModel.ReportDataSet = ds.Copy();

				// ���
				adTypeMoniteringModel.ResultCnt = Utility.GetDatasetCount(adTypeMoniteringModel.ReportDataSet);

				ds.Dispose();

				// ����ڵ� ��Ʈ
				adTypeMoniteringModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<�������>");
				_log.Debug("ResultCnt:[" + adTypeMoniteringModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdTypeDetail() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adTypeMoniteringModel.ResultCD = "3000";
				if(isNotReady)
				{
					adTypeMoniteringModel.ResultDesc = "�ش� �Ⱓ�� �����Ͱ� ������� �ʾҽ��ϴ�.";
				}
				else
				{
					adTypeMoniteringModel.ResultDesc = "���������� ������ ���� ��ȸ�� ������ �߻��Ͽ����ϴ�";
					_log.Exception(ex);
				}
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