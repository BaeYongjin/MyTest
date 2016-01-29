using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
    public partial class AnalysisItemAutoAddForm : Form
    {

        #region 사용자정의 객체 및 변수

        AnalysisItemGroupModel analysisItemGroupModel = new AnalysisItemGroupModel();	// 분석용 광고묶음 모델
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;

        private string filter = "";
        private string analysisItemGroupName = "";
        private string analysisItemGroupMonth = "";
        private int analysisItemGroupNo = 0;

        #endregion

        #region 컨트롤 로드

        public AnalysisItemAutoAddForm(DataSet bc)
        {
            InitializeComponent();
            Utility.SetDataTable(analysisItemGroupDs.ContractItem, bc);


            //bsContractItem.
        }

        #endregion

        #region Public 메소드

        //바인딩소스에서 사용할 filter문장
        public string Filter
        {
            set { filter = value; }
            get { return filter; }
        }

        public int AnalysisItemGroupNo
        {
            set { analysisItemGroupNo = value; }
            get { return analysisItemGroupNo; }
        }

        public string AnalysisItemGroupName
        {
            set { analysisItemGroupName = value; }
            get { return analysisItemGroupName; }
        }

        public string AnalysisItemGroupMonth
        {
            set { analysisItemGroupMonth = value; }
            get { return analysisItemGroupMonth; }
        }

        #endregion

        #region 이벤트핸들러
        public event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler ProgressEvent;			// 처리중이벤트 핸들러
        #endregion

        #region 이벤트함수

        private void StatusMessage(string strMessage)
        {
            if (StatusEvent != null)
            {
                StatusEventArgs ea = new StatusEventArgs();
                ea.Message = strMessage;
                StatusEvent(this, ea);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            StatusMessage("자동등록이 진행중입니다.");

            if (ebAnalysisItemGroupName.Text.Trim().Length == 0)
            {
                MessageBox.Show("광고묶음명이 입력되지 않았습니다.", "광고묶음 저장",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ebAnalysisItemGroupName.Focus();
                return;
            }

            int groupno = 0;
            analysisItemGroupModel.AnalysisItemGroupName = ebAnalysisItemGroupName.Text;
            analysisItemGroupModel.AnalysisItemGroupMonth = AnalysisItemGroupMonth.Substring(2, 4);
            analysisItemGroupModel.AnalysisItemGroupType = "1";
            analysisItemGroupModel.Comment = ebComment.Text;
            new AnalysisItemGroupManager(systemModel, commonModel).SetAnalysisItemGroupCreate(analysisItemGroupModel);
            //ItemGroupNoCm = analysisItemGroupModel.AnalysisItemGroupNo; //리턴해줘야함
            groupno = analysisItemGroupModel.AnalysisItemGroupNo;
            //광고그룹등록성공하면 다음작업
            if (analysisItemGroupModel.ResultCD.Equals("0000"))
            {
                AnalysisItemGroupNo = analysisItemGroupModel.AnalysisItemGroupNo;
                analysisItemGroupModel.Init();

                int SetCount = 0;
                int AddCount = 0;

                foreach (Janus.Windows.GridEX.GridEXRow row in grdExContractItemList.GetCheckedRows())
                {

                    analysisItemGroupModel.Init();
                    analysisItemGroupModel.AnalysisItemGroupNo = groupno;

                    analysisItemGroupModel.ItemNo = Convert.ToInt16(row.Cells["ItemNo"].Value.ToString());

                    new AnalysisItemGroupManager(systemModel, commonModel).SetAnalysisItemGroupDetailCreate(analysisItemGroupModel);
                    AddCount++;
                    if (analysisItemGroupModel.ResultCD.Equals("0000"))
                    {
                        SetCount++;
                    }

                }

                if (SetCount > 0)
                {
                    StatusMessage("분석용광고묶음상세가 추가되었습니다.");
                    if (AddCount != SetCount)
                    {
                        MessageBox.Show("( " + (AddCount - SetCount) + " )건의 광고등록에 실패하였습니다.", "광고묶음자동 저장",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("등록된 광고목록이 없습니다.", "광고묶음자동 저장",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.Yes;
                this.Close();

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void AnalysisItemAutoAddForm_Load(object sender, EventArgs e)
        {
            //바인딩소스에서 where문과 같은기능을 지원한다.
            bsContractItem.Filter = filter;
            ebAnalysisItemGroupName.Text = AnalysisItemGroupName;
            grdExContractItemList.CheckAllRecords();
        }

        #endregion
    }
}
