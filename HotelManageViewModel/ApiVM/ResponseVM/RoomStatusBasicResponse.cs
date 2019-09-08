using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.ViewModel.ApiVM.ResponseVM
{
    public class RoomStatusBasicResponse
    {
        public int Id { get; set; }
        public string RoomNo { get; set; }
        public string RoomTypeName { get; set; }
        public int Status { get; set; }
    }
}
