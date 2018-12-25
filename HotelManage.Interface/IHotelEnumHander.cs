using HotelManage.DBModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Interface
{
    public interface IHotelEnumHander:IHotelManageHander<Hotelenum>
    {
        /// <summary>
        /// 根据key获取名称
        /// </summary>
        /// <param name="fullKey"></param>
        /// <returns></returns>
        string GetName(string fullKey);
    }
}
