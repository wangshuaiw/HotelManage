using HotelManage.DBModel;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM.ResponseVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.BLL
{
    public class RoomHander : HotelManageHander<Room>, IRoomHander
    {
        public RoomHander(hotelmanageContext context) : base(context)
        { }

        /// <summary>
        /// 获取房间类型
        /// </summary>
        /// <returns></returns>
        //public async Task<Dictionary<string,string>> GetRoomTypes()
        //{
        //    Dictionary<string, string> result = new Dictionary<string, string>();
        //    var roomTypes = await HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.RoomTypeEnumClass).Select(e => new KeyValuePair<string, string>(e.FullKey, e.Name)).Distinct().ToAsyncEnumerable().ToList();
        //    foreach(var t in roomTypes)
        //    {
        //        result.Add(t.Key,t.Value);
        //    }
        //    return result;
        //}

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public Room Add(Room room, string manager)
        {
            var roomTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.RoomTypeEnumClass).ToList();
            if (!roomTypes.Any(r => r.FullKey == room.RoomType))
            {
                throw new Exception("添加房间时房间类型错误!");
            }
            if (!HotelContext.Hotel.Any(h => !h.IsDel.Value && h.Id == room.HotelId))
            {
                throw new Exception("添加房间时宾馆未找到!");
            }
            if (!HotelContext.Hotelmanager.Any(m => !m.IsDel.Value && m.HotelId == room.HotelId && m.WxOpenId == manager))
            {
                throw new Exception("添加房间时没有管理员权限!");
            }
            return base.Create(room);
        }

        /// <summary>
        /// 获取房间
        /// </summary>
        /// <param name="hotelID"></param>
        /// <returns></returns>
        public List<Room> GetRooms(int hotelID)
        {
            return base.GetList(r => !r.IsDel.Value && r.HotelId == hotelID).OrderBy(r=>r.RoomNo).ToList();
        }

        /// <summary>
        /// 更改房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public void Update(Room room, string manager)
        {
            var roomTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.RoomTypeEnumClass).ToList();
            if (!roomTypes.Any(r => r.FullKey == room.RoomType))
            {
                throw new Exception("更新房间时房间类型错误!");
            }
            if (!HotelContext.Hotel.Any(h => !h.IsDel.Value && h.Id == room.HotelId))
            {
                throw new Exception("更新房间时宾馆未找到!");
            }
            if (!HotelContext.Hotelmanager.Any(m => !m.IsDel.Value && m.HotelId == room.HotelId && m.WxOpenId == manager))
            {
                throw new Exception("更新房间时没有管理员权限!");
            }
            room.UpdateTime = DateTime.Now;
            base.Update(room, "RoomNo", "RoomType", "Remark", "UpdateTime");
        }

        /// <summary>
        /// 逻辑删除房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public void Delete(Room room, string manager)
        {
            if (!HotelContext.Hotelmanager.Any(m => !m.IsDel.Value && m.HotelId == room.HotelId && m.WxOpenId == manager))
            {
                throw new Exception("更新房间时没有管理员权限!");
            }
            room.IsDel = true;
            room.UpdateTime = DateTime.Now;
            base.Update(room, "IsDel", "UpdateTime");
        }

    }
}
