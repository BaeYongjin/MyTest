
/*
 * -------------------------------------------------------
 * Class Name: UserMessageBox
 * 주요기능  : 메세지박스 레이블 설정
 * 작성자    : YJ.Park
 * 작성일    : 2014.08.07
 * -------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AdManagerClient.Common
{ 
    class UserMessageBox
    {
        delegate int hookProc(int code, IntPtr wParam, IntPtr IParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int hook, hookProc callback, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern IntPtr GetDlgItem(IntPtr hDlg, DialogResult nIDDlgItem);
        [DllImport("user32.dll")]
        static extern bool SetDlgItemText(IntPtr hDlg, DialogResult nIDDlgItem, string lpString);
        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();
        static IntPtr g_hHook;
        static string yes;
        static string cancel;
        static string no;

        /// <summary>
        /// 메시지 박스를 띠웁니다.
        /// </summary>
        /// <param name="text">텍스트 입니다.</param>
        /// <param name="caption">캡션 입니다.</param>
        /// <param name="yes">예 문자열 입니다.</param>
        /// <param name="no">아니오 문자열 입니다.</param>
        /// <param name="cancel">취소 문자열 입니다.</param>
        /// <returns></returns>
        public static DialogResult Show(string text, string caption, string yes, string no, string cancel)
        {
            UserMessageBox.yes = yes;
            UserMessageBox.cancel = cancel;
            UserMessageBox.no = no;
            g_hHook = SetWindowsHookEx(5, new hookProc(HookWndProc), IntPtr.Zero, GetCurrentThreadId());
            return MessageBox.Show(text, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        }
        static int HookWndProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            IntPtr hChildWnd;
            if (nCode == 5)
            {
                hChildWnd = wParam;
                if (GetDlgItem(hChildWnd, DialogResult.Yes) != null)
                    SetDlgItemText(hChildWnd, DialogResult.Yes, UserMessageBox.yes);
                if (GetDlgItem(hChildWnd, DialogResult.No) != null)
                    SetDlgItemText(hChildWnd, DialogResult.No, UserMessageBox.no);
                if (GetDlgItem(hChildWnd, DialogResult.Cancel) != null)
                    SetDlgItemText(hChildWnd, DialogResult.Cancel, UserMessageBox.cancel);
                UnhookWindowsHookEx(g_hHook);
            }
            else
                CallNextHookEx(g_hHook, nCode, wParam, lParam);
            return 0;
        }
    }
}
