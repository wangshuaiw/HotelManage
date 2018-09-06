using HotelManage.DBModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Interface
{
    public interface IHotelManageHander<T>
    {
        hotelmanageContext HotelContext { get; }

        Task<T> Get(Expression<Func<T, bool>> predicate);

        Task<List<T>> GetList(Expression<Func<T, bool>> predicate);

        Task<T> Create(T t);

        Task Update(T t, params string[] properties);

        //Task Delete(T t);
    }
}
