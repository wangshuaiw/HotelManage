using HotelManage.DBModel;
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
        Task<Dictionary<string, string>> GetRoomTypes();

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        new Task<Room> Create(Room room);

        /// <summary>
        /// 获取房间
        /// </summary>
        /// <param name="hotelID"></param>
        /// <returns></returns>
        Task<List<Room>> GetRooms(int hotelID);

        /// <summary>
        /// 更改房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        Task Update(Room room);

        /// <summary>
        /// 逻辑删除房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        Task Delete(Room room);
    }
}
