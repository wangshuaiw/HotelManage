using System;
using System.Collections.Generic;

namespace HotelManage.ViewModel.ApiVM
{
    public class BaseResponse
    {
        public StatusEnum Status { set; get; }

        public string Massage { set; get; }

    }

    public class Response<T> : BaseResponse
    {
        public T Data { set; get; }
    }

    public class ListResponse<T> : BaseResponse
    {
        public List<T> Data { set; get; }
    }


    public enum StatusEnum
    {
        Success = 1,
        Error = -1,//未知异常
        ValidateModelError = -2 //模型验证错误
    }
}
