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
        public static int PxtDp(Context context, float px)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Px, px, context.Resources.DisplayMetrics);
        }

        public static int Dp2Px(Context context, float dp)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
        }
    }
}