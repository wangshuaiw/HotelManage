using HotelManage.Common;
using HotelManage.DBModel;
using HotelManage.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HotelManage.BLL
{
    public class HotelHander : HotelManageHander<Hotel>, IHotelHander
    {
        public HotelHander(hotelmanageContext context):base(context)
        { }


        public async Task<bool> Create(Hotel hotel, Hotelmanager manager)
        {
            var tran = await HotelContext.Database.BeginTransactionAsync();
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
                return true;
            }
            catch(Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }
    }
}
