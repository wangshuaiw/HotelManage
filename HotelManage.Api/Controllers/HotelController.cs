using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManage.DBModel;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM;
using HotelManage.ViewModel.ApiVM.RequestVM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HotelManage.Api.Controllers
{

    public class HotelController : Controller
    {
        //public ILogger<HotelController> Logger { get; }
        //public IConfiguration Configuration { get; }
        public IHotelHander Hander { get; }

        public HotelController(//ILogger<HotelController> logger,
            //IConfiguration configuration,
            IHotelHander hander)
        {
            //Logger = logger;
            //Configuration = configuration;
            Hander = hander;
        }

        /// <summary>
        /// 添加宾馆
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<Hotel>> Create([FromBody]HotelAndManager hotelAndManager)
        {
            Hotel hotel = new Hotel()
            {
                Name = hotelAndManager.Name,
                HotelPassword = hotelAndManager.Password,
                Address = hotelAndManager.Address,
                Region = hotelAndManager.Region
            };
            Hotelmanager manager = new Hotelmanager()
            {
                WxOpenId = hotelAndManager.WxOpenId,
                WxUnionId = hotelAndManager.WxUnionId
            };
            await Hander.Create(hotel, manager);
            return new Response<Hotel>()
            {
                Status = StatusEnum.Success,
                Massage = "添加成功",
                Data=hotel
            };
        }

        //模糊查询
        [HttpPost]
        public async Task<ListResponse<Hotel>> GetHotels([FromBody]dynamic obj)
        {
            string name = obj.name;
            string region = obj.region;
            var hotels = await Hander.GetList(h => h.Region == region && name.Contains(h.Name) && !h.IsDel.Value);
            return new ListResponse<Hotel>()
            {
                Status = StatusEnum.Success,
                Massage = "查询成功",
                Data = hotels
            };
        }

        //更新
        [HttpPost]
        public async Task<Response<Hotel>> Update([FromBody]Hotel hotel)
        {
            hotel.UpdateTime = DateTime.Now;
            await Hander.Update(hotel, "Name", "Region", "Address", "UpdateTime");
            return new Response<Hotel>()
            {
                Status = StatusEnum.Success,
                Massage = "修改成功",
                Data = hotel
            };
        }

        //删除
        [HttpPost]
        public async Task<BaseResponse> Delete([FromBody]Hotel hotel)
        {
            hotel.IsDel = true;
            hotel.UpdateTime = DateTime.Now;
            await Hander.Update(hotel, "IsDel", "UpdateTime");
            return new BaseResponse()
            {
                Status = StatusEnum.Success,
                Massage = "删除成功"
            };
        }
    }
}