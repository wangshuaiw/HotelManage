using System;
using System.Collections.Generic;

namespace HotelManage.DBModel
{
    public partial class Guest
    {
        public long Id { get; set; }
        public long CheckId { get; set; }
        public string Name { get; set; }
        public string CertType { get; set; }
        public string CertId { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public bool? IsDel { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
