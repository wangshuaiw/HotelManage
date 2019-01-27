using System;
using System.Collections.Generic;

namespace HotelManage.DBModel
{
    public partial class Baiduapilog
    {
        public long Id { get; set; }
        public int HotelId { get; set; }
        public int? Type { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
