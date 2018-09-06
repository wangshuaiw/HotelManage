using System;
using System.Collections.Generic;

namespace HotelManage.DBModel
{
    public partial class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HotelPassword { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public bool? IsDel { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Salt { get; set; }
        public string Remark { get; set; }
    }
}
