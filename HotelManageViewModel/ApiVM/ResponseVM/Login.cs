﻿using HotelManage.DBModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.ViewModel.ApiVM.ResponseVM
{
    public class Login
    {
        public string JwtToken { set; get; }

        public Hotel Hotel { set; get; }
    }
}
