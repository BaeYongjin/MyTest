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
	/// ���� ��/�ٿ�ε�� ���� ���� ǥ�ÿ�
	/// </summary>
	public delegate void PositionDelegate(int inx);
	
	/// <summary>
	/// Ftp ���� ��/�ٿ�ε�, ���� ���ϸ� ����
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
		/// ������ �ִ� �뷮 �� ���
		/// </summary>
		public event PositionDelegate OnMaxPosition;
		protected virtual void DrawMaxPosition()
		{
			OnMaxPosition(this.maxPos);
		}



		private static	int	BUFFER_SIZE = 1024 * 8;
		private static  Encoding ASCII = Encoding.ASCII;	// �ѱ��� ����

		private	int		serverPort;
		
		private string	serverIp;

		private	Socket	ftpSocket;
		
		private	string	userId		=	string.Empty;
		private string	userPwd		=	string.Empty;
		
		/// <summary>
		/// ���� ���� ���� ���� ����
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
		#region �Ӽ���
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
		/// ���� ����
		/// </summary>
		public bool Connect()
		{
			ftpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			
            // 2011.10.25 RH.Jung Dsn.Resolve ������
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
		/// ���� ����
		/// </summary>
		public bool ConnectFtp()
		{
			ftpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			
            // 2011.10.25 RH.Jung Dsn.Resolve ������
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
			
			// Ftp ���� OS �˾Ƴ���
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
				
				throw new Exception("�˼� ���� �����Դϴ�.");			
			}

			Debug.WriteLine("severOS:"+severOS.ToString());


			this.ChangeDir(this.remotePath);

			return connected;
		}
		

		private void sendCommand(string cmd)
		{		
		//---------------
		// ��ɾ� ����
		//---------------
			
			if (!IsConnected) return;
			
			int	recLen = 0;
			
			try
			{
				Byte[] cmdBytes	=	Encoding.Default.GetBytes( (cmd.Trim() + "\r\n").ToCharArray() );
				
				recLen = ftpSocket.Send( cmdBytes, cmdBytes.Length, 0);
				
				// ���� �޽��� ���� ó��
				this.readResponse();
			}
			catch(Exception)
			{
			}
		}

		
		/// <summary>
		/// ���ϸ� ����
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

				// ���� �̸��� �ִ��� üũ-����� ����-overwrite -false
				if (!overwrite)
				{
					this.sendCommand("SIZE " + newFileName);
					
					if (this.resultCode == 213)
						throw new Exception("���� �̸��� ���� �մϴ�.");
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
		/// ���� ����
		/// </summary>
		public bool DeleteFile(string fileName)
		{
			bool flag = false;
			
			try
			{
				if (!this.connected ) 
					this.ConnectFtp();

				//���� ����.codes: 250 450 550 500 501 502 421 530</remarks>
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
		/// ���� ��� 
		/// </summary>
		public string[] GetFileList()
		{
			return this.GetFileList("*.wmv");
		}
		
		
		/// <summary>
		/// Ư�� file type ���
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
		/// ���� ������
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
			// DATA ��/������ ���� ������ Data Port �ʿ���.
			// PASV �� �����ϸ� �� ������ �������ش� ������ Data port �� ������.
			// 21�� �ܽ� �޽��� �ۼ��ſ���.
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
                // 2011.10.25 RH.Jung Dsn.Resolve ������
                //ep = new IPEndPoint(Dns.Resolve(ipAddress).AddressList[0], port);
                ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);

				socket.Connect(ep);
			}
			catch (Exception ex)
			{				
				if (socket != null && socket.Connected) 
					socket.Close();

				throw new Exception("Data Port ���� ����", ex);
			}

			return socket;
		}
		
		

	

		/// <summary>
		/// File ���ε�
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
			resume = true;	//���� ���ϸ� ���ε� �Ұ�-������ ����.
			
			if (resume)
			{
				try
				{
					this.BinaryMode = true;	// File Mode
					
					//������ �ִ� ���� ������
					offset = GetFileSize( Path.GetFileName(fileName) );
				}
				catch(Exception)
				{
					offset = 0;		//���� �̸��� ������ ���� ���� ����
				}
			}
			
			// file stream Read
			FileStream input = new FileStream(fileName, FileMode.Open, System.IO.FileAccess.Read );

			if ( resume && input.Length < offset )
			{
				// different file size
				//MessageBox.Show(fileName + " �� �����մϴ�. �ٸ� �̸����� ���ε� �ϼ���!");
				offset = 0;
				throw new Exception("�ش������� �̹� �����մϴ�. �ٸ� ���ϸ����� ���ε��Ͻñ� �ٶ��ϴ�.");			
			}
			else if ( resume && input.Length == offset )
			{
				// file done
				input.Close();
				throw new Exception("�ش������� �̹� �����մϴ�. �ٸ� ���ϸ����� ���ε��Ͻñ� �ٶ��ϴ�.");			
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

			// ���ε� ����
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
			resume = true;	//���� ���ϸ� ���ε� �Ұ�-������ ����.
			
			if (resume)
			{
				try
				{
					this.BinaryMode = true;	// File Mode
					
					//������ �ִ� ���� ������
					offset = GetFileSize( Path.GetFileName(RemoteFileName) );
				}
				catch(Exception)
				{
					offset = 0;		//���� �̸��� ������ ���� ���� ����
				}
			}
			
			// file stream Read
			FileStream input = new FileStream(LocalFileName, FileMode.Open, System.IO.FileAccess.Read );

			if ( resume && input.Length < offset )
			{
				// different file size
				//MessageBox.Show(fileName + " �� �����մϴ�. �ٸ� �̸����� ���ε� �ϼ���!");
				offset = 0;
				throw new Exception("�ش������� �̹� �����մϴ�. �ٸ� ���ϸ����� ���ε��Ͻñ� �ٶ��ϴ�.");			
			}
			else if ( resume && input.Length == offset )
			{
				// file done
				input.Close();
				throw new Exception("�ش������� �̹� �����մϴ�. �ٸ� ���ϸ����� ���ε��Ͻñ� �ٶ��ϴ�.");			
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

			// ���ε� ����
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
		/// ���丮 ���ε�
		/// </summary>
		public void UploadDirectory(string path, bool recurse)
		{
			this.UploadDirectory(path,recurse,"*.*");
		}
		
		
		/// <summary>
		/// ���丮 ���ε�
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
		/// ���� �ٿ�ε� - �̾�ޱ� ���� �ϵ���..
		/// </summary>		
		public void Download(string remFileName ,string locFileName ,bool resume)
		{
		
			try
			{				
				if (!this.connected ) 
					this.ConnectFtp();
				
				//File Type
				this.BinaryMode = true;			

				//���� ���ϸ����� ����
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
					if ( offset > 0 )	//���� �̾�ޱ�
					{
						// codes: 500 501 502 421 530 350
						this.sendCommand( "REST " + offset );
						if (this.resultCode != 350)
							offset = 0;	//resume ������	
						else
						{	
							// ������ FileMode.Open ���� ����� ��.
							fileOutput = new FileStream(locFileName, FileMode.Open);
							// fileOutput.Seek( offset, SeekOrigin.Begin );
						}
					}
					else
					{	// ���� ���Ӱ� ����
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

				// Ftp ������ �ִ� ���� ������
				long fSize =  this.GetFileSize(remFileName);
				//maxPos = (int)fSize;
				maxPos = Convert.ToInt32(fSize*0.001);
				
				// ���� ���� �ִ� �� ����
				this.DrawMaxPosition(); 
			
				//codes: 125 150 110 226 250 425 426 451 450 550 500 501 421 530
				this.sendCommand("RETR " + remFileName);
						
				//codes: 125,150,110,250,226
				if ( this.resultCode != 150 && this.resultCode != 125 && this.resultCode != 110 && this.resultCode != 250 && this.resultCode != 226 )
				{
					throw new Exception(this.resultCode.ToString());			
				}
				
				pos = (int)offset; // ���� offset ��ġ
				
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
					
					//���� ����
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
		/// ���α׷� ������Ʈ �ٿ�ε� ��
		/// </summary>		
		public void UpdateDownLoad(string remFileName ,string locFileName ,bool resume, bool binary)
		{
			try
			{				
				if (!this.connected ) 
					this.ConnectFtp();
				
				//File Type
				this.BinaryMode = binary;			

				//���� ���ϸ����� ����
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
					if ( offset > 0 )	//���� �̾�ޱ�
					{
						// codes: 500 501 502 421 530 350
						this.sendCommand( "REST " + offset );
						if (this.resultCode != 350)
							offset = 0;	//resume ������	
						else
						{	
							// ������ FileMode.Open ���� ����� ��.
							fileOutput = new FileStream(locFileName, FileMode.Open);
							// fileOutput.Seek( offset, SeekOrigin.Begin );
						}
					}
					else
					{	// ���� ���Ӱ� ����
						// fileOutput = new FileStream(locFileName ,FileMode.Create);
						fileOutput = new FileStream(locFileName ,FileMode.OpenOrCreate, FileAccess.Write);
						fileOutput.Seek(0, SeekOrigin.Begin);
					}
				}

				// Ftp ������ �ִ� ���� ������
				long fSize =  this.GetFileSize(Path.GetFileName(remFileName));
						
				//codes: 125 150 110 226 250 425 426 451 450 550 500 501 421 530
				this.sendCommand("RETR " + remFileName);
						
				//codes: 125,150,110,250,226
				if ( this.resultCode != 150 && this.resultCode != 125 && this.resultCode != 110 && this.resultCode != 250 && this.resultCode != 226 )
				{
					throw new Exception(this.resultCode.ToString());			
				}
				
				pos = (int)offset; // ���� offset ��ġ
				while (true)
				{
					this.bytes = cSocket.Receive(buffer, buffer.Length, 0);
					
					//���� ����
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
				Trace.Write("�ٿ� ����:"+ex.Message);
				throw new Exception(ex.Message);			
			}
		}



		/// <summary>
		/// ���α׷� ������Ʈ �ٿ�ε� ��
		/// </summary>		
		public void UpdateDownXML(string remFileName ,string locFileName ,bool resume, bool binary)
		{
			try
			{				
				if (!this.connected ) 
					this.ConnectFtp();
				
				//File Type
				this.BinaryMode = binary;			

				//���� ���ϸ����� ����
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
					if ( offset > 0 )	//���� �̾�ޱ�
					{
						// codes: 500 501 502 421 530 350
						this.sendCommand( "REST " + offset );
						if (this.resultCode != 350)
							offset = 0;	//resume ������	
						else
						{	
							// ������ FileMode.Open ���� ����� ��.
							fileOutput = new FileStream(locFileName, FileMode.Open);
							// fileOutput.Seek( offset, SeekOrigin.Begin );
						}
					}
					else
					{	// ���� ���Ӱ� ����
						// fileOutput = new FileStream(locFileName ,FileMode.Create);
						fileOutput = new FileStream(locFileName ,FileMode.OpenOrCreate, FileAccess.Write);
						fileOutput.Seek(0, SeekOrigin.Begin);
					}
				}

				// Ftp ������ �ִ� ���� ������
				long fSize =  this.GetFileSize(Path.GetFileName(remFileName));
						
				//codes: 125 150 110 226 250 425 426 451 450 550 500 501 421 530
				this.sendCommand("RETR " + remFileName);
						
				//codes: 125,150,110,250,226
				if ( this.resultCode != 150 && this.resultCode != 125 && this.resultCode != 110 && this.resultCode != 250 && this.resultCode != 226 )
				{
					throw new Exception(this.resultCode.ToString());			
				}
				
				pos = (int)offset; // ���� offset ��ġ
				while (true)
				{
					this.bytes = cSocket.Receive(buffer, buffer.Length, 0);
					
					//���� ����
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
				Trace.Write("�ٿ� ����:"+ex.Message);
				throw new Exception(ex.Message);			
			}
		}


		#region ���丮 ���� ��ɾ�..
		//----------------------------------------

		/// <summary>
		/// ���丮 ����
		/// </summary>		
		public string ChangeDir(string dirName)
		{
			if( dirName == null || dirName.Equals(".") || dirName.Length == 0 )
			{
				return "";
			}

			if ( !this.connected ) this.ConnectFtp();

			
			// ���丮 ����.codes: 250 500 501 502 421 530 550
			this.sendCommand( "CWD " + dirName );
			
			if ( (this.resultCode == 500) || (this.resultCode == 501) || (this.resultCode == 502) || (this.resultCode == 421) || (this.resultCode == 530) || (this.resultCode == 550) )
				 throw new Exception(result.Substring(4));
			
			//���� ��ġ: codes: 257 500 501 502 421 550
			this.sendCommand( "PWD" );

			if ( (this.resultCode == 500) || (this.resultCode == 501) || (this.resultCode == 502) || (this.resultCode == 421) || (this.resultCode == 550) )
				throw new Exception(result.Substring(4));
			
			this.remotePath = this.message.Split('"')[1];			
			return this.remotePath;
		}

		

		/// <summary>
		/// ���丮 ����
		/// </summary>
		public void MakeDir(string dirName)
		{
			if ( !this.connected ) this.ConnectFtp();

			// ���丮 �����.codes: 257 500 501 502 421 530 550</remarks>
			this.sendCommand( "MKD " + dirName );

			if ( this.resultCode != 250 && this.resultCode != 257 )
				throw new Exception(this.result.Substring(4));				

//			MessageBox.Show(dirName + " ����� �Ϸ�");
		}



		/// <summary>
		/// ���丮 ����
		/// </summary>
		public void RemoveDir(string dirName)
		{
			if ( !this.connected ) this.ConnectFtp();
		
			// ���� ��ɾ�. codes: 250 500 501 502 421 530 550
			this.sendCommand( "RMD " + dirName );

			if ( this.resultCode != 250 )  
				throw new Exception(this.result.Substring(4));
			
			
//			MessageBox.Show(dirName + " ���� �Ǿ����ϴ�");

		}

		
		//----------------------------------------
		#endregion


		/// <summary>
		/// ���� �޽��� ó��
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
		/// ���� �޽��� ����
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
