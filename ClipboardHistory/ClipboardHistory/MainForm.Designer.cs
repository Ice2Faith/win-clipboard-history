namespace ClipboardHistory
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelControlArea = new System.Windows.Forms.Panel();
            this.tableLayoutPanelControl = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxEnable = new System.Windows.Forms.CheckBox();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonClean = new System.Windows.Forms.Button();
            this.checkBoxTopMost = new System.Windows.Forms.CheckBox();
            this.checkBoxTransparent = new System.Windows.Forms.CheckBox();
            this.checkBoxFloatBorder = new System.Windows.Forms.CheckBox();
            this.panelMainArea = new System.Windows.Forms.Panel();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolTipsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolHelpLine = new System.Windows.Forms.ToolStripStatusLabel();
            this.listViewClipItems = new System.Windows.Forms.ListView();
            this.columnIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnLock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.previewContextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyContextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteContextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.commentItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLockItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeLockItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.searchItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.timerTips = new System.Windows.Forms.Timer(this.components);
            this.panelControlArea.SuspendLayout();
            this.tableLayoutPanelControl.SuspendLayout();
            this.panelMainArea.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.contextMenuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControlArea
            // 
            this.panelControlArea.Controls.Add(this.tableLayoutPanelControl);
            this.panelControlArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlArea.Location = new System.Drawing.Point(0, 0);
            this.panelControlArea.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelControlArea.Name = "panelControlArea";
            this.panelControlArea.Size = new System.Drawing.Size(988, 64);
            this.panelControlArea.TabIndex = 0;
            // 
            // tableLayoutPanelControl
            // 
            this.tableLayoutPanelControl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tableLayoutPanelControl.ColumnCount = 7;
            this.tableLayoutPanelControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanelControl.Controls.Add(this.checkBoxEnable, 0, 0);
            this.tableLayoutPanelControl.Controls.Add(this.buttonCopy, 1, 0);
            this.tableLayoutPanelControl.Controls.Add(this.buttonRemove, 2, 0);
            this.tableLayoutPanelControl.Controls.Add(this.buttonClean, 3, 0);
            this.tableLayoutPanelControl.Controls.Add(this.checkBoxTopMost, 5, 0);
            this.tableLayoutPanelControl.Controls.Add(this.checkBoxTransparent, 6, 0);
            this.tableLayoutPanelControl.Controls.Add(this.checkBoxFloatBorder, 4, 0);
            this.tableLayoutPanelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelControl.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanelControl.Name = "tableLayoutPanelControl";
            this.tableLayoutPanelControl.RowCount = 1;
            this.tableLayoutPanelControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelControl.Size = new System.Drawing.Size(988, 64);
            this.tableLayoutPanelControl.TabIndex = 0;
            // 
            // checkBoxEnable
            // 
            this.checkBoxEnable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxEnable.AutoSize = true;
            this.checkBoxEnable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.checkBoxEnable.Location = new System.Drawing.Point(37, 17);
            this.checkBoxEnable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxEnable.Name = "checkBoxEnable";
            this.checkBoxEnable.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxEnable.Size = new System.Drawing.Size(67, 29);
            this.checkBoxEnable.TabIndex = 0;
            this.checkBoxEnable.Text = "启用";
            this.checkBoxEnable.UseVisualStyleBackColor = false;
            this.checkBoxEnable.CheckedChanged += new System.EventHandler(this.checkBoxEnable_CheckedChanged);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.buttonCopy.CausesValidation = false;
            this.buttonCopy.FlatAppearance.BorderSize = 0;
            this.buttonCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCopy.Location = new System.Drawing.Point(173, 14);
            this.buttonCopy.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCopy.Size = new System.Drawing.Size(76, 35);
            this.buttonCopy.TabIndex = 1;
            this.buttonCopy.Text = "复制";
            this.buttonCopy.UseVisualStyleBackColor = false;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.buttonRemove.FlatAppearance.BorderSize = 0;
            this.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemove.Location = new System.Drawing.Point(314, 14);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonRemove.Size = new System.Drawing.Size(76, 35);
            this.buttonRemove.TabIndex = 2;
            this.buttonRemove.Text = "移除";
            this.buttonRemove.UseVisualStyleBackColor = false;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonClean
            // 
            this.buttonClean.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonClean.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonClean.FlatAppearance.BorderSize = 0;
            this.buttonClean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClean.Location = new System.Drawing.Point(455, 14);
            this.buttonClean.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonClean.Name = "buttonClean";
            this.buttonClean.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonClean.Size = new System.Drawing.Size(76, 35);
            this.buttonClean.TabIndex = 3;
            this.buttonClean.Text = "清空";
            this.buttonClean.UseVisualStyleBackColor = false;
            this.buttonClean.Click += new System.EventHandler(this.buttonClean_Click);
            // 
            // checkBoxTopMost
            // 
            this.checkBoxTopMost.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxTopMost.AutoSize = true;
            this.checkBoxTopMost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.checkBoxTopMost.Location = new System.Drawing.Point(742, 17);
            this.checkBoxTopMost.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxTopMost.Name = "checkBoxTopMost";
            this.checkBoxTopMost.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxTopMost.Size = new System.Drawing.Size(67, 29);
            this.checkBoxTopMost.TabIndex = 6;
            this.checkBoxTopMost.Text = "置顶";
            this.checkBoxTopMost.UseVisualStyleBackColor = false;
            this.checkBoxTopMost.CheckedChanged += new System.EventHandler(this.checkBoxTopMost_CheckedChanged);
            // 
            // checkBoxTransparent
            // 
            this.checkBoxTransparent.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxTransparent.AutoSize = true;
            this.checkBoxTransparent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.checkBoxTransparent.Location = new System.Drawing.Point(876, 17);
            this.checkBoxTransparent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxTransparent.Name = "checkBoxTransparent";
            this.checkBoxTransparent.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxTransparent.Size = new System.Drawing.Size(82, 29);
            this.checkBoxTransparent.TabIndex = 7;
            this.checkBoxTransparent.Text = "半透明";
            this.checkBoxTransparent.UseVisualStyleBackColor = false;
            this.checkBoxTransparent.CheckedChanged += new System.EventHandler(this.checkBoxTransparent_CheckedChanged);
            // 
            // checkBoxFloatBorder
            // 
            this.checkBoxFloatBorder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxFloatBorder.AutoSize = true;
            this.checkBoxFloatBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.checkBoxFloatBorder.Location = new System.Drawing.Point(601, 17);
            this.checkBoxFloatBorder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxFloatBorder.Name = "checkBoxFloatBorder";
            this.checkBoxFloatBorder.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxFloatBorder.Size = new System.Drawing.Size(67, 29);
            this.checkBoxFloatBorder.TabIndex = 8;
            this.checkBoxFloatBorder.Text = "悬浮";
            this.checkBoxFloatBorder.UseVisualStyleBackColor = false;
            this.checkBoxFloatBorder.CheckedChanged += new System.EventHandler(this.checkBoxFloatBorder_CheckedChanged);
            // 
            // panelMainArea
            // 
            this.panelMainArea.Controls.Add(this.statusStripMain);
            this.panelMainArea.Controls.Add(this.listViewClipItems);
            this.panelMainArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainArea.Location = new System.Drawing.Point(0, 64);
            this.panelMainArea.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelMainArea.Name = "panelMainArea";
            this.panelMainArea.Size = new System.Drawing.Size(988, 451);
            this.panelMainArea.TabIndex = 1;
            // 
            // statusStripMain
            // 
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolTipsLabel,
            this.toolHelpLine});
            this.statusStripMain.Location = new System.Drawing.Point(0, 426);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(988, 25);
            this.statusStripMain.TabIndex = 1;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolTipsLabel
            // 
            this.toolTipsLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolTipsLabel.Name = "toolTipsLabel";
            this.toolTipsLabel.Size = new System.Drawing.Size(39, 20);
            this.toolTipsLabel.Text = "就绪";
            // 
            // toolHelpLine
            // 
            this.toolHelpLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolHelpLine.Name = "toolHelpLine";
            this.toolHelpLine.Size = new System.Drawing.Size(69, 20);
            this.toolHelpLine.Text = "操作提示";
            // 
            // listViewClipItems
            // 
            this.listViewClipItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnIndex,
            this.columnType,
            this.columnLock,
            this.columnDate,
            this.columnComment,
            this.columnContent});
            this.listViewClipItems.ContextMenuStrip = this.contextMenuStripMain;
            this.listViewClipItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewClipItems.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewClipItems.FullRowSelect = true;
            this.listViewClipItems.GridLines = true;
            this.listViewClipItems.Location = new System.Drawing.Point(0, 0);
            this.listViewClipItems.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listViewClipItems.Name = "listViewClipItems";
            this.listViewClipItems.Size = new System.Drawing.Size(988, 451);
            this.listViewClipItems.TabIndex = 0;
            this.listViewClipItems.UseCompatibleStateImageBehavior = false;
            this.listViewClipItems.View = System.Windows.Forms.View.Details;
            this.listViewClipItems.DoubleClick += new System.EventHandler(this.listViewClipItems_DoubleClick);
            this.listViewClipItems.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listViewClipItems_KeyUp);
            // 
            // columnIndex
            // 
            this.columnIndex.Text = "序号";
            this.columnIndex.Width = 80;
            // 
            // columnType
            // 
            this.columnType.Text = "类型";
            this.columnType.Width = 100;
            // 
            // columnLock
            // 
            this.columnLock.Text = "锁定";
            // 
            // columnDate
            // 
            this.columnDate.Text = "时间";
            this.columnDate.Width = 120;
            // 
            // columnComment
            // 
            this.columnComment.Text = "备注";
            this.columnComment.Width = 120;
            // 
            // columnContent
            // 
            this.columnContent.Text = "内容";
            this.columnContent.Width = 360;
            // 
            // contextMenuStripMain
            // 
            this.contextMenuStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewContextToolStripMenuItem,
            this.toolStripSeparator1,
            this.copyContextToolStripMenuItem,
            this.toolStripSeparator2,
            this.deleteContextToolStripMenuItem,
            this.toolStripSeparator3,
            this.commentItemToolStripMenuItem,
            this.lockItemToolStripMenuItem,
            this.toolStripSeparator5,
            this.searchItemToolStripMenuItem,
            this.ToolsToolStripMenuItem});
            this.contextMenuStripMain.Name = "contextMenuStripMain";
            this.contextMenuStripMain.Size = new System.Drawing.Size(130, 196);
            // 
            // previewContextToolStripMenuItem
            // 
            this.previewContextToolStripMenuItem.Name = "previewContextToolStripMenuItem";
            this.previewContextToolStripMenuItem.Size = new System.Drawing.Size(129, 24);
            this.previewContextToolStripMenuItem.Text = "预览(&V)";
            this.previewContextToolStripMenuItem.Click += new System.EventHandler(this.previewContextToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(126, 6);
            // 
            // copyContextToolStripMenuItem
            // 
            this.copyContextToolStripMenuItem.Name = "copyContextToolStripMenuItem";
            this.copyContextToolStripMenuItem.Size = new System.Drawing.Size(129, 24);
            this.copyContextToolStripMenuItem.Text = "复制(&C)";
            this.copyContextToolStripMenuItem.Click += new System.EventHandler(this.copyContextToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(126, 6);
            // 
            // deleteContextToolStripMenuItem
            // 
            this.deleteContextToolStripMenuItem.Name = "deleteContextToolStripMenuItem";
            this.deleteContextToolStripMenuItem.Size = new System.Drawing.Size(129, 24);
            this.deleteContextToolStripMenuItem.Text = "移除(&D)";
            this.deleteContextToolStripMenuItem.Click += new System.EventHandler(this.deleteContextToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(126, 6);
            // 
            // commentItemToolStripMenuItem
            // 
            this.commentItemToolStripMenuItem.Name = "commentItemToolStripMenuItem";
            this.commentItemToolStripMenuItem.Size = new System.Drawing.Size(129, 24);
            this.commentItemToolStripMenuItem.Text = "备注";
            this.commentItemToolStripMenuItem.Click += new System.EventHandler(this.commentItemToolStripMenuItem_Click);
            // 
            // lockItemToolStripMenuItem
            // 
            this.lockItemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLockItemToolStripMenuItem,
            this.removeLockItemToolStripMenuItem});
            this.lockItemToolStripMenuItem.Name = "lockItemToolStripMenuItem";
            this.lockItemToolStripMenuItem.Size = new System.Drawing.Size(129, 24);
            this.lockItemToolStripMenuItem.Text = "锁定";
            // 
            // addLockItemToolStripMenuItem
            // 
            this.addLockItemToolStripMenuItem.Name = "addLockItemToolStripMenuItem";
            this.addLockItemToolStripMenuItem.Size = new System.Drawing.Size(114, 26);
            this.addLockItemToolStripMenuItem.Text = "上锁";
            this.addLockItemToolStripMenuItem.Click += new System.EventHandler(this.addLockItemToolStripMenuItem_Click);
            // 
            // removeLockItemToolStripMenuItem
            // 
            this.removeLockItemToolStripMenuItem.Name = "removeLockItemToolStripMenuItem";
            this.removeLockItemToolStripMenuItem.Size = new System.Drawing.Size(114, 26);
            this.removeLockItemToolStripMenuItem.Text = "解锁";
            this.removeLockItemToolStripMenuItem.Click += new System.EventHandler(this.removeLockItemToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(126, 6);
            // 
            // searchItemToolStripMenuItem
            // 
            this.searchItemToolStripMenuItem.Name = "searchItemToolStripMenuItem";
            this.searchItemToolStripMenuItem.Size = new System.Drawing.Size(129, 24);
            this.searchItemToolStripMenuItem.Text = "搜索(&B)";
            this.searchItemToolStripMenuItem.Click += new System.EventHandler(this.searchItemToolStripMenuItem_Click);
            // 
            // ToolsToolStripMenuItem
            // 
            this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            this.ToolsToolStripMenuItem.Size = new System.Drawing.Size(129, 24);
            this.ToolsToolStripMenuItem.Text = "工具(&T)";
            this.ToolsToolStripMenuItem.Click += new System.EventHandler(this.ToolsToolStripMenuItem_Click);
            // 
            // timerMain
            // 
            this.timerMain.Tick += new System.EventHandler(this.timerMain_Tick);
            // 
            // timerTips
            // 
            this.timerTips.Tick += new System.EventHandler(this.timerTips_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 515);
            this.Controls.Add(this.panelMainArea);
            this.Controls.Add(this.panelControlArea);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.Text = "历史剪切板";
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseCaptureChanged += new System.EventHandler(this.MainForm_MouseCaptureChanged);
            this.MouseEnter += new System.EventHandler(this.MainForm_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.MainForm_MouseLeave);
            this.MouseHover += new System.EventHandler(this.MainForm_MouseHover);
            this.panelControlArea.ResumeLayout(false);
            this.tableLayoutPanelControl.ResumeLayout(false);
            this.tableLayoutPanelControl.PerformLayout();
            this.panelMainArea.ResumeLayout(false);
            this.panelMainArea.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.contextMenuStripMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControlArea;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelControl;
        private System.Windows.Forms.CheckBox checkBoxEnable;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonClean;
        private System.Windows.Forms.Panel panelMainArea;
        private System.Windows.Forms.ListView listViewClipItems;
        private System.Windows.Forms.ColumnHeader columnIndex;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ColumnHeader columnDate;
        private System.Windows.Forms.ColumnHeader columnContent;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolTipsLabel;
        private System.Windows.Forms.CheckBox checkBoxTopMost;
        private System.Windows.Forms.CheckBox checkBoxTransparent;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem previewContextToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem copyContextToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem deleteContextToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolHelpLine;
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.CheckBox checkBoxFloatBorder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnComment;
        private System.Windows.Forms.ColumnHeader columnLock;
        private System.Windows.Forms.ToolStripMenuItem commentItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lockItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addLockItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeLockItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.Timer timerTips;
        private System.Windows.Forms.ToolStripMenuItem searchItemToolStripMenuItem;
    }
}

