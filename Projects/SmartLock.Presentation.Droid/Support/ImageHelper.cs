using System.IO;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.Provider;
using Android.Support.V4.Graphics.Drawable;
using Android.Views;
using Android.Widget;
using File = Java.IO.File;
using Uri = Android.Net.Uri;

namespace SmartLock.Presentation.Droid.Support
{
    public static class ImageHelper
    {
        //public static void SetImageView(ImageView imageView, Cache cache, int px = 512)
        //{
        //    SetImageBitmap(imageView, cache, px);
        //}

        //public static void SetThumbnail(ImageView imageView, Cache cache, int px = 256)
        //{
        //    SetImageBitmap(imageView, cache, px);
        //}

        //private static void SetImageBitmap(ImageView imageView, Cache cache, int px)
        //{
        //    if (cache == null) return;
        //    var imageFile = new File(cache.NativePath);

        //    using (var bitmap = BitmapHelper.DecodeSampledBitmapFromFile(imageFile.AbsolutePath, px, px))
        //    {
        //        imageView.SetImageBitmap(bitmap);
        //    }
        //}

        public static void SetDrawableBackgroundColor(View view, Color color)
        {
            if (view.Background is ShapeDrawable shapeDrawable)
            {
                shapeDrawable.Mutate();
                shapeDrawable.Paint.Color = color;
            }
            else if (view.Background is GradientDrawable gradientDrawable)
            {
                gradientDrawable.Mutate();
                gradientDrawable.SetColor(color);
            }
            else if (view.Background is ColorDrawable colorDrawable)
            {
                colorDrawable.Color = color;
            }
        }

        public static Drawable GetTintImageDrawable(Drawable drawable, Color color)
        {
            var drawableCompat = DrawableCompat.Wrap(drawable);
            DrawableCompat.SetTint(drawableCompat, color);
            return drawableCompat;
        }

        public static byte[] CompressAndRotate(Context context, string path, int max = 1280)
        {
            using (var bitmap = BitmapHelper.DecodeSampledBitmapFromFile(path))
            {
                using (var rotatedbitmap = BitmapHelper.RotateImageIfRequired(context, bitmap, Android.Net.Uri.FromFile(new File(path))))
                {
                    using (var resizedBitmap = BitmapHelper.ResizeBitmap(rotatedbitmap, max))
                    {
                        return BitmapHelper.Bitmap2Bytes(resizedBitmap);
                    }
                }
            }
        }
    }
}