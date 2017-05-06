using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuoteCreator.Controllers
{
    public class QuoteController : ApiController
    {
        private Quote quote;

        [Route("api/CreateQuote")]
        public void Get()
        {
            try
            {
                var resp = (HttpWebRequest.Create(Strings.RandomURL)).GetResponse();
                if (resp.ContentLength != 0)
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        string data = sr.ReadToEnd();
                        quote = Quote.GetQuote(data);
                        CreateImage();
                    }
                }
            }
            catch (WebException) { }
        }

        private void CreateImage()
        {
            Image img = new Bitmap(1366, 768, PixelFormat.Format24bppRgb);
            using (Graphics grp = Graphics.FromImage(img))
            {
                grp.FillRectangle(
                    Brushes.Black, 0, 0, img.Width, img.Height);
            }
            Graphics graphicsImage = Graphics.FromImage(img);

            StringFormat stringformat = new StringFormat();
            stringformat.Alignment = StringAlignment.Center;
            stringformat.LineAlignment = StringAlignment.Center;

            var f1 = new Font("Segoe UI", 30, FontStyle.Bold);
            var f2 = new Font("Segoe UI", 40, FontStyle.Bold);
            Graphics g = Graphics.FromImage(img);
            SizeF sizeOfString1 = new SizeF();
            sizeOfString1 = g.MeasureString(quote.Author, f1);
            SizeF sizeOfString2 = new SizeF();
            sizeOfString2 = g.MeasureString(quote.Content, f1, 700);

            Color StringColor = ColorTranslator.FromHtml(Strings.QuoteColor);
            var placement_Y1 = Convert.ToInt32(Math.Floor((img.Height / 2) - ((sizeOfString1.Height + sizeOfString2.Height + 10) / 2)));
      
            Point p1 = new Point(img.Width / 2, placement_Y1);
            Point p2 = new Point(img.Width / 2, 34);

            RectangleF rf = new RectangleF(33, p2.Y, 1300, 700);
            graphicsImage.DrawString(quote.Author, new Font("Segoe UI", 30, FontStyle.Bold), Brushes.Yellow, p1, stringformat);
            graphicsImage.DrawString(quote.Content, new Font("Segoe UI", 40, FontStyle.Bold), new SolidBrush(StringColor), rf, stringformat);

            graphicsImage.Save();
            img.Save(Strings.ImagePath + quote.ID + ".jpg", ImageFormat.Jpeg);
        }
    }
    class Quote
    {
        [JsonProperty("ID")]
        public string ID { get; set; }
        [JsonProperty("title")]
        public string Author { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }

        public static Quote GetQuote(string data)
        {
            Quote q = JsonConvert.DeserializeObject<List<Quote>>(data)[0];
            q.Content = q.Content.Replace("<p>", "");
            q.Content = q.Content.Replace("</p>", "");
            q.Content = q.Content.Replace("&#8217;", "'");
            return q;
        }
    }
    class Strings
    {
        public static string QuoteColor = "#16B6DF";
        public static string RandomURL = "http://quotesondesign.com/wp-json/posts?filter[orderby]=rand&filter[posts_per_page]=1";
        public static string MultiURL = "http://quotesondesign.com/wp-json/posts?filter[orderby]=rand&filter[posts_per_page]=";
        public static string ImagePath = @"D:\Wallpapers\";
    }
}