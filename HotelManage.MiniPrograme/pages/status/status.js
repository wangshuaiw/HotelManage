// pages/status/status.js
var util = require('../../utils/util.js');
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    hotel: {},
    date:null,
    room:{},
    isEdit:false,
    certTypes:[],
    selectCertType:null,
    planedCheckinDate:null,
    planedCheckinTime:null,
    planedCheckoutDate:null,
    planedCheckoutTime:null
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    if (app.globalData.hotel){
      this.setData({
        hotel: app.globalData.hotel,
        date:options.date
      });
      //获取房间信息
      wx.request({
        url: app.globalData.url + 'RoomCheck/GetRoomStatus',
        method: 'GET',
        header: {
          "Authorization": "Bearer " + app.globalData.token.token
        },
        data: {
          roomId: options.roomId,
          checkId:options.id,
          date:options.date
        },
        success: res => {
          if (res.data && res.data.status && res.data.status == 1 && res.data.data) {
            this.setData({
              room: res.data.data
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
      //获取证件类型
      wx.request({
        url: app.globalData.url + 'HotelEnum/GetCertTypes',
        method: 'GET',
        header: {
          "Authorization": "Bearer " + app.globalData.token.token
        },
        success: res => {
          if (res.data && res.data.status && res.data.status == 1 && res.data.data) {
            var types = new Array();
            var selectType;
            for (var i = 0; i < res.data.data.length; i++) {
              var thisType = { "key": res.data.data[i].fullKey, "value": res.data.data[i].name };
              types.push(thisType);
              if (thisType.key =="CertType/IdCard"){
                selectType = thisType;
              }
            }
            this.setData({
              certTypes: types,
              selectCertType: selectType,
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
    }else{
      wx.showToast({
        title: '系统问题，请重启小程序',
        icon: 'none'
      });
    }
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {
  
  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {
  
  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {
  
  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {
  
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {
  
  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {
  
  },

  btnReserve:function(e){
    this.data.room.status=1;
    this.data.room.guests = [{ isEdit:true}]
    var defaultPlanedCheckoutDate = new Date(this.data.date);
    defaultPlanedCheckoutDate.setDate(defaultPlanedCheckoutDate.getDate() + 1);
    this.setData({
      room:this.data.room,
      isEdit:true,
      planedCheckinDate: this.data.date,
      planedCheckinTime: util.formatOnlyHourMinite(new Date()),
      planedCheckoutDate: util.formatDate(defaultPlanedCheckoutDate),
      planedCheckoutTime:"12:00"
    })
  },
  saveGuest:function(e){
    //console.log(e);
    if (!e.detail.value.name){
      wx.showToast({
        title: '请填写姓名',
        icon: 'none'
      })
      return;
    }
    if (!e.detail.value.certId){
      wx.showToast({
        title: '请填写证件号码',
        icon: 'none'
      })
      return;
    }

    var i = e.target.dataset.index;
    if (this.data.room.id && this.data.room.id>0){
      //原有订单
      if (this.data.room.guests[i].id && this.data.room.guests[i].id>0){
        //原有入住人，修改
        wx.request({
          url: app.globalData.url + 'Guest/Update',
          method: 'POST',
          header: {
            "Authorization": "Bearer " + app.globalData.token.token
          },
          data: {
            id: this.data.room.guests[i].id,
            checkId: this.data.room.id,
            name: e.detail.value.name,
            certType: this.data.selectCertType.key,
            certId: e.detail.value.certId,
            mobile: e.detail.value.mobile
          },
          success: res => {
            if (res.data && res.data.status && res.data.status == 1) {
              this.data.room.guests[i].name = e.detail.value.name;
              this.data.room.guests[i].certType = this.data.selectCertType.key;
              this.data.room.guests[i].certTypeName = this.data.selectCertType.value;
              this.data.room.guests[i].certId = e.detail.value.certId;
              this.data.room.guests[i].mobile = e.detail.value.mobile;
              this.data.room.guests[i].isEdit = false;
              this.setData({
                room: this.data.room
              });
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
      }else{
        //添加新入住人
        wx.request({
          url: app.globalData.url + 'Guest/Add',
          method: 'POST',
          header: {
            "Authorization": "Bearer " + app.globalData.token.token
          },
          data: {
            checkId: this.data.room.id,
            name: e.detail.value.name,
            certType: this.data.selectCertType.key,
            certId: e.detail.value.certId,
            mobile: e.detail.value.mobile
          },
          success: res => {
            if (res.data && res.data.status && res.data.status == 1 && res.data.data) {
              this.data.room.guests[i].id = res.data.data.Id
              this.data.room.guests[i].name = e.detail.value.name;
              this.data.room.guests[i].certType = this.data.selectCertType.key;
              this.data.room.guests[i].certTypeName = this.data.selectCertType.value;
              this.data.room.guests[i].certId = e.detail.value.certId;
              this.data.room.guests[i].mobile = e.detail.value.mobile;
              this.data.room.guests[i].isEdit = false;
              this.setData({
                room: this.data.room
              });
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
    }else{
      //新增订单
      this.data.room.guests[i].name = e.detail.value.name;
      this.data.room.guests[i].certType = this.data.selectCertType.key;
      this.data.room.guests[i].certTypeName = this.data.selectCertType.value;
      this.data.room.guests[i].certId = e.detail.value.certId;
      this.data.room.guests[i].mobile = e.detail.value.mobile;
      this.data.room.guests[i].isEdit = false;
      this.setData({
        room: this.data.room
      });
    }
  },
  btnCancel:function(e){
    //console.log(e);
    var i = e.target.dataset.index;
    if (this.data.room.guests[i].id > 0){
      this.data.room.guests[i].isEdit = false;
    }else{
      this.data.room.guests.splice(index, 1);
    }
    this.setData({
      room:this.data.room
    })
  },
  addGuest:function(e){
    for (var i = 0; i < this.data.room.guests.length; i++) {
      if (this.data.room.guests[i].isEdit == true) {
        wx.showToast({
          title: '请先保存正在编辑的入住人',
          icon: 'none'
        })
        return
      }
    }
    this.data.room.guests.push({isEdit:true}),
    this.setData({
      room:this.data.room
    })
  },
  saveCheck:function(e){
    for (var i = 0; i < this.data.room.guests.length; i++) {
      if (this.data.room.guests[i].isEdit == true) {
        wx.showToast({
          title: '请先保存正在编辑的入住人',
          icon: 'none'
        })
        return;
      }
    }
    if (this.data.room.guests.length<=0){
      wx.showToast({
        title: '请先添加入住人',
        icon: 'none'
      })
      return;
    }
    if (!e.detail.value.prices || e.detail.value.prices<=0){
      wx.showToast({
        title: '请先填写价格',
        icon: 'none'
      })
      return;
    }
    this.data.room.reserveTime = this.data.room.status == 1 ? util.formatTime(new Date()) : null;
    this.data.room.planedCheckinTime = this.data.planedCheckinDate + ' ' + this.data.planedCheckinTime;
    this.data.room.checkinTime = this.data.room.status == 2 ? util.formatTime(new Date()) : null;
    this.data.room.planedCheckoutTime = this.data.planedCheckoutDate + ' ' + this.data.planedCheckoutTime;
    this.data.room.prices = e.detail.value.prices;
    this.data.room.deposit = e.detail.value.deposit;
    this.data.room.remark = e.detail.value.remark;
    wx.request({
      url: app.globalData.url + 'RoomCheck/ModifyRoomCheck',
      method: 'POST',
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: this.data.room,
      success: res => {
        if (res.data && res.data.status && res.data.status == 1 && res.data.data) {
          this.data.room.id = res.data.data.id;
          this.setData({
            room: this.data.room,
            isEdit:false
          });
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

  certTypeChange:function(e){
    this.setData({
      selectCertType: this.data.certTypes[e.detail.value]
    })
  },
  planedCheckinDateChange:function(e){
    this.setData({
      planedCheckinDate: e.detail.value
    })
  },
  planedCheckinTimeChange:function(e){
    this.setData({
      planedCheckinTime: e.detail.value
    })
  },
  planedCheckoutDateChange:function(e){
    this.setData({
      planedCheckoutDate:e.detail.value
    })
  },
  planedCheckoutTimeChange:function(e){
    this.setData({
      planedCheckoutTime:e.detail.value
    })
  }
})