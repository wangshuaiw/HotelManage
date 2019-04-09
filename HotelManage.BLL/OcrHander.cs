using HotelManage.Common;
using HotelManage.DBModel;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HotelManage.BLL
{
    public class OcrHander : IOcrHander
    {
        public hotelmanageContext HotelContext { get; }

        public OcrHander(hotelmanageContext context)
        {
            HotelContext = context;
        }

        public KeyValuePair<Guest, string> GetGuestInfoFromCertByBaidu(string uri, string imageBuffer, int hotelId, string manager, int maxNum)
        {
            StringBuilder requestData = new StringBuilder();
            requestData.Append("detect_direction=true");
            requestData.Append("&id_card_side=front");
            requestData.Append($"&image={ HttpUtility.UrlEncode(imageBuffer) }");
            requestData.Append("&detect_risk=false");

            if (!HotelContext.Hotelmanager.Any(m => !m.IsDel.Value && m.HotelId == hotelId && m.WxOpenId == manager))
            {
                return new KeyValuePair<Guest, string>(null, "没有管理员权限!");
            }

            int hasNum = HotelContext.Baiduapilog.Where(l =>l.Type == 0 && l.CreateTime >= DateTime.Now.Date && l.CreateTime < DateTime.Now.Date.AddDays(1)).Count();
            if(hasNum >= maxNum)
            {
                return new KeyValuePair<Guest, string>(null, "已超过免费识别次数");
            }

            Baiduapilog log = new Baiduapilog()
            {
                HotelId = hotelId,
                @Type = 0,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            HotelContext.Baiduapilog.Add(log);
            HotelContext.SaveChanges();
            
            string idcardResult = HttpHelper.HttpFormPostRequest(uri, requestData.ToString());

            if (string.IsNullOrEmpty(idcardResult))
            {
                throw new Exception("调用百度接口返回错误！");
            }
            var jObject = JObject.Parse(idcardResult);
            if (jObject.Property("image_status") != null && jObject.Property("image_status").ToString() != "")
            {
                if (jObject["image_status"].ToString().ToLower() != "normal")
                {
                    return new KeyValuePair<Guest, string>(null, "照片不好,重新拍摄");
                }
                string name = jObject["words_result"]["姓名"]["words"].ToString();
                string certId = jObject["words_result"]["公民身份号码"]["words"].ToString();
                string address = jObject["words_result"]["住址"]["words"].ToString();

                return new KeyValuePair<Guest, string>(new Guest()
                {
                    Name = name,
                    CertId = certId,
                    Address = address
                }, "解析成功");
            }
            else
            {
                throw new Exception("调用百度接口返回错误！");
            }
        }
    }
}
