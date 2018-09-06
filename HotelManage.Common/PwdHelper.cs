using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManage.Common
{
    public class PwdHelper
    {
        /// <summary>
        /// SHA256+盐 生成密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GetPassword(string password,string salt)
        {
            byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
