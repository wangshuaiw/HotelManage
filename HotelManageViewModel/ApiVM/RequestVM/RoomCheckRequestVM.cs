using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.ViewModel.ApiVM.RequestVM
{
    public class RoomCheckRequestVM
    {
        public int Type { set; get; }

        public DateTime BeginTime { set; get; }

        public DateTime EndTime { set; get; }

        public int[] RoomID { set; get; }

        public decimal Prices { get; set; }

        public decimal? Deposit { get; set; }

        public string Remark { set; get; }

        public List<GuestRequestVM> Guests { set; get; }
    }

    public class GuestRequestVM
    {
        public string Name { get; set; }
        public int Gender { get; set; }
        public string CertType { get; set; }
        public string CertId { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
    }
}
