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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HotelManage.Api.Controllers
{
    [Authorize]
    public class HotelController : Controller
    {
        //public ILogger<HotelController> Logger { get; }
        //public IConfiguration Configuration { get; }
        private IHotelHander Hander { get; }
        private IHotelManagerHander ManagerHander { get; }

        public HotelController(//ILogger<HotelController> logger,
            //IConfiguration configuration,
            IHotelHander hander, IHotelManagerHander managerHander)
        {
            //Logger = logger;
            //Configuration = configuration;
            Hander = hander;
            ManagerHander = managerHander;
        }

        /// <summary>
        /// 添加宾馆
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<Hotel>> Create([FromBody]HotelAndManager hotelAndManager)
        {
            if (string.IsNullOrEmpty(hotelAndManager.WxOpenId))
            {
                return new Response<Hotel>() { Status = StatusEnum.ValidateModelError, Massage = "没有微信ID" };
            }
            if (string.IsNullOrEmpty(hotelAndManager.Name))
            {
                return new Response<Hotel>() { Status = StatusEnum.ValidateModelError, Massage = "没有宾馆名称" };
            }
            var existHotel = await Task.Run(() => { return ManagerHander.GetHotelByOpenId(hotelAndManager.WxOpenId); });
            if (existHotel != null)
            {
                return new Response<Hotel>() { Status = StatusEnum.Error, Massage = "此用户已有宾馆" };
            }
            Hotel hotel = new Hotel()
            {
                Name = hotelAndManager.Name,
                HotelPassword = hotelAndManager.Password,
                Address = hotelAndManager.Address,
                Region = hotelAndManager.Region,
                Remark = hotelAndManager.Remark
            };
            Hotelmanager manager = new Hotelmanager()
            {
                WxOpenId = hotelAndManager.WxOpenId,
                WxUnionId = hotelAndManager.WxUnionId
            };
            hotel = await Task.Run(() => { return Hander.Create(hotel, manager); });
            hotel.HotelPassword = null;
            hotel.Salt = null;

            return new Response<Hotel>()
            {
                Status = StatusEnum.Success,
                Massage = "添加成功",
                Data = hotel
            };
        }

        /// <summary>
        /// 根据ID查询宾馆
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<Hotel>> GetHotelById(int hotelId)
        {
            //int hotelId;

            //if(!int.TryParse(obj.hotelId.toString(),out hotelId))
            if (hotelId <= 0)
            {
                return new Response<Hotel>() { Status = StatusEnum.ValidateModelError, Massage = "参数错误" };
            }

            Hotel hotel = await Task.Run(() => { return Hander.Get(h => h.Id == hotelId && !h.IsDel.Value); });
            return new Response<Hotel>() { Status = StatusEnum.Success, Data = hotel, Massage = "获取成功" };
        }

        //模糊查询
        [HttpPost]
        public async Task<ListResponse<Hotel>> GetHotels([FromBody]dynamic obj)
        {
            string name = obj.name;
            string region = obj.region;
            var hotels = await Task.Run(() => {
                return Hander.GetList(h => h.Region == region && name.Contains(h.Name) && !h.IsDel.Value);
            });
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
            //if (string.IsNullOrEmpty(hotel.Name))
            //{
            //    return new Response<Hotel>() { Status = StatusEnum.ValidateModelError, Massage = "没有宾馆名称" };
            //}
            //if (!Hander.CheckPassword(hotel))
            //{
            //    return new Response<Hotel>() { Status = StatusEnum.ValidateModelError, Massage = "密码错误" };
            //}
            //hotel.UpdateTime = DateTime.Now;
            //await Hander.Update(hotel, "Name", "Region", "Address", "UpdateTime","Remark");
            KeyValuePair<bool, string> result = await Task.Run(() => { return Hander.Update(hotel); });
            if (result.Key)
            {
                hotel.HotelPassword = null;
                return new Response<Hotel>()
                {
                    Status = StatusEnum.Success,
                    Massage = "修改成功",
                    Data = hotel
                };
            }
            else
            {
                return new Response<Hotel>() { Status = StatusEnum.ValidateModelError, Massage = result.Value };
            }
        }

        //删除
        [HttpPost]
        public async Task<BaseResponse> Delete([FromBody]Hotel hotel)
        {
            hotel.IsDel = true;
            hotel.UpdateTime = DateTime.Now;
            await Task.Run(() => {
                Hander.Update(hotel, "IsDel", "UpdateTime");
            });
            return new BaseResponse()
            {
                Status = StatusEnum.Success,
                Massage = "删除成功"
            };
        }
    }
}