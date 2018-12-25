using HotelManage.DBModel;
using HotelManage.ViewModel.ApiVM.ResponseVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Interface
{
    public interface IRoomHander:IHotelManageHander<Room>
    {
        /// <summary>
        /// 获取房间类型
        /// </summary>
        /// <returns></returns>
        //Task<Dictionary<string, string>> GetRoomTypes();

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        Room Add(Room room, string manager);

        /// <summary>
        /// 获取房间
        /// </summary>
        /// <param name="hotelID"></param>
        /// <returns></returns>
        List<Room> GetRooms(int hotelID);

        /// <summary>
        /// 更改房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        void Update(Room room, string manager);

        /// <summary>
        /// 逻辑删除房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        void Delete(Room room, string manager);

    }
}
