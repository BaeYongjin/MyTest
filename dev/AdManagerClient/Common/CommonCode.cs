using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient.Common
{
    class CommonCode
    {
        public static void Init_RepCode(SystemModel sysModel, CommonModel commonModel, Janus.Windows.EditControls.UIComboBox combo)
        {
            // 랩을 조회한다.
            MediaRapCodeModel model = new MediaRapCodeModel();
            new AdManagerClient.MediaRapCodeManager(sysModel, commonModel).GetMediaRapCodeList(model);

            if (model.ResultCD.Equals("0000"))
            {
                combo.Items.Clear();

                // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
                Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[model.ResultCnt + 1];

                comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택", "00");

                for (int i=0; i<model.ResultCnt; i++)
                {
                    DataRow row = model.MediaRapCodeDataSet.Tables[0].Rows[i];
                                        
                    string val = row["RapCode"].ToString();
                    string txt = row["RapName"].ToString();
                    comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
                }
                // 콤보에 셋트
                combo.Items.AddRange(comboItems);
                combo.SelectedIndex = 0;

                if (commonModel.UserLevel == "30")
                {
                    combo.SelectedValue = commonModel.RapCode;
                    combo.ReadOnly = true;
                }
            }
        }

        public static void Init_AdType(SystemModel sysModel, CommonModel commonModel, Janus.Windows.EditControls.UIComboBox combo)
        {
            // 코드에서 내역상태를 조회한다.
            CodeModel codeModel = new CodeModel();
            codeModel.Section = "26";				// '26':광고종류  TODO: 코드분류는 추후 XML로 관리되어야...
            new CodeManager(sysModel, commonModel).GetCodeList(codeModel);

            if (codeModel.ResultCD.Equals("0000"))
            {
                combo.Items.Clear();
                
                // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
                Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

                comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("[광고종류]","00");

                for (int i=0; i<codeModel.ResultCnt; i++)
                {
                    DataRow row = codeModel.CodeDataSet.Tables[0].Rows[i];

                    string val = row["Code"].ToString();
                    string txt = row["CodeName"].ToString();
                    comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
                }

                // 콤보에 셋트
                combo.Items.AddRange(comboItems);
                combo.SelectedIndex = 0;
            }
        }
    }
}
