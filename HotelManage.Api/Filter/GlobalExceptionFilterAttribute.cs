using HotelManage.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManage.Api.Filter
{
    public class GlobalExceptionFilterAttributeAttribute: ExceptionFilterAttribute
    {
        private readonly ILogger logger;

        public GlobalExceptionFilterAttributeAttribute()
        {
            if(logger==null)
            {
                ILoggerFactory LoggerFactory = new LoggerFactory();
                logger = LoggerFactory.CreateLogger("GlobalException");
            }
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, "系统错误！{0}",context.Exception.Message);

            //返回
            BaseResponse response = new BaseResponse() { Status = StatusEnum.Error, Massage = "系统错误" };
            context.Result = new OkObjectResult(response);
        }

    }
}
