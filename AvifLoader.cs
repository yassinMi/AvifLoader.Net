using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AvifLoaderNET
{







    public class ImageSourceAvifConverter : ImageSourceConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {

            return base.ConvertFrom(context, culture, value);


        }
    }

    public static class AvifLoader
    {


        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool SetDllDirectory(string pathName);


        [DllImport("libMiAvifJpgWraper.dll")]
        static extern int ConvertFile(IntPtr srcFile, IntPtr saveFile);
        [DllImport("libMiAvifJpgWraper.dll")]
        static extern int LoadAvifAsJpg(IntPtr srcFile, out IntPtr buffer, out long size);
        [DllImport("libMiAvifJpgWraper.dll")]
        static extern int LoadAvifAsRgb(IntPtr srcFile, out IntPtr buffer, out int stride, out int width, out int heigh);

        /// <summary>
        /// free the created jpeg buffer (to be used in conjunction with LoadAvifAsRgb or LoadAvifAsJpg)
        /// </summary>
        /// <param name="arr"></param>
        [DllImport("libMiAvifJpgWraper.dll")]

        static extern void RmBuff(IntPtr arr);


        public static int MaxThreads { get; set; }



        static AvifLoader()
        {
            //this is disabled for simplicity. using the 64 dll only
            /*
            var dllDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AvifLoaderNet");
            dllDir = Path.Combine(dllDir, Environment.Is64BitProcess? "x64": "x86");
            SetDllDirectory(dllDir);
            */
        }


        public static byte[] AvifToJpeg(byte[] AvifFileBytes)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// fails quickly with a FileFormatException in case the file is not aviff format
        /// </summary>
        /// <param name="avifFilePath"></param>
        /// <returns></returns>
        unsafe public static BitmapSource AvifToImageSource(string avifFilePath)
        {
            IntPtr buff;
            long size;
            int stride, width, height;
            var res = LoadAvifAsRgb(Marshal.StringToHGlobalAnsi(avifFilePath), out buff, out stride, out width, out height);
            if (res != 0) throw new FileFormatException("Cannot decode avif image file");
            size = stride * height;
            var bf = BitmapFrame.Create(width, height, 96, 96, PixelFormats.Rgb24, BitmapPalettes.WebPalette, buff, (int)size, stride);
            RmBuff(buff);
            return bf;
        }
    }
}
