using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;

using DZ.MediaPlayer;
using DZ.MediaPlayer.Vlc.Io;
using DZ.MediaPlayer.Vlc.WindowsForms;
using DZ.MediaPlayer.Vlc.Deployment;
using SimplePlayer.MediaInfo;

using AdManagerModel;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;


namespace AdManagerClient.AdFile
{
    public partial class AdFile_Viewer2 : Form
    {
        private bool isUpdateTrack;
        private PlaylistItem playListItem;

        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private AdFileModel adFileModel = new AdFileModel();	// 광고파일모델

        private string localFIle = string.Empty;
        private string _fileName = string.Empty;
        // FTP업로드정보
        string FtpUploadHost;
        string FtpUploadPort;
        string FtpUploadID;
        string FtpUploadPW;

        private FtpManager ftm;
        private string fileMax = string.Empty;
        private bool firstDraw = true;
        private DateTime start;
        private TimeSpan timeSpan;
        private int tm, m, s = 0;

        private bool _fileCdn = false;
        /// <summary>
        /// 광고파일이 CDN에 있는지 여부를 설정한다
        /// </summary>
        public bool FileCDN
        {
            set
            {
                _fileCdn = value;
            }
            get
            {
                return _fileCdn;
            }
        }

        public AdFile_Viewer2()
        {
            InitializeComponent();
            initDeployment();
            initializeVideoWindow();
            initializeStatusBar();
            initializeTrackbarPosition();
            initializeTrackbarVolume();

            isUpdateTrack = true;
        }
        
        /// <summary>
        /// 재생할 파일이름을 가져오거나 설정합니다
        /// </summary>
        public string FileName
        {
            set
            {
                _fileName = value;
            }
            get
            {
                return _fileName;
            }
        }

        /// <summary>
        /// Download N Play
        /// </summary>
        public void RunJob()
        {
            // 다운로드 기능 호출
            btnDown_Click(this, EventArgs.Empty);
        }

        private void initDeployment()
        {
            VlcDeployment deployment = VlcDeployment.Default;
            if (!deployment.CheckVlcLibraryExistence(true, false))
            {               
                deployment.Install(true, true, false, false);               
            }
        }

        private void initializeVideoWindow()
        {
            if (!vlcPlayer.IsInitialized)
                vlcPlayer.Initialize(this);
            vlcPlayer.StateChanged += vlc_onStateChanged;
            vlcPlayer.PositionChanged += vlc_onPositionChanged;
            vlcPlayer.EndReached += vlc_onEndReached;
            vlcPlayer.TimeChanged += vlc_onTimeChanged;
        }

        private void initializeStatusBar()
        {
            stlabel.Items["stlabelTitle"].Text = "상태:";
            stlabel.Items["stlabelDesc"].Text = String.Format("{0}", vlcPlayer.Time);
        }
        private void initializeTrackbarPosition()
        {
            
        }
        private void initializeTrackbarVolume()
        {
            tbVolume.Value = (int)((1.0 * vlcPlayer.Volume / 100) * tbVolume.Maximum);
        }

        /// <summary>
        /// VLC Player 초기화
        /// </summary>
        private void initializeVlcPlayerControl()
        {
            initializeVideoWindow();

            if (!vlcPlayer.IsInitialized)
                vlcPlayer.Initialize(this);
        }


        #region VLC Player 이벤트 핸들링...
        
        private void vlc_onStateChanged(object sender, EventArgs e)
        {
            VlcPlayerControlState currentState = vlcPlayer.State;
            switch (currentState)
            {
                case VlcPlayerControlState.Idle:
                    {
                        btnPlaynPause.Text = "Play";
                        break;
                    }
                case VlcPlayerControlState.Paused:
                    {
                        btnPlaynPause.Text = "Resume";
                        break;
                    }
                case VlcPlayerControlState.Playing:
                    {
                        btnPlaynPause.Text = "Pause";
                        break;
                    }

            }
            stlabel.Items["stlabelDesc"].Text = Convert.ToString(currentState);
            
        }
        private void vlc_onTimeChanged(object sender, EventArgs e)
        {
            // get currenttime bug 인듯...라이브러리에서 데이터를 
            // 가져오는데 0 으로 찍힘..
            //updateStatusBar();
        }
        private void vlc_onPositionChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new ThreadStart(callBackPosition));
            else
                callBackPosition();
        }
        private void callBackPosition()
        {
            updatePositionBar();
            //updateStatusBar();
        }


        private void updatePositionBar()
        {
            if (!isUpdateTrack)
                return;
            
            try
            {
                int newPos = (int)(vlcPlayer.Position * 1000);
                if (newPos != pgBarPlay.Value)
                {
                    pgBarPlay.Value = newPos > pgBarPlay.Maximum ? pgBarPlay.Maximum : newPos;
                }
            }
            finally
            {
                
            }
        }
        private void updateStatusBar()
        {
            //Trace.WriteLine("Time:" + vlcPlayer.Time);
            //TimeSpan time = vlcPlayer.Time;
            //stlabel.Items["stlabelDesc"].Text = String.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
        }
        
        
        /// <summary>
        /// 재생 완료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vlc_onEndReached(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new ThreadStart(callBackComplete));
            }
            else
                callBackComplete();                
        }
        private void callBackComplete()
        {
            stlabel.Items["stlabelDesc"].Text = "광고 재생이 완료되었습니다.";
            //stopPlayer();
        }
        

        private void tbPosition_MouseDown(object sender, MouseEventArgs e)
        {
            isUpdateTrack = false;
        }

        private void tbPosition_MouseUp(object sender, MouseEventArgs e)
        {
            isUpdateTrack = true;
        }

        #endregion


        private void downLoadFile()
        {
            long localSize = 0;
            FileInfo loFile = null;
            stlabel.Items["stlabelDesc"].Text = "로컬파일 확인중...";
           
            Application.DoEvents();

            try
            {
                localFIle = Environment.CurrentDirectory + @"\adv\" + this.FileName;
                //localFIle = @"N:\Temp\ts파일\c11314-110901162317.ts";
                if (File.Exists(localFIle))
                {
                    loFile = new FileInfo(localFIle);
                    localSize = loFile.Length;
                    File.Delete(localFIle);
                }

                stlabel.Items["stlabelDesc"].Text = "FTP 접속 중...";
                Application.DoEvents();

                // FTP 생성//
                createFtp();
                stlabel.Items["stlabelDesc"].Text = "원격 파일 확인 중...";
                Application.DoEvents();

                long remoteLen = checkFile("/adv", _fileName);
                if (remoteLen == 0)
                {
                    FrameSystem.showMsgForm("파일 다운로드 오류", new string[] { "파일명: /adv/" + this.FileName, "서버에 광고파일이 존재하지 않습니다.", "관리자에게 문의하십시오!" });

                    ftm.Close();
                    ftm = null;
                    stlabel.Items["stlabelDesc"].Text = "서버에 파일이 없습니다";
                    Application.DoEvents();
                    Thread.Sleep(500);

                    lbFileFlow.Visible = false;
                    progBar.Visible = false;


                    btnDown.Enabled = false;
                    btnPlaynPause.Enabled = false;
                    return;
                }

                if (!Directory.Exists(Environment.CurrentDirectory + @"\adv"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + @"\adv");
                }

                stlabel.Items["stlabelDesc"].Text = "다운로드 시작...";
                Application.DoEvents();
                Thread.Sleep(500);


                lbFileFlow.Visible = true;
                progBar.Visible = true;
                Application.DoEvents();

                ftm.Download("/adv/" + this.FileName, localFIle, false);
                ftm.Close();
                ftm = null;

                lbFileFlow.Visible = false;
                progBar.Visible = false;
                btnDown.Enabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception("파일 다운로드 오류-파일명: /adv/" + this.FileName + "\r["+ex.Message + "] \r관리자에게 문의하십시오!");
            }
            finally
            {
                btnClose.Enabled = true;
            }
        }
        
        private void createPlayItem()
        {
            using (MediaInfoLibrary mediaInfoLibrary = new MediaInfoLibrary())
            {
                BasicVideoInformation information = MediaInfoHelper.GetBasicVideoInfo(mediaInfoLibrary, localFIle);
                if (String.IsNullOrEmpty(information.VideoCodec) && String.IsNullOrEmpty(information.AudioCodec))
                {
                    throw new Exception("코덱,오디오 예외 발생");
                }

                playListItem = new PlaylistItem(
                                    new MediaInput(MediaInputType.File, localFIle),
                                    localFIle, TimeSpan.FromMilliseconds(information.DurationMilliseconds));

            }
           
            btnPlaynPause.Enabled = true;
            stlabel.Items["stlabelDesc"].Text = "재생 준비 중...";
            Application.DoEvents();
            
        }

        private void stopPlayer()
        {
            try
            {
                if (vlcPlayer.State != VlcPlayerControlState.Idle)
                {
                    vlcPlayer.Stop();
                    updatePositionBar();
                    updateStatusBar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("정지할 수 없습니다.!:{0}", ex));
            }
        }

        private void startPlayer()
        {
            try
            {
                initializeVlcPlayerControl();
                playListItem.IsError = false;
                switch (vlcPlayer.State)
                {
                    case VlcPlayerControlState.Idle:
                        {
                            vlcPlayer.Play(playListItem.MediaInput);
                            break;
                        }
                    case VlcPlayerControlState.Paused:
                        {
                            vlcPlayer.PauseOrResume();
                            break;
                        }
                    case VlcPlayerControlState.Playing:
                        {
                            vlcPlayer.PauseOrResume();
                            break;
                        }
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("파일이 존재 하지 않습니다.!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("재생 예외발생:" + ex.Message);
            }
        }

       



        #region FTP처리함수

        private void InitFtpInfo()
        {
            try
            {
                adFileModel.Init();

                // 광고파일목록조회 서비스를 호출한다.
                new AdFileManager(systemModel, commonModel).GetFtpConfig(adFileModel);

                if (adFileModel.ResultCD.Equals("0000"))
                {
                    if (_fileCdn)
                    {
                        FtpUploadHost = adFileModel.FtpCdnHost;
                        FtpUploadPort = adFileModel.FtpCdnPort;
                        FtpUploadID = adFileModel.FtpCdnID;
                        FtpUploadPW = Security.Decrypt(adFileModel.FtpCdnPW);
                    }
                    else
                    {
                        FtpUploadHost = adFileModel.FtpUploadHost;
                        FtpUploadPort = adFileModel.FtpUploadPort;
                        FtpUploadID = adFileModel.FtpUploadID;
                        FtpUploadPW = Security.Decrypt(adFileModel.FtpUploadPW);
                    }

                    //strDefaultPath = adFileModel.FtpUploadPath;
                }
                else
                {
                    FrameSystem.showMsgForm("FTP 정보조회 오류", new string[] { adFileModel.ResultCD, adFileModel.ResultDesc });
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("FTP업로드 정보조회 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("FTP업로드 정보조회 오류", new string[] { "", ex.Message });
            }
        }

        private void createFtp()
        {
            //--------------
            // Ftp 객체 생성
            //--------------
            try
            {
                if (ftm == null)
                {
                    ftm = new FtpManager();
                    ftm.OnPosition += new PositionDelegate(ftm_OnPosition);
                    ftm.OnMaxPosition += new PositionDelegate(ftm_OnMaxPosition);

                    ftm.SetIpAddress = FtpUploadHost;
                    ftm.SetPort = Convert.ToInt32(FtpUploadPort);
                    ftm.SetUserId = FtpUploadID;
                    ftm.SetUserPwd = FtpUploadPW;
                }
            }
            catch (Exception ex)
            {
                FrameSystem.oLog.Error("FTP서버 연결오류:" + ex.Message);
            }
        }

        private long checkFile(string Path, string FileName)
        {
            //------------------
            // 서버상의 파일존재여부 체크
            //------------------
            if (ftm.IsConnected == false)
            {
                // 미연결시 3회시도
                for (int retry = 3; retry > 0; retry--)
                {
                    try
                    {
                        ftm.Connect();
                        if (ftm.IsConnected == true) break;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(500);
                    }
                }
            }

            try
            {
                long sz = ftm.GetFileSize(Path + "/" + FileName);
                return sz;
            }
            catch
            {
                return 0;
            }
           
        }


        private void ftm_OnPosition(int inx)
        {

            //-----------------------------------
            // 현재 파일 상태 프로그레스바에 그리기
            //-----------------------------------
            try
            {
                if (firstDraw)
                {
                    start = DateTime.Now;	// 업로드 시작 시각
                    firstDraw = false;
                }

                Application.DoEvents();

                if (progBar.Maximum > inx)
                {
                    lbFileFlow.Text = string.Format("{0:0,00#}", inx) + " / " + fileMax;

                    progBar.Value = inx;

                    timeSpan = DateTime.Now - start;
                    tm = Convert.ToInt32(timeSpan.TotalSeconds);
                    m = tm / 60;
                    s = tm % 60;
                    lbTimeSpan.Text = string.Format("{0:0#}", m) + ":" + string.Format("{0:0#}", s);
                }
                else if (progBar.Maximum == inx)
                {
                    if (firstDraw) return;
                    progBar.Value = 0;
                    lbFileFlow.Text = fileMax = string.Empty;
                    lbTimeSpan.Text = string.Empty;
                    firstDraw = true;
                    tm = m = s = 0;
                }
            }
            catch (Exception ex)
            {
                MessageForm mf = new MessageForm();
                mf.SetMessage = new string[] { "", "업로드 중 알 수 없는 에러가 발생했습니다.!", ex.Message };
                mf.showMessage();
                mf.ShowDialog();
            }
        }


        private void ftm_OnMaxPosition(int inx)
        {
            //-------------------------------------------------
            // 다운로드 파일의 크기 값을 프로그레스 maxValue에 설정
            //-------------------------------------------------
            fileMax = string.Format("{0:0,00#}", inx);
            progBar.Maximum = inx;
        }

        private void messgaeForm()
        {
            MessageForm msgFrm = new MessageForm();
            msgFrm.SetMessageType = 1;
            msgFrm.SetMessage = new string[] { "", "업로드 준비 중입니다..", "잠시만 기다리세요.!" };
            msgFrm.Width -= 50;
            msgFrm.showMessage();
            msgFrm.ShowDialog();
        }


        #endregion

        #region 버튼 이벤트 핸들러....

        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                btnDown.Enabled = false;
                btnClose.Enabled = false;

                downLoadFile();
                createPlayItem();

                pgBarPlay.Value = 0;
                pgBarPlay.Visible = true;
                btnClose.Enabled = true;
                btnStop.Enabled = true;
                btnPlaynPause_Click(this, EventArgs.Empty);                                                
            }
            catch (Exception ex)
            {
                MessageBox.Show("다운로드,재생 예외:" + ex.Message);
            }
        }

        private void btnPlaynPause_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnClose.Enabled = true;
            startPlayer();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopPlayer();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            stopPlayer();
            this.Close();
        }

        #endregion

        private void AdFile_Viewer2_Load(object sender, EventArgs e)
        {
            try
            {
                btnDown.Enabled = true;
                btnPlaynPause.Enabled = false;
                btnStop.Enabled = false;
                btnClose.Enabled = false;
                stlabel.Items["stlabelDesc"].Text = "다운받기로 파일을 다운받으십시요...";
                InitFtpInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void tbVolume_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                TrackBar trackBar = (TrackBar)sender;
                vlcPlayer.Volume = (int)((1.0 * trackBar.Value / trackBar.Maximum) * 100);
            }
            catch (Exception ex)
            {                
                MessageBox.Show(String.Format("볼률 조절 실패: {0}", ex));
            }
        }

       
    }

}
