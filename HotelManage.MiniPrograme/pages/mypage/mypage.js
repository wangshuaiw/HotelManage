// pages/mypage/mypage.js
var eventH = require('../../utils/eventHelp.js');
const app = getApp()
Page({
  /**
   * 页面的初始数据
   */
  data: {
    userInfo:{},
    hasUserInfo:false,
    canIUse: wx.canIUse('button.open-type.getUserInfo'),
    myHotel:{},
    fullAddress:null,
    hasHotel:false,
    editHotel:0, //1-新增；2-修改
    region: null,
    canShare:false,
    isShare:false,

    //editRoom: 0, //1-新增；2-修改
    showAddRoom:false,
    rooms:[],
    roomTypes: null,
    //[{ key: 'dc', value: '大床房' }, { key: 'bj', value: '标准间' }, { key: 'jt', value:'家庭房'}],
    selectRoomType:null,
    //selectRoom:null
    manageRoom:false,
    selectAll:false,
    //token:null
    pullDownRefresh:false
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    if (app.globalData.userInfo) {
      this.setData({
        userInfo: app.globalData.userInfo,
        hasUserInfo: true
      })
    } else if (this.data.canIUse) {
      // 由于 getUserInfo 是网络请求，可能会在 Page.onLoad 之后才返回
      // 所以此处加入 callback 以防止这种情况
      app.userInfoReadyCallback = res => {
        this.setData({
          userInfo: res.userInfo,
          hasUserInfo: true
        })
      }
    } else {
      // 在没有 open-type=getUserInfo 版本的兼容处理
      wx.getUserInfo({
        success: res => {
          app.globalData.userInfo = res.userInfo
          this.setData({
            userInfo: res.userInfo,
            hasUserInfo: true
          })
        }
      })
    }

    if (app.globalData.hotel) {
      this.setData({
        myHotel: app.globalData.hotel,
        fullAddress: app.globalData.hotel.region.replace(/,/g, "") + app.globalData.hotel.address,
        hasHotel: true,
        isShare:false
      })
      //获取rooms
      this.getRooms(app.globalData.hotel.id);
    } else if (options.hotelId && app.globalData.token) {
      //get hotel by id
      wx.request({
        url: app.globalData.url +'Hotel/GetHotelById',
        method:'GET',
        header: {
          "Authorization": "Bearer " + app.globalData.token.token
        },
        data: {
          hotelId: options.hotelId
        },
        success:res=>{
          if (res.data && res.data.status && res.data.status==1 && res.data.data){
            this.setData({
              isShare: true,
              hasHotel: false,
              myHotel: res.data.data,
              fullAddress: res.data.data.region.replace(/,/g, "") + res.data.data.address,
            })
          }
        }
      })
    }

    if (app.globalData.token) {
      this.setData({
        //token: d.token
        canShare: app.globalData.token.role==0
      })
    }
    
    app.globalDataReadyCallback = d=>{
      if (d.hotel) {
        this.setData({
          myHotel: d.hotel,
          fullAddress: app.globalData.hotel.region.replace(/,/g, "") + app.globalData.hotel.address,
          hasHotel: true,
          isShare: false
        })
        //获取rooms
        this.getRooms(d.hotel.id);
      } else if (options.hotelId){
        wx.request({
          url: app.globalData.url + 'Hotel/GetHotelById',
          method: 'GET',
          header: {
            "Authorization": "Bearer " + app.globalData.token.token
          },
          data: {
            hotelId: options.hotelId
          },
          success: res => {
            if (res.data && res.data.status && res.data.status == 1 && res.data.data) {
              this.setData({
                isShare: true,
                hasHotel: false,
                myHotel: res.data.data,
                fullAddress: res.data.data.region.replace(/,/g, "") + res.data.data.address,
              })
            }
          }
        })
      };
      if (d.token) {
        this.setData({
          //token: d.token
          canShare: app.globalData.token.role == 0
        })
      };
    };
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
    this.data.pullDownRefresh = true
    this.getRooms(app.globalData.hotel.id);
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {
  
  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function (res) {
    if (res.from ==="button"){
      // 来自页面内转发按钮,共同管理宾馆
      return{
        title:"和我一起管理宾馆吧",
        path: "/pages/mypage/mypage?hotelId=" + this.data.myHotel.id
      }
    }else{
      //分享小程序
      return{
        title:"推荐一个宾馆管理小程序给你"
      }
    }
  },

  getUserInfo: function (e) {
    app.globalData.userInfo = e.detail.userInfo
    this.setData({
      userInfo: app.globalData.userInfo,
      hasUserInfo: true
    })
  },

  //获取rooms
  getRooms: function(hotelId){
    wx.request({
      url: app.globalData.url + 'Room/GetRooms',
      method: 'GET',
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: {
        hotelId: hotelId
      },
      success: res => {
        if (res.data && res.data.status && res.data.status == 1 && res.data.data) {
          this.setData({
            rooms: res.data.data
          })
          if (this.data.pullDownRefresh){
            wx.stopPullDownRefresh();
            this.data.pullDownRefresh =false;
          }
        }
      }
    })
  },

  tapHotel: function (e) {
    eventH.doubleTap(e, e, (e) => {
      this.updateHotel(e);
    })
  },

  longPressHotel: function (e) {
    wx.showActionSheet({
      itemList: ['编辑'],
      success: res => {
        if (res.tapIndex == 0) {
          this.updateHotel();
        }
      }
    })
  },

  //添加宾馆
  addHotel:function(e){
    this.setData({
      editHotel:1,
      region: ["山西省", "忻州市", "五台县"]
    });
  },

  bindRegionChange: function (e) {
    this.setData({
      region: e.detail.value
    })
  },

  //保存宾馆
  formSubmit: function (e) {
    //console.log(e.detail.value);
    var value = e.detail.value;
    if(!value){
      wx.showToast({
        title: '请填写内容',
        icon: 'none'
      })
      return;
    };
    if(!value.name){
      wx.showToast({
        title: '请填写宾馆名称',
        icon: 'none'
      })
      return;
    };
    
    if(this.data.editHotel==1){
      this.addHotelSubmit(value);
    } else if (this.data.editHotel==2){
      this.updateHotelSubmit(value);
    }
  },

  addHotelSubmit:function(value){
    if (!value.hotelPassword || !value.hotelPassword2 || value.hotelPassword != value.hotelPassword2) {
      wx.showToast({
        title: '请填写宾馆管理密码并确认管理密码',
        icon: 'none'
      })
      return;
    }
    wx.request({
      url: app.globalData.url + 'Hotel/Create',
      method: "POST",
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: {
        name: value.name,
        password: value.hotelPassword,
        region: value.region.join(),
        address: value.address,
        remark: value.remark,
        wxOpenId: app.globalData.token.openId,
        wxUnionId: app.globalData.token.unionId,
      },
      success: r => {
        //console.log(r);
        if (r.statusCode == 200 && r.data) {
          if (r.data.status == 1 && r.data.data) {
            this.setData({
              editHotel: 0,
              myHotel: r.data.data,
              fullAddress: r.data.data.region.replace(/,/g, "") + r.data.data.address,
              hasHotel: true
            })
          } else {
            wx.showToast({
              title: r.data.massage,
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
    });
  },

  //修改宾馆后台交互
  updateHotelSubmit:function(value){
    wx.request({
      url: app.globalData.url + 'Hotel/Update',
      method: "POST",
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data:{
        id:this.data.myHotel.id,
        name:value.name,
        hotelPassword: value.hotelPassword,
        region:value.region.join(),
        address:value.address,
        remark:value.remark
      },
      success:r=>{
        if (r.statusCode == 200 && r.data) {
          if (r.data.status == 1 && r.data.data){
            this.setData({
              editHotel: 0,
              myHotel: r.data.data,
              fullAddress: r.data.data.region.replace(/,/g, "") + r.data.data.address,
              hasHotel: true
            })
          }else{
            wx.showToast({
              title: r.data.massage,
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
      fail:r=>{
        wx.showToast({
          title: '网络问题，请稍后再试',
          icon: 'none'
        });
      }
    })
  },

  //修改宾馆信息
  updateHotel:function(e){
    this.setData({
      editHotel: 2,
      region: this.data.myHotel.region.split(',')
    })
  },
  editCancel:function(e){
    this.setData({
      editHotel: 0
    })
  },
  cancelAddManager:function(e){
    this.setData({
      isShare: false
    })
  },
  addManager:function(e){
    //console.log(e.detail.value)
    wx.request({
      url: app.globalData.url +'HotelManager/Add',
      method: "POST",
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: {
        hotelId: this.data.myHotel.id,
        wxOpenId: app.globalData.token.openId,
        wxUnionId: app.globalData.token.unionId,
        password: e.detail.value.hotelPassword
      },
      success: r => {
        if (r.statusCode == 200 && r.data) {
          if (r.data.status == 1 && r.data.data) {
            this.setData({
              hasHotel: true,
              isShare:false
            })
            this.getRooms(this.data.myHotel.id);
          } else {
            wx.showToast({
              title: r.data.massage,
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
  },

  //房间类型选择
  bindRoomTypeChange:function(e){
    //console.log('picker发送选择改变，携带值为', e.detail.value)
    this.setData({
      selectRoomType: this.data.roomTypes[e.detail.value]
    })
  },

  //添加房间按钮
  addRoom:function(e){
    if (!this.data.roomTypes || this.data.roomTypes.length==0){
      //初始化房间类型
      wx.request({
        url: app.globalData.url + 'HotelEnum/GetRoomTypes',
        method: "GET",
        header: {
          "Authorization": "Bearer " + app.globalData.token.token
        },
        success:r=>{
          if (r.statusCode == 200 && r.data) {
            if (r.data.status == 1 && r.data.data) {
              var types=new Array();
              for (var i = 0; i < r.data.data.length; i++){
                types.push({ "key": r.data.data[i].fullKey, "value": r.data.data[i].name})
              }
              this.setData({
                roomTypes: types,
                //editRoom:1,
                //selectRoom: null,
                selectRoomType: types[0],
                showAddRoom:true
              })
            }else{
              wx.showToast({
                title: '网络问题，请稍后再试',
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
        fail:r=>{
          wx.showToast({
            title: '网络问题，请稍后再试',
            icon: 'none'
          });
        }
      })
    }else{
      this.setData({
        showAddRoom:true
        //editRoom: 1,
        //selectRoom:null
        //selectRoomType: types[0]
      })
      if (!this.data.selectRoomType){
        this.setData({
          selectRoomType: types[0]
        })
      }
    }
  },

  //取消添加房间
  cancelAddRoom: function (e) {
    this.setData({
      showAddRoom: false,
    })
  },

  //添加，修改  房间提交
  editRoomSubmit:function(e){
    var value = e.detail.value;
    if (!value.roomNo){
      wx.showToast({
        title: '请填写房间编号',
        icon: 'none'
      })
      return;
    }
    if (!this.data.selectRoomType){
      wx.showToast({
        title: '请选房间类型',
        icon: 'none'
      })
      return;
    }
    var existRoom = this.data.rooms.find((v) => {
      return v.roomNo == value.roomNo && v.id != value.id
    });
    if (existRoom) {
      wx.showToast({
        title: '已存在相同的房间号',
        icon: 'none'
      })
      return;
    }

    var type = e.detail.target.dataset.type;
    if(type=="add"){
      this.addRoomSubmit(value);
    } else if (type == "update"){
      this.updateRoomSubmit(value);
    }
  },

  addRoomSubmit:function(value){
    
    wx.request({
      url: app.globalData.url + 'Room/Add',
      method: "POST",
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: {
        hotelId: this.data.myHotel.id,
        roomNo: value.roomNo,
        roomType: this.data.selectRoomType.key,
        remark: value.remark,
        wxOpenId: app.globalData.token.openId
      },
      success: r => {
        if (r.statusCode == 200 && r.data) {
          if (r.data.status == 1 && r.data.data) {
            var tempRooms = this.data.rooms;
            tempRooms.push(r.data.data);
            this.setData({
              rooms: tempRooms,
              //editRoom:0
              showAddRoom:false
            })
            //console.log(this.data.rooms);
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
  },

  tapRoom: function (e) {
    if (!this.data.manageRoom){
      eventH.doubleTap(e, e, (e) => {
        this.updateRoom(e);
      })
    }
  },

  longPressRoom: function (e) {
    if (!this.data.manageRoom) {
      wx.showActionSheet({
        itemList: ['修改', '删除'],
        success: res => {
          //console.log(res.tapIndex);
          if (res.tapIndex == 0) {
            this.updateRoom(e);
          } else if (res.tapIndex == 1){
            this.deleteRoom(e);
          }
        }
      })
    }
  },

  updateRoom:function(e){
    var index = e.currentTarget.dataset.index;
    this.data.rooms[index].edit=true;
    var room = this.data.rooms[index];
    this.setData({
      rooms: this.data.rooms
    })

    var selectType;
    if (this.data.roomTypes && this.data.roomTypes.length>0){
      selectType = this.data.roomTypes.find((e) => (e.key == room.roomType));
    }
    if (!this.data.roomTypes || this.data.roomTypes.length == 0 || !selectType) {
      //初始化房间类型
      wx.request({
        url: app.globalData.url + 'HotelEnum/GetRoomTypes',
        method: "GET",
        header: {
          "Authorization": "Bearer " + app.globalData.token.token
        },
        success: r => {
          if (r.statusCode == 200 && r.data) {
            if (r.data.status == 1 && r.data.data) {
              var types = new Array();
              for (var i = 0; i < r.data.data.length; i++) {
                types.push({ "key": r.data.data[i].fullKey, "value": r.data.data[i].name })
                if (r.data.data[i].fullKey == room.roomType){
                  selectType = { "key": r.data.data[i].fullKey, "value": r.data.data[i].name }
                }
              }
              this.setData({
                roomTypes: types,
                selectRoomType: selectType
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
    } else {
      this.setData({
        selectRoomType: selectType
      })
    }
  },

  //取消编辑
  cancelEditRoom: function (e) {
    var index = e.currentTarget.dataset.index;
    this.data.rooms[index].edit=false;
    this.setData({
      rooms:this.data.rooms
    })
  },

  updateRoomSubmit:function(value){
    wx.request({
      url: app.globalData.url + 'Room/Updete',
      method: "POST",
      header: {
        "Authorization": "Bearer " + app.globalData.token.token
      },
      data: {
        id: value.id,
        hotelId: this.data.myHotel.id,
        roomNo: value.roomNo,
        roomType: this.data.selectRoomType.key,
        remark: value.remark,
        //isDel: this.data.selectRoom.isDel,
        //createTime: this.data.selectRoom.createTime,
        //wxOpenId: app.globalData.token.openId
      },
      success: r => {
        if (r.statusCode == 200 && r.data) {
          if (r.data.status == 1) {
            for (var i = 0; i < this.data.rooms.length; i++) {
              if (this.data.rooms[i].id == value.id){
                this.data.rooms[i].roomNo = value.roomNo;
                this.data.rooms[i].remark = value.remark;
                this.data.rooms[i].roomType = this.data.selectRoomType.key;
                this.data.rooms[i].roomTypeName = this.data.selectRoomType.value;
                this.data.rooms[i].edit=false;
              }
            }
            this.setData({
              rooms: this.data.rooms
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
  },

  deleteRoom:function(e){
    var index = e.currentTarget.dataset.index;
    var room = this.data.rooms[index];
    wx.showModal({
      title: '确定删除！',
      content: '删除后，房间的历史入住信息也将被删除！',
      success: res=> {
        if(res.confirm) {
          wx.request({
            url: app.globalData.url + 'Room/Delete',
            method: "POST",
            header: {
              "Authorization": "Bearer " + app.globalData.token.token
            },
            data: {
              id: room.id,
              hotelId: room.hotelId,
            },
            success: r => {
              if (r.statusCode == 200 && r.data) {
                if (r.data.status == 1) {
                  this.data.rooms.splice(index, 1);
                  this.setData({
                    rooms: this.data.rooms,
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
        } else if(res.cancel) {
        }
      }
    })
  },

  manageRooms:function(e){
    this.setData({
      manageRoom:true
    })
  },

  completeManageRooms:function(e){
    this.setData({
      manageRoom: false
    })
  },

  checkboxChange(e) {
    for (var i = 0; i < this.data.rooms.length; i++) {
      if (e.detail.value.indexOf(i.toString())>=0){
        this.data.rooms[i].checked = true;
      }else{
        this.data.rooms[i].checked = false;
      }
    }
    this.setData({
      selectAll: false
    })
  },

  selectAllFunc:function(e){
    for(var i=0;i<this.data.rooms.length;i++){
      this.data.rooms[i].checked = true;
    }
    this.setData({
      rooms:this.data.rooms,
      selectAll:true
    })
  },

  selectNoneFunc:function(e){
    for (var i = 0; i < this.data.rooms.length; i++) {
      this.data.rooms[i].checked = false;
    }
    this.setData({
      rooms: this.data.rooms,
      selectAll: false
    })
  },

  delCheckedRooms:function(e){
    var checkedRooms = [];
    var i = this.data.rooms.length - 1;
    while (i >= 0) {
      if (this.data.rooms[i].checked) {
        checkedRooms.push(this.data.rooms[i]);
        this.data.rooms.splice(i, 1);
      }
      i--;
    }
    if (checkedRooms.length>0){
      wx.showModal({
        title: '确定删除！',
        content: '删除后，房间的历史入住信息也将被删除！',
        success: res => {
          if (res.confirm) {
            
            /*
            wx.request({
              url: app.globalData.url + 'Room/BatchDel',
              method: "POST",
              header: {
                "Authorization": "Bearer " + app.globalData.token.token
              },
              data: checkedRooms,
              success: r => {
                if (r.statusCode == 200 && r.data) {
                  if (r.data.status == 1) {
                    this.setData({
                      rooms: this.data.rooms,
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
            */

            for (var i = 0; i < checkedRooms.length;i++){
              if (checkedRooms[i].checked){
                var room = checkedRooms[i];
                wx.request({
                  url: app.globalData.url + 'Room/Delete',
                  method: "POST",
                  header: {
                    "Authorization": "Bearer " + app.globalData.token.token
                  },
                  data: {
                    id: room.id,
                    hotelId: room.hotelId,
                  },
                })
              }
            }
            this.setData({
              rooms: this.data.rooms,
            })
            
          } else if (res.cancel) {
          }
        }
      })
    }else{
      wx.showToast({
        title: '请选择房间',
        icon: 'none'
      });
    }
  }

})