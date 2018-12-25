using HotelManage.DBModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Interface
{
    public interface IHotelManagerHander:IHotelManageHander<Hotelmanager>
    {
        /// <summary>
        /// 根据微信openid获取宾馆
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        Hotel GetHotelByOpenId(string openId);

        /// <summary>
        /// 新增管理者
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        KeyValuePair<bool, string> Add(Hotelmanager manager, string password);

        
    }
}
