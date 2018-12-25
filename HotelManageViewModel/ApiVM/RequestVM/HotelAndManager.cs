using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.ViewModel.ApiVM.RequestVM
{
    public class HotelAndManager
    {
        public string Name { set; get; }

        public string Password { set; get; }

        public string Region { set; get; }

        public string Address { set; get; }

        public string Remark { get; set; }

        public string WxOpenId { set; get; }

        public string WxUnionId { set; get; }
    }
}
