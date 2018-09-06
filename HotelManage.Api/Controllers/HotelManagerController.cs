using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManage.DBModel;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManage.Api.Controllers
{
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
        public async Task<Response<Hotelmanager>> Create([FromBody]Hotelmanager hotelmanager)
        {
            var result = await Hander.Create(hotelmanager);
            if(result!=null)
            {
                return new Response<Hotelmanager>()
                {
                    Status = StatusEnum.Success,
                    Massage = "添加成功",
                    Data = result
                };
            }
            else
            {
                return new Response<Hotelmanager>()
                {
                    Status = StatusEnum.ValidateModelError,
                    Massage = "数据错误"
                };
            }
        }
    }
}