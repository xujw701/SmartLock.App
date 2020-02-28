using System;
using Android.Content;
using SmartLock.Infrastructure;
using SmartLock.Presentation.Core;
using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.OS;
using Android.Support.V4.Content;

namespace SmartLock.Presentation.Droid.Support
{
    public class FileHelper
    {
        public static void OpenFileAsIntent(Context context, File url)
        {
            // Create URI
            var file = url;
            var uri = Uri.FromFile(file);

            Intent intent = new Intent(Intent.ActionView);

            var urlString = url.ToString().ToLower();

            // Check what kind of file you are trying to open, by comparing the url with extensions.
            // When the if condition is matched, plugin sets the correct intent (mime) type, 
            // so Android knew what application to use to open the file
            if (urlString.Contains(".doc") || urlString.Contains(".docx"))
            {
                // Word document
                intent.SetDataAndType(uri, "application/msword");
            }
            else if(urlString.Contains(".pdf"))
            {
                // PDF file
                intent.SetDataAndType(uri, "application/pdf");
            }
            else if(urlString.Contains(".ppt") || urlString.Contains(".pptx"))
            {
                // Powerpoint file
                intent.SetDataAndType(uri, "application/vnd.ms-powerpoint");
            }
            else if(urlString.Contains(".xls") || urlString.Contains(".xlsx"))
            {
                // Excel file
                intent.SetDataAndType(uri, "application/vnd.ms-excel");
            }
            else if(urlString.Contains(".zip") || urlString.Contains(".rar"))
            {
                // WAV audio file
                intent.SetDataAndType(uri, "application/x-wav");
            }
            else if(urlString.Contains(".rtf"))
            {
                // RTF file
                intent.SetDataAndType(uri, "application/rtf");
            }
            else if(urlString.Contains(".wav") || urlString.Contains(".mp3"))
            {
                // WAV audio file
                intent.SetDataAndType(uri, "audio/x-wav");
            }
            else if(urlString.Contains(".gif"))
            {
                // GIF file
                intent.SetDataAndType(uri, "image/gif");
            }
            else if(urlString.Contains(".jpg") || urlString.Contains(".jpeg") || urlString.Contains(".png"))
            {
                // JPG file
                intent.SetDataAndType(uri, "image/jpeg");
            }
            else if(urlString.Contains(".txt"))
            {
                // Text file
                intent.SetDataAndType(uri, "text/plain");
            }
            else if(urlString.Contains(".3gp") || urlString.Contains(".mpg") || urlString.Contains(".mpeg") || urlString.Contains(".mpe") || urlString.Contains(".mp4") || urlString.Contains(".avi"))
            {
                // Video files
                intent.SetDataAndType(uri, "video/*");
            }
            else if (urlString.Contains(".xls") || urlString.Contains(".xlsm") || urlString.Contains(".xlsx") || urlString.Contains(".xlt"))
            {
                // Word document
                intent.SetDataAndType(uri, "application/vnd.ms-excel");
            }
            else if (urlString.Contains(".ppt") || urlString.Contains(".pptx"))
            {
                // Word document
                intent.SetDataAndType(uri, "application/vnd.ms-powerpoint");
            }
            else
            {
                //if you want you can also define the intent type for any other file
            
                //additionally use else clause below, to manage other unknown extensions
                //in this case, Android will show all applications installed on the device
                //so you can choose which application to use
                intent.SetDataAndType(uri, "*/*");
            }
        
            intent.AddFlags(ActivityFlags.NewTask);

            try
            {
                context.StartActivity(intent);
            }
            catch (Exception)
            {
                IoC.Resolve<IMessageBoxService>().ShowMessage("Error", "Unsupported file type.");
            }
        }

        public static byte[] GetByteArrayFromFile(File file)
        {
            var byteOutputStream = new ByteArrayOutputStream();
            var inputStream = new FileInputStream(file);
            byte[] buffer = new byte[1024];
            int n;
            while (-1 != (n = inputStream.Read(buffer)))
            {
                byteOutputStream.Write(buffer, 0, n);
            }

            byte[] bufferBytes = byteOutputStream.ToByteArray();

            return bufferBytes;
        }

        public static File GetTmpFile(bool isImage)
        {
            var dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures).AbsolutePath);
            if (!dir.Exists())
            {
                dir.Mkdirs();
            }
            return new File(dir, string.Format("tmp_{0}." + (isImage ? "jpg" : "mp4"), Guid.NewGuid()));
        }

        public static bool DeleteFile(string path)
        {
            var file = new File(path);
            if (file.Exists())
            {
                return file.Delete();
            }
            return false;
        }

        public static string GetFileSizeLabel(long size)
        {
            string hrSize;

            var k = size / 1024.0;
            var m = k / 1024.0;
            var g = m / 1024.0;
            var t = g / 1024.0;

            if (t > 1)
            {
                hrSize = string.Concat(t.ToString("#.##"), " TB");
            }
            else if (g > 1)
            {
                hrSize = string.Concat(g.ToString("#.##"), " GB");
            }
            else if (m > 1)
            {
                hrSize = string.Concat(m.ToString("#.##"), " MB");
            }
            else
            {
                hrSize = string.Concat(k.ToString("#.##"), " KB");
            }

            return hrSize;
        }

        public static Uri GetUriForFile(Context context, File file)
        {
            if (context == null || file == null)
            {
                return null;
            }
            Uri uri;
            if (Build.VERSION.SdkInt >=  BuildVersionCodes.N)
            {
                uri = FileProvider.GetUriForFile(context, context.ApplicationContext.PackageName + ".provider", file);
            }
            else
            {
                uri = Uri.FromFile(file);
            }
            return uri;
        }
    }
}