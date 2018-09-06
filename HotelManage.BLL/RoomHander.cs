using HotelManage.DBModel;
using HotelManage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.BLL
{
    public class RoomHander:HotelManageHander<Room>,IRoomHander
    {
        public RoomHander(hotelmanageContext context):base(context)
        { }

        /// <summary>
        /// 获取房间类型
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string,string>> GetRoomTypes()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var roomTypes = await HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.RoomTypeEnumClass).Select(e => new KeyValuePair<string, string>(e.FullKey, e.Name)).Distinct().ToAsyncEnumerable().ToList();
            foreach(var t in roomTypes)
            {
                result.Add(t.Key,t.Value);
            }
            return result;
        }

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public new async Task<Room> Create(Room room)
        {
            var roomTypes = await GetRoomTypes();
            if(!roomTypes.ContainsKey(room.RoomType))
            {
                return null;
            }
            if(!HotelContext.Hotel.Any(h=>!h.IsDel.Value && h.Id==room.HotelId))
            {
                return null;
            }
            return await base.Create(room);
        }

        /// <summary>
        /// 获取房间
        /// </summary>
        /// <param name="hotelID"></param>
        /// <returns></returns>
        public async Task<List<Room>> GetRooms(int hotelID)
        {
            return await base.GetList(r => !r.IsDel.Value && r.HotelId == hotelID);
        }

        /// <summary>
        /// 更改房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task Update(Room room)
        {
            room.UpdateTime = DateTime.Now;
            await base.Update(room, "RoomNo", "RoomType", "Remark", "UpdateTime");
        }

        /// <summary>
        /// 逻辑删除房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task Delete(Room room)
        {
            room.IsDel = true;
            room.UpdateTime = DateTime.Now;
            await base.Update(room, "IsDel", "UpdateTime");
        }
    }
}
