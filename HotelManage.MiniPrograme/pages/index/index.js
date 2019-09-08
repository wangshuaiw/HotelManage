//index.js
var util = require('../../utils/util.js');
var eventH = require('../../utils/eventHelp.js');
const app = getApp()

Page({
  data: {
    hotel: {},
    hasHotel: false,
    date:null,
    rooms:[],
    showCheckoutModal: false,
    checkoutRoom:null,
    checkoutDate:null,
    checkoutTime:null,
    pullDownRefresh:false
  },
  onLoad: function () {
    var sysDate = util.formatDate(new Date());
    this.setData({
      date: sysDate,
    });
  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
    if (app.globalData.hotel) {
      this.setData({
        hotel: app.globalData.hotel,
        hasHotel: true
      })
      //获取房间及现状的状态
      this.getRoomsStatus(app.globalData.hotel.id)
    } else {
      app.globalDataReadyCallback = d => {
        if (d.hotel) {
          this.setData({
            hotel: d.hotel,
            hasHotel: true
          })
          //获取房间及现状的状态
          this.getRoomsStatus(d.hotel.id)
        }
      }
    }
  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {
    this.data.pullDownRefresh = true;
    this.getRoomsStatus(this.data.hotel.id);
  },

  dateChange:function(e){
    this.setData({
      date: e.detail.value
    })
    this.getRoomsStatus(this.data.hotel.id);
  },

  getRoomsStatus:function(hotelId){
    //获取房间历史入住，当前，未来预定的状态
    wx.request({
      url: app.globalData.url + 'RoomCheck/GetRoomsStatus',
      method: 'GET',
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: {
        hotelId: hotelId,
        date: this.data.date
      },
      success: res => {
        if (res.data && res.data.status && res.data.status == 1 && res.data.data) {
          this.formatRoomTime(res.data.data);
          this.setData({
            rooms: res.data.data
          })
        } else {
          wx.showToast({
            title: '网络问题，请稍后再试',
            icon: 'none',
            duration:'3000'
          });
        }
      },
      fail: res => {
        wx.showToast({
          title: '网络问题，请稍后再试',
          icon: 'none',
          duration: '3000'
        });
      },
      complete:()=>{
        if(this.data.pullDownRefresh){
          wx.stopPullDownRefresh();
          this.data.pullDownRefresh=false;
        }
      }
    })
  },

  formatRoomTime:function(rooms){
    for (var i = 0; i < rooms.length; i++) {
      if (rooms[i].status == 1) {
        var checkoutTime = new Date(rooms[i].planedCheckoutTime);
        var checkinTime = new Date(rooms[i].planedCheckinTime);
        rooms[i].night = parseInt((new Date(checkoutTime.getFullYear(), checkoutTime.getMonth(), checkoutTime.getDate()).getTime() - new Date(checkinTime.getFullYear(), checkinTime.getMonth(), checkinTime.getDate()).getTime()) / (1000 * 60 * 60 * 24));
      } else if (rooms[i].status == 2){
        var checkoutTime = new Date(rooms[i].planedCheckoutTime);
        var checkinTime = new Date(rooms[i].checkinTime);
        rooms[i].night = parseInt((new Date(checkoutTime.getFullYear(), checkoutTime.getMonth(), checkoutTime.getDate()).getTime() - new Date(checkinTime.getFullYear(), checkinTime.getMonth(), checkinTime.getDate()).getTime()) / (1000 * 60 * 60 * 24));
      } else if (rooms[i].status == 3) {
        var checkoutTime = new Date(rooms[i].checkoutTime);
        var checkinTime = new Date(rooms[i].checkinTime);
        rooms[i].night = parseInt((new Date(checkoutTime.getFullYear(), checkoutTime.getMonth(), checkoutTime.getDate()).getTime() - new Date(checkinTime.getFullYear(), checkinTime.getMonth(), checkinTime.getDate()).getTime()) / (1000 * 60 * 60 * 24));
      }
      if (rooms[i].reserveTime) {
        rooms[i].reserveTime = util.formatYueRi(new Date(rooms[i].reserveTime));
      }
      if (rooms[i].planedCheckinTime) {
        rooms[i].planedCheckinTime = util.formatYueRi(new Date(rooms[i].planedCheckinTime));
      }
      if (rooms[i].checkinTime) {
        rooms[i].checkinTime = util.formatYueRi(new Date(rooms[i].checkinTime));
      }
      if (rooms[i].planedCheckoutTime) {
        rooms[i].planedCheckoutTime = util.formatYueRi(new Date(rooms[i].planedCheckoutTime));
      }
      if (rooms[i].checkoutTime) {
        rooms[i].checkoutTime = util.formatYueRi(new Date(rooms[i].checkoutTime));
      }
      
    }
  },

  tapRoom:function(e){
    eventH.doubleTap(e, e, (e) => {
      var item = e.currentTarget.dataset.item;
      this.navigateToDetail(item);
    })
  },
  longPressRoom:function(e){
    var item = e.currentTarget.dataset.item;
    if (item.status == 1){
      wx.showActionSheet({
        itemList: ['详细信息', '登记入住','取消预定'],
        success: res => {
          if (res.tapIndex == 0) {
            this.navigateToDetail(item);
          } else if (res.tapIndex == 1) {
            this.navigateToDetail(item);
          } else if (res.tapIndex == 2){
            this.cancelReserve(item);
          }
        }
      })
    } else if (item.status == 2){
      wx.showActionSheet({
        itemList: ['详细信息', '离店'],
        success: res => {
          if (res.tapIndex == 0) {
            this.navigateToDetail(item);
          } else if (res.tapIndex == 1) {
            this.data.checkoutRoom = item;
            this.showCheckout('open');
          }
        }
      })
    } else if (item.status == 3){
      wx.showActionSheet({
        itemList: ['详细信息'],
        success: res => {
          if (res.tapIndex == 0) {
            this.navigateToDetail(item);
          } 
        }
      })
    } else{
      wx.showActionSheet({
        itemList: ['登记预定', '登记入住'],
        success: res => {
          if (res.tapIndex == 0) {
            this.navigateToDetail(item);
          } else if (res.tapIndex == 1) {
            this.navigateToDetail(item);
          }
        }
      })
    }
    
  },
  tapReserve:function(e){
    wx.navigateTo({
      url: '/pages/order/order?type=1&date=' + this.data.date,
    })
  },
  tapCheckin:function(e){
    wx.navigateTo({
      url: '/pages/order/order?type=2&date=' + this.data.date,
    })
  },
  navigateToDetail:function(item){
    
    if(item.id&&item.id>0){
      wx.navigateTo({
        url: '/pages/order/order?id=' + item.id,
      })
    }else{
      wx.navigateTo({
        url: '/pages/order/order?roomId=' + item.roomId + '&date=' + this.data.date,
      })
    }
    /*
    wx.navigateTo({
      url: '/pages/status/status?id=' + item.id + '&roomId=' + item.roomId + '&date' + this.data.date,
    })
    */
  },

  tapPowerDrawer: function (e) {
    var currentStatu = e.currentTarget.dataset.statu;
    this.showCheckout(currentStatu)
  }, 

  showCheckout: function (currentStatu) {

    var animation = wx.createAnimation({
      duration: 200, 
      timingFunction: "linear", 
      delay: 0 
    }); 
    this.animation = animation;
    animation.opacity(0).rotateX(-100).step();
    this.setData({
      animationData: animation.export()
    })
    setTimeout(function () {
      animation.opacity(1).rotateX(0).step();
      this.setData({
        animationData: animation
      }) 
      
      if (currentStatu == "close") {
        this.setData(
          {
            showCheckoutModal: false
          }
        );
      }
    }.bind(this), 200) 
    
    if (currentStatu == "open") {
      var sysDate = util.formatDate(new Date());
      var sysTime = util.formatOnlyHourMinite(new Date())
      this.setData(
        {
          showCheckoutModal: true,
          checkoutDate: sysDate,
          checkoutTime: sysTime,
        }
      );
    }
  },
  checkoutDateChange:function(e){
    this.setData({
      checkoutDate: e.detail.value
    })
  },
  checkoutTimeChange: function (e) {
    this.setData({
      checkoutTime: e.detail.value
    })
  },
  tapCheckout: function(e) {
    wx.request({
      url: app.globalData.url + 'RoomCheck/Checkout',
      method: "POST",
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: {
        id: this.data.checkoutRoom.id,
        roomId: this.data.checkoutRoom.roomId,
        checkoutTime: this.data.checkoutDate + ' ' + this.data.checkoutTime
      },
      success: r => {
        if (r.statusCode == 200 && r.data) {
          if (r.data.status == 1) {
            this.getRoomsStatus(this.data.hotel.id);
          } else if (r.data.status == -2) {
            wx.showToast({
              title: r.data.massage,
              icon: 'none',
              duration: 3000
            });
          } else {
            wx.showToast({
              title: '网络问题，请稍后再试',
              icon: 'none',
              duration: 3000
            });
          }
        } else {
          wx.showToast({
            title: '网络问题，请稍后再试',
            icon: 'none',
            duration: 3000
          });
        }
      },
      fail: r => {
        wx.showToast({
          title: '网络问题，请稍后再试',
          icon: 'none',
          duration: 3000
        });
      },
      complete:r=>{
        this.showCheckout("close");
      }
    })
  },
  cancelReserve:function(item){
    wx.showModal({
      title: '确定取消订单！',
      content: '确定取消订单！',
      success: res => {
        if (res.confirm) {
          wx.request({
            url: app.globalData.url + 'RoomCheck/DeleteCheck',
            method: "POST",
            header: {
              "Authorization": "Bearer " + app.globalData.token.token
            },
            data: {
              id: item.id,
              roomId: item.roomId,
            },
            success: r => {
              if (r.statusCode == 200 && r.data) {
                if (r.data.status == 1) {
                  this.getRoomsStatus(this.data.hotel.id);
                } else if (r.data.status == -2) {
                  wx.showToast({
                    title: r.data.massage,
                    icon: 'none',
                    duration: 3000
                  });
                } else {
                  wx.showToast({
                    title: '网络问题，请稍后再试',
                    icon: 'none',
                    duration: 3000
                  });
                }
              } else {
                wx.showToast({
                  title: '网络问题，请稍后再试',
                  icon: 'none',
                  duration: 3000
                });
              }
            },
            fail: r => {
              wx.showToast({
                title: '网络问题，请稍后再试',
                icon: 'none',
                duration: 3000
              });
            }
          })
        } else if (res.cancel) {
          //console.log('用户点击取消')
        }
      }
    })
  }
})
