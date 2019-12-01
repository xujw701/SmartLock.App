using System.IO;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.Provider;
using File = Java.IO.File;
using Uri = Android.Net.Uri;

namespace SmartLock.Presentation.Droid.Support
{
    public static class BitmapHelper
    {
        public static Bitmap DecodeSampledBitmapFromFile(string filepath)
        {
            // First decode with inJustDecodeBounds=true to check dimensions
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeFile(filepath, options);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;
            options.InPreferredConfig = Bitmap.Config.Rgb565;
            options.InDither = true;

            return BitmapFactory.DecodeFile(filepath, options);
        }

        public static Bitmap DecodeSampledBitmapFromFile(string filepath, int reqWidth, int reqHeight)
        {
            // First decode with inJustDecodeBounds=true to check dimensions
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeFile(filepath, options);

            // Calculate inSampleSize
            options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

            // Decode bitmap with inSampleSize set
            options.InJustDecodeBounds = false;
            options.InPreferredConfig = Bitmap.Config.Rgb565;
            options.InDither = true;

            return BitmapFactory.DecodeFile(filepath, options);
        }


        private static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            // Raw height and width of image
            float height = options.OutHeight;
            float width = options.OutWidth;
            double inSampleSize = 1D;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = (int)(height / 2);
                int halfWidth = (int)(width / 2);

                // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
                while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return (int)inSampleSize;
        }

        public static Bitmap ResizeBitmap(Bitmap bitmap, int expectPx)
        {
            var bitmapWidth = bitmap.Width;
            var bitmapHeight = bitmap.Height;

            var longerSide = bitmapHeight >= bitmapWidth ? bitmapHeight : bitmapWidth;

            if (expectPx > longerSide) return bitmap;

            var scale = expectPx / (float)longerSide;
            Matrix matrix = new Matrix();
            matrix.PostScale(scale, scale);
            var resizedBitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmapWidth,
                bitmapHeight, matrix, true);
            return resizedBitmap;
        }

        public static bool IsImage(File file)
        {
            if (file == null || !file.Exists())
            {
                return false;
            }

            var options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeFile(file.Path, options);
            return options.OutWidth != -1 && options.OutHeight != -1;
        }

        public static Bitmap RotateImageIfRequired(Context context, Bitmap img, Uri selectedImage)
        {
            if (selectedImage.Scheme.Equals("content"))
            {
                string[] projection = { MediaStore.Images.ImageColumns.Orientation };

                ICursor cursor = context.ContentResolver.Query(selectedImage, projection, null, null, null);
                if (cursor.MoveToFirst())
                {
                    int rotation = cursor.GetInt(0);
                    cursor.Close();
                    return RotateImage(img, rotation);
                }
                return img;
            }
            else
            {
                ExifInterface ei = new ExifInterface(selectedImage.Path);

                int orientation = ei.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Normal);

                switch (orientation)
                {
                    case (int)Orientation.Rotate90:
                        return RotateImage(img, 90);
                    case (int)Orientation.Rotate180:
                        return RotateImage(img, 180);
                    case (int)Orientation.Rotate270:
                        return RotateImage(img, 270);
                    default:
                        return img;
                }
            }
        }

        private static Bitmap RotateImage(Bitmap img, int degree)
        {
            Matrix matrix = new Matrix();
            matrix.PostRotate(degree);
            Bitmap rotatedImg = Bitmap.CreateBitmap(img, 0, 0, img.Width, img.Height, matrix, true);
            return rotatedImg;
        }

        public static Drawable ZoomDrawable(Context context, int id, int expectHeightPx)
        {
            return ZoomDrawable(context, BitmapFactory.DecodeResource(context.Resources, id), expectHeightPx);
        }

        public static Drawable ZoomDrawable(Context context, Drawable drawable, int expectHeightPx)
        {
            return ZoomDrawable(context, DrawableToBitmap(drawable), expectHeightPx);
        }

        private static Drawable ZoomDrawable(Context context, Bitmap bitmap, int expectHeightPx)
        {
            int bitmapWidth = bitmap.Width;
            int bitmapHeight = bitmap.Height;
            float scale = expectHeightPx / (float)bitmapHeight;
            Matrix matrix = new Matrix();
            matrix.PostScale(scale, scale);
            Bitmap resizeBitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmapWidth,
                bitmapHeight, matrix, true);
            return new BitmapDrawable(resizeBitmap);
        }

        private static Bitmap DrawableToBitmap(Drawable drawable)
        {
            int width = drawable.IntrinsicWidth;
            int height = drawable.IntrinsicHeight;
            Bitmap.Config config = drawable.Opacity != -1/*PixelFormat.OPAQUE*/ ? Bitmap.Config.Argb8888 : Bitmap.Config.Rgb565;
            Bitmap bitmap = Bitmap.CreateBitmap(width, height, config);
            Canvas canvas = new Canvas(bitmap);
            drawable.SetBounds(0, 0, width, height);
            drawable.Draw(canvas);
            return bitmap;
        }

        public static byte[] Bitmap2Bytes(Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                return stream.ToArray();
            }
        }
    }
}