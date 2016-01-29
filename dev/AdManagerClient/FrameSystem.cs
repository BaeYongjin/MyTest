// ===============================================================================
// TMS Manager System Frame Helper for Charites Project
//
// FrameSystem.cs
//
// �ý����� ����� �� ������Ʈ���� ���� 
//
// ===============================================================================
// Release history
//  
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * -------------------------------------------------------
 * Class Name: FrameSystem
 * �ֿ���  : Client �ʱ� ����
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
 *            - DBó�� Result �ڵ� ����-�ϵ��ڵ��� �κ���
 *              App.Config �� ���Ǹ��ؼ� ó�� �ϵ��� ��.
 * --------------------------------------------------------
 * �����ڵ�  : 
 * ������    : RH.Jung
 * ������    : 2011.10.25
 * ��������  :           
 *            - ��Ÿ���� .NET 4 �� ����
 *            - IP��� GetHostEntry�� ����
 *            - ConfigurationSettings�� ConfigurationManager�� ����
 * --------------------------------------------------------
 * �����ڵ�  : 
 * ������    : YS.Jang
 * ������    : 2012.01.03
 * ��������  :           
 *              - MediaMode app.config�� �߰���.
 *              ����FTP���ε嶧����( &TV�� BTV�� �и���)
 * --------------------------------------------------------
 * * 
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Net;
using System.Web;
using System.Collections;
using System.Xml;
using System.Drawing;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

using System.Diagnostics;
using System.Net.Sockets;


using WinFramework.Misc;
using WinFramework.Base;

namespace AdManagerClient
{
	/// <summary>
	/// �ý����� �⺻���� Ŭ����
	/// </summary>
	public class FrameSystem
	{
		
		// �ý��� Ÿ��
		public const  string m_SystemType = "ASMC";	// AdTargets Service Manager Client
		public static string m_SystemVersion = "Ver 1.0";
		public static int    m_SystemTimeout =  500;

		// Ŭ���̾�Ʈ ����
		public const  int _DEV   = 0;
		public const  int _TEST  = 1;
		public const  int _REAL  = 2;
		public const  int _TEST_DEV = 3;
		public static int m_ClientType = _DEV;	

		// �ý��� Ŭ���̾�Ʈ Ű : SystemType + IP
		public static string m_ClientKey = null;

		// �ý��� ���� ��
		public static SystemModel	oSysModel	= null;

		// ����� ���� ��
		public static CommonModel	 oComModel	= null;

		/// �α� ������Ʈ
		public static Logger		oLog = null;

		/// Config ������Ʈ
		public static ConfigReader	oCfg = null;

		// �޴� ������Ʈ
		public static MenuPower     oMenu = null;

		/// ��������
		public static string	m_WebServer_Host	= "";
		public static int		m_WebServer_Port	= 8086;
        // BMK ���� ����
		// HACK [�������� ���ǻ���] ���������� ���ø����̼� ��Ī�� ��ġ ���Ѿ� ��.
		// 2014�� 3�� 28�� �� V4�����.
        // 2014�� 8�� 28�� �� V5�����.
		// 2015�� 3�� 11�� �� V6����� <- ������ ���DB��
        public static string m_WebServer_App = "AdManagerWebservice";
		
		// �α׷���
		public static int       m_LogLevel           = Logger._DEBUG;

		// Default ��ü�ڵ�
		public const int  _HANATV   = 1;

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

        /// <summary>
        /// �̵���, ����� BTV�� &TV�� ���Ǹ�, ���Ϲ����κ��� �����ϴ�
        /// </summary>
        public static string m_MediaName = "BTV";

        /// <summary>
        /// &TV�� FTP����
        /// </summary>
        public static int m_FtpCnt    = 1;
        public static string[] m_FtpIp;
        public static int m_FtpPort;
        public static string m_FtpUser;
        public static string m_FtpPass;
        public static string m_FtpPath;

		public FrameSystem(){}

		/// <summary>
		/// �ý����� �⵿�Ѵ�.
		/// </summary>
		public static int Start()
		{
			// Logger�� �⵿
			try
			{
				oLog = new Logger("Log", "AdTargetsPlus_" + m_SystemType, Logger._DEBUG);
				oLog.Start();

				oLog.Message("");
				oLog.Message("");
				oLog.Message("############################################################");
				oLog.Message("#                                                          #");
				oLog.Message("#                    AdTargets Manager v2.0                #");
				oLog.Message("#                Copyright(c) DARTmedia Inc.               #");
				oLog.Message("#                                                          #");
				oLog.Message("############################################################");
			}
			catch(Exception)
			{
				return 1001;		// �α׽��� ����
			}

			
			// �ý��� ���� ���� �� ȯ�溯�� �б�
			try
			{
				oLog.Message("ȯ�������� �н��ϴ�.");

				oSysModel = new SystemModel(m_SystemType);

                // 2011.10.25 RH.Jung IP��� ����
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipv4 = null;

                //�ý��ۿ��� ����ִ� ù��° IP�� �����Ѵ�. 
                foreach (IPAddress ipAddr in host.AddressList)
                {
                    if (ipAddr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4 = ipAddr;
                        break;
                    }
                }

                oSysModel.SysIP =  ipv4.ToString();

	            // ���� ������ ���� 2011.10.25
                // GetHostByName ������
				// oSysModel.SysIP   = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
                
                // 2011
				// �ý��� ����
                m_SystemVersion = ConfigurationManager.AppSettings.Get("System.Version");
                //m_SystemVersion = ConfigurationSettings.AppSettings.Get("System.Version");
                oLog.Message("SYSTEM  VERSION:" + m_SystemVersion);
				oSysModel.SysVersion = m_SystemVersion;

				// �ý��� ȣ�� Ÿ�Ӿƿ� (�и���)
                string to = ConfigurationManager.AppSettings.Get("System.Timeout");
				if(to != null && !to.Equals(""))
				{
					m_SystemTimeout =  Convert.ToInt32(to);
				}
				
				oLog.Message("SYSTEM TIMEOUT:"+m_SystemTimeout.ToString());


				// Ŭ���̾�Ʈ Ÿ��

                string clientType = ConfigurationManager.AppSettings.Get("Client.Type");
				
				oLog.Message("������ ����");
				oLog.Message("CLIENT TYPE:"+clientType);

				// ������ ���� ����
				switch(clientType)
				{
					case "TEST" : m_ClientType = _TEST;
                        m_WebServer_Host = ConfigurationManager.AppSettings.Get("WebServer.Test.Host");
                        m_WebServer_Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("WebServer.Test.Port"));
						break;
					case "REAL" : m_ClientType = _REAL;
                        m_WebServer_Host = ConfigurationManager.AppSettings.Get("WebServer.Real.Host");
                        m_WebServer_Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("WebServer.Real.Port"));
						break;
					default     : m_ClientType = _DEV;
                        m_WebServer_Host = ConfigurationManager.AppSettings.Get("WebServer.Dev.Host");
                        m_WebServer_Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("WebServer.Dev.Port"));
						break;
				}

				oLog.Message("������ ����");
				oLog.Message("HOST:"+m_WebServer_Host);
				oLog.Message("PORT:"+m_WebServer_Port.ToString());
		
				// �α׷���
                string sLogLevel = ConfigurationManager.AppSettings.Get("Log.Level");
				oLog.Message("�α� ����");
				oLog.Message("LOG LEVEL:" + sLogLevel);

				switch(sLogLevel)
				{
					case "MESSAGE" : m_LogLevel = Logger._MESSAGE;  break;
					case "WARNING" : m_LogLevel = Logger._WARNING;  break;
					case "ERROR"   : m_LogLevel = Logger._ERROR ;   break;
					default        : m_LogLevel = Logger._DEBUG ;   break;
				}

				// ������ �α׷����� �α��� ��⵿
				oLog.Stop();
				oLog.Start(m_LogLevel);

				// DBó�� �ڵ� ���� Reading �����ڵ�[E_01]-�ֻ�� �ּ�����
                DBSuccess = ConfigurationManager.AppSettings.Get("DBSuccess");
                DBListFail = ConfigurationManager.AppSettings.Get("DBListFail");
                DBAddFail = ConfigurationManager.AppSettings.Get("DBAddFail");
                DBEditFail = ConfigurationManager.AppSettings.Get("DBEditFail");
                DBDelFail = ConfigurationManager.AppSettings.Get("DBDelFail");

                try
                {
                    m_MediaName = ConfigurationManager.AppSettings.Get("MediaMode");
                    //<add key="Ftp" value="114.31.38.107^114.31.38.108^114.31.38.109;21;dartmedia;~!dartmedia@#;/"/>

                    if (m_MediaName.Equals("NTV"))
                    {
                        try
                        {
                            string ftpTemp  = ConfigurationManager.AppSettings.Get("Ftp");
                            string[] split = ftpTemp.Split(';');
                            if (split.Length == 5)
                            {
                                string[] splitIp = split[0].Split('^');
                                m_FtpCnt = splitIp.Length;
                                m_FtpIp = splitIp;
                                m_FtpPort = Convert.ToInt32(split[1].ToString());
                                m_FtpUser = split[2].ToString();
                                m_FtpPass = split[3].ToString();
                                m_FtpPath = split[4].ToString();
                            }

                        }
                        catch (Exception)
                        {
                            m_FtpCnt = 0;
                        }
                    }
                }
                catch (Exception)
                {
                    m_MediaName = "BTV";
                }

				oLog.Message("ȯ�漳���� �Ϸ��Ͽ����ϴ�.");
				oLog.Message("");

			}
			catch(Exception)
			{
				return 1003;
			}


			return 0;
		}

		/// <summary>
		/// ����� ���� �޽��� �ڽ�..
		/// </summary>
		/// <param name="title">�޽��� Ÿ��Ʋ</param>
		/// <param name="msg">�޽��� stirng �迭</param>
		public static void showMsgForm(string title, string []msg)
		{			
			MessageForm mform = new MessageForm();					
			
			try
			{
				mform.SetTitle = title;
				mform.SetMessage = msg;
				mform.showMessage();			
				mform.ShowDialog();
			}
			catch (Exception ex)
			{
				oLog.Message(title + ":"+ex.Message);
			}
			finally{}					

		}


        /// <summary>
        /// ���ڿ��� ��¥������ ��ȯ�Ѵ�
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ConverStrTotDate( string str)
        {
            int year    = DateTime.Now.Year;
            int month   = DateTime.Now.Month;
            int day     = DateTime.Now.Day;
            try
            {
                year    = Convert.ToInt32(str.Substring(0,4));
                month   = Convert.ToInt32(str.Substring(5,2));
                day     = Convert.ToInt32(str.Substring(8,2));
            }
            catch(Exception ex)
            {
                oLog.Message("��¥�� ��ȯ ����:" + str + ":" + ex.Message);
            }
            return new DateTime( year, month, day);
        }

        /// <summary>
        /// �����񽺷� ���� ����Ǿ� ���ϵ� �����͸� �����Ͽ� DataSet���� ��ȯ�Ͽ� ������
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static DataSet DeCompressData(byte[] src)
        {
            DataSet ds = new DataSet();
            MemoryStream inMs = new MemoryStream(src);
            inMs.Seek(0, 0);

            //
            System.IO.Compression.DeflateStream zipStream = new DeflateStream(inMs, CompressionMode.Decompress, true);
            byte[] outByte = ReadFullStream(zipStream);

            zipStream.Flush();
            zipStream.Close();

            MemoryStream outMs = new MemoryStream(outByte);
            outMs.Seek(0, 0);
            ds.RemotingFormat = SerializationFormat.Binary;
            BinaryFormatter bf = new BinaryFormatter();

            return (DataSet)bf.Deserialize(outMs, null);
        }

        public static byte[] ReadFullStream(Stream data)
        {
            byte[] buffer = new byte[2048];

            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = data.Read(buffer, 0, buffer.Length);

                    if (read <= 0)
                        return ms.ToArray();

                    ms.Write(buffer, 0, read);
                }
            }
        }
	}
}
