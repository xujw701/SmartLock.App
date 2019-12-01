using Android.Views;
using Android.Widget;
using static Android.Views.View;

namespace SmartLock.Presentation.Droid.Support
{
    public static class ListViewHelper
    {
        public static void SetListViewHeightBasedOnChildren(ListView listView)
        {
            var listAdapter = listView.Adapter;
            if (listAdapter == null)
            {
                // pre-condition
                return;
            }

            int totalHeight = 0;
            int desiredWidth = MeasureSpec.MakeMeasureSpec(listView.Width, MeasureSpecMode.AtMost);
            for (int i = 0; i < listAdapter.Count; i++)
            {
                View listItem = listAdapter.GetView(i, null, listView);
                listItem.Measure(desiredWidth, 0);
                totalHeight += listItem.MeasuredHeight;
            }

            ViewGroup.LayoutParams parameters = listView.LayoutParameters;
            parameters.Height = totalHeight + (listView.DividerHeight * (listAdapter.Count - 1));
            listView.LayoutParameters = parameters;
            listView.RequestLayout();
        }
    }
}