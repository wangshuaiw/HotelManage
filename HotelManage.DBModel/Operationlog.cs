using System;
using System.Collections.Generic;

namespace HotelManage.DBModel
{
    public partial class Operationlog
    {
        public long Id { get; set; }
        public int Type { get; set; }
        public string TableName { get; set; }
        public string ForeignKey { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public bool? IsDel { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
