using HotelManage.Common;
using HotelManage.DBModel;
using HotelManage.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.BLL
{
    public class HotelManageHander<T> : IHotelManageHander<T> where T:class
    {
        public hotelmanageContext HotelContext { get; }

        public HotelManageHander(hotelmanageContext context)
        {
            HotelContext = context;
        }

        public T Create(T t)
        {
            HotelContext.Add<T>(t);
            HotelContext.SaveChanges();
            return t;
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return HotelContext.Set<T>().FirstOrDefault(predicate);
        }

        public List<T> GetList(Expression<Func<T, bool>> predicate)
        {
            return HotelContext.Set<T>().Where(predicate).ToList();
        }

        //更新
        public void Update(T t, params string[] properties)
        {
            var entry = HotelContext.Entry<T>(t);
            //entry.State = EntityState.Unchanged;  //当前面有查询的时候此处会报错！！！
            foreach (var p in properties)
            {
                entry.Property(p).IsModified = true;
            }
            entry.State = EntityState.Modified;
            foreach (var p in entry.Properties)
            {
                p.IsModified = false;
            }
            foreach (var p in properties)
            {
                entry.Property(p).IsModified = true;
            }
            HotelContext.SaveChanges();
        }
    }
}
