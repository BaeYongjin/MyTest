/*
 * -------------------------------------------------------
 * Class Name: FrameSystem
 * 주요기능  : 웹서비스 초기 구성
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
 *            - IP로 분기해서 서버 타입과 DB 연결IP정보를
 *              처리했지만 Web.Config 에 <SeverType>추가해서
 *              서버타입 및 DB연결 정보를 처리하도록 함.
 *            - DB처리 Result 코드 정의-하드코딩된 부분을
 *              Web.Config 에 정의를해서 처리 하도록 함.
 * --------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : 김보배
 * 수정일    : 2013.06.04
 * 수정내용  : 
 *            - 삭제했었던 CompressData() 함수 복원
 * -------------------------------------------------------- */

using System;
using System.Net;
using System.Text;
using System.Data;
using System.Threading;
using System.Configuration;

// 데이터 압축을 위해서 추가함(2013.04.30:장용석)
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
	/// 시스템의 기본설정 클래스
	/// </summary>
	public class FrameSystem
	{
		#region 변수선언
		// 시스템 타입
		public const string  m_SystemType = "NSADV";	// AdTargets Service Manager Server
		public static string m_SystemVersion = "Ver 1.0";
		
		// 시스템 정보 모델
		public static SystemModel	oSysModel	= null;

		/// 로그 오브젝트
		public static Logger oLog         = null;
		public static int    m_LogLevel   = Logger._DEBUG;

		// DB Connecting String
		public static string connDbString  = "";
        public static string connSummaryDbString = "";
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
		#endregion

		private FrameSystem(){}
				
		/// <summary> 
		/// 시스템을 기동한다.
		/// </summary>
		public static int Start()
		{
			// Logger의 기동
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

			// 시스템 버전
            m_SystemVersion = ConfigurationManager.AppSettings.Get("SystemVersion");
			oLog.Message("SYSTEM VERSION:"+m_SystemVersion);					
			
			#region 서버 타입 처리 및 db연결 문자열 구성 수정코드 E_01참고
            string svrType = ConfigurationManager.AppSettings.Get("SeverType");
						
			if (svrType.ToUpper().Equals("DEV"))
			{	 
				// 복호화를 통해서 저장
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

			// 로그레벨
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

			// 설정된 로그레벨로 로그의 재기동
			oLog.Stop();
			oLog.Start(m_LogLevel);

			// 시스템모델 
			oSysModel = new SystemModel(m_SystemType);
			oSysModel.SysIP       = "";
			oSysModel.SysVersion  = m_SystemVersion;
			
			// DB처리 코드 정보 Reading 수정코드 E_01
            DBSuccess = ConfigurationManager.AppSettings.Get("DBSuccess");
            DBListFail = ConfigurationManager.AppSettings.Get("DBListFail");
            DBAddFail = ConfigurationManager.AppSettings.Get("DBAddFail");
            DBEditFail = ConfigurationManager.AppSettings.Get("DBEditFail");
            DBDelFail = ConfigurationManager.AppSettings.Get("DBDelFail");
			
			oLog.Message("웹서비스가 시작되었습니다.");
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
            // 1. DataSet을 바이너리 포멧으로 전환한 뒤, 배열로 전환
            ds.RemotingFormat = SerializationFormat.Binary;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, ds);
            byte[] inbyte = ms.ToArray();

            // 2. 메모리 스트림을 압축하고 배열로 리턴
            MemoryStream objStream = new MemoryStream();
            System.IO.Compression.DeflateStream objZS = new DeflateStream(objStream, CompressionMode.Compress);

            objZS.Write(inbyte, 0, inbyte.Length);
            objZS.Flush();
            objZS.Close();

            return objStream.ToArray();
        }
	}
}