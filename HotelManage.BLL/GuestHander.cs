using HotelManage.DBModel;
using HotelManage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelManage.BLL
{
    public class GuestHander:HotelManageHander<Guest>,IGuestHander
    {
        public GuestHander(hotelmanageContext context):base(context)
        {
        }

        public Guest Add(Guest guest,string manager)
        {
            if(string.IsNullOrWhiteSpace(guest.Name))
            {
                throw new Exception("入住人姓名为空");
            }
            if(string.IsNullOrWhiteSpace(guest.CertId))
            {
                throw new Exception("入住人证件号码为空");
            }
            if(guest.CheckId<=0)
            {
                throw new Exception("订单外键错误");
            }
            if(!HotelContext.Hotelenum.Any(e=>e.FullKey==guest.CertType&&!e.IsDel.Value))
            {
                throw new Exception("证件类型错误");
            }
            var check = HotelContext.Roomcheck.FirstOrDefault(c => c.Id == guest.CheckId && !c.IsDel.Value);
            if (check == null)
            {
                throw new Exception("为找到有效订单");
            }
            var room = HotelContext.Room.FirstOrDefault(r => r.Id == check.RoomId && !r.IsDel.Value);
            if(room==null)
            {
                throw new Exception("为找到订单所在的房间");
            }
            if(!HotelContext.Hotelmanager.Any(m=>m.HotelId==room.HotelId&&m.WxOpenId==manager&&!m.IsDel.Value))
            {
                throw new Exception("添加入住人时没有管理员权限!");
            }
            guest.IsDel = false;
            guest.CreateTime = DateTime.Now;
            guest.UpdateTime = DateTime.Now;
            base.Create(guest);
            return guest;
        }

        public void Update(Guest guest, string manager)
        {
            if (string.IsNullOrWhiteSpace(guest.Name))
            {
                throw new Exception("入住人姓名为空");
            }
            if (string.IsNullOrWhiteSpace(guest.CertId))
            {
                throw new Exception("入住人证件号码为空");
            }
            if (guest.CheckId <= 0)
            {
                throw new Exception("订单外键错误");
            }
            if (!HotelContext.Hotelenum.Any(e => e.FullKey == guest.CertType && !e.IsDel.Value))
            {
                throw new Exception("证件类型错误");
            }
            var check = HotelContext.Roomcheck.FirstOrDefault(c => c.Id == guest.CheckId && !c.IsDel.Value);
            if (check == null)
            {
                throw new Exception("为找到有效订单");
            }
            var room = HotelContext.Room.FirstOrDefault(r => r.Id == check.RoomId && !r.IsDel.Value);
            if (room == null)
            {
                throw new Exception("为找到订单所在的房间");
            }
            if (!HotelContext.Hotelmanager.Any(m => m.HotelId == room.HotelId && m.WxOpenId == manager && !m.IsDel.Value))
            {
                throw new Exception("添加入住人时没有管理员权限!");
            }
            guest.UpdateTime = DateTime.Now;
            base.Update(guest, "CheckId", "Name", "CertType", "CertId", "Mobile", "UpdateTime");

        }

        public void Delete(Guest guest,string manager)
        {
            if (guest.CheckId <= 0)
            {
                throw new Exception("订单外键错误");
            }
            var check = HotelContext.Roomcheck.FirstOrDefault(c => c.Id == guest.CheckId && !c.IsDel.Value);
            if (check == null)
            {
                throw new Exception("为找到有效订单");
            }
            var room = HotelContext.Room.FirstOrDefault(r => r.Id == check.RoomId && !r.IsDel.Value);
            if (room == null)
            {
                throw new Exception("为找到订单所在的房间");
            }
            if (!HotelContext.Hotelmanager.Any(m => m.HotelId == room.HotelId && m.WxOpenId == manager && !m.IsDel.Value))
            {
                throw new Exception("添加入住人时没有管理员权限!");
            }
            guest.IsDel = true;
            guest.UpdateTime = DateTime.Now;
            base.Update(guest, "IsDel", "UpdateTime");
        }
    }
}
