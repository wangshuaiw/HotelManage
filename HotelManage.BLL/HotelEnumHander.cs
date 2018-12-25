using HotelManage.DBModel;
using HotelManage.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.BLL
{
    public class HotelEnumHander: HotelManageHander<Hotelenum>, IHotelEnumHander
    {
        public HotelEnumHander(hotelmanageContext context) : base(context)
        {

        }

        public string GetName(string fullKey)
        {
            var result = HotelContext.Hotelenum.FirstOrDefault(e => !e.IsDel.Value && e.FullKey == fullKey);
            if(result!=null)
            {
                return result.Name;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
