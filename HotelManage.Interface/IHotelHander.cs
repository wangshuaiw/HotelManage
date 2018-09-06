using HotelManage.DBModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Interface
{
    public interface IHotelHander:IHotelManageHander<Hotel>
    {
        Task<bool> Create(Hotel hotel, Hotelmanager manager);

    }
}
