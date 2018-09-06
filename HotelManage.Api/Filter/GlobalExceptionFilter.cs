using HotelManage.ViewModel.ApiVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManage.Api.Filter
{
    public class GlobalExceptionFilter: IExceptionFilter
    {
        readonly ILogger<GlobalExceptionFilter> logger;

        public GlobalExceptionFilter (ILogger<GlobalExceptionFilter> _logger)
        {
            logger = _logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, "系统错误！");

            //返回
            BaseResponse response = new BaseResponse() { Status = StatusEnum.Error, Massage = "系统错误" };
            context.Result = new OkObjectResult(response);
        }

    }
}
