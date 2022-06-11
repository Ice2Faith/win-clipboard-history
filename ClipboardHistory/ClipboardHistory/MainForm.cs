using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections.Specialized;
using Microsoft.VisualBasic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClipboardHistory
{
    
    public partial class MainForm : Form
    {
        public int childrenCount = 0;
        public List<WaitProcessItem> waitProcesses = new List<WaitProcessItem>();

        private ClipboardListener clipListener;
        private List<ClipDataItem> historys = new List<ClipDataItem>();
        private int defaultWidth;
        private int defaultHeight;
        private int fooWidth = 20;
        private int fooHeight = 20;

        private string[] tips ={
                                   "双击列表行快速复制！",
                                   "Ctrl+Z反选",
                                   "Ctrl+A全选",
                                   "Ctrl+S上锁",
                                   "Ctrl+Q解锁",
                                   "Ctrl+C复制",
                                   "Alt+D删除",
                                   "Delete删除",
                                   "Ctrl+R添加到待办",
                                   "Ctrl+W打开待办",
                                   "Ctrl+B搜索项"
                              };
        private int currentTipsIdx = 0;
        public MainForm()
        {
            InitializeComponent();

            loadWaitProcess();

            this.ShowInTaskbar = false;

            this.defaultWidth = this.Width;
            this.defaultHeight = this.Height;

            clipListener = new ClipboardListener(this);

            Rectangle workArea=Screen.GetWorkingArea(this);
            this.Location=new Point(workArea.Width/2-this.Width/2,workArea.Height*1/3);

            this.listenClipboardChange(true);

            this.topmostWindow(true);

            this.displayFloatBorderEnable(true);

            this.displayTransparentMode(true);

            this.MinimizeBox = false;

            this.timerMain.Interval = 15*1000;

            this.timerMain.Start();

            this.timerTips.Interval = 3 * 1000;

            this.timerTips.Start();
        }
        private void timerTips_Tick(object sender, EventArgs e)
        {
            string tipLine = tips[currentTipsIdx];
            currentTipsIdx = (currentTipsIdx + 1) % tips.Length;
            this.toolHelpLine.Text = tipLine;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.toolHelpLine.Text = "双击列表行快速复制！Ctrl+Z反选";
        }

        protected override void OnClosed(EventArgs e)
        {
            listenClipboardChange(false);
            this.timerMain.Stop();

            this.timerTips.Stop();
        }

        protected override void WndProc(ref Message m)
        {
            bool isChange=clipListener.winProcCallback(ref m);
            if (isChange)
            {
                IDataObject obj=Clipboard.GetDataObject();

                int idx = 0;
                for (int i = 0; i < historys.Count; i++)
                {
                    if (historys[i].locked == false)
                    {
                        idx = i;
                        break;
                    }
                }

                historys.Insert(idx, new ClipDataItem(obj));
                if (historys.Count > 128)
                {
                    historys.RemoveAt(historys.Count - 1);
                }
                uniqueHistory(30);
                refreshList();
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        public void uniqueHistory(int maxCount)
        {
            List<string> strs = new List<string>();
            Stack<int> idxs = new Stack<int>();

            for (int i = 0; i != maxCount && i<historys.Count; i++)
            {
                ClipDataItem item = historys[i];
                if(item.type==DataFormats.Html 
                    || item.type==DataFormats.OemText
                    || item.type==DataFormats.Text
                    || item.type == DataFormats.UnicodeText)
                {
                    string str = item.data as string;
                    int idx = -1;
                    for (int j = 0; j < strs.Count; j++)
                    {
                        if (str == strs[j])
                        {
                            idx = j;
                            break;
                        }
                    }
                    if (idx == -1)
                    {
                        strs.Add(str);
                    }
                    else
                    {
                        idxs.Push(i);
                    }

                }
            }
            while (idxs.Count > 0)
            {
                int idx = idxs.Pop();
                historys.RemoveAt(idx);
            }
        }

        public void listenClipboardChange(bool enable)
        {
            this.checkBoxEnable.Checked = enable;
            if (enable)
            {
                IntPtr ptr=clipListener.addListen();
                this.toolTipsLabel.Text = "已启用监听:"+ptr;
            }
            else
            {
                clipListener.removeListen();
                this.toolTipsLabel.Text = "已禁用监听";
            }
        }

        public void refreshList()
        {
            this.listViewClipItems.Items.Clear();

            int i=0;
            foreach (ClipDataItem obj in historys)
            {
                i++;
                ListViewItem item = new ListViewItem();
                item.Text = i + "";

                item.Tag = obj;

                string type = obj.type;
                string content = "";
                object data = obj.data;
                if (type==DataFormats.Text)
                {
                    content = data as string;
                }
                else if (type == DataFormats.UnicodeText)
                {
                    content = data as string;
                }
                else if (type == DataFormats.StringFormat)
                {
                    content = data as string;
                }
                else if (type == DataFormats.Rtf)
                {
                    content = data as string;
                }
                else if (type == DataFormats.OemText)
                {
                    content = data as string;
                }
                else if (type == DataFormats.Html)
                {
                    content = data as string;
                }
                else if (type == DataFormats.CommaSeparatedValue)
                {
                    content = data as string;
                }
                else if (type == DataFormats.FileDrop)
                {
                    object typ=data.GetType();
                    string[] col=data as string[];
                    string text = "";
                    foreach (object str in col)
                    {
                        text += str;
                        text += "\r\n";
                    }
                    content = text;
                }
                else
                {
                    content = "" + data;
                }

                // 类型
                item.SubItems.Add(type);
                // 锁定
                item.SubItems.Add(obj.locked ? "lock" : "");
                 // 时间
                item.SubItems.Add(String.Format("{0:HH:mm:ss MM-dd}",obj.date));
                //备注
                item.SubItems.Add(obj.comment);
                //内容
                item.SubItems.Add(content);
                
                this.listViewClipItems.Items.Add(item);
            }
        }

        public void copyItem()
        {
            if (this.listViewClipItems.SelectedItems.Count <= 0)
            {
                this.toolTipsLabel.Text = "没有选中项！";
                return;
            }

            ListViewItem item = this.listViewClipItems.SelectedItems[0];
            ClipDataItem data = item.Tag as ClipDataItem;
            string index = item.Text;

            this.historys.RemoveAt(Convert.ToInt32(index) - 1);

            IDataObject obj = new DataObject();
            obj.SetData(data.type, data.data);
            Clipboard.SetDataObject(obj);
            this.toolTipsLabel.Text = "行" + index + "已复制到剪切板：" + data.type;
            
            refreshList();
        }

        public void removeItem()
        {
            if (this.listViewClipItems.SelectedItems.Count <= 0)
            {
                this.toolTipsLabel.Text = "没有选中项！";
                return;
            }

            ListViewItem item = this.listViewClipItems.SelectedItems[0];
            ClipDataItem data = item.Tag as ClipDataItem;
            string index = item.Text;
            this.toolTipsLabel.Text = "行" + index + "已删除：" + data.type;
            this.historys.RemoveAt(Convert.ToInt32(index)-1);
            refreshList();
        }

        public void cleanAllItem()
        {
            int count = this.historys.Count;
            this.toolTipsLabel.Text = count+"行已删除";
            this.historys.Clear();
            refreshList();
        }


        public void displayFloatScreenBorder(bool foo)
        {
            if (foo)
            {
                this.WindowState = FormWindowState.Normal;
                //1、在屏幕的右下角显示窗体
                //这个区域不包括任务栏的
                Rectangle workArea = Screen.GetWorkingArea(this);
                //这个区域包括任务栏，就是屏幕显示的物理范围
                //Rectangle screenArea = Screen.GetBounds(this);
                this.Size = new Size(fooWidth,fooHeight);
                this.Location = new Point(workArea.Width - this.Width, this.Location.Y); //指定窗体显示在右下角
                this.ControlBox = false;
                topmostWindow(true);
            }
            else
            {
                //1、在屏幕的右下角显示窗体
                //这个区域不包括任务栏的
                Rectangle workArea = Screen.GetWorkingArea(this);
                //这个区域包括任务栏，就是屏幕显示的物理范围
                //Rectangle screenArea = Screen.GetBounds(this);
                this.Size = new Size(defaultWidth, defaultHeight);
                this.Location = new Point(workArea.Width - this.Width, this.Location.Y); //指定窗体显示在右下角
                this.ControlBox = true;
                
            }
        }

        public void topmostWindow(bool enable)
        {
            this.checkBoxTopMost.Checked = enable;
            this.TopMost = enable;
        }

        public void displayTransparentMode(bool enable)
        {
            this.checkBoxTransparent.Checked = enable;
            if (enable)
            {
                this.Opacity = 0.8;
            }
            else
            {
                this.Opacity = 1.0;
            }
        }

        public void displayFloatBorderEnable(bool enable)
        {
            this.checkBoxFloatBorder.Checked = enable;
            displayFloatScreenBorder(enable);
        }

        public void previewItem(ListViewItem item)
        {
            ClipDataItem data=null;
            if (item != null)
            {
                data = item.Tag as ClipDataItem;
            }
            new ClipboardItemViewForm().preview(this, data);
        }

        private void listViewClipItems_DoubleClick(object sender, EventArgs e)
        {
            copyItem();
        }

        private void checkBoxEnable_CheckedChanged(object sender, EventArgs e)
        {
            listenClipboardChange(this.checkBoxEnable.Checked);
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            copyItem();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            removeItem();
        }

        private void buttonClean_Click(object sender, EventArgs e)
        {
            cleanAllItem();
        }

        private void MainForm_MouseEnter(object sender, EventArgs e)
        {
            displayFloatScreenBorder(false);
        }

        private void MainForm_MouseHover(object sender, EventArgs e)
        {
            displayFloatScreenBorder(false);
        }

        private void MainForm_MouseCaptureChanged(object sender, EventArgs e)
        {
            displayFloatScreenBorder(false);
        }

        private void checkBoxTopMost_CheckedChanged(object sender, EventArgs e)
        {
            topmostWindow(this.checkBoxTopMost.Checked);
        }

        private void MainForm_MouseLeave(object sender, EventArgs e)
        {
            if (this.checkBoxFloatBorder.Checked)
            {
                displayFloatScreenBorder(true);
            }
        }

        private void checkBoxTransparent_CheckedChanged(object sender, EventArgs e)
        {
            displayTransparentMode(this.checkBoxTransparent.Checked);
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            if (this.checkBoxFloatBorder.Checked)
            {
                displayFloatScreenBorder(true);
            }
        }

        private void previewContextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listViewClipItems.SelectedItems.Count == 0)
            {
                previewItem(null);
                return;
            }
            previewItem(this.listViewClipItems.SelectedItems[0]);
        }

        private void copyContextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyItem();
        }

        private void deleteContextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeItem();
        }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            if (!this.ControlBox)
            {
                if (childrenCount <= 0)
                {
                    topmostWindow(true);
                }
            }
            storeWaitProcess();
        }

        public void storeWaitProcess()
        {
            
            FileStream fos=null;
            try
            {
                fos = new FileStream("./clipbord-history-wait-process.data", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fos, waitProcesses);
                fos.Close();
            }
            catch (Exception)
            {
                if (fos != null)
                {
                    fos.Close();
                }
            }
        }

        public void loadWaitProcess()
        {
            FileStream fos=null;
            try
            {
                fos = new FileStream("./clipbord-history-wait-process.data", FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                List<WaitProcessItem> list = bf.Deserialize(fos) as List<WaitProcessItem>;
                if (list != null)
                {
                    this.waitProcesses = list;
                }
                fos.Close();
            }
            catch (Exception)
            {
                if (fos != null)
                {
                    fos.Close();
                }
            }
        }

        private void checkBoxFloatBorder_CheckedChanged(object sender, EventArgs e)
        {
            displayFloatBorderEnable(this.checkBoxFloatBorder.Checked);
        }

        private void ToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            previewItem(null);
        }

        private void commentItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listViewClipItems.SelectedItems.Count == 0)
            {
                return;
            }

            string cmt = InputForm.input(this, "请输入备注", "输入框", "");
            foreach (int idx in this.listViewClipItems.SelectedIndices)
            {
                historys[idx].comment = cmt;
            }
            refreshList();
        }

        public void lockItem()
        {
            if (this.listViewClipItems.SelectedItems.Count == 0)
            {
                return;
            }
            List<ClipDataItem> lockes = new List<ClipDataItem>();
            foreach (int idx in this.listViewClipItems.SelectedIndices)
            {
                historys[idx].locked = true;
                lockes.Add(historys[idx]);
            }
            foreach (ClipDataItem item in lockes)
            {
                historys.Remove(item);
                historys.Insert(0, item);
            }
            refreshList();
        }

        private void addLockItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lockItem();
        }

        public void unlockItem()
        {
            if (this.listViewClipItems.SelectedItems.Count == 0)
            {
                return;
            }

            foreach (int idx in this.listViewClipItems.SelectedIndices)
            {
                historys[idx].locked = false;
            }
            refreshList();
        }
        private void removeLockItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            unlockItem();   
        }

        public void selectAll()
        {
            foreach (ListViewItem item in this.listViewClipItems.Items)
            {
                item.Selected = true;
            }
        }

        public void toggleSelect()
        {
            foreach (ListViewItem item in this.listViewClipItems.Items)
            {
                item.Selected = !item.Selected;
            }
        }

        public void addToWait()
        {
            if (this.listViewClipItems.SelectedItems.Count == 0)
            {
                return;
            }
            foreach (ListViewItem item in this.listViewClipItems.SelectedItems){
                WaitProcessItem wait = new WaitProcessItem();
                wait.beginTime = DateTime.Now;
                wait.endTime = DateTime.Now.AddDays(1);
                wait.priority = "高";
                wait.content = item.SubItems[5].Text;
                waitProcesses.Insert(0, wait);
            }
            new ClipboardItemViewForm().open(this, "wait", null, null);
        }
        public void searchItem()
        {
            string content=null;
            foreach (ListViewItem item in this.listViewClipItems.SelectedItems){
                content = item.SubItems[5].Text;
                break;
            }
            new ClipboardItemViewForm().open(this, "web", "search", content);
        }
        private void listViewClipItems_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.C)
                {
                    copyItem();
                }
                else if (e.KeyCode == Keys.A)
                {
                    selectAll();
                }
                else if (e.KeyCode == Keys.Z)
                {
                    toggleSelect();
                }
                else if (e.KeyCode == Keys.S)
                {
                    lockItem();
                }
                else if (e.KeyCode == Keys.Q)
                {
                    unlockItem();
                }
                else if (e.KeyCode == Keys.R)
                {
                    addToWait();
                }
                else if (e.KeyCode == Keys.W)
                {
                    new ClipboardItemViewForm().open(this, "wait", null, null);
                }
                else if (e.KeyCode == Keys.B)
                {
                    searchItem();
                }
            }
            if (e.Alt)
            {
                if (e.KeyCode == Keys.D)
                {
                    removeItem();
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                removeItem();
            }

        }

        private void searchItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            searchItem();
        }

    }
}
