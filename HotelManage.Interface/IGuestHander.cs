using HotelManage.DBModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.Interface
{
    public interface IGuestHander:IHotelManageHander<Guest>
    {
        /// <summary>
        /// 添加入住人
        /// </summary>
        /// <param name="guest"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        Guest Add(Guest guest, string manager);

        /// <summary>
        /// 更新入住人
        /// </summary>
        /// <param name="guest"></param>
        /// <param name="manager"></param>
        void Update(Guest guest, string manager);

        /// <summary>
        /// 删除入住人
        /// </summary>
        /// <param name="guest"></param>
        /// <param name="manager"></param>
        void Delete(Guest guest, string manager);
    }
}
