using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Droid.Support;

namespace SmartLock.Presentation.Droid.Adapters
{
    public class ImagePagerAdapter : PagerAdapter
    {
        private readonly Context _context;
        private readonly Action<Cache> _imageClicked;

        public List<Cache> Items;

        public ImagePagerAdapter(Context context, List<Cache> items, Action<Cache> imageClicked)
        {
            _context = context;

            Items = items;

            _imageClicked = imageClicked;
        }

        public override int Count => Items.Count;

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view == @object;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            var layoutInflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            var view = layoutInflater.Inflate(Resource.Layout.Item_ViewPager_Image, container, false);

            var imageView = view.FindViewById<ImageView>(Resource.Id.iv_image);

            imageView.SetImageResource(0);

            GetCachedData(imageView, position);

            container.AddView(view);

            return view;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            container.RemoveView((View)@object);
        }

        private void GetCachedData(ImageView imageView, int position)
        {
            var cache = Items[position];

            ImageHelper.SetImageView(imageView, cache);

            imageView.Click += (s, a) => { _imageClicked?.Invoke(cache); };
        }
    }
}