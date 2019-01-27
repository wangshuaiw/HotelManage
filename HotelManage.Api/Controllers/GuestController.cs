using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelManage.Interface;
using HotelManage.ViewModel.ApiVM;
using HotelManage.DBModel;
using Microsoft.Extensions.Configuration;
using HotelManage.Common;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using HotelManage.ViewModel.ApiVM.RequestVM;
using System.Net.Http;
using System.Web;
using System.Net;
using System.IO;

namespace HotelManage.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class GuestController : Controller
    {
        private IGuestHander Hander { get; }
        private IOcrHander OcrHander { get; }
        public IConfiguration Configuration { get; }

        public GuestController(IGuestHander hander,IOcrHander ocrHander, IConfiguration configuration)
        {
            Hander = hander;
            OcrHander = ocrHander;
            Configuration = configuration;
        }

        /// <summary>
        /// 添加入住人
        /// </summary>
        /// <param name="guest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<Guest>> Add([FromBody]Guest guest)
        {
            string manager = HttpContext.User.Identity.Name;
            await Task.Run(() => { Hander.Add(guest, manager); });
            return new Response<Guest>()
            {
                Status = StatusEnum.Success,
                Massage = "添加入住人成功",
                Data = guest
            };
        }

        /// <summary>
        /// 修改入住人
        /// </summary>
        /// <param name="guest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Update([FromBody]Guest guest)
        {
            string manager = HttpContext.User.Identity.Name;
            await Task.Run(() => { Hander.Update(guest, manager); });
            return new BaseResponse()
            {
                Status = StatusEnum.Success,
                Massage = "修改入住人成功"
            };
        }

        /// <summary>
        /// 删除入住人
        /// </summary>
        /// <param name="guest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseResponse> Delete([FromBody]Guest guest)
        {
            string manager = HttpContext.User.Identity.Name;
            await Task.Run(() => { Hander.Delete(guest, manager); });
            return new BaseResponse()
            {
                Status = StatusEnum.Success,
                Massage = "删除入住人成功"
            };
        }

        /// <summary>
        /// 证件识别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json", "multipart/form-data")]
        public async Task<Response<Guest>> GetInfoFromCert(int hotelId, IFormCollection files)
        {
            if (files.Files.Count < 1)
                return new Response<Guest>()
                {
                    Status = StatusEnum.ValidateModelError,
                    Massage = "没有上传图片",
                    Data = null
                };

            IFormFile file = files.Files[0];
            string strBuffer = string.Empty;
            using (var stream = file.OpenReadStream())
            {
                byte[] buffer = new byte[(int)stream.Length];
                stream.Read(buffer,0, (int)stream.Length);
                strBuffer = Convert.ToBase64String(buffer);
            }

            if(string.IsNullOrEmpty(strBuffer))
            {
                return new Response<Guest>()
                {
                    Status = StatusEnum.ValidateModelError,
                    Massage = "图片错误",
                    Data = null
                };
            }

            if (StaticData.BaiduToken == null ||
                string.IsNullOrEmpty(StaticData.BaiduToken.access_token) ||
                DateTime.Now > StaticData.BaiduToken.get_token_time.AddMinutes(StaticData.BaiduToken.expires_in).AddHours(-1)
              )
            {

                string baiduTokenUri = $"{ Configuration.GetValue<string>("BaiduApi:tokenUri") }?grant_type={ Configuration.GetValue<string>("BaiduApi:grant_type") }&client_id={ Configuration.GetValue<string>("BaiduApi:client_id") }&client_secret={ Configuration.GetValue<string>("BaiduApi:client_secret") }";
                string tokenResult = await Task.Run(() => { return HttpHelper.HttpFormPostRequest(baiduTokenUri, ""); });
                if (!string.IsNullOrEmpty(tokenResult))
                {
                    BaiduAccessToken token = JsonConvert.DeserializeObject<BaiduAccessToken>(tokenResult);
                    if (token == null || string.IsNullOrEmpty(token.access_token))
                    {
                        return new Response<Guest>()
                        {
                            Status = StatusEnum.Error,
                            Massage = "系统错误",
                            Data = null
                        };
                    }

                    token.get_token_time = DateTime.Now;
                    StaticData.BaiduToken = token;
                }
                else
                {
                    return new Response<Guest>()
                    {
                        Status = StatusEnum.Error,
                        Massage = "系统错误",
                        Data = null
                    };
                }
            }
            string manager = HttpContext.User.Identity.Name;
            string idcardUri = $"{ Configuration.GetValue<string>("BaiduApi:idcardUri") }?access_token={ StaticData.BaiduToken.access_token }";
            int maxNum = Configuration.GetValue<int>("BaiduApi:maxNum");

            KeyValuePair<Guest, string> guest = await Task.Run(() => { return OcrHander.GetGuestInfoFromCertByBaidu(idcardUri, strBuffer, hotelId, manager, maxNum); });

            if (guest.Key == null)
            {
                return new Response<Guest>()
                {
                    Status = StatusEnum.ValidateModelError,
                    Massage = guest.Value,
                    Data = null
                };
            }
            else
            {
                return new Response<Guest>()
                {
                    Status = StatusEnum.Success,
                    Massage = "证件解析成功",
                    Data = guest.Key
                };
            }

        }
    }
}