using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM;
using HotelManage.DBModel;

namespace HotelManage.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class GuestController : Controller
    {
        private IGuestHander Hander { get; }

        public GuestController(IGuestHander hander)
        {
            Hander = hander;
        }

        /// <summary>
        /// 添加入住人
        /// </summary>
        /// <param name="guest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<Guest>> Add(Guest guest)
        {
            string manager = HttpContext.User.Identity.Name;
            await Task.Run(() => { Hander.Add(guest, manager); });
            return new Response<Guest>()
            {
                Status = StatusEnum.Success,
                Massage = "添加入住人成功",
                Data = guest
            };
        }

        /// <summary>
        /// 修改入住人
        /// </summary>
        /// <param name="guest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Update(Guest guest)
        {
            string manager = HttpContext.User.Identity.Name;
            await Task.Run(() => { Hander.Update(guest, manager); });
            return new BaseResponse()
            {
                Status = StatusEnum.Success,
                Massage = "修改入住人成功"
            };
        }

        /// <summary>
        /// 删除入住人
        /// </summary>
        /// <param name="guest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Delete(Guest guest)
        {
            string manager = HttpContext.User.Identity.Name;
            await Task.Run(() => { Hander.Delete(guest, manager); });
            return new BaseResponse()
            {
                Status = StatusEnum.Success,
                Massage = "删除入住人成功"
            };
        }
    }
}