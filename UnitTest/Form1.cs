using APILibrary.Database;
using APILibrary.LogHelper;
using APILibrary.XMLHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.SQLTypeCmb.SelectedIndex = 0;
        }
        private void ShowResult(string result)
        {
            this.ResultRtb.AppendText(DateTime.Now.ToString() + ": " + result + "\n");
            this.ResultRtb.Select(this.ResultRtb.TextLength, 0);//设置光标的位置到文本尾
            this.ResultRtb.ScrollToCaret();//滚动到控件光标处
        }
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            this.ResultRtb.Clear();
        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogBtn_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            int number = r.Next();
            LogClass.Info("UnitTest", "LogBtn_Click", number.ToString());
            ShowResult("写入日志：" + number.ToString());
        }
        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SQLLinkBtn_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(this.SQLTypeCmb.Text) || string.IsNullOrEmpty(this.SQLConTxt.Text.Trim()))
                return;
            this.SQLLinkBtn.Enabled = false;
            SqlType sqltype = (SqlType)Enum.Parse(typeof(SqlType), this.SQLTypeCmb.Text);
            SqlManipulation sql = new SqlManipulation(this.SQLConTxt.Text.Trim(), sqltype);
            if (sql.GetConnect() != null)
            {
                ShowResult("连接成功！");
            }
            else
            {
                ShowResult("连接失败！");
            }
            this.SQLLinkBtn.Enabled = true;
        }

        private void XMLBtn_Click(object sender, EventArgs e)
        {
            XMLHelper xm = new XMLHelper(ShareCode.XMLPath);
            ShowResult(xm.GetElement("SQLServerMsg", "SqlUrl"));
        }
    }
}
