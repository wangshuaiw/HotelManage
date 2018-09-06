using System;
using System.Collections.Generic;

namespace HotelManage.DBModel
{
    public partial class Roomstatus
    {
        public long Id { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public decimal? Prices { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
