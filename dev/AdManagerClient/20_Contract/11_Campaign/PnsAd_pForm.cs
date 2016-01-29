// ===============================================================================
// SchHomeAd SearchForm  for Charites Project
//
// ItemAd_pForm.cs
//
// 홈광고 편성대상 조회. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
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

namespace AdManagerClient._20_Contract._11_Campaign
{
    public partial class PnsAd_pForm : Form
    {
        #region 이벤트핸들러및 함수
        public event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러

        private void StatusMessage(string strMessage)
        {
            if (StatusEvent != null)
            {
                StatusEventArgs ea = new StatusEventArgs();
                ea.Message = strMessage;
                StatusEvent(this, ea);
            }
        }
        #endregion

        #region 사용자정의 객체 및 변수
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;

        // 사용할 정보모델
        CampaignModel campaignModel = new CampaignModel();	// 홈광고편성모델

        // 화면처리용 변수
        bool IsNewSearchKey = true;					// 검색어입력 여부

        // 이 창을 연 컨트롤
        CampaignControl Opener = null;

        // 매체
        public string keyMediaCode = "";			// 팝업호출시 매체셋트
        public string keyContractSeq = "";			// 팝업호출시 매체셋트
        public string keyCampaign = "";			// 팝업호출시 매체셋트

        #endregion

        public PnsAd_pForm(UserControl parent)
        {
            InitializeComponent();
            Opener = (CampaignControl)parent;
            campaignModel.Init();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchList();
        }

        /// <summary>
        /// 그리드 데이터를 읽어온다
        /// </summary>
        private void SearchList()
        {
            try
            {
                grdExItemList.UnCheckAllRecords();
                campaignModel.Init();

                // 데이터모델에 전송할 내용을 셋트한다.
                if (ebSearchKey.Text.Length == 0)
                {
                    campaignModel.SearchKey = "";
                }
                else
                {
                    campaignModel.SearchKey = ebSearchKey.Text;
                }

                if (chkAdState_10.Checked) campaignModel.SearchchkAdState_10 = "Y";
                else campaignModel.SearchchkAdState_10 = "N";
                if (chkAdState_20.Checked) campaignModel.SearchchkAdState_20 = "Y";
                else campaignModel.SearchchkAdState_20 = "N";
                campaignModel.SearchchkAdState_30 = "N";
                campaignModel.SearchchkAdState_40 = "N";

                
                // 목록조회 서비스를 호출한다.
                new CampaignManager(systemModel, commonModel).GetPnsPopList(campaignModel);

                if (campaignModel.ResultCD.Equals("0000"))
                {
                    campaign_pDs.PnsList.Clear();
                    Utility.SetDataTable(campaign_pDs.PnsList, campaignModel.ContractDataSet);
                    grdExItemList.Focus();
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("팝업목록 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("팝업목록 조회오류", new string[] { "", ex.Message });
            }
        }

        private bool AddItem()
        {
            int selCnt = 0;
            int runCnt = 0;
            
            grdExItemList.UpdateData();
            DataRow[] foundRows = campaign_pDs.PnsList.Select("CheckYn = 'True'");

            selCnt = foundRows.Length;

            if (selCnt == 0)
            {
                MessageBox.Show("선택된 팝업이 없습니다.", "캠페인관리", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            
            try
            {
                foreach (DataRow row in foundRows)
                {
                    campaignModel.Init();
                    campaignModel.CampaignCode = keyCampaign;
                    campaignModel.ItemNo = row["PnsId"].ToString();
                    campaignModel.CampaignName = row["PnsName"].ToString();
                    campaignModel.AgencyCode = row["PnsCampaign"].ToString();
                    campaignModel.AdvertiserCode = row["PnsMaterial"].ToString();

                    new CampaignManager(systemModel, commonModel).SetCampaignPnsCreate(campaignModel);

                    if (campaignModel.ResultCD.Equals("0000"))
                    {
                        runCnt++;
                    }
                }

                if (runCnt > 0)
                {
                    Opener.ReloadList();
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("캠페인디테일 편성오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("캠페인디테일 편성오류", new string[] { "", ex.Message });
            }
            return true;
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            AddItem();
            this.Close();
        }

        private void ClearListCheck()
        {

            // 체크된 모든 항목을 클리어
            grdExItemList.UnCheckAllRecords();
            grdExItemList.UpdateData();
        }
    }
}
