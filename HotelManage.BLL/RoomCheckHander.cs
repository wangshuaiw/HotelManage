using HotelManage.DBModel;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM.ResponseVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManage.BLL
{
    public class RoomCheckHander: HotelManageHander<Roomcheck>, IRoomCheckHander
    {
        public RoomCheckHander(hotelmanageContext context) : base(context)
        { }

        /// <summary>
        /// 获取当前房间状态
        /// </summary>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        public List<RoomStatusRespones> GetRoomNowStatus(int hotelId)
        {
            List<RoomStatusRespones> result = new List<RoomStatusRespones>();
            var rooms = HotelContext.Room.Where(r => !r.IsDel.Value && r.HotelId == hotelId).ToList();
            var roomTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.RoomTypeEnumClass).ToList();
            var certTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.CertTypeEnumClass).ToList();
            foreach (Room r in rooms)
            {
                RoomStatusRespones status = new RoomStatusRespones()
                {
                    RoomId = r.Id,
                    RoomNo = r.RoomNo,
                    RoomTypeName = roomTypes.FirstOrDefault(t => t.FullKey == r.RoomType) == null ? "" : roomTypes.FirstOrDefault(t => t.FullKey == r.RoomType).Name
                };
                var check = HotelContext.Roomcheck.FirstOrDefault(o => !o.IsDel.Value && o.RoomId == r.Id && (o.Status == (int)RoomStatus.Checkin || o.Status == (int)RoomStatus.Reserved));
                if (check != null)
                {
                    status.Id = check.Id;
                    status.Status = check.Status;
                    status.ReserveTime = check.ReserveTime;
                    status.PlanedCheckinTime = check.PlanedCheckinTime;
                    status.CheckinTime = check.CheckinTime;
                    status.PlanedCheckoutTime = check.PlanedCheckoutTime;
                    status.CheckoutTime = check.CheckoutTime;
                    status.Prices = check.Prices;
                    status.Deposit = check.Deposit;
                    status.Guests = HotelContext.Guest.Where(g => !g.IsDel.Value && g.CheckId == check.Id).Select(g => new GuestResponse()
                    {
                        Id = g.Id,
                        CheckId = g.CheckId,
                        Name = g.Name,
                        CertType = g.CertType,
                        CertTypeName = certTypes.FirstOrDefault(t => t.FullKey == g.CertType) == null ? "" :
                              certTypes.FirstOrDefault(t => t.FullKey == g.CertType).Name,
                        CertId = g.CertId,
                        Mobile = g.Mobile,
                        Address = g.Address
                    }).ToList();
                };

                result.Add(status);
            }
            
            return result;
        }

        /// <summary>
        /// 获取历史入住信息
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="date"></param>
        /// <param name="freeChenkinTime"></param>
        /// <param name="freeCheckoutTime"></param>
        /// <returns></returns>
        public List<RoomStatusRespones> GetRoomHistoryCheckin(int hotelId, DateTime date, int freeChenkinTime, int freeCheckoutTime)
        {
            DateTime freeChenkinDateTime = new DateTime(date.Year, date.Month, date.Day, freeChenkinTime, 0, 0);
            DateTime freeCheckoutDateTime = new DateTime(date.Year, date.Month, date.Day, freeChenkinTime, 0, 0);
            List<RoomStatusRespones> result = new List<RoomStatusRespones>();
            var rooms = HotelContext.Room.Where(r => !r.IsDel.Value && r.HotelId == hotelId).ToList();
            var roomTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.RoomTypeEnumClass).ToList();
            var certTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.CertTypeEnumClass).ToList();
            foreach (Room r in rooms)
            {
                RoomStatusRespones status = new RoomStatusRespones()
                {
                    RoomId = r.Id,
                    RoomNo = r.RoomNo,
                    RoomTypeName = roomTypes.FirstOrDefault(t => t.FullKey == r.RoomType) == null ? "" : roomTypes.FirstOrDefault(t => t.FullKey == r.RoomType).Name
                };
                //每日结算点前入住，结算点后离店的和结算点后入住，次日免费入住点前入住的
                var checks = HotelContext.Roomcheck.Where(
                    c => !c.IsDel.Value
                    && c.Status == (int)RoomStatus.Ckeckout
                    && ((c.CheckinTime <= freeCheckoutDateTime && c.CheckoutTime > freeCheckoutDateTime)
                         || (c.CheckinTime > freeCheckoutDateTime && c.CheckinTime < freeChenkinDateTime.AddDays(1)))
                    ).ToList();

                checks.AddRange(HotelContext.Roomcheck.Where(
                    c => !c.IsDel.Value
                    && c.Status == (int)RoomStatus.Checkin
                    && ((c.CheckinTime <= freeCheckoutDateTime && c.CheckoutTime == null)
                         || (c.CheckinTime > freeCheckoutDateTime && c.CheckinTime < freeChenkinDateTime.AddDays(1)))
                    ));
                if (checks != null && checks.Count() > 0)
                {
                    foreach (var check in checks)
                    {
                        status.Id = check.Id;
                        status.Status = check.Status;
                        status.ReserveTime = check.ReserveTime;
                        status.PlanedCheckinTime = check.PlanedCheckinTime;
                        status.CheckinTime = check.CheckinTime;
                        status.PlanedCheckoutTime = check.PlanedCheckoutTime;
                        status.CheckoutTime = check.CheckoutTime;
                        status.Prices = check.Prices;
                        status.Deposit = check.Deposit;
                        status.Guests = HotelContext.Guest.Where(g => !g.IsDel.Value && g.CheckId == check.Id).Select(g => new GuestResponse()
                        {
                            Id = g.Id,
                            CheckId = g.CheckId,
                            Name = g.Name,
                            CertType = g.CertType,
                            CertTypeName = certTypes.FirstOrDefault(t => t.FullKey == g.CertType) == null ? "" :
                                  certTypes.FirstOrDefault(t => t.FullKey == g.CertType).Name,
                            CertId = g.CertId,
                            Mobile = g.Mobile,
                            Address = g.Address
                        }).ToList();
                    }
                };

                result.Add(status);
            }
            return result;
        }

        /// <summary>
        /// 获取未来预定情况
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="date"></param>
        /// <param name="freeChenkinTime"></param>
        /// <param name="freeCheckoutTime"></param>
        /// <returns></returns>
        public List<RoomStatusRespones> GetRoomFutureReserve(int hotelId, DateTime date, int freeChenkinTime, int freeCheckoutTime)
        {
            DateTime freeChenkinDateTime = new DateTime(date.Year, date.Month, date.Day, freeChenkinTime, 0, 0);
            DateTime freeCheckoutDateTime = new DateTime(date.Year, date.Month, date.Day, freeChenkinTime, 0, 0);
            List<RoomStatusRespones> result = new List<RoomStatusRespones>();
            var rooms = HotelContext.Room.Where(r => !r.IsDel.Value && r.HotelId == hotelId).ToList();
            var roomTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.RoomTypeEnumClass).ToList();
            var certTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.CertTypeEnumClass).ToList();
            foreach (Room r in rooms)
            {
                RoomStatusRespones status = new RoomStatusRespones()
                {
                    RoomId = r.Id,
                    RoomNo = r.RoomNo,
                    RoomTypeName = roomTypes.FirstOrDefault(t => t.FullKey == r.RoomType) == null ? "" : roomTypes.FirstOrDefault(t => t.FullKey == r.RoomType).Name
                };
                //每日结算点前入住，结算点后离店的和结算点后入住，次日免费入住点前入住的
                var checks = HotelContext.Roomcheck.Where(
                    c => !c.IsDel.Value
                    && c.Status == (int)RoomStatus.Reserved
                    && ((c.PlanedCheckinTime <= freeCheckoutDateTime && c.PlanedCheckoutTime > freeCheckoutDateTime)
                         || (c.PlanedCheckinTime > freeCheckoutDateTime && c.PlanedCheckinTime < freeChenkinDateTime.AddDays(1)))
                    ).ToList();
                checks.AddRange(HotelContext.Roomcheck.Where(
                    c => !c.IsDel.Value
                    && c.Status == (int)RoomStatus.Checkin
                    && (c.CheckinTime <= freeCheckoutDateTime && c.PlanedCheckoutTime > freeCheckoutDateTime)
                    ));
                if (checks != null && checks.Count() > 0)
                {
                    foreach (var check in checks)
                    {
                        status.Id = check.Id;
                        status.Status = check.Status;
                        status.ReserveTime = check.ReserveTime;
                        status.PlanedCheckinTime = check.PlanedCheckinTime;
                        status.CheckinTime = check.CheckinTime;
                        status.PlanedCheckoutTime = check.PlanedCheckoutTime;
                        status.CheckoutTime = check.CheckoutTime;
                        status.Prices = check.Prices;
                        status.Deposit = check.Deposit;
                        status.Guests = HotelContext.Guest.Where(g => !g.IsDel.Value && g.CheckId == check.Id).Select(g => new GuestResponse()
                        {
                            Id = g.Id,
                            CheckId = g.CheckId,
                            Name = g.Name,
                            CertType = g.CertType,
                            CertTypeName = certTypes.FirstOrDefault(t => t.FullKey == g.CertType) == null ? "" :
                                  certTypes.FirstOrDefault(t => t.FullKey == g.CertType).Name,
                            CertId = g.CertId,
                            Mobile = g.Mobile,
                            Address = g.Address
                        }).ToList();
                    }
                };

                result.Add(status);
            }
            return result;
        }

        /// <summary>
        /// 根据入住ID获取入住信息
        /// </summary>
        /// <param name="checkId"></param>
        /// <returns></returns>
        public RoomStatusRespones GetRoomCheckById(long checkId)
        {
            RoomStatusRespones result = new RoomStatusRespones();
            Roomcheck check = base.Get(c => c.Id == checkId && !c.IsDel.Value);
            if (check != null)
            {
                result.Id = check.Id;
                result.RoomId = check.RoomId;
                Room room = HotelContext.Room.FirstOrDefault(r => r.Id == check.RoomId && !r.IsDel.Value);
                if (room != null)
                {
                    result.RoomNo = room.RoomNo;
                    var roomTypeEnum = HotelContext.Hotelenum.FirstOrDefault(e => e.FullKey == room.RoomType && !e.IsDel.Value);
                    result.RoomTypeName = roomTypeEnum == null ? "" : roomTypeEnum.Name;
                }
                result.Status = check.Status;
                result.ReserveTime = check.ReserveTime;
                result.PlanedCheckinTime = check.PlanedCheckinTime;
                result.CheckinTime = check.CheckinTime;
                result.PlanedCheckoutTime = check.PlanedCheckoutTime;
                result.CheckoutTime = check.CheckoutTime;
                result.Prices = check.Prices;
                result.Deposit = check.Deposit;
                result.Remark = check.Remark;

                var certTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.CertTypeEnumClass).ToList();
                result.Guests = HotelContext.Guest.Where(g => !g.IsDel.Value && g.CheckId == check.Id).Select(g => new GuestResponse()
                {
                    Id = g.Id,
                    CheckId = g.CheckId,
                    Name = g.Name,
                    CertType = g.CertType,
                    CertTypeName = certTypes.FirstOrDefault(t => t.FullKey == g.CertType) == null ? "" :
                          certTypes.FirstOrDefault(t => t.FullKey == g.CertType).Name,
                    CertId = g.CertId,
                    Mobile = g.Mobile,
                    Address = g.Address
                }).ToList();
            }
            return result;
        }
        
        /// <summary>
        /// 获取指定房间的当前状态
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public RoomStatusRespones GetRoomNowStatusByRoomId(int roomId)
        {
            RoomStatusRespones result = new RoomStatusRespones();
            Room room = HotelContext.Room.FirstOrDefault(r => r.Id == roomId && !r.IsDel.Value);
            if(room!=null)
            {
                result.RoomId = roomId;
                result.RoomNo = room.RoomNo;
                var roomTypeName = HotelContext.Hotelenum.FirstOrDefault(e => e.FullKey == room.RoomType && !e.IsDel.Value);
                result.RoomTypeName = roomTypeName == null ? "" : roomTypeName.Name;
                var check = HotelContext.Roomcheck.FirstOrDefault(o => !o.IsDel.Value && o.RoomId == room.Id && (o.Status == (int)RoomStatus.Checkin || o.Status == (int)RoomStatus.Reserved));
                if (check != null)
                {
                    result.Id = check.Id;
                    result.Status = check.Status;
                    result.ReserveTime = check.ReserveTime;
                    result.PlanedCheckinTime = check.PlanedCheckinTime;
                    result.CheckinTime = check.CheckinTime;
                    result.PlanedCheckoutTime = check.PlanedCheckoutTime;
                    result.CheckoutTime = check.CheckoutTime;
                    result.Prices = check.Prices;
                    result.Deposit = check.Deposit;
                    var certTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.CertTypeEnumClass).ToList();
                    result.Guests = HotelContext.Guest.Where(g => !g.IsDel.Value && g.CheckId == check.Id).Select(g => new GuestResponse()
                    {
                        Id = g.Id,
                        CheckId = g.CheckId,
                        Name = g.Name,
                        CertType = g.CertType,
                        CertTypeName = certTypes.FirstOrDefault(t => t.FullKey == g.CertType) == null ? "" :
                              certTypes.FirstOrDefault(t => t.FullKey == g.CertType).Name,
                        CertId = g.CertId,
                        Mobile = g.Mobile,
                        Address = g.Address
                    }).ToList();
                };
            }
            return result;
        }

        /// <summary>
        /// 根据房间ID和日期获取历史入住信息
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="date"></param>
        /// <param name="freeChenkinTime"></param>
        /// <param name="freeCheckoutTime"></param>
        /// <returns></returns>
        public RoomStatusRespones GetRoomHistoryCheckinByRoomId(int roomId, DateTime date, int freeChenkinTime, int freeCheckoutTime)
        {
            RoomStatusRespones result = new RoomStatusRespones();
            DateTime freeChenkinDateTime = new DateTime(date.Year, date.Month, date.Day, freeChenkinTime, 0, 0);
            DateTime freeCheckoutDateTime = new DateTime(date.Year, date.Month, date.Day, freeChenkinTime, 0, 0);
            Room room = HotelContext.Room.FirstOrDefault(r => r.Id == roomId && !r.IsDel.Value);
            if (room != null)
            {
                result.RoomId = roomId;
                result.RoomNo = room.RoomNo;
                var roomTypeName = HotelContext.Hotelenum.FirstOrDefault(e => e.FullKey == room.RoomType && !e.IsDel.Value);
                result.RoomTypeName = roomTypeName == null ? "" : roomTypeName.Name;

                Roomcheck check = HotelContext.Roomcheck.FirstOrDefault(
                    c => !c.IsDel.Value
                    && c.Status == (int)RoomStatus.Ckeckout
                    && ((c.CheckinTime <= freeCheckoutDateTime && c.CheckoutTime > freeCheckoutDateTime)
                         || (c.CheckinTime > freeCheckoutDateTime && c.CheckinTime < freeChenkinDateTime.AddDays(1)))
                    );

                if(check==null)
                {
                    check = HotelContext.Roomcheck.FirstOrDefault(
                    c => !c.IsDel.Value
                    && c.Status == (int)RoomStatus.Checkin
                    && ((c.CheckinTime <= freeCheckoutDateTime && c.CheckoutTime == null)
                         || (c.CheckinTime > freeCheckoutDateTime && c.CheckinTime < freeChenkinDateTime.AddDays(1)))
                    );
                }

                if (check != null)
                {
                    result.Id = check.Id;
                    result.Status = check.Status;
                    result.ReserveTime = check.ReserveTime;
                    result.PlanedCheckinTime = check.PlanedCheckinTime;
                    result.CheckinTime = check.CheckinTime;
                    result.PlanedCheckoutTime = check.PlanedCheckoutTime;
                    result.CheckoutTime = check.CheckoutTime;
                    result.Prices = check.Prices;
                    result.Deposit = check.Deposit;
                    var certTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.CertTypeEnumClass).ToList();
                    result.Guests = HotelContext.Guest.Where(g => !g.IsDel.Value && g.CheckId == check.Id).Select(g => new GuestResponse()
                    {
                        Id = g.Id,
                        CheckId = g.CheckId,
                        Name = g.Name,
                        CertType = g.CertType,
                        CertTypeName = certTypes.FirstOrDefault(t => t.FullKey == g.CertType) == null ? "" :
                              certTypes.FirstOrDefault(t => t.FullKey == g.CertType).Name,
                        CertId = g.CertId,
                        Mobile = g.Mobile,
                        Address = g.Address
                    }).ToList();
                };
            }
            return result;
        }

        /// <summary>
        /// 根据房间ID和日期获取未来预定情况
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="date"></param>
        /// <param name="freeChenkinTime"></param>
        /// <param name="freeCheckoutTime"></param>
        /// <returns></returns>
        public RoomStatusRespones GetRoomFutureReserveByRoomId(int roomId, DateTime date, int freeChenkinTime, int freeCheckoutTime)
        {
            RoomStatusRespones result = new RoomStatusRespones();
            DateTime freeChenkinDateTime = new DateTime(date.Year, date.Month, date.Day, freeChenkinTime, 0, 0);
            DateTime freeCheckoutDateTime = new DateTime(date.Year, date.Month, date.Day, freeChenkinTime, 0, 0);
            Room room = HotelContext.Room.FirstOrDefault(r => r.Id == roomId && !r.IsDel.Value);
            if (room != null)
            {
                result.RoomId = roomId;
                result.RoomNo = room.RoomNo;
                var roomTypeName = HotelContext.Hotelenum.FirstOrDefault(e => e.FullKey == room.RoomType && !e.IsDel.Value);
                result.RoomTypeName = roomTypeName == null ? "" : roomTypeName.Name;

                Roomcheck check = HotelContext.Roomcheck.FirstOrDefault(
                    c => !c.IsDel.Value
                    && c.Status == (int)RoomStatus.Reserved
                    && ((c.PlanedCheckinTime <= freeCheckoutDateTime && c.PlanedCheckoutTime > freeCheckoutDateTime)
                         || (c.PlanedCheckinTime > freeCheckoutDateTime && c.PlanedCheckinTime < freeChenkinDateTime.AddDays(1)))
                    );
                if (check == null)
                {
                    check = HotelContext.Roomcheck.FirstOrDefault(
                        c => !c.IsDel.Value
                        && c.Status == (int)RoomStatus.Checkin
                        && (c.CheckinTime <= freeCheckoutDateTime && c.PlanedCheckoutTime > freeCheckoutDateTime)
                        );
                }

                if (check != null)
                {
                    result.Id = check.Id;
                    result.Status = check.Status;
                    result.ReserveTime = check.ReserveTime;
                    result.PlanedCheckinTime = check.PlanedCheckinTime;
                    result.CheckinTime = check.CheckinTime;
                    result.PlanedCheckoutTime = check.PlanedCheckoutTime;
                    result.CheckoutTime = check.CheckoutTime;
                    result.Prices = check.Prices;
                    result.Deposit = check.Deposit;
                    var certTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.CertTypeEnumClass).ToList();
                    result.Guests = HotelContext.Guest.Where(g => !g.IsDel.Value && g.CheckId == check.Id).Select(g => new GuestResponse()
                    {
                        Id = g.Id,
                        CheckId = g.CheckId,
                        Name = g.Name,
                        CertType = g.CertType,
                        CertTypeName = certTypes.FirstOrDefault(t => t.FullKey == g.CertType) == null ? "" :
                              certTypes.FirstOrDefault(t => t.FullKey == g.CertType).Name,
                        CertId = g.CertId,
                        Mobile = g.Mobile,
                        Address = g.Address
                    }).ToList();
                };
            }
            return result;
        }

        //添加订单
        public KeyValuePair<bool,string> AddCheck(RoomStatusRespones roomcheck,string manager)
        {
            if(roomcheck.Status!= (int)RoomStatus.Reserved&&roomcheck.Status!=(int)RoomStatus.Checkin)
            {
                return new KeyValuePair<bool, string>(false, "数据错误!");
            }
            if(roomcheck.Status == (int)RoomStatus.Reserved)
            {
                if(!roomcheck.PlanedCheckinTime.HasValue)
                {
                    return new KeyValuePair<bool, string>(false, "预定房间必须要有计划入住时间!");
                }
            }
            if(roomcheck.Status == (int)RoomStatus.Checkin)
            {
                if (!roomcheck.CheckinTime.HasValue)
                {
                    return new KeyValuePair<bool, string>(false, "入住房间必须要有入住时间!");
                }
            }
            if(!roomcheck.PlanedCheckoutTime.HasValue)
            {
                return new KeyValuePair<bool, string>(false, "必须要有计划离店时间!");
            }
            if(roomcheck.Guests==null|| roomcheck.Guests.Count<=0)
            {
                return new KeyValuePair<bool, string>(false, "必须要有入住人!");
            }
            var certTypes = HotelContext.Hotelenum.Where(e => !e.IsDel.Value && e.EnumClass == DbConst.CertTypeEnumClass).ToList();
            foreach (GuestResponse g in roomcheck.Guests)
            {
                if(string.IsNullOrWhiteSpace(g.Name))
                {
                    return new KeyValuePair<bool, string>(false, "入住人存在姓名为空!");
                }
                if(string.IsNullOrWhiteSpace(g.CertId)&& roomcheck.Status == (int)RoomStatus.Checkin)
                {
                    return new KeyValuePair<bool, string>(false, "入住人存在证件号码为空!");
                }
                if(!certTypes.Any(c=>c.FullKey==g.CertType) && roomcheck.Status == (int)RoomStatus.Checkin)
                {
                    return new KeyValuePair<bool, string>(false, "入住人存在证件类型错误!");
                }
            }
            Room room = HotelContext.Room.FirstOrDefault(r => !r.IsDel.Value && r.Id == roomcheck.RoomId);
            if(room==null)
            {
                return new KeyValuePair<bool, string>(false, "数据错误!");
            }
            if (!HotelContext.Hotelmanager.Any(m => !m.IsDel.Value && m.HotelId == room.HotelId && m.WxOpenId == manager))
            {
                return new KeyValuePair<bool, string>(false, "没有管理员权限!");
            }
            Roomcheck check = new Roomcheck()
            {
                RoomId = roomcheck.RoomId,
                Status = roomcheck.Status,
                ReserveTime = roomcheck.ReserveTime,
                PlanedCheckinTime = roomcheck.PlanedCheckinTime,
                CheckinTime = roomcheck.CheckinTime,
                PlanedCheckoutTime = roomcheck.PlanedCheckoutTime,
                CheckoutTime = roomcheck.CheckoutTime,
                Prices = roomcheck.Prices,
                Deposit = roomcheck.Deposit,
                Remark = roomcheck.Remark,
                IsDel = false,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            string msg;
            if(!canReserveOrCheckin(check,out msg))
            {
                return new KeyValuePair<bool, string>(false, msg);
            }
            //add
            var tran = HotelContext.Database.BeginTransaction();
            try
            {
                
                HotelContext.Roomcheck.Add(check);
                HotelContext.SaveChanges();
                roomcheck.Id = check.Id;

                foreach (GuestResponse g in roomcheck.Guests)
                {
                    Guest guest = new Guest()
                    {
                        CheckId = check.Id,
                        Name = g.Name,
                        CertType = g.CertType,
                        CertId = g.CertId,
                        Mobile = g.Mobile,
                        Address = g.Address,
                        IsDel = false,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    };
                    HotelContext.Guest.Add(guest);
                    HotelContext.SaveChanges();
                    g.CheckId = check.Id;
                    g.Id = guest.Id;
                }
                tran.Commit();
            }
            catch(Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            return new KeyValuePair<bool, string>(true,"添加成功");
        }

        //修改订单
        public KeyValuePair<bool, string> UpdateCheck(Roomcheck check, string manager)
        {
            if(check.Id<=0)
            {
                return new KeyValuePair<bool, string>(false, "数据错误!");
            }
            if (check.Status == (int)RoomStatus.Reserved)
            {
                if (!check.PlanedCheckinTime.HasValue)
                {
                    return new KeyValuePair<bool, string>(false, "预定房间必须要有计划入住时间!");
                }
            }
            if (check.Status == (int)RoomStatus.Checkin)
            {
                if (!check.CheckinTime.HasValue)
                {
                    return new KeyValuePair<bool, string>(false, "入住房间必须要有入住时间!");
                }
            }
            if (!check.PlanedCheckoutTime.HasValue)
            {
                return new KeyValuePair<bool, string>(false, "必须要有计划离店时间!");
            }
            Room room = HotelContext.Room.FirstOrDefault(r => !r.IsDel.Value && r.Id == check.RoomId);
            if (room == null)
            {
                return new KeyValuePair<bool, string>(false, "数据错误!");
            }
            if (!HotelContext.Hotelmanager.Any(m => !m.IsDel.Value && m.HotelId == room.HotelId && m.WxOpenId == manager))
            {
                return new KeyValuePair<bool, string>(false, "没有管理员权限!");
            }
            string msg;
            if(!canReserveOrCheckin(check,out msg))
            {
                return new KeyValuePair<bool, string>(false, msg);
            }
            //update
            check.UpdateTime = DateTime.Now;
            base.Update(check, "RoomId", "Status", "ReserveTime", "PlanedCheckinTime", "CheckinTime", "PlanedCheckoutTime", "CheckoutTime", "Prices", "Deposit", "Remark", "UpdateTime");
            return new KeyValuePair<bool, string>(true, "更新成功!");
        }

        //删除订单
        public KeyValuePair<bool, string> DeleteCheck(Roomcheck check, string manager)
        {
            if (check.Id <= 0)
            {
                return new KeyValuePair<bool, string>(false, "数据错误!");
            }
            Room room = HotelContext.Room.FirstOrDefault(r => !r.IsDel.Value && r.Id == check.RoomId);
            if (room == null)
            {
                return new KeyValuePair<bool, string>(false, "数据错误!");
            }
            if (!HotelContext.Hotelmanager.Any(m => !m.IsDel.Value && m.HotelId == room.HotelId && m.WxOpenId == manager))
            {
                return new KeyValuePair<bool, string>(false, "没有管理员权限!");
            }
            List<Guest> guests = HotelContext.Guest.Where(g => g.CheckId == check.Id && !g.IsDel.Value).ToList();
            var tran = HotelContext.Database.BeginTransaction();
            try
            {
                foreach(Guest g in guests)
                {
                    g.IsDel = true;
                    g.UpdateTime = DateTime.Now;
                }
                check.IsDel = true;
                check.UpdateTime = DateTime.Now;
                base.Update(check, "IsDel", "UpdateTime");
                tran.Commit();
            }
            catch(Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            return new KeyValuePair<bool, string>(true, "取消成功!");
        }

        //离店
        public KeyValuePair<bool,string> Checkout(Roomcheck check,string manager)
        {
            if (check.Id <= 0)
            {
                return new KeyValuePair<bool, string>(false, "数据错误!");
            }
            Room room = HotelContext.Room.FirstOrDefault(r => !r.IsDel.Value && r.Id == check.RoomId);
            if (room == null)
            {
                return new KeyValuePair<bool, string>(false, "数据错误!");
            }
            if (!HotelContext.Hotelmanager.Any(m => !m.IsDel.Value && m.HotelId == room.HotelId && m.WxOpenId == manager))
            {
                return new KeyValuePair<bool, string>(false, "没有管理员权限!");
            }

            Roomcheck oldCheck = HotelContext.Roomcheck.Find(check.Id);
            if(oldCheck.Status!=(int)RoomStatus.Checkin)
            {
                return new KeyValuePair<bool, string>(false, "数据错误!");
            }

            oldCheck.Status = (int)RoomStatus.Ckeckout;
            oldCheck.CheckoutTime = check.CheckoutTime;
            oldCheck.UpdateTime = DateTime.Now;
            HotelContext.SaveChanges();

            return new KeyValuePair<bool, string>(true, "离店成功!");
        }

        /// <summary>
        /// 判断是否可以预定或者入住
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        private bool canReserveOrCheckin(Roomcheck check,out string msg)
        {
            msg = null;
            DateTime beginTime; 
            if(check.Status== (int)RoomStatus.Reserved)
            {
                beginTime = check.PlanedCheckinTime.Value;
            }
            else
            {
                beginTime = check.CheckinTime.Value;
            }

            if(check.Status == (int)RoomStatus.Reserved|| check.Status == (int)RoomStatus.Checkin)
            {
                //判断在预定或者入住时间里是否已有预定或者入住的订单
                var existReserve = HotelContext.Roomcheck.FirstOrDefault(c => c.RoomId == check.RoomId
                                                                           && c.Id != check.Id
                                                                           && c.Status == (int)RoomStatus.Reserved
                                                                           && !c.IsDel.Value
                                                                           && c.PlanedCheckinTime < check.PlanedCheckoutTime
                                                                           && c.PlanedCheckoutTime > beginTime);
                if (existReserve!=null)
                {
                    msg = $"已经有人预定，时间从{ existReserve.PlanedCheckinTime }到{ existReserve.PlanedCheckoutTime }";
                    return false;
                }
                else
                {
                    var existCheckin = HotelContext.Roomcheck.FirstOrDefault(c => c.RoomId == check.RoomId
                                                                               && c.Id != check.Id
                                                                               && c.Status == (int)RoomStatus.Checkin
                                                                               && !c.IsDel.Value
                                                                               && c.CheckinTime < check.PlanedCheckoutTime
                                                                               && c.PlanedCheckoutTime > beginTime);
                    if (existCheckin != null)
                    {
                        msg = $"已经有人入住，时间从{ existCheckin.CheckinTime }到{ existCheckin.PlanedCheckoutTime }";
                        return false;
                    }
                    else
                    {
                        var existCheckout = HotelContext.Roomcheck.FirstOrDefault(c => c.RoomId == check.RoomId
                                                                                    && c.Id != check.Id
                                                                                    && c.Status == (int)RoomStatus.Ckeckout
                                                                                    && !c.IsDel.Value
                                                                                    && c.CheckinTime < check.CheckoutTime
                                                                                    && c.PlanedCheckoutTime > beginTime);
                        if (existCheckout != null)
                        {
                            msg = $"已经有人入住过，时间从{ existCheckin.CheckinTime }到{ existCheckin.PlanedCheckoutTime }";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                return true;
            }
        }
    }
}
