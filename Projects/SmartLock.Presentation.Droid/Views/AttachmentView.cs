using Android.App;
using Android.OS;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Support;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar")]
    public class AttachmentView : ViewBase<IAttachmentView>, IAttachmentView
    {
        private ImageView _btnBack;
        private ImageView _ivConent;

        protected override int LayoutId => Resource.Layout.View_Attachment;

        public event Action BackClick;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            _ivConent = FindViewById<ImageView>(Resource.Id.ivConent);

            _btnBack.Click += (s, e) => BackClick?.Invoke();
        }

        public void Show(Cache cache)
        {
            //ImageHelper.SetImageView(_ivConent, cache);
            var bitmap = BitmapHelper.DecodeSampledBitmapFromFile(cache.NativePath);
            _ivConent.SetImageBitmap(bitmap);
        }
    }
}