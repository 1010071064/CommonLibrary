using APILibrary.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;

namespace APILibrary.Database
{
    /// <summary>
    /// 处理所有错误。通常这里将提供日志服务，以保持流程连贯。
    /// </summary>
    static class ErrorHelper
    {
        /// <summary>
        /// 在这里实现您自己的错误处理逻辑。日志服务，echo可以包括在内。
        /// </summary>
        /// <param name="e"></param>
        public static void Error(Exception e)
        {
            //I still use messagebox for demostration.
            //MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
