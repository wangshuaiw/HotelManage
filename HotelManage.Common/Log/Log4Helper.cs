using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace HotelManage.Common.Log
{
    public class Log4Helper : ILogger
    {
        private ILog logger;

        //定义一个静态变量来保存类的实例
        private static Log4Helper _instance = null;

        //定义一个标识确保线程同步
        private static object obj;

        //定义私有构造函数，使外界不能创建该类实例
        private Log4Helper()
        {
            logger = LogManager.GetLogger("HotelManage", "HotelManage");
        }

        //可以定义公有属性来提供全局访问点
        public static Log4Helper Instance
        {
            get
            {
                if(_instance==null)
                {
                    lock(obj)
                    {
                        _instance = new Log4Helper();
                    }
                }
                return _instance;
            }
        }

        public Log4Helper(string name)
        {
            logger = LogManager.GetLogger("HotelManage",name);
        }

        public void Debug(object msg)
        {
            logger.Debug(msg);
        }

        public void Error(object msg)
        {
            logger.Error(msg);
        }

        public void Error(object msg, Exception ex)
        {
            logger.Error(msg, ex);
        }

        public void Info(object msg)
        {
            logger.Info(msg);
        }
    }
}
