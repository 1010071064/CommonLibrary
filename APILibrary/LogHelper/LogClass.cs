using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APILibrary.LogHelper
{
    /*
     使用log4net.dll，引用后，在Properties->AssemblyInfo.cs 添加以下
     * [assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4j.xml",ConfigFileExtension = "xml", Watch = true)]
     */


    /// <summary>
    /// 添加日记类
    /// </summary>
    public class LogClass
    {
        /// <summary>
        /// 添加日志。
        /// </summary>
        /// <param name="clsName">触发日志的当前对象</param>
        /// <param name="funcName">功能名称</param>
        /// <param name="messeg">消息</param>
        public static void Debug(string clsName, string funcName, string messeg)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(clsName + "-->" + funcName);
            log.Debug(messeg);
        }

        /// <summary>
        /// 添加日志。
        /// </summary>
        /// <param name="clsName">触发日志的当前对象</param>
        /// <param name="funcName">功能名称</param>
        /// <param name="messeg">消息</param>
        public static void Error(string clsName, string funcName, string messeg)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(clsName + "-->" + funcName);
            log.Error(messeg);
        }

        public static void Error(string clsName, string funcName, Exception exp)
        {
            if (exp != null)
            {
                string expMessage = exp.Message;
                if (exp.InnerException != null)
                {
                    expMessage += exp.InnerException.Message;
                }
                expMessage += exp.StackTrace;
                LogClass.Error(clsName, funcName, expMessage);
            }
        }

        public static void Error(string funcName, Exception exp)
        {
            if (exp != null)
            {
                string expMessage = exp.Message;
                if (exp.InnerException != null)
                {
                    expMessage += exp.InnerException.Message;
                }
                expMessage += "crcn" + exp.StackTrace;
                LogClass.Error("Error", funcName, expMessage);
            }
        }
        /// <summary>
        /// 添加日志。
        /// </summary>
        /// <param name="clsName">触发日志的当前对象</param>
        /// <param name="funcName">功能名称</param>
        /// <param name="messeg">消息</param>
        public static void Info(string clsName, string funcName, string messeg)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(clsName + "-->" + funcName);
            log.Info(messeg);
        }
    }
}
