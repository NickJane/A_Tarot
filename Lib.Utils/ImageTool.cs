using System;
using System.IO;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Lib.Utils
{
    /// <summary>
    /// 图片操作类
    /// </summary>
    public class ImageTool
    { 
        #region 给图片加水印
        /// <summary>
        /// 给图片加水印
        /// 示例： Mark(@"D:\a.jpg", @"D:\logo.jpg", @"D:\b.jpg", 10, 10, 40);
        /// </summary>
        /// <param name="srcPicFileFullName">源文件物理路径</param>
        /// <param name="markPicFileFullName">水印图物理路径</param>
        /// <param name="outPicFileFullName">输出文件物理路径</param>
        /// <param name="rightSpace">水印图在全图的右边距</param>
        /// <param name="bottomSpace">水印图在全图的下边距</param>
        /// <param name="lucencyPercent">透明度 0:全透明 100:不透明</param>
        /// <returns>成功返回true 否则返回false</returns>
        public static bool Mark(string srcPicFileFullName, string markPicFileFullName, string outPicFileFullName, int rightSpace, int bottomSpace, int lucencyPercent)
        {
            bool result = false;
            Image srcImage = null;
            Image maskImage = null;
            Graphics g = null;

            try
            {
                //建立图形对象
                srcImage = Image.FromFile(srcPicFileFullName);
                maskImage = Image.FromFile(markPicFileFullName);
                g = Graphics.FromImage(srcImage);
                //获取要绘制图形坐标
                int x = srcImage.Width - rightSpace - maskImage.Width;
                int y = srcImage.Height - bottomSpace - maskImage.Height;
                //设置颜色矩阵
                float[][] matrixItems ={
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, (float)lucencyPercent/100f, 0},
                    new float[] {0, 0, 0, 0, 1}
                };
                ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
                ImageAttributes imgAttr = new ImageAttributes();
                imgAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                //绘制阴影图像
                g.DrawImage(maskImage, new Rectangle(x, y, maskImage.Width, maskImage.Height),
                    0, 0, maskImage.Width, maskImage.Height, GraphicsUnit.Pixel, imgAttr);

                //保存文件
                string[] allowImageType ={ ".jpg", ".gif", ".png", ".bmp", ".tiff", ".wmf", ".ico" };
                FileInfo file = new FileInfo(srcPicFileFullName);

                ImageFormat imageType = ImageFormat.Gif;
                switch (file.Extension.ToLower())
                {
                    case ".jpg":
                        imageType = ImageFormat.Jpeg;
                        break;
                    case ".gif":
                        imageType = ImageFormat.Gif;
                        break;
                    case ".png":
                        imageType = ImageFormat.Png;
                        break;
                    case ".bmp":
                        imageType = ImageFormat.Bmp;
                        break;
                    case ".tif":
                        imageType = ImageFormat.Tiff;
                        break;
                    case ".wmf":
                        imageType = ImageFormat.Wmf;
                        break;
                    case ".ico":
                        imageType = ImageFormat.Icon;
                        break;
                    default:
                        break;
                }

                MemoryStream ms = new MemoryStream();
                srcImage.Save(ms, imageType);
                byte[] imgData = ms.ToArray();
                srcImage.Dispose();
                maskImage.Dispose();
                g.Dispose();

                FileStream fs = null;

                fs = new FileStream(outPicFileFullName, FileMode.Create, FileAccess.Write);

                if (fs != null)
                {
                    fs.Write(imgData, 0, imgData.Length);
                    fs.Close();
                }

                result = true;
            }
            catch
            {
                result = false;
            }

            finally
            {
                try
                {
                    maskImage.Dispose();
                    srcImage.Dispose();
                    g.Dispose();
                }
                catch { }
            }

            return result;
        }
        /// <summary>
        /// 给图片加水印
        /// 示例：Mark(@"D:\a.jpg", @"D:\logo.jpg", @"D:\b.jpg", 3, 4, 20, 10); 
        /// </summary>
        /// <param name="srcPicFileFullName">源文件物理路径</param>
        /// <param name="markPicFileFullName">水印图物理路径</param>
        /// <param name="outPicFileFullName">输出文件物理路径</param>
        /// <param name="rightSpacePercent">水印图右边距占全图宽度的百分比</param>
        /// <param name="bottomSpacePercnet">水印图下边距占全图高度的百分比</param>
        /// <param name="maskWidthSizePercent">水印图宽度占全图宽度的百分比</param>
        /// <param name="lucencyPercent">透明度 0:全透明 100:不透明</param>
        /// <returns>成功返回true 否则返回false</returns>
        public static bool Mark(string srcPicFileFullName, string markPicFileFullName, string outPicFileFullName, int rightSpacePercent, int bottomSpacePercnet, int maskWidthSizePercent, int lucencyPercent)
        {
            bool result = false;
            Image srcImage = null;
            Image maskImage = null;
            Graphics g = null;

            try
            {
                //建立图形对象
                srcImage = Image.FromFile(srcPicFileFullName);
                maskImage = Image.FromFile(markPicFileFullName);
                g = Graphics.FromImage(srcImage);

                int rectWidth = Convert.ToInt32(srcImage.Width * ((float)maskWidthSizePercent / 100f));
                int rectHeight = Convert.ToInt32(maskImage.Height * ((float)rectWidth / maskImage.Width));
                int rightSpace = Convert.ToInt32(srcImage.Width * ((float)rightSpacePercent / 100f)) + rectWidth;
                int bottomSpace = Convert.ToInt32(srcImage.Height * ((float)bottomSpacePercnet / 100f)) + rectHeight;
                //获取要绘制图形坐标
                int x = srcImage.Width - rightSpace;
                int y = srcImage.Height - bottomSpace;
                //设置颜色矩阵
                float[][] matrixItems ={
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, (float)lucencyPercent/100f, 0},
                    new float[] {0, 0, 0, 0, 1}
                };
                ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
                ImageAttributes imgAttr = new ImageAttributes();
                imgAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                //绘制阴影图像
                g.DrawImage(maskImage, new Rectangle(x, y, rectWidth, rectHeight),
                    0, 0, maskImage.Width, maskImage.Height, GraphicsUnit.Pixel, imgAttr);

                //保存文件
                string[] allowImageType ={ ".jpg", ".gif", ".png", ".bmp", ".tiff", ".wmf", ".ico" };
                FileInfo file = new FileInfo(srcPicFileFullName);

                ImageFormat imageType = ImageFormat.Gif;
                switch (file.Extension.ToLower())
                {
                    case ".jpg":
                        imageType = ImageFormat.Jpeg;
                        break;
                    case ".gif":
                        imageType = ImageFormat.Gif;
                        break;
                    case ".png":
                        imageType = ImageFormat.Png;
                        break;
                    case ".bmp":
                        imageType = ImageFormat.Bmp;
                        break;
                    case ".tif":
                        imageType = ImageFormat.Tiff;
                        break;
                    case ".wmf":
                        imageType = ImageFormat.Wmf;
                        break;
                    case ".ico":
                        imageType = ImageFormat.Icon;
                        break;
                    default:
                        break;
                }

                MemoryStream ms = new MemoryStream();
                srcImage.Save(ms, imageType);
                byte[] imgData = ms.ToArray();
                srcImage.Dispose();
                maskImage.Dispose();
                g.Dispose();

                FileStream fs = null;

                fs = new FileStream(outPicFileFullName, FileMode.Create, FileAccess.Write);

                if (fs != null)
                {
                    fs.Write(imgData, 0, imgData.Length);
                    fs.Close();
                }

                result = true;
            }
            catch
            {
                result = false;
            }

            finally
            {
                try
                {
                    maskImage.Dispose();
                    srcImage.Dispose();
                    g.Dispose();
                }
                catch { }
            }

            return result;
        }

        #endregion 给图片加水印

        #region 生成缩略图

        /// <summary>
        /// 按照尺寸和压缩质量生成缩略图。需要指定缩略图绝对路径。使用默认的压缩质量。
        /// </summary>
        /// <param name="srcFileFullName">源图片url</param>
        /// <param name="disFileFullName">目的图片url</param>
        /// <param name="smallSize">缩略图尺寸</param> 
        /// <returns>生成成功返回true，失败返回false。</returns>
        public static bool MakeSmallImage(string srcFileFullName, string disFileFullName, Size smallSize)
        {
            Image srcImg = null;

            try
            {
                srcImg = Image.FromFile(srcFileFullName);
            }
            catch
            {
                return false;
            }

            return MakeSmallImage(srcImg, disFileFullName, smallSize, 75);
        }

        /// <summary>
        /// 按照尺寸和压缩质量生成缩略图。需要指定缩略图绝对路径。
        /// </summary>
        /// <param name="srcFileFullName">源图片url</param>
        /// <param name="disFileFullName">目的图片url</param>
        /// <param name="smallSize">缩略图尺寸</param>
        /// <param name="quality">缩略图质量。赋值范围0－100。一般75能兼顾质量和图片大小。</param>
        /// <returns>生成成功返回true，失败返回false。</returns>
        public static bool MakeSmallImage(string srcFileFullName,string disFileFullName,Size smallSize,int quality)
        {
            Image srcImg = null;

            try
            {
                srcImg = Image.FromFile(srcFileFullName);
            }
            catch 
            {
                return false;
            }

            return MakeSmallImage(srcImg, disFileFullName, smallSize, quality);         
        }

        /// <summary>
        /// 按照限制的边长和压缩质量生成缩略图。需要指定缩略图绝对路径。使用默认的压缩质量。
        /// 如果图片的边长不大于指定的最大边长，直接拷贝原图作为缩略图。
        /// </summary>
        /// <param name="srcFileFullName">源图片url</param>
        /// <param name="disFileFullName">目的图片url</param>
        /// <param name="maxSideLength">最大边长</param>
        /// <param name="limitMode">最大边长限制模式：按照宽度，高度或最大边长。</param>       
        /// <returns>生成成功返回true，失败返回false。</returns>
        public static bool MakeSmallImage(string srcFileFullName, string disFileFullName, int maxSideLength, LimitSideMode limitMode)
        {
            Image srcImg = null;

            try
            {
                srcImg = Image.FromFile(srcFileFullName);
            }
            catch
            {
                return false;
            }
            decimal compressRate = 1;

            //按照压缩模式计算压缩比例
            switch (limitMode)
            {
                case LimitSideMode.Width:

                    if (srcImg.Width <= maxSideLength)
                    {
                        File.Copy(srcFileFullName, disFileFullName);
                        return true;
                    }

                    compressRate = Convert.ToDecimal(maxSideLength) / srcImg.Width;

                    break;

                case LimitSideMode.Height:

                    if (srcImg.Height <= maxSideLength)
                    {
                        File.Copy(srcFileFullName, disFileFullName);
                        return true;
                    }

                    compressRate = Convert.ToDecimal(maxSideLength) / srcImg.Height;

                    break;

                case LimitSideMode.Auto:
                    if (srcImg.Width >= srcImg.Height)
                    {
                        if (srcImg.Width <= maxSideLength)
                        {
                            File.Copy(srcFileFullName, disFileFullName);
                            return true;
                        }
                        compressRate = Convert.ToDecimal(maxSideLength) / srcImg.Width;
                    }
                    else
                    {
                        if (srcImg.Height <= maxSideLength)
                        {
                            File.Copy(srcFileFullName, disFileFullName);
                            return true;
                        }
                        compressRate = Convert.ToDecimal(maxSideLength) / srcImg.Height;
                    }
                    break;
            }

            //计算缩略图大小
            Size smallSize = new Size(Convert.ToInt32(srcImg.Width * compressRate), Convert.ToInt32(srcImg.Height * compressRate));

            return MakeSmallImage(srcImg, disFileFullName, smallSize, 75);

        }

        /// <summary>
        /// 按照限制的边长和压缩质量生成缩略图。需要指定缩略图绝对路径。
        /// 如果图片的边长不大于指定的最大边长，直接拷贝原图作为缩略图。
        /// </summary>
        /// <param name="srcFileFullName">源图片url</param>
        /// <param name="disFileFullName">目的图片url</param>
        /// <param name="maxSideLength">最大边长</param>
        /// <param name="limitMode">最大边长限制模式：按照宽度，高度或最大边长。</param>
        /// <param name="quality">缩略图质量。赋值范围0－100。一般75能兼顾质量和图片大小。</param>
        /// <returns>生成成功返回true，失败返回false。</returns>
        public static bool MakeSmallImage(string srcFileFullName, string disFileFullName, int maxSideLength, LimitSideMode limitMode, int quality)
        {
            Image srcImg = null;

            try
            {
                srcImg = Image.FromFile(srcFileFullName);
            }
            catch
            {
                return false;
            }
            decimal compressRate = 1;
         
            //按照压缩模式计算压缩比例
            switch (limitMode)
            {
                case LimitSideMode.Width:

                    if (srcImg.Width <= maxSideLength)
                    {
                        File.Copy(srcFileFullName, disFileFullName);
                        return true;
                    }

                    compressRate = Convert.ToDecimal( maxSideLength) / srcImg.Width;     
               
                    break;

                case LimitSideMode.Height:

                    if (srcImg.Height  <= maxSideLength)
                    {
                        File.Copy(srcFileFullName, disFileFullName);
                        return true;
                    }

                    compressRate =  Convert.ToDecimal(maxSideLength )/ srcImg.Height;

                    break;

                case  LimitSideMode.Auto: 
                    if (srcImg.Width >= srcImg.Height)
                    {
                        if (srcImg.Width <= maxSideLength)
                        {
                            File.Copy(srcFileFullName, disFileFullName);
                            return true;
                        }
                        compressRate = Convert.ToDecimal(maxSideLength) / srcImg.Width;
                    }
                    else
                    {
                        if (srcImg.Height <= maxSideLength)
                        {
                            File.Copy(srcFileFullName, disFileFullName);
                            return true;
                        }
                        compressRate = Convert.ToDecimal(maxSideLength) / srcImg.Height;
                    }
                    break; 
            }

            //计算缩略图大小
            Size smallSize = new Size(Convert.ToInt32(srcImg.Width * compressRate), Convert.ToInt32(srcImg.Height * compressRate)); 

            return MakeSmallImage(srcImg, disFileFullName, smallSize, quality);      

        }

        /// <summary>
        /// 按照尺寸和压缩质量生成缩略图
        /// </summary>
        /// <param name="srcImage">源图片</param>
        /// <param name="disFileFullName"></param>
        /// <param name="smallSize"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        private static bool MakeSmallImage(Image srcImage,string disFileFullName, Size smallSize, int quality)
        {
            bool rtn = true;

          
            Bitmap outBmp = null;

            try
            {
                ImageFormat thisFormat = srcImage.RawFormat;
                outBmp = new Bitmap(srcImage, smallSize);
                Graphics g = Graphics.FromImage(outBmp);

                // 设置画布的描绘质量
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(srcImage, new Rectangle(0, 0, smallSize.Width, smallSize.Height), 0, 0, srcImage.Width, srcImage.Height, GraphicsUnit.Pixel);
                g.Dispose();

                // 以下代码为保存图片时,设置压缩质量
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityValue = new long[1];
                qualityValue[0] = quality;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityValue);
                encoderParams.Param[0] = encoderParam;

                //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICI = null;
                for (int i = 0; i < arrayICI.Length; i++)
                {
                    if (arrayICI[i].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[i];
                        //设置JPEG编码
                        break;
                    }
                }

                //保存图片
                if (jpegICI != null)
                {
                    outBmp.Save(disFileFullName, jpegICI, encoderParams);
                }
                else
                {
                    outBmp.Save(disFileFullName, thisFormat);
                }
            }
            catch
            {
                rtn = false;
            }
            finally
            {
                if (srcImage != null)
                {
                    srcImage.Dispose();
                }

                if (outBmp != null)
                {
                    outBmp.Dispose();
                }
            }

            return rtn;
        }

        /// <summary>
        /// 生成缩略图时的最大边长限制模式：按照宽度，高度或最大边长。
        /// </summary>
        public enum LimitSideMode
        {
            /// <summary>
            /// 宽度固定
            /// </summary>
            Width =0,

            /// <summary>
            /// 高度固定
            /// </summary>
            Height =1,
            
            /// <summary>
            /// 按照最大边长
            /// </summary>
            Auto =2
        }

        #endregion 
    }
}