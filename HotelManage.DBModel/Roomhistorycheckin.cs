using System;
using System.Collections.Generic;

namespace HotelManage.DBModel
{
    public partial class Roomhistorycheckin
    {
        public long Id { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public string OccupantName { get; set; }
        public string OccupantCertType { get; set; }
        public string OccupantId { get; set; }
        public string OccupantMobile { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
