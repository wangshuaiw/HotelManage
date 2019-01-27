using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.ViewModel.ApiVM
{
    public class BaiduAccessToken
    {
        public string access_token;

        public int expires_in;

        public DateTime get_token_time;
    }
}
