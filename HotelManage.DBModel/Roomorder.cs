using System;
using System.Collections.Generic;

namespace HotelManage.DBModel
{
    public partial class Roomorder
    {
        public long Id { get; set; }
        public int? RoomId { get; set; }
        public string OccupantName { get; set; }
        public string OccupantCertType { get; set; }
        public string OccupantId { get; set; }
        public string OccupantMobile { get; set; }
        public decimal? Deposit { get; set; }
        public DateTime? ReserveTime { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public bool? IsDel { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
