using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManage.DBModel;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM;
using HotelManage.ViewModel.ApiVM.ResponseVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HotelManage.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class RoomCheckController : Controller
    {
        private IConfiguration Configuration { get; }
        private IRoomCheckHander Hander { get; }

        public RoomCheckController(IConfiguration config,IRoomCheckHander hander)
        {
            Configuration = config;
            Hander = hander;
        }


        /// <summary>
        /// 获取房间状态(包括当前状态,历史入住情况,未来预定情况)
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListResponse<RoomStatusRespones>> GetRoomsStatus(int hotelId, DateTime? date)
        {
            int freeCheckinHour = Configuration.GetValue<int>("AppSetting:FreeCheckinHour");
            int freeCheckoutHour = Configuration.GetValue<int>("AppSetting:FreeCheckoutHour");
            List<RoomStatusRespones> result = null;
            if (date == null || date == DateTime.Now.Date)
            {
                //当前状态
                result = await Task.Run(() => { return Hander.GetRoomNowStatus(hotelId); });
            }
            else if (date < DateTime.Now.Date)
            {
                //历史入住情况
                result = await Task.Run(() =>
                {
                    return Hander.GetRoomHistoryCheckin(hotelId, date.Value, freeCheckinHour, freeCheckoutHour);
                });
            }
            else
            {
                //未来预定情况
                result = await Task.Run(() =>
                {
                    return Hander.GetRoomFutureReserve(hotelId, date.Value, freeCheckinHour, freeCheckoutHour);
                });
            }

            return new ListResponse<RoomStatusRespones>()
            {
                Status = StatusEnum.Success,
                Massage = "获取房间状态成功",
                Data = result
            };
        }

        /// <summary>
        /// 获取某个房间状态(包括当前状态,历史入住情况,未来预定情况)
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<RoomStatusRespones>> GetRoomStatus(int roomId, long? checkId, DateTime? date)
        {
            RoomStatusRespones result = null;
            if (checkId.HasValue && checkId.Value > 0)
            {
                result = await Task.Run(() => { return Hander.GetRoomCheckById(checkId.Value); });
            }
            else
            {
                int freeCheckinHour = Configuration.GetValue<int>("AppSetting:FreeCheckinHour");
                int freeCheckoutHour = Configuration.GetValue<int>("AppSetting:FreeCheckoutHour");
                if (date == null || date == DateTime.Now.Date)
                {
                    //当前状态
                    result = await Task.Run(() => { return Hander.GetRoomNowStatusByRoomId(roomId); });
                }
                else if (date < DateTime.Now.Date)
                {
                    //历史入住情况
                    result = await Task.Run(() =>
                    {
                        return Hander.GetRoomHistoryCheckinByRoomId(roomId, date.Value, freeCheckinHour, freeCheckoutHour);
                    });
                }
                else
                {
                    //未来预定情况
                    result = await Task.Run(() =>
                    {
                        return Hander.GetRoomFutureReserveByRoomId(roomId, date.Value, freeCheckinHour, freeCheckoutHour);
                    });
                }
            }
            return new Response<RoomStatusRespones>()
            {
                Status = StatusEnum.Success,
                Massage = "获取房间状态成功",
                Data = result
            };
        }

        /// <summary>
        /// 增加或者修改订单
        /// </summary>
        /// <param name="roomCheck"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<RoomStatusRespones>> ModifyRoomCheck([FromBody]RoomStatusRespones roomCheck)
        {
            string manager = HttpContext.User.Identity.Name;
            if (roomCheck.Id > 0)
            {
                Roomcheck check = new Roomcheck()
                {
                    Id = roomCheck.Id,
                    RoomId = roomCheck.RoomId,
                    Status = roomCheck.Status,
                    ReserveTime = roomCheck.ReserveTime,
                    PlanedCheckinTime = roomCheck.PlanedCheckinTime,
                    CheckinTime = roomCheck.CheckinTime,
                    PlanedCheckoutTime = roomCheck.PlanedCheckoutTime,
                    CheckoutTime = roomCheck.CheckoutTime,
                    Prices = roomCheck.Prices,
                    Deposit = roomCheck.Deposit,
                    Remark = roomCheck.Remark,
                    UpdateTime = DateTime.Now
                };
                await Task.Run(() => { Hander.UpdateCheck(check, manager); });
                
            }
            else
            {
                await Task.Run(() => { Hander.AddCheck(roomCheck, manager); });
            }
            return new Response<RoomStatusRespones>()
            {
                Status = StatusEnum.Success,
                Massage = "执行成功",
                Data = roomCheck
            };
        }
    }
}