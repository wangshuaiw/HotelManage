using HotelManage.Common;
using HotelManage.DBModel;
using HotelManage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.BLL
{
    public class HotelManagerHander:HotelManageHander<Hotelmanager>,IHotelManagerHander
    {
        public HotelManagerHander(hotelmanageContext context):base(context)
        {
        }

        /// <summary>
        /// 根据微信openid获取宾馆
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public Hotel GetHotelByOpenId(string openId)
        {
            return (from m in HotelContext.Hotelmanager
             join h in HotelContext.Hotel
             on m.HotelId equals h.Id
             where m.IsDel == false && h.IsDel == false && m.WxOpenId == openId
             select h).FirstOrDefault();
        }

        /// <summary>
        /// 新增管理者
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public KeyValuePair<bool, string> Add(Hotelmanager manager,string password)
        {
            Hotel hotel = HotelContext.Hotel.FirstOrDefault(h => !h.IsDel.Value && h.Id == manager.HotelId);
            if (hotel==null)
            {
                return new KeyValuePair<bool, string>(false, "传入的宾馆有误");
            }
            string pwd = PwdHelper.GetPassword(password, hotel.Salt);
            if(pwd!=hotel.HotelPassword)
            {
                return new KeyValuePair<bool, string>(false, "密码错误");
            }
            base.Create(manager);
            return new KeyValuePair<bool, string>(true, "添加成功");
        }

        
    }
}
