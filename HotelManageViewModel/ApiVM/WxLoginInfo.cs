using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.ViewModel.ApiVM
{
    public class WxLoginInfo
    {
        public string openid { set; get; }

        public string session_key { set; get; }

        public string unionid { set; get; }
    }
}
