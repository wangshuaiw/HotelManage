using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotelManage.Common;
using HotelManage.DBModel;
using HotelManage.Interface;
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
        public IHotelManagerHander Hander { get; }
        public IHotelHander HotelHander { get; }

        public LoginController(IConfiguration configuration, ILogger<LoginController> logger, IHotelManagerHander hander,IHotelHander hotelHander)

        {
            Configuration = configuration;
            Logger = logger;
            Hander = hander;
            HotelHander = hotelHander;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<Response<Login>> WxLogin([FromBody]dynamic request)
        {
            string wxcode = request.wxcode;

            Login resultData = new Login();
            string appid = Configuration.GetValue<string>("AppSetting:WxAppid");
            string secret = Configuration.GetValue<string>("AppSetting:WxSecret");
            string uri = $"https://api.weixin.qq.com/sns/jscode2session?appid={appid}&secret={secret}&js_code={wxcode}&grant_type=authorization_code";
            string response = await Task.Run(() => { return HttpHelper.HttpJsonGetRequest(uri); });
            if (!string.IsNullOrEmpty(response))
            {
                WxLoginInfo wxInfo = JsonConvert.DeserializeObject<WxLoginInfo>(response);

                if (wxInfo != null && !string.IsNullOrEmpty(wxInfo.openid))
                {
                    resultData.OpenId = wxInfo.openid;
                    resultData.UnionId = wxInfo.unionid;

                    var claims = new Claim[] {
                        new Claim(ClaimTypes.Name, wxInfo.openid),
                        //new Claim(JwtRegisteredClaimNames.NameId, wxInfo.openid),
                        //new Claim(JwtRegisteredClaimNames.UniqueName,string.IsNullOrEmpty(wxInfo.unionid)?"":wxInfo.unionid)
                    };

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("AppSetting:JwtSigningKey")));
                    var token = new JwtSecurityToken(
                        issuer: Configuration.GetValue<string>("AppSetting:JwtIssuer"),
                        audience: Configuration.GetValue<string>("AppSetting:JwtAudience"),
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
                    );

                    resultData.JwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                    //获取对应的宾馆
                    //var hotel = Hander.GetHotelByOpenId(wxInfo.openid);


                    var manager = await Task.Run(() =>
                    {
                        return Hander.Get(m => m.WxOpenId == wxInfo.openid && m.IsDel.HasValue && !m.IsDel.Value);
                    });
                    if(manager!=null)
                    {
                        resultData.Role = manager.Role;
                        var hotel = await Task.Run(() => { return HotelHander.Get(h => h.Id == manager.HotelId); });
                        resultData.Hotel = hotel;
                    }
                    else
                    {
                        resultData.Role = -1;
                    }
                }
                else
                {
                    throw new Exception("微信登录接口返回异常！" + response);
                }
            }
            else
            {
                throw new Exception("微信登录接口返回为空");
            }
            return new Response<Login>() { Status = StatusEnum.Success, Massage = "登录成功", Data = resultData };
        }

        
    }
}