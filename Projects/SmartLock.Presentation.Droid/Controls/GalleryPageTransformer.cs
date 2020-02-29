using System;
using Android.Support.V4.View;
using Android.Views;

namespace SmartLock.Presentation.Droid.Controls
{
    public class GalleryPageTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        private const float MAX_ALPHA = 0.5f;
        private const float MAX_SCALE = 0.9f;

        public void TransformPage(View page, float position)
        {
            if (position < -1 || position > 1)
            {
                page.Alpha = MAX_ALPHA;
                page.ScaleX = MAX_SCALE;
                page.ScaleY = MAX_SCALE;
            }
            else
            {
                if (position <= 0)
                {
                    page.Alpha = MAX_ALPHA + MAX_ALPHA * (1 + position);
                }
                else
                {
                    page.Alpha = MAX_ALPHA + MAX_ALPHA * (1 - position);
                }

                var scale = Math.Max(MAX_SCALE, 1 - Math.Abs(position));
                page.ScaleX = scale;
                page.ScaleY = scale;
            }
        }
    }
}