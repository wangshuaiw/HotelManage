using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.DBModel
{
    public enum ManagerRole
    {
        First = 0,
        Assist = 1
    }

    public enum OperationgType
    {
        Insert = 1,
        Update = 2,
        Delete = 3
    }

    public enum RoomStatus
    {
        Reserved = 1,
        Checkin = 2,
        Ckeckout = 3
    }
}
