using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Diagnostics;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;
using Excel = Microsoft.Office.Interop.Excel; // 엑셀 참조
using System.Reflection;

namespace AdManagerClient._10_Media._10_Group
{
	/// <summary>
	/// GroupMappingInfo에 대한 요약 설명입니다.
	/// </summary>
	public class GroupMappingInfo : System.Windows.Forms.Form
	{
        #region 이벤트핸들러
        public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러

        private void StatusMessage(string strMessage)
        {
            if (StatusEvent != null) 
            {
                StatusEventArgs ea = new StatusEventArgs();
                ea.Message   = strMessage;
                StatusEvent(this,ea);
            }
        }
        #endregion

        private AdManagerClient._10_Media._10_Group.GroupDs groupDs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Janus.Windows.EditControls.UIButton btnExcel;
        private Janus.Windows.EditControls.UIButton btnClose;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.GridEX gridEXMain;
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GroupMappingInfo()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			
			//
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupMappingInfo));
            Janus.Windows.GridEX.GridEXLayout gridEXMain_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridEXMain = new Janus.Windows.GridEX.GridEX();
            this.groupDs = new AdManagerClient._10_Media._10_Group.GroupDs();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEXMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnExcel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 40);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 40);
            this.label1.TabIndex = 7;
            this.label1.Text = "카테고리 및 장르에 설정된 편성그룹 현황";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.Icon = ((System.Drawing.Icon)(resources.GetObject("btnClose.Icon")));
            this.btnClose.Location = new System.Drawing.Point(672, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.ShowFocusRectangle = false;
            this.btnClose.Size = new System.Drawing.Size(104, 28);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.ToolTipText = "화면닫기";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.BackColor = System.Drawing.Color.White;
            this.btnExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExcel.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExcel.Icon = ((System.Drawing.Icon)(resources.GetObject("btnExcel.Icon")));
            this.btnExcel.Location = new System.Drawing.Point(560, 5);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.ShowFocusRectangle = false;
            this.btnExcel.Size = new System.Drawing.Size(104, 28);
            this.btnExcel.TabIndex = 5;
            this.btnExcel.Text = "Excel";
            this.btnExcel.ToolTipText = "엑셀파일로 변환합니다";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridEXMain);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(794, 707);
            this.panel2.TabIndex = 1;
            // 
            // gridEXMain
            // 
            this.gridEXMain.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEXMain.AlternatingColors = true;
            this.gridEXMain.AutomaticSort = false;
            this.gridEXMain.BorderStyle = Janus.Windows.GridEX.BorderStyle.RaisedLight3D;
            this.gridEXMain.ColumnAutoResize = true;
            this.gridEXMain.DataSource = this.groupDs.GroupMap;
            this.gridEXMain.DefaultFilterRowComparison = Janus.Windows.GridEX.FilterConditionOperator.Equal;
            gridEXMain_DesignTimeLayout.LayoutString = resources.GetString("gridEXMain_DesignTimeLayout.LayoutString");
            this.gridEXMain.DesignTimeLayout = gridEXMain_DesignTimeLayout;
            this.gridEXMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEXMain.EmptyRows = true;
            this.gridEXMain.FocusCellFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.SunkenLight;
            this.gridEXMain.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.gridEXMain.FocusCellFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.gridEXMain.FocusCellFormatStyle.ForeColor = System.Drawing.Color.Gold;
            this.gridEXMain.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.gridEXMain.Font = new System.Drawing.Font("굴림", 9F);
            this.gridEXMain.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEXMain.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEXMain.GroupByBoxVisible = false;
            this.gridEXMain.Location = new System.Drawing.Point(0, 0);
            this.gridEXMain.Name = "gridEXMain";
            this.gridEXMain.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.gridEXMain.Size = new System.Drawing.Size(794, 707);
            this.gridEXMain.TabIndex = 0;
            this.gridEXMain.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.gridEXMain.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.gridEXMain.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // groupDs
            // 
            this.groupDs.DataSetName = "GroupDs";
            this.groupDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.groupDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GroupMappingInfo
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(794, 747);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "GroupMappingInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "편성그룹 설정 현황";
            this.Load += new System.EventHandler(this.GroupMappingInfo_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEXMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        // 사용할 정보모델
        GroupModel groupModel  = new GroupModel();	// 컨텐츠정보모델
        #endregion

        private void SearchData()
        {
            StatusMessage("그룹설정 정보를 조회합니다.");

            try
            {
                // 컨텐츠목록조회 서비스를 호출한다.
                new GroupManager(systemModel,commonModel).GetGroupMapList(groupModel);

                if (groupModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(groupDs.GroupMap, groupModel.GroupMapDataSet);				
                    StatusMessage(groupModel.ResultCnt + "건의 그룹설정 정보가 조회되었습니다.");
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("그룹설정 조회 오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("그룹설정 조회 오류",new string[] {"",ex.Message});
            }
        }

        private void GroupMappingInfo_Load(object sender, System.EventArgs e)
        {
            SearchData();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {

            this.Close();
        }

        #region 엑셀 출력
        /// <summary>
        /// 엑셀 생성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, System.EventArgs e)
        {	
            Excel.Application xlApp= null;
            Excel._Workbook   oWB = null;
            Excel._Worksheet  oSheet = null;
            Excel.Range       oRng = null;
			
            try
            {	
                int ColMax          = 7; // 컬럼수   				

                int TitleRow        = 1;
                int HeaderRow       = 2;
                int DataRow         = 3;
                string StartCol     = "A";
                string EndCol       = "G";
                int DataCount       = 0;
                int HeaderCount     = 0;

                // 마지막 컬럼의 인덱스문자
                EndCol = GetColumnIndex(ColMax);

                xlApp = new Excel.Application();
                xlApp.Visible = true;
                xlApp.UserControl = true;

                oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                // 타이틀
                oSheet.Cells[TitleRow,1] = "카테고리/장르별 편성그룹 설정현황";
                oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), EndCol+Convert.ToString(TitleRow));	// 헤더의 범위
                oRng.Font.Bold  = true;
                oRng.Font.Size  = 10;
                oRng.Merge(true);

                // 헤더
                HeaderCount = 1;
                oSheet.Cells[HeaderRow,HeaderCount++] = "카테고리";
                oSheet.Cells[HeaderRow,HeaderCount++] = "장르";
                oSheet.Cells[HeaderRow,HeaderCount++] = "설정1";
                oSheet.Cells[HeaderRow,HeaderCount++] = "설정2";
                oSheet.Cells[HeaderRow,HeaderCount++] = "설정3";
                oSheet.Cells[HeaderRow,HeaderCount++] = "설정4";
                oSheet.Cells[HeaderRow,HeaderCount++] = "설정5";

                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow));
                oRng.Font.Bold              = true;							// 폰트 굵게
                oRng.Font.Size              = 8;
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
                oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
                oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);			//텍스트색			
				
                DataCount = 0;
                // 데이터 추출
                string Category = "";

                for (int inx =0; inx < groupDs.GroupMap.Rows.Count; inx++)
                {		
                    DataRow Row = groupDs.GroupMap.Rows[inx];

                    int ColCnt = 1;

                    if( Row["MenuLevel"].ToString() == "1" )
                    {
                        oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount));
                        oRng.Font.Bold              = true;							// 폰트 굵게
                        oRng.Font.Size              = 8;
                        oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);   //셀 배경색 

                        Category = Row["MenuName"].ToString();
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Category;
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = "";
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Group1Nm"].ToString();
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Group2Nm"].ToString();
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Group3Nm"].ToString();
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Group4Nm"].ToString();
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Group5Nm"].ToString();
                    }
                    else
                    {
                        oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount));
                        oRng.Font.Size   = 8;

                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Category;
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["MenuName"].ToString();
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Group1Nm"].ToString();
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Group2Nm"].ToString();
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Group3Nm"].ToString();
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Group4Nm"].ToString();
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Group5Nm"].ToString();
                    }

                    DataCount++;
                }

                DataCount--;

                // 데이터 작성
                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
                oRng.Font.Size = 8;
                oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤
                oRng.RowHeight = 14;
                oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
                oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetColumnIndex(int ColCount)
        {
            string[] ColName = {"Z","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y"};

            string ColumnIndex;

            // 26보다 크면
            if(ColCount > ColName.Length)
            {
                // 2자리 인덱스문자 26 => Z;  27->AA
                ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount/ColName.Length)))] + ColName[(int)(Math.Floor((float)(ColCount%ColName.Length)))];
            }
            else
            {
                ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount%ColName.Length)))];
            }

            return ColumnIndex;
        }

        #endregion
	}
}
