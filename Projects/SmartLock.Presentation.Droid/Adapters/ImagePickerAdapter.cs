using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Droid.Support;

namespace SmartLock.Presentation.Droid.Adapters
{
    public class ImagePickerAdapter : BaseAdapter
    {
        private readonly Context _context;

        private List<Cache> _items;
        private Action _addClicked;
        private Action<Cache> _imageClicked;
        private Action<Cache> _imageDeleteClicked;

        private ImageView _imageView;
        private TextView _tvDelete;

        public ImagePickerAdapter(Context context, List<Cache> items, Action addClicked, Action<Cache> imageClicked, Action<Cache> imageDeleteClicked)
        {
            _context = context;

            _items = items;
            _addClicked = addClicked;
            _imageClicked = imageClicked;
            _imageDeleteClicked = imageDeleteClicked;
        }

        public override int Count
        {
            get => _items.Count + 1;
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
                _tvDelete = convertView.FindViewById<TextView>(Resource.Id.tvDelete);

                _imageView.Click += (s, a) =>
                {
                    if (position == Count - 1)
                    {
                        _addClicked?.Invoke();
                        return;
                    }

                    if (!string.IsNullOrEmpty(_items[position].NativePath))
                    {
                        _imageClicked?.Invoke(_items[position]);
                    }
                };

                _tvDelete.Click += (s, a) =>
                {
                    if (!string.IsNullOrEmpty(_items[position].NativePath))
                    {
                        _imageDeleteClicked?.Invoke(_items[position]);
                    }
                };
            }

            if (position == Count - 1)
            {
                _imageView.SetImageResource(Resource.Drawable.icon_add_photo);
                _imageView.SetPadding(Dp2Px(28), Dp2Px(28), Dp2Px(28), Dp2Px(28));
                _imageView.SetColorFilter(new Color(_context.GetColor(Resource.Color.attachment_add)));

                _tvDelete.Visibility = ViewStates.Invisible;
            }
            else
            {
                ImageHelper.SetThumbnail(_imageView, _items[position]);

                _tvDelete.Visibility = ViewStates.Visible;
            }

            return convertView;
        }

        private int Dp2Px(int dp)
        {
            return DisplayMetricsHelper.Dp2Px(_context, dp);
        }
    }
}