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

        public static string HttpFormPostRequest(string uri,string requestData)
        {
            return HttpRequest(uri, requestData, "POST", "application/x-www-form-urlencoded");
        }

        public static string HttpRequest(string uri,string requestContent,string method,string contentType)
        {
            Uri adress = new Uri(uri);
            HttpWebRequest requestHttp = WebRequest.Create(adress) as HttpWebRequest;
            requestHttp.Method = method;
            requestHttp.ContentType = contentType;
            byte[] requestData = Encoding.Default.GetBytes(requestContent);
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

        //// 身份证识别
        //public static string idcard()
        //{
        //    string token = "#####调用鉴权接口获取的token#####";
        //    //string strbaser64 = FileUtils.getFileBase64("/work/ai/images/ocr/idcard.jpeg"); // 图片的base64编码
        //    string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/idcard?access_token=" + token;
        //    Encoding encoding = Encoding.Default;
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
        //    request.Method = "post";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.KeepAlive = true;
        //    String str = "id_card_side=front&image=" + HttpUtility.UrlEncode(strbaser64);
        //    byte[] buffer = encoding.GetBytes(str);
        //    request.ContentLength = buffer.Length;
        //    request.GetRequestStream().Write(buffer, 0, buffer.Length);
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
        //    string result = reader.ReadToEnd();
        //    //Console.WriteLine("身份证识别:");
        //    //Console.WriteLine(result);
        //    return result;
        //}


        //private string PostWebRequest(string postUrl, string paramData)
        //{
        //    string ret = string.Empty;
        //    try
        //    {
        //        if (!postUrl.StartsWith("http://"))
        //            return "";
        //        byte[] byteArray = Encoding.Default.GetBytes(paramData);
        //        HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
        //        webReq.Method = "POST";
        //        webReq.ContentType = "application/x-www-form-urlencoded";
        //        webReq.ContentLength = byteArray.Length;
        //        Stream newStream = webReq.GetRequestStream();
        //        newStream.Write(byteArray, 0, byteArray.Length);//写入参数               
        //        newStream.Close();
        //        HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
        //        StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        //        ret = sr.ReadToEnd();
        //        sr.Close();
        //        response.Close();
        //        newStream.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //    return ret;
        //}

    }
}
