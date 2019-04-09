using HotelManage.DBModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.ViewModel.ApiVM.ResponseVM
{
    public class RoomStatusRespones
    {
        public long Id { get; set; }
        public int RoomId { get; set; }
        public string RoomNo { get; set; }
        public string RoomTypeName { get; set; }
        public int Status { get; set; }
        //public string OccupantName { get; set; }
        //public string OccupantCertType { get; set; }
        //public string OccupantId { get; set; }
        //public string OccupantMobile { get; set; }
        public DateTime? ReserveTime { get; set; }
        public DateTime? PlanedCheckinTime { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? PlanedCheckoutTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public decimal? Prices { get; set; }
        public decimal? Deposit { get; set; }
        public string Remark { get; set; }

        public List<GuestResponse> Guests { get; set; }
    }

    public class GuestResponse
    {
        public long Id { get; set; }
        public long CheckId { get; set; }
        public string Name { get; set; }
        public string CertType { get; set; }
        public string CertTypeName { get; set; }
        public string CertId { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
    }
}
