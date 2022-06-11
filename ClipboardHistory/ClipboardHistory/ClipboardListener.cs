using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClipboardHistory
{
    class ClipboardListener
    {
        private IntPtr nextClipboardViewer=IntPtr.Zero;
        private Form form=null;

        public ClipboardListener(Form form)
        {
            this.form = form;
            this.nextClipboardViewer = IntPtr.Zero;
        }

        public IntPtr addListen()
        {
            if (isListening())
            {
                return nextClipboardViewer;
            }
            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)form.Handle);
            return nextClipboardViewer;
        }

        public void removeListen()
        {
            if (!isListening())
            {
                return;
            }
            ChangeClipboardChain(form.Handle, nextClipboardViewer);
            this.nextClipboardViewer = IntPtr.Zero;
        }

        public bool isListening()
        {
            return this.nextClipboardViewer!=IntPtr.Zero;
        }

        public bool winProcCallback(ref Message msg)
        {
            if (!isListening())
            {
                return false;
            }
            bool ret = false;
            // defined in winuser.h
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (msg.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    ret = true;
                    //SendMessage(nextClipboardViewer, msg.Msg, msg.WParam, msg.LParam);
                    break;
                case WM_CHANGECBCHAIN:
                    //if (msg.WParam == nextClipboardViewer)
                    //    nextClipboardViewer = msg.LParam;
                    //else
                    //    SendMessage(nextClipboardViewer, msg.Msg, msg.WParam, msg.LParam);
                    break;
                default:
                    break;
            }

            return ret;
        }

        #region WindowsAPI
        /// <summary>
        /// 将CWnd加入一个窗口链，每当剪贴板的内容发生变化时，就会通知这些窗口
        /// </summary>
        /// <param name="hWndNewViewer">句柄</param>
        /// <returns>返回剪贴板观察器链中下一个窗口的句柄</returns>
        [DllImport("User32.dll")]
        protected static extern int SetClipboardViewer(int hWndNewViewer);

        /// <summary>
        /// 从剪贴板链中移出的窗口句柄
        /// </summary>
        /// <param name="hWndRemove">从剪贴板链中移出的窗口句柄</param>
        /// <param name="hWndNewNext">hWndRemove的下一个在剪贴板链中的窗口句柄</param>
        /// <returns>如果成功，非零;否则为0。</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        /// <summary>
        /// 将指定的消息发送到一个或多个窗口
        /// </summary>
        /// <param name="hwnd">其窗口程序将接收消息的窗口的句柄</param>
        /// <param name="wMsg">指定被发送的消息</param>
        /// <param name="wParam">指定附加的消息特定信息</param>
        /// <param name="lParam">指定附加的消息特定信息</param>
        /// <returns>消息处理的结果</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        #endregion
    }
}
