using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using HotelManage.Common;
using HotelManage.DBModel;
using HotelManage.ViewModel.ApiVM;
using HotelManage.ViewModel.ApiVM.ResponseVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace HotelManage.Api.Controllers
{
    public class LoginController : Controller
    {
        public IConfiguration Configuration { get; }
        public ILogger<LoginController> Logger { get; }
        public hotelmanageContext HotelContext { get; }

        public LoginController(IConfiguration configuration, ILogger<LoginController> logger,hotelmanageContext context)

        {
            Configuration = configuration;
            Logger = logger;
            HotelContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public Response<Login> WxLogin([FromBody]dynamic request)
        {
            string wxcode = request.wxcode;

            Login resultData = new Login();
            string appid = Configuration.GetValue<string>("AppSetting:WxAppid");
            string secret = Configuration.GetValue<string>("AppSetting:WxSecret");
            string uri = $"https://api.weixin.qq.com/sns/jscode2session?appid={appid}&secret={secret}&js_code={wxcode}&grant_type=authorization_code";
            string response = HttpHelper.HttpJsonGetRequest(uri);
            if (!string.IsNullOrEmpty(response))
            {
                WxLoginInfo wxInfo = JsonConvert.DeserializeObject<WxLoginInfo>(response);

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("AppSetting:JwtSigningKey")));
                if (wxInfo != null && string.IsNullOrEmpty(wxInfo.openid))
                {
                    var claims = new Claim[] {
                        //new Claim(ClaimTypes.Name, "John"),
                        new Claim(JwtRegisteredClaimNames.NameId, wxInfo.openid)
                    };
                    var token = new JwtSecurityToken(
                        issuer: Configuration.GetValue<string>("AppSetting:JwtIssuer"),
                        audience: Configuration.GetValue<string>("AppSetting:JwtAudience"),
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
                    );

                    resultData.JwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                    //获取对应的宾馆 todo
                    var hotel = (from m in HotelContext.Hotelmanager
                                 join h in HotelContext.Hotel
                                 on m.HotelId equals h.Id
                                 where m.IsDel == false && h.IsDel == false && m.WxOpenId == wxInfo.openid
                                 select h).FirstOrDefault();
                    resultData.Hotel = hotel;
                }
            }
            return new Response<Login>() { Status = StatusEnum.Success, Massage = "登录成功", Data = resultData };
        }
        
    }
}