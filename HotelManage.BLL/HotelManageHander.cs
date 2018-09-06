using HotelManage.Common;
using HotelManage.DBModel;
using HotelManage.Interface;
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

        public async Task<T> Create(T t)
        {
            await HotelContext.AddAsync<T>(t);
            await HotelContext.SaveChangesAsync();
            return t;
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            return await HotelContext.FindAsync<T>(predicate);
        }

        public async Task<List<T>> GetList(Expression<Func<T, bool>> predicate)
        {
            return await HotelContext.Set<T>().Where(predicate).ToAsyncEnumerable().ToList();
        }

        public async Task Update(T t, params string[] properties)
        {
            var entry = EFHelper.GetEntry<T>(HotelContext, t, properties);
            await HotelContext.SaveChangesAsync();
        }
    }
}
