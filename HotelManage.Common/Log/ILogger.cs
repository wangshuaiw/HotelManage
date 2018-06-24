using System;
using System.Collections.Generic;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace HotelManage.Common.Log
{
    /// <summary>
    ///  Log interface
    /// </summary>
    public interface ILogger
    {
        void Debug(object msg);

        void Info(object msg);

        void Error(object msg);

        void Error(object msg, Exception ex);
    }
}
