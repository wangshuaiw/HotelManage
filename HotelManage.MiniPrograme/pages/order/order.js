// pages/order/order.js
const app = getApp();
var util = require('../../utils/util.js');
Page({

  /**
   * 页面的初始数据
   */
  data: {
    type:0, //1-预定；2-入住
    id: 0,
    status:0,
    showDatePicker:false,
    beginDate: new Date(),
    endDate: new Date(new Date().setDate(new Date().getDate() + 1)),
    beginYear:null,
    beginMonth:null,
    beginDay:null,
    endYear:null,
    endMonth:null,
    endDay:null,
    bookinNight:1,
    rooms:null,
    loadingRoomsStatus:'loading',
    showRoomSelect:false,
    selectRoom:[],
    hasSelectRooms:false,
    price:0.00,
    deposit:0.00,
    guests:null,
    saving:false,
    remark:null,
    showCamera:false,
    isBackDevicePosition:true
    //toView: 'red',
    //scrollTop: 100
  },

  myData:{
    cameraForGuestIndex:-1
  },
  
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    if (!app.globalData.hotel){
      wx.showToast({
        title: '系统问题，请重启小程序',
        icon: 'none',
        duration:3000
      });
    }
    if (options.type){
      this.setData({
        type: options.type
      })
    }
    if(options.id){
      //this.data.id = options.id
      this.getRoomCheck(options.id);
    } else{
      this.data.beginDate = new Date();
      this.data.endDate = new Date(new Date().setDate(new Date().getDate() + 1));
      if (options.date) {
        this.data.beginDate = new Date(options.date);
        this.data.endDate = new Date(new Date(options.date).setDate(new Date(options.date).getDate() + 1));
      }
      this.setData({
        beginYear: this.data.beginDate.getFullYear(),
        beginMonth: this.data.beginDate.getMonth() + 1,
        beginDay: this.data.beginDate.getDate(),
        endYear: this.data.endDate.getFullYear(),
        endMonth: this.data.endDate.getMonth() + 1,
        endDay: this.data.endDate.getDate(),
        bookinNight: parseInt((this.data.endDate.getTime() - this.data.beginDate.getTime()) / (1000 * 60 * 60 * 24))
      })
      //预定或者入住按钮进入时添加初始化第一个客人
      if (!options.id) {
        this.setData({
          guests: [{ id: 0, name: "", gender: "0", mobile: "", certId: "", address: "" }]
        })
      }

      if (options.roomId) {
        this.getRooms(options.roomId);
      }
    }
    this.ctx = wx.createCameraContext();
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

  getRoomCheck:function(checkId){
    wx.request({
      url: app.globalData.url + 'RoomCheck/GetRoomCheck',
      method: 'GET',
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: {
        checkId: checkId
      },
      success: res => {
        if (res.data && res.data.status && res.data.status == 1 && res.data.data) {
          if (res.data.data.status==1){ //预定
            this.data.beginDate = new Date(res.data.data.planedCheckinTime);
            this.data.endDate = new Date(res.data.data.planedCheckoutTime);
          } else if (res.data.data.status == 2){ //入住
            this.data.beginDate = new Date(res.data.data.checkinTime);
            this.data.endDate = new Date(res.data.data.planedCheckoutTime)
          } else if (res.data.data.status == 3){ //离店（历史订单）
            this.data.beginDate = new Date(res.data.data.checkinTime);
            this.data.endDate = new Date(res.data.data.checkoutTime)
          }
          var night = parseInt((new Date(this.data.endDate.getFullYear(), this.data.endDate.getMonth(), this.data.endDate.getDate()).getTime() - new Date(this.data.beginDate.getFullYear(), this.data.beginDate.getMonth(), this.data.beginDate.getDate()).getTime()) / (1000 * 60 * 60 * 24));
          this.data.selectRoom = {
            id: res.data.data.roomId,
            roomNo: res.data.data.roomNo,
            roomTypeName: res.data.data.roomTypeName,
            status: res.data.data.status,
            selected:true,
          }
          this.setData({
            id: checkId,
            status: res.data.data.status,
            beginDate: this.data.beginDate,
            endDate:this.data.endDate,
            selectRoom: this.data.selectRoom,
            hasSelectRooms:true,
            price: res.data.data.prices,
            deposit: res.data.data.deposit,
            guests: res.data.data.guests,
            remark: res.data.data.remark,
            beginYear: this.data.beginDate.getFullYear(),
            beginMonth: this.data.beginDate.getMonth() + 1,
            beginDay: this.data.beginDate.getDate(),
            endYear: this.data.endDate.getFullYear(),
            endMonth: this.data.endDate.getMonth() + 1,
            endDay: this.data.endDate.getDate(),
            bookinNight: night
          })
        } else {
          wx.showModal({
            title: '加载失败',
            content: '网络问题，请稍后再试',
            showCancel: false,
          })
        }
      },
      fail: res => {
        wx.showModal({
          title: '加载失败',
          content: '网络问题，请稍后再试',
          showCancel: false,
        })
      }
    })
  },

  tapDatepicker:function(e){
    this.setData({
      showDatePicker: true
    })
  },
  hideDatePicker:function(){
    if(this.data.endDate){
      this.setData({
        showDatePicker: false
      })
    }
  },

  tapDate(e) {
    var calendar = this.selectComponent('#myCalendar');
    var date = e.detail.item.fullDate;
    if(this.data.endDate){
      this.data.beginDate = date;
      this.data.endDate = null;
      calendar.setSelectDate(e.detail.index, 1);
    } else if (date > this.data.beginDate){
      this.data.endDate = date;
      calendar.setSelectDate(e.detail.index, 2);
      this.setData({
        showDatePicker:false,
        beginYear: this.data.beginDate.getFullYear(),
        beginMonth: this.data.beginDate.getMonth() + 1,
        beginDay: this.data.beginDate.getDate(),
        endYear: this.data.endDate.getFullYear(),
        endMonth: this.data.endDate.getMonth() + 1,
        endDay: this.data.endDate.getDate(),
        bookinNight: parseInt((this.data.endDate.getTime() - this.data.beginDate.getTime()) / (1000 * 60 * 60 * 24))
      })
    }else{
      this.data.beginDate = date;
      this.data.endDate = null;
      calendar.setSelectDate(e.detail.index, 1);
    }
  },

  none:function(){
  },

  showRoomSelect:function(e){
    if (this.data.rooms && this.data.rooms.beginDate == this.data.beginDate && this.data.rooms.endDate == this.data.endDate
      && (new Date() - this.data.rooms.getTime < 1000 * 60)) {
      //1分钟内获取过不再获取
      this.setData({
        showRoomSelect: true
      })
      return;
    }
    this.setData({
      showRoomSelect: true,
      loadingRoomsStatus:'loading',
    })
    this.getRooms();
  },
  getRooms:function(roomId){
    var beginTime = new Date(this.data.beginDate.getFullYear(), this.data.beginDate.getMonth(), this.data.beginDate.getDate(), 12);
    var endTime = new Date(this.data.endDate.getFullYear(), this.data.endDate.getMonth(), this.data.endDate.getDate(), 12);
    if (this.data.type == 2) {
      var nowTime = new Date();
      beginTime = new Date(this.data.beginDate.getFullYear(), this.data.beginDate.getMonth(), this.data.beginDate.getDate(),
        nowTime.getHours(), nowTime.getMinutes(), nowTime.getSeconds());
    }
    wx.request({
      url: app.globalData.url + 'RoomCheck/GetRoomsStatusBasicInfo',
      method: 'GET',
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: {
        hotelId: app.globalData.hotel.id,
        beginTime: util.formatTime(beginTime),
        endTime: util.formatTime(endTime),
        checkId:this.data.id
      },
      success: res => {
        if (res.data && res.data.status && res.data.status == 1 && res.data.data) {
          var newRooms = res.data.data;
          for (var i = 0; i < newRooms.length; i++) {
            newRooms[i].selected = false;
            if (this.data.selectRoom && this.data.selectRoom.id == newRooms[i].id) {
              newRooms[i].selected = true;
            } else if (roomId && roomId == newRooms[i].id){
              newRooms[i].selected = true;
              this.setData({
                selectRoom: {
                  id: newRooms[i].id,
                  roomNo: newRooms[i].roomNo,
                  roomTypeName: newRooms[i].roomTypeName,
                  status: newRooms[i].status,
                  selected: true,
                },
                hasSelectRooms: true
              })
            }
          }
          this.setData({
            rooms: {
              getTime: new Date(),
              beginDate: this.data.beginDate,
              endDate: this.data.endDate,
              rooms: newRooms
            },
            loadingRoomsStatus: 'success'
          })
        } else {
          this.setData({
            loadingRoomsStatus: 'fail'
          })
        }
      },
      fail: res => {
        this.setData({
          loadingRoomsStatus: 'fail'
        })
      }
    })
  },

  hideRoomSelect:function(e){
    if(this.data.selectRoom){
      this.setData({
        showRoomSelect: false
      });
    }
    //this.setSelectRooms();
  },
  selectRoom:function(e){
    var index = e.currentTarget.dataset.index;
    if (this.data.rooms.rooms[index].status == 1 || this.data.rooms.rooms[index].status==2){
      return;
    }
    for (var i = 0; i < this.data.rooms.rooms.length; i++){
      if (this.data.rooms.rooms[i].selected){
        this.data.rooms.rooms[i].selected = false;
      }
    }
    this.data.rooms.rooms[index].selected = true;
    this.setData({
      rooms:this.data.rooms,
      selectRoom: this.data.rooms.rooms[index],
      hasSelectRooms:true,
      showRoomSelect:false
    })
    //this.setSelectRooms();
  },
  /*
  setSelectRooms:function(){
    var selectRooms = [];
    for (var i = 0; i < this.data.rooms.rooms.length; i++) {
      if (this.data.rooms.rooms[i].selected) {
        selectRooms.push(this.data.rooms.rooms[i]);
      }
    }
    this.setData({
      selectRooms: selectRooms,
      hasSelectRooms: selectRooms.length > 0
    })
  },
  */
  /*
  deleteSelectRoom:function(e){
    var index =e.target.dataset.index;
    var item = this.data.selectRooms.splice(index, 1)[0];
    for (var i = 0; i < this.data.rooms.rooms.length; i++) {
      if (this.data.rooms.rooms[i].id == item.id) {
        this.data.rooms.rooms[i].selected =false;
      }
    }
    this.setData({
      rooms: this.data.rooms,
      selectRooms: this.data.selectRooms,
      hasSelectRooms: this.data.selectRooms.length>0
    });
  },
  */
  onInput:function(e){
    var index = e.currentTarget.dataset.index;
    var field = e.currentTarget.dataset.field;
    var todo = 'guests[' + index + '].' + field;
    this.setData({
      [todo]: e.detail.value
    })
  },
  changeGender:function(e){
    var index = e.currentTarget.dataset.index;
    //this.data.guests[index].gender = e.detail.value;
    var todo = 'guests[' + index + '].gender';

    this.setData({
      [todo]: e.detail.value
    })
  },  
  addGuest:function(e){
    var length = this.data.guests.length;
    var todo = 'guests[' + length + ']';
    var guest = { id: 0, name: "", gender: "0", mobile: "", certId: "", address: "" }
    this.setData({
      [todo]: guest
    })
    //this.data.guests.push({ id: 0, name: "", gender: "", mobile: "", certId: "", address: "" });
    //this.setData({
    //  guests: this.data.guests
    //})
  },
  deleteGuest:function(e){
    var index = e.target.dataset.index;
    this.data.guests.splice(index, 1);
    this.setData({
      guests: this.data.guests
    })
  },
  save:function(e){
    if(this.data.saving){
      return;
    }
    this.data.saving = true;
    wx.showLoading({
      title: '保存中。。。',
      mask:true
    })
    var value = e.detail.value;
    //this.formatGuests(value);
    if(e.detail.target.dataset.type){
      this.data.type = e.detail.target.dataset.type
    }

    var regNull = /^[ ]+$/;
    for (var i = 0; i < this.data.guests.length; i++) {
      var nameIsNull =!this.data.guests[i].name || regNull.test(this.data.guests[i].name);
      var mobileIsNull = !this.data.guests[i].mobile || regNull.test(this.data.guests[i].mobile);
      var certIdIsNull = !this.data.guests[i].certId || regNull.test(this.data.guests[i].certId);
      var addressIsNull = !this.data.guests[i].address || regNull.test(this.data.guests[i].address);
      if (nameIsNull && mobileIsNull && certIdIsNull && addressIsNull){
        this.data.guests.splice(i, 1);
      }
    }
    
    var errMsg = "";
    if (!this.data.selectRoom) {
      errMsg = errMsg + '必须选择房间。\r\n'
    }
    if (!value.price||value.price<=0){
      errMsg = errMsg + '必须输入价格。\r\n';
    }
    if(!this.data.guests||this.data.guests.length<=0){
      errMsg = errMsg + '必须填写入住人。\r\n';
    }
    var hasGuestError =false;
    for(var i=0;i<this.data.guests.length;i++){
      if (!this.data.guests[i].name || regNull.test(this.data.guests[i].name)){
        hasGuestError = true;
        break;
      }
    }
    if (hasGuestError){
      errMsg = errMsg + '必须填写预定人/入住人姓名。\r\n';
    }
    var regIdNo = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/; 
    hasGuestError = false;
    if (this.data.type == 2) {
      for (var i = 0; i < this.data.guests.length; i++) {
        if (!this.data.guests[i].certId || !regIdNo.test(this.data.guests[i].certId)) {
          hasGuestError = true;
          break;
        }
      }
    }
    if (hasGuestError) {
      errMsg = errMsg + '入住人身份证存在错误。\r\n';
    }

    if (errMsg.length > 0) {
      wx.showModal({
        title: '缺少内容',
        content: errMsg,
        showCancel: false,
      })
      this.data.saving = false;
      wx.hideLoading();
      return;
    }
    
    var questData=null;
    //预定 处理
    if (this.data.type == 1) {
      questData = {
        id: this.data.id,
        roomId: this.data.selectRoom.id,
        roomNo: this.data.selectRoom.roomNo,
        status: 1,
        reserveTime: util.formatTime(new Date()),
        planedCheckinTime: this.data.beginDate.getFullYear().toString()+'-'+(this.data.beginDate.getMonth() + 1).toString()+'-'+this.data.beginDate.getDate().toString()+' 12:00:00',
        checkinTime: null,
        planedCheckoutTime: this.data.endDate.getFullYear().toString()+'-'+(this.data.endDate.getMonth() + 1).toString()+'-'+this.data.endDate.getDate()+' 12:00:00',
        checkoutTime:null,
        prices: value.price,
        deposit: value.deposit,
        remark:value.remark,
        guests:this.data.guests
      }
    } else if (this.data.type == 2){
      questData = {
        id: this.data.id,
        roomId: this.data.selectRoom.id,
        roomNo: this.data.selectRoom.roomNo,
        status: 2,
        reserveTime: null,
        planedCheckinTime: null,
        checkinTime: this.data.beginDate.getFullYear().toString() + '-' + (this.data.beginDate.getMonth() + 1).toString() + '-' + this.data.beginDate.getDate().toString() + ' '+(new Date()).getHours().toString()+':'+(new Date()).getMinutes().toString()+':'+(new Date()).getSeconds().toString(),
        planedCheckoutTime: this.data.endDate.getFullYear().toString() + '-' + (this.data.endDate.getMonth() + 1).toString() + '-' + this.data.endDate.getDate() + ' 12:00:00',
        checkoutTime: null,
        prices: value.price,
        deposit: value.deposit,
        remark: value.remark,
        guests: this.data.guests
      }
    }
    wx.request({
      url: app.globalData.url + 'RoomCheck/ModifyRoomCheck',
      method: 'POST',
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: questData,
      success: res =>{
        if (res.data && res.data.status && res.data.status == 1){
          this.data.id=res.data.data.id;
          this.setData({
            guests: res.data.data.guests
          })
          wx.showToast({
            title: '保存成功',
            icon: 'success',
            duration: 2000,
            success:()=>{
              wx.navigateBack({
                delta: 1
              });
            }
          });
        } else if (res.data && res.data.status && res.data.status == -2) {
          wx.showModal({
            title: '错误',
            content: res.data.massage,
            showCancel: false,
          })
        }else{
          wx.showModal({
            title: '错误',
            content: '网络问题，请稍后再试',
            showCancel: false,
          })
        }
      },
      fail: res =>{
        wx.showModal({
          title: '错误',
          content: '网络问题，请稍后再试',
          showCancel: false,
        })
      },
      complete:()=>{
        this.data.saving = false;
        wx.hideLoading();
      }
    })
  },
  formatGuests: function (value) {
    for (var property in value) {
      for (var i = 0; i < this.data.guests.length; i++) {
        switch (property) {
          case 'name' + i.toString():
            this.data.guests[i].name = value[property];
            break;
          case 'gender' + i.toString():
            this.data.guests[i].gender = value[property];
            break;
          case 'mobile' + i.toString():
            this.data.guests[i].mobile = value[property];
            break;
          case 'certId' + i.toString():
            this.data.guests[i].certId = value[property];
            break;
          case 'address' + i.toString():
            this.data.guests[i].address = value[property];
            break;
          default:
        }
      }
    }
  },
  cancel:function(e){
    wx.showModal({
      title: '确认取消订单',
      content: '确认取消订单',
      success: r => {
        if (r.confirm) {
          if (this.data.saving) {
            return;
          }
          this.data.saving = true;
          wx.showLoading({
            title: '取消中。。。',
            mask: true
          })
          wx.request({
            url: app.globalData.url + 'RoomCheck/DeleteCheck',
            method: 'POST',
            header: {
              "Authorization": "Bearer " + app.globalData.token.token
            },
            data: {
              id: this.data.id,
              roomId: this.data.selectRoom.id
            },
            success: res => {
              if (res.data && res.data.status && res.data.status == 1) {
                wx.showToast({
                  title: '取消预定成功',
                  icon: 'success',
                  duration: 2000,
                  success: () => {
                    wx.navigateBack({
                      delta: 1
                    });
                  }
                });
              } else if (res.data && res.data.status && res.data.status == -2) {
                wx.showModal({
                  title: '错误',
                  content: res.data.massage,
                  showCancel: false,
                })
              } else {
                wx.showModal({
                  title: '错误',
                  content: '网络问题，请稍后再试',
                  showCancel: false,
                })
              }
            },
            fail: res => {
              wx.showModal({
                title: '错误',
                content: '网络问题，请稍后再试',
                showCancel: false,
              })
            },
            complete: () => {
              this.data.saving = false;
              wx.hideLoading();
            }
          })
        } else if (r.cancel) {
          //console.log('用户点击取消')
        }
      }
    })
  },
  checkout:function(e){
    wx.showModal({
      title: '确认离店',
      content: '确认离店',
      success: r => {
        if (r.confirm) {
          if (this.data.saving) {
            return;
          }
          this.data.saving = true;
          wx.showLoading({
            title: '离店中。。。',
            mask: true
          })
          wx.request({
            url: app.globalData.url + 'RoomCheck/Checkout',
            method: "POST",
            header: {
              "Authorization": "Bearer " + app.globalData.token.token
            },
            data: {
              id: this.data.id,
              roomId: this.data.selectRoom.id,
              checkoutTime: new Date()
            },
            success: res => {
              if (res.data && res.data.status && res.data.status == 1) {
                wx.showToast({
                  title: '已离店',
                  icon: 'success',
                  duration: 2000,
                  success: () => {
                    wx.navigateBack({
                      delta: 1
                    });
                  }
                });
              } else if (res.data && res.data.status && res.data.status == -2) {
                wx.showModal({
                  title: '错误',
                  content: res.data.massage,
                  showCancel: false,
                })
              } else {
                wx.showModal({
                  title: '错误',
                  content: '网络问题，请稍后再试',
                  showCancel: false,
                })
              }
            },
            fail: r => {
              wx.showModal({
                title: '错误',
                content: '网络问题，请稍后再试',
                showCancel: false,
              })
            },
            complete: () => {
              this.data.saving = false;
              wx.hideLoading();
            }
          })
        } else if (r.cancel) {
          //console.log('用户点击取消')
        }
      }
    })
  },
  showCamera:function(e){
    this.setData({
      showCamera:true
    });
    this.myData.cameraForGuestIndex = e.target.dataset.index;
  },
  hideCamera:function(e){
    this.setData({
      showCamera: false
    })
  },
  changeDevicePosition:function(e){
    this.setData({
      isBackDevicePosition: !this.data.isBackDevicePosition
    })
  },
  takePhoto:function(e){
    
    this.ctx.takePhoto({
      quality: 'high',
      success: (res) => {
        this.setData({
          showCamera: false
        })
        wx.showLoading({
          title: '加载中',
        })
        wx.uploadFile({
          url: app.globalData.url + 'Guest/GetInfoFromCert',
          filePath: res.tempImagePath,
          name: 'file',
          header: {
            "Authorization": "Bearer " + app.globalData.token.token
          },
          formData: {
            hotelId: app.globalData.hotel.id
          },
          success: res => {
            if (res.statusCode == 200 && res.data) {
              var result = JSON.parse(res.data);
              if (result.status == 1) {
                var i = this.myData.cameraForGuestIndex;
                this.data.guests[i].name = result.data.name;
                this.data.guests[i].gender = result.data.gender;
                this.data.guests[i].certId = result.data.certId;
                this.data.guests[i].address = result.data.address;
                var todo = 'guests[' + i + ']';
                this.setData({
                  [todo]: this.data.guests[i]
                })
              } else if (result.status == -2) {
                wx.showModal({
                  title: '错误',
                  content: result.massage,
                  showCancel: false,
                });
              } else {
                wx.showModal({
                  title: '错误',
                  content: '系统问题，请稍后再试',
                  showCancel: false,
                });
              }
            } else {
              wx.showModal({
                title: '错误',
                content: '系统问题，请稍后再试',
                showCancel: false,
              });
            }
          },
          fail: res => {
            wx.showModal({
              title: '错误',
              content: '系统问题，请稍后再试',
              showCancel: false,
            });
          },
          complete: res => {
            wx.hideLoading()
          }
        })
      }
    })
  }
})