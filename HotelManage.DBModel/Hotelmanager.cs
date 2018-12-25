using System;
using System.Collections.Generic;

namespace HotelManage.DBModel
{
    public partial class Hotelmanager
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string WxOpenId { get; set; }
        public string WxUnionId { get; set; }
        public bool? IsDel { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Role { get; set; }
    }
}
