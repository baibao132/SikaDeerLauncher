using Newtonsoft.Json;
using SquareMinecraftLauncher;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace mcbbs
{
    public sealed class mcbbsnews
    {
        const string a = "	<div id=\"slideshow_3\" class=\"slideshow\" style=\"overflow: hidden;\"><div class=\"slideshow_item\">";
        const string a1 = "        <!--版主推荐-->";
        const string b = "		</div><div class=\"slideshow_item\">";
        const string c = "<div class=\"image\"><a href=\"";
        const string c1 = "\"  title=\"";
        const string d = "\"  target=\"_blank\">";
        const string e = "<img src=\"";
        const string e1 = "\" alt=\"";
        string url = "";
        Download web = new Download();
        Download Web = new Download();
        /// <summary>
        /// 取中间内容数组,失败返回false，成功返回true
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="text">内容</param>
        /// <param name="lt">左边</param>
        /// <param name="rt">右边</param>
        /// <returns></returns>
        public bool ArrayTakeTheMiddle(ref string[] array, string text, string lt, string rt)
        {
            if (text == "" && lt == "" && rt == "")
            {
                return false;
            }

            string[] a = Regex.Split(text, lt);
            int mun = 0;

            for (int i = 0; a.Length > i; i++)
            {
                string[] b = Regex.Split(a[i], rt);
                if (b[0] != a[i]) mun++;
            }

            array = new string[mun];
            int mun1 = 0;

            for (int i = 0; a.Length > i; i++)
            {
                string[] b = Regex.Split(a[i], rt);

                if (b[0] != a[i])
                {
                    array[mun1] = b[0];
                    mun1++;
                }
            }

            return true;
        }

        public ImageBrush get_image(string url)
        {
            var image = new ImageBrush();

            try
            {
                var webreq = System.Net.WebRequest.Create(url);
                var webres = webreq.GetResponse();
                var stream = webres.GetResponseStream();
                var img1 = System.Drawing.Image.FromStream(stream);
                var bmp = new System.Drawing.Bitmap(img1);
                IntPtr hBitmap = bmp.GetHbitmap();
                System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                image.ImageSource = WpfBitmap;
                image.Stretch = Stretch.Uniform;
                stream.Dispose();
            }
            catch (Exception e)
            {
                return null;
            }

            return image;
        }

        public bool McbbsAPI(ref API[] API)
        {
            var html = web.getHtml("https://authentication.x-speed.cc/mcbbsNews/");
            List<API> aPIs = new List<API>();

            if (html != null)
            {
                var jArray = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(html);

                foreach (var a in jArray)
                {
                    var Api = new API();
                    Api.title = a["title"].ToString();
                    Api.classify = a["classify"].ToString();
                    Api.link = a["link"].ToString();
                    Api.time = a["time"].ToString();
                    aPIs.Add(Api);
                }

                API = aPIs.ToArray();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 我的世界论坛新闻，成功返回true，失败返回false
        /// </summary>
        /// <param name="news">返回的数组</param>
        /// <returns></returns>
        public bool News(ref newsArray[] news)
        {
            if (url == "")
            {
                url = Web.getHtml("http://www.mcbbs.net/", true);
            }

            string ns;

            if (url != null) ns = TakeTheMiddle(url, a, a1);
            else return false;

            string[] s = Regex.Split(ns, b);
            List<newsArray> ap = new List<newsArray>();

            for (int i = 0; s.Length > i; i++)
            {
                var News = new newsArray();
                News.Url = TakeTheMiddle(s[i], c, c1);
                var ps = News.Url;

                if (ps.IndexOf('n') == -1)
                {
                    News.Url = "http://www.mcbbs.net" + News.Url;
                }

                News.Text = TakeTheMiddle(s[i], c1, d);
                News.IMG = TakeTheMiddle(s[i], e, e1);
                ap.Add(News);
            }

            news = ap.ToArray();
            return true;
        }

        /// <summary>
        /// mcbbs推荐新闻，成功返回true，失败返回false
        /// </summary>
        /// <param name="news">newArray数组</param>
        /// <returns></returns>
        public bool Recommend(ref newsArray[] news)
        {
            const string stra1 = "        <!--版主推荐-->";
            const string stra2 = "        <!--视频实况-->";
            const string strb1 = "           <a href=\"";
            const string strb2 = "\" class=\"";
            const string strc1 = "\"><img src=\"	";
            const string strc2 = "\" alt=\"";
            const string strd1 = "\" title=\"";

            if (url == "")
            {
                url = Web.getHtml("http://www.mcbbs.net/", true);
            }

            string ns;

            if (url != null)
            {
                ns = TakeTheMiddle(url, stra1, stra2);
            }
            else
            {
                return false;
            }

            const string stra3 = "<div class=\"portal_li\">";
            const string stra4 = "\"/></a>";
            string[] str1 = new string[0];             ArrayTakeTheMiddle(ref str1, ns, stra3, stra4);
            List<newsArray> ap = new List<newsArray>();

            for (int i = 0; str1.Length > i; i++)
            {
                var News = new newsArray();
                News.Url = TakeTheMiddle(str1[i], strb1, strb2);
                var ps = News.Url;

                if (ps.IndexOf('n') == -1)
                {
                    if (ps.IndexOf('/') == -1)
                    {
                        News.Url = "http://www.mcbbs.net/" + News.Url;
                    }
                    else
                    {
                        News.Url = "http://www.mcbbs.net" + News.Url;
                    }
                }

                News.IMG = TakeTheMiddle(str1[i], strc1, strc2);
                News.Text = TakeTheMiddle(str1[i], strc2, strd1);
                ap.Add(News);
            }

            news = ap.ToArray();
            return true;
        }

        /// <summary>
        /// 取字符串中间内容
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="LeftText">左边字符串</param>
        /// <param name="RightText">右边字符串</param>
        /// <returns></returns>
        public string TakeTheMiddle(string text, string LeftText, string RightText)
        {
            if (text != "" && LeftText != "" && RightText != "")
            {
                string[] str1 = Regex.Split(text, LeftText);

                if (str1.Length == 1 && str1[0] == text)
                {
                    return "";
                }
                else
                {
                    string[] str3 = Regex.Split(str1[1], RightText);

                    if (str3.Length == 1 && str3[0] == str1[1])
                    {
                        return "";
                    }

                    return str3[0];
                }
            }
            else
            {
                return "";
            }
        }
        public sealed class API
        {
            /// <summary>
            /// 资讯类型
            /// </summary>
            public string classify { get; internal set; }

            /// <summary>
            /// 网址
            /// </summary>
            public string link { get; internal set; }

            /// <summary>
            /// 时间
            /// </summary>
            public string time { get; internal set; }

            /// <summary>
            /// 标题
            /// </summary>
            public string title { get; internal set; }
        }

        public class newsArray
        {
            public string IMG { get; set; }
            public string Text { get; set; }
            public string Url { get; set; }
        }
    }
} 