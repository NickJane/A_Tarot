using System;
using System.IO;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Lib.Utils
{
    /// <summary>
    /// ͼƬ������
    /// </summary>
    public class ImageTool
    { 
        #region ��ͼƬ��ˮӡ
        /// <summary>
        /// ��ͼƬ��ˮӡ
        /// ʾ���� Mark(@"D:\a.jpg", @"D:\logo.jpg", @"D:\b.jpg", 10, 10, 40);
        /// </summary>
        /// <param name="srcPicFileFullName">Դ�ļ�����·��</param>
        /// <param name="markPicFileFullName">ˮӡͼ����·��</param>
        /// <param name="outPicFileFullName">����ļ�����·��</param>
        /// <param name="rightSpace">ˮӡͼ��ȫͼ���ұ߾�</param>
        /// <param name="bottomSpace">ˮӡͼ��ȫͼ���±߾�</param>
        /// <param name="lucencyPercent">͸���� 0:ȫ͸�� 100:��͸��</param>
        /// <returns>�ɹ�����true ���򷵻�false</returns>
        public static bool Mark(string srcPicFileFullName, string markPicFileFullName, string outPicFileFullName, int rightSpace, int bottomSpace, int lucencyPercent)
        {
            bool result = false;
            Image srcImage = null;
            Image maskImage = null;
            Graphics g = null;

            try
            {
                //����ͼ�ζ���
                srcImage = Image.FromFile(srcPicFileFullName);
                maskImage = Image.FromFile(markPicFileFullName);
                g = Graphics.FromImage(srcImage);
                //��ȡҪ����ͼ������
                int x = srcImage.Width - rightSpace - maskImage.Width;
                int y = srcImage.Height - bottomSpace - maskImage.Height;
                //������ɫ����
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
                //������Ӱͼ��
                g.DrawImage(maskImage, new Rectangle(x, y, maskImage.Width, maskImage.Height),
                    0, 0, maskImage.Width, maskImage.Height, GraphicsUnit.Pixel, imgAttr);

                //�����ļ�
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
        /// ��ͼƬ��ˮӡ
        /// ʾ����Mark(@"D:\a.jpg", @"D:\logo.jpg", @"D:\b.jpg", 3, 4, 20, 10); 
        /// </summary>
        /// <param name="srcPicFileFullName">Դ�ļ�����·��</param>
        /// <param name="markPicFileFullName">ˮӡͼ����·��</param>
        /// <param name="outPicFileFullName">����ļ�����·��</param>
        /// <param name="rightSpacePercent">ˮӡͼ�ұ߾�ռȫͼ��ȵİٷֱ�</param>
        /// <param name="bottomSpacePercnet">ˮӡͼ�±߾�ռȫͼ�߶ȵİٷֱ�</param>
        /// <param name="maskWidthSizePercent">ˮӡͼ���ռȫͼ��ȵİٷֱ�</param>
        /// <param name="lucencyPercent">͸���� 0:ȫ͸�� 100:��͸��</param>
        /// <returns>�ɹ�����true ���򷵻�false</returns>
        public static bool Mark(string srcPicFileFullName, string markPicFileFullName, string outPicFileFullName, int rightSpacePercent, int bottomSpacePercnet, int maskWidthSizePercent, int lucencyPercent)
        {
            bool result = false;
            Image srcImage = null;
            Image maskImage = null;
            Graphics g = null;

            try
            {
                //����ͼ�ζ���
                srcImage = Image.FromFile(srcPicFileFullName);
                maskImage = Image.FromFile(markPicFileFullName);
                g = Graphics.FromImage(srcImage);

                int rectWidth = Convert.ToInt32(srcImage.Width * ((float)maskWidthSizePercent / 100f));
                int rectHeight = Convert.ToInt32(maskImage.Height * ((float)rectWidth / maskImage.Width));
                int rightSpace = Convert.ToInt32(srcImage.Width * ((float)rightSpacePercent / 100f)) + rectWidth;
                int bottomSpace = Convert.ToInt32(srcImage.Height * ((float)bottomSpacePercnet / 100f)) + rectHeight;
                //��ȡҪ����ͼ������
                int x = srcImage.Width - rightSpace;
                int y = srcImage.Height - bottomSpace;
                //������ɫ����
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
                //������Ӱͼ��
                g.DrawImage(maskImage, new Rectangle(x, y, rectWidth, rectHeight),
                    0, 0, maskImage.Width, maskImage.Height, GraphicsUnit.Pixel, imgAttr);

                //�����ļ�
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

        #endregion ��ͼƬ��ˮӡ

        #region ��������ͼ

        /// <summary>
        /// ���ճߴ��ѹ��������������ͼ����Ҫָ������ͼ����·����ʹ��Ĭ�ϵ�ѹ��������
        /// </summary>
        /// <param name="srcFileFullName">ԴͼƬurl</param>
        /// <param name="disFileFullName">Ŀ��ͼƬurl</param>
        /// <param name="smallSize">����ͼ�ߴ�</param> 
        /// <returns>���ɳɹ�����true��ʧ�ܷ���false��</returns>
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
        /// ���ճߴ��ѹ��������������ͼ����Ҫָ������ͼ����·����
        /// </summary>
        /// <param name="srcFileFullName">ԴͼƬurl</param>
        /// <param name="disFileFullName">Ŀ��ͼƬurl</param>
        /// <param name="smallSize">����ͼ�ߴ�</param>
        /// <param name="quality">����ͼ��������ֵ��Χ0��100��һ��75�ܼ��������ͼƬ��С��</param>
        /// <returns>���ɳɹ�����true��ʧ�ܷ���false��</returns>
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
        /// �������Ƶı߳���ѹ��������������ͼ����Ҫָ������ͼ����·����ʹ��Ĭ�ϵ�ѹ��������
        /// ���ͼƬ�ı߳�������ָ�������߳���ֱ�ӿ���ԭͼ��Ϊ����ͼ��
        /// </summary>
        /// <param name="srcFileFullName">ԴͼƬurl</param>
        /// <param name="disFileFullName">Ŀ��ͼƬurl</param>
        /// <param name="maxSideLength">���߳�</param>
        /// <param name="limitMode">���߳�����ģʽ�����տ�ȣ��߶Ȼ����߳���</param>       
        /// <returns>���ɳɹ�����true��ʧ�ܷ���false��</returns>
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

            //����ѹ��ģʽ����ѹ������
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

            //��������ͼ��С
            Size smallSize = new Size(Convert.ToInt32(srcImg.Width * compressRate), Convert.ToInt32(srcImg.Height * compressRate));

            return MakeSmallImage(srcImg, disFileFullName, smallSize, 75);

        }

        /// <summary>
        /// �������Ƶı߳���ѹ��������������ͼ����Ҫָ������ͼ����·����
        /// ���ͼƬ�ı߳�������ָ�������߳���ֱ�ӿ���ԭͼ��Ϊ����ͼ��
        /// </summary>
        /// <param name="srcFileFullName">ԴͼƬurl</param>
        /// <param name="disFileFullName">Ŀ��ͼƬurl</param>
        /// <param name="maxSideLength">���߳�</param>
        /// <param name="limitMode">���߳�����ģʽ�����տ�ȣ��߶Ȼ����߳���</param>
        /// <param name="quality">����ͼ��������ֵ��Χ0��100��һ��75�ܼ��������ͼƬ��С��</param>
        /// <returns>���ɳɹ�����true��ʧ�ܷ���false��</returns>
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
         
            //����ѹ��ģʽ����ѹ������
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

            //��������ͼ��С
            Size smallSize = new Size(Convert.ToInt32(srcImg.Width * compressRate), Convert.ToInt32(srcImg.Height * compressRate)); 

            return MakeSmallImage(srcImg, disFileFullName, smallSize, quality);      

        }

        /// <summary>
        /// ���ճߴ��ѹ��������������ͼ
        /// </summary>
        /// <param name="srcImage">ԴͼƬ</param>
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

                // ���û������������
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(srcImage, new Rectangle(0, 0, smallSize.Width, smallSize.Height), 0, 0, srcImage.Width, srcImage.Height, GraphicsUnit.Pixel);
                g.Dispose();

                // ���´���Ϊ����ͼƬʱ,����ѹ������
                EncoderParameters encoderParams = new EncoderParameters();
                long[] qualityValue = new long[1];
                qualityValue[0] = quality;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityValue);
                encoderParams.Param[0] = encoderParam;

                //��ð����й�����ͼ��������������Ϣ��ImageCodecInfo ����.
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICI = null;
                for (int i = 0; i < arrayICI.Length; i++)
                {
                    if (arrayICI[i].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[i];
                        //����JPEG����
                        break;
                    }
                }

                //����ͼƬ
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
        /// ��������ͼʱ�����߳�����ģʽ�����տ�ȣ��߶Ȼ����߳���
        /// </summary>
        public enum LimitSideMode
        {
            /// <summary>
            /// ��ȹ̶�
            /// </summary>
            Width =0,

            /// <summary>
            /// �߶ȹ̶�
            /// </summary>
            Height =1,
            
            /// <summary>
            /// �������߳�
            /// </summary>
            Auto =2
        }

        #endregion 
    }
}