using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace SmartLock.Presentation.Droid.Support
{
    public static class DisplayMetricsHelper
    {
        public static double ConvertPixelsToDp(Context context, double px)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            double dp = px / ((double)metrics.DensityDpi / 160f);
            return Math.Round(dp);
        }

        public static double ConvertDpToPixel(Context context, double dp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            var px = dp * ((double)metrics.DensityDpi / 160.0);
            return Math.Round(px);
        }
    }
}