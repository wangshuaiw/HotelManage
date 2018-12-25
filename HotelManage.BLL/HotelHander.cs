using HotelManage.Common;
using HotelManage.DBModel;
using HotelManage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HotelManage.BLL
{
    public class HotelHander : HotelManageHander<Hotel>, IHotelHander
    {
        public HotelHander(hotelmanageContext context):base(context)
        { }

        public Hotel Create(Hotel hotel, Hotelmanager manager)
        {
            var tran = HotelContext.Database.BeginTransaction();
            try
            {
                hotel.Salt = Guid.NewGuid().ToString();
                hotel.HotelPassword = PwdHelper.GetPassword(hotel.HotelPassword, hotel.Salt);
                HotelContext.Hotel.Add(hotel);
                HotelContext.SaveChanges();

                manager.HotelId = hotel.Id;
                manager.Role = (int)ManagerRole.First;
                HotelContext.Hotelmanager.Add(manager);
                HotelContext.SaveChanges();

                tran.Commit();                
                return hotel;
            }
            catch(Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }

        public KeyValuePair<bool,string> Update(Hotel hotel)
        {
            if(string.IsNullOrEmpty(hotel.Name))
            {
                return new KeyValuePair<bool, string>(false, "宾馆名称不能为空");
            }
            Hotel oldHotel = null;
            if(!CheckPassword(hotel,out oldHotel))
            {
                return new KeyValuePair<bool, string>(false,"密码错误");
            }
            else
            {
                oldHotel.Name = hotel.Name;
                oldHotel.Region = hotel.Region;
                oldHotel.Address = hotel.Address;
                oldHotel.Remark = hotel.Remark;
                oldHotel.UpdateTime = DateTime.Now;
                HotelContext.SaveChanges();
                return new KeyValuePair<bool, string>(true, "更新成功");
            }
        }

        /// <summary>
        /// 校验密码
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        public bool CheckPassword(Hotel hotel,out Hotel oldHotel)
        {
            oldHotel = HotelContext.Hotel.FirstOrDefault(h => !h.IsDel.Value && h.Id == hotel.Id);
            string pwd = PwdHelper.GetPassword(hotel.HotelPassword, oldHotel.Salt);
            if (pwd != oldHotel.HotelPassword)
            {
                return false;
            }
            return true;
        }
    }
}
