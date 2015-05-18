using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Drawing;
using System.Security.Cryptography;

namespace ReaderV2.Helper
{
    class Switch
    {
        /*************************************************************************  图片与字节流间的转换  ******************************************************************/
        /// <summary>
        /// 图片转换成字节流
        /// </summary>
        /// <param name="img">要转换的Image对象</param>
        /// <returns>转换后返回的字节流</returns>
        public static byte[] ImgToByt(Image img)
        {
            MemoryStream ms = new MemoryStream();
            byte[] imagedata = null;
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            imagedata = ms.GetBuffer();
            return imagedata;
        }

        /// <summary>
        /// 字节流转换成图片
        /// </summary>
        /// <param name="byt">要转换的字节流</param>
        /// <returns>转换得到的Image对象</returns>
        public static Image BytToImg(byte[] byt)
        {
            MemoryStream ms = new MemoryStream(byt);
            Image img = Image.FromStream(ms);
            return img;
        }

        //
        /// <summary>
        /// 根据图片路径返回图片的字节流byte[]
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns>返回的字节流</returns>
        public static byte[] getImageByte(string imagePath)
        {
            FileStream files = new FileStream(imagePath, FileMode.Open);
            byte[] imgByte = new byte[files.Length];
            files.Read(imgByte, 0, imgByte.Length);
            files.Close();
            return imgByte;
        }



        /*************************************************************************  DateTime与UTC间的转换  ******************************************************************/
        // 时间戳转为C#格式时间
        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dateTimeStart.Add(toNow);
        }

        // DateTime时间格式转换为Unix时间戳格式
        public static int DateTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }



    }
}
