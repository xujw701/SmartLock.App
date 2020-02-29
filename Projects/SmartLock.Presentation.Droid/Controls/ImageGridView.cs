using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SmartLock.Presentation.Droid.Controls
{
    public class ImageGridView : GridView
    {
        public bool Expanded { get; set; } = true;

        public ImageGridView(Context context) : base(context)
        {
        }

        public ImageGridView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public ImageGridView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public ImageGridView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected ImageGridView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (Expanded)
            {
                // Calculate entire height by providing a very large height hint.
                // View.MEASURED_SIZE_MASK represents the largest height possible.
                var expandSpec = MeasureSpec.MakeMeasureSpec(int.MaxValue, MeasureSpecMode.AtMost);
                base.OnMeasure(widthMeasureSpec, expandSpec);
            }
            else
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
        }
    }
}