// ===============================================================================
// TMS Manager System Frame Helper for Charites Project
//
// FrameSystem.cs
//
// 시스템의 운영정보 및 오브젝트들을 정의 
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
 * 주요기능  : Client 초기 구성
 * 작성자    : bae 
 * 작성일    : 2010.06.07
 * 특이사항  : 
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : bae
 * 수정일    : 2010.06.07
 * 수정내용  :           
 *            - DB처리 Result 코드 정의-하드코딩된 부분을
 *              App.Config 에 정의를해서 처리 하도록 함.
 * --------------------------------------------------------
 * 수정코드  : 
 * 수정자    : RH.Jung
 * 수정일    : 2011.10.25
 * 수정내용  :           
 *            - 런타임을 .NET 4 로 변경
 *            - IP얻기 GetHostEntry로 변경
 *            - ConfigurationSettings를 ConfigurationManager로 변경
 * --------------------------------------------------------
 * 수정코드  : 
 * 수정자    : YS.Jang
 * 수정일    : 2012.01.03
 * 수정내용  :           
 *              - MediaMode app.config에 추가함.
 *              파일FTP업로드때문에( &TV와 BTV를 분리함)
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
	/// 시스템의 기본설정 클래스
	/// </summary>
	public class FrameSystem
	{
		
		// 시스템 타입
		public const  string m_SystemType = "ASMC";	// AdTargets Service Manager Client
		public static string m_SystemVersion = "Ver 1.0";
		public static int    m_SystemTimeout =  500;

		// 클라이언트 구분
		public const  int _DEV   = 0;
		public const  int _TEST  = 1;
		public const  int _REAL  = 2;
		public const  int _TEST_DEV = 3;
		public static int m_ClientType = _DEV;	

		// 시스템 클라이언트 키 : SystemType + IP
		public static string m_ClientKey = null;

		// 시스템 정보 모델
		public static SystemModel	oSysModel	= null;

		// 사용자 정보 모델
		public static CommonModel	 oComModel	= null;

		/// 로그 오브젝트
		public static Logger		oLog = null;

		/// Config 오브젝트
		public static ConfigReader	oCfg = null;

		// 메뉴 오브젝트
		public static MenuPower     oMenu = null;

		/// 서버정보
		public static string	m_WebServer_Host	= "";
		public static int		m_WebServer_Port	= 8086;
        // BMK 서비스 어플
		// HACK [상용배포시 주의사항] 광고웹서비스 어플리케이션 명칭과 일치 시켜야 함.
		// 2014년 3월 28일 자 V4사용중.
        // 2014년 8월 28일 자 V5사용중.
		// 2015년 3월 11일 자 V6사용중 <- 리포팅 백업DB쪽
        public static string m_WebServer_App = "AdManagerWebservice";
		
		// 로그레벨
		public static int       m_LogLevel           = Logger._DEBUG;

		// Default 매체코드
		public const int  _HANATV   = 1;

		/// <summary>
		/// DB처리 성공-0000
		/// </summary>
		public static string DBSuccess  = "";
		/// <summary>
		/// DB리스트 조회실패-3000
		/// </summary>
		public static string DBListFail = "";
		/// <summary>
		/// DB추가 실패-3101
		/// </summary>
		public static string DBAddFail  = "";
		/// <summary>
		/// DB수정 실패-3201
		/// </summary>
		public static string DBEditFail = "";
		/// <summary>
		/// DB삭제 실패-3301
		/// </summary>
		public static string DBDelFail  = "";

        /// <summary>
        /// 미디어명, 현재는 BTV와 &TV가 사용되며, 파일배포부분이 상이하다
        /// </summary>
        public static string m_MediaName = "BTV";

        /// <summary>
        /// &TV용 FTP정보
        /// </summary>
        public static int m_FtpCnt    = 1;
        public static string[] m_FtpIp;
        public static int m_FtpPort;
        public static string m_FtpUser;
        public static string m_FtpPass;
        public static string m_FtpPath;

		public FrameSystem(){}

		/// <summary>
		/// 시스템을 기동한다.
		/// </summary>
		public static int Start()
		{
			// Logger의 기동
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
				return 1001;		// 로그시작 오류
			}

			
			// 시스템 모델의 생성 및 환경변수 읽기
			try
			{
				oLog.Message("환경파일을 읽습니다.");

				oSysModel = new SystemModel(m_SystemType);

                // 2011.10.25 RH.Jung IP얻기 수정
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipv4 = null;

                //시스템에서 살아있는 첫번째 IP로 설정한다. 
                foreach (IPAddress ipAddr in host.AddressList)
                {
                    if (ipAddr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4 = ipAddr;
                        break;
                    }
                }

                oSysModel.SysIP =  ipv4.ToString();

	            // 위의 것으로 변경 2011.10.25
                // GetHostByName 사용안함
				// oSysModel.SysIP   = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
                
                // 2011
				// 시스템 버전
                m_SystemVersion = ConfigurationManager.AppSettings.Get("System.Version");
                //m_SystemVersion = ConfigurationSettings.AppSettings.Get("System.Version");
                oLog.Message("SYSTEM  VERSION:" + m_SystemVersion);
				oSysModel.SysVersion = m_SystemVersion;

				// 시스템 호출 타임아웃 (밀리초)
                string to = ConfigurationManager.AppSettings.Get("System.Timeout");
				if(to != null && !to.Equals(""))
				{
					m_SystemTimeout =  Convert.ToInt32(to);
				}
				
				oLog.Message("SYSTEM TIMEOUT:"+m_SystemTimeout.ToString());


				// 클라이언트 타입

                string clientType = ConfigurationManager.AppSettings.Get("Client.Type");
				
				oLog.Message("웹서버 정보");
				oLog.Message("CLIENT TYPE:"+clientType);

				// 웹서버 정보 설정
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

				oLog.Message("웹서버 정보");
				oLog.Message("HOST:"+m_WebServer_Host);
				oLog.Message("PORT:"+m_WebServer_Port.ToString());
		
				// 로그레벨
                string sLogLevel = ConfigurationManager.AppSettings.Get("Log.Level");
				oLog.Message("로그 정보");
				oLog.Message("LOG LEVEL:" + sLogLevel);

				switch(sLogLevel)
				{
					case "MESSAGE" : m_LogLevel = Logger._MESSAGE;  break;
					case "WARNING" : m_LogLevel = Logger._WARNING;  break;
					case "ERROR"   : m_LogLevel = Logger._ERROR ;   break;
					default        : m_LogLevel = Logger._DEBUG ;   break;
				}

				// 설정된 로그레벨로 로그의 재기동
				oLog.Stop();
				oLog.Start(m_LogLevel);

				// DB처리 코드 정보 Reading 수정코드[E_01]-최상단 주석참고
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

				oLog.Message("환경설정을 완료하였습니다.");
				oLog.Message("");

			}
			catch(Exception)
			{
				return 1003;
			}


			return 0;
		}

		/// <summary>
		/// 사용자 정의 메시지 박스..
		/// </summary>
		/// <param name="title">메시지 타이틀</param>
		/// <param name="msg">메시지 stirng 배열</param>
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
        /// 문자열을 날짜형으로 변환한다
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
                oLog.Message("날짜형 변환 오류:" + str + ":" + ex.Message);
            }
            return new DateTime( year, month, day);
        }

        /// <summary>
        /// 웹서비스로 부터 압축되어 리턴된 데이터를 해제하여 DataSet으로 변환하여 돌려줌
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
