using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HotelManage.Common
{
    public class HttpHelper
    {
        public static string HttpJsonGetRequest(string uri)
        {
            Uri adress = new Uri(uri);
            HttpWebRequest requestHttp = WebRequest.Create(adress) as HttpWebRequest;
            requestHttp.Method = "GET";
            requestHttp.ContentType = "application/json";
            string responeData;
            using (HttpWebResponse response = requestHttp.GetResponse() as HttpWebResponse)
            {
                StreamReader sr = new StreamReader(response.GetResponseStream());
                responeData = sr.ReadToEnd();
            }
            return responeData;
        }

        public static string HttpJsonPostRequest(string uri,string requestJson)
        {
            return HttpRequest(uri, requestJson, "POST", "application/json");
        }

        public static string HttpRequest(string uri,string requestContent,string method,string contentType)
        {
            Uri adress = new Uri(uri);
            HttpWebRequest requestHttp = WebRequest.Create(adress) as HttpWebRequest;
            requestHttp.Method = method;
            requestHttp.ContentType = contentType;
            byte[] requestData = Encoding.UTF8.GetBytes(requestContent);
            requestHttp.ContentLength = requestData.Length;

            using (Stream postStream = requestHttp.GetRequestStream())
            {
                postStream.Write(requestData, 0, requestData.Length);
            }
            string responeData;
            using (HttpWebResponse response = requestHttp.GetResponse() as HttpWebResponse)
            {
                StreamReader sr = new StreamReader(response.GetResponseStream());
                responeData = sr.ReadToEnd();
            }
            return responeData;
        }
    }
}
