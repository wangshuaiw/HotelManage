// pages/status/status.js
var util = require('../../utils/util.js');
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    showCamera:false,
    //showLoading:false,
    hotel: {},
    date:null,
    room:{},
    isEdit:false,
    preRoomStatus:0,
    certTypes:[],
    selectCertType:null,
    reserveTime:null,
    checkinTime:null,
    checkoutTime:null,
    planedCheckinDate:null,
    planedCheckinTime:null,
    planedCheckoutDate:null,
    planedCheckoutTime:null,

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
            });
            if (res.data.data.reserveTime){
              this.setData({
                reserveTime: util.formatTimeNoSecond(new Date(res.data.data.reserveTime))
              })
            }
            if (res.data.data.checkinTime){
              this.setData({
                checkinTime: util.formatTimeNoSecond(new Date(res.data.data.checkinTime))
              })
            }
            if (res.data.data.planedCheckinTime){
              this.setData({
                planedCheckinDate: util.formatDate(new Date(res.data.data.planedCheckinTime)),
                planedCheckinTime: util.formatOnlyHourMinite(new Date(res.data.data.planedCheckinTime))
              })
            }
            if (res.data.data.planedCheckoutTime){
              this.setData({
                planedCheckoutDate: util.formatDate(new Date(res.data.data.planedCheckoutTime)),
                planedCheckoutTime: util.formatOnlyHourMinite(new Date(res.data.data.planedCheckoutTime))
              })
            }
            if (res.data.data.checkoutTime) {
              this.setData({
                checkoutTime: util.formatTimeNoSecond(new Date(res.data.data.checkoutTime))
              })
            }
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
    this.ctx = wx.createCameraContext()
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
    this.onLoad({
      roomId: this.data.room.roomId,
      id: this.data.room.id,
      date: this.data.date
    });
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
    this.data.preRoomStatus = this.data.room.status;
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
  btnCheckin: function (e) {
    this.data.preRoomStatus = this.data.room.status;
    this.data.room.status = 2;
    if (this.data.preRoomStatus == 1) {
      this.setData({
        room: this.data.room,
        isEdit: true,
      })
    } else {
      //直接入住 
      this.data.room.guests = [{ isEdit: true }]
      var defaultPlanedCheckoutDate = new Date(this.data.date);
      defaultPlanedCheckoutDate.setDate(defaultPlanedCheckoutDate.getDate() + 1);
      this.setData({
        room: this.data.room,
        isEdit: true,
        planedCheckoutDate: util.formatDate(defaultPlanedCheckoutDate),
        planedCheckoutTime: "12:00"
      })
    }

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
            mobile: e.detail.value.mobile,
            address: e.detail.value.address
          },
          success: res => {
            if (res.data && res.data.status && res.data.status == 1) {
              this.data.room.guests[i].name = e.detail.value.name;
              this.data.room.guests[i].certType = this.data.selectCertType.key;
              this.data.room.guests[i].certTypeName = this.data.selectCertType.value;
              this.data.room.guests[i].certId = e.detail.value.certId;
              this.data.room.guests[i].mobile = e.detail.value.mobile;
              this.data.room.guests[i].address = e.detail.value.address;
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
            mobile: e.detail.value.mobile,
            address: e.detail.value.address
          },
          success: res => {
            if (res.data && res.data.status && res.data.status == 1 && res.data.data) {
              this.data.room.guests[i].id = res.data.data.Id
              this.data.room.guests[i].name = e.detail.value.name;
              this.data.room.guests[i].certType = this.data.selectCertType.key;
              this.data.room.guests[i].certTypeName = this.data.selectCertType.value;
              this.data.room.guests[i].certId = e.detail.value.certId;
              this.data.room.guests[i].mobile = e.detail.value.mobile;
              this.data.room.guests[i].address = e.detail.value.address;
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
      this.data.room.guests[i].address = e.detail.value.address;
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
      this.data.room.guests.splice(i, 1);
    }
    this.setData({
      room:this.data.room
    })
  },
  btnAddGuest:function(e){
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
  btnEditGuest:function(e){
    var index = e.target.dataset.index;
    for (var i = 0; i < this.data.room.guests.length; i++) {
      if (this.data.room.guests[i].isEdit == true) {
        wx.showToast({
          title: '请先保存正在编辑的入住人',
          icon: 'none'
        })
        return
      }
    }
    this.data.room.guests[index].isEdit=true;
    this.setData({
      room: this.data.room
    })
  },
  btnDelGuest:function(e){
    var index = e.target.dataset.index;
    wx.showModal({
      title: '确定删除！',
      content: '确定删除入住人信息！',
      success: res => {
        if (res.confirm) {
          wx.request({
            url: app.globalData.url + 'Guest/Delete',
            method: "POST",
            header: {
              "Authorization": "Bearer " + app.globalData.token.token
            },
            data: {
              id: this.data.room.guests[index].id,
              checkId: this.data.room.id
            },
            success: r => {
              if (r.statusCode == 200 && r.data) {
                if (r.data.status == 1) {
                  this.data.room.guests.splice(index, 1);
                  this.setData({
                    room: this.data.room
                  })
                } else {
                  wx.showToast({
                    title: '网络问题，请稍后再试',
                    icon: 'none'
                  });
                }
              } else {
                wx.showToast({
                  title: '网络问题，请稍后再试',
                  icon: 'none'
                });
              }
            },
            fail: r => {
              wx.showToast({
                title: '网络问题，请稍后再试',
                icon: 'none'
              });
            }
          })
        } else if (res.cancel) {
          //console.log('用户点击取消')
        }
      }
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
    if (!this.data.room.reserveTime){
      this.data.room.reserveTime = this.data.room.status == 1 ? util.formatTime(new Date()) : null;
    }
    if (this.data.planedCheckinDate && this.data.planedCheckinTime){
      this.data.room.planedCheckinTime = this.data.planedCheckinDate + ' ' + this.data.planedCheckinTime;
    }
    if (!this.data.room.checkinTime){
      this.data.room.checkinTime = this.data.room.status == 2 ? util.formatTime(new Date()) : null;
    }
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
            isEdit:false,
            //checkinTime: this.data.room.checkinTime
          });
          if (res.data.data.reserveTime) {
            this.setData({
              reserveTime: util.formatTimeNoSecond(new Date(res.data.data.reserveTime))
            })
          }
          if (res.data.data.checkinTime) {
            this.setData({
              checkinTime: util.formatTimeNoSecond(new Date(res.data.data.checkinTime))
            })
          }
        }else if (res.data && res.data.status && res.data.status == -2){
          wx.showToast({
            title: res.data.massage,
            icon: 'none'
          });
        }else {
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
  btnCancelEdit:function(e){
    for (var i = 0; i < this.data.room.guests.length; i++) {
      if (this.data.room.guests[i].isEdit == true) {
        if (this.data.room.guests[i].id > 0 ) {
          this.data.room.guests[i].isEdit = false;
        } else {
          this.data.room.guests.splice(i, 1);
        }
      } else if (!this.data.room.guests[i].id){
        this.data.room.guests.splice(i, 1);
      }
      
    }

    this.data.room.status=this.data.preRoomStatus;
    this.setData({
      isEdit:false,
      room: this.data.room
    })
  },
  btnEdit:function(e){
    this.data.preRoomStatus=this.data.room.status;
    this.setData({
      isEdit:true
    })
  },
  btnDelCheck:function(e){
    wx.showModal({
      title: '确定删除！',
      content: '确定删除订单信息！',
      success: res => {
        if (res.confirm) {
          wx.request({
            url: app.globalData.url + 'RoomCheck/DeleteCheck',
            method: "POST",
            header: {
              "Authorization": "Bearer " + app.globalData.token.token
            },
            data: {
              id: this.data.room.id,
              roomId:this.data.room.roomId,
            },
            success: r => {
              if (r.statusCode == 200 && r.data) {
                if (r.data.status == 1) {
                  this.data.room.id = 0;
                  this.data.room.status = 0;
                  this.data.room.reserveTime = null;
                  this.data.room.planedCheckinTime = null;
                  this.data.room.checkinTime = null;
                  this.data.room.planedCheckoutTime = null;
                  this.data.room.checkoutTime = null;
                  this.data.room.prices = 0;
                  this.data.room.deposit = 0;
                  this.data.room.remark = null;
                  this.data.room.guests = null;
                  this.setData({
                    room: this.data.room
                  })
                } else if (r.data.status == -2) {
                  wx.showToast({
                    title: r.data.massage,
                    icon: 'none'
                  });
                } else {
                  wx.showToast({
                    title: '网络问题，请稍后再试',
                    icon: 'none'
                  });
                }
              } else {
                wx.showToast({
                  title: '网络问题，请稍后再试',
                  icon: 'none'
                });
              }
            },
            fail: r => {
              wx.showToast({
                title: '网络问题，请稍后再试',
                icon: 'none'
              });
            }
          })
        } else if (res.cancel) {
          //console.log('用户点击取消')
        }
      }
    })
  },
  btnCheckout:function(e){
    wx.showModal({
      title: '确认离店！',
      content: '确认客人已离店！',
      success: res => {
        if (res.confirm) {
          wx.request({
            url: app.globalData.url + 'RoomCheck/Checkout',
            method: "POST",
            header: {
              "Authorization": "Bearer " + app.globalData.token.token
            },
            data: {
              id: this.data.room.id,
              roomId: this.data.room.roomId,
              checkoutTime:new Date()
            },
            success: r => {
              if (r.statusCode == 200 && r.data) {
                if (r.data.status == 1) {
                  this.data.room.id = 0;
                  this.data.room.status = 0;
                  this.data.room.reserveTime = null;
                  this.data.room.planedCheckinTime = null;
                  this.data.room.checkinTime = null;
                  this.data.room.planedCheckoutTime = null;
                  this.data.room.checkoutTime = null;
                  this.data.room.prices = 0;
                  this.data.room.deposit = 0;
                  this.data.room.remark = null;
                  this.data.room.guests = null;
                  this.setData({
                    room: this.data.room
                  })
                } else if (r.data.status == -2) {
                  wx.showToast({
                    title: r.data.massage,
                    icon: 'none'
                  });
                } else {
                  wx.showToast({
                    title: '网络问题，请稍后再试',
                    icon: 'none'
                  });
                }
              } else {
                wx.showToast({
                  title: '网络问题，请稍后再试',
                  icon: 'none'
                });
              }
            },
            fail: r => {
              wx.showToast({
                title: '网络问题，请稍后再试',
                icon: 'none'
              });
            }
          })
        } else if (res.cancel) {
          //console.log('用户点击取消')
        }
      }
    })
  },
  btnShowCamera:function(e){
    wx.getSetting({
      success:res=>{
        if (res.authSetting['scope.camera']){
          this.setData({
            showCamera:true
          })
        }else{
          wx.authorize({
            scope: 'scope.camera',
            success() {
              this.setData({
                showCamera: true
              })
            }
          })

        }
      }
    })
  },
  takePhoto:function(e){
    this.setData({
      showCamera: false
      //showLoading:true
    })

    wx.showLoading({
      title: '加载中',
    })

    this.ctx.takePhoto({
      quality: 'high',
      success: (res) => {
        //console.log(res);

        wx.uploadFile({
          url: app.globalData.url + 'Guest/GetInfoFromCert', 
          filePath: res.tempImagePath,
          name: 'file',
          header: {
            "Authorization": "Bearer " + app.globalData.token.token
          },
          formData: {
            hotelId: this.data.hotel.id
          },
          success:res=> {
            //const data = res.data
            console.log(res);
            if (res.statusCode == 200 && res.data) {
              var result = JSON.parse(res.data);
              if (result.status == 1) {
                for (var i = 0; i < this.data.room.guests.length; i++) {
                  if (this.data.room.guests[i].isEdit == true) {
                    this.data.room.guests[i].certId = result.data.certId;
                    this.data.room.guests[i].name = result.data.name;
                    this.data.room.guests[i].address = result.data.address;
                  }
                }
                this.setData({
                  room:this.data.room
                })
                console.log(this.data.room);
              } else if (result.status == -2) {
                wx.showToast({
                  title: res.data.massage,
                  icon: 'none'
                });
              } else {
                wx.showToast({
                  title: '系统问题',
                  icon: 'none'
                });
              }
            }else{
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
          },
          complete:res=>{
            //this.setData({
            //  showLoading: false
            //})
            wx.hideLoading()
          }
        })
      }
    })
  },
  cancelPhoto:function(){
    this.setData({
      showCamera:false
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