using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Configuration;

namespace AdManagerWebService.Common
{
	/// <summary>
	/// 파일 업/다운로드시 진행 상태 표시용
	/// </summary>
	public delegate void PositionDelegate(int inx);
	
	/// <summary>
	/// Ftp 파일 업/다운로드, 서버 파일명 변경
	/// </summary>
	public class FtpManager
	{	
		private int	pos		=	0;
		
		private int maxPos	=	0;

		public event PositionDelegate OnPosition;
		protected virtual void DrawPosition()
		{
			OnPosition(this.pos);
		}

		
		/// <summary>
		/// 파일의 최대 용량 값 얻기
		/// </summary>
		public event PositionDelegate OnMaxPosition;
		protected virtual void DrawMaxPosition()
		{
			OnMaxPosition(this.maxPos);
		}



		private static	int	BUFFER_SIZE = 1024 * 8;
		private static  Encoding ASCII = Encoding.ASCII;	// 한글이 깨짐

		private	int		serverPort;
		
		private string	serverIp;

		private	Socket	ftpSocket;
		
		private	string	userId		=	string.Empty;
		private string	userPwd		=	string.Empty;
		
		/// <summary>
		/// 서버 접속 상태 저장 변수
		/// </summary>
		private	bool	connected	=	false;
				
		private int		resultCode	=	0;
		
		/// <summary>
		/// Ftp Sever Type
		/// </summary>
		private int		severOS		=	0;
		
		private string	message		=	string.Empty;
		
		private	string	result		=	string.Empty;
		
		private string	remotePath	=	string.Empty;
		
		private bool	binMode		=	false;
		private int		bytes		=	0;
		private Byte[]	buffer		=	new Byte[BUFFER_SIZE];
		private Byte[]	recBuffer	=	new Byte[BUFFER_SIZE * 2];
		private Byte[]  recXmlBuffer = new Byte[1024];
		#region 속성들
		//---------------------------

		public int SetPort
		{
			set
			{
				this.serverPort = value;
			}
		}
		public string SetIpAddress
		{
			set
			{
				this.serverIp = value;
			}
		}

		public bool IsConnected
		{
			get
			{
				return this.connected;
			}
		}

		public string SetUserId
		{
			set
			{
				this.userId = value;
			}
		}
		public string SetUserPwd
		{
			set
			{
				this.userPwd = value;
			}
		}

		public bool BinaryMode
		{
			get
			{
				return this.binMode;
			}
			set
			{
				if ( this.binMode == value ) return;

				if ( value )
					sendCommand("TYPE I");

				else
					sendCommand("TYPE A");

				if ( this.resultCode != 200 ) throw new Exception(result.Substring(4));
			}
		}


		//--------------------------
		#endregion


		public FtpManager()
		{
			
		}
				
		
		~FtpManager()
		{
			this.cleanUp();
		}


		public void Close()
		{			
			if	(this.ftpSocket != null)
				this.sendCommand("QUIT");
			
			this.cleanUp();
		}


		private void cleanUp()
		{
			if (this.ftpSocket != null)
			{
				this.ftpSocket.Close();
				this.ftpSocket = null;
			}
			this.connected = false;
		}

		/// <summary>
		/// 서버 접속
		/// </summary>
		public bool Connect()
		{
			ftpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			
            // 2011.10.25 RH.Jung Dsn.Resolve 사용안함
            //IPEndPoint ep = new IPEndPoint(Dns.Resolve(this.serverIp).AddressList[0], this.serverPort);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(this.serverIp), this.serverPort);
			
			try
			{
				ftpSocket.Connect(ep);
				connected = true;
			}
			catch (IOException)
			{
				connected = false;
				return connected;
			}

			this.readResponse();

			if (this.resultCode != 220)
			{
				this.Close();
				
				connected = false;
				
				return connected;
			}
		
			// codes: 230 530 500 501 421 331 332
			this.sendCommand( "USER " + userId );

			if( !(this.resultCode == 331 || this.resultCode == 230) )
			{
				this.cleanUp();
				
				//throw new Exception(this.result.Substring(4));
				
				connected = false;
				
				return connected;
			}	

			if (this.resultCode != 230 )
			{				
				this.sendCommand( "PASS " + userPwd );

				if( !(this.resultCode == 230 || this.resultCode == 202) )
				{
					this.cleanUp();
					
					//throw new Exception(this.result.Substring(4));
					
					connected = false;
					
					return connected;
				}
			}

			connected = true;

			return connected;
		}
		
		
		/// <summary>
		/// 서버 접속
		/// </summary>
		public bool ConnectFtp()
		{
			ftpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			
            // 2011.10.25 RH.Jung Dsn.Resolve 사용안함
            //IPEndPoint ep = new IPEndPoint(Dns.Resolve(this.serverIp).AddressList[0], this.serverPort);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(this.serverIp), this.serverPort);
			
			try
			{
				ftpSocket.Connect(ep);
				connected = true;
			}
			catch (IOException)
			{
				connected = false;
				return connected;
			}

			this.readResponse();

			if (this.resultCode != 220)
			{
				this.Close();
				
				connected = false;
				
				return connected;
			}
		
			// codes: 230 530 500 501 421 331 332
			this.sendCommand( "USER " + userId );

			if( !(this.resultCode == 331 || this.resultCode == 230) )
			{
				this.cleanUp();
				
				//throw new Exception(this.result.Substring(4));
				
				connected = false;
				
				return connected;
			}	

			if (this.resultCode != 230 )
			{				
				this.sendCommand( "PASS " + userPwd );

				if( !(this.resultCode == 230 || this.resultCode == 202) )
				{
					this.cleanUp();
					
					//throw new Exception(this.result.Substring(4));
					
					connected = false;
					
					return connected;
				}
			}

			connected = true;
			
			// Ftp 서버 OS 알아내기
			// codes: 215 500 501 502 421
			this.sendCommand( "SYST\r\n" );
			
			if (!(this.resultCode == 215) )
			{
				throw new Exception(this.result.Substring(4));
			}

			string temp = this.result.Remove(0,4).Substring(0,4);
			
			if ( temp == "UNIX" ) 
			{
				severOS = 1;
			}
			else if ( temp == "Wind" ) 
			{
				severOS = 2;
			}
			else 
			{
				severOS = 3;
	
				this.cleanUp();
				
				throw new Exception("알수 없는 서버입니다.");			
			}

			Debug.WriteLine("severOS:"+severOS.ToString());


			this.ChangeDir(this.remotePath);

			return connected;
		}
		

		private void sendCommand(string cmd)
		{		
		//---------------
		// 명령어 전송
		//---------------
			
			if (!IsConnected) return;
			
			int	recLen = 0;
			
			try
			{
				Byte[] cmdBytes	=	Encoding.Default.GetBytes( (cmd.Trim() + "\r\n").ToCharArray() );
				
				recLen = ftpSocket.Send( cmdBytes, cmdBytes.Length, 0);
				
				// 서버 메시지 수신 처리
				this.readResponse();
			}
			catch(Exception)
			{
			}
		}

		
		/// <summary>
		/// 파일명 변경
		/// </summary>
		public void RenameFile(string oldFileName,string newFileName, bool overwrite)
		{
			try
			{
				if ( !this.connected ) this.ConnectFtp();
			
				// codes: 450 550 500 501 502 421 530 350</remarks>
				this.sendCommand( "RNFR " + oldFileName );

				if (this.resultCode != 350) 
					throw new Exception(this.result.Substring(4));

				// 같은 이름이 있는지 체크-덮어쓰기 금지-overwrite -false
				if (!overwrite)
				{
					this.sendCommand("SIZE " + newFileName);
					
					if (this.resultCode == 213)
						throw new Exception("같은 이름이 존재 합니다.");
				}
			
				//codes: 250 532 553 500 501 502 503 421 530</remarks>
				this.sendCommand( "RNTO " + newFileName );
				
				if (this.resultCode != 250) 
				{
					throw new Exception(this.result.Substring(4));
				}
								
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			
		}
		
		
		/// <summary>
		/// 파일 삭제
		/// </summary>
		public bool DeleteFile(string fileName)
		{
			bool flag = false;
			
			try
			{
				if (!this.connected ) 
					this.ConnectFtp();

				//파일 삭제.codes: 250 450 550 500 501 502 421 530</remarks>
				this.sendCommand( "DELE " + fileName );

				if (this.resultCode != 250) 
					throw new Exception(this.result.Substring(4));
			
				flag = true;
			}
			catch(Exception)
			{
				flag = false;
			}

			return flag;
		}


		/// <summary>
		/// 파일 목록 
		/// </summary>
		public string[] GetFileList()
		{
			return this.GetFileList("*.wmv");
		}
		
		
		/// <summary>
		/// 특정 file type 목록
		/// </summary>
		public string[] GetFileList(string mask)
		{
			if (!this.connected ) 
				this.ConnectFtp();
			
			// data Socket
			Socket cSocket = createDataSocket();

			//codes: 125 150 226 250 425 426 451 450 500 501 502 421 530
			//this.sendCommand("LIST " + mask);
			this.sendCommand("NLST " + mask);

			if (!(this.resultCode == 150 || this.resultCode == 125 || this.resultCode == 250 || this.resultCode == 226)) 
				throw new Exception(this.result.Substring(4));

			this.message = "";

			int recLen			= 0;
			string strOutput	= "";
			string strTmp		= "";
			
			recBuffer.Initialize();
			strTmp		= "";
			strOutput	= "";

			Thread.Sleep(700);

			for ( ; ( recLen = cSocket.Receive(recBuffer)) > 0 ;  ) 
			{
				strTmp = Encoding.Default.GetString(recBuffer,0, recLen);
				
				strOutput += strTmp;
				
				if ( cSocket.Available == 0 )
					break;				
			}

			this.message = strOutput;
			
			Regex re = new Regex(@"\r\n");
			
			string[] msg = re.Split(this.message);
			//string[] msg = this.message.Replace("\r","").Split('\n');

			cSocket.Close();

			if (this.message.IndexOf( "No such file or directory" ) != -1)
				msg = new string[]{};

			return msg;
		}
		
		

		
		/// <summary>
		/// 파일 사이즈
		/// </summary>
		public long GetFileSize(string fileName)
		{
			if (!this.connected) 
				this.ConnectFtp();

			this.sendCommand("SIZE " + fileName);
			
			long size=0;

			if (this.resultCode == 213)
				size = long.Parse(this.result.Substring(4));
			else
				throw new Exception(this.result.Substring(4));

			return size;
		}


		private Socket createDataSocket()
		{
			// DATA 송/수신의 경우는 별도의 Data Port 필요함.
			// PASV 를 전송하면 는 서버가 지정해준는 것으로 Data port 가 결정됨.
			// 21은 단시 메시지 송수신용임.
			// codes: 227 500 501 502 421 530</remarks>
			this.sendCommand("PASV");

			if (this.resultCode != 227 ) 
				throw new Exception(this.result.Substring(4));

			int index1 = this.result.IndexOf('(');
			int index2 = this.result.IndexOf(')');

			string ipData = this.result.Substring( index1+1 ,index2 - index1 - 1 );

			int[] parts = new int[6];

			int len = ipData.Length;
			int partCount = 0;
			string buf="";

			for (int i = 0; i < len && partCount <= 6; i++)
			{
				char ch = char.Parse( ipData.Substring(i,1) );

				if (char.IsDigit(ch))
					buf+=ch;

				else if (ch != ',')
					throw new Exception(result);

				if (ch == ',' || i+1 == len)
				{
					try
					{
						parts[partCount++] = int.Parse(buf);
						
						buf = "";
					}
					catch (Exception ex)
					{
						throw new Exception(this.result, ex);
					}
				}
			}

			string ipAddress = parts[0] + "."+ parts[1]+ "." + parts[2] + "." + parts[3];

			int port = (parts[4] << 8) + parts[5];

			Socket socket = null;
			
			IPEndPoint ep = null;

			try
			{
				socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                // 2011.10.25 RH.Jung Dsn.Resolve 사용안함
                //ep = new IPEndPoint(Dns.Resolve(ipAddress).AddressList[0], port);
                ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);

				socket.Connect(ep);
			}
			catch (Exception ex)
			{				
				if (socket != null && socket.Connected) 
					socket.Close();

				throw new Exception("Data Port 생성 실패", ex);
			}

			return socket;
		}
		
		

	

		/// <summary>
		/// File 업로드
		/// </summary>
		public void Upload(string fileName)
		{
			this.Upload(fileName,false);
		}
				
		public void Upload(string fileName, bool resume)
		{
			if (!this.connected ) 
				this.ConnectFtp();

			Socket cSocket = null;
			long offset = 0;
			resume = true;	//같은 파일명 업로드 불가-사이즈 비교함.
			
			if (resume)
			{
				try
				{
					this.BinaryMode = true;	// File Mode
					
					//서버상에 있는 파일 사이즈
					offset = GetFileSize( Path.GetFileName(fileName) );
				}
				catch(Exception)
				{
					offset = 0;		//같은 이름의 파일이 존재 하지 않음
				}
			}
			
			// file stream Read
			FileStream input = new FileStream(fileName, FileMode.Open, System.IO.FileAccess.Read );

			if ( resume && input.Length < offset )
			{
				// different file size
				//MessageBox.Show(fileName + " 이 존재합니다. 다른 이름으로 업로드 하세요!");
				offset = 0;
				throw new Exception("해당파일이 이미 존재합니다. 다름 파일명으로 업로드하시기 바랍니다.");			
			}
			else if ( resume && input.Length == offset )
			{
				// file done
				input.Close();
				throw new Exception("해당파일이 이미 존재합니다. 다름 파일명으로 업로드하시기 바랍니다.");			
			}

			//data socket 
			cSocket = this.createDataSocket();

			if ( offset > 0 )
			{
				// codes: 500 501 502 421 530 350</remarks>
				this.sendCommand( "REST " + offset );
				if ( this.resultCode != 350 )
					offset = 0;
			}

			// codes: 125 150 110 226 250 425 426 451 551 552 532 450 452 553 500 501 421 530</remarks>
			this.sendCommand( "STOR " + Path.GetFileName(fileName) );

			if (this.resultCode != 125 && this.resultCode != 150)
			{				
				throw new Exception(result.Substring(4));			
			}

			if (offset != 0)
				input.Seek(offset	,SeekOrigin.Begin);
			
			long tmp = 0;

			// 업로드 시작
			while ((bytes = input.Read(buffer ,0 ,buffer.Length)) > 0)
			{	
				Application.DoEvents();

				//pos += bytes;
				
				tmp += bytes;
				pos = Convert.ToInt32(tmp * 0.001);

				DrawPosition();
				cSocket.Send(buffer, bytes, 0);				
			}
			
			pos = 0;

			input.Close();

			if (cSocket.Connected) cSocket.Close();

			this.readResponse();

			if( this.resultCode != 226 && this.resultCode != 250 )
			{
				throw new Exception(this.resultCode.ToString());			
			}
		}
		
		public void Upload(string LocalFileName, string RemoteFileName)
		{
			this.Upload(LocalFileName,RemoteFileName,false);
		}
				
		public void Upload(string LocalFileName, string RemoteFileName, bool resume)
		{
			if (!this.connected ) 
				this.ConnectFtp();

			Socket cSocket = null;
			long offset = 0;
			resume = true;	//같은 파일명 업로드 불가-사이즈 비교함.
			
			if (resume)
			{
				try
				{
					this.BinaryMode = true;	// File Mode
					
					//서버상에 있는 파일 사이즈
					offset = GetFileSize( Path.GetFileName(RemoteFileName) );
				}
				catch(Exception)
				{
					offset = 0;		//같은 이름의 파일이 존재 하지 않음
				}
			}
			
			// file stream Read
			FileStream input = new FileStream(LocalFileName, FileMode.Open, System.IO.FileAccess.Read );

			if ( resume && input.Length < offset )
			{
				// different file size
				//MessageBox.Show(fileName + " 이 존재합니다. 다른 이름으로 업로드 하세요!");
				offset = 0;
				throw new Exception("해당파일이 이미 존재합니다. 다름 파일명으로 업로드하시기 바랍니다.");			
			}
			else if ( resume && input.Length == offset )
			{
				// file done
				input.Close();
				throw new Exception("해당파일이 이미 존재합니다. 다름 파일명으로 업로드하시기 바랍니다.");			
			}

			//data socket 
			cSocket = this.createDataSocket();

			if ( offset > 0 )
			{
				// codes: 500 501 502 421 530 350</remarks>
				this.sendCommand( "REST " + offset );
				if ( this.resultCode != 350 )
					offset = 0;
			}

			// codes: 125 150 110 226 250 425 426 451 551 552 532 450 452 553 500 501 421 530</remarks>
			this.sendCommand( "STOR " + Path.GetFileName(RemoteFileName) );

			if (this.resultCode != 125 && this.resultCode != 150)
			{				
				throw new Exception(result.Substring(4));			
			}

			if (offset != 0)
				input.Seek(offset	,SeekOrigin.Begin);
			
			long tmp = 0;

			// 업로드 시작
			while ((bytes = input.Read(buffer ,0 ,buffer.Length)) > 0)
			{	
				Application.DoEvents();

				//pos += bytes;
				
				tmp += bytes;
				pos = Convert.ToInt32(tmp * 0.001);

				DrawPosition();
				cSocket.Send(buffer, bytes, 0);				
			}
			
			pos = 0;

			input.Close();

			if (cSocket.Connected) cSocket.Close();

			this.readResponse();

			if( this.resultCode != 226 && this.resultCode != 250 )
			{
				throw new Exception(this.resultCode.ToString());			
			}
		}
		

		
		/// <summary>
		/// 디렉토리 업로드
		/// </summary>
		public void UploadDirectory(string path, bool recurse)
		{
			this.UploadDirectory(path,recurse,"*.*");
		}
		
		
		/// <summary>
		/// 디렉토리 업로드
		/// </summary>		
		public void UploadDirectory(string path, bool recurse, string mask)
		{
			string[] dirs = path.Replace("/",@"\").Split('\\');
			string rootDir = dirs[ dirs.Length - 1 ];
			
			try
			{				
				this.GetFileList(rootDir);
			}
			catch
			{
				this.MakeDir(rootDir);
			}

			this.ChangeDir(rootDir);

			foreach ( string file in Directory.GetFiles(path,mask) )
			{
				this.Upload(file ,true);
			}
			if ( recurse )
			{
				foreach ( string directory in Directory.GetDirectories(path) )
				{
					this.UploadDirectory(directory,recurse,mask);
				}
			}

			this.ChangeDir("..");
		}

		

		/// <summary>
		/// 파일 다운로드 - 이어받기 가능 하도록..
		/// </summary>		
		public void Download(string remFileName ,string locFileName ,bool resume)
		{
		
			try
			{				
				if (!this.connected ) 
					this.ConnectFtp();
				
				//File Type
				this.BinaryMode = true;			

				//서버 파일명으로 생성
				if (locFileName.Equals(""))
					locFileName = remFileName;
			
				FileStream output		= null;
				FileStream fileOutput	= null;
				
				long offset = 0;
	
				if (!File.Exists(locFileName))	
				{
					//output = File.Create(locFileName);
					offset = 0;
				}
				else
				{
					output = new FileStream(locFileName, FileMode.Open);
					offset = output.Length;
					output.Close();
				}
			
				Socket cSocket = createDataSocket();

				if ( resume )
				{
					if ( offset > 0 )	//파일 이어받기
					{
						// codes: 500 501 502 421 530 350
						this.sendCommand( "REST " + offset );
						if (this.resultCode != 350)
							offset = 0;	//resume 미지원	
						else
						{	
							// 파일을 FileMode.Open 모드로 열어야 함.
							fileOutput = new FileStream(locFileName, FileMode.Open);
							// fileOutput.Seek( offset, SeekOrigin.Begin );
						}
					}
					else
					{	// 파일 새롭게 생성
						// fileOutput = new FileStream(locFileName ,FileMode.Create);
						fileOutput = new FileStream(locFileName ,FileMode.OpenOrCreate, FileAccess.Write);
						fileOutput.Seek(0, SeekOrigin.Begin);
					}
				}
				else
				{
					fileOutput = new FileStream(locFileName ,FileMode.OpenOrCreate, FileAccess.Write);
					fileOutput.Seek(0, SeekOrigin.Begin);
				}

				// Ftp 서버상에 있는 파일 사이즈
				long fSize =  this.GetFileSize(remFileName);
				//maxPos = (int)fSize;
				maxPos = Convert.ToInt32(fSize*0.001);
				
				// 메인 폼에 최대 값 전달
				this.DrawMaxPosition(); 
			
				//codes: 125 150 110 226 250 425 426 451 450 550 500 501 421 530
				this.sendCommand("RETR " + remFileName);
						
				//codes: 125,150,110,250,226
				if ( this.resultCode != 150 && this.resultCode != 125 && this.resultCode != 110 && this.resultCode != 250 && this.resultCode != 226 )
				{
					throw new Exception(this.resultCode.ToString());			
				}
				
				pos = (int)offset; // 파일 offset 위치
				
				long lngTmp = 0;
				lngTmp = pos;
					
				while (true)
				{
					this.bytes = cSocket.Receive(buffer, buffer.Length, 0);
					
					//Progress Draw
					//pos += bytes;		
					
					lngTmp += bytes;			
					pos = Convert.ToInt32(lngTmp * 0.001);
					this.DrawPosition();	
					
					//파일 쓰기
					fileOutput.Write(this.buffer ,0 ,this.bytes);
				
					if ( this.bytes <= 0)
						break;					
				}
			
				fileOutput.Close();
			
				pos = maxPos = 0;

				if ( cSocket.Connected ) cSocket.Close();

				this.readResponse();

				if( this.resultCode != 226 && this.resultCode != 250 )
				{
					throw new Exception(this.resultCode.ToString());			
				}
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);			
			}
		}

		
		/// <summary>
		/// 프로그램 업데이트 다운로드 용
		/// </summary>		
		public void UpdateDownLoad(string remFileName ,string locFileName ,bool resume, bool binary)
		{
			try
			{				
				if (!this.connected ) 
					this.ConnectFtp();
				
				//File Type
				this.BinaryMode = binary;			

				//서버 파일명으로 생성
				if (locFileName.Equals(""))
					locFileName = remFileName;
			
				FileStream output		= null;
				FileStream fileOutput	= null;
				
				long offset = 0;
	
				if (!File.Exists(locFileName))	
				{
					//output = File.Create(locFileName);
					offset = 0;
				}
				else
				{
					output = new FileStream(locFileName, FileMode.Open);
					offset = output.Length;
					output.Close();
				}
			
				Socket cSocket = createDataSocket();

				if ( resume )
				{
					if ( offset > 0 )	//파일 이어받기
					{
						// codes: 500 501 502 421 530 350
						this.sendCommand( "REST " + offset );
						if (this.resultCode != 350)
							offset = 0;	//resume 미지원	
						else
						{	
							// 파일을 FileMode.Open 모드로 열어야 함.
							fileOutput = new FileStream(locFileName, FileMode.Open);
							// fileOutput.Seek( offset, SeekOrigin.Begin );
						}
					}
					else
					{	// 파일 새롭게 생성
						// fileOutput = new FileStream(locFileName ,FileMode.Create);
						fileOutput = new FileStream(locFileName ,FileMode.OpenOrCreate, FileAccess.Write);
						fileOutput.Seek(0, SeekOrigin.Begin);
					}
				}

				// Ftp 서버상에 있는 파일 사이즈
				long fSize =  this.GetFileSize(Path.GetFileName(remFileName));
						
				//codes: 125 150 110 226 250 425 426 451 450 550 500 501 421 530
				this.sendCommand("RETR " + remFileName);
						
				//codes: 125,150,110,250,226
				if ( this.resultCode != 150 && this.resultCode != 125 && this.resultCode != 110 && this.resultCode != 250 && this.resultCode != 226 )
				{
					throw new Exception(this.resultCode.ToString());			
				}
				
				pos = (int)offset; // 파일 offset 위치
				while (true)
				{
					this.bytes = cSocket.Receive(buffer, buffer.Length, 0);
					
					//파일 쓰기
					fileOutput.Write(this.buffer ,0 ,this.bytes);
				
					if ( this.bytes <= 0)
						break;					
				}
			
				fileOutput.Close();

				if ( cSocket.Connected ) cSocket.Close();

				this.readResponse();

				if( this.resultCode != 226 && this.resultCode != 250 )
				{
					throw new Exception(this.resultCode.ToString());			
				}
			}
			catch(Exception ex)
			{
				Trace.Write("다운 에러:"+ex.Message);
				throw new Exception(ex.Message);			
			}
		}



		/// <summary>
		/// 프로그램 업데이트 다운로드 용
		/// </summary>		
		public void UpdateDownXML(string remFileName ,string locFileName ,bool resume, bool binary)
		{
			try
			{				
				if (!this.connected ) 
					this.ConnectFtp();
				
				//File Type
				this.BinaryMode = binary;			

				//서버 파일명으로 생성
				if (locFileName.Equals(""))
					locFileName = remFileName;
			
				FileStream output		= null;
				FileStream fileOutput	= null;
				
				long offset = 0;
	
				if (!File.Exists(locFileName))	
				{
					//output = File.Create(locFileName);
					offset = 0;
				}
				else
				{
					output = new FileStream(locFileName, FileMode.Open);
					offset = output.Length;
					output.Close();
				}
			
				Socket cSocket = createDataSocket();

				if ( resume )
				{
					if ( offset > 0 )	//파일 이어받기
					{
						// codes: 500 501 502 421 530 350
						this.sendCommand( "REST " + offset );
						if (this.resultCode != 350)
							offset = 0;	//resume 미지원	
						else
						{	
							// 파일을 FileMode.Open 모드로 열어야 함.
							fileOutput = new FileStream(locFileName, FileMode.Open);
							// fileOutput.Seek( offset, SeekOrigin.Begin );
						}
					}
					else
					{	// 파일 새롭게 생성
						// fileOutput = new FileStream(locFileName ,FileMode.Create);
						fileOutput = new FileStream(locFileName ,FileMode.OpenOrCreate, FileAccess.Write);
						fileOutput.Seek(0, SeekOrigin.Begin);
					}
				}

				// Ftp 서버상에 있는 파일 사이즈
				long fSize =  this.GetFileSize(Path.GetFileName(remFileName));
						
				//codes: 125 150 110 226 250 425 426 451 450 550 500 501 421 530
				this.sendCommand("RETR " + remFileName);
						
				//codes: 125,150,110,250,226
				if ( this.resultCode != 150 && this.resultCode != 125 && this.resultCode != 110 && this.resultCode != 250 && this.resultCode != 226 )
				{
					throw new Exception(this.resultCode.ToString());			
				}
				
				pos = (int)offset; // 파일 offset 위치
				while (true)
				{
					this.bytes = cSocket.Receive(buffer, buffer.Length, 0);
					
					//파일 쓰기
					fileOutput.Write(this.buffer ,0 ,this.bytes);
				
					if ( this.bytes <= 0)
						break;					
				}
			
				fileOutput.Close();

				if ( cSocket.Connected ) 
					cSocket.Close();

//				this.readResponse();
//
//				if( this.resultCode != 226 && this.resultCode != 250 )
//				{
//					//MessageBox.Show(this.result.Substring(4));
//					flag = false;
//				}
			}
			catch(Exception ex)
			{
				Trace.Write("다운 에러:"+ex.Message);
				throw new Exception(ex.Message);			
			}
		}


		#region 디렉토리 관련 명령어..
		//----------------------------------------

		/// <summary>
		/// 디렉토리 변경
		/// </summary>		
		public string ChangeDir(string dirName)
		{
			if( dirName == null || dirName.Equals(".") || dirName.Length == 0 )
			{
				return "";
			}

			if ( !this.connected ) this.ConnectFtp();

			
			// 디렉토리 변경.codes: 250 500 501 502 421 530 550
			this.sendCommand( "CWD " + dirName );
			
			if ( (this.resultCode == 500) || (this.resultCode == 501) || (this.resultCode == 502) || (this.resultCode == 421) || (this.resultCode == 530) || (this.resultCode == 550) )
				 throw new Exception(result.Substring(4));
			
			//현재 위치: codes: 257 500 501 502 421 550
			this.sendCommand( "PWD" );

			if ( (this.resultCode == 500) || (this.resultCode == 501) || (this.resultCode == 502) || (this.resultCode == 421) || (this.resultCode == 550) )
				throw new Exception(result.Substring(4));
			
			this.remotePath = this.message.Split('"')[1];			
			return this.remotePath;
		}

		

		/// <summary>
		/// 디렉토리 생성
		/// </summary>
		public void MakeDir(string dirName)
		{
			if ( !this.connected ) this.ConnectFtp();

			// 디렉토리 만들기.codes: 257 500 501 502 421 530 550</remarks>
			this.sendCommand( "MKD " + dirName );

			if ( this.resultCode != 250 && this.resultCode != 257 )
				throw new Exception(this.result.Substring(4));				

//			MessageBox.Show(dirName + " 만들기 완료");
		}



		/// <summary>
		/// 디렉토리 삭제
		/// </summary>
		public void RemoveDir(string dirName)
		{
			if ( !this.connected ) this.ConnectFtp();
		
			// 삭제 명령어. codes: 250 500 501 502 421 530 550
			this.sendCommand( "RMD " + dirName );

			if ( this.resultCode != 250 )  
				throw new Exception(this.result.Substring(4));
			
			
//			MessageBox.Show(dirName + " 삭제 되었습니다");

		}

		
		//----------------------------------------
		#endregion


		/// <summary>
		/// 수신 메시지 처리
		/// </summary>
		private void readResponse()
		{
			try
			{
				this.message = string.Empty;
				this.result  = this.readLine();
			
				if (this.result.Length > 3)
					this.resultCode = int.Parse(this.result.Substring(0,3) );
				else
					this.result = null;
			}
			catch(Exception ex)
			{
				Trace.Write(ex.Message);
			}
		}

		
		/// <summary>
		/// 서버 메시지 수신
		/// </summary>
 		private string readLine()
		{
			try
			{
				int		recLen		= 0;
				string	strOutput	= string.Empty;
				string	strTmp		= string.Empty;
			
				recBuffer.Initialize();
				strTmp		= "";
				strOutput	= "";

				Thread.Sleep(700);
			
				for ( ; (recLen = ftpSocket.Receive(recBuffer)) > 0 ; ) 
				{
					strTmp = Encoding.ASCII.GetString(recBuffer ,0 ,recLen);
					strOutput += strTmp;
					if ( ftpSocket.Available == 0 )
						break;				
				}				

				this.message = strOutput;

				string[] msg = this.message.Split('\n');

				if ( this.message.Length > 2 )
					this.message = msg[ msg.Length - 2 ];

				else
					this.message = msg[0];

				if ( this.message.Length > 4 && !this.message.Substring(3,1).Equals(" ") ) 
					return this.readLine();
			}
			catch(Exception ex)
			{
				Trace.Write(ex.Message);
			}
			return message;
		}


		
	}
}
