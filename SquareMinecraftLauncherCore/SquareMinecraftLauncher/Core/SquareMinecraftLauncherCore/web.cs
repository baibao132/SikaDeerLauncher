namespace SquareMinecraftLauncher
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    public sealed class Download
    {
        public string getHtml(string url)
        {
            Stream stream;
            var client = new WebClient();
            try
            {
                stream = client.OpenRead(url);
                stream.ReadTimeout = 1000;
            }
            catch (WebException exception1)
            {
                var message = exception1.Message;
                return null;
            }
            var reader = new StreamReader(stream, Encoding.UTF8);
            try
            {
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal HttpWebResponse CreateGetHttpResponse(string url)
        {
            var request1 = (HttpWebRequest)WebRequest.Create(url);
            request1.Timeout = 0xbb8;
            request1.ContentType = "text/html;chartset=UTF-8";
            request1.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 48.0) Gecko / 20100101 Firefox / 48.0";
            request1.Method = "GET";
            return (HttpWebResponse)request1.GetResponse();
        }
        internal string getHtml(string url, bool WL)
        {
            if (WL == true)
            {
                if (!Core.ping.CheckServeStatus()) return null;
            }
            return getHtml(url);
        }

        internal string Post(string URL, string jsonParas) => Post(URL, jsonParas, "application/json");

        internal string Post(string URL, string jsonParas, string Type)
        {
            Stream requestStream;
            HttpWebResponse response;
            var request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = Type;
            request.ReadWriteTimeout = 1000;
            var s = jsonParas;
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            request.ContentLength = bytes.Length;
            try
            {
                requestStream = request.GetRequestStream();
            }
            catch (Exception)
            {
                requestStream = null;
                return "";
            }
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException exception2)
            {
                response = exception2.Response as HttpWebResponse;
            }
            var reader1 = new StreamReader(response.GetResponseStream());
            var str2 = reader1.ReadToEnd();
            reader1.Close();
            return str2;
        }
    }
}