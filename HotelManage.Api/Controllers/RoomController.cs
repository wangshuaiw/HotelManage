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
    public class RoomController : Controller
    {
        public IRoomHander Hander { get; }

        public RoomController(IRoomHander hander)
        {
            Hander = hander;
        }

        /// <summary>
        /// 获取房间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ListResponse<Room>> GetRooms([FromBody]dynamic obj)
        {
            int hotelId = obj.hotelId;
            return new ListResponse<Room>()
            {
                Status=StatusEnum.Success,
                Massage="获取房间成功",
                Data = await Hander.GetRooms(hotelId)
            };
        }

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<Room>> Create([FromBody]Room room)
        {
            var r = await Hander.Create(room);
            if(r==null)
            {
                return new Response<Room>()
                {
                    Status = StatusEnum.ValidateModelError,
                    Massage = "数据错误"
                };
            }
            else
            {
                return new Response<Room>()
                {
                    Status = StatusEnum.Success,
                    Massage = "添加成功",
                    Data = r
                };
            }
        }

        /// <summary>
        /// 更新房间
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Updete([FromBody]Room room)
        {
            await Hander.Update(room);
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
            await Hander.Delete(room);
            return new BaseResponse()
            {
                Status = StatusEnum.Success,
                Massage = "删除成功"
            };
        }
    }
}