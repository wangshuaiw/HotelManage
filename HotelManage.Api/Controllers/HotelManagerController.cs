using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManage.DBModel;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM;
using HotelManage.ViewModel.ApiVM.RequestVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManage.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class HotelManagerController : Controller
    {
        public IHotelManagerHander Hander { get; }

        public HotelManagerController(IHotelManagerHander hander)
        {
            Hander = hander;
        }

        /// <summary>
        /// 添加管理者
        /// </summary>
        /// <param name="hotelmanager"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<Hotelmanager>> Add([FromBody]AddManager request)
        {
            Hotelmanager manager = new Hotelmanager()
            {
                HotelId = request.HotelId,
                WxOpenId = request.WxOpenId,
                WxUnionId = request.WxUnionId,
                Role = (int)ManagerRole.Assist,
                IsDel = false
            };
            var result = await Task.Run(() => { return Hander.Add(manager, request.Password); });
            if(result.Key)
            {
                return new Response<Hotelmanager>()
                {
                    Status = StatusEnum.Success,
                    Massage = "添加成功",
                    Data = manager
                };
            }
            else
            {
                return new Response<Hotelmanager>()
                {
                    Status = StatusEnum.ValidateModelError,
                    Massage = result.Value
                };
            }
        }
    }
}