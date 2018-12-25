using System;
using System.Collections.Generic;

namespace HotelManage.DBModel
{
    public partial class Roomcheck
    {
        public long Id { get; set; }
        public int RoomId { get; set; }
        public int Status { get; set; }
        public DateTime? ReserveTime { get; set; }
        public DateTime? PlanedCheckinTime { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? PlanedCheckoutTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public decimal? Prices { get; set; }
        public decimal? Deposit { get; set; }
        public string Remark { get; set; }
        public bool? IsDel { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
