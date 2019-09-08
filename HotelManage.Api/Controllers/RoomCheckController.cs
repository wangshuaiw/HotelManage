using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManage.DBModel;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM;
using HotelManage.ViewModel.ApiVM.RequestVM;
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
                    return Hander.GetRoomHistoryCheckin(hotelId, date.Value);
                });
            }
            else
            {
                //未来预定情况
                result = await Task.Run(() =>
                {
                    return Hander.GetRoomFutureReserve(hotelId, date.Value);
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
        /// 指定时间内的房间状态
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListResponse<RoomStatusBasicResponse>> GetRoomsStatusBasicInfo(int hotelId,DateTime beginTime,DateTime endTime,long? checkId)
        {
            List<RoomStatusBasicResponse> result = await Task.Run(() =>
            {
                return Hander.GetRoomsStatusBasicInfo(hotelId, beginTime, endTime, checkId);
            });
            return new ListResponse<RoomStatusBasicResponse>()
            {
                Status = StatusEnum.Success,
                Massage = "success",
                Data = result
            };
        }

        /// <summary>
        /// 获取订单
        /// </summary>
        /// <param name="checkId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<RoomStatusRespones>> GetRoomCheck(long checkId)
        {
            RoomStatusRespones result = await Task.Run(() => { return Hander.GetRoomCheckById(checkId); });

            return new Response<RoomStatusRespones>()
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
                        return Hander.GetRoomHistoryCheckinByRoomId(roomId, date.Value);
                    });
                }
                else
                {
                    //未来预定情况
                    result = await Task.Run(() =>
                    {
                        return Hander.GetRoomFutureReserveByRoomId(roomId, date.Value);
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
            KeyValuePair<bool, string> result;
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
                List<Guest> guests = new List<Guest>();
                if(roomCheck.Guests!=null&&roomCheck.Guests.Count>0)
                {
                    guests = roomCheck.Guests.Select(g => new Guest()
                    {
                        Id = g.Id,
                        CheckId = check.Id,
                        Name = g.Name,
                        Gender =g.Gender,
                        CertType = g.CertType,
                        CertId = g.CertId,
                        Mobile = g.Mobile,
                        Address = g.Address,
                        IsDel = false
                    }).ToList();
                }
               result = await Task.Run(() => { return Hander.UpdateCheck(check,guests, manager); });
                
            }
            else
            {
                result = await Task.Run(() => { return Hander.AddCheck(roomCheck, manager); });
            }
            if(result.Key)
            {
                return new Response<RoomStatusRespones>()
                {
                    Status = StatusEnum.Success,
                    Massage = "执行成功",
                    Data = roomCheck
                };
            }
            else
            {
                return new Response<RoomStatusRespones>()
                {
                    Status = StatusEnum.ValidateModelError,
                    Massage = result.Value.StartsWith("数据错误")? "数据错误!请刷新重试!":result.Value,
                    Data = null
                };
            }
            
        }

        //[HttpPost]
        //public async Task<Response<RoomStatusRespones>> SaveRoomCkeck([FromBody]RoomCheckRequestVM quest)
        //{
        //    string manager = HttpContext.User.Identity.Name;
        //    if(quest.Type==(int)RoomStatus.Reserved)
        //    {
        //        RoomStatusRespones check = new RoomStatusRespones()
        //        {
        //            RoomId = quest.RoomID,
        //            RoomNo = quest.
        //        }
        //    }
            

        //}

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="roomCheck"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> DeleteCheck([FromBody]Roomcheck roomCheck)
        {
            string manager = HttpContext.User.Identity.Name;
            KeyValuePair<bool,string> result = await Task.Run(() => { return Hander.DeleteCheck(roomCheck, manager); });
            if (result.Key)
            {
                return new BaseResponse()
                {
                    Status = StatusEnum.Success,
                    Massage = "订单取消成功"
                };
            }
            else
            {
                return new BaseResponse()
                {
                    Status = StatusEnum.ValidateModelError,
                    Massage = result.Value
                };
            }
        }

        /// <summary>
        /// 离店
        /// </summary>
        /// <param name="roomCheck"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Checkout([FromBody]Roomcheck roomCheck)
        {
            string manager = HttpContext.User.Identity.Name;
            KeyValuePair<bool, string> result = await Task.Run(() => { return Hander.Checkout(roomCheck, manager); });
            if (result.Key)
            {
                return new BaseResponse()
                {
                    Status = StatusEnum.Success,
                    Massage = "订单取消成功"
                };
            }
            else
            {
                return new BaseResponse()
                {
                    Status = StatusEnum.ValidateModelError,
                    Massage = result.Value
                };
            }
        }
    }
}