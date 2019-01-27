using HotelManage.DBModel;
using HotelManage.ViewModel.ApiVM.ResponseVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.Interface
{
    public interface IRoomCheckHander:IHotelManageHander<Roomcheck>
    {
        /// <summary>
        /// 获取当前房间状态
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        List<RoomStatusRespones> GetRoomNowStatus(int hotelId);

        /// <summary>
        /// 获取历史入住情况
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        List<RoomStatusRespones> GetRoomHistoryCheckin(int hotelId, DateTime date, int freeChenkinTime, int freeCheckoutTime);

        /// <summary>
        /// 获取未来预定情况
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="date"></param>
        /// <param name="freeChenkinTime"></param>
        /// <param name="freeCheckoutTime"></param>
        /// <returns></returns>
        List<RoomStatusRespones> GetRoomFutureReserve(int hotelId, DateTime date, int freeChenkinTime, int freeCheckoutTime);

        /// <summary>
        /// 根据Id获取入住情况
        /// </summary>
        /// <param name="checkId"></param>
        /// <returns></returns>
        RoomStatusRespones GetRoomCheckById(long checkId);

        /// <summary>
        /// 根据房间ID获取当前状态
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        RoomStatusRespones GetRoomNowStatusByRoomId(int roomId);

        /// <summary>
        /// 根据房间ID和日期获取历史入住信息
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        RoomStatusRespones GetRoomHistoryCheckinByRoomId(int roomId, DateTime date, int freeChenkinTime, int freeCheckoutTime);

        /// <summary>
        /// 根据房间ID和日期获取未来预定情况
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        RoomStatusRespones GetRoomFutureReserveByRoomId(int roomId, DateTime date, int freeChenkinTime, int freeCheckoutTime);

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="roomcheck"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        KeyValuePair<bool, string> AddCheck(RoomStatusRespones roomcheck, string manager);

        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="check"></param>
        /// <param name="manager"></param>
        KeyValuePair<bool, string> UpdateCheck(Roomcheck check, string manager);

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="check"></param>
        /// <param name="manager"></param>
        KeyValuePair<bool, string> DeleteCheck(Roomcheck check, string manager);

        /// <summary>
        /// 客人离店
        /// </summary>
        /// <param name="check"></param>
        /// <param name="manager"></param>
        /// <returns></returns>
        KeyValuePair<bool, string> Checkout(Roomcheck check, string manager);
    }
}
