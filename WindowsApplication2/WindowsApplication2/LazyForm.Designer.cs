namespace LazyPPiigg
{
    partial class LazyForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該公開 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LazyForm));
            this.ColorLabel = new System.Windows.Forms.Label();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.Position = new System.Windows.Forms.Label();
            this.MouserTimer = new System.Windows.Forms.Timer(this.components);
            this.LeftAndRightCheckBox = new System.Windows.Forms.CheckBox();
            this.DoubleClickCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.attackCheckBox = new System.Windows.Forms.CheckBox();
            this.Magnifier = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.point = new System.Windows.Forms.PictureBox();
            this.ToolsCheckBox = new System.Windows.Forms.CheckBox();
            this.assistCheckBox = new System.Windows.Forms.CheckBox();
            this.HpLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.specifyTxt = new System.Windows.Forms.Label();
            this.targetTxt = new System.Windows.Forms.Label();
            this.timeTxt = new System.Windows.Forms.Label();
            this.mpTxt = new System.Windows.Forms.Label();
            this.hpTxt = new System.Windows.Forms.Label();
            this.specifyLable = new System.Windows.Forms.Label();
            this.targetLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.mpLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.siegeCheckBox = new System.Windows.Forms.CheckBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconExit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitContextMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.primaryCombo = new System.Windows.Forms.ComboBox();
            this.SetGroup = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.addedCombo = new System.Windows.Forms.ComboBox();
            this.otherCombo = new System.Windows.Forms.ComboBox();
            this.secondaryCombo = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.Magnifier)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.point)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.notifyIconExit.SuspendLayout();
            this.SetGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ColorLabel
            // 
            this.ColorLabel.AutoSize = true;
            this.ColorLabel.Location = new System.Drawing.Point(33, 18);
            this.ColorLabel.Name = "ColorLabel";
            this.ColorLabel.Size = new System.Drawing.Size(0, 12);
            this.ColorLabel.TabIndex = 0;
            // 
            // Timer
            // 
            this.Timer.Interval = 10;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // Position
            // 
            this.Position.AutoSize = true;
            this.Position.Location = new System.Drawing.Point(125, 18);
            this.Position.Name = "Position";
            this.Position.Size = new System.Drawing.Size(0, 12);
            this.Position.TabIndex = 1;
            // 
            // MouserTimer
            // 
            this.MouserTimer.Interval = 500;
            this.MouserTimer.Tick += new System.EventHandler(this.MouserTimer_Tick);
            // 
            // LeftAndRightCheckBox
            // 
            this.LeftAndRightCheckBox.AutoSize = true;
            this.LeftAndRightCheckBox.Location = new System.Drawing.Point(9, 45);
            this.LeftAndRightCheckBox.Name = "LeftAndRightCheckBox";
            this.LeftAndRightCheckBox.Size = new System.Drawing.Size(84, 16);
            this.LeftAndRightCheckBox.TabIndex = 3;
            this.LeftAndRightCheckBox.Text = "左右鍵連點";
            this.LeftAndRightCheckBox.UseVisualStyleBackColor = true;
            // 
            // DoubleClickCheckBox
            // 
            this.DoubleClickCheckBox.AutoSize = true;
            this.DoubleClickCheckBox.Location = new System.Drawing.Point(119, 23);
            this.DoubleClickCheckBox.Name = "DoubleClickCheckBox";
            this.DoubleClickCheckBox.Size = new System.Drawing.Size(72, 16);
            this.DoubleClickCheckBox.TabIndex = 4;
            this.DoubleClickCheckBox.Text = "滑鼠連點\r\n";
            this.DoubleClickCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "顏色";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "座標";
            // 
            // attackCheckBox
            // 
            this.attackCheckBox.AutoSize = true;
            this.attackCheckBox.Location = new System.Drawing.Point(119, 45);
            this.attackCheckBox.Name = "attackCheckBox";
            this.attackCheckBox.Size = new System.Drawing.Size(48, 16);
            this.attackCheckBox.TabIndex = 7;
            this.attackCheckBox.Text = "練功";
            this.attackCheckBox.UseVisualStyleBackColor = true;
            this.attackCheckBox.CheckedChanged += new System.EventHandler(this.attackCheckBox_CheckedChanged);
            // 
            // Magnifier
            // 
            this.Magnifier.Location = new System.Drawing.Point(51, 38);
            this.Magnifier.Name = "Magnifier";
            this.Magnifier.Size = new System.Drawing.Size(100, 100);
            this.Magnifier.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Magnifier.TabIndex = 8;
            this.Magnifier.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.point);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Magnifier);
            this.groupBox1.Controls.Add(this.ColorLabel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Position);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(4, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 146);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工具";
            // 
            // point
            // 
            this.point.BackColor = System.Drawing.Color.Black;
            this.point.Location = new System.Drawing.Point(99, 86);
            this.point.Name = "point";
            this.point.Size = new System.Drawing.Size(4, 4);
            this.point.TabIndex = 9;
            this.point.TabStop = false;
            // 
            // ToolsCheckBox
            // 
            this.ToolsCheckBox.AutoSize = true;
            this.ToolsCheckBox.Location = new System.Drawing.Point(9, 23);
            this.ToolsCheckBox.Name = "ToolsCheckBox";
            this.ToolsCheckBox.Size = new System.Drawing.Size(72, 16);
            this.ToolsCheckBox.TabIndex = 10;
            this.ToolsCheckBox.Text = "游標工具";
            this.ToolsCheckBox.UseVisualStyleBackColor = true;
            this.ToolsCheckBox.CheckedChanged += new System.EventHandler(this.Tools_CheckedChanged);
            // 
            // assistCheckBox
            // 
            this.assistCheckBox.AutoSize = true;
            this.assistCheckBox.Location = new System.Drawing.Point(119, 67);
            this.assistCheckBox.Name = "assistCheckBox";
            this.assistCheckBox.Size = new System.Drawing.Size(48, 16);
            this.assistCheckBox.TabIndex = 11;
            this.assistCheckBox.Text = "助功";
            this.assistCheckBox.UseVisualStyleBackColor = true;
            // 
            // HpLabel
            // 
            this.HpLabel.AutoSize = true;
            this.HpLabel.Location = new System.Drawing.Point(7, 22);
            this.HpLabel.Name = "HpLabel";
            this.HpLabel.Size = new System.Drawing.Size(63, 12);
            this.HpLabel.TabIndex = 12;
            this.HpLabel.Text = "血條(Alt+1)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.specifyTxt);
            this.groupBox2.Controls.Add(this.targetTxt);
            this.groupBox2.Controls.Add(this.timeTxt);
            this.groupBox2.Controls.Add(this.mpTxt);
            this.groupBox2.Controls.Add(this.hpTxt);
            this.groupBox2.Controls.Add(this.specifyLable);
            this.groupBox2.Controls.Add(this.targetLabel);
            this.groupBox2.Controls.Add(this.timeLabel);
            this.groupBox2.Controls.Add(this.mpLabel);
            this.groupBox2.Controls.Add(this.HpLabel);
            this.groupBox2.Location = new System.Drawing.Point(223, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(153, 146);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "座標";
            // 
            // specifyTxt
            // 
            this.specifyTxt.AutoSize = true;
            this.specifyTxt.Location = new System.Drawing.Point(84, 122);
            this.specifyTxt.Name = "specifyTxt";
            this.specifyTxt.Size = new System.Drawing.Size(33, 12);
            this.specifyTxt.TabIndex = 21;
            this.specifyTxt.Text = "label7";
            // 
            // targetTxt
            // 
            this.targetTxt.AutoSize = true;
            this.targetTxt.Location = new System.Drawing.Point(84, 97);
            this.targetTxt.Name = "targetTxt";
            this.targetTxt.Size = new System.Drawing.Size(33, 12);
            this.targetTxt.TabIndex = 20;
            this.targetTxt.Text = "label6";
            // 
            // timeTxt
            // 
            this.timeTxt.AutoSize = true;
            this.timeTxt.Location = new System.Drawing.Point(84, 71);
            this.timeTxt.Name = "timeTxt";
            this.timeTxt.Size = new System.Drawing.Size(33, 12);
            this.timeTxt.TabIndex = 19;
            this.timeTxt.Text = "label5";
            // 
            // mpTxt
            // 
            this.mpTxt.AutoSize = true;
            this.mpTxt.Location = new System.Drawing.Point(84, 45);
            this.mpTxt.Name = "mpTxt";
            this.mpTxt.Size = new System.Drawing.Size(33, 12);
            this.mpTxt.TabIndex = 18;
            this.mpTxt.Text = "label4";
            // 
            // hpTxt
            // 
            this.hpTxt.AutoSize = true;
            this.hpTxt.Location = new System.Drawing.Point(84, 22);
            this.hpTxt.Name = "hpTxt";
            this.hpTxt.Size = new System.Drawing.Size(33, 12);
            this.hpTxt.TabIndex = 17;
            this.hpTxt.Text = "label3";
            // 
            // specifyLable
            // 
            this.specifyLable.AutoSize = true;
            this.specifyLable.Location = new System.Drawing.Point(7, 122);
            this.specifyLable.Name = "specifyLable";
            this.specifyLable.Size = new System.Drawing.Size(74, 12);
            this.specifyLable.TabIndex = 16;
            this.specifyLable.Text = "(10,47)(Alt+5)";
            // 
            // targetLabel
            // 
            this.targetLabel.AutoSize = true;
            this.targetLabel.Location = new System.Drawing.Point(6, 97);
            this.targetLabel.Name = "targetLabel";
            this.targetLabel.Size = new System.Drawing.Size(63, 12);
            this.targetLabel.TabIndex = 15;
            this.targetLabel.Text = "目標(Alt+4)";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(6, 71);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(63, 12);
            this.timeLabel.TabIndex = 14;
            this.timeLabel.Text = "時間(Alt+3)";
            // 
            // mpLabel
            // 
            this.mpLabel.AutoSize = true;
            this.mpLabel.Location = new System.Drawing.Point(6, 45);
            this.mpLabel.Name = "mpLabel";
            this.mpLabel.Size = new System.Drawing.Size(63, 12);
            this.mpLabel.TabIndex = 13;
            this.mpLabel.Text = "法力(Alt+2)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.siegeCheckBox);
            this.groupBox3.Controls.Add(this.ToolsCheckBox);
            this.groupBox3.Controls.Add(this.DoubleClickCheckBox);
            this.groupBox3.Controls.Add(this.assistCheckBox);
            this.groupBox3.Controls.Add(this.LeftAndRightCheckBox);
            this.groupBox3.Controls.Add(this.attackCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(8, 164);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(204, 124);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "功能";
            // 
            // siegeCheckBox
            // 
            this.siegeCheckBox.AutoSize = true;
            this.siegeCheckBox.Location = new System.Drawing.Point(9, 67);
            this.siegeCheckBox.Name = "siegeCheckBox";
            this.siegeCheckBox.Size = new System.Drawing.Size(48, 16);
            this.siegeCheckBox.TabIndex = 12;
            this.siegeCheckBox.Text = "攻城";
            this.siegeCheckBox.UseVisualStyleBackColor = true;
            this.siegeCheckBox.CheckedChanged += new System.EventHandler(this.siegeCheckBox_CheckedChanged);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyIconExit;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "❤(｡◕‿◕)ﾉﾟ･✿お♥や♥す♥み♥ー✿ﾟ･ヽ(◕‿◕｡)❤";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // notifyIconExit
            // 
            this.notifyIconExit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitContextMenu});
            this.notifyIconExit.Name = "notifyIconExit";
            this.notifyIconExit.Size = new System.Drawing.Size(111, 26);
            // 
            // exitContextMenu
            // 
            this.exitContextMenu.Name = "exitContextMenu";
            this.exitContextMenu.Size = new System.Drawing.Size(110, 22);
            this.exitContextMenu.Text = "結束(&X)";
            this.exitContextMenu.Click += new System.EventHandler(this.exitContextMenu_Click);
            // 
            // primaryCombo
            // 
            this.primaryCombo.FormattingEnabled = true;
            this.primaryCombo.Items.AddRange(new object[] {
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F11"});
            this.primaryCombo.Location = new System.Drawing.Point(54, 19);
            this.primaryCombo.Name = "primaryCombo";
            this.primaryCombo.Size = new System.Drawing.Size(94, 20);
            this.primaryCombo.TabIndex = 15;
            this.primaryCombo.Text = "F5";
            this.primaryCombo.SelectedIndexChanged += new System.EventHandler(this.primaryCombo_SelectedIndexChanged);
            // 
            // SetGroup
            // 
            this.SetGroup.Controls.Add(this.label6);
            this.SetGroup.Controls.Add(this.label5);
            this.SetGroup.Controls.Add(this.label4);
            this.SetGroup.Controls.Add(this.label3);
            this.SetGroup.Controls.Add(this.addedCombo);
            this.SetGroup.Controls.Add(this.otherCombo);
            this.SetGroup.Controls.Add(this.secondaryCombo);
            this.SetGroup.Controls.Add(this.primaryCombo);
            this.SetGroup.Location = new System.Drawing.Point(218, 164);
            this.SetGroup.Name = "SetGroup";
            this.SetGroup.Size = new System.Drawing.Size(158, 124);
            this.SetGroup.TabIndex = 16;
            this.SetGroup.TabStop = false;
            this.SetGroup.Text = "設定";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 22;
            this.label6.Text = "喝水";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 21;
            this.label5.Text = "其他";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "次要";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "主要";
            // 
            // addedCombo
            // 
            this.addedCombo.FormattingEnabled = true;
            this.addedCombo.Items.AddRange(new object[] {
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F11"});
            this.addedCombo.Location = new System.Drawing.Point(54, 98);
            this.addedCombo.Name = "addedCombo";
            this.addedCombo.Size = new System.Drawing.Size(94, 20);
            this.addedCombo.TabIndex = 18;
            this.addedCombo.Text = "F8";
            this.addedCombo.SelectedIndexChanged += new System.EventHandler(this.addedCombo_SelectedIndexChanged);
            // 
            // otherCombo
            // 
            this.otherCombo.FormattingEnabled = true;
            this.otherCombo.Items.AddRange(new object[] {
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F11"});
            this.otherCombo.Location = new System.Drawing.Point(54, 71);
            this.otherCombo.Name = "otherCombo";
            this.otherCombo.Size = new System.Drawing.Size(94, 20);
            this.otherCombo.TabIndex = 17;
            this.otherCombo.Text = "F7";
            this.otherCombo.SelectedIndexChanged += new System.EventHandler(this.otherCombo_SelectedIndexChanged);
            // 
            // secondaryCombo
            // 
            this.secondaryCombo.FormattingEnabled = true;
            this.secondaryCombo.Items.AddRange(new object[] {
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F11"});
            this.secondaryCombo.Location = new System.Drawing.Point(54, 45);
            this.secondaryCombo.Name = "secondaryCombo";
            this.secondaryCombo.Size = new System.Drawing.Size(94, 20);
            this.secondaryCombo.TabIndex = 16;
            this.secondaryCombo.Text = "F6";
            this.secondaryCombo.SelectedIndexChanged += new System.EventHandler(this.secondaryCombo_SelectedIndexChanged);
            // 
            // LazyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(386, 300);
            this.Controls.Add(this.SetGroup);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LazyForm";
            this.Text = "❤(｡◕‿◕)ﾉﾟ･✿お♥や♥す♥み♥ー✿ﾟ･ヽ(◕‿◕｡)❤";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LazyForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.Magnifier)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.point)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.notifyIconExit.ResumeLayout(false);
            this.SetGroup.ResumeLayout(false);
            this.SetGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ColorLabel;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Label Position;
        private System.Windows.Forms.Timer MouserTimer;
        private System.Windows.Forms.CheckBox LeftAndRightCheckBox;
        private System.Windows.Forms.CheckBox DoubleClickCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox attackCheckBox;
        private System.Windows.Forms.PictureBox Magnifier;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox point;
        private System.Windows.Forms.CheckBox ToolsCheckBox;
        private System.Windows.Forms.CheckBox assistCheckBox;
        private System.Windows.Forms.Label HpLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label mpLabel;
        private System.Windows.Forms.Label specifyLable;
        private System.Windows.Forms.Label targetLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox siegeCheckBox;
        private System.Windows.Forms.Label specifyTxt;
        private System.Windows.Forms.Label targetTxt;
        private System.Windows.Forms.Label timeTxt;
        private System.Windows.Forms.Label mpTxt;
        private System.Windows.Forms.Label hpTxt;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconExit;
        private System.Windows.Forms.ToolStripMenuItem exitContextMenu;
        private System.Windows.Forms.ComboBox primaryCombo;
        private System.Windows.Forms.GroupBox SetGroup;
        private System.Windows.Forms.ComboBox addedCombo;
        private System.Windows.Forms.ComboBox otherCombo;
        private System.Windows.Forms.ComboBox secondaryCombo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}

