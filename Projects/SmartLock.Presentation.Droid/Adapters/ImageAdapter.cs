using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Droid.Support;

namespace SmartLock.Presentation.Droid.Adapters
{
    public class ImageAdapter : BaseAdapter
    {
        private readonly Context _context;

        private List<Cache> _items;
        private List<bool> _itemClickBind;
        private Action<Cache> _imageClicked;

        private ImageView _imageView;

        public ImageAdapter(Context context, List<Cache> items, Action<Cache> imageClicked)
        {
            _context = context;

            _items = items;
            _itemClickBind = items.Select(i => false).ToList();
            _imageClicked = imageClicked;
        }

        public override int Count
        {
            get => _items.Count;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                var layoutInflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);

                convertView = layoutInflater.Inflate(Resource.Layout.Item_GridView_Image, null);

                _imageView = convertView.FindViewById<ImageView>(Resource.Id.iv_image);
                _imageView.Click += (s, a) =>
                {
                    if (!string.IsNullOrEmpty(_items[position].NativePath))
                    {
                        _imageClicked?.Invoke(_items[position]);
                    }
                };
            }

            SetImageView(position);

            return convertView;
        }

        private void SetImageView(int position)
        {
            ImageHelper.SetThumbnail(_imageView, _items[position]);
        }
    }
}