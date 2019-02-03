//index.js
var util = require('../../utils/util.js');
const app = getApp()

Page({
  data: {
    hotel: {},
    hasHotel: false,
    date:null,
    rooms:[],
  },
  onLoad: function () {
    var sysDate = util.formatDate(new Date());
    this.setData({
      date: sysDate
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
      wx.request({
        url: app.globalData.url + 'RoomCheck/GetRoomsStatus',
        method: 'GET',
        header: {
          "Authorization": "Bearer " + app.globalData.token.token
        },
        data: {
          hotelId: app.globalData.hotel.id
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
              icon: 'none'
            });
          }
        },
        fail: res => {
          wx.showToast({
            title: '网络问题，请稍后再试',
            icon: 'none'
          });
        }
      })

    } else {
      app.globalDataReadyCallback = d => {
        if (d.hotel) {
          this.setData({
            hotel: d.hotel,
            hasHotel: true
          })
          //获取房间及现状的状态
          wx.request({
            url: app.globalData.url + 'RoomCheck/GetRoomsStatus',
            method: 'GET',
            header: {
              "Authorization": "Bearer " + app.globalData.token.token
            },
            data: {
              hotelId: d.hotel.id
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
                  icon: 'none'
                });
              }
            },
            fail: res => {
              wx.showToast({
                title: '网络问题，请稍后再试',
                icon: 'none'
              });
            }
          })
        }
      }
    }
  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {
    this.onLoad();
    this.onShow();
  },

  dateChange:function(e){
    this.setData({
      date: e.detail.value
    })

    //获取房间历史入住，当前，未来预定的状态
    wx.request({
      url: app.globalData.url + 'RoomCheck/GetRoomsStatus',
      method: 'GET',
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: {
        hotelId: this.data.hotel.id,
        date:this.data.date
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
            icon: 'none'
          });
        }
      },
      fail: res => {
        wx.showToast({
          title: '网络问题，请稍后再试',
          icon: 'none'
        });
      }
    })
  },

  formatRoomTime:function(rooms){
    for (var i = 0; i < rooms.length; i++) {
      if (rooms[i].reserveTime) {
        rooms[i].reserveTime = util.formatTimeNoSecond(new Date(rooms[i].reserveTime));
      }
      if (rooms[i].planedCheckinTime) {
        rooms[i].planedCheckinTime = util.formatTimeNoSecond(new Date(rooms[i].planedCheckinTime));
      }
      if (rooms[i].checkinTime) {
        rooms[i].checkinTime = util.formatTimeNoSecond(new Date(rooms[i].checkinTime));
      }
      if (rooms[i].planedCheckoutTime) {
        rooms[i].planedCheckoutTime = util.formatTimeNoSecond(new Date(rooms[i].planedCheckoutTime));
      }
      if (rooms[i].checkoutTime) {
        rooms[i].checkoutTime = util.formatTimeNoSecond(new Date(rooms[i].checkoutTime));
      }
    }
  }
})
