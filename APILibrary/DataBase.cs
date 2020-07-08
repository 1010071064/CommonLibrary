using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data;
using System;
using System.IO;
using System.Net;
using System.Xml;
//using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Text;
using APILibrary.XMLHelpers;
using APILibrary.Database;
using APILibrary.LogHelper;

namespace CodeBase
{
    public class DataBase
    {
        XMLHelper xm;
        DataSet ds = new DataSet();

        private string _strDBCon = "";

        public DataBase()
        {
            UpdateDBstr();
        }

        public void UpdateDBstr()
        {
            xm = new XMLHelper(ShareCode.XMLPath);
            _strDBCon = xm.GetElement("SqlserverIP", "sqlurl");
        }

        public void SetDBConInfo(string dbConInfo)
        {
            _strDBCon = dbConInfo;
        }

        public SqlConnection SqlConn()
        {
            SqlConnection con = null;
            try
            {
                GC.Collect();
                con = new SqlConnection(_strDBCon);
                con.Open();
                return con;
            }
            catch (Exception e)
            {
                GC.Collect();
                LogClass.Error("DataBase", "SqlConn", e);
                return null;
            }
        }

        public SqlConnection SqlConn(string constr)
        {
            try
            {
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                return con;
            }
            catch
            {
                return null;
            }
        }

        public void SqlClose(SqlConnection con)
        {
            if (con != null && con.State != ConnectionState.Closed)
            {
                con.Close();
                con.Dispose();
            }
        }
        //取出数据集 
        public DataSet GetDataSet(string sqlstr)
        {
            SqlConnection con = SqlConn();
            try
            {
                SqlDataAdapter fs = new SqlDataAdapter(sqlstr, con);
                DataSet ds = new DataSet();
                fs.Fill(ds);
                SqlClose(con);
                return ds;
            }
            catch (Exception e)
            {   
                LogClass.Error("DataBase", "GetDataSet", e);
                return null;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
                GC.Collect();
            }
        }

        //修改数据 
        public bool UpDataNone(string sqlstr)
        {
            SqlConnection con = SqlConn();
            try
            {
                SqlCommand cmd = new SqlCommand(sqlstr, con);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                LogClass.Error("DataBase", "UpDataNone", e);
                return false;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed)
                {
                    con.Close();
                    con.Dispose();
                }
                GC.Collect();
            }
        }

        private SqlConnection _con = null;
        private SqlTransaction _tran = null;
        public bool BeginUpDataNone()
        {
            _con = SqlConn();
            _tran = _con.BeginTransaction(IsolationLevel.ReadCommitted);
            return _con != null;
        }

        public bool ExcuteSql(string strSql)
        {
            try
            {
                if (BeginUpDataNone())
                {
                    SqlCommand cmd = new SqlCommand(strSql, _con);
                    cmd.Transaction = _tran;
                    cmd.ExecuteNonQuery();
                    EndUpDataNone();
                    return true;
                }
            }
            catch (Exception e)
            {
                LogClass.Error("DataBase", "UpDataNone", e);
                RollBack();
            }
            finally
            {
                GC.Collect();
            }
            return false;
        }

        public void RollBack()
        {
            if (_tran != null)
            {
                _tran.Rollback();
            }
            if (_con != null && _con.State != ConnectionState.Closed)
            {
                _con.Close();
                _con.Dispose();
            }
        }

        public void EndUpDataNone()
        {
            if (_tran != null)
            {
                _tran.Commit();
            }
            if (_con != null && _con.State != ConnectionState.Closed)
            {
                _con.Close();
                _con.Dispose();
            }
        }

        //public void keypress(object sender, KeyPressEventArgs e)
        //{
        //    if ((e.KeyChar < '0' && e.KeyChar != '.' || e.KeyChar > '9' && e.KeyChar != '.' || ((TextBox)(sender)).Text.IndexOf('.') >= 0 && e.KeyChar == '.') && e.KeyChar != (char)13 && e.KeyChar != (char)8)
        //    {
        //        e.Handled = true;
        //    }

        //}
        //public void keypressint(object sender, KeyPressEventArgs e)
        //{
        //    if ((e.KeyChar < '0' || e.KeyChar > '9' || ((TextBox)(sender)).Text.IndexOf('.') >= 0 && e.KeyChar == '.') && e.KeyChar != (char)13 && e.KeyChar != (char)8)
        //    {
        //        e.Handled = true;
        //    }

        //}
        //public void keypressintgrd(object sender, KeyEventArgs e)
        //{
        //    if ((e.KeyValue < '0' || e.KeyValue > '9' || ((TextBox)(sender)).Text.IndexOf('.') >= 0 && e.KeyValue == '.') && e.KeyValue != (char)13 && e.KeyValue != (char)8)
        //    {
        //        e.Handled = true;
        //    }

        //}
        //获取省局服务器时间  add by Carrot at 2017-03-14
        public string GetServerTime(string strSql)
        {
            SqlConnection con = null;
            string result = "";
            try
            {
                xm = new XMLHelper(ShareCode.XMLPath);
                string sqlurl = xm.GetElement("SqlserverIP", "sqlurlTime");
                con = new SqlConnection(sqlurl);
                if (con == null)
                {
                    con.Open();
                }
                else if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                else if (con.State == System.Data.ConnectionState.Broken)
                {
                    con.Close();
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand(strSql, con);
                result = cmd.ExecuteScalar().ToString();
                con.Close();

            }
            catch (Exception e)
            {
                con.Close();
                return DateTime.Now.ToString();
            }
            return result;
        }

    }
}
