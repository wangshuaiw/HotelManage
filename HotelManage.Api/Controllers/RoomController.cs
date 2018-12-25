using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManage.Common;
using HotelManage.DBModel;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM;
using HotelManage.ViewModel.ApiVM.RequestVM;
using HotelManage.ViewModel.ApiVM.ResponseVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HotelManage.Api.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private IConfiguration Configuration { get; }
        private IRoomHander Hander { get; }
        private IHotelEnumHander EnumHander { get; }

        public RoomController(IConfiguration config, IRoomHander hander, IHotelEnumHander enumHander)
        {
            Configuration = config;
            Hander = hander;
            EnumHander = enumHander;
        }

        /// <summary>
        /// 获取房间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListResponse<RoomResponseExt>> GetRooms(int hotelId)
        {
            var rooms = await Task.Run(() => { return Hander.GetRooms(hotelId); });
            var result = rooms.Select(r => new RoomResponseExt()
            {
                Id = r.Id,
                HotelId = r.HotelId,
                RoomNo = r.RoomNo,
                RoomType = r.RoomType,
                Remark = r.Remark,
                IsDel = r.IsDel,
                roomTypeName = EnumHander.GetName(r.RoomType),
                CreateTime = r.CreateTime,
                UpdateTime = r.UpdateTime
            }).ToList();
            return new ListResponse<RoomResponseExt>()
            {
                Status=StatusEnum.Success,
                Massage="获取房间成功",
                Data = result
            };
        }

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<RoomResponseExt>> Add([FromBody]Room room)
        {
            string manager = HttpContext.User.Identity.Name;
            
            room.IsDel = false;

            Room r = await Task.Run(() => { return Hander.Add(room, manager); });
            return new Response<RoomResponseExt>()
            {
                Status = StatusEnum.Success,
                Massage = "添加成功",
                Data = new RoomResponseExt()
                {
                    Id = r.Id,
                    HotelId = r.HotelId,
                    RoomNo = r.RoomNo,
                    RoomType = r.RoomType,
                    Remark = r.Remark,
                    IsDel = r.IsDel,
                    roomTypeName = EnumHander.GetName(r.RoomType),
                    CreateTime = r.CreateTime,
                    UpdateTime = r.UpdateTime
                }
            };
        }

        /// <summary>
        /// 更新房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Updete([FromBody]Room room)
        {
            string manager = HttpContext.User.Identity.Name;

            await Task.Run(() => { Hander.Update(room, manager); });
            return new BaseResponse()
            {
                Status = StatusEnum.Success,
                Massage = "更新成功"
            };
        }

        /// <summary>
        /// 删除房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Delete([FromBody]Room room)
        {
            string manager = HttpContext.User.Identity.Name;

            await Task.Run(() => { Hander.Delete(room, manager); });
            return new BaseResponse()
            {
                Status = StatusEnum.Success,
                Massage = "删除成功"
            };
        }


    }
}