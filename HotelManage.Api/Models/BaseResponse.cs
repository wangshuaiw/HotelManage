using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManage.Api.Models
{
    public class BaseResponse
    {
        public StatusEnum Status { set; get; }

        public string Massage { set; get; }

        public object Data { set; get; }
    }

    public enum StatusEnum
    {
        Success = 1,
        Error = -1,//未知异常
        ValidateModelError = -2 //模型验证错误
    }
}
