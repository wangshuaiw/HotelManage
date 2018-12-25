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

        T Get(Expression<Func<T, bool>> predicate);

        List<T> GetList(Expression<Func<T, bool>> predicate);

        T Create(T t);

        void Update(T t, params string[] properties);

        //Task Delete(T t);
    }
}
