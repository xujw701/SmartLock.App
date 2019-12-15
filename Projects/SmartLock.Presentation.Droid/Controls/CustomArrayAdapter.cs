using Android.Content;
using Android.Widget;
using System.Collections;

namespace SmartLock.Presentation.Droid.Controls
{
    public class CustomArrayAdapter : ArrayAdapter
    {
        public CustomArrayAdapter(Context context, int resource, IList objects) : base(context, resource, objects)
        {
        }

        public override int Count
        {
            get
            {
                var count = base.Count;
                return count > 0 ? count - 1 : count;
            }
        }
    }
}