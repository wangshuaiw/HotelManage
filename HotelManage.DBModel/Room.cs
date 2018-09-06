using System;
using System.Collections.Generic;

namespace HotelManage.DBModel
{
    public partial class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string RoomNo { get; set; }
        public string RoomType { get; set; }
        public bool? IsDel { get; set; }
        public string Remark { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
