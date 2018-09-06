using System;
using System.Collections.Generic;

namespace HotelManage.DBModel
{
    public partial class Hotelenum
    {
        public int Id { get; set; }
        public string FullKey { get; set; }
        public string Name { get; set; }
        public string EnumClass { get; set; }
        public bool? IsDel { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
