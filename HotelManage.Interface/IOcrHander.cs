using HotelManage.DBModel;
using HotelManage.ViewModel.ApiVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.Interface
{
    public interface IOcrHander
    {
        //BaiduAccessToken GetBaiduAccess(string uri);

        KeyValuePair<Guest,string> GetGuestInfoFromCertByBaidu(string uri, string imageBuffer, int hotelId, string manager, int maxNum);
    }
}
