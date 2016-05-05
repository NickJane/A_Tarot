using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.Utils;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace TarotWinServicesFramework.Implement
{
    public class CompressImage : IWindowTask
    {
        static string imgpath = @"D:\wwwwr\Tarot\Content\Images\PostsCardF";
        public void Run(params object[] parms)
        {
            var dic =new System.IO.DirectoryInfo(imgpath);
            var files = dic.GetFileSystemInfos("*.jpg");

            var images = files.Where(x => (DateTime.Now - x.CreationTime).Days < 1);
            foreach (FileInfo  item in images)
            {
                if (item.Length > 1024 * 40) {
                    Image img = Image.FromFile(item.FullName);
                    Bitmap bitmap = new Bitmap(img);
                    var newname=System.Guid.NewGuid().ToString();
                    CompressAsJPG(bitmap, imgpath +"\\temp"+newname + ".jpg", 50);
                    bitmap.Dispose();
                    img.Dispose();

                    var tempname = item.FullName.Split('\\').Last().Split('.').First();
                    File.Delete(item.FullName);
                    FileInfo info = new FileInfo(imgpath + "\\temp" + newname + ".jpg");
                    info.CopyTo(imgpath + "\\" + tempname + ".jpg");
                    File.Delete(info.FullName);
                }
            }
            
        }

        /// <summary>
        /// 获取指定mimeType的ImageCodecInfo
        /// </summary>
        private static ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /**/
        /// <summary>
        ///  获取inputStream中的Bitmap对象
        /// </summary>
        public static Bitmap GetBitmapFromStream(Stream inputStream)
        {
            Bitmap bitmap = new Bitmap(inputStream);
            return bitmap;
        }

        /**/
        /// <summary>
        /// 将Bitmap对象压缩为JPG图片类型
        /// </summary>
        /// </summary>
        /// <param name="bmp">源bitmap对象</param>
        /// <param name="saveFilePath">目标图片的存储地址</param>
        /// <param name="quality">压缩质量，越大照片越清晰，推荐80</param>
        public static void CompressAsJPG(Bitmap bmp, string saveFilePath, int quality)
        {
            EncoderParameter p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality); ;
            EncoderParameters ps = new EncoderParameters(1);
            ps.Param[0] = p;
            bmp.Save(saveFilePath, GetImageCodecInfo("image/jpeg"), ps);
            bmp.Dispose();
        }

        /**/
        /// <summary>
        /// 将inputStream中的对象压缩为JPG图片类型
        /// </summary>
        /// <param name="inputStream">源Stream对象</param>
        /// <param name="saveFilePath">目标图片的存储地址</param>
        /// <param name="quality">压缩质量，越大照片越清晰，推荐80</param>
        public static void CompressAsJPG(Stream inputStream, string saveFilePath, int quality)
        {
            Bitmap bmp = GetBitmapFromStream(inputStream);
            CompressAsJPG(bmp, saveFilePath, quality);
        }


        /**/
        /// <summary>
        /// 生成缩略图（JPG 格式）
        /// </summary>
        /// <param name="inputStream">包含图片的Stream</param>
        /// <param name="saveFilePath">目标图片的存储地址</param>
        /// <param name="width">缩略图的宽</param>
        /// <param name="height">缩略图的高</param>
        public static void ThumbAsJPG(Stream inputStream, string saveFilePath, int width, int height)
        {
            Image image = Image.FromStream(inputStream);
            if (image.Width == width && image.Height == height)
            {
                CompressAsJPG(inputStream, saveFilePath, 80);
            }
            int tWidth, tHeight, tLeft, tTop;
            double fScale = (double)height / (double)width;
            if (((double)image.Width * fScale) > (double)image.Height)
            {
                tWidth = width;
                tHeight = (int)((double)image.Height * (double)tWidth / (double)image.Width);
                tLeft = 0;
                tTop = (height - tHeight) / 2;
            }
            else
            {
                tHeight = height;
                tWidth = (int)((double)image.Width * (double)tHeight / (double)image.Height);
                tLeft = (width - tWidth) / 2;
                tTop = 0;
            }
            if (tLeft < 0) tLeft = 0;
            if (tTop < 0) tTop = 0;

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);

            //可以在这里设置填充背景颜色
            graphics.Clear(Color.White);
            graphics.DrawImage(image, new Rectangle(tLeft, tTop, tWidth, tHeight));
            image.Dispose();
            try
            {
                CompressAsJPG(bitmap, saveFilePath, 80);
            }
            catch
            {
                ;
            }
            finally
            {
                bitmap.Dispose();
                graphics.Dispose();
            }
        }

        /**/
        /// <summary>
        /// 将Bitmap对象裁剪为指定JPG文件
        /// </summary>
        /// <param name="bmp">源bmp对象</param>
        /// <param name="saveFilePath">目标图片的存储地址</param>
        /// <param name="x">开始坐标x，单位：像素</param>
        /// <param name="y">开始坐标y，单位：像素</param>
        /// <param name="width">宽度：像素</param>
        /// <param name="height">高度：像素</param>
        public static void CutAsJPG(Bitmap bmp, string saveFilePath, int x, int y, int width, int height)
        {
            int bmpW = bmp.Width;
            int bmpH = bmp.Height;

            if (x >= bmpW || y >= bmpH)
            {
                CompressAsJPG(bmp, saveFilePath, 80);
                return;
            }

            if (x + width > bmpW)
            {
                width = bmpW - x;
            }

            if (y + height > bmpH)
            {
                height = bmpH - y;
            }

            Bitmap bmpOut = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmpOut);
            g.DrawImage(bmp, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            g.Dispose();
            bmp.Dispose();
            CompressAsJPG(bmpOut, saveFilePath, 80);
        }

        /**/
        /// <summary>
        /// 将Stream中的对象裁剪为指定JPG文件
        /// </summary>
        /// <param name="inputStream">源bmp对象</param>
        /// <param name="saveFilePath">目标图片的存储地址</param>
        /// <param name="x">开始坐标x，单位：像素</param>
        /// <param name="y">开始坐标y，单位：像素</param>
        /// <param name="width">宽度：像素</param>
        /// <param name="height">高度：像素</param>
        public static void CutAsJPG(Stream inputStream, string saveFilePath, int x, int y, int width, int height)
        {
            Bitmap bmp = GetBitmapFromStream(inputStream);
            CutAsJPG(bmp, saveFilePath, x, y, width, height);
        }
        
    }
}
