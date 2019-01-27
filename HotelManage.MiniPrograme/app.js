//app.js
App({
  onLaunch: function () {
    // 展示本地存储能力
    /*
    var logs = wx.getStorageSync('logs') || []
    logs.unshift(Date.now())
    wx.setStorageSync('logs', logs)
    */

    // 登录
    wx.login({
      success: res => {
        // 发送 res.code 到后台换取 openId, sessionKey, unionId
        if(res.code){
          wx.request({
            url: this.globalData.url +'login/wxlogin',
            method:"POST",
            data:{
              wxcode: res.code
            },
            success:r=>{
              if (r.statusCode&&r.statusCode==200&&r.data){
                var response=r.data;
                if (response.status==1&&response.data){
                  this.globalData.hotel = response.data.hotel;
                  this.globalData.token = { 
                    "token": response.data.jwtToken,
                    "openId": response.data.openId,
                    "unionId": response.data.unionId,
                    "role":response.data.role
                  };
                  if (this.globalDataReadyCallback){
                    this.globalDataReadyCallback(this.globalData);
                  }
                }else{
                  wx.navigateTo({
                    url: '/pages/loginerror/loginerror',
                  })
                }
              }else{
                wx.navigateTo({
                  url: '/pages/loginerror/loginerror',
                })
              }
            },
            fail:e=>{
              wx.navigateTo({
                url: '/pages/loginerror/loginerror',
              })
            }
          })
        }else{
          wx.navigateTo({
            url: '/pages/loginerror/loginerror',
          })
        }
      }
    })
    // 获取用户信息
    wx.getSetting({
      success: res => {
        if (res.authSetting['scope.userInfo']) {
          // 已经授权，可以直接调用 getUserInfo 获取头像昵称，不会弹框
          wx.getUserInfo({
            success: res => {
              // 可以将 res 发送给后台解码出 unionId
              this.globalData.userInfo = res.userInfo

              // 由于 getUserInfo 是网络请求，可能会在 Page.onLoad 之后才返回
              // 所以此处加入 callback 以防止这种情况
              if (this.userInfoReadyCallback) {
                this.userInfoReadyCallback(res)
              }
            }
          })
        }else{
          //wx.switchTab({
          //  url: '/pages/mypage/mypage'
          //})
          wx.navigateTo({
            url: '/pages/login/login',
          })
        }
      }
    })
  },
  globalData: {
    url:'http://192.168.0.103:5000/api/',
    userInfo: null,
    hotel: null,
    token:null
  }
})