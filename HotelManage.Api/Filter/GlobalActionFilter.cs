using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HotelManage.ViewModel.ApiVM;

namespace HotelManage.Api.Filter
{
    /// <summary>
    /// 自定义全局ActionFilter
    /// </summary>
    public class GlobalActionFilter: IActionFilter
    {
        private readonly ILogger<GlobalActionFilter> logger;
        private IConfiguration configuration;

        int processWarnTime;

        public GlobalActionFilter(ILogger<GlobalActionFilter> _logger, IConfiguration _configuration)
        {
            logger = _logger;
            configuration = _configuration;
            processWarnTime = configuration.GetValue<int>("AppSetting:ProcessWarnTime");
        }

        /// <summary>
        /// 模型验证 and 计时器
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //模型验证
            if (!context.ModelState.IsValid)
            {
                var modelState = context.ModelState.FirstOrDefault(f => f.Value.Errors.Any());
                string errorMsg = modelState.Value.Errors.First().ErrorMessage;
                BaseResponse response = new BaseResponse()
                {
                    Status = StatusEnum.ValidateModelError,
                    Massage = "数据校验不符:" + errorMsg
                };
                context.Result = new OkObjectResult(response);
            }

            //开启计时器
            var stopwach = new Stopwatch();
            stopwach.Start();
            context.HttpContext.Items.Add("__GlabolStopwach__", stopwach);

        }

        /// <summary>
        /// 超时记录 and ...
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var httpContext = context.HttpContext;
            var stopwach = httpContext.Items["__GlabolStopwach__"] as Stopwatch;
            stopwach.Stop();
            var time = stopwach.Elapsed;

            if (time.TotalSeconds > processWarnTime)
            {
                logger.LogWarning($"{context.ActionDescriptor.DisplayName}执行耗时过长:{time.ToString()}");
            }
        }
    }
}
