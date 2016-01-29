
#region �ּ�...
/*
 * -------------------------------------------------------
 * Class Name: IUserControl
 * �ֿ���  : ����� ���� ��Ʈ���� ���� interface
 * �ۼ���    : Bae 
 * �ۼ���    : 2010.05.11
 * Ư�̻���  : ��� ����� ���� ��Ʈ���� �� �������̽� ���
 *             �������� �̺�Ʈ�� �Ӽ��� ���� �� ����
 *             �̸� ���� ����ϴ�(ȣ��)�ϴ� ������
 *             �����ڵ�� ó���ϱ� ���ؼ� �����
 * -------------------------------------------------------
 * [��������]-bae
 * -------------------------------------------------------
 * �ڵ�      : 
 * ������    : 
 * ������    : 
 * ��������  : 
 * --------------------------------------------------------
 */
#endregion

using System;
using System.Windows.Forms;

using WinFramework.Base;

namespace AdManagerClient
{
	/// <summary>
	/// IUserControl�� ���� ��� �����Դϴ�.
	/// </summary>
	public interface IUserControl
	{
        #region �̺�Ʈ�ڵ鷯
        event StatusEventHandler StatusEvent;			// �����̺�Ʈ �ڵ鷯
        event ProgressEventHandler ProgressEvent;	    // ó�����̺�Ʈ �ڵ鷯
        #endregion

        /// <summary>
        /// �޴� �ڵ�-������ �ʿ��� ȭ�鿡 �ʿ���-�������
        /// </summary>
        string MenuCode
        { set; get; }
        /// <summary>
        /// �θ� ��Ʈ�� ����-�������
        /// </summary>
        /// <param name="control"></param>
        void SetParent(Control control);
        /// <summary>
        /// DockStype ����-�������
        /// </summary>
        /// <param name="style"></param>
        void SetDockStyle(DockStyle style);	
	}
}
