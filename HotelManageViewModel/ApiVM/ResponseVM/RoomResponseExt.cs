using HotelManage.DBModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.ViewModel.ApiVM.ResponseVM
{
    public class RoomResponseExt:Room
    {
        public string roomTypeName { set; get; }
    }
}
