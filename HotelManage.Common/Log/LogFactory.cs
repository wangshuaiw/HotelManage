using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.Common.Log
{
    public class LogFactory
    {
        /// <summary>
        /// 默认Log4net日志
        /// </summary>
        /// <returns></returns>
        public static ILogger Default()
        {
            return Log4Helper.Instance;
        }
    }
}
