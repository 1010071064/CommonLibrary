namespace UnitTest
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.LogBtn = new System.Windows.Forms.Button();
            this.SQLLinkBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SQLConTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SQLTypeCmb = new System.Windows.Forms.ComboBox();
            this.ResultRtb = new System.Windows.Forms.RichTextBox();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.XMLBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LogBtn
            // 
            this.LogBtn.Location = new System.Drawing.Point(12, 41);
            this.LogBtn.Name = "LogBtn";
            this.LogBtn.Size = new System.Drawing.Size(75, 32);
            this.LogBtn.TabIndex = 0;
            this.LogBtn.Text = "写日志";
            this.LogBtn.UseVisualStyleBackColor = true;
            this.LogBtn.Click += new System.EventHandler(this.LogBtn_Click);
            // 
            // SQLLinkBtn
            // 
            this.SQLLinkBtn.Location = new System.Drawing.Point(913, 3);
            this.SQLLinkBtn.Name = "SQLLinkBtn";
            this.SQLLinkBtn.Size = new System.Drawing.Size(92, 32);
            this.SQLLinkBtn.TabIndex = 1;
            this.SQLLinkBtn.Text = "数据库连接";
            this.SQLLinkBtn.UseVisualStyleBackColor = true;
            this.SQLLinkBtn.Click += new System.EventHandler(this.SQLLinkBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(215, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "连接字符串：";
            // 
            // SQLConTxt
            // 
            this.SQLConTxt.Location = new System.Drawing.Point(318, 7);
            this.SQLConTxt.Name = "SQLConTxt";
            this.SQLConTxt.Size = new System.Drawing.Size(579, 25);
            this.SQLConTxt.TabIndex = 3;
            this.SQLConTxt.Text = "server=localhost;database=US_Smartdb;uid=sa;pwd=123456";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "数据库类型";
            // 
            // SQLTypeCmb
            // 
            this.SQLTypeCmb.FormattingEnabled = true;
            this.SQLTypeCmb.Items.AddRange(new object[] {
            "SqlServer",
            "MySql",
            "PostgresQL",
            "Oracle",
            "SQLite",
            "Odbc"});
            this.SQLTypeCmb.Location = new System.Drawing.Point(101, 8);
            this.SQLTypeCmb.Name = "SQLTypeCmb";
            this.SQLTypeCmb.Size = new System.Drawing.Size(107, 23);
            this.SQLTypeCmb.TabIndex = 5;
            // 
            // ResultRtb
            // 
            this.ResultRtb.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ResultRtb.Location = new System.Drawing.Point(0, 296);
            this.ResultRtb.Name = "ResultRtb";
            this.ResultRtb.Size = new System.Drawing.Size(1061, 252);
            this.ResultRtb.TabIndex = 6;
            this.ResultRtb.TabStop = false;
            this.ResultRtb.Text = "";
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(0, 267);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(109, 23);
            this.ClearBtn.TabIndex = 7;
            this.ClearBtn.Text = "清空";
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // XMLBtn
            // 
            this.XMLBtn.Location = new System.Drawing.Point(101, 41);
            this.XMLBtn.Name = "XMLBtn";
            this.XMLBtn.Size = new System.Drawing.Size(75, 32);
            this.XMLBtn.TabIndex = 8;
            this.XMLBtn.Text = "读取XML";
            this.XMLBtn.UseVisualStyleBackColor = true;
            this.XMLBtn.Click += new System.EventHandler(this.XMLBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 548);
            this.Controls.Add(this.XMLBtn);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.ResultRtb);
            this.Controls.Add(this.SQLTypeCmb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SQLConTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SQLLinkBtn);
            this.Controls.Add(this.LogBtn);
            this.Name = "Form1";
            this.Text = "测试";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LogBtn;
        private System.Windows.Forms.Button SQLLinkBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SQLConTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox SQLTypeCmb;
        private System.Windows.Forms.RichTextBox ResultRtb;
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.Button XMLBtn;
    }
}

