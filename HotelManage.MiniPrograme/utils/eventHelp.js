// 触摸开始时间
//touchStartTime = 0;
// 触摸结束时间
//touchEndTime = 0;
// 最后一次单击事件点击发生时间
var lastTapTime = 0,
// 单击事件点击后要触发的函数
//var lastTapTimeoutFunc = null;

/// 按钮触摸开始触发的事件
//touchStart = function(e) {
//    this.touchStartTime = e.timeStamp
//}

/// 按钮触摸结束触发的事件
//touchEnd = function(e) {
//  this.touchEndTime = e.timeStamp
//},

/// 双击
doubleTap = function(e,p,callback) {
  var that = this
  // 控制点击事件在350ms内触发，加这层判断是为了防止长按时会触发点击事件
  //if (that.touchEndTime - that.touchStartTime < 350) {
    // 当前点击的时间
    var currentTime = e.timeStamp
    var lastTapTime = that.lastTapTime
    // 更新最后一次点击时间
    that.lastTapTime = currentTime

    // 如果两次点击时间在300毫秒内，则认为是双击事件
    if (currentTime - lastTapTime < 300) {
      // 成功触发双击事件时，取消单击事件的执行
      //clearTimeout(that.lastTapTimeoutFunc);
      
      callback(p);
    }
  //}
}

module.exports = {
  doubleTap: doubleTap
}