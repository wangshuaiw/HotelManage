using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.ViewModel.ApiVM.RequestVM
{
    public class AddManager
    {
        public int HotelId { get; set; }
        public string WxOpenId { get; set; }
        public string WxUnionId { get; set; }
        public string Password { get; set; }
    }
}
