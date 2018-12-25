using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManage.DBModel;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManage.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class HotelEnumController : Controller
    {

        public IHotelEnumHander Hander { get; }

        public HotelEnumController(IHotelEnumHander hander)
        {
            Hander = hander;
        }

        //获取房间类型枚举
        [HttpGet]
        public async Task<ListResponse<Hotelenum>> GetRoomTypes()
        {
            var data = await Task.Run(() =>
            {
                return Hander.GetList(e => e.EnumClass == DbConst.RoomTypeEnumClass && !e.IsDel.Value);
            });
            return new ListResponse<Hotelenum>()
            {
                Status = StatusEnum.Success,
                Massage = "获取成功",
                Data= data
            };
        }

        //获取证件类型枚举
        [HttpGet]
        public async Task<ListResponse<Hotelenum>> GetCertTypes()
        {
            var data = await Task.Run(() =>
            {
                return Hander.GetList(e => e.EnumClass == DbConst.CertTypeEnumClass && !e.IsDel.Value);
            });
            return new ListResponse<Hotelenum>()
            {
                Status = StatusEnum.Success,
                Massage = "获取成功",
                Data = data
            };
        }
    }
}