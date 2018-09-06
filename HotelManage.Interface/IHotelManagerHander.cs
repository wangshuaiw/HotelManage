﻿using HotelManage.DBModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Interface
{
    public interface IHotelManagerHander:IHotelManageHander<Hotelmanager>
    {
        /// <summary>
        /// 新增管理者
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<string> Create(Hotelmanager manager, string password);
    }
}