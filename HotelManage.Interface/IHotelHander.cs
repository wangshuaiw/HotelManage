using HotelManage.DBModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Interface
{
    public interface IHotelHander:IHotelManageHander<Hotel>
    {
        /// <summary>
        /// 新增宾馆
        /// </summary>
        /// <param name="hotel"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        Hotel Create(Hotel hotel, Hotelmanager manager);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        KeyValuePair<bool, string> Update(Hotel hotel);

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        bool CheckPassword(Hotel hotel, out Hotel oldHotel);
    }
}
