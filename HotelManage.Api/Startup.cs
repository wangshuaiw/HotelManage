using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManage.Api.Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using HotelManage.DBModel;
using HotelManage.Interface;
using HotelManage.BLL;

namespace HotelManage.Api
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string signingKey = Configuration.GetValue<string>("AppSetting:JwtSigningKey");
            string issuer = Configuration.GetValue<string>("AppSetting:JwtIssuer");
            string audience = Configuration.GetValue<string>("AppSetting:JwtAudience");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true, //validate the expiration and not before values in the token
                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });

            services.AddMvc(options=> {
                options.Filters.Add<GlobalActionFilter>();
                options.Filters.Add<GlobalExceptionFilter>();
            });

            var connection = Configuration.GetValue<string>("ConnectionStrings:HotelManageConnection"); 
            services.AddDbContext<hotelmanageContext>(options => options.UseMySql(connection));

            services.AddScoped<IHotelManagerHander, HotelManagerHander>();
            services.AddScoped<IHotelHander, HotelHander>();
            services.AddScoped<IRoomHander, RoomHander>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes => routes.MapRoute("default", "api/{controller=Values}/{action=Get}/{id?}"));
        }
    }
}
