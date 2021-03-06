﻿using System;
using System.Collections.Generic;
using System.Data.Odbc;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.Data.SQLite;
using Npgsql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
//using System.Windows.Forms;
using System.Data;
using System.IO;
using APILibrary.XMLHelpers;
using APILibrary.LogHelper;

namespace APILibrary.Database
{
    /// <summary>
    /// Add or remove the supported type by adding or removing the related GetConnection or GetDbDataAdapter method.
    /// 可以根据支持的Sql类型增加或删除类型，需要增加或删除对应的GetConnection和GetDbDataAdapter方法。
    /// </summary>
    public enum SqlType
    {
        SqlServer,
        MySql,
        PostgresQL,
        Oracle,
        SQLite,
        //(ODBC ONLY) host system must have the data driver installed. ODBC data source is necessary if DSN is used.
        //对ODBC方式需要格外注意，目标系统必须预先安装有对应的数据驱动，如果使用DSN，那么还需要使用配置ODBC数据源
        Odbc
    }
    /// <summary>
    /// The class use ADO.NET access to database. It is thread safe if a single active object is used.
    /// 使用ADO.NET控制对数据库的基本访问方法，对同一个活动对象（不关闭）线程安全。
    /// </summary>
    public class SqlManipulation : IDisposable
    {
        public SqlManipulation(string strDSN, SqlType sqlType)
        {
            _sqlType = sqlType;
            _strDSN = strDSN;
        }

        /// <summary>默认为SQL Server</summary>
        public SqlManipulation()
        {
            UpdateDBstr();
        }
        public void UpdateDBstr()
        {
            XMLHelper xm = new XMLHelper(ShareCode.XMLPath);
            try
            {
                _strDSN = xm.GetElement("SQLServerMsg", "SqlUrl");
            }
            catch (Exception e)
            {
                _strDSN = "";
                LogClass.Error("SqlManipulation", "UpdateDBstr", e.Message);
            }
        }
        /// <summary>
        ///
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                DbConnection conn = null;
                if (conn == null) return CreatConnection();
                else return conn;
            }
        }

        #region private variables
        /// <summary>数据库类型</summary>
        private SqlType _sqlType = SqlType.SqlServer;
        /// <summary>数据库连接字符串</summary>
        private string _strDSN;
        ///// <summary>数据库的连接</summary>
        //private DbConnection _conn;
        ///// <summary>是否释放</summary>
        //private bool _disposed;
        #endregion

        #region private functions --私有函数
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        private DbConnection CreatConnection()
        {
            DbConnection conn;
            switch (_sqlType)
            {
                case SqlType.SqlServer:
                    conn = new SqlConnection(_strDSN);
                    return conn;
                case SqlType.MySql:
                    conn = new MySqlConnection(_strDSN);
                    return conn;
                case SqlType.PostgresQL:
                    conn = new NpgsqlConnection(_strDSN);
                    return conn;
                case SqlType.Oracle:
                    conn = new OracleConnection(_strDSN);
                    return conn;
                case SqlType.SQLite:
                    conn = new SQLiteConnection(_strDSN);
                    return conn;
                case SqlType.Odbc:
                    conn = new OdbcConnection(_strDSN);
                    return conn;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 创建数据库的适配器
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private DbDataAdapter GetDbDataAdapter(string sql, DbConnection conn)
        {
            DbDataAdapter adp;
            switch (_sqlType)
            {
                case SqlType.SqlServer:
                    adp = new SqlDataAdapter(sql, conn as SqlConnection);
                    return adp;
                case SqlType.MySql:
                    adp = new MySqlDataAdapter(sql, conn as MySqlConnection);
                    return adp;
                case SqlType.PostgresQL:
                    adp = new NpgsqlDataAdapter(sql, conn as NpgsqlConnection);
                    return adp;
                case SqlType.Oracle:
                    adp = new OracleDataAdapter(sql, conn as OracleConnection);
                    return adp;
                case SqlType.SQLite:
                    adp = new SQLiteDataAdapter(sql, conn as SQLiteConnection);
                    return adp;
                case SqlType.Odbc:
                    adp = new OdbcDataAdapter(sql, conn as OdbcConnection);
                    return adp;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 向数据库发送SQL命令
        /// </summary>
        /// <param name="conn">数据库的连接</param>
        /// <param name="strSQL">数据库语句</param>
        /// <returns></returns>
        private DbCommand GetCommand(DbConnection conn, string strSQL)
        {
            DbCommand command = conn.CreateCommand();
            command.CommandText = strSQL;
            return command;
        }

        /// <summary>
        /// 数据读取对象
        /// </summary>
        /// <param name="conn">数据库的连接</param>
        /// <param name="strSQL">数据库语句</param>
        /// <returns></returns>
        private DbDataReader GetDataReader(DbConnection conn, string strSQL)
        {
            DbCommand command = GetCommand(conn, strSQL);
            return command.ExecuteReader();
        }

        /// <summary>
        /// 使用DataReader方式循环执行ReadSingleRow委托方法
        /// Use DataReader object to execute the delegate method
        /// </summary>
        /// <param name="strSQL">需要执行的SQL语句</param>
        /// <param name="ReadSingleRow">需要循环执行的方法</param>
        public void ExecuteReader(string strSQL, Action<IDataRecord> ReadSingleRow)
        {
            DbConnection conn = CreatConnection();
            using (DbDataReader myReader = GetDataReader(conn, strSQL))
            {
                while (myReader.Read())
                {
                    ReadSingleRow(myReader);
                }
            }
        }

        /// <summary>
        /// 直接获取DataReader对象，用于后续操作，请注意需要在使用完毕使用reader.Close()方法或者直接使用using语法.
        /// Get DataReader object directly for manipulation. Caution: use close() method to close reader after finishing the execution or simplely use using clause.
        /// for example:
        /// using (var reader = GetDataReader(strSQL))
        /// { while(reader.Read()) { ... } }
        /// </summary>
        /// <param name="strSQL">需要执行的SQL语句</param>
        /// <returns>DataReader对象</returns>
        public DbDataReader GetDataReader(string strSQL)
        {
            DbConnection conn = CreatConnection();
            return GetDataReader(conn, strSQL);
        }
        #endregion

        /// <summary>
        /// Init a connection and open it.
        /// 初始化连接并打开
        /// </summary>
        /// <returns>true if successful</returns>
        public DbConnection GetConnect()
        {
            try
            {
                DbConnection conn = CreatConnection();
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                //log and exit
                //ErrorHelper.Error(e);
                LogClass.Error("SqlManipulation", "InitConnect", e.Message);
                return null;
            }
        }

        /// <summary>
        /// Execute the select query.
        /// 执行SELECT查询语句，并返回DataTable对象。
        /// </summary>
        /// <param name="strSQL">
        /// Sql to be executed.
        /// 需要执行的sql语句
        /// </param>
        /// <returns>DataTable object</returns>
        public DataSet ExcuteQuery(string strSQL)
        {
            DbConnection con = GetConnect();
            DbDataAdapter adp = null;
            DataSet ds = new DataSet();
            try
            {
                adp = GetDbDataAdapter(strSQL, con);
                adp.Fill(ds);
            }
            catch (Exception e)
            {
                //log error and return null
                //ErrorHelper.Error(e);
                LogClass.Error("SqlManipulation", "ExcuteQuery", e.Message + Environment.NewLine +strSQL);
                return null;
            }
            finally
            {
                if(con !=null && con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行非Select语句，包括UPDATE DELETE INSERT
        /// </summary>
        /// <param name="strSQL">需要执行的sql语句</param>
        /// <returns>受影响的行数</returns>
        public int ExcuteNonQuery(string strSQL)
        {
            DbConnection con = GetConnect();
            //实例化OdbcCommand对象
            DbCommand myCmd = null;

            try
            {
                //执行方法
                myCmd = GetCommand(con, strSQL);
                return myCmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //log error and return null
                //ErrorHelper.Error(e);
                LogClass.Error("SqlManipulation", "ExcuteNonQuery", e.Message + Environment.NewLine + strSQL);
                return 0;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        /// <summary>
        /// 通过事务批量执行非查询SQL语句
        /// </summary>
        /// <param name="strSQLs">需要批量执行的SQL</param>
        /// <returns>受影响的行数，发生回滚则返回-1</returns>
        public int ExecuteNonQueryTransaction(List<string> strSQLs)
        {
            DbConnection con = GetConnect();
            DbCommand myCmd = GetCommand(con, "");
            int sumAffected = 0;

            DbTransaction transaction = null; con.BeginTransaction(IsolationLevel.ReadCommitted);
            
            try
            {
                transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
                myCmd.Transaction = transaction;
                foreach (var n in strSQLs)
                {
                    myCmd.CommandText = n;
                    sumAffected += myCmd.ExecuteNonQuery();
                }
                transaction.Commit();
                return sumAffected;
            }
            catch (Exception e)
            {
                //ErrorHelper.Error(e);
                LogClass.Error("SqlManipulation", "ExecuteNonQueryTransaction", e.Message + Environment.NewLine + myCmd.CommandText);
                if(transaction != null)
                    transaction.Rollback();
                return -1;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        /// <summary>
        /// 执行非查询SQL，使用Oracle的Blob对象
        /// </summary>
        /// <param name="sql">需要执行的sql语句，语句中有且仅有一个‘:blob’参数</param>
        /// <param name="path">需要插入的blob文件路径</param>
        /// <returns>受影响的行数，执行不成功则返回-1</returns>
        public int OracleBlobNonQuery(string sql, string path)
        {
            DbConnection conn = CreatConnection();
            if (!(conn is OracleConnection))
            {
                return -1;
            }

            //OracleCommand cmd = new OracleCommand("INSERT INTO TEST SET F2 =:blob", _conn as OracleConnection);
            OracleCommand cmd = new OracleCommand(sql, conn as OracleConnection);

            cmd.Parameters.Add(new OracleParameter("blob", OracleDbType.Blob));
            FileInfo fi = new FileInfo(path);
            var mydata = File.ReadAllBytes(path);
            cmd.Parameters["blob"].Value = mydata;
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogClass.Error("SqlManipulation", "OracleBlobNonQuery", e.Message + Environment.NewLine + sql);
                return -1;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 执行非查询SQL，使用SQL Server的Image对象
        /// </summary>
        /// <param name="sql">需要执行的sql语句，语句中有且仅有一个‘@blob’参数</param>
        /// <param name="path">需要插入的blob文件路径</param>
        /// <returns>受影响的行数，执行不成功则返回-1</returns>
        public int SqlImageNonQuery(string sql, string path)
        {
            DbConnection conn = CreatConnection();
            if (!(conn is SqlConnection))
            {
                return -1;
            }

            //SqlCommand cmd = new SqlCommand("INSERT INTO TEST SET F2 =@blob", _conn as SqlConnection);
            SqlCommand cmd = new SqlCommand(sql, conn as SqlConnection);

            cmd.Parameters.Add(new SqlParameter("@blob", SqlDbType.Image));
            FileInfo fi = new FileInfo(path);
            var mydata = File.ReadAllBytes(path);
            cmd.Parameters["@blob"].Value = mydata;
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                LogClass.Error("SqlManipulation", "SqlImageNonQuery", e.Message + Environment.NewLine + sql);
                return -1;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        #region resource cleanup 资源清理
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //if (!_disposed)
            //{
            if (disposing)
            {
                //托管对象释放
            }
            //非托管对象释放
            //if (_conn != null)
            //{
            //    if (_conn.State != ConnectionState.Closed)
            //    {
            //        _conn.Close();
            //    }
            //    else
            //    {
            //        _conn = null;
            //    }
            //}
            //    _disposed = true;
            //}
            //this.Dispose();
        }

        ~SqlManipulation()
        {
            Dispose(false);
        }
        #endregion
        #region 源
        //public SqlManipulation(string strDSN, SqlType sqlType)
        //{
        //    _sqlType = sqlType;
        //    _strDSN = strDSN;
        //}

        //public DbConnection Connection
        //{
        //    get
        //    {
        //        if (_conn == null) return GetConnection();
        //        else return _conn;
        //    }
        //}

        //#region private variables
        ///// <summary>数据库类型</summary>
        //private SqlType _sqlType;
        ///// <summary>数据库连接字符串</summary>
        //private string _strDSN;
        ///// <summary>数据库的连接</summary>
        //private DbConnection _conn;
        /////// <summary>是否释放</summary>
        ////private bool _disposed;
        //#endregion

        //#region private functions --私有函数
        ///// <summary>
        ///// 创建数据库连接
        ///// </summary>
        ///// <returns></returns>
        //private DbConnection GetConnection()
        //{
        //    DbConnection conn;
        //    switch (_sqlType)
        //    {
        //        case SqlType.SqlServer:
        //            conn = new SqlConnection(_strDSN);
        //            return conn;
        //        case SqlType.MySql:
        //            conn = new MySqlConnection(_strDSN);
        //            return conn;
        //        case SqlType.PostgresQL:
        //            conn = new NpgsqlConnection(_strDSN);
        //            return conn;
        //        case SqlType.Oracle:
        //            conn = new OracleConnection(_strDSN);
        //            return conn;
        //        case SqlType.SQLite:
        //            conn = new SQLiteConnection(_strDSN);
        //            return conn;
        //        case SqlType.Odbc:
        //            conn = new OdbcConnection(_strDSN);
        //            return conn;
        //        default:
        //            return null;
        //    }
        //}

        ///// <summary>
        ///// 创建数据库的适配器
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        //private DbDataAdapter GetDbDataAdapter(string sql)
        //{
        //    DbDataAdapter adp;
        //    switch (_sqlType)
        //    {
        //        case SqlType.SqlServer:
        //            adp = new SqlDataAdapter(sql, _conn as SqlConnection);
        //            return adp;
        //        case SqlType.MySql:
        //            adp = new MySqlDataAdapter(sql, _conn as MySqlConnection);
        //            return adp;
        //        case SqlType.PostgresQL:
        //            adp = new NpgsqlDataAdapter(sql, _conn as NpgsqlConnection);
        //            return adp;
        //        case SqlType.Oracle:
        //            adp = new OracleDataAdapter(sql, _conn as OracleConnection);
        //            return adp;
        //        case SqlType.SQLite:
        //            adp = new SQLiteDataAdapter(sql, _conn as SQLiteConnection);
        //            return adp;
        //        case SqlType.Odbc:
        //            adp = new OdbcDataAdapter(sql, _conn as OdbcConnection);
        //            return adp;
        //        default:
        //            return null;
        //    }
        //}

        ///// <summary>
        ///// 向数据库发送SQL命令
        ///// </summary>
        ///// <param name="conn">数据库的连接</param>
        ///// <param name="strSQL">数据库语句</param>
        ///// <returns></returns>
        //private DbCommand GetCommand(DbConnection conn, string strSQL)
        //{
        //    DbCommand command = conn.CreateCommand();
        //    command.CommandText = strSQL;
        //    return command;
        //}

        ///// <summary>
        ///// 数据读取对象
        ///// </summary>
        ///// <param name="conn">数据库的连接</param>
        ///// <param name="strSQL">数据库语句</param>
        ///// <returns></returns>
        //private DbDataReader GetDataReader(DbConnection conn, string strSQL)
        //{
        //    DbCommand command = GetCommand(conn, strSQL);
        //    return command.ExecuteReader();
        //}

        ///// <summary>
        ///// 使用DataReader方式循环执行ReadSingleRow委托方法
        ///// Use DataReader object to execute the delegate method
        ///// </summary>
        ///// <param name="strSQL">需要执行的SQL语句</param>
        ///// <param name="ReadSingleRow">需要循环执行的方法</param>
        //public void ExecuteReader(string strSQL, Action<IDataRecord> ReadSingleRow)
        //{
        //    using (DbDataReader myReader = GetDataReader(_conn, strSQL))
        //    {
        //        while (myReader.Read())
        //        {
        //            ReadSingleRow(myReader);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 直接获取DataReader对象，用于后续操作，请注意需要在使用完毕使用reader.Close()方法或者直接使用using语法.
        ///// Get DataReader object directly for manipulation. Caution: use close() method to close reader after finishing the execution or simplely use using clause.
        ///// for example:
        ///// using (var reader = GetDataReader(strSQL))
        ///// { while(reader.Read()) { ... } }
        ///// </summary>
        ///// <param name="strSQL">需要执行的SQL语句</param>
        ///// <returns>DataReader对象</returns>
        //public DbDataReader GetDataReader(string strSQL)
        //{
        //    return GetDataReader(_conn, strSQL);
        //}
        //#endregion

        ///// <summary>
        ///// Init a connection and open it.
        ///// 初始化连接并打开
        ///// </summary>
        ///// <returns>true if successful</returns>
        //public bool Init()
        //{
        //    try
        //    {
        //        _conn = GetConnection();
        //        _conn.Open();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        //log and exit
        //        ErrorHelper.Error(e);
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Execute the select query.
        ///// 执行SELECT查询语句，并返回DataTable对象。
        ///// </summary>
        ///// <param name="strSQL">
        ///// Sql to be executed.
        ///// 需要执行的sql语句
        ///// </param>
        ///// <returns>DataTable object</returns>
        //public DataTable ExcuteQuery(string strSQL)
        //{
        //    DbDataAdapter adp = GetDbDataAdapter(strSQL);
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        adp.Fill(dt);
        //    }
        //    catch (Exception e)
        //    {
        //        //log error and return null
        //        ErrorHelper.Error(e);
        //        return null;
        //    }
        //    return dt;
        //}

        ///// <summary>
        ///// 执行非Select语句，包括UPDATE DELETE INSERT
        ///// </summary>
        ///// <param name="strSQL">需要执行的sql语句</param>
        ///// <returns>受影响的行数</returns>
        //public int ExcuteNonQuery(string strSQL)
        //{
        //    //实例化OdbcCommand对象
        //    DbCommand myCmd = GetCommand(_conn, strSQL);

        //    try
        //    {
        //        //执行方法
        //        return myCmd.ExecuteNonQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        //记录日志，并返回0
        //        ErrorHelper.Error(e);
        //        return 0;
        //    }
        //}

        ///// <summary>
        ///// 通过事务批量执行非查询SQL语句
        ///// </summary>
        ///// <param name="strSQLs">需要批量执行的SQL</param>
        ///// <returns>受影响的行数，发生回滚则返回-1</returns>
        //public int ExecuteNonQueryTransaction(List<string> strSQLs)
        //{
        //    DbCommand myCmd = GetCommand(_conn, "");
        //    int sumAffected = 0;

        //    DbTransaction transaction = _conn.BeginTransaction();
        //    myCmd.Transaction = transaction;

        //    try
        //    {
        //        foreach (var n in strSQLs)
        //        {
        //            myCmd.CommandText = n;
        //            sumAffected += myCmd.ExecuteNonQuery();
        //        }
        //        transaction.Commit();
        //        return sumAffected;
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorHelper.Error(e);
        //        transaction.Rollback();
        //        return -1;
        //    }
        //}

        ///// <summary>
        ///// 执行非查询SQL，使用Oracle的Blob对象
        ///// </summary>
        ///// <param name="sql">需要执行的sql语句，语句中有且仅有一个‘:blob’参数</param>
        ///// <param name="path">需要插入的blob文件路径</param>
        ///// <returns>受影响的行数，执行不成功则返回-1</returns>
        //public int OracleBlobNonQuery(string sql, string path)
        //{
        //    if (!(_conn is OracleConnection))
        //    {
        //        return -1;
        //    }

        //    //OracleCommand cmd = new OracleCommand("INSERT INTO TEST SET F2 =:blob", _conn as OracleConnection);
        //    OracleCommand cmd = new OracleCommand(sql, _conn as OracleConnection);

        //    cmd.Parameters.Add(new OracleParameter("blob", OracleDbType.Blob));
        //    FileInfo fi = new FileInfo(path);
        //    var mydata = File.ReadAllBytes(path);
        //    cmd.Parameters["blob"].Value = mydata;
        //    try
        //    {
        //        return cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorHelper.Error(e);
        //        return -1;
        //    }
        //}

        ///// <summary>
        ///// 执行非查询SQL，使用SQL Server的Image对象
        ///// </summary>
        ///// <param name="sql">需要执行的sql语句，语句中有且仅有一个‘@blob’参数</param>
        ///// <param name="path">需要插入的blob文件路径</param>
        ///// <returns>受影响的行数，执行不成功则返回-1</returns>
        //public int SqlImageNonQuery(string sql, string path)
        //{
        //    if (!(_conn is SqlConnection))
        //    {
        //        return -1;
        //    }

        //    //SqlCommand cmd = new SqlCommand("INSERT INTO TEST SET F2 =@blob", _conn as SqlConnection);
        //    SqlCommand cmd = new SqlCommand(sql, _conn as SqlConnection);

        //    cmd.Parameters.Add(new SqlParameter("@blob", SqlDbType.Image));
        //    FileInfo fi = new FileInfo(path);
        //    var mydata = File.ReadAllBytes(path);
        //    cmd.Parameters["@blob"].Value = mydata;
        //    try
        //    {
        //        return cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        ErrorHelper.Error(e);
        //        return -1;
        //    }
        //}

        //#region resource cleanup 资源清理
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    //if (!_disposed)
        //    //{
        //    if (disposing)
        //    {
        //        //托管对象释放
        //    }
        //    //非托管对象释放
        //    if (_conn != null)
        //    {
        //        if (_conn.State != ConnectionState.Closed)
        //        {
        //            _conn.Close();
        //        }
        //        else
        //        {
        //            _conn = null;
        //        }
        //    }
        //    //    _disposed = true;
        //    //}
        //}

        //~SqlManipulation()
        //{
        //    Dispose(false);
        //}
        //#endregion
        #endregion
    }
}
