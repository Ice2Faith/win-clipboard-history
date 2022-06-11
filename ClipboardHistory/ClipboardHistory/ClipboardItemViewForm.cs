using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Net;

namespace ClipboardHistory
{
    public partial class ClipboardItemViewForm : Form
    {
        private Image img;
        private string text;
        private bool isFirstWebViewShow = true;
        private MainForm form;
        public ClipboardItemViewForm()
        {
            InitializeComponent();
            initWebView();
            initWaitProcessTab();
        }

        public void initWebView()
        {
            this.webBrowserView.WebBrowserShortcutsEnabled = true;
            this.webBrowserView.AllowNavigation = true;
            this.webBrowserView.AllowWebBrowserDrop = true;
            this.webBrowserView.CausesValidation = true;
            this.webBrowserView.ScriptErrorsSuppressed = true;
            this.webBrowserView.ScrollBarsEnabled = true;

        }

        public void initWaitProcessTab()
        {
            this.tabControl.TabPages.Remove(this.tabPageWaitProcessEdit);
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (this.tabControl.SelectedTab == this.tabPageWebView)
            {
                if (isFirstWebViewShow)
                {
                    baiduWebView();
                    isFirstWebViewShow = false;
                }
            }
            if (this.tabControl.SelectedTab == this.tabPageWaitProcess)
            {
                refreshWaitProcessList();
            }
        }

        public void webViewUrl(string url)
        {
            this.urlTextBoxtoolStripTextBox.Text = url;
            this.webBrowserView.Url = new Uri(url);
        }

        public void open(Form parent, string tab, string action, object arg)
        {
            this.form = parent as MainForm;
            this.form.childrenCount++;
            this.Show(parent);

            if (tab == "text")
            {
                this.tabControl.SelectedTab = this.tabPageText;
            }
            else if (tab == "image")
            {
                this.tabControl.SelectedTab = this.tabPageViewImage;
            }
            else if (tab == "web")
            {
                this.tabControl.SelectedTab = this.tabPageWebView;
                if (action == "search")
                {
                    if (arg != null)
                    {
                        string url = "http://www.baidu.com/s?wd=" + arg;
                        webViewUrl(url);
                    }
                }
            }
            else if (tab == "wait")
            {
                this.tabControl.SelectedTab = this.tabPageWaitProcess;
            }
            
        }

        public void preview(Form parent, ClipDataItem obj)
        {
            if (obj != null)
            {
                img = showImage(obj);
                text = showText(obj);
                if (img != null)
                {
                    // C# 会自动缩放，直到最大化，因此不用再计算一次了
                    this.Size = new Size(img.Size.Width + 5, img.Size.Height + 20);
                    this.tabControl.SelectedTab = this.tabPageViewImage;
                }
                else
                {
                    this.tabControl.SelectedTab = this.tabPageText;
                }
            }
            this.form = parent as MainForm;
            this.form.childrenCount++;
            this.Show(parent);
        }

        public Image showImage(ClipDataItem obj)
        {
            Image img = null;
            string type = obj.type;
            object data = obj.data;
            if (type == DataFormats.Bitmap)
            {
                img = data as Image;
                this.pictureBoxViewImage.Image = img;
            }
            else if (type == DataFormats.Dib)
            {
                img = data as Image;
                if (img == null)
                {
                    try
                    {
                        MemoryStream ms = data as MemoryStream;
                        ms.Position = 0;
                        img = Image.FromStream(ms);
                        ms.Position = 0;
                    }
                    catch (Exception)
                    {

                    }
                }
                this.pictureBoxViewImage.Image = img;
            }
            else if (type == DataFormats.Tiff)
            {
                img = data as Image;
                this.pictureBoxViewImage.Image = img;
            }
            return img;
        }

        public string showText(ClipDataItem obj)
        {
            this.tabPageText.Show();
            string type = obj.type;
            string content = "";
            object data = obj.data;
            if (type == DataFormats.Text)
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
                object typ = data.GetType();
                string[] col = data as string[];
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

            this.textBoxViewText.Text = content;

            return content;
        }

        private void SaveAsInFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filter = "所有文件|*.*";
            string fname = "clip";
            if (img != null)
            {
                filter = "png文件|*.png|jpg图片|*.jpg|gif图片|*.gif|bmp位图文件|*.bmp|所有文件|*.*";
                fname = "clip.png";
            }
            else
            {
                filter = "text文件|*.txt|所有文件|*.*";
                fname = "clip.txt";
            }
            this.saveFileDialogMain.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.saveFileDialogMain.Filter = filter;
            this.saveFileDialogMain.FileName = fname;

            DialogResult rs = this.saveFileDialogMain.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                string filename = this.saveFileDialogMain.FileName;
                if (filename != null && filename.Length > 0)
                {
                    int idx = filename.LastIndexOf(".");
                    string suffix = "";
                    if (idx >= 0)
                    {
                        suffix = filename.Substring(idx);
                    }
                    if (img != null)
                    {
                        suffix = suffix.ToLower();
                        ImageFormat fmt = ImageFormat.Png;
                        if (".png" == suffix)
                        {
                            fmt = ImageFormat.Png;
                        }
                        else if (".jpg" == suffix)
                        {
                            fmt = ImageFormat.Jpeg;
                        }
                        else if (".gif" == suffix)
                        {
                            fmt = ImageFormat.Gif;
                        }
                        else if (".bmp" == suffix)
                        {
                            fmt = ImageFormat.Bmp;
                        }
                        img.Save(filename, fmt);
                    }
                    else
                    {
                        FileStream fos = new FileStream(filename, FileMode.Create);
                        StreamWriter writer = new StreamWriter(fos, Encoding.Default);
                        writer.Write(text);
                        writer.Close();
                        fos.Close();
                    }
                }
            }
        }

        //////////////////////////////////////////////////////

        public void refreshText(bool checkTab)
        {
            if (text == null)
            {
                text = "";
            }
            this.textBoxViewText.Text = text;
            if (checkTab)
            {
                this.tabControl.SelectedTab = this.tabPageText;
            }
        }

        public void refreshImage(bool checkTab)
        {
            if (img != null)
            {
                this.pictureBoxViewImage.Image = img;
            }
            if (checkTab)
            {
                this.tabControl.SelectedTab = this.tabPageViewImage;
            }
        }

        private void loadFileInFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filter = "text文件|*.txt|png文件|*.png|jpg图片|*.jpg|gif图片|*.gif|bmp位图文件|*.bmp|所有文件|*.*";
            this.openFileDialogMain.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.openFileDialogMain.Filter = filter;
            DialogResult rs = this.openFileDialogMain.ShowDialog(this);
            if (rs == DialogResult.OK)
            {
                string filename = this.openFileDialogMain.FileName;
                if (filename != null && filename.Length > 0)
                {
                    this.textBoxViewText.Text = filename;
                    int idx = filename.LastIndexOf(".");
                    string suffix = "";
                    if (idx >= 0)
                    {
                        suffix = filename.Substring(idx);
                    }
                    suffix = suffix.ToLower();
                    if (suffix == ".png" || suffix == ".jpg" || suffix == ".gif" || suffix == ".jpeg" || suffix == ".bmp" || suffix == ".ico")
                    {
                        try
                        {
                            img = Image.FromFile(filename);
                            refreshImage(true);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            FileStream stream = new FileStream(filename, FileMode.Open);
                            try
                            {
                                StreamReader reader = new StreamReader(stream, Encoding.Default);
                                text = reader.ReadToEnd();
                                reader.Close();
                                refreshText(true);
                            }
                            catch (Exception)
                            {
                            }
                            stream.Close();
                        }
                        catch (Exception)
                        {

                        }
                    }

                }
            }
        }

        private void copyInFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img != null)
            {
                Clipboard.SetImage(img);
            }
            else if (text != null)
            {
                Clipboard.SetText(text);
            }
        }

        private void trimInTrimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (text == null)
            {
                text = "";
            }
            text = text.Trim();
            refreshText(false);
        }

        private void trimLeftInTrimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (text == null)
            {
                text = "";
            }
            text = text.TrimStart();
            refreshText(false);
        }

        private void trimRightInTrimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (text == null)
            {
                text = "";
            }
            text = text.TrimEnd();
            refreshText(false);
        }

        public bool isSpaceCh(char ch)
        {
            return ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r' || ch == '\v';
        }

        public bool isUpperCh(char ch)
        {
            return ch >= 'A' && ch <= 'Z';
        }

        public bool isLowerCh(char ch)
        {
            return ch >= 'a' && ch <= 'z';
        }

        public bool isNamingCh(char ch)
        {
            return (ch == '_') || (ch == '$') || (ch == '-') || (ch >= '0' && ch <= '9') || isLowerCh(ch) || isUpperCh(ch);
        }

        public bool isNamingSplitCh(char ch)
        {
            return ch == '_' || ch == '$' || ch == '-';
        }

        public delegate string wordProcess(string word);

        public string wordsProcessText(string str, wordProcess processor)
        {
            string ret = "";

            int i = 0;
            while (i < str.Length)
            {
                if (isNamingCh(str[i]))
                {
                    string word = "";
                    int j = 0;
                    while ((i + j < str.Length) && isNamingCh(str[i + j]))
                    {
                        word += str[i + j];
                        j++;
                    }
                    i += j;
                    ret += processor(word);
                }
                else
                {
                    ret += str[i];
                    i++;
                }

            }

            return ret;
        }


        public string capitalText(string str)
        {
            return wordsProcessText(str, (word) =>
            {
                return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
            });
        }

        public string pascalText(string str)
        {
            return wordsProcessText(str, (word) =>
            {
                string ret = "";
                int i = 0;
                while (i < word.Length)
                {
                    int j = 0;
                    string part = "";
                    part += word[i];
                    while ((i + 1 + j < word.Length) && !isNamingSplitCh(word[i + 1 + j]) && !isUpperCh(word[i + 1 + j]))
                    {
                        part += word[i + 1 + j];
                        j++;
                    }
                    while ((i + 1 + j < word.Length) && isNamingSplitCh(word[i + 1 + j]))
                    {
                        j++;
                    }
                    i += j + 1;

                    ret += part.Substring(0, 1).ToUpper() + part.Substring(1);
                }
                return ret;
            });
        }

        public string camelText(string str)
        {
            return wordsProcessText(str, (word) =>
            {
                string ret = "";
                int i = 0;
                bool first = true;
                while (i < word.Length)
                {
                    int j = 0;
                    string part = "";
                    part += word[i];
                    while ((i + 1 + j < word.Length) && !isNamingSplitCh(word[i + 1 + j]) && !isUpperCh(word[i + 1 + j]))
                    {
                        part += word[i + 1 + j];
                        j++;
                    }
                    while ((i + 1 + j < word.Length) && isNamingSplitCh(word[i + 1 + j]))
                    {
                        j++;
                    }
                    i += j + 1;
                    if (first)
                    {
                        ret += part.Substring(0, 1).ToLower() + part.Substring(1);
                        first = false;
                    }
                    else
                    {
                        ret += part.Substring(0, 1).ToUpper() + part.Substring(1);
                    }
                }
                return ret;
            });
        }

        public string underscoreText(string str)
        {
            return wordsProcessText(str, (word) =>
            {
                string ret = "";
                int i = 0;
                bool first = true;
                while (i < word.Length)
                {
                    int j = 0;
                    string part = "";
                    part += word[i];
                    while ((i + 1 + j < word.Length) && !isNamingSplitCh(word[i + 1 + j]) && !isUpperCh(word[i + 1 + j]))
                    {
                        part += word[i + 1 + j];
                        j++;
                    }
                    while ((i + 1 + j < word.Length) && isNamingSplitCh(word[i + 1 + j]))
                    {
                        j++;
                    }
                    i += j + 1;
                    if (!first)
                    {
                        ret += "_";
                    }
                    ret += part.ToLower();
                    first = false;
                }
                return ret;
            });
        }

        public string firstUpperText(string str)
        {
            return wordsProcessText(str, (word) =>
            {
                return word.Substring(0, 1).ToUpper() + word.Substring(1);
            });
        }

        public string firstLowerText(string str)
        {
            return wordsProcessText(str, (word) =>
            {
                return word.Substring(0, 1).ToLower() + word.Substring(1);
            });
        }

        private void capitalInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = capitalText(text);
            refreshText(false);
        }

        private void pascalInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = pascalText(text);
            refreshText(false);
        }

        private void camelInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = camelText(text);
            refreshText(false);
        }

        private void underscoreInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = underscoreText(text);
            refreshText(false);
        }

        private void firstUpperInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = firstUpperText(text);
            refreshText(false);
        }

        private void firstLowerInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = firstLowerText(text);
            refreshText(false);
        }

        private void upperInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = text.ToUpper();
            refreshText(false);
        }

        private void lowerInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = text.ToLower();
            refreshText(false);
        }

        public string removeEmptyLineText(string str)
        {
            string ret = "";
            int i = 0;
            while (i < str.Length)
            {
                if (str[i] == '\n')
                {
                    int j = 0;
                    while ((i + j) < str.Length && isSpaceCh(str[i + j]))
                    {
                        j++;
                    }
                    while (str[i + j] != '\n')
                    {
                        j--;
                    }
                    i += j + 1;
                    ret += "\r\n";

                }
                else
                {
                    ret += str[i];
                    i++;
                }
            }

            return ret;
        }
        private void removeEmptyLineInLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = removeEmptyLineText(text);
            refreshText(false);
        }

        public string sortByLine(string str, bool asc)
        {
            string[] arr = str.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < arr.Length; i++)
            {
                bool swap = false;
                for (int j = 0; j < arr.Length - 1; j++)
                {
                    if ((arr[j].CompareTo(arr[j + 1]) > 0) == asc)
                    {
                        swap = true;
                        string tmp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = tmp;
                    }
                }
                if (!swap)
                {
                    break;
                }
            }
            string ret = "";
            bool first = true;
            for (int i = 0; i < arr.Length; i++)
            {
                if (!first)
                {
                    ret += "\r\n";
                }
                ret += arr[i];
                first = false;
            }
            return ret;
        }

        private void orderAscInLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = sortByLine(text, true);
            refreshText(false);
        }

        private void orderDescInLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = sortByLine(text, false);
            refreshText(false);
        }

        public string shuffleByLine(string str, bool asc)
        {
            Random rand = new Random();
            string[] arr = str.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = arr.Length - 1; i > 0; i--)
            {
                string tmp = arr[i];
                int nidx = rand.Next(i);
                arr[i] = arr[nidx];
                arr[nidx] = tmp;
            }
            string ret = "";
            bool first = true;
            for (int i = 0; i < arr.Length; i++)
            {
                if (!first)
                {
                    ret += "\r\n";
                }
                ret += arr[i];
                first = false;
            }
            return ret;
        }
        private void orderShuffleInLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = shuffleByLine(text, false);
            refreshText(false);
        }

        public string tab2spaceText(string str, int spaceCount)
        {
            string rps = "";
            for (int j = 0; j < spaceCount; j++)
            {
                rps += " ";
            }
            string ret = "";
            int i = 0;
            while (i < str.Length)
            {
                if (str[i] == '\t')
                {
                    ret += rps;
                    i++;
                }
                else
                {
                    ret += str[i];
                    i++;
                }
            }
            return ret;
        }
        private void tab2space4InFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = tab2spaceText(text, 4);
            refreshText(false);
        }
        public string space2tabText(string str, int spaceCount)
        {
            string ret = "";

            int i = 0;
            while (i < str.Length)
            {
                if (str[i] == ' ')
                {
                    int j = 0;
                    while (j < spaceCount && (i + j) < str.Length && str[i + j] == ' ')
                    {
                        j++;
                    }
                    int cnt = j / spaceCount;
                    int lcnt = j % spaceCount;
                    for (int m = 0; m < cnt; m++)
                    {
                        ret += "\t";
                    }
                    for (int m = 0; m < lcnt; m++)
                    {
                        ret += " ";
                    }
                    i += j;
                }
                else
                {
                    ret += str[i];
                    i++;
                }
            }

            return ret;
        }
        private void space2tab4InFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = space2tabText(text, 4);
            refreshText(false);
        }

        public string mergespaceText(string str)
        {
            string ret = "";
            int i = 0;
            while (i < str.Length)
            {
                if (isSpaceCh(str[i]))
                {
                    int j = 0;
                    bool includeLine = false;
                    while ((i + j) < str.Length && isSpaceCh(str[i + j]))
                    {
                        if (str[i + j] == '\n')
                        {
                            includeLine = true;
                        }
                        j++;
                    }

                    i += j;
                    if (includeLine)
                    {
                        ret += "\r\n";
                    }
                    else
                    {
                        ret += " ";
                    }
                }
                else
                {
                    ret += str[i];
                    i++;
                }
            }
            return ret;
        }

        private void mergespaceInFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = mergespaceText(text);
            refreshText(false);
        }

        public string removeClangSingleLineCommentText(string str)
        {
            string ret = "";

            int i = 0;
            while (i < str.Length)
            {
                if (str[i] == '/')
                {
                    if ((i + 1) < str.Length)
                    {
                        if (str[i + 1] == '/')
                        {
                            i += 2;
                            while (i < str.Length && str[i] != '\n')
                            {
                                i++;
                            }
                            ret += "\r\n";
                        }
                        else
                        {
                            ret += str[i];
                            i++;
                        }
                    }
                }
                else
                {
                    ret += str[i];
                    i++;
                }
            }
            return ret;
        }

        private void removeClangSingleLineCommentInCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = removeClangSingleLineCommentText(text);
            refreshText(false);
        }

        public string removeClangMultiLineCommentText(string str)
        {
            string ret = "";

            int i = 0;
            while (i < str.Length)
            {
                if (str[i] == '/')
                {
                    if ((i + 1) < str.Length)
                    {
                        if (str[i + 1] == '*')
                        {
                            i += 2;
                            while (i < str.Length)
                            {
                                if (str[i] == '*')
                                {
                                    if ((i + 1) < str.Length)
                                    {
                                        if (str[i + 1] == '/')
                                        {
                                            i += 2;
                                            ret += "\r\n";
                                            break;
                                        }
                                    }
                                }
                                i++;
                            }
                        }
                        else
                        {
                            ret += str[i];
                            i++;
                        }
                    }
                }
                else
                {
                    ret += str[i];
                    i++;
                }
            }
            return ret;
        }

        private void removeClangMultiLineCommentInCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = removeClangMultiLineCommentText(text);
            refreshText(false);
        }

        public string removePyLangSingleLineCommentText(string str)
        {
            string ret = "";

            int i = 0;
            while (i < str.Length)
            {
                if (str[i] == '#')
                {
                    while (i < str.Length && str[i] != '\n')
                    {
                        i++;
                    }
                    ret += "\r\n";
                }
                else
                {
                    ret += str[i];
                    i++;
                }
            }

            return ret;
        }

        private void removePyLangSingleLineCommentInCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = removePyLangSingleLineCommentText(text);
            refreshText(false);
        }

        public string removeSqlLangSingleLineCommentText(string str)
        {
            string ret = "";

            int i = 0;
            while (i < str.Length)
            {
                if (str[i] == '-')
                {
                    if ((i + 1) < str.Length)
                    {
                        if (str[i + 1] == '-')
                        {
                            if ((i + 2) < str.Length)
                            {
                                if (isSpaceCh(str[i + 2]))
                                {
                                    i += 3;
                                    while (i < str.Length && str[i] != '\n')
                                    {
                                        i++;
                                    }
                                    ret += "\r\n";
                                }
                                else
                                {
                                    ret += str[i];
                                    ret += str[i + 2];
                                    i += 2;
                                }
                            }
                        }
                        else
                        {
                            ret += str[i];
                            i++;
                        }
                    }
                }
                else
                {
                    ret += str[i];
                    i++;
                }
            }
            return ret;
        }
        private void removeSqlLangSingleLineCommentInCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = removeSqlLangSingleLineCommentText(text);
            refreshText(false);
        }

        public string toBase64Text(string str, Encoding encode)
        {
            byte[] bytes = encode.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }


        public string formBase64Text(string str, Encoding encode)
        {
            if (str == null || str == "")
            {
                return str;
            }
            byte[] outputb = Convert.FromBase64String(str);
            return encode.GetString(outputb);
        }

        private void tobase64InCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = toBase64Text(text, Encoding.UTF8);
            refreshText(false);
        }

        private void formbase64InCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = formBase64Text(text, Encoding.UTF8);
            refreshText(false);
        }

        public string urlencodeText(string str, Encoding encode)
        {
            return HttpUtility.UrlEncode(str, encode);
        }


        public string urldecodeText(string str, Encoding encode)
        {
            return HttpUtility.UrlDecode(str, encode);
        }

        private void urlencodeInCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = urlencodeText(text, Encoding.UTF8);
            refreshText(false);
        }

        private void urldecodeInCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = urldecodeText(text, Encoding.UTF8);
            refreshText(false);
        }

        /// <summary>
        /// Unicode编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string EnUnicodeText(string str)
        {
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    strResult.Append("\\u");
                    strResult.Append(((int)str[i]).ToString("x"));
                }
            }
            return strResult.ToString();
        }

        /// <summary>
        /// Unicode解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DeUnicodeText(string str)
        {
            //最直接的方法Regex.Unescape(str);
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{1,4})");
            return reg.Replace(str, delegate(Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        }

        private void UcodeEncodeInCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = EnUnicodeText(text);
            refreshText(false);
        }

        private void UcodeDecodeInCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            text = DeUnicodeText(text);
            refreshText(false);
        }

        string number2Hex(double num, int baseNum = 16, int decimalNum = 4)
        {
            string hex = "";
            if (baseNum < 2 || baseNum > 16 || decimalNum < 0)
            {
                return hex;
            }


            string map = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string temp = "";
            int otc = (int)num;
            double flo = num - otc;
            int retlen = 0;
            int len = 0;
            while (otc != 0)
            {
                int pnum = otc % baseNum;
                temp += map[pnum];
                len++;
                otc = (int)otc / baseNum;
            }
            int tlen = len - 1;
            if (tlen <= 0)
            {
                hex += '0';
                retlen++;
            }

            while (tlen >= 0)
            {
                hex += temp[tlen];
                retlen++;
                tlen--;
            }
            hex += '.';
            retlen++;
            int dotIndex = retlen;
            int i = 0;
            while (flo != 0.0 && i < decimalNum)
            {
                int pnum = (int)(flo * baseNum);
                hex += map[pnum];
                retlen++;
                flo = flo * baseNum - pnum;
                i++;
            }
            if (dotIndex == retlen)
            {
                hex += '0';
                retlen++;
            }

            return hex;
        }

        int getNumCharValue(char ch, int baseNum = 10)
        {
            string map = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (ch >= 'a' && ch <= 'z')
            {
                ch = (char)((ch - 'a') + 'A');
            }
            int i = 0;
            while (i < map.Length && i < baseNum)
            {
                if (ch == map[i])
                    return i;
                i++;
            }
            return -1;
        }
        bool isNumberChar(char ch, int baseNum = 10)
        {
            string map = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (ch >= 'a' && ch <= 'z')
            {
                ch = (char)((ch - 'a') + 'A');
            }
            int i = 0;
            while (i < map.Length && i < baseNum)
            {
                if (ch == map[i])
                    return true;
                i++;
            }
            return false;
        }
        double hex2Number(string str, int baseNum = 10)
        {
            double ret = 0;
            if (isNumberChar(str[0], baseNum) == false)
            {
                return 0;
            }
            int i = 0;
            while (i < str.Length && isNumberChar(str[i], baseNum))
            {
                ret *= baseNum;
                ret += getNumCharValue(str[i], baseNum);
                i++;
            }
            if (i < str.Length && str[i] == '.')
            {
                i++;
                double lbit = 1;
                while (i < str.Length && isNumberChar(str[i], baseNum))
                {
                    lbit *= baseNum;
                    ret += getNumCharValue(str[i], baseNum) / lbit;
                    i++;
                }
            }
            return ret;
        }

        private void number2hexInHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string str = InputForm.input(this, "请输入目标进制[2-36]:", "输入框", "16");
            int baseNum = Convert.ToInt32(str);
            if (baseNum < 2)
            {
                baseNum = 2;
            }
            if (baseNum > 36)
            {
                baseNum = 36;
            }
            double dnum = Convert.ToDouble(text);
            text = number2Hex(dnum, baseNum, 4);
            refreshText(false);
        }

        private void hex2numberInHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string str = InputForm.input(this, "请输入源进制[2-36]:", "输入框", "16");
            int baseNum = Convert.ToInt32(str);
            if (baseNum < 2)
            {
                baseNum = 2;
            }
            if (baseNum > 36)
            {
                baseNum = 36;
            }
            text = hex2Number(text, baseNum) + "";
            refreshText(false);
        }


        public string deEscapeText(string str)
        {
            string ret = "";

            int i = 0;
            while (i < str.Length)
            {
                if (str[i] == '\\')
                {
                    if ((i + 1) < str.Length)
                    {
                        i += 1;
                        char pch = str[i];
                        if (pch == 't')
                        {
                            ret += "\t";
                        }
                        else if (pch == 'n')
                        {
                            ret += "\n";
                        }
                        else if (pch == 'r')
                        {
                            ret += "\r";
                        }
                        else if (pch == 'v')
                        {
                            ret += "\v";
                        }
                        else if (pch == '\\')
                        {
                            ret += "\\";
                        }
                        else
                        {
                            ret += "\\";
                            ret += pch;
                        }
                        i += 1;
                    }
                    else
                    {
                        ret += str[i];
                        i++;
                    }
                }
                else
                {
                    ret += str[i];
                    i++;
                }
            }

            return ret;
        }

        public string splitText(string str, string sep)
        {
            string[] arr = str.Split(new string[] { sep }, StringSplitOptions.None);
            string ret = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (i != 0)
                {
                    ret += "\r\n";
                }
                ret += arr[i];
            }
            return ret;
        }

        public string joinText(string str, string sep)
        {
            string[] arr = str.Split(new string[] { "\n" }, StringSplitOptions.None);
            string ret = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (i != 0)
                {
                    ret += sep;
                }
                ret += arr[i];
            }
            return ret;
        }

        private void normalInSplitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string sep = InputForm.input(this, "请输入分隔字符串", "输入框", "");
            text = splitText(text, sep);
            refreshText(false);
        }

        private void escapeInSplitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string sep = InputForm.input(this, "请输入分隔字符串(转义)", "输入框", "");
            sep = deEscapeText(sep);
            text = splitText(text, sep);
            refreshText(false);
        }

        private void normalInJoinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string sep = InputForm.input(this, "请输入连接字符串", "输入框", "");
            text = joinText(text, sep);
            refreshText(false);
        }

        private void escapeInJoinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string sep = InputForm.input(this, "请输入连接字符串(转义)", "输入框", "");
            sep = deEscapeText(sep);
            text = joinText(text, sep);
            refreshText(false);
        }

        private void normalInReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string srcStr = InputForm.input(this, "请输入源字符串", "输入框", "");
            string dstStr = InputForm.input(this, "请输入新字符串", "输入框", "");
            text = text.Replace(srcStr, dstStr);
            refreshText(false);
        }

        private void escapeSrcInReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string srcStr = InputForm.input(this, "请输入源字符串(转义)", "输入框", "");
            string dstStr = InputForm.input(this, "请输入新字符串", "输入框", "");
            srcStr = deEscapeText(srcStr);
            text = text.Replace(srcStr, dstStr);
            refreshText(false);
        }

        private void escapeDstInReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string srcStr = InputForm.input(this, "请输入源字符串", "输入框", "");
            string dstStr = InputForm.input(this, "请输入新字符串(转义)", "输入框", "");
            dstStr = deEscapeText(dstStr);
            text = text.Replace(srcStr, dstStr);
            refreshText(false);
        }

        private void escapeAllInReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string srcStr = InputForm.input(this, "请输入源字符串(转义)", "输入框", "");
            string dstStr = InputForm.input(this, "请输入新字符串(转义)", "输入框", "");
            srcStr = deEscapeText(srcStr);
            dstStr = deEscapeText(dstStr);
            text = text.Replace(srcStr, dstStr);
            refreshText(false);
        }

        public void webEnterInputUrl()
        {
            string text = this.urlTextBoxtoolStripTextBox.Text.Trim();
            if (text.IndexOf("://") < 0)
            {
                text = "http://" + text;
            }
            webViewUrl(text);
        }
        private void webUrltoolStripMenuItem_Click(object sender, EventArgs e)
        {
            webEnterInputUrl();
        }

        public void baiduWebView()
        {
            string url = "http://www.baidu.com/";
            webViewUrl(url);
        }
        private void baiduInWebSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            baiduWebView();
        }

        private void backWebToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.webBrowserView.GoBack();
        }

        private void forwardWebToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.webBrowserView.GoForward();
        }

        private void urlTextBoxtoolStripTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.Enter)
            {
                webEnterInputUrl();
            }
        }

        private void webBrowserView_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void webBrowserView_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //将所有的链接的目标，指向本窗体
            foreach (HtmlElement archor in this.webBrowserView.Document.Links)
            {
                archor.SetAttribute("target", "_self");
            }

            //将所有的FORM的提交目标，指向本窗体
            foreach (HtmlElement form in this.webBrowserView.Document.Forms)
            {
                form.SetAttribute("target", "_self");
            }

        }

        private void sinaInWebSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "http://www.sina.com.cn/";
            webViewUrl(url);
        }

        private void youdaoTranslateInWebSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://fanyi.youdao.com/";
            webViewUrl(url);
        }

        private void googleTranslateInWebSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://translate.google.cn/";
            webViewUrl(url);
        }

        private void webBrowserView_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            this.urlTextBoxtoolStripTextBox.Text = e.Url.ToString();
        }

        private void weatherCnInWebSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "http://www.weather.com.cn/";
            webViewUrl(url);
        }

        /// <summary>
        /// ///////////////////////////////////////////////////
        /// </summary>
        private bool waitEditMode = false;
        private int editIndex = -1;
        public void refreshWaitProcessList()
        {
            this.tabControl.TabPages.Remove(this.tabPageWaitProcessEdit);
            List<WaitProcessItem> list = form.waitProcesses;
            this.listViewWaitProcess.Items.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                WaitProcessItem data = list[i];

                ListViewItem item = new ListViewItem();
                item.Tag = data;

                item.Text = (i + 1) + "";

                item.SubItems.Add(data.priority);

                item.SubItems.Add(String.Format("{0:HH:mm:ss MM-dd}", data.beginTime));

                item.SubItems.Add(String.Format("{0:HH:mm:ss MM-dd}", data.endTime));

                item.SubItems.Add(data.content);

                this.listViewWaitProcess.Items.Add(item);
            }

        }

        public void editWaitProcess(int idx)
        {
            if (idx >= 0)
            {
                this.waitEditMode = true;
                this.editIndex = this.listViewWaitProcess.SelectedIndices[0];
                WaitProcessItem item = form.waitProcesses[this.editIndex];
                this.comboBoxPriority.SelectedItem = item.priority;
                this.dateTimePickerBeginTime.Value = item.beginTime;
                this.dateTimePickerEndTime.Value = item.endTime;
                this.textBoxContent.Text = item.content;
            }
            else
            {
                this.waitEditMode = false;
                this.editIndex = -1;
                this.comboBoxPriority.SelectedIndex = 0;
                this.dateTimePickerBeginTime.Value = DateTime.Now;
                this.dateTimePickerEndTime.Value = DateTime.Now.AddDays(1);
                this.textBoxContent.Text = "";
            }
            this.tabControl.TabPages.Remove(this.tabPageWaitProcess);
            this.tabControl.TabPages.Add(this.tabPageWaitProcessEdit);
            this.tabControl.SelectedTab = this.tabPageWaitProcessEdit;
        }

        private void listViewWaitProcess_DoubleClick(object sender, EventArgs e)
        {
            if (this.listViewWaitProcess.SelectedItems.Count <= 0)
            {
                return;
            }
            int idx = this.listViewWaitProcess.SelectedIndices[0];
            editWaitProcess(idx);
        }

        private void addInWaitProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editWaitProcess(-1);
        }

        private void editInWaitProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listViewWaitProcess.SelectedItems.Count <= 0)
            {
                return;
            }
            int idx = this.listViewWaitProcess.SelectedIndices[0];
            editWaitProcess(idx);
        }

        public void deleteItem()
        {
            if (this.listViewWaitProcess.SelectedItems.Count <= 0)
            {
                return;
            }
            foreach (ListViewItem item in this.listViewWaitProcess.SelectedItems)
            {
                form.waitProcesses.Remove(item.Tag as WaitProcessItem);
            }
            refreshWaitProcessList();
        }
        private void deleteInWaitProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteItem();
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            if (this.waitEditMode)
            {
                form.waitProcesses[editIndex].priority = this.comboBoxPriority.SelectedItem as string;
                form.waitProcesses[editIndex].beginTime = this.dateTimePickerBeginTime.Value;
                form.waitProcesses[editIndex].endTime = this.dateTimePickerEndTime.Value;
                form.waitProcesses[editIndex].content = this.textBoxContent.Text;
            }
            else
            {
                WaitProcessItem item = new WaitProcessItem();
                item.priority = this.comboBoxPriority.SelectedItem as string;
                item.beginTime = this.dateTimePickerBeginTime.Value;
                item.endTime = this.dateTimePickerEndTime.Value;
                item.content = this.textBoxContent.Text;

                form.waitProcesses.Insert(0, item);
            }
            this.waitEditMode = false;
            this.editIndex = -1;
            this.tabControl.TabPages.Remove(this.tabPageWaitProcessEdit);
            this.tabControl.TabPages.Add(this.tabPageWaitProcess);
            this.tabControl.SelectedTab = this.tabPageWaitProcess;
            refreshWaitProcessList();
        }

        /// <summary>
        /// ///////////////////////////////////////////////////
        /// </summary>
        public void pickColorMode(bool enable)
        {
            if (enable)
            {
                this.pictureBoxViewImage.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pictureBoxViewImage.Cursor = Cursors.Hand;
                this.pickerColorToolStripMenuItem.BackColor = Color.OrangeRed;
            }
            else
            {
                this.pictureBoxViewImage.SizeMode = PictureBoxSizeMode.Zoom;
                this.pictureBoxViewImage.Cursor = Cursors.Default;
                this.pickerColorToolStripMenuItem.BackColor = Color.White;
            }
        }
        public void togglePickerColorMode()
        {
            if (this.pictureBoxViewImage.SizeMode == PictureBoxSizeMode.Zoom)
            {
                pickColorMode(true);
            }
            else
            {
                pickColorMode(false);
            }
        }
        private void pickerColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            togglePickerColorMode();
        }

        public int rgb2gray(int r, int g, int b)
        {
            // Gray = (R^2.2 * 0.2973  + G^2.2  * 0.6274  + B^2.2  * 0.0753)^(1/2.2)
            return (int)Math.Pow((Math.Pow(r, 2.2) * 0.2973) + (Math.Pow(g, 2.2) * 0.6274) + (Math.Pow(b, 2.2) * 0.0753), (1 / 2.2));
        }

        public Color rgbColor2GrayColor(Color c)
        {
            int gy = rgb2gray(c.R, c.G, c.B);
            return Color.FromArgb(c.A, gy, gy, gy);
        }

        public Color pickColor = Color.Transparent;
        private void pictureBoxViewImage_MouseClick(object sender, MouseEventArgs e)
        {
            if (img == null)
            {
                return;
            }
            if (this.pictureBoxViewImage.SizeMode != PictureBoxSizeMode.StretchImage)
            {
                return;
            }
            Bitmap bmp = new Bitmap(img);
            int x = (int)((e.X * 1.0 / this.pictureBoxViewImage.Width) * bmp.Width);
            int y = (int)((e.Y * 1.0 / this.pictureBoxViewImage.Height) * bmp.Height);
            Color color = bmp.GetPixel(x, y);
            pickColor = color;
            int r = color.R;
            int g = color.G;
            int b = color.B;
            int a = color.A;
            string rh = r.ToString("X").PadLeft(2, '0');
            string gh = g.ToString("X").PadLeft(2, '0');
            string bh = b.ToString("X").PadLeft(2, '0');
            string ah = a.ToString("X").PadLeft(2, '0');

            int gy = rgb2gray(r, g, b);
            string gyh = gy.ToString("X").PadLeft(2, '0');

            string rgbaHex = String.Format("rgba=0x{0}{1}{2}{3}", rh, gh, bh, ah);
            string rgba = String.Format("rgba=({0},{1},{2},{3})", r, g, b, a);
            string gray = String.Format("gray={0},0x{1}", gy, gyh);

            this.toolStripTextBoxRgbColorValue.Text = rgba;
            this.toolStripTextBoxHexColorValue.Text = rgbaHex;
            this.toolStripTextBoxGrayColor.Text = gray;
            this.toolStripMenuItemRgbPreview.BackColor = color;
            this.toolStripMenuItemGrayPreview.BackColor = Color.FromArgb(255, gy, gy, gy);

            string hsb = String.Format("hsb=({0},{1},{2})", color.GetHue(), color.GetSaturation(), color.GetBrightness());
            this.toolStripTextBoxHsbValue.Text = hsb;
            pointActionDispatcher(x,y);
        }

       
        public Image img2grayImage(Image img)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(img.Width, img.Height);
            for (int x = 0; x < simg.Width; x++)
            {
                for (int y = 0; y < simg.Height; y++)
                {
                    Color sc = simg.GetPixel(x, y);
                    Color gc = rgbColor2GrayColor(sc);
                    rimg.SetPixel(x, y, gc);
                }
            }

            return rimg;
        }

        private void toGrayInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            Image rimg = img2grayImage(img);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image img2doubleValueImage(Image img)
        {
            int half = 256 / 2;
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(img.Width, img.Height);
            for (int x = 0; x < simg.Width; x++)
            {
                for (int y = 0; y < simg.Height; y++)
                {
                    Color sc = simg.GetPixel(x, y);
                    int gy = rgb2gray(sc.R, sc.G, sc.B);
                    if (gy > half)
                    {
                        gy = 255;
                    }
                    else
                    {
                        gy = 0;
                    }
                    rimg.SetPixel(x, y, Color.FromArgb(sc.A, gy, gy, gy));
                }
            }

            return rimg;
        }

        private void doubleValueInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            Image rimg = img2doubleValueImage(img);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image img2stepGrayValueImage(Image img, int step)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(img.Width, img.Height);
            for (int x = 0; x < simg.Width; x++)
            {
                for (int y = 0; y < simg.Height; y++)
                {
                    Color sc = simg.GetPixel(x, y);
                    int gy = rgb2gray(sc.R, sc.G, sc.B);
                    double rgbRate = gy * 1.0 / 255;
                    int curStep = (int)(rgbRate * step);
                    gy = (int)(curStep * 1.0 / step * 255);
                    rimg.SetPixel(x, y, Color.FromArgb(sc.A, gy, gy, gy));
                }
            }

            return rimg;
        }
        private void grayStepStyleInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            int step = 2;
            string str = InputForm.input(this, "请输入阶数(2<阶数<256)", "输入框", "");
            try
            {
                step = Convert.ToInt32(str);
            }
            catch (Exception)
            {
                step = 2;
            }
            if (step < 2)
            {
                step = 2;
            }
            if (step > 256)
            {
                step = 256;
            }
            Image rimg = img2stepGrayValueImage(img, step);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public int px2pt(int px)
        {
            //7pt=9px
            return (int)(px * (7.0 / 9));
        }
        public Image img2asciiStypePicture(Image img, char[] ascii, bool keepSize, bool keepRgb)
        {
            int px = 12;
            int pt = px2pt(px);

            Font font = new Font("黑体", pt, FontStyle.Regular);
            Brush brush = new SolidBrush(Color.Black);
            Bitmap simg = new Bitmap(img);
            int dwid = img.Width * px;
            int dhei = img.Height * px;
            if (keepSize)
            {
                dwid = img.Width * 2;
                dhei = img.Height * 2;
            }
            Bitmap rimg = new Bitmap(dwid, dhei);
            Graphics g = Graphics.FromImage(rimg);
            g.FillRectangle(new SolidBrush(Color.White), -1, -1, rimg.Width + 1, rimg.Height + 1);
            for (int x = 0; x < rimg.Width / px; x++)
            {
                for (int y = 0; y < rimg.Height / px; y++)
                {
                    int ax = (int)(x * px * 1.0 / rimg.Width * simg.Width);
                    int ay = (int)(y * px * 1.0 / rimg.Height * simg.Height);
                    Color sc = simg.GetPixel(ax, ay);
                    int gy = rgb2gray(sc.R, sc.G, sc.B);
                    double rgbRate = gy * 1.0 / 255;
                    int curStep = (int)(rgbRate * ascii.Length);
                    char ch = ascii[ascii.Length - 1 - curStep];
                    PointF point = new PointF(x * px, y * px);
                    if (keepRgb)
                    {
                        brush = new SolidBrush(sc);
                    }
                    g.DrawString("" + ch, font, brush, point);

                }
            }


            return rimg;
        }

        private void asciiStyleInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            char[] chArr = { ' ', '`', '.', '^', ',', ':', '~', '"', '<', '!', 'c', 't', '+',
		'{', 'i', '7', '?', 'u', '3', '0', 'p', 'w', '4', 'A', '8', 'D', 'X', '%', '#', 'H', 'W', 'M' };

            if (img == null)
            {
                return;
            }
            bool keepSize = true;
            bool keepRgb = true;
            DialogResult rs = MessageBox.Show(this, "是否保持图片大小？", "询问框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            keepSize = (rs == DialogResult.Yes);
            rs = MessageBox.Show(this, "是否保持图片RGB色彩？", "询问框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            keepRgb = (rs == DialogResult.Yes);

            Image rimg = img2asciiStypePicture(img, chArr, keepSize, keepRgb);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image img2stringStypePicture(Image img, string str, bool keepSize)
        {
            int px = 12;
            int pt = px2pt(px);
            int chidx = 0;
            Font font = new Font("黑体", pt, FontStyle.Regular);
            Bitmap simg = new Bitmap(img);
            int dwid = img.Width * px;
            int dhei = img.Height * px;
            if (keepSize)
            {
                dwid = img.Width * 2;
                dhei = img.Height * 2;
            }
            Bitmap rimg = new Bitmap(dwid, dhei);
            Graphics g = Graphics.FromImage(rimg);
            g.FillRectangle(new SolidBrush(Color.White), -1, -1, rimg.Width + 1, rimg.Height + 1);
            for (int y = 0; y < rimg.Height / px; y++)
            {
                for (int x = 0; x < rimg.Width / px; x++)
                {

                    int ax = (int)(x * px * 1.0 / rimg.Width * simg.Width);
                    int ay = (int)(y * px * 1.0 / rimg.Height * simg.Height);
                    Color sc = simg.GetPixel(ax, ay);

                    PointF point = new PointF(x * px, y * px);
                    Brush brush = new SolidBrush(sc);
                    g.DrawString("" + str[chidx], font, brush, point);

                    chidx = (chidx + 1) % str.Length;

                }
            }


            return rimg;
        }
        private void stringStyleInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            bool keepSize = true;
            DialogResult rs = MessageBox.Show(this, "是否保持图片大小？", "询问框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            keepSize = (rs == DialogResult.Yes);
            string str = InputForm.input(this, "请输入填充文字", "输入框", "小可爱");


            Image rimg = img2stringStypePicture(img, str, keepSize);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image resizeImagePicture(Image img, int wid, int hei)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(wid, hei);
            for (int y = 0; y < rimg.Height; y++)
            {
                double dy = (y * 1.0 / rimg.Height) * simg.Height;
                int iy = (int)dy;
                double yrate = dy - iy;
                for (int x = 0; x < rimg.Width; x++)
                {
                    double dx = (x * 1.0 / rimg.Width) * simg.Width;
                    int ix = (int)dx;
                    double xrate = dx - ix;

                    Color pc = simg.GetPixel(ix, iy);
                    int dr = 0;
                    int dg = 0;
                    int db = 0;
                    int da = 0;


                    if (ix + 1 < simg.Width && iy + 1 < simg.Height)
                    {
                        Color rc = simg.GetPixel(ix + 1, iy);
                        Color bc = simg.GetPixel(ix, iy + 1);
                        Color rbc = simg.GetPixel(ix + 1, iy + 1);

                        double arate = (xrate + yrate) / 2;

                        double dr1 = (pc.R * xrate + rc.R * (1.0 - xrate));
                        double dg1 = (pc.G * xrate + rc.G * (1.0 - xrate));
                        double db1 = (pc.B * xrate + rc.B * (1.0 - xrate));
                        double da1 = (pc.A * xrate + rc.A * (1.0 - xrate));

                        double dr2 = (pc.R * yrate + bc.R * (1.0 - yrate));
                        double dg2 = (pc.G * yrate + bc.G * (1.0 - yrate));
                        double db2 = (pc.B * yrate + bc.B * (1.0 - yrate));
                        double da2 = (pc.A * yrate + bc.A * (1.0 - yrate));

                        double dr3 = (pc.R * arate + rbc.R * (1.0 - arate));
                        double dg3 = (pc.G * arate + rbc.G * (1.0 - arate));
                        double db3 = (pc.B * arate + rbc.B * (1.0 - arate));
                        double da3 = (pc.A * arate + rbc.A * (1.0 - arate));

                        dr = (int)((dr1 + dr2 + dr3) / 3);
                        dg = (int)((dg1 + dg2 + dg3) / 3);
                        db = (int)((db1 + db2 + db3) / 3);
                        da = (int)((da1 + da2 + da3) / 3);

                    }
                    else if (ix + 1 < simg.Width)
                    {
                        Color rc = simg.GetPixel(ix + 1, iy);
                        dr = (int)(pc.R * xrate + rc.R * (1.0 - xrate));
                        dg = (int)(pc.G * xrate + rc.G * (1.0 - xrate));
                        db = (int)(pc.B * xrate + rc.B * (1.0 - xrate));
                        da = (int)(pc.A * xrate + rc.A * (1.0 - xrate));
                    }
                    else if (iy + 1 < simg.Height)
                    {
                        Color bc = simg.GetPixel(ix, iy + 1);
                        dr = (int)(pc.R * xrate + bc.R * (1.0 - xrate));
                        dg = (int)(pc.G * xrate + bc.G * (1.0 - xrate));
                        db = (int)(pc.B * xrate + bc.B * (1.0 - xrate));
                        da = (int)(pc.A * xrate + bc.A * (1.0 - xrate));

                    }
                    else
                    {
                        dr = pc.R;
                        dg = pc.G;
                        db = pc.B;
                        da = pc.A;
                    }



                    rimg.SetPixel(x, y, Color.FromArgb(da, dr, dg, db));

                }
            }

            return rimg;
        }

        private void sizeInVaryPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            int wid = img.Width;
            int hei = img.Height;
            string sizeStr = InputForm.input(this, "请输入新大小", "输入框", "" + wid + "*" + hei);
            string[] arr = sizeStr.Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length > 0)
            {
                try
                {
                    wid = Convert.ToInt32(arr[0]);
                }
                catch (Exception)
                {

                }
            }
            if (arr.Length > 1)
            {
                try
                {
                    hei = Convert.ToInt32(arr[1]);
                }
                catch (Exception)
                {

                }
            }

            Image rimg = resizeImagePicture(img, wid, hei);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image leftSpinPicture(Image img)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Height, simg.Width);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);
                    int rx = (simg.Height + y) % simg.Height;
                    int ry = (simg.Width - x) % simg.Width;
                    rimg.SetPixel(rx, ry, sc);

                }
            }

            return rimg;
        }

        private void leftSpinInVaryPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }

            Image rimg = leftSpinPicture(img);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }
        public Image rightSpinPicture(Image img)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Height, simg.Width);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);
                    int rx = (simg.Height - y) % simg.Height;
                    int ry = (simg.Width + x) % simg.Width;
                    rimg.SetPixel(rx, ry, sc);

                }
            }

            return rimg;
        }
        private void rightSpinInVaryPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }

            Image rimg = rightSpinPicture(img);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image spin180Picture(Image img)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);
                    int ry = (simg.Height - y) % simg.Height;
                    int rx = (simg.Width - x) % simg.Width;
                    rimg.SetPixel(rx, ry, sc);

                }
            }

            return rimg;
        }

        private void spin180InVaryPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }

            Image rimg = spin180Picture(img);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image horMirrorPicture(Image img)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);
                    int ry = (simg.Height + y) % simg.Height;
                    int rx = (simg.Width - x) % simg.Width;
                    rimg.SetPixel(rx, ry, sc);

                }
            }

            return rimg;
        }

        private void horMirrorInVaryPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }

            Image rimg = horMirrorPicture(img);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image velMirrorPicture(Image img)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);
                    int ry = (simg.Height - y) % simg.Height;
                    int rx = (simg.Width + x) % simg.Width;
                    rimg.SetPixel(rx, ry, sc);

                }
            }

            return rimg;
        }

        private void velMirrorInVaryPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }

            Image rimg = velMirrorPicture(img);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }


        public double cmpColorDiffRate(Color c1, Color c2)
        {
            double rate = (Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B) + Math.Abs(c1.A - c2.A)) / 4.0 / 256.0;
            return rate;
        }
        public Image borderImagePicture(Image img, Color whiteBolor, double wrongRate, bool keepRgbBorder, Color borderColor)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int x = 0; x < img.Width - 1; x++)
            {
                for (int y = 0; y < img.Height - 1; y++)
                {
                    Color rightBottomColor = simg.GetPixel(x + 1, y + 1);
                    Color rightColor = simg.GetPixel(x + 1, y);
                    Color bottomColor = simg.GetPixel(x, y + 1);

                    int diffCount = 3;
                    Color[] diffArr = { rightBottomColor, rightColor, bottomColor };

                    Color currentColor = simg.GetPixel(x, y);

                    bool isBorder = false;
                    for (int i = 0; i < diffCount; i++)
                    {
                        if (wrongRate <= 0)
                        {
                            if (currentColor != diffArr[i])
                            {
                                isBorder = true;
                                break;
                            }
                        }
                        else
                        {
                            double rate = cmpColorDiffRate(currentColor, diffArr[i]);
                            if (rate > wrongRate)
                            {
                                isBorder = true;
                                break;
                            }
                        }
                    }
                    if (isBorder)
                    {
                        if (!keepRgbBorder)
                        {
                            rimg.SetPixel(x, y, borderColor);
                        }
                        else
                        {
                            rimg.SetPixel(x, y, currentColor);
                        }
                    }
                    else
                    {
                        rimg.SetPixel(x, y, whiteBolor);
                        if (x == 1 || y == 1)
                        {
                            rimg.SetPixel(x, y, whiteBolor);
                        }
                    }
                }
            }
            return rimg;
        }
        private void borderStyleInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            bool keepRgb = true;
            double wrongRate = -1;
            DialogResult rs = MessageBox.Show(this, "是否保持边缘RGB色彩？", "询问框", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            keepRgb = (rs == DialogResult.Yes);
            string str = InputForm.input(this, "请输入容错率[0.0-1.0]", "输入框", "-1");
            try
            {
                wrongRate = Convert.ToDouble(str);
            }
            catch (Exception)
            {

            }
            Image rimg = borderImagePicture(img, Color.White, wrongRate, keepRgb, Color.Black);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public void registryAutoBoot(string key, string path)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey.SetValue(key, path);
            registryKey.Close();
        }
        public void unregistryAutoBoot(string key)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey.DeleteValue(key);
            registryKey.Close();
        }
        public void autoBootEnable(bool enable)
        {
            string exec = Application.ExecutablePath;
            string appKey = "clipbordHistory";
            if (enable)
            {
                registryAutoBoot(appKey, exec);
            }
            else
            {
                unregistryAutoBoot(appKey);
            }
        }
        private void enableAutoBootInAutoBootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoBootEnable(true);
        }

        private void disableAutoBootInAutobootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoBootEnable(false);
        }

        private void ClipboardItemViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.childrenCount--;
        }

        public Image enhanceComparePicture(Image img, double rate)
        {
            int half = 256 / 2;
            double hrate = 1.0 + rate;
            double lrate = 1.0 - rate;
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);

                    int a = sc.A;
                    int r = sc.R;
                    int g = sc.G;
                    int b = sc.B;
                    if (a > half)
                    {
                        a = (int)(a * hrate);
                    }
                    else
                    {
                        a = (int)(a * lrate);
                    }

                    if (r > half)
                    {
                        r = (int)(r * hrate);
                    }
                    else
                    {
                        r = (int)(r * lrate);
                    }

                    if (g > half)
                    {
                        g = (int)(g * hrate);
                    }
                    else
                    {
                        g = (int)(g * lrate);
                    }

                    if (b > half)
                    {
                        b = (int)(b * hrate);
                    }
                    else
                    {
                        b = (int)(b * lrate);
                    }

                    if (a > 255)
                    {
                        a = 255;
                    }
                    if (a < 0)
                    {
                        a = 0;
                    }

                    if (r > 255)
                    {
                        r = 255;
                    }
                    if (r < 0)
                    {
                        r = 0;
                    }

                    if (g > 255)
                    {
                        g = 255;
                    }
                    if (g < 0)
                    {
                        g = 0;
                    }

                    if (b > 255)
                    {
                        b = 255;
                    }
                    if (b < 0)
                    {
                        b = 0;
                    }

                    Color dc = Color.FromArgb(a, r, g, b);
                    rimg.SetPixel(x, y, dc);

                }
            }

            return rimg;
        }
        private void enhanceCompareInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            double rate = 0.01;
            string str = InputForm.input(this, "请输入对比度增强度[0.0-1.0]:", "输入框", "0.05");
            try
            {
                rate = Convert.ToDouble(str);
            }
            catch (Exception)
            {

            }
            Image rimg = enhanceComparePicture(img, rate);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        
        
        public Image enhanceHuePicture(Image img, double rate)
        {
            double hrate = 1.0 + rate;
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);
                    HslColor hc = HslColor.rgbToHsv(sc);

                    double h = hc.h * hrate;
                    hc.h = HslColor.stdHsl(h);

                    Color dc = HslColor.hsvToRgb(hc);
                    rimg.SetPixel(x, y, dc);

                }
            }

            return rimg;
        }

        private void enhanceHueInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            double rate = 0.01;
            string str = InputForm.input(this, "请输入色相增强度[-1.0,1.0]:", "输入框", "0.2");
            try
            {
                rate = Convert.ToDouble(str);
            }
            catch (Exception)
            {

            }
            Image rimg = enhanceHuePicture(img, rate);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image enhanceSaturationPicture(Image img, double rate)
        {
            double hrate = 1.0 + rate;
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);
                    HslColor hc = HslColor.rgbToHsv(sc);

                    double s = hc.s * hrate;
                    hc.s = HslColor.stdHsl(s);

                    Color dc = HslColor.hsvToRgb(hc);
                    rimg.SetPixel(x, y, dc);

                }
            }

            return rimg;
        }
        private void enhanceSaturationInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            double rate = 0.01;
            string str = InputForm.input(this, "请输入饱和度增强度[-1.0,1.0]:", "输入框", "0.2");
            try
            {
                rate = Convert.ToDouble(str);
            }
            catch (Exception)
            {

            }
            Image rimg = enhanceSaturationPicture(img, rate);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image enhanceBrightnessPicture(Image img, double rate)
        {
            double hrate = 1.0 + rate;
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);
                    HslColor hc = HslColor.rgbToHsv(sc);

                    double v = hc.l * hrate;
                    hc.l = HslColor.stdHsl(v);

                    Color dc = HslColor.hsvToRgb(hc);
                    rimg.SetPixel(x, y, dc);

                }
            }

            return rimg;
        }
        private void enhanceBrightnessInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            double rate = 0.01;
            string str = InputForm.input(this, "请输入亮度增强度[-1.0,1.0]:", "输入框", "0.2");
            try
            {
                rate = Convert.ToDouble(str);
            }
            catch (Exception)
            {

            }
            Image rimg = enhanceBrightnessPicture(img, rate);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }
        public void selectAll()
        {
            foreach (ListViewItem item in this.listViewWaitProcess.Items)
            {
                item.Selected = true;
            }
        }

        public void toggleSelect()
        {
            foreach (ListViewItem item in this.listViewWaitProcess.Items)
            {
                item.Selected = !item.Selected;
            }
        }
        private void listViewWaitProcess_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                
                if (e.KeyCode == Keys.A)
                {
                    selectAll();
                }
                else if (e.KeyCode == Keys.Z)
                {
                    toggleSelect();
                }
                
            }
            if (e.Alt)
            {
                if (e.KeyCode == Keys.D)
                {
                    deleteItem();
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                deleteItem();
            }
        }

        public Image reverseColorPicture(Image img)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);

                    Color dc = Color.FromArgb(sc.A, 255 - sc.R, 255 - sc.G, 255 - sc.B);
                    rimg.SetPixel(x, y, dc);

                }
            }

            return rimg;
        }

        private void aitiColorInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            Image rimg = reverseColorPicture(img);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image removeColorPicture(Image img,Color tarColor,double wrongRate,Color repColor)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);

                    bool isTarget = false;
                    if (wrongRate <= 0)
                    {
                        if (sc == tarColor)
                        {
                            isTarget = true;
                            
                        }
                    }
                    else
                    {
                        double rate = cmpColorDiffRate(sc, tarColor);
                        if (rate <= wrongRate)
                        {
                            isTarget = true;
                            
                        }
                    }

                    Color dc=sc;
                    if (isTarget)
                    {
                        dc = repColor;
                    }
                    
                    rimg.SetPixel(x, y, dc);

                }
            }

            return rimg;
        }

        private void colorRemoveInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            Color repColor = Color.Transparent;
            double wrongRate = -1;

            string srate = InputForm.input(this, "请输入容错率：[0.0,1.0]", "颜色容错率", "" + wrongRate);
            try
            {
                wrongRate = Convert.ToDouble(srate);
            }
            catch (Exception)
            {

            }

            Image rimg = removeColorPicture(img, pickColor, wrongRate, repColor);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        private void replaceColorInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            Color repColor = Color.Transparent;
            double wrongRate = -1;

            this.colorDialogPicker.Color = repColor;
            this.colorDialogPicker.AllowFullOpen = true;
            this.colorDialogPicker.AnyColor = true;
            this.colorDialogPicker.FullOpen = true;

            DialogResult rs = this.colorDialogPicker.ShowDialog(this);
            repColor = this.colorDialogPicker.Color;

            string srate = InputForm.input(this, "请输入容错率：[0.0,1.0]", "颜色容错率", "" + wrongRate);
            try
            {
                wrongRate = Convert.ToDouble(srate);
            }
            catch (Exception)
            {

            }

            Image rimg = removeColorPicture(img, pickColor, wrongRate, repColor);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        private void fullColorReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            Color repColor = Color.Transparent;
            double wrongRate = -1;

            string scolor=InputForm.input(this, "请输入8位16进制颜色值ARGB：例如：FF00FF00或FF,00,FF,00或#FF00FF00或0xFF00FF00表示纯绿色", "颜色输入", "00,00,00,00");
            scolor=scolor.Trim();
            scolor = scolor.Replace("#", "");
            scolor = scolor.Replace("0x", "");
            scolor = scolor.Replace("0X", "");
            scolor=scolor.Replace(",", "");
            scolor = scolor.Replace("，", "");
            if (scolor.Length == 8)
            {
                string sa = scolor.Substring(0, 2);
                string sr = scolor.Substring(2, 2);
                string sg = scolor.Substring(4, 2);
                string sb = scolor.Substring(6, 2);

                try
                {
                    int a = Convert.ToInt32(sa, 16);
                    int r = Convert.ToInt32(sr, 16);
                    int g = Convert.ToInt32(sg, 16);
                    int b = Convert.ToInt32(sb, 16);
                    repColor = Color.FromArgb(a, r, g, b);
                }
                catch (Exception)
                {

                }
            }

            string srate = InputForm.input(this, "请输入容错率：[0.0,1.0]", "颜色容错率", "" + wrongRate);
            try
            {
                wrongRate = Convert.ToDouble(srate);
            }
            catch (Exception)
            {

            }

            Image rimg = removeColorPicture(img, pickColor, wrongRate, repColor);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public string templateLineText(string format, string[] lines, bool lineSplit, string split)
        {
            string ret = "";
            foreach (string line in lines)
            {
                string[] args = { line };
                if (lineSplit)
                {
                    args = line.Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);
                }
                ret += String.Format(format, args);
            }
            return ret;
        }

        private void lineTemplateInTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string[] lines = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string format = InputForm.input(this,"请输入格式化串：{0}代表一行作为参数,{{和}}表示{}本身", "格式化串输入框", "{0}");
            text = templateLineText(format,lines,false,null);
            refreshText(false);
        }

        private void splitLineTemplateInTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string[] lines = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string format = InputForm.input(this, "请输入格式化串：{0}，{1}，{i}等代表一行被分隔之后的第i个参数,{{和}}表示{}本身", "格式化串输入框", "{0}");
            string split = InputForm.input(this, "请输入行参数分隔符：", "分隔符输入框", " ");
            text = templateLineText(format,lines,true,split);
            refreshText(false);
        }

        private void pascalAfterInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string[] lines = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            text = "";
            foreach (string line in lines)
            {
                text += line + " " + pascalText(line)+"\r\n";
            }

            refreshText(false);
        }

        private void camelAfterInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string[] lines = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            text = "";
            foreach (string line in lines)
            {
                text += line + " " + camelText(line) + "\r\n";
            }

            refreshText(false);
        }

        private void unserscoreAfterInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string[] lines = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            text = "";
            foreach (string line in lines)
            {
                text += line + " " + underscoreText(line) + "\r\n";
            }

            refreshText(false);
        }

        private void upperAfterInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string[] lines = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            text = "";
            foreach (string line in lines)
            {
                text += line + " " + line.ToUpper() + "\r\n";
            }

            refreshText(false);
        }

        private void lowerAfterInCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string[] lines = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            text = "";
            foreach (string line in lines)
            {
                text += line + " " + line.ToLower() + "\r\n";
            }

            refreshText(false);
        }
        private string pointAction = null;

        public void pointActionDispatcher(int x, int y)
        {
            if (pointAction == "seedRemoveColor")
            {
                actionSeedRemoveColor(x, y,false);
                pickColorMode(false);
                pointAction = null;
            }
            else if (pointAction == "fullColorSeedRemoveColor")
            {
                actionSeedRemoveColor(x, y, true);
                pickColorMode(false);
                pointAction = null;
            }
            else if (pointAction == "rectangleStylePicture")
            {
                rectanglePoints[rectangleLen] = new Point(x, y);
                rectangleLen++;
                if (rectangleLen == rectanglePoints.Length)
                {
                    rectangleLen = 0;
                    pickColorMode(false);
                    pointAction = null;
                    actionRectangleStylePicture();
                }
            }
            else if (pointAction == "cutoffPicture")
            {
                cutoffPoints[cutoffLen] = new Point(x, y);
                cutoffLen++;
                if (cutoffLen == cutoffPoints.Length)
                {
                    cutoffLen = 0;
                    pickColorMode(false);
                    pointAction = null;
                    actionCutoffPicture();
                }
            }

        }
        public Image seedReplaceColorPicture(Image img, int x, int y, double wrongRate, Color repColor)
        {
            Color maskColor = Color.Red;
            Bitmap dmp = new Bitmap(img);
            Bitmap mmp = new Bitmap(dmp.Width,dmp.Height);
            Color cmpColor=dmp.GetPixel(x,y);
            List<Point> points = new List<Point>();
            Point p = new Point(x, y);
            // 八个方向遍历
            int[][] steps ={
                              new int[]{-1,-1},
                              new int[]{0,-1},
                              new int[]{1,-1},
                              new int[]{-1,0},
                              new int[]{1,0},
                              new int[]{-1,1},
                              new int[]{0,1},
                              new int[]{1,1}
                          };
            points.Add(p);

           
            while (points.Count>0)
            {
                Point cur = points[0];
                
                mmp.SetPixel(cur.X, cur.Y, maskColor);

                for (int j = 0; j < steps.Length; j++)
                {
                    int nx = cur.X + steps[j][0];
                    int ny = cur.Y + steps[j][1];

                    if (nx < 0 || nx >= dmp.Width || ny < 0 || ny >= dmp.Height)
                    {
                        continue;
                    }
                    if (mmp.GetPixel(nx, ny) == maskColor)
                    {
                        continue;
                    }
                    Color tarColor = dmp.GetPixel(nx, ny);
                    bool isTarget = false;
                    if (wrongRate <= 0)
                    {
                        if (cmpColor == tarColor)
                        {
                            isTarget = true;

                        }
                    }
                    else
                    {
                        double rate = cmpColorDiffRate(cmpColor, tarColor);
                        if (rate <= wrongRate)
                        {
                            isTarget = true;
                        }
                    }

                    if (isTarget)
                    {
                        points.Add(new Point(nx, ny));
                        dmp.SetPixel(nx, ny, repColor);
                    }
                    mmp.SetPixel(nx, ny, maskColor);
                }

                points.RemoveAt(0);
                
            }

            return dmp;
        }

        public void actionSeedRemoveColor(int x, int y,bool fullColor)
        {
            if (img == null)
            {
                return;
            }
            Color repColor = Color.Transparent;
            double wrongRate = -1;

            if (!fullColor)
            {
                this.colorDialogPicker.Color = repColor;
                this.colorDialogPicker.AllowFullOpen = true;
                this.colorDialogPicker.AnyColor = true;
                this.colorDialogPicker.FullOpen = true;

                DialogResult rs = this.colorDialogPicker.ShowDialog(this);
                repColor = this.colorDialogPicker.Color;
            }
            else
            {
                string scolor = InputForm.input(this, "请输入8位16进制颜色值ARGB：例如：FF00FF00或FF,00,FF,00或#FF00FF00或0xFF00FF00表示纯绿色", "颜色输入", "00,00,00,00");
                scolor = scolor.Trim();
                scolor = scolor.Replace("#", "");
                scolor = scolor.Replace("0x", "");
                scolor = scolor.Replace("0X", "");
                scolor = scolor.Replace(",", "");
                scolor = scolor.Replace("，", "");
                if (scolor.Length == 8)
                {
                    string sa = scolor.Substring(0, 2);
                    string sr = scolor.Substring(2, 2);
                    string sg = scolor.Substring(4, 2);
                    string sb = scolor.Substring(6, 2);

                    try
                    {
                        int a = Convert.ToInt32(sa, 16);
                        int r = Convert.ToInt32(sr, 16);
                        int g = Convert.ToInt32(sg, 16);
                        int b = Convert.ToInt32(sb, 16);
                        repColor = Color.FromArgb(a, r, g, b);
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            string srate = InputForm.input(this, "请输入容错率：[0.0,1.0]", "颜色容错率", "" + wrongRate);
            try
            {
                wrongRate = Convert.ToDouble(srate);
            }
            catch (Exception)
            {

            }

            Image rimg = seedReplaceColorPicture(img, x, y, wrongRate, repColor);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }
        private void seedRemoveColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pickColorMode(true);
            pointAction = "seedRemoveColor";
        }

        private void fullColorSeedRemoveColorInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pickColorMode(true);
            pointAction = "fullColorSeedRemoveColor";
        }

        private void changePictureBoxBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.colorDialogPicker.AllowFullOpen = true;
            this.colorDialogPicker.AnyColor = true;
            this.colorDialogPicker.Color = this.changePictureBoxBackgroundColorToolStripMenuItem.BackColor;
            this.colorDialogPicker.FullOpen = true;
            this.colorDialogPicker.ShowDialog(this);

            this.changePictureBoxBackgroundColorToolStripMenuItem.BackColor = this.colorDialogPicker.Color;
            this.pictureBoxViewImage.BackColor = this.colorDialogPicker.Color;
        }

        private void textBoxViewText_DoubleClick(object sender, EventArgs e)
        {
            this.textBoxViewText.SelectAll();
        }

        private void textBoxViewText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                this.textBoxViewText.SelectAll();
            }
        }

        private void textBoxContent_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                this.textBoxContent.SelectAll();
            }
        }

        private void textBoxContent_DoubleClick(object sender, EventArgs e)
        {
            this.textBoxContent.SelectAll();
        }
        public Image img2stepRgbaValueImage(Image img, int step)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(img.Width, img.Height);
            for (int x = 0; x < simg.Width; x++)
            {
                for (int y = 0; y < simg.Height; y++)
                {
                    Color sc = simg.GetPixel(x, y);

                    int ca = sc.A;
                    int cr = sc.R;
                    int cg = sc.G;
                    int cb = sc.B;

                    double rgbRate = ca * 1.0 / 255;
                    int curStep = (int)(rgbRate * step);
                    ca = (int)(curStep * 1.0 / step * 255);

                    rgbRate = cr * 1.0 / 255;
                    curStep = (int)(rgbRate * step);
                    cr = (int)(curStep * 1.0 / step * 255);

                    rgbRate = cg * 1.0 / 255;
                    curStep = (int)(rgbRate * step);
                    cg = (int)(curStep * 1.0 / step * 255);

                    rgbRate = cb * 1.0 / 255;
                    curStep = (int)(rgbRate * step);
                    cb = (int)(curStep * 1.0 / step * 255);

                    rimg.SetPixel(x, y, Color.FromArgb(ca, cr, cg, cb));
                }
            }

            return rimg;
        }
        private void rgbaStepStyleInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            int step = 2;
            string str = InputForm.input(this, "请输入阶数(2<阶数<256)", "输入框", "");
            try
            {
                step = Convert.ToInt32(str);
            }
            catch (Exception)
            {
                step = 2;
            }
            if (step < 2)
            {
                step = 2;
            }
            if (step > 256)
            {
                step = 256;
            }
            Image rimg = img2stepRgbaValueImage(img, step);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public void actionRectangleStylePicture()
        {
            if (img == null)
            {
                return;
            }
            int nwid = img.Width;
            int nhei = img.Height;
            DialogResult rs=MessageBox.Show(this, "是否保留不规则矩形大小？不保留则按照图片大小","大小询问", MessageBoxButtons.YesNo);
            
            if (rs == DialogResult.Yes)
            {
                int minx = Math.Min(rectanglePoints[0].X, Math.Min(rectanglePoints[1].X, Math.Min(rectanglePoints[2].X, rectanglePoints[3].X)));
                int miny = Math.Min(rectanglePoints[0].Y, Math.Min(rectanglePoints[1].Y, Math.Min(rectanglePoints[2].Y, rectanglePoints[3].Y)));
                int maxx = Math.Max(rectanglePoints[0].X, Math.Max(rectanglePoints[1].X, Math.Max(rectanglePoints[2].X, rectanglePoints[3].X)));
                int maxy = Math.Max(rectanglePoints[0].Y, Math.Max(rectanglePoints[1].Y, Math.Max(rectanglePoints[2].Y, rectanglePoints[3].Y)));
                nwid = Math.Abs(maxx - minx);
                nhei = Math.Abs(maxy - miny);
            }
            Image rimg = rectangleNormalizePicture(img, rectanglePoints,nwid,nhei);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        /// <summary>
        /// 校正变形矩形
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="points">顺时针环绕的变形矩形四个顶点</param>
        /// <param name="nwid">新宽度</param>
        /// <param name="nhei">新高度</param>
        /// <returns></returns>
        public Image rectangleNormalizePicture(Image img, Point[] points,int nwid,int nhei)
        {
            if (nwid <= 0)
            {
                nwid = img.Width;
            }
            if (nhei <= 0)
            {
                nhei = img.Height;
            }
            Bitmap simg = new Bitmap(img);
            Bitmap dimg = new Bitmap(nwid,nhei);

            Point leftTop = points[0];
            Point rightTop = points[1];
            Point rightDown = points[2];
            Point leftDown = points[3];

            for (int i = 0; i < nhei; i++)
            {
                for (int j = 0; j < nwid; j++)
                {
                    double ratex = j * 1.0 / nwid;
                    double ratey = i * 1.0 / nhei;
                    PointF toptp = GetLineRatePoint(leftTop, rightTop, ratex);
                    PointF downtp = GetLineRatePoint(leftDown, rightDown, ratex);
                    PointF target = GetLineRatePoint(toptp, downtp, ratey);

                    Color color = simg.GetPixel((int)target.X, (int)target.Y);


                    dimg.SetPixel(j, i, color);
                }
            }

            return dimg;
        }

        double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2.0) + Math.Pow(p2.Y - p1.Y, 2.0)); ;
        }
        PointF GetLineRatePoint(PointF sp, PointF ep, double rate)
        {
            PointF ret = new PointF(0, 0);
            float dx = ep.X - sp.X;
            float dy = ep.Y - sp.Y;
            ret.X = (float)(sp.X + dx * rate);
            ret.Y = (float)(sp.Y + dy * rate);
            return ret;
        }

        private Point[] rectanglePoints = new Point[4];
        private int rectangleLen = 0;
        private void rectangleStyleInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pickColorMode(true);
            rectangleLen = 0;
            pointAction = "rectangleStylePicture";
            MessageBox.Show(this, "请顺时针依次点击变形矩形的四个点", "操作提示");

        }

        private Point[] cutoffPoints = new Point[2];
        private int cutoffLen = 0;
        private void cutoffPictureInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pickColorMode(true);
            cutoffLen = 0;
            pointAction = "cutoffPicture";
            MessageBox.Show(this, "请选择2个点进行裁剪", "操作提示");
        }
        public Image cutoffPicture(Image img, Point[] points)
        {
            int minx = Math.Min(points[0].X, points[1].X);
            int miny = Math.Min(points[0].Y, points[1].Y);
            int maxx = Math.Max(points[0].X, points[1].X);
            int maxy = Math.Max(points[0].Y, points[1].Y);
            int nwid=Math.Abs(maxx-minx);
            int nhei=Math.Abs(maxy-miny);
            Bitmap simg = new Bitmap(img);
            Bitmap dimg = new Bitmap(nwid, nhei);
            for (int x = 0; x < nwid; x++)
            {
                for (int y = 0; y < nhei; y++)
                {
                    int sx = x + minx;
                    int sy = y + miny;
                    Color color = simg.GetPixel(sx, sy);
                    dimg.SetPixel(x, y, color);
                }
            }
                return dimg;
        }
        public void actionCutoffPicture()
        {
            if (img == null)
            {
                return;
            }

            Image rimg = cutoffPicture(img, cutoffPoints);
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public bool matchRegex(string str, string regex)
        {
            Regex reg = new Regex(regex);
            return reg.IsMatch(str);
        }

        public List<string> allMatchedItemsRegex(string str, string regex)
        {
            Regex reg = new Regex(regex);
            MatchCollection col = reg.Matches(str);
            List<string> ret = new List<string>();
            foreach (Match item in col)
            {
                ret.Add(item.Value);
            }
            return ret;
        }

        public string matchAndReplace(string str, string regex, string rep)
        {
            Regex reg = new Regex(regex);
            MatchCollection col = reg.Matches(str);
            string ret = "";
            int lst = 0;
            for (int i = 0; i < col.Count; i++)
            {
                Match item=col[i];
                ret += str.Substring(lst, item.Index - lst);

                string pv=string.Format(rep, item.Value);
                ret += pv;

                lst = item.Index + item.Length;
            }

            if (lst < str.Length)
            {
                ret += str.Substring(lst);
            }

            return ret;
        }

        private void matchInRegexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string regex=InputForm.input(this,"请输入正则表达式","正则输入",".+");
            bool rs=matchRegex(text, regex);

            MessageBox.Show(this, "" + rs, "匹配结果");
        }

        private void matchAllInRegexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string regex = InputForm.input(this, "请输入正则表达式", "正则输入", ".+");
            List<string> list = allMatchedItemsRegex(text, regex);
            string display = "";
            foreach (string line in list)
            {
                display += line + "\r\n";
            }

            InputForm.input(this, "匹配结果：", "正则匹配结果", display);
        }

        private void matchReplaceInRegexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text = this.textBoxViewText.Text;
            if (text == null)
            {
                text = "";
            }
            string regex = InputForm.input(this, "请输入正则表达式", "正则输入", ".+");
            string rep = InputForm.input(this, "请输入替换字符串", "输入", "{0}");

            string display = matchAndReplace(text, regex, rep);

            InputForm.input(this, "匹配结果：", "正则匹配结果", display);
        }

        /// //////////////////////////////////////////////////////////////////////////////////
        public class HttpRequestDto{
            public string baseUrl;
            public string path;
            public string method;
            public List<string> urlParams;
            public List<string> headers;
            public string body;
        }

        public class HttpResponseDto{
            public List<string> headers;
            public string body;
        }

        public class AnyWebHeaderCollection : WebHeaderCollection
        {
            public override void Add(string name, string value)
            {
                AddWithoutValidate(name, value);
            }
        }
        
        private void buttonReqRun_Click(object sender, EventArgs e)
        {
            string baseUrl = this.textBoxReqBaseUrl.Text;
            string path = this.textBoxReqPath.Text;
            string method = this.comboBoxReqMethod.SelectedItem as string;
            string urlParamsStr = this.textBoxReqUrlParams.Text;
            string headerStr = this.textBoxReqHeader.Text;
            string body = this.textBoxReqBody.Text;

            HttpRequestDto req = new HttpRequestDto();
            req.baseUrl = baseUrl;
            req.path = path;
            req.method = method;
            req.body = body;
            List<string> urlParams = new List<string>();
            string[] paramLines = urlParamsStr.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in paramLines)
            {
                string[] pairs = line.Split(new string[] { "=", ":" },2, StringSplitOptions.RemoveEmptyEntries);
                if (pairs.Length == 2)
                {
                    urlParams.Add(pairs[0] + "=" + pairs[1]);
                }
                else if (pairs.Length == 1)
                {
                    urlParams.Add(pairs[0] + "=");
                }
            }
            req.urlParams = urlParams;

            List<string> headers = new List<string>();
            string[] headerLines = headerStr.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in headerLines)
            {
                string[] pairs = line.Split(new string[] { "=", ":" }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (pairs.Length == 2)
                {
                    headers.Add(pairs[0] + "=" + pairs[1]);
                }
                else if (pairs.Length == 1)
                {
                    headers.Add(pairs[0] + "=");
                }
            }
            req.headers = headers;

            HttpResponseDto resp = doHttp(req);

            string respHeaderStr = "";
            List<string> respHeaders = resp.headers;
            foreach (string line in respHeaders)
            {
                respHeaderStr += line + "\r\n";
            }
            this.textBoxRespHeader.Text = respHeaderStr;
            this.textBoxRespBody.Text = resp.body;
        }

    
        public string genUrl(HttpRequestDto request)
        {
            string baseUrl = request.baseUrl;
            string path = request.path;
            List<string> urlParams = request.urlParams;
            baseUrl = baseUrl.Trim();
            if (!baseUrl.Contains("://"))
            {
                baseUrl = "http://"+baseUrl;
            }
            int idx = baseUrl.IndexOf("?");
            string originParamsStr = "";
            if (idx >= 0)
            {
                originParamsStr = baseUrl.Substring(idx + 1);
                baseUrl = baseUrl.Substring(0, idx);
            }
            if (path != null && path != "")
            {
                path = path.Trim();
                if (path.StartsWith("/"))
                {
                    if (baseUrl.EndsWith("/"))
                    {
                        baseUrl += path.Substring(1);
                    }
                    else
                    {
                        baseUrl += path;
                    }
                }
                else
                {
                    if (baseUrl.EndsWith("/"))
                    {
                        baseUrl += path;
                    }
                    else
                    {
                        baseUrl += "/"+path;
                    }
                }
            }

            if (urlParams != null)
            {
                foreach (string line in urlParams)
                {
                    string[] pairs = line.Split(new string[] { "=" }, 2, StringSplitOptions.RemoveEmptyEntries);
                    originParamsStr += "&";
                    if (pairs.Length == 2)
                    {
                        originParamsStr += pairs[0] + "=" + urlencodeText(pairs[1], Encoding.UTF8);
                    }
                    else
                    {
                        originParamsStr += pairs[0] + "=";
                    }
                }
            }

            if (originParamsStr.StartsWith("&"))
            {
                originParamsStr = originParamsStr.Substring(1);
            }

            return baseUrl + "?" + originParamsStr;
            
        }

        public WebHeaderCollection genHeader(HttpRequestDto request)
        {
            AnyWebHeaderCollection col = new AnyWebHeaderCollection();
            foreach (string line in request.headers)
            {
                string[] pairs = line.Split(new string[] { "=" }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (pairs.Length == 2)
                {
                    col.Add(pairs[0], pairs[1]);
                }
                else if (pairs.Length == 1)
                {
                    col.Add(pairs[0], "");
                }
            }
            return col;
        }

        public List<string> resolveHeader(WebHeaderCollection col)
        {
            List<string> ret = new List<string>();
            foreach (string key in col.AllKeys)
            {
                ret.Add(key+"="+col.Get(key));
            }
            return ret;
        }

        public string resolveCharset(WebHeaderCollection col)
        {
            string charset = "UTF-8";
            string contentType = "";

            foreach (string key in col.AllKeys)
            {
                if ("Content-Type".Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    contentType = col.Get(key);
                    break;
                }
            }


            if (contentType != null)
            {
                int idx = contentType.ToLower().IndexOf("charset");
                if (idx >= 0)
                {
                    charset = contentType.Substring(idx + "charset".Length);
                    charset = charset.Trim();
                    idx = charset.IndexOf(";");
                    if (idx >= 0)
                    {
                        charset = charset.Substring(0, idx).Trim();
                    }
                }
            }

            return charset;
        }

        public void applyRequestHeaders(WebHeaderCollection col,HttpWebRequest client)
        {
            foreach (string key in col.AllKeys)
            {
                string pk = key;
                pk = pk.Replace("-", "");
                pk = pk.Replace("_", "");
                pk = pk.ToLower();
                if (pk == "contenttype")
                {
                    client.ContentType = col.Get(key);
                }
                else if (pk == "accept")
                {
                    client.Accept = col.Get(key);
                }
                else if (pk == "connection")
                {
                    client.Connection = col.Get(key);
                }
                else if (pk == "contentlength")
                {
                    client.ContentLength = Convert.ToInt64(col.Get(key));
                }
                else if (pk == "expect")
                {
                    client.Expect = col.Get(key);
                }
                else if (pk == "mediatype")
                {
                    client.MediaType = col.Get(key);
                }
                else if (pk == "referer")
                {
                    client.Referer = col.Get(key);
                }
                else if (pk == "transferencoding")
                {
                    client.TransferEncoding = col.Get(key);
                }
                else if (pk == "useragent")
                {
                    client.UserAgent = col.Get(key);
                }

            }
        }
        public HttpResponseDto doHttp(HttpRequestDto request)
        {
            string url = genUrl(request);
            HttpWebRequest client = HttpWebRequest.Create(url) as HttpWebRequest;
            client.Method = request.method;
            WebHeaderCollection headers=genHeader(request);
            applyRequestHeaders(headers, client);
            if (request.body != null && request.body!="")
            {
                using (Stream sos = client.GetRequestStream())
                {
                    string charset = resolveCharset(headers);
                    StreamWriter writer = new StreamWriter(sos, Encoding.GetEncoding(charset));
                    writer.Write(request.body);
                }
            }

            HttpResponseDto ret = new HttpResponseDto();
            using(HttpWebResponse resp=client.GetResponse() as HttpWebResponse)
            {
                List<string> respHeaders = resolveHeader(resp.Headers);
                ret.headers = respHeaders;
                string charset = resolveCharset(resp.Headers);
                using(Stream sis=resp.GetResponseStream()){
                    StreamReader reader = new StreamReader(sis, Encoding.GetEncoding(charset));
                    string content = reader.ReadToEnd();
                    ret.body = content;
                }
            }
            return ret;
        }

        public Image nearToColor(Image img, Color nc, double rate)
        {
            double srate=1.0-rate;
            double nrate=rate;
            int na=nc.A;
            int nr=nc.R;
            int ng=nc.G;
            int nb=nc.B;
            Bitmap simg = new Bitmap(img);
            Bitmap dimg = new Bitmap(simg.Width, simg.Height);
            for (int x = 0; x < simg.Width; x++)
            {
                for (int y = 0; y < simg.Height; y++)
                {
                    Color sc = simg.GetPixel(x, y);

                    int a = sc.A;
                    int r = sc.R;
                    int g = sc.G;
                    int b = sc.B;

                    a = (int)(a * srate + na * nrate);
                    r = (int)(r * srate + nr * nrate);
                    g = (int)(g * srate + ng * nrate);
                    b = (int)(b * srate + nb * nrate);

                    Color dc = Color.FromArgb(a, r, g, b);

                    dimg.SetPixel(x, y, dc);
                }
            }

            return dimg;
        }

        private void coldHueColorInHueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            double rate=0.01;
            Color dc=Color.FromArgb(255,0,128,255);
            string str=InputForm.input(this,"请输入色调趋近率[0.0-1.0]：","输入框",""+rate);
            try
            {
                rate = Convert.ToDouble(str);
            }
            catch (Exception)
            {

            }
            Image rimg = nearToColor(img, dc, rate); ;
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        private void sunHueInHueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            double rate = 0.01;
            Color dc = Color.FromArgb(255, 255, 128, 0);
            string str = InputForm.input(this, "请输入色调趋近率[0.0-1.0]：", "输入框", "" + rate);
            try
            {
                rate = Convert.ToDouble(str);
            }
            catch (Exception)
            {

            }
            Image rimg = nearToColor(img, dc, rate); ;
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public double distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));
        }

        public int lineSum(int num)
        {
            int ret = 0;
            for (int i = 0; i <= num; i++)
            {
                ret += i;
            }
            return ret;
        }

        public Image blurPicture(Image img, int radius)
        {
            
            int rsum = lineSum(radius);
            Bitmap simg = new Bitmap(img);
            Bitmap dimg = new Bitmap(simg.Width, simg.Height);
            for (int x = 0; x < dimg.Width; x++)
            {
                for (int y = 0; y < dimg.Height; y++)
                {
                    List<int> dises = new List<int>();
                    List<Point> points = new List<Point>();
                    int[] cnts = new int[radius + 1];
                    for (int i = 0; i < radius + 1; i++)
                    {
                        cnts[i] = 0;
                    }
                    for (int i = (0-radius); i <= radius; i++)
                    {
                        if (x + i < 0 || x + i >= dimg.Width)
                        {
                            continue;
                        }
                        for (int j = (0 - radius); j <= radius; j++)
                        {
                            if( y + j < 0 || y + j >= dimg.Height)
                            {
                                continue;
                            }
                            double dis = distance(x, y, x + i, y + j);
                            if (dis > radius)
                            {
                                continue;
                            }
                            int idis = (int)dis;
                            cnts[idis]++;
                            dises.Add(idis);
                            points.Add(new Point(x + i, y + j));
                        }
                    }

                    double da = 0;
                    double dr = 0;
                    double dg = 0;
                    double db = 0;
                    for (int i = 0; i < points.Count; i++)
                    {
                        int dis = dises[i];
                        Point p = points[i];
                        Color c = simg.GetPixel(p.X, p.Y);
                        int ndis = radius - dis;
                        double rate = ndis * 1.0 / rsum;
                        double srate = rate / cnts[dis];
                        da += c.A * srate;
                        dr += c.R * srate;
                        dg += c.G * srate;
                        db += c.B * srate;
                    }

                    if (da > 255)
                    {
                        da = 255;
                    }
                    if (dr > 255)
                    {
                        dr = 255;
                    }
                    if (dg > 255)
                    {
                        dg = 255;
                    }
                    if (db > 255)
                    {
                        db = 255;
                    }

                    Color dc = Color.FromArgb((int)da, (int)dr, (int)dg, (int)db);

                    dimg.SetPixel(x, y, dc);
                }
            }

            return dimg;
        }
        private void blurInPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            int radius = 3;
            string str = InputForm.input(this, "请输入模糊半径：", "输入框", "" + radius);
            try
            {
                radius = Convert.ToInt32(str);
            }
            catch (Exception)
            {

            }
            Image rimg = blurPicture(img, radius); ;
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        private void customHueInHueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            double rate = 0.01;
            Color dc = Color.FromArgb(255, 255, 0, 128);
            this.colorDialogPicker.Color = dc;
            this.colorDialogPicker.AllowFullOpen = true;
            this.colorDialogPicker.AnyColor = true;
            this.colorDialogPicker.FullOpen = true;

            DialogResult rs = this.colorDialogPicker.ShowDialog(this);
            dc = this.colorDialogPicker.Color;
            string str = InputForm.input(this, "请输入色调趋近率[0.0-1.0]：", "输入框", "" + rate);
            try
            {
                rate = Convert.ToDouble(str);
            }
            catch (Exception)
            {

            }
            Image rimg = nearToColor(img, dc, rate); ;
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image lighterDarkPicture(Image img, double rate)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);
                    HslColor hc = HslColor.rgbToHsv(sc);


                    double v = hc.l * (1.0 + (rate * (1.0 - hc.l)));
                    hc.l = HslColor.stdHsl(v);
                    

                    Color dc = HslColor.hsvToRgb(hc);
                    rimg.SetPixel(x, y, dc);

                }
            }

            return rimg;
        }

        private void lighterDarkInAdjustToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            double rate = 0.01;
            string str = InputForm.input(this, "请输入暗部增亮度[0.0-1.0]：", "输入框", "" + rate);
            try
            {
                rate = Convert.ToDouble(str);
            }
            catch (Exception)
            {

            }
            
            Image rimg = lighterDarkPicture(img, rate); ;
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

        public Image darkerLightPicture(Image img, double rate)
        {
            Bitmap simg = new Bitmap(img);
            Bitmap rimg = new Bitmap(simg.Width, simg.Height);
            for (int y = 0; y < simg.Height; y++)
            {
                for (int x = 0; x < simg.Width; x++)
                {
                    Color sc = simg.GetPixel(x, y);
                    HslColor hc = HslColor.rgbToHsv(sc);


                    double v = hc.l * (1.0 - (rate * (hc.l)));
                    hc.l = HslColor.stdHsl(v);


                    Color dc = HslColor.hsvToRgb(hc);
                    rimg.SetPixel(x, y, dc);

                }
            }

            return rimg;
        }

        private void darkerLightInAdjustToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                return;
            }
            double rate = 0.01;
            string str = InputForm.input(this, "请输入亮部降暗度[0.0-1.0]：", "输入框", "" + rate);
            try
            {
                rate = Convert.ToDouble(str);
            }
            catch (Exception)
            {

            }
            
            Image rimg = darkerLightPicture(img, rate); ;
            this.img = rimg;
            this.pictureBoxViewImage.Image = img;
        }

    }

    
}
