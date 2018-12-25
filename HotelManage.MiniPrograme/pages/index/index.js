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
            this.setData({
              rooms: res.data.data
            })
          }else{
            wx.showToast({
              title: '网络问题，请稍后再试',
              icon: 'none'
            });
          }
        },
        fail:res=>{
          wx.showToast({
            title: '网络问题，请稍后再试',
            icon: 'none'
          });
        }
      })
      
    } else {
      app.globalDataReadyCallback=d=>{
        if(d.hotel){
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
})
