using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using APILibrary.XMLHelpers;

namespace APILibrary.Database
{
    /// <summary>
    /// 共享的、通用的、全局的变量集的类
    /// </summary>
    public class ShareCode
    {
        /// <summary>
        /// XML配置文档的路径
        /// </summary>
        public static string XMLPath
        {
            get
            {
                //return System.Windows.Forms.Application.StartupPath + "\\LocalSetting.XML";
                return System.AppDomain.CurrentDomain.BaseDirectory + "LocalSetting.XML";
            }
        }
        #region 数据库连接字符串
        /// <summary>
        /// SQLite数据库连接字符串
        /// </summary>
        public static string SQLiteConnectionStr
        {
            get 
            {
                return "Data Source  = ./App_Data/UserInfo.db;Password=wipinfo123.";
                //return "Data Source  = ./App_Data/UserInfo.db";
            }
        }

        /// <summary>
        /// Access数据库连接字符串
        /// </summary>
        public static string AccessConnectionStr
        {
            get
            {
                //<!--Access数据库连接字符串_ACE引擎 包括JET引擎访问-->
                //<accessurl_ACE>Provider = Microsoft.Ace.OLEDB.12.0;Data Source = ./App_Data/DB_Lobby.accdb;Jet OLEDB:Database Password = wipinfo123.</accessurl_ACE> <!--& App.Path & "/chncmadb1.mdb-->
                //<!--Access数据库连接字符串_JET引擎 office97~office2003-->
                //<accessurl_JET>Provider=Microsoft.Jet.OleDb.4.0;Data Source = ./App_Data/DB_Lobby.accdb;Jet OLEDB:Database Password = wipinfo123.</accessurl_JET>
                return "Provider = Microsoft.Ace.OLEDB.12.0;Data Source = ./App_Data/DB_Lobby.accdb;Jet OLEDB:Database Password = 'wipinfo123.'";
            }
        }


        #endregion

        //以下可以根据项目来定义字段
        private static string _username;
        /// <summary>账号名称</summary>
        public static string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        private static string _userid = "";
        public static string UserId
        {
            get
            {
                return _userid;
            }
            set
            {
                _userid = value;
            }
        }


        public static string Url;
        public static string WebUrlIp
        {
            get
            {
                XMLHelper xml = new XMLHelper(XMLPath);
                return xml.GetElement("WebserverIP", "weburl");
            }
        }

        public static string strSN
        {
            get
            {
                XMLHelper xml = new XMLHelper(XMLPath);
                return xml.GetElement("zwtest", "strSN");
            }
        }
       
    }
}
