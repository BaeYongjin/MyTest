/*
 * -------------------------------------------------------
 * Class Name: FrameSystem
 * �ֿ���  : ������ �ʱ� ����
 * �ۼ���    : bae 
 * �ۼ���    : 2010.06.07
 * Ư�̻���  : 
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.06.07
 * ��������  : 
 *            - IP�� �б��ؼ� ���� Ÿ�԰� DB ����IP������
 *              ó�������� Web.Config �� <SeverType>�߰��ؼ�
 *              ����Ÿ�� �� DB���� ������ ó���ϵ��� ��.
 *            - DBó�� Result �ڵ� ����-�ϵ��ڵ��� �κ���
 *              Web.Config �� ���Ǹ��ؼ� ó�� �ϵ��� ��.
 * --------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : �躸��
 * ������    : 2013.06.04
 * ��������  : 
 *            - �����߾��� CompressData() �Լ� ����
 * -------------------------------------------------------- */

using System;
using System.Net;
using System.Text;
using System.Data;
using System.Threading;
using System.Configuration;

// ������ ������ ���ؼ� �߰���(2013.04.30:��뼮)
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

using AdManagerWebService.Contract;

namespace AdManagerWebService
{
	/// <summary>
	/// �ý����� �⺻���� Ŭ����
	/// </summary>
	public class FrameSystem
	{
		#region ��������
		// �ý��� Ÿ��
		public const string  m_SystemType = "NSADV";	// AdTargets Service Manager Server
		public static string m_SystemVersion = "Ver 1.0";
		
		// �ý��� ���� ��
		public static SystemModel	oSysModel	= null;

		/// �α� ������Ʈ
		public static Logger oLog         = null;
		public static int    m_LogLevel   = Logger._DEBUG;

		// DB Connecting String
		public static string connDbString  = "";
        public static string connSummaryDbString = "";
		/// <summary>
		/// DBó�� ����-0000
		/// </summary>
		public static string DBSuccess  = "";
		/// <summary>
		/// DB����Ʈ ��ȸ����-3000
		/// </summary>
		public static string DBListFail = "";
		/// <summary>
		/// DB�߰� ����-3101
		/// </summary>
		public static string DBAddFail  = "";
		/// <summary>
		/// DB���� ����-3201
		/// </summary>
		public static string DBEditFail = "";
		/// <summary>
		/// DB���� ����-3301
		/// </summary>
		public static string DBDelFail  = "";
		#endregion

		private FrameSystem(){}
				
		/// <summary> 
		/// �ý����� �⵿�Ѵ�.
		/// </summary>
		public static int Start()
		{
			// Logger�� �⵿
			oLog = new Logger("C:\\Adtargets\\Log", m_SystemType, Logger._DEBUG);
			oLog.Start();

			oLog.Message("");
			oLog.Message("");
			oLog.Message("###########################################################");
			oLog.Message("#                                                         #");
			oLog.Message("#             AdTargetsPlus Manager Server v1.0           #");
			oLog.Message("#              Copyright(c) DARTmedia Inc.				#");
			oLog.Message("#                                                         #");
			oLog.Message("###########################################################");
			oLog.Message("");
			oLog.Message("AdTargetPlus Manager for BTV");
			oLog.Message("");

			// �ý��� ����
            m_SystemVersion = ConfigurationManager.AppSettings.Get("SystemVersion");
			oLog.Message("SYSTEM VERSION:"+m_SystemVersion);					
			
			#region ���� Ÿ�� ó�� �� db���� ���ڿ� ���� �����ڵ� E_01����
            string svrType = ConfigurationManager.AppSettings.Get("SeverType");
						
			if (svrType.ToUpper().Equals("DEV"))
			{	 
				// ��ȣȭ�� ���ؼ� ����
                //connDbString = Security.Decrypt(ConfigurationSettings.AppSettings.Get("ConnectionString_DEV"));
                connDbString = ConfigurationManager.AppSettings.Get("ConnectionString_DEV");
                connSummaryDbString = ConfigurationManager.AppSettings.Get("ConnectionString_Summary_DEV");
                oLog.Message("SERVER TYPE   :DEV");
                oLog.Message("connDbString  :" + connDbString);
                oLog.Message("connSummaryDbString:" + connSummaryDbString);
            }
			else if (svrType.ToUpper().Equals("TEST"))
			{
                connDbString = Security.Decrypt(ConfigurationManager.AppSettings.Get("ConnectionString_TEST"));
                connSummaryDbString = Security.Decrypt(ConfigurationManager.AppSettings.Get("ConnectionString_Summary_TEST"));
				oLog.Message("SERVER TYPE   :TEST");
			}
			else if (svrType.ToUpper().Equals("REAL"))
			{
                connDbString = Security.Decrypt(ConfigurationManager.AppSettings.Get("ConnectionString_REAL"));
                connSummaryDbString = Security.Decrypt(ConfigurationManager.AppSettings.Get("ConnectionString_Summary_REAL"));
				oLog.Message("SERVER TYPE   :REAL");
			}
			#endregion

			// �α׷���
            string sLogLevel = ConfigurationManager.AppSettings.Get("LogLevel");
			oLog.Message("LOG LEVEL     :" + sLogLevel);

			switch(sLogLevel)
			{
				case "DEBUG"   : m_LogLevel = Logger._DEBUG ;   break;
				case "MESSAGE" : m_LogLevel = Logger._MESSAGE;  break;
				case "WARNING" : m_LogLevel = Logger._WARNING;  break;
				case "ERROR"   : m_LogLevel = Logger._ERROR ;   break;
				default        : m_LogLevel = Logger._DEBUG ;   break;
			}			

			// ������ �α׷����� �α��� ��⵿
			oLog.Stop();
			oLog.Start(m_LogLevel);

			// �ý��۸� 
			oSysModel = new SystemModel(m_SystemType);
			oSysModel.SysIP       = "";
			oSysModel.SysVersion  = m_SystemVersion;
			
			// DBó�� �ڵ� ���� Reading �����ڵ� E_01
            DBSuccess = ConfigurationManager.AppSettings.Get("DBSuccess");
            DBListFail = ConfigurationManager.AppSettings.Get("DBListFail");
            DBAddFail = ConfigurationManager.AppSettings.Get("DBAddFail");
            DBEditFail = ConfigurationManager.AppSettings.Get("DBEditFail");
            DBDelFail = ConfigurationManager.AppSettings.Get("DBDelFail");
			
			oLog.Message("�����񽺰� ���۵Ǿ����ϴ�.");
			oLog.Message("");
			
			return 0;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static byte[] CompressData(DataSet ds)
        {
            // 1. DataSet�� ���̳ʸ� �������� ��ȯ�� ��, �迭�� ��ȯ
            ds.RemotingFormat = SerializationFormat.Binary;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, ds);
            byte[] inbyte = ms.ToArray();

            // 2. �޸� ��Ʈ���� �����ϰ� �迭�� ����
            MemoryStream objStream = new MemoryStream();
            System.IO.Compression.DeflateStream objZS = new DeflateStream(objStream, CompressionMode.Compress);

            objZS.Write(inbyte, 0, inbyte.Length);
            objZS.Flush();
            objZS.Close();

            return objStream.ToArray();
        }
	}
}