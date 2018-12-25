using HotelManage.DBModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.ViewModel.ApiVM.RequestVM
{
    public class RoomExt:Room
    {
        public string WxOpenId { set; get; }
    }
}
