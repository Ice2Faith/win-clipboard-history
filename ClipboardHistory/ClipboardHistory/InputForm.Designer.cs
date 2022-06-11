namespace ClipboardHistory
{
    partial class InputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTips = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBoxInputs = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkBoxEnterKeyOk = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(553, 232);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelTips);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(547, 52);
            this.panel1.TabIndex = 1;
            // 
            // labelTips
            // 
            this.labelTips.AutoSize = true;
            this.labelTips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTips.Location = new System.Drawing.Point(0, 0);
            this.labelTips.Name = "labelTips";
            this.labelTips.Size = new System.Drawing.Size(59, 17);
            this.labelTips.TabIndex = 0;
            this.labelTips.Text = "请输入";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBoxInputs);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 61);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(547, 110);
            this.panel2.TabIndex = 3;
            // 
            // textBoxInputs
            // 
            this.textBoxInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInputs.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxInputs.Location = new System.Drawing.Point(0, 0);
            this.textBoxInputs.Multiline = true;
            this.textBoxInputs.Name = "textBoxInputs";
            this.textBoxInputs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxInputs.Size = new System.Drawing.Size(547, 110);
            this.textBoxInputs.TabIndex = 0;
            this.textBoxInputs.DoubleClick += new System.EventHandler(this.textBoxInputs_DoubleClick);
            this.textBoxInputs.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxInputs_KeyUp);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBoxEnterKeyOk);
            this.panel3.Controls.Add(this.buttonCancel);
            this.panel3.Controls.Add(this.buttonOk);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 177);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(3);
            this.panel3.Size = new System.Drawing.Size(547, 52);
            this.panel3.TabIndex = 5;
            // 
            // checkBoxEnterKeyOk
            // 
            this.checkBoxEnterKeyOk.AutoSize = true;
            this.checkBoxEnterKeyOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.checkBoxEnterKeyOk.Checked = true;
            this.checkBoxEnterKeyOk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnterKeyOk.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxEnterKeyOk.FlatAppearance.BorderSize = 0;
            this.checkBoxEnterKeyOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEnterKeyOk.Location = new System.Drawing.Point(3, 3);
            this.checkBoxEnterKeyOk.Name = "checkBoxEnterKeyOk";
            this.checkBoxEnterKeyOk.Size = new System.Drawing.Size(236, 46);
            this.checkBoxEnterKeyOk.TabIndex = 2;
            this.checkBoxEnterKeyOk.Text = "回车确认(Ctrl+Enter换行)";
            this.checkBoxEnterKeyOk.UseVisualStyleBackColor = false;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(394, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 46);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonOk.FlatAppearance.BorderSize = 0;
            this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOk.Location = new System.Drawing.Point(469, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 46);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "确定";
            this.buttonOk.UseVisualStyleBackColor = false;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(553, 232);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel);
            this.MinimizeBox = false;
            this.Name = "InputForm";
            this.Text = "输入框";
            this.tableLayoutPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTips;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBoxInputs;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.CheckBox checkBoxEnterKeyOk;
    }
}