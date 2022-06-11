using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClipboardHistory
{
    public partial class InputForm : Form
    {
     
        private InputForm()
        {
            InitializeComponent();
        }
        public static string input(Form parent, string tips)
        {
            return input(parent, tips, null, null);
        }
        public static string input(Form parent, string tips, string title)
        {
            return input(parent, tips, title, null);
        }
        
        public static string input(Form parent, string tips, string title, string content)
        {
            if (title == null)
            {
                title = "输入框";
            }
            if (content == null)
            {
                content = "";
            }
            if (tips == null)
            {
                tips = "请输入:";
            }
            InputForm form = new InputForm();
            form.Text = title;
            form.labelTips.Text = tips;
            form.textBoxInputs.Text = content;
            int x=parent.Location.X+(parent.Width/2-form.Width/2);
            int y=parent.Location.Y+(parent.Height/2-form.Height/2);
            form.Location = new Point(x,y);
            form.ShowDialog(parent);
            return form.textBoxInputs.Text;
        }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.textBoxInputs.Text = "";
            this.Close();
        }

        private void textBoxInputs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!e.Control)
                {
                    if (this.checkBoxEnterKeyOk.Checked)
                    {
                        this.Close();
                    }
                }
            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                this.textBoxInputs.SelectAll();
            }
           
        }

        private void textBoxInputs_DoubleClick(object sender, EventArgs e)
        {
            this.textBoxInputs.SelectAll();
        }
    }
}
